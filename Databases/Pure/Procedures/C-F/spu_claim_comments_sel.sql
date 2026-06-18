SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_claim_comments_sel'
GO

CREATE PROCEDURE spu_claim_comments_sel
 @claim_id int,
 @comment_type int,
 @entity_id int
AS
SELECT
    comment_line
FROM Claim_Comments
WHERE claim_id = @claim_id
AND comment_type = @comment_type
AND entity_id = @entity_id
ORDER BY claim_comment_id ASC

GO

