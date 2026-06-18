SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Renewal_Process_del'
GO

CREATE PROCEDURE spe_Renewal_Process_del
    @renewal_process_id int
AS
DELETE FROM Renewal_Process
WHERE renewal_process_id = @renewal_process_id

GO

