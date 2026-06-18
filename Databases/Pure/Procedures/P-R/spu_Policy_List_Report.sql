SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Policy_List_Report'
GO

CREATE PROCEDURE spu_Policy_List_Report
    @language_id INT
AS

DECLARE @insurance_file_status_id   INT,
    @policy_status  VARCHAR(20),
    @account_handler_cnt    INT,
    @account_handler    VARCHAR(20)

INSERT INTO ReportPolicyList
SELECT  ifi.insurance_file_cnt,
    p.shortname,
    p.resolved_name,
    ifi.insurance_ref,
    i.shortname insurer_code,
    i.resolved_name insurer_name,
    c.caption risk_group,
    ifi.cover_start_date effective_date,
    ifi.this_premium gross_premium,
    ifi.brokerage_amount commission,
    ifi.insurance_file_status_id,
    "" policy_status,
    c2.caption policy_type,
    ifi.account_handler_cnt,
    "" account_handler
FROM    insurance_file ifi,
    party p,
    party i,
    risk_code rc,
    risk_group rg,
    pmcaption c,
    insurance_file_type ift,
    pmcaption c2
WHERE   ifi.insured_cnt = p.party_cnt
AND p.is_deleted = 0
AND ifi.lead_insurer_cnt = i.party_cnt
AND ifi.risk_code_id = rc.risk_code_id
AND rc.risk_group_id = rg.risk_group_id
AND rg.caption_id = c.caption_id
AND c.language_id = @language_id
AND ifi.policy_ignore IS NULL
AND ifi.insurance_file_type_id = ift.insurance_file_type_id
AND ift.caption_id = c2.caption_id
AND c2.language_id = @language_id

DECLARE List_Cursor CURSOR FAST_FORWARD FOR
    SELECT  DISTINCT insurance_file_status_id
    FROM    ReportPolicyList
    WHERE   insurance_file_status_id IS NOT NULL

OPEN List_Cursor
FETCH NEXT FROM List_Cursor INTO @insurance_file_status_id

WHILE @@FETCH_STATUS = 0 BEGIN
    SELECT  @policy_status = NULL

    SELECT  @policy_status = c.caption
    FROM    insurance_file_status ifs,
        pmcaption c
    WHERE   ifs.insurance_file_status_id = @insurance_file_status_id
    AND ifs.caption_id = c.caption_id
    AND c.language_id = @language_id

    IF @policy_status IS NOT NULL BEGIN
        UPDATE  ReportPolicyList
        SET policy_status = @policy_status
        WHERE   insurance_file_status_id = @insurance_file_status_id
    END

    FETCH NEXT FROM List_Cursor INTO @insurance_file_status_id
END

CLOSE List_Cursor
DEALLOCATE List_Cursor

UPDATE  ReportPolicyList
SET policy_status = "Live"
WHERE   insurance_file_status_id IS NULL

DECLARE List_Cursor2 CURSOR FAST_FORWARD FOR
    SELECT  DISTINCT account_handler_cnt
    FROM    ReportPolicyList
    WHERE   account_handler_cnt IS NOT NULL

OPEN List_Cursor2
FETCH NEXT FROM List_Cursor2 INTO @account_handler_cnt

WHILE @@FETCH_STATUS = 0 BEGIN
    SELECT  @account_handler = NULL

    SELECT  @account_handler = p.shortname
    FROM    party p
    WHERE   p.party_cnt = @account_handler_cnt

    IF @account_handler IS NOT NULL BEGIN
        UPDATE  ReportPolicyList
        SET account_handler = @account_handler
        WHERE   account_handler_cnt = @account_handler_cnt
    END

    FETCH NEXT FROM List_Cursor2 INTO @account_handler_cnt
END

CLOSE List_Cursor2
DEALLOCATE List_Cursor2

UPDATE  ReportPolicyList
SET account_handler = "None"
WHERE   account_handler = ""

UPDATE  ReportPolicyList
SET commission = 0
WHERE   commission IS NULL

SELECT  insurance_file_cnt,
    shortname,
    resolved_name,
    insurance_ref,
    insurer_code,
    insurer_name,
    risk_group,
    effective_date,
    gross_premium,
    commission,
    policy_status,
    policy_type,
    account_handler
FROM    ReportPolicyList
ORDER BY shortname,
     insurance_ref

DELETE FROM ReportPolicyList

GO

