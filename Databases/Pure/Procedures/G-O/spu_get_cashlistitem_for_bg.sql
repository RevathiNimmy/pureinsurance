SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_get_cashlistitem_for_bg'
GO

CREATE PROCEDURE spu_get_cashlistitem_for_bg
	@cashlist_id	Int
AS


	SELECT	bg_id,
		cashList_id,
		cashListItem_id,
		insurance_file_cnt,
		amt_to_be_posted

	FROM	CashListItem_BG
	WHERE	CashList_Id = @cashlist_id
	
	
	GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

