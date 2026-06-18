SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_GetCreditCardDetails'
GO

CREATE  Procedure spu_GetCreditCardDetails 
    @OldInsurancefilecnt INT ,
	@NewInsuranceFileCnt INT   
AS    
DECLARE @cc_transaction_code VARCHAR(255) ='',    
@cc_auth_code VARCHAR(255) ='',    
@cc_token_id VARCHAR(255) ='',    
@cc_media_type VARCHAR(255)  ='',     
@Payment_Method VARCHAR(255)  ='' ,  
@PremiumAmount Decimal,  
@CurrencyCode VARCHAR(60), 
@SelectDefaultCard INT =0,
@PartyKey INT=0

SELECT @SelectDefaultCard=value FROM System_Options WHERE option_number =5199
  
SELECT @PremiumAmount=THIS_PREMIUM_ORIGINAL FROM Stats_Detail sd LEFT OUTER JOIN Stats_Folder SF ON SD.stats_folder_cnt =SF.stats_folder_cnt AND SF.insurance_file_cnt =@NewInsuranceFileCnt
WHERE SD.stats_detail_type ='GRS'   
    
SELECT @Payment_Method = payment_method     
FROM Insurance_File    
WHERE insurance_file_cnt=@OldInsurancefilecnt      
  
IF( UPPER(ISNULL(@Payment_Method,'')))= 'PAYNOW'    
BEGIN  
 SELECT
 @cc_transaction_code=cli.cc_transaction_code,
 @cc_auth_code=cli.cc_manual_auth_code,
 @cc_token_id=cli.cc_token_id ,
 @cc_media_type= mt.code,
 @CurrencyCode=curr.Code
 FROM
 CashListItem  cli
 JOIN CashList cl ON cli.cashlist_id=cl.cashlist_id
 JOIN Currency curr ON curr.currency_id=cl.currency_id 
 LEFT OUTER JOIN MediaType mt on CLI.mediatype_id =mt.mediatype_id
 WHERE cc_insurance_file_cnt=@OldInsurancefilecnt  
 
 IF (@SelectDefaultCard =1 AND @PartyKey <>0 )  
 SELECT @cc_token_id=pb.cc_tracking_number,
  @cc_auth_code=pb.manual_auth_number FROM Party_Bank pb 
 INNER JOIN ACCOUNT a on pb.account_id =a.account_id 
 WHERE A.account_key =@PartyKey AND PB.is_default =@SelectDefaultCard
END  
 SELECT    
 @Payment_Method,    
 @cc_media_type,    
 @cc_transaction_code,    
 @cc_auth_code,    
 @cc_token_id,  
 @PremiumAmount,
 @CurrencyCode 
 
 GO