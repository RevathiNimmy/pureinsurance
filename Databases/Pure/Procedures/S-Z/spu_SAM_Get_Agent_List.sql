SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SAM_Get_Agent_List'
GO

CREATE PROCEDURE spu_SAM_Get_Agent_List

AS

SELECT party_cnt
   FROM party_agent

GO