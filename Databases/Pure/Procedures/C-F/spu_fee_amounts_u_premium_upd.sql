EXECUTE DDLDropProcedure 'spu_fee_amounts_u_premium_upd'
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE spu_fee_amounts_u_premium_upd

(
    @insurance_file_cnt            int,
    @annual_premium           numeric(19,4)
)

AS


BEGIN

    UPDATE
        Insurance_file
    SET
	annual_premium = @annual_premium	
    WHERE
        insurance_file_cnt = @insurance_file_cnt 

END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

