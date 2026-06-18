SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_wp_ThisClaim'
GO


CREATE PROCEDURE spu_wp_ThisClaim  
    @PartyCnt INT,  
    @InsuranceFileCnt INT,  
    @RiskId INT,  
    @ClaimCnt INT,  
    @DocumentRef VARCHAR(25),  
    @Instance1 INT,  
    @Instance2 INT,  
    @Instance3 INT  
AS  
  
DECLARE @claim_payment_id int

DECLARE @ThisPaymentBaseCur Money,  
  @ThisPaymentLossCur Money,  
  @ThisPaymentTranCur Money,  
  @TotalPaidLossCur Money,  
  @TotalPaidBaseCur Money  
  
DECLARE @BaseCurDesc as varchar(255),  
  @BaseCurMinorpart as varchar(30),  
  @LastPaymentCurName varchar(255),  
  @LastPaymentCurCode varchar(4),  
  @LastPaymentCurSymbol varchar(4),  
  @LastPaymentCurMinor varchar(30)  
  
DECLARE @LossCurrencyID Int,  
  @LossCurrencyRate Money  
  
DECLARE @CurrentReserveLossCur Money,  
  @CurrentReserveBaseCur Money,  
  @TotalReserveLossCur Money,  
  @TotalReserveBaseCur Money  
  
-- get base currency description and minor_part  
SELECT @BaseCurDesc = currency.description, @BaseCurMinorpart = currency.minor_part  
FROM Claim  
 INNER JOIN Insurance_File ifi ON  
  Claim.Policy_Id = ifi.Insurance_File_cnt  
  INNER JOIN Currency ON  
   ifi.base_currency_id = currency.currency_id  
WHERE claim_id =  @ClaimCnt  
  
--SELECT @BaseCurDesc = description, @BaseCurMinorpart = minor_part  
--FROM Currency WHERE is_deleted = 0 and is_base = 1  
  
-- get loss currency id and loss currency rate against base currency  
SELECT @LossCurrencyID = c.currency_id, @LossCurrencyRate = cr.rate_against_base  
FROM Insurance_File ifi JOIN Claim c ON ifi.insurance_file_cnt = c.policy_id  
JOIN CurrencyRate cr ON c.currency_id = cr.currency_id AND cr.company_id = ifi.source_id  
WHERE c.claim_id = @ClaimCnt  
  
-- get this payment in loss currency  
SELECT @ThisPaymentLossCur=SUM(this_payment) FROM Reserve r JOIN Claim_Peril cp ON r.claim_peril_id = cp.claim_peril_id WHERE cp.claim_id = @ClaimCnt  
  
-- get this payment in base currency (there will be rounding error with this method, but there is no other way at present  
SELECT @ThisPaymentBaseCur = @ThisPaymentLossCur * @LossCurrencyRate  
  
-- get this payment in transaction currency and currency details  
SELECT  
  @claim_payment_id = MAX(claim_payment_id),
  @ThisPaymentTranCur = SUM(ISNULL(this_payment,0)),  
  @LastPaymentCurName = Max(c.description),  
  @LastPaymentCurCode = Max(c.iso_code),  
  @LastPaymentCurSymbol = Max(c.symbol),  
  @LastPaymentCurMinor = Max(c.minor_part)  

FROM  Claim_Payment_Item p 

	INNER JOIN Currency c ON p.currency_id = c.currency_id  
		
-- ensure this is the last payment
AND  claim_payment_id IN  
	 (SELECT max(p2.claim_payment_id)  
	  FROM claim_payment p2 
    	  WHERE claim_id = @claimcnt
	  AND ISNULL(amount,0) <> 0)  


-- get total paid in loss currency  
SELECT @TotalPaidLossCur = SUM(paid_to_date) FROM Reserve r JOIN Claim_Peril cp ON r.claim_peril_id = cp.claim_peril_id WHERE cp.claim_id = @ClaimCnt  
  
-- get total paid in base currency  
SELECT @TotalPaidBaseCur = @TotalPaidLossCur * @LossCurrencyRate  
  
-- get current reserve in loss currency  
SELECT  @CurrentReserveLossCur = sum(Initial_reserve + Revised_reserve - Paid_to_date)  
FROM Reserve r JOIN Claim_Peril cp ON r.claim_peril_id = cp.claim_peril_id  
JOIN Reserve_Type rt ON rt.reserve_type_id = r.reserve_type_id AND rt.Include_in_Total = 1  
WHERE cp.claim_id = @ClaimCnt  
  
-- get current reserve in base currency  
SELECT @CurrentReserveBaseCur =  @CurrentReserveLossCur * @LossCurrencyRate  
  
-- get total reserve in loss currency  
SELECT  @TotalReserveLossCur = sum(Initial_reserve + Revised_reserve)  
FROM Reserve r JOIN Claim_Peril cp ON r.claim_peril_id = cp.claim_peril_id  
JOIN Reserve_type rt ON rt.reserve_type_id = r.reserve_type_id AND rt.Include_in_Total = 1  
WHERE cp.claim_id = @ClaimCnt  
  
-- get total reserve in base currency  
SELECT @TotalReserveBaseCur = @TotalReserveLossCur * @LossCurrencyRate  
  
SELECT c.iso_code Currency_Code,  
  c.description Currency_Desc,  
  c.symbol Currency_Symbol,  
  @ThisPaymentLossCur ThisPaymentLossCur,  
  @ThisPaymentBaseCur ThisPaymentBaseCur,  
  @ThisPaymentTranCur ThisPaymentTranCur,  
  IsNull(@LastPaymentCurName, '') LastPaymentCurrencyName,  
  IsNull(@LastPaymentCurCode,'') LastPaymentCurrencyCode,  
  IsNull(@LastPaymentCurSymbol,'') LastPaymentCurrencySymbol,  
  RTrim(Convert(varchar(255),@ThisPaymentLossCur)) + '|' + RTrim(c.description) + '|' + RTrim(c.minor_part) ThisPaymentWordLossCur,  
  RTrim(Convert(varchar(255),@ThisPaymentBaseCur)) + '|' + RTrim(@BaseCurDesc) + '|' + RTrim(@BaseCurMinorpart) ThisPaymentWordBaseCur,  
  RTrim(Convert(varchar(255),@ThisPaymentTranCur)) + '|' + RTrim(@LastPaymentCurName) + '|' + RTrim(@LastPaymentCurMinor) ThisPaymentWordTranCur,  
  @CurrentReserveLossCur CurrentReserveLossCur,  
  @CurrentReserveBaseCur CurrentReserveBaseCur,  
  @TotalReserveLossCur TotalReserveLossCur,  
  @TotalReserveBaseCur TotalReserveBaseCur,  
  @TotalPaidLossCur TotalPaidLossCur,  
  @TotalPaidBaseCur TotalPaidBaseCur,  
  d.document_ref  

FROM Claim clm 

	INNER JOIN Currency c ON clm.currency_id = c.currency_id  

	LEFT JOIN (SELECT claim_id, document_id 
		   FROM claim_payment 
		   WHERE claim_payment_id = @claim_payment_id) cp ON 
		cp.claim_id = clm.claim_id

		LEFT JOIN Document d ON 
			cp.document_id = d.document_id	

WHERE clm.claim_id = @ClaimCnt  


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
