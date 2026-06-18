SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Renewal_Process_Status_sel'
GO

CREATE PROCEDURE spe_Renewal_Process_Status_sel
    @renewal_status_type_id int,
    @renewal_process_id int
AS
SELECT
    renewal_status_type_id,
    renewal_process_id,
    description
 FROM Renewal_Process_Status
WHERE renewal_status_type_id = @renewal_status_type_id AND renewal_process_id = @renewal_process_id

GO

