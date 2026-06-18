SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.spu_GIS_CloneSchemeRating    Script Date: 13/02/2002 15:43:32 ******/
EXECUTE DDLDropProcedure 'spu_GIS_CloneSchemeRating'
GO

CREATE PROCEDURE spu_GIS_CloneSchemeRating 
--Copies rating structure from specified scheme to most recently added scheme
--in use by scheme maintenance for CNIC
--See JES for extra help 13/02/2002

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

--grouping
INSERT INTO [GIS_List_Grouping_Items]( [gis_list_grouping_id], [gis_scheme_id], [gis_list_items_id])
SELECT [gis_list_grouping_id], @lastschemeid, [gis_list_items_id]
FROM GIS_List_Grouping_Items
WHERE gis_scheme_id=@schemeid

--rate lookups
INSERT INTO [GIS_Rate_Type]( [description], [gis_scheme_id], [gis_list_type_lookup2], [gis_list_type_lookup1], [gis_list_type_lookup3])
SELECT [description], @lastschemeid, [gis_list_type_lookup2], [gis_list_type_lookup1], [gis_list_type_lookup3] 
FROM Gis_rate_type
WHERE gis_scheme_id =@schemeID

--matrix

INSERT INTO [GIS_Rate_Items]([lookup1], [lookup2], [lookup3], [rate], [gis_rate_type_id])

SELECT [lookup1], [lookup2], [lookup3], [rate], t2.gis_rate_type_id from
gis_rate_items i 
INNER JOIN gis_rate_type t1
ON i.gis_rate_type_id = t1.gis_rate_type_id
INNER JOIN gis_rate_type t2 
ON t1.description=t2.description
WHERE t2.gis_scheme_id=@lastschemeid 
AND t1.gis_scheme_id=@schemeid
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

