SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_pmnav_map_steps_sel
GO

CREATE PROCEDURE spu_pmnav_map_steps_sel
    @pmnav_map_id INTEGER,  
    @language_id SMALLINT  
AS  
  
/*******************************************************************************************************/  
/* sp_pmnav_map_steps_sel selects all of the steps for a specific Map. */  
/*******************************************************************************************************/  
/*******************************************************************************************************/  
/* Revision Description of Modification Date Who */  
/* -------- --------------------------- ---- --- */  
/* 1.0 Original 02/09/1998 RFC */  
/*******************************************************************************************************/  
BEGIN  

    SELECT
        pmnav_map_id,
        ns.pmnav_step_id,
        ns.pmnav_component_id,
        nc.nav_component_type,
        nc.object_name,
        nc.class_name,
        nc.is_server_side,
        ns.sub_nav_map_id,
        ns.task,
        ns.ok_action,
        ns.cancel_action,
        ns.ok_no_of_steps,
        ns.cancel_no_of_steps,
        ns.ok_nav_process_id,
        ns.cancel_nav_process_id,
        ns.navigate_status,
        pmc.caption,
        ns.is_hidden,
        ns.is_logged
    FROM    
        pmnav_step ns
        LEFT OUTER JOIN pmnav_component nc
            ON ns.pmnav_component_id = nc.pmnav_component_id
        LEFT OUTER JOIN pmcaption pmc
            ON ns.caption_id = pmc.caption_id
            AND pmc.language_id = @language_id
    WHERE
        ns.pmnav_map_id = @pmnav_map_id
    ORDER BY
        pmnav_step_id ASC
 
END  

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO