Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmOutcomes
	Inherits System.Windows.Forms.Form
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "frmOutcomes"
	
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
	
    Private m_vTaskOutcomes As Object
	Private m_oMaintainItem As MaintainData
	Private m_lActionType As Integer
	
	'************************
	Public WriteOnly Property TaskOutcomes() As Object()
		Set(ByVal Value() As Object)
			m_vTaskOutcomes = Value
		End Set
	End Property
	'************************
	
	Public Property MaintainITem() As MaintainData
		Get
			Return m_oMaintainItem
		End Get
		Set(ByVal Value As MaintainData)
			m_oMaintainItem = Value
		End Set
	End Property
	'************************
	
	Public WriteOnly Property ActionType() As Integer
		Set(ByVal Value As Integer)
			m_lActionType = Value
		End Set
	End Property
	
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		m_lReturn = ActionCancel()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		m_lReturn = ActionOK()
	End Sub
	

	Private Sub frmOutcomes_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
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
			
			m_lReturn = PopulateList()
			
			m_lReturn = SetReadOnlyFields()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
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
			
			Me.Text = GetResString(ACResDataOutcomesTitle)
			tabMainTab.SelectedTab.Text = GetResString(ACResDataOutcomesTabOutcomes)
			
			cmdOK.Text = GetResString(ACResDataOutcomesButtonOK)
			cmdCancel.Text = GetResString(ACResDataOutcomesButtonCancel)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: PopulateList
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 23-06-2003 : Workflow
	' ***************************************************************** '
	Private Function PopulateList() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "PopulateListView"
		
		Dim llbound, lUbound As Integer
		Dim sTaskOutcomeDescription As String = ""
		Dim lTaskOutcomeId As Integer
		Dim bItemSelected As Boolean
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' if we have a valid collection
			If Information.IsArray(m_vTaskOutcomes) Then
				
				' get array boundaries
				llbound = m_vTaskOutcomes.GetLowerBound(1)
				lUbound = m_vTaskOutcomes.GetUpperBound(1)
				
				' for each item in the array
				For lItem As Integer = llbound To lUbound
					
					' get task outcome properties
					lTaskOutcomeId = CInt(m_vTaskOutcomes(0, lItem))
					sTaskOutcomeDescription = CStr(m_vTaskOutcomes(1, lItem))
					
					' set up list item
					lstOutcomes.Items.Insert(lItem, sTaskOutcomeDescription)
					VB6.SetItemData(lstOutcomes, lItem, lTaskOutcomeId)
					
					' if we have a task action type outcome that matches
					' then show this item as selected
					If m_oMaintainItem.OutcomeSelected(v_lTaskOutcomeId:=lTaskOutcomeId, r_bSelected:=bItemSelected) = gPMConstants.PMEReturnCode.PMTrue Then
						
						lstOutcomes.SetItemChecked(lItem, bItemSelected)
						
					End If
					
				Next lItem
				
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
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
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
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
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result


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
		
		Dim lTaskOutcomeId As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' if this is add or edit mode
			If m_lActionType = ACActionAdd Or m_lActionType = ACActionEdit Then
				
				m_oMaintainItem.OutcomesUpdated = True
				
				' reset collection
				m_oMaintainItem.TaskOutcomes = New Collection()
				
				' for each item in the list
				For lItem As Integer = 1 To lstOutcomes.Items.Count
					
					' if the item is selected
					If lstOutcomes.GetItemChecked(lItem - 1) Then
						
						' add a task outcome to the maintain item
						lTaskOutcomeId = VB6.GetItemData(lstOutcomes, lItem - 1)
						m_lReturn = CType(m_oMaintainItem.AddOutcome(lTaskOutcomeId), gPMConstants.PMEReturnCode)
					End If
					
				Next lItem
				
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result


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
	Private Function ActionCancel() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "ActionCancel"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			Me.Close()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
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
				Case ACActionView
					lstOutcomes.Enabled = False
					
			End Select
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
End Class
