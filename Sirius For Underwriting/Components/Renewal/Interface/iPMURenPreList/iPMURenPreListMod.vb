Option Strict Off
Option Explicit On
Imports System
'Developer Guide No. 129
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
    Public Const ACApp As String = "iPMURenPreList"


    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons


    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101
    Public Const ACRenewalDate As Integer = 102
    Public Const ACProductCode As Integer = 103
    Public Const ACPreview As Integer = 104
    Public Const ACPrint As Integer = 105

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

    ' Constants to define print operations
    Public Const AC_VIEW_ONLY As Integer = 0
    Public Const AC_PRINT_ONLY As Integer = 1
    Public Const AC_PRINT_AND_VIEW As Integer = 2

    ' Renewal List Position constants.
    Public Const PMFieldPosInsuranceFileCnt As Integer = 0
    Public Const PMFieldPosInsuranceHolderCnt As Integer = 1
    Public Const PMFieldPosProductID As Integer = 2
    Public Const PMFieldPosLeadAgentCnt As Integer = 3
    Public Const PMFieldPosInsuranceRef As Integer = 4
    Public Const PMFieldPosCoverStartDate As Integer = 5
    Public Const PMFieldPosCoverEndDate As Integer = 6
    Public Const PMFieldPosClientName As Integer = 7
    Public Const PMFieldPosAgentName As Integer = 8
    Public Const PMFieldPosIsAutoRenewable As Integer = 9
    Public Const PMFieldPosProductCode As Integer = 10
    Public Const PMFieldPosPolicyStopReason As Integer = 11
    Public Const PMFieldPosClientStopReason As Integer = 12
    Public Const PMFieldPosReferredAtRenewal As Integer = 13
    Public Const PMFieldPosInsuranceFolderCnt As Integer = 14
    Public Const PMFieldPosRenewalDate As Integer = 15
    Public Const PMFieldPosHolderName As Integer = 16
    Public Const PMFieldPosAgentStopReason As Integer = 17
    Public Const PMFieldPosClosedBranch As Integer = 18
    Public Const PMFieldPosAgentInTransfer As Integer = 19
    'Developer Guide No. 50
    Public objfrmInterface As frmInterface
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

    ' KB 130801 required for help text, screenhelpid will need to be amended
    ' when text available
    Public Const ScreenHelpID As Integer = 4001
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions


    Sub Main_Renamed()

    End Sub
End Module