SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_DOC_Add_doc_keyword'
GO
CREATE PROCEDURE spu_DOC_Add_doc_keyword
    @doc_keyword_id integer OUTPUT,
    @keyword_id integer,
    @doc_num integer,
    @user_name varchar(16),
    @create_date datetime
AS
BEGIN
    INSERT INTO DOC_doc_keyword (
        keyword_id,
        doc_num,
        user_name,
        create_date)
    VALUES (
        @keyword_id,
        @doc_num,
        @user_name,
        @create_date)

    SELECT @doc_keyword_id = @@IDENTITY
END
GO

