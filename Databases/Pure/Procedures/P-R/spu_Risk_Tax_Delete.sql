SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Risk_Tax_Delete'
GO

CREATE PROCEDURE spu_Risk_Tax_Delete
    @tax_calculation_cnt int
AS

begin
	DELETE FROM Tax_Calculation
	WHERE tax_calculation_cnt = @tax_calculation_cnt 
end