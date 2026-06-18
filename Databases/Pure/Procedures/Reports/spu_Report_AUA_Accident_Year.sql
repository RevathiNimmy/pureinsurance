SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_AUA_Accident_Year'
GO


CREATE PROCEDURE spu_Report_AUA_Accident_Year
    @start_date datetime,
    @end_date datetime
AS
/**********************************************************************************************************************************
** Created by Kerry Butler
** 17/09/2001
** AUA Reports -  AUA_Accident_Year.rpt
**
**********************************************************************************************************************************
** 1.1      28/11/2001   JMK     remove Reinsurer parameter and UW_Type
**                               get year from loss_date
***********************************************************************************************************************************/

SET NOCOUNT ON
/*
-- test
DECLARE @start_date datetime,
    @end_date datetime

SELECT @start_date = convert(datetime, '5-28-2001'),
    @end_date = convert(datetime, '11-28-2001')
*/
CREATE TABLE #tempAUA_Accident_Year
        (LossYear               varchar (10)    NULL,
        PeriodNumber            int             NULL,
        ReinsurerName           varchar (20)    NULL,
        REinsurerResolvedName   varchar (100)   NULL,
        LossFromDate            datetime        NULL,
        PaidClaim               decimal (19,4)  NULL,
        Description             varchar (255)   NULL,
        StatsDetailType         varchar (3)     NULL
    )

INSERT into #tempAUA_Accident_Year
    SELECT datepart(year,c.loss_from_date),
        sf.posting_period_number,
        ri_shortname,
        CASE sd.stats_detail_type
            WHEN 'TTY' THEN
                (SELECT description FROM Treaty WHERE ri_shortname = code)
            ELSE
                (SELECT resolved_name FROM Party WHERE ri_shortname = shortname)
            END,
        c.loss_from_date,
        this_premium_home,
        LEFT(c.description,255),
        sd.stats_detail_type
    FROM stats_detail sd
    JOIN stats_folder sf ON sf.stats_folder_cnt = sd.stats_folder_cnt
    JOIN claim c ON c.claim_id = sf.loss_id
    WHERE   sf.transaction_type_code = 'C_CP'
    AND sd.stats_detail_type IN ('TTY','COI')
    AND sf.transaction_date BETWEEN @start_date AND @end_date

-- In case of duff data...
UPDATE #tempAUA_Accident_Year
SET  ReinsurerResolvedName = ReinsurerName
WHERE ReinsurerResolvedName IS NULL

SELECT * FROM #tempAUA_Accident_Year
DROP TABLE #tempAUA_Accident_Year

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO

