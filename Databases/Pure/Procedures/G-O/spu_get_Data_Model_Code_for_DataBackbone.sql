
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_get_Data_Model_Code_for_DataBackbone'
GO


/*************************************************************************/  
/*Get DataModel codes for Data Backbone structure*/
/*************************************************************************/  
  
  
CREATE PROCEDURE spu_get_Data_Model_Code_for_DataBackbone
AS        
        
BEGIN  
DECLARE @TableOrder TABLE (OrderId INT, gis_data_model_type_id INT)
INSERT INTO @TableOrder (OrderId, gis_data_model_type_id) VALUES(1,4)
INSERT INTO @TableOrder (OrderId, gis_data_model_type_id) VALUES(2,1)
INSERT INTO @TableOrder (OrderId, gis_data_model_type_id) VALUES(3,2)
INSERT INTO @TableOrder (OrderId, gis_data_model_type_id) VALUES(4,5)
 
	SELECT DISTINCT wp.data_model, t.OrderId, gd.gis_data_model_type_id
	FROM wp_fields wp
	JOIN GIS_Data_Model gd
	ON wp.data_model=gd.code
	JOIN @TableOrder t
	ON t.gis_data_model_type_id=gd.gis_data_model_type_id
	WHERE wp.data_model <> ''
	AND wp.data_model NOT LIKE '%S4i%'
	AND wp.data_model NOT LIKE '%GII%'	
	AND wp.data_model NOT LIKE '%TEST%'
	AND gd.is_deleted <> 1

	ORDER BY t.OrderId
END  
GO