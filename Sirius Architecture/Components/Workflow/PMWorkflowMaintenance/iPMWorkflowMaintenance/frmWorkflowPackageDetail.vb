Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles

Friend Partial Class frmWorkflowPackageDetail
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmWorkflowPackageDetail
	'
	' Date: 22/01/2003
	'
	' Description:
	'
	' Edit History:
	'               AMB 22/01/2003 - Created
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmWorkflowPackageDetail"
	
	'Declarations
	
	Private m_lStatus As Integer
	
	Private m_lError As Integer
	
	Private m_lFormMode As Integer
	
	Private m_lCaptionID As Integer
	Private m_sCode As String = ""
	Private m_sDescription As String = ""
	Private m_iIsDeleted As Integer
	Private m_dtEffectiveDate As Date
	Private m_lPackageID As Integer
	
	Private m_iStatus As gPMConstants.PMEReturnCode
	Private m_iTask As gPMConstants.PMEComponentAction
	
	Private m_iProductFamily As Integer
	
	' Declare an instance of the FormControl object
	Private m_oFormFields As iPMFormControl.FormFields
	
	' PMAuthorityLevel
	Private m_lPMAuthorityLevel As Integer
	
	' Return variable
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	Private m_oBusiness As Object
	
	Public Property PackageID() As Integer
		Get
			
			Return m_lPackageID
			
		End Get
		Set(ByVal Value As Integer)
			
			m_lPackageID = Value
			
		End Set
	End Property
	
	
	
	Private Property FormMode() As Integer
		Get
			
			Return m_lFormMode
			
		End Get
		Set(ByVal Value As Integer)
			
			m_lFormMode = Value
			
		End Set
	End Property
	
	
	Public Property ProductFamily() As Integer
		Get
			Return m_iProductFamily
		End Get
		Set(ByVal Value As Integer)
			m_iProductFamily = Value
		End Set
	End Property
	
	Public ReadOnly Property Status() As Integer
		Get
			Return m_iStatus
		End Get
	End Property
	
	
	Public Property CaptionID() As Integer
		Get
			Return m_lCaptionID
		End Get
		Set(ByVal Value As Integer)
			m_lCaptionID = Value
		End Set
	End Property
	
	Public Property Code() As String
		Get
			Return m_sCode
		End Get
		Set(ByVal Value As String)
			m_sCode = Value
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
	
	Public Property IsDeleted() As Integer
		Get
			Return m_iIsDeleted
		End Get
		Set(ByVal Value As Integer)
			m_iIsDeleted = Value
		End Set
	End Property
	
	Public Property EffectiveDate() As Date
		Get
			Return m_dtEffectiveDate
		End Get
		Set(ByVal Value As Date)
			m_dtEffectiveDate = Value
		End Set
	End Property
	
	Public WriteOnly Property ProcessMode() As Integer
		Set(ByVal Value As Integer)
			m_iTask = Value
		End Set
	End Property
	
	Public Property PMAuthorityLevel() As Integer
		Get
			Return m_lPMAuthorityLevel
		End Get
		Set(ByVal Value As Integer)
			m_lPMAuthorityLevel = Value
		End Set
	End Property
	
	'*************************************************************
	'
	' Function Name:ShowForm()
	'
	' Description: Shows form details which correspond with what
	'              the user has selected from the previous form
	'*************************************************************
	
	Public Function ShowForm(ByRef lEditMode As Integer) As Integer
		
		Dim result As Integer = 0

		Try 
			
			FormMode = lEditMode
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			m_lReturn = SetFieldValidation()
			
			m_lReturn = m_oFormFields.FormatControl(txtCode, m_sCode)
			m_lReturn = m_oFormFields.FormatControl(txtDescription, m_sDescription)
			m_lReturn = m_oFormFields.FormatControl(txtEffectiveDate, m_dtEffectiveDate)
			
			Select Case FormMode
				Case USRAddPackage
					
					txtCode.Text = ""
					txtDescription.Text = ""
					txtEffectiveDate.Text = StringsHelper.Format(DateTime.Now, "General Date")
					
					'Give some names
					Me.Text = "Add Workflow Package"
					
					txtCode.Enabled = True
					txtEffectiveDate.Enabled = True
					
					cmdSteps.Enabled = False
					
					ToolTip1.SetToolTip(cmdOK, "Accepts entry of new package")
					ToolTip1.SetToolTip(cmdCancel, "Cancels entry of new package")
					
				Case USREditPackage
					
					txtCode.Text = m_sCode.Trim()
					txtDescription.Text = m_sDescription.Trim()
					txtEffectiveDate.Text = StringsHelper.Format(m_dtEffectiveDate, "General Date")
					
					Me.Text = m_sCode.Trim() & " Properties"
					
					'Set some defaults
					txtCode.Enabled = False
					txtEffectiveDate.Enabled = False
					
					cmdSteps.Enabled = True
					
					ToolTip1.SetToolTip(cmdOK, "Accept Changes and return to previous screen")
					ToolTip1.SetToolTip(cmdCancel, "Cancel Changes and return to previous screen")
					
			End Select
			
			'Show the form
			Me.ShowDialog()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			'Error Section
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to show Group Form", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
        Dim lReturn As gPMConstants.PMEReturnCode
		
		Application.DoEvents()
		
		' Check mandatory fields
		m_lReturn = m_oFormFields.CheckMandatoryControls()
		
		If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
			Exit Sub
		End If
		

		m_sCode = CStr(m_oFormFields.UnformatControl(txtCode))

		m_sDescription = CStr(m_oFormFields.UnformatControl(txtDescription))

		m_dtEffectiveDate = CDate(m_oFormFields.UnformatControl(txtEffectiveDate))
		
		If FormMode = USRAddPackage Then
			' Check for Duplicate Package Code
			lReturn = CType(CheckPackageName(m_sCode.ToUpper()), gPMConstants.PMEReturnCode)
			
			If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("A workflow package named '" & m_sCode &  _
				                "' already exists." & Strings.Chr(13) & Strings.Chr(10) &  _
				                "Please choose another code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				
				txtCode.Focus()
				txtCode.Text = ""
				Exit Sub
			End If
			
			If lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
				' error...
				Exit Sub
			End If
			
		End If
		
		m_iStatus = gPMConstants.PMEReturnCode.PMOK
		Me.Hide()
		
	End Sub
	
	
	' ***************************************************************** '
	' Name: CheckPackageName
	'
	' Description:
	'
	'
	' ***************************************************************** '
	Private Function CheckPackageName(ByVal v_m_sPackageName As String) As Integer
		
		Dim result As Integer = 0

		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Getdetails from business

			m_lReturn = m_oBusiness.GetDetailsByCode(v_m_sPackageName)
			
			
			If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
				Return gPMConstants.PMEReturnCode.PMFalse
			Else
				Return m_lReturn
			End If
		
		Catch 
		End Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMError
		
		' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckPackageNameFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckPackageName", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		Return result
		
	End Function
	
	
	Private Sub cmdSteps_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSteps.Click
		
		m_lReturn = OpenSteps()
		
	End Sub
	
	Private Function OpenSteps() As Integer
		Dim result As Integer = 0
        'Dim iPMStepMaintenance As Object
		
        Dim oObject As iPMStepMaintenance.Interface_Renamed
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
            oObject = New iPMStepMaintenance.Interface_Renamed
			

			m_lReturn = oObject.Initialise
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				oObject = Nothing
				Return result
			End If
			

			oObject.WorkflowID = PackageID
			

			m_lReturn = oObject.Start
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				oObject = Nothing
				Return result
			End If
			

            oObject.Dispose()
            oObject = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OpenSteps Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenSteps", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	
	Private Sub Form_Initialize_Renamed()
		
		m_lReturn = InitialForm()
		
	End Sub
	
	Private Function InitialForm() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get an instance of the business object via
			' the public object manager.
			Dim temp_m_oBusiness As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bPMWorkflowMaintenance.Business", vInstanceManager:="ClientManager")
			m_oBusiness = temp_m_oBusiness
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get an instance of the business object.
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Display message.
				MessageBox.Show(ACBusinessFailText, ACBusinessFailTitleText, MessageBoxButtons.OK, MessageBoxIcon.Error)
				
				Return result
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			'Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the business", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	Private Sub txtCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCode.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtCode)
	End Sub
	
	Private Sub txtCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCode.Leave
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtCode)
	End Sub
	
	Private Sub txtDescription_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDescription)
	End Sub
	
	Private Sub txtDescription_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Leave
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDescription)
	End Sub
	
	Private Sub txtEffectiveDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEffectiveDate.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtEffectiveDate)
	End Sub
	
	Private Sub txtEffectiveDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEffectiveDate.Leave
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtEffectiveDate)
	End Sub
	' ***************************************************************** '
	' Name: SetFieldValidation
	'
	' Description:
	'
	'
	' ***************************************************************** '
	Private Function SetFieldValidation() As Integer
		
		Dim result As Integer = 0

		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			m_oFormFields = New iPMFormControl.FormFields()
			
			m_oFormFields.LanguageID = g_iLanguageID
			
			' Code
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCode, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Description
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDescription, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Effective Date
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtEffectiveDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lFieldType:=gPMConstants.PMEDataType.PMDate, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetFieldValidation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' ***************************************************************** '
	' Name: SetProperties
	'
	' Description: Set the text values from the properties
	'
	' ***************************************************************** '
	Private Function SetProperties() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Code
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCode, vControlValue:=m_sCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Description
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDescription, vControlValue:=m_sDescription)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Effective Date
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtEffectiveDate, vControlValue:=m_dtEffectiveDate)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: Start
	'
	' Description: Sets the properties and displays the form
	'
	' ***************************************************************** '
	Public Function Start() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'DAK251199
			m_iStatus = gPMConstants.PMEReturnCode.PMCancel
			
			' Set the mouse pointer to the hourglass
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Create a new instance of form fields
			m_oFormFields = New iPMFormControl.FormFields()
			
			m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				' Set the mouse pointer to the default
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Return result
			End If
			
			' Set the properties on the form
			m_lReturn = CType(SetProperties(), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				' Set the mouse pointer to the hourglass
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Return result
			End If
			
			' Disable the controls if in view mode
			If m_iTask = gPMConstants.PMEComponentAction.PMView Then
				m_lReturn = CType(DisableForm(), gPMConstants.PMEReturnCode)
			End If
			
			' Set the mouse pointer to the hourglass
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Display the form
			Me.ShowDialog()
			
			'DAK080200 - only update if in Add or Edit mode
			If m_iStatus = gPMConstants.PMEReturnCode.PMOK And (m_iTask = gPMConstants.PMEComponentAction.PMAdd Or m_iTask = gPMConstants.PMEComponentAction.PMEdit) Then
				' Get the values off the form
				m_lReturn = CType(GetProperties(), gPMConstants.PMEReturnCode)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			End If
			
			' Terminate form control
            m_oFormFields.Dispose()
			
			' Remove the instance of form control
			m_oFormFields = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetProperties
	'
	' Description:
	'
	' ***************************************************************** '
	Private Function GetProperties() As Integer
		
		Dim result As Integer = 0

		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

			m_sCode = CStr(m_oFormFields.UnformatControl(ctlControl:=txtCode))

			m_sDescription = CStr(m_oFormFields.UnformatControl(ctlControl:=txtDescription))

			m_dtEffectiveDate = CDate(m_oFormFields.UnformatControl(ctlControl:=txtEffectiveDate))
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' ***************************************************************** '
	' Name: DisableForm
	'
	' Description: Disables controls on the form (for View mode).
	'              There's no need to be able re-enable them.
	'
	' ***************************************************************** '
	Private Function DisableForm() As Integer
		
		Dim result As Integer = 0
		
		' CF 310899 - Added type of CheckBox
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

			For	Each oControl As Control In ContainerHelper.Controls(Me)
				
				If TypeOf oControl Is TextBox Then
					oControl.Enabled = False
				ElseIf TypeOf oControl Is Label Then 
					oControl.Enabled = False
				ElseIf TypeOf oControl Is CheckBox Then 
					oControl.Enabled = False
				End If
				
			Next oControl
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisableForm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		Me.Hide()
		
	End Sub
End Class
