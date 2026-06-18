SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_next_risk_no'
GO

-- Created: PW041002

CREATE PROCEDURE spu_get_next_risk_no
    @risk_number integer OUT,
    @insurance_file_cnt integer
AS

BEGIN

        SELECT @risk_number = max(risk_number)
          FROM risk r
    INNER JOIN insurance_file_risk_link ifrl
            ON r.risk_cnt = ifrl.risk_cnt
         WHERE insurance_file_cnt = @insurance_file_cnt

END
GO