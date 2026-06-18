Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Windows.Forms
'Developer Guide no. 129
Imports SharedFiles
Partial Friend Class frmReceiptDetails
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmReceiptDetails
    '
    ' Date:
    '
    ' Description: Main interface form.
    '
    ' Edit History:
    ' ***************************************************************** '



    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmReceiptDetails"

    '********************************
    ' General Interface Property variables
    '********************************
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lPMAuthorityLevel As Integer
    Private m_bError As Integer

    'Modified as per Vb code 
    'Private m_oBusiness As bCLMPeril.Business
    Private m_oBusiness As Object
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_bInterfaceError As Boolean
    Private m_iUserId As Integer
    Private m_oFormFields As iPMFormControl.FormFields

    '********************************
    ' objects
    '********************************

    Private m_oCurrencyConvert As bACTCurrencyConvert.Form

    Private m_oPaymentMethod As bCLMPaymentMethod.Business

    '********************************
    ' specific interface property variables
    '********************************
    Private m_bViewReceiptMode As Boolean
    Private m_bAdvancedTaxScriptOptionOn As Boolean
    Private m_bAllowNegativeReserve As Boolean
    Private m_bLoading As Boolean

    '********************************
    ' claim details
    '********************************
    Private m_lClaimSourceId As Integer
    Private m_crThisReceiptLossCurrency As Decimal
    Private m_lClaimId As Integer
    Private m_sRisktype As String = ""
    Private m_sLossCurrency As String = ""
    Private m_lLossCurrencyId As Integer
    Private m_lClaimReceivableAccountId As Integer
    Private m_lClaimPerilId As Integer

    '********************************
    ' reserve details
    '********************************
    Private m_crTotalReserve As Decimal
    Private m_crRecoveredToDate As Decimal
    Private m_crBalance As Decimal
    'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
    Private m_vPayeeDetails(,) As Object
    'End - - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)

    '********************************
    ' payment details
    '********************************
    Private m_lPayeePartyId As Integer

    '********************************
    ' payment item details
    '********************************
    Private m_lRecoveryId As Integer
    Private m_sRecoveryType As String = ""
    Private m_lCurrencyId As Integer
    Private m_crThisReceipt As Decimal
    Private m_lTaxGroupId As Integer
    Private m_crTaxAmount As Decimal
    Private m_dCurrencyToBaseXRate As Double
    Private m_dtCurrencyToBaseDate As Date
    Private m_dAccountToBaseXRate As Double
    Private m_dtAccountToBaseDate As Date
    Private m_dSystemToBaseXRate As Double
    Private m_dtSystemToBaseDate As Date
    Private m_dReceiptToLossXRate As Double
    Private m_lExchangeOverrideReasonId As Integer
    Private m_sCurrencyDescription As String = ""
    Private m_sTaxGroupDescription As String = ""
    Private m_vTaxBandRate As Object
    Private m_sCurrencyCode As String = ""
    Private m_crScriptedTaxAmount As Decimal

    '********************************
    ' lookup details
    '********************************
    Private m_vTaxGroupArray(,) As Object
    Private m_vCurrencyArray(,) As Object
    Private m_vTaxGroupLookup(,) As Object
    Private m_vTaxBandLookup As Object
    Private m_vTaxGroupTaxBandLookup(,) As Object
    Private m_vClassOfBusinessLookup As Object


    Private m_oREceiptItem As cReceiptItem
    Private m_oTaxItem As cTaxParameters
    Private m_bIsWithHoldingTax As Boolean
    Private m_lClaimBaseCurrencyId As Integer
    Private m_sPreviousTaxGroup As String = ""
    Private m_lNoOfTaxBandRateRows As Integer
    Private m_sAdvancedTaxScript As String = ""
    Private m_lPaymentCurrencyFilter As Integer
    Private m_lViewReceiptMode As Integer
    Private m_bIsSalvageRecovery As Boolean
    Private m_bTaxGroupMandatory As Boolean
    Private m_vErrorMessage As Object
    Private m_bIsTaxOverridden As Boolean
    Private m_sPreviousTaxAmount As String

    ''Start(Saurabh Agrawal) Tech spec QBE004 Claim Recovery Reinsurance
    Private m_bReceiptExcludeTax As Boolean
    ''End(Saurabh Agrawal) Tech spec QBE004 Claim Recovery Reinsurance


    'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
    Public WriteOnly Property PayeeDetails() As Object
        Set(ByVal Value As Object)
            m_vPayeeDetails = Value
        End Set
    End Property
    ' End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)

    Public WriteOnly Property IsSalvageRecovery() As Boolean
        Set(ByVal Value As Boolean)
            m_bIsSalvageRecovery = Value
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

    Public WriteOnly Property ViewReceiptMode() As Boolean
        Set(ByVal Value As Boolean)
            m_bViewReceiptMode = Value
        End Set
    End Property

    Public ReadOnly Property ExchangeRateOverrideReasonId() As Integer
        Get
            Return m_lExchangeOverrideReasonId
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

    Public WriteOnly Property ScriptedTaxAmount() As Decimal
        Set(ByVal Value As Decimal)
            m_crScriptedTaxAmount = Value
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

    Public WriteOnly Property Business() As bCLMPeril.Business
        Set(ByVal Value As bCLMPeril.Business)
            m_oBusiness = Value
        End Set
    End Property

    Public WriteOnly Property ClaimBaseCurrencyId() As Integer
        Set(ByVal Value As Integer)
            m_lClaimBaseCurrencyId = Value
        End Set
    End Property

    Public WriteOnly Property ClaimReceivableAccountId() As Integer
        Set(ByVal Value As Integer)
            m_lClaimReceivableAccountId = Value
        End Set
    End Property

    Public WriteOnly Property ClaimID() As Integer
        Set(ByVal Value As Integer)
            m_lClaimId = Value
        End Set
    End Property

    Public WriteOnly Property UserId() As Integer
        Set(ByVal Value As Integer)
            m_iUserId = Value
        End Set
    End Property


    Public Property ReceiptItem() As cReceiptItem
        Get
            Return m_oREceiptItem
        End Get
        Set(ByVal Value As cReceiptItem)
            m_oREceiptItem = Value
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


    Public Property RecoveredToDate() As Decimal
        Get
            Return m_crRecoveredToDate
        End Get
        Set(ByVal Value As Decimal)
            m_crRecoveredToDate = Value
        End Set
    End Property

    Public WriteOnly Property Balance() As Decimal
        Set(ByVal Value As Decimal)
            m_crBalance = Value
        End Set
    End Property

    Public WriteOnly Property CurrencyConvert() As bACTCurrencyConvert.Form
        Set(ByVal Value As bACTCurrencyConvert.Form)
            m_oCurrencyConvert = Value
        End Set
    End Property

    Public WriteOnly Property PaymentMethod() As bCLMPaymentMethod.Business
        Set(ByVal Value As bCLMPaymentMethod.Business)
            m_oPaymentMethod = Value
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


    Public Property RecoveryType() As String
        Get
            Return m_sRecoveryType
        End Get
        Set(ByVal Value As String)
            m_sRecoveryType = Value
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


    Public Property RecoveryId() As Integer
        Get
            Return m_lRecoveryId
        End Get
        Set(ByVal Value As Integer)
            m_lRecoveryId = Value
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


    Public Property ThisReceipt() As Decimal
        Get
            Return m_crThisReceipt
        End Get
        Set(ByVal Value As Decimal)
            m_crThisReceipt = Value
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


    Public Property ReceiptToLossXRate() As Double
        Get
            Return m_dReceiptToLossXRate
        End Get
        Set(ByVal Value As Double)
            m_dReceiptToLossXRate = Value
        End Set
    End Property

    Public WriteOnly Property AdvancedTaxScriptOptionOn() As Boolean
        Set(ByVal Value As Boolean)
            m_bAdvancedTaxScriptOptionOn = Value
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
    '********************************
    ' Property for Error Message in ATS

    Public Property ErrorMessage() As Object
        Get
            Return m_vErrorMessage
        End Get
        Set(ByVal Value As Object)


            m_vErrorMessage = Value
        End Set
    End Property

    Private Sub cboCurrency_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCurrency.SelectedIndexChanged
        ActionTransasctionCurrencySelection()

    End Sub

    Private Sub cboTaxGroup_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTaxGroup.SelectedIndexChanged
        If cboCurrency.SelectedIndex <> -1 Then
            ActionTaxGroupSelection()
            ' save the selected tax group
            m_sPreviousTaxGroup = cboTaxGroup.Text
            m_sPreviousTaxAmount = txtTaxAmount.Text
        End If
    End Sub


    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
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

            'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
            g_oObjectManager = New bObjectManager.ObjectManager()
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:="bCLMRecovery.Business")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            'End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)

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
    Private Sub frmReceiptDetails_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
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
    Public Sub frmReceiptDetails_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

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

    Public Sub frmReceiptDetails_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Const kMethodName As String = "Form_Load"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lSubValue As Integer
        Dim sResult As String = "" ''Saurabh
        Try

            ' set up interface
            ' option 5056
            lReturn = CType(iPMFunc.GetSystemOption(v_iOptionNumber:=5056, r_sOptionValue:=sResult), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSystemOption Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If sResult.Trim() = "1" Then
                m_bTaxGroupMandatory = True
            End If

            ''Saurabh(Saurabh Agrawal)Tech Spec QBE004 Claim Recovery Reinsurance
            lReturn = CType(iPMFunc.RetrieveSingleSystemOption(v_iOptionNumber:=kSIROPTReceiptExcludeTax, r_sOptionValue:=sResult), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "RetrieveSingleSystemOption Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_bReceiptExcludeTax = (Not String.IsNullOrEmpty(sResult) And sResult = "1")
            ''End(Saurabh Agrawal)Tech Spec QBE004 Claim Recovery Reinsurance
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

            If Not m_bViewReceiptMode Then

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

            If m_bViewReceiptMode Then

                cboCurrency.Visible = False
                cboTaxGroup.Visible = False

                txtCurrency.Visible = True
                txtTaxGroup.Visible = True

                If m_lTaxGroupId = 0 Then
                    txtTaxGroup.Text = "(none)"
                End If

                cmdCancel.Text = "Close"
                cmdOk.Visible = False

                txtThisReceipt.BackColor = SystemColors.Control
                txtThisReceipt.ReadOnly = True

                txtTaxAmount.BackColor = SystemColors.Control
                txtTaxAmount.ReadOnly = True

                txtThisReceipt.TabStop = False
                txtTaxAmount.TabStop = False

            Else
                If m_lPaymentCurrencyFilter = 0 Then
                    txtCurrency.Visible = False
                    txtTaxGroup.Visible = False
                Else
                    txtCurrency.Visible = True
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
    ' Name: PopulateFormData
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 11-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function PopulateFormData() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateFormData"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_bLoading = True

            If Not m_bViewReceiptMode Then

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
                        SelectcboItem(cboCurrency, m_lCurrencyId)
                    End If
                End If

                SelectcboItem(cboTaxGroup, m_lTaxGroupId)
                If m_lTaxGroupId = 0 Then
                    cboTaxGroup.SelectedIndex = 0
                End If
                txtCurrency.Text = cboCurrency.Text
                txtTaxGroup.Text = cboTaxGroup.Text

            Else

                txtCurrency.Text = m_sCurrencyDescription
                txtTaxGroup.Text = m_sTaxGroupDescription

            End If

            ' claim
            txtRiskType.Text = m_sRisktype

            ' recovery
            txtRecoveryType.Text = m_sRecoveryType
            txtTotalReserve.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_crTotalReserve)
            txtRecoveredToDate.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_crRecoveredToDate)
            txtBalance.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_crBalance)

            ' gross receipt
            txtCurrencyRate.Text = CStr(m_dReceiptToLossXRate)
            txtThisReceipt.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_crThisReceipt)

            ' taxes
            ''Saurabh
            If Not m_bAdvancedTaxScriptOptionOn Then
                txtTaxAmount.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_crTaxAmount)
            Else
                txtTaxAmount.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_crScriptedTaxAmount)
            End If

            ' net amount
            ''Saurabh Agrawal

            'PN-61621 (Sushil Kumar)
            If m_bReceiptExcludeTax Then ''PN 3004
                txtNetReceipt.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_crThisReceipt + m_crTaxAmount)
            Else
                txtNetReceipt.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_crThisReceipt - m_crTaxAmount)
            End If

            ' loss currency
            txtLossCurrency.Text = m_sLossCurrency

            ' calculate loss currency amounts
            lReturn = CalculateLossCurrencyAmounts()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CalculateLossCurrencyAmounts Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            m_bLoading = False


            '        Return result
            '        Resume
            '        Return result
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
    Private Function PopulateCombo(ByRef r_oComboBox As ComboBox, ByVal v_vValuesArray(,) As Object, Optional ByVal v_sDefaultEntry As String = "") As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateCombo"

        Dim lReturn As Integer
        Dim sDescription As String = ""
        Dim lItemId As Integer
        Dim sCode As String = ""
        Dim llBound, lUBound, lComboItem As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' clear combo of all entries
            r_oComboBox.Items.Clear()

            ' if there is a default entry add it to the combo
            If v_sDefaultEntry <> "" Then
                lComboItem = 0
                r_oComboBox.Items.Insert(lComboItem, v_sDefaultEntry)
                VB6.SetItemData(r_oComboBox, lComboItem, 0)
                lComboItem = 1
            End If

            If Information.IsArray(v_vValuesArray) Then

                ' get the array boundaries
                llBound = v_vValuesArray.GetLowerBound(1)
                lUBound = v_vValuesArray.GetUpperBound(1)

                ' for each item in the array
                For lItem As Integer = llBound To lUBound

                    ' get item details

                    lItemId = CInt(v_vValuesArray(kLookupItemId, lItem))

                    sDescription = CStr(v_vValuesArray(kLookupDescription, lItem))

                    ' add item to combo
                    r_oComboBox.Items.Insert(lComboItem, sDescription)
                    VB6.SetItemData(r_oComboBox, lComboItem, lItemId)

                    lComboItem += 1

                Next

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
            If v_lSelectedId <> 0 Then

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

                lReturn = m_oCurrencyConvert.GetCurrencyRate(v_lCurrencyID:=m_lCurrencyId, v_lCompanyId:=m_lClaimSourceId, v_dtConversionDate:=DateTime.Today, r_vConversionRate:=dPaymentToBaseXRate)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to Rate Against Base for Payment Currency", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' Get loss rate to base

                lReturn = m_oCurrencyConvert.GetCurrencyRate(v_lCurrencyID:=m_lLossCurrencyId, v_lCompanyId:=m_lClaimSourceId, v_dtConversionDate:=DateTime.Today, r_vConversionRate:=dLossCurrencyToBaseXRate)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                        ' msgbox
                        MessageBox.Show("Currency Rate Not Found for ", Application.ProductName)
                    Else
                        gPMFunctions.RaiseError(kMethodName, "Failed to Rate Against Base for Loss Currency", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If

                'If dPaymentToBaseXRate = 0 Then
                '	gPMFunctions.RaiseError(kMethodName, "Payment To Base Exchange Rate set to 0", gPMConstants.PMELogLevel.PMLogError)
                'End If

                'If dLossCurrencyToBaseXRate = 0 Then
                '	gPMFunctions.RaiseError(kMethodName, "Loss Currency To Base Exchange Rate set to 0", gPMConstants.PMELogLevel.PMLogError)
                'End If
                If dPaymentToBaseXRate = 0 Or dLossCurrencyToBaseXRate = 0 Then
                    MsgBox("A currency exchange rate does not exist for the selected transaction currency." _
                            & vbCrLf & "Please either select an alternative currency or create a valid exchange rate for this currency", _
                            vbOKOnly, "Claim Receipt Validation")
                    m_dReceiptToLossXRate = 0
                Else
                    ' Calculate payment rate to loss
                    m_dReceiptToLossXRate = dPaymentToBaseXRate / dLossCurrencyToBaseXRate
                End If

            Else
                m_dReceiptToLossXRate = 1
            End If

            txtCurrencyRate.Text = CStr(m_dReceiptToLossXRate)


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ShowMultiCurrencyDialogue
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
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

            m_lReturn = m_oPaymentMethod.GetUserCurrencyAuthorities(v_iUserID:=m_iUserId, r_bChangeDate:=bChangeDate, r_bChangeRate:=bChangeRate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to get user currency authorities from business")
            End If

            ' either option set to true will do
            bCanChangeCurrency = (bChangeDate Or bChangeRate)

            'm_lClaimCurrencyID
            ' get the claim base currency details

            m_lReturn = m_oPaymentMethod.GetClaimBaseCurrencyDetails(v_lClaimId:=m_lClaimId, r_vResults:=vClaimBaseCurrencyDetails)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to get claim base branch currency from business")
            Else
                If Information.IsArray(vClaimBaseCurrencyDetails) Then

                    iBaseCurrencyID = CInt(vClaimBaseCurrencyDetails(0, 0))
                Else
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to get claim base branch currency from business")
                End If
            End If

            ' option 157
            m_lReturn = CType(iPMFunc.GetSystemOption(v_iOptionNumber:=157, r_sOptionValue:=sResult), gPMConstants.PMEReturnCode)

            oForm = New frmMultiCurrency()


            'Set up multi-currency screen
            oForm.TransactionCurrencyID = m_lCurrencyId
            oForm.TransactionAmount = m_crThisReceipt
            oForm.SourceID = m_lClaimSourceId
            oForm.PartyCnt = m_lPayeePartyId
            oForm.ClaimID = m_lClaimId
            oForm.ScreenMethod = kScreenMethodPayment
            oForm.LossCurrencyAmount = m_crThisReceiptLossCurrency
            oForm.LossCurrencyID = m_lLossCurrencyId
            oForm.ClaimReceivableAccountId = m_lClaimReceivableAccountId
            'Developer Guide no. 9
            lReturn = oForm.Initialise()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    ShowMultiCurrencyDialogue = gPMConstants.PMEReturnCode.PMNotFound
                    Exit Function
                End If
                gPMFunctions.RaiseError(kMethodName, "frmPaymentDetails.Initialise", gPMConstants.PMELogLevel.PMLogError)
            End If

            If (bCanChangeCurrency Or sResult.Trim() = "1") And Not (m_lCurrencyId = iBaseCurrencyID) Then
                'show the form
                oForm.ShowDialog()
                lStatus = oForm.Status
            Else
                ' claim currency is base currency, so silently save the rates
                lReturn = oForm.InterfaceToProperties()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
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
                m_lExchangeOverrideReasonId = oForm.OverrideId
                'Override the exchange rate
                If m_lExchangeOverrideReasonId > 0 Then
                    m_dReceiptToLossXRate = m_dCurrencyToBaseXRate
                End If
            End If


        Catch ex As Exception

            lStatus = gPMConstants.PMEReturnCode.PMError

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            result = lStatus

            oForm = Nothing

            '        Return result
            '        Resume
            '        Return result
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

            If Not m_bViewReceiptMode Then

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
                    If m_lPaymentCurrencyFilter = 0 Then
                        ' show the multi currency dialogue or at least call it to get the
                        ' values for the payment item
                        lReturn = ShowMultiCurrencyDialogue()
                        If lReturn <> gPMConstants.PMEReturnCode.PMOK Then
                            If lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                                Return result
                            ElseIf lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                                ' Do nothing
                            Else
                                gPMFunctions.RaiseError(kMethodName, "ShowMultiCurrencyDialogue Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                        End If
                        ' write the code to update party with salvage
                        ''''''''''''''''''''''''''''''''''''''''''''''
                    End If
                    'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
                    lReturn = CType(AttachParty(), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "AttachParty Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                    'End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)

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

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
    Public Function AttachParty() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AttachParty"

        Dim iCount As Integer

        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            If Information.IsArray(m_vPayeeDetails) Then

                iCount = m_vPayeeDetails.GetUpperBound(1)
                For iCounter As Integer = 0 To iCount
                    If CDbl(m_vPayeeDetails(kNewPartyRecoveryId, iCounter)) = m_lRecoveryId Then

                        Dim temp_m_oBusiness As Object
                        m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bCLMRecovery.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                        m_oBusiness = temp_m_oBusiness
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "GetInstance Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If


                        m_lReturn = m_oBusiness.UpdateRecoveryPartyLink(lRecoveryId:=m_lRecoveryId, lRecoveryPartyTypeId:=ReceiptItem.RecoveryPartyTypeId, lRecoveryPartyCnt:=ReceiptItem.RecoveryPartyCnt)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "UpdateRecoveryPartyLink Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                        Exit For
                    End If
                Next
            End If

            Return result

        Catch ex As Exception



            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)


            'm_oBusiness.Terminate
            Return result
        End Try
    End Function
    ''' <summary>
    ''' CalculateLossCurrencyAmounts
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Taxes on Claims</remarks>
    Public Function CalculateLossCurrencyAmounts() As Integer

        Const kMethodName As String = "CalculateLossCurrencyAmounts"
        Dim nResult As Integer = 0
        Dim dThisReceipt As Double
        Dim dTaxAmount As Double
        Dim dLCTaxAmount As Double
        Dim dLCThisReceipt As Double
        Dim dScriptingTaxAmount As Double
        Dim dLCScriptingTaxAmount As Double
        Dim dRecoveryBalance As Double

        Try



            nResult = gPMConstants.PMEReturnCode.PMTrue

            If CDec(txtCurrencyRate.Text) <> 0 Then

                ' receipt currency
                dRecoveryBalance = gPMFunctions.ToSafeDouble(txtBalance.Text, 0)
                dThisReceipt = gPMFunctions.ToSafeDouble(txtThisReceipt.Text, 0)
                dTaxAmount = gPMFunctions.ToSafeDouble(txtTaxAmount.Text, 0)
                If m_bAdvancedTaxScriptOptionOn Then
                    dScriptingTaxAmount = gPMFunctions.ToSafeDouble(txtTaxAmount.Text, 0)
                End If

                ' loss currency
                dLCThisReceipt = dThisReceipt * m_dReceiptToLossXRate
                dLCTaxAmount = dTaxAmount * m_dReceiptToLossXRate
                dLCScriptingTaxAmount = dScriptingTaxAmount * m_dReceiptToLossXRate

                ' receipt
                txtThisReceipt.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, dThisReceipt)
                txtLCThisReceipt.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, dLCThisReceipt)
                txtLCThisReceiptBalance.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, dRecoveryBalance - dLCThisReceipt)

                ' tax
                If m_bAdvancedTaxScriptOptionOn Then
                    txtTaxAmount.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, dScriptingTaxAmount)
                    txtLCTaxAmount.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, dLCScriptingTaxAmount)
                Else
                    txtTaxAmount.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, dTaxAmount)
                    txtLCTaxAmount.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, dLCTaxAmount)
                End If


                ' net
                If m_bReceiptExcludeTax Then
                    If m_bAdvancedTaxScriptOptionOn Then
                        txtNetReceipt.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, dThisReceipt + dScriptingTaxAmount)
                        txtLCNetReceipt.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, dLCThisReceipt + dLCScriptingTaxAmount)
                    Else
                        txtNetReceipt.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, dThisReceipt + dTaxAmount)
                        txtLCNetReceipt.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, dLCThisReceipt + dLCTaxAmount)
                    End If
                Else
                    If m_bAdvancedTaxScriptOptionOn Then
                        txtNetReceipt.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, dThisReceipt - dScriptingTaxAmount)
                        txtLCNetReceipt.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, dLCThisReceipt - dLCScriptingTaxAmount)
                    Else
                        txtNetReceipt.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, dThisReceipt - dTaxAmount)
                        txtLCNetReceipt.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, dLCThisReceipt - dLCTaxAmount)
                    End If
                End If

            Else

                ' reset all fields to zero as no currency rate available
                ' receipt
                txtThisReceipt.Text = "0.00"
                txtLCThisReceipt.Text = "0.00"
                txtLCThisReceiptBalance.Text = "0.00"

                ' taxes
                txtTaxAmount.Text = "0.00"
                txtLCTaxAmount.Text = "0.00"

                ' net
                txtNetReceipt.Text = "0.00"
                txtLCNetReceipt.Text = "0.00"

            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
            ' If you want to rollback a transaction or something, do it here
            Return nResult
        End Try
        Return nResult
    End Function

    ' ***************************************************************** '
    ' Name: ActionTransasctionCurrencySelection
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' Created : MEvans : 12-08-2005 : 360 - Taxes on Claims
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

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ValidateFormData
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 12-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function ValidateFormData() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ValidateFormData"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lValid As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            lValid = gPMConstants.PMEReturnCode.PMTrue

            If Not m_bViewReceiptMode Then

                ' check form field mandatory controls
                lReturn = m_oFormFields.CheckMandatoryControls()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    lValid = gPMConstants.PMEReturnCode.PMFalse
                End If

                '        If lValid = PMTrue Then
                '            If ToSafeCurrency(txtThisReceipt.Text) < 0 Then
                '                MsgBox "The specified receipt amount must be greater than zero.", vbInformation, "Claim Receipt Validation"
                '                txtThisReceipt.SetFocus
                '                lValid = PMFalse
                '            End If
                '        End If

                'ATS Message Validation
                If m_bAdvancedTaxScriptOptionOn And gPMFunctions.ToSafeString(m_oTaxItem.ErrorMessage, "") <> "" And cboTaxGroup.Text <> "(none)" Then
                    MessageBox.Show(gPMFunctions.ToSafeString(m_oTaxItem.ErrorMessage, ""), "ATS Error Message", MessageBoxButtons.OK)
                    lValid = gPMConstants.PMEReturnCode.PMFalse
                End If

                If m_bTaxGroupMandatory And (cboTaxGroup.Text = "(none)") Then

                    MessageBox.Show("Please select a valid Tax Group", "Claim Payment", MessageBoxButtons.OK)

                    lValid = gPMConstants.PMEReturnCode.PMFalse
                End If
                If lValid = gPMConstants.PMEReturnCode.PMTrue Then
                    If ToSafeCurrency(txtCurrencyRate.Text) = 0 Then
                        MsgBox("A currency exchange rate does not exist for the selected transaction currency." _
                            & vbCrLf & "Please either select an alternative currency or create a valid exchange rate for this currency", _
                            vbOKOnly, "Claim Receipt Validation")
                        cboCurrency.Focus()
                        lValid = gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If


        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return lValid
    End Function

    Private Sub txtThisReceipt_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtThisReceipt.Leave
        If Not m_bViewReceiptMode Then
            m_oFormFields.LostFocus(ctlControl:=txtThisReceipt)
            ActionRecalculate()
        End If
    End Sub

    Private Sub txtTaxAmount_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTaxAmount.Leave
        If Not m_bViewReceiptMode Then
            m_oFormFields.LostFocus(ctlControl:=txtTaxAmount)
            If m_sPreviousTaxAmount <> txtTaxAmount.Text Then
                m_bIsTaxOverridden = True
            End If
            ActionTaxAmountChange()
        End If
    End Sub

    Private Sub txtThisReceipt_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtThisReceipt.Enter
        If Not m_bViewReceiptMode Then
            m_oFormFields.GotFocus(ctlControl:=txtThisReceipt)
            ActionRecalculate()
        End If
    End Sub

    Private Sub txtTaxAmount_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTaxAmount.Enter
        If Not m_bViewReceiptMode Then
            m_oFormFields.GotFocus(ctlControl:=txtTaxAmount)
            CalculateLossCurrencyAmounts()
        End If
    End Sub

    ' ***************************************************************** '
    ' Name: SaveFormData
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function SaveFormData() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SaveFormData"

        Dim lReturn As Integer
        Dim sResult As String = ""
        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            m_crTaxAmount = gPMFunctions.ToSafeCurrency(txtTaxAmount.Text, 0)
            m_crThisReceipt = gPMFunctions.ToSafeCurrency(txtThisReceipt.Text, 0)
            m_crThisReceiptLossCurrency = gPMFunctions.ToSafeCurrency(txtLCThisReceipt.Text, 0)
            m_dReceiptToLossXRate = gPMFunctions.ToSafeCurrency(txtCurrencyRate.Text, 0)
            m_lCurrencyId = gPMFunctions.ToSafeLong(VB6.GetItemData(cboCurrency, cboCurrency.SelectedIndex), 0)
            m_lTaxGroupId = gPMFunctions.ToSafeLong(VB6.GetItemData(cboTaxGroup, cboTaxGroup.SelectedIndex), 0)

            m_sCurrencyDescription = cboCurrency.Text
            m_sTaxGroupDescription = cboTaxGroup.Text


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function ActionTaxGroupSelection() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ActionTaxGroupSelection"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lTaxGroupId, llBound, lUBound As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If cboTaxGroup.Text = "(none)" Then

                txtTaxAmount.ReadOnly = True
                txtTaxAmount.BackColor = ColorTranslator.FromOle(-2147483633)
                txtTaxAmount.Text = "0.00"

            Else
                txtTaxAmount.ReadOnly = m_bAdvancedTaxScriptOptionOn
                txtTaxAmount.BackColor = ColorTranslator.FromOle(-2147483643)

            End If

            ' get the selected claim payment to option
            lTaxGroupId = VB6.GetItemData(cboTaxGroup, cboTaxGroup.SelectedIndex)
            m_lTaxGroupId = lTaxGroupId

            If Information.IsArray(m_vTaxGroupArray) Then
                ' get array boundaries
                llBound = m_vTaxGroupArray.GetLowerBound(1)
                lUBound = m_vTaxGroupArray.GetUpperBound(1)

                ' for each claim payment to option
                For lItem As Integer = llBound To lUBound

                    ' if the option matches the selected one
                    If gPMFunctions.ToSafeDouble(m_vTaxGroupArray(kLookupItemId, lItem)) = lTaxGroupId Then

                        ' get the is withholding tax indicator based on the selected tax group
                        m_bIsWithHoldingTax = gPMFunctions.ToSafeBoolean(m_vTaxGroupArray(kLookupTaxGroupIsWithHoldingTax, lItem), 0)
                        m_sAdvancedTaxScript = CStr(m_vTaxGroupArray(kLookupTaxGroupAdvancedTaxScript, lItem)).Trim()

                        Exit For
                    End If

                Next
            End If


            ' if the selected tax group is different to the previously selected
            ' tax group then recalculate the tax amount
            If cboTaxGroup.Text <> m_sPreviousTaxGroup And Not m_bLoading Then

                ' recalculate the tax amount
                lReturn = RecalculateTaxAmount()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "RecalculateTaxAmount Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' if there is only one tax band rate then
                ' allow user to manually change the tax amount
                If Information.IsArray(m_vTaxBandRate) Then

                    If m_vTaxBandRate.GetUpperBound(1) = 0 Then
                        txtTaxAmount.ReadOnly = m_bAdvancedTaxScriptOptionOn
                    Else
                        txtTaxAmount.ReadOnly = True
                    End If
                Else
                    ' if no available tax band rate items to update
                    ' then lock out any user changes to the tax amount
                    txtTaxAmount.ReadOnly = True
                End If

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            lReturn = CalculateLossCurrencyAmounts()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CalculateLossCurrencyAmounts Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ''' <summary>
    ''' RecalculateTaxAmount
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Created : MEvans : 22-08-2005 : 360 - Taxes on Claims</remarks>
     Public Function RecalculateTaxAmount() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RecalculateTaxAmount"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim crCurrencyAmount, crTaxLossAmount, crTaxBaseAmount, crThisReceipt As Decimal
        Dim sTransType As String = ""

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            crThisReceipt = gPMFunctions.ToSafeCurrency(txtThisReceipt.Text, 0)

            ' set the appropriate tax transtype when calling through to calculate
            ' the taxes
            If m_bIsSalvageRecovery Then
                sTransType = kTaxTransTypeClaimSalvageReceipt
            Else
                sTransType = kTaxTransTypeClaimThirdPartyRecoveryReceipt
            End If

            If Not IsArray(m_vTaxBandRate) Then
                m_vTaxBandRate = Nothing
            End If

            lReturn = m_oBusiness.CalculateTaxAmounts(v_lCompanyId:=m_lClaimSourceId, v_lTaxGroupId:=m_lTaxGroupId, v_sTranstype:=sTransType, v_lCurrencyId:=m_lCurrencyId, v_lLossCurrencyId:=m_lLossCurrencyId, v_crAmount:=crThisReceipt, r_crTaxCurrencyAmount:=crCurrencyAmount, r_crTaxLossAmount:=crTaxLossAmount, r_crTaxBaseAmount:=crTaxBaseAmount, v_lClaimPerilId:=m_lClaimPerilId, v_lClaimPaymentId:=0, v_lClaimReceiptId:=1, v_lClaimPaymentItemId:=0, v_lClaimReceiptItemId:=1, v_lCalculateOnly:=1, r_vResults:=m_vTaxBandRate)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CalculateTaxAmounts", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' after we have calculated the tax amount
            ' based on the system defaults
            ' check for any advanced tax configuration
            If m_bAdvancedTaxScriptOptionOn Then
                ' if a an advanced tax script has been specified
                ' against the selected tax group
                If m_sAdvancedTaxScript <> "" Then
                    ' run the rule
                    lReturn = CType(ExecuteAdvancedTaxScript(v_crThisReceipt:=crThisReceipt, nTaxGroupID:=m_lTaxGroupId), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "ExecuteAdvancedTaxScript Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    m_oREceiptItem.TaxBandRateArray = m_oTaxItem.TaxArray

                    If gPMFunctions.ToSafeString(m_oTaxItem.ErrorMessage, "") = "" And cboTaxGroup.Text <> "(none)" Then
                        txtScriptedTaxAmount.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(gPMFunctions.ToSafeCurrency(m_oREceiptItem.ScriptedTaxAmount, 0)))
                    End If
                    txtTaxAmount.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(gPMFunctions.ToSafeCurrency(m_oREceiptItem.ScriptedTaxAmount, 0))) ''Saurabh

                End If
            End If


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
            ''Saurabh

            If Not m_bAdvancedTaxScriptOptionOn Then
                txtTaxAmount.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crCurrencyAmount)
            ElseIf m_bAdvancedTaxScriptOptionOn And gPMFunctions.ToSafeString(m_oTaxItem.ErrorMessage, "") <> "" And cboTaxGroup.Text <> "(none)" Then
                txtTaxAmount.Text = "0.00"
            Else
                txtTaxAmount.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_oREceiptItem.ScriptedTaxAmount)
            End If


            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ActionRecalculate
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function ActionRecalculate() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ActionRecalculate"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = RecalculateTaxAmount()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "RecalculateTaxAmount Failed", gPMConstants.PMELogLevel.PMLogError)
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

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function ActionTaxAmountChange() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ActionTaxAmountChange"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            If StringsHelper.Format(gPMFunctions.ToSafeString(gPMFunctions.ToSafeCurrency(txtTaxAmount.Text, 0.0#), "0.0"), "0.0") <> StringsHelper.Format(txtTaxAmount.Text, "0.0") Then
                txtTaxAmount.Text = "0.00"
                MessageBox.Show("Invalid Tax Amount", "Recovery Detail", MessageBoxButtons.OK, MessageBoxIcon.Information)
                m_lReturn = CalculateLossCurrencyAmounts()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Call to CalculateLossCurrency Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                Return result
            End If
            ' if there is a tax band rate item
            If Information.IsArray(m_vTaxBandRate) Then
                ' if there is only one item

                If m_vTaxBandRate.GetUpperBound(1) = 0 Then
                    ' allow user to manually update the tax amount
                    If m_vTaxBandRate(kTaxArrayValue, 0) <> gPMFunctions.ToSafeCurrency(txtTaxAmount.Text, 0.0#) Then
                        ' reset tax band rate details to show it has been manually adjusted

                        m_vTaxBandRate(kTaxArrayValue, 0) = gPMFunctions.ToSafeCurrency(txtTaxAmount.Text, 0)

                        m_vTaxBandRate(kTaxArrayPercentage, 0) = 0

                        m_vTaxBandRate(kTaxArrayIsValue, 0) = 1

                        m_vTaxBandRate(kTaxArrayIsManuallyChanged, 0) = kIsManuallyChangedUser
                    End If
                End If
            End If

            ' call calculate routines to recalculate based on new tax amount
            lReturn = CalculateLossCurrencyAmounts()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CalculateLossCurrencyAmounts Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function SetupFormFields() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupFormFields"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            ' currency
            lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)


            ' tax group
            lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboTaxGroup, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)


            ' this receipt
            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtThisReceipt, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Add new form field txtThisReceipt failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' tax amount
            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtTaxAmount, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Add new form field txtTaxAmount failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lblCurrency.Left = cboCurrency.Left - VB6.TwipsToPixelsX(120) - lblCurrency.Width
            lblTaxGroup.Left = cboTaxGroup.Left - VB6.TwipsToPixelsX(120) - lblTaxGroup.Width
            lblThisReceipt.Left = txtThisReceipt.Left - VB6.TwipsToPixelsX(120) - lblThisReceipt.Width

            If m_bAdvancedTaxScriptOptionOn OrElse _
            cboTaxGroup.SelectedIndex < 1 OrElse _
            ToSafeInteger(m_vTaxGroupArray(kLookupTaxGroupIsTaxAmountEditable, cboTaxGroup.SelectedIndex - 1)) = 0 Then
                txtTaxAmount.ReadOnly = True
                txtTaxAmount.BackColor = ColorTranslator.FromOle(-2147483633)
            Else
                txtTaxAmount.ReadOnly = False
                txtTaxAmount.BackColor = Color.White
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
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

            If m_bAdvancedTaxScriptOptionOn Then
                fraTaxes.Height = VB6.TwipsToPixelsY(1380)
                lblScriptedTaxAmount.Visible = True
                lblLCScriptedTaxAmount.Visible = True
                txtScriptedTaxAmount.Visible = True
                txtLCScriptedTaxAmount.Visible = True
            Else
                fraTaxes.Height = VB6.TwipsToPixelsY(1050)
                lblScriptedTaxAmount.Visible = False
                lblLCScriptedTaxAmount.Visible = False
                txtScriptedTaxAmount.Visible = False
                txtLCScriptedTaxAmount.Visible = False
            End If

            fraTotal.Top = fraTaxes.Top + fraTaxes.Height

            cmdOk.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(fraTotal.Top) + VB6.PixelsToTwipsY(fraTotal.Height) + 80)
            cmdCancel.Top = cmdOk.Top

            Me.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(cmdOk.Top) + VB6.PixelsToTwipsY(cmdOk.Height) + 550)


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ExecuteAdvancedTaxScript
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-08-2005 : 360 - Tax on Claims
    ' ***************************************************************** '
    Private Overloads Function ExecuteAdvancedTaxScript(ByRef v_crThisReceipt As Decimal) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ExecuteAdvancedTaxScript"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vTaxParameters, vUpdatedTaxParameters As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the tax parameters array

            lReturn = CType(LoadScriptingArray(v_crThisReceipt:=v_crThisReceipt, r_vTaxParametersArray:=vTaxParameters), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "LoadScriptingArray Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' execute the advanced tax script

            lReturn = m_oBusiness.ExecuteAdvancedTaxScript(v_lTaxScriptMode:=kTaxScriptModeReceipt, v_sTaxScriptName:=m_sAdvancedTaxScript, v_vTaxParameters:=vTaxParameters, r_vUpdatedTaxParameters:=vUpdatedTaxParameters)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ExecuteAdvancedTaxScript Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' validate the data returned from the array is in a valid format


            lReturn = CType(ValidateTaxArray(v_sTaxScriptName:=m_sAdvancedTaxScript, v_vOrigTaxParameters:=vTaxParameters, r_vUpdatedTaxParameters:=vUpdatedTaxParameters), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                cmdOk.Enabled = False
                gPMFunctions.RaiseError(kMethodName, "ValidateTaxArray Failed", gPMConstants.PMELogLevel.PMLogError)
            Else

                ' save any updates to the tax parameters array

                lReturn = CType(SaveScriptingArray(v_vTaxParametersArray:=vUpdatedTaxParameters), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SaveScriptingArray Failed", gPMConstants.PMELogLevel.PMLogError)
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

    Private Overloads Function ExecuteAdvancedTaxScript(ByRef v_crThisReceipt As Decimal, ByVal nTaxGroupID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ExecuteAdvancedTaxScript"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vTaxParameters, vUpdatedTaxParameters As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the tax parameters array

            lReturn = CType(LoadScriptingArray(v_crThisReceipt:=v_crThisReceipt, r_vTaxParametersArray:=vTaxParameters), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "LoadScriptingArray Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' execute the advanced tax script

            lReturn = m_oBusiness.ExecuteAdvancedTaxScript(v_lTaxScriptMode:=kTaxScriptModeReceipt, v_sTaxScriptName:=m_sAdvancedTaxScript, v_vTaxParameters:=vTaxParameters, r_vUpdatedTaxParameters:=vUpdatedTaxParameters, nTaxGroupID:=nTaxGroupID)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ExecuteAdvancedTaxScript Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' validate the data returned from the array is in a valid format


            lReturn = CType(ValidateTaxArray(v_sTaxScriptName:=m_sAdvancedTaxScript, v_vOrigTaxParameters:=vTaxParameters, r_vUpdatedTaxParameters:=vUpdatedTaxParameters), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                cmdOk.Enabled = False
                gPMFunctions.RaiseError(kMethodName, "ValidateTaxArray Failed", gPMConstants.PMELogLevel.PMLogError)
            Else

                ' save any updates to the tax parameters array

                lReturn = CType(SaveScriptingArray(v_vTaxParametersArray:=vUpdatedTaxParameters), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SaveScriptingArray Failed", gPMConstants.PMELogLevel.PMLogError)
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
    ' Name: LoadScriptingArray
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function LoadScriptingArray(ByVal v_crThisReceipt As Decimal, ByRef r_vTaxParametersArray() As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "LoadScriptingArray"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oTaxParameters As cTaxParameters

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oTaxItem

                ' populate scripting params from interface

                m_oTaxItem.Amount = v_crThisReceipt

                m_oTaxItem.CurrencyCode = m_sCurrencyCode


                m_oTaxItem.TaxArray = m_vTaxBandRate

                ' get scripting array
                lReturn = CType(m_oTaxItem.DataToArray(r_vTaxParametersArray), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "cTaxParameters.DataToArray Failed", gPMConstants.PMELogLevel.PMLogError)
                End If


                m_oTaxItem.ErrorMessage = m_vErrorMessage

            End With



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SaveScriptingArray
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function SaveScriptingArray(ByVal v_vTaxParametersArray() As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SaveScriptingArray"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oTaxParameters As cTaxParameters

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' save array details back to data properties
            lReturn = CType(m_oTaxItem.ArrayToData(v_vTaxParametersArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ArrayToData Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' save the tax array details


            m_vTaxBandRate = m_oTaxItem.TaxArray



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




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
        Dim vOrigTaxArray, vUpdatedTaxArray(,) As Object
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

                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                            gPMFunctions.RaiseError(kMethodName, "A tax validation error has occurred." & Strings.Chr(13) & Strings.Chr(10) & _
                                                    sValidationErrorMessage & _
                                                    ".", gPMConstants.PMELogLevel.PMLogInfo)
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

            ' indicate to user an error has occurred and what is going to happen
            MessageBox.Show("A tax validation error has occurred." & Strings.Chr(13) & Strings.Chr(10) & _
                            sValidationErrorMessage & _
                            " View the error log for further details.", "Tax Item Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) ' if the tax band array could not be validated

            Erase vUpdatedTaxArray

        Finally

            ' pass back the (potentially updated (erased and currency conversions)) array


            r_vUpdatedTaxParameters(kTaxArray) = vUpdatedTaxArray




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ValidateTaxItem
    '
    ' Parameters: n/a
    '
    ' Description: Validates the return data in the tax array is of
    '               the correct type and that the value are in range.
    '
    '               Also sneakily updates the value to the payment
    '               currency as part of the validation checks is that
    '               they correctly convert to the payment currency
    '
    ' History:
    '           Created : MEvans : 30-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function ValidateTaxItem(ByVal v_sTaxScriptName As String, ByRef r_vUpdatedTaxArray(,) As Object, ByVal v_lTaxItem As Integer, ByRef r_sValidationMessage As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ValidateTaxItem"

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim llBound, lUBound As Integer
        Dim bValid As Boolean
        Dim sTaxArrayItem, sValidationMessageTypeError, sValidationMessageValueError As String

        Dim lItemId As Integer
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
            lReturn = CType(GetLookupItemFromArray(v_vArray:=m_vTaxGroupLookup, r_sItemDesc:=sTaxGroupDescription, r_sItemCode:="", r_lItemId:=lTaxGroupId), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    gPMFunctions.RaiseError(kMethodName, "GetLookupItemFromArray Tax Group Failed to find TaxGRoupId:=" & lTaxGroupId, gPMConstants.PMELogLevel.PMLogError)
                Else
                    sValidationMessageValueError = sValidationMessageValueError & "Invalid tax group specified. Tax Group Id " & CStr(lTaxGroupId) & " could not be found." & Strings.Chr(13) & Strings.Chr(10)
                End If
            Else

                r_vUpdatedTaxArray(kTaxArrayTaxGroupDescription, v_lTaxItem) = sTaxGroupDescription
            End If

            '*********************
            '* VALIDATE TAX BAND *
            '*********************

            lReturn = CType(GetLookupItemFromArray(v_vArray:=m_vTaxBandLookup, r_sItemDesc:=sTaxBandDescription, r_sItemCode:="", r_lItemId:=lTaxBandId), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    gPMFunctions.RaiseError(kMethodName, "GetLookupItemFromArray Tax Band Failed to find TaxBandId:=" & lTaxBandId, gPMConstants.PMELogLevel.PMLogError)
                Else
                    sValidationMessageValueError = sValidationMessageValueError & "Invalid tax band specified. Tax Band Id " & CStr(lTaxBandId) & " could not be found." & Strings.Chr(13) & Strings.Chr(10)
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
                    lTaxGroupId = CInt(m_vTaxGroupTaxBandLookup(kLookupTGTBDetailsTaxGroupId, lItem))
                    lTaxBandId = CInt(m_vTaxGroupTaxBandLookup(kLookupTGTBDetailsTaxBandId, lItem))
                    bIsWithHoldingTax = CBool(m_vTaxGroupTaxBandLookup(kLookupTGTBDetailsIsWithholdingTax, lItem))

                    ' if the items matches the selected one
                    If lTaxGroupId = lTaxGroupId And lTaxBandId = lTaxBandId Then

                        bValid = True
                        Exit For
                    End If

                Next

            End If

            If Not bValid Then
                sValidationMessageValueError = sValidationMessageValueError & "Invalid tax group / tax band relationship. " & _
                                               " No relationship could be found for tax group id = " & CStr(lTaxGroupId) & _
                                               " and tax band id = " & CStr(lTaxBandId) & "." & Strings.Chr(13) & Strings.Chr(10)
            End If

            '**************************
            '* VALIDATE CURRENCY CODE *
            '**************************
            sItemDesc = ""

            lReturn = CType(GetLookupItemFromArray(v_vArray:=m_vCurrencyArray, r_sItemDesc:="", r_sItemCode:=sCurrencyCode, r_lItemId:=lCurrencyId), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    gPMFunctions.RaiseError(kMethodName, "GetLookupItemFromArray Currency Failed to find Currency Code :=" & sCurrencyCode, gPMConstants.PMELogLevel.PMLogError)
                Else
                    sValidationMessageValueError = sValidationMessageValueError & "Invalid Currency Code specified. Currency Code :=" & sCurrencyCode & " could not be found." & Strings.Chr(13) & Strings.Chr(10)
                End If
            Else

                If lCurrencyId <> m_lCurrencyId Then

                    ' Convert to Payment Currency

                    lReturn = m_oCurrencyConvert.CurrencyToCurrencyConversion(v_lCurrencyIdFrom:=lCurrencyId, v_crCurrencyAmountFrom:=crTaxValue, v_lCompanyId:=m_lClaimSourceId, v_lCurrencyIdTo:=m_lCurrencyId, r_crCurrencyAmountTo:=crPaymentCurrencyTaxAmount)

                    If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        ' move the value to the payment currency

                        r_vUpdatedTaxArray(kTaxArrayTaxCurrencyCode, v_lTaxItem) = m_sCurrencyCode

                        r_vUpdatedTaxArray(kTaxArrayValue, v_lTaxItem) = crPaymentCurrencyTaxAmount
                    Else
                        sValidationMessageValueError = sValidationMessageValueError & "Invalid Currency Code specified." & _
                                                       "Unable to convert specified tax amount from Currency Code:=" & sCurrencyCode & _
                                                       " to Payment Currency Code := " & m_sCurrencyCode
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

                lReturn = CType(GetLookupItemFromArray(v_vArray:=m_vClassOfBusinessLookup, r_sItemDesc:="", r_sItemCode:="", r_lItemId:=lClassOfBusinessId), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                        gPMFunctions.RaiseError(kMethodName, "GetLookupItemFromArray Class Of Business Failed to find Class Of Business Id:=" & lClassOfBusinessId, gPMConstants.PMELogLevel.PMLogError)
                    Else
                        sValidationMessageValueError = sValidationMessageValueError & "Invalid Class Of Business Id specified. Class Of Business Id:= " & CStr(lClassOfBusinessId) & " could not be found." & Strings.Chr(13) & Strings.Chr(10)
                    End If
                End If

            End If
            '***********************
            '* VALIDATE PERCENTAGE *
            '***********************

            If dTaxPercentage < 0 Or dTaxPercentage > 100 Then
                sValidationMessageValueError = sValidationMessageValueError & "Invalid Percentage Specified. Percentage :=" & CStr(dTaxPercentage) & " is not valid. Percentage value must be between 0 and 100." & Strings.Chr(13) & Strings.Chr(10)
            End If

            ' display error message....
            r_sValidationMessage = sValidationMessageTypeError & sValidationMessageValueError

            If r_sValidationMessage <> "" Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If



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
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            If Not bFoundMatch Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If




        End Try
        Return result
    End Function

End Class