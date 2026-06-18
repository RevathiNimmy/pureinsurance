SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_StatementDetail'
GO


CREATE PROCEDURE spu_ACT_Update_StatementDetail
    @statementdetail_id int,
    @statement_id int,
    @transdetail_id int,
    @os_base_amount numeric(19,4),
    @os_currency_amount numeric(19,4),
    @currency_id smallint,
    @document_id int,
    @document_sequence smallint,
    @documenttype_id smallint,
    @document_ref varchar(25),
    @accounting_date datetime,
    @amount numeric(19,4),
    @fully_matched bit,
    @currency_amount numeric(19,4),
    @currency_base_xrate numeric(12,8),
    @comment varchar(60),
    @project varchar(20),
    @contract varchar(20),
    @product varchar(20),
    @department varchar(20),
    @agent varchar(20),
    @client varchar(20),
    @ref_date datetime,
    @ref_amount numeric(19,4),
    @ref_quantity numeric(19,6),
    @ref_units varchar(30)
AS


BEGIN
UPDATE StatementDetail
    SET
    statement_id=@statement_id,
    transdetail_id=@transdetail_id,
    os_base_amount=@os_base_amount,
    os_currency_amount=@os_currency_amount,

    currency_id=@currency_id,
    document_id=@document_id,
    document_sequence=@document_sequence,
    documenttype_id=@documenttype_id,
    document_ref=@document_ref,
    accounting_date=@accounting_date,
    amount=@amount,
    fully_matched=@fully_matched,
    currency_amount=@currency_amount,
    currency_base_xrate=@currency_base_xrate,
    comment=@comment,
    project=@project,
    contract=@contract,
    product=@product,
    department=@department,
    agent=@agent,
    client=@client,
    ref_date=@ref_date,
    ref_amount=@ref_amount,
    ref_quantity=@ref_quantity,
    ref_units=@ref_units
WHERE statementdetail_id = @statementdetail_id
END
GO


