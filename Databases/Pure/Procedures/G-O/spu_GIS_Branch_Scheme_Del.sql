-- AK22072004: Create spu_GIS_Branch_Scheme_Sel.sql
-- Deletes branch usage data for the selected scheme

SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_GIS_Branch_Scheme_Del'
GO

CREATE PROCEDURE spu_GIS_Branch_Scheme_Del
    @SourceID integer,
    @SchemeID integer
AS

-- If the record exists...
IF EXISTS (SELECT source_id 
             FROM GIS_Branch_Scheme 
            WHERE source_id = @SourceID 
              AND gis_scheme_id = @SchemeID) BEGIN

    -- ...delete it
    DELETE FROM GIS_Branch_Scheme
          WHERE source_id = @SourceID
            AND gis_scheme_id = @SchemeID
END
GO

