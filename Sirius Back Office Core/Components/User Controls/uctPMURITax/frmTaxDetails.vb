Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmTaxDetail
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmTaxDetails
    '
    ' Date: 25-04-2005
    '
    ' Description: Main interface form.
    '
    ' Edit History:
    '   NB: All Initial functionality has been ripped from iPMURITax
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmTaxDetails"

    '********************************
    ' General Property variables
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lPMAuthorityLevel As Integer
    Private m_bError As Integer

    Private m_oBusiness As bSIRRITax.Business
    Private m_lReturn As Integer
    Private m_bInterfaceError As Boolean
    '********************************

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    Private m_vRITax(,) As Object
    Private m_lSelectedItem As Integer
    Private m_bReadOnly As Boolean
    Private m_bRefresh As Boolean
    Private m_cTaxValue As Decimal
    Private m_bDataHasChanged As Boolean
    Private m_sTransactionType As String = ""
    Private m_bSuppressDecimalValues As Boolean
    ''' <summary>
    ''' Holds The decimal suppress configuration.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsSuppressDecimalValues() As Boolean
        Get
            Return m_bSuppressDecimalValues
        End Get
        Set(ByVal Value As Boolean)
            m_bSuppressDecimalValues = Value
        End Set
    End Property

    Public ReadOnly Property DataHasChanged() As Boolean
        Get
            Return m_bDataHasChanged
        End Get
    End Property

    Public WriteOnly Property ReadOnly_Renamed() As Boolean
        Set(ByVal Value As Boolean)
            m_bReadOnly = Value
        End Set
    End Property

    Public WriteOnly Property Business() As bSIRRITax.Business
        Set(ByVal Value As bSIRRITax.Business)
            m_oBusiness = Value
        End Set
    End Property

    Public Property RITax() As Object
        Get
            Return VB6.CopyArray(m_vRITax)
        End Get
        Set(ByVal Value As Object)
            m_vRITax = Value
        End Set
    End Property

    Public WriteOnly Property SelectedItem() As Integer
        Set(ByVal Value As Integer)
            m_lSelectedItem = Value
        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            m_sTransactionType = Value
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

    Private Sub chkAllowCredit_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkAllowCredit.CheckStateChanged
        If m_bRefresh Then
            Exit Sub
        End If
        ' Recalculate
        m_lReturn = CalculateTaxes()
    End Sub

    Private Sub chkIsValue_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsValue.CheckStateChanged
        ' Display appropriate fields
        SetFormControls()
        ' Recalculate
        m_lReturn = CalculateTaxes()
    End Sub

    Private Sub chkNotApplied_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkNotApplied.CheckStateChanged

        If chkNotApplied.CheckState = CheckState.Checked Then
            chkIncludeIns.Enabled = False
            chkSpread.Enabled = False
        Else
            chkIncludeIns.Enabled = True
            chkSpread.Enabled = True

        End If

    End Sub

    Private Sub chkRounded_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkRounded.CheckStateChanged
        ' Recalculate
        m_lReturn = CalculateTaxes()
    End Sub

    '(RC) AUS Tax
    Private isInitializingComponent As Boolean
    Private Sub optApplyTax_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _optApplyTax_2.CheckedChanged, _optApplyTax_1.CheckedChanged, _optApplyTax_0.CheckedChanged, _optApplyTax_3.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            Dim Index As Integer = Array.IndexOf(optApplyTax, eventSender)

            Dim vRITax As Object
            If m_bRefresh Then
                Exit Sub
            End If

            Dim lTaxCalculationCnt As Integer = CInt(m_vRITax(ACRPrimaryKeyTaxCnt, m_lSelectedItem))
            Dim lApplyTaxBy As Integer = Index


            m_lReturn = m_oBusiness.RecalculateSingleRiskTax(lTaxCalculationCnt, lApplyTaxBy, m_sTransactionType, vRITax)
            If Information.IsArray(vRITax) Then

                m_vRITax(ACRPrimaryKeyTaxCnt, m_lSelectedItem) = vRITax(ACRPrimaryKeyTaxCnt, 0)

                m_lReturn = PopulateSingleRiskTax(vRITax)
            End If

        End If
    End Sub

    Private Sub optBasis_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _optBasis_0.CheckedChanged, _optBasis_1.CheckedChanged, _optBasis_3.CheckedChanged, _optBasis_2.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            ' Display appropriate fields
            SetFormControls()
            ' Recalculate
            m_lReturn = CalculateTaxes()
        End If
    End Sub

    Private Sub txtBasisValue_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBasisValue.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        ' Recalculate
        m_lReturn = CalculateTaxes()
    End Sub

    Private Sub txtPercentage_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPercentage.Enter
        m_lReturn = m_oFormFields.GotFocus(txtPercentage)
    End Sub

    Private Sub txtPercentage_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPercentage.Leave
        m_lReturn = m_oFormFields.LostFocus(txtPercentage)
        ' If blank, set to 0
        If txtPercentage.Text.Trim() = "" Then
            m_lReturn = m_oFormFields.FormatControl(txtPercentage, 0)
        End If

        ' Recalculate
        m_lReturn = CalculateTaxes()

    End Sub

    Private Sub txtValue_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtValue.Enter
        m_lReturn = m_oFormFields.GotFocus(txtValue)
    End Sub

    Private Sub txtValue_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtValue.Leave
        m_lReturn = m_oFormFields.LostFocus(txtValue)
        ' If blank, set to 0
        If txtValue.Text.Trim() = "" Then
            m_lReturn = m_oFormFields.FormatControl(txtValue, 0)
        End If

        ' Recalculate
        m_lReturn = CalculateTaxes()
    End Sub


    Private Sub cmdDetailCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDetailCancel.Click
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        Me.Hide()
    End Sub

    Private Sub cmdDetailOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDetailOK.Click
        If Not m_bReadOnly Then
            Save()
        Else
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel
            Me.Hide()
        End If
    End Sub

    ' ***************************************************************** '
    ' Name: Form_QueryUnload
    '
    ' Description: Determines whether any actions need to take place
    '               before unload.
    ' History:
    '           Created : MEvans : 25-04-2005 : AUS005
    ' ***************************************************************** '
    Private Sub frmTaxDetail_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        eventArgs.Cancel = Cancel <> 0
    End Sub
    ' ***************************************************************** '
    ' Name: Form_Unload
    '
    ' Description: Destroys all object references
    '
    ' History:
    '           Created : MEvans : 25-04-2005 : AUS005
    ' ***************************************************************** '
    Private Sub frmTaxDetail_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

        Const sFunctionName As String = "Form_Unload"

        Try

            ' Terminate the business object

            m_oBusiness.Dispose()
            ' destroy object reference
            m_oBusiness = Nothing

        Catch excep As System.Exception



            m_bInterfaceError = True

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Form_Load
    '
    ' Parameters: N/A
    '
    ' Description: Sets up the form, populates controls, etc
    '
    ' History:
    '           Created : MEvans : 25-04-2005 : AUS005
    ' ***************************************************************** '

    Private Sub frmTaxDetail_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Const sFunctionName As String = "Form_Load"

        Try

            ' set up interface
            m_lReturn = SetUpForm()
            Dim sTempOptionValue As String = ""
            iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTEnableDecimalsSuppression, v_vBranch:=1, r_vUnderwriting:=sTempOptionValue)
            If Trim(sTempOptionValue) <> "" AndAlso Trim(sTempOptionValue) = "1" Then
                IsSuppressDecimalValues = True
            End If


        Catch excep As System.Exception



            m_bInterfaceError = True

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: SetUpForm
    '
    ' Parameters: n/a
    '
    ' Description: call all routines required to set up the form
    '
    ' History:
    '           Created : MEvans : 25-04-2005 : AUS005
    ' ***************************************************************** '
    Public Function SetUpForm() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetUpForm"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = SetFieldValidation()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetFieldValidation Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = PopulateForm()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateForm Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


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

    ' ***************************************************************** '
    ' Name: Save
    '
    ' Parameters: n/a
    '
    ' Description: saves the on screen details back to the tax array
    '
    ' History:
    '           Created : MEvans : 25-04-2005 : AUS005
    ' ***************************************************************** '
    Public Function Save() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Save"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vOrigRITaxes As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            vOrigRITaxes = VB6.CopyArray(m_vRITax)

            ' Calculation basis
            For lCount As Integer = optBasis.GetLowerBound(0) To optBasis.GetUpperBound(0)
                If optBasis(lCount).Checked Then
                    m_vRITax(ACRCalcBasis, m_lSelectedItem) = CStr(lCount)
                    Exit For
                End If
            Next lCount

            ' Apply Tax By (RC)
            For lCount As Integer = optApplyTax.GetLowerBound(0) To optApplyTax.GetUpperBound(0)
                If optApplyTax(lCount).Checked Then
                    m_vRITax(ACRApplyTaxBy, m_lSelectedItem) = CStr(lCount)
                    Exit For
                End If
            Next lCount

            ' Is value
            m_vRITax(ACRIsValue, m_lSelectedItem) = CStr(chkIsValue.CheckState)

            ' Rate
            If chkIsValue.CheckState = CheckState.Checked Then

                m_vRITax(ACRTaxRate, m_lSelectedItem) = CStr(CDec(m_oFormFields.UnformatControl(txtValue)))
            Else

                m_vRITax(ACRTaxRate, m_lSelectedItem) = CStr(CDec(m_oFormFields.UnformatControl(txtPercentage)))
            End If

            ' Basis value

            m_vRITax(ACRBasisValue, m_lSelectedItem) = ToSafeString(ToSafeDecimal(txtBasisValue.Text))

            ' Is rounded
            m_vRITax(ACRIsSIRounded, m_lSelectedItem) = CStr(chkRounded.CheckState)

            ' Allow tax credits?
            m_vRITax(ACRAllowTaxCredit, m_lSelectedItem) = CStr(chkAllowCredit.CheckState)

            ' Tax value
            m_vRITax(ACRTaxValue, m_lSelectedItem) = CDec(txtTaxValue.Text)
            'Instalment
            m_vRITax(ACRIsAppliedToClnt, m_lSelectedItem) = CStr(chkNotApplied.CheckState)
            m_vRITax(ACRIncludeIns, m_lSelectedItem) = CStr(chkIncludeIns.CheckState)
            m_vRITax(ACRSpread, m_lSelectedItem) = CStr(chkSpread.CheckState)

            ' If value is zero mark tax as deleted
            'If txtTaxValue.Text = 0 Then
            'm_vRITax(ACRIsDeleted, m_lSelectedItem) = CStr(1)
            'End If


            lReturn = CType(CheckIsDataChanged(v_vOrigRITaxes:=vOrigRITaxes, v_vRITaxes:=m_vRITax, r_bDataHasChanged:=m_bDataHasChanged), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CheckIsDataChanged Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If m_bDataHasChanged Then
                ' Data is manually changed
                m_vRITax(ACRIsManuallyChanged, m_lSelectedItem) = CStr(1)
            End If

            m_lStatus = gPMConstants.PMEReturnCode.PMOK


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' hide the form
            Me.Hide()

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PopulateForm
    '
    ' Parameters: n/a
    '
    ' Description: populates the tax details form with details for the
    '               selected item from the tax array
    '
    ' History:
    '           Created : MEvans : 25-04-2005 : AUS005
    ' ***************************************************************** '
    Public Function PopulateForm() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateForm"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_bRefresh = False

            ' Summary fields
            txtTaxBand.Text = gPMFunctions.NullToString(CStr(m_vRITax(ACRDescription, m_lSelectedItem)))

            m_lReturn = m_oFormFields.FormatControl(txtSumInsured, m_vRITax(ACRSumInsured, m_lSelectedItem))
            m_lReturn = m_oFormFields.FormatControl(txtSumInsuredChange, m_vRITax(ACRSumInsuredChange, m_lSelectedItem))
            m_lReturn = m_oFormFields.FormatControl(txtOriginalSumInsured, m_vRITax(ACROriginalSumInsured, m_lSelectedItem))
            m_lReturn = m_oFormFields.FormatControl(txtPremium, m_vRITax(ACRPremium, m_lSelectedItem))
            m_lReturn = m_oFormFields.FormatControl(txtRunningTotal, m_vRITax(ACRRunningTotal, m_lSelectedItem))

            ' Precentage/Rate/Value
            m_lReturn = m_oFormFields.FormatControl(txtPercentage, m_vRITax(ACRTaxRate, m_lSelectedItem))
            'Removed the formatting as it is rounding tax rate to upto 2 decimal , which affects the tax value change on screen
            txtValue.Text = m_vRITax(ACRTaxRate, m_lSelectedItem)
            If Not m_bReadOnly Then
                ' Calculation
                optBasis(CInt(m_vRITax(ACRCalcBasis, m_lSelectedItem))).Checked = True
                'Apply Tax by (RC)
                optApplyTax(gPMFunctions.ToSafeInteger(CStr(m_vRITax(ACRApplyTaxBy, m_lSelectedItem)))).Checked = True
                ' Basis value
                'Removed the formatting as it is rounding the basis value to 0 decimal places and hence marks it for data change
                txtBasisValue.Text = m_vRITax(ACRBasisValue, m_lSelectedItem)
            End If

            ' Is Value?
            chkIsValue.CheckState = Math.Abs(CInt(gPMFunctions.ToSafeBoolean(CStr(m_vRITax(ACRIsValue, m_lSelectedItem)))))

            ' Is rounded?
            chkRounded.CheckState = Math.Abs(CInt(gPMFunctions.ToSafeBoolean(CStr(m_vRITax(ACRIsSIRounded, m_lSelectedItem)))))
            ' Allow tax credit?
            chkAllowCredit.CheckState = Math.Abs(CInt(gPMFunctions.ToSafeBoolean(CStr(m_vRITax(ACRAllowTaxCredit, m_lSelectedItem)))))

            ' Filters
            txtCountry.Text = gPMFunctions.NullToString(CStr(m_vRITax(ACRCountry, m_lSelectedItem)))
            txtState.Text = gPMFunctions.NullToString(CStr(m_vRITax(ACRState, m_lSelectedItem)))
            txtCOB.Text = gPMFunctions.NullToString(CStr(m_vRITax(ACRClassOfBusiness, m_lSelectedItem)))
            'BPIS- Partial instalment
            chkNotApplied.CheckState = Math.Abs(CInt(gPMFunctions.ToSafeBoolean(CStr(m_vRITax(ACRIsAppliedToClnt, m_lSelectedItem)))))
            chkIncludeIns.CheckState = Math.Abs(CInt(gPMFunctions.ToSafeBoolean(CStr(m_vRITax(ACRIncludeIns, m_lSelectedItem)))))
            chkSpread.CheckState = Math.Abs(CInt(gPMFunctions.ToSafeBoolean(CStr(m_vRITax(ACRSpread, m_lSelectedItem)))))
            ' Set tax value
            m_lReturn = m_oFormFields.FormatControl(txtTaxValue, m_vRITax(ACRTaxValue, m_lSelectedItem))


            ' Set visible states
            SetFormControls()

            ' Set enabled states
            optBasis(0).Enabled = (Not m_bReadOnly)
            optBasis(1).Enabled = (Not m_bReadOnly)
            optBasis(2).Enabled = (Not m_bReadOnly)
            optBasis(3).Enabled = (Not m_bReadOnly)
            chkIsValue.Enabled = (Not m_bReadOnly)
            txtValue.Enabled = (Not m_bReadOnly)
            txtPercentage.Enabled = (Not m_bReadOnly)
            txtBasisValue.Enabled = (Not m_bReadOnly)
            chkRounded.Enabled = (Not m_bReadOnly)
            chkAllowCredit.Enabled = (Not m_bReadOnly)
            chkNotApplied.Enabled = (Not m_bReadOnly)
            If chkNotApplied.CheckState = CheckState.Checked Then
                chkIncludeIns.Enabled = False
                chkSpread.Enabled = False
            Else
                chkIncludeIns.Enabled = (Not m_bReadOnly)
                chkSpread.Enabled = (Not m_bReadOnly)
            End If
            '(RC)
            optApplyTax(0).Enabled = (Not m_bReadOnly)
            optApplyTax(1).Enabled = (Not m_bReadOnly)
            optApplyTax(2).Enabled = (Not m_bReadOnly)


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            m_bRefresh = False

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Parameters: n/a
    '
    ' Description: applies any form validation to the relevant fields
    '
    ' History:
    '           Created : MEvans : 25-04-2005 : AUS005
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetFieldValidation"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oFormFields Is Nothing Then
                m_oFormFields = New iPMFormControl.FormFields()
            End If

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSumInsured, lFieldType:=gPMConstants.PMEDataType.PMDecimal, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lCurrencyID:=0, lDecimalPlaces:=0, lGridColumn:=0, lMandatory:=0)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddFormField Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSumInsuredChange, lFieldType:=gPMConstants.PMEDataType.PMDecimal, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lCurrencyID:=0, lDecimalPlaces:=0, lGridColumn:=0, lMandatory:=0)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddFormField Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOriginalSumInsured, lFieldType:=gPMConstants.PMEDataType.PMDecimal, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lCurrencyID:=0, lDecimalPlaces:=0, lGridColumn:=0, lMandatory:=0)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddFormField Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPremium, lFieldType:=gPMConstants.PMEDataType.PMDecimal, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lCurrencyID:=0, lDecimalPlaces:=0, lGridColumn:=0, lMandatory:=0)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddFormField Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtRunningTotal, lFieldType:=gPMConstants.PMEDataType.PMDecimal, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lCurrencyID:=0, lDecimalPlaces:=0, lGridColumn:=0, lMandatory:=0)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddFormField Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtValue, lFieldType:=gPMConstants.PMEDataType.PMDecimal, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddFormField Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPercentage, lFieldType:=gPMConstants.PMEDataType.PMDecimal, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory, lDecimalPlaces:=-5)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddFormField Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtBasisValue, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddFormField Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtTaxValue, lFieldType:=gPMConstants.PMEDataType.PMDecimal, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddFormField Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


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

    ' ***************************************************************** '
    ' Name: SetFormControls
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-04-2005 : AUS005
    ' ***************************************************************** '
    Public Function SetFormControls() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetFormControls"

        Dim lReturn, lBasis As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the calculation basis
            For lCount As Integer = optBasis.GetLowerBound(0) To optBasis.GetUpperBound(0)
                If optBasis(lCount).Checked Then
                    lBasis = lCount
                    Exit For
                End If
            Next

            ' Set control properties
            txtPercentage.Visible = (chkIsValue.CheckState <> CheckState.Checked)
            txtValue.Visible = (chkIsValue.CheckState = CheckState.Checked)

            lblPer.Visible = (lBasis = 1 Or lBasis = 2) And (chkIsValue.CheckState = CheckState.Checked)
            txtBasisValue.Visible = lblPer.Visible
            lblOfSI.Visible = lblPer.Visible
            chkRounded.Visible = lblPer.Visible

            ' Set appropriate rate caption
            If chkIsValue.CheckState Then
                lblPercentage.Text = "Value:"
            Else
                lblPercentage.Text = "Rate:"
            End If


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

    ' ***************************************************************** '
    ' Name: CalculateTaxes
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-04-2005 : AUS005
    ' ***************************************************************** '
    Private Function CalculateTaxes() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CalculateTaxes"

        Dim lReturn, lCalcBasis As Integer
        Dim bIsValue As Boolean
        Dim dPercentage As Double
        Dim cFixedRate, cBasisValue As Decimal
        Dim bIsRounded, bAllowTaxCredit As Boolean
        Dim cTaxValue As Decimal

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If m_bRefresh Then
                Return result
            End If

            ' Get new values
            For lCalcBasis = optBasis.GetLowerBound(0) To optBasis.GetUpperBound(0)
                If optBasis(lCalcBasis).Checked Then
                    Exit For
                End If
            Next lCalcBasis

            bIsValue = (chkIsValue.CheckState = CheckState.Checked)

            dPercentage = CDbl(m_oFormFields.UnformatControl(txtPercentage))

            cFixedRate = CDec(m_oFormFields.UnformatControl(txtValue))

            cBasisValue = ToSafeDecimal(txtBasisValue.Text)
            bIsRounded = (chkRounded.CheckState = CheckState.Checked)
            bAllowTaxCredit = (chkAllowCredit.CheckState = CheckState.Checked)

            ' Calculate through business object

            m_lReturn = m_oBusiness.CalculateTax(vPremium:=m_vRITax(ACRPremium, m_lSelectedItem), vSumInsured:=m_vRITax(ACRSumInsured, m_lSelectedItem), vSumInsuredChange:=m_vRITax(ACRSumInsuredChange, m_lSelectedItem), vRunningTotal:=m_vRITax(ACRRunningTotal, m_lSelectedItem), vCalcBasis:=lCalcBasis, vIsValue:=bIsValue, vPercentage:=dPercentage, vFixedRate:=cFixedRate, vBasisValue:=cBasisValue, vIsRounded:=bIsRounded, vAllowTaxCredit:=bAllowTaxCredit, rTaxValue:=cTaxValue)

            ' Store and show new value
            m_cTaxValue = cTaxValue
            m_lReturn = m_oFormFields.FormatControl(txtTaxValue, cTaxValue)



        Catch ex As Exception

            ' If we failed default tax to zero
            m_lReturn = m_oFormFields.FormatControl(txtTaxValue, 0)

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: CheckIsDataChanged
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function CheckIsDataChanged(ByVal v_vOrigRITaxes(,) As Object, ByVal v_vRITaxes(,) As Object, ByRef r_bDataHasChanged As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CheckIsDataChanged"

        Dim lReturn, llBoundNoOfItems, lUBoundNoOfItems, lLBoundNoOfProps, lUBoundNoOfProps As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            r_bDataHasChanged = False

            ' NB: this routines assumes both array have same number of rows and props

            llBoundNoOfItems = v_vOrigRITaxes.GetLowerBound(1)
            lUBoundNoOfItems = v_vOrigRITaxes.GetUpperBound(1)
            lLBoundNoOfProps = v_vOrigRITaxes.GetLowerBound(0)
            lUBoundNoOfProps = v_vOrigRITaxes.GetUpperBound(0)

            For lItem As Integer = llBoundNoOfItems To lUBoundNoOfItems

                For lProp As Integer = lLBoundNoOfProps To lUBoundNoOfProps



                    If Not v_vOrigRITaxes(lProp, lItem).Equals(v_vRITaxes(lProp, lItem)) Then
                        r_bDataHasChanged = True
                        Exit For
                    End If

                Next

                If r_bDataHasChanged Then
                    Exit For
                End If

            Next


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


    ' ***************************************************************** '
    ' Name: PopulateSingleRiskTax
    '
    ' Parameters: n/a
    '
    ' Description: populates the tax details form with details for the
    '               selected item from the tax array
    '
    ' History:
    '           Created : Rajesh Choudhary : 26-10-2006 : AUS015
    ' ***************************************************************** '
    Public Function PopulateSingleRiskTax(ByVal v_vRITax(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateSingleRiskTax"

        Dim lReturn As Integer
        Dim lSelectedItem As Integer = 0

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_bRefresh = True

            ' Summary fields

            txtTaxBand.Text = gPMFunctions.NullToString(CStr(v_vRITax(ACRDescription, lSelectedItem)))

            m_lReturn = m_oFormFields.FormatControl(txtSumInsured, v_vRITax(ACRSumInsured, lSelectedItem))
            m_lReturn = m_oFormFields.FormatControl(txtSumInsuredChange, v_vRITax(ACRSumInsuredChange, lSelectedItem))
            m_lReturn = m_oFormFields.FormatControl(txtOriginalSumInsured, v_vRITax(ACROriginalSumInsured, lSelectedItem))
            m_lReturn = m_oFormFields.FormatControl(txtPremium, v_vRITax(ACRPremium, lSelectedItem))
            m_lReturn = m_oFormFields.FormatControl(txtRunningTotal, v_vRITax(ACRRunningTotal, lSelectedItem))

            ' Calculation

            optBasis(CInt(v_vRITax(ACRCalcBasis, lSelectedItem))).Checked = True

            'Apply Tax by (RC)

            optApplyTax(CInt(v_vRITax(ACRApplyTaxBy, lSelectedItem))).Checked = True

            ' Is Value?

            chkIsValue.CheckState = Math.Abs(CInt(gPMFunctions.ToSafeBoolean(CStr(v_vRITax(ACRIsValue, lSelectedItem)))))
            ' Precentage/Rate/Value
            m_lReturn = m_oFormFields.FormatControl(txtPercentage, v_vRITax(ACRTaxRate, lSelectedItem))
            m_lReturn = m_oFormFields.FormatControl(txtValue, v_vRITax(ACRTaxRate, lSelectedItem))
            ' Basis value
            m_lReturn = m_oFormFields.FormatControl(txtBasisValue, v_vRITax(ACRBasisValue, lSelectedItem))
            ' Is rounded?

            chkRounded.CheckState = Math.Abs(CInt(gPMFunctions.ToSafeBoolean(CStr(v_vRITax(ACRIsSIRounded, lSelectedItem)))))
            ' Allow tax credit?

            chkAllowCredit.CheckState = Math.Abs(CInt(gPMFunctions.ToSafeBoolean(CStr(v_vRITax(ACRAllowTaxCredit, lSelectedItem)))))

            ' Filters

            txtCountry.Text = gPMFunctions.NullToString(CStr(v_vRITax(ACRCountry, lSelectedItem)))

            txtState.Text = gPMFunctions.NullToString(CStr(v_vRITax(ACRState, lSelectedItem)))

            txtCOB.Text = gPMFunctions.NullToString(CStr(v_vRITax(ACRClassOfBusiness, lSelectedItem)))
            'BPIS- Partial instalment

            chkNotApplied.CheckState = Math.Abs(CInt(gPMFunctions.ToSafeBoolean(CStr(v_vRITax(ACRIsAppliedToClnt, lSelectedItem)))))

            chkIncludeIns.CheckState = Math.Abs(CInt(gPMFunctions.ToSafeBoolean(CStr(v_vRITax(ACRIncludeIns, lSelectedItem)))))

            chkSpread.CheckState = Math.Abs(CInt(gPMFunctions.ToSafeBoolean(CStr(v_vRITax(ACRSpread, lSelectedItem)))))
            ' Set tax value
            m_lReturn = m_oFormFields.FormatControl(txtTaxValue, v_vRITax(ACRTaxValue, lSelectedItem))

            ' Set visible states
            SetFormControls()

            ' Set enabled states
            optBasis(0).Enabled = (Not m_bReadOnly)
            optBasis(1).Enabled = (Not m_bReadOnly)
            optBasis(2).Enabled = (Not m_bReadOnly)
            optBasis(3).Enabled = (Not m_bReadOnly)
            chkIsValue.Enabled = (Not m_bReadOnly)
            txtValue.Enabled = (Not m_bReadOnly)
            txtPercentage.Enabled = (Not m_bReadOnly)
            txtBasisValue.Enabled = (Not m_bReadOnly)
            chkRounded.Enabled = (Not m_bReadOnly)
            chkAllowCredit.Enabled = (Not m_bReadOnly)
            chkNotApplied.Enabled = (Not m_bReadOnly)
            chkIncludeIns.Enabled = (Not m_bReadOnly)
            chkSpread.Enabled = (Not m_bReadOnly)
            '(RC)
            optApplyTax(0).Enabled = (Not m_bReadOnly)
            optApplyTax(1).Enabled = (Not m_bReadOnly)
            optApplyTax(2).Enabled = (Not m_bReadOnly)


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            m_bRefresh = False

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function
    Private Sub txtValue_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtValue.KeyPress
        Select Case sender.name
            Case txtValue.Name
                If chkIsValue.Checked = True AndAlso IsSuppressDecimalValues Then
                    gPMFunctions.NumPress(sender, e)
                End If
        End Select
    End Sub

    
End Class
