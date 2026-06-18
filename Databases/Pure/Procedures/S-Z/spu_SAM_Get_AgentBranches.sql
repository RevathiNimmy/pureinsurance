SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_SAM_Get_AgentBranches'
GO


CREATE Procedure spu_SAM_Get_AgentBranches  
@Party_CNT int  
As  
SELECT s.source_id, s.[code], s.[description], s.country_id 
FROM   source s
JOIN  Party_Agent_branch PAB ON S.source_id =PAB.source_id 
WHERE PAB.party_cnt =@Party_CNT


GO
