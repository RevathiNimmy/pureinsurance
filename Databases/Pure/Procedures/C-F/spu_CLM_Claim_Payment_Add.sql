SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Claim_Payment_Add'
GO

CREATE PROCEDURE spu_CLM_Claim_Payment_Add  
  
@claim_payment_id int OUTPUT,  
@claim_id int,  
@claim_peril_id int,  
@date_of_payment datetime,  
@amount money,  
@tax_amount money,  
@tax_amount_WHT money,  
@party_cnt int,  
@comments varchar(255),
@is_referred tinyint,  
@created_by int,  
@PayeeMediaType int,  
@PayeeName varchar(255),  
@PayeeBankName varchar(255),  
@PayeeSortCode varchar(8),  
@PayeeAccountNo varchar(30),  
@PayeeCountry int,  
@PayeeComments varchar(255),  
@SequenceNo int,  
@treaty_id int,  
@claim_payment_to_id int,  
@payment_party_to tinyint,  
@insured_domiciled tinyint,  
@insured_percentage float,  
@insured_tax_number varchar(50),  
@payee_domiciled tinyint,  
@payee_percentage float,  
@payee_tax_number varchar(50),  
@safe_harbour_id int,  
@safe_harbour_percentage float,  
@is_tax_exempt tinyint,  
@is_wht_exempt tinyint,  
@is_settlement tinyint,  
@document_id int,  
@Media_Ref varchar(100),  
@currency_id smallint,  
@excess_amount money,  
@payeeAddress1 varchar(60),  
@payeeAddress2 varchar(60),  
@payeeAddress3 varchar(60),  
@payeeAddress4 varchar(60),  
@payeePostalCode varchar(20),  
@ThirdPartyReference varchar(30),
@Cheque_Date datetime = NULL,
@Party_Bank_Id 	  INT = NULL,
@our_ref	varchar(255) = NULL,
@ultimate_payee	varchar(255) = NULL,
@is_ex_gratia  tinyint = 0,
@sBusinessIdentifierCode VARCHAR(50)=NULL,
@sInternationalBankAccountNumber VARCHAR(50)=NULL,
@sPayee_Account_type VARCHAR(255) = NULL 
AS

BEGIN
UPDATE Claim
SET Last_modified_date = Getdate()
WHERE Claim_id = @claim_id

 DECLARE @version_id int

 EXEC spu_CLM_get_claim_version
  @claim_id = @claim_id,
  @version_id = @version_id OUTPUT

 INSERT INTO claim_payment (
  claim_id,
  claim_peril_id,
  date_of_payment,
  amount,  
  tax_amount,  
  tax_amount_WHT,  
  party_cnt,  
  comments,  
  is_referred,  
  created_by,  
  PayeeMediaType,  
  PayeeName,  
  PayeeBankName,  
  PayeeSortCode,  
  PayeeAccountNo,  
  PayeeCountry,  
  PayeeComments,  
  SequenceNo,  
  treaty_id,  
  claim_payment_to_id,  
  payment_party_to,  
  insured_domiciled,  
  insured_percentage,  
  insured_tax_number,  
  payee_domiciled,  
  payee_percentage,  
  payee_tax_number,  
  safe_harbour_id,  
  safe_harbour_percentage,  
  is_tax_exempt,  
  is_wht_exempt,  
  is_settlement,  
  document_id,  
  Media_Ref,  
  currency_id,  
  excess_amount,  
  payeeAddress1,  
  payeeAddress2,  
  payeeAddress3,  
  payeeAddress4,  
  payeePostalCode,  
  ThirdPartyReference,  
  version_id,  
  Cheque_Date,
  Party_bank_Id,
  our_ref, 
  ultimate_payee,
  is_ex_gratia,
  business_identifier_code,
  international_bank_account_number,
  Payee_Account_type
)
  
 VALUES (  
  @claim_id,  
  @claim_peril_id,  
  @date_of_payment,  
  @amount,  
  @tax_amount,  
  @tax_amount_WHT,  
  @party_cnt,  
  @comments,  
  @is_referred,  
  @created_by,  
  @PayeeMediaType,  
  @PayeeName,  
  @PayeeBankName,  
  @PayeeSortCode,  
  @PayeeAccountNo ,  
  @PayeeCountry,  
  @PayeeComments,  
  @SequenceNo,  
  @treaty_id,  
  @claim_payment_to_id,  
  @payment_party_to,  
  @insured_domiciled,  
  @insured_percentage,  
  @insured_tax_number,  
  @payee_domiciled,  
  @payee_percentage,  
  @payee_tax_number,  
  @safe_harbour_id,  
  @safe_harbour_percentage,  
  @is_tax_exempt,  
  @is_wht_exempt,  
  @is_settlement,  
  @document_id,  
  @Media_Ref,  
  @currency_id,  
  @excess_amount,  
  @payeeAddress1,  
  @payeeAddress2,  
  @payeeAddress3,  
  @payeeAddress4,  
  @payeePostalCode,  
  @ThirdPartyReference,  
  @version_id,  
  @Cheque_Date,
  @Party_Bank_Id, 
  @our_ref,@ultimate_payee,
  @is_ex_gratia,
  @sBusinessIdentifierCode,
  @sInternationalBankAccountNumber,
  @sPayee_Account_type
  )
  
 SELECT @claim_payment_id = SCOPE_IDENTITY()
  
 -- update the claim payments base claim payment id to match its newly added payment id  
 UPDATE Claim_Payment  
 SET Base_Claim_Payment_Id = @claim_payment_id  
 WHERE claim_payment_id = @claim_payment_id  
  
END  



GO
