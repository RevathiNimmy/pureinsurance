EXECUTE DDLDropProcedure 'spu_fee_amounts_u_ammended_check'
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE spu_fee_amounts_u_ammended_check 
(
    @fee_amount_id	   int
)

AS


UPDATE fee_amounts_u
SET is_ammended=1
WHERE fee_amount_id = @fee_amount_id

GO


