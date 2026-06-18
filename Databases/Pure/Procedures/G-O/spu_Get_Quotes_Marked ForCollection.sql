SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXEC DDLDropProcedure "spu_Get_Quotes_Marked_ForCollection"
GO

CREATE PROCEDURE spu_Get_Quotes_Marked_ForCollection 
@client_id int=null,
@insurancefilecnt int=null,
@agent_id int=null,
@product_ids varchar(500)=NULL,
@start_date datetime=null,
@end_date datetime=null,
@direct bit=null
AS

DECLARE @sqlstr NVARCHAR(4000)

SELECT @sqlstr = ""

CREATE Table #Insurance_file_Temp(
	Insurance_file_cnt int,
	insurance_ref varchar(30),
	client_id int,
	client_name varchar(20),
	client_resolved_name  varchar(387),
	agent_id int,
	agent_name varchar(20),
	agent_resolved_name varchar(387),
	product_id int,
	product_code varchar(10),
	source_id int,
	source_code varchar(10),
	currency_id int,
	currency_code varchar(10),
	Premium numeric(10,4),
	Insurance_file_type numeric(10,4),
	AgentType Varchar(20),
	AgentCommission numeric(10,4),
	is_roundoff_to_zero tinyint
)

--Unmark the older quotes
Update Insurance_file set marked_for_collection=0 where convert(varchar(10),marked_date,112)<convert(varchar(10),getdate(),112) and marked_for_collection = 1 


SELECT @sqlstr = "INSERT INTO #Insurance_file_Temp" + char(13)
SELECT @sqlstr = @sqlstr + " SELECT TOP 500 
			Insurance_file_cnt ,
			insurance_ref , 
			i.Insured_cnt,
			pt.shortname,
			pt.resolved_name, 
			i.Lead_Agent_cnt,
			ag.shortname,
			ag.resolved_name, 
			i.Product_id,
			p.code, 
			s.source_id, 
			s.code, 
			i.currency_id,
			c.code, 
			0, 
			ift.insurance_file_type_id, 
			RTRIM(pat.code),
			0, is_roundoff_to_zero"  + char(13)  

SELECT @sqlstr = @sqlstr + " FROM insurance_file i LEFT JOIN party pt ON i.Insured_cnt=pt.party_cnt
			LEFT JOIN party ag on i.Lead_Agent_cnt=ag.party_cnt
			LEFT JOIN party_agent pag on pag.party_cnt=ag.party_cnt
			LEFT JOIN party_agent_type pat on pag.party_agent_type_id=pat.party_agent_type_id
			LEFT JOIN product p on p.product_id=i.product_id
			LEFT JOIN source s on s.source_id=i.source_id
			LEFT JOIN currency c on c.currency_id=i.currency_id
			LEFT JOIN insurance_file_type ift on i.insurance_file_type_id=ift.insurance_file_type_id " + char(13)

SELECT @sqlstr = @sqlstr + " WHERE marked_for_collection=1 and convert(varchar(10),marked_date,112) = convert(varchar(10),getdate(),112)"
			 + " and i.insurance_file_type_id in (1,3,4,7,10)" 

IF ISNULL(@insurancefilecnt,0) > 0
BEGIN
	SELECT @sqlstr = @sqlstr + " and (i.insurance_file_cnt ="+ convert (varchar,@insurancefilecnt) + ")"
END

IF ISNULL(@client_id,0) > 0
BEGIN
	SELECT @sqlstr = @sqlstr + " and (i.insured_cnt=" + cast(@client_id  as varchar) + ")"
END

IF ISNULL(@agent_id,0) > 0
BEGIN
	SELECT @sqlstr = @sqlstr + " and (i.lead_agent_cnt="+ cast(@agent_id  as varchar) + ")"
END

IF ISNULL(@start_date,"") <> ""
BEGIN
	SELECT @sqlstr = @sqlstr + " and (cover_start_date>='" + cast(@start_date  as varchar(30)) + "')"
END

IF ISNULL(@end_date,"")<>""
BEGIN
	SELECT @sqlstr = @sqlstr + " and (cover_start_date<='" + cast(@end_date  as varchar(30)) + "')"
END

IF ISNULL(@direct,0) > 0
BEGIN
	SELECT @sqlstr = @sqlstr + " and (i.lead_agent_cnt is null)"
END


IF ISNULL(@product_ids,"") <> "" 
BEGIN
 SELECT @sqlstr = @sqlstr + " AND (i.product_id IN (" + cast(@product_ids as varchar(500)) + "))"
END


exec sp_EXECutesql @sqlstr

 DECLARE @insurance_file_cnt INT    
 DECLARE @insurance_file_tax money  
 DECLARE @risk_tax money  
 DECLARE @fee_amount money  
 DECLARE @this_premium money  
 DECLARE @policy_tax money  
 DECLARE @policy_fee_tax money  
 DECLARE @risk_fee_tax money  
 DECLARE @insurance_folder_cnt int  
 DECLARE @product_is_true_monthly_policy int  
 DECLARE @amount_to_be_put_on_next_instalment money  
 DECLARE @policy_fee_amount money  
 DECLARE @risk_fee_amount money  
 DECLARE @agenttype varchar(20)
 DECLARE @agentid varchar(20)
 DECLARE @agent_commission money 

DECLARE cur_insfile CURSOR FAST_FORWARD FOR SELECT insurance_File_cnt,agenttype,agent_id FROM #Insurance_file_Temp
OPEN cur_insfile
FETCH NEXT FROM cur_insfile INTO @insurance_file_cnt,@agenttype,@agentid
While @@fetch_status=0
Begin

 -- annual premium  
 SELECT @this_premium = sum(ROUND(this_premium,2)) FROM peril p join peril_type pt on p.peril_type_id=pt.peril_type_id
 WHERE risk_cnt IN (SELECT risk_cnt FROM risk WHERE risk_cnt IN (SELECT risk_cnt FROM insurance_file_risk_link WHERE insurance_file_cnt = @insurance_file_cnt) and is_risk_selected =1) 
 and isnull(is_stamp_duty_insurer,0)=0

  
 -- policy level tax  
  SELECT @insurance_file_tax = sum(value) FROM tax_calculation WHERE insurance_file_cnt = @insurance_file_cnt and transtype ="TTIF" and is_not_applied_to_client<>1  
  
 -- risk_tax  
 SELECT @risk_tax = SUM(ROUND(value,2)) FROM tax_calculation WHERE insurance_file_cnt = @insurance_file_cnt AND risk_cnt IN (SELECT risk_cnt FROM risk WHERE risk_cnt IN (SELECT risk_cnt FROM insurance_file_risk_link WHERE insurance_file_cnt = @insurance_file_cnt)  
 and is_risk_selected =1) and transtype ="TTR"  and is_not_applied_to_client<>1  
  
 -- policy fees tax  
 SELECT @policy_fee_tax = sum(value) FROM tax_calculation WHERE insurance_file_cnt = @insurance_file_cnt and transtype ="TTF"  and risk_cnt IS NULL  and is_not_applied_to_client<>1  
  
 -- risk fees tax  
 SELECT @risk_fee_tax = sum(ROUND(value,2)) FROM tax_calculation WHERE insurance_file_cnt = @insurance_file_cnt and transtype ="TTF"  
 AND risk_cnt IN (SELECT risk_cnt FROM risk WHERE risk_cnt IN (SELECT risk_cnt FROM insurance_file_risk_link WHERE insurance_file_cnt = @insurance_file_cnt) and is_risk_selected =1) and is_not_applied_to_client<>1  
  
 -- policy_fees_u  - policy fees  
SELECT @policy_fee_amount = SUM(currency_amount) FROM policy_fee_u WHERE insurance_file_cnt = @insurance_file_cnt   and risk_cnt IS NULL  
  
 -- policy_fees_u  - risk fees  
 SELECT @risk_fee_amount = SUM(ROUND(currency_amount,2)) FROM policy_fee_u  
 INNER JOIN risk ON  
  policy_fee_u.risk_cnt = risk.risk_cnt  
 WHERE insurance_file_cnt = @insurance_file_cnt  
 AND policy_fee_u.risk_cnt IS NOT NULL  
 AND risk.is_risk_selected = 1  

 IF @agenttype='Broker'
	SELECT @agent_commission=isnull(sum(commission_value),0) FROM agent_commission WHERE party_cnt=@agentid and insurance_file_cnt = @insurance_file_cnt  
 else
    SELECT @agent_commission=0
 -- return the premium and the details of the items included in the total amount  
 UPDATE #Insurance_file_Temp SET Premium= (ISNULL(@this_premium,0) +  
 ISNULL(@insurance_file_tax,0) +  
 ISNULL(@risk_tax,0)+  
 ISNULL(@policy_fee_amount,0) +  
 ISNULL(@risk_fee_amount,0) +  
 ISNULL(@policy_fee_tax, 0) +  
 ISNULL(@risk_fee_tax, 0)), AgentCommission =@agent_commission
 WHERE Insurance_file_cnt=@insurance_file_cnt



FETCH NEXT FROM cur_insfile INTO @insurance_file_cnt,@agenttype,@agentid
End
CLOSE cur_insfile
DEALLOCATE cur_insfile

SELECT TOP 500 
Insurance_file_cnt "InsuranceFileKey",
insurance_ref "InsuranceFileRef",
client_id "PartyKey",
client_name "PartyShortName",
client_resolved_name "PartyName",
agent_id "AgentKey",
agent_name "AgentShortName",
agent_resolved_name "AgentName",
product_id "ProductKey",
product_code "ProductCode",
source_id "BranchKey",
source_code "BranchCode",
currency_id "CurrencyKey",
currency_code "CurrencyCode",
case is_roundoff_to_zero when 1 Then round(Premium,0) else Premium end "Premium",  
Insurance_file_type "InsuranceFileTypeCode",
AgentType "AgentTypeCode",
AgentCommission "AgentCommission",
case is_roundoff_to_zero when 1 Then round(Premium,0)- Premium else 0 end "RoundAmount"  
FROM #Insurance_file_Temp as MarkedQuotes

DROP TABLE #Insurance_file_Temp

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
