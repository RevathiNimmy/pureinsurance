SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_get_task_assignment_sub_details'
GO

CREATE PROCEDURE spu_get_task_assignment_sub_details
    @insurance_file_cnt int
AS
BEGIN

    IF EXISTS (SELECT * FROM Hidden_Options WHERE UW_type = 'U') BEGIN
    
    	-- Underwriting
        SELECT    
            src.code Source,         
            prd.code Product,    
            RSCC.code RenewalStopCodeClient,
            RSCP.code RenewalStopCodePolicy,
            AEP.shortname AccountExecutive,
            AHP.shortname AccountManager,
            prd.is_auto_renewable AutomateRenewalsFlag,
            i.is_referred_at_renewal ReferredAtRenewal,
            src.Description BranchName,
            SB.Description SubBranchName,
			c.description CurrencyDesc,
			src.source_id SourceID,
			c.currency_id CurrencyID,
			a.account_id AccountID
			

        FROM 
            insurance_file i
            LEFT OUTER JOIN Product prd ON prd.product_id = i.product_id
            LEFT OUTER JOIN source src ON src.source_id = i.source_id
            LEFT OUTER JOIN Party AHP ON AHP.party_cnt = i.account_handler_cnt 
            LEFT OUTER JOIN Renewal_stop_code RSCP ON RSCP.Renewal_stop_code_id = i.Renewal_stop_code_id
            LEFT OUTER JOIN sub_branch SB ON SB.sub_branch_id = i.source_id
            LEFT OUTER JOIN currency c ON c.currency_id = i.currency_id
            
            JOIN Party insP ON insP.party_cnt = i.insured_cnt  
			LEFT OUTER JOIN account a ON a.account_key = insP.party_cnt
            LEFT OUTER JOIN Party AEP ON AEP.party_cnt = insP.Consultant_cnt
            LEFT OUTER JOIN Renewal_stop_code RSCC ON RSCC.Renewal_stop_code_id = insP.Renewal_stop_code_id
        
        
        WHERE i.insurance_file_cnt = @insurance_file_cnt
         

    END ELSE BEGIN
    
    	-- Broking 
        SELECT    
            src.code Source,         
            NULL Product,    
            RSCC.code RenewalStopCodeClient,
            RSCP.code RenewalStopCodePolicy,
            AEP.shortname AccountExecutive,
            AHP.shortname AccountManager,
            NULL AutomateRenewalsFlag,
            NULL ReferredAtRenewal,
            src.Description BranchName,
            SB.Description SubBranchName,
			c.description CurrencyDesc,
			src.source_id SourceID,
			c.currency_id CurrencyID,
			a.account_id AccountID

            
        FROM 
            insurance_file i
            LEFT OUTER JOIN source src ON src.source_id = i.source_id
            LEFT OUTER JOIN Party AHP ON AHP.party_cnt = i.account_handler_cnt 
            LEFT OUTER JOIN Renewal_stop_code RSCP ON RSCP.Renewal_stop_code_id = i.Renewal_stop_code_id
            LEFT OUTER JOIN sub_branch SB ON SB.sub_branch_id = i.source_id
			LEFT OUTER JOIN currency c ON c.currency_id = i.currency_id
            
            JOIN Party insP ON insP.party_cnt = i.insured_cnt  
			LEFT OUTER JOIN account a ON a.account_key = insP.party_cnt
            LEFT OUTER JOIN Party AEP ON AEP.party_cnt = insP.Consultant_cnt
            LEFT OUTER JOIN Renewal_stop_code RSCC ON RSCC.Renewal_stop_code_id = insP.Renewal_stop_code_id
 
        WHERE i.insurance_file_cnt = @insurance_file_cnt
              
    
    
    END
END

GO


