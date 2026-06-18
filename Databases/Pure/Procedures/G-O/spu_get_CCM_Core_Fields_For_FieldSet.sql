
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_get_CCM_Core_Fields_For_FieldSet'
GO
  
CREATE PROCEDURE spu_get_CCM_Core_Fields_For_FieldSet
@Table_Name AS VARCHAR(255)
AS        
        
BEGIN  
	SELECT DISTINCT column_name
	FROM wp_fields
	WHERE Table_Name=@Table_Name
END  
GO