SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Report_Client_Statemt_SFU'
GO

/****** Object:  Stored Procedure dbo.sp_Report_Client_Statemt    Script Date: 16/10/00 12:25:55 ******/
/******************************************************************/
/* NAME         : sp_Report_Client_Statemt                      ***/
/* CREATED BY   : Ram Chandrabose                               ***/
/* DATE         : 02-11-2000                                    ***/
/* Description  : Used to Fetch Data for Client_Statement Report***/
/* Used by      : ClientStatement.rpt (RSA Reports)             ***/
/*                                                              ***/
/* Parameters   :-                                              ***/
/* If Needed    : @sClient (Short Name of the Party)    ***/
/******************************************************************/
/* CHANGES                                                      */
/* 08/12/2000   Jude Killip     rewrite                         */
/*                              use parameters                   */
/*                                                              */
-- 26/06/2001   Jude Killip     get documenttype description to display instead of ledger code
-- 02-10-2004   JT      Parameter for Date Criteria PN-16023
/****************************************************************/
CREATE PROCEDURE spu_Report_Client_Statemt_SFU
    @PersonalClient VARCHAR(20) = NULL,
    @GroupClient VARCHAR(20) = NULL,
    @CorporateClient VARCHAR(20) = NULL,
    @Underwriting_Year CHAR(10) = NULL,
    @TypeOfCurrency VARCHAR(15) = NULL,
    @GroupBy VARCHAR(20) = NULL,
    @start_date NVARCHAR(50) = NULL,  
    @end_date NVARCHAR(50) = NULL,  
    @party_cnt INT = NULL,
    @TransactionType VARCHAR(30)=null
    
AS
IF  @start_date IS NOT NULL
SELECT @start_date= CONVERT(DATETIME,@start_date,103)

IF  @end_date IS NOT NULL
SELECT @end_date= CONVERT(DATETIME,@end_date,103)

DECLARE @sClient varchar (20)
DECLARE @CompanyCode VARCHAR(10)
DECLARE @CompanyDesc VARCHAR(255)
DECLARE @SystemCurrencyCode VARCHAR(10)
DECLARE @SystemCurrencyDesc VARCHAR(255)

/*Get the party name*/
IF ISNULL(@party_cnt,0) <> 0
BEGIN
    SELECT @sClient = shortname
    FROM party
    WHERE party_cnt = @party_cnt
END
ELSE
BEGIN
    /*Get client shortname from one of the three client parameters */
    IF @PersonalClient <> 'ALL'
    BEGIN
        SELECT @sClient = @PersonalClient
    END

    ELSE IF @GroupClient <> 'ALL'
    BEGIN
        SELECT @sClient = @GroupClient
    END

    ELSE
    BEGIN
        SELECT @sClient = @CorporateClient
    END
END

/*Default the parameters*/
IF @sClient = 'ALL'
BEGIN
    SELECT @sClient = NULL
END

IF RTRIM(@Underwriting_Year) IN ('','ALL')
BEGIN
    SELECT @Underwriting_Year = NULL
END

IF ISNULL(@TypeOfCurrency,'') = ''
BEGIN
    SELECT @TypeOfCurrency = 'Base'
END

IF ISNULL(@GroupBy,'') = ''
BEGIN
    SELECT @GroupBy = 'No Grouping'
END

IF @start_date = '1899-12-30 23:59:59'
BEGIN
    SELECT @start_date = NULL
END

IF @end_date = '1899-12-30 23:59:59'
BEGIN
    SELECT @end_date = NULL
END
SELECT @end_date = ISNULL(@end_date, GETDATE())


/*Get System Currency*/
SELECT
    @SystemCurrencyCode = c.iso_code,
    @SystemCurrencyDesc = c.description
FROM PMSystem pms
JOIN currency c
    ON c.currency_id = pms.currency_id
WHERE pms.system_id = 1

CREATE TABLE #tempRSACliStatment
(
    Company VARCHAR(255),
    CompanyAddress1 VARCHAR(40),
    CompanyAddress2 VARCHAR(40),
    CompanyAddress3 VARCHAR(40),
    CompanyAddress4 VARCHAR(40),
    PhoneAreaCode VARCHAR(10),
    PhoneNumber VARCHAR(15),
    PhoneExtension VARCHAR(6),
    ClientResolvedName VARCHAR(387),
    LedgerID SMALLINT,
    TransType VARCHAR(255),
    AccountKey INT,
    AccountID INT,
    AccountCode VARCHAR(30),
    AccountName VARCHAR(60),
    AccountAddress1 VARCHAR(40),
    AccountAddress2 VARCHAR(40),
    AccountAddress3 VARCHAR(40),
    AccountAddress4 VARCHAR(40),
    DocRef VARCHAR(25),
    DocDate DATETIME,
    CreateDate DATETIME,
    InsuranceRef VARCHAR(30),
    Amount MONEY,
    Outstanding MONEY,
    PostalCode VARCHAR(20),
    Underwriting_year CHAR(10),
    CompanyCode VARCHAR(10),
    CompanyDesc VARCHAR(255),
    CurrencyCode VARCHAR(10),
    CurrencyDesc VARCHAR(255),
    Code         VARCHAR(10) ,
    DueDate DATETIME           
)

INSERT INTO #tempRSACliStatment
SELECT 
    C.Description,
    C.address1,
    C.address2,
    C.address3,
    C.address4,
    C.phone_area_code,
    C.phone_number,
    C.phone_extension,
    NULL,
    l.ledger_id,
    (SELECT dt.description FROM documenttype dt
        WHERE dt.documenttype_id = d.documenttype_id),
    a.account_key,
    a.account_id,
    a.short_code,
    a.account_name,
    a.address1,
    a.address2,
    a.address3,
    a.address4,
    d.document_ref,
    d.document_date,
    d.created_date,
    t.insurance_ref,
    CASE @TypeOfCurrency
        WHEN 'Account' THEN t.account_amount
        WHEN 'Base' THEN t.amount
        WHEN 'System' THEN t.system_amount
        WHEN 'Transaction' THEN t.currency_amount
    END,
    CASE @TypeOfCurrency
        WHEN 'Account' THEN t.outstanding_account_amount
        WHEN 'Base' THEN t.outstanding_amount
        WHEN 'System' THEN t.outstanding_system_amount
        WHEN 'Transaction' THEN t.outstanding_currency_amount
    END,
    a.postal_code,
    uy.code,
    c.code,
    c.description,
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
    END,
(SELECT dt.code FROM documenttype dt
        WHERE dt.documenttype_id = d.documenttype_id),T.DUE_DATE

FROM Account a
JOIN Ledger l
    ON a.ledger_id = l.ledger_id
JOIN transdetail t 
    ON a.account_id = t.account_id
JOIN Document d
    ON t.document_id = d.document_id
JOIN company c
    ON c.company_id = t.company_id
JOIN currency cb /*Base Currency*/
    ON cb.currency_id = c.base_currency
JOIN currency ca /*Account Currency*/
    ON ca.currency_id = a.currency_id
JOIN currency ct /*Transaction Currency*/
    ON ct.currency_id = t.currency_id
LEFT OUTER JOIN underwriting_year uy 
    ON t.underwriting_year_id = uy.underwriting_year_id
WHERE l.ledger_short_name = 'SA'
AND a.short_code = ISNULL(@sClient,a.short_code)
AND (uy.code = @Underwriting_Year OR @Underwriting_Year IS NULL)
AND (d.document_date >= ISNULL(@start_date,d.document_date) AND d.document_date <= @end_date)

--Print 'Update with Client Resolved Name'
UPDATE #tempRSACliStatment
SET ClientResolvedName = p.resolved_name
FROM Party p
WHERE p.party_cnt = AccountKey

SET NOCOUNT OFF

--Main select statement
IF EXISTS (SELECT 1 FROM #tempRSACliStatment)
BEGIN
IF @TransactionType ='Premium Transactions Only'
BEGIN
SELECT 
    *,
    CASE @GroupBy
        WHEN 'Branch' THEN CompanyCode
        WHEN 'Branch and Currency' THEN CompanyCode
        WHEN 'Currency' THEN CurrencyCode
        ELSE ''
    END 'GroupByCode1',DueDate 
FROM #tempRSACliStatment WHERE code not like 'c%' AND IsNull(Outstanding,0)<>0
END
IF @TransactionType ='Claims Transactions Only'
BEGIN
SELECT 
    *,
    CASE @GroupBy
        WHEN 'Branch' THEN CompanyCode
        WHEN 'Branch and Currency' THEN CompanyCode
        WHEN 'Currency' THEN CurrencyCode
        ELSE ''
    END 'GroupByCode1',DueDate 
FROM #tempRSACliStatment WHERE code in ('CLA','CLP','CLR','CLO') AND IsNull(Outstanding,0)<>0
END
IF @TransactionType ='Premium & Claim Transactions' OR @TransactionType=''
BEGIN
SELECT 
    *,
    CASE @GroupBy
        WHEN 'Branch' THEN CompanyCode
        WHEN 'Branch and Currency' THEN CompanyCode
        WHEN 'Currency' THEN CurrencyCode
        ELSE ''
    END 'GroupByCode1',DueDate 
FROM #tempRSACliStatment WHERE IsNull(Outstanding,0)<>0
END
END

IF NOT EXISTS (SELECT 1 FROM #tempRSACliStatment)
BEGIN
	SELECT 
		'' AS Company, '' AS CompanyAddress1, '' AS CompanyAddress2, '' AS CompanyAddress3, '' AS CompanyAddress4, 
		'' AS PhoneAreaCode, '' AS PhoneNumber, '' AS PhoneExtension, '' AS ClientResolvedName, NULL AS LedgerID, 
		'' AS TransType, NULL AS AccountKey, NULL AS AccountID, '' AS AccountCode, '' AS AccountName, '' AS AccountAddress1, 
		'' AS AccountAddress2, '' AS AccountAddress3, '' AS AccountAddress4, '' AS DocRef, NULL AS DocDate, NULL AS CreateDate, 
		'' AS InsuranceRef, 0 AS Amount, 0 AS Outstanding, '' AS PostalCode, '' AS Underwriting_year, '' AS CompanyCode, 
		'' AS CompanyDesc, '' AS CurrencyCode, '' AS CurrencyDesc, '' AS Code, NULL AS DueDate
		
END

/*Delete Main Temporary Table*/
DROP TABLE #tempRSACliStatment




GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO