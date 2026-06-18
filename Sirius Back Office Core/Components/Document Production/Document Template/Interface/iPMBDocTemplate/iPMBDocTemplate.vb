Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Runtime.InteropServices
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 10/05/1999
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	Public Declare Function FindWindow Lib "user32.dll"  Alias "FindWindowA"(ByVal lpClassName As String, ByVal lpWindowName As String) As Integer
	Public Declare Function IsWindow Lib "user32.dll" (ByVal hwnd As Integer) As Integer
	Public Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Integer)
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMBDocTemplate"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACLblCode As Integer = 102
	Public Const ACLblDescription As Integer = 103
	Public Const ACLblType As Integer = 104
	Public Const ACLblSlot As Integer = 105
	Public Const ACLblRisk As Integer = 106
	Public Const ACLblGroup As Integer = 107
	Public Const ACLblTypeEditable As Integer = 108
	Public Const ACLblEditable As Integer = 109
	Public Const ACLblBranch As Integer = 110
	'RWH(21/08/2000) RSAIB Process 12
	Public Const ACTabTitle2 As Integer = 111
	Public Const AClvwHeader1 As Integer = 112
	Public Const AClvwHeader2 As Integer = 113
	Public Const AClvwHeader3 As Integer = 114
	
	Public Const ACLblDocument_Filter As Integer = 115
	Public Const ACLblVisibleFromWeb As Integer = 116
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	Public Const ACEditButton As Integer = 204
	Public Const ACDeleteButton As Integer = 205
	Public Const ACUndeleteButton As Integer = 206
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	Public Const ACAddDetailsTitle As Integer = 304
	Public Const ACAddDetails As Integer = 305
	Public Const ACMailLogonFailed As Integer = 306
	Public Const ACUnsupportedWordVersion As Integer = 307
	Public Const AClblEffectiveDate As Integer = 308
	
	Public Const ACArchiveSilentMode As Integer = 8
	Public Const ACPrintOnlySilentMode As Integer = 9
	Public Const ACPrintSpoolArchiveSilentMode As Integer = 10
	Public Const ACPrintSpoolSilentMode As Integer = 11
	Public Const ACSpoolArchiveSilentMode As Integer = 12
	Public Const ACSpoolSilentMode As Integer = 13

    Public Const ACEmailOnlySilent As Integer = 14

    ' Menus
    'Public Const ACUserChoice = 15           ' To support user choice



    ' {* USER DEFINED CODE (End) *}

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    'PR 7/11/2002 Start
    'Previously moved to gSIRLibrary, so
    'need removing here.
    '
    'Public Const ACNormalMode = 0
    'Public Const ACMergeMode = 1
    '' CTAF 130600 - Modes for printing
    'Public Const ACPrintMode = 2
    'Public Const ACPrintSilentMode = 3
    '
    ''TN20010130 Start
    'Public Const ACSpoolDocMode = 4
    'Public Const ACSpoolReportMode = 5
    ''TN20010130 End
    'PR 7/11/2002 End

    ' Public source and language ID's from the
    ' Object Manager.

    <ThreadStatic()> _
 Public g_iSourceID As Integer

    <ThreadStatic()> _
 Public g_iLanguageID As Integer

    <ThreadStatic()> _
 Public g_iUserID As Integer

    <ThreadStatic()> _
 Public g_oZipper As bPMZipper.Business


    <ThreadStatic()> _
 Public g_oBusiness As bSIRDocTemplate.Business

    ' Public instance of the object manager.

    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	'UPGRADE_NOTE: (1053) g_sProductFamily was changed from a Constant to a Variable. More Information: http://www.vbtonet.com/ewis/ewi1053.aspx
	Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	
	'Public Const ScreenHelpID As Integer = 4017
	
	' KB 310701 4017 is help for Find Document Template, 4072 is the template itself
	
	Public Const ScreenHelpID As Integer = 4072
	
	'DC260104 PN9904 added to fix problem editting uniplex document
	Public Const DbTag As String = "DB"
	Public Const TableTag As String = "TBL"
	Public Const FieldTag As String = "FLD"
	Public Const LoopTag As String = "LOOP"
	Public Const EndLoopTag As String = "ENDLOOP"
	Public Const FileTag As String = "FILE"
	Public Const QuestionTag As String = "KEY0"
	Public Const MandQuestionTag As String = "KEY0M"
	Public Const Separator As String = "_"
	Public Const ClauseTag As String = "CL" 'RWH(17/08/2000) RSAIB Process 12.
	Public Const RiskLoopTag As String = "RSKLOOP" 'RWH(12/09/2000) RSAIB Process 28.
	Public Const RiskHeaderTag As String = "RSKHEADER" 'RWH(12/09/2000) RSAIB Process 28.
	Public Const RiskDocPrefix As String = "RK" 'RWH(12/09/2000) RSAIB Process 28.
	Public Const StandardWordingsTag As String = "STANDARDWORDINGS" 'RWH(23/04/2001) RSAIB Process 28.
	Public Const StandardWordingNPTag As String = "STANDARDWORDINGSNP"
	Public Const IfTag As String = "IF"
	Public Const ElseTag As String = "ELSE"
	Public Const EndIfTag As String = "ENDIF"
	Public Const TotalTag As String = "TOTAL"
	Public Const lCLAUSE_TYPE_ID As Integer = 7 'RWH(18/08/2000) RSAIB Process 12.
	Public Const lLETTER_TYPE_ID As Integer = 5
	Public Const lSUBDOC_TYPE_ID As Integer = 9
	
	
	Public Const VER_PLATFORM_WIN32_NT As Integer = 2
	'
	' Type required for the GetVersionEx API function
	'
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Public Structure OSVERSIONINFO
		Dim dwOSVersionInfoSize As Integer
		Dim dwMajorVersion As Integer
		Dim dwMinorVersion As Integer
		Dim dwBuildNumber As Integer
		Dim dwPlatformId As Integer
		<VBFixedString(128),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=128)> _
		Public szCSDVersion As FixedLengthString '  Maintenance string for PSS usage
		Public Shared Function CreateInstance() As OSVERSIONINFO
			Dim result As New OSVERSIONINFO
			result.szCSDVersion = New FixedLengthString(128)
			Return result
		End Function
	End Structure
	'
	' API function to return the version of Windows in use
	'

	Public Declare Function GetVersionEx Lib "kernel32"  Alias "GetVersionExA"(ByRef lpVersionInformation As OSVERSIONINFO) As Integer
	Public Declare Function GetUserName Lib "advapi32.dll"  Alias "GetUserNameA"(ByVal lpBuffer As String, ByRef nSize As Integer) As Integer
	
	'MKW170703 PN5375 END
	
	Sub Main_Renamed()
		
    End Sub

End Module