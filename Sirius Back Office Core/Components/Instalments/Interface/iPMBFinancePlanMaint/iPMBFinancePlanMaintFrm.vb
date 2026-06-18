Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Globalization
Imports System.Text
Imports System.Windows.Forms

Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 17/02/1997
    '
    ' Description: Main interface.
    '
    ' Edit History: 170297 - Created
    ' TF240498 - ProcessPartyInterface() added to activate refresh on
    '           return to Find
    ' SP011298 - changes to support new business roadmap
    ' RAW 04/03/2003 : ISS2592 : moved underwriting test to start of Form_load sub
    ' RAW 07/03/2003 : added extra functionality to save instalments from cmdSave when in NewBusiness roadmap
    ' RAW 11/03/2003 : ISS2580 : Process deposit received AFTER posting plan
    '                            wrap cmdSave and cmdTransact processing within a database transaction
    ' TR  25/03/2003 : TS17 Recovery By Instalments changes
    ' CJB 05/10/2005 : PN24604 Changed BusinessToInterface to disable all controls that are enabled (apart from
    '                  command buttons) if user does not have Edit Finance Plan Authority
    ' ***************************************************************** '

    'Party Bank Details
    Private lBankPaymentTypeID As Integer
    Private aoPartyBankDetails(,) As Object = Nothing
    Private oProduct As Object
    Private aoPlanPartyArray(,) As Object = Nothing
    Private bIsSinglePlanParty As Boolean
    Private lLeadAgentCnt As Integer
    Private lIsBroker As Integer
    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmInterface"
    Private Const vbFormCode As Integer = 0
    Private m_bPlanReleasedFromOnHold As Boolean
    Private m_nUnloadMode As Integer = 0
    ' TR  24/03/2003 : TS17 Recovery By Instalments changes
    Public Enum PrintDocType
        DocTypeQuote = 1
        DocTypeBank = 2
        DocTypeCredit = 3
        DocTypeConfirmation = 4
        DocTypeInstalmentPlan = 5
        DocTypeInstalmentCancel = 6
        DocTypeInstalmentSettle = 7
        DocTypeInstalmentEdit = 8
        DocTypePCLSG = 9
    End Enum

    'Party Bank Details
    Private Enum ENPartyBank
        RowStatus = 0
        RowIndex = 1
        PartyBankId = 2
        IsBank = 3
        AccountID = 4
        BankPaymentTypeId = 5
        BankAccountTypeId = 6
        AccountHolderName = 7
        AccountNumber = 8
        BankNameId = 9
        BankBranch = 10
        BankBranchCode = 11
        BankAdd1 = 12
        BankAdd2 = 13
        BankAdd3 = 14
        BankTown = 15
        BankPCode = 16
        BankRegion = 17
        BankCountry = 18
        CCNum = 19
        CCStartDate = 20
        CCExpiryDate = 21
        CCIssueNum = 22
        CCPIN = 23
        IsRegistered = 24
        CCAdd1 = 25
        CCAdd2 = 26
        CCAdd3 = 27
        CCTown = 28
        CCPCode = 29
        CCCountry = 30
        IsDeleted = 31
        NameOnCard = 32
        CCManualAuthorisationNum = 33
        PFLINKEXISTS = 34
        CLILINKEXISTS = 35
        CPLINKEXISTS = 36
        BIC = 37
        IBAN = 38
    End Enum

    Private Enum ENPMLookups
        Id = 0
        Description = 1
    End Enum

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As gPMConstants.PMEReturnCode
    Private m_lNavigate As Integer
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private m_sTransactionType As String = ""
    Private m_lMTAType As Integer
    Private m_dtEffectiveDate As Date

    ' Local variables
    Private m_lPartyCnt As Integer
    Private m_sShortName As String = ""
    Private m_lFinancePlanCnt As Integer
    Private m_lFinancePlanVersion As Integer
    Private m_sCurrencyISOCode As String
    Public m_vFinancePlanArray As Object = Nothing
    Private m_vFinancePlanTransArray As Object = Nothing
    Private m_vFinancePlanMTATransArray As Object = Nothing
    Private m_vFinancePlanInstalmentArray(,) As Object = Nothing
    Private m_lClientID As Integer
    Private m_vBankAccountDetails As Object = Nothing
    Private m_vCreditCardDetails As Object = Nothing
    Private m_vPFFrequencies(,) As Object = Nothing
    Private m_vAgentDetails As Object = Nothing
    Private m_sCreditCardNo As String = ""
    Private m_iMTAType As Integer
    'Flag to conditionally make the CC details mandatory
    Private m_bMandatoryCCDetails As Boolean
    Private m_bRequireSGCompanyDetails As Boolean

    'PN12594
    Private m_sBusinessCode As String = ""
    Private m_lBusinessCode_id As Integer

    'DC220206 PN26057 keep track of when bank details changed
    Private m_bBankAccountDetailsChange As Boolean

    'Thinh Nguyen 01/02/2004 - insurance file count of current plan
    Private m_lPlanInsuranceFolderCnt As Integer

    Private m_bUpdate As Boolean
    Private m_bNewTrans As Boolean
    'Private m_bWasInterestFree As Boolean          ' TR  24/03/2003 : TS17 Recovery By Instalments changes
    'Private m_bDeposit As Boolean                  ' RAW 11/03/2003 : ISS2580 : removed
    Private m_sRunningContext As String = ""

    'Variables for History
    Private m_bHistory As Boolean
    Private m_vFinanceHistory(,) As Object = Nothing
    Private m_lHistoryVersion As Integer
    Private m_ofrmHistory As iPMBFinancePlanMaint.frmHistory

    'Variables for Merges
    Private m_vFinanceMerges As Object = Nothing
    Private m_vFinanceParents As Object = Nothing
    Private m_bSpawned As Boolean

    'Variables for Transactions
    Private m_ofrmTransactions As iPMBFinancePlanMaint.frmTransactions
    Private m_vTransactionArray(,) As Object = Nothing

    'Variables for Children
    Private m_vFinanceChildren As Object = Nothing

    'Variables for Transactions
    Private m_ofrmInstalment As iPMBFinancePlanMaint.frmInstalment

    'Scheme Selection Variables
    Public m_sProductName As String = ""

    'Public m_sTransType As String
    Public m_vProductClassCodes As Object = Nothing

    ' Client Data Variables
    Private m_sStatusInd As String = ""
    Private m_lSchemeTypeID As Integer
    Private m_lPFRFID As Integer
    Private m_lMediaTypeID As Integer
    Private m_BankMandatory As Boolean

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMBFinancePlanMaint.General

    ' Declare an instance of the nav starter
    Private WithEvents m_oNavStart As iPMNavStart.Interface_Renamed
    Private m_lDepositTransId As Integer
    'PN11263
    Private m_cDepositAmount As Decimal
    Private m_lAccountID As Integer
    'PN11263End

    'ACR 24-04-05 Added to store partner details

    Private m_vPartnerArray As Object = Nothing
    'added to store original transaction array
    Private m_vSGTransactionArray As Object = Nothing
    'END ACR 24-04-05

    ' Variables to store the lookup values/details.

    Private m_vLookupValues As Object = Nothing
    Private m_vLookupDetails As Object = Nothing

    ' Stores the return value for the a function call.
    Private m_lReturn As Integer

    ' Declare an instance of the Lock object.

    Private m_oPMLock As bPMLock.User

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control
    ' Stores the search data from the business object.
    Public m_vSearchData As Object = Nothing
    Private m_bNavCompleted As Boolean
    Private m_bProcessComplete As Boolean
    Private m_sUnderwriting As String = ""
    ' RAM20030404 : Flag to notify whether, we are using existing plan details
    ' if this flag is set, then we need to diable the controls
    ' holding the bank account information Ref. Issue 2915
    Private m_bUseExistingBankDetails As Boolean
    Private m_bUseExistingCreditCardDetails As Boolean ' For Credit Cards
    ' TR  24/03/2003 : TS17 Recovery By Instalments changes
    Private m_bMediaViaThirdParty As Boolean
    Private m_sRecoveryAgentPartyTypeCode As String = ""
    Private m_bIsDocumasterInstalled As Boolean
    Private m_lOriginalRecoveryAgentCnt As Integer
    Private m_dtOriginalReviewDate As Date
    Private m_sOriginalStatementFrequency As String = ""
    Private m_sClientDocSetting As String = ""
    Private m_bCreditCardOrBankDetailsChanged As Boolean
    Private m_bReviewDateChanged As Boolean
    Private m_vPreviousFinancePlanArray(,) As Object = Nothing
    Private m_bUseVersionHighlighting As Boolean
    Private m_bBankDetailsAdded As Boolean
    Private m_bBankDetailsEdited As Boolean
    Private m_bCreditCardEdited As Boolean
    ' TR  24/03/2003 : TS17 Recovery By Instalments changes

    'TR - Added for PL 28/03/03
    Private m_sCCNoGot As String = ""

    'DD 31/07/2003: Added for Premium Finance

    Private m_oPartyFP As bSIRPartyFP.Business

    Private m_oParty As bSIRParty.Services
    Private m_bRequireDateOfBirth As Boolean

    Private m_vDateOfBirth As Object = Nothing
    Private m_bRequireCompanyReg As Boolean
    Private m_sCompanyReg As String = ""
    Private m_bRequireRefundType As Boolean

    'DD 17/11/2003: Added for Premium Finance

    Private m_oPFScheme As bSIRPFScheme.Business

    'SMJB CQ2155 02/09/03
    Private m_lInsuranceFile_cnt As Integer
    Private m_lMTAVersion As Integer

    'PN 47714
    Private m_bChangeAccountType As Boolean

    'PN: 48187
    Private m_sAccountCode As String = ""
    Private m_sBankBranchCode As String = ""
    Private m_bBankAccountValidated As Boolean
    Private m_sBIC As String = ""
    Private m_sIBAN As String = ""

    ' START CHANGES - Changed By: AAB  - Changed On: 08-Oct-2003 11:17
    ' Added to use support the deposit transaction
    Private Const PFTransactionCreate As Integer = 1
    Private Const PFTransactionCancel As Integer = 2
    Private Const PFTransactionFirst As Integer = 3
    Private Const PFTransactionOngoing As Integer = 4
    Private Const PFTransactionRepresent As Integer = 5
    Private Const PFTransactionLast As Integer = 6
    Private Const PFTransactionDeposit As Integer = 7

    Private m_lSilentTransact As Integer

    Private m_sProviderCode As String = "" 'The finance provider code

    Private m_vMediaTypeHistory(,) As Object = Nothing

    Private m_bCCCancelled As Boolean
    Private m_bDDCancelled As Boolean
    Private m_bDDReinstated As Boolean
    Private m_IsMediaTypeChanged As Boolean
    Private m_sOldMediaTypeCode As String = ""
    Private m_bOnlyAccountNameChanged As Boolean
    Private m_bReferenceExist As Boolean
    Private m_bLoading As Boolean
    Private m_bDontDeleteScheme As Boolean
    Private m_dOutStandingDeposit As Decimal
    ' Removed system options m_bIsCCAuthorisationOff and m_bIsIgnore9
    ' Adding a system option m_bIsExternalCreditCardProcessing
    Private m_bIsExternalCreditCardProcessing As Boolean
    Private m_iSchemeCountryID As Integer = 0

    Private m_oOldTransArray, m_oOldPFArray As Object
    Dim obSIRPFInstalments As Object
    '********************************************
    '*********** LVWHISTORY COLUMNS *************
    '********************************************

    ' column header indexes

    Public Enum HistoryListViewColIndex
        ActionCode = 1
        DateModified = 2
        PaymentType = 3
        AccountType = 4
        Name_Renamed = 5
        Branch = 6
        AccountName = 7
        AccountCode = 8
        AccountNumber = 9
        BIC = 10
        IBAN = 11
        User = 12
        AddressLine1 = 13
        PostCode = 14
        PaperDD = 15
    End Enum

    Public Enum CCHistoryListViewColIndex
        ActionCode = 1
        DateModified = 2
        AccType
        CCNumber
        CCStartDate
        CCExpiryDate
        CardHolderName
        CCIssueNo
        CCIsRegisterCardHolder
        CardHolderAddressLine1
        CardHolderPostCode
        User
    End Enum

    ' column header keys
    Private Const colHKeyActionCode As String = "ActionCode"
    Private Const colHKeyDateModified As String = "DateModified"
    Private Const colHKeyBankName As String = "BankName"
    Private Const colHKeyBranch As String = "BankBranch"
    Private Const colHKeyAccountName As String = "BankAccountName"
    Private Const colHKeyBankAccountCode As String = "BankAccountCode"
    Private Const colHKeyAccountNumber As String = "BankAccountNumber"
    Private Const colHKeyUser As String = "Username"
    Private Const colHKeyAddressLine1 As String = "BankAddressLine1"
    Private Const colHKeyPostCode As String = "BankPostCode"
    Private Const colHKeyPaperDD As String = "Paper DD"
    Private Const colHKeyCCNumber As String = "CCNumber"
    Private Const colHKeyCCStartDate As String = "CCStartDate"
    Private Const colHKeyExpiryDate As String = "CCExpiryDate"
    Private Const colHKeyCardHolderName As String = "CardHolderName"
    Private Const colHKeyCardHolderAddressLine1 As String = "CardHolderAddressLine1"
    Private Const colHKeyCardHolderPostCode As String = "CardHolderPostCode"
    Private Const colHKeyPaymentType As String = "PaymentType"
    Private Const colHKeyAccountType As String = "AccountType"
    Private Const kcolHKeyCCIssueNo As String = "CCIssueNo"
    Private Const kcolHKeyCCIsRegisteredCardHolder As String = "CCIsRegisteredCardHolder"
    Private m_oNextInstalmentDate As Object

    Private bIsEnableBankControls As Boolean
    Private nBankSelectedItem As Integer
    Private nBankSelectedCount As Integer
    Private sCCPinNo As String
    Private m_iEditableInstalmentNumber As Integer
    Private m_dtNextInstalmentDuedate As Date
    ' column header keys

    ' END CHANGES - Changed By: AAB  - Changed On: 08-Oct-2003 11:17

    ' PUBLIC Property Procedures (Begin)

    'Start (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)
    ' Removed Property IsCCAuthorisationOff
    ' Adding Property IsExternalCreditCardProcessing

    Public Property IsExternalCreditCardProcessing() As Boolean
        Get
            Return m_bIsExternalCreditCardProcessing
        End Get
        Set(ByVal Value As Boolean)
            m_bIsExternalCreditCardProcessing = Value
        End Set
    End Property
    Public Property AccountID() As Integer
        Get
            Return m_lAccountID
        End Get
        Set(ByVal value As Integer)
            m_lAccountID = value
        End Set
    End Property
    'End (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)

    Public WriteOnly Property SilentTransact() As Integer
        Set(ByVal Value As Integer)
            m_lSilentTransact = Value
        End Set
    End Property

    'ACR 12-07-05 added to persist the original transaction array
    Public WriteOnly Property SGTransactionArray() As Object
        Set(ByVal Value As Object)

            m_vSGTransactionArray = Value
        End Set
    End Property
    Public ReadOnly Property ErrorNumber() As Integer
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber
        End Get
    End Property
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
    End Property
    Public WriteOnly Property Task() As Integer
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property
    Public Property Status() As Integer
        Get
            ' Return the interface exit status.
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property
    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)
            ' Set the navigate flag.
            m_lNavigate = Value
        End Set
    End Property
    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)
            ' Set the process mode.
            m_lProcessMode = Value
        End Set
    End Property
    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            ' Set the type of business.
            m_sTransactionType = Value
        End Set
    End Property
    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)
            ' Set the effective date.
            m_dtEffectiveDate = Value
        End Set
    End Property
    Public Property partyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property
    Public Property ShortName() As String
        Get
            Return m_sShortName
        End Get
        Set(ByVal Value As String)
            m_sShortName = Value
        End Set
    End Property
    Public Property FinancePlanCnt() As Integer
        Get
            Return m_lFinancePlanCnt
        End Get
        Set(ByVal Value As Integer)
            m_lFinancePlanCnt = Value
        End Set
    End Property
    Public Property FinancePlanVersion() As Integer
        Get
            Return m_lFinancePlanVersion
        End Get
        Set(ByVal Value As Integer)
            m_lFinancePlanVersion = Value
        End Set
    End Property
    Public Property MTAVersion() As Integer
        Get
            Return m_lMTAVersion
        End Get
        Set(ByVal Value As Integer)
            m_lMTAVersion = Value
        End Set
    End Property
    Public WriteOnly Property FinancePlanTransArray() As Object
        Set(ByVal Value As Object)

            m_vFinancePlanTransArray = Value
        End Set
    End Property
    Public Property MTAType() As Integer
        Get
            Return m_iMTAType
        End Get
        Set(ByVal Value As Integer)
            m_iMTAType = Value
        End Set
    End Property
    Public Property FinancePlanArray() As Object
        Get
            Return VB6.CopyArray(m_vFinancePlanArray)
        End Get
        Set(ByVal Value As Object)
            m_vFinancePlanArray = Value
        End Set
    End Property
    Public Property FinancePlanMTATransArray() As Object
        Get
            Return m_vFinancePlanMTATransArray
        End Get
        Set(ByVal Value As Object)

            m_vFinancePlanMTATransArray = Value
        End Set
    End Property

    Public Property History() As Boolean
        Get
            Return m_bHistory
        End Get
        Set(ByVal Value As Boolean)
            m_bHistory = Value
        End Set
    End Property
    Public Property Spawned() As Boolean
        Get
            Return m_bSpawned
        End Get
        Set(ByVal Value As Boolean)
            m_bSpawned = Value
        End Set
    End Property
    Public Property RunningContext() As String
        Get
            Dim result As String = String.Empty
            m_sRunningContext = result
            Return result
        End Get
        Set(ByVal Value As String)
            m_sRunningContext = Value
        End Set
    End Property
    Public WriteOnly Property DontDeleteScheme() As Boolean
        Set(ByVal Value As Boolean)
            m_bDontDeleteScheme = Value
        End Set
    End Property

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' RAM20030404 : Added the following 4 properties
    '               Ref. Issue 2915 Changes
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public WriteOnly Property SelectedBankAccountDetails() As Object
        Set(ByVal Value As Object)

            m_vBankAccountDetails = Value
        End Set
    End Property

    Public WriteOnly Property SelectedCreditCardDetails() As Object
        Set(ByVal Value As Object)

            m_vCreditCardDetails = Value
        End Set
    End Property

    Public WriteOnly Property UseExistingBankDetails() As Boolean
        Set(ByVal Value As Boolean)
            m_bUseExistingBankDetails = Value
        End Set
    End Property

    Public WriteOnly Property UseExistingCreditCardDetails() As Boolean
        Set(ByVal Value As Boolean)
            m_bUseExistingCreditCardDetails = Value
        End Set
    End Property
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' RAM20030404   : Issue 2915 Changes - END
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    '*************************************************************************
    'Name:          ShowThirdPartyRecoveryData
    'Description:   Populates the ThirdPartyRecovery Controls with data
    'History:       TR25/03/03 - Created
    '*************************************************************************
    Public Sub ShowThirdPartyRecoveryData()

        Try

            'TR - Get the Confirmed Date info
            If Information.IsDate(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDateConfirmed, 0)) Then
                dtpConfirmedDate.Value = CDate(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDateConfirmed, 0))
            Else

                dtpConfirmedDate.Value = Nothing
            End If
            'Call ApplyVersionHighlighting(k_PFPlanDateConfirmed, dtpConfirmedDate)

            'TR - Get the Review Date info
            If Information.IsDate(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDateReview, 0)) Then
                If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDateReview, 0)) = "00:00:00" Then
                    dtpReviewDate.Format = DateTimePickerFormat.Custom
                    dtpReviewDate.CustomFormat = " "

                    dtpReviewDate.Value = Nothing
                Else
                    dtpReviewDate.Format = DateTimePickerFormat.Short
                    dtpReviewDate.Value = CDate(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDateReview, 0))
                End If
            Else
                SetReviewedFromConfirmedDate()
            End If
            'Call ApplyVersionHighlighting(k_PFPlanDateReview, dtpReviewDate)

            'TR - Get the "no-Statement" status. Null means not set yet
            Dim dbNumericTemp As Double

            If Convert.IsDBNull(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanNoStatements, 0)) Or IsNothing(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanNoStatements, 0)) Or Not Double.TryParse(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanNoStatements, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                'TR - By default select unchecked
                chkNoStatements.CheckState = CheckState.Unchecked
            Else
                chkNoStatements.CheckState = (m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanNoStatements, 0))
            End If
            'Call ApplyVersionHighlighting(k_PFPlanNoStatements, chkNoStatements)

            'TR - Populate the Statement Frequency Combo box
            PopulateFrequencyCombo()
            'TR - Enable/disable Statement Frequency based on chkNoStatements
            SetStatementsCombo()
            'TR - Get the Agent Tab details
            DisplayAgentDetails()

        Catch excep As System.Exception

            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "There was an error populating the " & "controls for ThirdPartyRecovery", ACApp, ACClass, "ShowThirdPartyRecoveryData", Information.Err().Number, excep.Message, excep:=excep)
            Exit Sub
        End Try

    End Sub

    '*************************************************************************
    'Name:          DisplayAgentDetails
    'Description:   Populates the Agent Details tab
    'History:       TR25/03/03 - Created
    '*************************************************************************
    Public Sub DisplayAgentDetails()
        Dim oAgentParty As bSIRParty.Services

        Try

            'Populate the Agent's Ref
            txtAgentRef.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAgentRef, 0)).Trim()
            ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanAgentRef, txtAgentRef)

            'TR - Does this Plan have an Agent saved against it?
            If Conversion.Val(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAgentCnt, 0))) > 0 Then

                'TR - Create a Party bo to hold Agent details
                Dim temp_oAgentParty As Object = Nothing
                m_lReturn = g_oObjectManager.GetInstance(temp_oAgentParty, "bSIRParty.Services", gPMConstants.PMGetViaClientManager)
                oAgentParty = temp_oAgentParty
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MsgBoxSirius("Unable to get Party Agent Business")
                    Exit Sub
                End If

                'TR - Set the Agent ID

                oAgentParty.partyCnt = m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAgentCnt, 0)

                'TR - Get the Agent

                m_lReturn = oAgentParty.GetDetails
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MsgBoxSirius("Unable to get Party Agent Business")
                    Exit Sub
                End If

                'TR - Check that enough of the address is populated

                If oAgentParty.Address1.Trim().Length = 0 Or oAgentParty.Address2.Trim().Length = 0 Then
                    MsgBoxSirius("The address for this agent is" & Strings.Chr(13) & Strings.Chr(10) & "insufficient. Please correct this prior to" & Strings.Chr(13) & Strings.Chr(10) & "printing any documents", MsgBoxStyle.Exclamation, "Address Incomplete")
                End If

                'TR - Populate the screen
                With uctPMAgentAddressControl

                    .AddressLine1 = oAgentParty.Address1

                    .AddressLine2 = oAgentParty.Address2

                    .AddressLine3 = oAgentParty.Address3

                    .AddressLine4 = oAgentParty.Address4

                    .PostCode = oAgentParty.PostalCode

                    .CountryId = oAgentParty.CountryID
                End With

                txtAgent.Text = oAgentParty.Name
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanAgentCnt, txtAgent)

                txtAgentTelAreaCode.Text = oAgentParty.AreaCode

                txtAgentTelNumber.Text = oAgentParty.Number

                txtAgentTelExtension.Text = oAgentParty.Extension
            Else
                'TR - Blank the screen
                With uctPMAgentAddressControl
                    .AddressLine1 = ""
                    .AddressLine2 = ""
                    .AddressLine3 = ""
                    .AddressLine4 = ""
                    .PostCode = ""
                    'PN14258
                    '       .CountryID = 0
                    .CountryId = g_oObjectManager.CountryID
                    '
                End With
                txtAgent.Text = ""
                txtAgentTelAreaCode.Text = ""
                txtAgentTelNumber.Text = ""
                txtAgentTelExtension.Text = ""
            End If

            'TR - Diable the Agent control
            uctPMAgentAddressControl.Enabled = False

            'TR - Destroy the objects
            oAgentParty = Nothing

        Catch excep As System.Exception

            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "DisplayAgentDetails Failed", ACApp, ACClass, "DisplayAgentDetails", Information.Err().Number, excep.Message, excep:=excep)
            Exit Sub
        End Try
    End Sub

    '*************************************************************************
    'Name:          SelectAgent
    'Description:   Shows the Party search screen for the type of Party
    '               stated in System Option "RecoveryAgentPartyTypeCode"
    'History:       TR25/03/03 - Created
    '*************************************************************************
    Public Sub SelectAgent()
        Dim oFindAgent As iPMBFindParty.Interface_Renamed
        Dim vKeyArray(,) As Object = Nothing

        Try

            'TR - Create the Find Party object
            Dim temp_oFindAgent As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindAgent, "iPMBFindParty.Interface_Renamed", gPMConstants.PMGetLocalInterface)
            oFindAgent = temp_oFindAgent
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MsgBoxSirius("Could not display Find Agent Screen")
                oFindAgent = Nothing
                Exit Sub
            End If

            'TR - Set the process modes
            oFindAgent.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            'TR - Set the properties.
            ReDim vKeyArray(1, 0)

            vKeyArray(0, 0) = "special_party"

            vKeyArray(1, 0) = m_sRecoveryAgentPartyTypeCode

            m_lReturn = oFindAgent.SetKeys(vKeyArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MsgBoxSirius("Could not display Find Agent Screen")
                oFindAgent = Nothing
                Exit Sub
            End If

            oFindAgent.CallingAppName = m_sCallingAppName

            'TR - Display the search screen

            m_lReturn = oFindAgent.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MsgBoxSirius("Could not display Find Agent Screen")
                Exit Sub
            End If

            'TR - Make sure that the search screen exited ok & user did not cancel

            If oFindAgent.Status = gPMConstants.PMEReturnCode.PMOK Then
                'TR - Save selected Agent Cnt to the array holding the rest of
                'the plans details (but not to the db)

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAgentCnt, 0) = oFindAgent.PartyCnt
            End If

            'TR - Now populate the Agent Details Tab
            DisplayAgentDetails()

            ' Destroy Find Party object

            oFindAgent.Dispose()
            oFindAgent = Nothing

        Catch excep As System.Exception
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to process Find Party.", ACApp, ACClass, "SelectAgent", Information.Err().Number, excep.Message, excep:=excep)
            Exit Sub

        End Try

    End Sub

    '*************************************************************************
    'Name:          ShowHideThirdPartyRecoveryControls
    'Description:   Shows and Hides certain controls that are only displayed
    '               if the Plan is a Third Party Recovery one.
    'History:       TR25/03/03 - Created
    '*************************************************************************
    Public Sub ShowHideThirdPartyRecoveryControls()

        Try

            'TR - Assume we hide the Agents tab
            If tabMainTab.TabCount >= 5 Then
                SSTabHelper.SetTabVisible(tabMainTab, 5, False)
            End If

            'TR - Show or hide the controls
            If m_lSchemeTypeID = bSIRPremFinConst.PFThirdPartyRecovery Then
                dtpConfirmedDate.Visible = True
                lblConfirmedDate.Visible = True
                dtpReviewDate.Visible = True
                lblReviewedDate.Visible = True
                chkNoStatements.Visible = True
                'TR - If this Policy is Live, disable the Confirmed Date field
                dtpConfirmedDate.Enabled = Not (m_sStatusInd >= bSIRPremFinConst.PFStatusIndLive)
                cboStatementFrequency.Visible = True
                lblStatementFrequency.Visible = True
                'TR - Display the Agent Tab if the Media Type is Other Party
                If m_bMediaViaThirdParty Then
                    SSTabHelper.SetTabVisible(tabMainTab, 5, True)
                End If
                'TR - Populate the controls
                ShowThirdPartyRecoveryData()
            Else
                dtpConfirmedDate.Visible = False
                lblConfirmedDate.Visible = False
                dtpReviewDate.Visible = False
                lblReviewedDate.Visible = False
                chkNoStatements.Visible = False
                cboStatementFrequency.Visible = False
                lblStatementFrequency.Visible = False
            End If

        Catch excep As System.Exception

            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "There was an error Showing / Hiding " & "controls for ThirdPartyRecovery", ACApp, ACClass, "ShowHideThirdPartyRecoveryControls", Information.Err().Number, excep.Message, excep:=excep)
            Exit Sub
        End Try
    End Sub

    '*************************************************************************
    'Name:          SetStatementsCombo
    'Description:   When the user changes whether or not Statements are
    '               required, the Statements combo box must be reset
    'History:       TR25/03/03 - Created
    '*************************************************************************
    Public Sub SetStatementsCombo()

        Try

            'TR - Are Statements required?
            If chkNoStatements.CheckState = CheckState.Unchecked Then
                'TR - Yes please - want statements
                Dim dbNumericTemp As Double
                If Not Double.TryParse(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStatementPFFrequencyID, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    'TR - No saved data - default to null
                    cboStatementFrequency.SelectedIndex = -1
                Else
                    'TR - Default to previously saved data
                    For lCount As Integer = 0 To cboStatementFrequency.Items.Count - 1
                        'TR - See if this Item Data matches the one
                        If VB6.GetItemData(cboStatementFrequency, lCount) = Conversion.Val(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStatementPFFrequencyID, 0))) Then
                            cboStatementFrequency.SelectedIndex = lCount
                            m_sOriginalStatementFrequency = VB6.GetItemString(cboStatementFrequency, lCount)
                            Exit For
                        End If
                    Next lCount
                End If
                cboStatementFrequency.Enabled = True
                lblStatementFrequency.Enabled = True
                lblStatementFrequency.Font = VB6.FontChangeBold(lblStatementFrequency.Font, True)
            Else
                'TR - No thanks. No statements required
                cboStatementFrequency.SelectedIndex = -1
                cboStatementFrequency.Enabled = False
                lblStatementFrequency.Enabled = False
                lblStatementFrequency.Font = VB6.FontChangeBold(lblStatementFrequency.Font, False)
            End If

        Catch excep As System.Exception

            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "SetStatementsCombo Failed", ACApp, ACClass, "SetStatementsCombo", Information.Err().Number, excep.Message, excep:=excep)
            Exit Sub
        End Try
    End Sub

    '*************************************************************************
    'Name:          SetReviewedFromConfirmedDate
    'Description:   When the user changes the Confirmed Date, this changes
    '               the reviewed date to a default of plus 1 year
    'History:       TR25/03/03 - Created
    '*************************************************************************
    Public Sub SetReviewedFromConfirmedDate()

        Try

            'TR - Check that a date has been selected, if not then display blank

            If Convert.IsDBNull(dtpConfirmedDate.Value) Or IsNothing(dtpConfirmedDate.Value) Then
                dtpConfirmedDate.Format = DateTimePickerFormat.Custom
                dtpConfirmedDate.CustomFormat = " "
                dtpReviewDate.Format = DateTimePickerFormat.Custom
                dtpReviewDate.CustomFormat = " "

                dtpReviewDate.Value = Nothing
            Else
                'TR - Ok to display date, and review date which is Date + 1 year
                dtpConfirmedDate.Format = DateTimePickerFormat.Short
                dtpReviewDate.Format = DateTimePickerFormat.Short
                'TR - Set the Review Date to default to Confirmed Date plus 1 year

                dtpReviewDate.Value = DateTime.FromOADate(dtpConfirmedDate.Value.ToOADate()).AddYears(1)
            End If

        Catch excep As System.Exception

            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "SetReviewedFromConfirmedDate Failed", ACApp, ACClass, "SetReviewedFromConfirmedDate", Information.Err().Number, excep.Message, excep:=excep)
            Exit Sub
        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business objects
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim nResult As Integer = 0
        Dim sRecoveryAgentPartyTypeID As String = String.Empty
        Dim sDocumasterInstalled As String = String.Empty

        'DD 31/07/2003
        Dim nFinanceProviderCnt As Integer
        Dim nRequireDOB, nRequireCompanyReg As Integer
        Dim nUpper As Integer

        Const k_sFUNCTION_NAME As String = "GetBusiness"

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Disable parts of the interface while a search is in progress.
            m_lReturn = DisableInterface(True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Premium Finance Plan

            m_lReturn = g_oBusiness.GetSingleFinancePlan(m_lFinancePlanCnt, m_lFinancePlanVersion, m_vFinancePlanArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(m_vFinancePlanArray) Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Failed to get the Premium Finance " & "Record", ACApp, ACClass, k_sFUNCTION_NAME)
                Return nResult
            End If
            m_lReturn = g_oBusiness.GetSingleFinancePlanNextInstDate(m_lFinancePlanCnt, m_lFinancePlanVersion,
                                                                    m_oNextInstalmentDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GetBusiness = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            g_oBusiness.GetSourceCountryCode(v_lSourceID:=CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanSource_ID, 0)), r_iCountryId:=m_iSchemeCountryID)
            'DD 31/07/2003: Check to see if we need the Finance Provider Object
            If m_oPartyFP Is Nothing And (CDbl(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanSchemeType_ID, 0)) = bSIRPremFinConst.PFThirdParty Or CDbl(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanSchemeType_ID, 0)) = bSIRPremFinConst.PFThirdPartyViaStargate) Then

                ' Get an instance of the Finance Provider business object via
                ' the public object manager.
                Dim temp_m_oPartyFP As Object = Nothing
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oPartyFP, "bSIRPartyFP.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oPartyFP = temp_m_oPartyFP

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Failed to get the Finance Provider " & "Business Object", ACApp, ACClass, k_sFUNCTION_NAME)
                    Return nResult
                End If
            End If

            'DD 31/07/2003: Check to see if we need the PFScheme Object
            If m_oPFScheme Is Nothing And CDbl(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanSchemeType_ID, 0)) = bSIRPremFinConst.PFThirdParty Then

                ' Get an instance of the Finance Provider business object via
                ' the public object manager.
                Dim temp_m_oPFScheme As Object = Nothing
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oPFScheme, "bSIRPFScheme.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oPFScheme = temp_m_oPFScheme

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Failed to get the PFScheme " & "Business Object", ACApp, ACClass, k_sFUNCTION_NAME)
                    Return nResult
                End If
            End If

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanProductClass, 0)).Trim() = "SG" Then
                Try
                    nUpper = -1
                    nUpper = m_vPartnerArray.GetUpperBound(1)
                Catch ex As Exception

                End Try

                If nUpper < 0 Then
                    If gPMFunctions.ToSafeLong(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanInsuranceFileCnt, 0), 0) <> 0 Then

                        m_lReturn = g_oBusiness.GetPartners(CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanInsuranceFileCnt, 0)), m_vPartnerArray)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            RaiseError("GetBusiness", "Call to GetPartners Failed.")
                        End If
                    End If
                End If
            End If

            'DD 31/07/2003: Now see if we need to get extra details from the Party
            If CDbl(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanSchemeType_ID, 0)) = bSIRPremFinConst.PFThirdParty Or CDbl(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanSchemeType_ID, 0)) = bSIRPremFinConst.PFThirdPartyViaStargate Then

                'Get the Finance Provider from the Scheme

                m_lReturn = g_oBusiness.GetFinanceProviderDetails(v_lCompanyNo:=gPMFunctions.ToSafeLong(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCompanyNo, 0)), v_lSchemeNo:=gPMFunctions.ToSafeLong(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanSchemeNo, 0)), v_lSchemeVersion:=gPMFunctions.ToSafeLong(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanSchemeVersion, 0)), r_lPartyCnt:=nFinanceProviderCnt, r_sShortName:=m_sProviderCode)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Call to g_oBusiness.GetFinanceProviderDetails " &
                                       "failed.", ACApp, ACClass, k_sFUNCTION_NAME)
                    Return nResult
                End If

                'Get the flags for additional fields

                m_lReturn = m_oPartyFP.GetNext(vPartyCnt:=nFinanceProviderCnt, vDOB:=nRequireDOB, vCompanyReg:=nRequireCompanyReg)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Call to m_oPartyFP.GetNext failed.", ACApp, ACClass, k_sFUNCTION_NAME)
                    Return nResult
                End If
            End If

            If CDbl(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanSchemeType_ID, 0)) = bSIRPremFinConst.PFThirdParty Or CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanProductClass, 0)).Trim() = "SG" Then
                'Store the flags
                m_bRequireDateOfBirth = (nRequireDOB = 1)
                m_bRequireCompanyReg = (nRequireCompanyReg = 1) Or (CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanProductClass, 0)).Trim() = "SG")

                'MKW PN13910 Create Party.Services regardless of option as used elsewhere
                'Now, do we need do get the information from the Party
                'If m_bRequireDateOfBirth Or m_bRequireCompanyReg Then
                If m_oParty Is Nothing Then
                    ' Get an instance of the Party Services object via
                    ' the public object manager.
                    Dim temp_m_oParty As Object = Nothing
                    m_lReturn = g_oObjectManager.GetInstance(temp_m_oParty, "bSIRParty.Services", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                    m_oParty = temp_m_oParty

                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Failed to get the Party Services " & "Business Object", ACApp, ACClass, k_sFUNCTION_NAME)
                        Return nResult
                    End If
                End If

                'Load up the party

                m_oParty.PartyCnt = m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientId, 0)

                m_lReturn = m_oParty.GetDetails()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "m_oParty.GetDetails failed.", ACApp, ACClass, k_sFUNCTION_NAME)
                    Return nResult
                End If

                'DC120804 PN14147 unset mandatory fields if not applicable

                If m_oParty.PartyType <> "PC" Then
                    m_bRequireDateOfBirth = False
                End If

                If m_oParty.PartyType <> "CC" Then
                    m_bRequireCompanyReg = False
                End If

                'MKW PN13910 Only do following if options true.
                If m_bRequireDateOfBirth Or m_bRequireCompanyReg Then
                    If m_bRequireDateOfBirth Then
                        'Trap null

                        If m_oParty.DateOfBirth = #12/29/1899# Then
                            m_vDateOfBirth = ""
                        Else

                            m_vDateOfBirth = m_oParty.DateOfBirth
                        End If
                    End If

                    If m_bRequireCompanyReg Then

                        m_sCompanyReg = gPMFunctions.NullToString(m_oParty.CompanyReg)
                    End If
                End If

            End If

            'TR - Save the original Agent's ID locally
            m_lOriginalRecoveryAgentCnt = CInt(Conversion.Val(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAgentCnt, 0))))
            'TR - Save the original Review Date
            If Information.IsDate(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDateReview, 0)) Then
                m_dtOriginalReviewDate = CDate(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDateReview, 0))
            Else

                m_dtOriginalReviewDate = #12/29/1899#
            End If

            'TR - 24/03/03 - TS17 4.1.5.4 Version Highlighting
            'TR - See if this Plan is the first version
            If CDbl(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFVersion, 0)) > 1 Then
                'TR - Get the previous version of the plan (= this ver minus 1)

                m_lReturn = g_oBusiness.GetSingleFinancePlan(m_lFinancePlanCnt, Conversion.Val(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFVersion, 0))) - 1, m_vPreviousFinancePlanArray)
                'TR - Only switch on Version Highlighting if this previous Plan
                'has been successfully loaded here
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue And Information.IsArray(m_vPreviousFinancePlanArray) Then
                    'TR - switch on Version Highlighting
                    m_bUseVersionHighlighting = True
                End If

                If CDbl(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanSchemeType_ID, 0)) = bSIRPremFinConst.PFThirdPartyRecovery Then
                    'TR - Get the RecoveryAgentPartyType System Option
                    m_lReturn = iPMFunc.GetSystemOption(1028, sRecoveryAgentPartyTypeID)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Could not get System Option: " & "RecoveryAgentPartyType", ACApp, ACClass, k_sFUNCTION_NAME)
                        Return nResult
                    End If

                    'TR - Get the Party Type Code

                    m_lReturn = g_oBusiness.GetPartyTypeCode(Conversion.Val(sRecoveryAgentPartyTypeID), m_sRecoveryAgentPartyTypeCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Could not get Party Type Code for: " & sRecoveryAgentPartyTypeID, ACApp, ACClass, k_sFUNCTION_NAME)
                        Return nResult
                    End If
                End If
            End If

            If Not Information.IsArray(m_vFinancePlanTransArray) Then
                ' Get transactions for the Finance Plan

                m_lReturn = g_oBusiness.GetFinancePlanTransactions(vFinancePlanCnt:=m_lFinancePlanCnt, vFinancePlanVersion:=m_lFinancePlanVersion, vFinancePlanTransactionArray:=m_vFinancePlanTransArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Failed to get the Transactions for" & " this Plan", ACApp, ACClass, k_sFUNCTION_NAME)
                    Return nResult
                End If
            End If

            If Not (Information.IsArray(m_vSGTransactionArray)) Then
                ' Get transactions for the Finance Plan

                m_lReturn = g_oBusiness.GetSGFinancePlanTransactions(vFinancePlanCnt:=m_lFinancePlanCnt, vFinancePlanVersion:=m_lFinancePlanVersion, vFinancePlanTransactionArray:=m_vSGTransactionArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Failed to get SG Transactions for" & " this Plan", ACApp, ACClass, k_sFUNCTION_NAME)
                    Return nResult
                End If
            End If

            'Save the number of transactions to the plan object
            If Information.IsArray(m_vFinancePlanTransArray) Then

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanTransactionCount, 0) = m_vFinancePlanTransArray.GetUpperBound(1) + 1
            End If

            'TR - Get all the instalments for this plan

            m_lReturn = g_oBusiness.GetFinancePlanInstalments(v_lFinancePlanCnt:=m_lFinancePlanCnt, v_lFinancePlanVersion:=m_lFinancePlanVersion, r_vFinancePlanArray:=m_vFinancePlanInstalmentArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Failed to get the Instalments for " & "this Plan", ACApp, ACClass, k_sFUNCTION_NAME)
                Return nResult
            End If

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanProductClass, 0)).Trim() = "MTA" Then
                If m_vFinancePlanTransArray IsNot Nothing Then
                    If Not Information.IsArray(m_vFinancePlanMTATransArray) Then
                        m_vFinancePlanMTATransArray = m_vFinancePlanTransArray
                        If m_sTransactionType = "MTA" Then
                            For iFPCnt As Integer = 0 To UBound(m_vFinancePlanMTATransArray, 2)
                                m_vFinancePlanMTATransArray(2, iFPCnt) = 0
                            Next
                        Else
                            For iFPCnt As Integer = 0 To UBound(m_vFinancePlanMTATransArray, 2)
                                m_vFinancePlanMTATransArray(2, iFPCnt) = m_vFinancePlanMTATransArray(4, iFPCnt)
                            Next
                        End If
                    End If
                End If
            End If

            'TR - Get the Is Documaster Installed System Option
            m_lReturn = iPMFunc.GetSystemOption(10, sDocumasterInstalled)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Could not get System Option: " & "Is Documaster Installed", ACApp, ACClass, k_sFUNCTION_NAME)
                Return nResult
            End If

            'Set flag if system option says that DocuMaster is installed
            m_bIsDocumasterInstalled = sDocumasterInstalled.Trim() = "1"

            ' get pfmediatypehistory

            m_lReturn = g_oBusiness.GetPFMediaTypeHistory(v_lPFPremiumFinanceId:=m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFCnt, 0), v_lPFPremiumFinanceVersion:=m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFVersion, 0), r_vResults:=m_vMediaTypeHistory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Failed to get media type history", ACApp, ACClass, k_sFUNCTION_NAME)
                Return nResult
            End If

            m_lReturn = g_oBusiness.IsSinglePlanParty(v_lPartyCnt:=gPMFunctions.ToSafeLong(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanInsuranceFileCnt, 0)), r_vSinglePlanParty:=aoPlanPartyArray, v_sPlanStatusInd:=gPMFunctions.ToSafeString(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStatusInd, 0)))

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'm_lErrorNumber& = PMFalse
                'Exit Sub
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Failed to get media type history", ACApp, ACClass, k_sFUNCTION_NAME)
                Return nResult
            End If

            If Information.IsArray(aoPlanPartyArray) Then
                bIsSinglePlanParty = CBool(aoPlanPartyArray(0, 0))

                m_lReturn = g_oBusiness.GetAgent(v_lInsuranceFileCnt:=m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanInsuranceFileCnt, 0), r_lLeadAgent:=lLeadAgentCnt)
            End If

            Return nResult

        Catch ex As Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "There was an error retrieving data " & "from the business objects", ACApp, ACClass, k_sFUNCTION_NAME, excep:=ex)

        End Try

        Return nResult
    End Function

    ' ***************************************************************** '
    ' Name: BusinessToData
    ' Description: Updates the data storage from the business object.
    ' History:
    '   PF210901 - Removed unneccesary local variable references
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'TR - Populate the modular variables from the business objects' data
            m_lPartyCnt = CInt(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientId, 0)).Trim())
            m_sShortName = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientName, 0)).Trim()
            iSIRPremFinConst.m_sTransType = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanTransactionType, 0)).Trim().ToUpper()
            m_sStatusInd = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStatusInd, 0)).Trim()

            If m_bDontDeleteScheme = True Then m_sStatusInd = PFStatusIndSaved

            If m_sStatusInd = bSIRPremFinConst.PFStatusIndCancelled Then
                m_iTask = gPMConstants.PMEComponentAction.PMView
            End If
            m_lClientID = CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientId, 0))
            m_lMediaTypeID = CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanMediaType_ID, 0))
            'TR - 24/03/03 - TS17 Recovery By Instalments changes
            'm_lMediaTypeValidation = m_vFinancePlanArray(k_PFPlanMediaTypeValidation, 0)
            m_lSchemeTypeID = CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanSchemeType_ID, 0))
            'TR - 24/03/03 - TS17 Recovery By Instalments changes
            'TR - Get the Via Third Party info
            m_bMediaViaThirdParty = gPMFunctions.NullToInteger(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanMediaTypeIsViaThirdParty, 0))
            m_lPFRFID = CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFRF_ID, 0))
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    Private Function ResetIsMandatoryControls() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oFormFields.Item("txtAccountNumber-0").IsMandatory = False
            m_oFormFields.Item("txtAccountName-0").IsMandatory = False
            m_oFormFields.Item("txtCardNo-0").IsMandatory = False
            m_oFormFields.Item("txtExpiryDate-0").IsMandatory = False
            m_oFormFields.Item("txtBankName-0").IsMandatory = False
            m_oFormFields.Item("txtBranch-0").IsMandatory = False
            m_oFormFields.Item("txtSortCode-0").IsMandatory = False
            m_oFormFields.Item("txtPFReference-0").IsMandatory = False
            m_oFormFields.Item("cboBusinessCode-0").IsMandatory = False
            m_oFormFields.Item("txtDateOfBirth-0").IsMandatory = False

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business object.
    '              TR - 24/03/03 - TS17 Recovery By Instalments changes
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer

        Dim result As Integer = 0
        Dim vDepositOnOtherMediaType As String = ""
        Dim vAgentRefMandatory, vPFMessage As Object
        Dim vBusinessCodeMandatory As Object = Nothing
        Dim lTmp As Integer
        Dim lUpper As Integer
        Dim bFound As Boolean
        Dim vSchemeArray(,) As Object = Nothing
        Dim iIsPlanReferenceEditable As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.
            ' Assign the details from the business object to the data storage.
            m_lReturn = BusinessToData()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = ResetIsMandatoryControls()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_sCurrencyISOCode = ""

            m_lReturn = g_oBusiness.GetSchemeCurrencyISOCode(m_lFinancePlanCnt, m_lFinancePlanVersion, m_sCurrencyISOCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_dOutStandingDeposit = 0
            If m_iMTAType = bSIRPremFinConst.m_klInstalmentMTAType_NoAmountChange Then
                m_lReturn = g_oBusiness.GetOutStandingDeposit(v_lPremiumFinanceCnt:=m_lFinancePlanCnt,
                                                                 v_lPremiumFinancePlanVer:=m_lFinancePlanVersion,
                                                                 r_cDepositAmount:=m_dOutStandingDeposit)



            End If
            fraPayment.Text = "Breakdown" & " (" & m_sCurrencyISOCode & ")"
            fraSummary.Text = CStr(iPMFunc.GetResData(g_iLanguageID, ACFraSummary, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)) & " (" & m_sCurrencyISOCode & ")"


            ' Assign the details to the interface.
            'Non Amendable Stuff goes here
            Text = "Finance Plan: " & CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAutoGenPlanRef, 0)).Trim()

            ' Set start date
            Select Case m_sStatusInd
                Case bSIRPremFinConst.PFStatusIndSaved, bSIRPremFinConst.PFStatusIndUpdated, bSIRPremFinConst.PFStatusIndQuotePrinted
                    If m_lSchemeTypeID <> bSIRPremFinConst.PFThirdParty Then
                        If CDbl(m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanInstAlign_to, 0)) = 0 Then 'Renewal date
                            m_lReturn = m_oFormFields.FormatControl(txtStartDate, DateTime.Today.AddDays(CDbl(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDaysDelay, 0))))
                            ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanDaysDelay, txtStartDate)
                            txtStartDate.Enabled = True
                            lblStartDate.Font = VB6.FontChangeBold(lblStartDate.Font, True)
                        Else
                            m_lReturn = m_oFormFields.FormatControl(txtStartDate, m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStartDate, 0))
                            ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanStartDate, txtStartDate)
                        End If
                    Else
                        m_lReturn = m_oFormFields.FormatControl(txtStartDate, DateTime.Today)
                    End If
                Case Else
                    m_lReturn = m_oFormFields.FormatControl(txtStartDate, m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStartDate, 0))
                    ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanStartDate, txtStartDate)
            End Select

            txtDaysDelay.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDaysDelay, 0))
            ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanDaysDelay, txtDaysDelay)

            cboRefundType.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanRefundType, 0))

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanTerms, 0)) <> "" Then
                lTmp = CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanTerms, 0))
            End If

            txtTerms.Visible = lTmp > 0
            lblTerms.Visible = lTmp > 0
            txtTerms.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanTerms, 0))

            txtNumberOfInstalments.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanNoOfInstalments, 0) + m_dOutStandingDeposit)
            ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanNoOfInstalments, txtNumberOfInstalments)

            With m_oFormFields
                .FormatControl(txtFinancedAmount, m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAmountToFinance, 0))
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanAmountToFinance, txtFinancedAmount)

                .FormatControl(txtAPR, m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAPR, 0))
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanAPR, txtAPR)

                .FormatControl(txtInterestRate, m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanInterestRate, 0))
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanInterestRate, txtInterestRate)
                'tr - todo
                .FormatControl(txtFinanceCharge, gPMFunctions.NullToDouble(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanInterestCost, 0)) + gPMFunctions.NullToDouble(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanFinanceCharge, 0)))
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanInterestCost, txtFinanceCharge)

                .FormatControl(txtTaxes, m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanTaxCost, 0))
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanTaxCost, txtTaxes)

                .FormatControl(txtTotalAmount, m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanTotalCost, 0))
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanTotalCost, txtTotalAmount)

                .FormatControl(txtCostOfProtection, m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCostOfProtection, 0))
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanCostOfProtection, txtCostOfProtection)

                .FormatControl(txtDeposit, m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDeposit, 0))
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanDeposit, txtDeposit)

                .FormatControl(txtFirstInstalmentValue, m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanFirstInstalment, 0))
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanFirstInstalment, txtFirstInstalmentValue)

                .FormatControl(txtOtherInstalments, m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanOtherInstalments, 0))
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanOtherInstalments, txtOtherInstalments)

                'MKW24062004 PN12422 Don't populate date fields for 3rd Party (as providers set). START
                If m_lSchemeTypeID = bSIRPremFinConst.PFThirdParty Then

                    .FormatControl(txtFirstInstalmentDate, "")

                    .FormatControl(txtNextInstalmentDate, "")

                    .FormatControl(txtLastInstalmentDate, "")

                Else

                    .FormatControl(txtFirstInstalmentDate, m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanFirstInstalmentdate, 0))
                    ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanFirstInstalmentdate, txtFirstInstalmentDate)

                    .FormatControl(txtNextInstalmentDate, m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanNextInstalmentdate, 0))
                    ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanNextInstalmentdate, txtNextInstalmentDate)

                    .FormatControl(txtLastInstalmentDate, m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanLastInstalmentdate, 0))
                    ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanLastInstalmentdate, txtLastInstalmentDate)

                End If
                'MKW24062004 PN12422 Don't populate date fields for 3rd Party (as providers set). END

                'TR - 24/03/03 - TS17 Recovery By Instalments changes
                .FormatControl(txtOriginalDebt, m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanOriginalAmount, 0))
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanOriginalAmount, txtOriginalDebt)
            End With

            dtpCreatedDate.Value = DateTime.Today
            dtpModifiedDate.Value = DateTime.Today
            If Information.IsDate(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDateCreated, 0)) Then
                dtpCreatedDate.Value = CDate(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDateCreated, 0))
            Else

                'TODO:
                'dtpCreatedDate.Value = Nothing
            End If
            ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanDateCreated, dtpCreatedDate)

            If Information.IsDate(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDateModified, 0)) Then
                dtpModifiedDate.Value = CDate(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDateModified, 0))
            Else

                'TODO:
                'dtpModifiedDate.Value = Nothing
            End If
            ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanDateModified, dtpModifiedDate)

            SetCombo(cboStatus, CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStatusInd, 0)).Trim())
            ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanStatusInd, cboStatus)

            ' Client Tab
            txtClientName.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientName, 0)).Trim()
            ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanClientName, txtClientName)

            'TR - Todo - apply VersionHighlighting to the Address control??
            addClient.AddressLine1 = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientAddress1, 0)).Trim()

            addClient.AddressLine2 = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientAddress2, 0)).Trim()

            addClient.AddressLine3 = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientAddress3, 0)).Trim()

            addClient.AddressLine4 = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientAddress4, 0)).Trim()

            addClient.PostCode = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientPostcode, 0)).Trim()

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientCountry_ID, 0)) <> "" Then
                addClient.CountryId = CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientCountry_ID, 0))
                'PN14258
            Else
                addClient.CountryId = g_oObjectManager.CountryID
            End If

            txtClientAreaCode.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientAreaCode, 0)).Trim()
            ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanClientAreaCode, txtClientAreaCode)

            txtClientNumber.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientPhone, 0)).Trim()
            ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanClientPhone, txtClientNumber)

            txtClientExtension.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientExtn, 0)).Trim()
            ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanClientExtn, txtClientExtension)

            txtClientFaxCode.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientFaxCode, 0)).Trim()
            ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanClientFaxCode, txtClientFaxCode)

            txtClientFaxNumber.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientFax, 0)).Trim()
            ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanClientFax, txtClientFaxNumber)

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20030404 : If we have an existing bank account details / Credit Card
            '               details, then populate those values to associated controls
            '               and disable them so that the user can't change them
            '               Ref. Issue 2915 Changes
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' Do we need to use existing bank account details ??
            If m_bUseExistingBankDetails Then

                txtBankName.Text = CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankName, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanClientBankName, txtBankName)

                'Party Bank Details
                Dim auxVar As Object = m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankPaymentTypeId, 0)

                If CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankPaymentTypeId, 0)) = "" Or Convert.IsDBNull(auxVar) Or IsNothing(auxVar) Then
                    uctPartyBankCombo1.SelectedPaymentID = 0
                Else

                    uctPartyBankCombo1.SelectedPaymentID = CInt(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankPaymentTypeId, 0))
                    'Call ApplyVersionHighlighting(k_PFPlanClientBankPaymentTypeId, uctPartyBankCombo1.SelectedPaymentID)
                End If

                txtSortCode.Text = CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankSortCode, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanClientBankSortCode, txtSortCode)

                txtAccountNumber.Text = CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankAccountNo, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanClientBankAccountNo, txtAccountNumber)

                txtAccountName.Text = CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankAccountName, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanClientBankAccountName, txtAccountName)

                txtBranch.Text = CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankBranch, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanClientBankBranch, txtBranch)

                'TR - TODO

                addBank.AddressLine1 = CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankAddress1, 0)).Trim()

                addBank.AddressLine2 = CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankAddress2, 0)).Trim()

                addBank.AddressLine3 = CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankAddress3, 0)).Trim()

                addBank.AddressLine4 = CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankAddress4, 0)).Trim()

                addBank.PostCode = CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankPostCode, 0)).Trim()

                If CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankCountry_ID, 0)) <> "" Then

                    addBank.CountryId = CInt(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankCountry_ID, 0))
                    'PN14258
                Else
                    If m_iSchemeCountryID = 0 Then
                        addBank.CountryId = g_oObjectManager.CountryID
                    Else
                        addBank.CountryId = m_iSchemeCountryID
                    End If
                End If

                txtAreaCode.Text = CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankAreaCode, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanClientBankAreaCode, txtAreaCode)

                txtNumber.Text = CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankPhone, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanClientBankPhone, txtNumber)

                txtExtension.Text = CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankExtn, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanClientBankExtn, txtExtension)

                txtFaxAreaCode.Text = CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankFaxCode, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanClientBankFaxCode, txtFaxAreaCode)

                txtFaxNumber.Text = CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankFax, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanClientBankFax, txtFaxNumber)

                ' We need to disable them
                m_lReturn = EnableBankRelatedFields(False)

            Else
                txtBankName.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankName, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanBankName, txtBankName)

                'Party Bank Details

                If Convert.IsDBNull(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPartyBankIdSel, 0)) OrElse CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPartyBankIdSel, 0)) = "" OrElse IsNothing(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPartyBankIdSel, 0)) Then
                    uctPartyBankCombo1.SelectedPaymentID = 0
                    uctPartyBankCombo2.SelectedPaymentID = 0
                Else
                    uctPartyBankCombo1.SelectedPaymentID = CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPartyBankIdSel, 0))
                    uctPartyBankCombo2.SelectedPaymentID = CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPartyBankIdSel, 0))
                    'Call ApplyVersionHighlighting(k_PFPlanBankPaymentTypeId, uctPartyBankCombo1.SelectedPaymentID)
                End If

                txtSortCode.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankSortCode, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanBankSortCode, txtSortCode)

                txtAccountNumber.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankAccountNo, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanBankAccountNo, txtAccountNumber)

                txtBIC.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.kBIC, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.kBIC, txtBIC)

                txtIBAN.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.kIBAN, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.kIBAN, txtIBAN)

                txtAccountName.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankAccountName, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanBankAccountName, txtAccountName)

                txtBranch.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankBranch, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanBankBranch, txtBranch)
                'TR - TODO
                addBank.AddressLine1 = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankAddress1, 0)).Trim()

                addBank.AddressLine2 = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankAddress2, 0)).Trim()

                addBank.AddressLine3 = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankAddress3, 0)).Trim()

                addBank.AddressLine4 = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankAddress4, 0)).Trim()

                addBank.PostCode = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankPostcode, 0)).Trim()

                If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankCountry_ID, 0)) <> "" Then
                    addBank.CountryId = CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankCountry_ID, 0))
                Else
                    If m_iSchemeCountryID = 0 Then
                        addBank.CountryId = g_oObjectManager.CountryID
                    Else
                        addBank.CountryId = m_iSchemeCountryID
                    End If
                End If

                txtAreaCode.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankAreaCode, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanBankAreaCode, txtAreaCode)

                txtNumber.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankPhone, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanBankPhone, txtNumber)

                txtExtension.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankExtn, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanBankExtn, txtExtension)

                txtFaxAreaCode.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankFaxCode, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanBankFaxCode, txtFaxAreaCode)

                txtFaxNumber.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankFax, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanBankFax, txtFaxNumber)

            End If

            ' Do we need to use existing Credit Card details ??
            If m_bUseExistingCreditCardDetails Then

                m_sCreditCardNo = CStr(m_vCreditCardDetails(bSIRPremFinConst.k_PFPlanClientcc_number, 0)).Trim()

                ' Note : We can't display all the digits of the credit card !!!
                '          so displaying only the last four digits

                'Start (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)
                ' Removed codes related to m_bIsCCAuthorisationOff -(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)
                txtCardNo.Text = "**** **** **** " & m_sCreditCardNo.Substring(m_sCreditCardNo.Length - 4)

                txtTrackingNumber.Text = m_sCreditCardNo
                'End (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)

                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanClientcc_number, txtCardNo)

                txtCardName.Text = CStr(m_vCreditCardDetails(bSIRPremFinConst.k_PFPlanClientCreditCardName, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanClientCreditCardName, txtCardName)

                txtExpiryDate.Text = CStr(m_vCreditCardDetails(bSIRPremFinConst.k_PFPlanClientcc_expiry_date, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanClientcc_expiry_date, txtExpiryDate)

                txtCardStartDate.Text = CStr(m_vCreditCardDetails(bSIRPremFinConst.k_PFPlanClientcc_start_date, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanClientcc_start_date, txtCardStartDate)

                txtIssueNo.Text = CStr(m_vCreditCardDetails(bSIRPremFinConst.k_PFPlanClientcc_issue, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanClientcc_issue, txtIssueNo)

                txtPin.Text = CStr(m_vCreditCardDetails(bSIRPremFinConst.k_PFPlanClientcc_pin, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanClientcc_pin, txtPin)

                txtCardName.Text = CStr(m_vCreditCardDetails(bSIRPremFinConst.k_PFPlanClientCreditCardName, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanClientCreditCardName, txtCardName)

                m_lReturn = EnableCreditCardRelatedFields(False)
            Else
                ' Credit Card Tab
                'PSL 17/02/2003 2647
                If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCNumber, 0)).Trim() <> "" Then
                    txtCardNo.Text = "**** **** **** " &
                                     CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCNumber, 0)).Trim().Substring(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCNumber, 0)).Trim().Length - 4)

                    'Start (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)
                    txtTrackingNumber.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCNumber, 0)).Trim()
                    'End (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)
                    ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanCCNumber, txtCardNo)
                End If
                m_sCreditCardNo = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCNumber, 0)).Trim()
                'If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                '    txtCardName.Text = Trim(m_vFinancePlanArray(k_PfPlanCardholderName, 0))
                'Else
                '    txtCardName.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankName, 0)).Trim()
                '    ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanBankName, txtCardName)
                'End If
                txtExpiryDate.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCExpiryDate, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanCCExpiryDate, txtExpiryDate)

                chkCardholder.CheckState = gPMFunctions.ToSafeInteger(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanIsCardholder, 0)).Trim())
                txtCardholderName.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanCardholderName, 0)).Trim()
                txtCardStartDate.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCStartDate, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanCCStartDate, txtCardStartDate)

                txtIssueNo.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCIssue, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanCCIssue, txtIssueNo)

                txtPin.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCPin, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanCCPin, txtPin)
                txtCardholderName.Text = Trim(m_vFinancePlanArray(k_PfPlanCardholderName, 0))
                txtCardName.Text = Trim(m_vFinancePlanArray(k_PFPlanBankAccountName, 0))

                If Not m_iTask = gPMConstants.PMEComponentAction.PMView Then
                    ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanBankAccountName, txtCardName)
                End If
                cboCardType.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCardType, 0)).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanCardType, cboCardType)

                If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCardType, 0)).Trim() <> "" Then
                    For iCounter As Integer = 0 To cboCardType.Items.Count - 1
                        If VB6.GetItemString(cboCardType, iCounter) = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCardType, 0)).Trim() Then
                            cboCardType.SelectedIndex = iCounter
                            ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanCardType, txtCardName)
                            Exit For
                        End If
                    Next
                End If

            End If

            'DC220206 PN26057 show date bank details changed for use in adjusting payment dates
            If IsDate(m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanDateBankDetailsChanged, 0)) Then
                txtDateBankDetailsChanged.Text = gPMFunctions.ToSafeString(CDate(m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanDateBankDetailsChanged, 0))).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PfPlanDateBankDetailsChanged, txtDateBankDetailsChanged)

                txtDateBankDetailsChanged.Text = gPMFunctions.ToSafeString(CDate(m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanDateBankDetailsChanged, 0))).Trim()
                ApplyVersionHighlighting(bSIRPremFinConst.k_PfPlanDateBankDetailsChanged, txtDateBankDetailsChanged)
            End If
            lblHeader.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanSchemeName, 0)).Trim() & Strings.Chr(13) & Strings.Chr(10) & CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanFrequency, 0)).Trim() & ", " &
                             CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPaymentMethod, 0)).Trim()

            'Display/Hide Policy List tab.
            If bIsSinglePlanParty Then
                SSTabHelper.SetTabVisible(tabMainTab, 6, True)
            Else
                If tabMainTab.TabCount >= 6 Then
                    SSTabHelper.SetTabVisible(tabMainTab, 6, False)
                End If
            End If

            'TR - Set the 3rd Party Recovery Controls
            ShowHideThirdPartyRecoveryControls()

            ' Display the instalments
            'TODO_:
            m_lReturn = DisplayInstalments()

            'Direct Debit Cancelled

            If Conversion.Val(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDDCancelled, 0)) = 1 Then
                chkDDCancelled.CheckState = CheckState.Checked
            Else
                chkDDCancelled.CheckState = CheckState.Unchecked
            End If

            ' Credit Card Cancelled

            If Conversion.Val(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCCancelled, 0)) = 1 Then
                chkCCCancelled.CheckState = CheckState.Checked
            Else
                chkCCCancelled.CheckState = CheckState.Unchecked
            End If

            ' Paper DD

            If Conversion.Val(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPaperDD, 0)) = 1 Then
                chkPaperDD.CheckState = CheckState.Checked
            Else
                chkPaperDD.CheckState = CheckState.Unchecked
            End If
            If Conversion.Val(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDDCancelled, 0)) = 1 Then
                chkDDCancelled.CheckState = CheckState.Checked
            Else
                chkDDCancelled.CheckState = CheckState.Unchecked
            End If

            ' Credit Card Cancelled
            If Conversion.Val(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCCancelled, 0)) = 1 Then
                chkCCCancelled.CheckState = CheckState.Checked
            Else
                chkCCCancelled.CheckState = CheckState.Unchecked
            End If

            ' Paper DD
            If Conversion.Val(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPaperDD, 0)) = 1 Then
                chkPaperDD.CheckState = CheckState.Checked
            Else
                chkPaperDD.CheckState = CheckState.Unchecked
            End If

            ' Set the command buttons
            m_lReturn = CommandSet()

            If (CDbl(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanImmediateBankDetails, 0)) = 1 And CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanParentPlanCnt, 0)) = "") Or CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanMediaTypeValidation, 0)).Trim() = "BANK" Then
                If m_lSchemeTypeID = PFThirdParty Then
                    m_oFormFields.Item("txtAccountNumber-0").IsMandatory = False
                    m_oFormFields.Item("txtAccountName-0").IsMandatory = False
                Else
                    m_oFormFields.Item("txtAccountNumber-0").IsMandatory = True
                    m_oFormFields.Item("txtAccountName-0").IsMandatory = True
                    lblAccountNumber.Font = VB6.FontChangeBold(lblAccountNumber.Font, True)
                    lblAccountName.Font = VB6.FontChangeBold(lblAccountName.Font, True)
                End If
                'SMJB CQ2193 10/09/03 Ensure CC fields are now non mandatory
                m_oFormFields.Item("txtCardNo-0").IsMandatory = False
                m_oFormFields.Item("txtExpiryDate-0").IsMandatory = False

            End If

            'Check the scheme for mandatory bank details
            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanMediaTypeValidation, 0)).Trim() = "BANK" Then
                If CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankNameMandatory, 0)) = 1 Then
                    m_oFormFields.Item("txtBankName-0").IsMandatory = True
                    lblBankName.Font = VB6.FontChangeBold(lblBankName.Font, True)
                End If
                If CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankAddressMandatory, 0)) = 1 Then
                    m_BankMandatory = True
                    'SJ 20/08/2004 - start
                    addBank.CaptionFontBoldAddress1 = True
                    addBank.CaptionFontBoldPostCode = True
                Else
                    addBank.CaptionFontBoldAddress1 = False
                    addBank.CaptionFontBoldPostCode = False
                End If
                'SJ 20/08/2004 - end

                If CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBranchNameMandatory, 0)) = 1 Then
                    m_oFormFields.Item("txtBranch-0").IsMandatory = True
                    lblBranch.Font = VB6.FontChangeBold(lblBranch.Font, True)
                End If

                If m_sProviderCode = "CLOSEIP" Then
                    m_oFormFields.Item("txtSortCode-0").IsMandatory = True
                    lblSortCode.Font = VB6.FontChangeBold(lblSortCode.Font, True)
                Else
                    If CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBranchCodeMandatory, 0)) = 1 Then
                        m_oFormFields.Item("txtSortCode-0").IsMandatory = True
                        lblSortCode.Font = VB6.FontChangeBold(lblSortCode.Font, True)
                    End If
                End If
            End If
            'Thinh Nguyen 18/03/2002 (end) - don't need to get bank details if immediate bank details is not set

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanParentPlanCnt, 0)) <> "" Then
                If tabMainTab.TabCount >= 2 Then
                    SSTabHelper.SetTabVisible(tabMainTab, 2, False)
                End If
                If tabMainTab.TabCount >= 3 Then
                    SSTabHelper.SetTabVisible(tabMainTab, 3, False)
                End If
            Else
                'DD 14/08/2003: Reset the visibility
                If tabMainTab.TabCount >= 2 Then
                    SSTabHelper.SetTabVisible(tabMainTab, 2, True)
                End If
                If tabMainTab.TabCount >= 3 Then
                    SSTabHelper.SetTabVisible(tabMainTab, 3, True)
                End If
            End If

            'DD 17/11/2003: Added and revised for Premium Finance
            If m_lSchemeTypeID = bSIRPremFinConst.PFThirdParty Or m_lSchemeTypeID = bSIRPremFinConst.PFThirdPartyViaStargate Then
                txtAgentRef.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAgentRef, 0)).Trim()
                txtPFReference.Text = txtAgentRef.Text
                txtPFReference.Visible = True
                lblPFReference.Visible = True
                'PN12594
                lblBusinessCode.Visible = True
                cboBusinessCode.Visible = True
                m_sBusinessCode = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBusinessCode, 0)).Trim()
                For lRow As Integer = 0 To m_vLookupDetails.GetUpperBound(1)
                    If CStr(m_vLookupDetails(2, lRow)).Trim() = m_sBusinessCode Then
                        ' CJB 100904 Bugfix to prevent item being positioned to incorrect text
                        SetCombo(cboBusinessCode, m_vLookupDetails(0, lRow))
                        m_lBusinessCode_id = CInt(m_vLookupDetails(0, lRow))
                        Exit For
                    End If
                Next

            End If

            If m_lSchemeTypeID = bSIRPremFinConst.PFThirdParty Then
                'PN12594End

                'Get additional scheme information
                'PN12594 return the business code
                'AR20061106 - PN31399 Business Object changed to return array rather than individual fields

                If m_oPFScheme.GetDetails(lCompanyNo:=m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCompanyNo, 0), lSchemeNo:=m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanSchemeNo, 0), lSchemeVersion:=m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanSchemeVersion, 0), r_vSchemeArray:=vSchemeArray) <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMError

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the Scheme object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result

                End If

                If Information.IsArray(vSchemeArray) Then

                    'AR20061106 - PN31399 Assign array items to variables
                    vDepositOnOtherMediaType = gPMFunctions.NullToString(vSchemeArray(bSIRPremFinConst.k_PFSchemeDepositOnOtherMediaType, 0))

                    vAgentRefMandatory = vSchemeArray(bSIRPremFinConst.k_PFSchemeAgentRefMandatory, 0)

                    vPFMessage = gPMFunctions.NullToString(vSchemeArray(bSIRPremFinConst.k_PFSchemePFMessage, 0))

                    vBusinessCodeMandatory = vSchemeArray(bSIRPremFinConst.k_PFSchemeBusinessCodeMandatory, 0)

                    If gPMFunctions.NullToString(vAgentRefMandatory) = "1" Then
                        m_oFormFields.Item("txtPFReference-0").IsMandatory = True
                        lblPFReference.Font = VB6.FontChangeBold(lblPFReference.Font, True)
                    End If

                    If CStr(vPFMessage) <> "" And CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStatusInd, 0)) < bSIRPremFinConst.PFStatusIndLive And CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStatusInd, 0)) <> bSIRPremFinConst.PFStatusIndUpdated Then
                        'Pop up the message box for the user if we are pre-live
                        MsgBoxSirius(vPFMessage, MsgBoxStyle.Information, "Finance Provider Message")
                    End If
                    'PN12594
                    If gPMFunctions.NullToString(vBusinessCodeMandatory) = "1" Then
                        m_oFormFields.Item("cboBusinessCode-0").IsMandatory = True
                        lblBusinessCode.Font = VB6.FontChangeBold(lblBusinessCode.Font, True)
                    End If

                Else

                    result = gPMConstants.PMEReturnCode.PMError

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the Scheme object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result

                End If

            End If

            If m_sProviderCode = "PCLSG" Then
                cboBusinessCode.Visible = False
                lblBusinessCode.Visible = False
            End If
            txtAuthCode.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAuthCode, 0)).Trim()
            txtAuthCode.Visible = Not (CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanProductClass, 0)).Trim() = "SG")
            lblAuthCode.Visible = Not (CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanProductClass, 0)).Trim() = "SG")

            'Only show the bank or credit card tab if the mediatype is appropiate
            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanMediaTypeValidation, 0)).Trim() <> "BANK" Then
                If tabMainTab.TabCount >= 2 Then
                    SSTabHelper.SetTabVisible(tabMainTab, 2, False)
                End If
            Else
                'Populate the list box with an available bank details for this client
                BankAccountDetailsList()
                If (m_sTransactionType <> "NB" And m_sTransactionType <> "MTR" And m_sTransactionType <> "G_NB" And m_sTransactionType <> "MTA") Then
                    PopulateBankDetails()
                End If
            End If

            If m_sProviderCode = "AMBER" And CDbl(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDeposit, 0)) > 0 Then
                SSTabHelper.SetTabVisible(tabMainTab, 3, True)
                lblCardNo.Font = VB6.FontChangeBold(lblCardNo.Font, True)
                lblExpiryDate.Font = VB6.FontChangeBold(lblExpiryDate.Font, True)
                lblCardName.Font = VB6.FontChangeBold(lblCardName.Font, True)
                lblCardStartDate.Font = VB6.FontChangeBold(lblCardStartDate.Font, True)
                lblCardType.Font = VB6.FontChangeBold(lblCardType.Font, True)

                m_oFormFields.Item("txtCardNo-0").IsMandatory = True
                m_oFormFields.Item("txtExpiryDate-0").IsMandatory = True
                lblCardNo.Font = VB6.FontChangeBold(lblCardNo.Font, True)
                lblExpiryDate.Font = VB6.FontChangeBold(lblExpiryDate.Font, True)

                'Populate the list box with an available credit card details for this client
                CreditCardDetailsList()
            ElseIf CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanMediaTypeValidation, 0)).Trim() <> "CC" And vDepositOnOtherMediaType <> "1" Then
                If m_sProviderCode = "CLOSEIP" Then
                    SSTabHelper.SetTabVisible(tabMainTab, 3, True)
                Else
                    If tabMainTab.TabCount >= 3 Then
                        SSTabHelper.SetTabVisible(tabMainTab, 3, False)
                    End If
                End If
                lblCardNo.Font = VB6.FontChangeBold(lblCardNo.Font, m_sProviderCode = "CLOSEIP")
                lblExpiryDate.Font = VB6.FontChangeBold(lblExpiryDate.Font, m_sProviderCode = "CLOSEIP")
                lblCardName.Font = VB6.FontChangeBold(lblCardName.Font, m_sProviderCode = "CLOSEIP")
                lblCardStartDate.Font = VB6.FontChangeBold(lblCardStartDate.Font, m_sProviderCode = "CLOSEIP")
                lblCardType.Font = VB6.FontChangeBold(lblCardType.Font, m_sProviderCode = "CLOSEIP")
            Else

                ' if the main payment method is not by credit card hide the cancel credit card agreement
                ' indicator
                If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanMediaTypeValidation, 0)).Trim() <> "CC" Then
                    chkCCCancelled.Visible = False
                End If

                m_oFormFields.Item("txtCardNo-0").IsMandatory = True
                m_oFormFields.Item("txtExpiryDate-0").IsMandatory = True
                lblCardNo.Font = VB6.FontChangeBold(lblCardNo.Font, True)
                lblExpiryDate.Font = VB6.FontChangeBold(lblExpiryDate.Font, True)

                If vDepositOnOtherMediaType <> "1" Then
                    'SMJB CQ2193 10/09/03 Ensure Bank fields are now non mandatory
                    m_oFormFields.Item("txtAccountNumber-0").IsMandatory = False
                    m_oFormFields.Item("txtAccountName-0").IsMandatory = False
                    m_oFormFields.Item("txtBankName-0").IsMandatory = False
                    m_oFormFields.Item("txtSortCode-0").IsMandatory = False
                    m_oFormFields.Item("txtBranch-0").IsMandatory = False
                    m_oFormFields.Item("txtBIC-0").IsMandatory = False
                    m_oFormFields.Item("txtIBAN-0").IsMandatory = False
                End If

                'Populate the list box with an available credit card details for this client
                CreditCardDetailsList()
            End If

            'DD 31/07/2003: Added for Premium Finance
            'PN13636
            'If m_bRequireDateOfBirth Or m_bRequireCompanyReg Then
            If m_bRequireDateOfBirth Then

                If m_oParty.PartyType = "PC" Then
                    lblDateOfBirth.Visible = True
                    txtDateOfBirth.Visible = True
                    m_oFormFields.Item("txtDateOfBirth-0").IsMandatory = True
                    txtDateOfBirth.Text = m_vDateOfBirth
                End If
            End If
            If m_bRequireCompanyReg Then

                If m_oParty.PartyType = "CC" Then
                    lblCompanyReg.Visible = True
                    If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanProductClass, 0)).Trim() = "SG" Then
                        txtCompanyReg.Visible = chkPartnership.CheckState
                        lblCompanyReg.Visible = txtCompanyReg.Visible
                        m_oFormFields.Item("txtCompanyReg-0").IsMandatory = Not (chkPartnership.CheckState)
                    Else
                        txtCompanyReg.Visible = True
                        lblCompanyReg.Visible = txtCompanyReg.Visible
                        m_oFormFields.Item("txtCompanyReg-0").IsMandatory = True
                    End If

                    m_oFormFields.FormatControl(txtCompanyReg, m_sCompanyReg)
                    m_bRequireSGCompanyDetails = True
                Else
                    chkPartnership.Visible = False
                    txtCompanyReg.Visible = False
                    addClient.Width = VB6.TwipsToPixelsX(8625)
                    lblCompanyReg.Visible = False
                    m_oFormFields.Item("txtCompanyReg-0").IsMandatory = False
                    m_bRequireCompanyReg = False
                    frmPartners.Visible = False
                    m_bRequireSGCompanyDetails = False
                End If
            End If
            'End If
            'PN13636End

            'DD 17/12/2003 - Added so that user can override Sirius default
            txtReference.Text = gPMFunctions.NullToString(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAutoGenPlanRef, 0)).Trim()

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanProductClass, 0)).Trim() = "SG" Then
                'ACR 25-04-05
                Try
                    lUpper = -1
                    lUpper = m_vPartnerArray.GetUpperBound(1)
                Catch ex As Exception

                End Try

                If lUpper >= 0 Then
                    For x As Integer = 0 To lUpper
                        bFound = False

                        For y As Integer = 1 To lvwPartners.Items.Count
                            If lvwPartners.Items.Item(y - 1).Text = m_vPartnerArray(0, x) Then
                                bFound = True
                            End If
                        Next y

                        If Not (bFound) Then
                            With lvwPartners.Items.Add(m_vPartnerArray(0, x).Replace(" ", ""), m_vPartnerArray(0, x), "")
                                ListViewHelper.GetListViewSubItem(lvwPartners.Items.Add(m_vPartnerArray(0, x).Replace(" ", ""), m_vPartnerArray(0, x), ""), 1).Text = m_vPartnerArray(1, x)
                                ListViewHelper.GetListViewSubItem(lvwPartners.Items.Add(m_vPartnerArray(0, x).Replace(" ", ""), m_vPartnerArray(0, x), ""), 2).Text = m_vPartnerArray(5, x)
                            End With
                        End If
                    Next x
                End If

                If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanLimitedCompany, 0)) <> "" Then
                    chkPartnership.CheckState = CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanLimitedCompany, 0))
                End If

                chkPartnership_CheckStateChanged(chkPartnership, New EventArgs())

                If m_sProviderCode = "CLOSEIP" Or m_sProviderCode = "AMBER" Then
                    lblBusinessCode.Font = VB6.FontChangeBold(lblBusinessCode.Font, True)
                    'check if we have a deposit..
                    If CDbl(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDeposit, 0)) > 0 Then
                        'populate the data
                        If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanIsCardholder, 0)) <> "" Then
                            chkCardholder.CheckState = CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanIsCardholder, 0))
                            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                                txtCardholderName.Text = ""
                            Else
                                txtCardholderName.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanCardholderName, 0))
                            End If
                            addCardholder.AddressLine1 = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanCardholderAddress1, 0))
                            addCardholder.AddressLine2 = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanCardholderAddress2, 0))
                            addCardholder.AddressLine3 = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanCardholderAddress3, 0))
                            addCardholder.AddressLine4 = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanCardholderAddress4, 0))
                            addCardholder.PostCode = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanCardholderPostcode, 0))
                            addCardholder.CountryId = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanCardholderCountryID, 0))
                        Else
                            addCardholder.CountryId = m_iSchemeCountryID

                        End If
                        'END ACR
                    End If
                End If
                'END ACR

                If m_oParty.PartyType <> "CC" Then
                    chkPartnership.Visible = False
                    txtCompanyReg.Visible = False
                    addClient.Width = VB6.TwipsToPixelsX(8625)
                    lblCompanyReg.Visible = False
                    m_oFormFields.Item("txtCompanyReg-0").IsMandatory = False
                    m_bRequireCompanyReg = False
                    frmPartners.Visible = False

                    m_bRequireSGCompanyDetails = False
                End If

                'ACR 05-06-07 hide credit card tab after save if not required..
                If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanProviderCollectDeposit, 0)) = "1" Then
                    If m_sProviderCode = "CLOSEIP" Then
                        SSTabHelper.SetTabVisible(tabMainTab, 3, True)
                    Else
                        If tabMainTab.TabCount >= 3 Then
                            SSTabHelper.SetTabVisible(tabMainTab, 3, False)
                        End If
                    End If
                Else
                    If tabMainTab.TabCount >= 3 Then
                        SSTabHelper.SetTabVisible(tabMainTab, 3, False)
                    End If
                End If
            Else
                m_bRequireRefundType = False
                cboRefundType.Visible = False
                lblRefundType.Visible = False
                chkPartnership.Visible = False
                txtCompanyReg.Visible = False
                addClient.Width = VB6.TwipsToPixelsX(8625)
                lblCompanyReg.Visible = False
                m_oFormFields.Item("txtCompanyReg-0").IsMandatory = False
                m_bRequireCompanyReg = False
                frmPartners.Visible = False

                m_bRequireSGCompanyDetails = False
                m_bMandatoryCCDetails = False

                If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanIsCardholder, 0)) <> "" Then
                    chkCardholder.CheckState = CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanIsCardholder, 0))
                    'If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                    '    txtCardholderName.Text = ""
                    'Else
                    txtCardholderName.Text = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanCardholderName, 0))
                    'End If
                    addCardholder.AddressLine1 = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanCardholderAddress1, 0))
                    addCardholder.AddressLine2 = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanCardholderAddress2, 0))
                    addCardholder.AddressLine3 = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanCardholderAddress3, 0))
                    addCardholder.AddressLine4 = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanCardholderAddress4, 0))
                    addCardholder.PostCode = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanCardholderPostcode, 0))
                    addCardholder.CountryId = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanCardholderCountryID, 0))
                Else
                    addCardholder.CountryId = m_iSchemeCountryID

                End If

            End If

            ' If user does not have Edit Finance Plan Authority then disable all apart from
            ' command buttons PN24604
            If Not g_bEditFinancePlanAuthority Then
                txtReference.Enabled = False
                m_lReturn = EnableBankRelatedFields(False)
                m_lReturn = EnableCreditCardRelatedFields(False)
                m_lReturn = EnableClientInformationRelatedFields(False)
                m_lReturn = DisableButtons()
            End If
            ' PN74526
            ' if is_planref_editable of pfscheme is true then editable = true else false
            If m_oPFScheme Is Nothing Then
                m_lReturn = g_oObjectManager.GetInstance(
                    oObject:=m_oPFScheme,
                    sClassName:="bSIRPFScheme.Business",
                    vInstanceManager:=PMGetViaClientManager)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError("BusinessToInterface", "GetInstance of bSIRPFScheme.Business Failed.")
                    Exit Function
                End If
            End If
            m_lReturn = m_oPFScheme.GetDetails(
                    lCompanyNo:=m_vFinancePlanArray(k_PFPlanCompanyNo, 0),
                    lSchemeNo:=m_vFinancePlanArray(k_PFPlanSchemeNo, 0),
                    lSchemeVersion:=m_vFinancePlanArray(k_PFPlanSchemeVersion, 0),
                    r_vSchemeArray:=vSchemeArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("BusinessToInterface", "GetDetails Failed.")
                Exit Function
            End If
            If IsArray(vSchemeArray) Then
                iIsPlanReferenceEditable = gPMFunctions.NullToString(vSchemeArray(bSIRPremFinConst.k_PFSchemePlanRefEditable, 0))
            End If
            If iIsPlanReferenceEditable = 1 And m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                txtReference.Enabled = True
            Else
                txtReference.Enabled = False
            End If

            framAdditional.Visible = False

            If SSTabHelper.GetTabVisible(tabMainTab, kMainTabBankDetails) Then
                SetupBankMediaTypeHistoryListView()
                PopulateBankMediaTypeHistoryListView()
            End If

            If SSTabHelper.GetTabVisible(tabMainTab, kMainTabCreditCardDetails) Then
                SetupCreditCardMediaTypeHistoryListView()
                PopulateCCMediaTypeHistoryListView()
            End If

            If bIsSinglePlanParty Then
                ' Display the PolicyList
                m_lReturn = DisplayPolicyList()
            End If

            If Not Object.Equals(m_vFinancePlanArray(bSIRPremFinConst.k_PFCancelReasonId, 0), Nothing) Then
                cboCancelReason.ItemId = CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFCancelReasonId, 0))
            End If
            FillPartyBankDetails()
            Return result

        Catch ex As Exception
            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", excep:=ex)
        End Try
        Return result

    End Function

    ' ***************************************************************** '
    ' Name          : EnableClientInformationRelatedFields
    ' Description   : Private Function to enable/disable the fields on the
    '                 Client Information Tab...created as part of PN24604.
    '                 Will initially only be called when EditFinancePlanAuth
    '                 has NOT been granted to the user (and this will only be
    '                 False if coming to this function via Client Manager and
    '                 Client Manager security has been switched on in Sys Options.
    ' Parameters    : v_bTrueOrFalse : Boolean, This will determine the state of
    '                 the controls
    ' Author        : Chris Barnes
    ' Edit Histroy  :
    ' CJB20051005   : Created
    ' ***************************************************************** '
    Private Function EnableClientInformationRelatedFields(ByVal v_bTrueOrFalse As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            txtClientName.Enabled = v_bTrueOrFalse
            chkPartnership.Enabled = v_bTrueOrFalse
            addClient.Enabled = v_bTrueOrFalse
            lvwPartners.Enabled = v_bTrueOrFalse
            cmdAddPartner.Enabled = v_bTrueOrFalse
            cmdDeletePartner.Enabled = v_bTrueOrFalse
            txtClientAreaCode.Enabled = v_bTrueOrFalse
            txtClientNumber.Enabled = v_bTrueOrFalse
            txtClientExtension.Enabled = v_bTrueOrFalse
            txtClientFaxCode.Enabled = v_bTrueOrFalse
            txtClientFaxNumber.Enabled = v_bTrueOrFalse
            txtAuthCode.Enabled = v_bTrueOrFalse
            txtPFReference.Enabled = v_bTrueOrFalse
            cboBusinessCode.Enabled = v_bTrueOrFalse
            txtDateOfBirth.Enabled = v_bTrueOrFalse
            txtCompanyReg.Enabled = v_bTrueOrFalse
            cboRefundType.Enabled = v_bTrueOrFalse

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnableClientInformationRelatedFields Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnableClientInformationRelatedFields", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    '*************************************************************************
    'Name:          PopulateFrequencyCombo
    'Description:   Gets all the Frequencies and puts them in
    '               cboStatementFrequency
    'History:       TR25/03/03 - Created
    '*************************************************************************
    Public Sub PopulateFrequencyCombo()

        Try

            If Not (g_oBusiness Is Nothing) Then

                g_oBusiness.GetAllFrequencies(m_vPFFrequencies)
            End If
            If Information.IsArray(m_vPFFrequencies) Then
                cboStatementFrequency.Items.Clear()
                For lCount As Integer = m_vPFFrequencies.GetLowerBound(1) To m_vPFFrequencies.GetUpperBound(1)
                    cboStatementFrequency.Items.Add(CStr(m_vPFFrequencies(2, lCount)))
                    VB6.SetItemData(cboStatementFrequency, lCount, CInt(m_vPFFrequencies(0, lCount)))
                Next lCount
            End If

        Catch excep As System.Exception

            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "PopulateFrequencyCombo Failed", ACApp, ACClass, "PopulateFrequencyCombo", Information.Err().Number, excep.Message, excep:=excep)
            Exit Sub

        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: DisplayInstalments
    '
    ' Description: Updates all interface details from the business object.
    ' ***************************************************************** '
    Public Function DisplayInstalments() As Integer

        Dim result As Integer = 0
        Dim dtDueDate As Date
        Dim nLower, nUpper As Integer
        Dim oListItem As ListViewItem
        Dim lStatusInd As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check scheme is in house and further than simply saved
            If m_lSchemeTypeID <> bSIRPremFinConst.PFThirdParty And m_lSchemeTypeID <> bSIRPremFinConst.PFThirdPartyViaStargate And m_sStatusInd >= bSIRPremFinConst.PFStatusIndLive Then
                ' Yes we have instalments display them
                SSTabHelper.SetTabVisible(tabMainTab, 4, True)

                lvwInstalment.Items.Clear()

                If Information.IsArray(m_vFinancePlanInstalmentArray) Then
                    nLower = m_vFinancePlanInstalmentArray.GetLowerBound(1)
                    nUpper = m_vFinancePlanInstalmentArray.GetUpperBound(1)

                    If m_sCurrencyISOCode.Trim() <> "" Then
                        lvwInstalment.Columns.Item(3).Text = "Amount (" & m_sCurrencyISOCode & ")"
                    Else
                        lvwInstalment.Columns.Item(3).Text = "Amount"

                    End If

                    For nCount As Integer = nLower To nUpper

                        If Not Convert.IsDBNull(m_vFinancePlanInstalmentArray(bSIRPremFinConst.PFInstTransactionCode, nCount)) AndAlso CDbl(m_vFinancePlanInstalmentArray(bSIRPremFinConst.PFInstTransactionCode, nCount)) = PFTransactionDeposit Then
                            oListItem = lvwInstalment.Items.Add("Deposit")
                        Else
                            oListItem = lvwInstalment.Items.Add(CStr(m_vFinancePlanInstalmentArray(bSIRPremFinConst.PFInstInstalmentNumber, nCount)))
                        End If
                        'TR - Due Date
                        dtDueDate = Strings.Format(CDate(m_vFinancePlanInstalmentArray(bSIRPremFinConst.PFInstDueDate, nCount)), "Short Date")
                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = dtDueDate
                        'TR - BatchExportDate
                        'SW - No, this should be the posted date
                        If Information.IsDate(m_vFinancePlanInstalmentArray(bSIRPremFinConst.PFInstPostedDate, nCount)) Then
                            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = Strings.Format(CDate(m_vFinancePlanInstalmentArray(bSIRPremFinConst.PFInstPostedDate, nCount)), "Short Date")
                        Else
                            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = (CStr(m_vFinancePlanInstalmentArray(bSIRPremFinConst.PFInstPostedDate, nCount)))
                        End If
                        'TR - Amount
                        ListViewHelper.GetListViewSubItem(oListItem, 3).Text = StringsHelper.Format(m_vFinancePlanInstalmentArray(bSIRPremFinConst.PFInstAmount, nCount), "#,##0.00")
                        'TR - Status
                        lStatusInd = CInt(m_vFinancePlanInstalmentArray(bSIRPremFinConst.PFInstStatus, nCount))
                        'TR - Set up a default status
                        ListViewHelper.GetListViewSubItem(oListItem, 4).Text = iSIRPremFinConst.GetInstalmentStatus(CStr(lStatusInd))
                        'TR - Bit of Logic for the Status Column
                        If m_lSchemeTypeID = bSIRPremFinConst.PFThirdPartyRecovery Then
                            If lStatusInd = bSIRPremFinConst.PFStatusNew Then
                                If dtDueDate < DateTime.Now Then
                                    'TR - Overwrite the Status for this scenario
                                    ListViewHelper.GetListViewSubItem(oListItem, 4).Text = "Missed"
                                End If
                            End If
                        End If
                        'If status is collected overwrite the description to collected
                        If lStatusInd = 3 Then
                            If CStr(m_vFinancePlanInstalmentArray(bSIRPremFinConst.kPFInstWriteOffReasonID, nCount)) = "0" Then
                                ListViewHelper.GetListViewSubItem(oListItem, 5).Text = "Collected"
                            Else
                                ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(m_vFinancePlanInstalmentArray(bSIRPremFinConst.kPFInstWriteOffReason, nCount))
                            End If
                        Else
                            ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(m_vFinancePlanInstalmentArray(bSIRPremFinConst.PFInstReason, nCount))
                        End If

                        ' Tag our index in as well
                        oListItem.Tag = CStr(nCount)
                    Next
                End If

                lvwInstalment.GridLines = True

                ListView6Func.ListViewAutoSize(lvwList:=lvwInstalment)
            Else
                ' No we don't, so hide them
                If tabMainTab.TabCount >= 4 Then
                    SSTabHelper.SetTabVisible(tabMainTab, 4, False)
                End If
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayInstalments", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '*************************************************************************
    ' Name:         ProcessMTA
    ' Description:  Determines if the MTA is an additional amount (asks user
    '               to select more transactions) or same amount with different
    '               Collection Media / Frquency Gets Additional Transactions
    '               and Processes required type
    ' History:      TR09122002 - TS23 - Changed from just handling additional
    '               amounts to handling changes in Instalments as well.
    '               DD 16/07/2003: Changed to support MTA on Premium Finance
    '*************************************************************************
    Private Function ProcessMTA() As Integer

        Dim result As Integer = 0
        Dim enMsgBoxResult As DialogResult
        Dim crTmpPrem As Decimal
        m_oOldTransArray = Nothing
        m_oOldPFArray = Nothing

        Try

            'TR - Assume success
            result = gPMConstants.PMEReturnCode.PMTrue

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAutoGenPlanRef, 0)).Trim() <> txtReference.Text Then
                cmdSave_Click(cmdSave, New EventArgs())
            End If

            If gPMFunctions.ToSafeLong(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanInsuranceFileCnt, 0), 0) <> 0 Then
                'Thinh Nguyen 01/02/2004 - get insurance folder count to limit transactions to this policy

                m_lReturn = g_oBusiness.GetInsuranceFolderCnt(v_lInsuranceFileCnt:=gPMFunctions.NullToLong(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanInsuranceFileCnt, 0)), r_lInsuranceFolderCnt:=m_lPlanInsuranceFolderCnt)
            End If

            If m_lSchemeTypeID = bSIRPremFinConst.PFThirdParty Then
                'DD 16/07/2003: Premium Finance does not quote here.
                'The user just selects the transactions and these are merged
                'on the existing plan.
                m_lReturn = SelectTransactions()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If
            Else
                'ACR 12-07-05 only display the message for non-sg schemes
                If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanProductClass, 0)).Trim() = "SG" Then
                    enMsgBoxResult = System.Windows.Forms.DialogResult.No
                Else
                    'TR - First ask the user if they want to JUST change Collection Media
                    'Type or Instalment Frequencies

                    enMsgBoxResult = MsgBoxSirius(iPMFunc.GetResData(g_iLanguageID, ACMTAChangeType, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager), MsgBoxStyle.YesNoCancel + MsgBoxStyle.Question, "Finance Plan MTA")
                End If

                'TR -See what the User decided

                Select Case enMsgBoxResult
                    'TR - User just wants to change collection type/method, not the premium
                    Case System.Windows.Forms.DialogResult.Yes
                        'TR - Set the MTA Type (3 = same amount, just different media
                        'type or frequency)
                        m_IsMediaTypeChanged = True
                        m_lMTAType = bSIRPremFinConst.m_klInstalmentMTAType_NoAmountChange
                        'TR - Show the Quotes screen for this type of MTA
                        m_lReturn = QuoteFinanceForTransactions()

                        'TR - User wants to select additional transactions to be collected
                        'with the existing ones. So show the Select Transactions screen.
                    Case System.Windows.Forms.DialogResult.No
                        'TR - Set the MTA type as 0. Any further choices will occur in
                        'the Instalments user control
                        m_lMTAType = bSIRPremFinConst.m_klInstalmentMTAType_AddAndSpread
                        'TR - Show the Transaction Find screen for these types of MTA
                        m_lReturn = SelectTransactions()
                        If m_lReturn = 0 Then
                            Return result
                        End If

                        If m_sProviderCode = "PCLSG" Then

                            'ACR 21-03-05 Just need to pop up a message to let the user know
                            'that no-one supports MTA's yet.. this code will be removed
                            'once the process is supported by credit providers
                            enMsgBoxResult = MsgBoxSirius("MTA requests will be submitted to the finance provider for processing. Do you wish to continue?", MsgBoxStyle.YesNo Or MsgBoxStyle.Information, "Connectivity MTA")

                            If enMsgBoxResult = System.Windows.Forms.DialogResult.Yes Then
                                'crTmpPrem = m_vFinancePlanArray(k_PFPlanAmountToFinance, 0)

                                'add up the mta transactions..

                                For x As Integer = m_vFinancePlanMTATransArray.GetLowerBound(1) To m_vFinancePlanMTATransArray.GetUpperBound(1)
                                    'add to the running total

                                    crTmpPrem += CDec(m_vFinancePlanMTATransArray(2, x))
                                Next x

                                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAmountToFinance, 0) = crTmpPrem

                                'set the mousepointer to an hour glass
                                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
                                'the user wants to continue - we now send an acceptmtarates message off to
                                'the credit provider who will note the users interest in an MTA and contact
                                'them accordingly..

                                m_lReturn = g_oBusiness.NotifyConnectivityMTA(m_vFinancePlanArray)

                                'check if we had any errors..

                                If g_oBusiness.ErrorCode <> "" Then

                                    MsgBoxSirius("There has been an error while connecting to the web service." &
                                                 Strings.Chr(13) & Strings.Chr(10) & g_oBusiness.ErrorText, MsgBoxStyle.Exclamation, "Error")

                                    Return result
                                End If

                                m_lReturn = gPMConstants.PMEReturnCode.PMTrue

                                'set the mouse pointer back
                                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                                m_lReturn = g_oBusiness.InsertNewVersion(m_vFinancePlanArray, CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFCnt, 0)), CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFVersion, 0)), m_vFinancePlanArray)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    Return result
                                End If
                            End If
                        Else
                            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                m_lReturn = QuoteFinanceForTransactions()
                            End If
                        End If

                        'TR - User has changed their mind
                    Case System.Windows.Forms.DialogResult.Cancel
                        Return result
                    Case Else
                        Return result
                End Select
            End If

            'TR  - Make sure it all worked OK and the user didn't cancel.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'TR - Now we have done the different processing for the types of MTA,
            'we can do the processing that they have in common, on the modular
            'TransArray which should now be populated

            m_oOldTransArray = m_vFinancePlanMTATransArray

            m_oOldPFArray = VB6.CopyArray(m_vFinancePlanArray)
            m_lReturn = GetBusiness()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                Throw New Exception()
            End If

            'ACR 14-07-05 copy over some of the details from the old array to the new one
            ' for stargate quotes

            m_sOldMediaTypeCode = CStr(m_oOldPFArray(bSIRPremFinConst.k_PFPlanMediaTypeValidation, 0))

            m_IsMediaTypeChanged = Not (CStr(m_oOldPFArray(bSIRPremFinConst.k_PFPlanMediaTypeCode, 0)).Trim() = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanMediaTypeCode, 0)).Trim())
            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanProductClass, 0)).Trim() = "SG" Then

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanInterestRate, 0) = m_oOldPFArray(bSIRPremFinConst.k_PFPlanInterestRate, 0)

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanTerms, 0) = m_oOldPFArray(bSIRPremFinConst.k_PFPlanTerms, 0)

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBusinessCode, 0) = m_oOldPFArray(bSIRPremFinConst.k_PFPlanBusinessCode, 0)

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanRefundType, 0) = m_oOldPFArray(bSIRPremFinConst.k_PFPlanRefundType, 0)

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanReference, 0) = m_oOldPFArray(bSIRPremFinConst.k_PFPlanReference, 0)
            End If

            'MKW120903 PF 3rd Party MTA START
            If m_lSchemeTypeID = bSIRPremFinConst.PFThirdParty Or m_lSchemeTypeID = bSIRPremFinConst.PFThirdPartyViaStargate Then

                m_lReturn = g_oBusiness.PostMTA_ThirdParty(m_vFinancePlanArray, m_oOldTransArray, m_oOldPFArray)
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lFinancePlanCnt = CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFCnt, 0))
                    m_lFinancePlanVersion = CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFVersion, 0))
                    m_lReturn = GetBusiness()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn
                        Throw New Exception()
                    End If

                    m_lReturn = BusinessToInterface()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn
                        Throw New Exception()
                    End If

                    'TR - Everything worked OK
                    MsgBoxSirius("The MTA has processed successfully.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "MTA Processed")

                    'And close down for Third Party
                    Me.Hide()

                Else
                    ' CJB 100904 PN14774 Handle no quotes gracefully
                    If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then

                        m_vFinancePlanMTATransArray = Nothing
                        MsgBoxSirius("The finance plan does not have any rates setup for the premium being charged.", MsgBoxStyle.Exclamation, "MTA Failed")
                    End If
                End If

            Else
                If m_vFinancePlanArray(85, 0) <> m_oOldPFArray(85, 0) Then
                    MsgBoxSirius("The MTA has been spread over the remaining instalments." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Press OK to show the new Plan.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "MTA Processed")
                End If
                BusinessToInterface()

            End If
            'Party Bank Details
            PopulateBankDetails()
            'MKW120903 PF 3rd Party MTA END

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to process the MTA", ACApp, ACClass, "ProcessMTA", Information.Err().Number, excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '*************************************************************************
    'Name:          QuoteFinanceForTransactions
    'Description:   Populates the Premium Finance Quotes form with all
    '               required Data for an MTA Quote and displays it.
    'History:       TR101202 - Created as per TS23
    '*************************************************************************
    Private Function QuoteFinanceForTransactions() As Integer
        Dim result As Integer = 0
        Dim vKeyArray(,) As Object = Nothing
        Dim oFinancePlanQuote As iPMBFinancePlanQuote.Interface_Renamed

        Try

            'TR - Assume success
            result = gPMConstants.PMEReturnCode.PMTrue

            'TR - Create the FinancePlanQuote interface
            Dim temp_oFinancePlanQuote As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oFinancePlanQuote, "iPMBFinancePlanQuote.Interface_Renamed", gPMConstants.PMGetLocalInterface)
            oFinancePlanQuote = temp_oFinancePlanQuote
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception()
            End If

            'TR - Create an array and Populate it with the Data that the
            'Quote Form requires

            'Extra param - insurance_file_cnt for U/W
            ReDim vKeyArray(1, 7)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNamePartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientId, 0)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameFinancePlanCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFCnt, 0)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameFinancePlanVersion

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFVersion, 0)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameTaskGroupCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = "MTA"
            'TR - Use this Constant for MTAType as it's only ever going to be used here

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameTransactionType

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_lMTAType

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNameFinancePlanTransactions

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_vFinancePlanMTATransArray

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.PMKeyNameInsuranceFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanInsuranceFileCnt, 0)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = PMNavKeyConst.PMKeyNamePlanIsSingleInstalment

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = bIsSinglePlanParty

            'TR - Use the Quote Form
            With oFinancePlanQuote
                'TR - Now Populate the Quote form with the Data it requires

                .CallingAppName = ACApp
                .SetKeys(vKeyArray)
                .SetProcessModes(gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, gPMConstants.PMEProcessMode.PMProcessModeGeneric, , DateTime.Now)
                .Start()
                .GetKeys(vKeyArray)
                .Dispose()
                'TR - Make sure that the user didn't cancel
                If .Status = gPMConstants.PMEReturnCode.PMCancel Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            'TR - Save the TransArray passed in to the modular variable
            'm_vFinancePlanMTATransArray = vKeyArray(1, 1)

            m_lFinancePlanVersion = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4))

            'TR - Destroy objects
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Error Displaying Finance Quotes", ACApp, ACClass, "QuoteFinanceForTransactions", Information.Err().Number, excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '*************************************************************************
    'Name:          SelectTransactions
    'Description:   Populates the Transaction Find form with all
    '               required Data to select additional transactions
    'History:       TR101202 - Created as per TS23
    '*************************************************************************
    Private Function SelectTransactions() As Integer
        Dim result As Integer = 0
        Dim vKeyArray(,) As Object = Nothing

        Dim oSelectTransScreen As iPMBFinanceTransactions.Interface_Renamed

        Try

            'TR - Assume success
            result = gPMConstants.PMEReturnCode.PMTrue

            'TR - Create the FinanceTransactions interface
            Dim temp_oSelectTransScreen As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oSelectTransScreen, "iPMBFinanceTransactions.Interface_Renamed", gPMConstants.PMGetLocalInterface)
            oSelectTransScreen = temp_oSelectTransScreen
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception()
            End If

            'TR - Create an array and Populate it with the Data that the Finance
            'Transactions Form requires
            'Thinh Nguyen 01/02/2004 - limit transactions to this policy
            ReDim vKeyArray(1, 4)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNamePartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lPartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameClientCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_sShortName

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameFinancePlanCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_lFinancePlanCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNamePlanInsuranceFolderCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_lPlanInsuranceFolderCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNamePlanIsSingleInstalment

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = bIsSinglePlanParty

            'TR - Use the screen object
            With oSelectTransScreen
                'TR - Now Populate the  form with the Data it requires

                m_lReturn = .SetKeys(vKeyArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Throw New Exception()
                End If

                m_lReturn = .SetProcessModes(, gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, gPMConstants.PMEProcessMode.PMProcessModeGeneric, , DateTime.Now)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Throw New Exception()
                End If

                m_lReturn = .Start()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'GoTo Err_SelectTransactions
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .GetKeys(vKeyArray)

                .Dispose()

                If .Status = gPMConstants.PMEReturnCode.PMCancel Or Not Information.IsArray(vKeyArray) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            'TR - Save the TransArray passed in to the modular variable

            m_vFinancePlanMTATransArray = vKeyArray(1, 0)

            'TR - Destroy objects
            oSelectTransScreen = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Error Processing MTA's", ACApp, ACClass, "SelectTransactions", Information.Err().Number, excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessCash
    ' Description: Start the navigator map
    ' History: 05/09/1999 CTAF - Created.
    ' ***************************************************************** '
    Private Function ProcessCash(Optional ByVal v_vAmount As Object = Nothing, Optional isSettlement As Boolean = False, Optional ByVal v_iSchemeCurrencyID As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lBatchId As Integer
        Dim vKeyArray As Object = Nothing
        Dim lAccountID As Integer

        Dim sOptionValue As String = ""

        'SMJB 02/09/03
        Dim sInsuranceRef As String = ""

        Const kiSysOptUseCashDrawers As Integer = 4100

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(m_vFinancePlanTransArray) Then
                MsgBoxSirius("No Transactions selected")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Busy mouse
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'MKW020204 PN10133 Retrieve values locally.
            m_lInsuranceFile_cnt = gPMFunctions.ToSafeLong(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanInsuranceFileCnt, 0), 0)

            If m_lInsuranceFile_cnt > 0 Then

                m_lReturn = g_oBusiness.GetInsuranceMediaRef(v_lInsuranceFile_cnt:=m_lInsuranceFile_cnt, r_sMediaRef:=sInsuranceRef)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the media ref", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCash", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If
            End If

            ' Create a batch
            m_lReturn = CreateBatch(m_vFinancePlanTransArray, lBatchId)

            ' Reset mouse
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get an instance of iPMNavStart
            m_oNavStart = New iPMNavStart.Interface_Renamed()

            ' Initialise it

            m_lReturn = m_oNavStart.Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise m_oNavStart.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCash", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Set its properties
            m_oNavStart.CallingAppName = ACApp

            ' Set the process to start
            '    sProcessCode = ACProcessCode
            '    m_oNavStart.ProcessCode = sProcessCode
            m_oNavStart.ProcessCode = ACProcessCode

            If ToSafeInteger(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAgentCnt, 0)).Trim()) <> 0 Then
                m_lReturn = GetOrionAccountId(ToSafeInteger(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAgentCnt, 0)).Trim()), lAccountID)
            Else
                m_lReturn = GetOrionAccountId(m_lPartyCnt, lAccountID)
            End If

            If iPMFunc.GetSystemOption(
                   v_iOptionNumber:=kiSysOptUseCashDrawers,
                   r_sOptionValue:=sOptionValue) <> gPMConstants.PMEReturnCode.PMTrue Then

                ProcessCash = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(
                                    iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                    sMsg:="Failed to start m_oNavStart",
                                    vApp:=ACApp,
                                    vClass:=ACClass,
                                    vMethod:="ProcessCash",
                                    vErrNo:=Err.Number,
                                    vErrDesc:=Err.Description)
                Exit Function

                m_oNavStart.Dispose()

            End If
            '#End If

            'Use cashdrawers if return value is set
            'SMJB CQ2155 02/09/03 Extra key added at index 5
            If sOptionValue = "1" Then

                ReDim vKeyArray(1, 10)

                'Indicates to cashlist that once the drawer is selected to go directly to
                'the details form of cashlistitem

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 8) = PMNavKeyConst.PMKeyNameScreenType

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8) = "SELECT"

                'Start cashlist at the list form

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNameStartForm

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = "start_list"

                'DD 02/09/2003: Hide the Cash List Items

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 10) = PMNavKeyConst.ACTKeyNameCashListItemMode

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 10) = "2"
            Else
                'use the old cashlists as before
                ReDim vKeyArray(1, 7)
            End If

            'The XML roadmap to use
            m_oNavStart.NavXMLFile = "PFCASH.XML"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameBatchID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = lBatchId

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameAccountID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = lAccountID

            If Information.IsNothing(v_vAmount) Then

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameFinanceDeposit

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = CDec(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDeposit, 0))
            Else
                Dim dbNumericTemp As Double
                If Double.TryParse(CStr(v_vAmount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameFinanceDeposit

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = v_vAmount
                End If
            End If

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.ACTKeyNameCashListAllocationRoadmap

            If isSettlement = True Then
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = "PFCASHSETTLEMENT"
            Else
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = "PFCASH"
            End If

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.ACTKeyAllowAllocateButton
            'SMJB CQ2155 02/09/03 - Pass insurance ref

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNameInsReference

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = sInsuranceRef
            'DD 02/09/2003: Pass through Branch

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.PMKeyNameSourceId

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanSource_ID, 0)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = PMNavKeyConst.PMKeyNameCurrencyKey

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = v_iSchemeCurrencyID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = False

            m_lReturn = m_oNavStart.SetKeys(vKeyArray:=vKeyArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_bNavCompleted = False

            ' Start it
            If m_sCallingAppName = "" Or m_sCallingAppName = "iPMBFindFinancePlan" Then
                m_oNavStart.IsChildNavigatorON = True
            End If

            m_lReturn = m_oNavStart.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start m_oNavStart", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCash", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Do
                Application.DoEvents()
            Loop While Not m_bNavCompleted

            ' RAW 07/03/2003 : added
            ' Terminate Navigator
            m_oNavStart.Dispose()
            m_oNavStart = Nothing
            ' RAW 07/03/2003 : end

            If m_bProcessComplete Then
                'worked
                result = gPMConstants.PMEReturnCode.PMTrue
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'AJM 27112001 - Use business object to get process property details

            m_lReturn = g_oBusiness.GetUserProperty(g_sUserName, "Cash Transaction", m_lDepositTransId)

            m_lReturn = g_oBusiness.DeleteUserProperty(g_sUserName, "Cash Transaction")

            'PN11263

            m_lReturn = g_oBusiness.GetUserProperty(g_sUserName, "Cash Amount", m_cDepositAmount)

            m_lReturn = g_oBusiness.DeleteUserProperty(g_sUserName, "Cash Amount")

            m_lAccountID = lAccountID
            'PN11263End

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Messagex
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessCash Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCash", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: OpenNewPlanForm
    '
    ' Description:  Opens another Plan form. The History flag opens the
    '               form in read only mode.
    '
    ' History: 03/10/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function OpenNewPlanForm(ByRef lFinancePlanCnt As Integer, ByRef lFinancePlanVersion As Integer, ByRef bHistory As Boolean) As Integer

        Dim result As Integer = 0

        Dim m_oAnother As Interface_Renamed = Nothing

        Try

            'try and create another instance of meself !
            If m_oAnother Is Nothing Then

                ' Get an instance of the coinsurer interface object via
                ' the public object manager.
                Dim temp_m_oAnother As Object = Nothing
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oAnother, sClassName:="iPMBFinancePlanMaint.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oAnother = temp_m_oAnother

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get another object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdHistory_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result

                End If
                m_oAnother.History = bHistory
                m_oAnother.Spawned = True
                m_oAnother.FinancePlanCnt = lFinancePlanCnt
                m_oAnother.FinancePlanVersion = lFinancePlanVersion

                m_lReturn = m_oAnother.Start()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get another object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdHistory_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result

                End If
                m_oAnother.Dispose()

                m_oAnother = Nothing
            End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OpenNewPlanForm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenNewPlanForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetOrionAccountID
    ' Description: Gets the Account Id for Party
    ' History: 06/12/2000 ECK - Created.
    ' ***************************************************************** '
    Private Function GetOrionAccountId(ByVal lpartyCnt As Integer, ByRef lAccountID As Integer) As Integer
        Dim result As Integer = 0
        Dim oExplorer As bACTExplorer.Form

        Dim oParty As bSIRParty.Services
        Dim lPartyId As Integer
        Dim iSourceId As Integer
        Dim vAccountIds As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue

        Dim temp_oParty As Object = Nothing
        m_lReturn = g_oObjectManager.GetInstance(temp_oParty, "bSIRParty.Services", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oParty = temp_oParty

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MsgBoxSirius("Unable to get Party Business")
            Return result
        End If
        With oParty

            .PartyCnt = lpartyCnt

            m_lReturn = .GetDetails()

            iSourceId = .SourceID

            lPartyId = .PartyID
        End With

        Dim lAccountKey As Integer = lpartyCnt

        Dim temp_oExplorer As Object = Nothing
        m_lReturn = g_oObjectManager.GetInstance(temp_oExplorer, "bACTExplorer.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oExplorer = temp_oExplorer

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MsgBoxSirius("Unable to get ACcount Id")
            Return result
        End If

        m_lReturn = oExplorer.GetAccountIdFromKey(v_lKey:=lAccountKey, r_vAccountIds:=vAccountIds)

        lAccountID = CInt(vAccountIds(0, 0))

        oExplorer.Dispose()

        oExplorer = Nothing

        Return result

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Messagex
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOrionACcountId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOrionAccountID", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: CreateBatch
    ' Description: Creates a new batch with the TransDetailIDs, for the
    '              roadmap.
    ' History: 14/09/1999 CTAF - Created.
    ' ***************************************************************** '
    Private Function CreateBatch(ByVal v_vTransDetailIDs(,) As Object, ByRef r_lBatchID As Integer) As Integer
        Dim result As Integer = 0

        Dim oBatch As bPMNavBatch.Business

        Dim vBatchArray As Array

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the batch object
            Dim temp_oBatch As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oBatch, "bPMNavBatch.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oBatch = temp_oBatch
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bPMNavBatch", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBatch", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Create the batch

            m_lReturn = oBatch.CreateBatchSet(v_sNavBatchCode:=ACBatchCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to oBatch.CreateBatchSet", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBatch", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Get the ID

            r_lBatchID = oBatch.BatchSetID

            ' Convert the batch to a 2d array

            vBatchArray = Array.CreateInstance(GetType(Object), New Integer() {1, v_vTransDetailIDs.GetUpperBound(1) - v_vTransDetailIDs.GetLowerBound(1) + 1}, New Integer() {0, v_vTransDetailIDs.GetLowerBound(1)})

            For iLoop1 As Integer = v_vTransDetailIDs.GetLowerBound(1) To v_vTransDetailIDs.GetUpperBound(1)

                vBatchArray(0, iLoop1) = v_vTransDetailIDs(0, iLoop1)
            Next iLoop1

            ' Set the values in the batch

            m_lReturn = oBatch.AddBatchRecord(v_vBatchArray:=vBatchArray, v_sNavBatchCode:=ACBatchCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to oBatch.AddBatchRecord", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBatch", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Terminate and remove the instance

            oBatch.Dispose()
            oBatch = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateBatch Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBatch", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' CommandSet
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CommandSet() As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim bIsMTA As Boolean

        If History Then
            ' History Mode
            SetCommand(cmdRePrint, False, False)
            SetCommand(cmdDelete, False, False)
            SetCommand(cmdReSend, False, False)
            SetCommand(cmdSettlePlan, False, False)
            SetCommand(cmdMTA, False, False)
            SetCommand(cmdCancel, False, False)
            SetCommand(cmdTransact, False, False)
            SetCommand(cmdSave, False, False)
            SetCommand(cmdRelease, False, False)
            SetCommand(cmdHistory, True, False)
            SetCommand(cmdCancelPolicy, False, False)
            m_lReturn = DisableForm(bDisable:=True)
        Else
            ' Set state for history button
            SetCommand(cmdHistory, True, m_lFinancePlanVersion > 1)
            ' Determine configuration of buttons
            'TR - Is this a Parent Plan? If so hide everything except the Save button
            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanIsParentPlan, 0)) = "1" Then
                SetCommand(cmdRePrint, False, False)
                SetCommand(cmdDelete, False, False)
                SetCommand(cmdReSend, False, False)
                SetCommand(cmdSettlePlan, False, False)
                SetCommand(cmdMTA, False, False)
                SetCommand(cmdCancel, False, False)
                SetCommand(cmdTransact, False, False)
                SetCommand(cmdSave, True, True)
                SetCommand(cmdRelease, False, False)
                SetCommand(cmdCancelPolicy, False, False)
                'TR - Otherwise, If it's been saved then show a few more, but
                'still hide Send, Settle, MTA, Cancel, Release and Transact buttons
            ElseIf m_sStatusInd = bSIRPremFinConst.PFStatusIndSaved Then
                'TR - Hide more buttons if in Third Party Recovery Mode
                If m_lSchemeTypeID = bSIRPremFinConst.PFThirdPartyRecovery Then
                    SetCommand(cmdRePrint, False, False)
                    SetCommand(cmdDelete, False, False)
                Else
                    'DC140704 PN12448 disabled until saved
                    SetCommand(cmdRePrint, True, False, "&Print")
                    If iSIRPremFinConst.m_sTransType = "MTA" Or iSIRPremFinConst.m_sTransType = "M" Then
                        SetCommand(cmdDelete, True, False)
                    Else
                        SetCommand(cmdDelete, True, True)
                    End If
                End If
                SetCommand(cmdReSend, False, False)
                SetCommand(cmdSettlePlan, False, False)
                SetCommand(cmdMTA, False, False)
                SetCommand(cmdCancel, False, False)
                SetCommand(cmdTransact, False, False)
                SetCommand(cmdSave, True, True)
                SetCommand(cmdRelease, False, False)
                SetCommand(cmdCancelPolicy, False, False)
                'TR - If it's been Updated or Printed, then Check that this is an
                'MTA, and show the Transact butotn if it is.
            ElseIf m_sStatusInd = bSIRPremFinConst.PFStatusIndUpdated Or m_sStatusInd = bSIRPremFinConst.PFStatusIndQuotePrinted Then
                'TR - Hide more buttons if in Third Party Recovery Mode
                If m_lSchemeTypeID = bSIRPremFinConst.PFThirdPartyRecovery Then
                    SetCommand(cmdRePrint, False, False)
                    SetCommand(cmdDelete, False, False)
                    SetCommand(cmdTransact, False, False)
                    SetCommand(cmdSave, True, True)
                Else
                    'DC140704 PN12448 should be print then later reprint
                    SetCommand(cmdRePrint, True, True, "&Print")
                    bIsMTA = Information.IsArray(m_vFinancePlanMTATransArray)
                    If iSIRPremFinConst.m_sTransType = "MTA" Or iSIRPremFinConst.m_sTransType = "M" Then
                        SetCommand(cmdTransact, bIsMTA, bIsMTA)
                        SetCommand(cmdSave, Not bIsMTA, Not bIsMTA)
                        SetCommand(cmdDelete, True, False)
                    Else
                        SetCommand(cmdTransact, Not bIsMTA, Not bIsMTA)
                        SetCommand(cmdSave, bIsMTA, bIsMTA)
                        SetCommand(cmdDelete, True, True)
                    End If
                End If
                SetCommand(cmdReSend, False, False)
                SetCommand(cmdSettlePlan, False, False)
                SetCommand(cmdMTA, False, False)
                SetCommand(cmdCancel, False, False)
                SetCommand(cmdRelease, False, False)
                SetCommand(cmdCancelPolicy, False, False)
            ElseIf m_sStatusInd = bSIRPremFinConst.PFStatusIndLive Then
                'TR - Hide more buttons if in Third Party Recovery Mode
                If m_lSchemeTypeID = bSIRPremFinConst.PFThirdPartyRecovery Then
                    SetCommand(cmdRePrint, False, False)
                    SetCommand(cmdCancel, False, False)
                    SetCommand(cmdMTA, False, False)
                    SetCommand(cmdReSend, False, False)
                    SetCommand(cmdSettlePlan, False, False)
                    SetCommand(cmdCancelPolicy, False, False)
                Else
                    SetCommand(cmdRePrint, True, True, "Re&Print")
                    SetCommand(cmdCancel, True, True)
                    SetCommand(cmdCancelPolicy, True, False)
                    ' CJB 130904 PN14930 - Don't show MTA button if going thru NB generic process
                    If m_sTransactionType = gPMConstants.PMTransactionTypeNB Then
                        SetCommand(cmdMTA, False, False)
                    Else
                        SetCommand(cmdMTA, True, True)
                    End If

                    SetCommand(cmdReSend, Not (m_lSchemeTypeID <> bSIRPremFinConst.PFThirdParty), Not (m_lSchemeTypeID <> bSIRPremFinConst.PFThirdParty))
                    SetCommand(cmdSettlePlan, m_lSchemeTypeID <> bSIRPremFinConst.PFThirdParty, m_lSchemeTypeID <> bSIRPremFinConst.PFThirdParty)
                End If
                SetCommand(cmdDelete, False, False)
                SetCommand(cmdTransact, False, False)
                SetCommand(cmdSave, True, True)
                SetCommand(cmdRelease, False, False)

            ElseIf m_sStatusInd = bSIRPremFinConst.PFStatusIndOnHold And m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                SetCommand(cmdRePrint, False, False)
                SetCommand(cmdDelete, False, False)
                SetCommand(cmdReSend, False, False)
                SetCommand(cmdSettlePlan, False, False)
                SetCommand(cmdMTA, False, False)
                SetCommand(cmdCancel, False, False)
                SetCommand(cmdTransact, False, False)
                SetCommand(cmdSave, False, False)
                SetCommand(cmdRelease, True, True)
                SetCommand(cmdCancelPolicy, False, False)
                ' the plan is on hold
                ' you cant save until the plan is released
                ' so lock down all the controls on the tabs until the plan is released
                PaymentAgreementInterfaceProcessing(True)

            ElseIf m_sStatusInd = bSIRPremFinConst.PFStatusIndCancelled Then  'PN 27807 Condition added for Cancelled Plans
                SetCommand(cmdSave, True, True)
                SetCommand(cmdRePrint, True, True)
                SetCommand(cmdDelete, False, False)
                SetCommand(cmdReSend, False, False)
                SetCommand(cmdSettlePlan, False, False)
                SetCommand(cmdMTA, False, False)
                SetCommand(cmdCancel, False, False)
                SetCommand(cmdTransact, False, False)
                SetCommand(cmdRelease, False, False)
                SetCommand(cmdCancelPolicy, False, False)
            Else
                If m_sStatusInd = bSIRPremFinConst.PFStatusIndSuperseded Then
                    SetCommand(cmdSave, True, False)
                Else
                    SetCommand(cmdSave, True, True)
                End If
                SetCommand(cmdRePrint, True, True)
                SetCommand(cmdDelete, False, False)
                SetCommand(cmdReSend, False, False)
                SetCommand(cmdSettlePlan, False, False)
                SetCommand(cmdMTA, False, False)
                SetCommand(cmdCancel, True, True)
                SetCommand(cmdCancelPolicy, True, False)
                SetCommand(cmdTransact, False, False)
                SetCommand(cmdRelease, False, False)

            End If
        End If

        '(RC) PLICO 9-10
        'Hide the MTA button if the Finance Plan is Live and third party
        If m_sStatusInd = bSIRPremFinConst.PFStatusIndLive And m_lSchemeTypeID = bSIRPremFinConst.PFThirdParty Then
            SetCommand(cmdMTA, False, False)
        End If

        If m_sStatusInd = bSIRPremFinConst.PFStatusIndLive OrElse m_sStatusInd = bSIRPremFinConst.PFStatusIndCompleted Then
            btnReverseInstalment.Enabled = True
        Else
            btnReverseInstalment.Enabled = False
        End If
        If m_iTask = gPMConstants.PMEComponentAction.PMView Then
            cmdSave.Enabled = False
            cmdDelete.Enabled = False
            cmdMTA.Enabled = False
            DisableForm(True)
            addClient.Enabled = False
            uctPMAgentAddressControl.Enabled = False
            addCardholder.Enabled = False
            addBank.Enabled = False
            btnReverseInstalment.Enabled = False
        End If

        Return nResult
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToBusiness
    '
    ' Description: Updates all business members from the interface details.
    ' ***************************************************************** '
    Public Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0
        Dim sActionCode As String = ""
        Dim vMediaHistoryPrevId As Object = Nothing
        Dim vMediaHistoryCurrId As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the business object.

            ' Assign the details from the interface to the data storage.
            m_lReturn = InterfaceToData()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not m_bUpdate Then
                Return result
            Else
                m_bUpdate = False
                m_sStatusInd = bSIRPremFinConst.PFStatusIndUpdated
            End If

            ' RAM20050907 - PN23789 - We need to check whether we are going to
            '                           do a new plan, if not, then we are doing
            '                           an MTA on the existing installment Plan, then we
            '                           have to see check do we have an m_vFinancePlanMTATransArray
            '                           So, if we supply the array to the business
            '                               object, it will double the AmountToFinance Field in
            '                               PFPremiumFinance Table, since it contains the full transaction
            '                               amount for the whole MTA Process

            m_bNewTrans = Information.IsArray(m_vFinancePlanMTATransArray)

            If m_bNewTrans Then
                'New transactions to be financed
                Select Case m_sStatusInd
                    Case bSIRPremFinConst.PFStatusIndSaved, bSIRPremFinConst.PFStatusIndUpdated

                        m_lReturn = g_oBusiness.UpdateExistingRecord(vExistingRecord:=m_vFinancePlanArray,
                                                                                     vPremiumFinanceCnt:=m_lFinancePlanCnt,
                                                                                     vPremiumFinanceVersion:=m_lFinancePlanVersion)
                    Case bSIRPremFinConst.PFStatusIndLive

                        m_lReturn = g_oBusiness.InsertNewVersion(m_vFinancePlanArray, m_lFinancePlanCnt, m_lFinancePlanVersion, m_vFinancePlanMTATransArray)
                End Select
            Else
                ' No Transactions added here

                m_lReturn = g_oBusiness.UpdateExistingRecord(vExistingRecord:=m_vFinancePlanArray,
                                                             vPremiumFinanceCnt:=m_lFinancePlanCnt,
                                                             vPremiumFinanceVersion:=m_lFinancePlanVersion)
            End If

            If m_bCreditCardOrBankDetailsChanged Or iSIRPremFinConst.m_sTransType = "REN" Then

                If m_sTransactionType = "NB" Or m_sTransactionType = "REN" Or Not Information.IsArray(m_vMediaTypeHistory) Then
                    sActionCode = "Setup"
                Else

                    If m_bCCCancelled Or m_bDDCancelled Then
                        sActionCode = kActionCodeCancellation
                    Else
                        If iSIRPremFinConst.m_sTransType = "MTA" And Information.IsArray(m_vPreviousFinancePlanArray) Then
                            If gPMFunctions.ToSafeString(CStr(m_vPreviousFinancePlanArray(bSIRPremFinConst.k_PFPlanBankAccountName, 0)).Trim()) <> gPMFunctions.ToSafeString(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankAccountName, 0)).Trim()) Or gPMFunctions.ToSafeString(CStr(m_vPreviousFinancePlanArray(bSIRPremFinConst.k_PFPlanBankAccountNo, 0)).Trim()) <> gPMFunctions.ToSafeString(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankAccountNo, 0)).Trim()) Or gPMFunctions.ToSafeString(CStr(m_vPreviousFinancePlanArray(bSIRPremFinConst.k_PFPlanBankSortCode, 0)).Trim()) <> gPMFunctions.ToSafeString(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankSortCode, 0)).Trim()) Or gPMFunctions.ToSafeString(CStr(m_vPreviousFinancePlanArray(bSIRPremFinConst.k_PFPlanAutoGenPlanRef, 0)).Trim()) <> gPMFunctions.ToSafeString(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAutoGenPlanRef, 0)).Trim()) Then
                                sActionCode = kActionCodeAmendment
                            End If
                        ElseIf iSIRPremFinConst.m_sTransType <> "MTA" And Not Information.IsArray(m_vPreviousFinancePlanArray) And m_bCreditCardOrBankDetailsChanged Then
                            sActionCode = kActionCodeAmendment
                        End If
                    End If

                End If
                If gPMFunctions.ToSafeString(sActionCode.Trim()) <> "" And Not (m_bBankDetailsAdded Or m_bBankDetailsEdited Or m_bCreditCardEdited) Then

                    m_lReturn = g_oBusiness.SaveInstalmentsPlanMediaTypeDetails(m_lFinancePlanCnt, m_lFinancePlanVersion, sActionCode)
                End If
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to " &
                                   "business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            End If

            'DD 31/07/2003: Added for Premium Finance
            'DC120804 PN14147 only update fields if applicable
            If CDbl(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanSchemeType_ID, 0)) = bSIRPremFinConst.PFThirdParty Or CDbl(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanSchemeType_ID, 0)) = bSIRPremFinConst.PFThirdPartyViaStargate Then

                If (m_oParty.PartyType = "PC" And m_bRequireDateOfBirth) Or (m_oParty.PartyType = "CC" And m_bRequireCompanyReg) Then

                    'DC120804 PN14147 only update if personal client

                    If m_oParty.PartyType = "PC" Then

                        If m_bRequireDateOfBirth Then

                            m_oParty.DateOfBirth = m_vDateOfBirth
                        End If

                    End If

                    'DC120804 PN14147 only update if corporate client

                    If m_oParty.PartyType = "CC" Then
                        If m_bRequireCompanyReg Then

                            m_oParty.CompanyReg = m_sCompanyReg
                        End If
                    End If

                    'DC120804 PN14147 only update if not group client

                    If m_oParty.PartyType <> "GC" Then

                        'Save the changes

                        m_lReturn = m_oParty.UpdateParty()

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the Party Services Object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
                        End If

                    End If

                End If

            End If

            'DC220206 PN26057 keep track of change of bank details
            If m_bBankAccountDetailsChange And cmdMTA.Enabled And m_lSchemeTypeID = bSIRPremFinConst.PFInHouse Then

                m_lReturn = g_oBusiness.UpdateBACSInstalmentStatus(m_lFinancePlanCnt, m_lFinancePlanVersion)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the BACS Agreement Instalment zero", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
                End If
            End If

            If iSIRPremFinConst.m_sTransType <> "MTA" And iSIRPremFinConst.m_sTransType <> "M" Then

                m_vFinancePlanMTATransArray = Nothing
            End If

            m_lReturn = g_oBusiness.GetMediaHistoryId(lPremFinanceCnt:=m_lFinancePlanCnt, lPremFinanceVersion:=m_lFinancePlanVersion, vMediaHistoryPrev:=vMediaHistoryPrevId, vMediaHistoryCurrent:=vMediaHistoryCurrId)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                If m_bDDCancelled Then

                    m_lReturn = g_oBusiness.CreateInstalmentNotification(m_lFinancePlanCnt, m_lFinancePlanVersion, "0C", vMediaHistoryPrevId)
                ElseIf CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanMediaTypeValidation, 0)).Trim() = "BANK" Then
                    If m_IsMediaTypeChanged Then

                        m_lReturn = g_oBusiness.CreateInstalmentNotification(m_lFinancePlanCnt, m_lFinancePlanVersion, "0N", vMediaHistoryCurrId)

                        m_lReturn = g_oBusiness.updateTransactionCode(m_lFinancePlanCnt, m_lFinancePlanVersion)
                    ElseIf m_bDDReinstated Then

                        m_lReturn = g_oBusiness.CreateInstalmentNotification(m_lFinancePlanCnt, m_lFinancePlanVersion, "0N", vMediaHistoryCurrId)

                    ElseIf m_bCreditCardOrBankDetailsChanged And iSIRPremFinConst.m_sTransType = "MTA" And Information.IsArray(m_vPreviousFinancePlanArray) Then
                        ' if payment frequency has changed and media type remains same for uploaded policies then no need to generate notifications
                        If iSIRPremFinConst.m_sTransType = "MTA" And (gPMFunctions.ToSafeLong(m_vPreviousFinancePlanArray(bSIRPremFinConst.k_PFPlanPfFrequency_ID, 0)) <> gPMFunctions.ToSafeLong(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPfFrequency_ID, 0)) And gPMFunctions.ToSafeLong(m_vPreviousFinancePlanArray(bSIRPremFinConst.k_PFPlanMediaType_ID, 0)) = gPMFunctions.ToSafeLong(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanMediaType_ID, 0))) And gPMFunctions.ToSafeString(CStr(m_vPreviousFinancePlanArray(bSIRPremFinConst.k_PFPlanAutoGenPlanRef, 0)).Trim()) = gPMFunctions.ToSafeString(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAutoGenPlanRef, 0)).Trim()) And sActionCode <> kActionCodeAmendment Then
                        ElseIf Not m_bOnlyAccountNameChanged Then

                            m_lReturn = g_oBusiness.CreateInstalmentNotification(m_lFinancePlanCnt, m_lFinancePlanVersion, "0C", vMediaHistoryPrevId)

                            m_lReturn = g_oBusiness.CreateInstalmentNotification(m_lFinancePlanCnt, m_lFinancePlanVersion, "0N", vMediaHistoryCurrId)
                        End If
                    ElseIf m_bCreditCardOrBankDetailsChanged And Not m_bOnlyAccountNameChanged And m_sCallingAppName = "iPMBFindFinancePlan" Then

                        m_lReturn = g_oBusiness.CreateInstalmentNotification(m_lFinancePlanCnt, m_lFinancePlanVersion, "0C", vMediaHistoryPrevId)

                        m_lReturn = g_oBusiness.CreateInstalmentNotification(m_lFinancePlanCnt, m_lFinancePlanVersion, "0N", vMediaHistoryCurrId)

                    End If
                ElseIf m_sOldMediaTypeCode.Trim().ToLower() = "bank" And CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanMediaTypeValidation, 0)).Trim().ToLower() = "cc" Then
                    If m_lFinancePlanVersion > 1 Then

                        m_lReturn = g_oBusiness.CreateInstalmentNotification(m_lFinancePlanCnt, m_lFinancePlanVersion - 1, "0C", vMediaHistoryPrevId)
                    End If
                End If
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         InterfaceToData
    ' Description:  Updates the data storage from the interface details.
    ' ***************************************************************** '
    Private Function InterfaceToData() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the data storage.
            m_bUpdate = False
            m_bCreditCardOrBankDetailsChanged = False
            m_bReviewDateChanged = False
            m_bOnlyAccountNameChanged = False
            If Information.IsArray(m_vFinancePlanMTATransArray) Then
                m_bUpdate = True
            End If

            ' Check for auto update to saved state
            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStatusInd, 0)) = bSIRPremFinConst.PFStatusIndSaved Then
                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStatusInd, 0) = bSIRPremFinConst.PFStatusIndUpdated
                m_bUpdate = True
            End If

            ' Store start date for scheme transact

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStartDate, 0)).Trim() <> CStr(m_oFormFields.UnformatControl(txtStartDate)).Trim() Then

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStartDate, 0) = m_oFormFields.UnformatControl(txtStartDate)
                m_bUpdate = True
            End If

            '********************
            ' MEvans : 09-06-2003 : bug fix

            If Convert.IsDBNull(dtpConfirmedDate.Value) Or IsNothing(dtpConfirmedDate.Value) Then

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDateConfirmed, 0) = DBNull.Value
                m_bUpdate = True
            Else
                'TR - 24/03/03 - TS17 Recovery By Instalments changes
                If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDateConfirmed, 0)).Trim() <> dtpConfirmedDate.Value.ToString() Then

                    m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDateConfirmed, 0) = dtpConfirmedDate.Value
                    m_bUpdate = True
                End If
            End If
            '********************

            If Not (Convert.IsDBNull(dtpReviewDate.Value) Or IsNothing(dtpReviewDate.Value)) Then
                If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDateReview, 0)).Trim() <> dtpReviewDate.Value.ToString() Then

                    m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDateReview, 0) = dtpReviewDate.Value
                    m_bUpdate = True
                End If
            Else
                'Developer Gudie No 113
                If Convert.ToString(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDateReview, 0)) <> "#12/29/1899#" Then

                    m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDateReview, 0) = DBNull.Value
                    m_bReviewDateChanged = True
                    m_bUpdate = True
                End If
            End If

            'TR - Are Statements switched on?
            If chkNoStatements.CheckState <> CheckState.Checked Then
                'TR - If they are on, is the Frequency the same as before?
                If cboStatementFrequency.SelectedIndex > -1 Then
                    If VB6.GetItemData(cboStatementFrequency, cboStatementFrequency.SelectedIndex) <> Conversion.Val(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStatementPFFrequencyID, 0))) Then
                        m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStatementPFFrequencyID, 0) = VB6.GetItemData(cboStatementFrequency, cboStatementFrequency.SelectedIndex)
                        m_bUpdate = True
                    End If
                Else

                    If m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStatementPFFrequencyID, 0) Is DBNull.Value Then

                        m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStatementPFFrequencyID, 0) = DBNull.Value
                        m_bUpdate = True
                    End If
                End If
            Else
                'TR - Statements are off - blank the frequency

                If m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStatementPFFrequencyID, 0) Is DBNull.Value Then

                    m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStatementPFFrequencyID, 0) = DBNull.Value
                    m_bUpdate = True
                End If
            End If

            ' Build Array's Client Components

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientName, 0)).Trim() <> CStr(m_oFormFields.UnformatControl(txtClientName)) Then

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientName, 0) = m_oFormFields.UnformatControl(txtClientName)
                m_bUpdate = True
            End If
            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientAddress1, 0)).Trim() <> addClient.AddressLine1 Then
                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientAddress1, 0) = addClient.AddressLine1
                m_bUpdate = True
            End If

            If Not (m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientAddress2, 0)) Is Nothing AndAlso CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientAddress2, 0)).Trim() <> addClient.AddressLine2 Then
                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientAddress2, 0) = addClient.AddressLine2
                m_bUpdate = True
            End If
            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientAddress3, 0)).Trim() <> addClient.AddressLine3 Then
                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientAddress3, 0) = addClient.AddressLine3
                m_bUpdate = True
            End If
            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientAddress4, 0)).Trim() <> addClient.AddressLine4 Then
                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientAddress4, 0) = addClient.AddressLine4
                m_bUpdate = True
            End If
            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientPostcode, 0)).Trim() <> addClient.PostCode Then
                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientPostcode, 0) = addClient.PostCode
                m_bUpdate = True
            End If

            If Conversion.Val(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientCountry_ID, 0)).Trim()) <> addClient.CountryId Then
                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientCountry_ID, 0) = addClient.CountryId
                m_bUpdate = True
            End If

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientAreaCode, 0)).Trim() <> CStr(m_oFormFields.UnformatControl(txtClientAreaCode)) Then

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientAreaCode, 0) = m_oFormFields.UnformatControl(txtClientAreaCode)
                m_bUpdate = True
            End If

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientPhone, 0)).Trim() <> CStr(m_oFormFields.UnformatControl(txtClientNumber)) Then

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientPhone, 0) = m_oFormFields.UnformatControl(txtClientNumber)
                m_bUpdate = True
            End If

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientExtn, 0)).Trim() <> CStr(m_oFormFields.UnformatControl(txtClientExtension)) Then

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientExtn, 0) = m_oFormFields.UnformatControl(txtClientExtension)
                m_bUpdate = True
            End If

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientFaxCode, 0)).Trim() <> CStr(m_oFormFields.UnformatControl(txtClientFaxCode)) Then

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientFaxCode, 0) = m_oFormFields.UnformatControl(txtClientFaxCode)
                m_bUpdate = True
            End If

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientFax, 0)).Trim() <> CStr(m_oFormFields.UnformatControl(txtClientFaxNumber)) Then

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientFax, 0) = m_oFormFields.UnformatControl(txtClientFaxNumber)
                m_bUpdate = True
            End If

            ' Build Array's Bank Components

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankName, 0)).Trim() <> CStr(m_oFormFields.UnformatControl(txtBankName)) Then

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankName, 0) = m_oFormFields.UnformatControl(txtBankName)
                m_bUpdate = True
                'm_bCreditCardOrBankDetailsChanged = True
            End If

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankAddress1, 0)).Trim() <> addBank.AddressLine1 Then
                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankAddress1, 0) = addBank.AddressLine1
                m_bUpdate = True
                'm_bCreditCardOrBankDetailsChanged = True
            End If
            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankAddress2, 0)).Trim() <> addBank.AddressLine2 Then
                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankAddress2, 0) = addBank.AddressLine2
                m_bUpdate = True
                'm_bCreditCardOrBankDetailsChanged = True
            End If
            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankAddress3, 0)).Trim() <> addBank.AddressLine3 Then
                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankAddress3, 0) = addBank.AddressLine3
                m_bUpdate = True
                'm_bCreditCardOrBankDetailsChanged = True
            End If
            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankAddress4, 0)).Trim() <> addBank.AddressLine4 Then
                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankAddress4, 0) = addBank.AddressLine4
                m_bUpdate = True
                'm_bCreditCardOrBankDetailsChanged = True
            End If
            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankPostcode, 0)).Trim() <> addBank.PostCode Then
                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankPostcode, 0) = addBank.PostCode
                m_bUpdate = True
                'm_bCreditCardOrBankDetailsChanged = True
            End If

            If Conversion.Val(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankCountry_ID, 0)).Trim()) <> addBank.CountryId Then
                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankCountry_ID, 0) = addBank.CountryId
                m_bUpdate = True
                'm_bCreditCardOrBankDetailsChanged = True
            End If

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankAreaCode, 0)).Trim() <> CStr(m_oFormFields.UnformatControl(txtAreaCode)) Then

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankAreaCode, 0) = m_oFormFields.UnformatControl(txtAreaCode)
                m_bUpdate = True
            End If

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankPhone, 0)).Trim() <> CStr(m_oFormFields.UnformatControl(txtNumber)) Then

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankPhone, 0) = m_oFormFields.UnformatControl(txtNumber)
                m_bUpdate = True
            End If

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankExtn, 0)).Trim() <> CStr(m_oFormFields.UnformatControl(txtExtension)) Then

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankExtn, 0) = m_oFormFields.UnformatControl(txtExtension)
                m_bUpdate = True
            End If

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankFaxCode, 0)).Trim() <> CStr(m_oFormFields.UnformatControl(txtFaxAreaCode)) Then

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankFaxCode, 0) = m_oFormFields.UnformatControl(txtFaxAreaCode)
                m_bUpdate = True
                'm_bCreditCardOrBankDetailsChanged = True
            End If

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankFax, 0)).Trim() <> CStr(m_oFormFields.UnformatControl(txtFaxNumber)) Then

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankFax, 0) = m_oFormFields.UnformatControl(txtFaxNumber)
                m_bUpdate = True
                'm_bCreditCardOrBankDetailsChanged = True
            End If
            ' Build Array's Bank Account Components
            'DD 19/08/2003: This field is shared as Credit Card Name
            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanMediaTypeValidation, 0)).Trim() = "BANK" Then
                If (m_lSchemeTypeID <> PFThirdParty) Then
                    If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankAccountName, 0)).Trim() <> CStr(m_oFormFields.UnformatControl(txtAccountName)) Then

                        m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankAccountName, 0) = m_oFormFields.UnformatControl(txtAccountName)
                        m_bUpdate = True
                        m_bCreditCardOrBankDetailsChanged = True
                        m_bOnlyAccountNameChanged = True
                        'DC220206 PN26057 show date bank details changed for use in adjusting payment dates
                        m_bBankAccountDetailsChange = True
                        txtDateBankDetailsChanged.Text = CStr(DateTime.Today)
                    End If
                End If
            End If

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankSortCode, 0)).Trim() <> CStr(m_oFormFields.UnformatControl(txtSortCode)) Then

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankSortCode, 0) = m_oFormFields.UnformatControl(txtSortCode)
                m_bUpdate = True
                m_bCreditCardOrBankDetailsChanged = True
                m_bOnlyAccountNameChanged = False
                'DC220206 PN26057 show date bank details changed for use in adjusting payment dates
                m_bBankAccountDetailsChange = True
                txtDateBankDetailsChanged.Text = CStr(DateTime.Today)

            End If
            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanMediaTypeValidation, 0)).Trim() = "BANK" Then
                If (m_lSchemeTypeID <> PFThirdParty) Then
                    If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankAccountNo, 0)).Trim() <> CStr(m_oFormFields.UnformatControl(txtAccountNumber)) Then

                        m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankAccountNo, 0) = m_oFormFields.UnformatControl(txtAccountNumber)
                        m_bUpdate = True
                        m_bCreditCardOrBankDetailsChanged = True
                        m_bOnlyAccountNameChanged = False
                    End If
                End If
            Else
                If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankAccountNo, 0)).Trim() <> "" Then

                    m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankAccountNo, 0) = ""
                    m_bUpdate = True
                    m_bCreditCardOrBankDetailsChanged = True
                End If
            End If

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.kBIC, 0)).Trim() <> CStr(m_oFormFields.UnformatControl(txtBIC)) Then

                m_vFinancePlanArray(bSIRPremFinConst.kBIC, 0) = m_oFormFields.UnformatControl(txtBIC)
                m_bUpdate = True
                m_bCreditCardOrBankDetailsChanged = True
            End If

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.kIBAN, 0)).Trim() <> CStr(m_oFormFields.UnformatControl(txtIBAN)) Then

                m_vFinancePlanArray(bSIRPremFinConst.kIBAN, 0) = m_oFormFields.UnformatControl(txtIBAN)
                m_bUpdate = True
                m_bCreditCardOrBankDetailsChanged = True
            End If


            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankBranch, 0)).Trim() <> CStr(m_oFormFields.UnformatControl(txtBranch)) Then

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankBranch, 0) = m_oFormFields.UnformatControl(txtBranch)
                m_bUpdate = True
                'm_bCreditCardOrBankDetailsChanged = True
            End If

            'DD 17/8/2004 PN14220 - Only write back to the array if the fields are not blank
            'The fields are blank for Third Party plans

            If (CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanFirstInstalmentdate, 0)).Trim() <> CStr(m_oFormFields.UnformatControl(txtFirstInstalmentDate)).Trim()) And txtFirstInstalmentDate.Text <> "" Then

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanFirstInstalmentdate, 0) = m_oFormFields.UnformatControl(txtFirstInstalmentDate)
                m_bUpdate = True
            End If

            If (CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanNextInstalmentdate, 0)).Trim() <> CStr(m_oFormFields.UnformatControl(txtNextInstalmentDate)).Trim()) And txtNextInstalmentDate.Text <> "" Then

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanNextInstalmentdate, 0) = m_oFormFields.UnformatControl(txtNextInstalmentDate)
                m_bUpdate = True
            End If

            If (CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanLastInstalmentdate, 0)).Trim() <> CStr(m_oFormFields.UnformatControl(txtLastInstalmentDate)).Trim()) And txtLastInstalmentDate.Text <> "" Then

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanLastInstalmentdate, 0) = m_oFormFields.UnformatControl(txtLastInstalmentDate)
                m_bUpdate = True
            End If
            'END DD 17/8/2004

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanTaxCost, 0)).Trim() <> CStr(m_oFormFields.UnformatControl(txtTaxes)).Trim() Then

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanTaxCost, 0) = m_oFormFields.UnformatControl(txtTaxes)
                m_bUpdate = True
            End If

            'DD 06/03/2003: If the CC Number has not been edited then pass back the
            'database value as the field has been masked.
            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanMediaTypeValidation, 0)).Trim() = "CC" Then

                If CStr(m_oFormFields.UnformatControl(txtCardNo)).IndexOf("*"c) >= 0 Then
                    m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCNumber, 0) = m_sCreditCardNo
                Else

                    If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCNumber, 0)).Trim() <> CStr(m_oFormFields.UnformatControl(txtCardNo)) Then

                        m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCNumber, 0) = m_oFormFields.UnformatControl(txtCardNo)
                        m_bUpdate = True
                        m_bCreditCardOrBankDetailsChanged = True
                    End If
                End If
            Else
                If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCNumber, 0)).Trim() <> "" Then
                    m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCNumber, 0) = ""
                    m_bUpdate = True
                    m_bCreditCardOrBankDetailsChanged = True
                End If
            End If
            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanMediaTypeValidation, 0)).Trim() = "CC" Then

                If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCExpiryDate, 0)).Trim() <> CStr(m_oFormFields.UnformatControl(txtExpiryDate)) Then

                    m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCExpiryDate, 0) = m_oFormFields.UnformatControl(txtExpiryDate)
                    m_bUpdate = True
                    m_bCreditCardOrBankDetailsChanged = True
                End If
            Else
                If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCExpiryDate, 0)).Trim() <> "" Then

                    m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCExpiryDate, 0) = ""
                    m_bUpdate = True
                    m_bCreditCardOrBankDetailsChanged = True
                End If
            End If

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCStartDate, 0)).Trim() <> CStr(m_oFormFields.UnformatControl(txtCardStartDate)) Then

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCStartDate, 0) = m_oFormFields.UnformatControl(txtCardStartDate)
                m_bUpdate = True
                m_bCreditCardOrBankDetailsChanged = True
            End If

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCIssue, 0)).Trim() <> CStr(m_oFormFields.UnformatControl(txtIssueNo)).Trim() Then

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCIssue, 0) = m_oFormFields.UnformatControl(txtIssueNo)
                m_bUpdate = True
                m_bCreditCardOrBankDetailsChanged = True
            End If

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCPin, 0)).Trim() <> CStr(m_oFormFields.UnformatControl(txtPin)) Then

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCPin, 0) = m_oFormFields.UnformatControl(txtPin)
                m_bUpdate = True
                m_bCreditCardOrBankDetailsChanged = True
            End If

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCardType, 0)).Trim() <> (cboCardType.Text) Then
                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCardType, 0) = cboCardType.Text
                m_bUpdate = True
                m_bCreditCardOrBankDetailsChanged = True
            End If

            'DD 11/08/2003: Added Credit Card name (shares Bank Account Name field)
            'DD 19/08/2003: This field is shared as Bank Account Name
            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanMediaTypeValidation, 0)).Trim() = "CC" Then

                If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankAccountName, 0)).Trim() <> CStr(m_oFormFields.UnformatControl(txtCardName)) Then

                    m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankAccountName, 0) = m_oFormFields.UnformatControl(txtCardName)
                    m_bUpdate = True
                    m_bCreditCardOrBankDetailsChanged = True
                End If
            End If
            'TR - 24/03/03 - TS17 Recovery By Instalments changes
            'TAB 5 - Agent Details
            'TR - See if the Agent has changed
            If Conversion.Val(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAgentCnt, 0))) <> m_lOriginalRecoveryAgentCnt Then
                m_bUpdate = True
            End If
            'TR - See if the AgentRef has changed
            If gPMFunctions.NullToString(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAgentRef, 0)) <> txtAgentRef.Text Then
                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAgentRef, 0) = txtAgentRef.Text
                m_bUpdate = True
            End If

            If cboBusinessCode.SelectedIndex > -1 Then
                If m_lBusinessCode_id <> cboBusinessCode.SelectedIndex + 1 Then
                    m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBusinessCode, 0) = m_vLookupDetails(2, cboBusinessCode.SelectedIndex)
                    m_bUpdate = True
                End If
            End If

            m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanMediaType_ID, 0) = m_lMediaTypeID
            m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFRF_ID, 0) = m_lPFRFID
            m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanSchemeType_ID, 0) = m_lSchemeTypeID

            'DD 31/07/2003: Added for Premium Finance
            If m_bRequireDateOfBirth Then
                m_vDateOfBirth = txtDateOfBirth.Text
            End If

            If m_bRequireCompanyReg Then
                m_sCompanyReg = txtCompanyReg.Text
            End If

            'DD 21/11/2003 Added for Premium Finance
            If gPMFunctions.NullToString(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAuthCode, 0)) <> txtAuthCode.Text Then
                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAuthCode, 0) = txtAuthCode.Text
                m_bUpdate = True
            End If

            'DD 17/12/2003 - Added so that user can override Sirius default
            If (CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAutoGenPlanRef, 0)).Trim() <> txtReference.Text) And (Not m_bReferenceExist) Then
                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAutoGenPlanRef, 0) = txtReference.Text
                m_bUpdate = True
                m_bCreditCardOrBankDetailsChanged = True
            End If

            'ACR 23-05-05 - Add the refund type (SG schemes only..)
            m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanRefundType, 0) = VB6.GetItemString(cboRefundType, cboRefundType.SelectedIndex)
            m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanLimitedCompany, 0) = chkPartnership.CheckState
            'END ACR

            'third party credit card for close iprompt..

            ' check if credit cardholders details have changed
            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanMediaTypeValidation, 0)).Trim() = "CC" Then
                If gPMFunctions.ToSafeInteger(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanIsCardholder, 0)) <> chkCardholder.CheckState Then
                    m_bCreditCardOrBankDetailsChanged = True
                    m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanIsCardholder, 0) = chkCardholder.CheckState
                End If
            End If

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCardType, 0)) <> cboCardType.Text Then
                m_bCreditCardOrBankDetailsChanged = True
            End If

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanCardholderName, 0)) <> txtCardholderName.Text Then
                m_bCreditCardOrBankDetailsChanged = True
            End If

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanCardholderAddress1, 0)) <> addCardholder.AddressLine1 Then
                m_bCreditCardOrBankDetailsChanged = True
            End If

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanCardholderAddress2, 0)) <> addCardholder.AddressLine2 Then
                m_bCreditCardOrBankDetailsChanged = True
            End If

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanCardholderAddress3, 0)) <> addCardholder.AddressLine3 Then
                m_bCreditCardOrBankDetailsChanged = True
            End If

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanCardholderAddress4, 0)) <> addCardholder.AddressLine4 Then
                m_bCreditCardOrBankDetailsChanged = True
            End If

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanCardholderPostcode, 0)) <> addCardholder.PostCode Then
                m_bCreditCardOrBankDetailsChanged = True
            End If

            m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanIsCardholder, 0) = chkCardholder.CheckState
            m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCardType, 0) = cboCardType.Text
            m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanCardholderName, 0) = txtCardholderName.Text
            m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanCardholderAddress1, 0) = addCardholder.AddressLine1
            m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanCardholderAddress2, 0) = addCardholder.AddressLine2
            m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanCardholderAddress3, 0) = addCardholder.AddressLine3
            m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanCardholderAddress4, 0) = addCardholder.AddressLine4
            m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanCardholderPostcode, 0) = addCardholder.PostCode

            If Conversion.Val(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanCardholderCountryID, 0)).Trim()) <> addCardholder.CountryId Then
                m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanCardholderCountryID, 0) = addCardholder.CountryId
                m_bUpdate = True
            End If

            If Not (Convert.IsDBNull(dtpCreatedDate.Value) Or IsNothing(dtpCreatedDate.Value)) Then

                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDateCreated, 0) = dtpCreatedDate.Value
            Else
                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDateCreated, 0) = DateTime.Today
            End If

            m_bDDCancelled = False

            ' Direct Debit Cancelled
            If chkDDCancelled.CheckState = CheckState.Checked Then
                If gPMFunctions.ToSafeLong(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDDCancelled, 0), 0) <> 1 Then
                    m_bCreditCardOrBankDetailsChanged = True
                    m_bDDCancelled = True
                    m_bUpdate = True
                End If
            ElseIf chkDDCancelled.CheckState = CheckState.Unchecked Then
                If gPMFunctions.ToSafeLong(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDDCancelled, 0), 0) <> 0 Then
                    m_bCreditCardOrBankDetailsChanged = True
                    m_bUpdate = True
                End If
            End If

            If chkDDCancelled.CheckState = CheckState.Checked Then
                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDDCancelled, 0) = 1
            Else
                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDDCancelled, 0) = 0
            End If

            m_bCCCancelled = False

            ' Credit Card Cancelled
            If chkCCCancelled.CheckState = CheckState.Checked Then
                If gPMFunctions.ToSafeLong(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCCancelled, 0), 0) <> 1 Then
                    m_bCreditCardOrBankDetailsChanged = True
                    m_bCCCancelled = True
                End If
            ElseIf chkCCCancelled.CheckState = CheckState.Unchecked Then
                If gPMFunctions.ToSafeLong(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCCancelled, 0), 0) <> 0 Then
                    m_bCreditCardOrBankDetailsChanged = True
                End If
            End If

            If chkCCCancelled.CheckState = CheckState.Checked Then
                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCCancelled, 0) = 1
            Else
                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCCancelled, 0) = 0
            End If

            ' Paper DD
            If chkPaperDD.CheckState = CheckState.Checked Then
                If gPMFunctions.ToSafeLong(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPaperDD, 0), 0) <> 1 Then
                    m_bCreditCardOrBankDetailsChanged = True
                End If
            ElseIf chkPaperDD.CheckState = CheckState.Unchecked Then
                If gPMFunctions.ToSafeLong(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPaperDD, 0), 0) <> 0 Then
                    m_bCreditCardOrBankDetailsChanged = True
                End If
            End If

            If chkPaperDD.CheckState = CheckState.Checked Then
                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPaperDD, 0) = 1
            Else
                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPaperDD, 0) = 0
            End If

            'DC220206 PN26057 show date bank details changed for use in adjusting payment dates
            If m_bBankAccountDetailsChange Or m_bCreditCardOrBankDetailsChanged Then
                m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanDateBankDetailsChanged, 0) = DateTime.Today
                m_bUpdate = True
            End If

            If m_bUpdate Then
                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDateModified, 0) = DateTime.Today
            Else

                If Not (Convert.IsDBNull(dtpModifiedDate.Value) Or IsNothing(dtpModifiedDate.Value)) Then

                    m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDateModified, 0) = dtpModifiedDate.Value
                End If
            End If

            'Party Bank Details
            If SSTabHelper.GetTabVisible(tabMainTab, kMainTabBankDetails) Then
                If gPMFunctions.ToSafeLong(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPartyBankIdSel, 0)) <> uctPartyBankCombo1.SelectedPaymentID Then
                    m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPartyBankIdSel, 0) = uctPartyBankCombo1.SelectedPaymentID
                    m_bUpdate = True
                    m_bCreditCardOrBankDetailsChanged = True
                End If
            Else
                If gPMFunctions.ToSafeLong(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPartyBankIdSel, 0)) <> uctPartyBankCombo2.SelectedPaymentID Then
                    m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPartyBankIdSel, 0) = uctPartyBankCombo2.SelectedPaymentID
                    m_bUpdate = True
                    m_bCreditCardOrBankDetailsChanged = True
                End If
            End If

            If Convert.ToString(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPartyBankIdSel, 0)) = "" OrElse CDbl(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPartyBankIdSel, 0)) = 0 Then
                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPartyBankIdSel, 0) = DBNull.Value
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessHistory (Standard Method)
    '
    ' Description: Calls the appropriate methods to process the interface.
    ' ***************************************************************** '
    Private Function ProcessHistory() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Load the interface into memory.
            m_ofrmHistory = New iPMBFinancePlanMaint.frmHistory()

            ' Assign the parameters to the interface properties.

            m_ofrmHistory.FinancePlanVersions = m_vFinanceHistory

            ' Load the instance of the interface into memory.

            ' Display the interface.
            m_ofrmHistory.ShowDialog()

            ' Assign the property members from the interface parameters.
            m_lHistoryVersion = m_ofrmHistory.Version

            ' Unload and destroy the instance of the interface from memory.
            m_ofrmHistory.Close()
            m_ofrmHistory = Nothing

            Return result

        Catch excep As System.Exception

            ' Release object and set error
            m_ofrmHistory = Nothing
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessHistory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessInstalment (Standard Method)
    ' Description: Calls the appropriate methods to process the interface.
    ' ***************************************************************** '
    Private Function ProcessInstalment() As Integer

        Dim result As Integer = 0
        Try

            Dim lPreviousStatus, lNewStatus As Integer
            Dim dtPreviousDuedate, dtNewDuedate As Date
            result = gPMConstants.PMEReturnCode.PMTrue

            If lvwInstalment.FocusedItem Is Nothing Then
                MsgBoxSirius("No instalment item selected.", MsgBoxStyle.Exclamation)
            Else
                m_ofrmInstalment = New iPMBFinancePlanMaint.frmInstalment()

                ' Assign the parameters to the interface properties.

                m_ofrmInstalment.Instalment = Convert.ToString(lvwInstalment.FocusedItem.Tag)
                m_ofrmInstalment.InstalmentArray = VB6.CopyArray(m_vFinancePlanInstalmentArray)
                m_ofrmInstalment.Task = m_iTask

                m_ofrmInstalment.Business = g_oBusiness

                lPreviousStatus = CInt(m_vFinancePlanInstalmentArray(bSIRPremFinConst.PFInstStatus, Convert.ToString(lvwInstalment.FocusedItem.Tag)))

                dtPreviousDuedate = Strings.Format(CDate(m_vFinancePlanInstalmentArray(bSIRPremFinConst.PFInstDueDate, Convert.ToString(lvwInstalment.FocusedItem.Tag))), "Short Date")

                If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAutoGenPlanRef, 0)).Trim() <> "" Then
                    m_ofrmInstalment.PlanAutoGenPlanRef = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAutoGenPlanRef, 0)).Trim()
                End If

                ' Load the instance of the interface into memory.
                If m_vFinancePlanInstalmentArray(bSIRPremFinConst.PFInstInstalmentNumber, CInt(Convert.ToString(lvwInstalment.FocusedItem.Tag))) = m_iEditableInstalmentNumber Then
                    m_ofrmInstalment.IsDueDateEditable = True
                End If

                'Store the cover end date
                If IsDate(m_vFinancePlanArray(k_PFPlanCoverEndDate, 0)) Then
                    m_ofrmInstalment.CoverExpiryDate = ToSafeDate(m_vFinancePlanArray(k_PFPlanCoverEndDate, 0))
                End If

                'Store the cover end date
                If IsDate(m_dtNextInstalmentDuedate) Then
                    m_ofrmInstalment.NextInstalmentDuedate = m_dtNextInstalmentDuedate
                End If

                ' Display the interface.
                m_ofrmInstalment.ShowDialog()

                m_vFinancePlanInstalmentArray = m_ofrmInstalment.InstalmentArray

                lNewStatus = CInt(m_vFinancePlanInstalmentArray(bSIRPremFinConst.PFInstStatus, Convert.ToString(lvwInstalment.FocusedItem.Tag)))
                dtNewDuedate = Strings.Format(CDate(m_vFinancePlanInstalmentArray(bSIRPremFinConst.PFInstDueDate, Convert.ToString(lvwInstalment.FocusedItem.Tag))), "Short Date")

                ' if the status has changed then update the listview
                If lPreviousStatus <> lNewStatus Then
                    lvwInstalment.FocusedItem.SubItems.Item(4).Text = iSIRPremFinConst.GetInstalmentStatus(CStr(lNewStatus))
                End If
                ' if Duedate has changed then update thelistview
                If dtPreviousDuedate <> dtNewDuedate Then
                    lvwInstalment.FocusedItem.SubItems.Item(1).Text = Strings.Format(dtNewDuedate, "Short Date")
                End If
                ' Unload and destroy the instance of the interface from memory.
                m_ofrmInstalment.Close()
                m_ofrmInstalment = Nothing
            End If

            Return result

        Catch excep As System.Exception

            ' Release object and set error
            m_ofrmInstalment = Nothing
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessInstalment", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessTransactions (Standard Method)
    ' Description: Calls the appropriate methods to process the interface.
    ' ***************************************************************** '
    Private Function ProcessTransactions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new instance of the form
            m_ofrmTransactions = New iPMBFinancePlanMaint.frmTransactions()

            ' Assign the parameters to the interface properties.

            m_ofrmTransactions.TransactionData = m_vTransactionArray
            ' Load the instance of the interface into memory.

            ' Display the interface.
            m_ofrmTransactions.ShowDialog()

            ' Unload and destroy the instance of the interface from memory.
            m_ofrmTransactions.Close()
            m_ofrmTransactions = Nothing

            Return result

        Catch excep As System.Exception

            ' Release object and set error
            m_ofrmTransactions = Nothing
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Private Function SetFieldValidation() As Integer

        Dim result As Integer = 0

        Try

            'Tab 0 - Plan Details
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboStatus, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtStartDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateShort, lFieldType:=gPMConstants.PMEDataType.PMDate, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFinancedAmount, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtNumberOfInstalments, lFieldType:=gPMConstants.PMEDataType.PMInteger, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDaysDelay, lFieldType:=gPMConstants.PMEDataType.PMInteger, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtInterestRate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lFieldType:=gPMConstants.PMEDataType.PMDecimal, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory, lDecimalPlaces:=2)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDeposit, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCostOfProtection, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAPR, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lFieldType:=gPMConstants.PMEDataType.PMDecimal, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory, lDecimalPlaces:=2)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFinanceCharge, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtTaxes, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFirstInstalmentDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateShort, lFieldType:=gPMConstants.PMEDataType.PMDate, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtNextInstalmentDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateShort, lFieldType:=gPMConstants.PMEDataType.PMDate, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtLastInstalmentDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateShort, lFieldType:=gPMConstants.PMEDataType.PMDate, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFirstInstalmentValue, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOtherInstalments, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtTotalAmount, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtReference, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            'Tab 1 - Client Information
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtClientName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtClientAreaCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtClientNumber, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtClientExtension, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtClientFaxCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtClientFaxNumber, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            'Tab 2 - Bank Details
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtBankName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSortCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAccountNumber, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAccountName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtBranch, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAreaCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtNumber, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtExtension, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFaxAreaCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFaxNumber, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboCopyFromBank, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtBIC, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtIBAN, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            'Tab 3 - Credit Card Details
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCardNo, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtExpiryDate, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtIssueNo, lFieldType:=gPMConstants.PMEDataType.PMInteger, lFormat:=gPMConstants.PMEFormatStyle.PMFormatStringCase, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboCopyFromCard, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCardName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCardStartDate, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPin, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            'TR - 24/03/03 - TS17 Recovery By Instalments changes
            'Tab 0
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboStatementFrequency, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOriginalDebt, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            'DD 31/07/2003: Added for Premium Finance
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCompanyReg, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            'DD 31/07/2003: Added for Premium Finance
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDateOfBirth, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateShort, lFieldType:=gPMConstants.PMEDataType.PMDate, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            'DD 14/11/2003: Added for Premium Finance
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPFReference, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            'PN12594 Added for Premium Finance
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboBusinessCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSelectedTotal, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            Return gPMConstants.PMEReturnCode.PMTrue
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    ' Description: Sets all of the interface default values.
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the status of the Navigate button.
            Select Case (m_lNavigate)
                Case gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = True

                Case gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled

                    cmdNavigate.Visible = False
                    cmdNavigate.Enabled = False

                Case Else
                    cmdNavigate.Visible = False
            End Select

            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
            cmdSave.Visible = True
            cmdSave.Enabled = True
            cmdExit.Visible = True
            cmdExit.Enabled = True

            txtStartDate.Text = CStr(DateTime.Today)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            cboCancelReason.FirstItem = "None"
            cboCancelReason.Enabled = False

            'Start (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)
            txtTrackingNumber.Enabled = False
            If m_bIsExternalCreditCardProcessing Then
                txtTrackingNumber.Visible = True
                lblTrackingNumber.Visible = True
            Else
                txtTrackingNumber.Visible = False
                lblTrackingNumber.Visible = False
            End If
            'End (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetFirstLastControls
    '
    ' Description: Sets the first and last data entry controls for
    '              each tab to the control array, for use with the
    '              keyboard navigation.
    '
    ' ***************************************************************** '
    Private Function SetFirstLastControls() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            ReDim m_ctlTabFirstLast(1, 0)

            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' {* USER DEFINED CODE (Begin) *}

            ' Display all language specific captions.
            'Tab1

            lblStartDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACStartDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblDaysDelay.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDaysDelay, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblNumberOfInstalments.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNumberOfInstalments, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblCostOfProtection.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCostOfProtection, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblDeposit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeposit, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblFirstInstalmentValue.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFirstInstalment, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblOtherInstalments.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOtherInstalments, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblFinanceAmount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACGrossAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblAPR.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAPR, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblInterestRate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterestRate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblTotalAmount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNetAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'TR - 24/03/03 - TS17 Recovery By Instalments changes

            fraDates.Text = CStr(iPMFunc.GetResData(g_iLanguageID, ACTabDates, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            fraSummary.Text = CStr(iPMFunc.GetResData(g_iLanguageID, ACFraSummary, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblCreateDate.Text = CStr(iPMFunc.GetResData(g_iLanguageID, ACCreatedDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblModifiedDate.Text = CStr(iPMFunc.GetResData(g_iLanguageID, ACModifiedDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblConfirmedDate.Text = CStr(iPMFunc.GetResData(g_iLanguageID, ACConfirmedDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblReviewedDate.Text = CStr(iPMFunc.GetResData(g_iLanguageID, ACReviewDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            chkNoStatements.Text = CStr(iPMFunc.GetResData(g_iLanguageID, ACOptNoStatements, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblStatementFrequency.Text = CStr(iPMFunc.GetResData(g_iLanguageID, ACStmtFrequency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblOriginalDebt.Text = CStr(iPMFunc.GetResData(g_iLanguageID, ACOriginalDebt, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'Tab2

            lblClientName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClientName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblClientAreaCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClientAreaCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblClientNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClientNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblClientExtension.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClientExtension, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblClientTelephone.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClientTelephone, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblClientFax.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClientFax, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'Tab3

            lblBankName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBankName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblSortCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSortCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblAccountNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAccountNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblBranch.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBranch, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblAccountName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAccountName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblAreaCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAreaCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblExtension.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACExtension, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblBankPhone.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBankTelephone, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblBankFax.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBankFax, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'TR - 24/03/03 - TS17 Recovery By Instalments changes

            fraAgentDetails.Text = CStr(iPMFunc.GetResData(g_iLanguageID, ACAgentfraAgentInfo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblAgentRef.Text = CStr(iPMFunc.GetResData(g_iLanguageID, ACAgentRef, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdAgentSelect.Text = CStr(iPMFunc.GetResData(g_iLanguageID, ACAgent, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            uctPMAgentAddressControl.CaptionAddress1 = CStr(iPMFunc.GetResData(g_iLanguageID, ACAgentAddress, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            uctPMAgentAddressControl.CaptionPostCode = CStr(iPMFunc.GetResData(g_iLanguageID, ACAgentPostcode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            uctPMAgentAddressControl.CaptionCountry = CStr(iPMFunc.GetResData(g_iLanguageID, ACAgentCountry, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblAgentTelNo.Text = CStr(iPMFunc.GetResData(g_iLanguageID, ACAgentTelephone, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblAgentFaxNo.Text = CStr(iPMFunc.GetResData(g_iLanguageID, ACAgentFaxNo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblAgentAreaCode.Text = CStr(iPMFunc.GetResData(g_iLanguageID, ACAgentAreaCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblAgentNumber.Text = CStr(iPMFunc.GetResData(g_iLanguageID, ACAgentNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblAgentExtension.Text = CStr(iPMFunc.GetResData(g_iLanguageID, ACAgentExtension, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'TR - 24/03/03 - TS17 Recovery By Instalments changes

            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(g_iLanguageID, ACTabTitle1, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            SSTabHelper.SetTabCaption(tabMainTab, 1, iPMFunc.GetResData(g_iLanguageID, ACTabTitle2, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            SSTabHelper.SetTabCaption(tabMainTab, 2, iPMFunc.GetResData(g_iLanguageID, ACTabTitle3, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            SSTabHelper.SetTabCaption(tabMainTab, 3, iPMFunc.GetResData(g_iLanguageID, ACTabTitle4, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            SSTabHelper.SetTabCaption(tabMainTab, 4, iPMFunc.GetResData(g_iLanguageID, ACTabTitle5, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            SSTabHelper.SetTabCaption(tabMainTab, 5, iPMFunc.GetResData(g_iLanguageID, ACTabTitle6, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdTransact.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTransactButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdRePrint.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReprintButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdReSend.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReSendButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdSave.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSaveButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdMTA.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMTAButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdExit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACExitButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdCancelPolicy.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelPolicyButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'ends
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: DisplayMessage
    '
    ' Description: To display data validation error message.
    '
    ' History: 01/06/2000 IAC - Created.
    '
    ' ***************************************************************** '
    Private Sub DisplayMessage(ByRef sMsg As String)

        Try

            MsgBoxSirius(sMsg, MsgBoxStyle.Critical)

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayMessage Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayMessage", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    ' Added for PN 12594
    ' ***************************************************************** '
    Private Function DisplayLookupDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            m_lReturn = GetBusinessLookupValues()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Get all of the lookup details.

            ' {* USER DEFINED CODE (Begin) *}

            m_lReturn = GetBusinessLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupPFBusinessCode, ctlLookup:=cboBusinessCode)

            ' Populate cboCardType dropdown
            m_lReturn = GetBusinessLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupTypeofCard, ctlLookup:=cboCardType)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetBusinessLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    ' Added for PN12594
    ' ***************************************************************** '
    Private Function GetBusinessLookupValues() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Gets all of the lookup values.

            ' Check the task.

            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Get all of the lookup values.

                    m_lReturn = g_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Get all of the lookup values with the correct
                    ' effective date.

                    m_lReturn = g_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMView
                    ' Get lookup values for viewing only.

                    m_lReturn = g_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
            End Select

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusinessLookupValues")

                Return result
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusinessLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetBusinessLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    ' Added for PN12594
    ' ***************************************************************** '
    'Private Function GetBusinessLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control) As Integer
    Private Function GetBusinessLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As ComboBox) As Integer

        Dim result As Integer = 0
        Dim lRow As Integer
        Dim bFoundMatch As Boolean

        ' Lookup value contants.
        Const ACValueTableName As Integer = 0
        Const ACValueID As Integer = 1
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3

        ' Lookup detail contants.
        Const ACDetailKey As Integer = 0
        Const ACDetailDesc As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            bFoundMatch = False

            For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
                ' Check for a match of the table name.
                If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
                    ' Found a match
                    bFoundMatch = True
                    Exit For
                End If
            Next lRow

            ' Check if there has been a table match.
            If Not bFoundMatch Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusinessLookupDetails")

                Return result
            End If

            'DJM 26/04/2002 : Clear the control first. This prevents duplicates in
            '                 the comboboxes when the apply button in clicked.

            'ctlLookup.Clear()
            ctlLookup.Items.Clear()

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.
            For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
                ' Add the details to the control.

                Dim listIndex As Integer = ctlLookup.Items.Add(New VB6.ListBoxItem(m_vLookupDetails(ACDetailDesc, lCntr), CInt(m_vLookupDetails(ACDetailKey, lCntr))))
                ' Check if this is the selected index.
                If CStr(m_vLookupValues(ACValueID, lRow)) <> "" Then
                    If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then

                        ctlLookup.SelectedIndex = listIndex
                    End If
                End If
            Next lCntr

            ' Check if the selected index is blank. If so,
            ' we set the controls index to minus 1.
            'It used to be 0, but empty tables caused an error
            If CStr(m_vLookupValues(ACValueID, lRow)) = "" Then

                '
                ctlLookup.SelectedIndex = -1
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusinessLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '*************************************************************************
    ' Name:         ValidateDataEntry
    ' Description:  Validate mandatory fields contain data
    ' History:      01/06/2000 IAC - Created.
    ' RAM20030404   : Issue 2915 Changes
    '               TR - 24/03/03 - TS17 Recovery By Instalments changes
    '*************************************************************************
    Private Function ValidateDataEntry() As Integer

        Dim result As Integer = 0
        Const Valid As String = "0123456789"

        Dim sMsg As String = ""
        Dim enMsgBoxResult As DialogResult
        Dim vOriginalPlan As Object = Nothing
        Dim bStatementChanged As Boolean
        Dim sEventMessage As String = ""
        Dim vTaxArray(,) As Object = Nothing
        Dim iFlag As gPMConstants.PMEReturnCode
        Dim sTaxCode As String = ""
        Dim sMissingTaxCode As New StringBuilder
        Dim bPlanExists As Boolean
        Dim lTaxAccountID As Integer
        Dim sCountryCode As String = String.Empty

        Const k_sFUNCTION_NAME As String = "ValidateDataEntry"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Conversion.Val(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanTaxGroupID, 0))) > 0 Then

                m_lReturn = g_oBusiness.GetTaxBandsByTaxGroup(v_sTaxGroup:=m_vFinancePlanArray(bSIRPremFinConst.k_PfPlanTaxGroupID, 0), r_vTaxBands:=vTaxArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Information.IsArray(vTaxArray) Then
                    iFlag = gPMConstants.PMEReturnCode.PMFalse

                    For iTaxPtr As Integer = 0 To vTaxArray.GetUpperBound(1)
                        lTaxAccountID = gPMFunctions.ToSafeLong(vTaxArray(0, iTaxPtr))

                        sTaxCode = CStr(vTaxArray(1, iTaxPtr))
                        If lTaxAccountID = 0 Then
                            sMissingTaxCode.Append(sTaxCode & Environment.NewLine)
                            iFlag = gPMConstants.PMEReturnCode.PMTrue
                        End If
                    Next iTaxPtr

                    If iFlag = gPMConstants.PMEReturnCode.PMTrue Then
                        MsgBoxSirius("Following Tax Accounts are Missing for This Tax Posting." & Environment.NewLine & sMissingTaxCode.ToString() & " Please create using Account Explorer", MsgBoxStyle.Information, "Tax")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

            End If

            ' For in-house only, check start date
            If m_lSchemeTypeID <> bSIRPremFinConst.PFThirdParty Then
                Select Case m_sStatusInd
                    Case bSIRPremFinConst.PFStatusIndSaved, bSIRPremFinConst.PFStatusIndUpdated, bSIRPremFinConst.PFStatusIndQuotePrinted
                        ' Check the start date, it must be at least 10 days ahead
                        If Information.IsDate(txtStartDate.Text) Then
                            'Thinh Nguyen 18/02/2002 (start)
                            'If DateDiff("D", Date, txtStartDate) < PFDirectDebitDelay Then

                            ' RAM20030404 : Issue 2915 Changes
                            'If m_bUseExistingBankDetails Or m_bUseExistingCreditCardDetails Then
                            ' Note : If we are using the existing bank or credit card details,
                            '         then we don't need to worry about the default Direct Debit Delay days
                            'Else
                            ' AAB - 05-22-03 - based on Danny Davis advise this code is no longer needed.
                            '                        If DateDiff("D", Date, m_oFormFields.UnformatControl(txtStartDate)) < PFDirectDebitDelay Then
                            '                        'Thinh Nguyen 18/02/2002 (end)
                            '                            DisplayMessage "Start Date Cannot Be Within " & PFDirectDebitDelay & " Days."
                            '                            SelectControl txtStartDate, tabMainTab, 0
                            '                            ValidateDataEntry = PMFalse
                            '                            Exit Function
                            '                        End If
                            'End If
                        Else
                            DisplayMessage("Must Enter Valid Start Date")
                            SelectControl(txtStartDate, tabMainTab, 0)
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    Case Else
                        ' The date does not matter
                End Select
            End If

            'TR - 24/03/03 - TS17 Recovery By Instalments changes
            'TR - Validation for Recovery By Instalments ONLY
            If m_lSchemeTypeID = bSIRPremFinConst.PFThirdPartyRecovery Then
                'Validate the Review Date

                If dtpReviewDate.Value.ToString() = "" Then
                    'TR - Review Date must be at least 7 days from today
                    If dtpReviewDate.Value < DateTime.Now.AddDays(7) Then
                        DisplayMessage("The Review Date must be at least 7 days from today")
                        SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                        dtpReviewDate.Focus()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    'TR - Review Date "should" be more than a year from the previous
                    'Review Date
                    If m_dtOriginalReviewDate <> CDate("00:00:00") Then
                        If dtpReviewDate.Value < m_dtOriginalReviewDate.AddYears(1) Then
                            'TR - Ask the USer if they are sure about this
                            enMsgBoxResult = MsgBoxSirius("This Review Date is greater " & "than a year from the previous." & Strings.Chr(13) & Strings.Chr(10) & "Are " & "you sure?", MsgBoxStyle.YesNo, "Review Date")
                            If enMsgBoxResult <> System.Windows.Forms.DialogResult.Yes Then
                                SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                                dtpReviewDate.Focus()
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                        End If
                    End If
                Else
                    'TR - If the Plan is Live, then the Date_review cannot be blank
                    If m_sStatusInd = bSIRPremFinConst.PFStatusIndLive Then

                        If Convert.IsDBNull(dtpReviewDate.Value) Or IsNothing(dtpReviewDate.Value) Then
                            DisplayMessage("The Review Date cannot be blank for a Live policy")
                            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                            dtpReviewDate.Focus()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                End If

                'TR - Validate the Statment PFFrequency
                'TR - Is the Option to Have Statements selected?
                If chkNoStatements.CheckState = CheckState.Checked Then
                    'TR - See if the Statements option was off originally
                    If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanNoStatements, 0)) <> "1" Then
                        bStatementChanged = True
                        sEventMessage = "Statements have been switched off"
                    End If
                Else
                    'TR - See if the Statements option was on originally
                    If CStr(Conversion.Val(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanNoStatements, 0)))) <> "0" Then
                        bStatementChanged = True
                        sEventMessage = "Statements have been switched on."
                    End If

                    'TR - Has the Statement Frequency changed?
                    If cboStatementFrequency.SelectedIndex <> -1 Then
                        If VB6.GetItemData(cboStatementFrequency, cboStatementFrequency.SelectedIndex) <> Conversion.Val(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStatementPFFrequencyID, 0))) Then
                            bStatementChanged = True
                            sEventMessage = sEventMessage & " Statement Frequency " & "ID Changed from " & m_sOriginalStatementFrequency & " to " & cboStatementFrequency.Text
                        End If
                    Else
                        If Strings.Len(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStatementPFFrequencyID, 0))) <> 0 Then
                            bStatementChanged = True
                            sEventMessage = sEventMessage & " Statement Frequency " & "ID Changed from " & m_sOriginalStatementFrequency & " to none selected"
                        End If
                    End If
                End If

                'TR - Now write any events and do further validation for Changed
                'Statement options
                If bStatementChanged Then

                    'TR - Create the Event Log object
                    'Data Migration Issue No. 2490 JP 03/01/2012
                    If sEventMessage Is Nothing Then sEventMessage = ""
                    sEventMessage = "Plan Ref : " & txtReference.Text & ". " & sEventMessage
                    CreateEventLogEntry(sEventMessage)

                    'TR - if we're creating a new version of a Plan, then set the
                    'LastStatement Date.
                    If m_lFinancePlanVersion > 1 Then
                        'TR - Get the ORIGINAL Premium Finance Plan

                        m_lReturn = g_oBusiness.GetSingleFinancePlan(m_lFinancePlanCnt, 1, vOriginalPlan)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(m_vFinancePlanArray) Then
                            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to get original " & "version of this Plan", ACApp, ACClass, k_sFUNCTION_NAME)
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        'TR - Get the Last Statement Date from the Original plan and
                        'save ot to the current
                        If Information.IsDate(vOriginalPlan(bSIRPremFinConst.k_PFPlanDateLastStatement, 0)) Then

                            m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDateLastStatement, 0) = vOriginalPlan(bSIRPremFinConst.k_PFPlanDateLastStatement, 0)
                        End If
                        'TR - Else, if this existing pLan is LIVE, set last Statement
                        'date as today
                    ElseIf m_sStatusInd = bSIRPremFinConst.PFStatusIndLive Then
                        m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDateLastStatement, 0) = DateTime.Now.ToString("d")
                    End If
                End If

            End If

            'PN 21891 check account number and sort code to be numeric
            'Moved validation code from the validate event to the ValidateDataEntry Method
            'because while changing the tabs validate event is not generated.
            '-- start
            For iCount As Integer = 1 To Strings.Len(txtSortCode.Text)
                If (Valid.IndexOf(Mid(txtSortCode.Text.Trim(), iCount, 1)) + 1) = 0 Then
                    MsgBoxSirius("Sort Code can only be made up of numbers.", MsgBoxStyle.Exclamation, "Invalid Sort Code")
                    SSTabHelper.SetSelectedIndex(tabMainTab, 2)
                    If txtSortCode.Enabled Then
                        txtSortCode.Focus()
                        txtSortCode.SelectionStart = 0
                        txtSortCode.SelectionLength = Strings.Len(txtSortCode.Text)
                    End If
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Next iCount

            Dim iSourceId As Integer
            If CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanSource_ID, 0)) = 0 Then
                iSourceId = g_iSourceID
            Else
                iSourceId = CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanSource_ID, 0))
            End If

            m_lReturn = g_oBusiness.GetSourceCountryCode(v_lSourceID:=iSourceId, r_sCountryCode:=sCountryCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ValidateDataEntry = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            'PN10301.  Validate SortCode to 6 chars for UK.
            If (txtSortCode.Text.Trim().Length > 0) And (Trim(sCountryCode) = "GBR" Or Trim(sCountryCode) = "RSA") Then
                If txtSortCode.Text.Trim().Length <> 6 Then
                    SSTabHelper.SetSelectedIndex(tabMainTab, 2)
                    If txtSortCode.Enabled Then
                        txtSortCode.Focus()
                    End If
                    txtSortCode.SelectionStart = 0
                    txtSortCode.SelectionLength = Strings.Len(txtSortCode.Text)
                    MsgBoxSirius("Sort Code Invalid", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "Invalid Sort Code")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            For iCount As Integer = 1 To Strings.Len(txtAccountNumber.Text)
                If (Valid.IndexOf(Mid(txtAccountNumber.Text.Trim(), iCount, 1)) + 1) = 0 Then
                    MsgBoxSirius("Account Number can only be made up of numbers.", MsgBoxStyle.Exclamation, "Invalid Account Number")
                    SSTabHelper.SetSelectedIndex(tabMainTab, 2)
                    If txtAccountNumber.Enabled Then
                        txtAccountNumber.Focus()
                    End If
                    txtAccountNumber.SelectionStart = 0
                    txtAccountNumber.SelectionLength = Strings.Len(txtAccountNumber.Text)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Next iCount
            '-- end
            If ValidateAccountNumber() = gPMConstants.PMEReturnCode.PMFalse Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If m_lSilentTransact <> 1 Then
                'TR - Validate all the normal mandatory controls
                result = m_oFormFields.CheckMandatoryControls()
                'PN12445
            End If

            If m_BankMandatory Then
                If addBank.AddressLine1 = "" Or addBank.PostCode = "" Then
                    MsgBoxSirius("You must enter the first line of the bank address, " & "and the postcode", MsgBoxStyle.Exclamation, "Mandatory Bank Details")
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanProductClass, 0)).Trim() = "SG" Then
                If m_bRequireSGCompanyDetails Then
                    If chkPartnership.CheckState Then
                        If txtCompanyReg.Text.Trim() = "" And txtCompanyReg.Visible Then
                            MsgBoxSirius("You must enter a valid company registration number", MsgBoxStyle.Exclamation, "Company Reg No")
                            SSTabHelper.SetSelectedIndex(tabMainTab, 1)
                            txtCompanyReg.Focus()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Else
                        If lvwPartners.Items.Count = 0 Then
                            MsgBoxSirius("You must provide details of the partners in your business", MsgBoxStyle.Exclamation, "Partners")
                            SSTabHelper.SetSelectedIndex(tabMainTab, 1)
                            cmdAddPartner.Focus()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                End If

                'now check if the refund type has been entered
                'If m_bRequireRefundType Then

                'ACR 11-09-07 changed as refund type is currently not showing up as mandatory..
                If cboRefundType.Visible Then
                    If cboRefundType.Text.Trim() = "" Then
                        MessageBox.Show("You must choose a policy refund type", "Refund Type", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        SSTabHelper.SetSelectedIndex(tabMainTab, 1)
                        cboRefundType.Focus()

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                If cboBusinessCode.Visible Then
                    If CBool(CStr(cboBusinessCode.Text = "").Trim()) Then
                        MessageBox.Show("You must choose a business code", "Business Code", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        SSTabHelper.SetSelectedIndex(tabMainTab, 1)
                        cboBusinessCode.Focus()

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                If gPMFunctions.ToSafeDouble(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDeposit, 0)) > 0 Then
                    If m_bMandatoryCCDetails Then
                        If txtCardNo.Text.Trim() = "" Or txtExpiryDate.Text.Trim() = "" Or txtStartDate.Text.Trim() = "" Or cboCardType.Text.Trim() = "" Then

                            MsgBoxSirius("You must provide details of the credit card that the provider will collect the deposit from", MsgBoxStyle.Information, "Credit card details")

                            SSTabHelper.SetSelectedIndex(tabMainTab, 3)
                            Return gPMConstants.PMEReturnCode.PMFalse
                        Else
                            If chkCardholder.CheckState Then
                                If txtCardName.Text.Trim() = "" Then
                                    MsgBoxSirius("You must provide details of the credit card that the provider will collect the deposit from", MsgBoxStyle.Information, "Credit card details")

                                    SSTabHelper.SetSelectedIndex(tabMainTab, 3)
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If
                            Else
                                If txtCardholderName.Text.Trim() = "" Or addCardholder.AddressLine1.Trim() = "" Or addCardholder.PostCode.Trim() = "" Then

                                    MsgBoxSirius("You must provide the credit card holders details to continue", MsgBoxStyle.Information, "Third party credit card")

                                    SSTabHelper.SetSelectedIndex(tabMainTab, 3)
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If
                            End If
                        End If
                    End If
                End If
            End If

            'PN#43383 Parallel fix- copy of PN#42864
            m_bReferenceExist = False
            'Start Renuka PN 61485
            'If (Trim(m_vFinancePlanArray(k_PFPlanAutoGenPlanRef, 0)) <> txtReference.Text)  Then
            If (CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAutoGenPlanRef, 0)).Trim() <> txtReference.Text) And txtReference.Text.Trim().Length > 0 Then
                'End Renuka PN 61485

                m_lReturn = g_oBusiness.CheckForDuplicatePlanReference(v_sPlanReference:=txtReference.Text, r_bPlanExists:=bPlanExists)
                If bPlanExists Then
                    MessageBox.Show("Plan Reference already exists.  ", "Duplicate Plan Reference", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    m_bReferenceExist = True
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' only raise the messages when we have successfully passed all other validation checks
            If result = gPMConstants.PMEReturnCode.PMTrue Then
                '****************************

                If chkDDCancelled.CheckState = CheckState.Checked AndAlso Not (m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDDCancelled, 0)) Is Nothing AndAlso gPMFunctions.ToSafeDouble(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDDCancelled, 0), 0) <> 1 Then

                    ' direct debit cancelled
                    If MessageBox.Show("The direct debit agreement has been cancelled, if you continue the plan will be put on hold. Do you wish to continue?", "Direct Debit Agreement Cancelled", MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.Yes Then
                        m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStatusInd, 0) = bSIRPremFinConst.PFStatusIndOnHold
                    Else
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                ElseIf (chkDDCancelled.CheckState = CheckState.Unchecked And gPMFunctions.ToSafeInteger(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDDCancelled, 0), 0) <> 0) Then

                    ' direct debit reinstated
                    If MessageBox.Show("The direct debit agreement has been reinstated, if you continue the plan will be made live. Do you wish to continue?", "Direct Debit Agreement Reinstated", MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.Yes Then
                        m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStatusInd, 0) = bSIRPremFinConst.PFStatusIndLive
                        If gPMFunctions.ToSafeDate(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDateModified, 0)) <> DateTime.Today Then
                            m_bDDReinstated = True
                        End If
                    Else
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

                If chkCCCancelled.CheckState = CheckState.Checked AndAlso Not (m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCCancelled, 0)) Is Nothing AndAlso gPMFunctions.ToSafeDouble(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCCancelled, 0), 0) <> 1 Then
                    ' credit card cancelled

                    ' direct debit cancelled
                    If MessageBox.Show("The credit card agreement has been cancelled, if you continue the plan will be put on hold. Do you wish to continue?", "Credit Card Agreement Cancelled", MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.Yes Then
                        m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStatusInd, 0) = bSIRPremFinConst.PFStatusIndOnHold
                    Else
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                ElseIf (chkCCCancelled.CheckState = CheckState.Unchecked And gPMFunctions.ToSafeInteger(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCCCancelled, 0), 0) <> 0) Then

                    ' direct debit cancelled
                    If MessageBox.Show("The credit card agreement has been reinstated, if you continue the plan will be made live. Do you wish to continue?", "Credit Card Agreement Reinstated", MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.Yes Then
                        m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStatusInd, 0) = bSIRPremFinConst.PFStatusIndLive
                    Else
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If

            '    "The direct debit agreement has been cancelled, if you continue the plan will be put on hold"
            '    "The direct debit agreement has been reinstated, if you continue the plan will be made live"
            '    "The credit card agreement has been cancelled, if you continue the plan will be put on hold"
            '    "The credit card agreement has been reinstated, if you continue the plan will be made live"

            '****************************

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateDataEntry Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=k_sFUNCTION_NAME, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: DisableInterface
    '
    ' Description: Disables parts of the interface while a search is
    '              in progress.
    '
    ' ***************************************************************** '
    Private Function DisableInterface(ByRef bDisable As Boolean) As Integer

        Dim result As Integer = 0
        Try

            'If we're here we're searching.  Disable it until an item is clicked.

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisableForm
    '
    ' Description: Disables parts of the interface while a search is
    '              in progress.
    '
    ' ***************************************************************** '
    Private Function DisableForm(ByRef bDisable As Boolean) As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'If we're here we're searching.  Disable it until an item is clicked.

            For Each ctlFormControl As Control In ContainerHelper.Controls(Me)
                ' Check the type of the control.
                If TypeOf ctlFormControl Is TextBox Then
                    ControlHelper.SetEnabled(ctlFormControl, Not bDisable)
                ElseIf (TypeOf ctlFormControl Is ComboBox) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not bDisable)
                ElseIf (TypeOf ctlFormControl Is CheckBox) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not bDisable)
                End If
            Next ctlFormControl
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ResizeInterface
    '
    ' Description: Resizes the interface controls.
    ' ***************************************************************** '
    Private Function ResizeInterface() As Integer

        Try

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch

            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    Private Sub addBank_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles addBank.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        If m_BankMandatory Then
            If addBank.AddressLine1 = "" Or addBank.PostCode = "" Then
                MsgBoxSirius("You must enter the first line of the bank address, " & "and the postcode", MsgBoxStyle.Exclamation, "Mandatory Bank Details")
            End If
        End If
        eventArgs.Cancel = Cancel
    End Sub

    Private Sub cboCopyFromBank_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCopyFromBank.SelectionChangeCommitted

        With cboCopyFromBank

            txtBankName.Text = CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankName, .SelectedIndex)).Trim()

            txtSortCode.Text = CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankSortCode, .SelectedIndex)).Trim()

            txtAccountNumber.Text = CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankAccountNo, .SelectedIndex)).Trim()
            txtBIC.Text = CStr(m_vBankAccountDetails(bSIRPremFinConst.kBIC, .SelectedIndex)).Trim()
            txtIBAN.Text = CStr(m_vBankAccountDetails(bSIRPremFinConst.kIBAN, .SelectedIndex)).Trim()

            txtAccountName.Text = CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankAccountName, .SelectedIndex)).Trim()

            txtBranch.Text = CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankBranch, .SelectedIndex)).Trim()

            addBank.AddressLine1 = CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankAddress1, .SelectedIndex)).Trim()

            addBank.AddressLine2 = CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankAddress2, .SelectedIndex)).Trim()

            addBank.AddressLine3 = CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankAddress3, .SelectedIndex)).Trim()

            addBank.AddressLine4 = CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankAddress4, .SelectedIndex)).Trim()

            addBank.PostCode = CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankPostCode, .SelectedIndex)).Trim()

            If CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankCountry_ID, .SelectedIndex)) <> "" Then

                addBank.CountryId = CInt(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankCountry_ID, .SelectedIndex))
            End If

            txtAreaCode.Text = CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankAreaCode, .SelectedIndex)).Trim()

            txtNumber.Text = CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankPhone, .SelectedIndex)).Trim()

            txtExtension.Text = CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankExtn, .SelectedIndex)).Trim()

            txtFaxAreaCode.Text = CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankFaxCode, .SelectedIndex)).Trim()

            txtFaxNumber.Text = CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankFax, .SelectedIndex)).Trim()
        End With
    End Sub
    Private Sub cboCopyFromCard_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCopyFromCard.SelectionChangeCommitted
        With cboCopyFromCard

            m_sCreditCardNo = CStr(m_vCreditCardDetails(bSIRPremFinConst.k_PFPlanClientcc_number, .SelectedIndex)).Trim()
            txtCardNo.Text = cboCopyFromCard.Text

            txtCardName.Text = CStr(m_vCreditCardDetails(bSIRPremFinConst.k_PFPlanClientCreditCardName, .SelectedIndex)).Trim()

            txtExpiryDate.Text = CStr(m_vCreditCardDetails(bSIRPremFinConst.k_PFPlanClientcc_expiry_date, .SelectedIndex)).Trim()

            txtCardStartDate.Text = CStr(m_vCreditCardDetails(bSIRPremFinConst.k_PFPlanClientcc_start_date, .SelectedIndex)).Trim()

            txtIssueNo.Text = CStr(m_vCreditCardDetails(bSIRPremFinConst.k_PFPlanClientcc_issue, .SelectedIndex)).Trim()

            txtPin.Text = CStr(m_vCreditCardDetails(bSIRPremFinConst.k_PFPlanClientcc_pin, .SelectedIndex)).Trim()

            txtCardName.Text = CStr(m_vCreditCardDetails(bSIRPremFinConst.k_PFPlanClientCreditCardName, .SelectedIndex)).Trim()

            ' we need to perform the following validation
            ' as we are loading value from the db and these could be corrupt
            txtCardNo_Validating(txtCardNo, New CancelEventArgs(False))
            txtExpiryDate_Validating(txtExpiryDate, New CancelEventArgs(False))
            txtCardStartDate_Validating(txtCardStartDate, New CancelEventArgs(False))
            '**************
        End With
    End Sub

    Private Sub chkCardholder_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkCardholder.CheckStateChanged
        Frame2.Visible = Not (chkCardholder.Checked)
        txtCardName.Enabled = chkCardholder.Checked
        lblCardName.Enabled = txtCardName.Enabled
    End Sub

    Private Sub chkPartnership_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkPartnership.CheckStateChanged
        Select Case chkPartnership.CheckState
            Case True
                addClient.Width = VB6.TwipsToPixelsX(8505)
                lblCompanyReg.Visible = True
                txtCompanyReg.Visible = True
                m_oFormFields.Item("txtCompanyReg-0").IsMandatory = True
                m_bRequireCompanyReg = True
                frmPartners.Visible = False
            Case False
                addClient.Width = VB6.TwipsToPixelsX(4575)
                lblCompanyReg.Visible = False
                txtCompanyReg.Visible = False
                m_oFormFields.Item("txtCompanyReg-0").IsMandatory = False
                m_bRequireCompanyReg = False
                frmPartners.Visible = True
        End Select
    End Sub

    Private Sub cmdAddPartner_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddPartner.Click
        Dim lUpper As Integer

        Dim frmAP As New frmAddParty

        With frmAP
            .ShowDialog()

            lUpper = -1
            Try
                lUpper = m_vPartnerArray.GetUpperBound(1)

            Catch
            End Try

            If lUpper = -1 Then
                ReDim m_vPartnerArray(5, 0)
                lUpper = 0
            Else
                lUpper = m_vPartnerArray.GetUpperBound(1) + 1
                ReDim Preserve m_vPartnerArray(5, lUpper)
            End If

            m_vPartnerArray(0, lUpper) = .FullName
            m_vPartnerArray(1, lUpper) = .Address1
            m_vPartnerArray(2, lUpper) = .Address2
            m_vPartnerArray(3, lUpper) = .Address3
            m_vPartnerArray(4, lUpper) = .Address4
            m_vPartnerArray(5, lUpper) = .PostCode

            With lvwPartners.Items.Add(.FullName.Replace(" ", ""), .FullName, "")
                ListViewHelper.GetListViewSubItem(lvwPartners.Items.Add(frmAP.FullName.Replace(" ", ""), frmAP.FullName, ""), 1).Text = m_vPartnerArray(1, lUpper)
                ListViewHelper.GetListViewSubItem(lvwPartners.Items.Add(frmAP.FullName.Replace(" ", ""), frmAP.FullName, ""), 2).Text = m_vPartnerArray(5, lUpper)
            End With
        End With
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        Dim vCredits As Object = Nothing
        Dim vDebits As Object = Nothing
        Dim vPlanCancellationTransaction As Object = Nothing
        ReDim vPlanCancellationTransaction(1, 0)
        Dim fCancelPlan As frmCancelPlanReason
        Dim vPremiumFinancePolicy(,) As Object = Nothing
        Dim nLower, nUpper As Integer
        Dim vPlanCancellation As Object = Nothing
        Dim sActionCode As String = ""

        Try
            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            fCancelPlan = New frmCancelPlanReason()

            '(RC) PLICO 9-10
            If GetOutstandingBalance() = 0 And m_lSchemeTypeID <> bSIRPremFinConst.PFThirdParty Then
                MsgBoxSirius("This Instalment plan has no outstanding balance.", MsgBoxStyle.Exclamation, "Cancel not allowed")
            Else
                'm_lReturn = MsgBoxSirius("You are about to Cancel the Finance Plan." & vbCrLf & _
                ''"Are " & _
                ''"you sure ? ", vbYesNo, "Cancel Finance Plan")
                fCancelPlan.ShowDialog()
                'If m_lReturn = vbYes Then
                If fCancelPlan.Status = gPMConstants.PMEReturnCode.PMOK Then
                    '-------PN 5436
                    cmdCancel.Enabled = False
                    cmdSave.Enabled = False
                    cmdExit.Enabled = False
                    '-------PN 5436


                    If GetOutstandingBalance() = 0 And m_lSchemeTypeID <> PFThirdParty Then
                        m_lReturn = g_oBusiness.StatusUpdate(vPremiumFinanceCnt:=m_lFinancePlanCnt,
                                         vPremiumFinanceVersion:=m_lFinancePlanVersion, vStatusInd:=PFStatusIndCancelled)
                    ElseIf m_lSchemeTypeID = bSIRPremFinConst.PFInHouse Then
                        m_vFinancePlanArray(bSIRPremFinConst.k_PFCancelReasonId, 0) = fCancelPlan.CancelReasonID
                        g_oBusiness.IsSingleInstalmentPlan = bIsSinglePlanParty
                        m_lReturn = g_oBusiness.CancelPlanInHouse(m_vFinancePlanArray, 0, vCredits, vDebits)

                        vPlanCancellationTransaction(0, 0) = vCredits

                        vPlanCancellationTransaction(1, 0) = vDebits

                        m_lReturn = g_oBusiness.SavePlanCancellationTransactions(m_lFinancePlanCnt, m_lFinancePlanVersion, vPlanCancellationTransaction)
                        If bIsSinglePlanParty Then
                            SetCommand(cmdCancel, True, False)
                            SetCommand(cmdCancelPolicy, True, True)
                        End If
                    Else

                        '(RC) PLICO 9-10
                        'm_lReturn& = g_oBusiness.StatusUpdate(vPremiumFinanceCnt:=m_lFinancePlanCnt, _
                        'vPremiumFinanceVersion:=m_lFinancePlanVersion, vStatusInd:=PFStatusIndCancelled)

                        m_lReturn = g_oBusiness.CancelPlanThirdParty(m_vFinancePlanArray, 0)

                        m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStatusInd, 0) = bSIRPremFinConst.PFStatusIndCancelled
                        m_lReturn = g_oBusiness.UpdateExistingRecord(m_vFinancePlanArray, m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFCnt, 0), m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFVersion, 0))
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("cmdCancel_Click", "UpdateExistingRecord" & " Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If
                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                        'DC150704 PN13181 only produce edi message for third party
                        If m_lSchemeTypeID = bSIRPremFinConst.PFThirdParty Then
                            'Steve Watton - send an EDI message to indcate that the plan has been cancelled
                            'PN Issue 12141 07/06/2004.

                            m_lReturn = g_oBusiness.ProduceEDIMessage(vPremFinanceArray:=m_vFinancePlanArray, vReTransmit:="", bIsCancellation:=True, bIsNonTransactionalMTA:=False)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                MsgBoxSirius("An Error occured sending the EDI message", MsgBoxStyle.OkOnly, "Cancel " &
                                             "Finance Plan")
                            End If

                            ''''' End of Steve Watton 07/06/2004
                        ElseIf (m_lSchemeTypeID = bSIRPremFinConst.PFThirdPartyViaStargate) Then

                            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanXSLCode, 0)) = "PCLPSG" Then
                                'Steve Watton - send an EDI message to indcate that the plan has been cancelled
                                'PN Issue 12141 07/06/2004.

                                m_lReturn = g_oBusiness.CancelSGPlan(v_vPremFinanceArray:=m_vFinancePlanArray)

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    MsgBoxSirius("An Error occured sending the cancellation request", MsgBoxStyle.OkOnly, "Cancel " &
                                                 "Finance Plan")
                                End If
                            End If
                        End If

                        If bIsSinglePlanParty Then

                            m_lReturn = g_oBusiness.GetPremiumFinancePolicy(v_lPfPremiumFinanceCnt:=gPMFunctions.ToSafeLong(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFCnt, 0)), r_vPolicy:=vPremiumFinancePolicy)
                            If Information.IsArray(vPremiumFinancePolicy) Then

                                nLower = vPremiumFinancePolicy.GetLowerBound(1)

                                nUpper = vPremiumFinancePolicy.GetUpperBound(1)
                                For nCount As Integer = nLower To nUpper

                                    m_lReturn = g_oBusiness.CreateEvent(gPMConstants.PMEComponentAction.PMDelete, gPMFunctions.ToSafeLong(vPremiumFinancePolicy(0, nCount), 0), vPremiumFinancePolicy(1, nCount), v_sPlanRef:=m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAutoGenPlanRef, 0))
                                Next
                            End If
                        Else

                            m_lReturn = g_oBusiness.CreateEvent(gPMConstants.PMEComponentAction.PMDelete, gPMFunctions.ToSafeLong(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanInsuranceFileCnt, 0), 0), m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientId, 0), v_sPlanRef:=m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAutoGenPlanRef, 0))
                        End If

                        ' HG23072003 - Generate Documentation (Print off Instalment Plan)
                        'PN13594
                        If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanQuoteDocID, 0)) = "" And CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankDocID, 0)) = "" And CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCreditDocID, 0)) = "" Then
                        Else
                            If MsgBoxSirius("Generate Instalment Documentation?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Print Plan") = System.Windows.Forms.DialogResult.Yes Then

                                ' HG23072003 - We don't have to check the return val as all errors are
                                '              picked up and reported in the print component. Also a
                                '              failure here shouldn't rollback the whole lot.
                                m_lReturn = CreateInstalmentsDocs(v_eDocType:=PrintDocType.DocTypeInstalmentCancel)
                            End If
                        End If 'PN13594
                        If m_lSchemeTypeID = bSIRPremFinConst.PFInHouse Then
                            ' If m_bSinglePlanParty Then

                            m_lReturn = g_oBusiness.GetPlanCalculationTransactions(v_lPremiumFinanceCnt:=m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFCnt, 0), v_lPremiumFinanceVer:=m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFVersion, 0), r_vPlanCancellationTransaction:=vPlanCancellation)

                            m_lReturn = CancelPolicies(vPlanCancellation)
                            'End If
                        End If

                        'PN48836
                        sActionCode = ""
                        If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanMediaTypeCode, 0)).Trim().ToUpper() = "DD" Or CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanMediaTypeCode, 0)).Trim().ToUpper() = "CC" Then
                            sActionCode = kActionCodeCancellation
                        End If
                        If gPMFunctions.ToSafeString(sActionCode.Trim()) <> "" Then

                            m_lReturn = g_oBusiness.SaveInstalmentsPlanMediaTypeDetails(m_lFinancePlanCnt, m_lFinancePlanVersion, sActionCode)
                        End If

                        ' Everything OK, so we can hide the interface.
                        Me.Hide()
                    End If
                End If
            End If

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdCancelPolicy_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancelPolicy.Click
        Dim vPlanCancellationTransactions As Object = Nothing

        m_lReturn = g_oBusiness.GetPlanCalculationTransactions(v_lPremiumFinanceCnt:=m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFCnt, 0), v_lPremiumFinanceVer:=m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFVersion, 0), r_vPlanCancellationTransaction:=vPlanCancellationTransactions)

        CancelPolicies(vPlanCancellationTransactions)
        ' Everything OK, so we can hide the interface.
        Me.Hide()
    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click

        Try

            ' Warn the user of the consequences
            m_lReturn = MsgBoxSirius("This will delete the current premium finance" & Strings.Chr(13) & Strings.Chr(10) &
                        "quote and release all related transactions." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) &
                        "Continue?", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, "Premium Finance")

            If m_lReturn = System.Windows.Forms.DialogResult.Yes Then
                ' Delete the plan

                m_lReturn = g_oBusiness.DeletePlan(v_lPremiumFinanceCnt:=m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFCnt, 0), v_lPremiumFinanceVersion:=m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFVersion, 0), v_sStatusInd:=m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStatusInd, 0))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MsgBoxSirius("Unable to delete the premium finance plan", MsgBoxStyle.Critical, "Premium " &
                                 "Finance")
                    Exit Sub
                ElseIf (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) Then
                    m_lReturn =
                       g_oBusiness.UpdatePaymentMethod(v_lInsuranceFileCnt:=ToSafeLong(m_vFinancePlanArray(k_PFPlanInsuranceFileCnt, 0)))
                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                        MsgBoxSirius("Unable to update payment method", vbCritical, "Premium " &
                                "Finance")
                        Exit Sub
                    End If
                End If

                ' We've delete so we must hide
                m_lStatus = gPMConstants.PMEReturnCode.PMOK
                Me.Hide()
            End If

        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDelete_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDelete_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdDeletePartner_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeletePartner.Click
        Dim vArray(,) As String
        Dim lUpper, lOffsetX As Integer

        If Not (lvwPartners.FocusedItem Is Nothing) Then

            lUpper = m_vPartnerArray.GetUpperBound(1)

            If lUpper > 0 Then
                ReDim Preserve vArray(5, lUpper - 1)

                For x As Integer = 0 To lUpper
                    If Not (m_vPartnerArray(0, x) = lvwPartners.FocusedItem.Text) Then
                        vArray(0, lOffsetX) = m_vPartnerArray(0, x)
                        vArray(1, lOffsetX) = m_vPartnerArray(1, x)
                        vArray(2, lOffsetX) = m_vPartnerArray(2, x)
                        vArray(3, lOffsetX) = m_vPartnerArray(3, x)
                        vArray(4, lOffsetX) = m_vPartnerArray(4, x)
                        vArray(5, lOffsetX) = m_vPartnerArray(5, x)

                        lOffsetX += 1
                    Else
                        lOffsetX = x
                    End If
                Next x

                m_vPartnerArray = vArray

                lUpper = m_vPartnerArray.GetUpperBound(1)

                lvwPartners.Items.Clear()

                For x As Integer = 0 To lUpper
                    With lvwPartners.Items.Add(m_vPartnerArray(0, x).Replace(" ", ""), m_vPartnerArray(0, x), "")
                        ListViewHelper.GetListViewSubItem(lvwPartners.Items.Add(m_vPartnerArray(0, x).Replace(" ", ""), m_vPartnerArray(0, x), ""), 1).Text = m_vPartnerArray(1, x)
                        ListViewHelper.GetListViewSubItem(lvwPartners.Items.Add(m_vPartnerArray(0, x).Replace(" ", ""), m_vPartnerArray(0, x), ""), 2).Text = m_vPartnerArray(5, x)
                    End With
                Next x
            Else
                lvwPartners.Items.Clear()
                ReDim m_vPartnerArray(0)
            End If
        End If
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdExit.Click

        Try

           If ((VB6.GetItemData(cboStatus, cboStatus.SelectedIndex) = 10 OrElse VB6.GetItemData(cboStatus, cboStatus.SelectedIndex) = 11) AndAlso (m_iTask <> gPMConstants.PMEComponentAction.PMView)) Then
                RollbackPlan()
            End If

            Dim sTransType As String = IIf(m_sTransactionType IsNot Nothing, m_sTransactionType.Trim().ToUpper(), String.Empty)
            If m_iTask <> gPMConstants.PMEComponentAction.PMView AndAlso
               (sTransType = "NB" OrElse sTransType = "MTA" OrElse sTransType = "REN" OrElse sTransType = "MTR") Then
                If VB6.GetItemData(cboStatus, cboStatus.SelectedIndex) = 10 Or VB6.GetItemData(cboStatus, cboStatus.SelectedIndex) = 11 Then
                    m_lReturn = g_oBusiness.UpdatePaymentMethod(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanInsuranceFileCnt, 0))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Failed to display the inteface.
                        MsgBoxSirius("Unable to update payment method")
                        Exit Sub
                    End If
                End If
            End If

            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            m_iTask = Nothing
            Me.Hide()

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdExit_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdExit_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'Private Sub cmdHelp_Click()
    '    ' Show help
    '    m_lReturn& = ShowHelp(dlgHelp:=dlgHelp, lContextID:=ScreenHelpID)
    'End Sub

    Private Sub cmdHistory_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHistory.Click

        m_lReturn = g_oBusiness.GetFinancePlanVersions(vFinanceCnt:=m_lFinancePlanCnt, vFinancePlanArray:=m_vFinanceHistory)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to display the inteface.
            MsgBoxSirius("Unable to access finance plan versions")
            Exit Sub
        End If

        m_lReturn = ProcessHistory()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to display the inteface.
            MsgBoxSirius("Unable to view version list")
            Exit Sub
        End If

        'Open the new form
        If m_lHistoryVersion <> 0 Then
            OpenNewPlanForm(m_lFinancePlanCnt, m_lHistoryVersion, True)
        End If
        iSIRPremFinConst.m_sTransType = m_sTransactionType
    End Sub

    Private Sub cmdMTA_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdMTA.Click
        If GetOutstandingBalance() <= 0 Then
            MsgBoxSirius("This Instalment plan has no outstanding balance.", MsgBoxStyle.Exclamation, "MTA not allowed")
        Else
            ' Pass through to function
            m_lReturn = ProcessMTA()
        End If
    End Sub
    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click
        Try
            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMNavigate
            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()
            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdRelease_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRelease.Click

        Try

            ' Prompt the user to confirm
            m_lReturn = MsgBoxSirius("Release the 'Held' status on this plan?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Premium Finance")
            If m_lReturn = System.Windows.Forms.DialogResult.No Then
                Exit Sub
            End If

            ' Ask the business object to release the plan

            m_lReturn = g_oBusiness.ReleasePlan(v_lPremiumFinanceCnt:=m_lFinancePlanCnt, v_lPremiumFinanceVersion:=m_lFinancePlanVersion)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MsgBoxSirius("Failed to release the Plan")
                Throw New Exception()
            End If

            ' We got this far, we now need to refresh everything
            m_lReturn = m_oGeneral.GetInterfaceDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            ' the plan is no longer on hold
            ' enable all the locked down fields
            ' but force the user to respecify bank details
            PaymentAgreementInterfaceProcessing(False)

            m_sCreditCardNo = ""
            txtCardNo.Text = ""
            txtCardName.Text = ""
            txtExpiryDate.Text = ""
            txtCardStartDate.Text = ""
            txtIssueNo.Text = ""
            txtPin.Text = ""
            txtCardName.Text = ""
            cboCardType.Text = ""
            txtCardNo_Validating(txtCardNo, New CancelEventArgs(False))
            txtExpiryDate_Validating(txtExpiryDate, New CancelEventArgs(False))
            txtCardStartDate_Validating(txtCardStartDate, New CancelEventArgs(False))
            txtCardholderName.Text = ""

            addCardholder.AddressLine1 = ""
            addCardholder.AddressLine2 = ""
            addCardholder.AddressLine3 = ""
            addCardholder.AddressLine4 = ""
            addCardholder.PostCode = ""
            uctPartyBankCombo1.SelectedPaymentID = 0
            uctPartyBankCombo2.SelectedPaymentID = 0
            txtTrackingNumber.Text = ""
            'End (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)
            ' Set this so the roadmap is happy we have done something (I think)
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            m_bPlanReleasedFromOnHold = True
        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Release Plan button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdRelease_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub

        End Try

    End Sub

    Private Sub PaymentAgreementInterfaceProcessing(ByVal v_bDisable As Boolean)

        'If chkCCCancelled = vbChecked Or chkDDCancelled = vbChecked Then

        ' re-enable form and user controls
        DisableForm(v_bDisable)

        uctPMAgentAddressControl.Enabled = Not v_bDisable
        lvwInstalment.Enabled = Not v_bDisable
        addBank.Enabled = Not v_bDisable
        addCardholder.Enabled = Not v_bDisable

        'End If

        If chkDDCancelled.CheckState = CheckState.Checked And Not v_bDisable Then

            chkDDCancelled.CheckState = CheckState.Unchecked
            txtAccountName.Text = ""
            txtAccountNumber.Text = ""
            txtSortCode.Text = ""
            txtBIC.Text = ""
            txtIBAN.Text = ""
            cmdExit.Enabled = False

        ElseIf chkCCCancelled.CheckState = CheckState.Checked And Not v_bDisable Then

            chkCCCancelled.CheckState = CheckState.Unchecked

            txtCardNo.Text = ""
            txtExpiryDate.Text = ""
            txtCardStartDate.Text = ""

            'Start (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)
            txtTrackingNumber.Text = ""
            'End (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)

            ' clear credit card details when those are specified

            cmdExit.Enabled = False

        End If

    End Sub

    Private Sub cmdRePrint_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRePrint.Click

        Dim lMode As Integer

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' RAM20050831 - PN 22793 - Always set the flag to 0 irrespective of m_sTransType,
            '                           so that it will print all the 4 documents
            'If m_sTransType = "NB" Or m_sTransType = "REN" Then
            lMode = 0
            'Else
            '    lMode = 1
            'End If

            'see if this is a pclsg quote
            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanProductClass, 0)).Trim() = "SG" Then
                'now check if all of the required information has been entered
                If chkPartnership.CheckState Then
                    If txtCompanyReg.Text.Trim() = "" And txtCompanyReg.Visible Then
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                        MsgBoxSirius("Not all mandatory data has been entered. Please provide your company registration number", MsgBoxStyle.Information, "Print")
                        Exit Sub
                    End If
                Else
                    If lvwPartners.Items.Count = 0 Then
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                        MsgBoxSirius("Not all mandatory data has been entered. Please provide details of the main partner in your business", MsgBoxStyle.Information, "Print")
                        Exit Sub
                    End If
                End If
            End If

            m_lReturn = DocumentPrint(lMode)
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdRePrint_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdPrint_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdReSend_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdReSend.Click

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' PF280901 - We can call the ProcessPlan here for both in-house and 3rd party
            '            schemes, the function internally branches to correct code
            ' RAW 11/03/2003 : ISS2580 : removed bDeposit argument
            'MKW030204 PN10149 Changed arguments to match bSirPremiumFinance.

            m_lReturn = g_oBusiness.ProcessPlan(lMode:=2, v_vPremiumFinanceArray:=m_vFinancePlanArray, v_vTransArray:=m_vFinancePlanTransArray, v_iMTAType:=MTAType)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Resend_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdResend_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdSave_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSave.Click

        Dim lMode As Integer ' RAW 07/03/2003 : added

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'TR - Assume sccuess
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            If Not (Information.IsArray(m_vFinancePlanArray)) Then

                m_lReturn = g_oBusiness.GetSingleFinancePlan(m_lFinancePlanCnt, m_lFinancePlanVersion, m_vFinancePlanArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If
            End If

            'Validate Mandatory Fields
            m_lReturn = ValidateDataEntry()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            ' RAW 11/03/2003 : ISS2580 : added
            'm_lReturn = g_oBusiness.BeginTrans
            'If m_lReturn <> PMTrue Then
            '    SetMousePointer PMMouseReset
            '    MsgBoxSirius "Failed to start a database transaction"
            '    GoTo Err_cmdSave_Click
            'End If

            'TR - 24/03/03 - TS17 Recovery By Instalments changes
            If m_lSchemeTypeID = bSIRPremFinConst.PFThirdPartyRecovery Then
                m_lReturn = ProcessThirdPartyRecovery()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception()
                End If
            End If

            ' Process the next set of actions depending upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            ' RAW 07/03/2003 : added
            If m_sRunningContext.ToUpper() = "UWNB" Or m_sTransType = "REN" Or m_sTransType = "NB" Then
                ' called from NewBusiness roadmap
                ' Save Instalments
                If m_sCallingAppName = "iPMBFindFinancePlan" Then
                    lMode = 0
                Else
                    lMode = 5
                End If
                ' RAW 11/03/2003 : ISS2580 : removed bDeposit and lDepositTransId argument

                m_lReturn = g_oBusiness.ProcessPlan(lMode:=lMode, v_vPremiumFinanceArray:=m_vFinancePlanArray, v_vTransArray:=m_vFinancePlanTransArray, v_iMTAType:=MTAType)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' RAW 11/03/2003 : ISS2580 : added
                    MsgBoxSirius("Failed to process the plan", MsgBoxStyle.OkOnly, "Process Plan")
                    Throw New Exception()
                End If

            End If

            If iSIRPremFinConst.m_sTransType = "MTA" And Information.IsArray(m_vFinancePlanMTATransArray) AndAlso VB6.GetItemData(cboStatus, cboStatus.SelectedIndex) = 10 Then
                'TR - Do the Posting
                If m_sCallingAppName = "iPMBFindFinancePlan" Then
                    m_lReturn = g_oBusiness.PostMTA(m_vFinancePlanArray, m_oOldTransArray, m_oOldPFArray)
                    If m_lReturn = 9 Then

                        m_vFinancePlanMTATransArray = ""
                        MsgBoxSirius("The transactions you have chosen do not meet the " & "Minimum MTA amount - please Reselect.", MsgBoxStyle.Exclamation, "MTA Failed")
                    ElseIf m_lReturn = 99 Then

                        m_vFinancePlanMTATransArray = ""
                        MsgBoxSirius("There are not enough instalments left over to spread " & "the MTA.", MsgBoxStyle.Exclamation, "MTA Failed")
                    ElseIf m_lReturn = bSIRPremFinConst.k_PFPlanNoFinanceRate Then

                        m_vFinancePlanMTATransArray = ""
                        MsgBoxSirius("This Finance Scheme does not offer MTA Instalments.", MsgBoxStyle.Exclamation, "MTA Failed")
                    ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception()
                    Else
                        'Reload with the new information
                        m_lFinancePlanCnt = CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFCnt, 0))
                        m_lFinancePlanVersion = CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFVersion, 0))

                        'PSL 02/04/2003 It should be live
                        m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStatusInd, 0) = bSIRPremFinConst.PFStatusIndLive
                        'TR - Update the Premium Fianance Plan (should have a TransdetailID now)

                        m_lReturn = g_oBusiness.UpdateExistingRecord(m_vFinancePlanArray, m_lFinancePlanCnt, m_lFinancePlanVersion)
                        'TR - Make sure that this workde OK
                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                            'TR - Everything worked OK
                            'MsgBoxSirius("The MTA has been spread over the remaining instalments." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Press OK to show the new Plan.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "MTA Processed")
                            'Reload with the new information

                            m_lReturn = g_oBusiness.DeletePFTransID(v_lPremFinanceCnt:=m_lFinancePlanCnt, v_lPremFinanceVersion:=m_lFinancePlanVersion)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                MsgBoxSirius("Failed to process DeletePFTransID()", MsgBoxStyle.OkOnly, "cmdSave_Click()")
                                Throw New Exception()
                            End If
                            m_lReturn = g_oBusiness.InsertPFTransID(v_vPFTransArray:=m_vFinancePlanMTATransArray, v_lPremFinanceCnt:=m_lFinancePlanCnt, v_lPremFinanceVersion:=m_lFinancePlanVersion)

                            m_lFinancePlanCnt = CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFCnt, 0))
                            m_lFinancePlanVersion = CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFVersion, 0))
                            m_lReturn = BusinessToInterface()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                'result = m_lReturn
                                Throw New Exception()
                            End If
                            If txtTotalAmount.Text.Length > 0 AndAlso IsNumeric(txtTotalAmount.Text) AndAlso gPMFunctions.ToSafeCurrency(txtTotalAmount.Text, 0) = 0 Then
                                Dim aoPremiumFinanceArrayObject As Object(,) = Nothing
                                m_lReturn = g_oBusiness.GetSingleFinancePlan(m_lFinancePlanCnt, m_lFinancePlanVersion, aoPremiumFinanceArrayObject)
                                If m_lReturn <> PMEReturnCode.PMTrue Then
                                    MsgBoxSirius("failed to load method bSIRPremiumFinance.Business.GetSingleFinancePlan", MsgBoxStyle.OkOnly, "cmdSave_Click()")
                                    Throw New Exception("failed to load method bSIRPremiumFinance.Business.GetSingleFinancePlan")
                                End If
                                If IsArray(aoPremiumFinanceArrayObject) AndAlso aoPremiumFinanceArrayObject.Length > 0 AndAlso gPMFunctions.ToSafeCurrency(aoPremiumFinanceArrayObject(k_PFPlanTotalCost, 0), 0) = 0 _
                                AndAlso Convert.ToString(aoPremiumFinanceArrayObject(k_PFPlanStatusInd, 0)) = PFStatusIndLive Then
                                    m_lReturn = g_oBusiness.StatusUpdate(CType(m_lFinancePlanCnt, Integer), m_lFinancePlanVersion, PFStatusIndCompleted)
                                    If m_lReturn <> PMEReturnCode.PMTrue Then
                                        MsgBoxSirius("failed to load method bSIRPremiumFinance.Business.StatusUpdate", MsgBoxStyle.OkOnly, "cmdSave_Click()")
                                        Throw New Exception("failed to load method bSIRPremiumFinance.Business.StatusUpdate")
                                    End If
                                    Me.Hide()
                                End If
                            End If
                                m_lReturn = g_oBusiness.DeletePFTransID(v_lPremFinanceCnt:=m_lFinancePlanCnt, v_lPremFinanceVersion:=m_lFinancePlanVersion)
                             If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                 Throw New Exception()
                             End If

                             m_lReturn = g_oBusiness.InsertPFTransID(v_vPFTransArray:=m_vFinancePlanMTATransArray, v_lPremFinanceCnt:=m_lFinancePlanCnt, v_lPremFinanceVersion:=m_lFinancePlanVersion)

                            m_vFinancePlanMTATransArray = Nothing
                        Else

                            m_vFinancePlanMTATransArray = ""
                            MsgBoxSirius("Error updating the Finance Scheme.", MsgBoxStyle.Exclamation, "MTA Failed")

                        End If

                        'cmdSave.Enabled = False
                    End If
                End If

            ElseIf Information.IsArray(m_vFinancePlanTransArray) And iSIRPremFinConst.m_sTransType <> "MTA" Then

                m_lReturn = g_oBusiness.DeletePFTransID(v_lPremFinanceCnt:=m_lFinancePlanCnt, v_lPremFinanceVersion:=m_lFinancePlanVersion)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception()
                End If

                m_lReturn = g_oBusiness.InsertPFTransID(v_vPFTransArray:=m_vFinancePlanTransArray, v_lPremFinanceCnt:=m_lFinancePlanCnt, v_lPremFinanceVersion:=m_lFinancePlanVersion)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanInsuranceFileCnt, 0)) <> "" Then

                m_lReturn = g_oBusiness.AddPartners(v_lPremiumFinanceCnt:=CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanInsuranceFileCnt, 0)), v_vPartnerArray:=m_vPartnerArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception()
                End If

                'Changes done as per VB code
                'If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStatusInd, 0)) <> "011" Then
                If m_sTransactionType <> "MTC" Then '-- PN 4299

                    m_lReturn = g_oBusiness.CreateEvent(v_lMode:=m_iTask, v_lInsuranceFileCnt:=m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanInsuranceFileCnt, 0), v_lPartyCnt:=m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientId, 0), v_sPlanRef:=m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAutoGenPlanRef, 0))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception()
                    End If

                End If
                'Changes done as per VB code
                'End If
            Else

                m_lReturn = g_oBusiness.CreateEvent(v_lMode:=m_iTask, v_lInsuranceFileCnt:=0, v_lPartyCnt:=m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientId, 0), v_sPlanRef:=m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanAutoGenPlanRef, 0))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception()
                End If
            End If

            If Spawned Then
                Me.Hide()
            Else
                m_lReturn = m_oGeneral.GetInterfaceDetails()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception()
                End If
            End If

            ' HG23072003 - Generate Documentation (Print off Instalment Plan)
            If m_sRunningContext.ToUpper() <> "UWNB" And CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStatusInd, 0)) = bSIRPremFinConst.PFStatusIndLive Then

                'Steve Watton PN 12140, if changes are being saved to client details and the policy is live the
                'we need to send an EDI message. 08/06/2004

                'PN12762 Only do this for third party plans!
                If m_lSchemeTypeID = bSIRPremFinConst.PFThirdParty Or m_lSchemeTypeID = bSIRPremFinConst.PFThirdPartyRecovery Then

                    m_lReturn = g_oBusiness.ProduceEDIMessage(vPremFinanceArray:=m_vFinancePlanArray, vReTransmit:="", bIsCancellation:=False, bIsNonTransactionalMTA:=True)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MsgBoxSirius("An Error occured sending the MTA EDI message", MsgBoxStyle.OkOnly, "Save Finance Plan")
                    End If
                End If 'PN12762

                ''''' End of Steve Watton 08/06/2004

                'PN13594
                If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanQuoteDocID, 0)) = "" And CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankDocID, 0)) = "" And CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCreditDocID, 0)) = "" Then
                Else

                    If MsgBoxSirius("Generate Instalment Documentation?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Print Plan") = System.Windows.Forms.DialogResult.Yes Then

                        ' HG23072003 - We don't have to check the return val as all errors are
                        '              picked up and reported in the print component. Also a
                        '              failure here shouldn't rollback the whole lot.
                        m_lReturn = CreateInstalmentsDocs(v_eDocType:=PrintDocType.DocTypeInstalmentEdit)
                    End If
                End If 'PN13594
            End If

            ' RAW 07/03/2003 : moved from earlier in procedure
            ' Refresh the form with any new details
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            'If the status is live then we should close the form to let the user know that all was successful
            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStatusInd, 0)) = bSIRPremFinConst.PFStatusIndLive And Not Spawned Then
                Me.Hide()
            End If
            '    bUpdateSuccessfull = True

        Catch excep As System.Exception

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "cmdSave_Click Failed", ACApp, ACClass, "cmdSave_Click", Information.Err().Number, excep.Message, excep:=excep)

            ' RAW 11/03/2003 : ISS2580 : added
            'g_oBusiness.RollbackTrans

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
            Exit Sub

        End Try

    End Sub

    Private Sub cmdSettlePlan_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSettlePlan.Click

        Dim frm As frmSettle
        Dim vDebitTransdetail As Object = Nothing

        Try

            If GetOutstandingBalance() <= 0 Then
                MsgBoxSirius("This Instalment plan has no outstanding balance.", MsgBoxStyle.Exclamation, "Settle not allowed")
            Else
                frm = New frmSettle()
                frm.FinancePlan = VB6.CopyArray(m_vFinancePlanArray)
                VB6.ShowForm(frm, FormShowConstants.Modal, Me)

                If frm.OK Then

                    m_lReturn = g_oBusiness.CancelPlanInHouse(m_vFinancePlanArray, 0, r_vDebitTransDetail:=vDebitTransdetail)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Settle the Plan", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdSettlePlan_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Else
                        m_lInsuranceFile_cnt = gPMFunctions.ToSafeLong(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanInsuranceFileCnt, 0), 0)
                        ReDim m_vFinancePlanTransArray(0, 0)

                        m_vFinancePlanTransArray(0, 0) = vDebitTransdetail
                        m_lReturn = ProcessCash(frm.SettleAmount, True)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            'The receipt roadmap has failed or been cancelled
                            'let the user know what to do.
                            MsgBoxSirius("The receipt has not been completed for the settlement amount." &
                                         Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) &
                                         "The settlement debt will remain on the account until paid.", MsgBoxStyle.Exclamation, "Settlement Receipt not completed")
                            Exit Sub
                        Else

                            ' PN 57172 - If we are Settling the Plan then it should have a status as Completed

                            m_lReturn = g_oBusiness.StatusUpdate(vPremiumFinanceCnt:=m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFCnt, 0), vPremiumFinanceVersion:=m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFVersion, 0), vStatusInd:=bSIRPremFinConst.PFStatusIndCompleted)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Update the Plan Status", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdSettlePlan_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                            End If
                            Me.Hide()
                        End If
                    End If

                    ' HG23072003 - Generate Documentation (Print off Instalment Plan)
                    'PN13594
                    If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanQuoteDocID, 0)) = "" And CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankDocID, 0)) = "" And CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCreditDocID, 0)) = "" Then
                    Else
                        If MsgBoxSirius("Generate Instalment Documentation?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Print Plan") = System.Windows.Forms.DialogResult.Yes Then

                            ' HG23072003 - We don't have to check the return val as all errors are
                            '              picked up and reported in the print component. Also a
                            '              failure here shouldn't rollback the whole lot.
                            m_lReturn = CreateInstalmentsDocs(v_eDocType:=PrintDocType.DocTypeInstalmentSettle)
                        End If
                    End If 'PN13594
                End If
                frm = Nothing
            End If

        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Settle Plan button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdSettlePlan_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdTransact_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdTransact.Click

        Dim lMode As Integer
        Dim bHaveOpenTransaction, bAllocationExist, bCheckDeposit As Boolean
        Dim lOldPlanCnt, lVersion As Integer
        Dim vMediaHistoryPrevId As Object = Nothing
        Dim vMediaHistoryCurrId As Object = Nothing
        Dim lPlanRefChanged As Integer
        Dim lDepositAmount As Decimal
        Dim bDepositAsInstalment As Boolean

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            If m_sUnderwriting <> "A" Then

                m_lReturn = g_oBusiness.CheckAllocationAgainstPolicy(v_lInsuranceFileCnt:=gPMFunctions.ToSafeLong(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanInsuranceFileCnt, 0), 0), r_bAllocationExist:=bAllocationExist)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError("cmdTransact_click", "Call to CheckAllocationAgainstPolicy Failed.")
                End If

                If bAllocationExist Then
                    If MsgBoxSirius("Payment Allocation against this Policy already exists." & Strings.Chr(13) & Strings.Chr(10) & "Do you wish to continue?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Cannot Transact") = System.Windows.Forms.DialogResult.No Then

                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                        Exit Sub
                    End If
                End If
            End If

            ' Validate Mandatory Fields
            m_lReturn = ValidateDataEntry()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                Exit Sub
            End If

            ' Process the next set of actions depending upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("cmdTransact_click", "Call to ProcessCommand Failed.")
            End If

            ' RAW 11/03/2003 : ISS2580 : added

            m_lReturn = g_oBusiness.BeginTrans
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                MsgBoxSirius("Failed to start a database transaction")
                RaiseError("cmdTransact_click", "Call to BeginTrans Failed.")
            End If
            bHaveOpenTransaction = True

            ' Process the plan depending on current state


            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanProductClass, 0)).Trim() = "SG" Then

                m_vFinancePlanTransArray = m_vSGTransactionArray
            ElseIf Information.IsArray(m_vFinancePlanMTATransArray) And iSIRPremFinConst.m_sTransType = "MTA" Then

                m_vFinancePlanTransArray = m_vFinancePlanMTATransArray
            End If

            If iSIRPremFinConst.m_sTransType = "NB" Or iSIRPremFinConst.m_sTransType = "REN" Or iSIRPremFinConst.m_sTransType = "SG" Then
                lMode = 3

                m_lReturn = g_oBusiness.LoadPartners(m_vPartnerArray)
                ' RAW 11/03/2003 : ISS2580 : removed tests against m_bDeposit and bDeposit argument

                m_lReturn = g_oBusiness.ProcessPlan(lMode:=lMode, v_vPremiumFinanceArray:=m_vFinancePlanArray, v_vTransArray:=m_vFinancePlanTransArray, v_iMTAType:=MTAType, v_lAccountId:=m_lAccountID)

                lPlanRefChanged = 0
                If iSIRPremFinConst.m_sTransType = "REN" And CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanMediaTypeValidation, 0)).Trim() = "BANK" Then

                    m_lReturn = g_oBusiness.GetOldPremimFinanceCnt(m_lFinancePlanCnt, lOldPlanCnt, lVersion, lPlanRefChanged)
                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                        m_lReturn = g_oBusiness.GetMediaHistoryId(lPremFinanceCnt:=m_lFinancePlanCnt, lPremFinanceVersion:=m_lFinancePlanVersion, vMediaHistoryPrev:=vMediaHistoryPrevId, vMediaHistoryCurrent:=vMediaHistoryCurrId)

                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                            If lPlanRefChanged = 1 Then
                                'fetch old plan media type id

                                m_lReturn = g_oBusiness.GetMediaHistoryId(lPremFinanceCnt:=lOldPlanCnt, lPremFinanceVersion:=lVersion, vMediaHistoryPrev:=vMediaHistoryPrevId, vMediaHistoryCurrent:=vMediaHistoryCurrId)

                                m_lReturn = g_oBusiness.CreateInstalmentNotification(lOldPlanCnt, lVersion, "0C", vMediaHistoryCurrId)

                                'fetch new plan media type id

                                m_lReturn = g_oBusiness.GetMediaHistoryId(lPremFinanceCnt:=m_lFinancePlanCnt, lPremFinanceVersion:=m_lFinancePlanVersion, vMediaHistoryPrev:=vMediaHistoryPrevId, vMediaHistoryCurrent:=vMediaHistoryCurrId)

                                m_lReturn = g_oBusiness.CreateInstalmentNotification(m_lFinancePlanCnt, m_lFinancePlanVersion, "0N", vMediaHistoryCurrId)

                            End If
                        End If
                    End If
                End If
            Else
                'Thinh Nguyen 29/11/2001
                lMode = 4
                ' RAW 11/03/2003 : ISS2580 : removed bDeposit argument

                m_lReturn = g_oBusiness.ProcessPlan(lMode:=lMode, v_vPremiumFinanceArray:=m_vFinancePlanArray, v_vTransArray:=m_vFinancePlanTransArray, v_iMTAType:=MTAType, v_lAccountId:=m_lAccountID)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                If g_oBusiness.ErrorCode <> "" Then

                    MsgBoxSirius("There was an error while trying to contact the webservice." & Strings.Chr(13) & Strings.Chr(10) & g_oBusiness.ErrorCode & ": " & g_oBusiness.ErrorText)
                    RaiseError("cmdTransact_click", "Call to ProcessPlan Failed.")
                Else
                    ' RAW 11/03/2003 : ISS2580 : added
                    MsgBoxSirius("Failed to process the plan", MsgBoxStyle.OkOnly, "Process Plan")
                    Exit Sub
                End If
            End If

            'Start - Sankar  - PN 56850
            ' determine if the plan is zero rated if so update status to completed rather than live

            m_lReturn = g_oBusiness.CheckDeposit(v_lPremiumFinanceCnt:=m_lFinancePlanCnt, v_lPremiumFinancePlanVer:=m_lFinancePlanVersion, r_cDepositAmount:=lDepositAmount, r_bDepositAsInstalment:=bDepositAsInstalment)

            If CDbl(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanTotalCost, 0)) = 0 Then
                If bDepositAsInstalment Then
                    If lDepositAmount <> 0 Then
                        ' Update the plans status to live

                        m_lReturn = g_oBusiness.StatusUpdate(vPremiumFinanceCnt:=m_lFinancePlanCnt, vPremiumFinanceVersion:=m_lFinancePlanVersion, vStatusInd:=bSIRPremFinConst.PFStatusIndLive)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            MsgBoxSirius("Failed to update the Plan Status")
                            RaiseError("cmdTransact_click", "Failed to update the Plan Status")
                        End If
                    Else
                        ' Update the plans status to completed

                        m_lReturn = g_oBusiness.StatusUpdate(vPremiumFinanceCnt:=m_lFinancePlanCnt, vPremiumFinanceVersion:=m_lFinancePlanVersion, vStatusInd:=bSIRPremFinConst.PFStatusIndCompleted)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            MsgBoxSirius("Failed to update the Plan Status")
                            RaiseError("cmdTransact_click", "Failed to update the Plan Status")
                        End If

                        m_lReturn = g_oBusiness.UpdateInstalmentStatusToCollected(v_nPremiumFinanceCnt:=m_lFinancePlanCnt, v_nPremiumFinanceVersion:=m_lFinancePlanVersion)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                            RaiseError("cmdTransact_click", "Failed to update the Instalment Status")
                        End If
                    End If
                Else
                    ' Update the plans status to completed

                    m_lReturn = g_oBusiness.StatusUpdate(vPremiumFinanceCnt:=m_lFinancePlanCnt, vPremiumFinanceVersion:=m_lFinancePlanVersion, vStatusInd:=bSIRPremFinConst.PFStatusIndCompleted)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        RaiseError("cmdTransact_click", "Failed to update the Plan Status")
                    End If
                    m_lReturn = g_oBusiness.UpdateInstalmentStatusToCollected(v_nPremiumFinanceCnt:=m_lFinancePlanCnt, v_nPremiumFinanceVersion:=m_lFinancePlanVersion)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError("cmdTransact_click", "Failed to update the Instalment Status")
                    End If
                End If
            Else
                ' Update the plans status to live

                m_lReturn = g_oBusiness.StatusUpdate(vPremiumFinanceCnt:=m_lFinancePlanCnt, vPremiumFinanceVersion:=m_lFinancePlanVersion, vStatusInd:=bSIRPremFinConst.PFStatusIndLive)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MsgBoxSirius("Failed to update the Plan Status")
                    RaiseError("cmdTransact_click", "Call to StatusUpdate Failed.")
                End If
            End If
            ' RAW 11/03/2003 : ISS2580 : added

            m_lReturn = g_oBusiness.CommitTrans
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                MsgBoxSirius("Failed to commit all changes to the database")
                RaiseError("cmdTransact_click", "Call to CommitTrans Failed.")
            End If

            bHaveOpenTransaction = False

            ' Get the interface details.
            m_lReturn = m_oGeneral.GetInterfaceDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("cmdTransact_click", "Call to GetInterfaceDetails Failed.")
            End If

            ' RAW 11/03/2003 : ISS2580 : moved from before call to ProcessPlan
            'TR - From which Roadmap context have we approached this?
            If m_sRunningContext.ToUpper() <> "UWNB" Then
                If CDec(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDeposit, 0)) > 0 And (CDbl(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanSchemeType_ID, 0)) = bSIRPremFinConst.PFInHouse) Then
                    ' We only need to ask this question if the "Deposit as instalment" check is not checked
                    If Not CBool(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDepositAsInstalment, 0)) Then
                        If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanProviderCollectDeposit, 0)).Trim() <> "" Then
                            bCheckDeposit = Not (CBool(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanProviderCollectDeposit, 0)))
                        Else
                            bCheckDeposit = True
                        End If

                        If bCheckDeposit Then
                            m_lReturn = MsgBoxSirius("Is the Deposit to be Receipted now?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Transact " &
                                        "Instalment Plan")
                            If m_lReturn = System.Windows.Forms.DialogResult.Yes Then
                                ' m_bDeposit = True           ' RAW 11/03/2003 : ISS2580 : removed

                                Dim iSchemeCurrencyID As Integer
                                Dim dXRate As Double

                                m_lReturn = g_oBusiness.GetSchemeCurrency(m_lFinancePlanCnt, m_lFinancePlanVersion, iSchemeCurrencyID, dXRate)

                                m_lReturn = ProcessCash(v_iSchemeCurrencyID:=iSchemeCurrencyID)

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                    Exit Sub
                                    'GoTo Err_cmdTransact_Click
                                End If

                                'PN11263 Allocate the deposit transaction to journal

                                m_lReturn = g_oBusiness.AllocateDeposit(lAccountId:=m_lAccountID, vDepositDetails:=CStr(m_lDepositTransId) & "|" & CStr(m_cDepositAmount * dXRate * -1))
                                If m_lDepositTransId = 0 Or m_cDepositAmount = 0 Then
                                    MessageBox.Show("Deposit not entered correctly." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) &
                                                    "Please add a receipt manually and allocate it against the deposit journal.", "No Deposit", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                    RaiseError("cmdTransact_click", "Call to AllocateDeposit Failed.")
                                End If
                            End If
                        End If
                    End If
                End If
            End If

            'Thinh Nguyen 29/11/2001 (start) - mode = 0,1,3,4 Print. mode = 2 send EDI message
            'only mode 3 or 4 here so lets print

            ' RAM20050901 - PN 22793 - Always set the flag to 3 irrespective of m_sTransType,
            '                           so that it will print all the 4 documents
            lMode = 3
            m_lReturn = DocumentPrint(lMode)

            ' Inform the user of success
            MsgBoxSirius("Instalment Plan Successfully Transacted", MsgBoxStyle.Information, "Transact Instalment Plan")

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            If m_lSilentTransact = 0 Then
                Me.Hide()
            End If

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanSchemeTypeCode, 0)).Trim().ToUpper() = "TP" Then

                m_lReturn = g_oBusiness.UpdateSuspendedAccountsTransactions(v_lInsuranceFileCnt:=m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanInsuranceFileCnt, 0), v_lLinked_transdetail_id:=m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPlanTransactionID, 0))
            End If

            Dim temp_m_oProduct As Object
            Dim lReturn = MainModule.g_oObjectManager.GetInstance(temp_m_oProduct, "bSIRProduct.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oProduct = temp_m_oProduct

            Dim oInsuranceFileDetails As Object
            Dim m_oBusiness As Object
            lReturn = MainModule.g_oObjectManager.GetInstance(m_oBusiness, "bSIRListRisks.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)

            Dim nResult = m_oBusiness.GetInsuranceFileDetails(v_lInsuranceFileCnt:=m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanInsuranceFileCnt, 0), r_vResults:=oInsuranceFileDetails)


            Dim vresultarray As Object
            Dim istruemonthly As Integer
            Dim isautorenewbdm As Integer
            m_lReturn = oProduct.GetProductValue(v_lProductId:=m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanProduct_ID, 0), v_sColumnName:="is_true_monthly_policy", r_vProductArray:=vresultarray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError("cmdmakelive", "failed to retreive product risk maintainence option for taskgroup", gPMConstants.PMELogLevel.PMLogError)
            Else
                istruemonthly = gPMFunctions.ToSafeLong(vresultarray(0, 0), 0)
            End If

            m_lReturn = oProduct.GetProductValue(v_lProductId:=m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanProduct_ID, 0), v_sColumnName:="auto_renew_bdmpolicy", r_vProductArray:=vresultarray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError("cmdmakelive", "failed to retreive product risk maintainence option for taskgroup", gPMConstants.PMELogLevel.PMLogError)
            Else
                isautorenewbdm = gPMFunctions.ToSafeLong(vresultarray(0, 0), 0)
            End If

            If istruemonthly = 1 AndAlso isautorenewbdm = 1 AndAlso (m_sTransactionType.Trim().ToUpper() = "NB" Or m_sTransactionType.Trim().ToUpper() = "MTR") AndAlso m_lSilentTransact <> 1 Then
                Dim frmIrenewalcatch As iPMURenewalCatchUp.frmInterface
                frmIrenewalcatch = New iPMURenewalCatchUp.frmInterface()
                frmIrenewalcatch.PolicyRef = oInsuranceFileDetails(0, 0)
                frmIrenewalcatch.CoverStart = m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCoverStartDate, 0)
                frmIrenewalcatch.CoverEnd = m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCoverEndDate, 0)
                frmIrenewalcatch.SchemeNo = m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanSchemeNo, 0)
                If m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCoverStartDate, 0) < DateTime.Now.AddMonths(-1) Then
                    frmIrenewalcatch.ShowDialog()
                End If
            End If

        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Transact button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdTransact_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
        Finally
            If bHaveOpenTransaction Then
                ' RAW 11/03/2003 : ISS2580 : added

                g_oBusiness.RollbackTrans()
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
        End Try
    End Sub

    Private Sub cmdTransactions_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdTransactions.Click

        Dim nCount, nLower, nUpper As Integer
        Dim sTransdetailIDs As New StringBuilder

        Try

            If Not Information.IsArray(m_vFinancePlanTransArray) Then
                MsgBoxSirius("Transaction Data is not available for this Plan.", MsgBoxStyle.Exclamation, "No transaction data.")
            Else
                m_vTransactionArray = Nothing

                ' If we already have this array don't build it again
                Dim temp_g_oFindTransaction As Object = Nothing
                m_lReturn = g_oObjectManager.GetInstance(temp_g_oFindTransaction, "bACTFindTransaction.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                g_oFindTransaction = temp_g_oFindTransaction
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACClass + ", Unable to get Transaction Data")
                End If

                m_lReturn = g_oFindTransaction.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vEffectiveDate:=m_dtEffectiveDate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACClass + ", Unable to set Transaction Process Modes")
                End If

                MainModule.GetLookupValues()

                ' Get array bounds and walk

                nLower = m_vFinancePlanTransArray.GetLowerBound(1)

                nUpper = m_vFinancePlanTransArray.GetUpperBound(1)
                sTransdetailIDs = New StringBuilder("")

                For nCount = nLower To nUpper

                    sTransdetailIDs.Append(Mid(",", 1, nCount) & CStr(m_vFinancePlanTransArray(0, nCount)))
                Next nCount

                m_lReturn = g_oFindTransaction.SelectTransQuery(r_vResultArray:=m_vTransactionArray, r_lNumberOfRecords:=nCount, v_iCompanyID:=g_iSourceID, v_vTransDetailIDs:=sTransdetailIDs.ToString())
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACClass + ", Unable to retreive transactions")
                    Exit Sub
                End If

                m_lReturn = ProcessTransactions()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACClass + ", Unable to view version list")
                End If

                g_oFindTransaction.Dispose()
                g_oFindTransaction = Nothing

            End If

        Catch excep As System.Exception

            ' Let the user know what happened
            MsgBoxSirius("Unable to get Transaction Data")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to process transactions", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub dtpReviewDate_ValueChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles dtpReviewDate.ValueChanged

        If Convert.IsDBNull(dtpReviewDate.Value) Or IsNothing(dtpReviewDate.Value) Then
            dtpReviewDate.Format = DateTimePickerFormat.Custom
            dtpReviewDate.CustomFormat = " "
        Else
            'TR - TODO get a different event (for check/uncheck only?)
            'Call SetReviewedFromConfirmedDate
            dtpReviewDate.Format = DateTimePickerFormat.Short
        End If
    End Sub

    Private Sub Form_Initialize_Renamed()

        Try
            Dim sValue As String = ""
            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMBFinancePlanMaint.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(Me, g_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            'Get bPMLock
            Dim temp_m_oPMLock As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oPMLock = temp_m_oPMLock

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            'Start (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)
            ' Removed codes related to m_bIsCCAuthorisationOff and m_bIsIgnore9
            m_lReturn = iPMFunc.GetSystemOption(5069, sValue, 1) ' System option credit card processing method
            m_bIsExternalCreditCardProcessing = (sValue = "1") '0- Internal ; 1- External
            'End (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub LoadFunction()

        ' Forms load event.
        Try

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' RAW 04/03/2003 : ISS2592 : moved from later in this sub
            'Thinh Nguyen 18/03/2002 (start) - get underwriting switch

            If g_oBusiness.GetHiddenOption(r_sValue:=m_sUnderwriting) <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            m_sUnderwriting = m_sUnderwriting.ToUpper()

            m_oFormFields = New iPMFormControl.FormFields()
            m_bLoading = True
            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()
            LoadStatusCombo()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            'PN12594
            m_lReturn = DisplayLookupDetails()
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            'SJ 20/08/2004 - start
            m_lReturn = SetQASDatabaseId()
            'SJ 20/08/2004 - end

            'SJ 20/08/2004 - start
            EnableDisableTabs(0)
            'SJ 20/08/2004 - end

            'ACR 05-05-05 Enable the print button for PCLSG quotes..
            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanProductClass, 0)).Trim() = "SG" Then
                cmdRePrint.Enabled = True
                cmdSettlePlan.Enabled = False
            End If

            'ACR 25-07-05 show credit card tab for close iprompt..
            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanProductClass, 0)).Trim() = "SG" Then
                If CDbl(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDeposit, 0)) > 0 Then
                    If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanProviderCollectDeposit, 0)).Trim() = "" Then
                        If Interaction.MsgBox("Do you want the finance provider to collect the deposit via credit/debit card?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "Deposit") = System.Windows.Forms.DialogResult.Yes Then

                            m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanProviderCollectDeposit, 0) = 1
                            SSTabHelper.SetTabVisible(tabMainTab, 3, True)
                            m_bMandatoryCCDetails = True
                        Else
                            m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanProviderCollectDeposit, 0) = 0
                            If tabMainTab.TabCount >= 3 Then
                                SSTabHelper.SetTabVisible(tabMainTab, 3, False)
                            End If
                            m_bMandatoryCCDetails = False
                        End If

                        'save the change we just made..
                        m_lReturn = InterfaceToBusiness()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New Exception()
                        End If
                    End If
                Else
                    m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanProviderCollectDeposit, 0) = 0
                    If tabMainTab.TabCount >= 3 Then
                        SSTabHelper.SetTabVisible(tabMainTab, 3, False)
                    End If
                    m_bMandatoryCCDetails = False
                End If
            End If

            'Make telephone mandatory for Amber
            If m_sProviderCode = "AMBER" Then
                m_oFormFields.Item("txtClientAreaCode-0").IsMandatory = True
                m_oFormFields.Item("txtClientNumber-0").IsMandatory = True
            End If

            addClient.Width = VB6.TwipsToPixelsX(8625)
            frmPartners.Visible = False

            addBank.Initialise()
            'END ACR
            Dim lLeadAgentCnt As Long

            m_lReturn = g_oBusiness.GetAgentAndType(v_lPremFinanceCnt:=ToSafeLong(m_vFinancePlanArray(k_PFPlanPFCnt, 0)),
            v_lPremFinanceVersion:=ToSafeLong(m_vFinancePlanArray(k_PFPlanPFVersion, 0)),
            r_lLeadAgentCnt:=lLeadAgentCnt,
            r_lAgentType:=lIsBroker)

            If lLeadAgentCnt <> 0 Then
                lLeadAgentCnt = lLeadAgentCnt
            End If
            'Party Bank Details
            PopulateBankDetails()

            If Not m_bHistory And bIsSinglePlanParty Then
                If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStatusInd, 0)).Trim() = "999" And StringsHelper.ToDoubleSafe(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFIsCancelPolicyRun, 0)).Trim()) <> 1 Then
                    SetCommand(cmdCancelPolicy, True, True)
                End If
            End If

            Me.Refresh()

            m_bChangeAccountType = True
            m_bLoading = False
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        m_nUnloadMode = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If m_nUnloadMode <> vbFormCode Then
                'Validate Mandatory Fields
                If m_bPlanReleasedFromOnHold Then
                    m_lReturn = ValidateDataEntry()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        eventArgs.Cancel = True
                        Exit Sub
                    End If
                End If

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If

                If (VB6.GetItemData(cboStatus, cboStatus.SelectedIndex) = 10 OrElse VB6.GetItemData(cboStatus, cboStatus.SelectedIndex) = 11 And m_sTransType = "REN") AndAlso m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                    RollbackPlan()
                End If

                If (VB6.GetItemData(cboStatus, cboStatus.SelectedIndex) = 10 Or VB6.GetItemData(cboStatus, cboStatus.SelectedIndex) = 11) AndAlso m_sCallingAppName <> "iPMBFindFinancePlan" AndAlso m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                    m_lReturn = g_oBusiness.UpdatePaymentMethod(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanInsuranceFileCnt, 0))
                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()

            ' Destroy the instance of the general object from memory.
            m_oGeneral = Nothing

            ' Destroy the instance of the lock object from memory.
            If Not (m_oPMLock Is Nothing) Then
                m_oPMLock = Nothing
            End If

            ' Terminate the form control object.
            m_oFormFields.Dispose()

            ' Destroy the instance of the form control object from memory.
            m_oFormFields = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to terminate the interface", MainModule.ACApp, ACClass, "Form_QueryUnload", Information.Err().Number, excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If
        ' Pass through to function
        m_lReturn = ResizeInterface()
    End Sub

    Private Sub lvwHistory_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwHistory.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwHistory.Columns(eventArgs.Column)

        With lvwHistory

            If gPMFunctions.ToSafeString(ColumnHeader.Tag) = gPMFunctions.ToSafeString(1) Then
                ColumnHeader.Tag = CStr(0)
            Else
                ColumnHeader.Tag = CStr(1)
            End If

            Select Case ColumnHeader.Index + 1
                Case HistoryListViewColIndex.AccountCode, HistoryListViewColIndex.AccountName, HistoryListViewColIndex.AccountNumber,
                    HistoryListViewColIndex.ActionCode, HistoryListViewColIndex.AddressLine1, HistoryListViewColIndex.Branch,
                    HistoryListViewColIndex.Name_Renamed, HistoryListViewColIndex.PostCode, HistoryListViewColIndex.User,
                    HistoryListViewColIndex.PaperDD, HistoryListViewColIndex.PaymentType, HistoryListViewColIndex.AccountType,
                    HistoryListViewColIndex.BIC, HistoryListViewColIndex.IBAN

                    ListViewHelper.SetSortedProperty(lvwHistory, False)
                    ListViewHelper.SetSortKeyProperty(lvwHistory, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortOrderProperty(lvwHistory, IIf(ListViewHelper.GetSortOrderProperty(lvwHistory) = SortOrder.Ascending, SortOrder.Descending, SortOrder.Ascending))
                    ListViewHelper.SetSortedProperty(lvwHistory, True)

                Case HistoryListViewColIndex.DateModified

                    ListView6Func.ListViewSortByDate(lvwHistory, ColumnHeader.Index + 1 - 1, (ListViewHelper.GetSortOrderProperty(lvwHistory) + 1) Mod 2)

            End Select

        End With

    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        Try

            'SJ 20/08/2004 - start
            EnableDisableTabs(SSTabHelper.GetSelectedIndex(tabMainTab))
            'SJ 20/08/2004 - end

            ' Now I know this is crap, this goes against all my principles, but
            ' for some reason when using the mouse to select a tab the setfocus
            ' code below doesn't work. The cursor sticks, and you can't tab off.
            ' Therefore I've used this to get around the problem.
            Application.DoEvents()

            ' Set focus to the first control on the tab.
            '    If (tabMainTab.Tab <= UBound(m_ctlTabFirstLast, 2)) Then
            '        m_ctlTabFirstLast(ACControlStart, tabMainTab.Tab).SetFocus
            '    End If

        Catch

            tabMainTabPreviousTab = tabMainTab.SelectedIndex
        End Try

    End Sub

    ' ***************************************************************** '
    ' Region: m_oNavStart Events
    ' ***************************************************************** '
    Private Sub m_oNavStart_NavigatorClose() Handles m_oNavStart.NavigatorClose

        m_bNavCompleted = True

        cmdTransact.Enabled = True

        ' Gets the interface details to be displayed.
        m_lReturn = m_oGeneral.GetInterfaceDetails()

    End Sub

    Private Sub m_oNavStart_SetProcessStatus(ByVal v_bProcessComplete As Boolean) Handles m_oNavStart.SetProcessStatus
        ' Store the result
        m_bProcessComplete = v_bProcessComplete
    End Sub

    'Private Sub txtAccountNumber_Validate(Cancel As Boolean)
    '
    '    Dim oSirMediaTypeValidation As Object
    '    Dim bValid As Boolean
    '    Dim sStrippedString As String
    '
    '    On Error GoTo Err_txtAccountNumber_Validate
    '
    '    'TR - Make sure that Account Number field is populated
    '    If Len(txtAccountNumber.Text) > 0 Then
    '
    '        m_lReturn& = g_oObjectManager.GetInstance(oObject:=oSirMediaTypeValidation, _
    ''                sClassName:="bSirMediaTypeValidation.business", vInstanceManager:= _
    ''                PMGetViaClientManager)
    '
    '        'TR PN5080- Append the Account Number to the Sort Code field. Do
    '        'not check the Sort code field as some customers will put the sort code into
    '        'the account field (IAG), but for other (i.e. GB) customers the sort
    '        'code goes into it's own field. So sort code can be blank.
    '        'Strip the Spaces from the SortCode & AccountNumber before Validation
    '        sStrippedString = Replace(txtSortCode.Text, " ", "") & "|" & _
    ''            Replace(txtAccountNumber.Text, " ", "")
    '
    '        'TR - Perform the validation
    '        m_lReturn = oSirMediaTypeValidation.ValidateNumber(m_lMediaTypeID, _
    ''                g_lCountryID, sStrippedString, bValid)
    '        If m_lReturn = PMError Then
    '            MsgBoxSirius "Failed to validate Account No", vbExclamation, "Validate"
    '            Cancel = True
    '        Else
    '            If bValid <> True Then
    '                MsgBoxSirius "This is not a valid bank account", vbExclamation, _
    ''                        "Invalid Bank Account"
    '                Cancel = True
    '            End If
    '            Set oSirMediaTypeValidation = Nothing
    '        End If
    '
    '    End If
    '
    '    Exit Sub
    '
    'Err_txtAccountNumber_Validate:
    '
    '    ' Log Error Message
    '    LogMessage _
    ''            iType:=PMLogOnError, _
    ''            sMsg:="Validate Account No Failed", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="txtAccountNumber_Validate", _
    ''            vErrNo:=Err.Number, _
    ''            vErrDesc:=Err.Description
    '
    '    Exit Sub
    '
    'End Sub
    '
    Private Sub txtCardNo_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCardNo.Enter
        'TR - Added for PL 28/03/03
        If m_sCCNoGot <> "Invalid" Then
            m_sCCNoGot = txtCardNo.Text
        End If
    End Sub

    Private Sub txtCardNo_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtCardNo.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)

        'Allow backspace
        If KeyAscii = 8 Then
            If KeyAscii = 0 Then
                eventArgs.Handled = True
            End If
            Exit Sub
        End If
        'Allow Enter
        If KeyAscii = 13 Then
            If KeyAscii = 0 Then
                eventArgs.Handled = True
            End If
            Exit Sub
        End If

        If (Strings.Chr(KeyAscii).ToString() < "0" Or Strings.Chr(KeyAscii).ToString() > "9") And Strings.Chr(KeyAscii).ToString() <> " " Then
            KeyAscii = 0
        End If

        If (Strings.Chr(KeyAscii).ToString() >= "0" Or Strings.Chr(KeyAscii).ToString() <= "9") And txtCardNo.Text.Replace(" ", "").Length = 16 Then
            KeyAscii = 0
        End If

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    ' ***************************************************************** '
    ' Name: txtCardNo_Validate
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :
    '              Updated : MEvans : 28-03-2003 : Issue 2647
    ' ***************************************************************** '
    Private Sub txtCardNo_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles txtCardNo.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        Const sFunctionName As String = "txtCardNo_Validate"

        Dim oSirMediaTypeValidation As bSIRMediaTypeValidation.Business
        Dim bValid As Boolean
        Dim sCCNOMessage As String = String.Empty
        Dim sCCNoMessageBoxTitle As String = String.Empty
        Dim sCreditCardNo As String = String.Empty

        Try

            ' get the card no from the "copy from" variable if
            ' there is a mask "*" in the card no field otherwise
            ' use the value from the screen
            If Strings.InStr(8, txtCardNo.Text, "*") = 0 Then
                sCreditCardNo = txtCardNo.Text.Replace(" ", "")
            Else
                sCreditCardNo = m_sCreditCardNo
            End If

            ' if only tabbing through code then - dont validate
            ' save will demand that the field is populated
            'Start (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)
            ' Removed codes related to m_bIsCCAuthorisationOff and m_bIsIgnore9
            ' Don't do any validation if credit card processing value is external.
            If sCreditCardNo <> "" And Not m_bIsExternalCreditCardProcessing Then
                Dim temp_oSirMediaTypeValidation As Object = Nothing

                If g_oObjectManager.GetInstance(temp_oSirMediaTypeValidation, "bSIRMediaTypeValidation.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager) = gPMConstants.PMEReturnCode.PMTrue Then
                    oSirMediaTypeValidation = temp_oSirMediaTypeValidation

                    ' get ccno validate message box title
                    sCCNoMessageBoxTitle = GetResString(v_lItemId:=ACResFileMessageTitleCCNoValidate)

                    ' only a sub so no return to confirm or deny function worked / failed
                    ' so just run off status

                    oSirMediaTypeValidation.ValidateNumber(m_lMediaTypeID, g_lCountryID, sCreditCardNo, bValid)

                    If Not bValid Then
                        ' get invalid card number message
                        sCCNOMessage = GetResString(v_lItemId:=ACRefFileMessageCCNoInvalidCardNo)
                    End If

                Else
                    oSirMediaTypeValidation = temp_oSirMediaTypeValidation

                    bValid = False

                    '******************************
                    ' Log Error.
                    gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to create bSirMediaTypeValidation.business", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))
                    '*******************************

                End If

                ' cancel if number is not valid
                If Not bValid Then
                    Cancel = True
                    MsgBoxSirius(sCCNOMessage, MsgBoxStyle.Exclamation, sCCNoMessageBoxTitle)
                    ' reset card number if it is not valid
                    ' forcing validation before leaving screen
                    txtCardNo.Text = ""
                    sCreditCardNo = ""
                    If txtCardNo.Visible And txtCardNo.Enabled Then
                        txtCardNo.Focus()
                    End If
                Else
                    Cancel = False
                End If
                ' destroy object reference
                oSirMediaTypeValidation = Nothing

            Else
                ' reset the displayed card no as this shouldnt be set
                ' if the behind the scenes value sCreditCardNo isnt set
                'txtCardNo.Text = ""
                Cancel = False
            End If

            ' If valid, populate Tracking Number field
            If Not Cancel Then
                txtTrackingNumber.Text = m_sCreditCardNo
            Else
                txtTrackingNumber.Text = ""
            End If
            'End (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)

        Catch excep As System.Exception

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Exit Sub
            eventArgs.Cancel = Cancel
        End Try

    End Sub

    Private Sub txtCardStartDate_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtCardStartDate.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        'Allow backspace
        If KeyAscii = 8 Then
            If KeyAscii = 0 Then
                eventArgs.Handled = True
            End If
            Exit Sub
        End If
        'Allow Enter
        If KeyAscii = 13 Then
            If KeyAscii = 0 Then
                eventArgs.Handled = True
            End If
            Exit Sub
        End If
        If (Strings.Chr(KeyAscii).ToString() < "0" Or Strings.Chr(KeyAscii).ToString() > "9") And Strings.Chr(KeyAscii).ToString() <> "/" Then
            KeyAscii = 0
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    ' ***************************************************************** '
    ' Name: txtCardStartDate_Validate
    '
    ' Parameters: n/a
    '
    ' Description: Runs a series of checks on the credit cards start date
    '
    ' History:
    '           Created : MEvans : 28-03-2003 : Issue 2647
    ' ***************************************************************** '
    Private Sub txtCardStartDate_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles txtCardStartDate.Validating
        Dim Cancel As Boolean = eventArgs.Cancel

        Const sFunctionName As String = "txtCardStartDate_Validate"

        Dim bValid As Boolean
        Dim dtStartDate, dtExpiryDate As Date
        Dim sErrorMessageTitle As String = String.Empty
        Dim sErrorMessage As String = String.Empty

        Try

            ' validate if there is data to validate
            ' else will will be forced here as date is mandatory
            If txtCardStartDate.Text <> "" Then

                bValid = False

                ' check in correct format
                If Strings.Len(txtCardStartDate.Text) = 5 Then
                    Dim dbNumericTemp2 As Double
                    Dim dbNumericTemp As Double
                    If Double.TryParse(txtCardStartDate.Text.Substring(0, 2), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) And Double.TryParse(txtCardStartDate.Text.Substring(txtCardStartDate.Text.Length - 2), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                        If StringsHelper.ToDoubleSafe(txtCardStartDate.Text.Substring(0, 2)) <= 12 And StringsHelper.ToDoubleSafe(txtCardStartDate.Text.Substring(0, 2)) > 0 Then
                            If txtCardStartDate.Text.Substring(2, 1) = "/" Then

                                bValid = True
                            End If
                        End If
                    End If
                End If

                ' get error message
                If Not bValid Then
                    sErrorMessage = GetResString(v_lItemId:=ACResFileMessageCCStartDateWrongFormat)
                End If

                ' check valid date
                If bValid Then

                    ' get date string for checks
                    dtStartDate = DateAndTime.DateSerial(CInt(txtCardStartDate.Text.Substring(txtCardStartDate.Text.Length - 2)), CInt(txtCardStartDate.Text.Substring(0, 2)), CInt("01"))

                    ' expiry date must be after start date
                    If txtExpiryDate.Text <> "" Then
                        dtExpiryDate = DateAndTime.DateSerial(CInt(txtExpiryDate.Text.Substring(txtExpiryDate.Text.Length - 2)), CInt(txtExpiryDate.Text.Substring(0, 2)), CInt("01"))
                        If dtStartDate >= dtExpiryDate Then
                            sErrorMessage = GetResString(v_lItemId:=ACResFileMessageCCStartDateInvalidDate)
                            bValid = False
                        End If
                    End If

                    ' check if card has been activated
                    If bValid Then
                        If dtStartDate > DateTime.Now Then
                            sErrorMessage = GetResString(v_lItemId:=ACResFileMessageCCNotActivated)
                            bValid = False
                        End If
                    End If
                End If

                ' on error display message and reset field
                If Not bValid Then
                    sErrorMessageTitle = GetResString(v_lItemId:=ACResFileMessageTitleCCStartDateValidate)
                    MsgBoxSirius(sErrorMessage, MsgBoxStyle.Exclamation, sErrorMessageTitle)
                    txtCardStartDate.Text = ""
                    Cancel = True

                End If

            End If

        Catch excep As System.Exception

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Exit Sub

            eventArgs.Cancel = Cancel
        End Try

    End Sub

    Private Sub txtExpiryDate_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtExpiryDate.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        'Allow backspace
        If KeyAscii = 8 Then
            If KeyAscii = 0 Then
                eventArgs.Handled = True
            End If
            Exit Sub
        End If
        'Allow Enter
        If KeyAscii = 13 Then
            If KeyAscii = 0 Then
                eventArgs.Handled = True
            End If
            Exit Sub
        End If

        If (Strings.Chr(KeyAscii).ToString() < "0" Or Strings.Chr(KeyAscii).ToString() > "9") And Strings.Chr(KeyAscii).ToString() <> "/" Then
            KeyAscii = 0
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    ' ***************************************************************** '
    ' Name: txtExpiryDate_Validate
    '
    ' Parameters: n/a
    '
    ' Description: Runs a series of checks on the credit cards expiry date
    '
    ' History:
    '           Created : MEvans : 28-03-2003 : Issue 2647
    ' ***************************************************************** '
    Private Sub txtExpiryDate_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles txtExpiryDate.Validating
        Dim Cancel As Boolean = eventArgs.Cancel

        Const sFunctionName As String = "txtExpiryDate_Validate"

        'PSL 14/03/2003 Iss2467
        Dim bValid As Boolean

        Dim dtStartDate, dtExpiryDate As Date
        Dim sErrorMessageTitle As String = String.Empty
        Dim sErrorMessage As String = String.Empty

        Try

            If txtExpiryDate.Text <> "" Then

                bValid = False

                ' check expirty date is in the correct format
                If Strings.Len(txtExpiryDate.Text) = 5 Then
                    Dim dbNumericTemp2 As Double
                    Dim dbNumericTemp As Double
                    If Double.TryParse(txtExpiryDate.Text.Substring(0, 2), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) And Double.TryParse(txtExpiryDate.Text.Substring(txtExpiryDate.Text.Length - 2), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                        If StringsHelper.ToDoubleSafe(txtExpiryDate.Text.Substring(0, 2)) <= 12 And StringsHelper.ToDoubleSafe(txtExpiryDate.Text.Substring(0, 2)) > 0 Then
                            If txtExpiryDate.Text.Substring(2, 1) = "/" Then

                                bValid = True
                            End If
                        End If
                    End If
                End If

                ' get incorrect format error message
                If Not bValid Then
                    sErrorMessage = GetResString(v_lItemId:=ACResFileMessageCCExpiryDateWrongFormat)
                End If

                ' check date valid
                If bValid Then
                    ' get date string for checks
                    dtExpiryDate = DateAndTime.DateSerial(CInt(txtExpiryDate.Text.Substring(txtExpiryDate.Text.Length - 2)), CInt(txtExpiryDate.Text.Substring(0, 2)), CInt("01"))

                    ' expiry date must be after start date
                    If txtCardStartDate.Text <> "" Then
                        dtStartDate = DateAndTime.DateSerial(CInt(txtCardStartDate.Text.Substring(txtCardStartDate.Text.Length - 2)), CInt(txtCardStartDate.Text.Substring(0, 2)), CInt("01"))
                        If dtStartDate >= dtExpiryDate Then
                            sErrorMessage = GetResString(v_lItemId:=ACResFileMessageCCExpirtyDateInvalidDate)
                            bValid = False
                        End If
                    End If

                    ' check if the credit card has expired
                    If bValid Then
                        If dtExpiryDate < DateTime.Now Then
                            sErrorMessage = GetResString(v_lItemId:=ACResFileMessageCCExpired)
                            bValid = False
                        End If
                    End If

                End If

                '  on error display message and reset field
                If Not bValid Then
                    MsgBoxSirius(sErrorMessage, MsgBoxStyle.Exclamation, sErrorMessageTitle)
                    sErrorMessageTitle = GetResString(v_lItemId:=ACResFileMessageTitleCCExpiryDateValidate)
                    txtExpiryDate.Text = ""
                    Cancel = True

                End If

            End If

        Catch excep As System.Exception

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Exit Sub

            eventArgs.Cancel = Cancel
        End Try

    End Sub

    Private Sub txtIssueNo_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtIssueNo.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        'Allow backspace
        If KeyAscii = 8 Then
            If KeyAscii = 0 Then
                eventArgs.Handled = True
            End If
            Exit Sub
        End If
        'Allow Enter
        If KeyAscii = 13 Then
            If KeyAscii = 0 Then
                eventArgs.Handled = True
            End If
            Exit Sub
        End If

        If Strings.Chr(KeyAscii).ToString() < "0" Or Strings.Chr(KeyAscii).ToString() > "9" Then
            KeyAscii = 0
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtPFReference_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPFReference.Leave
        'Copy to hidden field, the PF field uses the reference field
        txtAgentRef.Text = txtPFReference.Text
    End Sub

    Private Sub txtPin_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtPin.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        'Allow backspace
        If KeyAscii = 8 Then
            If KeyAscii = 0 Then
                eventArgs.Handled = True
            End If
            Exit Sub
        End If
        'Allow Enter
        If KeyAscii = 13 Then
            If KeyAscii = 0 Then
                eventArgs.Handled = True
            End If
            Exit Sub
        End If

        If Strings.Chr(KeyAscii).ToString() < "0" Or Strings.Chr(KeyAscii).ToString() > "9" Then
            KeyAscii = 0
        End If

        If (Strings.Chr(KeyAscii).ToString() >= "0" Or Strings.Chr(KeyAscii).ToString() <= "9") And txtPin.Text.Replace(" ", "").Length = 4 Then
            KeyAscii = 0
        End If

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    '************************************************************************
    ' Name : DocumentPrint
    ' Desc : Depending on which PrintDocType the schema uses, mode 0,3
    '        print all docs; mode 1,4 print quote details
    ' Hist : Thinh Nguyen 07/12/2001
    '************************************************************************
    Private Function DocumentPrint(ByVal v_lMode As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DocumentPrint"

        Dim sSchemaPrintType As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Do not proceed if the finance plan array is empty or if the scheme is third party recovery
            If Information.IsArray(m_vFinancePlanArray) And m_lSchemeTypeID <> bSIRPremFinConst.PFThirdPartyRecovery Then

                'Always show the SG connectivity document.
                If v_lMode = 0 Or v_lMode = 3 Then
                    If gPMFunctions.NullToString(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanProductClass, 0)).Trim() = "SG" Then
                        m_lReturn = PrintDocument(PrintDocType.DocTypePCLSG, True)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("PrintDocument", "v_enDocType:=DocTypePCLSG", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If
                End If

                'Retrieve setting to see if we should print the documents
                sSchemaPrintType = CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanSchemePrintType, 0)).ToUpper()

                'Don't print when the "When to Print" setting for this scheme is set to "No Documents"
                If sSchemaPrintType <> "ND" Then

                    If v_lMode = 0 Or v_lMode = 1 Or v_lMode = 3 Or v_lMode = 4 Then

                        If gPMFunctions.NullToString(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanQuoteDocID, 0)) <> "" Then
                            m_lReturn = PrintDocument(PrintDocType.DocTypeQuote, True)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("PrintDocument", "v_enDocType:=DocTypeQuote", gPMConstants.PMELogLevel.PMLogError)
                            End If
                        End If

                    End If

                    If v_lMode = 0 Or v_lMode = 3 Then

                        If gPMFunctions.NullToString(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankDocID, 0)) <> "" Then
                            m_lReturn = PrintDocument(PrintDocType.DocTypeBank, True)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("PrintDocument", "v_enDocType:=DocTypeBank", gPMConstants.PMELogLevel.PMLogError)
                            End If
                        End If

                        If gPMFunctions.NullToString(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCreditDocID, 0)) <> "" Then
                            m_lReturn = PrintDocument(PrintDocType.DocTypeCredit, True)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("PrintDocument", "v_enDocType:=DocTypeCredit", gPMConstants.PMELogLevel.PMLogError)
                            End If
                        End If

                        If gPMFunctions.NullToString(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanConfirmationDocID, 0)) <> "" Then
                            m_lReturn = PrintDocument(PrintDocType.DocTypeConfirmation, True)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("PrintDocument", "v_enDocType:=DocTypeConfirmation", gPMConstants.PMELogLevel.PMLogError)
                            End If
                        End If

                    End If

                End If

            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here



            ' This is for debugging only



        End Try
        Return result
    End Function

    '************************************************************************
    ' Name : PrintDocument
    ' Desc : spool document according to v_enDocType
    '        mode = 1 Quote details
    '        mode = 2 Bank details
    '        mode = 3 Credit details
    ' Hist : Thinh Nguyen 29/11/2001 - Created
    '************************************************************************
    Private Function PrintDocument(ByVal v_enDocType As PrintDocType, Optional ByVal v_bArchiveToDocumaster As Boolean = False) As Integer
        Dim result As Integer = 0
        Dim oDocTemplate As iPMBDocTemplate.Interface_Renamed
        Dim lDocTemplateID, lDocTypeID, lInsuranceFileCnt, lInsuranceFolderCnt, lpartyCnt, lPremFinanceCnt, lPremFinanceVersion As Integer
        Dim sExpectedCode As String = ""
        Dim oFindDocTemplate As iPMBFindDocTemplate.Interface_Renamed
        Dim bDocumentCancelled As Boolean
        Try

            'TR - Make sure the important array is populated
            If Information.IsArray(m_vFinancePlanArray) Then

                'TR - Populate the local variables from the modular finance array
                lpartyCnt = CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientId, 0))
                lPremFinanceCnt = CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFCnt, 0))
                lPremFinanceVersion = CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFVersion, 0))

                'TR - Assume success
                result = gPMConstants.PMEReturnCode.PMTrue

                bDocumentCancelled = False

                'DC191004 PN15894 only get document_id for quote, credit agreement, confirmation & bank details
                'DC221004 PN15981 removed check for underwriting as agreed with SJ that ok
                If v_enDocType < 5 Then
                    'TR - Get the document
                    'TR - 24/03/03 - TS17 Recovery By Instalments changes - added lDocTemplateID

                    m_lReturn = g_oBusiness.GetDocDetail(v_enDocType, lPremFinanceCnt, lPremFinanceVersion, lDocTemplateID, lDocTypeID, sExpectedCode)

                    'TR - Make sure that this worked OK
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to get document " &
                                           "template id and document type id" & Strings.Chr(13) & Strings.Chr(10) &
                                           (IIf(sExpectedCode.Trim() <> "", " Document Code: " & sExpectedCode, "")) &
                                           (IIf(m_lReturn = gPMConstants.PMEReturnCode.PMNotFound, " (Not Found)", "")), ACApp, ACClass, "PrintDocument", Information.Err().Number, Information.Err().Description)
                        Return result
                    End If

                    'DC191004 PN15894 otherwise other types use needs to select specific document
                ElseIf v_enDocType = Me.PrintDocType.DocTypePCLSG Then
                    m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanRefundType, 0) = cboRefundType.Text
                    m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCompanyNo, 0) = txtCompanyReg.Text
                    m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientAddress1, 0) = addClient.AddressLine1
                    m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientAddress2, 0) = addClient.AddressLine2
                    m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientAddress3, 0) = addClient.AddressLine3
                    m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientAddress4, 0) = addClient.AddressLine4
                    m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientPostcode, 0) = addClient.PostCode

                    m_lReturn = PrintSGDocument(lPremFinanceCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    End If

                    Return result
                Else

                    Dim temp_oFindDocTemplate As Object = Nothing
                    m_lReturn = g_oObjectManager.GetInstance(temp_oFindDocTemplate, "iPMBFindDocTemplate.Interface_Renamed", gPMConstants.PMGetLocalInterface)
                    oFindDocTemplate = temp_oFindDocTemplate

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to create " & "iPMBFindDocTemplate object", ACApp, ACClass, "PrintDocument", Information.Err().Number, Information.Err().Description)
                        Return result
                    End If

                    With oFindDocTemplate

                        m_lReturn = .Start()

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If .Status <> gPMConstants.PMEReturnCode.PMCancel Then

                            lDocTemplateID = .DocumentTemplateId

                        Else

                            bDocumentCancelled = True

                        End If

                        .Dispose()

                    End With

                    oFindDocTemplate = Nothing

                End If

                'DC191004 PN15894 do not try and process document if no template to use
                If bDocumentCancelled Then
                    Return result
                End If

                'TR - Get the Insurance File count for this Policy

                m_lReturn = g_oBusiness.GetPolicyID(lPremFinanceCnt, lPremFinanceVersion, lInsuranceFileCnt, lInsuranceFolderCnt)

                'Plans with multiple policies will not return any policy details.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to get Policy IDs for " & "this premium finance plan", ACApp, ACClass, "PrintDocument", Information.Err().Number, Information.Err().Description)
                    Return result
                End If

                'TR - Create a business object
                Dim temp_oDocTemplate As Object = Nothing
                m_lReturn = g_oObjectManager.GetInstance(temp_oDocTemplate, "iPMBDocTemplate.Interface_Renamed", gPMConstants.PMGetLocalInterface)
                oDocTemplate = temp_oDocTemplate

                'TR - Make sure that this worked OK
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to create " & "iPMBDocTemplate object", ACApp, ACClass, "PrintDocument", Information.Err().Number, Information.Err().Description)
                    Return result
                End If

                With oDocTemplate

                    .PartyCnt = lpartyCnt

                    .InsuranceFolderCnt = lInsuranceFolderCnt

                    .InsuranceFileCnt = lInsuranceFileCnt
                    'Pass the lPremFinanceCnt into the ClaimCnt so we know exactly which one we are printing against.

                    .DocumentRef = CStr(lPremFinanceCnt)

                    Select Case v_enDocType
                        Case PrintDocType.DocTypeQuote

                            .SpoolDesc = "Instalment Quote"
                        Case PrintDocType.DocTypeBank

                            .SpoolDesc = "Instalment Bank Details"
                        Case PrintDocType.DocTypeCredit

                            .SpoolDesc = "Instalment Credit Agreement"
                            'TR - 24/03/03 - TS17 Recovery By Instalments changes
                        Case PrintDocType.DocTypeConfirmation

                            .SpoolDesc = "Instalment Confirmation"
                        Case PrintDocType.DocTypeInstalmentPlan

                            .SpoolDesc = "Instalment Plan"
                        Case PrintDocType.DocTypeInstalmentCancel

                            .SpoolDesc = "Cancel Instalment Plan"
                        Case PrintDocType.DocTypeInstalmentSettle

                            .SpoolDesc = "Settle Instalment Plan"
                        Case PrintDocType.DocTypeInstalmentEdit

                            .SpoolDesc = "Edit Instalment Plan"
                    End Select

                    .DocumentTemplateId = lDocTemplateID
                    If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanSchemePrintType, 0)).ToUpper() = "DIR" Then

                        .Mode = 3 'print document
                    Else

                        .Mode = 4 'spool document
                    End If

                    'TR - 24/03/03 - TS17 Recovery By Instalments changes
                    'TR - Get the interface to do the archiving
                    If v_bArchiveToDocumaster Then

                        .ArchiveAfterPrinting = True

                        .DocumentTypeId = 3 '3 = Claims file
                        'TR - Todo - find out description

                        .DocumentDescription = ""
                    Else

                        .ArchiveAfterPrinting = False

                        .DocumentTypeId = lDocTypeID
                    End If

                    ' HG24072003 : Overide all scenario's for the following doc types
                    If v_enDocType = Me.PrintDocType.DocTypeInstalmentPlan Or v_enDocType = Me.PrintDocType.DocTypeInstalmentCancel Or v_enDocType = Me.PrintDocType.DocTypeInstalmentSettle Or v_enDocType = Me.PrintDocType.DocTypeInstalmentEdit Then

                        .DocumentTypeId = lDocTypeID

                        .ArchiveAfterPrinting = True
                        'DC221004 PN15981 these docs can be editted and from then on be archived or spooled when prompted
                        '               .mode = 4

                        .Mode = 1
                    End If

                    m_lReturn = .Start()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    .Dispose()
                End With
                oDocTemplate = Nothing
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PrintDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '************************************************************************
    ' Name:         CreateRecoveryDocuments
    ' Description:  Use this function to call PrintDocument multiple times,
    '               one for each Doc Type. Used for Recovery By Instalments'
    ' History:      TR - 24/03/03 Created for TS17 Recovery By Instalments
    '************************************************************************

    Private Function CreateRecoveryDocuments(ByVal v_dtRecInitDate As Date, ByVal v_sRecInitCode As String, ByVal v_bCreateWorkTask As Boolean, Optional ByVal v_sPMWrkTaskDescription As String = "", Optional ByVal v_dtPMWrkTaskDueDate As Object = "00:00:00", Optional ByVal v_lClientID As Integer = 0, Optional ByVal v_dtExpectedate As Object = "00:00:00", Optional ByVal v_vOutcome As String = "", Optional ByVal v_dtOutcomeDate As Object = "00:00:00") As Integer

        Dim result As Integer = 0
        Const k_sFUNCTION_NAME As String = "CreateRecoveryDocuments"

        Try

            'TR - Assume success
            result = gPMConstants.PMEReturnCode.PMTrue

            'TR - Is there a Quote Doc template set on the Scheme?

            If Not (Convert.IsDBNull(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanQuoteDocID, 0)) Or IsNothing(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanQuoteDocID, 0))) Then

                'TR - 11.d.ii.1.a.i & 11.e.iii.1.a.i.1
                'Print the Quote doc & store in Documaster
                m_lReturn = PrintDocument(PrintDocType.DocTypeQuote, True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Could not print out Quote " & "Doc for this Plan", ACApp, ACClass, k_sFUNCTION_NAME)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'TR - 11.d.ii.1.a.ii & 11.e.iii.1.a.i.2
                'Create a Recovery Initiative for each doc
                'TR - TODO - see Dave NEwson or Russell Hill

                If v_bCreateWorkTask Then
                    'TR - 11.d.ii.1.a.iii - Create a  Work Manager Task for each document
                    m_lReturn = CreateWorkManagerTask(1, "PFMAINT", CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientName, 0)), v_dtPMWrkTaskDueDate, CInt(Conversion.Val(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanReviewPMuserGroupID, 0)))), v_sPMWrkTaskDescription, gPMConstants.PMEWrkManTaskStatus.pmeWMTSNew, 0, 0, , g_iUserId, , 1)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to Create a Work " & "Manager Task", ACApp, ACClass, k_sFUNCTION_NAME)
                        Return result
                    End If
                End If
            End If

            'TR - Is there a Bank Doc template set on the Scheme?

            If Not (Convert.IsDBNull(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankDocID, 0)) Or IsNothing(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanBankDocID, 0))) Then
                'TR - 11.d.ii.1.a.i - Print the Bank doc
                m_lReturn = PrintDocument(PrintDocType.DocTypeBank, True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Could not print out Bank " & "Doc for this Plan", ACApp, ACClass, k_sFUNCTION_NAME)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'TR - 11.d.ii.1.a.ii - Create a Recovery Initiative
                'TR - TODO - see Dave NEwson or Russell Hill
                If v_bCreateWorkTask Then
                    'TR - 11.d.ii.1.a.iii - Create a  Work Manager Task for each document
                    m_lReturn = CreateWorkManagerTask(1, "PFMAINT", CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientName, 0)), v_dtPMWrkTaskDueDate, CInt(Conversion.Val(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanReviewPMuserGroupID, 0)))), v_sPMWrkTaskDescription, gPMConstants.PMEWrkManTaskStatus.pmeWMTSNew, 0, 0, , g_iUserId, , 1)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to Create a Work " & "Manager Task", ACApp, ACClass, k_sFUNCTION_NAME)
                        Return result
                    End If
                End If
            End If

            'TR - Is there a Credit Doc template set on the Scheme?

            If Not (Convert.IsDBNull(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCreditDocID, 0)) Or IsNothing(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCreditDocID, 0))) Then
                'TR - 11.d.ii.1.a.i - Print the Credit doc
                m_lReturn = PrintDocument(PrintDocType.DocTypeCredit, True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Could not print out Credit " & "Doc for this Plan", ACApp, ACClass, k_sFUNCTION_NAME)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'TR - 11.d.ii.1.a.ii - Create a Recovery Initiative
                'TR - TODO - see Dave NEwson or Russell Hill
                If v_bCreateWorkTask Then
                    'TR - 11.d.ii.1.a.iii - Create a  Work Manager Task for each document
                    m_lReturn = CreateWorkManagerTask(1, "PFMAINT", CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientName, 0)), v_dtPMWrkTaskDueDate, CInt(Conversion.Val(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanReviewPMuserGroupID, 0)))), v_sPMWrkTaskDescription, gPMConstants.PMEWrkManTaskStatus.pmeWMTSNew, 0, 0, , g_iUserId, , 1)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to Create a Work " & "Manager Task", ACApp, ACClass, k_sFUNCTION_NAME)
                        Return result
                    End If
                End If
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "CreateRecoveryDocuments Failed", ACApp, ACClass, k_sFUNCTION_NAME, Information.Err().Number, excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    '************************************************************************
    ' Name:         CreateConfirmationDocument
    ' Description:  This function calls PrintDocument once, to create the
    '               confirmation Type Doc. Used for Recovery By Instalments
    ' History:      TR - 24/03/03 Created for TS17 Recovery By Instalments
    '************************************************************************
    Private Function CreateConfirmationDocument() As Integer

        Dim result As Integer = 0
        Const k_sFUNCTION_NAME As String = "CreateConfirmationDocument"

        Try

            'TR - Assume success
            result = gPMConstants.PMEReturnCode.PMTrue

            'TR - Make sure that there is a ConfirmationDoc template set on the Scheme?

            If Not (Convert.IsDBNull(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanConfirmationDocID, 0)) Or IsNothing(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanConfirmationDocID, 0))) Then
                'TR - 11.e.v - Print the Confirmation doc and Store it in Documaster
                m_lReturn = PrintDocument(PrintDocType.DocTypeConfirmation, True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Could not print out Quote " & "Doc for this Plan", ACApp, ACClass, k_sFUNCTION_NAME)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'TR - Inform the user
                MsgBoxSirius("A confirmation document will be" & Strings.Chr(13) & Strings.Chr(10) & "printed for the Debtor", MsgBoxStyle.OkOnly, "Document Printed")

                'TR - 11.e.vi - Create a Recovery Initiative for each doc
                'TR - TODO - see Dave NEwson or Russell Hill
                'Date = Now
                'Code = "Instalment Plan Created"
                'ExpectedDate = now
                'Outcome = successful
                'outcome_date = Now
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "CreateConfirmationDocument Failed", ACApp, ACClass, k_sFUNCTION_NAME, Information.Err().Number, excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Sub LoadStatusCombo()
        cboStatus.Items.Add("Saved")
        VB6.SetItemData(cboStatus, 0, CInt("010"))
        cboStatus.Items.Add("Updated")
        VB6.SetItemData(cboStatus, 1, CInt("011"))
        cboStatus.Items.Add("Printed")
        VB6.SetItemData(cboStatus, 2, CInt("012"))
        cboStatus.Items.Add("Live")
        VB6.SetItemData(cboStatus, 3, CInt("040"))
        cboStatus.Items.Add("On Hold")
        VB6.SetItemData(cboStatus, 4, CInt("140"))
        cboStatus.Items.Add("Completed")
        VB6.SetItemData(cboStatus, 5, CInt("900"))
        cboStatus.Items.Add("Superseded")
        VB6.SetItemData(cboStatus, 6, CInt("990"))
        cboStatus.Items.Add("Cancelled")
        VB6.SetItemData(cboStatus, 7, CInt("999"))
    End Sub

    Sub SetCombo(ByRef Cbo As ComboBox, ByVal vValue As Object)

        For n As Integer = 0 To Cbo.Items.Count - 1

            If VB6.GetItemData(Cbo, n) = CDbl(vValue) Then
                Cbo.SelectedIndex = n
                Exit For
            End If
        Next

    End Sub

    Public Function CreditCardDetailsList() As Integer
        Dim result As Integer = 0
        Try

            ' RAM20030404 : Issue 2915 Changes
            ' We don't need to fetch the Credit Card details, if we have those
            ' details already set.
            If m_bUseExistingCreditCardDetails And Information.IsArray(m_vCreditCardDetails) Then
            Else

                g_oBusiness.CreditCardDetailsList(m_lClientID, m_vCreditCardDetails)
            End If

            With cboCopyFromCard
                .Items.Clear()
                ' RAM20030408   : Check if we have an Array
                If Information.IsArray(m_vCreditCardDetails) Then

                    For nRow As Integer = m_vCreditCardDetails.GetLowerBound(1) To m_vCreditCardDetails.GetUpperBound(1)
                        ' Prakash: Removed codes related to m_bIsCCAuthorisationOff -(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)

                        If CStr(m_vCreditCardDetails(1, nRow)).Length - 4 > 0 Then
                            .Items.Add("**** **** **** " & CStr(m_vCreditCardDetails(1, nRow)).Substring(CStr(m_vCreditCardDetails(1, nRow)).Length - 4))
                        End If
                    Next nRow
                End If
            End With

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreditCardDetailsList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreditCardDetailsList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function BankAccountDetailsList() As Integer
        Dim result As Integer = 0
        Try

            ' RAM20030404 : Issue 2915 Changes
            ' We don't need to fetch the bank account details, if we have those
            ' details already set.
            If m_bUseExistingBankDetails And Information.IsArray(m_vBankAccountDetails) Then
            Else
                g_oBusiness.BankAccountDetailsList(m_lClientID, m_vBankAccountDetails)
            End If

            With cboCopyFromBank
                .Items.Clear()
                ' RAM20030408   : Check if we have an Array
                If Information.IsArray(m_vBankAccountDetails) Then

                    For nRow As Integer = m_vBankAccountDetails.GetLowerBound(1) To m_vBankAccountDetails.GetUpperBound(1)

                        .Items.Add(CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankSortCode, nRow)) & " - " &
                                   CStr(m_vBankAccountDetails(bSIRPremFinConst.k_PFPlanClientBankAccountNo, nRow)))
                    Next nRow
                End If
            End With

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BankAccountDetailsList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BankAccountDetailsList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetResString
    '
    ' Parameters: n/a
    '
    ' Description: Returns string item from resource file
    '
    ' History:
    '           Created : MEvans : Date : Process Id
    ' ***************************************************************** '
    Private Function GetResString(ByVal v_lItemId As Integer) As String

        Dim result As String = String.Empty
        Const sFunctionName As String = "GetResString"

        Dim sReturn As String = ""

        Try

            ' always want to return a string

            sReturn = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=v_lItemId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return sReturn

        Catch excep As System.Exception

            result = "Error"

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name          : EnableBankRelatedFields
    ' Description   : Private Function to diable the Bank Account Related
    '                   fields, so that the users can't change them
    ' Parameters    : v_bTrueOrFalse : Boolean, This will determined the
    '                                  state of the controls
    ' Author        : Ram Chandrabose
    ' Note          : Ref. Issue 2915
    ' Edit Histroy  :
    ' RAM20030404   : Created
    ' ***************************************************************** '
    Private Function EnableBankRelatedFields(ByVal v_bTrueOrFalse As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            txtBankName.Enabled = v_bTrueOrFalse
            txtSortCode.Enabled = v_bTrueOrFalse
            txtAccountNumber.Enabled = v_bTrueOrFalse
            txtAccountName.Enabled = v_bTrueOrFalse
            txtBranch.Enabled = v_bTrueOrFalse
            addBank.Enabled = v_bTrueOrFalse
            txtAreaCode.Enabled = v_bTrueOrFalse
            txtNumber.Enabled = v_bTrueOrFalse
            txtExtension.Enabled = v_bTrueOrFalse
            txtFaxAreaCode.Enabled = v_bTrueOrFalse
            txtFaxNumber.Enabled = v_bTrueOrFalse
            txtBIC.Enabled = v_bTrueOrFalse
            txtIBAN.Enabled = v_bTrueOrFalse
            ' Copy from Bank
            cboCopyFromBank.Enabled = v_bTrueOrFalse

            ' Startting date
            txtStartDate.Enabled = v_bTrueOrFalse

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnableBankRelatedFields Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnableBankRelatedFields", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name          : EnableCreditCardRelatedFields
    ' Description   : Private Function to diable the Credit Card Related
    '                   fields, so that the users can't change them
    ' Parameters    : v_bTrueOrFalse : Boolean, This will determined the
    '                                  state of the controls
    ' Author        : Ram Chandrabose
    ' Note          : Ref. Issue 2915
    ' Edit Histroy  :
    ' RAM20030404   : Created
    ' ***************************************************************** '
    Private Function EnableCreditCardRelatedFields(ByVal v_bTrueOrFalse As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            txtCardNo.Enabled = v_bTrueOrFalse
            txtCardName.Enabled = v_bTrueOrFalse
            txtExpiryDate.Enabled = v_bTrueOrFalse
            txtCardStartDate.Enabled = v_bTrueOrFalse
            txtIssueNo.Enabled = v_bTrueOrFalse
            txtPin.Enabled = v_bTrueOrFalse
            txtCardName.Enabled = v_bTrueOrFalse

            ' Copy from Credit Card
            Me.cboCopyFromCard.Enabled = v_bTrueOrFalse

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnableCreditCardRelatedFields Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnableCreditCardRelatedFields", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Private Function DisableButtons() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DisableButtons"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cmdRePrint.Enabled = False
            cmdSettlePlan.Enabled = False
            cmdMTA.Enabled = False
            cmdCancel.Enabled = False
            cmdSave.Enabled = False
            cmdTransact.Enabled = False
            cmdDelete.Enabled = False
            cmdRelease.Enabled = False
            cmdReSend.Enabled = False
            cmdNavigate.Enabled = False



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sUsername:=g_sUserName, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here



            ' This is for debugging only



        End Try
        Return result
    End Function

    '************************************************************************
    ' Name:         ProcessThirdPartyRecovery
    ' Description:  Does the "extra" processing required for RecoveryByIntalments
    ' History:      TR - 07/04/03 Created for TS17 Recovery By Instalments
    '************************************************************************
    Private Function ProcessThirdPartyRecovery() As Integer

        Dim result As Integer = 0
        Dim enMsgBoxResult As DialogResult

        Const k_sFUNCTION_NAME As String = "ProcessThirdPartyRecovery"

        Try

            'TR - Initialise Required Variables
            result = gPMConstants.PMEReturnCode.PMTrue

            'TR - 11.d - If the date_confirmed is blank

            If Convert.IsDBNull(dtpConfirmedDate.Value) Or IsNothing(dtpConfirmedDate.Value) Then

                '        'TR - TODO when the claims side is ready - page 10, section 11.d.i
                '        'TR - 11.d.i - If Recovery Initiative does not exist, then create one
                '        m_lReturn = CreateRecoveryInitiative(Date = now, Code = "
                '       Instalment Plan Created", PartyId = m_lPartyID, Outcome = blank)
                '        If m_lReturn <> PMTrue Then
                '            ProcessThirdParty = PMFalse
                '            Call LogMessage(PMLogOnError, "Cannot Create Recovery " _
                ''                & "initiative", ACApp, ACClass, _
                ''                k_sFUNCTION_NAME)
                '            Exit Function
                '        End If

                'TR - 24/03/03 - TS17 Recovery By Instalments changes
                'TR - 11.d.ii - User Is asked to confirm the creation of all
                'documents as this is optional
                enMsgBoxResult = MsgBoxSirius("Do you wish to print the payment" & Strings.Chr(13) & Strings.Chr(10) & "documents?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Print Documents")
                If enMsgBoxResult = System.Windows.Forms.DialogResult.Yes Then
                    'TR - 11.d.ii.1 - For each doc, store the doc and create a
                    'Recovery Initiative
                    m_lReturn = CreateRecoveryDocuments(DateTime.Now, "PFMAINT", True, "Deduction Authority Issued", , m_lClientID)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to print " & "claim details", ACApp, ACClass, k_sFUNCTION_NAME)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                'TR - Otherwise Confirmed_Date IS set
            Else
                'TR - 11.e - If the date_confirmed is set and Status is NOT Live...
                If m_sStatusInd <> bSIRPremFinConst.PFStatusIndLive Then
                    'TR - 11.e.i - Change the Status to Live now (in local variable and array)
                    m_sStatusInd = bSIRPremFinConst.PFStatusIndLive
                    m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStatusInd, 0) = bSIRPremFinConst.PFStatusIndLive
                    'TR - 11.e.ii - If this is the first version.....
                    If Conversion.Val(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanParentPlanVersion, 0))) = 1 Then
                        'TR - 11.e.ii.1 - Set the Last Statement Date

                        m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDateLastStatement, 0) = dtpConfirmedDate.Value
                    End If
                    'TR - 11.e.iii - If the CC/Bank Details have changed for this or
                    'from previous version then
                    If m_bCreditCardOrBankDetailsChanged Then
                        'TR - 11.e.iii.1 - User Is asked to confirm the creation of all
                        'documents as this is optional
                        enMsgBoxResult = MsgBoxSirius("Do you wish to print the payment" & Strings.Chr(13) & Strings.Chr(10) & "documents?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Print Documents")
                        If enMsgBoxResult = System.Windows.Forms.DialogResult.Yes Then
                            'TR - 11.e.iii.1.a - For each doc, store the doc and
                            'create a Recovery Initiative
                            m_lReturn = CreateRecoveryDocuments(DateTime.Now, "Deduction Authority issued", False, , , m_lClientID)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to print " & "claim details", ACApp, ACClass, k_sFUNCTION_NAME)
                                'ProcessThirdPartyRecovery = PMFalse
                                'Exit Function
                            End If
                        End If
                    End If

                    'TR - 11.e.iv - Produce the confirmation document set on PFScheme.
                    m_lReturn = CreateConfirmationDocument()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to create " & "Confirmation document", ACApp, ACClass, k_sFUNCTION_NAME)
                        'ProcessThirdPartyRecovery = PMFalse
                        'Exit Function
                    End If
                End If

                'TR - 11.f If the Date_confirmed is set and the review_date changed
                If m_bReviewDateChanged Then
                    'TR - 11.f.i - Add Event Log to state that the review Date was changed
                    CreateEventLogEntry("Plan Ref : " & txtReference.Text & ". " & "Review Date has been changed ")

                    'TR - 11.f.ii & iii - If pmwrk_tasks_instance_id is Null or Task
                    'Instance is complete
                    m_lReturn = ManageReviewWorkTaskInstance()
                End If
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to Process the Third Party " & "Recovery specific rules", ACApp, ACClass, k_sFUNCTION_NAME, Information.Err().Number, excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    '************************************************************************
    ' Name:         CreateworkManagerTask
    ' Description:  Creates a Task in Work Manager
    ' History:      TR - 07/04/03 Created for TS17 Recovery By Instalments
    '************************************************************************
    Private Function CreateWorkManagerTask(ByVal v_lPMWrkTaskGroupID As Integer, ByVal v_sPMWrkTaskCode As String, ByVal v_sCustomer As String, ByVal v_dtTaskDueDate As Date, ByVal v_lPMUserGroupID As Integer, ByVal v_sDescription As String, ByVal v_iTaskStatus As Integer, ByVal v_iIsUrgent As Integer, ByRef r_lPMWrkTaskInstanceCnt As Integer, Optional ByRef v_sWorkflowInformation As String = "", Optional ByVal v_iUserID As Integer = 0, Optional ByVal v_vKeyArray(,) As Object = Nothing, Optional ByVal v_iIsVisible As Integer = gPMConstants.PMEReturnCode.PMTrue) As Integer

        Dim result As Integer = 0
        Dim obPMWrkTaskInstance As bPMWrkTaskInstance.TaskControl

        Try

            'TR -  Initialise Required Variables
            result = gPMConstants.PMEReturnCode.PMTrue

            'Create the business Object
            obPMWrkTaskInstance = New bPMWrkTaskInstance.TaskControl()

            'Initialise with the Sirius user and password
            m_lReturn = CType(obPMWrkTaskInstance, SSP.S4I.Interfaces.IBusiness).Initialise(sUsername:=g_oObjectManager.UserName, sPassword:=g_oObjectManager.Password, iUserID:=g_oObjectManager.UserID, iSourceID:=g_oObjectManager.SourceID, iLanguageID:=g_oObjectManager.LanguageID, iCurrencyID:=g_oObjectManager.CurrencyID, iLogLevel:=g_oObjectManager.LogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to Initialise " & "workmanager Task Business Object", ACApp, ACClass, "CreateWorkManagerTask")
                Return result
            End If

            'TR - Create the WorkManager Task - just pass through the parameters
            'Create Using Code not the bloody ID which changes!
            m_lReturn = obPMWrkTaskInstance.CreateNewByCode(v_lPMWrkTaskGroupID, v_sPMWrkTaskCode, v_sCustomer, v_dtTaskDueDate, v_lPMUserGroupID, v_sDescription, v_iTaskStatus, v_iIsUrgent, r_lPMWrkTaskInstanceCnt, v_sWorkflowInformation, v_iUserID, v_vKeyArray, v_iIsVisible)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to Create Work Manager " & "Task.", ACApp, ACClass, "CreateWorkManagerTask")
                Return result
            End If

            obPMWrkTaskInstance = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to Create workmanager " & "Task.", ACApp, ACClass, "CreateWorkManagerTask", Information.Err().Number, excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    '************************************************************************
    ' Name:         ManageReviewWorkTaskInstance
    ' Description:  Creates a Task in Work Manager
    ' History:      TR - 07/04/03 Created for TS17 Recovery By Instalments
    '************************************************************************
    Private Function ManageReviewWorkTaskInstance() As Integer

        Dim result As Integer = 0

        'TODO:
        'Dim obPMWrkTaskInstance As bPMWrkTaskInstance.Business
        Dim obPMWrkTaskInstance As Object = Nothing
        Dim lPMWrkTaskGroupID, lPMWrkTaskID As Integer
        Dim sCustomer As String = ""
        Dim dtTaskDueDate As Date
        Dim lPMUserGroupID As Integer
        Dim iUserID As Integer
        Dim sDescription As String = ""
        Dim lTaskStatus As gPMConstants.PMEWrkManTaskStatus
        Dim iIsUrgent As Integer
        Dim dtDateCreated As Date
        Dim iCreatedByID As Integer
        Dim dtLastModified As Date
        Dim iModifiedByID As Integer
        Dim lNewReviewTaskInstanceID As Integer

        Const k_sFUNCTION_NAME As String = "ManageReviewWorkTaskInstance"

        Try

            'TR -  Initialise Required Variables
            result = gPMConstants.PMEReturnCode.PMTrue

            'TR - 11.f.ii - If pmwrk_tasks_instance_id is Null or Task Instance
            'is complete
            If Conversion.Val(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanReviewPMWorkTaskInstance, 0))) > 0 Then
                'TR - Create the work Task Instance object
                Dim temp_obPMWrkTaskInstance As Object = Nothing
                m_lReturn = g_oObjectManager.GetInstance(temp_obPMWrkTaskInstance, "bPMWrkTaskInstance.Business", gPMConstants.PMGetViaClientManager)
                obPMWrkTaskInstance = temp_obPMWrkTaskInstance
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to create Work " & "Task Instance", ACApp, ACClass, k_sFUNCTION_NAME)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'TR - Now get the Work Task Instance
                m_lReturn = obPMWrkTaskInstance.GetDetails(CInt(Conversion.Val(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanReviewPMWorkTaskInstance, 0)))), lPMWrkTaskGroupID, lPMWrkTaskID, sCustomer, dtTaskDueDate, lPMUserGroupID, iUserID, sDescription, lTaskStatus, iIsUrgent, dtDateCreated, iCreatedByID, dtLastModified, iModifiedByID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to get Work " & "Task Instance " & CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanReviewPMWorkTaskInstance, 0)), ACApp, ACClass, k_sFUNCTION_NAME)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'TR - 11.f.ii - If pmwrk_tasks_instance_id is Null or Task
            'Instance is complete
            If Conversion.Val(CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanReviewPMWorkTaskInstance, 0))) = 0 Or lTaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete Then
                'TR - 11.f.ii.1 - Create Work Manager Task
                m_lReturn = CreateWorkManagerTask(1, "PFMAINT", CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanClientName, 0)), CDate(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDateReview, 0)), CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanReviewPMuserGroupID, 0)), "Instalment Plan Review", gPMConstants.PMEWrkManTaskStatus.pmeWMTSNew, 0, lNewReviewTaskInstanceID, "", g_iUserId, , 1)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to create a Work " & "Manager Task", ACApp, ACClass, k_sFUNCTION_NAME)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'TR - 11.f.ii.2 - Set the review_pmwrk_task_instance_id
                'to new task instance
                m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanReviewPMWorkTaskInstance, 0) = lNewReviewTaskInstanceID
            Else
                'TR - 11.f.iii.1 - Locate and Update Work Manager Task
                'Check we've already got the Work Task Instance
                If Not (obPMWrkTaskInstance Is Nothing) Then
                    'TR - 11.f.iii.1.a
                    m_lReturn = obPMWrkTaskInstance.AmendDetails(lPMWrkTaskID, sCustomer, CDate(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanReviewPMuserGroupID, 0)), sDescription, iIsUrgent)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to update Work " & "Manager Task", ACApp, ACClass, k_sFUNCTION_NAME)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'TR - Destroy object
                    obPMWrkTaskInstance = Nothing

                    'TR - 11.f.iii.2 - Add Event Log for change of review date
                    CreateEventLogEntry("Plan Ref : " & txtReference.Text & ". " & "Review Date changed to " &
                                        CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanDateReview, 0)))
                Else
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "obPMWrkTaskInstance no " & "instantiated when it should be", ACApp, ACClass, k_sFUNCTION_NAME)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            obPMWrkTaskInstance = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to Create workmanager " & "Task.", ACApp, ACClass, "ManageReviewWorkTaskInstance", Information.Err().Number, excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    '************************************************************************
    ' Name:         CreateEventLogEntry
    ' Description:  Creates a Task in Work Manager
    ' History:      TR - 07/04/03 Created for TS17 Recovery By Instalments
    '************************************************************************
    Private Sub CreateEventLogEntry(ByVal v_sEventMessage As String)

        Dim obSIREvent As Object = Nothing

        Const k_sFUNCTION_NAME As String = "CreateEventLogEntry"

        Try

            'TR - Create the Event Log object

            m_lReturn = g_oObjectManager.GetInstance(obSIREvent, "bSIREvent." & "Business", gPMConstants.PMGetViaClientManager)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to Create the Event Log " & "Object", ACApp, ACClass, k_sFUNCTION_NAME)
                Exit Sub
            End If

            'TR - Add Event Log with the message passed in

            m_lReturn = obSIREvent.DirectAdd(vPartyCnt:=m_lPartyCnt, vEventType:=5, vUserid:=g_iUserId, vEventDate:=DateTime.Now, vDescription:=v_sEventMessage)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to update the Event Log", ACApp, ACClass, k_sFUNCTION_NAME)
            End If

            obSIREvent = Nothing

        Catch excep As System.Exception

            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to Add the Event Log Message " & v_sEventMessage, ACApp, ACClass, k_sFUNCTION_NAME, Information.Err().Number, excep.Message, excep:=excep)
            Exit Sub
        End Try
    End Sub

    '************************************************************************
    ' Name:         ApplyVersionHighlighting
    ' Description:  Creates a Task in Work Manager
    ' History:      TR - 07/04/03 Created for TS17 Recovery By Instalments
    '************************************************************************
    Private Sub ApplyVersionHighlighting(ByVal v_iPlanArrayConstant As Integer, ByRef r_objControl As Control)

        Try

            'TR - Compare values between the Current Plan Array (m_vFinancePlanArray)
            'and the Previous Plan array (m_vPreviousFinancePlanArray)
            If m_bUseVersionHighlighting Then
                If gPMFunctions.ToSafeString(m_vFinancePlanArray(v_iPlanArrayConstant, 0)).Trim.ToUpper <> gPMFunctions.ToSafeString(m_vPreviousFinancePlanArray(v_iPlanArrayConstant, 0)).Trim.ToUpper Then

                    'TR - 4.1.5.4.1 Set the background colour
                    If TypeOf r_objControl Is DateTimePicker Then
                        'TR - Do nothing
                    ElseIf TypeOf r_objControl Is CheckBox Then
                        'TR - Do nothing
                    Else
                        r_objControl.BackColor = SystemColors.GrayText
                        'r_objControl.ForeColor = vbInfoText
                    End If

                    'TR - 4.1.5.4.2 Set the tooltip to display the original value

                    'TODO-:
                    'r_objControl.ToolTipText = "Previous Version: " & CStr(m_vPreviousFinancePlanArray(v_iPlanArrayConstant, 0)).Trim()
                Else
                    'TR - 4.1.5.4.1 Un-Set the background colour
                    If TypeOf r_objControl Is DateTimePicker Then
                        'TR - Do nothing
                    ElseIf TypeOf r_objControl Is CheckBox Then
                        'TR - Do nothing
                    Else
                        r_objControl.BackColor = SystemColors.Window
                    End If

                    'TR - 4.1.5.4.2 Un-Set the tooltip

                    'TODO-:
                    'r_objControl.ToolTipText = ""
                End If
            End If

        Catch excep As System.Exception

            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to Apply Version Highlighting", ACApp, ACClass, "ApplyVersionHighlighting", Information.Err().Number, excep.Message, excep:=excep)
            Exit Sub

        End Try
    End Sub

    Private Sub chkNoStatements_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkNoStatements.CheckStateChanged
        SetStatementsCombo()
    End Sub
    Private Sub cmdAgentSelect_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAgentSelect.Click
        SelectAgent()
    End Sub
    Private Sub dtpConfirmedDate_ValueChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles dtpConfirmedDate.ValueChanged
        SetReviewedFromConfirmedDate()
    End Sub

    '************************************************************************
    ' Name:         CreateInstalmentsDocs
    ' Description:  Use this function to call PrintDocument, to print of
    '               instalment documents.
    ' History:      HG - 22/07/03
    '************************************************************************
    Private Function CreateInstalmentsDocs(ByVal v_eDocType As PrintDocType) As Integer

        Dim result As Integer = 0
        Const k_sFUNCTION_NAME As String = "CreateInstalmentsDocs"

        Dim sDocType As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = PrintDocument(v_eDocType, True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Could not print out Instalment/Quote " & "Doc for this Plan", ACApp, ACClass, k_sFUNCTION_NAME)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "CreateInstalmentsDocs Failed", ACApp, ACClass, k_sFUNCTION_NAME, Information.Err().Number, excep.Message, excep:=excep)
            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: SetQASDatabaseId
    '
    ' Description:
    '
    ' History: 20/08/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function SetQASDatabaseId() As Integer
        Dim result As Integer = 0
        Dim bSIROptions As Object = Nothing

        Const kMethodName As String = "SetQASDatabaseId"

        Dim oOptions As bSIROptions.Business = Nothing
        Dim sValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oOptions As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oOptions, "bSIROptions.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oOptions = temp_oOptions
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "sClassName:=bSIROptions.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = oOptions.GetOption(iOptionNumber:=13, sValue:=sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oOptions.GetOption", "iOptionNumber:=13", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Set the correct QAS database
            addClient.QASDatabaseID = gPMFunctions.ToSafeLong(sValue)
            addBank.QASDatabaseID = gPMFunctions.ToSafeLong(sValue)
            addCardholder.QASDatabaseID = gPMFunctions.ToSafeLong(sValue)
            uctPMAgentAddressControl.QASDatabaseID = gPMFunctions.ToSafeLong(sValue)

            If gPMFunctions.ToSafeLong(sValue) <> 0 Then
                addClient.PMDatabaseID = 0
                addBank.PMDatabaseID = 0
                addCardholder.PMDatabaseID = 0
                uctPMAgentAddressControl.PMDatabaseID = 0
            Else
                addClient.PMDatabaseID = 1
                addBank.PMDatabaseID = 1
                addCardholder.PMDatabaseID = 1
                uctPMAgentAddressControl.PMDatabaseID = 1
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here
            If Not (oOptions Is Nothing) Then

                oOptions.Dispose()
                oOptions = Nothing
            End If



            ' This is for debugging only



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: EnableDisableTabs
    '
    ' Description:
    '
    ' History: 20/08/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Private Sub EnableDisableTabs(ByVal v_iTab As Integer)

        Try

            fraSummary.Enabled = False
            fraDates.Enabled = False
            fraPayment.Enabled = False
            fraClient.Enabled = False
            fraBank.Enabled = False
            fraAccount.Enabled = False
            Frame1.Enabled = False
            lvwInstalment.Enabled = False
            fraAgentDetails.Enabled = False

            Select Case v_iTab
                Case 0
                    fraSummary.Enabled = True
                    fraDates.Enabled = True
                    fraPayment.Enabled = True
                Case 1
                    fraClient.Enabled = True
                Case 2
                    fraBank.Enabled = True
                    fraAccount.Enabled = True
                Case 3
                    Frame1.Enabled = True
                Case 4
                    lvwInstalment.Enabled = True
                Case 5
                    fraAgentDetails.Enabled = True
            End Select

        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnableDisableTabs Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnableDisableTabs", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Function PrintSGDocument(ByRef v_lPremFinanceCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            Dim sDocPath As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = g_oBusiness.GetSGDocumentURL(v_lPremFinanceCnt, m_vFinancePlanArray, sDocPath)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            If sDocPath.Trim() = "" Then
                Return result
            End If

            If Not (ShellEx(sDocPath)) Then
                Throw New Exception()
            End If

            Return result

        Catch

            Return result
        End Try
    End Function

    Public Function SilentTransaction() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            cmdTransact_Click(cmdTransact, New EventArgs())

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError
            gPMFunctions.LogMessageToFile(g_sUserName, gPMConstants.PMELogLevel.PMLogOnError, "Failed to transact silently", ACApp, ACClass, "SilentTransaction", excep:=ex)

        Finally

        End Try
        Return result
    End Function

    Public Function MsgBoxSirius(Optional ByVal prompt As Object = Nothing, Optional ByVal Buttons As MsgBoxStyle = MsgBoxStyle.OkOnly, Optional ByVal title As String = "", Optional ByVal HelpFile As Object = Nothing, Optional ByVal context As Object = Nothing) As DialogResult

        Dim result As DialogResult = System.Windows.Forms.DialogResult.OK
        If Information.IsNothing(title) Then title = Me.Text
        If m_lSilentTransact = 0 Then

            If Not Information.IsNothing(HelpFile) And Not Information.IsNothing(context) Then

                result = Interaction.MsgBox(CStr(prompt), Buttons, title)
            ElseIf Not Information.IsNothing(HelpFile) Then

                result = Interaction.MsgBox(CStr(prompt), Buttons, title)
            ElseIf Not Information.IsNothing(context) Then

                result = Interaction.MsgBox(CStr(prompt), Buttons, title)
            Else

                result = Interaction.MsgBox(CStr(prompt), Buttons, title)
            End If
        End If
        Return result
    End Function

    Private Function GetOutstandingBalance() As Decimal
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetOutstandingBalance
        ' PURPOSE: Returns the outstanding balance for the Plan
        ' AUTHOR: Danny Davis
        ' DATE: 24 October 2005, 16:27:15
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Decimal = 0
        Dim cOutstanding As Decimal

        Try

            m_lReturn = g_oBusiness.SettlePlanCalculate(v_vPremiumFinance:=m_vFinancePlanArray, r_crSettlement:=cOutstanding, r_crRefund:=0, r_dtNextInstalmentDate:=#12/30/1899#, r_dtNextInstalmentDatePlus1:=#12/30/1899#, r_dtLastInstalmentDate:=#12/30/1899#, r_dtLastPaidInstalmentDate:=#12/30/1899#, r_vSettlementFormatted:="")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", GetOutstandingBalance, Settle Plan Calculate Error")
            End If
            g_oBusiness.PlanOutstandingAmt = cOutstanding
            result = cOutstanding



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOutstandingBalance", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = 0

            End Select

        Finally


        End Try
        Return result
    End Function

    Private Function RollbackPlan() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: RollbackPlan
        ' PURPOSE: Rolls back a cancelled Instalment Plan Quote during the
        '          NB/MTA Roadmap in S4I
        ' AUTHOR: Danny Davis
        ' DATE: 28 March 2007, 16:18:06
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            If m_sTransactionType <> "MTC" Then
                'This only runs when in the S4I NB or MTA roadmap
                'PN: 51509, Check the Transaction Type also for m_sTransactionType = "G_NB"
                'to make this function work in case of New Installment Plan task too.
                If m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeMTAQuote Or m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeNBQuote Or m_sTransactionType = "G_NB" Or m_sTransType = "MTA" Then

                    m_lReturn = g_oBusiness.RollbackPlan(lPFFinancePlanCnt:=m_lFinancePlanCnt, lPFFinancePlanVersion:=m_lFinancePlanVersion)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Failed to display the inteface.
                        MsgBoxSirius("Failed to rollback MTA Plan", , "Rollback MTA Plan")
                        Return result
                    End If
                End If
            End If


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackPlan", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse


            End Select

        Finally

        End Try
        Return result
    End Function

    ''' <summary>
    ''' PopulateBankMediaTypeHistoryListView
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PopulateBankMediaTypeHistoryListView() As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "PopulateBankMediaTypeHistoryListView"

        Const kArrayPosMediaTypeCode As Integer = 2
        Const kArrayPosActionCode As Integer = 3
        Const kArrayPosDateModified As Integer = 33
        Const kArrayPosBankName As Integer = 7
        Const kArrayPosBranch As Integer = 8
        Const kArrayPosAccountName As Integer = 4
        Const kArrayPosBankAccountCode As Integer = 5
        Const kArrayPosAccountNumber As Integer = 6
        Const kArrayPosBIC As Integer = 38
        Const kArrayPosIBAN As Integer = 39
        Const kArrayPosUserName As Integer = 34
        Const kArrayPosAddress1 As Integer = 9
        Const kArrayPosPostCode As Integer = 13
        Const kArrayPosPaperDD As Integer = 35
        Const kArrayPosPaymentType As Integer = 36
        Const kArrayPosAccountType As Integer = 37

        Const kBankMediaTypeHistorySubItemDateModified As Integer = 1
        Const kBankMediaTypeHistorySubItemPaymentType As Integer = 2
        Const kBankMediaTypeHistorySubItemAccountType As Integer = 3
        Const kBankMediaTypeHistorySubItemBankName As Integer = 4
        Const kBankMediaTypeHistorySubItemBankBranch As Integer = 5
        Const kBankMediaTypeHistorySubItemBankAccountName As Integer = 6
        Const kBankMediaTypeHistorySubItemBankAccountCode As Integer = 7
        Const kBankMediaTypeHistorySubItemBankAccountNumber As Integer = 8
        Const kBankMediaTypeHistorySubItemBIC As Integer = 9
        Const kBankMediaTypeHistorySubItemIBAN As Integer = 10
        Const kBankMediaTypeHistorySubItemUsername As Integer = 11
        Const kBankMediaTypeHistorySubItemBankAddressLine1 As Integer = 12
        Const kBankMediaTypeHistorySubItemBankAddressPostCode As Integer = 13
        Const kBankMediaTypeHistorySubItemPaperDD As Integer = 14

        Dim nlBound, nUBound As Integer
        Dim oListItem As ListViewItem

        Dim sActionCode As String = ""
        Dim dtDateModified As Date
        Dim sBankName, sBankBranch, sBankAccountname, sBankAccountCode, sBankAccountNumber, sUsername, sAddressLine1, sBankPostCode As String
        Dim sBIC As String = String.Empty
        Dim sIBAN As String = String.Empty
        Dim bPaperDD As Boolean
        Dim sMediaTypeCode, sPaymentType, sAccountType As String

        Try

            lvwHistory.Items.Clear()

            If Information.IsArray(m_vMediaTypeHistory) Then

                nlBound = m_vMediaTypeHistory.GetLowerBound(1)
                nUBound = m_vMediaTypeHistory.GetUpperBound(1)
                For lRow As Integer = nlBound To nUBound

                    sMediaTypeCode = CStr(m_vMediaTypeHistory(kArrayPosMediaTypeCode, lRow)).Trim()

                    If sMediaTypeCode = kMediaTypeValidationCodeBank Then

                        ' columns
                        '1.  Action Code        3
                        '2.  Date Modified      33
                        '3.  Bank Name          7
                        '4.  Branch             8
                        '5.  Account Name       4
                        '6.  Bank Account Code  5
                        '7.  Account Number     6
                        '8.  User               34
                        '9.  Address 1          9
                        '10. Postcode           13
                        '11. PaperDD            35

                        sActionCode = CStr(m_vMediaTypeHistory(kArrayPosActionCode, lRow))
                        dtDateModified = gPMFunctions.ToSafeDate(m_vMediaTypeHistory(kArrayPosDateModified, lRow))
                        sBankName = CStr(m_vMediaTypeHistory(kArrayPosBankName, lRow))
                        sBankBranch = CStr(m_vMediaTypeHistory(kArrayPosBranch, lRow))
                        sBankAccountname = CStr(m_vMediaTypeHistory(kArrayPosAccountName, lRow))
                        sBankAccountCode = CStr(m_vMediaTypeHistory(kArrayPosBankAccountCode, lRow))
                        sBankAccountNumber = CStr(m_vMediaTypeHistory(kArrayPosAccountNumber, lRow))
                        sBIC = CStr(m_vMediaTypeHistory(kArrayPosBIC, lRow))
                        sIBAN = CStr(m_vMediaTypeHistory(kArrayPosIBAN, lRow))
                        sUsername = CStr(m_vMediaTypeHistory(kArrayPosUserName, lRow))
                        sAddressLine1 = CStr(m_vMediaTypeHistory(kArrayPosAddress1, lRow))
                        sBankPostCode = CStr(m_vMediaTypeHistory(kArrayPosPostCode, lRow))
                        bPaperDD = gPMFunctions.ToSafeBoolean(m_vMediaTypeHistory(kArrayPosPaperDD, lRow), False)
                        sPaymentType = CStr(m_vMediaTypeHistory(kArrayPosPaymentType, lRow))
                        sAccountType = CStr(m_vMediaTypeHistory(kArrayPosAccountType, lRow))

                        oListItem = lvwHistory.Items.Add(sActionCode)

                        ListViewHelper.GetListViewSubItem(oListItem, kBankMediaTypeHistorySubItemDateModified).Text = CStr(dtDateModified)
                        ListViewHelper.GetListViewSubItem(oListItem, kBankMediaTypeHistorySubItemPaymentType).Text = sPaymentType
                        ListViewHelper.GetListViewSubItem(oListItem, kBankMediaTypeHistorySubItemAccountType).Text = sAccountType
                        ListViewHelper.GetListViewSubItem(oListItem, kBankMediaTypeHistorySubItemBankName).Text = sBankName
                        ListViewHelper.GetListViewSubItem(oListItem, kBankMediaTypeHistorySubItemBankBranch).Text = sBankBranch
                        ListViewHelper.GetListViewSubItem(oListItem, kBankMediaTypeHistorySubItemBankAccountName).Text = sBankAccountname
                        ListViewHelper.GetListViewSubItem(oListItem, kBankMediaTypeHistorySubItemBankAccountCode).Text = sBankAccountCode
                        ListViewHelper.GetListViewSubItem(oListItem, kBankMediaTypeHistorySubItemBankAccountNumber).Text = sBankAccountNumber
                        ListViewHelper.GetListViewSubItem(oListItem, kBankMediaTypeHistorySubItemBIC).Text = sBIC
                        ListViewHelper.GetListViewSubItem(oListItem, kBankMediaTypeHistorySubItemIBAN).Text = sIBAN
                        ListViewHelper.GetListViewSubItem(oListItem, kBankMediaTypeHistorySubItemUsername).Text = sUsername
                        ListViewHelper.GetListViewSubItem(oListItem, kBankMediaTypeHistorySubItemBankAddressLine1).Text = sAddressLine1
                        ListViewHelper.GetListViewSubItem(oListItem, kBankMediaTypeHistorySubItemBankAddressPostCode).Text = sBankPostCode
                        ListViewHelper.GetListViewSubItem(oListItem, kBankMediaTypeHistorySubItemPaperDD).Text = CStr(bPaperDD)

                    End If

                    ' array fields
                    '0    pfprem_finance_cnt,
                    '1    pfprem_finance_version,
                    '2    mediatype_validation_code
                    '3    action_code,
                    '4    BankAccountName,
                    '5    BankSortCode,
                    '6    BankAccountNo,
                    '7    BankName,
                    '8    BankBranch,
                    '9    BankAddr1,
                    '10   BankAddr2,
                    '11   BankAddr3,
                    '12   BankTown,
                    '13   BankPCode,
                    '14   BankRegion,
                    '15   BankCountry,
                    '16   BankAreaCode,
                    '17   BankPhoneNo,
                    '18   BankExtension,
                    '19   BankFaxAreaCode,
                    '20   BankFaxNo,
                    '21   cc_number,
                    '22   cc_expiry_date,
                    '23   cc_start_date,
                    '24   cc_issue,
                    '25   cc_pin,
                    '26   cardholder_name,
                    '27   cardholder_address1,
                    '28   cardholder_address2,
                    '29   cardholder_address3,
                    '30   cardholder_address4,
                    '31   cardholder_postcode,
                    '32   user_id,
                    '33   date_modified
                    '34   username
                    '35   paperdd

                Next

                ListView6Autosize(lvwCCHistory, True)

            End If

        Catch ex As Exception



            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return nResult
    End Function

    ' ***************************************************************** '
    ' Name: PopulateCCMediaTypeHistoryListView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 09-05-2007 : Instalment_Import Changes
    ' ***************************************************************** '
    Public Function PopulateCCMediaTypeHistoryListView() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateCCMediaTypeHistoryListView"

        Const kArrayPosMediaTypeCode As Integer = 2
        Const kArrayPosActionCode As Integer = 3
        Const kArrayPosCCNumber As Integer = 21
        Const kArrayPosExpiryDate As Integer = 22
        Const kArrayPosStartDate As Integer = 23
        Const kArrayPosCardHolderName As Integer = 26
        Const kArrayPosCardHolderAddressLine1 As Integer = 27
        Const kArrayPosCardHolderPostCode As Integer = 31
        Const kArrayPosDateModified As Integer = 33
        Const kArrayPosUserName As Integer = 34
        Const kArrayPosPaperDD As Integer = 35
        Const kArrayPosBankAccountName As Integer = 4

        Const kCCMediaTypeHistorySubItemDateModified As Integer = 1
        Const kCCMediaTypeHistorySubItemCCNumber As Integer = 2
        Const kCCMediaTypeHistorySubItemCCStartDate As Integer = 3
        Const kCCMediaTypeHistorySubItemCCExpiryDate As Integer = 4
        Const kCCMediaTypeHistorySubItemCCCardHolderName As Integer = 5
        Const kCCMediaTypeHistorySubItemCCCardHolderAddressLine1 As Integer = 6
        Const kCCMediaTypeHistorySubItemCCCardHolderPostCode As Integer = 7
        Const kCCMediaTypeHistorySubItemUsername As Integer = 8

        Dim llBound, lUBound As Integer
        Dim oListItem As ListViewItem

        ' media type history fields
        Dim sActionCode As String = ""
        Dim dtDateModified As Date
        Dim sUsername, sCCNumber, sCCStartDate, sCCExpiryDate, sCardHolderName, sCardHolderAddressLine1, sCardHolderPostCode, sMediaTypeCode, sCCNumberToDisplay As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lvwCCHistory.Items.Clear()

            If Information.IsArray(m_vMediaTypeHistory) Then

                ' get array bounds
                llBound = m_vMediaTypeHistory.GetLowerBound(1)
                lUBound = m_vMediaTypeHistory.GetUpperBound(1)

                For lRow As Integer = llBound To lUBound

                    sMediaTypeCode = CStr(m_vMediaTypeHistory(kArrayPosMediaTypeCode, lRow)).Trim()

                    If sMediaTypeCode = kMediaTypeValidationCodeCreditCard Then

                        ' columns
                        '1  Action Code             3
                        '2  Date Modified           33
                        '3   cc_number,             21
                        '4   cc_expiry_date         22
                        '5   cc_start_date          23
                        '6   cardholder_name        26
                        '7   cardholder_address1    27
                        '8   cardholder_postcode    31

                        sActionCode = CStr(m_vMediaTypeHistory(kArrayPosActionCode, lRow))
                        If Information.IsDate(m_vMediaTypeHistory(kArrayPosDateModified, lRow)) Then
                            dtDateModified = CDate(m_vMediaTypeHistory(kArrayPosDateModified, lRow))
                        End If
                        sUsername = CStr(m_vMediaTypeHistory(kArrayPosUserName, lRow))

                        sCCNumber = CStr(m_vMediaTypeHistory(kArrayPosCCNumber, lRow))
                        sCCStartDate = CStr(m_vMediaTypeHistory(kArrayPosStartDate, lRow))
                        sCCExpiryDate = CStr(m_vMediaTypeHistory(kArrayPosExpiryDate, lRow))
                        sCardHolderName = CStr(m_vMediaTypeHistory(kArrayPosCardHolderName, lRow))

                        If sCardHolderName = "" Then
                            ' get the bank account name as cardholder name is blank
                            sCardHolderName = CStr(m_vMediaTypeHistory(kArrayPosBankAccountName, lRow))
                        End If

                        sCardHolderAddressLine1 = CStr(m_vMediaTypeHistory(kArrayPosCardHolderAddressLine1, lRow))
                        sCardHolderPostCode = CStr(m_vMediaTypeHistory(kArrayPosCardHolderPostCode, lRow))
                        'If sCCNumber.Length > 3 Then
                        '    sCCNumberToDisplay = "**** **** **** " & sCCNumber.Substring(sCCNumber.Length - 4)
                        'Else
                        '    sCCNumberToDisplay = "**** **** **** "
                        'End If
                        sCCNumberToDisplay = "**** **** **** " + Strings.Right(sCCNumber, 4)
                        oListItem = lvwCCHistory.Items.Add(sActionCode)

                        ListViewHelper.GetListViewSubItem(oListItem, kCCMediaTypeHistorySubItemDateModified).Text = CStr(dtDateModified)
                        ListViewHelper.GetListViewSubItem(oListItem, kCCMediaTypeHistorySubItemCCNumber).Text = sCCNumberToDisplay
                        ListViewHelper.GetListViewSubItem(oListItem, kCCMediaTypeHistorySubItemCCStartDate).Text = sCCStartDate
                        ListViewHelper.GetListViewSubItem(oListItem, kCCMediaTypeHistorySubItemCCExpiryDate).Text = sCCExpiryDate
                        ListViewHelper.GetListViewSubItem(oListItem, kCCMediaTypeHistorySubItemCCCardHolderName).Text = sCardHolderName
                        ListViewHelper.GetListViewSubItem(oListItem, kCCMediaTypeHistorySubItemCCCardHolderAddressLine1).Text = sCardHolderAddressLine1
                        ListViewHelper.GetListViewSubItem(oListItem, kCCMediaTypeHistorySubItemCCCardHolderPostCode).Text = sCardHolderPostCode
                        ListViewHelper.GetListViewSubItem(oListItem, kCCMediaTypeHistorySubItemUsername).Text = sUsername

                    End If

                    ' array fields
                    '0    pfprem_finance_cnt,
                    '1    pfprem_finance_version,
                    '2    mediatype_validation_cod
                    '3    action_code,
                    '4    BankAccountName,
                    '5    BankSortCode,
                    '6    BankAccountNo,
                    '7    BankName,
                    '8    BankBranch,
                    '9    BankAddr1,
                    '10   BankAddr2,
                    '11   BankAddr3,
                    '12   BankTown,
                    '13   BankPCode,
                    '14   BankRegion,
                    '15   BankCountry,
                    '16   BankAreaCode,
                    '17   BankPhoneNo,
                    '18   BankExtension,
                    '19   BankFaxAreaCode,
                    '20   BankFaxNo,
                    '21   cc_number,
                    '22   cc_expiry_date,
                    '23   cc_start_date,
                    '24   cc_issue,
                    '25   cc_pin,
                    '26   cardholder_name,
                    '27   cardholder_address1,
                    '28   cardholder_address2,
                    '29   cardholder_address3,
                    '30   cardholder_address4,
                    '31   cardholder_postcode,
                    '32   user_id,
                    '33   date_modified
                    '34   username
                    '35   paperdd

                Next

                ListView6Autosize(lvwHistory, True)

            End If
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetupBankMediaTypeHistoryListView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 09-05-2007 : Instalment_Import Changes
    ' ***************************************************************** '
    Public Sub SetupBankMediaTypeHistoryListView()

        Const kMethodName As String = "SetupBankMediaTypeHistoryListView"

        Dim lSubReturn As Integer

        Try

            lvwHistory.Columns.Clear()

            ' developer guide no. 210
            lvwHistory.Columns.Add(colHKeyActionCode, "Action Code", 1500, HorizontalAlignment.Left, HistoryListViewColIndex.AccountCode)

            lvwHistory.Columns.Add(colHKeyDateModified, "Date Modified", 1500, HorizontalAlignment.Left, HistoryListViewColIndex.DateModified)

            lvwHistory.Columns.Add(colHKeyPaymentType, "Payment Type", 1500, HorizontalAlignment.Left, HistoryListViewColIndex.PaymentType)

            lvwHistory.Columns.Add(colHKeyAccountType, "Account Type", 1500, HorizontalAlignment.Left, HistoryListViewColIndex.AccountType)

            lvwHistory.Columns.Add(colHKeyBankName, "Bank Name", 1500, HorizontalAlignment.Center, HistoryListViewColIndex.Name_Renamed)

            lvwHistory.Columns.Add(colHKeyBranch, "Branch", 1500, HorizontalAlignment.Center, HistoryListViewColIndex.Branch)

            lvwHistory.Columns.Add(colHKeyAccountName, "Account Name", 1500, HorizontalAlignment.Center, HistoryListViewColIndex.AccountName)

            lvwHistory.Columns.Add(colHKeyBankAccountCode, "Bank Account Code", 1500, HorizontalAlignment.Center, HistoryListViewColIndex.AccountCode)

            lvwHistory.Columns.Add(colHKeyAccountNumber, "Account Number", 1500, HorizontalAlignment.Center, HistoryListViewColIndex.AccountNumber)

            lvwHistory.Columns.Add(colHKeyAccountNumber, "BIC", 1500, HorizontalAlignment.Center, HistoryListViewColIndex.AccountNumber)

            lvwHistory.Columns.Add(colHKeyAccountNumber, "IBAN", 1500, HorizontalAlignment.Center, HistoryListViewColIndex.AccountNumber)

            lvwHistory.Columns.Add(colHKeyUser, "User", 1500, HorizontalAlignment.Center, HistoryListViewColIndex.User)

            lvwHistory.Columns.Add(colHKeyAddressLine1, "Address Line 1", 1500, HorizontalAlignment.Center, HistoryListViewColIndex.AddressLine1)

            lvwHistory.Columns.Add(colHKeyPostCode, "Post Code", 1500, HorizontalAlignment.Right, HistoryListViewColIndex.PostCode)

            lvwHistory.Columns.Add(colHKeyPaperDD, "Paper DD", 1500, HorizontalAlignment.Center, HistoryListViewColIndex.PaperDD)

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubReturn, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: SetupCreditCardMediaTypeHistoryListView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 09-05-2007 : Instalment_Import Changes
    ' ***************************************************************** '
    Public Sub SetupCreditCardMediaTypeHistoryListView()

        Const kMethodName As String = "SetupCreditCardMediaTypeHistoryListView"

        Dim lSubReturn As Integer
        Try
            lvwCCHistory.Columns.Clear()

            lvwCCHistory.Columns.Add(colHKeyActionCode, "Action Code", 1500, HorizontalAlignment.Left, CCHistoryListViewColIndex.ActionCode)

            lvwCCHistory.Columns.Add(colHKeyDateModified, "Date Modified", 1500, HorizontalAlignment.Left, CCHistoryListViewColIndex.DateModified)

            lvwCCHistory.Columns.Add(colHKeyCCNumber, "CC Number", 1500, HorizontalAlignment.Left, CCHistoryListViewColIndex.CCNumber)

            lvwCCHistory.Columns.Add(colHKeyCCStartDate, "Start Date", 1500, HorizontalAlignment.Left, CCHistoryListViewColIndex.CCStartDate)

            lvwCCHistory.Columns.Add(colHKeyExpiryDate, "Expiry Date", 1500, HorizontalAlignment.Left, CCHistoryListViewColIndex.CCExpiryDate)

            lvwCCHistory.Columns.Add(colHKeyCardHolderName, "Card Holder Name", 1500, HorizontalAlignment.Left, CCHistoryListViewColIndex.CardHolderName)

            lvwCCHistory.Columns.Add(colHKeyCardHolderAddressLine1, "Address Line 1", 1500, HorizontalAlignment.Left, CCHistoryListViewColIndex.CardHolderAddressLine1)

            lvwCCHistory.Columns.Add(colHKeyCardHolderPostCode, "Post Code", 1500, HorizontalAlignment.Left, CCHistoryListViewColIndex.CardHolderPostCode)

            lvwCCHistory.Columns.Add(colHKeyUser, "User", 1500, HorizontalAlignment.Left, CCHistoryListViewColIndex.User)

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubReturn, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
    End Sub


    ''' <summary>
    ''' ValidateAccountNumber
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateAccountNumber() As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim oSirMediaTypeValidation As bSIRMediaTypeValidation.Business
        Dim bValid As Boolean
        Dim sStrippedString As String = ""

        Dim sBankName As String = "", sAddress1 As String = "", sAddress2 As String = "", sAddress3 As String = "", sAddress4 As String = "", sPostalCode As String = ""
        Dim vValidationMessage As Object = Nothing
        Dim bValidationOverridable As Boolean
        Dim iCountryID As Integer = 0

        Try

            'TR - Make sure that Account Number field is populated
            Dim sMessage As String = String.Empty
            Dim IsValid As String = String.Empty

            If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanMediaTypeCode, 0)).Trim() <> "CC" And Strings.Len(txtAccountNumber.Text) > 0 Then

                If AlphanumericValidation(Trim(txtBIC.Text)) <> PMEReturnCode.PMTrue Then
                    MessageBox.Show("Only alphanumeric characters allowed in BIC field.", "Bank Validation",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return PMEReturnCode.PMFalse
                End If

                If AlphanumericValidation(Trim(txtIBAN.Text)) <> PMEReturnCode.PMTrue Then
                    MessageBox.Show("Only alphanumeric characters allowed in IBAN field.", "Bank Validation",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return PMEReturnCode.PMFalse
                End If

                Dim temp_oSirMediaTypeValidation As Object = Nothing
                'm_lReturn = g_oObjectManager.GetInstance(temp_oSirMediaTypeValidation, "bSirMediaTypeValidation.business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_lReturn = g_oObjectManager.GetInstance(temp_oSirMediaTypeValidation, "bSIRMediaTypeValidation.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oSirMediaTypeValidation = temp_oSirMediaTypeValidation

                'TR PN5080- Append the Account Number to the Sort Code field. Do
                'not check the Sort code field as some customers will put the sort code into
                'the account field (IAG), but for other (i.e. GB) customers the sort
                'code goes into it's own field. So sort code can be blank.
                'Strip the Spaces from the SortCode & AccountNumber before Validation
                sStrippedString = txtSortCode.Text.Replace(" ", "") & "|" &
                                  txtAccountNumber.Text.Replace(" ", "") & "|" & uctPartyBankCombo1.SelectedAccountType

                'PN: 48187
                ' Check if bank account validation has been performed
                If m_bBankAccountValidated Then
                    'if the bank branch code and account number haven't change then no need to perform bank account validation again
                    If m_sBankBranchCode = txtSortCode.Text.Replace(" ", "") AndAlso
                        m_sAccountCode = txtAccountNumber.Text.Replace(" ", "") AndAlso
                        m_sBIC = Trim(txtBIC.Text) AndAlso
                        m_sIBAN = Trim(txtIBAN.Text) Then
                        Return nResult
                    End If
                End If

                If addBank.CountryId = 0 Then
                    iCountryID = g_lCountryID
                Else
                    iCountryID = addBank.CountryId
                End If

                'TR - Perform the validation
                oSirMediaTypeValidation.ValidateNumber(m_lMediaTypeID, iCountryID, sStrippedString, bValid,
                                                       sBankName, sAddress1, sAddress2, sAddress3, sAddress4, sPostalCode,
                                                       vValidationMessage, bValidationOverridable,
                                                       sBIC:=Trim(txtBIC.Text), sIBAN:=Trim(txtIBAN.Text))

                If m_lReturn = PMEReturnCode.PMError Then
                    MsgBoxSirius("Failed to validate Account No", MsgBoxStyle.Exclamation, "Validate")
                Else
                    If Not bValid Then
                        If Information.IsArray(vValidationMessage) Then

                            For iErrCount As Integer = 0 To vValidationMessage.GetUpperBound(0)

                                sMessage = sMessage & Strings.Chr(13) & Strings.Chr(10) & CStr(vValidationMessage(iErrCount))
                            Next
                        Else
                            'if there is no message then store the generic message
                            sMessage = "Bank details have failed validation"
                        End If

                        'if validation are overridable then show the message with vbYesNo
                        If bValidationOverridable Then
                            sMessage = sMessage & Strings.Chr(13) & Strings.Chr(10) & "Do you wish to override the bank validation?"
                            IsValid = CStr(MessageBox.Show(sMessage, "Bank Validation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
                            If IsValid = System.Windows.Forms.DialogResult.Yes Then
                                txtBankName.Text = sBankName
                                addBank.AddressLine1 = sAddress1
                                addBank.AddressLine2 = sAddress2
                                addBank.AddressLine3 = sAddress3
                                addBank.AddressLine4 = sAddress4
                                addBank.PostCode = sPostalCode
                                addBank.CountryId = iCountryID
                                'PN: 48187
                                'Store the bank branch code, account number and set the validation flag status
                                m_sBankBranchCode = txtSortCode.Text.Replace(" ", "")
                                m_sAccountCode = txtAccountNumber.Text.Replace(" ", "")
                                m_sBIC = txtBIC.Text
                                m_sIBAN = txtIBAN.Text
                                m_bBankAccountValidated = True
                                nResult = PMEReturnCode.PMTrue
                            Else
                                nResult = PMEReturnCode.PMFalse
                            End If
                        ElseIf Not bValidationOverridable Then
                            MessageBox.Show(sMessage, "Bank Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            txtBankName.Text = sBankName
                            addBank.AddressLine1 = sAddress1
                            addBank.AddressLine2 = sAddress2
                            addBank.AddressLine3 = sAddress3
                            addBank.AddressLine4 = sAddress4
                            addBank.PostCode = sPostalCode
                            addBank.CountryId = iCountryID
                            nResult = PMEReturnCode.PMFalse
                            'PN: 47717
                            'cmdSave.Enabled = False
                        End If
                    Else
                        If m_lReturn = PMEReturnCode.PMTrue Then
                            If (txtBankName.Text = String.Empty) Then
                                txtBankName.Text = sBankName
                            End If
                            If (addBank.AddressLine1 = String.Empty) Then
                                addBank.AddressLine1 = sAddress1
                            End If
                            If (addBank.AddressLine2 = String.Empty) Then
                                addBank.AddressLine2 = sAddress2
                            End If
                            If (addBank.AddressLine3 = String.Empty) Then
                                addBank.AddressLine3 = sAddress3
                            End If
                            If (addBank.AddressLine4 = String.Empty) Then
                                addBank.AddressLine4 = sAddress4
                            End If
                            If (addBank.PostCode = String.Empty) Then
                                addBank.PostCode = sPostalCode
                            End If
                            If addBank.CountryId = 0 Then
                                addBank.CountryId = g_lCountryID
                            End If
                        End If
                        'PN: 48187
                        'Store the bank branch code, account number and set the validation flag status
                        m_sBankBranchCode = txtSortCode.Text.Replace(" ", "")
                        m_sAccountCode = txtAccountNumber.Text.Replace(" ", "")
                        m_sBIC = txtBIC.Text
                        m_sIBAN = txtIBAN.Text
                        m_bBankAccountValidated = True
                        nResult = PMEReturnCode.PMTrue
                    End If
                    oSirMediaTypeValidation = Nothing

                End If

            End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Validate Account No Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="txtAccountNumber_Validate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult
        End Try
    End Function

    'Party Bank Details
    Private Function FillPartyBankDetails() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "FillPartyBankDetails"

        Dim vPartyBankId As Object = Nothing
        Dim bIsBank As Boolean
        Dim vAccountId As Object = Nothing
        Dim vBankPaymentTypeId As String = ""
        Dim vBankAccountTypeId As Object = Nothing
        Dim vBankAccountType As Object = Nothing
        Dim sAccountHolderName As String = String.Empty
        Dim sAccountNumber As String = String.Empty
        Dim sBankNameId As String = String.Empty
        Dim sBankName As String = String.Empty
        Dim sBankBranch As String = String.Empty
        Dim sBankBranchCode As String = String.Empty
        Dim sBankAddress1 As String = String.Empty
        Dim sBankAddress2 As String = String.Empty
        Dim sBankAddress3 As String = String.Empty
        Dim sBankTown As String = String.Empty
        Dim sBankPostCode As String = String.Empty
        Dim sBankRegion As String = String.Empty
        Dim nBankCountry As Object = Nothing
        Dim sCardNumber As String = String.Empty
        Dim sCardStart As String = String.Empty
        Dim sCardExpiryDate As String = String.Empty
        Dim sCardIssueNumber As String = String.Empty
        Dim sCardPin As String = String.Empty
        Dim sCardIsRegistered As Object = Nothing
        Dim sCardAddress1 As String = String.Empty
        Dim sCardAddress2 As String = String.Empty
        Dim sCardAddress3 As String = String.Empty
        Dim sCardTown As String = String.Empty
        Dim sCardPostCode As String = String.Empty
        Dim nCardCountry As Object = Nothing
        Dim vIsDeleted As Object = Nothing
        Dim bMatchFound As Boolean
        Dim nBankPaymentId As Integer
        Dim sNameOnCard As String = String.Empty
        Dim bCardIsRegistered As Boolean
        Dim sBIC As String = String.Empty
        Dim sIBAN As String = String.Empty

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If SSTabHelper.GetTabVisible(tabMainTab, kMainTabBankDetails) Then
                nBankPaymentId = uctPartyBankCombo1.SelectedPaymentID
            Else
                nBankPaymentId = uctPartyBankCombo2.SelectedPaymentID
            End If

            If (Information.IsArray(aoPartyBankDetails) Or nBankPaymentId > 0) And m_iTask <> gPMConstants.PMEComponentAction.PMView Then

                If Information.IsArray(aoPartyBankDetails) Then
                    For lPaymentCount As Integer = 0 To aoPartyBankDetails.GetUpperBound(1)
                        If ToSafeInteger(aoPartyBankDetails(ENPartyBank.PartyBankId, lPaymentCount)) = nBankPaymentId Then
                            g_oBusiness.AccountType = ToSafeString(aoPartyBankDetails(ENPartyBank.BankAccountTypeId, lPaymentCount))
                            bIsBank = ToSafeBoolean(aoPartyBankDetails(ENPartyBank.IsBank, lPaymentCount))
                            sAccountHolderName = ToSafeString(aoPartyBankDetails(ENPartyBank.AccountHolderName, lPaymentCount))
                            sAccountNumber = ToSafeString(aoPartyBankDetails(ENPartyBank.AccountNumber, lPaymentCount))
                            If IsArray(aoPartyBankDetails(ENPartyBank.BankNameId, lPaymentCount)) Then
                                sBankName = ToSafeString(aoPartyBankDetails(ENPartyBank.BankNameId, lPaymentCount).GetValue(ENPMLookups.Description))
                            End If
                            sBankBranch = ToSafeString(aoPartyBankDetails(ENPartyBank.BankBranch, lPaymentCount))
                            sBankBranchCode = ToSafeString(aoPartyBankDetails(ENPartyBank.BankBranchCode, lPaymentCount))
                            sBankAddress1 = ToSafeString(aoPartyBankDetails(ENPartyBank.BankAdd1, lPaymentCount))
                            sBankAddress2 = ToSafeString(aoPartyBankDetails(ENPartyBank.BankAdd2, lPaymentCount))
                            sBankAddress3 = ToSafeString(aoPartyBankDetails(ENPartyBank.BankAdd3, lPaymentCount))
                            sBankTown = ToSafeString(aoPartyBankDetails(ENPartyBank.BankTown, lPaymentCount))
                            sBankPostCode = ToSafeString(aoPartyBankDetails(ENPartyBank.BankPCode, lPaymentCount))
                            sBankRegion = ToSafeString(aoPartyBankDetails(ENPartyBank.BankRegion, lPaymentCount))

                            nBankCountry = ToSafeInteger(aoPartyBankDetails(ENPartyBank.BankCountry, lPaymentCount)(ENPMLookups.Id))
                            bCardIsRegistered = ToSafeBoolean(aoPartyBankDetails(ENPartyBank.IsRegistered, lPaymentCount))

                            sCardNumber = ToSafeString(aoPartyBankDetails(ENPartyBank.CCNum, lPaymentCount))
                            sCardStart = ToSafeString(aoPartyBankDetails(ENPartyBank.CCStartDate, lPaymentCount))
                            sCardExpiryDate = ToSafeString(aoPartyBankDetails(ENPartyBank.CCExpiryDate, lPaymentCount))
                            sCardIssueNumber = ToSafeString(aoPartyBankDetails(ENPartyBank.CCIssueNum, lPaymentCount))
                            sCardPin = ToSafeString(aoPartyBankDetails(ENPartyBank.CCPIN, lPaymentCount))
                            sCardAddress1 = ToSafeString(aoPartyBankDetails(ENPartyBank.CCAdd1, lPaymentCount))
                            sCardAddress2 = ToSafeString(aoPartyBankDetails(ENPartyBank.CCAdd2, lPaymentCount))
                            sCardAddress3 = ToSafeString(aoPartyBankDetails(ENPartyBank.CCAdd3, lPaymentCount))
                            sCardTown = ToSafeString(aoPartyBankDetails(ENPartyBank.CCTown, lPaymentCount))
                            sCardPostCode = ToSafeString(aoPartyBankDetails(ENPartyBank.CCPCode, lPaymentCount))
                            nCardCountry = aoPartyBankDetails(ENPartyBank.CCCountry, lPaymentCount)(ENPMLookups.Id)
                            sNameOnCard = ToSafeString(aoPartyBankDetails(ENPartyBank.NameOnCard, lPaymentCount))

                            sBIC = CStr(aoPartyBankDetails(ENPartyBank.BIC, lPaymentCount))
                            sIBAN = CStr(aoPartyBankDetails(ENPartyBank.IBAN, lPaymentCount))

                            bMatchFound = True

                        End If
                    Next

                    If bMatchFound Then

                        If SSTabHelper.GetTabVisible(tabMainTab, kMainTabBankDetails) And bIsBank Then
                            'Associated  Credit Card should be cleared as payment is via Bank
                            If ClearFields(False) <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "ClearFields Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                            txtBankName.Text = sBankName
                            ApplyHighlighting(k_PFPlanBankName, txtBankName)

                            txtSortCode.Text = sBankBranchCode
                            ApplyHighlighting(k_PFPlanBankSortCode, txtSortCode)

                            txtAccountNumber.Text = sAccountNumber
                            ApplyHighlighting(k_PFPlanBankAccountNo, txtAccountNumber)

                            txtAccountName.Text = sAccountHolderName
                            ApplyHighlighting(k_PFPlanBankAccountName, txtAccountName)

                            txtBranch.Text = sBankBranch
                            ApplyHighlighting(k_PFPlanBankBranch, txtBranch)

                            addBank.AddressLine1 = sBankAddress1
                            addBank.AddressLine2 = sBankAddress2
                            addBank.AddressLine3 = sBankAddress3
                            addBank.AddressLine4 = sBankRegion
                            addBank.PostCode = sBankPostCode

                            If nBankCountry <> 0 Then
                                addBank.CountryId = nBankCountry
                            Else
                                addBank.CountryId = g_oObjectManager.CountryID
                            End If

                            txtAreaCode.Text = Trim(m_vFinancePlanArray(k_PFPlanBankAreaCode, 0))
                            ApplyVersionHighlighting(k_PFPlanBankAreaCode, txtAreaCode)

                            txtNumber.Text = Trim(m_vFinancePlanArray(k_PFPlanBankPhone, 0))
                            ApplyVersionHighlighting(k_PFPlanBankPhone, txtNumber)

                            txtExtension.Text = Trim(m_vFinancePlanArray(k_PFPlanBankExtn, 0))
                            ApplyVersionHighlighting(k_PFPlanBankExtn, txtExtension)

                            txtFaxAreaCode.Text = Trim(m_vFinancePlanArray(k_PFPlanBankFaxCode, 0))
                            ApplyVersionHighlighting(k_PFPlanBankFaxCode, txtFaxAreaCode)

                            txtFaxNumber.Text = Trim(m_vFinancePlanArray(k_PFPlanBankFax, 0))
                            ApplyVersionHighlighting(k_PFPlanBankFax, txtFaxNumber)
                        ElseIf (SSTabHelper.GetTabVisible(tabMainTab, kMainTabCreditCardDetails) AndAlso Not bIsBank) Then
                            'Associated  Bank fields should be cleared as payment is via Credit Card
                            If ClearFields(True) <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "ClearFields Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If

                            If sCardNumber <> "" Then
                                txtCardNo.Text = "**** **** **** " &
                                                 ToSafeString(sCardNumber).Substring(ToSafeString(sCardNumber).Length - 4)

                                ApplyHighlighting(k_PFPlanCCNumber, txtCardNo)
                            End If
                            m_sCreditCardNo = sCardNumber

                            If Not bCardIsRegistered Then
                                txtCardholderName.Text = sNameOnCard
                            Else
                                txtCardholderName.Text = sAccountHolderName
                            End If
                            ApplyHighlighting(k_PFPlanBankName, txtCardName)

                            txtExpiryDate.Text = sCardExpiryDate
                            ApplyHighlighting(k_PFPlanCCExpiryDate, txtExpiryDate)

                            chkCardholder.Checked = bCardIsRegistered

                            txtCardStartDate.Text = sCardStart
                            ApplyHighlighting(k_PFPlanCCStartDate, txtCardStartDate)

                            txtIssueNo.Text = sCardIssueNumber
                            ApplyHighlighting(k_PFPlanCCIssue, txtIssueNo)

                            txtCardName.Text = sAccountHolderName
                            ApplyHighlighting(k_PFPlanBankAccountName, txtCardName)
                            txtBIC.Text = CStr(sBIC)
                            txtIBAN.Text = CStr(sIBAN)

                            If ToSafeString(sCardNumber) <> "" Then
                                txtCardNo.Text = "**** **** **** " & ToSafeString(sCardNumber).Substring(ToSafeString(sCardNumber).Length - 4)
                            End If
                            txtTrackingNumber.Text = CStr(sCardNumber)
                            txtExpiryDate.Text = CStr(sCardExpiryDate)
                            txtCardStartDate.Text = CStr(sCardStart)
                            txtIssueNo.Text = CStr(sCardIssueNumber)
                            txtPin.Text = CStr(sCardPin)
                            chkCardholder.CheckState = CInt(sCardIsRegistered)
                            If sCardIsRegistered = 0 Then
                                txtCardholderName.Text = CStr(sNameOnCard)
                            Else
                                txtCardholderName.Text = CStr(sAccountHolderName)
                            End If

                            addCardholder.AddressLine1 = sCardAddress1
                            addCardholder.AddressLine2 = sCardAddress2
                            addCardholder.AddressLine3 = sCardAddress3
                            addCardholder.AddressLine4 = sCardTown
                            addCardholder.PostCode = sCardPostCode
                            addCardholder.CountryId = nCardCountry
                        End If
                    End If

                End If

            End If



        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally




        End Try
        Return result
    End Function

    Private Function ClearFields(ByRef bClearBankFields As Boolean) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ClearFields"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'For Bank
            If bClearBankFields Then
                txtBankName.Text = ""
                addBank.AddressLine1 = ""
                addBank.AddressLine2 = ""
                addBank.AddressLine3 = ""
                addBank.AddressLine4 = ""
                addBank.PostCode = ""
                txtBranch.Text = ""
                txtSortCode.Text = ""
                txtAccountName.Text = ""
                txtAccountNumber.Text = ""
                txtBIC.Text = ""
                txtIBAN.Text = ""
                'For Credit Card
            ElseIf (Not bClearBankFields) Then
                txtCardNo.Text = ""
                'txtCardName.Text = Not Captured yet
                txtExpiryDate.Text = ""
                txtCardStartDate.Text = ""
                txtIssueNo.Text = ""
                txtPin.Text = ""
                chkCardholder.CheckState = CheckState.Unchecked
                txtCardholderName.Text = ""
                addCardholder.AddressLine1 = ""
                addCardholder.AddressLine2 = ""
                addCardholder.AddressLine3 = ""
                addCardholder.AddressLine4 = ""
                addCardholder.PostCode = ""

            End If



        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally




        End Try
        Return result
    End Function
    Private Sub uctPartyBankCombo1_ComboChange(ByVal Sender As Object, ByVal e As uctPartyBank.uctPartyBankCombo.ComboChangeEventArgs) Handles uctPartyBankCombo1.ComboChange
        If e.lSelItemID > 0 Then
            GetPartyBanks()
            FillPartyBankDetails()
            EnableDisableBankControls(False)
        Else
            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                EnableDisableBankControls(False)
                Exit Sub
            End If
            If m_bLoading And m_sTransactionType <> "NB" Then Exit Sub
            If ((m_iTask = gPMConstants.PMEComponentAction.PMAdd) Or (m_iTask = gPMConstants.PMEComponentAction.PMEdit And m_bChangeAccountType)) AndAlso m_nUnloadMode <> 3 Then
                ClearFields(True)
            End If
            EnableDisableBankControls(True)
        End If
    End Sub

    Private Sub uctPartyBankCombo2_ComboChange(ByVal Sender As Object, ByVal e As uctPartyBank.uctPartyBankCombo.ComboChangeEventArgs) Handles uctPartyBankCombo2.ComboChange

        If e.lSelItemID > 0 Then
            GetPartyBanks()
            FillPartyBankDetails()
            EnableDisableBankControls(False)
        Else
            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                EnableDisableBankControls(False)
                Exit Sub
            End If
            If m_bLoading And m_sTransactionType <> "NB" Then Exit Sub

            If (m_iTask = gPMConstants.PMEComponentAction.PMAdd) Or (m_iTask = gPMConstants.PMEComponentAction.PMEdit And m_bChangeAccountType) Then
                ClearFields(False)
            End If
            EnableDisableBankControls(True)
        End If
    End Sub

    Private Sub uctPartyBankCombo2_AddPartyBankItem(ByVal Sender As Object, ByVal e As uctPartyBank.uctPartyBankCombo.AddPartyBankItemEventArgs) Handles uctPartyBankCombo2.AddPartyBankItem
        m_lReturn = GetPartyBanks()
        FillPartyBankDetails()
    End Sub

    Private Sub uctPartyBankCombo2_EditPartyBankItem(Sender As Object, e As uctPartyBank.uctPartyBankCombo.EditPartyBankItemEventArgs) Handles uctPartyBankCombo2.EditPartyBankItem
        m_lReturn = GetPartyBanks()
        m_bCreditCardEdited = True
        FillPartyBankDetails()
    End Sub

    'TODO::
    'Changes done as per VB Code
    Private Sub uctPartyBankCombo1_EditPartyBankItem(ByVal Sender As System.Object, ByVal e As uctPartyBank.uctPartyBankCombo.EditPartyBankItemEventArgs) Handles uctPartyBankCombo1.EditPartyBankItem
        m_lReturn = GetPartyBanks()
        m_bBankDetailsEdited = True
        FillPartyBankDetails()
    End Sub

    ' ***************************************************************** '
    ' Name: DisplayInstalments
    ' ***************************************************************** '
    Public Function DisplayPolicyList() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DisplayPolicyList"

        Dim nLower, nUpper As Integer
        Dim oListItem As ListViewItem
        Dim vPolicyListArray(,) As Object = Nothing

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = g_oBusiness.GetPolicyList(v_lPlanPFCnt:=m_lFinancePlanCnt, r_vPolicyListArray:=vPolicyListArray, v_lPlanPFVersion:=m_lFinancePlanVersion, v_bHistory:=m_bHistory)
            SSTabHelper.SetTabVisible(tabMainTab, 6, True)

            lvwPolicyList.Items.Clear()

            If Information.IsArray(vPolicyListArray) Then

                nLower = vPolicyListArray.GetLowerBound(1)

                nUpper = vPolicyListArray.GetUpperBound(1)

                For nCount As Integer = nLower To nUpper

                    oListItem = lvwPolicyList.Items.Add(CStr(vPolicyListArray(1, nCount)))

                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(vPolicyListArray(2, nCount))

                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = StringsHelper.Format(vPolicyListArray(3, nCount), "#,##0.00")
                    If Information.IsDate(vPolicyListArray(4, nCount)) Then

                        ListViewHelper.GetListViewSubItem(oListItem, 3).Text = DateTime.Parse(CDate(vPolicyListArray(4, nCount))).ToString("d")
                    Else

                        ListViewHelper.GetListViewSubItem(oListItem, 3).Text = (CStr(vPolicyListArray(4, nCount)))
                    End If

                    ' Tag our index in as well
                    oListItem.Tag = CStr(nCount)
                Next
            End If

            lvwPolicyList.GridLines = True

            ListView6Func.ListViewAutoSize(lvwList:=lvwPolicyList)
        Catch ex As Exception


            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: CancelPolicies
    ' ***************************************************************** '
    Public Function CancelPolicies(ByVal vPlanCancellationTransactions(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CancelPolicies"

        Dim vPolicyPaidToDate As Object = Nothing
        Dim cTotalPaidToDate, cTotalPremium As Decimal
        Dim dLapsedDate, dCoverStartDate, dCoverExpiryDate As Date
        Dim fCancelPolicy As frmCancelPolicy
        Dim fCancel As frmCancelPoliciesFailed
        Dim sError As String = ""
        Dim vPolicyList As Object = Nothing
        Dim bIsInstalmentCollected As Boolean
        Dim dtInceptionDateTPI As Date

        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            fCancelPolicy = New frmCancelPolicy()

            If Information.IsArray(vPlanCancellationTransactions) Then

                fCancelPolicy.AccountID = CInt(vPlanCancellationTransactions(2, 0))
            End If
            If bIsSinglePlanParty Then
                'Get the payment status
                m_lReturn = g_oBusiness.GetPolicyList(v_lPlanPFCnt:=m_vFinancePlanArray(k_PFPlanPFCnt, 0),
                                                r_vPolicyListArray:=vPolicyList)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "g_oBusiness.GetPolicyPaidToDate" & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                m_lReturn = g_oBusiness.IsInstalmentCollected(v_lpfprem_finance_cnt:=m_vFinancePlanArray(k_PFPlanPFCnt, 0),
                                                              v_lpfprem_finance_version:=m_vFinancePlanArray(k_PFPlanPFVersion, 0),
                                                r_bIsInstalmentCollected:=bIsInstalmentCollected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "g_oBusiness.GetPolicyPaidToDate" & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                If IsArray(vPolicyList) Then
                    ' Store the cover start date
                    If bIsInstalmentCollected Then
                        If IsDate(m_vFinancePlanArray(k_PFPlanCoverEndDate, 0)) Then
                            dCoverStartDate = ToSafeDate(m_vFinancePlanArray(k_PFPlanFirstInstalmentdate, 0))
                        End If
                    Else
                        If IsDate(vPolicyList(9, 0)) Then
                            dCoverStartDate = ToSafeDate(vPolicyList(9, 0))
                        End If
                    End If
                    'Store the cover end date
                    If IsDate(m_vFinancePlanArray(k_PFPlanCoverEndDate, 0)) Then
                        dCoverExpiryDate = ToSafeDate(m_vFinancePlanArray(k_PFPlanCoverEndDate, 0))
                    End If
                Else
                    ' Store the cover start date
                    If Information.IsDate(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCoverStartDate, 0)) Then
                        dCoverStartDate = CDate(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCoverStartDate, 0))
                    End If

                    ' Store the cover end date
                    If Information.IsDate(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCoverEndDate, 0)) Then
                        dCoverExpiryDate = CDate(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanCoverEndDate, 0))
                    End If
                End If
            Else
                ' Store the cover start date
                If IsDate(m_vFinancePlanArray(k_PFPlanCoverStartDate, 0)) Then
                    dCoverStartDate = ToSafeDate(m_vFinancePlanArray(k_PFPlanCoverStartDate, 0))
                End If

                ' Store the cover end date
                If IsDate(m_vFinancePlanArray(k_PFPlanCoverEndDate, 0)) Then
                    dCoverExpiryDate = ToSafeDate(m_vFinancePlanArray(k_PFPlanCoverEndDate, 0))
                End If
            End If

            m_lReturn = g_oBusiness.GetPolicyPaidToDate(v_lPremiumFinanceCnt:=m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFCnt, 0), v_lPremiumFinanceVer:=m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPFVersion, 0), r_vPolicyPaidToDate:=vPolicyPaidToDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "g_oBusiness.GetPolicyPaidToDate" & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Information.IsArray(vPolicyPaidToDate) Then

                cTotalPremium = Conversion.Val(CStr(vPolicyPaidToDate(0, 0)))

                cTotalPaidToDate = Conversion.Val(CStr(vPolicyPaidToDate(1, 0)))
                dtInceptionDateTPI = gPMFunctions.ToSafeDate(vPolicyPaidToDate(2, 0))
                ' Work out lapsed date:
                'Add [(total paid/total prem) * policy days] to start date
                Dim iDays As Integer = ((cTotalPaidToDate / cTotalPremium) * DateAndTime.DateDiff("d", dtInceptionDateTPI, dCoverExpiryDate, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1))

                dLapsedDate = dtInceptionDateTPI.AddDays(iDays)
            Else
                ' If nothing returned from previous SQL, then no instalments
                ' have been paid so set lapsed date to start date
                dLapsedDate = dCoverStartDate
            End If

            fCancelPolicy.PolicyLapseDate = dLapsedDate
            fCancelPolicy.FinancePlanArray = m_vFinancePlanArray ''R
            fCancelPolicy.ShowDialog()

            If fCancelPolicy.Status = gPMConstants.PMEReturnCode.PMOK Then

                Do While fCancelPolicy.PolicyLapseDate < dCoverStartDate
                    MsgBox("Backdated Cancellation is not allowed via this Roadmap." _
                    & vbCrLf & "Please Specify a valid Date or Cancel The Policy via Underwriting Cancel Policy Task.", vbCritical, "Cancel Policy")
                    fCancelPolicy.ShowDialog()
                    If fCancelPolicy.Status = gPMConstants.PMEReturnCode.PMCancel Then
                        Exit Function
                    End If
                Loop

                fCancel = New frmCancelPoliciesFailed()

                Dim vDocumentArray() As Object = Nothing

                m_lReturn = g_oBusiness.CancelPolicies(r_vPremiumFinance:=m_vFinancePlanArray, r_sErrors:=sError, v_vCredits:=0, v_vDebits:=0, v_bWriteOff:=fCancelPolicy.WriteOff, v_bSpoolDocument:=fCancelPolicy.SpoolDocument, v_lLapseReasonId:=fCancelPolicy.LapseReasonID, v_dPolicyLapseDate:=fCancelPolicy.PolicyLapseDate, r_vDocumentArray:=vDocumentArray)

                If m_lReturn = gPMConstants.PMEReturnCode.PMFail Then
                    fCancel.Errors = sError
                    fCancel.ShowDialog()
                End If
                If IsArray(vDocumentArray) Then
                    Dim vSpoolDocArray(,) As Object = Nothing
                    For icnt As Integer = 0 To vDocumentArray.GetUpperBound(0)
                        vSpoolDocArray = vDocumentArray(icnt)
                        m_lReturn = GenerateDocument(vKeyArray:=vSpoolDocArray)
                    Next
                End If

            End If
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try

        Return result

    End Function
    Private Sub EnableDisableBankControls(ByRef bEnable As Boolean)
        If SSTabHelper.GetTabVisible(tabMainTab, kMainTabCreditCardDetails) Then
            txtCardNo.Enabled = bEnable
            txtCardName.Enabled = bEnable
            txtExpiryDate.Enabled = bEnable
            txtCardStartDate.Enabled = bEnable
            txtIssueNo.Enabled = bEnable
            txtPin.Enabled = bEnable
            chkCardholder.Enabled = bEnable
            txtCardholderName.Enabled = bEnable
            addCardholder.Enabled = bEnable
        End If
        If SSTabHelper.GetTabVisible(tabMainTab, kMainTabBankDetails) Then
            txtBankName.Enabled = bEnable
            addBank.Enabled = bEnable
            addBank.Enabled = bEnable
            addBank.Enabled = bEnable
            addBank.Enabled = bEnable
            addBank.Enabled = bEnable
            addBank.Enabled = bEnable
            txtBranch.Enabled = bEnable
            txtSortCode.Enabled = bEnable
            txtAccountName.Enabled = bEnable
            txtAccountNumber.Enabled = bEnable
            txtBIC.Enabled = bEnable
            txtIBAN.Enabled = bEnable
        End If
    End Sub

    Private Function GetPartyBanks() As Integer
        Dim result As Integer = 0
        Dim bSIRPartyBank As Object = Nothing

        Const kMethodName As String = "GetPartyBanks"

        Dim oPartyBank As bSIRPartyBank.Business
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oPartyBank As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oPartyBank, "bSIRPartyBank.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPartyBank = temp_oPartyBank

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Raise Error.
                gPMFunctions.RaiseError("FillPartyBankCombo", "Unable to get instance of  bSIRPartyBank.Business")
            End If

            If bIsSinglePlanParty Then

                m_lReturn = oPartyBank.GetPartyBankDetails(vPartyCnt:=lLeadAgentCnt, vPartyBankDetails:=aoPartyBankDetails, vAccountID:=Nothing)
            Else

                m_lReturn = oPartyBank.GetPartyBankDetails(vPartyCnt:=m_lPartyCnt, vPartyBankDetails:=aoPartyBankDetails, vAccountID:=Nothing)
            End If

            If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Return result
    End Function

    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

        If e.Alt And e.KeyCode = Keys.D1 And SSTabHelper.GetTabVisible(tabMainTab, 0) Then
            tabMainTab.SelectedTab = _tabMainTab_TabPage0
        End If
        If e.Alt And e.KeyCode = Keys.D2 And SSTabHelper.GetTabVisible(tabMainTab, 1) Then
            tabMainTab.SelectedTab = _tabMainTab_TabPage1
        End If
        If e.Alt And e.KeyCode = Keys.D3 And SSTabHelper.GetTabVisible(tabMainTab, 2) Then
            tabMainTab.SelectedTab = _tabMainTab_TabPage2
        End If
        If e.Alt And e.KeyCode = Keys.D4 And SSTabHelper.GetTabVisible(tabMainTab, 3) Then
            tabMainTab.SelectedTab = _tabMainTab_TabPage3
        End If
        If e.Alt And e.KeyCode = Keys.D5 And SSTabHelper.GetTabVisible(tabMainTab, 4) Then
            tabMainTab.SelectedTab = _tabMainTab_TabPage4
        End If
        If e.Alt And e.KeyCode = Keys.D6 And SSTabHelper.GetTabVisible(tabMainTab, 5) Then
            tabMainTab.SelectedTab = _tabMainTab_TabPage5
        End If
        If e.Alt And e.KeyCode = Keys.D7 And SSTabHelper.GetTabVisible(tabMainTab, 6) Then
            tabMainTab.SelectedTab = _tabMainTab_TabPage6
        End If

    End Sub

    Private Sub lvwInstalment_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwInstalment.Click

        Try
            GetInstalmentSelectedTotal()
        Catch ex As Exception
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "There was an error to get sum of all selected instalment", ACApp, ACClass, "lvwInstalment.Click", Information.Err().Number, ex.Message, excep:=ex)
        End Try
    End Sub

    Private Sub lvwInstalment_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lvwInstalment.MouseDoubleClick
        GetInstalmentValidForUpdateDueDate()
        ProcessInstalment()
    End Sub

    Private Sub PopulateBankDetails()
        Dim iBankTask As gPMConstants.PMEComponentAction
        If partyCnt <> 0 Then
            If bIsSinglePlanParty Then
                m_lReturn = g_oPartyBank.GetPartyBankDetails(vPartyCnt:=lLeadAgentCnt, vPartyBankDetails:=aoPartyBankDetails, vAccountID:=Nothing)
            Else
                m_lReturn = g_oPartyBank.GetPartyBankDetails(vPartyCnt:=partyCnt, vPartyBankDetails:=aoPartyBankDetails, vAccountID:=Nothing)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If
            End If

            If m_sTransactionType = "NB" Then
                iBankTask = gPMConstants.PMEComponentAction.PMAdd
            Else
                iBankTask = gPMConstants.PMEComponentAction.PMEdit
            End If

            If bIsSinglePlanParty Then
                If SSTabHelper.GetTabVisible(tabMainTab, kMainTabBankDetails) Then

                    uctPartyBankCombo1.IsBank = 1

                    uctPartyBankCombo1.PartyCnt = lLeadAgentCnt
                    uctPartyBankCombo1.BankPaymentTypeCode = "INS"
                    uctPartyBankCombo1.Task = iBankTask
                    'Developer Guide No 9
                    uctPartyBankCombo1.Initialise()
                    uctPartyBankCombo1.PopulateScreen()
                Else

                    uctPartyBankCombo2.IsBank = 0

                    uctPartyBankCombo2.PartyCnt = lLeadAgentCnt
                    uctPartyBankCombo2.BankPaymentTypeCode = "INS"

                    uctPartyBankCombo1.Task = iBankTask
                    'Developer Guide No 9
                    uctPartyBankCombo2.Initialise()
                    uctPartyBankCombo2.PopulateScreen()

                End If
            Else
                If SSTabHelper.GetTabVisible(tabMainTab, kMainTabBankDetails) Then

                    uctPartyBankCombo1.IsBank = 1

                    uctPartyBankCombo1.PartyCnt = partyCnt
                    uctPartyBankCombo1.BankPaymentTypeCode = "INS"
                    uctPartyBankCombo1.Task = iBankTask
                    'Developer Guide No 9
                    uctPartyBankCombo1.Initialise()
                    uctPartyBankCombo1.PopulateScreen()
                Else

                    uctPartyBankCombo2.IsBank = 0

                    uctPartyBankCombo2.PartyCnt = partyCnt
                    uctPartyBankCombo2.BankPaymentTypeCode = "INS"
                    uctPartyBankCombo1.Task = iBankTask
                    'Developer Guide No 9
                    uctPartyBankCombo2.Initialise()
                    uctPartyBankCombo2.PopulateScreen()
                End If
            End If

        End If

        If m_iTask = gPMConstants.PMEComponentAction.PMView Or m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
            If SSTabHelper.GetTabVisible(tabMainTab, kMainTabBankDetails) Then
                If Not Convert.IsDBNull(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPartyBankIdSel, 0)) Then
                    If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPartyBankIdSel, 0)) <> "" Then
                        uctPartyBankCombo1.SelectedPaymentID = CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPartyBankIdSel, 0))
                    End If
                End If
                If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                    uctPartyBankCombo1.EnableCombo = False
                End If
            Else
                If CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPartyBankIdSel, 0)) <> "" Then
                    uctPartyBankCombo2.SelectedPaymentID = CInt(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanPartyBankIdSel, 0))
                End If
                If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                    uctPartyBankCombo2.EnableCombo = False
                End If
            End If
        End If
    End Sub
    ''' <summary>
    ''' Called on KeyPress event of textox to validate only alphanumeric input
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub AlphanumericValidation(sender As Object, e As KeyPressEventArgs) Handles txtBIC.KeyPress, txtIBAN.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(e.KeyChar)
        If (KeyAscii >= 48 AndAlso KeyAscii <= 57) OrElse
            (KeyAscii >= 65 AndAlso KeyAscii <= 90) OrElse
            (KeyAscii >= 97 And KeyAscii <= 122) _
            OrElse KeyAscii = 8 OrElse KeyAscii = 127 Then
        Else
            KeyAscii = 0
        End If
        If KeyAscii = 0 Then
            e.Handled = True
        End If
        e.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    ''' <summary>
    ''' Called to validate alphanumeric validation via Copy/Paste 
    ''' </summary>
    ''' <param name="sInput"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AlphanumericValidation(ByVal sInput As String) As Integer
        If System.Text.RegularExpressions.Regex.IsMatch(sInput, "^[a-zA-Z0-9]*$") Then
            Return PMEReturnCode.PMTrue
        Else
            Return PMEReturnCode.PMFalse
        End If
    End Function

    Private Sub ApplyHighlighting(ByVal nPlanArrayConstant As Integer, ByRef r_oControl As TextBox)

        Try
            If m_bUseVersionHighlighting Then
                If gPMFunctions.ToSafeString(r_oControl.Text).Trim.ToUpper <> gPMFunctions.ToSafeString(m_vPreviousFinancePlanArray(nPlanArrayConstant, 0)).Trim.ToUpper Then
                    r_oControl.BackColor = SystemColors.GrayText
                End If
            End If

        Catch excep As System.Exception
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to Apply Highlighting ", ACApp, ACClass, "ApplyHighlighting",
                               Information.Err().Number, excep.Message)
            Exit Sub
        End Try
    End Sub

    Private Sub btnReverseInstalment_Click(sender As Object, e As EventArgs) Handles btnReverseInstalment.Click

        Dim iCount As Integer
        Dim nListCount As Integer
        Dim bSelected As Boolean
        Dim nReturn As Boolean = True
        Dim bCanBeRecalled As Boolean
        Dim bAllocationDaysExceeded As Boolean

        m_lReturn = gPMConstants.PMEReturnCode.PMTrue

        Try
            nListCount = lvwInstalment.Items.Count - 1

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_lReturn = g_oObjectManager.GetInstance(oObject:=obSIRPFInstalments, sClassName:="bSIRPFInstalments.Business", vInstanceManager:=PMGetViaClientManager)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New ApplicationException("Unable to get instance of  bSIRPFInstalments.Business")
            End If

            For iCount = 0 To nListCount

                If lvwInstalment.Items(iCount).Selected = True AndAlso Convert.ToString(lvwInstalment.Items(iCount).SubItems(4).Text).Trim().ToUpper() <> "COLLECTED" Then
                    MessageBox.Show("Reversal of Instalment " & lvwInstalment.Items(iCount).SubItems(0).Text & " with a status of " & lvwInstalment.Items(iCount).SubItems(4).Text & " is not allowed.", "Reversal not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If
                If lvwInstalment.Items(iCount).Selected = True Then
                    bSelected = True

                    m_lReturn = obSIRPFInstalments.CanInstalmentBeRecalled(nUserID:=g_iUserId,
                                        nPFInstalmentsID:=ToSafeInteger(m_vFinancePlanInstalmentArray(PFInstId, lvwInstalment.Items(iCount).Tag)),
                                        r_bCanBeRecalled:=bCanBeRecalled, r_bAllocationDaysExceeded:=bAllocationDaysExceeded)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New ApplicationException("CanInstalmentBeRecalled() method Failed")
                    End If
                    If Not bCanBeRecalled Then
                        MessageBox.Show("You do not have authority to Reverse Instalment.", "Reversal not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        Exit Sub
                    ElseIf bAllocationDaysExceeded Then
                        MessageBox.Show("The number of days of allocation for Instalment " & lvwInstalment.Items(iCount).SubItems(4).Text & " exceeds your limit for reversal.", "Reversal not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        Exit Sub
                    End If
                End If
            Next iCount
            If Not bSelected Then
                MessageBox.Show("You must choose at least one instalment.", "No Instalment Chosen", MessageBoxButtons.OK, MessageBoxIcon.Error)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If
            iCount = 0
            If MessageBox.Show("Are you sure you want to proceed and reverse the selected Instalments?", "Reverse Instalments", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If
            For iCount = 0 To nListCount
                If lvwInstalment.Items(iCount).Selected = True Then

                    m_lReturn = obSIRPFInstalments.ReverseCollectedInstalment(nPFInstalmentsID:=ToSafeInteger(m_vFinancePlanInstalmentArray(PFInstId, lvwInstalment.Items(iCount).Tag)),
                                          sPFPlanStatusInd:=m_vFinancePlanArray(k_PFPlanStatusInd, 0))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Instalment " & lvwInstalment.Items(iCount).Text & " cannot be reversed.", "Reversal failure", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Throw New ApplicationException("ReverseCollectedInstalment() method Failed.")
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        Exit Sub
                    End If
                End If
            Next iCount
            If m_sStatusInd = PFStatusIndCompleted Then
                m_vFinancePlanArray(k_PFPlanStatusInd, 0) = PFStatusIndLive
                SetCombo(cboStatus, CStr(m_vFinancePlanArray(bSIRPremFinConst.k_PFPlanStatusInd, 0)).Trim())
                ApplyVersionHighlighting(bSIRPremFinConst.k_PFPlanStatusInd, cboStatus)
                m_sStatusInd = bSIRPremFinConst.PFStatusIndLive
                CommandSet()
            End If

            m_lReturn = g_oBusiness.GetFinancePlanInstalments(v_lFinancePlanCnt:=m_lFinancePlanCnt, v_lFinancePlanVersion:=m_lFinancePlanVersion, r_vFinancePlanArray:=m_vFinancePlanInstalmentArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New ApplicationException("GetFinancePlanInstalments() method Failed.")
            End If

            m_lReturn = DisplayInstalments()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New ApplicationException("DisplayInstalments() method Failed.")
            End If

            GetInstalmentSelectedTotal()
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="btnReverseInstalment_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="btnReverseInstalment_Click", vErrNo:=Information.Err().Number, vErrDesc:=ex.Message, excep:=ex)
        Finally
            If Not obSIRPFInstalments Is Nothing Then
                obSIRPFInstalments.Dispose()
                obSIRPFInstalments = Nothing
            End If
        End Try
    End Sub
    ''' <summary>
    ''' Call this method to dispaly sum of all selected instalment.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetInstalmentSelectedTotal()
        Dim iCount As Integer
        Dim nListCount As Integer
        Dim crTotalAmount As Decimal

        nListCount = lvwInstalment.Items.Count - 1
        crTotalAmount = 0

        For iCount = 0 To nListCount
            If lvwInstalment.Items(iCount).Selected = True Then
                crTotalAmount = crTotalAmount + ToSafeCurrency(lvwInstalment.Items(iCount).SubItems(3).Text)
            End If
        Next iCount

        m_oFormFields.FormatControl(txtSelectedTotal, crTotalAmount)
    End Sub

    Private Sub GetInstalmentValidForUpdateDueDate()
        Dim iCount As Integer
        Dim iListCount As Integer

        If lvwInstalment.Items(0).Text = "Deposit" Then
            iListCount = lvwInstalment.Items.Count - 1
        Else
            iListCount = lvwInstalment.Items.Count
        End If
        m_iEditableInstalmentNumber = 0
        For iCount = 0 To iListCount - 1
            If UCase(Trim(lvwInstalment.Items(iCount).SubItems(4).Text)) = "NEW" Then
                If lvwInstalment.Items(0).Text = "Deposit" Then
                    m_iEditableInstalmentNumber = 0
                Else
                    m_iEditableInstalmentNumber = CInt(lvwInstalment.Items(iCount).SubItems(0).Text)
                End If

                If iCount <> iListCount - 1 Then
                    m_dtNextInstalmentDuedate = Strings.Format(CDate(lvwInstalment.Items(iCount + 1).SubItems(1).Text), "Short Date")
                End If
                Exit For
            End If
        Next iCount
    End Sub

    Private Function GenerateDocument(ByVal vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oGetDocument As iPMUGetDocument.Interface_Renamed

        result = gPMConstants.PMEReturnCode.PMTrue

        'Generate document.
        oGetDocument = New iPMUGetDocument.Interface_Renamed()

        If oGetDocument Is Nothing Then

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create iPMUGetDocument object", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        oGetDocument.Initialise()

        m_lReturn = oGetDocument.SetKeys(vKeyArray:=vKeyArray)

        oGetDocument.FunctionalArea = 1
        oGetDocument.TransactionType = "NB"

        m_lReturn = oGetDocument.Start()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        oGetDocument.Dispose()
        oGetDocument = Nothing

        Return result

    End Function

End Class
