SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Policy_Discount_Adjust_Values_Taxes'
GO

CREATE PROCEDURE spu_SIR_Policy_Discount_Adjust_Values_Taxes
	@insurance_file_cnt int,
	@discount_percentage float
AS

	BEGIN	
		UPDATE tax_calculation 
			SET value = value + (value * (@discount_percentage/100))
		WHERE insurance_file_Cnt = @insurance_file_cnt
		AND transtype in ('TTIF', 'TTR', 'TTF')
		AND is_value = 1
	END 


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
