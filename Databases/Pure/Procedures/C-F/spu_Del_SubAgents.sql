SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Del_SubAgents'
GO


CREATE PROCEDURE spu_Del_SubAgents
    @insurance_file_cnt int
AS


DELETE insurance_file_agent
WHERE insurance_file_cnt = @insurance_file_cnt
GO


