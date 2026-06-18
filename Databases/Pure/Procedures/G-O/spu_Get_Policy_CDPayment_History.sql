  
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
EXEC DDLDropProcedure 'spu_Get_Policy_CDPayment_History'
GO
--Start - Renuka - (WPR85_Cash_Deposit_Process)  
CREATE  PROCEDURE spu_Get_Policy_CDPayment_History  
 @Party_Cnt INT,  
 @Insurance_Folder_Cnt INT   
AS   
BEGIN  
  
 DECLARE @ReceiptDocumentType_ID AS INT  
 SELECT   
  @ReceiptDocumentType_ID=DocumentType_ID  
 FROM   
  DocumentType  
 WHERE  
  CODE='SRP'  
  
   
 SELECT DISTINCT  
  CDT.CashDeposit_ID,  
  CDT.Account_ID,  
  CDT.Party_ID,  
  CDT.CashDeposit_Ref,  
  (SELECT  
    SUM(amount)*-1  
   FROM   
    TransDetail TDT  
    INNER JOIN Document DOC  
     ON DOC.Document_ID=TDT.Document_ID  
   WHERE  
    TDT.Account_ID=CDT.Account_ID  
    AND DOC.DocumentType_ID=@ReceiptDocumentType_ID  
  )AS Amount,  
  (SELECT  
   SUM(outstanding_amount)*-1  
   FROM   
    TransDetail TDT  
    INNER JOIN Document DOC  
        ON DOC.Document_ID=TDT.Document_ID  
   WHERE  
    TDT.Account_ID=CDT.Account_ID  
    AND DOC.DocumentType_ID=@ReceiptDocumentType_ID  
  ) AS Available_Balance,  
  CDT.Date_Created  
 FROM   
  CashDeposit CDT  
  INNER JOIN CashDeposit_Policy_Link CPL  
   ON CPL.CashDeposit_ID=CDT.CashDeposit_ID  
 WHERE  
  CDT.Party_ID=@Party_Cnt  
  AND CDT.Is_Deleted<>1  
  AND CPL.Insurance_File_Cnt IN (  
          SELECT   
           Insurance_File_Cnt  
          FROM  
           Insurance_File  
          WHERE  
           Insurance_Folder_Cnt=@Insurance_Folder_Cnt  
           )  
            
    
END  
--End - Renuka - (WPR85_Cash_Deposit_Process) 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
