  
  SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SIR_Delete_Chase_Cycle_Item'
GO


/*************************************************************************/  
/*Description: Delete record from Chase_Cycle_Item table on the basis of  Chase_Cycle_item_id        */  
/* DATE:-06/03/2013                 */  
/*************************************************************************/  
  
CREATE PROCEDURE spu_SIR_Delete_Chase_Cycle_Item    
    @Chase_Cycle_item_id INT,    
    @bDelete_Permanent BIT = 0    
AS    
    
BEGIN    
    
IF (@bDelete_Permanent = 0)    
    UPDATE Chase_Cycle_Item    
    SET is_deleted=1    
    WHERE Chase_Cycle_item_id = @Chase_Cycle_item_id    
ELSE    
    DELETE Chase_Cycle_Item    
    WHERE Chase_Cycle_item_id = @Chase_Cycle_item_id    
END    
  go