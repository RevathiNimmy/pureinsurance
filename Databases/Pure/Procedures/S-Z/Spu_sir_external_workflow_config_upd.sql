Execute DDLDropProcedure 'Spu_sir_external_workflow_config_upd'
GO
 
CREATE PROCEDURE Spu_sir_external_workflow_config_upd 

@nExternal_WorkFlow_Config_ID As Integer=0,

@bEnablebackgroundjob_ForFailure TinyINT

AS
Begin


IF NOT EXISTS ( select top 1 NULL from External_Workflow_Config)

BEGIN
      INSERT INTO External_Workflow_Config (Schedule_backGroundjob_ForFailure) Values (@bEnablebackgroundjob_ForFailure)
end

Else

Begin
       Update  External_Workflow_Config Set Schedule_backGroundjob_ForFailure=@bEnablebackgroundjob_ForFailure Where External_Workflow_Config_id=@nExternal_WorkFlow_Config_ID
End
END

GO

