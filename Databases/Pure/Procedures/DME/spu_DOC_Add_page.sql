SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_DOC_Add_page'
GO
CREATE PROCEDURE spu_DOC_Add_page
    @page_name varchar(255),
    @doc_num integer,
    @page_num integer,
    @page_type varchar(255),
    @page_size integer,
    @volume_id integer,
    @create_date datetime
AS
BEGIN
    INSERT INTO DOC_page (
        page_name,
        doc_num,
        page_num,
        page_type,
        page_size,
        volume_id,
        create_date)
    VALUES (
        @page_name,
        @doc_num,
        @page_num,
        @page_type,
        @page_size,
        @volume_id,
        @create_date)
END
GO

