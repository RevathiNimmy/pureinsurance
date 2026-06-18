SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


Execute DDLDropProcedure 'spu_add_stats_details_treaty'
GO


CREATE PROCEDURE spu_add_stats_details_treaty    
    @stats_folder_cnt int    
AS    
        
    --**********************************************************************************    
    -- sp_add_stats_details_treaty creates stats details records for    
    -- Treaty reinsurance by product/class/peril/treaty.    
    --    
    -- 1 parameter is passed in - @stats_folder_cnt    
    --    
    -- This stored procedure is called by sp_add_stats_details_control.    
    --    
    -- A failure in this procedure will be passed back to the calling procedure.    
    --**********************************************************************************    
    -- Revision     Description of Modification                             Date        Who    
    -- --------     ---------------------------                             ----        ---    
    -- 1.0          Original                                                25/06/1997  TF    
    -- 1.5          Database model adjusted    
    --              source_id removed from primary keys                     15/07/1997  TF    
    -- 1.6          Account for coinsurance.                                20/03/2001  RWH    
    -- 1.7          Get commission from Treaty_Party table rather    
    --              than Treaty.                                            19/05/2001  RWH    
    -- 1.8          Link to Lead_Commission on risk_type_id as agents may    
    --              have different rates for different risk types.          11/06/2001  RWH    
    -- 1.9          use this_premium instead of anual premium for this_premium_original    
    -- 2.0          Don't apply coinsurance to annual_premium.              11/07/2001  RWH    
    -- 2.1          Check this_premium instead of annual_premium when retrieving    
    --              peril info eliminating only those that are 0.           08/08/2001  RWH    
    -- 2.2          Removed check on @peril_share_with_coinsurer flag as this    
    --              should only apply to tax.                               20/08/2001  RWH    
    -- 2.3          Insert code reinsurance_type now linked to Treaty table    
    --              in ri_party_type. Also put Treaty.code instead of    
    --              description into re_shortname.                          05/09/2001  RWH    
    -- 2.4          Multi-currency & rounding.                              23/10/2001  TOT    
    -- 2.5          Retrieve reins commission % from ri_arrangement_line    
    --              rather than Treaty_Party to ensure manual changes made    
    --              to value during creation of policy are picked up.       02/11/2001  RWH    
    -- 2.6          Also apply coins to commission.                         05/11/2001  RWH    
    -- 2.7          Also process deleted risks.                             15/02/2002  Tom    
    -- 2.8          include items where is_levy_tax = true                  19/07/2002  Thinh    
    --              and write them out as tax    
    -- 2.9          Exclude Retained Treaties                               19/05/2003  JMK    
    --              i.e. any treaty code starting with "RET"    
    -- 1.8.6 SR17   Recoded to be more legible and to store correct values! 01/09/2003  PWF    
    --**********************************************************************************    
        
    -- Declare variable for all columns in stats_details table    
    DECLARE    
        -- Working variables    
        @is_coinsured_policy smallint,    
        @is_ri_at_risk_level int,    
        -- Working IDs    
        @insurance_file_cnt int,    
        @ri_arrangement_id int,    
        @orig_ri_arrangement_id int,    
        @source_id int,    
        -- RWH (20/06/2001) Account for Coinsurance.    
        @retained_coins_percent float,    
        -- Peril variables    
        @peril_risk_id int,    
        @peril_risk_type_id int,    
        @peril_risk_type_code char(10),    
        @peril_id int,    
        @peril_description varchar(30),    
        @peril_type_id int,    
        @peril_type_code char(10),    
        @peril_policy_section_type_id int,    
        @peril_policy_section_type_code char(10),    
        @peril_class_of_business_id int,    
        @peril_class_of_business_code char(10),    
        @peril_annual_premium numeric(19, 4),    
        @peril_this_premium_original numeric(19, 4),    
        @peril_lead_commission_value numeric(19, 4),    
        @peril_sub_commission_value numeric(19, 4),    
        @peril_this_sum_insured money,    
        @peril_rating_section_id int,    
        @peril_ri_band int,    
        @peril_share_with_coinsurer int,    
        @peril_is_levy_tax int,            -- Accounting for levies    
        @peril_original_flag int,    
        -- treaty variables    
        @treaty_ri_share_percent float,    
        @treaty_ri_party_cnt int,    
        @treaty_ri_shortname varchar(20),    
        @treaty_premium_percent float,    
        @treaty_commission_percent float,    
        @treaty_sum_insured money,    
        @treaty_premium_value money,    
        @treaty_commission_value money,    
        @treaty_ri_party_type char(3),    
        @treaty_original_sum_insured money,    
        @treaty_is_commission_modified int,
        -- Output values    
        @out_stats_detail_id int,    
        @out_stats_detail_type char(3),    
        @out_ri_agreement_code varchar(20),    
        @out_currency_code char(10),    
        @out_currency_id int,    
        @out_currency_rate numeric(19, 8),    
        @out_system_rate numeric(19, 8),    
        @out_this_premium_home numeric(19, 4),    
        @out_this_premium_system numeric(19, 4),    
        @out_sum_insured_home numeric(19, 4),    
        @out_sum_insured_system numeric(19, 4),    
        @out_sum_insured_change numeric(19, 4),    
     	@out_sum_insured_change_home numeric(19, 4),    
        @out_lead_commission_value_home numeric(19, 4),    
        @out_lead_commission_value_system numeric(19, 4),    
        @out_sub_commission_value_home numeric(19, 4),    
        @out_sub_commission_value_system numeric(19, 4),
		--Start (Arul Stephen)-(Tech Spec - HGPS001 Reinsurance Modifications)        		
		@product_id int,
		@ri_manual_premium_adjustment int
		--End (Arul Stephen)-(Tech Spec - HGPS001 Reinsurance Modifications)

    DECLARE @company_id INT    
    DECLARE @return_status INT    
    
	--Start (Prakash Varghese) - (Tech Spec - TRAC 4761 Stats Detail to RI Line.docx) - (6.2)  
	-- Adding variable to get RI_Arrangement_Line_ID
	DECLARE @RI_Arrangement_Line_ID INT
	--End (Prakash Varghese) - (Tech Spec - TRAC 4761 Stats Detail to RI Line.docx) - (6.2)
    
    -- Get insurance file cnt    
    SELECT  @insurance_file_cnt = insurance_file_cnt,    
            @source_id = branch_id    
    FROM    stats_folder    
    WHERE   stats_folder.stats_folder_cnt = @stats_folder_cnt    
        
    /*Get details from insurance file*/    
    SELECT    
     @company_id = source_id,    
     @out_currency_id = currency_id,    
     @out_currency_rate = currency_base_xrate,    
     @out_system_rate = system_base_xrate,
	 --(Arul Stephen)-(Tech Spec - HGPS001 Reinsurance Modifications)
     @product_id=product_id        
    FROM insurance_file    
    WHERE insurance_file_cnt = @insurance_file_cnt    

   --Start(Arul Stephen)-(Tech Spec - HGPS001 Reinsurance Modifications)
	SELECT  
		@ri_manual_premium_adjustment=ri_manual_premium_adjustment
    FROM Product  
    WHERE Product_id = @product_id  
    --End(Arul Stephen)-(Tech Spec - HGPS001 Reinsurance Modifications)
        
    /*Get details about the currency*/    
    SELECT    
     @out_currency_code = code    
    FROM currency    
    WHERE currency_id = @out_currency_id    
        
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
        
    -- Declare peril cursor    
    DECLARE c_peril CURSOR FAST_FORWARD FOR    
        SELECT  P.risk_cnt,    
                R.risk_type_id,    
                RT.code,    
                P.peril_id,    
                P.description,    
                P.peril_type_id,    
                PT.code,    
                RS.policy_section_type_id,    
                PS.code,    
                P.class_of_business_id,    
                CB.code,    
                P.annual_premium,    
                P.this_premium,    
                P.lead_commission_value,    
                P.sub_commission_value,    
                P.sum_insured,    
                P.rating_section_id,    
                P.ri_band,                     -- RWH (19/06/01) Need ri_band to get ri_arrangement_line later.    
                RT.is_share_with_co_insurers,  -- RWH (20/06/01)    
                IsNull(P.is_levy_tax,0),       -- Thinh Nguyen (19/07/2002)    
                RS.original_flag    
        FROM    Insurance_File_Risk_Link IFR    
        JOIN    Peril P                        ON IFR.risk_cnt = P.risk_cnt    
        JOIN    Peril_Type PT                  ON P.peril_type_id = PT.peril_type_id    
        JOIN    Rating_Section RS              ON P.rating_section_id = RS.rating_section_id    
                                              AND P.Risk_cnt = RS.Risk_cnt    
        JOIN    Class_Of_Business CB           ON P.class_of_business_id = CB.class_of_business_id    
        JOIN    Risk R                         ON P.risk_cnt = R.risk_cnt    
        JOIN    Risk_Type RT                   ON R.risk_type_id = RT.risk_type_id    
        LEFT JOIN    
                Policy_Section_Type PS         ON RS.policy_section_type_id = PS.policy_section_type_id    
        WHERE   IFR.insurance_file_cnt = @insurance_file_cnt    
        AND     IFR.status_flag <> 'U' AND (IFR.original_risk_cnt IS NULL 
                OR (IFR.original_risk_cnt IS NOT NULL AND ISNULL (IFR.is_risk_edited, 0) = 1)
                OR (IFR.status_flag IN ('C','D') AND IFR.is_manually_changed IS NOT NULL))  
        AND    (P.is_premium = 1               -- Only select perils which are 'FAP' or 'SI'    
             OR P.is_sum_insured = 1    
             OR IsNull(P.is_levy_tax, 0) = 1)  -- Thinh Nguyen (19/07/2002) also pick up levy tax    
        AND
                ISNULL(P.this_premium, 0) != 0 --Only get details for non-zero premiums
        ORDER BY    
                P.rating_section_id ASC    
        
    -- Open the Peril Cursor    
    OPEN c_peril    
    FETCH NEXT FROM c_peril    
        INTO    @peril_risk_id,    
                @peril_risk_type_id,    
                @peril_risk_type_code,    
                @peril_id,    
                @peril_description,    
                @peril_type_id,    
                @peril_type_code,    
                @peril_policy_section_type_id,    
                @peril_policy_section_type_code,    
                @peril_class_of_business_id,    
                @peril_class_of_business_code,    
                @peril_annual_premium,    
                @peril_this_premium_original,    
                @peril_lead_commission_value,    
                @peril_sub_commission_value,    
                @peril_this_sum_insured,    
                @peril_rating_section_id,    
                @peril_ri_band,    
                @peril_share_with_coinsurer,    
                @peril_is_levy_tax,    --Thinh Nguyen (19/07/2002)    
                @peril_original_flag    
        
    -- Get the column values    
    WHILE (@@FETCH_STATUS = 0)    
    BEGIN    
        -- Get the ri arrangement for this peril and ri_band    
        SELECT  @ri_arrangement_id = ri_arrangement_id    
        FROM    ri_arrangement    
        WHERE   risk_cnt = @peril_risk_id    
        AND     (ri_band_id = @peril_ri_band  or ri_band_id IS NULL)  
        AND     original_flag = @peril_original_flag    
    
        -- ISS4752 Peter Finney - Get paired ri_arrangement id    
        SELECT  @orig_ri_arrangement_id = ri_arrangement_id    
        FROM    ri_arrangement    
        WHERE   risk_cnt = @peril_risk_id    
        AND     (ri_band_id = @peril_ri_band  or ri_band_id IS NULL)  
        AND     original_flag <> @peril_original_flag    
    
        -- RWH (05/09/01) Retrieve code from reinsurance_type newly linked to Treaty.    
        -- This is inserted into ri_party_type.    
        -- Get code from Treaty instead of description to insert into ri_shortname.    
        -- Peter Finney 22/08/2003 - Use the real fields, they now have the required precision    
        DECLARE TTY_Cursor CURSOR FAST_FORWARD FOR    
            SELECT  R.this_share_percent,    
                    T.treaty_id,    
                    T.code,    
                    R.premium_percent,    
                    R.commission_percent,    
                    R.sum_insured,    
                    R.premium_value,    
                    R.commission_value,    
                    RT.code,    
                   (SELECT Sum(R2.sum_insured)    
                    FROM   RI_Arrangement_Line R2    
                    WHERE  R2.treaty_id = R.treaty_id    
                    AND    R2.ri_arrangement_id = @orig_ri_arrangement_id),
                    R.is_commission_modified,
					--Start (Prakash Varghese) - (Tech Spec - TRAC 4761 Stats Detail to RI Line.docx) - (6.2)
					R.ri_arrangement_line_id
					--End (Prakash Varghese) - (Tech Spec - TRAC 4761 Stats Detail to RI Line.docx) - (6.2)
            FROM    RI_Arrangement_Line R    
            JOIN    Treaty T                ON T.treaty_id = R.treaty_id    
            JOIN    Reinsurance_Type RT     ON RT.reinsurance_type_id = T.reinsurance_type_id    
            WHERE   R.ri_arrangement_id = @ri_arrangement_id    
            AND     R.type IN ('T','TFS')
    
        
        -- Open the cursor    
        OPEN TTY_Cursor    
        FETCH NEXT FROM TTY_Cursor    
            INTO    @treaty_ri_share_percent,    
                    @treaty_ri_party_cnt,    
                    @treaty_ri_shortname,    
                    @treaty_premium_percent,    
                    @treaty_commission_percent,    
                    @treaty_sum_insured,    
                    @treaty_premium_value,    
                    @treaty_commission_value,    
                    @treaty_ri_party_type,    
                    @treaty_original_sum_insured,
                    @treaty_is_commission_modified,
					--Start (Prakash Varghese) - (Tech Spec - TRAC 4761 Stats Detail to RI Line.docx) - (6.2)
					@RI_Arrangement_Line_ID
					--End (Prakash Varghese) - (Tech Spec - TRAC 4761 Stats Detail to RI Line.docx) - (6.2)
        
        WHILE (@@FETCH_STATUS = 0)    
        BEGIN    
            -- 05/08/2003 Peter Finney - Would be lovely if we could use the stored premium value    
            --    but unfortunately we can't, it may be split over multiple perils and we need to    
            --    allow for this...(Note: Rounding may also split this!!!)    
			IF (@peril_this_premium_original<>0)
				 --Start-(Arul Stephen)-(Tech Spec - HGPS001 Reinsurance Modifications)
				BEGIN
					IF @ri_manual_premium_adjustment<>1 				 														
						SELECT  @treaty_premium_value = @peril_this_premium_original * @treaty_premium_percent / 100
					END
			else
			    SET @treaty_premium_value=0

            SELECT @treaty_sum_insured = @peril_this_sum_insured * @treaty_premium_percent / 100  
        
            -- Account for Coinsurance.    
            IF (@is_coinsured_policy > 0) --AND (@peril_share_with_coinsurer = 1)    
                SELECT  @treaty_premium_value = @treaty_premium_value * @retained_coins_percent,    
                        @treaty_sum_insured = @treaty_sum_insured * @retained_coins_percent    

	    --If there is no change in gross premium for a risk then post zero comm 
	    --@out_sum_insured_change = 0 wouldn't help because there could be a change in Treaty party
	    --specific to MTAs only
	    /*If (
		(Select Sum(Round(premium, 2)) From ri_arrangement Where risk_cnt = @peril_risk_id Group By risk_cnt) = 0
		AND
		EXISTS (Select NULL From insurance_file ifi
		    INNER JOIN insurance_file_type ift ON ifi.insurance_file_type_id = ift.insurance_file_type_id
			Where insurance_file_cnt = @insurance_file_cnt AND ift.code like 'MTA%') 
		)
		Begin
		    SELECT @treaty_commission_value = 0
		End
	    Else
		Begin*/
		    -- calculate lead_commission from adjusted premium    
		    SELECT @treaty_commission_value = @treaty_premium_value * @treaty_commission_percent / 100    
		--End

      		-- Calculate the sum_insured_change figure    
            SELECT @out_sum_insured_change = @treaty_sum_insured + ISNULL(@treaty_original_sum_insured, 0)    
        
            -- Calculate home premiums    
    		EXEC spu_ACT_Do_Currency_Conversion    
                @company_id = @company_id,    
                @currency_id = @out_currency_id,    
                @currency_amount_unrounded = @treaty_premium_value,    
                @mode = 'ALL',    
                @base_amount = @out_this_premium_home OUTPUT,    
                @system_amount = @out_this_premium_system OUTPUT,    
                @currency_base_xrate = @out_currency_rate OUTPUT,    
                @system_base_xrate = @out_system_rate OUTPUT,    
                @return_status = @return_status OUTPUT    
    		
    		EXEC spu_ACT_Do_Currency_Conversion    
                @company_id = @company_id,    
                @currency_id = @out_currency_id,    
                @currency_amount_unrounded = @treaty_sum_insured,    
                @mode = 'ALL',    
                @base_amount = @out_sum_insured_home OUTPUT,    
                @system_amount = @out_sum_insured_system OUTPUT,    
                @currency_base_xrate = @out_currency_rate OUTPUT,    
                @system_base_xrate = @out_system_rate OUTPUT,    
                @return_status = @return_status OUTPUT    
    
    		EXEC spu_ACT_Do_Currency_Conversion    
                @company_id = @company_id,    
                @currency_id = @out_currency_id,    
                @currency_amount_unrounded = @treaty_commission_value,    
                @mode = 'ALL',    
                @base_amount_unrounded = @out_lead_commission_value_home OUTPUT,    
                @system_amount_unrounded = @out_lead_commission_value_system OUTPUT,    
                @currency_base_xrate = @out_currency_rate OUTPUT,    
                @system_base_xrate = @out_system_rate OUTPUT,    
                @return_status = @return_status OUTPUT    
    		
    		EXEC spu_ACT_Do_Currency_Conversion    
                @company_id = @company_id,    
                @currency_id = @out_currency_id,    
                @currency_amount_unrounded = @peril_sub_commission_value,    
                @mode = 'ALL',    
                @base_amount = @out_sub_commission_value_home OUTPUT,    
                @system_amount = @out_sub_commission_value_system OUTPUT,    
                @currency_base_xrate = @out_currency_rate OUTPUT,    
                @system_base_xrate = @out_system_rate OUTPUT,    
                @return_status = @return_status OUTPUT    
    		
    		EXEC spu_ACT_Do_Currency_Conversion    
                @company_id = @company_id,    
                @currency_id = @out_currency_id,    
                @currency_amount_unrounded = @out_sum_insured_change,    
                @mode = 'BASE',    
                @base_amount = @out_sum_insured_change_home OUTPUT,    
                @currency_base_xrate = @out_currency_rate OUTPUT,    
                @system_base_xrate = @out_system_rate OUTPUT,    
                @return_status = @return_status OUTPUT    
        
            -- Check we have something to store!??    
            IF (@treaty_premium_value <> 0) OR (@treaty_sum_insured <> 0) OR (@treaty_commission_value <> 0)    
            BEGIN    
                -- Thinh Nguyen (19/07/2002) start - write out as tax    
                IF @peril_is_levy_tax <> 1    
                /*
                BEGIN    
                    -- Get next stats_detail_id and set type    
                    SELECT  @out_stats_detail_id = ISNULL(MAX(stats_detail_id), 0) + 1,    
                            @out_stats_detail_type = 'TAT'    
                    FROM    Stats_Detail    
                    WHERE   stats_folder_cnt = @stats_folder_cnt    
        
                    -- Insert the Stats Detail    
                    INSERT INTO Stats_Detail    
    			    (    
    			     stats_folder_cnt,    
    			     stats_detail_id,    
    			     stats_detail_type,    
    			     risk_id,    
    			     risk_type_id,    
    			     risk_type_code,    
    			     ri_shortname,    
    			     ri_share_percent,    
    			     this_premium_original,    
    			     this_premium_home,    
    			     this_premium_system,    
    			     currency_rate,    
    			     currency_code,    
    			     tax_type_code,    
    			     tax_value    
    			    )    
                    VALUES    
    			    (    
    			     @stats_folder_cnt,    
    			     @out_stats_detail_id,    
    			     @out_stats_detail_type,    
    			     @peril_risk_id,    
    			     @peril_risk_type_id,    
    			     @peril_risk_type_code,    
    			     NULL,    
    			     @treaty_ri_share_percent,    
    			     @treaty_premium_value,    
    			     @out_this_premium_home,    
    			     @out_this_premium_system,    
    			     @out_currency_rate,    
    			     @out_currency_code,    
    			     RTRIM(@peril_class_of_business_code),    
    			     @treaty_premium_value    
    			    )    
                        -- Get next stats_detail_id and set type    
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
    			     risk_type_id,    
    			     risk_type_code,    
    			     peril_id,    
    			     peril_description,    
    			     peril_type_id,    
    			     peril_type_code,    
    			     policy_section_type_id,    
    			     policy_section_type_code,    
    			     class_of_business_id,    
    			     class_of_business_code,    
    			     tax_type_code,    
    			     tax_value,    
    			     ri_party_cnt,    
    			     ri_shortname,    
    			     ri_party_type,    
    			     ri_share_percent,    
    			     ri_agreement_code,    
    			     this_premium_original,    
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
    			     @peril_risk_id,    
    			     @peril_risk_type_id,    
    			     @peril_risk_type_code,    
    			     @peril_id,    
    			     @peril_description,    
    			     @peril_type_id,    
    			     @peril_type_code,    
    			     @peril_policy_section_type_id,    
    			     @peril_policy_section_type_code,    
    			     @peril_class_of_business_id,    
    			     @peril_class_of_business_code,    
    			     RTRIM(@peril_class_of_business_code),    
    			     -@treaty_premium_value,    
    			     @treaty_ri_party_cnt,    
    			     'NOTARI' + RTRIM(@peril_class_of_business_code),    
    			     @treaty_ri_party_type,    
    			     @treaty_ri_share_percent,    
    			     @out_ri_agreement_code,    
    			     -@treaty_premium_value,    
    			     -@out_this_premium_home,    
    			     -@out_this_premium_system,    
    			     @out_currency_code,    
    			     @out_currency_rate    
    			    )    
                END    
                ELSE    */
                BEGIN    
                    -- Get next stats_detail_id and set type    
                    SELECT  @out_stats_detail_id = ISNULL(MAX(stats_detail_id), 0) + 1,    
                            @out_stats_detail_type = 'TTY'    
                    FROM    Stats_Detail    
                    WHERE   stats_folder_cnt = @stats_folder_cnt    
        
                    -- Insert the Stats Detail    
                    INSERT INTO Stats_Detail ( 
                            stats_folder_cnt,    
                            stats_detail_id,    
                            stats_detail_type,    
                            risk_id,    
                            risk_type_id,    
                            risk_type_code,    
                            peril_id,    
                            peril_description,    
                            peril_type_id,    
                            peril_type_code,    
                            policy_section_type_id,    
                            policy_section_type_code,    
                            class_of_business_id,    
                            class_of_business_code,    
                            ri_party_cnt,    
                            ri_shortname,    
                            ri_party_type,    
                            ri_share_percent,    
                            ri_agreement_code,    
                            annual_premium,    
                            currency_code,    
                            currency_rate,    
                            this_premium_original,    
                            this_premium_home,    
                            this_premium_system,    
                            commission_percent,    
                            lead_commission_value_home,    
                            lead_commission_value_system,    
                            sub_commission_value_home,    
                            sub_commission_value_system,    
                            sum_insured_home,    
                            sum_insured_system,    
                            sum_insured_currency_code,    
                            sum_insured_change,
                            is_commission_modified,
							--Start (Prakash Varghese) - (Tech Spec - TRAC 4761 Stats Detail to RI Line.docx) - (6.2)
							ri_arrangement_line_id
							--End (Prakash Varghese) - (Tech Spec - TRAC 4761 Stats Detail to RI Line.docx) - (6.2)
							)    
                    VALUES (@stats_folder_cnt,    
                            @out_stats_detail_id,    
                            @out_stats_detail_type,    
                            @peril_risk_id,    
                            @peril_risk_type_id,    
                            @peril_risk_type_code,    
                            @peril_id,    
                            @peril_description,    
                            @peril_type_id,    
                            @peril_type_code,    
                            @peril_policy_section_type_id,    
                            @peril_policy_section_type_code,    
                            @peril_class_of_business_id,    
                            @peril_class_of_business_code,    
                            @treaty_ri_party_cnt,    
                            @treaty_ri_shortname,    
                            @treaty_ri_party_type,    
                            @treaty_ri_share_percent,    
                            @out_ri_agreement_code,    
                            -@peril_annual_premium,    
                            @out_currency_code,    
                            @out_currency_rate,    
                            -@treaty_premium_value,    
                            -@out_this_premium_home,    
                            -@out_this_premium_system,    
                            @treaty_commission_percent,    
                            -@out_lead_commission_value_home,    
                            -@out_lead_commission_value_system,    
                            -@out_sub_commission_value_home,    
                            -@out_sub_commission_value_system,    
                            -@out_sum_insured_home,    
                            -@out_sum_insured_system,    
                            @out_currency_code,    
                            -@out_sum_insured_change_home,
                            @treaty_is_commission_modified,
							--Start (Prakash Varghese) - (Tech Spec - TRAC 4761 Stats Detail to RI Line.docx) - (6.2)
							@RI_Arrangement_Line_ID
							--End (Prakash Varghese) - (Tech Spec - TRAC 4761 Stats Detail to RI Line.docx) - (6.2)
							)      
                END    
            END    
        
            FETCH NEXT FROM TTY_Cursor    
                INTO    @treaty_ri_share_percent,    
                        @treaty_ri_party_cnt,    
                        @treaty_ri_shortname,    
                        @treaty_premium_percent,    
                        @treaty_commission_percent,    
                        @treaty_sum_insured,    
                        @treaty_premium_value,    
                        @treaty_commission_value,    
                        @treaty_ri_party_type,    
                        @treaty_original_sum_insured,
                        @treaty_is_commission_modified,
						--Start (Prakash Varghese) - (Tech Spec - TRAC 4761 Stats Detail to RI Line.docx) - (6.2)  
						@RI_Arrangement_Line_ID
						--End (Prakash Varghese) - (Tech Spec - TRAC 4761 Stats Detail to RI Line.docx) - (6.2)
        END    
        
        CLOSE TTY_Cursor    
        DEALLOCATE TTY_Cursor    
        
        -- Fetch Next    
        FETCH NEXT FROM c_peril    
            INTO    @peril_risk_id,
                    @peril_risk_type_id,    
                    @peril_risk_type_code,    
                    @peril_id,    
                    @peril_description,    
                    @peril_type_id,    
                    @peril_type_code,    
                    @peril_policy_section_type_id,    
                    @peril_policy_section_type_code,    
                    @peril_class_of_business_id,    
                    @peril_class_of_business_code,    
                    @peril_annual_premium,    
                    @peril_this_premium_original,    
                    @peril_lead_commission_value,    
                    @peril_sub_commission_value,    
                    @peril_this_sum_insured,    
                    @peril_rating_section_id,    
                    @peril_ri_band,    
                    @peril_share_with_coinsurer,    
                    @peril_is_levy_tax,    --Thinh Nguyen (19/07/2002)    
                    @peril_original_flag    
        
    END    
        
    -- Closedown    
    CLOSE c_peril    
    DEALLOCATE c_peril    
        
    -- ********************************************************************    
    --            TAX RECORDS FOR TREATY PREMIUM AND COMMISSION    
    -- ********************************************************************    
        
    DECLARE @risk_cnt INT,    
    		@risk_type_id INT,    
    		@risk_type_code VARCHAR(20),    
    		@out_ri_shortname VARCHAR(20)    
        
    DECLARE	@tax_band_id INT,    
       		@tax_band_code VARCHAR(20),    
    		@tax_premium NUMERIC(19,4),    
    		@tax_percentage FLOAT,    
    		@tax_is_value TINYINT,    
    		@tax_value NUMERIC(19,4),    
    		@out_tax_value_home NUMERIC(19,4),    
    		@out_tax_value_system NUMERIC(19,4),    
    		@transtype VARCHAR(10),   
    		@ri_party_cnt int,
            @is_withholding_tax int,
            @multiplier int
        
    /*Declare Tax Cursor for Treaty Tax on Premium and Commission */    
    DECLARE Taxes_Cursor CURSOR FAST_FORWARD FOR    
        SELECT  tc.tax_band_id,    
                tc.premium,    
                tc.percentage,    
                tc.value,    
                tc.is_value,    
                tb.code,    
       			tc.risk_cnt,    
                R.risk_type_id,    
                RT.code,    
                TC.transtype,   
                tc.ri_party_cnt,
                TG.is_withholding_tax,
				--Start (Prakash Varghese) - (Tech Spec - TRAC 4761 Stats Detail to RI Line.docx) - (6.2)
				RAL.ri_arrangement_line_id
				--End (Prakash Varghese) - (Tech Spec - TRAC 4761 Stats Detail to RI Line.docx) - (6.2)
        FROM    Tax_Calculation TC    
        JOIN    tax_band TB                 ON tb.tax_band_id = tc.tax_band_id    
        JOIN    Tax_Type TT                 ON tt.tax_type_id = tb.tax_type_id   
        JOIN    Tax_Group TG                ON tg.tax_group_id = tc.tax_group_id 
        JOIN    Risk R                      ON R.risk_cnt = TC.risk_cnt
        JOIN    Risk_Type RT                ON RT.risk_type_id = R.risk_type_id    
        JOIN    Insurance_File_Risk_Link RL ON RL.risk_cnt = TC.risk_cnt
                                           AND RL.insurance_file_cnt = TC.insurance_file_cnt
        LEFT JOIN    ri_arrangement_line RAL 
                                            ON RAL.ri_arrangement_line_id= TC.ri_arrangement_line_ID                                         
        WHERE   TC.insurance_file_cnt = @insurance_file_cnt    
        AND     TC.transtype IN ('TTRITP','TTRITC')    
        AND     R.is_risk_selected = 1  
        AND     RL.status_flag <> 'U' AND (RL.original_risk_cnt IS NULL 
        OR (RL.original_risk_cnt IS NOT NULL AND ISNULL (RL.is_risk_edited, 0) = 1))
        AND     RAL.Type NOT IN ('TX','FX')        
        
    OPEN Taxes_Cursor    
    FETCH NEXT FROM Taxes_Cursor    
        INTO    @tax_band_id,    
                @tax_premium,    
                @tax_percentage,    
                @tax_value,    
                @tax_is_value,    
                @tax_band_code,    
                @risk_cnt,    
                @risk_type_id,    
                @risk_type_code,    
                @transtype,   
                @ri_party_cnt,
                @is_withholding_tax,
				--Start (Prakash Varghese) - (Tech Spec - TRAC 4761 Stats Detail to RI Line.docx) - (6.2)
				@RI_Arrangement_Line_ID
				--End (Prakash Varghese) - (Tech Spec - TRAC 4761 Stats Detail to RI Line.docx) - (6.2)
        
    WHILE (@@FETCH_STATUS = 0)    
    BEGIN    
        IF @tax_value <> 0    
        BEGIN    
    		IF @is_withholding_tax=0
    			SELECT @multiplier=1
    		ELSE
    			SELECT @multiplier=-1
    
            -- Set record type for TAX record    
            IF @transtype='TTRITP'    
                SELECT  @out_stats_detail_type = 'TTP'    
            ELSE    
                SELECT  @out_stats_detail_type = 'TTC', @tax_value=-@tax_value    
        
            EXEC spu_ACT_Do_Currency_Conversion    
                @company_id = @company_id,    
                @currency_id = @out_currency_id,    
                @currency_amount_unrounded = @tax_value,    
                @mode = 'ALL',    
                @base_amount = @out_tax_value_home OUTPUT,    
                @system_amount = @out_tax_value_system OUTPUT,    
                @currency_base_xrate = @out_currency_rate OUTPUT,    
                @system_base_xrate = @out_system_rate OUTPUT,    
                @return_status = @return_status OUTPUT    
        
            -- Get next stats_detail_id and set type    
            SELECT  @out_stats_detail_id = ISNULL(MAX(stats_detail_id), 0) + 1    
            FROM    Stats_Detail    
            WHERE   stats_folder_cnt = @stats_folder_cnt    
        
            -- Insert the Stats Detail    
            INSERT INTO Stats_Detail (    
                stats_folder_cnt,    
                stats_detail_id,    
                stats_detail_type,    
                risk_id,    
                risk_type_id,    
                risk_type_code,    
                ri_share_percent,    
                this_premium_original,    
                this_premium_home,    
                this_premium_system,    
                currency_rate,    
                currency_code,    
                tax_type_id,    
                tax_type_code,    
                tax_value,   
                ri_party_cnt,
				--Start (Prakash Varghese) - (Tech Spec - TRAC 4761 Stats Detail to RI Line.docx) - (6.2)
				ri_arrangement_line_id
				--End (Prakash Varghese) - (Tech Spec - TRAC 4761 Stats Detail to RI Line.docx) - (6.2)
				)
            VALUES (   
                @stats_folder_cnt,    
                @out_stats_detail_id,    
                @out_stats_detail_type,    
                @risk_cnt,    
                @risk_type_id,    
                @risk_type_code,    
                @tax_percentage,    
                -@tax_value*@multiplier,    
                -@out_tax_value_home*@multiplier,    
                -@out_tax_value_system*@multiplier,    
                @out_currency_rate,    
                @out_currency_code,    
                @tax_band_id,    
                @tax_band_code,    
                -@tax_value*@multiplier,   
                @ri_party_cnt,
				--Start (Prakash Varghese) - (Tech Spec - TRAC 4761 Stats Detail to RI Line.docx) - (6.2)
				@RI_Arrangement_Line_ID
				--End (Prakash Varghese) - (Tech Spec - TRAC 4761 Stats Detail to RI Line.docx) - (6.2)
				)   
        
            --Write tax payable record    
            SELECT  @out_ri_shortname = 'NOTA' + RTRIM(@tax_band_code)    
        
            -- Get next stats_detail_id and set type    
            SELECT  @out_stats_detail_type = 'TAN'    
        
            SELECT  @out_stats_detail_id = ISNULL(MAX(stats_detail_id), 0) + 1    
            FROM    Stats_Detail    
            WHERE   stats_folder_cnt = @stats_folder_cnt    
        
            INSERT INTO Stats_Detail (    
                stats_folder_cnt,    
                stats_detail_id,    
                stats_detail_type,    
                risk_id,    
                risk_type_id,    
                risk_type_code,    
                ri_shortname,    
                ri_share_percent,    
                this_premium_original,    
                this_premium_home,    
                this_premium_system,    
                currency_rate,    
                currency_code,    
                tax_type_id,    
                tax_type_code,    
                tax_value,
				--Start (Prakash Varghese) - (Tech Spec - TRAC 4761 Stats Detail to RI Line.docx) - (6.2)
				ri_arrangement_line_id
				--End (Prakash Varghese) - (Tech Spec - TRAC 4761 Stats Detail to RI Line.docx) - (6.2)
				)    
            VALUES (    
                @stats_folder_cnt,    
                @out_stats_detail_id,    
                @out_stats_detail_type,    
                @risk_cnt,    
                @risk_type_id,    
                @risk_type_code,    
                @out_ri_shortname,    
                @tax_percentage,    
                @tax_value*@multiplier,    
                @out_tax_value_home*@multiplier,    
                @out_tax_value_system*@multiplier,    
                @out_currency_rate,    
                @out_currency_code,    
                @tax_band_id,    
                @tax_band_code,    
                @tax_value*@multiplier,
				--Start (Prakash Varghese) - (Tech Spec - TRAC 4761 Stats Detail to RI Line.docx) - (6.2)
				@RI_Arrangement_Line_ID
				--End (Prakash Varghese) - (Tech Spec - TRAC 4761 Stats Detail to RI Line.docx) - (6.2)
				)        
        END --IF @tax_value <> 0    
        
     FETCH NEXT FROM Taxes_Cursor    
         INTO   @tax_band_id,    
                @tax_premium,    
                @tax_percentage,    
                @tax_value,    
                @tax_is_value,    
                @tax_band_code,    
                @risk_cnt,    
                @risk_type_id,    
                @risk_type_code,    
                @transtype, 
    	        @ri_party_cnt,
                @is_withholding_tax,
				--Start (Prakash Varghese) - (Tech Spec - TRAC 4761 Stats Detail to RI Line.docx) - (6.2)  
				@RI_Arrangement_Line_ID
				--End (Prakash Varghese) - (Tech Spec - TRAC 4761 Stats Detail to RI Line.docx) - (6.2)
    END    
        
    -- Only close the cursor, we will use it again    
    CLOSE Taxes_Cursor    
    DEALLOCATE Taxes_Cursor    
      

GO

