SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_Cobol_Linkage_delall'
GO


CREATE PROCEDURE spu_GIS_Cobol_Linkage_delall
AS


TRUNCATE TABLE GIS_Cobol_Linkage
GO


