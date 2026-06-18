Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmDetails
	Inherits System.Windows.Forms.Form
	
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "frmDetails"
	
	'********************************
	' General Property variables
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lPMAuthorityLevel As Integer
	Private m_bError As Integer
	Private m_oBusiness As Object
	Private m_lReturn As gPMConstants.PMEReturnCode
	Private m_bInterfaceError As Boolean
	'********************************
	
	
	Private m_oMaintainData As MaintainData
	Private m_lActionType As Integer
	Private m_vDocumentTemplates As Object
	Private m_lReturnType As Integer
	Private m_vCodes As Object
    Private m_vTaskOutcomes As Object
	
	'***********************
	
	Public Property MaintainData() As MaintainData
		Get
			Return m_oMaintainData
		End Get
		Set(ByVal Value As MaintainData)
			m_oMaintainData = Value
		End Set
	End Property
	'***********************
	Public WriteOnly Property TaskOutcomes() As Object()
		Set(ByVal Value() As Object)
			m_vTaskOutcomes = Value
		End Set
	End Property
	'***********************
	Public Property ActionType() As Integer
		Get
			Return m_lActionType
		End Get
		Set(ByVal Value As Integer)
			m_lActionType = Value
		End Set
	End Property
	'***********************
	Public WriteOnly Property DocumentTemplates() As Object
		Set(ByVal Value As Object)


			m_vDocumentTemplates = Value
		End Set
	End Property
	'***********************
	
	Public ReadOnly Property ReturnType() As Integer
		Get
			Return m_lReturnType
		End Get
	End Property
	'***********************
	Public ReadOnly Property Error_Renamed() As Boolean
		Get
			Return m_bError
		End Get
	End Property
	'***********************
	Public WriteOnly Property Codes() As Object
		Set(ByVal Value As Object)


			m_vCodes = Value
		End Set
	End Property
	
	
	' ***************************************************************** '
	' Name: GetResourceData
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 23-06-2003 : WorkFlow
	' ***************************************************************** '
	Private Function GetResourceData() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "GetResourceData"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			Me.Text = GetResString(ACResDataDetailsTitle)
			tabMainTab.SelectedTab.Text = GetResString(ACResDataDetailsTabActionType)
			
			cmdOK.Text = GetResString(ACResDataDetailsButtonOK)
			cmdCancel.Text = GetResString(ACResDataDetailsButtonCancel)
			cmdOutcomes.Text = GetResString(ACResDataDetailsButtonOutcomes)
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		m_lReturnType = ACReturnCancel
		Me.Close()
	End Sub
	
	'UPGRADE_NOTE: (7001) The following declaration (cmdClear_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub cmdClear_Click()
		'cboTemplate.ListIndex = -1
	'End Sub
	
	Private Sub cmdFindDocTemplate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindDocTemplate.Click
		m_lReturn = SelectDocumentTemplate()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		m_lReturnType = ACReturnOk
		
		If ActionOK() <> gPMConstants.PMEReturnCode.PMTrue Then
			
		End If
		
		
	End Sub
	
	Private Sub cmdOutcomes_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOutcomes.Click
		m_lReturn = ActionOutcomes()
	End Sub
	

	Private Sub frmDetails_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		' set up interface
		m_lReturn = SetUpForm()
		
	End Sub
	
	' ***************************************************************** '
	' Name: SetUpForm
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 23-06-2003 : WorkFlow
	' ***************************************************************** '
	Private Function SetUpForm() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "SetUpForm"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			m_lReturn = GetResourceData()
			
			m_lReturn = SetMandatoryFields()
			
			m_lReturn = SetReadOnlyFields()
			
			m_lReturn = PopulateForm()
			
			m_lReturn = SetupButtons()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: PopulateForm
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 24-06-2003 : workflow
	' ***************************************************************** '
	Private Function PopulateForm() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "PopulateForm"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If m_lActionType = ACActionEdit Or m_lActionType = ACActionView Then
				
				txtCode.Text = m_oMaintainData.Code
				txtDescription.Text = m_oMaintainData.Description
				txtEffectiveDate.Text = DateTimeHelper.ToString(m_oMaintainData.EffectiveDate)
				
				txtDueDays.Text = CStr(m_oMaintainData.DueDays)
				
				If m_oMaintainData.OutcomeEditable Then
					chkOutcomeEditable.CheckState = CheckState.Checked
				Else
					chkOutcomeEditable.CheckState = CheckState.Unchecked
				End If
				
				txtDocTemplate.Text = m_oMaintainData.DocumentTemplateCode
				
			End If
			
			' populate the check box for all actions
			'm_lReturn = PopulateDocumentTemplatesCombo(v_sItemCode:=m_oMaintainData.DocumentTemplateCode)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: ActionOK
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 24-06-2003: workflow
	' ***************************************************************** '
	Private Function ActionOK() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "ActionOK"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If PopulateObject() = gPMConstants.PMEReturnCode.PMTrue Then
				Me.Close()
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: ActionCancel
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 24-06-2003 : workflow
	' ***************************************************************** '
	'UPGRADE_NOTE: (7001) The following declaration (ActionCancel) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function ActionCancel() As Integer
		'
		'Dim result As Integer = 0
		'Const sFunctionName As String = "ActionCancel"
		'
		'Try 
			'
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
			'******************************
			' Log Error.
			'gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
			'*******************************
			'
			'Return result
			'
		'End Try
	'End Function
	
	
	
	' ***************************************************************** '
	' Name: ActionOutcomes
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 24-06-2003 : workflow
	' ***************************************************************** '
	Function ActionOutcomes() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "ActionOutcomes"
		
		Dim ofrmOutcomes As frmOutcomes
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'  create new instance of frmoutcomes
			ofrmOutcomes = New frmOutcomes()
			
			' set properties
			With ofrmOutcomes
				.MaintainITem = m_oMaintainData
				.ActionType = m_lActionType
				.TaskOutcomes = VB6.CopyArray(m_vTaskOutcomes)
			End With
			
			' load

            'Load(ofrmOutcomes)
			
			' show
			ofrmOutcomes.ShowDialog()
			
			' get the changes back already
			If m_lActionType = ACActionEdit Or m_lActionType = ACActionAdd Then
				m_oMaintainData = ofrmOutcomes.MaintainITem
			End If
			
			' unload
			ofrmOutcomes.Close()
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
		
		End Try
		
		
		
		Return result
		
	End Function
	
	' ***************************************************************** '
	' Name: PopulateDocumentTemplatesCombo
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 24-06-2003 : workflow
	' ***************************************************************** '
	'Private Function PopulateDocumentTemplatesCombo(ByVal v_sItemCode As String) As Long
	'
	'    Const sFunctionName = "PopulateDocumentTemplatesCombo"
	'
	'    Dim lItem As Long
	'    Dim llbound As Long
	'    Dim lUbound As Long
	'    Dim lItemIndex As Long
	'
	'    On Error GoTo Err_PopulateDocumentTemplatesCombo
	'
	'    PopulateDocumentTemplatesCombo = PMTrue
	'
	'    ' initialise
	'    lItemIndex = -1
	'
	'    ' disable the control
	'    cboTemplate.Enabled = False
	'
	'    ' if there are document templates
	'    If IsArray(m_vDocumentTemplates) Then
	'
	'        ' get array boundaries
	'        llbound = LBound(m_vDocumentTemplates, 2)
	'        lUbound = UBound(m_vDocumentTemplates, 2)
	'
	'        If m_lActionType = ACActionEdit Or m_lActionType = ACActionAdd Then
	'            cboTemplate.AddItem ""
	'        End If
	'
	'        ' for each item in the array
	'        For lItem = llbound To lUbound
	'
	'             ' add the item to the combo
	'            cboTemplate.AddItem Trim$(m_vDocumentTemplates(ACDocTemplateDescription, lItem))
	'
	'            ' get the listindex of the item we have already selected
	'            If v_sItemCode = Trim$(m_vDocumentTemplates(ACDocTemplateCode, lItem)) Then
	'                lItemIndex = lItem
	'            End If
	'
	'        Next lItem
	'
	'        ' if we are in edit mode or add mode
	'        If m_lActionType = ACActionEdit Or m_lActionType = ACActionAdd Then
	'            cboTemplate.Enabled = True
	'        End If
	'    End If
	'
	'    ' if we have found a match, selected the specified item
	'    If lItemIndex <> -1 Then
	'        cboTemplate.ListIndex = lItemIndex
	'    End If
	'
	'    Exit Function
	'
	'Err_PopulateDocumentTemplatesCombo:
	'
	'    PopulateDocumentTemplatesCombo = PMError
	'
	'    '******************************
	'    ' Log Error.
	'    LogMessageToFile _
	''        sUserName:=g_oObjectManager.UserName, _
	''        iType:=PMLogOnError, _
	''        sMsg:=sFunctionName & " Failed", _
	''        vApp:=ACApp, _
	''        vClass:=ACClass, _
	''        vMethod:=sFunctionName, _
	''        vErrNo:=Err.Number, _
	''        vErrDesc:=Err.Description
	'    '*******************************
	'
	'    Exit Function
	'
	'End Function
	
	
	
	
	' ***************************************************************** '
	' Name: PopulateCbo
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 03-06-2003 : 223
	' ***************************************************************** '
	'UPGRADE_NOTE: (7001) The following declaration (PopulateCbo) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function PopulateCbo(ByRef r_vDataArray() As Object, ByRef r_oObject As ComboBox) As Integer
		'
		'Dim result As Integer = 0
		'Const sFunctionName As String = "PopulateCbo"
		'
		'Dim llbound, lUbound As Integer
		'
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			' clear down the combo
			'r_oObject.Items.Clear()
			'
			' if we have a populates array
			'If Information.IsArray(r_vDataArray) Then
				'
				' get array boundaries
				'llbound = r_vDataArray.GetLowerBound(0)
				'lUbound = r_vDataArray.GetUpperBound(0)
				'
				' for each item in the array
				'For 'lItem As Integer = llbound To lUbound
					'
					' add the item to the specified combo

					'r_oObject.Items.Add(CStr(r_vDataArray(lItem)).Trim())
					'
				'Next lItem
				'
				' enable the combo
				'r_oObject.Enabled = True
				'
			'Else
				' no data so disable the combo
				'r_oObject.Enabled = False
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
			'******************************
			' Log Error.
			'gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
			'*******************************
			'
			'Return result


			'Return result
		'End Try
	'End Function
	
	
	' ***************************************************************** '
	' Name: GetLookupItem
	'
	' Parameters: n/a
	'
	' Description: Returns all the item information for a specifed item
	'                  in a specified array
	'
	' History:
	'           Created : MEvans : 24-06-2003 : workflow
	' ***************************************************************** '
	'UPGRADE_NOTE: (7001) The following declaration (GetArrayItem) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function GetArrayItem(ByVal v_vArray( ,  ) As Object, ByVal r_sItemDesc As String, ByRef r_sItemCode As String, ByRef r_lItemId As Integer) As Integer
		'
		'Dim result As Integer = 0
		'Const sFunctionName As String = "GetArrayItem"
		'
		'
		'Dim v_vLookupItem As String = ""
		'Dim lLookupItem, llbound, lUbound As Integer
		'
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			' set lookup properties
			'If r_lItemId <> 0 Then
				'v_vLookupItem = CStr(r_lItemId)
				'lLookupItem = 0
				'
			'ElseIf r_sItemDesc <> "" Then 
				'v_vLookupItem = r_sItemDesc
				'lLookupItem = 1
				'
			'ElseIf r_sItemCode <> "" Then 
				'v_vLookupItem = r_sItemCode
				'lLookupItem = 2
			'End If
			'
			'
			'llbound = v_vArray.GetLowerBound(1)
			'lUbound = v_vArray.GetUpperBound(1)
			'
			' loop around the available items in the specified array
			'For 'lItem As Integer = llbound To lUbound
				'
				' look for a match

				'If CStr(v_vArray(lLookupItem, lItem)).Trim() = v_vLookupItem Then
					'
					' return the requested code, id, description

					'r_sItemDesc = CStr(v_vArray(ACDetailDesc, lItem)).Trim()

					'r_sItemCode = CStr(v_vArray(ACDetailCode, lItem)).Trim()

					'r_lItemId = CInt(CStr(v_vArray(ACDetailKey, lItem)).Trim())
					'
					'Exit For
				'End If
				'
			'Next lItem
			'
			' if we dont find the values specified then return false
			'If r_sItemCode = "" Then
				'
				'result = gPMConstants.PMEReturnCode.PMFalse
				'
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
			'******************************
			' Log Error.
			'gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
			'*******************************
			'
			'Return result


			'Return result
		'End Try
	'End Function
	
	
	' ***************************************************************** '
	' Name: GetResString
	'
	' Parameters: n/a
	'
	' Description: Returns string item from resource file
	'
	' History:
	'           Created : MEvans : Date : Process Id
	' ***************************************************************** '
	Private Function GetResString(ByVal v_lItemId As Integer) As String
		
		Dim result As String = String.Empty
		Const sFunctionName As String = "GetResString"
		
		Dim sReturn As String = ""
		
		Try 
			
			' always want to return a string

			sReturn = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=v_lItemId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			
			Return sReturn
		
		Catch excep As System.Exception
			
			
			
			result = "Error"
			
			'******************************
			' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
			oDict.Add("v_lItemId", v_lItemId)
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: PopulateObject
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 24-06-2003 : workflow
	' ***************************************************************** '
	Private Function PopulateObject() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "PopulateObject"
		
		Dim sMessageTitle, sMessage As String
		Dim bError As Boolean
		Dim scode As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' set the default message title
			sMessageTitle = GetResString(ACResDataDetailsTitle)
			
			' Code
			' if we are in add action then
			If m_lActionType = ACActionAdd Then

				If ItemInArray(r_vArray:=m_vCodes, v_sItemValue:=txtCode.Text) <> gPMConstants.PMEReturnCode.PMTrue Then
					sMessage = GetResString(ACResDataDetailsMessageInvalidCode)
					MessageBox.Show(sMessage, sMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
					txtCode.Focus()
					bError = True
				End If
			End If
			
			If Not bError Then
				m_oMaintainData.Code = txtCode.Text
			End If
			
			' Description
			If Not bError Then
				m_oMaintainData.Description = txtDescription.Text
			End If
			
			' Effective Date
			If Not bError Then
				If Information.IsDate(txtEffectiveDate.Text) Then
					m_oMaintainData.EffectiveDate = CDate(txtEffectiveDate.Text)
				Else
					sMessage = GetResString(ACResDataDetailsMessageInvalidDate)
					MessageBox.Show(sMessage, sMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
					txtEffectiveDate.Focus()
					bError = True
				End If
			End If
			
			' Due Days
			If Not bError Then
				Dim dbNumericTemp As Double
				If Double.TryParse(txtDueDays.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Or txtDueDays.Text = "" Then

					m_oMaintainData.DueDays = CInt(ConvertFromNullValues(txtDueDays, gPMConstants.PMEDataType.PMLong))
				Else
					sMessage = GetResString(ACResDataDetailsMessageInvalidDueDays)
					MessageBox.Show(sMessage, sMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
					txtDueDays.Focus()
					bError = True
				End If
			End If
			
			' Document Template Code
			If Not bError Then
				If txtDocTemplate.Text <> "" Then
					m_oMaintainData.DocumentTemplateCode = txtDocTemplate.Text.Trim()
					'            If GetArrayItem(v_vArray:=m_vDocumentTemplates, _
					''                            r_sItemDesc:=cboTemplate.Text, _
					''                            r_sItemCode:=scode, _
					''                            r_lItemId:=0) = PMTrue Then
					'                m_oMaintainData.DocumentTemplateCode = scode
					'            Else
					'                m_oMaintainData.DocumentTemplateCode = ""
					'            End If
				Else
					m_oMaintainData.DocumentTemplateCode = ""
				End If
			End If
			
			' Outcome Not Editable
			If Not bError Then
				m_oMaintainData.OutcomeNotEditable = (chkOutcomeEditable.CheckState = CheckState.Unchecked)
			End If
			
			'
			If m_lActionType = ACActionAdd Then
				' copies the current functional values to the original properties
				' so comparisons can determine if any values have been updated
				m_lReturn = m_oMaintainData.CopyToOriginalData()
			End If
			
			If bError Then
				result = gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: ConvertFromNullValues
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 05-06-2003 : 223
	' ***************************************************************** '
	Private Function ConvertFromNullValues(ByRef r_vValue As TextBox, ByVal v_iDataType As Integer) As Object
		
		Dim result As Object = Nothing
		Const sFunctionName As String = "ConvertFromNullValues"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			
			
			Select Case v_iDataType
				Case gPMConstants.PMEDataType.PMLong, gPMConstants.PMEDataType.PMCurrency, gPMConstants.PMEDataType.PMBoolean, gPMConstants.PMEDataType.PMDate

					If r_vValue.Text Is DBNull.Value Or r_vValue.Text = "" Then
						Return 0
					Else
						Return r_vValue.Text
					End If
					
				Case Else
					Return r_vValue.Text
					
			End Select
		
		Catch 
		End Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMError
		
		'******************************
		' Log Error.
        gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))
		'*******************************
		
		Return result
		
	End Function
	
	' ***************************************************************** '
	' Name:SetMandatoryFields
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : Date : Process ID
	' ***************************************************************** '
	Private Function SetMandatoryFields() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "SetMandatoryFields"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			lblCode.Font = VB6.FontChangeBold(lblCode.Font, True)
			lblDescription.Font = VB6.FontChangeBold(lblDescription.Font, True)
			lblEffectiveDate.Font = VB6.FontChangeBold(lblEffectiveDate.Font, True)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: SetReadOnlyFields
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 24-06-2003 : workflow
	' ***************************************************************** '
	Private Function SetReadOnlyFields() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "SetReadOnlyFields"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			
			Select Case m_lActionType
				Case ACActionEdit
					txtCode.Enabled = False
					
				Case ACActionView
					txtCode.Enabled = False
					txtDescription.Enabled = False
					txtEffectiveDate.Enabled = False
					txtDueDays.Enabled = False
					'cboTemplate.Enabled = False
					chkOutcomeEditable.Enabled = False
					cmdFindDocTemplate.Enabled = False
					
			End Select
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: ItemInArray
	'
	' Parameters: n/a
	'
	' Description: Checks the array to see if the specified item
	'                 already exists
	' History:
	'           Created : MEvans : 03-06-2003 : 223
	' ***************************************************************** '
	Private Function ItemInArray(ByRef r_vArray() As Object, ByVal v_sItemValue As String) As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "ItemInArray"
		
		Dim llbound, lUbound As Integer
		Dim bItemExists As Boolean
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' create new collection
			If Information.IsArray(r_vArray) Then
				
				' get array boundaries
				llbound = r_vArray.GetLowerBound(0)
				lUbound = r_vArray.GetUpperBound(0)
				
				' for each item in the array
				For lItem As Integer = llbound To lUbound
					
					' check if we match the values from the array specified

					If CStr(r_vArray(lItem)).Trim() = v_sItemValue.Trim() Then
						' indicate item already exists
						bItemExists = True
						
						' quit loop
						lItem = lUbound
					End If
					
				Next lItem
				
			End If
			
			' if the item doesnt already exist
			' add it to the array
			If bItemExists Then
				result = gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result


			Return result
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: SetupButtons
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 23-06-2003 : Workflow
	' ***************************************************************** '
	Private Function SetupButtons() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "SetupButtons"
		
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
	End Function
	
	Private Sub txtEffectiveDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEffectiveDate.Leave
		If Information.IsDate(txtEffectiveDate.Text) Then
			txtEffectiveDate.Text = CDate(txtEffectiveDate.Text).ToString("dd/MM/yyyy")
		End If
	End Sub
	
	' ***************************************************************** '
	' Name: SelectDocumentTemplate
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 30-07-2003 : 223 document production
	' ***************************************************************** '
	Function SelectDocumentTemplate() As Integer
		Dim result As Integer = 0
        Const sFunctionName As String = "SelectDocumentTemplate"
		

		Dim oFindDocTemplate As iPMBFindDocTemplate.Interface_Renamed
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' create instance of find doc template
			Dim temp_oFindDocTemplate As Object
			If g_oObjectManager.GetInstance(temp_oFindDocTemplate, sClassName:="iPMBFindDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface) = gPMConstants.PMEReturnCode.PMTrue Then
				oFindDocTemplate = temp_oFindDocTemplate
				
				

				oFindDocTemplate.CallingAppName = ACApp
				
				' set process modes

				If oFindDocTemplate.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit) = gPMConstants.PMEReturnCode.PMTrue Then
					

					oFindDocTemplate.Mode = 1
					

					If oFindDocTemplate.Start() <> gPMConstants.PMEReturnCode.PMTrue Then
						
						result = gPMConstants.PMEReturnCode.PMFalse
						
                        ' Log Error.
                        gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to start iPMBFindDocTemplate.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
					Else
						

						If oFindDocTemplate.Status = gPMConstants.PMEReturnCode.PMOK Then

							m_oMaintainData.DocumentTemplateCode = oFindDocTemplate.DocumentCode
							txtDocTemplate.Text = m_oMaintainData.DocumentTemplateCode
							'm_sDocumentTemplateCode = oFindDocTemplate.DocumentCode
							'm_lDocumentTemplateId = oFindDocTemplate.DocumentTemplateId
						End If
						
					End If
				Else
					
					result = gPMConstants.PMEReturnCode.PMFalse
					
					' Log Error.
					gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to set process modes for iPMBFindDocTemplate.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
				End If
			Else
				oFindDocTemplate = temp_oFindDocTemplate
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to create instance of iPMBFindDocTemplate.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
				
			End If
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
		
		Finally 
			' destroy object instance
		End Try
		
		
		
		Return result
		
	End Function
End Class