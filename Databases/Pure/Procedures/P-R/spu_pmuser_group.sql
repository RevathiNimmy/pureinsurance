SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_pmuser_group'
GO

CREATE PROCEDURE  [dbo].[spu_pmuser_group]  
    @Language_Id     INT = NULL ,  
	@username varchar(50)= NULL ,
	@AgentKey INT=0   
AS  
/********************************************************************************************************/  
/* Revision Description of Modification Date Who */  
/* -------- --------------------------- ---- --- */  
/* 1.0 Original 20/08/1997 JW */  
/* 1.1 Added Is Supervisor and Is Sys Admin Group flags 20/11/1997 Tom */  
/********************************************************************************************************/  
SELECT 'group' user_or_group,  
    ug.pmuser_group_id id,  
    ug.caption_id caption_id,  
    ug.code code,  
    --Start (Ravikumar Pasupuleti) - (Tech Spec - UIIC WR01 - User Access - Get User Groups.doc)  
    description=ISNULL(pmc.caption, ug.description) ,  
    ug.is_deleted is_deleted,  
    ug.effective_date effective_date,  
    0 included,  
    0 is_supervisor,  
    ug.is_sys_admin_group is_sys_admin_group, 
	CASE ISNULL(dug.pmuser_group_id,0) WHEN 0 THEN 0 ELSE 1 END Is_debtor_pmuser_group 
   FROM pmuser_group ug  
   LEFT JOIN PMUser_Group_User ugu ON ugu.pmuser_group_id =ug.pmuser_group_id  
   LEFT JOIN PMUser u ON u.user_id = ugu.user_id  
   LEFT JOIN Debtor_User_Groups dug ON dug.pmuser_group_id=ug.pmuser_group_id
   LEFT JOIN  
        pmCaption pmc ON ug.caption_id = pmc.caption_id  
    WHERE  
       ( (@language_id IS NULL AND pmc.language_id = 1)  
        OR  
        (@language_id IS NOT NULL AND pmc.language_id = @Language_Id) )  
  AND  
  (ISNULL(@UserName,'')='' OR(ISNULL(@UserName,'')<>'' AND u.username = @UserName  )) 
 AND (u.party_cnt=@AgentKey OR @AgentKey=0)  
GROUP BY  
        ug.pmuser_group_id  ,  
        ug.caption_id  ,  
        ug.code  ,  
  pmc.caption,  
  ug.description  ,  
  ug.is_deleted  ,  
  ug.effective_date ,  
  ug.is_sys_admin_group,dug.pmuser_group_id
  --End (Ravikumar Pasupuleti) - (Tech Spec - UIIC WR01 - User Access - Get User Groups.doc)  
   ORDER BY 6 ASC, 4 ASC 
  
GO 
SET QUOTED_IDENTIFIER OFF 
GO 
SET ANSI_NULLS ON 
GO

