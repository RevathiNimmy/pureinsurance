Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
<System.Runtime.InteropServices.ProgId("NavProcConst_NET.NavProcConst")> _
 Public Module NavProcConst
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	'***********************************************************
	'***********************************************
	' TO DO: GLOBAL VARIABLE IN BAS MODULE
	'' Public constants for NavMapItem
	'***********************************************
	
	Public Const NavGrpKey As String = "Keys"
	Public Const NavGrpComponent As String = "Components"
	Public Const NavGrpProcess As String = "Processes"
	Public Const NavGrpMap As String = "Maps"
	
	Public Const NavGrpStep As String = "Steps"
	Public Const NavGrpRoadMap As String = "RoadMap"
	
	'Field Validation constants
	
	Public Const ACText As Integer = 0
	Public Const ACNumeric As Integer = 1
	Public Const ACDate As Integer = 2
	Public Const ACTextLookUp As Integer = 3
	
	'LookUp Constants
	
	Public Const ACLTabPMNav_Map As Integer = 0
	Public Const ACLTabPMNav_Component As Integer = 1
	Public Const ACLTabPMNav_Process As Integer = 2
	Public Const ACLTabPMNav_Step As Integer = 3
	Public Const ACLTabPMProduct As Integer = 4
	Public Const ACLTabPMProc_Lock_Group As Integer = 5
	Public Const ACLTabTransaction_Type As Integer = 6
	Public Const ACLTabPMCaption As Integer = 7
	
	Public Const ACLTasks As Integer = 8
	Public Const ACLNavigateStatus As Integer = 9
	Public Const ACLActions As Integer = 10
	Public Const ACLProcessModes As Integer = 11
	Public Const ACLComponentTypes As Integer = 12
	Public Const ACLTabPMNav_StartMap As Integer = 13
	Public Const ACLTabPMNav_SubMap As Integer = 14
	Public Const ACLNavigatorModes As Integer = 15
	
	'Process Map Constants
	Public Const ACRoot As String = "RR"
	Public Const ACRootProcess As String = "RP"
	Public Const ACRootComponent As String = "RC"
	Public Const ACRootMap As String = "RM"
	Public Const ACProcessMap As String = "PM"
	Public Const ACMapStep As String = "MS"
	Public Const ACStepMap As String = "SM"
	Public Const ACStepComponent As String = "SC"
	
	' Node levels
	Public Const NodeProcess As Integer = 1
	Public Const NodeMap As Integer = 2
	Public Const NodeComponent As Integer = 3
	Public Const NodeSubMap As Integer = 4
	Public Const NodeStepFindForm As Integer = 5
	Public Const NodeStepDecisionForm As Integer = 6
	Public Const NodeStepDataForm As Integer = 7
	Public Const NodeStepBusinessObject As Integer = 8
	Public Const NodeStepSubMap As Integer = 9
	
	
	'Get/Set Keys Constants
	Public Const ACSetKey As String = "SK"
	Public Const ACGetKey As String = "GK"
	
	'Constants for outputing copy scripts
	Public Const ACTAB As String = "    "
	Public Const ACSQLPush As String = "PUSH"
	Public Const ACSQLPull As String = "PULL"
	
	
	Public Const ACSQLMAXFILELENGTH As Integer = 50000
	
	Public Const ACSQLMAXFILES As Integer = 100
	Public Const ACMAXCAT As Integer = 100
	
	Public Const ACSQLScriptFileName As String = "dbo.spu_pm_insert_process"
	Public Const ACSQLScriptDirectory As String = "C:\"
	
	Public Const ACSQLStart As Integer = 0
	Public Const ACSQLComment As Integer = 1
	Public Const ACSQLExecuteSP As Integer = 2
	Public Const ACSQLStatement As Integer = 3
	Public Const ACSQLStatementBlock As Integer = 4
	Public Const ACSQLComplete As Integer = 5
	Public Const ACSQLBegin As Integer = 6
	
	'Double Carriage return
	Public Const vbCrLf2 As String = Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)
    'Modified by Deepak Sharma on 4/20/2010 2:52:32 PM refer developer guide no. 29(No Solutions)
    'Shared Sub New()
    '	MainModule.JustForInvokeMain()

End Module