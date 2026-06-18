SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Profit_And_Loss'
GO


CREATE PROCEDURE spu_Report_Profit_And_Loss
    @branch_id INT,
    @period_end_date DATETIME,
    @cost_centre CHAR(10)
AS

DECLARE 
    @dPeriodEndDateIn DATETIME,
    @dNextMonth DATETIME,
    @sYear CHAR(4),
    @sMonth CHAR(2),
    @dFirstDay DATETIME,
    @dLastDay DATETIME,
    @d2YearsAgoEndDate DATETIME,
    @d1stPeriodLastYearEndDate DATETIME,
    @dPrevPeriodLastYearEndDate DATETIME,
    @dThisPeriodLastYearEndDate DATETIME,
    @dLastYearEndDate DATETIME,
    @d1stPeriodEndDate DATETIME,
    @dPrevPeriodEndDate DATETIME,
    @dPeriodEndDate DATETIME,
    @sYearNameThisYear VARCHAR(20),
    @sPeriodNameThisYear VARCHAR(20),
    @sYearNameLastYear VARCHAR(20),
    @sPeriodNameLastYear VARCHAR(20),
    @iBranchID INT,
    @iCompanyID SMALLINT,
    @PeriodCompanyID SMALLINT,
    @iAccountID INT,
    @nThisPeriod MONEY,
    @nPeriodBudget MONEY,
    @nYearToDate MONEY,
    @nBudgetToDate MONEY,
    @nPeriodLastYear MONEY,
    @nLastYearToDate MONEY,
    @iCostCentre INT

-- Set cost centre (department)
IF @cost_centre <> 'ALL'
    SELECT @iCostCentre = CostCentre_id
    FROM CostCentre
    WHERE code = @cost_centre
ELSE
    SELECT @iCostCentre = 0

-- Empty temporary tables
SET NOCOUNT ON

DELETE FROM Report_Transaction
DELETE FROM Report_TreePathNames


--MKW190603 PN4843  If multi accounting use node 0 (therefore all nodes). START
--    EXECUTE spu_Report_ShortTreePathNames 3
if exists(select null from hidden_options where option_number=16)
    begin
    EXECUTE spu_Report_ShortTreePathNames 0
    delete from report_treepathnames where element_name2 <> 'Profit and Loss' --only retrieve p@l nodes.
    end
else
    begin
        EXEC  spu_Report_FullTreePathNames 3
    end
--MKW190603 PN4843  If multi accounting use node 0 (therefore all nodes). END

SELECT @dPeriodEndDateIn = ISNULL(@period_end_date, GETDATE())
SELECT @iBranchID = ISNULL(@branch_id, 0)

---Declare the company cursor
DECLARE cCompany CURSOR FAST_FORWARD FOR
SELECT company_id
FROM Company
WHERE ( @iBranchID = 0
        OR
        ( @iBranchID <> 0
            AND
            company_id = @iBranchID
        )
    )

OPEN cCompany

-- Get the transactions for each company
FETCH NEXT FROM cCompany INTO @iCompanyID

---PN 39963

IF EXISTS (SELECT NULL FROM hidden_options WHERE option_number=16)  
  
        BEGIN  
              /*For multi-ledger sites use periods defined for each branch.*/  
  
            SELECT @PeriodCompanyID = @iCompanyID  
        END  
  
   ELSE  
  
        BEGIN  
              /*For single-ledger sites use periods set up for branch one.*/  
  
            SELECT @PeriodCompanyID = 1  
        END  
 
WHILE @@FETCH_STATUS = 0 BEGIN
-- If Period end date is not found, default to end of input month
    SELECT @dNextMonth = DATEADD(month, 1, @dPeriodEndDateIn)
    SELECT @sYear = CONVERT(char(4), DATEPART(year, @dNextMonth))
    SELECT @sMonth = CONVERT(char(2), DATEPART(month, @dNextMonth))
    SELECT @dFirstDay = CONVERT(datetime, @sYear + '-' + @sMonth + '-01 23:59:59')
    SELECT @dLastDay = DATEADD(day, -1, @dFirstDay)

    SELECT @dPeriodEndDate = (SELECT ISNULL(MIN(period_end_date), @dLastDay)
                  FROM Period
                  WHERE company_id = @PeriodCompanyID
                  AND period_end_date >= @dPeriodEndDateIn)

-- If Previous period is not found, default will be set to last day in previous month.


    SELECT @sYear = CONVERT(char(4), DATEPART(year, @dPeriodEndDate))
    SELECT @sMonth = CONVERT(char(2), DATEPART(month, @dPeriodEndDate))
    SELECT @dFirstDay = CONVERT(datetime, @sYear + '-' + @sMonth + '-01 23:59:59')
    SELECT @dLastDay = DATEADD(day, -1, @dFirstDay)

    SELECT @dPrevPeriodEndDate = (SELECT ISNULL(MAX(period_end_date), @dLastDay)
                      FROM Period
                      WHERE company_id = @PeriodCompanyID
                      AND period_end_date < @dPeriodEndDate)
--Eck PN4095

    SELECT @sYearNameThisYear = (SELECT(ISNULL(year_name, '')) 
                            FROM Period
                            WHERE company_id = @PeriodCompanyID
                            AND period_end_date = @dPeriodEndDate
                AND period_id = (select min(p.period_id) 
                         from period p
                         where p.company_id = @PeriodCompanyID
                                     AND   p.period_end_date = @dPeriodEndDate)) 

-- If 1st period end date is not found set it to current period end date


   SELECT @d1stPeriodEndDate = (SELECT ISNULL(MIN(period_end_date), @dPeriodEndDate)
                     FROM Period
                     WHERE company_id = @PeriodCompanyID
                     AND year_name = @sYearNameThisYear)



-- If Last period last year not found set it to last day of month prior to 1st period end date
    SELECT @sYear = CONVERT(char(4), DATEPART(year, @d1stPeriodEndDate))
    SELECT @sMonth = CONVERT(char(2), DATEPART(month, @d1stPeriodEndDate))
    SELECT @dFirstDay = CONVERT(datetime, @sYear + '-' + @sMonth + '-01 23:59:59')
    SELECT @dLastDay = DATEADD(day, -1, @dFirstDay)

    SELECT @dLastYearEndDate = (SELECT ISNULL(MAX(period_end_date), @dLastDay)
                    FROM Period
                    WHERE company_id = @PeriodCompanyID
                    AND period_end_date < @d1stPeriodEndDate)


--Eck PN4095
    SELECT @sPeriodNameThisYear = (SELECT(ISNULL(period_name, '')) 
                            FROM Period
                            WHERE company_id = @PeriodCompanyID
                            AND period_end_date = @dPeriodEndDate
                AND period_id = (select min(p.period_id) 
                         from period p
                         where p.company_id = @PeriodCompanyID
                                     AND   p.period_end_date = @dPeriodEndDate))  


-- Default to 1 year before period end date
    SELECT @dThisPeriodLastYearEndDate = DATEADD(year, -1, @dPeriodEndDate)

-- If Previous period is not found, default will be set to last day in previous month.
    SELECT @sYear = CONVERT(char(4), DATEPART(year, @dThisPeriodLastYearEndDate))
    SELECT @sMonth = CONVERT(char(2), DATEPART(month, @dThisPeriodLastYearEndDate))
    SELECT @dFirstDay = CONVERT(datetime, @sYear + '-' + @sMonth + '-01 23:59:59')
    SELECT @dLastDay = DATEADD(day, -1, @dFirstDay)

    SELECT @dPrevPeriodLastYearEndDate = (SELECT ISNULL(MAX(period_end_date), @dLastDay)
                          FROM Period
                          WHERE company_id = @PeriodCompanyID
                          AND period_end_date < @dThisPeriodLastYearEndDate)


--Eck PN4095
    SELECT @sYearNameLastYear = (SELECT(ISNULL(year_name, '')) 
                            FROM Period
                            WHERE company_id = @PeriodCompanyID
                            AND period_end_date = @dThisPeriodLastYearEndDate
                AND period_id = (select min(p.period_id) 
                         from period p
                         where p.company_id = @PeriodCompanyID
                                     AND   p.period_end_date = @dThisPeriodLastYearEndDate))  

-- If 1st period last year end date not found, default to 1 year before 1st period end date
    SELECT @d1stPeriodLastYearEndDate = (SELECT ISNULL(MIN(period_end_date),
                                DATEADD(year, -1, @d1stPeriodEndDate))
                         FROM Period
                         WHERE company_id = @PeriodCompanyID
                         AND year_name = @sYearNameLastYear)

-- If Previous period is not found, default will be set to last day in previous month
    SELECT @sYear = CONVERT(char(4), DATEPART(year, @d1stPeriodLastYearEndDate))
    SELECT @sMonth = CONVERT(char(2), DATEPART(month, @d1stPeriodLastYearEndDate))
    SELECT @dFirstDay = CONVERT(datetime, @sYear + '-' + @sMonth + '-01 23:59:59')
    SELECT @dLastDay = DATEADD(day, -1, @dFirstDay)

    SELECT @d2YearsAgoEndDate = (SELECT ISNULL(MAX(period_end_date), @dLastDay)
                     FROM Period
                     WHERE company_id = @PeriodCompanyID
                     AND period_end_date < @d1stPeriodLastYearEndDate)

-- Get all accounts



     INSERT INTO Report_Transaction(
        account_id,
        account_name,
        account_code,
        extra_char1, /* This Period name */
        extra_datetime1, /* 2 years ago end date */
        extra_datetime2, /* prev period last year end date */
        extra_datetime3, /* this period last year end date */
        extra_datetime4, /* last year end date */
        extra_datetime5, /* 1st period end date */
        extra_datetime6, /* prev period end date */
        extra_datetime7, /* period end date */
        amount, /* This period actual */
        extra_numeric1, /* This period budget */
        extra_numeric2, /* Year to date actual */
        extra_numeric3, /* Year to date budget */
        extra_numeric4, /* This period last year */
        extra_numeric5, /* Year to date last year */
        ledger_type,
        account_type)
    SELECT Account.account_id,
        Account.account_name,
        Account.short_code,
        @sPeriodNameThisYear,
        @d2YearsAgoEndDate,
        @dPrevPeriodLastYearEndDate,
        @dThisPeriodLastYearEndDate,
        @dLastYearEndDate,
        @d1stPeriodEndDate,
        @dPrevPeriodEndDate,
        @dPeriodEndDate,
        0.0,
        0.0,
        0.0,
        0.0,
        0.0,
        0.0,
        ISNULL(LedgerType.description, ''),
        ISNULL(AccountType.description, '')
    FROM Report_TreePathNames RTPN
    JOIN Account Account
    ON RTPN.account_id = Account.account_id
    LEFT OUTER JOIN Ledger Ledger
    ON Account.ledger_id = Ledger.ledger_id
    LEFT OUTER JOIN LedgerType LedgerType
    ON Ledger.ledgertype_id = LedgerType.ledgertype_id
    LEFT OUTER JOIN AccountType AccountType
    ON Account.accounttype_id = AccountType.accounttype_id

    FETCH NEXT FROM cCompany INTO @iCompanyID

END

CLOSE cCompany
DEALLOCATE cCompany



-- Process accounts
DECLARE cAccount CURSOR FAST_FORWARD FOR
    SELECT account_id,
        extra_datetime1,
        extra_datetime2,
        extra_datetime3,
        extra_datetime4,
        extra_datetime5,
        extra_datetime6,
        extra_datetime7
    FROM Report_Transaction

OPEN cAccount

FETCH NEXT FROM cAccount INTO @iAccountID,
                @d2YearsAgoEndDate,
                @dPrevPeriodLastYearEndDate,
                @dThisPeriodLastYearEndDate,
                @dLastYearEndDate,
                @d1stPeriodEndDate,
                @dPrevPeriodEndDate,
                @dPeriodEndDate

WHILE @@FETCH_STATUS = 0 BEGIN

    /*This period Actual*/
    SELECT 
        @nThisPeriod = 
            (
                SELECT ISNULL(SUM(ROUND(ISNULL(TransDetail.amount, 0.0),2)), 0.0)
                FROM TransDetail TransDetail
                JOIN Document Document
                    ON TransDetail.document_id = Document.document_id
                WHERE Document.document_date > @dPrevPeriodEndDate
                AND Document.document_date <= @dPeriodEndDate
                AND TransDetail.account_id = @iAccountID
                AND Document.company_id = ISNULL(@branch_id, Document.company_id)
                AND Transdetail.spare NOT IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
                AND ( 
                        @iCostCentre = 0
                        OR
                        ( 
                            @iCostCentre <> 0
                            AND
                            TransDetail.department_id = @iCostCentre
                        )
                    )
                AND ISNULL(TransDetail.comment,'') <> 'Year End Retained Profit'
            ) 
            +
            (
                SELECT ISNULL(SUM(ROUND(ISNULL(TransDetail.amount, 0.0),2)), 0.0)
                FROM TransDetail TransDetail
                JOIN Document Document
                    ON TransDetail.document_id = Document.document_id
                WHERE Transdetail.ref_date > @dPrevPeriodEndDate
                AND Transdetail.ref_date <= @dPeriodEndDate
                AND TransDetail.account_id = @iAccountID
                AND Document.company_id = ISNULL(@branch_id, Document.company_id)
                AND Transdetail.spare IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
                AND ( 
                        @iCostCentre = 0
                        OR
                        (
                            @iCostCentre <> 0
                            AND
                            TransDetail.department_id = @iCostCentre
                        )
                    )
                AND ISNULL(TransDetail.comment,'') <> 'Year End Retained Profit'
            )

    /*This period budget*/
    SELECT 
        @nPeriodBudget = ISNULL(SUM(ROUND(ISNULL(BD.budget_amount, 0.0),2)), 0.0)
    FROM Budget B
    JOIN Budget_Detail BD
        ON BD.budget_id = B.budget_id
    JOIN Period P
        ON P.period_id = BD.period_id
    WHERE P.period_end_date = @dPeriodEndDate
    AND BD.account_id = @iAccountID
    AND B.budget_id = 
        (
            SELECT MAX(BX.budget_id)
            FROM Budget BX
            JOIN Budget_Detail BDX
                ON BDX.budget_id = BX.budget_id
            WHERE BDX.period_id = P.period_id
            AND BDX.account_id = BD.account_id
        )


    /*Year to date actual*/
    SELECT 
        @nYearToDate = 
            (
                SELECT ISNULL(SUM(ROUND(ISNULL(TransDetail.amount, 0.0),2)), 0.0)
                FROM TransDetail TransDetail
                JOIN Document Document
                    ON TransDetail.document_id = Document.document_id
                WHERE Document.document_date > @dLastYearEndDate
                AND Document.document_date <= @dPeriodEndDate
                AND TransDetail.account_id = @iAccountID
                AND Document.company_id = ISNULL(@branch_id, Document.company_id)
                AND Transdetail.spare NOT IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
                AND (
                        @iCostCentre = 0
                        OR
                        (
                            @iCostCentre <> 0
                            AND
                            TransDetail.department_id = @iCostCentre
                        )
                    )
                AND ISNULL(TransDetail.comment,'') <> 'Year End Retained Profit'
            )
            +
            (
                SELECT ISNULL(SUM(ROUND(ISNULL(TransDetail.amount, 0.0),2)), 0.0)
                FROM TransDetail TransDetail
                JOIN Document Document
                    ON TransDetail.document_id = Document.document_id
                WHERE Transdetail.ref_date > @dLastYearEndDate
                AND Transdetail.ref_date <= @dPeriodEndDate
                AND TransDetail.account_id = @iAccountID
                AND Document.company_id = ISNULL(@branch_id, Document.company_id)
                AND Transdetail.spare IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
                AND ( 
                        @iCostCentre = 0
                        OR
                        (
                            @iCostCentre <> 0
                            AND
                            TransDetail.department_id = @iCostCentre
                        )
                    )
                AND ISNULL(TransDetail.comment,'') <> 'Year End Retained Profit'
            )

    /*Year to date budget*/
    SELECT 
        @nBudgetToDate = ISNULL(SUM(ROUND(ISNULL(BD.budget_amount, 0.0),2)), 0.0)
    FROM Budget B
    JOIN Budget_Detail BD
        ON BD.budget_id = B.budget_id
    JOIN Period P
        ON P.period_id = BD.period_id
    WHERE P.period_end_date BETWEEN @d1stPeriodEndDate AND @dPeriodEndDate
    AND BD.account_id = @iAccountID
    AND B.budget_id = 
        (
            SELECT MAX(BX.budget_id)
            FROM Budget BX
            JOIN Budget_Detail BDX
                ON BDX.budget_id = BX.budget_id
            WHERE BDX.period_id = P.period_id
            AND BDX.account_id = BD.account_id
        )

    /*This period last year*/
    SELECT 
        @nPeriodLastYear = 
            (
                SELECT ISNULL(SUM(ROUND(ISNULL(TransDetail.amount, 0.0),2)), 0.0)
                FROM TransDetail TransDetail
                JOIN Document Document
                    ON TransDetail.document_id = Document.document_id
                WHERE Document.document_date > @dPrevPeriodLastYearEndDate
                AND Document.document_date <= @dThisPeriodLastYearEndDate
                AND TransDetail.account_id = @iAccountID
                AND Document.company_id = ISNULL(@branch_id, Document.company_id)
                AND Transdetail.spare NOT IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
                AND (
                        @iCostCentre = 0
                        OR
                        (
                            @iCostCentre <> 0
                            AND
                            TransDetail.department_id = @iCostCentre
                        )
                    )
                AND ISNULL(TransDetail.comment,'') <> 'Year End Retained Profit'
            )
            +
            (
                SELECT ISNULL(SUM(ROUND(ISNULL(TransDetail.amount, 0.0),2)), 0.0)
                FROM TransDetail TransDetail
                JOIN Document Document
                    ON TransDetail.document_id = Document.document_id
                WHERE Transdetail.ref_date > @dPrevPeriodLastYearEndDate
                AND Transdetail.ref_date <= @dThisPeriodLastYearEndDate
                AND TransDetail.account_id = @iAccountID
                AND Document.company_id = ISNULL(@branch_id, Document.company_id)
                AND Transdetail.spare IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
                AND ( 
                    @iCostCentre = 0
                    OR
                    (
                        @iCostCentre <> 0
                        AND
                        TransDetail.department_id = @iCostCentre
                    )
                )
                AND ISNULL(TransDetail.comment,'') <> 'Year End Retained Profit'
            )

    /*Year to date last year*/
    SELECT 
        @nLastYearToDate = 
            (
                SELECT ISNULL(SUM(ROUND(ISNULL(TransDetail.amount, 0.0),2)), 0.0)
                FROM TransDetail TransDetail
                JOIN Document Document
                    ON TransDetail.document_id = Document.document_id
                WHERE Document.document_date > @d2YearsAgoEndDate
                AND Document.document_date <= @dThisPeriodLastYearEndDate
                AND TransDetail.account_id = @iAccountID
                AND Document.company_id = ISNULL(@branch_id, Document.company_id)
                AND Transdetail.spare NOT IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
                AND (
                        @iCostCentre = 0
                        OR
                        (
                            @iCostCentre <> 0
                            AND
                            TransDetail.department_id = @iCostCentre
                        )
                    )
                AND ISNULL(TransDetail.comment,'') <> 'Year End Retained Profit'
            )
            +
            (
                SELECT ISNULL(SUM(ROUND(ISNULL(TransDetail.amount, 0.0),2)), 0.0)
                FROM TransDetail TransDetail
                JOIN Document Document
                ON TransDetail.document_id = Document.document_id
                WHERE Transdetail.ref_date > @d2YearsAgoEndDate
                AND Transdetail.ref_date <= @dThisPeriodLastYearEndDate
                AND TransDetail.account_id = @iAccountID
                AND Document.company_id = ISNULL(@branch_id, Document.company_id)
                AND Transdetail.spare IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
                AND (
                        @iCostCentre = 0
                        OR
                        (
                            @iCostCentre <> 0
                            AND
                            TransDetail.department_id = @iCostCentre
                        )
                    )
                AND ISNULL(TransDetail.comment,'') <> 'Year End Retained Profit'
            )

    /*Add values to Record_Transaction table for account*/
    UPDATE Report_Transaction
    SET amount = @nThisPeriod,
        extra_numeric1 = @nPeriodBudget,
        extra_numeric2 = @nYearToDate,
        extra_numeric3 = @nBudgetToDate,
        extra_numeric4 = @nPeriodLastYear,
        extra_numeric5 = @nLastYearToDate
    WHERE account_id = @iAccountID

    FETCH NEXT FROM cAccount INTO
                    @iAccountID,
                    @d2YearsAgoEndDate,
                    @dPrevPeriodLastYearEndDate,
                    @dThisPeriodLastYearEndDate,
                    @dLastYearEndDate,
                    @d1stPeriodEndDate,
                    @dPrevPeriodEndDate,
                    @dPeriodEndDate

END

CLOSE cAccount
DEALLOCATE cAccount

/*Extract data*/
SET NOCOUNT OFF

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
    RT.Amount this_period_amount,
    RT.extra_numeric1 this_period_budget,
    RT.extra_numeric2 this_year_to_date_amount,
    RT.extra_numeric3 this_year_to_date_budget,
    RT.extra_numeric4 last_year_period_amount,
    RT.extra_numeric5 last_year_to_date_amount,
    RT.account_name,
    RT.account_code short_code,
    RT.extra_char1 this_period_name,
    RT.extra_datetime1 two_years_ago_end_date,
    RT.extra_datetime2 prev_period_last_year_end_date,
    RT.extra_datetime3 this_period_last_year_end_date,
    RT.extra_datetime4 last_year_end_date,
    RT.extra_datetime5 first_period_end_date,
    RT.extra_datetime6 prev_period_end_date,
    RT.extra_datetime7 period_end_date,
    RT.ledger_type,
    RT.account_type
FROM Report_TreePathNames RTPN
JOIN Report_Transaction RT
    ON RTPN.account_id = RT.account_id
ORDER BY Report_Map_Id1, Report_Map_Id2, Report_Map_Id3, Report_Map_Id4, Report_Map_Id5, Report_Map_Id6, Report_Map_Id7, Report_Map_Id8, Report_Map_Id9, Report_Map_Id10, short_code

/*Empty temporary tables*/
SET NOCOUNT ON
DELETE FROM Report_Transaction
DELETE FROM Report_TreePathNames
SET NOCOUNT OFF


 

GO

