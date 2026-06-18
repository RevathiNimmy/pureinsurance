Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
Friend NotInheritable Class MaintainData 
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "MaintainData"
	
	
	
	'***************************
	'*** Default Properties ****
	'***************************
	Private m_lTaskId As Integer
	Private m_lTaskGroupId As Integer
	Private m_sTaskCode As String = ""
	Private m_sTaskGroupCode As String = ""
	Private m_sTaskDescription As String = ""
	Private m_lId As Integer
	
	Private m_bActionTypesUpdated As Boolean
	Private m_sTaskActionTypeIds As String = ""
	Private m_colTaskActionTypes As New Collection
	
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
	
	Public Property TaskId() As Integer
		Get
			Return m_lTaskId
		End Get
		Set(ByVal Value As Integer)
			m_lTaskId = Value
		End Set
	End Property
	'************************
	Public Property TaskGroupId() As Integer
		Get
			Return m_lTaskGroupId
		End Get
		Set(ByVal Value As Integer)
			m_lTaskGroupId = Value
		End Set
	End Property
	'************************
	Public Property TaskCode() As String
		Get
			Return m_sTaskCode
		End Get
		Set(ByVal Value As String)
			m_sTaskCode = Value
		End Set
	End Property
	'************************
	Public Property TaskGroupCode() As String
		Get
			Return m_sTaskGroupCode
		End Get
		Set(ByVal Value As String)
			m_sTaskGroupCode = Value
		End Set
	End Property
	'************************
	
	Public Property TaskDescription() As String
		Get
			Return m_sTaskDescription
		End Get
		Set(ByVal Value As String)
			m_sTaskDescription = Value
		End Set
	End Property
	
	
	'************************
	
	Public Property TaskActionTypes() As Collection
		Get
			Return m_colTaskActionTypes
		End Get
		Set(ByVal Value As Collection)
			m_colTaskActionTypes = Value
		End Set
	End Property
	'************************
	Public Property TaskActionTypesUpdated() As Boolean
		Get
			Return m_bActionTypesUpdated
		End Get
		Set(ByVal Value As Boolean)
			m_bActionTypesUpdated = Value
		End Set
	End Property
	'************************
	Public ReadOnly Property TaskActionTypeIds() As String
		Get
			Dim result As String = String.Empty
			If BuildTaskActionTypeIdString() Then
				result = m_sTaskActionTypeIds
			End If
			Return result
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
			
			' get the items task ActionType collection
			m_colTaskActionTypes = v_oMaintainData.TaskActionTypes
			
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
	' Name: AddTaskActionType
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 25-06-2003 : workflow
	' ***************************************************************** '
	Function AddTaskActionType(ByVal v_lTaskActionTypeId As Integer) As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "AddTaskActionType"
		
		Dim oActionType As ActionType
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' create new instance of ActionType data object
			oActionType = New ActionType()
			
			' set properties
			oActionType.TaskActionTypeId = v_lTaskActionTypeId
			
			' add the task ActionType id to the collection
			m_colTaskActionTypes.Add(oActionType, CStr(v_lTaskActionTypeId))
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
		
		Finally 
			' destroy object reference
		End Try
		
		
		
		
		Return result


		Return result
	End Function
	
	' ***************************************************************** '
	' Name: ActionTypeSelected
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 25-06-2003 : workflow
	' ***************************************************************** '
	Public Function ActionTypeSelected(ByVal v_lTaskActionTypeId As Integer, ByRef r_bSelected As Boolean) As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "ActionTypeSelected"

        Dim oActionTypeData As ActionType
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If Not (m_colTaskActionTypes Is Nothing) Then
				
				oActionTypeData = m_colTaskActionTypes.Item(CStr(v_lTaskActionTypeId))
				
			End If
			
			r_bSelected = Not (oActionTypeData Is Nothing)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			If Information.Err().Number = 5 Then


			End If
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
			oDict.Add("v_lTaskActionTypeId", v_lTaskActionTypeId)
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	
	
	Protected Overrides Sub Finalize()
		m_colTaskActionTypes = Nothing
	End Sub
	
	' ***************************************************************** '
	' Name: ResetTaskActionTypeCollection
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 25-06-2003 : workflow
	' ***************************************************************** '
	'UPGRADE_NOTE: (7001) The following declaration (ResetTaskActionTypeCollection) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function ResetTaskActionTypeCollection() As Integer
		'
		'Dim result As Integer = 0
		'Const sFunctionName As String = "ResetTaskActionTypeCollection"
		'
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			'm_colTaskActionTypes = Nothing
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
			'gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
			'*******************************
			'
			'Return result
			'
		'End Try
	'End Function
	
	' ***************************************************************** '
	' Name: BuildTaskActionTypeIdString
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 25-06-2003 : workflow
	' ***************************************************************** '
	Private Function BuildTaskActionTypeIdString() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "BuildTaskActionTypeIdString"
		
		
		 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' set default task ActionTypeid
			' so stored proc doesnt fall over if we have
			' removed all task ActionTypes from a task action type
			m_sTaskActionTypeIds = "0,"
			
			' if we have a valid collection
			If Not (m_colTaskActionTypes Is Nothing) Then
				
				' for each item in collection
				For lItem As Integer = 1 To m_colTaskActionTypes.Count
					
					' build the string of task ActionType ids

					m_sTaskActionTypeIds = m_sTaskActionTypeIds & m_colTaskActionTypes.Item(lItem).TaskActionTypeId & ","
					
				Next lItem
				
			End If
			
			' remove trailing comma
			m_sTaskActionTypeIds = Mid(m_sTaskActionTypeIds, 1, m_sTaskActionTypeIds.Length - 1)
			
			Return result
		
	End Function
End Class

