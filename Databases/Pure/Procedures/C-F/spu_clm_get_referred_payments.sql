
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_clm_get_referred_payments'
GO

CREATE PROCEDURE spu_clm_get_referred_payments(  
@claim_number  varchar(30) = NULL,  
@policy_number  varchar(30) = NULL,  
@client_id  int      = NULL,  
@date_of_payment datetime    = NULL,  
@userid   smallint    = NULL,  
@source_id  int         = NULL,  
@other_party_id int = null,  
@nauthorizerId     smallint    = NULL,  
@sClient_Code CHAR(20)=NULL,  
@sUser_Name VARCHAR(255)=NULL,  
@sQUERY VARCHAR(10)='LIKE',  
@CaseNumber VARCHAR(50)=NULL,  
@PayeeName VARCHAR(255)=NULL,
@AgentKey INT=0)
  
AS  
  
IF @other_party_id=0  
SET @other_party_id=NULL  
  
DECLARE @sSQL nVARCHAR(max)  
  
SELECT @sSQL = 'SELECT Claim.Claim_id,  
    Claim.Claim_Number,  
    Claim.Policy_Number,  
    Claim.Client_name,  
       Round(ISNULL(Claim_Payment.amount,0) + ISNULL(Claim_Payment.tax_amount,0) + ISNULL(Claim_Payment.tax_amount_WHT,0),2) as Payment_Amount,  
    Claim_Payment.date_of_payment,  
    PMUser.username,  
    ''Pending'' as Status,  
    Claim_Payment.claim_payment_id,  
    PMUser.user_id AS PaymentCreatroUser,  
    Claim_payment.is_referred_for_recommendation,  
    U1.UserName ''Recommender'',  
    Insurance_File.Product_ID,  
    Insurance_File.insured_cnt as ''Client_id'',  
    Claim.Client_short_name as ''Client_code'',  
    Currency.code as ''CurrencyCode'',  
    [Case].case_number as ''CaseNumber'',  
    Claim_Payment.PayeeName as ''PayeeName'',  
    Case Claim_Payment.payment_party_to  
  When 1 Then ''Claim Payable''  
  When 2 Then ''Party''  
  When 4 Then ''Agent''  
  When 8 Then ''Client''  
 End ''PayeeType'',  
        Currency.currency_id Currency_id,  
  
  ISNULL(S.closed_allow_claims,0) AllowedCLosedBranchClaims,
  p.Authorisation_Threshold
  
FROM     Claim_Payment  
  
INNER JOIN (SELECT MIN(version_id) as version_id, base_claim_payment_id  
     FROM claim_payment  
     GROUP by base_claim_payment_id) claim_payment_versions  
 ON claim_payment.version_id = claim_payment_versions.version_id  
 and claim_payment.base_claim_payment_id = claim_payment_versions.base_claim_payment_id  
  
    INNER JOIN  Claim ON  
        Claim_Payment.claim_id = Claim.Claim_id  
  
    INNER JOIN  Insurance_File ON  
    Claim.Policy_id = Insurance_File.insurance_file_cnt  
    LEFT JOIN Product p ON p.Product_ID=Insurance_File.Product_ID
  
    LEFT OUTER JOIN  
        PMUser ON Claim_Payment.created_by = PMUser.user_id  
    LEFT JOIN  
        PMUser U1 ON Claim_payment.recommended_by = U1.user_id  
    LEFT OUTER JOIN  
        Currency ON Claim_Payment.currency_id = Currency.currency_id  
    INNER JOIN  
        Party ON party.party_cnt = insurance_file.insured_cnt  
  
 LEFT JOIN [case] on  
        [case].case_id  = claim.base_case_id  
 LEFT JOIN Source s ON Insurance_File.source_id=S.source_id  
WHERE (Claim_Payment.amount <> 0)  
AND (Claim_Payment.is_referred = 1)  '  
  
IF @claim_number IS NOT NULL AND @sQUERY='LIKE'  
 SELECT @sSQL=@sSQL+ ' AND Claim.Claim_Number LIKE ''' + @claim_number +''''  
ELSE IF @claim_number IS NOT NULL AND @sQUERY='='  
 SELECT @sSQL=@sSQL+ ' AND Claim.Claim_Number=''' + @claim_number +''''  
  
IF @policy_number IS NOT NULL AND @sQUERY='LIKE'  
 SELECT @sSQL=@sSQL+ ' AND Claim.Policy_Number LIKE ''' + @policy_number +''''  
ELSE IF @policy_number IS NOT NULL AND @sQUERY='='  
 SELECT @sSQL=@sSQL+ ' AND Claim.Policy_Number = ''' + @policy_number +''''  
IF @client_id IS NOT NULL  
SELECT @sSQL=@sSQL+ ' AND Claim.client_id = '+ CONVERT(varchar(20),@client_id)  
IF @date_of_payment IS NOT NULL  
SELECT @sSQL=@sSQL+ ' AND Convert(varchar(30), Claim_Payment.date_of_payment, 106) =''' + CONVERT(varchar(30),@date_of_payment, 106) +''''  
IF @userid IS NOT NULL  
SELECT @sSQL=@sSQL+ ' AND Claim_Payment.created_by = ' + CONVERT(varchar(20),@userid)  
IF @source_id IS NOT NULL  
SELECT @sSQL=@sSQL+ ' AND Insurance_File.source_id = ' + CONVERT(varchar(20),@source_id)  
IF @nauthorizerId IS NOT Null  
SELECT @sSQL=@sSQL+ ' AND (Insurance_File.source_id NOT IN (SELECT source_id FROM pmuser_source WITH (NOLOCK) WHERE user_id ='+ CONVERT(varchar(20),@nauthorizerId) +'))'  
IF @sUser_Name IS NOT NULL AND @sQUERY='LIKE'  
SELECT @sSQL=@sSQL+ ' AND PMUSER.username LIKE '''+ @sUser_Name +''''  
ELSE IF @sUser_Name IS NOT NULL AND @sQUERY='='  
SELECT @sSQL=@sSQL+ ' AND PMUSER.username = '''+ @sUser_Name +''''  
IF @sClient_Code IS NOT NULL AND @sQUERY='LIKE'  
SELECT @sSQL=@sSQL+ ' AND Party.shortname  LIKE '''+ @sClient_Code +''''  
ELSE IF @sClient_Code IS NOT NULL AND @sQUERY='='  
SELECT @sSQL=@sSQL+ ' AND Party.shortname  = '''+ @sClient_Code +''''  
IF @CaseNumber IS NOT NULL AND @sQUERY='LIKE'  
SELECT @sSQL=@sSQL+ ' AND [case].case_number LIKE ''' + @CaseNumber + ''''  
ELSE IF @CaseNumber IS NOT NULL AND @sQUERY='='  
SELECT @sSQL=@sSQL+ ' AND [case].case_number = ''' + @CaseNumber + ''''  
IF @other_party_id IS NOT NULL AND  @sQUERY='LIKE'  
SELECT @sSQL=@sSQL+ ' AND claim.other_party_id LIKE '''+ CONVERT(varchar(20),@other_party_id)  +''''  
ELSE IF @other_party_id IS NOT NULL AND  @sQUERY='='  
SELECT @sSQL=@sSQL+ ' AND claim.other_party_id = '''+ CONVERT(varchar(20),@other_party_id)  +''''  
IF @PayeeName IS NOT NULL AND  @sQUERY='LIKE'  
SELECT @sSQL=@sSQL+ ' AND Claim_Payment.PayeeName LIKE '''+ @PayeeName  +''''  
ELSE IF @PayeeName IS NOT NULL AND  @sQUERY='='  
SELECT @sSQL=@sSQL+ ' AND Claim_Payment.PayeeName = '''+ @PayeeName  +''''  

IF @AgentKey >0  
SELECT @sSQL=@sSQL+ ' AND Insurance_File.lead_agent_cnt= ' + CONVERT(varchar(20),@AgentKey)  

EXEC sp_executeSQL @sSQL  
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
