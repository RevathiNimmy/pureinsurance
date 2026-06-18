SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Claim_Receipt_Add'
GO

CREATE PROCEDURE spu_CLM_Claim_Receipt_Add  
 @claim_receipt_id int OUTPUT,  
 @claim_id  int,  
 @claim_peril_id  int,  
 @date_of_receipt datetime,  
 @party_cnt  int,  
 @Amount   money,  
 @tax_amount  money,  
 @comments  varchar(255),  
 @created_by smallint,  
 @insured_domiciled tinyint,  
 @insured_percentage float,  
 @insured_tax_number varchar(255),  
 @receivable_tax_percentage float,  
 @is_tax_exempt  tinyint,  
 @is_settlement  tinyint,  
 @PayeeMediaType  int,  
 @PayeeName  varchar(255),  
 @PayeeBankName  varchar(255),  
 @PayeeSortCode  varchar(255),  
 @PayeeAccountNo  varchar(255),  
 @PayeeCountry  int,  
 @PayeeComments  varchar(255),  
 @PayeeMediaRef  varchar(255),  
 @document_id  int,  
 @currency_id  smallint,
@theirRef varchar(50)=NULL,
@address1 varchar(50)=NULL,
@address2 varchar(50)=NULL,
@address3 varchar(50)=NULL,
@address4 varchar(50)=NULL,
@postalCode varchar(50)=NULL   
  
AS  
  
BEGIN  
  
 DECLARE @version_id  int

 EXEC spu_CLM_get_claim_version 
		@claim_id = @claim_id, 
		@version_id = @version_id OUTPUT

 
 INSERT INTO claim_receipt (  
  claim_id,  
  claim_peril_id,  
  date_of_receipt,  
  party_cnt,  
  Amount,  
  tax_amount,  
  comments,  
  created_by,  
  insured_domiciled,  
  insured_percentage,  
  insured_tax_number,  
  receivable_tax_percentage,  
  is_tax_exempt,  
  is_settlement,  
  PayeeMediaType,  
  PayeeName,  
  PayeeBankName,  
  PayeeSortCode,  
  PayeeAccountNo,  
  PayeeCountry,  
  PayeeComments,  
  PayeeMediaRef,  
  document_id,  
  currency_id,
PayeeAddress1,
PayeeAddress2,
PayeeAddress3,
PayeeAddress4,
PayeePostalCode,
ThirdPartyReference,
version_id 
  )  
 VALUES (  
 @claim_id,  
 @claim_peril_id,  
 @date_of_receipt,  
 @party_cnt,  
 @Amount,  
 @tax_amount,  
 @comments,  
 @created_by,  
 @insured_domiciled,  
 @insured_percentage,  
 @insured_tax_number,  
 @receivable_tax_percentage,  
 @is_tax_exempt,  
 @is_settlement,  
 @PayeeMediaType,  
 @PayeeName,  
 @PayeeBankName,  
 @PayeeSortCode,  
 @PayeeAccountNo,  
 @PayeeCountry,  
 @PayeeComments,  
 @PayeeMediaRef,  
 @document_id,  
 @currency_id,
 @address1,
@address2,
@address3,
@address4,
@postalCode,
@theirRef, 
 @version_id)  
  
 SELECT @claim_receipt_id =  @@IDENTITY  
  
 UPDATE claim_receipt 
 SET base_claim_receipt_id =  @claim_receipt_id  
 WHERE claim_receipt_id = @claim_receipt_id
  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
