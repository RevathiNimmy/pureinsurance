SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON 
GO


EXECUTE DDLDropProcedure 'spu_Report_OS_Loss_Reserves_SFU'
GO

/**********************************************************************************************************************************  
** Created by Kerry Butler  
** 31/08/2001  
** Agency Reports - Outstanding_Loss_Reserves.rpt  
**  
**********************************************************************************************************************************  
**  The contents of field EXP are still to be clarified.  
**  
** 1.1      29/11/2001  JMK Change route for getting Treaty Information  
**                          (drop old sp "sp_Report_Claims_OS_By_Reinsurer")  
**  
** 1.2      08/07/2002  JMK Add @IncludeZeroRes parameter to optionally include claims with zero reserves outstanding  
**  
** 1.3      06/09/2002  JMK Summarise amounts in query so that Claim OS TOTAL of zero can be excluded  
**  
** 1.4  10/07/2003 JMK #tempOSLossReserves increase AgentCode and ClientCode to varchar(20) (was 10)  
  
** 1.5  23/08/2004 JT MultiCurrency changes  
***********************************************************************************************************************************/  
/*  
    Claim status id constants  
    1 = Provisional Open Claim  
    2 = Live Open Claim  
    3 = Closed  
    4 = ReOpen  
    5 = ReClosed  
*/  
  

CREATE PROCEDURE spu_Report_OS_Loss_Reserves_SFU (
         @Treaty varchar (100),  
         @IncludeZeroRes varchar (10),  
         @TypeOfCurrency Varchar(30),  
         @GroupByCode Varchar(30),  
         @TPACode Varchar(30) = NULL
        )  
        AS  
  
/*  
-- test  
declare @Treaty varchar (100)  
declare @IncludeZeroRes varchar (10)  
select @Treaty = 'all'  
select @IncludeZeroRes = 'yes'  
--select @Treaty = 'Property Quota Share 01'  
*/  
DECLARE @TPAId INT

SELECT @TPAId=party_cnt from party where shortname= @TPACode

CREATE TABLE #tempOSLossReserves  
(  
        TreatyCode varchar (10) NULL,  
        TreatyDesc varchar (255) NULL,  
        ReserveTypeID int NULL,  
        ReserveType varchar (255) NULL,  
        ClaimNumber varchar (30) NULL,  
        AgentCode varchar (20) NULL,  
        InsuranceRef varchar (30) NULL,  
        ClientCode varchar (20) NULL,  
        ClientName varchar (255) NULL,  
        LossFromDate datetime NULL,  
        LossYear int NULL,  
        ClaimDesc varchar (255) NULL,  
        CausationCodeID int NULL,  
        CatastropheCodeID int NULL,  
        CausationCode varchar (10) NULL,  
        CatastropheCode varchar (10) NULL,  
        InitialReserve money NULL,  
        Payments money NULL,  
        RevisedReserve money NULL,          
        SourceId INT NULL  
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
  
IF @Treaty = 'ALL'  
BEGIN  
    -- print 'get outstanding claims with Reserves '  
    INSERT INTO #tempOSLossReserves  
        SELECT t.code,  
            t.description,  
            res.Reserve_type_id,  
            NULL,  
            c.claim_number,  
            c.insurer_short_name,  
            c.policy_number,  
            c.client_short_name,  
            c.client_name,  
            c.loss_from_date,  
            datepart(year,c.loss_from_date),  
            LEFT(c.description,255),  
            c.Primary_Cause_id,  
            NULL,  
            c.Catastrophe_code_id,  
            NULL,  
            /*sum(res.initial_reserve),  
            sum(res.revised_reserve),  
            sum(res.paid_to_date)*/  
            Case @TypeOfCurrency  
        WHEN 'Base' THEN (isnull(res.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))  
        WHEN 'System' THEN (isnull(res.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))  
       END,  
       Case @TypeOfCurrency  
       WHEN 'Base' THEN (isnull(res.Paid_to_date,0)*  
      ISNULL(c.currency_base_xrate,CR.rate_against_base))  
     WHEN 'System' THEN (isnull(res.Paid_to_date,0)*  
      ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))  
       END,  
       Case @TypeOfCurrency  
        WHEN 'Base' THEN (isnull(res.Revised_reserve,0)*  
        ISNULL(c.currency_base_xrate,CR.rate_against_base))  
        WHEN 'System' THEN  (isnull(res.Revised_reserve,0)*  
        ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))  
       END,ifi.source_id  
        FROM claim c  
        JOIN Claim_Peril cp                 ON c.claim_id = cp.claim_id                                     -- Claim_Peril link  
        JOIN Reserve res                    ON cp.claim_peril_id = res.claim_peril_id  
         JOIN (SELECT base_claim_id,
                              MAX(version_id) version_id
                       FROM   claim
                       WHERE  is_dirty <> 1
                       GROUP  BY base_claim_id) max_version
                   ON max_version.base_claim_id = c.base_claim_id
                      AND max_version.version_id = c.version_id
                      
        JOIN Claim_RI_Arrangement ria       ON ria.claim_id = c.claim_id  
                                            AND ria.risk_cnt = c.risk_type_id                               -- Claim -> Treaty links  
        JOIN Claim_RI_Arrangement_Line ril  ON ril.claim_id = ria.claim_id  
                                            AND ril.ri_arrangement_id = ria.ri_arrangement_id  
        JOIN Treaty t                       ON t.treaty_id = ril.treaty_id  
        INNER JOIN insurance_file ifi      ON ifi.insurance_file_cnt  = C.policy_id  
  INNER JOIN currencyrate CR    ON CR.currency_id = C.currency_id  
           AND CR.company_id = ISNULL(@branch,IFI.source_id)  
        WHERE c.Claim_Number is not null  
         AND c.is_dirty<>1
        AND (c.other_party_id= @TPAId or @TPAId IS NULL)
        --GROUP BY c.claim_number,c.insurer_short_name, c.policy_number, c.client_short_name, c.client_name, c.loss_from_date, c.description,  
        --    t.code, t.description, res.Reserve_type_id, c.Primary_Cause_id, c.Catastrophe_code_id,ifi.source_id  
       AND CR.effective_from IN  
					  (  
					  SELECT MAX(effective_from)  
					  FROM CurrencyRate  
					  WHERE effective_from <= C.reported_date  
					  AND   currency_id = CR.currency_id  
					  AND company_id = CR.company_id  
					  )  
  
    INSERT INTO #tempOSLossReserves  
        SELECT t.code,  
            t.description,  
            res.Reserve_type_id,  
            NULL,  
            c.claim_number,  
            c.insurer_short_name,  
            c.policy_number,  
            c.client_short_name,  
            c.client_name,  
            c.loss_from_date,  
            datepart(year,c.loss_from_date),  
            LEFT(c.description,255),  
            c.Primary_Cause_id,  
            NULL,  
            c.Catastrophe_code_id,  
            NULL,  
            /*sum(res.initial_reserve),  
            sum(res.revised_reserve),  
            sum(res.paid_to_date)*/  
  Case @TypeOfCurrency  
    WHEN 'Base' THEN (isnull(res.Initial_reserve,0)*  
     ISNULL(C.currency_base_xrate,CR.rate_against_base))  
    WHEN 'System' THEN (isnull(res.Initial_reserve,0)*  
     ISNULL(C.currency_base_xrate,CR.rate_against_base) / ISNULL(C.system_base_xrate,CR.rate_against_base))  
   END,  
   Case @TypeOfCurrency  
   WHEN 'Base' THEN (isnull(res.Paid_to_date,0)*  
    ISNULL(c.currency_base_xrate,CR.rate_against_base))  
   WHEN 'System' THEN (isnull(res.Paid_to_date,0) *  
    ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))  
   END,  
   Case @TypeOfCurrency  
    WHEN 'Base' THEN (isnull(res.Revised_reserve,0)*  
    ISNULL(c.currency_base_xrate,CR.rate_against_base))  
    WHEN 'System' THEN  (isnull(res.Revised_reserve,0)*  
    ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))  
   END,ifi.source_id  
        FROM claim c  
        JOIN Claim_Peril cp                 ON c.claim_id = cp.claim_id                                     -- Claim_Peril link  
        JOIN Reserve res                    ON cp.claim_peril_id = res.claim_peril_id  
        JOIN (SELECT base_claim_id,
                              MAX(version_id) version_id
                       FROM   claim
                       WHERE  is_dirty <> 1
                       GROUP  BY base_claim_id) max_version
                   ON max_version.base_claim_id = c.base_claim_id
                      AND max_version.version_id = c.version_id
        JOIN Claim_RI_Arrangement ria       ON ria.claim_id = c.claim_id  
                                            AND ria.risk_cnt = c.risk_type_id  
        JOIN Claim_RI_Arrangement_Line ril  ON ril.claim_id = ria.claim_id  
                                            AND ril.ri_arrangement_id = ria.ri_arrangement_id  
        JOIN Treaty t                       ON t.treaty_id = ril.treaty_id  
  
        INNER JOIN insurance_file ifi      ON ifi.insurance_file_cnt  = C.policy_id  
  INNER JOIN currencyrate CR    ON CR.currency_id = C.currency_id  
           AND CR.company_id = ISNULL(@branch,IFI.source_id)  
        WHERE c.Claim_Number is null  
          AND c.is_dirty<>1
        AND t.code NOT IN ('DAS', 'DAS978', 'DASHELP', 'RETLIMIT')  
        AND (c.other_party_id= @TPAId or @TPAId IS NULL)
        --GROUP BY c.claim_number,c.insurer_short_name, c.policy_number, c.client_short_name, c.client_name, c.loss_from_date, c.description,  
        --t.code, t.description, res.Reserve_type_id, c.Primary_Cause_id, c.Catastrophe_code_id,ifi.source_id  
           AND CR.effective_from IN  
					  (  
					  SELECT MAX(effective_from)  
					  FROM CurrencyRate  
					  WHERE effective_from <= C.reported_date  
					  AND   currency_id = CR.currency_id  
					  AND company_id = CR.company_id  
					  )  
  
END  
ELSE  
BEGIN  
    INSERT INTO #tempOSLossReserves  
        SELECT t.code,  
            t.description,  
            res.Reserve_type_id,  
            NULL,  
            c.claim_number,  
            c.insurer_short_name,  
            c.policy_number,  
            c.client_short_name,  
            c.client_name,  
            c.loss_from_date,  
            datepart(year,c.loss_from_date),  
            LEFT(c.description,255),  
            c.Primary_Cause_id,  
            NULL,  
            c.Catastrophe_code_id,  
            NULL,  
            /*sum(res.initial_reserve),  
            sum(res.revised_reserve),  
            sum(res.paid_to_date)*/  
    Case @TypeOfCurrency  
     WHEN 'Base' THEN (isnull(res.Initial_reserve,0)*  
     ISNULL(C.currency_base_xrate,CR.rate_against_base))  
     WHEN 'System' THEN (isnull(res.Initial_reserve,0)*  
     ISNULL(C.currency_base_xrate,CR.rate_against_base) / ISNULL(C.system_base_xrate,CR.rate_against_base))  
    END,  
    Case @TypeOfCurrency  
     WHEN 'Base' THEN (isnull(res.Paid_to_date,0)*  
     ISNULL(c.currency_base_xrate,CR.rate_against_base))  
     WHEN 'System' THEN (isnull(res.Paid_to_date,0)*  
     ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))  
    END,  
    Case @TypeOfCurrency  
     WHEN 'Base' THEN (isnull(res.Revised_reserve,0)*  
     ISNULL(c.currency_base_xrate,CR.rate_against_base))  
     WHEN 'System' THEN  (isnull(res.Revised_reserve,0)*  
     ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))  
    END,ifi.source_id  
  
        FROM claim c  
        JOIN Claim_Peril cp                 ON c.claim_id = cp.claim_id                                     -- Claim_Peril link  
        JOIN Reserve res                    ON cp.claim_peril_id = res.claim_peril_id  
        JOIN (SELECT base_claim_id,
                              MAX(version_id) version_id
                       FROM   claim
                       WHERE  is_dirty <> 1
                       GROUP  BY base_claim_id) max_version
                   ON max_version.base_claim_id = c.base_claim_id
                      AND max_version.version_id = c.version_id
                      
        JOIN Claim_ri_arrangement ria       ON ria.claim_id = c.claim_id  
                                            AND ria.risk_cnt = c.risk_type_id                               -- Claim -> Treaty links  
        JOIN Claim_RI_Arrangement_Line ril  ON ril.claim_id = ria.claim_id  
                                            AND ril.ri_arrangement_id = ria.ri_arrangement_id  
        JOIN Treaty t                       ON t.treaty_id = ril.treaty_id  
  
        INNER JOIN insurance_file ifi      ON ifi.insurance_file_cnt  = C.policy_id  
  INNER JOIN currencyrate CR    ON CR.currency_id = C.currency_id  
           AND CR.company_id = ISNULL(@branch,IFI.source_id)  
        WHERE t.description = @Treaty  
        AND c.Claim_Number is not null  
        AND c.is_dirty<>1
        AND (c.other_party_id= @TPAId or @TPAId IS NULL)
        
        --GROUP BY c.claim_number,c.insurer_short_name, c.policy_number, c.client_short_name, c.client_name, c.loss_from_date, c.description,  
        --t.code, t.description, res.Reserve_type_id, c.Primary_Cause_id, c.Catastrophe_code_id,ifi.source_id  
        
         AND CR.effective_from IN  
					  (  
					  SELECT MAX(effective_from)  
					  FROM CurrencyRate  
					  WHERE effective_from <= C.reported_date  
					  AND   currency_id = CR.currency_id  
					  AND company_id = CR.company_id  
					  )  
					  
  
    INSERT INTO #tempOSLossReserves  
        SELECT t.code,  
            t.description,  
            res.Reserve_type_id,  
            NULL,  
            c.claim_number,  
            c.insurer_short_name,  
            c.policy_number,  
            c.client_short_name,  
            c.client_name,  
            c.loss_from_date,  
            datepart(year,c.loss_from_date),  
            LEFT(c.description,255),  
            c.Primary_Cause_id,  
            NULL,  
            c.Catastrophe_code_id,  
            NULL,  
            /*sum(res.initial_reserve),  
            sum(res.revised_reserve),  
            sum(res.paid_to_date)*/  
  Case @TypeOfCurrency  
   WHEN 'Base' THEN (isnull(res.Initial_reserve,0)*  
    ISNULL(C.currency_base_xrate,CR.rate_against_base))  
   WHEN 'System' THEN (isnull(res.Initial_reserve,0)*  
    ISNULL(C.currency_base_xrate,CR.rate_against_base) / ISNULL(C.system_base_xrate,CR.rate_against_base))  
  END,  
  Case @TypeOfCurrency  
   WHEN 'Base' THEN (isnull(res.Paid_to_date,0)*  
    ISNULL(c.currency_base_xrate,CR.rate_against_base))  
   WHEN 'System' THEN (isnull(res.Paid_to_date,0)*  
    ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))  
  END,  
  Case @TypeOfCurrency  
   WHEN 'Base' THEN (isnull(res.Revised_reserve,0)*  
    ISNULL(c.currency_base_xrate,CR.rate_against_base))  
   WHEN 'System' THEN  (isnull(res.Revised_reserve,0)*  
    ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))  
  END,ifi.source_id  
        FROM claim c  
        JOIN Claim_Peril cp                 ON c.claim_id = cp.claim_id                                     -- Claim_Peril link  
        JOIN Reserve res                    ON cp.claim_peril_id = res.claim_peril_id  
          JOIN (SELECT base_claim_id,
                              MAX(version_id) version_id
                       FROM   claim
                       WHERE  is_dirty <> 1
                       GROUP  BY base_claim_id) max_version
                   ON max_version.base_claim_id = c.base_claim_id
                      AND max_version.version_id = c.version_id
                      
        JOIN Claim_ri_arrangement ria       ON ria.claim_id = c.claim_id  
                                            AND ria.risk_cnt = c.risk_type_id  
        JOIN Claim_RI_Arrangement_Line ril  ON ril.claim_id = ria.claim_id  
                                            AND ril.ri_arrangement_id = ria.ri_arrangement_id  
        JOIN Treaty t                       ON t.treaty_id = ril.treaty_id  
  
        INNER JOIN insurance_file ifi      ON ifi.insurance_file_cnt  = C.policy_id  
  INNER JOIN currencyrate CR    ON CR.currency_id = C.currency_id  
           AND CR.company_id = ISNULL(@branch,IFI.source_id)  
        WHERE t.description = @Treaty  
        AND c.Claim_Number is null  
        AND c.is_dirty<>1
        AND t.code NOT IN ('DAS', 'DAS978', 'DASHELP', 'RETLIMIT')  
        AND (c.other_party_id= @TPAId or @TPAId IS NULL)
        --GROUP BY c.claim_number,c.insurer_short_name, c.policy_number, c.client_short_name, c.client_name, c.loss_from_date, c.description,  
        --t.code, t.description, res.Reserve_type_id, c.Primary_Cause_id, c.Catastrophe_code_id,ifi.source_id  
         AND CR.effective_from IN  
					  (  
					  SELECT MAX(effective_from)  
					  FROM CurrencyRate  
					  WHERE effective_from <= C.reported_date  
					  AND   currency_id = CR.currency_id  
					  AND company_id = CR.company_id  
					  )  
  
END  
  
UPDATE #tempOSLossReserves  
    SET ReserveType = rt.description,  
        CausationCode = pc.code,  
        CatastropheCode = cc.code  
    FROM #tempOSLossReserves  
    LEFT OUTER JOIN     reserve_type rt     ON rt.Reserve_type_id = ReserveTypeID  
    LEFT OUTER JOIN     Primary_Cause pc    ON pc.primary_cause_id = CausationCodeID  
    LEFT OUTER JOIN     Catastrophe_Code cc ON cc.Catastrophe_code_id = CatastropheCodeID  
  
IF @IncludeZeroRes = 'yes'  
BEGIN  
    SELECT TreatyCode,  
        TreatyDesc,  
        ReserveType,  
        ClaimNumber,  
        AgentCode,  
        InsuranceRef,  
        ClientCode,  
        ClientName,  
        LossFromDate,  
        LossYear,  
        ClaimDesc,  
        CausationCode,  
        CatastropheCode,  
        InitialReserve,  
        RevisedReserve,  
        Payments,S.Code CompanyCode,  
        S.description CompanyDesc,  
  Case @TypeOfCurrency  
   WHEN 'System' THEN  @Systemcurrencycode  
   WHEN 'Base' THEN C.Code  
  END CurrencyCode,  
  Case @TypeOfCurrency  
   WHEN 'System' THEN @SystemCurrencyDesc  
   WHEN  'Base' THEN C.description  
  END CurrencyDesc,  
  Case @GroupbyCode  
   WHEN 'Branch' THEN S.Code  
   WHEN 'Branch And Currency' THEN S.Code  
  ELSE ''  
  END 'GroupByCode'  
    FROM #tempOSLossReserves TR  
    INNER JOIN SOurce S ON S.source_id = TR.sourceid  
 INNER JOIN CURRENCY C ON C.Currency_id= S.base_currency_id  
END  
ELSE  
BEGIN  
    SELECT TreatyCode,  
        TreatyDesc,  
        0,  
        ClaimNumber,  
        AgentCode,  
        InsuranceRef,  
        ClientCode,  
        ClientName,  
        LossFromDate,  
        LossYear,  
        ClaimDesc,  
        CausationCode,  
        CatastropheCode,  
        SUM(InitialReserve) InitialReserve,    
        SUM(RevisedReserve) RevisedReserve,    
        SUM(Payments) Payments,    
        S.Code CompanyCode,  
  S.description CompanyDesc,  
  Case @TypeOfCurrency  
   WHEN 'System' THEN  @Systemcurrencycode  
   WHEN 'Base' THEN C.Code  
  END CurrencyCode,  
  Case @TypeOfCurrency  
   WHEN 'System' THEN @SystemCurrencyDesc  
   WHEN  'Base' THEN C.description  
  END CurrencyDesc,  
  Case @GroupbyCode  
   WHEN 'Branch' THEN S.Code  
   WHEN 'Branch And Currency' THEN S.Code  
  ELSE ''  
  END 'GroupByCode'  
    FROM #tempOSLossReserves TR  
 INNER JOIN Source S ON S.source_id = TR.sourceid  
 INNER JOIN CURRENCY C ON C.Currency_id= S.base_currency_id  
  GROUP BY  TreatyCode, TreatyDesc ,   ClaimNumber    ,
        AgentCode,    
        InsuranceRef,    
        ClientCode,    
        ClientName,    
        LossFromDate,    
        LossYear,    
        ClaimDesc,    
        CausationCode,    
        CatastropheCode,
        C.description,
         S.Code,
          S.description,
          C.Code      
   HAVING  SUM(InitialReserve) + SUM(RevisedReserve) - SUM(Payments) <> 0
END  
  
DROP TABLE #tempOSLossReserves  


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

