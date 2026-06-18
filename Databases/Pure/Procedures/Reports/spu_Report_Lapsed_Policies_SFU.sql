SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


EXECUTE DDLDropProcedure 'spu_Report_Lapsed_Policies_SFU'
GO

/**********************************************************************************************************************************
** Created by Kerry Butler
** 20/9/01
**
** NAME:        spu_Report_Lapses_In_Month_SFU
**
** PARAMETERS:  @start_date
**              @end_date
**
** 
** DESCRIPTION: Created for AUA
**              Reports Policies lapsed within a selected date range.
**
**1.0	JT 		Multicurrency changes
**
**1.1   RC      14Jun2006	Filter by Agent Group
**********************************************************************************************************************************
**
***********************************************************************************************************************************/ 
CREATE PROCEDURE spu_Report_Lapsed_Policies_SFU
	@start_date 	datetime,
	@end_date 	datetime,
	@agent_code	varchar(20),
	@TypeOfCurrency	Varchar(30),
	@GroupByCode	Varchar(30),
	@AgentGroupCode Varchar(30)
AS
SET NOCOUNT ON
Declare @system_base_xrate FLOAT,@system_base_date DATETIME
Declare @currency_base_xrate FLOAT,	@currency_base_date DATETIME
Declare @TypeOfRates Int
DECLARE @sAgentCode	varchar(20),
	@iAgentId	int

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

IF @agent_code = 'ALL' BEGIN
	SELECT @iAgentId = NULL
END	
ELSE BEGIN
	SELECT @iAgentId = party_cnt
		FROM party
		WHERE shortname = @agent_code
END
SELECT @iAgentId = ISNULL(@iAgentId, 0)
Declare @Branch Int
EXEC spu_ACT_GetTypeOfRates @TypeOfRates OUTPUT 
IF @TypeOfRates =1 
	SELECT @branch=1 
ELSE 
	SELECT @branch=NULL 

                         
CREATE TABLE #tmpLapsesInMonth
(
	AgentName		varchar(255) 	NULL,
	ClassOfBusiness  	varchar(255) 	NULL,
	PolicyNo		varchar(30) 	NULL,
   	Insured			varchar(387) 	NULL,
	LapseDate		datetime  	NULL,
	GrossPremium		decimal(19,4) 	NULL,
  	Reason_for_lapse	varchar(255) 	NULL,
  	CurrencyCode		Varchar(30)	NULL,
  	CurrencyDesc		Varchar(255)	NULL,
  	CompanyCode			Varchar(30)	NULL,
  	CompanyDesc			Varchar(255)	NULL,
  	GroupByCode			Varchar(30)	NULL
)


INSERT INTO #tmpLapsesInMonth
	select 	pa.trading_name, 
		p.description, 
		ifi.insurance_ref, 
		py.resolved_name, 
		ifi.lapsed_date, 
		CASE @typeofcurrency
			WHEN 'Base' THEN ifi.this_premium * 
			ISNULL(IFI.currency_base_xrate,CR.rate_against_base)
			WHEN 'System' THEN (ifi.this_premium * 
			ISNULL(IFI.currency_base_xrate,CR.rate_against_base)) / ISNULL(IFI.system_base_xrate,CR.rate_against_base)
		END this_premium,
		ifi.lapsed_description,
		Case @TypeOfcurrency 
			WHEN 'Base' THEN CB.CODE
			WHEN 'System' THEN @SystemCurrencyCode
		END,
		Case @TypeOfcurrency 
			WHEN 'Base' THEN CB.DESCRIPTION
			WHEN 'System' THEN @SystemCurrencyDesc
		END,
		S.Code,S.Description,
		Case @GroupByCode 
			WHEN 'Branch' THEN S.CODE
			WHEN 'Branch And Currency' THEN S.Code
		Else ' '
		END
       	from insurance_file ifi 
		join product  p 
			on ifi.product_id = p.product_id
		join party_agent pa 
			on lead_agent_cnt = pa.party_cnt
		join insurance_folder ifo 
			on ifo.insurance_folder_cnt = ifi.insurance_folder_cnt
		join party py 	
			on  ifo.insurance_holder_cnt = py.party_cnt
		 JOIN currencyrate CR
			ON CR.currency_id = IFI.currency_id
			AND  CR.company_id = ISNULL(@branch,IFI.source_id)
		JOIN SOURCE S ON S.Source_id= IFI.Source_id
		JOIN CURRENCY CB ON CB.Currency_id = S.base_currency_id
		
		WHERE lapsed_date IS NOT NULL and (lapsed_date BETWEEN @Start_date and @End_Date)
		and (@iAgentId = 0 or
			(@iAgentId <> 0 and @iAgentId = pa.party_cnt))
		AND CR.effective_from IN
		(
			SELECT MAX(effective_from)
				FROM CurrencyRate 
				WHERE effective_from <= IFI.cover_start_date
				AND   currency_id = CR.currency_id
				AND company_id = CR.company_id
		)
AND ifi.cover_start_date = (select max(cover_start_date) from insurance_file ifl where ifl.insurance_folder_cnt = ifi.insurance_folder_cnt )
AND ifi.insurance_file_cnt  = (select max(insurance_file_cnt) from insurance_file ifl where ifl.insurance_folder_cnt = ifi.insurance_folder_cnt )
AND ifi.insurance_file_status_id in(1, 2, 308)

SET NOCOUNT OFF

IF LOWER(@AgentGroupCode) = 'all'
BEGIN

	PRINT 'ENTER1'
	
	Select * FROM #tmpLapsesInMonth
END

IF LOWER(@AgentGroupCode) <> 'all'
BEGIN

	PRINT 'ENTER2'
	
	Select * FROM #tmpLapsesInMonth
    --RC-- 14 Jun 2006
    WHERE AgentName IN(
    select trading_name from party_agent where linked_account_group = (
    select  party_cnt from party where shortname = @AgentGroupCode) )
    --RC-- 14 Jun 2006

END

DROP TABLE #tmpLapsesInMonth

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO
