SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_BankGuarantee_PLSBranch'
GO 
CREATE  PROCEDURE spu_BankGuarantee_PLSBranch      
@BG_Id  INT,      
@Branch_Id      INT      
AS        
        
INSERT INTO      
     
    BG_Branch_Link (Bg_Id,Source_Id)        
VALUES        
    (@BG_Id, @Branch_Id)      
 GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO    
