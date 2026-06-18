SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_SelAll_Rep_TB_Det'
GO


CREATE PROCEDURE spu_ACT_SelAll_Rep_TB_Det
AS


SELECT
    rep_TB_det_id,
    rep_TB_id,
    account_id,
    ledger_id,
    account_code,
    account_name,
    short_code,
    credit_amount,
    debit_amount
FROM Rep_TB_Det
GO


