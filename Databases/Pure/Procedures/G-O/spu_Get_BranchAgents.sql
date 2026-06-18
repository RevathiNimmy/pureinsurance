SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Get_BranchAgents'
GO


CREATE Procedure spu_Get_BranchAgents  
@Branchid int  
As  
Select PAB.party_cnt,name from Party P    
Inner Join Party_Agent_branch PAB On P.party_cnt=PAB.party_cnt 
Inner Join Party_Agent PA On PA.party_cnt=PAB.party_cnt    
Where PAB.source_id=@Branchid    
And P.is_deleted=0 
And PA.party_agent_type_id<>2 --filter out Sub Agents     
Order By Name    
  

GO
