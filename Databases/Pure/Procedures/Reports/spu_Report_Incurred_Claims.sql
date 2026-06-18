SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Incurred_Claims'
GO


CREATE PROCEDURE spu_Report_Incurred_Claims
                    @IncludeRecoveries varchar(255),
                    @IncludeClosed varchar(255),
                    @Start_Date datetime,
                    @End_Date datetime,
                    @DateRange varchar(255)
AS
/*****************************************
**  Incurred Claims Report.
**
**  Created: P. Haynes 3/9/2001
**
**  Incurred_Claims.rpt
**
***********************************************************************************************************************************
** VER      DATE        WHO     WHAT
** 1.01     25/01/2002  JMK     Add date parameters
**                              Add Include Closed parameter
**
** 1.02     15/02/2002  JMK     Increase description fields to accept db maximum
**
***********************************************************************************************************************************/
SET NOCOUNT ON
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
*/
/*
DECLARE @IncludeRecoveries varchar (3),
        @IncludeClosed varchar (5),
        @Start_Date datetime,
        @End_Date datetime,
        @DateRange  varchar(20)
SELECT @IncludeRecoveries = 'yes', @IncludeClosed = 'no',
        @start_date = dateadd(day,-55,getdate()),
        @end_date = getdate(),
        @DateRange = 'This Month'
*/
CREATE TABLE #TempClaim
(
    Initial_Reserve     money null,
    Revised_Reserve     money null,
    Sum_insured         money null,
    Paid_to_Date        money null,
    claim_Number        varchar(60),
    Policy_number       varchar(60),
    Client_Name         varchar(60),
    Insurer_Name        varchar(60),
    loss_from_date      datetime null,
    Client_Short_name   varchar(60),
    ClaimDescription    varchar(255),
    ProductDescription  varchar(255),
    RiskTypeDescription varchar(255),
    IncurredAmount      money null,
    CurrentReserve      money null,
    IsRecovery          varchar(3)
)

IF @IncludeRecoveries <> 'Yes'
BEGIN
    --print 'insert1: @IncludeRecoveries = ' + @IncludeRecoveries
    --print '@IncludeClosed = ' + @IncludeClosed
    INSERT INTO #tempClaim

    SELECT  r.Initial_Reserve,
        r.Revised_Reserve,
        r.Sum_insured,
        r.Paid_to_Date,
        c.claim_Number,
        c.Policy_number,
        c.Client_Name,
        c.Insurer_Name,
        c.loss_from_date,
        c.Client_Short_name,
        LEFT(c.description,255) ClaimDescription,
        p.description ProductDescription,
        rt.description RiskTypeDescription,
        (r.Initial_reserve + r.revised_reserve) as IncurredAmount,
        ((r.Initial_reserve + r.revised_reserve) - r.Paid_to_Date) as CurrentReserve,
        'no'

    FROM
        Reserve r
        join claim_peril cp on r.claim_peril_id = cp.claim_peril_id
        join claim c on cp.claim_id = c.claim_id
        join Insurance_file I on c.Policy_id = I.insurance_file_cnt
        left join Product p on i.product_id = P.product_id
        left join claim_risk cr on c.claim_id = cr. claim_id
        left join Risk_type rt on cr.risk_type_id = rt.risk_type_Id

    WHERE (
        (c.Claim_Status_id in (2, 4) AND @IncludeClosed = 'no')
        OR
        (c.Claim_Status_id <> 1 AND @IncludeClosed = 'yes')
        )
    AND (
        isnull(r.initial_reserve,0) <> 0 OR
        isnull(r.revised_reserve,0) <> 0 OR
        isnull(r.paid_to_date,0) <> 0
        )
    AND (
        @DateRange = 'specify dates' AND
            (
            datediff(day, @start_date, c.loss_from_date) >=0
            AND
            datediff(day, c.loss_from_date, @end_date) >=0
            )
        OR
        (@DateRange = 'yesterday' AND
        datediff (day, c.loss_from_date, getdate())= 1)
        OR
        (@DateRange = 'today' AND
        datediff (day, c.loss_from_date, getdate())= 0)
        OR
        (@DateRange = 'last full week' AND
        datediff (week, c.loss_from_date, getdate())= 1)
        OR
        (@DateRange = 'this week' AND
        datediff (week, c.loss_from_date, getdate())= 0)
        OR
        (@DateRange = 'last full month' AND
        datediff (month, c.loss_from_date, getdate())= 1)
        OR
        (@DateRange = 'this month' AND
        datediff (month, c.loss_from_date, getdate())= 0)
        )

END
ELSE
BEGIN
    --print 'insert2: @IncludeRecoveries = ' + @IncludeRecoveries
    --print '@IncludeClosed = ' + @IncludeClosed
    INSERT INTO #tempClaim

    SELECT  r.Initial_Reserve,
        r.Revised_Reserve,
        r.Sum_insured,
        r.Paid_to_Date,
        c.claim_Number,
        c.Policy_number,
        c.Client_Name,
        c.Insurer_Name,
        c.loss_from_date,
        c.Client_Short_name,
        LEFT(c.description,255),
        p.description,
        rt.description RiskTypeDescription,
        (r.Initial_reserve + r.revised_reserve),
        ((r.Initial_reserve + r.revised_reserve) - r.Paid_to_Date),
        'no'

    FROM
        Reserve r
        join claim_peril cp on r.claim_peril_id = cp.claim_peril_id
        join claim c on cp.claim_id = c.claim_id
        join Insurance_file I on c.Policy_id = I.insurance_file_cnt
        left join Product p on i.product_id = P.product_id
        left join claim_risk cr on c.claim_id = cr. claim_id
        left join Risk_type rt on cr.risk_type_id = rt.risk_type_Id

    WHERE (
        (c.Claim_Status_id in (2, 4) AND @IncludeClosed = 'no')
        OR
        (c.Claim_Status_id <> 1 AND @IncludeClosed = 'yes')
        )
    AND (
        isnull(r.initial_reserve,0) <> 0 OR
        isnull(r.revised_reserve,0) <> 0 OR
        isnull(r.paid_to_date,0) <> 0
        )
    AND (
        @DateRange = 'specify dates' AND
            (
            datediff(day, @start_date, c.loss_from_date) >=0
            AND
            datediff(day, c.loss_from_date, @end_date) >=0
            )
        OR
        (@DateRange = 'yesterday' AND
        datediff (day, c.loss_from_date, getdate())= 1)
        OR
        (@DateRange = 'today' AND
        datediff (day, c.loss_from_date, getdate())= 0)
        OR
        (@DateRange = 'last full week' AND
        datediff (week, c.loss_from_date, getdate())= 1)
        OR
        (@DateRange = 'this week' AND
        datediff (week, c.loss_from_date, getdate())= 0)
        OR
        (@DateRange = 'last full month' AND
        datediff (month, c.loss_from_date, getdate())= 1)
        OR
        (@DateRange = 'this month' AND
        datediff (month, c.loss_from_date, getdate())= 0)
        )

    --
    --print 'insert3: @IncludeRecoveries = ' + @IncludeRecoveries
    --print '@IncludeClosed = ' + @IncludeClosed
    INSERT INTO #tempClaim

    SELECT  r.Initial_Reserve,
        r.Revised_Reserve,
        null,
        r.Received_to_date,
        c.claim_Number,
        c.Policy_number,
        c.Client_Name,
        c.Insurer_Name,
        c.loss_from_date,
        c.Client_Short_name,
        LEFT(c.description,255),
        p.description,
        rt.description RiskTypeDescription,
        (r.Initial_reserve + r.revised_reserve),
        ((r.Initial_reserve + r.revised_reserve) - r.Received_to_date),
        'yes'

    FROM
        [Recovery] r
        join claim_peril cp on r.claim_peril_id = cp.claim_peril_id
        join Claim c on cp.claim_id = c.claim_id
        join Insurance_file I on c.Policy_id = I.insurance_file_cnt
        left join Product p on i.product_id = P.product_id
        left join claim_risk cr on c.claim_id = cr. claim_id
        left join Risk_type rt on cr.risk_type_id = rt.risk_type_Id

    WHERE (
        (c.Claim_Status_id in (2, 4) AND @IncludeClosed = 'no')
        OR
        (c.Claim_Status_id <> 1 AND @IncludeClosed = 'yes')
        )
    AND (
        isnull(r.initial_reserve,0) <> 0 OR
        isnull(r.revised_reserve,0) <> 0 OR
        isnull(r.Received_to_date,0) <> 0
        )
    AND (
        @DateRange = 'specify dates' AND
            (
            datediff(day, @start_date, c.loss_from_date) >=0
            AND
            datediff(day, c.loss_from_date, @end_date) >=0
            )
        OR
        (@DateRange = 'yesterday' AND
        datediff (day, c.loss_from_date, getdate())= 1)
        OR
        (@DateRange = 'today' AND
        datediff (day, c.loss_from_date, getdate())= 0)
        OR
        (@DateRange = 'last full week' AND
        datediff (week, c.loss_from_date, getdate())= 1)
        OR
        (@DateRange = 'this week' AND
        datediff (week, c.loss_from_date, getdate())= 0)
        OR
        (@DateRange = 'last full month' AND
        datediff (month, c.loss_from_date, getdate())= 1)
        OR
        (@DateRange = 'this month' AND
        datediff (month, c.loss_from_date, getdate())= 0)
        )

END
--
SET NOCOUNT OFF
SELECT * FROM #tempClaim

DROP TABLE #tempClaim
GO