SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_DOC_Add_doc_info'
GO
CREATE PROCEDURE spu_DOC_Add_doc_info
    @doc_num integer,
    @expiry_date datetime,
    @scan_user varchar(16),
    @doc_date datetime,
    @last_user varchar(16),
    @last_date datetime
AS
BEGIN
    INSERT INTO DOC_doc_info (
        doc_num,
        expiry_date,
        scan_user,
        doc_date,
        last_user,
        last_date)
    VALUES (
        @doc_num,
        @expiry_date,
        @scan_user,
        @doc_date,
        @last_user,
        @last_date)
END
GO

