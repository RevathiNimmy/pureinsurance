Execute DDLDropProcedure 'Spu_sir_external_workflow_usergroups_upd'
GO
 
Create PROCEDURE Spu_sir_external_workflow_usergroups_upd
@npmuser_group_id INT

AS
Begin
INSERT INTO External_Workflow_UserGroups(usergroup_id)Values(@npmuser_group_id)
End


GO

