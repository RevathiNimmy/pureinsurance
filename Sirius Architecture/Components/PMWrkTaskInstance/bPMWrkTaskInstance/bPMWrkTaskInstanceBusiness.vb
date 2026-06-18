Option Strict Off
Option Explicit On
Imports System.Text
Imports SSP.Shared
Friend NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 30/10/1998
    '
    ' Description:
    '
    '
    ' Edit History:
    ' DAK121099 - Changes for Product Licencing and linked objects
    ' DAK141299 - Add is_visible column to task instance
    ' DAK231299 - Replace Task Group Category with Task Category
    ' DAK240100 - correct the array processing in CreateNew
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' ************************************************
    ' Added to replace global variables 18/09/2003
    ' Username.
    Private m_sUsername As String = ""
    ' Password.
    Private m_sPassword As String = ""
    ' User ID
    Private m_iUserID As Integer
    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************

    ' Task Instance
    Private m_oTaskInstance As bPMWrkTaskInstance.PMWrkTaskInstance
    Private m_bRestrictedTaskView As Boolean
    Private m_oPMLock As bpmlock.User
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture
        End Get
    End Property
    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        ' RDC 13062002 CompServ replaced witrh bAS module
        'Dim oCS As sPMServerCS.PMServerBusinessCS
        Dim result As Integer = 0
        Dim bMultiTreeAcc, bRestrictedTaskView As Boolean
        Dim vValue As Byte

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            ' Check the Supplied Database.

            m_lReturn = gPMComponentServices.CheckDatabase(v_sUsername:=m_sUsername, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            '    Set oCS = Nothing

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oTaskInstance = New bPMWrkTaskInstance.PMWrkTaskInstance()
            m_lReturn = m_oTaskInstance.Initialise(vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' SET 18/04/2007
            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, v_vBranch:=m_iSourceID, r_vUnderwriting:=CStr(vValue))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            End If

            If vValue = 1 Then
                bMultiTreeAcc = True
            End If

            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiCoWorkManagerTaskRestriction, v_vBranch:=m_iSourceID, r_vUnderwriting:=CStr(vValue))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue (Restricted Client View) Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            If vValue = 1 Then
                bRestrictedTaskView = True
            End If

            If bRestrictedTaskView And bMultiTreeAcc Then
                m_bRestrictedTaskView = True
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If m_oTaskInstance IsNot Nothing Then
                    m_oTaskInstance.Dispose()
                    m_oTaskInstance = Nothing
                End If
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ''' <summary>
    ''' Function designed to convert XML to Safe XML
    ''' </summary>
    ''' <param name="sInXMLString"></param>
    ''' <param name="sOutXMLString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ToSafeXMLString(ByRef sInXMLString As String, ByRef sOutXMLString As String) As Integer

        Dim Entities As New Generic.Dictionary(Of Char, String)
        Entities.Add("&"c, "&amp;")
        Entities.Add("'"c, "&apos;")


        Dim sOutString As New StringBuilder
        Dim cSelected As Char
        If sInXMLString.Trim Is Nothing OrElse sInXMLString = String.Empty Then Return String.Empty

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue


        Try

            For iCount As Integer = 0 To sInXMLString.Length - 1
                cSelected = sInXMLString(iCount)
                If Entities.ContainsKey(cSelected) Then
                    sOutString.Append(Entities.Item(cSelected))
                ElseIf (AscW(cSelected) = &H9 OrElse AscW(cSelected) = &HA OrElse AscW(cSelected) = &HD) _
                    OrElse ((AscW(cSelected) >= &H20) AndAlso (AscW(cSelected) <= &HD7FF)) _
                    OrElse ((AscW(cSelected) >= &HE000) AndAlso (AscW(cSelected) <= &HFFFD)) _
                    OrElse ((AscW(cSelected) >= &H10000) AndAlso (AscW(cSelected) <= &H10FFFF)) Then

                    sOutString.Append(cSelected)
                End If
            Next

            sOutXMLString = sOutString.ToString

            Return nResult
        Catch ex As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="ToSafeXMLString",
                               vApp:=ACApp, vClass:=ACClass,
                               vMethod:="ToSafeXMLString",
                               vErrNo:=Informations.Err().Number,
                               vErrDesc:=ex.Message)
            Return nResult

        End Try

    End Function

    ' ***************************************************************** '
    ' Name: CreateNew
    '
    ' Description: Creates a New Task Instance.
    '
    ' 1 - Add the Task Instance.
    ' 2 - Add any Keys supplied.
    '
    ' AMB 20/01/2003 - workflow_information column added
    ' ***************************************************************** '
    'DAK141299
    'developer guide no. 17
    Public Function CreateNew(ByVal v_lPMWrkTaskID As Integer,
                              ByVal v_lPMWrkTaskGroupID As Integer,
                              ByVal v_sCustomer As String,
                              ByVal v_dtTaskDueDate As Date,
                              ByVal v_lPMUserGroupID As Integer,
                              ByVal v_sDescription As String,
                              ByVal v_iTaskStatus As Integer,
                              ByVal v_iIsUrgent As Integer,
                              ByRef r_lPMWrkTaskInstanceCnt As Integer,
                              Optional ByVal v_sWorkflowInformation As String = "",
                              Optional ByVal v_dtDateCreated As Date = #12/30/1899#,
                              Optional ByVal v_iCreatedByID As Integer = 0,
                              Optional ByVal v_iUserID As Integer = 0,
                              Optional ByVal v_vKeyArray(,) As Object = Nothing,
                              Optional ByVal v_iIsVisible As Integer = gPMConstants.PMEReturnCode.PMTrue,
                              Optional ByVal v_iIsTaskReview As Integer = 0,
                              Optional ByVal bIsExternalWorkItem As Boolean = False,
                              Optional ByRef r_sGuidPMExternalItem As String = "",
                              Optional ByVal nParentTaskId As Integer = 0,
                              Optional ByVal sExternalTaskCategoryCode As String = "",
                              Optional ByVal sLockKeyName As LockName = LockName.InvalidValue,
                              Optional ByVal nLockKeyValue As Integer = 0,
                              Optional ByVal bViaBackgrounJobProcess As Boolean = False,
                              Optional ByVal nExternalTaskStatus As Integer = -1,
                              Optional ByVal bIsExternalChildTask As Boolean = False) As Integer



        Dim nResult As Integer = 0
        Dim sKeyName As String = String.Empty
        Dim sKeyValue As String = String.Empty
        Dim oLock As New bpmlock.User
        Dim bExternalWorkItem As Boolean = bIsExternalWorkItem
        Dim bScheduleInBackgroundJob As Boolean = False
        Dim sCurrentlyLockedBy As String = String.Empty
        Dim sLockKeyNameString As String = LockNameString(sLockKeyName)
        Dim uGuidPMExternalItem As System.Guid = Guid.Empty
        Dim uGuidPMExternalParentItem As System.Guid = Guid.Empty
        Dim bLock As Boolean = False

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue


            'We are trying to create a child task through background job
            'Find the parent task and its external guid
            If bIsExternalChildTask = True AndAlso nParentTaskId = 0 AndAlso bViaBackgrounJobProcess = True Then
                If Not Informations.IsNothing(v_vKeyArray) Then
                    If Informations.IsArray(v_vKeyArray) Then
                        'This KeyName/KeyValue pair should be common between Master and Child task
                        'This common KeyName/KeyValue should be first one

                        Dim oTaskArray(,) As Object = Nothing

                        sKeyName = v_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0)
                        sKeyValue = v_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0)

                        m_oDatabase.Parameters.Clear()
                        If m_oDatabase.Parameters.Add(sName:="pmwrk_task_instance_cnt", vValue:=CStr(-1),
                              iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                              iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If m_oDatabase.Parameters.Add(sName:="key_name", vValue:=sKeyName,
                              iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                              iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If m_oDatabase.Parameters.Add(sName:="key_value", vValue:=sKeyValue,
                              iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                              iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectSingleSQL,
                                                          sSQLName:=ACSelectSingleName,
                                                          bStoredProcedure:=ACSelectSingleStored,
                                                          vResultArray:=oTaskArray,
                                                          bKeepNulls:=True)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If oTaskArray IsNot Nothing Then
                            For nRow As Integer = oTaskArray.GetLowerBound(1) To oTaskArray.GetUpperBound(1)
                                'Find the Parent Task
                                If oTaskArray(18, nRow) IsNot Nothing AndAlso oTaskArray(18, nRow) = 1 AndAlso
                                    oTaskArray(19, nRow) IsNot Nothing AndAlso
                                    oTaskArray(20, nRow) Is Nothing Then

                                    r_sGuidPMExternalItem = oTaskArray(19, nRow)
                                    nParentTaskId = oTaskArray(nRow, 0)
                                    Exit For
                                End If
                            Next
                        End If
                    End If
                End If
            End If


            'r_sGuidPMExternalItem : if we get a value and nParentTaskId <> 0, treat it as parent guid.
            'r_sGuidPMExternalItem : Create a master or child task and return the newly created guid in r_sGuidPMExternalItem 
            If nParentTaskId <> 0 AndAlso r_sGuidPMExternalItem IsNot Nothing AndAlso r_sGuidPMExternalItem <> String.Empty Then
                uGuidPMExternalParentItem = New Guid(r_sGuidPMExternalItem)
            End If



            'Check External WorkFlow Configuration is switched on or off.
            Dim sValue As Object = String.Empty
            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername,
                                          v_sPassword:=m_sPassword,
                                          v_iUserID:=m_iUserID,
                                          v_iMainSourceID:=m_iSourceID,
                                          v_iLanguageID:=m_iLanguageID,
                                          v_iCurrencyID:=m_iCurrencyID,
                                          v_iLogLevel:=m_iLogLevel,
                                          v_sCallingAppName:=m_sCallingAppName,
                                          v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIRROPTEnableExternalWorkflowSystem,
                                          v_vBranch:=m_iSourceID, r_vUnderwriting:=sValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername,
                                   iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="getProductOptionValue Failed",
                                   vApp:=ACApp,
                                   vClass:=ACClass,
                                   vMethod:="CreateNew",
                                   vErrNo:=Informations.Err().Number,
                                   vErrDesc:=Informations.Err().Description)
            End If

            'If system option is not on then bIsExternalWorkItem = False
            If gPMFunctions.NullToString(sValue) = "0" OrElse gPMFunctions.NullToString(sValue) = "" Then
                bExternalWorkItem = False

            ElseIf gPMFunctions.NullToString(sValue) = "1" Then
                'If Option is on but bIsExternalWorkItem is False
                'check the user group configured through ExternalWorkFlow task
                Dim oUserGroup(,) As Object = Nothing
                m_oDatabase.Parameters.Clear()
                If m_oDatabase.Parameters.Add(sName:="dtEffective_date", vValue:=Date.Today,
                                              iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                              iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If m_oDatabase.Parameters.Add(sName:="nSelected_UserGroup_id", vValue:=v_lPMUserGroupID,
                                              iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                              iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=KSelExternalWorkFlowConfiguration_UsergroupsSQL,
                                                  sSQLName:=KSelExternalWorkFlowConfiguration_UsergroupName,
                                                  bStoredProcedure:=KSelExternalWorkFlowConfiguration_UsergroupStored,
                                                  vResultArray:=oUserGroup,
                                                  bKeepNulls:=True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'We have found the record for the user group, so set bIsExternalWorkItem = True
                If oUserGroup IsNot Nothing Then
                    bExternalWorkItem = True
                    If oUserGroup(5, 0) IsNot Nothing AndAlso oUserGroup(5, 0) = 1 Then
                        bScheduleInBackgroundJob = True
                        bLock = True
                    End If
                End If

                If sExternalTaskCategoryCode = "" AndAlso nExternalTaskStatus = -1 Then
                    bExternalWorkItem = False
                    bScheduleInBackgroundJob = False
                    bLock = False
                End If

            End If



            If bExternalWorkItem = True Then

                'Intialise bPMLock Object, when we have correct values to lock










                oLock = New bpmlock.User
                m_lReturn = oLock.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                       sMsg:="Failed to get instance of bPMLock.User", vApp:=ACApp,
                                       vClass:=ACClass, vMethod:="CreateNew",
                                       vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If
                m_oDatabase.SQLBeginTrans()
            End If

            ' Create a New Task Instance Class
            m_lReturn = NewTaskInstance()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the Defaults
            m_oTaskInstance.Default_Renamed()

            ' Set the properties
            With m_oTaskInstance
                .PmwrkTaskGroupID = v_lPMWrkTaskGroupID
                .PmwrkTaskID = v_lPMWrkTaskID
                .Customer = v_sCustomer
                .TaskDueDate = v_dtTaskDueDate
                .PmuserGroupID = v_lPMUserGroupID
                .Description = v_sDescription
                .TaskStatus = v_iTaskStatus
                .IsUrgent = v_iIsUrgent
                If v_iCreatedByID > 0 Then
                    .CreatedByID = v_iCreatedByID
                    .DateCreated = v_dtDateCreated
                End If
                If v_iUserID > 0 Then
                    .UserID = v_iUserID
                End If
                'DAK141299
                .IsVisible = v_iIsVisible
                ' AMB 20/01/2003
                .WorkflowInformation = v_sWorkflowInformation
                .SourceID = m_iSourceID
                .IsTaskReview = v_iIsTaskReview
                .IsExternalWorkItem = bExternalWorkItem
                If nParentTaskId = 0 Then
                    .GuidPMExternalItem = Guid.Empty 'Verify GUID as its in string and in DB its in uniqueidentifier
                Else
                    .GuidPMExternalItem = uGuidPMExternalItem
                End If

                .ParentTaskId = nParentTaskId
            End With

            m_lReturn = m_oTaskInstance.Add()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Key of the row added.
            r_lPMWrkTaskInstanceCnt = m_oTaskInstance.PMWrkTaskInstanceCnt

            'Do not change sInputXML strings default value
            Dim sInputXML As String = "<start>"
            Dim sSafeXML As String = String.Empty
            Dim oFillteredKeyValueArray(,) As Object = Nothing

            ' Add the Task Instance Keys, if there are any

            If Not Informations.IsNothing(v_vKeyArray) Then
                If Informations.IsArray(v_vKeyArray) Then

                    'DAK240100
                    For lRow As Integer = v_vKeyArray.GetLowerBound(1) To v_vKeyArray.GetUpperBound(1)
                        'PN 44217

                        If Not Informations.IsArray(v_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) And Not Object.Equals(v_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow), Nothing) Then

                            sKeyName = v_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)

                            sKeyValue = v_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)

                            m_lReturn = AddTaskInstKey(v_lPMWrkTaskInstanceCnt:=r_lPMWrkTaskInstanceCnt, v_sKeyName:=sKeyName, v_sKeyValue:=sKeyValue)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                        End If

                        If bExternalWorkItem Then
                            'Build the sInputXML string in the following format
                            sInputXML = sInputXML + "<" + Trim(sKeyName) + ">" 'Add the KeyName opening tag
                            sInputXML = sInputXML + Trim(gPMFunctions.ToSafeString(sKeyValue)) 'Set the KeyName value
                            sInputXML = sInputXML + "</" + Trim(gPMFunctions.ToSafeString(sKeyName)) + ">" 'Add the KeyName closing tag
                        End If
                    Next lRow

                    'Add the clsoing tag of the sInputXML and call the SP to get the filtered arrays
                    If bExternalWorkItem Then
                        sInputXML = sInputXML + "</start>"
                        If sInputXML <> "<start><></></start>" Then

                            If ToSafeXMLString(sInputXML, sSafeXML) <> gPMConstants.PMEReturnCode.PMTrue Then
                                Throw New Exception("bPMWrkTaskInstance.ToSafeXMLString Failed")
                            End If

                            m_oDatabase.Parameters.Clear()
                            If m_oDatabase.Parameters.Add(sName:="InputXML", vValue:=sSafeXML,
                                                          iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                          iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                                'Raise exception , so we can rollback the transaction and schedule it background job
                                Throw New Exception("Failed to add input parameter to spu_sir_get_key_settings")
                            End If

                            m_lReturn = m_oDatabase.SQLSelect(sSQL:=KFilteredKeyArrayStoredSQL,
                                                              sSQLName:=KFilteredKeyArrayStoredName,
                                                              bStoredProcedure:=KFilteredKeyArrayStored,
                                                              vResultArray:=oFillteredKeyValueArray,
                                                              bKeepNulls:=True)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                'Raise exception , so we can rollback the transaction and schedule it background job
                                Throw New Exception("Call to spu_sir_get_key_settings failed.")
                            End If
                        End If
                    End If
                End If
            End If

            Dim oExternalWorkItem As Object = Nothing
            Dim bTimestampMatches As Boolean = False



            If bExternalWorkItem Then
                'Create the object 
                oExternalWorkItem = New bExternalWorkItem.e5IntegrationProxyLayer.Business

                'Set some intial values
                oExternalWorkItem.Initialise(ToSafeString(m_sUsername), ToSafeString(m_sPassword), ToSafeInteger(m_iUserID), ToSafeInteger(m_iSourceID),
                                             ToSafeInteger(m_iLanguageID), ToSafeInteger(m_iCurrencyID), ToSafeInteger(m_iLogLevel), ToSafeInteger(m_sCallingAppName))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Raise exception , so we can rollback the transaction and schedule it background job
                    Throw New Exception("bExternalWorkItem.Initialise Failed")
                End If


                Dim oExternalWorkFlowId As System.Guid
                Dim _uGuidPMExternalItem As Object = uGuidPMExternalItem
                'Make a call to CreateExternalWorkItem, this will do all the processing.
                m_lReturn = oExternalWorkItem.CreateExternalWorkItem(sExternalCategoryCode:=ToSafeString(sExternalTaskCategoryCode),
                                                                     oKeyArray:=CType(oFillteredKeyValueArray, Object),
                                                                     r_uExternalWorkId:=_uGuidPMExternalItem,
                                                                     sGuidParentWorkId:=uGuidPMExternalParentItem.ToString(),
                                                                     nExternalTaskStatus:=ToSafeInteger(nExternalTaskStatus))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Raise exception , so we can rollback the transaction and schedule it background job
                    Throw New Exception("bExternalWorkItem.CreateExternalWorkItem Failed")
                End If
                uGuidPMExternalItem = _uGuidPMExternalItem

                'Every thing is successfull , update the relevant values as rest of them already set when we called m_oTaskInstance.Add()
                With m_oTaskInstance
                    .PMWrkTaskInstanceCnt = r_lPMWrkTaskInstanceCnt
                    .GuidPMExternalItem = uGuidPMExternalItem
                End With

                m_lReturn = m_oTaskInstance.Update()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    'In case update fails, log the GUID and PMWrkTaskInstanceCnt in the error log
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                       sMsg:="Failed to updated GUID PMExternalItem : " + oExternalWorkFlowId.ToString() + " for PMWrkTaskInstanceCnt : " + r_lPMWrkTaskInstanceCnt,
                                       vApp:=ACApp,
                                       vClass:=ACClass, vMethod:="CreateNew",
                                       vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                r_sGuidPMExternalItem = uGuidPMExternalItem.ToString()

            End If

            If bExternalWorkItem Then
                'Unlock when we have the values
                If sLockKeyNameString <> String.Empty AndAlso
                nLockKeyValue <> 0 AndAlso
                bViaBackgrounJobProcess = True AndAlso
                bLock = True Then
                    m_lReturn = oLock.UnLockKey(sKeyName:=sLockKeyNameString, vKeyValue:=nLockKeyValue, bDeleteSystemLock:=True)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'nResult = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                           sMsg:="Unable to unlock using " + sLockKeyNameString + " and lock value " + nLockKeyValue.ToString(), vApp:=ACApp,
                                           vClass:=ACClass, vMethod:="CreateNew",
                                           vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        'Return nResult
                    End If
                ElseIf bViaBackgrounJobProcess = True AndAlso
                bLock = True Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                       sMsg:="System lock has not been released since lock keys are missing for PMWrkTaskInstanceCnt : " + r_lPMWrkTaskInstanceCnt, vApp:=ACApp,
                                       vClass:=ACClass, vMethod:="CreateNew",
                                       vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                End If

                oLock = Nothing
                oExternalWorkItem = Nothing
                m_oDatabase.SQLCommitTrans()
            End If

            Return nResult

        Catch ex As System.Exception
            If bExternalWorkItem Then
                m_oDatabase.SQLRollbackTrans()

                'How do we identify if its related to claims or not ?????


                Dim sErrorMsg As String = ""
                'Lock, If values supplied else write to log file, only for claims
                'Lock if bScheduleInBackgroundJob = True, 
                'Lock bViaBackgrounJobProcess = False, if we are coming bViaBackgrounJobProcess do not attempt to lock second time
                'bLock =  True, lock only for master claim
                If sLockKeyNameString <> String.Empty AndAlso
                nLockKeyValue <> 0 AndAlso
                bScheduleInBackgroundJob = True AndAlso
                bLock = True AndAlso
                bViaBackgrounJobProcess = False Then


                    'Do not lock if  we are coming through BackGroundJobProcess as its already locked
                    m_lReturn = oLock.LockKey(sKeyName:=sLockKeyNameString,
                                              vKeyValue:=nLockKeyValue,
                                              iUserID:=m_iUserID,
                                              sCurrentlyLockedBy:=sCurrentlyLockedBy, v_bOtherUserOnly:=True, vKey2Value:=0,
                                              IsSystemLock:=True)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        If sCurrentlyLockedBy <> String.Empty Then
                            sErrorMsg = " ,as its currently locked by " + sCurrentlyLockedBy
                        End If

                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to lock using " + sLockKeyNameString + " and lock value " + nLockKeyValue.ToString() + sErrorMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="CreateNew", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    End If


                ElseIf bScheduleInBackgroundJob = True AndAlso
                bLock = True AndAlso
                bViaBackgrounJobProcess = False Then
                    'No point generating msg if bScheduleInBackgroundJob = False and bLock =  False , as we only lock for Master Claim
                    'bViaBackgrounJobProcess = False, do not generate msg a second time
                    'It would have been raised the very first time.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="System lock has not been generated since lock keys are missing." + Microsoft.VisualBasic.vbCrLf + "v_lPMWrkTaskGroupID : " + v_lPMWrkTaskGroupID.ToString() + Microsoft.VisualBasic.vbCrLf + "v_lPMWrkTaskID : " + v_lPMWrkTaskID.ToString() + Microsoft.VisualBasic.vbCrLf + "v_sCustomer : " + v_sCustomer + Microsoft.VisualBasic.vbCrLf + "v_dtTaskDueDate : " + v_dtTaskDueDate.ToString() + Microsoft.VisualBasic.vbCrLf + "v_lPMUserGroupID : " + v_lPMUserGroupID.ToString() + Microsoft.VisualBasic.vbCrLf + "v_sDescription : " + v_sDescription.ToString() + Microsoft.VisualBasic.vbCrLf + "v_iTaskStatus : " + v_iTaskStatus.ToString() + Microsoft.VisualBasic.vbCrLf + "v_iIsUrgent : " + v_iIsUrgent.ToString() + Microsoft.VisualBasic.vbCrLf + "v_iCreatedByID : " + v_iCreatedByID.ToString() + Microsoft.VisualBasic.vbCrLf + "v_dtDateCreated : " + v_dtDateCreated.ToString() + Microsoft.VisualBasic.vbCrLf + "v_iUserID : " + v_iUserID.ToString() + Microsoft.VisualBasic.vbCrLf + "v_iIsVisible : " + v_iIsVisible.ToString() + Microsoft.VisualBasic.vbCrLf + "v_sWorkflowInformation : " + v_sWorkflowInformation.ToString() + Microsoft.VisualBasic.vbCrLf + "m_iSourceID : " + m_iSourceID.ToString() + Microsoft.VisualBasic.vbCrLf + "v_iIsTaskReview : " + v_iIsTaskReview.ToString() + Microsoft.VisualBasic.vbCrLf + "bExternalWorkItem : " + bExternalWorkItem.ToString() + Microsoft.VisualBasic.vbCrLf + "nParentTaskId : " + nParentTaskId.ToString() + Microsoft.VisualBasic.vbCrLf + "uGuidPMExternalItem : " + uGuidPMExternalItem.ToString(), vApp:=ACApp, vClass:=ACClass, vMethod:="CreateNew", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                End If

                'Schedule it in BackgroundJob if we are not already coming through bViaBackgrounJobProcess
                'No need to do it twice
                If bViaBackgrounJobProcess = False _
                AndAlso bScheduleInBackgroundJob = True Then

                    GenerateXMLAndSchedule(nPMWrkTaskID:=v_lPMWrkTaskID,
                                           nPMWrkTaskGroupID:=v_lPMWrkTaskGroupID,
                                           sCustomer:=v_sCustomer,
                                           dtTaskDueDate:=v_dtTaskDueDate,
                                           nPMUserGroupID:=v_lPMUserGroupID,
                                           sDescription:=v_sDescription,
                                           nTaskStatus:=v_iTaskStatus,
                                           nIsUrgent:=v_iIsUrgent,
                                           sWorkflowInformation:=v_sWorkflowInformation,
                                           dtDateCreated:=v_dtDateCreated,
                                           nCreatedByID:=v_iCreatedByID,
                                           nUserID:=v_iUserID,
                                           oKeyArray:=v_vKeyArray,
                                           nIsVisible:=v_iIsVisible,
                                           nIsTaskReview:=v_iIsTaskReview,
                                           bIsExternalWorkItem:=bIsExternalWorkItem,
                                           nParentTaskId:=nParentTaskId,
                                           sExternalTaskCategoryCode:=sExternalTaskCategoryCode,
                                           sLockKeyName:=sLockKeyName,
                                           nLockKeyValue:=nLockKeyValue,
                                           sActionType:="NEWTASK",
                                           uGuidPMExternalItem:=Guid.Empty,
                                           PMWrkTaskInstanceCnt:=0,
                                           nExternalTaskStatus:=nExternalTaskStatus,
                                           bIsExternalChildTask:=bIsExternalChildTask)

                End If
            End If
            nResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateNewFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateNew", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return nResult
        End Try

    End Function


    Private Function GenerateXMLAndSchedule(ByVal nPMWrkTaskID As Integer,
                                           ByVal nPMWrkTaskGroupID As Integer,
                                           ByVal sCustomer As String,
                                           ByVal dtTaskDueDate As Date,
                                           ByVal nPMUserGroupID As Integer,
                                           ByVal sDescription As String,
                                           ByVal nTaskStatus As Integer,
                                           ByVal nIsUrgent As Integer,
                                           ByVal sWorkflowInformation As String,
                                           ByVal dtDateCreated As Date,
                                           ByVal nCreatedByID As Integer,
                                           ByVal nUserID As Integer,
                                           ByVal oKeyArray(,) As Object,
                                           ByVal nIsVisible As Integer,
                                           ByVal nIsTaskReview As Integer,
                                           ByVal bIsExternalWorkItem As Boolean,
                                           ByVal nParentTaskId As Integer,
                                           ByVal sExternalTaskCategoryCode As String,
                                           ByVal sLockKeyName As LockName,
                                           ByVal nLockKeyValue As Integer,
                                           ByVal sActionType As String,
                                           ByVal uGuidPMExternalItem As System.Guid,
                                           ByVal PMWrkTaskInstanceCnt As Integer,
                                           ByVal nExternalTaskStatus As Integer,
                                           ByVal bIsExternalChildTask As Boolean) As Integer




        Dim nResult As Integer = 0

        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue


            Dim sInputXML As String = String.Empty
            Dim sKeyName As String = String.Empty
            Dim sKeyValue As String = String.Empty

            'All double quotes which are part of the xml must be escapeed using one more double quote , so " becomes "" and "" becomes """"
            sInputXML = "<BACKGROUND_JOB>" + Environment.NewLine 'Start Tag of Background Job
            sInputXML = sInputXML + "<JOB jobtype=""EXWRKITEM"">" + Environment.NewLine ' Start tag of Job 
            sInputXML = sInputXML + "<PARAMETERS>" + Environment.NewLine  ' Start tag of Parameters


            sInputXML = sInputXML + "<PARAMETER name=""WrkTaskGroupId"" value=""" + nPMWrkTaskGroupID.ToString() + """ />" + Environment.NewLine
            sInputXML = sInputXML + "<PARAMETER name=""WrkTaskId"" value=""" + nPMWrkTaskID.ToString() + """ />" + Environment.NewLine
            sInputXML = sInputXML + "<PARAMETER name=""WrkCustomer"" value=""" + sCustomer + """ />" + Environment.NewLine
            sInputXML = sInputXML + "<PARAMETER name=""WrkDueDate"" value=""" + dtTaskDueDate.ToString() + """ />" + Environment.NewLine
            sInputXML = sInputXML + "<PARAMETER name=""WrkUserGroupId"" value=""" + nPMUserGroupID.ToString() + """ />" + Environment.NewLine
            sInputXML = sInputXML + "<PARAMETER name=""WrkDescription"" value=""" + sDescription + """ />" + Environment.NewLine
            sInputXML = sInputXML + "<PARAMETER name=""WrkStatus"" value=""" + nTaskStatus.ToString() + """ />" + Environment.NewLine
            sInputXML = sInputXML + "<PARAMETER name=""WrkIsUrgent"" value=""" + nIsUrgent.ToString() + """ />" + Environment.NewLine
            sInputXML = sInputXML + "<PARAMETER name=""WrkFlowInformation"" value=""" + sWorkflowInformation + """ />" + Environment.NewLine
            sInputXML = sInputXML + "<PARAMETER name=""WrkUserId"" value=""" + nUserID.ToString() + """ />" + Environment.NewLine
            sInputXML = sInputXML + "<PARAMETER name=""WrkIsVisible"" value=""" + nIsVisible.ToString() + """ />" + Environment.NewLine
            sInputXML = sInputXML + "<PARAMETER name=""WrkIsReview"" value=""" + nIsTaskReview.ToString() + """ />" + Environment.NewLine
            sInputXML = sInputXML + "<PARAMETER name=""WrkIsExternal"" value=""" + bIsExternalWorkItem.ToString() + """ />" + Environment.NewLine
            sInputXML = sInputXML + "<PARAMETER name=""WrkParentTaskId"" value=""" + nParentTaskId.ToString() + """ />" + Environment.NewLine
            sInputXML = sInputXML + "<PARAMETER name=""WrkExternalCategoryCode"" value=""" + sExternalTaskCategoryCode + """ />" + Environment.NewLine
            sInputXML = sInputXML + "<PARAMETER name=""WrkLockKeyName"" value=""" + LockNameString(sLockKeyName) + """ />" + Environment.NewLine
            sInputXML = sInputXML + "<PARAMETER name=""WrkLockKeyValue"" value=""" + nLockKeyValue.ToString() + """ />" + Environment.NewLine
            sInputXML = sInputXML + "<PARAMETER name=""WrkActionType"" value=""" + sActionType + """ />" + Environment.NewLine
            sInputXML = sInputXML + "<PARAMETER name=""WrkguidExternalItem"" value=""" + uGuidPMExternalItem.ToString() + """ />" + Environment.NewLine
            sInputXML = sInputXML + "<PARAMETER name=""WrkTaskInstanceCnt"" value=""" + PMWrkTaskInstanceCnt.ToString() + """ />" + Environment.NewLine
            sInputXML = sInputXML + "<PARAMETER name=""WrkExternalTaskStatus"" value=""" + nExternalTaskStatus.ToString() + """ />" + Environment.NewLine
            sInputXML = sInputXML + "<PARAMETER name=""WrkIsExternalChildTask"" value=""" + bIsExternalChildTask.ToString() + """ />" + Environment.NewLine

            sInputXML = sInputXML + "</PARAMETERS>" + Environment.NewLine  ' End tag of Parameters
            sInputXML = sInputXML + "<KEYDATA>" + Environment.NewLine  ' Start tag of KeyData

            If Not Informations.IsNothing(oKeyArray) Then
                If Informations.IsArray(oKeyArray) Then
                    For lRow As Integer = oKeyArray.GetLowerBound(1) To oKeyArray.GetUpperBound(1)
                        If Not Informations.IsArray(oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) And Not Object.Equals(oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow), Nothing) Then
                            sKeyName = oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)
                            sKeyValue = oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)

                            'Add Key Names and Value pair
                            sInputXML = sInputXML + "<KEY NAME=""" + gPMFunctions.ToSafeString(sKeyName) + """" + " VALUE=""" + gPMFunctions.ToSafeString(sKeyValue) + """ />" + Environment.NewLine

                        End If

                    Next lRow
                End If
            End If

            sInputXML = sInputXML + "</KEYDATA>" + Environment.NewLine  ' End tag of the KeyData
            sInputXML = sInputXML + "</JOB>" + Environment.NewLine  ' End tag of Job 
            sInputXML = sInputXML + "</BACKGROUND_JOB>" + Environment.NewLine  'End Tag of Background Job
            Dim nBackgroundjobID As Integer
            Dim sSafeInputXML As String = String.Empty
            m_lReturn = ToSafeXMLString(sInputXML, sSafeInputXML)

            m_lReturn = CreateBackgroundJob(o_nBackgroundjobID:=nBackgroundjobID, r_sBackGroundJobXml:=sInputXML, r_nJob_user_id:=m_iUserID, r_sBackGroundJobDescription:="")
            'This method gets called from a catch , so instead of returning thrwo an exception so it can be logged
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception("bPMWrkTaskInstanceBusiness.CreateBackgroundJob Failed")
            End If


            Return nResult

        Catch ex As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="GenerateXMLAndSchedule",
                               vApp:=ACApp, vClass:=ACClass,
                               vMethod:="GenerateXMLAndSchedule",
                               vErrNo:=Informations.Err().Number,
                               vErrDesc:=ex.Message)
            Return nResult

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetDetails
    '
    ' Description: Gets the details for the Task Instance.
    '
    ' AMB 20/01/2003 - workflow_information column added
    ' ***************************************************************** '
    Public Function GetDetails(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByRef r_lPMWrkTaskGroupID As Integer, ByRef r_lPMWrkTaskID As Integer, ByRef r_sCustomer As String, ByRef r_dtTaskDueDate As Date, ByRef r_lPMUserGroupID As Integer, ByRef r_iUserID As Integer, ByRef r_sDescription As String, ByRef r_iTaskStatus As Integer, ByRef r_iIsUrgent As Integer, ByRef r_dtDateCreated As Date, ByRef r_iCreatedByID As Integer, ByRef r_dtLastModified As Date, ByRef r_iModifiedByID As Integer, Optional ByRef r_sWorkflowInformation As String = "", Optional ByRef r_vIsVisible As Object = Nothing, Optional ByRef r_iIsTaskReview As Integer = 0) As Integer
        ' AMB 20/01/2003

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Select the Task Instance
            m_lReturn = GetTaskInstance(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Details.
            With m_oTaskInstance
                r_lPMWrkTaskGroupID = .PmwrkTaskGroupID
                r_lPMWrkTaskID = .PmwrkTaskID
                r_sCustomer = .Customer
                r_dtTaskDueDate = .TaskDueDate
                r_lPMUserGroupID = .PmuserGroupID
                r_iUserID = .UserID
                r_sDescription = .Description
                r_iTaskStatus = .TaskStatus
                r_iIsUrgent = .IsUrgent
                r_dtDateCreated = .DateCreated
                r_iCreatedByID = .CreatedByID
                r_dtLastModified = .LastModified
                r_iModifiedByID = .ModifiedByID
                ' AMB 20/01/2003
                r_sWorkflowInformation = .WorkflowInformation
                'DAK141299

                If Not Informations.IsNothing(r_vIsVisible) Then

                    .IsVisible = CInt(r_vIsVisible)
                End If
                r_iIsTaskReview = .IsTaskReview
            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetailsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetTaskInstKeys
    '
    ' Description: Gets all of the Keys for a Single Task Instance.
    '
    ' ***************************************************************** '
    Public Function GetTaskInstKeys(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByRef r_vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="pmwrk_task_instance_cnt", vValue:=CStr(v_lPMWrkTaskInstanceCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLSelect(sSQL:=ACGetTaskInstKeysSQL, sSQLName:=ACGetTaskInstKeysName, bStoredProcedure:=ACGetTaskInstKeysStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vKeyArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTaskInstKeysFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaskInstKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetTaskDetails
    '
    ' Description: Gets the details for the Task itself.
    '
    ' DAK121099 - Changes for Product Licencing and linked objects
    ' ***************************************************************** '
    Public Function GetTaskDetails(ByVal v_lPMWrkTaskID As Integer, ByRef r_iIsSystemTask As Integer, ByRef r_iTypeOfTask As Integer, ByRef r_lPMNavProcessID As Integer, ByRef r_sComponentObjectName As String, ByRef r_sComponentClassName As String, ByRef r_lAutoDeleteAfterNumDays As Integer, ByRef r_lDisplayIcon As Integer, ByRef r_iIsViewOnlyTask As Integer, ByRef r_sLinkedObjectName As String, ByRef r_sLinkedClassName As String, ByRef r_sLinkedCaption As String, ByRef r_iIsAvailableTask As Integer) As Integer
        Return GetTaskDetails(v_lPMWrkTaskID:=v_lPMWrkTaskID, r_iIsSystemTask:=r_iIsSystemTask, r_iTypeOfTask:=r_iTypeOfTask, r_lPMNavProcessID:=r_lPMNavProcessID, r_sComponentObjectName:=r_sComponentObjectName, r_sComponentClassName:=r_sComponentClassName, r_lAutoDeleteAfterNumDays:=r_lAutoDeleteAfterNumDays, r_lDisplayIcon:=r_lDisplayIcon, r_iIsViewOnlyTask:=r_iIsViewOnlyTask, r_sLinkedObjectName:=r_sLinkedObjectName, r_sLinkedClassName:=r_sLinkedClassName, r_sLinkedCaption:=r_sLinkedCaption, r_iIsAvailableTask:=r_iIsAvailableTask, r_sNavXMLfile:="", r_sTaskDescription:="")
    End Function

    Public Function GetTaskDetails(ByVal v_lPMWrkTaskID As Integer, ByRef r_iIsSystemTask As Integer, ByRef r_iTypeOfTask As Integer, ByRef r_lPMNavProcessID As Integer, ByRef r_sComponentObjectName As String, ByRef r_sComponentClassName As String, ByRef r_lAutoDeleteAfterNumDays As Integer, ByRef r_lDisplayIcon As Integer, ByRef r_iIsViewOnlyTask As Integer, ByRef r_sLinkedObjectName As String, ByRef r_sLinkedClassName As String, ByRef r_sLinkedCaption As String, ByRef r_iIsAvailableTask As Integer, ByRef r_sNavXMLfile As String) As Integer
        Return GetTaskDetails(v_lPMWrkTaskID:=v_lPMWrkTaskID, r_iIsSystemTask:=r_iIsSystemTask, r_iTypeOfTask:=r_iTypeOfTask, r_lPMNavProcessID:=r_lPMNavProcessID, r_sComponentObjectName:=r_sComponentObjectName, r_sComponentClassName:=r_sComponentClassName, r_lAutoDeleteAfterNumDays:=r_lAutoDeleteAfterNumDays, r_lDisplayIcon:=r_lDisplayIcon, r_iIsViewOnlyTask:=r_iIsViewOnlyTask, r_sLinkedObjectName:=r_sLinkedObjectName, r_sLinkedClassName:=r_sLinkedClassName, r_sLinkedCaption:=r_sLinkedCaption, r_iIsAvailableTask:=r_iIsAvailableTask, r_sNavXMLfile:=r_sNavXMLfile, r_sTaskDescription:="")
    End Function

    Public Function GetTaskDetails(ByVal v_lPMWrkTaskID As Integer, ByRef r_iIsSystemTask As Integer, ByRef r_iTypeOfTask As Integer, ByRef r_lPMNavProcessID As Integer, ByRef r_sComponentObjectName As String, ByRef r_sComponentClassName As String, ByRef r_lAutoDeleteAfterNumDays As Integer, ByRef r_lDisplayIcon As Integer, ByRef r_iIsViewOnlyTask As Integer, ByRef r_sLinkedObjectName As String, ByRef r_sLinkedClassName As String, ByRef r_sLinkedCaption As String, ByRef r_iIsAvailableTask As Integer, ByRef r_sNavXMLfile As String, ByRef r_sTaskDescription As String) As Integer

        Dim result As Integer = 0
        Dim oTask As bPMTask.Business
        ' RDC 13062002 CompServ repalced with BAS module
        'Dim oCS As sPMServerCS.PMServerBusinessCS

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Component Services
            '    Set oCS = New sPMServerCS.PMServerBusinessCS

            ' Create bPMTask.Business

            oTask = New bPMTask.Business
            m_lReturn = oTask.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            '    Set oCS = Nothing
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oTask = Nothing
                Return m_lReturn
            End If

            ' Get the Task Details from the DB

            m_lReturn = oTask.GetDetails(v_lPMWrkTaskID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oTask.Dispose()
                oTask = Nothing
                Return m_lReturn
            End If

            ' Return the Task Details

            m_lReturn = oTask.GetNext(vIsSystemTask:=r_iIsSystemTask, vTypeOfTask:=r_iTypeOfTask, vPMNavProcessId:=r_lPMNavProcessID, vComponentObjectName:=r_sComponentObjectName, vComponentClassName:=r_sComponentClassName, vAutoDeleteAfterNumDays:=r_lAutoDeleteAfterNumDays, vDisplayIcon:=r_lDisplayIcon, vIsViewOnlyTask:=r_iIsViewOnlyTask, vLinkedObjectName:=r_sLinkedObjectName, vLinkedClassName:=r_sLinkedClassName, vLinkedCaption:=r_sLinkedCaption, vIsAvailableTask:=r_iIsAvailableTask, vNavXMLFile:=r_sNavXMLfile, vDescription:=r_sTaskDescription)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oTask.Dispose()
                oTask = Nothing
                Return m_lReturn
            End If

            ' Terminate bPMTask.Business

            oTask.Dispose()
            oTask = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTaskDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaskDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AmendDetails
    '
    ' Description: Amend the Task Details.
    '
    ' Note:        The User Group/User ID can only be changed via the
    '              assign/reassign methods.
    '              The Task Status can only be changed via the StartTask,
    '              CompleteTask, InCompleteTask methods.
    '              The TaskType cannot be amended.
    'DAK141299
    '              Cannot change IsVisible.
    '
    ' 1 - Cannot change the details if the Task is InProgress, Complete
    ' 2 - Amend the details.
    '
    ' AMB 21/01/2003 - Added workflow_information field - IAG Spec 229
    ' ***************************************************************** '
    Public Function AmendDetails(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_sCustomer As String, ByVal v_dtTaskDueDate As Date, ByVal v_sDescription As String, ByVal v_iIsUrgent As Integer) As Integer
        Return AmendDetails(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_sCustomer:=v_sCustomer, v_dtTaskDueDate:=v_dtTaskDueDate, v_sDescription:=v_sDescription, v_iIsUrgent:=v_iIsUrgent, v_sWorkflowInformation:="", v_dtLastModified:=#12/30/1899#, v_iModifiedByID:=0, v_iIsTaskReview:=0)
    End Function

    Public Function AmendDetails(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_sCustomer As String, ByVal v_dtTaskDueDate As Date, ByVal v_sDescription As String, ByVal v_iIsUrgent As Integer, ByVal v_sWorkflowInformation As String) As Integer
        Return AmendDetails(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_sCustomer:=v_sCustomer, v_dtTaskDueDate:=v_dtTaskDueDate, v_sDescription:=v_sDescription, v_iIsUrgent:=v_iIsUrgent, v_sWorkflowInformation:=v_sWorkflowInformation, v_dtLastModified:=#12/30/1899#, v_iModifiedByID:=0, v_iIsTaskReview:=0)
    End Function

    Public Function AmendDetails(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_sCustomer As String, ByVal v_dtTaskDueDate As Date, ByVal v_sDescription As String, ByVal v_iIsUrgent As Integer, ByVal v_sWorkflowInformation As String, ByVal v_dtLastModified As Date) As Integer
        Return AmendDetails(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_sCustomer:=v_sCustomer, v_dtTaskDueDate:=v_dtTaskDueDate, v_sDescription:=v_sDescription, v_iIsUrgent:=v_iIsUrgent, v_sWorkflowInformation:=v_sWorkflowInformation, v_dtLastModified:=v_dtLastModified, v_iModifiedByID:=0, v_iIsTaskReview:=0)
    End Function

    Public Function AmendDetails(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_sCustomer As String, ByVal v_dtTaskDueDate As Date, ByVal v_sDescription As String, ByVal v_iIsUrgent As Integer, ByVal v_sWorkflowInformation As String, ByVal v_dtLastModified As Date, ByVal v_iModifiedByID As Integer) As Integer
        Return AmendDetails(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_sCustomer:=v_sCustomer, v_dtTaskDueDate:=v_dtTaskDueDate, v_sDescription:=v_sDescription, v_iIsUrgent:=v_iIsUrgent, v_sWorkflowInformation:=v_sWorkflowInformation, v_dtLastModified:=v_dtLastModified, v_iModifiedByID:=v_iModifiedByID, v_iIsTaskReview:=0)
    End Function

    Public Function AmendDetails(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_sCustomer As String, ByVal v_dtTaskDueDate As Date, ByVal v_sDescription As String, ByVal v_iIsUrgent As Integer, ByVal v_dtLastModified As Date, ByVal v_iModifiedByID As Integer) As Integer
        Return AmendDetails(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_sCustomer:=v_sCustomer, v_dtTaskDueDate:=v_dtTaskDueDate, v_sDescription:=v_sDescription, v_iIsUrgent:=v_iIsUrgent, v_sWorkflowInformation:="", v_dtLastModified:=v_dtLastModified, v_iModifiedByID:=v_iModifiedByID, v_iIsTaskReview:=0)
    End Function

    Public Function AmendDetails(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_sCustomer As String, ByVal v_dtTaskDueDate As Date, ByVal v_sDescription As String, ByVal v_iIsUrgent As Integer, ByVal v_sWorkflowInformation As String, ByVal v_dtLastModified As Date, ByVal v_iModifiedByID As Integer, ByVal v_iIsTaskReview As Integer) As Integer
        ' AMB 21/01/2003

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Select the Task Instance
            m_lReturn = GetTaskInstance(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Amend the Task Instance
            With m_oTaskInstance

                ' If the Task Is Already In Progress
                If .TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSInProgress Then
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot Amend a Task which is Already in Progress PMWrkTaskInstanceCnt = " & v_lPMWrkTaskInstanceCnt, vApp:=ACApp, vClass:=ACClass)
                    Return gPMConstants.PMEReturnCode.PMMAlreadyInUse
                End If

                ' If the Task is Complete
                If .TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete Then
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot Amend a Task which is Already Complete PMWrkTaskInstanceCnt = " & v_lPMWrkTaskInstanceCnt, vApp:=ACApp, vClass:=ACClass)
                    Return gPMConstants.PMEReturnCode.PMInvalidRequest
                End If

                .Customer = v_sCustomer
                .TaskDueDate = v_dtTaskDueDate
                .Description = v_sDescription
                .IsUrgent = v_iIsUrgent
                ' AMB 21/01/2003
                .WorkflowInformation = v_sWorkflowInformation

                If v_iModifiedByID > 0 Then
                    .LastModified = v_dtLastModified
                    .ModifiedByID = v_iModifiedByID
                Else
                    .LastModified = DateTime.Now
                    .ModifiedByID = m_iUserID
                End If
                .IsTaskReview = v_iIsTaskReview
            End With

            ' Update the Task Instance
            m_lReturn = m_oTaskInstance.Update()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AmendDetailsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="AmendDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Assign
    '
    ' Description: Assigns the new Task to a specific user.
    '
    ' Note: The Group must have already been specified when the Task was
    '       created.
    ' ***************************************************************** '
    Public Function Assign(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_iUserID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Select the Task Instance
            m_lReturn = GetTaskInstance(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the we can do this action.
            With m_oTaskInstance
                ' If the Task Is Already In Progress
                If .TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSInProgress Then
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot Assign a Task which is Already in Progress PMWrkTaskInstanceCnt = " & v_lPMWrkTaskInstanceCnt, vApp:=ACApp, vClass:=ACClass)
                    Return gPMConstants.PMEReturnCode.PMMAlreadyInUse
                End If

                ' If the Task is Complete
                If .TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete Then
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot Assign a Task which is Already Complete PMWrkTaskInstanceCnt = " & v_lPMWrkTaskInstanceCnt, vApp:=ACApp, vClass:=ACClass)
                    Return gPMConstants.PMEReturnCode.PMInvalidRequest
                End If

                ' Set the Assigned User ID
                .UserID = v_iUserID

                ' Set the Last Modified Fields
                .ModifiedByID = m_iUserID
                .LastModified = DateTime.Now

            End With

            ' Update the Task Instance
            m_lReturn = m_oTaskInstance.Update()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AssignFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="Assign", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ReAssign
    '
    ' Description: Reassign the Task to another Group or specific User.
    '
    ' 1 - Check that the Task is not InProgress or Complete.
    ' 2 - ReAssign the Task
    ' ***************************************************************** '
    Public Function ReAssign(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_lPMUserGroupID As Integer) As Integer
        Return ReAssign(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_lPMUserGroupID:=v_lPMUserGroupID, v_iUserID:=0)
    End Function

    Public Function ReAssign(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_lPMUserGroupID As Integer, Optional ByVal v_iUserID As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Select the Task Instance
            m_lReturn = GetTaskInstance(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            With m_oTaskInstance

                ' If the Task Is Already In Progress
                If .TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSInProgress Then
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot Assign a Task which is Already in Progress PMWrkTaskInstanceCnt = " & v_lPMWrkTaskInstanceCnt, vApp:=ACApp, vClass:=ACClass)
                    Return gPMConstants.PMEReturnCode.PMMAlreadyInUse
                End If

                ' If the Task is Complete
                If .TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete Then
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot Start a Task which is Already Complete PMWrkTaskInstanceCnt = " & v_lPMWrkTaskInstanceCnt, vApp:=ACApp, vClass:=ACClass)
                    Return gPMConstants.PMEReturnCode.PMInvalidRequest
                End If

                ' Set the Assigned User Group
                .PmuserGroupID = v_lPMUserGroupID
                ' If supplied, set the assigned User
                If v_iUserID > 0 Then
                    .UserID = v_iUserID
                Else

                    .UserID = Nothing
                End If
                ' Set the last Amended Fields
                .ModifiedByID = m_iUserID
                .LastModified = DateTime.Now
            End With

            ' Update the Task Instance
            m_lReturn = m_oTaskInstance.Update()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReAssignFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReAssign", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetStatusComplete
    '
    ' Description: Set the Task Status to Complete if the action
    '              is valid.
    ' ***************************************************************** '
    Public Function SetStatusComplete(ByVal v_lPMWrkTaskInstanceCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Select the Task Instance
            m_lReturn = GetTaskInstance(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the we can do this action.
            With m_oTaskInstance

                '        ' If the Task is Assigned to Someone Else
                '        If (.UserID <> 0) _
                ''        And (.UserID <> m_iUserID) Then
                '            LogMessage "", _
                ''                iType:=PMLogError, _
                ''                sMsg:="Cannot Complete a Task which is Assigned to someone else PMWrkTaskInstanceCnt = " _
                ''                    & v_lPMWrkTaskInstanceCnt, _
                ''                vApp:=ACApp, _
                ''                vClass:=ACClass
                '            SetStatusComplete = PMMNoAccess
                '            Exit Function
                '        End If

                ' Mark it as In Progress
                .TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete
                .UserID = m_iUserID
                ' Set the Last Modified Fields
                .ModifiedByID = m_iUserID
                .LastModified = DateTime.Now

            End With

            ' Update the Task Instance
            m_lReturn = m_oTaskInstance.Update()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatusCompleteFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatusComplete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetStatusInComplete
    '
    ' Description: Set the Task Status to Incomplete if this action
    '              is valid.
    '
    ' ***************************************************************** '
    Public Function SetStatusInComplete(ByVal v_lPMWrkTaskInstanceCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Select the Task Instance
            m_lReturn = GetTaskInstance(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the we can do this action.
            With m_oTaskInstance

                '        ' If the Task is Assigned to Someone Else
                '        If (.UserID <> 0) _
                ''        And (.UserID <> m_iUserID) Then
                '            LogMessage "", _
                ''                iType:=PMLogError, _
                ''                sMsg:="Cannot Start a Task which is Assigned to someone else PMWrkTaskInstanceCnt = " _
                ''                    & v_lPMWrkTaskInstanceCnt, _
                ''                vApp:=ACApp, _
                ''                vClass:=ACClass
                '            SetStatusInComplete = PMMNoAccess
                '            Exit Function
                '        End If

                ' Mark it as In Progress
                '.TaskStatus = pmeWMTSIncomplete

                If .Description = "Quote Manager" Then

                    .TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSInProgress
                Else
                    .TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSIncomplete
                End If
                ' Set the Last Modified Fields
                .ModifiedByID = m_iUserID
                .LastModified = DateTime.Now

            End With

            ' Update the Task Instance
            m_lReturn = m_oTaskInstance.Update()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatusInCompleteFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatusInComplete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetStatusInProgress
    '
    ' Description: Set the Task Status to In Progress if this action
    '              is valid.
    '
    ' DAK121099 - Changes for Product Licencing and linked objects
    ' ***************************************************************** '
    Public Function SetStatusInProgress(ByVal v_lPMWrkTaskInstanceCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Select the Task Instance
            m_lReturn = GetTaskInstance(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the we can do this action.
            With m_oTaskInstance
                ' If the Task Is Already In Progress
                If .TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSInProgress Then
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot Start a Task which is Already in Progress PMWrkTaskInstanceCnt = " & v_lPMWrkTaskInstanceCnt, vApp:=ACApp, vClass:=ACClass)
                    Return gPMConstants.PMEReturnCode.PMMAlreadyInUse
                End If

                ' If the Task Is Already Complete
                If .TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete Then
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot Start a Task which is Already Complete PMWrkTaskInstanceCnt = " & v_lPMWrkTaskInstanceCnt, vApp:=ACApp, vClass:=ACClass)
                    Return gPMConstants.PMEReturnCode.PMInvalidRequest
                End If

                ' If the licence limit has been exceeded for the category
                m_lReturn = CheckLicenceLimit(v_lPMTaskInstanceCnt:= .PMWrkTaskInstanceCnt, v_lPMTaskID:= .PmwrkTaskID)
                result = m_lReturn
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMWarnLicenceExceeded Then
                    Return result
                End If

                '        ' If the Task is Assigned to Someone Else
                '        If (.UserID <> 0) _
                ''        And (.UserID <> m_iUserID) Then
                '            LogMessage "", _
                ''                iType:=PMLogError, _
                ''                sMsg:="Cannot Start a Task which is Assigned to someone else PMWrkTaskInstanceCnt = " _
                ''                    & v_lPMWrkTaskInstanceCnt, _
                ''                vApp:=ACApp, _
                ''                vClass:=ACClass
                '            SetStatusInProgress = PMMNoAccess
                '            Exit Function
                '        End If

                ' Mark it as In Progress
                .TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSInProgress

                ' Set the Assigned User to the User who is running it.
                .UserID = m_iUserID
                ' Set the audit fields
                .ModifiedByID = m_iUserID
                .LastModified = DateTime.Now

            End With

            ' Update the Task Instance
            m_lReturn = m_oTaskInstance.Update()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatusInProgressFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatusInProgress", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Delete
    '
    ' Description: Delete a Task and associated bits.
    '
    '
    ' ***************************************************************** '
    Public Function Delete(ByVal v_lPMWrkTaskInstanceCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Select the Task Instance
            m_lReturn = GetTaskInstance(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check that we can do this Action
            With m_oTaskInstance
                ' If the Task Is Already In Progress
                If .TaskStatus <> gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete Then
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot Delete a Task which is NOT Complete PMWrkTaskInstanceCnt = " & v_lPMWrkTaskInstanceCnt, vApp:=ACApp, vClass:=ACClass)
                    Return gPMConstants.PMEReturnCode.PMInvalidRequest
                End If
            End With

            ' Delete the Task Instance
            m_lReturn = m_oTaskInstance.Delete()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AutoDelete
    '
    ' Description: Automatically delete Completed Tasks.
    ' ***************************************************************** '
    Public Function AutoDelete() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oDatabase.SQLBeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Call the Stored Procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAutoDeleteSQL, sSQLName:=ACAutoDeleteName, bStoredProcedure:=ACAutoDeleteStored)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLCommitTrans()
            Else
                m_lReturn = m_oDatabase.SQLRollbackTrans()
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoDeleteFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: GetTaskInstance
    '
    ' Description: Gets the Task Instance for the Specified Key.
    '              It may already be the one loaded, so check first,
    '              before going to the database.
    ' ***************************************************************** '
    Private Function GetTaskInstance(ByVal v_lPMWrkTaskInstanceCnt As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_lPMWrkTaskInstanceCnt < 1 Then
                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="PMWrkTaskInstanceCnt must be specified.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaskInstance")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_lPMWrkTaskInstanceCnt = m_oTaskInstance.PMWrkTaskInstanceCnt Then
                ' Already Loaded, Nothing to do.
                m_lReturn = m_oTaskInstance.SelectSingle()
            Else
                ' Create a New Task Instance
                m_lReturn = NewTaskInstance()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                ' Load the data from the Database
                m_oTaskInstance.PMWrkTaskInstanceCnt = v_lPMWrkTaskInstanceCnt
                m_lReturn = m_oTaskInstance.SelectSingle()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTaskInstanceFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaskInstance", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: NewTaskInstance
    '
    ' Description: Creates a New Task Instance
    '
    ' ***************************************************************** '
    Private Function NewTaskInstance() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Terminate the Existing Task Instance
            m_oTaskInstance.Dispose()
            ' Create a new one
            m_oTaskInstance = New bPMWrkTaskInstance.PMWrkTaskInstance()
            m_lReturn = m_oTaskInstance.Initialise(vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewTaskInstanceFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewTaskInstance", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddTaskInstKey
    '
    ' Description: Adds a single Task Instance Key
    '
    ' ***************************************************************** '
    Private Function AddTaskInstKey(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_sKeyName As String, ByVal v_sKeyValue As String) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="pmwrk_task_instance_cnt", vValue:=CStr(v_lPMWrkTaskInstanceCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="key_name", vValue:=v_sKeyName.Trim(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="key_value", vValue:=v_sKeyValue.Trim(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLAction(sSQL:=ACAddTaskInstKeySQL, sSQLName:=ACAddTaskInstKeyName, bStoredProcedure:=ACAddTaskInstKeyStored, lRecordsAffected:=lRecordsAffected)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If lRecordsAffected < 1 Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTaskInstKeyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskInstKey", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckLicenceLimit
    '
    ' Description:
    '
    ' History: 12/10/1999 DAK - Created.
    ' DAK231299 - Replace Task Group Category with Task Category
    ' ***************************************************************** '
    Private Function CheckLicenceLimit(ByRef v_lPMTaskInstanceCnt As Integer, ByRef v_lPMTaskID As Integer) As Integer

        ' RDC 13062002 CompServ replaced with BAS module
        'Dim oCS As sPMServerCS.PMServerBusinessCS
        Dim result As Integer = 0
        Dim oTask As bPMTask.Business
        Dim iIsViewOnlyTask As gPMConstants.PMEReturnCode
        Dim oCategoryLookup As bPMTaskCategory.Lookup
        'DAK231299
        Dim lPMTaskCategoryID As Integer
        Dim oCategoryBusiness As bPMTaskCategory.Business
        Dim lLicenceLimit As Integer
        Dim iIsBlockAboveLicenceLimit As gPMConstants.PMEReturnCode
        Dim iIsWarnAboveLicenceLimit As gPMConstants.PMEReturnCode
        Dim lWarnsSinceLicenceUpgrade, lCategoryTaskCount As Integer



        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Component Services
            '    Set oCS = New sPMServerCS.PMServerBusinessCS

            ' Create bPMTask.Business

            oTask = New bPMTask.Business
            m_lReturn = oTask.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            '    Set oCS = Nothing

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oTask = Nothing
                Return m_lReturn
            End If

            ' Get the Task Details from the DB

            m_lReturn = oTask.GetDetails(v_lPMTaskID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                oTask.Dispose()
                oTask = Nothing
                Return result
            End If

            ' Return the whether the Task is view only or not

            m_lReturn = oTask.GetNext(vIsViewOnlyTask:=iIsViewOnlyTask)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                oTask.Dispose()
                oTask = Nothing
                Return result
            End If

            ' Terminate bPMTask.Business

            oTask.Dispose()
            oTask = Nothing

            If iIsViewOnlyTask = gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' Create Component Services
            '    Set oCS = New sPMServerCS.PMServerBusinessCS

            'DAK231299
            ' Create bPMTaskCategory.Lookup

            oCategoryLookup = New bPMTaskCategory.Lookup
            m_lReturn = oCategoryLookup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            '    Set oCS = Nothing
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oCategoryLookup = Nothing
                Return result
            End If

            ' Get the Category id for this task instance
            'DAK231299

            m_lReturn = oCategoryLookup.GetInstanceCategory(v_lPMTaskInstanceCnt:=v_lPMTaskInstanceCnt, r_lPMTaskCategoryID:=lPMTaskCategoryID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                oCategoryLookup.Dispose()
                oCategoryLookup = Nothing
                Return result
            End If

            ' Terminate bPMTaskGroupCategory.Lookup

            oCategoryLookup.Dispose()
            oCategoryLookup = Nothing

            ' Create Component Services
            '    Set oCS = New sPMServerCS.PMServerBusinessCS

            'DAK231299
            ' Create bPMTaskCategory.Business

            oCategoryBusiness = New bPMTaskCategory.Business
            m_lReturn = oCategoryBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            '    Set oCS = Nothing
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oCategoryBusiness = Nothing
                Return result
            End If

            ' Get the category details
            'DAK231299

            m_lReturn = oCategoryBusiness.GetDetails(vPMTaskCategoryId:=lPMTaskCategoryID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                oCategoryBusiness.Dispose()
                oCategoryBusiness = Nothing
                Return result
            End If

            'DAK070100

            oCategoryBusiness.CurrentRecord = 1

            m_lReturn = oCategoryBusiness.ValidateLicenceLimit()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                oCategoryBusiness.Dispose()
                oCategoryBusiness = Nothing
                Return result
            End If


            m_lReturn = oCategoryBusiness.GetNext(vLicenceLimit:=lLicenceLimit, vIsBlockAboveLicenceLimit:=iIsBlockAboveLicenceLimit, vIsWarnAboveLicenceLimit:=iIsWarnAboveLicenceLimit, vWarnsSinceLicenceUpgrade:=lWarnsSinceLicenceUpgrade)

            ' Get the number of current licenced tasks in progress for the
            ' category from the DB
            'DAK231299

            m_lReturn = oCategoryBusiness.CountCategoryTasks(v_lPMTaskCategoryID:=lPMTaskCategoryID, r_lCategoryTaskCount:=lCategoryTaskCount)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                oCategoryBusiness.Dispose()
                oCategoryBusiness = Nothing
                Return result
            End If

            If lCategoryTaskCount >= lLicenceLimit Then
                If iIsBlockAboveLicenceLimit = gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMBlockLicenceExceeded
                ElseIf iIsWarnAboveLicenceLimit = gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMWarnLicenceExceeded
                    ' Increment the number of warnings for the category

                    m_lReturn = oCategoryBusiness.EditUpdate(lRow:=1, vWarnsSinceLicenceUpgrade:=(lWarnsSinceLicenceUpgrade + 1))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn

                        oCategoryBusiness.Dispose()
                        oCategoryBusiness = Nothing
                        Return result
                    End If


                    m_lReturn = oCategoryBusiness.Update()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn

                        oCategoryBusiness.Dispose()
                        oCategoryBusiness = Nothing
                        Return result
                    End If
                End If
            End If


            oCategoryBusiness.Dispose()
            oCategoryBusiness = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckLicenceLimit Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckLicenceLimit", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Try
            Dispose(False)
        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Terminate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Terminate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Exit Sub

        End Try
    End Sub


    ' ***************************************************************** '
    ' Name: ReAssignMultipleTask
    '
    ' Description: Reassign Multiple Tasks to another Group or specific User.
    '
    ' Edit History
    ' RAM20020715 : Created
    ' ***************************************************************** '
    Public Function ReAssignMultipleTask(ByVal v_vPMWrkTaskInstanceCntArray As Object, ByVal v_lPMUserGroupID As Integer) As Integer
        Return ReAssignMultipleTask(v_vPMWrkTaskInstanceCntArray:=v_vPMWrkTaskInstanceCntArray, v_lPMUserGroupID:=v_lPMUserGroupID, v_iUserID:=0)
    End Function

    Public Function ReAssignMultipleTask(ByVal v_vPMWrkTaskInstanceCntArray As Object, ByVal v_lPMUserGroupID As Integer, Optional ByVal v_iUserID As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Informations.IsArray(v_vPMWrkTaskInstanceCntArray) Then


                For iCounter As Integer = v_vPMWrkTaskInstanceCntArray.GetLowerBound(0) To v_vPMWrkTaskInstanceCntArray.GetUpperBound(0)

                    With m_oDatabase

                        .Parameters.Clear()

                        'pmwrk_task_instance_cnt_multiple


                        m_lReturn = .Parameters.Add(sName:="pmwrk_task_instance_cnt", vValue:=CStr(v_vPMWrkTaskInstanceCntArray(iCounter)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = .Parameters.Add(sName:="pmuser_group_id", vValue:=CStr(v_lPMUserGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If


                        If v_iUserID < 1 Or Convert.IsDBNull(v_iUserID) Or Informations.IsNothing(v_iUserID) Then

                            'developers guide no. 85
                            m_lReturn = .Parameters.Add(sName:="user_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                        Else
                            m_lReturn = .Parameters.Add(sName:="user_id", vValue:=CStr(v_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                        End If
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        ' Update the last Modified Details
                        m_lReturn = .Parameters.Add(sName:="modified_by_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        'developer guide no. 40
                        m_lReturn = .Parameters.Add(sName:="last_modified", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        ' Execute SQL Statement
                        m_lReturn = .SQLAction(sSQL:=ACUpdateReAssignTasksSQL, sSQLName:=ACUpdateReAssignTasksName, bStoredProcedure:=ACUpdateReAssignTasksStored, lRecordsAffected:=lRecordsAffected)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        ' Check to see that the record was updated OK
                        If lRecordsAffected > 0 Then
                            ' Updated No action required
                        Else
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    End With

                Next iCounter

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReAssignMultipleTask Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReAssignMultipleTask", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    Private Shared _DefaultInstance As Business = Nothing
    Public Shared ReadOnly Property DefaultInstance() As Business
        Get
            If _DefaultInstance Is Nothing Then
                _DefaultInstance = New Business
            End If
            Return _DefaultInstance
        End Get
    End Property

    Public Function UpdateTaskStatus(ByVal nPMWrkTaskInstanceCnt As Integer,
                                     ByVal nTaskStatus As Integer,
                                     ByVal sGuidPMExternalItem As String,
                                     ByVal bViaBackgrounJobProcess As Boolean,
                                     ByVal nExternalTaskStatus As Integer, ByRef sErrMessageUpdateTask As String) As Integer

        Dim nResult As Integer = 0
        Dim bExternalWorkItem As Boolean = False
        Dim bScheduleInBackgroundJob As Boolean = False
        Dim uGuidPMExternalItem As New System.Guid(sGuidPMExternalItem)
        Dim sErrorMessage As String = String.Empty

        'Task Status Mapping
        'Pure InComplete - E5 Activate , value = 0
        'Pure Compelte    - E5 Complete, value =  6 

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = GetTaskInstance(v_lPMWrkTaskInstanceCnt:=nPMWrkTaskInstanceCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Dim nPMUserGroupID As Integer = m_oTaskInstance.PmuserGroupID

            'Check External WorkFlow Configuration is switched on or off.
            Dim sValue As Object = String.Empty
            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername,
                                          v_sPassword:=m_sPassword,
                                          v_iUserID:=m_iUserID,
                                          v_iMainSourceID:=m_iSourceID,
                                          v_iLanguageID:=m_iLanguageID,
                                          v_iCurrencyID:=m_iCurrencyID,
                                          v_iLogLevel:=m_iLogLevel,
                                          v_sCallingAppName:=m_sCallingAppName,
                                          v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIRROPTEnableExternalWorkflowSystem,
                                          v_vBranch:=m_iSourceID, r_vUnderwriting:=sValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername,
                                   iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="getProductOptionValue Failed",
                                   vApp:=ACApp,
                                   vClass:=ACClass,
                                   vMethod:="CreateNew",
                                   vErrNo:=Informations.Err().Number,
                                   vErrDesc:=Informations.Err().Description)
            End If

            'If system option is not on then bIsExternalWorkItem = False
            If gPMFunctions.NullToString(sValue) = "0" OrElse gPMFunctions.NullToString(sValue) = "" Then
                bExternalWorkItem = False

            ElseIf gPMFunctions.NullToString(sValue) = "1" AndAlso bExternalWorkItem = False Then
                'If Option is on but bIsExternalWorkItem is False
                'check the user group configured through ExternalWorkFlow task
                Dim oUserGroup(,) As Object = Nothing
                m_oDatabase.Parameters.Clear()
                If m_oDatabase.Parameters.Add(sName:="dtEffective_date", vValue:=Date.Today,
                                              iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                              iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If m_oDatabase.Parameters.Add(sName:="nSelected_UserGroup_id", vValue:=nPMUserGroupID,
                                              iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                              iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=KSelExternalWorkFlowConfiguration_UsergroupsSQL,
                                                  sSQLName:=KSelExternalWorkFlowConfiguration_UsergroupName,
                                                  bStoredProcedure:=KSelExternalWorkFlowConfiguration_UsergroupStored,
                                                  vResultArray:=oUserGroup,
                                                  bKeepNulls:=True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'We have found the record for the user group, so set bIsExternalWorkItem = True
                If oUserGroup IsNot Nothing Then
                    bExternalWorkItem = True
                    If oUserGroup(5, 0) IsNot Nothing AndAlso oUserGroup(5, 0) = 1 Then
                        bScheduleInBackgroundJob = True
                    End If
                End If
            End If


            If bExternalWorkItem = True Then
                m_oDatabase.SQLBeginTrans()
            End If

            Select Case nTaskStatus
                Case 0
                    'gPMConstants.PMEWrkManTaskStatus.pmeWMTSNew

                Case 1
                    'gPMConstants.PMEWrkManTaskStatus.pmeWMTSInProgress

                    'Set the values
                    With m_oTaskInstance

                        ' If the Task Is Already In Progress
                        If .TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSInProgress Then
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot Start a Task which is Already in Progress PMWrkTaskInstanceCnt = " & nPMWrkTaskInstanceCnt, vApp:=ACApp, vClass:=ACClass)
                            Return gPMConstants.PMEReturnCode.PMMAlreadyInUse
                        End If

                        ' If the Task Is Already Complete
                        If .TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete Then
                            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot Start a Task which is Already Complete PMWrkTaskInstanceCnt = " & nPMWrkTaskInstanceCnt, vApp:=ACApp, vClass:=ACClass)
                            Return gPMConstants.PMEReturnCode.PMInvalidRequest
                        End If

                        ' If the licence limit has been exceeded for the category
                        m_lReturn = CheckLicenceLimit(v_lPMTaskInstanceCnt:= .PMWrkTaskInstanceCnt, v_lPMTaskID:= .PmwrkTaskID)
                        nResult = m_lReturn
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMWarnLicenceExceeded Then
                            Return nResult
                        End If
                        .TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSInProgress
                        .UserID = m_iUserID
                        .ModifiedByID = m_iUserID
                        .LastModified = DateTime.Now

                    End With

                Case 2
                    'gPMConstants.PMEWrkManTaskStatus.pmeWMTSIncomplete

                    'Set the values
                    With m_oTaskInstance
                        If .Description = "Quote Manager" Then
                            .TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSInProgress
                        Else
                            .TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSIncomplete
                        End If
                        .ModifiedByID = m_iUserID
                        .LastModified = DateTime.Now
                    End With

                Case 3
                    'gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete

                    'Set the values
                    With m_oTaskInstance
                        .TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete
                        .ModifiedByID = m_iUserID
                        .LastModified = DateTime.Now
                    End With

                Case 4
                    'gPMConstants.PMEWrkManTaskStatus.pmeWMTSDeleted
            End Select

            Dim oExternalWorkItem As Object = Nothing
            If bExternalWorkItem Then

                If nExternalTaskStatus = -1 Then
                    Throw New Exception("External Task Status " + nExternalTaskStatus + " is invalid.")
                End If

                'Create the object 
                oExternalWorkItem = New bExternalWorkItem.e5IntegrationProxyLayer.Business

                'Set some intial values
                m_lReturn = oExternalWorkItem.Initialise(ToSafeString(m_sUsername), ToSafeString(m_sPassword), ToSafeInteger(m_iUserID), ToSafeInteger(m_iSourceID),
                                             ToSafeInteger(m_iLanguageID), ToSafeInteger(m_iCurrencyID), ToSafeInteger(m_iLogLevel), ToSafeString(m_sCallingAppName))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Raise exception , so we can rollback the transaction and schedule it background job
                    Throw New Exception("bExternalWorkItem.Initialise Failed")
                End If

                'Make a call to UpdateExternalWorkItemStatus, this will do all the processing.
                m_lReturn = oExternalWorkItem.UpdateExternalWorkItemStatus(CType(uGuidPMExternalItem, Guid), ToSafeInteger(nExternalTaskStatus), ToSafeString(sErrorMessage))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Raise exception , so we can rollback the transaction and schedule it background job
                    If Not String.IsNullOrEmpty(sErrorMessage) Then
                        Throw New Exception(sErrorMessage)
                    Else
                        Throw New Exception("bExternalWorkItem.UpdateExternalWorkItemStatus Failed")
                    End If
                End If
            End If
            'Every thing is successfull , update the relevant values as rest of them already set
            m_lReturn = m_oTaskInstance.Update()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Raise exception , so we can rollback the transaction and schedule it background job
                Throw New Exception("bPMWRKTaskInstance.Update Failed")
            End If

            If bExternalWorkItem Then
                oExternalWorkItem = Nothing
                m_oDatabase.SQLCommitTrans()
            End If


            Return nResult

        Catch ex As System.Exception
            If bExternalWorkItem Then
                m_oDatabase.SQLRollbackTrans()


                'No locking required
                If bScheduleInBackgroundJob = True AndAlso
                bViaBackgrounJobProcess = False Then
                    With m_oTaskInstance
                        GenerateXMLAndSchedule(nPMWrkTaskID:= .PmwrkTaskID,
                           nPMWrkTaskGroupID:= .PmwrkTaskGroupID,
                           sCustomer:= .Customer,
                           dtTaskDueDate:= .TaskDueDate,
                           nPMUserGroupID:= .PmuserGroupID,
                           sDescription:= .Description,
                           nTaskStatus:=nTaskStatus,
                           nIsUrgent:= .IsUrgent,
                           sWorkflowInformation:= .WorkflowInformation,
                           dtDateCreated:=Date.Today,
                           nCreatedByID:= .CreatedByID,
                           nUserID:= .UserID,
                           oKeyArray:=Nothing,
                           nIsVisible:= .IsVisible,
                           nIsTaskReview:= .IsTaskReview,
                           bIsExternalWorkItem:= .IsExternalWorkItem,
                           nParentTaskId:= .ParentTaskId,
                           sExternalTaskCategoryCode:="",
                           sLockKeyName:="",
                           nLockKeyValue:=0, sActionType:="UPDATETASK",
                           uGuidPMExternalItem:=uGuidPMExternalItem,
                           PMWrkTaskInstanceCnt:=nPMWrkTaskInstanceCnt,
                           nExternalTaskStatus:=nExternalTaskStatus,
                           bIsExternalChildTask:=False)
                    End With
                End If



            End If
            If Not String.IsNullOrEmpty(sErrorMessage) Then
                sErrMessageUpdateTask = sErrorMessage
            Else
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatusCompleteFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatusComplete", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            End If
            nResult = gPMConstants.PMEReturnCode.PMError
            Return nResult
        End Try
    End Function


    ''' <summary>
    '''  This Method is Used to make a backGround Job.
    ''' </summary>
    ''' <param name="r_sBackGroundJobXml"></param>
    ''' <param name="r_nJob_user_id"></param>
    ''' <param name="o_nBackgroundjobID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateBackgroundJob(ByRef o_nBackgroundjobID As Integer, ByVal r_sBackGroundJobXml As String,
                                    ByVal r_nJob_user_id As Integer
                                      ) As Integer
        Return CreateBackgroundJob(o_nBackgroundjobID:=o_nBackgroundjobID, r_sBackGroundJobXml:=r_sBackGroundJobXml,
                                    r_nJob_user_id:=r_nJob_user_id,
                                    r_sBackGroundJobDescription:=""
                                      )
    End Function
    Public Function CreateBackgroundJob(ByRef o_nBackgroundjobID As Integer, ByVal r_sBackGroundJobXml As String,
                                        ByVal r_nJob_user_id As Integer,
                                        Optional ByVal r_sBackGroundJobDescription As String = ""
                                          ) As Integer

        Dim nResult As Integer

        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="background_job_id", vValue:=0, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput,
                                            iDataType:=gPMConstants.PMEDataType.PMLong)

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    Return nResult
                End If

                If Trim(r_sBackGroundJobDescription) = "" Then
                    r_sBackGroundJobDescription = "External Work Manager"
                End If
                nResult = .Parameters.Add(sName:="description", vValue:=CStr(r_sBackGroundJobDescription), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                          iDataType:=gPMConstants.PMEDataType.PMString)

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    Return nResult
                End If

                nResult = .Parameters.Add(sName:="job_xml", vValue:=CStr(r_sBackGroundJobXml), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                          iDataType:=gPMConstants.PMEDataType.PMString)

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    Return nResult
                End If

                nResult = .Parameters.Add(sName:="job_when_to_start", vValue:=DateTime.Now().Date, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                             iDataType:=gPMConstants.PMEDataType.PMDate)


                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    Return nResult
                End If

                nResult = .Parameters.Add(sName:="job_user_id", vValue:=r_nJob_user_id, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                          iDataType:=gPMConstants.PMEDataType.PMInteger)

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    Return nResult
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=kBackgroundJobAddSQL, sSQLName:=kBackgroundJobAddName, bStoredProcedure:=kBackgroundJobAddStored)

                o_nBackgroundjobID = Convert.ToInt32(.Parameters.Item("background_job_id").Value)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            Return nResult

        Catch ex As Exception
            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateBackgroundJob Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBackgroundJob", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            nResult = gPMConstants.PMEReturnCode.PMFalse
            Return nResult
            Throw ex
        End Try

    End Function

End Class
