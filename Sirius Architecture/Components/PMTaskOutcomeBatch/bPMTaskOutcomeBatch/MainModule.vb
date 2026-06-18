Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports SharedFiles
Module MainModule
	
	Public Const ACApp As String = "bPMTaskOutcomeBatch"
	
	Private Const ACClass As String = "MainModule"
	Private Const ACStepInstanceCnt As Integer = 0
	Private Const ACWorkflowPackageId As Integer = 1
	Private Const ACTaskInstanceCnt As Integer = 2
	Private Const ACDefaultIncompleteTaskOutcomeId As Integer = 3
	Private Const ACTaskInstanceStatusCompleted As Integer = 3
	Private Const ACNextWorkflowStepId As Integer = 4
	
	Private m_sUsername As New FixedLengthString(12)
	Private m_sPassword As New FixedLengthString(30)
	Private m_iUserID As Integer
	Private m_sCallingAppName As String = ""
	Private m_iSourceID As Integer
	Private m_iLanguageID As Integer
	Private m_iCurrencyID As Integer
	Private m_iLogLevel As Integer
	
	Private m_oBatch As Object
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	Private m_oDatabase As dPMDAO.Database
	Private m_bCloseDatabase As Boolean
	
	' ***************************************************************** '
	' Name: Main
	'
	' Parameters: n/a
	'
	' Description: starts the task outcome batch process
	'
	' History:
	'           Created : MEvans : 01-10-2003 : 229
	' ***************************************************************** '
	Public Sub Main()
		
		Const sFunctionName As String = "Main"
		
		Try 
			
			'**************
			' initialisation
			m_bCloseDatabase = True
			m_sUsername.Value = "sirius"
			m_sPassword.Value = "sirius"
			m_iUserID = 1
			m_sCallingAppName = ACApp
			m_iSourceID = 1
			m_iLanguageID = 1
			m_iCurrencyID = 26
			m_iLogLevel = 6
			'**************
			
			If Initialise() = gPMConstants.PMEReturnCode.PMTrue Then
				
				If ProcessTaskOutcomeBatch() <> gPMConstants.PMEReturnCode.PMTrue Then
					
                    gPMFunctions.LogMessageToFile(sUserName:=m_sUsername.Value, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to process task outcome batch", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
					
				End If
				
			End If
		
		Catch excep As System.Exception
			
			
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername.Value, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'******************************
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: Initialise
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 01-10-2003 : 229
	' ***************************************************************** '
	Private Function Initialise() As Integer



		
		Dim result As Integer = 0
		Const sFunctionName As String = "Initialise"
		
		 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'**************
			' initialisation
			m_bCloseDatabase = True
			m_sUsername.Value = "sirius"
			m_sPassword.Value = "sirius"
			m_iUserID = 1
			m_sCallingAppName = ACApp
			m_iSourceID = 1
			m_iLanguageID = 1
			m_iCurrencyID = 26
			m_iLogLevel = 6
			'**************
			
			m_lReturn = CType(gPMComponentServices.CheckDatabase(v_sUsername:=m_sUsername.Value, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				
				result = gPMConstants.PMEReturnCode.PMFalse
				
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername.Value, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to create database", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
				
				Return result
				
			End If
			
			Return result
		
	End Function
	
	
	' ***************************************************************** '
	' Name: ProcessTaskOutcomeBatch
	'
	' Parameters: n/a
	'
	' Description: oversees the batch process..
	'
	' History:
	'           Created : MEvans : 01-10-2003 : 229
	' ***************************************************************** '
	Private Function ProcessTaskOutcomeBatch() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "ProcessTaskOutcomeBatch"
		
		Dim vDefaultIncompleteTaskOutcomeId As String = ""
		Dim vBatchTaskDetails As Object
		Dim llBound, lUBound, lWorkflowStepInstanceCnt, lWorkflowPackageId, lWorkflowTaskInstanceCnt, lOverdueNextWorkflowStepId As Integer
		Dim bTransactionError As Boolean
		
		 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' get any overdue batch tasks
			If GetOverdueBatchTasks(r_vResults:=vBatchTaskDetails) = gPMConstants.PMEReturnCode.PMTrue Then
				
				' if we have any overdue tasks to process
				If Information.IsArray(vBatchTaskDetails) Then
					
					' get the array boundaries

					llBound = vBatchTaskDetails.GetLowerBound(1)

					lUBound = vBatchTaskDetails.GetUpperBound(1)
					
					'*************
					' put everything in a big transaction
					' so if one thing fails the lot does
					'm_oDatabase.BeginTrans
					'*************
					
					' for each overdue task item
					For lBatchItem As Integer = llBound To lUBound
						
						' get the item details from the array
						
						' the over due step instance id

                        If CStr(vBatchTaskDetails(ACStepInstanceCnt, lBatchItem)) = "" Then
                            lWorkflowStepInstanceCnt = 0
                        Else

                            lWorkflowStepInstanceCnt = CInt(vBatchTaskDetails(ACStepInstanceCnt, lBatchItem))
                        End If

                        ' the over due step package id

                        If CStr(vBatchTaskDetails(ACWorkflowPackageId, lBatchItem)) = "" Then
                            lWorkflowPackageId = 0
                        Else

                            lWorkflowPackageId = CInt(vBatchTaskDetails(ACWorkflowPackageId, lBatchItem))
                        End If

                        ' the task instance of the overdue task

                        If CStr(vBatchTaskDetails(ACTaskInstanceCnt, lBatchItem)) = "" Then
                            lWorkflowTaskInstanceCnt = 0
                        Else

                            lWorkflowTaskInstanceCnt = CInt(vBatchTaskDetails(ACTaskInstanceCnt, lBatchItem))
                        End If

                        ' the default task outcome status for the task instance

                        If CStr(vBatchTaskDetails(ACDefaultIncompleteTaskOutcomeId, lBatchItem)) = "" Then

                            vDefaultIncompleteTaskOutcomeId = Nothing
                        Else

                            vDefaultIncompleteTaskOutcomeId = CStr(vBatchTaskDetails(ACDefaultIncompleteTaskOutcomeId, lBatchItem))
                        End If

                        ' the next step in the package to be executed..

                        If CStr(vBatchTaskDetails(ACNextWorkflowStepId, lBatchItem)) = "" Then
                            lOverdueNextWorkflowStepId = 0
                        Else

                            lOverdueNextWorkflowStepId = CInt(vBatchTaskDetails(ACNextWorkflowStepId, lBatchItem))
                        End If
						
						' update the task outcome status / date
						If UpdateOverdueTaskOutcomes(v_lTaskInstanceCnt:=lWorkflowTaskInstanceCnt, v_vTaskOutcomeId:=vDefaultIncompleteTaskOutcomeId, v_lTaskStatusId:=ACTaskInstanceStatusCompleted) <> gPMConstants.PMEReturnCode.PMTrue Then
							
							bTransactionError = True
							Exit For
						End If
						
						If ProcessNextPackageStep(v_lStepInstanceCnt:=lWorkflowStepInstanceCnt, v_lTaskInstanceCnt:=lWorkflowTaskInstanceCnt, v_lWorkflowPackageId:=lWorkflowPackageId, v_lNextStepId:=lOverdueNextWorkflowStepId) <> gPMConstants.PMEReturnCode.PMTrue Then
							
							bTransactionError = True
							Exit For
						End If
						
					Next lBatchItem
					
				Else
					' do nothing
					' it is valid to have no batch items marked as overdue..
				End If
				
			End If
		
			'    If bTransactionError Then
			'        m_oDatabase.RollbackTrans
			'    Else
			'        m_oDatabase.CommitTrans
			'    End If
		
		
		
		Return result
		
	End Function
	
	' ***************************************************************** '
	' Name: GetOverdueBatchTasks
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 01-10-2003 : 229
	' ***************************************************************** '
	Private Function GetOverdueBatchTasks(ByRef r_vResults(,) As Object) As Integer 
		
		Dim result As Integer = 0 
		Const sFunctionName As String = "GetOverdueBatchTasks"
		
		 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Clear Down Database Parameters
			m_oDatabase.Parameters.Clear()
			
			' Execute selection Query
			If m_oDatabase.SQLSelect(sSQL:=ACGetOverdueBatchTasksSQL, sSQLName:=ACGetOverdueBatchTasksName, bStoredProcedure:=True, vResultArray:=r_vResults) <> gPMConstants.PMEReturnCode.PMTrue Then
				
				result = gPMConstants.PMEReturnCode.PMFalse
				
				'******************************
				' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername.Value, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to retrieve over batch task details", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))
				'******************************
				
			End If
			
			Return result
		
	End Function
	
	' ***************************************************************** '
	' Name: UpdateOverdueTaskOutcomes
	'
	' Parameters: n/a
	'
	' Description: Updates the specified task instance task outcome id
	'                   and task outcome date
	' History:
	'           Created : MEvans : 01-10-2003 : 229
	' ***************************************************************** '
	Private Function UpdateOverdueTaskOutcomes(ByVal v_lTaskInstanceCnt As Integer, ByVal v_vTaskOutcomeId As Object, ByVal v_lTaskStatusId As Integer) As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "UpdateOverdueTaskOutcomes"
		
		 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Clear Down Database Parameters
			m_oDatabase.Parameters.Clear()
			
			' Add Required Stored Procedure Parameters
			
			' pwmrk_task_instance_cnt
			m_lReturn = CType(AddInputParameter(v_sName:="pmwrk_task_instance_cnt", v_vValue:=v_lTaskInstanceCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
			
			' task_outcome_id - note this can be null
			m_lReturn = CType(AddInputParameter(v_sName:="task_outcome_id", v_vValue:=v_vTaskOutcomeId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
			
			' task status
			m_lReturn = CType(AddInputParameter(v_sName:="task_status", v_vValue:=v_lTaskStatusId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
			
			' Execute Action Query
			If m_oDatabase.SQLAction(sSQL:=ACUpdateOverdueTaskOutcomesSQL, sSQLName:=ACUpdateOverdueTaskOutcomesName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then
				
				result = gPMConstants.PMEReturnCode.PMFalse
				
				'******************************
				' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
				oDict.Add("v_lTaskInstanceCnt", v_lTaskInstanceCnt)
				oDict.Add("v_vTaskOutcomeId", v_vTaskOutcomeId)
				oDict.Add("v_lTaskStatusId", v_lTaskStatusId)
				gPMFunctions.LogMessageToFile(sUserName:=m_sUsername.Value, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to update overdue task outcome for pwmk_task_instance_cnt:" & CStr(v_lTaskInstanceCnt), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
				'******************************
				
			End If
			
			Return result
		
	End Function
	
	' ***************************************************************** '
	' Name: ProcessNextPackageStep
	'
	' Parameters: n/a
	'
	' Description: Call the package step processor to handle the
	'               creation of the next step task and associated
	'                 details...
	'
	' History:
	'           Created : MEvans : 01-10-2003: 229
	' ***************************************************************** '
	Private Function ProcessNextPackageStep(ByVal v_lStepInstanceCnt As Integer, ByVal v_lTaskInstanceCnt As Integer, ByVal v_lWorkflowPackageId As Integer, ByVal v_lNextStepId As Integer) As Integer
		Dim result As Integer = 0
        Const sFunctionName As String = "ProcessNextPackageStep"
		
		Dim oPackageStepProcessor As Object
		
		 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' create instance of package step processor
			oPackageStepProcessor = New bPMPackageStep.Business()
			
			' if we have successfully got an instance
			If Not (oPackageStepProcessor Is Nothing) Then
				
				' initialise the component

				If oPackageStepProcessor.Initialise(sUserName:=m_sUsername.Value, sPassword:=m_sPassword.Value, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) = gPMConstants.PMEReturnCode.PMTrue Then
					
					' process the next step in the package

					If oPackageStepProcessor.ProcessNextStep(v_lStepInstanceCnt, v_lTaskInstanceCnt, v_lNextStepId) <> gPMConstants.PMEReturnCode.PMTrue Then
						
						result = gPMConstants.PMEReturnCode.PMFalse
						
                    Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
						oDict.Add("v_lStepInstanceCnt", v_lStepInstanceCnt)
						oDict.Add("v_lTaskInstanceCnt", v_lTaskInstanceCnt)
						oDict.Add("v_lWorkflowPackageId", v_lWorkflowPackageId)
						oDict.Add("v_lNextStepId", v_lNextStepId)
						gPMFunctions.LogMessageToFile(sUserName:=m_sUsername.Value, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to process the next step " & " in workflow package:" & CStr(v_lWorkflowPackageId), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)
						
					End If
					
				Else
					
					result = gPMConstants.PMEReturnCode.PMFalse
					
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
					oDict.Add("v_lStepInstanceCnt", v_lStepInstanceCnt)
					oDict.Add("v_lTaskInstanceCnt", v_lTaskInstanceCnt)
					oDict.Add("v_lWorkflowPackageId", v_lWorkflowPackageId)
					oDict.Add("v_lNextStepId", v_lNextStepId)
					gPMFunctions.LogMessageToFile(sUserName:=m_sUsername.Value, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to initialise bPMPackageStep.Business", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)
					
				End If
				
			Else
				
				result = gPMConstants.PMEReturnCode.PMFalse
				
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
				oDict.Add("v_lStepInstanceCnt", v_lStepInstanceCnt)
				oDict.Add("v_lTaskInstanceCnt", v_lTaskInstanceCnt)
				oDict.Add("v_lWorkflowPackageId", v_lWorkflowPackageId)
				oDict.Add("v_lNextStepId", v_lNextStepId)
				gPMFunctions.LogMessageToFile(sUserName:=m_sUsername.Value, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to create instance of bPMPackageStep.Business", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)
				
			End If
			
			Return result
		
	End Function
	
	
	' ***************************************************************** '
	' Name: AddInputParameter
	'
	' Parameters: v_sName   : Parameter Name
	'             v_vValue  : Parameter Value
	'             v_iType   : Parameter DataType
	'
	' Description: Adds an input parameter to the database parameters
	'
	' History:
	'           Created : MEvans : 18-12-2002 : 202
	' ***************************************************************** '
	Private Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "AddInputParameter"
		
		 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Add Parameter to database object

			If m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=CStr(v_vValue), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType) <> gPMConstants.PMEReturnCode.PMTrue Then
				
				' Log Error.
				result = gPMConstants.PMEReturnCode.PMFalse
				

            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername.Value, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to add parameter name:" & v_sName & _
                                          ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))
				
			End If
			
			Return result
		
	End Function
End Module

