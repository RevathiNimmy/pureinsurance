EXEC DDLDropProcedure 'spu_Party_Get_GIS_Policy_Link'
GO

CREATE PROCEDURE spu_Party_Get_GIS_Policy_Link
	@party_cnt INT,
	@gis_policy_link_id INT OUTPUT
AS

/*
	Return the associated GIS_Policy_Link for the Party passed in. 
	If the link is not set up then return NULL
*/

DECLARE @party_builder_data_model int

SELECT
	@party_builder_data_model=gis_data_model_type_id
FROM
	gis_data_model_type
WHERE
	code='PARTY'

SELECT
	@gis_policy_link_id=GPL.gis_policy_link_id
FROM
	GIS_Policy_Link GPL
INNER JOIN
	GIS_Data_Model GDM
ON
	GPL.gis_data_model_id=GDM.gis_data_model_id
WHERE
	GPL.party_cnt=@party_cnt
AND	GDM.gis_data_model_type_id=@party_builder_data_model


GO