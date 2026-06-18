Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Globalization
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctCreditCard_NET.uctCreditCard")>
Partial Public Class uctCreditCard
    Inherits System.Windows.Forms.UserControl
    Implements IDisposable
    '
    ' History : CJB 04/01/2005 User Control created for Perkins Slade - Retail Logic Integration
    '                          For correct operation of this control ensure the parent sets any
    '                          property variables and then calls the Initialise method.
    '
    '                          Note that the control is in ViewOnly mode (controls disabled) by
    '                          default until a value (other than 0) is set for MediaTypeID.
    '
    '                          If the following properties are changed (after Initialise has been
    '                          called then it will trigger the control to clear all data and reload
    '                          itself (as long as it is not in ViewOnly mode):
    '                               MediaTypeID
    '                               MediaTypeIssuerID
    '                               AccountID
    '                               ViewOnlyMode (this will just cause a reload without a clear)
    '
    '                          Note that IsPayment must be set before the Initialise method is called
    '                          and will greatly effect the display, behaviour and validation within the
    '                          control.  Set to True if Payments are to be made and False if Receipts
    '                          are to be made.
    '
    '                          FYI at the time of writing this control has been integrated with
    '                          iACTCashListItem.dll and depends upon bACTCreditCard.dll and a number of stored
    '                          procedures too.
    '
    ' CJB 29/07/2005 PN22728 Changed LoadCardNumberCombo to not attempt to get data if no account is set


    Private Const ACClass As String = "uctCreditCard"

    ' Customer combo values
    Private Const ACCustomerNotPresent As String = "Not Present"
    Private Const ACCustomerPresent As String = "Present"

    Private Const ACDataArray_CSVNumberLength As Integer = 0
    Private Const ACDataArray_ConnectorCode As Integer = 1
    Private Const ACDataArray_MinCardAmount As Integer = 2
    Private Const ACDataArray_MaxCardAmount As Integer = 3
    Private Const ACDataArray_ConnectorDesc As Integer = 4

    ' Label colour constants
    Private Const ACEnabledColor As Integer = -2147483630
    Private Const ACDisabledColor As Integer = -2147483631

    ' Global error checker
    Private m_lReturn As Integer

    ' Array of previously used credit card numbers (with min and max values that are checked in
    ' validate method against the payment amount)
    Private m_vPreviousCCData(,) As Object

    ' Previously used cc data array index constants (used when making Payments back to customers)
    Private Const ACCCNumberArray_CCNumber As Integer = 0
    Private Const ACCCNumberArray_MinAmt As Integer = 1
    Private Const ACCCNumberArray_MaxAmt As Integer = 2
    Private Const ACCCNumberArray_LastUsedDate As Integer = 3
    Private Const ACCCNumberArray_CCExpiryDate As Integer = 4
    Private Const ACCCNumberArray_CCStartDate As Integer = 5
    Private Const ACCCNumberArray_CCIssueNo As Integer = 6
    Private Const ACCCNumberArray_CCPIN As Integer = 7
    Private Const ACCCNumberArray_CCNameOnCard As Integer = 8
    Private Const ACCCNumberArray_CCCustomerFlag As Integer = 9

    ' Flag that is False until Initialise method is called - to prevent refreshing and unnecessary loading
    ' etc while parent sets certain key properties that effect the usercontrol display and validation
    Private m_bControlInitialisedFlag As Boolean

    ' MediaTypeIssuerConnectorCode (a third party card processing connector for the selected issuer) - if this and
    ' MediaTypeIssuerID have values then Manual Auth is disabled (meaning the usercontrol will try to do a payment/receipt
    ' with the provider), else it is enabled and mandatory. This value is set internally (as long as we have a value set for
    ' m_lMediaTypeIssuerID) from a call to g_oBusiness.GetMediaTypeIssuerAndConnectorData. So if we have a code then
    ' g_oBusiness.AuthorisePayment will be called in the Validate method, if not then a manual authorisation will have to
    ' take place.
    Private m_sMediaTypeIssuerConnectorCode As String = ""
    Private m_sMediaTypeIssuerConnectorDesc As String = ""

    ' CSV Number Length. This value is set internally (as long as we have a value set for m_lMediaTypeIssuerID) from a call
    ' to g_oBusiness.GetMediaTypeIssuerAndConnectorData. If the value returned is > 0 then the CSV field is made mandatory
    ' and checked to be the specified length during validation.
    Private m_iCSVNumberLength As Integer

    ' Min and Max allowable card amounts for the selected card issuer
    Private m_cMinCardAmount As Decimal
    Private m_cMaxCardAmount As Decimal

    '------------------------------------
    ' Property Procedure Access Variables
    '------------------------------------

    ' Used for View Only Mode to disable all controls. If set this will also restrict usercontrol display processing.
    ' Must be set by container component to True when applicable...setting to false will not enable the control - see
    ' m_lMediaTypeID to do this.
    Private m_bViewOnlyMode As Boolean

    ' Is this a Payment (if False then this implies it is a Receipt). This will greatly effect the display and validation
    ' of the usercontrol. Must be set by container component but will default to False.
    Private m_bIsPayment As Boolean

    ' Media Type ID (for a Media Type e.g. Credit Card etc) is required for CC Number validation...the setting of this value,
    ' if the usercontrol was previously disabled, will also enable the control. The changing of this value will clear all data
    ' input controls and refresh the display of the usercontrol. Must be set by container component or the control will remain
    ' in ViewOnly mode.
    Private m_lMediaTypeID As Integer

    ' MediaType IssuerID (for a given Issuer e.g. Mastercard, Visa etc) - if this and MediaTypeIssuerConnectorCode have
    ' values then Manual Auth is disabled, else it is enabled and mandatory. Also if this is set then the CSV/PIN field is
    ' mandatory (as long as the length retrieved from the db is not zero). This value is also used when loading the combo of
    ' previously used cc numbers for the selected account. The changing of this value will clear all data input controls and
    ' refresh the display of the usercontrol. May be set by container component.
    Private m_lMediaTypeIssuerID As Integer

    ' AccountID required for lookup of previous cc no's to load into combo. The changing of this value will clear all data
    ' input controls and refresh the display of the usercontrol. May be set by container component.
    Private m_lAccountID As Integer

    ' If set, then takes precedence over m_lAccountID when finding previously used cc numbers to load into the cc number combo.
    ' If, in the future, this is changable by the container component then this property will need to be changed in here to
    ' make the usercontrol clear all controls and refresh (as in AccountID for example). May be set by container component.
    Private m_lInsuranceFileCnt As Integer

    ' The Customer Flag (Present/Not Present)
    Private m_sCCCustomer As String = ""

    ' The CC Number (in textbox for Receipts or combo of previously used credit cards for Payments)
    Private m_sCCNumber As String = ""

    ' The Amount to debit/credit (not displayed) - must be set by container component.
    Private m_cCCAmount As Decimal

    ' The Currency of the Amount (not displayed). This is used when calling g_oBusiness.AuthorisePayment. Must be set by
    ' container component
    Private m_iCCCurrencyID As Integer

    ' Line 1 of the cardholder's address. This is used when calling g_oBusiness.AuthorisePayment. Must be set by container
    ' component.
    Private m_sCCAddress1 As String = ""

    ' Postcode of the cardholder's address. This is used when calling g_oBusiness.AuthorisePayment. Must be set by container
    ' component.
    Private m_sCCPostcode As String = ""

    ' Error Return Status returned from Connector. Will be shown to user when provider or validation errors occur.
    Private m_sCCReturnStatus As String = ""

    ' Transaction Code returned from Connector (not displayed)...used for final funds commitment. Container component will need
    ' to inspect this value.
    Private m_sCCTransactionCode As String = ""

    ' Claim Payment Type Flag. Used when loading the combo of previously used cc numbers for the selected account. Must be set by
    ' container component.
    Private m_bIsClaimPaymentType As Boolean
    Private m_sCreditCardNo As String = ""
    Private m_vUnderwriting As String = ""
    Private m_nCCIsDefault As Integer = 0

    'Start (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)
    ' Removed system options m_bIsCCAuthorisationOff and m_bIsIgnore9
    ' Adding a system option m_bIsExternalCreditCardProcessing
    Private m_bIsExternalCreditCardProcessing As Boolean
    'End (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)


    'WPR12- Enhancement Quote Collection Process
    Private m_bIsAdditionalDetailOption As Boolean

    '--------------------
    ' Property Procedures - for information on each see respective Property Procedure Acesss Variables
    '--------------------


    'Start (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)
    ' Removed Property IsCCAuthorisationOff
    ' Adding Property IsExternalCreditCardProcessing
    Public Property DefaultBankPaymentType As String
    Public Property DefaultAccountType As String

    Public Property ResetPreviousOne As Boolean = False
    <Browsable(True)>
    Public Property IsExternalCreditCardProcessing() As Boolean
        Get
            Return m_bIsExternalCreditCardProcessing
        End Get
        Set(ByVal Value As Boolean)
            m_bIsExternalCreditCardProcessing = Value
        End Set
    End Property
    'End (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)


    <Browsable(True)>
    Public Property ViewOnlyMode() As Boolean
        Get
            Return m_bViewOnlyMode
        End Get
        Set(ByVal Value As Boolean)

            ' If no change in this property value then do nothing!
            If Value = m_bViewOnlyMode Then Exit Property

            ' If we are putting the control into ViewOnly mode then do so (note that setting to false will not
            ' set it back - we need to have a value in MediaTypeID to do that).
            If Value Then
                m_bViewOnlyMode = Value

                ' Reformat the display of the usercontrol based upon any properties that have been set
                m_lReturn = SetInterfaceDefaults()
            Else
                m_bViewOnlyMode = Value
            End If

        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property IsPayment() As Boolean
        Set(ByVal Value As Boolean)
            m_bIsPayment = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property IsClaimPaymentType() As Boolean
        Set(ByVal Value As Boolean)
            m_bIsClaimPaymentType = Value
        End Set
    End Property
    <Browsable(False)>
    Public WriteOnly Property Encrypt() As Boolean
        Set(ByVal Value As Boolean)
            ' Prakash: Removed codes related to m_bIsCCAuthorisationOff -(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)
            If Value Then
                If Strings.Len(txtCardNumber.Text) > 4 Then
                    txtCardNumber.Text = "**** **** **** " & txtCardNumber.Text.Substring(txtCardNumber.Text.Length - 4)
                End If
            End If
        End Set
    End Property
    <Browsable(False)>
    Public WriteOnly Property ControlInitialisedFlag() As Boolean
        Set(ByVal Value As Boolean)
            m_bControlInitialisedFlag = Value
        End Set
    End Property
    <Browsable(False)>
    Public WriteOnly Property MediaTypeIssuerID() As Integer
        Set(ByVal Value As Integer)

            ' If no change in this property value then do nothing!
            If Value = m_lMediaTypeIssuerID Then Exit Property

            m_lMediaTypeIssuerID = Value

            'If NOT in View Only mode
            If Not m_bViewOnlyMode Then

                ' Ensure we are not setting this property before the usercontrol Initialise has been called
                If m_bControlInitialisedFlag Then

                    ' Clear out data input controls
                    'm_lReturn = ClearControls()

                    ' Reformat the display of the usercontrol based upon any properties that have been set
                    m_lReturn = SetInterfaceDefaults()

                End If
            End If

        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property MediaTypeID() As Integer
        Set(ByVal Value As Integer)

            ' If no change in this property value then do nothing!
            If Value = m_lMediaTypeID Then Exit Property

            ' If media type is set and control was previously disabled then enable it all
            If Value <> 0 And m_bViewOnlyMode Then
                m_bViewOnlyMode = False

                ' Ensure we are not setting this property before the usercontrol Initialise has been called
                If m_bControlInitialisedFlag Then

                    ' Clear out data input controls
                    m_lReturn = ClearControls()

                    ' Reformat the display of the usercontrol based upon any properties that have been set
                    m_lReturn = SetInterfaceDefaults()

                End If
            End If

            m_lMediaTypeID = Value

        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property AccountID() As Integer
        Set(ByVal Value As Integer)

            ' If no change in this property value then do nothing!
            If Value = m_lAccountID Then Exit Property

            'If NOT in View Only mode
            If m_bViewOnlyMode Then
                m_lAccountID = Value
            Else
                m_lAccountID = Value

                ' Ensure we are not setting this property before the usercontrol Initialise has been called
                If m_bControlInitialisedFlag Then

                    ' Clear out data input controls
                    m_lReturn = ClearControls()

                    ' Reformat the display of the usercontrol based upon any properties that have been set
                    m_lReturn = SetInterfaceDefaults()

                End If
            End If

        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property CCAmount() As Decimal
        Set(ByVal Value As Decimal)
            m_cCCAmount = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property CCCurrencyID() As Integer
        Set(ByVal Value As Integer)
            m_iCCCurrencyID = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property CCNumber() As String
        Get

            ' Get the CCNumber from the relevant control depending what mode we are in
            If m_bIsPayment Then
                'CCNumber = Trim(m_vPreviousCCData(ACCCNumberArray_CCNumber, cboCardNumber.ListIndex))
                ' Start - Sankar - PN 56728
                Return m_sCreditCardNo
                ' End - Sankar - PN 56728
            Else
                ' In receipt mode we have a credit card number textbox
                Return m_sCreditCardNo.Trim()
            End If

        End Get
        Set(ByVal Value As String)
            m_sCCNumber = Value

            ' Prakash : Setting m_sCreditCardNo also to sCCNumber since credit card field becomes empty when a receipt item is edited twice.
            m_sCreditCardNo = Value
        End Set
    End Property

    'Party Bank Details
    <Browsable(False)>
    Public WriteOnly Property CCNumber1() As String
        Set(ByVal Value As String)
            ' Prakash: Removed codes related to m_bIsCCAuthorisationOff -(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)
            m_sCreditCardNo = Value
            If txtCardNumber.Visible = False Then
                txtCardNumber.Visible = True
            End If
            If Not (Value Is Nothing) Then
                If Value.Length > 4 Then
                    txtCardNumber.Text = "**** **** **** " & Value.Substring(Value.Length - 4)
                Else
                    txtCardNumber.Text = Value
                End If
            End If

            txtTrackingNumber.Text = Value
        End Set
    End Property


    <Browsable(True)>
    Public Property CCName() As String
        Get
            Return txtNameOnCard.Text.Trim()
        End Get
        Set(ByVal Value As String)
            txtNameOnCard.Text = Value
        End Set
    End Property


    <Browsable(True)>
    Public Property CCExpiry() As String
        Get
            Return txtExpiryDate.Text
        End Get
        Set(ByVal Value As String)

            ' This should be in format MM/YY
            txtExpiryDate.Text = Value

        End Set
    End Property


    <Browsable(True)>
    Public Property CCStart() As String
        Get
            Return txtStartDate.Text
        End Get
        Set(ByVal Value As String)

            ' This should be in format MM/YY
            txtStartDate.Text = Value

        End Set
    End Property


    <Browsable(True)>
    Public Property CCIssue() As String
        Get
            Return txtIssueNumber.Text.Trim()
        End Get
        Set(ByVal Value As String)
            txtIssueNumber.Text = Value
        End Set
    End Property


    <Browsable(True)>
    Public Property CCPIN() As String
        Get
            Return txtCSVPIN.Text.Trim()
        End Get
        Set(ByVal Value As String)
            txtCSVPIN.Text = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property CCAddress1() As String
        Set(ByVal Value As String)
            m_sCCAddress1 = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property CCPostcode() As String
        Set(ByVal Value As String)
            m_sCCPostcode = Value
        End Set
    End Property


    <Browsable(True)>
    Public Property CCAutoAuthCode() As String
        Get
            Return txtAutoAuthCode.Text.Trim()
        End Get
        Set(ByVal Value As String)
            txtAutoAuthCode.Text = Value
        End Set
    End Property


    <Browsable(True)>
    Public Property CCManualAuthCode() As String
        Get
            Return txtManualAuth.Text.Trim()
        End Get
        Set(ByVal Value As String)
            txtManualAuth.Text = Value
        End Set
    End Property


    <Browsable(True)>
    Public Property CCCustomerFlag() As String
        Get
            Return cboCustomer.Text
        End Get
        Set(ByVal Value As String)
            m_sCCCustomer = Value
        End Set
    End Property

    <Browsable(False)>
    Public ReadOnly Property CCStatusText() As String
        Get
            Return txtStatus.Text.Trim()
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property CCReturnStatus() As String
        Get
            Return m_sCCReturnStatus.Trim()
        End Get
    End Property


    <Browsable(True)>
    Public Property CCTransactionCode() As String
        Get
            Return m_sCCTransactionCode.Trim()
        End Get
        Set(ByVal Value As String)
            m_sCCTransactionCode = Value
        End Set
    End Property
    <Browsable(True)>
    Public Property CCIsDefault() As Integer
        Get
            Return m_nCCIsDefault
        End Get
        Set(ByVal Value As Integer)
            m_nCCIsDefault = Value
            chkIsDefault.Checked = Value
        End Set
    End Property
    'WPR12- Enhancement Quote Collection Process

    <Browsable(True)>
    Public Property IsAdditionalDetailOption() As Boolean
        Get
            Return m_bIsAdditionalDetailOption
        End Get
        Set(ByVal Value As Boolean)
            m_bIsAdditionalDetailOption = Value
            If m_bIsAdditionalDetailOption Then
                m_lReturn = EnableDisableAdditionalDetails()
            End If
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property CaptionNameOnCardAdditionalOption() As Boolean
        Set(ByVal Value As Boolean)
            If Value Then
                lblNameOnCard.Text = "Name of Card Holder:"
            Else
                lblNameOnCard.Text = "Name on Card:"
            End If
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property CaptionExpiryDateAdditionalOption() As Boolean
        Set(ByVal Value As Boolean)
            If Value Then
                lblExpiryDate.Text = "Expiry Date MM/YY:"
            Else
                lblExpiryDate.Text = "Expiry Date:"
            End If
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property CaptionCVSPINAdditionalOption() As Boolean
        Set(ByVal Value As Boolean)
            If Value Then
                lblCSVPIN.Text = "CVV:"
            Else
                lblCSVPIN.Text = "CSV/PIN:"
            End If
        End Set
    End Property


    <Browsable(True)>
    Public Property CCBankId() As Integer
        Get
            Dim result As Integer = 0
            If cboCCBank.Visible Then
                If cboCCBank.ListIndex <> -1 Then
                    result = cboCCBank.ItemData(cboCCBank.ListIndex)
                End If
            Else
                result = -1
            End If
            Return result
        End Get
        Set(ByVal Value As Integer)
            'cboCCBank.ListIndex = Value
            cboCCBank.ItemId = Value
        End Set
    End Property


    <Browsable(True)>
    Public Property CardTypeId() As Integer
        Get
            Dim result As Integer = 0
            If cboCardType.Visible Then
                If cboCardType.ListIndex <> -1 Then
                    result = cboCardType.ItemData(cboCardType.ListIndex)
                End If
            Else
                result = -1
            End If
            Return result
        End Get
        Set(ByVal Value As Integer)
            cboCardType.ItemId = Value
        End Set
    End Property


    <Browsable(True)>
    Public Property CardTransSlipNo() As String
        Get
            If txtCardTransSlipNo.Visible Then
                Return txtCardTransSlipNo.Text.Trim()
            Else
                Return ""
            End If
        End Get
        Set(ByVal Value As String)
            txtCardTransSlipNo.Text = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property IsDefault() As Boolean
        Get
            Return chkIsDefault.CheckState
        End Get
        Set(ByVal Value As Boolean)
            chkIsDefault.Checked = Value
        End Set
    End Property
    ' ***************************************************************** '
    '
    ' Name: SetViewOnlyMode
    '
    ' Description: Set the enabled property of all data input controls
    '              on the user control to be False. See ViewOnlyMode
    '              Property Procedure for more info.
    '
    ' ***************************************************************** '
    Private Function SetViewOnlyMode() As Integer

        Dim result As Integer = 0
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' In view only mode we'll show the cc number in a textbox (normally its just a textbox for receipts and is a
            ' combo for payments).
            lblCardNumber.Font = VB6.FontChangeBold(lblCardNumber.Font, False)
            lblCardNumber.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
            cboCardNumber.Visible = False
            txtCardNumber.Visible = True
            txtCardNumber.Enabled = False
            chkIsDefault.Enabled = False
            ' Mask all but the last 4 digits of the CC Number
            If Strings.Len(txtCardNumber.Text) > 4 Then
                txtCardNumber.Text = "**** **** **** " & txtCardNumber.Text.Substring(txtCardNumber.Text.Length - 4)
            End If

            txtNameOnCard.Enabled = False
            lblNameOnCard.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
            lblNameOnCard.Font = VB6.FontChangeBold(lblNameOnCard.Font, False)

            txtExpiryDate.Enabled = False
            lblExpiryDate.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
            lblExpiryDate.Font = VB6.FontChangeBold(lblExpiryDate.Font, False)

            txtStartDate.Enabled = False
            lblStartDate.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
            lblStartDate.Font = VB6.FontChangeBold(lblStartDate.Font, False)

            txtIssueNumber.Enabled = False
            lblIssueNumber.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
            lblIssueNumber.Font = VB6.FontChangeBold(lblIssueNumber.Font, False)

            txtCSVPIN.Enabled = False
            lblCSVPIN.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
            lblCSVPIN.Font = VB6.FontChangeBold(lblCSVPIN.Font, False)

            ' Mask all of the PIN
            If Strings.Len(txtCSVPIN.Text) > 0 Then
                txtCSVPIN.Text = New String("*", Strings.Len(txtCSVPIN.Text))
            End If

            txtManualAuth.Enabled = False
            lblManualAuth.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
            lblManualAuth.Font = VB6.FontChangeBold(lblManualAuth.Font, False)

            cboCustomer.Enabled = False
            lblCustomer.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
            lblCustomer.Font = VB6.FontChangeBold(lblCustomer.Font, False)

            lblAutoAuthCode.Font = VB6.FontChangeBold(lblAutoAuthCode.Font, False)

            'Start (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)
            txtTrackingNumber.Enabled = False
            txtTrackingNumber.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
            txtTrackingNumber.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
            'End (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)

            'WPR12- Enhancement Quote Collection Process
            If IsAdditionalDetailOption Then
                cboCardType.Visible = True
                lblCardType.Visible = True
                lblCardType.ForeColor = ColorTranslator.FromOle(ACEnabledColor)
                lblCardType.Font = VB6.FontChangeBold(lblCardType.Font, True)

                cboCCBank.Visible = True
                lblCCBank.Visible = True
                lblCCBank.ForeColor = ColorTranslator.FromOle(ACEnabledColor)
                lblCCBank.Font = VB6.FontChangeBold(lblCCBank.Font, True)

                txtCardTransSlipNo.Visible = True
                lblCardTransSlipNo.Visible = True
                lblCardTransSlipNo.ForeColor = ColorTranslator.FromOle(ACEnabledColor)
                lblCardTransSlipNo.Font = VB6.FontChangeBold(lblCardTransSlipNo.Font, True)
            Else
                cboCardType.Visible = False
                lblCardType.Visible = False

                cboCCBank.Visible = False
                lblCCBank.Visible = False

                txtCardTransSlipNo.Visible = False
                lblCardTransSlipNo.Visible = False
            End If



        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetViewOnlyMode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetViewOnlyMode", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
            result = gPMConstants.PMEReturnCode.PMFalse
        Finally



        End Try
        Return result
    End Function


    ' ***************************************************************** '
    '
    ' Name: EnableDisableControls
    '
    ' Description:  Enable/Disable relevant control s dependent on if this
    '               is a card Payment (True passed in) or a card Receipt
    '               (False passed in) then
    '
    ' ***************************************************************** '
    Private Function EnableDisableControls(ByVal v_bIsPayment As Boolean) As Integer

        Dim result As Integer = 0
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' For card PAYMENTS just enable the card number combo (has previously used numbers in it)
            If v_bIsPayment Then

                lblCardNumber.ForeColor = ColorTranslator.FromOle(ACEnabledColor)

                If cboCardNumber.Items.Count > 0 Then
                    cboCardNumber.Visible = True
                    cboCardNumber.Enabled = True
                    txtCardNumber.Visible = False
                Else
                    cboCardNumber.Visible = False
                    txtCardNumber.Visible = True
                    txtManualAuth.Enabled = False
                End If
                txtNameOnCard.Enabled = False
                lblNameOnCard.ForeColor = ColorTranslator.FromOle(ACDisabledColor)

                txtExpiryDate.Enabled = False
                lblExpiryDate.ForeColor = ColorTranslator.FromOle(ACDisabledColor)

                txtStartDate.Enabled = False
                lblStartDate.ForeColor = ColorTranslator.FromOle(ACDisabledColor)

                txtIssueNumber.Enabled = False
                lblIssueNumber.ForeColor = ColorTranslator.FromOle(ACDisabledColor)

                txtCSVPIN.Enabled = False
                lblCSVPIN.ForeColor = ColorTranslator.FromOle(ACDisabledColor)

                cboCustomer.Enabled = False
                lblCustomer.ForeColor = ColorTranslator.FromOle(ACDisabledColor)

                'Start (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)
                txtTrackingNumber.Enabled = False
                txtTrackingNumber.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                txtTrackingNumber.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                'End (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)

                'WPR12- Enhancement Quote Collection Process
                If IsAdditionalDetailOption Then
                    cboCardType.Enabled = False
                    lblCardType.ForeColor = ColorTranslator.FromOle(ACDisabledColor)

                    cboCCBank.Enabled = False
                    lblCCBank.ForeColor = ColorTranslator.FromOle(ACDisabledColor)

                    txtCardTransSlipNo.Enabled = False
                    lblCardTransSlipNo.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                Else

                    cboCardType.Visible = False
                    lblCardType.Visible = False

                    cboCCBank.Visible = False
                    lblCCBank.Visible = False

                    txtCardTransSlipNo.Visible = False
                    lblCardTransSlipNo.Visible = False
                End If

            Else
                ' For card RECEIPTS enable the card number textbox, name on card textbox, expiry date textbox,
                ' start date textbox, issue number textbox, CSV/PIN textbox and customer combo.
                txtCardNumber.Visible = True
                txtCardNumber.Enabled = True
                lblCardNumber.ForeColor = ColorTranslator.FromOle(ACEnabledColor)
                cboCardNumber.Visible = False

                txtNameOnCard.Enabled = True
                lblNameOnCard.ForeColor = ColorTranslator.FromOle(ACEnabledColor)

                txtExpiryDate.Enabled = True
                lblExpiryDate.ForeColor = ColorTranslator.FromOle(ACEnabledColor)

                txtStartDate.Enabled = True
                lblStartDate.ForeColor = ColorTranslator.FromOle(ACEnabledColor)

                txtIssueNumber.Enabled = True
                lblIssueNumber.ForeColor = ColorTranslator.FromOle(ACEnabledColor)

                txtCSVPIN.Enabled = True
                lblCSVPIN.ForeColor = ColorTranslator.FromOle(ACEnabledColor)

                cboCustomer.Enabled = True
                lblCustomer.ForeColor = ColorTranslator.FromOle(ACEnabledColor)

                'Start (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)
                txtTrackingNumber.Enabled = False
                txtTrackingNumber.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                txtTrackingNumber.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                'End (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)

                'WPR12- Enhancement Quote Collection Process
                If IsAdditionalDetailOption Then
                    cboCardType.Enabled = True
                    lblCardType.ForeColor = ColorTranslator.FromOle(ACEnabledColor)

                    cboCCBank.Enabled = True
                    lblCCBank.ForeColor = ColorTranslator.FromOle(ACEnabledColor)

                    txtCardTransSlipNo.Enabled = True
                    lblCardTransSlipNo.ForeColor = ColorTranslator.FromOle(ACEnabledColor)
                Else
                    cboCardType.Visible = False
                    lblCardType.Visible = False

                    cboCCBank.Visible = False
                    lblCCBank.Visible = False

                    txtCardTransSlipNo.Visible = False
                    lblCardTransSlipNo.Visible = False
                End If

            End If

            ' Lock certain controls (they will never allow data entry but text can be copied from them if desired)
            txtAutoAuthCode.ReadOnly = True
            txtStatus.ReadOnly = True



        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnableDisableControls Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnableDisableControls", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
            result = gPMConstants.PMEReturnCode.PMFalse
        Finally



        End Try
        Return result
    End Function
    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description: PUBLIC method that gets the instance to the business
    '              object, sets all interface defaults etc. Should be
    '              called after setting any public property values.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try

            Dim vValue As Object
            Dim sValue As String = ""
            Me.cboCardType.FirstItem = ""
            Me.cboCCBank.FirstItem = ""
            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = iPMFunc.getUnderwritingOrAgency(r_vUnderwriting:=m_vUnderwriting)

            ' Get an instance of object manager
            g_oObjectManager = New bObjectManager.ObjectManager()

            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.Initialise", "sCallingAppName = " & ACApp)
            End If

            ' Get an instance of the business object
            Dim temp_g_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bACTCreditCard.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            g_oBusiness = temp_g_oBusiness
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                txtStatus.Text = "Connector initialisation failed."
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "sCallingAppName = " & ACApp & ", sClassName:=bACTCreditCard.Business")
            End If

            ' Update status of connector to show 'Ready'
            txtStatus.Text = "Ready"

            ' This will indicate to certain key property procedures that if their value changes we need to clear and reload the
            ' usercontrol.
            m_bControlInitialisedFlag = True

            'Start (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)
            ' Removed codes related to m_bIsCCAuthorisationOff and m_bIsIgnore9
            m_lReturn = iPMFunc.GetSystemOption(5069, sValue, 1) ' System option credit card processing method
            m_bIsExternalCreditCardProcessing = (sValue = "1") '0- Internal ; 1- External
            'End (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)

            ' Reformat the display of the usercontrol based upon any properties that have been set
            m_lReturn = SetInterfaceDefaults()


        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
            result = gPMConstants.PMEReturnCode.PMFalse
        Finally
            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values and generally
    '              formats the display of the usercontrol depending on various
    '              Property Procedure values that have been set previously.
    '              Note that this is called from within the Initialise method
    '              and also if certain key values are changed in the usercontrol
    '              by the container component (that necessite the usercontrol to
    '              be reformatted).
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Dim vOutputDetails As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Ensure we are not formatting the control before the usercontrol Initialise has been called
            If Not m_bControlInitialisedFlag Then
                Return result
            End If

            m_sMediaTypeIssuerConnectorCode = ""
            m_iCSVNumberLength = 0
            m_cMinCardAmount = 0
            m_cMaxCardAmount = 0

            ' If we don't have enough info to get the business info. then don't even try! This will occur if there are no
            ' issuers or connectors for an issuer and so the card will have to be authorised manually...
            If m_lMediaTypeIssuerID <> 0 Then

                ' Call business component to get additional data values required for display and validation
                ' Pass MediaTypeIssuerID value to bACTCreditCard and from the mediatype_issuer table return:
                ' csv_number_length. Also get MediaType_Connector.code for the selected Issuer - if no code
                ' then enable and make mandatory the Manual Auth textbox.

                m_lReturn = g_oBusiness.GetMediaTypeIssuerAndConnectorData(v_lMediaType_Issuer_ID:=m_lMediaTypeIssuerID, r_vOutputDetails:=vOutputDetails)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("g_oBusiness.GetMediaTypeIssuerAndConnectorData", "sCallingAppName = " & ACApp & ", v_lMediaType_Issuer_ID:=" & CStr(m_lMediaTypeIssuerID))
                End If

                If Information.IsArray(vOutputDetails) Then

                    m_sMediaTypeIssuerConnectorCode = CStr(vOutputDetails(ACDataArray_ConnectorCode, 0)).Trim()

                    m_sMediaTypeIssuerConnectorDesc = CStr(vOutputDetails(ACDataArray_ConnectorDesc, 0)).Trim()

                    m_iCSVNumberLength = CInt(vOutputDetails(ACDataArray_CSVNumberLength, 0))

                    If CStr(vOutputDetails(ACDataArray_MinCardAmount, 0)) <> "" Then

                        m_cMinCardAmount = CDec(vOutputDetails(ACDataArray_MinCardAmount, 0))
                    End If

                    If CStr(vOutputDetails(ACDataArray_MaxCardAmount, 0)) <> "" Then

                        m_cMaxCardAmount = CDec(vOutputDetails(ACDataArray_MaxCardAmount, 0))
                    End If
                End If
            End If

            If cboCustomer.Items.Count > 0 Then cboCustomer.SelectedIndex = -1

            ' Check if to disable ALL visible controls (for View Only mode) and to mask most of the card no. and
            ' all of the PIN
            If m_bViewOnlyMode Then
                m_lReturn = SetViewOnlyMode()
                Return result
            End If

            ' If cc number combo shown (only in Payment mode) then load it with past credit cards used for this account
            If m_bIsPayment Then
                m_lReturn = LoadCardNumberCombo()
            ElseIf m_sCCNumber <> "" Then
                'Load cc no. into textbox (shown when in Receipt mode)
                txtCardNumber.Text = "**** **** **** " & m_sCCNumber.Substring(m_sCCNumber.Length - 4)
                'Start (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)
                txtTrackingNumber.Text = m_sCCNumber
                'End (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)
            End If

            ' Populate 'Customer' combo
            m_lReturn = LoadCustomerCombo()

            ' Set whether controls are visible and enabled/disabled depending on if this is a card PAYMENT or a RECEIPT
            m_lReturn = EnableDisableControls(v_bIsPayment:=m_bIsPayment)

            ' Make labels of Mandatory controls BOLD (if corresponding control is enabled)
            m_lReturn = MakeControlsMandatoryOrOptional()

            'Start (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)
            ' If credit card processing method is external, show the tracking number field
            If m_bIsExternalCreditCardProcessing Then
                txtTrackingNumber.Visible = True
                lblTrackingNumber.Visible = True
            Else
                txtTrackingNumber.Visible = False
                lblTrackingNumber.Visible = False
            End If
            'End (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)

            ' Visible Additional Options
            'WPR12- Enhancement Quote Collection Process
            m_lReturn = AdditionalDetailOptions()



        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetInterfaceDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
            result = gPMConstants.PMEReturnCode.PMFalse
        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description: PUBLIC function that releases any object references
    '              we have to free up resources.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If g_oBusiness IsNot Nothing Then
                    g_oBusiness.Dispose()

                End If
                g_oBusiness = Nothing
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()

                End If
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
                g_oObjectManager = Nothing

            End If
        End If
        Me.disposedValue = True
    End Sub


    Private Sub cboCardNumber_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCardNumber.SelectedIndexChanged

        Try


            ' If we have selected a previously used credit card then copy the other details through to all the
            ' other (read-only) controls on the usercontrol.
            If cboCardNumber.SelectedIndex <> -1 Then
                txtNameOnCard.Text = CStr(m_vPreviousCCData(ACCCNumberArray_CCNameOnCard, cboCardNumber.SelectedIndex)).Trim()
                txtExpiryDate.Text = CStr(m_vPreviousCCData(ACCCNumberArray_CCExpiryDate, cboCardNumber.SelectedIndex)).Trim()
                txtStartDate.Text = CStr(m_vPreviousCCData(ACCCNumberArray_CCStartDate, cboCardNumber.SelectedIndex)).Trim()
                txtIssueNumber.Text = CStr(m_vPreviousCCData(ACCCNumberArray_CCIssueNo, cboCardNumber.SelectedIndex)).Trim()
                txtCSVPIN.Text = CStr(m_vPreviousCCData(ACCCNumberArray_CCPIN, cboCardNumber.SelectedIndex)).Trim()
                If cboCustomer.Items.Count > 0 And CStr(m_vPreviousCCData(ACCCNumberArray_CCCustomerFlag, cboCardNumber.SelectedIndex)).Trim() <> "" Then
                    cboCustomer.Text = CStr(m_vPreviousCCData(ACCCNumberArray_CCCustomerFlag, cboCardNumber.SelectedIndex)).Trim()
                End If
            Else
                txtNameOnCard.Text = ""
                txtExpiryDate.Text = ""
                txtStartDate.Text = ""
                txtIssueNumber.Text = ""
                txtCSVPIN.Text = ""
            End If



        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cboCardNumber_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cboCardNumber_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally


        End Try
    End Sub

    Private Sub txtCardNumber_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCardNumber.Enter
        txtCardNumber.SelectionStart = 0
        txtCardNumber.SelectionLength = Strings.Len(txtCardNumber.Text)
    End Sub

    Private Sub txtCardNumber_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtCardNumber.KeyPress
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
        'Allow Ctrl+V
        If KeyAscii = 22 Then
            Exit Sub
        End If
        If (Strings.Chr(KeyAscii).ToString() < "0" Or Strings.Chr(KeyAscii).ToString() > "9") And Strings.Chr(KeyAscii).ToString() <> " " Then
            KeyAscii = 0
        End If

        If (Strings.Chr(KeyAscii).ToString() >= "0" Or Strings.Chr(KeyAscii).ToString() <= "9") And txtCardNumber.Text.Replace(" ", "").Length = 16 Then
            KeyAscii = 0
        End If

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtCardNumber_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles txtCardNumber.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        Dim bSirMediaTypeValidation As Object

        Const sFunctionName As String = "txtCardNo_Validate"

        Dim oSirMediaTypeValidation As Object
        Dim bValid As Boolean
        Dim sCCNOMessage, sCCNoMessageBoxTitle, sCreditCardNo As String

        Try

            ' get the card no from the "copy from" variable if
            ' there is a mask "*" in the card no field otherwise
            ' use the value from the screen
            If (txtCardNumber.Text.IndexOf("*"c) + 1) = 0 Then
                sCreditCardNo = txtCardNumber.Text.Replace(" ", "")
                m_sCreditCardNo = sCreditCardNo
            Else
                sCreditCardNo = m_sCreditCardNo
            End If

            ' if only tabbing through code then - dont validate
            ' save will demand that the field is populated
            'Start (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)
            ' Removed codes related to m_bIsCCAuthorisationOff and m_bIsIgnore9
            ' Don't do any validation if credit card processing value is external.
            If sCreditCardNo <> "" And Not m_bIsExternalCreditCardProcessing Then

                Dim temp_oSirMediaTypeValidation As Object
                If g_oObjectManager.GetInstance(temp_oSirMediaTypeValidation, "bSIRMediaTypeValidation.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager) = gPMConstants.PMEReturnCode.PMTrue Then
                    oSirMediaTypeValidation = temp_oSirMediaTypeValidation

                    ' get ccno validate message box title
                    sCCNoMessageBoxTitle = "Credit Card Validation"

                    ' Get the media type if for the credit card


                    m_lReturn = oSirMediaTypeValidation.GetMediaTypeIdForCode("CC", m_lMediaTypeID)

                    ' only a sub so no return to confirm or deny function worked / failed
                    ' so just run off status

                    oSirMediaTypeValidation.ValidateNumber(m_lMediaTypeID, g_oObjectManager.CountryID, sCreditCardNo, bValid)

                    If Not bValid Then
                        ' get invalid card number message
                        sCCNOMessage = "This is an invalid credit card number."
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
                    MessageBox.Show(sCCNOMessage, sCCNoMessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    ' reset card number if it is not valid
                    ' forcing validation before leaving screen
                    txtCardNumber.Text = ""
                    sCreditCardNo = ""
                    If txtCardNumber.Visible And txtCardNumber.Enabled Then
                        txtCardNumber.Focus()
                    End If
                Else
                    Cancel = False
                End If

                ' destroy object reference
                oSirMediaTypeValidation = Nothing
            Else
                ' reset the displayed card no as this shouldnt be set
                ' if the behind the scenes value sCreditCardNo isnt set
                'txtCardNumber.Text = ""
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

    Private Sub txtCSVPIN_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCSVPIN.Enter
        txtCSVPIN.SelectionStart = 0
        txtCSVPIN.SelectionLength = Strings.Len(txtCSVPIN.Text)
    End Sub

    Private Sub txtExpiryDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtExpiryDate.Enter
        txtExpiryDate.SelectionStart = 0
        txtExpiryDate.SelectionLength = Strings.Len(txtExpiryDate.Text)
    End Sub

    Private Sub txtIssueNumber_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtIssueNumber.Enter
        txtIssueNumber.SelectionStart = 0
        txtIssueNumber.SelectionLength = Strings.Len(txtIssueNumber.Text)
    End Sub

    Private Sub txtIssueNumber_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtIssueNumber.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        ' Only allow 0-9,Ctrl+V and backspace
        If (KeyAscii < 48 Or KeyAscii > 57) And KeyAscii <> 8 And KeyAscii <> 22 Then KeyAscii = 0
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtManualAuth_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtManualAuth.Enter
        txtManualAuth.SelectionStart = 0
        txtManualAuth.SelectionLength = Strings.Len(txtManualAuth.Text)
    End Sub

    Private Sub txtNameOnCard_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtNameOnCard.Enter
        txtNameOnCard.SelectionStart = 0
        txtNameOnCard.SelectionLength = Strings.Len(txtNameOnCard.Text)
    End Sub

    Private Sub txtStartDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtStartDate.Enter
        txtStartDate.SelectionStart = 0
        txtStartDate.SelectionLength = Strings.Len(txtStartDate.Text)
    End Sub

    Private Sub UserControl_Initialize()

        m_bControlInitialisedFlag = False

        ' Default the user control into ViewOnly mode as it'll only be enabled if CC is selected
        ' on the container component (the setting of MediaType ID when this happens will trigger
        ' the usercontrol to be enabled.
        ViewOnlyMode = True

        ' Clear out data input controls
        m_lReturn = ClearControls()
        'Me.cboCardType.FirstItem = ""
        'Me.cboCCBank.FirstItem = ""

    End Sub


    ' ***************************************************************** '
    '
    ' Name: Validate
    '
    ' Description: PUBLIC method called from container that validates data
    '              that has been entered on the user control.
    '              Exits if in ViewOnly mode.
    '              If possible we will try to connect to a connector also
    '              for validation and authorisation.
    '
    ' ***************************************************************** '
    Public Shadows Function Validate() As Integer
        Dim result As Integer = 0
        Dim bSIRMediaTypeValidation As Object
        Dim oValidation As Object
        Dim vValid As Integer
        Dim vlCountryID As Integer
        Dim sCCNumber As String = ""
        Const cCorrectCCDateFormat As Integer = 5
        Const cSlashPosition As Integer = 3
        Dim sStartDate, sExpiryDate, sAutoAuthCode As String

        Try



            ' Do not validate if in View Only mode
            If m_bViewOnlyMode Then
                result = gPMConstants.PMEReturnCode.PMTrue
                Return result
            End If

            ' If this is a PAYMENT then just the Credit Card Number combo and possibly the
            ' Manual Auth Code are mandatory
            If m_bIsPayment Then

                ' Check CC Number selected in Combo
                If cboCardNumber.SelectedIndex = -1 Then
                    MessageBox.Show("Please select a Card Number.", "Mandatory value is missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    cboCardNumber.Focus()
                    Return result
                End If

                ' If we have previous CC data then check the selected cc number's min and max limits
                ' Note that a zero in either means no limit has been setup
                If Information.IsArray(m_vPreviousCCData) Then

                    If CDbl(m_vPreviousCCData(ACCCNumberArray_MinAmt, cboCardNumber.SelectedIndex)) <> 0 Then
                        If m_cCCAmount < m_vPreviousCCData(ACCCNumberArray_MinAmt, cboCardNumber.SelectedIndex) Then
                            MessageBox.Show("The transaction amount is less than the minimum amount allowed for the selected card issuer.", "Transaction amount is less than the minimum limit allowed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            cboCardNumber.Focus()
                            Return result
                        End If
                    End If

                    If CDbl(m_vPreviousCCData(ACCCNumberArray_MaxAmt, cboCardNumber.SelectedIndex)) <> 0 Then
                        If m_cCCAmount > m_vPreviousCCData(ACCCNumberArray_MaxAmt, cboCardNumber.SelectedIndex) Then
                            MessageBox.Show("The transaction amount is greater than the maximum amount allowed for the selected card issuer.", "Transaction amount is greater than the maximum limit allowed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            cboCardNumber.Focus()
                            Return result
                        End If
                    End If
                End If

            Else
                ' This is a RECEIPT so check all relevant fields

                ' Check that the amount is within any min and max limits that may have been setup
                ' Note that a zero in either means no limit has been setup
                If m_cMinCardAmount <> 0 Then
                    If m_cCCAmount < m_cMinCardAmount Then
                        MessageBox.Show("The transaction amount is less than the minimum amount allowed for the selected card issuer.", "Transaction amount is less than the minimum limit allowed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        txtCardNumber.Focus()
                        Return result
                    End If
                End If

                If m_cMaxCardAmount <> 0 Then
                    If m_cCCAmount > m_cMaxCardAmount Then
                        MessageBox.Show("The transaction amount is greater than the maximum amount allowed for the selected card issuer.", "Transaction amount is greater than the maximum limit allowed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        txtCardNumber.Focus()
                        Return result
                    End If
                End If

                ' Check CC Number entered in textbox
                If txtCardNumber.Text.Trim().Length = 0 Then
                    MessageBox.Show("Please enter Card Number.", "Mandatory Field", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    txtCardNumber.Focus()
                    Return result
                End If

                'Start (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)
                ' Removed codes related to m_bIsCCAuthorisationOff and m_bIsIgnore9
                ' Don't do any validation if credit card processing value is external.
                If Not m_bIsExternalCreditCardProcessing Then
                    'End (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)

                    ' Check CC Number is numeric
                    Dim temp_oValidation As Object
                    m_lReturn = g_oObjectManager.GetInstance(temp_oValidation, "bSIRMediaTypeValidation.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                    oValidation = temp_oValidation

                    vlCountryID = g_oObjectManager.CountryID

                    oValidation.ValidateNumber(m_lMediaTypeID, vlCountryID, m_sCreditCardNo, vValid)
                    oValidation = Nothing

                    If Not vValid Then
                        MessageBox.Show("The Credit Card number supplied is not valid.", "Invalid Credit Card Number", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        txtCardNumber.Focus()
                        Return result
                    End If
                End If
                ' Check Name on Card entered
                If m_vUnderwriting <> "U" Then
                    If txtNameOnCard.Text.Trim().Length = 0 Then
                        MessageBox.Show("Please enter Name on Card.", "Mandatory value is missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        txtNameOnCard.Focus()
                        Return result
                    End If
                End If
                ' Check Expiry Date entered
                If txtExpiryDate.Text.Trim().Length = 0 Then
                    MessageBox.Show("Please enter Expiry Date.", "Mandatory value is missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    txtExpiryDate.Focus()
                    Return result
                End If

                ' Check Expiry Date is valid (in format MM/YY)
                txtExpiryDate.Text = txtExpiryDate.Text.Trim()

                Dim dbNumericTemp2 As Double
                Dim dbNumericTemp As Double
                If (txtExpiryDate.Text.Trim().Length <> cCorrectCCDateFormat) OrElse
                ((txtExpiryDate.Text.IndexOf("/"c) + 1) <> cSlashPosition) OrElse
                (Not Double.TryParse(txtExpiryDate.Text.Substring(0, 2), NumberStyles.Number,
                                     CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) OrElse
                (Not Double.TryParse(txtExpiryDate.Text.Substring(txtExpiryDate.Text.Length - 2),
                                     NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2)) Or Not Information.IsDate("01/" & txtExpiryDate.Text) Then
                    MessageBox.Show("Please re-enter Expiry Date in the format 'MM/YY'.", "Invalid Expiry Date.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    txtExpiryDate.Focus()
                    Return result
                    ''Start (Saurabh) PN58994
                ElseIf Conversion.Val(txtExpiryDate.Text.Substring(0, 2)) > 12 Or Conversion.Val(txtExpiryDate.Text.Substring(0, 2)) < 1 Then
                    MessageBox.Show("Please re-enter Expiry Date in the format 'MM/YY'.", "Invalid Expiry Date.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    txtExpiryDate.Focus()
                    Return result
                    ''End (Saurabh) PN58994
                Else
                    'Check whether the expiry date is greater than todays date.
                    If DateTime.Today.Year > CDate("01/01/" & Conversion.Val(txtExpiryDate.Text.Substring(txtExpiryDate.Text.Length - 2))).Year Or Conversion.Val(CStr(DateTime.Today.Year).Substring(CStr(DateTime.Today.Year).Length - 2)) = Conversion.Val(txtExpiryDate.Text.Substring(txtExpiryDate.Text.Length - 2)) And Conversion.Val(CStr(DateTime.Today.Month)) > Conversion.Val(txtExpiryDate.Text.Substring(0, 2)) Then
                        MessageBox.Show("The Credit Card has expired.", "Credit card is invalid", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        txtExpiryDate.Focus()
                        Return result
                    End If
                End If

                ' If Start Date has been entered check it is valid (MM/YY)
                If txtStartDate.Text.Trim() <> "" Then
                    txtStartDate.Text = txtStartDate.Text.Trim()
                    Dim dbNumericTemp4 As Double
                    Dim dbNumericTemp3 As Double
                    If (txtStartDate.Text.Trim().Length <> cCorrectCCDateFormat) Or ((txtStartDate.Text.IndexOf("/"c) + 1) <> cSlashPosition) Or (Not Double.TryParse(txtStartDate.Text.Substring(0, 2), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3)) Or (Not Double.TryParse(txtStartDate.Text.Substring(txtStartDate.Text.Length - 2), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4)) Or Not Information.IsDate("01/" & txtStartDate.Text) Then
                        MessageBox.Show("Please re-enter Start Date in the format 'MM/YY'.", "Invalid Start Date.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        txtStartDate.Focus()
                        Return result
                    Else
                        ' Check whether the start date is before today
                        If DateTime.Today < CDate("01/" & txtStartDate.Text) Then
                            MessageBox.Show("The Credit Card is not yet valid.", "Credit card is not yet valid", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            txtStartDate.Focus()
                            Return result
                        End If
                    End If
                End If

                ' If Issue Number entered, check it is numeric
                If txtIssueNumber.Text <> "" Then
                    Dim dbNumericTemp5 As Double
                    If Not Double.TryParse(txtIssueNumber.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                        MessageBox.Show("Invalid Issue Number entered.", "Invalid Issue Number", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        txtIssueNumber.Focus()
                        Return result
                    End If
                End If

                ' Check CSV/PIN entered (if there is a connector AND length from db > 0)
                If m_lMediaTypeIssuerID <> 0 And m_iCSVNumberLength > 0 Then
                    If txtCSVPIN.Text.Trim().Length <> m_iCSVNumberLength Then
                        MessageBox.Show("Please enter a " & m_iCSVNumberLength & " character CSV/PIN.", "Mandatory value is missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        txtCSVPIN.Focus()
                        Return result
                    End If
                End If

            End If

            ' The following is validation common to both PAYMENTS and RECEIPTS

            'If Manual Auth Code is enabled then it is mandatory
            If m_vUnderwriting <> "U" Then
                If txtManualAuth.Enabled And txtManualAuth.Text.Trim().Length = 0 Then
                    MessageBox.Show("Please enter Manual Auth.", "Mandatory value is missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    txtManualAuth.Focus()
                    Return result
                End If
            End If
            ' If we have a connector then call the business component to do more detailed validation
            If m_sMediaTypeIssuerConnectorCode <> "" Then

                If m_bIsPayment Then
                    sCCNumber = CStr(m_vPreviousCCData(ACCCNumberArray_CCNumber, cboCardNumber.SelectedIndex)).Trim()
                Else
                    sCCNumber = txtCardNumber.Text
                End If

                ' Prepare dates (pass em as YYYY/MM/DD)
                If txtStartDate.Text.Trim() <> "" Then
                    sStartDate = "20" & txtStartDate.Text.Substring(txtStartDate.Text.Length - 2) & "/" & txtStartDate.Text.Substring(0, 2) & "/01"
                End If

                If txtExpiryDate.Text.Trim() <> "" Then
                    sExpiryDate = "20" & txtExpiryDate.Text.Substring(txtExpiryDate.Text.Length - 2) & "/" & txtExpiryDate.Text.Substring(0, 2) & "/01"
                End If

                txtStatus.Text = "Authorising Card with " & m_sMediaTypeIssuerConnectorDesc & "..."


                m_lReturn = g_oBusiness.AuthorisePayment(sMediaTypeConnector:=m_sMediaTypeIssuerConnectorCode, bIsReceipt:=Not (m_bIsPayment), cCCAmount:=m_cCCAmount, lCCCurrencyID:=m_iCCCurrencyID, sCCNumber:=sCCNumber, sCCName:=txtNameOnCard.Text, sCCExpiry:=sExpiryDate, sCCStart:=sStartDate, sCCIssue:=txtIssueNumber.Text, sCCPIN:=txtCSVPIN.Text, sCCAddress1:=m_sCCAddress1, sCCPostcode:=m_sCCPostcode, sCCCustomerFlag:=cboCustomer.Text.ToLower().Replace(" "c, "_"c), r_sCCReturnStatus:=m_sCCReturnStatus, r_sCCAutoAuthCode:=sAutoAuthCode, r_sCCTransactionCode:=m_sCCTransactionCode)

                ' If validation failed...
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    txtAutoAuthCode.Text = ""

                    ' For PMFail (e.g. timeout on connection to connector etc) then allow manual auth
                    If m_lReturn = gPMConstants.PMEReturnCode.PMFail Then
                        txtStatus.Text = "Communications failure."

                        MessageBox.Show("Failed to connect to Credit Card Connector (" & m_sCCReturnStatus & "). Please enter manual authorisation code.", "Failed to connect to Credit Card Connector", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        txtManualAuth.Enabled = True
                        lblManualAuth.ForeColor = ColorTranslator.FromOle(ACEnabledColor)
                        lblManualAuth.Font = VB6.FontChangeBold(lblManualAuth.Font, True)
                        m_lMediaTypeIssuerID = 0
                        m_sMediaTypeIssuerConnectorCode = ""
                        txtManualAuth.Focus()
                        Return result

                    Else
                        txtStatus.Text = "Authorisation failed."

                        ' For all other errors e.g. validation failed (pmfalse type errors) then show error and allow retry
                        MessageBox.Show("Validation failed - " & m_sCCReturnStatus, "Credit Card Connector Validation Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return result
                    End If
                Else
                    txtStatus.Text = "Authorisation successful."
                    txtAutoAuthCode.Text = sAutoAuthCode
                End If
            End If

            'WPR12- Enhancement Quote Collection Process
            If IsAdditionalDetailOption Then

                If txtNameOnCard.Text.Trim().Length = 0 Then
                    MessageBox.Show("Please enter Name on Card Holder.", "Mandatory value is missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    txtNameOnCard.Focus()
                    Return result
                End If

                If cboCardType.ListIndex = -1 Then
                    MessageBox.Show("Please select Type of Card.", "Mandatory value is missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    cboCardType.Focus()
                    Return result
                End If

                If cboCCBank.ListIndex = -1 Then
                    MessageBox.Show("Please select Card Issuing Bank Name.", "Mandatory value is missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    cboCCBank.Focus()
                    Return result
                End If

                If txtCardTransSlipNo.Text.Trim().Length = 0 Then
                    MessageBox.Show("Please enter Transaction Number.", "Mandatory value is missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    txtCardTransSlipNo.Focus()
                    Return result
                End If

            End If

            Dim str As String = ""
            Dim oDefaultCreditcardDetails(,) As Object = Nothing
            result = g_oBusiness.GetDefaultCreditCardByAccount(m_lAccountID, oDefaultCreditcardDetails)
            If Information.IsArray(oDefaultCreditcardDetails) AndAlso oDefaultCreditcardDetails.Length > 0 AndAlso chkIsDefault.CheckState = CheckState.Checked Then

                str = "A Default Credit Card Already Exist with " + vbCrLf + "Bank Payment Type:" + oDefaultCreditcardDetails(0, 0) + vbCrLf + "Account Type:" + oDefaultCreditcardDetails(1, 0) + vbCrLf +
                    "Do you want to set this Credit Card as default?"

                If MessageBox.Show(str, "Default Credit Card Already Exist", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = DialogResult.No Then
                    chkIsDefault.CheckState = CheckState.Unchecked
                    m_nCCIsDefault = 0
                Else
                    ResetPreviousOne = True
                    chkIsDefault.CheckState = CheckState.Checked
                End If
            End If
            If result = PMEReturnCode.PMNotFound Then
                result = PMEReturnCode.PMTrue
            End If
            m_nCCIsDefault = chkIsDefault.CheckState

            Return result

        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Validate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Validate", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
            result = gPMConstants.PMEReturnCode.PMError
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: ClearControls
    '
    ' Description: Clears all controls...This is required if certain key
    '              values like MediaType are changed (and so we need to
    '              reload and clear all controls as it effects the display
    '              and validation of them.)
    '
    ' ***************************************************************** '
    Public Function ClearControls() As Integer

        Dim result As Integer = 0
        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            txtNameOnCard.Text = ""
            txtCardNumber.Text = ""
            txtExpiryDate.Text = ""
            txtStartDate.Text = ""
            txtIssueNumber.Text = ""
            txtCSVPIN.Text = ""
            txtAutoAuthCode.Text = ""
            txtManualAuth.Text = ""
            cboCustomer.SelectedIndex = -1
            m_sCCCustomer = ACCustomerNotPresent
            m_sCCNumber = ""
            'Start (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)
            txtTrackingNumber.Text = ""
            'End (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)

            'WPR12- Enhancement Quote Collection Process
            cboCardType.ListIndex = -1
            cboCCBank.ListIndex = -1
            txtCardTransSlipNo.Text = ""



        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ClearControls Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearControls", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
            result = gPMConstants.PMEReturnCode.PMFalse
        Finally
            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    '
    ' Name: MakeControlsMandatoryOrOptional
    '
    ' Description: Make labels of Mandatory controls BOLD (if the
    '              corresponding control is enabled).
    '
    ' ***************************************************************** '
    Private Function MakeControlsMandatoryOrOptional() As Integer

        Dim result As Integer = 0
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            lblCardNumber.Font = VB6.FontChangeBold(lblCardNumber.Font, True)
            If m_vUnderwriting = "U" Then
                lblNameOnCard.Font = VB6.FontChangeBold(lblNameOnCard.Font, False)
            Else
                If txtNameOnCard.Enabled Then
                    lblNameOnCard.Font = VB6.FontChangeBold(lblNameOnCard.Font, True)
                Else
                    lblNameOnCard.Font = VB6.FontChangeBold(lblNameOnCard.Font, False)
                End If
            End If
            If txtExpiryDate.Enabled Then
                lblExpiryDate.Font = VB6.FontChangeBold(lblExpiryDate.Font, True)
            Else
                lblExpiryDate.Font = VB6.FontChangeBold(lblExpiryDate.Font, False)
            End If

            ' Make the CSV/PIN textbox mandatory if we have an Issuer and the issuer has stated a CSV no. length
            If (txtCSVPIN.Enabled) And (m_lMediaTypeIssuerID <> 0) And (m_iCSVNumberLength > 0) Then
                lblCSVPIN.Font = VB6.FontChangeBold(lblCSVPIN.Font, True)
            Else
                lblCSVPIN.Font = VB6.FontChangeBold(lblCSVPIN.Font, False)
            End If

            If cboCustomer.Enabled Then
                lblCustomer.Font = VB6.FontChangeBold(lblCustomer.Font, True)
            Else
                lblCustomer.Font = VB6.FontChangeBold(lblCustomer.Font, False)
            End If

            'WPR12- Enhancement Quote Collection Process
            If IsAdditionalDetailOption Then
                lblCCBank.Font = VB6.FontChangeBold(lblCCBank.Font, True)
                lblNameOnCard.Font = VB6.FontChangeBold(lblNameOnCard.Font, True)
                lblCardType.Font = VB6.FontChangeBold(lblCardType.Font, True)
                lblCardTransSlipNo.Font = VB6.FontChangeBold(lblCardTransSlipNo.Font, True)
            Else
                lblCCBank.Font = VB6.FontChangeBold(lblCCBank.Font, False)
                lblNameOnCard.Font = VB6.FontChangeBold(lblNameOnCard.Font, False)
                lblCardType.Font = VB6.FontChangeBold(lblCardType.Font, False)
                lblCardTransSlipNo.Font = VB6.FontChangeBold(lblCardTransSlipNo.Font, False)
            End If


            ' Check whether to disable the Manual Auth. textbox (do so if there is a MediaTypeIssuerID
            ' AND there IS a MediaTypeIssuerConnectorCode set), else make enabled and mandatory.
            ' If we have a value in the manual auth textbox then enable it though (may have entered one and
            ' exited the screen and reloaded)
            If m_vUnderwriting = "U" Then
                If (m_lMediaTypeIssuerID <> 0) And (m_sMediaTypeIssuerConnectorCode <> "") And (txtManualAuth.Text = "") Then
                    txtManualAuth.Enabled = False
                    lblManualAuth.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                    lblManualAuth.Font = VB6.FontChangeBold(lblManualAuth.Font, False)
                    txtManualAuth.Text = ""
                Else
                    txtManualAuth.Enabled = True
                    lblManualAuth.ForeColor = ColorTranslator.FromOle(ACEnabledColor)
                    lblManualAuth.Font = VB6.FontChangeBold(lblManualAuth.Font, False)
                    txtAutoAuthCode.Text = ""
                End If
            Else
                If (m_lMediaTypeIssuerID <> 0) And (m_sMediaTypeIssuerConnectorCode <> "") And (txtManualAuth.Text = "") Then
                    txtManualAuth.Enabled = False
                    lblManualAuth.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                    lblManualAuth.Font = VB6.FontChangeBold(lblManualAuth.Font, False)
                    txtManualAuth.Text = ""
                Else
                    txtManualAuth.Enabled = True
                    lblManualAuth.ForeColor = ColorTranslator.FromOle(ACEnabledColor)
                    lblManualAuth.Font = VB6.FontChangeBold(lblManualAuth.Font, True)
                    txtAutoAuthCode.Text = ""
                End If
            End If


        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MakeControlsMandatoryOrOptional Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MakeControlsMandatoryOrOptional", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
            result = gPMConstants.PMEReturnCode.PMFalse
        Finally



        End Try
        Return result
    End Function


    ' ***************************************************************** '
    '
    ' Name: LoadCardNumberCombo
    '
    ' Description: Called in Payment mode when a combo of previously used
    '              card numbers needs to be loaded.
    '
    ' ***************************************************************** '
    Private Function LoadCardNumberCombo() As Integer

        Dim result As Integer = 0
        Dim iComboPosition As Integer
        Dim bComboItemMatch As Boolean

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            With cboCardNumber
                .Items.Clear()
                m_vPreviousCCData = Nothing
                ' If we don't have enough info to get the past cc numbers then don't even try!  PN?????
                If Not (m_lAccountID = 0 Or m_lMediaTypeIssuerID = 0) Then

                    ' Call business component to get list of past cc numbers used (note that we'll also return other info.
                    ' related to each cc that'll be required when we process the payment and also populated in the other
                    ' controls when we select a card number). Note that if we pass m_lInsuranceFileCnt then this will take
                    ' precedent over m_lAccountID for finding previously used card numbers

                    m_lReturn = g_oBusiness.GetPreviouslyUsedCCNumbers(v_lAccountID:=m_lAccountID, v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lMediaTypeIssuerID:=m_lMediaTypeIssuerID, v_bIsClaimTypePayment:=m_bIsClaimPaymentType, r_vOutputDetails:=m_vPreviousCCData)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("g_oBusiness.GetPreviouslyUsedCCNumbers", "sCallingAppName = " & ACApp & ", m_lAccountID:=" & CStr(m_lAccountID) & ", m_lInsuranceFileCnt:=" & CStr(m_lInsuranceFileCnt) & ", m_lMediaTypeIssuerID:=" & CStr(m_lMediaTypeIssuerID) & ", m_bIsClaimPaymentType:=" & CStr(m_bIsClaimPaymentType))
                    End If

                    ' Note that we'll use the data returned for validation (min value and max value) when the Validate method is
                    ' called and will also use the other values to load into the usercontrol controls when a combo item is clicked
                    If Information.IsArray(m_vPreviousCCData) Then
                        For iItem As Integer = 0 To m_vPreviousCCData.GetUpperBound(1)
                            Dim newIndex As Integer = -1
                            With cboCardNumber
                                newIndex = .Items.Add(New VB6.ListBoxItem(MaskedCC(CStr(m_vPreviousCCData(ACCCNumberArray_CCNumber, iItem))), iItem))
                            End With

                            ' If we have a value for the cc number already then save the index no. to position to later
                            If CStr(m_vPreviousCCData(ACCCNumberArray_CCNumber, iItem)).Trim() = m_sCCNumber.Trim() Then
                                iComboPosition = iItem
                                bComboItemMatch = True
                            End If
                        Next
                    End If
                End If

                ' Position to an item in the cc number combo if we found a match against a previously set value
                If bComboItemMatch Then
                    .SelectedIndex = iComboPosition
                    bComboItemMatch = False
                Else
                    .SelectedIndex = -1
                End If
            End With



        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadCardNumberCombo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadCardNumberCombo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
            result = gPMConstants.PMEReturnCode.PMFalse
        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: LoadCustomerCombo
    '
    ' Description: Loads the 'Customer' combo and positions to an item
    '              when applicable.
    '
    ' ***************************************************************** '
    Private Function LoadCustomerCombo() As Integer

        Dim result As Integer = 0
        Dim iComboPosition As Integer

        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            Dim newIndex As Integer = -1
            With cboCustomer
                .Items.Clear()
                newIndex = .Items.Add(New VB6.ListBoxItem(ACCustomerNotPresent, 0))

                If m_sCCCustomer = ACCustomerNotPresent Then
                    iComboPosition = 0
                End If

                newIndex = .Items.Add(New VB6.ListBoxItem(ACCustomerPresent, 1))

                If m_sCCCustomer = ACCustomerPresent Then
                    iComboPosition = 1
                End If

                ' Position to correct combo item (or Not Present which is the default)
                .SelectedIndex = iComboPosition

            End With



        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadCustomerCombo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadCustomerCombo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
            result = gPMConstants.PMEReturnCode.PMFalse
        Finally

        End Try
        Return result
    End Function

    Private Function MaskedCC(ByVal sCCNumber As String) As String
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: MaskedCC
        ' PURPOSE: Masks a Credit Card Number
        ' AUTHOR: Danny Davis
        ' DATE: 25 January 2005, 10:46:53
        ' RETURNS: Masked string
        ' CHANGES:
        ' ---------------------------------------------------------------------------
        Const kMask As String = "XXXXXXXXXXXXXXXXXXXX"

        sCCNumber = sCCNumber.Trim()
        Dim iLength As Integer = sCCNumber.Length

        Return kMask.Substring(0, iLength - 4) & sCCNumber.Substring(sCCNumber.Length - 4)
    End Function

    'Party Bank Details
    Public Function ShowPartyCreditCardScreen() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ShowPartyCreditCardScreen"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cboCardNumber.Visible = False
            lblCustomer.Visible = False
            cboCustomer.Visible = False
            txtStatus.Text = ""


        Catch ex As Exception
            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    Public Function AdditionalDetailOptions() As Integer


        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMFalse



            If m_bIsAdditionalDetailOption Then
                lblCardType.Visible = True
                cboCardType.Visible = True
                lblCCBank.Visible = True
                cboCCBank.Visible = True
                lblCardTransSlipNo.Visible = True
                txtCardTransSlipNo.Visible = True
            Else
                lblCardType.Visible = False
                cboCardType.Visible = False
                lblCCBank.Visible = False
                cboCCBank.Visible = False
                lblCardTransSlipNo.Visible = False
                txtCardTransSlipNo.Visible = False
            End If


        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AdditionalDetailOptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AdditionalDetailOptions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
            result = gPMConstants.PMEReturnCode.PMFalse
        Finally

        End Try
        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    Private Function EnableDisableAdditionalDetails() As Integer

        Dim result As Integer = 0
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            'WPR12- Enhancement Quote Collection Process
            If IsAdditionalDetailOption Then
                cboCardType.Visible = True
                lblCardType.Visible = True
                lblCardType.ForeColor = ColorTranslator.FromOle(ACEnabledColor)
                lblCardType.Font = VB6.FontChangeBold(lblCardType.Font, True)

                cboCCBank.Visible = True
                lblCCBank.Visible = True
                lblCCBank.ForeColor = ColorTranslator.FromOle(ACEnabledColor)
                lblCCBank.Font = VB6.FontChangeBold(lblCCBank.Font, True)

                txtCardTransSlipNo.Visible = True
                lblCardTransSlipNo.Visible = True
                lblCardTransSlipNo.ForeColor = ColorTranslator.FromOle(ACEnabledColor)
                lblCardTransSlipNo.Font = VB6.FontChangeBold(lblCardTransSlipNo.Font, True)
            Else
                cboCardType.Visible = False
                lblCardType.Visible = False

                cboCCBank.Visible = False
                lblCCBank.Visible = False

                txtCardTransSlipNo.Visible = False
                lblCardTransSlipNo.Visible = False
            End If



        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnableDisableAdditionalDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnableDisableAdditionalDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
            result = gPMConstants.PMEReturnCode.PMFalse
        Finally



        End Try
        Return result
    End Function


    Public Function SetSplitReceiptDefaults() As Integer
        Dim kMethodName As String = "SetSplitReceiptDefaults"
        Dim iResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        'Call this function when IsSplitReceipt = True and IsLeadAccount = False
        Try


            txtNameOnCard.Enabled = False
            txtCardNumber.Enabled = False
            txtExpiryDate.Enabled = False
            txtIssueNumber.Enabled = False
            txtCSVPIN.Enabled = False
            txtStartDate.Enabled = False
            txtManualAuth.Enabled = False
            cboCustomer.Enabled = False

            m_lReturn = MakeControlsMandatoryOrOptional()
            lblCardNumber.Font = VB6.FontChangeBold(lblCardNumber.Font, False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MakeControlsMandatoryOrOptional Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If


            txtNameOnCard.Text = CCName
            If CCNumber.Length >= 4 Then
                txtCardNumber.Text = "**** **** **** " & m_sCCNumber.Substring(CCNumber.Length - 4)
            End If

            txtExpiryDate.Text = CCExpiry
            txtIssueNumber.Text = CCIssue
            txtCSVPIN.Text = CCPIN
            txtStartDate.Text = CCStart
            txtManualAuth.Text = CCManualAuthCode

            If m_sCCCustomer = ACCustomerNotPresent Then
                cboCustomer.SelectedIndex = 0
            ElseIf m_sCCCustomer = ACCustomerPresent Then
                cboCustomer.SelectedIndex = 1
            End If

        Catch ex As Exception
            iResult = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetSplitReceiptDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
        End Try

        Return iResult

    End Function

    Private Sub txtCSVPIN_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCSVPIN.KeyPress
        ' Only allow 0-9,backspace and Ctrl+V
        If (Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57) And Asc(e.KeyChar) <> 8 And Asc(e.KeyChar) <> 22 Then
            e.KeyChar = ChrW(0)
        End If
    End Sub

    Private Sub txtCSVPIN_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCSVPIN.TextChanged
        If IsNumeric(txtCSVPIN.Text) = False Then
            txtCSVPIN.Text = ""
        End If
    End Sub

    Private Sub txtIssueNumber_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtIssueNumber.TextChanged
        If IsNumeric(txtIssueNumber.Text) = False Then
            txtIssueNumber.Text = ""
        End If
    End Sub

    Private Sub chkIsDefault_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsDefault.CheckedChanged
        If chkIsDefault.CheckState = CheckState.Checked Then
            Dim str As String = ""
            If Not (String.IsNullOrEmpty(DefaultAccountType) AndAlso String.IsNullOrEmpty(DefaultBankPaymentType)) Then
                str = "You have recently added a default credit card with" + vbCrLf + "Bank Payment Type:" + DefaultBankPaymentType + vbCrLf + "Account Type:" + DefaultAccountType + vbCrLf +
                    "Do you want to set this Credit Card as default?"

                If MessageBox.Show(str, "Default Credit Card Already Exist", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = DialogResult.No Then
                    chkIsDefault.CheckState = CheckState.Unchecked
                    m_nCCIsDefault = 0
                    Exit Sub
                Else
                    ResetPreviousOne = True
                End If
            End If
            'Dim nResult As Integer = 0
            'Dim oDefaultCreditcardDetails(,) As Object = Nothing
            'nResult = g_oBusiness.GetDefaultCreditCardByAccount(m_lAccountID, oDefaultCreditcardDetails)
            'If Information.IsArray(oDefaultCreditcardDetails) AndAlso oDefaultCreditcardDetails.Length > 0 Then
            '    str = ""

            '    str = "A Default Credit Card Already Exist with " + vbCrLf + "Bank Payment Type:" + oDefaultCreditcardDetails(0, 0) + vbCrLf + "Account Type:" + oDefaultCreditcardDetails(1, 0) + vbCrLf +
            '        "Do you want to set this Credit Card as default?"

            '    If MessageBox.Show(Str, "Default Credit Card Already Exist", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = DialogResult.No Then
            '        chkIsDefault.CheckState = CheckState.Unchecked
            '        m_nCCIsDefault = 0
            '        Exit Sub
            '    Else
            '        ResetPreviousOne = True
            '    End If
            'End If

        End If

        m_nCCIsDefault = chkIsDefault.CheckState
    End Sub

    Private Sub uctCreditCard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim sValue As String = "0"
        m_lReturn = iPMFunc.GetSystemOption(5199, sValue, 1) ' System option credit card processing method
        If sValue = "1" Then
            chkIsDefault.Visible = True
        Else
            chkIsDefault.Visible = False
        End If
    End Sub
End Class
