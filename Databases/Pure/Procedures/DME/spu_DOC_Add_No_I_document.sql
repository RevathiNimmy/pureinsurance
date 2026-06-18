SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_DOC_Add_No_I_document'
GO
CREATE PROCEDURE spu_DOC_Add_No_I_document
    @doc_num integer,
    @folder_num integer,
    @doc_name varchar(50),
    @ex_code varchar(20),
    @doc_type char(1),
    @access_level tinyint,
    @password varchar(12),
    @zipped char(1),
    @create_date datetime,
    @link integer
AS
BEGIN
    INSERT INTO DOC_document (
        doc_num,
        folder_num,
        doc_name,
        ex_code,
        doc_type,
        access_level,
        password,
        zipped,
        create_date,
        link)
    VALUES (
        @doc_num,
        @folder_num,
        @doc_name,
        @ex_code,
        @doc_type,
        @access_level,
        @password,
        @zipped,
        @create_date,
        @link)
END
GO

