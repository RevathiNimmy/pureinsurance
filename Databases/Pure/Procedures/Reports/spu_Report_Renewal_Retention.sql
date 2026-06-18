SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Renewal_Retention'
GO

CREATE PROCEDURE spu_Report_Renewal_Retention
    @start_date DATETIME,
    @end_date DATETIME,
    @group_by VARCHAR(20),
    @then_by VARCHAR(20),
    @customer_category VARCHAR(20),
    @unique_report_name VARCHAR(300),
    @report_type VARCHAR(20)

AS

DECLARE @fsa_customer_category int
DECLARE @max_agent_id int
DECLARE @max_sub_agent_id int

IF @group_by=@then_by
	SELECT @then_by='None'

IF @customer_category='Commercial'
	SELECT @fsa_customer_category=0
ELSE
	IF @customer_category='Retail'
		SELECT @fsa_customer_category=1


CREATE TABLE #tmpRR
(
	groupby1_name varchar(20),
	groupby1_id int,
	groupby1_description varchar(255),
	groupby2_name varchar(20),
	groupby2_id int,
	groupby2_description varchar(255),
	report_section_id int,
	policy_status_id int,
	policy_status_description varchar(255),
	client_code char(20),
	client_name varchar(255),
	policy_number varchar(30),
	insurer_code char(20),
	cover_from_date datetime,
	document_reference varchar(25),
	documenttype_id int,
	gross_premium numeric(19,4),
	gross_commission numeric(19,4),
	extras numeric(19,4),
	fees numeric(19,4),
	os_balance numeric(19,4),
	renewal_date datetime,
	lapsed_date datetime,
	lapsed_reason varchar(255),
	branch_id int,
	branch_description varchar(255),
	account_exec_id int,
	account_exec_description varchar(255),
	account_handler_id int,
	account_handler_description varchar(255),
	risk_id int,
	risk_description varchar(255),
	insurer_id int,
	insurer_description varchar(255),
	business_type_id int,
	business_type_description varchar(255),
	agent_id int,
	agent_description varchar(255),
	sub_agent_id int,
	sub_agent_description varchar(255),
	renewal_or_transferred tinyint,
	report_type varchar(20),
	gr1_ren int,
	gr1_tra int,
	gr1_lap int,
	gr1_rep int,
	gr1_ros int,
	gr2_ren int,
	gr2_tra int,
	gr2_lap int,
	gr2_rep int,
	gr2_ros int,
	tot_ren int,
	tot_tra int,
	tot_lap int,
	tot_rep int,
	tot_ros int
)


INSERT INTO #tmpRR
SELECT
@group_by,
NULL,
NULL,
@then_by,
NULL,
NULL,
NULL,
IFL.insurance_file_status_id,
ISNULL(IFS.description,'Current'),
P.shortname,
P.resolved_name,
IFL.insurance_ref,
P2.shortname,
IFL.cover_start_date,
D.document_ref,
D.documenttype_id,
EIF.this_premium,
ISNULL(EIF.commission_amount,0) + (SELECT ISNULL(SUM(event_policy_fee.commission_amount),0) FROM event_policy_fee INNER JOIN party ON event_policy_fee.party_cnt=party.party_cnt WHERE event_policy_fee.insurance_file_cnt=EIF.insurance_file_cnt AND party.party_type_id=10),
(SELECT ISNULL(SUM(event_policy_fee.total_fee),0) FROM event_policy_fee INNER JOIN party ON event_policy_fee.party_cnt=party.party_cnt WHERE event_policy_fee.insurance_file_cnt=EIF.insurance_file_cnt AND party.party_type_id=10),
(SELECT ISNULL(SUM(event_policy_fee.total_fee),0) FROM event_policy_fee INNER JOIN party ON event_policy_fee.party_cnt=party.party_cnt WHERE event_policy_fee.insurance_file_cnt=EIF.insurance_file_cnt AND party.party_type_id=9),
(SELECT ISNULL(SUM(outstanding_amount),0) FROM transdetail TD INNER JOIN account A ON TD.account_id=A.account_id INNER JOIN ledger L ON A.ledger_id=L.ledger_id WHERE TD.document_id=D.document_id AND L.ledger_short_name='SA'),
NULL,
NULL,
NULL,
S.source_id,
S.description,
P4.party_cnt,
P4.shortname,
P3.party_cnt,
P3.resolved_name,
RC.risk_code_id,
RC.description,
P2.party_cnt,
P2.resolved_name,
BT.business_type_id,
BT.description,
CASE (SELECT COUNT(*) FROM event_policy_agents INNER JOIN party_agent ON event_policy_agents.agent_cnt=party_agent.party_cnt WHERE event_policy_agents.insurance_file_cnt=EIF.insurance_file_cnt and party_agent.party_agent_type_id=1)
WHEN 0 THEN NULL
WHEN 1 THEN (SELECT event_policy_agents.agent_cnt FROM event_policy_agents INNER JOIN party_agent ON event_policy_agents.agent_cnt=party_agent.party_cnt WHERE event_policy_agents.insurance_file_cnt=EIF.insurance_file_cnt and party_agent.party_agent_type_id=1)
ELSE -1 END,
NULL,
CASE (SELECT COUNT(*) FROM event_policy_agents INNER JOIN party_agent ON event_policy_agents.agent_cnt=party_agent.party_cnt WHERE event_policy_agents.insurance_file_cnt=EIF.insurance_file_cnt and party_agent.party_agent_type_id=2)
WHEN 0 THEN NULL
WHEN 1 THEN (SELECT event_policy_agents.agent_cnt FROM event_policy_agents INNER JOIN party_agent ON event_policy_agents.agent_cnt=party_agent.party_cnt WHERE event_policy_agents.insurance_file_cnt=EIF.insurance_file_cnt and party_agent.party_agent_type_id=2)
ELSE -1 END,
NULL,
1,
@report_type,
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
FROM
document D
INNER JOIN insurance_file IFL ON D.insurance_file_cnt=IFL.insurance_file_cnt
INNER JOIN transaction_export_folder TEF ON D.document_ref=TEF.document_ref AND D.company_id=TEF.source_id
INNER JOIN event_log EL ON TEF.event_log_id=EL.event_cnt
INNER JOIN event_insurance_file EIF ON EIF.insurance_file_cnt=EL.event_cnt
INNER JOIN party P on IFL.insured_cnt=P.party_cnt
INNER JOIN party P2 ON IFL.lead_insurer_cnt=P2.party_cnt
INNER JOIN source S ON IFL.source_id=S.source_id
INNER JOIN risk_code RC ON IFL.risk_code_id=RC.risk_code_id
INNER JOIN business_type BT ON BT.business_type_id=IFL.business_type_id
LEFT OUTER JOIN insurance_file_status IFS ON IFS.insurance_file_status_id=IFL.insurance_file_status_id
LEFT OUTER JOIN lapsed_reason LR ON IFL.lapsed_reason_id=LR.lapsed_reason_id
LEFT OUTER JOIN party P3 ON IFL.account_handler_cnt=P3.party_cnt
LEFT OUTER JOIN party P4 ON P4.party_cnt = P.consultant_cnt
WHERE
D.documenttype_id IN (15,16,35,36)
AND
TEF.effective_date BETWEEN @start_date AND @end_date
AND
IFL.source_id NOT IN (SELECT id FROM temp_report_exclude WHERE type='BR' and unique_report_name=@unique_report_name)
AND
ISNULL(IFL.account_executive_cnt,0) NOT IN (SELECT id FROM temp_report_exclude WHERE type='AE' and unique_report_name=@unique_report_name)
AND
ISNULL(IFL.account_handler_cnt,0) NOT IN (SELECT id FROM temp_report_exclude WHERE type='AH' and unique_report_name=@unique_report_name)
AND
IFL.risk_code_id NOT IN (SELECT id FROM temp_report_exclude WHERE type='RC' and unique_report_name=@unique_report_name)
AND
IFL.lead_insurer_cnt NOT IN (SELECT id FROM temp_report_exclude WHERE type='IN' and unique_report_name=@unique_report_name)
AND
IFL.fsa_customer_category_id=ISNULL(@fsa_customer_category,IFL.fsa_customer_category_id)

INSERT INTO #tmpRR
SELECT
@group_by,
NULL,
NULL,
@then_by,
NULL,
NULL,
NULL,
ISNULL(IFL.insurance_file_status_id,0),
ISNULL(IFS.description,'Current'),
P.shortname,
P.resolved_name,
IFL.insurance_ref,
P2.shortname,
NULL,
NULL,
NULL,
IFL.this_premium,
ISNULL(IFL.commission_amount,0) + (SELECT ISNULL(SUM(policy_fee.commission_amount),0) FROM policy_fee INNER JOIN party ON policy_fee.party_cnt=party.party_cnt WHERE policy_fee.insurance_file_cnt=IFL.insurance_file_cnt AND party.party_type_id=10),
(SELECT ISNULL(SUM(policy_fee.total_fee),0) FROM policy_fee INNER JOIN party ON policy_fee.party_cnt=party.party_cnt WHERE policy_fee.insurance_file_cnt=IFL.insurance_file_cnt AND party.party_type_id=10),
(SELECT ISNULL(SUM(policy_fee.total_fee),0) FROM policy_fee INNER JOIN party ON policy_fee.party_cnt=party.party_cnt WHERE policy_fee.insurance_file_cnt=IFL.insurance_file_cnt AND party.party_type_id=9),
0,
IFL.renewal_date,
IFL.lapsed_date,
LR.description,
S.source_id,
S.description,
P4.party_cnt,
P4.shortname,
P3.party_cnt,
P3.resolved_name,
RC.risk_code_id,
RC.description,
P2.party_cnt,
P2.resolved_name,
BT.business_type_id,
BT.description,
CASE (SELECT COUNT(*) FROM policy_agents INNER JOIN party_agent ON policy_agents.agent_cnt=party_agent.party_cnt WHERE policy_agents.insurance_file_cnt=IFL.insurance_file_cnt and party_agent.party_agent_type_id=1)
WHEN 0 THEN NULL
WHEN 1 THEN (SELECT policy_agents.agent_cnt FROM policy_agents INNER JOIN party_agent ON policy_agents.agent_cnt=party_agent.party_cnt WHERE policy_agents.insurance_file_cnt=IFL.insurance_file_cnt and party_agent.party_agent_type_id=1)
ELSE -1 END,
NULL,
CASE (SELECT COUNT(*) FROM policy_agents INNER JOIN party_agent ON policy_agents.agent_cnt=party_agent.party_cnt WHERE policy_agents.insurance_file_cnt=IFL.insurance_file_cnt and party_agent.party_agent_type_id=2)
WHEN 0 THEN NULL
WHEN 1 THEN (SELECT policy_agents.agent_cnt FROM policy_agents INNER JOIN party_agent ON policy_agents.agent_cnt=party_agent.party_cnt WHERE policy_agents.insurance_file_cnt=IFL.insurance_file_cnt and party_agent.party_agent_type_id=2)
ELSE -1 END,
NULL,
0,
@report_type,
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
FROM
insurance_file IFL
INNER JOIN party P on IFL.insured_cnt=P.party_cnt
INNER JOIN party P2 ON IFL.lead_insurer_cnt=P2.party_cnt
INNER JOIN source S ON IFL.source_id=S.source_id
INNER JOIN risk_code RC ON IFL.risk_code_id=RC.risk_code_id
INNER JOIN business_type BT ON BT.business_type_id=IFL.business_type_id
LEFT OUTER JOIN insurance_file_status IFS ON IFS.insurance_file_status_id=IFL.insurance_file_status_id
LEFT OUTER JOIN lapsed_reason LR ON IFL.lapsed_reason_id=LR.lapsed_reason_id
LEFT OUTER JOIN party P3 ON IFL.account_handler_cnt=P3.party_cnt
LEFT OUTER JOIN party P4 ON P4.party_cnt = P.consultant_cnt
WHERE
IFL.renewal_date BETWEEN @start_date AND @end_date
AND
ISNULL(IFL.insurance_file_status_id,0) IN (0,2,4)
AND
IFL.source_id NOT IN (SELECT id FROM temp_report_exclude WHERE type='BR' and unique_report_name=@unique_report_name)
AND
ISNULL(IFL.account_executive_cnt,0) NOT IN (SELECT id FROM temp_report_exclude WHERE type='AE' and unique_report_name=@unique_report_name)
AND
ISNULL(IFL.account_handler_cnt,0) NOT IN (SELECT id FROM temp_report_exclude WHERE type='AH' and unique_report_name=@unique_report_name)
AND
IFL.risk_code_id NOT IN (SELECT id FROM temp_report_exclude WHERE type='RC' and unique_report_name=@unique_report_name)
AND
IFL.lead_insurer_cnt NOT IN (SELECT id FROM temp_report_exclude WHERE type='IN' and unique_report_name=@unique_report_name)
AND
IFL.fsa_customer_category_id=ISNULL(@fsa_customer_category,IFL.fsa_customer_category_id)

UPDATE #tmpRR
SET report_section_id=1 WHERE documenttype_id IN (15,16) AND renewal_or_transferred=1

UPDATE #tmpRR
SET report_section_id=2 WHERE documenttype_id IN (35,36) AND renewal_or_transferred=1

UPDATE #tmpRR
SET report_section_id=3 WHERE policy_status_id=2 AND renewal_or_transferred=0

UPDATE #tmpRR
SET report_section_id=4 WHERE policy_status_id=4 AND renewal_or_transferred=0

UPDATE #tmpRR
SET report_section_id=5 WHERE policy_status_id=0 AND renewal_or_transferred=0

UPDATE #tmpRR
SET gross_premium=-gross_premium,gross_commission=-gross_commission,extras=-extras,fees=-fees,os_balance=-os_balance
WHERE documenttype_id IN (15,35)

SELECT @max_agent_id=ISNULL(MAX(agent_id),0)+1 FROM #tmpRR
SELECT @max_sub_agent_id=ISNULL(MAX(sub_agent_id),0)+1 FROM #tmpRR

UPDATE #tmpRR
SET agent_description=(SELECT party.resolved_name from party WHERE party.party_cnt=agent_id)
WHERE agent_id>0

UPDATE #tmpRR
SET agent_description='Multiple Agents', agent_id=@max_agent_id WHERE agent_id=-1

UPDATE #tmpRR
SET sub_agent_description=(SELECT party.resolved_name from party WHERE party.party_cnt=sub_agent_id)
WHERE sub_agent_id>0

UPDATE #tmpRR
SET sub_agent_description='Multiple Sub-Agents', sub_agent_id=@max_sub_agent_id WHERE sub_agent_id=-1

UPDATE #tmpRR
SET
groupby1_id=CASE @group_by
WHEN 'Branch' THEN branch_id
WHEN 'Account Executive' THEN account_exec_id
WHEN 'Account Handler' THEN account_handler_id
WHEN 'Risk' THEN risk_id
WHEN 'Policy Status' THEN policy_status_id
WHEN 'Insurer' THEN insurer_id
WHEN 'Business Type' THEN business_type_id
WHEN 'Sub Agent' THEN sub_agent_id
WHEN 'Agent' THEN agent_id
END,
groupby1_description=CASE @group_by
WHEN 'Branch' THEN branch_description
WHEN 'Account Executive' THEN account_exec_description
WHEN 'Account Handler' THEN account_handler_description
WHEN 'Risk' THEN risk_description
WHEN 'Policy Status' THEN policy_status_description
WHEN 'Insurer' THEN insurer_description
WHEN 'Business Type' THEN business_type_description
WHEN 'Sub Agent' THEN sub_agent_description
WHEN 'Agent' THEN agent_description
END,
groupby2_id=CASE @then_by
WHEN 'Branch' THEN branch_id
WHEN 'Account Executive' THEN account_exec_id
WHEN 'Account Handler' THEN account_handler_id
WHEN 'Risk' THEN risk_id
WHEN 'Policy Status' THEN policy_status_id
WHEN 'Insurer' THEN insurer_id
WHEN 'Business Type' THEN business_type_id
WHEN 'Sub Agent' THEN sub_agent_id
WHEN 'Agent' THEN agent_id
WHEN 'None' THEN NULL
END,
groupby2_description=CASE @then_by
WHEN 'Branch' THEN branch_description
WHEN 'Account Executive' THEN account_exec_description
WHEN 'Account Handler' THEN account_handler_description
WHEN 'Risk' THEN risk_description
WHEN 'Policy Status' THEN policy_status_description
WHEN 'Insurer' THEN insurer_description
WHEN 'Business Type' THEN business_type_description
WHEN 'Sub Agent' THEN sub_agent_description
WHEN 'Agent' THEN agent_description
WHEN 'None' THEN NULL
END

--AR:Crystal7 does not calculate running totals correctly, so calculate them within the sp.
CREATE TABLE #tmpCube
(
grouping1 int,
grouping2 int,
grouping3 int,
total int
)

INSERT INTO #tmpCube
SELECT
CASE WHEN (GROUPING(groupby1_id) = 1) THEN -1
ELSE groupby1_id END,
CASE WHEN (GROUPING(groupby2_id) = 1) THEN -1
ELSE groupby2_id END,
CASE WHEN (GROUPING(report_section_id) = 1) THEN -1
ELSE report_section_id END,
count(*)
FROM
#tmpRR
GROUP BY groupby1_id,groupby2_id,report_section_id WITH CUBE

UPDATE #tmpRR SET
gr1_ren=ISNULL((SELECT ISNULL(total,0) FROM #tmpCube WHERE ISNULL(#tmpCube.grouping1,-2)=ISNULL(#tmpRR.groupby1_id,-2) AND #tmpCube.grouping2=-1 AND #tmpCube.grouping3=1),0),
gr1_tra=ISNULL((SELECT ISNULL(total,0) FROM #tmpCube WHERE ISNULL(#tmpCube.grouping1,-2)=ISNULL(#tmpRR.groupby1_id,-2) AND #tmpCube.grouping2=-1 AND #tmpCube.grouping3=2),0),
gr1_lap=ISNULL((SELECT ISNULL(total,0) FROM #tmpCube WHERE ISNULL(#tmpCube.grouping1,-2)=ISNULL(#tmpRR.groupby1_id,-2) AND #tmpCube.grouping2=-1 AND #tmpCube.grouping3=3),0),
gr1_rep=ISNULL((SELECT ISNULL(total,0) FROM #tmpCube WHERE ISNULL(#tmpCube.grouping1,-2)=ISNULL(#tmpRR.groupby1_id,-2) AND #tmpCube.grouping2=-1 AND #tmpCube.grouping3=4),0),
gr1_ros=ISNULL((SELECT ISNULL(total,0) FROM #tmpCube WHERE ISNULL(#tmpCube.grouping1,-2)=ISNULL(#tmpRR.groupby1_id,-2) AND #tmpCube.grouping2=-1 AND #tmpCube.grouping3=5),0),
tot_ren=ISNULL((SELECT ISNULL(total,0) FROM #tmpCube WHERE #tmpCube.grouping1=-1 AND #tmpCube.grouping2=-1 AND #tmpCube.grouping3=1),0),
tot_tra=ISNULL((SELECT ISNULL(total,0) FROM #tmpCube WHERE #tmpCube.grouping1=-1 AND #tmpCube.grouping2=-1 AND #tmpCube.grouping3=2),0),
tot_lap=ISNULL((SELECT ISNULL(total,0) FROM #tmpCube WHERE #tmpCube.grouping1=-1 AND #tmpCube.grouping2=-1 AND #tmpCube.grouping3=3),0),
tot_rep=ISNULL((SELECT ISNULL(total,0) FROM #tmpCube WHERE #tmpCube.grouping1=-1 AND #tmpCube.grouping2=-1 AND #tmpCube.grouping3=4),0),
tot_ros=ISNULL((SELECT ISNULL(total,0) FROM #tmpCube WHERE #tmpCube.grouping1=-1 AND #tmpCube.grouping2=-1 AND #tmpCube.grouping3=5),0)

IF @then_by <> 'None'
UPDATE #tmpRR SET
gr2_ren=ISNULL((SELECT ISNULL(total,0) FROM #tmpCube WHERE ISNULL(#tmpCube.grouping1,-2)=ISNULL(#tmpRR.groupby1_id,-2) AND ISNULL(#tmpCube.grouping2,-2)=ISNULL(#tmpRR.groupby2_id,-2) AND #tmpCube.grouping3=1),0),
gr2_tra=ISNULL((SELECT ISNULL(total,0) FROM #tmpCube WHERE ISNULL(#tmpCube.grouping1,-2)=ISNULL(#tmpRR.groupby1_id,-2) AND ISNULL(#tmpCube.grouping2,-2)=ISNULL(#tmpRR.groupby2_id,-2) AND #tmpCube.grouping3=2),0),
gr2_lap=ISNULL((SELECT ISNULL(total,0) FROM #tmpCube WHERE ISNULL(#tmpCube.grouping1,-2)=ISNULL(#tmpRR.groupby1_id,-2) AND ISNULL(#tmpCube.grouping2,-2)=ISNULL(#tmpRR.groupby2_id,-2) AND #tmpCube.grouping3=3),0),
gr2_rep=ISNULL((SELECT ISNULL(total,0) FROM #tmpCube WHERE ISNULL(#tmpCube.grouping1,-2)=ISNULL(#tmpRR.groupby1_id,-2) AND ISNULL(#tmpCube.grouping2,-2)=ISNULL(#tmpRR.groupby2_id,-2) AND #tmpCube.grouping3=4),0),
gr2_ros=ISNULL((SELECT ISNULL(total,0) FROM #tmpCube WHERE ISNULL(#tmpCube.grouping1,-2)=ISNULL(#tmpRR.groupby1_id,-2) AND ISNULL(#tmpCube.grouping2,-2)=ISNULL(#tmpRR.groupby2_id,-2) AND #tmpCube.grouping3=5),0)

DROP TABLE #tmpCube

SELECT
*
FROM
#tmpRR
ORDER BY groupby1_id, groupby2_id, report_section_id

DROP TABLE #tmpRR

GO

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
