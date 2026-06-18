SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Sel_Agent_payment'
GO


CREATE PROCEDURE spu_ACT_Sel_Agent_payment
    @document_id INT
AS

DECLARE @branch_id INT

/*If not multi-ledger than use branch_id of 1 to get system options.*/
IF NOT EXISTS
    (
        SELECT NULL
        FROM hidden_options
        WHERE branch_id = 1
        AND option_number = 16
        AND value = 1
    )
BEGIN
    SELECT @branch_id = 1
END

SELECT
    td.transdetail_id,
    td.account_id,
    (
        SELECT ISNULL(SUM(amount),0) - ISNULL(SUM(ref_amount),0)
        FROM transdetail
        WHERE document_id = td.document_id
        AND account_id = a.account_id
    )
FROM document d
JOIN transdetail td
    ON td.document_id = d.document_id
JOIN account a
    ON a.account_id = td.account_id
JOIN ledger l
    ON l.ledger_id = a.ledger_id
JOIN party p
    ON p.party_cnt = a.account_key
JOIN transaction_export_folder tef
    ON tef.source_id = d.company_id 
    AND tef.document_ref = d.document_ref
    AND tef.accounts_export_status = 'c'
JOIN event_log el
    ON el.transaction_export_folder_cnt = tef.transaction_export_folder_cnt 
JOIN event_policy_agents epg
    ON epg.insurance_file_cnt = el.event_cnt
    AND epg.agent_cnt = p.party_cnt
WHERE d.document_id = @document_id
AND td.spare = 'AGENT'
AND l.ledger_short_name = 'AG'
AND epg.apply_perc_to_prem_or_comm = 1
AND td.currency_amount <> 
    (
        SELECT ISNULL(SUM(tm.currency_match_amount),0)
        FROM transmatch tm              
        WHERE tm.transdetail_id = td.transdetail_id
    )
AND EXISTS
    (
        SELECT NULL
        FROM system_options so
        WHERE so.branch_id = ISNULL(@branch_id, A.company_id)
        AND so.option_number = 77
        AND so.value = 1
    )


GO
 


