Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Public Class frmInterface
    Inherits System.Windows.Forms.Form

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    ' PUBLIC Data Members (Begin)

    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.

    'Niit 19-oct-2012
    Dim m_vIsBindRenewalWithoutInvitation As Object
    'Niit 19-oct-2012 end

    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sStepStatus As String = ""

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMUProduct.General
    'Developer Guide No.19 (no solution)
    Private Const vbFormCode As Integer = 0
    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the return value for the a function call.
    Private m_lReturn As Integer

    ' {* USER DEFINED CODE (Begin) *}

    Private m_lProductID As Integer
    Private m_sCode As String = ""
    Private m_dtProductEffectiveDate As Date
    Private m_vRenewalPeriod As Object
    Private m_vDescription As Object
    Private m_vSchemeAgencyRef As Object
    Private m_vBlockNo As Object
    Private m_vProvClaimAutoNumberingID As Object
    Private m_vQuoteAutoNumberingID As Object
    Private m_vPolicyAutoNumberingID As Object
    Private m_vFullClaimAutoNumberingID As Object
    Private m_vMidnightRenewal As Object
    Private m_vAutoRenewal As Object
    Private m_vTaxSuppressed As Object
    Private m_vShortPeriodRated As Object
    Private m_vAccumulation As Object
    Private m_vRIPointer As Object
    Private m_vReportPointer As Object
    'True Monthly Policy
    Private m_vUnifiedRenewalDay As Object
    Private m_vLeadAllowConsolidateComm As Object
    Private m_vLeadMonthInCycle As Object
    Private m_vLeadSuspenseAcct As Object
    Private m_vSubAllowConsolidateComm As Object
    Private m_vSubMonthInCycle As Object
    Private m_vSubSuspenseAcct As Object
    ' PW311002
    Private m_vFollowUpTimeFrame As Object
    Private m_vGracePeriod As Object
    ' Alix
    Private m_vPreventCancelledAgents As Object
    Private m_vAllowPositiveCancellation As Object
    'DJM 23/03/2004
    Private m_vMediaTypeMandatory As Object
    'DJM 24/03/2004
    Private m_vPolicyStyleID As Object
    Private m_vPolicyStyleMandatory As Object


    Private m_vChangePolicyNumberAtRenewal As Object
    Private m_vHideSummaryAtRenewalAcceptance As Object

    'TN20001031 (Start) process 29
    Private m_vClaimYearToCheck As Object
    Private m_vMaxSingleClaimValue As Object
    Private m_vMaxNumberOfClaim As Object
    Private m_vMaxTotalClaimValue As Object
    Private m_vAllowedCausation As Object
    'TN20001031 (End) process 29

    'TN20010514 start
    Private m_vNBProrata As Object
    Private m_vMTAProrata As Object
    'TN20010514 end

    'JMK 25/07/2001 start
    Private m_vRoundPremium As Object
    Private m_vRoundingSection As Object
    'JMK 25/07/2001 end

    Private m_vPolicyNumberAtQuote As Object

    Private m_vAllowedRiskTypeGroup(,) As Object

    'JMK 23/10/2001 display Insurer/Reinsurer
    Private m_sUnderwritingType As String = ""

    ' PW311002
    Private m_bIsNRMA As Boolean

    ' RDC 04052004
    Private m_vCurrencyChange As Object
    ' DD 26/07/2004
    Private m_vLossCurrencyChange As Object
    Private m_vAllowStandardWordingEdit As Object

    Private m_bTrueMonthlyPoliciesEnabled As Boolean
    Private m_vProductIsTrueMonthlyPolicy As Object
    Private m_vProductAnniversaryRenewalWeeks As Object

    Private m_vOrigSuppressClaimTransactionReserve As Object
    Private m_vOrigSuppressClaimTransactionPayment As Object
    Private m_vOrigSuppressClaimTransactionRecovery As Object

    Private m_vSuppressClaimTransactionReserve As Object
    Private m_vSuppressClaimTransactionPayment As Object
    Private m_vSuppressClaimTransactionRecovery As Object

    Private m_iCanMakeLiveInvoice As Integer
    Private m_iCanMakeLiveInstalments As Integer
    Private m_iCanMakeLivePaynow As Integer
    Private m_iCanMakeLiveBankGuarantee As Integer ' Gaurav
    Private m_iCanMakeLiveCashDeposit As Integer 'Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling

    Private m_vRunAuthorisationScriptsforClaimsPayments As Object
    Private m_vMultipleClaimsPayments As String = ""
    Private m_cMaxUnauthorisedClaimValue As Decimal
    Private m_iMaxNoofUnauthorisedClaimPayments As Integer


    Private m_iProduceSchedule As Integer
    Private m_iProduceCertificate As Integer
    Private m_iProduceDebitNote As Integer
    Private m_sPaymentMethod As String = ""
    Private i_Options As Integer

    Private m_iTradeNBOnline As Integer
    Private m_iTradeMTAOnline As Integer
    Private m_iTradeRNLOnline As Integer
    Private m_vOnlineCommencedOn As Object

    Private m_iIsRenewable As CheckState
    Private m_iIsRenewalSelectionEnabled As CheckState
    Private m_iTrueMonthlyPolicyRenewalCommunication As Integer
    Private m_vRenewalSelectionManReviewTemplateId As Object
    Private m_vRenewalSelectionManReviewAttachmentTemplateId As Object
    Private m_vRenewalSelectionInviteTemplateId As Object
    Private m_vRenewalSelectionInviteAttachmentTemplateId As Object
    Private m_vRenewalSelectionUpdateTemplateId As Object
    Private m_vRenewalSelectionUpdateAttachmentTemplateId As Object
    Private m_iIsRenewalInviteEnabled As CheckState
    Private m_vRenewalInviteManReviewTemplateId As Object
    Private m_vRenewalInviteManReviewAttachmentTemplateId As Object
    Private m_vRenewalInviteInviteTemplateId As Object
    Private m_vRenewalInviteInviteAttachmentTemplateId As Object
    Private m_vRenewalInviteUpdateTemplateId As Object
    Private m_vRenewalInviteUpdateAttachmentTemplateId As Object
    Private m_iIsRenewalUpdateEnabled As CheckState
    Private m_vRenewalUpdateManReviewTemplateId As Object
    Private m_vRenewalUpdateManReviewAttachmentTemplateId As Object
    Private m_vRenewalUpdateInviteTemplateId As Object
    Private m_vRenewalUpdateInviteAttachmentTemplateId As Object
    Private m_vRenewalUpdateUpdateTemplateId As Object
    Private m_vRenewalUpdateUpdateAttachmentTemplateId As Object
    Private m_iIsAgentRenewalSelectionEnabled As Integer
    Private m_iIsAgentRenewalInviteEnabled As CheckState
    Private m_iIsAgentRenewalUpdateEnabled As CheckState
    Private m_vAgentRenewalManReviewTemplateId As Object
    Private m_vAgentRenewalManReviewReportId As Object
    Private m_vAgentRenewalInviteTemplateId As Object
    Private m_vAgentRenewalInviteReportId As Object
    Private m_vAgentRenewalUpdateTemplateId As Object
    Private m_vAgentRenewalUpdateReportId As Object

    Private m_oFindEmailDocs As Object

    'Pankaj
    'Developer Guide No. 101
    Private m_vProductList As Object
    Private m_vBankAccountId As Object
    Private m_vClaimValueForLargeLossAdvice As Double
    Private m_vInclusionofCoInsurersOnClaims As Object
    Private m_vAllowNegativeReserve As Object
    Private m_vExtClmHandlerAcknowledgedTaskAllowedTime As Object
    Private m_vExtClmHandlerSupplyPreReportTaskAllowedTime As Object
    Private m_vValidPolicyVersionAtLossDate As Object
    Private m_vIsGrossClaimPaymentAmount As Object
    Private m_vClaimTaskGroup As Object
    Private m_vClaimUserGroup As Object
    Private m_vClaimsUDTA As Object
    Private m_vClaimsUDTB As Object
    Private m_vClaimsUDTC As Object
    Private m_vClaimsUDTD As Object
    Private m_vClaimsUDTE As Object
    Private m_vIsDuplicateClaimCheckEnabled As Object
    Private m_vIsAdvancedTaxScriptEnabled As Object
    Private m_vIsPaymentRefCheckEnabled As Object
    Private m_vIsRecommender As Object

    ' PN 68349
    Private m_vUseNBRenPaymentTermsAtSelection As Object
    ' End
    'Start (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section(4.3.2.1)

    Private m_vCNDefaultPeriod As Object
    Private m_vCNMaxNo As Object
    Private m_vCNDocTemplateId As Object
    Private m_vCNCoverNoteNumberingID As Object
    Private m_vCNTemplateCode As String = ""
    'End (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section(4.3.2.1)
    Private m_vDoNotDeleteRenewalQuoteOnMTA As Integer
    Private m_vDeleteRenewalQuoteReRunOnMTA As Integer

    '1.12 PLICO45
    Private Const MTA_DATE_NOT_ALLOWED As Integer = 0
    Private Const MTA_DATE_CURRENT_PERIOD_ONLY As Integer = 1
    Private Const MTA_DATE_CURRENT_PLUS_1 As Integer = 2
    Private Const MTA_DATE_UNRESTRICTED As Integer = 3

    Private Const MTA_ALLOCATION_LEAVE_ALONE As Integer = 0
    Private Const MTA_ALLOCATION_REVERSE As Integer = 1

    Private m_iMTA_dateAllowed As Integer
    Private m_iMTA_Allocation As Integer
    Private m_vDefaultRenMonths As Object
    Private m_vPaymentCannotExceedReserve As Object

    '(Arul Stephen)-(Tech Spec - HGPS001 Reinsurance Modifications)-(6.1.1.1)
    Private m_vRIManualPremiumAdjustment As Object

    Private m_bMTCRatingRulesEnabled As Boolean
    'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTA - Product Option.doc) - (5.1.1.2)
    Private m_vAllowBackdatedMTAs As Boolean
    'End (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTA - Product Option.doc) - (5.1.1.2)
    ' {* USER DEFINED CODE (End) *}
    ''(start) Saurabh Agrawal Out of sequence MTA Bug Fixing
    Private m_bDoNotDeleteRenQuoteOnMTA As Boolean

    Private m_vOutOfsequenceMtaUserGroup As Object
    Private m_vOutOfsequenceMtaTaskGroup As Object
    ''(start) Saurabh Agrawal Out of sequence MTA Bug Fixing
    'PN 55338
    ' Private m_vIsRI2007 As Byte
    Private m_vIsRI2007 As String = "0"
    'Start - Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling
    Private m_bRoundOffToZero As Boolean
    Private m_sAccountCode As String = ""
    'End - Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling
    Private m_vAllowBackdatedCan As Boolean
    'Start - Sankar - (WPRvb64 Media Type Status) - Paralleling
    Private m_vCheckMediaTypeStatusAtClaimPayment As Object
    Private m_vCheckMediaTypeStatusAtPolicyRefund As Object
    Private m_vChangePolicyNumberAtRenewalAutomatically As Object
    Private m_vTMPAutoRenFAC As Object
    ' WPR53
    Private m_vMandatoryRiskTypeId As Integer
    ' PRIVATE Data Members (End)
    Private m_iReminderUserGroup As Integer
    Private m_iReminderTaskGroup As Integer
    Private m_iTaskManagerDays As Integer
    Private m_bWrittenPolicyStatus As Boolean
    Private m_vUsePriorTermSchemeAtRenewal As Object
    Private m_vProductEnablePrePayment As Object
    'End  Written Status
    Private m_sDefaultCoverToDateToLastDay As String
    Private m_oUnifiedrenewalDateIsReadOnly As Object
    ' PUBLIC Property Procedures (Begin)
    Private m_bIsReservesReadOnly As Boolean
    Private m_bIsRecoveriesReadOnly As Boolean
    Private m_bIsPaymentsReadOnly As Boolean
    Private m_DisplayRerateForQuoteAndNB As Boolean
    Private m_DisplayRerateForMTA As Boolean
    Private m_DisplayRerateForCancellationsAndReinstatments As Boolean
    Private m_DisplayReRateForRenewal As Boolean
    Private autoRenewBackDatedMonthlyPolicy As Boolean
    Private m_bRetainPolicyNumberonCopy As Boolean
    Private m_EditAnnivDate As Boolean
    Private m_bDisableCoverStartDateOnREN As Boolean
    Private m_bUsePolicyInceptionDate As Boolean
    Private m_vAuthorisationThreshold As Object
    Private m_sUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""
	Private m_bVoidTransaction As Boolean
    Private m_iIsQuoteVersioning As Integer
    Private m_iDeleteQuoteAfter As Integer
    Private m_vRecoveryInstalmentsEnabled As Object

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

    'USER DEFINED PUBLIC PROPERTY (End)

    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)

    Public Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
        Set(ByVal Value As Integer)

            ' Set the interface exit status.
            m_lStatus = Value

        End Set
    End Property

    Public Property Task() As Integer
        Get

            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            m_iTask = Value

        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            m_lNavigate = Value

        End Set
    End Property

    Public Property StepStatus() As String
        Get

            Return m_sStepStatus

        End Get
        Set(ByVal Value As String)

            m_sStepStatus = Value

        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            m_dtEffectiveDate = Value

        End Set
    End Property

    'Pankaj
    'Developer Guide No. 101
    Public WriteOnly Property ProductList() As Object
        'Set(ByVal Value() As Object)
        Set(ByVal Value As Object)

            m_vProductList = Value

        End Set
    End Property

    'USER DEFINED PUBLIC PROPERTY (Begin)

    Public Property ProductID() As Integer
        Get
            Return m_lProductID
        End Get
        Set(ByVal Value As Integer)
            m_lProductID = Value
        End Set
    End Property

    Public ReadOnly Property Code() As String
        Get
            Return m_sCode
        End Get
    End Property

    Public ReadOnly Property Description() As String
        Get

            Return CStr(m_vDescription)
        End Get
    End Property

    Public ReadOnly Property ProductEffectiveDate() As Date
        Get
            Return m_dtProductEffectiveDate
        End Get
    End Property

    Public Property UniqueId() As String
        Get
            Return CStr(m_sUniqueId)
        End Get
        Set(ByVal Value As String)
            m_sUniqueId = Value
        End Set
    End Property

    Public Property ScreenHierarchy() As String
        Get
            Return CStr(m_sScreenHierarchy)
        End Get
        Set(ByVal Value As String)
            m_sScreenHierarchy = Value
        End Set
    End Property
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)
    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign all of the controls to
            ' PMFormControl
            '
            ' Example:-
            '
            '        ' Pass control and required settings to FormControl
            'm_lReturn = m_oFormFields.AddNewFormField( _
            '                       ctlControl:=<Control Name>, _
            '                       lFieldType:=<PM field type>, _
            '                       lFormat:=<PM format string>, _
            '                       lMandatory:=<PMMandatory or PMNonMandatory)

            '        'Error checking
            '        If m_lReturn <> PMTrue Then
            '          SetFieldValidation = PMFalse
            '          Exit Function
            '        End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtEffectiveDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtRenewalPeriod, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtRIPointer, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtReportPointer, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDescription, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSchemeAgencyRef, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtBlockNo, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboProvClaimAutoNumberingID, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboQuoteAutoNumberingID, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboPolicyAutoNumberingID, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboFullClaimAutoNumberingID, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Start (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section(4.3.2)

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboCNAutoNumberingID, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCNDefaultPeriod, lFieldType:=gPMConstants.PMEDataType.PMInteger, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory, lDecimalPlaces:=0)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCNMaxNo, lFieldType:=gPMConstants.PMEDataType.PMInteger, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory, lDecimalPlaces:=0)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCNDocTemplate, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'End (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section(4.3.2)



            'TN20001101 (Start) process 29
            If m_oFormFields.AddNewFormField(ctlControl:=txtClaimYear, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oFormFields.AddNewFormField(ctlControl:=txtSingleClaimValue, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oFormFields.AddNewFormField(ctlControl:=txtAllowedClaims, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oFormFields.AddNewFormField(ctlControl:=txtTotalClaimsValue, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TN20001101 (End) process 29

            ' PW311002 - add the grace period
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtGracePeriod, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (End) *}

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOnlineCommencedOn, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboAgentReviewAttachment, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboAgentInviteAttachment, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboAgentUpdateAttachment, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtLargeLossAdviceValue, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAckTaskAllowedTime, lFieldType:=gPMConstants.PMEDataType.PMInteger, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPreReportAllowedTime, lFieldType:=gPMConstants.PMEDataType.PMInteger, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtdefaultRenMth, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Spec - Written
            m_lReturn = m_oFormFields.AddNewFormField(
                       ctlControl:=txtTaskManagerDays,
                       lFieldType:=gPMConstants.PMEDataType.PMInteger,
                       lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger,
                       lMandatory:=PMNonMandatory)
            'End- Written
            ' WPR53
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboApplyMandatoryRisk, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Quote Versioning - Delete Quote After field
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDeleteQuoteAfter, lFieldType:=gPMConstants.PMEDataType.PMInteger, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=PMNonMandatory, lDecimalPlaces:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ''' <summary>
    ''' GetBusiness
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetBusiness() As Integer
        Dim nResult As Integer
        Dim oResultArray(,) As Object = Nothing

        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            nResult = m_oBusiness.GetProductDetails(m_lProductID, oResultArray)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            If Not Information.IsArray(oResultArray) Then
                'if can't find product details then there must be something wrong
                Return nResult
            End If

            If String.IsNullOrEmpty(m_sUniqueId) Then
                m_sUniqueId = GetUniqueID()
            End If

            m_sScreenHierarchy = $"Product({CStr(oResultArray(ACICode, 0)).Trim()})"
            uctSIRSelectClauses.UniqueId = m_sUniqueId
            uctSIRSelectClauses.ScreenHierarchy = m_sScreenHierarchy

            nResult = uctSIRSelectClauses.Initialise()

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="uctSIRSelectClauses.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return nResult
            End If

            uctSIRSelectClauses.ProductId = m_lProductID
            uctSIRSelectClauses.ClauseId = MainModule.ENClauseType.ProductType

            nResult = uctSIRSelectClauses.Load_Renamed
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="uctSIRSelectClauses.Load Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return nResult
            End If

            m_lProductID = CInt(oResultArray(ACIProductid, 0))
            m_sCode = CStr(oResultArray(ACICode, 0))
            m_dtProductEffectiveDate = CDate(oResultArray(ACIProductEffectiveDate, 0))
            m_vDescription = oResultArray(ACIDescription, 0)
            m_vSchemeAgencyRef = oResultArray(ACISchemeAgencyRef, 0)
            m_vBlockNo = oResultArray(ACIBlockNo, 0)
            m_vTaxSuppressed = oResultArray(ACIIsTaxSuppressed, 0)
            m_vShortPeriodRated = oResultArray(ACIIsShortPeriodRated, 0)
            m_vMidnightRenewal = oResultArray(ACIIsMidnightRenewal, 0)
            m_vAutoRenewal = oResultArray(ACIIsAutoRenewable, 0)
            m_vRenewalPeriod = oResultArray(ACIRenewalPeriod, 0)
            m_vQuoteAutoNumberingID = oResultArray(ACIQuoteAutoNumberingID, 0)
            m_vPolicyAutoNumberingID = oResultArray(ACIPolicyAutoNumberingID, 0)
            m_vProvClaimAutoNumberingID = oResultArray(ACIProvClaimAutoNumberingID, 0)
            m_vFullClaimAutoNumberingID = oResultArray(ACIFullClaimAutoNumberingID, 0)
            m_vMandatoryRiskTypeId = oResultArray(ACICPSelMandatoryRiskTypeId, 0)
            m_vCNCoverNoteNumberingID = oResultArray(ACICPSelCNNumberingId, 0)
            m_vAccumulation = oResultArray(ACIAccumulation, 0)
            m_vRIPointer = oResultArray(ACIRIPointer, 0)
            m_vReportPointer = oResultArray(ACIReportPointer, 0)
            m_vClaimYearToCheck = oResultArray(ACIClaimYearToCheck, 0)
            m_vMaxSingleClaimValue = oResultArray(ACIMaxSingleClaimValue, 0)
            m_vMaxNumberOfClaim = oResultArray(ACIMaxNumberOfClaim, 0)
            m_vMaxTotalClaimValue = oResultArray(ACIMaxTotalClaimValue, 0)
            m_vNBProrata = oResultArray(ACINBProrata, 0)
            m_vMTAProrata = oResultArray(ACIMTAProrata, 0)
            m_vRoundPremium = oResultArray(ACIRoundPremium, 0)
            m_vRoundingSection = oResultArray(ACIRoundingSection, 0)
            m_vPolicyNumberAtQuote = oResultArray(ACIPolicyNumberAtQuote, 0)
            m_vUseNBRenPaymentTermsAtSelection = oResultArray(ACICPSelUseNBRenPaymentTermsAtSelection, 0)
            m_vUsePriorTermSchemeAtRenewal = oResultArray(ACICPSelUsePriorTermSchemeAtRenewal, 0)
            m_vAllowStandardWordingEdit = oResultArray(ACIAllowStandardWordingEdit, 0)
            ' PW311002 - add follow up time frame and grace period
            m_vFollowUpTimeFrame = oResultArray(ACIFollowUpTimeFrame, 0)
            m_vGracePeriod = oResultArray(ACIGracePeriod, 0)
            m_vPreventCancelledAgents = oResultArray(ACIPreventCancelledAgents, 0)
            m_vAllowPositiveCancellation = oResultArray(ACIAllowPositiveValues, 0)
            m_vMediaTypeMandatory = oResultArray(ACIMediaTypeMandatory, 0)
            m_vPolicyStyleID = oResultArray(ACIPolicyStyleID, 0)
            m_vPolicyStyleMandatory = oResultArray(ACIPolicyStyleMandatory, 0)
            m_vHideSummaryAtRenewalAcceptance = oResultArray(ACIHideSummaryAtRenewalAcceptance, 0)
            m_vProductIsTrueMonthlyPolicy = gPMFunctions.ToSafeLong(oResultArray(ACIProductTrueMonthlyPolicy, 0), 0)
            m_vProductAnniversaryRenewalWeeks = gPMFunctions.ToSafeLong(oResultArray(ACIProductAnniversaryRenewalWeeks, 0), 0)
            m_iCanMakeLiveInstalments = gPMFunctions.ToSafeInteger(oResultArray(ACICanMakeLiveInstalments, 0), 0)
            m_iCanMakeLiveInvoice = gPMFunctions.ToSafeInteger(oResultArray(ACICanMakeLiveInvoice, 0), 0)
            m_iCanMakeLivePaynow = gPMFunctions.ToSafeInteger(oResultArray(ACICanMakeLivePaynow, 0), 0)
            m_iCanMakeLiveBankGuarantee = gPMFunctions.ToSafeInteger(oResultArray(ACICPSelCanMakeBankGuarantee, 0), 0) 'gaurav
            m_iCanMakeLiveCashDeposit = gPMFunctions.ToSafeInteger(oResultArray(ACICPSelCanMakeCashDeposit, 0), 0) 'Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
            m_iProduceSchedule = gPMFunctions.ToSafeInteger(oResultArray(ACIProduceSchedule, 0), 1)
            m_iProduceCertificate = gPMFunctions.ToSafeInteger(oResultArray(ACIProduceCertificate, 0), 1)
            m_iProduceDebitNote = gPMFunctions.ToSafeInteger(oResultArray(ACIProduceDebitNote, 0), 1)

            nResult = m_oBusiness.GetAllowedRiskTypeGroup(m_lProductID, m_vAllowedRiskTypeGroup)

            m_iTradeNBOnline = gPMFunctions.ToSafeInteger(oResultArray(ACITradeNbOnline, 0))
            m_iTradeMTAOnline = gPMFunctions.ToSafeInteger(oResultArray(ACITradeMtaOnline, 0))
            m_iTradeRNLOnline = gPMFunctions.ToSafeInteger(oResultArray(ACITradeRnlOnline, 0))
            m_vOnlineCommencedOn = oResultArray(ACIOnlineTradingCommencedOn, 0)
            m_iIsRenewable = gPMFunctions.ToSafeInteger(oResultArray(ACIIsRenewable, 0))
            m_iIsRenewalSelectionEnabled = gPMFunctions.ToSafeInteger(oResultArray(ACIIsRenewalSelectionEnabled, 0))
            m_iTrueMonthlyPolicyRenewalCommunication = gPMFunctions.ToSafeInteger(oResultArray(ACITrueMonthlyPolicyRenewalCommunication, 0))
            m_vRenewalSelectionManReviewTemplateId = oResultArray(ACIRenewalSelectionManReviewTemplateId, 0)
            m_vRenewalSelectionManReviewAttachmentTemplateId = oResultArray(ACIRenewalSelectionManReviewAttachmentTemplateId, 0)
            m_vRenewalSelectionInviteTemplateId = oResultArray(ACIRenewalSelectionInviteTemplateId, 0)
            m_vRenewalSelectionInviteAttachmentTemplateId = oResultArray(ACIRenewalSelectionInviteAttachmentTemplateId, 0)
            m_vRenewalSelectionUpdateTemplateId = oResultArray(ACIRenewalSelectionUpdateTemplateId, 0)
            m_vRenewalSelectionUpdateAttachmentTemplateId = oResultArray(ACIRenewalSelectionUpdateAttachmentTemplateId, 0)
            m_iIsRenewalInviteEnabled = gPMFunctions.ToSafeInteger(oResultArray(ACIIsRenewalInviteEnabled, 0))
            m_vRenewalInviteManReviewTemplateId = oResultArray(ACIRenewalInviteManReviewTemplateId, 0)
            m_vRenewalInviteManReviewAttachmentTemplateId = oResultArray(ACIRenewalInviteManReviewAttachmentTemplateId, 0)
            m_vRenewalInviteInviteTemplateId = oResultArray(ACIRenewalInviteInviteTemplateId, 0)
            m_vRenewalInviteInviteAttachmentTemplateId = oResultArray(ACIRenewalInviteInviteAttachmentTemplateId, 0)
            m_vRenewalInviteUpdateTemplateId = oResultArray(ACIRenewalInviteUpdateTemplateId, 0)
            m_vRenewalInviteUpdateAttachmentTemplateId = oResultArray(ACIRenewalInviteUpdateAttachmentTemplateId, 0)
            m_iIsRenewalUpdateEnabled = gPMFunctions.ToSafeInteger(oResultArray(ACIIsRenewalUpdateEnabled, 0))
            m_vRenewalUpdateManReviewTemplateId = oResultArray(ACIRenewalUpdateManReviewTemplateId, 0)
            m_vRenewalUpdateManReviewAttachmentTemplateId = oResultArray(ACIRenewalUpdateManReviewAttachmentTemplateId, 0)
            m_vRenewalUpdateInviteTemplateId = oResultArray(ACIRenewalUpdateInviteTemplateId, 0)
            m_vRenewalUpdateInviteAttachmentTemplateId = oResultArray(ACIRenewalUpdateInviteAttachmentTemplateId, 0)
            m_vRenewalUpdateUpdateTemplateId = oResultArray(ACIRenewalUpdateUpdateTemplateId, 0)
            m_vRenewalUpdateUpdateAttachmentTemplateId = oResultArray(ACIRenewalUpdateUpdateAttachmentTemplateId, 0)
            m_iIsAgentRenewalSelectionEnabled = gPMFunctions.ToSafeInteger(oResultArray(ACIIsAgentRenewalSelectionEnabled, 0))
            m_iIsAgentRenewalInviteEnabled = gPMFunctions.ToSafeInteger(oResultArray(ACIIsAgentRenewalInviteEnabled, 0))
            m_iIsAgentRenewalUpdateEnabled = gPMFunctions.ToSafeInteger(oResultArray(ACIIsAgentRenewalUpdateEnabled, 0))
            m_vAgentRenewalManReviewTemplateId = oResultArray(ACIAgentRenewalManReviewTemplateId, 0)
            m_vAgentRenewalManReviewReportId = oResultArray(ACIAgentRenewalManReviewReportId, 0)
            m_vAgentRenewalInviteTemplateId = oResultArray(ACIAgentRenewalInviteTemplateId, 0)
            m_vAgentRenewalInviteReportId = oResultArray(ACIAgentRenewalInviteReportId, 0)
            m_vAgentRenewalUpdateTemplateId = oResultArray(ACIAgentRenewalUpdateTemplateId, 0)
            m_vAgentRenewalUpdateReportId = oResultArray(ACIAgentRenewalUpdateReportId, 0)

            txtEmailComm(0).Text = gPMFunctions.ToSafeString(oResultArray(ACIRenewalSelectionManReviewTemplateCode, 0))
            txtEmailComm(1).Text = gPMFunctions.ToSafeString(oResultArray(ACIRenewalSelectionManReviewAttachmentTemplateCode, 0))
            txtEmailComm(2).Text = gPMFunctions.ToSafeString(oResultArray(ACIRenewalSelectionInviteTemplateCode, 0))
            txtEmailComm(3).Text = gPMFunctions.ToSafeString(oResultArray(ACIRenewalSelectionInviteAttachmentTemplateCode, 0))
            txtEmailComm(4).Text = gPMFunctions.ToSafeString(oResultArray(ACIRenewalSelectionUpdateTemplateCode, 0))
            txtEmailComm(5).Text = gPMFunctions.ToSafeString(oResultArray(ACIRenewalSelectionUpdateAttachmentTemplateCode, 0))
            txtEmailComm(6).Text = gPMFunctions.ToSafeString(oResultArray(ACIRenewalInviteManReviewTemplateCode, 0))
            txtEmailComm(7).Text = gPMFunctions.ToSafeString(oResultArray(ACIRenewalInviteManReviewAttachmentTemplateCode, 0))
            txtEmailComm(8).Text = gPMFunctions.ToSafeString(oResultArray(ACIRenewalInviteInviteTemplateCode, 0))
            txtEmailComm(9).Text = gPMFunctions.ToSafeString(oResultArray(ACIRenewalInviteInviteAttachmentTemplateCode, 0))
            txtEmailComm(10).Text = gPMFunctions.ToSafeString(oResultArray(ACIRenewalInviteUpdateTemplateCode, 0))
            txtEmailComm(11).Text = gPMFunctions.ToSafeString(oResultArray(ACIRenewalInviteUpdateAttachmentTemplateCode, 0))
            txtEmailComm(12).Text = gPMFunctions.ToSafeString(oResultArray(ACIRenewalUpdateManReviewTemplateCode, 0))
            txtEmailComm(13).Text = gPMFunctions.ToSafeString(oResultArray(ACIRenewalUpdateManReviewAttachmentTemplateCode, 0))
            txtEmailComm(14).Text = gPMFunctions.ToSafeString(oResultArray(ACIRenewalUpdateInviteTemplateCode, 0))
            txtEmailComm(15).Text = gPMFunctions.ToSafeString(oResultArray(ACIRenewalUpdateInviteAttachmentTemplateCode, 0))
            txtEmailComm(16).Text = gPMFunctions.ToSafeString(oResultArray(ACIRenewalUpdateUpdateTemplateCode, 0))
            txtEmailComm(17).Text = gPMFunctions.ToSafeString(oResultArray(ACIRenewalUpdateUpdateAttachmentTemplateCode, 0))
            txtEmailComm(18).Text = gPMFunctions.ToSafeString(oResultArray(ACIAgentRenewalManReviewTemplateCode, 0))
            txtEmailComm(19).Text = gPMFunctions.ToSafeString(oResultArray(ACIAgentRenewalInviteTemplateCode, 0))
            txtEmailComm(20).Text = gPMFunctions.ToSafeString(oResultArray(ACIAgentRenewalUpdateTemplateCode, 0))
            cboAgentReviewAttachment.Text = gPMFunctions.ToSafeString(oResultArray(ACIAgentRenewalManReviewReportCode, 0))
            cboAgentInviteAttachment.Text = gPMFunctions.ToSafeString(oResultArray(ACIAgentRenewalInviteReportCode, 0))
            cboAgentUpdateAttachment.Text = gPMFunctions.ToSafeString(oResultArray(ACIAgentRenewalUpdateReportCode, 0))

            If CStr(oResultArray(ACICPSelMultipleClaimPayments, 0)) = "1" Then
                m_vMultipleClaimsPayments = CStr(1)
                m_cMaxUnauthorisedClaimValue = gPMFunctions.ToSafeCurrency(oResultArray(ACICPSelMaxUnauthorisedClaimValue, 0), 0)
                m_iMaxNoofUnauthorisedClaimPayments = gPMFunctions.ToSafeInteger(oResultArray(ACICPSelMaxNoofUnauthorisedClaimPayments, 0), 0)
            End If

            m_vRunAuthorisationScriptsforClaimsPayments = gPMFunctions.ToSafeInteger(oResultArray(ACICPSelRunAuthorisationScriptsforClaimPayments, 0), 0)

            m_bIsReservesReadOnly = gPMFunctions.ToSafeBoolean(oResultArray.GetValue(ACICPSelIsReservesReadOnly, 0))
            m_bIsRecoveriesReadOnly = gPMFunctions.ToSafeBoolean(oResultArray.GetValue(ACICPSelIsRecoveriesReadOnly, 0))
            m_bIsPaymentsReadOnly = gPMFunctions.ToSafeBoolean(oResultArray.GetValue(ACICPSelIsPaymentsReadOnly, 0))

            m_DisplayRerateForQuoteAndNB = gPMFunctions.ToSafeBoolean(oResultArray.GetValue(ACICPDisplayRerateForQuoteAndNB, 0))
            m_DisplayRerateForCancellationsAndReinstatments = gPMFunctions.ToSafeBoolean(oResultArray.GetValue(ACICPDisplayRerateForCancellationsAndReinstatments, 0))
            m_DisplayRerateForMTA = gPMFunctions.ToSafeBoolean(oResultArray.GetValue(ACICPDisplayRerateForMTA, 0))
            m_DisplayReRateForRenewal = gPMFunctions.ToSafeBoolean(oResultArray.GetValue(ACICPDisplayRerateForRenewal, 0))

            autoRenewBackDatedMonthlyPolicy = gPMFunctions.ToSafeBoolean(oResultArray.GetValue(ACICPautoRenewBackDatedMonthlyPolicy, 0))

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oBusiness.GetAllowedCausation(v_lProductID:=m_lProductID, r_vResultArray:=m_vAllowedCausation) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_vCurrencyChange = oResultArray(ACICurrencyChange, 0)
            m_vLossCurrencyChange = oResultArray(ACILossCurrencyChange, 0)
            m_vChangePolicyNumberAtRenewal = oResultArray(ACIChangePolicyNumberAtRenewal, 0)

            ' claim transactions
            m_vSuppressClaimTransactionReserve = oResultArray(ACIProductSuppressClaimTransactionsReserves, 0)
            m_vSuppressClaimTransactionPayment = oResultArray(ACIProductSuppressClaimTransactionsPayments, 0)
            m_vSuppressClaimTransactionRecovery = oResultArray(ACIProductSuppressClaimTransactionsRecoveries, 0)

            m_vOrigSuppressClaimTransactionReserve = m_vSuppressClaimTransactionReserve
            m_vOrigSuppressClaimTransactionPayment = m_vSuppressClaimTransactionPayment
            m_vOrigSuppressClaimTransactionRecovery = m_vSuppressClaimTransactionRecovery

            m_vUnifiedRenewalDay = oResultArray(ACIUnifiedRenewalDay, 0)
            m_vLeadAllowConsolidateComm = oResultArray(ACILeadAllowConsolidateComm, 0)
            m_vLeadMonthInCycle = oResultArray(ACILeadMonthInCycle, 0)
            m_vLeadSuspenseAcct = oResultArray(ACILeadSuspenseAcct, 0)
            m_vSubAllowConsolidateComm = oResultArray(ACISubAllowConsolidateComm, 0)
            m_vSubMonthInCycle = oResultArray(ACISubMonthInCycle, 0)
            m_vSubSuspenseAcct = oResultArray(ACISubSuspenseAcct, 0)
            m_sPaymentMethod = gPMFunctions.ToSafeString(oResultArray(ACIPaymentMethod, 0), "")
            m_vBankAccountId = gPMFunctions.ToSafeLong(oResultArray(ACICPSelBankAccountId, 0))
            m_vClaimValueForLargeLossAdvice = gPMFunctions.ToSafeDouble(oResultArray(ACICPSelClaimValueForLargeLossAdvice, 0))
            m_vInclusionofCoInsurersOnClaims = gPMFunctions.ToSafeInteger(oResultArray(ACICPSelInclusionofCoInsurersOnClaims, 0))
            m_vAllowNegativeReserve = gPMFunctions.ToSafeInteger(oResultArray(ACICPSelAllowNegativeReserve, 0))
            m_vExtClmHandlerAcknowledgedTaskAllowedTime = gPMFunctions.ToSafeInteger(oResultArray(ACICPSelExtClmHandlerAcknowledgedTaskAllowedTime, 0))
            m_vExtClmHandlerSupplyPreReportTaskAllowedTime = gPMFunctions.ToSafeInteger(oResultArray(ACICPSelExtClmHandlerSupplyPreReportTaskAllowedTime, 0))
            m_vValidPolicyVersionAtLossDate = gPMFunctions.ToSafeInteger(oResultArray(ACICPSelValidPolicyVersionAtLossDate, 0))
            m_vIsGrossClaimPaymentAmount = gPMFunctions.ToSafeInteger(oResultArray(ACICPSelIsGrossClaimPaymentAmount, 0))
            m_vClaimTaskGroup = gPMFunctions.ToSafeInteger(oResultArray(ACICPSelClaimTaskGroup, 0))
            m_vClaimUserGroup = gPMFunctions.ToSafeInteger(oResultArray(ACICPSelClaimUserGroup, 0))
            m_vClaimsUDTA = gPMFunctions.ToSafeInteger(oResultArray(ACICPSelClaimsUDTA, 0))
            m_vClaimsUDTB = gPMFunctions.ToSafeInteger(oResultArray(ACICPSelClaimsUDTB, 0))
            m_vClaimsUDTC = gPMFunctions.ToSafeInteger(oResultArray(ACICPSelClaimsUDTC, 0))
            m_vClaimsUDTD = gPMFunctions.ToSafeInteger(oResultArray(ACICPSelClaimsUDTD, 0))
            m_vClaimsUDTE = gPMFunctions.ToSafeInteger(oResultArray(ACICPSelClaimsUDTE, 0))
            m_vIsDuplicateClaimCheckEnabled = gPMFunctions.ToSafeInteger(oResultArray(ACICPSelIsDuplicateClaimCheckEnabled, 0))
            m_vIsAdvancedTaxScriptEnabled = gPMFunctions.ToSafeInteger(oResultArray(ACICPSelIsAdvancedTaxScriptEnabled, 0))
            m_vIsPaymentRefCheckEnabled = gPMFunctions.ToSafeInteger(oResultArray(ACICPSelIsPaymentRefCheckEnabled, 0))
            m_vIsRecommender = gPMFunctions.ToSafeInteger(oResultArray(ACICPSelIsRecommender, 0))
            m_iMTA_dateAllowed = gPMFunctions.ToSafeInteger(oResultArray(ACICPSelDateAllowed, 0))
            m_iMTA_Allocation = gPMFunctions.ToSafeInteger(oResultArray(ACICPSelAllocation, 0))
            m_vDefaultRenMonths = gPMFunctions.ToSafeInteger(oResultArray(ACICPSelRenewalMonths, 0))
            m_vPaymentCannotExceedReserve = gPMFunctions.ToSafeInteger(oResultArray(ACICPSelPaymentCannotExceedReserve, 0))
            m_bMTCRatingRulesEnabled = CInt(oResultArray(ACICPSelMTCRatingRulesEnabled, 0)) = 1
            m_vAllowBackdatedMTAs = CInt(oResultArray(ACICPSelAllowBackdatedMTAs, 0)) = 1
            m_vAllowBackdatedCan = CInt(oResultArray(ACICPSelAllowBackdatedCan, 0)) = 1
            m_vCNDefaultPeriod = oResultArray(ACICPSelCNDefaultPeriod, 0)
            m_vCNMaxNo = oResultArray(ACICPSelCNMaxNo, 0)
            m_vCNDocTemplateId = gPMFunctions.ToSafeLong(oResultArray(ACICPSelCNDocTemplateID, 0))
            m_vCNTemplateCode = gPMFunctions.ToSafeString(oResultArray(ACICPSelCNCode, 0))
            m_vOutOfsequenceMtaUserGroup = gPMFunctions.ToSafeInteger(oResultArray(ACICPSelOutOfSequencemtaUsergroup, 0))
            m_vOutOfsequenceMtaTaskGroup = gPMFunctions.ToSafeInteger(oResultArray(ACICPSelOutOfSequencemtaTaskgroup, 0))
            m_bRoundOffToZero = CInt(oResultArray(ACICPSelRoundOffToZero, 0)) = 1
            m_vCheckMediaTypeStatusAtClaimPayment = oResultArray(ACICPSelCheckMediaTypeStatusAtClaimPayment, 0)
            m_vCheckMediaTypeStatusAtPolicyRefund = oResultArray(ACICPSelCheckMediaTypeStatusAtPolicyRefund, 0)
            m_vChangePolicyNumberAtRenewalAutomatically = oResultArray(ACICPSelChangePolicyNumberAtRenewalAutomatically, 0)
            m_vRIManualPremiumAdjustment = gPMFunctions.ToSafeInteger(oResultArray(ACICPSelRIManualPremiumAdjustment, 0))
            m_vTMPAutoRenFAC = ToSafeString(oResultArray(ACICPSelTMPAutoRenFac, 0))
            m_iReminderTaskGroup = gPMFunctions.ToSafeInteger(oResultArray(ACIPSelReminderTaskGroup, 0))
            m_iReminderUserGroup = gPMFunctions.ToSafeInteger(oResultArray(ACIPSelReminderUserGroup, 0))

            If CInt(oResultArray(ACIPSelWrittenPolicyStatus, 0)) = 1 Then
                m_bWrittenPolicyStatus = True
            Else
                m_bWrittenPolicyStatus = False
            End If

            m_iTaskManagerDays = gPMFunctions.ToSafeInteger(oResultArray(ACIPSelTaskManagerDays, 0))
            m_vIsBindRenewalWithoutInvitation = gPMFunctions.ToSafeInteger(oResultArray(ACICPSelBindRenewalWithoutInvitation, 0))
            m_vProductEnablePrePayment = ToSafeString(oResultArray(ACICPSelEnablePrePayment, 0))
            m_sDefaultCoverToDateToLastDay = ToSafeString(oResultArray(kICPDefaultCoverToDateToLastDay, 0))
            m_vDoNotDeleteRenewalQuoteOnMTA = ToSafeInteger(oResultArray(ACICPDoNotDeleteRenewalQuoteOnMTA, 0))
            m_oUnifiedrenewalDateIsReadOnly = ToSafeString(oResultArray(kICPUnifiedrenewalDateIsReadOnly, 0))
            m_sDefaultCoverToDateToLastDay = ToSafeString(oResultArray(kICPDefaultCoverToDateToLastDay, 0))
            m_vDeleteRenewalQuoteReRunOnMTA = ToSafeInteger(oResultArray(ACICPDeleteRenewalQuoteReRunOnMTA, 0))
            m_bRetainPolicyNumberonCopy = ToSafeBoolean(oResultArray(ACICPSelRetainPolicyNumberonCopy, 0))
            m_EditAnnivDate = gPMFunctions.ToSafeBoolean(oResultArray.GetValue(ACICPEditAnnivDate, 0))
            m_bDisableCoverStartDateOnREN = gPMFunctions.ToSafeBoolean(oResultArray.GetValue(ACICPDisableCoverStartDateOnREN, 0))
            m_bUsePolicyInceptionDate = ToSafeBoolean(oResultArray(ACICPSelUsePolicyInceptionDate, 0))
            'm_vAuthorisationThreshold = oResultArray(ACICPSelAuthorisationThreshold, 0)
            m_bVoidTransaction = gPMFunctions.ToSafeBoolean(oResultArray.GetValue(ACICPSelVoidTransaction, 0))
            m_iIsQuoteVersioning = gPMFunctions.ToSafeInteger(oResultArray.GetValue(ACICPSelIsQuoteVersioning, 0))
            m_iDeleteQuoteAfter = gPMFunctions.ToSafeInteger(oResultArray.GetValue(ACICPSelDeleteQuoteAfter, 0))
            If oResultArray.GetUpperBound(0) >= ACICPSelRecoveryInstalmentsEnabled Then
                m_vRecoveryInstalmentsEnabled = gPMFunctions.ToSafeInteger(oResultArray.GetValue(ACICPSelRecoveryInstalmentsEnabled, 0))
            Else
                m_vRecoveryInstalmentsEnabled = 0
            End If
            Return nResult


        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult
        End Try
    End Function

    ''' <summary>
    ''' BusinessToInterface
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function BusinessToInterface() As Integer

        Dim nResult As Integer
        Dim bFound As Boolean
        Dim oResultArray(,) As Object = Nothing

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            nResult = m_oFormFields.FormatControl(ctlControl:=txtCode, vControlValue:=m_sCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return nResult
            End If

            nResult = m_oBusiness.GetSuspendedTransaction(m_lProductID, oResultArray)
            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return nResult
            End If

            If Information.IsArray(oResultArray) Then
                chkAllowConsolidateCommissionLA.CheckState = CheckState.Checked
                chkAllowConsolidateCommissionSA.CheckState = CheckState.Checked
                cboMonthInCycleLA.Enabled = False
                actSuspenseAcc.Enabled = False
                lblMonthInCycleLA.Enabled = False
                lblLeadAgentCommSuspenseLA.Enabled = False
                cboMonthInCycleSA.Enabled = False
                actSuspenseAcc1.Enabled = False
                lblMonthInCycleSA.Enabled = False
                lblSubAgentCommSuspense.Enabled = False
            End If
            '---------------------------------------------------------------------------------
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtEffectiveDate, vControlValue:=m_dtProductEffectiveDate)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Convert.IsDBNull(m_vRenewalPeriod) Or IsNothing(m_vRenewalPeriod) Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtRenewalPeriod, vControlValue:="")
            Else
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtRenewalPeriod, vControlValue:=m_vRenewalPeriod)
            End If

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Convert.IsDBNull(m_vRIPointer) Or IsNothing(m_vRIPointer) Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtRIPointer, vControlValue:="")
            Else
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtRIPointer, vControlValue:=m_vRIPointer)
            End If

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Convert.IsDBNull(m_vReportPointer) Or IsNothing(m_vReportPointer) Then
                txtReportPointer.Text = ""
            Else
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtReportPointer, vControlValue:=m_vReportPointer)
            End If

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Convert.IsDBNull(m_vDescription) Or IsNothing(m_vDescription) Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDescription, vControlValue:="")
            Else
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDescription, vControlValue:=m_vDescription)
            End If

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Convert.IsDBNull(m_vSchemeAgencyRef) Or IsNothing(m_vSchemeAgencyRef) Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtSchemeAgencyRef, vControlValue:="")
            Else
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtSchemeAgencyRef, vControlValue:=m_vSchemeAgencyRef)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Convert.IsDBNull(m_vBlockNo) Or IsNothing(m_vBlockNo) Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtBlockNo, vControlValue:="")
            Else
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtBlockNo, vControlValue:=m_vBlockNo)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Convert.IsDBNull(m_vGracePeriod) Or IsNothing(m_vGracePeriod) Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtGracePeriod, vControlValue:="")
            Else
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtGracePeriod, vControlValue:=m_vGracePeriod)
            End If

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If CStr(m_vPreventCancelledAgents) = "1" Then
                chkCheckAgent.CheckState = CheckState.Checked
            Else
                chkCheckAgent.CheckState = CheckState.Unchecked
            End If

            If Convert.IsDBNull(m_vAllowPositiveCancellation) Or IsNothing(m_vAllowPositiveCancellation) Then
                cboPosValues.SelectedIndex = gPMConstants.PM_ALLOW
            Else
                cboPosValues.SelectedIndex = CInt(m_vAllowPositiveCancellation)
            End If

            If CStr(m_vMediaTypeMandatory) = "1" Then
                chkMediaTypeMandatory.CheckState = CheckState.Checked
            Else
                chkMediaTypeMandatory.CheckState = CheckState.Unchecked
            End If

            If Not (Convert.IsDBNull(m_vPolicyStyleID) Or IsNothing(m_vPolicyStyleID)) Then

                cboPolicyStyle.ItemId = CInt(m_vPolicyStyleID)
            Else
                cboPolicyStyle.ItemId = 0
            End If

            If CBool(m_vPolicyStyleMandatory) Then
                chkPolicyStyleMandatory.CheckState = CheckState.Checked
            Else
                chkPolicyStyleMandatory.CheckState = CheckState.Unchecked
            End If

            'check and select provisional claim auto numbering from combobox
            bFound = False

            If Not (Convert.IsDBNull(m_vProvClaimAutoNumberingID) Or IsNothing(m_vProvClaimAutoNumberingID)) Then
                For iCounter As Integer = 0 To Me.cboProvClaimAutoNumberingID.Items.Count - 1
                    Me.cboProvClaimAutoNumberingID.SelectedIndex = iCounter

                    If VB6.GetItemData(Me.cboProvClaimAutoNumberingID, iCounter) = CInt(m_vProvClaimAutoNumberingID) Then
                        bFound = True
                        Exit For
                    End If
                Next
            End If

            If Not bFound Then
                Me.cboProvClaimAutoNumberingID.SelectedIndex = 0
            End If

            'check and select quote auto numbering from combobox
            bFound = False

            If Not (Convert.IsDBNull(m_vQuoteAutoNumberingID) Or IsNothing(m_vQuoteAutoNumberingID)) Then
                For iCounter As Integer = 0 To Me.cboQuoteAutoNumberingID.Items.Count - 1
                    Me.cboQuoteAutoNumberingID.SelectedIndex = iCounter

                    If VB6.GetItemData(Me.cboQuoteAutoNumberingID, iCounter) = CInt(m_vQuoteAutoNumberingID) Then
                        bFound = True
                        Exit For
                    End If
                Next
            End If

            If Not bFound Then
                Me.cboQuoteAutoNumberingID.SelectedIndex = 0
            End If

            'check and select policy auto numbering from combobox
            bFound = False

            If Not (Convert.IsDBNull(m_vPolicyAutoNumberingID) Or IsNothing(m_vPolicyAutoNumberingID)) Then
                For iCounter As Integer = 0 To Me.cboPolicyAutoNumberingID.Items.Count - 1
                    Me.cboPolicyAutoNumberingID.SelectedIndex = iCounter

                    If VB6.GetItemData(Me.cboPolicyAutoNumberingID, iCounter) = CInt(m_vPolicyAutoNumberingID) Then
                        bFound = True
                        Exit For
                    End If
                Next
            End If

            If Not bFound Then
                Me.cboPolicyAutoNumberingID.SelectedIndex = 0
            End If

            'check and select full claim auto numbering from combobox
            bFound = False

            If Not (Convert.IsDBNull(m_vFullClaimAutoNumberingID) Or IsNothing(m_vFullClaimAutoNumberingID)) Then
                For iCounter As Integer = 0 To Me.cboFullClaimAutoNumberingID.Items.Count - 1
                    Me.cboFullClaimAutoNumberingID.SelectedIndex = iCounter

                    If VB6.GetItemData(Me.cboFullClaimAutoNumberingID, iCounter) = CInt(m_vFullClaimAutoNumberingID) Then
                        bFound = True
                        Exit For
                    End If
                Next
            End If

            If Not bFound Then
                Me.cboFullClaimAutoNumberingID.SelectedIndex = 0
            End If

            bFound = False

            If Not (Convert.IsDBNull(m_vCNCoverNoteNumberingID) Or IsNothing(m_vCNCoverNoteNumberingID)) Then
                For iCounter As Integer = 0 To Me.cboCNAutoNumberingID.Items.Count - 1
                    Me.cboCNAutoNumberingID.SelectedIndex = iCounter

                    If VB6.GetItemData(Me.cboCNAutoNumberingID, iCounter) = CInt(m_vCNCoverNoteNumberingID) Then
                        bFound = True
                        Exit For
                    End If
                Next
            End If

            If Not bFound Then
                Me.cboCNAutoNumberingID.SelectedIndex = 0
            End If

            bFound = False
            If Not IsDBNull(m_vMandatoryRiskTypeId) And m_vMandatoryRiskTypeId > 0 Then
                For iCounter As Integer = 0 To Me.cboApplyMandatoryRisk.ListCount - 1
                    Me.cboApplyMandatoryRisk.ListIndex = iCounter
                    If (Me.cboApplyMandatoryRisk.ItemData(iCounter) = CLng(m_vMandatoryRiskTypeId)) Then
                        bFound = True
                        Exit For
                    End If
                Next
            End If

            If (Not bFound) Then
                Me.cboApplyMandatoryRisk.ListIndex = 0
            End If

            If Not (Convert.IsDBNull(m_vMidnightRenewal) Or IsNothing(m_vMidnightRenewal)) Then

                Me.chkMidNightRenewal.CheckState = CInt(m_vMidnightRenewal)
            Else
                Me.chkMidNightRenewal.CheckState = CheckState.Unchecked
            End If

            If Not (Convert.IsDBNull(m_vAutoRenewal) Or IsNothing(m_vAutoRenewal)) Then
                Me.chkAutoRenewal.CheckState = CInt(m_vAutoRenewal)
            Else
                Me.chkAutoRenewal.CheckState = CheckState.Unchecked
            End If

            If Not (Convert.IsDBNull(m_vTaxSuppressed) Or IsNothing(m_vTaxSuppressed)) Then
                Me.chkTaxSuppressed.CheckState = CInt(m_vTaxSuppressed)
            Else
                Me.chkTaxSuppressed.CheckState = CheckState.Unchecked
            End If

            If Not (Convert.IsDBNull(m_vShortPeriodRated) Or IsNothing(m_vShortPeriodRated)) Then
                Me.chkShortPeriodRated.CheckState = CInt(m_vShortPeriodRated)
            Else
                Me.chkShortPeriodRated.CheckState = CheckState.Unchecked
            End If

            If Not (Convert.IsDBNull(m_vAccumulation) Or IsNothing(m_vAccumulation)) Then
                Me.chkAccumulation.CheckState = CInt(m_vAccumulation)
            Else
                Me.chkAccumulation.CheckState = CheckState.Unchecked
            End If

            If Not (Convert.IsDBNull(m_vNBProrata) Or IsNothing(m_vNBProrata)) Then

                Me.chkNBProRata.CheckState = CInt(m_vNBProrata)
            Else
                Me.chkNBProRata.CheckState = CheckState.Unchecked
            End If

            If Not (Convert.IsDBNull(m_vMTAProrata) Or IsNothing(m_vMTAProrata)) Then

                Me.chkMTAProRata.CheckState = CInt(m_vMTAProrata)
            Else
                Me.chkMTAProRata.CheckState = CheckState.Unchecked
            End If

            If Not (Convert.IsDBNull(m_vRoundPremium) Or IsNothing(m_vRoundPremium)) Then

                Me.chkRoundPremium.CheckState = CInt(m_vRoundPremium)
            Else
                Me.chkRoundPremium.CheckState = CheckState.Unchecked
            End If

            If Not (Convert.IsDBNull(m_vRoundingSection) Or IsNothing(m_vRoundingSection)) Then

                Me.cboRoundingSection.ItemId = CInt(m_vRoundingSection)
            Else
                Me.cboRoundingSection.ItemId = 0
            End If

            If Not (Convert.IsDBNull(m_vPolicyNumberAtQuote) Or IsNothing(m_vPolicyNumberAtQuote)) Then
                chkPolicyNumberAtQuote.CheckState = CInt(m_vPolicyNumberAtQuote)
            Else
                chkPolicyNumberAtQuote.CheckState = CheckState.Unchecked
            End If

            If Not (Convert.IsDBNull(m_vAllowStandardWordingEdit) Or IsNothing(m_vAllowStandardWordingEdit)) Then
                chkAllowStandardWordingEdit.CheckState = CInt(m_vAllowStandardWordingEdit)
            Else
                chkAllowStandardWordingEdit.CheckState = CheckState.Unchecked
            End If

            'enable short period rate if short period rate is checked
            'and we are not in adding mode
            If Me.chkShortPeriodRated.CheckState And Task <> gPMConstants.PMEComponentAction.PMAdd Then
                Me.cmdSPR.Enabled = True
            End If

            'check and select risk type group from combobox
            'there should only be one risk type group (current business rule)

            If Information.IsArray(m_vAllowedRiskTypeGroup) Then
                cboRiskTypeGroup.DefaultItemId = CInt(m_vAllowedRiskTypeGroup(0, 0))
            End If

            cboRiskTypeGroup.RefreshList()

            If Convert.IsDBNull(m_vClaimYearToCheck) Or IsNothing(m_vClaimYearToCheck) Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtClaimYear, vControlValue:="")
            Else
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtClaimYear, vControlValue:=m_vClaimYearToCheck)
            End If

            If Convert.IsDBNull(m_vMaxNumberOfClaim) Or IsNothing(m_vMaxNumberOfClaim) Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAllowedClaims, vControlValue:="")
            Else
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAllowedClaims, vControlValue:=m_vMaxNumberOfClaim)
            End If

            If Convert.IsDBNull(m_vMaxSingleClaimValue) Or IsNothing(m_vMaxSingleClaimValue) Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtSingleClaimValue, vControlValue:="")
            Else
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtSingleClaimValue, vControlValue:=m_vMaxSingleClaimValue)
            End If

            If Convert.IsDBNull(m_vMaxTotalClaimValue) Or IsNothing(m_vMaxTotalClaimValue) Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtTotalClaimsValue, vControlValue:="")
            Else
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtTotalClaimsValue, vControlValue:=m_vMaxTotalClaimValue)
            End If

            If Not (Convert.IsDBNull(m_vCurrencyChange) Or IsNothing(m_vCurrencyChange)) Then
                Me.chkCurrencyChange.CheckState = CInt(m_vCurrencyChange)
            Else
                Me.chkCurrencyChange.CheckState = CheckState.Unchecked
            End If

            If Not (Convert.IsDBNull(m_vLossCurrencyChange) Or IsNothing(m_vLossCurrencyChange)) Then
                chkLossCurrencyChange.CheckState = CInt(m_vLossCurrencyChange)
            Else
                chkLossCurrencyChange.CheckState = CheckState.Unchecked
            End If

            If Not (Convert.IsDBNull(m_vChangePolicyNumberAtRenewal) Or IsNothing(m_vChangePolicyNumberAtRenewal)) Then
                chkChangePolicyNumberAtRenewal.CheckState = CInt(m_vChangePolicyNumberAtRenewal)
                chkChangePolicyNumberAtRenewalAutomatically.Enabled = chkChangePolicyNumberAtRenewal.CheckState
                chkChangePolicyNumberAtRenewalAutomatically.CheckState = gPMFunctions.ToSafeInteger(m_vChangePolicyNumberAtRenewalAutomatically)
            Else
                chkChangePolicyNumberAtRenewal.CheckState = CheckState.Unchecked
                chkChangePolicyNumberAtRenewalAutomatically.CheckState = CheckState.Unchecked
                chkChangePolicyNumberAtRenewalAutomatically.Enabled = False
            End If

            If Not (Convert.IsDBNull(m_vHideSummaryAtRenewalAcceptance) Or IsNothing(m_vHideSummaryAtRenewalAcceptance)) Then
                chkHideSummaryAtRenewalAcceptance.CheckState = CInt(m_vHideSummaryAtRenewalAcceptance)
            Else
                chkHideSummaryAtRenewalAcceptance.CheckState = CheckState.Unchecked
            End If

            If Not (Convert.IsDBNull(m_vUseNBRenPaymentTermsAtSelection) Or IsNothing(m_vUseNBRenPaymentTermsAtSelection)) Then
                chkUseNBRenPaymentTermsAtSelection.CheckState = CInt(m_vUseNBRenPaymentTermsAtSelection)
            Else
                chkUseNBRenPaymentTermsAtSelection.CheckState = CheckState.Unchecked
            End If

            chkDoNotDeleteRenewalQuoteOnMTA.CheckState = m_vDoNotDeleteRenewalQuoteOnMTA

            If Not (Convert.IsDBNull(m_vDoNotDeleteRenewalQuoteOnMTA) OrElse IsNothing(m_vDoNotDeleteRenewalQuoteOnMTA)) Then


                chkDoNotDeleteRenewalQuoteOnMTA.CheckState = CInt(m_vDoNotDeleteRenewalQuoteOnMTA)
            Else
                chkDoNotDeleteRenewalQuoteOnMTA.CheckState = CheckState.Unchecked
            End If

            If chkDoNotDeleteRenewalQuoteOnMTA.Checked Then
                chkDeleteRenewalQuoteReRunOnMTA.CheckState = CheckState.Unchecked
            Else
                chkDeleteRenewalQuoteReRunOnMTA.CheckState = m_vDeleteRenewalQuoteReRunOnMTA

                If Not (Convert.IsDBNull(m_vDeleteRenewalQuoteReRunOnMTA) OrElse IsNothing(m_vDeleteRenewalQuoteReRunOnMTA)) Then


                    chkDeleteRenewalQuoteReRunOnMTA.CheckState = CInt(m_vDeleteRenewalQuoteReRunOnMTA)
                Else
                    chkDeleteRenewalQuoteReRunOnMTA.CheckState = CheckState.Unchecked
                End If

            End If


            If Not (Convert.IsDBNull(m_vUsePriorTermSchemeAtRenewal) Or IsNothing(m_vUsePriorTermSchemeAtRenewal)) Then
                chkUsePriorSchemeAtRenewal.CheckState = CInt(m_vUsePriorTermSchemeAtRenewal)
            Else
                chkUsePriorSchemeAtRenewal.CheckState = CheckState.Unchecked
            End If
            chkIsReservesReadonly.Checked = m_bIsReservesReadOnly
            chkIsRecoveriesReadonly.Checked = m_bIsRecoveriesReadOnly
            chkIsPaymentsReadonly.Checked = m_bIsPaymentsReadOnly

            chkDisplayRerateForQuoteAndNB.Checked = m_DisplayRerateForQuoteAndNB
            chkDisplayRerateForCancellationsAndReinstatments.Checked = m_DisplayRerateForCancellationsAndReinstatments
            chkDisplayRerateForMTA.Checked = m_DisplayRerateForMTA
            chkDisplayRerateForRenewal.Checked = m_DisplayReRateForRenewal

            chkAutoRenBDMonthlyPol.Checked = autoRenewBackDatedMonthlyPolicy
            chkEditAnnivDate.Checked = m_EditAnnivDate
            chkDisableCoverStartDateonREN.Checked = m_bDisableCoverStartDateOnREN
            'If Not (Convert.IsDBNull(m_DisplayRerateForQuoteAndNB) Or IsNothing(m_DisplayRerateForQuoteAndNB)) Then
            '    chkDisplayRerateForQuoteAndNB.Checked = CInt(m_DisplayRerateForQuoteAndNB)
            'Else
            '    chkDisplayRerateForQuoteAndNB.Checked = CheckState.Unchecked
            'End If

            'If Not (Convert.IsDBNull(m_DisplayRerateForMTA) Or IsNothing(m_DisplayRerateForMTA)) Then
            '    chkDisplayRerateForMTA.Checked = CInt(m_DisplayRerateForMTA)
            'Else
            '    chkDisplayRerateForMTA.Checked = CheckState.Unchecked
            'End If

            'If Not (Convert.IsDBNull(m_DisplayRerateForCancellationsAndReinstatments) Or IsNothing(m_DisplayRerateForCancellationsAndReinstatments)) Then
            '    chkDisplayRerateForCancellationsAndReinstatments.Checked = CInt(m_DisplayRerateForCancellationsAndReinstatments)
            'Else
            '    chkDisplayRerateForCancellationsAndReinstatments.Checked = CheckState.Unchecked
            'End If

            '---------------------------------------------------------------------------------
            'Value assigned to interface for TMP
            '---------------------------------------------------------------------------------
            txtUnifiedRenewalDay.Text = CStr(gPMFunctions.ToSafeLong(m_vUnifiedRenewalDay))
            If txtUnifiedRenewalDay.Text.Trim() = "0" Then txtUnifiedRenewalDay.Text = ""
            chkAllowConsolidateCommissionLA.CheckState = gPMFunctions.ToSafeLong(m_vLeadAllowConsolidateComm)
            cboMonthInCycleLA.Text = CStr(gPMFunctions.ToSafeLong(m_vLeadMonthInCycle))
            actSuspenseAcc.Text = gPMFunctions.ToSafeString(m_vLeadSuspenseAcct)
            chkAllowConsolidateCommissionSA.CheckState = gPMFunctions.ToSafeLong(m_vSubAllowConsolidateComm)
            cboMonthInCycleSA.Text = CStr(gPMFunctions.ToSafeLong(m_vSubMonthInCycle))
            actSuspenseAcc1.Text = gPMFunctions.ToSafeString(m_vSubSuspenseAcct)
            chkTrueMonthlyPolicy.CheckState = gPMFunctions.ToSafeLong(m_vProductIsTrueMonthlyPolicy, 0)
            ' anniversary renewal weeks
            txtAnniversaryRenewalWeeks.Text = CStr(gPMFunctions.ToSafeLong(m_vProductAnniversaryRenewalWeeks, 0))

            If m_sPaymentMethod = "Invoice" Then
                OptInvoice.Checked = True
            End If
            If m_sPaymentMethod = "Instalments" Then
                OptInstalments.Checked = True
            End If

            SetupTrueMonthlyPolicy()

            ' get reserve details
            If CStr(m_vSuppressClaimTransactionReserve) = "1" Then
                chkReserve.CheckState = CheckState.Checked
            Else
                chkReserve.CheckState = CheckState.Unchecked
            End If

            ' get payment details
            If CStr(m_vSuppressClaimTransactionPayment) = "1" Then
                chkPayments.CheckState = CheckState.Checked
            Else
                chkPayments.CheckState = CheckState.Unchecked
            End If

            ' get recovery details

            If CStr(m_vSuppressClaimTransactionRecovery) = "1" Then
                chkRecoveries.CheckState = CheckState.Checked
            Else
                chkRecoveries.CheckState = CheckState.Unchecked
            End If

            'Get Pre-Payment Details
            If m_iCanMakeLiveInvoice = 1 Then
                chkInvoice.CheckState = CheckState.Checked
            Else
                chkInvoice.CheckState = CheckState.Unchecked
            End If

            If m_iCanMakeLiveInstalments = 1 Then
                chkInstalments.CheckState = CheckState.Checked
            Else
                chkInstalments.CheckState = CheckState.Unchecked
            End If

            If m_iCanMakeLivePaynow = 1 Then
                chkPayNow.CheckState = CheckState.Checked
            Else
                chkPayNow.CheckState = CheckState.Unchecked
            End If

            If m_iCanMakeLiveBankGuarantee = 1 Then
                chkBankGuarantee.CheckState = CheckState.Checked
            Else
                chkBankGuarantee.CheckState = CheckState.Unchecked
            End If

            If m_iCanMakeLiveCashDeposit = 1 Then
                chkCashDeposit.CheckState = CheckState.Checked
            Else
                chkCashDeposit.CheckState = CheckState.Unchecked
            End If

            'Get Produce Documents Detail
            If m_iProduceSchedule = 1 Then
                chkProduceSchedule.CheckState = CheckState.Checked
            Else
                chkProduceSchedule.CheckState = CheckState.Unchecked
            End If

            If m_iProduceCertificate = 1 Then
                chkProduceCertificate.CheckState = CheckState.Checked
            Else
                chkProduceCertificate.CheckState = CheckState.Unchecked
            End If

            If m_iProduceDebitNote = 1 Then
                chkProduceDebitNote.CheckState = CheckState.Checked
            Else
                chkProduceDebitNote.CheckState = CheckState.Unchecked
            End If

            If Convert.IsDBNull(m_vOnlineCommencedOn) Or IsNothing(m_vOnlineCommencedOn) Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtOnlineCommencedOn, vControlValue:="")
            Else
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtOnlineCommencedOn, vControlValue:=m_vOnlineCommencedOn)
            End If
            txtOnlineCommencedOn.Enabled = False

            If m_iTradeNBOnline = 1 Then
                chkTradeNbOnline.CheckState = CheckState.Checked
            Else
                chkTradeNbOnline.CheckState = CheckState.Unchecked
            End If
            chkTradeNbOnline.Enabled = False

            If m_iTradeMTAOnline = 1 Then
                chkTradeMtaOnline.CheckState = CheckState.Checked
            Else
                chkTradeMtaOnline.CheckState = CheckState.Unchecked
            End If
            chkTradeMtaOnline.Enabled = False

            If m_iTradeRNLOnline = 1 Then
                chkTradeRnlOnline.CheckState = CheckState.Checked
            Else
                chkTradeRnlOnline.CheckState = CheckState.Unchecked
            End If
            chkTradeRnlOnline.Enabled = False

            ' setup interface
            SetupClaimTransactionSuppression()

            chkRenewable.CheckState = m_iIsRenewable
            chkEnabledRenSelection.CheckState = m_iIsRenewalSelectionEnabled

            If m_iTrueMonthlyPolicyRenewalCommunication = 1 Then
                optRenewalProcessRun.Checked = True
                optAnniversaryDate.Checked = False
            ElseIf m_iTrueMonthlyPolicyRenewalCommunication = 2 Then
                optAnniversaryDate.Checked = True
                optRenewalProcessRun.Checked = False
            Else
                optRenewalProcessRun.Checked = False
                optAnniversaryDate.Checked = False
            End If

            txtEmailComm(0).Tag = CStr(gPMFunctions.ToSafeLong(m_vRenewalSelectionManReviewTemplateId))
            txtEmailComm(1).Tag = CStr(gPMFunctions.ToSafeLong(m_vRenewalSelectionManReviewAttachmentTemplateId))
            txtEmailComm(2).Tag = CStr(gPMFunctions.ToSafeLong(m_vRenewalSelectionInviteTemplateId))
            txtEmailComm(3).Tag = CStr(gPMFunctions.ToSafeLong(m_vRenewalSelectionInviteAttachmentTemplateId))
            txtEmailComm(4).Tag = CStr(gPMFunctions.ToSafeLong(m_vRenewalSelectionUpdateTemplateId))
            txtEmailComm(5).Tag = CStr(gPMFunctions.ToSafeLong(m_vRenewalSelectionUpdateAttachmentTemplateId))
            chkEnabledRenInvite.CheckState = m_iIsRenewalInviteEnabled
            txtEmailComm(6).Tag = CStr(gPMFunctions.ToSafeLong(m_vRenewalInviteManReviewTemplateId))
            txtEmailComm(7).Tag = CStr(gPMFunctions.ToSafeLong(m_vRenewalInviteManReviewAttachmentTemplateId))
            txtEmailComm(8).Tag = CStr(gPMFunctions.ToSafeLong(m_vRenewalInviteInviteTemplateId))
            txtEmailComm(9).Tag = CStr(gPMFunctions.ToSafeLong(m_vRenewalInviteInviteAttachmentTemplateId))
            txtEmailComm(10).Tag = CStr(gPMFunctions.ToSafeLong(m_vRenewalInviteUpdateTemplateId))
            txtEmailComm(11).Tag = CStr(gPMFunctions.ToSafeLong(m_vRenewalInviteUpdateAttachmentTemplateId))
            chkEnabledRenUpdate.CheckState = m_iIsRenewalUpdateEnabled
            txtEmailComm(12).Tag = CStr(gPMFunctions.ToSafeLong(m_vRenewalUpdateManReviewTemplateId))
            txtEmailComm(13).Tag = CStr(gPMFunctions.ToSafeLong(m_vRenewalUpdateManReviewAttachmentTemplateId))
            txtEmailComm(14).Tag = CStr(gPMFunctions.ToSafeLong(m_vRenewalUpdateInviteTemplateId))
            txtEmailComm(15).Tag = CStr(gPMFunctions.ToSafeLong(m_vRenewalUpdateInviteAttachmentTemplateId))
            txtEmailComm(16).Tag = CStr(gPMFunctions.ToSafeLong(m_vRenewalUpdateUpdateTemplateId))
            txtEmailComm(17).Tag = CStr(gPMFunctions.ToSafeLong(m_vRenewalUpdateUpdateAttachmentTemplateId))
            chkAgentRenSelection.CheckState = gPMFunctions.ToSafeLong(m_iIsAgentRenewalSelectionEnabled)
            chkAgentRenInvite.CheckState = m_iIsAgentRenewalInviteEnabled
            chkAgentRenUpdate.CheckState = m_iIsAgentRenewalUpdateEnabled
            txtEmailComm(18).Tag = CStr(gPMFunctions.ToSafeLong(m_vAgentRenewalManReviewTemplateId))
            txtEmailComm(19).Tag = CStr(gPMFunctions.ToSafeLong(m_vAgentRenewalInviteTemplateId))
            txtEmailComm(20).Tag = gPMFunctions.ToSafeString(m_vAgentRenewalUpdateTemplateId)

            If m_vMultipleClaimsPayments = "1" Then
                chkMultiplePayments.CheckState = CheckState.Checked
                txtMaxNoofUnauthorisedClaimPayments.Text = CStr(gPMFunctions.ToSafeInteger(m_iMaxNoofUnauthorisedClaimPayments))
                txtMaxUnauthorisedClaimsValue.Text = StringsHelper.Format(gPMFunctions.ToSafeCurrency(m_cMaxUnauthorisedClaimValue), "0.00")
            Else
                chkMultiplePayments.CheckState = CheckState.Unchecked
            End If

            chkRunAuthorisationScriptsClaimPayments.CheckState = CInt(m_vRunAuthorisationScriptsforClaimsPayments)
            m_lReturn = SetRenewalOnlineOptions()

            If CDbl(m_vBankAccountId) > 0 Then
                For iCounter As Integer = 1 To cboBankAccount.ListCount - 1
                    If cboBankAccount.ItemData(iCounter) = m_vBankAccountId Then
                        cboBankAccount.ListIndex = iCounter
                        Exit For
                    End If
                Next iCounter
            Else
                cboBankAccount.ListIndex = 0
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtLargeLossAdviceValue, vControlValue:=m_vClaimValueForLargeLossAdvice)

            cboCoinsurerInclusion.SelectedIndex = CInt(m_vInclusionofCoInsurersOnClaims)

            chkAllowNegativeReserve.CheckState = CInt(m_vAllowNegativeReserve)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAckTaskAllowedTime, vControlValue:=m_vExtClmHandlerAcknowledgedTaskAllowedTime)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPreReportAllowedTime, vControlValue:=m_vExtClmHandlerSupplyPreReportTaskAllowedTime)

            chkValidPolicyAtLossDate.CheckState = CInt(m_vValidPolicyVersionAtLossDate)

            chkClaimPaymentGross.CheckState = CInt(m_vIsGrossClaimPaymentAmount)

            '' If System Option '5240 - Claims Reserve are Gross' is checked
            Dim isClaimsReserveForGross As String = String.Empty

            m_lReturn = iPMFunc.GetSystemOption(systemOptionClaimsReserveAreGross, isClaimsReserveForGross, g_iSourceID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError("ValidateForm", "Failed to get System Option", gPMConstants.PMELogLevel.PMLogError)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If isClaimsReserveForGross = "1" Then
                chkClaimPaymentGross.CheckState = CheckState.Checked
                chkClaimPaymentGross.Enabled = False
            End If

            chkRecommender.CheckState = CInt(m_vIsRecommender)
            If (chkRecommender.CheckState = CheckState.Checked) Then
                lblAuthorisationThreshold.Visible = True
                txtAuthorisationThreshold.Visible = True
                txtAuthorisationThreshold.Text = CStr(m_vAuthorisationThreshold)
            Else
                lblAuthorisationThreshold.Visible = False
                txtAuthorisationThreshold.Visible = False
            End If

            If m_iMTA_dateAllowed > -1 Then
                For irow As Integer = 0 To cbodtAllowed.Items.Count
                    If VB6.GetItemData(cbodtAllowed, irow) = m_iMTA_dateAllowed Then
                        cbodtAllowed.SelectedIndex = irow
                        Exit For
                    End If
                Next
            Else : cbodtAllowed.SelectedIndex = -1
            End If
            If m_iMTA_Allocation > -1 Then
                For irow As Integer = 0 To cboallocation.Items.Count
                    If VB6.GetItemData(cboallocation, irow) = m_iMTA_Allocation Then
                        cboallocation.SelectedIndex = irow
                        Exit For
                    End If
                Next
            Else : cboallocation.SelectedIndex = -1
            End If

            If CDbl(m_vClaimTaskGroup) > 0 Then
                For iCounter As Integer = 0 To cboClaimTaskGroup.Items.Count - 1
                    If VB6.GetItemData(cboClaimTaskGroup, iCounter) = CInt(m_vClaimTaskGroup) Then
                        cboClaimTaskGroup.SelectedIndex = iCounter
                        Exit For
                    End If
                Next iCounter
            Else
                cboClaimTaskGroup.SelectedIndex = 0
            End If

            If CDbl(m_vClaimUserGroup) > 0 Then
                For iCounter As Integer = 0 To cboClaimUserGroup.Items.Count - 1
                    If VB6.GetItemData(cboClaimUserGroup, iCounter) = CInt(m_vClaimUserGroup) Then
                        cboClaimUserGroup.SelectedIndex = iCounter
                        Exit For
                    End If
                Next iCounter
            Else
                cboClaimUserGroup.SelectedIndex = 0
            End If

            If CDbl(m_vClaimsUDTA) > 0 Then
                For iCounter As Integer = 0 To cboUDT(0).Items.Count - 1
                    If VB6.GetItemData(cboUDT(0), iCounter) = CInt(m_vClaimsUDTA) Then
                        cboUDT(0).SelectedIndex = iCounter
                        Exit For
                    End If
                Next iCounter
            Else
                cboUDT(0).SelectedIndex = 0
            End If

            If CDbl(m_vClaimsUDTB) > 0 Then
                For iCounter As Integer = 0 To cboUDT(1).Items.Count - 1
                    If VB6.GetItemData(cboUDT(1), iCounter) = CInt(m_vClaimsUDTB) Then
                        cboUDT(1).SelectedIndex = iCounter
                        Exit For
                    End If
                Next iCounter
            Else
                cboUDT(1).SelectedIndex = 0
            End If

            If CDbl(m_vClaimsUDTC) > 0 Then
                For iCounter As Integer = 0 To cboUDT(2).Items.Count - 1
                    If VB6.GetItemData(cboUDT(2), iCounter) = CInt(m_vClaimsUDTC) Then
                        cboUDT(2).SelectedIndex = iCounter
                        Exit For
                    End If
                Next iCounter
            Else
                cboUDT(2).SelectedIndex = 0
            End If

            If CDbl(m_vClaimsUDTD) > 0 Then
                For iCounter As Integer = 0 To cboUDT(3).Items.Count - 1

                    If VB6.GetItemData(cboUDT(3), iCounter) = CInt(m_vClaimsUDTD) Then
                        cboUDT(3).SelectedIndex = iCounter
                        Exit For
                    End If
                Next iCounter
            Else
                cboUDT(3).SelectedIndex = 0
            End If

            If CDbl(m_vClaimsUDTE) > 0 Then
                For iCounter As Integer = 0 To cboUDT(4).Items.Count - 1

                    If VB6.GetItemData(cboUDT(4), iCounter) = CInt(m_vClaimsUDTE) Then
                        cboUDT(4).SelectedIndex = iCounter
                        Exit For
                    End If
                Next iCounter
            Else
                cboUDT(4).SelectedIndex = 0
            End If

            chkDuplicateClaim.CheckState = CInt(m_vIsDuplicateClaimCheckEnabled)
            chkAdvanceTaxScript.CheckState = CInt(m_vIsAdvancedTaxScriptEnabled)
            chkPaymentRefCheck.CheckState = CInt(m_vIsPaymentRefCheckEnabled)
            chkBackdatedMTA.CheckState = IIf(m_vAllowBackdatedMTAs, 1, 0)

            If chkBackdatedMTA.CheckState = CheckState.Checked Then
                chkBackdatedCan.CheckState = CheckState.Unchecked
                chkBackdatedCan.Enabled = False
            Else
                chkBackdatedCan.CheckState = IIf(m_vAllowBackdatedCan, 1, 0)
            End If

            If Not (Convert.IsDBNull(m_bMTCRatingRulesEnabled) Or IsNothing(m_bMTCRatingRulesEnabled)) Then
                Me.chkMTCRatingRules.CheckState = IIf(m_bMTCRatingRulesEnabled, CheckState.Checked, CheckState.Unchecked)
            Else
                Me.chkMTCRatingRules.CheckState = CheckState.Unchecked
            End If
            SetupClaimWorkflow(m_lProductID)

            If Convert.IsDBNull(m_vDefaultRenMonths) Or IsNothing(m_vDefaultRenMonths) Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtdefaultRenMth, vControlValue:="")
            Else
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtdefaultRenMth, vControlValue:=m_vDefaultRenMonths)
            End If

            chkPaymentCannotExceedReserve.CheckState = CInt(m_vPaymentCannotExceedReserve)
            chkRecoveryInstalmentsEnabled.CheckState = CInt(m_vRecoveryInstalmentsEnabled)

            If Not (Convert.IsDBNull(m_vCNDefaultPeriod) Or IsNothing(m_vCNDefaultPeriod)) Then

                txtCNDefaultPeriod.Text = CStr(m_vCNDefaultPeriod)
            End If

            If Not (Convert.IsDBNull(m_vCNMaxNo) Or IsNothing(m_vCNMaxNo)) Then

                txtCNMaxNo.Text = CStr(m_vCNMaxNo)
            End If

            If Not (Convert.IsDBNull(m_vCNDocTemplateId) Or IsNothing(m_vCNDocTemplateId)) Then
                txtCNDocTemplate.Text = m_vCNTemplateCode
            End If

            If CDbl(m_vOutOfsequenceMtaUserGroup) > 0 Then
                For iCounter As Integer = 0 To cboOSMTAUserGroup.Items.Count - 1

                    If VB6.GetItemData(cboOSMTAUserGroup, iCounter) = CInt(m_vOutOfsequenceMtaUserGroup) Then
                        cboOSMTAUserGroup.SelectedIndex = iCounter
                        Exit For
                    End If
                Next iCounter
            Else
                cboOSMTAUserGroup.SelectedIndex = 0
            End If

            If CDbl(m_vOutOfsequenceMtaTaskGroup) > 0 Then
                For iCounter As Integer = 0 To cboOSMTATaskGroup.Items.Count - 1

                    If VB6.GetItemData(cboOSMTATaskGroup, iCounter) = CInt(m_vOutOfsequenceMtaTaskGroup) Then
                        cboOSMTATaskGroup.SelectedIndex = iCounter
                        Exit For
                    End If
                Next iCounter
            Else
                cboOSMTATaskGroup.SelectedIndex = 0
            End If

            If Not (Convert.IsDBNull(m_bRoundOffToZero) Or IsNothing(m_bRoundOffToZero)) Then
                Me.chkRoundOffToZero.CheckState = IIf(m_bRoundOffToZero, CheckState.Checked, CheckState.Unchecked)
            Else
                Me.chkRoundOffToZero.CheckState = CheckState.Unchecked
            End If

            If Not (Convert.IsDBNull(m_vCheckMediaTypeStatusAtClaimPayment) Or IsNothing(m_vCheckMediaTypeStatusAtClaimPayment)) Then
                chkValidateMediaTypeStatusAtClaimPayment.CheckState = gPMFunctions.ToSafeInteger(m_vCheckMediaTypeStatusAtClaimPayment)
            Else
                chkValidateMediaTypeStatusAtClaimPayment.CheckState = CheckState.Unchecked
            End If

            If Not (Convert.IsDBNull(m_vCheckMediaTypeStatusAtPolicyRefund) Or IsNothing(m_vCheckMediaTypeStatusAtPolicyRefund)) Then
                chkValidateMediaTypeStatusAtPolicyRefund.CheckState = gPMFunctions.ToSafeInteger(m_vCheckMediaTypeStatusAtPolicyRefund)
            Else

                m_vCheckMediaTypeStatusAtPolicyRefund.Value = 0
            End If

            If CInt(m_vRIManualPremiumAdjustment) = 1 Then
                chkReinsuranceManualPremiumAdjustment.CheckState = CheckState.Checked
            Else
                chkReinsuranceManualPremiumAdjustment.CheckState = CheckState.Unchecked
            End If

            If m_vTMPAutoRenFAC = "1" Then
                chkTMPAutoRenFAC.CheckState = CheckState.Checked
            Else
                chkTMPAutoRenFAC.CheckState = CheckState.Unchecked
            End If

            If CInt(m_vIsBindRenewalWithoutInvitation) = 1 Then
                chkBindRenewalWOInvitation.CheckState = CheckState.Checked
            Else
                chkBindRenewalWOInvitation.CheckState = CheckState.Unchecked
            End If

            If m_bWrittenPolicyStatus = True Then
                chkWrittenPolicy.Checked = CheckState.Checked
            Else
                chkWrittenPolicy.Checked = CheckState.Unchecked
            End If

            If m_bWrittenPolicyStatus = True Then
                txtTaskManagerDays.Text = m_iTaskManagerDays
                Dim iCounter As Integer
                If m_iReminderUserGroup > 0 Then
                    For iCounter = 0 To cboReminderUserGroup.Items.Count - 1
                        If VB6.GetItemData(cboReminderUserGroup, iCounter) = m_iReminderUserGroup Then
                            cboReminderUserGroup.SelectedIndex = iCounter
                            Exit For
                        End If
                    Next iCounter
                Else
                    cboReminderUserGroup.SelectedIndex = 0
                End If

                If m_iReminderTaskGroup > 0 Then
                    For iCounter = 0 To cboReminderTaskGroup.Items.Count - 1
                        If VB6.GetItemData(cboReminderTaskGroup, iCounter) = m_iReminderTaskGroup Then
                            cboReminderTaskGroup.SelectedIndex = iCounter
                            Exit For
                        End If
                    Next iCounter
                Else
                    cboReminderTaskGroup.SelectedIndex = 0
                End If
            End If

            If m_vProductEnablePrePayment = "1" Then
                chkEnablePrePayment.CheckState = CheckState.Checked
            Else
                chkEnablePrePayment.CheckState = CheckState.Unchecked
            End If

            If m_oUnifiedrenewalDateIsReadOnly = "1" Then
                chkUnifiedRenewalDateIsReadOnly.Checked = True
            Else
                chkUnifiedRenewalDateIsReadOnly.Checked = False
            End If

            If m_sDefaultCoverToDateToLastDay = "1" Then
                chkDefaultCovertoDatetolastday.Checked = True
            Else
                chkDefaultCovertoDatetolastday.Checked = False
            End If

            If m_bRetainPolicyNumberonCopy = "1" Then
                chkRetainPolicyNumber.CheckState = CheckState.Checked
            Else
                chkRetainPolicyNumber.CheckState = CheckState.Unchecked
            End If

            If m_bUsePolicyInceptionDate Then
                rbPolicyInceptionDate.Checked = True
            Else
                rbEffectiveDate.Checked = True
            End If

            If m_bVoidTransaction Then
                chkVoidPolicyVersion.Checked = True
            Else
                chkVoidPolicyVersion.Checked = False
            End If

            If m_iIsQuoteVersioning = 1 Then
                chkQuoteVersioning.CheckState = CheckState.Checked
                ' Once Quote Versioning is enabled in DB, prevent it from being disabled
                chkQuoteVersioning.Enabled = False
            Else
                chkQuoteVersioning.CheckState = CheckState.Unchecked
                ' Allow enabling Quote Versioning if not yet enabled
                chkQuoteVersioning.Enabled = True
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDeleteQuoteAfter, vControlValue:=m_iDeleteQuoteAfter)

            ' Set the enabled state of txtDeleteQuoteAfter based on chkQuoteVersioning
            If chkQuoteVersioning.CheckState = CheckState.Checked Then
                txtDeleteQuoteAfter.Enabled = True
            Else
                txtDeleteQuoteAfter.Enabled = False
            End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ''' <summary>
    ''' InterfaceToBusiness
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function InterfaceToBusiness() As Integer

        Dim nResult As Integer
        Dim bTransOpen As Boolean
        Const kMethodName As String = "InterfaceToBusiness"
        Try
            Dim oParamArray(ACICPUpdMAXCount) As Object

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details from the interface to the data storage.
            m_lReturn = InterfaceToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oParamArray(ACIProduceSchedule) = m_iProduceSchedule

            oParamArray(ACIProduceCertificate) = m_iProduceCertificate

            oParamArray(ACIProduceDebitNote) = m_iProduceDebitNote

            oParamArray(ACICode) = m_sCode

            oParamArray(ACIProductEffectiveDate) = m_dtProductEffectiveDate

            oParamArray(ACIRenewalPeriod) = m_vRenewalPeriod

            oParamArray(ACIRIPointer) = m_vRIPointer

            oParamArray(ACIReportPointer) = m_vReportPointer

            oParamArray(ACIDescription) = m_vDescription

            oParamArray(ACISchemeAgencyRef) = m_vSchemeAgencyRef

            oParamArray(ACIBlockNo) = m_vBlockNo

            oParamArray(ACIProvClaimAutoNumberingID) = m_vProvClaimAutoNumberingID

            oParamArray(ACIQuoteAutoNumberingID) = m_vQuoteAutoNumberingID

            oParamArray(ACIPolicyAutoNumberingID) = m_vPolicyAutoNumberingID

            oParamArray(ACIFullClaimAutoNumberingID) = m_vFullClaimAutoNumberingID

            oParamArray(ACIIsMidnightRenewal) = m_vMidnightRenewal

            oParamArray(ACIIsAutoRenewable) = m_vAutoRenewal

            oParamArray(ACIIsTaxSuppressed) = m_vTaxSuppressed

            oParamArray(ACIIsShortPeriodRated) = m_vShortPeriodRated

            oParamArray(ACIAccumulation) = m_vAccumulation

            oParamArray(ACIIsDeleted) = gPMConstants.PMEReturnCode.PMFalse

            oParamArray(ACIClaimYearToCheck) = m_vClaimYearToCheck

            oParamArray(ACIMaxSingleClaimValue) = m_vMaxSingleClaimValue

            oParamArray(ACIMaxNumberOfClaim) = m_vMaxNumberOfClaim

            oParamArray(ACIMaxTotalClaimValue) = m_vMaxTotalClaimValue

            oParamArray(ACINBProrata) = m_vNBProrata

            oParamArray(ACIMTAProrata) = m_vMTAProrata

            oParamArray(ACIRoundPremium) = m_vRoundPremium

            oParamArray(ACIRoundingSection) = m_vRoundingSection

            oParamArray(ACIPolicyNumberAtQuote) = m_vPolicyNumberAtQuote

            oParamArray(ACIFollowUpTimeFrame) = m_vFollowUpTimeFrame

            oParamArray(ACIGracePeriod) = m_vGracePeriod

            oParamArray(ACIPreventCancelledAgents) = m_vPreventCancelledAgents

            oParamArray(ACIAllowPositiveValues) = m_vAllowPositiveCancellation

            oParamArray(ACIMediaTypeMandatory) = m_vMediaTypeMandatory

            oParamArray(ACIPolicyStyleID) = m_vPolicyStyleID

            oParamArray(ACIPolicyStyleMandatory) = m_vPolicyStyleMandatory

            oParamArray(ACICurrencyChange) = m_vCurrencyChange

            oParamArray(ACILossCurrencyChange) = m_vLossCurrencyChange

            oParamArray(ACIChangePolicyNumberAtRenewal) = m_vChangePolicyNumberAtRenewal

            oParamArray(ACIAllowStandardWordingEdit) = m_vAllowStandardWordingEdit

            oParamArray(ACIHideSummaryAtRenewalAcceptance) = m_vHideSummaryAtRenewalAcceptance

            oParamArray(ACIProductTrueMonthlyPolicy) = m_vProductIsTrueMonthlyPolicy

            oParamArray(ACIProductAnniversaryRenewalWeeks) = m_vProductAnniversaryRenewalWeeks

            oParamArray(ACIProductSuppressClaimTransactionsReserves) = m_vSuppressClaimTransactionReserve

            oParamArray(ACIProductSuppressClaimTransactionsPayments) = m_vSuppressClaimTransactionPayment

            oParamArray(ACIProductSuppressClaimTransactionsRecoveries) = m_vSuppressClaimTransactionRecovery

            oParamArray(ACIUnifiedRenewalDay) = m_vUnifiedRenewalDay

            oParamArray(ACILeadAllowConsolidateComm) = m_vLeadAllowConsolidateComm

            oParamArray(ACILeadMonthInCycle) = m_vLeadMonthInCycle

            oParamArray(ACILeadSuspenseAcct) = m_vLeadSuspenseAcct

            oParamArray(ACISubAllowConsolidateComm) = m_vSubAllowConsolidateComm

            oParamArray(ACISubMonthInCycle) = m_vSubMonthInCycle

            oParamArray(ACISubSuspenseAcct) = m_vSubSuspenseAcct

            oParamArray(ACICanMakeLiveInvoice) = m_iCanMakeLiveInvoice

            oParamArray(ACICanMakeLiveInstalments) = m_iCanMakeLiveInstalments

            oParamArray(ACICanMakeLivePaynow) = m_iCanMakeLivePaynow

            oParamArray(ACICanMakeBankGuarantee) = m_iCanMakeLiveBankGuarantee

            oParamArray(ACICanMakeCashDeposit) = m_iCanMakeLiveCashDeposit

            oParamArray(ACIIsRenewable) = m_iIsRenewable

            oParamArray(ACIIsRenewalSelectionEnabled) = m_iIsRenewalSelectionEnabled

            oParamArray(ACITrueMonthlyPolicyRenewalCommunication) = m_iTrueMonthlyPolicyRenewalCommunication

            oParamArray(ACIRenewalSelectionManReviewTemplateId) = m_vRenewalSelectionManReviewTemplateId

            oParamArray(ACIRenewalSelectionManReviewAttachmentTemplateId) = m_vRenewalSelectionManReviewAttachmentTemplateId

            oParamArray(ACIRenewalSelectionInviteTemplateId) = m_vRenewalSelectionInviteTemplateId

            oParamArray(ACIRenewalSelectionInviteAttachmentTemplateId) = m_vRenewalSelectionInviteAttachmentTemplateId

            oParamArray(ACIRenewalSelectionUpdateTemplateId) = m_vRenewalSelectionUpdateTemplateId

            oParamArray(ACIRenewalSelectionUpdateAttachmentTemplateId) = m_vRenewalSelectionUpdateAttachmentTemplateId

            oParamArray(ACIIsRenewalInviteEnabled) = m_iIsRenewalInviteEnabled

            oParamArray(ACIRenewalInviteManReviewTemplateId) = m_vRenewalInviteManReviewTemplateId

            oParamArray(ACIRenewalInviteManReviewAttachmentTemplateId) = m_vRenewalInviteManReviewAttachmentTemplateId

            oParamArray(ACIRenewalInviteInviteTemplateId) = m_vRenewalInviteInviteTemplateId

            oParamArray(ACIRenewalInviteInviteAttachmentTemplateId) = m_vRenewalInviteInviteAttachmentTemplateId

            oParamArray(ACIRenewalInviteUpdateTemplateId) = m_vRenewalInviteUpdateTemplateId

            oParamArray(ACIRenewalInviteUpdateAttachmentTemplateId) = m_vRenewalInviteUpdateAttachmentTemplateId

            oParamArray(ACIIsRenewalUpdateEnabled) = m_iIsRenewalUpdateEnabled

            oParamArray(ACIRenewalUpdateManReviewTemplateId) = m_vRenewalUpdateManReviewTemplateId

            oParamArray(ACIRenewalUpdateManReviewAttachmentTemplateId) = m_vRenewalUpdateManReviewAttachmentTemplateId

            oParamArray(ACIRenewalUpdateInviteTemplateId) = m_vRenewalUpdateInviteTemplateId

            oParamArray(ACIRenewalUpdateInviteAttachmentTemplateId) = m_vRenewalUpdateInviteAttachmentTemplateId

            oParamArray(ACIRenewalUpdateUpdateTemplateId) = m_vRenewalUpdateUpdateTemplateId

            oParamArray(ACIRenewalUpdateUpdateAttachmentTemplateId) = m_vRenewalUpdateUpdateAttachmentTemplateId

            oParamArray(ACIIsAgentRenewalSelectionEnabled) = m_iIsAgentRenewalSelectionEnabled

            oParamArray(ACIIsAgentRenewalInviteEnabled) = m_iIsAgentRenewalInviteEnabled

            oParamArray(ACIIsAgentRenewalUpdateEnabled) = m_iIsAgentRenewalUpdateEnabled

            oParamArray(ACIAgentRenewalManReviewTemplateId) = m_vAgentRenewalManReviewTemplateId

            oParamArray(ACIAgentRenewalManReviewReportId) = m_vAgentRenewalManReviewReportId

            oParamArray(ACIAgentRenewalInviteTemplateId) = m_vAgentRenewalInviteTemplateId

            oParamArray(ACIAgentRenewalInviteReportId) = m_vAgentRenewalInviteReportId

            oParamArray(ACIAgentRenewalUpdateTemplateId) = m_vAgentRenewalUpdateTemplateId

            oParamArray(ACIAgentRenewalUpdateReportId) = m_vAgentRenewalUpdateReportId

            oParamArray(ACICPUpdMultipleClaimPayments) = m_vMultipleClaimsPayments

            oParamArray(ACICPUpdMaxUnauthorisedClaimValue) = m_cMaxUnauthorisedClaimValue

            oParamArray(ACICPUpdMaxNoofUnauthorisedClaimPayments) = m_iMaxNoofUnauthorisedClaimPayments

            oParamArray(ACICPUpdRunAuthorisationScriptsforClaimPayments) = m_vRunAuthorisationScriptsforClaimsPayments

            oParamArray(ACICPUpdBankAccountId) = m_vBankAccountId

            oParamArray(ACICPUpdClaimValueForLargeLossAdvice) = m_vClaimValueForLargeLossAdvice

            oParamArray(ACICPUpdInclusionofCoInsurersOnClaims) = m_vInclusionofCoInsurersOnClaims

            oParamArray(ACICPUpdAllowNegativeReserve) = m_vAllowNegativeReserve

            oParamArray(ACICPUpdExtClmHandlerAcknowledgedTaskAllowedTime) = m_vExtClmHandlerAcknowledgedTaskAllowedTime

            oParamArray(ACICPUpdExtClmHandlerSupplyPreReportTaskAllowedTime) = m_vExtClmHandlerSupplyPreReportTaskAllowedTime

            oParamArray(ACICPUpdValidPolicyVersionAtLossDate) = m_vValidPolicyVersionAtLossDate

            oParamArray(ACICPUpdIsGrossClaimPaymentAmount) = m_vIsGrossClaimPaymentAmount

            oParamArray(ACICPUpdClaimTaskGroup) = m_vClaimTaskGroup

            oParamArray(ACICPUpdClaimUserGroup) = m_vClaimUserGroup

            oParamArray(ACICPUpdClaimsUDTA) = m_vClaimsUDTA

            oParamArray(ACICPUpdClaimsUDTB) = m_vClaimsUDTB

            oParamArray(ACICPUpdClaimsUDTC) = m_vClaimsUDTC

            oParamArray(ACICPUpdClaimsUDTD) = m_vClaimsUDTD

            oParamArray(ACICPUpdClaimsUDTE) = m_vClaimsUDTE

            oParamArray(ACICPUpdIsDuplicateClaimCheckEnabled) = m_vIsDuplicateClaimCheckEnabled

            oParamArray(ACICPUpdIsAdvancedTaxScriptEnabled) = m_vIsAdvancedTaxScriptEnabled

            oParamArray(ACICPUpdIsPaymentRefCheckEnabled) = m_vIsPaymentRefCheckEnabled

            oParamArray(ACICPUpdIsRecommender) = m_vIsRecommender

            oParamArray(ACICPUpdMTADateAllowed) = m_iMTA_dateAllowed

            oParamArray(ACICPUpdMTAAllocation) = m_iMTA_Allocation

            oParamArray(ACICPUpdDefaultRenewalMonths) = m_vDefaultRenMonths

            oParamArray(ACICPUpdPaymentCannotExceedReserve) = m_vPaymentCannotExceedReserve
            oParamArray(ACICPUpdRecoveryInstalmentsEnabled) = m_vRecoveryInstalmentsEnabled

            oParamArray(ACMTCRatingRulesEnabled) = IIf(m_bMTCRatingRulesEnabled, 1, 0)

            oParamArray(ACICPUpdAllowBackdatedMTAs) = IIf(m_vAllowBackdatedMTAs, 1, 0)

            oParamArray(ACICPUpdAllowBackdatedCan) = IIf(m_vAllowBackdatedCan, 1, 0)

            oParamArray(ACICPUpdCNNumberingId) = gPMFunctions.ZeroToNull(m_vCNCoverNoteNumberingID)

            If Convert.IsDBNull(oParamArray(ACICPUpdCNNumberingId)) Or IsNothing(oParamArray(ACICPUpdCNNumberingId)) Then

                oParamArray(ACICPUpdCNDefaultPeriod) = gPMFunctions.ZeroToNull(m_vCNDefaultPeriod)

                oParamArray(ACICPUpdCNDocTemplateID) = gPMFunctions.ZeroToNull(m_vCNDocTemplateId)

                oParamArray(ACICPUpdCNMaxNo) = gPMFunctions.ZeroToNull(m_vCNMaxNo)
            Else

                oParamArray(ACICPUpdCNDefaultPeriod) = m_vCNDefaultPeriod

                oParamArray(ACICPUpdCNDocTemplateID) = m_vCNDocTemplateId

                oParamArray(ACICPUpdCNMaxNo) = m_vCNMaxNo
            End If

            oParamArray(ACICPUpdOutOfsequenceMTAUserGroup) = m_vOutOfsequenceMtaUserGroup

            oParamArray(ACICPUpdOutOfsequenceMTATaskGroup) = m_vOutOfsequenceMtaTaskGroup

            oParamArray(ACICPUpdRoundOffToZero) = IIf(m_bRoundOffToZero, 1, 0)

            oParamArray(ACICPUpdCheckMediaTypeStatusAtClaimPayment) = m_vCheckMediaTypeStatusAtClaimPayment

            oParamArray(ACICPUpdCheckMediaTypeStatusAtPolicyRefund) = m_vCheckMediaTypeStatusAtPolicyRefund

            oParamArray(ACIPUpdWrittenPolicyStatus) = IIf(m_bWrittenPolicyStatus = True, 1, 0)
            oParamArray(ACIPUpdTaskManagerDays) = m_iTaskManagerDays
            oParamArray(ACIPUpdReminderUserGroup) = m_iReminderUserGroup
            oParamArray(ACIPUpdReminderTaskGroup) = m_iReminderTaskGroup

            oParamArray(ACICPUpdChangePolicyNumberAtRenewalAutomatically) = m_vChangePolicyNumberAtRenewalAutomatically

            oParamArray(ACICPUpdUseNBRenPaymentTermsAtSelection) = m_vUseNBRenPaymentTermsAtSelection

            oParamArray(ACICPUpdUsePriorTermSchemeAtRenewal) = m_vUsePriorTermSchemeAtRenewal

            oParamArray(ACICPUpdRIManualPremiumAdjustment) = m_vRIManualPremiumAdjustment

            oParamArray(ACICPUpdTMPAutoRenFac) = m_vTMPAutoRenFAC
            oParamArray(ACICPUpdMandatoryRiskTypeId) = m_vMandatoryRiskTypeId

            oParamArray(ACICPUpdBindRenewalWithoutInvitation) = m_vIsBindRenewalWithoutInvitation

            oParamArray(ACICPUpdEnablePrePayment) = m_vProductEnablePrePayment

            oParamArray(ACICPUpdDoNotDeleteRenewalQuoteOnMTA) = m_vDoNotDeleteRenewalQuoteOnMTA

            oParamArray(kICPUpdUnifiedRenewlDateIsReadOnly) = m_oUnifiedrenewalDateIsReadOnly
            oParamArray(kICPUpdDefaultCoverToDateToLastDay) = m_sDefaultCoverToDateToLastDay

            oParamArray(ACICPUpdIsReservesReadonly) = m_bIsReservesReadOnly
            oParamArray(ACICPUpdIsRecoveriesReadonly) = m_bIsRecoveriesReadOnly
            oParamArray(ACICPUpdIsPaymentsReadonly) = m_bIsPaymentsReadOnly

            oParamArray(ACICPUpdDisplayRerateForQuoteAndNB) = m_DisplayRerateForQuoteAndNB
            oParamArray(ACICPUpdDisplayRerateForCancellationsAndReinstatments) = m_DisplayRerateForCancellationsAndReinstatments
            oParamArray(ACICPUpdDisplayRerateForMTA) = m_DisplayRerateForMTA
            oParamArray(ACICPUpdDisplayRerateForRenewal) = m_DisplayReRateForRenewal

            oParamArray(ACICPUpdAutoRenewBackDatedMonthlyPolicy) = autoRenewBackDatedMonthlyPolicy
            oParamArray(ACICPUpdDoNotDeleteRenQuote) = IIf(m_vDeleteRenewalQuoteReRunOnMTA, 1, 0)
            oParamArray(ACICPUpdRetainPolicyNumberonCopy) = m_bRetainPolicyNumberonCopy
            oParamArray(ACICPUpdEditAnnivDate) = m_EditAnnivDate
            oParamArray(ACICPUpdDisableCoverStartDateonREN) = m_bDisableCoverStartDateOnREN
            oParamArray(ACICPUpdUsePolicyInceptionDate) = m_bUsePolicyInceptionDate
            oParamArray(ACICPUpdAuthorisationThreshold) = m_vAuthorisationThreshold
            oParamArray(ACICPUpdVoidTransaction) = m_bVoidTransaction
            oParamArray(ACICPUpdIsQuoteVersioning) = If(chkQuoteVersioning.CheckState = CheckState.Checked, 1, 0)
            oParamArray(ACICPUpdDeleteQuoteAfter) = m_oFormFields.UnformatControl(txtDeleteQuoteAfter)

            If String.IsNullOrEmpty(m_sUniqueId) Then
                m_sUniqueId = GetUniqueID()
            End If

            m_sScreenHierarchy = $"Product({txtCode.Text.Trim()})"
            m_lReturn = m_oBusiness.UpdateProduct(v_iTask:=m_iTask, r_lProductID:=m_lProductID, r_vParamArray:=oParamArray, r_vAllowedRiskTypeGroup:=m_vAllowedRiskTypeGroup, r_vAllowedCausation:=m_vAllowedCausation, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=m_sScreenHierarchy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
            Else
                m_lReturn = m_oBusiness.UpdateProductPaymentMethod(v_lProductID:=m_lProductID, v_sPaymentMethod:=m_sPaymentMethod)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            m_lReturn = UpdateClaimWorkflow(m_lProductID, m_sUniqueId, m_sScreenHierarchy)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = m_oBusiness.RollBackTrans
                bTransOpen = False
                RaiseError(kMethodName, "Update UpdateClaimWorkflow Failed")
            Else
                m_lReturn = m_oBusiness.CommitTrans
                bTransOpen = False
            End If


            m_lReturn = GenerateClaimsTransactionSuppressionEvent()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                uctPickListCausations.ForeignKeys.Item("product_id").Value = m_lProductID

                uctPickListMTAEvent.ForeignKeys.Item("product_id").Value = m_lProductID

                uctPickListClaimEvent.ForeignKeys.Item("product_id").Value = m_lProductID

                uctPickListBranches.ForeignKeys.Item("product_id").Value = m_lProductID

            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd OrElse m_iTask = gPMConstants.PMEComponentAction.PMEdit Then

                uctPickListCausations.ForeignKeys.Item("UserId").Value = Nothing
                uctPickListCausations.ForeignKeys.Item("UniqueId").Value = m_sUniqueId
                uctPickListCausations.ForeignKeys.Item("ScreenHierarchy").Value = m_sScreenHierarchy

                uctPickListMTAEvent.ForeignKeys.Item("UserId").Value = Nothing
                uctPickListMTAEvent.ForeignKeys.Item("UniqueId").Value = m_sUniqueId
                uctPickListMTAEvent.ForeignKeys.Item("ScreenHierarchy").Value = m_sScreenHierarchy

                uctPickListClaimEvent.ForeignKeys.Item("UserId").Value = Nothing
                uctPickListClaimEvent.ForeignKeys.Item("UniqueId").Value = m_sUniqueId
                uctPickListClaimEvent.ForeignKeys.Item("ScreenHierarchy").Value = m_sScreenHierarchy

                uctPickListBranches.ForeignKeys.Item("UserId").Value = Nothing
                uctPickListBranches.ForeignKeys.Item("UniqueId").Value = m_sUniqueId
                uctPickListBranches.ForeignKeys.Item("ScreenHierarchy").Value = m_sScreenHierarchy
            End If

            uctPickListCausations.Save()
            uctPickListMTAEvent.Save()
            uctPickListClaimEvent.Save()
            uctPickListBranches.Save()

            'Renewal Printing Docs
            With uctDocumentLink1
                .FuntionalArea = gPMConstants.PMDocLinkPolicy
                .ProductID = m_lProductID
                .UniqueId = m_sUniqueId
                .ScreenHierarchy = m_sScreenHierarchy

                m_lReturn = .Initialise()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddDocLinks()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                End If
            End With
            'Docs for Open/Maintain Claim
            With DocumentLinkClaim
                .FuntionalArea = gPMConstants.PMDocLinkOpenMaintainClaims
                .ProductID = m_lProductID
                .UniqueId = m_sUniqueId
                .ScreenHierarchy = m_sScreenHierarchy

                m_lReturn = .Initialise()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .AddDocLinks()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            'Docs for Payment of Claim
            With DocumentLinkClaimPayment
                .FuntionalArea = gPMConstants.PMDocLinkPayClaims
                .ProductID = m_lProductID
                .UniqueId = m_sUniqueId
                .ScreenHierarchy = m_sScreenHierarchy

                m_lReturn = .Initialise()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .AddDocLinks()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                End If
            End With


            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function
    ''' <summary>
    ''' InterfaceToData
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function InterfaceToData() As Integer

        Dim nResult As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            m_sCode = CStr(m_oFormFields.UnformatControl(ctlControl:=txtCode))

            m_dtProductEffectiveDate = CDate(m_oFormFields.UnformatControl(ctlControl:=txtEffectiveDate))

            If txtRenewalPeriod.Text.Trim() = "" Then
                m_vRenewalPeriod = DBNull.Value
            Else
                m_vRenewalPeriod = m_oFormFields.UnformatControl(ctlControl:=txtRenewalPeriod)
            End If

            If txtDescription.Text.Trim() = "" Then
                m_vDescription = DBNull.Value
            Else
                m_vDescription = m_oFormFields.UnformatControl(ctlControl:=txtDescription)
            End If

            If txtSchemeAgencyRef.Text.Trim() = "" Then
                m_vSchemeAgencyRef = DBNull.Value
            Else
                m_vSchemeAgencyRef = m_oFormFields.UnformatControl(ctlControl:=txtSchemeAgencyRef)
            End If

            If txtBlockNo.Text.Trim() = "" Then
                m_vBlockNo = DBNull.Value
            Else
                m_vBlockNo = m_oFormFields.UnformatControl(ctlControl:=txtBlockNo)
            End If

            m_vProvClaimAutoNumberingID = VB6.GetItemData(Me.cboProvClaimAutoNumberingID, Me.cboProvClaimAutoNumberingID.SelectedIndex)

            m_vQuoteAutoNumberingID = VB6.GetItemData(Me.cboQuoteAutoNumberingID, Me.cboQuoteAutoNumberingID.SelectedIndex)

            m_vPolicyAutoNumberingID = VB6.GetItemData(Me.cboPolicyAutoNumberingID, Me.cboPolicyAutoNumberingID.SelectedIndex)

            m_vFullClaimAutoNumberingID = VB6.GetItemData(Me.cboFullClaimAutoNumberingID, Me.cboFullClaimAutoNumberingID.SelectedIndex)

            m_vCNCoverNoteNumberingID = VB6.GetItemData(Me.cboCNAutoNumberingID, Me.cboCNAutoNumberingID.SelectedIndex)

            m_vMidnightRenewal = Me.chkMidNightRenewal.CheckState

            m_vAutoRenewal = Me.chkAutoRenewal.CheckState

            m_vTaxSuppressed = Me.chkTaxSuppressed.CheckState

            m_vShortPeriodRated = Me.chkShortPeriodRated.CheckState

            m_vAccumulation = Me.chkAccumulation.CheckState

            m_vCurrencyChange = Me.chkCurrencyChange.CheckState

            m_vLossCurrencyChange = Me.chkLossCurrencyChange.CheckState

            m_vNBProrata = Me.chkNBProRata.CheckState

            m_vMTAProrata = Me.chkMTAProRata.CheckState

            m_vRoundPremium = Me.chkRoundPremium.CheckState

            m_vRoundingSection = Me.cboRoundingSection.ItemId

            m_vPolicyNumberAtQuote = chkPolicyNumberAtQuote.CheckState

            m_vUseNBRenPaymentTermsAtSelection = chkUseNBRenPaymentTermsAtSelection.CheckState

            m_vUsePriorTermSchemeAtRenewal = chkUsePriorSchemeAtRenewal.CheckState

            m_bIsReservesReadOnly = chkIsReservesReadonly.Checked
            m_bIsRecoveriesReadOnly = chkIsRecoveriesReadonly.Checked
            m_bIsPaymentsReadOnly = chkIsPaymentsReadonly.Checked

            m_DisplayRerateForQuoteAndNB = chkDisplayRerateForQuoteAndNB.Checked
            m_DisplayRerateForCancellationsAndReinstatments = chkDisplayRerateForCancellationsAndReinstatments.Checked
            m_DisplayRerateForMTA = chkDisplayRerateForMTA.Checked
            m_DisplayReRateForRenewal = chkDisplayRerateForRenewal.Checked

            autoRenewBackDatedMonthlyPolicy = chkAutoRenBDMonthlyPol.Checked
            m_vAllowStandardWordingEdit = chkAllowStandardWordingEdit.CheckState

            m_vDoNotDeleteRenewalQuoteOnMTA = chkDoNotDeleteRenewalQuoteOnMTA.CheckState
            m_vDeleteRenewalQuoteReRunOnMTA = chkDeleteRenewalQuoteReRunOnMTA.CheckState
            m_EditAnnivDate = chkEditAnnivDate.Checked
            m_bDisableCoverStartDateOnREN = chkDisableCoverStartDateonREN.Checked
            If txtRIPointer.Text.Trim() = "" Then
                m_vRIPointer = DBNull.Value
            Else
                m_vRIPointer = m_oFormFields.UnformatControl(ctlControl:=txtRIPointer)
            End If

            If txtReportPointer.Text.Trim() = "" Then
                m_vReportPointer = DBNull.Value
            Else
                m_vReportPointer = m_oFormFields.UnformatControl(ctlControl:=txtReportPointer)
            End If

            If cboRiskTypeGroup.ItemId = 0 Then
                m_vAllowedRiskTypeGroup = VB6.CopyArray(Nothing)
            Else
                ReDim m_vAllowedRiskTypeGroup(0, 0)
                m_vAllowedRiskTypeGroup(0, 0) = cboRiskTypeGroup.ItemId
            End If

            If txtClaimYear.Text.Trim() = "" Then

                m_vClaimYearToCheck = DBNull.Value
            Else

                m_vClaimYearToCheck = m_oFormFields.UnformatControl(ctlControl:=txtClaimYear)
            End If

            If txtSingleClaimValue.Text.Trim() = "" Then
                m_vMaxSingleClaimValue = DBNull.Value
            Else
                m_vMaxSingleClaimValue = m_oFormFields.UnformatControl(ctlControl:=txtSingleClaimValue)
            End If

            If txtAllowedClaims.Text.Trim() = "" Then
                m_vMaxNumberOfClaim = DBNull.Value
            Else
                m_vMaxNumberOfClaim = m_oFormFields.UnformatControl(ctlControl:=txtAllowedClaims)
            End If

            If txtTotalClaimsValue.Text.Trim() = "" Then
                m_vMaxTotalClaimValue = DBNull.Value
            Else
                m_vMaxTotalClaimValue = m_oFormFields.UnformatControl(ctlControl:=txtTotalClaimsValue)
            End If

            If txtGracePeriod.Text.Trim() = "" Then
                m_vGracePeriod = DBNull.Value
            Else
                m_vGracePeriod = m_oFormFields.UnformatControl(ctlControl:=txtGracePeriod)
            End If
            m_vAllowBackdatedCan = gPMFunctions.ToSafeInteger(chkBackdatedCan.CheckState)

            If chkCheckAgent.CheckState = CheckState.Checked Then
                m_vPreventCancelledAgents = "1"
            Else
                m_vPreventCancelledAgents = "0"
            End If

            m_vAllowPositiveCancellation = cboPosValues.SelectedIndex

            If chkMediaTypeMandatory.CheckState = CheckState.Checked Then
                m_vMediaTypeMandatory = "1"
            Else
                m_vMediaTypeMandatory = "0"
            End If

            m_vPolicyStyleID = cboPolicyStyle.ItemId

            m_vPolicyStyleMandatory = chkPolicyStyleMandatory.CheckState = CheckState.Checked

            m_vChangePolicyNumberAtRenewal = chkChangePolicyNumberAtRenewal.CheckState

            m_vChangePolicyNumberAtRenewalAutomatically = chkChangePolicyNumberAtRenewalAutomatically.CheckState

            m_vHideSummaryAtRenewalAcceptance = chkHideSummaryAtRenewalAcceptance.CheckState

            If chkTrueMonthlyPolicy.CheckState = CheckState.Checked Then

                m_vProductIsTrueMonthlyPolicy = 1

                m_vProductAnniversaryRenewalWeeks = gPMFunctions.ToSafeLong(txtAnniversaryRenewalWeeks.Text, 0)

                m_vUnifiedRenewalDay = gPMFunctions.ToSafeLong(txtUnifiedRenewalDay.Text.Trim())

                m_vLeadAllowConsolidateComm = chkAllowConsolidateCommissionLA.CheckState

                m_vLeadMonthInCycle = cboMonthInCycleLA.Text

                m_vLeadSuspenseAcct = actSuspenseAcc.Text

                m_vSubAllowConsolidateComm = chkAllowConsolidateCommissionSA.CheckState

                m_vSubMonthInCycle = cboMonthInCycleSA.Text

                m_vSubSuspenseAcct = actSuspenseAcc1.Text
            Else

                m_vProductIsTrueMonthlyPolicy = 0

                m_vProductAnniversaryRenewalWeeks = DBNull.Value
            End If

            If chkReserve.CheckState = CheckState.Checked Then

                m_vSuppressClaimTransactionReserve = "1"
            Else
                m_vSuppressClaimTransactionReserve = DBNull.Value
            End If

            If chkPayments.CheckState = CheckState.Checked Then

                m_vSuppressClaimTransactionPayment = "1"
            Else
                m_vSuppressClaimTransactionPayment = DBNull.Value
            End If

            If chkRecoveries.CheckState = CheckState.Checked Then

                m_vSuppressClaimTransactionRecovery = "1"
            Else
                m_vSuppressClaimTransactionRecovery = DBNull.Value
            End If
            'Float Balance and Pre-Payment

            If chkInvoice.CheckState = CheckState.Checked Then
                m_iCanMakeLiveInvoice = 1
            Else
                m_iCanMakeLiveInvoice = 0
            End If

            If chkInstalments.CheckState = CheckState.Checked Then
                m_iCanMakeLiveInstalments = 1
            Else
                m_iCanMakeLiveInstalments = 0
            End If

            If chkPayNow.CheckState = CheckState.Checked Then
                m_iCanMakeLivePaynow = 1
            Else
                m_iCanMakeLivePaynow = 0
            End If

            If chkBankGuarantee.CheckState = CheckState.Checked Then
                m_iCanMakeLiveBankGuarantee = 1
            Else
                m_iCanMakeLiveBankGuarantee = 0
            End If

            If chkCashDeposit.CheckState = CheckState.Checked Then
                m_iCanMakeLiveCashDeposit = 1
            Else
                m_iCanMakeLiveCashDeposit = 0
            End If

            'Produce Documents values
            If chkProduceSchedule.CheckState = CheckState.Checked Then
                m_iProduceSchedule = 1
            Else
                m_iProduceSchedule = 0
            End If

            If chkProduceCertificate.CheckState = CheckState.Checked Then
                m_iProduceCertificate = 1
            Else
                m_iProduceCertificate = 0
            End If

            If chkProduceDebitNote.CheckState = CheckState.Checked Then
                m_iProduceDebitNote = 1
            Else
                m_iProduceDebitNote = 0
            End If
            ' {* USER DEFINED CODE (End) *}

            If chkTrueMonthlyPolicy.CheckState = CheckState.Checked Then
                If OptInvoice.Checked Then
                    m_sPaymentMethod = "Invoice"
                ElseIf OptInstalments.Checked Then
                    m_sPaymentMethod = "Instalments"
                End If
            Else
                m_sPaymentMethod = ""
            End If

            m_iIsRenewable = chkRenewable.CheckState
            m_iIsRenewalSelectionEnabled = chkEnabledRenSelection.CheckState

            If optRenewalProcessRun.Checked Then
                m_iTrueMonthlyPolicyRenewalCommunication = 1
            ElseIf optAnniversaryDate.Checked Then
                m_iTrueMonthlyPolicyRenewalCommunication = 2
            Else
                m_iTrueMonthlyPolicyRenewalCommunication = 0
            End If

            m_vRenewalSelectionManReviewTemplateId = gPMFunctions.ZeroToNull(Convert.ToString(txtEmailComm(0).Tag))

            m_vRenewalSelectionManReviewAttachmentTemplateId = gPMFunctions.ZeroToNull(Convert.ToString(txtEmailComm(1).Tag))

            m_vRenewalSelectionInviteTemplateId = gPMFunctions.ZeroToNull(Convert.ToString(txtEmailComm(2).Tag))

            m_vRenewalSelectionInviteAttachmentTemplateId = gPMFunctions.ZeroToNull(Convert.ToString(txtEmailComm(3).Tag))

            m_vRenewalSelectionUpdateTemplateId = gPMFunctions.ZeroToNull(Convert.ToString(txtEmailComm(4).Tag))

            m_vRenewalSelectionUpdateAttachmentTemplateId = gPMFunctions.ZeroToNull(Convert.ToString(txtEmailComm(5).Tag))
            m_iIsRenewalInviteEnabled = chkEnabledRenInvite.CheckState

            m_vRenewalInviteManReviewTemplateId = gPMFunctions.ZeroToNull(Convert.ToString(txtEmailComm(6).Tag))

            m_vRenewalInviteManReviewAttachmentTemplateId = gPMFunctions.ZeroToNull(Convert.ToString(txtEmailComm(7).Tag))

            m_vRenewalInviteInviteTemplateId = gPMFunctions.ZeroToNull(Convert.ToString(txtEmailComm(8).Tag))

            m_vRenewalInviteInviteAttachmentTemplateId = gPMFunctions.ZeroToNull(Convert.ToString(txtEmailComm(9).Tag))

            m_vRenewalInviteUpdateTemplateId = gPMFunctions.ZeroToNull(Convert.ToString(txtEmailComm(10).Tag))

            m_vRenewalInviteUpdateAttachmentTemplateId = gPMFunctions.ZeroToNull(Convert.ToString(txtEmailComm(11).Tag))
            m_iIsRenewalUpdateEnabled = chkEnabledRenUpdate.CheckState

            m_vRenewalUpdateManReviewTemplateId = gPMFunctions.ZeroToNull(Convert.ToString(txtEmailComm(12).Tag))

            m_vRenewalUpdateManReviewAttachmentTemplateId = gPMFunctions.ZeroToNull(Convert.ToString(txtEmailComm(13).Tag))

            m_vRenewalUpdateInviteTemplateId = gPMFunctions.ZeroToNull(Convert.ToString(txtEmailComm(14).Tag))

            m_vRenewalUpdateInviteAttachmentTemplateId = gPMFunctions.ZeroToNull(Convert.ToString(txtEmailComm(15).Tag))

            m_vRenewalUpdateUpdateTemplateId = gPMFunctions.ZeroToNull(Convert.ToString(txtEmailComm(16).Tag))

            m_vRenewalUpdateUpdateAttachmentTemplateId = gPMFunctions.ZeroToNull(Convert.ToString(txtEmailComm(17).Tag))
            m_iIsAgentRenewalSelectionEnabled = chkAgentRenSelection.CheckState
            m_iIsAgentRenewalInviteEnabled = chkAgentRenInvite.CheckState
            m_iIsAgentRenewalUpdateEnabled = chkAgentRenUpdate.CheckState

            m_vAgentRenewalManReviewTemplateId = gPMFunctions.ZeroToNull(Convert.ToString(txtEmailComm(18).Tag))
            m_vAgentRenewalManReviewReportId = gPMFunctions.ZeroToNull(m_vAgentRenewalManReviewReportId)

            m_vAgentRenewalInviteTemplateId = gPMFunctions.ZeroToNull(Convert.ToString(txtEmailComm(19).Tag))
            m_vAgentRenewalInviteReportId = gPMFunctions.ZeroToNull(m_vAgentRenewalInviteReportId)

            m_vAgentRenewalUpdateTemplateId = gPMFunctions.ZeroToNull(Convert.ToString(txtEmailComm(20).Tag))

            m_vAgentRenewalUpdateReportId = gPMFunctions.ZeroToNull(m_vAgentRenewalUpdateReportId)

            m_vMultipleClaimsPayments = CStr(chkMultiplePayments.CheckState)
            m_iMaxNoofUnauthorisedClaimPayments = gPMFunctions.ToSafeInteger(txtMaxNoofUnauthorisedClaimPayments.Text)
            m_cMaxUnauthorisedClaimValue = gPMFunctions.ToSafeCurrency(txtMaxUnauthorisedClaimsValue.Text)

            m_vRunAuthorisationScriptsforClaimsPayments = chkRunAuthorisationScriptsClaimPayments.CheckState

            m_vBankAccountId = gPMFunctions.ToSafeInteger(cboBankAccount.ItemData(cboBankAccount.ListIndex))
            m_vClaimValueForLargeLossAdvice = gPMFunctions.ToSafeDouble(txtLargeLossAdviceValue.Text)

            m_vInclusionofCoInsurersOnClaims = gPMFunctions.ToSafeInteger(cboCoinsurerInclusion.SelectedIndex)

            m_vAllowNegativeReserve = gPMFunctions.ToSafeInteger(chkAllowNegativeReserve.CheckState)

            m_vExtClmHandlerAcknowledgedTaskAllowedTime = gPMFunctions.ToSafeInteger(txtAckTaskAllowedTime.Text)

            m_vExtClmHandlerSupplyPreReportTaskAllowedTime = gPMFunctions.ToSafeInteger(txtPreReportAllowedTime.Text)

            m_vValidPolicyVersionAtLossDate = gPMFunctions.ToSafeInteger(chkValidPolicyAtLossDate.CheckState)

            m_vIsGrossClaimPaymentAmount = gPMFunctions.ToSafeInteger(chkClaimPaymentGross.CheckState)

            m_vClaimTaskGroup = gPMFunctions.ToSafeInteger(VB6.GetItemData(cboClaimTaskGroup, cboClaimTaskGroup.SelectedIndex))

            m_vClaimUserGroup = gPMFunctions.ToSafeInteger(VB6.GetItemData(cboClaimUserGroup, cboClaimUserGroup.SelectedIndex))

            m_vClaimsUDTA = gPMFunctions.ToSafeInteger(VB6.GetItemData(cboUDT(0), cboUDT(0).SelectedIndex))

            m_vClaimsUDTB = gPMFunctions.ToSafeInteger(VB6.GetItemData(cboUDT(1), cboUDT(1).SelectedIndex))

            m_vClaimsUDTC = gPMFunctions.ToSafeInteger(VB6.GetItemData(cboUDT(2), cboUDT(2).SelectedIndex))

            m_vClaimsUDTD = gPMFunctions.ToSafeInteger(VB6.GetItemData(cboUDT(3), cboUDT(3).SelectedIndex))

            m_vClaimsUDTE = gPMFunctions.ToSafeInteger(VB6.GetItemData(cboUDT(4), cboUDT(4).SelectedIndex))

            m_vIsDuplicateClaimCheckEnabled = gPMFunctions.ToSafeInteger(chkDuplicateClaim.CheckState)

            m_vIsAdvancedTaxScriptEnabled = gPMFunctions.ToSafeInteger(chkAdvanceTaxScript.CheckState)

            m_vIsPaymentRefCheckEnabled = gPMFunctions.ToSafeInteger(chkPaymentRefCheck.CheckState)

            m_vIsRecommender = gPMFunctions.ToSafeInteger(chkRecommender.CheckState)
            m_vAuthorisationThreshold = txtAuthorisationThreshold.Text
            m_iMTA_dateAllowed = VB6.GetItemData(cbodtAllowed, cbodtAllowed.SelectedIndex)
            m_iMTA_Allocation = VB6.GetItemData(cboallocation, cboallocation.SelectedIndex)
            If txtdefaultRenMth.Text.Trim() = "" Then
                m_vDefaultRenMonths = DBNull.Value
            Else
                m_vDefaultRenMonths = m_oFormFields.UnformatControl(ctlControl:=txtdefaultRenMth)
            End If

            m_vPaymentCannotExceedReserve = gPMFunctions.ToSafeInteger(chkPaymentCannotExceedReserve.CheckState)
            m_vRecoveryInstalmentsEnabled = gPMFunctions.ToSafeInteger(chkRecoveryInstalmentsEnabled.CheckState)

            m_bMTCRatingRulesEnabled = chkMTCRatingRules.CheckState = CheckState.Checked

            m_vAllowBackdatedMTAs = gPMFunctions.ToSafeInteger(chkBackdatedMTA.CheckState)

            'Start (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section(4.3.3.3)

            m_vCNDefaultPeriod = txtCNDefaultPeriod.Text

            m_vCNMaxNo = txtCNMaxNo.Text

            m_vOutOfsequenceMtaUserGroup = gPMFunctions.ToSafeInteger(VB6.GetItemData(cboOSMTAUserGroup, cboOSMTAUserGroup.SelectedIndex))

            m_vOutOfsequenceMtaTaskGroup = gPMFunctions.ToSafeInteger(VB6.GetItemData(cboOSMTATaskGroup, cboOSMTATaskGroup.SelectedIndex))

            m_bRoundOffToZero = chkRoundOffToZero.CheckState = CheckState.Checked

            m_vCheckMediaTypeStatusAtClaimPayment = chkValidateMediaTypeStatusAtClaimPayment.CheckState

            m_vCheckMediaTypeStatusAtPolicyRefund = chkValidateMediaTypeStatusAtPolicyRefund.CheckState

            If chkReinsuranceManualPremiumAdjustment.CheckState = CheckState.Checked Then
                m_vRIManualPremiumAdjustment = 1
            Else
                m_vRIManualPremiumAdjustment = 0
            End If

            If chkTMPAutoRenFAC.CheckState = CheckState.Checked Then
                m_vTMPAutoRenFAC = 1
            Else
                m_vTMPAutoRenFAC = 0
            End If

            m_bWrittenPolicyStatus = ToSafeBoolean(chkWrittenPolicy.Checked)
            If cboReminderTaskGroup.SelectedIndex = -1 Or cboReminderTaskGroup.SelectedIndex = 0 Then
                m_iReminderUserGroup = Nothing
            Else
                m_iReminderUserGroup = gPMFunctions.ToSafeInteger(VB6.GetItemData(cboReminderUserGroup, cboReminderUserGroup.SelectedIndex))
            End If
            If cboReminderTaskGroup.SelectedIndex = -1 Or cboReminderTaskGroup.SelectedIndex = 0 Then
                m_iReminderTaskGroup = Nothing
            Else
                m_iReminderTaskGroup = gPMFunctions.ToSafeInteger(VB6.GetItemData(cboReminderTaskGroup, cboReminderTaskGroup.SelectedIndex))
            End If
            m_iTaskManagerDays = ToSafeInteger(txtTaskManagerDays.Text)

            m_vMandatoryRiskTypeId = Me.cboApplyMandatoryRisk.ItemData(Me.cboApplyMandatoryRisk.ListIndex)

            If chkBindRenewalWOInvitation.CheckState = CheckState.Checked Then
                m_vIsBindRenewalWithoutInvitation = 1
            Else
                m_vIsBindRenewalWithoutInvitation = 0
            End If

            If chkEnablePrePayment.CheckState = CheckState.Checked Then
                m_vProductEnablePrePayment = 1
            Else
                m_vProductEnablePrePayment = 0
            End If

            If chkUnifiedRenewalDateIsReadOnly.Checked Then
                m_oUnifiedrenewalDateIsReadOnly = 1
            Else
                m_oUnifiedrenewalDateIsReadOnly = 0
            End If

            If chkDefaultCovertoDatetolastday.Checked Then
                m_sDefaultCoverToDateToLastDay = "1"
            Else
                m_sDefaultCoverToDateToLastDay = "0"
            End If

            If chkRetainPolicyNumber.CheckState = CheckState.Checked Then
                m_bRetainPolicyNumberonCopy = 1
            Else
                m_bRetainPolicyNumberonCopy = 0
            End If

            m_bUsePolicyInceptionDate = rbPolicyInceptionDate.Checked
            m_bVoidTransaction = chkVoidPolicyVersion.Checked
            m_iIsQuoteVersioning = If(chkQuoteVersioning.CheckState = CheckState.Checked, 1, 0)

            If txtDeleteQuoteAfter.Text.Trim() = "" Then
                m_iDeleteQuoteAfter = 0
            Else
                m_iDeleteQuoteAfter = gPMFunctions.ToSafeInteger(m_oFormFields.UnformatControl(txtDeleteQuoteAfter))
            End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    Private Sub chkBackdatedMTA_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkBackdatedMTA.CheckStateChanged
        If chkBackdatedMTA.CheckState = CheckState.Checked Then
            chkBackdatedCan.CheckState = CheckState.Unchecked
            chkBackdatedCan.Enabled = False
        Else
            chkBackdatedCan.Enabled = True
        End If
    End Sub

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            'default to first tab
            SSTabHelper.SetSelectedIndex(Me.tabMainTab, 0)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Retrieve the product option to find on which type of
            ' RI processing we need to work with
            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, v_vBranch:=g_iSourceID, r_vUnderwriting:=m_vIsRI2007)


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            'default values when adding
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                OptInvoice.Checked = True

                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtEffectiveDate, vControlValue:=DateTime.Today)

                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtRenewalPeriod, vControlValue:=0)

                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtRIPointer, vControlValue:=0)


                ' PW311002
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtGracePeriod, vControlValue:=0)


                'Make Live Option Defaults to be Checked
                chkInstalments.CheckState = CheckState.Checked
                chkInvoice.CheckState = CheckState.Checked
                chkPayNow.CheckState = CheckState.Checked
                chkBankGuarantee.CheckState = CheckState.Checked ' Gaurav
                chkCashDeposit.CheckState = CheckState.Checked 'Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling

                m_lReturn = SetRenewalOnlineOptions()

                'By default switch off claim payment process in open & maintain claim
                chkOpenClaimWorkflow(11).CheckState = CheckState.Unchecked
                chkMaintainClaimWorkflow(12).CheckState = CheckState.Unchecked


                chkOpenClaimWorkflow(10).CheckState = CheckState.Checked
                chkOpenClaimWorkflow(22).CheckState = CheckState.Checked
                chkMaintainClaimWorkflow(11).CheckState = CheckState.Checked
                chkPaymentClaimWorkflow(13).CheckState = CheckState.Checked


                'Produce Document Option Defaults to be Checked
                chkProduceCertificate.CheckState = CheckState.Checked
                chkProduceDebitNote.CheckState = CheckState.Checked
                chkProduceSchedule.CheckState = CheckState.Checked
                chkEnablePrePayment.CheckState = CheckState.Checked
                chkQuoteVersioning.CheckState = CheckState.Unchecked
                chkQuoteVersioning.Enabled = True
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDeleteQuoteAfter, vControlValue:=0)
                txtDeleteQuoteAfter.Enabled = False
            End If

            'Getlookup values
            m_lReturn = GetLookUpDetails(v_sTableName:="Report", r_cboControl:=cboAgentReviewAttachment)
            m_lReturn = GetLookUpDetails(v_sTableName:="Report", r_cboControl:=cboAgentInviteAttachment)
            m_lReturn = GetLookUpDetails(v_sTableName:="Report", r_cboControl:=cboAgentUpdateAttachment)

            'disable the short period rate button by default
            Me.cmdSPR.Enabled = False

            'get quote auto numbering
            If GetComboDetails(1, Me.cboQuoteAutoNumberingID) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Me.cboQuoteAutoNumberingID.SelectedIndex = 0

            'get policy auto numbering
            If GetComboDetails(2, Me.cboPolicyAutoNumberingID) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Me.cboPolicyAutoNumberingID.SelectedIndex = 0

            'get provisional claim auto numbering
            If GetComboDetails(3, Me.cboProvClaimAutoNumberingID) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Me.cboProvClaimAutoNumberingID.SelectedIndex = 0

            'get full claim auto numbering
            If GetComboDetails(4, Me.cboFullClaimAutoNumberingID) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Me.cboFullClaimAutoNumberingID.SelectedIndex = 0

            'Start (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section(4.3.2)

            'get full Cover Note auto numbering
            If GetComboDetails(8, Me.cboCNAutoNumberingID) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Me.cboCNAutoNumberingID.SelectedIndex = 0

            'End (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section(4.3.2)

            cmdRIModel.Enabled = m_iTask = gPMConstants.PMEComponentAction.PMEdit



            ' Alix - 13/02/2004
            Dim cboPosValues_NewIndex As Integer = -1


            cboPosValues_NewIndex = cboPosValues.Items.Add("Allow")
            VB6.SetItemData(cboPosValues, cboPosValues_NewIndex, gPMConstants.PM_ALLOW)
            cboPosValues_NewIndex = cboPosValues.Items.Add("Deny")
            VB6.SetItemData(cboPosValues, cboPosValues_NewIndex, gPMConstants.PM_DENY)
            cboPosValues_NewIndex = cboPosValues.Items.Add("Prompt")
            VB6.SetItemData(cboPosValues, cboPosValues_NewIndex, gPMConstants.PM_PROMPT)
            cboPosValues.SelectedIndex = gPMConstants.PM_ALLOW
            ' /Alix

            ' setup initial values for true monthly policy
            SetupTrueMonthlyPolicy()

            ' setup initial values for claims transaction suppression
            SetupClaimTransactionSuppression()

            uctPickListCausations.AvailableCaption = "Available"
            uctPickListMTAEvent.AvailableCaption = "Available"
            uctPickListClaimEvent.AvailableCaption = "Available"
            If chkRunAuthorisationScriptsClaimPayments.CheckState = CheckState.Unchecked Then
                chkMultiplePayments.CheckState = CheckState.Unchecked
                chkMultiplePayments.Enabled = False
                chkRecommender.Enabled = False
            End If

            If (chkRecommender.CheckState = CheckState.Checked) Then
                lblAuthorisationThreshold.Visible = True
                txtAuthorisationThreshold.Visible = True
            Else
                lblAuthorisationThreshold.Visible = False
                txtAuthorisationThreshold.Visible = False
            End If
            'TN20001031 (End) process 29


            cboCoinsurerInclusion.Items.Clear()
            cboCoinsurerInclusion.Items.Add("Not Required")
            cboCoinsurerInclusion.Items.Add("Required")
            cboCoinsurerInclusion.Items.Add("Required with Statistics")
            cboCoinsurerInclusion.SelectedIndex = 0

            m_lReturn = GetLookUpDetails(v_sTableName:="GIS_User_Def_Header", r_cboControl:=cboUDT(0))
            m_lReturn = GetLookUpDetails(v_sTableName:="GIS_User_Def_Header", r_cboControl:=cboUDT(1))
            m_lReturn = GetLookUpDetails(v_sTableName:="GIS_User_Def_Header", r_cboControl:=cboUDT(2))
            m_lReturn = GetLookUpDetails(v_sTableName:="GIS_User_Def_Header", r_cboControl:=cboUDT(3))
            m_lReturn = GetLookUpDetails(v_sTableName:="GIS_User_Def_Header", r_cboControl:=cboUDT(4))
            cboUDT(0).SelectedIndex = 0
            cboUDT(1).SelectedIndex = 0
            cboUDT(2).SelectedIndex = 0
            cboUDT(3).SelectedIndex = 0
            cboUDT(4).SelectedIndex = 0

            m_lReturn = GetLookUpDetails(v_sTableName:="PMWrk_Task_Group", r_cboControl:=cboClaimTaskGroup)
            m_lReturn = GetLookUpDetails(v_sTableName:="PMUser_Group", r_cboControl:=cboClaimUserGroup)
            cboClaimTaskGroup.SelectedIndex = 0
            cboClaimUserGroup.SelectedIndex = 0

            m_lReturn = GetLookUpDetails(v_sTableName:="PMWrk_Task_Group", r_cboControl:=cboOSMTATaskGroup, bIsNoneRequire:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("SetInterfaceDefaults", "Failed to get Lookup details", gPMConstants.PMELogLevel.PMLogError)
            End If
            m_lReturn = GetLookUpDetails(v_sTableName:="PMUser_Group", r_cboControl:=cboOSMTAUserGroup, bIsNoneRequire:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("SetInterfaceDefaults", "Failed to get Lookup details", gPMConstants.PMELogLevel.PMLogError)
            End If

            cboOSMTATaskGroup.SelectedIndex = 0
            cboOSMTAUserGroup.SelectedIndex = 0
            'Start Written status
            fraWrittenStatus.Enabled = False
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtTaskManagerDays,
                                                    vControlValue:="")

            'ENdWritten Status
            'PN 55338
            ' if RI2007 product option then by default BackDatedMTA is checked
            If m_vIsRI2007 = "1" Then
                chkBackdatedMTA.CheckState = CheckState.Checked
            Else
                chkBackdatedMTA.CheckState = CheckState.Unchecked
            End If

            With cbodtAllowed
                .Items.Clear()
                'Developer Guide No. 153
                cbodtAllowed.Items.Add(New VB6.ListBoxItem(Trim("Not Allowed"), CInt(MTA_DATE_NOT_ALLOWED)))

                cbodtAllowed.Items.Add(New VB6.ListBoxItem(Trim("Current Period Only"), CInt(MTA_DATE_CURRENT_PERIOD_ONLY)))

                cbodtAllowed.Items.Add(New VB6.ListBoxItem(Trim("Current Period + 1"), CInt(MTA_DATE_CURRENT_PLUS_1)))

                cbodtAllowed.Items.Add(New VB6.ListBoxItem(Trim("Unrestricted"), CInt(MTA_DATE_UNRESTRICTED)))

                .SelectedIndex = 1
            End With

            With cboallocation
                .Items.Clear()
                'Developer Guide No. 153
                cboallocation.Items.Add(New VB6.ListBoxItem(Trim("Leave Unallocated"), CInt(MTA_ALLOCATION_LEAVE_ALONE)))

                cboallocation.Items.Add(New VB6.ListBoxItem(Trim("Reverse Allocations"), CInt(MTA_ALLOCATION_REVERSE)))

                .SelectedIndex = 0
            End With

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtdefaultRenMth, vControlValue:=0)

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
            ReDim m_ctlTabFirstLast(1, 4)

            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to set the first and last data entry
            ' controls for all of the tabs.
            '
            ' Example:-
            '
            '    Set m_ctlTabFirstLast(ACControlStart, 0) = txtName
            '    Set m_ctlTabFirstLast(ACControlEnd, 0) = txtAge
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            m_ctlTabFirstLast(ACControlStart, 0) = txtCode
            '    Set m_ctlTabFirstLast(ACControlEnd, 0) = chkRoundPremium
            m_ctlTabFirstLast(ACControlEnd, 0) = cboRoundingSection

            'TN20001101 (Start) process 29
            m_ctlTabFirstLast(ACControlStart, 1) = txtClaimYear
            m_ctlTabFirstLast(ACControlEnd, 1) = cmdCancel
            'TN20001101 (End) process 29

            m_ctlTabFirstLast(ACControlStart, 2) = txtAnniversaryRenewalWeeks
            m_ctlTabFirstLast(ACControlEnd, 2) = cmdCancel

            m_ctlTabFirstLast(ACControlStart, 3) = chkInvoice
            m_ctlTabFirstLast(ACControlEnd, 3) = cmdCancel

            m_ctlTabFirstLast(ACControlStart, 4) = uctPickListMTAEvent
            m_ctlTabFirstLast(ACControlEnd, 4) = cmdCancel

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

            ' Display all language specific captions.


            'Developer Guide No. 243
            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdApply.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACApplyButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'JMK 23/10/2001 - Display Insurer/Reinsurer


            cmdRIModel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRIModelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdSPR.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSPRButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'TN20001101 (Start) process 29

            SSTabHelper.SetTabCaption(tabMainTab, 1, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'TN20001101 (End) process 29

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    lblDesc.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************


            lblCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblEffectiveDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEffectiveDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblRIPointer.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRIPointer, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblReportPointer.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReportPointer, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblDescription.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblSchemeAgencyRef.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSchemeAgencyRef, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblBlockNo.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBlockNo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblProvClaimAutoNumberingID.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACProvClaimAutoNumberingID, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblQuoteAutoNumberingID.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACQuoteAutoNumberingID, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPolicyAutoNumberingID.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPolicyAutoNumberingID, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblFullClaimAutoNumberingID.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFullClaimAutoNumberingID, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            '    lblMidNightRenewal.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lId:=ACMidnightRenewal, _
            ''        iDataType:=PMResString)


            lblAutoRenewal.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAutoRenewal, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblTaxSuppressed.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTaxSuppressed, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblShortPeriodRated.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACShortPeriodRated, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAccumulation.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAccumulation, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'TN20010514 start

            lblNBProRata.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNBProrata, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblMTAProRata.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMTAProrata, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'TN20010514 end
            'JMK 25/07/2001 start

            lblRoundPremium.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRoundPremium, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblRoundingSection.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRoundingSection, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'JMK 25/07/2001 end


            lblPolicyNumberAtQuote.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPolicyNumberAtQuote, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAllowStandardWordingEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAllowStandardWordingEdit, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'TN20001101 (Start) process 29

            lblClaimYear.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClaimYearToCheck, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblSingleClaimValue.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMaxSingleClaimValue, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAllowedClaims.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMaxNumberOfClaim, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblTotalClaimsValue.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMaxTotalClaimValue, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'TN20001101 (End) process 29

            ' PW311002 - add grace period

            lblGracePeriod.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACGracePeriod, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Alix - 13/02/2004

            chkCheckAgent.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCheckAgent, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPositiveValues.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPositiveValues, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            ' /Alix

            'DJM 23/03/2004

            chkMediaTypeMandatory.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCheckMediaTypeMandatory, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'DJM 24/03/2004

            lblPolicyStyle.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPolicyStyleID, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPolicyStyleMandatory.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPolicyStyleMandatory, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            SSTabHelper.SetTabCaption(tabMainTab, 3, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            SSTabHelper.SetTabCaption(tabMainTab, 4, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 5, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            If chkTrueMonthlyPolicy.CheckState <> CheckState.Checked Then
                'SSTabHelper.SetTabCaption(tabMainTab, 3, "&4" & Mid(SSTabHelper.GetTabCaption(tabMainTab, 3), 3))
                'SSTabHelper.SetTabCaption(tabMainTab, 4, "&5" & Mid(SSTabHelper.GetTabCaption(tabMainTab, 4), 3))
                'SSTabHelper.SetTabCaption(tabMainTab, 5, "&6" & Mid(SSTabHelper.GetTabCaption(tabMainTab, 5), 3))
                SSTabHelper.SetTabCaption(tabMainTab, 3, "4" & Mid(SSTabHelper.GetTabCaption(tabMainTab, 3), 2))
                SSTabHelper.SetTabCaption(tabMainTab, 4, "5" & Mid(SSTabHelper.GetTabCaption(tabMainTab, 4), 2))
                SSTabHelper.SetTabCaption(tabMainTab, 5, "6" & Mid(SSTabHelper.GetTabCaption(tabMainTab, 5), 2))
            End If
            ' {* USER DEFINED CODE (End) *}
            lblApplyMandatoryRisk.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACApplyMandatoryRisk, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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
    ' Name: ValidateForm
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function ValidateForm() As Integer

        Dim result As Integer = 0
        Try

            Dim lAnniversaryRenewalWeeks, lUnifiedRenewalDay As Integer
            Dim bFound As Boolean

            result = gPMConstants.PMEReturnCode.PMTrue

            '<PN:39823 Pankaj>
            If Information.IsArray(m_vProductList) Then
                If m_sCode.Trim().ToUpper() <> Me.txtCode.Text.Trim().ToUpper() Then
                    For lRow As Integer = m_vProductList.GetLowerBound(1) To m_vProductList.GetUpperBound(1)
                        If CStr(m_vProductList(ACICode, lRow)).Trim().ToUpper() = Me.txtCode.Text.Trim().ToUpper() Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            MessageBox.Show("Product Code already exists", "Product Code", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Me.txtCode.Focus()
                            Return result
                        End If
                    Next lRow
                End If
            End If
            '</Pankaj

            If Me.cboRiskTypeGroup.ItemId = -1 Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("A value must be entered", "Risk Type Group", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.cboRiskTypeGroup.Focus()
                Return result
            End If

            If Me.cboFullClaimAutoNumberingID.SelectedIndex = -1 Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("A value must be entered", "Full Claim Auto Numbering ID", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.cboFullClaimAutoNumberingID.Focus()
                Return result
            End If

            If Me.cboPolicyAutoNumberingID.SelectedIndex = -1 Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("A value must be entered", "Policy Auto Numbering ID", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.cboPolicyAutoNumberingID.Focus()
                Return result
            End If

            If Me.cboProvClaimAutoNumberingID.SelectedIndex = -1 Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("A value must be entered", "Provisional Claim Auto Numbering ID", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.cboProvClaimAutoNumberingID.Focus()
                Return result
            End If

            If Me.cboQuoteAutoNumberingID.SelectedIndex = -1 Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("A value must be entered", "Quote Auto Numbering ID", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.cboQuoteAutoNumberingID.Focus()
                Return result
            End If

            'Start (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section(4.3.2)

            If Me.cboCNAutoNumberingID.SelectedIndex = -1 Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("A value must be entered", "Cover Note Numbering ID", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.cboCNAutoNumberingID.Focus()
                Return result
            End If

            'End (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section(4.3.2)

            'JMK 25/07/2001
            If chkRoundPremium.CheckState = CheckState.Checked Then
                If cboRoundingSection.ListIndex = 0 Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Round Premium is checked:" & Strings.Chr(13) & Strings.Chr(10) &
                                    "A Rounding Section must be selected", "Rounding Section", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    '            If cboRoundingSection.Visible Then
                    '                If cboRoundingSection.Enabled Then
                    cboRoundingSection.Focus()
                    '                End If
                    '            End If
                    Return result
                End If
            End If

            ' get a safe version of the anniversary weeks
            If chkTrueMonthlyPolicy.CheckState = CheckState.Checked Then
                lAnniversaryRenewalWeeks = gPMFunctions.ToSafeLong(txtAnniversaryRenewalWeeks.Text, 0)
                If lAnniversaryRenewalWeeks = 0 Or lAnniversaryRenewalWeeks < 0 Or lAnniversaryRenewalWeeks > 51 Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Anniversary renewal weeks must be a value between 0 and 52", "Product Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    txtAnniversaryRenewalWeeks.Focus()
                    Return result
                End If
                If txtUnifiedRenewalDay.Text.Trim() <> "" Then
                    lUnifiedRenewalDay = gPMFunctions.ToSafeLong(txtUnifiedRenewalDay.Text, 0)
                    If lUnifiedRenewalDay = 0 Or lUnifiedRenewalDay < 0 Or lUnifiedRenewalDay > 31 Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        MessageBox.Show("Unified Renewal Day Value can only be between 1 and 31", "Product Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        txtUnifiedRenewalDay.Focus()
                        Return result
                    End If
                End If
            End If

            If chkMultiplePayments.CheckState = "1" Then
                Dim dbNumericTemp As Double
                If Not Double.TryParse(txtMaxUnauthorisedClaimsValue.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) And txtMaxUnauthorisedClaimsValue.Text.Trim() <> "" Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Max. Unauthorised Claims Value should be Numeric. ", "Product Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return result
                End If
                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(txtMaxNoofUnauthorisedClaimPayments.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) And txtMaxNoofUnauthorisedClaimPayments.Text.Trim() <> "" Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("'Max. No. of Unauthorised Claim Payments' should be Numeric. ", "Product Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return result
                End If
            End If

            ' At least one of the Make Live Options must be choosen
            'Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
            If chkInvoice.CheckState <> CheckState.Checked And chkInstalments.CheckState <> CheckState.Checked And chkPayNow.CheckState <> CheckState.Checked And chkBankGuarantee.CheckState <> CheckState.Checked And chkCashDeposit.CheckState <> CheckState.Checked Then
                MessageBox.Show("You must choose at least one of the Make Live options.", "Product Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                SSTabHelper.SetSelectedIndex(tabMainTab, 4)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            bFound = False
            If chkAllowConsolidateCommissionLA.CheckState = CheckState.Checked Then
                If cboMonthInCycleLA.SelectedIndex = -1 Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("A valid month in Cycle entry is required", "Product Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.cboMonthInCycleLA.Focus()
                    Return result
                Else
                    If actSuspenseAcc.Text.Trim() = "" Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        MessageBox.Show("A Lead Agent Commission Suspense Account must be selected", "Product Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Me.actSuspenseAcc.Focus()
                        Return result
                    Else

                        m_lReturn = m_oBusiness.ValidateAccountCode(actSuspenseAcc.Text.Trim(), bFound)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        If Not bFound Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            MessageBox.Show("Invalid Lead Agent Commission Suspense Account Code", "Product Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Me.actSuspenseAcc.Focus()
                            Return result
                        End If
                    End If
                End If
            End If

            bFound = False
            If chkAllowConsolidateCommissionSA.CheckState = CheckState.Checked Then
                If cboMonthInCycleSA.SelectedIndex = -1 Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("A valid month in Cycle entry is required", "Product Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.cboMonthInCycleSA.Focus()
                    Return result
                Else
                    If actSuspenseAcc1.Text.Trim() = "" Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        MessageBox.Show("A Sub Agent Commission Suspense Account must be selected", "Product Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Me.actSuspenseAcc1.Focus()
                        Return result
                    Else

                        m_lReturn = m_oBusiness.ValidateAccountCode(actSuspenseAcc1.Text.Trim(), bFound)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        If Not bFound Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            MessageBox.Show("Invalid Sub Agent Commission Suspense Account Code", "Product Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Me.actSuspenseAcc1.Focus()
                            Return result
                        End If
                    End If
                End If
            End If

            If Me.cboClaimTaskGroup.SelectedIndex = 0 Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("Please enter a Claim Task Group.", "Claim Task Group", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If chkTrueMonthlyPolicy.CheckState = CheckState.Checked Then
                    SSTabHelper.SetSelectedIndex(tabMainTab, 5)
                Else
                    SSTabHelper.SetSelectedIndex(tabMainTab, 4)
                End If
                Me.cboClaimTaskGroup.Focus()
                Return result
            End If
            If Me.cboClaimUserGroup.SelectedIndex = 0 Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("Please enter a Claim User Group", "Claim User Group", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If chkTrueMonthlyPolicy.CheckState = CheckState.Checked Then
                    SSTabHelper.SetSelectedIndex(tabMainTab, 5)
                Else
                    SSTabHelper.SetSelectedIndex(tabMainTab, 4)
                End If
                Me.cboClaimUserGroup.Focus()
                Return result
            End If

            'Start (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()
            If VB6.GetItemData(cboCNAutoNumberingID, cboCNAutoNumberingID.SelectedIndex) <> 0 Then


                Select Case PMEmptyString
                    Case txtCNDefaultPeriod.Text.Trim(), gPMFunctions.NullToString(gPMFunctions.ToSafeInteger(txtCNDefaultPeriod.Text))
                        result = gPMConstants.PMEReturnCode.PMFalse
                        MessageBox.Show("'Cover Note default period' must be entered", "Cover Note Group", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If chkTrueMonthlyPolicy.CheckState = CheckState.Checked Then
                            SSTabHelper.SetSelectedIndex(tabMainTab, 5)
                        Else
                            SSTabHelper.SetSelectedIndex(tabMainTab, 4)
                        End If
                        txtCNDefaultPeriod.Focus()
                        Return result

                    Case txtCNDocTemplate.Text.Trim()
                        result = gPMConstants.PMEReturnCode.PMFalse
                        MessageBox.Show("'Cover Note Doc Template' must be selected", "Cover Note Group", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If chkTrueMonthlyPolicy.CheckState = CheckState.Checked Then
                            SSTabHelper.SetSelectedIndex(tabMainTab, 5)
                        Else
                            SSTabHelper.SetSelectedIndex(tabMainTab, 4)
                        End If
                        'txtCNDocTemplate.SetFocus
                        cmdCNDocTemplate.Focus()
                        Return result

                    Case txtCNMaxNo.Text.Trim(), gPMFunctions.NullToString(gPMFunctions.ZeroToNull(txtCNMaxNo.Text))
                        result = gPMConstants.PMEReturnCode.PMFalse
                        MessageBox.Show("'Maximum No. of Cover Notes' must be entered", "Cover Note Group", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If chkTrueMonthlyPolicy.CheckState = CheckState.Checked Then
                            SSTabHelper.SetSelectedIndex(tabMainTab, 5)
                        Else
                            SSTabHelper.SetSelectedIndex(tabMainTab, 4)
                        End If
                        txtCNMaxNo.Focus()
                        Return result
                End Select

                Dim dbNumericTemp3 As Double
                If Not Double.TryParse(txtCNDefaultPeriod.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Warning: 'Cover Note default period' field accepts only integer values. Please try again", "Cover Note Group", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If chkTrueMonthlyPolicy.CheckState = CheckState.Checked Then
                        SSTabHelper.SetSelectedIndex(tabMainTab, 5)
                    Else
                        SSTabHelper.SetSelectedIndex(tabMainTab, 4)
                    End If
                    txtCNDefaultPeriod.Focus()
                    Return result
                End If


                Dim dbNumericTemp4 As Double
                If Not Double.TryParse(txtCNMaxNo.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Warning: 'Maximum No. of Cover Notes' field accepts only integer values. Please try again", "Cover Note Group", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If chkTrueMonthlyPolicy.CheckState = CheckState.Checked Then
                        SSTabHelper.SetSelectedIndex(tabMainTab, 5)
                    Else
                        SSTabHelper.SetSelectedIndex(tabMainTab, 4)
                    End If
                    txtCNMaxNo.Focus()
                    Return result
                End If

            End If
            'End (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()

            'Start - Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling
            If chkRoundOffToZero.CheckState = CheckState.Checked Then
                m_lReturn = iPMFunc.GetSystemOption(kSystemOptionRoundOffAccount, m_sAccountCode, g_iSourceID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("ValidateForm", "Failed to get System Option", gPMConstants.PMELogLevel.PMLogError)
                    Return gPMConstants.PMEReturnCode.PMFalse
                ElseIf m_sAccountCode = "0" Or m_sAccountCode = "" Then
                    MessageBox.Show("No Round off Account Code configured - Please contact system administrator", "Product Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            'End - Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling
            If uctPickListBranches.SelectedItems = 0 Then
                MessageBox.Show("You must attach at least one Branch to the Product", "Product Maintenance ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                result = gPMConstants.PMEReturnCode.PMFalse
                SSTabHelper.SetSelectedIndex(tabMainTab, 10)
                uctPickListBranches.Focus()
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateForm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '
    '

    ' ***************************************************************** '
    '
    ' Name: GetLookUpDetails
    '
    ' Description: Get lookup values from table to be display in combo box
    '
    ' ***************************************************************** '
    Private Function GetLookUpDetails(ByVal v_sTableName As String, ByRef r_cboControl As ComboBox, Optional ByVal bIsNoneRequire As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'add N/A to combo box
            r_cboControl.Items.Clear()

            ' this is to remove the blank space from the combobox
            'Modified by Sumeet Singh on 5/12/2010 12:12:15 PM made the variable global so that it can be used at all places
            Dim r_cboControl_NewIndex As Integer = -1
            If bIsNoneRequire Then
                'Dim r_cboControl_NewIndex As Integer = -1
                r_cboControl_NewIndex = r_cboControl.Items.Add("None")
            Else
                r_cboControl_NewIndex = r_cboControl.Items.Add("")
            End If
            VB6.SetItemData(r_cboControl, r_cboControl_NewIndex, 0)


            If m_oBusiness.GetLookUp(v_sTableName:=v_sTableName, r_vResultArray:=vResultArray) = gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMTrue

                If Information.IsArray(vResultArray) Then

                    For lCount As Integer = 0 To vResultArray.GetUpperBound(1)

                        r_cboControl_NewIndex = r_cboControl.Items.Add(CStr(vResultArray(2, lCount)))

                        VB6.SetItemData(r_cboControl, r_cboControl_NewIndex, CInt(vResultArray(0, lCount)))
                    Next
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookUpDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookUpDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetComboDetails
    '
    ' Description: get details from numbering scheme and add to combobox
    '
    '
    ' ***************************************************************** '
    Private Function GetComboDetails(ByVal v_lNumberingSchemeTypeID As Integer, ByRef r_cboControl As ComboBox) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            'make sure combobox is empty
            r_cboControl.Items.Clear()

            'add in non applicable value with ID of 0
            Dim r_cboControl_NewIndex As Integer = -1
            r_cboControl_NewIndex = r_cboControl.Items.Add("(N/A)")
            VB6.SetItemData(r_cboControl, r_cboControl_NewIndex, 0)


            m_lReturn = m_oBusiness.GetComboDetails(v_lNumberingSchemeTypeID, vResultArray)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                If Information.IsArray(vResultArray) Then

                    For icount As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                        r_cboControl_NewIndex = r_cboControl.Items.Add(CStr(vResultArray(1, icount)))

                        VB6.SetItemData(r_cboControl, r_cboControl_NewIndex, CInt(vResultArray(0, icount)))
                    Next
                End If
            End If


            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetComboDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetComboDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cboCNAutoNumberingID_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCNAutoNumberingID.SelectedIndexChanged
        If VB6.GetItemData(cboCNAutoNumberingID, cboCNAutoNumberingID.SelectedIndex) <> 0 Then

            txtCNDefaultPeriod.Enabled = True
            lblCNDefaultPeriod.Font = VB6.FontChangeBold(lblCNDefaultPeriod.Font, True)

            txtCNMaxNo.Enabled = True
            lblCNMaxNo.Font = VB6.FontChangeBold(lblCNMaxNo.Font, True)

            'm_oFormFields.Item(txtCNMaxNo).IsMandatory = PMNonMandatory

            cmdCNDocTemplate.Enabled = True
            txtCNDocTemplate.Enabled = False
            lblCNDocTemplate.Font = VB6.FontChangeBold(lblCNDocTemplate.Font, True)

            'm_oFormFields.Item(txtCNDocTemplate).IsMandatory = PMNonMandatory

        Else

            txtCNDefaultPeriod.Enabled = False
            txtCNDefaultPeriod.Text = ""
            lblCNDefaultPeriod.Font = VB6.FontChangeBold(lblCNDefaultPeriod.Font, False)

            'm_oFormFields.Item(txtCNDefaultPeriod).IsMandatory = PMMandatory

            txtCNMaxNo.Enabled = False
            txtCNMaxNo.Text = ""
            lblCNMaxNo.Font = VB6.FontChangeBold(lblCNMaxNo.Font, False)

            'm_oFormFields.Item(txtCNMaxNo).IsMandatory = PMMandatory

            cmdCNDocTemplate.Enabled = False
            txtCNDocTemplate.Enabled = False
            txtCNDocTemplate.Text = ""

            m_vCNDocTemplateId = 0
            m_vCNTemplateCode = ""
            lblCNDocTemplate.Font = VB6.FontChangeBold(lblCNDocTemplate.Font, False)

            'm_oFormFields.Item(txtCNDocTemplate).IsMandatory = PMMandatory
        End If
    End Sub
    'End (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()

    Private Sub chkAllowConsolidateCommissionLA_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkAllowConsolidateCommissionLA.CheckStateChanged

        'SOCB (Mansi Gandhi 8 Jan 2014) Product Screen- True Monthly Policy
        If chkAllowConsolidateCommissionLA.CheckState = CheckState.Checked Then
            cboMonthInCycleLA.Visible = True
            actSuspenseAcc.Visible = True
            lblMonthInCycleLA.Visible = True
            lblLeadAgentCommSuspenseLA.Visible = True

        Else
            cboMonthInCycleLA.Visible = False
            actSuspenseAcc.Visible = False
            lblMonthInCycleLA.Visible = False
            lblLeadAgentCommSuspenseLA.Visible = False
            cboMonthInCycleLA.SelectedIndex = 0
            actSuspenseAcc.Text = ""

            'EOCB (Mansi Gandhi 8 Jan 2014) Product Screen- True Monthly Policy
        End If
    End Sub

    Private Sub chkAllowConsolidateCommissionSA_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkAllowConsolidateCommissionSA.CheckStateChanged
        If chkAllowConsolidateCommissionSA.CheckState = CheckState.Checked Then
            cboMonthInCycleSA.Visible = True
            actSuspenseAcc1.Visible = True
            lblMonthInCycleSA.Visible = True
            lblSubAgentCommSuspense.Visible = True
        Else
            cboMonthInCycleSA.Visible = False
            actSuspenseAcc1.Visible = False
            lblMonthInCycleSA.Visible = False
            lblSubAgentCommSuspense.Visible = False
        End If
    End Sub

    Private Sub chkMultiplePayments_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkMultiplePayments.CheckStateChanged
        If chkMultiplePayments.CheckState = CheckState.Checked Then
            txtMaxUnauthorisedClaimsValue.Visible = True
            lblMaxUnauthorisedClaimsValue.Visible = True
            txtMaxNoofUnauthorisedClaimPayments.Visible = True
            lblMaxNoofUnauthorisedClaimPayments.Visible = True
        Else
            txtMaxUnauthorisedClaimsValue.Visible = False
            lblMaxUnauthorisedClaimsValue.Visible = False
            txtMaxNoofUnauthorisedClaimPayments.Visible = False
            lblMaxNoofUnauthorisedClaimPayments.Visible = False
            txtMaxUnauthorisedClaimsValue.Text = ""
            txtMaxNoofUnauthorisedClaimPayments.Text = ""
        End If
    End Sub

    Private Sub chkOpenClaimWorkflow_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _chkOpenClaimWorkflow_25.CheckStateChanged, _chkOpenClaimWorkflow_24.CheckStateChanged, _chkOpenClaimWorkflow_23.CheckStateChanged, _chkOpenClaimWorkflow_22.CheckStateChanged, _chkOpenClaimWorkflow_21.CheckStateChanged, _chkOpenClaimWorkflow_20.CheckStateChanged, _chkOpenClaimWorkflow_19.CheckStateChanged, _chkOpenClaimWorkflow_18.CheckStateChanged, _chkOpenClaimWorkflow_17.CheckStateChanged, _chkOpenClaimWorkflow_16.CheckStateChanged, _chkOpenClaimWorkflow_15.CheckStateChanged, _chkOpenClaimWorkflow_14.CheckStateChanged, _chkOpenClaimWorkflow_13.CheckStateChanged, _chkOpenClaimWorkflow_12.CheckStateChanged, _chkOpenClaimWorkflow_11.CheckStateChanged, _chkOpenClaimWorkflow_10.CheckStateChanged, _chkOpenClaimWorkflow_9.CheckStateChanged, _chkOpenClaimWorkflow_8.CheckStateChanged, _chkOpenClaimWorkflow_7.CheckStateChanged, _chkOpenClaimWorkflow_6.CheckStateChanged, _chkOpenClaimWorkflow_5.CheckStateChanged, _chkOpenClaimWorkflow_4.CheckStateChanged, _chkOpenClaimWorkflow_3.CheckStateChanged, _chkOpenClaimWorkflow_2.CheckStateChanged, _chkOpenClaimWorkflow_1.CheckStateChanged, _chkOpenClaimWorkflow_0.CheckStateChanged
        Dim Index As Integer = Array.IndexOf(chkOpenClaimWorkflow, eventSender)
        Select Case Index
            Case 9
                'Claim Notification doc message should have Generate doc checked
                If chkOpenClaimWorkflow(9).CheckState = CheckState.Checked Then
                    chkOpenClaimWorkflow(10).CheckState = CheckState.Checked
                End If

            Case 10
                'if Generate doc unchecked then Claim Notification docs message shouldn't come
                If chkOpenClaimWorkflow(10).CheckState = CheckState.Unchecked Then
                    chkOpenClaimWorkflow(9).CheckState = CheckState.Unchecked
                End If

            Case 11
                'An unchecked Claim Payment Process should have
                'all the payment related steps unchecked
                If chkOpenClaimWorkflow(11).CheckState = CheckState.Unchecked Then
                    chkOpenClaimWorkflow(12).CheckState = CheckState.Unchecked
                    chkOpenClaimWorkflow(13).CheckState = CheckState.Unchecked
                    chkOpenClaimWorkflow(14).CheckState = CheckState.Unchecked
                    chkOpenClaimWorkflow(15).CheckState = CheckState.Unchecked
                    chkOpenClaimWorkflow(16).CheckState = CheckState.Unchecked
                    chkOpenClaimWorkflow(17).CheckState = CheckState.Unchecked
                    chkOpenClaimWorkflow(18).CheckState = CheckState.Unchecked
                    chkOpenClaimWorkflow(19).CheckState = CheckState.Unchecked
                    chkOpenClaimWorkflow(20).CheckState = CheckState.Unchecked
                    chkOpenClaimWorkflow(21).CheckState = CheckState.Unchecked
                    chkOpenClaimWorkflow(22).CheckState = CheckState.Unchecked
                    chkOpenClaimWorkflow(23).CheckState = CheckState.Unchecked
                    chkOpenClaimWorkflow(24).CheckState = CheckState.Unchecked
                    chkOpenClaimWorkflow(25).CheckState = CheckState.Unchecked
                Else
                    'Mandatory steps should be checked
                    chkOpenClaimWorkflow(12).CheckState = CheckState.Checked
                    chkOpenClaimWorkflow(14).CheckState = CheckState.Checked
                    chkOpenClaimWorkflow(15).CheckState = CheckState.Checked
                    chkOpenClaimWorkflow(18).CheckState = CheckState.Checked
                    chkOpenClaimWorkflow(20).CheckState = CheckState.Checked
                    chkOpenClaimWorkflow(24).CheckState = CheckState.Checked
                    chkOpenClaimWorkflow(25).CheckState = CheckState.Checked
                End If
            Case 13, 16, 17, 19, 21, 22, 23
                'If any configurable step is checked then claim payment process should be checked
                If chkOpenClaimWorkflow(Index).CheckState = CheckState.Checked Then
                    chkOpenClaimWorkflow(11).CheckState = CheckState.Checked
                End If

                If Index = 21 And chkOpenClaimWorkflow(21).CheckState = CheckState.Checked Then
                    'Claim Payment doc message should have Generate doc checked
                    chkOpenClaimWorkflow(22).CheckState = CheckState.Checked
                ElseIf Index = 22 And chkOpenClaimWorkflow(22).CheckState = CheckState.Unchecked Then
                    'if Generate doc unchecked then Claim Payment doc message shouldn't come
                    chkOpenClaimWorkflow(21).CheckState = CheckState.Unchecked
                End If

        End Select
    End Sub

    Private Sub chkMaintainClaimWorkflow_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _chkMaintainClaimWorkflow_26.CheckStateChanged, _chkMaintainClaimWorkflow_25.CheckStateChanged, _chkMaintainClaimWorkflow_24.CheckStateChanged, _chkMaintainClaimWorkflow_23.CheckStateChanged, _chkMaintainClaimWorkflow_22.CheckStateChanged, _chkMaintainClaimWorkflow_21.CheckStateChanged, _chkMaintainClaimWorkflow_20.CheckStateChanged, _chkMaintainClaimWorkflow_19.CheckStateChanged, _chkMaintainClaimWorkflow_18.CheckStateChanged, _chkMaintainClaimWorkflow_17.CheckStateChanged, _chkMaintainClaimWorkflow_16.CheckStateChanged, _chkMaintainClaimWorkflow_15.CheckStateChanged, _chkMaintainClaimWorkflow_14.CheckStateChanged, _chkMaintainClaimWorkflow_13.CheckStateChanged, _chkMaintainClaimWorkflow_12.CheckStateChanged, _chkMaintainClaimWorkflow_11.CheckStateChanged, _chkMaintainClaimWorkflow_10.CheckStateChanged, _chkMaintainClaimWorkflow_8.CheckStateChanged, _chkMaintainClaimWorkflow_7.CheckStateChanged, _chkMaintainClaimWorkflow_6.CheckStateChanged, _chkMaintainClaimWorkflow_5.CheckStateChanged, _chkMaintainClaimWorkflow_4.CheckStateChanged, _chkMaintainClaimWorkflow_3.CheckStateChanged, _chkMaintainClaimWorkflow_2.CheckStateChanged, _chkMaintainClaimWorkflow_1.CheckStateChanged, _chkMaintainClaimWorkflow_0.CheckStateChanged, _chkMaintainClaimWorkflow_9.CheckStateChanged
        Dim Index As Integer = Array.IndexOf(chkMaintainClaimWorkflow, eventSender)
        Select Case Index
            Case 10
                'Claim Notification doc message should have Generate doc checked
                If chkMaintainClaimWorkflow(10).CheckState = CheckState.Checked Then
                    chkMaintainClaimWorkflow(11).CheckState = CheckState.Checked
                End If

            Case 11
                'if Generate doc unchecked then Claim Notification docs message shouldn't come
                If chkMaintainClaimWorkflow(11).CheckState = CheckState.Unchecked Then
                    chkMaintainClaimWorkflow(10).CheckState = CheckState.Unchecked
                End If

            Case 12
                'An unchecked Claim Payment Process should have
                'all the payment related steps unchecked
                If chkMaintainClaimWorkflow(12).CheckState = CheckState.Unchecked Then
                    chkMaintainClaimWorkflow(13).CheckState = CheckState.Unchecked
                    chkMaintainClaimWorkflow(14).CheckState = CheckState.Unchecked
                    chkMaintainClaimWorkflow(15).CheckState = CheckState.Unchecked
                    chkMaintainClaimWorkflow(16).CheckState = CheckState.Unchecked
                    chkMaintainClaimWorkflow(17).CheckState = CheckState.Unchecked
                    chkMaintainClaimWorkflow(18).CheckState = CheckState.Unchecked
                    chkMaintainClaimWorkflow(19).CheckState = CheckState.Unchecked
                    chkMaintainClaimWorkflow(20).CheckState = CheckState.Unchecked
                    chkMaintainClaimWorkflow(21).CheckState = CheckState.Unchecked
                    chkMaintainClaimWorkflow(22).CheckState = CheckState.Unchecked
                    chkMaintainClaimWorkflow(23).CheckState = CheckState.Unchecked
                    chkMaintainClaimWorkflow(24).CheckState = CheckState.Unchecked
                    chkMaintainClaimWorkflow(26).CheckState = CheckState.Unchecked
                Else
                    'Mandatory steps should be checked
                    chkMaintainClaimWorkflow(13).CheckState = CheckState.Checked
                    chkMaintainClaimWorkflow(15).CheckState = CheckState.Checked
                    chkMaintainClaimWorkflow(16).CheckState = CheckState.Checked
                    chkMaintainClaimWorkflow(19).CheckState = CheckState.Checked
                    chkMaintainClaimWorkflow(21).CheckState = CheckState.Checked
                    chkMaintainClaimWorkflow(26).CheckState = CheckState.Checked
                End If
            Case 14, 17, 18, 20, 22, 23, 24
                'If any configurable step is checked then claim payment process should be checked
                If chkMaintainClaimWorkflow(Index).CheckState = CheckState.Checked Then
                    chkMaintainClaimWorkflow(12).CheckState = CheckState.Checked
                End If

                If Index = 22 And chkMaintainClaimWorkflow(22).CheckState = CheckState.Checked Then
                    'Claim Payment doc message should have Generate doc checked
                    chkMaintainClaimWorkflow(23).CheckState = CheckState.Checked
                ElseIf Index = 23 And chkMaintainClaimWorkflow(23).CheckState = CheckState.Unchecked Then
                    'if Generate doc unchecked then Claim Payment doc message shouldn't come
                    chkMaintainClaimWorkflow(22).CheckState = CheckState.Unchecked
                End If

        End Select
    End Sub

    Private Sub chkPaymentClaimWorkflow_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _chkPaymentClaimWorkflow_16.CheckStateChanged, _chkPaymentClaimWorkflow_15.CheckStateChanged, _chkPaymentClaimWorkflow_14.CheckStateChanged, _chkPaymentClaimWorkflow_13.CheckStateChanged, _chkPaymentClaimWorkflow_12.CheckStateChanged, _chkPaymentClaimWorkflow_11.CheckStateChanged, _chkPaymentClaimWorkflow_10.CheckStateChanged, _chkPaymentClaimWorkflow_9.CheckStateChanged, _chkPaymentClaimWorkflow_8.CheckStateChanged, _chkPaymentClaimWorkflow_7.CheckStateChanged, _chkPaymentClaimWorkflow_6.CheckStateChanged, _chkPaymentClaimWorkflow_5.CheckStateChanged, _chkPaymentClaimWorkflow_4.CheckStateChanged, _chkPaymentClaimWorkflow_3.CheckStateChanged, _chkPaymentClaimWorkflow_2.CheckStateChanged, _chkPaymentClaimWorkflow_1.CheckStateChanged, _chkPaymentClaimWorkflow_0.CheckStateChanged

        Dim Index As Integer = Array.IndexOf(chkPaymentClaimWorkflow, eventSender)
        Select Case Index
            Case 12
                'Claim Payment doc message should have Generate doc checked
                If chkPaymentClaimWorkflow(12).CheckState = CheckState.Checked Then
                    chkPaymentClaimWorkflow(13).CheckState = CheckState.Checked
                End If

            Case 13
                'if Generate doc unchecked then Claim Payment docs message shouldn't come
                If chkPaymentClaimWorkflow(13).CheckState = CheckState.Unchecked Then
                    chkPaymentClaimWorkflow(12).CheckState = CheckState.Unchecked
                End If

        End Select
    End Sub

    Private Sub chkPolicyNumberAtQuote_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkPolicyNumberAtQuote.CheckStateChanged
        If chkPolicyNumberAtQuote.CheckState = CheckState.Checked Then
            lblQuoteAutoNumberingID.Enabled = False
            cboQuoteAutoNumberingID.Enabled = False
        Else
            lblQuoteAutoNumberingID.Enabled = True
            cboQuoteAutoNumberingID.Enabled = True
        End If
    End Sub

    Private Sub chkReserve_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkReserve.CheckStateChanged
        SetupClaimTransactionSuppression()
    End Sub

    Private Sub chkRoundPremium_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkRoundPremium.CheckStateChanged
        cboRoundingSection.Enabled = (chkRoundPremium.CheckState = CheckState.Checked)

        If Not cboRoundingSection.Enabled Then
            cboRoundingSection.ItemId = 0
        End If
    End Sub

    Private Sub chkRunAuthorisationScriptsClaimPayments_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkRunAuthorisationScriptsClaimPayments.CheckStateChanged
        If chkRunAuthorisationScriptsClaimPayments.CheckState = CheckState.Unchecked Then
            chkMultiplePayments.CheckState = CheckState.Unchecked
            chkMultiplePayments.Enabled = False
            chkRecommender.CheckState = CheckState.Unchecked
            chkRecommender.Enabled = False
        Else
            chkMultiplePayments.Enabled = True
            chkRecommender.Enabled = True
        End If
    End Sub

    Private Sub chkShortPeriodRated_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkShortPeriodRated.CheckStateChanged
        'enable short period rate button is short period rate is checked
        'and we are not in adding mode
        If Task = gPMConstants.PMEComponentAction.PMEdit Then
            Me.cmdSPR.Enabled = Me.chkShortPeriodRated.CheckState
        End If
    End Sub


    Private Sub chkTrueMonthlyPolicy_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkTrueMonthlyPolicy.CheckStateChanged
        SetupTrueMonthlyPolicy()
    End Sub

    Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click
        ' Click event of the Apply button.
        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            m_lReturn = ValidateForm()
            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'ajm - display message for failed apply
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            Task = gPMConstants.PMEComponentAction.PMEdit
            cmdRIModel.Enabled = True

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Apply command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdApply_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdCNDocTemplate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCNDocTemplate.Click

        Dim sDocumentCode As String = ""
        Dim lDocumentTemplateId As Integer

        m_lReturn = GetTheTemplate(lDocumentTemplateId, sDocumentCode, False)

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            If sDocumentCode.Length > 0 Then

                txtCNDocTemplate.Text = sDocumentCode

                m_vCNDocTemplateId = lDocumentTemplateId

            End If
        End If



    End Sub
    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, ScreenhelpID)
    End Sub

    Private Sub cmdRIModel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRIModel.Click

        Dim oRIModel As iPMURIModelUsage.Interface_Renamed

        Try

            oRIModel = New iPMURIModelUsage.Interface_Renamed()
            'Developer Guide No. 9
            If oRIModel.Initialise() <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to initialise RIModelUsage object", Application.ProductName)
                oRIModel = Nothing
                Exit Sub
            End If

            oRIModel.SetProcessModes(m_iTask)

            'TODOF
            'oRIModel.ProductID = m_lProductID

            oRIModel.Description = CStr(m_vDescription)

            If oRIModel.Start() <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to start RIModelUsage", ACApp, MessageBoxButtons.OK)
            End If

            oRIModel.Dispose()

            oRIModel = Nothing

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the RI Model Usage object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdRIModel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)



            Exit Sub

        End Try

    End Sub

    Private Sub cmdSPR_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSPR.Click

        Dim oSPR As iPMUShortPeriodRateFind.Interface_Renamed


        Try

            oSPR = New iPMUShortPeriodRateFind.Interface_Renamed()
            'Developer Guide No. 9
            If oSPR.Initialise() <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to initialise ShortPeriodRate object", ACApp, MessageBoxButtons.OK)

                oSPR = Nothing
                Exit Sub
            End If

            oSPR.SetProcessModes(Task)
            oSPR.ProductId = m_lProductID

            If oSPR.Start() <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to start ShortPeriodRate object", ACApp, MessageBoxButtons.OK)
            End If

            oSPR.Dispose()

            oSPR = Nothing

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the Short Period Rate object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdSPR_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' PRIVATE Methods (End)

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.
        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRProduct.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse


                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If


            ' Create an instance of the general interface object.
            m_oGeneral = New iPMUProduct.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID

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
    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' PW311002
        Dim vIsNRMA As Byte
        Dim vTrueMonthlyPoliciesEnabled As Object
        Dim iTabNum As Integer

        Dim key As uctPickList.PickListKey


        ' Forms load event.

        Try
            'Developer Guide No. 38
            Me.cboRoundingSection.FirstItem = "(N/A)"
            Me.cboPolicyStyle.FirstItem = "(N/A)"
            Me.cboRiskTypeGroup.FirstItem = "(N/A)"
            Me.cboBankAccount.FirstItem = "(None)"
            Me.cboApplyMandatoryRisk.FirstItem = "(N/A)"
            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}

            ' {* USER DEFINED CODE (End) *}

            ' PW311002 - Get the NRMA flag
            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTIsNRMA, v_vBranch:=g_iSourceID, r_vUnderwriting:=CStr(vIsNRMA))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTIsNRMA, vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
            End If
            m_bIsNRMA = (vIsNRMA = 1)

            ' Determine if True Monthly Policies is enabled

            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTrueMonthlyPoliciesEnabled, v_vBranch:=g_iSourceID, r_vUnderwriting:=CStr(vTrueMonthlyPoliciesEnabled))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTIsNRMA, vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
            End If

            'm_bTrueMonthlyPoliciesEnabled = (vTrueMonthlyPoliciesEnabled = 1)

            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            'JMK 23/10/2001 - get hidden option

            m_sUnderwritingType = m_oBusiness.UnderwritingType

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()
            If chkTrueMonthlyPolicy.CheckState = CheckState.Unchecked Then
                SSTabHelper.SetTabVisible(tabMainTab, 3, False)
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If


            key = New uctPickList.PickListKey()
            key.KeyName = "product_id"
            key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListCausations.ForeignKeys.Add(key, Key:="product_id")

            key = New uctPickList.PickListKey()
            key.KeyName = "primary_cause_id"
            key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListCausations.ForeignKeys.Add(key, Key:="primary_cause_id")

            key = New uctPickList.PickListKey()
            key.KeyName = "UserId"
            key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListCausations.ForeignKeys.Add(key, Key:="UserId")

            key = New uctPickList.PickListKey()
            key.KeyName = "UniqueId"
            key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListCausations.ForeignKeys.Add(key, Key:="UniqueId")

            key = New uctPickList.PickListKey()
            key.KeyName = "ScreenHierarchy"
            key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListCausations.ForeignKeys.Add(key, Key:="ScreenHierarchy")


            uctPickListCausations.ForeignKeys.Item("product_id").Value = m_lProductID

            uctPickListCausations.ForeignKeys.Item("primary_cause_id").Value = 1


            'Developer Guide No. 184 (Latest Guide)
            m_lReturn = uctPickListCausations.Load_Renamed

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to load list of Causations", "Product Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If


            key = New uctPickList.PickListKey()
            key.KeyName = "product_id"
            key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListMTAEvent.ForeignKeys.Add(key, Key:="product_id")

            key = New uctPickList.PickListKey()
            key.KeyName = "mta_event_description_id"
            key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListMTAEvent.ForeignKeys.Add(key, Key:="mta_event_description_id")

            key = New uctPickList.PickListKey()
            key.KeyName = "UserId"
            key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListMTAEvent.ForeignKeys.Add(key, Key:="UserId")

            key = New uctPickList.PickListKey()
            key.KeyName = "UniqueId"
            key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListMTAEvent.ForeignKeys.Add(key, Key:="UniqueId")

            key = New uctPickList.PickListKey()
            key.KeyName = "ScreenHierarchy"
            key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListMTAEvent.ForeignKeys.Add(key, Key:="ScreenHierarchy")

            uctPickListMTAEvent.ForeignKeys.Item("product_id").Value = m_lProductID

            uctPickListMTAEvent.ForeignKeys.Item("mta_event_description_id").Value = 1
            'Developer Guide No. 184 (Latest Guide)
            m_lReturn = uctPickListMTAEvent.Load_Renamed

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to load list of MTA Events Description", "Product Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            key = New uctPickList.PickListKey()
            key.KeyName = "product_id"
            key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListClaimEvent.ForeignKeys.Add(key, Key:="product_id")

            key = New uctPickList.PickListKey()
            key.KeyName = "claim_event_description_id"
            key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListClaimEvent.ForeignKeys.Add(key, Key:="claim_event_description_id")

            key = New uctPickList.PickListKey()
            key.KeyName = "UserId"
            key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListClaimEvent.ForeignKeys.Add(key, Key:="UserId")

            key = New uctPickList.PickListKey()
            key.KeyName = "UniqueId"
            key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListClaimEvent.ForeignKeys.Add(key, Key:="UniqueId")

            key = New uctPickList.PickListKey()
            key.KeyName = "ScreenHierarchy"
            key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListClaimEvent.ForeignKeys.Add(key, Key:="ScreenHierarchy")

            uctPickListClaimEvent.ForeignKeys.Item("product_id").Value = m_lProductID

            uctPickListClaimEvent.ForeignKeys.Item("claim_event_description_id").Value = 1
            'Developer Guide No. 184 (Latest Guide)
            m_lReturn = uctPickListClaimEvent.Load_Renamed

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to load list of Claim Events Description", "Product Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            key = New uctPickList.PickListKey()
            key.KeyName = "product_id"
            key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListBranches.ForeignKeys.Add(key, Key:="product_id")

            key = New uctPickList.PickListKey()
            key.KeyName = "source_id"
            key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListBranches.ForeignKeys.Add(key, Key:="source_id")

            key = New uctPickList.PickListKey()
            key.KeyName = "UserId"
            key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListBranches.ForeignKeys.Add(key, Key:="UserId")

            key = New uctPickList.PickListKey()
            key.KeyName = "UniqueId"
            key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListBranches.ForeignKeys.Add(key, Key:="UniqueId")

            key = New uctPickList.PickListKey()
            key.KeyName = "ScreenHierarchy"
            key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListBranches.ForeignKeys.Add(key, Key:="ScreenHierarchy")

            uctPickListBranches.ForeignKeys.Item("product_id").Value = m_lProductID

            uctPickListBranches.ForeignKeys.Item("source_id").Value = 1

            m_lReturn = uctPickListBranches.Load_Renamed

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to load list of Branches", "Product Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


            If Task = gPMConstants.PMEComponentAction.PMAdd Then

                uctPickListCausations.AddAllItems()
                uctPickListClaimEvent.AddAllItems()
                uctPickListMTAEvent.AddAllItems()
                uctPickListBranches.DeleteAllItems()
            End If

            'If String.IsNullOrEmpty(m_sUniqueId) Then
            '    m_sUniqueId = GetUniqueID()
            'End If

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                uctDocumentLink1.ProductID = 0
                DocumentLinkClaim.ProductID = 0
                DocumentLinkClaimPayment.ProductID = 0
            Else
                uctDocumentLink1.ProductID = m_lProductID
                uctDocumentLink1.FuntionalArea = gPMConstants.PMDocLinkPolicy
                uctDocumentLink1.UniqueId = m_sUniqueId
                uctDocumentLink1.ScreenHierarchy = $"Product({txtCode.Text.Trim()})"

                DocumentLinkClaim.ProductID = m_lProductID
                DocumentLinkClaim.FuntionalArea = gPMConstants.PMDocLinkOpenMaintainClaims
                DocumentLinkClaim.UniqueId = m_sUniqueId
                DocumentLinkClaim.ScreenHierarchy = $"Product({txtCode.Text.Trim()})"

                DocumentLinkClaimPayment.ProductID = m_lProductID
                DocumentLinkClaimPayment.FuntionalArea = gPMConstants.PMDocLinkPayClaims
                DocumentLinkClaim.UniqueId = m_sUniqueId
                DocumentLinkClaim.ScreenHierarchy = $"Product({txtCode.Text.Trim()})"

            End If
            'Start- Written Status
            uctDocumentLink1.WrittenPolicyStatus = m_bWrittenPolicyStatus
            'End-  Written Status

            'Developer Guide No. 9
            m_lReturn = uctDocumentLink1.Initialise()
            m_lReturn = DocumentLinkClaim.Initialise()
            m_lReturn = DocumentLinkClaimPayment.Initialise()


            uctDocumentLink1.TabCaption(0) = "Document Link"
            DocumentLinkClaim.TabCaption(0) = "Open/Maintain Claim"
            DocumentLinkClaimPayment.TabCaption(0) = "Claim Payment"
            'Set Tab Caption according to there orders

            iTabNum = 1
            For iTabCount As Integer = 0 To SSTabHelper.GetTabCount(tabMainTab) - 1
                If SSTabHelper.GetTabVisible(tabMainTab, iTabCount) Then
                    'SSTabHelper.SetTabCaption(tabMainTab, iTabCount, "&" & iTabNum & Mid(SSTabHelper.GetTabCaption(tabMainTab, iTabCount), 3))
                    SSTabHelper.SetTabCaption(tabMainTab, iTabCount, iTabNum & Mid(SSTabHelper.GetTabCaption(tabMainTab, iTabCount), 2))
                    iTabNum += 1
                End If
            Next iTabCount

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            txtCode.Focus()


        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                Status = gPMConstants.PMEReturnCode.PMCancel
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If

            End If





            ' Terminate the general object.
            m_oGeneral.Dispose()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Terminate the business object


            ' Terminate the form control object.
            m_oFormFields.Dispose()
            ' Destroy the instance of the form control object
            ' from memory.
            m_oFormFields = Nothing


            ' Terminate the policy wording object (if used)
            If Not (m_oFindEmailDocs Is Nothing) Then


                m_oFindEmailDocs.Dispose()
                ' Destroy the instance of the policy wording object
                ' from memory.
                m_oFindEmailDocs = Nothing

            End If

            uctDocumentLink1.Dispose()
            DocumentLinkClaim.Dispose()
            DocumentLinkClaimPayment.Dispose()
            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown As Integer

        Dim iTab As Integer

        Const ACCtrlMask As Integer = 5

        Try

            ' Set the control key value.
            iCtrlDown = (Shift And ACCtrlMask) > 0
            iTab = SSTabHelper.GetSelectedIndex(tabMainTab)

            With tabMainTab
                ' Check the key pressed.
                Select Case KeyCode
                    Case Keys.PageUp
                        ' Page Up key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the first tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                        Else
                            ' Check we are not on the
                            ' first tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
                                ' Display the previous tab.
                                For lCount As Integer = iTab - 1 To 0 Step -1
                                    If SSTabHelper.GetTabVisible(tabMainTab, lCount) Then
                                        SSTabHelper.SetSelectedIndex(tabMainTab, lCount)
                                        Exit For
                                    End If
                                Next

                                '                        .Tab = .Tab - 1
                            End If
                        End If

                    Case Keys.PageDown
                        ' Page Down key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the last tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetTabCount(tabMainTab) - 1)
                        Else
                            ' Check we are not on the
                            ' last tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) < (SSTabHelper.GetTabCount(tabMainTab) - 1) Then
                                ' Display the next tab.
                                For lCount As Integer = iTab + 1 To SSTabHelper.GetTabCount(tabMainTab) - 1
                                    If SSTabHelper.GetTabVisible(tabMainTab, lCount) Then
                                        SSTabHelper.SetSelectedIndex(tabMainTab, lCount)
                                        Exit For
                                    End If
                                Next
                                '                          .Tab = .Tab + 1

                            End If
                        End If

                    Case Keys.Home
                        ' Home key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If

                    Case Keys.End
                        ' End key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlEnd, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If
                End Select
            End With
            'Developer Guide No 293
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D2 Then
                tabMainTab.SelectedIndex = 1
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D3 Then
                tabMainTab.SelectedIndex = 2
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D4 Then
                tabMainTab.SelectedIndex = 3
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D5 Then
                tabMainTab.SelectedIndex = 4
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D7 Then
                tabMainTab.SelectedIndex = 6
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D8 Then
                tabMainTab.SelectedIndex = 7
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D9 Then
                tabMainTab.SelectedIndex = 8
            End If

            If eventArgs.Alt And eventArgs.KeyCode = Keys.D7 Then

                If chkTrueMonthlyPolicy.Checked Then
                    tabMainTab.SelectedIndex = 7
                End If

            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D8 Then

                If chkTrueMonthlyPolicy.Checked Then
                    tabMainTab.SelectedIndex = 8
                End If

            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D6 And chkTrueMonthlyPolicy.Checked Then
                If tabMainTab.SelectedIndex <> 5 And tabMainTab.SelectedIndex <> 6 Then
                    tabMainTab.SelectedIndex = 5
                Else
                    If tabMainTab.SelectedIndex = 5 Then
                        tabMainTab.SelectedIndex = 6
                    Else
                        tabMainTab.SelectedIndex = 5
                    End If
                End If
            ElseIf eventArgs.Alt And eventArgs.KeyCode = Keys.D6 Then
                tabMainTab.SelectedIndex = 5
                If uctPickListMTAEvent.Visible Then
                    tabMainTab.SelectedIndex = 5
                End If
                If uctSIRSelectClauses.Visible Then
                    tabMainTab.SelectedIndex = 9
                End If
            End If
        Catch




            Exit Sub
        End Try


    End Sub


    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            m_lReturn = ValidateForm()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub



    Private Sub txtAllowedClaims_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAllowedClaims.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtAllowedClaims)
    End Sub

    Private Sub txtAllowedClaims_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAllowedClaims.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtAllowedClaims)
    End Sub

    Private Sub txtBlockNo_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBlockNo.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtBlockNo)
    End Sub

    Private Sub txtBlockNo_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBlockNo.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtBlockNo)
    End Sub

    Private Sub txtClaimYear_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtClaimYear.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtClaimYear)
    End Sub

    Private Sub txtClaimYear_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtClaimYear.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtClaimYear)
    End Sub

    'Start (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()

    Private Sub txtCNDefaultPeriod_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCNDefaultPeriod.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtCNDefaultPeriod)
        txtCNDefaultPeriod.MaxLength = 5
    End Sub

    Private Sub txtCNDefaultPeriod_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtCNDefaultPeriod.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        If KeyAscii = Strings.Asc("."c) Then

            KeyAscii = False
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    'Private Sub txtCNDefaultPeriod_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCNDefaultPeriod.Leave

    '    Dim bIsValid As Boolean = True

    '    If txtCNDefaultPeriod.Text.Trim() <> "" Then

    '        Dim dbNumericTemp As Double
    '        If Not Double.TryParse(txtCNDefaultPeriod.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

    '            MessageBox.Show("Warning: 'Cover Note default period' field accepts only integer values. Please try again", "Cover Note Group", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            bIsValid = False

    '        ElseIf (Conversion.Val(txtCNDefaultPeriod.Text) > 32767) Then

    '            ' Check that the value is within valid range
    '            MessageBox.Show("'Cover Note default period' field has a value that is not within valid range. Please re-enter.", "Cover Note Group", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            bIsValid = False

    '        End If
    '    End If

    '    If Not bIsValid Then
    '        If chkTrueMonthlyPolicy.CheckState = CheckState.Checked Then
    '            SSTabHelper.SetSelectedIndex(tabMainTab, 5)
    '        Else
    '            SSTabHelper.SetSelectedIndex(tabMainTab, 4)
    '        End If
    '        txtCNDefaultPeriod.Focus()
    '        Exit Sub
    '    End If
    '    ' m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtCNDefaultPeriod)
    'End Sub


    Private Sub txtCNMaxNo_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCNMaxNo.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtCNMaxNo)
        txtCNMaxNo.MaxLength = 5
    End Sub


    Private Sub txtCNMaxNo_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtCNMaxNo.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        If KeyAscii = Strings.Asc("."c) Then

            KeyAscii = False
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtCNMaxNo_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCNMaxNo.Leave
        ' m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtCNMaxNo)
        Dim bIsValid As Boolean = True

        If txtCNMaxNo.Text.Trim() <> "" Then
            Dim dbNumericTemp As Double
            If Not Double.TryParse(txtCNMaxNo.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                MessageBox.Show("Warning: 'Maximum No. of Cover Notes' field accepts only integer values. Please try again", "Cover Note Group", MessageBoxButtons.OK, MessageBoxIcon.Error)
                bIsValid = False

            ElseIf (Conversion.Val(txtCNMaxNo.Text) > 32767) Then

                ' Check that the value is within valid range
                MessageBox.Show("'Maximum No. of Cover Notes' field has a value that is not within valid range. Please re-enter.", "Cover Note Group", MessageBoxButtons.OK, MessageBoxIcon.Error)
                bIsValid = False

            End If
        End If

        If Not bIsValid Then
            If chkTrueMonthlyPolicy.CheckState = CheckState.Checked Then
                SSTabHelper.SetSelectedIndex(tabMainTab, 5)
            Else
                SSTabHelper.SetSelectedIndex(tabMainTab, 4)
            End If
            txtCNMaxNo.Focus()
            Exit Sub
        End If
    End Sub

    'End (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()

    Private Sub txtCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCode.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtCode)
    End Sub

    Private Sub txtCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCode.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtCode)
    End Sub

    Private Sub txtDescription_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDescription)
    End Sub

    Private Sub txtDescription_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDescription)
    End Sub

    Private Sub txtEffectiveDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEffectiveDate.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtEffectiveDate)
    End Sub

    Private Sub txtEffectiveDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEffectiveDate.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtEffectiveDate)
    End Sub

    Private Sub txtGracePeriod_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtGracePeriod.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtGracePeriod)
    End Sub

    Private Sub txtGracePeriod_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGracePeriod.KeyPress
        If (Microsoft.VisualBasic.Asc(e.KeyChar) < 48) Or (Microsoft.VisualBasic.Asc(e.KeyChar) > 57) Then
            e.Handled = True
        End If
        If (Microsoft.VisualBasic.Asc(e.KeyChar) = 8) Then
            e.Handled = False
        End If
    End Sub

    Private Sub txtGracePeriod_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtGracePeriod.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtGracePeriod)
    End Sub

    Private Sub txtLargeLossAdviceValue_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLargeLossAdviceValue.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtLargeLossAdviceValue)
    End Sub

    Private Sub txtLargeLossAdviceValue_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLargeLossAdviceValue.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtLargeLossAdviceValue)
    End Sub

    Private Sub txtAckTaskAllowedTime_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAckTaskAllowedTime.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtAckTaskAllowedTime)
    End Sub

    Private Sub txtAckTaskAllowedTime_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAckTaskAllowedTime.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtAckTaskAllowedTime)
    End Sub

    Private Sub txtPreReportAllowedTime_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPreReportAllowedTime.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtPreReportAllowedTime)
    End Sub

    Private Sub txtPreReportAllowedTime_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPreReportAllowedTime.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtPreReportAllowedTime)
    End Sub
    Private Sub txtRenewalPeriod_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRenewalPeriod.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtRenewalPeriod)
    End Sub

    Private Sub txtRenewalPeriod_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRenewalPeriod.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtRenewalPeriod)
    End Sub

    Private Sub txtReportPointer_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtReportPointer.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtReportPointer)
    End Sub

    Private Sub txtReportPointer_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtReportPointer.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtReportPointer)
    End Sub

    Private Sub txtRIPointer_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRIPointer.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtRIPointer)
    End Sub

    Private Sub txtRIPointer_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRIPointer.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtRIPointer)
    End Sub

    Private Sub txtSchemeAgencyRef_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSchemeAgencyRef.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtSchemeAgencyRef)
    End Sub

    Private Sub txtSchemeAgencyRef_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSchemeAgencyRef.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtSchemeAgencyRef)
    End Sub

    Private Sub txtSingleClaimValue_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSingleClaimValue.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtSingleClaimValue)
    End Sub

    Private Sub txtSingleClaimValue_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSingleClaimValue.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtSingleClaimValue)
    End Sub

    Private Sub txtTotalClaimsValue_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTotalClaimsValue.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtTotalClaimsValue)
    End Sub

    Private Sub txtTotalClaimsValue_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTotalClaimsValue.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtTotalClaimsValue)
    End Sub

    Private Sub chkHideSummaryAtRenewalAcceptance_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkHideSummaryAtRenewalAcceptance.CheckStateChanged
        If chkHideSummaryAtRenewalAcceptance.CheckState = CheckState.Checked Then
            chkChangePolicyNumberAtRenewal.CheckState = CheckState.Unchecked
            chkChangePolicyNumberAtRenewal.Enabled = False
            'Start - Renuka - (WPR87 Paralleling)
            chkChangePolicyNumberAtRenewalAutomatically.CheckState = CheckState.Unchecked
            chkChangePolicyNumberAtRenewalAutomatically.Enabled = False
            'End - Renuka - (WPR87 Paralleling)
        Else
            chkChangePolicyNumberAtRenewal.Enabled = True
            'Start - Renuka - (WPR87 Paralleling)
            chkChangePolicyNumberAtRenewalAutomatically.Enabled = False
            'End - Renuka - (WPR87 Paralleling)
        End If

    End Sub

    Private Sub chkChangePolicyNumberAtRenewal_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkChangePolicyNumberAtRenewal.CheckStateChanged
        If chkChangePolicyNumberAtRenewal.CheckState = CheckState.Checked Then
            chkHideSummaryAtRenewalAcceptance.CheckState = CheckState.Unchecked
            chkHideSummaryAtRenewalAcceptance.Enabled = False
            SSTabHelper.SetTabVisible(tabMainTab, 6, False)
            'Start - Renuka - (WPR87 Paralleling)
            chkChangePolicyNumberAtRenewalAutomatically.Enabled = True
            'End - Renuka - (WPR87 Paralleling)
        Else
            chkHideSummaryAtRenewalAcceptance.Enabled = True
            If chkTradeRnlOnline.CheckState = CheckState.Checked Then
                SSTabHelper.SetTabVisible(tabMainTab, 6, True)
            End If
            'Start - Renuka - (WPR87 Paralleling)
            chkChangePolicyNumberAtRenewalAutomatically.CheckState = CheckState.Unchecked
            chkChangePolicyNumberAtRenewalAutomatically.Enabled = False
            'End - Renuka - (WPR87 Paralleling)
        End If
    End Sub

    ' ***************************************************************** '
    ' Name: SetupTrueMonthlyPolicy
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function SetupTrueMonthlyPolicy() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupTrueMonthlyPolicy"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lNoOfPolicies As Integer
        Dim vNoOfPoliciesOnProduct As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'If m_bTrueMonthlyPoliciesEnabled Then (TMP)

            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                ' determine if there are any policies on this product
                ' if there are policies on this product then the true monthly policy
                ' indicator cannot be altered

                lReturn = m_oBusiness.GetNoOfPoliciesOnProduct(v_lProductID:=m_lProductID, r_vResults:=vNoOfPoliciesOnProduct)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetNoOfPoliciesOnProduct", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' get the number of policies on the product
                If Information.IsArray(vNoOfPoliciesOnProduct) Then

                    lNoOfPolicies = CInt(vNoOfPoliciesOnProduct(0, 0))
                Else
                    lNoOfPolicies = 0
                End If
            ElseIf m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                lNoOfPolicies = 0
            End If

            ' if true monthly policy value is check then
            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                chkTrueMonthlyPolicy.Enabled = False
            End If
            If chkTrueMonthlyPolicy.CheckState = CheckState.Checked Then
                SSTabHelper.SetTabVisible(tabMainTab, 3, True)
                lblAnniversaryRenewalWeeks.Enabled = True
                txtAnniversaryRenewalWeeks.Enabled = True
                chkHideSummaryAtRenewalAcceptance.CheckState = CheckState.Checked
                chkHideSummaryAtRenewalAcceptance.Enabled = False
                If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                    chkNBProRata.CheckState = CheckState.Checked
                    chkMTAProRata.CheckState = CheckState.Checked
                End If
            Else
                SSTabHelper.SetTabVisible(tabMainTab, 3, False)
                lblAnniversaryRenewalWeeks.Enabled = False
                txtAnniversaryRenewalWeeks.Enabled = False
                txtAnniversaryRenewalWeeks.Text = ""
                chkHideSummaryAtRenewalAcceptance.Enabled = True
                If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                    chkNBProRata.CheckState = CheckState.Unchecked
                    chkMTAProRata.CheckState = CheckState.Unchecked
                    chkHideSummaryAtRenewalAcceptance.CheckState = CheckState.Unchecked
                End If
            End If

            ' if there are policies already on the product
            ' then the true monthly policy indicator cannot be changed
            If lNoOfPolicies <> 0 Then
                chkTrueMonthlyPolicy.Enabled = False
            End If
            If chkTrueMonthlyPolicy.CheckState <> CheckState.Checked Then
                SSTabHelper.SetTabCaption(tabMainTab, 4, "4" & Mid(SSTabHelper.GetTabCaption(tabMainTab, 4), 2))
                SSTabHelper.SetTabCaption(tabMainTab, 5, "5" & Mid(SSTabHelper.GetTabCaption(tabMainTab, 5), 2))
                SSTabHelper.SetTabCaption(tabMainTab, 6, "6" & Mid(SSTabHelper.GetTabCaption(tabMainTab, 6), 2))
            Else
                SSTabHelper.SetTabCaption(tabMainTab, 4, "5" & Mid(SSTabHelper.GetTabCaption(tabMainTab, 4), 2))
                SSTabHelper.SetTabCaption(tabMainTab, 5, "6" & Mid(SSTabHelper.GetTabCaption(tabMainTab, 5), 2))
                SSTabHelper.SetTabCaption(tabMainTab, 6, "7" & Mid(SSTabHelper.GetTabCaption(tabMainTab, 6), 2))
            End If
            'Else(TMP)
            'lblAnniversaryRenewalWeeks.Visible = False
            'txtAnniversaryRenewalWeeks.Visible = False
            'chkTrueMonthlyPolicy.Visible = False
            'End If

            If chkTrueMonthlyPolicy.CheckState = CheckState.Checked Then
                fraTrueMonthlyPolFreqOfComm.Enabled = True
                optRenewalProcessRun.Checked = True
            Else
                fraTrueMonthlyPolFreqOfComm.Enabled = False
                optRenewalProcessRun.Checked = False
                optAnniversaryDate.Checked = False
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
    ' Name: SetupClaimTransactionSuppression
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 28-10-2005 : Suppress Claim Transactions
    ' ***************************************************************** '
    Private Function SetupClaimTransactionSuppression() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupClaimTransactionSuppression"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' recoveries and payments can only be suppressed
            ' if the reserves are already suppressed
            If chkReserve.CheckState = CheckState.Checked Then
                chkRecoveries.Enabled = True
                chkPayments.Enabled = True
            Else
                ' if the reserves are not already suppressed then
                ' disable recoveries and payments and reset so they
                ' are not suppressed
                chkRecoveries.CheckState = CheckState.Unchecked
                chkRecoveries.Enabled = False

                chkPayments.CheckState = CheckState.Unchecked
                chkPayments.Enabled = False
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
    ' Name: GenerateClaimsTransactionSuppressionEvent
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function GenerateClaimsTransactionSuppressionEvent() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GenerateClaimsTransactionSuppressionEvent"

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim bRecoverySuppressedChanged, bReserveSuppressedChanged, bPaymentSuppressedChanged As Boolean

        Dim sRecoverySuppressionStatus, sReserveSuppressionStatus, sPaymentSuppressionStatus As String

        Dim sPaymentSuppression, sReserveSuppression, sRecoverySuppression As String

        Dim sUser, sProduct, sSuppressionMessage As String

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' if the suppression indicator for payments has changed
            If gPMFunctions.ToSafeLong(m_vOrigSuppressClaimTransactionPayment, 0) <> gPMFunctions.ToSafeLong(m_vSuppressClaimTransactionPayment, 0) Then

                ' build event log description
                If chkPayments.CheckState = CheckState.Checked Then
                    sPaymentSuppressionStatus = "on"
                Else
                    sPaymentSuppressionStatus = "off"
                End If

                sPaymentSuppression = "Payments Suppression has been switched " & sPaymentSuppressionStatus & "."

                bPaymentSuppressedChanged = True

            End If

            ' if the suppression indicator for recoveries has changed
            If gPMFunctions.ToSafeLong(m_vOrigSuppressClaimTransactionRecovery, 0) <> gPMFunctions.ToSafeLong(m_vSuppressClaimTransactionRecovery, 0) Then

                ' build event log description
                If chkRecoveries.CheckState = CheckState.Checked Then
                    sRecoverySuppressionStatus = "on"
                Else
                    sRecoverySuppressionStatus = "off"
                End If

                sRecoverySuppression = "Recovery Suppression has been switched " & sRecoverySuppressionStatus & "."

                bRecoverySuppressedChanged = True
            End If

            ' if the suppression indicator for reserves has changed
            If gPMFunctions.ToSafeLong(m_vOrigSuppressClaimTransactionReserve, 0) <> gPMFunctions.ToSafeLong(m_vSuppressClaimTransactionReserve, 0) Then

                ' build event log description
                If chkRecoveries.CheckState = CheckState.Checked Then
                    sReserveSuppressionStatus = "on"
                Else
                    sReserveSuppressionStatus = "off"
                End If

                sReserveSuppression = "Reserve Suppression has been switched " & sReserveSuppressionStatus & "."

                bReserveSuppressedChanged = True
            End If

            sUser = "User: " & g_oObjectManager.UserName & " made the following changes to claims transaction suppression"
            sProduct = " for Product Id :-" & m_lProductID & " Product Code :-" & m_sCode & "."

            ' generate suppression event description
            If bPaymentSuppressedChanged Then
                sSuppressionMessage = sSuppressionMessage & sPaymentSuppression
            End If

            If bRecoverySuppressedChanged Then
                sSuppressionMessage = sSuppressionMessage & sRecoverySuppression
            End If

            If bReserveSuppressedChanged Then
                sSuppressionMessage = sSuppressionMessage & sReserveSuppression
            End If

            ' if the have been changes made in this session then
            If sSuppressionMessage <> "" Then

                sSuppressionMessage = sUser & sProduct & sSuppressionMessage


                lReturn = m_oBusiness.CreateTransSuppressionNotificationTask(v_sDescription:=sSuppressionMessage)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "CreateTransSuppressionNotificationTask Failed", gPMConstants.PMELogLevel.PMLogError)
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


    Private Function CheckPaymentMethod() As Object
        If chkTrueMonthlyPolicy.CheckState = CheckState.Checked Then
            If i_Options = 1 Then
                If chkInvoice.CheckState = CheckState.Unchecked And OptInvoice.Checked Then
                    chkInvoice.CheckState = CheckState.Checked
                    MessageBox.Show("You cannot uncheck the MakeLive Payment Option which is selected as Default Payment Method", "Product", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Function
                End If
            ElseIf i_Options = 2 Then
                If chkInstalments.CheckState = CheckState.Unchecked And OptInstalments.Checked Then
                    chkInstalments.CheckState = CheckState.Checked
                    MessageBox.Show("You cannot uncheck the MakeLive Payment Option which is selected as Default Payment Method", "Product", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Function
                End If
            End If
        End If
    End Function

    Private Sub chkInstalments_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkInstalments.CheckStateChanged
        i_Options = 2
        CheckPaymentMethod()
    End Sub
    Private Sub chkInvoice_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkInvoice.CheckStateChanged
        i_Options = 1
        CheckPaymentMethod()
    End Sub

    Private Sub cboAgentReviewAttachment_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboAgentReviewAttachment.SelectionChangeCommitted
        If cboAgentReviewAttachment.SelectedIndex <> -1 Then

            m_vAgentRenewalManReviewReportId = VB6.GetItemData(cboAgentReviewAttachment, cboAgentReviewAttachment.SelectedIndex)
        End If
    End Sub

    Private Sub cboAgentInviteAttachment_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboAgentInviteAttachment.SelectionChangeCommitted
        If cboAgentInviteAttachment.SelectedIndex <> -1 Then

            m_vAgentRenewalInviteReportId = VB6.GetItemData(cboAgentInviteAttachment, cboAgentInviteAttachment.SelectedIndex)
        End If
    End Sub

    Private Sub cboAgentUpdateAttachment_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboAgentUpdateAttachment.SelectionChangeCommitted
        If cboAgentUpdateAttachment.SelectedIndex <> -1 Then

            m_vAgentRenewalUpdateReportId = VB6.GetItemData(cboAgentUpdateAttachment, cboAgentUpdateAttachment.SelectedIndex)
        End If
    End Sub

    Private Sub chkEnabledRenSelection_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkEnabledRenSelection.CheckStateChanged
        Dim bEnabled As Boolean

        If chkEnabledRenSelection.CheckState = CheckState.Checked Then
            bEnabled = True
        End If

        For iCnt As Integer = 0 To 5
            If Not bEnabled Then
                txtEmailComm(iCnt).Tag = CStr(0)
                txtEmailComm(iCnt).Text = ""
            End If
            lblEmailComm(iCnt).Enabled = bEnabled
            txtEmailComm(iCnt).Enabled = bEnabled
            cmdEmailCommSelect(iCnt).Enabled = bEnabled
            cmdEmailCommDeSelect(iCnt).Enabled = bEnabled
        Next iCnt
    End Sub

    Private Sub chkEnabledRenInvite_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkEnabledRenInvite.CheckStateChanged
        Dim bEnabled As Boolean

        If chkEnabledRenInvite.CheckState = CheckState.Checked Then
            bEnabled = True
        End If

        For iCnt As Integer = 6 To 11
            If Not bEnabled Then
                txtEmailComm(iCnt).Tag = CStr(0)
                txtEmailComm(iCnt).Text = ""
            End If
            lblEmailComm(iCnt).Enabled = bEnabled
            txtEmailComm(iCnt).Enabled = bEnabled
            cmdEmailCommSelect(iCnt).Enabled = bEnabled
            cmdEmailCommDeSelect(iCnt).Enabled = bEnabled
        Next iCnt
    End Sub

    Private Sub chkEnabledRenUpdate_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkEnabledRenUpdate.CheckStateChanged
        Dim bEnabled As Boolean

        If chkEnabledRenUpdate.CheckState = CheckState.Checked Then
            bEnabled = True
        End If


        For iCnt As Integer = 12 To 17

            If Not bEnabled Then
                txtEmailComm(iCnt).Tag = CStr(0)
                txtEmailComm(iCnt).Text = ""
            End If

            lblEmailComm(iCnt).Enabled = bEnabled
            txtEmailComm(iCnt).Enabled = bEnabled
            cmdEmailCommSelect(iCnt).Enabled = bEnabled
            cmdEmailCommDeSelect(iCnt).Enabled = bEnabled
        Next iCnt

    End Sub

    Private Sub chkAgentRenSelection_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkAgentRenSelection.CheckStateChanged
        Dim bEnabled As Boolean

        If chkAgentRenSelection.CheckState = CheckState.Checked Then
            bEnabled = True
        Else
            cboAgentReviewAttachment.SelectedIndex = -1
            cboAgentReviewAttachment.Text = ""
            txtEmailComm(18).Tag = CStr(0)
            txtEmailComm(18).Text = ""
        End If

        lblEmailComm(18).Enabled = bEnabled
        lblEmailComm(21).Enabled = bEnabled
        txtEmailComm(18).Enabled = bEnabled
        cmdEmailCommSelect(18).Enabled = bEnabled
        cmdEmailCommDeSelect(18).Enabled = bEnabled
        cboAgentReviewAttachment.Enabled = bEnabled
    End Sub

    Private Sub chkAgentRenInvite_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkAgentRenInvite.CheckStateChanged
        Dim bEnabled As Boolean

        If chkAgentRenInvite.CheckState = CheckState.Checked Then
            bEnabled = True
        Else
            cboAgentInviteAttachment.SelectedIndex = -1
            cboAgentInviteAttachment.Text = ""
            txtEmailComm(19).Tag = CStr(0)
            txtEmailComm(19).Text = ""
        End If

        lblEmailComm(19).Enabled = bEnabled
        lblEmailComm(22).Enabled = bEnabled
        txtEmailComm(19).Enabled = bEnabled
        cmdEmailCommSelect(19).Enabled = bEnabled
        cmdEmailCommDeSelect(19).Enabled = bEnabled
        cboAgentInviteAttachment.Enabled = bEnabled

    End Sub

    Private Sub chkAgentRenUpdate_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkAgentRenUpdate.CheckStateChanged
        Dim bEnabled As Boolean

        If chkAgentRenUpdate.CheckState = CheckState.Checked Then
            bEnabled = True
        Else
            cboAgentUpdateAttachment.SelectedIndex = -1
            cboAgentUpdateAttachment.Text = ""
            txtEmailComm(20).Tag = CStr(0)
            txtEmailComm(20).Text = ""
        End If

        lblEmailComm(20).Enabled = bEnabled
        lblEmailComm(23).Enabled = bEnabled
        txtEmailComm(20).Enabled = bEnabled
        cmdEmailCommSelect(20).Enabled = bEnabled
        cmdEmailCommDeSelect(20).Enabled = bEnabled
        cboAgentUpdateAttachment.Enabled = bEnabled
    End Sub

    Private Function SetRenewalOnlineOptions() As Integer
        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        If m_iTradeRNLOnline = 0 Or gPMFunctions.ToSafeBoolean(m_vChangePolicyNumberAtRenewal) Then
            SSTabHelper.SetTabVisible(tabMainTab, 6, False)
        End If

        If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
            If chkTrueMonthlyPolicy.CheckState = CheckState.Checked Then
                fraTrueMonthlyPolFreqOfComm.Enabled = True
                optRenewalProcessRun.Checked = True
            End If
        End If


        If m_iTask <> gPMConstants.PMEComponentAction.PMAdd Then
            chkRenewable.CheckState = m_iIsRenewable
        Else
            chkRenewable.CheckState = CheckState.Checked
        End If

        'Check Renewal Selection
        Dim bEnabled As Boolean = False
        If m_iTask <> gPMConstants.PMEComponentAction.PMAdd Then
            bEnabled = m_iIsRenewalSelectionEnabled
        End If

        For iCnt As Integer = 0 To 5
            lblEmailComm(iCnt).Enabled = bEnabled
            txtEmailComm(iCnt).Enabled = bEnabled
            cmdEmailCommSelect(iCnt).Enabled = bEnabled
            cmdEmailCommDeSelect(iCnt).Enabled = bEnabled
        Next iCnt

        'Check Renewal Invite
        bEnabled = False
        If m_iTask <> gPMConstants.PMEComponentAction.PMAdd Then
            bEnabled = m_iIsRenewalInviteEnabled
        End If

        For iCnt As Integer = 6 To 11
            lblEmailComm(iCnt).Enabled = bEnabled
            txtEmailComm(iCnt).Enabled = bEnabled
            cmdEmailCommSelect(iCnt).Enabled = bEnabled
            cmdEmailCommDeSelect(iCnt).Enabled = bEnabled
        Next iCnt

        'Check Renewal Update
        bEnabled = False
        If m_iTask <> gPMConstants.PMEComponentAction.PMAdd Then
            bEnabled = m_iIsRenewalUpdateEnabled
        End If

        For iCnt As Integer = 12 To 17
            lblEmailComm(iCnt).Enabled = bEnabled
            txtEmailComm(iCnt).Enabled = bEnabled
            cmdEmailCommSelect(iCnt).Enabled = bEnabled
            cmdEmailCommDeSelect(iCnt).Enabled = bEnabled
        Next iCnt

        bEnabled = False
        If m_iTask <> gPMConstants.PMEComponentAction.PMAdd Then
            bEnabled = m_iIsAgentRenewalSelectionEnabled
        End If

        lblEmailComm(18).Enabled = bEnabled
        txtEmailComm(18).Enabled = bEnabled
        cmdEmailCommSelect(18).Enabled = bEnabled
        cmdEmailCommDeSelect(18).Enabled = bEnabled
        cboAgentReviewAttachment.Enabled = bEnabled
        lblEmailComm(21).Enabled = bEnabled

        bEnabled = False
        If m_iTask <> gPMConstants.PMEComponentAction.PMAdd Then
            bEnabled = m_iIsAgentRenewalInviteEnabled
        End If

        lblEmailComm(19).Enabled = bEnabled
        txtEmailComm(19).Enabled = bEnabled
        cmdEmailCommSelect(19).Enabled = bEnabled
        cmdEmailCommDeSelect(19).Enabled = bEnabled
        cboAgentInviteAttachment.Enabled = bEnabled
        lblEmailComm(22).Enabled = bEnabled

        bEnabled = False
        If m_iTask <> gPMConstants.PMEComponentAction.PMAdd Then
            bEnabled = m_iIsAgentRenewalUpdateEnabled
        End If

        lblEmailComm(20).Enabled = bEnabled
        txtEmailComm(20).Enabled = bEnabled
        cmdEmailCommSelect(20).Enabled = bEnabled
        cmdEmailCommDeSelect(20).Enabled = bEnabled
        cboAgentUpdateAttachment.Enabled = bEnabled
        lblEmailComm(23).Enabled = bEnabled

        Return result
    End Function

    Private Sub cmdEmailCommSelect_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdEmailCommSelect_20.Click, _cmdEmailCommSelect_19.Click, _cmdEmailCommSelect_18.Click, _cmdEmailCommSelect_12.Click, _cmdEmailCommSelect_13.Click, _cmdEmailCommSelect_14.Click, _cmdEmailCommSelect_15.Click, _cmdEmailCommSelect_16.Click, _cmdEmailCommSelect_17.Click, _cmdEmailCommSelect_6.Click, _cmdEmailCommSelect_7.Click, _cmdEmailCommSelect_8.Click, _cmdEmailCommSelect_9.Click, _cmdEmailCommSelect_10.Click, _cmdEmailCommSelect_11.Click, _cmdEmailCommSelect_5.Click, _cmdEmailCommSelect_4.Click, _cmdEmailCommSelect_3.Click, _cmdEmailCommSelect_2.Click, _cmdEmailCommSelect_1.Click, _cmdEmailCommSelect_0.Click
        Dim Index As Integer = Array.IndexOf(cmdEmailCommSelect, eventSender)
        Dim sDocumentCode As String = ""
        Dim lDocumentTemplateId As Integer


        Select Case Index
            Case 1, 3, 5, 7, 9, 11, 13, 15, 17
                m_lReturn = GetDocument(r_sDocumentCode:=sDocumentCode, r_lDocumentTemplateID:=lDocumentTemplateId, v_iDocumentTypeId:=0)
            Case Else
                m_lReturn = GetDocument(r_sDocumentCode:=sDocumentCode, r_lDocumentTemplateID:=lDocumentTemplateId, v_iDocumentTypeId:=8)
        End Select

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            txtEmailComm(Index).Text = sDocumentCode
            txtEmailComm(Index).Tag = CStr(lDocumentTemplateId)
        End If

    End Sub
    Private Sub cmdEmailCommDeSelect_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdEmailCommDeSelect_20.Click, _cmdEmailCommDeSelect_19.Click, _cmdEmailCommDeSelect_18.Click, _cmdEmailCommDeSelect_12.Click, _cmdEmailCommDeSelect_13.Click, _cmdEmailCommDeSelect_14.Click, _cmdEmailCommDeSelect_15.Click, _cmdEmailCommDeSelect_16.Click, _cmdEmailCommDeSelect_17.Click, _cmdEmailCommDeSelect_6.Click, _cmdEmailCommDeSelect_7.Click, _cmdEmailCommDeSelect_8.Click, _cmdEmailCommDeSelect_9.Click, _cmdEmailCommDeSelect_10.Click, _cmdEmailCommDeSelect_11.Click, _cmdEmailCommDeSelect_5.Click, _cmdEmailCommDeSelect_4.Click, _cmdEmailCommDeSelect_3.Click, _cmdEmailCommDeSelect_2.Click, _cmdEmailCommDeSelect_1.Click, _cmdEmailCommDeSelect_0.Click
        Dim Index As Integer = Array.IndexOf(cmdEmailCommDeSelect, eventSender)
        txtEmailComm(Index).Text = ""
        txtEmailComm(Index).Tag = CStr(0)
    End Sub
    Private Function GetDocument(ByRef r_sDocumentCode As String, ByRef r_lDocumentTemplateID As Integer, ByVal v_iDocumentTypeId As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the document template

            m_lReturn = GetDocumentTemplate(r_lDocumentTemplateID:=r_lDocumentTemplateID, r_sDocumentCode:=r_sDocumentCode, v_iDocumentTypeId:=CInt(v_iDocumentTypeId))
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMCancel) Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocumentTemplate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocument")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetDocumentTemplate
    '
    ' Description: Creates an instance of find document template and gets the properties
    '
    ' ***************************************************************** '
    Private Function GetDocumentTemplate(ByRef r_lDocumentTemplateID As Integer, ByRef r_sDocumentCode As String, ByVal v_iDocumentTypeId As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'Create Email Docs object if not already done so
            If m_oFindEmailDocs Is Nothing Then

                ' Get an instance of the contact interface object via
                ' the public object manager.
                Dim temp_m_oFindEmailDocs As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oFindEmailDocs, sClassName:="iPMBFindDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oFindEmailDocs = temp_m_oFindEmailDocs

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Find Email Docs object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddPolicyWording_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result

                End If

            End If


            m_lReturn = m_oFindEmailDocs.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'Run it in Merge mode to hide some buttons

            m_oFindEmailDocs.Mode = 1
            'Email Docs...

            m_oFindEmailDocs.DocumentTypeId = v_iDocumentTypeId


            m_oFindEmailDocs.ProductID = 0


            m_lReturn = m_oFindEmailDocs.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Exit out if it was cancelled

            If m_oFindEmailDocs.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Return gPMConstants.PMEReturnCode.PMCancel
            End If

            ' Get it's properties
            With m_oFindEmailDocs

                r_lDocumentTemplateID = .DocumentTemplateId

                r_sDocumentCode = .DocumentCode
            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocumentTemplate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocumentTemplate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function SetupClaimWorkflow(ByRef v_lProductID As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "SetupClaimWorkflow"
        Dim vResult(,) As Object

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            m_lReturn = m_oBusiness.GetClaimWorkflow(v_lProductID:=v_lProductID, r_vResults:=vResult)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Information.IsArray(vResult) Then
                Exit Function
            End If

            For cnt As Integer = vResult.GetLowerBound(1) To vResult.GetUpperBound(1)
                Select Case vResult(gPMConstants.EClaimWorkflowId.EClaim_Process_Type_Id, cnt)
                    Case gPMConstants.PMWorkflowOpenClaim
                        chkOpenClaimWorkflow(2).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.ECheck_Unpaid_Status, cnt))
                        chkOpenClaimWorkflow(4).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.EReinsurance_Recovery, cnt))
                        chkOpenClaimWorkflow(5).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.ESalvage_Recovery, cnt))
                        chkOpenClaimWorkflow(6).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.EThird_Party_Recovery, cnt))
                        chkOpenClaimWorkflow(7).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.EExternal_Claim_Handling, cnt))
                        chkOpenClaimWorkflow(9).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.EClaim_Notification_Doc_Message, cnt))
                        chkOpenClaimWorkflow(10).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.EGenerate_Claim_Notification_Doc, cnt))
                        chkOpenClaimWorkflow(11).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.EClaim_Payment_Process, cnt))
                        chkOpenClaimWorkflow(13).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.EFast_Track_Claims, cnt))
                        chkOpenClaimWorkflow(16).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.EReinsurance_Payment, cnt))
                        chkOpenClaimWorkflow(17).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.EDescription_for_Change_in_Payment, cnt))
                        chkOpenClaimWorkflow(19).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.ECash_Payment_process, cnt))
                        chkOpenClaimWorkflow(21).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.EClaim_Payment_Doc_Message, cnt))
                        chkOpenClaimWorkflow(22).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.EGenerate_Claim_Payment_doc, cnt))
                        chkOpenClaimWorkflow(23).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.EMake_Further_Payments, cnt))

                    Case gPMConstants.PMWorkflowMaintainClaim
                        chkMaintainClaimWorkflow(4).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.EReinsurance_Recovery, cnt))
                        chkMaintainClaimWorkflow(5).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.ESalvage_Recovery, cnt))
                        chkMaintainClaimWorkflow(6).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.EThird_Party_Recovery, cnt))
                        chkMaintainClaimWorkflow(7).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.EDescription_for_Change_in_Reserve, cnt))
                        chkMaintainClaimWorkflow(10).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.EClaim_Notification_Doc_Message, cnt))
                        chkMaintainClaimWorkflow(11).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.EGenerate_Claim_Notification_Doc, cnt))
                        chkMaintainClaimWorkflow(12).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.EClaim_Payment_Process, cnt))
                        chkMaintainClaimWorkflow(14).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.EFast_Track_Claims, cnt))
                        chkMaintainClaimWorkflow(17).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.EReinsurance_Payment, cnt))
                        chkMaintainClaimWorkflow(18).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.EDescription_for_Change_in_Payment, cnt))
                        chkMaintainClaimWorkflow(20).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.ECash_Payment_process, cnt))
                        chkMaintainClaimWorkflow(22).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.EClaim_Payment_Doc_Message, cnt))
                        chkMaintainClaimWorkflow(23).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.EGenerate_Claim_Payment_doc, cnt))
                        chkMaintainClaimWorkflow(24).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.EMake_Further_Payments, cnt))

                    Case gPMConstants.PMWorkflowPayClaim
                        chkPaymentClaimWorkflow(2).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.ECheck_Unpaid_Status, cnt))
                        chkPaymentClaimWorkflow(3).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.ECheck_Deferred_Reinsurance, cnt))
                        chkPaymentClaimWorkflow(4).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.EFast_Track_Claims, cnt))
                        chkPaymentClaimWorkflow(7).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.EReinsurance_Payment, cnt))
                        chkPaymentClaimWorkflow(8).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.EDescription_for_Change_in_Payment, cnt))
                        chkPaymentClaimWorkflow(10).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.ECash_Payment_process, cnt))
                        chkPaymentClaimWorkflow(12).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.EClaim_Payment_Doc_Message, cnt))
                        chkPaymentClaimWorkflow(13).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.EGenerate_Claim_Payment_doc, cnt))
                        chkPaymentClaimWorkflow(14).CheckState = gPMFunctions.ToSafeInteger(vResult(gPMConstants.EClaimWorkflowId.EMake_Further_Payments, cnt))

                End Select
            Next cnt

            Return result

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            Return result

        Finally

        End Try

    End Function

    Private Function UpdateClaimWorkflow(ByRef v_lProductID As Object, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "UpdateClaimWorkflow"
        Dim vWorkflowArray As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim vWorkflowArray(17)

            'Retreive Open claim workflow
            'N/a ones will be set to null
            If Not String.IsNullOrEmpty(sScreenHierarchy) Then
                sScreenHierarchy = sScreenHierarchy & $"/Claims Workflow"
            End If

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EClaim_Process_Type_Id) = gPMConstants.PMWorkflowOpenClaim

            vWorkflowArray(gPMConstants.EClaimWorkflowId.ECheck_Unpaid_Status) = chkOpenClaimWorkflow(2).CheckState

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EReinsurance_Recovery) = chkOpenClaimWorkflow(4).CheckState

            vWorkflowArray(gPMConstants.EClaimWorkflowId.ESalvage_Recovery) = chkOpenClaimWorkflow(5).CheckState

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EThird_Party_Recovery) = chkOpenClaimWorkflow(6).CheckState

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EExternal_Claim_Handling) = chkOpenClaimWorkflow(7).CheckState


            vWorkflowArray(gPMConstants.EClaimWorkflowId.EDescription_for_Change_in_Reserve) = DBNull.Value

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EClaim_Notification_Doc_Message) = chkOpenClaimWorkflow(9).CheckState

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EGenerate_Claim_Notification_Doc) = chkOpenClaimWorkflow(10).CheckState

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EClaim_Payment_Process) = chkOpenClaimWorkflow(11).CheckState


            vWorkflowArray(gPMConstants.EClaimWorkflowId.ECheck_Deferred_Reinsurance) = DBNull.Value

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EFast_Track_Claims) = chkOpenClaimWorkflow(13).CheckState

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EReinsurance_Payment) = chkOpenClaimWorkflow(16).CheckState

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EDescription_for_Change_in_Payment) = chkOpenClaimWorkflow(17).CheckState

            vWorkflowArray(gPMConstants.EClaimWorkflowId.ECash_Payment_process) = chkOpenClaimWorkflow(19).CheckState

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EClaim_Payment_Doc_Message) = chkOpenClaimWorkflow(21).CheckState

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EGenerate_Claim_Payment_doc) = chkOpenClaimWorkflow(22).CheckState

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EMake_Further_Payments) = chkOpenClaimWorkflow(23).CheckState


            m_lReturn = m_oBusiness.UpdateClaimWorkflow(v_iTask:=m_iTask, v_lProductID:=v_lProductID, vWorkflowArray:=vWorkflowArray, sUniqueId:=sUniqueId, sScreenHierarchy:=sScreenHierarchy & "/OpenClaim")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                gPMFunctions.RaiseError(kMethodName, "Update of open claim workflow Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If


            'Retreive Maintain claim workflow
            'N/a ones will be set to null

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EClaim_Process_Type_Id) = gPMConstants.PMWorkflowMaintainClaim


            vWorkflowArray(gPMConstants.EClaimWorkflowId.ECheck_Unpaid_Status) = DBNull.Value

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EReinsurance_Recovery) = chkMaintainClaimWorkflow(4).CheckState

            vWorkflowArray(gPMConstants.EClaimWorkflowId.ESalvage_Recovery) = chkMaintainClaimWorkflow(5).CheckState

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EThird_Party_Recovery) = chkMaintainClaimWorkflow(6).CheckState


            vWorkflowArray(gPMConstants.EClaimWorkflowId.EExternal_Claim_Handling) = DBNull.Value

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EDescription_for_Change_in_Reserve) = chkMaintainClaimWorkflow(7).CheckState

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EClaim_Notification_Doc_Message) = chkMaintainClaimWorkflow(10).CheckState

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EGenerate_Claim_Notification_Doc) = chkMaintainClaimWorkflow(11).CheckState

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EClaim_Payment_Process) = chkMaintainClaimWorkflow(12).CheckState


            vWorkflowArray(gPMConstants.EClaimWorkflowId.ECheck_Deferred_Reinsurance) = DBNull.Value

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EFast_Track_Claims) = chkMaintainClaimWorkflow(14).CheckState

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EReinsurance_Payment) = chkMaintainClaimWorkflow(17).CheckState

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EDescription_for_Change_in_Payment) = chkMaintainClaimWorkflow(18).CheckState

            vWorkflowArray(gPMConstants.EClaimWorkflowId.ECash_Payment_process) = chkMaintainClaimWorkflow(20).CheckState

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EClaim_Payment_Doc_Message) = chkMaintainClaimWorkflow(22).CheckState

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EGenerate_Claim_Payment_doc) = chkMaintainClaimWorkflow(23).CheckState

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EMake_Further_Payments) = chkMaintainClaimWorkflow(24).CheckState


            m_lReturn = m_oBusiness.UpdateClaimWorkflow(v_iTask:=m_iTask, v_lProductID:=v_lProductID, vWorkflowArray:=vWorkflowArray, sUniqueId:=sUniqueId, sScreenHierarchy:=sScreenHierarchy & "/MaintainClaim")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                gPMFunctions.RaiseError(kMethodName, "Update of maintain claim workflow Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If


            'Retreive Pay claim workflow
            'N/a ones will be set to null

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EClaim_Process_Type_Id) = gPMConstants.PMWorkflowPayClaim

            vWorkflowArray(gPMConstants.EClaimWorkflowId.ECheck_Unpaid_Status) = chkPaymentClaimWorkflow(2).CheckState


            vWorkflowArray(gPMConstants.EClaimWorkflowId.EReinsurance_Recovery) = DBNull.Value


            vWorkflowArray(gPMConstants.EClaimWorkflowId.ESalvage_Recovery) = DBNull.Value


            vWorkflowArray(gPMConstants.EClaimWorkflowId.EThird_Party_Recovery) = DBNull.Value


            vWorkflowArray(gPMConstants.EClaimWorkflowId.EExternal_Claim_Handling) = DBNull.Value


            vWorkflowArray(gPMConstants.EClaimWorkflowId.EDescription_for_Change_in_Reserve) = DBNull.Value


            vWorkflowArray(gPMConstants.EClaimWorkflowId.EClaim_Notification_Doc_Message) = DBNull.Value


            vWorkflowArray(gPMConstants.EClaimWorkflowId.EGenerate_Claim_Notification_Doc) = DBNull.Value


            vWorkflowArray(gPMConstants.EClaimWorkflowId.EClaim_Payment_Process) = DBNull.Value

            vWorkflowArray(gPMConstants.EClaimWorkflowId.ECheck_Deferred_Reinsurance) = chkPaymentClaimWorkflow(3).CheckState

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EFast_Track_Claims) = chkPaymentClaimWorkflow(4).CheckState

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EReinsurance_Payment) = chkPaymentClaimWorkflow(7).CheckState

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EDescription_for_Change_in_Payment) = chkPaymentClaimWorkflow(8).CheckState

            vWorkflowArray(gPMConstants.EClaimWorkflowId.ECash_Payment_process) = chkPaymentClaimWorkflow(10).CheckState

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EClaim_Payment_Doc_Message) = chkPaymentClaimWorkflow(12).CheckState

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EGenerate_Claim_Payment_doc) = chkPaymentClaimWorkflow(13).CheckState

            vWorkflowArray(gPMConstants.EClaimWorkflowId.EMake_Further_Payments) = chkPaymentClaimWorkflow(14).CheckState


            m_lReturn = m_oBusiness.UpdateClaimWorkflow(v_iTask:=m_iTask, v_lProductID:=v_lProductID, vWorkflowArray:=vWorkflowArray, sUniqueId:=sUniqueId, sScreenHierarchy:=sScreenHierarchy & "/PayClaim")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                gPMFunctions.RaiseError(kMethodName, "Update of pay claim workflow Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
        Return result
    End Function

    'Start (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()
    Private Function GetTheTemplate(ByRef r_lDocumentTemplateID As Integer, ByRef r_sDocumentCode As String, ByRef bQuite As Boolean) As Integer
        Dim result As Integer = 0

        Dim oDocument As iPMBFindDocTemplate.Interface_Renamed
        Dim lDocumentTypeId As Integer

        Const kMethodName As String = "GetTheTemplate"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oDocument As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oDocument, sClassName:="iPMBFindDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oDocument = temp_oDocument


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Unable to get the Instance of iPMBFindDocTemplate Object", gPMConstants.PMELogLevel.PMLogError)
            End If


            'Developer Guide No. 9
            m_lReturn = oDocument.Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Unable to Initialise Find Document Template Interface", gPMConstants.PMELogLevel.PMLogError)
            End If



            oDocument.CallingAppName = ACApp

            m_lReturn = GetDocumentType(lDocumentTypeId, "COVERNOTE")

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Unable to Retrive the Document Type Id for Document code 'COVERNOTE'", gPMConstants.PMELogLevel.PMLogError)
            End If



            oDocument.DocumentTypeId = lDocumentTypeId
            '''
            '''    If r_lDocumentTemplateID > 0 Then
            '''
            '''        oDocument.DocumentTemplateId = r_lDocumentTemplateID
            '''
            '''    End If
            '''


            ''    If bQuite Then
            ''        m_lReturn =
            ''    Else
            ''
            ''        m_lReturn = oDocument.SetProcessModes(vTask:=PMView)
            ''
            ''    End If
            ''


            m_lReturn = oDocument.SetProcessModes(vProcessMode:=gPMConstants.PMEComponentAction.PMEdit)

            ' m_lReturn = oDocument.SetProcessModes(vTask:=PMEdit)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Unable to set the Process Mode of Find Document Template to Edit Mode", gPMConstants.PMELogLevel.PMLogError)
            End If


            oDocument.Mode = gPMConstants.PMEProcessMode.PMProcessModeFull


            m_lReturn = oDocument.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Unable to Start Find Document Template", gPMConstants.PMELogLevel.PMLogError)
            End If



            r_lDocumentTemplateID = gPMFunctions.ToSafeLong(oDocument.DocumentTemplateId)


            r_sDocumentCode = gPMFunctions.ToSafeString(oDocument.DocumentCode)



            oDocument.Dispose()
            oDocument = Nothing



        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            oDocument = Nothing

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally





        End Try
        Return result
    End Function

    Private Function GetDocumentType(ByRef r_lDocumentTypeId As Integer, ByVal v_sCode As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object
        Dim lDocumentTypeId As Integer
        Dim v_sTableName As String = ""

        Const kMethodName As String = "GetDocumentType"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            v_sTableName = "Document_Type"


            m_lReturn = m_oBusiness.GetLookUp(v_sTableName:=v_sTableName, r_vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Lookup Failed to Execute", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Information.IsArray(vResultArray) Then

                For lCount As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                    If CStr(vResultArray(1, lCount)).Trim().ToUpper() = v_sCode Then
                        lDocumentTypeId = gPMFunctions.ToSafeLong(vResultArray(0, lCount))
                        Exit For
                    End If
                Next
            End If

            r_lDocumentTypeId = lDocumentTypeId



        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally






        End Try
        Return result
    End Function

    Private Sub chkWrittenPolicy_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkWrittenPolicy.CheckedChanged
        If chkWrittenPolicy.Checked Then
            fraWrittenStatus.Enabled = True
            lblTaskManagerDays.Enabled = True
            txtTaskManagerDays.Enabled = True
            lblReminderTaskGroup.Enabled = True
            lblReminderUserGroup.Enabled = True
            cboReminderTaskGroup.Enabled = True
            cboReminderUserGroup.Enabled = True
            uctDocumentLink1.Status = True

            m_lReturn = GetLookUpDetails(v_sTableName:="PMWrk_Task_Group", r_cboControl:=cboReminderTaskGroup)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SetInterfaceDefaults", "Failed to get Lookup details", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = GetLookUpDetails(v_sTableName:="PMUser_Group", r_cboControl:=cboReminderUserGroup)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SetInterfaceDefaults", "Failed to get Lookup details", gPMConstants.PMELogLevel.PMLogError)
            End If

            cboReminderTaskGroup.SelectedIndex = 0
            cboReminderUserGroup.SelectedIndex = 0
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtTaskManagerDays,
                                                    vControlValue:=0)
            m_bWrittenPolicyStatus = True
        Else

            uctDocumentLink1.Status = False
            cboReminderTaskGroup.SelectedIndex = -1
            cboReminderUserGroup.SelectedIndex = -1
            cboReminderTaskGroup.Text = ""
            cboReminderUserGroup.Text = ""
            txtTaskManagerDays.Text = ""
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtTaskManagerDays,
                                                    vControlValue:="")
            fraWrittenStatus.Enabled = False
            m_bWrittenPolicyStatus = False
        End If
    End Sub

    Private Sub txtTaskManagerDays_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTaskManagerDays.LostFocus
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtTaskManagerDays)
    End Sub

    Private Sub lblTaskManagerDays_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblTaskManagerDays.GotFocus
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtTaskManagerDays)
    End Sub
    'End-  Written Status

    Private Sub uctPickListBranches_Change(ByVal Sender As Object, ByVal e As uctPickList.PickList.ChangeEventArgs) Handles uctPickListBranches.Change
        If (e.IsEmpty = False) Then
            If (e.Action = uctPickList.PickList.ChangeAction.Delete Or e.Action = uctPickList.PickList.ChangeAction.DeleteAll) Then
                If MessageBox.Show("You are attempting to remove Branch access from this Product.Confirmation of this action will result in all system Quotes/Policies assigned to this Product losing access to this Branch." & vbNewLine & "Do you wish to proceed?", "Branch Selection", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                    e.Cancel = True
                End If
            End If
        End If

    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tabMainTab.SelectedIndexChanged

    End Sub

    Private Sub chkDoNotDeleteRenewalQuoteOnMTA_CheckStateChanged(sender As Object, e As EventArgs) Handles chkDoNotDeleteRenewalQuoteOnMTA.CheckStateChanged
        If chkDoNotDeleteRenewalQuoteOnMTA.Checked Then
            chkDeleteRenewalQuoteReRunOnMTA.Enabled = False
        Else
            chkDeleteRenewalQuoteReRunOnMTA.Enabled = True
        End If
    End Sub

    Private Sub chkDeleteRenewalQuoteReRunOnMTA_CheckStateChanged(sender As Object, e As EventArgs) Handles chkDeleteRenewalQuoteReRunOnMTA.CheckStateChanged
        If chkDeleteRenewalQuoteReRunOnMTA.Checked Then
            chkDoNotDeleteRenewalQuoteOnMTA.Enabled = False
        Else
            chkDoNotDeleteRenewalQuoteOnMTA.Enabled = True
        End If
    End Sub

    Private Sub chkRecommender_CheckedChanged(sender As Object, e As EventArgs) Handles chkRecommender.CheckedChanged
        If chkRecommender.CheckState = CheckState.Checked Then
            lblAuthorisationThreshold.Visible = True
            txtAuthorisationThreshold.Visible = True
        Else
            lblAuthorisationThreshold.Visible = False
            txtAuthorisationThreshold.Visible = False
        End If
    End Sub

    Private Sub txtAuthorisationThreshold_Leave(sender As Object, e As EventArgs) Handles txtAuthorisationThreshold.Leave
        Dim bIsValid As Boolean = True

        If txtAuthorisationThreshold.Text.Trim() <> "" Then
            Dim dbNumericTemp As Double
            If Not Double.TryParse(txtAuthorisationThreshold.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                MessageBox.Show("Must be Numeric value", "Cover Note Group", MessageBoxButtons.OK, MessageBoxIcon.Error)
                bIsValid = False

            End If
        End If
        If Not bIsValid Then
            txtAuthorisationThreshold.Text = ""
            txtAuthorisationThreshold.Focus()
            Exit Sub
        End If

    End Sub

    ''' <summary>
    ''' Event handler for Quote Versioning checkbox state change
    ''' Enables/disables the Delete Quote After textbox based on checkbox state
    ''' Note: If Quote Versioning is already enabled in DB, the checkbox is disabled to prevent unchecking
    ''' </summary>
    Private Sub chkQuoteVersioning_CheckStateChanged(sender As Object, e As EventArgs) Handles chkQuoteVersioning.CheckStateChanged
        ' Only allow changes if the checkbox is enabled (i.e., not locked from DB)
        If Not chkQuoteVersioning.Enabled Then
            Exit Sub
        End If

        If chkQuoteVersioning.CheckState = CheckState.Checked Then
            ' Enable the Delete Quote After field when Quote Versioning is checked
            txtDeleteQuoteAfter.Enabled = True
        Else
            ' Disable and clear the Delete Quote After field when Quote Versioning is unchecked
            txtDeleteQuoteAfter.Enabled = False
            txtDeleteQuoteAfter.Text = ""
        End If
    End Sub

    Private Sub txtDeleteQuoteAfter_Leave(sender As Object, e As EventArgs) Handles txtDeleteQuoteAfter.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDeleteQuoteAfter)
    End Sub
End Class
