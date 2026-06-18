SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Risk_group_upd'
GO

CREATE PROCEDURE spe_Risk_group_upd
    @risk_group_id int,
    @caption_id int,
    @code char(10),
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime
AS
BEGIN
UPDATE Risk_group
    SET
    caption_id=@caption_id,
    code=@code,
    description=@description,
    is_deleted=@is_deleted,
    effective_date=@effective_date
WHERE risk_group_id = @risk_group_id
END

GO

