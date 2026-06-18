SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_sir_get_wht_value'
GO

CREATE PROCEDURE spu_sir_get_wht_value
	 @TaxGroupCode varchar(10)
AS
BEGIN

    -- Insert statements for procedure here
	select is_withholding_tax as IsWithholdingTax from tax_group where code= @TaxGroupCode
	
END
