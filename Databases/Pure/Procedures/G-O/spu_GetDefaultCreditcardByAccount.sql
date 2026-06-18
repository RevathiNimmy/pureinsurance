SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_GetDefaultCreditcardByAccount'
GO
  
CREATE PROCEDURE spu_GetDefaultCreditcardByAccount  
    @AccountId INT  
AS  
  
BEGIN  
  
SELECT bpt.description,pb.account_type   FROM Party_bank pb (nolock)   
JOIN Bank_Payment_Type bpt ON pb.bank_payment_type_id =bpt.bank_payment_type_id  
WHERE PB.is_bank =0 AND ISNULL(PB.is_default,0)=1 AND PB.account_id=@AccountId  
  
END  
  
GO

