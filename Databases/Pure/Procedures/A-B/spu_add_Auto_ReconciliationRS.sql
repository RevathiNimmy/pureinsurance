
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

EXECUTE DDLDropProcedure 'spu_add_Auto_ReconciliationRS'
GO
  
CREATE PROCEDURE spu_add_Auto_ReconciliationRS    
    
  @Auto_ReconciliationRS_ID int OUTPUT,    
  @batch_id INT,    
  @InsurerID VARCHAR(50),    
  @InsurerName VARCHAR(100),    
  @DateGenerated DateTime    
    
As    
  insert into Auto_ReconciliationRS    
 (    
  batch_id,    
  InsurerID,    
  InsurerName,    
  DateGenerated    
 )    
 values    
 (    
  @batch_id,    
  @InsurerID,    
  @InsurerName,    
  @DateGenerated    
 )    
    
    
SELECT @Auto_ReconciliationRS_ID = @@IDENTITY 

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


