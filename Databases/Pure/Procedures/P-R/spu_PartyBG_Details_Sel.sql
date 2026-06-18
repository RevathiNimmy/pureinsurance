SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_PartyBG_Details_Sel'
GO
CREATE PROCEDURE spu_PartyBG_Details_Sel
@Party_Cnt INT = NULL,
@bg_id  INT = NULL

As
BEGIN

  SELECT          
         -1  RowStatus,          
         0  RowIndex,          
         bg_id,          
         bank_name_id,          
         bank_branch,          
         BG.Party_Cnt,          
         BG_ref,          
         BG_currency_Id,          
         BG_limit,          
         available_bal,       
         --****************************************************************************************  
         --Note:Currency Conversion related codes are commented below since it may be used in future  
         --and it is told by Gaurav  
         --****************************************************************************************        
         --exchange_rate_override_reason_id,     
         --base_currency_id,          
         --account_currency_id,        
         --base_currency_xrate,          
         --account_currency_id,          
         --account_currency_xrate,          
         --system_currency_xrate,          
         --base_Currency_date,          
         --account_currency_date,          
         --system_currency_date,         
         --Effective_Date,          
         expiry_date,          
         is_policy_lock,         
         bg_status_id,         
         custody_branch_id,        
                issue_date,        
                NULL,          
                NULL,          
                BG.is_deleted,          
                P.Shortname,          
                P.Resolved_Name          
  FROM Bank_Guarantee BG          
  INNER JOIN Party P          
  ON BG.Party_cnt = P.Party_Cnt          
  WHERE (@Party_Cnt IS NULL OR BG.Party_Cnt = @Party_Cnt)          
  AND (@bg_id IS NULL OR bg_id = @bg_id)          
END 
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO        