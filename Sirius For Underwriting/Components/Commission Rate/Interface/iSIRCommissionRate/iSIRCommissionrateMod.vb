Option Strict Off
Option Explicit On
Imports System
'developer guide no.129
Imports SharedFiles
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 04/09/2000
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '

    <ThreadStatic()> _
    Public frmDetail As frmDetail

    <ThreadStatic()> _
    Public frmInterface As frmInterface
    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iSirCommissionRate"

    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons

    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabCaption As Integer = 101
    Public Const ACPartyTypeLabel As Integer = 111
    Public Const ACPartyLabel As Integer = 112
    Public Const ACProductLabel As Integer = 113
    Public Const ACRiskTypeLabel As Integer = 114
    Public Const ACCommissionBandLabel As Integer = 115
    Public Const ACTransactionTypeLabel As Integer = 116
    Public Const ACEffectiveDateLabel As Integer = 117
    Public Const ACRateLabel As Integer = 118
    Public Const ACIsValueLabel As Integer = 119
    ' CMG / PB 23072002 New Commission Grouping functionality
    Public Const ACCommissionGroupLabel As Integer = 120
    ' CMG End

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203
    Public Const ACAddButton As Integer = 204
    Public Const ACEditButton As Integer = 205
    Public Const ACDeletebutton As Integer = 206
    Public Const ACUndeletebutton As Integer = 207

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303
    Public Const ACDuplicateItemTitle As Integer = 304
    Public Const ACDuplicateItem As Integer = 305

    ' {* USER DEFINED CODE (End) *}

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    'Constants to define the columns of the list view

    ' CMG / PB 23072002 New Commission Grouping functionality shifts these around
    Public Const ACRateCol As Integer = 7
    Public Const ACIsValueCol As Integer = 8
    Public Const ACEffectiveDateCol As Integer = 9
    Public Const ACPartyTypeCol As Integer = 10
    Public Const ACPartyCol As Integer = 11
    Public Const ACProductCol As Integer = 12
    Public Const ACRiskTypeCol As Integer = 13
    Public Const ACTransactionTypeCol As Integer = 14
    Public Const ACCommissionBandCol As Integer = 15
    Public Const ACCommissionGroupCol As Integer = 16
    Public Const ACTaxGroupID As Integer = 17
    Public Const ACTaxGroup As Integer = 18
    'Start - Renuka - (WPR64 Paralleling)
    Public Const ACMaximumRateCol As Integer = 19
    'End - Renuka - (WPR64 Paralleling)
    'SAGICOR WPR14
    Public Const ACCommissionLevelID As Integer = 20
    Public Const ACCommissionLevelDESC As Integer = 21
    'Thinh Nguyen 01/07/2003
    Public Const ACIsDeletedCol As Integer = 17

    'TransactionTypeIDs
    Public Const ACTTNewBusiness As Integer = 4
    Public Const ACTTCancelPolicy As Integer = 7
    Public Const ACTTMTA As Integer = 9
    Public Const ACTTRenewals As Integer = 10

    ' Public source and language ID's from the
    ' Object Manager.

    <ThreadStatic()> _
    Public g_iSourceID As Integer

    <ThreadStatic()> _
    Public g_iLanguageID As Integer

    <ThreadStatic()> _
    Public g_sUsername As String = ""

    <ThreadStatic()> _
    Public g_sPassword As String = ""

    ' Public array for storing data for the grid.

    <ThreadStatic()> _
    Public g_vGridData As Object

    ' Public instance of the object manager.

    <ThreadStatic()> _
    Public g_oObjectManager As bObjectManager.ObjectManager

    ' Required for link to help files

    'UPGRADE_NOTE: (1053) g_sProductFamily was changed from a Constant to a Variable. More Information: http://www.vbtonet.com/ewis/ewi1053.aspx
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
    Public Const ScreenhelpID As Integer = 4063

    Sub Main_Renamed()

    End Sub

    'start

End Module