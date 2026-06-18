SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO
exec DDLDropProcedure spu_Report_Account_Enquiry_SFU
GO

CREATE PROCEDURE spu_Report_Account_Enquiry_SFU
(
	@short_code varchar(30),  
	@TypeofCurrency varchar(15),  
	@GroupByCode varchar(255)  
)
/*
		Created By			: ???
		Creattion Date		: ???
		Description			: Returns financial data for the Account Enquiry Crystal Report
		
		Ammendments
		
		Date		Developer		IM #		Description on change
		-------------------------------------------------------------------------------------
		26 Aug 2015	George Harris	IM895712	Removed the  Where rt.transdetail_id = @transdetail_id  from teh temp table update as it was incorrect

	
*/
AS  
SET NOCOUNT ON  
/*  
*/  
DECLARE @account_id int,  
        @bf_balance numeric(19,4),  
        @cf_balance numeric(19,4),  
        @transdetail_id int,  
        @match_total    numeric(19,4),  
        @cf_Cashbalance numeric(19,4),  
        @cf_DCbalance numeric(19,4),  
        @CashSettled_total numeric(19,4),  
        @DCSettled_total numeric(19,4),  
        @IsCash int,  
        @GrossAmount numeric(19,4),  
        @Settled numeric(19,4),  
        @OA_total numeric(19,4),  
        @systemCurrencyCode Varchar(10),  
        @systemCurrencyDesc Varchar(255)  
  
  
IF @short_code = 'all'  
BEGIN  
    SELECT  @account_id = 0  
END  
ELSE  
BEGIN  
    SELECT  @account_id = account_id  
    FROM    Account  
    WHERE   short_code = @short_code  
END  
/*Get System Currency Details*/  
SELECT  
 @SystemCurrencyCode = c.iso_code,  
 @SystemCurrencyDesc = c.description  
FROM PMSystem pms  
JOIN currency c  
 ON c.currency_id = pms.currency_id  
WHERE pms.system_id = 1  
/*end  Get System Currency*/  
  
CREATE TABLE #Report_Transaction  
(  
    account_code            char (30) NULL,  
    account_name            varchar (60) NULL,  
    document_date           datetime NULL,  
    document_ref            varchar (25) NULL,  
    IsCash                  int,  
    transdetail_id          int NULL,  
    InsuranceRef            varchar (30) NULL,  
    amount                  numeric(19, 4) NULL,  
    media_ref               varchar (100) NULL,  
    Insured                 varchar (255) NULL,  
    balanceBF               numeric(19, 4) NULL,  
    balanceBFCash           numeric(19, 4) NULL,    -- Cash  
    balanceBFDebitCredit    numeric(19, 4) NULL,    -- Debit/Credit  
    balanceCF               numeric(19, 4) NULL,  
    balanceCFCash           numeric(19, 4) NULL,    -- Cash  
    balanceCFDebitCredit    numeric(19, 4) NULL,    -- Debit/Credit  
    matched_amount          numeric(19, 4) NULL,  
    CashSettled_amount      numeric(19, 4) NULL,    -- Cash  
    DCSettled_amount        numeric(19, 4) NULL,    -- Debit/Credit  
    Settled                 numeric(19, 4) NULL,    -- matched_amount - OATotal  
    OATotal                 numeric(19, 4) NULL,     -- OverAllocated Amount  
    CompanyCode Varchar(10) NULL, --Company for multiple currency feature  
    CompanyDesc Varchar(255) NULL,  
    CurrencyCode Varchar(10) NULL, --Currency for multiple currency feature  
    CurrencyDesc Varchar(255) NULL  
)  
INSERT INTO #Report_Transaction  
    SELECT A.short_code,            --account_code  
        A.account_name,             --account_name  
        D.document_date,            --document_date  
        D.document_ref,             --document_ref  
        CASE D.documenttype_id  
            WHEN 22 THEN 1  
            WHEN 23 THEN 1  
            ELSE 0  
        END,                        --IsCash  
        T.transdetail_id,           --transdetail_id  
        T.insurance_ref,            --InsuranceRef  
        CASE @TypeOfCurrency  
  WHEN 'Account' THEN ISNULL(ROUND(T.account_amount,2), 0.0)  
  WHEN 'Base' THEN ISNULL(ROUND(T.amount,2), 0.0)  
  WHEN 'System' THEN ISNULL(ROUND(T.system_amount,2), 0.0)  
  WHEN 'Transaction' THEN ISNULL(ROUND(T.currency_amount,2), 0.0)  
 END Amount,  
--T.amount,                   --amount  
        I.media_ref,                --media_ref  
        '',  
        0,  
        0,  
        0,  
        0,  
        0,  
        0,  
        0,  
        0,  
        0,  
        0,  
        0,  
        C.Code,C.Description,  
        CASE @TypeOfCurrency  
  WHEN 'Account' THEN ca.iso_code  
  WHEN 'Base' THEN cb.iso_code  
  WHEN 'System' THEN @SystemCurrencyCode  
  WHEN 'Transaction' THEN ct.iso_code  
 END,  
 CASE @TypeOfCurrency  
  WHEN 'Account' THEN ca.description  
  WHEN 'Base' THEN cb.description  
  WHEN 'System' THEN @SystemCurrencyDesc  
  WHEN 'Transaction' THEN ct.description  
 END  
  
    FROM Account A  
    JOIN Transdetail T                ON A.account_id = T.account_id  
    JOIN Document D                   ON  D.document_id = T.document_id  
    JOIN company c  
     ON c.company_id = t.company_id  
    JOIN currency ca /*Account Currency*/  
     ON ca.currency_id = a.currency_id  
    JOIN currency cb /*Base Currency*/  
     ON cb.currency_id = c.base_currency  
    JOIN currency ct /*Transaction Currency*/  
 ON ct.currency_id = t.currency_id  
    LEFT OUTER JOIN CashlistItem I    ON T.transdetail_id = I.transdetail_id  
    WHERE A.account_id = @account_id  
 
UPDATE  
    rt  
SET  
    matched_amount =( CASE @TypeOfCurrency  
                                WHEN 'Account' THEN ISNULL(ROUND(td.account_amount,2), 0.0)  
                                WHEN 'Base' THEN ISNULL(ROUND(td.amount,2), 0.0)  
                                WHEN 'System' THEN ISNULL(ROUND(td.system_amount,2), 0.0)  
                                WHEN 'Transaction' THEN ISNULL(ROUND(td.currency_amount,2), 0.0)  
                               END  
                              ) -  
                              ( CASE @TypeOfCurrency  
                                    WHEN 'Account' THEN ISNULL(td.outstanding_account_amount,0.0)  
                                    WHEN 'Base' THEN ISNULL(td.outstanding_amount,0.0)  
                                    WHEN 'System' THEN ISNULL(td.outstanding_system_amount,0.0)  
                                    WHEN 'Transaction' THEN ISNULL(td.outstanding_currency_amount,0.0)  
                                END  
                               ),  
      Settled =( CASE @TypeOfCurrency  
                                WHEN 'Account' THEN ISNULL(ROUND(td.account_amount,2), 0.0)  
                                WHEN 'Base' THEN ISNULL(ROUND(td.amount,2), 0.0)  
                                WHEN 'System' THEN ISNULL(ROUND(td.system_amount,2), 0.0)  
                                WHEN 'Transaction' THEN ISNULL(ROUND(td.currency_amount,2), 0.0)  
                               END  
                              ) -  
                              ( CASE @TypeOfCurrency  
                                    WHEN 'Account' THEN ISNULL(td.outstanding_account_amount,0.0)  
                                    WHEN 'Base' THEN ISNULL(td.outstanding_amount,0.0)  
                                    WHEN 'System' THEN ISNULL(td.outstanding_system_amount,0.0)  
                                    WHEN 'Transaction' THEN ISNULL(td.outstanding_currency_amount,0.0)  
                                END  
                               ),  

      OATotal =         ( CASE @TypeOfCurrency  
                                    WHEN 'Account' THEN ISNULL(td.outstanding_account_amount,0.0)  
                                    WHEN 'Base' THEN ISNULL(td.outstanding_amount,0.0)  
                                    WHEN 'System' THEN ISNULL(td.outstanding_system_amount,0.0)  
                                    WHEN 'Transaction' THEN ISNULL(td.outstanding_currency_amount,0.0)  
                                END  
                               ),  
  
      CashSettled_amount = Case rt.IsCash When 1 Then Settled Else 0 End,  
      DCSettled_amount =  Case rt.IsCash When 1 Then 0 Else Settled End  
  
FROM  
    #Report_Transaction rt  
    JOIN  
    transdetail td ON rt.transdetail_id = td.transdetail_id  

    SELECT @cf_balance =  
     CASE @TypeOfCurrency  
  WHEN 'Account' THEN ISNULL(sum(T.account_amount), 0.0)  
  WHEN 'Base' THEN ISNULL(sum(T.amount), 0.0)  
  WHEN 'System' THEN ISNULL(sum(T.system_amount), 0.0)  
  WHEN 'Transaction' THEN ISNULL(sum(T.currency_amount), 0.0)  
 END  
    FROM TransDetail T  
    JOIN Document D      ON D.document_id = T.document_id  
    WHERE ( ( ISNULL(@account_id, 0) = 0 )  
          OR (  (ISNULL(@account_id, 0) <> 0 )  
          AND T.account_id = @account_id )  
          )  
  
    SELECT @cf_Cashbalance =  
 CASE @TypeOfCurrency  
  WHEN 'Account' THEN ISNULL(sum(T.account_amount), 0.0)  
  WHEN 'Base' THEN ISNULL(sum(T.amount), 0.0)  
  WHEN 'System' THEN ISNULL(sum(T.system_amount), 0.0)  
  WHEN 'Transaction' THEN ISNULL(sum(T.currency_amount), 0.0)  
 END  
    FROM TransDetail T  
    JOIN Document D      ON D.document_id = T.document_id  
    WHERE ( ( ISNULL(@account_id, 0) = 0 )  
          OR (  (ISNULL(@account_id, 0) <> 0 )  
          AND T.account_id = @account_id )  
          )  
    AND D.documenttype_id IN (22,23)   -- SRP, SPY  
  
    SELECT @cf_DCbalance =  
     CASE @TypeOfCurrency  
      WHEN 'Account' THEN ISNULL(sum(T.account_amount), 0.0)  
      WHEN 'Base' THEN ISNULL(sum(T.amount), 0.0)  
      WHEN 'System' THEN ISNULL(sum(T.system_amount), 0.0)  
      WHEN 'Transaction' THEN ISNULL(sum(T.currency_amount), 0.0)  
 END  
    FROM TransDetail T  
    JOIN Document D      ON D.document_id = T.document_id      WHERE ( ( ISNULL(@account_id, 0) = 0 )  
          OR (  (ISNULL(@account_id, 0) <> 0 )  
          AND T.account_id = @account_id )  
          )  
    AND D.documenttype_id NOT IN (22,23)  
  
UPDATE #Report_Transaction  
    SET --balanceBF = @bf_balance,  
        balanceCF = @cf_balance,  
        balanceCFCash = @cf_Cashbalance,  
        balanceCFDebitCredit = @cf_DCbalance  
UPDATE #Report_Transaction  
    SET Insured =  pClient.resolved_name  
    FROM party pClient  
    WHERE pClient.party_cnt =  
        (SELECT max(insured_cnt)  
        FROM Insurance_File WHERE insurance_ref = InsuranceRef  
        AND isnull(InsuranceRef, '') <> ''  
        )  
SELECT  account_code,  
        account_name,  
        document_date,  
        document_ref,  
        InsuranceRef,  
        Insured,  
        media_ref,  
        amount,  
        balanceBF,  
        balanceBFCash,  
        balanceBFDebitCredit,  
        balanceCF,  
        balanceCFCash,  
        balanceCFDebitCredit,  
        matched_amount,  
        CashSettled_amount,  
        DCSettled_amount,  
        Settled,  
        CompanyCode,  
        CompanyDesc,  
        CurrencyCode,  
        CurrencyDesc,  
        CASE @GroupByCode  
  WHEN 'Branch' THEN CompanyCode  
  WHEN 'Branch and Currency' THEN CompanyCode  
  WHEN 'Currency' THEN CurrencyCode  
  ELSE ''  
 END 'GroupByCode1',  
 ISCash  
FROM #Report_Transaction  
DROP TABLE #Report_Transaction  