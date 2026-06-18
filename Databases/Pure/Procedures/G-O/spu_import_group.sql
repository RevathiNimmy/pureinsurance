EXECUTE DDLDropProcedure 'spu_import_group'
GO


SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_import_group

@gis_list_type_id int,
@gis_Scheme_ID int,
@group_code varchar(20),
@ABICode varchar(20)

AS


DECLARE @Group_id int

-- CTAF 100602 - Added gis_list_type_id
SELECT @Group_id =  gis_list_grouping_id 
FROM gis_list_grouping
WHERE code = @Group_Code
AND gis_scheme_id=@gis_scheme_id
AND gis_list_type_id=@gis_list_type_id

IF @Group_id is Null
BEGIN
	INSERT INTO gis_list_grouping(gis_scheme_id, gis_list_type_id, code, is_deleted, description)
	VALUES (@gis_Scheme_ID,@gis_list_type_id,@Group_Code,0,@Group_Code)

	SELECT @Group_id = @@IDENTITY
END

INSERT INTO gis_list_grouping_items (gis_list_grouping_id, gis_scheme_id, gis_list_items_id)
SELECT @group_id, @gis_scheme_id, gli.gis_list_items_id
FROM gis_list_items gli
INNER JOIN gis_list_type_usage  gltu
ON gli.gis_list_items_id=gltu.gis_list_items_id
WHERE code = @ABICode
AND gltu.gis_list_type_id=@gis_list_type_id
AND gltu.effective_date <= GetDate()

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO


