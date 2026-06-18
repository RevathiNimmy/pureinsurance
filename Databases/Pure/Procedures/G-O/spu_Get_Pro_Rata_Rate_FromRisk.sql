
EXECUTE Ddldropprocedure 'spu_Get_Pro_Rata_Rate_FromRisk'

GO

Create Procedure spu_Get_Pro_Rata_Rate_FromRisk
	@Risk_cnt INT
AS
	SELECT Pro_rata_rate FROM Risk 
	WHERE  Risk_cnt = @Risk_cnt
	
GO
	