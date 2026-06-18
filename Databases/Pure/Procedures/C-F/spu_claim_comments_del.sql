SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_claim_comments_del'
GO

CREATE PROCEDURE spu_claim_comments_del
 @claim_id int,
 @comment_type int,
 @entity_id int
AS
DELETE FROM Claim_Comments
 WHERE claim_id = @claim_id
AND comment_type = @comment_type
AND entity_id = @entity_id

GO

