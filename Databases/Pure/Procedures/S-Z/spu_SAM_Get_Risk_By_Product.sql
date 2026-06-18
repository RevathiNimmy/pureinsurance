SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SAM_Get_Risk_By_Product'
GO

CREATE PROCEDURE spu_SAM_Get_Risk_By_Product
    @ProductID int=0
AS

SELECT DISTINCT rt.risk_type_id,
		rt.code,
		c.caption,
		rt.gis_screen_id,
		gdm.code AS DataModel,
		gs.code AS Screen
	   FROM Risk_Type rt
     INNER JOIN GIS_Screen gs ON rt.gis_screen_id = gs.gis_screen_id
     INNER JOIN GIS_Data_Model gdm ON gs.gis_data_model_id = gdm.gis_data_model_id
     INNER JOIN Risk_Type_Usage rtu ON rt.risk_type_id = rtu.risk_type_id
     INNER JOIN Product_Risk_Type_Group prtg ON rtu.risk_type_group_id = prtg.risk_type_group_id
     INNER JOIN PMCaption c ON rt.caption_id = c.caption_id
          WHERE (c.language_id = 1)
            And (rt.is_deleted = 0)
            And (rt.gis_screen_id Is Not Null)
            And (((prtg.product_id = @ProductID)AND @ProductID<>0)OR (@ProductID=0))

GO