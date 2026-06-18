SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_SelAll_Chase_Cycle_Item'
GO
--**************************************************************************
--Created Date:-01/03/2013
--Details:- Select all details from Chase cycle item
--Input Parameter:N/A
--Output Parameter:-Chase_Cycle_item_id, Chase_Cycle_reason,    insurance_file_cnt,     insurance_folder_cnt,    
                   -- can_auto_cancel,   will_auto_cancel,  Chase_Cycle_step_id,  created_date,   due_date,    
                  --    letter_sent,    NULL as next_step_id,   pmuser_group_id, pmuser_id, is_deleted    
                  
--*************************************************************************
CREATE PROCEDURE spu_SIR_SelAll_Chase_Cycle_Item    
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
  ORDER BY Chase_Cycle_item_id    
    
END    
go