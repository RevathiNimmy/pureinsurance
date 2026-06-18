SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Get_AccountIDs_From_BatchSourceCode'
GO

CREATE PROCEDURE spu_ACT_Get_AccountIDs_From_BatchSourceCode
@code varchar(30)
AS
select 
ba.bankaccount_id, ba.account_id, bs.batch_source_id 
from
batch_source bs, bankaccount ba
where
bs.code = @code
and
bs.bankaccount_id = ba.bankaccount_id
GO
