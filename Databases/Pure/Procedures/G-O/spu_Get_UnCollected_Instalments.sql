
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

EXECUTE DDLDropProcedure 'spu_Get_UnCollected_Instalments'
GO

Create PROCEDURE spu_Get_UnCollected_Instalments
@pfprem_finance_cnt int,
@pfprem_finance_version int

AS

DECLARE @nUncollectedInstalmentCount INT

SELECT @nUncollectedInstalmentCount = Count(*) 
FROM PFInstalments
WHERE pfprem_finance_cnt = @pfprem_finance_cnt
AND pfprem_finance_version = @pfprem_finance_version
AND Status <> 3
AND TransactionCode > 2
AND InstalmentNumber != 0
SELECT @nUncollectedInstalmentCount

Go