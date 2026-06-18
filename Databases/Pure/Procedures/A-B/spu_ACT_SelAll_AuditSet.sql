SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_SelAll_AuditSet'
GO


CREATE PROCEDURE spu_ACT_SelAll_AuditSet
    @cashlistitem_id int = 0
AS
IF IsNull(@cashlistitem_id,0) = 1 

BEGIN
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
WHERE cashlistitem_id = @cashlistitem_id
END
ELSE
BEGIN
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
END
GO


