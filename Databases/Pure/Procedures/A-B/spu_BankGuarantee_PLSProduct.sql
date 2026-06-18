SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_BankGuarantee_PLSProduct'
GO 
CREATE  PROCEDURE spu_BankGuarantee_PLSProduct   
@BG_Id  INT,  
@Product_Id  INT  
AS    
  
INSERT INTO    
    Bg_Product_Link (Bg_Id, Product_Id)    
VALUES    
    (@BG_Id, @Product_Id) 

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO                
