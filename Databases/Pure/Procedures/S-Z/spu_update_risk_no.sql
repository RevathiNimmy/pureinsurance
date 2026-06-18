SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_update_risk_no'
GO

-- Created: PW041002

CREATE PROCEDURE spu_update_risk_no
    @risk_cnt integer,
    @risk_number integer
AS

BEGIN

    UPDATE risk
       SET risk_number = @risk_number,
	   	   variation_number = 0
     WHERE risk_cnt = @risk_cnt

END
GO