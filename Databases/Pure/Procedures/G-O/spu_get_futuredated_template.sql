SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF

GO

EXECUTE DDLDropProcedure 'spu_get_futuredated_template'

GO

CREATE PROCEDURE spu_get_futuredated_template
    @code char(10),
    @effective_date datetime
AS
    SELECT @effective_date=convert(char(10),@effective_date,101) + ' 23:59:59.998'
    
    SELECT document_template_id , code
    FROM document_template 
    WHERE code = @code 
    AND is_deleted = 0 
    AND effective_date > @effective_date 

GO