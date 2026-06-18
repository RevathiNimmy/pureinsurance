DDLDropProcedure spu_DOC_set_option
GO

CREATE PROCEDURE spu_DOC_set_option
    @option_name VARCHAR(50),
    @option_value VARCHAR(100)
AS
BEGIN

UPDATE doc_options 
SET option_value = @option_value 
WHERE option_name = @option_name

END
GO
