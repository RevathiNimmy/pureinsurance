
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SIR_Select_Chase_Cycle_Item'
GO
--**************************************************************
--Created date:-01/03/2013
--Input param:-@Chase_Cycle_item_id
--o/p Param:- Chase_Cycle_item_id,    Chase_Cycle_reason, insurance_file_cnt, insurance_folder_cnt,                   
             -- can_auto_cancel, will_auto_cancel,  Chase_Cycle_step_id, created_date,  due_date,  letter_sent,    
             --NULL as next_step_id, pmuser_group_id, pmuser_id, is_deleted  
   --Description:-It will   select differtent items from Chase_Cycle_Item       
      
 --**************************************************************
create PROCEDURE spu_SIR_Select_Chase_Cycle_Item    
    
    @Chase_Cycle_item_id INT    
AS    
    
BEGIN    
    
    SELECT Chase_Cycle_item_id,    
           Chase_Cycle_reason,    
           insurance_file_cnt,    
           insurance_folder_cnt,    
           can_auto_cancel,    
           will_auto_cancel,    
           Chase_Cycle_step_id,    
           created_date,    
           due_date,    
           letter_sent,    
   NULL as next_step_id,    
   pmuser_group_id,    
   pmuser_id,    
   is_deleted    
      FROM Chase_Cycle_Item    
    
       
     WHERE Chase_Cycle_item_id = @Chase_Cycle_item_id    
    
END    
go