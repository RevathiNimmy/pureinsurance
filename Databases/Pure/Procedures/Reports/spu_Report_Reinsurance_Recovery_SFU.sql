
EXECUTE DDLDropProcedure 'spu_Report_Reinsurance_Recovery_SFU'
GO

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
** 1.03		03/9/2004	JT		Multicurrency changes
***********************************************************************************************************************************/

CREATE PROCEDURE spu_Report_Reinsurance_Recovery_SFU
    @Start_Date datetime,
    @End_Date datetime,
    @DateRange varchar(20),
    @DateType varchar(20),
    @TypeOfCurrency Varchar(30),
	@GroupByCode	Varchar(30)

AS 
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
/*Get System Currency Details--jitendra*/
	declare @SystemCurrencyCode varchar(10)
	declare @SystemCurrencyDesc varchar(255)
    SELECT
    	@SystemCurrencyCode = c.iso_code,
    	@SystemCurrencyDesc = c.description
    FROM PMSystem pms
    JOIN currency c
    	ON c.currency_id = pms.currency_id
    WHERE pms.system_id = 1
/*end  Get System Currency*/
Declare @Branch Int
Declare @TypeOfRates Int
EXEC spu_ACT_GetTypeOfRates @TypeOfRates OUTPUT 
IF @TypeOfRates =1 
	SELECT @branch=1 
ELSE 
	BEGIN
		SELECT @branch=NULL 
	END


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
    (
    (( 
    Case @TypeOfCurrency 
		WHEN 'Base' THEN (isnull(r.initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
		WHEN 'System' THEN (isnull(r.initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
	END
	+ 
	Case @TypeOfCurrency 
		WHEN 'Base' THEN (isnull(r.revised_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
		WHEN 'System' THEN (isnull(r.revised_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
	END
	) - 
	Case @TypeOfCurrency 
		WHEN 'Base' THEN (isnull(r.paid_to_date,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
		WHEN 'System' THEN (isnull(r.paid_to_date,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
	END	)
	) / 100 * cp.share as Share_Value,
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
    Case @TypeOfCurrency 
		WHEN 'Base' THEN (isnull(r.initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
		WHEN 'System' THEN (isnull(r.initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
	END initial_reserve,
    Case @TypeOfCurrency 
		WHEN 'Base' THEN (isnull(r.revised_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
		WHEN 'System' THEN (isnull(r.revised_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
	END revised_reserve,
    Case @TypeOfCurrency 
		WHEN 'Base' THEN (isnull(r.paid_to_date,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
		WHEN 'System' THEN (isnull(r.paid_to_date,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
	END Paid_to_date ,
    (Case @TypeOfCurrency 
		WHEN 'Base' THEN (isnull(r.initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
		WHEN 'System' THEN (isnull(r.initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
	END
	+
	Case @TypeOfCurrency 
		WHEN 'Base' THEN (isnull(r.revised_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
		WHEN 'System' THEN (isnull(r.revised_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
	END) -
	Case @TypeOfCurrency 
		WHEN 'Base' THEN (isnull(r.paid_to_date,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
		WHEN 'System' THEN (isnull(r.paid_to_date,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
	END as Incurred_Amount,
	S.Code CompanyCode,
			S.description CompanyDesc,
			Case @TypeOfCurrency 
				WHEN 'System' THEN  @Systemcurrencycode
				WHEN 'Base' THEN CB.Code
			END CurrencyCode,
			Case @TypeOfCurrency 
				WHEN 'System' THEN @SystemCurrencyDesc
				WHEN  'Base' THEN CB.description
			END CurrencyDesc,
			Case @GroupbyCode 
				WHEN 'Branch' THEN S.Code
				WHEN 'Branch And Currency' THEN S.Code
			ELSE ''
		END 'GroupByCode'

FROM #tempClaims tc
    JOIN claim c            ON tc.ClaimID = c.claim_id
    left join claim_party cp     ON cp.claim_id = c.claim_id
    join Insurance_file I   on c.Policy_id = I.insurance_file_cnt
	INNER JOIN currencyrate CR ON CR.currency_id = C.currency_id
	AND CR.company_id = ISNULL(@branch,I.source_id)
	left join Product p     on i.product_id = P.product_id
	left join claim_risk crk on c.claim_id = crk. claim_id
	left join Risk_type rt  on crk.risk_type_id = rt.risk_type_Id
	join claim_peril cPer   on cPer.claim_id = c.claim_id
	join reserve r          on r.Claim_peril_id = cper.claim_peril_id
	left join party              on cp.party_id = party.party_cnt
	Join Source S 			ON S.Source_id = I.source_id
	Join Currency CB		ON CB.currency_id = S.Base_currency_id
	where   c.Claim_Status_id in (2, 4)
		and (cp.Insurer_type = 1 OR cp.Claim_id IS NULL)
		AND CR.effective_from IN
		(
		SELECT MAX(effective_from)
		FROM CurrencyRate 
		WHERE effective_from <= C.reported_date
		AND   currency_id = CR.currency_id
		AND company_id = CR.company_id
		)

    and r.revision_count = (Select max(revision_Count)
	
    FROM    reserve
    where   reserve.claim_peril_id = cper.claim_peril_id)
    and (r.initial_reserve + r.revised_reserve) - r.paid_to_date > 0
DROP TABLE #tempClaims
GO