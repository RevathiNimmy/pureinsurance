GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SIR_Chase_Cycle_Item_Update'
GO
--**************************************************************************
--I/P Param:- @Chase_Cycle_item_id,@Chase_Cycle_reason,@Chase_Cycle_step_id,@due_date
--O/P Param:-N/A
--Description:-UPdate Chase_Cycle_item table
--Date:-04/03/2013
--**************************************************************************
CREATE PROCEDURE spu_SIR_Chase_Cycle_Item_Update    
    
@Chase_Cycle_item_id int,    
@Chase_Cycle_reason varchar(50),    
@Chase_Cycle_step_id int,    
@due_date datetime    
    
AS    
    
BEGIN    
    
 UPDATE Chase_Cycle_item    
 SET    
  Chase_Cycle_step_id = @Chase_Cycle_step_id,    
  Chase_Cycle_reason = @Chase_Cycle_reason,    
  due_date = @due_date    
 WHERE Chase_Cycle_item_id =@Chase_Cycle_item_id    
    
END 

go