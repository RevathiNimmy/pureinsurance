Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Text
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmDetails
	Inherits System.Windows.Forms.Form
	
	Private Const ACClass As String = "frmDetails"
	
	Private m_sDescription As String = ""
	Private m_sNominalAccount As String = ""
	Private m_lNominalAccountID As Integer
	Private m_vAmount As Object
	Private m_lCurrencyId As Integer
	Private m_lDepartmentID As Integer
	Private m_sDepartment As String = ""
	Private m_vDeptAmount As Object
	Private m_bHasVAT As Boolean
	'Datasure
	Private m_cTaxAmount As Decimal
    'Developer Guide No.101
    Private m_aTaxArray(,) As Object = Nothing
	Private m_iTaxGroupId As Integer
	' Declare an instance of the Tax Calculation Business object.
	Private m_oTaxCalculation As Object
	Private m_oBusiness As Object
	
	Private m_lErrorNumber As Integer
	Private m_lStatus As Integer
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	Private m_iTask As Integer
	
	'DC020806
	Private m_sTaxGroupCode As String = ""
	
	Private m_oFormFields As iPMFormControl.FormFields
	
	'PN5503
	'Public Property Get CurrencyID() As Long
	'    CurrencyID = m_lCurrencyID&
	'End Property
	'Datasure - reinstated
	Public WriteOnly Property CurrencyID() As Integer
		Set(ByVal Value As Integer)
			m_lCurrencyId = Value
		End Set
	End Property
	
	Public Property NominalAccount() As String
		Get
			Return m_sNominalAccount
		End Get
		Set(ByVal Value As String)
			m_sNominalAccount = Value
		End Set
	End Property
	
	Public Property Department() As String
		Get
			Return m_sDepartment
		End Get
		Set(ByVal Value As String)
			m_sDepartment = Value
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
	
	Public Property Status() As Integer
		Get
			Return m_lStatus
		End Get
		Set(ByVal Value As Integer)
			m_lStatus = Value
		End Set
	End Property
	
	Public Property Description() As String
		Get
			Return m_sDescription
		End Get
		Set(ByVal Value As String)
			m_sDescription = Value
		End Set
	End Property
	
	Public Property NominalAccountID() As Integer
		Get
			Return m_lNominalAccountID
		End Get
		Set(ByVal Value As Integer)
			m_lNominalAccountID = Value
		End Set
	End Property
	
	Public Property Amount() As Object
		Get
			Return m_vAmount
		End Get
		Set(ByVal Value As Object)


			m_vAmount = Value
		End Set
	End Property
	
	Public Property DepartmentID() As Integer
		Get
			Return m_lDepartmentID
		End Get
		Set(ByVal Value As Integer)
			m_lDepartmentID = Value
		End Set
	End Property
	
	Public Property DeptAmount() As Object
		Get
			Return m_vDeptAmount
		End Get
		Set(ByVal Value As Object)


			m_vDeptAmount = Value
		End Set
	End Property
	
	Public Property HasVAT() As Boolean
		Get
			Return m_bHasVAT
		End Get
		Set(ByVal Value As Boolean)
			m_bHasVAT = Value
		End Set
	End Property
	Public Property TaxAmount() As Decimal
		Get
			Return m_cTaxAmount
		End Get
		Set(ByVal Value As Decimal)
			m_cTaxAmount = Value
		End Set
	End Property
	
	Public Property TaxGroupId() As Integer
		Get
			Return m_iTaxGroupId
		End Get
		Set(ByVal Value As Integer)
			m_iTaxGroupId = Value
		End Set
    End Property
    'Developer Guide no. 101
    Public Property TaxArray() As Object
        Get
            Return m_aTaxArray
        End Get
        'Developer Guide no. 101
        Set(ByVal Value As Object)

            m_aTaxArray = Value
        End Set
    End Property

    'DC020806 set tax group code
    Public Property TaxGroupCode() As String
        Get
            Return m_sTaxGroupCode
        End Get
        Set(ByVal Value As String)
            m_sTaxGroupCode = Value
        End Set
    End Property
    'PN5508 Moved to main screen
    'Private Sub cboCurrency_LostFocus()
    '
    ' End Sub

    Private Sub cboCostCentre_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCostCentre.Click

        If cboCostCentre.ItemId = 0 Then
            ' Hide the amount/combo
            txtDeptAmount.Visible = False
            cboValType.Visible = False
            lblDeptAmount.Visible = False
        Else
            ' Hide the amount/combo
            txtDeptAmount.Visible = True
            cboValType.Visible = True
            lblDeptAmount.Visible = True
        End If

    End Sub

   

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        Me.Hide()

    End Sub

    ' ***************************************************************** '
    '
    ' Name: InterfaceToProperties
    '
    ' Description:
    '
    ' History: 10/04/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function InterfaceToProperties() As Integer

        Dim result As Integer = 0
        Dim dPercentage As Double
        Dim vTotalIncVAT As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_sDescription = CStr(m_oFormFields.UnformatControl(ctlControl:=txtDescription))
            m_lNominalAccountID = uctNominalAccount.AccountId
            m_sNominalAccount = uctNominalAccount.Text


            m_vAmount = m_oFormFields.UnformatControl(ctlControl:=txtAmount)
            m_lDepartmentID = cboCostCentre.ItemId
            m_sDepartment = cboCostCentre.ItemCaption
            'PN5508 Moved to main screen
            '    m_lCurrencyID& = cboCurrency.ItemId


            vTotalIncVAT = m_oFormFields.UnformatControl(ctlControl:=txtTotalIncVat)

            'Datasure

            m_cTaxAmount = CDec(m_oFormFields.UnformatControl(ctlControl:=txtVATAmount))
            m_iTaxGroupId = cboPMTax.ItemId

            m_bHasVAT = Not (m_iTaxGroupId = 0)

            If txtDeptAmount.Text.Trim().Length > 0 Then


                Select Case VB6.GetItemData(cboValType, cboValType.SelectedIndex)
                    Case 1 ' Percent

                        dPercentage = CDbl(m_oFormFields.UnformatControl(txtDeptAmount))

                        If dPercentage > 100 Then
                            MessageBox.Show("You cannot allocate more than 100% of the amount to a department.", "Percentage", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If dPercentage < 0 Then
                            MessageBox.Show("The percentage allocated to a department must be a positive number.", "Department percentage", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        ' Calculate the amount


                        m_vDeptAmount = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=(CStr((CDbl(vTotalIncVAT) / 100) * dPercentage)))

                    Case 2 ' Amount


                        m_vDeptAmount = m_oFormFields.UnformatControl(txtDeptAmount)

                End Select

                ' Check if it's greater than the total amount


                If Math.Abs(CDec(m_vDeptAmount)) > Math.Abs(CDec(vTotalIncVAT)) Then
                    MessageBox.Show("The department amount cannot be greater than the overall amount.", "Amount", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    If txtDeptAmount.Visible And txtDeptAmount.Enabled Then
                        txtDeptAmount.Focus()
                    End If
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else


                m_vDeptAmount = 0

                ' Have they chosen an department, but not entered an amount?
                If m_lDepartmentID <> 0 Then

                    If MessageBox.Show("You have selected a department but not entered an amount." & Environment.NewLine & "Do you wish to do so?", "No amount", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
                        If txtDeptAmount.Visible And txtDeptAmount.Enabled Then
                            txtDeptAmount.Focus()
                        End If
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InterfaceToProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: PropertiesToInterface
    '
    ' Description:
    '
    ' History: 10/04/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function PropertiesToInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oFormFields.FormatControl(txtDescription, m_sDescription)

            If (m_lNominalAccountID = 0) And (m_sNominalAccount.Trim().Length > 0) Then
                uctNominalAccount.Text = m_sNominalAccount
            Else
                uctNominalAccount.AccountId = m_lNominalAccountID
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAmount, vControlValue:=m_vAmount)
            cboCostCentre.ItemId = m_lDepartmentID

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDeptAmount, vControlValue:=m_vDeptAmount)
            'PN5503 Moved to main screen
            '    cboCurrency.ItemId = m_lCurrencyID&

            'Datasure
            cboPMTax.ItemId = m_iTaxGroupId

            'DC020806 Datasure
            m_sTaxGroupCode = cboPMTax.ItemCode

            ' Default to amount as this is what's stored
            cboValType.SelectedIndex = 1

            ' Update VAT
            m_lReturn = CType(UpdateTax(), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PropertiesToInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Private Function UpdateTax() As Integer

        Dim crAmount As Decimal
        Dim nTaxGroupId As Integer
        Dim crTax As Decimal
        Dim crTotal As Decimal

        If String.IsNullOrEmpty(txtAmount.Text) Then Exit Function
        If cboPMTax.ItemId = 0 Then
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtVATAmount, vControlValue:=0)
            txtTotalIncVat.Text = txtAmount.Text
            m_aTaxArray = Nothing
        Else
            'VAT is checked.

            crAmount = gPMFunctions.ToSafeCurrency(CStr(m_oFormFields.UnformatControl(txtAmount)))

            nTaxGroupId = cboPMTax.ItemId

            'DC020806 get grouo code
            m_sTaxGroupCode = cboPMTax.ItemCode

            'Calculate the Tax here
            If m_lCurrencyId = 0 Then
                m_lCurrencyId = g_iCurrencyID
            End If

            m_lReturn = m_oTaxCalculation.PreviewTax(v_lTaxGroupId:=nTaxGroupId, v_iCurrencyId:=m_lCurrencyId, v_cTaxableAmount:=crAmount, v_dtEffectiveDate:=DateTime.Now, r_vTax:=m_aTaxArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to calculate taxes", "Error", MessageBoxButtons.OK)
            End If
            If crAmount > 0 Then
                If Information.IsArray(m_aTaxArray) Then
                    For i As Integer = 0 To m_aTaxArray.GetUpperBound(1)
                        crTax += Math.Round(CDec(m_aTaxArray(1, i)), 2)
                        m_aTaxArray(1, i) = CStr(Math.Round(CDec(m_aTaxArray(1, i)), 2))
                    Next i
                End If
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtVATAmount, vControlValue:=crTax)

            crTax = CDec(m_oFormFields.UnformatControl(txtVATAmount))

            crTotal = crAmount + crTax

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtTotalIncVat, vControlValue:=crTotal)

        End If

        Exit Function
    End Function
    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        Dim sTaxCode As String = ""
        Dim lTaxAccountID As Integer
        Dim sMissingTaxCode As New StringBuilder
        Dim iFlag As gPMConstants.PMEReturnCode

        m_lStatus = gPMConstants.PMEReturnCode.PMOK

        m_lReturn = m_oFormFields.CheckMandatoryControls()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        'eck051001
        If uctNominalAccount.Text.Trim() = "" Then
            MessageBox.Show(" Please Enter a valid Nominal Account", "Nominal Account", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If Information.IsArray(m_aTaxArray) Then

            iFlag = gPMConstants.PMEReturnCode.PMFalse
            For iTaxPtr As Integer = 0 To m_aTaxArray.GetUpperBound(1)
                'DC020806 change format for tax account code
                sTaxCode = "TAX" & m_sTaxGroupCode.TrimEnd() & "IN"

                m_lReturn = m_oBusiness.GetAccountID(lTaxAccountID, sTaxCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                Else
                    If lTaxAccountID = 0 Then
                        sMissingTaxCode.Append(sTaxCode & Environment.NewLine)
                        iFlag = gPMConstants.PMEReturnCode.PMTrue
                    End If
                End If
            Next iTaxPtr

            If iFlag = gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Following Tax Accounts are Missing for This Tax Posting." & Environment.NewLine & sMissingTaxCode.ToString() & " Please create using Account Explorer", "Tax", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

        End If

        m_lReturn = CType(InterfaceToProperties(), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        Me.Hide()

    End Sub

    ' ***************************************************************** '
    '
    ' Name: SetFieldValidation
    '
    ' Description:
    '
    ' History: 10/04/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oFormFields

                m_lReturn = .AddNewFormField(ctlControl:=txtDescription, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

                m_lReturn = .AddNewFormField(ctlControl:=txtAmount, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

                m_lReturn = .AddNewFormField(ctlControl:=txtDeptAmount, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

                ' txtTotalIncVat
                m_lReturn = .AddNewFormField(ctlControl:=txtTotalIncVat, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory, lDecimalPlaces:=2)

                ' txtVATAmount
                m_lReturn = .AddNewFormField(ctlControl:=txtVATAmount, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory, lDecimalPlaces:=2)

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetFieldValidation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub Form_Initialize_Renamed()
        Dim sMessage, sTitle As String

        'Datasure - tax calculation
        ' Get an instance of the tax calculation object via
        ' the public object manager.
        Dim temp_m_oTaxCalculation As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_m_oTaxCalculation, "bSIRRITax.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        m_oTaxCalculation = temp_m_oTaxCalculation

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bACTInvoice.Business", vInstanceManager:="ClientManager")
            m_oBusiness = temp_m_oBusiness
        End If


        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get an instance of the business object.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

            ' Display error stating the problem.

            ' Get description from the resource file.

            'Developer Guide No. 243
            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


            'Developer Guide No. 243
            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))

            ' Display message.
            MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

            Exit Sub
        End If


    End Sub


    Private Sub frmDetails_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load



        'Developer Guide no. 220
        Me.cboCostCentre.FirstItem = "(none)"
        Me.cboPMTax.FirstItem = "(none)"
        ' Get form control
        m_oFormFields = New iPMFormControl.FormFields()

        ' Add the values to the combo box
        With cboValType
            .Items.Insert(0, "Percent")
            VB6.SetItemData(cboValType, 0, 1)
            .Items.Insert(1, "Amount")
            VB6.SetItemData(cboValType, 1, 2)

            .SelectedIndex = 0
        End With
        'PN30095
        If cboPMTax.ListCount >= 1 Then
            cboPMTax.ListIndex = 1
        End If
        m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)

        Select Case (m_iTask)
            Case gPMConstants.PMEComponentAction.PMEdit
                m_lReturn = CType(PropertiesToInterface(), gPMConstants.PMEReturnCode)

            Case gPMConstants.PMEComponentAction.PMAdd
                ' Default the currency to GBP
                'PN5508
                'cboCurrency.ItemId = 26
                ' Set the nominal
                uctNominalAccount.AccountId = m_lNominalAccountID

        End Select

        cboCostCentre_Click(cboCostCentre, Nothing)

    End Sub

    Private Sub frmDetails_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
        'Datasure
        ' Terminate the tax calculation business object

        m_oTaxCalculation.Dispose()
        ' Destroy the instance of the tax calculation object
        ' from memory.
        m_oTaxCalculation = Nothing
        m_oBusiness = Nothing

        eventArgs.Cancel = Cancel <> 0
    End Sub

    Private Sub txtAmount_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAmount.Enter

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtAmount)

    End Sub

    Private Sub txtAmount_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAmount.Leave

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtAmount)

        If Strings.Len(txtAmount.Text) > 0 Then
            ' Update VAT
            m_lReturn = CType(UpdateTax(), gPMConstants.PMEReturnCode)
        End If

    End Sub

    Private Sub txtAmount_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles txtAmount.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        Dim cAmount As Decimal = gPMFunctions.ToSafeCurrency(txtAmount.Text)
        txtAmount.Text = CStr(cAmount)
        eventArgs.Cancel = Cancel
    End Sub

    Private Sub txtDeptAmount_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDeptAmount.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDeptAmount)
    End Sub

    Private Sub txtDeptAmount_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDeptAmount.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDeptAmount)
    End Sub

    Private Sub txtDescription_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDescription)
    End Sub

    Private Sub txtDescription_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDescription)
    End Sub

    Private Sub txtTotalIncVat_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTotalIncVat.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtTotalIncVat)
    End Sub

    Private Sub txtTotalIncVat_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTotalIncVat.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtTotalIncVat)
    End Sub

    Private Sub txtVATAmount_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtVATAmount.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtVATAmount)
    End Sub

    Private Sub txtVATAmount_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtVATAmount.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtVATAmount)
    End Sub


    Private Sub frmDetails_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
    End Sub

  
    Private Sub cboPMTax_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPMTax.Leave
        m_lReturn = UpdateTax()
    End Sub
    
End Class
