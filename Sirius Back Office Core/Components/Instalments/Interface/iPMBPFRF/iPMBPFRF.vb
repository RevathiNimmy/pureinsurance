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
    ' Date: 24/10/2000
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iPMBPFRF"


    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
    Public Const ScreenHelpID As Integer = 1

    ' General Icons

    Public Const ACMaxInstalments As Integer = 0

    Public Const ACParamsCount As Integer = 1
    Public Const ACFinanceNetCommission As Integer = 62
    Public Const ACSingleInstalmentPerMonth As Integer = 63
    Public Const ACFirstInstalmentAlignWithDayInMonth As Integer = 64
    Public Const ACApplyFeePercentagesToPolicyRisk As Integer = 66
    Public Const ACApplyFeePercentagesToTaxes As Integer = 67
    Public Const ACTransactionType As Integer = 68

    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101
    Public Const ACLblStatFrequ As Integer = 304
    Public Const ACLblStatDoc As Integer = 305
    Public Const ACLblAdvance As Integer = 306
    Public Const ACLblUserGroup As Integer = 307
    Public Const ACLblThreshhold As Integer = 308
    Public Const ACLblAtEnd As Integer = 309
    Public Const ACTabTitle4 As Integer = 310
    Public Const ACLblMaxInst As Integer = 311

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203

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

    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager


    Sub Main_Renamed()

    End Sub
End Module