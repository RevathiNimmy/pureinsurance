SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_DOC_Add_doc_name'
GO
CREATE PROCEDURE spu_DOC_Add_doc_name
    @doc_name_id integer OUTPUT,
    @doc_name varchar(254)
AS
BEGIN
    INSERT INTO DOC_doc_name (
        doc_name)
    VALUES (
        @doc_name)

    SELECT @doc_name_id = @@IDENTITY
END
GO

