Option Strict Off
Option Explicit On
Imports System
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
    Public Const ACApp As String = "iPMBPolicyNumMaint"

    Public Const MainScreenHelpID As Integer = 4057
    Public Const DetailScreenHelpID As Integer = 4058

    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons

    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101
    Public Const ACFraSchemes As Integer = 102
    Public Const ACInterfaceTitle2 As Integer = 105
    Public Const ACTabTitle2 As Integer = 106

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203
    Public Const ACEditButton As Integer = 204
    Public Const ACAddButton As Integer = 205

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303
    Public Const ACAddDetailsTitle As Integer = 304
    Public Const ACAddDetails As Integer = 305
    Public Const ACMsgBoxTitle As Integer = 306
    Public Const ACNextGtThanHighest As Integer = 307
    Public Const ACDuplicateScheme1 As Integer = 308
    Public Const ACDuplicateScheme2 As Integer = 309
    Public Const ACSelGenVal As Integer = 310
    Public Const ACNumSchemeLimit As Integer = 311
    Public Const ACValidCharsVal As Integer = 312
    Public Const ACMaskCharsConsecutive As Integer = 313
    Public Const ACMask_FixedCode As Integer = 314
    Public Const ACHighestNumberDigits As Integer = 315
    Public Const ACDelSchemes As Integer = 316
    ' Start - (Sankar) - (Tech Spec - VAL P14 - Policy Numbering.doc)
    Public Const ACRenewalCodeAtEnd As Integer = 317
    Public Const ACInValidRenewalCode As Integer = 318
    ' End - (Sankar) - (Tech Spec - VAL P14 - Policy Numbering.doc)
    'Start - Renuka - (WPR87 Paralleling)
    Public Const ACInvalidAccountingPeriodFormat As Integer = 319
    'End - Renuka - (WPR87 Paralleling)
    ' Menus


    ' {* USER DEFINED CODE (End) *}

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    Public Const ACNormalMode As Integer = 0
    Public Const ACMergeMode As Integer = 1

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

    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

    'Public Const g_sMSGBOX_TITLE = "Policy Number Maintenance"
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_sMsgBoxTitle As String = ""
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_sCancelMsg As String = ""






    Sub Main_Renamed()

    End Sub
End Module