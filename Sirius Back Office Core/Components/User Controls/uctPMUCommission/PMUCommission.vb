Option Strict Off
Option Explicit On
Imports System
'Developer Guide No. 129
Imports SharedFiles
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 01/12/1997
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "uctPMUCommission"

    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons

    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabCaption As Integer = 101
    Public Const ACPolicyHolderLabel As Integer = 102
    Public Const ACPolicyRefLabel As Integer = 103
    Public Const ACPartyGridColumn As Integer = 104
    Public Const ACAgentTypeGridColumn As Integer = 105
    Public Const ACRisktypeGridColumn As Integer = 106
    Public Const ACCommissionBandGridColumn As Integer = 107
    Public Const ACPremiumGridColumn As Integer = 108
    Public Const ACRateGridColumn As Integer = 109
    Public Const ACValueGridColumn As Integer = 110
    Public Const ACIsLeadAgentGridColumn As Integer = 120
    Public Const ACIsAmendedGridColumn As Integer = 121
    Public Const ACTotalCommission As Integer = 122

    'Detail Form
    Public Const ACDetailTitle As Integer = 111
    Public Const ACDetailTabCaption As Integer = 112
    Public Const ACPartyLabel As Integer = 113
    Public Const ACAgentTypeLabel As Integer = 114
    Public Const ACRiskTypeLabel As Integer = 115
    Public Const ACCommissionBandLabel As Integer = 116
    Public Const ACPremiumLabel As Integer = 117
    Public Const ACRateLabel As Integer = 118
    Public Const ACValueLabel As Integer = 119

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203
    Public Const ACAddButton As Integer = 204
    Public Const ACEditButton As Integer = 205
    Public Const ACDeleteButton As Integer = 206

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303
    Public Const ACCommissionTooHigh As Integer = 304
    Public Const ACCommissionTooHighTitle As Integer = 305

    'Constants to define the result array positions
    Public Const ACAgent As Integer = 0
    Public Const ACAgentType As Integer = 1
    Public Const ACRiskType As Integer = 2
    Public Const ACCommissionBand As Integer = 3
    Public Const ACPremium As Integer = 4
    Public Const ACCommPercent As Integer = 5
    Public Const ACCommValue As Integer = 6
    Public Const ACIsLeadAgent As Integer = 7
    Public Const ACIsAmended As Integer = 8
    Public Const ACAgentCnt As Integer = 9
    Public Const ACAgentTypeId As Integer = 10
    Public Const ACRiskTypeId As Integer = 11
    Public Const ACCommissionBandId As Integer = 12
    Public Const ACCurrency As Integer = 13
    Public Const ACTaxGroupID As Integer = 14
    Public Const ACTaxGroupDescription As Integer = 15
    Public Const ACTaxAmount As Integer = 16
    Public Const ACCalculatedCommissionValue As Integer = 17
    Public Const ACOverrideReason As Integer = 18

    'Start - Renuka - (WPR64 Paralleling)
    Public Const ACMaximumRate As Integer = 19
    Public Const ACIsValue As Integer = 20
    'End - Renuka - (WPR64 Paralleling)
    'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx)
    Public Const ACIsTaxAmended As Integer = 21
    'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx)

    'PN_68557 Start
    Public Const ACOrigCommissionRate As Integer = 22
    Public Const ACOrigTaxGroup As Integer = 23
    Public Const ACOrigTaxValue As Integer = 24
    Public Const ACPerilTypeId As Integer = 25
    Public Const ACClassOfBusinessId As Integer = 26
    Public Const ACRawCommValue As Integer = 27
    Public Const ACRawTaxAmount As Integer = 28

    Public Const ACOrgPerilTypeId As Integer = 21
    Public Const ACOrgClassOfBusinessId As Integer = 22
    'PN_68557 End

    ' {* USER DEFINED CODE (End) *}

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager

    ' Links for help text
    'UPGRADE_NOTE: (1053) g_sProductFamily was changed from a Constant to a Variable. More Information: http://www.vbtonet.com/ewis/ewi1053.aspx
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

    Sub Main_Renamed()

    End Sub
End Module