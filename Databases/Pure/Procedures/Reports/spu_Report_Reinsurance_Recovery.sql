SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Reinsurance_Recovery'
GO


CREATE PROCEDURE spu_Report_Reinsurance_Recovery
    @Start_Date datetime,
    @End_Date datetime,
    @DateRange varchar(255),
    @DateType varchar(255)
AS
/*************************************************
**  Outstadning External Reinsurance Recovery REPORT
**
**  Created: P. Haynes 17/9/2001
**
**  Reinsurance_Recovery.rpt
**
***********************************************************************************************************************************
** VER      DATE        WHO     WHAT
** 1.01     29/01/2002  JMK     Add date parameters
**
** 1.02     01/02/2001  JMK     Amend Party join
**                              also party.party_cnt = claim_party.party_id
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
DECLARE @Start_Date datetime,
        @End_Date datetime,
        @DateRange  varchar(20),
        @DateType varchar(20)
SELECT @start_date = dateadd(day,-55,getdate()),
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

SELECT  distinct cp.claim_id,
    cp.party_id,
    cper.claim_peril_id,
    cp.share,
    r.reserve_id,
    (((r.initial_reserve + r.revised_reserve) - r.paid_to_date)) / 100 * cp.share as Share_Value,
    party.resolved_Name,
    c.claim_Number,
    c.Policy_number,
    c.Client_Name,
    c.Insurer_Name,
    c.loss_from_date,
    c.Client_Short_name,
    c.description ClaimDescription,
    p.description ProductDescription,
    rt.description RiskTypeDescription,
    r.initial_reserve,
    r.revised_reserve,
    r.paid_to_date,
    (r.initial_reserve + r.revised_reserve) - r.paid_to_date as Incurred_Amount

FROM #tempClaims tc
    JOIN claim c            ON tc.ClaimID = c.claim_id
    join claim_party cp     ON cp.claim_id = c.claim_id
    join Insurance_file I   on c.Policy_id = I.insurance_file_cnt
    left join Product p     on i.product_id = P.product_id
    left join claim_risk cr on c.claim_id = cr. claim_id
    left join Risk_type rt  on cr.risk_type_id = rt.risk_type_Id
    join claim_peril cPer   on cPer.claim_id = c.claim_id
    join reserve r          on r.Claim_peril_id = cper.claim_peril_id
    join party              on cp.party_id = party.party_cnt

where   c.Claim_Status_id in (2, 4)
    and cp.Insurer_type = 1
    and r.revision_count = (Select max(revision_Count)

    FROM    reserve
    where   reserve.claim_peril_id = cper.claim_peril_id)
    and (r.initial_reserve + r.revised_reserve) - r.paid_to_date > 0

DROP TABLE #tempClaims
GO