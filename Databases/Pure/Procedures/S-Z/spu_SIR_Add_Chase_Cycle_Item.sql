  GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SIR_Add_Chase_Cycle_Item'
GO
--**************************************************************************
--Created Date:- 01/03/2013
--Description:-It will insert  all chase cycle item table data
--I/p Param:-n/a
--O/P Param:-     Chase_Cycle_reason,insurance_folder_cnt, insurance_file_cnt,  can_auto_cancel, will_auto_cancel, 
                --  Chase_Cycle_step_id,   created_date,  due_date,letter_sent,  pmuser_group_id ,  pmuser_id ,  is_deleted
   
--**************************************************************************
create PROCEDURE spu_SIR_Add_Chase_Cycle_Item    
    @Chase_Cycle_item_id INT OUTPUT,    
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
 --    
AS    
    
BEGIN    
    
    INSERT INTO Chase_Cycle_Item (    
        Chase_Cycle_reason,    
        insurance_folder_cnt,    
        insurance_file_cnt,    
        can_auto_cancel,    
        will_auto_cancel,    
        Chase_Cycle_step_id,    
        created_date,    
        due_date,    
        letter_sent,    
  pmuser_group_id ,    
  pmuser_id ,    
  is_deleted  
)    
    VALUES (    
        @Chase_Cycle_reason,    
        @insurance_folder_cnt,    
        @insurance_file_cnt,    
        @can_auto_cancel,    
        @will_auto_cancel,    
        @Chase_Cycle_step_id,    
        @created_date,    
        @due_date,    
        @letter_sent,    
  @pmuser_group_id ,    
  @pmuser_id ,    
  @is_deleted   
)    
    
END    
    
BEGIN    
    
    SELECT @Chase_Cycle_item_id = @@IDENTITY    
    
END    
go