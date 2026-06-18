SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_GetDefaultBankAccountWithCurrency'
GO
CREATE PROCEDURE spu_SAM_GetDefaultBankAccountWithCurrency  
@nProductId INT=0,
@nMediaTypeId INT=0,
@nCashListTypeId INT=0,
@nSourceId INT=0  

AS  

BEGIN

DECLARE @sSqlSelect VARCHAR(8000)
DECLARE @sSqlWhere VARCHAR(5000)
DECLARE @nCount INT

IF @nProductId <> 0  
        BEGIN 
           SELECT @nCount=COUNT(*) FROM  BankAccount_Default WHERE product_id=@nProductId
           IF @nCount =0
			SELECT @nProductId=0 
        END 

SELECT @sSqlSelect= 'SELECT BAD.bankaccount_default_id BankAccountDefaultID ,
BAD.source_id SourceID,
BAD.cashlisttype_id CashListTypeID,
BAD.bankaccount_id BankAccountID,
BAD.effective_date EffectiveDate,
BAD.description Description,
BAD.code Code,
ISNULL(BAD.mediatype_id,0) MediaTypeID,
ISNULL(BAD.product_id,0) ProductID, 
BA.code BankAccountCode,
ISNULL(CLT.code,''NULL'') CashListTypeCode,
ISNULL(MT.code,''NULL'') MediaTypeCode,
ISNULL(P.Code,''NULL'') ProductCode,
BA.Currency_ID CurrencyID,
C.Code CurrencyCode  
FROM BankAccount_Default BAD 
LEFT JOIN CashListType CLT ON BAD.cashlisttype_id =CLT.cashlisttype_id 
LEFT JOIN MediaType MT ON BAD.mediatype_id =MT.mediatype_id 
LEFT JOIN PRODUCT P ON BAD.product_id =P.Product_ID 
LEFT JOIN BankAccount BA ON BAD.bankaccount_id =BA.bankaccount_id 
LEFT JOIN Currency C ON BA.Currency_ID=C.Currency_Id '
 
 SELECT @sSqlWhere  = 'WHERE BAD.is_deleted <> 1 AND BAD.effective_date <= GetDate() '
  SELECT @sSqlSelect = @sSqlSelect + @sSqlWhere
  IF @nProductId <> 0  
        BEGIN 
       
            SELECT @sSqlSelect = @sSqlSelect +' AND BAD.product_id= '  + CONVERT(VARCHAR,@nProductId)
        END 
            
 IF @nMediaTypeId <> 0  
        BEGIN 
            SELECT @sSqlSelect = @sSqlSelect +' AND BAD.MediaType_Id= '  + CONVERT(VARCHAR,@nMediaTypeId)
        END  
           
  IF @nCashListTypeId <> 0  
        BEGIN 
           SELECT @sSqlSelect = @sSqlSelect +' AND BAD.CashListType_ID= '  + CONVERT(VARCHAR,@nCashListTypeId)
        END     

  IF @nSourceId <> 0  
        BEGIN 
           SELECT @sSqlSelect = @sSqlSelect +' AND BAD.source_id= '  + CONVERT(VARCHAR,@nSourceId)
        END     

  EXEC (@sSqlSelect)  

END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

