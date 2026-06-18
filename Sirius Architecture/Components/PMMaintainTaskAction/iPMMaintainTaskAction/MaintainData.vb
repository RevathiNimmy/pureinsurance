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
	Private m_sCode As String = ""
	Private m_lId As Integer
	Private m_sDescription As String = ""
	Private m_bIsDeleted As Boolean
	Private m_dtEffectiveDate As Date
	Private m_bItemIsLive As Boolean
	Private m_bSaveOriginal As Boolean
	Private m_bItemUpdated As Boolean
	'***************************
	
	Private m_lDueDays As Integer
	Private m_sTemplateCode As String = ""
	Private m_bOutcomeNotEditable As Boolean
	
	'**********************
	' Original Properties
	'**********************
	' default
	Private m_sOrigDescription As String = ""
	Private m_bOrigIsDeleted As Boolean
	Private m_dtOrigEffectiveDate As Date
	
	' custom
	Private m_lOrigDueDays As Integer
	Private m_sOrigTemplateCode As String = ""
	Private m_bOrigOutcomeNotEditable As Boolean
	'**********************
	
	Private m_bOutcomesUpdated As Boolean
	Private m_sTaskOutcomeIds As String = ""
	
	Private m_colTaskOutcomes As New Collection
	
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
	
	Public Property IsDeleted() As Boolean
		Get
			Return m_bIsDeleted
		End Get
		Set(ByVal Value As Boolean)
			m_bIsDeleted = Value
		End Set
	End Property
	' ************************
	
	Public Property EffectiveDate() As Date
		Get
			Return m_dtEffectiveDate
		End Get
		Set(ByVal Value As Date)
			m_dtEffectiveDate = Value
		End Set
	End Property
	
	'************************
	'************************
	'************************
	Public Property DueDays() As Integer
		Get
			Return m_lDueDays
		End Get
		Set(ByVal Value As Integer)
			m_lDueDays = Value
		End Set
	End Property
	'************************
	Public Property DocumentTemplateCode() As String
		Get
			Return m_sTemplateCode
		End Get
		Set(ByVal Value As String)
			m_sTemplateCode = Value
		End Set
	End Property
	'************************
	Public Property OutcomeNotEditable() As Boolean
		Get
			Return m_bOutcomeNotEditable
		End Get
		Set(ByVal Value As Boolean)
			m_bOutcomeNotEditable = Value
		End Set
	End Property
	Public ReadOnly Property OutcomeEditable() As Boolean
		Get
			Return Not m_bOutcomeNotEditable
		End Get
	End Property
	'************************
	
	Public Property ItemIsLive() As Boolean
		Get
			Return m_bItemIsLive
		End Get
		Set(ByVal Value As Boolean)
			m_bItemIsLive = Value
		End Set
	End Property
	'************************
	
	Public Property TaskOutcomes() As Collection
		Get
			Return m_colTaskOutcomes
		End Get
		Set(ByVal Value As Collection)
			m_colTaskOutcomes = Value
		End Set
	End Property
	'************************
	Public Property OutcomesUpdated() As Boolean
		Get
			Return m_bOutcomesUpdated
		End Get
		Set(ByVal Value As Boolean)
			m_bOutcomesUpdated = Value
		End Set
	End Property
	'************************
	Public ReadOnly Property TaskOutcomeIds() As String
		Get
			Dim result As String = String.Empty
			If BuildTaskOutcomeIdString() Then
				result = m_sTaskOutcomeIds
			End If
			Return result
		End Get
	End Property
	
	Public ReadOnly Property ItemUpdated() As Boolean
		Get
			
			If m_sOrigDescription = m_sDescription And m_dtOrigEffectiveDate = m_dtEffectiveDate And m_bOrigIsDeleted = m_bIsDeleted And m_lOrigDueDays = m_lDueDays And m_sOrigTemplateCode = m_sTemplateCode And m_bOrigOutcomeNotEditable = m_bOutcomeNotEditable Then
				
				m_bItemUpdated = False
			Else
				m_bItemUpdated = True
			End If
			
			Return m_bItemUpdated
			
		End Get
	End Property
	'************************
	
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
			
			' generic data
			' no need to copy this back as it shouldnt have changed
			'm_sCode = v_oMaintainData.Code
			'm_lId = v_oMaintainData.Id
			
			m_sDescription = v_oMaintainData.Description
			m_bIsDeleted = v_oMaintainData.IsDeleted
			m_dtEffectiveDate = v_oMaintainData.EffectiveDate
			'***************************
			
			m_lDueDays = v_oMaintainData.DueDays
			m_sTemplateCode = v_oMaintainData.DocumentTemplateCode
			m_bOutcomeNotEditable = v_oMaintainData.OutcomeNotEditable
			
			m_bOutcomesUpdated = v_oMaintainData.OutcomesUpdated
			
			' get the items task outcome collection
			m_colTaskOutcomes = v_oMaintainData.TaskOutcomes
			
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
			
			' default values
			m_sOrigDescription = m_sDescription
			m_bOrigIsDeleted = m_bIsDeleted
			m_dtOrigEffectiveDate = m_dtEffectiveDate
			
			' custom entries
			m_lOrigDueDays = m_lDueDays
			m_sOrigTemplateCode = m_sTemplateCode
			m_bOrigOutcomeNotEditable = m_bOutcomeNotEditable
			
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
	' Name: AddOutcome
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 25-06-2003 : workflow
	' ***************************************************************** '
	Function AddOutcome(ByVal v_lTaskOutcomeId As Integer) As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "AddOutcome"
		
		Dim oOutcomeData As OutcomeData
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' create new instance of outcome data object
			oOutcomeData = New OutcomeData()
			
			' set properties
			oOutcomeData.TaskOutcomeId = v_lTaskOutcomeId
			
			' add the task outcome id to the collection
			m_colTaskOutcomes.Add(oOutcomeData, CStr(v_lTaskOutcomeId))
		
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
	' Name: OutcomeSelected
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 25-06-2003 : workflow
	' ***************************************************************** '
	Public Function OutcomeSelected(ByVal v_lTaskOutcomeId As Integer, ByRef r_bSelected As Boolean) As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "OutcomeSelected"
        Dim oOutcomeData As OutcomeData
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If Not (m_colTaskOutcomes Is Nothing) Then
				
				oOutcomeData = m_colTaskOutcomes.Item(CStr(v_lTaskOutcomeId))
				
			End If
			
			r_bSelected = Not (oOutcomeData Is Nothing)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			If Information.Err().Number = 5 Then


			End If
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
			oDict.Add("v_lTaskOutcomeId", v_lTaskOutcomeId)
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	
	
	Protected Overrides Sub Finalize()
		m_colTaskOutcomes = Nothing
	End Sub
	
	' ***************************************************************** '
	' Name: ResetTaskOutcomeCollection
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 25-06-2003 : workflow
	' ***************************************************************** '
	'UPGRADE_NOTE: (7001) The following declaration (ResetTaskOutcomeCollection) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function ResetTaskOutcomeCollection() As Integer
		'
		'Dim result As Integer = 0
		'Const sFunctionName As String = "ResetTaskOutcomeCollection"
		'
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			'm_colTaskOutcomes = Nothing
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
	' Name: BuildTaskOutcomeIdString
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 25-06-2003 : workflow
	' ***************************************************************** '
	Private Function BuildTaskOutcomeIdString() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "BuildTaskOutcomeIdString"
		
		
		 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' set default task outcomeid
			' so stored proc doesnt fall over if we have
			' removed all task outcomes from a task action type
			m_sTaskOutcomeIds = "0,"
			
			' if we have a valid collection
			If Not (m_colTaskOutcomes Is Nothing) Then
				
				' for each item in collection
				For lItem As Integer = 1 To m_colTaskOutcomes.Count
					
					' build the string of task outcome ids

					m_sTaskOutcomeIds = m_sTaskOutcomeIds & m_colTaskOutcomes.Item(lItem).TaskOutcomeId & ","
					
				Next lItem
				
			End If
			
			' remove trailing comma
			m_sTaskOutcomeIds = Mid(m_sTaskOutcomeIds, 1, m_sTaskOutcomeIds.Length - 1)
			
			Return result
		
	End Function
End Class

