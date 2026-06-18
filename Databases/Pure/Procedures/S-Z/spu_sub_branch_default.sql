/* Select default sub_branch record for a given branch (source_id) */
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_sub_branch_default'
GO


CREATE PROCEDURE spu_sub_branch_default

@source_id int,
@sub_branch_id int OUTPUT

AS

SELECT  @sub_branch_id=MIN(sub_branch_id)
FROM    sub_branch
WHERE   source_id=@source_id
