SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  OFF 
GO

EXECUTE DDLDropProcedure 'spu_Report_Premium_Bordereau_SFU'
GO

/**********************************************************************************************************************************
** Created by Kerry Butler
** 03/09/2001
** Agency Reports - Premium_Bordereau.rpt
**
**    v1.1 KB 2/1/2 Added variables for IPT and VAT. Added selection criteria for IPT and VAT
**     1.2 KB 10/01/02  Dont include tax records when calculating the premium amount.
**          Update tax records with treaty so they get anaysed properly.
**     1.3 PW110402 - use period instead of start/end date
**     1.4 JMK 22-Oct-02    - take out IF ELSE method of dealing with @Reinsurer parameter, to avoid duplicate chunks of SQL
**                              (and it makes selection of "ALL" faster)
**                          - Remove EXP code, @currentPeriod and redundant stuff
**                          - Add stats_folder_cnt for grouping stats

**		1.5	JT	17/08/2004	JT- MultiCurrency Changes
**********************************************************************************************************************************
**
***********************************************************************************************************************************/
CREATE PROCEDURE spu_Report_Premium_Bordereau_SFU
        @Reinsurer  varchar (100),
        @PeriodDate varchar (24),
        @TypeOfCurrency	Varchar(30),
        @GroupByCode	Varchar(30)
AS

SET NOCOUNT ON
/*
declare @Reinsurer  varchar (100),
        @PeriodDate varchar (20)
select @Reinsurer = 'HSBC Insurance',
    @PeriodDate = '31 Aug 2002'
*/
-- PW110402 - find which period we want to base this report on
DECLARE @SelectedPeriodID int, @dtSelectedPeriodEnd datetime

SELECT @PeriodDate = @PeriodDate + ' 23:59:59.000'
SELECT @dtSelectedPeriodEnd = CONVERT (Datetime, @PeriodDate)

SELECT @SelectedPeriodID = period_id
FROM Period
WHERE period_end_date = @dtSelectedPeriodEnd


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


CREATE TABLE #tempPremiumBordereau
(
        PolNum varchar (30) NULL,           -- 2
        Client varchar (60) NULL,           -- 4
        TransTypeID int NULL,               -- 5
        TransTypeCode varchar (10) NULL,    -- 6
        ProductCode varchar (10) NULL,      -- 7
        Product varchar (255) NULL,         -- 9
        RiskTypeID int NULL,                -- 11
        ReinsurerCnt int NULL,              -- 12
        ReinsurerShort varchar (20) NULL,   -- 13
        Reinsurer varchar (100) NULL,       -- 14
        AmountPremium decimal (19,4) NULL,  -- 15
        IPT decimal (19,4)  NULL,
        VAT decimal (19,4)  NULL,
        RiskType varchar (255) NULL,           -- 18
        FromDate    datetime NULL,
        ToDate datetime NULL,
        Stats_detail_type varchar(3) NULL ,       -- 19
        UWType char(1) NULL,
        TransDate datetime,
        StatsFolder int, 
        CompanyCode	Varchar(30) NULL,
        CompanyDesc	Varchar(255)	NULL,
        CurrencyCode	Varchar(30) NULL,
        CurrencyDesc	Varchar(255)	NULL
        )

-- Decide if underwriting or Agency
DECLARE @iLedgerID int, @UWType char(1)

SELECT @UWtype = UW_type FROM hidden_options

IF isnull(@Reinsurer,'') = ''
    SELECT @Reinsurer = 'ALL'

INSERT INTO #tempPremiumBordereau
    SELECT
        sf.insurance_ref,         -- 2
        sf.insurance_holder_name, -- 4
        sf.transaction_type_id,   -- 5
        sf.transaction_type_code, -- 6
        sf.product_code,          -- 7
        p.description,            -- 9
        sd.risk_type_id,          -- 11
        sd.ri_party_cnt,          -- 12
        sd.ri_shortname,          -- 13
        NULL,                 	  -- 14
        (SELECT 
        Case @TypeOfCurrency 
        	WHEN 'Base' THEN sd.this_premium_home 
        	WHEN 'System' THEN	sd.this_premium_system
		WHEN 'Account' THEN	(SELECT MIN(T.account_base_xrate)
						FROM TransDetail T
						JOIN Document D ON d.document_id=t.document_id
						WHERE D.insurance_file_cnt = i.insurance_file_cnt
						AND T.account_id = a.account_id)* sd.this_premium_home 
        END   WHERE sd.stats_detail_type in ('TTY', 'COI')),      -- 15
        (SELECT sd.tax_value WHERE sd.stats_detail_type = 'TAT' and sd.tax_type_code = 'IPT'),
        (SELECT sd.tax_value WHERE sd.stats_detail_type = 'TAT' and sd.tax_type_code = 'VAT'),
        rt.description,         -- 18
        sf.cover_start_date,
        sf.expiry_date,
        sd.stats_detail_type,
        @UWType,
        sf.document_date,
        sf.stats_folder_cnt,S.Code,S.Description,
    	Case @TypeOfCurrency 
	    	WHEN 'Base' THEN CB.Code
    		WHEN 'System' THEN @SystemcurrencyCode
		WHEN 'Account' THEN ca.Code

    	END CurrencyCode,
    	Case @TypeOfCurrency 
		WHEN 'Base' THEN CB.description
		WHEN 'System' THEN @SystemcurrencyDesc
		WHEN 'Account' THEN ca.description

    	END CurrencyCode
    	
    FROM Stats_Folder sf
    JOIN Stats_Detail sd 	 ON sf.stats_folder_cnt = sd.stats_folder_cnt
    JOIN SOURCE S 		 ON S.source_id = sf.source_id
    JOIN Currency CB 		 ON CB.Currency_id= s.base_currency_id
    JOIN Insurance_File i	 ON i.insurance_file_cnt=sf.insurance_file_cnt
    JOIN Account a		 ON a.account_key=i.insured_cnt
    JOIN Currency ca          	 ON ca.currency_id =a.currency_id 
    LEFT OUTER JOIN Risk_Type rt ON sd.risk_type_id = rt.risk_type_id
    LEFT OUTER JOIN Product p    ON sf.product_id = p.product_id

    WHERE sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims
    AND (stats_detail_type IN ('TTY', 'COI', 'TAT'))
    -- PW110402 - use period instead of start/end date
    AND (sf.posting_period_number = @SelectedPeriodID)
    AND (
            (sd.ri_party_cnt IN
                    (SELECT party_cnt
                    FROM Party
                    WHERE resolved_name LIKE (@Reinsurer))               -- allow for wildcard searches
            OR
            @Reinsurer = 'ALL'
         )
    AND ((isnull(sd.this_premium_home,0) <> 0 AND @TypeOfCurrency = 'Base')     OR 
    	 (isnull(sd.this_premium_system,0) <> 0 AND @TypeOfCurrency = 'System') OR
	 (isnull(sd.this_premium_home,0) <> 0 AND @TypeOfCurrency = 'Account'))
    	)
        OR
        ((isnull(sd.lead_commission_value_home,0) +
          isnull(sd.sub_commission_value_home,0) <> 0 And @TypeOfCurrency='Base') 
        OR 
        (isnull(sd.lead_commission_value_system,0) +
         isnull(sd.sub_commission_value_system,0) <> 0 And @TypeOfCurrency='System')
        OR 
        (isnull(sd.lead_commission_value_home,0) +
         isnull(sd.sub_commission_value_home,0) <> 0 And @TypeOfCurrency='Account'))

-- Update with appropriate details for treaty/coinsurance
UPDATE #tempPremiumBordereau
SET Reinsurer = t.description
FROM treaty t
WHERE  ReinsurerShort = t.code  and stats_detail_type IN ('TTY','TAT')

UPDATE #tempPremiumBordereau
SET Reinsurer = py.name
FROM party py
WHERE  ReinsurerShort = py.shortname  and stats_detail_type = 'COI'

SET NOCOUNT OFF

if @Reinsurer ='ALL'
  
	SELECT *,  
	Case @GroupByCode   
	 WHEN 'Branch' THEN CompanyCode  
	 WHEN 'Branch And Company' THEN CompanyCode  
	 ELSE ''   
	END 'GroupByCode'  
	FROM #tempPremiumBordereau where AmountPremium is not null  

ELSE

	SELECT *,  
	Case @GroupByCode   
	 WHEN 'Branch' THEN CompanyCode  
	 WHEN 'Branch And Company' THEN CompanyCode  
	 ELSE ''   
	END 'GroupByCode'  
	FROM #tempPremiumBordereau where AmountPremium is not null  and ReinsurerShort LIKE (@Reinsurer)



DROP TABLE  #tempPremiumBordereau


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

