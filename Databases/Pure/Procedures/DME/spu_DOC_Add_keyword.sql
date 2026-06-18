SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_DOC_Add_keyword'
GO
CREATE PROCEDURE spu_DOC_Add_keyword
    @keyword_id integer OUTPUT,
    @keyword varchar(30),
    @deleted char(1)
AS
BEGIN
    INSERT INTO DOC_keyword (
        keyword,
        deleted)
    VALUES (
        @keyword,
        @deleted)

    SELECT @keyword_id = @@IDENTITY
END
GO

