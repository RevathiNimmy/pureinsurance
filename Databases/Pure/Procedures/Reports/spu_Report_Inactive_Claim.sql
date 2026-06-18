SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Inactive_Claims'
GO


CREATE PROCEDURE spu_Report_Inactive_Claims
                @LastModifiedDate datetime
AS
/*****************************************
**  INACTIVE CLAIMS REPORT
**
**  Created: P. Haynes 3/9/2001
**
**  InactiveClaims.rpt
**
*****************************************/


--SELECT    c.Policy_number, c.Claim_Number, c.Description, c.Last_Modified_Date,
--  c.Client_Name, c.Insurer_Name, c.short_name,
--  r.Initial_reserve, r.Revised_reserve, r.Paid_to_date,
--  r.sum_insured,
--  ((R.Initial_Reserve + R.Revised_Reserve) - r.Paid_to_date) as Current_Reserve,
--  (R.Initial_Reserve + R.Revised_Reserve) as IncurredAmount

--FROM claim c
--join Claim_peril cp on c.claim_id = cp.claim_id
--join reserve r on r.claim_peril_id = cp.claim_peril_id

--WHERE     c.last_modified_date <= @LastModifiedDate
--and   c.Claim_Status_id in (2, 4)

SELECT  r.Sum_insured,
    r.Initial_Reserve,
    r.Revised_Reserve,
    r.Initial_Reserve + r.Revised_reserve as IncurredAmount,
    r.Initial_Reserve + r.Revised_Reserve - r.Paid_to_date as CurrentReserve,
    c.claim_Number,
    c.Policy_number,
    c.Client_Name,
    c.Insurer_Name,
    c.loss_from_date,
    c.Reported_date,
    c.Last_Modified_Date,
    c.Client_Short_name,
    c.description as ClaimDescription,
    p.description as ProductDescription,
    rt.description as RiskDescription,
    r.paid_to_date

FROM
    Reserve r
    join claim_peril cp on r.claim_peril_id = cp.claim_peril_id
    join Claim c on cp.claim_id = c.claim_id
    join Insurance_file I on c.Policy_id = I.insurance_file_cnt
    left join Product p on i.product_id = P.product_id
    left join claim_risk cr on c.claim_id = cr. claim_id
    left join Risk_type rt on cr.risk_type_id = rt.risk_type_Id

where   c.Claim_Status_id in (2, 4)
and     c.last_modified_date <= @LastModifiedDate