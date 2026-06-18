SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Claim_Recovery'
GO


CREATE PROCEDURE spu_Report_Claim_Recovery
    @Start_Date datetime,
    @End_Date datetime,
    @DateRange varchar(255),
    @DateType varchar(255)
AS
/*****************************************
**  Claims Recovery Report.
**
**  Created: P. Haynes 3/9/2001
**
**  Claim_Recovery.rpt
**
*****************************************/
--  c.claim_Number,
--  c.Policy_number,
--  c.Client_Name,
--  c.Insurer_Name,
--  c.loss_from_date,
--  c.Reported_Date,
--  c.Client_Short_name,
--  c.description ClaimDescription,

create table #tempClaims
(
    Claim_ID int null,
    Policy_ID int null,
    Claim_status_id int null,
    claim_Number varchar(60) null,
    Policy_number varchar(60) null,
    Client_Name varchar(60) null,
    Insurer_Name varchar(60) null,
    loss_from_date datetime null,
    Reported_date datetime null,
    Client_Short_name varchar(60),
    Description varchar(255)
)

if @dateType = 'Loss Date'
begin
INSERT INTO #tempClaims
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
end

else if @dateType = 'Reported Date'
begin
INSERT INTO #tempClaims
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
end
--

SELECT  r.Initial_Reserve,
    r.Revised_Reserve,
    r.Received_to_date,
    RecType.Description,
    c.claim_Number,
    c.Policy_number,
    c.Client_Name,
    c.Insurer_Name,
    c.loss_from_date,
    c.Reported_Date,
    c.Client_Short_name,
    c.description ClaimDescription,
    p.description ProductDescription,
    rt.description RiskTypeDescription,
    (r.Initial_reserve + r.revised_reserve) as IncurredAmount,
    ((r.Initial_reserve + r.revised_reserve) - r.Received_to_date) as CurrentReserve

FROM
    [Recovery] r
    join claim_peril cp on r.claim_peril_id = cp.claim_peril_id
    left join Recovery_type RecType on r.Recovery_type_id = RecType.Recovery_Type_ID
    join #TempClaims c on cp.claim_id = c.claim_id
    join Insurance_file I on c.Policy_id = I.insurance_file_cnt
    left join Product p on i.product_id = P.product_id
    left join claim_risk cr on c.claim_id = cr. claim_id
    left join Risk_type rt on cr.risk_type_id = rt.risk_type_Id

where   c.Claim_Status_id in (2, 4) and
    r.initial_reserve is not null or
    r.revised_reserve is not null or
    r.Received_to_date is not null

drop table #tempClaims

GO