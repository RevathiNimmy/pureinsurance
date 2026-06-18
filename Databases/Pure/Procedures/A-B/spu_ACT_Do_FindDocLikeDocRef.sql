SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Do_FindDocLikeDocRef'
GO


CREATE PROCEDURE spu_ACT_Do_FindDocLikeDocRef
    @document_ref varchar(25),
    @sub_branch_id int
AS


SELECT document_ref,
       document_date,
       documenttype_id,
       postingstatus_id,
       comment
FROM   Document
WHERE  document_ref like @document_ref +'%'
AND    sub_branch_id = @sub_branch_id
ORDER BY document_ref


GO


