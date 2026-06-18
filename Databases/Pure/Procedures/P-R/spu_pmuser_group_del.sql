SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmuser_group_del'
GO


CREATE PROCEDURE spu_pmuser_group_del
    @pmuser_group_id INT,
    --Start (PraveenGora) - (Tech Spec - UIIC WR01 - User Access - Delete Undelete User Group.doc) - (7.1.5.5)  
    @is_deleted TINYINT = 0  
    --End (PraveenGora) - (Tech Spec - UIIC WR01 - User Access - Delete Undelete User Group.doc) - (7.1.5.5)
AS    
    
/********************************************************************************************************/    
/* Revision Description of Modification Date Who */    
/* -------- --------------------------- ---- --- */    
/* 1.0 Original 20/08/1997 JW */    
/* 2.0 Modify   24/06/2008 PraveenGora */
/********************************************************************************************************/    
--Start (PraveenGora) - (Tech Spec - UIIC WR01 - User Access - Delete Undelete User Group.doc) - (7.1.5.5)
IF(@is_deleted = 1)  
BEGIN  
--End (PraveenGora) - (Tech Spec - UIIC WR01 - User Access - Delete Undelete User Group.doc) - (7.1.5.5)
UPDATE pmuser_group    
    SET is_deleted = 1    
    WHERE pmuser_group_id = @pmuser_group_id 
--Start (PraveenGora) - (Tech Spec - UIIC WR01 - User Access - Delete Undelete User Group.doc) - (7.1.5.5)   
END  
ELSE  
BEGIN  
UPDATE pmuser_group    
    SET is_deleted = 0    
    WHERE pmuser_group_id = @pmuser_group_id    
END  
--End (PraveenGora) - (Tech Spec - UIIC WR01 - User Access - Delete Undelete User Group.doc) - (7.1.5.5)
  
  
GO
 
