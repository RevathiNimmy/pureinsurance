SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_pmnav_process_sel
GO

CREATE PROCEDURE spu_pmnav_process_sel
    @pmnav_process_id INTEGER,  
    @language_id SMALLINT  
AS  
  
/*******************************************************************************************************/  
/* sp_pmnav_process_sel selects the details for a specific Navigator Process */  
/*******************************************************************************************************/  
/*******************************************************************************************************/  
/* Revision Description of Modification Date Who */  
/* -------- --------------------------- ---- --- */  
/* 1.0 Original 26/09/1997 RFC */  
/*******************************************************************************************************/  
BEGIN  

    SELECT  
        pmnav_process_id,
        pmp.pmproduct_id,
        np.code code,
        caption,
        tt.code transaction_type,
        process_mode,
        start_nav_map_id,
        is_logged,
        is_user_driven
    FROM    
        pmnav_process np
        INNER JOIN pmproduct pmp
            ON np.pmproduct_id = pmp.pmproduct_id  
        LEFT OUTER JOIN transaction_type tt
            ON np.transaction_type_id = tt.transaction_type_id
        LEFT OUTER JOIN pmcaption pmc  
            ON np.caption_id = pmc.caption_id 
            AND pmc.language_id = @language_id
    WHERE   
        pmnav_process_id = @pmnav_process_id

END  

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO