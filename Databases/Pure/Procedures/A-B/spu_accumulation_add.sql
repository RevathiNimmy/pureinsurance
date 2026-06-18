SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_accumulation_add'
GO
CREATE PROCEDURE spu_accumulation_add
    @accumulation_id integer OUTPUT,
    @code char(10),
    @caption_id integer,
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime,
    @quick_code char(10),
    @caption char(40),
    @parent_id integer,
    @accumulation_class_id integer
AS
BEGIN

    INSERT INTO accumulation 
	(
        code,
        caption_id,
        description,
        is_deleted,
        effective_date,
        quick_code,
        caption,
        parent_id,
        accumulation_class_id
    )
	VALUES 
	(
        @code,
        @caption_id,
        @description,
        @is_deleted,
        @effective_date,
        @quick_code,
        @caption,
        @parent_id,
        @accumulation_class_id
    )

    SELECT @accumulation_id=@@IDENTITY
END

GO

