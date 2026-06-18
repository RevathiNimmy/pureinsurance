SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_AUA_Quote_to_NB'
GO


CREATE PROCEDURE  spu_Report_AUA_Quote_to_NB
                    @start_date datetime,
                    @end_date datetime
AS
/**********************************************************************************************************************************
** Created by Kerry Butler
** 04/10/2001
** AUA Reports - Quotations and New Business Report
**
**********************************************************************************************************************************
** Description:     Shows quotations for a specified period and year to date by agent and class of business
**                      Of these quotations to indicate how many have become new business policies
** 1.1          Populate Agent Name with Direct so that direct policies have a title. KB 29/11/01.
** 1.2                  ON data transfer the party_agent trading_name which we are using to pick up the agent name
**                      has not been populated.
**                      So lets use the name field on party table.
** 1.3          CMG/PB 05082002 Add source_id parameter to give filtering by Sub Branch
***********************************************************************************************************************************/
SET NOCOUNT ON
CREATE TABLE #tempAUAQuoteNB
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


)

DECLARE
    @dStartDate datetime,
    @dEndDate       datetime

IF @start_date IS NULL OR @start_date = ''
        SELECT @dStartDate = GETDATE()
ELSE
        SELECT @dStartDate = @start_date

IF @end_date IS NULL OR @end_date = ''
        SELECT @dEndDate = GETDATE()
ELSE
        SELECT @dEndDate = @end_date


--Pick up quotes and values
INSERT into #tempAUAQuoteNB
    SELECT
    ifi.insurance_file_cnt,
    ' Direct',
    ifi.lead_agent_cnt,
    ifi.product_id,
    NULL,
    ifs.date_created,
    (select 1 where (ifi.insurance_file_type_id = 1) or (ifi.insurance_file_type_id = 2)),
    (select r.total_this_premium where (ifi.insurance_file_type_id = 1) or (ifi.insurance_file_type_id = 2)),

    NULL,  -- cant populate it now - else pick up 2 records
    (select 1 where ifi.insurance_file_type_id = 2),
    (select r.total_this_premium where ifi.insurance_file_type_id = 2 )
    FROM insurance_file ifi

JOIN insurance_file_system ifs on ifs.insurance_file_cnt = ifi.insurance_file_cnt
JOIN insurance_file_risk_link ifrl on ifrl.insurance_file_cnt = ifi.insurance_file_cnt
JOIN risk r on r.risk_cnt = ifrl.risk_cnt



WHERE
        (ifs.date_created <= @dEndDate)  AND
        (datediff(year,ifs.date_created, @dEndDate) < 1 )

--INSERT into #tempAUANB
--  SELECT
--      ifi.lead_agent_cnt,
--      ifi.product_id,

--      el.event_date,
--      r.total_this_premium,
--      (SELECT 1 WHERE ifs.date_created >= @dStartDate)
--  FROM insurance_file ifi
--join insurance_file_system ifs on ifs.insurance_file_cnt = ifi.insurance_file_cnt
--join insurance_file_risk_link ifrl on ifrl.insurance_file_cnt = ifi.insurance_file_cnt
--join risk r on r.risk_cnt = ifrl.risk_cnt
--join event_log el on el.insurance_file_cnt = ifi.insurance_file_cnt
--where

--        (el.description like 'Policy Made Live')


UPDATE #tempAUAQuoteNB
    SET  NBDate = el.event_date
    FROM event_log el
    WHERE el.insurance_file_cnt = InsFileCnt

UPDATE #tempAUAQuoteNB
    SET AgentName = pa.name
    FROM party pa
    WHERE AgentCnt = pa.party_cnt



UPDATE #tempAUAQuoteNB
    SET ProductName = p.description
    FROM product p
    WHERE ProductID = p.product_id


SELECT * from #tempAUAQuoteNB
DROP table #tempAUAQuoteNB

GO
