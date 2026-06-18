SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Party_Details_Update'
GO

CREATE PROCEDURE spu_SIR_Party_Details_Update    
    
@party_cnt int,    
@tax_number varchar(50),    
@domiciled_for_tax tinyint,    
@tax_exempt tinyint,    
@tax_percentage float,   
@blacklist_reason_id int  
    
AS    
    
BEGIN    
 UPDATE Party    
 SET Tax_Number = @Tax_Number,    
  Domiciled_for_Tax = @Domiciled_For_Tax,    
  Tax_Exempt = @Tax_Exempt,    
  Tax_Percentage = @Tax_Percentage,  
  BlackList_Reason_id = @blacklist_reason_id  
 WHERE Party_Cnt = @party_cnt    
END    


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
