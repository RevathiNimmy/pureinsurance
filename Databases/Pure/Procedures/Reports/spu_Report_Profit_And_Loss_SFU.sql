SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Report_Profit_And_Loss_SFU'
GO
CREATE PROCEDURE spu_Report_Profit_And_Loss_SFU
    @branch_id int,
    @period_end_date datetime,
    @cost_centre char(10),
    @TypeOfCurrency	varchar(50),
    @GroupByCode	Varchar(50)

AS

DECLARE @dPeriodEndDateIn datetime,
    @dNextMonth datetime,
    @sYear char(4),
    @sMonth char(2),
    @dFirstDay datetime,
    @dLastDay datetime,
    @d2YearsAgoEndDate datetime,
    @d1stPeriodLastYearEndDate datetime,
    @dPrevPeriodLastYearEndDate datetime,
    @dThisPeriodLastYearEndDate datetime,
    @dLastYearEndDate datetime,
    @d1stPeriodEndDate datetime,
    @dPrevPeriodEndDate datetime,
    @dPeriodEndDate datetime,
    @sYearNameThisYear varchar(20),
    @sPeriodNameThisYear varchar(20),
    @sYearNameLastYear varchar(20),
    @sPeriodNameLastYear varchar(20),
    @iBranchID int,
    @iCompanyID smallint,
    @iAccountID int,
    @nThisPeriod numeric(19, 4),
    @nPeriodBudget numeric(19, 4),
    @nYearToDate numeric(19, 4),
    @nBudgetToDate numeric(19, 4),
    @nPeriodLastYear numeric(19, 4),
    @nLastYearToDate numeric(19, 4),
    @iCostCentre int,
    @iCompanyCode Varchar(50),
    @iCompanyDesc	Varchar(500),
    @iBaseCurrency	Varchar(255),
    @CurrCompanyId	INT,
    @BranchId		INT

    /*Get System Currency Details*/
    DECLARE @SystemCurrencyCode varchar(15)
	DECLARE @SystemCurrencyDesc varchar(255)
		SELECT
			@SystemCurrencyCode = c.iso_code,
			@SystemCurrencyDesc = c.description
		FROM PMSystem pms
		JOIN currency c
			ON c.currency_id = pms.currency_id
		WHERE pms.system_id = 1
	/*******************/

    IF @cost_centre <> 'ALL'
        SELECT @iCostCentre = CostCentre_id
        FROM CostCentre
        WHERE code = @cost_centre
    ELSE
        SELECT @iCostCentre = 0
    SET NOCOUNT ON
    
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

BEGIN TRAN
    
    DELETE FROM Report_TreePathNames
    if exists(select null from hidden_options where option_number=16)
        begin
	    EXECUTE spu_Report_ShortTreePathNames 0
	    delete from report_treepathnames where element_name2 <> 'Profit and Loss'
        end
    else
        begin
            EXEC  spu_Report_FullTreePathNames 3
        end
        
    SELECT * INTO #Report_TreePathNames FROM Report_TreePathNames
	DELETE FROM Report_TreePathNames
	COMMIT TRAN

   
        
    SELECT @dPeriodEndDateIn = ISNULL(@period_end_date, GETDATE())
    SELECT @iBranchID = ISNULL(@branch_id, 0)
    DECLARE cCompany CURSOR FAST_FORWARD FOR
    SELECT company_id,code,Description,base_currency
    FROM Company
    WHERE (
--                company_id = 1
			company_id = @iBranchID OR @iBranchID= 0
            )
    OPEN cCompany
    FETCH NEXT FROM cCompany INTO @iCompanyID,@iCompanyCode,@iCompanyDesc,@iBaseCurrency
    WHILE @@FETCH_STATUS = 0 BEGIN
        SELECT @dNextMonth = DATEADD(month, 1, @dPeriodEndDateIn)
        if not exists(select null from hidden_options where option_number=16)
	SET @BranchId = @iCompanyID
        SET @iCompanyID = 1
        SELECT @sYear = CONVERT(char(4), DATEPART(year, @dNextMonth))
        SELECT @sMonth = CONVERT(char(2), DATEPART(month, @dNextMonth))
        SELECT @dFirstDay = CONVERT(datetime, @sYear + '-' + @sMonth + '-01 23:59:59')
        SELECT @dLastDay = DATEADD(day, -1, @dFirstDay)
        SELECT @dPeriodEndDate = (SELECT ISNULL(MIN(period_end_date), @dLastDay)
                      FROM Period
                      WHERE company_id = @iCompanyID
                      AND period_end_date >= @dPeriodEndDateIn)
        SELECT @sYear = CONVERT(char(4), DATEPART(year, @dPeriodEndDate))
        SELECT @sMonth = CONVERT(char(2), DATEPART(month, @dPeriodEndDate))
        SELECT @dFirstDay = CONVERT(datetime, @sYear + '-' + @sMonth + '-01 23:59:59')
        SELECT @dLastDay = DATEADD(day, -1, @dFirstDay)
        SELECT @dPrevPeriodEndDate = (SELECT ISNULL(MAX(period_end_date), @dLastDay)
                          FROM Period
                          WHERE company_id = @iCompanyID
                          AND period_end_date < @dPeriodEndDate)
        SELECT @sYearNameThisYear = (SELECT(ISNULL(year_name, ''))
                         		FROM Period
                         		WHERE company_id = @iCompanyID
                         		AND period_end_date = @dPeriodEndDate
			 		AND period_id = (select min(p.period_id)
						     from period p
						     where p.company_id = @iCompanyID
                         			     AND   p.period_end_date = @dPeriodEndDate))
       SELECT @d1stPeriodEndDate = (SELECT ISNULL(MIN(period_end_date), @dPeriodEndDate)
                         FROM Period
                         WHERE company_id = @iCompanyID
                         AND year_name = @sYearNameThisYear)
        SELECT @sYear = CONVERT(char(4), DATEPART(year, @d1stPeriodEndDate))
        SELECT @sMonth = CONVERT(char(2), DATEPART(month, @d1stPeriodEndDate))
        SELECT @dFirstDay = CONVERT(datetime, @sYear + '-' + @sMonth + '-01 23:59:59')
        SELECT @dLastDay = DATEADD(day, -1, @dFirstDay)
        SELECT @dLastYearEndDate = (SELECT ISNULL(MAX(period_end_date), @dLastDay)
                        FROM Period
                        WHERE company_id = @iCompanyID
                        AND period_end_date < @d1stPeriodEndDate)
        SELECT @sPeriodNameThisYear = (SELECT(ISNULL(period_name, ''))
                         		FROM Period
                         		WHERE company_id = @iCompanyID
                         		AND period_end_date = @dPeriodEndDate
			 		AND period_id = (select min(p.period_id)
						     from period p
						     where p.company_id = @iCompanyID
                         			     AND   p.period_end_date = @dPeriodEndDate))
        SELECT @dThisPeriodLastYearEndDate = (SELECT ISNULL(MAX(period_end_date),
                                    DATEADD(year, -1, @dPeriodEndDate))
                              FROM Period
                              WHERE company_id = @iCompanyID 
                              AND period_end_date <= DATEADD(day, 1, DATEADD(year, -1, @dPeriodEndDate)))
        SELECT @sYear = CONVERT(char(4), DATEPART(year, @dThisPeriodLastYearEndDate))
        SELECT @sMonth = CONVERT(char(2), DATEPART(month, @dThisPeriodLastYearEndDate))
        SELECT @dFirstDay = CONVERT(datetime, @sYear + '-' + @sMonth + '-01 23:59:59')
        SELECT @dLastDay = DATEADD(day, -1, @dFirstDay)
        SELECT @dPrevPeriodLastYearEndDate = (SELECT ISNULL(MAX(period_end_date), @dLastDay)
                              FROM Period
                              WHERE company_id = @iCompanyID
                              AND period_end_date < @dThisPeriodLastYearEndDate)
        SELECT @sYearNameLastYear = (SELECT(ISNULL(year_name, ''))
                         		FROM Period
                         		WHERE company_id = @iCompanyID
                         		AND period_end_date = @dThisPeriodLastYearEndDate
			 		AND period_id = (select min(p.period_id)
						     from period p
						     where p.company_id = @iCompanyID
                         			     AND   p.period_end_date = @dThisPeriodLastYearEndDate))
        SELECT @d1stPeriodLastYearEndDate = (SELECT ISNULL(MIN(period_end_date),
                                    DATEADD(year, -1, @d1stPeriodEndDate))
                             FROM Period
                             WHERE company_id = @iCompanyID
                             AND year_name = @sYearNameLastYear)
        SELECT @sYear = CONVERT(char(4), DATEPART(year, @d1stPeriodLastYearEndDate))
        SELECT @sMonth = CONVERT(char(2), DATEPART(month, @d1stPeriodLastYearEndDate))
        SELECT @dFirstDay = CONVERT(datetime, @sYear + '-' + @sMonth + '-01 23:59:59')
        SELECT @dLastDay = DATEADD(day, -1, @dFirstDay)
        SELECT @d2YearsAgoEndDate = (SELECT ISNULL(MAX(period_end_date), @dLastDay)
                         FROM Period
                         WHERE company_id = @iCompanyID
                         AND period_end_date < @d1stPeriodLastYearEndDate)
         INSERT INTO #Report_Transaction(
            account_id,
            account_name,
            account_code,
            extra_char1,
            extra_char2,/*currencycode*/
            extra_char3,/*currencydesc*/
            extra_char4,/*companycode*/
            extra_char5,/*companydesc*/
            extra_datetime1,
            extra_datetime2,
            extra_datetime3,
            extra_datetime4,
            extra_datetime5,
            extra_datetime6,
            extra_datetime7,
            amount,
            extra_numeric1,
            extra_numeric2,
            extra_numeric3,
            extra_numeric4,
            extra_numeric5,
            ledger_type,
            account_type,
	    branch_id)
        SELECT Account.account_id,
            Account.account_name,
            Account.short_code,
            @sPeriodNameThisYear,
             CASE @TypeOfCurrency
				WHEN 'Base' THEN cb.iso_code
				WHEN 'System' THEN @SystemCurrencyCode
			END,
			CASE @TypeOfCurrency
				WHEN 'Base' THEN cb.description
				WHEN 'System' THEN @SystemCurrencyDesc
			END, --added later for multicurrency
			@iCompanyCode,@iCompanyDesc,
			@d2YearsAgoEndDate,
            @dPrevPeriodLastYearEndDate,
            @dThisPeriodLastYearEndDate,            @dLastYearEndDate,
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
            ISNULL(AccountType.description, ''),
	    @BranchId
        FROM #Report_TreePathNames RTPN
        JOIN Account Account
        ON RTPN.account_id = Account.account_id
        /* AND Account.company_id = @iCompanyID */
        LEFT OUTER JOIN Ledger Ledger
        ON Account.ledger_id = Ledger.ledger_id
        LEFT OUTER JOIN LedgerType LedgerType
        ON Ledger.ledgertype_id = LedgerType.ledgertype_id
        LEFT OUTER JOIN AccountType AccountType
        ON Account.accounttype_id = AccountType.accounttype_id
        LEFT OUTER JOIN Currency CB
        ON CB.Currency_id = @iBaseCurrency
        FETCH NEXT FROM cCompany INTO @iCompanyID,@iCompanyCode,@iCompanyDesc,@iBaseCurrency
    END
    CLOSE cCompany
    DEALLOCATE cCompany


    DECLARE cAccount CURSOR FAST_FORWARD FOR
        SELECT account_id,
            extra_datetime1,
            extra_datetime2,
            extra_datetime3,
            extra_datetime4,
            extra_datetime5,
            extra_datetime6,
            extra_datetime7,
	    branch_id
        FROM #Report_Transaction ORDER BY branch_id
    OPEN cAccount
    FETCH NEXT FROM cAccount INTO @iAccountID,
                    @d2YearsAgoEndDate,
                    @dPrevPeriodLastYearEndDate,
                    @dThisPeriodLastYearEndDate,
                    @dLastYearEndDate,
                    @d1stPeriodEndDate,
                    @dPrevPeriodEndDate,
                    @dPeriodEndDate,
		    @CurrCompanyId
    WHILE @@FETCH_STATUS = 0 BEGIN

    SET @branch_id = @CurrCompanyId
    SELECT @nThisPeriod = ( SELECT
    						Case @TypeOfCurrency
    							WHEN 'System' Then ISNULL(SUM(ROUND(ISNULL(TransDetail.System_amount, 0.0),2)), 0.0)
    							WHEN 'Base' Then ISNULL(SUM(ROUND(ISNULL(TransDetail.amount, 0.0),2)), 0.0)
    						END
                    FROM TransDetail TransDetail
                    JOIN Document Document
                    ON TransDetail.document_id = Document.document_id
                    WHERE Document.document_date > @dPrevPeriodEndDate
                    AND Document.document_date <= @dPeriodEndDate
                    AND TransDetail.account_id = @iAccountID
                    AND ( @Branch_id = 0
                            OR
                            ( @branch_id <> 0
                                AND
                                TransDetail.company_id = @branch_id
                            )
                        )
                    AND Transdetail.spare NOT IN ('COMM ADJ', 'AGENT ADJ')
                    AND TransDetail.Document_Sequence NOT IN
                            ( SELECT Document_Sequence + 1
                                FROM TransDetail
                                WHERE document_id = Document.document_id
                                AND spare = 'COMM ADJ'
                            )
                    AND ( @iCostCentre = 0
                            OR
                            ( @iCostCentre <> 0
                                AND
                                TransDetail.department_id = @iCostCentre
                            )
                        )
                ) +
                ( SELECT Case @TypeOfCurrency
                			When 'System' Then ISNULL(SUM(ROUND(ISNULL(TransDetail.system_amount, 0.0),2)), 0.0)
                			When 'Base' Then ISNULL(SUM(ROUND(ISNULL(TransDetail.amount, 0.0),2)), 0.0)
                		END
                    FROM TransDetail TransDetail
                    JOIN Document Document
                    ON TransDetail.document_id = Document.document_id
                    WHERE Transdetail.ref_date > @dPrevPeriodEndDate
                    AND Transdetail.ref_date <= @dPeriodEndDate
                    AND TransDetail.account_id = @iAccountID
                    AND ( @Branch_id = 0
                            OR
                            ( @branch_id <> 0
                                AND
                                TransDetail.company_id = @branch_id
                            )
                        )
                    AND ( Transdetail.spare IN ('COMM ADJ', 'AGENT ADJ')
                            OR
                            Transdetail.Document_Sequence IN
                                ( SELECT Document_Sequence + 1
                                    FROM TransDetail
                                    WHERE document_id = Document.document_id
                                    AND spare = 'COMM ADJ'
                                )
                        )
                    AND ( @iCostCentre = 0
                            OR
                            ( @iCostCentre <> 0
                                AND
                                TransDetail.department_id = @iCostCentre
                            )
                        )
                )
    SELECT @nPeriodBudget = ISNULL(SUM(ROUND(ISNULL(Budget_Detail.budget_amount, 0.0),2)), 0.0)
                    FROM Budget_Detail Budget_Detail
                    JOIN Period Period
                    ON Budget_Detail.period_id = Period.period_id
                    WHERE Period.period_end_date = @dPeriodEndDate
                    AND Budget_Detail.account_id = @iAccountID
    SELECT @nYearToDate = ( SELECT Case @TypeOfCurrency
    									When 'System' Then ISNULL(SUM(ROUND(ISNULL(TransDetail.amount, 0.0),2)), 0.0)
    									When 'Base' Then ISNULL(SUM(ROUND(ISNULL(TransDetail.amount, 0.0),2)), 0.0)
    								END
                    FROM TransDetail TransDetail
                    JOIN Document Document
                    ON TransDetail.document_id = Document.document_id
                    WHERE Document.document_date > @dLastYearEndDate
                    AND Document.document_date <= @dPeriodEndDate
                    AND TransDetail.account_id = @iAccountID
                    AND ( @Branch_id = 0
                            OR
                            ( @branch_id <> 0
                                AND
                                TransDetail.company_id = @branch_id
                            )
                        )
                    AND Transdetail.spare NOT IN ('COMM ADJ', 'AGENT ADJ')
                    AND TransDetail.Document_Sequence NOT IN
                            ( SELECT Document_Sequence + 1
                                FROM TransDetail
                                WHERE document_id = Document.document_id
                                AND spare = 'COMM ADJ'
                            )
                    AND ( @iCostCentre = 0
                            OR
                            ( @iCostCentre <> 0
                                AND
                                TransDetail.department_id = @iCostCentre
                            )
                        )
                ) +
                ( SELECT Case @TypeOfCurrency
                			When 'System' Then ISNULL(SUM(ROUND(ISNULL(TransDetail.system_amount, 0.0),2)), 0.0)
                			When 'Base' Then ISNULL(SUM(ROUND(ISNULL(TransDetail.amount, 0.0),2)), 0.0)
                		END
                    FROM TransDetail TransDetail
                    JOIN Document Document
                    ON TransDetail.document_id = Document.document_id
                    WHERE Transdetail.ref_date > @dLastYearEndDate
                    AND Transdetail.ref_date <= @dPeriodEndDate
                    AND TransDetail.account_id = @iAccountID
                    AND ( @Branch_id = 0
                            OR
                            ( @branch_id <> 0
                                AND
                                TransDetail.company_id = @branch_id
                            )
                        )
                    AND ( Transdetail.spare IN ('COMM ADJ', 'AGENT ADJ')
                            OR
                            Transdetail.Document_Sequence IN
                                ( SELECT Document_Sequence + 1
                                    FROM TransDetail
                                    WHERE document_id = Document.document_id
                                    AND spare = 'COMM ADJ'
                                )
                        )
                    AND ( @iCostCentre = 0
                            OR
                            ( @iCostCentre <> 0
                                AND
                                TransDetail.department_id = @iCostCentre
                            )
                        )
                )
    SELECT @nBudgetToDate = ISNULL(SUM(ROUND(ISNULL(Budget_Detail.budget_amount, 0.0),2)), 0.0)
                    FROM Budget_Detail Budget_Detail
                    JOIN Period Period
                    ON Budget_Detail.period_id = Period.period_id
                    WHERE Period.period_end_date >= @d1stPeriodEndDate
                    AND Period.period_end_date <= @dPeriodEndDate
                    AND Budget_Detail.account_id = @iAccountID
    SELECT @nPeriodLastYear = ( SELECT Case @TypeOfCurrency
    										When 'System' Then ISNULL(SUM(ROUND(ISNULL(TransDetail.system_amount, 0.0),2)), 0.0)
    										When 'Base' Then ISNULL(SUM(ROUND(ISNULL(TransDetail.amount, 0.0),2)), 0.0)
    									END
                        FROM TransDetail TransDetail
                        JOIN Document Document
                        ON TransDetail.document_id = Document.document_id
                        WHERE Document.document_date > @dPrevPeriodLastYearEndDate
                        AND Document.document_date <= @dThisPeriodLastYearEndDate
                        AND TransDetail.account_id = @iAccountID
                        AND ( @Branch_id = 0
                            OR
                            ( @branch_id <> 0
                                AND
                                TransDetail.company_id = @branch_id
                            )
                        )
                        AND Transdetail.spare NOT IN ('COMM ADJ', 'AGENT ADJ')
                        AND TransDetail.Document_Sequence NOT IN
                                ( SELECT Document_Sequence + 1
                                    FROM TransDetail
                                    WHERE document_id = Document.document_id
                                    AND spare = 'COMM ADJ'
                                )
                        AND ( @iCostCentre = 0
                                OR
                                ( @iCostCentre <> 0
                                    AND
                                    TransDetail.department_id = @iCostCentre
                                )
                            )
                    ) +
                    ( SELECT Case @TypeOfCurrency
    										When 'System' Then ISNULL(SUM(ROUND(ISNULL(TransDetail.system_amount, 0.0),2)), 0.0)
    										When 'Base' Then ISNULL(SUM(ROUND(ISNULL(TransDetail.amount, 0.0),2)), 0.0)
    								END
                        FROM TransDetail TransDetail
                        JOIN Document Document
                        ON TransDetail.document_id = Document.document_id
                        WHERE Transdetail.ref_date > @dPrevPeriodLastYearEndDate
                        AND Transdetail.ref_date <= @dThisPeriodLastYearEndDate
                        AND TransDetail.account_id = @iAccountID
                    AND ( @Branch_id = 0
                            OR
                            ( @branch_id <> 0
                                AND
                                TransDetail.company_id = @branch_id
                            )
                        )
                        AND ( Transdetail.spare IN ('COMM ADJ', 'AGENT ADJ')
                                OR
                                Transdetail.Document_Sequence IN
                                    ( SELECT Document_Sequence + 1
                                        FROM TransDetail
                                        WHERE document_id = Document.document_id
                                        AND spare = 'COMM ADJ'
                                    )
                            )
                        AND ( @iCostCentre = 0
                                OR
                                ( @iCostCentre <> 0
                                    AND
                                    TransDetail.department_id = @iCostCentre
                                )
                            )
                    )
    SELECT @nLastYearToDate = (  SELECT Case @TypeOfCurrency
    										When 'System' Then ISNULL(SUM(ROUND(ISNULL(TransDetail.system_amount, 0.0),2)), 0.0)
    										When 'Base' Then ISNULL(SUM(ROUND(ISNULL(TransDetail.amount, 0.0),2)), 0.0)
    									END
                        FROM TransDetail TransDetail
                        JOIN Document Document
                        ON TransDetail.document_id = Document.document_id
                        WHERE Document.document_date > @d2YearsAgoEndDate
                        AND Document.document_date <= @dThisPeriodLastYearEndDate
                        AND TransDetail.account_id = @iAccountID
                    AND ( @Branch_id = 0
                            OR
                            ( @branch_id <> 0
                                AND
                                TransDetail.company_id = @branch_id
                            )
                        )
                        AND Transdetail.spare NOT IN ('COMM ADJ', 'AGENT ADJ')
                        AND TransDetail.Document_Sequence NOT IN
                                ( SELECT Document_Sequence + 1
                                    FROM TransDetail
                                    WHERE document_id = Document.document_id
                                    AND spare = 'COMM ADJ'
                                )
                        AND ( @iCostCentre = 0
                                OR
                                ( @iCostCentre <> 0
                                    AND
                                    TransDetail.department_id = @iCostCentre
                                )
                            )
                    ) +
                    (  SELECT Case @TypeOfCurrency
    										When 'System' Then ISNULL(SUM(ROUND(ISNULL(TransDetail.system_amount, 0.0),2)), 0.0)
    										When 'Base' Then ISNULL(SUM(ROUND(ISNULL(TransDetail.amount, 0.0),2)), 0.0)
    									END
                        FROM TransDetail TransDetail
                        JOIN Document Document
                        ON TransDetail.document_id = Document.document_id
                        WHERE Transdetail.ref_date > @d2YearsAgoEndDate
                        AND Transdetail.ref_date <= @dThisPeriodLastYearEndDate
                        AND TransDetail.account_id = @iAccountID
                    AND ( @Branch_id = 0
                            OR
                            ( @branch_id <> 0
                                AND
                                TransDetail.company_id = @branch_id
                            )
                        )
			AND	(Transdetail.spare IN ('COMM ADJ', 'AGENT ADJ')
				OR
				Transdetail.Document_Sequence IN
                                ( SELECT Document_Sequence + 1
                                    FROM TransDetail
                                    WHERE document_id = Document.document_id
                                    AND spare = 'COMM ADJ'
                                )
			)
                        AND ( @iCostCentre = 0
                                OR
                                ( @iCostCentre <> 0
                                    AND
                                    TransDetail.department_id = @iCostCentre
                                )
                            )
                    )
    UPDATE #Report_Transaction
    SET amount = @nThisPeriod,
        extra_numeric1 = @nPeriodBudget,
        extra_numeric2 = @nYearToDate,
        extra_numeric3 = @nBudgetToDate,
        extra_numeric4 = @nPeriodLastYear,
        extra_numeric5 = @nLastYearToDate
    WHERE account_id = @iAccountID AND branch_id = @branch_id

    --For Debug

    --SELECT amount,extra_numeric1,extra_numeric2,extra_numeric3,
    --extra_numeric4,extra_numeric5,branch_id,account_id from report_transaction
    --WHERE account_id = @iAccountID
	
    FETCH NEXT FROM cAccount INTO
                    @iAccountID,
                    @d2YearsAgoEndDate,
                    @dPrevPeriodLastYearEndDate,
                    @dThisPeriodLastYearEndDate,
                    @dLastYearEndDate,
                    @d1stPeriodEndDate,
                    @dPrevPeriodEndDate,
                    @dPeriodEndDate,
		    @CurrCompanyId
    END
    CLOSE cAccount
    DEALLOCATE cAccount
SET NOCOUNT OFF
SELECT RTPN.element_name1,
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
        RT.account_type,
        extra_char2 currencycode,
		extra_char3 currencydesc,
		extra_char4 companycode,
        extra_char5 companydesc,
        Case @GroupbyCode
        	WHEN 'Branch' THEN extra_char4
        	WHEN 'Branch and Currency' THEN extra_char4
        	ELSE ''
        END

FROM #Report_TreePathNames RTPN
        JOIN #Report_Transaction RT
        ON RTPN.account_id = RT.account_id
WHERE (RT.Amount <> 0 OR RT.extra_numeric1 <> 0 OR RT.extra_numeric2 <> 0 OR RT.extra_numeric3 <> 0)
ORDER BY Report_Map_Id1, Report_Map_Id2, Report_Map_Id3, Report_Map_Id4, Report_Map_Id5, Report_Map_Id6, Report_Map_Id7, Report_Map_Id8, Report_Map_Id9, Report_Map_Id10

SET NOCOUNT ON
DROP TABLE #Report_Transaction
DROP TABLE #Report_TreePathNames
SET NOCOUNT OFF

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

