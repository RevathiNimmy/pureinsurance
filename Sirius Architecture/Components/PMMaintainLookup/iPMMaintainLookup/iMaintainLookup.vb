Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 13/05/99
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iPMMaintainLookup"


    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons


    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203
    Public Const ACApplyButton As Integer = 204

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303

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

    ' Do we have extra columns
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_bHasExtras As Boolean

    ' Do we have foreign keys
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_bHasKeys As Boolean
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_bUnderWriting As Boolean

    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager

    ' ***************************************************************** '
    ' Name: IDToEnglish
    '
    ' Description: Converts an ID field name to "english"
    '              Eg. caption_id  ->   Caption ID
    '
    ' ***************************************************************** '
    Public Function IDToEnglish(ByVal v_sID As String, ByRef r_sEnglish As String) As Integer

        Dim result As Integer = 0
        Dim sString As String = ""
        'Developer Guide No. 128
        Dim sByte_Renamed As New Char

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_sEnglish = ""

            'Developer Guide No. 128
            For Each sByte_Renamed In v_sID
                If sByte_Renamed = "_" Then
                    r_sEnglish = r_sEnglish & " "
                Else
                    'Developer Guide No. 128
                    r_sEnglish = r_sEnglish & sByte_Renamed
                End If
                'Developer Guide No. 128
            Next sByte_Renamed

            ' Uppercase the first letter, lowercase the rest
            r_sEnglish = (r_sEnglish.Substring(0, 1).ToUpper() & _
                         r_sEnglish.Substring(r_sEnglish.Length - (r_sEnglish.Length - 1)).ToLower()).Trim()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IDToEnglish Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IDToEnglish", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


End Module
