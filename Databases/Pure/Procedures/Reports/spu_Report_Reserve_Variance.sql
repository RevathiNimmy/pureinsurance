SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Reserve_Variance'
GO


CREATE PROCEDURE spu_Report_Reserve_Variance
                @sAgent varchar(255),
                @Start_Date datetime,
                @End_Date datetime,
                @DateRange varchar(255),
                @DateType varchar(255)
AS

/*****************************************
**  Reserve Variance report
**
**  Created: P. Haynes 4/9/2001
**
**  Reserve_Variance.rpt
**
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
DECLARE @sAgent varchar(60),
        @Start_Date datetime,
        @End_Date datetime,
        @DateRange  varchar(20),
        @DateType varchar(20)
SELECT @sAgent = 'ALL',
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

CREATE TABLE #tempReserveVariance
(
    Initial_Reserve     money null,
    Revised_Reserve     money null,
    Sum_Insured     money null,
    Claim_Number        varchar(60) null,
    Policy_Number       varchar(60) null,
    Client_Name     varchar(60) null,
    Insurer_Name        varchar(60) null,
    loss_from_date      datetime null,
    Client_Short_name   varchar(60) null,
    description     varchar(255) null,
    ProductDescription  varchar(60) null,
    RiskDescription     varchar(60) null,
)
INSERT #tempReserveVariance

SELECT  r.Initial_Reserve,
    r.Revised_Reserve,
    r.Sum_insured,
    c.claim_Number,
    c.Policy_number,
    c.Client_Name,
    c.Insurer_Name,
    c.loss_from_date,
    c.Client_Short_name,
    LEFT(c.description,255),
    p.description,
    rt.description

FROM
    Reserve r
    join claim_peril cp on r.claim_peril_id = cp.claim_peril_id
    join claim c on cp.claim_id = c.claim_id
    JOIN #tempClaims tc ON tc.ClaimID = c.claim_id
    join Insurance_file I on c.Policy_id = I.insurance_file_cnt
    left join Product p on i.product_id = P.product_id
    left join claim_risk cr on c.claim_id = cr. claim_id
    left join Risk_type rt on cr.risk_type_id = rt.risk_type_Id

where   c.Claim_Status_id in (2, 4)

DROP TABLE #tempClaims

IF @sAgent <> 'ALL'
    SELECT * FROM #tempReserveVariance
    WHERE Insurer_Name Like @sAgent
else
    SELECT * FROM #tempReserveVariance

DROP TABLE #tempReserveVariance

GO