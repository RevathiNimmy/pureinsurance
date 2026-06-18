SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Report_Binder_Items_SFU'
GO

/****** Object:  Stored Procedure dbo.sp_Report_Binder_Items    Script Date: 16/10/00 12:25:52 ******/
/**********************************************************************************************************************************
** Created by Jude Killip
** 05/09/2000
** RSA Reports -  Binder_Items.rpt
**      Created with dummy data to build the report
**
******************************* ***************************************************************************************************
** 22/11/2000   Jude Killip     Real Data
**
** 18/09/2001   Jude Killip     Get Commission and Tax values
**                              add account_name parameter
**                              amend export folder query
**
** 21/09/2001   Jude Killip     Filter out allocated payments (IsMatched = 1)
**
** 26/09/2001   Jude Killip     transmatch join was creating duplicates - sum in subquery instead
**                              only pick up successful transactions
**
** 27/09/2001   Jude Killip     check EXPORTED account status only
**
** 12/11/2001   JMK             get UWtype (display Insurer/Reinsurer)
**
** 19/11/2001   JMK             get UWType using sp_Report_GetUnderwritingType
**
** 18/04/2002   JMK             RSA_TRANSFER!
**
** 23/10/2003   JMK     Apply changes by Sarah Johnstone - check for Allocation_id in transmatch table
** 06/09/2004   JT      Multi Currency Changes
** 25/10/2004   JT      Merging the 20.2 version
** 14Jun2006    RC      Filter by Agent Group
***********************************************************************************************************************************/
CREATE PROCEDURE spu_Report_Binder_Items_SFU
    @account_type varchar (20),
    @short_code varchar (30),
	@Underwriting_Year char(10),
	@TypeOfCurrency VARCHAR(15),
	@GroupByCode VARCHAR(20),
	@TransactionType VARCHAR(30),
	@end_date DATETIME = NULL,
	@Curr_mon  varchar(3)='NO',
	@AgentGroupCode Varchar(30)='ALL'
AS

EXEC DDLDropTable #tempRSABinderItems

/*
 -- for testing
DECLARE @account_type varchar (20), @short_code varchar (20)

SELECT @account_type = 'ALL', @short_code = 'ALL'
*/
/*Get System Currency Details*/
Declare @SystemCurrencyCode Varchar(30)
Declare @SystemCurrencyDesc Varchar(255)
SELECT
    @SystemCurrencyCode = c.iso_code,
    @SystemCurrencyDesc = c.description
FROM PMSystem pms
JOIN currency c
    ON c.currency_id = pms.currency_id
WHERE pms.system_id = 1

DECLARE @iLedgerID int, @UWType char(1)

SELECT @iLedgerID = CASE @account_type
        WHEN  'REINSURER' THEN 4
        WHEN  'AGENT' THEN 5
        WHEN  'SUBAGENT' THEN 10
END
IF @end_date = '1899-12-30 23:59:59'
BEGIN
    SELECT @end_date = NULL
END
SELECT @end_date = ISNULL(@end_date, GETDATE())
-- If underwriting year is not passed in, set to ALL
if isnull(@Underwriting_Year, '') = ''
    set @Underwriting_Year = 'ALL'

-- get UWType
EXECUTE spu_Report_GetUnderwritingType_SFU @UWType OUTPUT

CREATE TABLE #tempRSABinderItems
(
    AccountID int NULL,                     /* Account.account_id - */
    AccountCode varchar (30),               /* Account.short_code - */
    AccountName varchar (60),               /* Account.account_name */
    RecordType int NULL,                    /* Account.ledger_id  */
    DocID int NULL,
    DocRef varchar (25) NULL,               /* Document.document_ref */
    DocDate datetime NULL,                  /* Document.document_date */
    DocTypeID int NULL,                     /* Document.documenttype_id */
    LedgerName varchar (30) NULL,           /* Ledger.ledger_name */
    TransDetailID int NULL,                 /* TransDetail.transdetail_id */
    InsuranceRef varchar (30) NULL,         /* TransDetail.insurance_ref */
    AccountingDate datetime NULL,           /* TransDetail.accounting_date */
    FullyMatched int NULL,                  /* TransDetail.fully_matched */
    Amount numeric (19,4) NULL,             /* TransDetail.amount */
    Commission numeric (19,4) NULL,         /* COMM  */
    GrossAmount numeric (19,4) NULL,        /* GROSS */
    IsMatched int NULL,                     /* Match indicator */
    MatchAmount numeric (19,4) NULL,        /* Total match amount for transaction */
    ProductCode varchar (10) NULL,          /* Transaction_Export_Folder.product_code */
    EffectiveDate datetime NULL,            /* Transaction_Export_Folder.effective_date */
    InsuranceHolder varchar (20) NULL,      /* Transaction_Export_Folder.insurance_holder_shortname */
    AccountExportStatus char (1) NULL,      /* Transaction_Export_Folder.accounts_export_status */
    FromSirius tinyint NULL,
    UWType char(1) NULL,
    TaxAmount numeric (19,4) NULL,      /* TAX  */
    CoverStartDate datetime NULL,
    Underwriting_year char (10) NULL,
    CompanyCode     Varchar(30) NULL,
    ComapnyDesc     Varchar(255) NULL,
    CurrencyCode    Varchar(30) NULL,
    CurrencyDesc    Varchar(255) NULL,
    GroupByCode     Varchar(30) NULL

)

IF IsNull(@short_code,'ALL') <> 'ALL'
BEGIN
--print 'Specific Account' + @short_code
    INSERT INTO #tempRSABinderItems
            SELECT  a.account_id,
            a.short_code,
            a.account_name,
            a.ledger_id,
            d.document_id,
            d.document_ref,
            d.document_date,
            d.documenttype_id,
            l.ledger_name,
            td.transdetail_id,
            CASE LEFT(d.document_ref,1) WHEN 'C' THEN td.spare ELSE td.insurance_ref END,
            td.accounting_date,
            td.fully_matched,
            CASE @TypeOfCurrency
        WHEN 'Account' THEN td.account_amount
        WHEN 'Base' THEN td.amount
        WHEN 'System' THEN td.system_amount
        WHEN 'Transaction' THEN td.currency_amount
        END,
            (SELECT CASE @TypeOfCurrency
            WHEN 'Account' THEN td.account_amount
            WHEN 'Base' THEN td.amount
            WHEN 'System' THEN td.system_amount
            WHEN 'Transaction' THEN td.currency_amount
            END
        WHERE td.spare = 'COMM' ),
            (SELECT CASE @TypeOfCurrency
            WHEN 'Account' THEN td.account_amount
            WHEN 'Base' THEN td.amount
            WHEN 'System' THEN td.system_amount
            WHEN 'Transaction' THEN td.currency_amount
            END WHERE td.spare = 'GROSS'),
            0,
			(SELECT     Case @TypeOfCurrency
                      WHEN 'Base' THEN Sum(ad.alloc_base_amount)
                      WHEN 'Account' THEN Sum(ad.alloc_account_amount)
                      WHEN 'System' THEN  Sum(ad.alloc_System_amount)
                      WHEN 'Transaction' THEN Sum(ad.alloc_ccy_amount)
                     END
                FROM AllocationDetail ad
                WHERE td.transdetail_id = ad.transdetail_id),
            NULL,
            NULL,
            NULL,
            NULL,
            (SELECT dt.from_sirius
                FROM documenttype dt
                WHERE d.documenttype_id = dt.documenttype_id),
            @UWType,
            (
                SELECT
                    CASE @TypeOfCurrency
                        WHEN 'Account' THEN td.account_amount
                        WHEN 'Base' THEN td.amount
                        WHEN 'System' THEN td.system_amount
                        WHEN 'Transaction' THEN td.currency_amount
                    END
                WHERE td.spare LIKE ('TAX%')
            ),
            i.cover_start_date,
            underwriting_year.code,
            s.code CompanyCode,
            s.description CompanyDesc,
            CASE @TypeOfCurrency
                WHEN 'Account' THEN ca.iso_code
                WHEN 'Base' THEN cb.iso_code
                WHEN 'System' THEN @SystemCurrencyCode
                WHEN 'Transaction' THEN ct.iso_code
            END CurrencyCode,
            CASE @TypeOfCurrency
                WHEN 'Account' THEN ca.description
                WHEN 'Base' THEN cb.description
                WHEN 'System' THEN @SystemCurrencyDesc
                WHEN 'Transaction' THEN ct.description
            END CurrencyDesc,
            CASE @GroupByCode
                WHEN 'Branch' THEN S.Code
                WHEN 'Branch and Currency' THEN S.Code
                WHEN 'Currency' THEN CB.Code
                ELSE ''
            END 'GroupByCode'

        FROM Account a
        JOIN  Ledger l          ON a.ledger_id = l.ledger_id
        JOIN  TransDetail td    ON a.account_id = td.account_id
            AND td.fully_matched <> 1
        JOIN  Document d        ON td.document_id = d.document_id
        LEFT JOIN insurance_file i  ON i.insurance_file_cnt = d.insurance_file_cnt
        LEFT JOIN underwriting_year ON td.underwriting_year_id = underwriting_year.underwriting_year_id
    JOIN Source s
        ON s.source_id = td.company_id
    JOIN Currency cb /*Base Currency*/
        ON cb.currency_id = s.base_currency_id
    JOIN Currency ct /*Transaction Currency*/
        ON ct.currency_id = td.currency_id
    JOIN Currency ca /*Account Currency*/
    ON ca.currency_id = a.currency_id
        WHERE a.short_code LIKE  @short_code + '%'
        AND a.ledger_id IN (4, 5, 10)
        AND (underwriting_year.code = @Underwriting_Year OR rtrim(@Underwriting_Year) = 'ALL')
END
ELSE
IF @account_type = 'ALL'
BEGIN
--print 'Get all 3 Ledger Types: Reinsurer, Agent and Subagent'
    INSERT INTO #tempRSABinderItems
            SELECT  a.account_id,
            a.short_code,
            a.account_name,
            a.ledger_id,
            d.document_id,
            d.document_ref,
            d.document_date,
            d.documenttype_id,
            l.ledger_name,
            td.transdetail_id,
            CASE LEFT(d.document_ref,1) WHEN 'C' THEN td.spare ELSE td.insurance_ref END,
            td.accounting_date,
            td.fully_matched,
            CASE @TypeOfCurrency
                WHEN 'Account' THEN td.account_amount
                WHEN 'Base' THEN td.amount
                WHEN 'System' THEN td.system_amount
                WHEN 'Transaction' THEN td.currency_amount
        END,
            (SELECT CASE @TypeOfCurrency
            WHEN 'Account' THEN td.account_amount
            WHEN 'Base' THEN td.amount
            WHEN 'System' THEN td.system_amount
            WHEN 'Transaction' THEN td.currency_amount
            END
            WHERE td.spare = 'COMM'),
            (SELECT CASE @TypeOfCurrency
            WHEN 'Account' THEN td.account_amount
            WHEN 'Base' THEN td.amount
            WHEN 'System' THEN td.system_amount
            WHEN 'Transaction' THEN td.currency_amount
            END
            WHERE td.spare = 'GROSS'),
            0,
			(SELECT     Case @TypeOfCurrency
                      WHEN 'Base' THEN Sum(ad.alloc_base_amount)
                      WHEN 'Account' THEN Sum(ad.alloc_account_amount)
                      WHEN 'System' THEN  Sum(ad.alloc_System_amount)
                      WHEN 'Transaction' THEN Sum(ad.alloc_ccy_amount)
                     END
                FROM AllocationDetail ad
                WHERE td.transdetail_id = ad.transdetail_id),
            NULL,
            NULL,
            NULL,
            NULL,
            (SELECT dt.from_sirius
                FROM documenttype dt
                WHERE d.documenttype_id = dt.documenttype_id),
            @UWType,
            (
                SELECT
                    CASE @TypeOfCurrency
                        WHEN 'Account' THEN td.account_amount
                        WHEN 'Base' THEN td.amount
                        WHEN 'System' THEN td.system_amount
                        WHEN 'Transaction' THEN td.currency_amount
              END
                WHERE td.spare LIKE ('TAX%')
            ),
            i.cover_start_date,
            underwriting_year.code,
            s.code CompanyCode,
            s.description CompanyDesc,
            CASE @TypeOfCurrency
                WHEN 'Account' THEN ca.iso_code
                WHEN 'Base' THEN cb.iso_code
                WHEN 'System' THEN @SystemCurrencyCode
                WHEN 'Transaction' THEN ct.iso_code
            END CurrencyCode,
            CASE @TypeOfCurrency
                WHEN 'Account' THEN ca.description
                WHEN 'Base' THEN cb.description
                WHEN 'System' THEN @SystemCurrencyDesc
                WHEN 'Transaction' THEN ct.description
            END CurrencyDesc,
            CASE @GroupByCode
                WHEN 'Branch' THEN S.Code
                WHEN 'Branch and Currency' THEN S.Code
                WHEN 'Currency' THEN CB.Code
                ELSE ''
            END 'GroupByCode'

        FROM Account a
        JOIN  Ledger l          ON a.ledger_id = l.ledger_id
        JOIN  TransDetail td    ON a.account_id = td.account_id
            AND td.fully_matched <> 1
        JOIN  Document d        ON td.document_id = d.document_id
        LEFT JOIN insurance_file i  ON i.insurance_file_cnt = d.insurance_file_cnt
        LEFT JOIN underwriting_year ON td.underwriting_year_id = underwriting_year.underwriting_year_id
    JOIN Source s
        ON s.source_id = td.company_id
    JOIN Currency cb /*Base Currency*/
        ON cb.currency_id = s.base_currency_id
    JOIN Currency ct /*Transaction Currency*/
        ON ct.currency_id = td.currency_id
    JOIN Currency ca /*Account Currency*/
    ON ca.currency_id = a.currency_id
        WHERE a.ledger_id IN (4, 5, 10)
        AND (underwriting_year.code = @Underwriting_Year OR rtrim(@Underwriting_Year) = 'ALL')
END
ELSE
BEGIN
--print 'Get Specific Ledger Type ' +  @account_type
    INSERT INTO #tempRSABinderItems
            SELECT  a.account_id,
            a.short_code,
            a.account_name,
            a.ledger_id,
            d.document_id,
            d.document_ref,
            d.document_date,
            d.documenttype_id,
            l.ledger_name,
            td.transdetail_id,
            CASE LEFT(d.document_ref,1) WHEN 'C' THEN td.spare ELSE td.insurance_ref END,
            td.accounting_date,
            td.fully_matched,
            CASE @TypeOfCurrency
                WHEN 'Account' THEN td.account_amount
                WHEN 'Base' THEN td.amount
                WHEN 'System' THEN td.system_amount
                WHEN 'Transaction' THEN td.currency_amount
        END,
            (SELECT CASE @TypeOfCurrency
            WHEN 'Account' THEN td.account_amount
            WHEN 'Base' THEN td.amount
            WHEN 'System' THEN td.system_amount
            WHEN 'Transaction' THEN td.currency_amount
            END WHERE td.spare = 'COMM'),
            (SELECT CASE @TypeOfCurrency
            WHEN 'Account' THEN td.account_amount
            WHEN 'Base' THEN td.amount
            WHEN 'System' THEN td.system_amount
            WHEN 'Transaction' THEN td.currency_amount
            END WHERE td.spare = 'GROSS'),
            0,
           (SELECT     Case @TypeOfCurrency
                      WHEN 'Base' THEN Sum(ad.alloc_base_amount)
                      WHEN 'Account' THEN Sum(ad.alloc_account_amount)
                      WHEN 'System' THEN  Sum(ad.alloc_System_amount)
                      WHEN 'Transaction' THEN Sum(ad.alloc_ccy_amount)
                     END
                FROM AllocationDetail ad
                WHERE td.transdetail_id = ad.transdetail_id),
            NULL,
            NULL,
            NULL,
            NULL,
            (SELECT dt.from_sirius
                FROM documenttype dt
                WHERE d.documenttype_id = dt.documenttype_id),
            @UWType,
            (
                SELECT
                    CASE @TypeOfCurrency
                        WHEN 'Account' THEN td.account_amount
                        WHEN 'Base' THEN td.amount
                        WHEN 'System' THEN td.system_amount
                        WHEN 'Transaction' THEN td.currency_amount
                    END
                WHERE td.spare LIKE ('TAX%')
            ),
            i.cover_start_date,
            underwriting_year.code,
            s.code CompanyCode,
            s.description CompanyDesc,
            CASE @TypeOfCurrency
                WHEN 'Account' THEN ca.iso_code
                WHEN 'Base' THEN cb.iso_code
                WHEN 'System' THEN @SystemCurrencyCode
                WHEN 'Transaction' THEN ct.iso_code
            END CurrencyCode,
            CASE @TypeOfCurrency
                WHEN 'Account' THEN ca.description
                WHEN 'Base' THEN cb.description
                WHEN 'System' THEN @SystemCurrencyDesc
                WHEN 'Transaction' THEN ct.description
            END CurrencyDesc,
            CASE @GroupByCode
                WHEN 'Branch' THEN S.Code
                WHEN 'Branch and Currency' THEN S.Code
                WHEN 'Currency' THEN CB.Code
                ELSE ''
            END 'GroupByCode'

        FROM Account a
        JOIN  Ledger l          ON a.ledger_id = l.ledger_id
        JOIN  TransDetail td    ON a.account_id = td.account_id
            AND td.fully_matched <> 1
        JOIN  Document d        ON td.document_id = d.document_id
        LEFT JOIN insurance_file i  ON i.insurance_file_cnt = d.insurance_file_cnt
        LEFT JOIN underwriting_year ON td.underwriting_year_id = underwriting_year.underwriting_year_id
    JOIN Source s
        ON s.source_id = td.company_id
    JOIN Currency cb /*Base Currency*/
        ON cb.currency_id = s.base_currency_id
    JOIN Currency ct /*Transaction Currency*/
        ON ct.currency_id = td.currency_id
    JOIN Currency ca /*Account Currency*/
    ON ca.currency_id = a.currency_id
        WHERE a.ledger_id = @iLedgerID
        AND (underwriting_year.code = @Underwriting_Year OR rtrim(@Underwriting_Year) = 'ALL')
END

 --print 'Identify matched amounts'
UPDATE #tempRSABinderItems
SET IsMatched = 1 WHERE MatchAmount = Amount

--print 'Update with export folder details'
UPDATE #tempRSABinderItems
    SET ProductCode = tef.product_code,
        EffectiveDate = tef.effective_date,
        InsuranceHolder = tef.insurance_holder_shortname,
        AccountExportStatus = tef.accounts_export_status
    FROM Transaction_Export_Folder tef
    WHERE DocRef = tef.document_ref

IF LOWER(@AgentGroupCode) = 'all'
 BEGIN

 PRINT 'ENTER1'

	--print  'Extract the data filtering out matched amounts and failed transactions'
	If @TransactionType='Premium Transactions Only' AND @curr_mon='NO'
	BEGIN
		SELECT * FROM #tempRSABinderItems
		WHERE IsMatched <> 1 AND Amount <> 0
		AND (FromSirius = 1 and AccountExportStatus = 'c'
		    OR FromSirius = 0) AND docRef not like 'c%' AND (DocDate<=@end_date)
	END

	If @TransactionType='Claims Transactions Only'AND @curr_mon='NO'
	BEGIN
		SELECT * FROM #tempRSABinderItems
		WHERE IsMatched <> 1 AND Amount <> 0
		AND (FromSirius = 1 and AccountExportStatus = 'c'
		    OR FromSirius = 0) AND docRef like 'c%' AND(DocDate<=@end_date)
	END

	If @TransactionType='Premium & Claim Transactions'AND @curr_mon='NO'
		BEGIN
		SELECT * FROM #tempRSABinderItems
		WHERE IsMatched <> 1 AND Amount <> 0
		AND (FromSirius = 1 and AccountExportStatus = 'c'
		    OR FromSirius = 0) AND(DocDate<=@end_date)
	END

	--Monthly Report
	If @TransactionType='Premium Transactions Only' AND @curr_mon='YES'
	BEGIN
		SELECT * FROM #tempRSABinderItems
		WHERE IsMatched <> 1 AND Amount <> 0
		AND (FromSirius = 1 and AccountExportStatus = 'c'
		    OR FromSirius = 0) AND docRef not like 'c%' AND (datepart(m,DocDate)=datepart(m,@end_date) AND
      			datepart(yy,DocDate)=datepart(yy,@end_date))
	END

	If @TransactionType='Claims Transactions Only'AND @curr_mon='YES'
	BEGIN
		SELECT * FROM #tempRSABinderItems
		WHERE IsMatched <> 1 AND Amount <> 0
		AND (FromSirius = 1 and AccountExportStatus = 'c'
		    OR FromSirius = 0) AND docRef like 'c%' AND(datepart(m,DocDate)=datepart(m,@end_date) AND
      			datepart(yy,DocDate)=datepart(yy,@end_date))
	END

	If @TransactionType='Premium & Claim Transactions'AND @curr_mon='YES'
	BEGIN
		SELECT * FROM #tempRSABinderItems
		WHERE IsMatched <> 1 AND Amount <> 0
		AND (FromSirius = 1 and AccountExportStatus = 'c'
		    OR FromSirius = 0) AND(datepart(m,DocDate)=datepart(m,@end_date) AND
      			datepart(yy,DocDate)=datepart(yy,@end_date))
	END

END

IF LOWER(@AgentGroupCode) <> 'all'
 BEGIN

 PRINT 'ENTER2'

	--print  'Extract the data filtering out matched amounts and failed transactions'
	If @TransactionType='Premium Transactions Only' AND @curr_mon='NO'
	BEGIN
		SELECT * FROM #tempRSABinderItems
		WHERE IsMatched <> 1 AND Amount <> 0
		AND (FromSirius = 1 and AccountExportStatus = 'c'
		    OR FromSirius = 0) AND docRef not like 'c%' AND (DocDate<=@end_date)
	    --RC-- 14 Jun 2006
	    AND AccountName IN(
	    select trading_name from party_agent where linked_account_group = (
	    select  party_cnt from party where shortname = @AgentGroupCode) )
	    --RC-- 14 Jun 2006
	END

	If @TransactionType='Claims Transactions Only'AND @curr_mon='NO'
	BEGIN
		SELECT * FROM #tempRSABinderItems
		WHERE IsMatched <> 1 AND Amount <> 0
		AND (FromSirius = 1 and AccountExportStatus = 'c'
		    OR FromSirius = 0) AND docRef like 'c%' AND(DocDate<=@end_date)
	    --RC-- 14 Jun 2006
	    AND AccountName IN(
	    select trading_name from party_agent where linked_account_group = (
	    select  party_cnt from party where shortname = @AgentGroupCode) )
	    --RC-- 14 Jun 2006
	END

	If @TransactionType='Premium & Claim Transactions'AND @curr_mon='NO'
	BEGIN
		SELECT * FROM #tempRSABinderItems
		WHERE IsMatched <> 1 AND Amount <> 0
		AND (FromSirius = 1 and AccountExportStatus = 'c'
		    OR FromSirius = 0) AND(DocDate<=@end_date)
	    --RC-- 14 Jun 2006
	    AND AccountName IN(
	    select trading_name from party_agent where linked_account_group = (
	    select  party_cnt from party where shortname = @AgentGroupCode) )
	    --RC-- 14 Jun 2006
	END

	--Monthly Report
	If @TransactionType='Premium Transactions Only' AND @curr_mon='YES'
	BEGIN
		SELECT * FROM #tempRSABinderItems
		WHERE IsMatched <> 1 AND Amount <> 0
		AND (FromSirius = 1 and AccountExportStatus = 'c'
		    OR FromSirius = 0) AND docRef not like 'c%' AND (datepart(m,DocDate)=datepart(m,@end_date) AND
      		      datepart(yy,DocDate)=datepart(yy,@end_date))
	    --RC-- 14 Jun 2006
	    AND AccountName IN(
	    select trading_name from party_agent where linked_account_group = (
	    select  party_cnt from party where shortname = @AgentGroupCode) )
	    --RC-- 14 Jun 2006
	END

	If @TransactionType='Claims Transactions Only'AND @curr_mon='YES'
	BEGIN
		SELECT * FROM #tempRSABinderItems
		WHERE IsMatched <> 1 AND Amount <> 0
		AND (FromSirius = 1 and AccountExportStatus = 'c'
		    OR FromSirius = 0) AND docRef like 'c%' AND(datepart(m,DocDate)=datepart(m,@end_date) AND
      			datepart(yy,DocDate)=datepart(yy,@end_date))
	    --RC-- 14 Jun 2006
	    AND AccountName IN(
	    select trading_name from party_agent where linked_account_group = (
	    select  party_cnt from party where shortname = @AgentGroupCode) )
	    --RC-- 14 Jun 2006
	END

	If @TransactionType='Premium & Claim Transactions'AND @curr_mon='YES'
	BEGIN
		SELECT * FROM #tempRSABinderItems
		WHERE IsMatched <> 1 AND Amount <> 0
		AND (FromSirius = 1 and AccountExportStatus = 'c'
		    OR FromSirius = 0) AND(datepart(m,DocDate)=datepart(m,@end_date) AND
      			datepart(yy,DocDate)=datepart(yy,@end_date))
        --RC-- 14 Jun 2006
        AND AccountName IN(
        select trading_name from party_agent where linked_account_group = (
        select  party_cnt from party where shortname = @AgentGroupCode) )
        --RC-- 14 Jun 2006
	END
END

DROP TABLE #tempRSABinderItems
GO