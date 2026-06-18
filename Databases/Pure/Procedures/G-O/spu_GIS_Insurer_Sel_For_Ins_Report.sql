SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GIS_Insurer_Sel_For_Ins_Report'
GO


CREATE PROCEDURE spu_GIS_Insurer_Sel_For_Ins_Report
AS


SELECT
    description,
    gis_insurer_id
FROM GIS_Insurer
WHERE is_deleted = 0
GO


