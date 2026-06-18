Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports SSP.Shared
'Developer Guide No. 129
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 24th October 1996
    '
    ' Description: Creatable Business class which contains all the
    '              methods, business rules required to manipulate
    '              a PMTask.
    '
    ' Edit History:
    ' DAK200999 - New field, DisplayIcon, on PMWrkTask table.
    ' DAK041099 - New fields for view only task, tasks linked to objects,
    '             and whether task can be run directly from available tasks
    '             bar.
    ' DAK231199 - Check PMProduct lookup for permissions
    ' DAK291199 - bKeepNulls set to true when retreiving data
    ' DAK221299 - Add PMWrk_Task_Category
    '             PMProductLookup.GetDetails - PMProductID changed to
    '             variant.
    ' DAK220600 - Add new function to retrieve details by task code
    ' RAW 14/02/2003 : ISS2153 : added new nav_xml_file column
    ' ***************************************************************** '


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


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Collection of PMTasks (Private)
    Private m_oPMTasks As bPMTask.PMTasks

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer (Private)
    Private m_lCurrentRecord As gPMConstants.PMEReturnCode

    ' Error Code (Private)
    Private m_lError As gPMConstants.PMEReturnCode

    ' Caption lookup
    Private m_oCaption As bPMCaption.Business
    'Private m_oCaption As bPMCaption.Business

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture

        End Get
    End Property


    Public Property CurrentRecord() As Integer
        Get

            Return m_lCurrentRecord

        End Get
        Set(ByVal Value As Integer)

            If Value < 0 Then
                m_lCurrentRecord = gPMConstants.PMEReturnCode.PMFalse
            ElseIf (Value > m_oPMTasks.Count()) Then
                m_lCurrentRecord = m_oPMTasks.Count()
            Else
                m_lCurrentRecord = Value
            End If

        End Set
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

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
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


            ' Initialisation Code.

            ' Set Username and Password

            ' Set User ID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level


            If Informations.IsNothing(vDatabase) Then
                lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            Else

                lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            End If

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oCaption = New bPMCaption.Business
            lReturn = m_oCaption.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'create empty tasks collection
            m_oPMTasks = New bPMTask.PMTasks()

            ' Set Current Record to zero
            m_lCurrentRecord = gPMConstants.PMEReturnCode.PMFalse

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
            Me.disposedValue = True
            If disposing Then
                If m_oCaption IsNot Nothing Then
                    m_oCaption.Dispose()
                    m_oCaption = Nothing
                End If
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: Add (Public)
    '
    ' Description: Adds a single PMTask directly into the database.
    '              Note: The PMTask will NOT be added to the collection.
    '
    ' DAK200999 - New column, DisplayIcon, on PMWrkTask Table
    ' DAK041099 - More new columns
    ' DAK221299 - Add PMWrk_Task_Category
    ' RAW 14/02/2003 : ISS2153 : added NavXMLFile
    ' ***************************************************************** '
    Public Function Add(ByRef lTaskID As Integer, ByRef lCaptionID As Integer, ByRef sCode As String, ByRef sDescription As String, ByRef iIsDeleted As Integer, ByRef dtEffectiveDate As Date, ByRef iIsSystemTask As Integer, ByRef iTypeOfTask As Integer, ByRef lPMNavProcessId As Integer, ByRef sComponentObjectName As String, ByRef sComponentClassName As String, ByRef lAutoDeleteAfterNumDays As Integer, ByRef lDisplayIcon As Integer, ByRef iIsViewOnlyTask As Integer, ByRef sLinkedObjectName As String, ByRef sLinkedClassName As String, ByRef sLinkedCaption As String, ByRef iIsAvailableTask As Integer, ByRef lTaskCategoryID As Integer, Optional ByRef sNavXMLFile As String = "") As Integer

        Dim result As Integer = 0
        Dim oPMTask As bPMTask.PMTask


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new PMTask
            oPMTask = New bPMTask.PMTask()

            ' Populate PMTask Attributes
            'DAK221299
            ' RAW 14/02/2003 : ISS2153 : added NavXMLFile
            m_lError = CType(SetProperties(oPMTask:=oPMTask, iStatus:=gPMConstants.PMEComponentAction.PMAdd, vTaskId:=lTaskID, vCaptionID:=lCaptionID, vCode:=sCode, vDescription:=sDescription, vIsDeleted:=iIsDeleted, vEffectiveDate:=dtEffectiveDate, vIsSystemTask:=iIsSystemTask, vTypeOfTask:=iTypeOfTask, vPMNavProcessId:=lPMNavProcessId, vComponentObjectName:=sComponentObjectName, vComponentClassName:=sComponentClassName, vAutoDeleteAfterNumDays:=lAutoDeleteAfterNumDays, vDisplayIcon:=lDisplayIcon, vIsViewOnlyTask:=iIsViewOnlyTask, vLinkedObjectName:=sLinkedObjectName, vLinkedClassName:=sLinkedClassName, vLinkedCaption:=sLinkedCaption, vIsAvailableTask:=iIsAvailableTask, vTaskCategoryID:=lTaskCategoryID, vNavXMLFile:=sNavXMLFile), gPMConstants.PMEReturnCode)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the PMTask to the Database
            m_lError = CType(AddItem(oPMTask), gPMConstants.PMEReturnCode)
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the ID of the PMTask Added
            lTaskID = oPMTask.TaskID

            oPMTask = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required PMTasks and populate the Collection
    ' for the UserId if supplied, else returns all of the users
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vTaskId As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oPMTask As bPMTask.PMTask

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Check that we do not have any outstanding changes.
            m_lError = CType(Cancel(), gPMConstants.PMEReturnCode)

            ' If changes are outstanding, exit.
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lError
            End If

            ' Clear the Collection
            m_oPMTasks.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = gPMConstants.PMEReturnCode.PMFalse

            'if the userid was supplied

            If Not Informations.IsNothing(vTaskId) Then

                ' If the supplied keys are not valid, exit

                Dim dbNumericTemp As Double
                If Not Double.TryParse(CStr(vTaskId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : MessageID=" & CStr(vTaskId), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                    Return result

                End If

                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                ' Add the UserId parameter (INPUT)

                m_lError = m_oDatabase.Parameters.Add(sName:="pmwrk_task_id", vValue:=CStr(vTaskId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'DAK291199
                ' Execute SQL Statement
                m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetDetailsSQL, sSQLName:=ACGetDetailsName, bStoredProcedure:=ACGetDetailsStored, lNumberRecords:=0, bKeepNulls:=True)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else
                ' select all of the PMTask records

                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'DAK291199
                ' Execute SQL Statement
                m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0, bKeepNulls:=True)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?

            If lRecordCount < 1 Then

                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMFalse

            Else

                ' Yes, load them into the collection

                For lSub As Integer = 1 To lRecordCount

                    ' Create New PMTask
                    oPMTask = New bPMTask.PMTask()

                    m_lError = CType(SetPropertiesFromDB(oPMTask:=oPMTask, lRecordNumber:=lSub), gPMConstants.PMEReturnCode)
                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Add PMTask to collection
                    'If (m_oPMTasks.Count = 0) Then
                    '    m_oPMTasks.Add(New bPMTask.PMTask())
                    'End If
                    If (m_oPMTasks.Count = 0) Then
                        m_oPMTasks.Add(Nothing)
                    End If
                    m_lError = CType(m_oPMTasks.Add(oNewPMTask:=oPMTask), gPMConstants.PMEReturnCode)
                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oPMTask = Nothing

                Next lSub

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required PMTasks and populate the Collection
    '
    ' DAK200999 - New column, DisplayIcon, on PMWrkTask Table
    ' DAK041099 - More new columns
    ' DAK221299 - Add PMWrk_Task_Category
    ' RAW 14/02/2003 : ISS2153 : added NavXMLFile
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vTaskId As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vIsSystemTask As Object = Nothing, Optional ByRef vTypeOfTask As Object = Nothing, Optional ByRef vPMNavProcessId As Object = Nothing, Optional ByRef vComponentObjectName As Object = Nothing, Optional ByRef vComponentClassName As Object = Nothing, Optional ByRef vAutoDeleteAfterNumDays As Object = Nothing, Optional ByRef vDisplayIcon As Object = Nothing, Optional ByRef vIsViewOnlyTask As Object = Nothing, Optional ByRef vLinkedObjectName As Object = Nothing, Optional ByRef vLinkedClassName As Object = Nothing, Optional ByRef vLinkedCaption As Object = Nothing, Optional ByRef vIsAvailableTask As Object = Nothing, Optional ByRef vTaskCategoryID As Object = Nothing, Optional ByRef vNavXMLFile As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oPMTask As bPMTask.PMTask
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oPMTasks.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord = CType(m_lCurrentRecord + 1, gPMConstants.PMEReturnCode)
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oPMTask = m_oPMTasks.Item(m_lCurrentRecord)

            ' Get the PMTask Property Values
            'DAK221299
            ' RAW 14/02/2003 : ISS2153 : added NavXMLFile


            'm_lError = CType(GetProperties(oPMTask, iStatus, CInt(vTaskId), CInt(vCaptionID), CStr(vCode), CStr(vDescription), CInt(vIsDeleted), CDate(vEffectiveDate), CInt(vIsSystemTask), CInt(vTypeOfTask), CInt(vPMNavProcessId), CStr(vComponentObjectName), CStr(vComponentClassName), CInt(vAutoDeleteAfterNumDays), CInt(vDisplayIcon), CInt(vIsViewOnlyTask), CStr(vLinkedObjectName), CStr(vLinkedClassName), CStr(vLinkedCaption), CInt(vIsAvailableTask), CInt(vTaskCategoryID), CStr(vNavXMLFile)), gPMConstants.PMEReturnCode)
            'm_lError = CType(GetProperties(oPMTask, iStatus, Convert.ToInt32(vTaskId), Convert.ToInt32(vCaptionID), Convert.ToString(vCode), Convert.ToString(vDescription), vIsDeleted, Convert.ToDateTime(vEffectiveDate), Convert.ToInt32(vIsSystemTask), Convert.ToInt32(vTypeOfTask), Convert.ToInt32(vPMNavProcessId), Convert.ToString(vComponentObjectName), Convert.ToString(vComponentClassName), Convert.ToInt32(vAutoDeleteAfterNumDays), Convert.ToInt32(vDisplayIcon), Convert.ToInt32(vIsViewOnlyTask), Convert.ToString(vLinkedObjectName), Convert.ToString(vLinkedClassName), Convert.ToString(vLinkedCaption), Convert.ToInt32(vIsAvailableTask), Convert.ToInt32(vTaskCategoryID), Convert.ToString(vNavXMLFile)), gPMConstants.PMEReturnCode)
            m_lError = CType(GetProperties(oPMTask, iStatus, vTaskId, vCaptionID, vCode, vDescription, vIsDeleted, vEffectiveDate, vIsSystemTask, vTypeOfTask, vPMNavProcessId, vComponentObjectName, vComponentClassName, vAutoDeleteAfterNumDays, vDisplayIcon, vIsViewOnlyTask, vLinkedObjectName, vLinkedClassName, vLinkedCaption, vIsAvailableTask, vTaskCategoryID, vNavXMLFile), gPMConstants.PMEReturnCode)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oPMTask = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied PMTask into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' DAK200999 - New column, DisplayIcon, on PMWrkTask Table
    ' DAK041099 - More new columns
    ' DAK221299 - Add PMWrk_Task_Category
    ' RAW 14/02/2003 : ISS2153 : added NavXMLFile
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vTaskId As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vIsSystemTask As Object = Nothing, Optional ByRef vTypeOfTask As Object = Nothing, Optional ByRef vPMNavProcessId As Object = Nothing, Optional ByRef vComponentObjectName As Object = Nothing, Optional ByRef vComponentClassName As Object = Nothing, Optional ByRef vAutoDeleteAfterNumDays As Object = Nothing, Optional ByRef vDisplayIcon As Object = Nothing, Optional ByRef vIsViewOnlyTask As Object = Nothing, Optional ByRef vLinkedObjectName As Object = Nothing, Optional ByRef vLinkedClassName As Object = Nothing, Optional ByRef vLinkedCaption As Object = Nothing, Optional ByRef vIsAvailableTask As Object = Nothing, Optional ByRef vTaskCategoryID As Object = Nothing, Optional ByRef vNavXMLFile As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oPMTask As bPMTask.PMTask

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oPMTasks.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new PMTask
            oPMTask = New bPMTask.PMTask()

            ' Populate PMTask Attributes
            'DAK221299
            ' RAW 14/02/2003 : ISS2153 : added NavXMLFile

            'developer guide no.98
            m_lError = CType(SetProperties(oPMTask:=oPMTask, iStatus:=gPMConstants.PMEComponentAction.PMAdd, vTaskId:=vTaskId, vCaptionID:=vCaptionID, vCode:=vCode, vDescription:=vDescription, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vIsSystemTask:=vIsSystemTask, vTypeOfTask:=vTypeOfTask, vPMNavProcessId:=vPMNavProcessId, vComponentObjectName:=vComponentObjectName, vComponentClassName:=vComponentClassName, vAutoDeleteAfterNumDays:=vAutoDeleteAfterNumDays, vDisplayIcon:=vDisplayIcon, vIsViewOnlyTask:=vIsViewOnlyTask, vLinkedObjectName:=vLinkedObjectName, vLinkedClassName:=vLinkedClassName, vLinkedCaption:=vLinkedCaption, vIsAvailableTask:=vIsAvailableTask, vTaskCategoryID:=vTaskCategoryID, vNavXMLFile:=vNavXMLFile), gPMConstants.PMEReturnCode)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add PMTask to collection
            If (m_oPMTasks.Count = 0) Then
                m_oPMTasks.Add(Nothing)
            End If
            m_lError = CType(m_oPMTasks.Add(oNewPMTask:=oPMTask), gPMConstants.PMEReturnCode)
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oPMTask = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the PMTask
    '              specified and updates the PMTask with the new values.
    '
    ' DAK200999 - New column, DisplayIcon, on PMWrkTask Table
    ' DAK041099 - More new columns
    ' DAK221299 - Add PMWrk_Task_Category
    ' RAW 14/02/2003 : ISS2153 : added NavXMLFile
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vTaskId As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vIsSystemTask As Object = Nothing, Optional ByRef vTypeOfTask As Object = Nothing, Optional ByRef vPMNavProcessId As Object = Nothing, Optional ByRef vComponentObjectName As Object = Nothing, Optional ByRef vComponentClassName As Object = Nothing, Optional ByRef vAutoDeleteAfterNumDays As Object = Nothing, Optional ByRef vDisplayIcon As Object = Nothing, Optional ByRef vIsViewOnlyTask As Object = Nothing, Optional ByRef vLinkedObjectName As Object = Nothing, Optional ByRef vLinkedClassName As Object = Nothing, Optional ByRef vLinkedCaption As Object = Nothing, Optional ByRef vIsAvailableTask As Object = Nothing, Optional ByRef vTaskCategoryID As Object = Nothing, Optional ByRef vNavXMLFile As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oPMTask As bPMTask.PMTask
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oPMTasks.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oPMTask = m_oPMTasks.Item(lRow)

            ' Check the Status of the PMTask

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oPMTask.DatabaseStatus
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Leave Status as Add
                    iStatus = gPMConstants.PMEComponentAction.PMAdd
                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                    ' Error
                    result = gPMConstants.PMEReturnCode.PMFalse
                Case Else
                    ' Set Edit (Update) Status
                    iStatus = gPMConstants.PMEComponentAction.PMEdit
            End Select

            ' Update PMTask Attributes
            'DAK221299
            ' RAW 14/02/2003 : ISS2153 : added NavXMLFile

            'developer guide no.98
            m_lError = CType(SetProperties(oPMTask:=oPMTask, iStatus:=iStatus, vTaskId:=vTaskId, vCaptionID:=vCaptionID, vCode:=vCode, vDescription:=vDescription, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vIsSystemTask:=vIsSystemTask, vTypeOfTask:=vTypeOfTask, vPMNavProcessId:=vPMNavProcessId, vComponentObjectName:=vComponentObjectName, vComponentClassName:=vComponentClassName, vAutoDeleteAfterNumDays:=vAutoDeleteAfterNumDays, vDisplayIcon:=vDisplayIcon, vIsViewOnlyTask:=vIsViewOnlyTask, vLinkedObjectName:=vLinkedObjectName, vLinkedClassName:=vLinkedClassName, vLinkedCaption:=vLinkedCaption, vIsAvailableTask:=vIsAvailableTask, vTaskCategoryID:=vTaskCategoryID, vNavXMLFile:=vNavXMLFile), gPMConstants.PMEReturnCode)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Release reference to PMTask
            oPMTask = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified PMTask can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oPMTask As bPMTask.PMTask

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oPMTasks.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oPMTask = m_oPMTasks.Item(lRow)

            ' Check the Status of the PMTask

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oPMTask.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oPMTask.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                'set deleted status to true
                oPMTask.IsDeleted = gPMConstants.PMEVarTrueFalse.PMVarTrue

                'set database status to update
                oPMTask.DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit
            End If

            ' Release reference to PMTask
            oPMTask = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Cancel (Public)
    '
    ' Description: Checks the Collection to see if Cancel is OK.
    '              i.e. Do we need any Adding, Deleting or Updating.
    '
    '              Returns PMTrue if all items are clean
    '                      (PMView or PMDummyDelete)
    '              Otherwise returns PMDataChanged.
    ' ***************************************************************** '
    Public Function Cancel() As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Loop round Collection
            For lSub As Integer = 1 To m_oPMTasks.Count()
                Select Case m_oPMTasks.Item(lSub).DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing
                    Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
                        result = gPMConstants.PMEReturnCode.PMDataChanged
                        Exit For
                End Select
            Next lSub

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Cancel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description: Loops round the collection, doing any required
    '              Adds, Deletes or Updates.
    '
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim oPMTask As bPMTask.PMTask
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub As Integer = 1 To m_oPMTasks.Count()
                oPMTask = m_oPMTasks.Item(lSub)


                Select Case oPMTask.DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing

                    Case gPMConstants.PMEComponentAction.PMAdd

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lError = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Add Item
                        m_lError = CType(AddItem(oPMTask), gPMConstants.PMEReturnCode)
                        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMEdit

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lError = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Update Item
                        m_lError = CType(UpdateItem(oPMTask), gPMConstants.PMEReturnCode)
                        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMDelete

                        'do nothing because in this case the isdeleted flag is set to true
                        'and the row is simply updated

                End Select

            Next lSub

            ' Release last reference
            oPMTask = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lError = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Set all items in the collection to PMView
                    For lSub As Integer = 1 To m_oPMTasks.Count()
                        m_oPMTasks.Item(lSub).DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                    Next lSub

                Else

                    m_lError = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetDataByCode
    '
    ' Description: Returns task data for a given task code
    '
    ' History: 22/06/2000 DAK - Created.
    '
    ' RAW 14/02/2003 : ISS2153 : added NavXMLFile
    ' ***************************************************************** '
    Public Function GetDataByCode(ByVal v_sPMWrkTaskCode As String, Optional ByRef r_vTaskId As Object = Nothing, Optional ByRef r_vCaptionID As Object = Nothing, Optional ByRef r_vDescription As Object = Nothing, Optional ByRef r_vIsDeleted As Object = Nothing, Optional ByRef r_vEffectiveDate As Object = Nothing, Optional ByRef r_vIsSystemTask As Object = Nothing, Optional ByRef r_vTypeOfTask As Object = Nothing, Optional ByRef r_vPMNavProcessId As Object = Nothing, Optional ByRef r_vComponentObjectName As Object = Nothing, Optional ByRef r_vComponentClassName As Object = Nothing, Optional ByRef r_vAutoDeleteAfterNumDays As Object = Nothing, Optional ByRef r_vDisplayIcon As Object = Nothing, Optional ByRef r_vIsViewOnlyTask As Object = Nothing, Optional ByRef r_vLinkedObjectName As Object = Nothing, Optional ByRef r_vLinkedClassName As Object = Nothing, Optional ByRef r_vLinkedCaption As Object = Nothing, Optional ByRef r_vIsAvailableTask As Object = Nothing, Optional ByRef r_vTaskCategoryID As Object = Nothing, Optional ByRef r_vNavXMLFile As Object = Nothing) As Integer
        Dim result As Integer = 0
        Dim oPMTask As PMTask = Nothing
        Dim iStatus As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If we haven't got any tasks - get them from the database
            If m_oPMTasks Is Nothing Or m_oPMTasks.Count() = 0 Then

                m_lError = CType(GetDetails(), gPMConstants.PMEReturnCode)
                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lError
                End If

            End If

            ' Select the task from the collection using the task code
            For lSub As Integer = 1 To m_oPMTasks.Count()

                oPMTask = m_oPMTasks.Item(lSub)
                If oPMTask.TaskCode.Trim() = v_sPMWrkTaskCode.Trim() Then
                    Exit For
                End If

                oPMTask = Nothing

            Next lSub

            ' No task found so return false
            If oPMTask Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the requested data
            ' RAW 14/02/2003 : ISS2153 : added NavXMLFile

            'developer guide no.98
            m_lError = CType(GetProperties(oPMTask:=oPMTask, vStatus:=iStatus, vTaskId:=r_vTaskId, vCaptionID:=r_vCaptionID, vDescription:=r_vDescription, vIsDeleted:=r_vIsDeleted, vEffectiveDate:=r_vEffectiveDate, vIsSystemTask:=r_vIsSystemTask, vTypeOfTask:=r_vTypeOfTask, vPMNavProcessId:=r_vPMNavProcessId, vComponentObjectName:=r_vComponentObjectName, vComponentClassName:=r_vComponentClassName, vAutoDeleteAfterNumDays:=r_vAutoDeleteAfterNumDays, vDisplayIcon:=r_vDisplayIcon, vIsViewOnlyTask:=r_vIsViewOnlyTask, vLinkedObjectName:=r_vLinkedObjectName, vLinkedClassName:=r_vLinkedClassName, vLinkedCaption:=r_vLinkedCaption, vIsAvailableTask:=r_vIsAvailableTask, vTaskCategoryID:=r_vTaskCategoryID, vNavXMLFile:=r_vNavXMLFile), gPMConstants.PMEReturnCode)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lError
            End If

            oPMTask = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDataByCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDataByCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: AddItem (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Private Function AddItem(ByRef oPMTask As bPMTask.PMTask) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters
        m_lError = CType(AddInputParam(oPMTask:=oPMTask), gPMConstants.PMEReturnCode)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lError = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateItem (Public)
    '
    ' Description: UpdateItems a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Private Function UpdateItem(ByRef oPMTask As bPMTask.PMTask) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the Input Parameters to the Parameters collection
        m_lError = m_oDatabase.Parameters.Add(sName:="pmwrk_task_id", vValue:=CStr(oPMTask.TaskID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' now for the rest of the parameters
        m_lError = CType(AddInputParam(oPMTask:=oPMTask), gPMConstants.PMEReturnCode)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lError = m_oDatabase.SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored, lRecordsAffected:=lRecordsAffected)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' If the record was NOT UpdateItemd reselect it to see if the data
        ' has been changed or the record deleted

        If lRecordsAffected > 0 Then

            ' UpdatedItem, No action required

        Else

            result = gPMConstants.PMEReturnCode.PMFalse

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DeleteItem (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (DeleteItem) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function DeleteItem(ByRef oPMTask As bPMTask.PMTask) As Integer
    '
    'Dim result As Integer = 0
    'Dim lRecordsAffected As Integer
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Clear the Database Parameters Collection
    'm_oDatabase.Parameters.Clear()
    '
    ' Add the InsuranceFileID INPUT parameter
    'm_lError = m_oDatabase.Parameters.Add(sName:="pmwrk_task_id", vValue:=CStr(oPMTask.TaskID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
    '
    'If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' Execute SQL Statement
    'm_lError = m_oDatabase.SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)
    '
    'If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' If record wasn't deleted, error
    'If lRecordsAffected > 0 Then
    ' Deleted, No action required
    'Else
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    '
    ' Name: GetPrivilegeLevel
    '
    ' Description:
    '
    ' History: 10/11/1999 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function GetPrivilegeLevel(ByRef r_iPrivilegeLevel As Integer) As Integer
        Dim result As Integer = 0
        Dim lPMProductID As Integer
        ' RDC 13062002 CompServ replaced with BAS module
        'Dim oCS As sPMServerCS.PMServerBusinessCS
        Dim oPMProductLookup As bPMProductLookup.Business


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lError = CType(GetProductID(lPMProductID), gPMConstants.PMEReturnCode)
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create the User Group Component to do this work.
            '    Set oCS = New sPMServerCS.PMServerBusinessCS
            '    m_lError = oCS.CreateBusinessObject( _
            'r_oObject:=oPMProductLookup, _
            'v_sClassName:="bPMProductLookup.Business", _
            'v_sCallingAppName:=m_sCallingAppName, _
            'v_sUserName:=m_sUsername, _
            'v_sPassword:=m_sPassword, _
            'v_iUserID:=m_iUserID, _
            'v_iSourceID:=m_iSourceID, _
            'v_iLanguageId:=m_iLanguageID, _
            'v_iCurrencyID:=m_iCurrencyID, _
            'v_iLogLevel:=m_iLogLevel, _
            'v_oDatabase:=m_oDatabase)

            oPMProductLookup = New bPMProductLookup.Business
            m_lError = oPMProductLookup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            '    Set oCS = Nothing
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DAK221299

            m_lError = oPMProductLookup.GetDetails(vPMProductId:=lPMProductID, vTableName:="PMWrk_Task")
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then

                oPMProductLookup.Dispose()
                oPMProductLookup = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lError = oPMProductLookup.GetNext(vPrivilegeLevel:=r_iPrivilegeLevel)

            oPMProductLookup.Dispose()
            oPMProductLookup = Nothing
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPrivilegeLevel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPrivilegeLevel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetProductID
    '
    ' Description:
    '
    ' History: 11/11/1999 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function GetProductID(ByRef r_lPMProductID As Integer) As Integer
        ' RDC 13062002 CompServ repplaced with BAS module
        'Dim oCS As sPMServerCS.PMServerBusinessCS
        Dim result As Integer = 0
        Dim oPMProduct As bPMProduct.Business


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the User Group Component to do this work.
            '    Set oCS = New sPMServerCS.PMServerBusinessCS
            '    m_lError = oCS.CreateBusinessObject( _
            'r_oObject:=oPMProduct, _
            'v_sClassName:="bPMProduct.Business", _
            'v_sCallingAppName:=m_sCallingAppName, _
            'v_sUserName:=m_sUsername, _
            'v_sPassword:=m_sPassword, _
            'v_iUserID:=m_iUserID, _
            'v_iSourceID:=m_iSourceID, _
            'v_iLanguageId:=m_iLanguageID, _
            'v_iCurrencyID:=m_iCurrencyID, _
            'v_iLogLevel:=m_iLogLevel, _
            'v_oDatabase:=m_oDatabase)

            oPMProduct = New bPMProduct.Business
            m_lError = oPMProduct.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            '    Set oCS = Nothing
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lError = oPMProduct.GetProductID(v_sProductCode:="Sirius", r_lPMProductID:=r_lPMProductID)

            oPMProduct.Dispose()
            oPMProduct = Nothing
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProductID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProductID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetUserAuthority
    '
    ' Description: Gets the User Groups that the current user is a
    '              Supervisor of.
    '
    ' ***************************************************************** '
    Public Function GetUserAuthority(ByRef r_bIsAdministrator As Boolean, ByRef r_vSupervisedGroups As Object) As Integer

        Dim result As Integer = 0
        Dim oUserGroup As bPMUserGroup.Utilities
        ' RDC 13062002 CompServ replaced with BAS module
        'Dim oCS As sPMServerCS.PMServerBusinessCS

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the User Group Component to do this work.
            '    Set oCS = New sPMServerCS.PMServerBusinessCS
            '     m_lError = oCS.CreateBusinessObject( _
            'r_oObject:=oUserGroup, _
            'v_sClassName:="bPMUserGroup.Utilities", _
            'v_sCallingAppName:=m_sCallingAppName, _
            'v_sUserName:=m_sUsername, _
            'v_sPassword:=m_sPassword, _
            'v_iUserID:=m_iUserID, _
            'v_iSourceID:=m_iSourceID, _
            'v_iLanguageId:=m_iLanguageID, _
            'v_iCurrencyID:=m_iCurrencyID, _
            'v_iLogLevel:=m_iLogLevel, _
            'v_oDatabase:=m_oDatabase)

            oUserGroup = New bPMUserGroup.Utilities
            m_lError = oUserGroup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            '    Set oCS = Nothing
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Is the User an Administrator

            m_lError = oUserGroup.IsUserAdministrator(v_iUserID:=m_iUserID, r_bIsAdministrator:=r_bIsAdministrator)
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                oUserGroup.Dispose()
                oUserGroup = Nothing
                Return result
            End If

            ' Get the Groups they Supervise

            m_lError = oUserGroup.GetGroupsSupervisedByUser(v_iUserID:=m_iUserID, r_vSupervisedGroups:=r_vSupervisedGroups)
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                oUserGroup.Dispose()
                oUserGroup = Nothing
                Return result
            End If

            ' Terminate

            oUserGroup.Dispose()
            oUserGroup = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserAuthorityFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserAuthority", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetPropertiesFromDB (Private)
    '
    ' Description: Sets the supplied PMTask properties from a database
    '              record.
    ' DAK200999 - New column, DisplayIcon, on PMWrkTask Table
    ' DAK041099 - More new columns
    ' ***************************************************************** '
    Private Function SetPropertiesFromDB(ByRef oPMTask As bPMTask.PMTask, ByRef lRecordNumber As Integer) As Integer

        Dim result As Integer = 0
        Dim sDescription As String = ""
        ' RDC dPMDAO changes - new object referenced
        'Developer Guide No. 21
        Dim oFields As DataRow
        Dim lLinkedCaptionID As Integer
        Dim sLinkedCaption As String = ""




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        oFields = m_oDatabase.Records.Item(lRecordNumber - 1).Fields()

        ' Populate Base Details

        With oPMTask

            .TaskID = oFields("id")
            .CaptionID = oFields("caption_id")
            .TaskCode = oFields("code")

            'Developer Guide No. 8
            m_lError = m_oCaption.GetCaptionDesc(.CaptionID, sDescription)
            .Description = sDescription
            .IsDeleted = oFields("is_Deleted")
            .EffectiveDate = oFields("effective_date")
            .IsSystemTask = oFields("is_system_task")
            .TypeOfTask = oFields("type_of_task")

            If Convert.IsDBNull(oFields("pmnav_process_id")) Or Informations.IsNothing(oFields("pmnav_process_id")) Then
                .PMNavProcessId = -1
            Else
                .PMNavProcessId = oFields("pmnav_process_id")
            End If
            'DAK280700 - allow for null value in component object and class names

            If Convert.IsDBNull(oFields("component_object_name")) Or Informations.IsNothing(oFields("component_object_name")) Then
                .ComponentObjectName = ""
            Else
                .ComponentObjectName = oFields("component_object_name")
            End If

            If Convert.IsDBNull(oFields("component_class_name")) Or Informations.IsNothing(oFields("component_class_name")) Then
                .ComponentClassName = ""
            Else
                .ComponentClassName = oFields("component_class_name")
            End If

            If Convert.IsDBNull(oFields("auto_delete_after_num_days")) Or Informations.IsNothing(oFields("auto_delete_after_num_days")) Then
                .AutoDeleteAfterNumDays = -1
            Else
                .AutoDeleteAfterNumDays = oFields("auto_delete_after_num_days")
            End If
            .DatabaseStatus = gPMConstants.PMEComponentAction.PMView
            .DisplayIcon = oFields("display_icon")
            .IsViewOnlyTask = oFields("is_view_only_task")
            'DAK291199

            If Convert.IsDBNull(oFields("linked_caption_id")) Or Informations.IsNothing(oFields("linked_caption_id")) Then
                .LinkedCaption = ""
                .LinkedObjectName = ""
                .LinkedClassName = ""
            Else
                lLinkedCaptionID = oFields("linked_caption_id")

                m_lError = m_oCaption.GetCaptionDesc(v_lCaptionId:=lLinkedCaptionID, r_sCaption:=sLinkedCaption)
                .LinkedCaption = sLinkedCaption
                .LinkedObjectName = oFields("linked_object_name")
                .LinkedClassName = oFields("linked_class_name")
            End If
            .IsAvailableTask = oFields("is_available_task")
            'DAK221299
            .TaskCategoryID = oFields("pmwrk_task_category_id")
            'eck14092004

            If Convert.IsDBNull(oFields("pmnavxm_file")) Or Informations.IsNothing(oFields("pmnavxm_file")) Then
                .NavXMLFile = ""
            Else
                .NavXMLFile = oFields("pmnavxm_file")
            End If
        End With

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied PMTask property values.
    '
    ' DAK200999 - New column, DisplayIcon, on PMWrkTask Table
    ' DAK041099 - More new columns
    ' DAK221299 - Add PMWrk_Task_Category
    ' RAW 14/02/2003 : ISS2153 : added NavXMLFile
    ' ***************************************************************** '
    'developer guide no.101
    Private Function SetProperties(ByRef oPMTask As bPMTask.PMTask, ByRef iStatus As Integer, Optional ByRef vTaskId As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vIsSystemTask As Object = Nothing, Optional ByRef vTypeOfTask As Object = Nothing, Optional ByRef vPMNavProcessId As Object = Nothing, Optional ByRef vComponentObjectName As Object = Nothing, Optional ByRef vComponentClassName As Object = Nothing, Optional ByRef vAutoDeleteAfterNumDays As Object = Nothing, Optional ByRef vDisplayIcon As Object = Nothing, Optional ByRef vIsViewOnlyTask As Object = Nothing, Optional ByRef vLinkedObjectName As Object = Nothing, Optional ByRef vLinkedClassName As Object = Nothing, Optional ByRef vLinkedCaption As Object = Nothing, Optional ByRef vIsAvailableTask As Object = Nothing, Optional ByRef vTaskCategoryID As Object = Nothing, Optional ByRef vNavXMLFile As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sDescription As String = ""
        Dim lCaptionID As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        'decide whether call is an add or an edit
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            'decide whether all of the mandatory values have been supplied
            'DAK221299
            ' RAW 14/02/2003 : ISS2153 : added NavXMLFile
            m_lError = CType(MandatoryParameterCheck(vTaskId:=vTaskId, vCaptionID:=vCaptionID, vCode:=vCode, vDescription:=vDescription, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vIsSystemTask:=vIsSystemTask, vTypeOfTask:=vTypeOfTask, vPMNavProcessId:=vPMNavProcessId, vComponentObjectName:=vComponentObjectName, vComponentClassName:=vComponentClassName, vAutoDeleteAfterNumDays:=vAutoDeleteAfterNumDays, vDisplayIcon:=vDisplayIcon, vIsViewOnlyTask:=vIsViewOnlyTask, vLinkedObjectName:=vLinkedObjectName, vLinkedClassName:=vLinkedClassName, vLinkedCaption:=vLinkedCaption, vIsAvailableTask:=vIsAvailableTask, vTaskCategoryID:=vTaskCategoryID, vNavXMLFile:=vNavXMLFile), gPMConstants.PMEReturnCode)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'supply defaults for any missing parameters
            'DAK221299
            ' RAW 14/02/2003 : ISS2153 : added NavXMLFile
            m_lError = CType(DefaultMissingParameters(vTaskId:=vTaskId, vCaptionID:=vCaptionID, vCode:=vCode, vDescription:=vDescription, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vIsSystemTask:=vIsSystemTask, vTypeOfTask:=vTypeOfTask, vPMNavProcessId:=vPMNavProcessId, vComponentObjectName:=vComponentObjectName, vComponentClassName:=vComponentClassName, vAutoDeleteAfterNumDays:=vAutoDeleteAfterNumDays, vDisplayIcon:=vDisplayIcon, vIsViewOnlyTask:=vIsViewOnlyTask, vLinkedObjectName:=vLinkedObjectName, vLinkedClassName:=vLinkedClassName, vLinkedCaption:=vLinkedCaption, vIsAvailableTask:=vIsAvailableTask, vTaskCategoryID:=vTaskCategoryID, vNavXMLFile:=vNavXMLFile), gPMConstants.PMEReturnCode)

            If m_lError = gPMConstants.PMEReturnCode.PMFalse Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        'check whether the values in the parameters are valid
        'DAK221299
        ' RAW 14/02/2003 : ISS2153 : added NavXMLFile
        m_lError = CType(ValidateParameters(vTaskId:=vTaskId, vCaptionID:=vCaptionID, vCode:=vCode, vDescription:=vDescription, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vIsSystemTask:=vIsSystemTask, vTypeOfTask:=vTypeOfTask, vPMNavProcessId:=vPMNavProcessId, vComponentObjectName:=vComponentObjectName, vComponentClassName:=vComponentClassName, vAutoDeleteAfterNumDays:=vAutoDeleteAfterNumDays, vDisplayIcon:=vDisplayIcon, vIsViewOnlyTask:=vIsViewOnlyTask, vLinkedObjectName:=vLinkedObjectName, vLinkedClassName:=vLinkedClassName, vLinkedCaption:=vLinkedCaption, vIsAvailableTask:=vIsAvailableTask, vTaskCategoryID:=vTaskCategoryID), gPMConstants.PMEReturnCode)

        If m_lError = gPMConstants.PMEReturnCode.PMFalse Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Quick, let's go get the caption id

        If Not Informations.IsNothing(vDescription) Then
            sDescription = vDescription


            m_lError = m_oCaption.GetCaptionID(v_sCaption:=sDescription, r_lCaptionId:=lCaptionID)
        End If

        ' Set Property values.
        With oPMTask

            If Not Informations.IsNothing(vTaskId) Then
                .TaskID = vTaskId
            End If


            If Not Informations.IsNothing(vCode) Then
                .TaskCode = vCode
            End If


            If Not Informations.IsNothing(vDescription) Then
                .Description = vDescription
                .CaptionID = lCaptionID
            End If


            If Not Informations.IsNothing(vIsDeleted) Then
                .IsDeleted = vIsDeleted
            End If


            If Not Informations.IsNothing(vEffectiveDate) Then
                .EffectiveDate = vEffectiveDate
            End If


            If Not Informations.IsNothing(vIsSystemTask) Then
                .IsSystemTask = vIsSystemTask
            End If


            If Not Informations.IsNothing(vTypeOfTask) Then
                .TypeOfTask = vTypeOfTask
            End If


            If Not Informations.IsNothing(vPMNavProcessId) Then
                .PMNavProcessId = vPMNavProcessId
            End If


            If Not Informations.IsNothing(vComponentObjectName) Then
                .ComponentObjectName = vComponentObjectName
            End If


            If Not Informations.IsNothing(vComponentClassName) Then
                .ComponentClassName = vComponentClassName
            End If


            If Not Informations.IsNothing(vAutoDeleteAfterNumDays) Then
                .AutoDeleteAfterNumDays = vAutoDeleteAfterNumDays
            End If


            If Not Informations.IsNothing(vDisplayIcon) Then
                .DisplayIcon = vDisplayIcon
            End If


            If Not Informations.IsNothing(vIsViewOnlyTask) Then
                .IsViewOnlyTask = vIsViewOnlyTask
            End If


            If Not Informations.IsNothing(vLinkedObjectName) Then
                .LinkedObjectName = vLinkedObjectName
            End If


            If Not Informations.IsNothing(vLinkedClassName) Then
                .LinkedClassName = vLinkedClassName
            End If


            If Not Informations.IsNothing(vLinkedCaption) Then
                .LinkedCaption = vLinkedCaption
            End If


            If Not Informations.IsNothing(vIsAvailableTask) Then
                .IsAvailableTask = vIsAvailableTask
            End If

            'DAK221299

            If Not Informations.IsNothing(vTaskCategoryID) Then
                .TaskCategoryID = vTaskCategoryID
            End If
            'eck140904

            If Not Informations.IsNothing(vNavXMLFile) Then
                .NavXMLFile = vNavXMLFile
            End If
            'eck140904End
            .DatabaseStatus = iStatus

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Private)
    '
    ' Description: Returns the supplied PMTask property values.
    '
    ' DAK200999 - New column, DisplayIcon, on PMWrkTask Table
    ' DAK041099 - More new columns
    ' DAK221299 - Add PMWrk_Task_Category
    ' RAW 14/02/2003 : ISS2153 : added NavXMLFile
    ' ***************************************************************** '
    'developer guide no.101
    Private Function GetProperties(ByRef oPMTask As bPMTask.PMTask, ByRef vStatus As Object, Optional ByRef vTaskId As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vIsSystemTask As Object = Nothing, Optional ByRef vTypeOfTask As Object = Nothing, Optional ByRef vPMNavProcessId As Object = Nothing, Optional ByRef vComponentObjectName As Object = Nothing, Optional ByRef vComponentClassName As Object = Nothing, Optional ByRef vAutoDeleteAfterNumDays As Object = Nothing, Optional ByRef vDisplayIcon As Object = Nothing, Optional ByRef vIsViewOnlyTask As Object = Nothing, Optional ByRef vLinkedObjectName As Object = Nothing, Optional ByRef vLinkedClassName As Object = Nothing, Optional ByRef vLinkedCaption As Object = Nothing, Optional ByRef vIsAvailableTask As Object = Nothing, Optional ByRef vTaskCategoryID As Object = Nothing, Optional ByRef vNavXMLFile As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set Property values.
        With oPMTask

            'developer guide no.118

            vTaskId = .TaskID




            vCaptionID = .CaptionID




            vCode = .TaskCode




            vDescription = .Description




            vIsDeleted = .IsDeleted




            vEffectiveDate = .EffectiveDate




            vIsSystemTask = .IsSystemTask




            vTypeOfTask = .TypeOfTask




            vPMNavProcessId = .PMNavProcessId




            vComponentObjectName = .ComponentObjectName




            vComponentClassName = .ComponentClassName




            vAutoDeleteAfterNumDays = .AutoDeleteAfterNumDays




            vDisplayIcon = .DisplayIcon




            vIsViewOnlyTask = .IsViewOnlyTask




            vLinkedObjectName = .LinkedObjectName




            vLinkedClassName = .LinkedClassName




            vLinkedCaption = .LinkedCaption




            vIsAvailableTask = .IsAvailableTask


            'DAK221299


            vTaskCategoryID = .TaskCategoryID

            'eck140904


            vNavXMLFile = .NavXMLFile



        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddInputParam (Private)
    '
    ' Description: Adds all of the INPUT parameters
    '              required for an Insert or Update.
    '
    ' DAK200999 - New column, DisplayIcon, on PMWrkTask Table
    ' DAK041099 - More new columns
    ' DAK221299 - Add PMWrk_Task_Category
    ' ***************************************************************** '
    Private Function AddInputParam(ByRef oPMTask As bPMTask.PMTask) As Integer

        Dim result As Integer = 0
        Dim sCaption As String = ""
        Dim lCaptionID As Integer
        Dim sLinkedCaption As String = ""
        Dim lLinkedCaptionID As Integer




        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            sCaption = oPMTask.Description


            m_lError = m_oCaption.GetCaptionID(v_sCaption:=sCaption, r_lCaptionId:=lCaptionID)

            m_lError = .Parameters.Add(sName:="caption_id", vValue:=CStr(lCaptionID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="code", vValue:=oPMTask.TaskCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="description", vValue:=oPMTask.Description, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="is_deleted", vValue:=CStr(oPMTask.IsDeleted), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="effective_date", vValue:=ToSafeString(oPMTask.EffectiveDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="is_system_task", vValue:=CStr(oPMTask.IsSystemTask), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="type_of_task", vValue:=CStr(oPMTask.TypeOfTask), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'RFC300399
            If oPMTask.PMNavProcessId < 1 Then

                'Developer Guide No. 85
                m_lError = .Parameters.Add(sName:="pmnav_process_id", vValue:=(DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lError = .Parameters.Add(sName:="pmnav_process_id", vValue:=CStr(oPMTask.PMNavProcessId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="component_object_name", vValue:=oPMTask.ComponentObjectName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="component_class_name", vValue:=oPMTask.ComponentClassName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'RFC300399
            If oPMTask.AutoDeleteAfterNumDays = -1 Then

                'Developer Guide No. 85
                m_lError = .Parameters.Add(sName:="auto_delete_after_num_days", vValue:=(DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lError = .Parameters.Add(sName:="auto_delete_after_num_days", vValue:=CStr(oPMTask.AutoDeleteAfterNumDays), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DAK200999
            m_lError = .Parameters.Add(sName:="display_icon", vValue:=CStr(oPMTask.DisplayIcon), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DAK041099
            m_lError = .Parameters.Add(sName:="is_view_only_task", vValue:=CStr(oPMTask.IsViewOnlyTask), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oPMTask.LinkedCaption = "" Then


                'Developer Guide No. 85
                m_lError = .Parameters.Add(sName:="linked_caption_id", vValue:=(DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                'Developer Guide No. 85
                m_lError = .Parameters.Add(sName:="linked_object_name", vValue:=(DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                'Developer Guide No. 85
                m_lError = .Parameters.Add(sName:="linked_class_name", vValue:=(DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else

                sLinkedCaption = oPMTask.LinkedCaption

                m_lError = m_oCaption.GetCaptionID(v_sCaption:=sLinkedCaption, r_lCaptionId:=lLinkedCaptionID)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lError = .Parameters.Add(sName:="linked_caption_id", vValue:=CStr(lLinkedCaptionID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lError = .Parameters.Add(sName:="linked_object_name", vValue:=oPMTask.LinkedObjectName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lError = .Parameters.Add(sName:="linked_class_name", vValue:=oPMTask.LinkedClassName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            m_lError = .Parameters.Add(sName:="is_available_task", vValue:=CStr(oPMTask.IsAvailableTask), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DAK221299
            m_lError = .Parameters.Add(sName:="task_category_id", vValue:=CStr(oPMTask.TaskCategoryID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'RSB010903 start - (sp#246)
            m_lError = .Parameters.Add(sName:="source_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'RSB010903 end - (sp#246)

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ValidateParameters (Private)
    '
    ' Description: Checks that all parameters are valid for the datatype
    '
    ' DAK200999 - New column, DisplayIcon, on PMWrkTask Table
    ' DAK041099 - More new columns
    ' DAK221299 - Add PMWrk_Task_Category
    ' RAW 14/02/2003 : ISS2153 : added NavXMLFile
    ' ***************************************************************** '
    Private Function ValidateParameters(Optional ByRef vTaskId As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vIsSystemTask As Object = Nothing, Optional ByRef vTypeOfTask As Object = Nothing, Optional ByRef vPMNavProcessId As Object = Nothing, Optional ByRef vComponentObjectName As Object = Nothing, Optional ByRef vComponentClassName As Object = Nothing, Optional ByRef vAutoDeleteAfterNumDays As Object = Nothing, Optional ByRef vDisplayIcon As Object = Nothing, Optional ByRef vIsViewOnlyTask As Object = Nothing, Optional ByRef vLinkedObjectName As Object = Nothing, Optional ByRef vLinkedClassName As Object = Nothing, Optional ByRef vLinkedCaption As Object = Nothing, Optional ByRef vIsAvailableTask As Object = Nothing, Optional ByRef vTaskCategoryID As Object = Nothing, Optional ByRef vNavXMLFile As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue


        If Not Informations.IsNothing(vTaskId) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vTaskId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateParameters vTaskID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateParameters", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If
        End If


        If Not Informations.IsNothing(vIsDeleted) Then
            If vIsDeleted <> 0 And vIsDeleted <> 1 Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateParameters vIsDeleted Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateParameters", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If
        End If


        If Not Informations.IsNothing(vEffectiveDate) Then
            If Not Informations.IsDate(vEffectiveDate) Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateParameters vEffectiveDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateParameters", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If
        End If

        'DAK221299

        If Not Informations.IsNothing(vTaskCategoryID) Then

            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(vTaskCategoryID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateParameters vTaskCategoryID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateParameters", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DefaultMissingProperties (Private)
    '
    ' Description: Checks that all mandatory paramaters have been
    '               supplied and sets defaults for the non mandatory ones
    '
    ' DAK200999 - New column, DisplayIcon, on PMWrkTask Table
    ' DAK041099 - More new columns
    ' DAK221299 - Add PMWrk_Task_Category
    ' RAW 14/02/2003 : ISS2153 : added NavXMLFile
    ' ***************************************************************** '
    Private Function DefaultMissingParameters(Optional ByRef vTaskId As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vIsSystemTask As Object = Nothing, Optional ByRef vTypeOfTask As Object = Nothing, Optional ByRef vPMNavProcessId As Object = Nothing, Optional ByRef vComponentObjectName As Object = Nothing, Optional ByRef vComponentClassName As Object = Nothing, Optional ByRef vAutoDeleteAfterNumDays As Object = Nothing, Optional ByRef vDisplayIcon As Object = Nothing, Optional ByRef vIsViewOnlyTask As Object = Nothing, Optional ByRef vLinkedObjectName As Object = Nothing, Optional ByRef vLinkedClassName As Object = Nothing, Optional ByRef vLinkedCaption As Object = Nothing, Optional ByRef vIsAvailableTask As Object = Nothing, Optional ByRef vTaskCategoryID As Object = Nothing, Optional ByRef vNavXMLFile As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'set defaults for any properties which have not been supplied

        If Informations.IsNothing(vTaskId) Then
            vTaskId = 1
        End If


        If Informations.IsNothing(vIsDeleted) Then
            vIsDeleted = 0
        End If


        If Informations.IsNothing(vEffectiveDate) Then
            vEffectiveDate = DateTime.Now
        End If


        If Informations.IsNothing(vDisplayIcon) Then
            vDisplayIcon = 1
        End If


        If Informations.IsNothing(vIsViewOnlyTask) Then
            vIsViewOnlyTask = gPMConstants.PMEReturnCode.PMFalse
        End If


        If Informations.IsNothing(vIsAvailableTask) Then
            vIsAvailableTask = gPMConstants.PMEReturnCode.PMFalse
        End If

        'DAK221299

        If Informations.IsNothing(vTaskCategoryID) Then
            vTaskCategoryID = 1
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: MandatoryParameterCheck (Private)
    '
    ' Description: Checks that all mandatory paramaters have been
    '               supplied
    '
    ' DAK200999 - New column, DisplayIcon, on PMWrkTask Table
    ' DAK041099 - More new columns
    ' DAK221299 - Add PMWrk_Task_Category
    ' RAW 14/02/2003 : ISS2153 : added NavXMLFile
    ' ***************************************************************** '

    Private Function MandatoryParameterCheck(Optional ByRef vTaskId As Object = Nothing, Optional ByRef vCaptionID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vIsSystemTask As Object = Nothing, Optional ByRef vTypeOfTask As Object = Nothing, Optional ByRef vPMNavProcessId As Object = Nothing, Optional ByRef vComponentObjectName As Object = Nothing, Optional ByRef vComponentClassName As Object = Nothing, Optional ByRef vAutoDeleteAfterNumDays As Object = Nothing, Optional ByRef vDisplayIcon As Object = Nothing, Optional ByRef vIsViewOnlyTask As Object = Nothing, Optional ByRef vLinkedObjectName As Object = Nothing, Optional ByRef vLinkedClassName As Object = Nothing, Optional ByRef vLinkedCaption As Object = Nothing, Optional ByRef vIsAvailableTask As Object = Nothing, Optional ByRef vTaskCategoryID As Object = Nothing, Optional ByRef vNavXMLFile As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'check that all mandatory parameters have been supplied

        If Informations.IsNothing(vTaskId) Then

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Mandatory Property, vTaskId, Was Not Supplied", vApp:=ACApp, vClass:=ACClass, vMethod:="MandatoryParameterCheck", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'DAK221299

        If Informations.IsNothing(vTaskCategoryID) Then

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Mandatory Property, vTaskCategoryID, Was Not Supplied", vApp:=ACApp, vClass:=ACClass, vMethod:="MandatoryParameterCheck", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Private Function BeginTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lError = m_oDatabase.SQLBeginTrans()

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Private Function CommitTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lError = m_oDatabase.SQLCommitTrans()

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Private Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lError = m_oDatabase.SQLRollbackTrans()

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dispose(False)
    End Sub



    Public Function GetUserAuthorityToRunTask(ByVal v_iUserID As Integer, ByVal v_sTaskCode As String, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetUserAuthorityToRunTask"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters

            lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(v_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddParameters Failed")
            End If
            lReturn = m_oDatabase.Parameters.Add(sName:="Task_code", vValue:=v_sTaskCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddParameters Failed")
            End If


            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACUserAuthorityTorunTaskSQL, sSQLName:=ACUserAuthorityTorunTaskName, bStoredProcedure:=True, vResultArray:=r_vResults)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACUserAuthorityTorunTaskName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '      
            '        Return result
        End Try
        Return result
    End Function
End Class
