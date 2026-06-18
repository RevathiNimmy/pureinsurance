/*
    This stored procedure is required for the ASP.NET version of PBASPGenerator
*/
DDLDropProcedure spu_PBGEN_GetChildControl
GO
CREATE PROCEDURE spu_PBGEN_GetChildControl
                @parent_header_id int
AS
BEGIN

     DECLARE @gis_user_def_header_id int

    SELECT @gis_user_def_header_id = @parent_header_id
    
    SELECT  go.object_name, gp.property_name
    FROM    gis_property gp
    INNER JOIN gis_screen_detail gs ON gs.gis_property_id = gp.gis_property_id
    INNER JOIN gis_user_def_header gh ON gh.parent = @parent_header_id
    INNER JOIN gis_object go ON gs.gis_object_id = go.gis_object_id
    WHERE gp.specials_type = 6 
		  AND gp.specials_type_Reference = convert(varchar, gh.gis_user_def_header_id)


END
GO