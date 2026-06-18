Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
Friend NotInheritable Class MaintainData 
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "MaintainData"
	
	'***************************
	'*** Default Properties ****
	Private m_sCode As String = ""
	Private m_lId As Integer
	Private m_sDescription As String = ""
	
	Private m_bSaveOriginal As Boolean
	Private m_bItemUpdated As Boolean
	'***************************
	
	Private m_lCompletionTaskId As Integer
	Private m_lIncompleteTaskId As Integer
	Private m_bActionRequired As Boolean
	
	Private m_sCompletionTaskCode As String = ""
	Private m_sCompletionTaskDescription As String = ""
	Private m_sIncompleteTaskCode As String = ""
	Private m_sIncompleteTaskDescription As String = ""
	
	'********
	' MEvans : 20-10-2003 : Continuation Tasks
	Private m_bAutoUpdateBatch As Boolean
	'********
	
	'**********************
	' Original Properties
	'**********************
	Private m_lOrigCompletionTaskId As Integer
	Private m_lOrigIncompleteTaskId As Integer
	Private m_bOrigActionRequired As Boolean
	
	'********
	' MEvans : 20-10-2003 : Continuation Tasks
	Private m_bOrigAutoUpdateBatch As Boolean
	'********
	
	'***************************
	'*** Default Properties ****
	'***************************
	
	Public Property Id() As Integer
		Get
			Return m_lId
		End Get
		Set(ByVal Value As Integer)
			m_lId = Value
		End Set
	End Property
	'************************
	
	Public Property Code() As String
		Get
			Return m_sCode
		End Get
		Set(ByVal Value As String)
			m_sCode = Value
		End Set
	End Property
	'***************************
	
	Public Property CompletionTaskId() As Integer
		Get
			Return m_lCompletionTaskId
		End Get
		Set(ByVal Value As Integer)
			m_lCompletionTaskId = Value
		End Set
	End Property
	'***************************
	Public Property CompletionTaskCode() As String
		Get
			Return m_sCompletionTaskCode
		End Get
		Set(ByVal Value As String)
			m_sCompletionTaskCode = Value
		End Set
	End Property
	'***************************
	Public Property CompletionTaskDescription() As String
		Get
			Return m_sCompletionTaskDescription
		End Get
		Set(ByVal Value As String)
			m_sCompletionTaskDescription = Value
		End Set
	End Property
	'***************************
	Public Property IncompleteCode() As String
		Get
			Return m_sIncompleteTaskCode
		End Get
		Set(ByVal Value As String)
			m_sIncompleteTaskCode = Value
		End Set
	End Property
	'***************************
	Public Property IncompleteDescription() As String
		Get
			Return m_sIncompleteTaskDescription
		End Get
		Set(ByVal Value As String)
			m_sIncompleteTaskDescription = Value
		End Set
	End Property
	'***************************
	
	Public Property IncompleteTaskId() As Integer
		Get
			Return m_lIncompleteTaskId
		End Get
		Set(ByVal Value As Integer)
			m_lIncompleteTaskId = Value
		End Set
	End Property
	'***************************
	
	Public Property ActionRequired() As Boolean
		Get
			Return m_bActionRequired
		End Get
		Set(ByVal Value As Boolean)
			m_bActionRequired = Value
		End Set
	End Property
	'************************
	' MEvans : 20-10-2003 : Continuation Tasks
	
	Public Property AutoUpdateBatch() As Boolean
		Get
			Return m_bAutoUpdateBatch
		End Get
		Set(ByVal Value As Boolean)
			m_bAutoUpdateBatch = Value
		End Set
	End Property
	'************************
	Public Property Description() As String
		Get
			Return m_sDescription
		End Get
		Set(ByVal Value As String)
			m_sDescription = Value
		End Set
	End Property
	'************************
	Public ReadOnly Property ItemUpdated() As Boolean
		Get
			
			' MEvans : 20-10-2003 : Continuation Tasks
			' added autoupdatebatch to check
			
			
			m_bItemUpdated = Not (m_lOrigCompletionTaskId = m_lCompletionTaskId And m_lOrigIncompleteTaskId = m_lIncompleteTaskId And m_bOrigActionRequired = m_bActionRequired And m_bOrigAutoUpdateBatch = m_bAutoUpdateBatch)
			
			Return m_bItemUpdated
			
		End Get
	End Property
	' ***************************************************************** '
	' Name: Copy
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 24-06-2003 : workflow
	' ***************************************************************** '
	Public Function Copy(ByVal v_oMaintainData As MaintainData) As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "Copy"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			
			
			m_lCompletionTaskId = v_oMaintainData.CompletionTaskId
			m_lIncompleteTaskId = v_oMaintainData.IncompleteTaskId
			
			m_sCompletionTaskCode = v_oMaintainData.CompletionTaskCode
			m_sCompletionTaskDescription = v_oMaintainData.CompletionTaskDescription
			
			m_sIncompleteTaskCode = v_oMaintainData.IncompleteCode
			m_sIncompleteTaskDescription = v_oMaintainData.IncompleteDescription
			
			'***********
			' MEvans : 20-10-2003 : Continuation Tasks
			m_bAutoUpdateBatch = v_oMaintainData.AutoUpdateBatch
			'***********
			
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
	' Name: CopyToOriginalData
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 25-06-2003 : workflow
	' ***************************************************************** '
	Public Function CopyToOriginalData() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "CopyToOriginalData"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' custom entries
			m_lOrigCompletionTaskId = m_lCompletionTaskId
			m_lOrigIncompleteTaskId = m_lIncompleteTaskId
			m_bOrigActionRequired = m_bActionRequired
			m_bOrigAutoUpdateBatch = m_bAutoUpdateBatch
			
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
End Class
