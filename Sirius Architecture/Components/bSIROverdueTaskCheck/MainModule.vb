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
	
	Public Const ACApp As String = "bSIROverdueTaskCheck"
	
	Private Declare Sub ExitProcess Lib "kernel32" (ByVal uExitCode As Integer)
	
	Private Const ACClass As String = "MainModule"
	
	Private Const ACPMWrkTaskInstanceCnt As Integer = 0
	Private Const ACPMUserGroupId As Integer = 1
	Private Const ACPMUserId As Integer = 2
	
	Private m_sUsername As New FixedLengthString(12)
	Private m_sPassword As New FixedLengthString(30)
	Private m_iUserID As Integer
	Private m_sCallingAppName As String = ""
	Private m_iSourceID As Integer
	Private m_iLanguageID As Integer
	Private m_iCurrencyID As Integer
	Private m_iLogLevel As Integer
	
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	Private m_oDatabase As dPMDAO.Database
	Private m_bCloseDatabase As Boolean
	Private m_oObjectManager As bObjectManager.ObjectManager
	
	' ***************************************************************** '
	' Name: Main
	'
	' Parameters: n/a
	'
	' Description: starts the overdue task check
	'
	' History:
	'           Created : DC : 020704 :
	' ***************************************************************** '
	Public Sub Main()
		
		Const sFunctionName As String = "Main"
		
		Try 
			
			'**************
			' initialisation
			m_bCloseDatabase = True
			m_sUsername.Value = "siriuscomm"
			m_sPassword.Value = "NGMBMKqSMcc5"
			m_iUserID = 1
			m_sCallingAppName = ACApp
			m_iSourceID = 1
			m_iLanguageID = 1
			m_iCurrencyID = 26
			m_iLogLevel = 6
			'**************
			
			Dim lReturn As gPMConstants.PMEReturnCode
			
			lReturn = Initialise()
			
			If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				
				lReturn = ProcessOverdueTaskCheck()
				
				If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.LogMessageToFile(sUserName:=m_sUsername.Value, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to process overdue task check", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
				End If
			End If
			
			ExitProcess(lReturn)
		
		Catch excep As System.Exception
			
			
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername.Value, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'******************************
			
			ExitProcess(gPMConstants.PMEReturnCode.PMError)
			
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
	'           Created : DC : 020704 :
	' ***************************************************************** '
	Private Function Initialise() As Integer



		
		Dim result As Integer = 0 
		Const sFunctionName As String = "Initialise"
		
		Dim sMessage As String = "" 
		Dim vUserData(,) As Object 
		
		 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Create an instance of the object manager
			m_oObjectManager = New bObjectManager.ObjectManager()
			
			m_oDatabase = New dPMDAO.Database()
			
			' Set Username and Password
			m_sUsername.Value = "siriuscomm"
			m_sPassword.Value = "NGMBMKqSMcc5"
			
			m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername.Value, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_bNewInstanceCreated:=True, r_oCheckedDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'        Set oComponentServices = Nothing
				result = gPMConstants.PMEReturnCode.PMFalse
				sMessage = "Failed to Get S4B Database"
				Throw New Exception()
			End If
			
			With m_oDatabase
				.Parameters.Clear()
				
				m_lReturn = .Parameters.Add(sName:="Username", vValue:=m_sUsername.Value, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
				
				m_lReturn = .Parameters.Add(sName:="Password", vValue:=m_sPassword.Value, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
				
				'Get the User details
				m_lReturn = .SQLSelect(sSQL:=ACSelectUserDetailsSQL, sSQLName:=ACSelectUserDetailsName, bStoredProcedure:=ACSelectUserDetailsStored, vResultArray:=vUserData)
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					result = gPMConstants.PMEReturnCode.PMFalse
					sMessage = "Unable to find User ID for User Sirius"
					Return result
				End If
				If Not Information.IsArray(vUserData) Then
					result = gPMConstants.PMEReturnCode.PMFalse
					sMessage = "Unable to find user ID for User Sirius"
					Return result
				End If
			End With
			

            m_iUserID = CInt(vUserData(0, 0))

            m_iLanguageID = CInt(vUserData(1, 0))
            m_iSourceID = 1
            m_iCurrencyID = 26
            m_iLogLevel = 9

            ' it's being called by the scheduler
            ' don't show a login prompt
            m_lReturn = m_oObjectManager.InitialiseWithUserState(sUsername:=m_sUsername.Value, sPassword:=m_sPassword.Value, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iCountryID:=1, iLanguageId:=m_iLanguageID, iLogLevel:=m_iLogLevel, iCurrencyID:=m_iCurrencyID, lPartyCnt:=0, sCallingAppName:=m_sCallingAppName)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.

                ' Set the object manager to nothing.
                m_oObjectManager = Nothing

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With m_oObjectManager
                m_iLanguageID = .LanguageID
                m_iSourceID = .SourceID
            End With

            '    '**************
            '    ' initialisation
            '    m_bCloseDatabase = True
            '    m_sUsername = "siriuscomm"
            '    m_sPassword = "NGMBMKqSMcc5"
            '    m_iUserID = 1
            '    m_sCallingAppName = ACApp
            '    m_iSourceID = 1
            '    m_iLanguageID = 1
            '    m_iCurrencyID = 26
            '    m_iLogLevel = 6
            '    '**************
            '
            '    m_lReturn = gPMComponentServices.CheckDatabase( _
            ''                        v_sUsername:=m_sUsername, _
            ''                        v_iSourceID:=m_iSourceID, _
            ''                        v_iLanguageID:=m_iLanguageID, _
            ''                        v_lPMProductFamily:=pmePFSiriusSolutions, _
            ''                        r_bNewInstanceCreated:=m_bCloseDatabase, _
            ''                        r_oCheckedDatabase:=m_oDatabase)
            '
            '    If (m_lReturn <> PMTrue) Then
            '
            '        Initialise = PMFalse
            '
            '        LogMessageToFile _
            ''            sUserName:=m_sUsername, _
            ''            iType:=PMLogOnError, _
            ''            sMsg:=sFunctionName & " Failed to create database", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:=sFunctionName
            '
            '        Exit Function
            '
            '    End If
            '
            Return result

    End Function


    ' ***************************************************************** '
    ' Name: ProcessOverdueTaskCheck
    '
    ' Parameters: n/a
    '
    ' Description: check for overdue tasks
    '
    ' History:
    '           Created : DC : 020704
    ' ***************************************************************** '
    Private Function ProcessOverdueTaskCheck() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "ProcessOverdueTaskCheck"

        Dim vOverdueTasks As Object
        Dim llBound, lUBound As Integer
        Dim bTransactionError As Boolean

        Dim lpmwrktaskinstancecnt As Integer

        

            result = gPMConstants.PMEReturnCode.PMTrue

            ' get any overdue tasks
            If GetOverdueTasks(r_vResults:=vOverdueTasks) = gPMConstants.PMEReturnCode.PMTrue Then

                ' if we have any overdue tasks to process
                If Information.IsArray(vOverdueTasks) Then

                    ' get the array boundaries

                    llBound = vOverdueTasks.GetLowerBound(1)

                    lUBound = vOverdueTasks.GetUpperBound(1)

                    ' for each overdue task item
                    For lOverdueItem As Integer = llBound To lUBound

                        ' get the item details from the array

                        ' outstanding task

                        If CStr(vOverdueTasks(ACPMWrkTaskInstanceCnt, lOverdueItem)) = "" Then
                            lpmwrktaskinstancecnt = 0
                        Else

                            lpmwrktaskinstancecnt = CInt(vOverdueTasks(ACPMWrkTaskInstanceCnt, lOverdueItem))
                        End If

                        If CreateTaskToAlertSupervisor(v_lpmwrktaskinstancecnt:=lpmwrktaskinstancecnt) <> gPMConstants.PMEReturnCode.PMTrue Then

                            bTransactionError = True
                            Exit For
                        End If

                    Next lOverdueItem

                End If

            End If



        Return result

    End Function


    ' ***************************************************************** '
    ' Name: GetOverdueTasks
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : DC : 020704 :
    ' ***************************************************************** '
    Private Function GetOverdueTasks(ByRef r_vResults(,) As Object) As Integer 

        Dim result As Integer = 0 
        Const sFunctionName As String = "GetOverdueTasks"

        

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=ACGetOverdueTasksSQL, sSQLName:=ACGetOverdueTasksName, bStoredProcedure:=True, vResultArray:=r_vResults) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername.Value, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to retrieve over batch task details", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))
                '******************************

            End If

            Return result

    End Function


    ' ***************************************************************** '
    ' Name: CreateTaskToAlertSupervisor
    '
    ' Parameters: n/a
    '
    ' Description: Create Task For Supervisor To Be Alerted Of Oustanding Task
    '
    ' History:
    '           Created : DC : 020704 :
    ' ***************************************************************** '
    Private Function CreateTaskToAlertSupervisor(ByVal v_lpmwrktaskinstancecnt As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "CreateTaskToAlertSupervisor"

        

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add the ID parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="os_pmwrk_task_instance_cnt", vValue:=CStr(v_lpmwrktaskinstancecnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=ACCreateCheckTaskSQL, sSQLName:=ACCreateCheckTaskName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lpmwrktaskinstancecnt", v_lpmwrktaskinstancecnt)
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername.Value, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to create alerting task", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
                '******************************

            End If

            Return result

    End Function
End Module

