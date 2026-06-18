SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_TXN_policy_agent_fetch'
GO

CREATE PROCEDURE spu_TXN_policy_agent_fetch
(
@from_event bit,
@insurance_file_cnt int,
@task_type int = NULL,
@transaction_type int = 0,
@from_breakdown bit = 0
)

AS

DECLARE @premium numeric(19,4)
DECLARE @commission numeric(19,4)
DECLARE @insurer char(20)
DECLARE @insurer_cnt int
DECLARE @risk_code_id int
DECLARE @risk_group_id int
DECLARE @effective_date datetime
DECLARE @override_rate_table tinyint
DECLARE @agent_cnt int
DECLARE @default_rate numeric(19,4)
DECLARE @default_value numeric(19,4)
DECLARE @default_minimum numeric(19,4)
DECLARE @rate_type_ind tinyint
DECLARE @default_tax_group_id int

CREATE TABLE #TXN_policy_agent (
	agent_cnt int,
	[name] varchar (255),
	agent_type char (10),
	amount numeric(19, 4),
	agent_commission_percentage numeric(7, 5),
	agent_commission_amount numeric(19, 9),
	agent_commission_value numeric(19, 9),
	tax_group_code varchar (10),
	tax_group_percentage float,
	tax_group_value money,
	agent_commission_total_value numeric(19, 9),
	apply_perc_to_prem_or_comm tinyint,
	is_minimum_brokerage tinyint,
	tax_group_id int,
	is_viewable_only tinyint,
	override_rate_table tinyint,
	default_commission_percentage numeric(19,4),
	default_commission_charge numeric(19,4),
	default_commission_minimum numeric(19,4) ) 

IF @from_event=0
BEGIN

	SELECT @insurer=P.shortname, @risk_code_id=INF.risk_code_id, @effective_date=INF.cover_start_date, @risk_group_id=RC.risk_group_id FROM
	insurance_file INF
	INNER JOIN party P ON INF.lead_insurer_cnt=P.party_cnt
	INNER JOIN risk_code RC ON RC.risk_code_id=INF.risk_code_id
	WHERE INF.insurance_file_cnt=@insurance_file_cnt
	
	SELECT @premium=SUM(ICS.premium_excluding_tax)
	FROM Insurance_COB_section ICS
	INNER JOIN COB_rating_section CRS ON ICS.COB_rating_section_id=CRS.COB_rating_section_id
	WHERE ICS.insurance_file_cnt=@insurance_file_cnt AND CRS.is_in_TP_premium_calculation=1
	
	IF @insurer='MULTI'
		SELECT @commission=SUM(PCS.commission_exc_tax) FROM
		policy_coinsurers_section PCS
		INNER JOIN COB_rating_section CRS ON PCS.COB_rating_section_id=CRS.COB_rating_section_id
		WHERE insurance_file_cnt=@insurance_file_cnt AND CRS.is_in_TP_commission_calculation=1
	ELSE
		SELECT @commission=SUM(ICS.commission_net)
		FROM Insurance_COB_section ICS
		INNER JOIN COB_rating_section CRS ON ICS.COB_rating_section_id=CRS.COB_rating_section_id
		WHERE ICS.insurance_file_cnt=@insurance_file_cnt AND CRS.is_in_TP_commission_calculation=1
	
	INSERT INTO #TXN_policy_agent 
	SELECT
	PA.agent_cnt,
	P.name,
	CASE PYA.party_agent_type_id
	WHEN 1 THEN 'AG'
	WHEN 2 THEN 'SA'
	WHEN 3 THEN 'INT'
	ELSE '' END as agent_type,
	CASE PA.apply_perc_to_prem_or_comm
	WHEN 1 THEN @commission
	ELSE @premium
	END AS amount,
	PA.agent_commission_percentage/100,
	PA.agent_commission_amount,
	PA.agent_commission_value,
	ISNULL(TG.code,''),
	ISNULL(TC.percentage,0)/100,
	ISNULL(TC.value,0),
	PA.agent_commission_value + ISNULL(TC.value,0),
	PA.apply_perc_to_prem_or_comm,
	PA.is_minimum_brokerage,
	ISNULL(TC.tax_group_id,0),
	ISNULL(PYA.is_viewable_only,0),
	PA.override_rate_table,
	0,
	0,
	0
	FROM
	policy_agents PA
	INNER JOIN party P ON P.party_cnt=PA.agent_cnt
	INNER JOIN party_agent PYA ON PYA.party_cnt=P.party_cnt
	LEFT OUTER JOIN tax_calculation TC ON TC.policy_agents_id=PA.policy_agents_id and TC.insurance_file_cnt=@insurance_file_cnt
	LEFT OUTER JOIN tax_group TG ON TC.tax_group_id=TG.tax_group_id
	WHERE
	PA.insurance_file_cnt=@insurance_file_cnt

END

ELSE

BEGIN

	SELECT @insurer=P.shortname, @risk_code_id=INF.risk_code_id, @effective_date=getdate(), @risk_group_id=RC.risk_group_id FROM
	event_insurance_file INF
	INNER JOIN party P ON INF.lead_insurer_cnt=P.party_cnt
	INNER JOIN risk_code RC ON RC.risk_code_id=INF.risk_code_id
	WHERE INF.insurance_file_cnt=@insurance_file_cnt

	SELECT @premium=SUM(ICS.premium_excluding_tax)
	FROM event_Insurance_COB_section ICS
	INNER JOIN COB_rating_section CRS ON ICS.COB_rating_section_id=CRS.COB_rating_section_id
	WHERE ICS.insurance_file_cnt=@insurance_file_cnt AND CRS.is_in_TP_premium_calculation=1

	IF @insurer='MULTI'
		SELECT @commission=SUM(PCS.commission_exc_tax) FROM
		event_policy_coinsurers_section PCS
		INNER JOIN COB_rating_section CRS ON PCS.COB_rating_section_id=CRS.COB_rating_section_id
		WHERE insurance_file_cnt=@insurance_file_cnt AND CRS.is_in_TP_commission_calculation=1
	ELSE
		SELECT @commission=SUM(ICS.commission_net)
		FROM event_Insurance_COB_section ICS
		INNER JOIN COB_rating_section CRS ON ICS.COB_rating_section_id=CRS.COB_rating_section_id
		WHERE ICS.insurance_file_cnt=@insurance_file_cnt AND CRS.is_in_TP_commission_calculation=1
	
	INSERT INTO #TXN_policy_agent 
	SELECT
	PA.agent_cnt,
	P.name,
	CASE PYA.party_agent_type_id
	WHEN 1 THEN 'AG'
	WHEN 2 THEN 'SA'
	WHEN 3 THEN 'INT'
	ELSE '' END as agent_type,
	CASE PA.apply_perc_to_prem_or_comm
	WHEN 1 THEN @commission
	ELSE @premium
	END AS amount,
	PA.agent_commission_percentage/100,
	PA.agent_commission_amount,
	PA.agent_commission_value,
	ISNULL(TG.code,'') as tax_group,
	ISNULL(TC.percentage,0)/100 as tax_percentage,
	ISNULL(TC.value,0) as tax_value,
	PA.agent_commission_value + ISNULL(TC.value,0),
	PA.apply_perc_to_prem_or_comm,
	PA.is_minimum_brokerage,
	ISNULL(TC.tax_group_id,0) as tax_group_id,
	ISNULL(PYA.is_viewable_only,0),
	PA.override_rate_table,
	0,
	0,
	0
	FROM
	event_policy_agents PA
	INNER JOIN party P ON P.party_cnt=PA.agent_cnt
	INNER JOIN party_agent PYA ON PYA.party_cnt=P.party_cnt
	LEFT OUTER JOIN event_tax_calculation TC ON TC.policy_agents_id=PA.policy_agents_id and TC.insurance_file_cnt=@insurance_file_cnt
	LEFT OUTER JOIN tax_group TG ON TC.tax_group_id=TG.tax_group_id
	WHERE
	PA.insurance_file_cnt=@insurance_file_cnt
	
END

/*Adding default third party agent in policy for add mode only*/
IF @task_type = 1  
BEGIN
    INSERT INTO #txn_policy_agent
    SELECT p.party_cnt,
       p.name,
       'AG',
       0,
       0,
       0,
       0,
       0,
       0,
       0,
       0,
       0,
       0,
       0,
       0,
       0,
       0,
       0,
       0
    FROM party p
    JOIN party p1
           ON p1.agent_cnt = p.party_cnt
    JOIN insurance_file inf
           ON inf.insured_cnt = p1.party_cnt
    WHERE  inf.insurance_file_cnt = @insurance_file_cnt
           AND p.party_cnt NOT IN (SELECT agent_cnt
				       FROM   #txn_policy_agent)
END

DECLARE AGENT_CURSOR CURSOR FORWARD_ONLY FOR
SELECT agent_cnt,override_rate_table FROM #TXN_policy_agent

OPEN AGENT_CURSOR

FETCH NEXT FROM AGENT_CURSOR INTO @agent_cnt, @override_rate_table

WHILE @@FETCH_STATUS=0
BEGIN
	exec spu_AGR_agent_rate_by_transtype_select @party_id=@agent_cnt, @risk_Code_id=@risk_code_id, @risk_group_id=@risk_group_id, @effective_date=@effective_date, @transaction_type=@transaction_type, @agent_rate=@default_rate OUTPUT, @agent_value=@default_value OUTPUT, @agent_minimum=@default_minimum OUTPUT, @rate_type_ind=@rate_type_ind OUTPUT, @tax_group_id=@default_tax_group_id OUTPUT
	UPDATE #TXN_policy_agent SET default_commission_percentage=ISNULL(@default_rate,0)/100, default_commission_charge=@default_value, default_commission_minimum=@default_minimum WHERE CURRENT OF AGENT_CURSOR

	IF ISNULL(@override_rate_table,0)=0 AND @from_event<>0 and @from_breakdown=0
		UPDATE #TXN_policy_agent SET agent_commission_percentage=ISNULL(@default_rate,0)/100, agent_commission_amount=@default_value WHERE CURRENT OF AGENT_CURSOR

	FETCH NEXT FROM AGENT_CURSOR INTO @agent_cnt, @override_rate_table
END

CLOSE AGENT_CURSOR
DEALLOCATE AGENT_CURSOR
    
SELECT * FROM #TXN_policy_agent

DROP TABLE #TXN_policy_agent
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

