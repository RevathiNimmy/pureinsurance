SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Get_Party_AccountsType'
GO


  CREATE PROCEDURE spu_ACT_Get_Party_AccountsType  
    @PartyCnt INT,  
    @AccountID INT = NULL,  
    @BankPaymentTypeCode varchar(255),  
    @IsBank varchar(1) = NULL ,
	@CCExpiryDate  varchar(7) = NULL 
AS  
  
IF @ISBank=''  
    Set @IsBank = NULL  
 
IF @AccountID IS NULL OR @AccountID = 0   
BEGIN  
    Select  PB.Party_bank_id, PB.Account_type  
    FROM Party_Bank PB  
    JOIN bank_payment_type BPT   
    ON PB.bank_payment_type_id = BPT.bank_payment_type_id      
WHERE  Account_ID  =( Select Account_id FROM  Account Where Account_key= @PartyCnt)  
    AND ( BPT.Code = @BankPaymentTypeCode OR BPT.Code = 'ANY')  
    AND ( PB.is_Bank = @IsBank OR @IsBank  IS NULL)
	AND PB.is_deleted = 0 
    AND PB.Account_Type IS NOT NULL and RTRIM(PB.Account_Type)<>''
    AND (
		(ISNULL(@CCExpiryDate,'')='' OR ISNULL(cc_expiry_date,'')='')
		OR  (
			ISNULL(@CCExpiryDate,'')<>'' 
			AND (
				(RIGHT(cc_expiry_date,2) < RIGHT(@CCExpiryDate,2) )
				OR (RIGHT(cc_expiry_date,2) =RIGHT(@CCExpiryDate,2) AND LEFT(cc_expiry_date,2) <= LEFT(@CCExpiryDate,2)) 
				)
			)
		)
 END                   
ELSE  
BEGIN  
    SELECT  PB.Party_bank_id, PB.Account_type  
    FROM Party_Bank PB  
        JOIN bank_payment_type BPT   
        ON PB.bank_payment_type_id = BPT.bank_payment_type_id      
    WHERE  Account_ID  = @AccountID  
        AND ( BPT.Code = @BankPaymentTypeCode OR BPT.Code = 'ANY')  
        AND ( PB.is_Bank = @IsBank OR @IsBank  IS NULL)
		AND PB.is_deleted = 0  
        AND PB.Account_Type IS NOT NULL and RTRIM(PB.Account_Type)<>''
		 AND (
		(ISNULL(@CCExpiryDate,'')='' OR ISNULL(cc_expiry_date,'')='')
		OR  (
			ISNULL(@CCExpiryDate,'')<>'' 
			AND (
				(RIGHT(cc_expiry_date,2)< RIGHT(@CCExpiryDate,2) )
				OR (RIGHT(cc_expiry_date,2) =RIGHT(@CCExpiryDate,2) AND LEFT(cc_expiry_date,2) <= LEFT(@CCExpiryDate,2)) 
				)
			)
		)
END  
  
  

