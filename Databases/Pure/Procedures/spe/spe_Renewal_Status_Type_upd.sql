SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Renewal_Status_Type_upd'
GO

CREATE PROCEDURE spe_Renewal_Status_Type_upd
    @renewal_status_type_id int,
    @caption_id int,
    @code char(10),
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime
AS
BEGIN
UPDATE Renewal_Status_Type
    SET
    caption_id=@caption_id,
    code=@code,
    description=@description,
    is_deleted=@is_deleted,
    effective_date=@effective_date
WHERE renewal_status_type_id = @renewal_status_type_id
END

GO

