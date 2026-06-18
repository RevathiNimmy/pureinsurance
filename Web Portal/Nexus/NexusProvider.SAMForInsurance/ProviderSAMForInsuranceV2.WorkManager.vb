Imports NexusProvider.SAMForInsurance.PureService
Imports System.ServiceModel
Imports System.Configuration.Provider
Imports System.Web
Imports System.Web.HttpContext
Imports System.Web.Configuration.WebConfigurationManager
Imports Microsoft.Web.Services3.Security.Tokens
Imports SiriusFS.SAM.Client.Security
Imports System.Xml.Serialization
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports System.Diagnostics
Imports System.Text
Imports System.Xml

Partial Public Class ProviderSAMForInsuranceV2

    Public Overrides Sub UpdateTaskGroups(ByRef oTaskGroup As TaskGroup,
                                              Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock

            Dim oSAM As PureServiceClient 'SAMForInsuranceV2's Object
            Dim oUpdatetaskgrouptasksRequest As UpdateTaskGroupsRequestType  ' Request Type
            Dim oUpdatetaskgrouptasksResponse As UpdateTaskGroupsResponseType  ' Response Type
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oUpdatetaskgrouptasksRequest = New UpdateTaskGroupsRequestType
                oUpdatetaskgrouptasksResponse = New UpdateTaskGroupsResponseType
                sbLogMessage = New StringBuilder


                With oUpdatetaskgrouptasksRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty 
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If
                    'WorkManager Request
                    If oTaskGroup.TaskGroupKey = 0 Then
                        Throw New ArgumentNullException("WorkManager")
                    Else
                        .TaskGroupKey = oTaskGroup.TaskGroupKey
                        .Code = oTaskGroup.Code
                        .Description = oTaskGroup.Description
                        .CaptionId = oTaskGroup.CaptionId
                        .IsDeleted = oTaskGroup.IsDeleted
                        .TaskGroupCategoryKey = oTaskGroup.TaskGroupCategoryKey
                        .EffectiveDate = oTaskGroup.EffectiveDate
                    End If

                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oUpdatetaskgrouptasksResponse = oSAM.UpdateTaskGroups(oUpdatetaskgrouptasksRequest)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                ' Disposing the SAM's object


                With oUpdatetaskgrouptasksResponse  'With Response Type
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        'WorkManager Response
                        'Fetching from the  WorkManager Response Collection 
                        'No Response Object..
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("WorkManager executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & oTaskGroup.Print() & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & oTaskGroup.Print() & "results" & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oUpdatetaskgrouptasksRequest = Nothing
                oUpdatetaskgrouptasksResponse = Nothing
            End Try


        End SyncLock
    End Sub

    Public Overrides Sub UpdateTaskGroupTasks(ByRef oTaskGroup As TaskGroup,
                                              Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock
            Dim oSAM As PureServiceClient 'SAMForInsuranceV2's Object
            Dim oUpdatetaskgrouptasksRequest As UpdateTaskGroupTasksRequestType    ' Request Type
            Dim oUpdatetaskgrouptasksResponse As UpdateTaskGroupTasksResponseType    ' Response Type
            Dim oNewKey As Task
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oUpdatetaskgrouptasksRequest = New UpdateTaskGroupTasksRequestType
                oUpdatetaskgrouptasksResponse = New UpdateTaskGroupTasksResponseType
                sbLogMessage = New StringBuilder


                With oUpdatetaskgrouptasksRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty 
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If
                    'WorkManager Request
                    If oTaskGroup.TaskGroupKey = String.Empty Then
                        Throw New ArgumentNullException("WorkManager")
                    Else
                        .TaskGroupKey = oTaskGroup.TaskGroupKey
                        .TimeStamp = oTaskGroup.TimeStamp


                        oNewKey = New Task
                        .Tasks = New List(Of BaseUpdateTaskGroupTasksRequestTypeRow)
                        For Each oGroup As BaseUpdateTaskGroupTasksRequestTypeRow In .Tasks
                            oGroup.TaskCode = oNewKey.TaskCode
                            oGroup.DisplaySequence = oNewKey.DisplaySequence
                            oGroup.DisplaySequenceSpecified = oNewKey.DisplaySequenceSpecified
                        Next
                    End If
                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oUpdatetaskgrouptasksResponse = oSAM.UpdateTaskGroupTasks(oUpdatetaskgrouptasksRequest)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                ' Disposing the SAM's object


                With oUpdatetaskgrouptasksResponse  'With Response Type
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        'WorkManager Response
                        'Fetching from the  WorkManager Response Collection 
                        'oUpdatetaskgrouptasksResponse.TimeStamp
                        oTaskGroup.TimeStamp = .TimeStamp
                    End If
                End With


                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("WorkManager executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & oTaskGroup.Print() & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & oTaskGroup.Print() & "results" & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oUpdatetaskgrouptasksRequest = Nothing
                oUpdatetaskgrouptasksResponse = Nothing
            End Try


        End SyncLock
    End Sub

    Public Overrides Sub UpdateUserGroup(ByRef r_oWorkManager As WorkManager,
                                        Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock
            Dim oSAM As PureServiceClient 'SAMForInsuranceV2's Object
            Dim oUpdateUserGroupRequest As UpdateUserGroupRequestType   ' Request Type
            Dim oUpdateUserGroupResponse As UpdateUserGroupResponseType   ' Response Type
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oUpdateUserGroupRequest = New UpdateUserGroupRequestType
                oUpdateUserGroupResponse = New UpdateUserGroupResponseType
                sbLogMessage = New StringBuilder


                With oUpdateUserGroupRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty 
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If
                    'WorkManager Request
                    If r_oWorkManager.UserGroupKey = 0 Then
                        Throw New ArgumentNullException("UserGroupKey")
                    Else
                        .UserGroupKey = r_oWorkManager.UserGroupKey
                        .Code = r_oWorkManager.Code
                        .Description = r_oWorkManager.Description
                        .EffectiveDate = r_oWorkManager.EffectiveDate
                        .IsDeleted = r_oWorkManager.IsDeleted
                        .IsSysAdmin = r_oWorkManager.IsSysAdmin
                    End If
                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oUpdateUserGroupResponse = oSAM.UpdateUserGroup(oUpdateUserGroupRequest)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                ' Disposing the SAM's object


                With oUpdateUserGroupResponse  'With Response Type
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        'WorkManager Response
                        'Fetching from the UserGroup Response Collection 
                        'No Response Object..
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("WorkManager executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & r_oWorkManager.Print() & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & r_oWorkManager.Print() & "results" & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oUpdateUserGroupRequest = Nothing
                oUpdateUserGroupResponse = Nothing
            End Try


        End SyncLock
    End Sub

    '     Public Overrides sub GetWmTask(ByVal r_oWorkManager As WorkManager, _
    '                                            Optional ByVal v_sBranchCode As String = Nothing)
    'For  converting the Sub to Function with TaskLogCollection as returnType.
    Public Overrides Function GetWmTask(ByVal r_oWorkManager As WorkManager,
                                                Optional ByVal v_sBranchCode As String = Nothing) As WorkManager
        SyncLock oLock

            Dim oSAM As PureServiceClient 'SAMForInsuranceV2's Object
            Dim oGetWmTaskRequest As GetWmTaskRequestType ' Request Type
            Dim oGetWmTaskResponse As GetWmTaskResponseType  ' Response Type
            Dim oNewKey As KeyData
            Dim oNewKeyDataCollection As KeyDataCollection = Nothing
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetWmTaskRequest = New GetWmTaskRequestType
                oGetWmTaskResponse = New GetWmTaskResponseType
                sbLogMessage = New StringBuilder

                With oGetWmTaskRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty 
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    'WorkManager Request
                    If r_oWorkManager.TaskInstanceKey = 0 Then
                        Throw New ArgumentNullException("WorkManager.TaskInstanceKey")
                    Else
                        .TaskInstanceKey = r_oWorkManager.TaskInstanceKey
                    End If
                End With

                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oGetWmTaskResponse = oSAM.GetWmTask(oGetWmTaskRequest)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                ' Disposing the SAM's object


                With oGetWmTaskResponse  'With Response Type
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else


                        r_oWorkManager.TaskInstanceKey = oGetWmTaskResponse.TaskInstanceKey
                        r_oWorkManager.TaskGroupKey = oGetWmTaskResponse.TaskGroupKey
                        r_oWorkManager.TaskGroupCode = oGetWmTaskResponse.TaskGroupCode
                        r_oWorkManager.TaskKey = oGetWmTaskResponse.TaskKey
                        r_oWorkManager.TaskCode = oGetWmTaskResponse.TaskCode
                        r_oWorkManager.Customer = oGetWmTaskResponse.Customer
                        r_oWorkManager.DueDate = oGetWmTaskResponse.DueDate
                        r_oWorkManager.UserGroupKey = oGetWmTaskResponse.UserGroupKey
                        r_oWorkManager.UserGroupCode = oGetWmTaskResponse.UserGroupCode
                        r_oWorkManager.UserKey = oGetWmTaskResponse.UserKey

                        r_oWorkManager.UserCode = oGetWmTaskResponse.UserCode
                        r_oWorkManager.Description = oGetWmTaskResponse.Description
                        r_oWorkManager.TaskStatusKey = oGetWmTaskResponse.TaskStatusKey
                        r_oWorkManager.IsUrgent = oGetWmTaskResponse.IsUrgent
                        r_oWorkManager.IsTaskReview = oGetWmTaskResponse.IsTaskReview
                        r_oWorkManager.CreatedByKey = oGetWmTaskResponse.CreatedByKey
                        r_oWorkManager.DateCreated = oGetWmTaskResponse.DateCreated
                        r_oWorkManager.ModifiedByKey = oGetWmTaskResponse.ModifiedByKey
                        r_oWorkManager.LastModified = oGetWmTaskResponse.LastModified
                        r_oWorkManager.Isvisible = oGetWmTaskResponse.Isvisible
                        r_oWorkManager.WorkflowInformation = oGetWmTaskResponse.WorkflowInformation
                        r_oWorkManager.TimeStamp = oGetWmTaskResponse.TaskTimestamp
                        r_oWorkManager.CreatedBy = oGetWmTaskResponse.CreatedByUser
                        r_oWorkManager.ModifiedBy = oGetWmTaskResponse.ModifiedByUser

                        'Code Changed  on December 24th Begin 


                        oNewKeyDataCollection = New KeyDataCollection
                        If .KeyData IsNot Nothing AndAlso .KeyData.Count > 0 Then
                            For Each oKey As BaseGetWmTaskResponseTypeRow In .KeyData
                                oNewKey = New KeyData
                                oNewKey.KeyName = oKey.KeyName
                                oNewKey.KeyValue = oKey.KeyValue
                                oNewKeyDataCollection.Add(oNewKey)
                            Next
                            r_oWorkManager.KeyData = oNewKeyDataCollection
                            'Code Changed  on December 24th End 
                        End If
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("WorkManager executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & r_oWorkManager.Print() & vbCrLf)
                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & r_oWorkManager.Print() & " results" & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetWmTaskRequest = Nothing
                oGetWmTaskResponse = Nothing
            End Try

            Return r_oWorkManager

        End SyncLock
    End Function
    ' Public Overrides sub GetWmTaskLog(ByVal r_oWorkManager As WorkManager, _
    '                                           Optional ByVal v_sBranchCode As String = Nothing)
    ' For  converting the Sub to Function with TaskLogCollection as returnType.
    Public Overrides Function GetWmTaskLog(ByVal r_oWorkManager As WorkManager,
                                                Optional ByVal v_sBranchCode As String = Nothing) As TaskLogCollection
        SyncLock oLock

            Dim oSAM As PureServiceClient 'SAMForInsuranceV2's Object
            Dim oGetWmTaskLogRequest As GetWmTaskLogRequestType  ' Request Type
            Dim oGetWmTaskLogResponse As GetWmTaskLogResponseType   ' Response Type
            Dim oNewTaskLog As TaskLog   'Object of WorkManager Class
            Dim oNewTaskLogCollection As TaskLogCollection = Nothing
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetWmTaskLogRequest = New GetWmTaskLogRequestType
                oGetWmTaskLogResponse = New GetWmTaskLogResponseType
                sbLogMessage = New StringBuilder

                With oGetWmTaskLogRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty 
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    'WorkManager Request

                    'if r_oWorkManager.TaskInstanceKey is string.empty then 
                    If r_oWorkManager.TaskInstanceKey = 0 Then 'Changed  on 23rd December

                        Throw New ArgumentNullException("WorkManager.TaskInstanceKey")
                    Else
                        .TaskInstanceKey = r_oWorkManager.TaskInstanceKey
                    End If
                End With

                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oGetWmTaskLogResponse = oSAM.GetWmTaskLog(oGetWmTaskLogRequest)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                ' Disposing the SAM's object



                oNewTaskLogCollection = New TaskLogCollection

                With oGetWmTaskLogResponse  'With Response Type
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        'WorkManager Response
                        'Fetching from the  WorkManager Response Collection 
                        'Code Changed  on December 24th Begin 
                        If .TaskLog IsNot Nothing AndAlso .TaskLog.Count > 0 Then
                            For Each oTask As BaseGetWmTaskLogResponseTypeTaskLogRow In .TaskLog
                                oNewTaskLog = New NexusProvider.TaskLog
                                oNewTaskLog.CreatedByKey = oTask.CreatedByKey
                                oNewTaskLog.DateCreated = oTask.DateCreated
                                oNewTaskLog.LogText = oTask.LogText
                                oNewTaskLog.TaskInstanceKey = oTask.TaskInstanceKey
                                oNewTaskLog.UserName = oTask.UserName
                                oNewTaskLogCollection.Add(oNewTaskLog)
                                'Code Changed  on December 24th End 
                            Next
                        End If
                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("WorkManager executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & r_oWorkManager.Print() & vbCrLf)
                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & r_oWorkManager.Print() & "results" & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetWmTaskLogRequest = Nothing
                oGetWmTaskLogResponse = Nothing
            End Try


            Return oNewTaskLogCollection
        End SyncLock
    End Function

    Public Overrides Function GetWorkManagerScheduledTasks(ByVal oWorkManager As WorkManager,
                                           Optional ByVal v_sBranchCode As String = Nothing) As WorkManagerCollection
        SyncLock oLock

            Dim oSAM As PureServiceClient 'SAMForInsuranceV2's Object
            Dim oGetWmScheduledtasksRequest As GetWorkManagerScheduledTasksRequestType   ' Request Type
            Dim oNewWorkManager As WorkManager  'Object of WorkManager Class
            Dim oNewWorkCollection As WorkManagerCollection = Nothing
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetWmScheduledtasksRequest = New GetWorkManagerScheduledTasksRequestType
                sbLogMessage = New StringBuilder

                Static oGetWmScheduledtasksResponse As New GetWorkManagerScheduledTasksResponseType    ' Response Type

                With oGetWmScheduledtasksRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty 
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If
                    'WorkManager Request
                    'Added on 22 DEC for the creation of WorkManager.aspx 
                    '------------------------------------------------------
                    'Enum Values for Task Status
                    Select Case oWorkManager.TaskStatus
                        Case TaskStatus.All
                            .TaskStatusKey = TaskStatus.All
                            .TaskStatusKeySpecified = True
                        Case TaskStatus.Complete
                            .TaskStatusKey = TaskStatus.Complete
                            .TaskStatusKeySpecified = True
                        Case TaskStatus.InComplete
                            .TaskStatusKey = TaskStatus.InComplete
                            .TaskStatusKeySpecified = True
                        Case TaskStatus.InProgress
                            .TaskStatusKey = TaskStatus.InProgress
                            .TaskStatusKeySpecified = True
                        Case TaskStatus.[New]
                            .TaskStatusKey = TaskStatus.[New]
                            .TaskStatusKeySpecified = True
                        Case TaskStatus.NotComplete
                            .TaskStatusKey = TaskStatus.NotComplete
                            .TaskStatusKeySpecified = True
                    End Select
                    ' EnumValues for DateRange
                    Select Case oWorkManager.DateRange
                        Case DateRange.AllDates
                            .Date = DateRange.AllDates
                            .DateSpecified = True
                        Case DateRange.Next14Days
                            .Date = DateRange.Next14Days
                            .DateSpecified = True
                        Case DateRange.Next28Days
                            .Date = DateRange.Next28Days
                            .DateSpecified = True
                        Case DateRange.Next2Days
                            .Date = DateRange.Next2Days
                            .DateSpecified = True
                        Case DateRange.Next3Days
                            .Date = DateRange.Next3Days
                            .DateSpecified = True
                        Case DateRange.Next4Days
                            .Date = DateRange.Next4Days
                            .DateSpecified = True
                        Case DateRange.Next5Days
                            .Date = DateRange.Next5Days
                            .DateSpecified = True
                        Case DateRange.Next6Days
                            .Date = DateRange.Next6Days
                            .DateSpecified = True
                        Case DateRange.Next7Days
                            .Date = DateRange.Next7Days
                            .DateSpecified = True
                        Case DateRange.Today
                            .Date = DateRange.Today
                            .DateSpecified = True
                        Case DateRange.Tomorrow
                            .Date = DateRange.Tomorrow
                            .DateSpecified = True
                    End Select
                    'EnumValues for ShowType
                    Select Case oWorkManager.ShowType
                        Case ShowType.All
                            .ShowSystemKEY = ShowType.All
                            .ShowSystemKEYSpecified = True
                        Case ShowType.Sys
                            .ShowSystemKEY = ShowType.Sys
                            .ShowSystemKEYSpecified = True
                        Case ShowType.User
                            .ShowSystemKEY = ShowType.User
                            .ShowSystemKEYSpecified = True
                    End Select

                    'Added on 22 DEC for the creation of WorkManager.aspx 
                    '---------------------------------------------------------------------
                    .UserGroupCODE = oWorkManager.UserGroupCode
                    .UserCODE = oWorkManager.UserCode
                    .PartyKey = oWorkManager.PartyKey
                End With

                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oGetWmScheduledtasksResponse = oSAM.GetWorkManagerScheduledTasks(oGetWmScheduledtasksRequest)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                ' Disposing the SAM's object




                oNewWorkCollection = New WorkManagerCollection

                With oGetWmScheduledtasksResponse  'With Response Type
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        'WorkManager Response
                        'Fetching from the  WorkManager Response Collection 
                        If .Tasks IsNot Nothing AndAlso .Tasks.Count > 0 Then
                            For Each oWork As BaseGetWorkManagerScheduledTasksResponseTypeRow In .Tasks
                                oNewWorkManager = New NexusProvider.WorkManager
                                oNewWorkManager.Urgent = oWork.Urgent
                                oNewWorkManager.TaskInstanceKey = oWork.TaskInstanceKey
                                oNewWorkManager.DueDate = oWork.DueDate
                                oNewWorkManager.Description = oWork.Description
                                oNewWorkManager.Customer = oWork.Customer
                                oNewWorkManager.Branch = oWork.Branch
                                oNewWorkManager.Type = oWork.Type
                                oNewWorkManager.UserGroupKey = oWork.UserGroupKey
                                oNewWorkManager.UserKey = oWork.UserKey
                                oNewWorkManager.TaskStatusKey = oWork.TaskStatusKey 'Changed  on 5-1-2009
                                oNewWorkManager.UserGroupCode = oWork.UserGroupCode
                                oNewWorkManager.UserGroupDescription = oWork.UserGroupDescription
                                oNewWorkManager.UserCode = oWork.UserCode
                                oNewWorkManager.TaskGroupKey = oWork.TaskGroupKey
                                oNewWorkManager.TaskKey = oWork.TaskKey
                                oNewWorkManager.PartyName = oWork.PartyName
                                oNewWorkManager.InsuranceFolderKey = oWork.InsuranceFolderKey
                                oNewWorkCollection.Add(oNewWorkManager)
                            Next
                        End If
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("WorkManager executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & oWorkManager.Print() & vbCrLf)
                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & oNewWorkCollection.Print() & "results" & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetWmScheduledtasksRequest = Nothing
            End Try


            Return oNewWorkCollection  'Returning WorkManager Collection
        End SyncLock
    End Function

    Public Overrides Sub UpdateWmTask(ByRef r_oWorkManager As WorkManager,
                                          Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock

            Dim oSAM As PureServiceClient 'SAMForInsuranceV2's Object
            Dim oUpdatewmTasktasksRequest As UpdateWmTaskRequestType ' Request Type
            Dim oUpdatewmTasktasksResponse As UpdateWmTaskResponseType ' Response Type
            Dim iKeyDataCount As Integer
            Dim oNewKey(r_oWorkManager.KeyData.Count) As BaseUpdateWmTaskRequestTypeRow
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oUpdatewmTasktasksRequest = New UpdateWmTaskRequestType
                oUpdatewmTasktasksResponse = New UpdateWmTaskResponseType
                sbLogMessage = New StringBuilder


                With oUpdatewmTasktasksRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty 
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    'WorkManager Request
                    If r_oWorkManager.TaskInstanceKey = 0 Then
                        Throw New ArgumentNullException("WorkManager")
                    Else
                        .TaskInstanceKey = r_oWorkManager.TaskInstanceKey
                        .DueDate = r_oWorkManager.DueDate
                        .Client = r_oWorkManager.Client
                        .Description = r_oWorkManager.Description
                        .Urgent = r_oWorkManager.IsUrgentForUpdate
                        .TaskStatusKey = r_oWorkManager.TaskStatusKey
                        .UserGroupCode = r_oWorkManager.UserGroupCode
                        .UserCode = r_oWorkManager.UserCode
                        .TaskTimeStamp = r_oWorkManager.TaskTimeStamp
                        .ActionType = WMActionType.Update



                        For iKeyDataCount = 0 To r_oWorkManager.KeyData.Count - 1
                            oNewKey(iKeyDataCount) = New BaseUpdateWmTaskRequestTypeRow
                            oNewKey(iKeyDataCount).KeyName = r_oWorkManager.KeyData(iKeyDataCount).KeyName
                            oNewKey(iKeyDataCount).KeyValue = r_oWorkManager.KeyData(iKeyDataCount).KeyValue
                        Next

                    End If
                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oUpdatewmTasktasksResponse = oSAM.UpdateWmTask(oUpdatewmTasktasksRequest)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                ' Disposing the SAM's object


                With oUpdatewmTasktasksResponse  'With Response Type
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        'WorkManager Response
                        'Fetching from the  WorkManager Response Collection 
                        r_oWorkManager.TaskTimeStamp = .TaskTimeStamp
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("WorkManager executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & r_oWorkManager.Print() & vbCrLf)
                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & r_oWorkManager.Print() & "results" & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oUpdatewmTasktasksRequest = Nothing
                oUpdatewmTasktasksResponse = Nothing
            End Try



        End SyncLock
    End Sub

    Public Overrides Sub ReAssignMultipleWMTasks(ByVal v_oWorkManagerCollection As WorkManagerCollection,
                                                           Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oReAssignMultipleWMTasksRequest As ReAssignMultipleWmTasksRequestType
            Dim oReAssignMultipleWMTasksResponse As ReAssignMultipleWmTasksResponseType
            Dim iCounter As Integer
            Dim oReAssignMultipleWmTasks(v_oWorkManagerCollection.Count) As BaseReAssignMultipleWmTasksRequestTypeRow
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oReAssignMultipleWMTasksRequest = New ReAssignMultipleWmTasksRequestType
                oReAssignMultipleWMTasksResponse = New ReAssignMultipleWmTasksResponseType
                sbLogMessage = New StringBuilder


                With oReAssignMultipleWMTasksRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If

                    End If

                    For iCounter = 0 To v_oWorkManagerCollection.Count - 1
                        oReAssignMultipleWmTasks(iCounter) = New BaseReAssignMultipleWmTasksRequestTypeRow

                        If (v_oWorkManagerCollection(iCounter).TaskInstanceKey > 0) Then
                            oReAssignMultipleWmTasks(iCounter).TaskInstanceKey = v_oWorkManagerCollection(iCounter).TaskInstanceKey
                        Else
                            Throw New ArgumentNullException("ReAssignMultipleWmTasks.TaskInstanceKey")
                        End If

                        oReAssignMultipleWmTasks(iCounter).UserCode = v_oWorkManagerCollection(iCounter).UserCode
                        oReAssignMultipleWmTasks(iCounter).UserGroupCode = v_oWorkManagerCollection(iCounter).UserGroupCode

                    Next

                End With


                Using trace As New Tracer(Category.Trace)
                    oReAssignMultipleWMTasksResponse = oSAM.ReAssignMultipleWmTasks(oReAssignMultipleWMTasksRequest)
                End Using

                With oReAssignMultipleWMTasksResponse

                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("ReAssignMultipleWMTasksResponse executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine(v_oWorkManagerCollection.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oReAssignMultipleWMTasksRequest = Nothing
                oReAssignMultipleWMTasksResponse = Nothing
            End Try


        End SyncLock
    End Sub

    Public Overrides Sub AddTaskGroup(ByRef v_oAddTaskGroup As TaskGroup,
                    Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oAddTaskGroupRequest As BaseAddTaskGroupRequestType
            Dim oAddTaskGroupResponse As BaseAddTaskGroupResponseType
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oAddTaskGroupRequest = New BaseAddTaskGroupRequestType
                oAddTaskGroupResponse = New BaseAddTaskGroupResponseType
                sbLogMessage = New StringBuilder


                With oAddTaskGroupRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .Code = v_oAddTaskGroup.Code
                    .Description = v_oAddTaskGroup.Description

                    If v_oAddTaskGroup.CaptionId > 0 Then
                        .CaptionId = v_oAddTaskGroup.CaptionId
                    Else
                        Throw New ArgumentException("TaskGroup.CaptionId")
                    End If
                    If v_oAddTaskGroup.EffectiveDate = DateTime.MinValue Then
                        Throw New ArgumentException("TaskGroup.EffectiveDate")
                    Else
                        .EffectiveDate = v_oAddTaskGroup.EffectiveDate
                    End If

                    .IsDeleted = v_oAddTaskGroup.IsDeleted

                    If v_oAddTaskGroup.TaskGroupCategoryKey > 0 Then
                        .TaskGroupCategoryKey = v_oAddTaskGroup.TaskGroupCategoryKey
                    Else
                        Throw New ArgumentException("TaskGroup.TaskGroupCategoryKey")
                    End If
                End With


                Using trace As New Tracer(Category.Trace)
                    oAddTaskGroupResponse = oSAM.AddTaskGroup(oAddTaskGroupRequest)
                End Using

                With oAddTaskGroupResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else

                        v_oAddTaskGroup.TaskGroupKey = .TaskGroupKey
                        v_oAddTaskGroup.TimeStamp = .TimeStamp
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("AddTaskGroup executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Input: " & vbCrLf)
                    sbLogMessage.AppendLine(v_oAddTaskGroup.Print.Replace("<br />", vbCrLf))

                    sbLogMessage.AppendLine("Output: " & vbCrLf)
                    If Not IsNothing(v_oAddTaskGroup.TaskGroupKey) Then
                        sbLogMessage.AppendLine("Task Group Key: " & v_oAddTaskGroup.TaskGroupKey.ToString() & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Task Group Key: nothing" & vbCrLf)
                    End If

                    If Not IsNothing(v_oAddTaskGroup.TimeStamp) Then
                        sbLogMessage.AppendLine("Time Stamp: " & v_oAddTaskGroup.TimeStamp.ToString() & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Time Stamp: nothing" & vbCrLf)
                    End If

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oAddTaskGroupRequest = Nothing
                oAddTaskGroupResponse = Nothing
            End Try


        End SyncLock
    End Sub

    Public Overrides Sub AddUserGroup(ByRef v_oAddUserGroup As UserGroup,
                Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oAddUserGroupRequest As BaseAddUserGroupRequestType
            Dim oAddUserGroupResponse As BaseAddUserGroupResponseType
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oAddUserGroupRequest = New BaseAddUserGroupRequestType
                oAddUserGroupResponse = New BaseAddUserGroupResponseType
                sbLogMessage = New StringBuilder


                With oAddUserGroupRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .Code = v_oAddUserGroup.Code
                    .Description = v_oAddUserGroup.Description

                    If v_oAddUserGroup.EffectiveDate = DateTime.MinValue Then
                        Throw New ArgumentException("UserGroup.EffectiveDate")
                    Else
                        .EffectiveDate = v_oAddUserGroup.EffectiveDate
                    End If

                    .IsDeleted = v_oAddUserGroup.IsDeleted
                    .IsSysAdmin = v_oAddUserGroup.IsSysAdmin
                End With


                Using trace As New Tracer(Category.Trace)
                    oAddUserGroupResponse = oSAM.AddUserGroup(oAddUserGroupRequest)
                End Using

                With oAddUserGroupResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        v_oAddUserGroup.UserGroupKey = .UserGroupKey
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("AddUserGroup executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Input: " & vbCrLf)
                    sbLogMessage.AppendLine(v_oAddUserGroup.Print().Replace("<br />", vbCrLf))

                    sbLogMessage.AppendLine("Output: " & vbCrLf)
                    If Not IsNothing(v_oAddUserGroup.UserGroupKey) Then
                        sbLogMessage.AppendLine("Task Group Key: " & v_oAddUserGroup.UserGroupKey.ToString() & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Task Group Key: nothing" & vbCrLf)
                    End If

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oAddUserGroupRequest = Nothing
                oAddUserGroupResponse = Nothing
            End Try

        End SyncLock
    End Sub

    Public Overrides Sub AddWmTaskLog(ByVal v_iTaskInstanceKey As Integer,
                ByVal v_sLogText As String,
                Optional ByVal v_sBranchCode As String = Nothing)


        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oAddWmTaskLogRequest As AddWmTaskLogRequestType
            Dim oAddWmTaskLogResponse As AddWmTaskLogResponseType
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oAddWmTaskLogRequest = New AddWmTaskLogRequestType
                oAddWmTaskLogResponse = New AddWmTaskLogResponseType
                sbLogMessage = New StringBuilder

                With oAddWmTaskLogRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If

                    If v_iTaskInstanceKey > 0 Then
                        .TaskInstanceKey = v_iTaskInstanceKey
                    Else
                        Throw New ArgumentException("WMTaskLog.TaskInstanceKey")
                    End If

                    .LogText = v_sLogText

                End With


                Using trace As New Tracer(Category.Trace)
                    oAddWmTaskLogResponse = oSAM.AddWmTaskLog(oAddWmTaskLogRequest)
                End Using

                With oAddWmTaskLogResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("AddWmTaskLog executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Input: " & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If IsNothing(v_iTaskInstanceKey) Then
                        sbLogMessage.AppendLine("Task Instance Key: nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Task Instance Key: " & v_iTaskInstanceKey & vbCrLf)
                    End If

                    If IsNothing(v_sLogText) Then
                        sbLogMessage.AppendLine("Log Text: nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Log Text: " & v_sLogText & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oAddWmTaskLogRequest = Nothing
                oAddWmTaskLogResponse = Nothing
            End Try


        End SyncLock
    End Sub

    Public Overrides Function CreateWmTask(ByVal v_oCreateWmTask As WorkManager,
                                           Optional ByVal v_sBranchCode As String = Nothing) As WorkManager

        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oCreateWmTaskRequest As CreateWmTaskRequestType
            Dim oCreateWmTaskResponse As CreateWmTaskResponseType
            Dim iCounter As Integer = 0
            Dim oKeyData As BaseCreateWmTaskRequestTypeRow
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oCreateWmTaskRequest = New CreateWmTaskRequestType
                oCreateWmTaskResponse = New CreateWmTaskResponseType
                sbLogMessage = New StringBuilder




                With oCreateWmTaskRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If

                    .AllocationUser = v_oCreateWmTask.AllocationUser
                    .AllocationUserGroup = v_oCreateWmTask.AllocationUserGroup
                    .Client = v_oCreateWmTask.Client
                    .Description = v_oCreateWmTask.Description
                    .IsExternalItem = v_oCreateWmTask.IsExternalItem
                    .ExternalTaskCategoryCode = v_oCreateWmTask.ExternalTaskCategoryCode
                    .ParentTaskId = v_oCreateWmTask.ParentTaskId
                    .GuidPMExternalItem = v_oCreateWmTask.GuidPMExternalItem
                    .LockName = v_oCreateWmTask.LockName
                    .LockValue = v_oCreateWmTask.LockValue
                    If v_oCreateWmTask.DueDate = DateTime.MinValue Then
                        Throw New ArgumentException("WmTask.DueDateTime")
                    Else
                        .DueDateTime = v_oCreateWmTask.DueDate
                    End If
                    .IsComplete = v_oCreateWmTask.IsComplete
                    .IsTaskReview = v_oCreateWmTask.IsTaskReview
                    .IsUrgent = v_oCreateWmTask.IsUrgent
                    .Task = v_oCreateWmTask.Task
                    .TaskGroup = v_oCreateWmTask.TaskGroup
                    .ExternalTaskStatus = v_oCreateWmTask.ExternalTaskStatus 'Send -1 if you do not want to use it.
                    .IsExternalChildTask = v_oCreateWmTask.IsExternalChildTask
                    'Adding KeyData Collection
                    If v_oCreateWmTask.KeyData IsNot Nothing AndAlso v_oCreateWmTask.KeyData.Count > 0 Then
                        Dim oKeyData(v_oCreateWmTask.KeyData.Count - 1) As BaseCreateWmTaskRequestTypeRow

                        .KeyData = New List(Of BaseCreateWmTaskRequestTypeRow)

                        For iCounter = 0 To v_oCreateWmTask.KeyData.Count - 1
                            oKeyData = New BaseCreateWmTaskRequestTypeRow
                            oKeyData.KeyName = v_oCreateWmTask.KeyData(iCounter).KeyName
                            oKeyData.KeyValue = v_oCreateWmTask.KeyData(iCounter).KeyValue
                            .KeyData.Add(oKeyData)
                        Next


                    End If
                End With


                Using trace As New Tracer(Category.Trace)
                    oCreateWmTaskResponse = oSAM.CreateWmTask(oCreateWmTaskRequest)
                End Using


                With oCreateWmTaskResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        oWorkManager.GuidPMExternalItem = oCreateWmTaskResponse.GuidPMExternalItem
                        oWorkManager.TaskInstanceKey = oCreateWmTaskResponse.TaskInstanceKey

                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("CreateWmTask executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Input: " & vbCrLf)
                    sbLogMessage.AppendLine(v_oCreateWmTask.Print().Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If
                    LogMessageEntry(sbLogMessage)
                End If
                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oCreateWmTaskRequest = Nothing
                oCreateWmTaskResponse = Nothing
            End Try


        End SyncLock
    End Function

    Public Overrides Sub DeleteUndeleteUserGroup(ByVal v_bDeleted As Boolean,
                                              ByVal v_sUserGroupCode As String,
                                              Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oDeleteUndeleteUserGroupRequest As DeleteUndeleteUserGroupRequestType
            Dim oDeleteUndeleteUserGroupResponse As DeleteUndeleteUserGroupResponseType
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oDeleteUndeleteUserGroupRequest = New DeleteUndeleteUserGroupRequestType
                oDeleteUndeleteUserGroupResponse = New DeleteUndeleteUserGroupResponseType
                sbLogMessage = New StringBuilder


                With oDeleteUndeleteUserGroupRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode

                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .Deleted = v_bDeleted
                    .UserGroupCode = v_sUserGroupCode
                End With


                Using trace As New Tracer(Category.Trace)
                    oDeleteUndeleteUserGroupResponse = oSAM.DeleteUndeleteUserGroup(oDeleteUndeleteUserGroupRequest)
                End Using




                With oDeleteUndeleteUserGroupResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("DeleteUndeleteUserGroup executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oDeleteUndeleteUserGroupRequest = Nothing
                oDeleteUndeleteUserGroupResponse = Nothing
            End Try


        End SyncLock
    End Sub

    Public Overrides Sub DeleteWmTask(ByRef r_oWorkManager As WorkManager,
                             Optional ByVal v_sBranchCode As String = Nothing)


        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oDeleteWmTaskRequest As DeleteWmTaskRequestType
            Dim oDeleteWmTaskResponse As DeleteWmTaskResponseType
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oDeleteWmTaskRequest = New DeleteWmTaskRequestType
                oDeleteWmTaskResponse = New DeleteWmTaskResponseType
                sbLogMessage = New StringBuilder


                With oDeleteWmTaskRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .TaskInstanceKey = r_oWorkManager.TaskInstanceKey
                    .TaskTimeStamp = r_oWorkManager.TimeStamp

                End With


                Using trace As New Tracer(Category.Trace)
                    oDeleteWmTaskResponse = oSAM.DeleteWmTask(oDeleteWmTaskRequest)
                End Using




                With oDeleteWmTaskResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If
                    r_oWorkManager.TimeStamp = .TaskTimeStamp
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("DeleteWmTask executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If


                    sbLogMessage.AppendLine("Output : " & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)



            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oDeleteWmTaskRequest = Nothing
                oDeleteWmTaskResponse = Nothing
            End Try


        End SyncLock
    End Sub

    Public Overrides Function FindUsers(ByVal v_oFindUserSearchCriteria As FindUsersSearchCriteria,
                    Optional ByVal v_sBranchCode As String = Nothing) As UserCollection
        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oFindUsersRequest As FindUsersRequestType
            Dim oFindUsersResponse As FindUsersResponseType
            Dim oUsers As UserCollection = Nothing
            Dim oUser As User = Nothing
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oFindUsersRequest = New FindUsersRequestType
                oFindUsersResponse = New FindUsersResponseType
                sbLogMessage = New StringBuilder


                With oFindUsersRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .FullName = v_oFindUserSearchCriteria.FullName
                    .UserName = v_oFindUserSearchCriteria.UserName

                    If v_oFindUserSearchCriteria.MaxRowsToFetch > 0 Then
                        .MaxRowsToFetch = v_oFindUserSearchCriteria.MaxRowsToFetch
                        .MaxRowsToFetchSpecified = True
                    Else
                        .MaxRowsToFetchSpecified = False
                    End If

                End With


                Using trace As New Tracer(Category.Trace)
                    oFindUsersResponse = oSAM.FindUsers(oFindUsersRequest)
                End Using

                With oFindUsersResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If

                    If .Users IsNot Nothing AndAlso .Users.Count > 0 Then


                        oUsers = New UserCollection()

                        For Each oBaseUser As BaseFindUsersResponseTypeRow In .Users
                            oUser = New User()
                            With oUser
                                .UserId = oBaseUser.UserId
                                .UserName = oBaseUser.UserName
                                .FullName = oBaseUser.FullName
                                .EffectiveDate = oBaseUser.EffectiveDate
                            End With
                            oUsers.Add(oUser)
                        Next
                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("FindUsers executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Input : " & vbCrLf)
                    sbLogMessage.AppendLine(v_oFindUserSearchCriteria.Print().Replace("<br />", vbCrLf))
                    sbLogMessage.AppendLine("Output : " & vbCrLf)
                    'sbLogMessage.AppendLine(oUsers.Print().Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oFindUsersRequest = Nothing
                oFindUsersResponse = Nothing
            End Try


            Return oUsers
        End SyncLock
    End Function

    Public Overrides Function GetSubAgents(ByVal v_iInsuranceFileKey As Integer,
                            Optional ByVal v_sBranchCode As String = Nothing) As SubAgentCollection

        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oGetSubAgentsRequest As GetSubAgentsRequestType
            Dim oGetSubAgentsResponse As GetSubAgentsResponseType
            Dim oSubAgents As SubAgentCollection = Nothing
            Dim oSubAgent As SubAgent = Nothing
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetSubAgentsRequest = New GetSubAgentsRequestType
                oGetSubAgentsResponse = New GetSubAgentsResponseType
                sbLogMessage = New StringBuilder


                With oGetSubAgentsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    If v_iInsuranceFileKey > 0 Then
                        .InsuranceFileKey = v_iInsuranceFileKey
                    Else
                        Throw New ArgumentException("SubAgents.InsuranceFileKey")
                    End If

                End With


                Using trace As New Tracer(Category.Trace)
                    oGetSubAgentsResponse = oSAM.GetSubAgents(oGetSubAgentsRequest)
                End Using

                With oGetSubAgentsResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If

                    If .SubAgents IsNot Nothing AndAlso .SubAgents.Count > 0 Then

                        oSubAgents = New SubAgentCollection()

                        For Each oBaseSubAgent As BaseGetSubAgentsResponseTypeRow In .SubAgents
                            oSubAgent = New SubAgent()
                            With oSubAgent
                                .PartyKey = oBaseSubAgent.PartyKey
                                .Name = oBaseSubAgent.Name
                                .Code = oBaseSubAgent.Code
                                .Amount = oBaseSubAgent.Amount
                                .Percentage = oBaseSubAgent.Percentage
                            End With
                            oSubAgents.Add(oSubAgent)
                        Next
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetSubAgents executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If IsNothing(v_iInsuranceFileKey) Then
                        sbLogMessage.AppendLine("Insurance File Key : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Insurance File Key : " & v_iInsuranceFileKey & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output : " & vbCrLf)
                    If oSubAgents IsNot Nothing Then
                        sbLogMessage.AppendLine(oSubAgents.Print().Replace("<br />", vbCrLf))
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetSubAgentsRequest = Nothing
                oGetSubAgentsResponse = Nothing
            End Try


            Return oSubAgents
        End SyncLock
    End Function

    Public Overrides Function GetTaskGroupTasks(ByVal v_iTaskGroupKey As Integer,
                                                ByVal v_dtEffectiveDate As Date,
                                                Optional ByVal v_sBranchCode As String = Nothing) As TaskGroupCollection
        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oGetTaskGroupTasksRequest As GetTaskGroupTasksRequestType
            Dim oGetTaskGroupTasksResponse As GetTaskGroupTasksResponseType
            Dim oTaskGroupTaskCollection As TaskGroupCollection = Nothing
            'Dim oTaskGroupTasksDetails As TaskGroupTasksDetails = Nothing
            Dim oTaskGroupTask As TaskGroup = Nothing
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetTaskGroupTasksRequest = New GetTaskGroupTasksRequestType
                oGetTaskGroupTasksResponse = New GetTaskGroupTasksResponseType
                sbLogMessage = New StringBuilder


                With oGetTaskGroupTasksRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If

                    If v_iTaskGroupKey > 0 Then
                        .TaskGroupKey = v_iTaskGroupKey
                    Else
                        Throw New ArgumentException("TaskGroupTasks.TaskGroupKey")
                    End If

                    If v_dtEffectiveDate = DateTime.MinValue Then
                        Throw New ArgumentException("TaskGroupTasks.EffectiveDate")
                    Else
                        .EffectiveDate = v_dtEffectiveDate
                    End If
                End With


                Using trace As New Tracer(Category.Trace)
                    oGetTaskGroupTasksResponse = oSAM.GetTaskGroupTasks(oGetTaskGroupTasksRequest)
                End Using


                With oGetTaskGroupTasksResponse
                    'oTaskGroupTasksDetails = New TaskGroupTasksDetails
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If

                    If .TaskGroupTasks IsNot Nothing AndAlso .TaskGroupTasks.Count > 0 Then


                        oTaskGroupTaskCollection = New TaskGroupCollection
                        For Each oBaseTaskGroupTask As BaseGetTaskGroupTasksResponseTypeRow In .TaskGroupTasks
                            oTaskGroupTask = New TaskGroup()
                            oTaskGroupTask.TaskKey = oBaseTaskGroupTask.TaskKey
                            oTaskGroupTask.Name = oBaseTaskGroupTask.Name.Trim
                            oTaskGroupTask.EffectiveDate = oBaseTaskGroupTask.EffectiveDate
                            oTaskGroupTask.Description = oBaseTaskGroupTask.Description
                            oTaskGroupTask.IsDeleted = oBaseTaskGroupTask.IsDeleted
                            oTaskGroupTask.IsIncluded = oBaseTaskGroupTask.IsIncluded
                            oTaskGroupTask.IsViewOnly = oBaseTaskGroupTask.IsViewOnly
                            oTaskGroupTask.IsAvailable = oBaseTaskGroupTask.IsAvailable
                            oTaskGroupTask.TaskCategoryKey = oBaseTaskGroupTask.TaskCategoryKey
                            oTaskGroupTask.DisplayIcon = oBaseTaskGroupTask.DisplayIcon
                            oTaskGroupTaskCollection.Add(oTaskGroupTask)
                        Next
                        'oTaskGroupTasksDetails = New TaskGroupTasksDetails
                        'oTaskGroupTasksDetails.TaskGroupTasks = oTaskGroupTaskCollection
                        'oTaskGroupTasksDetails.TimeStamp = .TimeStamp
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetTaskGroupTasks executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If IsNothing(v_iTaskGroupKey) Then
                        sbLogMessage.AppendLine("Task Group Key : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Task Group Key : " & v_iTaskGroupKey.ToString() & vbCrLf)
                    End If

                    If IsNothing(v_dtEffectiveDate) Then
                        sbLogMessage.AppendLine("Effective Date : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Effective Date : " & v_dtEffectiveDate.ToString() & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output : " & vbCrLf)
                    sbLogMessage.AppendLine(oTaskGroupTaskCollection.Print().Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetTaskGroupTasksRequest = Nothing
                oGetTaskGroupTasksResponse = Nothing
            End Try


            Return oTaskGroupTaskCollection
        End SyncLock
    End Function

    Public Overrides Function GetTaskGroups(Optional ByVal v_sBranchCode As String = Nothing) As TaskGroupCollection
        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oGetTaskGroupsRequest As GetTaskGroupsRequestType
            Dim oGetTaskGroupsResponse As GetTaskGroupsResponseType
            Dim oTaskGroups As TaskGroupCollection = Nothing
            Dim oTaskGroup As TaskGroup = Nothing
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetTaskGroupsRequest = New GetTaskGroupsRequestType
                oGetTaskGroupsResponse = New GetTaskGroupsResponseType
                sbLogMessage = New StringBuilder


                With oGetTaskGroupsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If
                End With


                Using trace As New Tracer(Category.Trace)
                    oGetTaskGroupsResponse = oSAM.GetTaskGroups(oGetTaskGroupsRequest)
                End Using

                With oGetTaskGroupsResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If

                    If .TaskGroups IsNot Nothing AndAlso .TaskGroups.Count > 0 Then


                        oTaskGroups = New TaskGroupCollection()
                        For Each oBaseTaskGroup As BaseGetTaskGroupsResponseTypeRow In .TaskGroups
                            oTaskGroup = New TaskGroup()
                            With oTaskGroup
                                .CaptionId = oBaseTaskGroup.CaptionID
                                .Code = oBaseTaskGroup.Code
                                .Description = oBaseTaskGroup.Description
                                .EffectiveDate = oBaseTaskGroup.EffectiveDate
                                .IsDeleted = oBaseTaskGroup.IsDeleted
                                .TaskGroupCategoryKey = oBaseTaskGroup.TaskGroupCategoryKey
                                .TaskGroupKey = oBaseTaskGroup.TaskGroupKey
                            End With
                            oTaskGroups.Add(oTaskGroup)
                        Next
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetTaskGroup executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output : " & vbCrLf)
                    sbLogMessage.AppendLine(oTaskGroups.Print().Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetTaskGroupsRequest = Nothing
                oGetTaskGroupsResponse = Nothing
            End Try


            Return oTaskGroups

        End SyncLock
    End Function

    Public Overrides Function GetUserGroupTaskGroups(ByVal v_sUserGroupCode As String,
                                                         ByVal v_dtEffectiveDate As Date,
                                                         Optional ByVal v_sBranchCode As String = Nothing) As TaskGroup
        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oGetUserGroupTaskGroupsRequest As GetUserGroupTaskGroupsRequestType
            Dim oGetUserGroupTaskGroupsResponse As GetUserGroupTaskGroupsResponseType
            Dim oTaskGroups As TaskGroupCollection = Nothing
            Dim oUserGroupTaskGroupDetails As TaskGroup = Nothing
            Dim oTaskGroup As TaskGroup = Nothing
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetUserGroupTaskGroupsRequest = New GetUserGroupTaskGroupsRequestType
                oGetUserGroupTaskGroupsResponse = New GetUserGroupTaskGroupsResponseType
                sbLogMessage = New StringBuilder


                With oGetUserGroupTaskGroupsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If
                    .UserGroupCode = v_sUserGroupCode
                    If v_dtEffectiveDate = DateTime.MinValue Then
                        Throw New ArgumentException("UserGroupTaskGroups.EffectiveDate")
                    Else
                        .EffectiveDate = v_dtEffectiveDate
                    End If
                End With


                Using trace As New Tracer(Category.Trace)
                    oGetUserGroupTaskGroupsResponse = oSAM.GetUserGroupTaskGroups(oGetUserGroupTaskGroupsRequest)
                End Using

                With oGetUserGroupTaskGroupsResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If

                    If .TaskGroups IsNot Nothing AndAlso .TaskGroups.Count > 0 Then


                        oTaskGroups = New TaskGroupCollection()
                        For Each oBaseUserGroupTaskGroup As BaseGetUserGroupTaskGroupsResponseTypeRow In .TaskGroups
                            oTaskGroup = New TaskGroup()
                            With oTaskGroup
                                .Code = oBaseUserGroupTaskGroup.Code
                                .Description = oBaseUserGroupTaskGroup.Description
                                .EffectiveDate = oBaseUserGroupTaskGroup.EffectiveDate
                                .IsDeleted = oBaseUserGroupTaskGroup.IsDeleted
                                .TaskGroupKey = oBaseUserGroupTaskGroup.TaskGroupKey
                                .DisplaySequence = oBaseUserGroupTaskGroup.DisplaySequence
                                .IsIncluded = oBaseUserGroupTaskGroup.IsIncluded
                            End With
                            oTaskGroups.Add(oTaskGroup)
                        Next
                    End If
                    oUserGroupTaskGroupDetails.TaskGroup = oTaskGroups
                    oUserGroupTaskGroupDetails.TimeStamp = .TimeStamp
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetUserGroupTaskGroups executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If IsNothing(v_dtEffectiveDate) Then
                        sbLogMessage.AppendLine("Effective Date : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Effective Date : " & v_dtEffectiveDate.ToString() & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output : " & vbCrLf)
                    sbLogMessage.AppendLine(oUserGroupTaskGroupDetails.Print().Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetUserGroupTaskGroupsRequest = Nothing
                oGetUserGroupTaskGroupsResponse = Nothing
            End Try


            Return oUserGroupTaskGroupDetails

        End SyncLock
    End Function

    Public Overrides Function GetUserGroupUsers(ByVal v_sUserGroupCode As String,
                    ByVal v_dtEffectiveDate As Date, ByVal v_bRestrictUserList As Boolean,
                    ByVal v_bRestrictUserListSpecified As Boolean,
                    Optional ByVal v_sBranchCode As String = Nothing) As UserCollection
        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oGetUserGroupUsersRequest As GetUserGroupUsersRequestType
            Dim oGetUserGroupUsersResponse As GetUserGroupUsersResponseType
            Dim oUserGroupUsers As UserCollection = Nothing
            Dim oUserGroup As User = Nothing
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetUserGroupUsersRequest = New GetUserGroupUsersRequestType
                oGetUserGroupUsersResponse = New GetUserGroupUsersResponseType
                sbLogMessage = New StringBuilder


                With oGetUserGroupUsersRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If
                    .UserGroupCode = v_sUserGroupCode
                    If v_dtEffectiveDate = DateTime.MinValue Then
                        Throw New ArgumentException("UserGroupTaskGroups.EffectiveDate")
                    Else
                        .EffectiveDate = v_dtEffectiveDate
                    End If
                    .RestrictUserList = v_bRestrictUserList
                    .RestrictUserListSpecified = v_bRestrictUserListSpecified
                End With


                Using trace As New Tracer(Category.Trace)
                    oGetUserGroupUsersResponse = oSAM.GetUserGroupUsers(oGetUserGroupUsersRequest)
                End Using

                With oGetUserGroupUsersResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If

                    If .UserGroupUsers IsNot Nothing AndAlso .UserGroupUsers.Count > 0 Then


                        oUserGroupUsers = New UserCollection()
                        For Each oBaseUserGroupUser As BaseGetUserGroupUsersResponseTypeRow In .UserGroupUsers
                            oUserGroup = New User()
                            With oUserGroup

                                .UserId = oBaseUserGroupUser.UserKey
                                .UserName = oBaseUserGroupUser.Name
                                .EmailAddress = oBaseUserGroupUser.EmailAddress
                            End With
                            oUserGroupUsers.Add(oUserGroup)
                        Next
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetUserGroupUsers executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If IsNothing(v_sUserGroupCode) Then
                        sbLogMessage.AppendLine("UserGroup Code : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("UserGroup Code : " & v_sUserGroupCode & vbCrLf)
                    End If

                    If IsNothing(v_dtEffectiveDate) Then
                        sbLogMessage.AppendLine("Effective Date : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Effective Date : " & v_dtEffectiveDate.ToString() & vbCrLf)
                    End If

                    If IsNothing(v_bRestrictUserList) Then
                        sbLogMessage.AppendLine("RestrictUser List : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("RestrictUser List : " & v_bRestrictUserList.ToString() & vbCrLf)
                    End If

                    If IsNothing(v_bRestrictUserListSpecified) Then
                        sbLogMessage.AppendLine("RestrictUser List Specified : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("RestrictUser List Specified : " & v_bRestrictUserListSpecified.ToString() & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output : " & vbCrLf)
                    sbLogMessage.AppendLine(oUserGroupUsers.Print().Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetUserGroupUsersRequest = Nothing
                oGetUserGroupUsersResponse = Nothing
            End Try


            Return oUserGroupUsers
        End SyncLock
    End Function

    Public Overrides Function GetUserGroups(Optional ByVal v_sBranchCode As String = Nothing) As UserGroupCollection
        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oGetUserGroupsRequest As GetUserGroupsRequestType
            Dim oGetUserGroupsResponse As GetUserGroupsResponseType
            Dim oUserGroups As UserGroupCollection = Nothing
            Dim oUserGroup As UserGroup = Nothing
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetUserGroupsRequest = New GetUserGroupsRequestType
                oGetUserGroupsResponse = New GetUserGroupsResponseType
                sbLogMessage = New StringBuilder


                With oGetUserGroupsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode

                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                End With


                Using trace As New Tracer(Category.Trace)
                    oGetUserGroupsResponse = oSAM.GetUserGroups(oGetUserGroupsRequest)
                End Using

                With oGetUserGroupsResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If

                    If .UserGroups IsNot Nothing AndAlso .UserGroups.Count > 0 Then


                        oUserGroups = New UserGroupCollection()
                        For Each oBaseUserGroup As BaseGetUserGroupsResponseTypeRow In .UserGroups
                            oUserGroup = New UserGroup()
                            With oUserGroup
                                .Code = oBaseUserGroup.Code.Trim
                                .Description = oBaseUserGroup.Description.Trim
                                .EffectiveDate = oBaseUserGroup.EffectiveDate
                                .IsDeleted = oBaseUserGroup.IsDeleted
                                .IsSysAdmin = oBaseUserGroup.IsSystemAdmin
                                .UserGroupKey = oBaseUserGroup.UserGroupKey
                            End With
                            oUserGroups.Add(oUserGroup)
                        Next
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetUserGroups executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output : " & vbCrLf)
                    sbLogMessage.AppendLine(oUserGroups.Print().Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetUserGroupsRequest = Nothing
                oGetUserGroupsResponse = Nothing
            End Try


            Return oUserGroups
        End SyncLock
    End Function

    Public Overrides Function GetUserGroupsbyTask(ByVal v_sTaskGroupCode As String,
                Optional ByVal v_sBranchCode As String = Nothing) As UserGroupCollection
        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oGetUserGroupsbyTaskRequest As GetUserGroupsbyTaskRequestType
            Dim oGetUserGroupsbyTaskResponse As GetUserGroupsbyTaskResponseType
            Dim oUserGroupsbyTask As UserGroupCollection = Nothing
            Dim oUserGroupbyTask As UserGroup = Nothing
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetUserGroupsbyTaskRequest = New GetUserGroupsbyTaskRequestType
                oGetUserGroupsbyTaskResponse = New GetUserGroupsbyTaskResponseType
                sbLogMessage = New StringBuilder


                With oGetUserGroupsbyTaskRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If

                    .TaskGroupCode = v_sTaskGroupCode
                End With


                Using trace As New Tracer(Category.Trace)
                    oGetUserGroupsbyTaskResponse = oSAM.GetUserGroupsbyTask(oGetUserGroupsbyTaskRequest)
                End Using

                With oGetUserGroupsbyTaskResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If

                    If .UserGroups IsNot Nothing AndAlso .UserGroups.Count > 0 Then


                        oUserGroupsbyTask = New UserGroupCollection()
                        For Each oBaseUserGroupbyTask As BaseGetUserGroupsbyTaskResponseTypeRow In .UserGroups
                            oUserGroupbyTask = New UserGroup()
                            With oUserGroupbyTask
                                .Code = oBaseUserGroupbyTask.UserGroupCode.Trim
                                .Description = oBaseUserGroupbyTask.UserGroupDescription.Trim
                                .UserGroupKey = oBaseUserGroupbyTask.UserGroupKey
                            End With
                            oUserGroupsbyTask.Add(oUserGroupbyTask)
                        Next
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetUserGroups executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If IsNothing(v_sTaskGroupCode) Then
                        sbLogMessage.AppendLine("TaskGroup Code : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("TaskGroup Code : " & v_sTaskGroupCode & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output : " & vbCrLf)
                    sbLogMessage.AppendLine(oUserGroupsbyTask.Print().Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetUserGroupsbyTaskRequest = Nothing
                oGetUserGroupsbyTaskResponse = Nothing
            End Try


            Return oUserGroupsbyTask
        End SyncLock
    End Function

    Public Overrides Sub UpdateSubAgents(ByRef v_oQuote As NexusProvider.Quote,
                                              ByVal v_oSubAgents As NexusProvider.SubAgentCollection,
                                              Optional ByVal v_sBranchCode As String = Nothing)

        Dim oSAM As PureServiceClient 'SAMForInsuranceV2's Object
        Dim oUpdateSubAgentsRequest As UpdateSubAgentsRequestType    ' Request Type
        Dim oUpdateSubAgentsResponse As UpdateSubAgentsResponseType   ' Response Type
        Dim v_oSubAgent As BaseUpdateSubAgentsRequestTypeSubAgentsRow
        Dim iCt As Integer
        Dim sbLogMessage As StringBuilder

        Try
            oSAM = InitializeServiceMethod()
            oUpdateSubAgentsRequest = New UpdateSubAgentsRequestType
            oUpdateSubAgentsResponse = New UpdateSubAgentsResponseType
            sbLogMessage = New StringBuilder


            With oUpdateSubAgentsRequest
                .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                'if the passed parameter v_sBranchCode is empty 
                If String.IsNullOrEmpty(v_sBranchCode) Then
                    'if the branch code is NOT in session 
                    If String.IsNullOrEmpty(sBranchCode) Then
                        'Use the default branch code
                        .BranchCode = sDefaultBranchCode
                    Else
                        'Use the branch code in session 
                        .BranchCode = sBranchCode
                    End If
                Else
                    'use the passed parameter v_sBranchCode
                    .BranchCode = v_sBranchCode

                End If
                'WorkManager Request
                If v_oQuote.InsuranceFileKey = 0 Then
                    Throw New ArgumentNullException("WorkManager")
                Else
                    .TimeStamp = v_oQuote.TimeStamp
                    .InsuranceFileKey = v_oQuote.InsuranceFileKey

                    If v_oSubAgents.Count > 0 Then
                        .SubAgents = New List(Of BaseUpdateSubAgentsRequestTypeSubAgentsRow)
                        For iCt = 0 To v_oSubAgents.Count - 1
                            v_oSubAgent = New BaseUpdateSubAgentsRequestTypeSubAgentsRow
                            v_oSubAgent.PartyKey = v_oSubAgents(iCt).PartyKey
                            v_oSubAgent.Amount = v_oSubAgents(iCt).Amount
                            v_oSubAgent.Percentage = v_oSubAgents(iCt).Percentage
                            .SubAgents.Add(v_oSubAgent)
                        Next
                    End If

                End If
            End With


            'Calling the SAM Method with Request Type
            'add trace to allow us to debug slow SAM calls
            Using trace As New Tracer(Category.Trace)
                oUpdateSubAgentsResponse = oSAM.UpdateSubAgents(oUpdateSubAgentsRequest)
            End Using

            'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

            ' Disposing the SAM's object


            With oUpdateSubAgentsResponse  'With Response Type
                If .Errors IsNot Nothing Then
                    'Process the error object if errors, and throw as a single exception
                    Throw New NexusException(.Errors)
                Else
                    'WorkManager Response
                    'Fetching from the  SubAgents Response Collection 
                    v_oQuote.TimeStamp = .TimeStamp
                End If
            End With

            If Logger.IsLoggingEnabled Then
                sbLogMessage.AppendLine("Work Manager UpdateSubAgent executed ok" & vbCrLf)
                sbLogMessage.AppendLine("Input:" & v_oQuote.Print() & vbCrLf)
                If Not IsNothing(v_sBranchCode) Then
                    sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                Else
                    sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                End If

                sbLogMessage.AppendLine("Returned " & v_oQuote.Print() & "results" & vbCrLf)

                LogMessageEntry(sbLogMessage)
            End If

            'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

            'FaultErrorHandler(ex) ' handling fault error messages 

        Catch ex As Exception
            Throw
        Finally
            oSAM.Close()
            oUpdateSubAgentsRequest = Nothing
            oUpdateSubAgentsResponse = Nothing
        End Try

    End Sub

    Public Overrides Function UpdateUsersGroupUsers(ByVal v_oWorkManager As WorkManager,
                                                Optional ByVal v_sBranchCode As String = Nothing) As WorkManager

        Dim oSAM As PureServiceClient 'SAMForInsuranceV2's Object
        Dim oUpdateUserGroupUsersRequest As UpdateUserGroupUsersRequestType  ' Request Type
        Dim oUpdateUserGroupUsersResponse As UpdateUserGroupUsersResponseType  ' Response Type
        Dim v_oNewWorkManager As WorkManager  'Object of WorkManager Class
        Dim sbLogMessage As StringBuilder

        Try
            oSAM = InitializeServiceMethod()
            oUpdateUserGroupUsersRequest = New UpdateUserGroupUsersRequestType
            oUpdateUserGroupUsersResponse = New UpdateUserGroupUsersResponseType
            sbLogMessage = New StringBuilder


            With oUpdateUserGroupUsersRequest
                .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                'if the passed parameter v_sBranchCode is empty 
                If String.IsNullOrEmpty(v_sBranchCode) Then
                    'if the branch code is NOT in session 
                    If String.IsNullOrEmpty(sBranchCode) Then
                        'Use the default branch code
                        .BranchCode = sDefaultBranchCode
                    Else
                        'Use the branch code in session 
                        .BranchCode = sBranchCode

                    End If
                Else
                    'use the passed parameter v_sBranchCode
                    .BranchCode = v_sBranchCode
                End If
                'WorkManager Request
                If v_oWorkManager.TaskInstanceKey = String.Empty Then
                    Throw New ArgumentNullException("WorkManager.TaskInstanceKey")
                Else
                    .TimeStamp = v_oWorkManager.TimeStamp
                    .UserGroupKey = v_oWorkManager.UserGroupKey
                    .Users = New List(Of BaseUpdateUserGroupUsersRequestTypeUsersRow)
                    For Each v_oUser As BaseUpdateUserGroupUsersRequestTypeUsersRow In .Users ' BaseUpdateUserGroupUsersRequestTypeRow In .Users

                        v_oUser.DisplaySequence = v_oWorkManager.DisplaySequence
                        v_oUser.DisplaySequenceSpecified = v_oWorkManager.DisplaySequenceSpecified
                        v_oUser.IsSupervisor = v_oWorkManager.IsSupervisor
                        v_oUser.UserKey = v_oWorkManager.UserKey
                    Next

                End If
            End With

            'Calling the SAM Method with Request Type
            'add trace to allow us to debug slow SAM calls
            Using trace As New Tracer(Category.Trace)
                oUpdateUserGroupUsersResponse = oSAM.UpdateUserGroupUsers(oUpdateUserGroupUsersRequest)
            End Using

            'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

            ' Disposing the SAM's object


            With oUpdateUserGroupUsersResponse  'With Response Type
                If .Errors IsNot Nothing Then
                    'Process the error object if errors, and throw as a single exception
                    Throw New NexusException(.Errors)
                Else

                    'WorkManager Response
                    'Fetching from the  WorkManager Response Collection
                    v_oNewWorkManager = New NexusProvider.WorkManager
                    v_oNewWorkManager.TimeStamp = oUpdateUserGroupUsersResponse.TimeStamp
                End If
            End With

            If Logger.IsLoggingEnabled Then
                sbLogMessage.AppendLine("WorkManager executed ok" & vbCrLf)
                sbLogMessage.AppendLine("Input:" & v_oWorkManager.Print() & vbCrLf)
                If Not IsNothing(v_sBranchCode) Then
                    sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                Else
                    sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                End If

                sbLogMessage.AppendLine("Returned " & v_oNewWorkManager.Print() & " results" & vbCrLf)

                LogMessageEntry(sbLogMessage)
            End If

            'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

            'FaultErrorHandler(ex) ' handling fault error messages 

        Catch ex As Exception
            Throw
        Finally
            oSAM.Close()
            oUpdateUserGroupUsersRequest = Nothing
            oUpdateUserGroupUsersResponse = Nothing
        End Try


        Return v_oNewWorkManager  'Returning WorkManager Collection

    End Function
End Class
