SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_ACT_Update_CashList_For_SplitReceipt'
GO

CREATE  PROCEDURE spu_ACT_Update_CashList_For_SplitReceipt
    @CashList_Id INT,
	@Status TINYINT
	
AS	
BEGIN

	UPDATE CashList set 
	is_split_receipt = @Status 
	where cashlist_id = @CashList_Id


END
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO