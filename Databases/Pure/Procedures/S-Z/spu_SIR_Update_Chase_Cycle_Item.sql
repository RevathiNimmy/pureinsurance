GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SIR_Update_Chase_Cycle_Item'
GO
--*****************************************************************************************************
--I/P Param:- @Chase_Cycle_item_id , @Chase_Cycle_reason,  @insurance_folder_cnt, @insurance_file_cnt,@can_auto_cancel ,    
      --       @will_auto_cancel ,   @Chase_Cycle_step_id , @created_date ,    @due_date ,  @letter_sent ,    
     --        @pmuser_group_id ,    @pmuser_id ,    @is_deleted 
--O/P Param :- N/A
--DESCRIPTION:- It will update  Chase_Cycle_Item table  
--Date:- 04/03/2013 
--*****************************************************************************************************
create PROCEDURE spu_SIR_Update_Chase_Cycle_Item    
    @Chase_Cycle_item_id INT,    
    @Chase_Cycle_reason VARCHAR(50),    
    @insurance_folder_cnt INT,    
    @insurance_file_cnt INT,    
    @can_auto_cancel TINYINT,    
    @will_auto_cancel TINYINT,    
    @Chase_Cycle_step_id INT,    
    @created_date DATETIME,    
    @due_date DATETIME,    
    @letter_sent TINYINT,    
 @pmuser_group_id INT,    
 @pmuser_id INT,    
 @is_deleted tinyint  
AS    
    
BEGIN    
    
    UPDATE Chase_Cycle_Item    
       SET Chase_Cycle_reason = @Chase_Cycle_reason,    
           insurance_folder_cnt = @insurance_folder_cnt,    
           insurance_file_cnt = @insurance_file_cnt,    
           can_auto_cancel = @can_auto_cancel,    
           will_auto_cancel = @will_auto_cancel,    
           Chase_Cycle_step_id = @Chase_Cycle_step_id,    
           created_date = @created_date,    
           due_date = @due_date,    
           letter_sent = @letter_sent,    
   pmuser_group_id=@pmuser_group_id ,    
   pmuser_id=@pmuser_id ,    
   is_deleted=@is_deleted   
     WHERE Chase_Cycle_item_id = @Chase_Cycle_item_id    
    
END 
go