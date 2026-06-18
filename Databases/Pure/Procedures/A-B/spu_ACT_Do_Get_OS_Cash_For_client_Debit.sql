SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Do_Get_OS_Cash_For_client_Debit'
GO


CREATE PROCEDURE spu_ACT_Do_Get_OS_Cash_For_client_Debit
    @account_id INT,
    @document_ref VARCHAR(255),
    @source_id INT
AS

SELECT
    MAX(td.currency_id),
    SUM(ISNULL(td.currency_amount, 0)) - SUM(ISNULL(tm.currency_match_amount,0))
FROM document d
JOIN transdetail td
    ON td.document_id = d.document_id
LEFT JOIN transmatch tm 
    ON tm.transdetail_id = td.transdetail_id
WHERE d.document_ref = @document_ref
AND d.company_id = @source_id
AND td.account_id = @account_id


GO


