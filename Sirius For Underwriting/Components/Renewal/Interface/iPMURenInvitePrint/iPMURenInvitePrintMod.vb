Option Strict Off
Option Explicit On
Imports System
'Developer Guide No.129
Imports SharedFiles
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 06/09/2000
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iPMURenInvitePrint"


    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons


    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101
    Public Const ACSelectionDate As Integer = 102
    Public Const ACProductCode As Integer = 103
    Public Const ACAgentCode As Integer = 104
    Public Const ACSortOrder As Integer = 105

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203
    Public Const ACRePrintButton As Integer = 207

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303

    ' Constants to define print operations
    Public Const AC_VIEW_ONLY As Integer = 0
    Public Const AC_PRINT_ONLY As Integer = 1
    Public Const AC_PRINT_AND_VIEW As Integer = 2

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
    ' JMK 10/08/2001
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iUserID As Integer

    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager

    ' KB 130801 required for help text, screenhelpid will need to be amended
    ' when text available
    Public Const ScreenHelpID As Integer = 4001
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oRenewal As bSIRRenewal.Business


    Sub Main_Renamed()

    End Sub
End Module