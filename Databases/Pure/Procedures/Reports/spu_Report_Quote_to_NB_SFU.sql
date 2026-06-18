SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


EXECUTE DDLDropProcedure 'spu_Report_Quote_to_NB_SFU'
GO

/**********************************************************************************************************************************
** Created by Kerry Butler
** 04/10/2001
** Agency Reports - Quotations and New Business Report
**      
**********************************************************************************************************************************
** Description:		Shows quotations for a specified period and year to date by agent and class of business
**                      Of these quotations to indicate how many have become new business policies
** 1.1			Populate Agent Name with Direct so that direct policies have a title. KB 29/11/01.
** 1.2                  ON data transfer the party_agent trading_name which we are using to pick up the agent name 
**                      has not been populated.                                                      
**                      So lets use the name field on party table.
**1.3		JT 	Multi currency changes
***********************************************************************************************************************************/
CREATE PROCEDURE spu_Report_Quote_to_NB_SFU
(	@start_date  	datetime,
	@end_date      	datetime,
	@agent_code	varchar(20),
	@TypeOfCurrency	Varchar(30),
	@GroupbyCode	Varchar(30)
	)
 
                        
AS


/*DECLARE @start_date  	datetime,
	@end_date     	datetime,
	@agent_code	varchar(20)
	
SELECT 	@start_date = '2004-06-01',
	@end_date = '2004-06-09',
	@agent_code = 'ALL'
*/
SET NOCOUNT ON
CREATE TABLE #tempQuoteNB
(
		InsFileCnt int NULL,
		AgentName varchar (255) NULL,
		AgentCnt  int NULL,
		ProductID int NULL,
		ProductName varchar (255) NULL,
		QuoteDate datetime NULL,
		QuoteCount int NULL,
		QuoteValue decimal (19,4) NULL,
		NBDate datetime NULL,
		NBCount int NULL,
		NBValue decimal (19,4) NULL,
		RecordType int NULL,
		CurrencyCode	Varchar(30) NULL,
		CurrencyDesc	Varchar(255) NULL,
		CompanyCode	Varchar(30) NULL,
		CompnayDesc	Varchar(255) NULL,
		GroupByCode	Varchar(255)NULL
)

DECLARE 
	@dStartDate	datetime,
	@dEndDate       datetime,
	@dFinanceYrStrt	datetime,
	@sAgentCode	varchar (20),
	@iAgentCnt	int
	
IF @start_date IS NULL OR @start_date = ''
        SELECT @dStartDate = GETDATE()
ELSE
        SELECT @dStartDate = @start_date
        
IF @end_date IS NULL OR @end_date = ''
        SELECT @dEndDate = GETDATE()
ELSE
        SELECT @dEndDate = @end_date
IF @agent_code = 'ALL' BEGIN
	SELECT @agent_code = NULL
END	
SELECT @sAgentCode = ISNULL(@agent_code,'')

IF @sAgentCode <> '' 
	SELECT @iAgentCnt = party_cnt FROM party WHERE shortname = @sAgentCode
ELSE
	SELECT @iAgentCnt = 0

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
-- Find Start Date of Financial Year - Last Day of previous financial year + 1 
SELECT @dFinanceYrStrt = (Period_End_Date - 30) 
FROM Period 
WHERE Period_id = 
(
	SELECT min(period_id)
	FROM	period
	WHERE year(Period_End_Date)= Year(@dEndDate)
)	

	
--Pick up quotes and values
INSERT into #tempQuoteNB
	SELECT 
	ifi.insurance_file_cnt,
	' Direct',
	ifi.lead_agent_cnt,
	ifi.product_id,
	NULL,
	ifs.date_created,
	(select 1 where (ifi.insurance_file_type_id = 1) or (ifi.insurance_file_type_id = 2)),
	--(select r.total_this_premium where (ifi.insurance_file_type_id = 1) or (ifi.insurance_file_type_id = 2)),
	CASE @typeofcurrency
				WHEN 'Base' THEN 
				(select r.total_this_premium where (ifi.insurance_file_type_id = 1) or (ifi.insurance_file_type_id = 2))* 
				(ISNULL(IFI.currency_base_xrate,CR.rate_against_base))
				WHEN 'System' THEN 
				(select r.total_this_premium where (ifi.insurance_file_type_id = 1) or (ifi.insurance_file_type_id = 2))* 
				(ISNULL(IFI.currency_base_xrate,CR.rate_against_base)) / (ISNULL(IFI.system_base_xrate,CR.rate_against_base))
		END total_this_premium,
	
	NULL,  -- cant populate it now - else pick up 2 records	
	(select 1 where ifi.insurance_file_type_id = 2),
--	(select r.total_this_premium where ifi.insurance_file_type_id = 2 ),
	CASE @typeofcurrency
		WHEN 'Base' THEN 
				(select r.total_this_premium where ifi.insurance_file_type_id = 2 )*(ISNULL(IFI.currency_base_xrate,CR.rate_against_base))
				WHEN 'System' THEN 
				(select r.total_this_premium where ifi.insurance_file_type_id = 2 )*(ISNULL(IFI.currency_base_xrate,CR.rate_against_base)) / ISNULL(IFI.system_base_xrate,CR.rate_against_base)
		END total_this_premium,
	1		-- Default to include in Annual figures
	,
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
	END 'GroupByCode' 

	FROM insurance_file ifi

JOIN insurance_file_system ifs on ifs.insurance_file_cnt = ifi.insurance_file_cnt
JOIN insurance_file_risk_link ifrl on ifrl.insurance_file_cnt = ifi.insurance_file_cnt
JOIN risk r on r.risk_cnt = ifrl.risk_cnt
JOIN currencyrate CR
ON CR.currency_id = IFI.currency_id
AND CR.company_id = IFI.source_id
JOIN CURRENCY CB ON CB.Currency_id = IFI.currency_id
JOIN SOURCE S ON S.Source_id= IFI.Source_id



WHERE 		
		(ifs.date_created <= @dEndDate)  AND 	
		(ifs.date_created >= @dFinanceYrStrt)
		AND CR.effective_from IN
				(
					SELECT MAX(effective_from)
						FROM CurrencyRate 
						WHERE effective_from <= IFI.cover_start_date
						AND   currency_id = CR.currency_id
						AND company_id = CR.company_id
				)

--		(datediff(year,ifs.date_created, @dEndDate) < 1 )

--INSERT into #tempNB
--	SELECT 
--		ifi.lead_agent_cnt, 
--		ifi.product_id, 
		
--		el.event_date, 
--		r.total_this_premium, 
--		(SELECT 1 WHERE ifs.date_created >= @dStartDate)
--	FROM insurance_file ifi
--join insurance_file_system ifs on ifs.insurance_file_cnt = ifi.insurance_file_cnt
--join insurance_file_risk_link ifrl on ifrl.insurance_file_cnt = ifi.insurance_file_cnt
--join risk r on r.risk_cnt = ifrl.risk_cnt
--join event_log el on el.insurance_file_cnt = ifi.insurance_file_cnt
--where 	 

--        (el.description like 'Policy Made Live')


UPDATE #tempQuoteNB
	SET  NBDate = el.event_date
	FROM event_log el
	WHERE el.insurance_file_cnt = InsFileCnt
	
UPDATE #tempQuoteNB
	SET AgentName = pa.name
	FROM party pa
	WHERE AgentCnt = pa.party_cnt


	
UPDATE #tempQuoteNB
	SET ProductName = p.description
	FROM product p
	WHERE ProductID = p.product_id
	
	
UPDATE #tempQuoteNB
	SET RecordType = 2
	WHERE QuoteDate >= @dStartDate and QuoteDate <= @dEndDate

	
SELECT * from #tempQuoteNB where (@iAgentCnt = 0 or (@iAgentCnt <> 0 AND @iAgentCnt = AgentCnt))
DROP table #tempQuoteNB



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

