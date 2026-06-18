SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Check_RunAuthScriptsEnabledAtPoroduct'
GO

CREATE PROCEDURE spu_Check_RunAuthScriptsEnabledAtPoroduct
AS
Select Count(*) from Product
WHERE run_authorisation_scripts_claim_payments =1 