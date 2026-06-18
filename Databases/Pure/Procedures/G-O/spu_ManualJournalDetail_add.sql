 
 SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_ManualJournalDetail_add'
GO

 
CREATE PROCEDURE spu_ManualJournalDetail_add    
    
	 @ManualJournalDetail_Id int OUTPUT,      
 @ManualJournal_id int,     
 @Account_id int,    
 @Amount numeric(19,2),    
 @Currency_id int,    
 @Currency_Rate numeric(19,2),    
 @Base_Amount numeric(19,2),    
 @Alternate_ref varchar(50),    
 @Comment varchar(500),    
 @UnderwritingYear_id int,         
 @CostCenterId int,     
 @Insurance_ref varchar(30),    
 @Purchase_order_No varchar(40),    
 @Purchase_Invoice_No varchar(40)    
      
AS      
     
BEGIN     
      
INSERT INTO ManualJournalDetail      
(      
 ManualJournal_id ,     
 Account_id ,    
 Amount ,    
 Currency_id ,    
 Currency_Rate ,    
 Base_Amount ,    
 Alternate_ref ,    
 Comment ,    
 UnderwritingYear_id ,         
 CostCenterId ,     
 Insurance_ref ,    
 Purchase_order_No ,    
 Purchase_Invoice_No     
)      
VALUES      
(      
 @ManualJournal_id ,     
 @Account_id ,    
 @Amount ,    
 @Currency_id ,    
 @Currency_Rate ,    
 @Base_Amount ,    
 @Alternate_ref ,    
 @Comment ,    
 @UnderwritingYear_id ,         
 @CostCenterId ,     
 @Insurance_ref ,    
 @Purchase_order_No ,    
 @Purchase_Invoice_No     
      
)    
SELECT @ManualJournalDetail_Id = SCOPE_IDENTITY() 
END  
GO

