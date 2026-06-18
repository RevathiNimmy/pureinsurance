SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_GIS_Scheme_EDI_Link_Clone'
GO

CREATE PROCEDURE spu_GIS_Scheme_EDI_Link_Clone
@old_gis_scheme_id int,
@new_gis_scheme_id int

AS

--********************************************************************************************************
--* Stored Procedure spu_GIS_Scheme_EDI_Link_Clone copies any GIS_Scheme_EDI_Link records from one       *
--* scheme and adds them to another that has just been cloned.                                           *
--********************************************************************************************************

INSERT INTO [GIS_Scheme_EDI_Link]( [gis_scheme_id], [external_scheme_no])
  SELECT @new_gis_scheme_id, [external_scheme_no]
  FROM   GIS_Scheme_EDI_Link
  WHERE  gis_scheme_id=@old_gis_scheme_id 


