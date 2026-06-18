DDLDropProcedure spu_DOC_get_option
GO

CREATE PROCEDURE spu_DOC_get_option
    @option_name VARCHAR(50)
AS
BEGIN

SELECT option_value 
FROM doc_options 
WHERE option_name = @option_name

END
GO
