SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_PartyBank_Details_DelDB'
GO
CREATE PROCEDURE spu_PartyBank_Details_DelDB
	    @Party_bank_id	INT
AS  
BEGIN

	DELETE FROM party_bank_history 
	WHERE  Party_bank_id=@Party_bank_id

	DELETE FROM party_bank 
	WHERE  Party_bank_id=@Party_bank_id

END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO