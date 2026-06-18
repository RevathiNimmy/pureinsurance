SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_GIS_CloneSchemeRating'
GO

--Copies rating structure from specified scheme to most recently added scheme
--in use by scheme maintenance 
--See JES for extra help 13/02/2002
--APS 13/01/03	Amended to link items and rates to new list grouping
--CJB 25/02/05  PN16559 Fix bugs in area that copies the matrix data

CREATE   PROCEDURE spu_GIS_CloneSchemeRating
@SchemeID int
AS

--get last schemeid ( last one saved )
declare @LastSchemeID  int
declare @RateTypeID int

select @LastSchemeID = max(gis_scheme_id) from gis_scheme

--groups
INSERT INTO [GIS_List_Grouping]( [gis_scheme_id], [gis_list_type_id], [code], [is_deleted], [description])
SELECT @lastschemeid, [gis_list_type_id], [code], [is_deleted], [description]
FROM gis_list_grouping
WHERE gis_scheme_id=@schemeid

--grouping items
INSERT INTO [GIS_List_Grouping_Items]( [gis_list_grouping_id], [gis_scheme_id], [gis_list_items_id])
SELECT
(select gis_list_grouping_id from gis_list_grouping where code in
    (select code from gis_list_grouping where gis_list_grouping_id = GLGI.gis_list_grouping_id)
    and gis_list_type_id in
          (select gis_list_type_id from gis_list_grouping where gis_list_grouping_id = GLGI.gis_list_grouping_id)
    and gis_scheme_id = @lastschemeid),
@lastschemeid, [gis_list_items_id]
FROM GIS_List_Grouping_Items GLGI
WHERE gis_scheme_id=@schemeid

--rate lookups
INSERT INTO [GIS_Rate_Type]( [description], [gis_scheme_id], [gis_list_type_lookup2], [gis_list_type_lookup1], [gis_list_type_lookup3])
SELECT [description], @lastschemeid, [gis_list_type_lookup2], [gis_list_type_lookup1], [gis_list_type_lookup3]
FROM Gis_rate_type
WHERE gis_scheme_id =@schemeID

--matrix
INSERT INTO [GIS_Rate_Items]([lookup1], [lookup2], [lookup3], [rate], [gis_rate_type_id])

SELECT
(select gis_list_grouping_id from gis_list_grouping g1 where gis_list_type_id =
     (select gis_list_type_lookup1 from gis_rate_type where gis_rate_type_id=t2.gis_rate_type_id) and gis_scheme_id = @lastschemeid and g1.code=g2.code) as 'Lookup1',
(select gis_list_grouping_id from gis_list_grouping g1 where gis_list_type_id =
     (select gis_list_type_lookup2 from gis_rate_type where gis_rate_type_id=t2.gis_rate_type_id) and gis_scheme_id = @lastschemeid and g1.code=g3.code) as 'Lookup2',
(select gis_list_grouping_id from gis_list_grouping g1 where gis_list_type_id =
     (select gis_list_type_lookup3 from gis_rate_type where gis_rate_type_id=t2.gis_rate_type_id) and gis_scheme_id = @lastschemeid and g1.code=g4.code and code =
     	(select code from gis_list_grouping where gis_list_grouping_id = [LOOKUP3])) as 'lookup3',
[rate], t2.gis_rate_type_id from
gis_rate_items i
INNER JOIN gis_rate_type t1
ON i.gis_rate_type_id = t1.gis_rate_type_id

INNER JOIN gis_rate_type t2
ON t1.description=t2.description

INNER JOIN gis_list_grouping g2 
ON i.lookup1=g2.gis_list_grouping_id

LEFT JOIN gis_list_grouping g3 
ON i.lookup2=g3.gis_list_grouping_id

LEFT JOIN gis_list_grouping g4 
ON i.lookup3=g4.gis_list_grouping_id

WHERE t2.gis_scheme_id=@lastschemeid
AND   t1.gis_scheme_id=@schemeID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO