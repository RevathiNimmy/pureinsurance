SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Party_agent_Branch_Add'
GO


Create procedure spu_Party_agent_Branch_Add
	@Party_cnt bigint,
	@BranchID int
as
	Insert into Party_agent_Branch (Party_cnt,Source_id) 
	Values(@Party_cnt,@BranchID)