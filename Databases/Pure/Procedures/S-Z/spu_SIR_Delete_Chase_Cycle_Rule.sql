  
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SIR_Delete_Chase_Cycle_Rule'
GO


/*************************************************************************/  
/* Description: delete record from Chase_Cycle_Rule table on the basis of Chase_Cycle_rule_id           */  
/*DATE:- 06/03/2013             */  
/*************************************************************************/  
CREATE PROCEDURE spu_SIR_Delete_Chase_Cycle_Rule    
    @Chase_Cycle_rule_id INT    
AS    
    
BEGIN    
    
    DELETE    
      FROM Chase_Cycle_Rule    
     WHERE Chase_Cycle_rule_id = @Chase_Cycle_rule_id    
    
END   
go 