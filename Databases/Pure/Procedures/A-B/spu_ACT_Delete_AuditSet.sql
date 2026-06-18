SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_AuditSet'
GO


CREATE PROCEDURE spu_ACT_Delete_AuditSet
    @auditset_id int
AS


DELETE FROM AuditSet
WHERE auditset_id = @auditset_id
GO


