Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
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
	Private m_lReturnType As Integer
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
	Public WriteOnly Property TaskOutcomes() As Object
		Set(ByVal Value As Object)


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
			
			fraDefaults.Text = GetResString(ACResDataDetailsFrameDefaults)
			
			lblCode.Text = GetResString(ACResDataDetailsLabelCode)
			lblDescription.Text = GetResString(ACResDataDetailsLabelDescription)
			lblCompletionTask.Text = GetResString(ACResDataDetailsLabelCompletionTask)
			lblIncompleteTask.Text = GetResString(ACResDataDetailsLabelIncompleteTask)
			lblActionRequired.Text = GetResString(ACResDataDetailsLabelActionRequired)
			
			cmdOK.Text = GetResString(ACResDataDetailsButtonOK)
			cmdCancel.Text = GetResString(ACResDataDetailsButtonCancel)
			
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
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		m_lReturnType = ACReturnOk
		
		If ActionOK() <> gPMConstants.PMEReturnCode.PMTrue Then
			
		End If
		
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
				
				If m_oMaintainData.ActionRequired Then
					chkActionRequired.CheckState = CheckState.Checked
				Else
					chkActionRequired.CheckState = CheckState.Unchecked
				End If
				
				'*********
				' MEvans : 20-10-2003 : Continuation Tasks
				If m_oMaintainData.AutoUpdateBatch Then
					chkAutoUpdateBatch.CheckState = CheckState.Checked
				Else
					chkAutoUpdateBatch.CheckState = CheckState.Unchecked
				End If
				'*********
				
			End If
			
			' populate the check box for all actions

			m_lReturn = CType(PopulateCombo(r_cbo:=cboCompletionTask, v_vDataArray:=m_vTaskOutcomes, v_lSelectedItemId:=m_oMaintainData.CompletionTaskId), gPMConstants.PMEReturnCode)
			
			' populate the check box for all actions

			m_lReturn = CType(PopulateCombo(r_cbo:=cboIncompleteTask, v_vDataArray:=m_vTaskOutcomes, v_lSelectedItemId:=m_oMaintainData.IncompleteTaskId), gPMConstants.PMEReturnCode)
			
			
			
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
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			'Me.Close()
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
			'
		'End Try
	'End Function
	
	
	' ***************************************************************** '
	' Name: PopulateCombo
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 24-06-2003 : workflow
	' ***************************************************************** '
	Private Function PopulateCombo(ByRef r_cbo As ComboBox, ByVal v_vDataArray( ,  ) As Object, ByVal v_lSelectedItemId As Integer) As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "PopulateCombo"
		
		Dim llbound, lUbound, lItemIndex As Integer
		Dim bBlankItemIncluded As Boolean
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' initialise
			lItemIndex = -1
			
			' disable the control
			r_cbo.Enabled = False
			
			
			' if there are document templates
			If Information.IsArray(v_vDataArray) Then
				
				' get array boundaries
				llbound = v_vDataArray.GetLowerBound(1)
				lUbound = v_vDataArray.GetUpperBound(1)
				
				If m_lActionType = ACActionEdit Or m_lActionType = ACActionAdd Then
					r_cbo.Items.Add("")
					bBlankItemIncluded = True
				End If
				
				' for each item in the array
				For lItem As Integer = llbound To lUbound
					
					' add the item to the combo

					r_cbo.Items.Add(CStr(v_vDataArray(ACTaskOutcomePosDescription, lItem)).Trim())
					
					' get the listindex of the item we have already selected

					If v_lSelectedItemId = StringsHelper.ToDoubleSafe(CStr(v_vDataArray(ACTaskOutcomePosId, lItem)).Trim()) Then
						' because we have included a blank item
						' need to increment the list index of the selected item to find the
						' correct item
						If bBlankItemIncluded Then
							lItemIndex = lItem + 1
						Else
							lItemIndex = lItem
						End If
					End If
					
				Next lItem
				
				' if we are in edit mode or add mode
				If m_lActionType = ACActionEdit Or m_lActionType = ACActionAdd Then
					r_cbo.Enabled = True
				End If
			End If
			
			' if we have found a match, selected the specified item
			If lItemIndex <> -1 Then
				r_cbo.SelectedIndex = lItemIndex
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
			oDict.Add("v_lSelectedItemId", v_lSelectedItemId)
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	
	
	
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
	Private Function GetArrayItem(ByVal v_vArray( ,  ) As Object, ByVal r_sItemDesc As String, ByRef r_sItemCode As String, ByRef r_lItemId As Integer) As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "GetArrayItem"
		
		
		Dim v_vLookupItem As String = ""
		Dim lLookupItem, llbound, lUbound As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' set lookup properties
			If r_lItemId <> 0 Then
				v_vLookupItem = CStr(r_lItemId)
				lLookupItem = 0
				
			ElseIf r_sItemDesc <> "" Then 
				v_vLookupItem = r_sItemDesc
				lLookupItem = 1
				
			ElseIf r_sItemCode <> "" Then 
				v_vLookupItem = r_sItemCode
				lLookupItem = 2
			End If
			
			
			llbound = v_vArray.GetLowerBound(1)
			lUbound = v_vArray.GetUpperBound(1)
			
			' loop around the available items in the specified array
			For lItem As Integer = llbound To lUbound
				
				' look for a match

				If CStr(v_vArray(lLookupItem, lItem)).Trim() = v_vLookupItem Then
					
					' return the requested code, id, description

					r_sItemDesc = CStr(v_vArray(ACDetailDesc, lItem)).Trim()

					r_sItemCode = CStr(v_vArray(ACDetailCode, lItem)).Trim()

					r_lItemId = CInt(CStr(v_vArray(ACDetailKey, lItem)).Trim())
					
					Exit For
				End If
				
			Next lItem
			
			' if we dont find the values specified then return false
			If r_sItemCode = "" Then
				
				result = gPMConstants.PMEReturnCode.PMFalse
				
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
			oDict.Add("r_sItemCode", r_sItemCode)
			oDict.Add("r_lItemId", r_lItemId)
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
			'*******************************
			
			Return result


			Return result
		End Try
	End Function
	
	
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
		
        Dim sMessageTitle As String
		Dim bError As Boolean
		Dim sCode As String = ""
		Dim lId As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' set the default message title
			sMessageTitle = GetResString(ACResDataDetailsTitle)
			
			' Completion Task Outcome
			If Not bError Then
				
				' reset all values
				m_oMaintainData.CompletionTaskCode = ""
				m_oMaintainData.CompletionTaskDescription = ""
				m_oMaintainData.CompletionTaskId = 0
				
				If cboCompletionTask.Text <> "" Then

					If GetArrayItem(v_vArray:=m_vTaskOutcomes, r_sItemDesc:=cboCompletionTask.Text, r_sItemCode:=sCode, r_lItemId:=lId) = gPMConstants.PMEReturnCode.PMTrue Then
						
						m_oMaintainData.CompletionTaskCode = sCode
						m_oMaintainData.CompletionTaskDescription = cboCompletionTask.Text
						m_oMaintainData.CompletionTaskId = lId
						
					End If
				End If
			End If
			
			' Incomplete Task Outcome
			If Not bError Then
				
				sCode = ""
				lId = 0
				
				m_oMaintainData.IncompleteCode = ""
				m_oMaintainData.IncompleteDescription = ""
				m_oMaintainData.IncompleteTaskId = 0
				
				If cboIncompleteTask.Text <> "" Then

					If GetArrayItem(v_vArray:=m_vTaskOutcomes, r_sItemDesc:=cboIncompleteTask.Text, r_sItemCode:=sCode, r_lItemId:=lId) = gPMConstants.PMEReturnCode.PMTrue Then
						m_oMaintainData.IncompleteCode = sCode
						m_oMaintainData.IncompleteDescription = cboIncompleteTask.Text
						m_oMaintainData.IncompleteTaskId = lId
					End If
				End If
			End If
			
			' Action Required
			If Not bError Then
				m_oMaintainData.ActionRequired = (chkActionRequired.CheckState = CheckState.Checked)
			End If
			
			'***********
			' MEvans : 20-10-2003 : Continuation Tasks
			If Not bError Then
				m_oMaintainData.AutoUpdateBatch = (chkAutoUpdateBatch.CheckState = CheckState.Checked)
			End If
			'***********
			
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
	'UPGRADE_NOTE: (7001) The following declaration (ConvertFromNullValues) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function ConvertFromNullValues(ByRef r_vValue As gPMConstants.PMEReturnCode, ByVal v_iDataType As Integer) As gPMConstants.PMEReturnCode
		'
		'Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
		'Const sFunctionName As String = "ConvertFromNullValues"
		'
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			'
			'
			'Select Case v_iDataType
				'Case gPMConstants.PMEDataType.PMLong, gPMConstants.PMEDataType.PMCurrency, gPMConstants.PMEDataType.PMBoolean, gPMConstants.PMEDataType.PMDate

					'If r_vValue Is DBNull.Value Or r_vValue = "" Then
						'Return gPMConstants.PMEReturnCode.PMFalse
					'Else
						'Return r_vValue
					'End If
					'
				'Case Else
					'Return r_vValue
					'
			'End Select
		'
		'Catch 
		'End Try
		'
		'
		'
		'result = gPMConstants.PMEReturnCode.PMError
		'
		'******************************
		' Log Error.
		'gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
		'*******************************
		'
		'Return result
		'
	'End Function
	
	
	
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
			
			txtCode.Enabled = False
			txtDescription.Enabled = False
			
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
	'UPGRADE_NOTE: (7001) The following declaration (ItemInArray) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function ItemInArray(ByRef r_vArray() As Object, ByVal v_sItemValue As String) As Integer
		'
		'Dim result As Integer = 0
		'Const sFunctionName As String = "ItemInArray"
		'
		'Dim llbound, lUbound As Integer
		'Dim bItemExists As Boolean
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			' create new collection
			'If Information.IsArray(r_vArray) Then
				'
				' get array boundaries
				'llbound = r_vArray.GetLowerBound(0)
				'lUbound = r_vArray.GetUpperBound(0)
				'
				' for each item in the array
				'For 'lItem As Integer = llbound To lUbound
					'
					' check if we match the values from the array specified

					'If CStr(r_vArray(lItem)).Trim() = v_sItemValue.Trim() Then
						' indicate item already exists
						'bItemExists = True
						'
						' quit loop
						'lItem = lUbound
					'End If
					'
				'Next lItem
				'
			'End If
			'
			' if the item doesnt already exist
			' add it to the array
			'If bItemExists Then
				'result = gPMConstants.PMEReturnCode.PMFalse
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
	
	'UPGRADE_NOTE: (7001) The following declaration (lblExecutable_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub lblExecutable_Click()
		'
	'End Sub
End Class