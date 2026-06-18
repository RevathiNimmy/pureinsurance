SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_StatementDetail'
GO


CREATE PROCEDURE spu_ACT_Select_StatementDetail
    @statementdetail_id int
AS


SELECT
    statementdetail_id,
    statement_id,
    transdetail_id,
    os_base_amount,
    os_currency_amount,
    currency_id,
    document_id,
    document_sequence,
    documenttype_id,
    document_ref,
    accounting_date,
    amount,
    fully_matched,
    currency_amount,
    currency_base_xrate,
    comment,
    project,
    contract,
    product,
    department,
    agent,
    client,
    ref_date,
    ref_amount,
    ref_quantity,
    ref_units
FROM StatementDetail
WHERE statementdetail_id = @statementdetail_id
GO


