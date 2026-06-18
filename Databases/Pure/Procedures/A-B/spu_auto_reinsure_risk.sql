SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_auto_reinsure_risk'
GO


CREATE PROCEDURE spu_auto_reinsure_risk
    @risk_cnt integer
AS


BEGIN

    /* Select 'Is Auto RI', 'Is RI At Risk Level' and 'Risk Type' for the Risk */
    SELECT  r.is_auto_reinsured
    FROM    risk r
    WHERE   r.risk_cnt = @risk_cnt

END
GO


