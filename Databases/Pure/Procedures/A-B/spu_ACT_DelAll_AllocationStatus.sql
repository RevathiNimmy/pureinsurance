SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_DelAll_AllocationStatus'
GO


CREATE PROCEDURE spu_ACT_DelAll_AllocationStatus
AS


DELETE FROM AllocationStatus
GO


