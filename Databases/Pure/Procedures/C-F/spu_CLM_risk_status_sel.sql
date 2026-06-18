-------------------------------------------------------------------------------
--  Author:  AMB
--  Date:    09 Sept 2003
--  Desc:    SFU 1.8.6 Deferred Reinsurance development
--           Called by bOpenClaim
-------------------------------------------------------------------------------

SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_CLM_risk_status_sel'
GO


CREATE PROCEDURE spu_CLM_risk_status_sel
    @risk_cnt int
AS

SELECT
    r.risk_status_id
,   rs.code
FROM
    Risk AS r
INNER JOIN
    Risk_Status AS rs ON rs.risk_status_id = r.risk_status_id
WHERE
    r.risk_cnt = @risk_cnt
