SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_PFPremiumFinance_Agent_Upd'
GO

CREATE PROCEDURE spu_PFPremiumFinance_Agent_Upd
   @pfprem_finance_cnt Int,
   @pfprem_finance_version Int,
   @agent_cnt Int
AS
UPDATE PFPremiumFinance
SET agent_cnt=@agent_cnt
WHERE pfprem_finance_cnt=@pfprem_finance_cnt
AND pfprem_finance_version=@pfprem_finance_version
GO
