SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_AuditSetType'
GO


CREATE PROCEDURE spu_ACT_Select_AuditSetType
    @AuditSet_TypeCode varchar(20)
AS


SELECT
    auditset_type_id,
    code,
    description,
    caption_id,
    effective_date,
    is_deleted
FROM AuditSet_Type
WHERE code = @AuditSet_TypeCode
GO


