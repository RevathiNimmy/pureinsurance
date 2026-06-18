SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_update_risk_sel_status'
GO

-- Created: PW041002

CREATE PROCEDURE spu_update_risk_sel_status
    @risk_cnt integer,
    @is_selected tinyint
AS
BEGIN

    UPDATE risk
       SET is_risk_selected = @is_selected
     WHERE risk_cnt = @risk_cnt

END
GO
