SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Renewal_Process_sel'
GO

CREATE PROCEDURE spe_Renewal_Process_sel
    @renewal_process_id int
AS
SELECT
    renewal_process_id,
    caption_id,
    code,
    description,
    is_deleted,
    effective_date
 FROM Renewal_Process
WHERE renewal_process_id = @renewal_process_id

GO

