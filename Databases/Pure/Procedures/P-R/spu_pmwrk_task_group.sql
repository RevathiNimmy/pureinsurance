SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_pmwrk_task_group'
GO

CREATE  PROCEDURE spu_pmwrk_task_group  
	@Language_Id 	INT = 1 --If Language ID is not specified then get Caption Id Correponding to UK
AS  
  
/********************************************************************************************************/  
/* Revision Description of Modification Date Who */  
/* -------- --------------------------- ---- --- */  
/* 1.1 Add Task Category 06/10/1999 DAK */  
/* 1.2 Remove Task Category 21/12/1999 DAK */  
/********************************************************************************************************/  
SELECT tg.pmwrk_task_group_id id,  
    tg.caption_id caption_id,  
    tg.code code,  
--Start (Sriram P)-(Tech Spec - UIIC WR01 - User Access - Get Task Groups.doc)-(6)  
    description=isnull(pmc.caption, tg.description),  
--End (Sriram P)-(Tech Spec - UIIC WR01 - User Access - Get Task Groups.doc)-(6)  
    tg.is_deleted is_deleted,  
    tg.effective_date effective_date,  
    tg.display_icon,  
    0 included  
    FROM PMWrk_Task_Group tg  
--Start (Sriram P)-(Tech Spec - UIIC WR01 - User Access - Get Task Groups.doc)-(6)  
    left join  
    pmCaption pmc 
	ON 	tg.caption_id = pmc.caption_id  
	AND	pmc.language_id = @language_id	
--End (Sriram P)-(Tech Spec - UIIC WR01 - User Access - Get Task Groups.doc)-(6)  
    ORDER BY is_deleted ASC, code ASC
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
