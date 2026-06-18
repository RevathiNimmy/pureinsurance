SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_PFPremiumFinance_Status_Update'
GO


CREATE PROCEDURE spu_PFPremiumFinance_Status_Update

@pfprem_finance_cnt int, 
@pfprem_finance_version int, 
@statusind varchar(3)


AS

BEGIN

UPDATE pfPremiumFinance 
SET statusInd = @statusInd
WHERE pfprem_financE_cnt = @pfprem_financE_cnt
AND pfprem_finance_version = @pfprem_finance_version

END




GO
