SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Risk_renewal_days_del'
GO

CREATE PROCEDURE spu_Risk_renewal_days_del
    @risk_group_id int
AS
DELETE FROM risk_renewal_days
WHERE risk_group_id = @risk_group_id
GO

