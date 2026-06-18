SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_GetLookupDetail
GO

CREATE PROCEDURE spu_GetLookupDetail
    @lookup_key int,  
    @line_key int  
AS  
BEGIN
  
    SELECT  GLD.lookup_key,
            GLD.line_key,  
            GLD.key_level,  
            GLD.value,  
            GLD.type,  
            GLH.lookup_name,  
            GLH.effective_date,  
            GLH.status,  
            GLH.definition,  
            GLH.valid_constants,  
            GLH.default_value
    FROM    Gis_Lookup_Data GLD
        LEFT OUTER JOIN Gis_Lookup_Header GLH
            ON GLD.lookup_key = GLH.lookup_key  
    WHERE   GLD.lookup_key = @lookup_key  
        AND GLD.line_key = @line_key  

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO