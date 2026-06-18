
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_get_CCM_Fields_For_FieldSet'
GO

CREATE PROCEDURE spu_get_CCM_Fields_For_FieldSet
@FieldSet VARCHAR(255)
AS        
        
BEGIN  
	SELECT DISTINCT column_name 
	FROM wp_fields 
	WHERE table_name = @FieldSet
	AND specials_type <> 5 /*Endorsement*/
END  
GO