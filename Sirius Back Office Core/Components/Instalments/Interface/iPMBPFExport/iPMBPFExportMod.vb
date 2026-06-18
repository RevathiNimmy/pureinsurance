Option Strict Off
Option Explicit On
Imports System
'refer Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 05/05/1999
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '

    'refer Developer Guide No. 50
    'Developer Guide No. 107
    <ThreadStatic()> _
    Public m_ofrmInterface As frmInterface

    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iPMBPFExport"


    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

    Public Const ScreenHelpID As Integer = 1

    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons


    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACLeadDays As Integer = 101
    Public Const ACBatchNo As Integer = 102

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203
    Public Const ACExport As Integer = 207
    Public Const ACPost As Integer = 208
    Public Const ACRecall As Integer = 209
    Public Const ACPayment As Integer = 210

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
    Public Const kSystemOptionPaymentHubEnabled As Integer = 5200
    ' Public source and language ID's from the
    ' Object Manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer

    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager

    Sub Main_Renamed()

    End Sub
End Module