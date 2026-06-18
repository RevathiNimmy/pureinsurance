Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'Developer Guide 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form

    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 23-07-1997
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' RAW 17/12/2002 : PS187 : Added ProofListReportID and BordereauReportID
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const vbFormCode As Integer = 0
    Private Const ACClass As String = "frmInterface"

    Private m_oFormFields As iPMFormControl.FormFields

    Private m_iCompanyID As Integer

    ' PRIVATE Data Members (Begin)
    Private m_lAccountID As Integer
    Private m_iPurgefrequencyID As Integer
    Private m_iCurrencyID As Integer
    Private m_iAccounttypeID As Integer
    Private m_iLedgerID As Integer
    Private m_iPaymenttypeID As Integer
    Private m_sAccountName As String
    Private m_sShortCode As String
    Private m_iRestrictEnquiry As Integer
    Private m_iRestrictUpdate As Integer
    Private m_iDeleteAtPurge As Integer

    Private m_sContactName As String
    Private m_sAddress1 As String
    Private m_sAddress2 As String
    Private m_sAddress3 As String
    Private m_sAddress4 As String
    Private m_sPostalCode As String
    Private m_iAddressCountry As Integer
    Private m_sPhoneAreaCode As String
    Private m_sPhoneNumber As String
    Private m_sPhoneExtension As String
    Private m_sFaxAreaCode As String
    Private m_sFaxNumber As String
    Private m_sFaxExtension As String
    Private m_sPaymentName As String
    Private m_sPaymentAccountCode As String
    Private m_sPaymentBranchCode As String
    Private m_dtPaymentExpiryDate As Date
    Private m_sPaymentReference1 As String
    Private m_sPaymentReference2 As String
    Private m_lProofListReportID As Integer ' RAW 17/12/2002 : PS187 : Added
    Private m_lBordereauReportID As Integer ' RAW 17/12/2002 : PS187 : Added
    Private m_vdCreditLimit As Object
    Private m_vdDiscountPercentage As Object
    Private m_iSettlementPeriod As Integer
    Private m_sBankName As String
    Private m_sBankAddress1 As String
    Private m_sBankAddress2 As String
    Private m_sBankAddress3 As String
    Private m_sBankAddress4 As String
    Private m_sBankPostalCode As String
    Private m_iBankCountry As Integer
    Private m_sBankPhoneAreaCode As String
    Private m_sBankPhoneNumber As String
    Private m_sBankPhoneExtension As String
    Private m_sBankFaxAreaCode As String
    Private m_sBankFaxNumber As String
    Private m_sBankFaxExtension As String
    Private m_sComments As String
    Private m_lNominalCode As Integer
    Private m_iAccountStatusID As Integer
    Private m_iOriginalAccountStatusID As Integer
    ' RDC 12112003
    Private m_bAllowElectronicPayment As Boolean

    'EK091298
    Private m_sFullPath As String = ""

    'CF 040399
    Private m_lPMAuthorityLevel As gPMConstants.PMEAuthorityLevel

    'Party Bank Details
    Private m_vPartyBankDetails(,) As Object

    ' *** BEGIN Inserted By ResGen ***

    ' Form Constants for Captions

    Const ACInterfaceCaption As Integer = 100
    Const ACMainTabTitle0 As Integer = 101
    Const ACMainTabTitle1 As Integer = 102
    Const ACMainTabTitle2 As Integer = 103
    Const ACMainTabTitle3 As Integer = 104
    Const ACMainTabTitle4 As Integer = 105
    Const ACPurgefrequencyIDCaption As Integer = 106
    Const ACRestrictEnquiryCaption As Integer = 107
    Const ACRestrictUpdateCaption As Integer = 108
    Const ACDeleteAtPurgeCaption As Integer = 109
    Const ACAccounttypeIDCaption As Integer = 110
    Const ACLedgerIDCaption As Integer = 111
    Const ACAccountCodeCaption As Integer = 112
    Const ACAccountNameCaption As Integer = 113
    Const ACShortCodeCaption As Integer = 114
    Const ACExtensionCaption As Integer = 115
    Const ACContactNameCaption As Integer = 116
    Const ACAddress1Caption As Integer = 117
    Const ACAddress2Caption As Integer = 118
    Const ACAddresss3Caption As Integer = 119
    Const ACAddress4Caption As Integer = 120
    Const ACPostalCodeCaption As Integer = 121
    Const ACAddressCountryCaption As Integer = 122
    Const ACTelephoneCaption As Integer = 123
    Const ACFaxCaption As Integer = 124
    Const ACAreaCodeCaption As Integer = 125
    Const ACNumberCaption As Integer = 126
    Const ACSettlementTermsCaption As Integer = 127
    Const ACDaysCaption As Integer = 128
    Const ACSettlementPeriodCaption As Integer = 129
    Const ACDiscountPercentageCaption As Integer = 130
    Const ACCreditLimitCaption As Integer = 131
    Const ACPaymentAccountCodeCaption As Integer = 132
    Const ACPaymentBranchCodeCaption As Integer = 133
    Const ACPaymentExpiryDateCaption As Integer = 134
    Const ACPaymentReference2Caption As Integer = 135
    Const ACPaymentReference1Caption As Integer = 136
    Const ACPaymenttypeIDCaption As Integer = 137
    Const ACPaymentNameCaption As Integer = 138
    Const ACBankNumberCaption As Integer = 139
    Const ACBankAreaCodeCaption As Integer = 140
    Const ACBankFaxCaption As Integer = 141
    Const ACBankPhoneCaption As Integer = 142
    Const ACBankCountryCaption As Integer = 143
    Const ACBankPostalCodeCaption As Integer = 144
    Const ACBankAddress4Caption As Integer = 145
    Const ACBankAddress3Caption As Integer = 146
    Const ACBankAddress2Caption As Integer = 147
    Const ACBankAddress1Caption As Integer = 148
    Const ACBankNameCaption As Integer = 149
    Const ACBankExtensionCaption As Integer = 150
    Const ACNominalCodeCaption As Integer = 151
    Const ACAccountStatusCaption As Integer = 152
    Const ACProofListReportIDCaption As Integer = 153 ' RAW 17/12/2002 : PS187 : Added
    Const ACBordereauReportIDCaption As Integer = 154 ' RAW 17/12/2002 : PS187 : Added
    Const ACClientMoneyCalcAccTye As Integer = 155
    Const ACClientBankAccType As Integer = 156

    ' Button Constants for Captions

    Const ACNavigateCaption As Integer = 200
    Const ACHelpCaption As Integer = 201
    Const ACCancelCaption As Integer = 202
    Const ACOKCaption As Integer = 203
    Const ACOKTip As Integer = 204
    Const ACNextCaption0 As Integer = 205
    Const ACNextCaption1 As Integer = 206
    Const ACNextCaption2 As Integer = 207
    Const ACNextCaption3 As Integer = 208


    ' Message Constants for Captions

    'Party Bank Details
    Const ACMainTabTitle5 As Integer = 304

    'Start - Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
    Const ACMainTabTitle6 As Integer = 305
    Const ACMainTabTitle7 As Integer = 306
    Const ACCashDeposit As Integer = 157
    'End - Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
    ' *** END Inserted By ResGen ***

    Const ACMerchantIdArrPos As Integer = 0
    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_iCurrentTab As Integer

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iACTAccount.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object
    'EK091298

    Private m_oExplorer As bACTExplorer.Form

    'Party Bank Details

    Private m_oPartyBank As bSIRPartyBank.Business

    'DD 15/07/2002: True if new product option is enabled
    Private m_bEnhancedSecurity As Boolean
    Private m_bHasUnrestrictedUpdate As Boolean
    ' RDC 13112003
    Private m_bElectronicPayment As Boolean

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object
    Private m_vLedgerDetails(,) As Object

    ' Stores the return value for function calls.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' RAW 12/16/2002 : PS187 : replaces local definitions within procedures
    Private Const m_kiTableAccountType As Integer = 0
    Private Const m_kiTablePurgeFrequency As Integer = 1
    Private Const m_kiTableAddressCountry As Integer = 2
    Private Const m_kiTablePaymentType As Integer = 3
    Private Const m_kiTableBankCountry As Integer = 4
    Private Const m_kiTableAccountStatus As Integer = 5
    Private Const m_kiTableProofListReport As Integer = 6 ' RAW 17/12/2002 : PS187 : added
    Private Const m_kiTableBordereauReport As Integer = 7 ' RAW 17/12/2002 : PS187 : added
    Private Const m_kiTableArrayUpperBound As Integer = 7 ' RAW 17/12/2002 : PS187 : added
    ' RAW 12/16/2002 : PS187 : end
    Private m_iMoneyCalcAccType As Integer
    'Private m_vClientBankAccType As Byte
    Private m_vClientBankAccType As String
    Private m_sMerchantId As String = ""
    Private m_sLinkedAccountsTitle5 As String = ""
    Private m_sLinkedAccountsTitle7 As String = ""

    ' Authority Level
    Public Property PMAuthorityLevel() As Integer
        Get
            Return m_lPMAuthorityLevel
        End Get
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    ' Company ID needs to be set by the caller


    Public Property CompanyID() As Integer
        Get
            Return m_iCompanyID
        End Get
        Set(ByVal Value As Integer)
            m_iCompanyID = Value
        End Set
    End Property

    ' The Id of the record being created/Edited


    Public Property AccountID() As Integer
        Get
            Return m_lAccountID
        End Get
        Set(ByVal Value As Integer)
            m_lAccountID = Value
        End Set
    End Property

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    ' PRIVATE Property Procedures (Begin)

    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    '
    ' Standard Property.
    '
    ' Set the interface exit status.
    'm_lStatus = Value
    '
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public Property Task() As Integer
        Get

            ' Return the objects task.
            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the objects task.
            m_iTask = Value

        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_lNavigate = Value

        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the process mode.
            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the type of business.
            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            ' Standard Property.

            ' Set the effective date.
            m_dtEffectiveDate = Value

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
        Dim lAccountIDMandy, lPurgefrequencyIDMandy, lCurrencyIDMandy, lAccounttypeIDMandy, lLedgerIDMandy, lPaymenttypeIDMandy, lAccountNameMandy, lShortCodeMandy, lRestrictEnquiryMandy, lRestrictUpdateMandy, lDeleteAtPurgeMandy, lContactNameMandy, lAddress1Mandy, lAddress2Mandy, lAddress3Mandy, lAddress4Mandy, lPostalCodeMandy, lAddressCountryMandy, lPhoneAreaCodeMandy, lPhoneNumberMandy, lPhoneExtensionMandy, lFaxAreaCodeMandy, lFaxNumberMandy, lFaxExtensionMandy, lPaymentNameMandy, lPaymentAccountCodeMandy, lPaymentBranchCodeMandy, lPaymentExpiryDateMandy, lPaymentReference1Mandy, lPaymentReference2Mandy, lProofListReportIDMandy As Integer ' RAW 17/12/2002 : PS187 : Added
        Dim lBordereauReportIDMandy As Integer ' RAW 17/12/2002 : PS187 : Added
        Dim lCreditLimitMandy, lDiscountPercentageMandy, lSettlementPeriodMandy, lBankNameMandy, lBankAddress1Mandy, lBankAddress2Mandy, lBankAddress3Mandy, lBankAddress4Mandy, lBankPostalCodeMandy, lBankCountryMandy, lBankPhoneAreaCodeMandy, lBankPhoneNumberMandy, lBankPhoneExtensionMandy, lBankFaxAreaCodeMandy, lBankFaxNumberMandy, lBankFaxExtensionMandy, lCommentsMandy As Integer
        'EK091298
        Dim lAccountCodeMandy, lAccountStatusIDMandy As Integer
        ' RDC 12112003
        Dim lAllowElectronicPaymentMandy As Integer

        Dim lMoneyCalcAccTypeMandy, lClientBankAccTypeMandy, lMerchantIdMandy As Integer

        Const ACPercentageDecimalPlaces As Integer = 2

        Try

            ' Get the Mandy details from the business object.

            ' RAW 17/12/2002 : PS187 : ProofListReportID and BordereauReportID added
            ' RDC 12112003

            m_lReturn = m_oBusiness.GetMandatory(lAccountIDMandy:=lAccountIDMandy, lPurgefrequencyIDMandy:=lPurgefrequencyIDMandy, lCurrencyIDMandy:=lCurrencyIDMandy, lAccounttypeIDMandy:=lAccounttypeIDMandy, lLedgerIDMandy:=lLedgerIDMandy, lPaymenttypeIDMandy:=lPaymenttypeIDMandy, lAccountNameMandy:=lAccountNameMandy, lShortCodeMandy:=lShortCodeMandy, lRestrictEnquiryMandy:=lRestrictEnquiryMandy, lRestrictUpdateMandy:=lRestrictUpdateMandy, lDeleteAtPurgeMandy:=lDeleteAtPurgeMandy, lContactNameMandy:=lContactNameMandy, lAddress1Mandy:=lAddress1Mandy, lAddress2Mandy:=lAddress2Mandy, lAddress3Mandy:=lAddress3Mandy, lAddress4Mandy:=lAddress4Mandy, lPostalCodeMandy:=lPostalCodeMandy, lAddressCountryMandy:=lAddressCountryMandy, lPhoneAreaCodeMandy:=lPhoneAreaCodeMandy, lPhoneNumberMandy:=lPhoneNumberMandy, lPhoneExtensionMandy:=lPhoneExtensionMandy, lFaxAreaCodeMandy:=lFaxAreaCodeMandy, lFaxNumberMandy:=lFaxNumberMandy, lFaxExtensionMandy:=lFaxExtensionMandy, lPaymentNameMandy:=lPaymentNameMandy, lPaymentAccountCodeMandy:=lPaymentAccountCodeMandy, lPaymentBranchCodeMandy:=lPaymentBranchCodeMandy, lPaymentExpiryDateMandy:=lPaymentExpiryDateMandy, lPaymentReference1Mandy:=lPaymentReference1Mandy, lPaymentReference2Mandy:=lPaymentReference2Mandy, lCreditLimitMandy:=lCreditLimitMandy, lDiscountPercentageMandy:=lDiscountPercentageMandy, lSettlementPeriodMandy:=lSettlementPeriodMandy, lBankNameMandy:=lBankNameMandy, lBankAddress1Mandy:=lBankAddress1Mandy, lBankAddress2Mandy:=lBankAddress2Mandy, lBankAddress3Mandy:=lBankAddress3Mandy, lBankAddress4Mandy:=lBankAddress4Mandy, lBankPostalCodeMandy:=lBankPostalCodeMandy, lBankCountryMandy:=lBankCountryMandy, lBankPhoneAreaCodeMandy:=lBankPhoneAreaCodeMandy, lBankPhoneNumberMandy:=lBankPhoneNumberMandy, lBankPhoneExtensionMandy:=lBankPhoneExtensionMandy, lBankFaxAreaCodeMandy:=lBankFaxAreaCodeMandy, lBankFaxNumberMandy:=lBankFaxNumberMandy, lBankFaxExtensionMandy:=lBankFaxExtensionMandy, lCommentsMandy:=lCommentsMandy, lAccountStatusIDMandy:=lAccountStatusIDMandy, lProofListReportIDMandy:=lProofListReportIDMandy, lBordereauReportIDMandy:=lBordereauReportIDMandy, lAllowElectronicPayment:=lAllowElectronicPaymentMandy)

            m_oFormFields = New iPMFormControl.FormFields()

            m_oFormFields.LanguageID = g_iLanguageID

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboPurgefrequencyID, lFieldType:=gPMConstants.PMEDataType.PMLookup, lFormat:=gPMConstants.PMEFormatStyle.PMFormatBoolean, lMandatory:=lPurgefrequencyIDMandy)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    m_lReturn = m_oFormFields.AddNewFormField( _
            ''                 ctlControl:=cboCurrencyID, _
            ''                 lFieldType:=PMLookup, _
            ''                 lFormat:=PMLookup, _
            ''                 lMandatory:=lCurrencyIDMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboAccounttypeID, lFieldType:=gPMConstants.PMEDataType.PMLookup, lFormat:=gPMConstants.PMEFormatStyle.PMFormatBoolean, lMandatory:=lAccounttypeIDMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboLedgerID, lFieldType:=gPMConstants.PMEDataType.PMLookup, lFormat:=gPMConstants.PMEFormatStyle.PMFormatBoolean, lMandatory:=lLedgerIDMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboPaymentTypeID, lFieldType:=gPMConstants.PMEDataType.PMLookup, lFormat:=gPMConstants.PMEFormatStyle.PMFormatBoolean, lMandatory:=lPaymenttypeIDMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAccountName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lAccountNameMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtShortCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatStringUpper, lMandatory:=lShortCodeMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=chkRestrictEnquiry, lFieldType:=gPMConstants.PMEDataType.PMBoolean, lFormat:=gPMConstants.PMEFormatStyle.PMFormatBoolean, lMandatory:=lRestrictEnquiryMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=chkRestrictUpdate, lFieldType:=gPMConstants.PMEDataType.PMBoolean, lFormat:=gPMConstants.PMEFormatStyle.PMFormatBoolean, lMandatory:=lRestrictUpdateMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=chkDeleteAtPurge, lFieldType:=gPMConstants.PMEDataType.PMBoolean, lFormat:=gPMConstants.PMEFormatStyle.PMFormatBoolean, lMandatory:=lDeleteAtPurgeMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtContactName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lContactNameMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAddress1, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lAddress1Mandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAddress2, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lAddress2Mandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAddress3, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lAddress3Mandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAddress4, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lAddress4Mandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPostalCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lPostalCodeMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboAddressCountry, lFieldType:=gPMConstants.PMEDataType.PMInteger, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=lAddressCountryMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPhoneAreaCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lPhoneAreaCodeMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPhoneNumber, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lPhoneNumberMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPhoneExtension, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lPhoneExtensionMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFaxAreaCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lFaxAreaCodeMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFaxNumber, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lFaxNumberMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPaymentName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lPaymentNameMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPaymentAccountCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lPaymentAccountCodeMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPaymentBranchCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lPaymentBranchCodeMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPaymentExpiryDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=lPaymentExpiryDateMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPaymentReference1, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lPaymentReference1Mandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPaymentReference2, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lPaymentReference2Mandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' RAW 17/12/2002 : PS187 : Added
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboProofListReportID, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatLong, lMandatory:=lProofListReportIDMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboBordereauReportID, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatLong, lMandatory:=lBordereauReportIDMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' RAW 17/12/2002 : PS187 : End

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCreditLimit, lFieldType:=gPMConstants.PMEDataType.PMDecimal, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDecimal, lMandatory:=lCreditLimitMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDiscountPercentage, lFieldType:=gPMConstants.PMEDataType.PMDecimal, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDecimal, lMandatory:=lDiscountPercentageMandy, lDecimalPlaces:=ACPercentageDecimalPlaces)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSettlementPeriod, lFieldType:=gPMConstants.PMEDataType.PMInteger, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=lSettlementPeriodMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtBankName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lBankNameMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtBankAddress1, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lBankAddress1Mandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtBankAddress2, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lBankAddress2Mandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtBankAddress3, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lBankAddress3Mandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtBankAddress4, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lBankAddress4Mandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtBankPostalCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lBankPostalCodeMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboBankCountry, lFieldType:=gPMConstants.PMEDataType.PMLookup, lFormat:=gPMConstants.PMEFormatStyle.PMFormatBoolean, lMandatory:=lBankCountryMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtBankPhoneAreaCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lBankPhoneAreaCodeMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtBankPhoneNumber, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lBankPhoneNumberMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtBankPhoneExtension, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lBankPhoneExtensionMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtBankFaxAreaCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lBankFaxAreaCodeMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtBankFaxNumber, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lBankFaxNumberMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtComments, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lCommentsMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'EK091298
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAccountCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lAccountCodeMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' CF 040399
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboAccountStatus, lFieldType:=gPMConstants.PMEDataType.PMInteger, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=lAccountStatusIDMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboMoneyCalcAccType, lFieldType:=gPMConstants.PMEDataType.PMInteger, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=lMoneyCalcAccTypeMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboClientBankAccType, lFieldType:=gPMConstants.PMEDataType.PMInteger, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=lClientBankAccTypeMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=chkElectronicPayment, lFieldType:=gPMConstants.PMEDataType.PMBoolean, lFormat:=gPMConstants.PMEFormatStyle.PMFormatBoolean, lMandatory:=lAllowElectronicPaymentMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtMerchantId, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=lMerchantIdMandy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function
    ' ***************************************************************** '
    ' Name: Initialise
    '
    ' Description: Explicit intialise for the form
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Dim sMessage, sTitle As String

        'TN20010628 - variable to see if we need to display postcode
        Dim lDisplayPostCode As gPMConstants.PMEReturnCode

        Try
            'developer guide no. 38
            Me.cboPaymentTypeID.FirstItem = "(N/A)"
            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value. ? why
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Initailise business objects to serve this form
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bACTAccount.Form", vInstanceManager:="ClientManager")
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)


                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'EK091298

            Dim temp_m_oExplorer As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oExplorer, "bACTExplorer.Form", vInstanceManager:="ClientManager")
            m_oExplorer = temp_m_oExplorer


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)


                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iACTAccount.General()

            'Party Bank Details
            Dim temp_m_oPartyBank As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oPartyBank, "bSIRPartyBank.Business", vInstanceManager:="ClientManager")
            m_oPartyBank = temp_m_oPartyBank

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)


                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the cancelled property to true. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Return result

            End If
            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If

            m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If

            'TN20010629 - Start (hide postcode if it not required for this address)

            m_lReturn = m_oBusiness.IsPostCode(v_lAccountId:=m_lAccountID, r_lResult:=lDisplayPostCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If

            If lDisplayPostCode <> gPMConstants.PMEReturnCode.PMTrue Then
                lblPostalCode.Visible = False
                txtPostalCode.Visible = False
            Else
                lblPostalCode.Visible = True
                txtPostalCode.Visible = True
            End If
            'TN20010629 - End


            m_lReturn = CType(GetLedgerDetails(ctlLookup:=cboLedgerID), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise the interface form", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Dim bHasUnrestrictedEnquiry As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}


            m_lReturn = m_oBusiness.GetDetails(vAccountID:=m_lAccountID)

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If

            'DD 15/07/2002: Added enhanced security option
            If m_bEnhancedSecurity Then

                m_lReturn = m_oBusiness.GetAccountSecurity(v_lAccountId:=m_lAccountID, r_bHasUnrestrictedEnquiry:=bHasUnrestrictedEnquiry, r_bHasUnrestrictedUpdate:=m_bHasUnrestrictedUpdate)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get details.
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get Account Security from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    Return result
                End If

                If Not bHasUnrestrictedEnquiry Then
                    'User does not have rights to this Account
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="User does not have rights to view this account.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")


                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Set the OK enabled property
                cmdOK.Enabled = m_bHasUnrestrictedUpdate
            End If

            'EK091298

            m_sFullPath = m_oExplorer.FullKey(lAccountId:=m_lAccountID)

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the explorer object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business
    '              object.
    '
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "BusinessToInterface" 'Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
        Try

            'Party Bank Details
            Dim lAccountKey As Integer

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Assign the details from the business object
            ' to the data storage.
            m_lReturn = CType(BusinessToData(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Assign the details to the interface.
            m_lReturn = m_oFormFields.FormatControl(cboPurgefrequencyID, m_iPurgefrequencyID)
            m_lReturn = m_oFormFields.FormatControl(cboAccounttypeID, m_iAccounttypeID)
            m_lReturn = m_oFormFields.FormatControl(cboLedgerID, m_iLedgerID)
            'm_lReturn = m_oFormFields.FormatControl(cboPaymentTypeID, CVar(m_iPaymenttypeID))
            cboPaymentTypeID.ItemId = m_iPaymenttypeID
            m_lReturn = m_oFormFields.FormatControl(txtAccountName, m_sAccountName)

            ' Only enable account status if its not a Nominal Account
            If m_iLedgerID <> gACTLibrary.ACTLedgerTypeGeneral Then
                If m_lPMAuthorityLevel <> gPMConstants.PMEAuthorityLevel.pmeALUser Then
                    lblAccountStatus.Enabled = True
                    cboAccountStatus.Enabled = True
                End If
            Else
                lblAccountStatus.Enabled = False
                cboAccountStatus.Enabled = False
            End If

            m_lReturn = m_oFormFields.FormatControl(txtShortCode, m_sShortCode)

            ' CF 040199 - Added short code to form's caption
            Me.Text = Me.Text & " (" & m_sShortCode.Trim() & ")"

            m_lReturn = m_oFormFields.FormatControl(txtContactName, m_sContactName)
            m_lReturn = m_oFormFields.FormatControl(txtAddress1, m_sAddress1)
            m_lReturn = m_oFormFields.FormatControl(txtAddress2, m_sAddress2)
            m_lReturn = m_oFormFields.FormatControl(txtAddress3, m_sAddress3)
            m_lReturn = m_oFormFields.FormatControl(txtAddress4, m_sAddress4)
            m_lReturn = m_oFormFields.FormatControl(txtPostalCode, m_sPostalCode)
            m_lReturn = m_oFormFields.FormatControl(cboAddressCountry, m_iAddressCountry)
            m_lReturn = m_oFormFields.FormatControl(txtPhoneAreaCode, m_sPhoneAreaCode)
            m_lReturn = m_oFormFields.FormatControl(txtPhoneNumber, m_sPhoneNumber)
            m_lReturn = m_oFormFields.FormatControl(txtPhoneExtension, m_sPhoneExtension)
            m_lReturn = m_oFormFields.FormatControl(txtFaxAreaCode, m_sFaxAreaCode)
            m_lReturn = m_oFormFields.FormatControl(txtFaxNumber, m_sFaxNumber)
            m_lReturn = m_oFormFields.FormatControl(txtPaymentName, m_sPaymentName)
            m_lReturn = m_oFormFields.FormatControl(txtPaymentAccountCode, m_sPaymentAccountCode)
            m_lReturn = m_oFormFields.FormatControl(txtPaymentBranchCode, m_sPaymentBranchCode)
            m_lReturn = m_oFormFields.FormatControl(txtPaymentExpiryDate, m_dtPaymentExpiryDate)
            m_lReturn = m_oFormFields.FormatControl(txtPaymentReference1, m_sPaymentReference1)
            m_lReturn = m_oFormFields.FormatControl(txtPaymentReference2, m_sPaymentReference2)
            m_lReturn = m_oFormFields.FormatControl(cboProofListReportID, m_lProofListReportID) ' RAW 17/12/2002 : PS187 : Added
            m_lReturn = m_oFormFields.FormatControl(cboBordereauReportID, m_lBordereauReportID) ' RAW 17/12/2002 : PS187 : Added
            m_lReturn = m_oFormFields.FormatControl(txtCreditLimit, m_vdCreditLimit)
            m_lReturn = m_oFormFields.FormatControl(txtDiscountPercentage, m_vdDiscountPercentage)
            m_lReturn = m_oFormFields.FormatControl(txtSettlementPeriod, m_iSettlementPeriod)
            m_lReturn = m_oFormFields.FormatControl(txtBankName, m_sBankName)
            m_lReturn = m_oFormFields.FormatControl(txtBankAddress1, m_sBankAddress1)
            m_lReturn = m_oFormFields.FormatControl(txtBankAddress2, m_sBankAddress2)
            m_lReturn = m_oFormFields.FormatControl(txtBankAddress3, m_sBankAddress3)
            'eck041001
            '   m_lReturn = m_oFormFields.FormatControl(txtBankPostalCode, CVar(m_sBankAddress4))
            m_lReturn = m_oFormFields.FormatControl(txtBankAddress4, m_sBankAddress4)
            m_lReturn = m_oFormFields.FormatControl(txtBankPostalCode, m_sBankPostalCode)
            m_lReturn = m_oFormFields.FormatControl(cboBankCountry, m_iBankCountry)
            m_lReturn = m_oFormFields.FormatControl(txtBankPhoneAreaCode, m_sBankPhoneAreaCode)
            m_lReturn = m_oFormFields.FormatControl(txtBankPhoneNumber, m_sBankPhoneNumber)
            m_lReturn = m_oFormFields.FormatControl(txtBankPhoneExtension, m_sBankPhoneExtension)
            m_lReturn = m_oFormFields.FormatControl(txtBankFaxAreaCode, m_sBankFaxAreaCode)
            m_lReturn = m_oFormFields.FormatControl(txtBankFaxNumber, m_sBankFaxNumber)
            m_lReturn = m_oFormFields.FormatControl(txtComments, m_sComments)
            m_lReturn = m_oFormFields.FormatControl(chkRestrictEnquiry, m_iRestrictEnquiry)
            m_lReturn = m_oFormFields.FormatControl(chkRestrictUpdate, m_iRestrictUpdate)
            m_lReturn = m_oFormFields.FormatControl(chkDeleteAtPurge, m_iDeleteAtPurge)
            ' RDC 12112003
            m_lReturn = m_oFormFields.FormatControl(chkElectronicPayment, m_bAllowElectronicPayment)

            'EK091298
            m_sFullPath = m_sFullPath & "\" & m_sShortCode
            m_lReturn = m_oFormFields.FormatControl(txtAccountCode, m_sFullPath)

            'CF150199
            uctAccountLookup.AccountId = m_lNominalCode

            ' CF040399
            m_lReturn = m_oFormFields.FormatControl(cboAccountStatus, m_iAccountStatusID)

            Select Case (m_iMoneyCalcAccType)
                Case 1
                    cboMoneyCalcAccType.Text = "Client Bank Account"
                Case 2
                    cboMoneyCalcAccType.Text = "Client Money Suspense Account"
                Case 3
                    cboMoneyCalcAccType.Text = "Designated Investment Account"
                Case Else
                    cboMoneyCalcAccType.Text = "Other"
            End Select

            If m_iMoneyCalcAccType <> 0 Then
                Select Case (m_vClientBankAccType)
                    Case 0
                        cboClientBankAccType.Enabled = True
                        lblClientBankAccType.Enabled = True
                        cboClientBankAccType.Text = "Statutory Trust Account"
                    Case 1
                        cboClientBankAccType.Enabled = True
                        lblClientBankAccType.Enabled = True
                        cboClientBankAccType.Text = "Non-Statutory Trust Account"
                    Case Else
                        lblClientBankAccType.Enabled = False
                        cboClientBankAccType.Enabled = False
                End Select
            Else
                cboClientBankAccType.SelectedIndex = -1
                lblClientBankAccType.Enabled = False
                cboClientBankAccType.Enabled = False
            End If

            'DC220703 -ISS5502
            If cboLedgerID.Text.Trim().ToLower() = "purchase" Then
                lblNominalCode.Enabled = True
                uctAccountLookup.Enabled = True
                SSTabHelper.SetTabVisible(tabMainTab, 3, True)
                SSTabHelper.SetTabVisible(tabMainTab, 4, True)
                cmdNext(2).Visible = True
                SSTabHelper.SetTabVisible(tabMainTab, 5, False)
            Else
                lblNominalCode.Enabled = False
                uctAccountLookup.Enabled = False
                SSTabHelper.SetTabVisible(tabMainTab, 3, False)
                SSTabHelper.SetTabVisible(tabMainTab, 4, False)
            End If

            ' RDC 13112003 property can be changed only if enabled in options
            lblElectronicPayment.Visible = m_bElectronicPayment
            chkElectronicPayment.Visible = m_bElectronicPayment
            lblElectronicPayment.Enabled = m_bElectronicPayment
            chkElectronicPayment.Enabled = m_bElectronicPayment

            m_lReturn = m_oFormFields.FormatControl(txtMerchantId, m_sMerchantId)

            uctPartyBankControl2.AccountId = m_lAccountID
            'developer guide no. 9
            uctPartyBankControl2.Initialise()
            'Developer Guide- Related to no 108
            uctPartyBankControl2.Load_Renamed()
            'Party Bank Details
            If Not gPMFunctions.IsArrayEmpty(m_vPartyBankDetails) Then

                uctPartyBankControl2.PartyBankDetails = VB6.CopyArray(m_vPartyBankDetails)
                uctPartyBankControl2.BusinessToInterface()
                'uctPartyBankControl1.ReadOnly = 1
            End If

            m_lReturn = m_oBusiness.GetAccountKey(lAccountID:=m_lAccountID, r_lAccountKey:=lAccountKey)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                uctPartyBankControl2.ReadOnly_Renamed = 1
            End If

            If lAccountKey > 0 Then
                uctPartyBankControl2.ReadOnly_Renamed = 1
            End If
            uctPartyBankControl2.AccountId = AccountID

            'Start - Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
            uctLinkedAccountsControl.AccountId = m_lAccountID
            'developer guide no. 9
            uctLinkedAccountsControl.Initialise()
            'Developer Guide- Related to no 108
            uctLinkedAccountsControl.Load_Renamed()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "uctLinkedAccountsControl.Load Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            If uctLinkedAccountsControl.ItemCount <= 0 Then
                SSTabHelper.SetTabEnabled(tabMainTab, 6, False)
            Else
                SSTabHelper.SetTabEnabled(tabMainTab, 6, True)
            End If
            'End - Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         CreateWorkManagerMemo
    '
    ' Description:  Creates a memo for an insurance team memeber
    '               to change the Agent status to Stopped or Active.
    '
    ' ***************************************************************** '
    Private Function CreateWorkManagerMemo(ByVal v_iAccountStatusID As Integer, ByVal v_sAccountCode As String, ByVal v_sAccountName As String) As Integer
        Dim result As Integer = 0

        Dim vKeys As Object
        Dim sTaskDescription As String = ""

        Dim oWrkTaskInstance As iPMWrkTaskInstance.NavigatorV3

        Dim oPMLookUp As bPMLookup.Business
        Dim lTaskGroupID, lTaskID As Integer
        Dim sVerb As String = ""

        Const TASK_CODE As String = "MEMO"
        Const TASK_GROUP_CODE As String = "COMMON"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim vKeys(1, 7)

            ' Object to create work manager tasks
            Dim temp_oWrkTaskInstance As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oWrkTaskInstance, "iPMWrkTaskInstance.NavigatorV3", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oWrkTaskInstance = temp_oWrkTaskInstance
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create an instance of bPMLookup
            Dim temp_oPMLookUp As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMLookUp, "bPMLookup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLookUp = temp_oPMLookUp
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the product family

            oPMLookUp.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusArchitecture

            ' Use the lookup to get the ID for the group COMMON

            m_lReturn = oPMLookUp.GetEffectiveIDFromCode(v_sTableName:="pmwrk_task_group", v_sCode:=TASK_GROUP_CODE, v_dtEffectiveDate:=DateTime.Today, r_lID:=lTaskGroupID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Use the lookup to get the ID of the MEMO task

            m_lReturn = oPMLookUp.GetEffectiveIDFromCode(v_sTableName:="pmwrk_task", v_sCode:=TASK_CODE, v_dtEffectiveDate:=DateTime.Today, r_lID:=lTaskID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Remove instance of lookup

            oPMLookUp.Dispose()
            oPMLookUp = Nothing

            ' Set the authority level


            ' Set to ADD mode
            m_lReturn = CType(oWrkTaskInstance.NavigatorV3_SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_iAccountStatusID = gACTLibrary.ACTAccountStatusActive Then
                sVerb = "ACTIVATED"
            Else
                sVerb = "STOPPED"
            End If

            ' Active or Stopped ?
            sTaskDescription = "The account """ & v_sAccountCode.Trim() & """ has been " &
                               sVerb & " in Orion. " &
                               "The corresponding agent must be " &
                               sVerb & " in Sirius Back Office."

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameTaskGroupID
            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = lTaskGroupID
            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameTaskGroupCode
            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = TASK_GROUP_CODE
            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameTaskID
            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = lTaskID
            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameTaskCode
            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = TASK_CODE
            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameTaskDescription
            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = sTaskDescription
            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNameTaskCustomer
            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_sAccountName
            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.PMKeyNameTaskDueDate
            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = DateTime.Today
            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = PMNavKeyConst.PMKeyNameTaskIsUrgent
            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = gPMConstants.PMEReturnCode.PMTrue

            ' Pass the keys in
            m_lReturn = CType(oWrkTaskInstance.NavigatorV3_SetKeys(vKeyArray:=vKeys), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Display the form

            m_lReturn = oWrkTaskInstance.NavigatorV3_Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Remove instance

            oWrkTaskInstance.Dispose()
            oWrkTaskInstance = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateWorkManagerMemo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateWorkManagerMemo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToBusiness
    '
    ' Description: Updates all business members from the interface
    '              details.
    '
    ' ***************************************************************** '
    Public Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0
        Dim lBusinessDataID As Integer
        Dim vParamArray() As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ReDim vParamArray(0)

            ' Update the business object.

            ' Assign the details from the interface to the data storage.
            m_lReturn = CType(InterfaceToData(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the business data ID to one because we are only
            ' dealing with one record item only.
            lBusinessDataID = 1


            vParamArray(ACMerchantIdArrPos) = m_sMerchantId
            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Inform the business object with a new data item.

                    ' {* USER DEFINED CODE (Begin) *}
                    ' RAW 17/12/2002 : PS187 : Added ProofListReportID and BordereauReportID
                    ' RDC 12112003 added vAllowElectronicPayment

                    m_lReturn = m_oBusiness.EditAdd(lRow:=lBusinessDataID, vAccountID:=m_lAccountID, vPurgefrequencyID:=m_iPurgefrequencyID, vCurrencyID:=m_iCurrencyID, vAccounttypeID:=m_iAccounttypeID, vLedgerID:=m_iLedgerID, vPaymenttypeID:=m_iPaymenttypeID, vAccountName:=m_sAccountName, vShortCode:=m_sShortCode, vRestrictEnquiry:=m_iRestrictEnquiry, vRestrictUpdate:=m_iRestrictUpdate, vDeleteAtPurge:=m_iDeleteAtPurge, vContactName:=m_sContactName, vAddress1:=m_sAddress1, vAddress2:=m_sAddress2, vAddress3:=m_sAddress3, vAddress4:=m_sAddress4, vPostalCode:=m_sPostalCode, vAddressCountry:=m_iAddressCountry, vPhoneAreaCode:=m_sPhoneAreaCode, vPhoneNumber:=m_sPhoneNumber, vPhoneExtension:=m_sPhoneExtension, vFaxAreaCode:=m_sFaxAreaCode, vFaxNumber:=m_sFaxNumber, vFaxExtension:=m_sFaxExtension, vPaymentName:=m_sPaymentName, vPaymentAccountCode:=m_sPaymentAccountCode, vPaymentBranchCode:=m_sPaymentBranchCode, vPaymentExpiryDate:=m_dtPaymentExpiryDate, vPaymentReference1:=m_sPaymentReference1, vPaymentReference2:=m_sPaymentReference2, vCreditLimit:=m_vdCreditLimit, vDiscountPercentage:=m_vdDiscountPercentage, vSettlementPeriod:=m_iSettlementPeriod, vBankName:=m_sBankName, vBankAddress1:=m_sBankAddress1, vBankAddress2:=m_sBankAddress2, vBankAddress3:=m_sBankAddress3, vBankAddress4:=m_sBankAddress4, vBankPostalCode:=m_sBankPostalCode, vBankCountry:=m_iBankCountry, vBankPhoneAreaCode:=m_sBankPhoneAreaCode, vBankPhoneNumber:=m_sBankPhoneNumber, vBankPhoneExtension:=m_sBankPhoneExtension, vBankFaxAreaCode:=m_sBankFaxAreaCode, vBankFaxNumber:=m_sBankFaxNumber, vBankFaxExtension:=m_sBankFaxExtension, vComments:=m_sComments, vNominalAccountID:=m_lNominalCode, vAccountStatusID:=m_iAccountStatusID, vProofListReportID:=m_lProofListReportID, vBordereauReportID:=m_lBordereauReportID, vAllowElectronicPayment:=m_bAllowElectronicPayment, vMoneyCalcAccType:=m_iMoneyCalcAccType, vClientBankAccType:=m_vClientBankAccType, vParamArray:=vParamArray)

                    ' {* USER DEFINED CODE (End) *}

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Inform the business object with an updated data item.

                    ' {* USER DEFINED CODE (Begin) *}
                    ' RAW 17/12/2002 : PS187 : Added ProofListReportID and BordereauReportID
                    ' RDC 12112003 added AllowElectronicPayment

                    m_lReturn = m_oBusiness.EditUpdate(lRow:=lBusinessDataID, vAccountID:=m_lAccountID, vPurgefrequencyID:=m_iPurgefrequencyID, vCurrencyID:=m_iCurrencyID, vAccounttypeID:=m_iAccounttypeID, vLedgerID:=m_iLedgerID, vPaymenttypeID:=m_iPaymenttypeID, vAccountName:=m_sAccountName, vShortCode:=m_sShortCode, vRestrictEnquiry:=m_iRestrictEnquiry, vRestrictUpdate:=m_iRestrictUpdate, vDeleteAtPurge:=m_iDeleteAtPurge, vContactName:=m_sContactName, vAddress1:=m_sAddress1, vAddress2:=m_sAddress2, vAddress3:=m_sAddress3, vAddress4:=m_sAddress4, vPostalCode:=m_sPostalCode, vAddressCountry:=m_iAddressCountry, vPhoneAreaCode:=m_sPhoneAreaCode, vPhoneNumber:=m_sPhoneNumber, vPhoneExtension:=m_sPhoneExtension, vFaxAreaCode:=m_sFaxAreaCode, vFaxNumber:=m_sFaxNumber, vFaxExtension:=m_sFaxExtension, vPaymentName:=m_sPaymentName, vPaymentAccountCode:=m_sPaymentAccountCode, vPaymentBranchCode:=m_sPaymentBranchCode, vPaymentExpiryDate:=m_dtPaymentExpiryDate, vPaymentReference1:=m_sPaymentReference1, vPaymentReference2:=m_sPaymentReference2, vCreditLimit:=m_vdCreditLimit, vDiscountPercentage:=m_vdDiscountPercentage, vSettlementPeriod:=m_iSettlementPeriod, vBankName:=m_sBankName, vBankAddress1:=m_sBankAddress1, vBankAddress2:=m_sBankAddress2, vBankAddress3:=m_sBankAddress3, vBankAddress4:=m_sBankAddress4, vBankPostalCode:=m_sBankPostalCode, vBankCountry:=m_iBankCountry, vBankPhoneAreaCode:=m_sBankPhoneAreaCode, vBankPhoneNumber:=m_sBankPhoneNumber, vBankPhoneExtension:=m_sBankPhoneExtension, vBankFaxNumber:=m_sBankFaxNumber, vBankFaxExtension:=m_sBankFaxExtension, vComments:=m_sComments, vNominalAccountID:=m_lNominalCode, vAccountStatusID:=m_iAccountStatusID, vProofListReportID:=m_lProofListReportID, vBordereauReportID:=m_lBordereauReportID, vAllowElectronicPayment:=m_bAllowElectronicPayment, vMoneyCalcAccType:=m_iMoneyCalcAccType, vClientBankAccType:=m_vClientBankAccType, vParamArray:=vParamArray)


                    ' {* USER DEFINED CODE (End) *}
            End Select

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                If m_lReturn <> gPMConstants.PMEReturnCode.PMRecordInUse Then

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")

                Else

                    ' Dont bother displaying the message if the user is cancelling.
                    If m_lStatus <> gPMConstants.PMEReturnCode.PMCancel Then
                        MessageBox.Show("The short code '" & m_sShortCode.Trim() & "' has already be used. Please try another.", "Error - Short Code", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If

                End If

            Else

                ' Only ask to create task if not in Cancel mode
                If m_lStatus <> gPMConstants.PMEReturnCode.PMCancel Then

                    ' Check if we need to create a work manager task
                    If m_iAccountStatusID <> m_iOriginalAccountStatusID Then

                        m_lReturn = CType(CreateWorkManagerMemo(v_iAccountStatusID:=m_iAccountStatusID, v_sAccountCode:=m_sShortCode, v_sAccountName:=m_sAccountName), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    End If

                End If

                'Party Bank Details
                'If m_iTask = PMEdit Then
                If Information.IsArray(m_vPartyBankDetails) Then

                    m_lReturn = m_oPartyBank.UpdatePartyBankDetails(m_lAccountID, m_vPartyBankDetails)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the Update Party Bank Details", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
                    End If
                End If
                'End If

            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    '
    ' ***************************************************************** '
    Public Function DisplayLookupDetails() As Integer

        ' RAW 17/12/2002 : PS187 : Table index constants moved to module level constants
        'Const CAccountType = 0
        'Const CPurgeFrequency = 1
        'Const CAddressCountry = 2
        'Const CPaymentType = 3
        'Const CBankCountry = 4
        'Const CAccountStatus = 5

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            m_lReturn = CType(GetLookupValues(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get all of the lookup details.

            ' RAW 17/12/2002 : PS187 : constant renamed
            m_lReturn = CType(GetLookupDetails(iLookupTable:=m_kiTableAccountType, ctlLookup:=cboAccounttypeID), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' RAW 17/12/2002 : PS187 : constant renamed
            m_lReturn = CType(GetLookupDetails(iLookupTable:=m_kiTablePurgeFrequency, ctlLookup:=cboPurgefrequencyID), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' RAW 17/12/2002 : PS187 : constant renamed
            m_lReturn = CType(GetLookupDetails(iLookupTable:=m_kiTableAddressCountry, ctlLookup:=cboAddressCountry), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' RAW 17/12/2002 : PS187 : constant renamed
            m_lReturn = CType(GetLookupDetails(iLookupTable:=m_kiTableBankCountry, ctlLookup:=cboBankCountry), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' RAW 17/12/2002 : PS187 : constant renamed
            'm_lReturn& = GetLookupDetails( _
            ''    iLookupTable:=m_kiTablePaymentType, _
            ''    ctlLookup:=cboPaymentTypeID)

            ' Check for errors.
            'If (m_lReturn& <> PMTrue) Then
            '    DisplayLookupDetails = PMFalse
            '    Exit Function
            'End If

            ' RAW 17/12/2002 : PS187 : constant renamed
            m_lReturn = CType(GetLookupDetails(iLookupTable:=m_kiTableAccountStatus, ctlLookup:=cboAccountStatus), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' RAW 17/12/2002 : PS187 : Added
            m_lReturn = CType(GetLookupDetails(iLookupTable:=m_kiTableProofListReport, ctlLookup:=cboProofListReportID), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' RAW 17/12/2002 : PS187 : constant renamed
            m_lReturn = CType(GetLookupDetails(iLookupTable:=m_kiTableBordereauReport, ctlLookup:=cboBordereauReportID), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' RAW 17/12/2002 : PS187 : End

            Return result

        Catch excep As System.Exception



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: BusinessToData
    '
    ' Description: Updates the data storage from the business object.
    '
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Dim vParamArray() As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ReDim vParamArray(0)
            ' Assign the details to the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            ' RAW 17/12/2002 : PS187 : Added ProofListReportID & BordereauReportID
            ' RDC 12112003

            'Trim the string to remove spaces
            m_lReturn = m_oBusiness.GetNext(vAccountID:=m_lAccountID, vPurgefrequencyID:=m_iPurgefrequencyID, vAccountName:=m_sAccountName, vShortCode:=m_sShortCode, vRestrictEnquiry:=m_iRestrictEnquiry, vCurrencyID:=m_iCurrencyID, vAccounttypeID:=m_iAccounttypeID, vLedgerID:=m_iLedgerID, vPaymenttypeID:=m_iPaymenttypeID, vRestrictUpdate:=m_iRestrictUpdate, vDeleteAtPurge:=m_iDeleteAtPurge, vContactName:=m_sContactName, vAddress1:=m_sAddress1, vAddress2:=m_sAddress2, vAddress3:=m_sAddress3, vAddress4:=m_sAddress4, vPostalCode:=m_sPostalCode, vAddressCountry:=m_iAddressCountry, vPhoneAreaCode:=m_sPhoneAreaCode, vPhoneNumber:=m_sPhoneNumber, vPhoneExtension:=m_sPhoneExtension, vFaxAreaCode:=m_sFaxAreaCode, vFaxNumber:=m_sFaxNumber, vFaxExtension:=m_sFaxExtension, vPaymentName:=m_sPaymentName, vPaymentAccountCode:=m_sPaymentAccountCode, vPaymentBranchCode:=m_sPaymentBranchCode, vPaymentExpiryDate:=m_dtPaymentExpiryDate, vPaymentReference1:=m_sPaymentReference1, vPaymentReference2:=m_sPaymentReference2, vCreditLimit:=m_vdCreditLimit, vDiscountPercentage:=m_vdDiscountPercentage, vSettlementPeriod:=m_iSettlementPeriod, vBankName:=m_sBankName, vBankAddress1:=m_sBankAddress1, vBankAddress2:=m_sBankAddress2, vBankAddress3:=m_sBankAddress3, vBankAddress4:=m_sBankAddress4, vBankPostalCode:=m_sBankPostalCode, vBankCountry:=m_iBankCountry, vBankPhoneAreaCode:=m_sBankPhoneAreaCode, vBankPhoneNumber:=m_sBankPhoneNumber, vBankPhoneExtension:=m_sBankPhoneExtension, vBankFaxAreaCode:=m_sBankFaxAreaCode, vBankFaxNumber:=m_sBankFaxNumber, vBankFaxExtension:=m_sBankFaxExtension, vComments:=m_sComments, vNominalAccountID:=m_lNominalCode, vAccountStatusID:=m_iAccountStatusID, vProofListReportID:=m_lProofListReportID, vBordereauReportID:=m_lBordereauReportID, vAllowElectronicPayment:=m_bAllowElectronicPayment, vMoneyCalcAccType:=m_iMoneyCalcAccType, vClientBankAccType:=m_vClientBankAccType, vParamArray:=vParamArray)

            ' Save the account status as we need to find out if it's changed
            ' later on in order to create a work manager task or not.
            m_iOriginalAccountStatusID = m_iAccountStatusID
            If Information.IsArray(vParamArray) Then

                m_sMerchantId = CStr(vParamArray(ACMerchantIdArrPos))
            End If

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'Party Bank Details


            'Developer Guide No.119
            m_lReturn = m_oPartyBank.GetPartyBankDetails(vAccountID:=m_lAccountID, vPartyBankDetails:=m_vPartyBankDetails, vPartyCnt:=Nothing)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToData
    '
    ' Description: Updates the data storage from the interface details.
    '
    ' ***************************************************************** '
    Private Function InterfaceToData() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the data storage.


            'developer guide no 250
            m_iPurgefrequencyID = VB6.GetItemData(cboPurgefrequencyID, cboPurgefrequencyID.SelectedIndex)



            'developer guide no 250
            m_iAccounttypeID = VB6.GetItemData(cboAccounttypeID, cboAccounttypeID.SelectedIndex)
            m_iLedgerID = VB6.GetItemData(cboLedgerID, cboLedgerID.SelectedIndex)
            'developer guide no 250
            m_iPaymenttypeID = cboPaymentTypeID.ItemId

            'Developer Guide No 236
            'Start
            m_sAccountName = m_oFormFields.UnformatControl(txtAccountName)

            m_sShortCode = m_oFormFields.UnformatControl(txtShortCode)

            m_sContactName = m_oFormFields.UnformatControl(txtContactName)

            m_sAddress1 = m_oFormFields.UnformatControl(txtAddress1)

            m_sAddress2 = m_oFormFields.UnformatControl(txtAddress2)

            m_sAddress3 = m_oFormFields.UnformatControl(txtAddress3)

            m_sAddress4 = m_oFormFields.UnformatControl(txtAddress4)

            m_sPostalCode = m_oFormFields.UnformatControl(txtPostalCode)

            m_iAddressCountry = VB6.GetItemData(cboAddressCountry, cboAddressCountry.SelectedIndex)

            m_sPhoneAreaCode = m_oFormFields.UnformatControl(txtPhoneAreaCode)

            m_sPhoneNumber = m_oFormFields.UnformatControl(txtPhoneNumber)

            m_sPhoneExtension = m_oFormFields.UnformatControl(txtPhoneExtension)

            m_sFaxAreaCode = m_oFormFields.UnformatControl(txtFaxAreaCode)

            m_sFaxNumber = m_oFormFields.UnformatControl(txtFaxNumber)

            m_sPaymentName = m_oFormFields.UnformatControl(txtPaymentName)

            m_sPaymentAccountCode = m_oFormFields.UnformatControl(txtPaymentAccountCode)


            m_sPaymentBranchCode = m_oFormFields.UnformatControl(txtPaymentBranchCode)

            m_dtPaymentExpiryDate = CDate(m_oFormFields.UnformatControl(txtPaymentExpiryDate))

            m_sPaymentReference1 = m_oFormFields.UnformatControl(txtPaymentReference1)

            m_sPaymentReference2 = m_oFormFields.UnformatControl(txtPaymentReference2)


            m_lProofListReportID = VB6.GetItemData(cboProofListReportID, cboProofListReportID.SelectedIndex)


            m_lBordereauReportID = VB6.GetItemData(cboBordereauReportID, cboBordereauReportID.SelectedIndex) ' RAW 17/12/2002 : PS187 : Added



            m_vdCreditLimit = m_oFormFields.UnformatControl(txtCreditLimit)


            m_vdDiscountPercentage = m_oFormFields.UnformatControl(txtDiscountPercentage)

            m_iSettlementPeriod = CInt(m_oFormFields.UnformatControl(txtSettlementPeriod))

            m_sBankName = m_oFormFields.UnformatControl(txtBankName)

            m_sBankAddress1 = m_oFormFields.UnformatControl(txtBankAddress1)

            m_sBankAddress2 = m_oFormFields.UnformatControl(txtBankAddress2)

            m_sBankAddress3 = m_oFormFields.UnformatControl(txtBankAddress3)
            'eck041001 save Bank Address line 4

            m_sBankAddress4 = m_oFormFields.UnformatControl(txtBankAddress4)

            m_sBankPostalCode = m_oFormFields.UnformatControl(txtBankPostalCode)

            'developer guide no 250
            m_iBankCountry = VB6.GetItemData(cboBankCountry, cboBankCountry.SelectedIndex)

            m_sBankPhoneAreaCode = m_oFormFields.UnformatControl(txtBankPhoneAreaCode)

            m_sBankPhoneNumber = m_oFormFields.UnformatControl(txtBankPhoneNumber)

            m_sBankPhoneExtension = m_oFormFields.UnformatControl(txtBankPhoneExtension)

            m_sBankFaxAreaCode = m_oFormFields.UnformatControl(txtBankFaxAreaCode)

            m_sBankFaxNumber = m_oFormFields.UnformatControl(txtBankFaxNumber)

            m_sComments = m_oFormFields.UnformatControl(txtComments)

            'End
            m_iRestrictEnquiry = CInt(m_oFormFields.UnformatControl(chkRestrictEnquiry))

            m_iRestrictUpdate = CInt(m_oFormFields.UnformatControl(chkRestrictUpdate))

            m_iDeleteAtPurge = CInt(m_oFormFields.UnformatControl(chkDeleteAtPurge))
            ' RDC 12112003

            m_bAllowElectronicPayment = CBool(m_oFormFields.UnformatControl(chkElectronicPayment))

            ' CF150199
            m_lNominalCode = uctAccountLookup.AccountId

            ' CF040399

            m_iAccountStatusID = VB6.GetItemData(cboAccountStatus, cboAccountStatus.SelectedIndex)

            Select Case (cboMoneyCalcAccType.Text)
                Case "Client Bank Account"
                    m_iMoneyCalcAccType = 1
                Case "Client Money Suspense Account"
                    m_iMoneyCalcAccType = 2
                Case "Designated Investment Account"
                    m_iMoneyCalcAccType = 3
                Case Else
                    m_iMoneyCalcAccType = 0
            End Select

            Select Case (cboClientBankAccType.Text)
                Case "Statutory Trust Account"
                    m_vClientBankAccType = 0
                Case "Non-Statutory Trust Account"
                    m_vClientBankAccType = 1
                Case Else

                    m_vClientBankAccType = Nothing
            End Select


            m_sMerchantId = CStr(m_oFormFields.UnformatControl(txtMerchantId))

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

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
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = False

                Case Else
                    cmdNavigate.Visible = False
            End Select

            ' CF 040399
            ' Disable/Enable account status depending on user's authority level
            If m_lPMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALUser Then
                cboAccountStatus.Enabled = False
                lblAccountStatus.Enabled = False
            Else
                cboAccountStatus.Enabled = True
                lblAccountStatus.Enabled = True
            End If

            With cboMoneyCalcAccType
                .Items.Add("Other")
                .Items.Add("Client Bank Account")
                .Items.Add("Client Money Suspense Account")
                .Items.Add("Designated Investment Account")
            End With

            With cboClientBankAccType
                .Items.Add("Statutory Trust Account")
                .Items.Add("Non-Statutory Trust Account")
            End With


            m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_iCurrentTab = -1

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: SetMandatoryDetails
    '
    ' Description: Sets the mandatory details on the interface.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (SetMandatoryDetails) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function SetMandatoryDetails() As Integer
    '
    'Dim result As Integer = 0
    'Dim vAccountName As Object
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Get the mandatory details from the business object.

    'm_lReturn = m_oBusiness.GetMandatory(vAccountName:=vAccountName)
    '
    ' Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    '    ' Clear the collection
    '    m_lReturn = m_oGeneral.ResetMandatoryControls
    ''
    '    If (m_lReturn& <> PMTrue) Then
    '        SetMandatoryDetails = PMFalse
    '        Exit Function
    '    End If
    ''
    '    ' Set the mandatory details on the interface.
    ''
    '    If (vAccountName = PMMandatory) Then
    '        m_lReturn = m_oGeneral.SetMandatoryControl(txtAccountName)
    '    End If
    ''
    '    If m_lReturn <> PMTrue Then
    '      SetMandatoryDetails = PMFalse
    '      Exit Function
    '    End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the mandatory details", vApp:=ACApp, vClass:=ACClass, vMethod:="SetMandatoryDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
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
        Const CDetailsTab As Integer = 0
        Const CAddressTab As Integer = 1
        Const CPaymentTab As Integer = 2
        Const CBankTab As Integer = 3
        Const CCommentsTab As Integer = 4

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'ReDim m_ctlTabFirstLast(ACControlStart To ACControlEnd, _
            'CDetailsTab To CCommentsTab)
            m_ctlTabFirstLast = Array.CreateInstance(GetType(Control), New Integer() {ACControlEnd - ACControlStart + 1, CCommentsTab - CDetailsTab + 1}, New Integer() {ACControlStart, CDetailsTab})
            m_ctlTabFirstLast(ACControlStart, CDetailsTab) = txtShortCode
            m_ctlTabFirstLast(ACControlEnd, CDetailsTab) = cboAccountStatus
            m_ctlTabFirstLast(ACControlStart, CAddressTab) = txtContactName
            m_ctlTabFirstLast(ACControlEnd, CAddressTab) = txtFaxNumber
            m_ctlTabFirstLast(ACControlStart, CPaymentTab) = txtPaymentName
            m_ctlTabFirstLast(ACControlEnd, CPaymentTab) = txtSettlementPeriod
            m_ctlTabFirstLast(ACControlStart, CBankTab) = txtBankName
            m_ctlTabFirstLast(ACControlEnd, CBankTab) = txtBankFaxNumber
            m_ctlTabFirstLast(ACControlStart, CCommentsTab) = txtComments
            m_ctlTabFirstLast(ACControlEnd, CCommentsTab) = txtComments

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Private Function SetTabDefaults() As Integer

        Try

            With tabMainTab

                For iTab As Integer = 0 To pnlMain.Length - 1
                    pnlMain(iTab).Visible = iTab = SSTabHelper.GetSelectedIndex(tabMainTab)
                Next iTab

                ' Set the default button.
                If SSTabHelper.GetSelectedIndex(tabMainTab) < cmdNext.Length Then
                    VB6.SetDefault(cmdNext(SSTabHelper.GetSelectedIndex(tabMainTab)), True)
                Else
                    VB6.SetDefault(cmdOK, True)
                End If

                ' Now I know this is crap, this goes against
                ' all my principles, but for some reason when
                ' using the mouse to select a tab the setfocus
                ' code below doesn't work. The cursor sticks,
                ' and you can't tab off. Therefore I've used
                ' this to get around the problem.
                Application.DoEvents()

                ' Set focus to the first control on the tab.
                If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                    If m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Visible And m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Enabled Then
                        m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                    End If
                End If
                m_iCurrentTab = SSTabHelper.GetSelectedIndex(tabMainTab)
            End With

        Catch



            ' Error Section.

            Exit Function
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

            ' Display all language specific captions


            'developer guide no. 243
            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ToolTip1.SetToolTip(cmdOK, CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKTip, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMainTabTitle0, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 1, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMainTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 2, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMainTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 3, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMainTabTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 4, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMainTabTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Party Bank Details

            SSTabHelper.SetTabCaption(tabMainTab, 5, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMainTabTitle5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            cmdNext(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNextCaption0, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPurgefrequencyID.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPurgefrequencyIDCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblRestrictEnquiry.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRestrictEnquiryCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblRestrictUpdate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRestrictUpdateCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblDeleteAtPurge.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteAtPurgeCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAccounttypeID.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAccounttypeIDCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblLedgerID.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLedgerIDCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAccountCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAccountCodeCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAccountName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAccountNameCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblShortCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACShortCodeCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' CF150199

            lblNominalCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNominalCodeCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' CF050399

            lblAccountStatus.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAccountStatusCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            cmdNext(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNextCaption1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblExtension.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACExtensionCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblContactName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACContactNameCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAddress1.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddress1Caption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAddress2.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddress2Caption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAddresss3.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddresss3Caption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAddress4.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddress4Caption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPostalCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPostalCodeCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAddressCountry.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressCountryCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblTelephone.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTelephoneCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblFax.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFaxCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAreaCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAreaCodeCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNumberCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            fraSettlementTerms.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSettlementTermsCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            cmdNext(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNextCaption2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblDays.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDaysCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblSettlementPeriod.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSettlementPeriodCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblDiscountPercentage.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDiscountPercentageCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblCreditLimit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCreditLimitCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPaymentAccountCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPaymentAccountCodeCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPaymentBranchCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPaymentBranchCodeCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPaymentExpiryDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPaymentExpiryDateCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPaymentReference2.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPaymentReference2Caption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPaymentReference1.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPaymentReference1Caption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' RAW 17/12/2002 : PS187 : Added

            lblProofListReportID.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACProofListReportIDCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblBordereauReportID.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBordereauReportIDCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            ' RAW 17/12/2002 : PS187 : End


            lblPaymenttypeID.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPaymenttypeIDCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPaymentName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPaymentNameCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            cmdNext(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNextCaption3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblBankNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBankNumberCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblBankAreaCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBankAreaCodeCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblBankFax.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBankFaxCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblBankPhone.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBankPhoneCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblBankCountry.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBankCountryCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblBankPostalCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBankPostalCodeCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblBankAddress4.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBankAddress4Caption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblBankAddress3.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBankAddress3Caption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblBankAddress2.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBankAddress2Caption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblBankAddress1.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBankAddress1Caption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblBankName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBankNameCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblBankExtension.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBankExtensionCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblMoneyCalcAccType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClientMoneyCalcAccTye, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblClientBankAccType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClientBankAccType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Start - Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling

            m_sLinkedAccountsTitle5 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMainTabTitle6, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            SSTabHelper.SetTabCaption(tabMainTab, 6, m_sLinkedAccountsTitle5)


            m_sLinkedAccountsTitle7 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMainTabTitle7, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            fraCashDeposit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCashDeposit, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'End - Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling

            ' *** END Inserted By ResGen ***

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Private Function GetLookupValues() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Gets all of the lookup values.

            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Get all of the lookup values.

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Get all of the lookup values with the correct
                    ' effective date.

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMView
                    ' Get lookup values for viewing only.

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
            End Select

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")

                Return result
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
    ' Name: GetLedgerDetails
    '
    ' Description: Gets ledger details for this company
    '              then assigns them to the list or combo control passed.
    '
    ' ***************************************************************** '
    Private Function GetLedgerDetails(ByRef ctlLookup As ComboBox) As Integer

        ' Constants for ledger details

        Dim result As Integer = 0
        Const CLedgerID As Integer = 0
        Const CLedgerName As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get array containing list of ledger dets

            m_lReturn = m_oBusiness.GetLedgerDetails(vResultArray:=m_vLedgerDetails)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If
            Dim newIndex As Integer = -1
            For lLedgerRow As Integer = 0 To m_vLedgerDetails.GetUpperBound(0) - 1

                'developer guide no 28

                newIndex = ctlLookup.Items.Add(New VB6.ListBoxItem(m_vLedgerDetails(lLedgerRow, CLedgerName), CInt(m_vLedgerDetails(lLedgerRow, CLedgerID))))
                If CInt(m_vLedgerDetails(lLedgerRow, CLedgerID)) = m_iLedgerID Then

                    'developer guide no 28
                    ctlLookup.SelectedIndex = newIndex

                    'DD 07/05/2003: Removed so that all Accounts forms are the same.
                    'If CInt(m_vLedgerDetails(lLedgerRow, CLedgerType)) = ACTLedgerTypeGeneral Then
                    ' For General ledger accounts only enable tabs 1 and 5
                    '    For iTab = 1 To tabMainTab.Tabs - 2
                    '        tabMainTab.TabEnabled(iTab) = False
                    '        tabMainTab.TabVisible(iTab) = False
                    '        cmdNext(0).Visible = False
                    '    Next iTab
                    'End If

                End If

            Next lLedgerRow

            ' CF060199 - Added check if if it doesnt have a Ledger
            'eck310700 - Removed this check at request of MCIS
            '  If (m_iTask% = PMEdit) And (cboLedgerID.ListIndex <> -1) Then
            '    ctlLookup.Enabled = False 'can't change an existing account's ledger
            '  End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLedgerDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

    Private Function GetLookupDetails(ByRef iLookupTable As Integer, ByRef ctlLookup As ComboBox) As Integer

        Dim result As Integer = 0
        Dim lRow, lCountryId, lDefaultCountryIndex As Integer

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

            lRow = iLookupTable

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.

            ' RAW 17/12/2002 : PS187 : Added
            'Add an entry for "none" where applicable
            Select Case iLookupTable
                Case m_kiTableProofListReport, m_kiTableBordereauReport

                    'developer guide no. 28
                    ctlLookup.Items.Add(New VB6.ListBoxItem("*none*", 0))
                Case m_kiTableAddressCountry, m_kiTableBankCountry

                    If CStr(m_vLookupValues(ACValueID, lRow)) = "" Then


                        m_lReturn = m_oBusiness.GetBaseCountry(v_lAccountId:=m_lAccountID, r_lCountryId:=lCountryId)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                            Return result
                        End If

                    End If

            End Select
            ' RAW 17/12/2002 : PS187 : end
            Dim newindex As Integer = -1
            For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)

                ' Add the details to the control.

                'developer guide no. 28
                newindex = ctlLookup.Items.Add(New VB6.ListBoxItem(CStr(m_vLookupDetails(ACDetailDesc, lCntr)), CInt(m_vLookupDetails(ACDetailKey, lCntr))))

                ' Check if this is the selected index.
                If m_vLookupValues(ACValueID, lRow).Equals(m_vLookupDetails(ACDetailKey, lCntr)) Then


                    'developer guide no. 28
                    ctlLookup.SelectedIndex = newindex

                End If

                If (iLookupTable = m_kiTableAddressCountry Or iLookupTable = m_kiTableBankCountry) And (CInt(m_vLookupDetails(ACDetailKey, lCntr)) = lCountryId) Then

                    'developer guide no. 28
                    lDefaultCountryIndex = newindex
                End If

            Next lCntr

            If CStr(m_vLookupValues(ACValueID, lRow)) = "" Then
                If iLookupTable = m_kiTableAddressCountry Or iLookupTable = m_kiTableBankCountry Then

                    'developer guide no. 28
                    ctlLookup.SelectedIndex = lDefaultCountryIndex

                Else

                    'developer guide no. 28
                    ctlLookup.SelectedIndex = 0

                End If
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
    ' PRIVATE Methods (End)


    Private Sub cboAddressCountry_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboAddressCountry.SelectedIndexChanged
        'TN20010702 - start (hide postcode if not required)
        If cboAddressCountry.Text.ToUpper() <> "UNITED KINGDOM" Then
            lblPostalCode.Visible = False
            txtPostalCode.Visible = False
        Else
            lblPostalCode.Visible = True
            txtPostalCode.Visible = True
        End If
        'TN20010702 - end
    End Sub

    Private Sub cboLedgerID_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboLedgerID.SelectedIndexChanged

        ' CF150199 - Only enable nominal ledger for purchase ledger ac/s
        'DC220703 -ISS5502 -added trim
        If cboLedgerID.Text.Trim().ToLower() = "purchase" Then
            lblNominalCode.Enabled = True
            uctAccountLookup.Enabled = True
            'DC220703 -ISS5502
            SSTabHelper.SetTabVisible(tabMainTab, 3, True)
            SSTabHelper.SetTabVisible(tabMainTab, 4, True)
            cmdNext(2).Visible = True
            SSTabHelper.SetTabCaption(tabMainTab, 6, m_sLinkedAccountsTitle7) 'Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
        Else
            lblNominalCode.Enabled = False
            uctAccountLookup.Enabled = False
            'DC220703 -ISS5502
            SSTabHelper.SetTabVisible(tabMainTab, 3, False)
            SSTabHelper.SetTabVisible(tabMainTab, 4, False)
            'cmdNext(2).Visible = False
            SSTabHelper.SetTabCaption(tabMainTab, 6, m_sLinkedAccountsTitle5) 'Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
        End If

    End Sub

    Private Sub cboMoneyCalcAccType_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboMoneyCalcAccType.SelectionChangeCommitted
        If cboMoneyCalcAccType.Text = "Other" Then

            m_vClientBankAccType = Nothing
            cboClientBankAccType.Text = ""
            lblClientBankAccType.Enabled = False
            cboClientBankAccType.Enabled = False
        Else
            lblClientBankAccType.Enabled = True
            cboClientBankAccType.Enabled = True
            cboClientBankAccType.SelectedIndex = 0
        End If
    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen
        'Developer Guide:184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID), gPMConstants.PMEReturnCode)


    End Sub

    Private Sub cmdPrevious_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdPrevious_5.Click, _cmdPrevious_4.Click, _cmdPrevious_0.Click, _cmdPrevious_2.Click, _cmdPrevious_3.Click, _cmdPrevious_1.Click
        Dim Index As Integer = Array.IndexOf(cmdPrevious, eventSender)

        Try

            'change to previous tab
            If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
                'Party Bank Details
                If (cboLedgerID.Text.Trim().ToLower() <> "purchase") And SSTabHelper.GetSelectedIndex(tabMainTab) = 5 Then
                    SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) - 3)
                Else
                    SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) - 1)
                End If
            End If

            ' Set focus to the first control on the tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                m_ctlTabFirstLast(ACControlStart, Index + 1).Focus()
            End If

        Catch



            Exit Sub
        End Try


    End Sub

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            If m_iCurrentTab < 0 Then
                SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                SetTabDefaults()
            End If

        End If
    End Sub

    Private Sub Form_Initialize_Renamed()

        'DD 15/07/2002: Get product option setting
        Dim vValue As String = ""
        iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTEnhancedOrionSecurity, m_iCompanyID, vValue)
        m_bEnhancedSecurity = (vValue = "1")

        ' RDC 13112003
        iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTAllowElectronicPayment, m_iCompanyID, vValue)
        m_bElectronicPayment = (vValue = "1")
    End Sub

    ' PRIVATE Events (Begin)

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            'Developer Guide: 119(no solution)
            If UnloadMode <> vbFormCode Then

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)


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



            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Terminate the business object

            m_oBusiness.Dispose()



            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing
            'EK091298
            ' Terminate the explorer business object

            m_oExplorer.Dispose()



            ' Destroy the instance of the explorer business object
            ' from memory.
            m_oExplorer = Nothing


            m_oPartyBank.Dispose()



            ' Destroy the instance of the explorer business object
            ' from memory.
            m_oPartyBank = Nothing


            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Error Section.

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

        Const ACCtrlMask As Integer = 2

        Try

            ' Set the control key value.
            iCtrlDown = (Shift And ACCtrlMask) > 0

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
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) - 1)
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
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) + 1)
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
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D6 Then
                tabMainTab.SelectedIndex = 5
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D7 Then
                tabMainTab.SelectedIndex = 6
            End If
        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub


    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged


        Try

            SetTabDefaults()

        Catch



            ' Error Section.


            tabMainTabPreviousTab = tabMainTab.SelectedIndex
        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            m_lReturn = m_oFormFields.CheckMandatoryControls()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            If Not ValidateMediaDetails() Then
                Exit Sub
            End If

            ' Process the next set of actions depending
            ' upon the interface task etc.

            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

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

            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)


            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click

        ' Click event of the Navigate button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMNavigate

            ' Process the next set of actions depending
            ' upon the interface task etc.

            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)


            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNext_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdNext_5.Click, _cmdNext_4.Click, _cmdNext_0.Click, _cmdNext_1.Click, _cmdNext_3.Click, _cmdNext_2.Click
        Dim Index As Integer = Array.IndexOf(cmdNext, eventSender)

        Try

            ' Change to the next tab.
            'Party Bank Details
            If (cboLedgerID.Text.Trim().ToLower() <> "purchase") And SSTabHelper.GetSelectedIndex(tabMainTab) = 2 Then
                Index += 2
            End If
            If SSTabHelper.GetSelectedIndex(tabMainTab) < SSTabHelper.GetTabCount(tabMainTab) - 1 Then
                'Start - Sankar -(UIIC_WPR85_Cash_Deposit_Process) - Paralleling
                If SSTabHelper.GetTabEnabled(tabMainTab, Index + 1) Then
                    SSTabHelper.SetSelectedIndex(tabMainTab, Index + 1)
                End If
                'End - Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
            End If

            ' Set focus to the first control on the tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                m_ctlTabFirstLast(ACControlStart, Index + 1).Focus()
            End If

        Catch



            ' Error Section

            Exit Sub
        End Try


    End Sub

    Private Sub cboPurgefrequencyID_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPurgefrequencyID.Enter
        m_lReturn = m_oFormFields.GotFocus(cboPurgefrequencyID)
    End Sub
    Private Sub cboPurgefrequencyID_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPurgefrequencyID.Leave
        m_lReturn = m_oFormFields.LostFocus(cboPurgefrequencyID)
    End Sub
    Private Sub cboAccounttypeID_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboAccounttypeID.Enter
        m_lReturn = m_oFormFields.GotFocus(cboAccounttypeID)
    End Sub
    Private Sub cboAccounttypeID_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboAccounttypeID.Leave
        m_lReturn = m_oFormFields.LostFocus(cboAccounttypeID)
    End Sub
    Private Sub cboLedgerID_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboLedgerID.Enter
        m_lReturn = m_oFormFields.GotFocus(cboLedgerID)
    End Sub
    Private Sub cboLedgerID_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboLedgerID.Leave
        m_lReturn = m_oFormFields.LostFocus(cboLedgerID)
    End Sub
    Private Sub cboPaymenttypeID_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPaymentTypeID.GotFocus
        m_lReturn = m_oFormFields.GotFocus(cboPaymentTypeID)
    End Sub
    Private Sub cboPaymenttypeID_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPaymentTypeID.LostFocus
        m_lReturn = m_oFormFields.LostFocus(cboPaymentTypeID)
    End Sub
    Private Sub txtAccountName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAccountName.Enter
        m_lReturn = m_oFormFields.GotFocus(txtAccountName)
    End Sub
    Private Sub txtAccountName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAccountName.Leave
        m_lReturn = m_oFormFields.LostFocus(txtAccountName)
    End Sub
    Private Sub txtShortCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtShortCode.Enter
        m_lReturn = m_oFormFields.GotFocus(txtShortCode)
    End Sub
    Private Sub txtShortCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtShortCode.Leave
        m_lReturn = m_oFormFields.LostFocus(txtShortCode)
    End Sub
    Private Sub chkRestrictEnquiry_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkRestrictEnquiry.Enter
        m_lReturn = m_oFormFields.GotFocus(chkRestrictEnquiry)
    End Sub
    Private Sub chkRestrictEnquiry_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkRestrictEnquiry.Leave
        m_lReturn = m_oFormFields.LostFocus(chkRestrictEnquiry)
    End Sub
    Private Sub chkRestrictUpdate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkRestrictUpdate.Enter
        m_lReturn = m_oFormFields.GotFocus(chkRestrictUpdate)
    End Sub
    Private Sub chkRestrictUpdate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkRestrictUpdate.Leave
        m_lReturn = m_oFormFields.LostFocus(chkRestrictUpdate)
    End Sub
    Private Sub chkDeleteAtPurge_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkDeleteAtPurge.Enter
        m_lReturn = m_oFormFields.GotFocus(chkDeleteAtPurge)
    End Sub
    Private Sub chkDeleteAtPurge_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkDeleteAtPurge.Leave
        m_lReturn = m_oFormFields.LostFocus(chkDeleteAtPurge)
    End Sub
    Private Sub txtContactName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtContactName.Enter
        m_lReturn = m_oFormFields.GotFocus(txtContactName)
    End Sub
    Private Sub txtContactName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtContactName.Leave
        m_lReturn = m_oFormFields.LostFocus(txtContactName)
    End Sub
    Private Sub txtAddress1_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAddress1.Enter
        m_lReturn = m_oFormFields.GotFocus(txtAddress1)
    End Sub
    Private Sub txtAddress1_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAddress1.Leave
        m_lReturn = m_oFormFields.LostFocus(txtAddress1)
    End Sub
    Private Sub txtAddress2_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAddress2.Enter
        m_lReturn = m_oFormFields.GotFocus(txtAddress2)
    End Sub
    Private Sub txtAddress2_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAddress2.Leave
        m_lReturn = m_oFormFields.LostFocus(txtAddress2)
    End Sub
    Private Sub txtAddress3_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAddress3.Enter
        m_lReturn = m_oFormFields.GotFocus(txtAddress3)
    End Sub
    Private Sub txtAddress3_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAddress3.Leave
        m_lReturn = m_oFormFields.LostFocus(txtAddress3)
    End Sub
    Private Sub txtAddress4_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAddress4.Enter
        m_lReturn = m_oFormFields.GotFocus(txtAddress4)
    End Sub
    Private Sub txtAddress4_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAddress4.Leave
        m_lReturn = m_oFormFields.LostFocus(txtAddress4)
    End Sub
    Private Sub txtPostalCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPostalCode.Enter
        m_lReturn = m_oFormFields.GotFocus(txtPostalCode)
    End Sub
    Private Sub txtPostalCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPostalCode.Leave
        m_lReturn = m_oFormFields.LostFocus(txtPostalCode)
    End Sub
    Private Sub cboAddressCountry_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboAddressCountry.Enter
        m_lReturn = m_oFormFields.GotFocus(cboAddressCountry)
    End Sub
    Private Sub cboAddressCountry_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboAddressCountry.Leave
        m_lReturn = m_oFormFields.LostFocus(cboAddressCountry)
    End Sub
    Private Sub txtPhoneAreaCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPhoneAreaCode.Enter
        m_lReturn = m_oFormFields.GotFocus(txtPhoneAreaCode)
    End Sub
    Private Sub txtPhoneAreaCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPhoneAreaCode.Leave
        m_lReturn = m_oFormFields.LostFocus(txtPhoneAreaCode)
    End Sub
    Private Sub txtPhoneNumber_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPhoneNumber.Enter
        m_lReturn = m_oFormFields.GotFocus(txtPhoneNumber)
    End Sub
    Private Sub txtPhoneNumber_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPhoneNumber.Leave
        m_lReturn = m_oFormFields.LostFocus(txtPhoneNumber)
    End Sub
    Private Sub txtPhoneExtension_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPhoneExtension.Enter
        m_lReturn = m_oFormFields.GotFocus(txtPhoneExtension)
    End Sub
    Private Sub txtPhoneExtension_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPhoneExtension.Leave
        m_lReturn = m_oFormFields.LostFocus(txtPhoneExtension)
    End Sub
    Private Sub txtFaxAreaCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFaxAreaCode.Enter
        m_lReturn = m_oFormFields.GotFocus(txtFaxAreaCode)
    End Sub
    Private Sub txtFaxAreaCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFaxAreaCode.Leave
        m_lReturn = m_oFormFields.LostFocus(txtFaxAreaCode)
    End Sub
    Private Sub txtFaxNumber_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFaxNumber.Enter
        m_lReturn = m_oFormFields.GotFocus(txtFaxNumber)
    End Sub
    Private Sub txtFaxNumber_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFaxNumber.Leave
        m_lReturn = m_oFormFields.LostFocus(txtFaxNumber)
    End Sub
    Private Sub txtPaymentName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPaymentName.Enter
        m_lReturn = m_oFormFields.GotFocus(txtPaymentName)
    End Sub
    Private Sub txtPaymentName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPaymentName.Leave
        m_lReturn = m_oFormFields.LostFocus(txtPaymentName)
    End Sub
    Private Sub txtPaymentAccountCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPaymentAccountCode.Enter
        m_lReturn = m_oFormFields.GotFocus(txtPaymentAccountCode)
    End Sub
    Private Sub txtPaymentAccountCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPaymentAccountCode.Leave
        m_lReturn = m_oFormFields.LostFocus(txtPaymentAccountCode)
    End Sub
    Private Sub txtPaymentBranchCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPaymentBranchCode.Enter
        m_lReturn = m_oFormFields.GotFocus(txtPaymentBranchCode)
    End Sub
    Private Sub txtPaymentBranchCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPaymentBranchCode.Leave
        m_lReturn = m_oFormFields.LostFocus(txtPaymentBranchCode)
    End Sub
    Private Sub txtPaymentExpiryDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPaymentExpiryDate.Enter
        m_lReturn = m_oFormFields.GotFocus(txtPaymentExpiryDate)
    End Sub
    Private Sub txtPaymentExpiryDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPaymentExpiryDate.Leave
        m_lReturn = m_oFormFields.LostFocus(txtPaymentExpiryDate)
    End Sub
    Private Sub txtPaymentReference1_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPaymentReference1.Enter
        m_lReturn = m_oFormFields.GotFocus(txtPaymentReference1)
    End Sub
    Private Sub txtPaymentReference1_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPaymentReference1.Leave
        m_lReturn = m_oFormFields.LostFocus(txtPaymentReference1)
    End Sub
    Private Sub txtPaymentReference2_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPaymentReference2.Enter
        m_lReturn = m_oFormFields.GotFocus(txtPaymentReference2)
    End Sub
    Private Sub txtPaymentReference2_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPaymentReference2.Leave
        m_lReturn = m_oFormFields.LostFocus(txtPaymentReference2)
    End Sub
    ' RAW 17/12/2002 : PS187 : Added
    Private Sub cboProofListReportID_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboProofListReportID.Enter
        m_lReturn = m_oFormFields.GotFocus(cboProofListReportID)
    End Sub
    Private Sub cboProofListReportID_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboProofListReportID.Leave
        m_lReturn = m_oFormFields.LostFocus(cboProofListReportID)
    End Sub
    Private Sub cboBordereauReportID_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBordereauReportID.Enter
        m_lReturn = m_oFormFields.GotFocus(cboBordereauReportID)
    End Sub
    Private Sub cboBordereauReportID_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBordereauReportID.Leave
        m_lReturn = m_oFormFields.LostFocus(cboBordereauReportID)
    End Sub
    ' RAW 17/12/2002 : PS187 : End
    Private Sub txtCreditLimit_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCreditLimit.Enter
        m_lReturn = m_oFormFields.GotFocus(txtCreditLimit)
    End Sub
    Private Sub txtCreditLimit_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCreditLimit.Leave
        m_lReturn = m_oFormFields.LostFocus(txtCreditLimit)
    End Sub
    Private Sub txtDiscountPercentage_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDiscountPercentage.Enter
        m_lReturn = m_oFormFields.GotFocus(txtDiscountPercentage)
    End Sub
    Private Sub txtDiscountPercentage_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDiscountPercentage.Leave
        m_lReturn = m_oFormFields.LostFocus(txtDiscountPercentage)
    End Sub
    Private Sub txtSettlementPeriod_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSettlementPeriod.Enter
        m_lReturn = m_oFormFields.GotFocus(txtSettlementPeriod)
    End Sub
    Private Sub txtSettlementPeriod_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSettlementPeriod.Leave
        m_lReturn = m_oFormFields.LostFocus(txtSettlementPeriod)
    End Sub
    Private Sub txtBankName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankName.Enter
        m_lReturn = m_oFormFields.GotFocus(txtBankName)
    End Sub
    Private Sub txtBankName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankName.Leave
        m_lReturn = m_oFormFields.LostFocus(txtBankName)
    End Sub
    Private Sub txtBankAddress1_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankAddress1.Enter
        m_lReturn = m_oFormFields.GotFocus(txtBankAddress1)
    End Sub
    Private Sub txtBankAddress1_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankAddress1.Leave
        m_lReturn = m_oFormFields.LostFocus(txtBankAddress1)
    End Sub
    Private Sub txtBankAddress2_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankAddress2.Enter
        m_lReturn = m_oFormFields.GotFocus(txtBankAddress2)
    End Sub
    Private Sub txtBankAddress2_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankAddress2.Leave
        m_lReturn = m_oFormFields.LostFocus(txtBankAddress2)
    End Sub
    Private Sub txtBankAddress3_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankAddress3.Enter
        m_lReturn = m_oFormFields.GotFocus(txtBankAddress3)
    End Sub
    Private Sub txtBankAddress3_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankAddress3.Leave
        m_lReturn = m_oFormFields.LostFocus(txtBankAddress3)
    End Sub
    Private Sub txtBankAddress4_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankAddress4.Enter
        m_lReturn = m_oFormFields.GotFocus(txtBankAddress4)
    End Sub
    Private Sub txtBankAddress4_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankAddress4.Leave
        m_lReturn = m_oFormFields.LostFocus(txtBankAddress4)
    End Sub
    Private Sub txtBankPostalCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankPostalCode.Enter
        m_lReturn = m_oFormFields.GotFocus(txtBankPostalCode)
    End Sub
    Private Sub txtBankPostalCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankPostalCode.Leave
        m_lReturn = m_oFormFields.LostFocus(txtBankPostalCode)
    End Sub
    Private Sub cboBankCountry_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBankCountry.Enter
        m_lReturn = m_oFormFields.GotFocus(cboBankCountry)
    End Sub
    Private Sub cboBankCountry_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBankCountry.Leave
        m_lReturn = m_oFormFields.LostFocus(cboBankCountry)
    End Sub
    Private Sub txtBankPhoneAreaCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankPhoneAreaCode.Enter
        m_lReturn = m_oFormFields.GotFocus(txtBankPhoneAreaCode)
    End Sub
    Private Sub txtBankPhoneAreaCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankPhoneAreaCode.Leave
        m_lReturn = m_oFormFields.LostFocus(txtBankPhoneAreaCode)
    End Sub
    Private Sub txtBankPhoneNumber_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankPhoneNumber.Enter
        m_lReturn = m_oFormFields.GotFocus(txtBankPhoneNumber)
    End Sub
    Private Sub txtBankPhoneNumber_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankPhoneNumber.Leave
        m_lReturn = m_oFormFields.LostFocus(txtBankPhoneNumber)
    End Sub
    Private Sub txtBankPhoneExtension_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankPhoneExtension.Enter
        m_lReturn = m_oFormFields.GotFocus(txtBankPhoneExtension)
    End Sub
    Private Sub txtBankPhoneExtension_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankPhoneExtension.Leave
        m_lReturn = m_oFormFields.LostFocus(txtBankPhoneExtension)
    End Sub
    Private Sub txtBankFaxAreaCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankFaxAreaCode.Enter
        m_lReturn = m_oFormFields.GotFocus(txtBankFaxAreaCode)
    End Sub
    Private Sub txtBankFaxAreaCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankFaxAreaCode.Leave
        m_lReturn = m_oFormFields.LostFocus(txtBankFaxAreaCode)
    End Sub
    Private Sub txtBankFaxNumber_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankFaxNumber.Enter
        m_lReturn = m_oFormFields.GotFocus(txtBankFaxNumber)
    End Sub
    Private Sub txtBankFaxNumber_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankFaxNumber.Leave
        m_lReturn = m_oFormFields.LostFocus(txtBankFaxNumber)
    End Sub
    Private Sub txtComments_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtComments.Enter
        m_lReturn = m_oFormFields.GotFocus(txtComments)
    End Sub
    Private Sub txtComments_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtComments.Leave
        m_lReturn = m_oFormFields.LostFocus(txtComments)
    End Sub
    Private Sub txtAccountCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAccountCode.Enter
        iPMFunc.SelectText(txtAccountCode)
    End Sub
    Private Sub cboAccountStatus_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboAccountStatus.Leave
        m_lReturn = m_oFormFields.LostFocus(cboAccountStatus)
    End Sub
    Private Sub cboAccountStatus_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboAccountStatus.Enter
        m_lReturn = m_oFormFields.GotFocus(cboAccountStatus)
    End Sub
    ' RDC 12112003
    Private Sub chkElectronicPayment_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkElectronicPayment.Leave
        m_lReturn = m_oFormFields.LostFocus(chkElectronicPayment)
    End Sub
    Private Sub chkElectronicPayment_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkElectronicPayment.Enter
        m_lReturn = m_oFormFields.GotFocus(chkElectronicPayment)
    End Sub

    Private Function ValidateMediaDetails() As Boolean
        Dim result As Boolean = False
        Dim bSIRMediaTypeValidation As Object

        Dim oValidation As bSIRMediaTypeValidation.Business
        Dim vlMediaID As Integer
        Dim vlCountryID As Integer
        Dim vValid As Integer
        Dim sBankName, sAddress1, sAddress2, sAddress3, sAddress4, sPostalCode As String
        Dim vValidationMessage As Object
        Dim bValidationOverridable As Boolean
        Dim sStrippedString As String = ""

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            vlMediaID = cboPaymentTypeID.ItemId

            vlCountryID = g_oObjectManager.CountryID

            Dim temp_oValidation As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oValidation, "bSIRMediaTypeValidation.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oValidation = temp_oValidation

            sStrippedString = txtPaymentBranchCode.Text.Replace(" ", "") & "|" & _
                              txtPaymentAccountCode.Text.Replace(" ", "")


            oValidation.ValidateNumber(vlMediaID, vlCountryID, sStrippedString, vValid, sBankName, sAddress1, sAddress2, sAddress3, sAddress4, sPostalCode, vValidationMessage, bValidationOverridable)


            'if vValid = false then check for ValidationMessage and store all of
            'them in a string
            Dim sMessage, IsValid As String
            If Not vValid Then
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
                        txtBankAddress1.Text = sAddress1
                        txtBankAddress2.Text = sAddress2
                        txtBankAddress3.Text = sAddress3
                        txtBankAddress4.Text = sAddress4
                        txtBankPostalCode.Text = sPostalCode
                        result = True
                    Else
                        result = False
                    End If
                ElseIf Not bValidationOverridable Then
                    MessageBox.Show(sMessage, "Bank Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    result = gPMConstants.PMEReturnCode.PMFalse
                    txtBankName.Text = sBankName
                    txtBankAddress1.Text = sAddress1
                    txtBankAddress2.Text = sAddress2
                    txtBankAddress3.Text = sAddress3
                    txtBankAddress4.Text = sAddress4
                    txtBankPostalCode.Text = sPostalCode
                    'cmdOK.Enabled = False
                End If
                SSTabHelper.SetTabVisible(tabMainTab, 2, True)
                SSTabHelper.SetSelectedIndex(tabMainTab, 2)
            Else
                txtBankName.Text = sBankName
                txtBankAddress1.Text = sAddress1
                txtBankAddress2.Text = sAddress2
                txtBankAddress3.Text = sAddress3
                txtBankAddress4.Text = sAddress4
                txtBankPostalCode.Text = sPostalCode
                result = True
            End If

            oValidation = Nothing



        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process ValidateMediaDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateMediaDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

            result = gPMConstants.PMEReturnCode.PMFalse

        Finally



        End Try
        Return result
    End Function

    'UPGRADE_NOTE: (7001) The following declaration (uctPartyBankControl1_RefreshBankDetails) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    Private Sub uctPartyBankControl2_RefreshBankDetails(ByVal eventSender As Object, ByVal e As uctPartyBank.uctPartyBankControl.RefreshBankDetailsEventArgs) Handles uctPartyBankControl2.RefreshBankDetails
        m_vPartyBankDetails = e.vBankDetails
    End Sub
End Class
