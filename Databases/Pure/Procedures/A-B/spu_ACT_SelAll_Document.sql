SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_SelAll_Document'
GO


CREATE PROCEDURE spu_ACT_SelAll_Document
    @sub_branch_id int = NULL
AS


IF @sub_branch_id IS NULL
    SELECT document_id,
           company_id,
           postingstatus_id,
           documenttype_id,
           auditset_id,
           batch_id,
           document_ref,
           document_date,
           created_date,
           authorised_date,
           comment,
           write_off_reason_id
    FROM   Document
ELSE
    SELECT document_id,
           company_id,
           postingstatus_id,
           documenttype_id,
           auditset_id,
           batch_id,
           document_ref,
           document_date,
           created_date,
           authorised_date,
           comment,
           write_off_reason_id
    FROM   Document
    WHERE  sub_branch_id = @sub_branch_id


GO


