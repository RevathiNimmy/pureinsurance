SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_prospect_status_upd'
GO

CREATE PROCEDURE spe_prospect_status_upd
    @prospect_status_id int,
    @caption_id int,
    @code char(10),
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime
AS
BEGIN
UPDATE prospect_status
    SET
    caption_id=@caption_id,
    code=@code,
    description=@description,
    is_deleted=@is_deleted,
    effective_date=@effective_date
WHERE prospect_status_id = @prospect_status_id
END

GO

