EXEC DDLDropProcedure 'spu_Copy_Standard_Wording'
GO

CREATE PROCEDURE spu_Copy_Standard_Wording
	@data_model AS VARCHAR(50),
	@new_policy_link INT,
	@old_policy_link INT
AS

DECLARE @additional_fields VARCHAR(2000)
DECLARE @select_fields VARCHAR(2000)
DECLARE @field_name VARCHAR(50)
DECLARE @SQL VARCHAR(2500)

SELECT @additional_fields=''
SELECT @select_fields=''

-- Work out additional fields in table
DECLARE AdditionalFields CURSOR FAST_FORWARD FOR
	SELECT sc.name 
	FROM syscolumns sc
	JOIN sysobjects so ON so.id=sc.id
	AND so.name=@data_model+'_Standard_Wording'
	WHERE sc.name NOT IN (@data_model+'_Policy_Binder_id', 'sequence_id',
	'document_template_id', 'gis_property_id', 'gis_object_id', 'child')

OPEN AdditionalFields
FETCH NEXT FROM AdditionalFields INTO @field_name

WHILE @@FETCH_STATUS = 0
BEGIN
	SELECT @additional_fields=@additional_fields+', '+@field_name
	SELECT @select_fields=@select_fields+', s.'+@field_name
	FETCH NEXT FROM AdditionalFields INTO @field_name
END
CLOSE AdditionalFields
DEALLOCATE AdditionalFields

-- Construct the SQL
SELECT @SQL='INSERT INTO '+@data_model+'_standard_wording ('

SELECT @SQL=@SQL+@data_model+'_Policy_binder_id, sequence_id, document_template_id, gis_property_id, gis_object_id, child'
SELECT @SQL=@SQL+@additional_fields+') '

SELECT @SQL=@SQL+'SELECT p1.'+@data_model+'_policy_binder_id, s.sequence_id, s.document_template_id, s.gis_property_id, s.gis_object_id, s.child'+@select_fields
SELECT @SQL=@SQL+' FROM '+@data_model+'_standard_wording s, '+@data_model+'_policy_binder p1, '+@data_model+'_policy_binder p2 '
SELECT @SQL=@SQL+' WHERE p1.gis_policy_link_id = '+ CAST(@new_policy_link AS VARCHAR(50))
SELECT @SQL=@SQL+' AND s.'+@data_model+'_policy_binder_id = p2.'+@data_model+'_policy_binder_id '
SELECT @SQL=@SQL+' AND p2.gis_policy_link_id = '+ CAST(@old_policy_link AS VARCHAR(50))

EXEC (@SQL)
GO
