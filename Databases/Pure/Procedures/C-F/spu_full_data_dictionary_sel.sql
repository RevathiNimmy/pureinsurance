SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_full_data_dictionary_sel'
GO


CREATE PROCEDURE spu_full_data_dictionary_sel
    @GIS_screen_id INT ,
    @GIS_data_model_type_id INT = NULL
AS

--***********************************************************************************
-- Recursively get all screen ids and put them into a temporary table
declare @child_level integer
SET NOCOUNT ON 
create table #TempID (gis_screen_id integer, [level] integer)
create clustered index NUPK on #TempID (gis_screen_id, [level]) --this reduces execution of the select time by 30%!
set @child_level = 0
insert into #TempID values (@GIS_screen_id, @child_level) -- make sure the parent screen is in the list
 
while @@rowcount > 0 begin
    set @child_level = @child_level + 1
    insert into #TempID
        select GIS_Screen.gis_screen_id, @child_level
        from GIS_Screen
        where exists (select null from #TempID as TempID where TempID.[level] = @child_level - 1 and TempID.gis_screen_id = GIS_Screen.parent_id)
end
 
--***********************************************************************************
-- Having got the screen ids get the object ids referenced by the screen's screen 
-- details and put them into a temporary table
create table #TempID2 (gis_object_id integer)
insert into #TempID2
	select distinct GIS_Screen_Detail.gis_object_id
	    from GIS_Screen_Detail
	    where GIS_Screen_Detail.gis_object_id is not null
	    and exists (select null from #TempID as TempID where TempID.gis_screen_id = GIS_Screen_Detail.gis_screen_id)

--****************************************************************************************

-- This is basically the orinal sp with the addition of the last AND clause 
DECLARE @GIS_data_model_id INT

SELECT  @GIS_data_model_id = (
 SELECT TOP 1 O.gis_data_model_id  
    FROM    GIS_object O  
        JOIN  GIS_screen_detail D ON D.gis_object_id = O.gis_object_id  
    WHERE   D.gis_screen_id = @GIS_screen_id  
    /*SELECT  DISTINCT (O.GIS_data_model_id)
    FROM    GIS_object O,
        GIS_screen_detail D
    WHERE   D.gis_screen_id = @GIS_screen_id
    AND D.gis_object_id = O.gis_object_id*/
    )
SELECT  O.gis_object_id,
    O.gis_data_model_id,
    O.object_name,
    O.table_name,
    O.max_instances,
    O.is_quote_object,
    O.parent_object_id,
    O.polaris_object_id,
    O.is_selectable_for_screen,
    O.is_non_gis,
    Null,
    Null,
    Null,
    Null,
    Null,
    Null,
    Null,
    Null,
    Null,
    Null,
    Null,
    Null,
    Null,
    Null,
    O.gis_object_id,
    Null
FROM    GIS_object O
WHERE   O.gis_data_model_id = @GIS_data_model_id
AND O.gis_object_id IS NULL
AND O.is_selectable_for_screen = 1
AND EXISTS (select NULL  from #TempID2 where gis_object_id =o.gis_object_id)
UNION
SELECT  O.gis_object_id,
    O.gis_data_model_id,
    O.object_name,
    O.table_name,
    O.max_instances,
    O.is_quote_object,
    O.parent_object_id,
    O.polaris_object_id,
    O.is_selectable_for_screen,
    O.is_non_gis,
    P.gis_property_id,
    P.gis_object_id,
    P.property_name,
    P.column_name,
    P.data_type,
    P.is_input_property,
    P.is_identifying_property,
    P.is_primary_key,
    P.polaris_property_id,
    P.is_deleted,
    P.is_search_property,
    P.Edit_Flags,
    P.Specials_Type,
    P.Specials_Type_Reference,
    O.gis_object_id,
    case P.Specials_Type
	when 6 then (SELECT parent FROM gis_user_def_header WHERE gis_user_def_header_id = convert(int, P.Specials_Type_Reference))
	else null
    end
FROM    GIS_object O,
    GIS_property P
WHERE   O.gis_data_model_id = @GIS_data_model_id
AND O.gis_object_id = P.gis_object_id
AND O.is_selectable_for_screen = 1
AND P.is_primary_key = 0
AND EXISTS (select NULL  from #TempID2 where gis_object_id =o.gis_object_id)
ORDER BY 10, 25, 1, 12, 11

-- Dropt the tempoarary tables
drop table #TempID
drop table #TempID2


GO

