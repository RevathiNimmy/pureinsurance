SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_OutStanding_Claims'
GO


CREATE PROCEDURE spu_Report_OutStanding_Claims
                @SalvageAndTPRecovery varchar(255),
                @Start_Date datetime,
                @End_Date datetime,
                @DateRange varchar(255),
                @DateType varchar(255)
AS
/**********************************************************************************************************************************
** Created by Jude Killip
** 22/08/2000
** RSA Reports - Outstanding_Claims.rpt
**  Created with dummy data to build the report
**********************************************************************************************************************************
** 09/11/2000   Thinh Nguyen    real data
**
** 30/04/2001   Jude Killip     include Recoveries, add in the parameter
**
** 04/05/2001   Jude Kilip      fix dodgy query (adding too many times)
**
** 03/07/2001   Jude Killip     c.description 255
**                              limit to non-closed or reclosed claims, and filter out claims with zero values
**
** 20/09/2001   Jude Killip     redo the Salvage & Recovery option
**                              retrieve Reserve and Recovery data in separate chunks
**
** 22/10/2001   Jude Killip     calculate home currency values
**
** 31/10/2001   Jude Killip     retrieve currency rates properly
***********************************************************************************************************************************
** VER      DATE        WHO     WHAT
** 1.01     29/01/2002  JMK     Add date parameters
***********************************************************************************************************************************/
/*
    Claim status id constants
    1 = Provisional Open Claim
    2 = Live Open Claim
    3 = Closed
    4 = ReOpen
    5 = ReClosed
*/

/*
SalvageAndTPRecovery:
    exclude
    include
    only
DateRange:
    Specify Dates
    Today
    Yesterday
    This Week
    Last Full Week
    This Month
    Last Full Month
DateType:
    Loss Date
    Reported Date
*/
/*
--for testing
DECLARE @SalvageAndTPRecovery varchar (10),
        @Start_Date datetime,
        @End_Date datetime,
        @DateRange  varchar(20),
        @DateType varchar(20)
SELECT @SalvageAndTPRecovery = 'only',
        @start_date = dateadd(day,-55,getdate()),
        @end_date = getdate(),
        @DateRange = 'This Month',
        @DateType = 'Loss Date'
*/

CREATE TABLE #tempClaims
(
    ClaimID int
)

IF @dateType = 'Loss Date'
BEGIN
    INSERT INTO #tempClaims
        SELECT Claim_id
        FROM Claim
        WHERE (
        @DateRange = 'specify dates' AND
            (
            datediff(day, @start_date, loss_from_date) >=0
            AND datediff(day, loss_from_date, @end_date) >=0
            )
        OR
        @DateRange = 'yesterday' AND
        datediff (day, loss_from_date, getdate())= 1
        OR
        @DateRange = 'today' AND
        datediff (day, loss_from_date, getdate())= 0
        OR
        @DateRange = 'last full week' AND
        datediff (week, loss_from_date, getdate())= 1
        OR
        @DateRange = 'this week' AND
        datediff (week, loss_from_date, getdate())= 0
        OR
        @DateRange = 'last full month' AND
        datediff (month, loss_from_date, getdate())= 1
        OR
        @DateRange = 'this month' AND
        datediff (month, loss_from_date, getdate())= 0
        )
END
ELSE
BEGIN
    INSERT INTO #tempClaims
        SELECT Claim_id
        FROM Claim
        WHERE (
        @DateRange = 'specify dates' AND
            (
            datediff(day, @start_date, Reported_date) >=0
            AND datediff(day, Reported_date, @end_date) >=0
            )
        OR
        @DateRange = 'yesterday' AND
        datediff (day, Reported_date, getdate())= 1
        OR
        @DateRange = 'today' AND
        datediff (day, Reported_date, getdate())= 0
        OR
        @DateRange = 'last full week' AND
        datediff (week, Reported_date, getdate())= 1
        OR
        @DateRange = 'this week' AND
        datediff (week, Reported_date, getdate())= 0
        OR
        @DateRange = 'last full month' AND
        datediff (month, Reported_date, getdate())= 1
        OR
        @DateRange = 'this month' AND
        datediff (month, Reported_date, getdate())= 0
        )
END

CREATE TABLE #tempRSAOutstClaims
(
    TempID int IDENTITY,
    RiskTypeCode varchar (10) NULL,
    RiskTypeDesc varchar (255) NULL,
    ReserveType varchar (255) NULL,
    ClaimNumber varchar (30) NULL,
    AgentCode varchar (10) NULL,
    InsuranceRef varchar (30) NULL,
    ClientCode varchar (10) NULL,
    ClientName varchar (60) NULL,
    LossFromDate datetime NULL,
    ClaimDesc varchar (255) NULL,
    CurrencyRate money NULL,
    CurrencyID int NULL,
    InitialReserve money NULL,
    RevisedReserve money NULL,
    Payments money NULL
)
IF @SalvageAndTPRecovery = 'exclude'
BEGIN
    -- print 'get outstanding claims with Reserves '
    INSERT INTO #tempRSAOutstClaims
        SELECT rt.code,
            rt.description,
            (select description from reserve_type where Reserve_type_id = res.Reserve_type_id),
            c.claim_number,
            c.insurer_short_name,
            c.policy_number,
            c.client_short_name,
            c.client_name,
            c.loss_from_date,
            LEFT(c.description,255),
            1,
            c.currency_id,
            res.initial_reserve,
            res.revised_reserve,
            res.paid_to_date
        FROM #tempClaims tc
        JOIN claim c            ON tc.ClaimID = c.claim_id
        JOIN Claim_Peril cp     ON c.claim_id = cp.claim_id                 -- Claim_Peril link
        JOIN Reserve res        ON cp.claim_peril_id = res.claim_peril_id
        JOIN Risk r             ON c.risk_type_id = r.risk_cnt
        JOIN Risk_Type rt       ON rt.risk_type_id = r.risk_type_id
        WHERE c.claim_status_id not in (3,5)
END
ELSE
IF @SalvageAndTPRecovery = 'only'
BEGIN
    -- print 'get outstanding claims with Recoveries '
    INSERT INTO #tempRSAOutstClaims
        SELECT rt.code,
            rt.description,
            (select description from recovery_type where recovery_type_id = rec.recovery_type_id),
            c.claim_number,
            c.insurer_short_name,
            c.policy_number,
            c.client_short_name,
            c.client_name,
            c.loss_from_date,
            LEFT(c.description,255),
            1,
            c.currency_id,
            rec.initial_reserve,
            rec.revised_reserve,
            rec.received_to_date
        FROM #tempClaims tc
        JOIN claim c            ON tc.ClaimID = c.claim_id
        JOIN Claim_Peril cp     ON c.claim_id = cp.claim_id                 -- Claim_Peril link
        JOIN [Recovery] rec     ON cp.claim_peril_id = rec.claim_peril_id
        JOIN Risk r             ON c.risk_type_id = r.risk_cnt
        JOIN Risk_Type rt       ON rt.risk_type_id = r.risk_type_id
        WHERE c.claim_status_id not in (3,5)
END
ELSE
BEGIN

    -- print 'get outstanding claims, Reserves & Recoveries'
    -- print 'Reserves first...'
    INSERT INTO #tempRSAOutstClaims
        SELECT rt.code,
            rt.description,
            (select description from reserve_type where Reserve_type_id = res.Reserve_type_id),
            c.claim_number,
            c.insurer_short_name,
            c.policy_number,
            c.client_short_name,
            c.client_name,
            c.loss_from_date,
            LEFT(c.description,255),
            1,
            c.currency_id,
            res.initial_reserve,
            res.revised_reserve,
            res.paid_to_date
        FROM #tempClaims tc
        JOIN claim c            ON tc.ClaimID = c.claim_id
        JOIN Claim_Peril cp     ON c.claim_id = cp.claim_id                 -- Claim_Peril link
        JOIN Reserve res        ON cp.claim_peril_id = res.claim_peril_id
        JOIN Risk r             ON c.risk_type_id = r.risk_cnt
        JOIN Risk_Type rt       ON rt.risk_type_id = r.risk_type_id
        WHERE c.claim_status_id not in (3,5)

    -- print '...then Recoveries'
    INSERT INTO #tempRSAOutstClaims
        SELECT rt.code,
            rt.description,
            (select description from recovery_type where recovery_type_id = rec.recovery_type_id),
            c.claim_number,
            c.insurer_short_name,
            c.policy_number,
            c.client_short_name,
            c.client_name,
            c.loss_from_date,
            LEFT(c.description,255),
            1,
            c.currency_id,
            rec.initial_reserve,
            rec.revised_reserve,
            rec.received_to_date
        FROM #tempClaims tc
        JOIN claim c            ON tc.ClaimID = c.claim_id
        JOIN Claim_Peril cp     ON c.claim_id = cp.claim_id                 -- Claim_Peril link
        JOIN [Recovery] rec     ON cp.claim_peril_id = rec.claim_peril_id
        JOIN Risk r             ON c.risk_type_id = r.risk_cnt
        JOIN Risk_Type rt       ON rt.risk_type_id = r.risk_type_id
        WHERE c.claim_status_id not in (3,5)

END

DROP TABLE #tempClaims

-- Use cursor to fetch currency rate details
-- Cursor variables
DECLARE @TempID int,
        @LossFromDate datetime,
        @CurrencyRate money,
        @CurrencyId int

-- Additional variable for calculating home currency values
DECLARE @CCYeffectiveFrom datetime

DECLARE OSClaims_cursor CURSOR FAST_FORWARD FOR
    SELECT TempID,
            LossFromDate,
            CurrencyRate,
            CurrencyId
     FROM #tempRSAOutstClaims

OPEN    OSClaims_cursor

    FETCH NEXT FROM OSClaims_cursor
    INTO    @TempID,
            @LossFromDate,
            @CurrencyRate,
            @CurrencyId

    WHILE @@FETCH_STATUS = 0 BEGIN
        IF @CurrencyId <> 26            -- 26 = id of home currency (leave as it is)
        BEGIN
            print 'not home currency'
            SELECT @CCYeffectiveFrom = (SELECT max(effective_from)
                                        FROM  CurrencyRate
                                        WHERE currency_id = @CurrencyId
                                        AND effective_from <= @LossFromDate
                                        GROUP BY currency_id)

            IF @CCYeffectiveFrom IS NOT NULL
            BEGIN
                SELECT @CurrencyRate = (SELECT rate_against_base
                                            FROM  CurrencyRate
                                            WHERE currency_id = @CurrencyId
                                            AND effective_from = @CCYeffectiveFrom)

                UPDATE #tempRSAOutstClaims
                    SET  CurrencyRate = @CurrencyRate
                    WHERE TempID = @TempID
            END
        END

        FETCH NEXT FROM OSClaims_cursor
        INTO    @TempID,
                @LossFromDate,
                @CurrencyRate,
                @CurrencyId

    END
CLOSE OSClaims_cursor
DEALLOCATE OSClaims_cursor

-- print 'select from temp table, calculating currencies'
SELECT RiskTypeCode,
        RiskTypeDesc,
        ReserveType,
        ClaimNumber,
        AgentCode,
        InsuranceRef,
        ClientCode,
        ClientName,
        LossFromDate,
        ClaimDesc,
        InitialReserve*CurrencyRate InitialReserve,
        RevisedReserve*CurrencyRate RevisedReserve,
        Payments*CurrencyRate Payments
FROM #tempRSAOutstClaims

DROP TABLE #tempRSAOutstClaims

GO
