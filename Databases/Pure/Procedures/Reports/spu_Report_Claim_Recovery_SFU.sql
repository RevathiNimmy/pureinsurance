
EXECUTE DDLDropProcedure 'spu_Report_Claim_Recovery_SFU'
GO

/*****************************************
**	Claims Recovery Report.
**
**	Created: P. Haynes 3/9/2001
**
**	Claim_Recovery.rpt

** Modified: 1.1	J TIwari 26-08-2004
					Multicurency changes 
*****************************************/
	
CREATE PROCEDURE spu_Report_Claim_Recovery_SFU(
	@Start_Date datetime,
	@End_Date datetime,
	@DateRange varchar(20),
	@DateType varchar(20),
	@TypeOfCurrency Varchar(30),
	@GroupByCode	Varchar(30)	
)
AS

--	c.claim_Number,
--	c.Policy_number, 
--	c.Client_Name,
--	c.Insurer_Name,
--	c.loss_from_date,
--	c.Reported_Date,
--	c.Client_Short_name, 
--	c.description ClaimDescription,

create table #tempClaims
(
	Claim_ID int null,
	Policy_ID int null,
	Claim_status_id int null,
	claim_Number varchar(60) null, 
	Policy_number varchar(60) null, 
	Client_Name varchar(255) null,
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
Declare @Branch Int
Declare @TypeOfRates Int
EXEC spu_ACT_GetTypeOfRates @TypeOfRates OUTPUT 
IF @TypeOfRates =1 
	SELECT @branch=1 
ELSE 
	BEGIN
		SELECT @branch=NULL 
	END
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

SELECT 	
	/*r.Initial_Reserve,
	r.Revised_Reserve,
	r.Received_to_date,*/
	Case @TypeOfCurrency 
		WHEN 'Base' THEN (isnull(r.Initial_reserve,0)*ISNULL(Cl.currency_base_xrate,CR.rate_against_base))
		WHEN 'System' THEN (isnull(r.Initial_reserve,0)*ISNULL(Cl.currency_base_xrate,CR.rate_against_base)/ISNULL(Cl.system_base_xrate,CR.rate_against_base))
		WHEN 'Transaction' THEN isnull(r.Initial_reserve,0)
	END Initial_Reserve,
	Case @TypeOfCurrency 
		WHEN 'Base' THEN (isnull(r.Revised_reserve,0)*ISNULL(cl.currency_base_xrate,CR.rate_against_base))
		WHEN 'System' THEN  (isnull(r.Revised_reserve,0)*ISNULL(cl.currency_base_xrate,CR.rate_against_base) / ISNULL(cl.system_base_xrate,CR.rate_against_base))
		WHEN 'Transaction' THEN isnull(r.Revised_reserve,0)
	END Revised_Reserve,
	Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Received_to_date,0)*ISNULL(cl.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN  (isnull(r.Received_to_date,0)*ISNULL(cl.currency_base_xrate,CR.rate_against_base) / ISNULL(cl.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(r.Received_to_date,0)
	END Received_to_date,
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
	--(r.Initial_reserve + r.revised_reserve) as IncurredAmount,
	--((r.Initial_reserve + r.revised_reserve) - r.Received_to_date) as CurrentReserve
	(Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Initial_reserve,0)*ISNULL(Cl.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN (isnull(r.Initial_reserve,0)*ISNULL(Cl.currency_base_xrate,CR.rate_against_base)/ISNULL(Cl.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(r.Initial_reserve,0)
		END
		+
		Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Revised_reserve,0)*ISNULL(cl.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN  (isnull(r.Revised_reserve,0)*ISNULL(cl.currency_base_xrate,CR.rate_against_base) / ISNULL(cl.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(r.Revised_reserve,0)
	END ) IncurredAmount,
	((Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Initial_reserve,0)*ISNULL(Cl.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN (isnull(r.Initial_reserve,0)*ISNULL(Cl.currency_base_xrate,CR.rate_against_base)/ISNULL(Cl.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(r.Initial_reserve,0)
			
		END)
		+
		(Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Revised_reserve,0)*ISNULL(cl.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN  (isnull(r.Revised_reserve,0)*ISNULL(cl.currency_base_xrate,CR.rate_against_base) / ISNULL(cl.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(r.Revised_reserve,0)			
	END )
	- 
	(Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Received_to_date,0)*ISNULL(cl.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN  (isnull(r.Received_to_date,0)*ISNULL(cl.currency_base_xrate,CR.rate_against_base) / ISNULL(cl.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(r.Received_to_date,0)
			
	END))	CurrentReserve,
			S.Code CompanyCode,
			S.description CompanyDesc,
			Case @TypeOfCurrency 
				WHEN 'System' THEN  @Systemcurrencycode
				WHEN 'Base' THEN Currency.Code
				WHEN 'Transaction' THEN CT.COde
			END CurrencyCode,
			Case @TypeOfCurrency 
				WHEN 'System' THEN @SystemCurrencyDesc
				WHEN  'Base' THEN Currency.description
				WHEN 'Transaction' THEN CT.description
			END CurrencyDesc,
			Case @GroupbyCode 
				WHEN 'Branch' THEN S.Code
				WHEN 'Branch And Currency' THEN S.Code
				WHEN 'Currency' THEN CT.Code
			ELSE ''
		END 'GroupByCode'
	
FROM
	[Recovery] r 
	join claim_peril cp on r.claim_peril_id = cp.claim_peril_id
	left join Recovery_type RecType on r.Recovery_type_id = RecType.Recovery_Type_ID
	join #TempClaims c on cp.claim_id = c.claim_id
	join claim Cl ON cl.claim_id = cp.Claim_id
	join (select base_claim_id, max(version_id) version_id from claim group by base_claim_id) max_version
	ON max_version.base_claim_id = CL.base_claim_id and max_version.version_id = CL.version_id	
	join Insurance_file I on c.Policy_id = I.insurance_file_cnt
	INNER JOIN currencyrate CR ON CR.currency_id = Cl.currency_id
	AND CR.company_id = ISNULL(@branch,I.source_id)
	left join Product p on i.product_id = P.product_id
	left join claim_risk crk on c.claim_id = crk. claim_id
	left join Risk_type rt on crk.risk_type_id = rt.risk_type_Id
	JOIN SOURCE S ON S.source_id = i.source_id 
	JOIN Currency ON Currency.Currency_id = S.base_currency_id
	JOIN Currency ct /*Transaction Currency*/
	ON ct.currency_id = Cl.currency_id	

where 	c.Claim_Status_id in (2, 4) and
	(r.initial_reserve is not null or
	r.revised_reserve is not null or
	r.Received_to_date is not null)
	AND CR.effective_from IN
		(
		SELECT MAX(effective_from)
			FROM CurrencyRate 
			WHERE effective_from <= Cl.reported_date
			AND   currency_id = CR.currency_id
			AND company_id = CR.company_id
		)
	
drop table #tempClaims
