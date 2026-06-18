SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Sel_Reversal'
GO


CREATE PROCEDURE spu_ACT_Sel_Reversal
    @company_id int,    
    @document_ref varchar(255),
    @account_id int
AS

    SELECT T.transdetail_id,
           T.currency_amount
    FROM   Document D
    JOIN   Transdetail T ON D.document_id = T.document_id
    WHERE  D.document_ref = @document_ref
    AND    D.company_id = @company_id
    AND    T.account_id =@account_id


GO


