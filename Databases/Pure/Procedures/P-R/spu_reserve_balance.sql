SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_reserve_balance'
GO

CREATE PROCEDURE spu_reserve_balance  
    @claim_id int,  
    @user_id int,  
    @user_name varchar(255)  
AS  
  
    DECLARE @this_revision_total money,  
            @insurance_file_cnt int,  
            @document_comment varchar(255),  
            @stats_folder_cnt int,  
            @claim_peril_id int,  
            @cob_id int,  
            @cob_code varchar(255),  
            @ri_shortname varchar(255)  
  
    -- Balance all reserves on reserve tables for given claim  
    UPDATE  wr  
    SET     revision_count = ISNULL(revision_count, 0) + 1,  
            this_payment = 0, -- ensure zero  
            this_revision = Case this_revision WHEN 0 THEN 
								paid_to_date - (initial_reserve + revised_reserve)
							ELSE
								this_revision
							END,
            revised_reserve = revised_reserve + (paid_to_date - (initial_reserve + revised_reserve))  
    FROM    reserve wr  
    JOIN    claim_peril wcp  
            ON wcp.claim_peril_id = wr.claim_peril_id  
    WHERE   wcp.claim_id = @claim_id  
  
    -- Get total of reserve adjustments  
    SELECT  @this_revision_total = SUM(wr.this_revision)  
    FROM    reserve wr  
    JOIN    claim_peril wcp  
            ON wcp.claim_peril_id = wr.claim_peril_id  
    WHERE   wcp.claim_id = @claim_id  
  
    -- If we have an adjustment we must create stats  
    IF ISNULL(@this_revision_total, 0) <> 0 BEGIN  
        -- Get additional info from claim header  
        SELECT  @insurance_file_cnt = policy_id,  
                @document_comment = 'Reserve for claim number ' + claim_number  
        FROM    claim  
        WHERE   claim_id = @claim_id  
  
        -- Create stats folder  
        EXECUTE spu_add_stats_folder_claims  
            @stats_folder_cnt = @stats_folder_cnt OUTPUT,  
            @insurance_file_cnt = @insurance_file_cnt,  
            @debit_credit = 'D',  
            @document_comment = @document_comment,  
            @transaction_type_id = 28,  
            @transaction_type_code = 'C_CR',  
            @user_id = @user_id,  
            @user_name = @user_name,  
            @claim_id = @claim_id,  
            @documenttype_id = 35  
  
        -- We need to create gross stats for this claim by peril  
        DECLARE Peril_Cursor CURSOR FAST_FORWARD FOR  
            SELECT  wcp.claim_peril_id,  
                    SUM(wr.this_revision),  
                    cob.class_of_business_id,  
                    cob.code,  
                    'CLMRES' + cob.code  
            FROM    reserve wr  
            JOIN    claim_peril wcp  
                    ON wcp.claim_peril_id = wr.claim_peril_id  
            JOIN    peril_type pt  
                    ON pt.peril_type_id = wcp.peril_type_id  
            JOIN    class_of_business cob  
                    ON cob.class_of_business_id = pt.class_of_business_id  
            WHERE   wcp.claim_id = @claim_id  
            GROUP BY  
                    wcp.claim_peril_id, cob.class_of_business_id, cob.code  
            HAVING  SUM(wr.this_revision) <> 0  
            ORDER BY  
                    wcp.claim_peril_id  
  
        -- Open cursor and get first record  
        OPEN Peril_Cursor  
        FETCH NEXT FROM Peril_Cursor INTO  
            @claim_peril_id, @this_revision_total, @cob_id, @cob_code, @ri_shortname  
  
        -- Process all perils  
        WHILE @@FETCH_STATUS = 0 BEGIN  
            -- Create the gross line for this peril  
            EXECUTE spu_add_stats_details_claims  
                @stats_folder_cnt = @stats_folder_cnt,  
                @claim_id = @claim_id,  
                @peril_id = @claim_peril_id,  
                @stats_detail_type = 'GRS',  
                @class_of_business_id = @cob_id,  
                @class_of_business_code = @cob_code,  
                @ri_party_cnt = 0,  
                @ri_shortname = @ri_shortname,  
                @ri_party_type = 0,  
                @ri_share_percent = 0,  
                @transaction_amount = @this_revision_total,  
                @documenttype_id = 35  
  
            -- Next row  
            FETCH NEXT FROM Peril_Cursor INTO  
                @claim_peril_id, @this_revision_total, @cob_id, @cob_code, @ri_shortname  
        END  
  
        -- Close and deallocate cursor  
        CLOSE Peril_Cursor  
        DEALLOCATE Peril_Cursor  
    END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
