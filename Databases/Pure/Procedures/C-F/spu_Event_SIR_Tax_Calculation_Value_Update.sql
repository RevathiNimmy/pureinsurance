SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Event_SIR_Tax_Calculation_Value_Update'
GO

CREATE PROCEDURE spu_Event_SIR_Tax_Calculation_Value_Update
	@tax_calculation_cnt int,
	@tax_value money

AS 

BEGIN

	Update event_tax_calculation 
	SET value = @tax_value	
	WHERE tax_calculation_cnt = @tax_calculation_cnt

END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
