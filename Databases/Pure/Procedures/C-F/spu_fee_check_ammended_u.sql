EXECUTE DDLDropProcedure 'spu_fee_check_ammended_u'
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE spu_fee_check_ammended_u

(
	@fee_amount_id	int,
	@fee_percentage	numeric(7,4),
	@fee_amount		numeric(19,4)
)

 AS

SELECT     fee_percentage , fee_amount FROM fee_amounts_u WHERE     (fee_amount_id = @fee_amount_id)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

