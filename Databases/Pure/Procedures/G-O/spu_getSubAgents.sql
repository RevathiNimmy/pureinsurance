

/* Created by : Vidya Rangdale
Creation Date : 26/02/2014
Description   : This is used to select details from insurance_file_agent table
Test Code     : Exec spu_getSubAgents  
 */

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_getSubAgents'
GO

CREATE PROCEDURE spu_getSubAgents  
   @nPremiumFinanceCnt INT,  
   @nPremiumFinanceVersion INT  
AS  
SELECT ifa.party_cnt FROM insurance_file_agent ifa  
INNER JOIN insurance_file ifi ON ifa.insurance_file_cnt=ifi.insurance_file_cnt  
INNER JOIN PFPremiumFinance pmf ON ifi.insurance_file_cnt=pmf.insurance_file_cnt  
WHERE pmf.pfprem_finance_cnt=@nPremiumFinanceCnt   
AND pmf.pfprem_finance_version=@nPremiumFinanceVersion  

GO
