Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'developer guide no 129. 
Imports SharedFiles

Partial Friend Class frmDetailsUW
    Inherits System.Windows.Forms.Form

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmDetailsUW"
    Private Const vbFormControlMenu As Integer = 1
    Private Const PMTaxArray_TaxTypeID As Integer = 0
    Private Const PMTaxArray_TaxTypeDesc As Integer = 1
    Private Const PMTaxArray_TaxBandID As Integer = 2
    Private Const PMTaxArray_TaxBandDesc As Integer = 3
    Private Const PMTaxArray_TaxIsValue As Integer = 4
    Private Const PMTaxArray_TaxRate As Integer = 5
    Private Const PMTaxArray_TaxTypeCode As Integer = 6

    '=================
    'Public Properties
    '=================
    'Payment Line level
    Public RiskType As String = ""
    Public CurrentReserve As Decimal
    Public PaidToDate As Decimal
    Public ThisPaymentLossCurrency As Decimal
    Public ThisPayment As Decimal
    Public LossCurrencyName As String = ""
    Public PaymentCurrencyName As String = ""
    Public TaxTypeCode As String = ""
    Public TaxBand As String = ""
    Public TaxValue As Decimal
    Public TotalInclTax As Decimal
    Public CurrencyRatePayToLoss As Double
    Public PaymentCurrencyID As Integer
    Public ReserveAdjustment As Decimal 'If Payment > Reserve and allow Neg reserves is switched off

    'Claim Level
    Public RevisionHasBeenAmended As Boolean
    Public Task As gPMConstants.PMEComponentAction
    Public TransactionType As String = ""
    Public AllowNegativeReserve As gPMConstants.PMEReturnCode
    Public obCLMPeril As Object
    Public ScreenMode As MainModule.UWDetailScreenMode

    Private m_oFormFields As iPMFormControl.FormFields
    Private m_lReturn As Integer
    Private m_vTaxArray(,) As Object 'Tracy Richards - 29/06/2003 - VAT on Claims
    Private m_lClaimCompanyID As Integer

    Private m_oCurrencyConvert As Object
    Private m_oInsuranceFile As Object
    Private m_oUserAuthorities As Object
    Private m_lClaimId As Integer

    Public WriteOnly Property ClaimId() As Integer
        Set(ByVal Value As Integer)
            m_lClaimId = Value
        End Set
    End Property

    Public ReadOnly Property TaxValueLossCurrency() As Decimal
        Get
            Return Math.Round(TaxValue * CurrencyRatePayToLoss, 2)
        End Get
    End Property


    '==============
    'Private Events
    '==============

    Private isInitializingComponent As Boolean
    Private Sub cboTaxBand_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTaxBand.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        CalculateTax()
    End Sub
    Private Sub cboTaxBand_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTaxBand.SelectedIndexChanged
        CalculateTax()
    End Sub

    Private Sub cboTaxType_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTaxType.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        ' Populate tax bands available for selected tax type
        If cboTaxType.SelectedIndex > -1 Then
            FillTaxBandCombo(VB6.GetItemData(cboTaxType, cboTaxType.SelectedIndex))
        Else
            cboTaxBand.Items.Clear()
        End If
        CalculateTax()
    End Sub

    Private Sub cboTaxType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTaxType.SelectedIndexChanged
        ' Populate tax bands available for selected tax type
        If cboTaxType.SelectedIndex > -1 Then
            FillTaxBandCombo(VB6.GetItemData(cboTaxType, cboTaxType.SelectedIndex))
        Else
            cboTaxBand.Items.Clear()
        End If
        CalculateTax()
    End Sub

    Private Sub cboTaxType_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles cboTaxType.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        ' Stop typing
        KeyAscii = 0
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    '******************************************************************************
    ' Name:         cmdCancel_Click
    ' Description:  Unload the form when the Cancel Button is clicked
    '******************************************************************************
    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        RevisionHasBeenAmended = False
        Me.Hide()
    End Sub

    '******************************************************************************
    ' Name:         cmdOk_Click
    ' Description:  Change the data in the List View when the Ok button is clicked
    '               and unload the form
    '******************************************************************************
    Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click

        Try

            'If the user was able to update the fields - then save to vars
            If cboPaymentCurrency.Visible Then
                If cboPaymentCurrency.SelectedIndex >= 0 Then
                    'Save the ID while we're at it. The parent needs this
                    PaymentCurrencyID = VB6.GetItemData(cboPaymentCurrency, cboPaymentCurrency.SelectedIndex)
                    PaymentCurrencyName = cboPaymentCurrency.Text
                Else
                    MessageBox.Show("You must choose a Payment Currency.", "No Currency Set", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If

            If cboTaxType.Visible Then
                TaxTypeCode = cboTaxType.Text
            End If
            If cboTaxBand.Visible Then
                TaxBand = cboTaxBand.Text
            End If
            If txtTaxValue.Visible Then
                TaxValue = CDec(txtTaxValue.Text)
            End If

            ThisPaymentLossCurrency = CDec(txtThisPaymentLoss.Text)
            ThisPayment = CDec(txtThisPayment.Text)
            'READ-ONLY controls - never need updating
            'LossCurrencyName = txtLossCurrencyName.Text
            'RiskType = txtRiskType.Text
            'CurrentReserve = txtCurrentReserve.Text
            'PaidToDate = txtPaidToDate.Text

            iPMFunc.ForceLostFocus(cmdOk)

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            Me.Hide()

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process OK button " & "of frmDetailsUW", vApp:=ACApp, vClass:=ACClass, vMethod:="cmd_OK Click of frmDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try

    End Sub

    Private Sub frmDetailsUW_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            ' ensure that these value are always set.
            txtThisPaymentLoss.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, ReplaceNullWithDefault(ThisPaymentLossCurrency, 0))
            txtThisPayment.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, ReplaceNullWithDefault(ThisPayment, 0))
            txtTaxValue.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, ReplaceNullWithDefault(TaxValue, 0))
            txtTotalInclTax.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, ReplaceNullWithDefault(CDec(txtThisPayment.Text) + CDec(txtTaxValue.Text), 0))

        End If
    End Sub

    '******************************************************************************
    ' Name:         Form_load
    ' Description:  Loads the form with the minimum set of information required for
    '               the details screen depending on the modes that are set for the
    '               screen
    ' History:      04/09/2001 Tinny - renumbering column, add revision amount to
    '               payment
    '******************************************************************************

    Private Sub frmDetailsUW_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Const c_lStart As Integer = 360
        Const c_lStep As Integer = 480
        Const c_lEnd As Integer = 400

        Dim lCurrentTop As Integer

        Try

            txtRiskType.Enabled = False

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID

            'Get the claim company id
            m_lReturn = GetClaimCompany()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function GetClaimCompany failed.")
            End If

            'Hide all of the controls
            HideControls()

            'Show/Hide controls
            lblCurrentReserve.Visible = True
            txtCurrentReserve.Visible = True
            txtCurrentReserve.Enabled = False

            lblPaidToDate.Visible = True
            txtPaidToDate.Visible = True
            txtPaidToDate.Enabled = False

            lblThisPaymentLoss.Visible = True
            txtThisPaymentLoss.Visible = True

            lblThisPayment.Visible = True
            txtThisPayment.Visible = True

            lblLossCurrency.Visible = True
            txtLossCurrencyName.Visible = True

            lblCurrency.Visible = True
            cboPaymentCurrency.Visible = True

            '    lblCurrencyRate.Visible = False
            '    txtCurrencyRate.Visible = False

            lblTaxType.Visible = True
            cboTaxType.Visible = True

            lblTaxBand.Visible = True
            cboTaxBand.Visible = True

            lblTaxValue.Visible = True
            txtTaxValue.Visible = True

            lblTotalInclTax.Visible = True
            txtTotalInclTax.Visible = True

            'Position shown controls
            lCurrentTop = c_lStart
            lblRiskType.Top = VB6.TwipsToPixelsY(lCurrentTop)
            txtRiskType.Top = VB6.TwipsToPixelsY(lCurrentTop)

            lCurrentTop += c_lStep
            lblCurrentReserve.Top = VB6.TwipsToPixelsY(lCurrentTop)
            txtCurrentReserve.Top = VB6.TwipsToPixelsY(lCurrentTop)

            lCurrentTop += c_lStep
            lblPaidToDate.Top = VB6.TwipsToPixelsY(lCurrentTop)
            txtPaidToDate.Top = VB6.TwipsToPixelsY(lCurrentTop)

            lCurrentTop += c_lStep
            lblThisPaymentLoss.Top = VB6.TwipsToPixelsY(lCurrentTop)
            txtThisPaymentLoss.Top = VB6.TwipsToPixelsY(lCurrentTop)

            lCurrentTop += c_lStep
            lblThisPayment.Top = VB6.TwipsToPixelsY(lCurrentTop)
            txtThisPayment.Top = VB6.TwipsToPixelsY(lCurrentTop)

            lCurrentTop += c_lStep
            lblLossCurrency.Top = VB6.TwipsToPixelsY(lCurrentTop)
            txtLossCurrencyName.Top = VB6.TwipsToPixelsY(lCurrentTop)

            lCurrentTop += c_lStep
            lblCurrency.Top = VB6.TwipsToPixelsY(lCurrentTop)
            cboPaymentCurrency.Top = VB6.TwipsToPixelsY(lCurrentTop)

            lCurrentTop += c_lStep
            lblTaxType.Top = VB6.TwipsToPixelsY(lCurrentTop)
            cboTaxType.Top = VB6.TwipsToPixelsY(lCurrentTop)

            lCurrentTop += c_lStep
            lblTaxBand.Top = VB6.TwipsToPixelsY(lCurrentTop)
            cboTaxBand.Top = VB6.TwipsToPixelsY(lCurrentTop)

            lCurrentTop += c_lStep
            lblTaxValue.Top = VB6.TwipsToPixelsY(lCurrentTop)
            txtTaxValue.Top = VB6.TwipsToPixelsY(lCurrentTop)

            lCurrentTop += c_lStep
            lblTotalInclTax.Top = VB6.TwipsToPixelsY(lCurrentTop)
            txtTotalInclTax.Top = VB6.TwipsToPixelsY(lCurrentTop)

            ' Pass control and required settings to FormControl
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtThisPayment, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            iPMFunc.SelectText(txtThisPayment)

            'Resize the form to fit all shown controls
            lCurrentTop += c_lEnd
            Me.Height = VB6.TwipsToPixelsY(lCurrentTop + 1725)
            cmdCancel.Top = VB6.TwipsToPixelsY(lCurrentTop + 900)
            cmdOk.Top = VB6.TwipsToPixelsY(lCurrentTop + 900)
            fraReserveDetails.Height = VB6.TwipsToPixelsY(lCurrentTop + 150)
            SSTab1.Height = VB6.TwipsToPixelsY(lCurrentTop + 675)

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try

    End Sub


    Private Sub frmDetailsUW_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)



        If UnloadMode = vbFormControlMenu Then

            ' mimic user pressing cancel button
            RevisionHasBeenAmended = False

            ' don't kill the form here, just hide it
            ' so control returns to the calling form
            Cancel = True
            Me.Hide()
        End If

        eventArgs.Cancel = Cancel <> 0
    End Sub

    Private Sub frmDetailsUW_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
        If Not (m_oCurrencyConvert Is Nothing) Then

            m_oCurrencyConvert.Dispose()
            m_oCurrencyConvert = Nothing
        End If

        If Not (m_oInsuranceFile Is Nothing) Then

            m_oInsuranceFile.Dispose()
            m_oInsuranceFile = Nothing
        End If

        If Not (m_oUserAuthorities Is Nothing) Then

            m_oUserAuthorities.Dispose()
            m_oUserAuthorities = Nothing
        End If
    End Sub


    'Private Sub lblThisRevision_Click()
    'Inform parent that this form has gathered useful info
    'RevisionHasBeenAmended = True
    'End Sub

    Private Sub txtThisPayment_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtThisPayment.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        'Inform parent that this form has gathered useful info
        RevisionHasBeenAmended = True

        If gPMFunctions.ToSafeCurrency(txtThisPayment.Text) <> 0 And CurrencyRatePayToLoss <> 0 Then
            ThisPaymentLossCurrency = Math.Round(ThisPayment * CurrencyRatePayToLoss, 2)
        End If

        txtThisPaymentLoss.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, ThisPaymentLossCurrency)

    End Sub

    '******************************************************************************
    ' Name:         txtThisPayment_LostFocus
    ' Description:  Change the display format for the value in the text box
    '******************************************************************************
    Private Sub txtThisPayment_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtThisPayment.Leave

        'get the currency value for efficiency
        Dim cNewPaymentAmount As Decimal = gPMFunctions.ToSafeCurrency(txtThisPayment.Text)
        'See if the payment has changed
        If ThisPayment <> cNewPaymentAmount Then
            'TR - First check that this new payment amount is not greater than
            'the reserve if the "allow negative reserves" hidden option is switched off
            If cNewPaymentAmount > CurrentReserve And AllowNegativeReserve <> gPMConstants.PMEReturnCode.PMTrue Then
                If MessageBox.Show("Payment is more than current reserve." & Strings.Chr(13) & Strings.Chr(10) & _
                                   "Adjustment will be made against reserve", "Generic Peril", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
                    'Allow the new payment amount....
                    ThisPayment = cNewPaymentAmount
                    ThisPaymentLossCurrency = Math.Round(ThisPayment * CurrencyRatePayToLoss, 2)
                    '...but Adjust the reserve!
                    ReserveAdjustment = ThisPaymentLossCurrency * -1
                Else
                    'Over-ride the user's input
                    ThisPayment = 0
                    txtThisPayment.Focus()
                End If
            Else
                'Just allow the payment as it has been entered
                ThisPayment = cNewPaymentAmount
                ThisPaymentLossCurrency = Math.Round(ThisPayment * CurrencyRatePayToLoss, 2)
            End If

            CalculateTax()
        End If

        txtThisPayment.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, ThisPayment)
        txtThisPaymentLoss.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, ThisPaymentLossCurrency)

    End Sub

    'Private Sub txtCurrencyRate_LostFocus()
    '    If IsNumeric(txtCurrencyRate) Then
    '        CurrencyRatePayToLoss = CDbl(txtCurrencyRate.Text)
    '    End If
    '    txtCurrencyRate.Text = Format(CurrencyRatePayToLoss, "0.00######")
    '    If ThisPayment <> 0 And CurrencyRatePayToLoss <> 0 Then
    '        ThisPaymentLossCurrency = Round(ThisPayment * CurrencyRatePayToLoss, 2)
    '    End If
    '    txtThisPaymentLoss.Text = FormatField(PMFormatCurrency, ThisPaymentLossCurrency)
    'End Sub

    Private Sub txtTaxValue_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTaxValue.Leave

        Dim dbNumericTemp As Double
        If Double.TryParse(txtTaxValue.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            TaxValue = CDec(txtTaxValue.Text)
        End If

        txtTaxValue.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, TaxValue)
        txtTotalInclTax.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, ThisPayment + TaxValue)
    End Sub

    Private Sub cboPaymentCurrency_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPaymentCurrency.SelectedIndexChanged

        PaymentCurrencyID = VB6.GetItemData(cboPaymentCurrency, cboPaymentCurrency.SelectedIndex)
        PaymentCurrencyName = VB6.GetItemString(cboPaymentCurrency, cboPaymentCurrency.SelectedIndex).Trim()

        m_lReturn = GetCurrencyRate()

        'txtCurrencyRate.Text = Format(CurrencyRatePayToLoss, "0.00######")

        If ThisPayment <> 0 And CurrencyRatePayToLoss <> 0 Then
            ThisPaymentLossCurrency = Math.Round(ThisPayment * CurrencyRatePayToLoss, 2)
        End If

        txtThisPaymentLoss.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, ThisPaymentLossCurrency)

    End Sub

    '==============
    'Public Methods
    '==============
    '******************************************************************************
    ' Name:         Initialise
    ' Description:  Function specially made for this user control to pass all
    '               required data in one go. Not used in the same way in original
    '               iCLMPeril (generic peril) code
    '******************************************************************************
    Public Function Initialise() As Integer



        Try

            'Setup data and stuff
            SetInterfaceDefaults()

            'Fill up the values for the screen
            txtRiskType.Text = RiskType

            '    txtCurrentReserve.Text = FormatField(PMFormatCurrency, CurrentReserve)
            '    txtPaidToDate.Text = FormatField(PMFormatCurrency, PaidToDate)
            '    txtThisPaymentLoss.Text = FormatField(PMFormatCurrency, ThisPaymentLossCurrency)
            '    txtThisPayment.Text = FormatField(PMFormatCurrency, ThisPayment)
            '    txtLossCurrencyName.Text = LossCurrencyName
            '    cboPaymentCurrency.Text = PaymentCurrencyName
            '    txtTaxValue.Text = FormatField(PMFormatCurrency, TaxValue)

            txtCurrentReserve.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, ReplaceNullWithDefault(CurrentReserve, 0))
            txtPaidToDate.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, ReplaceNullWithDefault(PaidToDate, 0))
            txtThisPaymentLoss.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, ReplaceNullWithDefault(ThisPaymentLossCurrency, 0))
            txtThisPayment.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, ReplaceNullWithDefault(ThisPayment, 0))
            txtLossCurrencyName.Text = LossCurrencyName
            cboPaymentCurrency.Text = PaymentCurrencyName
            txtTaxValue.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, ReplaceNullWithDefault(TaxValue, 0))
            txtTotalInclTax.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, ReplaceNullWithDefault(CDec(txtThisPayment.Text) + CDec(txtTaxValue.Text), 0))

            'Fill the combos
            'TAX TYPE
            For lLoop As Integer = 0 To cboTaxType.Items.Count - 1
                If VB6.GetItemString(cboTaxType, lLoop) = TaxTypeCode Then
                    cboTaxType.SelectedIndex = lLoop
                    Exit For
                End If
            Next
            'TAX BAND
            For lLoop As Integer = 0 To cboTaxBand.Items.Count - 1
                If VB6.GetItemString(cboTaxBand, lLoop) = TaxBand Then
                    cboTaxBand.SelectedIndex = lLoop
                    Exit For
                End If
            Next

        Catch


            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function

    '********************************************************************************
    ' Name:         FillTaxTypeCombo
    ' Author:       Alix Bergeret
    ' Date:         14/05/2003
    ' Description:  -
    '********************************************************************************
    Public Function FillTaxTypeCombo(ByRef r_oBusiness As Object) As Integer

        Dim result As Integer = 0
        Dim sPrevious As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Empty lists
            cboTaxType.Items.Clear()
            cboTaxBand.Items.Clear()

            ' Get items from DB

            m_lReturn = r_oBusiness.GetTaxTypesTaxBands(m_vTaxArray)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                For lIndex As Integer = m_vTaxArray.GetLowerBound(1) To m_vTaxArray.GetUpperBound(1)
                    If sPrevious <> CStr(m_vTaxArray(PMTaxArray_TaxTypeDesc, lIndex)).Trim() Then
                        ' Add tax type description
                        Dim cboTaxType_NewIndex As Integer = -1
                        cboTaxType_NewIndex = cboTaxType.Items.Add(CStr(m_vTaxArray(PMTaxArray_TaxTypeDesc, lIndex)).Trim())
                        ' Add tax type ID
                        VB6.SetItemData(cboTaxType, cboTaxType_NewIndex, CInt(m_vTaxArray(PMTaxArray_TaxTypeID, lIndex)))
                    End If
                    sPrevious = CStr(m_vTaxArray(PMTaxArray_TaxTypeDesc, lIndex)).Trim()
                Next lIndex
            ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then

            Else
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load tax types" & " and bands from DB.", vApp:=ACApp, vClass:=ACClass, vMethod:="FillTaxTypeCombo")
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FillTaxTypeCombo failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FillTaxTypeCombo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '===============
    'Private Methods
    '===============

    '********************************************************************************
    ' Name:         FillTaxBandCombo
    ' Author:       Alix Bergeret
    ' Date:         15/05/2003
    ' Description:  -
    '********************************************************************************
    Private Function FillTaxBandCombo(ByVal v_lTaxTypeID As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Empty list
            cboTaxBand.Items.Clear()

            ' Loop through array
            If Information.IsArray(m_vTaxArray) Then
                For lIndex As Integer = m_vTaxArray.GetLowerBound(1) To m_vTaxArray.GetUpperBound(1)
                    If v_lTaxTypeID = gPMFunctions.NullToLong(CStr(m_vTaxArray(PMTaxArray_TaxTypeID, lIndex))) Then
                        ' Add tax type description
                        Dim cboTaxBand_NewIndex As Integer = -1
                        cboTaxBand_NewIndex = cboTaxBand.Items.Add(CStr(m_vTaxArray(PMTaxArray_TaxBandDesc, lIndex)).Trim())
                        ' Add tax type ID
                        VB6.SetItemData(cboTaxBand, cboTaxBand_NewIndex, CInt(m_vTaxArray(PMTaxArray_TaxBandID, lIndex)))
                        ' Save tax type code in variable
                        '                TaxTypeCode = m_vTaxArray(PMTaxArray_TaxTypeCode, lIndex)
                        '                m_sTaxBandDesc = m_vTaxArray(PMTaxArray_TaxTypeDesc, lIndex)
                    End If
                Next lIndex
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FillTaxBandCombo failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FillTaxBandCombo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    '********************************************************************************
    ' Name:         CalculateTax
    ' Author:       Alix Bergeret
    ' Date:         15/05/2003
    ' Description:  -
    '********************************************************************************
    Private Function CalculateTax() As Integer

        Dim result As Integer = 0
        Dim iIsValue As Integer
        Dim dRate As Double

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get rate from array
            'Male sure valid Tax Type and band have been selected
            If Information.IsArray(m_vTaxArray) And cboTaxBand.SelectedIndex > -1 And cboTaxType.SelectedIndex > -1 Then
                For lIndex As Integer = m_vTaxArray.GetLowerBound(1) To m_vTaxArray.GetUpperBound(1)
                    If VB6.GetItemData(cboTaxBand, cboTaxBand.SelectedIndex) = CDbl(m_vTaxArray(PMTaxArray_TaxBandID, lIndex)) Then
                        iIsValue = gPMFunctions.NullToInteger(CStr(m_vTaxArray(PMTaxArray_TaxIsValue, lIndex)))
                        dRate = gPMFunctions.NullToDouble(CStr(m_vTaxArray(PMTaxArray_TaxRate, lIndex)))
                        Exit For
                    End If
                Next lIndex
            End If

            ' Calculate tax
            If iIsValue = 1 Then
                TaxValue = dRate
            Else
                TaxValue = ThisPayment * dRate / 100
            End If

            ' Work out total payment
            TotalInclTax = ThisPayment + TaxValue

            ' Display tax amount and total payment
            txtTaxValue.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, TaxValue)
            txtTotalInclTax.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, TotalInclTax)

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CalculateTax failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CalculateTax", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result


            Return result
        End Try
    End Function

    Private Function CreateCurrencyConvertObject() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oCurrencyConvert Is Nothing Then
                'Get Currency Convert Object.
                Dim temp_m_oCurrencyConvert As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oCurrencyConvert, "bACTCurrencyConvert.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oCurrencyConvert = temp_m_oCurrencyConvert
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", , " + "Failed to create instance of " & "bACTCurrencyConvert.Form")
                End If
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateCurrencyConvertObject" & " failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateCurrencyConvertObject", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Private Function CreateInsuranceFileObject() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oInsuranceFile Is Nothing Then
                'Get Insurance File Object.
                Dim temp_m_oInsuranceFile As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oInsuranceFile, "bSIRInsuranceFile.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oInsuranceFile = temp_m_oInsuranceFile
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to create instance of bSIRInsuranceFile.Business")
                End If
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateInsuranceFileObject " & "failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateInsuranceFileObject", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    '******************************************************************************
    ' Name:         GetCurrencyRate
    ' Description:  Get the current currency rate for converting from payment
    '               currency to loss currency.
    ' History:
    '******************************************************************************
    Private Function GetCurrencyRate() As Integer

        Dim result As Integer = 0
        Dim dPaymentRate, dLossRate As Double

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get business object
            m_lReturn = CreateCurrencyConvertObject()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function GetCurrencyRate failed.")
            End If

            'Get payment rate to base

            m_lReturn = m_oCurrencyConvert.GetCurrencyRate(v_lCurrencyID:=PaymentCurrencyID, v_lCompanyID:=m_lClaimCompanyID, v_dtConversionDate:=DateTime.Now, r_vConversionRate:=dPaymentRate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function m_oCurrencyConvert.GetCurrencyRate failed.")
            End If

            'Get loss rate to base

            m_lReturn = m_oCurrencyConvert.GetCurrencyRate(v_lCurrencyID:=g_lCurrencyID, v_lCompanyID:=m_lClaimCompanyID, v_dtConversionDate:=DateTime.Now, r_vConversionRate:=dLossRate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function m_oCurrencyConvert.GetCurrencyRate failed.")
            End If

            'Calculate payment rate to loss
            CurrencyRatePayToLoss = dPaymentRate / dLossRate

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCurrencyRate failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrencyRate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    Private Function GetClaimCompany() As Integer

        Dim result As Integer = 0
        Dim vInsuranceFile As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get insurance file object
            m_lReturn = CreateInsuranceFileObject()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function CreateInsuranceFileObject failed.")
            End If

            'Get company id from insurance file

            m_lReturn = m_oInsuranceFile.GetDetails(vInsuranceFileCnt:=g_lInsurance_file_cnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function m_oInsuranceFile.GetDetails failed.")
            End If


            m_lReturn = m_oInsuranceFile.GetNext(r_vFieldArray:=vInsuranceFile)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function m_oInsuranceFile.GetNext failed.")
            End If


            m_lClaimCompanyID = CInt(vInsuranceFile(5))

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimCompany failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimCompany", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result


            Return result
        End Try
    End Function

    Private Sub HideControls()

        lblCurrentReserve.Visible = False
        txtCurrentReserve.Visible = False

        lblPaidToDate.Visible = False
        txtPaidToDate.Visible = False

        lblThisPayment.Visible = False
        txtThisPayment.Visible = False

        lblThisPaymentLoss.Visible = False
        txtThisPaymentLoss.Visible = False

        lblCurrency.Visible = False
        cboPaymentCurrency.Visible = False

        '    lblCurrencyRate.Visible = False
        '    txtCurrencyRate.Visible = False

        lblTaxType.Visible = False
        cboTaxType.Visible = False
        lblTaxBand.Visible = False
        cboTaxBand.Visible = False
        lblTaxValue.Visible = False
        txtTaxValue.Visible = False
        lblTotalInclTax.Visible = False
        txtTotalInclTax.Visible = False

    End Sub

    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get data for the currency combos
            FillCurrencyCombo()
            SetDefaultForCurrencyCombo()
            FillTaxTypeCombo(obCLMPeril)

            lblCurrentReserve.Text = "Current Reserve :" & Strings.Chr(13) & Strings.Chr(10) & "(Loss Currency)"
            lblPaidToDate.Text = "Paid to Date :" & Strings.Chr(13) & Strings.Chr(10) & "(Loss Currency)"

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetInterfaceDefaults failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    Private Function FillCurrencyCombo() As Integer

        Dim result As Integer = 0
        Dim vReturnArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'm_lReturn = obCLMPeril.RetrieveCurrenciesForBranch(isourceid:=g_iSourceID, _
            'vReturnArray:=vReturnArray)


            m_lReturn = obCLMPeril.RetrieveCurrenciesForClaimBranch(v_lClaimId:=m_lClaimId, r_vResults:=vReturnArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function FillCurrencyCombo failed")
            End If


            For lLoop As Integer = vReturnArray.GetLowerBound(1) To vReturnArray.GetUpperBound(1)
                Dim cboPaymentCurrency_NewIndex As Integer = -1

                cboPaymentCurrency_NewIndex = cboPaymentCurrency.Items.Add(CStr(vReturnArray(1, lLoop)))

                VB6.SetItemData(cboPaymentCurrency, cboPaymentCurrency_NewIndex, CInt(vReturnArray(0, lLoop)))
            Next

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FillCurrencyCombo failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FillCurrencyCombo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    Private Function SetDefaultForCurrencyCombo() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iLoop As Integer = 0 To cboPaymentCurrency.Items.Count - 1
                'Default to loss currency or current payment currency.
                If PaymentCurrencyID <> 0 Then
                    'A payment currency has already been selected for this payment (through a payment to another reserve) so select this payment currnecy and disable the control so that the user cannot change it
                    cboPaymentCurrency.Enabled = False
                    If VB6.GetItemData(cboPaymentCurrency, iLoop) = PaymentCurrencyID Then
                        cboPaymentCurrency.SelectedIndex = iLoop
                        Exit For
                    End If
                Else
                    'As yet there is no payment currency selected - so default the selection to the loss currency and leave this control enabled for the user to change it.
                    If VB6.GetItemData(cboPaymentCurrency, iLoop) = g_lCurrencyID Then
                        cboPaymentCurrency.SelectedIndex = iLoop
                        Exit For
                    End If
                End If
            Next
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetDefaultForCurrencyCombo " & "failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetDefaultForCurrencyCombo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: ReplaceNullWithDefault
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 24-06-2004 : CQ4740
    ' ***************************************************************** '
    Private Function ReplaceNullWithDefault(ByRef v_vValue As Object, ByVal v_vDefault As Object) As Object

        Dim result As Object = Nothing
        Const sFunctionName As String = "ReplaceNullWithDefault"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue





            If CStr(v_vValue) = "" Or v_vValue Is DBNull.Value Or Convert.IsDBNull(v_vValue) Or IsNothing(v_vValue) Or CDbl(v_vValue) = 0 Then



                v_vValue = v_vDefault

            End If


            Return v_vValue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)

            Return result

        End Try
    End Function
End Class
