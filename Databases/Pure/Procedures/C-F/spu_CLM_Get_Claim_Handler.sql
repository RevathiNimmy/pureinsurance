SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Claim_Handler'
GO

Create Procedure spu_CLM_Get_Claim_Handler
AS
	Select Handler_Id , Description 
		From Handler
