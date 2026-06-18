Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Globalization
Imports System.Windows.Forms
'Modified by Sumeet Singh on 5/10/2010 1:06:43 PM refer developer guide no. 129
Imports SharedFiles
Friend Partial Class frmDetails
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmDetails
	'
	' Date:  01/02/2001
	'
	' Created By: Ajit Kumar
	'
	' Description: Details interface
	'
	' Edit History:
	'   26/06/2002 SJP  - Merged from Carole Nash into Broking
	' RAM20040225       - Code changes related to PN Issue 6151, 6748, 7408, 10286 (1.8.5 Catch-up)
	' CLG 01/09/2004 : RFC71  Added product option to allow spool and archive of documents
	' CJB 26/04/2005 : PN1926 Changed cboProcess_Click to not allow agents to be attached to any renewal
	'                  process as we have not yet developed the system to cater for this when producing
	'                  docs during these stages.
	' ***************************************************************** '
	
	
	Private m_lReturn As Integer
	
	Private Const ACClass As String = "frmDetails"
	
	Private m_lStatus As gPMConstants.PMEReturnCode
	' Risk Group
	Private m_sGISScheme As String = ""
	Private m_lGISSchemeID As Integer
	' Process
	Private m_sProcess As String = ""
	Private m_lProcessID As Integer
	' SchemeVer
	Private m_lSchemeVer As Integer
	'Agent
	Private m_lAgentCnt As Integer
	Private m_sAgentCode As String = ""
	Private m_sAgent As String = ""
	' Document type
	Private m_sDocType As String = ""
	
	' Document Template
	Private m_sDocTemplate As String = ""
	Private m_sDocTemplateCode As String = ""
	Private m_lDocTemplateID As Integer
	Private m_lDocTemplateTypeID As Integer
	Private m_sDocTemplateTypeCode As String = ""
	Private m_sDocTemplateType As String = ""
	Private m_vProcess( ,  ) As Object
	Private m_vActionType As gPMConstants.PMEComponentAction
	Private m_iSpoolDocs As Integer
	Private m_iAutoArchiveDocument As Integer
	
	' Declare an instance of the FormControl object
	Private m_oFormFields As iPMFormControl.FormFields
	
	' AllMandatory
	Private m_bAllMandatory As Boolean
	
	'CLG RFC71 2004-09-01
	Private m_lSpoolAndArchiveMode As Integer
	
	'DC060505 PN20763
	Private m_vUnderwritingOrAgency As Object
	
	Public ReadOnly Property Status() As Integer
		Get
			
			' Return the interface exit status.
			Return m_lStatus
			
		End Get
	End Property
	
	' BSJ191202 - Scheme version
	
	Public Property SchemeVer() As Integer
		Get
			Return m_lSchemeVer
		End Get
		Set(ByVal Value As Integer)
			m_lSchemeVer = Value
		End Set
	End Property
	
	
	Public Property GISScheme() As String
		Get
			Return m_sGISScheme
		End Get
		Set(ByVal Value As String)
			m_sGISScheme = Value
		End Set
	End Property
	
	Public Property Process() As String
		Get
			Return m_sProcess
		End Get
		Set(ByVal Value As String)
			m_sProcess = Value
		End Set
	End Property
	
	
	Public Property ProcessID() As Integer
		Get
			Return m_lProcessID
		End Get
		Set(ByVal Value As Integer)
			m_lProcessID = Value
		End Set
	End Property
	
	'DC040702 -start
	
	Public Property Agent() As String
		Get
			'sj 13/08/2002 - start
			'Process = m_sAgent
			Return m_sAgent
			'sj 13/08/2002 - end
		End Get
		Set(ByVal Value As String)
			m_sAgent = Value
		End Set
	End Property
	
	
	Public Property AgentID() As Integer
		Get
			Return m_lAgentCnt
		End Get
		Set(ByVal Value As Integer)
			m_lAgentCnt = Value
		End Set
	End Property
	'DC040702 -end
	
	Public Property DocType() As String
		Get
			Return m_sDocType
		End Get
		Set(ByVal Value As String)
			m_sDocType = Value
		End Set
	End Property
	Public Property DocTemp() As String
		Get
			'    DocTemp = m_sDocTemp
		End Get
		Set(ByVal Value As String)
			'    m_sDocTemp = sDocTemp
		End Set
	End Property
	
	' CJB 290802 Define Document Template ID too
	Public Property DocTempID() As Integer
		Get
			'    DocTempID = m_lDocTempID
		End Get
		Set(ByVal Value As Integer)
			'    m_lDocTempID = lDocTempID
		End Set
	End Property
	' CJB 290802 Define Document Template ID too
	
	Public WriteOnly Property DocTempArray() As Object
		Set(ByVal Value As Object)
			'    m_vDocTemp = vDocTemp
		End Set
	End Property
	
	
	Public WriteOnly Property ActionType() As gPMConstants.PMEComponentAction
		Set(ByVal Value As gPMConstants.PMEComponentAction)
			m_vActionType = Value
		End Set
	End Property
	
	Public Property DocTemplateType() As String
		Get
			Return m_sDocTemplateType
		End Get
		Set(ByVal Value As String)
			m_sDocTemplateType = Value
		End Set
	End Property
	
	
	Public Property DocTemplateTypeID() As Integer
		Get
			Return m_lDocTemplateTypeID
		End Get
		Set(ByVal Value As Integer)
			m_lDocTemplateTypeID = Value
		End Set
	End Property
	
	Public Property DocTemplate() As String
		Get
			Return m_sDocTemplate
		End Get
		Set(ByVal Value As String)
			m_sDocTemplate = Value
		End Set
	End Property
	
	Public Property DocTemplateID() As Integer
		Get
			Return m_lDocTemplateID
		End Get
		Set(ByVal Value As Integer)
			m_lDocTemplateID = Value
		End Set
	End Property
	
	Public Property AgentCode() As String
		Get
			Return m_sAgentCode
		End Get
		Set(ByVal Value As String)
			m_sAgentCode = Value
		End Set
	End Property
	
	Public Property SpoolDocuments() As Integer
		Get
			Return m_iSpoolDocs
		End Get
		Set(ByVal Value As Integer)
			chkSpoolDocument.CheckState = Value
			m_iSpoolDocs = Value
		End Set
	End Property
	
	Public Property AutoArchiveDocument() As Integer
		Get
			Return m_iAutoArchiveDocument
		End Get
		Set(ByVal Value As Integer)
			chkAutoArchiveDocument.CheckState = Value
			m_iAutoArchiveDocument = Value
		End Set
	End Property
	'CLG RFC71 2004-09-01
	Public WriteOnly Property SpoolAndArchiveMode() As Integer
		Set(ByVal Value As Integer)
			m_lSpoolAndArchiveMode = Value
		End Set
	End Property
	
	' ***************************************************************** '
	'
	' Name: InterfaceToProperties
	'
	' Description: Sets the properties from the form
	'
	' History: 01/02/02 - AK - Created.
	'
	' ***************************************************************** '
	Private Function InterfaceToProperties() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			m_sGISScheme = txtGISScheme.Text.Trim()
			m_sProcess = cboProcess.Text.Trim()
			m_lProcessID = VB6.GetItemData(cboProcess, cboProcess.SelectedIndex)
			m_sAgent = txtAgent.Text.Trim()
			m_iSpoolDocs = chkSpoolDocument.CheckState
			m_iAutoArchiveDocument = chkAutoArchiveDocument.CheckState
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InterfaceToProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	
	Private Sub cboProcess_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboProcess.SelectedIndexChanged
		
		' Do not allow agents to be attached to any renewal process as we have not yet
		' developed the system to cater for this when producing docs during these stages  PN1926
		If cboProcess.Text.Substring(0, 7) = "Renewal" Then
			txtAgent.Text = ""
			txtAgent.Enabled = False
			cmdFindAgent.Enabled = False
			lblAgent.ForeColor = SystemColors.ControlDark
		Else
			txtAgent.Enabled = True
			cmdFindAgent.Enabled = True
			lblAgent.ForeColor = Color.Black
		End If
		
	End Sub
	
	Private Sub chkAutoArchiveDocument_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkAutoArchiveDocument.CheckStateChanged
		m_iAutoArchiveDocument = chkAutoArchiveDocument.CheckState
		'CLG RFC71 2004-09-01
		chkSpoolDocument_CheckStateChanged(chkSpoolDocument, New EventArgs())
	End Sub
	
	Private Sub chkSpoolDocument_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkSpoolDocument.CheckStateChanged
		
		m_iSpoolDocs = chkSpoolDocument.CheckState
		
		'CLG RFC71 2004-09-01
		If m_lSpoolAndArchiveMode = 0 Then
			If m_iSpoolDocs = 1 Then
				chkAutoArchiveDocument.CheckState = CheckState.Unchecked
				chkAutoArchiveDocument.Enabled = False
			Else
				chkAutoArchiveDocument.Enabled = True
			End If
			
			'CLG RFC71 2004-09-01
			If m_iAutoArchiveDocument = 1 Then
				chkSpoolDocument.CheckState = CheckState.Unchecked
				chkSpoolDocument.Enabled = False
			Else
				chkSpoolDocument.Enabled = True
			End If
		End If
		
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		' Set the status to Cancel
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		
		' Hide the form
		Me.Hide()
		
	End Sub
	
	' ***************************************************************** '
	'
	' Name: CheckValid
	'
	' Description:
	'
	' History: 01/02/2001 AK - Created.
	'
	' ***************************************************************** '
	'UPGRADE_NOTE: (7001) The following declaration (CheckValid) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function CheckValid(ByVal txtCheck As TextBox) As Integer
		'
		'Dim result As Integer = 0
		'Dim vText As String = ""
		'
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			' Trim the text of the textbox
			'txtCheck.Text = txtCheck.Text.Trim()
			'
			' Get the text of the control
			'vText = txtCheck.Text
			'
			' If we have no text then it's ok
			'If vText = "" Then
				'
				'If m_bAllMandatory Then
					'result = gPMConstants.PMEReturnCode.PMFalse
					'
					' Display a message
					'MessageBox.Show("You must enter a value for " & Convert.ToString(txtCheck.Tag) & "." & Environment.NewLine & Environment.NewLine & "All values are mandatory for Global settings.", "Invalid data", MessageBoxButtons.OK, MessageBoxIcon.Information)
					'
					' Set focus to the control
					'txtCheck.Focus()
					'
				'End If
				'
				'Return result
				'
			'End If
			'
			' Make sure its a number
			'Dim dbNumericTemp As Double
			'If Not Double.TryParse(vText, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
				'result = gPMConstants.PMEReturnCode.PMFalse
				'
				' Display a message
				'MessageBox.Show("You must enter a numeric value for " & Convert.ToString(txtCheck.Tag) & ".", "Invalid data", MessageBoxButtons.OK, MessageBoxIcon.Information)
				'
				' Set focus to the control
				'txtCheck.Focus()
				'
				'Return result
			'End If
			'
			' Make sure its positive
			'If CInt(vText) < 0 Then
				'result = gPMConstants.PMEReturnCode.PMFalse
				'
				' Display a message
				'MessageBox.Show("You must enter a positive value for " & Convert.ToString(txtCheck.Tag) & ".", "Invalid data", MessageBoxButtons.OK, MessageBoxIcon.Information)
				'
				' Set focus to the control
				'txtCheck.Focus()
				'
				'Return result
			'End If
			'
			'Return result
		'
		'Catch excep As System.Exception
			'
			'
			'
			'result = gPMConstants.PMEReturnCode.PMError
			'
			' Log Error Message
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckValid Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckValid", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Return result
			'
		'End Try
	'End Function
	
	
	' ***************************************************************** '
	'
	' Name: ValidateData
	'
	' Description:
	'
	' History: 01/02/2001 AK - Created.
	'
	' ***************************************************************** '
	'UPGRADE_NOTE: (7001) The following declaration (ValidateData) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function ValidateData() As Integer
		'
		'Dim result As Integer = 0
		'Dim txtBox As Object
		'
		'Try 
			'
			'
			'Return gPMConstants.PMEReturnCode.PMTrue
		'
		'Catch excep As System.Exception
			'
			'
			'
			'result = gPMConstants.PMEReturnCode.PMError
			'
			' Log Error Message
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Return result
			'
		'End Try
	'End Function
	
	
	Private Sub cmdFindAgent_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindAgent.Click
		Try 
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			m_lReturn = LaunchFindAgent()
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				txtAgent.Text = m_sAgentCode ' m_sAgent
			End If
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
			
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdFindDocumentTemplate_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdFindDocumentTemplate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub


		End Try
	End Sub
	
	Private Sub cmdFindDocumentTemplate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindDocumentTemplate.Click
		Try 
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			m_lReturn = LaunchFindDocumentTemplate()
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				txtDocType.Text = m_sDocTemplateType
				txtDocumentTemplate.Text = m_sDocTemplate
			End If
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
			
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdFindDocumentTemplate_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdFindDocumentTemplate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub


		End Try
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		' Set the status to OK
		m_lStatus = gPMConstants.PMEReturnCode.PMOK
		
		m_lReturn = ValidateOK()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			Exit Sub
		End If
		
		' Set the properties back
		m_lReturn = InterfaceToProperties()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			Exit Sub
		End If
		
		' Hide the form
		Me.Hide()
		
	End Sub
	
	' ***************************************************************** '
	'
	' Name: DataToInterface
	'
	' Description: Set the properties on the form
	'
	' History: 09/03/2001 CTAF - Created.
	'
	' ***************************************************************** '
	Public Function DataToInterface() As Integer
		
		Dim result As Integer = 0

		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			txtGISScheme.Text = m_sGISScheme ' Read Only
			
			Select Case m_vActionType
				Case gPMConstants.PMEComponentAction.PMAdd
					txtDocType.Text = ""
					txtDocumentTemplate.Text = ""
					txtAgent.Text = ""
					m_lReturn = PopulateProcessTypeCombobox() ' Populate the Process Type combo box
					
				Case gPMConstants.PMEComponentAction.PMEdit
					txtDocType.Text = m_sDocTemplateType ' Read Only
					txtDocumentTemplate.Text = m_sDocTemplate
					txtAgent.Text = m_sAgent
					' Populate the Process Type combo box with the value selected
					m_lReturn = PopulateProcessTypeCombobox(vSelectedValue:=m_sProcess)
			End Select
			
			' Set the form's caption
			Me.Text = m_sGISScheme & " - " & m_sProcess
			If m_sAgent.Length > 0 Then
				Me.Text = Me.Text & " - " & m_sAgent
			End If
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DataToInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result


			Return result
		End Try
	End Function
	
	Public Function PopulateProcessTypeCombobox(Optional ByVal vSelectedValue As String = "") As gPMConstants.PMEReturnCode
		
		Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get the exisiting Process Types

			m_lReturn = m_oBusiness.GetProcessType(r_vProcessType:=m_vProcess)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = m_lReturn
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to retrieve Process-Type data from business object.", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateProcessTypeCombobox", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				Return result
			End If
			
			'Populate process combobox
			If Information.IsArray(m_vProcess) Then
				cboProcess.Items.Clear()
				For iNum As Integer = 0 To m_vProcess.GetUpperBound(1)
					Dim cboProcess_NewIndex As Integer = -1
					cboProcess_NewIndex = cboProcess.Items.Add(CStr(m_vProcess(ACArrayProcess, iNum)))
					VB6.SetItemData(cboProcess, cboProcess_NewIndex, CInt(m_vProcess(ACArrayProcessID, iNum)))
				Next 
			End If
			

			If Information.IsNothing(vSelectedValue) Then
				'Default to the first entry in the list
				cboProcess.SelectedIndex = 0
			Else
				For iNum As Integer = 0 To cboProcess.Items.Count
					If VB6.GetItemString(cboProcess, iNum).Trim().ToUpper() = vSelectedValue.Trim().ToUpper() Then
						cboProcess.SelectedIndex = iNum
						Exit For
					End If
				Next iNum
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateProcessTypeCombobox Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateProcessTypeCombobox", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result


			Return result
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: Initialise
	'
	' Description:
	'
	' History: 09/03/2001 CTAF - Created.
	'
	' ***************************************************************** '
	Public Function Initialise() As Integer



		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Set the form fields
			m_lReturn = SetFieldValidation()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set the properties on the form
			m_lReturn = DataToInterface()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result


			Return result
		End Try
	End Function
	
	Public Function SetFieldValidation() As Integer
		
		Dim result As Integer = 0
		Try 
			
			' {* USER DEFINED CODE (Begin) *}
			result = gPMConstants.PMEReturnCode.PMTrue
			
			m_oFormFields = New iPMFormControl.FormFields()
			
			m_lReturn = CType(m_oFormFields, SSP.S4I.Interfaces.ILocalInterface).Initialise()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set the language ID
			m_oFormFields.LanguageID = g_oObjectManager.LanguageID
			
			
			'GIS Scheme (Mandatory).
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtGISScheme, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Document Type     -  txtDocType
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDocType, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Document Template - txtDocumentTemplate
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDocumentTemplate, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Process - cboProcess
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboProcess, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result


			Return result
		End Try
	End Function
	
	Private Sub frmDetails_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		
		' Remove all the objects
		m_oFormFields.Dispose()
		
		m_oFormFields = Nothing
		
	End Sub
	
	Private Function LaunchFindDocumentTemplate() As Integer
		Dim result As Integer = 0
		Dim oFindDocTemplate As iPMBFindDocTemplate.Interface_Renamed
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			Dim temp_oFindDocTemplate As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_oFindDocTemplate, sClassName:="iPMBFindDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
			oFindDocTemplate = temp_oFindDocTemplate
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return m_lReturn
			End If
			

			m_lReturn = oFindDocTemplate.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView)

			oFindDocTemplate.Mode = 1
			

			m_lReturn = oFindDocTemplate.Start()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				oFindDocTemplate = Nothing
				Return result
			End If
			

			If oFindDocTemplate.Status = gPMConstants.PMEReturnCode.PMOK Then
				With oFindDocTemplate

					m_lDocTemplateID = .DocumentTemplateId

					m_sDocTemplateCode = .DocumentCode

					m_sDocTemplate = .DocumentTemplateDescription

					m_lDocTemplateTypeID = .DocumentTypeID

					m_sDocTemplateTypeCode = .DocumentTypeCode

					m_sDocTemplateType = .DocumentTypeDescription
				End With
			End If
			

            oFindDocTemplate.Dispose()
			oFindDocTemplate = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			If oFindDocTemplate Is Nothing Then
			Else

                oFindDocTemplate.Dispose()
				oFindDocTemplate = Nothing
			End If
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Launch FindDocumentTemplate Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="LaunchFindDocumentTemplate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result


			Return result
		End Try
	End Function
	
	Private Function LaunchFindAgent() As Integer
		Dim result As Integer = 0
		Dim oFindAgent As iPMBFindParty.Interface_Renamed
        Dim vKeyArray(,) As Object
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			Dim temp_oFindAgent As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_oFindAgent, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
			oFindAgent = temp_oFindAgent
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return m_lReturn
			End If
			

			m_lReturn = oFindAgent.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView)

			oFindAgent.CallingAppName = ACApp & "." & ACClass
			
			' Set the SetKeys
			ReDim vKeyArray(1, 0)

			vKeyArray(0, 0) = "special_party"

			vKeyArray(1, 0) = "AG" ' Which filters the Agents

			m_lReturn = oFindAgent.SetKeys(vKeyArray)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				oFindAgent = Nothing
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetKeys to FindAgent Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="LaunchFindAgent", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				Return result
			End If
			

			m_lReturn = oFindAgent.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView)
			
			' You can not add or edit an Agent through this program

			oFindAgent.NotEditable = 1
			

			m_lReturn = oFindAgent.Start()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				oFindAgent = Nothing
				Return result
			End If
			
			
			'TR - Make sure that the search screen exited ok & user did not cancel

			If oFindAgent.Status = gPMConstants.PMEReturnCode.PMOK Then
				With oFindAgent

					m_lAgentCnt = .PartyCnt

					m_sAgentCode = .ShortName

					m_sAgent = .LongName
				End With
			End If
			
            oFindAgent.Dispose()
            oFindAgent = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			If oFindAgent Is Nothing Then
            Else

                oFindAgent.Dispose()
                oFindAgent = Nothing
			End If
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Launch FindAgent Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="LaunchFindAgent", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
	
	
	Private Function ValidateOK() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			If txtGISScheme.Text = "" Then
				MessageBox.Show("Scheme Details Missing. Please select a scheme", "Missing Scheme Details", MessageBoxButtons.OK, MessageBoxIcon.Error)
				Return result
			End If
			
			If txtDocType.Text = "" Then
				MessageBox.Show("Document Template Type Details Missing. Please select a Document Template", "Missing Document TempalteType Details", MessageBoxButtons.OK, MessageBoxIcon.Error)
				Return result
			End If
			
			If txtDocumentTemplate.Text = "" Then
				MessageBox.Show("Document Template Details Missing. Please select a Document Template", "Missing Document Tempalte Details", MessageBoxButtons.OK, MessageBoxIcon.Error)
				Return result
			End If
			
			If cboProcess.Text = "" Then
				MessageBox.Show("Process Details Missing. Please select a Process", "Missing Process Details", MessageBoxButtons.OK, MessageBoxIcon.Error)
				Return result
			End If
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateOK Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateOK", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
End Class
