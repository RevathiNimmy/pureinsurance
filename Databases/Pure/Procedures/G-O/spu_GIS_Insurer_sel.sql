SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GIS_Insurer_sel'
GO


CREATE PROCEDURE spu_GIS_Insurer_sel
    @polaris_insurer_no int,
    @gis_insurer_id int OUTPUT
AS


SELECT @gis_insurer_id = (SELECT gis_insurer_id
    FROM GIS_Insurer
    WHERE polaris_insurer_no = @polaris_insurer_no)

IF @gis_insurer_id IS NULL
    SELECT @gis_insurer_id = 0

SELECT @gis_insurer_id
GO


