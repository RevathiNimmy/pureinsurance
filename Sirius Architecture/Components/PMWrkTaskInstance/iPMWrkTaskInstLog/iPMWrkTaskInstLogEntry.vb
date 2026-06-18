Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmTaskLogEntry
	Inherits System.Windows.Forms.Form
	
	'Declarations
	Private m_lPMWrkTaskInstanceCnt As Integer
	Private m_dtDateCreated As Date
	Private m_sLogText As String = ""
	Private m_iCreatedByID As Integer
	
	Private m_lStatus As Integer
	
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	Private m_lFormMode As Integer
	
	' Declare an instance of the Form Control object.
	Private m_oFormFields As iPMFormControl.FormFields
	
	Private Const ACClass As String = "frmTaskLogEntry"
	
	
	Public Property PMWrkTaskInstanceCnt() As Integer
		Get
			
			Return m_lPMWrkTaskInstanceCnt
			
		End Get
		Set(ByVal Value As Integer)
			
			m_lPMWrkTaskInstanceCnt = Value
			
		End Set
	End Property
	
	
	Public Property DateCreated() As Date
		Get
			
			Return m_dtDateCreated
			
		End Get
		Set(ByVal Value As Date)
			
			m_dtDateCreated = Value
			
		End Set
	End Property
	
	
	Public Property LogText() As String
		Get
			
			Return m_sLogText
			
		End Get
		Set(ByVal Value As String)
			
			m_sLogText = Value
			
		End Set
	End Property
	
	
	Public Property CreatedByID() As Integer
		Get
			
			Return m_iCreatedByID
			
		End Get
		Set(ByVal Value As Integer)
			
			m_iCreatedByID = Value
			
		End Set
	End Property
	
	Public ReadOnly Property Status() As Integer
		Get
			
			Return m_lStatus
			
		End Get
	End Property
	
	
	Private Property FormMode() As Integer
		Get
			
			Return m_lFormMode
			
		End Get
		Set(ByVal Value As Integer)
			
			m_lFormMode = Value
			
		End Set
	End Property
	
	'*************************************************************
	'
	' Function Name:ShowForm()
	'
	' Description: Shows form details which correspond with what
	'              the Task has selected from the previous form
	'*************************************************************
	
	Public Function ShowForm(ByRef lEditMode As Integer) As Integer
		
		Dim result As Integer = 0

		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			FormMode = lEditMode
			
			m_lReturn = SetFieldValidation()
			
			cboPMUserLookup1.SingleUserID = m_iCreatedByID
			cboPMUserLookup1.RefreshList()

            'developers guid no 26
            lbl3CreatedBy.Text = cboPMUserLookup1.ItemUsername

			m_lReturn = m_oFormFields.FormatControl(txtTaskLogDate, m_dtDateCreated)
			m_lReturn = m_oFormFields.FormatControl(txtTaskLogTime, m_dtDateCreated)
			
			m_lReturn = m_oFormFields.FormatControl(txtTaskLogText, m_sLogText)
			
			
			Select Case lEditMode
				Case USRAddTask
					'Give some names
					Me.Text = "Add Task Log Entry"
					txtTaskLogText.Enabled = True
					
				Case USRViewTask
					'Give some names
					Me.Text = "View Task Log Entry"
					txtTaskLogText.Enabled = False
					
				Case Else
					
			End Select
			'Show the form
			Me.ShowDialog()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			'Error Section
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to show Task Form", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: SetFieldValidation
	'
	' Description: Sets the rules for validating fields.
	'
	' ***************************************************************** '
	Private Function SetFieldValidation() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			m_oFormFields = New iPMFormControl.FormFields()
			
			m_oFormFields.LanguageID = g_iLanguageID
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtTaskLogText, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtTaskLogDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtTaskLogTime, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatTimeLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		
		Me.Hide()
		
	End Sub
	
	Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
		
		MessageBox.Show("There is no help associated with this screen", "Task Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click


		m_sLogText = CStr(m_oFormFields.UnformatControl(txtTaskLogText))
		
		' Check to see if text has been entered
		If m_sLogText = "" Then
			
			MessageBox.Show("Task log text not entered.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
			txtTaskLogText.Focus()
			Exit Sub
			
		End If
		
		If FormMode = USRAddTask Then

			m_lReturn = g_oBusiness.AddLogEntry(v_lPMWrkTaskInstanceCnt:=m_lPMWrkTaskInstanceCnt, v_dtDateCreated:=m_dtDateCreated, v_sText:=m_sLogText, v_iCreatedByID:=m_iCreatedByID)

        End If
		
		'Set status to PmOK
		m_lStatus = gPMConstants.PMEReturnCode.PMOK
		
		'hide this form
		Me.Hide()
		
	End Sub
	
	Private Sub frmTaskLogEntry_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			
			With uctPMResizer1
				.SetControlResizeOption("cmdOK", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdCancel", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdHelp", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("tabMainTab", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				
			End With
			
		End If
	End Sub
	

	Private Sub frmTaskLogEntry_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		With uctPMResizer1
			.NoResizeByDefault = True
			.FormMinHeight = 4050
			.FormMinWidth = 7260
		End With
		
	End Sub
	
	Private Sub txtTaskLogText_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTaskLogText.Enter
		
		m_lReturn = m_oFormFields.GotFocus(txtTaskLogText)
		
	End Sub
	
	Private Sub txtTaskLogText_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTaskLogText.Leave
		
		m_lReturn = m_oFormFields.LostFocus(txtTaskLogText)
		
	End Sub
End Class
