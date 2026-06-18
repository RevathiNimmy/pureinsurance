SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Party_Details_Select'
GO

CREATE PROCEDURE spu_SIR_Party_Details_Select    
    
@party_cnt int    
    
AS    
    
BEGIN    
 SELECT    
  Tax_Number,    
  Domiciled_for_Tax,    
  Tax_Exempt,    
  Tax_Percentage,   
  blacklist_reason_id  
 FROM Party    
 WHERE Party_Cnt = @party_cnt    
END    


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
