SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  OFF
GO
EXECUTE DDLDropProcedure 'spu_Report_Claims_Cat_Gross_To_Net_SFU'
GO
/**********************************************************************************************************************************
** Created by Jude Killip
** 21/12/2001
** Reports - Claims_by_Catastrophe_Gross_To_Net.rpt
**
**********************************************************************************************************************************
** DATE        	WHO	DESC
**
** 13/10/2003	JMK	Add Branch parameter
**					Amend current period selection
** 27/08/2004	JT	Multicurrency Changes
***********************************************************************************************************************************/
/*
    Claim status id constants
    1 = Provisional Open Claim
    2 = Live Open Claim
    3 = Closed
    4 = ReOpen
    5 = ReClosed

*/
CREATE PROCEDURE spu_Report_Claims_Cat_Gross_To_Net_SFU
				@branch_id int,
				@PeriodDate varchar (20),
                @Basis varchar(50),
                @CatastropheCode varchar (60),
                @IncludeClosed varchar (5),
                @TypeOfCurrency	Varchar(30),
                @GroupByCode	Varchar(30)
				
AS



SET NOCOUNT ON

/*
--for testing
DECLARE			@branch_id int,
				@PeriodDate varchar (20),
                @Basis varchar(50),
                @CatastropheCode varchar (60),
                @IncludeClosed varchar (5)
SELECT @branch_id = 0
SELECT @CatastropheCode = 'all', @IncludeClosed = 'yes'
SELECT @PeriodDate = 'Jan 31 2002', @Basis = 'Transaction Period'
*/

DECLARE @period_end_date    datetime
SELECT @PeriodDate = @PeriodDate + ' 23:59:59'
SELECT @period_end_date = CONVERT (Datetime, @PeriodDate)

DECLARE @dtSelectedPeriodEnd datetime,  @dtPrevPeriodEnd datetime
DECLARE @SelectedPeriodID int
DECLARE @iBranchPeriod int
-- Always use Branch 1 period table
-- to be amended if anyone sets up different periods for different branches
SELECT @iBranchPeriod = 1

-- Selected period values
SELECT @dtSelectedPeriodEnd = max(period_end_date)
FROM period
WHERE period_end_date <= @period_end_date
AND company_id = @iBranchPeriod

SELECT @SelectedPeriodID = period_id 
FROM period
WHERE period_end_Date  = @dtSelectedPeriodEnd
AND company_id = @iBranchPeriod

-- Previous period values
SELECT @dtPrevPeriodEnd = max(period_end_date)
FROM Period
WHERE period_end_date < @period_end_date
AND company_id = @iBranchPeriod

-- *If current period is the first period set up
-- (no need for exact date)
IF @dtPrevPeriodEnd IS NULL
	SELECT  @dtPrevPeriodEnd = dateadd(month, -1, @dtSelectedPeriodEnd)


-- Get IDs of all outstanding claims
CREATE TABLE #tempClmCatGrossNet1
(
    ClaimID int,
    CatastropheID int
)

DECLARE @SelectedCatID int
IF @CatastropheCode = 'ALL'
    SELECT @SelectedCatID = 0
ELSE
    SELECT @SelectedCatID = catastrophe_code_id FROM catastrophe_code WHERE description = @CatastropheCode


INSERT INTO #tempClmCatGrossNet1
	SELECT * FROM
		(
		SELECT claim_id,
			   (select TOP 1 catastrophe_code_id from 
		claim  where base_claim_id=
		(SELECT base_claim_id FROM claim c WHERE c.claim_id=cl.claim_id)
		AND 
		version_id=(select max(version_id) from claim c where c.is_dirty <> 1 and base_claim_id =
					(select base_claim_id from claim c where c.claim_id=cl.claim_id))) catastrophe_code_id  
		FROM claim CL
		WHERE 
		   (   (claim_status_id in (2, 4) AND @IncludeClosed = 'no')
			OR (claim_status_id <> 1 AND @IncludeClosed = 'yes')
			)
	 AND
		 isnull(catastrophe_code_id,0)<>  case WHEN 
  				(select count(version_id) from claim 
				 where base_claim_id=(select base_claim_id from 
				 claim c where c.claim_id=cl.claim_id and c.is_dirty <> 1) and isnull(catastrophe_code_id ,0)<>0)=0 Then 0			
				  else -1
				END
	AND cl.is_dirty <> 1
	) ClaimWithCatastrophe
	WHERE  (@SelectedCatID = ClaimWithCatastrophe.catastrophe_code_id)
		OR (isnull(@SelectedCatID,0) = 0)

CREATE TABLE #tempClmCatGrossNet
(
    ClaimNum varchar (30) NULL,
    CatastropheCode varchar (50) NULL,
    ProductDescription  varchar(255) null,
    RiskDescription     varchar(255) null,
    PolNum varchar (30) NULL,
    Client varchar (20) NULL,
    DocRef varchar (25) NULL,
    Agency varchar (20) NULL,
    Gross decimal (19,4) NULL,
    Coinsurance decimal (19,4) NULL,
    Treaty decimal (19,4) NULL,
    Facultative decimal (19,4) NULL,
    Retained decimal (19,4) NULL,
    TransDate datetime NULL,
    dtSelectedPeriod datetime,
    CompanyCode	Varchar(30)	Null,
    CompanyDesc	Varchar(255) Null,
    CurrencyCode	Varchar(30) NULL,
    CurrencyDesc	Varchar(255) NULL,
    GroupByCode	Varchar(255) NULL
    
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

INSERT INTO #tempClmCatGrossNet
    SELECT sf.loss_code,
        (SELECT description FROM catastrophe_code WHERE catastrophe_code_id = T1.CatastropheID),
        (SELECT description FROM product WHERE product_id = sf.product_id),
        (SELECT description FROM risk_type WHERE risk_type_id = sd.risk_type_id),
        sf.insurance_ref,
        sf.insurance_holder_shortname,
        sf.document_ref,
        isnull(sf.agent_shortname, ' Direct'),
        (SELECT 
        Case @TypeOfCurrency WHEN 'Base' THEN sd.this_premium_home 
        	WHEN 'system' THEN sd.this_premium_system
        	WHEN 'Transaction' THEN sd.this_premium_original
        END WHERE sd.stats_detail_type = 'GRS'),
        (SELECT 
        	CASE @TypeOfCurrency WHEN 'Base' THEN sd.this_premium_home 
        		WHEN 'system' THEN sd.this_premium_system
        		WHEN 'Transaction' THEN sd.this_premium_original
        	END
        WHERE sd.stats_detail_type = 'COI'),
        (SELECT 
        	Case @TypeOfCurrency WHEN 'Base' THEN sd.this_premium_home  
        		WHEN 'system' THEN sd.this_premium_system  
        		WHEN 'Transaction' THEN sd.this_premium_original 
        	END
        WHERE sd.stats_detail_type = 'TTY'),
        
        (SELECT 
        Case @TypeOfCurrency 
        	WHEN 'Base' THEN  sd.this_premium_home
        	WHEN 'system' THEN sd.this_premium_system
        	WHEN 'Transaction' THEN sd.this_premium_original
        END    WHERE sd.stats_detail_type = 'FAC'),
        
        (SELECT 
        Case @TypeOfCurrency 
        	WHEN 'Base' THEN  sd.this_premium_home
        	WHEN 'system' THEN sd.this_premium_system
        	WHEN 'Transaction' THEN sd.this_premium_original
        END WHERE sd.stats_detail_type = 'NET'),
        sf.transaction_date,
        @dtSelectedPeriodEnd,
        s.code CompanyCode,s.description CompanyDesc,
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
    FROM Stats_Folder sf
    JOIN #tempClmCatGrossNet1 T1 	ON ClaimID = sf.loss_id
    JOIN Stats_Detail sd    		ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND ((isnull(sd.this_premium_home,0) <> 0 AND @typeofcurrency = 'Base') 
        	OR (isnull(sd.this_premium_system,0) <> 0 AND @typeofcurrency = 'System')
        	OR (isnull(sd.this_premium_original,0) <> 0 AND @typeofcurrency = 'Transaction'))
    JOIN Product p          		ON sf.product_id = p.product_id
    JOIN Risk_Type rt       		ON sd.risk_type_id = rt.risk_type_id
    JOIN Source S					ON S.source_id = sf.Source_id
    JOIN Currency CB				ON CB.currency_id =s.base_currency_id
    JOIN Currency ct /*Transaction Currency*/
									ON ct.iso_code = sd.currency_Code	
    WHERE sf.transaction_type_code IN ('C_CO', 'C_CR')
    AND (
			(@Basis = 'Transaction Period' AND sf.posting_period_number <= @SelectedPeriodID)
			OR
			(@Basis = 'Transaction Date' AND sf.transaction_date <= @dtSelectedPeriodEnd)
		)
	AND (@branch_id= 0 OR (@branch_id <> 0 AND sf.Branch_id = @branch_id)
		)

SET NOCOUNT OFF

SELECT 	ClaimNum,
		CatastropheCode,
		ProductDescription,
		RiskDescription,
		PolNum,
		Client,
		DocRef,
		Agency,
		Gross,
		Coinsurance,
		Treaty,
		Facultative,
		Retained,
		TransDate,
		dtSelectedPeriod,CompanyCode,CompanyDesc,CurrencyCode,CurrencyDesc,GroupByCode
FROM #tempClmCatGrossNet 

DROP TABLE #tempClmCatGrossNet1
DROP TABLE #tempClmCatGrossNet

GO

SET QUOTED_IDENTIFIER  OFF    
SET ANSI_NULLS  ON
GO

