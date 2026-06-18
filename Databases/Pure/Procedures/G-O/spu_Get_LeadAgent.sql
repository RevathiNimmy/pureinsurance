SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_LeadAgent'
GO

CREATE PROCEDURE spu_Get_LeadAgent
   @insurance_file_cnt Int
AS
SELECT lead_agent_cnt 
FROM insurance_file ifl
WHERE ifl.insurance_file_cnt=@insurance_file_cnt
GO
