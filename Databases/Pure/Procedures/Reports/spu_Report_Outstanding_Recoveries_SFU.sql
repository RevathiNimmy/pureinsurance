SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

EXECUTE DDLDropProcedure 'spu_Report_Outstanding_Recoveries_SFU'
GO



/**********************************************************************************************************************************
** Created by Kerry Butler
** 14/11/2001
** AUA Bespoke Reports - Outstanding_Recoveries_Report
** Changed By Jitendra
**	24/08/2004
**	MultiCurrency Changes 
**********************************************************************************************************************************
**
***********************************************************************************************************************************/
CREATE PROCEDURE spu_Report_Outstanding_Recoveries_SFU
        @Treaty     varchar (10),
        @TypeOfCurrency Varchar(30),
        @GroupByCode	Varchar(30)
        
AS
SET NOCOUNT ON
CREATE TABLE #tempOS_Recoveries
(
    Treaty      varchar(255)    NULL,
    ClaimNo     varchar(30) NULL,
    ClientName  varchar(60) NULL,
    LossDate    datetime    NULL,
    Causation   varchar(50) NULL,
    Catastrophe varchar(50) NULL,
    LossDesc    varchar(255)    NULL,
    PaidToDate  decimal(19,4)   NULL,
    OS_Recovery decimal(19,4)   NULL,
    UWType      char(1)     NULL,
    SourceID	int	NULL
    )

-- Decide if underwriting or Agency
DECLARE  @UWType char(1)
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
Declare @TypeOfRates Int
Declare @branch Int
EXEC spu_ACT_GetTypeOfRates @TypeOfRates OUTPUT 
IF @TypeOfRates =1 
	SELECT @branch=1 
ELSE 
	BEGIN
		SELECT @branch=NULL 
	END
SELECT @UWtype = UW_type FROM hidden_options

IF isnull(@Treaty,'') = ''
    SELECT @Treaty = 'ALL'

IF @Treaty = 'ALL'
BEGIN
INSERT INTO #tempOS_Recoveries
-- PW090402 - make select distinct to avoid results being multiplied by nth degree
SELECT DISTINCT
    sd.ri_shortname,
    sf.loss_code,
    insurance_holder_shortname,
    loss_date,
    pc.description,
    cc.description,
    LEFT(c.description,255),
    Case @TypeOfCurrency 
		WHEN 'Base' THEN (isnull(r.received_to_date,0)*
			ISNULL(c.currency_base_xrate,CR.rate_against_base))
		WHEN 'System' THEN (isnull(r.received_to_date,0)*
			ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))
	END received_to_date,
 --   (initial_reserve + revised_reserve - received_to_date),
		(Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN (isnull(r.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
		END) +
	(	Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.Revised_reserve,0)*
			ISNULL(c.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN  (isnull(r.Revised_reserve,0)*
			ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))
		END) -
		(Case @TypeOfCurrency 
			WHEN 'Base' THEN (isnull(r.received_to_date,0)*
				ISNULL(c.currency_base_xrate,CR.rate_against_base))
			WHEN 'System' THEN (isnull(r.received_to_date,0)*
				ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))
		END),
    @UWType,ifi.Source_id
FROM Stats_Folder sf

JOIN
        stats_detail sd ON sd.stats_folder_cnt = sf.stats_folder_cnt
JOIN
        claim c ON c.claim_number = sf.loss_code
JOIN
        primary_cause pc ON pc.primary_cause_id = c.primary_cause_id
LEFT OUTER JOIN
    catastrophe_code cc ON cc.catastrophe_code_id = c.catastrophe_code_id
JOIN
    claim_peril cp ON cp.claim_id = c.claim_id
JOIN
    recovery r ON r.claim_peril_id = cp.claim_peril_id

JOIN insurance_file ifi     	ON ifi.insurance_file_cnt  = C.policy_id
JOIN currencyrate CR		 	ON CR.currency_id = C.currency_id AND CR.company_id = ISNULL(@branch,IFI.source_id)

WHERE   (transaction_type_code LIKE 'C_S%' OR
        transaction_type_code LIKE 'C_R%')
    AND sd.stats_detail_type = 'TTY'  -- do we need COI?

END
ELSE
BEGIN
INSERT INTO #tempOS_Recoveries
-- PW090402 - make select distinct to avoid results being multiplied by nth degree
SELECT DISTINCT
    sd.ri_shortname,
    sf.loss_code,
        insurance_holder_shortname,
    loss_date,
    pc.description,
    cc.description,
    c.description,
   Case @TypeOfCurrency 
   		WHEN 'Base' THEN (isnull(r.received_to_date,0)*
   			ISNULL(c.currency_base_xrate,CR.rate_against_base))
   		WHEN 'System' THEN (isnull(r.received_to_date,0)*
   			ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))
   	END received_to_date,
    --   (initial_reserve + revised_reserve - received_to_date),
   		(Case @TypeOfCurrency 
   			WHEN 'Base' THEN (isnull(r.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base))
   			WHEN 'System' THEN (isnull(r.Initial_reserve,0)*ISNULL(C.currency_base_xrate,CR.rate_against_base)/ISNULL(C.system_base_xrate,CR.rate_against_base))
   		END) +
   	(	Case @TypeOfCurrency 
   			WHEN 'Base' THEN (isnull(r.Revised_reserve,0)*
   			ISNULL(c.currency_base_xrate,CR.rate_against_base))
   			WHEN 'System' THEN  (isnull(r.Revised_reserve,0)*
   			ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))
   		END) -
   		(Case @TypeOfCurrency 
   			WHEN 'Base' THEN (isnull(r.received_to_date,0)*
   				ISNULL(c.currency_base_xrate,CR.rate_against_base))
   			WHEN 'System' THEN (isnull(r.received_to_date,0)*
   				ISNULL(c.currency_base_xrate,CR.rate_against_base) / ISNULL(c.system_base_xrate,CR.rate_against_base))
		END),
    @UWType,ifi.source_id

FROM Stats_Folder sf

JOIN
        stats_detail sd ON sd.stats_folder_cnt = sf.stats_folder_cnt
JOIN
        claim c ON c.claim_number = sf.loss_code
JOIN
        primary_cause pc ON pc.primary_cause_id = c.primary_cause_id
LEFT OUTER JOIN
    catastrophe_code cc ON cc.catastrophe_code_id = c.catastrophe_code_id
JOIN
    claim_peril cp ON cp.claim_id = c.claim_id
JOIN
    recovery r ON r.claim_peril_id = cp.claim_peril_id
JOIN insurance_file ifi     	ON ifi.insurance_file_cnt  = C.policy_id
JOIN currencyrate CR		 	ON CR.currency_id = C.currency_id AND CR.company_id = ISNULL(@branch,IFI.source_id)


WHERE   (transaction_type_code LIKE 'C_S%' OR
        transaction_type_code LIKE 'C_R%')
    AND sd.stats_detail_type = 'TTY'  -- do we need COI?
    AND sd.ri_shortname = @Treaty
END

SET NOCOUNT OFF

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
		END CurrencyDesc,
		Case @GroupbyCode 
			WHEN 'Branch' THEN S.Code
			WHEN 'Branch And Currency' THEN S.Code
		ELSE ''
		END 'GroupByCode'	
    FROM #tempOS_Recoveries TS
	INNER JOIN Source S ON S.source_id = TS.SourceId
	INNER JOIN CURRENCY C ON C.Currency_id= S.base_currency_id

DROP TABLE  #tempOS_Recoveries



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

