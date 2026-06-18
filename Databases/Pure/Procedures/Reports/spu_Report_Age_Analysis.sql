/**********************************************************************************************************************************
** Created by Jude Killip
** 14/11/2000
** RSA Reports -  Age_Analysis.rpt
**      Adapted from spu_Report_Balance_Statement
**      Age Analysis reports for        71 - Client
**                                      72 - Reinsurer
**                                      74 - Agent
**                                      76 - SubAgent
**********************************************************************************************************************************
** 08/12/2000   Jude Killip     remove superfluous (?) grouping and functions
**
** 22/06/2001   Jude Killip     add DocCashType indicator (flag for displaying Unallocated Cash)
**                              filter out matched records (no outstanding amount)
**
** 25/09/2001   Jude Killip     use Account Executive (= consultant) instead of Handler
**                              add resolved name for client accounts
**
** 25/09/2001   Jude Killip     amend DocCashType to exclude Claim payments and receipts
**                              include all records, fully matched or not
**
** 27/09/2001   Jude Killip     filter out dodgy exported account details
**                              transmatch join was creating duplicates - sum in subquery instead
**
** 02/10/2001   Jude Killip     there could be duplicate document ids...get them all
**
** 02/10/2001   Jude Killip     date parameter - not included in <= comparison, use datediff instead
**                              rename temp table
**
** 02/10/2001   Jude Killip     left join transaction export folder so non sirius transactions are included
**
** 25/10/2001   Jude Killip     option to select Purchase Ledgers (Purchase Creditors)
**
** 12/11/2001   JMK             get Underwriting Type flag (display Insurer/Reinsurer)
**
** 19/11/2001   JMK             get UWType using spu_Report_GetUnderwritingType
***********************************************************************************************************************************/
SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Age_Analysis'
GO


CREATE PROCEDURE spu_Report_Age_Analysis
    @end_date datetime,
    @statement_type varchar(255),
    @branch_id int
AS


/*
-- for testing
DECLARE @end_date       datetime,
        @branch_id      int,
        @statement_type char(20)
SELECT @end_date = getdate(),
        @branch_id = 1,
        @statement_type = 'client'
*/

DECLARE @dEndDate       datetime,
        @iBranchID      int,
        @iLedgerID      int,
        @iAccountID     int,
        @UWType         char(1)


IF @end_date IS NULL OR @end_date = ''
        SELECT @dEndDate = GETDATE()
ELSE
        SELECT @dEndDate = @end_date

IF @branch_id IS NULL
        SELECT @iBranchID = 0
ELSE
        SELECT @iBranchID = @branch_id

SELECT @iLedgerID = CASE UPPER(LTRIM(@statement_type))
        WHEN  'CLIENT' THEN 2
        WHEN  'REINSURER' THEN 4
        WHEN  'AGENT' THEN 5
        WHEN  'SUBAGENT' THEN 10
        WHEN  'PURCHASE' THEN 3
END

-- get UWType
EXECUTE spu_Report_GetUnderwritingType @UWType OUTPUT

CREATE TABLE #tmpRSAReportAgeAnal
(
    TransDetailID int NULL,                 /* TransDetail.transdetail_id */
    AccountID int NULL,                     /* Account.account_id - */
    AccountCode varchar (30),               /* Account.short_code - */
    AccountName varchar (60),               /* Account.account_name */
    ResolvedName varchar (100) NULL,        /* Party.resolved_name */
    DocRef varchar (25) NULL,               /* Document.document_ref */
    DocDate datetime NULL,                  /* Document.document_date */
    AccountingDate datetime NULL,           /* TransDetail.accounting_date */
    LedgerName varchar (30) NULL,           /* Ledger.ledger_name */
    InsuranceRef varchar (30) NULL,         /* TransDetail.insurance_ref */
    DocTypeID int NULL,                     /* Document.documenttype_id */
    DocCashType int NULL,                   /* Indicator, based on documenttype_id (22, 23) Payments, Receipts (not claims)
                                                1= payment/receipt, 2 = not payment/receipt */
    Amount numeric (19,4) NULL,             /* TransDetail.amount */
    CompanyID int NULL,                     /* Account.company_id */
    BranchCode varchar (10) NULL,
    IsMatched int NULL,                     /* Match indicator */
    IsEXECDuplicate int NULL,               /* duplicated record indicator */
    MatchAmount numeric (19,4) NULL,        /* Total match amount for transaction */
    RecordType int NULL,                    /* Account.ledger_id  */
    FromSirius tinyint NULL,
    ExportStatus char (1) NULL,
    UWType char (1)
)

IF @branch_id > 0
BEGIN
 --print 'specific branch'
    INSERT INTO #tmpRSAReportAgeAnal
        SELECT td.transdetail_id,
            a.account_id,
            a.short_code,
            a.account_name,
            NULL,
            d.document_ref,
            d.document_date,
            td.accounting_date,
            l.ledger_name,
            td.insurance_ref,
            d.documenttype_id,
            CASE WHEN d.documenttype_id in (22, 23)
                THEN
                    1
                ELSE
                    2
            END,
            td.Amount,
            a.company_id,
            NULL,
            0,
            0,
            (SELECT SUM(tm.base_match_amount)
                FROM TransMatch tm
                JOIN MatchGroup mg ON tm.match_id = mg.match_id
                    AND datediff(day, mg.match_date, @dEndDate) > = 0
                WHERE td.transdetail_id = tm.transdetail_id),
            a.ledger_id,
            (SELECT dt.from_sirius FROM documenttype dt WHERE d.documenttype_id = dt.documenttype_id),
            --sj 31/07/2002 - start
            --tef.accounts_export_status,
            'c',
            --sj 31/07/2002 - start
            @UWType
        FROM Account a
        JOIN  Ledger l          ON a.ledger_id = l.ledger_id
        JOIN  TransDetail td    ON a.account_id = td.account_id
        JOIN  Document d        ON td.document_id = d.document_id
            AND datediff(day,d.document_date, @dEndDate) > = 0
        --sj 31/07/2002 - start
        --LEFT OUTER JOIN  Transaction_Export_Folder tef             ON tef.document_ref = d.document_ref
        --    AND tef.insurance_ref = td.insurance_ref
        --sj 31/07/2002 - end
        WHERE a.ledger_id = @iLedgerID
        AND a.company_id = @iBranchID             -- specific branch
END
ELSE
BEGIN
 --print 'all branches'
    INSERT INTO #tmpRSAReportAgeAnal
        SELECT td.transdetail_id,
            a.account_id,
            a.short_code,
            a.account_name,
            NULL,
            d.document_ref,
            d.document_date,
            td.accounting_date,
            l.ledger_name,
            td.insurance_ref,
            d.documenttype_id,
            CASE WHEN d.documenttype_id in (22, 23)
                THEN
                    1
                ELSE
                    2
            END,
            td.Amount,
            a.company_id,
            NULL,
            0,
            0,
            (SELECT SUM(tm.base_match_amount)
                FROM TransMatch tm
                JOIN MatchGroup mg ON tm.match_id = mg.match_id
                    AND datediff(day, mg.match_date, @dEndDate) > = 0
                WHERE td.transdetail_id = tm.transdetail_id),
            a.ledger_id,
            (SELECT dt.from_sirius FROM documenttype dt WHERE d.documenttype_id = dt.documenttype_id),
            --tef.accounts_export_status,
            'c',
            --sj 31/07/2002 - start
            @UWType
        FROM Account a
        JOIN  Ledger l          ON a.ledger_id = l.ledger_id
        JOIN  TransDetail td    ON a.account_id = td.account_id
        JOIN  Document d        ON td.document_id = d.document_id
            AND datediff(day,d.document_date, @dEndDate) > = 0
        --sj 31/07/2002 - start
        --LEFT OUTER JOIN  Transaction_Export_Folder tef             ON tef.document_ref = d.document_ref
        --    AND tef.insurance_ref = td.insurance_ref
        --sj 31/07/2002 - end
        WHERE a.ledger_id = @iLedgerID
END


/* If it's a Client report, duplicate the information adding AccountEXEC name
        so information can also be summarised by Account Executive (consultant).
        Tried using subreport, but could not pass the SP parameters successfully */


IF @iLedgerID = 2
BEGIN
 --print 'get EXEC duplicates'
    INSERT INTO #tmpRSAReportAgeAnal
        SELECT TransDetailID,
            NULL,
            AccountCode,
            NULL,
            NULL,
            DocRef,
            DocDate,
            AccountingDate,
            LedgerName,
            InsuranceRef,
            DocTypeID,
            DocCashType,
            Amount,
            CompanyID,
            BranchCode,
            IsMatched,
            1,
            MatchAmount,
            RecordType,
            FromSirius,
            ExportStatus,
            @UWType
        FROM #tmpRSAReportAgeAnal

--print 'update with exec name'
    UPDATE #tmpRSAReportAgeAnal
        SET AccountID = p.consultant_cnt,
        AccountCode = (SELECT c.shortname from Party c where c.party_cnt = p.consultant_cnt),
        AccountName = (SELECT c.name from Party c where c.party_cnt = p.consultant_cnt),
        ResolvedName = (SELECT c.resolved_name from Party c where c.party_cnt = p.consultant_cnt)
        FROM #tmpRSAReportAgeAnal AA
        JOIN Party p  ON AA.AccountCode = p.shortname
        WHERE AA.IsEXECDuplicate = 1

--print 'update with client resolved name'
    UPDATE #tmpRSAReportAgeAnal
        SET ResolvedName = (SELECT c.resolved_name from Party c where c.party_cnt = p.party_cnt)
        FROM #tmpRSAReportAgeAnal AA
        JOIN Party p  ON AA.AccountCode = p.shortname
        WHERE AA.IsEXECDuplicate <> 1
END

 --print 'Update with Branch Name'
UPDATE #tmpRSAReportAgeAnal
SET BranchCode = c.code FROM Company c WHERE CompanyID = c.company_id

 --print 'Update with IsMatched'
UPDATE #tmpRSAReportAgeAnal
SET IsMatched = 1 WHERE Amount = MatchAmount

-- It also uses the MatchAmount, so let's set that
UPDATE #tmpRSAReportAgeAnal
SET MatchAmount = Amount
WHERE IsMatched = 1

 --print 'Extract the data - minus dodgy transactions'
SELECT * FROM #tmpRSAReportAgeAnal
WHERE  FromSirius = 1 and ExportStatus = 'c'
    OR FromSirius = 0

DROP TABLE #tmpRSAReportAgeAnal
GO


