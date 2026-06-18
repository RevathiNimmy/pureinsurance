SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Claims_Opened_Warning'
GO


CREATE PROCEDURE spu_Report_Claims_Opened_Warning
                @iElapsedDays int,
                @Start_Date datetime,
                @End_Date datetime,
                @DateRange varchar(255),
                @DateType varchar(255)
AS

/*****************************************
**  Claims Opened Warning report
**
**  Created: P. Haynes 9/9/2001
**
**  Claims_Opened_Warning.rpt
**
**
**    Claim status id constants
**    1 = Provisional Open Claim
**    2 = Live Open Claim
**    3 = Closed
**    4 = ReOpen
**    5 = ReClosed
**
***********************************************************************************************************************************
** VER      DATE        WHO     WHAT
** 1.01     29/01/2002  JMK     Add test details
**                              Re-name script to match sp name
**
** 1.02     20/02/2002  JMK     Amend join to insurance folder for getting inception date
**                              Amend WHERE to include policies with elapsed days = @iElapsedDays
**                              Retrieve product code as well as description
**                              Match data types of temp tables with source tables
***********************************************************************************************************************************/
--for testing
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
DECLARE @iElapsedDays int,
    @Start_Date datetime,
    @End_Date datetime,
    @DateRange varchar(20),
    @DateType varchar(20)
SELECT @iElapsedDays = 4,
    @Start_Date = dateadd(day, -100, getdate()),
    @End_Date = getdate(),
    @DateRange = 'Specify Dates',
    @DateType = 'Loss Date'
*/
create table #tempClaim
(
    claim_id int,
    Policy_id int,
    Claim_Status_ID int,
    claim_number varchar(30),
    policy_number varchar(30),
    client_name varchar(60),
    insurer_name varchar(60),
    loss_from_date datetime,
    reported_date datetime,
    client_short_name varchar(10),
    description varchar(255)
)

IF @dateType = 'Loss Date'
BEGIN
    INSERT INTO #tempClaim
        SELECT Claim_ID, Policy_ID, Claim_Status_ID, claim_number, policy_number, client_name,
            Insurer_Name, loss_from_date, Reported_Date, Client_Short_name, LEFT(description,255)
        FROM claim
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
    --if @dateType = 'Reported Date'
BEGIN
    INSERT INTO #tempClaim
        SELECT Claim_ID, Policy_ID, Claim_Status_ID, claim_number, policy_number, client_name,
            Insurer_Name, loss_from_date, Reported_Date, Client_Short_name, LEFT(description,255)
        FROM claim
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



--
CREATE TABLE #tempClaimsOpenendWarning
(
    Policy_Number           varchar(30) null,
    Claim_Number            varchar(30) null,
    ClaimDescription        varchar(255) null,
    Insurer_Name            varchar(60) null,
    Client_Name             varchar(60) null,
    loss_from_date          datetime null,
    ClientShortName         varchar(10) null,
    claimperildescription   varchar(50) null,
    Sum_Insured             money null,
    Initial_Reserve         money null,
    Revised_Reserve         money null,
    Paid_to_date            money null,
    CurrentReserve          money null,
    IncurredAmount          money null,
    PaymentID               int null,
    dateofpayment           datetime null,
    PaymentAmount           money null,
    ReserveTypeDescription  varchar(50) null,
    ProductDescription      varchar(255) null,
    ProductCode             varchar(10) null,
    RiskDescription         varchar(255) null,
    Insurer                 varchar(60) null,
    InceptionDate           datetime null,
    Days                    int
)

INSERT INTO #tempClaimsOpenendWarning

    SELECT
        c.policy_number,
        c.claim_number,
        LEFT(c.description,255) claimdescription,
        c.insurer_name,
        c.client_name,
        c.loss_from_date,
        c.client_short_name,
        cp.description claimperildescription,
        r.sum_Insured,
        r.initial_reserve,
        r.revised_reserve,
        r.paid_to_date,
        ((r.Initial_reserve + r.Revised_reserve) - r.Paid_to_date),
        (r.Initial_reserve + r.Revised_reserve),
        p.payment_id,
        p.date_of_payment,
        p.amount,
        rt.description,
        Prod.description,
        Prod.code,
        riskT.description,
        c.insurer_name,
        ifol.Inception_date,
        --(SELECT inception_date FROM insurance_folder WHERE
        Datediff(dd, ifol.Inception_date, c.Loss_from_date)

    FROM
        #TempClaim c join claim_peril cp on c.claim_id = cp.claim_id
        join reserve r on r.claim_Peril_Id = cp.claim_peril_id
        join payment p on r.reserve_id = p.reserve_id
        join reserve_type rt on r.reserve_type_Id = rt.reserve_type_id
        join Insurance_file I on c.Policy_id = I.insurance_file_cnt
        left join Product Prod on i.product_id = Prod.product_id
        left join claim_risk cr on c.claim_id = cr. claim_id
        left join Risk_type riskT on cr.risk_type_id = riskT.risk_type_Id
        --join Insurance_Folder IFol on I.insurance_file_id = IFol.insurance_folder_id
        join Insurance_Folder IFol on I.insurance_folder_cnt = IFol.insurance_folder_cnt

    WHERE
        c.claim_status_id in (2, 4) and
        c.Loss_from_date <= DateAdd(dd, @iElapsedDays+1, ifol.Inception_date)

SELECT * FROM #tempClaimsOpenendWarning

DROP TABLE #tempClaimsOpenendWarning
DROP TABLE #tempClaim
GO
