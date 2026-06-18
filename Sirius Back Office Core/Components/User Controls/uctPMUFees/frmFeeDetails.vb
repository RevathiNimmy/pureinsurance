Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.Globalization
Imports System.IO
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Friend Partial Class frmFeeDetail
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
    Private m_oBusiness As Object
	Private m_lReturn As Integer
	Private m_bInterfaceError As Boolean
	'********************************
	
	' Declare an instance of the FormControl object
	Private m_oFormFields As iPMFormControl.FormFields
	
	Private m_vFeeDetails( ,  ) As Object
	Private m_lSelectedItem As Integer
	Private m_bReadOnly As Boolean
	Private m_bRefresh As Boolean
    Private m_bDataHasChanged As Boolean

    Private m_bApplyProrated As Boolean
    Private m_sMakeLivePaymentMethod As String
    Private m_sMakeLivePaymentTerm As String
    Private m_nInsuranceFileCnt As Integer
    Private m_nriskCnt As Integer
    Private m_dProRataRate As Double
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
    Public WriteOnly Property MakeLivePaymentMethod() As String
        Set(ByVal Value As String)
            m_sMakeLivePaymentMethod = Value
        End Set
    End Property

    Public WriteOnly Property MakeLivePaymentTerm() As String
        Set(ByVal Value As String)
            m_sMakeLivePaymentTerm = Value
        End Set
    End Property

    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_nInsuranceFileCnt = Value
        End Set
    End Property

    Public WriteOnly Property RiskCnt() As Integer
        Set(ByVal Value As Integer)
            m_nriskCnt = Value
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
    Public Property FeeDetails() As Object
        Get
            Return VB6.CopyArray(m_vFeeDetails)
        End Get
        Set(ByVal Value As Object)
            m_vFeeDetails = Value
        End Set
    End Property

    Public WriteOnly Property SelectedItem() As Integer
        Set(ByVal Value As Integer)
            m_lSelectedItem = Value
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

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        Me.Hide()
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        If Validate_Renamed() Then
            Save()
        End If
    End Sub


    ' ***************************************************************** '
    ' Name: Form_Initialize
    '
    ' Description:
    '
    ' History:
    ' Created : Inder : 2-07-2014 : 
    ' ***************************************************************** '
    Private Sub Form_Initialize_Renamed()

        iPMFunc.ShowFormInTaskBar_Attach()

        Const sFunction As String = "Form_Initialize"

        Try

            ' initialise form error indicator
            m_bError = False

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            If g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPartyFee.UBusiness", vInstanceManager:=gPMConstants.PMGetViaClientManager) <> gPMConstants.PMEReturnCode.PMTrue Then
                m_oBusiness = temp_m_oBusiness

                ' interface error shut down
                m_bError = False

                ' Failed to get an instance of the business object.
                gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunction & "Failed to create business object", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunction, excep:= New Exception(Information.Err().Description))

                ' reset mouse pointer
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Else
                m_oBusiness = temp_m_oBusiness
            End If

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

        Catch excep As System.Exception

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunction & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunction, excep:=excep)

            ' reset mouse pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Exit Sub

        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: Form_QueryUnload
    '
    ' Description: Determines whether any actions need to take place
    '               before unload.
    ' History:
    '           Created : MEvans : 25-04-2005 : AUS005
    ' ***************************************************************** '
    Private Sub frmFeeDetail_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
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
    Private Sub frmFeeDetail_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

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

    Private Sub frmFeeDetail_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Const sFunctionName As String = "Form_Load"

        Try

            ' set up interface
            m_lReturn = SetUpForm()
            'Get the Decimal Suppression flag
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

        lReturn = PopulateForm()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "PopulateForm Failed", gPMConstants.PMELogLevel.PMLogError)
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
        Dim lCount As Integer
        Dim vOrigFeeDetails As Object
        Dim crFeePercentage, crFeeAmount As Decimal

        Try

        result = gPMConstants.PMEReturnCode.PMTrue

        ' save the original details

        vOrigFeeDetails = VB6.CopyArray(m_vFeeDetails)

        ' update array with new values
        If OptAmount.Checked Then
            crFeeAmount = CDec(txtRate.Text)
            crFeePercentage = 0
        Else
            crFeePercentage = CDec(txtRate.Text)
            crFeeAmount = 0
        End If

        m_vFeeDetails(kFeeItemFeePercentage, m_lSelectedItem) = crFeePercentage
        m_vFeeDetails(kFeeItemFeeAmount, m_lSelectedItem) = crFeeAmount

        ' check if any actual changes have been made

        lReturn = CType(CheckIsDataChanged(v_vOrigFeeDetails:=vOrigFeeDetails, v_vFeeDetails:=m_vFeeDetails, r_bDataHasChanged:=m_bDataHasChanged), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "CheckIsDataChanged Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lStatus = gPMConstants.PMEReturnCode.PMOK


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally

        ' hide the form
        Me.Hide()

'        Return result
'        Resume
'        Return result
        End Try
        Return result
    End Function
    ''' <summary>
    ''' PopulateForm
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PopulateForm() As Integer

        Dim nResult As Integer
        Const kMethodName As String = "PopulateForm"

        Dim nReturn As Integer
        Dim crRate As Decimal
        Dim crFeePercentage As Decimal
        Dim crFeeAmount As Decimal

        Try

            nResult = PMEReturnCode.PMTrue

            crFeePercentage = CDec(m_vFeeDetails(kFeeItemFeePercentage, m_lSelectedItem))
            crFeeAmount = CDec(m_vFeeDetails(kFeeItemFeeAmount, m_lSelectedItem))

            If CInt(m_vFeeDetails(kFeeItemApplyProRated, m_lSelectedItem)) = 1 Then
                m_bApplyProrated = True
            Else
                m_bApplyProrated = False
            End If
            lblProrataAmount.Visible = False
            txtProRateAmount.Visible = False

            If crFeeAmount <> 0 OrElse (crFeePercentage = 0 AndAlso m_bApplyProrated = True) Then
                nReturn = m_oBusiness.GetProRataRate(m_nInsuranceFileCnt, m_nriskCnt, m_dProRataRate)
                txtRate.Text = StringsHelper.Format(crFeeAmount, "0.00")
                OptAmount.Checked = True
                OptPercentage.Checked = False

                If m_bApplyProrated Then
                    lblProrataAmount.Visible = True
                    txtProRateAmount.Visible = True
                    txtProRateAmount.Text = Math.Round(ToSafeDouble(m_dProRataRate) * ToSafeDouble(txtRate.Text), 4)
                End If
            Else
                txtRate.Text = StringsHelper.Format(crFeePercentage, "0.00")
                OptAmount.Checked = False
                OptPercentage.Checked = True
            End If

            lblCurrencyValue.Text = CStr(m_vFeeDetails(kFeeItemCurrencyDesc, m_lSelectedItem))
            lblTaxGroupValue.Text = CStr(m_vFeeDetails(kFeeItemTaxGroupDescription, m_lSelectedItem))

            If m_bReadOnly Then
                OptAmount.Enabled = False
                OptPercentage.Enabled = False
                txtRate.Enabled = False
            End If
            Return nResult
        Catch ex As Exception
            nResult = PMEReturnCode.PMError
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
            Return nResult
        Finally
            m_bRefresh = False
        End Try
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
    Public Function CheckIsDataChanged(ByVal v_vOrigFeeDetails(,) As Object, ByVal v_vFeeDetails As Object, ByRef r_bDataHasChanged As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CheckIsDataChanged"

        Dim lReturn, llBoundNoOfItems, lUBoundNoOfItems, lLBoundNoOfProps, lUBoundNoOfProps, lProp, lItem As Integer

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        r_bDataHasChanged = False


        If StringsHelper.Format(m_vFeeDetails(kFeeItemFeePercentage, m_lSelectedItem), "0.00") <> StringsHelper.Format(v_vOrigFeeDetails(kFeeItemFeePercentage, m_lSelectedItem), "0.00") Then
            r_bDataHasChanged = True
        End If


        If StringsHelper.Format(m_vFeeDetails(kFeeItemFeeAmount, m_lSelectedItem), "0.00") <> StringsHelper.Format(v_vOrigFeeDetails(kFeeItemFeeAmount, m_lSelectedItem), "0.00") Then
            r_bDataHasChanged = True
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

    Private isInitializingComponent As Boolean
    Private Sub OptAmount_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptAmount.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            SetupFeeAmount()
        End If
    End Sub

    Private Sub OptPercentage_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptPercentage.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            SetupFeeAmount()
        End If
    End Sub
    ''' <summary>
    ''' SetupFeeAmount
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetupFeeAmount() As Integer

        Dim nResult As Integer
        Const kMethodName As String = "SetupFeeAmount"

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            If OptPercentage.Checked Then
                lblCurrencyValue.Visible = False
                lblCurrency.Visible = False
                lblProrataAmount.Visible = False
                txtProRateAmount.Visible = False
                txtProRateAmount.Text = 0
            ElseIf OptAmount.Checked Then
                lblCurrencyValue.Visible = True
                lblCurrency.Visible = True
                If m_bApplyProrated Then
                    lblProrataAmount.Visible = True
                    txtProRateAmount.Visible = True
                    nResult = m_oBusiness.GetProRataRate(m_nInsuranceFileCnt, m_nriskCnt, m_dProRataRate)
                    If ToSafeDouble(txtRate.Text) <> 0 Then
                        txtProRateAmount.Text = Math.Round(ToSafeDouble(txtRate.Text) * ToSafeDouble(m_dProRataRate), 4)
                    Else
                        txtProRateAmount.Text = 0
                    End If
                Else
                    txtProRateAmount.Text = txtRate.Text
                End If
            End If

            Return nResult
        Catch ex As Exception
            nResult = PMEReturnCode.PMError
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            Return nResult
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: Validate
    '
    ' Parameters: n/a
    '
    ' Description: Validates on screen fields
    '
    ' History:
    '           Created : HG010905 PN23633
    ' ***************************************************************** '
    Public Function Validate_Renamed() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Validate"

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        If Strings.Len(txtRate.Text) = 0 Then
            MessageBox.Show("A rate must be entered", "Validate Rate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            txtRate.Focus()
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Dim dbNumericTemp As Double
        Dim cRateTest As Decimal
        If Not Double.TryParse(txtRate.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            MessageBox.Show("The rate can only be numeric", "Validate Rate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            txtRate.Focus()
            Return gPMConstants.PMEReturnCode.PMFalse
        Else

            ' Try to convert the value in rate field to currency data type.
            cRateTest = gPMFunctions.ToSafeCurrency(txtRate.Text, 0)

            ' If user has not input 0 but after conversion we get 0
            ' it means a data type conversion error has occured. Also
            ' the rate should be valid for SQL numeric(7,4) data type.
            If OptPercentage.Checked = True Then
                If (cRateTest = 0 Or (cRateTest > 100)) Then
                    MessageBox.Show("The value entered in the rate field is not in valid range. Please re-enter.", "Validate Rate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    txtRate.Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                ElseIf cRateTest < 0 Then
                    MessageBox.Show("A percentage rate must be between (0-100).", "Validate Rate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    txtRate.Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                ' the rate should be valid for SQL numeric(19,4) data type
                ' but stats supports handling upto (12,4) for fee calculation
            ElseIf OptAmount.Checked = True Then
                'If (cRateTest = 0 Or (cRateTest > 100000000.0)) Then
                ' MessageBox.Show("The value entered in the rate field is not in valid range. Please re-enter.", "Validate Rate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                ' txtRate.Focus()
                ' Return gPMConstants.PMEReturnCode.PMFalse
                'End If
            End If
		Return result

        End If


        Catch ex As Exception

        result = gPMConstants.PMEReturnCode.PMFalse

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

    Private Sub txtRate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtRate.KeyPress
        Select Case sender.name
            Case txtRate.Name
                If OptAmount.Checked = True AndAlso IsSuppressDecimalValues Then
                    gPMFunctions.NumPress(sender, e)
                End If
        End Select
    End Sub
    ''' <summary>
    ''' txtRate_TextChanged
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtRate_TextChanged(sender As Object, e As EventArgs) Handles txtRate.TextChanged
        If OptAmount.Checked Then
            If Trim(txtRate.Text) = "" OrElse ToSafeDouble(txtRate.Text) = 0 Then
                txtProRateAmount.Text = "0.00"
            Else
                txtProRateAmount.Text = StringsHelper.Format(ToSafeDouble(m_dProRataRate, 1) * ToSafeDouble(txtRate.Text), "0.00")
            End If
        End If
    End Sub
End Class
