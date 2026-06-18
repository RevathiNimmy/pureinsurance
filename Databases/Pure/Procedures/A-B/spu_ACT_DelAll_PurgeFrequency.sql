SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_DelAll_PurgeFrequency'
GO


CREATE PROCEDURE spu_ACT_DelAll_PurgeFrequency
AS


DELETE FROM PurgeFrequency
GO


