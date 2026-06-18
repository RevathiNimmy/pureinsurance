SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Agent_Perf_by_Year_SFU'
GO



/**********************************************************************************************************************************
** Created by Kerry Butler
** 08/10/2001
** Agency Reports -  Agent Performance by Calendar Year
**  1.1 KB 17/12/01 Calculate unearned premium within the select statement, it doesnt seem
**              to work via updates
**              Ensure 'Direct' policies are listed as such
**      1.2 KB 08/01/02 Report should group by product rather than class of business.
**              So amended selection criteria to use product from stats_folder.
**              This means update to Class of Business from class-of_business_id is
**                              no longer required. Variable names have not been changed to avoid
**              changing the report itself.
**      1.3 KB 09/01/02 Use stats_detail record to get claim paid and reserve values rather than
**              going back to the claim table. This avoids double counting of records.
**
**      1.4 JMK 13/03/2002  Amend Earned Premium calc to take future cover start dates into account.
**                          Filter out direct client records.
**      1.5 KB  20/03/2002  Prevent divide by zero error
**      1.6 KB  22/03/2002  Use name from the party table to populate agent name as trading name on
**                          party_agent is not populated after data transfer
**      1.7 TOMB 28/02/2003 Stop any possibility of divide by zero errors
**                          Add Agent_ShortName to output:  Rename ClassofBus to Product
**                          Add parameters:  Branch ID, Basis (by Date or PeriodID), Report Level (Summary/Detail)
**      1.8 TOMB 08/07/2003 Change OSLossReseve column to CostOfClaim column
**      1.9 TOMB 17/07/2003 Replace ULR column with TAX, as AUA want figures net of tax
**		20.	JT	 13/08/2004	MultiCurrency changes. 	
**      2.1 RC 12Jun2006 - Filter by Agent Group
**********************************************************************************************************************************

***********************************************************************************************************************************/
create PROCEDURE spu_Report_Agent_Perf_by_Year_SFU 
    @branch_id int,
    @AgentShortName varchar(20),
    @sBasis varchar(50),
    @sReportLevel varchar(50),   -- eventually do detailed and summary versions
    @TypeOfCurrency	Varchar(30),
    @GroupbyCode	Varchar(30),
    @AgentGroupCode	Varchar(30)
         AS

SET NOCOUNT ON

DECLARE @dtCurrentPeriodEnd datetime
DECLARE @dDailyRate decimal (19,4)

-- TB 28/02/2003:  Parameter changes - set up code
-- Report Basis:  By Date or by Period ID
DECLARE @iBasis INT
-- Branch (Company No)
DECLARE @iBranchID INT
-- Branch to select from Period Table
DECLARE @iBranchPeriod INT
-- Always use Branch 1 in the period table
-- if anyone sets up different periods for different branches this will
-- need to be revisited and worked out for the branch
SELECT @iBranchPeriod = 1
IF @branch_id is null
    SELECT @iBranchID = 0
ELSE
    SELECT @iBranchID = @branch_id

Declare @Branch Int
Declare @TypeOfRates Int
EXEC spu_ACT_GetTypeOfRates @TypeOfRates OUTPUT 
IF @TypeOfRates =1 
	SELECT @branch=1 
ELSE 
	BEGIN
		SELECT @branch=NULL 
	END

IF @sBasis = 'Transaction Date'
BEGIN
    SELECT @iBasis = 1    -- Transaction Date
END
ELSE
BEGIN
    SELECT @iBasis = 0    -- Transaction Period
END

/*Get System Currency Details*/
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
/* AUA Claims Frig */

-- Get the Incurred Claims first
CREATE TABLE #ClaimReserve
 (
    ClaimID int,
    InitRsv numeric NULL,
    Paid    numeric(19,4) NULL,
    RevRsv  numeric(19,4) NULL
 )

IF @iBasis = 0  -- Transaction Period
BEGIN
    INSERT #ClaimReserve
    SELECT cp.claim_id,  
    Case @TypeOfCurrency 
    	WHEN 'Base' THEN sum(isnull(rv.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
    	WHEN 'System' THEN sum(isnull(rv.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base) / ISNULL(C.system_base_xrate,CR.rate_against_base))
    END,
    Case @TypeOfCurrency 
    WHEN 'Base' THEN sum(isnull(rv.Paid_to_date,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))
 	WHEN 'System' THEN sum(isnull(rv.Paid_to_date,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))
    END,
    Case @TypeOfCurrency 
        WHEN 'Base' THEN sum(isnull(rv.this_revision,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))
    	WHEN 'System' THEN  sum(isnull(rv.this_revision,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))
    END
    
    FROM reserve rv
    INNER JOIN claim_peril cp
       ON rv.claim_peril_id = cp.claim_peril_id
     INNER JOIN Claim C
     	ON C.claim_id = cp.claim_id
     INNER JOIN insurance_file ifi 
     	ON ifi.insurance_file_cnt  = C.policy_id
     JOIN currencyrate CR
	 	ON CR.currency_id = C.currency_id
		AND CR.company_id = ISNULL(@branch,IFI.source_id)
    WHERE cp.claim_id in (SELECT sf.loss_id
                     FROM stats_folder sf
                    INNER JOIN stats_detail sd
                       ON sd.stats_folder_cnt = sf.stats_folder_cnt
                    WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code LIKE ('C_%')               -- just claims
    /*                    AND ( sf.posting_period_number = @SelectedPeriodID OR
                              sf.posting_period_number = @12PeriodsAgoID OR
                             (sf.posting_period_number BETWEEN @YearStartPeriodID AND @SelectedPeriodID ) OR
                             (sf.posting_period_number BETWEEN @YearBeforeStartID AND @12PeriodsAgoID) OR
                             (sf.posting_period_number BETWEEN @12PeriodsAgoID + 1 AND @SelectedPeriodID) OR
                             (sf.posting_period_number BETWEEN @24PeriodsAgoID + 1  AND @12PeriodsAgoID)
                            ) */
                    )
					AND    CR.effective_from IN      
       (      
       SELECT MAX(effective_from)      
       FROM CurrencyRate      
       WHERE effective_from <= C.reported_date      
       AND   currency_id = CR.currency_id      
       AND company_id = CR.company_id      
       )    
    GROUP BY cp.claim_id
END    -- iBasis = 0 Transaction Period
ELSE
BEGIN    -- iBasis <> 0 Transaction Date
    INSERT #ClaimReserve
    SELECT cp.claim_id,  
    	Case @TypeOfCurrency 
	    	WHEN 'Base' THEN sum(isnull(rv.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
	    	WHEN 'System' THEN sum(isnull(rv.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base) / ISNULL(C.system_base_xrate,CR.rate_against_base))
		 END,
	    Case @TypeOfCurrency 
	    WHEN 'Base' THEN sum(isnull(rv.Paid_to_date,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base))
	 	WHEN 'System' THEN sum(isnull(rv.Paid_to_date,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))
	    END,
	    Case @TypeOfCurrency 
	    	WHEN 'Base' THEN sum(isnull(rv.this_revision,0)*	ISNULL(c.currency_base_xrate,CR.rate_against_base))
	    	WHEN 'System' THEN  sum(isnull(rv.this_revision,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))
    END
     FROM reserve rv
    INNER JOIN claim_peril cp
       ON rv.claim_peril_id = cp.claim_peril_id
	INNER JOIN Claim C
		ON C.claim_id = cp.claim_id
	INNER JOIN insurance_file ifi 
     	ON ifi.insurance_file_cnt  = C.policy_id
	JOIN currencyrate CR
		ON CR.currency_id = IFI.currency_id
	AND  CR.company_id = ISNULL(@branch,IFI.source_id)
    WHERE cp.claim_id in (SELECT sf.loss_id
                     FROM stats_folder sf
                    INNER JOIN stats_detail sd
                       ON sd.stats_folder_cnt = sf.stats_folder_cnt
                    WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code LIKE ('C_%')               -- just claims
     /*                   AND ( ( sf.document_Date > @prev_period_end_date AND sf.document_Date <= @period_end_date    ) OR
                              ( sf.document_Date > @dt12PeriodsAgoPrev   AND sf.document_date <= @dt12PeriodsAgo     ) OR
                              ( sf.document_date > @dtYEarStart          AND sf.document_Date <= @period_end_date    ) OR
                              ( sf.document_date > @dtYearBeforeStart    AND sf.document_date <= @dt12PeriodsAgoPrev ) OR
                              ( sf.document_date > @dt12PeriodsAgo       AND sf.document_date <= @period_end_date    ) OR
                              ( sf.document_date > @dt24PeriodsAgo       AND sf.document_date <= @dt12PeriodsAgoPrev )
                            ) */
                    )
					 
                 AND    CR.effective_from IN      
       (      
       SELECT MAX(effective_from)      
       FROM CurrencyRate      
       WHERE effective_from <= C.reported_date      
       AND   currency_id = CR.currency_id      
       AND company_id = CR.company_id      
       )       
    GROUP BY cp.claim_id
END    -- iBasis <> 0 Transaction Date


CREATE TABLE #ClaimRecovery
 (
    ClaimID int,
    InitRsv numeric NULL,
    Received numeric(19,4) NULL,
    RevRsv  numeric(19,4) NULL
 )

/*
IF @iBasis = 0  -- Transaction Period
BEGIN
END    -- iBasis = 0 Transaction Period
ELSE
BEGIN    -- iBasis <> 0 Transaction Date
END    -- iBasis <> 0 Transaction Date
*/

IF @iBasis = 0  -- Transaction Period
BEGIN
    INSERT #ClaimRecovery
    SELECT cp.claim_id, 
    /*sum(rc.Initial_reserve), 
    sum(rc.received_to_date), 
    sum(rc.Revised_reserve)*/
    Case @TypeOfCurrency 
		    	WHEN 'Base' THEN sum(rc.Initial_reserve *ISNULL(C.currency_base_xrate,CR.rate_against_base))
		    	WHEN 'System' THEN sum(rc.Initial_reserve *	ISNULL(C.currency_base_xrate,CR.rate_against_base) / ISNULL(C.system_base_xrate,CR.rate_against_base))
			 END,
		    Case @TypeOfCurrency 
		    WHEN 'Base' THEN sum(rc.received_to_date *ISNULL(c.currency_base_xrate,CR.rate_against_base))
		 	WHEN 'System' THEN sum(rc.received_to_date *ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))
		    END,
		    Case @TypeOfCurrency 
		    	WHEN 'Base' THEN sum(rc.Revised_reserve * ISNULL(c.currency_base_xrate,CR.rate_against_base))
		    	WHEN 'System' THEN  sum(isnull(rc.Revised_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))
    END
     FROM recovery rc
    INNER JOIN claim_peril cp
       ON rc.claim_peril_id = cp.claim_peril_id
    INNER JOIN Claim C
	   	ON C.claim_id = cp.claim_id
	INNER JOIN insurance_file ifi 
     	ON ifi.insurance_file_cnt  = C.policy_id
    JOIN currencyrate CR
	 	ON CR.currency_id = IFI.currency_id
		AND CR.company_id = ISNULL(@branch,IFI.source_id)
    WHERE cp.claim_id in (SELECT sf.loss_id
                         FROM stats_folder sf
                        INNER JOIN stats_detail sd
                           ON sd.stats_folder_cnt = sf.stats_folder_cnt
                        WHERE sd.stats_detail_type = 'GRS'
                          AND sf.transaction_type_code LIKE ('C_%')               -- just claims
    /*                      AND ( sf.posting_period_number = @SelectedPeriodID OR
                                sf.posting_period_number = @12PeriodsAgoID OR
                               (sf.posting_period_number BETWEEN @YearStartPeriodID AND @SelectedPeriodID ) OR
                               (sf.posting_period_number BETWEEN @YearBeforeStartID AND @12PeriodsAgoID) OR
                               (sf.posting_period_number BETWEEN @12PeriodsAgoID + 1 AND @SelectedPeriodID) OR
                               (sf.posting_period_number BETWEEN @24PeriodsAgoID + 1  AND @12PeriodsAgoID)
                              ) */
                       )
    GROUP BY cp.claim_id
END    -- iBasis = 0 Transaction Period
ELSE
BEGIN    -- iBasis <> 0 Transaction Date
    INSERT #ClaimRecovery
    SELECT cp.claim_id, 
    /*sum(rc.Initial_reserve), 
    sum(rc.received_to_date), 
    sum(rc.Revised_reserve)*/
				Case @TypeOfCurrency 
					WHEN 'Base' THEN sum(rc.Initial_reserve * ISNULL(C.currency_base_xrate,CR.rate_against_base))
					WHEN 'System' THEN sum(rc.Initial_reserve *ISNULL(C.currency_base_xrate,CR.rate_against_base) / ISNULL(C.system_base_xrate,CR.rate_against_base))
				 END,
			    Case @TypeOfCurrency 
			    WHEN 'Base' THEN sum(rc.received_to_date *ISNULL(c.currency_base_xrate,CR.rate_against_base))
			 	WHEN 'System' THEN sum(rc.received_to_date *ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))
			    END,
			    Case @TypeOfCurrency 
			    	WHEN 'Base' THEN sum(rc.Revised_reserve *ISNULL(c.currency_base_xrate,CR.rate_against_base))
			    	WHEN 'System' THEN  sum(isnull(rc.Revised_reserve,0)*ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))
   			 END
     FROM recovery rc
    INNER JOIN claim_peril cp
       ON rc.claim_peril_id = cp.claim_peril_id
       INNER JOIN Claim C
	   ON C.claim_id = cp.claim_id
       INNER JOIN insurance_file ifi 
	        	ON ifi.insurance_file_cnt  = C.policy_id
	       JOIN currencyrate CR
	   	 	ON CR.currency_id = IFI.currency_id
		AND CR.company_id = ISNULL(@branch,IFI.source_id)
    WHERE cp.claim_id in (SELECT sf.loss_id
                         FROM stats_folder sf
                        INNER JOIN stats_detail sd
                           ON sd.stats_folder_cnt = sf.stats_folder_cnt
                        WHERE sd.stats_detail_type = 'GRS'
                          AND sf.transaction_type_code LIKE ('C_%')               -- just claims
 /*                       AND ( ( sf.document_Date > @prev_period_end_date AND sf.document_Date <= @period_end_date    ) OR
                              ( sf.document_Date > @dt12PeriodsAgoPrev   AND sf.document_date <= @dt12PeriodsAgo     ) OR
                              ( sf.document_date > @dtYEarStart          AND sf.document_Date <= @period_end_date    ) OR
                              ( sf.document_date > @dtYearBeforeStart    AND sf.document_date <= @dt12PeriodsAgoPrev ) OR
                              ( sf.document_date > @dt12PeriodsAgo       AND sf.document_date <= @period_end_date    ) OR
                              ( sf.document_date > @dt24PeriodsAgo       AND sf.document_date <= @dt12PeriodsAgoPrev )
                            ) */
                    )
    GROUP BY cp.claim_id
END    -- iBasis <> 0 Transaction Date


CREATE TABLE #IncurredClaims
(
    ClaimID int,
    IncurredClaims numeric(19,4) NULL,
    OSLossReserve numeric(19,4) NULL,        -- TB 12/3/03 - use instead of IncurredClaims, original so left in case they want both
    CostOfClaim numeric(19,4) NULL            -- TB 08/07/03 - use instead of OSLossReserve
)

--SELECT * FROM #ClaimReserve
--SELECT * FROM #ClaimRecovery
/* View contents of claims incurred tables
SELECT rv.claimID, isnull(rv.InitRsv,0), isnull(rv.Paid,0), isnull(rv.RevRsv,0),
                   isnull(rc.InitRsv,0), isnull(rc.Received,0),  isnull(rc.RevRsv,0),
                   'Incurred'= isnull(rv.InitRsv,0) + isnull(rv.RevRsv,0) - isnull(rv.Paid,0) + isnull(rc.Received,0)
  FROM #ClaimReserve rv
  LEFT OUTER JOIN #ClaimRecovery rc
    ON rv.claimid = rc.claimid
 ORDER BY incurred
*/

INSERT #IncurredClaims
-- TB 16/08/02 - leave the old formula in, in case I need to change it back
--SELECT rv.claimID, 'Incurred'= isnull(rv.InitRsv,0) + isnull(rv.RevRsv,0) - isnull(rv.Paid,0) + isnull(rc.Received,0)
-- TB 12/3/03: This is Incurred, but report wants O/S Loss Reserve, so subtract payments
--SELECT rv.claimID, 'Incurred'= isnull(rv.InitRsv,0) + isnull(rv.RevRsv,0)
SELECT rv.claimID, 'IncurredClaims'= isnull(rv.RevRsv,0),
    'OSLossReserve'=  isnull(rv.RevRsv,0) - isnull(rv.Paid,0),
    'CostOfClaim'=  isnull(rv.RevRsv,0) - isnull(rc.Received,0) 
  FROM #ClaimReserve rv
  LEFT OUTER JOIN #ClaimRecovery rc
    ON rv.claimid = rc.claimid
-- ORDER BY incurred



CREATE TABLE #tempAgtPerfYear
    (
    CurrentYear datetime NULL,
    AgentCode   varchar(20) NULL,
        AgentName   varchar (255) NULL,
    AgentCnt   int NULL,
--    ClassofBus  varchar (255) NULL,  -- We are actually selecting product not COB (see comment above)
    Product  varchar (255) NULL,
    ClassofBusId    int NULL,
    GrossPremium    decimal (19,4) NULL,
    Commission  decimal (19,4) NULL,
    PPYear      int NULL,
    TransactionType varchar (20)   NULL,
    ClaimsPaid  decimal (19,4) NULL,
    IncurredClaims   decimal (19,4) NULL,
--    UnearnedPremium decimal (19,4) NULL,  -- Not required (Kassim)
    PolicyStart datetime NULL,
    PolicyEnd   datetime NULL,
    MidnightRenewal int NULL,

    YearCol     int NULL,
    GP5         decimal (19,4) NULL,    GP4  decimal (19,4) NULL, GP3  decimal (19,4) NULL,
    GP2         decimal (19,4) NULL,    GP1  decimal (19,4) NULL, GP0  decimal (19,4) NULL,
    COM5        decimal (19,4) NULL,    COM4 decimal (19,4) NULL, COM3 decimal (19,4) NULL,
    COM2        decimal (19,4) NULL,    COM1 decimal (19,4) NULL, COM0 decimal (19,4) NULL,
    PAY5        decimal (19,4) NULL,    PAY4 decimal (19,4) NULL, PAY3 decimal (19,4) NULL,
    PAY2        decimal (19,4) NULL,    PAY1 decimal (19,4) NULL, PAY0 decimal (19,4) NULL,
    OSL5        decimal (19,4) NULL,    OSL4 decimal (19,4) NULL, OSL3 decimal (19,4) NULL,
    OSL2        decimal (19,4) NULL,    OSL1 decimal (19,4) NULL, OSL0 decimal (19,4) NULL,
    TAX5        decimal (19,4) NULL,    TAX4 decimal (19,4) NULL, TAX3 decimal (19,4) NULL,
    TAX2        decimal (19,4) NULL,    TAX1 decimal (19,4) NULL, TAX0 decimal (19,4) NULL,

    Document_Ref varchar(25) NULL,
    insurance_Ref varchar(30) NULL,
    insurance_holder_shortname varchar(30) NULL,
    TAX decimal(19,4) NULL,
    ClaimID INT,SourceID	INT NULL
)

EXECUTE spu_Report_GetCurrentPeriod_SFU NULL ,  @dtCurrentPeriodEnd OUTPUT

IF @iBasis = 0  -- Transaction Date

BEGIN

INSERT INTO #tempAgtPerfYear

    SELECT NULL,
        sf.agent_shortname,
        NULL,
        sf.agent_cnt,
        sf.product_code,
        class_of_business_id,
        (SELECT Case @Typeofcurrency 
        	WHEN 'Base' THEN this_premium_home 
        	WHEN 'System' THEN this_premium_system
        END  
        where sf.transaction_type_code not like 'C%'  and sd.stats_detail_type = 'GRS'),
        --lead_Commission_value_home ,
        Case @Typeofcurrency 
		     WHEN 'Base' THEN lead_Commission_value_home 
		     WHEN 'System' THEN lead_Commission_value_system
        END,  
        posting_period_year,
        transaction_type_code,
        (SELECT 
        Case @Typeofcurrency 
        	WHEN 'Base' THEN this_premium_home 
        	WHEN 'System' THEN this_premium_system
        END
        where sf.transaction_type_code like 'C_CP'  and sd.stats_detail_type = 'GRS'),
    --      KB 9/1/02
    --  (SELECT this_premium_home where sf.transaction_type_code in ('C_CO','C_CR')), -- use the stats_deatils instead
        (SELECT --'OSLossRes' = ic.OSLossReserve
                --  'IncurredClaims' = ic.IncurredClaims
                'IncurredClaims'= ic.CostOfClaim
            where sd.stats_detail_type = 'GRS'
              AND sf.transaction_type_code IN ('C_CO','C_CR')),             -- just claims opened
    -- TB 28/02/2003
-- Unearned premium not required (Kassim)
/*        CASE WHEN datediff(day,sf.cover_start_date, @dtCurrentPeriodEnd) <= 0
            THEN (SELECT this_premium_home
                WHERE sf.transaction_type_code not like 'C%')
            ELSE
            -- TB 28/2/03: This can give a divide by zero if cover-start-date = expiry-date or both are null
                (SELECT this_premium_home
               		 *  datediff(day,@dtCurrentPeriodEnd,sf.expiry_date)
                    	/(datediff(day,sf.cover_start_date,sf.expiry_date)+ isnull(p.is_midnight_renewal,0))
                WHERE sf.transaction_type_code not like 'C%'and (datediff(day,sf.cover_start_date,sf.expiry_date)+ isnull(p.is_midnight_renewal,0)) <> 0)
            END,
*/
        sf.cover_start_date,                -- To calculate unearned premium
        sf.expiry_date,                     -- To calculate unearned premium
        isnull(p.is_midnight_renewal,0),    -- To calculate unearned premium
        NULL,
        NULL,   NULL,   NULL,   NULL,   NULL,   NULL,
        NULL,   NULL,   NULL,   NULL,   NULL,   NULL,
        NULL,   NULL,   NULL,   NULL,   NULL,   NULL,
        NULL,   NULL,   NULL,   NULL,   NULL,   NULL,
        NULL,   NULL,   NULL,   NULL,   NULL,   NULL,
        sf.document_Ref,
        sf.insurance_Ref,
        sf.insurance_holder_shortname,
        (SELECT Case @Typeofcurrency 
        	WHEN 'Base' THEN this_premium_home 
        	WHEN 'System' THEN this_premium_system
        END
        where sf.transaction_type_code not like 'C%' and sd.stats_detail_type = 'TAN'),
        (SELECT sf.loss_id where sf.transaction_type_code in ( 'C_CO', 'C_CP' ) ),Sf.source_id

    FROM stats_folder sf
--    LEFT OUTER JOIN stats_detail sd ON sd.stats_folder_cnt = sf.stats_folder_cnt
    INNER JOIN stats_detail sd ON sd.stats_folder_cnt = sf.stats_folder_cnt
    LEFT OUTER JOIN product p on p.product_id = sf.product_id
    LEFT OUTER JOIN #IncurredClaims ic ON sf.loss_id = ic.claimID
    WHERE ( sd.stats_detail_type = 'GRS' or sd.stats_detail_type = 'TAN' )
    AND isnull(sf.agent_cnt,0) <> 0
-- TB 28/02/03: Exclude possible divide by zero data
--  AND isnull(datediff(day,sf.cover_start_date,sf.expiry_date), 0) <> 0
    AND ( @iBranchID = 0
          or    (   @iBranchID <> 0 and sf.source_id = @iBranchID )
        )
    AND ( @AgentShortName = 'ALL'
          OR sf.agent_shortname = @AgentShortName
        )

END  -- Transaction Period Basis
ELSE
BEGIN  -- Transaction Date Basis

INSERT INTO #tempAgtPerfYear

--debug
--DECLARE @AgentShortName varchar(20)
--DECLARE @dtCurrentPeriodEnd datetime
--set @agentshortname = 'SYKES'
--set @dtCurrentPeriodEnd = Getdate()
--    SELECT DISTINCT NULL,
    SELECT NULL,
        sf.agent_shortname,
        NULL,
        sf.agent_cnt,
        sf.product_code,
        class_of_business_id,
        (SELECT Case @Typeofcurrency 
        			WHEN 'Base' THEN this_premium_home 
        			WHEN 'System' THEN this_premium_system
        		END  
        where sf.transaction_type_code not like 'C%' and sd.stats_detail_type = 'GRS'),
        --lead_Commission_value_home ,
        Case @Typeofcurrency 
			WHEN 'Base' THEN lead_Commission_value_home  
			WHEN 'System' THEN lead_Commission_value_system
        END ,
        (SELECT datepart(year,document_Date)),
        transaction_type_code,
        (SELECT 
        	Case @Typeofcurrency 
        		WHEN 'Base' THEN this_premium_home 
        		WHEN 'System' THEN this_premium_system
        	END
        where sf.transaction_type_code like 'C_CP' and sd.stats_detail_type = 'GRS'),
    --  (SELECT this_premium_home where sf.transaction_type_code in ('C_CO','C_CR')), -- use the stats_deatils instead
        (SELECT --'OSLossRes' = ic.OSLossReserve
                --  'IncurredClaims' = ic.IncurredClaims
                   'IncurredClaims' = ic.CostOFClaim
            where sd.stats_detail_type = 'GRS'
              AND sf.transaction_type_code IN ('C_CO','C_CR')),              -- just claims opened
-- Unearned premium not required (Kassim)
/*        CASE WHEN datediff(day,sf.cover_start_date, @dtCurrentPeriodEnd) <= 0
            THEN (SELECT this_premium_home
                WHERE sf.transaction_type_code not like 'C%')
            ELSE
            -- TB 28/2/03: This can give a divide by zero if cover-start-date = expiry-date or both are null
            	(SELECT this_premium_home
                	*  datediff(day,@dtCurrentPeriodEnd,sf.expiry_date)
                	    /datediff(day,sf.cover_start_date,sf.expiry_date)+ isnull(p.is_midnight_renewal,0)
                WHERE sf.transaction_type_code not like 'C%' and (datediff(day,sf.cover_start_date,sf.expiry_date)+ isnull(p.is_midnight_renewal,0)) <> 0)
            END,
*/
        sf.cover_start_date,                -- To calculate unearned premium
        sf.expiry_date,                     -- To calculate unearned premium
        isnull(p.is_midnight_renewal,0),    -- To calculate unearned premium
        NULL,
        NULL,   NULL,   NULL,   NULL,   NULL,   NULL,
        NULL,   NULL,   NULL,   NULL,   NULL,   NULL,
        NULL,   NULL,   NULL,   NULL,   NULL,   NULL,
        NULL,   NULL,   NULL,   NULL,   NULL,   NULL,
        NULL,   NULL,   NULL,   NULL,   NULL,   NULL,
        sf.document_Ref,
        sf.insurance_Ref,
        sf.insurance_holder_shortname,
        (SELECT Case @Typeofcurrency 
        			WHEN 'Base' THEN this_premium_home 
        			WHEN 'System' THEN this_premium_system
        			END  
        where sf.transaction_type_code not like 'C%' and sd.stats_detail_type = 'TAN'),
        (SELECT sf.loss_id where sf.transaction_type_code in ( 'C_CO', 'C_CP' ) ),sf.source_id
    FROM stats_folder sf
--    LEFT OUTER JOIN stats_detail sd ON sd.stats_folder_cnt = sf.stats_folder_cnt
    INNER JOIN stats_detail sd ON sd.stats_folder_cnt = sf.stats_folder_cnt
    LEFT OUTER JOIN product p on p.product_id = sf.product_id
    LEFT OUTER JOIN #IncurredClaims ic ON sf.loss_id = ic.claimID
--    INNER JOIN party py on py.party_cnt =  sf.agent_cnt
    WHERE ( sd.stats_detail_type = 'GRS' or sd.stats_detail_type = 'TAN' )
--    AND py.shortname = @agentshortname
    AND isnull(sf.agent_cnt,0) <> 0
-- TB 28/02/03: Exclude possible divide by zero data
    AND isnull(datediff(day,sf.cover_start_date,sf.expiry_date), 0) <> 0
    AND ( @iBranchID = 0
          or    (   @iBranchID <> 0 and sf.source_id = @iBranchID )
        )
    AND ( @AgentShortName = 'ALL'
          OR sf.agent_shortname = @AgentShortName
        )

END  -- Transaction Date Basis

UPDATE #tempAgtPerfYear

    SET @dDailyRate = GrossPremium / (datediff(day, PolicyStart, PolicyEnd) + MidnightRenewal)
    WHERE ((datediff(day,PolicyStart, PolicyEnd)+ MidnightRenewal) <> 0 AND PolicyEnd > @dtCurrentPeriodEnd)
-- TB 28/02/03: Exclude possible divide by zero data
      AND isnull(datediff(day, PolicyStart, PolicyEnd), 0) <> 0

--UPDATE #tempAgtPerfYear
    --SET UnearnedPremium = @dDailyRate * datediff(day, @dtCurrentPeriodEnd, PolicyEnd)
    --WHERE TransactionType not like 'C%'


UPDATE #tempAgtPerfYear
    SET AgentName = pa.name
    FROM Party pa
    WHERE AgentCnt = pa.party_cnt

UPDATE #tempAgtPerfYear
    SET AgentName = 'Direct'

    WHERE AgentCnt IS NULL

UPDATE #tempAgtPerfYear
    SET currentyear = @dtCurrentPeriodEnd
--  KB 8/1/2 No longer required as we are now using product, which we can pick up directly.
--UPDATE #tempAgtPerfYear
--  SET     ClassofBus  =   cb.description
--  FROM    Class_of_Business cb
--  WHERE   ClassofBusId    =   cb.class_of_business_id

----UPDATE #tempAgtPerfYear
----    SET PPYear = ( SELECT distinct datepart(yyyy, c.loss_from_date)
----                   FROM claim c
----                   WHERE isnull(ClaimID,0) = C.Claim_ID )
--WHERE isnull(ClaimID,0) <> 0
UPDATE #tempAgtPerfYear
    SET Yearcol =  (year (CurrentYear) - PPyear )

UPDATE  #tempAgtPerfYear
    SET     GP5 =   GrossPremium,
        Com5 =  Commission,
        Pay5 =  ClaimsPaid,
        OSL5 =  isnull(IncurredClaims,0), --   + isnull(ClaimsPaid,0)
        TAX5 =  isnull(Tax,0)

    WHERE Yearcol = 5


UPDATE  #tempAgtPerfYear
    SET     GP4 =   GrossPremium,
        Com4 =  Commission,
        Pay4 =  ClaimsPaid,
        OSL4 =  isnull(IncurredClaims,0),  -- + isnull(ClaimsPaid,0)
        TAX4 =  isnull(Tax,0)

    WHERE Yearcol = 4


UPDATE  #tempAgtPerfYear
    SET     GP3 =   GrossPremium,
        Com3 =  Commission,
        Pay3 =  ClaimsPaid,
        OSL3 =  isnull(IncurredClaims,0),  -- + isnull(ClaimsPaid,0)
        TAX3 =  isnull(Tax,0)

    WHERE Yearcol = 3


UPDATE  #tempAgtPerfYear
    SET     GP2 =   GrossPremium,
        Com2 =  Commission,
        Pay2 =  ClaimsPaid,
        OSL2 =  isnull(IncurredClaims,0), --  + isnull(ClaimsPaid,0)
        TAX2 =  isnull(Tax,0)

    WHERE Yearcol = 2


UPDATE  #tempAgtPerfYear
    SET     GP1 =   GrossPremium,
        Com1 =  Commission,
        Pay1 =  ClaimsPaid,
        OSL1 =  isnull(IncurredClaims,0), -- + isnull(ClaimsPaid,0)
        TAX1 =  isnull(Tax,0)

    WHERE Yearcol = 1


UPDATE  #tempAgtPerfYear
    SET     GP0 =   GrossPremium,
        Com0 =  Commission,
        Pay0 =  ClaimsPaid,
        OSL0 = isnull(IncurredClaims,0), -- + isnull(ClaimsPaid,0)
        TAX0 =  isnull(Tax,0)

    WHERE Yearcol = 0



--select * from   #tempAgtPerfYear
CREATE TABLE #tempPerfYear2
    (
    CurrentYear datetime NULL,
    AgentCode   varchar(20) NULL,
    AgentName   varchar (255) NULL,
    AgentCnt   int NULL,
--    ClassofBus  varchar (255) NULL,  -- We are actually selecting product not COB (see comment above)
    Product  varchar (255) NULL,
    ClassofBusId    int NULL,
    GrossPremium    decimal (19,4) NULL,
    Commission  decimal (19,4) NULL,
    PPYear      int NULL,
    TransactionType varchar (20)   NULL,
    ClaimsPaid  decimal (19,4) NULL,
    IncurredClaims   decimal (19,4) NULL,
--    UnearnedPremium decimal (19,4) NULL,  -- Not required (Kassim)
    PolicyStart datetime NULL,
    PolicyEnd   datetime NULL,
    MidnightRenewal int NULL,

    YearCol     int NULL,
    GP5         decimal (19,4) NULL,    GP4  decimal (19,4) NULL, GP3  decimal (19,4) NULL,
    GP2         decimal (19,4) NULL,    GP1  decimal (19,4) NULL, GP0  decimal (19,4) NULL,
    COM5        decimal (19,4) NULL,    COM4 decimal (19,4) NULL, COM3 decimal (19,4) NULL,
    COM2        decimal (19,4) NULL,    COM1 decimal (19,4) NULL, COM0 decimal (19,4) NULL,
    PAY5        decimal (19,4) NULL,    PAY4 decimal (19,4) NULL, PAY3 decimal (19,4) NULL,
    PAY2        decimal (19,4) NULL,    PAY1 decimal (19,4) NULL, PAY0 decimal (19,4) NULL,
    OSL5        decimal (19,4) NULL,    OSL4 decimal (19,4) NULL, OSL3 decimal (19,4) NULL,
    OSL2        decimal (19,4) NULL,    OSL1 decimal (19,4) NULL, OSL0 decimal (19,4) NULL,
    TAX5        decimal (19,4) NULL,    TAX4 decimal (19,4) NULL, TAX3 decimal (19,4) NULL,
    TAX2        decimal (19,4) NULL,    TAX1 decimal (19,4) NULL, TAX0 decimal (19,4) NULL,

    Document_Ref varchar(25) NULL,
    insurance_Ref varchar(30) NULL,
    insurance_holder_shortname varchar(30) NULL,SourceID	int NULL
)
insert into #tempPerfYear2
select CurrentYear,
       AgentCode,
        AgentName,
        AgentCnt,
        Product,
        ClassofBusID,
        'GrossPremium'=SUM(isnull(GrossPremium,0)),
        'Commission'=SUM(isnull(Commission,0)),
        PPYear,
        TransactionType,
        'ClaimsPaid'=SUM(isnull(ClaimsPaid,0)),
        'IncurredClaims'=SUM(isnull(IncurredClaims,0)),
        PolicyStart,
        PolicyEnd,
        MidnightRenewal,
        Yearcol,
        'GP5'=sum(isnull(GP5,0)),  'GP4'=sum(isnull(GP4,0)),  'GP3'=sum(isnull(GP3,0)),  'GP2'=sum(isnull(GP2,0)),  'GP1'=sum(isnull(GP1,0)),  'GP0'=sum(isnull(GP0,0)),  
        'COM5'=sum(isnull(COM5,0)),  'COM4'=sum(isnull(COM4,0)),  'COM3'=sum(isnull(COM3,0)),  'COM2'=sum(isnull(COM2,0)),  'COM1'=sum(isnull(COM1,0)),  'COM0'=sum(isnull(COM0,0)),  
        'PAY5'=sum(isnull(PAY5,0)),  'PAY4'=sum(isnull(PAY4,0)),  'PAY3'=sum(isnull(PAY3,0)),  'PAY2'=sum(isnull(PAY2,0)),  'PAY1'=sum(isnull(PAY1,0)),  'PAY0'=sum(isnull(PAY0,0)),
        'OSL5'=sum(isnull(OSL5,0)),  'OSL4'=sum(isnull(OSL4,0)),  'OSL3'=sum(isnull(OSL3,0)),  'OSL2'=sum(isnull(OSL2,0)),  'OSL1'=sum(isnull(OSL1,0)),  'OSL0'=sum(isnull(OSL0,0)),
        'TAX5'=sum(isnull(TAX5,0)),  'TAX4'=sum(isnull(TAX4,0)),  'TAX3'=sum(isnull(TAX3,0)),  'TAX2'=sum(isnull(TAX2,0)),  'TAX1'=sum(isnull(TAX1,0)),  'TAX0'=sum(isnull(TAX0,0)),
        document_Ref,
        insurance_ref,
        insurance_holder_Shortname,SourceID

from #tempAgtPerfYear

--where isnull(yearcol,99) in (0, 1, 2, 3, 4, 5)
group by CurrentYear, AgentCode, AgentName, AgentCnt, Product, PPYear, YearCol,
        ClassOfBusID, TransactionType, PolicyStart, PolicyEnd,
        MidnightRenewal, document_ref, insurance_Ref, insurance_holder_Shortname,SourceID




--RC-- 12 Jun 2006
IF LOWER(@AgentGroupCode) = 'all'
	BEGIN
		select *,S.Code CompanyCode,S.description CompanyDesc,
		Case @TypeOfCurrency
			WHEN 'System' THEN  @Systemcurrencycode
			WHEN 'Base' THEN C.Code
		END CurrencyCode,
		Case @TypeOfCurrency
			WHEN 'System' THEN @SystemCurrencyDesc
			WHEN  'Base' THEN C.description
		END CurrencyDesc,
		Case @GroupbyCode
			WHEN 'Branch' THEN S.Code
			WHEN 'Branch And Currency' THEN S.Code
		ELSE ''
		END 'GroupByCode'
		from #tempPerfYear2 TY
		INNER JOIN SOurce S ON S.source_id = TY.sourceid
		INNER JOIN CURRENCY C ON C.Currency_id= S.base_currency_id
		
		where isnull(yearcol,99) in (0, 1, 2, 3, 4, 5)

		order by AgentCode, product, PPYear, ClassofBusId, PolicyStart, insurance_holder_shortname

	END
ELSE
	BEGIN
		select *,S.Code CompanyCode,S.description CompanyDesc,
		Case @TypeOfCurrency
			WHEN 'System' THEN  @Systemcurrencycode
			WHEN 'Base' THEN C.Code
		END CurrencyCode,
		Case @TypeOfCurrency
			WHEN 'System' THEN @SystemCurrencyDesc
			WHEN  'Base' THEN C.description
		END CurrencyDesc,
		Case @GroupbyCode
			WHEN 'Branch' THEN S.Code
			WHEN 'Branch And Currency' THEN S.Code
		ELSE ''
		END 'GroupByCode'
		from #tempPerfYear2 TY
		INNER JOIN SOurce S ON S.source_id = TY.sourceid
		INNER JOIN CURRENCY C ON C.Currency_id= S.base_currency_id
		
		where isnull(yearcol,99) in (0, 1, 2, 3, 4, 5)

		and agentcnt IN(
		select party_cnt from party_agent where linked_account_group = (
		select  party_cnt from party where shortname = @AgentGroupCode) )

		order by AgentCode, product, PPYear, ClassofBusId, PolicyStart, insurance_holder_shortname
	END
--RC-- 12 Jun 2006


drop    table   #tempAgtPerfYear
drop    table   #tempPerfYear2
drop    table   #ClaimReserve
drop    table   #ClaimRecovery
drop    table   #IncurredClaims


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

