Execute DDLDropProcedure 'Spu_sir_external_workflow_usergroups_Del'
GO
 

CREATE PROCEDURE Spu_sir_external_workflow_usergroups_Del 

	@nExternal_Workflow_Groupid as integer=0,
	@npmuser_group_id as integer=0
	
AS
BEGIN
	DELETE FROM External_WorkFlow_usergroups
	WHERE 	usergroup_id = case When @npmuser_group_id=0 then usergroup_id Else  @npmuser_group_id End
END




GO

