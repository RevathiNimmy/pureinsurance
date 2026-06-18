SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Tax_Calculation_Value_Update'
GO

CREATE PROCEDURE spu_SIR_Tax_Calculation_Value_Update
	@tax_calculation_cnt int,
 @tax_value money output

AS 

BEGIN

	Update tax_calculation 
	SET value = ROUND(@tax_value,4)
	WHERE tax_calculation_cnt = @tax_calculation_cnt

 set @tax_value = ROUND(@tax_value,4)	 
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
