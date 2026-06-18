
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_get_SW_ColumnName'
GO

CREATE PROCEDURE spu_get_SW_ColumnName
@DataModelCode AS VARCHAR(50),
@Table_Name As VARCHAR(255)
AS        
        
BEGIN  

SELECT DISTINCT column_name FROM wp_fields
WHERE Table_Name=@Table_Name
AND specials_type=5  /*Endorsement*/
AND data_model=@DataModelCode
AND column_name IN (SELECT name FROM sys.columns WHERE object_id = OBJECT_ID(@Table_Name))

END  
GO