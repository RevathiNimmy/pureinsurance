SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_GetAgents_for_Tasks'
GO

CREATE procedure spu_GetAgents_for_Tasks
@User_ID INT,
@Current_Party_Cnt INT OUTPUT
AS
declare @party_cnt as int
select @party_cnt=p.party_cnt from party p join PMUser PM on PM.party_cnt=p.party_cnt
where p.party_type_id=3 and pm.user_id=@User_ID
if @party_cnt is NULL
set @Current_Party_Cnt=0
else
set @Current_Party_Cnt= @party_cnt
select Party_Cnt, shortname from Party Where Party_type_id=3

GO

