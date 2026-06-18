
EXECUTE DDLDropProcedure 'spu_Report_Claim_Payments_By_Reserve_Type_SFU'
GO

/*****************************************
**  Payment by Reserve Type report
**
**  Created: P. Haynes 5/9/2001
**
**  Payment_by_Reserve_Type.rpt
**
*****************************************/
/*
** 1.01 14/12/2001  JMK     Amend parameters
** 1.02	27/08/2004  JT	    Multicurrency Changes
**      19/10/2005  PKU     Payment Table into Claim_Payment 
*****************************************/
CREATE PROCEDURE spu_Report_Claim_Payments_By_Reserve_Type_SFU (
    @start_date  datetime,
    @end_date   datetime,
    @TypeOfCurrency Varchar(30),
    @GroupByCode  Varchar(30),
    @TPACode Varchar(30) = NULL
)
AS 
	
CREATE TABLE #tempPaymentsByReserveType
(
   Policy_Number       varchar(60) null,
    Claim_Number        varchar(60) null,
    ClaimDescription    varchar(1000) null,
    Insurer_Name        varchar(255) null,
    Client_Name     varchar(255) null,
    loss_from_date      datetime null,
    ClientShortName     varchar(60) null,
    claimperildescription   varchar(255) null,
    Sum_Insured     money null,
    Initial_Reserve     money null,
    Revised_Reserve     money null,
    Current_Reserve     money null,
    Incurred_Amount     money null,
    Paid_to_date        money null,
    PaymentID       int,--varchar(4),
    dateofpayment       datetime null,
    PaymentAmount       money null,
    ReserveTypeDescription  varchar(255) null,
    ProductDescription  varchar(255) null,
    RiskDescription     varchar(255) null,
    Insurer         varchar(255) null,
    CompanyCode		Varchar(50) null,
    CompanyDesc		Varchar(255) null,
    CurrencyCode	Varchar(50) null,
    CurrencyDesc	Varchar(255) null,
    GroupByCode		Varchar(255) null
)
/*Get System Currency Details--jitendra*/
	declare @SystemCurrencyCode varchar(10)
	declare @SystemCurrencyDesc varchar(255)
    SELECT
    	@SystemCurrencyCode = c.iso_code,
    	@SystemCurrencyDesc = c.description
    FROM PMSystem pms
    JOIN currency c
    	ON c.currency_id = pms.currency_id
    WHERE pms.system_id = 1
/*end  Get System Currency*/
Declare @Branch Int
Declare @TypeOfRates Int
EXEC spu_ACT_GetTypeOfRates @TypeOfRates OUTPUT 
IF @TypeOfRates =1 
	SELECT @branch=1 
ELSE 
	BEGIN
		SELECT @branch=NULL 
	END


DECLARE @TPAId INT

SELECT @TPAId=party_cnt from party where shortname= @TPACode


INSERT #tempPaymentsByReserveType

select
    c.policy_number,
    c.claim_number,
    LEFT(c.description,255) claimdescription,
    c.insurer_name,
    c.client_name,
    c.loss_from_date,
    c.client_short_name,
    cp.description claimperildescription,
    --r.sum_Insured,
    Case @TypeOfCurrency 
		WHEN 'Base' THEN (isnull(cp.Sum_insured,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
		WHEN 'System' THEN (isnull(cp.Sum_insured,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
		WHEN 'Transaction' THEN isnull(cp.Sum_insured,0)
	END,
    --r.initial_reserve,
    Case @TypeOfCurrency 
		WHEN 'Base' THEN (isnull(r.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
		WHEN 'System' THEN (isnull(r.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
		WHEN 'Transaction' THEN isnull(r.Initial_reserve,0)
	END,
--    r.revised_reserve,
	Case @TypeOfCurrency 
		WHEN 'Base' THEN (isnull(r.Revised_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))
		WHEN 'System' THEN  (isnull(r.Revised_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))
		WHEN 'Transaction' THEN isnull(r.Revised_reserve,0)
	END,

--    ((r.Initial_reserve + r.Revised_reserve) - r.Paid_to_date),
	(Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN (isnull(r.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(r.Initial_reserve,0)
	END) +
	(Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Revised_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN  (isnull(r.Revised_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(r.Revised_reserve,0)
	END)-
	(Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Paid_to_date,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN (isnull(r.Paid_to_date,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(r.Paid_to_date,0)
	END),
--    r.Initial_reserve + r.Revised_reserve,
	(Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN (isnull(r.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(r.Initial_reserve,0)
	END) 
		+
	(Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Revised_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN  (isnull(r.Revised_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(r.Revised_reserve,0)
	END),
    --r.paid_to_date,
    (Case @TypeOfCurrency    
   WHEN 'Base' THEN (isnull(r.Paid_to_date,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))    
   WHEN 'System' THEN (isnull(r.Paid_to_date,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))    
   WHEN 'Transaction' THEN isnull(r.Paid_to_date,0)    
 END),    
    p.claim_payment_id,    
    p.date_of_payment,    
   (Case @TypeOfCurrency    
 WHEN 'Base' THEN (isnull(p1.this_payment,0)*ISNULL(p1.currency_base_xrate,CR.rate_against_base))    
 WHEN 'System' THEN (isnull(p1.this_payment,0)*ISNULL(p1.currency_base_xrate,CR.rate_against_base) / ISNULL(p1.system_base_xrate,CR.rate_against_base))    
 WHEN 'Transaction' THEN isnull(p1.this_payment,0)    
 END),    
    rt.description,    
    Prod.description,    
    riskT.description,    
    c.insurer_name,s.code CompanyCode,s.description CompnayDesc,    
 Case @TypeOfCurrency    
  WHEN 'System' THEN  @Systemcurrencycode    
  WHEN 'Base' THEN CB.Code    
  WHEN 'Transaction' THEN CT.CODE    
 END CurrencyCode,    
 Case @TypeOfCurrency    
  WHEN 'System' THEN @SystemCurrencyDesc    
  WHEN  'Base' THEN CB.description    
  WHEN 'Transaction' THEN CT.description    
 END CurrencyDesc,    
 Case @GroupbyCode    
  WHEN 'Branch' THEN S.Code    
  WHEN 'Branch And Currency' THEN S.Code    
  WHEN 'Currency' THEN CT.Code    
 ELSE ''    
 END 'GroupByCode'     

from
    claim c join claim_peril cp on c.claim_id = cp.claim_id
    join reserve r on r.claim_Peril_Id = cp.claim_peril_id
	join (select base_claim_id, max(version_id) version_id from claim where is_dirty<>1 group by base_claim_id) max_version
    ON max_version.base_claim_id = c.base_claim_id and max_version.version_id = c.version_id
    join claim_payment_item p1 on r.reserve_id = p1.reserve_id
    join claim_payment p on p1.claim_payment_id = p.claim_payment_id and document_id is not null    
    join reserve_type rt on r.reserve_type_Id = rt.reserve_type_id
    join Insurance_file I on c.Policy_id = I.insurance_file_cnt
    INNER JOIN currencyrate CR ON CR.currency_id = C.currency_id
	AND CR.company_id = ISNULL(@branch,I.source_id)
    left join Product Prod on i.product_id = Prod.product_id
    left join claim_risk crk on c.claim_id = crk. claim_id
    left join Risk_type riskT on crk.risk_type_id = riskT.risk_type_Id
    join source s on s.source_id= i.source_id
    Join currency CB ON CB.currency_id = s.base_currency_id
    JOIN Currency ct /*Transaction Currency*/
 ON ct.currency_id = c.currency_id     

where
    c.claim_status_id <> 1
    AND  c.is_dirty<>1
    AND (c.other_party_id= @TPAId or @TPAId IS NULL)
    AND CR.effective_from IN
			(
				SELECT MAX(effective_from)
				 	FROM CurrencyRate 
					WHERE effective_from <= C.reported_date
					AND   currency_id = CR.currency_id
				AND company_id = CR.company_id
			)
			

SELECT  tmp.* FROM #tempPaymentsByReserveType tmp
JOIN Claim_Payment cp ON  cp.claim_payment_id =tmp.PaymentID
JOIN claim clm ON clm.Claim_id = cp.claim_id
WHERE dateofpayment >= @start_date  
AND dateofpayment <= @end_date  
AND paymentamount <> 0  
AND clm.is_dirty<>1
  

DROP TABLE #tempPaymentsByReserveType 

GO
