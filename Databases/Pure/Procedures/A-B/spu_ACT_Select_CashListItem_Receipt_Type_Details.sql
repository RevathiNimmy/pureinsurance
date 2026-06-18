SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Select_CashListItem_Receipt_Type_Details'
GO

CREATE PROCEDURE spu_ACT_Select_CashListItem_Receipt_Type_Details

@cashlistitem_receipt_type_id int 

AS

BEGIN
	SELECT 
		cashlistitem_receipt_type_id, 
		description, 
		code, 
		is_instalment 

	FROM cashlistitem_receipt_type
	WHERE cashlistitem_receipt_type_id = @cashlistitem_receipt_type_id
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
