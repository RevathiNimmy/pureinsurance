SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Claims_by_Catastrophe'
GO


CREATE PROCEDURE spu_Report_Claims_by_Catastrophe
                @CatastropheCode varchar(255),
                @ClaimType varchar(255)
AS
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
    FROM Claim
    WHERE   (
            (Claim_Status_id in (2, 4) AND @ClaimType = 'Outstanding Claims')
            OR
            (Claim_Status_id <> 1 AND @ClaimType = 'All Claims')
            OR
            (Claim_Status_id in (3, 5) AND @ClaimType = 'Closed Claims')
            )

    AND  isnull(catastrophe_code_id,0) <> 0


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
    CurrencyID          int NULL
)

INSERT INTO #tempClaimsByCatastrophe

    SELECT  r.Initial_Reserve,
        r.Revised_Reserve,
        r.Sum_insured,
        r.Paid_to_Date,
        (isnull(r.Initial_reserve,0) + isnull(r.Revised_reserve,0)) - isnull(r.Paid_to_date,0),
        isnull(r.Initial_reserve,0) + isnull(r.Revised_reserve,0),
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
        c.currency_id
    FROM
        Reserve r
        JOIN claim_peril cp     ON r.claim_peril_id = cp.claim_peril_id
        JOIN claim c            ON cp.claim_id = c.claim_id
        JOIN #tempClaims tc     ON tc.ClaimID = c.claim_id
        JOIN Insurance_file I   ON c.Policy_id = I.insurance_file_cnt
        LEFT JOIN Product p             ON i.product_id = P.product_id
        LEFT JOIN claim_risk cr         ON c.claim_id = cr. claim_id
        LEFT JOIN Risk_type rt          ON cr.risk_type_id = rt.risk_type_Id
        LEFT JOIN catastrophe_code cc   ON c.catastrophe_code_id = cc.catastrophe_code_id
    WHERE   (
            (cc.description = @CatastropheCode)
            OR
            (@CatastropheCode = 'ALL')
            )
    AND     (
            isnull(r.Initial_Reserve,0) <> 0
            OR
            isnull(r.Sum_insured,0) <> 0
            OR
            isnull(r.Sum_insured,0) <> 0
            OR
            isnull(r.Paid_to_Date,0) <> 0
            )

DROP TABLE #tempClaims

-- Use cursor to fetch currency rate details
-- Cursor variables
DECLARE @TempID int,
        @LossFromDate datetime,
        @CurrencyRate money,
        @CurrencyId int

-- Additional variable for calculating home currency values
DECLARE @CCYeffectiveFrom datetime

DECLARE CatClaims_cursor CURSOR FAST_FORWARD FOR
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

    WHILE @@FETCH_STATUS = 0 BEGIN
        IF @CurrencyId <> 26            -- 26 = id of home currency (leave as it is)
        BEGIN
            --print 'not home currency'
            SELECT @CCYeffectiveFrom = (SELECT max(effective_from)
                                        FROM  CurrencyRate
                                        WHERE currency_id = @CurrencyId
                                        AND effective_from <= @LossFromDate
                                        GROUP BY currency_id)

            IF @CCYeffectiveFrom IS NOT NULL BEGIN
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
DEALLOCATE CatClaims_cursor

SET NOCOUNT OFF

SELECT  Initial_Reserve*CurrencyRate Initial_Reserve,
        Revised_Reserve*CurrencyRate Revised_Reserve,
        Sum_Insured*CurrencyRate Sum_Insured,
        paid_to_date*CurrencyRate paid_to_date,
        CurrentReserve*CurrencyRate CurrentReserve,
        IncurredAmount*CurrencyRate IncurredAmount,
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
        catastrophe_code_id
FROM #tempClaimsByCatastrophe

DROP TABLE #tempClaimsByCatastrophe

GO
