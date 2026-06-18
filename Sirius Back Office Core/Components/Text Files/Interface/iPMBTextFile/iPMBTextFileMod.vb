Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Runtime.InteropServices
'Developer Guide No. 129
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


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iPMBTextFile"


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

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203
    Public Const ACEditButton As Integer = 204
    Public Const ACCloseButton As Integer = 205
    Public Const ACDeleteButton As Integer = 206

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303
    Public Const ACAddDetailsTitle As Integer = 304
    Public Const ACAddDetails As Integer = 305
    Public Const ACMailLogonFailed As Integer = 306
    Public Const ACUnsupportedWordVersion As Integer = 307

    ' Menus


    ' {* USER DEFINED CODE (End) *}

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Public source and language ID's from the
    ' Object Manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_sUserName As String = "" 'MKW150703 PN5359

    'Public g_oZipper As New bPMZipper.Business
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oZipper As bPMZipper.Business

    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager

    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

    Public Const DbTag As String = "DB"
    Public Const TableTag As String = "TBL"
    Public Const FieldTag As String = "FLD"
    Public Const LoopTag As String = "LOOP"
    Public Const EndLoopTag As String = "ENDLOOP"
    Public Const FileTag As String = "FILE"
    Public Const QuestionTag As String = "KEY0"
    Public Const Separator As String = "_"
    Public Const ClauseTag As String = "CL"

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
        <VBFixedString(128), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=128)> _
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

    Public Declare Function GetVersionEx Lib "kernel32" Alias "GetVersionExA" (ByRef lpVersionInformation As OSVERSIONINFO) As Integer
    Public Declare Function GetUserName Lib "advapi32.dll" Alias "GetUserNameA" (ByVal lpBuffer As String, ByRef nSize As Integer) As Integer
    Public Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Integer)

    Sub Main_Renamed()

    End Sub
End Module