SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_add_stats_details_tax'
GO


CREATE PROCEDURE spu_add_stats_details_tax 
    @stats_folder_cnt int
AS

    -- Declare variable for all columns in stats_details table 
    DECLARE 
        -- Working variables
        @is_coinsured_policy smallint,
        @is_ri_at_risk_level int,
        @is_share_with_coinsurer int,
        @is_share_with_reinsurer int,
        @uw_type char(1),
        @company_id int,
        @return_status int,
        -- Working IDs
        @insurance_file_cnt int,
        @source_id int,
        -- RWH (20/03/2001) Account for Coinsurance.
        @retained_coins_percent float,
        -- Tax variables
        @tax_risk_cnt int,
        @tax_band_id int,
        @tax_premium numeric(19,4),
        @tax_percentage numeric(19,6),
        @tax_value numeric(19,4),
        @tax_value_home numeric(19,4),
        @tax_value_system numeric(19,4),
        @tax_is_value int,
        @tax_band_code varchar(10),
        @tax_level varchar(10),
        -- Reinsurance variables
        @ri_shortname varchar (20),
        @ri_party_type char(3),
        @ri_premium_percent float,
        @ri_party_cnt int,
        @ri_is_treaty int,
        -- Output values
        @out_stats_detail_id int,
        @out_stats_detail_type char(3),
        @out_currency_id int,
        @out_currency_code char(10),
        @out_currency_rate numeric(19, 8),
        @out_system_rate numeric(19, 8),
        @out_tax_value numeric(19,4),
        @out_tax_value_home numeric(19,4),
        @out_tax_value_system numeric(19,4),
        @out_tax_recovered numeric(19,4),
        @out_tax_recovered_home numeric(19,4),
        @out_tax_recovered_system numeric(19,4),
        @out_tax_value_retained numeric(19,4),
        @out_tax_value_retained_home numeric(19,4),
        @out_tax_value_retained_system numeric(19,4),
        -- Old variables -- Code that still needs addressing
        @ri_arrangement_line_id int,
		@is_withholding_tax int,
		@multiplier int
        declare @document_ref	varchar(25)
    
    -- Get insurance file cnt
    SELECT  @insurance_file_cnt = insurance_file_cnt,
            @source_id = branch_id
            ,@document_ref = document_ref
    FROM    stats_folder
    WHERE   stats_folder.stats_folder_cnt = @stats_folder_cnt
    
    --Get details from insurance file
    SELECT
    	@company_id = source_id,
    	@out_currency_id = currency_id,
    	@out_currency_rate = currency_base_xrate,
    	@out_system_rate = system_base_xrate
    FROM insurance_file
    WHERE insurance_file_cnt = @insurance_file_cnt
    
    if substring(LTRIM(RTRIM(@document_ref)),0,4) ='SDD'
	begin
	set @out_currency_rate = 0
	end 
    
    --Get details about the currency
    SELECT  @out_currency_code = code
    FROM    currency
    WHERE   currency_id = @out_currency_id
    
    --RAG(11/12/01) - @tax_value should not be flipped for "Agency Mode" (see below)
    --KB  (20/02/02)  UW_TYpe column does not currently exist in some databases - so check first
    --and if not then default the type to underwriting (U)
    --JMK (26/02/2002) additional faffing necessary
    --SJP (13/06/2002) UW_Type selected by branch_id  = 1 and option_number = 1 to ensure unique record
    SELECT  @uw_type = ISNULL(UW_Type, 'U')
    FROM    hidden_options
    WHERE   branch_id = 1
    AND     option_number = 1
  
    
    --RWH Check to see if this policy employs coinsurance.
    SELECT  @is_coinsured_policy = count(*) 
    FROM    insurance_file ifi
    WHERE   ifi.insurance_file_cnt = @insurance_file_cnt
    AND     ifi.business_type_id IN
           (SELECT  business_type_id 
            FROM    business_type 
            WHERE   code like 'COIN LEAD%')
    
    
    --If this is a coinsured policy then retrieve the party_cnt for Retained.
    IF @is_coinsured_policy > 0
    BEGIN
        --Establish percentage retained.
        SELECT  @retained_coins_percent = ISNULL(SUM(cv.share_percent), 0) / 100
        FROM    Coi_Value cv
        JOIN    Party_Insurer pin
            ON  pin.party_cnt = cv.party_cnt
        WHERE   insurance_file_cnt = @insurance_file_cnt
        AND     pin.is_retained = 1
    END
    
    
    --RWH(23/03/2001) Tax stuff. Get all risk level taxes for this policy.
    --RWH(02/11/01) For some reason was only doing risk stuff here. Have now uncommented
    --previous UNION to retrieve risk AND policy taxes.
    DECLARE Taxes_Cursor CURSOR FAST_FORWARD FOR
        SELECT  rt.risk_cnt,
                rt.tax_band_id,
                rt.premium,
                rt.percentage,
                rt.value,
                rt.is_value,
                tb.code,
                'RISK',
				tg.is_withholding_tax
        FROM    tax_band tb
        JOIN    Tax_Calculation rt              ON rt.tax_band_id = tb.tax_band_id
        JOIN    insurance_file_risk_link ifr    ON ifr.risk_cnt = rt.risk_cnt
		JOIN	Tax_Group tg ON rt.tax_group_id=tg.tax_group_id
        WHERE   IFR.insurance_file_cnt = @insurance_file_cnt
        AND     IFR.status_flag NOT IN ('U','R') AND (IFR.original_risk_cnt IS NULL 
                  OR (IFR.original_risk_cnt IS NOT NULL AND ISNULL (IFR.is_risk_edited, 0) = 1))
        AND     RT.transtype = 'TTR'
    
    --RWH(27/03/2001) Write out gross records for all taxes
    OPEN Taxes_Cursor
    FETCH NEXT FROM Taxes_Cursor 
        INTO    @tax_risk_cnt,
                @tax_band_id,
                @tax_premium,
                @tax_percentage,
                @tax_value,
                @tax_is_value,
                @tax_band_code,
                @tax_level,
				@is_withholding_tax
    
    
    WHILE (@@FETCH_STATUS = 0)
    BEGIN
        --RWH(02/11/01) Reverse sign of reinsurer share of tax.
        --RAG - Only do this if not Agency mode (see above)
        IF @uw_type <> 'A'
            SELECT @tax_value = @tax_value * -1
    
    
        --RWH(19/04/2001) Reset tax recovered amount for this tax type.
        --Tomo(15/07/2002) Moved it to before the if statement
        --AUA have risk types that may have both IPT and VAT.  The second line of 0 VAT caused it to go boom
        --as the recovered was not reset
        SELECT  @out_tax_recovered = 0,
                @out_tax_recovered_home = 0
    
        -- RWH(23/08/01) Changed from > to <>.
        IF @tax_value <> 0
        BEGIN
    
			--(RC) PN 31886
           IF @uw_type = 'A'
               BEGIN
                    IF @is_withholding_tax = 0
          	         SELECT @multiplier = 1
                    ELSE
           	         SELECT @multiplier = -1
               END
           ELSE
               BEGIN
                    IF @is_withholding_tax = 0
          		   	 SELECT @multiplier = -1
                    ELSE
           	         SELECT @multiplier = 1
               END
	
    		EXEC spu_ACT_Do_Currency_Conversion
    				@company_id = @company_id,
    				@currency_id = @out_currency_id,
    				@currency_amount_unrounded = @tax_value,
    				@mode = 'ALL',
    				@base_amount = @tax_value_home OUTPUT,
    				@system_amount = @tax_value_system OUTPUT,
    				@currency_base_xrate = @out_currency_rate OUTPUT,
    				@system_base_xrate = @out_system_rate OUTPUT,
    				@return_status = @return_status OUTPUT
    
            -- Check the risk / coinsurer / reinsurer split
            SELECT  @is_ri_at_risk_level = r.is_ri_at_risk_level,
                    @is_share_with_coinsurer = rt.is_share_with_co_insurers,
                    @is_share_with_reinsurer = rt.is_share_with_re_insurers
            FROM    Risk r
            JOIN    Risk_type rt ON rt.risk_type_id = r.risk_type_id
            WHERE   r.risk_cnt = @tax_risk_cnt
    
    
            IF @is_share_with_reinsurer > 0
            BEGIN
                -- RWH(23/08/01) Link to Party table via Treaty_Party.
                DECLARE Reins_Cursor CURSOR FAST_FORWARD FOR
                            -- Peter Finney 10/10/2003
                            -- The percentage is calculated as tax share per peril split by reinsurance line 
                            -- but NOT treaty party, this is done when posting to trans_export!
                    SELECT  t.code,
                            rt.code,
                            (SUM(prl.this_premium) / rtx.premium) * R.premium_percent,
                            T.treaty_id,
                            1
                    FROM    Tax_Calculation rtx
                    JOIN    tax_group_tax_band tgb       ON tgb.tax_band_id = rtx.tax_band_id
                                                        AND tgb.tax_group_id = rtx.tax_group_id
                    JOIN    Peril prl                    ON prl.risk_cnt = rtx.risk_cnt
                                                        AND prl.tax_group = tgb.tax_group_id
                                                        AND prl.class_of_business_id = rtx.class_of_business_id
                    JOIN    Rating_section rs            ON rs.risk_cnt = prl.risk_cnt
                                                        AND rs.rating_section_id = prl.rating_section_id
                                                        AND isnull(rs.country_id, 0) = isnull(rtx.country_id, 0)
                                                        AND isnull(rs.state_id, 0) = isnull(rtx.state_id, 0)
                    JOIN    ri_arrangement rra           ON rra.risk_cnt = rs.risk_cnt
                                                        AND rra.original_flag = rs.original_flag
                                                        AND rra.ri_band_id = prl.ri_band
                    JOIN    RI_Arrangement_Line r        ON r.ri_arrangement_id = rra.ri_arrangement_id
                    JOIN    Treaty t                     ON t.treaty_id = r.treaty_id
                    JOIN    Reinsurance_Type rt          ON rt.reinsurance_type_id = t.reinsurance_type_id
                    WHERE   rtx.risk_cnt = @tax_risk_cnt  
                    AND     rtx.tax_band_id = @tax_band_id
                    AND     rtx.transtype = 'TTR'
                    AND     rtx.premium <> 0
                    AND     r.premium_percent <> 0
                    AND     r.type = 'T'
                    AND     t.code NOT IN ('FAC', 'DUM')
                    GROUP BY 
                            t.code, rt.code, rtx.premium, premium_percent, T.treaty_id

                    UNION

                            -- Peter Finney 10/10/2003
                            -- The percentage is calculated as tax share per peril split by fac share
                    SELECT  p.shortname,
                            rt.code,
                            (SUM(prl.this_premium) / rtx.premium) * r.premium_percent,
                            p.party_cnt,
                            0
                    FROM    Tax_Calculation rtx                 
                    JOIN    tax_group_tax_band tgb       ON tgb.tax_band_id = rtx.tax_band_id
                                                        AND tgb.tax_group_id = rtx.tax_group_id
                    JOIN    Peril prl                    ON prl.risk_cnt = rtx.risk_cnt
                                                        AND prl.tax_group = tgb.tax_group_id
                                                        AND prl.class_of_business_id = rtx.class_of_business_id
                    JOIN    Rating_section rs            ON rs.risk_cnt = prl.risk_cnt
                                                        AND rs.rating_section_id = prl.rating_section_id
                                                        AND isnull(rs.country_id, 0) = isnull(rtx.country_id, 0)
                                                        AND isnull(rs.state_id, 0) = isnull(rtx.state_id, 0)
                    JOIN    ri_arrangement rra           ON rra.risk_cnt = rs.risk_cnt
                                                        AND rra.original_flag = rs.original_flag
                                                        AND rra.ri_band_id = prl.ri_band
                    JOIN    RI_Arrangement_Line r        ON r.ri_arrangement_id = rra.ri_arrangement_id
                    JOIN    Party p                      ON p.party_cnt = r.party_cnt
                    JOIN    Party_Insurer pin            ON pin.party_cnt = p.party_cnt
                    JOIN    Reinsurance_Type rt          ON rt.reinsurance_type_id = pin.reinsurance_type
                    WHERE   rtx.risk_cnt = @tax_risk_cnt
                    AND     rtx.tax_band_id = @tax_band_id
                    AND     rtx.transtype = 'TTR'
                    AND     rtx.premium <> 0
                    GROUP BY
                            p.shortname, rt.code, rtx.premium, r.premium_percent, p.party_cnt

                -- RWH (20/03/2001) Account for Coinsurance.
                IF @is_coinsured_policy > 0
                    --Apply retained % to current values.
                    SELECT  @out_tax_value_retained = @tax_value * @retained_coins_percent,
                            @out_tax_value_retained_home = @tax_value_home * @retained_coins_percent,
    						@out_tax_value_retained_system = @tax_value_system * @retained_coins_percent
                ELSE
                    --RWH(19/04/2001) Set tax if no coinsurance
                    SELECT  @out_tax_value_retained = @tax_value,
                            @out_tax_value_retained_home = @tax_value_home,
    						@out_tax_value_retained_system = @tax_value_system
    
                -- Open the reinsurance cursor
                OPEN Reins_Cursor
                FETCH NEXT FROM Reins_Cursor 
                    INTO    @ri_shortname,
                            @ri_party_type,
                            @ri_premium_percent,
                            @ri_party_cnt,
                            @ri_is_treaty
    
                WHILE (@@FETCH_STATUS = 0)
                BEGIN
                    IF @ri_party_type <> 'RET'
                    BEGIN
                        --Adjust tax for reinsured share
                        SELECT  @out_tax_value = @out_tax_value_retained * @ri_premium_percent / 100,
                                @out_tax_value_home = @out_tax_value_retained_home * @ri_premium_percent / 100,
                                @out_tax_value_system = @out_tax_value_retained_system * @ri_premium_percent / 100
    
                        --RWH(19/04/2001) Keep running total of this tax recovered.
                        SELECT  @out_tax_recovered = @out_tax_recovered + @out_tax_value,
                                @out_tax_recovered_home = @out_tax_recovered_home + @out_tax_value_home,
    							@out_tax_recovered_system = @out_tax_recovered_system + @out_tax_value_system
    
                        -- Set record type for TTY record
                        -- RWH(30/04/01) Change type from TTY to TAT to indicate tax portion in stats.
                        IF @ri_is_treaty = 1
                            SELECT  @out_stats_detail_type = 'TAT'
                        ELSE
                            SELECT  @out_stats_detail_type = 'TAF'
    
                        -- Set stats_detail_id
                        SELECT  @out_stats_detail_id = ISNULL(MAX(stats_detail_id), 0) + 1
                        FROM    Stats_Detail
                        WHERE   stats_folder_cnt = @stats_folder_cnt
    
                        -- Insert the Stats Detail
                        INSERT INTO Stats_Detail 
    					(
    						stats_folder_cnt,
    						stats_detail_id,
    						stats_detail_type,
    						risk_id,
    						tax_type_id,
    						tax_type_code,
    						tax_value,
    						ri_party_cnt,
    						ri_shortname,
    						ri_party_type,
    						ri_share_percent,
    						this_premium_original,      --  TN 14 June 2001
    						this_premium_home,
    						this_premium_system,
    						currency_code,
    						currency_rate
    					)
                        VALUES
    					(
    						@stats_folder_cnt,
    						@out_stats_detail_id,
    						@out_stats_detail_type,
    						@tax_risk_cnt,
    						@tax_band_id,
    						@tax_band_code,
    						-@out_tax_value*@multiplier,
    						@ri_party_cnt,
    						@ri_shortname,
    						@ri_party_type,
    						@ri_premium_percent,
    						-@out_tax_value*@multiplier,     
    						-@out_tax_value_home*@multiplier,
    						-@out_tax_value_system*@multiplier,
    						@out_currency_code,
    						@out_currency_rate
    					)
    
                    END--IF @ri_party_type <> 'RET'
    
                    FETCH NEXT FROM Reins_Cursor 
                        INTO    @ri_shortname,
                                @ri_party_type,
                                @ri_premium_percent,
                                @ri_party_cnt,
                                @ri_is_treaty
                END -- (@@FETCH_STATUS = 0)
    
                CLOSE Reins_Cursor
                DEALLOCATE Reins_Cursor
            END --IF @is_share_with_reinsurer > 0
        END -- IF @tax_value >0
    
    
        --RWH(19/04/2001) Write record for total amount recovered of this tax type.
        IF @out_tax_recovered <> 0
        BEGIN
            -- RWH(27/03/2001) Get retained tax account name. Transfer to Orion code
            -- will automatically create this on Orion if it does not already exist.
            -- RWH(30/04/01) New Nominal Ledger account.
            SELECT  @ri_shortname = 'NOTARI' + RTRIM(@tax_band_code),
                    @ri_premium_percent = 100.0
    
            -- Set stats_detail_id
            SELECT  @out_stats_detail_id = ISNULL(MAX(stats_detail_id), 0) + 1,
                    @out_stats_detail_type = 'TAN'
            FROM    Stats_Detail
            WHERE   stats_folder_cnt = @stats_folder_cnt
    
            -- Insert the Stats Detail
            INSERT INTO Stats_Detail 
    		(
    			stats_folder_cnt,
    			stats_detail_id,
    			stats_detail_type,
    			risk_id,
    			tax_type_id,
    			tax_type_code,
    			tax_value,
    			ri_shortname,
    			ri_share_percent,
    			this_premium_original,      --  TN 14 June 2001
    			this_premium_home,
    			this_premium_system,
    			currency_code,
    			currency_rate
    		)
            VALUES
    		(
    			@stats_folder_cnt,
    			@out_stats_detail_id,
    			@out_stats_detail_type,
    			@tax_risk_cnt,
    			@tax_band_id,
    			@tax_band_code,
    			@out_tax_recovered*@multiplier,
    			@ri_shortname,
    			@ri_premium_percent,
    			@out_tax_recovered*@multiplier,             -- RWH(11/07/01)
    			@out_tax_recovered_home*@multiplier,             -- RWH(11/07/01)
    			@out_tax_recovered_system*@multiplier,
    			@out_currency_code,
    			@out_currency_rate
    		)
    
        END-- IF @out_tax_recovered <> 0
    
        FETCH NEXT FROM Taxes_Cursor 
            INTO    @tax_risk_cnt,
                    @tax_band_id,
                    @tax_premium,
                    @tax_percentage,
                    @tax_value,
                    @tax_is_value,
                    @tax_band_code,
                    @tax_level,
					@is_withholding_tax
    END
    
    CLOSE Taxes_Cursor
    DEALLOCATE Taxes_Cursor
    

GO


