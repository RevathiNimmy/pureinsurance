SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_AuditSet'
GO


CREATE PROCEDURE spu_ACT_Select_AuditSet
    @auditset_id int
AS


SELECT
    auditset_id,
    company_id,
    user_id,
    posted_date,
    comment,
    document_id,
    auditset_type_id,
    approved_date,
    approved_user_id,
    rejected,
    rejected_user_id,
    cashlistitem_id
FROM AuditSet
WHERE auditset_id = @auditset_id
GO


