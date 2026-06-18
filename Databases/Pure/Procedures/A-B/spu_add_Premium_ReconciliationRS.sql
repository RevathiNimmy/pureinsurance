   
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

EXECUTE DDLDropProcedure 'spu_add_Premium_ReconciliationRS'
GO
  
  
CREATE PROCEDURE spu_add_Premium_ReconciliationRS    
        
   @Auto_ReconciliationRS_ID Int,    
   @batch_id Int,    
   @Reconciliation_ID Varchar(40),    
   @Agent_Group_Code Varchar(40),    
   @Agent_Group_Name Varchar(100),    
   @Agent_Account_Ref Varchar(40),      
   @Payment_Reference_Number Varchar(40),   
   @Premium_ReconciliationRS_ID int output    
 As    
      
   insert into Premium_ReconciliationRS    
   (    
     Auto_ReconciliationRS_ID,    
  batch_id,    
  Reconciliation_ID,    
  Agent_Group_Code,    
  Agent_Group_Name,    
  Agent_Account_Ref,    
  Payment_Reference_Number  
   )    
   values    
   (    
     @Auto_ReconciliationRS_ID,    
  @batch_id,    
  @Reconciliation_ID,    
  @Agent_Group_Code,    
  @Agent_Group_Name,    
  @Agent_Account_Ref,    
  @Payment_Reference_Number    
   )    
    
    
SELECT @Premium_ReconciliationRS_ID = @@IDENTITY    

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO