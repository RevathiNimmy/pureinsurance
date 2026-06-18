
DECLARE @gis_data_model_id INT
DECLARE @gis_data_model_code VARCHAR(20)
SET @gis_data_model_code='G4'
SELECT @gis_data_model_id=gis_data_model_id from GIS_Data_Model WHERE code=@gis_data_model_code

IF @gis_data_model_id IS NOT NULL
BEGIN
DELETE GIS_Screen_Detail WHERE GIS_Screen_id IN (SELECT GIS_Screen_id FROM GIS_Screen WHERE gis_data_model_id=@gis_data_model_id)
UPDATE Party_Type SET gis_screen_id=null WHERE GIS_Screen_id IN (SELECT GIS_Screen_id FROM GIS_Screen WHERE gis_data_model_id=@gis_data_model_id)
DELETE risk_type_rule_set WHERE risk_type_id in (SELECT risk_type_id FROM Risk_Type WHERE gis_screen_id in (SELECT gis_screen_id FROM GIS_Screen WHERE gis_data_model_id=@gis_data_model_id))
DELETE Risk_Type_Rating_Section_Type WHERE risk_type_id in (SELECT risk_type_id FROM Risk_Type WHERE gis_screen_id in (SELECT gis_screen_id FROM GIS_Screen WHERE gis_data_model_id=@gis_data_model_id))
DELETE Wording_Risk_Type_Link WHERE Risk_Type_id in (SELECT risk_type_id FROM Risk_Type WHERE gis_screen_id in (SELECT gis_screen_id FROM GIS_Screen WHERE gis_data_model_id=@gis_data_model_id))
DELETE Risk_Type_Usage WHERE risk_type_id in (SELECT risk_type_id FROM Risk_Type WHERE gis_screen_id in (SELECT gis_screen_id FROM GIS_Screen WHERE gis_data_model_id=@gis_data_model_id))
DELETE Risk_Type WHERE gis_screen_id in (SELECT gis_screen_id FROM GIS_Screen WHERE gis_data_model_id=@gis_data_model_id)
DELETE Peril_Type_Usage WHERE peril_type_id in (SELECT peril_type_id from Peril_Type WHERE gis_screen_id in (SELECT gis_screen_id FROM GIS_Screen WHERE gis_data_model_id=@gis_data_model_id))
DELETE Peril_Type WHERE gis_screen_id in (SELECT gis_screen_id FROM GIS_Screen WHERE gis_data_model_id=@gis_data_model_id)
DELETE GIS_Screen WHERE gis_data_model_id=@gis_data_model_id
DELETE GIS_Property WHERE gis_object_id IN (SELECT gis_object_id from GIS_Object WHERE gis_data_model_id=@gis_data_model_id)
DELETE GIS_Object WHERE gis_data_model_id=@gis_data_model_id
DELETE GIS_QEM_Usage WHERE gis_data_model_id=@gis_data_model_id
DELETE GIS_Data_Model_Business WHERE gis_data_model_id=@gis_data_model_id
DELETE GIS_Data_Model WHERE code=@gis_data_model_code
END

print 'Completed'