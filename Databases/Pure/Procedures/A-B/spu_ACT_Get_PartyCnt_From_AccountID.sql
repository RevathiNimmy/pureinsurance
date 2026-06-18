SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Get_PartyCnt_From_AccountID'
GO


CREATE PROCEDURE spu_ACT_Get_PartyCnt_From_AccountID
@accountid int
 AS
select account_key from account where account_id = @accountid

GO
