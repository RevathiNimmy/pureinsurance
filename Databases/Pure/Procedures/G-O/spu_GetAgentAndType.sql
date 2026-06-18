SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_GetAgentAndType'
GO

CREATE PROCEDURE spu_GetAgentAndType  
 @PremiumFinanceCnt int,  
 @PremiumFinanceVersion int  
AS  
  
-- Underwriting / Broking wrapper  
IF EXISTS (SELECT * FROM Hidden_Options WHERE UW_type = 'U') BEGIN  
  
 --Get the Agent for the policy and the Agent Type  
 -- 1 = Broker Agent  
 -- 2 = Sub Agent  
 -- 3 = Commission Agent  

 -- Underwriting  
 SELECT ifi.lead_agent_cnt, PA.party_agent_type_id  
 FROM PFPremiumFinance pf  
  INNER JOIN Insurance_File ifi  
   ON pf.insurance_file_cnt = ifi.insurance_file_cnt  
  INNER JOIN Insurance_Folder ifo  
      ON ifi.insurance_folder_cnt = ifo.insurance_folder_cnt  
  INNER JOIN Party_agent PA  
   ON ifi.lead_agent_cnt = PA.party_cnt
 WHERE pf.pfprem_finance_cnt = @PremiumFinanceCnt  
 AND   pf.pfprem_finance_version = @PremiumFinanceVersion  
  
END ELSE BEGIN  
  
 -- Broking (only supports the first Agent)  
 SELECT TOP 1 PA.agent_cnt, 1  
 FROM PFPremiumFinance pf
  INNER JOIN Insurance_File ifi  
   ON pf.insurance_file_cnt = ifi.insurance_file_cnt  
  INNER JOIN Policy_Agents PA  
   ON PA.insurance_file_cnt = ifi.insurance_file_cnt  
 WHERE pf.pfprem_finance_cnt = @PremiumFinanceCnt  
 AND   pf.pfprem_finance_version = @PremiumFinanceVersion  
  
END  

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
