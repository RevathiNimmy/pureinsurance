SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_AccountType'
GO


CREATE PROCEDURE spu_ACT_Update_AccountType
    @accounttype_id smallint,
    @caption_id int,
    @is_deleted bit,
    @effective_date datetime,
    @description varchar(255),
    @code char(10),
    @fundamental_type smallint
AS


BEGIN
UPDATE AccountType
    SET
    caption_id=@caption_id,
    is_deleted=@is_deleted,
    effective_date=@effective_date,
    description=@description,
    code=@code,
    fundamental_type=@fundamental_type
WHERE accounttype_id = @accounttype_id
END
GO


