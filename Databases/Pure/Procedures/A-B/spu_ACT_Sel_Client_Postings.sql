SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Sel_Client_Postings'
GO


CREATE PROCEDURE spu_ACT_Sel_Client_Postings
    @company_id int,
    @document_ref varchar(255),
    @sub_branch_id int = NULL
AS

    -- TF160903 - PN6873 
    -- sub-branch not relevant for this procedure

    -- Get default sub_branch if we weren't passed one
--    IF @sub_branch_id IS NULL
--        EXEC spu_sub_branch_default @company_id, @sub_branch_id OUTPUT

    -- The original query (nearly)
    SELECT T.account_id,
           T.transdetail_id,
           T.currency_amount
    FROM   Document D
    JOIN   Transdetail T ON D.document_id = T.document_id
    JOIN   Account A ON T.account_id = A.account_id
    JOIN   Ledger L ON A.ledger_id = L.ledger_id
    WHERE  D.document_ref = @document_ref
--  AND    D.sub_branch_id = @sub_branch_id
    AND    D.company_id = @company_id
    AND    L.ledger_name = 'Client'

GO


