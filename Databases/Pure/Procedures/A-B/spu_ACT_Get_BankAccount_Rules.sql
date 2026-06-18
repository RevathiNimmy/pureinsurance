SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Get_BankAccount_Rules'
GO

CREATE PROCEDURE spu_ACT_Get_BankAccount_Rules
@bankaccountid int
AS
select
r.bankaccount_rules_id,
r.bankaccount_id,
r.mediatype_id,
m.description,
r.match_to_transdetail,
r.match_account_code,
r.code_is_merchant_number,
r.match_batch_number,
r.batch_is_remit_code,
r.match_cheque_number,
r.match_amount,
r.match_date,
r.skip_if_reason_null,
r.active
from 
bankaccount_rules r,
mediatype m
where
r.mediatype_id = m.mediatype_id
and
bankaccount_id = @bankaccountid
GO
