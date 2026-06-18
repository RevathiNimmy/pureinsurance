SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_Rep_TB_Det'
GO


CREATE PROCEDURE spu_ACT_Update_Rep_TB_Det
    @rep_TB_det_id int,
    @rep_TB_id int,
    @account_id int,
    @ledger_id smallint,
    @account_code varchar(255),
    @account_name varchar(60),
    @short_code char(20),
    @credit_amount numeric(19,4),
    @debit_amount numeric(19,4)
AS


BEGIN
UPDATE Rep_TB_Det
    SET
    rep_TB_id=@rep_TB_id,
    account_id=@account_id,
    ledger_id=@ledger_id,
    account_code=@account_code,
    account_name=@account_name,
    short_code=@short_code,
    credit_amount=@credit_amount,
    debit_amount=@debit_amount
WHERE rep_TB_det_id = @rep_TB_det_id
END
GO


