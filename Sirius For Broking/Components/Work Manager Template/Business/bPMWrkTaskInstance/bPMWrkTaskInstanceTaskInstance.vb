Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
'developer guide no 129. 
'Start
Imports SSP.Shared
'End
Friend NotInheritable Class PMWrkInstance
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: PMWrkTaskInstance
    '
    ' Date: 30/10/1998
    '
    ' Description: Describes the PMWrkTaskInstance attributes.
    '
    ' Edit History:
    ' DAK141299 - Add is_visible column to task instance
    ' DAK080200 - Prevent tasks from disappearing when started
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "PMWrkTaskInstance"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag
    Private m_bCloseDatabase As Boolean

    ' DataBase Attributes
    Private m_lPmwrkTaskInstanceCnt As Integer
    Private m_lPmwrkTaskGroupID As Integer
    Private m_lPmwrkTaskID As Integer
    Private m_sCustomer As String = ""
    Private m_dtTaskDueDate As Date
    Private m_lPmuserGroupID As Integer
    Private m_vUserID As Integer
    Private m_sDescription As String = ""
    Private m_iTaskStatus As Integer
    Private m_iIsUrgent As Integer
    Private m_dtDateCreated As Date
    Private m_iCreatedByID As Integer
    Private m_vLastModified As Date
    Private m_vModifiedByID As Integer
    'DAK141299
    ' IsVisible
    Private m_iIsVisible As Integer
    Private m_sWorkflowInformation As String = "" 'PN15774

    ' Function Return Code
    Private m_lReturn As gPMConstants.PMEReturnCode
    ' ************************************************
    ' Added to replace global variables 11/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As gPMConstants.PMEWrkManTaskStatus

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture

        End Get
    End Property

    Public WriteOnly Property Database() As dPMDAO.Database
        Set(ByVal Value As dPMDAO.Database)

            m_oDatabase = Value

        End Set
    End Property


    Public Property PMWrkTaskInstanceCnt() As Integer
        Get

            Return m_lPmwrkTaskInstanceCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPmwrkTaskInstanceCnt = Value

        End Set
    End Property

    Public Property PmwrkTaskGroupID() As Integer
        Get

            Return m_lPmwrkTaskGroupID

        End Get
        Set(ByVal Value As Integer)

            m_lPmwrkTaskGroupID = Value

        End Set
    End Property

    Public Property PmwrkTaskID() As Integer
        Get

            Return m_lPmwrkTaskID

        End Get
        Set(ByVal Value As Integer)

            m_lPmwrkTaskID = Value

        End Set
    End Property

    Public Property Customer() As String
        Get

            Return m_sCustomer.Trim()

        End Get
        Set(ByVal Value As String)

            m_sCustomer = Value.Trim()

        End Set
    End Property

    Public Property TaskDueDate() As Date
        Get

            Return m_dtTaskDueDate

        End Get
        Set(ByVal Value As Date)

            m_dtTaskDueDate = Value

        End Set
    End Property

    Public Property PmuserGroupID() As Integer
        Get

            Return m_lPmuserGroupID

        End Get
        Set(ByVal Value As Integer)

            m_lPmuserGroupID = Value

        End Set
    End Property

    Public Property UserID() As Integer
        Get

            Return m_vUserID

        End Get
        Set(ByVal Value As Integer)


            m_vUserID = CInt(Value)

        End Set
    End Property

    Public Property Description() As String
        Get

            Return m_sDescription.Trim()

        End Get
        Set(ByVal Value As String)

            m_sDescription = Value.Trim()

        End Set
    End Property

    Public Property TaskStatus() As Integer
        Get

            Return m_iTaskStatus

        End Get
        Set(ByVal Value As Integer)

            m_iTaskStatus = Value

        End Set
    End Property

    Public Property IsUrgent() As Integer
        Get

            Return m_iIsUrgent

        End Get
        Set(ByVal Value As Integer)

            m_iIsUrgent = Value

        End Set
    End Property

    Public Property DateCreated() As Date
        Get

            Return m_dtDateCreated

        End Get
        Set(ByVal Value As Date)

            m_dtDateCreated = Value

        End Set
    End Property

    Public Property CreatedByID() As Integer
        Get

            Return m_iCreatedByID

        End Get
        Set(ByVal Value As Integer)

            m_iCreatedByID = Value

        End Set
    End Property

    Public Property LastModified() As Date
        Get

            Return m_vLastModified

        End Get
        Set(ByVal Value As Date)


            m_vLastModified = CDate(Value)

        End Set
    End Property

    Public Property ModifiedByID() As Integer
        Get

            Return m_vModifiedByID

        End Get
        Set(ByVal Value As Integer)


            m_vModifiedByID = CInt(Value)

        End Set
    End Property

    'DAK141299
    Public Property IsVisible() As Integer
        Get
            Return m_iIsVisible
        End Get
        Set(ByVal Value As Integer)
            m_iIsVisible = Value
        End Set
    End Property
    'PN15774
    Public Property WorkflowInformation() As String
        Get
            Return m_sWorkflowInformation
        End Get
        Set(ByVal Value As String)
            m_sWorkflowInformation = Value
        End Set
    End Property


    Public Property SourceID() As Integer
        Get
            Return m_iSourceID
        End Get
        Set(ByVal Value As Integer)
            m_iSourceID = Value
        End Set
    End Property
    'PN15774End
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
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel



            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            If disposing Then
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: Default
    '
    ' Description: Set Property Defaults.
    '
    '
    ' ***************************************************************** '
    Public Sub Default_Renamed()

        Try

            PMWrkTaskInstanceCnt = 0
            DateCreated = DateTime.Now
            CreatedByID = m_iUserID

            LastModified = Nothing

            ModifiedByID = Nothing
            TaskStatus = gPMConstants.PMEWrkManTaskStatus.pmeWMTSNew

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DefaultFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="Default", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: Add (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Public Function Add() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' CTAF151200 - Moved Output Param before Input Params

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Add PrimaryKey as OUTPUT parameters
                m_lReturn = CType(AddKeyOutputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add the required INPUT parameters
                m_lReturn = CType(AddInputParam(bLimitBySource:=True), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=If(g_bInstance, ACAddSQLInstance, ACAddSQL), sSQLName:=ACAddName, bStoredProcedure:=ACAddStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Get the Primary Key of the record inserted
                m_lReturn = CType(GetNewPrimaryKeyID(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description: Updates a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Add PrimaryKey as INPUT parameters
                m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add the required INPUT parameters
                m_lReturn = CType(AddInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored, lRecordsAffected:=lRecordsAffected)

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

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Delete (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    ' ***************************************************************** '
    Public Function Delete() As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Add PrimaryKey as INPUT parameters
                m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If record wasn't deleted, error
                If lRecordsAffected > 0 Then
                    ' Deleted, No action required
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectSingle (Public)
    '
    ' Description: Selects the required PMWrkTaskInstance
    '
    ' ***************************************************************** '
    Public Function SelectSingle(Optional ByRef vLockMode As gPMConstants.PMELockMode = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Default to No Lock if not supplied or not numeric
                Dim dbNumericTemp As Double

                If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                    vLockMode = gPMConstants.PMELockMode.PMNoLock
                End If

                ' Add PrimaryKey as INPUT parameters
                m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=ACSelectSingleSQL, sSQLName:=ACSelectSingleName, bStoredProcedure:=ACSelectSingleStored, bKeepNulls:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' How many records were selected
                lRecordCount = .Records.Count()

                ' Do we have any records ?
                If lRecordCount = 1 Then
                    ' Selected, No action required
                Else
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

                ' Set properties
                m_lReturn = CType(SetPropertiesFromDB(oFields:= .Records.Item(1).Fields()), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectSingle Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectSingle", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: SetPropertiesFromDB (Public)
    '
    ' Description: Sets the supplied PMWrkTaskInstance properties from a database
    '              record.
    ' ***************************************************************** '
    'Developer Guie No 21
    Private Function SetPropertiesFromDB(ByRef oFields As DataRow) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Populate Base Details

        With oFields

            PMWrkTaskInstanceCnt = gPMFunctions.NullToLong(oFields("pmwrk_task_instance_temp_cnt"))
            PmwrkTaskGroupID = gPMFunctions.NullToLong(oFields("pmwrk_task_group_id"))
            PmwrkTaskID = gPMFunctions.NullToLong(oFields("pmwrk_task_id"))

            'Developer guie No 44
            If Convert.IsDBNull(oFields("customer")) OrElse Informations.IsNothing(oFields("customer")) Then
                Customer = ""
            Else
                'developer guide no 145. 
                Customer = oFields("customer").Value
            End If
            TaskDueDate = gPMFunctions.NullToDate(oFields("task_due_date"))
            PmuserGroupID = gPMFunctions.NullToLong(oFields("pmuser_group_id"))


            'Developer guie No 44
            If Convert.IsDBNull(oFields("user_id")) OrElse Informations.IsNothing(oFields("user_id")) Then
                UserID = 0
            Else
                'developer guide no 145. 
                UserID = oFields("user_id").Value
            End If


            'Developer guie No 44
            If Convert.IsDBNull(oFields("description")) OrElse Informations.IsNothing(oFields("description")) Then
                Description = ""
            Else
                'developer guide no 145.
                Description = oFields("description").Value
            End If
            TaskStatus = gPMFunctions.NullToLong(oFields("task_status"))
            IsUrgent = gPMFunctions.NullToLong(oFields("is_urgent"))
            DateCreated = gPMFunctions.NullToDate(oFields("date_created"))
            CreatedByID = gPMFunctions.NullToLong(oFields("created_by_id"))

            'Developer guie No 44
            If Convert.IsDBNull(oFields("last_modified")) OrElse Informations.IsNothing(oFields("last_modified")) Then
                LastModified = DateTime.Now
            Else
                'developer guide no 145.
                LastModified = oFields("last_modified").Value
            End If


            'Developer guie No 44
            If Convert.IsDBNull(oFields("modified_by_id")) OrElse Informations.IsNothing(oFields("modified_by_id")) Then
                ModifiedByID = 0
            Else
                'developer guide no 145.
                ModifiedByID = oFields("modified_by_id").Value
            End If
            'DAK080200
            IsVisible = gPMFunctions.NullToLong(oFields("is_visible"))

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddInputParam (Private)
    '
    ' Description: Adds all of the NON-KEY INPUT parameters
    '              required for an Insert or Update.
    '
    ' ***************************************************************** '
    Private Function AddInputParam(Optional ByVal bLimitBySource As Boolean = False) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' CTAF 151200 - Re-ordered paramters to be the same as the SP

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="pmwrk_task_group_id", vValue:=CStr(PmwrkTaskGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="pmwrk_task_id", vValue:=CStr(PmwrkTaskID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Customer = "" Then

                'developer guide no. 85
                m_lReturn = .Parameters.Add(sName:="customer", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="customer", vValue:=Customer, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="task_due_date", vValue:=TaskDueDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="pmuser_group_id", vValue:=CStr(PmuserGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If (UserID < 1) Or (Convert.IsDBNull(UserID) Or Informations.IsNothing(UserID)) Then

                'developer guide no. 85
                m_lReturn = .Parameters.Add(sName:="user_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="user_id", vValue:=CStr(UserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Description = "" Then

                'developer guide no. 85
                m_lReturn = .Parameters.Add(sName:="description", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="description", vValue:=Description, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="task_status", vValue:=CStr(TaskStatus), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="is_urgent", vValue:=CStr(IsUrgent), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="date_created", vValue:=DateCreated, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="created_by_id", vValue:=CStr(CreatedByID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If (ModifiedByID < 1) Or (Convert.IsDBNull(ModifiedByID) Or Informations.IsNothing(ModifiedByID)) Then


                'developer guide no. 85
                m_lReturn = .Parameters.Add(sName:="last_modified", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                'developer guide no. 85
                m_lReturn = .Parameters.Add(sName:="modified_by_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else
                m_lReturn = .Parameters.Add(sName:="last_modified", vValue:=LastModified, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="modified_by_id", vValue:=CStr(ModifiedByID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            'DAK141299
            m_lReturn = .Parameters.Add(sName:="is_visible", vValue:=CStr(IsVisible), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'PN15774
            If WorkflowInformation = "" Then

                'developer guide no. 85
                m_lReturn = .Parameters.Add(sName:="workflow_information", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="workflow_information", vValue:=WorkflowInformation, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'PN15774End
            If SourceID > 0 And bLimitBySource And g_bInstance Then
                m_lReturn = .Parameters.Add(sName:="source_id", vValue:=CStr(SourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddKeyInputParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY INPUT parameters
    '              required for a Select, Delete or Update.
    '
    ' ***************************************************************** '
    Private Function AddKeyInputParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="pmwrk_task_instance_temp_cnt", vValue:=CStr(PMWrkTaskInstanceCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddKeyOutputParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY OUTPUT parameters
    '              required for an Add.
    '
    ' ***************************************************************** '
    Private Function AddKeyOutputParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase
            If g_bInstance Then
                m_lReturn = .Parameters.Add(sName:="pmwrk_task_instance_cnt", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="pmwrk_task_instance_temp_cnt", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If
        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetNewPrimaryKeyID (Private)
    '
    ' Description: Returns the new PRIMARY KEY values from an Add.
    '
    ' ***************************************************************** '
    Private Function GetNewPrimaryKeyID() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            If g_bInstance Then
                PMWrkTaskInstanceCnt = .Parameters.Item("pmwrk_task_instance_cnt").Value
            Else
                PMWrkTaskInstanceCnt = .Parameters.Item("pmwrk_task_instance_temp_cnt").Value
            End If
        End With

        Return result

    End Function
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Private Shared _DefaultInstance As PMWrkInstance = Nothing
    Public Shared ReadOnly Property DefaultInstance() As PMWrkInstance
        Get
            If _DefaultInstance Is Nothing Then
                _DefaultInstance = New PMWrkInstance
            End If
            Return _DefaultInstance
        End Get
    End Property
End Class