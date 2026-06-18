SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GEM_Select_code_conversion'
GO


CREATE PROCEDURE spu_GEM_Select_code_conversion
    @conversion_list_id smallint,
    @input_code_value varchar(255)
AS


SELECT
    conversion_list_id,
    input_code_value,
    output_code_value
 FROM code_conversion
WHERE conversion_list_id = @conversion_list_id AND input_code_value = @input_code_value
GO


