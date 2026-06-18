SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Renewal_Process_upd'
GO

CREATE PROCEDURE spe_Renewal_Process_upd
    @renewal_process_id int,
    @caption_id int,
    @code char(10),
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime
AS
BEGIN
UPDATE Renewal_Process
    SET
    caption_id=@caption_id,
    code=@code,
    description=@description,
    is_deleted=@is_deleted,
    effective_date=@effective_date
WHERE renewal_process_id = @renewal_process_id
END

GO

