SET QUOTED_IDENTIFIER ON
GO

EXECUTE DDLDropProcedure 'spu_GetWPFieldsFor_ForAnalysisMode'
GO

CREATE PROCEDURE spu_GetWPFieldsFor_ForAnalysisMode 

AS

SELECT  DISTINCT  
wp.main_group MAINGROUP,  
wp.sub_group SUBGROUP,  
wp.Table_Name TABLENAME,  
wp.data_model datamodel,  
wp.loop1,  
wp.loop2,  
wp.loop3,  
datastructure_name,  
CASE specials_type  
WHEN 5 THEN column_name  
ELSE NULL end SW,  
CASE specials_type  
WHEN 5 THEN specials_type  
ELSE 0 end  
FROM wp_fields wp  
LEFT JOIN GIS_Data_Model gd  
ON gd.code = wp.data_model  
  
WHERE ISNULL(wp.table_name,'') <>''  
       AND ISNULL(wp.data_model,'') NOT LIKE '%S4i%'  
       AND ISNULL(wp.data_model,'') NOT LIKE '%GII%'  
       AND ISNULL(wp.data_model,'') NOT LIKE '%TEST%'  
       AND ISNULL(gd.is_deleted,0) <> 1  
  
AND ((specials_type = 5 and column_name NOT IN ('SWDESC','SWCODE' )) OR ISNULL(specials_type,0) <> 5)  
ORDER BY wp.data_model,wp.main_group,wp.sub_group, wp.loop1, wp.loop2, wp.loop3  


GO

