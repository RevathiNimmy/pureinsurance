SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_DOC_Add_document'
GO

CREATE PROCEDURE spu_DOC_Add_document
    @doc_num integer OUTPUT,
    @folder_num integer,
    @doc_name varchar(255),
    @ex_code varchar(20),
    @doc_type char(1),
    @access_level tinyint,
    @password varchar(12),
    @zipped char(1),
    @create_date datetime,
    @link integer,
    @document_template_id integer,
    @visible_from_web bit = 1
AS
BEGIN
    INSERT INTO DOC_document (
        folder_num,
        doc_name,
        ex_code,
        doc_type,
        access_level,
        password,
        zipped,
        create_date,
        link, 
        document_template_id,
	visible_from_web)
    VALUES (
        @folder_num,
        @doc_name,
        @ex_code,
        @doc_type,
        @access_level,
        @password,
        @zipped,
        @create_date,
        @link,
        @document_template_id,
	@visible_from_web)

    SELECT @doc_num = @@IDENTITY
END
GO


