SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Payments_By_Repairer'
GO


CREATE PROCEDURE spu_Report_Payments_By_Repairer
                     @start_date datetime,
                     @end_date datetime,
                     @Report_period varchar(255),
                     @Repairer varchar(255)
AS

SET NOCOUNT ON

CREATE TABLE #TempPaymentsByRepairerCode
(
        PolNum varchar (30) NULL,
        LossCode varchar (30) NULL,
        InsFileCnt int,
        TransTypeID int NULL,
        TransType varchar (10) NULL,
        ProductCode varchar (10) NULL,
        TransDate datetime NULL,
        DocRef varchar (25) NULL,
        FromDate datetime NULL,
        ToDate datetime NULL,
        Client varchar (100) NULL,
        Agency varchar(100) NULL,
        Product varchar (255) NULL,
        RiskType varchar (255) NULL,
        StatsFolderCnt int,
        StatsDetailID int,
        StatsDetailType char(3),
        RiskTypeID int NULL,
        PerilID int NULL,
        SumInsured decimal (19,4) NULL,
        SumInsuredSingle decimal (19,4) NULL,
        Duties decimal (19,4) NULL,
        Amount1 decimal (19,4) NULL
)

CREATE TABLE #tempStatsFolder
    (
        StatsFolderCnt int
    )

INSERT INTO #tempStatsFolder
    SELECT stats_folder_cnt
    FROM Stats_Folder
    WHERE document_ref like ('c%')
    AND (
    @Report_period = 'specify dates' AND
        (
        datediff(day, @start_date, transaction_date) >=0
        AND datediff(day, transaction_date, @end_date) >=0
        )
    OR
    @Report_period = 'yesterday' AND
    datediff (day, transaction_date, getdate())= 1
    OR
    @Report_period = 'today' AND
    datediff (day, transaction_date, getdate())= 0
    OR
    @Report_period = 'last full week' AND
    datediff (week, transaction_date, getdate())= 1
    OR
    @Report_period = 'this week' AND
    datediff (week, transaction_date, getdate())= 0
    OR
    @Report_period = 'last full month' AND
    datediff (month, transaction_date, getdate())= 1
    OR
    @Report_period = 'this month' AND
    datediff (month, transaction_date, getdate())= 0
    )

INSERT INTO #TempPaymentsByRepairerCode
    SELECT sf.insurance_ref,
    sf.loss_code,
    sf.insurance_file_cnt,
    sf.transaction_type_id,
    sf.transaction_type_code,
    sf.product_code,
    sf.transaction_date,
    sf.document_ref,
    sf.cover_start_date,
    sf.expiry_date,
    pt.resolved_Name,
    (SELECT resolved_name FROM Party WHERE shortname = sf.agent_shortname),
    p.description,
    rt.description,
    sd.stats_folder_cnt,
    sd.stats_detail_id,
    sd.stats_detail_type,
    sd.risk_type_id,
    sd.peril_id,
    sd.sum_insured_home,
    (SELECT sd.sum_insured_home
    WHERE sd.peril_id =
        (SELECT min(sd2.peril_id)
        FROM stats_detail sd2
        WHERE sd2.stats_folder_cnt = sd.stats_folder_cnt
        AND sd.stats_detail_type <> 'TTY'
        )
    ),
    NULL,
    NULL

FROM Stats_Detail sd
JOIN party pt on sd.ri_party_cnt = pt.party_cnt
left join party_type pty on pt.party_type_id = pty.party_type_id
JOIN Stats_Folder sf ON sd.stats_folder_cnt = sf.stats_folder_cnt
JOIN #tempStatsFolder sft ON sd.stats_folder_cnt = sft.StatsFolderCnt
LEFT OUTER JOIN Risk_Type rt ON sd.risk_type_id = rt.risk_type_id
LEFT OUTER JOIN Product p ON sf.product_id = p.product_id
WHERE sd.stats_detail_type = 'GRS'
and pty.code like 'ot%'
--and sf.document_ref like 'c%'

--print 'Update Gross Records - TP - ' + @sAmountType
UPDATE #TempPaymentsByRepairerCode
    SET  Amount1 = sd.this_premium_home
    FROM  #TempPaymentsByRepairerCode
    JOIN Stats_Detail sd ON (sd.stats_folder_cnt = StatsFolderCnt AND sd.stats_detail_id = StatsDetailID)
    WHERE sd.stats_detail_type = 'GRS'

SET NOCOUNT OFF
--
if @Repairer = 'ALL'
select * from #TempPaymentsByRepairerCode
else
select * from #TempPaymentsByRepairerCode where client = @Repairer

--
DROP TABLE #tempStatsFolder
DROP TABLE #TempPaymentsByRepairerCode
GO