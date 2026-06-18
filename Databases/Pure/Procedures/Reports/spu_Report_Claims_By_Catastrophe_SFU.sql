
EXECUTE DDLDropProcedure 'spu_Report_Claims_by_Catastrophe_SFU'
GO

/*****************************************
**  Claims by Catastrophe report
**
**  Created: P. Haynes 6/9/2001
**
**  Claims_by_Catastrophe.rpt
**
*****************************************
** VER      DATE        WHO     DESC
** 1.01                 JMK     add parameters
**                              retrieve currency rates properly (stored in Claims tables in original currency)
**
** 1.02     03/01/2002  JMK     increase to varchar(255) for descriptions
**                              remove "..ANSI NULLS..." part of script at start and end
**
** 1.03     01/02/2002  JMK     Add date parameters
**
** 1.04     04/04/2002  JMK     Remove date parameters! as requested by Steve Watson RSA
**                              Amend Claim type parameter
** 1.05		27/08/2004	JT		Multicurrency changes 
***********************************************************************************************************************************/

/*
    Claim status id constants
    1 = Provisional Open Claim
    2 = Live Open Claim
    3 = Closed
    4 = ReOpen
    5 = ReClosed
*/

/*
ClaimType:
    All Claims
    Outstanding Claims
    Closed Claims

*/

CREATE PROCEDURE spu_Report_Claims_by_Catastrophe_SFU
                @CatastropheCode varchar (60),
                @ClaimType varchar (20),
                @TypeOfCurrency	Varchar(30),
                @GroupByCode	Varchar(30)
                
AS
SET NOCOUNT ON

/*
--for testing
DECLARE @CatastropheCode varchar (60),
        @ClaimType varchar (20)
SELECT @CatastropheCode = 'all',
        @ClaimType = 'All Claims'
*/

CREATE TABLE #tempClaims
(
    ClaimID int
)
INSERT INTO #tempClaims
    SELECT Claim_id
    FROM Claim CL
    WHERE   (
            (Claim_Status_id in (2, 4) AND @ClaimType = 'Outstanding Claims')
            OR
            (Claim_Status_id <> 1 AND @ClaimType = 'All Claims')
            OR
            (Claim_Status_id in (3, 5) AND @ClaimType = 'Closed Claims')
            )

    AND  isnull(catastrophe_code_id,0) <> 0
    AND is_dirty <> 1
    AND  version_id=(select max(version_id) from claim c where base_claim_id =  
             (select base_claim_id from claim c where c.claim_id=cl.claim_id)) 




CREATE TABLE #tempClaimsByCatastrophe
(
    TempID int IDENTITY,
    Initial_Reserve     money null,
    Revised_Reserve     money null,
    Sum_Insured         money null,
    paid_to_date        money null,
    CurrentReserve      money null,
    IncurredAmount      money null,
    Claim_Number        varchar(60) null,
    Policy_Number       varchar(60) null,
    Client_Name         varchar(60) null,
    Insurer_Name        varchar(60) null,
    loss_from_date      datetime null,
    Client_Short_name   varchar(60) null,
    ClaimDescription    varchar(255) null,
    ProductCode         varchar(10) null,
    ProductDescription  varchar(255) null,
    RiskDescription     varchar(255) null,
    CatastropheDescription  varchar(60) null,
    catastrophe_code_id varchar(10) null,
    CurrencyRate        money NULL,
    CurrencyID          int NULL,
    SourceID			Int	NULL,
    IsSumInsured        Int NULL
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
Declare @ClaimNum varchar(50)
EXEC spu_ACT_GetTypeOfRates @TypeOfRates OUTPUT 
IF @TypeOfRates =1 
	SELECT @branch=1 
ELSE 
	BEGIN
		SELECT @branch=NULL 
	END
	
INSERT INTO #tempClaimsByCatastrophe

    SELECT  
    	--r.Initial_Reserve,
			Case @TypeOfCurrency 
				WHEN 'Base' THEN (isnull(r.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
				WHEN 'System' THEN (isnull(r.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
				WHEN 'Transaction' THEN isnull(r.Initial_reserve,0)
				
			END,
    	--r.Revised_Reserve,
    		Case @TypeOfCurrency 
				WHEN 'Base' THEN (isnull(r.Revised_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))
				WHEN 'System' THEN  (isnull(r.Revised_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))
				WHEN 'Transaction' THEN isnull(r.Revised_reserve,0)
			END,
    --    r.Sum_insured,
    	Case @TypeOfCurrency 
				WHEN 'Base' THEN (isnull(cp.Sum_insured,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
				WHEN 'System' THEN (isnull(cp.Sum_insured,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
				WHEN 'Transaction' THEN isnull(cp.Sum_insured,0)
		END,
--        r.Paid_to_Date,
		Case @TypeOfCurrency 
				WHEN 'Base' THEN (isnull(r.Paid_to_date,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))
				WHEN 'System' THEN (isnull(r.Paid_to_date,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))
				WHEN 'Transaction' THEN isnull(r.Paid_to_date,0)
		END,
		
		(Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN (isnull(r.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(r.Initial_reserve,0)
			
		END)
		+
		(Case @TypeOfCurrency 
				WHEN 'Base' THEN (isnull(r.Revised_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))
				WHEN 'System' THEN  (isnull(r.Revised_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))
				WHEN 'Transaction' THEN isnull(r.Revised_reserve,0)
		END
		) -
		(Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Paid_to_date,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN (isnull(r.Paid_to_date,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))
			WHEN 'Transaction' THEN isnull(r.Paid_to_date,0)
		END),
		
		(Case @TypeOfCurrency 
					WHEN 'Base' THEN (isnull(r.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
					WHEN 'System' THEN (isnull(r.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
					WHEN 'Transaction' THEN isnull(r.Initial_reserve,0)
		END)
			+
		(Case @TypeOfCurrency 
				WHEN 'Base' THEN (isnull(r.Revised_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))
				WHEN 'System' THEN  (isnull(r.Revised_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))
				WHEN 'Transaction' THEN isnull(r.Revised_reserve,0)
		END),

        --(isnull(r.Initial_reserve,0) + isnull(r.Revised_reserve,0)) - isnull(r.Paid_to_date,0),
        --isnull(r.Initial_reserve,0) + isnull(r.Revised_reserve,0),
        c.claim_Number,
        c.Policy_number,
        c.Client_Name,
        c.Insurer_Name,
        c.loss_from_date,
        c.Client_Short_name,
        LEFT(c.description,255),
        p.code,
        p.description,
        rt.description,
        cc.description,
        c.catastrophe_code_id,
        1,
        c.currency_id,I.source_id,0
    FROM
        Reserve r
        JOIN claim_peril cp     ON r.claim_peril_id = cp.claim_peril_id
        JOIN claim c            ON cp.claim_id = c.claim_id
        JOIN #tempClaims tc     ON tc.ClaimID = c.claim_id
        JOIN Insurance_file I   ON c.Policy_id = I.insurance_file_cnt
        INNER JOIN currencyrate CR ON CR.currency_id = C.currency_id
        AND CR.company_id = ISNULL(@branch,I.source_id)
        LEFT JOIN Product p             ON i.product_id = P.product_id
        LEFT JOIN claim_risk crk         ON c.claim_id = crk. claim_id
        LEFT JOIN Risk_type rt          ON crk.risk_type_id = rt.risk_type_Id
        LEFT JOIN catastrophe_code cc   ON c.catastrophe_code_id = cc.catastrophe_code_id
    WHERE   (
            (cc.description = @CatastropheCode)
            OR
            (@CatastropheCode = 'ALL')
            )
    --AND     (
    --        isnull(r.Initial_Reserve,0) <> 0
    --        OR
    --        isnull(r.Sum_insured,0) <> 0
    --        OR
    --        isnull(r.Sum_insured,0) <> 0
    --        OR
    --        isnull(r.Paid_to_Date,0) <> 0
    --        )
AND r.version_id=(SELECT MAX(version_id) FROM  claim WHERE claim_Number= c.claim_Number)     
AND CR.effective_from IN
		(
			SELECT MAX(effective_from)
				FROM CurrencyRate 
				WHERE effective_from <= C.reported_date
				AND   currency_id = CR.currency_id
				AND company_id = CR.company_id
		)
		AND c.is_dirty <> 1
--This cursor is specially used for sum insured field in the SP.The sum insured was repeating for all the claim perils.
--This cursor updates only top 1 record and rest are all zeros. This is done to show correct sum insured field on the report   PM022308
    Declare tmp_cursor Cursor Fast_Forward For  
      SELECT DISTINCT claim_number FROM #tempClaimsByCatastrophe 
    Open tmp_cursor  
    Fetch Next From tmp_cursor Into @ClaimNum  
    While (@@Fetch_Status = 0)  
    Begin 
		UPDATE TOP (1) tmp 
		SET IsSumInsured = 1
		FROM #tempClaimsByCatastrophe tmp WHERE tmp.claim_number = @ClaimNum
		
		UPDATE tmp 
		SET Sum_Insured = 0
		FROM #tempClaimsByCatastrophe tmp WHERE tmp.claim_number = @ClaimNum AND IsSumInsured <> 1
	   Fetch Next From tmp_cursor Into @ClaimNum  
    End  
    Close tmp_cursor  
    Deallocate tmp_cursor
DROP TABLE #tempClaims

/*-- Use cursor to fetch currency rate details
-- Cursor variables
DECLARE @TempID int,
        @LossFromDate datetime,
        @CurrencyRate money,
        @CurrencyId int

-- Additional variable for calculating home currency values
DECLARE @CCYeffectiveFrom datetime

DECLARE CatClaims_cursor CURSOR FOR
    SELECT TempID,
            loss_from_date,
            CurrencyRate,
            CurrencyId
     FROM #tempClaimsByCatastrophe

OPEN    CatClaims_cursor

    FETCH NEXT FROM CatClaims_cursor
    INTO    @TempID,
            @LossFromDate,
            @CurrencyRate,
            @CurrencyId

    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF @CurrencyId <> 26            -- 26 = id of home currency (leave as it is)
        BEGIN
            --print 'not home currency'
            SELECT @CCYeffectiveFrom = (SELECT max(effective_from)
                                        FROM  CurrencyRate
                                        WHERE currency_id = @CurrencyId
                                        AND effective_from <= @LossFromDate
                                        GROUP BY currency_id)

            IF @CCYeffectiveFrom IS NOT NULL
            BEGIN
                SELECT @CurrencyRate = (SELECT rate_against_base
                                            FROM  CurrencyRate
                                            WHERE currency_id = @CurrencyId
                                            AND effective_from = @CCYeffectiveFrom)

                UPDATE #tempClaimsByCatastrophe
                    SET  CurrencyRate = @CurrencyRate
                    WHERE TempID = @TempID
            END
        END

        FETCH NEXT FROM CatClaims_cursor
        INTO    @TempID,
                @LossFromDate,
                @CurrencyRate,
                @CurrencyId

    END
CLOSE CatClaims_cursor
DEALLOCATE CatClaims_cursor*/

SET NOCOUNT OFF

SELECT  Initial_Reserve,
        Revised_Reserve,
        Sum_Insured,
        paid_to_date,
        CurrentReserve,
        IncurredAmount,
        Claim_Number,
        Policy_Number,
        Client_Name,
        Insurer_Name,
        loss_from_date,
        Client_Short_name,
        ClaimDescription,
        ProductCode,
        ProductDescription,
        RiskDescription,
        CatastropheDescription,
        catastrophe_code_id,
        S.Code CompanyCode,S.Description CompanyDesc,
        Case @TypeOfCurrency 
        	WHEN 'Base' THEN CB.Code
        	WHEN 'System' THEN @SystemcurrencyCode
        	WHEN 'Transaction' THEN CT.Code
        END CurrencyCode,
        Case @TypeOfCurrency 
        	WHEN 'Base' THEN CB.Description
        	WHEN 'System' THEN @SystemcurrencyDesc
        	WHEN 'Transaction' THEN CT.description
        END CurencyDesc,
        Case @GroupByCode 
        	WHEN 'Branch' THEN S.Code
        	WHEN 'Branch And Currency' THEN S.Code
        	WHEN 'Currency' THEN CT.Code
        	ELSE ' '
        END GroupByCode
        
        
FROM #tempClaimsByCatastrophe TC
INNER JOIN Source S ON s.source_id = TC.SourceID
INNER JOIN Currency CB ON CB.Currency_id = S.Base_currency_id
JOIN Currency ct /*Transaction Currency*/
ON ct.currency_id = tc.currencyid	

DROP TABLE #tempClaimsByCatastrophe

GO
