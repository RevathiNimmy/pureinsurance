Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 23/12/1997
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "S4I Policy"

    Public Const FeeImage As String = "PolicyImage"
    Public Const NarrativeImage As String = "PolicyImage"

    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons

    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101
    Public Const ACTabTitle2 As Integer = 102
    Public Const ACTabTitle3 As Integer = 103
    Public Const kParamsCount As Integer = 20
    Public Const kEditAgentDuringMTAMTC As Integer = 20
    'TN20000816 - Doc Ref
    Public Const ACTabTitle4 As Integer = 148
    Public Const ACCapInsuredName As Integer = 149
    Public Const ACCapProduct As Integer = 150
    Public Const ACCapBranchCode As Integer = 109
    Public Const ACCapAlternateReference As Integer = 151
    Public Const ACCapAnalysisCode As Integer = 110
    Public Const ACCapOldPolicyNo As Integer = 122
    Public Const ACCapQuoteExpiryDate As Integer = 123
    Public Const ACCapInvoiceToClient As Integer = 152
    Public Const ACCapProposalDate As Integer = 153
    Public Const ACCapFrameAgent As Integer = 154
    Public Const ACCapHoldCoverExpiryDate As Integer = 156
    Public Const ACCapInceptionTPI As Integer = 157

    'Policy Discount
    Public Const ACCapDiscountReason As Integer = 158
    Public Const ACCapDiscountedPremium As Integer = 159
    Public Const ACCapRecurring As Integer = 160
    Public Const ACCapDiscountPercentage As Integer = 161
    Public Const ACCapFrameDiscount As Integer = 162
    Public Const ACCapManualDiscountPercentage As Integer = 163

    ' PWF20020624 - Associated Clients SQL - SBO 1.9 - 094 Customer Centricity (START)
    Public Const ACTabTitle5 As Integer = 305
    Public Const ACCPFrame As Integer = 306
    Public Const ACCPLVCol0 As Integer = 307
    Public Const ACCPLVCol1 As Integer = 308
    Public Const ACCPLVCol2 As Integer = 309
    Public Const ACCPLVCol3 As Integer = 310
    Public Const ACCPLVCol4 As Integer = 311
    Public Const ACCPSetLead As Integer = 312
    Public Const ACCPSetCrspnd As Integer = 313

    Public Const ACCPDlgTitle As Integer = 314
    Public Const ACCPDlgDeleteLead As Integer = 315
    Public Const ACCPDlgDeleteCrspnd As Integer = 316
    Public Const ACCPDlgAddExists As Integer = 317
    Public Const ACCPDlgDeleteConfirm As Integer = 318
    ' PWF20020624 - Associated Clients SQL - SBO 1.9 - 094 Customer Centricity (START)

    Public Const ACRisks As Integer = 104
    Public Const ACPolicyNo As Integer = 105
    Public Const ACRegarding As Integer = 106
    Public Const ACStatus As Integer = 107
    Public Const ACQuote As Integer = 108
    'TN20000816 Doc Ref 10 Public Const ACRisk = 109
    'TN20000816 Doc Ref 10 Public Const ACBusinessSource = 110
    Public Const ACDates As Integer = 111
    Public Const ACCoverFrom As Integer = 112
    Public Const ACInception As Integer = 113
    Public Const ACCoverTo As Integer = 114
    Public Const ACRenewal As Integer = 115
    Public Const ACPaymentMethodText As Integer = 116
    Public Const ACStatus2 As Integer = 117
    Public Const ACPolicyType As Integer = 118
    Public Const ACSchemeText As Integer = 119
    Public Const ACPremiumDetails As Integer = 120
    Public Const ACPremiumPayable As Integer = 121
    'TN20000816 - Doc Ref 10 Public Const ACIPT = 122
    'TN20000816 - Doc Ref 10 Public Const ACVAT = 123
    Public Const ACPremium As Integer = 124
    Public Const ACCurrency As Integer = 125
    Public Const ACFuturePremium As Integer = 126
    Public Const ACRetainedDocuments As Integer = 127
    Public Const ACSourceRenewalInformation As Integer = 128
    Public Const ACFrequency As Integer = 129
    Public Const ACLTUExpiry As Integer = 130
    Public Const ACStopReason As Integer = 131
    Public Const ACTimesRenewed As Integer = 132
    Public Const ACRelationship As Integer = 133
    Public Const ACLapseReason As Integer = 134
    Public Const ACLapsedDateText As Integer = 135
    Public Const ACStandardPolicyWording As Integer = 136
    'ECK 14/7/99
    Public Const ACCommAccount As Integer = 137
    Public Const ACCommPremium As Integer = 138
    Public Const ACCommPercentage As Integer = 139
    Public Const ACCommCharge As Integer = 140
    Public Const ACCommPayable As Integer = 141
    Public Const ACCommOverride As Integer = 142

    'Tomo050400
    Public Const ACReferredOnMTA As Integer = 143
    Public Const ACReferredAtRenewal As Integer = 144
    Public Const ACRenewalMethod As Integer = 145
    Public Const ACBusinessType As Integer = 146
    Public Const ACIssuedDate As Integer = 147

    'RWH(23/04/2001) System Option constance for Months in advance
    'a 'Cover From Date' should be set.
    Public Const ACSysOptionCoverFromAdvanceMonthsAllowed As Integer = 1008

    ' PW311002 System Option constance for Months in advance
    'a 'Cover To Date' should be set.
    Public Const ACSysOptionCoverToAdvanceMonthsAllowed As Integer = 1009

    Public Const ACSysOptionPolicyDiscount As Integer = 5004

    'Start (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1.1)
    Public Const ACSysOptionNextInstalmentRenewal As Integer = 5070
    'Start (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1.1)

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203
    Public Const ACAddButton As Integer = 204
    Public Const ACEditButton As Integer = 205
    Public Const ACDeleteButton As Integer = 206
    Public Const ACPrevious As Integer = 207
    Public Const ACNext As Integer = 208
    'TN20000816 - Doc Ref 10 Public Const ACInsurer = 209
    Public Const ACReInsurer As Integer = 209

    Public Const ACCommission As Integer = 210
    Public Const ACHandler As Integer = 211

    'TN20000816 Doc Ref 10 Public Const ACAgent = 212
    Public Const ACAgentCode As Integer = 212

    Public Const ACCoInsurers As Integer = 213

    'TN20000816 Doc Ref 10 Public Const ACInvoiceAccount = 214
    Public Const ACAlternativeAccount As Integer = 214

    Public Const ACRelatedPolicy As Integer = 215

    'JMK 13/11/2001
    Public Const ACInsurersButton As Integer = 217

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303

    Public Const ACRefExists As Integer = 304
    'EK 17/11/99
    Public Const ACAgentMissing As Integer = 305
    'PLICO 29
    Public Const ACPolicyDeductible As Integer = 320
    Public Const ACPolicyLimits As Integer = 321
    Public Const kCoInsHandlingGross As Integer = 326
    Public Const kCoInsHandlingNet As Integer = 327
    Public Const kCoinsurancePlacement As Integer = 325


    ' Menus

    ' CTAF 220200 - VAT
    Public Const ACRateVAT As String = "VAT"

    ' {* USER DEFINED CODE (End) *}

    ' Used by Voyager
    ' Message Texts
    Public Const ACCancelDetailsTitleText As String = "Cancel Details"
    Public Const ACCancelDetailsText As String = "Cancelling will lose any changes" & Strings.Chr(13) & Strings.Chr(10) & "Do you want to cancel?"

    Public Const ACSaveDetailsTitleText As String = "Save Details"
    Public Const ACSaveDetailsText As String = "Details have changed" & Strings.Chr(13) & Strings.Chr(10) & "Do you want to save?"

    Public Const ACBusinessFailTitleText As String = "Business Object"
    Public Const ACBusinessFailText As String = "Unable to gain access to the business object" & Strings.Chr(13) & Strings.Chr(10) & "Please try later"

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
    <ThreadStatic()>
    Public g_iSourceID As Integer
    'Developer Guide No. 107
    <ThreadStatic()>
    Public g_iLanguageID As Integer
    'Developer Guide No. 107
    <ThreadStatic()>
    Public g_iUserId As Integer
    'Developer Guide No. 107
    <ThreadStatic()>
    Public g_iCurrencyID As Integer
    'RWH(23/04/2001)
    'Developer Guide No. 107
    <ThreadStatic()>
    Public g_sUserName As String = ""
    'Developer Guide No. 107
    <ThreadStatic()>
    Public g_sPassword As String = ""
    'Developer Guide No. 107
    <ThreadStatic()>
    Public g_iLogLevel As Integer

    Public Const ScreenHelpID As Integer = 4
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()>
    Public g_oObjectManager As bObjectManager.ObjectManager

    'Global reference to ListManager
    'EK 040400 Its in PMBGeneralFunc Now !
    'Global g_oListManager As Object

    Public Const kLookupCodeRenewalFrequencyMonthly As String = "MONTH"
    'Developer Guide No. 107
    '   <ThreadStatic()> _
    'Public g_oGIS As Object

    Public Const kProcessRenewalDatesFromCoverFrom As Integer = 1
    Public Const kProcessRenewalDatesFromCoverTo As Integer = 2
    Public Const kProcessRenewalDatesFromRenewalDayNumber As Integer = 3
    Public Const kProcessRenewalDatesFromFrequency As Integer = 4

    Public Const kDiscountRecurringTypeIdTransaction As Integer = 1
    Public Const kDiscountRecurringTypeIdTerm As Integer = 2
    Public Const kDiscountRecurringTypeIdPolicy As Integer = 3

    Public Const kGenericCurrencyFormat As String = "0.00"
    Public Const kDiscountPercentageFormat As String = "0.00000000"
    Public Const kDiscountReasonNone As Integer = 0

    'Associated Sub Agent Details
    Public Const kAssociatedSubAgentCnt As Integer = 0
    Public Const kAssociatedSubAgentShortName As Integer = 1
    Public Const kAssociatedSubAgentDateCancelled As Integer = 2
    Public Const kAssociatedSubAgentName As Integer = 3
    Public Const kAssociatedSubAgentResolvedName As Integer = 4


    '*********************************************************************************'
    ' Name          :   ENSelectClause
    ' Description   :   To Create the Enum for Clause Grouping
    ' Modified by   :   Arul Stephen
    '*********************************************************************************'

    'Start -(Arul Stephen)-(TechSpec WR6 ClauseGrouping.doc)-(5.1.2)
    Public Enum ENSelectClause
        Id = 0
        Code = 1
        Description = 2
        Selected = 3
        Default_Renamed = 4
        Branch = 5
        DoNotMerge = 6
    End Enum
    'End -(Arul Stephen)-(TechSpec WR6 ClauseGrouping.doc)-(5.1.2)

    '*********************************************************************************'
    ' Name          :   ENClauseType
    ' Description   :   To Create the Enum for Clause Grouping
    ' Modified by   :   Arul Stephen
    '*********************************************************************************'
    'Start -(Arul Stephen)-(TechSpec WR6 ClauseGrouping.doc)-(5.1.2)
    Public Enum ENClauseType
        ProductType = 1
        RiskType = 2
    End Enum
    'End -(Arul Stephen)-(TechSpec WR6 ClauseGrouping.doc)-(5.1.2)
    'Start -(Arul Stephen)-(TechSpec WR6 ClauseGrouping.doc)-(5.1.2)
    Public Const kSelectClauseCode As Integer = 0
    Public Const kSelectClauseDescription As Integer = 1
    Public Const kSelectClauseId As Integer = 2
    Public Const kSelectClauseDoNotMerge As Integer = 3

    Public Const ACSelectClasueRowIndex As Integer = 2
    Public Const ACSelectClasueArrayIndex As Integer = 0
    Public Const ACSelectClasueLoadControl As String = "Load Control"




    'To set the columns
    Public Const kRegKeyConstLvwId As Integer = 322
    Public Const kRegKeyConstLvwCode As Integer = 323
    Public Const kRegKeyConstLvwDescription As Integer = 324
    Public Const kRegKeyConstLvwDoNotMerge As Integer = 328
    '**********************************************
    ' Select Clasues list view constants
    '**********************************************
    Public Const kSelectClauseColHIndexId As Integer = 0
    Public Const kSelectClauseColHIndexCode As Integer = 1
    Public Const kSelectClauseColHIndexDescription As Integer = 2
    Public Const kSelectClauseColHIndexDoNotMerge As Integer = 3
    '**********************************************
    ' Select Clasues listview width of the columns
    '**********************************************
    Public Const kSelectClauseWidthId As Integer = 0
    Public Const kSelectClauseColWidthCode As Integer = 2440
    Public Const kSelectClauseColWidthDescription As Integer = 5760
    Public Const kSelectClauseColWidthDoNotMerge As Integer = 1440
    'End -(Arul Stephen)-(TechSpec WR6 ClauseGrouping.doc)-(5.1.2)

    '**************************************************************************
    '
    ' Name    : IsInListView
    ' Desc    : return PMTrue if in list view
    ' History : 18/08/2000 Tinny (Created)
    '
    '**************************************************************************
    Public Function IsInListView(ByVal v_vKeyID As Integer, ByRef r_oListView As ListView) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'is v_vKeyID in list view
            For lCount As Integer = 1 To r_oListView.Items.Count

                'If r_oListView.ListItems(lCount&).Tag = v_vKeyID Then

                ' Ram 10-01-2001
                ' Added the cLng Conversion, since the v_vKeyID is a numeric,
                ' when compared with Tag property so it always mismatches
                If Convert.ToString(r_oListView.Items.Item(lCount - 1).Tag) = v_vKeyID Then
                    result = gPMConstants.PMEReturnCode.PMTrue
                    Exit For
                End If
            Next

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="IsInListView", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
End Module
