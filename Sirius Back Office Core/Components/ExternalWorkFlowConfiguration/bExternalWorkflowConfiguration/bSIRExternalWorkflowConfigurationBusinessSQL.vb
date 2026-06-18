Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
    ' ***************************************************************** '
    ' Class Name: BusinessSQL
    ' Date: 10/07/2014
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIRExternalWorkFlowSql.Business class.
    '
    ' Edit History:
    ' ***************************************************************** '

  
    Public Const KAddExternalWorkFlowConfiguration_UsergroupsStored As Boolean = True
    Public Const KAddExternalWorkFlowConfiguration_UsergroupName As String = "AddExternalWorkFlowConfiguration_Usergroups"
    Public Const KAddExternalWorkFlowConfiguration_UsergroupSQL As String = "Spu_sir_external_workflow_usergroups_upd"

    Public Const KDelExternalWorkFlowConfiguration_UsergroupStored As Boolean = True
    Public Const KDeExternalWorkFlowConfiguration_UsergroupName As String = "DeleteExternalWorkFlowConfiguration_Usergroups"
    Public Const KDelExternalWorkFlowConfiguration_UsergroupSQL As String = "Spu_sir_external_workflow_usergroups_Del"
	
    Public Const KSelExternalWorkFlowConfiguration_UsergroupStored As Boolean = True
    Public Const KSelExternalWorkFlowConfiguration_UsergroupName As String = "SelExternalWorkFlowConfiguration_Usergroups"
    Public Const KSelExternalWorkFlowConfiguration_UsergroupsSQL As String = "Spu_sir_external_workflow_usergroups_sel"

    Public Const KAddspu_SIR_ExternalWorkFlowConfigStored As Boolean = True
    Public Const KAddspu_SIR_ExternalWorkFlowConfigName As String = "Addspu_SIR_ExternalWorkFlowConfig_Upd"
    Public Const KAddspu_SIR_ExternalWorkFlowConfigSQL As String = "Spu_sir_external_workflow_config_upd"
End Module