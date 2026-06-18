SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


EXECUTE DDLDropProcedure 'spu_Report_TransProcessing_SFU'
GO

/**********************************************************************************************************************************
** Created by Kerry Butler
** 12/10/2001
**
** NAME:        spu_Report_TransProcessing_SFU
**		v1.1 	7/12/01 Pick up one record per policy for count
** 		v1.2	9/02/02 KB.  Use product rather than class of business but leave the variable name
**				unchanged to avoid changing the report.
** JT	v1.3	MultiCurrency Changes
**********************************************************************************************************************************
**
***********************************************************************************************************************************/

CREATE PROCEDURE spu_Report_TransProcessing_SFU
( 	@TypeOfCurrency	Varchar(30)
)

AS

DECLARE @CurrentPeriodID int
DECLARE @Current12PeriodID int
DECLARE @Current24PeriodID int

EXECUTE spu_Report_GetCurrentPeriod_SFU @CurrentPeriodID OUTPUT, NULL


EXECUTE spu_report_getcurrent12monthperiod_SFU @Current12PeriodID OUTPUT, NULL


EXECUTE spu_report_getcurrent24monthperiod_SFU @Current24PeriodID OUTPUT, NULL

/*print @CurrentPeriodID
print @Current12PeriodID
print @Current24PeriodID*/
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
CREATE TABLE #tempTransProc

(
	StatsFolder	int		NULL,
	BranchCode	int 		NULL,
	BranchDesc	varchar (255)	NULL,
	ClassofBusiness	int		NULL,
	ClassOfBusinessDesc	varchar	(255) NULL,
	TransType	varchar(20) 	NULL,
	PremiumNB	decimal (19,4)	NULL, 		-- New Business
	NBCount		int		NULL,
	PremiumNBR	decimal (19,4)	NULL,		-- New Business Return
	NBRCount	int		NULL,
	PremiumREN	decimal (19,4)	NULL,		-- Renewal
	RENCount	int		NULL,
	PremiumRENR	decimal (19,4)	NULL,		-- Renewal return
	RENRCount	int		NULL,
	AddPrem		decimal (19,4)	NULL,		-- AP
	APCount		int		NULL,
	RetPrem		decimal (19,4)	NULL,		-- RP
	RPCount		int		NULL,
	ClaimOpen	decimal (19,4)	NULL,		-- Claim open
	COCount		int		NULL,
	ClaimRevise	decimal (19,4)	NULL,		-- Claim revision
	CRCount		int		NULL,
	ClaimPaid	decimal (19,4)	NULL,		-- Claim payment
	CPCount		int		NULL,
	ClaimSalvage	decimal (19,4)	NULL,		-- Salvage
	CSCount		int		NULL,
	ClaimRecovery	decimal (19,4)	NULL,		-- Recovery
	CVCount		int		NULL,

	PeriodNo	int		NULL,
	PeriodYear	int		NULL,
	PeriodRange	int		NULL		-- 1 this period
							-- 2 this year to date (exc this period)
							-- 3 this period last year
							-- 4 last year cumulative 
)

INSERT INTO #tempTransProc

SELECT
	sf.stats_folder_cnt,
	branch_id, 
	NULL,
	class_of_business_id, 
 	--NULL,
 	p.description,		--sf.product_code,
	transaction_type_code,  
	(SELECT 
	Case @TypeOfCurrency WHEN 'Base' THEN this_premium_home 
		WHEN 'System' THEN this_premium_system
	END
	WHERE transaction_type_code = 'NB'), 
	
	(SELECT 1 WHERE transaction_type_code = 'NB' and stats_detail_id = 1), 
	
	(SELECT Case @TypeOfCurrency WHEN 'Base' THEN this_premium_home 
		WHEN 'System' THEN this_premium_system
	END
	WHERE transaction_type_code = 'NBR'),
	(SELECT 1 WHERE transaction_type_code = 'NBR'and stats_detail_id = 1),
	(SELECT Case @TypeOfCurrency WHEN 'Base' THEN this_premium_home 
		WHEN 'System' THEN this_premium_system
		END 
	WHERE transaction_type_code = 'REN'),
	(SELECT 1 WHERE transaction_type_code = 'REN'and stats_detail_id = 1),
	(SELECT Case @TypeOfCurrency WHEN 'Base' THEN this_premium_home 
		WHEN 'System' THEN this_premium_system
		END 
	WHERE transaction_type_code = 'RENR'),
	(SELECT 1 WHERE transaction_type_code = 'RENR'and stats_detail_id = 1),
	(SELECT Case @TypeOfCurrency WHEN 'Base' THEN this_premium_home 
		WHEN 'System' THEN this_premium_system
		END  
	WHERE transaction_type_code = 'MTA'),
	(SELECT 1 WHERE transaction_type_code = 'MTA'and stats_detail_id = 1),
	(SELECT Case @TypeOfCurrency WHEN 'Base' THEN this_premium_home 
		WHEN 'System' THEN this_premium_system
		END 
	WHERE transaction_type_code = 'MTC'),
	(SELECT 1 WHERE transaction_type_code = 'MTC'and stats_detail_id = 1),
	(SELECT Case @TypeOfCurrency WHEN 'Base' THEN this_premium_home 
		WHEN 'System' THEN this_premium_system
		END 
	WHERE transaction_type_code = 'C_CO'),
	(SELECT 1 WHERE transaction_type_code = 'C_CO'),
	(SELECT Case @TypeOfCurrency WHEN 'Base' THEN this_premium_home 
		WHEN 'System' THEN this_premium_system
		END 
	WHERE transaction_type_code = 'C_CR'),
	(SELECT 1 WHERE transaction_type_code = 'C_CR'),
	(SELECT Case @TypeOfCurrency WHEN 'Base' THEN this_premium_home 
		WHEN 'System' THEN this_premium_system
		END 
	WHERE transaction_type_code = 'C_CP'),
	(SELECT 1 WHERE transaction_type_code = 'C_CP'),
	(SELECT Case @TypeOfCurrency WHEN 'Base' THEN this_premium_home 
		WHEN 'System' THEN this_premium_system
		END 
	WHERE transaction_type_code = 'C_SA'),
	(SELECT 1 WHERE transaction_type_code = 'C_SA'),
	(SELECT Case @TypeOfCurrency WHEN 'Base' THEN this_premium_home 
		WHEN 'System' THEN this_premium_system
		END 
	WHERE transaction_type_code = 'C_RV'),
	(SELECT 1 WHERE transaction_type_code = 'C_RV'),
	posting_period_number, 
	posting_period_year, 
	NULL 
FROM stats_folder sf 
LEFT OUTER JOIN stats_detail sd ON sd.stats_folder_cnt = sf.stats_folder_cnt
JOIN Product p ON sf.product_code = p.code
WHERE  stats_detail_type = 'GRS'



UPDATE 
	#tempTransProc
	SET periodrange = 1 
	WHERE periodno = @CurrentPeriodID
  
UPDATE 
	#tempTransProc
  	SET periodrange = 2 
  	WHERE periodno BETWEEN @Current12PeriodID AND ( @CurrentPeriodID - 1)
  	
UPDATE 
	#tempTransProc
	SET periodrange = 3 
	WHERE periodno = @Current12PeriodID
	
UPDATE 
	#tempTransProc
	SET periodrange = 4 
	WHERE periodno BETWEEN @Current24PeriodID AND ( @Current12PeriodID - 1 )
	
	
-- KB 9/1/2 No longer needed as we are now using product.	
--UPDATE 
--	#tempTransProc
--	SET ClassofBusinessDesc = c.description
--	FROM class_of_business c
--	WHERE c.class_of_business_id = ClassofBusiness

UPDATE 
	#tempTransProc
	--SET BranchDesc = b.description
	--FROM branch b 
	--WHERE b.branch_id = BranchCode
	SET BranchDesc = s.code
	FROM source s
	WHERE s.source_id = BranchCode


SET NOCOUNT OFF

-- Squirt it all out to the report

SELECT *,
S.Code CompanyCode,
		S.description CompanyDesc,
		Case @TypeOfCurrency 
			WHEN 'System' THEN  @Systemcurrencycode
			WHEN 'Base' THEN C.Code
		END CurrencyCode,
		Case @TypeOfCurrency 
			WHEN 'System' THEN @SystemCurrencyDesc
			WHEN  'Base' THEN C.description
		END CurrencyDesc
    FROM #tempTransProc TP
	INNER JOIN Source S ON S.source_id = TP.branchcode
	INNER JOIN CURRENCY C ON C.Currency_id= S.base_currency_id
DROP TABLE #tempTransProc


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

