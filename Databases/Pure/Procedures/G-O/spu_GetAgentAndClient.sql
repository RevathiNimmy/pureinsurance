SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_GetAgentAndClient'
GO


CREATE PROCEDURE spu_GetAgentAndClient
	@PremFinanceCnt int,
	@PremFinanceVersion int

AS

SELECT ifi.lead_agent_cnt, ifo.insurance_holder_cnt
FROM PFPremiumFinance pf,
     Insurance_File ifi,
     Insurance_Folder ifo
WHERE pf.pfprem_finance_cnt = @PremFinanceCnt
AND   pf.pfprem_finance_version = @PremFinanceVersion
AND   pf.insurance_file_cnt = ifi.insurance_file_cnt
AND   ifi.insurance_folder_cnt = ifo.insurance_folder_cnt
GO
