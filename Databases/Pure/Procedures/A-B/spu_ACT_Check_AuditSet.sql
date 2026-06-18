SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_AuditSet'
GO


CREATE PROCEDURE spu_ACT_Check_AuditSet
    @auditset_id int OUTPUT
AS


BEGIN
    SELECT @auditset_id = auditset_id
    FROM AuditSet
    WHERE auditset_id = @auditset_id
END
BEGIN
IF @auditset_id = NULL
    SELECT @auditset_id = -1
END
GO


