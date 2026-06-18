SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXEC DDLDropProcedure 'spu_Update_Search_Transaction_Selected_Column'
GO   
CREATE PROCEDURE spu_Update_Search_Transaction_Selected_Column     
 @sUserName VARCHAR(255),      
 @bTransDetailKeys BOOLEAN,     
 @bBranchKey BOOLEAN,     
 @bBalanceType BOOLEAN,    
 @bAccount BOOLEAN,     
 @bDocRef BOOLEAN,    
 @bAltRef BOOLEAN ,     
 @bEffectiveDate BOOLEAN,     
 @bTransDate BOOLEAN,    
 @bDueDate BOOLEAN,    
 @bMediaType BOOLEAN,     
 @bAccountAmount_CurrencyAmount BOOLEAN,    
 @bPrimarySettled BOOLEAN,     
 @bOutstandingAmount BOOLEAN,    
 @bPaidDate BOOLEAN,     
 @bDocumentTypeCode BOOLEAN,     
 @bReference BOOLEAN,     
 @bOperatorName BOOLEAN,     
 @bPeriod BOOLEAN,     
 @bDocTypeGroupCode BOOLEAN,     
 @bClient BOOLEAN,     
 @bClientCode BOOLEAN,     
 @bMediaRef BOOLEAN,    
 @bAccountKey BOOLEAN,     
 @bPayeeName BOOLEAN,     
 @bUnderwritingYear BOOLEAN,     
 @bAccountOutStandingAmount BOOLEAN,     
 @bCurrencyAmount BOOLEAN,     
 @bOutStandingCurrencyAmount BOOLEAN,    
 @bBGRef BOOLEAN  ,  
 @bCashListkey BOOLEAN,  
 @bLeadagent BOOLEAN,  
 @bSplitReceipt BOOLEAN 
     
AS      
    
BEGIN      
        
 IF EXISTS (SELECT UserName FROM Search_Transaction_Selected_Column WHERE UserName= @sUserName)    
 BEGIN        
 UPDATE Search_Transaction_Selected_Column     
   SET     
   TransDetailKeys = @bTransDetailKeys,     
   BranchKey = @bBranchKey,     
   BalanceType = @bBalanceType,    
   Account = @bAccount,     
   DocRef = @bDocRef,    
   AltRef = @bAltRef,    
   EffectiveDate = @bEffectiveDate,    
   TransDate = @bTransDate,    
   DueDate = @bDueDate,    
   MediaType = @bMediaType,    
   AccountAmount_CurrencyAmount = @bAccountAmount_CurrencyAmount,    
   PrimarySettled = @bPrimarySettled,    
   OutstandingAmount = @bOutstandingAmount,    
   PaidDate = @bPaidDate,    
   DocumentTypeCode = @bDocumentTypeCode,    
   Reference = @bReference,    
   OperatorName = @bOperatorName,    
   Period = @bPeriod,    
   DocTypeGroupCode = @bDocTypeGroupCode,    
   Client = @bClient,    
   ClientCode = @bClientCode,    
   MediaRef = @bMediaRef,    
   AccountKey = @bAccountKey,    
   PayeeName = @bPayeeName,    
   UnderwritingYear = @bUnderwritingYear,    
   AccountOutStandingAmount = @bAccountOutStandingAmount,    
   CurrencyAmount = @bCurrencyAmount,    
   OutStandingCurrencyAmount = @bOutStandingCurrencyAmount,    
   BGRef = @bBGRef,  
   CashListkey = @bCashListkey,  
   IsLeadagent = @bLeadagent,  
   IsSplitReceipt = @bSplitReceipt  
     WHERE UserName = @sUserName      
       
   END    
    ELSE    
 BEGIN    
   INSERT INTO Search_Transaction_Selected_Column   
   (
    UserName, 
		TransDetailKeys, 
		BranchKey, 
		BalanceType,
		Account, 
		DocRef,
    AltRef, 
		EffectiveDate,
		TransDate, 
		DueDate, 
		MediaType, 
		AccountAmount_CurrencyAmount,
		PrimarySettled,
    OutstandingAmount, 
		PaidDate, 
    DocumentTypeCode,
		Reference, 
		OperatorName, 
		Period, 
		DocTypeGroupCode, 
		Client, 
		ClientCode, 
		MediaRef,
    AccountKey,
		PayeeName, 
		UnderwritingYear, 
		AccountOutStandingAmount, 
		CurrencyAmount, 
		OutStandingCurrencyAmount,
    BGRef, 
		CashListkey, 
		IsLeadagent, 
		IsSplitReceipt
    )     
  VALUES(@sUserName,      
 @bTransDetailKeys,     
 @bBranchKey ,     
 @bBalanceType ,    
 @bAccount,     
 @bDocRef ,    
 @bAltRef ,     
 @bEffectiveDate ,     
 @bTransDate ,    
 @bDueDate ,    
 @bMediaType ,     
 @bAccountAmount_CurrencyAmount ,    
 @bPrimarySettled ,     
 @bOutstandingAmount ,    
 @bPaidDate ,     
 @bDocumentTypeCode ,     
 @bReference ,     
 @bOperatorName ,     
 @bPeriod ,     
 @bDocTypeGroupCode ,     
 @bClient ,     
 @bClientCode ,     
 @bMediaRef ,    
 @bAccountKey ,     
 @bPayeeName ,     
 @bUnderwritingYear ,     
 @bAccountOutStandingAmount ,     
 @bCurrencyAmount ,     
 @boutStandingCurrencyAmount ,    
 @bBGRef,  
 @bCashListkey,  
 @bLeadagent,  
 @bSplitReceipt  
      )    
 END    
    
END    