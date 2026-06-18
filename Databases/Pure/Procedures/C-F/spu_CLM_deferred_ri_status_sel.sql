-------------------------------------------------------------------------------
--  Author:  AMB
--  Date:    02 Sept 2003
--  Desc:    SFU 1.8.6 Deferred Reinsurance development
--           Called by bCLMCheckDeferredRI
-------------------------------------------------------------------------------

SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_CLM_deferred_ri_status_sel'
GO


CREATE PROCEDURE spu_CLM_deferred_ri_status_sel
    @claim_id int
AS

SELECT
    r.risk_status_id
,   rs.code
,   c.client_name
FROM
    Claim AS c
INNER JOIN
    Risk AS r ON r.risk_cnt = c.risk_type_id
INNER JOIN
    Risk_Status AS rs ON rs.risk_status_id = r.risk_status_id
WHERE
    c.claim_id = @claim_id


