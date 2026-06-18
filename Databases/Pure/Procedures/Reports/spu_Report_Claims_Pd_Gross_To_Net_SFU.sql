SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Claims_Pd_Gross_To_Net_SFU'
GO


CREATE PROCEDURE spu_Report_Claims_Pd_Gross_To_Net_SFU
    @branch_id int,
    @PeriodDate varchar(20),
    @Basis varchar(50),
    @TypeOfCurrency varchar(30), 
    @GroupByCode varchar(30)
                
AS
    SET NOCOUNT ON

    DECLARE 
        @period_end_date datetime,
        @dtSelectedPeriodEnd datetime,  
        @dtPrevPeriodEnd datetime, 
        @dtYearStart datetime, 
        @dtPrevYearEnd datetime,
        @dtPrev12MonthAgoEnd datetime,
        @SelectedPeriodID int,
        @iBranchPeriod int,
        @SystemCurrencyCode varchar(10),
        @SystemCurrencyDesc varchar(255)


    -- Always use Branch 1 period table
    -- to be amended if anyone sets up different periods for different branches
    SELECT  @period_end_date = CONVERT(datetime, @PeriodDate + ' 23:59:59'),
            @iBranchPeriod = 1 


    -- Selected period values
    SELECT  @dtSelectedPeriodEnd = MAX(period_end_date)
    FROM    period
    WHERE   period_end_date <= @period_end_date
    AND     company_id = @iBranchPeriod

    SELECT  @SelectedPeriodID = period_id 
    FROM    period
    WHERE   period_end_Date = @dtSelectedPeriodEnd
    AND     company_id = @iBranchPeriod


    -- Previous period values
    SELECT  @dtPrevPeriodEnd = MAX(period_end_date)
    FROM    Period
    WHERE   period_end_date < @period_end_date
    AND     company_id = @iBranchPeriod

    -- If current period is the first period set up (no need for exact date)
    IF @dtPrevPeriodEnd IS NULL BEGIN
        SELECT  @dtPrevPeriodEnd = dateadd(month, -1, @dtSelectedPeriodEnd),
                @dtPrevYearEnd = @dtPrevPeriodEnd,
                @dtYearStart = @dtPrevPeriodEnd,
                @dtPrev12MonthAgoEnd = @dtPrevPeriodEnd
    END ELSE BEGIN
        -- year start period values
        SELECT  @dtYearStart = min(period_end_date)
        FROM    period
        WHERE   year_name = (SELECT year_name FROM period 
                                WHERE period_id = @SelectedPeriodID AND company_id = @iBranchPeriod)
        AND     company_id = @iBranchPeriod 
    
        -- previous year end values
        SELECT  @dtPrevYearEnd = max(period_end_date)
        FROM    period
        WHERE   period_end_date < @dtYearStart
        AND     company_id = @iBranchPeriod
    
        -- 12 month ago dates
        SELECT  @dtPrev12MonthAgoEnd = min(period_end_date)
        FROM    Period
        WHERE   period_end_date >= dateadd(year, -1, @dtPrevPeriodEnd)
        AND     company_id = @iBranchPeriod
    
        -- *If there are no periods set up for the previous year
        IF @dtPrevYearEnd IS NULL BEGIN
            SELECT  @dtPrevYearEnd = dateadd(month, -1, @dtYearStart),
                    @dtPrev12MonthAgoEnd = dateadd(month, -1, @dtYearStart)
        END
    END

    -- Create period range temp table
    CREATE TABLE #PeriodIDRange (
        Period_id int,
        CurrentYear int)

    -- Get all period ids up to selected period
    -- cannot assume ids are in date order
    INSERT INTO #PeriodIDRange
        SELECT  period_id, 0
        FROM    Period
        WHERE   period_end_date <= @dtSelectedPeriodEnd 
        AND     period_end_date > dateadd(year, -1, @dtSelectedPeriodEnd)
        AND     company_id = @iBranchPeriod 

    UPDATE  #PeriodIDRange
    SET     CurrentYear = 1
    FROM    #PeriodIDRange pr
    JOIN    Period p 
            ON p.period_id = pr.period_id
    WHERE   period_end_date > @dtPrevYearEnd
    AND     company_id = @iBranchPeriod 


    -- Get System Currency Details
    SELECT  @SystemCurrencyCode = c.iso_code,
            @SystemCurrencyDesc = c.description
    FROM    PMSystem pms
    JOIN    currency c
            ON c.currency_id = pms.currency_id
    WHERE   pms.system_id = 1


    -- Temporary table for all bands
    CREATE TABLE #tempClmPaidGrossNet (
        ProductCode varchar(10) NULL,
        ProductDesc varchar(255) NULL,
        PeriodRangeID int,  -- 1 = Current, 2 = YTD, 3 = 12Months
        PeriodRangeName varchar(30),
        RiskTypeCode varchar(10) NULL,
        RiskTypeDescription varchar(255) NULL,
        Gross decimal(19,4) NULL,
        Coinsurance decimal(19,4) NULL,
        Treaty decimal(19,4) NULL,
        Facultative decimal(19,4) NULL,
        XOL decimal(19,4) NULL,
        Retained decimal(19,4) NULL,
        TransDate datetime NULL,
        dtSelectedPeriodEnd datetime,
        PostingPeriodID int,
        SelectedPeriodID int,
        CurrencyCode varchar(5),
        SourceID int)
    

    -- SELECTED PERIOD
    -- Add Premium Records - Gross
    INSERT INTO #tempClmPaidGrossNet
        SELECT  sf.product_code,
                p.description,
                1,      -- Current Period
                'Selected Period',
                sd.risk_type_code,
                rt.description,
                CASE WHEN sd.stats_detail_type = 'GRS' THEN
                    CASE @TypeOfCurrency 
                        WHEN 'Base' THEN -sd.this_premium_home
                        WHEN 'system' THEN -sd.this_premium_system
                        WHEN 'Transaction' THEN -sd.this_premium_original
                    END END,
                CASE WHEN sd.stats_detail_type = 'COI' THEN
                    CASE @TypeOfCurrency 
                        WHEN 'Base' THEN sd.this_premium_home 
                        WHEN 'system' THEN sd.this_premium_system
                        WHEN 'Transaction' THEN sd.this_premium_original
                    END END,
                CASE WHEN sd.stats_detail_type = 'TTY' THEN
                    CASE @TypeOfCurrency 
                        WHEN 'Base' THEN sd.this_premium_home  
                        WHEN 'system' THEN sd.this_premium_system  
                        WHEN 'Transaction' THEN sd.this_premium_original 
                    END END,
                CASE WHEN sd.stats_detail_type = 'FAC' THEN
                    CASE @TypeOfCurrency 
                        WHEN 'Base' THEN sd.this_premium_home
                        WHEN 'system' THEN sd.this_premium_system
                        WHEN 'Transaction' THEN sd.this_premium_original
                    END END,
                CASE WHEN sd.stats_detail_type = 'XOL' or sd.stats_detail_type = 'TYX' THEN
                    CASE @TypeOfCurrency 
                        WHEN 'Base' THEN sd.this_premium_home
                        WHEN 'system' THEN sd.this_premium_system
                        WHEN 'Transaction' THEN sd.this_premium_original
                    END END,
                CASE WHEN sd.stats_detail_type = 'NET' THEN
                    CASE @TypeOfCurrency 
                        WHEN 'Base' THEN sd.this_premium_home
                        WHEN 'system' THEN sd.this_premium_system
                        WHEN 'Transaction' THEN sd.this_premium_original
                    END END,
                sf.transaction_date,
                @dtSelectedPeriodEnd,
                sf.posting_period_number,
                @SelectedPeriodID,
                sd.currency_code,
                sf.Source_id
        FROM    Stats_Folder sf
        JOIN    Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        JOIN    Product p          ON sf.product_id = p.product_id
        JOIN    Risk_Type rt       ON sd.risk_type_id = rt.risk_type_id
        WHERE   sf.transaction_type_code = 'C_CP'
        AND    (@branch_id = 0 OR sf.branch_id = @branch_id)
        AND   ((@TypeOfCurrency = 'Base'        AND IsNull(sd.this_premium_home, 0) <> 0)
            OR (@TypeOfCurrency = 'System'      AND IsNull(sd.this_premium_system, 0) <> 0)
            OR (@TypeOfCurrency = 'Transaction' AND IsNull(sd.this_premium_original, 0) <> 0))
                -- Filter out reserve amendments in payment stats
        AND    (sd.ri_party_cnt <> 0 OR sd.stats_detail_type = 'GRS') 
                -- Appropriate date filter for this period
        AND   ((@Basis = 'Transaction Period' AND sf.posting_period_number = @SelectedPeriodID)
            OR (@Basis = 'Transaction Date' AND sf.document_date > @dtPrevPeriodEnd AND sf.document_date <= @dtSelectedPeriodEnd))


    -- SELECTED YEAR TO DATE
    -- Add Premium Records - Gross
    INSERT INTO #tempClmPaidGrossNet
        SELECT  sf.product_code,
                p.description,
                2,      -- Current Year To Date
                'Selected Year',
                sd.risk_type_code,
                rt.description,
                CASE WHEN sd.stats_detail_type = 'GRS' THEN
                    CASE @TypeOfCurrency 
                        WHEN 'Base' THEN -sd.this_premium_home
                        WHEN 'system' THEN -sd.this_premium_system
                        WHEN 'Transaction' THEN -sd.this_premium_original
                    END END,
                CASE WHEN sd.stats_detail_type = 'COI' THEN
                    CASE @TypeOfCurrency 
                        WHEN 'Base' THEN sd.this_premium_home 
                        WHEN 'system' THEN sd.this_premium_system
                        WHEN 'Transaction' THEN sd.this_premium_original
                    END END,
                CASE WHEN sd.stats_detail_type = 'TTY' THEN
                    CASE @TypeOfCurrency 
                        WHEN 'Base' THEN sd.this_premium_home  
                        WHEN 'system' THEN sd.this_premium_system  
                        WHEN 'Transaction' THEN sd.this_premium_original 
                    END END,
                CASE WHEN sd.stats_detail_type = 'FAC' THEN
                    CASE @TypeOfCurrency 
                        WHEN 'Base' THEN sd.this_premium_home
                        WHEN 'system' THEN sd.this_premium_system
                        WHEN 'Transaction' THEN sd.this_premium_original
                    END END,
                CASE WHEN sd.stats_detail_type = 'XOL' or sd.stats_detail_type = 'TYX' THEN
                    CASE @TypeOfCurrency 
                        WHEN 'Base' THEN sd.this_premium_home
                        WHEN 'system' THEN sd.this_premium_system
                        WHEN 'Transaction' THEN sd.this_premium_original
                    END END,
                CASE WHEN sd.stats_detail_type = 'NET' THEN
                    CASE @TypeOfCurrency 
                        WHEN 'Base' THEN sd.this_premium_home
                        WHEN 'system' THEN sd.this_premium_system
                        WHEN 'Transaction' THEN sd.this_premium_original
                    END END,
                sf.transaction_date,
                @dtSelectedPeriodEnd,
                sf.posting_period_number,
                @SelectedPeriodID,
                sd.currency_code,
                sf.Source_id
        FROM    Stats_Folder sf
        JOIN    Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        JOIN    Product p          ON sf.product_id = p.product_id
        JOIN    Risk_Type rt       ON sd.risk_type_id = rt.risk_type_id
        WHERE   sf.transaction_type_code = 'C_CP'
        AND    (@branch_id = 0 OR sf.branch_id = @branch_id)
        AND   ((@TypeOfCurrency = 'Base'        AND IsNull(sd.this_premium_home, 0) <> 0)
            OR (@TypeOfCurrency = 'System'      AND IsNull(sd.this_premium_system, 0) <> 0)
            OR (@TypeOfCurrency = 'Transaction' AND IsNull(sd.this_premium_original, 0) <> 0))
                -- Filter out reserve amendments in payment stats
        AND    (sd.ri_party_cnt <> 0 OR sd.stats_detail_type = 'GRS') 
                -- Appropriate date filter for this period
        AND   ((@Basis = 'Transaction Period' AND sf.posting_period_number IN (SELECT Period_id FROM #PeriodIDRange WHERE CurrentYear = 1))
            OR (@Basis = 'Transaction Date' AND sf.document_date > @dtPrevYearEnd AND sf.document_Date <= @dtSelectedPeriodEnd))
    

    -- MOVING 12 MONTHS
    -- Add Premium Records - Gross
    INSERT INTO #tempClmPaidGrossNet
        SELECT  sf.product_code,
                p.description,
                3,      -- 12 Months
                '12 Months',
                sd.risk_type_code,
                rt.description,
                CASE WHEN sd.stats_detail_type = 'GRS' THEN
                    CASE @TypeOfCurrency 
                        WHEN 'Base' THEN -sd.this_premium_home
                        WHEN 'system' THEN -sd.this_premium_system
                        WHEN 'Transaction' THEN -sd.this_premium_original
                    END END,
                CASE WHEN sd.stats_detail_type = 'COI' THEN
                    CASE @TypeOfCurrency 
                        WHEN 'Base' THEN sd.this_premium_home 
                        WHEN 'system' THEN sd.this_premium_system
                        WHEN 'Transaction' THEN sd.this_premium_original
                    END END,
                CASE WHEN sd.stats_detail_type = 'TTY' THEN
                    CASE @TypeOfCurrency 
                        WHEN 'Base' THEN sd.this_premium_home
                        WHEN 'system' THEN sd.this_premium_system 
                        WHEN 'Transaction' THEN sd.this_premium_original
                    END END,
                CASE WHEN sd.stats_detail_type = 'FAC' THEN
                    CASE @TypeOfCurrency 
                        WHEN 'Base' THEN sd.this_premium_home
                        WHEN 'system' THEN sd.this_premium_system
                        WHEN 'Transaction' THEN sd.this_premium_original
                    END END,
                CASE WHEN sd.stats_detail_type = 'XOL' or sd.stats_detail_type = 'TYX' THEN
                    CASE @TypeOfCurrency 
                        WHEN 'Base' THEN sd.this_premium_home
                        WHEN 'system' THEN sd.this_premium_system
                        WHEN 'Transaction' THEN sd.this_premium_original
                    END END,
                CASE WHEN sd.stats_detail_type = 'NET' THEN
                    CASE @TypeOfCurrency 
                        WHEN 'Base' THEN sd.this_premium_home
                        WHEN 'system' THEN sd.this_premium_system
                        WHEN 'Transaction' THEN sd.this_premium_original
                    END END,
                sf.transaction_date,
                @dtSelectedPeriodEnd,
                sf.posting_period_number,
                @SelectedPeriodID,
                sd.currency_code,
                sf.Source_id
        FROM    Stats_Folder sf
        JOIN    Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        JOIN    Product p          ON sf.product_id = p.product_id
        JOIN    Risk_Type rt       ON sd.risk_type_id = rt.risk_type_id
        WHERE   sf.transaction_type_code = 'C_CP'
        AND    (@branch_id = 0 OR sf.branch_id = @branch_id)
        AND   ((@TypeOfCurrency = 'Base'        AND IsNull(sd.this_premium_home, 0) <> 0)
            OR (@TypeOfCurrency = 'System'      AND IsNull(sd.this_premium_system, 0) <> 0)
            OR (@TypeOfCurrency = 'Transaction' AND IsNull(sd.this_premium_original, 0) <> 0))
                -- Filter out reserve amendments in payment stats
        AND    (sd.ri_party_cnt <> 0 OR sd.stats_detail_type = 'GRS')
                -- Appropriate date filter for this period
        AND   ((@Basis = 'Transaction Period' AND sf.posting_period_number IN (SELECT Period_id FROM #PeriodIDRange))
            OR (@Basis = 'Transaction Date' AND sf.document_date > @dtPrev12MonthAgoEnd AND sf.document_Date <= @dtSelectedPeriodEnd))


    SET NOCOUNT OFF

    -- Return all records
    SELECT  ProductCode,
            ProductDesc,
            PeriodRangeID,
            PeriodRangeName,
            RiskTypeCode,
            RiskTypeDescription,
            Gross,
            Coinsurance,
            Treaty,
            Facultative,
            XOL,
            Retained,
            TransDate,
            dtSelectedPeriodEnd,
            PostingPeriodID,
            SelectedPeriodID,
            S.Code,
            S.Description,
            CASE @TypeOfCurrency 
                WHEN 'Base' THEN CB.Code
                WHEN 'System' THEN @Systemcurrencycode
                WHEN 'Transaction' THEN CT.Code
            END CurrencyCode,
            CASE @TypeOfCurrency 
                WHEN 'Base' THEN CB.description
                WHEN 'System' THEN @SystemCurrencyDesc
                WHEN 'Transaction' THEN CT.description
            END CurrencyDesc,
            CASE @GroupbyCode 
                WHEN 'Branch' THEN S.Code
                WHEN 'Branch And Currency' THEN S.Code
                WHEN 'Currency' THEN CT.Code
                ELSE ''
            END 'GroupByCode'    
    FROM    #tempClmPaidGrossNet TG
    JOIN    Source S 
            ON S.source_id = TG.Sourceid
    JOIN    Currency CB 
            ON CB.Currency_id = S.Base_currency_id
    JOIN    Currency CT 
            ON CT.iso_code = TG.CurrencyCode


    -- Drop temp tables
    DROP TABLE #tempClmPaidGrossNet
    DROP TABLE #PeriodIDRange

GO


