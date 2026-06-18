
EXECUTE DDLDropProcedure 'spu_Report_Reserve_Variance_SFU'
GO

/*****************************************
**  Reserve Variance report
**
**  Created: P. Haynes 4/9/2001
**
**  Reserve_Variance.rpt
**
***********************************************************************************************************************************
** VER      DATE        WHO     WHAT
** 1.01     29/01/2002  JMK     Add date parameters
** 1.02		03/09/2004	JT		MultiCurrency Changes
** 1.03     15Jun2006   RC      Filter by Agent Group
***********************************************************************************************************************************/

CREATE PROCEDURE spu_Report_Reserve_Variance_SFU
    (@sAgent varchar(60),
    @Start_Date datetime,
    @End_Date datetime,
    @DateRange varchar(20),
    @DateType varchar(20),
    @TypeOfCurrency Varchar(30),
	@GroupByCode	Varchar(30),
	@AgentGroupCode Varchar(30)
    )
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
DECLARE @sAgent varchar(60),
        @Start_Date datetime,
        @End_Date datetime,
        @DateRange  varchar(20),
        @DateType varchar(20)
SELECT @sAgent = 'ALL',
        @start_date = dateadd(day,-55,getdate()),
        @end_date = getdate(),
        @DateRange = 'This Month',
        @DateType = 'Loss Date'
*/

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
	
CREATE TABLE #tempReserveVariance
(
    Initial_Reserve     money null,
    Revised_Reserve     money null,
    Sum_Insured     money null,
    Claim_Number        varchar(60) null,
    Policy_Number       varchar(60) null,
    Client_Name     varchar(60) null,
    Insurer_Name        varchar(60) null,
    loss_from_date      datetime null,
    Client_Short_name   varchar(60) null,
    description     varchar(255) null,
    ProductDescription  varchar(60) null,
    RiskDescription     varchar(60) null,
    CurrencyId		Int,
    SourceId		INT	NULL
)
INSERT #tempReserveVariance

SELECT  
	Case @TypeOfCurrency 
		WHEN 'Base' THEN (isnull(r.initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
		WHEN 'System' THEN (isnull(r.initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
		WHEN 'Transaction' THEN isnull(r.initial_reserve,0)
	END initial_reserve,
    Case @TypeOfCurrency 
		WHEN 'Base' THEN ((isnull(r.revised_reserve,0) + isnull(r.initial_reserve,0)) *ISNULL(C.currency_base_xrate,CR.rate_against_base))        
		WHEN 'System' THEN ((isnull(r.revised_reserve,0) + isnull(r.initial_reserve,0)) *ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))           
		WHEN 'Transaction' THEN isnull(r.revised_reserve,0) + isnull(r.initial_reserve,0) 
	END revised_reserve,
    Case @TypeOfCurrency 
		WHEN 'Base' THEN (isnull(r.Sum_insured,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
		WHEN 'System' THEN (isnull(r.Sum_insured,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
		WHEN 'Transaction' THEN isnull(r.Sum_insured,0)
	END Sum_insured,
    c.claim_Number,
    c.Policy_number,
    c.Client_Name,
    c.Insurer_Name,
    c.loss_from_date,
    c.Client_Short_name,
    LEFT(c.description,255),
    p.description,
    rt.description,C.currency_id,I.Source_id

FROM
    Reserve r
    join claim_peril cp on r.claim_peril_id = cp.claim_peril_id
    join claim c on cp.claim_id = c.claim_id
    JOIN #tempClaims tc ON tc.ClaimID = c.claim_id
    join Insurance_file I on c.Policy_id = I.insurance_file_cnt
    INNER JOIN currencyrate CR ON CR.currency_id = C.currency_id
	AND CR.company_id = ISNULL(@branch,I.source_id)
    left join Product p on i.product_id = P.product_id
    left join claim_risk crk on c.claim_id = crk. claim_id
    left join Risk_type rt on crk.risk_type_id = rt.risk_type_Id

where   c.Claim_Status_id in (2, 4)
AND CR.effective_from IN
		(
		SELECT MAX(effective_from)
		FROM CurrencyRate 
		WHERE effective_from <= C.reported_date
		AND   currency_id = CR.currency_id
		AND company_id = CR.company_id
		)

DROP TABLE #tempClaims

IF LOWER(@AgentGroupCode) = 'all'
BEGIN
 PRINT 'ENTER1.1'

	IF @sAgent <> 'ALL'
	    SELECT *,
			S.Code CompanyCode,
			S.description CompanyDesc,
			Case @TypeOfCurrency 
				WHEN 'System' THEN  @Systemcurrencycode
				WHEN 'Base' THEN CB.Code
				WHEN 'Transaction' THEN CT.Code
			END CurrencyCode,
			Case @TypeOfCurrency 
				WHEN 'System' THEN @SystemCurrencyDesc
				WHEN  'Base' THEN CB.description
				WHEN 'Transaction' THEN CT.description
			END CurrencyDesc,
			Case @GroupbyCode 
				WHEN 'Branch' THEN S.Code
				WHEN 'Branch And Currency' THEN S.Code
				WHEN 'Currency' THEN CT.Code
			ELSE ''
			END 'GroupByCode'
	    FROM #tempReserveVariance TV
	    Join Source S 			ON S.Source_id = TV.sourceid
		Join Currency CB		ON CB.currency_id = S.Base_currency_id
		JOIN Currency Ct 		ON CT.currency_id = Tv.CurrencyId
	    WHERE Insurer_Name Like @sAgent
	
	else
	
	    SELECT * ,
			S.Code CompanyCode,
			S.description CompanyDesc,
			Case @TypeOfCurrency 
				WHEN 'System' THEN  @Systemcurrencycode
				WHEN 'Base' THEN CB.Code
				WHEN 'Transaction' THEN CT.Code
			END CurrencyCode,
			Case @TypeOfCurrency 
				WHEN 'System' THEN @SystemCurrencyDesc
				WHEN  'Base' THEN CB.description
				WHEN 'Transaction' THEN CT.description
			END CurrencyDesc,
			Case @GroupbyCode 
				WHEN 'Branch' THEN S.Code
				WHEN 'Branch And Currency' THEN S.Code
				WHEN 'Currency' THEN CT.Code
			ELSE ''
			END 'GroupByCode'
		
		    FROM #tempReserveVariance TV
			Join Source S 			ON S.Source_id = TV.sourceid
			Join Currency CB		ON CB.currency_id = S.Base_currency_id
			JOIN Currency Ct 		ON CT.currency_id = Tv.CurrencyId
END

IF LOWER(@AgentGroupCode) <> 'all'
BEGIN
 PRINT 'ENTER2.1'

	IF @sAgent <> 'ALL'
	    SELECT *,
			S.Code CompanyCode,
			S.description CompanyDesc,
			Case @TypeOfCurrency 
				WHEN 'System' THEN  @Systemcurrencycode
				WHEN 'Base' THEN CB.Code
				WHEN 'Transaction' THEN CT.Code
			END CurrencyCode,
			Case @TypeOfCurrency 
				WHEN 'System' THEN @SystemCurrencyDesc
				WHEN  'Base' THEN CB.description
				WHEN 'Transaction' THEN CT.description
			END CurrencyDesc,
			Case @GroupbyCode 
				WHEN 'Branch' THEN S.Code
				WHEN 'Branch And Currency' THEN S.Code
				WHEN 'Currency' THEN CT.Code
			ELSE ''
			END 'GroupByCode'
	    FROM #tempReserveVariance TV
	    Join Source S 			ON S.Source_id = TV.sourceid
		Join Currency CB		ON CB.currency_id = S.Base_currency_id
		JOIN Currency Ct 		ON CT.currency_id = Tv.CurrencyId
	    WHERE Insurer_Name Like @sAgent
		AND Insurer_Name IN(
		select trading_name from party_agent where linked_account_group = (
		select  party_cnt from party where shortname = @AgentGroupCode) )
	
	else
	
	    SELECT * ,
			S.Code CompanyCode,
			S.description CompanyDesc,
			Case @TypeOfCurrency 
				WHEN 'System' THEN  @Systemcurrencycode
				WHEN 'Base' THEN CB.Code
				WHEN 'Transaction' THEN CT.Code
			END CurrencyCode,
			Case @TypeOfCurrency 
				WHEN 'System' THEN @SystemCurrencyDesc
				WHEN  'Base' THEN CB.description
				WHEN 'Transaction' THEN CT.description
			END CurrencyDesc,
			Case @GroupbyCode 
				WHEN 'Branch' THEN S.Code
				WHEN 'Branch And Currency' THEN S.Code
				WHEN 'Currency' THEN CT.Code
			ELSE ''
			END 'GroupByCode'
		
		    FROM #tempReserveVariance TV
			Join Source S 			ON S.Source_id = TV.sourceid
			Join Currency CB		ON CB.currency_id = S.Base_currency_id
			JOIN Currency Ct 		ON CT.currency_id = Tv.CurrencyId
			WHERE Insurer_Name IN(
			select trading_name from party_agent where linked_account_group = (
			select  party_cnt from party where shortname = @AgentGroupCode) )
END

DROP TABLE #tempReserveVariance

GO
