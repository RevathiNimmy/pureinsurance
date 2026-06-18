EXECUTE DDLDropProcedure 'spu_Report_OutStanding_Claims_SFU'
GO

/**********************************************************************************************************************************
** Created by Jude Killip
** 22/08/2000
** RSA Reports - Outstanding_Claims.rpt
**  Created with dummy data to build the report
**********************************************************************************************************************************
** 09/11/2000   Thinh Nguyen    real data
**
** 30/04/2001   Jude Killip     include Recoveries, add in the parameter
**
** 04/05/2001   Jude Kilip      fix dodgy query (adding too many times)
**
** 03/07/2001   Jude Killip     c.description 255
**                              limit to non-closed or reclosed claims, and filter out claims with zero values
**
** 20/09/2001   Jude Killip     redo the Salvage & Recovery option
**                              retrieve Reserve and Recovery data in separate chunks
**
** 22/10/2001   Jude Killip     calculate home currency values
**
** 31/10/2001   Jude Killip     retrieve currency rates properly
**
** 09/06/03 Jon Kemp    Data type error fixing feilds : ClientCode & AgentCode to 20 varchar instead of 10 varchar
***********************************************************************************************************************************
** VER      DATE        WHO     WHAT
** 1.01     29/01/2002  JMK     Add date parameters
** 1.02           03/09/2004  JT          MultiCurrency changes
***********************************************************************************************************************************/
CREATE PROCEDURE spu_Report_OutStanding_Claims_SFU
    @SalvageAndTPRecovery varchar (10),
    @Start_Date datetime,
    @End_Date datetime,
    @DateRange varchar(20),
    @DateType varchar(20),
    @TypeOfCurrency Varchar(30),
 @GroupByCode Varchar(30) ,
	@TPACode Varchar(30) = NULL
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
SalvageAndTPRecovery:
    exclude
    include
    only
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
DECLARE @SalvageAndTPRecovery varchar (10),
        @Start_Date datetime,
        @End_Date datetime,
        @DateRange  varchar(20),
        @DateType varchar(20)
SELECT @SalvageAndTPRecovery = 'only',
        @start_date = dateadd(day,-55,getdate()),
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

    DECLARE @TPAId INT

SELECT @TPAId=party_cnt from party where shortname= @TPACode

/*end  Get System Currency*/
CREATE TABLE #tempClaims
(
    ClaimID int
)

IF @dateType = 'Loss Date'
BEGIN
    INSERT INTO #tempClaims
        SELECT Max(Claim_id)
        FROM Claim
        WHERE is_dirty=0 AND
        (
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
        )         AND (other_party_id= @TPAId or @TPAId IS NULL)
		GROUP BY Policy_id
END
ELSE
BEGIN
    INSERT INTO #tempClaims
        SELECT Claim_id
        FROM Claim
        WHERE is_dirty=0 AND
        (
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
        )         AND (other_party_id= @TPAId or @TPAId IS NULL)
END

CREATE TABLE #tempRSAOutstClaims
(
    TempID int IDENTITY,
    RiskTypeCode varchar (10) NULL,
    RiskTypeDesc varchar (255) NULL,
    ReserveType varchar (255) NULL,
    ClaimNumber varchar (30) NULL,
    AgentCode varchar (20) NULL,
    InsuranceRef varchar (30) NULL,
    ClientCode varchar (20) NULL,
    ClientName varchar (255) NULL,
    LossFromDate datetime NULL,
    ClaimDesc varchar (1000) NULL,
    CurrencyRate money NULL,
    CurrencyID int NULL,
    InitialReserve money NULL,
    RevisedReserve money NULL,
    Payments decimal(20,2) NULL,
    SourceId	Int Null
)
Declare @Branch Int
Declare @TypeOfRates Int
EXEC spu_ACT_GetTypeOfRates @TypeOfRates OUTPUT
IF @TypeOfRates =1
	SELECT @branch=1
ELSE
	BEGIN
		SELECT @branch=NULL
	END

IF @SalvageAndTPRecovery = 'exclude'
BEGIN
    -- print 'get outstanding claims with Reserves '
    INSERT INTO #tempRSAOutstClaims
        SELECT rt.code,
            rt.description,
            (select description from reserve_type where Reserve_type_id = res.Reserve_type_id),
            c.claim_number,
            c.insurer_short_name,
            c.policy_number,
            c.client_short_name,
            c.client_name,
            c.loss_from_date,
            LEFT(c.description,255),
            1,
            c.currency_id,
            --res.initial_reserve,
			Case @TypeOfCurrency
				WHEN 'Base' THEN (isnull(res.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
				WHEN 'System' THEN (isnull(res.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
				WHEN 'Transaction' THEN isnull(res.Initial_reserve,0)
			END,
--            res.revised_reserve,
			Case @TypeOfCurrency
    WHEN 'Base' THEN (isnull(res.revised_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))   -(isnull(cpp.unauthorise_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))
				WHEN 'System' THEN (isnull(res.revised_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
	- (isnull(cpp.unauthorise_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))
    WHEN 'Transaction' THEN isnull(res.revised_reserve,0)   -isnull(cpp.unauthorise_reserve,0)
			END,

            --res.paid_to_date
 			Case @TypeOfCurrency
    WHEN 'Base' THEN (isnull(res.paid_to_date,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))- ISNULL(cpp.this_payment_base,0)
    WHEN 'System' THEN (isnull(res.paid_to_date,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base)) - ISNULL(cpp.this_payment_system,0)
    WHEN 'Transaction' THEN isnull(res.paid_to_date,0) - ISNULL(cpp.this_payment_trans,0)
			END,I.source_id

        FROM #tempClaims tc
        JOIN claim c            ON tc.ClaimID = c.claim_id
        JOIN Claim_Peril cp     ON c.claim_id = cp.claim_id                 -- Claim_Peril link
        join (select base_claim_id, max(version_id) version_id from claim where is_dirty <> 1 group by base_claim_id) max_version
        ON max_version.base_claim_id = c.base_claim_id and max_version.version_id = c.version_id
        JOIN Reserve res        ON cp.claim_peril_id = res.claim_peril_id
        LEFT JOIN (
                  SELECT cpi.reserve_id,SUM(cpi.this_payment*currency_base_xrate) this_payment_base,
                              SUM(cpi.this_payment*currency_base_xrate/system_base_xrate) this_payment_system,
                              SUM(cpi.this_payment*payment_loss_xrate) this_payment_trans,
					SUM(r1.this_revision) unauthorise_reserve
                  FROM Claim_Payment cp2
                  INNER JOIN Claim_Payment_Item cpi ON cp2.claim_payment_id=cpi.claim_payment_id
                  INNER JOIN Claim_Payment cp1 ON cp1.claim_payment_id=cp2.base_claim_payment_id
				  INNER JOIN reserve r1 on r1.reserve_id= cpi.reserve_id
                  WHERE cp1.is_referred=1
                  AND (cp1.amount <> 0 and (r1.this_payment <> 0 or r1.this_revision <> 0))
                  GROUP BY cpi.reserve_id ) cpp ON res.Reserve_id=cpp.reserve_id
        JOIN Risk r             ON c.risk_type_id = r.risk_cnt
        JOIN Risk_Type rt       ON rt.risk_type_id = r.risk_type_id
        join Insurance_file I on c.Policy_id = I.insurance_file_cnt
        INNER JOIN currencyrate CR ON CR.currency_id = C.currency_id
		AND CR.company_id = ISNULL(@branch,I.source_id)

        WHERE c.claim_status_id not in (3,5)
		AND c.is_dirty=0
		AND CR.effective_from IN
		(
		SELECT MAX(effective_from)
		FROM CurrencyRate
		WHERE effective_from <= C.reported_date
		AND   currency_id = CR.currency_id
		AND company_id = CR.company_id
		)

END
ELSE
IF @SalvageAndTPRecovery = 'only'
BEGIN
    -- print 'get outstanding claims with Recoveries '
    INSERT INTO #tempRSAOutstClaims
        SELECT rt.code,
            rt.description,
            (select description from recovery_type where recovery_type_id = rec.recovery_type_id),
            c.claim_number,
            c.insurer_short_name,
            c.policy_number,
            c.client_short_name,
            c.client_name,
            c.loss_from_date,
            LEFT(c.description,255),
            1,
            c.currency_id,
		--res.initial_reserve,
		Case @TypeOfCurrency
			WHEN 'Base' THEN (isnull(rec.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN (isnull(rec.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(rec.Initial_reserve,0)
		END,
		--            res.revised_reserve,
		Case @TypeOfCurrency
			WHEN 'Base' THEN (isnull(rec.revised_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN (isnull(rec.revised_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(rec.revised_reserve,0)
		END,
		Case @TypeOfCurrency
					WHEN 'Base' THEN (isnull(rec.received_to_date,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
					WHEN 'System' THEN (isnull(rec.received_to_date,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
					WHEN 'Transaction' THEN isnull(rec.received_to_date,0)
		END,
		I.source_id

        FROM #tempClaims tc
        JOIN claim c            ON tc.ClaimID = c.claim_id
        JOIN Claim_Peril cp     ON c.claim_id = cp.claim_id                 -- Claim_Peril link
        join (select base_claim_id, max(version_id) version_id from claim where is_dirty <> 1 group by base_claim_id) max_version
        ON max_version.base_claim_id = c.base_claim_id and max_version.version_id = c.version_id
        JOIN [Recovery] rec     ON cp.claim_peril_id = rec.claim_peril_id
        JOIN Risk r             ON c.risk_type_id = r.risk_cnt
        JOIN Risk_Type rt       ON rt.risk_type_id = r.risk_type_id
        join Insurance_file I on c.Policy_id = I.insurance_file_cnt
		INNER JOIN currencyrate CR ON CR.currency_id = C.currency_id
		AND CR.company_id = ISNULL(@branch,I.source_id)

        WHERE c.claim_status_id not in (3,5)
		AND c.is_dirty=0
		AND CR.effective_from IN
		(
		SELECT MAX(effective_from)
		FROM CurrencyRate
		WHERE effective_from <= C.reported_date
		AND   currency_id = CR.currency_id
		AND company_id = CR.company_id
		)

END
ELSE
BEGIN

    -- print 'get outstanding claims, Reserves & Recoveries'
    -- print 'Reserves first...'
    INSERT INTO #tempRSAOutstClaims
        SELECT rt.code,
            rt.description,
            (select description from reserve_type where Reserve_type_id = res.Reserve_type_id),
            c.claim_number,
            c.insurer_short_name,
            c.policy_number,
            c.client_short_name,
            c.client_name,
            c.loss_from_date,
            LEFT(c.description,255),
            1,
            c.currency_id,
--res.initial_reserve,
		Case @TypeOfCurrency
			WHEN 'Base' THEN (isnull(res.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN (isnull(res.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(res.Initial_reserve,0)
		END,
--            res.revised_reserve,
		Case @TypeOfCurrency
			WHEN 'Base' THEN (isnull(res.revised_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN (isnull(res.revised_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(res.revised_reserve,0)
		END,

		--res.paid_to_date
		Case @TypeOfCurrency
   WHEN 'Base' THEN (isnull(res.paid_to_date,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)) - ISNULL(cpp.this_payment_base,0)
   WHEN 'System' THEN (isnull(res.paid_to_date,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base)) - ISNULL(cpp.this_payment_system,0)
   WHEN 'Transaction' THEN isnull(res.paid_to_date,0) - ISNULL(cpp.this_payment_trans,0)
		END,I.source_id

        FROM #tempClaims tc
        JOIN claim c            ON tc.ClaimID = c.claim_id
        JOIN Claim_Peril cp     ON c.claim_id = cp.claim_id                 -- Claim_Peril link
        join (select base_claim_id, max(version_id) version_id from claim where is_dirty<>1  group by base_claim_id) max_version
        ON max_version.base_claim_id = c.base_claim_id and max_version.version_id = c.version_id
        JOIN Reserve res        ON cp.claim_peril_id = res.claim_peril_id
          LEFT JOIN (
                  SELECT cpi.reserve_id,SUM(cpi.this_payment*currency_base_xrate) this_payment_base,
                              SUM(cpi.this_payment*currency_base_xrate/system_base_xrate) this_payment_system,
                              SUM(this_payment*payment_loss_xrate) this_payment_trans
                  FROM Claim_Payment cp2
                  INNER JOIN Claim_Payment_Item cpi ON cp2.claim_payment_id=cpi.claim_payment_id
                  INNER JOIN Claim_Payment cp1 ON cp1.claim_payment_id=cp2.base_claim_payment_id
                  WHERE cp1.is_referred=1
                  AND (cp1.amount <> 0)
                  GROUP BY cpi.reserve_id ) cpp ON res.Reserve_id=cpp.reserve_id
        JOIN Risk r             ON c.risk_type_id = r.risk_cnt
        JOIN Risk_Type rt       ON rt.risk_type_id = r.risk_type_id
        join Insurance_file I  ON c.Policy_id = I.insurance_file_cnt
		INNER JOIN currencyrate CR ON CR.currency_id = C.currency_id
		AND CR.company_id = ISNULL(@branch,I.source_id)

        WHERE c.claim_status_id not in (3,5)
		AND c.is_dirty=0
		AND CR.effective_from IN
		(
		SELECT MAX(effective_from)
		FROM CurrencyRate
		WHERE effective_from <= C.reported_date
		AND   currency_id = CR.currency_id
		AND company_id = CR.company_id
		)

    -- print '...then Recoveries'
    INSERT INTO #tempRSAOutstClaims
        SELECT rt.code,
            rt.description,
            (select description from recovery_type where recovery_type_id = rec.recovery_type_id),
            c.claim_number,
            c.insurer_short_name,
            c.policy_number,
            c.client_short_name,
            c.client_name,
            c.loss_from_date,
            LEFT(c.description,255),
            1,
            c.currency_id,
		--res.initial_reserve,
		0,
		--            res.revised_reserve,
		Case @TypeOfCurrency
			WHEN 'Base' THEN (isnull(rec.revised_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base) + isnull(rec.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))*-1
			WHEN 'System' THEN (isnull(rec.revised_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base) + isnull(rec.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))*-1
			WHEN 'Transaction' THEN isnull(rec.revised_reserve,0) + (isnull(rec.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))*-1
		END,

		Case @TypeOfCurrency
			WHEN 'Base' THEN (isnull(rec.received_to_date,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)) *-1
			WHEN 'System' THEN (isnull(rec.received_to_date,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))*-1
			WHEN 'Transaction' THEN isnull(rec.received_to_date,0)*-1
		END,I.source_id
        FROM #tempClaims tc
        JOIN claim c            ON tc.ClaimID = c.claim_id
        JOIN Claim_Peril cp     ON c.claim_id = cp.claim_id                 -- Claim_Peril link
        join (select base_claim_id, max(version_id) version_id from claim where is_dirty<>1 group by base_claim_id) max_version
        ON max_version.base_claim_id = c.base_claim_id and max_version.version_id = c.version_id
        JOIN [Recovery] rec     ON cp.claim_peril_id = rec.claim_peril_id
        JOIN Risk r             ON c.risk_type_id = r.risk_cnt
        JOIN Risk_Type rt       ON rt.risk_type_id = r.risk_type_id
        join Insurance_file I  ON c.Policy_id = I.insurance_file_cnt
		INNER JOIN currencyrate CR ON CR.currency_id = C.currency_id
		AND CR.company_id = ISNULL(@branch,I.source_id)

        WHERE c.claim_status_id not in (3,5)
        AND c.is_dirty=0
        AND CR.effective_from IN
		(
		SELECT MAX(effective_from)
		FROM CurrencyRate
		WHERE effective_from <= C.reported_date
		AND   currency_id = CR.currency_id
		AND company_id = CR.company_id
		)

END

DROP TABLE #tempClaims
-- print 'select from temp table, calculating currencies'
SELECT RiskTypeCode,
        RiskTypeDesc,
        ReserveType,
        ClaimNumber,
        AgentCode,
        InsuranceRef,
        ClientCode,
        ClientName,
        LossFromDate,
        ClaimDesc,
        InitialReserve ,
        RevisedReserve ,
        Payments ,
 		S.Code CompanyCode,
 		S.description CompanyDesc,
 		Case @TypeOfCurrency
 			WHEN 'System' THEN  @Systemcurrencycode
 			WHEN 'Base' THEN C.Code
 			WHEN 'Transaction' THEN CT.Code
 		END CurrencyCode,
 		Case @TypeOfCurrency
 			WHEN 'System' THEN @SystemCurrencyDesc
 			WHEN  'Base' THEN C.description
 			WHEN 'Transaction' THEN CT.description
 		END CurrencyDesc,
 		Case @GroupbyCode
 			WHEN 'Branch' THEN S.Code
 			WHEN 'Branch And Currency' THEN S.Code
 			WHEN 'Currency' THEN CT.Code
 		ELSE ''
 		END 'GroupByCode'
	FROM #tempRSAOutstClaims TA
 	INNER JOIN Source S ON S.source_id = TA.sourceid
	INNER JOIN CURRENCY C ON C.Currency_id= S.base_currency_id
	INNER JOIN Currency CT ON CT.Currency_id = TA.Currencyid
DROP TABLE #tempRSAOutstClaims

