EXEC DDLDropProcedure 'spu_ACT_Get_PostingStatusForCashListItem'
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE spu_ACT_Get_PostingStatusForCashListItem
	@cashlistitem_id int,
	@IsPosted int OUTPUT
AS

	DECLARE @transdetail_id as int
	DECLARE @document_id as int

	SET @IsPosted = 0

	SELECT @transdetail_id = transdetail_id FROM CashListItem WHERE cashlistitem_id = @cashlistitem_id
	IF ISNULL(@transdetail_id,0) > 0
		IF EXISTS (SELECT transdetail_id FROM TransDetail WHERE transdetail_id = @transdetail_id)
		BEGIN
			SELECT @document_id = document_id FROM TransDetail WHERE transdetail_id = @transdetail_id
			IF ISNULL(@document_id,0) > 0
				IF EXISTS (SELECT document_id FROM Document WHERE document_id = @document_id)
					SET @IsPosted = 1
		END
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO
