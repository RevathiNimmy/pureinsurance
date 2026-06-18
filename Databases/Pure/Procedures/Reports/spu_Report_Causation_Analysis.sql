SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Causation_Analysis'
GO


CREATE PROCEDURE spu_Report_Causation_Analysis
    @Primary_cause varchar(255),
    @Start_Date datetime,
    @End_Date datetime,
    @DateRange varchar(255),
    @DateType varchar(255)
AS
/*****************************************
**  Causation Analysis report
**
**  Created: P. Haynes 5/9/2001
**  Modified 20/09/2001
**
**  Causation_Analysis.rpt
**
*****************************************/

-- Get all CLAIM RECORDS WITHIN DATE RANGE

create table #tempClaims
(
    Claim_ID int null,
    Policy_ID int null,
    Primary_Cause_ID int null,
    Secondary_Cause_ID int null,
    Claim_status_id int null,
    claim_Number varchar(60) null,
    Policy_number varchar(60) null,
    Client_Name varchar(60) null,
    Insurer_Name varchar(60) null,
    loss_from_date datetime null,
    Reported_date datetime null,
    Client_Short_name varchar(60),
    description varchar(255)
)

if @dateType = 'Loss Date'
begin
INSERT INTO #tempClaims
    SELECT Claim_ID, Policy_ID, Primary_cause_id, Secondary_Cause_ID, Claim_Status_ID, claim_number, policy_number, client_name,
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
    SELECT Claim_ID, Policy_ID, Primary_cause_id, Secondary_Cause_ID, Claim_Status_ID, claim_number, policy_number, client_name,
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
CREATE TABLE #tempCausationAnalysis
(
    Sum_Insured     money null,
    Initial_Reserve     money null,
    Revised_Reserve     money null,
    Incurred_Amount     money null,
    Current_Reserve     money null,
    Claim_Number        varchar(60) null,
    Policy_Number       varchar(60) null,
    Client_Name     varchar(60) null,
    Insurer_Name        varchar(60) null,
    loss_from_date      datetime null,
    Reported_Date       datetime null,
    Client_Short_name   varchar(60) null,
    ClaimDescription    varchar(255) null,
    ProductDescription  varchar(60) null,
    RiskDescription     varchar(60) null,
    PrimaryCause        varchar(60) null,
    SecondayCause       varchar(60) null,
    Paid_to_date        money null,
    PrimaryCauseID      varchar(4),
    SecondaryCauseID    varchar(4)
)
--
INSERT #tempCausationAnalysis

SELECT  r.Sum_insured,
    r.Initial_Reserve,
    r.Revised_Reserve,
    r.Initial_Reserve + r.Revised_reserve,
    r.Initial_Reserve + r.Revised_Reserve - r.Paid_to_date,
    c.claim_Number,
    c.Policy_number,
    c.Client_Name,
    c.Insurer_Name,
    c.loss_from_date,
    c.Reported_date,
    c.Client_Short_name,
    LEFT(c.description,255),
    p.description,
    rt.description,
    pc.description,
    sc.description,
    r.paid_to_date,
    c.Primary_cause_id,
    c.Secondary_Cause_id

FROM
    Reserve r
    join claim_peril cp on r.claim_peril_id = cp.claim_peril_id
    join #TempClaims c on cp.claim_id = c.claim_id
    join Insurance_file I on c.Policy_id = I.insurance_file_cnt
    left join Product p on i.product_id = P.product_id
    left join claim_risk cr on c.claim_id = cr. claim_id
    left join Risk_type rt on cr.risk_type_id = rt.risk_type_Id
    left join Primary_Cause pc on c.Primary_cause_id = pc.primary_cause_id
    left join Secondary_cause sc on c.Secondary_Cause_id = sc.Secondary_cause_id

where   c.Claim_Status_id in (2, 4)
--
if @Primary_cause = 'all'
select * from #tempCausationAnalysis
else
select * from #TempCausationAnalysis where Primarycause = @Primary_cause
--
DROP TABLE #tempCausationAnalysis
DROP TABLE #TempClaims
GO