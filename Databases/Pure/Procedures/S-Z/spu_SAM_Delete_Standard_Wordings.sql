SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SAM_Delete_Standard_Wording'
GO
/*********************************************************************************************************/
/* spu_SAM_Delete_Standard_Wording - Deletes the Standard_Wording entries for a given property 		 */
/*                                                                                                       */
/* RDT 7/6/2007                                                                                          */
/*********************************************************************************************************/
CREATE PROCEDURE spu_SAM_Delete_Standard_Wording
    @gis_datamodel_code varchar(30),
    @gis_policy_binder_id int,
    @gis_object_name varchar(255),
    @gis_property_name varchar(255),
    @parent_key_name varchar(255) = NULL,
    @parent_key_value varchar(255) = NULL

AS
BEGIN

DECLARE @SQL varchar(5000)
DECLARE @gis_object_id int
DECLARE @gis_property_id int

SELECT @gis_datamodel_code = RTRIM(@gis_datamodel_code)
SELECT @gis_property_name = RTRIM(@gis_property_name)
SELECT @gis_object_name = RTRIM(@gis_object_name)

SELECT @gis_object_id = GIS_Object.gis_object_id,
       @gis_property_id = GIS_Property.gis_property_id
FROM
	GIS_Property
INNER JOIN GIS_Object ON GIS_Property.gis_object_id = GIS_Object.gis_object_id
INNER JOIN GIS_Data_Model ON GIS_Object.gis_data_model_id = GIS_Data_Model.gis_data_model_id
WHERE GIS_Object.object_name = @gis_object_name
	AND GIS_Data_Model.code = @gis_datamodel_code
	AND GIS_Property.property_name = @gis_property_name

SELECT @SQL = 'DELETE FROM ' + @gis_datamodel_code + '_standard_wording '
SELECT @SQL = @SQL + 'WHERE ' + @gis_datamodel_code + '_policy_binder_id = ' + convert(varchar(10),@gis_policy_binder_id) + ' '
SELECT @SQL = @SQL + 'AND gis_property_id = ' + convert(varchar(10),@gis_property_id) + ' AND gis_object_id = ' + convert(varchar(10),@gis_object_id) + ' '
IF (@parent_key_name IS NULL) OR (@parent_key_name = '')
BEGIN
	SELECT @SQL = @SQL + 'AND (child IS NULL OR child = 0)'
END
ELSE
BEGIN
	SELECT @SQL = @SQL + 'AND ' + @parent_key_name + ' = ' + @parent_key_value + ' AND child = 1'
END

/* This will produce a statement of the following structure :-
DELETE FROM
	QBENZ_standard_wording
WHERE
	QBENZ_policy_binder_id = 1673
	AND gis_property_id = 678
	AND gis_object_id = 64
	AND (child IS NULL OR child = 0)
*/

exec(@SQL)
--SELECT @SQL

END
