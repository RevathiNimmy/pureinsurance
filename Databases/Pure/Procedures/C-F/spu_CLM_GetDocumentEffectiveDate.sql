SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_CLM_GetDocumentEffectiveDate'
GO


CREATE PROCEDURE spu_CLM_GetDocumentEffectiveDate   
 @claim_id INT,  
 @effective_date DATETIME OUTPUT  
AS  
DECLARE @value int  
SELECT @value= value FROM system_options WHERE option_number = 5030 AND branch_id=1  
--Arul PN56858
If  @value=0 or isnull(@value,0) =0   /*system option is Reported Date*/  
 SELECT @effective_date = reported_date  
 FROM Claim  
 WHERE claim_id = @claim_id  
Else If @value =1  /*system option is System Date*/  
 SELECT @effective_date = GETDATE()  
Else If @value =2  /*system option is Loss Date*/  
 SELECT @effective_date = loss_from_date  
 FROM Claim  
 WHERE claim_id = @claim_id  

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO    
  
