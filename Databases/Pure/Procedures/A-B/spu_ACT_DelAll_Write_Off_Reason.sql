SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_DelAll_Write_Off_Reason'
GO


CREATE PROCEDURE spu_ACT_DelAll_Write_Off_Reason
AS


DELETE FROM Write_Off_Reason
GO


