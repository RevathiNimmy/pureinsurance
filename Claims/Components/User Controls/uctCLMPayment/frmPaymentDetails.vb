Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
'developer guide no 129. 
Imports SharedFiles

Partial Friend Class frmPaymentDetails
    Inherits System.Windows.Forms.Form
    Private Sub frmPaymentDetails_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
        End If
    End Sub
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date:
    '
    ' Description: Main interface form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmInterface"

    ' constants
    Private Const kLayoutAdvancedTaxOn As Integer = 1
    Private Const kLayoutExcessRow As Integer = 2
    Private Const kLayoutGeneral As Integer = 3

    '********************************
    ' General Interface Property variables
    '********************************
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lPMAuthorityLevel As Integer
    Private m_bError As Integer
    Private m_oBusiness As Object
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_bInterfaceError As Boolean
    Private m_iUserId As Integer
    Private m_oFormFields As iPMFormControl.FormFields

    '********************************
    ' objects
    '********************************
    Private m_oCurrencyConvert As Object
    Private m_oPaymentMethod As Object

    '********************************
    ' specific interface property variables
    '********************************
    Private m_bViewPaymentMode As Boolean
    Private m_bAdvancedTaxScriptOptionOn As Boolean
    Private m_bAllowNegativeReserve As Boolean
    Private m_bLoading As Boolean
    Private m_lLayout As Integer
    Private m_crGOTFOCUSLCPaymentAmount As Decimal

    '********************************
    ' claim details
    '********************************
    Private m_lClaimSourceId As Integer
    Private m_crThisPaymentLossCurrency As Decimal
    Private m_lClaimId As Integer
    Private m_sRisktype As String = ""
    Private m_sLossCurrency As String = ""
    Private m_lLossCurrencyId As Integer
    Private m_lClaimPayableAccountId As Integer
    Private m_lClaimPerilId As Integer

    '********************************
    ' reserve details
    '********************************
    Private m_crTotalReserve As Decimal
    Private m_crPaidToDate As Decimal
    Private m_crBalance As Decimal

    '********************************
    ' payment details
    '********************************
    Private m_lPayeePartyId As Integer

    '********************************
    ' payment item details
    '********************************
    Private m_lReserveId As Integer
    Private m_sReserveType As String = ""
    Private m_lCurrencyId As Integer
    Private m_crThisPayment As Decimal
    Private m_lTaxGroupId As Integer
    Private m_crTaxAmount As Decimal
    Private m_crTaxAmountWHT As Decimal
    Private m_crExcessAmount As Decimal
    Private m_dCurrencyToBaseXRate As Double
    Private m_dtCurrencyToBaseDate As Date
    Private m_dAccountToBaseXRate As Double
    Private m_dtAccountToBaseDate As Date
    Private m_dSystemToBaseXRate As Double
    Private m_dtSystemToBaseDate As Date
    Private m_dPaymentToLossXRate As Double
    Private m_lExchangeRateOverrideReasonId As Integer
    Private m_sCurrencyDescription As String = ""
    Private m_sTaxGroupDescription As String = ""
    Private m_vTaxBandRate As Object
    Private m_sCurrencyCode As String = ""
    Private m_bIsExcess As Boolean
    Private m_cPaymentAdjustment As Decimal

    '********************************
    ' lookup details
    '********************************
    Private m_vTaxGroupArray(,) As Object
    Private m_vCurrencyArray(,) As Object
    Private m_vTaxGroupLookup As Object
    Private m_vTaxBandLookup As Object
    Private m_vTaxGroupTaxBandLookup(,) As Object
    Private m_vClassOfBusinessLookup As Object
    'Private m_
    Private m_oPaymentItem As cPaymentItem
    Private m_oTaxItem As cTaxParameters
    Private m_bIsWithHoldingTax As Boolean
    Private m_lClaimBaseCurrencyId As Integer
    Private m_sPreviousTaxGroup As String = ""
    Private m_lNoOfTaxBandRateRows As Integer
    Private m_sAdvancedTaxScript As String = ""
    Private m_bIsPostClaimTaxes As Boolean

    Private m_lPaymentCurrencyFilter As Integer
    Private m_bClaimPaymentIsGross As Boolean
    Private m_bOpenClaimNoTrans As Boolean
    Private crTaxAmount As Decimal
    Private dTaxAmount As Double
    Private m_bRI2007Enabled As Boolean
    Private m_lParentHwnd As Integer

    Private m_cTotalUnAuthorisedClaimPayment As Decimal
    Private m_cMaxUnAuthorisedClaimPayment As Decimal
    Private m_bShowAsHidden As Boolean
    Private m_bPaymentCannotExceedReserve As Boolean
    Private m_bShowPaymentDetailsHiddenMode As Boolean
    'ATS
    Private m_cTotalTaxPaid As Decimal
    Private m_bTaxGroupMandatory As Boolean
    Private m_vErrorMessage As Object
    Private m_bIsTaxOverridden As Boolean
    Private m_sPreviousTaxAmount As String


    Public Property ClaimPaymentIsGross() As Boolean
        Get
            Return m_bClaimPaymentIsGross
        End Get
        Set(ByVal Value As Boolean)
            m_bClaimPaymentIsGross = Value
        End Set
    End Property

    Public WriteOnly Property IsPostClaimTaxes() As Boolean
        Set(ByVal Value As Boolean)
            m_bIsPostClaimTaxes = Value
        End Set
    End Property

    Public WriteOnly Property ClassOfBusinessLookup() As Object
        Set(ByVal Value As Object)


            m_vClassOfBusinessLookup = Value
        End Set
    End Property

    Public WriteOnly Property TaxGroupTaxBandLookup() As Object
        Set(ByVal Value As Object)
            m_vTaxGroupTaxBandLookup = Value
        End Set
    End Property
    Public WriteOnly Property TaxGroupLookup() As Object
        Set(ByVal Value As Object)


            m_vTaxGroupLookup = Value
        End Set
    End Property
    Public WriteOnly Property TaxBandLookup() As Object
        Set(ByVal Value As Object)


            m_vTaxBandLookup = Value
        End Set
    End Property

    Public WriteOnly Property TaxItem() As cTaxParameters
        Set(ByVal Value As cTaxParameters)
            m_oTaxItem = Value
        End Set
    End Property


    Public Property IsExcess() As Boolean
        Get
            Return m_bIsExcess
        End Get
        Set(ByVal Value As Boolean)
            m_bIsExcess = Value
        End Set
    End Property

    Public ReadOnly Property ExchangeRateOverrideReasonId() As Integer
        Get
            Return m_lExchangeRateOverrideReasonId
        End Get
    End Property

    Public WriteOnly Property PaymentCurrencyFilter() As Integer
        Set(ByVal Value As Integer)
            m_lPaymentCurrencyFilter = Value
        End Set
    End Property

    Public WriteOnly Property AllowNegativeReserve() As Boolean
        Set(ByVal Value As Boolean)
            m_bAllowNegativeReserve = Value
        End Set
    End Property


    Public Property TaxBandRateArray() As Object
        Get
            Return m_vTaxBandRate
        End Get
        Set(ByVal Value As Object)


            m_vTaxBandRate = Value
        End Set
    End Property

    Public Property CurrencyCode() As String
        Get
            Return m_sCurrencyCode
        End Get
        Set(ByVal Value As String)
            m_sCurrencyCode = Value
        End Set
    End Property


    Public Property PayeePartyId() As Integer
        Get
            Return m_lPayeePartyId
        End Get
        Set(ByVal Value As Integer)
            m_lPayeePartyId = Value
        End Set
    End Property


    Public Property AdvancedTaxScript() As String
        Get
            Return m_sAdvancedTaxScript
        End Get
        Set(ByVal Value As String)
            m_sAdvancedTaxScript = Value
        End Set
    End Property


    Public Property ClaimPerilId() As Integer
        Get
            Return m_lClaimPerilId
        End Get
        Set(ByVal Value As Integer)
            m_lClaimPerilId = Value
        End Set
    End Property

    Public WriteOnly Property Business() As Object
        Set(ByVal Value As Object)
            m_oBusiness = Value
        End Set
    End Property

    Public WriteOnly Property ClaimBaseCurrencyId() As Integer
        Set(ByVal Value As Integer)
            m_lClaimBaseCurrencyId = Value
        End Set
    End Property

    Public Property IsWithHoldingTax() As Boolean
        Get
            Return m_bIsWithHoldingTax
        End Get
        Set(ByVal Value As Boolean)
            m_bIsWithHoldingTax = Value
        End Set
    End Property

    Public WriteOnly Property ClaimPayableAccountId() As Integer
        Set(ByVal Value As Integer)
            m_lClaimPayableAccountId = Value
        End Set
    End Property

    Public WriteOnly Property ClaimId() As Integer
        Set(ByVal Value As Integer)
            m_lClaimId = Value
        End Set
    End Property

    Public WriteOnly Property UserId() As Integer
        Set(ByVal Value As Integer)
            m_iUserId = Value
        End Set
    End Property


    Public Property PaymentItem() As cPaymentItem
        Get
            Return m_oPaymentItem
        End Get
        Set(ByVal Value As cPaymentItem)
            m_oPaymentItem = Value
        End Set
    End Property


    Public Property TaxGroupDescription() As String
        Get
            Return m_sTaxGroupDescription
        End Get
        Set(ByVal Value As String)
            m_sTaxGroupDescription = Value
        End Set
    End Property


    Public Property CurrencyDescription() As String
        Get
            Return m_sCurrencyDescription
        End Get
        Set(ByVal Value As String)
            m_sCurrencyDescription = Value
        End Set
    End Property


    Public Property TotalReserve() As Decimal
        Get
            Return m_crTotalReserve
        End Get
        Set(ByVal Value As Decimal)
            m_crTotalReserve = Value
        End Set
    End Property


    Public Property PaidToDate() As Decimal
        Get
            Return m_crPaidToDate
        End Get
        Set(ByVal Value As Decimal)
            m_crPaidToDate = Value
        End Set
    End Property

    Public WriteOnly Property Balance() As Decimal
        Set(ByVal Value As Decimal)
            m_crBalance = Value
        End Set
    End Property

    Public WriteOnly Property CurrencyConvert() As Object
        Set(ByVal Value As Object)
            m_oCurrencyConvert = Value
        End Set
    End Property

    Public WriteOnly Property PaymentMethod() As Object
        Set(ByVal Value As Object)
            m_oPaymentMethod = Value
        End Set
    End Property

    Public WriteOnly Property ViewPaymentMode() As Boolean
        Set(ByVal Value As Boolean)
            m_bViewPaymentMode = Value
        End Set
    End Property

    Public WriteOnly Property LossCurrencyID() As Integer
        Set(ByVal Value As Integer)
            m_lLossCurrencyId = Value
        End Set
    End Property

    Public WriteOnly Property ClaimSourceId() As Integer
        Set(ByVal Value As Integer)
            m_lClaimSourceId = Value
        End Set
    End Property

    Public WriteOnly Property TaxGroupArray() As Object
        Set(ByVal Value As Object)
            m_vTaxGroupArray = Value
        End Set
    End Property

    Public WriteOnly Property CurrencyArray() As Object
        Set(ByVal Value As Object)
            m_vCurrencyArray = Value
        End Set
    End Property


    Public Property LossCurrency() As String
        Get
            Return m_sLossCurrency
        End Get
        Set(ByVal Value As String)
            m_sLossCurrency = Value
        End Set
    End Property


    Public Property ReserveType() As String
        Get
            Return m_sReserveType
        End Get
        Set(ByVal Value As String)
            m_sReserveType = Value
        End Set
    End Property


    Public Property RiskType() As String
        Get
            Return m_sRisktype
        End Get
        Set(ByVal Value As String)
            m_sRisktype = Value
        End Set
    End Property


    Public Property ReserveId() As Integer
        Get
            Return m_lReserveId
        End Get
        Set(ByVal Value As Integer)
            m_lReserveId = Value
        End Set
    End Property


    Public Property CurrencyId() As Integer
        Get
            Return m_lCurrencyId
        End Get
        Set(ByVal Value As Integer)
            m_lCurrencyId = Value
        End Set
    End Property


    Public Property ThisPayment() As Decimal
        Get
            Return m_crThisPayment
        End Get
        Set(ByVal Value As Decimal)
            m_crThisPayment = Value
        End Set
    End Property


    Public Property TaxGroupId() As Integer
        Get
            Return m_lTaxGroupId
        End Get
        Set(ByVal Value As Integer)
            m_lTaxGroupId = Value
        End Set
    End Property


    Public Property TaxAmount() As Decimal
        Get
            Return m_crTaxAmount
        End Get
        Set(ByVal Value As Decimal)
            m_crTaxAmount = Value
        End Set
    End Property


    Public Property TaxAmountWHT() As Decimal
        Get
            Return m_crTaxAmountWHT
        End Get
        Set(ByVal Value As Decimal)
            m_crTaxAmountWHT = Value
        End Set
    End Property


    Public Property ExcessAmount() As Decimal
        Get
            Return m_crExcessAmount
        End Get
        Set(ByVal Value As Decimal)
            m_crExcessAmount = Value
        End Set
    End Property


    Public Property SystemToBaseXRate() As Double
        Get
            Return m_dSystemToBaseXRate
        End Get
        Set(ByVal Value As Double)
            m_dSystemToBaseXRate = Value
        End Set
    End Property


    Public Property SystemToBaseDate() As Date
        Get
            Return m_dtSystemToBaseDate
        End Get
        Set(ByVal Value As Date)
            m_dtSystemToBaseDate = Value
        End Set
    End Property


    Public Property AccountToBaseXRate() As Double
        Get
            Return m_dAccountToBaseXRate
        End Get
        Set(ByVal Value As Double)
            m_dAccountToBaseXRate = Value
        End Set
    End Property


    Public Property AccountToBaseDate() As Date
        Get
            Return m_dtAccountToBaseDate
        End Get
        Set(ByVal Value As Date)
            m_dtAccountToBaseDate = Value
        End Set
    End Property


    Public Property CurrencyToBaseXRate() As Double
        Get
            Return m_dCurrencyToBaseXRate
        End Get
        Set(ByVal Value As Double)
            m_dCurrencyToBaseXRate = Value
        End Set
    End Property


    Public Property CurrencyToBaseDate() As Date
        Get
            Return m_dtCurrencyToBaseDate
        End Get
        Set(ByVal Value As Date)
            m_dtCurrencyToBaseDate = Value
        End Set
    End Property


    Public Property PaymentToLossXRate() As Double
        Get
            Return m_dPaymentToLossXRate
        End Get
        Set(ByVal Value As Double)
            m_dPaymentToLossXRate = Value
        End Set
    End Property

    Public WriteOnly Property AdvancedTaxScriptOptionOn() As Boolean
        Set(ByVal Value As Boolean)
            m_bAdvancedTaxScriptOptionOn = Value
        End Set
    End Property

    Public ReadOnly Property PaymentAdjustment() As Decimal
        Get
            Return m_cPaymentAdjustment
        End Get
    End Property

    Public WriteOnly Property TotalUnAuthorisedClaimPayment() As Decimal
        Set(ByVal Value As Decimal)
            m_cTotalUnAuthorisedClaimPayment = Value
        End Set
    End Property

    Public WriteOnly Property MaxUnAuthorisedClaimPayment() As Decimal
        Set(ByVal Value As Decimal)
            m_cMaxUnAuthorisedClaimPayment = Value
        End Set
    End Property

    '********************************
    ' General Interface Properties
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get
            Return m_lStatus
        End Get
    End Property

    Public ReadOnly Property Error_Renamed() As Integer
        Get
            Return m_bError
        End Get
    End Property

    Public WriteOnly Property PaymentCannotExceedReserve() As Boolean
        Set(ByVal Value As Boolean)
            m_bPaymentCannotExceedReserve = Value
        End Set
    End Property

    Public WriteOnly Property ShowPaymentDetailsHiddenMode() As Boolean
        Set(ByVal Value As Boolean)
            m_bShowPaymentDetailsHiddenMode = Value
        End Set
    End Property


    Public Property TotalTaxPaid() As Decimal
        Get
            Return m_cTotalTaxPaid
        End Get
        Set(ByVal Value As Decimal)
            m_cTotalTaxPaid = Value
        End Set
    End Property

    ' Property for Error Message in ATS
    Public Property ErrorMessage() As Object
        Get
            Return m_vErrorMessage
        End Get
        Set(ByVal Value As Object)
            m_vErrorMessage = Value
        End Set
    End Property

    Public WriteOnly Property IsOpenClaimNoTrans() As Boolean
        Set(ByVal Value As Boolean)
            m_bOpenClaimNoTrans = Value
        End Set
    End Property

    Public WriteOnly Property RI2007Enabled() As Boolean
        Set(ByVal Value As Boolean)
            m_bRI2007Enabled = Value
        End Set
    End Property
    '********************************

    Private Sub cboCurrency_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCurrency.SelectedIndexChanged
        ActionTransasctionCurrencySelection()
    End Sub

    Private Sub cboTaxGroup_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTaxGroup.SelectedIndexChanged
        If cboCurrency.SelectedIndex <> -1 Then
            ActionTaxGroupSelection()
            ' save the selected tax group
            m_sPreviousTaxGroup = cboTaxGroup.Text
            m_sPreviousTaxAmount = txtTaxAmount.Text
            CalculateLossCurrencyAmounts()
        End If
    End Sub

    Private Sub chkReverseExcessPayment_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkReverseExcessPayment.CheckStateChanged
        CalculateLossCurrencyAmounts()
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        'Clear for PN65840
        If m_oPaymentItem.TaxGroupId = -1 And Not m_bViewPaymentMode And m_bAdvancedTaxScriptOptionOn = True Then
            m_oPaymentItem.AccountToBaseDate = DateTime.Today
            m_oPaymentItem.AccountToBaseXRate = 0
            m_oPaymentItem.CurrencyToBaseDate = DateTime.Today
            m_oPaymentItem.CurrencyToBaseXRate = 0
            m_oPaymentItem.CurrencyId = 0
            m_oPaymentItem.PaymentToLossXRate = 0
            m_oPaymentItem.ReserveId = 0
            m_oPaymentItem.SystemToBaseDate = DateTime.Today
            m_oPaymentItem.SystemToBaseXRate = 0
            m_oPaymentItem.TaxAmount = 0
            m_oPaymentItem.TaxAmountWHT = 0
            m_oPaymentItem.TaxGroupId = -1
            m_oPaymentItem.ThisPayment = 0
        End If

        Me.Close()
    End Sub

    Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
        ActionOk()
    End Sub

    ' ***************************************************************** '
    ' Name: Form_Initialize
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Sub Form_Initialize_Renamed()

        Const kMethodName As String = "Form_Initialize"

        Dim lReturn, lSubValue As Integer

        Try



            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' initialise form error indicator
            m_bError = False

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Get an instance of the business object via
            ' the public object manager.
            '    lReturn = g_oObjectManager.GetInstance(oObject:=m_oBusiness, _
            'sClassName:="BUSINESS", _
            'vInstanceManager:=PMGetViaClientManager)
            '    If lReturn <> PMTrue Then
            '        RaiseError kMethodName, "Failed to get instance of BUSINESS", PMLogError
            '    End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: Form_QueryUnload
    '
    ' Description: Determines whether any actions need to take place
    '               before unload.
    ' History:
    '           Created : MEvans : Date : Process Id
    ' ***************************************************************** '
    Private Sub frmPaymentDetails_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)


        eventArgs.Cancel = Cancel <> 0
    End Sub

    ' ***************************************************************** '
    ' Name: Form_Unload
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Sub frmPaymentDetails_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

        Const kMethodName As String = "Form_Unload"

        Dim lReturn, lSubValue As Integer

        Try



            ' Create an instance of the form control object.
            m_oFormFields = Nothing

            ' Terminate the business object
            'lReturn = m_oBusiness.Terminate()
            ' Check for errors.
            'If lReturn <> PMTrue Then
            '    RaiseError kMethodName, "Failed to terminate the business object", PMLogError
            'End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' destroy object reference
            m_oBusiness = Nothing



        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: Form_Load
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '

    Public Sub frmPaymentDetails_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Const kMethodName As String = "Form_Load"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lSubValue As Integer
        Dim sResult As String = ""

        Try



            ' option 5063
            lReturn = CType(iPMFunc.GetSystemOption(v_iOptionNumber:=5063, r_sOptionValue:=sResult), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSystemOption Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If sResult.Trim() = "1" Then
                m_bTaxGroupMandatory = True
            End If

            ' set up interface
            lReturn = SetupForm()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupForm Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: SetupForm
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 11-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function SetupForm() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupForm"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' resize form
            lReturn = ResizeForm()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not m_bViewPaymentMode Then

                ' setup form fields
                lReturn = SetupFormFields()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SetupFormFields Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' populate currency combo
                lReturn = CType(PopulateCombo(cboCurrency, m_vCurrencyArray), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "PopulateCombo = ClaimPaymentTo Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' populate tax group combo
                lReturn = CType(PopulateCombo(cboTaxGroup, m_vTaxGroupArray, kTaxGroupNone), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "PopulateCombo = ClaimPaymentTo Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If

            ' populate the form data
            lReturn = PopulateFormData()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateFormData Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = SetFormLayout()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetFormLayout Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            Return PMEReturnCode.PMTrue

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    ''' <summary>
    ''' PopulateFormData
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Created : MEvans : 11-08-2005 : 360 - Taxes on Claims</remarks>
    Public Function PopulateFormData() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateFormData"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_bLoading = True

            If Not m_bViewPaymentMode Then

                ' populate selected combo values
                If m_lPaymentCurrencyFilter <> 0 Then
                    ' select the specified filter currency
                    ' and stop the user changing the selection
                    SelectcboItem(cboCurrency, m_lPaymentCurrencyFilter)

                    cboCurrency.Visible = False
                    txtCurrency.Text = cboCurrency.Text
                    txtCurrency.Visible = True
                Else
                    If m_lCurrencyId = 0 Then
                        SelectcboItem(cboCurrency, m_lLossCurrencyId)
                    Else
                        'SelectcboItem(cboCurrency, m_lCurrencyId)
                        'Select Loss Currency by default
                        m_lCurrencyId = m_lLossCurrencyId

                        ' get the transaction (payment) to loss currency exchange rate
                        result = GetPaymentLossExchangeRate()
                        If result <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "GetPaymentLossExchangeRate Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        ' get the currency code
                        If Information.IsArray(m_vCurrencyArray) Then

                            ' get array boundaries
                            Dim llBound As Integer = m_vCurrencyArray.GetLowerBound(1)
                            Dim lUBound As Integer = m_vCurrencyArray.GetUpperBound(1)

                            ' find the selected item and get its code
                            For lCurrencyItem As Integer = llBound To lUBound
                                If CDbl(m_vCurrencyArray(kLookupItemId, lCurrencyItem)) = m_lCurrencyId Then
                                    SelectcboItem(cboCurrency, m_lCurrencyId)
                                    m_sCurrencyCode = CStr(m_vCurrencyArray(kLookupCode, lCurrencyItem))
                                End If
                            Next lCurrencyItem

                        End If

                    End If
                End If

                If m_bIsWithHoldingTax Then
                    crTaxAmount = m_crTaxAmountWHT
                Else
                    crTaxAmount = m_crTaxAmount
                End If

                SelectcboItem(cboTaxGroup, m_lTaxGroupId)
                txtCurrency.Text = cboCurrency.Text
                txtTaxGroup.Text = cboTaxGroup.Text
                If Not m_bClaimPaymentIsGross Then
                    txtPaymentAmount.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_crThisPayment)
                Else
                    'ATS
                    If Not m_bIsExcess And m_cTotalTaxPaid > 0 Then
                        'txtPaymentAmount.Text = StringsHelper.Format(m_crThisPayment + m_cTotalTaxPaid, "0.00")
                        txtPaymentAmount.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_crThisPayment + m_cTotalTaxPaid)
                    Else
                        'txtPaymentAmount.Text = StringsHelper.Format(m_crThisPayment + m_crTaxAmount + m_crTaxAmountWHT, "0.00")
                        txtPaymentAmount.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_crThisPayment + m_crTaxAmount + m_crTaxAmountWHT)
                    End If
                End If

            Else

                txtCurrency.Text = m_sCurrencyDescription
                txtTaxGroup.Text = m_sTaxGroupDescription
                'txtPaymentAmount.Text = StringsHelper.Format(m_crThisPayment, "0.00")
                txtPaymentAmount.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_crThisPayment)

            End If

            ' claim
            txtRiskType.Text = m_sRisktype
            'txtTotalReserve.Text = StringsHelper.Format(m_crTotalReserve, "0.00")
            txtTotalReserve.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_crTotalReserve)
            'txtPaidToDate.Text = StringsHelper.Format(m_crPaidToDate, "0.00")
            txtPaidToDate.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_crPaidToDate)
            'txtBalance.Text = StringsHelper.Format(m_crBalance, "0.00")
            txtBalance.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_crBalance)

            ' reserve
            txtReserveType.Text = m_sReserveType

            ' payment

            ' if this is an excess row
            If m_bIsExcess Then
                ' if the payment amount is greater than zero
                If m_crThisPayment > 0 Then
                    ' it must have been a reversal so set checkbox
                    chkReverseExcessPayment.CheckState = CheckState.Checked
                End If
            End If

            txtCurrencyRate.Text = CStr(m_dPaymentToLossXRate)

            If Not m_bClaimPaymentIsGross Then
                txtPaymentAmount.Text = StringsHelper.Format(CStr(m_crThisPayment), "0.00")
            Else
                If Not m_bIsExcess AndAlso m_cTotalTaxPaid > 0 Then
                    txtPaymentAmount.Text = StringsHelper.Format(CStr(m_crThisPayment + m_cTotalTaxPaid), "0.00")
                Else
                    txtPaymentAmount.Text = StringsHelper.Format(CStr(m_crThisPayment + m_crTaxAmount + m_crTaxAmountWHT), "0.00")
                End If
            End If

            ' taxes
            If cboTaxGroup.Text = "(none)" Or cboTaxGroup.Text = "" Then

                txtTaxAmount.ReadOnly = True
                txtTaxAmount.BackColor = ColorTranslator.FromOle(-2147483633)
                txtTaxAmount.Text = "0.00"
            Else
                txtTaxAmount.Text = StringsHelper.Format(m_crTaxAmount + m_crTaxAmountWHT, "0.00")
            End If
            ' total
            'txtPaymentTotal.Text = StringsHelper.Format(m_crTaxAmount + m_crTaxAmountWHT + m_crThisPayment, "0.00")
            txtPaymentTotal.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_crTaxAmount + m_crTaxAmountWHT + m_crThisPayment)

            ' excess
            'txtExcessAmount.Text = Format(m_crExcessAmount, "0.00")

            ' loss currency
            txtLossCurrency.Text = m_sLossCurrency

            ' calculate loss currency amounts
            result = CalculateLossCurrencyAmounts()
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CalculateLossCurrencyAmounts Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            result = gPMConstants.PMEReturnCode.PMTrue

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            m_bLoading = False

        End Try
        Return result
    End Function
    ' ***************************************************************** '
    ' Name: PopulateCombo
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 09-08-2005 : 260 - Taxes on Claims
    ' ***************************************************************** '
    Private Function PopulateCombo(ByRef r_oComboBox As ComboBox, ByVal v_vValuesArray(,) As Object, Optional ByVal v_sDefaultEntry As String = "", Optional ByVal v_sNullEntry As String = "") As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "PopulateCombo"

        Dim nReturn As Integer
        Dim sDescription As String = ""
        Dim nItemId As Integer
        Dim sCode As String = ""
        Dim nlBound As Integer
        Dim nUBound As Integer
        Dim nComboItem As Integer

        Try



            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' clear combo of all entries
            r_oComboBox.Items.Clear()
            nComboItem = 0
            'PN 45184
            ' if there is a null entry add it to the combo
            If v_sNullEntry <> "" Then
                r_oComboBox.Items.Insert(nComboItem, v_sNullEntry.Trim())
                VB6.SetItemData(r_oComboBox, nComboItem, -1)
                nComboItem = 1
            End If

            ' if there is a default entry add it to the combo
            If v_sDefaultEntry <> "" Then
                r_oComboBox.Items.Insert(nComboItem, v_sDefaultEntry)
                VB6.SetItemData(r_oComboBox, nComboItem, 0)
                nComboItem += 1
                r_oComboBox.SelectedIndex = 0
            End If

            If Information.IsArray(v_vValuesArray) Then

                ' get the array boundaries
                nlBound = v_vValuesArray.GetLowerBound(1)
                nUBound = v_vValuesArray.GetUpperBound(1)

                ' for each item in the array
                For lItem As Integer = nlBound To nUBound

                    ' get item details

                    nItemId = CInt(v_vValuesArray(kLookupItemId, lItem))

                    sDescription = CStr(v_vValuesArray(kLookupDescription, lItem))

                    ' add item to combo
                    r_oComboBox.Items.Insert(nComboItem, sDescription)
                    VB6.SetItemData(r_oComboBox, nComboItem, nItemId)

                    nComboItem += 1

                Next

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
    ' Name: SelectcboItem
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 08-08-2005 : 360 - Taxes On Claims
    ' ***************************************************************** '
    Private Function SelectcboItem(ByRef r_oCbo As ComboBox, ByVal v_lSelectedId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SelectcboItem"

        Dim bItemNotFound As Boolean

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            bItemNotFound = True

            ' if the item id is valid
            If v_lSelectedId <> -1 Then

                ' for each item in the list
                For lItem As Integer = 0 To r_oCbo.Items.Count

                    ' search the item data array for a match
                    If VB6.GetItemData(r_oCbo, lItem) = v_lSelectedId Then

                        ' found a match - select the item
                        r_oCbo.SelectedIndex = lItem
                        bItemNotFound = False
                        Exit For
                    End If

                Next lItem


                If bItemNotFound Then

                    ' log that we havent found the specified item
                    Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                    oDict.Add("v_lSelectedId", v_lSelectedId)
                    gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed to find item with id:" & CStr(v_lSelectedId) & " in :" & r_oCbo.Name, vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, oDicParms:=oDict)

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

    ' ***************************************************************** '
    ' Name: GetPaymentLossExchangeRate
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 11-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function GetPaymentLossExchangeRate() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPaymentLossExchangeRate"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim dPaymentToBaseXRate, dLossCurrencyToBaseXRate As Double

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lCurrencyId <> m_lLossCurrencyId Then

                ' Get payment rate to base

                lReturn = m_oCurrencyConvert.GetCurrencyRate(v_lCurrencyId:=m_lCurrencyId, v_lCompanyID:=m_lClaimSourceId, v_dtConversionDate:=DateTime.Today, r_vConversionRate:=dPaymentToBaseXRate)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to Rate Against Base for Payment Currency", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' Get loss rate to base

                lReturn = m_oCurrencyConvert.GetCurrencyRate(v_lCurrencyId:=m_lLossCurrencyId, v_lCompanyID:=m_lClaimSourceId, v_dtConversionDate:=DateTime.Today, r_vConversionRate:=dLossCurrencyToBaseXRate)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                        ' msgbox
                        MessageBox.Show("Currency Rate Not Found for ", Application.ProductName)
                    Else
                        gPMFunctions.RaiseError(kMethodName, "Failed to Rate Against Base for Loss Currency", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If

                If dPaymentToBaseXRate = 0 Then
                    gPMFunctions.RaiseError(kMethodName, "Payment To Base Exchange Rate set to 0", gPMConstants.PMELogLevel.PMLogError)
                End If

                If dLossCurrencyToBaseXRate = 0 Then
                    gPMFunctions.RaiseError(kMethodName, "Loss Currency To Base Exchange Rate set to 0", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' Calculate payment rate to loss
                m_dPaymentToLossXRate = dPaymentToBaseXRate / dLossCurrencyToBaseXRate

            Else
                m_dPaymentToLossXRate = 1
            End If

            'txtCurrencyRate.Text = Format(m_dPaymentToLossXRate, "0.0000")
            txtCurrencyRate.Text = StringsHelper.Format(CStr(m_dPaymentToLossXRate), "0.00")


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    ''' <summary>
    ''' ShowMultiCurrencyDialogue
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Created : MEvans : Date : Process ID</remarks>
    Public Function ShowMultiCurrencyDialogue() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ShowMultiCurrencyDialogue"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bChangeDate, bChangeRate, bCanChangeCurrency As Boolean
        Dim iBaseCurrencyID, iClaimCurrencyID As Integer
        Dim lStatus As gPMConstants.PMEReturnCode
        Dim sResult As String = ""
        Dim oForm As frmMultiCurrency
        Dim vClaimBaseCurrencyDetails As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get user authorities

            result = m_oPaymentMethod.GetUserCurrencyAuthorities(v_iUserId:=m_iUserId, r_bChangeDate:=bChangeDate, r_bChangeRate:=bChangeRate)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to get user currency authorities from business")
            End If

            ' either option set to true will do
            bCanChangeCurrency = (bChangeDate Or bChangeRate)

            'm_lClaimCurrencyID
            ' get the claim base currency details

            result = m_oPaymentMethod.GetClaimBaseCurrencyDetails(v_lClaimId:=m_lClaimId, r_vResults:=vClaimBaseCurrencyDetails)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to get claim base branch currency from business")
            Else
                If Information.IsArray(vClaimBaseCurrencyDetails) Then

                    iBaseCurrencyID = CInt(vClaimBaseCurrencyDetails(0, 0))
                Else
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to get claim base branch currency from business")
                End If
            End If

            ' option 157
            result = CType(iPMFunc.GetSystemOption(v_iOptionNumber:=157, r_sOptionValue:=sResult), gPMConstants.PMEReturnCode)

            oForm = New frmMultiCurrency()

            m_lParentHwnd = oForm.Handle.ToInt32()
            KeepWindowOnTop(True)

            'Set up multi-currency screen
            oForm.TransactionCurrencyID = m_lCurrencyId
            oForm.TransactionAmount = m_crThisPayment
            oForm.SourceID = m_lClaimSourceId
            oForm.PartyCnt = m_lPayeePartyId
            oForm.ClaimId = m_lClaimId
            oForm.ScreenMethod = kScreenMethodPayment
            oForm.LossCurrencyAmount = m_crThisPaymentLossCurrency
            oForm.LossCurrencyID = m_lLossCurrencyId
            oForm.ClaimPayableAccountId = m_lClaimPayableAccountId

            'developer guide no.9
            result = oForm.Initialise()
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "frmPaymentDetails.Initialise", gPMConstants.PMELogLevel.PMLogError)
            End If

            If (bCanChangeCurrency Or sResult.Trim() = "1") And Not (m_lCurrencyId = iBaseCurrencyID) Then
                'show the form
                oForm.ShowDialog()
                lStatus = oForm.Status
            Else
                ' claim currency is base currency, so silently save the rates
                result = oForm.InterfaceToProperties()
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "frmMultiCurrency.InterfaceToProperties Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                lStatus = gPMConstants.PMEReturnCode.PMOK
            End If

            If lStatus = gPMConstants.PMEReturnCode.PMOK Then
                m_dtAccountToBaseDate = oForm.XChangeRateDate
                m_dtSystemToBaseDate = oForm.XChangeRateDate
                m_dtCurrencyToBaseDate = oForm.XChangeRateDate
                m_dAccountToBaseXRate = oForm.AccountToBaseXRate
                m_dCurrencyToBaseXRate = oForm.CurrencyToBaseXRate
                m_dSystemToBaseXRate = oForm.SystemToBaseXRate
                m_lExchangeRateOverrideReasonId = oForm.OverrideId
                'Override the exchange rate
                If m_lExchangeRateOverrideReasonId > 0 And m_lCurrencyId <> m_lLossCurrencyId And oForm.uctSIRMultiCurrency1.BaseCurrencyID = m_lCurrencyId Then
                    m_dPaymentToLossXRate = m_dCurrencyToBaseXRate
                End If
                If m_lCurrencyId <> m_lLossCurrencyId And oForm.uctSIRMultiCurrency1.BaseCurrencyID <> m_lCurrencyId Then
                    m_dPaymentToLossXRate = oForm.LossCurrencyAmount / (oForm.uctSIRMultiCurrency1.BaseAmount / m_dCurrencyToBaseXRate)
                End If
            End If
            result = gPMConstants.PMEReturnCode.PMTrue

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            result = lStatus
            oForm = Nothing

        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: ActionOk
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 11-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function ActionOk() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ActionOk"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            If m_bShowAsHidden Then
                m_bLoading = False
                m_sPreviousTaxGroup = ""
                ActionTaxGroupSelection()
            End If

            If Not m_bViewPaymentMode Then

                ' validate the data that has been entered
                lReturn = ValidateFormData()
                If lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                    ' save form data back to the local variables
                    lReturn = SaveFormData()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "SaveFormData Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' only get the currency's once for the first item
                    ' if they are invalid or a mistake has been made
                    ' the item can be cleared (deleted) and the rates
                    ' will be taken from the next item.
                    If Not m_bOpenClaimNoTrans Then
                        If m_lPaymentCurrencyFilter = 0 Then

                            ' show the multi currency dialogue or at least call it to get the
                            ' values for the payment item
                            lReturn = ShowMultiCurrencyDialogue()
                            If lReturn <> gPMConstants.PMEReturnCode.PMOK Then
                                If lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                                    Return result
                                Else
                                    gPMFunctions.RaiseError(kMethodName, "ShowMultiCurrencyDialogue Failed", gPMConstants.PMELogLevel.PMLogError)
                                End If
                            End If
                        End If
                    End If
                    ' set return status
                    m_lStatus = gPMConstants.PMEReturnCode.PMOK

                    Me.Close()

                End If

            Else
                ' set return status
                m_lStatus = gPMConstants.PMEReturnCode.PMOK

                Me.Close()

            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    Public Function CalculateLossCurrencyAmounts() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CalculateLossCurrencyAmounts"

        Dim dPaymentAmount, dLCTaxAmount, dLCPaymentAmount As Double

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_bIsTaxOverridden Then
                crTaxAmount = gPMFunctions.ToSafeDecimal(txtTaxAmount.Text, 0)
            End If

            If gPMFunctions.ToSafeDouble(txtCurrencyRate.Text, 0) <> 0 Then

                If m_bIsExcess Then
                    If m_crGOTFOCUSLCPaymentAmount <> gPMFunctions.ToSafeCurrency(txtLCPaymentAmount.Text, 0) Then
                        If m_dPaymentToLossXRate <> 0 Then
                            'txtPaymentAmount.Text = StringsHelper.Format(gPMFunctions.ToSafeCurrency(txtLCPaymentAmount.Text, 0) / m_dPaymentToLossXRate, "0.00")
                            txtPaymentAmount.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, gPMFunctions.ToSafeCurrency(txtLCPaymentAmount.Text, 0) / m_dPaymentToLossXRate)
                        End If
                    End If
                End If

                dPaymentAmount = gPMFunctions.ToSafeDouble(txtPaymentAmount.Text, 0)
                If (cboTaxGroup.Text = "(none)" Or cboTaxGroup.Text = "") And Not m_bViewPaymentMode Then
                    dTaxAmount = 0
                Else
                    If Not m_bViewPaymentMode Then
                        If m_bIsWithHoldingTax Then
                            m_crTaxAmountWHT = crTaxAmount
                        Else
                            m_crTaxAmount = crTaxAmount
                        End If
                    End If
                    dTaxAmount = gPMFunctions.ToSafeDouble(m_crTaxAmount, 0) + gPMFunctions.ToSafeDouble(m_crTaxAmountWHT, 0)
                End If
                ' excess rows should always be negative
                If m_bIsExcess Then
                    ' unless the reverse excess payment checkbox has been checked
                    If chkReverseExcessPayment.CheckState = CheckState.Unchecked Then
                        If dPaymentAmount > 0 Then
                            dPaymentAmount = -dPaymentAmount
                        End If
                    Else
                        If dPaymentAmount < 0 Then
                            dPaymentAmount = -dPaymentAmount
                        End If
                    End If
                End If

                ' loss currency
                dLCPaymentAmount = dPaymentAmount * m_dPaymentToLossXRate
                dLCTaxAmount = dTaxAmount * m_dPaymentToLossXRate

                'txtPaymentAmount.Text = StringsHelper.Format(dPaymentAmount, "0.00")
                txtPaymentAmount.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, dPaymentAmount)
                'txtLCPaymentAmount.Text = StringsHelper.Format(dLCPaymentAmount, "0.00")
                txtLCPaymentAmount.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, dLCPaymentAmount)

                If dTaxAmount <> 0 Then
                    txtTaxAmount.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, dTaxAmount)
                End If

                'txtLCTaxAmount.Text = StringsHelper.Format(dLCTaxAmount, "0.00")
                txtLCTaxAmount.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, dLCTaxAmount)

                If Not m_bViewPaymentMode Then
                    If Not m_bClaimPaymentIsGross Then
                        'txtPaymentTotal.Text = StringsHelper.Format(dPaymentAmount + dTaxAmount, "0.00")
                        txtPaymentTotal.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, dPaymentAmount + dTaxAmount)
                        'txtLCPaymentTotal.Text = StringsHelper.Format(dLCPaymentAmount + dLCTaxAmount, "0.00")
                        txtLCPaymentTotal.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, dLCPaymentAmount + dLCTaxAmount)
                    Else
                        ' if its a gross payment strip tax to show net totals
                        'txtPaymentTotal.Text = StringsHelper.Format(dPaymentAmount - dTaxAmount, "0.00")
                        txtPaymentTotal.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, dPaymentAmount - dTaxAmount)
                        'txtLCPaymentTotal.Text = StringsHelper.Format(dLCPaymentAmount - dLCTaxAmount, "0.00")
                        txtLCPaymentTotal.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, dLCPaymentAmount - dLCTaxAmount)
                    End If
                Else
                    If Not m_bClaimPaymentIsGross Then
                        txtPaymentTotal.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, dPaymentAmount + dTaxAmount)
                        txtLCPaymentTotal.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, dLCPaymentAmount + dLCTaxAmount)
                    Else
                        txtPaymentTotal.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, dPaymentAmount - dTaxAmount)
                        txtLCPaymentTotal.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, dLCPaymentAmount - dLCTaxAmount)
                    End If
                End If

            Else

                ' payment currency
                txtPaymentAmount.Text = "0.00"
                txtTaxAmount.Text = "0.00"
                txtPaymentTotal.Text = "0.00"

                ' loss currency
                txtLCPaymentAmount.Text = "0.00"
                txtLCTaxAmount.Text = "0.00"
                txtLCPaymentTotal.Text = "0.00"

            End If
            result = PMEReturnCode.PMTrue

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ActionTransasctionCurrencySelection
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 12-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function ActionTransasctionCurrencySelection() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ActionTransasctionCurrencySelection"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim llBound, lUBound As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' if a transaction currency has been selected
            If cboCurrency.SelectedIndex <> -1 Then

                ' get the currency id
                m_lCurrencyId = VB6.GetItemData(cboCurrency, cboCurrency.SelectedIndex)

                ' get the transaction (payment) to loss currency exchange rate
                lReturn = GetPaymentLossExchangeRate()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetPaymentLossExchangeRate Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' get the currency code
                If Information.IsArray(m_vCurrencyArray) Then

                    ' get array boundaries
                    llBound = m_vCurrencyArray.GetLowerBound(1)
                    lUBound = m_vCurrencyArray.GetUpperBound(1)

                    ' find the selected item and get its code
                    For lCurrencyItem As Integer = llBound To lUBound
                        If CDbl(m_vCurrencyArray(kLookupItemId, lCurrencyItem)) = m_lCurrencyId Then
                            m_sCurrencyCode = CStr(m_vCurrencyArray(kLookupCode, lCurrencyItem))
                        End If
                    Next lCurrencyItem

                End If

            End If

            ' recalculate the loss currency amount with the new rate
            CalculateLossCurrencyAmounts()

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    ''' <summary>
    ''' ValidateFormData
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Created : MEvans : 12-08-2005 : 360 - Taxes on Claims</remarks>
    Public Function ValidateFormData() As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "ValidateFormData"
        Dim crTotalPaymentAmount As Decimal
        Dim sStr As String = ""

        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            If m_bShowPaymentDetailsHiddenMode Then
                Return nResult
            End If

            If Not m_bViewPaymentMode Then

                ' check form field mandatory controls
                nResult = m_oFormFields.CheckMandatoryControls()

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return nResult
                End If

                'Check for Tax Groups, if Null Tax Groups selected then shows error Message to select given tax Groups
                If nResult = gPMConstants.PMEReturnCode.PMTrue Then
                    'PN: 53820
                    If m_bTaxGroupMandatory AndAlso cboTaxGroup.Text.Trim() = "" AndAlso Not m_bIsExcess Then
                        MessageBox.Show("The Tax Group is a Mandatory field.", "Claim Payment Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        Return nResult
                    ElseIf cboTaxGroup.Text.Trim() = "" AndAlso Not m_bIsExcess Then
                        MessageBox.Show("Please select tax group or none.", "Claim Payment Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        Return nResult
                    End If
                End If

                If m_cTotalUnAuthorisedClaimPayment + gPMFunctions.ToSafeCurrency(txtLCPaymentTotal.Text, 0) > m_cMaxUnAuthorisedClaimPayment And m_cMaxUnAuthorisedClaimPayment > 0 Then
                    MessageBox.Show("This Payment exceeds the maximum unauthorised claim value of " & m_cMaxUnAuthorisedClaimPayment, "Claim Payment", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    Return nResult
                End If

                If m_bPaymentCannotExceedReserve Then
                    Dim dThisClaimPayment As Decimal

					If m_bClaimPaymentIsGross Then
						dThisClaimPayment = ToSafeCurrency(txtLCPaymentTotal.Text, 0)
					Else
						dThisClaimPayment = ToSafeCurrency(txtLCPaymentTotal.Text, 0) - ToSafeCurrency(txtLCTaxAmount.Text, 0)
					End If
                    
					If dThisClaimPayment > ToSafeCurrency(txtBalance.Text, 0) Then
                        MessageBox.Show("Payment amount entered cannot be accepted as it exceeds the reserve", "Claim Payment", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        txtPaymentAmount.Text = "0.00"
                        txtPaymentAmount.Focus()
                        cboTaxGroup.Focus()
                        txtPaymentAmount.Focus()
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        Return nResult
                    End If
                End If

                If nResult = gPMConstants.PMEReturnCode.PMTrue Then

                    'ATS
                    If (m_bTaxGroupMandatory AndAlso (cboTaxGroup.Text = "" OrElse cboTaxGroup.Text = "(none)")) AndAlso m_bIsExcess = False Then
                        MessageBox.Show("Tax Group is mandatory" & Strings.Chr(13) & Strings.Chr(10) &
                                        "Please select a valid Tax Group", "Claim Payment", MessageBoxButtons.OK)
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        Return nResult
                    End If

                    ' if this is not a reserve entry
                    If Not m_bIsExcess Then

                        ' if a negative reserve is not allowed
                        If Not m_bAllowNegativeReserve Then

                            ' if claim taxes are to be posted seperately then the reserve
                            ' will only be adjusted with the payment amount
                            If m_bIsPostClaimTaxes Then
                                crTotalPaymentAmount = gPMFunctions.ToSafeCurrency(txtLCPaymentAmount.Text, 0)
                            Else
                                ' if claim taxes are "NOT" to be posted seperately then the reserve
                                ' will be adjustment by the total of (payment amount + tax amount)
                                crTotalPaymentAmount = gPMFunctions.ToSafeCurrency(txtLCPaymentAmount.Text, 0) + gPMFunctions.ToSafeCurrency(txtLCTaxAmount.Text, 0)
                            End If

                            ' if the remaining reserve balance is less than the current payment
                            ' thus causing a negative reserve amount
                            If ((gPMFunctions.ToSafeCurrency(txtBalance.Text, 0) < crTotalPaymentAmount) And Not m_bClaimPaymentIsGross) Or ((gPMFunctions.ToSafeCurrency(txtBalance.Text, 0) < (crTotalPaymentAmount - CDbl(StringsHelper.Format(crTaxAmount, "0.00")))) And m_bClaimPaymentIsGross) Then

                                ' message to the user indicating that if they continue with this payment
                                ' the reserve will be adjusted in line with the payment
                                ' so that the reserve is not negative....
                                'If Not m_bRI2007Enabled Then

                                If Not m_bOpenClaimNoTrans Then
                                    If MessageBox.Show("The specified payment amount is greater than the remaining reserve amount." & Strings.Chr(13) & Strings.Chr(10) &
                                                   "The reserve amount will be adjusted and reduced to zero following this payment." &
                                                       " Do you wish to continue?", "Claim Payment Validation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then

                                        'Over-ride the user's input
                                        txtPaymentAmount.Text = "0.00"

                                        ' recalculate the tax amount
                                        ' based on the new payment amount
                                        RecalculateTaxAmount()

                                        ' reset the onscreen balances
                                        CalculateLossCurrencyAmounts()

                                        ' force user back to the interface
                                        nResult = gPMConstants.PMEReturnCode.PMFalse
                                        Return nResult
                                    End If
                                End If
                            End If
                        End If
                    End If
                    'ATS Message Validation
                    If m_bAdvancedTaxScriptOptionOn And ErrorMessage <> "" And cboTaxGroup.Text <> "(none)" Then
                        MessageBox.Show(ErrorMessage, "ATS Error Message", MessageBoxButtons.OK)
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        Return nResult
                    End If
                End If
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            nResult = gPMConstants.PMEReturnCode.PMError

        End Try
        Return nResult
    End Function

    Private Sub txtLCPaymentAmount_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLCPaymentAmount.Enter
        m_crGOTFOCUSLCPaymentAmount = CDec(txtLCPaymentAmount.Text)
    End Sub

    Private Sub txtLCPaymentAmount_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLCPaymentAmount.Leave
        If gPMFunctions.ToSafeCurrency(txtLCPaymentAmount.Text, 0) <> m_crGOTFOCUSLCPaymentAmount Then
            ActionRecalculate()
            m_crGOTFOCUSLCPaymentAmount = gPMFunctions.ToSafeCurrency(txtLCPaymentAmount.Text, 0)
        End If
    End Sub

    Private Sub txtPaymentAmount_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPaymentAmount.Leave
        If Not m_bViewPaymentMode Then
            If (txtPaymentAmount.Text.IndexOf(".") + 1) > 18 Then
                txtPaymentAmount.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, Strings.Left(txtPaymentAmount.Text, 18))
            End If
            ActionRecalculate()
            m_oFormFields.LostFocus(ctlControl:=txtPaymentAmount)
        End If
    End Sub

    Private Sub txtTaxAmount_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTaxAmount.Leave
        If Not m_bViewPaymentMode Then
            If m_sPreviousTaxAmount <> txtTaxAmount.Text Then
                m_bIsTaxOverridden = True
            End If
            ActionTaxAmountChange()
            m_oFormFields.LostFocus(ctlControl:=txtTaxAmount)
        End If
    End Sub

    Private Sub txtPaymentAmount_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPaymentAmount.Enter
        If Not m_bViewPaymentMode Then
            m_crGOTFOCUSLCPaymentAmount = CDec(txtLCPaymentAmount.Text)
            m_oFormFields.GotFocus(ctlControl:=txtPaymentAmount)
            ActionRecalculate()
        End If
    End Sub

    Private Sub txtTaxAmount_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTaxAmount.Enter
        If Not m_bViewPaymentMode Then
            m_oFormFields.GotFocus(ctlControl:=txtTaxAmount)
            CalculateLossCurrencyAmounts()
        End If
    End Sub

    Public Function SaveFormData() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SaveFormData"

        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            crTaxAmount = dTaxAmount ' PN 45184
            If CInt(txtTaxAmount.Text) <> 0 Then
                If Not m_bIsWithHoldingTax Then
                    m_crTaxAmount = CDec(txtTaxAmount.Text)
                    m_crTaxAmountWHT = 0
                Else
                    m_crTaxAmountWHT = CDec(txtTaxAmount.Text)
                    m_crTaxAmount = 0
                End If
            Else
                m_crTaxAmountWHT = 0
                m_crTaxAmount = 0
            End If

            If Not m_bClaimPaymentIsGross Then
                m_crThisPayment = CDec(txtPaymentAmount.Text)
                m_crThisPaymentLossCurrency = CDec(txtLCPaymentAmount.Text)
            Else
                m_crThisPayment = gPMFunctions.ToSafeDouble(txtPaymentAmount.Text, 0) - crTaxAmount
                'm_crThisPayment = txtPaymentTotal.Text
                m_crThisPaymentLossCurrency = gPMFunctions.ToSafeDouble(txtLCPaymentAmount.Text, 0) - crTaxAmount
            End If

            m_dPaymentToLossXRate = CDbl(txtCurrencyRate.Text)
            m_lCurrencyId = VB6.GetItemData(cboCurrency, cboCurrency.SelectedIndex)

            If m_bIsExcess Then
                m_lTaxGroupId = -1
            ElseIf cboTaxGroup.SelectedIndex <> "-1" Then
                m_lTaxGroupId = VB6.GetItemData(cboTaxGroup, cboTaxGroup.SelectedIndex)
            End If

            m_sCurrencyDescription = cboCurrency.Text
            m_sTaxGroupDescription = cboTaxGroup.Text

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    Public Function ActionTaxGroupSelection(Optional ByRef bForced As Boolean = False) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ActionTaxGroupSelection"

        Dim lTaxGroupId, llBound, lUBound As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_lTaxGroupId = 0
            If cboTaxGroup.Text = "(none)" Or cboTaxGroup.Text = "" Then
                txtTaxAmount.ReadOnly = True
                txtTaxAmount.BackColor = ColorTranslator.FromOle(-2147483633)
                txtTaxAmount.Text = "0.00"
                If Information.IsArray(m_vTaxBandRate) Then
                    Erase m_vTaxBandRate
                End If
                m_vTaxBandRate = Nothing
            Else

                txtTaxAmount.ReadOnly = False
                txtTaxAmount.BackColor = ColorTranslator.FromOle(-2147483643)

                ' get the selected claim payment to option
                If cboTaxGroup.SelectedIndex <> "-1" Then
                    lTaxGroupId = VB6.GetItemData(cboTaxGroup, cboTaxGroup.SelectedIndex)
                End If
                m_lTaxGroupId = lTaxGroupId

                ' get array boundaries
                llBound = m_vTaxGroupArray.GetLowerBound(1)
                lUBound = m_vTaxGroupArray.GetUpperBound(1)

                ' for each claim payment to option
                For lItem As Integer = llBound To lUBound
                    ' if the option matches the selected one
                    If CDbl(m_vTaxGroupArray(kLookupItemId, lItem)) = lTaxGroupId Then
                        ' get the is withholding tax indicator based on the selected tax group
                        ' Payee's non domiciled will alway shave predence over tax type's withholding status
                        If m_oTaxItem.PayeeDomiciled Then
                            m_bIsWithHoldingTax = gPMFunctions.ToSafeBoolean(m_vTaxGroupArray(kLookupTaxGroupIsWithHoldingTax, lItem), 0)
                        Else
                            m_bIsWithHoldingTax = Not m_oTaxItem.PayeeDomiciled
                        End If
                        m_sAdvancedTaxScript = CStr(m_vTaxGroupArray(kLookupTaxGroupAdvancedTaxScript, lItem)).Trim()

                        Exit For
                    End If
                Next

                ' if the selected tax group is different to the previously selected
                ' tax group then recalculate the tax amount
                If (cboTaxGroup.Text <> m_sPreviousTaxGroup And Not m_bLoading) Or bForced Then

                    ' recalculate the tax amount
                    lReturn = RecalculateTaxAmount()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "RecalculateTaxAmount Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' if there is only one tax band rate then
                    ' allow user to manually change the tax amount
                    If Information.IsArray(m_vTaxBandRate) Then
                        If m_vTaxBandRate.GetUpperBound(1) = 0 Then
                            txtTaxAmount.ReadOnly = False
                            txtTaxAmount.BackColor = ColorTranslator.FromOle(-2147483643)
                        Else
                            txtTaxAmount.ReadOnly = True
                            txtTaxAmount.BackColor = ColorTranslator.FromOle(-2147483633)
                        End If
                    Else
                        ' if no available tax band rate items to update
                        ' then lock out any user changes to the tax amount
                        txtTaxAmount.ReadOnly = True
                        txtTaxAmount.BackColor = ColorTranslator.FromOle(-2147483633)
                    End If
                End If
            End If

            If m_bAdvancedTaxScriptOptionOn OrElse cboTaxGroup.SelectedIndex < 1 OrElse ToSafeInteger(m_vTaxGroupArray(kLookupTaxGroupIsTaxAmountEditable, cboTaxGroup.SelectedIndex - 1)) = 0 Then
                txtTaxAmount.ReadOnly = True
                txtTaxAmount.BackColor = ColorTranslator.FromOle(-2147483633)
            Else
                txtTaxAmount.ReadOnly = False
                txtTaxAmount.BackColor = Color.White
            End If

            Return PMEReturnCode.PMTrue
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            Return PMEReturnCode.PMFalse

            ' If you want to rollback a transaction or something, do it here

        Finally

            lReturn = CalculateLossCurrencyAmounts()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CalculateLossCurrencyAmounts Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        End Try
        Return result
    End Function

    ''' <summary>
    ''' RecalculateTaxAmount
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>History:Created : MEvans : 22-08-2005 : 360 - Taxes on Claims</remarks>
    Public Function RecalculateTaxAmount() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RecalculateTaxAmount"

        Dim crCurrencyAmount, crTaxLossAmount, crTaxBaseAmount, crThisPayment As Decimal
        Dim lTmpClaimReceiptId As Integer
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            crThisPayment = gPMFunctions.ToSafeCurrency(txtPaymentAmount.Text, 0)

            ' if this is a gross claim payment
            If m_bClaimPaymentIsGross Then
                ' then fool the calculation process into doing it as a gross figure
                ' as it does for receipting rather than a net figure as it normally
                ' does for claim payments
                lTmpClaimReceiptId = 1
            Else
                lTmpClaimReceiptId = 0
            End If


            lReturn = m_oBusiness.CalculateTaxAmounts(v_lCompanyID:=m_lClaimSourceId, v_lTaxGroupId:=m_lTaxGroupId, v_sTranstype:=kTaxTransTypeClaimPayment, v_lCurrencyId:=m_lCurrencyId, v_lLossCurrencyId:=m_lLossCurrencyId, v_crAmount:=crThisPayment, r_crTaxCurrencyAmount:=crCurrencyAmount, r_crTaxLossAmount:=crTaxLossAmount, r_crTaxBaseAmount:=crTaxBaseAmount, v_lClaimPerilId:=m_lClaimPerilId, v_lClaimPaymentId:=1, v_lClaimReceiptId:=lTmpClaimReceiptId, v_lClaimPaymentItemId:=1, v_lClaimReceiptItemId:=0, v_lCalculateOnly:=1, r_vResults:=m_vTaxBandRate)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CalculateTaxAmounts", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' after we have calculated the tax amount
            ' based on the system defaults
            ' check for any advanced tax configuration
            If m_bAdvancedTaxScriptOptionOn Then
                ' if a an advanced tax script has been specified
                ' against the selected tax group
                If m_sAdvancedTaxScript <> "" And cboTaxGroup.SelectedIndex > 0 Then
                    ' run the rule
                    lReturn = CType(ExecuteAdvancedTaxScript(v_crThisPayment:=crThisPayment, nTaxGroupID:=m_lTaxGroupId), gPMConstants.PMEReturnCode)

                    If gPMFunctions.ToSafeString(ErrorMessage) <> "" Then
                        MessageBox.Show(ErrorMessage, "ATS Error Message", MessageBoxButtons.OK)
                        lReturn = gPMConstants.PMEReturnCode.PMTrue
                        Return result
                    End If

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        cmdOk.Enabled = False
                        gPMFunctions.RaiseError(kMethodName, "ExecuteAdvancedTaxScript Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    m_oPaymentItem.TaxBandRateArray = m_oTaxItem.TaxArray

                    m_oPaymentItem.PaymentAdjustment = m_oTaxItem.PaymentAdjustment
                    m_oPaymentItem.RecalculateTaxAmounts()
                    crCurrencyAmount = m_oPaymentItem.TaxAmount
                    m_cPaymentAdjustment = m_oPaymentItem.PaymentAdjustment

                End If
            Else
                m_cPaymentAdjustment = 0
            End If

            If m_cPaymentAdjustment <> 0 Then
                lblPaymentAdjustment.Text = "There will be a Payment Adjustment of " & StringsHelper.Format(m_cPaymentAdjustment, "#,##0.00") & "."
                lblPaymentAdjustment.Visible = True
            Else
                lblPaymentAdjustment.Visible = False
            End If

            result = gPMConstants.PMEReturnCode.PMTrue

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
            If Information.IsArray(m_vTaxBandRate) Then
                m_lNoOfTaxBandRateRows = m_vTaxBandRate.GetUpperBound(1) + 1
            Else
                m_lNoOfTaxBandRateRows = 0
            End If

            If gPMFunctions.ToSafeString(ErrorMessage) = "" Then
                txtTaxAmount.Text = StringsHelper.Format(crCurrencyAmount, "0.00")
                crTaxAmount = crCurrencyAmount
            End If

        End Try
        Return result
    End Function

    Public Function ActionRecalculate() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ActionRecalculate"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' dont calculate excess for tax rows as this isnt necessary
            If Not m_bIsExcess Then
                lReturn = RecalculateTaxAmount()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "RecalculateTaxAmount Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            lReturn = CalculateLossCurrencyAmounts()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CalculateLossCurrencyAmounts Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    Public Function ActionTaxAmountChange() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ActionTaxAmountChange"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_bIsTaxOverridden AndAlso IsArray(m_vTaxBandRate) Then
                Dim nTotalBands As Integer = m_vTaxBandRate.GetUpperBound(1)

                m_vTaxBandRate(kTaxArrayValue, 0) = CDec(txtTaxAmount.Text)
                m_vTaxBandRate(kTaxArrayPercentage, 0) = 0
                m_vTaxBandRate(kTaxArrayIsValue, 0) = 1
                m_vTaxBandRate(kTaxArrayIsManuallyChanged, 0) = kIsManuallyChangedUser
                'Set amount on rest of the bands to 0
                For iBand As Integer = 1 To nTotalBands
                    m_vTaxBandRate(kTaxArrayValue, iBand) = 0
                    m_vTaxBandRate(kTaxArrayPercentage, iBand) = 0
                    m_vTaxBandRate(kTaxArrayIsValue, iBand) = 1
                    m_vTaxBandRate(kTaxArrayIsManuallyChanged, iBand) = kIsManuallyChangedUser
                Next
            End If

            ' call calculate routines to recalculate based on new tax amount
            result = CalculateLossCurrencyAmounts()
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CalculateLossCurrencyAmounts Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            result = PMEReturnCode.PMFalse

        Finally

        End Try
        Return result
    End Function

    Public Function SetupFormFields() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupFormFields"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            result = m_oFormFields.AddNewFormField(ctlControl:=cboCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New ApplicationException("Add New form field cboCurrency failed")
            End If

            If Not m_bIsExcess Then
                If m_bTaxGroupMandatory = True Then
                    result = m_oFormFields.AddNewFormField(ctlControl:=cboTaxGroup, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
                Else
                    result = m_oFormFields.AddNewFormField(ctlControl:=cboTaxGroup, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                End If
            End If

            ' payment amount
            result = m_oFormFields.AddNewFormField(ctlControl:=txtPaymentAmount, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Add new form field txtPaymentAmount failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' tax amount
            result = m_oFormFields.AddNewFormField(ctlControl:=txtTaxAmount, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Add new form field txtTaxAmount failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lblCurrency.Left = cboCurrency.Left - VB6.TwipsToPixelsX(120) - lblCurrency.Width
            lblTaxGroup.Left = cboTaxGroup.Left - VB6.TwipsToPixelsX(120) - lblTaxGroup.Width
            lblPaymentAmount.Left = txtPaymentAmount.Left - VB6.TwipsToPixelsX(120) - lblPaymentAmount.Width

            If m_bClaimPaymentIsGross Then
                lblPaymentTotal.Text = kCaptionTotalGrossPayment
                lblLCPaymentTotal.Text = kCaptionTotalGrossPayment
            Else
                lblPaymentTotal.Text = kCaptionTotalNetPayment
                lblLCPaymentTotal.Text = kCaptionTotalNetPayment
            End If

            lblPaymentTotal.Left = txtPaymentTotal.Left - VB6.TwipsToPixelsX(120) - lblPaymentTotal.Width
            lblLCPaymentTotal.Left = txtLCPaymentTotal.Left - VB6.TwipsToPixelsX(120) - lblLCPaymentTotal.Width

            If m_bAdvancedTaxScriptOptionOn OrElse _
            cboTaxGroup.SelectedIndex < 1 OrElse _
            ToSafeInteger(m_vTaxGroupArray(kLookupTaxGroupIsTaxAmountEditable, cboTaxGroup.SelectedIndex - 1)) = 0 Then
                txtTaxAmount.ReadOnly = True
                txtTaxAmount.BackColor = ColorTranslator.FromOle(-2147483633)
            Else
                txtTaxAmount.ReadOnly = False
                txtTaxAmount.BackColor = Color.White
            End If

            result = PMEReturnCode.PMTrue

        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            result = PMEReturnCode.PMFalse

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ResizeForm
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 02-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function ResizeForm() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ResizeForm"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            fraTaxes.Height = VB6.TwipsToPixelsY(1050)

            'fraExcess.Top = fraTaxes.Top + fraTaxes.Height
            fraTotal.Top = fraTaxes.Top + fraTaxes.Height

            cmdOk.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(fraTotal.Top) + VB6.PixelsToTwipsY(fraTotal.Height) + 80)
            cmdCancel.Top = cmdOk.Top

            Me.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(cmdOk.Top) + VB6.PixelsToTwipsY(cmdOk.Height) + 550)

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetFormLayout
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function SetFormLayout() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetFormLayout"

        Dim lReturn, lLayout As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' determine layout to use
            ' factors to consider
            ' 1 - is advanced tax script option on
            ' 2 - is this an excess row

            If m_bIsExcess Then
                m_lLayout = 2
            ElseIf m_bAdvancedTaxScriptOptionOn Then
                m_lLayout = 1
            Else
                m_lLayout = 3
            End If

            ' layout specific

            Select Case m_lLayout
                Case kLayoutExcessRow
                    ' hide all references to tax
                    fraTaxes.Visible = False
                    fraTotal.Top = fraTaxes.Top

                    ' enable loss currency payment field for data entry
                    txtLCPaymentAmount.Enabled = True
                    txtLCPaymentAmount.BackColor = SystemColors.Window
                    txtLCPaymentAmount.TabStop = True
                    txtLCPaymentAmount.ReadOnly = False
                    lblReverseExcessPayment.Visible = True
                    chkReverseExcessPayment.Visible = True

                Case kLayoutGeneral, kLayoutAdvancedTaxOn
                    ' display normal tax frame without scripted tax amounts
                    fraTaxes.Height = VB6.TwipsToPixelsY(1050)
                    lblReverseExcessPayment.Visible = False
                    chkReverseExcessPayment.Visible = False

                    fraTotal.Top = fraTaxes.Top + fraTaxes.Height

            End Select

            ' defaults
            cmdOk.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(fraTotal.Top) + VB6.PixelsToTwipsY(fraTotal.Height) + 80)
            cmdCancel.Top = cmdOk.Top
            Me.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(cmdOk.Top) + VB6.PixelsToTwipsY(cmdOk.Height) + 550)

            If m_bAdvancedTaxScriptOptionOn Then
                txtTaxAmount.ReadOnly = True
                txtTaxAmount.BackColor = ColorTranslator.FromOle(-2147483633)
            End If

            ' set readonly mode
            If m_bViewPaymentMode Then

                cboCurrency.Visible = False
                cboTaxGroup.Visible = False

                txtCurrency.Visible = True
                txtTaxGroup.Visible = True

                If m_lTaxGroupId = 0 Then
                    txtTaxGroup.Text = "(none)"
                ElseIf m_lTaxGroupId = -1 Then
                    txtTaxGroup.Text = ""
                End If

                cmdCancel.Text = "&Close"
                cmdOk.Visible = False

                txtPaymentAmount.BackColor = SystemColors.Control
                txtPaymentAmount.ReadOnly = True
                txtPaymentAmount.TabStop = False

                txtTaxAmount.BackColor = SystemColors.Control
                txtTaxAmount.ReadOnly = True
                txtTaxAmount.TabStop = False

            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    ''' <summary>
    ''' ExecuteAdvancedTaxScript
    ''' </summary>
    ''' <param name="v_crThisPayment"></param>
    ''' <returns></returns>
    ''' <remarks> Created : MEvans : 25-08-2005 : 360 - Tax on Claims</remarks>
    Private Function ExecuteAdvancedTaxScript(ByRef v_crThisPayment As Decimal, ByVal nTaxGroupID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ExecuteAdvancedTaxScript"

        Dim vTaxParameters, vUpdatedTaxParameters As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the tax parameters array
            result = CType(LoadScriptingArray(v_crThisPayment:=v_crThisPayment, r_vTaxParametersArray:=vTaxParameters), gPMConstants.PMEReturnCode)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "LoadScriptingArray Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lParentHwnd = Me.Handle.ToInt32()
            KeepWindowOnTop(False)

            ' execute the advanced tax script
            result = m_oBusiness.ExecuteAdvancedTaxScript(v_lTaxScriptMode:=kTaxScriptModePayment, v_sTaxScriptName:=m_sAdvancedTaxScript, v_vTaxParameters:=vTaxParameters, r_vUpdatedTaxParameters:=vUpdatedTaxParameters, nTaxGroupID:=nTaxGroupID)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ExecuteAdvancedTaxScript Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            KeepWindowOnTop(True)

            ErrorMessage = gPMFunctions.ToSafeString(vUpdatedTaxParameters(kErrorMessage))
            If ErrorMessage <> "" Then
                Return result
            End If

            ' validate the data returned from the array is in a valid format
            result = CType(ValidateTaxArray(v_sTaxScriptName:=m_sAdvancedTaxScript, v_vOrigTaxParameters:=vTaxParameters, r_vUpdatedTaxParameters:=vUpdatedTaxParameters), gPMConstants.PMEReturnCode)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ValidateTaxArray Failed", gPMConstants.PMELogLevel.PMLogError)
            Else

                ' save any updates to the tax parameters array
                result = CType(SaveScriptingArray(v_vTaxParametersArray:=vUpdatedTaxParameters), gPMConstants.PMEReturnCode)
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SaveScriptingArray Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            result = gPMConstants.PMEReturnCode.PMTrue

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally

        End Try
        Return result

    End Function

    ''' <summary>
    ''' LoadScriptingArray
    ''' </summary>
    ''' <param name="v_crThisPayment"></param>
    ''' <param name="r_vTaxParametersArray"></param>
    ''' <returns></returns>
    ''' <remarks>Created : MEvans : 25-08-2005 : 360 - Taxes on Claims</remarks>
    Private Function LoadScriptingArray(ByVal v_crThisPayment As Decimal, ByRef r_vTaxParametersArray() As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "LoadScriptingArray"

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            With m_oTaxItem

                ' populate scripting params from interface
                m_oTaxItem.Amount = v_crThisPayment
                m_oTaxItem.TaxArray = m_vTaxBandRate
                m_oTaxItem.CurrencyCode = m_lCurrencyId

                ' get scripting array
                result = CType(m_oTaxItem.DataToArray(r_vTaxParametersArray), gPMConstants.PMEReturnCode)
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "cTaxParameters.DataToArray Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End With

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        End Try
        Return result
    End Function

    ''' <summary>
    ''' SaveScriptingArray
    ''' </summary>
    ''' <param name="v_vTaxParametersArray"></param>
    ''' <returns></returns>
    ''' <remarks>Created : MEvans : 25-08-2005 : 360 - Taxes on Claims</remarks>
    Private Function SaveScriptingArray(ByVal v_vTaxParametersArray() As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SaveScriptingArray"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' save array details back to data properties
            result = CType(m_oTaxItem.ArrayToData(v_vTaxParametersArray), gPMConstants.PMEReturnCode)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ArrayToData Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' save the tax array details
            m_vTaxBandRate = m_oTaxItem.TaxArray

            result = gPMConstants.PMEReturnCode.PMTrue

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        End Try
        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ValidateTaxArray
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 30-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function ValidateTaxArray(ByVal v_sTaxScriptName As String, ByVal v_vOrigTaxParameters() As Object, ByRef r_vUpdatedTaxParameters() As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ValidateTaxArray"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vOrigTaxArray, vUpdatedTaxArray As Object
        Dim oTaxParameters As cTaxParameters
        Dim llBound, lUBound As Integer
        Dim bIsWithHoldingTax As Boolean
        Dim sValidationErrorMessage, sValidateItemMessage, sTaxArrayItem As String

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the tax parameters tax arrays - as these are the only details
            ' that should have changed


            vOrigTaxArray = v_vOrigTaxParameters(kTaxArray)


            vUpdatedTaxArray = r_vUpdatedTaxParameters(kTaxArray)

            ' flag manually changed items

            lReturn = CType(m_oTaxItem.CompareTaxArrays(vOrigTaxArray, vUpdatedTaxArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CompareTaxArrays", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Information.IsArray(vUpdatedTaxArray) Then


                llBound = vUpdatedTaxArray.GetLowerBound(1)

                lUBound = vUpdatedTaxArray.GetUpperBound(1)

                ' for each tax item in the updated tax array
                For lTaxItem As Integer = llBound To lUBound

                    ' if it was manually changed by user or user defined script

                    If CDbl(vUpdatedTaxArray(kTaxArrayIsManuallyChanged, lTaxItem)) = kIsManuallyChangedScript Then

                        ' validate tax item

                        lReturn = CType(ValidateTaxItem(v_sTaxScriptName:=v_sTaxScriptName, r_vUpdatedTaxArray:=vUpdatedTaxArray, v_lTaxItem:=lTaxItem, r_sValidationMessage:=sValidationErrorMessage), gPMConstants.PMEReturnCode)

                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or sValidationErrorMessage <> "" Then

                            MessageBox.Show("A tax validation error has occurred." & Strings.Chr(13) & Strings.Chr(10) & _
                                            sValidationErrorMessage & _
                                            " The payment interface has been disabled, no further payments will be allowed. View the error log for further details.", "Tax Item Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) ' if the tax band array could not be validated


                            vUpdatedTaxArray = Nothing
                            Exit For
                        End If

                        ' validate all manually changed items
                        ' validation to include tax group exists, tax band exists, tax band is based on tax group
                        ' percentage and rate dont matter here as the script can set any value it likes

                    End If

                Next

            End If

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' log message to file
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:=sTaxArrayItem & sValidationErrorMessage & sValidateItemMessage, vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName)

        Finally
            ' pass back the (potentially updated (erased and currency conversions)) array
            r_vUpdatedTaxParameters(kTaxArray) = vUpdatedTaxArray

        End Try
        Return result
    End Function

    ''' <summary>
    ''' ValidateTaxItem:  Validates the return data in the tax array is of
    '               the correct type and that the value are in range.
    '
    '               Also sneakily updates the value to the payment
    '               currency as part of the validation checks is that
    '               they correctly convert to the payment currency
    ''' </summary>
    ''' <param name="v_sTaxScriptName"></param>
    ''' <param name="r_vUpdatedTaxArray"></param>
    ''' <param name="v_lTaxItem"></param>
    ''' <param name="r_sValidationMessage"></param>
    ''' <returns></returns>
    ''' <remarks>Created : MEvans : 30-08-2005 : 360 - Taxes on Claims</remarks>
    Private Function ValidateTaxItem(ByVal v_sTaxScriptName As String, ByRef r_vUpdatedTaxArray(,) As Object, ByVal v_lTaxItem As Integer, ByRef r_sValidationMessage As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ValidateTaxItem"

        Dim llBound, lUBound As Integer
        Dim bValid As Boolean
        Dim sTaxArrayItem, sValidationMessageTypeError As String
        Dim sValidationMessageValueError As New StringBuilder
        Dim sItemDesc, sItemCode As String
        Dim lTaxGroupId, lTaxBandId As Integer
        Dim sCurrencyCode As String = ""
        Dim dTaxPercentage As Double
        Dim crTaxValue As Decimal
        Dim lIsValue, lClassOfBusinessId As Integer

        Dim vTaxGroupId As Object
        Dim vTaxBandId As Object
        Dim vCurrencyCode As Object
        Dim vTaxPercentage As Object
        Dim vTaxValue As Object
        Dim vIsValue As Object
        Dim vClassOfBusinessId As Object
        Dim bErrorOccurred, bIsWithHoldingTax As Boolean
        Dim lCurrencyId As Integer
        Dim crPaymentCurrencyTaxAmount As Decimal
        Dim sTaxGroupDescription, sTaxBandDescription As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' get details from array

            vTaxGroupId = r_vUpdatedTaxArray(kTaxArrayTaxGroupId, v_lTaxItem)
            vTaxBandId = r_vUpdatedTaxArray(kTaxArrayTaxBandId, v_lTaxItem)
            vCurrencyCode = Convert.ToString(r_vUpdatedTaxArray(kTaxArrayTaxCurrencyCode, v_lTaxItem)).Trim()
            vTaxPercentage = r_vUpdatedTaxArray(kTaxArrayPercentage, v_lTaxItem)
            vTaxValue = r_vUpdatedTaxArray(kTaxArrayValue, v_lTaxItem)
            vClassOfBusinessId = r_vUpdatedTaxArray(kTaxArrayClassOfBusinessId, v_lTaxItem)
            vIsValue = r_vUpdatedTaxArray(kTaxArrayIsValue, v_lTaxItem)

            ' set up generic default message containing
            ' tax array item details in case of failure
            sTaxArrayItem = "Validation of Tax Item Generated / Amended by Tax Script " & v_sTaxScriptName & " Failed." & Strings.Chr(13) & Strings.Chr(10) & _
                            "The tax array item details are as follows : - " & Strings.Chr(13) & Strings.Chr(10) & _
                                "Tax Group Id:=" & Convert.ToString(vTaxGroupId) & Strings.Chr(13) & Strings.Chr(10) & _
                                "Tax Band Id:=" & Convert.ToString(vTaxBandId) & Strings.Chr(13) & Strings.Chr(10) & _
                                "Currency Code:=" & vCurrencyCode & Strings.Chr(13) & Strings.Chr(10) & _
                                "Tax Percenage:=" & Convert.ToString(vTaxPercentage) & Strings.Chr(13) & Strings.Chr(10) & _
                                "Value:=" & Convert.ToString(vTaxValue) & Strings.Chr(13) & Strings.Chr(10) & _
                                "Class Of Business Id:=" & Convert.ToString(vClassOfBusinessId) & Strings.Chr(13) & Strings.Chr(10) & _
                                "Is Value:=" & Convert.ToString(vIsValue) & Strings.Chr(13) & Strings.Chr(10) & _
                            "Failure Reason/s as follows :-" & Strings.Chr(13) & Strings.Chr(10)

            '*******************
            ' VALIDATE TYPES
            '*******************

            ' tax group id
            lTaxGroupId = ConvertToSafe(gPMConstants.PMEDataType.PMLong, vTaxGroupId, 0, bErrorOccurred)
            If bErrorOccurred Then
                sValidationMessageTypeError = sValidationMessageTypeError & "Invalid Tax Group Id Specified := " & CStr(vTaxGroupId) & "." & Strings.Chr(13) & Strings.Chr(10)
                bErrorOccurred = False
            End If

            ' tax band id
            lTaxBandId = ConvertToSafe(gPMConstants.PMEDataType.PMLong, vTaxBandId, 0, bErrorOccurred)
            If bErrorOccurred Then
                sValidationMessageTypeError = sValidationMessageTypeError & "Invalid Tax Band Id Specified := " & CStr(vTaxBandId) & "." & Strings.Chr(13) & Strings.Chr(10)
            End If

            sCurrencyCode = vCurrencyCode

            ' tax percentage
            dTaxPercentage = ConvertToSafe(gPMConstants.PMEDataType.PMDouble, vTaxPercentage, 0, bErrorOccurred)
            If bErrorOccurred Then
                sValidationMessageTypeError = sValidationMessageTypeError & "Invalid TaxPercentage specified :-" & CStr(vTaxPercentage) & "." & Strings.Chr(13) & Strings.Chr(10)
            End If

            ' tax value
            crTaxValue = ConvertToSafe(gPMConstants.PMEDataType.PMCurrency, vTaxValue, 0, bErrorOccurred)
            If bErrorOccurred Then
                sValidationMessageTypeError = sValidationMessageTypeError & "Invalid Tax Value specified :-" & CStr(vTaxValue) & "." & Strings.Chr(13) & Strings.Chr(10)
            End If

            ' class of business id
            lClassOfBusinessId = ConvertToSafe(gPMConstants.PMEDataType.PMLong, vClassOfBusinessId, 0, bErrorOccurred)

            ' is value
            lIsValue = ConvertToSafe(gPMConstants.PMEDataType.PMLong, vIsValue, 0, bErrorOccurred)
            If bErrorOccurred Then
                sValidationMessageTypeError = sValidationMessageTypeError & "Invalid IsValue specified :-" & CStr(vIsValue) & "." & Strings.Chr(13) & Strings.Chr(10)
            End If

            '*******************
            ' VALIDATE VALUES
            '*******************

            '**********************
            '* VALIDATE TAX GROUP *
            '**********************

            result = CType(GetLookupItemFromArray(v_vArray:=m_vTaxGroupLookup, r_sItemDesc:=sTaxGroupDescription, r_sItemCode:="", r_lItemId:=lTaxGroupId), gPMConstants.PMEReturnCode)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                If result <> gPMConstants.PMEReturnCode.PMNotFound Then
                    gPMFunctions.RaiseError(kMethodName, "GetLookupItemFromArray Tax Group Failed to find Tax Group Id:=" & lTaxGroupId, gPMConstants.PMELogLevel.PMLogError)
                Else
                    sValidationMessageValueError.Append("Invalid tax group specified. Tax Group Id " & lTaxGroupId & " could not be found." & Strings.Chr(13) & Strings.Chr(10))
                End If
            Else

                r_vUpdatedTaxArray(kTaxArrayTaxGroupDescription, v_lTaxItem) = sTaxGroupDescription
            End If

            '*********************
            '* VALIDATE TAX BAND *
            '*********************

            result = CType(GetLookupItemFromArray(v_vArray:=m_vTaxBandLookup, r_sItemDesc:=sTaxBandDescription, r_sItemCode:="", r_lItemId:=lTaxBandId), gPMConstants.PMEReturnCode)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                If result <> gPMConstants.PMEReturnCode.PMNotFound Then
                    gPMFunctions.RaiseError(kMethodName, "GetLookupItemFromArray Tax Band Failed to find TaxBandId:=" & lTaxBandId, gPMConstants.PMELogLevel.PMLogError)
                Else
                    sValidationMessageValueError.Append("Invalid tax band specified. Tax Band Id " & lTaxBandId & " could not be found." & Strings.Chr(13) & Strings.Chr(10))
                End If
            Else

                r_vUpdatedTaxArray(kTaxArrayTaxBandDescription, v_lTaxItem) = sTaxBandDescription
            End If

            '**********************************************
            '* VALIDATE TAX GROUP / TAX BAND RELATIONSHIP *
            '**********************************************

            ' for each item in the tax group tax band array
            If Information.IsArray(m_vTaxGroupTaxBandLookup) Then

                ' get array boundaries
                llBound = m_vTaxGroupTaxBandLookup.GetLowerBound(1)
                lUBound = m_vTaxGroupTaxBandLookup.GetUpperBound(1)

                ' for each tax group tax band item
                For lItem As Integer = llBound To lUBound

                    ' get the item details
                    bIsWithHoldingTax = gPMFunctions.ToSafeBoolean(m_vTaxGroupTaxBandLookup(kLookupTGTBDetailsIsWithholdingTax, lItem))

                    ' if the items matches the selected one
                    If lTaxGroupId = CDbl(m_vTaxGroupTaxBandLookup(kLookupTGTBDetailsTaxGroupId, lItem)) And lTaxBandId = CDbl(m_vTaxGroupTaxBandLookup(kLookupTGTBDetailsTaxBandId, lItem)) Then

                        ' if the is withholding tax indicator is set
                        ' and the payee party should is domiciled for tax
                        ' raise a validation error
                        If gPMFunctions.ToSafeString(m_oTaxItem.PaymentToCode.Trim(), "") = "INSURED" Then
                            If bIsWithHoldingTax = m_oTaxItem.InsuredDomiciled Then

                                sValidationMessageValueError.Append("The specified tax group's is withholding tax indicator cannot be " & _
                                                                    " reconciled with the insured party's domiciled for tax indicator. " & Strings.Chr(13) & Strings.Chr(10) & _
                                                                    "The insured party's domiciled for tax indicator = " & m_oTaxItem.InsuredDomiciled & _
                                                                    " and the tax group's is withholding tax indicator = " & CStr(bIsWithHoldingTax) & "." & Strings.Chr(13) & Strings.Chr(10))
                            Else

                                bValid = True
                                Exit For
                            End If
                        Else

                            bValid = True
                            Exit For
                        End If

                    End If

                Next

            End If

            If Not bValid Then
                sValidationMessageValueError.Append("Invalid tax group / tax band relationship. " & _
                                                    " No relationship could be found for tax group id = " & CStr(lTaxGroupId) &
                                                    " and tax band id = " & CStr(lTaxBandId) & "." & Strings.Chr(13) & Strings.Chr(10))
            End If

            '**************************
            '* VALIDATE CURRENCY CODE *
            '**************************
            sItemDesc = ""

            result = CType(GetLookupItemFromArray(v_vArray:=m_vCurrencyArray, r_sItemDesc:="", r_sItemCode:=sCurrencyCode, r_lItemId:=lCurrencyId), gPMConstants.PMEReturnCode)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                If result <> gPMConstants.PMEReturnCode.PMNotFound Then
                    gPMFunctions.RaiseError(kMethodName, "GetLookupItemFromArray Currency Failed to find Currency Code :=" & sCurrencyCode, gPMConstants.PMELogLevel.PMLogError)
                Else
                    sValidationMessageValueError.Append("Invalid Currency Code specified. Currency Code :=" & sCurrencyCode & " could not be found." & Strings.Chr(13) & Strings.Chr(10))
                End If
            Else

                If lCurrencyId <> m_lCurrencyId Then

                    ' Convert to Payment Currency

                    result = m_oCurrencyConvert.CurrencyToCurrencyConversion(v_lCurrencyIdFrom:=lCurrencyId, v_crCurrencyAmountFrom:=crTaxValue, v_lCompanyID:=m_lClaimSourceId, v_lCurrencyIdTo:=m_lCurrencyId, r_crCurrencyAmountTo:=crPaymentCurrencyTaxAmount)

                    If result = gPMConstants.PMEReturnCode.PMTrue Then
                        ' move the value to the payment currency

                        r_vUpdatedTaxArray(kTaxArrayTaxCurrencyCode, v_lTaxItem) = m_sCurrencyCode

                        r_vUpdatedTaxArray(kTaxArrayValue, v_lTaxItem) = crPaymentCurrencyTaxAmount
                    Else
                        sValidationMessageValueError.Append("Invalid Currency Code specified." & _
                                                            "Unable to convert specified tax amount from Currency Code:=" & sCurrencyCode & _
                                                             " to Payment Currency Code := " & m_sCurrencyCode)
                    End If
                Else
                    ' nothing to do as already in correct currency
                End If

            End If

            '*********************************
            '* VALIDATE CLASS OF BUSINESS ID *
            '*********************************
            ' if the class of business has been specified
            ' 0 will be treated as null
            If lClassOfBusinessId <> 0 Then

                sItemDesc = ""
                sItemCode = ""

                ' verify that it is a valid class of business id

                result = CType(GetLookupItemFromArray(v_vArray:=m_vClassOfBusinessLookup, r_sItemDesc:="", r_sItemCode:="", r_lItemId:=lClassOfBusinessId), gPMConstants.PMEReturnCode)
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    If result <> gPMConstants.PMEReturnCode.PMNotFound Then
                        gPMFunctions.RaiseError(kMethodName, "GetLookupItemFromArray Class Of Business Failed to find Class Of Business Id:=" & lClassOfBusinessId, gPMConstants.PMELogLevel.PMLogError)
                    Else
                        sValidationMessageValueError.Append("Invalid Class Of Business Id specified. Class Of Business Id:= " & lClassOfBusinessId & " could not be found." & Strings.Chr(13) & Strings.Chr(10))
                    End If
                End If

            End If
            '***********************
            '* VALIDATE PERCENTAGE *
            '***********************

            If dTaxPercentage < 0 Or dTaxPercentage > 100 Then
                sValidationMessageValueError.Append("Invalid Percentage Specified. Percentage :=" & dTaxPercentage & " is not valid. Percentage value must be between 0 and 100." & Strings.Chr(13) & Strings.Chr(10))
            End If

            ' display error message....
            r_sValidationMessage = sValidationMessageTypeError & sValidationMessageValueError.ToString()

            result = gPMConstants.PMEReturnCode.PMTrue

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally

        End Try
        Return result
    End Function


    Private Function ConvertToSafe(ByVal ConvertType As Integer, ByVal value As Object, Optional ByRef Default_Renamed As Decimal = 0, Optional ByRef ErrorOccurred As Boolean = False) As Object

        Dim result As Double = 0
        Try

            ErrorOccurred = False

            Select Case ConvertType
                Case gPMConstants.PMEDataType.PMCurrency
                    result = CDec(value)
                Case gPMConstants.PMEDataType.PMLong
                    result = CLng(value)
                Case gPMConstants.PMEDataType.PMInteger
                    result = CInt(value)
                Case gPMConstants.PMEDataType.PMBoolean
                    result = CBool(value)
                Case gPMConstants.PMEDataType.PMDouble
                    result = CDbl(value)
            End Select

            Return result

        Catch



            ErrorOccurred = True

            Return Default_Renamed
        End Try

    End Function


    ' ***************************************************************** '
    ' Name: GetLookupItemFromArray
    '
    ' Parameters: n/a
    '
    ' Description: Returns the code for a specifed item description
    '                  in a specified lookup table..
    '
    ' History:
    '           Created : MEvans : 06-06-2003 : 223
    ' ***************************************************************** '
    Private Function GetLookupItemFromArray(ByVal v_vArray(,) As Object, ByRef r_sItemDesc As String, ByRef r_sItemCode As String, ByRef r_lItemId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetLookupItemFromArray"

        Dim lRow As Integer
        Dim bFoundMatch As Boolean
        Dim sCode As String = ""
        Dim llBound, lUBound As Integer
        Dim v_vLookupItem As String = ""
        Dim lLookupItem As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Lookup value contants.
            Const ACValueTableName As Integer = 0
            Const ACValueID As Integer = 1
            Const ACValueStartPos As Integer = 2
            Const ACValueNumber As Integer = 3

            Const ACDetailKey As Integer = 0
            Const ACDetailDesc As Integer = 1
            Const ACDetailCode As Integer = 2

            If Information.IsArray(v_vArray) Then

                llBound = v_vArray.GetLowerBound(1)
                lUBound = v_vArray.GetUpperBound(1)

                ' set lookup properties
                If r_lItemId <> 0 Then
                    v_vLookupItem = CStr(r_lItemId)
                    lLookupItem = 0

                ElseIf r_sItemDesc <> "" Then
                    v_vLookupItem = r_sItemDesc
                    lLookupItem = 1

                ElseIf r_sItemCode <> "" Then
                    v_vLookupItem = r_sItemCode
                    lLookupItem = 2
                End If

                ' loop around the available items for the specified table
                For lCntr As Integer = llBound To lUBound

                    ' get the code for the specified lookup items key

                    If Convert.ToString(v_vArray(lLookupItem, lCntr)).Trim() = v_vLookupItem Then

                        ' return the requested code, id, description

                        r_sItemDesc = Convert.ToString(v_vArray(ACDetailDesc, lCntr)).Trim()

                        r_sItemCode = Convert.ToString(v_vArray(ACDetailCode, lCntr)).Trim()

                        r_lItemId = gPMFunctions.ToSafeInteger(Convert.ToString(v_vArray(ACDetailKey, lCntr)).Trim())

                        bFoundMatch = True
                        Exit For
                    End If

                Next lCntr

            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

            ' If you want to rollback a transaction or something, do it here

        Finally
            If Not bFoundMatch Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

        End Try
        Return result
    End Function

    Private Function KeepWindowOnTop(ByVal bKeepOnTop As Boolean) As Integer

        Try

            m_lReturn = CType(iPMFunc.SetWindowPlacement(m_lParentHwnd, bKeepOnTop), gPMConstants.PMEReturnCode)
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then Throw New Exception

        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="KeepWindowOnTop function failed", vApp:=ACApp, vClass:=ACClass, vMethod:="KeepWindowOnTop", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
        End Try
        Return gPMConstants.PMEReturnCode.PMTrue
    End Function
End Class