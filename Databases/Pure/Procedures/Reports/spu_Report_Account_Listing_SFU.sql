
EXECUTE DDLDropProcedure 'spu_Report_Account_Listing_SFU'
GO
/**********************************************************************************************************************************
** Created by Jude Killip
** 06/09/2000
** RSA Reports -  Account_Listing.rpt
**
**********************************************************************************************************************************
** 19/09/2000   JMK     - change DB name and remove test stuff
**
** 29/11/2000   JMK     - add parameter, to correspond with Ledger (account 'type') lookup
**                      - filter Amount where TransDetail.document_sequence = 2   to get one element of the double entry (otherwise
**                        values cancel each other out)
**
** 28/06/2001   JMK     - rewrite!!
**                      - indicate cash, commission and matched amounts
**                      - limit to current year to date
**                      - only retrieve full details if needed
**
** 05/07/2001   JMK     - bug #1314 amend joins to include all accounts in long version
**
** 05/10/2001   JMK     - don't include claim payments and receipts as cash
**                      - check export status of 'from sirius' transactions
**
** 09/10/2001   JMK     - update Client Account Name with Party resolved_name
**                      - resize AccountName to accommodate resolved name
**
** 12/11/2001   JMK     - get Underwriting Type flag (display Insurer/Reinsurer)
**
** 19/11/2001   JMK             get UWType using sp_Report_GetUnderwritingType
**
** 28/05/2003   JOnkemp - branch parameter added

**-- 06/07/2005	JT	insured name is now 255 long
***********************************************************************************************************************************/
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 

CREATE PROCEDURE spu_Report_Account_Listing_SFU
        @LedgerName varchar (30),
        @ReportFormat varchar (15),
	    @branch_id int,
	    @agent_code	varchar(20)
	    As

declare @ibranchid int
IF @branch_id IS NULL
		SELECT @iBranchID = 0
	ELSE
		SELECT @iBranchID = @branch_id

/*
-- for testing
DECLARE @LedgerName varchar (30),
        @ReportFormat varchar (15)
SELECT @LedgerName = 'ALL', @ReportFormat = 'long'
*/

-- get UWType
DECLARE @UWType char(1)
EXECUTE spu_Report_GetUnderwritingType_SFU @UWType OUTPUT

CREATE TABLE #tempRSAAcntListing
    (
        LedgerID            int NULL,
        LedgerShortName     varchar (2) NULL,
        LedgerName          varchar (30) NULL,
        AccountKey          int NULL,
        AccountID           int NULL,
        AccountCode         varchar (30) NULL,
        AccountName         varchar (387) NULL,
        AccountAddress1     varchar (40) NULL,
        AccountAddress2     varchar (40) NULL,
        AccountAddress3     varchar (40) NULL,
        AccountAddress4     varchar (40) NULL,
        DocTypeID           int NULL,
        DocumentType        varchar (255) NULL,
        DocumentRef         varchar (25) NULL,
        DocumentDate        datetime NULL,
        DocCreatedDate      datetime NULL,
        IsCash              varchar (10) NULL,
        InsuranceRef        varchar (30) NULL,
        Amount              decimal (19,4) NULL,
        FullyMatched        int NULL,
        GrossAmount         decimal (19,4) NULL,
        MatchAmount         decimal (19,4) NULL,
        CommissionAmount    decimal (19,4) NULL,
        dtCurrentPeriodEnd  datetime,
        ExportStatus        char (1) NULL,
        FromSirius          tinyint NULL,
        UWType              char (1)
    )
 declare @iAgentCnt	int
declare @sAgentCode	varchar (20)
IF @agent_code = 'ALL' BEGIN
	SELECT @agent_code = NULL
END
SELECT @sAgentCode = ISNULL(@agent_code,'')

IF @sAgentCode <> ''
	SELECT @iAgentCnt = party_cnt FROM party WHERE shortname = @sAgentCode
ELSE
	SELECT @iAgentCnt = 0

IF @ReportFormat = 'Long'
-- Get account and document details
BEGIN

    -- get current period values
    DECLARE @CurrentPeriodID int, @dtCurrentPeriodEnd datetime
    EXECUTE spu_Report_GetCurrentPeriod_SFU @CurrentPeriodID OUTPUT,  @dtCurrentPeriodEnd OUTPUT

    -- get current year values
    DECLARE @CurrentYearStartPeriodID int, @dtLastYearPeriodEndDate datetime
    EXECUTE spu_Report_GetCurrentYear_SFU @CurrentYearStartPeriodID OUTPUT, @dtLastYearPeriodEndDate OUTPUT

    IF @LedgerName = 'ALL'
    /* Get all Ledger types */
    BEGIN
        INSERT INTO #tempRSAAcntListing
            SELECT l.ledger_id,
                l.ledger_short_name,
                l.ledger_name,
                a.account_key,
                a.account_id,
                a.short_code,
                a.account_name,
                a.address1,
                a.address2,
                a.address3,
                a.address4,
                d.documenttype_id,
                (SELECT dt.description
                    FROM documenttype dt
                    WHERE dt.documenttype_id = d.documenttype_id),
                d.document_ref,
                d.document_date,
                d.created_date,
                CASE WHEN d.documenttype_id IN (22, 23)
                    THEN
                        'cash'
                    ELSE
                        ''
                    END iscash,
                t.insurance_ref,
                t.amount,
                t.fully_matched,
                (SELECT t.amount WHERE spare <> 'COMM'),
                tm.base_match_amount,
                (SELECT t.amount WHERE spare = 'COMM'),
                @dtCurrentPeriodEnd,
                tef.accounts_export_status,
                (SELECT dt.from_sirius FROM documenttype dt WHERE d.documenttype_id = dt.documenttype_id),
                @UWType
            FROM Account a
            JOIN Ledger l ON a.ledger_id = l.ledger_id
            LEFT OUTER JOIN transdetail t ON a.account_id = t.account_id
                AND t.period_id BETWEEN @CurrentYearStartPeriodID AND @CurrentPeriodID
            LEFT OUTER JOIN  Document d ON t.document_id = d.document_id
            LEFT OUTER JOIN  TransMatch tm ON t.transdetail_id = tm.transdetail_id
            LEFT OUTER JOIN  Transaction_Export_Folder tef          ON tef.document_ref = d.document_ref
                AND tef.insurance_ref = t.insurance_ref
where ( @iBranchID= 0
              or    (   @iBranchID <> 0 and a.company_id = @iBranchID ))
    END
    ELSE
    /* Get specific Ledger type */
    BEGIN
        INSERT INTO #tempRSAAcntListing
            SELECT l.ledger_id,
                l.ledger_short_name,
                l.ledger_name,
                a.account_key,
                a.account_id,
                a.short_code,
                a.account_name,
                a.address1,
                a.address2,
                a.address3,
                a.address4,
                d.documenttype_id,
                (SELECT dt.description
                    FROM documenttype dt
                    WHERE dt.documenttype_id = d.documenttype_id),
                d.document_ref,
                d.document_date,
                d.created_date,
                CASE WHEN d.documenttype_id IN (22, 23)
                    THEN
                        'cash'
                    ELSE
                        ''
                    END iscash,
                t.insurance_ref,
                t.amount,
                t.fully_matched,
                (SELECT t.amount WHERE spare <> 'COMM'),
                tm.base_match_amount,
                (SELECT t.amount WHERE spare = 'COMM'),
                @dtCurrentPeriodEnd,
                tef.accounts_export_status,
                (SELECT dt.from_sirius FROM documenttype dt WHERE d.documenttype_id = dt.documenttype_id),
                @UWType
            FROM Account a
            JOIN Ledger l ON a.ledger_id = l.ledger_id
                AND l.ledger_name = @LedgerName
            LEFT OUTER JOIN transdetail t   ON a.account_id = t.account_id
                AND t.period_id BETWEEN @CurrentYearStartPeriodID AND @CurrentPeriodID
            LEFT OUTER JOIN  Document d     ON t.document_id = d.document_id
            LEFT OUTER JOIN  TransMatch tm  ON t.transdetail_id = tm.transdetail_id
            LEFT OUTER JOIN  Transaction_Export_Folder tef          ON tef.document_ref = d.document_ref
                AND tef.insurance_ref = t.insurance_ref
where ( @iBranchID= 0
              or    (   @iBranchID <> 0 and a.company_id = @iBranchID ))
    END

END
ELSE
-- Get account details only
BEGIN

 IF @LedgerName = 'ALL'
    /* Get all Ledger types */
    BEGIN
        INSERT INTO #tempRSAAcntListing
            SELECT l.ledger_id,
                l.ledger_short_name,
                l.ledger_name,
                a.account_key,
                a.account_id,
                a.short_code,
                a.account_name,
                a.address1,
                a.address2,
                a.address3,
                a.address4,
                NULL,
                NULL,
                NULL,
                NULL,
                NULL,
                NULL,
                NULL,
                NULL,
                NULL,
                NULL,
                NULL,
                NULL,
                NULL,
                NULL,
                NULL,
                @UWType
            FROM Account a
            JOIN Ledger l ON a.ledger_id = l.ledger_id
where ( @iBranchID= 0
              or    (   @iBranchID <> 0 and a.company_id = @iBranchID ))
    END
    ELSE
    /* Get specific Ledger type */
    BEGIN
        INSERT INTO #tempRSAAcntListing
            SELECT l.ledger_id,
                l.ledger_short_name,
                l.ledger_name,
                a.account_key,
                a.account_id,
                a.short_code,
                a.account_name,
                a.address1,
                a.address2,
                a.address3,
                a.address4,
                NULL,
                NULL,
                NULL,
                NULL,
                NULL,
                NULL,
                NULL,
                NULL,
                NULL,
                NULL,
                NULL,
                NULL,
                NULL,
                NULL,
                NULL,
                @UWType
            FROM Account a
            JOIN Ledger l ON a.ledger_id = l.ledger_id
                AND l.ledger_name = @LedgerName
where ( @iBranchID= 0
              or    (   @iBranchID <> 0 and a.company_id = @iBranchID ))
    END

END

UPDATE #tempRSAAcntListing
    SET AccountName = (SELECT c.resolved_name from Party c where c.party_cnt = p.party_cnt)
    FROM #tempRSAAcntListing al
    JOIN Party p  ON al.AccountCode = p.shortname
    WHERE LedgerName = 'Client'

SET NOCOUNT OFF
-- Squirt it all out to the report
SELECT * FROM #tempRSAAcntListing
WHERE ((@ReportFormat = 'Long'
        AND ExportStatus = 'c'
        AND FromSirius = 1)
OR (@ReportFormat = 'Long'
        AND FromSirius = 0)
OR @ReportFormat <> 'Long')AND (@iAgentCnt = 0 or (@iAgentCnt <> 0 AND @iAgentCnt = AccountKey))

DROP TABLE #tempRSAAcntListing
GO
