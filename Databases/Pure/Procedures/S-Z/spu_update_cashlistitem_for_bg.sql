SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_update_cashlistitem_for_bg'
GO

CREATE PROCEDURE spu_update_cashlistitem_for_bg
	@bg_id			INT,
	@cashList_id		INT,
	@cashListItem_id	INT,
	@insurance_file_cnt	INT,
	@amt_to_be_posted	Numeric(20,2)

AS

BEGIN    
	INSERT INTO	CashListItem_BG(bg_id,	
				cashList_id,
				cashListItem_id,
				insurance_file_cnt,
				amt_to_be_posted)
			VALUES	(@bg_id,	
				@cashList_id,
				@cashListItem_id,
				@insurance_file_cnt,
				@amt_to_be_posted
				)		

END
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

