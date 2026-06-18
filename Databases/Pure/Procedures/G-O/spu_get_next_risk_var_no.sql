SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_next_risk_var_no'
GO

-- Created: PW041002

CREATE PROCEDURE spu_get_next_risk_var_no
    @variation_number integer OUT,
    @risk_number integer,
    @insurance_file_cnt integer
AS

BEGIN

        SELECT @variation_number = max(variation_number)
          FROM risk r
    INNER JOIN insurance_file_risk_link ifrl
            ON r.risk_cnt = ifrl.risk_cnt
         WHERE ifrl.insurance_file_cnt = @insurance_file_cnt
           AND r.risk_number = @risk_number

END
GO