EXECUTE DDLDropProcedure 'spu_Report_Incurred_Claims_ALL_SFU'
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE spu_Report_Incurred_Claims_ALL_SFU
(
    @IncludeRecoveries varchar(3),
    @IncludeClosed varchar (5)
--    @Start_Date datetime,
--    @End_Date datetime,
--    @DateRange varchar(20)
)
AS
SET NOCOUNT ON
CREATE TABLE #TempClaim
(
	CurrencySymbol		char(4) null,
    Initial_Reserve     money null,
    Revised_Reserve     money null,
    Sum_insured         money null,
    Paid_to_Date        money null,
    claim_Number        varchar(60),
    Policy_number       varchar(60),
    Client_Name         varchar(255),
    Insurer_Name        varchar(255),
    loss_from_date      datetime null,
    Client_Short_name   varchar(60),
    ClaimDescription    varchar(1000),
    ProductDescription  varchar(255),
    RiskTypeDescription varchar(255),
    IncurredAmount      money null,
    CurrentReserve      money null,
    IsRecovery          varchar(3),
	Claim_id			integer
)
IF @IncludeRecoveries <> 'Yes'
BEGIN
    INSERT INTO #tempClaim
    SELECT ISNULL(cy.symbol,''),
		r.Initial_Reserve,
        r.Revised_Reserve,
        cp.Sum_insured,
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
        'no',
		c.Claim_id Claim_id
    FROM
        Reserve r
        join claim_peril cp on r.claim_peril_id = cp.claim_peril_id
        join claim c on cp.claim_id = c.claim_id
        join Insurance_file I on c.Policy_id = I.insurance_file_cnt
        left join Product p on i.product_id = P.product_id
        left join claim_risk cr on c.claim_id = cr. claim_id
        left join Risk_type rt on cr.risk_type_id = rt.risk_type_Id
		join Currency cy on c.Currency_id= cy.currency_id
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
    AND r.version_id=(SELECT MAX(version_id) FROM  claim WHERE claim_Number= c.claim_Number AND iSNULL(IS_dirty,0)=0)
	AND iSNULL(IS_dirty,0)=0

END
ELSE
BEGIN
    INSERT INTO #tempClaim
    SELECT ISNULL(cy.symbol,''),
		r.Initial_Reserve,
        r.Revised_Reserve,
        cp.Sum_insured,
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
        (r.Initial_reserve + r.revised_reserve)IncurredAmount,
        ((r.Initial_reserve + r.revised_reserve) - r.Paid_to_Date)CurrentReserve,
        'no',
		c.Claim_id Claim_id
    FROM
        Reserve r
        join claim_peril cp on r.claim_peril_id = cp.claim_peril_id
        join claim c on cp.claim_id = c.claim_id
        join Insurance_file I on c.Policy_id = I.insurance_file_cnt
        left join Product p on i.product_id = P.product_id
        left join claim_risk cr on c.claim_id = cr. claim_id
        left join Risk_type rt on cr.risk_type_id = rt.risk_type_Id
		join Currency cy on c.Currency_id= cy.currency_id
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
    AND r.version_id=(SELECT MAX(version_id) FROM  claim WHERE claim_Number= c.claim_Number AND iSNULL(IS_dirty,0)=0)
	AND iSNULL(IS_dirty,0)=0

    INSERT INTO #tempClaim
    SELECT ISNULL(cy.symbol,''),
		r.Initial_Reserve,
        r.Revised_Reserve,
        cp.Sum_insured,
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
        (r.Received_to_date)IncurredAmount,
        ((r.Initial_reserve + r.revised_reserve))CurrentReserve,
        'yes',
		c.Claim_id Claim_id
    FROM
        [Recovery] r
        join claim_peril cp on r.claim_peril_id = cp.claim_peril_id
        join Claim c on cp.claim_id = c.claim_id
        join Insurance_file I on c.Policy_id = I.insurance_file_cnt
        left join Product p on i.product_id = P.product_id
        left join claim_risk cr on c.claim_id = cr. claim_id
        left join Risk_type rt on cr.risk_type_id = rt.risk_type_Id
		join Currency cy on c.Currency_id= cy.currency_id
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
    AND r.version_id=(SELECT MAX(version_id) FROM  claim WHERE claim_Number= c.claim_Number AND iSNULL(IS_dirty,0)=0)
	AND iSNULL(IS_dirty,0)=0

       
END
SET NOCOUNT OFF
SELECT	MAX(CurrencySymbol)CurrencySymbol,
	convert(float, SUM((CASE WHEN IsRecovery ='no' THEN 1 ELSE 0 END)*Initial_Reserve))Initial_Reserve,
	convert(float,SUM((CASE WHEN IsRecovery ='no' THEN 1 ELSE 0 END)*Revised_Reserve))Revised_Reserve,
	convert(float,(select MAX(Sum_insured) from Claim_Peril where Claim_Peril.Claim_id =#tempClaim.Claim_id ))Sum_insured,
	convert(float,SUM((CASE WHEN IsRecovery ='no' THEN 1 ELSE 0 END)*Paid_to_Date))Paid_to_Date,
	claim_Number,
	MAX(Policy_number)Policy_number,
	MAX(Client_Name)Client_Name,
	MAX(Insurer_Name)Insurer_Name,
	Max(loss_from_date)loss_from_date,
	MAX(Client_Short_name)Client_Short_name,
	MAX(ClaimDescription)ClaimDescription,
	MAX(ProductDescription)ProductDescription,
	MAX(RiskTypeDescription)RiskTypeDescription,
	convert(float,SUM((CASE WHEN IsRecovery ='no' THEN 1 ELSE -1 END)*IncurredAmount))IncurredAmount,
	convert(float,SUM((CASE WHEN IsRecovery ='no' THEN 1 ELSE 0 END)*CurrentReserve))CurrentReserve,
	MAX(IsRecovery)IsRecovery,
	Claim_id  
FROM #tempClaim
group by claim_number,Claim_id

DROP TABLE #tempClaim
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

