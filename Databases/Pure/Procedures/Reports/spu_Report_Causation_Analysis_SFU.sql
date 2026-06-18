EXECUTE DDLDropProcedure 'spu_Report_Causation_analysis_SFU'
GO
	
CREATE PROCEDURE spu_Report_Causation_Analysis_SFU(
	@Primary_cause varchar(60),
	@Start_Date datetime,
	@End_Date datetime,
	@DateRange varchar(20),
	@DateType varchar(20),
	@TypeOfCurrency Varchar(30),
	@GroupByCode	Varchar(30)	,
	@TPACode 	Varchar(30) = NULL
	)
AS

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
	Client_Name varchar(255) null,
	Insurer_Name varchar(60) null,
	loss_from_date datetime null,
	Reported_date datetime null,
	Client_Short_name varchar(60), 
	description varchar(255)
)

DECLARE @TPAId INT

SELECT @TPAId=party_cnt from party where shortname= @TPACode

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
    )  AND (other_party_id= @TPAId or @TPAId IS NULL)
	 AND is_dirty <> 1
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
    )    AND (other_party_id= @TPAId or @TPAId IS NULL)
	 AND is_dirty <> 1
end
--
CREATE TABLE #tempCausationAnalysis
(
	Sum_Insured		money null,
	Initial_Reserve		money null,
	Revised_Reserve		money null,
	Incurred_Amount		money null,
	Current_Reserve		money null,
	Claim_Number		varchar(60) null,
	Policy_Number		varchar(60) null,
	Client_Name		varchar(255) null,
	Insurer_Name		varchar(60) null,		
	loss_from_date		datetime null,
	Reported_Date		datetime null,
	Client_Short_name	varchar(60) null,
	ClaimDescription	varchar(255) null,
	ProductDescription	varchar(60) null,
	RiskDescription		varchar(60) null,
	PrimaryCause		varchar(60) null,
	SecondayCause		varchar(60) null,
	Paid_to_date		money null,
	PrimaryCauseID		varchar(4),
	SecondaryCauseID	varchar(4),
	CurrencyID			int NULL,
	SourceId	int NULL,
	IsSumInsured Int NULL
)
--
Declare @ClaimNum Varchar(50)
Declare @Branch Int
Declare @TypeOfRates Int
EXEC spu_ACT_GetTypeOfRates @TypeOfRates OUTPUT 
IF @TypeOfRates =1 
	SELECT @branch=1 
ELSE 
	BEGIN
		SELECT @branch=NULL 
	END

INSERT #tempCausationAnalysis

SELECT	
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
		Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Revised_reserve,0)*ISNULL(cl.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN  (isnull(r.Revised_reserve,0)*ISNULL(cl.currency_base_xrate,CR.rate_against_base) / ISNULL(cl.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(r.Revised_reserve,0)
		END,
		(Case @TypeOfCurrency 
					WHEN 'Base' THEN (isnull(r.Initial_reserve,0)*ISNULL(Cl.currency_base_xrate,CR.rate_against_base))
					WHEN 'System' THEN (isnull(r.Initial_reserve,0)*ISNULL(Cl.currency_base_xrate,CR.rate_against_base)/ISNULL(Cl.system_base_xrate,CR.rate_against_base))
					WHEN 'Trasaction' THEN isnull(r.Initial_reserve,0)
		END) 
		+
		(Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Revised_reserve,0)*ISNULL(cl.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN  (isnull(r.Revised_reserve,0)*ISNULL(cl.currency_base_xrate,CR.rate_against_base) / ISNULL(cl.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(r.Revised_reserve,0)
		END),
		
		(Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Initial_reserve,0)*ISNULL(Cl.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN (isnull(r.Initial_reserve,0)*ISNULL(Cl.currency_base_xrate,CR.rate_against_base)/ISNULL(Cl.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(r.Initial_reserve,0)
		END) +
		(Case @TypeOfCurrency 
					WHEN 'Base' THEN (isnull(r.Revised_reserve,0)*ISNULL(cl.currency_base_xrate,CR.rate_against_base))
					WHEN 'System' THEN  (isnull(r.Revised_reserve,0)*ISNULL(cl.currency_base_xrate,CR.rate_against_base) / ISNULL(cl.system_base_xrate,CR.rate_against_base))
					WHEN 'Transaction' THEN isnull(r.Revised_reserve,0)
					
		END)-
		(Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Paid_to_date,0)*ISNULL(cl.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN (isnull(r.Paid_to_date,0)*ISNULL(cl.currency_base_xrate,CR.rate_against_base) / ISNULL(cl.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(r.Paid_to_date,0)
		END),		
	/*r.Sum_insured,
	r.Initial_Reserve,
	r.Revised_Reserve,
	r.Initial_Reserve + r.Revised_reserve,
	r.Initial_Reserve + r.Revised_Reserve - r.Paid_to_date,*/
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
	c.Secondary_Cause_id,cl.currency_id,i.source_id, 0

FROM
	Reserve r 
	join claim_peril cp on r.claim_peril_id = cp.claim_peril_id
	join #TempClaims c on cp.claim_id = c.claim_id
	join claim Cl ON cl.claim_id = cp.Claim_id
	 JOIN (SELECT base_claim_id,
                              MAX(version_id) version_id
                       FROM   claim
                       WHERE  is_dirty <> 1
                       GROUP  BY base_claim_id) max_version
                   ON max_version.base_claim_id = Cl.base_claim_id
                      AND max_version.version_id = Cl.version_id                      
	join Insurance_file I on c.Policy_id = I.insurance_file_cnt
	INNER JOIN currencyrate CR ON CR.currency_id = Cl.currency_id
	AND CR.company_id = ISNULL(@branch,I.source_id)
	left join Product p on i.product_id = P.product_id
	left join claim_risk crk on c.claim_id = crk. claim_id
	left join Risk_type rt on crk.risk_type_id = rt.risk_type_Id
	left join Primary_Cause pc on c.Primary_cause_id = pc.primary_cause_id
	left join Secondary_cause sc on c.Secondary_Cause_id = sc.Secondary_cause_id
	
where 	c.Claim_Status_id in (2,3,4)
AND Cl.is_dirty <> 1
AND CR.effective_from IN
		(
			SELECT MAX(effective_from)
				FROM CurrencyRate 
				WHERE effective_from <= Cl.reported_date
				AND   currency_id = CR.currency_id
				AND company_id = CR.company_id
		)
AND r.version_id=( ISNULL((SELECT MAX(version_id) FROM  claim WHERE claim_Number= c.claim_Number AND create_date >= @Start_Date AND create_date <= @End_Date AND ISNUll(is_dirty,0) <> 1 ),1) )
AND ISNULL(Cl.is_dirty,0) <> 1
--This cursor is specially used for sum insured field in the SP.The sum insured was repeating for all the claim perils.
--This cursor updates only top 1 record and rest are all zeros. This is done to show correct sum insured field on the report   PM022776
    Declare tmp_cursor Cursor Fast_Forward For  
      SELECT DISTINCT claim_number FROM #tempCausationAnalysis 
    Open tmp_cursor  
    Fetch Next From tmp_cursor Into @ClaimNum  
    While (@@Fetch_Status = 0)  
    Begin 
		UPDATE TOP (1) tmp 
		SET IsSumInsured = 1
		FROM #tempCausationAnalysis tmp WHERE tmp.claim_number = @ClaimNum
		
		UPDATE tmp 
		SET Sum_Insured = 0
		FROM #tempCausationAnalysis tmp WHERE tmp.claim_number = @ClaimNum AND IsSumInsured <> 1
	   Fetch Next From tmp_cursor Into @ClaimNum  
    End  
    Close tmp_cursor  
    Deallocate tmp_cursor  
    	
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

if @Primary_cause = 'all'
select *,
		S.Code CompanyCode,
		S.description CompanyDesc,
		Case @TypeOfCurrency 
			WHEN 'System' THEN  @Systemcurrencycode
			WHEN 'Base' THEN C.Code
			WHEN 'Transaction' THEN ct.Code
		END CurrencyCode,
		Case @TypeOfCurrency 
			WHEN 'System' THEN @SystemCurrencyDesc
			WHEN  'Base' THEN C.description
			WHEN 'Transaction' THEN ct.description
		END CurrencyDesc,
		Case @GroupbyCode 
			WHEN 'Branch' THEN S.Code
			WHEN 'Branch And Currency' THEN S.Code
			WHEN 'Currency' THEN CT.Code
		ELSE ''
		END 'GroupByCode'	
	from #tempCausationAnalysis TA
	INNER JOIN Source S ON S.source_id = TA.sourceid
	INNER JOIN CURRENCY C ON C.Currency_id= S.base_currency_id
	JOIN Currency ct /*Transaction Currency*/
	ON ct.currency_id = ta.currencyid	


else
select *,
		S.Code CompanyCode,
		S.description CompanyDesc,
		Case @TypeOfCurrency 
			WHEN 'System' THEN  @Systemcurrencycode
			WHEN 'Base' THEN C.Code
			WHEN 'Transaction' THEN ct.Code
		END CurrencyCode,
		Case @TypeOfCurrency 
			WHEN 'System' THEN @SystemCurrencyDesc
			WHEN  'Base' THEN C.description
			WHEN 'Transaction' THEN ct.description
		END CurrencyDesc,
		Case @GroupbyCode 
			WHEN 'Branch' THEN S.Code
			WHEN 'Branch And Currency' THEN S.Code
			WHEN 'Currency' THEN CT.code
		ELSE ''
		END 'GroupByCode'	
	from #tempCausationAnalysis TA
	INNER JOIN Source S ON S.source_id = TA.sourceid
	INNER JOIN CURRENCY C ON C.Currency_id= S.base_currency_id
	JOIN Currency ct /*Transaction Currency*/
	ON ct.currency_id = ta.currencyid	
where Primarycause = @Primary_cause
--
DROP TABLE #tempCausationAnalysis
DROP TABLE #TempClaims
GO
