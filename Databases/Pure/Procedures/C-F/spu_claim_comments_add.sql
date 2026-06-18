SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_claim_comments_add'
GO

CREATE PROCEDURE spu_claim_comments_add
 @claim_id int,
 @comment_type int,
 @entity_id int,
 @claim_comment_id int,
 @comment_line varchar(255)
AS
INSERT INTO Claim_Comments (claim_id, comment_type, entity_id, claim_comment_id, comment_line)
VALUES (@Claim_id, @comment_type, @entity_id, @claim_comment_id, @comment_line)

GO
