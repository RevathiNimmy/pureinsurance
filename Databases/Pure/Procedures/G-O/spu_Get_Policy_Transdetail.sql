EXECUTE DDLDropProcedure 'spu_Get_Policy_Transdetail'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_Get_Policy_Transdetail
@PlanRef Varchar(20)
AS

SELECT  PlanTransaction_id,amount,pfprem_finance_version FROM PFPremiumFinance 
JOIN TransDetail ON transdetail_id=PlanTransaction_id
WHERE pfprem_finance_cnt = @PlanRef

GO