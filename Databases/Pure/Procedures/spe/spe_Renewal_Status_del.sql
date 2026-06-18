SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Renewal_Status_del'
GO

CREATE PROCEDURE spe_Renewal_Status_del
    @renewal_status_cnt int
AS
DELETE FROM Renewal_Status
WHERE renewal_status_cnt = @renewal_status_cnt

GO

