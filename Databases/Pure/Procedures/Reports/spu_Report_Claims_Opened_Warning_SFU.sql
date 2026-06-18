SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Report_Claims_Opened_Warning_SFU'
GO

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
** 1.0.3    25/07/2002  TB      Change Inception date from policy inception date to
**                              claim reported date (Support call F00047012 - RSAIB)
**
** 1.04	    29/06/2003	JMK	SRF 5086: client_short_name truncated
** 1.05	    01/09/2004	JT	Multi Currency Changes 
** 1.06     19/10/2005	Puneet  Payment Table into Claim_Payment and Claim_payment_item   
***********************************************************************************************************************************/

CREATE PROCEDURE spu_Report_Claims_Opened_Warning_SFU (
    @iElapsedDays int,
    @Start_Date datetime,
    @End_Date datetime,
    @DateRange varchar(20),
    @DateType varchar(20),
    @TypeOfCurrency	Varchar(30),
    @GroupByCode	Varchar(30),
    @TPACode Varchar(30) = NULL
    
)
AS
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

DECLARE @TPAId INT

SELECT @TPAId=party_cnt from party where shortname= @TPACode


create table #tempClaim
(
    claim_id int,
    Policy_id int,
    Claim_Status_ID int,
    claim_number varchar(30),
    policy_number varchar(30),
    client_name varchar(255),
    insurer_name varchar(60),
    loss_from_date datetime,
    reported_date datetime,
    client_short_name varchar(20),
    description varchar(255)
)
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
        ) AND (other_party_id= @TPAId or @TPAId IS NULL)
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
        ) AND (other_party_id= @TPAId or @TPAId IS NULL)
END



--
CREATE TABLE #tempClaimsOpenendWarning
(
    Policy_Number           varchar(30) null,
    Claim_Number            varchar(30) null,
    ClaimDescription        varchar(255) null,
    Insurer_Name            varchar(60) null,
    Client_Name             varchar(255) null,
    loss_from_date          datetime null,
    ClientShortName         varchar(20) null, 
    claimperildescription   varchar(255) null,
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
    Days                    int,
    CurrencyID				Int, /*Transaction currency Code*/
    SourceID				Int 	NULL
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
        Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(cp.Sum_insured,0)*ISNULL(Cl.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN (isnull(cp.Sum_insured,0)*ISNULL(Cl.currency_base_xrate,CR.rate_against_base)/ISNULL(Cl.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(cp.Sum_insured,0)
		END,
		Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Initial_reserve,0)*ISNULL(Cl.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN (isnull(r.Initial_reserve,0)*ISNULL(Cl.currency_base_xrate,CR.rate_against_base)/ISNULL(Cl.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(r.Initial_reserve,0)
		END,
        --r.Revised_Reserve,
		    		Case @TypeOfCurrency 
						WHEN 'Base' THEN (isnull(r.Revised_reserve,0)*ISNULL(cl.currency_base_xrate,CR.rate_against_base))
						WHEN 'System' THEN  (isnull(r.Revised_reserve,0)*ISNULL(cl.currency_base_xrate,CR.rate_against_base) / ISNULL(cl.system_base_xrate,CR.rate_against_base))
						WHEN 'Transaction' THEN isnull(r.Revised_reserve,0)
			END,
        Case @TypeOfCurrency 
				WHEN 'Base' THEN (isnull(r.Paid_to_date,0)*ISNULL(cl.currency_base_xrate,CR.rate_against_base))
				WHEN 'System' THEN (isnull(r.Paid_to_date,0)*ISNULL(cl.currency_base_xrate,CR.rate_against_base) / ISNULL(cl.system_base_xrate,CR.rate_against_base))
				WHEN 'Transaction' THEN isnull(r.Paid_to_date,0)
		END,
		(Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Initial_reserve,0)*ISNULL(Cl.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN (isnull(r.Initial_reserve,0)*ISNULL(Cl.currency_base_xrate,CR.rate_against_base)/ISNULL(Cl.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(r.Initial_reserve,0)
		END)
		+
		(Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Revised_reserve,0)*ISNULL(cl.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN  (isnull(r.Revised_reserve,0)*ISNULL(cl.currency_base_xrate,CR.rate_against_base) / ISNULL(cl.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(r.Revised_reserve,0)
		END
		) -
		(Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Paid_to_date,0)*ISNULL(cl.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN (isnull(r.Paid_to_date,0)*ISNULL(cl.currency_base_xrate,CR.rate_against_base) / ISNULL(cl.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(r.Paid_to_date,0)
		END),
        (Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Initial_reserve,0)*ISNULL(Cl.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN (isnull(r.Initial_reserve,0)*ISNULL(Cl.currency_base_xrate,CR.rate_against_base)/ISNULL(Cl.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(r.Initial_reserve,0)
		END)
					+
		(Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Revised_reserve,0)*ISNULL(cl.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN  (isnull(r.Revised_reserve,0)*ISNULL(cl.currency_base_xrate,CR.rate_against_base) / ISNULL(cl.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(r.Revised_reserve,0)
		END),
        p.claim_payment_id,
        p.date_of_payment,
        p.amount,
        rt.description,
        Prod.description,
        Prod.code,
        riskT.description,
        c.insurer_name,
        ifol.Inception_date,
        Datediff(dd, ifol.Inception_date, c.Loss_from_date),cl.currency_id,i.source_id

    FROM
        #TempClaim c join claim_peril cp on c.claim_id = cp.claim_id
        join reserve r on r.claim_Peril_Id = cp.claim_peril_id
        join claim_payment_item p1 on r.reserve_id = p1.reserve_id
        join claim_payment p on p1.claim_payment_id = p.claim_payment_id
        JOIN claim cl  ON cp.claim_id = cl.claim_id
        join reserve_type rt on r.reserve_type_Id = rt.reserve_type_id
        join Insurance_file I on c.Policy_id = I.insurance_file_cnt
        INNER JOIN currencyrate CR ON CR.currency_id = Cl.currency_id
        AND CR.company_id = ISNULL(@branch,I.source_id)
        left join Product Prod on i.product_id = Prod.product_id
        left join claim_risk crk on c.claim_id = crk. claim_id
        left join Risk_type riskT on crk.risk_type_id = riskT.risk_type_Id
        join Insurance_Folder IFol on I.insurance_folder_cnt = IFol.insurance_folder_cnt

    WHERE
        c.claim_status_id in (2, 4) and
        c.Loss_from_date <= DateAdd(dd, @iElapsedDays+1, ifol.Inception_date)
		AND CR.effective_from IN
		(
		SELECT MAX(effective_from)
		FROM CurrencyRate 
		WHERE effective_from <= Cl.reported_date
		AND   currency_id = CR.currency_id
		AND company_id = CR.company_id
		)
AND r.version_id=( ISNULL((SELECT MAX(version_id) FROM  claim WHERE claim_Number= c.claim_Number AND create_date >= @Start_Date AND create_date <= @End_Date ),1) )  
AND p.claim_payment_id=( ISNULL((SELECT MAX(ccp.claim_payment_id) FROM  Claim_Payment ccp join Claim cc on cc.Claim_id = ccp.claim_id WHERE cc.claim_Number= c.claim_Number AND create_date >= @Start_Date AND create_date <= @End_Date ),0) )  
--the above condition is added for PM022776 to show only last version which has a last payment made
--it was done becausse sum insured field was not pupulating correct on the report
SELECT *,S.Code CompanyCode,S.Description CompanyDesc,
Case @TypeOfCurrency 
	WHEN 'Base' THEN CB.Code
	WHEN 'System' THEN @SystemCurrencyCode 
	WHEN 'Transaction' THEN ct.Code
END CurrencyCode,
Case @TypeOfCurrency 
	WHEN 'Base' THEN CB.Description
	WHEN 'System' THEN @SystemCurrencydesc
	WHEN 'Transaction' THEN ct.description
END CurrencyDesc,
Case @GroupByCode 
	WHEN 'Branch' THEN S.Code
	WHEN 'Branch And Currency' THEN S.code
	WHEN 'Currency' THEN CT.Code
	ELSE ' '
END 'GroupByCode'

FROM #tempClaimsOpenendWarning TW
Inner Join Source S ON S.source_id = TW.Sourceid
Inner Join Currency CB ON CB.Currency_id = S.Base_currency_id
JOIN Currency ct /*Transaction Currency*/
ON ct.currency_id = tw.currencyid	

DROP TABLE #tempClaimsOpenendWarning
DROP TABLE #tempClaim
GO
