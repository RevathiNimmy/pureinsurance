Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
 Public Module MainModule
    ' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 17/02/1997
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' SP130199 - Remove NavigatorV3 class an put in stub so can be called
	' iteratively.
	' ***************************************************************** '

    Public Const kMediaTypeValidationCodeBank As String = "BANK"
    Public Const kMediaTypeValidationCodeCreditCard As String = "CC"

    Public Const kActionCodeAmendment As String = "Amendment"
    Public Const kActionCodeCancellation As String = "Cancellation"

    Public Const kMainTabBankDetails As Integer = 2
    Public Const kMainTabCreditCardDetails As Integer = 3

    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iPMBFinancePlanMaint"

    Public Const ACProcessCode As String = "PFCASH"
    ' Batch code
    Public Const ACBatchCode As String = "PFCASH"
    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons
    ' Form
    Public Const ACInterfaceTitle As Integer = 100

    Public Const ACTabTitle1 As Integer = 101
    Public Const ACTabTitle2 As Integer = 102
    Public Const ACTabTitle3 As Integer = 103
    'TR - 24/03/03 - TS17 Recovery By Instalments changes
    Public Const ACTabTitle4 As Integer = 104
    Public Const ACTabTitle5 As Integer = 105
    Public Const ACTabTitle6 As Integer = 106
    'Tab1
    Public Const ACCompany As Integer = 110
    Public Const ACSchemeName As Integer = 111
    Public Const ACProductClass As Integer = 112
    Public Const ACStartDate As Integer = 113
    Public Const ACDaysDelay As Integer = 114
    Public Const ACNumberOfInstalments As Integer = 115
    Public Const ACPaymentProtect As Integer = 116
    Public Const ACCommissionOverride As Integer = 117
    Public Const ACOverrideRate As Integer = 118
    Public Const ACFinancedTransactions As Integer = 119
    Public Const ACCostOfProtection As Integer = 120
    Public Const ACDeposit As Integer = 121
    Public Const ACFirstInstalment As Integer = 122
    Public Const ACOtherInstalments As Integer = 123
    Public Const ACGrossAmount As Integer = 124
    Public Const ACAPR As Integer = 125
    Public Const ACInterestRate As Integer = 126
    Public Const ACAuthorisation As Integer = 127
    Public Const ACNetAmount As Integer = 128
    'Tab2
    Public Const ACClientName As Integer = 129
    Public Const ACAddressLine1 As Integer = 130
    Public Const ACAddressLine2 As Integer = 131
    Public Const ACAddressLine3 As Integer = 132
    Public Const ACCounty As Integer = 133
    Public Const ACPostcode As Integer = 134
    Public Const ACCountry As Integer = 135
    Public Const ACClientAreaCode As Integer = 136
    Public Const ACClientNumber As Integer = 137
    Public Const ACClientExtension As Integer = 138
    Public Const ACClientTelephone As Integer = 139
    Public Const ACClientFax As Integer = 140
    'Tab3
    Public Const ACBankName As Integer = 141
    Public Const ACSortCode As Integer = 142
    Public Const ACAccountNumber As Integer = 143
    Public Const ACBranch As Integer = 144
    Public Const ACAccountName As Integer = 145
    Public Const ACBankAddressLine1 As Integer = 146
    Public Const ACBankAddressLine2 As Integer = 147
    Public Const ACBankAddressLine3 As Integer = 148
    Public Const ACBankTown As Integer = 149
    Public Const ACBankPostCode As Integer = 150
    Public Const ACRegion As Integer = 151
    Public Const ACBankCountry As Integer = 152
    Public Const ACAreaCode As Integer = 153
    Public Const ACNumber As Integer = 154
    Public Const ACExtension As Integer = 155
    Public Const ACBankTelephone As Integer = 156
    Public Const ACBankFax As Integer = 157

    'TR - 24/03/03 - TS17 Recovery By Instalments changes
    Public Const ACTabDates As Integer = 209
    Public Const ACCreatedDate As Integer = 210
    Public Const ACModifiedDate As Integer = 211
    Public Const ACConfirmedDate As Integer = 212
    Public Const ACReviewDate As Integer = 213
    Public Const ACOptNoStatements As Integer = 214
    Public Const ACStmtFrequency As Integer = 215
    Public Const ACOriginalDebt As Integer = 216
    Public Const ACFraSummary As Integer = 217
    Public Const ACAgentfraAgentInfo As Integer = 218
    Public Const ACAgentRef As Integer = 219
    Public Const ACAgent As Integer = 220
    Public Const ACAgentAddress As Integer = 221
    Public Const ACAgentPostcode As Integer = 222
    Public Const ACAgentCountry As Integer = 223
    Public Const ACAgentTelephone As Integer = 224
    Public Const ACAgentFaxNo As Integer = 225
    Public Const ACAgentAreaCode As Integer = 226
    Public Const ACAgentNumber As Integer = 227
    Public Const ACAgentExtension As Integer = 228

    ' Buttons
    Public Const ACSaveButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203
    Public Const ACReprintButton As Integer = 204
    Public Const ACTransactButton As Integer = 205
    Public Const ACReSendButton As Integer = 206
    Public Const ACExitButton As Integer = 207
    Public Const ACMTAButton As Integer = 208

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303
    Public Const ACClearDetailsTitle As Integer = 304
    Public Const ACClearDetails As Integer = 305
    Public Const ACStatusSearching As Integer = 306
    Public Const ACStatusFound As Integer = 307
    Public Const ACAllTypes As Integer = 308
    Public Const ACYes As Integer = 309
    Public Const ACNo As Integer = 310
    Public Const ACPrimaryForAllocation As Integer = 311
    Public Const ACSecondaryForAllocation As Integer = 312
    Public Const ACMTAChangeType As Integer = 313

    '*************
    ' MEvans : 28-03-2003 : Issue 2647
    ' Credit Card No Validation Message Fields
    Public Const ACResFileMessageTitleCCNoValidate As Integer = 400
    Public Const ACRefFileMessageCCNoInvalidCardNo As Integer = 401

    ' Credit Card Expiry Date Validation Message Fields
    Public Const ACResFileMessageTitleCCExpiryDateValidate As Integer = 402
    Public Const ACResFileMessageCCExpiryDateWrongFormat As Integer = 403
    Public Const ACResFileMessageCCExpirtyDateInvalidDate As Integer = 404
    Public Const ACResFileMessageCCExpired As Integer = 408

    ' Credit Card Start Date Validation Message Fields
    Public Const ACResFileMessageTitleCCStartDateValidate As Integer = 405
    Public Const ACResFileMessageCCStartDateWrongFormat As Integer = 406
    Public Const ACResFileMessageCCStartDateInvalidDate As Integer = 407
    Public Const ACResFileMessageCCNotActivated As Integer = 409
    Public Const ACCancelPolicyButton As Integer = 410
    '*************

    Public Const ACIPlanReference As Integer = 0
    Public Const ACIPlanVersion As Integer = 1
    Public Const ACIStartDate As Integer = 6
    Public Const ACIStatus As Integer = 62
    Public Const ACICompanyName As Integer = 9
    Public Const ACISchemeName As Integer = 10
    Public Const ACIAmountFinanced As Integer = 13
    Public Const ACIMax As Integer = 14

    Public Const ACLDocumentType As Integer = 0
    Public Const ACLDocTypeGroup As Integer = 1
    Public Const ACLMax As Integer = 1

    ' Constants for data array indexes.
    Public Const ACIDocumentRef As Integer = 0
    Public Const ACIAccountingDate As Integer = 1
    Public Const ACIPeriodName As Integer = 2
    Public Const ACICurrencyAmount As Integer = 3
    Public Const ACIPrimarySettled As Integer = 4
    Public Const ACIMatchedCurrencyAmount As Integer = 5
    Public Const ACIDocumentTypeId As Integer = 6
    Public Const ACIDocTypeGroupId As Integer = 7
    Public Const ACIInsuranceRef As Integer = 8
    Public Const ACIOperatorName As Integer = 9
    Public Const ACIPurchaseInvoiceNo As Integer = 10
    Public Const ACIPurchaseOrderNo As Integer = 11
    Public Const ACIDepartment As Integer = 12
    Public Const ACISpare As Integer = 13
    Public Const ACIAccountShortCode As Integer = 14
    Public Const ACIAccountId As Integer = 15
    Public Const ACICurrencyID As Integer = 16
    Public Const ACITransDetailId As Integer = 17
    Public Const ACIBaseAmount As Integer = 18
    Public Const ACIDocumentSequence As Integer = 19
    Public Const ACIDocumentDate As Integer = 20
    Public Const ACISourceID As Integer = 21
    Public Const ACIMatchAmount As Integer = 22
    Public Const ACIMatchDate As Integer = 23
    Public Const ACIReason As Integer = 24
    Public Const ACIInsuredName As Integer = 25
    Public Const ACIInsuredAccount As Integer = 26
    Public Const ACIFlag As Integer = 27
    Public Const ACIDocInsuranceFileCnt As Integer = 28
    Public Const ACIDocDocumentID As Integer = 29
    Public Const ACIAuditSetID As Integer = 30
    Public Const ACIAuditSetUserID As Integer = 31
    Public Const ACITransCurrencyBaseXRate As Integer = 32
    Public Const ACIPayeeName As Integer = 33
    Public Const ACIAlternateReference As Integer = 34
    Public Const ACIPolicyTypeId As Integer = 35
    Public Const ACIComment As Integer = 36
    Public Const ACINotReported As Integer = 37
    Public Const ACIUnderwritingYear As Integer = 38
    Public Const ACIMediaType As Integer = 39
    Public Const ACICurrencyText As Integer = 40
    Public Const ACIAmountCurrencyText As Integer = 41
    Public Const ACIAmountCurrencyID As Integer = 42
    Public Const ACIAccountCurrencyID As Integer = 43
    Public Const ACIAccountAmount As Integer = 44
    Public Const ACIOutstandingBaseAmount As Integer = 45
    Public Const ACIOutstandingAccountAmount As Integer = 46
    Public Const ACIAmountUpdated As Integer = 47
    Public Const ACIOutstandingTransAmount As Integer = 48

    ' Constants for Transaction List View index.
    Public Const ACListSourceID As Integer = 0
    Public Const ACListAccountShortCode As Integer = 1
    Public Const ACListDocumentRef As Integer = 2
    Public Const ACListAccountingDate As Integer = 3
    Public Const ACListDocumentDate As Integer = 4
    Public Const ACListPeriodName As Integer = 5
    Public Const ACListCurrencyAmount As Integer = 6
    Public Const ACListPrimarySettled As Integer = 7
    Public Const ACListOSCurrencyAmount As Integer = 8
    Public Const ACListDocumentTypeId As Integer = 9
    Public Const ACListDocTypeGroupId As Integer = 10
    Public Const ACListInsuranceRef As Integer = 11
    Public Const ACListOperatorName As Integer = 12
    Public Const ACListSpare As Integer = 13
    Public Const ACListPurchaseInvoiceNo As Integer = 14
    Public Const ACListPurchaseOrderNo As Integer = 15
    Public Const ACListDepartment As Integer = 16
    Public Const ACListMatchAmount As Integer = 17

    Public Const ACTLookupDocumentType As String = "DocumentType"
    Public Const ACTLookupDocTypeGroup As String = "DocTypeGroup"

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the maxiumum search details.
    Public Const ACMaxSearchDetails As Integer = 500

    ' Constant for the miniumum search length.
    'Modified by ECK 11/05/99
    Public Const ACMinSearchLength As Integer = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Public source and language ID's from the
    ' Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iUserId As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sUserName As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lCountryID As Integer

    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager

    ' Public instance of the business object.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As bSIRPremiumFinance.Business
    Public g_oFindTransaction As Object

    <ThreadStatic()> _
 Public g_oFindInsurance As Object


    'Party Bank Details
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oPartyBank As bSIRPartyBank.Business

    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

    Public Const ScreenHelpID As Integer = 1

    '2005 Client Manager Security
    Public Const PMKeyNameFinancePlanEditAuthority As String = "FinancePlanEditAuthority"
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oUserAuthorities As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_bEditFinancePlanAuthority As Boolean

    Private m_lReturn As Integer
    Private m_vLookupValues As Object
    Private m_vLookupDetails As Object

    Sub Main_Renamed()

        Dim vKeyArray(,) As Object

        Dim oFinancePlanQuote As New Interface_Renamed

        Dim lError As Integer = CType(oFinancePlanQuote, SSP.S4I.Interfaces.ILocalInterface).Initialise()

        oFinancePlanQuote.CallingAppName = "TEST"
        lError = oFinancePlanQuote.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:="", vEffectiveDate:=DateTime.Now)

        ReDim vKeyArray(1, 0)
        lError = oFinancePlanQuote.Start()

        Dim lpartyCnt As Integer = oFinancePlanQuote.partyCnt
		oFinancePlanQuote.Dispose()

    End Sub

    ' ***************************************************************** '
    ' Name: GetLookupRow
    '
    ' Description: Converts a lookup table name to its matching row index
    '              in the table of lookup values.
    '              May be used to indirect GetLookupDetails, GetLookupDesc.
    '              Returns -1 if no match found
    '
    ' ***************************************************************** '
    Public Function GetLookupRow(ByRef sLookupTable As String) As Integer

        Dim result As Integer = 0
        Dim lRow As Integer
        Dim bFoundMatch As Boolean

        Try

            result = -1

            bFoundMatch = False

            For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
                ' Check for a match of the table name.

                If CStr(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, lRow)).Trim() = sLookupTable.Trim() Then
                    ' Found a match
                    bFoundMatch = True
                    Exit For
                End If
            Next lRow

            If bFoundMatch Then
                result = lRow
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to match lookup table", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupRow", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '
    'Public Function GetLookupDetails(ByRef lLookupRow As Integer, ByRef ctlLookup As Control, Optional ByVal vAllTypes As Object = Nothing) As Integer
    Public Function GetLookupDetails(ByRef lLookupRow As Integer, ByRef ctlLookup As ComboBox, Optional ByVal vAllTypes As Object = Nothing) As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if there has been a table match.
            If lLookupRow = -1 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.

            Dim sLookupDesc As String = ""
            If Not Information.IsNothing(vAllTypes) Then

                If CBool(vAllTypes) Then
                    ' First entry is all types (don't care)

                    'Developer Guide No 76
                    sLookupDesc = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAllTypes, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    'Developer Guide No 153
                    Dim listIndex As Integer = ctlLookup.Items.Add(New VB6.ListBoxItem(sLookupDesc, -1))

                End If
            End If

            For lCntr As Integer = CInt(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupStartPos, lLookupRow)) To CInt((CDbl(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupStartPos, lLookupRow)) + CDbl(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupNumOfItems, lLookupRow))) - 1)
                ' Add the details to the control.

                'Developer Guide No 153
                Dim listIndex As Integer = ctlLookup.Items.Add(New VB6.ListBoxItem(m_vLookupDetails(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption, lCntr), m_vLookupDetails(gPMConstants.PMELookupOutArrayColPos.PMLookupID, lCntr)))

                ' Check if this is the selected index.

                If m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupKey, lLookupRow).Equals(m_vLookupDetails(gPMConstants.PMELookupOutArrayColPos.PMLookupID, lCntr)) Then

                    'Developer Guide No 28
                    ctlLookup.SelectedIndex = listIndex
                End If
            Next lCntr

            ' Check if the selected index is blank. If so,
            ' we set the controls index to zero.

            If CStr(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupKey, lLookupRow)) = "" Then

                'Developer Guide No 28
                ctlLookup.SelectedIndex = 0
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    Public Function GetLookupValues() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Gets all of the lookup values.

            ReDim m_vLookupValues(3, ACLMax)

            ' Setup Lookup Table Names

            m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, ACLDocumentType) = ACTLookupDocumentType

            m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, ACLDocTypeGroup) = ACTLookupDocTypeGroup

            ' Do not supply a key
            For i As Integer = 0 To ACLMax

                m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupKey, i) = ""
            Next i

            ' Get all of the lookup values with the correct
            ' effective date.

            m_lReturn = g_oFindTransaction.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetLookupDesc
    '
    ' Description: Gets a description string for a given lookup set
    '              and lookup id.
    '
    ' ***************************************************************** '
    Public Function GetLookupDesc(ByRef lLookupRow As Integer, ByRef lLookupID As Integer, ByRef sLookupDesc As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if there has been a table match.
            If lLookupRow = -1 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Using the lookup values, populate the lookup
            ' string from the lookup details array when the
            ' lookup ID has been matched.

            For lCntr As Integer = CInt(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupStartPos, lLookupRow)) To CInt((CDbl(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupStartPos, lLookupRow)) + CDbl(m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupNumOfItems, lLookupRow))) - 1)
                ' Check for a match on the ID.

                If CInt(m_vLookupDetails(gPMConstants.PMELookupOutArrayColPos.PMLookupID, lCntr)) = lLookupID Then
                    ' Found a match

                    ' Store the details to the lookup string.

                    sLookupDesc = CStr(m_vLookupDetails(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption, lCntr)).Trim()

                    Exit For
                End If
            Next lCntr

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDesc", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Sub SelectControl(ByVal Destination As TextBox, Optional ByVal TabControl As TabControl = Nothing, Optional ByVal TabNumber As Integer = 0)

        Try

            ' Select all text in the control
            Destination.SelectionStart = 0
            Destination.SelectionLength = Strings.Len(Destination.Text)

            ' Check for tab control

            If Not Information.IsNothing(TabControl.SelectedTab.TabIndex) Then
                SSTabHelper.SetSelectedIndex(TabControl, TabNumber)
            End If

            ' Send focus to the control
            Destination.Focus()

        Catch

            ' Errors don't particularly matter, especially on the tab object, continue

        End Try

    End Sub

    'Developer Guide No 101
    Public Sub SetCommand(ByRef Button As Button, ByVal Visible As Boolean, ByVal Enabled As Boolean, Optional ByVal Caption As Object = Nothing)

        Try

            ' Select all text in the control
            Button.Visible = Visible
            Button.Enabled = Enabled

            If Not Information.IsNothing(Caption) Then
                Button.Text = Caption
            End If

        Catch

            ' Errors don't particularly matter, especially on the tab object, continue

        End Try

    End Sub
    
End Module
