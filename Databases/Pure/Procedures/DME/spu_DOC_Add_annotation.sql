SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_DOC_Add_annotation'
GO
CREATE PROCEDURE spu_DOC_Add_annotation
    @annotation_id integer OUTPUT,
    @doc_num integer,
    @ann_text varchar(255),
    @user_name varchar(16),
    @create_date datetime
AS
BEGIN
    INSERT INTO DOC_annotation (
        doc_num,
        ann_text,
        user_name,
        create_date)
    VALUES (
        @doc_num,
        @ann_text,
        @user_name,
        @create_date)

    SELECT @annotation_id = @@IDENTITY
END
GO

