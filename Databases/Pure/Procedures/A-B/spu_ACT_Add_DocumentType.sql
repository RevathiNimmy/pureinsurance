SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Add_DocumentType'
GO


CREATE PROCEDURE spu_ACT_Add_DocumentType
    @documenttype_id smallint,
    @doctypegroup_id smallint,
    @caption_id int,
    @is_deleted bit,
    @effective_date datetime,
    @description varchar(255),
    @code char(10)
AS


BEGIN
INSERT INTO DocumentType (
    documenttype_id ,
    doctypegroup_id ,
    caption_id ,
    is_deleted ,
    effective_date ,
    description ,
    code )
VALUES (
    @documenttype_id,
    @doctypegroup_id,
    @caption_id,
    @is_deleted,
    @effective_date,
    @description,
    @code)
END
GO


