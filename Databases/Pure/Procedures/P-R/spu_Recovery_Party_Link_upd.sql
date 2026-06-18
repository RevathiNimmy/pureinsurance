SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure spu_Recovery_Party_Link_upd
GO

CREATE PROCEDURE spu_Recovery_Party_Link_upd
    @recovery_id int,
    @recovery_party_cnt int,
    @recovery_party_type_id int
AS

	UPDATE Recovery 
		SET recovery_party_cnt=@recovery_party_cnt, 
		recovery_party_type_id=@recovery_party_type_id
	WHERE Recovery_id=@recovery_id


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

