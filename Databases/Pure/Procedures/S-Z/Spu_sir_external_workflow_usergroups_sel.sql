Execute DDLDropProcedure 'Spu_sir_external_workflow_usergroups_sel'
GO

CREATE PROCEDURE Spu_sir_external_workflow_usergroups_sel 



 @dtEffective_date datetime,

 @nSelected_UserGroup_id INT = 0

 AS

     BEGIN

			If @nSelected_UserGroup_id = 0 

				BEGIN



					select pg.pmuser_group_id, pg.code, pg.description, isnull(EWUG.UserGroup_id , 0) As Selected_User_Group_ID, ISNUll(pg.is_sys_admin_group,0) As  is_sys_admin_group,

					ISNUll((Select Schedule_backGroundjob_ForFailure from External_Workflow_Config where External_Workflow_Config_id=1),0) As Schedule_BackGroundForfailure,

					ISNUll((Select External_Workflow_Config_id from External_Workflow_Config where External_Workflow_Config_id=1),0) As External_Workflow_ConfigID,
					ISNULL(pg.is_sys_admin_group,0) As IsSystemAdminGroup

					from pmuser_group pg

					Left JOIN External_Workflow_UserGroups EWUG on EWUG.UserGroup_Id=PG.pmuser_group_id

					where CONVERT(date,pg.effective_date) <= CONVERT(date,@dtEffective_date)					      			 

					and pg.is_deleted = 0

				END

			ELSE

				BEGIN

					select pg.pmuser_group_id, pg.code, pg.description, isnull(EWUG.UserGroup_id , 0) As Selected_User_Group_ID, ISNUll(pg.is_sys_admin_group,0) is_sys_admin_group,

					ISNUll((Select Schedule_backGroundjob_ForFailure from External_Workflow_Config where External_Workflow_Config_id=1),0) As Schedule_BackGroundForfailure,

					ISNUll((Select External_Workflow_Config_id from External_Workflow_Config where External_Workflow_Config_id=1),0) As External_Workflow_ConfigID,
					ISNULL(pg.is_sys_admin_group,0) As IsSystemAdminGroup

					from pmuser_group pg

					Left JOIN External_Workflow_UserGroups EWUG on EWUG.UserGroup_Id=PG.pmuser_group_id

					where CONVERT(date,pg.effective_date) <= CONVERT(date,@dtEffective_date)		 

					And ISNULL(EWUG.UserGroup_id , 0) = @nSelected_UserGroup_id

					and pg.is_deleted = 0

				END





    END
GO


