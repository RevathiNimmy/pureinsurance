SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Earned_Unearned_SFU'
GO


CREATE PROCEDURE spu_Report_Earned_Unearned_SFU
    @DetailSummary varchar(10),
    @period_end_date datetime,
    @branch_id int,
    @Sbasis varchar(10)
as

    declare @ibranchid int
    IF @branch_id IS NULL
            SELECT @iBranchID = 0
        ELSE
            SELECT @iBranchID = @branch_id
    
    DECLARE @DEBUG INT
    SELECT @DEBUG = 0
    IF @DEBUG = 1
    BEGIN
        declare @TimeNow datetime
        declare @TimeInit datetime
        declare @TimePoint datetime
        select @TimeInit = getdate()
        select @TimeNow = getdate()
        select @TimePoint = @TimeNow
        print 'START Time: ' + convert(varchar(30), @TimeNow, 108)
        print 'Total Run Time: ' + convert(varchar(20),datediff(millisecond,@TimeInit, @TimeNow)) + ' milliseconds'
        print 'Section Time: ' + convert(varchar(20),datediff(millisecond,@TimePoint, @TimeNow)) + ' milliseconds'
    END
    DECLARE @CurrentPeriodID int, @dtCurrentPeriodEnd datetime
    --EXECUTE spu_Report_GetCurrentPeriod_SFU @CurrentPeriodID OUTPUT,  @dtCurrentPeriodEnd OUTPUT
    DECLARE  @12MonthPeriodID int, @dt12MonthPeriodEnd datetime
    --EXECUTE spu_Report_GetCurrent12MonthPeriod_SFU @12MonthPeriodID OUTPUT, @dt12MonthPeriodEnd OUTPUT
    --select @dtCurrentPeriodEnd = max(period_end_date) from period where period_end_date < getdate ()
    select @dtCurrentPeriodEnd = @period_end_date
    
    select @dt12MonthPeriodEnd  = max(period_end_date) from period where period_end_date < ( select dateadd(year, -1 , @dtCurrentPeriodEnd))
    --select @12 = datediff(day, sf.cover_start_date, sf.expiry_date)
    DECLARE @12MonthPeriodIDPlusOne int, @dtLastYearPeriodEndDate datetime,@CurrentYearStartPeriodID int
    
    SELECT @CurrentPeriodID = period_id
    FROM period
    WHERE period_end_Date  = @dtCurrentPeriodEnd
    
    IF @CurrentPeriodID IS NULL
        BEGIN
    SELECT @CurrentPeriodID = period_id + 1
    FROM period
    WHERE period_end_Date  <= @dtCurrentPeriodEnd
    END
    
    SELECT @CurrentYearStartPeriodID = min(period_id)
    FROM period
    WHERE year_name =
        (SELECT year_name
            FROM period
            WHERE period_id = @CurrentPeriodID)
    SELECT @dtLastYearPeriodEndDate = period_end_date
    FROM period
    WHERE period_id = @CurrentYearStartPeriodID -1
    
    IF @12MonthPeriodID = @CurrentPeriodID
        BEGIN
            SELECT  @12MonthPeriodIDPlusOne = @12MonthPeriodID
        END
    ELSE
        BEGIN
            SELECT @12MonthPeriodIDPlusOne = @12MonthPeriodID + 1
        END
    
    --IF @DEBUG = 1
    --BEGIN
    --    SELECT 'Current ID = ' , @CurrentPeriodID
    --    SELECT 'Current End Date = ', @dtCurrentPeriodEND
    --    SELECT '12 Month ID = ',12MonthPeriodID
    --    SELECT '12 month = ', @dt12MonthPeriodEnd
    --END
    
    CREATE TABLE #tempRSAUnEarndPrem
    (
        StatsFolderCnt int,
        ProductCode varchar (10) NULL,
        ProductDesc varchar (255) NULL,
        CommissionOrPremium int NULL,
        RiskTypeCode varchar (10) NULL,
        RiskTypeDescription varchar (255) NULL,
        InsuranceRef varchar (30) NULL,
        ClientCode Varchar (60) NULL,
        Gross decimal (19,8) NULL,
        GrossTotal decimal (19,4) NULL,
        Coinsurance decimal (19,8) NULL,
        CoinsTotal decimal (19,4) NULL,
        Treaty decimal (19,8) NULL,
        TreatyTotal decimal (19,4) NULL,
        Facultative decimal (19,8) NULL,
        FacTotal decimal (19,4) NULL,
        Nett decimal (19,4) NULL,
        Nettotal decimal (19,4) NULL,
        DocumentRef varchar (25) NULL,
        FromDate datetime NULL,
        ToDate datetime NULL,
        dtCurrentPeriodEnd datetime,
        PostingPeriodID int,
        CurrentPeriodID int,
        DaysOfCoverTotal int,
        IsMidnightRenewal int,
        BothDatesInRange tinyint            NULL,
        CalcDaysOfCover int                 NULL,
        GrossCoverRounded decimal (19,3)    NULL,
        CoInsCoverRounded decimal (19,3)    NULL,
        NetCoverRounded decimal (19,3)      NULL,
        TreatyCoverRounded decimal (19,3)   NULL,
        FACCoverRounded decimal (19,3)      NULL,
        RetainedCoverRounded decimal (19,3) NULL,
        Treatyid int NULL,
        EarningPatternId int
     )
    IF @DEBUG = 1
    BEGIN
        select @TimeNow = getdate()
        print 'LABEL1 Time: ' + convert(varchar(30), @TimeNow, 108)
        print 'Total Run Time: ' + convert(varchar(20),datediff(millisecond,@TimeInit, @TimeNow)) + ' milliseconds'
        print 'Section Time: ' + convert(varchar(20),datediff(millisecond,@TimePoint, @TimeNow)) + ' milliseconds'
        select @TimePoint = @TimeNow
    END
    INSERT INTO #tempRSAUnEarndPrem
        SELECT sd.stats_folder_cnt,
            sd.class_of_business_code,
            c.description,
            0,
            sd.class_of_business_code,
            c.description,
            sf.insurance_ref,
            sf.insurance_holder_shortname,
            NULL,
            (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'GRS'),
            NULL,
            (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'COI'),
            NULL,
            (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'TTY')*-1,
            NULL,
            (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'FAC')*-1,
            NULL,
            (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'NET')*-1,
            sf.document_ref,
            sf.cover_start_date,
            sf.expiry_date,
            @dtCurrentPeriodEnd,
            sf.posting_period_number,
            @CurrentPeriodID,
            datediff(day, sf.cover_start_date, sf.expiry_date),
            isnull(p.is_midnight_renewal,0),
            NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,
            sd.Earning_Pattern_id
        FROM Stats_Folder sf
        JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
            AND isnull(sd.this_premium_home,0) <> 0
            AND sd.stats_detail_type in ('GRS', 'COI', 'TTY', 'FAC','NET')
        JOIN Product p          ON sf.product_id = p.product_id
       JOIN class_of_business c          ON sd.class_of_business_id = c.class_of_business_id
        JOIN Risk_Type rt       ON sd.risk_type_id = rt.risk_type_id
        INNER JOIN Document AS doc
            ON doc.document_ref = sf.document_ref
          WHERE- datediff(day, sf.cover_start_date, sf.expiry_date) <> 0 
         and sf.expiry_date >= @dtCurrentPeriodEnd
        AND sf.transaction_type_code NOT LIKE ('C_%')
        AND ( @iBranchID= 0
                  or    (   @iBranchID <> 0 and sf.source_id = @iBranchID ))
		AND sf.cover_start_date <= @dtCurrentPeriodEnd
    
    IF @DEBUG = 1
    BEGIN
        select @TimeNow = getdate()
        print 'LABEL2 Time: ' + convert(varchar(30), @TimeNow, 108)
        print 'Total Run Time: ' + convert(varchar(20),datediff(millisecond,@TimeInit, @TimeNow)) + ' milliseconds'
        print 'Section Time: ' + convert(varchar(20),datediff(millisecond,@TimePoint, @TimeNow)) + ' milliseconds'
        select @TimePoint = @TimeNow
    END
    INSERT INTO #tempRSAUnEarndPrem
        SELECT sd.stats_folder_cnt,
            sd.class_of_business_code,
            c.description,
            1,
            sd.class_of_business_code,
            c.description,
            sf.insurance_ref,
            sf.insurance_holder_shortname,
            NULL,
            (SELECT (isnull(sd.lead_commission_value_home,0) +
                        isnull(sd.sub_commission_value_home,0)) WHERE sd.stats_detail_type = 'GRS'),
            NULL,
            (SELECT (isnull(sd.lead_commission_value_home,0) +
                        isnull(sd.sub_commission_value_home,0)) WHERE sd.stats_detail_type = 'COI'),
            NULL,
            (SELECT (isnull(sd.lead_commission_value_home,0) +
                        isnull(sd.sub_commission_value_home,0)) WHERE sd.stats_detail_type = 'TTY')*-1,
            NULL,
            (SELECT (isnull(sd.lead_commission_value_home,0) +
                        isnull(sd.sub_commission_value_home,0)) WHERE sd.stats_detail_type = 'FAC')*-1,
            NULL,
            NULL,
            sf.document_ref,
            sf.cover_start_date,
            sf.expiry_date,
            @dtCurrentPeriodEnd,
            sf.posting_period_number,
            @CurrentPeriodID,
            datediff(day, sf.cover_start_date, sf.expiry_date),
            isnull(p.is_midnight_renewal,0),
            NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,
            (select sdd.ri_party_cnt FROM stats_detail sdd
                LEFT join treaty tr on tr.treaty_id = sdd.ri_party_cnt 
                  where sdd.stats_detail_type like 'TTY' 
                         and sdd.stats_detail_id = sd.stats_detail_id
                         and reinsurance_type_id = 2 
                         and sdd.stats_folder_cnt = sd.stats_folder_cnt),
            sd.Earning_Pattern_id
                FROM Stats_Folder sf
            JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
            AND isnull(sd.lead_commission_value_home,0) +
                isnull(sd.sub_commission_value_home,0) <> 0
            AND sd.stats_detail_type in ('GRS', 'COI', 'TTY', 'FAC')
        JOIN Product p          ON sf.product_id = p.product_id
        JOIN class_of_business c          ON sd.class_of_business_id = c.class_of_business_id
        JOIN Risk_Type rt       ON sd.risk_type_id = rt.risk_type_id
        INNER JOIN Document AS doc
            ON doc.document_ref = sf.document_ref
        WHERE  datediff(day, sf.cover_start_date, sf.expiry_date) <> 0
         and sf.expiry_date >= @dtCurrentPeriodEnd
        AND sf.transaction_type_code NOT LIKE ('C_%')
        AND ( @iBranchID= 0
                  or    (   @iBranchID <> 0 and sf.source_id = @iBranchID ))
		AND sf.cover_start_date <= @dtCurrentPeriodEnd
    
    IF @DEBUG = 1
    BEGIN
        select @TimeNow = getdate()
        print 'LABEL3 Time: ' + convert(varchar(30), @TimeNow, 108)
        print 'Total Run Time: ' + convert(varchar(20),datediff(millisecond,@TimeInit, @TimeNow)) + ' milliseconds'
        print 'Section Time: ' + convert(varchar(20),datediff(millisecond,@TimePoint, @TimeNow)) + ' milliseconds'
        select @TimePoint = @TimeNow
    END
    

    UPDATE #TempRSAUnEarndPrem
        SET nettotal = isnull(treatytotal,0) * 0.01 * (select share_percent from treaty_party tp 
                       left join treaty tr on tr.treaty_id = tp.treaty_id 
                        where reinsurance_type_id = 2 and tp.treaty_id = Treatyid and 
                        party_cnt in (select party_cnt from party where shortname like 'RET%'))
    where TREATYID is not null and nettotal is NULL
    
    UPDATE #tempRSAUnEarndPrem
        SET
              Gross = CASE EarningPatternId
                          WHEN 1 THEN isnull(GrossTotal,0)/(DaysOfCoverTotal+IsMidnightRenewal)
                          ELSE isnull(GrossTotal,0)
                      END,
              Coinsurance = CASE EarningPatternId
                                WHEN 1 THEN isnull(CoinsTotal,0)/(DaysOfCoverTotal+IsMidnightRenewal)
                                ELSE isnull(CoinsTotal,0)
                            END,
              Treaty = CASE EarningPatternId
                           WHEN 1 THEN isnull(TreatyTotal,0)/(DaysOfCoverTotal+IsMidnightRenewal)
                           ELSE isnull(TreatyTotal,0)
                       END,
              Facultative = CASE EarningPatternId
                                WHEN 1 THEN isnull(FacTotal,0)/(DaysOfCoverTotal+IsMidnightRenewal)
                                ELSE isnull(FacTotal,0)
                            END,
              Nett = CASE EarningPatternId
                         WHEN 1 THEN isnull(NetTotal,0)/(DaysOfCoverTotal+IsMidnightRenewal)
                         ELSE isnull(NetTotal,0)
                     END,
              DaysOfCoverTotal = DaysOfCoverTotal+IsMidnightRenewal

    where TREATYID is null 
    
    UPDATE #tempRSAUnEarndPrem

        SET
              Gross = CASE EarningPatternId
                          WHEN 1 THEN isnull(GrossTotal,0)/(DaysOfCoverTotal+IsMidnightRenewal)
                          ELSE isnull(GrossTotal,0)
                      END,
              Coinsurance = CASE EarningPatternId
                                WHEN 1 THEN isnull(CoinsTotal,0)/(DaysOfCoverTotal+IsMidnightRenewal)
                                ELSE isnull(CoinsTotal,0)
                            END,
              Treaty = CASE EarningPatternId
                           WHEN 1 THEN isnull(TreatyTotal - Nettotal,0)/(DaysOfCoverTotal+IsMidnightRenewal)
                           ELSE isnull(TreatyTotal - Nettotal,0)
                       END,
              Facultative = CASE EarningPatternId
                                WHEN 1 THEN isnull(FacTotal,0)/(DaysOfCoverTotal+IsMidnightRenewal)
                                ELSE isnull(FacTotal,0)
                            END,
              Nett = CASE EarningPatternId
                         WHEN 1 THEN isnull(NetTotal,0)/(DaysOfCoverTotal+IsMidnightRenewal)
                         ELSE isnull(NetTotal,0)
                     END,
              DaysOfCoverTotal = DaysOfCoverTotal+IsMidnightRenewal

    where TREATYID is not null 
    
    /*UPDATE #TempRSAUnEarndPrem
        SET nettotal = isnull(GrossTotal,0) + isnull(CoinsTotal,0) - isnull(TreatyTotal,0) -  isnull(FacTotal,0)
    where CommissionOrPremium = 1 */
    
    
    
    
    UPDATE #TempRSAUnEarndPrem
        SET BothDatesInRange = 1,
            CalcDaysOfCover = datediff(day, FromDate, ToDate)+IsMidnightRenewal
    WHERE FromDate > dtCurrentPeriodEnd
    
    UPDATE #TempRSAUnEarndPrem
        SET BothDatesInRange = 0,
           CalcDaysOfCover = DateDiff(day, dtCurrentPeriodEnd,  ToDate)
    WHERE FromDate <= dtCurrentPeriodEnd
    
    UPDATE #TempRSAUnEarndPrem
        SET BothDatesInRange = 0,
            CalcDaysOfCover = 0
    WHERE dtCurrentPeriodEnd < FromDate
    
    UPDATE #TempRSAUnEarndPrem
        SET BothDatesInRange = 0,
            CalcDaysOfCover = DaysOfCoverTotal
    WHERE dtCurrentPeriodEnd > Todate
    
    IF @Sbasis like 'Earned' 
    BEGIN
    
	    UPDATE #TempRSAUnEarndPrem
	        SET         CalcDaysOfCover = 0
	    WHERE FromDate > dtCurrentPeriodEnd

	    UPDATE #TempRSAUnEarndPrem
                SET
                    CalcDaysOfCover = CASE EarningPatternId
                                          WHEN 1 THEN DaysOfCoverTotal - CalcDaysOfCover
                                          ELSE isnull(DaysOfCoverTotal,0)
                                      END
	            WHERE FromDate <= dtCurrentPeriodEnd

	    UPDATE #TempRSAUnEarndPrem
	        SET
	              GrossCoverRounded = CASE EarningPatternId
	                         WHEN 1 THEN (isnull(Gross,0) * CalcDaysOfCover)
	                         ELSE isnull(Gross,0)
	                     END,
	              CoInsCoverRounded = CASE EarningPatternId
	                         WHEN 1 THEN (isnull(CoInsurance,0) * CalcDaysOfCover)
	                         ELSE isnull(CoInsurance,0)
	                     END,
	
	              TreatyCoverRounded = CASE EarningPatternId
	                         WHEN 1 THEN (isnull(Treaty,0) * CalcDaysOfCover)
	                         ELSE isnull(Treaty,0)
	                     END,
	
	              FACCoverRounded = CASE EarningPatternId
	                         WHEN 1 THEN (isnull(Facultative,0) * CalcDaysOfCover)
	                         ELSE isnull(Facultative,0)
	                     END


    END 
    ELSE
    BEGIN
	    UPDATE #TempRSAUnEarndPrem
	        SET
	              GrossCoverRounded = CASE EarningPatternId
	                         WHEN 1 THEN (isnull(Gross,0) * CalcDaysOfCover)
	                         ELSE 0
	                     END,
	              CoInsCoverRounded = CASE EarningPatternId
	                         WHEN 1 THEN (isnull(CoInsurance,0) * CalcDaysOfCover)
	                         ELSE 0
	                     END,
	
	              TreatyCoverRounded = CASE EarningPatternId
	                         WHEN 1 THEN (isnull(Treaty,0) * CalcDaysOfCover)
	                         ELSE 0
	                     END,
	
	              FACCoverRounded = CASE EarningPatternId
	                         WHEN 1 THEN (isnull(Facultative,0) * CalcDaysOfCover)
	                         ELSE 0
	                     END

    END

    
    UPDATE #TempRSAUnEarndPrem
        SET
        NetCoverRounded      = isnull(GrossCoverRounded,0) - isnull(CoInsCoverRounded,0)
    
    UPDATE #TempRSAUnEarndPrem
        SET
        RetainedCoverRounded =  isnull(NetCoverRounded,0) - ( isnull(TreatyCoverRounded,0) + isnull(FACCoverRounded,0))
    
    Create Table  #TempEarnedandUnEarned
    (
            StatsFolderCnt int,
            ProductCode varchar(30),
            ProductDesc varchar(255),
            CommissionOrPremium int,
            RiskTypeDescription varchar(255),
            DocumentRef  varchar(20),
            InsuranceRef  varchar(30),
            ClientCode varchar(30),
            dtCurrentPeriodEnd datetime ,
            CalcDaysOfCover int,
            DaysOfCoverTotal int,
            Fromdate datetime,
            Todate datetime,
            Grosstotal decimal (19,2),
            NetTotal decimal (19,2),
            GrossCoverRounded decimal (19,2),
            CoInsCoverRounded decimal (19,2),
            NetCoverRounded decimal (19,2),
            TreatyCoverRounded decimal (19,2),
            FACCoverRounded decimal (19,2),
            RetainedCoverRounded decimal (19,2)
      )
    SET NOCOUNT OFF
    
    IF @DetailSummary = 'SUMMARY'
    
    BEGIN
	    INSERT INTO #TempEarnedandUnEarned
        SELECT
            COUNT(DISTINCT StatsFolderCnt),
                ProductCode,
            ProductDesc,
            CommissionOrPremium,
            RiskTypeDescription,
            NULL,
            NULL,
           NULL,
            @period_end_date, --dtCurrentPeriodEnd,
           CalcDaysOfCover,
            DaysOfCoverTotal,
            Fromdate,
            Todate,
            SUM(Grosstotal),
            Sum (isnull(NetTotal,0) ),
            SUM(GrossCoverRounded),
            SUM(CoInsCoverRounded),
            SUM(NetCoverRounded),
            SUM(TreatyCoverRounded),
            SUM(FACCoverRounded),
            SUM(RetainedCoverRounded)
          
        FROM #TempRSAUnEarndPrem
         Where   CalcDaysOfCover <> 0
        GROUP BY CommissionOrPRemium, ProductDesc, RiskTypeDescription,
              ProductCode, dtCurrentPeriodEnd,  CalcDaysOfCover,DaysOfCoverTotal,Fromdate, Todate
	END
	ELSE
    BEGIN 
	    INSERT INTO #TempEarnedandUnEarned
        SELECT
            StatsFolderCnt,
            ProductCode,
            ProductDesc,
            CommissionOrPremium,
            RiskTypeDescription,
            DocumentRef,
            InsuranceRef,
            ClientCode,
            @period_end_date, --dtCurrentPeriodEnd,
            CalcDaysOfCover,
            DaysOfCoverTotal,
            Fromdate,
            Todate,
            SUM(Grosstotal),
            Sum (isnull(NetTotal,0)),
            SUM(GrossCoverRounded),
            SUM(CoInsCoverRounded),
            SUM(NetCoverRounded),
            SUM(TreatyCoverRounded),
            SUM(FACCoverRounded),
            SUM(RetainedCoverRounded)
        FROM #TempRSAUnEarndPrem
        Where   CalcDaysOfCover <> 0
        GROUP BY CommissionOrPRemium, ProductDesc, RiskTypeDescription,
              ProductCode, StatsFolderCnt, DocumentRef, InsuranceRef,ClientCode,dtCurrentPeriodEnd, 
              CalcDaysOfCover,DaysOfCoverTotal,Fromdate, Todate
    END 
    
    --select * from  #tempRSAUnEarndPrem
    DROP TABLE #tempRSAUnEarndPrem 
    select * from  #TempEarnedandUnEarned
    DROP TABLE  #TempEarnedandUnEarned
GO
