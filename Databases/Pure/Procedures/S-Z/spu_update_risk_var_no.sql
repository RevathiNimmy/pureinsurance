SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_update_risk_var_no'
GO

-- Created: PW041002

CREATE PROCEDURE spu_update_risk_var_no
    @risk_cnt integer,
    @variation_number integer
AS

BEGIN

    UPDATE risk
       SET variation_number = @variation_number
     WHERE risk_cnt = @risk_cnt

END
GO

