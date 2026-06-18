SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_All_Registers'
GO

CREATE PROCEDURE spu_Report_All_Registers
    @register_type varchar(255),
    @start_date datetime,
    @end_date datetime,
    @register_period varchar(255)
AS
/**********************************************************************************************************************************
** Created by Jude Killip
** 18/11/2000
** RSA Reports  - Register.rpt
**                (Premium Register; Claim Register; Commission Register; Average Premium)
**
**********************************************************************************************************************************
** 07/12/2000 Jude Killip       bug 297 - stats_detail_type
**
** 13/03/2001 Jude Killip       add Average Premium as a TransactionType option
**                              base record selection on stats_detail_type
**
** 26/03/2001 Jude Killip       add Average Premium formula - premium as a percentage of sum insured
**                              use sum_insured_home - sum_insured_total not calculated/used unless coinsurance or claim
**
** 24/04/2001 Jude Killip       Add date parameters to move date selection criteria out of crystal report.
**                               Jury still out over how Claims will be recorded in STATS... so future work likely there
**                                 ditto on TAX.
**
** 16/06/2001 Jude Killip       Amend Tax - TAG = Gross Tax value
**                              Commission - use commission values now as they should be OK now
**                              Add PerilID (for report to prevent duplication of Sum insured)
**
** 29/06/2001 Jude Killip       No longer used for sum insured register
**                              Update Tax values according to Register type (TAC if commission, otherwise TAG)
**                              Retrieve Sum Insured just once for each peril
**
** 03/07/2001 Jude Killip       Add doc ref
**                              set 'NET','TTY','TAF','TAC','TAN','TAT' record values * -1
**                              Include Claims
**                              amend average premium  - don't calculate!
**
** 04/07/2001 Jude Killip       Sum Insured: filter out TTY sums insured
**
** 29/08/2001 Jude Killip       filter on dates at the beginning to speed things up
**                              add stats_folder.loss_code (claim number) for claims
**                              include COI and all relevant tax stats types
**
** 13/09/2001 Jude killip       get rid of Average Premium stuff - don't need it
**                              don't update duties on Commission Register
**
** 14/09/2001 Jude Killip       set 'TTY', 'FAC', 'TAF','TAC', 'TAT' record values * -1  for NON CLAIMS ONLY
**
** 27/09/2001 Jude killip       filter out failed stats
**                              check tax_type.is_not_applied_to_client before including tax
**
** 28/09/2001 Jude killip       left some rubbish in Claims section
**
** 02/10/2001 Jude Killip       sum_insured_total is now populated (for claims only so far)
**                              ...so replace sum_insured_home with this value for use in Claims Register (+ maybe the others later)
**
** 20/11/2001 JMK               More problems with Claims Sum Insured doubling up.
**                              ...  Divide it by no. of Peril Types so it will total up properly
**
** 19/12/2001 JMK               "Direct" if no Agent
***********************************************************************************************************************************/
SET NOCOUNT ON
/*
--for testing
declare @register_type varchar (20),
    @start_date datetime,
    @end_date datetime,
    @register_period varchar (20)
--pre
--com
--cla
--ave
select @register_type = 'com',
    @start_date = dateadd(day,-55,getdate()),
    @end_date = getdate(),
    @register_period = 'This Month'
--Specify Dates
--Today
--Yesterday
--This Week
--Last Full Week
--This Month
--Last Full Month
*/

DECLARE @sAmountType varchar (20)

/* Amount values according to Register type
        'ThisPremium'           this_premium_home
        'Commission'            lead_commission_home + sub_commission_home
        'SumInsured'            sum_insured_home
        'AveragePremium'        (sd.this_premium_home/sd.sum_insured_home)*100
*/
IF @register_type LIKE 'Com%'
    SELECT @sAmountType = 'Commission'
ELSE
    SELECT @sAmountType = 'ThisPremium'

CREATE TABLE #tempRegisters (
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
    Agency varchar (100) NULL,
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
    Amount1 decimal (19,4) NULL,
    Amount2 decimal (19,4) NULL,
    Amount3 decimal (19,4) NULL,
    Amount4 decimal (19,4) NULL
)

-- GET Stats_Folders
-- if Claims Register, select only Claims transaction_type_codes

CREATE TABLE #tempStatsFolder (
    StatsFolderCnt int
)

IF @register_type LIKE 'CLA%' BEGIN
    --print 'start of claim insert'

    INSERT INTO #tempStatsFolder
        SELECT sf.stats_folder_cnt
        FROM Stats_Folder sf
        WHERE sf.transaction_type_code LIKE ('C_%')
        --sj 31/07/2002 - start
        --AND (
        --    SELECT isnull(max(tef.accounts_export_status),'x')
        --    FROM transaction_export_folder tef WHERE sf.document_ref = tef.document_ref
        --    ) = 'c'
        --sj 31/07/2002 - end
        AND (
        @register_period = 'specify dates' AND
            (
            datediff(day, @start_date, transaction_date) >=0
            AND datediff(day, transaction_date, @end_date) >=0
            )
        OR
        @register_period = 'yesterday' AND
        datediff (day, transaction_date, getdate())= 1
        OR
        @register_period = 'today' AND
        datediff (day, transaction_date, getdate())= 0
        OR
        @register_period = 'last full week' AND
        datediff (week, transaction_date, getdate())= 1
        OR
        @register_period = 'this week' AND
        datediff (week, transaction_date, getdate())= 0
        OR
        @register_period = 'last full month' AND
        datediff (month, transaction_date, getdate())= 1
        OR
        @register_period = 'this month' AND
        datediff (month, transaction_date, getdate())= 0
        )
END

-- if NOT a Claims Register, only NON Claims transaction_type_codes
ELSE
BEGIN
    --print 'start of NON claim insert'

    INSERT INTO #tempStatsFolder
        SELECT sf.stats_folder_cnt
        FROM Stats_Folder sf
        WHERE sf.transaction_type_code NOT LIKE ('C_%')
        --sj 31/07/2002 - start
        --AND (
        --    SELECT isnull(max(tef.accounts_export_status),'x')
        --    FROM transaction_export_folder tef WHERE sf.document_ref = tef.document_ref
        --    ) = 'c'
        --sj 31/07/2002 - end
        AND (
        @register_period = 'specify dates' AND
            (
            datediff(day, @start_date, transaction_date) >=0
            AND datediff(day, transaction_date, @end_date) >=0
            )
        OR
        @register_period = 'yesterday' AND
        datediff (day, transaction_date, getdate())= 1
        OR
        @register_period = 'today' AND
        datediff (day, transaction_date, getdate())= 0
        OR
        @register_period = 'last full week' AND
        datediff (week, transaction_date, getdate())= 1
        OR
        @register_period = 'this week' AND
        datediff (week, transaction_date, getdate())= 0
        OR
        @register_period = 'last full month' AND
        datediff (month, transaction_date, getdate())= 1
        OR
        @register_period = 'this month' AND
        datediff (month, transaction_date, getdate())= 0
        )
END

INSERT INTO #tempRegisters
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
    (SELECT resolved_name FROM Party WHERE shortname = sf.insurance_holder_shortname),
    (SELECT isnull(resolved_name, ' Direct') FROM Party WHERE shortname = sf.agent_shortname),
    p.description,
    rt.description,
    sd.stats_folder_cnt,
    sd.stats_detail_id,
    sd.stats_detail_type,
    sd.risk_type_id,
    sd.peril_id,
    sd.sum_insured_total,
    (SELECT sd.sum_insured_home
    WHERE sd.peril_id =
        (SELECT min(sd2.peril_id)
        FROM stats_detail sd2
        WHERE sd2.stats_folder_cnt = sd.stats_folder_cnt
        AND sd.stats_detail_type <> 'TTY'
        )
    ),
    NULL,
    NULL,
    NULL,
    NULL,
    NULL
FROM Stats_Detail sd
JOIN Stats_Folder sf ON sd.stats_folder_cnt = sf.stats_folder_cnt
JOIN #tempStatsFolder sft ON sd.stats_folder_cnt = sft.StatsFolderCnt
LEFT OUTER JOIN Risk_Type rt ON sd.risk_type_id = rt.risk_type_id
LEFT OUTER JOIN Product p ON sf.product_id = p.product_id
WHERE sd.stats_detail_type IN ( 'GRS', 'TTY', 'FAC', 'COI')
OR  (
    sd.stats_detail_type IN ('TAG', 'TAT', 'TAF', 'TAC')
    AND
    (SELECT isnull(tt.is_not_applied_to_client,0) FROM tax_type tt WHERE tt.tax_type_id = sd.tax_type_id) <> 1
    )


IF @register_type LIKE 'CLA%' BEGIN
    --print 'Update Sum Insured for Claims'
    -- Use cursor
    -- Cursor variables
    DECLARE @StatsFolderCnt int,
            @StatsDetailID int,
            @SumInsured decimal (19,4)

    -- Additional variables
    DECLARE @CountPerilTypeId int,
            @SumInsuredSingle decimal (19,4)

    DECLARE Stats_cursor CURSOR FAST_FORWARD FOR
        SELECT sd.stats_folder_cnt,
                sd.stats_detail_id,
                sd.sum_insured_total
        FROM Stats_Detail sd
        JOIN #tempStatsFolder sft ON sd.stats_folder_cnt = sft.StatsFolderCnt
        WHERE sd.stats_detail_type IN ( 'GRS', 'TTY', 'FAC', 'COI')
        OR  (
            sd.stats_detail_type IN ('TAG', 'TAT', 'TAF', 'TAC')
            AND
            (SELECT isnull(tt.is_not_applied_to_client,0) FROM tax_type tt WHERE tt.tax_type_id = sd.tax_type_id) <> 1
        )


    OPEN Stats_cursor
    FETCH NEXT FROM Stats_cursor INTO
        @StatsFolderCnt,
        @StatsDetailID,
        @SumInsured

    WHILE @@FETCH_STATUS = 0 BEGIN

        SELECT @CountPerilTypeId = (SELECT Count(DISTINCT peril_type_id)
                        FROM stats_detail
                        WHERE stats_folder_cnt = @StatsFolderCnt
                        GROUP BY stats_folder_cnt)

        IF Isnull(@CountPerilTypeId,0) = 0 SELECT @CountPerilTypeId = 1

        SELECT @SumInsuredSingle = @SumInsured/@CountPerilTypeId

        UPDATE #tempRegisters
            SET SumInsuredSingle = @SumInsuredSingle
            WHERE StatsFolderCnt = @StatsFolderCnt
            AND StatsDetailID = @StatsDetailID

        --print 'debug'
        --SELECT @StatsFolderCnt 'StatsFolderCnt', @SumInsured 'SumInsured', @CountPerilTypeId '@CountPerilTypeId', @SumInsuredSingle 'SumInsuredSingle'

        FETCH NEXT FROM Stats_cursor INTO
            @StatsFolderCnt,
            @StatsDetailID,
            @SumInsured
    END

    CLOSE Stats_cursor
    DEALLOCATE Stats_cursor
END

DROP TABLE #tempStatsFolder

--print 'Update Amount1, Amount2, Amount3, Amount4 '
-- This Premium: non claims change the sign of TTY and FAC
-- This Premium: claims leave the sign alone

IF @sAmountType = 'ThisPremium' AND @register_type NOT LIKE 'Cla%' BEGIN
    --print 'Update Premium Records - ' + @sAmountType
    UPDATE #tempRegisters
        SET Amount1 = (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'GRS'),
            Amount2 = (SELECT sd.this_premium_home * -1 WHERE sd.stats_detail_type = 'TTY'),
            Amount3 = (SELECT sd.this_premium_home * -1 WHERE sd.stats_detail_type = 'FAC'),
            Amount4 = (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'COI')
        FROM  #tempRegisters
        JOIN Stats_Detail sd ON (sd.stats_folder_cnt = StatsFolderCnt AND sd.stats_detail_id = StatsDetailID)

END ELSE IF @sAmountType = 'ThisPremium' AND @register_type LIKE 'Cla%' BEGIN
    --print 'Update Premium Records - ' + @sAmountType
    UPDATE #tempRegisters
        SET Amount1 = (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'GRS'),
            Amount2 = (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'TTY'),
            Amount3 = (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'FAC'),
            Amount4 = (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'COI')
        FROM  #tempRegisters
        JOIN Stats_Detail sd ON (sd.stats_folder_cnt = StatsFolderCnt AND sd.stats_detail_id = StatsDetailID)

END ELSE IF @sAmountType = 'Commission' BEGIN
    --print 'Update Gross Records - com - ' + @sAmountType
    UPDATE #tempRegisters
        SET Amount1 = (SELECT isnull(sd.lead_commission_value_home,0) +
                        isnull(sd.sub_commission_value_home,0)
                        WHERE sd.stats_detail_type = 'GRS'),
            Amount2 = (SELECT(
                            isnull(sd.lead_commission_value_home,0) +
                            isnull(sd.sub_commission_value_home,0)
                            ) * -1
                    WHERE sd.stats_detail_type = 'TTY'),
            Amount3 = (SELECT(
                            isnull(sd.lead_commission_value_home,0) +
                            isnull(sd.sub_commission_value_home,0)
                            ) * -1
                    WHERE sd.stats_detail_type = 'FAC'),
            Amount4 = (SELECT isnull(sd.lead_commission_value_home,0) +
                    isnull(sd.sub_commission_value_home,0)
                    WHERE sd.stats_detail_type = 'COI')
        FROM  #tempRegisters
        JOIN Stats_Detail sd ON (sd.stats_folder_cnt = StatsFolderCnt AND sd.stats_detail_id = StatsDetailID)

END

--print 'Update Tax values - but not for Commission Register'
IF @register_type NOT LIKE 'Com%' BEGIN
    --print 'update TAX ' + @sAmountType
    UPDATE #tempRegisters
         SET  Duties =  CASE sd.stats_detail_type
            WHEN 'TAG' THEN
                tax_value
            ELSE
                tax_value * -1
            END
         FROM  #tempRegisters
         JOIN Stats_Detail sd ON (sd.stats_folder_cnt = StatsFolderCnt AND sd.stats_detail_id = StatsDetailID)
         WHERE sd.stats_detail_type IN ('TAG', 'TAT', 'TAF', 'TAC')
         AND isnull(RiskType,'') <> ''
END

SET NOCOUNT OFF

--print 'filter out zero values'
SELECT * FROM #tempRegisters
WHERE (
    Isnull(Amount1,0) <> 0
OR  Isnull(Amount2,0) <> 0
OR  Isnull(Amount3,0) <> 0
OR  Isnull(Amount4,0) <> 0
OR  Isnull(Duties,0) <> 0
    )

DROP TABLE #tempRegisters

GO

