SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Balance_Sheet'
GO

CREATE PROCEDURE spu_Report_Balance_Sheet
    @branch_id INT,
    @period_end_date DATETIME
AS

DECLARE 
    @dPeriodEndDateIn DATETIME,
    @dNextMonth DATETIME,
    @sYear VARCHAR(4),
    @sMonth VARCHAR(2),
    @dFirstDay DATETIME,
    @dLastDay DATETIME,
    @dPeriodEndDate DATETIME,
    @d1stPeriodStartDate DATETIME,
    @d1stPeriodEndDate DATETIME,
    @dPrevPeriodEndDate DATETIME,
    @sYearName VARCHAR(20),
    @iCompanyID SMALLINT,
    @iSubBranchID INT,
    @RootnodeID INT

/*Empty temporary tables*/
CREATE TABLE #Report_Transaction(
	transdetail_id int NULL,
	amount numeric(19, 4) NULL,
	document_sequence smallint NULL,
	policy_number varchar(30) NULL,
	branch_id int NULL,
	comment varchar(60) NULL,
	document_ref varchar(25) NULL,
	document_date datetime NULL,
	documenttype_id int NULL,
	account_id int NULL,
	account_code char(30) NULL,
	account_name varchar(100) NULL,
	account_type varchar(100) NULL,
	ledger_type varchar(100) NULL,
	branch_name varchar(100) NULL,
	period_id int NULL,
	record_type smallint NULL,
	transdetail_id2 int NULL,
	amount2 numeric(19, 4) NULL,
	document_sequence2 smallint NULL,
	policy_number2 varchar(30) NULL,
	branch_id2 int NULL,
	comment2 varchar(60) NULL,
	account_id2 int NULL,
	account_code2 char(20) NULL,
	account_name2 varchar(100) NULL,
	account_type2 varchar(100) NULL,
	ledger_type2 varchar(100) NULL,
	branch_name2 varchar(100) NULL,
	period_id2 int NULL,
	record_type2 smallint NULL,
	extra_char1 varchar(100) NULL,
	extra_char2 varchar(255) NULL,
	extra_char3 varchar(100) NULL,
	extra_char4 varchar(255) NULL,
	extra_char5 varchar(100) NULL,
	extra_char6 varchar(100) NULL,
	extra_char7 varchar(100) NULL,
	extra_int1 int NULL,
	extra_int2 int NULL,
	extra_int3 int NULL,
	extra_int4 int NULL,
	extra_int5 int NULL,
	extra_int6 int NULL,
	extar_int7 int NULL,
	extra_datetime1 datetime NULL,
	extra_datetime2 datetime NULL,
	extra_datetime3 datetime NULL,
	extra_datetime4 datetime NULL,
	extra_datetime5 datetime NULL,
	extra_datetime6 datetime NULL,
	extra_datetime7 datetime NULL,
	extra_numeric1 numeric(19, 4) NULL,
	extra_numeric2 numeric(19, 4) NULL,
	extra_numeric3 numeric(19, 4) NULL,
	extra_numeric4 numeric(19, 4) NULL,
	extra_numeric5 numeric(19, 4) NULL,
	extra_numeric6 numeric(19, 4) NULL,
	extra_numeric7 numeric(19, 4) NULL
) 



IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

BEGIN TRAN    
TRUNCATE TABLE Report_TreePathNames

SELECT @RootNodeID = 
    (
        SELECT 
            node_id 
        FROM structuretree
        WHERE mapping_id IS NULL
        AND account_id IS NULL
        AND node_id <> 0
        AND parent_node_id = 0
        AND company_id = @branch_id
    )

SELECT @RootNodeID = ISNULL(@RootNodeid, 1)   

EXECUTE spu_Report_BS_TreePathNames @rootnodeId

SELECT * INTO #Report_TreePathNames FROM Report_TreePathNames
TRUNCATE TABLE Report_TreePathNames

COMMIT TRAN

SELECT @dPeriodEndDateIn = ISNULL(@period_end_date, GETDATE())

EXEC spu_sub_branch_default @source_id=@iCompanyID, @sub_branch_id=@iSubBranchID OUTPUT

DECLARE cCompany CURSOR FAST_FORWARD FOR
    SELECT 
        company_id 
    FROM company
    WHERE company_id = ISNULL(@branch_id, company_id)


OPEN cCompany

-- Get the transactions for each company
FETCH NEXT FROM cCompany INTO @iCompanyID

WHILE @@FETCH_STATUS = 0 BEGIN
    -- If Period end date is not found, default to end of input month
    SELECT @dNextMonth = DATEADD(MONTH, 1, @dPeriodEndDateIn)
    SELECT @sYear = CONVERT(CHAR(4), DATEPART(YEAR, @dNextMonth))
    SELECT @sMonth = CONVERT(CHAR(2), DATEPART(MONTH, @dNextMonth))
    SELECT @dFirstDay = CONVERT(DATETIME, @sYear + '-' + @sMonth + '-01 23:59:59')
    SELECT @dLastDay = DATEADD(DAY, -1, @dFirstDay)

    SELECT 
        @dPeriodEndDate = ISNULL(MIN(period_end_date), @dLastDay)
    FROM period
    WHERE company_id = @iCompanyID
    AND period_end_date >= @dPeriodEndDateIn

    SELECT 
        @sYearName = ISNULL(year_name, '')
    FROM period
    WHERE company_id = @iCompanyID
    AND period_end_date = @dPeriodEndDate
    AND sub_branch_id = @iSubBranchID

-- If 1st Period End Date not found set it to the current period end date
    SELECT 
        @d1stPeriodEndDate = ISNULL(MIN(period_end_date), @dPeriodEndDate)
    FROM period
    WHERE company_id = @iCompanyID
    AND year_name = @sYearName

-- If previous period end date not found set it to end of previous month
    SELECT @sYear = CONVERT(CHAR(4), DATEPART(YEAR, @dPeriodEndDate))
    SELECT @sMonth = CONVERT(CHAR(2), DATEPART(MONTH, @dPeriodEndDate))

    SELECT @dFirstDay = CONVERT(DATETIME, @sYear + '-' + @sMonth + '-01 23:59:59')
    SELECT @dLastDay = DATEADD(DAY, -1, @dFirstDay)

    SELECT 
        @dPrevPeriodEndDate = ISNULL(MAX(period_end_date), @dLastDay)
    FROM period
    WHERE company_id = @iCompanyID
    AND period_end_date < @d1stPeriodEndDate

    INSERT INTO #Report_Transaction
    (
        transdetail_id,
        amount,
        account_id,
        account_name,
        account_code,
        extra_char1,
        extra_datetime1,
        document_date,
        documenttype_id,
        ledger_type,
        account_type
    )
    SELECT 
        td.transdetail_id,
        ROUND(td.amount,2),
        a.account_id,
        a.account_name,
        a.short_code,
        p.period_name,
        p.period_end_date,
        d.document_date,
        d.documenttype_id,
        lt.description,
        at.description
    FROM  #Report_TreePathNames rtpn
    JOIN account a
        ON a.account_id = rtpn.account_id
    JOIN transdetail td
        ON td.account_id = a.account_id
        AND td.company_id = @icompanyid
    JOIN period p
        ON p.period_id = td.period_id
    JOIN document d
        ON d.document_id = td.document_id
    JOIN accounttype at
        ON at.accounttype_id = a.accounttype_id
    JOIN ledger l
        ON l.ledger_id = a.ledger_id
    JOIN ledgertype lt
        ON lt.ledgertype_id = l.ledgertype_id
    JOIN elementextras ee
        ON ee.report_map_id = rtpn.report_map_id2
    WHERE ee.element_id in (SELECT element_id FROM element WHERE element_name in ('Balance Sheet','Profit And Loss'))
    AND
    (
        (
            d.document_date <= @dPeriodEndDate
            AND
            td.spare NOT IN ('AGENT ADJ', 'COMM ADJ', 'BROK ADJ')
        )
        OR
        ( 
            td.ref_date <= @dPeriodEndDate
            AND
            td.spare IN ('AGENT ADJ', 'COMM ADJ', 'BROK ADJ')
        )
    )

    INSERT INTO #Report_Transaction
    (
        transdetail_id,
        amount,
        account_id,
        account_name,
        account_code,
        extra_char1,
        extra_datetime1,
        document_date,
        documenttype_id,
        ledger_type,
        account_type
    )
    SELECT 
        0,
        0.0,
        a.account_id,
        a.account_name,
        a.short_code,
        '',
        @dPeriodEndDate,
        NULL,
        0,
        lt.description,
        at.description
    FROM  #Report_TreePathNames rtpn
    JOIN account a
        ON a.account_id = rtpn.account_id
    JOIN ledger l
        ON l.ledger_id = a.ledger_id
    JOIN ledgertype lt
        ON lt.ledgertype_id = l.ledgertype_id
    JOIN accounttype at
        ON at.accounttype_id = a.accounttype_id
    WHERE NOT EXISTS 
        (
            SELECT 
                NULL
            FROM #Report_Transaction
            WHERE account_id = rtpn.account_id 
        )

    FETCH NEXT FROM cCompany INTO @iCompanyID

END

CLOSE cCompany
DEALLOCATE cCompany

/*Select the return details*/
SELECT 
    RTPN.element_name1,
    RTPN.Report_Map_Id1,
    RTPN.element_name2,
    RTPN.Report_Map_Id2,
    RTPN.element_name3,
    RTPN.Report_Map_Id3,
    RTPN.element_name4,
    RTPN.Report_Map_Id4,
    RTPN.element_name5,
    RTPN.Report_Map_Id5,
    RTPN.element_name6,
    RTPN.Report_Map_Id6,
    RTPN.element_name7,
    RTPN.Report_Map_Id7,
    RTPN.element_name8,
    RTPN.Report_Map_Id8,
    RTPN.element_name9,
    RTPN.Report_Map_Id9,
    RTPN.element_name10,
    RTPN.Report_Map_Id10,
    ROUND(RT.Amount,2),
    RT.account_name,
    RT.account_code short_code,
    RT.extra_char1 period_name,
    RT.extra_datetime1 period_end_date,
    RT.document_date,
    RT.documenttype_id,
    RT.transdetail_id,
    RT.ledger_type,
    RT.account_type
FROM  #Report_TreePathNames RTPN
JOIN #Report_Transaction RT
    ON RTPN.account_id = RT.account_id
JOIN ElementExtras EE
    ON EE.report_map_id = RTPN.report_map_id2
WHERE EE.element_id IN (SELECT element_id FROM element WHERE element_name in ('Balance Sheet','Profit And Loss'))
ORDER BY Report_Map_Id1, Report_Map_Id2, Report_Map_Id3, Report_Map_Id4, Report_Map_Id5, Report_Map_Id6, Report_Map_Id7, Report_Map_Id8, Report_Map_Id9, Report_Map_Id10, short_code

/*Empty temporary tables*/
DROP TABLE #Report_Transaction
DROP TABLE  #Report_TreePathNames

GO

