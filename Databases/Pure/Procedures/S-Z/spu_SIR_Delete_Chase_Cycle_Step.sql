  SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SIR_Delete_Chase_Cycle_Step'
GO


/*************************************************************************/  
/*Description: Delete record from Chase_Cycle_Step table on the bas of  Chase_Cycle_Step_id              */  
/* DATE:-06/03/2013                 */  
/*************************************************************************/  
create PROCEDURE spu_SIR_Delete_Chase_Cycle_Step    
    @Chase_Cycle_Step_id INT    
AS    
    
BEGIN    
    
    DELETE    
      FROM Chase_Cycle_Step    
     WHERE Chase_Cycle_Step_id = @Chase_Cycle_Step_id    
    
END    
  go