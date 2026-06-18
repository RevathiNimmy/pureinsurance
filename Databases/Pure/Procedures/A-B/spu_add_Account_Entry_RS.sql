
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

EXECUTE DDLDropProcedure 'spu_add_Account_Entry_RS'
GO

  
CREATE PROCEDURE spu_add_Account_Entry_RS  
     
   @Premium_ReconciliationRS_ID Int,  
   @batch_id Int,  
   @Reference_Number VARCHAR(80),  
   @Effective_Date DateTime,  
   @Client_Name Varchar(255),  
   @Policy_Number varchar(40),  
   @Gross_Amount_Due NUMERIC(20,4),  
   @Commission_Due NUMERIC(20,4),  
   @Net_Amount_Due NUMERIC(20,4),  
   @Gross_Amount_Paid NUMERIC(20,4),  
   @Commission_Paid NUMERIC(20,4),  
   @Net_Amount_Paid NUMERIC(20,4),  
   @Revenue_Type Varchar(100),  
   @Posted_Date DateTime,  
   @Premium_Finance_Transaction TINYINT,  
   @Transaction_Status Varchar(2),  
   @Allocation_ID int,   
   @Comments VARCHAR(255)  
  
 As   
 insert into Account_Entry_RS  
 (  
 Premium_ReconciliationRS_ID ,  
  batch_id,  
   ACC_Reference_Number,  
   Effective_Date,  
   Client_Name,  
   Policy_Number,  
   Gross_Amount_Due,  
   Commission_Due,  
   Net_Amount_Due,  
   Gross_Amount_Paid,  
   Commission_Paid,  
   Net_Amount_Paid,  
   Revenue_Type,  
   Posted_Date,  
   Premium_Finance_Transaction,  
   Transaction_Status,     
   Allocation_ID,  
   Comments  
 )   
 values  
 (   
 @Premium_ReconciliationRS_ID,  
   @batch_id,  
   @Reference_Number,  
   @Effective_Date,  
   @Client_Name,  
   @Policy_Number,  
   @Gross_Amount_Due,  
   @Commission_Due,  
   @Net_Amount_Due,  
   @Gross_Amount_Paid,  
   @Commission_Paid,  
   @Net_Amount_Paid,  
   @Revenue_Type,  
   @Posted_Date,  
   @Premium_Finance_Transaction,  
   @Transaction_Status,  
   @Allocation_ID,   
   @Comments  
 )  
   
 GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO








