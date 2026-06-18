SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_Commission_Movement'
GO

CREATE PROCEDURE spu_Get_Commission_Movement
    @effective_date DATETIME
AS

SELECT
    d.document_ref,
    d.document_id,
    d.company_id 
FROM suspended_accounts_transactions sat
JOIN transdetail td
    ON td.transdetail_id = sat.suspended_transdetail_id 
JOIN document d
    ON d.document_id = td.document_id
JOIN transaction_export_folder tef
    ON tef.document_ref = d.document_ref
    AND tef.source_id = d.company_id
    AND tef.accounts_export_status = 'c'
WHERE DATEDIFF(d, tef.effective_date, @effective_date) >= 0
AND sat.is_deleted = 0
AND sat.linked_transdetail_id = 0
GROUP BY 
    d.document_ref,
    d.document_id,
    d.company_id
ORDER BY 
    d.company_id,
    d.document_id


GO
