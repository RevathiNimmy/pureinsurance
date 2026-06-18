SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_PolSchProp_add'
GO


CREATE PROCEDURE spu_GIS_PolSchProp_add
    @gis_scheme_id int,
    @required_pre int,
    @required_post int,
    @polaris_property_id int
AS
DECLARE @gis_property_id int
DECLARE @gis_object_id int
DECLARE @object_name varchar(70),
        @property_name varchar(70)

IF EXISTS (SELECT gis_property_id FROM gis_property
    WHERE polaris_property_id = @polaris_property_id)
    BEGIN
    SELECT @gis_property_id = p.gis_property_id,
           @property_name = p.property_name,
           @gis_object_id = p.gis_object_id,
           @object_name = o.object_name
    FROM   gis_property p,
           gis_object o
    WHERE  p.polaris_property_id = @polaris_property_id
    AND    p.gis_object_id = o.gis_object_id

    INSERT INTO GIS_Scheme_Property
        (property_name, object_name ,gis_scheme_id,required_pre,required_post)
    VALUES
        (@property_name, @object_name, @gis_scheme_id, @required_pre, @required_post)

   /* For debugging
    --SELECT @@gis_property_id
    --SELECT @@gis_object_id
   */
    END
GO


