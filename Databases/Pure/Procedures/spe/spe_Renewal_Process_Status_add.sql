SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Renewal_Process_Status_add'
GO

CREATE PROCEDURE spe_Renewal_Process_Status_add
    @renewal_status_type_id int,
    @renewal_process_id int,
    @description varchar(255)
AS
BEGIN
INSERT INTO Renewal_Process_Status (
    renewal_status_type_id ,
    renewal_process_id ,
    description )
VALUES (
    @renewal_status_type_id,
    @renewal_process_id,
    @description)
END

GO

