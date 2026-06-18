SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_TXN_policy_coinsurers_fetch'
GO

CREATE PROCEDURE spu_TXN_policy_coinsurers_fetch
(
@from_event bit,
@insurance_file_cnt int
)
AS

IF @from_event=0

	SELECT
	PC.party_cnt,
	P.name,
	PC.coinsurer_percentage,
	PC.coinsurer_cover_percentage,
	--PC.coinsurer_value,
	--PC.coinsurer_ipt_amount,
	--0,
	--coinsurer_commission_amount,
	(SELECT SUM(premium_inc_tax) FROM policy_coinsurers_section WHERE policy_coinsurers_section.insurance_file_cnt=@insurance_file_cnt AND policy_coinsurers_section.party_cnt=PC.party_cnt),
	(SELECT SUM(premium_inc_tax)-SUM(premium_exc_tax) FROM policy_coinsurers_section WHERE policy_coinsurers_section.insurance_file_cnt=@insurance_file_cnt AND policy_coinsurers_section.party_cnt=PC.party_cnt),
	(SELECT SUM(premium_exc_tax) FROM policy_coinsurers_section WHERE policy_coinsurers_section.insurance_file_cnt=@insurance_file_cnt AND policy_coinsurers_section.party_cnt=PC.party_cnt),
	(SELECT SUM(commission_inc_tax) FROM policy_coinsurers_section WHERE policy_coinsurers_section.insurance_file_cnt=@insurance_file_cnt AND policy_coinsurers_section.party_cnt=PC.party_cnt),
	PC.coinsurer_policy_number,
	PC.risk_transfer_agreement,
	PC.isleadunderwriter,
	PC.bureau_party_cnt,
	(SELECT ISNULL(resolved_name,name) FROM party WHERE party_cnt = pc.bureau_party_cnt) 'bureau_name',
	PC.linestands,
	PC.written_line_percentage,  
 	PC.risk_transfer_editable 
	FROM
	policy_coinsurers PC
	INNER JOIN party P ON P.party_cnt=PC.party_cnt
	WHERE
	PC.insurance_file_cnt=@insurance_file_cnt
	ORDER BY coinsurer_count ASC
	
ELSE

	SELECT
	PC.party_cnt,
	P.name,
	PC.coinsurer_percentage,
	PC.coinsurer_cover_percentage,
	--PC.coinsurer_value,
	--PC.coinsurer_ipt_amount,
	--0,
	--coinsurer_commission_amount,
	(SELECT SUM(premium_inc_tax) FROM event_policy_coinsurers_section WHERE event_policy_coinsurers_section.insurance_file_cnt=@insurance_file_cnt AND event_policy_coinsurers_section.party_cnt=PC.party_cnt),
	(SELECT SUM(premium_inc_tax)-SUM(premium_exc_tax) FROM event_policy_coinsurers_section WHERE event_policy_coinsurers_section.insurance_file_cnt=@insurance_file_cnt AND event_policy_coinsurers_section.party_cnt=PC.party_cnt),
	(SELECT SUM(premium_exc_tax) FROM event_policy_coinsurers_section WHERE event_policy_coinsurers_section.insurance_file_cnt=@insurance_file_cnt AND event_policy_coinsurers_section.party_cnt=PC.party_cnt),
	(SELECT SUM(commission_inc_tax) FROM event_policy_coinsurers_section WHERE event_policy_coinsurers_section.insurance_file_cnt=@insurance_file_cnt AND event_policy_coinsurers_section.party_cnt=PC.party_cnt),
	PC.coinsurer_policy_number,
	PC.risk_transfer_agreement,
	PC.isleadunderwriter,
	PC.bureau_party_cnt,
	(SELECT ISNULL(resolved_name,name) FROM party WHERE party_cnt = pc.bureau_party_cnt) 'bureau_name',
	PC.linestands,
	PC.written_line_percentage,  
 	PC.risk_transfer_editable 
	FROM
	event_policy_coinsurers PC
	INNER JOIN party P ON P.party_cnt=PC.party_cnt
	WHERE
	PC.insurance_file_cnt=@insurance_file_cnt
	ORDER BY coinsurer_count ASC
	
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

