SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_Write_Off_Reason'
GO


CREATE PROCEDURE spu_ACT_Update_Write_Off_Reason
    @write_off_reason_id int,
    @description varchar(255),
    @is_deleted tinyint,
    @code varchar(10),
    @caption_id int,
    @effective_date datetime
AS


BEGIN
UPDATE Write_Off_Reason
    SET
    description=@description,
    is_deleted=@is_deleted,
    code=@code,
    caption_id=@caption_id,
    effective_date=@effective_date
WHERE write_off_reason_id = @write_off_reason_id
END
GO


