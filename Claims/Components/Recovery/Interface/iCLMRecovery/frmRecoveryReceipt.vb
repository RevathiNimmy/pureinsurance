Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Windows.Forms
'Developer Guide no. 129
Imports SharedFiles
Partial Friend Class frmRecoveryReceipt
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name:   frmRecoveryReserve
    ' Date:        07/04/2005
    ' Description: Allow maintenance of recovery reserves
    ' ***************************************************************** '

    Private Const ACClass As String = "frmRecoveryReceipt"



    ' ***************************************************************** '
    '                       PRIVATE PROPERTIES
    ' ***************************************************************** '
    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Currency conversion object

    Private m_oCurrencyConvert As bACTCurrencyConvert.Form

    ' Array for tax config
    Private m_vTaxArray(,) As Object

    ' Stores the return value for the a function call.
    Private m_lReturn As Integer


    ' ***************************************************************** '
    '                       PUBLIC PROPERTIES
    '
    ' Note:
    '      Public PerilID As Long
    ' is the SAME as:
    '      Public Property Let PerilID(ByVal RHS As Long)
    '      End Property
    '      Public Property Get PerilID() As Long
    '      End Property
    ' ***************************************************************** '
    Public PerilID As Integer
    Public ClaimCompanyID As Integer

    Public RecoveryType As String = ""
    Public InitialReserve As Decimal
    Public TotalReserve As Decimal
    Public ReceivedToDate As Decimal

    Public ReceiptCurrencyID As Integer
    Public ReceiptCurrency As String = ""
    Public CurrencyRate As Double
    Public LossCurrencyID As Integer
    Public LossCurrency As String = ""

    Public ThisReceipt As Decimal

    Public TaxTypeID As Integer
    Public TaxType As String = ""
    Public TaxTypeCode As String = ""
    Public TaxBandID As Integer
    Public TaxBand As String = ""
    Public TaxAmount As Decimal

    Public Status As gPMConstants.PMEReturnCode
    Public AllowCurrencyRateOverride As Boolean
    Public ReceiptCurrencySet As Boolean


    Public ReadOnly Property Balance() As Decimal
        Get
            Return CurrentBalance - ThisReceiptLoss
        End Get
    End Property

    Public ReadOnly Property CurrentBalance() As Decimal
        Get
            Return TotalReserve - ReceivedToDate
        End Get
    End Property

    Public ReadOnly Property ThisReceiptLoss() As Decimal
        Get
            Return ThisReceipt * CurrencyRate
        End Get
    End Property

    Public ReadOnly Property NetReceipt() As Decimal
        Get
            Return ThisReceipt - TaxAmount
        End Get
    End Property

    Public ReadOnly Property NetReceiptLoss() As Decimal
        Get
            Return ThisReceiptLoss - TaxAmountLoss
        End Get
    End Property

    Public ReadOnly Property TaxAmountLoss() As Decimal
        Get
            Return TaxAmount * CurrencyRate
        End Get
    End Property



    ' ***************************************************************** '
    '                        PRIVATE FUNCTIONS
    ' ***************************************************************** '

    ' Calculate applicable tax
    Private Function CalculateTax() As Integer

        Dim result As Integer = 0
        Dim iIsValue As Integer
        Dim dRate As Double

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get rate from array
            If Information.IsArray(m_vTaxArray) Then
                For lCount As Integer = m_vTaxArray.GetLowerBound(1) To m_vTaxArray.GetUpperBound(1)
                    ' We only need look for the band, it is unique to the type
                    If CDbl(m_vTaxArray(ACTaxBandID, lCount)) = TaxBandID Then
                        iIsValue = gPMFunctions.ToSafeInteger(CInt(m_vTaxArray(ACTaxIsValue, lCount)))
                        dRate = gPMFunctions.ToSafeDouble(CInt(m_vTaxArray(ACTaxRate, lCount)))
                        Exit For
                    End If
                Next lCount
            End If

            ' Calculate new value, we don't need to display this will be done automatically
            If iIsValue Then
                TaxAmount = dRate
            Else
                TaxAmount = ThisReceipt - (ThisReceipt / (1 + (dRate / 100)))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CalculateTax failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CalculateTax", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' Create the currency convert object, if it hasn' t been already
    Private Function CreateCurrencyConvert() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oCurrencyConvert Is Nothing Then
                ' Get Currency Convert Object.
                Dim temp_m_oCurrencyConvert As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oCurrencyConvert, "bACTCurrencyConvert.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oCurrencyConvert = temp_m_oCurrencyConvert
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to create instance of bACTCurrencyConvert.Form")
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateCurrencyConvert failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateCurrencyConvert", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' Display all language specific captions.
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Select Case g_lRecoveryMode
                Case MainModule.RecoveryModeEnum.RMThirdPartyReserve, MainModule.RecoveryModeEnum.RMThirdPartyReceipt
                    Text = "Third Party Recovery Reserve"
                Case MainModule.RecoveryModeEnum.RMSalvageReserve, MainModule.RecoveryModeEnum.RMSalvageReceipt
                    Text = "Salvage Recovery Reserve"
                Case Else
                    ' Invalid mode for this dialog
                    result = gPMConstants.PMEReturnCode.PMFalse
            End Select


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' Get the current currency rate for converting from payment currency to loss currency.
    Private Function GetCurrencyRate() As Integer

        Dim result As Integer = 0
        Dim dPaymentRate, dLossRate As Double

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if rates are same
            If ReceiptCurrencyID = LossCurrencyID Then
                CurrencyRate = 1
                Return result
            End If

            ' Get business object
            m_lReturn = CreateCurrencyConvert()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function CreateCurrencyConvert failed.")
            End If

            ' Get payment rate to base

            m_lReturn = m_oCurrencyConvert.GetCurrencyRate(v_lCurrencyID:=ReceiptCurrencyID, v_lCompanyID:=ClaimCompanyID, v_dtConversionDate:=DateTime.Now, r_vConversionRate:=dPaymentRate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function m_oCurrencyConvert.GetCurrencyRate failed.")
            End If

            ' Get loss rate to base

            m_lReturn = m_oCurrencyConvert.GetCurrencyRate(v_lCurrencyID:=LossCurrencyID, v_lCompanyID:=ClaimCompanyID, v_dtConversionDate:=DateTime.Now, r_vConversionRate:=dLossRate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function m_oCurrencyConvert.GetCurrencyRate failed.")
            End If

            ' Calculate payment rate to loss
            CurrencyRate = dPaymentRate / dLossRate

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCurrencyRate failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrencyRate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' Updates the interface details from the property members.
    Private Function PropertiesToInterface() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reserve
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtRecoveryType, vControlValue:=RecoveryType)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtInitialReserve, vControlValue:=InitialReserve)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtTotalReserve, vControlValue:=TotalReserve)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtReceivedToDate, vControlValue:=ReceivedToDate)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCurrentBalance, vControlValue:=CurrentBalance)

            ' Receipt
            cboReceiptCurrency.CurrencyId = ReceiptCurrencyID
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtLossCurrency, vControlValue:=LossCurrency)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCurrencyRate, vControlValue:=CurrencyRate)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtThisReceipt, vControlValue:=ThisReceipt)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtThisReceiptLoss, vControlValue:=ThisReceiptLoss)

            ' Taxes
            m_lReturn = iPMFunc.SetComboBoxValue(cboTaxType, CStr(TaxTypeID))
            m_lReturn = iPMFunc.SetComboBoxValue(cboTaxBand, CStr(TaxBandID))
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtTaxAmount, vControlValue:=TaxAmount)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtTaxAmountLoss, vControlValue:=TaxAmountLoss)

            ' Totals
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtNetReceipt, vControlValue:=NetReceipt)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtNetReceiptLoss, vControlValue:=NetReceiptLoss)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtBalance, vControlValue:=Balance)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' Refresh all calculated values on the form
    Private Function RefreshInterface() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RefreshInterface"
        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Receipt
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCurrencyRate, vControlValue:=CurrencyRate)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtThisReceiptLoss, vControlValue:=ThisReceiptLoss)

            ' Taxes
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtTaxAmount, vControlValue:=TaxAmount)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtTaxAmountLoss, vControlValue:=TaxAmountLoss)

            ' Totals
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtNetReceipt, vControlValue:=NetReceipt)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtNetReceiptLoss, vControlValue:=NetReceiptLoss)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtBalance, vControlValue:=Balance)


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


            ' This is for debugging only



        End Try
        Return result
    End Function

    ' Sets the rules for validating fields.
    Private Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reserve
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtRecoveryType, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtInitialReserve, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtTotalReserve, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtReceivedToDate, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCurrentBalance, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency)

            ' Receipt
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboReceiptCurrency, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtLossCurrency, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCurrencyRate, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDouble, lDecimalPlaces:=4, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtThisReceipt, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtThisReceiptLoss, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency)

            ' Taxes
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboTaxType, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboTaxBand, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtTaxAmount, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtTaxAmountLoss, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency)

            ' Totals
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtNetReceipt, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtNetReceiptLoss, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtBalance, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' Sets all of the interface default values.
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetInterfaceDefaults"
        Dim lReturn As gPMConstants.PMEReturnCode

        Dim bAllowOverride As Boolean
        Dim lPrevious As Integer

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("DisplayCaptions", "Unable to display captions")
            End If

            ' Populate currency
            cboReceiptCurrency.CompanyId = ClaimCompanyID
            cboReceiptCurrency.Refresh()

            ' Get tax details

            lReturn = g_oBusiness.GetTaxTypesTaxBands(m_vTaxArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("PropertiesToInterface", "Failed to populate tax types")
            End If

            ' Load tax type combo
            lPrevious = -1
            Dim cboTaxType_NewIndex As Integer = -1
            cboTaxType_NewIndex = cboTaxType.Items.Add("(none)")
            For lCount As Integer = m_vTaxArray.GetLowerBound(1) To m_vTaxArray.GetUpperBound(1)
                ' Check if we're on a new tax type
                If lPrevious <> CDbl(m_vTaxArray(ACTaxTypeID, lCount)) Then
                    lPrevious = CInt(m_vTaxArray(ACTaxTypeID, lCount))

                    ' Add tax type
                    cboTaxType_NewIndex = cboTaxType.Items.Add(CStr(m_vTaxArray(ACTaxTypeDescription, lCount)).Trim())
                    VB6.SetItemData(cboTaxType, cboTaxType_NewIndex, lPrevious)
                End If
            Next lCount

            ' Update the interface details with the property members.
            lReturn = CType(PropertiesToInterface(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("PropertiesToInterface", "Unable to display data")
            End If

            ' Set currency enable state
            cboReceiptCurrency.Enabled = Not ReceiptCurrencySet
            txtCurrencyRate.ReadOnly = ReceiptCurrencySet Or (Not AllowCurrencyRateOverride)
            txtCurrencyRate.BackColor = IIf(txtCurrencyRate.ReadOnly, ColorTranslator.ToOle(SystemColors.Control), ColorTranslator.ToOle(SystemColors.Window))


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



    ' ***************************************************************** '
    '                         CONTROL EVENTS
    ' ***************************************************************** '
    Private Sub cboReceiptCurrency_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboReceiptCurrency.Click

        ' Store current values
        ReceiptCurrency = cboReceiptCurrency.Text
        ReceiptCurrencyID = cboReceiptCurrency.CurrencyId

        ' Get new rate and refresh interface
        m_lReturn = GetCurrencyRate()
        RefreshInterface()

    End Sub

    Private Sub cboTaxBand_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTaxBand.SelectedIndexChanged

        ' Store current type and populate tax bands available for selected tax type
        If cboTaxBand.SelectedIndex > 0 Then
            ' Store values
            TaxBand = cboTaxBand.Text
            TaxBandID = VB6.GetItemData(cboTaxBand, cboTaxBand.SelectedIndex)
            txtTaxAmount.Enabled = True

            ' Recalculate tax
            CalculateTax()
        Else
            ' Clear tax band
            TaxBand = ""
            TaxBandID = 0
            TaxAmount = 0
            txtTaxAmount.Enabled = False
        End If

        ' Refresh the interface
        RefreshInterface()

    End Sub

    Private Sub cboTaxType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTaxType.SelectedIndexChanged


        Try

            ' Clear tax bands
            cboTaxBand.Items.Clear()
            Dim cboTaxBand_NewIndex As Integer = -1
            cboTaxBand_NewIndex = cboTaxBand.Items.Add("(none)")

            ' Store current type and populate tax bands available for selected tax type
            If cboTaxType.SelectedIndex > 0 Then
                ' Store values
                TaxType = cboTaxType.Text
                TaxTypeID = VB6.GetItemData(cboTaxType, cboTaxType.SelectedIndex)

                ' Populate Tax Bands
                If Information.IsArray(m_vTaxArray) Then
                    For lCount As Integer = m_vTaxArray.GetLowerBound(1) To m_vTaxArray.GetUpperBound(1)
                        ' Is band from select type?
                        If CDbl(m_vTaxArray(ACTaxTypeID, lCount)) = TaxTypeID Then
                            ' Add tax band
                            cboTaxBand_NewIndex = cboTaxBand.Items.Add(CStr(m_vTaxArray(ACTaxBandDescription, lCount)).Trim())
                            VB6.SetItemData(cboTaxBand, cboTaxBand_NewIndex, CInt(m_vTaxArray(ACTaxBandID, lCount)))

                            ' Store type code
                            TaxTypeCode = CStr(m_vTaxArray(ACTaxTypeCode, lCount))
                        End If
                    Next lCount
                End If
            Else
                ' Clear tax type
                TaxType = ""
                TaxTypeID = 0
                TaxTypeCode = ""
            End If

            ' Set default item (unless we are loading the form)
            If Me.Visible Then
                cboTaxBand.SelectedIndex = 0
            End If

        Catch
        End Try




    End Sub


    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        Status = gPMConstants.PMEReturnCode.PMCancel
        Me.Hide()
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Force focus to trigger any outstanding validation
        If Not iPMFunc.ForceLostFocus(cmdOK) Then Exit Sub

        ' Check mandatory controls have been entered into.
        m_lReturn = m_oFormFields.CheckMandatoryControls()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        Status = gPMConstants.PMEReturnCode.PMOK
        Me.Hide()

    End Sub


    Private Sub txtCurrencyRate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCurrencyRate.Enter
        m_oFormFields.GotFocus(txtCurrencyRate)
    End Sub

    Private Sub txtCurrencyRate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCurrencyRate.Leave
        m_oFormFields.LostFocus(txtCurrencyRate)

        ' Store this value and refresh all values

        CurrencyRate = CDbl(m_oFormFields.UnformatControl(txtCurrencyRate))
        RefreshInterface()
    End Sub


    Private Sub txtThisReceipt_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtThisReceipt.Enter
        m_oFormFields.GotFocus(txtThisReceipt)
    End Sub

    Private Sub txtThisReceipt_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtThisReceipt.Leave
        m_oFormFields.LostFocus(txtThisReceipt)

        ' Store this value and refresh all values

        ThisReceipt = CDec(m_oFormFields.UnformatControl(txtThisReceipt))
        CalculateTax()
        RefreshInterface()
    End Sub


    Private Sub txtTaxAmount_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTaxAmount.Enter
        m_oFormFields.GotFocus(txtTaxAmount)
    End Sub

    Private Sub txtTaxAmount_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTaxAmount.Leave
        m_oFormFields.LostFocus(txtTaxAmount)

        ' Store this value and refresh all values

        TaxAmount = CDec(m_oFormFields.UnformatControl(txtTaxAmount))
        RefreshInterface()
    End Sub


    ' ***************************************************************** '
    '                           FORM EVENTS
    ' ***************************************************************** '

    ' Loads all required details of the form

    Private Sub frmRecoveryReceipt_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Try

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()
            m_oFormFields.LanguageID = g_iLanguageID

            ' Set field validation
            m_lReturn = SetFieldValidation()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Status = gPMConstants.PMEReturnCode.PMError
            End If

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Status = gPMConstants.PMEReturnCode.PMError
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub

    Private Sub frmRecoveryReceipt_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
        ' Terminate the form control object.
        m_oFormFields.Dispose()
        m_oFormFields = Nothing

        ' Terminate currency convert if necessary
        If Not (m_oCurrencyConvert Is Nothing) Then

            m_oCurrencyConvert.Dispose()
            m_oCurrencyConvert = Nothing
        End If
        eventArgs.Cancel = Cancel <> 0
    End Sub
End Class