
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[spu_XMLGenerator_GetRiskDescription]
(
	@ModelName varchar(60)
)
AS
BEGIN


SELECT description FROM risk_type WHERE gis_screen_id 
in (SELECT gis_screen_id FROM GIS_Screen WHERE gis_data_model_id 
in (SELECT gis_data_model_id  FROM GIS_Data_Model WHERE code =@ModelName))

END