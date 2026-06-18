Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmAuthorities
	Inherits System.Windows.Forms.Form
	
	Private Const ACClass As String = "frmAuthorities"
	
	Private m_iUserID As Integer
	Private m_sUserName As String = ""
	
	Private m_iUnrestrictedEnquiry As CheckState
	Private m_iUnrestrictedUpdate As CheckState
	Private m_iOverrideDate As Integer
	Private m_iOverrideRate As Integer
	Private m_iOverridePrePolicyDate As Integer
	Private m_iOverridePrePolicyRate As Integer
	
	Private m_iHasRefundAuthority As Integer
	Private m_iHasTransferAuthority As Integer
	
	Private m_iHasPaymentsAuthority As Integer
	Private m_iPaymentsCurrencyID As Integer
	Private m_dPaymentsAmount As Double
	
	Private m_iCanWriteOff As CheckState
	Private m_iWriteOffCurrencyID As Integer
	Private m_dWriteOffAmount As Double
	Private m_iHasTransWriteOffAuthority As Integer
	Private m_iTransWriteOffCurrencyID As Integer
	Private m_dTransWriteOffAmount As Double
	
	Private m_iHasClaimPaymentsAuthority As Integer
	Private m_iClaimPaymentsCurrencyID As Integer
	Private m_dClaimPaymentsAmount As Double
	
	' FeeDiscount
	Private m_vFeeDiscount As Object
	
	Private m_lReturn As gPMConstants.PMEReturnCode
	Private m_iStatus As gPMConstants.PMEReturnCode
	
	' Declare an instance of the FormControl object
	Private m_oFormFields As iPMFormControl.FormFields
	
	
	Public Property HasTransWriteOffAuthority() As Integer
		Get
			' AMB 05/02/2003 - Added for IAG 220 Manage Debtors development
			
			Return m_iHasTransWriteOffAuthority
			
		End Get
		Set(ByVal Value As Integer)
			' AMB 05/02/2003 - Added for IAG 220 Manage Debtors development
			
			m_iHasTransWriteOffAuthority = Value
			
		End Set
	End Property
	
	Public Property TransWriteOffAmount() As Double
		Get
			' AMB 05/02/2003 - Added for IAG 220 Manage Debtors development
			
			Return m_dTransWriteOffAmount
			
		End Get
		Set(ByVal Value As Double)
			' AMB 05/02/2003 - Added for IAG 220 Manage Debtors development
			
			m_dTransWriteOffAmount = Value
			
		End Set
	End Property
	
	Public Property HasRefundAuthority() As Integer
		Get
			' AMB 05/02/2003 - Added for IAG 220 Manage Debtors development
			
			Return m_iHasRefundAuthority
			
		End Get
		Set(ByVal Value As Integer)
			' AMB 05/02/2003 - Added for IAG 220 Manage Debtors development
			
			m_iHasRefundAuthority = Value
			
		End Set
	End Property
	
	Public Property HasTransferAuthority() As Integer
		Get
			' AMB 05/02/2003 - Added for IAG 220 Manage Debtors development
			
			Return m_iHasTransferAuthority
			
		End Get
		Set(ByVal Value As Integer)
			' AMB 05/02/2003 - Added for IAG 220 Manage Debtors development
			
			m_iHasTransferAuthority = Value
			
		End Set
	End Property
	
	Public Property FeeDiscount() As Object
		Get
			Return m_vFeeDiscount
		End Get
		Set(ByVal Value As Object)


			m_vFeeDiscount = Value
		End Set
	End Property
	' START CHANGES - Changed By: AAB  - Changed On: 19-Nov-2003 08:34
	' added to support payment & Claim payment authorities and approval steps
	Public Property HasPaymentsAuthority() As Integer
		Get
			Return m_iHasPaymentsAuthority
		End Get
		Set(ByVal Value As Integer)
			m_iHasPaymentsAuthority = Value
		End Set
	End Property
	Public Property PaymentsAmount() As Double
		Get
			Return m_dPaymentsAmount
		End Get
		Set(ByVal Value As Double)
			m_dPaymentsAmount = Value
		End Set
	End Property
	Public Property HasClaimPaymentsAuthority() As Integer
		Get
			Return m_iHasClaimPaymentsAuthority
		End Get
		Set(ByVal Value As Integer)
			m_iHasClaimPaymentsAuthority = Value
		End Set
	End Property
	Public Property ClaimPaymentsAmount() As Double
		Get
			Return m_dClaimPaymentsAmount
		End Get
		Set(ByVal Value As Double)
			m_dClaimPaymentsAmount = Value
		End Set
	End Property
	Public Property OverrideDate() As Integer
		Get
			Return m_iOverrideDate
		End Get
		Set(ByVal Value As Integer)
			m_iOverrideDate = Value
		End Set
	End Property
	Public Property OverrideRate() As Integer
		Get
			Return m_iOverrideRate
		End Get
		Set(ByVal Value As Integer)
			m_iOverrideRate = Value
		End Set
	End Property
	Public Property OverridePrePolicyDate() As Integer
		Get
			Return m_iOverridePrePolicyDate
		End Get
		Set(ByVal Value As Integer)
			m_iOverridePrePolicyDate = Value
		End Set
	End Property
	Public Property OverridePrePolicyRate() As Integer
		Get
			Return m_iOverridePrePolicyRate
		End Get
		Set(ByVal Value As Integer)
			m_iOverridePrePolicyRate = Value
		End Set
	End Property
	Public Property PaymentsCurrencyID() As Integer
		Get
			Return m_iPaymentsCurrencyID
		End Get
		Set(ByVal Value As Integer)
			m_iPaymentsCurrencyID = Value
		End Set
	End Property
	Public Property WriteOffCurrencyID() As Integer
		Get
			Return m_iWriteOffCurrencyID
		End Get
		Set(ByVal Value As Integer)
			m_iWriteOffCurrencyID = Value
		End Set
	End Property
	Public Property TransWriteOffCurrencyID() As Integer
		Get
			Return m_iTransWriteOffCurrencyID
		End Get
		Set(ByVal Value As Integer)
			m_iTransWriteOffCurrencyID = Value
		End Set
	End Property
	Public Property ClaimPaymentsCurrencyID() As Integer
		Get
			Return m_iClaimPaymentsCurrencyID
		End Get
		Set(ByVal Value As Integer)
			m_iClaimPaymentsCurrencyID = Value
		End Set
	End Property
	
	Public Property Status() As Integer
		Get
			Return m_iStatus
		End Get
		Set(ByVal Value As Integer)
			m_iStatus = Value
		End Set
	End Property
	
	Public Property UserID() As Integer
		Get
			Return m_iUserID
		End Get
		Set(ByVal Value As Integer)
			m_iUserID = Value
		End Set
	End Property
	
	Public Property CanWriteOff() As Integer
		Get
			Return m_iCanWriteOff
		End Get
		Set(ByVal Value As Integer)
			m_iCanWriteOff = Value
		End Set
	End Property
	'eck120600
	Public Property UnrestrictedEnquiry() As Integer
		Get
			Return m_iUnrestrictedEnquiry
		End Get
		Set(ByVal Value As Integer)
			m_iUnrestrictedEnquiry = Value
		End Set
	End Property
	Public Property UnrestrictedUpdate() As Integer
		Get
			Return m_iUnrestrictedUpdate
		End Get
		Set(ByVal Value As Integer)
			m_iUnrestrictedUpdate = Value
		End Set
	End Property
	
	Public Property WriteOffAmount() As Double
		Get
			Return m_dWriteOffAmount
		End Get
		Set(ByVal Value As Double)
			m_dWriteOffAmount = Value
		End Set
	End Property
	
	Public Property UserName() As String
		Get
			Return m_sUserName
		End Get
		Set(ByVal Value As String)
			m_sUserName = Value
		End Set
	End Property
	
	' ***************************************************************** '
	' Name: SetFieldValidation
	'
	' Description: Sets the rules for validating fields.
	'
	' AMB 05/02/2003 - Added fields for IAG 220 Manage Debtors development
	' ***************************************************************** '
	Public Function SetFieldValidation() As Integer
		
		Dim result As Integer = 0
		Dim lDecimalPlaces As Integer
		
		Try 
			
			' PWF - Set decimals to 4 for 1.9 and 2 for 1.8 pending KB's suggestion
#If CODEBASE = 19 Then

			lDecimalPlaces = 4
#ElseIf CODEBASE = 18 Then
			'KB PN 3096 Change DP to 2
			'WE probably should look this up from the database
			'But we can add that when we do all the other corrections
			'relating to hard-coding currencies and decimal places
			lDecimalPlaces = 2
#Else

			'No CodeBase variable set
#End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtWriteOff, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDouble, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory, lDecimalPlaces:=lDecimalPlaces)
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFeeDiscount, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDouble, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory, lDecimalPlaces:=lDecimalPlaces)
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtTransWriteOff, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDouble, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory, lDecimalPlaces:=lDecimalPlaces)
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPayments, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDouble, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory, lDecimalPlaces:=lDecimalPlaces)
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtClaimPayments, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDouble, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory, lDecimalPlaces:=lDecimalPlaces)
			
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: DataToInterface
	'
	' Description:
	'
	' History: 14/02/2000 CTAF - Created.
	'
	' AMB 05/02/2003 - Added fields for IAG 220 Manage Debtors development
	' ***************************************************************** '
	Private Function DataToInterface() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' set the caption on the form
			Me.Text = Me.Text & m_sUserName
			
			' set write off properties
			If m_iCanWriteOff = CheckState.Checked Then
				Me.chkWriteOffs.CheckState = CheckState.Checked
			Else
				Me.chkWriteOffs.CheckState = CheckState.Unchecked
			End If
			
			' write off amount
			cboWriteOffsCurrency.CurrencyId = m_iWriteOffCurrencyID
			txtWriteOff.Text = CStr(WriteOffAmount)
			
			' Enable or disable the text box
			m_lReturn = CType(EnableWriteOff(), gPMConstants.PMEReturnCode)
			
			If m_iUnrestrictedEnquiry = CheckState.Checked Then
				Me.chkUnrestrictedEnquiry.CheckState = CheckState.Checked
			Else
				Me.chkUnrestrictedEnquiry.CheckState = CheckState.Unchecked
			End If
			If m_iUnrestrictedUpdate = CheckState.Checked Then
				Me.chkUnrestrictedUpdate.CheckState = CheckState.Checked
			Else
				Me.chkUnrestrictedUpdate.CheckState = CheckState.Unchecked
			End If
			If m_iOverrideDate = 1 Then
				chkOverrideDate.CheckState = CheckState.Checked
			Else
				chkOverrideDate.CheckState = CheckState.Unchecked
			End If
			If m_iOverrideRate = 1 Then
				chkOverrideRate.CheckState = CheckState.Checked
			Else
				chkOverrideRate.CheckState = CheckState.Unchecked
			End If
			If m_iOverridePrePolicyDate = 1 Then
				chkOverridePrePolicyDate.CheckState = CheckState.Checked
			Else
				chkOverridePrePolicyDate.CheckState = CheckState.Unchecked
			End If
			If m_iOverridePrePolicyRate = 1 Then
				chkOverridePrePolicyRate.CheckState = CheckState.Checked
			Else
				chkOverridePrePolicyRate.CheckState = CheckState.Unchecked
			End If
			

			txtFeeDiscount.Text = FeeDiscount
			
			m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtFeeDiscount)
			m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtWriteOff)
			
			' set write off properties
			If m_iHasTransWriteOffAuthority = 1 Then
				Me.chkTransWriteOffs.CheckState = CheckState.Checked
			Else
				Me.chkTransWriteOffs.CheckState = CheckState.Unchecked
			End If
			
			' write off amount
			cboTransWriteOffsCurrency.CurrencyId = m_iTransWriteOffCurrencyID
			txtTransWriteOff.Text = CStr(m_dTransWriteOffAmount)
			
			If m_iHasRefundAuthority = 1 Then
				Me.chkHasRefundAuthority.CheckState = CheckState.Checked
			Else
				Me.chkHasRefundAuthority.CheckState = CheckState.Unchecked
			End If
			If m_iHasTransferAuthority = 1 Then
				Me.chkHasTransferAuthority.CheckState = CheckState.Checked
			Else
				Me.chkHasTransferAuthority.CheckState = CheckState.Unchecked
			End If
			
			' Enable or disable the text box
			m_lReturn = CType(EnableTransWriteOff(), gPMConstants.PMEReturnCode)
			' format the value
			m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtTransWriteOff)
			
			If m_iHasPaymentsAuthority = 1 Then
				chkPayments.CheckState = CheckState.Checked
			Else
				chkPayments.CheckState = CheckState.Unchecked
			End If
			
			cboPaymentsCurrency.CurrencyId = m_iPaymentsCurrencyID
			txtPayments.Text = CStr(m_dPaymentsAmount)
			m_lReturn = CType(EnablePayments(), gPMConstants.PMEReturnCode)
			m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtPayments)
			
			If m_iHasClaimPaymentsAuthority = 1 Then
				chkClaimPayments.CheckState = CheckState.Checked
			Else
				chkClaimPayments.CheckState = CheckState.Unchecked
			End If
			
			cboClaimPaymentsCurrency.CurrencyId = m_iClaimPaymentsCurrencyID
			txtClaimPayments.Text = CStr(m_dClaimPaymentsAmount)
			m_lReturn = CType(EnableClaimPayments(), gPMConstants.PMEReturnCode)
			m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtClaimPayments)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DataToInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: InterfaceToData
	'
	' Description:
	'
	' History: 14/02/2000 CTAF - Created.
	'
	' AMB 05/02/2003 - Added fields for IAG 220 Manage Debtors development
	' ***************************************************************** '
	Private Function InterfaceToData() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			m_iCanWriteOff = chkWriteOffs.CheckState
			m_iWriteOffCurrencyID = cboWriteOffsCurrency.CurrencyId

			m_dWriteOffAmount = CDbl(gPMFunctions.UnFormatField(iFormatTypeIn:=gPMConstants.PMEFormatStyle.PMFormatDouble, iDataTypeOut:=gPMConstants.PMEFormatStyle.PMFormatDouble, vFieldValue:=txtWriteOff.Text))
			
			m_iUnrestrictedEnquiry = chkUnrestrictedEnquiry.CheckState
			m_iUnrestrictedUpdate = chkUnrestrictedUpdate.CheckState
			m_iOverrideDate = chkOverrideDate.CheckState
			m_iOverrideRate = chkOverrideRate.CheckState
			m_iOverridePrePolicyDate = chkOverridePrePolicyDate.CheckState
			m_iOverridePrePolicyRate = chkOverridePrePolicyRate.CheckState
			


			m_vFeeDiscount = gPMFunctions.UnFormatField(iFormatTypeIn:=gPMConstants.PMEFormatStyle.PMFormatDouble, iDataTypeOut:=gPMConstants.PMEFormatStyle.PMFormatDouble, vFieldValue:=txtFeeDiscount.Text)
			
			m_iHasTransWriteOffAuthority = chkTransWriteOffs.CheckState
			m_iHasRefundAuthority = chkHasRefundAuthority.CheckState
			m_iHasTransferAuthority = chkHasTransferAuthority.CheckState
			
			m_iTransWriteOffCurrencyID = cboTransWriteOffsCurrency.CurrencyId

			m_dTransWriteOffAmount = CDbl(gPMFunctions.UnFormatField(iFormatTypeIn:=gPMConstants.PMEFormatStyle.PMFormatDouble, iDataTypeOut:=gPMConstants.PMEFormatStyle.PMFormatDouble, vFieldValue:=txtTransWriteOff.Text))
			
			m_iHasPaymentsAuthority = chkPayments.CheckState
			m_iHasClaimPaymentsAuthority = chkClaimPayments.CheckState
			
			m_iPaymentsCurrencyID = cboPaymentsCurrency.CurrencyId

			m_dPaymentsAmount = CDbl(gPMFunctions.UnFormatField(iFormatTypeIn:=gPMConstants.PMEFormatStyle.PMFormatDouble, iDataTypeOut:=gPMConstants.PMEFormatStyle.PMFormatDouble, vFieldValue:=txtPayments.Text))
			
			m_iClaimPaymentsCurrencyID = cboClaimPaymentsCurrency.CurrencyId

			m_dClaimPaymentsAmount = CDbl(gPMFunctions.UnFormatField(iFormatTypeIn:=gPMConstants.PMEFormatStyle.PMFormatDouble, iDataTypeOut:=gPMConstants.PMEFormatStyle.PMFormatDouble, vFieldValue:=txtClaimPayments.Text))
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InterfaceToData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: EnableWriteOff
	'
	' Description: Enables or disables the write off text box
	'
	' History: 16/02/2000 CTAF - Created.
	'
	' ***************************************************************** '
	Private Function EnableWriteOff() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If chkWriteOffs.CheckState = CheckState.Checked Then
				txtWriteOff.Enabled = True
				lblAmount.Enabled = True
				cboWriteOffsCurrency.Enabled = True
				lblWriteOffsCurrency.Enabled = True
			Else
				txtWriteOff.Enabled = False
				lblAmount.Enabled = False
				cboWriteOffsCurrency.Enabled = False
				lblWriteOffsCurrency.Enabled = False
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnableWriteOff Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnableWriteOff", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	'
	' Name: EnableTransWriteOff
	'
	' Description: Enables or disables the write off text box
	'
	' History: 06/02/2003 AMB - Created - copied from EnableTransWriteOff
	'
	' ***************************************************************** '
	Private Function EnableTransWriteOff() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If chkTransWriteOffs.CheckState = CheckState.Checked Then
				txtTransWriteOff.Enabled = True
				lblTransAmount.Enabled = True
				cboTransWriteOffsCurrency.Enabled = True
				lblTransWriteOffsCurrency.Enabled = True
			Else
				txtTransWriteOff.Enabled = False
				lblTransAmount.Enabled = False
				cboTransWriteOffsCurrency.Enabled = False
				lblTransWriteOffsCurrency.Enabled = False
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnableTransWriteOff Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnableTransWriteOff", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Sub chkClaimPayments_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkClaimPayments.CheckStateChanged
		m_lReturn = CType(EnableClaimPayments(), gPMConstants.PMEReturnCode)
	End Sub
	
	Private Sub chkPayments_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkPayments.CheckStateChanged
		m_lReturn = CType(EnablePayments(), gPMConstants.PMEReturnCode)
	End Sub
	
	Private Sub chkTransWriteOffs_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkTransWriteOffs.CheckStateChanged
		m_lReturn = CType(EnableTransWriteOff(), gPMConstants.PMEReturnCode)
	End Sub
	Private Sub chkWriteOffs_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkWriteOffs.CheckStateChanged
		m_lReturn = CType(EnableWriteOff(), gPMConstants.PMEReturnCode)
	End Sub
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		' Set to cancel
		m_iStatus = gPMConstants.PMEReturnCode.PMCancel
		
		' Hide the form
		Me.Hide()
	End Sub
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		' Set to ok
		m_iStatus = gPMConstants.PMEReturnCode.PMOK
		
		' Check mandatory controls have been entered into.
		m_lReturn = m_oFormFields.CheckMandatoryControls()
		
		' Check for errors
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			Exit Sub
		End If
		
		m_lReturn = CType(InterfaceToData(), gPMConstants.PMEReturnCode)
		
		' Hide the form
		Me.Hide()
		
	End Sub
	
	Private Sub Form_Initialize_Renamed()
		
		Try 
			
			' Create an instance of the form control object.
			m_oFormFields = New iPMFormControl.FormFields()
			
			' Set language
			m_oFormFields.LanguageID = g_iLanguageID
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	

	Private Sub frmAuthorities_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		Try 
			
			m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)
			
			m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)
			
			m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub frmAuthorities_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		Try 
			
			' Terminate the form control object.
		m_oFormFields.Dispose()
			
            ' Remove the instance
            m_oFormFields = Nothing
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
			eventArgs.Cancel = Cancel <> 0
		End Try
		
	End Sub
	
	Private Sub txtClaimPayments_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtClaimPayments.Enter
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtClaimPayments)
	End Sub
	Private Sub txtClaimPayments_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtClaimPayments.Leave
		If txtClaimPayments.Text <> "" Then
			Dim dbNumericTemp As Double
			If Not Double.TryParse(txtClaimPayments.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
				MessageBox.Show("Field amount doesn't allow alpha-numeric characters. Please re-enter.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				txtClaimPayments.Text = ""
				txtClaimPayments.Focus()
			End If
		End If
		
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtClaimPayments)
	End Sub
	
	Private Sub txtFeeDiscount_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFeeDiscount.Enter
		
		' AMB 06/02/2003 - IMPORTANT NOTE
		'                  The 'Fee Discount' field has bee made
		'                  invisible as it's use could not be determined.
		'                  See Danny Davis if you wish to re-instante this field.
		
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtFeeDiscount)
		
	End Sub
	
	Private Sub txtFeeDiscount_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFeeDiscount.Leave
		
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtFeeDiscount)
		
	End Sub
	Private Sub txtPayments_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPayments.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtPayments)
	End Sub
	
	Private Sub txtPayments_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPayments.Leave
		If txtPayments.Text <> "" Then
			Dim dbNumericTemp As Double
			If Not Double.TryParse(txtPayments.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
				MessageBox.Show("Field amount doesn't allow alpha-numeric characters. Please re-enter.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				txtPayments.Text = ""
				txtPayments.Focus()
			End If
		End If
		
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtPayments)
	End Sub
	
	Private Sub txtTransWriteOff_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTransWriteOff.Enter
		
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtTransWriteOff)
		
	End Sub
	
	Private Sub txtTransWriteOff_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTransWriteOff.Leave
		
		'MSB031201 - Field shouldn't allow alpha-numeric characters
		If txtTransWriteOff.Text <> "" Then
			Dim dbNumericTemp As Double
			If Not Double.TryParse(txtTransWriteOff.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
				MessageBox.Show("Field amount doesn't allow alpha-numeric characters. Please re-enter.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				txtTransWriteOff.Text = ""
				txtTransWriteOff.Focus()
			End If
		End If
		
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtTransWriteOff)
		
	End Sub
	
	Private Sub txtWriteOff_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtWriteOff.Enter
		
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtWriteOff)
		
	End Sub
	
	Private Sub txtWriteOff_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtWriteOff.Leave
		
		'MSB031201 - Field shouldn't allow alpha-numeric characters
		If txtWriteOff.Text <> "" Then
			Dim dbNumericTemp As Double
			If Not Double.TryParse(txtWriteOff.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
				MessageBox.Show("Field amount doesn't allow alpha-numeric characters. Please re-enter.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
				txtWriteOff.Text = ""
				txtWriteOff.Focus()
			End If
		End If
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtWriteOff)
	End Sub
	'MSB041201 - To check how many digits before and after the decimal place
	Public Function ValidateNumbers(ByVal sString As String, ByVal lNumberBeforeSeparator As Integer, ByVal lNumberAfterSeparator As Integer, ByVal sSeparator As String) As Boolean
		
		Dim result As Boolean = False
		Dim iSeparatorCount As Integer
		
		If sSeparator.Length Then
			iSeparatorCount = (sString.IndexOf(sSeparator) + 1)
			
			Select Case True
				Case iSeparatorCount > (lNumberBeforeSeparator + 1), (sString.Length - iSeparatorCount) > lNumberAfterSeparator
				Case Else
					result = True
			End Select
		End If
		
		Return result
	End Function
	
	Private Function EnableClaimPayments() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If chkClaimPayments.CheckState = CheckState.Checked Then
				txtClaimPayments.Enabled = True
				lblClaimPayments.Enabled = True
				cboClaimPaymentsCurrency.Enabled = True
				lblClaimPaymentsCurrency.Enabled = True
			Else
				txtClaimPayments.Enabled = False
				lblClaimPayments.Enabled = False
				cboClaimPaymentsCurrency.Enabled = False
				lblClaimPaymentsCurrency.Enabled = False
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnableClaimPayments Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnableClaimPayments", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	Private Function EnablePayments() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If chkPayments.CheckState = CheckState.Checked Then
				txtPayments.Enabled = True
				lblPayments.Enabled = True
				cboPaymentsCurrency.Enabled = True
				lblPaymentsCurrency.Enabled = True
			Else
				txtPayments.Enabled = False
				lblPayments.Enabled = False
				cboPaymentsCurrency.Enabled = False
				lblPaymentsCurrency.Enabled = False
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnablePayments Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnablePayments", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Function DisplayCaptions() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Display all language specific captions.
			

			Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAuthoritiesTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			If Me.Text = "" Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &  _
				                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")
				
				Return result
			End If
			

			SSTabHelper.SetTabCaption(tabMain, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAuthoritiesTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			fmeAccess.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAccessCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			chkUnrestrictedEnquiry.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACUnrestrictedEnquiryCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			chkUnrestrictedUpdate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACUnrestrictedUpdateCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			chkOverrideDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOverrideDateCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			chkOverrideRate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOverrideRateCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			fmeAuthority.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAuthorityCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			chkHasRefundAuthority.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRefundAuthorityCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			chkHasTransferAuthority.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTransferAuthorityRateCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			fmePayments.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPaymentsCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			chkPayments.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPaymentsAuthorityCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblPaymentsCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCurrencyCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblPayments.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmountCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			fmeWriteOffs.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACWriteOffsCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			chkWriteOffs.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAllocationWriteOffsCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblWriteOffsCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCurrencyCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblAmount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmountCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			chkTransWriteOffs.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDebtWriteOffsCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblTransWriteOffsCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCurrencyCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblTransAmount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmountCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			fmeClaimPayments.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClaimPaymentsCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			chkClaimPayments.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClaimPaymentsAuthorityCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblClaimPaymentsCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCurrencyCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblClaimPayments.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmountCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function

    Private Sub frmAuthorities_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMain.SelectedIndex = 0
        End If
    End Sub
End Class
