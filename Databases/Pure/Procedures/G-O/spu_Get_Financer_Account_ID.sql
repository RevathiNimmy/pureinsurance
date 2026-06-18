EXECUTE DDLDropProcedure 'spu_Get_Financer_Account_ID'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_Get_Financer_Account_ID
@PlanRef Varchar(20)
AS

SELECT a.account_id from pfpremiumfinance pfp
JOIN pfscheme pfs
ON pfs.companyno = pfp.companyno
AND pfs.schemeno = pfp.schemeno
AND pfs.schemeversion = pfp.schemeversion
JOIN account a on pfs.Party_Cnt = a.account_key
WHERE pfprem_finance_cnt = @PlanRef

GO