EXECUTE DDLDropProcedure 'spu_Report_Inactive_Claims_SFU'
GO
/*****************************************    
** INACTIVE CLAIMS REPORT    
**    
** Created: P. Haynes 3/9/2001    
**    
** InactiveClaims.rpt    
**  Modified: J Tiwari 01/09/2004    
*****************************************/    
    
CREATE PROCEDURE spu_Report_Inactive_Claims_SFU    
 (@LastModifiedDate datetime,  
  @TypeOfCurrency Varchar(30),  
  @GroupByCode Varchar(30)  
 ) AS  
  
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
SELECT
 /*r.Sum_insured,  
 r.Initial_Reserve,  
 r.Revised_Reserve,  
 r.Initial_Reserve + r.Revised_reserve as IncurredAmount,  
 r.Initial_Reserve + r.Revised_Reserve - r.Paid_to_date as CurrentReserve,*/  
  --r.claim_Peril_id,r.reserve_type_id,this_revision,r.revised_reserve,
Case @TypeOfCurrency 
WHEN 'Base' THEN (isnull(cp.Sum_insured,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
WHEN 'System' THEN (isnull(cp.Sum_insured,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
WHEN 'Transaction' THEN isnull(cp.Sum_insured,0)
END Sum_insured,
 Case @TypeOfCurrency  
  WHEN 'Base' THEN (isnull(r.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))  
  WHEN 'System' THEN (isnull(r.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))  
  WHEN 'Transaction' THEN isnull(r.Initial_reserve,0)  
 END Initial_Reserve,  
 Case @TypeOfCurrency  
  WHEN 'Base' THEN (isnull(r.revised_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))  
  WHEN 'System' THEN  (isnull(r.revised_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))  
  WHEN 'Transaction' THEN isnull(r.revised_reserve,0)  
 END Revised_Reserve,  

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
  WHEN 'Base' THEN ((select isnull(sum(rc.received_to_date),0) from Recovery rc where rc.claim_Peril_id=cp.Claim_Peril_id) *ISNULL(c.currency_base_xrate,CR.rate_against_base)) 
  WHEN 'System' THEN ((select isnull(sum(rc.received_to_date),0) from Recovery rc where rc.claim_Peril_id=cp.Claim_Peril_id)*ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))  
  WHEN 'Transaction' THEN (select isnull(sum(rc.received_to_date),0) from Recovery rc where rc.claim_Peril_id=cp.Claim_Peril_id)  
 END) as IncurredAmount,  
   
/*  
 (Case @TypeOfCurrency  
  WHEN 'Base' THEN (isnull(r.this_revision,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))  
  WHEN 'System' THEN (isnull(r.this_revision,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))  
  WHEN 'Transaction' THEN isnull(r.this_revision,0)  
 END) as IncurredAmount,  
*/  
 (Case @TypeOfCurrency        WHEN 'Base' THEN (isnull(r.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))  
    WHEN 'System' THEN (isnull(r.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))  
    WHEN 'Transaction' THEN isnull(r.Initial_reserve,0)  
 END) +  
 (Case @TypeOfCurrency  
     WHEN 'Base' THEN (isnull(r.Revised_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))  
     WHEN 'System' THEN  (isnull(r.Revised_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))  
     WHEN 'Transaction' THEN isnull(r.Revised_reserve,0)  
 END  
 ) -  
 (Case @TypeOfCurrency  
  WHEN 'Base' THEN (isnull(r.Paid_to_date,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))  
  WHEN 'System' THEN (isnull(r.Paid_to_date,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))  
  WHEN 'Transaction' THEN isnull(r.Paid_to_date,0)  
 END) as CurrentReserve,  

 c.claim_Number,  
 c.Policy_number,  
 c.Client_Name,  
 c.Insurer_Name,  
 c.loss_from_date,  
 c.Reported_date,  
 c.Last_Modified_Date,  
 c.Client_Short_name,  
 c.description as ClaimDescription,  
 p.description as ProductDescription,  
 rt.description as RiskDescription,  


 (Case @TypeOfCurrency  
  WHEN 'Base' THEN (isnull(r.paid_to_date,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))  
  WHEN 'System' THEN (isnull(r.paid_to_date,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))  
  WHEN 'Transaction' THEN isnull(r.paid_to_date,0)  
 END) as Paid_to_date,  
 S.Code CompanyCode,  
 S.description Companydesc,  
 Case @TypeOfCurrency  
  WHEN 'Base' THEN CB.Code  
  WHEN 'System' THEN @SystemCurrencyCode  
  WHEN 'Transaction' THEN ct.Code  
 END CurrencyCode,  
 Case @TypeOfCurrency  
   WHEN 'Base' THEN CB.description  
   WHEN 'System' THEN @SystemCurrencydesc  
   WHEN 'Transaction' THEN ct.description  
 END CurrencyDesc,  
 Case @GroupbyCode  
  WHEN 'Branch' THEN S.Code  
  WHEn 'Branch And Currency' THEN S.code  
  WHEN 'Currency' THEN CT.COde  
  ELSE  
  ''  
 END 'GroupByCode'  
  
FROM    
 Reserve r    
 join claim_peril cp on (r.claim_peril_id = cp.claim_peril_id AND (r.Revised_reserve<>0 or r.Initial_reserve <>0))    
 join Claim c on cp.claim_id = c.claim_id    
 join (select base_claim_id, max(version_id) version_id from claim group by base_claim_id) max_version    
  ON max_version.base_claim_id = c.base_claim_id and max_version.version_id = c.version_id    
 join Insurance_file I on c.Policy_id = I.insurance_file_cnt    
 INNER JOIN currencyrate CR ON CR.currency_id = C.currency_id    
    AND CR.company_id = ISNULL(@branch,I.source_id)    
 left join Product p on i.product_id = P.product_id    
 left join claim_risk crk on c.claim_id = crk. claim_id    
 left join Risk_type rt on crk.risk_type_id = rt.risk_type_Id    
 Join Source S ON s.source_id = I.source_id    
 Join Currency CB ON CB.Currency_id=S.base_currency_id    
 JOIN Currency ct /*Transaction Currency*/    
 ON ct.currency_id = C.currency_id    
where c.Claim_Status_id in (2, 4)    
and  c.last_modified_date < DATEADD (hour, 24, @LastModifiedDate)    
AND CR.effective_from IN    
  (    
   SELECT MAX(effective_from)    
    FROM CurrencyRate    
    WHERE effective_from <= C.reported_date    
    AND   currency_id = CR.currency_id    
    AND company_id = CR.company_id    
  ) 
  
