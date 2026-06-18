EXEC DDLDropProcedure 'spu_Party_Get_GIS_Screen'
GO

CREATE PROCEDURE spu_Party_Get_GIS_Screen
	@party_cnt INT,
	@gis_screen_id INT OUTPUT
AS

/*
	Return the associated GIS_Screen for the Party Type of the
	Party passed in. 

	If the screen is not set up then return	NULL
	If the wrong screen is used (eg. Risk screen) then return -1
*/

DECLARE @party_type_id INT
DECLARE @data_model_type_id INT

SELECT
	@gis_screen_id=PT.gis_screen_id,
	@party_type_id=P.party_type_id,
	@data_model_type_id=GDM.gis_data_model_type_id
FROM
	Party P
INNER JOIN Party_Type PT ON PT.party_type_id=P.party_type_id
INNER JOIN GIS_Screen GS ON GS.gis_screen_id=PT.gis_screen_id
LEFT JOIN GIS_Data_Model GDM ON GDM.gis_data_model_id=GS.gis_data_model_id
WHERE P.party_cnt=@party_cnt and GS.is_deleted = 0

IF NOT @gis_screen_id IS NULL
	IF ISNULL(@data_model_type_id,0)<>4
		SELECT @gis_screen_id=-1


GO