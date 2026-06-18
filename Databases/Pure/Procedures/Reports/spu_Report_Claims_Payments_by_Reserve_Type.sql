SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Claim_Payments_By_Reserve_Type'
GO


CREATE PROCEDURE spu_Report_Claim_Payments_By_Reserve_Type
    @start_date     datetime,
    @end_date   datetime
AS
/*****************************************
**  Payment by Reserve Type report
**
**  Created: P. Haynes 5/9/2001
**
**  Payment_by_Reserve_Type.rpt
**
*****************************************/
/*
** 1.01 14/12/2001  JMK     Amend parameters
*****************************************/
CREATE TABLE #tempPaymentsByReserveType
(
    Policy_Number       varchar(60) null,
    Claim_Number        varchar(60) null,
    ClaimDescription    varchar(255) null,
    Insurer_Name        varchar(60) null,
    Client_Name     varchar(60) null,
    loss_from_date      datetime null,
    ClientShortName     varchar(60) null,
    claimperildescription   varchar(60) null,
    Sum_Insured     money null,
    Initial_Reserve     money null,
    Revised_Reserve     money null,
    Current_Reserve     money null,
    Incurred_Amount     money null,
    Paid_to_date        money null,
    PaymentID       varchar(4),
    dateofpayment       datetime null,
    PaymentAmount       money null,
    ReserveTypeDescription  varchar(60) null,
    ProductDescription  varchar(60) null,
    RiskDescription     varchar(60) null,
    Insurer         varchar(60) null
)

INSERT #tempPaymentsByReserveType

select
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
    ((r.Initial_reserve + r.Revised_reserve) - r.Paid_to_date),
    r.Initial_reserve + r.Revised_reserve,
    r.paid_to_date,
    p.payment_id,
    p.date_of_payment,
    p.amount,
    rt.description,
    Prod.description,
    riskT.description,
    c.insurer_name

from
    claim c join claim_peril cp on c.claim_id = cp.claim_id
    join reserve r on r.claim_Peril_Id = cp.claim_peril_id
    join payment p on r.reserve_id = p.reserve_id
    join reserve_type rt on r.reserve_type_Id = rt.reserve_type_id

    join Insurance_file I on c.Policy_id = I.insurance_file_cnt
    left join Product Prod on i.product_id = Prod.product_id
    left join claim_risk cr on c.claim_id = cr. claim_id
    left join Risk_type riskT on cr.risk_type_id = riskT.risk_type_Id

where
    c.claim_status_id in (2, 4)

select * from #tempPaymentsByReserveType where dateofpayment >= @start_date
and dateofpayment <= @end_date
and paymentamount <> 0

DROP TABLE #tempPaymentsByReserveType

GO
