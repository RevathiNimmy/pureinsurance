SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Agent_Perf_by_Year'
GO


CREATE PROCEDURE spu_Report_Agent_Perf_by_Year
    @company_id int,
    @sub_branch_id int=NULL, --AMJ
    @AgentShortName varchar(255)
AS


/**********************************************************************************************************************************
** Created by Kerry Butler
** 08/10/2001
** AUA Reports -  Agent Performance by Calendar Year
**  1.1 KB 17/12/01 Calculate unearned premium within the select statement, it doesnt seem
**              to work via updates
**              Ensure 'Direct' policies are listed as such
**      1.2 KB 08/01/02 Report should group by product rather than class of business.
**              So amended selection criteria to use product from stats_folder.
**              This means update to Class of Business from class-of_business_id is
**                              no longer required. Variable names have not been changed to avoid
**              changing the report itself.
**      1.3 KB 09/01/02 Use stats_detail record to get claim paid and reserve values rather than
**              going back to the claim table. This avoids double counting of records.
**
**      1.4 JMK 13/03/2002  Amend Earned Premium calc to take future cover start dates into account.
**                          Filter out direct client records.
**      1.5 KB  20/03/2002  Prevent divide by zero error
**      1.6 KB  22/03/2002  Use name from the party table to populate agent name as trading name on
**                          party_agent is not populated after data transfer
**
** 01/08/2002   AMJ - branch specific change
**********************************************************************************************************************************

***********************************************************************************************************************************/
SET NOCOUNT ON

IF @sub_branch_id IS NULL
    EXEC spu_sub_branch_default @source_id=@company_id, @sub_branch_id=@sub_branch_id OUTPUT

DECLARE @dtCurrentPeriodEnd datetime
DECLARE @dDailyRate decimal (19,4)

CREATE TABLE #tempAgtPerfYear
    (
    CurrentYear datetime NULL,
        AgentName   varchar (255) NULL,
    AgentCnt   int NULL,
    ClassofBus  varchar (255) NULL,  -- We are actually selecting product not COB (see comment above)
    ClassofBusId    int NULL,
    GrossPremium    decimal (19,4) NULL,
    Commission  decimal (19,4) NULL,
    PPYear      int NULL,
    TransactionType varchar (20)   NULL,
    ClaimsPaid  decimal (19,4) NULL,
    OSLossRes   decimal (19,4) NULL,
    UnearnedPremium decimal (19,4) NULL,
    PolicyStart datetime NULL,
    PolicyEnd   datetime NULL,
    MidnightRenewal int NULL,

    YearCol     int NULL,
    GP5         decimal (19,4) NULL,    GP4  decimal (19,4) NULL, GP3  decimal (19,4) NULL,
    GP2         decimal (19,4) NULL,    GP1  decimal (19,4) NULL, GP0  decimal (19,4) NULL,
    COM5        decimal (19,4) NULL,    COM4 decimal (19,4) NULL, COM3 decimal (19,4) NULL,
    COM2        decimal (19,4) NULL,    COM1 decimal (19,4) NULL, COM0 decimal (19,4) NULL,
    PAY5        decimal (19,4) NULL,    PAY4 decimal (19,4) NULL, PAY3 decimal (19,4) NULL,
    PAY2        decimal (19,4) NULL,    PAY1 decimal (19,4) NULL, PAY0 decimal (19,4) NULL,
    OSL5        decimal (19,4) NULL,    OSL4 decimal (19,4) NULL, OSL3 decimal (19,4) NULL,
    OSL2        decimal (19,4) NULL,    OSL1 decimal (19,4) NULL, OSL0 decimal (19,4) NULL,
    UPR5        decimal (19,4) NULL,    UPR4 decimal (19,4) NULL, UPR3 decimal (19,4) NULL,
    UPR2        decimal (19,4) NULL,    UPR1 decimal (19,4) NULL, UPR0 decimal (19,4) NULL,

)


EXECUTE spu_Report_GetCurrentPeriod @sub_branch_id , NULL ,  @dtCurrentPeriodEnd OUTPUT

IF @AgentShortName = 'ALL'

BEGIN

INSERT INTO #tempAgtPerfYear

    SELECT NULL,
        NULL,
        sf.agent_cnt,
        sf.product_code,
        class_of_business_id,
        (SELECT this_premium_home  where sf.transaction_type_code not like 'C%'),
        lead_Commission_value_home ,
        posting_period_year,
        transaction_type_code,
        (SELECT this_premium_home where sf.transaction_type_code like 'C_CP'),
    --      KB 9/1/02
        (SELECT this_premium_home where sf.transaction_type_code in ('C_CO','C_CR')), -- use the stats_deatils instead
        CASE WHEN datediff(day,sf.cover_start_date, @dtCurrentPeriodEnd) <= 0
            THEN (SELECT this_premium_home
                WHERE sf.transaction_type_code not like 'C%')
            ELSE
                (SELECT this_premium_home
                     *  datediff(day,@dtCurrentPeriodEnd,sf.expiry_date)
                        /(datediff(day,sf.cover_start_date,sf.expiry_date)+ isnull(p.is_midnight_renewal,0))
                WHERE sf.transaction_type_code not like 'C%'and (datediff(day,sf.cover_start_date,sf.expiry_date)+ isnull(p.is_midnight_renewal,0)) <> 0)
            END,
        sf.cover_start_date,                -- To calculate unearned premium
        sf.expiry_date,                     -- To calculate unearned premium
        isnull(p.is_midnight_renewal,0),    -- To calculate unearned premium
        NULL,
        NULL,   NULL,   NULL,   NULL,   NULL,   NULL,
        NULL,   NULL,   NULL,   NULL,   NULL,   NULL,
        NULL,   NULL,   NULL,   NULL,   NULL,   NULL,
        NULL,   NULL,   NULL,   NULL,   NULL,   NULL,
        NULL,   NULL,   NULL,   NULL,   NULL,   NULL
    FROM stats_folder sf
    LEFT OUTER JOIN stats_detail sd ON sd.stats_folder_cnt = sf.stats_folder_cnt
    LEFT OUTER JOIN product p on p.product_id = sf.product_id
    WHERE sd.stats_detail_type = 'GRS'
    AND isnull(sf.agent_cnt,0) <> 0

END
ELSE
BEGIN

INSERT INTO #tempAgtPerfYear

--debug
--DECLARE @AgentShortName varchar(20)
--DECLARE @dtCurrentPeriodEnd datetime
--set @agentshortname = 'SYKES'
--set @dtCurrentPeriodEnd = Getdate()
    SELECT NULL,
        NULL,
        sf.agent_cnt,
        sf.product_code,
        class_of_business_id,
        (SELECT this_premium_home  where sf.transaction_type_code not like 'C%'),
        lead_Commission_value_home ,
        posting_period_year,
        transaction_type_code,
        (SELECT this_premium_home where sf.transaction_type_code like 'C_CP'),
        (SELECT this_premium_home where sf.transaction_type_code in ('C_CO','C_CR')), -- use the stats_deatils instead
        CASE WHEN datediff(day,sf.cover_start_date, @dtCurrentPeriodEnd) <= 0
            THEN (SELECT this_premium_home
                WHERE sf.transaction_type_code not like 'C%')
            ELSE
                (SELECT this_premium_home
                    *  datediff(day,@dtCurrentPeriodEnd,sf.expiry_date)
                        /datediff(day,sf.cover_start_date,sf.expiry_date)+ isnull(p.is_midnight_renewal,0)
                WHERE sf.transaction_type_code not like 'C%' and (datediff(day,sf.cover_start_date,sf.expiry_date)+ isnull(p.is_midnight_renewal,0)) <> 0)
            END,
        sf.cover_start_date,                -- To calculate unearned premium
        sf.expiry_date,                     -- To calculate unearned premium
        isnull(p.is_midnight_renewal,0),    -- To calculate unearned premium
        NULL,
        NULL,   NULL,   NULL,   NULL,   NULL,   NULL,
        NULL,   NULL,   NULL,   NULL,   NULL,   NULL,
        NULL,   NULL,   NULL,   NULL,   NULL,   NULL,
        NULL,   NULL,   NULL,   NULL,   NULL,   NULL,
        NULL,   NULL,   NULL,   NULL,   NULL,   NULL
    FROM stats_folder sf
    LEFT OUTER JOIN stats_detail sd ON sd.stats_folder_cnt = sf.stats_folder_cnt
    LEFT OUTER JOIN product p on p.product_id = sf.product_id
    JOIN party py on py.party_cnt =  sf.agent_cnt
    WHERE sd.stats_detail_type = 'GRS'
    AND py.shortname = @agentshortname
    AND isnull(sf.agent_cnt,0) <> 0

END

UPDATE #tempAgtPerfYear

    SET @dDailyRate = GrossPremium / (datediff(day, PolicyStart, PolicyEnd) + MidnightRenewal)
    WHERE ((datediff(day,PolicyStart, PolicyEnd)+ MidnightRenewal) <> 0 AND PolicyEnd > @dtCurrentPeriodEnd)

--UPDATE #tempAgtPerfYear
    --SET UnearnedPremium = @dDailyRate * datediff(day, @dtCurrentPeriodEnd, PolicyEnd)
    --WHERE TransactionType not like 'C%'


UPDATE #tempAgtPerfYear
    SET AgentName = pa.name
    FROM Party pa
    WHERE AgentCnt = pa.party_cnt

UPDATE #tempAgtPerfYear
    SET AgentName = 'Direct'

    WHERE AgentCnt IS NULL

UPDATE #tempAgtPerfYear
    SET currentyear = @dtCurrentPeriodEnd
--  KB 8/1/2 No longer required as we are now using product, which we can pick up directly.
--UPDATE #tempAgtPerfYear
--  SET     ClassofBus  =   cb.description
--  FROM    Class_of_Business cb
--  WHERE   ClassofBusId    =   cb.class_of_business_id

UPDATE #tempAgtPerfYear
    SET Yearcol =  (year (CurrentYear) - PPyear )

UPDATE  #tempAgtPerfYear
    SET     GP5 =   GrossPremium,
        Com5 =  Commission,
        Pay5 =  ClaimsPaid,
        OSL5 =  OSLossRes,
        UPR5 =  UnearnedPremium

    WHERE Yearcol = 5


UPDATE  #tempAgtPerfYear
    SET     GP4 =   GrossPremium,
        Com4 =  Commission,
        Pay4 =  ClaimsPaid,
        OSL4 =  OSLossRes,
        UPR4 =  UnearnedPremium

    WHERE Yearcol = 4


UPDATE  #tempAgtPerfYear
    SET     GP3 =   GrossPremium,
        Com3 =  Commission,
        Pay3 =  ClaimsPaid,
        OSL3 =  OSLossRes,
        UPR3 =  UnearnedPremium

    WHERE Yearcol = 3


UPDATE  #tempAgtPerfYear
    SET     GP2 =   GrossPremium,
        Com2 =  Commission,
        Pay2 =  ClaimsPaid,
        OSL2 =  OSLossRes,
        UPR2 =  UnearnedPremium

    WHERE Yearcol = 2


UPDATE  #tempAgtPerfYear
    SET     GP1 =   GrossPremium,
        Com1 =  Commission,
        Pay1 =  ClaimsPaid,
        OSL1 =  OSLossRes,
        UPR1 =  UnearnedPremium

    WHERE Yearcol = 1


UPDATE  #tempAgtPerfYear
    SET     GP0 =   GrossPremium,
        Com0 =  Commission,
        Pay0 =  ClaimsPaid,
        OSL0 =  OSLossRes,
        UPR0 =  UnearnedPremium

    WHERE Yearcol = 0



select * from   #tempAgtPerfYear
drop    table   #tempAgtPerfYear


GO

