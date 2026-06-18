SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXEC DDLDropProcedure 'spu_Get_Search_Transaction_Selected_Column'
GO   
CREATE PROCEDURE spu_Get_Search_Transaction_Selected_Column   
  @sUserName VARCHAR(255)    
  AS    
    
  BEGIN        
   SELECT UserName, 
		TransDetailKeys, 
		BranchKey, 
		BalanceType,
		Account, 
		DocRef,
        AltRef , 
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
 FROM Search_Transaction_Selected_Column WHERE UserName = @sUserName
  END 