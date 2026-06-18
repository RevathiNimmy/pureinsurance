Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
Friend Partial Class frmRefund
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmRefund
	'
	' Date: 10/02/2003
	'
	' Description: Main interface.
	'
	' Edit History:
	'       AMB 19/02/2003: PS220 - Manage Debtors development - created
	' ***************************************************************** '
    'replaced iPMFunc.GetResData with GetResData in the whole document
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "frmRefund"
	
	Private m_oFormfields As iPMFormControl.FormFields
	
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Stores the return value for function calls.
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	' passed in properties
	Private m_dRefundAmount As Double
	Private m_dOriginalRefundAmount As Double
	' returned properties
	Private m_lMediaTypeID As Integer
	Private m_sMediaTypeName As String = ""
	' PUBLIC Property Procedures (Begin)
	Public ReadOnly Property ErrorNumber() As Integer
		Get
			
			' Standard Property.
			
			' Return any error number that might have
			' occurred on the interface.
			Return m_lErrorNumber
			
		End Get
	End Property
	
	
	' {* USER DEFINED CODE (Begin) *}
	
	
	' {* USER DEFINED CODE (End) *}
	' PUBLIC Property Procedures (End)
	
	
	' PRIVATE Property Procedures (Begin)
	
	'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub Status(ByVal Value As Integer)
		'
		' Standard Property.
		'
		' Set the interface exit status.
		'm_lStatus = Value
		'
	'End Sub
	Public ReadOnly Property Status() As Integer
		Get
			
			' Standard Property.
			
			' Return the interface exit status.
			Return m_lStatus
			
		End Get
	End Property
	
	Public Property RefundAmount() As Double
		Get
			
			Return m_dRefundAmount
			
		End Get
		Set(ByVal Value As Double)
			
			m_dRefundAmount = Value
			
			Dim dbNumericTemp As Double
			If Double.TryParse(CStr(Value), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
				' set the texbox value
				txtRefundAmount.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, Value)
			End If
			
			' set the original refund amount value for later
			m_dOriginalRefundAmount = m_dRefundAmount
			
		End Set
	End Property
	
	Public ReadOnly Property MediaTypeID() As Integer
		Get
			
			Return m_lMediaTypeID
			
		End Get
	End Property
	
	Public ReadOnly Property MediaTypeName() As String
		Get
			
			Return m_sMediaTypeName
			
		End Get
	End Property
	' PRIVATE Property Procedures (End)
	
	' ***************************************************************** '
	' Name: GetMediaTypes
	'
	' Description: Gets a list of media types then loads them into the combo
	'
	' ***************************************************************** '
	Private Function GetMediaTypes(ByRef cboCombo As ComboBox) As Integer
		
		Dim result As Integer = 0
		Dim vMediaTypes(,) As Object
		
		Const MEDIA_TYPE_ID As Integer = 0
		Const MEDIA_TYPE_NAME As Integer = 2
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get array containing list of media types

			m_lReturn = m_oBusiness.GetMediaTypes(r_vResultArray:=vMediaTypes)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = m_lReturn
				
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to a list of media types", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMediaTypes", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				
				Return result
			End If
			
			If Information.IsArray(vMediaTypes) Then
				
				' fill the combo
				cboCombo.Items.Clear()

				For lLoopy As Integer = 0 To vMediaTypes.GetUpperBound(1)
					Dim cboCombo_NewIndex As Integer = -1

                    cboCombo_NewIndex = cboCombo.Items.Add(CStr(vMediaTypes(MEDIA_TYPE_NAME, lLoopy)))

                    VB6.SetItemData(cboCombo, cboCombo_NewIndex, CInt(vMediaTypes(MEDIA_TYPE_ID, lLoopy)))
				Next lLoopy
				
				' select the first one
				If cboCombo.Items.Count Then
					cboCombo.SelectedIndex = 0
				End If
				
			Else
				Return m_lReturn
				
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMediaTypes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
	
	' PUBLIC Methods (Begin)
	' ***************************************************************** '
	' Name: SetFieldValidation
	'
	' Description: Sets the rules for validating fields.
	'
	' ***************************************************************** '
	Public Function SetFieldValidation() As Integer
		
		Dim result As Integer = 0
		m_oFormfields = New iPMFormControl.FormFields()
		
		m_oFormfields.LanguageID = g_iLanguageID
		
		m_lReturn = m_oFormfields.AddNewFormField(ctlControl:=cboMediaType, lFieldType:=gPMConstants.PMEDataType.PMLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
		
		m_lReturn = m_oFormfields.AddNewFormField(ctlControl:=txtRefundAmount, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMInteger, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			Return gPMConstants.PMEReturnCode.PMFalse
		End If
		
		
		Return gPMConstants.PMEReturnCode.PMTrue
		
		
		
		' Error Section.
		
		result = gPMConstants.PMEReturnCode.PMError
		
		' Log Error.
		iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		Return result
		
	End Function
	' ***************************************************************** '
	' Name: DisplayCaptions
	'
	' Description: Display all language specific captions.
	'
	' ***************************************************************** '
	Private Function DisplayCaptions() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Display all language specific captions
			

            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRefundInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRefundCancelCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRefundOKCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            lblPaymentMethod.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRefundPaymentMethodLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            lblRefundAmount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRefundRefundAmountLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Sub cboMediaType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboMediaType.SelectedIndexChanged
		
		m_lMediaTypeID = VB6.GetItemData(cboMediaType, cboMediaType.SelectedIndex)
		m_sMediaTypeName = cboMediaType.Text
		
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		' Click event of the Cancel button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			' Everything OK, so we can hide the interface.
			Me.Hide()
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
		End Try
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		' Click event of the OK button.
		
		Try 
			
			m_lReturn = m_oFormfields.CheckMandatoryControls()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMOK
			
			' Everything OK, so we can hide the interface.
			Me.Hide()
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	
	Private Sub Form_Initialize_Renamed()
		Dim sMessage, sTitle As String
		
		' Forms initialise event.
		
		Try 
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Initialise the error number value.
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
			
			' Get an instance of the business object via
			' the public object manager.
			
			Dim temp_m_oBusiness As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bACTManageDebtors.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
			m_oBusiness = temp_m_oBusiness
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get an instance of the business object.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Display error stating the problem.
				
				' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
				

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
				
				' Display message.
				MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
				
				Exit Sub
			End If
			
			' Set the interface status to cancelled. This is done
			' so that any interface termination will be noted
			' as cancelled except in the event of accepting
			' the interface.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
		End Try
		
	End Sub
	

	Private Sub frmRefund_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		' Forms load event.
		
		Try 
			
			' Check if we have had an error so far.
			' Possibly creating the business object.
			If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
				' We have already encountered an error,
				' so we MUST exit now.
				Exit Sub
			End If
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Exit Sub
			End If
			
			' Set the interface default values.
			m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Exit Sub
			End If
			
			m_lReturn = CType(GetMediaTypes(cboCombo:=cboMediaType), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Exit Sub
			End If
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub frmRefund_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		' Forms query unload event.
		
		Try 
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Terminate the business object

		m_oBusiness.Dispose()
			
			
			
			' Destroy the instance of the business object
			' from memory.
			m_oBusiness = Nothing
			
            m_oFormfields.Dispose()

            m_oFormfields = Nothing
			
			
			' Reset the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
			eventArgs.Cancel = Cancel <> 0
		End Try
		
	End Sub
	' ***************************************************************** '
	' Name: SetInterfaceDefaults
	'
	' Description: Sets all of the interface default values.
	'
	' ***************************************************************** '
	Private Function SetInterfaceDefaults() As Integer
		
		Dim result As Integer = 0
		Dim sSelect As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Display all language specific captions.
			m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' PRIVATE Events (Begin)
	
    Private Sub txtRefundAmount_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRefundAmount.Enter
        m_lReturn = m_oFormfields.GotFocus(ctlControl:=txtRefundAmount)
    End Sub
	
	Private Sub txtRefundAmount_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRefundAmount.Leave
		
		Dim dCurrAmount As Double
		
		m_lReturn = m_oFormfields.LostFocus(ctlControl:=txtRefundAmount)
		
		' ensure that the refund amount doesn't exceed the total o/s amount
		If Strings.Len(txtRefundAmount.Text) Then
			Dim dbNumericTemp As Double
			If Double.TryParse(txtRefundAmount.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
				
				dCurrAmount = CDbl(txtRefundAmount.Text)
				
				' validate the refund
				If (dCurrAmount > m_dOriginalRefundAmount) Or (dCurrAmount < 0) Then
					' let's tell the silly user
					DisplayMessage(ACRefundInterfaceTitle, ACRefundAmountExceeded, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
					' reset the value
					txtRefundAmount.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_dOriginalRefundAmount)
					Exit Sub
				End If
				
				m_dRefundAmount = dCurrAmount
				
			End If
			
		End If
		
	End Sub
End Class
