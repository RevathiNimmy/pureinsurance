SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Transaction_Comment_Sel'
GO


CREATE PROCEDURE spu_ACT_Transaction_Comment_Sel
    @transdetail_id 	int,
    @cashlistitem_id 	int
AS

--Select all comments for the given transaction
IF ISNULL(@transdetail_id,0) <> 0 

	SELECT  tc.transaction_comment_id,
		tc.transdetail_id,
		tc.cashlistitem_id,
		tc.description,
		tc.comment_date,
		u.username
	FROM 	transaction_comment tc
	JOIN 	PMUser u
	ON 	tc.user_id = u.user_id
	WHERE   tc.transdetail_id = @transdetail_id
	ORDER BY tc.comment_date DESC

ELSE
--Select all comments for the given cashlistitem
	SELECT  tc.transaction_comment_id,
		tc.transdetail_id,
		tc.cashlistitem_id,
		tc.description,
		tc.comment_date,
		u.username
	FROM 	transaction_comment tc
	JOIN 	PMUser u
	ON 	tc.user_id = u.user_id
	WHERE   tc.transdetail_id = @cashlistitem_id
	ORDER BY tc.comment_date DESC
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF 
GO
