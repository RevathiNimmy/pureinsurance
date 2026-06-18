SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SIR_IsTMPProduct'
GO

Create Procedure spu_SIR_IsTMPProduct
@Product_Id as int
AS

Select is_true_monthly_policy from Product where Product_id = @Product_Id

GO