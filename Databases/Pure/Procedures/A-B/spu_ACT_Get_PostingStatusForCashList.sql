EXEC DDLDropProcedure 'spu_ACT_Get_PostingStatusForCashList'
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE spu_ACT_Get_PostingStatusForCashList
	@nCashlistID int,
	@nIsPosted int OUTPUT
AS

	DECLARE @transdetail_id as int
	DECLARE @document_id as int

	SET @nIsPosted = 0
	SET @transdetail_id = 0
	
	SELECT TOP 1 @transdetail_id = cli.transdetail_id FROM CashListItem cli
		Inner Join CashList cl On cl.cashlist_id = cli.cashlist_id
			WHERE cl.cashlist_id = @nCashlistID And ISNULL(cli.transdetail_id, 0) > 0
	IF ISNULL(@transdetail_id,0) > 0
		SET @nIsPosted = 1
		
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO
