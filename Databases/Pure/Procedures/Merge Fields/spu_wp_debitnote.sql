SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_debitnote'
GO

CREATE PROCEDURE spu_wp_debitnote  
    @PartyCnt INT,  
    @InsuranceFileCnt INT,  
    @RiskID INT,  
    @ClaimCnt INT,  
    @DocumentRef VARCHAR(25),  
    @Instance1 INT,  
    @Instance2 INT,  
    @Instance3 INT  
AS  
  
DECLARE @TotalValue MONEY  
DECLARE @PremiumValue MONEY  
DECLARE @IPTValue MONEY  
DECLARE @VATValue MONEY  
DECLARE @FeeValue MONEY  
  
DECLARE @CommTaxValue MONEY  
DECLARE @PremiumIncFeeValue MONEY  
DECLARE @PremiumExcFeeValue MONEY  
DECLARE @TotalFeesTax MONEY  
  
DECLARE @comm_trans MONEY  
DECLARE @SAcomm_value MONEY  
DECLARE @Acomm_value MONEY  
  
DECLARE @name1 VARCHAR(255)  
DECLARE @name2 VARCHAR(255)  
DECLARE @name3 VARCHAR(255)  
DECLARE @name4 VARCHAR(255)  
DECLARE @name5 VARCHAR(255)  
  
DECLARE @amount1 VARCHAR(10)  
DECLARE @amount2 VARCHAR(10)  
DECLARE @amount3 VARCHAR(10)  
DECLARE @amount4 VARCHAR(10)  
DECLARE @amount5 VARCHAR(10)  
  
DECLARE @addon_ipt_value1 VARCHAR(10)  
DECLARE @addon_ipt_value2 VARCHAR(10)  
DECLARE @addon_ipt_value3 VARCHAR(10)  
DECLARE @addon_ipt_value4 VARCHAR(10)  
DECLARE @addon_ipt_value5 VARCHAR(10)  
  
DECLARE @CurrencyTotalValue MONEY  
DECLARE @CurrencyPremiumValue MONEY  
DECLARE @CurrencyIPTValue MONEY  
DECLARE @CurrencyVATValue MONEY  
DECLARE @CurrencyFeeValue MONEY  
  
DECLARE @Currency_amount1 VARCHAR(10)  
DECLARE @Currency_amount2 VARCHAR(10)  
DECLARE @Currency_amount3 VARCHAR(10)  
DECLARE @Currency_amount4 VARCHAR(10)  
DECLARE @Currency_amount5 VARCHAR(10)  
  
DECLARE @Currency_addon_ipt_value1 VARCHAR(10)  
DECLARE @Currency_addon_ipt_value2 VARCHAR(10)  
DECLARE @Currency_addon_ipt_value3 VARCHAR(10)  
DECLARE @Currency_addon_ipt_value4 VARCHAR(10)  
DECLARE @Currency_addon_ipt_value5 VARCHAR(10)  
  
DECLARE @TotalFees MONEY  
DECLARE @TotalExtras MONEY  
DECLARE @TotalDiscounts MONEY  
  
DECLARE @CurrencyTotalFees MONEY  
DECLARE @CurrencyTotalExtras MONEY  
DECLARE @CurrencyTotalDiscounts MONEY  
  
DECLARE @AgentAmount MONEY  
DECLARE @SubAgentAmount MONEY  
DECLARE @SubAgentInvoice MONEY  
DECLARE @SubAgentExists INT  
  
DECLARE @SharedIndicator INT  
DECLARE @Share FLOAT  
  
DECLARE @Terms_of_payment varchar(255)  
DECLARE @Terms_of_Payment_Due_Date datetime  
  
DECLARE @Tran_Type CHAR(1)  
DECLARE @GrossAmount MONEY
DECLARE @CurrencyGrossAmount MONEY
  
SET NOCOUNT ON  
  
SELECT @SharedIndicator = CHARINDEX('|', @DocumentRef)  
  
If @SharedIndicator = 0  
BEGIN  
    SELECT @Share = 100  
END  
ELSE  
BEGIN  
  
    SELECT @Share = CONVERT(NUMERIC(15,11), RTRIM(SUBSTRING(@DocumentRef, @SharedIndicator + 1, 25 - @SharedIndicator)))  
  
    --Allow for zero percent shares.  
    --IF @Share = 0 SELECT @Share = 100  
  
    SELECT @DocumentRef = SUBSTRING(@DocumentRef, 1, @SharedIndicator -1)  
END  
  
/*Retrieve the insurance_file_cnt from event table*/    
DECLARE @event_insurance_file_cnt INT    
    
    SELECT    
     @event_insurance_file_cnt = EIF.insurance_file_cnt    
    FROM    
    transaction_export_folder TEF    
    JOIN event_log EL    
    ON EL.event_cnt=TEF.event_log_id    
    JOIN event_insurance_file EIF    
    ON EIF.insurance_folder_cnt=EL.event_cnt    
    WHERE TEF.document_ref = @DocumentRef    
    AND TEF.accounts_export_status='c'    
    AND TEF.source_id = (SELECT source_id FROM insurance_file WHERE insurance_file_cnt = @InsuranceFileCnt)    
    
  
SET NOCOUNT OFF  
  
SELECT @Tran_Type = debit_credit from transaction_export_folder    
WHERE document_ref = @DocumentRef    
  
/*Create temporary table*/  
CREATE TABLE #TempID  
(  
    temp_id INT IDENTITY(1,1),  
    account_id INT,  
    partytype VARCHAR(10),  
    transdetail_type_code VARCHAR(20),  
    name VARCHAR(255),  
    amount MONEY,  
    IPT MONEY,  
    VAT MONEY,  
    currency_amount MONEY,  
    currency_IPT MONEY,  
    currency_VAT MONEY  
)  
  
INSERT INTO #TempID  
SELECT  
    a.account_id,  
    pt.code party_type,  
    tt.code,  
    p.name,  
    (td.amount * @Share) / 100,  
    CASE  td.currency_amount   
    WHEN  0 THEN 0  
    ELSE   
        (ROUND(((td.ref_amount * td.amount)/td.currency_amount),2) * @Share) / 100  
    END,  
    CASE td.currency_amount  
    WHEN 0 THEN 0  
    ELSE  
        (ROUND(((td.ref_quantity * td.amount)/td.currency_amount),2) * @Share) / 100  
    END,  
    (td.currency_amount * @Share) / 100,  
    (td.ref_amount * @Share) / 100,  
    (td.ref_quantity * @Share) / 100  
FROM document d  
JOIN transdetail td  
    ON td.document_id = d.document_id  
JOIN account a  
    ON a.account_id = td.account_id  
JOIN party p  
    ON p.party_cnt = a.account_key  
JOIN party_type pt  
    ON pt.party_type_id = p.party_type_id  
JOIN transdetail_type tt  
    ON tt.transdetail_type_id = td.transdetail_type_id  
WHERE d.document_ref = @DocumentRef  
AND d.insurance_file_cnt = @InsuranceFileCnt  
AND tt.code <> 'COMM'  
AND pt.code IN ('IN', 'FE', 'EX', 'DI')  
  
/*DJM 07/01/2003 : Get the amount shared amounts so that total & premium don't have rounding errors*/  
SELECT  
    @IPTValue = ABS(SUM(IPT)),  
    @VatValue = ABS(SUM(VAT)),  
    @CurrencyPremiumValue= ABS(SUM(currency_amount)) - ABS(SUM(currency_IPT)) - ABS(SUM(currency_VAT)),  
    @CurrencyIPTValue = ABS(SUM(currency_IPT)),  
    @CurrencyVATValue = ABS(SUM(currency_VAT)),
    @GrossAmount =ABS(SUM(amount)),
    @CurrencyGrossAmount =ABS(SUM(currency_amount))  
FROM #TempID  
WHERE partytype = 'IN'  
AND transdetail_type_code = 'GROSS'  

    SELECT @IPTValue = (ROUND(((SUM(ISNULL(TC.value,0)) * @GrossAmount) / @currencyGrossAmount),2) * @Share)/100
    FROM event_insurance_COB_section ICS  
    JOIN COB_rating_section CRS ON CRS.COB_rating_section_id=ICS.COB_rating_section_id  
    JOIN tax_group TG ON TG.tax_group_id=ICS.tax_group_id  
    JOIN event_tax_calculation TC 
    ON TC.insurance_file_cnt = @event_insurance_file_cnt
    AND TC.insurance_section_id=ICS.insurance_section_id 	
    AND TC.is_commission_tax=0  
    WHERE  
    ICS.insurance_file_cnt = @event_insurance_file_cnt
    AND TG.code = 'IPT'

    SELECT @CurrencyIPTValue = (ROUND(SUM(ISNULL(TC.value,0)),2)* @Share)/100
    FROM event_insurance_COB_section ICS  
    JOIN COB_rating_section CRS ON CRS.COB_rating_section_id=ICS.COB_rating_section_id  
    JOIN tax_group TG ON TG.tax_group_id=ICS.tax_group_id  
    JOIN event_tax_calculation TC 
    ON TC.insurance_file_cnt = @event_insurance_file_cnt
    AND TC.insurance_section_id=ICS.insurance_section_id 	
    AND TC.is_commission_tax=0  
    WHERE  
    ICS.insurance_file_cnt = @event_insurance_file_cnt
    AND TG.code = 'IPT'

    SELECT @VatValue = (ROUND(((SUM(ISNULL(TC.value,0)) * @GrossAmount) / @currencyGrossAmount),2)* @Share)/100
    FROM event_insurance_COB_section ICS  
    JOIN COB_rating_section CRS ON CRS.COB_rating_section_id=ICS.COB_rating_section_id  
    JOIN tax_group TG ON TG.tax_group_id=ICS.tax_group_id  
    JOIN event_tax_calculation TC 
    ON TC.insurance_file_cnt = @event_insurance_file_cnt
    AND TC.insurance_section_id=ICS.insurance_section_id 	
    AND TC.is_commission_tax=0  
    WHERE  
    ICS.insurance_file_cnt = @event_insurance_file_cnt
    AND TG.code = 'VAT'
  
    SELECT @CurrencyVATValue = ROUND(SUM(ISNULL(TC.value,0)),2)    
    FROM event_insurance_COB_section ICS    
    JOIN COB_rating_section CRS ON CRS.COB_rating_section_id=ICS.COB_rating_section_id    
    JOIN tax_group TG ON TG.tax_group_id=ICS.tax_group_id    
    JOIN event_tax_calculation TC    
    ON TC.insurance_file_cnt = @event_insurance_file_cnt    
    AND TC.insurance_section_id=ICS.insurance_section_id    
    AND TC.is_commission_tax=0    
    WHERE    
    ICS.insurance_file_cnt = @event_insurance_file_cnt    
    AND TG.code = 'VAT'  
  
SELECT  
 @PremiumValue=ROUND(((eis.net_premium * @Share) / 100),2)  
FROM  
 insurance_file inf  
 JOIN document d ON d.company_id=inf.source_id AND d.insurance_file_cnt=inf.insurance_file_cnt  
 JOIN transaction_export_folder tef ON tef.document_ref = d.document_ref AND tef.source_id = d.company_id AND tef.accounts_export_status = 'c'  
 JOIN event_log e ON e.event_cnt = tef.event_log_id  
 JOIN event_insurance_file eis ON eis.insurance_folder_cnt=e.event_cnt  
WHERE  
 inf.insurance_file_cnt = @InsuranceFileCnt and  
  d.document_ref = @DocumentRef  
  
SELECT  
    @FeeValue = ABS(SUM(amount)),  
    @CurrencyFeeValue = ABS(SUM(currency_amount))  
FROM #TempID  
WHERE partytype = 'IN'  
AND transdetail_type_code = 'IFEE'  
  
IF @Share = 100  
BEGIN  
    SELECT  @TotalValue = ABS(SUM(amount)),  
        @currencyTotalvalue = ABS(SUM(currency_amount)) --MKW 040403 - PN2742  
    FROM    #TempID  
END  
ELSE  
BEGIN  
    SELECT  
        @TotalValue = ABS(SUM(td.amount)),  
        @currencyTotalValue = ABS(SUM(td.currency_amount)) --MKW 040403 - PN2742  
    FROM    document d,  
        transdetail td,  
        account ac,  
        party p,  
        insurance_file i  
    WHERE   d.document_ref = @DocumentRef  
    AND i.insurance_file_cnt = @InsuranceFileCnt  
    AND d.company_id = i.source_id  
    AND d.document_id = td.document_id  
    AND ac.account_id = td.account_id  
    AND ac.account_key = p.party_cnt  
    AND p.party_cnt = @partycnt  
    AND td.spare <> 'COMM'  
END  
  
/* Drop Temporary Table */  
DROP Table #TempID  
  
/*Create temporary table*/  
CREATE TABLE #TempID2  
(  
    temp_id INT IDENTITY(1,1),  
    account_id INT,  
    partytype VARCHAR(10),  
    name VARCHAR(255),  
    amount MONEY,  
    IPT MONEY,  
    currency_amount MONEY,  
    currency_IPT MONEY,  
    tax_amount MONEY  
)  
  
INSERT INTO #TempID2  
(  
    account_id,  
    partytype,  
    name,  
    amount,  
    IPT,  
    currency_amount,  
    currency_IPT,  
    tax_amount  
)  
SELECT  
    ac.account_id,  
    pt.code,  
    p.name,  
    ROUND((td.amount * @Share) / 100,2),    
    CASE td.currency_amount  
 WHEN 0 THEN 0  
 ELSE  
     ROUND(((ROUND(((td.ref_amount * td.amount)/td.currency_amount),2) * ROUND(@Share,11)) / 100),2)  
    END,  
    ROUND((td.currency_amount * @Share) / 100,2),  
    ROUND((td.ref_amount * @Share) / 100,2),  
    CASE WHEN @Tran_Type = 'C' 
		THEN CASE pe.fee_charge 
		     WHEN 1 THEN  ROUND((isnull(pf.tax_amount,0) * @Share) / 100,2) * -1  
   		     	    ELSE ROUND((isnull(pf.tax_amount,0) * @Share) / 100,2)     
       		     END  
 		ELSE   ROUND((isnull(pf.tax_amount,0) * @Share) / 100,2)   
        END  
FROM document d  
JOIN transdetail td  
    ON td.document_id = d.document_id  
JOIN transdetail_type tt  
    ON tt.transdetail_type_id = td.transdetail_type_id  
JOIN account ac  
    ON ac.account_id = td.account_id  
JOIN party p  
    ON p.party_cnt = ac.account_key  
JOIN party_type pt  
    ON pt.party_type_id = p.party_type_id  
JOIN insurance_file i  
    ON i.source_id = d.company_id  
JOIN transaction_export_folder tef  
    ON tef.document_ref = d.document_ref  
    AND tef.source_id = d.company_id  
    AND tef.accounts_export_status = 'c'  
JOIN event_log e  
    ON e.event_cnt = tef.event_log_id  
JOIN event_policy_fee pf  
    ON pf.insurance_file_cnt = e.event_cnt  
    AND pf.party_cnt = p.party_cnt  
LEFT JOIN party_extra pe  
    ON pe.party_cnt = p.party_cnt   
WHERE d.document_ref = @DocumentRef  
AND i.insurance_file_cnt = @InsuranceFileCnt  
AND td.spare <> 'COMM'  
AND (  
        pt.code IN ('FE', 'EX', 'DI')  
        OR  
        (  
            pt.code = 'IN'  
            AND  
            tt.code = 'IFEE'  
        )  
    )  
ORDER BY pf.policy_fee_id  

/*Retrieve the add-on totals*/  
SELECT  
    @TotalFees =  
        (  
            SELECT ISNULL(SUM(amount),0)  
            FROM #TempID2  
            WHERE partytype = 'FE'  
        ),  
    @TotalFeesTax =  
 (  
     SELECT ISNULL(SUM(Tax_Amount),0)  
     FROM #TempID2  
     WHERE partytype = 'FE'  
 ),  
    @TotalExtras =  
        (  
            SELECT ISNULL(SUM(amount),0)  
            FROM #TempID2  
            WHERE partytype = 'EX'  
        ),  
    @TotalDiscounts =  
        (  
            SELECT ISNULL(SUM(amount),0)  
            FROM #TempID2  
            WHERE partytype = 'DI'  
        ),  
    @CurrencyTotalFees =  
        (  
            SELECT ISNULL(SUM(currency_amount),0)  
            FROM #TempID2  
            WHERE partytype = 'FE'  
        ),  
    @CurrencyTotalExtras =  
        (  
            SELECT ISNULL(SUM(currency_amount),0)  
            FROM #TempID2  
            WHERE partytype = 'EX'  
        ),  
    @CurrencyTotalDiscounts =  
        (  
            SELECT ISNULL(SUM(currency_amount),0)  
            FROM #TempID2  
            WHERE partytype = 'DI'  
        )  
  
/*Sort out the sign of the add-on totals*/  
SELECT    
    @TotalFees =  
        CASE  
            WHEN @TotalFees < 0 AND @Tran_type = 'D' THEN @TotalFees * -1    
            ELSE @TotalFees  
        END,      
    @TotalExtras =  
        CASE  
            WHEN @TotalExtras < 0 THEN @TotalExtras * -1  
            ELSE @TotalExtras  
        END,  
    @TotalDiscounts = @TotalDiscounts * -1,  
   @CurrencyTotalFees =  
        CASE  
            WHEN @CurrencyTotalFees < 0 THEN @CurrencyTotalFees * -1  
            ELSE @CurrencyTotalFees  
        END,  
    @CurrencyTotalExtras =  
        CASE  
            WHEN @CurrencyTotalExtras < 0 THEN @CurrencyTotalExtras * -1  
            ELSE @CurrencyTotalExtras  
        END,  
    @CurrencyTotalDiscounts = @CurrencyTotalDiscounts * -1  
  
/*Retrieve the first add-ons details*/  
SELECT  
    @name1 =  
        CASE  
            WHEN name IS NULL THEN ''  
            ELSE name  
        END,  
    @amount1 =  
        CASE  
            WHEN amount IS NULL THEN ''  
            WHEN amount < 0 THEN CONVERT(VARCHAR(10), amount * -1)  
            WHEN amount >= 0 AND partytype = 'DI' THEN CONVERT(VARCHAR(10), amount * -1)  
            WHEN amount >= 0 THEN CONVERT(VARCHAR(10), amount)  
        END,  
    @addon_ipt_value1 =  
        CASE  
            WHEN IPT IS NULL THEN ''  
            ELSE CONVERT(VARCHAR(10),IPT)  
        END,  
    @currency_amount1 =  
        CASE  
            WHEN currency_amount IS NULL THEN ''  
            WHEN currency_amount < 0 THEN CONVERT(VARCHAR(10),currency_amount * -1)  
            WHEN currency_amount >= 0 AND partytype = 'DI' THEN CONVERT(VARCHAR(10),currency_amount * -1)  
            WHEN currency_amount >= 0 THEN CONVERT(VARCHAR(10),currency_amount)  
        END,  
    @currency_addon_ipt_value1 =  
        CASE  
            WHEN currency_IPT IS NULL THEN ''  
            ELSE CONVERT(VARCHAR(10),currency_IPT)  
        END  
FROM #TempID2  
WHERE temp_id = 1  
  
/*Retrieve the second add-ons details*/  
SELECT  
    @name2 =  
        CASE  
            WHEN name IS NULL THEN ''  
            ELSE name  
        END,  
    @amount2 =  
        CASE  
            WHEN amount IS NULL THEN ''  
            WHEN amount < 0 THEN CONVERT(VARCHAR(10), amount * -1)  
            WHEN amount >= 0 AND partytype = 'DI' THEN CONVERT(VARCHAR(10), amount * -1)  
            WHEN amount >= 0 THEN CONVERT(VARCHAR(10), amount)  
        END,  
    @addon_ipt_value2 =  
        CASE  
            WHEN IPT IS NULL THEN ''  
            ELSE CONVERT(VARCHAR(10),IPT)  
        END,  
    @currency_amount2 =  
        CASE  
            WHEN currency_amount IS NULL THEN ''  
            WHEN currency_amount < 0 THEN CONVERT(VARCHAR(10),currency_amount * -1)  
            WHEN currency_amount >= 0 AND partytype = 'DI' THEN CONVERT(VARCHAR(10),currency_amount * -1)  
            WHEN currency_amount >= 0 THEN CONVERT(VARCHAR(10),currency_amount)  
        END,  
    @currency_addon_ipt_value2 =  
        CASE  
            WHEN currency_IPT IS NULL THEN ''  
            ELSE CONVERT(VARCHAR(10),currency_IPT)  
        END  
FROM #TempID2  
WHERE temp_id = 2  
  
/*Retrieve the third add-ons details*/  
SELECT  
    @name3 =  
        CASE  
            WHEN name IS NULL THEN ''  
            ELSE name  
        END,  
    @amount3 =  
        CASE  
            WHEN amount IS NULL THEN ''  
            WHEN amount < 0 THEN CONVERT(VARCHAR(10), amount * -1)  
            WHEN amount >= 0 AND partytype = 'DI' THEN CONVERT(VARCHAR(10), amount * -1)  
            WHEN amount >= 0 THEN CONVERT(VARCHAR(10), amount)  
        END,  
    @addon_ipt_value3 =  
        CASE  
            WHEN IPT IS NULL THEN ''  
            ELSE CONVERT(VARCHAR(10),IPT)  
        END,  
    @currency_amount3 =  
        CASE  
            WHEN currency_amount IS NULL THEN ''  
            WHEN currency_amount < 0 THEN CONVERT(VARCHAR(10),currency_amount * -1)  
            WHEN currency_amount >= 0 AND partytype = 'DI' THEN CONVERT(VARCHAR(10),currency_amount * -1)  
            WHEN currency_amount >= 0 THEN CONVERT(VARCHAR(10),currency_amount)  
        END,  
    @currency_addon_ipt_value3 =  
        CASE  
            WHEN currency_IPT IS NULL THEN ''  
            ELSE CONVERT(VARCHAR(10),currency_IPT)  
        END  
FROM #TempID2  
WHERE temp_id = 3  
  
/*Retrieve the fourth add-ons details*/  
SELECT  
    @name4 =  
        CASE  
            WHEN name IS NULL THEN ''  
            ELSE name  
        END,  
    @amount4 =  
        CASE  
            WHEN amount IS NULL THEN ''  
            WHEN amount < 0 THEN CONVERT(VARCHAR(10), amount * -1)  
            WHEN amount >= 0 AND partytype = 'DI' THEN CONVERT(VARCHAR(10), amount * -1)  
            WHEN amount >= 0 THEN CONVERT(VARCHAR(10), amount)  
        END,  
    @addon_ipt_value4 =  
        CASE  
            WHEN IPT IS NULL THEN ''  
            ELSE CONVERT(VARCHAR(10),IPT)  
        END,  
    @currency_amount4 =  
        CASE  
            WHEN currency_amount IS NULL THEN ''  
            WHEN currency_amount < 0 THEN CONVERT(VARCHAR(10),currency_amount * -1)  
            WHEN currency_amount >= 0 AND partytype = 'DI' THEN CONVERT(VARCHAR(10),currency_amount * -1)  
            WHEN currency_amount >= 0 THEN CONVERT(VARCHAR(10),currency_amount)  
        END,  
    @currency_addon_ipt_value4 =  
        CASE  
            WHEN currency_IPT IS NULL THEN ''  
            ELSE CONVERT(VARCHAR(10),currency_IPT)  
        END  
FROM #TempID2  
WHERE temp_id = 4  
  
/*Retrieve the fifth add-ons details*/  
SELECT  
    @name5 =  
        CASE  
            WHEN name IS NULL THEN ''  
            ELSE name  
        END,  
    @amount5 =  
        CASE  
            WHEN amount IS NULL THEN ''  
            WHEN amount < 0 THEN CONVERT(VARCHAR(10), amount * -1)  
            WHEN amount >= 0 AND partytype = 'DI' THEN CONVERT(VARCHAR(10), amount * -1)  
            WHEN amount >= 0 THEN CONVERT(VARCHAR(10), amount)  
        END,  
    @addon_ipt_value5 =  
        CASE  
            WHEN IPT IS NULL THEN ''  
            ELSE CONVERT(VARCHAR(10),IPT)  
        END,  
    @currency_amount5 =  
        CASE  
            WHEN currency_amount IS NULL THEN ''  
            WHEN currency_amount < 0 THEN CONVERT(VARCHAR(10),currency_amount * -1)  
            WHEN currency_amount >= 0 AND partytype = 'DI' THEN CONVERT(VARCHAR(10),currency_amount * -1)  
            WHEN currency_amount >= 0 THEN CONVERT(VARCHAR(10),currency_amount)  
        END,  
    @currency_addon_ipt_value5 =  
        CASE  
            WHEN currency_IPT IS NULL THEN ''  
            ELSE CONVERT(VARCHAR(10),currency_IPT)  
        END  
FROM #TempID2  
WHERE temp_id = 5  
  
IF @name1 IS NULL  
    SELECT @name1 = ''  
IF @name2 IS NULL  
    SELECT @name2 = ''  
IF @name3 IS NULL  
    SELECT @name3 = ''  
IF @name4 IS NULL  
    SELECT @name4 = ''  
IF @name5 IS NULL  
    SELECT @name5 = ''  
  
IF @amount1 IS NULL  
    SELECT @amount1 = ''  
IF @amount2 IS NULL  
    SELECT @amount2 = ''  
IF @amount3 IS NULL  
    SELECT @amount3 = ''  
IF @amount4 IS NULL  
    SELECT @amount4 = ''  
IF @amount5 IS NULL  
    SELECT @amount5 = ''  
  
IF @addon_ipt_value1 IS NULL  
    SELECT @addon_ipt_value1 = ''  
IF @addon_ipt_value2 IS NULL  
    SELECT @addon_ipt_value2 = ''  
IF @addon_ipt_value3 IS NULL  
    SELECT @addon_ipt_value3 = ''  
IF @addon_ipt_value4 IS NULL  
    SELECT @addon_ipt_value4 = ''  
IF @addon_ipt_value5 IS NULL  
    SELECT @addon_ipt_value5 = ''  
  
--MKW 040403 - PN2742 START  
IF @currency_amount1 IS NULL  
    SELECT @addon_ipt_value1 = ''  
IF @currency_amount2 IS NULL  
    SELECT @addon_ipt_value2 = ''  
IF @currency_amount3 IS NULL  
    SELECT @addon_ipt_value3 = ''  
IF @currency_amount4 IS NULL  
    SELECT @addon_ipt_value4 = ''  
IF @currency_amount5 IS NULL  
    SELECT @addon_ipt_value5 = ''  
  
IF @currency_addon_ipt_value1 IS NULL  
    SELECT @currency_addon_ipt_value1 = ''  
IF @currency_addon_ipt_value2 IS NULL  
    SELECT @currency_addon_ipt_value2 = ''  
IF @currency_addon_ipt_value3 IS NULL  
    SELECT @currency_addon_ipt_value3 = ''  
IF @currency_addon_ipt_value4 IS NULL  
    SELECT @currency_addon_ipt_value4 = ''  
IF @currency_addon_ipt_value5 IS NULL  
    SELECT @currency_addon_ipt_value5 = ''  
--MKW 040403 - PN2742 END  
  
--KN (CMG) 251102 Start  
--KN (CMG) start  
  
/*select    @comm_value = sum(ted.transaction_amount)  
from    transaction_export_detail ted  
JOIN    transaction_export_folder tef  
on  tef.transaction_export_folder_cnt = ted.transaction_export_folder_cnt  
WHERE   tef.document_ref = @DocumentRef  
and     ted.spare IN ('BROK','COMM') */  
  
--KN (CMG) end  
  
--TF240903 - PN7006 - Need to return 0 if Agent/SubAgent not present  
--DC251102 -calculate share for share premiums  
--TF240903 - Use Document/Transdetail  to ensure correct company_id  
SELECT  @Acomm_value = (SUM(TD.amount) * @Share) / 100  
FROM    Transdetail td  
JOIN    Document D  
ON      D.document_id = TD.document_id  
WHERE   D.document_ref = @DocumentRef  
AND     D.insurance_file_cnt = @InsuranceFileCnt  
AND     TD.spare = 'AGENT'  
  
IF (@Acomm_value IS NULL)  
BEGIN  
    SELECT @Acomm_value = 0.00  
    SELECT @AgentAmount = 0.00  
END  
ELSE  
    SELECT @AgentAmount = @PremiumValue + @IPTValue + @VATValue - @Acomm_value  
  
--TF240903 - Check if SubAgents exist  
SELECT  @SubAgentExists = MIN(TD.transdetail_id)  
FROM    Transdetail TD  
JOIN    Document D  
ON      D.document_id = TD.document_id  
JOIN    Account A  
ON      A.account_id = TD.account_id  
JOIN    Ledger L  
ON      L.ledger_id = A.ledger_id  
WHERE   D.document_ref = @DocumentRef  
AND     D.insurance_file_cnt = @InsuranceFileCnt  
AND     L.ledger_short_name = 'UB'  
  
--DC251102 -calculate share for share premiums  
--TF240903 - Use Document/Transdetail  to ensure correct company_id  
IF (ISNULL(@SubAgentExists, 0) <> 0)  
BEGIN  
    SELECT  @SAcomm_value = ABS((SUM(TD.amount) * @Share) / 100)  
    FROM    Transdetail TD  
    JOIN    Document D  
    ON      D.document_id = TD.document_id  
    WHERE   D.document_ref = @DocumentRef  
    AND     D.insurance_file_cnt = @InsuranceFileCnt  
    AND     TD.spare IN ('BROK','COMM')  
  
    --Remove Agent comm from SubAgent comm figure  
    SELECT @SAcomm_value = @SAcomm_value + @Acomm_value  
  
    --Any remaining amount = SubAgent comm  
    SELECT @SubAgentAmount = @PremiumValue + @IPTValue + @VATValue - @SAcomm_value  
    SELECT @SubAgentInvoice = @SubAgentAmount + CAST(@amount1 AS MONEY) + CAST(@amount2 AS MONEY) + CAST(@amount3 AS MONEY) + CAST(@amount4 AS MONEY) + CAST(@amount5 AS MONEY)  
END  
ELSE  
    SELECT @SubAgentAmount = 0  
  
--KN (CMG) 251102 End  
  
--KN (CMG) 071102 Start  
--DC251102 -calculate share for shared premiums  
--TF240903 - Use Document/Transdetail  to ensure correct company_id  
SELECT  @comm_trans = (SUM(TD.amount) * @Share) / 100  
FROM    Transdetail td  
JOIN    Document D  
ON      D.document_id = TD.document_id  
WHERE   D.document_ref = @DocumentRef  
AND     D.insurance_file_cnt = @InsuranceFileCnt  
AND     TD.spare = 'BROK'  
--KN (CMG) 071102 End  
  
--S4BDAT004  
SELECT  @Terms_of_payment = tp.description,  
  @Terms_of_Payment_Due_Date = dateadd(day,tp.number_of_days,d.document_date)  
FROM  document d  
LEFT OUTER JOIN terms_of_payment tp  
ON  d.terms_of_payment_id = tp.terms_of_payment_id  
WHERE  d.document_ref = @DocumentRef  
--S4BDAT004 End  
  
SELECT  @CommTaxValue = ISNULL((SUM(TD.amount) * @Share) / 100, 0)  
FROM    Transdetail td  
JOIN    Document D  
ON      D.document_id = TD.document_id  
WHERE   D.document_ref = @DocumentRef  
AND     D.insurance_file_cnt = @InsuranceFileCnt  
AND     TD.spare = 'BROK TAX'  
 
select @PremiumIncFeeValue = isnull(@PremiumValue,0) + isnull(@TotalFees,0) + isnull(@TotalFeesTax,0) + isnull(@IPTValue,0) + isnull(@VATValue,0)  
  
select @PremiumExcFeeValue = isnull(@PremiumValue,0) + isnull(@IPTValue,0) + isnull(@VATValue,0)  
--PN30297  
IF SIGN(@PremiumExcFeeValue) <> SIGN(@comm_trans)  
BEGIN  
SET @comm_trans = @comm_trans * (-1)  
END  
SELECT @TotalValue = @TotalValue + @TotalFeesTax  
SELECT  
    premium_header = 'Premium ',  
    ipt_header = 'IPT and V.A.T on Premium ',  
    total_header = 'Total ',  
    premium_value = @PremiumValue,  
    ipt_value = @IPTValue,  
    total_value = @TotalValue,  
    vat_value = @VATValue,  
    fee_value = @FeeValue,  
    addon_ipt_header = 'IPT ',  
    addon_name1 = @name1,  
    addon_name2  = @name2,  
    addon_name3 = @name3,  
    addon_name4  = @name4,  
    addon_name5  = @name5,  
    addon_value1 = @amount1,  
    addon_value2  = @amount2 ,  
    addon_value3 = @amount3,  
    addon_value4 = @amount4,  
    addon_value5 = @amount5,  
    addon_ipt_value1 = @addon_ipt_value1,  
    addon_ipt_value2 = @addon_ipt_value2,  
    addon_ipt_value3 = @addon_ipt_value3,  
    addon_ipt_value4 = @addon_ipt_value4,  
    addon_ipt_value5 = @addon_ipt_value5,  
    SAnet_premium = @SubAgentAmount,  
    SAtotal_invoice = @SubAgentInvoice,  
    Anet_premium = @AgentAmount,  
    comm_trans = @comm_trans,  
    currency_premium_value = @currencyPremiumValue,  
    currency_ipt_value = @currencyIPTValue,  
    currency_total_value = @currencyTotalValue,  
    currency_vat_value = @currencyVATValue,  
    currency_fee_value = @CurrencyFeeValue,  
    currency_addon_value1 = @currency_amount1,  
    currency_addon_value2  = @currency_amount2 ,  
    currency_addon_value3 = @currency_amount3,  
    currency_addon_value4 = @currency_amount4,  
    currency_addon_value5 = @currency_amount5,  
    currency_addon_ipt_value1 = @currency_addon_ipt_value1,  
    currency_addon_ipt_value2 = @currency_addon_ipt_value2,  
    currency_addon_ipt_value3 = @currency_addon_ipt_value3,  
    currency_addon_ipt_value4 = @currency_addon_ipt_value4,  
    currency_addon_ipt_value5 = @currency_addon_ipt_value5,  
    TotalFees = ABS(@TotalFees),  
    TotalExtras = @TotalExtras,  
    TotalDiscounts = @TotalDiscounts,  
    CurrencyTotalFees = @CurrencyTotalFees,  
    CurrencyTotalExtras = @CurrencyTotalExtras,  
    CurrencyTotalDiscounts = @CurrencyTotalDiscounts,  
    terms_of_payment = @Terms_of_payment,  
    CommissionTaxValue = @CommTaxValue,  
    PremiumIncFeeValue = @PremiumIncFeeValue,  
    PremiumExcFeeValue = @PremiumExcFeeValue,  
    comm_trans_abs = abs(@comm_trans),  
    terms_of_payment_date = @Terms_of_Payment_Due_Date  
/*Drop Temporary Table*/  
DROP TABLE #TempID2  
go
