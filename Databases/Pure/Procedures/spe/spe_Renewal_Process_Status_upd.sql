SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Renewal_Process_Status_upd'
GO

CREATE PROCEDURE spe_Renewal_Process_Status_upd
    @renewal_status_type_id int,
    @renewal_process_id int,
    @description varchar(255)
AS
BEGIN
UPDATE Renewal_Process_Status
    SET
    description=@description
WHERE renewal_status_type_id = @renewal_status_type_id AND renewal_process_id = @renewal_process_id
END

GO

