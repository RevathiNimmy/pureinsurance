SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

EXECUTE DDLDropProcedure 'tmp_bd_fields'
GO

create procedure tmp_bd_fields
@cobol_name char(30)

AS
-- declare @cobol_name as char(30)
-- select @cobol_name = "broke-ovr-option"

select @cobol_name
select C.gis_property_id, C.gis_object_id,  G.property_name, O.object_name
 from gis_cobol_linkage as C,
      gis_property as G,
      gis_object as O
 where C.item_name = @cobol_name
   and C.gis_property_id = G.gis_property_id
   and C.gis_object_id = O.gis_object_id
   and linkage_map_id = 1060




GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

