SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_risk_renewal_days_sel'
GO

CREATE PROCEDURE spu_risk_renewal_days_sel
    @risk_group_id int
AS
SELECT
   service_level_id,
   renewal_days
 FROM risk_renewal_days
WHERE risk_group_id = @risk_group_id
ORDER BY service_level_id
GO

