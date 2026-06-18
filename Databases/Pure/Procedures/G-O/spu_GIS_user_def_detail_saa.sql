SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_GIS_user_def_detail_saa
GO

CREATE PROCEDURE spu_GIS_user_def_detail_saa
    @GIS_user_def_header_id INT  
AS  
BEGIN 
  
/*  
SELECT  
    GIS_user_def_detail_id,  
    GIS_user_def_header_id,  
    caption_id,  
    code,  
    description,  
    is_deleted,  
    effective_date,  
    Parent,  
    gis_user_def_header_inds_id  
 FROM GIS_user_def_detail  
WHERE GIS_user_def_header_id = @GIS_user_def_header_id  
  
ORDER BY GIS_user_def_detail_id ASC  
*/  

    SELECT  
        d1.GIS_user_def_detail_id,
        d1.GIS_user_def_header_id,
        d1.caption_id,
        d1.code,
        d1.description,
        d1.is_deleted,
        d1.effective_date,
        d1.Parent,
        d1.gis_user_def_header_inds_id,
        d2.description,
        NULL
    FROM    
        GIS_user_def_detail d1
        LEFT OUTER JOIN GIS_user_def_detail d2
            ON d1.parent = d2.gis_user_def_detail_id
    WHERE   
        d1.GIS_user_def_header_id = @GIS_user_def_header_id
        AND d1.gis_user_def_header_inds_id  IS NULL
    UNION
    SELECT  
        d1.GIS_user_def_detail_id,
        d1.GIS_user_def_header_id,
        d1.caption_id,
        d1.code,
        d1.description,
        d1.is_deleted,
        d1.effective_date,
        d1.Parent,
        d1.gis_user_def_header_inds_id,
        d2.description,
        hi.description
    FROM    
        GIS_user_def_detail d1
        INNER JOIN GIS_user_def_header_inds hi
            ON d1.GIS_user_def_header_id = hi.GIS_user_def_header_id
            AND d1.gis_user_def_header_inds_id = hi.gis_user_def_header_inds_id
        LEFT OUTER JOIN GIS_user_def_detail d2
            ON d1.parent = d2.gis_user_def_detail_id
    WHERE
        d1.GIS_user_def_header_id = @GIS_user_def_header_id
    ORDER BY 
        d1.GIS_user_def_detail_id ASC

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO