Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 24th October 1996
    '
    ' Description: Creatable Business class which contains all the
    '              methods, business rules required to manipulate
    '              a PMTaskGroup.
    '
    ' Edit History:
    ' Tom281098 - Nicked lock, stock and barrel from PMTaskGroup.
    ' DAK041099 - Add foriegn key to PMWrk_Task_Group_Category
    ' DAK231199 - Check PMProduct_Lookup for permissions
    ' DAK221299 - Remove PMWrk_Task_Group_Category
    '             PMProductLookup.GetDetails - PMProductID changed to
    '             variant.
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

    ' Collection of PMTaskGroups (Private)
    Private m_oPMTaskGroups As bPMTaskGroup.PMTaskGroups

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer (Private)
    Private m_lCurrentRecord As gPMConstants.PMEReturnCode

    ' Current Task Record Pointer (Private)
    Private m_lTaskCurrentRecord As gPMConstants.PMEReturnCode

    ' Error Code (Private)
    Private m_lError As gPMConstants.PMEReturnCode

    ' RDC 17102002
    Private m_lReturn As gPMConstants.PMEReturnCode

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
            ElseIf (Value > m_oPMTaskGroups.Count()) Then
                m_lCurrentRecord = m_oPMTaskGroups.Count()
            Else
                m_lCurrentRecord = Value
            End If

        End Set
    End Property


    Public Property TaskCurrentRecord() As Integer
        Get

            Return m_lTaskCurrentRecord

        End Get
        Set(ByVal Value As Integer)

            If Value < 0 Then
                m_lTaskCurrentRecord = gPMConstants.PMEReturnCode.PMFalse
            ElseIf (Value > m_oPMTaskGroups.Count()) Then
                m_lTaskCurrentRecord = m_oPMTaskGroups.Count()
            Else
                m_lTaskCurrentRecord = Value
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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise

        ' RDC 13062002 CompServ replaced with BAS module
        'Dim oComponentServices As PMServerBusinessCS
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


            If Information.IsNothing(vDatabase) Then
                lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            Else

                lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            End If

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '        Set oComponentServices = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Don't pass a database in, otherwise we'd corrupt the existing one...

            m_oCaption = New bPMCaption.Business
            lReturn = m_oCaption.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '        Set oComponentServices = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    Set oComponentServices = Nothing

            'create empty users collection
            m_oPMTaskGroups = New bPMTaskGroup.PMTaskGroups()

            ' Set Current Record to zero
            m_lCurrentRecord = gPMConstants.PMEReturnCode.PMFalse

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    ' Description: Adds a single PMTaskGroup directly into the database.
    '              Note: The PMTaskGroup will NOT be added to the collection.
    '
    ' DAK041099 - Add foriegn key to PMWrk_Task_Group_Category
    ' DAK221299 - Remove PMWrk_Task_Group_Category
    ' ***************************************************************** '
    Public Function Add(ByRef iPMTaskGroupID As Integer, ByRef sGroupCode As String, ByRef sGroupDescription As String, ByRef iIsDeleted As Integer, ByRef dtEffectiveDate As Date, ByRef lDisplayIcon As Integer) As Integer
        'DAK221299
        '                    lTaskGroupCategoryID As Long) As Long


        Dim result As Integer = 0
        Dim oPMTaskGroup As bPMTaskGroup.PMTaskGroup

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new PMTaskGroup
            oPMTaskGroup = New bPMTaskGroup.PMTaskGroup()

            ' Populate PMTaskGroup Attributes
            m_lError = CType(SetProperties(oPMTaskGroup:=oPMTaskGroup, iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPMTaskGroupId:=iPMTaskGroupID, vTaskGroupCode:=sGroupCode, vTaskGroupDescription:=sGroupDescription, vIsDeleted:=iIsDeleted, vEffectiveDate:=dtEffectiveDate, vDisplayIcon:=lDisplayIcon), gPMConstants.PMEReturnCode)
            'DAK221299
            '                              vTaskGroupCategoryID:=lTaskGroupCategoryID)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the PMTaskGroup to the Database
            m_lError = CType(AddItem(oPMTaskGroup), gPMConstants.PMEReturnCode)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the ID of the PMTaskGroup Added
            iPMTaskGroupID = oPMTaskGroup.TaskGroupID

            oPMTaskGroup = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required PMTaskGroups and populate the Collection
    ' for the GroupId if supplied, else returns all of the groups
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vPMTaskGroupId As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oPMTaskGroup As bPMTaskGroup.PMTaskGroup

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Check that we do not have any outstanding changes.

            m_lError = CType(Cancel(), gPMConstants.PMEReturnCode)

            ' If changes are outstanding, exit.
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lError
            End If

            ' Clear the Collection
            m_oPMTaskGroups.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = gPMConstants.PMEReturnCode.PMFalse

            'if the userid was supplied

            If Not Information.IsNothing(vPMTaskGroupId) Then

                ' If the supplied keys are not valid, exit

                Dim dbNumericTemp As Double
                If Not Double.TryParse(CStr(vPMTaskGroupId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : MessageID=" & CStr(vPMTaskGroupId), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                    Return result

                End If

                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                ' Add the UserId parameter (INPUT)

                m_lError = m_oDatabase.Parameters.Add(sName:="pmwrk_task_group_id", vValue:=CStr(vPMTaskGroupId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add the effective_date parameter (INPUT)
                'Developer Guide No. 41
                m_lError = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else
                ' select all of the PMTaskGroup records

                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetDetailsSQL, sSQLName:=ACGetDetailsName, bStoredProcedure:=ACGetDetailsStored, lNumberRecords:=0)

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

                    ' Create New PMTaskGroup
                    oPMTaskGroup = New bPMTaskGroup.PMTaskGroup()
                    'Developer Guide Nol 111
                    m_lError = CType(SetPropertiesFromDB(oPMTaskGroup:=oPMTaskGroup, lRecordNumber:=lSub - 1), gPMConstants.PMEReturnCode)

                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Add PMTaskGroup to collection
                    m_lError = CType(m_oPMTaskGroups.Add(oNewPMTaskGroup:=oPMTaskGroup), gPMConstants.PMEReturnCode)

                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oPMTaskGroup = Nothing

                Next lSub

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetTasks (Public)
    '
    ' Description: Gets the required Tasks and populate the Collection
    ' for the GroupId
    '
    ' ***************************************************************** '
    Public Function GetTasks(ByRef lGroupID As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oPMTaskGroup As bPMTaskGroup.PMTaskGroup

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Check that we do not have any outstanding changes.

            ' Clear the Collection
            m_oPMTaskGroups.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = gPMConstants.PMEReturnCode.PMFalse

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the group_id parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="pmwrk_task_group_id", vValue:=CStr(lGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the effective_date parameter (INPUT)
            'Developer Guide No. 41
            m_lError = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetEveryTaskSQL, sSQLName:=ACGetEveryTaskName, bStoredProcedure:=ACGetEveryTaskStored, lNumberRecords:=0)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
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

                    ' Create New PMTaskGroup
                    oPMTaskGroup = New bPMTaskGroup.PMTaskGroup()
                    'Developer Guide No. 111
                    m_lError = CType(SetPropertiesFromDB(oPMTaskGroup:=oPMTaskGroup, lRecordNumber:=lSub - 1), gPMConstants.PMEReturnCode)

                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Add PMTaskGroup to collection
                    m_lError = CType(m_oPMTaskGroups.Add(oNewPMTaskGroup:=oPMTaskGroup), gPMConstants.PMEReturnCode)

                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oPMTaskGroup = Nothing

                Next lSub

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTasks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTasks", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required PMTaskGroups and populate the Collection
    '
    ' DAK041099 - Add foriegn key to PMWrk_Task_Group_Category
    ' DAK221299 - Remove PMWrk_Task_Group_Category
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vPMTaskGroupId As Object = Nothing, Optional ByRef vTaskGroupCode As Object = Nothing, Optional ByRef vTaskGroupDescription As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vIncluded As Object = Nothing, Optional ByRef vDisplayIcon As Object = Nothing) As Integer
        'DAK221299
        '                        Optional vTaskGroupCategoryID As Variant) As Long


        Dim result As Integer = 0
        Dim oPMTaskGroup As bPMTaskGroup.PMTaskGroup
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oPMTaskGroups.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord = CType(m_lCurrentRecord + 1, gPMConstants.PMEReturnCode)
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oPMTaskGroup = m_oPMTaskGroups.Item(m_lCurrentRecord)

            ' Get the PMTaskGroup Property Values







            'Developer Guide No. 98
            m_lError = CType(GetProperties(oPMTaskGroup:=oPMTaskGroup, vStatus:=iStatus, vPMTaskGroupId:=vPMTaskGroupId, vTaskGroupCode:=vTaskGroupCode, vTaskGroupDescription:=vTaskGroupDescription, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vDisplayIcon:=vDisplayIcon, vIncluded:=vIncluded), gPMConstants.PMEReturnCode)
            'DAK221299
            '                              vTaskGroupCategoryID:=vTaskGroupCategoryID)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oPMTaskGroup = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied PMTaskGroup into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' DAK041099 - Add foriegn key to PMWrk_Task_Group_Category
    ' DAK221299 - Remove PMWrk_Task_Group_Category
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vPMTaskGroupId As Object = Nothing, Optional ByRef vTaskGroupCode As Object = Nothing, Optional ByRef vTaskGroupDescription As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vDisplayIcon As Object = Nothing, Optional ByRef vIncluded As Object = Nothing) As Integer
        'DAK221299
        '                        Optional vTaskGroupCategoryID) As Long

        Dim result As Integer = 0
        Dim oPMTaskGroup As bPMTaskGroup.PMTaskGroup

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oPMTaskGroups.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new PMTaskGroup
            oPMTaskGroup = New bPMTaskGroup.PMTaskGroup()

            ' Populate PMTaskGroup Attributes







            'Developer Guide No. 98
            m_lError = CType(SetProperties(oPMTaskGroup:=oPMTaskGroup, iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPMTaskGroupId:=vPMTaskGroupId, vTaskGroupCode:=vTaskGroupCode, vTaskGroupDescription:=vTaskGroupDescription, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vDisplayIcon:=vDisplayIcon, vIncluded:=vIncluded), gPMConstants.PMEReturnCode)
            'DAK221299
            '                              vTaskGroupCategoryID:=vTaskGroupCategoryID)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add PMTaskGroup to collection
            m_lError = CType(m_oPMTaskGroups.Add(oNewPMTaskGroup:=oPMTaskGroup), gPMConstants.PMEReturnCode)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oPMTaskGroup = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the PMTaskGroup
    '              specified and updates the PMTaskGroup with the new values.
    '
    ' DAK041099 - Add foriegn key to PMWrk_Task_Group_Category
    ' DAK221299 - Remove PMWrk_Task_Group_Category
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vPMTaskGroupId As Object = Nothing, Optional ByRef vTaskGroupCode As Object = Nothing, Optional ByRef vTaskGroupDescription As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vDisplayIcon As Object = Nothing, Optional ByRef vIncluded As Object = Nothing) As Integer
        'DAK221299
        '                           Optional vTaskGroupCategoryID As Variant) As Long


        Dim result As Integer = 0
        Dim oPMTaskGroup As bPMTaskGroup.PMTaskGroup
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oPMTaskGroups.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oPMTaskGroup = m_oPMTaskGroups.Item(lRow)

            ' Check the Status of the PMTaskGroup

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oPMTaskGroup.DatabaseStatus
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

            ' Update PMTaskGroup Attributes







            'Developer Guide No. 98
            m_lError = CType(SetProperties(oPMTaskGroup:=oPMTaskGroup, iStatus:=iStatus, vPMTaskGroupId:=vPMTaskGroupId, vTaskGroupCode:=vTaskGroupCode, vTaskGroupDescription:=vTaskGroupDescription, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vDisplayIcon:=vDisplayIcon, vIncluded:=vIncluded), gPMConstants.PMEReturnCode)
            'DAK221299
            '                              vTaskGroupCategoryID:=vTaskGroupCategoryID)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Release reference to PMTaskGroup
            oPMTaskGroup = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified PMTaskGroup can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oPMTaskGroup As bPMTaskGroup.PMTaskGroup

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oPMTaskGroups.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oPMTaskGroup = m_oPMTaskGroups.Item(lRow)

            ' Check the Status of the PMTaskGroup

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oPMTaskGroup.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oPMTaskGroup.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                'set deleted status to true
                oPMTaskGroup.IsDeleted = gPMConstants.PMEVarTrueFalse.PMVarTrue

                'set database status to update
                oPMTaskGroup.DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit
            End If

            ' Release reference to PMTaskGroup
            oPMTaskGroup = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditIgnore (Public)
    '
    ' Description: Validate that the specified PMTaskGroup can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditIgnore(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oPMTaskGroup As bPMTaskGroup.PMTaskGroup

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oPMTaskGroups.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oPMTaskGroup = m_oPMTaskGroups.Item(lRow)

            ' Check the Status of the PMTaskGroup

            oPMTaskGroup.Included = 0

            'set database status to update
            oPMTaskGroup.DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit

            ' Release reference to PMTaskGroup
            oPMTaskGroup = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditIgnore Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditIgnore", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditInclude (Public)
    '
    ' Description: Validate that the specified PMTaskGroup can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditInclude(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oPMTaskGroup As bPMTaskGroup.PMTaskGroup

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oPMTaskGroups.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oPMTaskGroup = m_oPMTaskGroups.Item(lRow)

            ' Check the Status of the PMTaskGroup

            oPMTaskGroup.Included = 1

            'set database status to update
            oPMTaskGroup.DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit

            ' Release reference to PMTaskGroup
            oPMTaskGroup = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditInclude Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditInclude", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            For lSub As Integer = 1 To m_oPMTaskGroups.Count()

                Select Case m_oPMTaskGroups.Item(lSub).DatabaseStatus

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Cancel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim oPMTaskGroup As bPMTaskGroup.PMTaskGroup
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub As Integer = 1 To m_oPMTaskGroups.Count()
                oPMTaskGroup = m_oPMTaskGroups.Item(lSub)


                Select Case oPMTaskGroup.DatabaseStatus
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
                        m_lError = CType(AddItem(oPMTaskGroup), gPMConstants.PMEReturnCode)
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
                        m_lError = CType(UpdateItem(oPMTaskGroup), gPMConstants.PMEReturnCode)
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
            oPMTaskGroup = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lError = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Set all items in the collection to PMView
                    For lSub As Integer = 1 To m_oPMTaskGroups.Count()
                        m_oPMTaskGroups.Item(lSub).DatabaseStatus = gPMConstants.PMEComponentAction.PMView
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateTasks (Public)
    '
    ' Description: Updates the tasks of the passed group
    '
    '
    ' ***************************************************************** '
    Public Function UpdateTasks(ByRef lGroupID As Integer) As Integer

        Dim result As Integer = 0
        Dim oPMTaskGroup As bPMTaskGroup.PMTaskGroup
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' If we haven't already started a transaction start one.
            If Not bTransStarted Then
                m_lError = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                bTransStarted = True
            End If

            'Get rid of existing records

            m_lError = CType(DeleteTasks(lGroupID:=lGroupID), gPMConstants.PMEReturnCode)

            ' Loop round Collection

            For lSub As Integer = 1 To m_oPMTaskGroups.Count()
                oPMTaskGroup = m_oPMTaskGroups.Item(lSub)

                If oPMTaskGroup.Included = 1 Then

                    m_lError = CType(AddTask(lGroupID, oPMTaskGroup), gPMConstants.PMEReturnCode)
                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Exit For
                    End If

                End If

            Next lSub

            ' Release last reference
            oPMTaskGroup = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lError = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Set all items in the collection to PMView
                    For lSub As Integer = 1 To m_oPMTaskGroups.Count()
                        m_oPMTaskGroups.Item(lSub).DatabaseStatus = gPMConstants.PMEComponentAction.PMView
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateTasks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateTasks", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: DeleteTasks (Public)
    '
    ' Description: Clears down the memberships.
    '
    ' ***************************************************************** '
    Private Function DeleteTasks(ByRef lGroupID As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters

        m_lError = m_oDatabase.Parameters.Add(sName:="pmwrk_task_group_id", vValue:=CStr(lGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lError = m_oDatabase.SQLAction(sSQL:=ACDeleteGroupTasksSQL, sSQLName:=ACDeleteGroupTasksName, bStoredProcedure:=ACDeleteGroupTasksStored, lRecordsAffected:=lRecordsAffected)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddTasks (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Private Function AddTask(ByRef lGroupID As Integer, ByRef oPMTaskGroup As bPMTaskGroup.PMTaskGroup) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters

        m_lError = m_oDatabase.Parameters.Add(sName:="pmwrk_task_group_id", vValue:=CStr(lGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lError = m_oDatabase.Parameters.Add(sName:="pmwrk_task_id", vValue:=CStr(oPMTaskGroup.TaskGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lError = m_oDatabase.Parameters.Add(sName:="display_sequence_num", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lError = m_oDatabase.SQLAction(sSQL:=ACAddGroupTaskSQL, sSQLName:=ACAddGroupTaskName, bStoredProcedure:=ACAddGroupTaskStored, lRecordsAffected:=lRecordsAffected)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: AddItem (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Private Function AddItem(ByRef oPMTaskGroup As bPMTaskGroup.PMTaskGroup) As Integer

        Dim result As Integer = 0
        Dim lNumberRecords As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters
        m_lError = CType(AddInputParam(oPMTaskGroup:=oPMTaskGroup), gPMConstants.PMEReturnCode)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lError = m_oDatabase.SQLSelect(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lNumberRecords:=lNumberRecords)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'Developer Guide No. 111
        oPMTaskGroup.TaskGroupID = m_oDatabase.Records.Item(0).Fields()("pmwrk_task_group_id")

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateItem (Public)
    '
    ' Description: UpdateItems a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Private Function UpdateItem(ByRef oPMTaskGroup As bPMTaskGroup.PMTaskGroup) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add PMTaskGroupID as an INPUT param for an update
        m_lError = m_oDatabase.Parameters.Add(sName:="pmwrk_task_group_id", vValue:=CStr(oPMTaskGroup.TaskGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the Input Parameters to the Parameteres collection
        m_lError = CType(AddInputParam(oPMTaskGroup:=oPMTaskGroup), gPMConstants.PMEReturnCode)

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
    'Private Function DeleteItem(ByRef oPMTaskGroup As bPMTaskGroup.PMTaskGroup) As Integer
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
    'm_lError = m_oDatabase.Parameters.Add(sName:="pmuser_group_id", vValue:=CStr(oPMTaskGroup.TaskGroupID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
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
            '    m_lError = CreateBusinessObject( _
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

            m_lError = oPMProductLookup.GetDetails(vPMProductID:=lPMProductID, vTableName:="PMWrk_Task_Group")
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPrivilegeLevel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPrivilegeLevel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        ' RDC 13062002 CompServ replaced with BAS module
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProductID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProductID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            '    m_lError = oCS.CreateBusinessObject( _
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserAuthorityFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserAuthority", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetPropertiesFromDB (Private)
    '
    ' Description: Sets the supplied PMTaskGroup properties from a database
    '              record.
    ' ***************************************************************** '
    Private Function SetPropertiesFromDB(ByRef oPMTaskGroup As bPMTaskGroup.PMTaskGroup, ByRef lRecordNumber As Integer) As Integer

        Dim result As Integer = 0
        'Developer Guide No. 112 (Guide)
        Dim oFields As DataRow
        Dim sDescription As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        oFields = m_oDatabase.Records.Item(lRecordNumber).Fields()

        ' Populate Base Details

        With oPMTaskGroup

            .TaskGroupID = oFields("id")
            .CaptionID = oFields("caption_id")
            .TaskGroupCode = oFields("code")


            m_lError = m_oCaption.GetCaptionDesc(v_lCaptionID:=.CaptionID, r_sCaption:=sDescription)
            .TaskGroupDescription = sDescription


            If Convert.IsDBNull(oFields("is_deleted")) Or IsNothing(oFields("is_deleted")) Then
                .IsDeleted = 0
            Else
                .IsDeleted = oFields("is_deleted")
            End If


            If Convert.IsDBNull(oFields("effective_date")) Or IsNothing(oFields("effective_date")) Then
                .EffectiveDate = #12/30/1899#
            Else
                .EffectiveDate = oFields("effective_date")
            End If

            .DisplayIcon = oFields("display_icon")

            .Included = oFields("included")

            'DAK221299
            '        .TaskGroupCategoryID = oFields.Item("task_group_category_id").Value

            .DatabaseStatus = gPMConstants.PMEComponentAction.PMView

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied PMTaskGroup property values.
    '
    ' DAK041099 - Add foriegn key to PMWrk_Task_Group_Category
    ' DAK221299 - Remove PMWrk_Task_Group_Category
    ' ***************************************************************** '
    Private Function SetProperties(ByRef oPMTaskGroup As bPMTaskGroup.PMTaskGroup, ByRef iStatus As Integer, Optional ByRef vPMTaskGroupId As Object = Nothing, Optional ByRef vTaskGroupCode As Object = Nothing, Optional ByRef vTaskGroupDescription As Object = Nothing, Optional ByRef vIsDeleted As gPMConstants.PMEVarTrueFalse = 0, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vDisplayIcon As Object = Nothing, Optional ByRef vIncluded As Object = Nothing) As Integer
        'DAK221299
        '                               Optional vTaskGroupCategoryID As Variant) As Long

        Dim result As Integer = 0
        Dim sDescription As String = ""
        Dim lCaptionID As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        'decide whether call is an add or an edit
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            'decide whether all of the mandatory values have been supplied
            m_lError = CType(MandatoryParameterCheck(vPMTaskGroupId:=vPMTaskGroupId, vTaskGroupCode:=vTaskGroupCode, vTaskGroupDescription:=vTaskGroupDescription, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vDisplayIcon:=vDisplayIcon, vIncluded:=vIncluded), gPMConstants.PMEReturnCode)
            'DAK221299
            '                                           vTaskGroupCategoryID:=vTaskGroupCategoryID)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'supply defaults for any missing parameters

            m_lError = CType(DefaultMissingParameters(vPMTaskGroupId:=vPMTaskGroupId, vTaskGroupCode:=vTaskGroupCode, vTaskGroupDescription:=vTaskGroupDescription, vIsDeleted:=vIsDeleted, vEffectiveDate:=CDate(vEffectiveDate), vDisplayIcon:=vDisplayIcon, vIncluded:=vIncluded), gPMConstants.PMEReturnCode)
            'DAK221299
            '                                            vTaskGroupCategoryID:=vTaskGroupCategoryID)

            If m_lError = gPMConstants.PMEReturnCode.PMFalse Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        'check whether the values in the parameters are valid
        m_lError = CType(ValidateParameters(vPMTaskGroupId:=vPMTaskGroupId, vTaskGroupCode:=vTaskGroupCode, vTaskGroupDescription:=vTaskGroupDescription, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vDisplayIcon:=vDisplayIcon, vIncluded:=vIncluded), gPMConstants.PMEReturnCode)
        'DAK221299
        '                                  vTaskGroupCategoryID:=vTaskGroupCategoryID)

        If m_lError = gPMConstants.PMEReturnCode.PMFalse Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Quick, let's go get the caption id

        If Not Information.IsNothing(vTaskGroupDescription) Then
            sDescription = vTaskGroupDescription


            m_lError = m_oCaption.GetCaptionID(v_scaption:=sDescription, r_lcaptionid:=lCaptionID)
        End If

        ' Set Property values.
        With oPMTaskGroup

            If Not Information.IsNothing(vPMTaskGroupId) Then
                .TaskGroupID = vPMTaskGroupId
            End If

            If Not Information.IsNothing(vTaskGroupCode) Then
                .TaskGroupCode = vTaskGroupCode
            End If

            If Not Information.IsNothing(vTaskGroupDescription) Then
                .TaskGroupDescription = vTaskGroupDescription
                .CaptionID = lCaptionID
            End If

            If Not Information.IsNothing(vIsDeleted) Then
                .IsDeleted = vIsDeleted
            End If

            If Not Information.IsNothing(vEffectiveDate) Then

                .EffectiveDate = CDate(vEffectiveDate)
            End If

            If Not Information.IsNothing(vDisplayIcon) Then
                .DisplayIcon = vDisplayIcon
            End If

            If Not Information.IsNothing(vIncluded) Then
                .Included = vIncluded
            End If
            'DAK221299
            '        If (Not IsMissing(vTaskGroupCategoryID) = True) Then
            '            .TaskGroupCategoryID = vTaskGroupCategoryID
            '        End If

            .DatabaseStatus = iStatus

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Private)
    '
    ' Description: Returns the supplied PMTaskGroup property values.
    '
    ' DAK041099 - Add foriegn key to PMWrk_Task_Group_Category
    ' DAK221299 - Remove PMWrk_Task_Group_Category
    ' ***************************************************************** '
    'Developer Guide No. 101
    Private Function GetProperties(ByRef oPMTaskGroup As bPMTaskGroup.PMTaskGroup, ByRef vStatus As Object, ByRef vPMTaskGroupId As Object, ByRef vTaskGroupCode As Object, ByRef vTaskGroupDescription As Object, ByRef vIsDeleted As Object, ByRef vEffectiveDate As Object, ByRef vDisplayIcon As Object, ByRef vIncluded As Object) As Integer
        'DAK221299
        '                               vTaskGroupCategoryID As Variant) As Long

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set Property values.
        With oPMTaskGroup

            If True Then
                vPMTaskGroupId = .TaskGroupID
            End If

            If True Then
                vTaskGroupCode = .TaskGroupCode
            End If

            If True Then
                vTaskGroupDescription = .TaskGroupDescription
            End If

            If True Then
                vIsDeleted = .IsDeleted
            End If

            If True Then

                vEffectiveDate = .EffectiveDate
            End If

            If True Then
                vDisplayIcon = .DisplayIcon
            End If

            If True Then
                vIncluded = .Included
            End If

            'DAK221299
            '        If (Not IsMissing(vTaskGroupCategoryID) = True) Then
            '            vTaskGroupCategoryID = .TaskGroupCategoryID
            '        End If

            If True Then
                vStatus = .DatabaseStatus
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddInputParam (Private)
    '
    ' Description: Adds all of the INPUT parameters
    '              required for an Insert or Update.
    '
    ' ***************************************************************** '
    Private Function AddInputParam(ByRef oPMTaskGroup As bPMTaskGroup.PMTaskGroup) As Integer

        Dim result As Integer = 0
        Dim sCaption As String = ""
        Dim lCaptionID As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            sCaption = oPMTaskGroup.TaskGroupDescription


            m_lError = m_oCaption.GetCaptionID(v_scaption:=sCaption, r_lcaptionid:=lCaptionID)

            m_lError = .Parameters.Add(sName:="caption_id", vValue:=CStr(lCaptionID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="code", vValue:=oPMTaskGroup.TaskGroupCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="description", vValue:=oPMTaskGroup.TaskGroupDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="is_deleted", vValue:=CStr(oPMTaskGroup.IsDeleted), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Developer Guide No. 41
            m_lError = .Parameters.Add(sName:="effective_date", vValue:=oPMTaskGroup.EffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="display_icon", vValue:=CStr(oPMTaskGroup.DisplayIcon), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' CTAF 191200 - Commented out as this is not used.

            '        m_lError = .Parameters.Add( _
            ''              sName:="included", _
            ''              vValue:=oPMTaskGroup.Included, _
            ''              iDirection:=PMParamInput, _
            ''              iDatatype:=PMInteger)
            '
            '        If (m_lError <> PMTrue) Then
            '            AddInputParam = PMFalse
            '            Exit Function
            '        End If

            'DAK221299
            '        m_lError = .Parameters.Add( _
            'sName:="task_group_category_id", _
            'vValue:=oPMTaskGroup.TaskGroupCategoryID, _
            'iDirection:=PMParamInput, _
            'iDatatype:=PMLong)

            '        If (m_lError <> PMTrue) Then
            '            AddInputParam = PMFalse
            '            Exit Function
            '        End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ValidateParameters (Private)
    '
    ' Description: Checks that all paramaters are valid for the datatype
    '
    ' DAK041099 - Add foriegn key to PMWrk_Task_Group_Category
    ' DAK221299 - Remove PMWrk_Task_Group_Category
    ' ***************************************************************** '
    Private Function ValidateParameters(Optional ByRef vPMTaskGroupId As Object = Nothing, Optional ByRef vTaskGroupCode As Object = Nothing, Optional ByRef vTaskGroupDescription As Object = Nothing, Optional ByRef vIsDeleted As Byte = 0, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vDisplayIcon As Object = Nothing, Optional ByRef vIncluded As Object = Nothing) As Integer
        'DAK221299
        '                                    Optional vTaskGroupCategoryID As Variant) As Long

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue


        If Not Information.IsNothing(vPMTaskGroupId) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vPMTaskGroupId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateParameters vPMTaskGroupId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateParameters", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If
        End If


        If Not Information.IsNothing(vTaskGroupCode) Then

        End If


        If Not Information.IsNothing(vTaskGroupDescription) Then

        End If


        If Not Information.IsNothing(vIsDeleted) Then
            If vIsDeleted <> 0 And vIsDeleted <> 1 Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateParameters vIsDeleted Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateParameters", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If
        End If


        If Not Information.IsNothing(vEffectiveDate) Then
            If Not Information.IsDate(vEffectiveDate) Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateParameters vEffectiveDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateParameters", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If
        End If


        If Not Information.IsNothing(vDisplayIcon) Then

        End If


        If Not Information.IsNothing(vIncluded) Then

        End If

        '    If (Not IsMissing(vTaskGroupCategoryID) = True) Then

        '    End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DefaultMissingProperties (Private)
    '
    ' Description: Checks that all mandatory paramaters have been
    '               supplied and sets defaults for the non mandatory ones
    '
    ' DAK041099 - Add foriegn key to PMWrk_Task_Group_Category
    ' DAK221299 - Remove PMWrk_Task_Group_Category
    ' ***************************************************************** '
    Private Function DefaultMissingParameters(Optional ByRef vPMTaskGroupId As Byte = 0, Optional ByRef vTaskGroupCode As Object = Nothing, Optional ByRef vTaskGroupDescription As Object = Nothing, Optional ByRef vIsDeleted As Byte = 0, Optional ByRef vEffectiveDate As Date = #12/30/1899#, Optional ByRef vDisplayIcon As Byte = 0, Optional ByRef vIncluded As Byte = 0) As Integer
        'DAK221299
        '                                          Optional vTaskGroupCategoryID) As Long

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'set defaults for any properties which have not been supplied

        If Information.IsNothing(vPMTaskGroupId) Then
            vPMTaskGroupId = 1
        End If


        If Information.IsNothing(vIsDeleted) Then
            vIsDeleted = 0
        End If


        If Information.IsNothing(vEffectiveDate) Then
            vEffectiveDate = DateTime.Now
        End If


        If Information.IsNothing(vDisplayIcon) Then
            vDisplayIcon = 0
        End If


        If Information.IsNothing(vIncluded) Then
            vIncluded = 0
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: MandatoryParameterCheck (Private)
    '
    ' Description: Checks that all mandatory paramaters have been
    '               supplied
    '
    ' DAK041099 - Add foriegn key to PMWrk_Task_Group_Category
    ' DAK221299 - Remove PMWrk_Task_Group_Category
    ' ***************************************************************** '

    Private Function MandatoryParameterCheck(Optional ByRef vPMTaskGroupId As Object = Nothing, Optional ByRef vTaskGroupCode As Object = Nothing, Optional ByRef vTaskGroupDescription As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vDisplayIcon As Object = Nothing, Optional ByRef vIncluded As Object = Nothing) As Integer
        'DAK221299
        '                                         Optional vTaskGroupCategoryID As Variant) As Long

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'check that all mandatory parameters have been supplied

        If Information.IsNothing(vTaskGroupCode) Or Information.IsNothing(vTaskGroupDescription) Then

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Mandatory Property Was Not Supplied", vApp:=ACApp, vClass:=ACClass, vMethod:="MandatoryParameterCheck", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'DAK221299
        '    If IsMissing(vTaskGroupCategoryID) Then

        ' Log Error Message
        '        LogMessage m_sUsername, _
        'iType:=PMLogOnError, _
        'sMsg:="Mandatory Property Was Not Supplied", _
        'vApp:=ACApp, _
        'vClass:=ACClass, _
        'vMethod:="MandatoryParameterCheck", _
        'vErrNo:=Err.Number, _
        'vErrDesc:=Err.Description

        '        MandatoryParameterCheck = PMFalse
        '       Exit Function
        '  End If

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: GetSysAdminStatus
    '
    ' Description: check if user is member of a SysAdmin user group.
    '
    ' History: RDC 17102002 created
    ' ***************************************************************** '
    Public Function GetSysAdminStatus(ByRef lStatus As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_lReturn = CType(gPMComponentServices.GetSysAdminAccessStatus(m_sUsername, m_iUserID, m_iSourceID, m_iLanguageID, lStatus, m_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSysAdminStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSysAdminStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
End Class
