SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_pmnav_map_sel
GO

CREATE PROCEDURE spu_pmnav_map_sel
    @pmnav_map_id INTEGER,  
    @language_id SMALLINT  
AS  
  
/*******************************************************************************************************/  
/* sp_pmnav_map_sel selects the details for a specific Navigator Map */  
/*******************************************************************************************************/  
/*******************************************************************************************************/  
/* Revision Description of Modification Date Who */  
/* -------- --------------------------- ---- --- */  
/* 1.0 Original 02/09/1998 RFC */  
/*******************************************************************************************************/  
BEGIN  
  
    SELECT
        pmnav_map_id,
        code,
        pmc.caption,
        is_start_map
    FROM    
        pmnav_map pmnm
        LEFT OUTER JOIN pmcaption pmc
        ON pmnm.caption_id = pmc.caption_id
        AND pmc.language_id = @language_id
    WHERE pmnav_map_id = @pmnav_map_id  
  
END  

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO