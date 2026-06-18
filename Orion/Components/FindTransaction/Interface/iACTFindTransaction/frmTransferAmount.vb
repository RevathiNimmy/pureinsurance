Option Strict Off
Option Explicit On
Imports System
Imports System.Globalization
Imports System.Windows.Forms

Imports SharedFiles
Friend Partial Class frmTransferAmount
	Inherits System.Windows.Forms.Form
	'*************************************************************************
	' Form Name:    frmTransferAmount
	' Description:  Popup form to allow user to input an amount to transfer
	' History:      TR140203 - Created for TS220 Manage Debtor Accounts
	'*************************************************************************
    'replaced iPMFunc.GetResData with GetResData in the whole document

	'=================
	'Private Variables
	'=================
	Private m_enStatus As gPMConstants.PMEReturnCode
	Private m_crOriginalAmount As Decimal
	Private m_oFormfields As iPMFormControl.FormFields
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	'=================
	'Public Properties
	'=================
	'Read/Write
	Public Property Amount() As Decimal
		Get
			'TR - Get User Input
			Dim dbNumericTemp As Double
			If Double.TryParse(txtCurrencyAmount.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
				Return CDec(txtCurrencyAmount.Text)
			Else
				Return 0
			End If
		End Get
		Set(ByVal Value As Decimal)
			'TR - Display passed in data
			txtCurrencyAmount.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, Value)
			m_crOriginalAmount = Value
		End Set
	End Property
	'Write Only
	Public WriteOnly Property PromptMessage() As String
		Set(ByVal Value As String)
			'TR - Display passed in data
			lblTransferAmount.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatString, Value)
		End Set
	End Property
	Public WriteOnly Property ReadOnly_Renamed() As Boolean
		Set(ByVal Value As Boolean)
			'TR - Enable/Disable fields
			txtCurrencyAmount.ReadOnly = Value
		End Set
	End Property
	'Read Only
	Public ReadOnly Property Status() As gPMConstants.PMEReturnCode
		Get
			Return m_enStatus
		End Get
	End Property
	
	'===========
	'Form Events
	'===========
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		'TR - Assume failure
		m_enStatus = gPMConstants.PMEReturnCode.PMCancel
		'TR - Validate the amount
		m_lReturn = CheckNewAmount()
		'TR - Only Hide form if the AMount is ok
		If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
			m_enStatus = gPMConstants.PMEReturnCode.PMOk
			'TR - Hide the form to give calling client access to data
			Me.Hide()
		End If
	End Sub
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		'TR - Just set the status to User Canelled
		m_enStatus = gPMConstants.PMEReturnCode.PMCancel
		'TR - Hide the form to give calling client access to data
		Me.Hide()
	End Sub

	Private Sub frmTransferAmount_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		iPMForms.SetFieldValidation(Me, m_oFormfields)
		iPMForms.DisplayCaptions(Me)
	End Sub
	Private Function CheckNewAmount() As Integer
		
		Dim result As Integer = 0
		Dim sMessage As String = ""
		
		'TR - Assume Success
		result = gPMConstants.PMEReturnCode.PMTrue
		
		'TR - Make sure that the user has inputted a numeric value
		Dim dbNumericTemp As Double
		If Double.TryParse(txtCurrencyAmount.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
			'TR - Make sure that the numeric value is equal or less than
			'the original
			If m_crOriginalAmount <> 0 Then
				If Math.Abs(CDec(txtCurrencyAmount.Text)) > Math.Abs(m_crOriginalAmount) Then
					result = gPMConstants.PMEReturnCode.PMFalse

                    sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACTransferAmountTooMuch, gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
					MessageBox.Show(sMessage, Application.ProductName)
				End If
			End If
		Else
			result = gPMConstants.PMEReturnCode.PMFalse

            sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACTransferAmountNotNumeric, gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			MessageBox.Show(sMessage, Application.ProductName)
		End If
		Return result
	End Function
End Class