SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_BankGuarantee_PLLSource'
GO 
CREATE PROCEDURE spu_BankGuarantee_PLLSource  
 @Bg_Id INT  
AS  
  
  
 SELECT  
  B.Source_id,  
  B.description,  
                CASE  
                WHEN BGBL.source_id IS NULL THEN 0  
                ELSE 1  
    END Chosen  
  
 FROM  
  Source B  
 INNER JOIN  
  BG_Branch_Link BGBL  
  ON  
         BGBL.Source_id = B.Source_id  
  AND BGBL.BG_Id = @Bg_Id  
  
 WHERE B.is_deleted <> 1     

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO    
