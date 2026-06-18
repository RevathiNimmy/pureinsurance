Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
'Developer Guide No. 129
Imports SSP.Shared

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
    '              a PMUserGroup.
    '
    ' Edit History:
    ' Tom231098 - Nicked lock, stock and barrel from PMUser.
    ' DAK221199 - Check PMProductLookup table to see if we can continue
    ' DAK221299 - PMProductLookup.GetDetails - PMProductID changed to
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

    ' Collection of PMUserGroups (Private)
    Private m_oPMUserGroups As bPMUserGroup.PMUserGroups

    ' Collection of PMGroupTasks (Private)
    Private m_oPMGroupTasks As bPMUserGroup.PMGroupTasks

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
            ElseIf (Value > m_oPMUserGroups.Count()) Then
                m_lCurrentRecord = m_oPMUserGroups.Count()
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
            ElseIf (Value > m_oPMGroupTasks.Count()) Then
                m_lTaskCurrentRecord = m_oPMGroupTasks.Count()
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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        ' RDC 13062002 CompServ repalced with BAS module
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

            '    Set oComponentServices = New PMServerBusinessCS


            If Informations.IsNothing(vDatabase) Then
                '        lReturn = oComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, _
                'r_bNewInstanceCreated:=m_bCloseDatabase, _
                'r_oCheckedDatabase:=m_oDatabase)
                lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            Else
                '        lReturn = oComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, _
                'r_bNewInstanceCreated:=m_bCloseDatabase, _
                'r_oCheckedDatabase:=m_oDatabase, _
                'v_vDatabase:=vDatabase)

                lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            End If

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '        Set oComponentServices = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Don't pass a database in, otherwise we'd corrupt the existing one...
            '    lReturn = oComponentServices.CreateBusinessObject(r_oObject:=m_oCaption, _
            'v_sClassName:="bPMCaption.Business", _
            'v_sCallingAppName:=ACApp, _
            'v_sUserName:=m_sUsername, _
            'v_sPassword:=m_sPassword, _
            'v_iUserID:=m_iUserID, _
            'v_iSourceID:=m_iSourceID, _
            'v_iLanguageId:=m_iLanguageID, _
            'v_iCurrencyID:=m_iCurrencyID, _
            'v_iLogLevel:=m_iLogLevel)

            m_oCaption = New bPMCaption.Business
            lReturn = m_oCaption.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '        Set oComponentServices = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    Set oComponentServices = Nothing

            'create empty users collection
            m_oPMUserGroups = New bPMUserGroup.PMUserGroups()

            ' Set Current Record to zero
            m_lCurrentRecord = gPMConstants.PMEReturnCode.PMFalse

            'create empty tasks collection
            m_oPMGroupTasks = New bPMUserGroup.PMGroupTasks()

            ' Set Current Record to zero
            m_lTaskCurrentRecord = gPMConstants.PMEReturnCode.PMFalse

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
    ' Description: Adds a single PMUserGroup directly into the database.
    '              Note: The PMUserGroup will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function Add(ByRef iPMUserGroupID As Integer, ByRef sGroupCode As String, ByRef sGroupDescription As String, ByRef iIsDeleted As Integer, ByRef dtEffectiveDate As Date, ByRef iIsSysAdminGroup As Integer) As Integer

        Dim result As Integer = 0
        Dim oPMUserGroup As bPMUserGroup.PMUserGroup

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new PMUserGroup
            oPMUserGroup = New bPMUserGroup.PMUserGroup()

            ' Populate PMUserGroup Attributes
            m_lError = CType(SetProperties(oPMUserGroup:=oPMUserGroup, iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPMUserGroupId:=iPMUserGroupID, vUserGroupCode:=sGroupCode, vUserGroupDescription:=sGroupDescription, vIsDeleted:=iIsDeleted, vEffectiveDate:=dtEffectiveDate, vIsSysAdminGroup:=iIsSysAdminGroup), gPMConstants.PMEReturnCode)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the PMUserGroup to the Database
            m_lError = CType(AddItem(oPMUserGroup), gPMConstants.PMEReturnCode)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the ID of the PMUserGroup Added
            iPMUserGroupID = oPMUserGroup.UserOrGroupID

            oPMUserGroup = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required PMUserGroups and populate the Collection
    ' for the GroupId if supplied, else returns all of the groups
    '
    ' ***************************************************************** '
    Public Function GetDetails() As Integer
        Return GetDetails(vPMUserGroupId:=Nothing)
    End Function

    Public Function GetDetails(ByRef vPMUserGroupId As Object) As Integer
        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oPMUserGroup As bPMUserGroup.PMUserGroup

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Check that we do not have any outstanding changes.

            m_lError = CType(Cancel(), gPMConstants.PMEReturnCode)

            ' If changes are outstanding, exit.
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lError
            End If

            ' Clear the Collection
            m_oPMUserGroups.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = gPMConstants.PMEReturnCode.PMFalse

            'if the userid was supplied

            If Not Informations.IsNothing(vPMUserGroupId) Then

                ' If the supplied keys are not valid, exit

                Dim dbNumericTemp As Double
                If Not Double.TryParse(CStr(vPMUserGroupId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message

                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : MessageID=" & CStr(vPMUserGroupId), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                    Return result

                End If

                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                ' Add the UserId parameter (INPUT)

                m_lError = m_oDatabase.Parameters.Add(sName:="pmuser_group_id", vValue:=CStr(vPMUserGroupId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add the effective_date parameter (INPUT)
                'Developer Guide No. 40
                'm_lError = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTimeHelper.ToString(DateTime.Now), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMDate)
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
                ' select all of the PMUserGroup records

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
                'Developer Guide No.111
                'For lSub As Integer = 1 To lRecordCount
                For lSub As Integer = 0 To lRecordCount - 1

                    ' Create New PMUserGroup
                    oPMUserGroup = New bPMUserGroup.PMUserGroup()

                    m_lError = CType(SetPropertiesFromDB(oPMUserGroup:=oPMUserGroup, lRecordNumber:=lSub), gPMConstants.PMEReturnCode)

                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Add PMUserGroup to collection
                    If (m_oPMUserGroups.Count = 0) Then
                        m_oPMUserGroups.Add(Nothing)
                    End If
                    m_lError = CType(m_oPMUserGroups.Add(oNewPMUserGroup:=oPMUserGroup), gPMConstants.PMEReturnCode)

                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oPMUserGroup = Nothing

                Next lSub

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAllUsersAndGroups (Public)
    '
    ' Description: Gets the required PMUserGroups and populate the Collection
    ' for the GroupId if supplied, else returns all of the groups
    '
    ' ***************************************************************** '
    Public Function GetAllUsersAndGroups(ByRef lGroupID As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oPMUserGroup As bPMUserGroup.PMUserGroup

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Check that we do not have any outstanding changes.

            m_lError = CType(Cancel(), gPMConstants.PMEReturnCode)

            ' If changes are outstanding, exit.
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lError
            End If

            ' Clear the Collection
            m_oPMUserGroups.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = gPMConstants.PMEReturnCode.PMFalse

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the group_id parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="pmuser_group_id", vValue:=CStr(lGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the effective_date parameter (INPUT)
            'Developer Guide No. 40
            'm_lError = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTimeHelper.ToString(DateTime.Now), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMDate)
            m_lError = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetEveryoneDetailsSQL, sSQLName:=ACGetEveryoneDetailsName, bStoredProcedure:=ACGetEveryoneDetailsStored, lNumberRecords:=0)

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

                    ' Create New PMUserGroup
                    oPMUserGroup = New bPMUserGroup.PMUserGroup()
                    'Developer Guide No.111
                    'm_lError = CType(SetPropertiesFromDB(oPMUserGroup:=oPMUserGroup, lRecordNumber:=lSub), gPMConstants.PMEReturnCode)
                    m_lError = CType(SetPropertiesFromDB(oPMUserGroup:=oPMUserGroup, lRecordNumber:=lSub - 1), gPMConstants.PMEReturnCode)

                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Add PMUserGroup to collection
                    If (m_oPMUserGroups.Count = 0) Then
                        m_oPMUserGroups.Add(Nothing)
                    End If
                    m_lError = CType(m_oPMUserGroups.Add(oNewPMUserGroup:=oPMUserGroup), gPMConstants.PMEReturnCode)

                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oPMUserGroup = Nothing

                Next lSub

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllUsersAndGroups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllUsersAndGroups", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC260903 -PS256 -fsa compliance
    ' ***************************************************************** '
    ' Name: GetUserGroupInfoForUser
    '
    ' Description: Get Info On All Groups For User
    '
    ' ***************************************************************** '
    Public Function GetUserGroupInfo(ByRef r_lUserId As Integer, ByRef r_vUserGroupInfo As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the user_id parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(r_lUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the effective_date parameter (INPUT)
            'Developer Guide No. 40
            'm_lError = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTimeHelper.ToString(DateTime.Now), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMDate)
            m_lError = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetUserGroupInfoSQL, sSQLName:=ACGetUserGroupInfoName, bStoredProcedure:=ACGetUserGroupInfoStored, vResultArray:=r_vUserGroupInfo, lNumberRecords:=lRecordCount)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?

            If lRecordCount < 1 Then

                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserGroupInfo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserGroupInfo", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC260903 -PS256 -fsa compliance
    ' ***************************************************************** '
    ' Name: UpdateUserGroupInfoForUser
    '
    ' Description: Update Info On For User
    '
    ' ***************************************************************** '
    Public Function UpdateUserGroupInfo(ByRef r_lUserId As Integer, ByRef r_lPMUserGroupId As Integer, ByRef r_iMode As Integer, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer
        Return UpdateUserGroupInfo(r_lUserId:=r_lUserId, r_lPMUserGroupId:=r_lPMUserGroupId, r_iMode:=r_iMode, r_iIsSupervisor:=0, v_sUniqueId:=v_sUniqueId, v_sScreenHierarchy:=v_sScreenHierarchy)
    End Function

    Public Function UpdateUserGroupInfo(ByRef r_lUserId As Integer, ByRef r_lPMUserGroupId As Integer, ByRef r_iMode As Integer, ByRef r_iIsSupervisor As Integer, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the pmuser_group_id parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="pmuser_group_id", vValue:=CStr(r_lPMUserGroupId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the user_id parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(r_lUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = m_oDatabase.Parameters.Add(sName:="unique_id", vValue:=CStr(v_sUniqueId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = m_oDatabase.Parameters.Add(sName:="screen_hierarchy", vValue:=CStr(v_sScreenHierarchy), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If r_iMode = 0 Then

                ' Execute SQL Statement
                m_lError = m_oDatabase.SQLSelect(sSQL:=ACDeleteUserGroupInfoSQL, sSQLName:=ACDeleteUserGroupInfoName, bStoredProcedure:=ACDeleteUserGroupInfoStored)

            Else

                ' Add the display_sequence parameter (INPUT)
                m_lError = m_oDatabase.Parameters.Add(sName:="is_supervisor", vValue:=CStr(r_iIsSupervisor), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lError = m_oDatabase.SQLSelect(sSQL:=ACUpdateUserGroupInfoSQL, sSQLName:=ACUpdateUserGroupInfoName, bStoredProcedure:=ACUpdateUserGroupInfoStored)

            End If

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateUserGroupInfo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateUserGroupInfo", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetSystemAdministrators (Public)
    '
    ' Description: Returns the count of System Administrators
    '
    ' ***************************************************************** '
    Public Function GetSystemAdministrators(ByRef lHowMany As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the effective_date parameter (INPUT)
            'Developer Guide No. 40
            'm_lError = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTimeHelper.ToString(DateTime.Now), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMDate)
            m_lError = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetSystemAdministratorsSQL, sSQLName:=ACGetSystemAdministratorsName, bStoredProcedure:=ACGetSystemAdministratorsStored, lNumberRecords:=0)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' What was the count
            ' RDC 21062002 fields now zero-based
            'Developer Guide No.111
            'lHowMany = m_oDatabase.Records.Item(1).Fields()(0)
            lHowMany = m_oDatabase.Records.Item(0).Fields()(0)

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSystemAdministrators Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSystemAdministrators", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim oPMGroupTask As bPMUserGroup.PMGroupTask

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Check that we do not have any outstanding changes.

            ' Clear the Collection
            m_oPMGroupTasks.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = gPMConstants.PMEReturnCode.PMFalse

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the group_id parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="pmuser_group_id", vValue:=CStr(lGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the effective_date parameter (INPUT)
            'Developer Guide No. 40
            'm_lError = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTimeHelper.ToString(DateTime.Now), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMDate)
            m_lError = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetAllTasksSQL, sSQLName:=ACGetAllTasksName, bStoredProcedure:=ACGetAllTasksStored, lNumberRecords:=0)

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
                'Developer Guide No.111
                'For lSub As Integer = 1 To lRecordCount
                For lSub As Integer = 0 To lRecordCount - 1

                    ' Create New PMUserGroup
                    oPMGroupTask = New bPMUserGroup.PMGroupTask()

                    m_lError = CType(SetTaskPropertiesFromDB(oPMGroupTask:=oPMGroupTask, lRecordNumber:=lSub), gPMConstants.PMEReturnCode)

                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Add PMGroupTask to collection
                    If (m_oPMUserGroups.Count = 0) Then
                        m_oPMUserGroups.Add(Nothing)
                    End If
                    m_lError = CType(m_oPMGroupTasks.Add(oNewPMGroupTask:=oPMGroupTask), gPMConstants.PMEReturnCode)

                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oPMGroupTask = Nothing

                Next lSub

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTasks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTasks", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required PMUserGroups and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vUserOrGroup As Object = Nothing, Optional ByRef vPMUserGroupId As Object = Nothing, Optional ByRef vUserGroupCode As Object = Nothing, Optional ByRef vUserGroupDescription As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vIncluded As Object = Nothing, Optional ByRef vIsSysAdminGroup As Object = Nothing, Optional ByRef vIsSupervisor As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oPMUserGroup As bPMUserGroup.PMUserGroup
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oPMUserGroups.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord = CType(m_lCurrentRecord + 1, gPMConstants.PMEReturnCode)
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oPMUserGroup = m_oPMUserGroups.Item(m_lCurrentRecord)

            ' Get the PMUserGroup Property Values









            'Developer Guide No.98
            'm_lError = CType(GetProperties(oPMUserGroup:=oPMUserGroup, vStatus:=iStatus, vUserOrGroup:=CStr(vUserOrGroup), vPMUserGroupId:=CInt(vPMUserGroupId), vUserGroupCode:=CStr(vUserGroupCode), vUserGroupDescription:=CStr(vUserGroupDescription), vIsDeleted:=CInt(vIsDeleted), vEffectiveDate:=vEffectiveDate, vIncluded:=CInt(vIncluded), vIsSysAdminGroup:=CInt(vIsSysAdminGroup), vIsSupervisor:=CDbl(vIsSupervisor)), gPMConstants.PMEReturnCode)
            m_lError = GetProperties(oPMUserGroup:=oPMUserGroup, vStatus:=iStatus, vUserOrGroup:=vUserOrGroup, vPMUserGroupId:=vPMUserGroupId, vUserGroupCode:=vUserGroupCode, vUserGroupDescription:=vUserGroupDescription, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vIncluded:=vIncluded, vIsSysAdminGroup:=vIsSysAdminGroup, vIsSupervisor:=vIsSupervisor)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oPMUserGroup = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNextTask (Public)
    '
    ' Description: Gets the required PMGroupTasks and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNextTask(ByRef vPMTaskGroupId As Object) As Integer
        Return GetNextTask(vPMTaskGroupId:=vPMTaskGroupId, vTaskGroupCode:=Nothing, vTaskGroupDescription:=Nothing, vIsDeleted:=Nothing, vEffectiveDate:=Nothing, vIncluded:=Nothing)
    End Function

    Public Function GetNextTask(ByRef vPMTaskGroupId As Object, ByRef vTaskGroupCode As Object) As Integer
        Return GetNextTask(vPMTaskGroupId:=vPMTaskGroupId, vTaskGroupCode:=vTaskGroupCode, vTaskGroupDescription:=Nothing, vIsDeleted:=Nothing, vEffectiveDate:=Nothing, vIncluded:=Nothing)
    End Function

    Public Function GetNextTask(ByRef vPMTaskGroupId As Object, ByRef vTaskGroupCode As Object, ByRef vTaskGroupDescription As Object) As Integer
        Return GetNextTask(vPMTaskGroupId:=vPMTaskGroupId, vTaskGroupCode:=vTaskGroupCode, vTaskGroupDescription:=vTaskGroupDescription, vIsDeleted:=Nothing, vEffectiveDate:=Nothing, vIncluded:=Nothing)
    End Function

    Public Function GetNextTask(ByRef vPMTaskGroupId As Object, ByRef vTaskGroupCode As Object, ByRef vTaskGroupDescription As Object, ByRef vIsDeleted As Object) As Integer
        Return GetNextTask(vPMTaskGroupId:=vPMTaskGroupId, vTaskGroupCode:=vTaskGroupCode, vTaskGroupDescription:=vTaskGroupDescription, vIsDeleted:=vIsDeleted, vEffectiveDate:=Nothing, vIncluded:=Nothing)
    End Function

    Public Function GetNextTask(ByRef vPMTaskGroupId As Object, ByRef vTaskGroupCode As Object, ByRef vTaskGroupDescription As Object, ByRef vIsDeleted As Object, ByRef vEffectiveDate As Object) As Integer
        Return GetNextTask(vPMTaskGroupId:=vPMTaskGroupId, vTaskGroupCode:=vTaskGroupCode, vTaskGroupDescription:=vTaskGroupDescription, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vIncluded:=Nothing)
    End Function

    Public Function GetNextTask(ByRef vPMTaskGroupId As Object, ByRef vTaskGroupCode As Object, ByRef vTaskGroupDescription As Object, ByRef vIsDeleted As Object, ByRef vEffectiveDate As Object, ByRef vIncluded As Object) As Integer


        Dim result As Integer = 0

        Dim oPMGroupTask As bPMUserGroup.PMGroupTask
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lTaskCurrentRecord < m_oPMGroupTasks.Count() Then
                ' Increment current record pointer
                m_lTaskCurrentRecord = CType(m_lTaskCurrentRecord + 1, gPMConstants.PMEReturnCode)
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oPMGroupTask = m_oPMGroupTasks.Item(m_lTaskCurrentRecord)

            ' Get the PMUserGroup Property Values





            'Developer Guide No.98
            'm_lError = CType(GetTaskProperties(oPMGroupTask:=oPMGroupTask, vStatus:=iStatus, vPMTaskGroupId:=CInt(vPMTaskGroupId), vTaskGroupCode:=CStr(vTaskGroupCode), vTaskGroupDescription:=CStr(vTaskGroupDescription), vIsDeleted:=CInt(vIsDeleted), vEffectiveDate:=vEffectiveDate, vIncluded:=CInt(vIncluded)), gPMConstants.PMEReturnCode)
            m_lError = GetTaskProperties(oPMGroupTask:=oPMGroupTask, vStatus:=iStatus, vPMTaskGroupId:=vPMTaskGroupId, vTaskGroupCode:=vTaskGroupCode, vTaskGroupDescription:=vTaskGroupDescription, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vIncluded:=vIncluded)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oPMGroupTask = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNextTask Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNextTask", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied PMUserGroup into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vUserOrGroup As Object = Nothing, Optional ByRef vPMUserGroupId As Object = Nothing, Optional ByRef vUserGroupCode As Object = Nothing, Optional ByRef vUserGroupDescription As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vIncluded As Object = Nothing, Optional ByRef vIsSysAdminGroup As Object = Nothing, Optional ByRef vIsSupervisor As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oPMUserGroup As bPMUserGroup.PMUserGroup

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oPMUserGroups.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new PMUserGroup
            oPMUserGroup = New bPMUserGroup.PMUserGroup()

            ' Populate PMUserGroup Attributes









            'Developer Guide No.98
            'm_lError = CType(SetProperties(oPMUserGroup:=oPMUserGroup, iStatus:=gPMConstants.PMEComponentAction.PMAdd, vUserOrGroup:=CStr(vUserOrGroup), vPMUserGroupId:=CInt(vPMUserGroupId), vUserGroupCode:=CStr(vUserGroupCode), vUserGroupDescription:=CStr(vUserGroupDescription), vIsDeleted:=CInt(vIsDeleted), vEffectiveDate:=vEffectiveDate, vIncluded:=CInt(vIncluded), vIsSysAdminGroup:=CInt(vIsSysAdminGroup), vIsSupervisor:=CDbl(vIsSupervisor)), gPMConstants.PMEReturnCode)
            m_lError = SetProperties(oPMUserGroup:=oPMUserGroup, iStatus:=gPMConstants.PMEComponentAction.PMAdd, vUserOrGroup:=vUserOrGroup, vPMUserGroupId:=vPMUserGroupId, vUserGroupCode:=vUserGroupCode, vUserGroupDescription:=vUserGroupDescription, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vIncluded:=vIncluded, vIsSysAdminGroup:=vIsSysAdminGroup, vIsSupervisor:=vIsSupervisor)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add PMUserGroup to collection
            If (m_oPMUserGroups.Count = 0) Then
                m_oPMUserGroups.Add(Nothing)
            End If
            m_lError = CType(m_oPMUserGroups.Add(oNewPMUserGroup:=oPMUserGroup), gPMConstants.PMEReturnCode)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oPMUserGroup = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the PMUserGroup
    '              specified and updates the PMUserGroup with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vUserOrGroup As Object = Nothing, Optional ByRef vPMUserGroupId As Object = Nothing, Optional ByRef vUserGroupCode As Object = Nothing, Optional ByRef vUserGroupDescription As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vIncluded As Object = Nothing, Optional ByRef vIsSysAdminGroup As Object = Nothing, Optional ByRef vIsSupervisor As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oPMUserGroup As bPMUserGroup.PMUserGroup
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oPMUserGroups.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oPMUserGroup = m_oPMUserGroups.Item(lRow)

            ' Check the Status of the PMUserGroup

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oPMUserGroup.DatabaseStatus
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

            ' Update PMUserGroup Attributes









            'Developer Guide No.98
            'm_lError = CType(SetProperties(oPMUserGroup:=oPMUserGroup, iStatus:=iStatus, vUserOrGroup:=CStr(vUserOrGroup), vPMUserGroupId:=CInt(vPMUserGroupId), vUserGroupCode:=CStr(vUserGroupCode), vUserGroupDescription:=CStr(vUserGroupDescription), vIsDeleted:=CInt(vIsDeleted), vEffectiveDate:=vEffectiveDate, vIncluded:=CInt(vIncluded), vIsSysAdminGroup:=CInt(vIsSysAdminGroup), vIsSupervisor:=CDbl(vIsSupervisor)), gPMConstants.PMEReturnCode)
            m_lError = SetProperties(oPMUserGroup:=oPMUserGroup, iStatus:=iStatus, vUserOrGroup:=vUserOrGroup, vPMUserGroupId:=vPMUserGroupId, vUserGroupCode:=vUserGroupCode, vUserGroupDescription:=vUserGroupDescription, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vIncluded:=vIncluded, vIsSysAdminGroup:=vIsSysAdminGroup, vIsSupervisor:=vIsSupervisor)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Release reference to PMUserGroup
            oPMUserGroup = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified PMUserGroup can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oPMUserGroup As bPMUserGroup.PMUserGroup

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oPMUserGroups.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oPMUserGroup = m_oPMUserGroups.Item(lRow)

            ' Check the Status of the PMUserGroup

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oPMUserGroup.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oPMUserGroup.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                'set deleted status to true
                oPMUserGroup.IsDeleted = gPMConstants.PMEVarTrueFalse.PMVarTrue

                'set database status to update
                oPMUserGroup.DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit
            End If

            ' Release reference to PMUserGroup
            oPMUserGroup = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditIgnore (Public)
    '
    ' Description: Validate that the specified PMUserGroup can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditIgnore(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oPMUserGroup As bPMUserGroup.PMUserGroup

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oPMUserGroups.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oPMUserGroup = m_oPMUserGroups.Item(lRow)

            ' Check the Status of the PMUserGroup

            oPMUserGroup.Included = 0

            'Let's turn off the supervisor flag as well
            oPMUserGroup.IsSupervisor = 0

            'set database status to update
            oPMUserGroup.DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit

            ' Release reference to PMUserGroup
            oPMUserGroup = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditIgnore Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditIgnore", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditInclude (Public)
    '
    ' Description: Validate that the specified PMUserGroup can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditInclude(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oPMUserGroup As bPMUserGroup.PMUserGroup

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oPMUserGroups.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oPMUserGroup = m_oPMUserGroups.Item(lRow)

            ' Check the Status of the PMUserGroup

            oPMUserGroup.Included = 1

            'set database status to update
            oPMUserGroup.DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit

            ' Release reference to PMUserGroup
            oPMUserGroup = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditInclude Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditInclude", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditIgnoreTask (Public)
    '
    ' Description: Validate that the specified PMUserGroup can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditIgnoreTask(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oPMGroupTask As bPMUserGroup.PMGroupTask

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oPMGroupTasks.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oPMGroupTask = m_oPMGroupTasks.Item(lRow)

            ' Check the Status of the PMUserGroup

            oPMGroupTask.Included = 0

            'set database status to update
            oPMGroupTask.DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit

            ' Release reference to PMUserGroup
            oPMGroupTask = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditIgnoreTask Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditIgnoreTask", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditIncludeTask (Public)
    '
    ' Description: Validate that the specified PMUserGroup can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditIncludeTask(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oPMGroupTask As bPMUserGroup.PMGroupTask

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oPMGroupTasks.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oPMGroupTask = m_oPMGroupTasks.Item(lRow)

            ' Check the Status of the PMUserGroup

            oPMGroupTask.Included = 1

            'set database status to update
            oPMGroupTask.DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit

            ' Release reference to PMUserGroup
            oPMGroupTask = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditIncludeTask Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditIncludeTask", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditToggleSupervisor (Public)
    '
    ' Description: Validate that the specified user is (not) a supervisor
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditToggleSupervisor(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oPMUserGroup As bPMUserGroup.PMUserGroup

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oPMUserGroups.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to toggle
            oPMUserGroup = m_oPMUserGroups.Item(lRow)

            ' Toggle the supervisor status

            oPMUserGroup.IsSupervisor = 1 - oPMUserGroup.IsSupervisor

            'set database status to update
            oPMUserGroup.DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit

            ' Release reference to PMUserGroup
            oPMUserGroup = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditToggleSupervisor Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditToggleSupervisor", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            For lSub As Integer = 1 To m_oPMUserGroups.Count()
                Select Case m_oPMUserGroups.Item(lSub).DatabaseStatus
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
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Cancel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim oPMUserGroup As bPMUserGroup.PMUserGroup
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub As Integer = 1 To m_oPMUserGroups.Count()
                oPMUserGroup = m_oPMUserGroups.Item(lSub)


                Select Case oPMUserGroup.DatabaseStatus
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
                        m_lError = CType(AddItem(oPMUserGroup), gPMConstants.PMEReturnCode)
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
                        m_lError = CType(UpdateItem(oPMUserGroup), gPMConstants.PMEReturnCode)
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
            oPMUserGroup = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lError = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Set all items in the collection to PMView
                    For lSub As Integer = 1 To m_oPMUserGroups.Count()
                        m_oPMUserGroups.Item(lSub).DatabaseStatus = gPMConstants.PMEComponentAction.PMView
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
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateMemberships (Public)
    '
    ' Description: Updates the memberships of the passed group
    '
    '
    ' ***************************************************************** '
    Public Function UpdateMemberships(ByRef lGroupID As Integer) As Integer

        Dim result As Integer = 0
        Dim oPMUserGroup As bPMUserGroup.PMUserGroup
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

            m_lError = CType(DeleteMemberships(lGroupID:=lGroupID), gPMConstants.PMEReturnCode)

            ' Loop round Collection

            For lSub As Integer = 1 To m_oPMUserGroups.Count()
                oPMUserGroup = m_oPMUserGroups.Item(lSub)

                If oPMUserGroup.Included = 1 Then

                    If oPMUserGroup.UserOrGroup = "group" Then
                        ' Add Group Item
                        m_lError = CType(AddGroupMembership(lGroupID, oPMUserGroup), gPMConstants.PMEReturnCode)
                        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Else
                        ' Add User Item
                        m_lError = CType(AddUserMembership(lGroupID, oPMUserGroup), gPMConstants.PMEReturnCode)
                        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    End If

                End If

            Next lSub

            ' Release last reference
            oPMUserGroup = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lError = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Set all items in the collection to PMView
                    For lSub As Integer = 1 To m_oPMUserGroups.Count()
                        m_oPMUserGroups.Item(lSub).DatabaseStatus = gPMConstants.PMEComponentAction.PMView
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
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateMemberships Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateMemberships", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

        Dim oPMGroupTask As bPMUserGroup.PMGroupTask
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

            For lSub As Integer = 1 To m_oPMGroupTasks.Count() - 1
                oPMGroupTask = m_oPMGroupTasks.Item(lSub)

                If oPMGroupTask.Included = 1 Then

                    m_lError = CType(AddTask(lGroupID, oPMGroupTask), gPMConstants.PMEReturnCode)
                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Exit For
                    End If

                End If

            Next lSub

            ' Release last reference
            oPMGroupTask = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lError = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Set all items in the collection to PMView
                    For lSub As Integer = 1 To m_oPMGroupTasks.Count() - 1
                        m_oPMGroupTasks.Item(lSub).DatabaseStatus = gPMConstants.PMEComponentAction.PMView
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
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateTasks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateTasks", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: DeleteMemberships (Public)
    '
    ' Description: Clears down the memberships.
    '
    ' ***************************************************************** '
    Private Function DeleteMemberships(ByRef lGroupID As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Do the groups
        ' Add the required INPUT parameters

        m_lError = m_oDatabase.Parameters.Add(sName:="pmuser_group_id", vValue:=CStr(lGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lError = m_oDatabase.SQLAction(sSQL:=ACDeleteGroupMembershipSQL, sSQLName:=ACDeleteGroupMembershipName, bStoredProcedure:=ACDeleteGroupMembershipStored, lRecordsAffected:=lRecordsAffected)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        'Do the users
        ' Add the required INPUT parameters

        m_lError = m_oDatabase.Parameters.Add(sName:="pmuser_group_id", vValue:=CStr(lGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lError = m_oDatabase.SQLAction(sSQL:=ACDeleteUserMembershipSQL, sSQLName:=ACDeleteUserMembershipName, bStoredProcedure:=ACDeleteUserMembershipStored, lRecordsAffected:=lRecordsAffected)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddGroupMembership (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Private Function AddGroupMembership(ByRef lGroupID As Integer, ByRef oPMUserGroup As bPMUserGroup.PMUserGroup) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters

        m_lError = m_oDatabase.Parameters.Add(sName:="pmuser_group_id", vValue:=CStr(lGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lError = m_oDatabase.Parameters.Add(sName:="pmuser_member_group_id", vValue:=CStr(oPMUserGroup.UserOrGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lError = m_oDatabase.Parameters.Add(sName:="display_sequence_num", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lError = m_oDatabase.SQLAction(sSQL:=ACAddGroupMembershipSQL, sSQLName:=ACAddGroupMembershipName, bStoredProcedure:=ACAddGroupMembershipStored, lRecordsAffected:=lRecordsAffected)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddUserMembership (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Private Function AddUserMembership(ByRef lGroupID As Integer, ByRef oPMUserGroup As bPMUserGroup.PMUserGroup) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters

        m_lError = m_oDatabase.Parameters.Add(sName:="pmuser_group_id", vValue:=CStr(lGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lError = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(oPMUserGroup.UserOrGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lError = m_oDatabase.Parameters.Add(sName:="display_sequence_num", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lError = m_oDatabase.Parameters.Add(sName:="is_supervisor", vValue:=CStr(oPMUserGroup.IsSupervisor), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lError = m_oDatabase.SQLAction(sSQL:=ACAddUserMembershipSQL, sSQLName:=ACAddUserMembershipName, bStoredProcedure:=ACAddUserMembershipStored, lRecordsAffected:=lRecordsAffected)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

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

        m_lError = m_oDatabase.Parameters.Add(sName:="pmuser_group_id", vValue:=CStr(lGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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
    Private Function AddTask(ByRef lGroupID As Integer, ByRef oPMGroupTask As bPMUserGroup.PMGroupTask) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters

        m_lError = m_oDatabase.Parameters.Add(sName:="pmuser_group_id", vValue:=CStr(lGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lError = m_oDatabase.Parameters.Add(sName:="pmwrk_task_group_id", vValue:=CStr(oPMGroupTask.TaskGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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
    Private Function AddItem(ByRef oPMUserGroup As bPMUserGroup.PMUserGroup) As Integer

        Dim result As Integer = 0
        Dim lNumberRecords As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters
        m_lError = CType(AddInputParam(oPMUserGroup:=oPMUserGroup), gPMConstants.PMEReturnCode)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lError = m_oDatabase.SQLSelect(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lNumberRecords:=lNumberRecords)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' RDC 21062002 fields now zero-based
        'Developer Guide No.111
        'oPMUserGroup.UserOrGroupID = m_oDatabase.Records.Item(1).Fields()(0)
        oPMUserGroup.UserOrGroupID = m_oDatabase.Records.Item(0).Fields()(0)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateItem (Public)
    '
    ' Description: UpdateItems a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Private Function UpdateItem(ByRef oPMUserGroup As bPMUserGroup.PMUserGroup) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add PMUserGroupID as an INPUT param for an update
        m_lError = m_oDatabase.Parameters.Add(sName:="pmuser_group_id", vValue:=CStr(oPMUserGroup.UserOrGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the Input Parameters to the Parameteres collection
        m_lError = CType(AddInputParam(oPMUserGroup:=oPMUserGroup), gPMConstants.PMEReturnCode)

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
    'Private Function DeleteItem(ByRef oPMUserGroup As bPMUserGroup.PMUserGroup) As Integer
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
    'm_lError = m_oDatabase.Parameters.Add(sName:="pmuser_group_id", vValue:=CStr(oPMUserGroup.UserOrGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMInteger)
    '
    'If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' Execute SQL Statement
    'm_lError = m_oDatabase.SQLAction(sSQL:=ACDeleteSQL, ssqlname:=ACDeleteName, bstoredprocedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)
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
    'bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
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

            ' Create the Product Lookup Component to do this work.
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

            m_lError = oPMProductLookup.GetDetails(vPMProductId:=lPMProductID, vTableName:="PMUser_Group")
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
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPrivilegeLevel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPrivilegeLevel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            ' Create the Product Component to do this work.
            '    Set oCS = New sPMServerCS.PMServerBusinessCS
            '    m_lError = CreateBusinessObject( _
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
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProductID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProductID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim oUtilities As bPMUserGroup.Utilities
        ' RDC 13062002 CompServ replaced with BAS module
        ''Dim oCS As sPMServerCS.PMServerBusinessCS

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the User Group Component to do this work.
            '    Set oCS = New sPMServerCS.PMServerBusinessCS
            '    m_lError = oCS.CreateBusinessObject( _
            'r_oObject:=oUtilities, _
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

            oUtilities = New bPMUserGroup.Utilities
            m_lError = oUtilities.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            '    Set oCS = Nothing
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Is the User an Administrator

            m_lError = oUtilities.IsUserAdministrator(v_iUserID:=m_iUserID, r_bIsAdministrator:=r_bIsAdministrator)
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                oUtilities.Dispose()
                oUtilities = Nothing
                Return result
            End If

            ' Get the Groups they Supervise

            m_lError = oUtilities.GetGroupsSupervisedByUser(v_iUserID:=m_iUserID, r_vSupervisedGroups:=r_vSupervisedGroups)
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                oUtilities.Dispose()
                oUtilities = Nothing
                Return result
            End If

            ' Terminate

            oUtilities.Dispose()
            oUtilities = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserAuthorityFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserAuthority", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetPropertiesFromDB (Private)
    '
    ' Description: Sets the supplied PMUserGroup properties from a database
    '              record.
    ' ***************************************************************** '
    Private Function SetPropertiesFromDB(ByRef oPMUserGroup As bPMUserGroup.PMUserGroup, ByRef lRecordNumber As Integer) As Integer

        Dim result As Integer = 0
        'Developer Guide No 21. 
        Dim oFields As DataRow
        Dim sDescription As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        oFields = m_oDatabase.Records.Item(lRecordNumber).Fields()

        ' Populate Base Details

        With oPMUserGroup

            .UserOrGroup = oFields("user_or_group")
            .UserOrGroupID = oFields("id")
            .CaptionID = oFields("caption_id")
            .UserGroupCode = oFields("code")

            If .UserOrGroup = "group" Then

                m_lError = m_oCaption.GetCaptionDesc(v_lCaptionId:= .CaptionID, r_sCaption:=sDescription)
                .UserGroupDescription = sDescription
            Else
                .UserGroupDescription = oFields("description")
            End If


            If Convert.IsDBNull(oFields("is_deleted")) Or Informations.IsNothing(oFields("is_deleted")) Then
                .IsDeleted = 0
            Else
                .IsDeleted = oFields("is_deleted")
            End If


            If Convert.IsDBNull(oFields("effective_date")) Or Informations.IsNothing(oFields("effective_date")) Then
                .EffectiveDate = #12/30/1899#
            Else
                .EffectiveDate = oFields("effective_date")
            End If


            If Convert.IsDBNull(oFields("is_sys_admin_group")) Or Informations.IsNothing(oFields("is_sys_admin_group")) Then
                .IsSysAdminGroup = 0
            Else
                .IsSysAdminGroup = oFields("is_sys_admin_group")
            End If


            If Convert.IsDBNull(oFields("is_supervisor")) Or Informations.IsNothing(oFields("is_supervisor")) Then
                .IsSupervisor = 0
            Else
                .IsSupervisor = oFields("is_supervisor")
            End If

            .Included = oFields("included")

            .DatabaseStatus = gPMConstants.PMEComponentAction.PMView

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetTaskPropertiesFromDB (Private)
    '
    ' Description: Sets the supplied PMGroupTask properties from a database
    '              record.
    ' ***************************************************************** '
    Private Function SetTaskPropertiesFromDB(ByRef oPMGroupTask As bPMUserGroup.PMGroupTask, ByRef lRecordNumber As Integer) As Integer

        Dim result As Integer = 0
        'Developer Guide No 21. 
        'Dim oFields As ADODB.Fields
        Dim oFields As DataRow
        Dim sDescription As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        oFields = m_oDatabase.Records.Item(lRecordNumber).Fields()

        ' Populate Base Details

        With oPMGroupTask

            .TaskGroupID = oFields("id")
            .CaptionID = oFields("caption_id")
            .TaskGroupCode = oFields("code")


            m_lError = m_oCaption.GetCaptionDesc(v_lCaptionId:= .CaptionID, r_sCaption:=sDescription)
            .TaskGroupDescription = sDescription


            If Convert.IsDBNull(oFields("is_deleted")) Or Informations.IsNothing(oFields("is_deleted")) Then
                .IsDeleted = 0
            Else
                .IsDeleted = oFields("is_deleted")
            End If


            If Convert.IsDBNull(oFields("effective_date")) Or Informations.IsNothing(oFields("effective_date")) Then
                .EffectiveDate = #12/30/1899#
            Else
                .EffectiveDate = oFields("effective_date")
            End If

            .Included = oFields("included")

            .DatabaseStatus = gPMConstants.PMEComponentAction.PMView

        End With

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied PMUserGroup property values.
    '
    ' **************************************************************** '
    'Developer Guide No.98
    'Private Function SetProperties(ByRef oPMUserGroup As bPMUserGroup.PMUserGroup, ByRef iStatus As Integer, Optional ByRef vUserOrGroup As String = "", Optional ByRef vPMUserGroupId As Integer = 0, Optional ByRef vUserGroupCode As String = "", Optional ByRef vUserGroupDescription As String = "", Optional ByRef vIsDeleted As gPMConstants.PMEVarTrueFalse = 0, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vIncluded As Integer = 0, Optional ByRef vIsSysAdminGroup As Integer = 0, Optional ByRef vIsSupervisor As Double = 0) As Integer
    Private Function SetProperties(ByRef oPMUserGroup As bPMUserGroup.PMUserGroup, ByRef iStatus As Integer, Optional ByRef vUserOrGroup As Object = Nothing, Optional ByRef vPMUserGroupId As Object = Nothing, Optional ByRef vUserGroupCode As Object = Nothing, Optional ByRef vUserGroupDescription As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vIncluded As Object = Nothing, Optional ByRef vIsSysAdminGroup As Object = Nothing, Optional ByRef vIsSupervisor As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sDescription As String = ""
        Dim lCaptionID As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        'decide whether call is an add or an edit
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            'decide whether all of the mandatory values have been supplied
            m_lError = CType(MandatoryParameterCheck(vUserOrGroup:=vUserOrGroup, vPMUserGroupId:=vPMUserGroupId, vUserGroupCode:=vUserGroupCode, vUserGroupDescription:=vUserGroupDescription, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vIncluded:=vIncluded, vIsSysAdminGroup:=vIsSysAdminGroup, vIsSupervisor:=vIsSupervisor), gPMConstants.PMEReturnCode)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'supply defaults for any missing parameters

            'Developer Guide No.98
            'm_lError = CType(DefaultMissingParameters(vUserOrGroup:=vUserOrGroup, vPMUserGroupId:=vPMUserGroupId, vUserGroupCode:=vUserGroupCode, vUserGroupDescription:=vUserGroupDescription, vIsDeleted:=vIsDeleted, vEffectiveDate:=CDate(vEffectiveDate), vIncluded:=vIncluded, vIsSysAdminGroup:=vIsSysAdminGroup, vIsSupervisor:=vIsSupervisor), gPMConstants.PMEReturnCode)
            m_lError = CType(DefaultMissingParameters(vUserOrGroup:=vUserOrGroup, vPMUserGroupId:=vPMUserGroupId, vUserGroupCode:=vUserGroupCode, vUserGroupDescription:=vUserGroupDescription, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vIncluded:=vIncluded, vIsSysAdminGroup:=vIsSysAdminGroup, vIsSupervisor:=vIsSupervisor), gPMConstants.PMEReturnCode)

            If m_lError = gPMConstants.PMEReturnCode.PMFalse Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        'check whether the values in the parameters are valid
        'Developer Guide No.98
        'm_lError = CType(ValidateParameters(vUserOrGroup:=CInt(vUserOrGroup), vPMUserGroupId:=vPMUserGroupId, vUserGroupCode:=vUserGroupCode, vUserGroupDescription:=vUserGroupDescription, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vIncluded:=vIncluded, vIsSysAdminGroup:=vIsSysAdminGroup, vIsSupervisor:=vIsSupervisor), gPMConstants.PMEReturnCode)
        m_lError = CType(ValidateParameters(vUserOrGroup:=vUserOrGroup, vPMUserGroupId:=vPMUserGroupId, vUserGroupCode:=vUserGroupCode, vUserGroupDescription:=vUserGroupDescription, vIsDeleted:=vIsDeleted, vEffectiveDate:=vEffectiveDate, vIncluded:=vIncluded, vIsSysAdminGroup:=vIsSysAdminGroup, vIsSupervisor:=vIsSupervisor), gPMConstants.PMEReturnCode)

        If m_lError = gPMConstants.PMEReturnCode.PMFalse Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Quick, let's go get the caption id

        If Not Informations.IsNothing(vUserGroupDescription) Then

            If Not Informations.IsNothing(vUserOrGroup) Then
                If vUserOrGroup = "group" Then
                    sDescription = vUserGroupDescription


                    m_lError = m_oCaption.GetCaptionID(v_sCaption:=sDescription, r_lCaptionId:=lCaptionID)
                Else
                    lCaptionID = 0
                End If
            Else
                sDescription = vUserGroupDescription


                m_lError = m_oCaption.GetCaptionID(v_sCaption:=sDescription, r_lCaptionId:=lCaptionID)
            End If
        End If

        ' Set Property values.
        With oPMUserGroup

            If Not Informations.IsNothing(vUserOrGroup) Then
                .UserOrGroup = vUserOrGroup
            End If

            If Not Informations.IsNothing(vPMUserGroupId) Then
                .UserOrGroupID = vPMUserGroupId
            End If

            If Not Informations.IsNothing(vUserGroupCode) Then
                .UserGroupCode = vUserGroupCode
            End If

            If Not Informations.IsNothing(vUserGroupDescription) Then
                .UserGroupDescription = vUserGroupDescription
                .CaptionID = lCaptionID
            End If

            If Not Informations.IsNothing(vIsDeleted) Then
                .IsDeleted = vIsDeleted
            End If

            If Not Informations.IsNothing(vEffectiveDate) Then

                .EffectiveDate = CDate(vEffectiveDate)
            End If

            If Not Informations.IsNothing(vIncluded) Then
                .Included = vIncluded
            End If

            If Not Informations.IsNothing(vIsSysAdminGroup) Then
                .IsSysAdminGroup = vIsSysAdminGroup
            End If

            If Not Informations.IsNothing(vIsSupervisor) Then
                .IsSupervisor = vIsSupervisor
            End If

            .DatabaseStatus = iStatus

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Private)
    '
    ' Description: Returns the supplied PMUserGroup property values.
    '
    ' ***************************************************************** '
    'Developer Guide No.101
    'Private Function GetProperties(ByRef oPMUserGroup As bPMUserGroup.PMUserGroup, ByRef vStatus As gPMConstants.PMEComponentAction, ByRef vUserOrGroup As String, ByRef vPMUserGroupId As Integer, ByRef vUserGroupCode As String, ByRef vUserGroupDescription As String, ByRef vIsDeleted As gPMConstants.PMEVarTrueFalse, ByRef vEffectiveDate As Object, ByRef vIncluded As Integer, ByRef vIsSysAdminGroup As Integer, ByRef vIsSupervisor As Double) As Integer
    Private Function GetProperties(ByRef oPMUserGroup As bPMUserGroup.PMUserGroup, ByRef vStatus As gPMConstants.PMEComponentAction, ByRef vUserOrGroup As Object, ByRef vPMUserGroupId As Object, ByRef vUserGroupCode As Object, ByRef vUserGroupDescription As Object, ByRef vIsDeleted As gPMConstants.PMEVarTrueFalse, ByRef vEffectiveDate As Object, ByRef vIncluded As Object, ByRef vIsSysAdminGroup As Object, ByRef vIsSupervisor As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set Property values.
        With oPMUserGroup

            If True Then
                vUserOrGroup = .UserOrGroup
            End If

            If True Then
                vPMUserGroupId = .UserOrGroupID
            End If

            If True Then
                vUserGroupCode = .UserGroupCode
            End If

            If True Then
                vUserGroupDescription = .UserGroupDescription
            End If

            If True Then
                vIsDeleted = .IsDeleted
            End If

            If True Then

                vEffectiveDate = .EffectiveDate
            End If

            If True Then
                vIncluded = .Included
            End If

            If True Then
                vIsSysAdminGroup = .IsSysAdminGroup
            End If

            If True Then
                vIsSupervisor = .IsSupervisor
            End If

            If True Then
                vStatus = .DatabaseStatus
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetTaskProperties (Private)
    '
    ' Description: Returns the supplied PMGroupTask property values.
    '
    ' ***************************************************************** '
    'Developer Guide No.101
    'Private Function GetTaskProperties(ByRef oPMGroupTask As bPMUserGroup.PMGroupTask, ByRef vStatus As gPMConstants.PMEComponentAction, ByRef vPMTaskGroupId As Integer, ByRef vTaskGroupCode As String, ByRef vTaskGroupDescription As String, ByRef vIsDeleted As Integer, ByRef vEffectiveDate As Object, ByRef vIncluded As Integer) As Integer
    Private Function GetTaskProperties(ByRef oPMGroupTask As bPMUserGroup.PMGroupTask, ByRef vStatus As gPMConstants.PMEComponentAction, ByRef vPMTaskGroupId As Object, ByRef vTaskGroupCode As Object, ByRef vTaskGroupDescription As Object, ByRef vIsDeleted As Object, ByRef vEffectiveDate As Object, ByRef vIncluded As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set Property values.
        With oPMGroupTask

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
                vIncluded = .Included
            End If

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
    Private Function AddInputParam(ByRef oPMUserGroup As bPMUserGroup.PMUserGroup) As Integer

        Dim result As Integer = 0
        Dim sCaption As String = ""
        Dim lCaptionID As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            sCaption = oPMUserGroup.UserGroupDescription


            m_lError = m_oCaption.GetCaptionID(v_sCaption:=sCaption, r_lCaptionId:=lCaptionID)

            m_lError = .Parameters.Add(sName:="caption_id", vValue:=CStr(lCaptionID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="code", vValue:=oPMUserGroup.UserGroupCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="description", vValue:=oPMUserGroup.UserGroupDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="is_deleted", vValue:=CStr(oPMUserGroup.IsDeleted), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Developer Guide No. 40
            'm_lError = .Parameters.Add(sName:="effective_date", vValue:=DateTimeHelper.ToString(oPMUserGroup.EffectiveDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMDate)
            m_lError = .Parameters.Add(sName:="effective_date", vValue:=oPMUserGroup.EffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' CTAF 181200 - Not used so removed

            '        m_lError = .Parameters.Add( _
            ''              sName:="included", _
            ''              vValue:=oPMUserGroup.Included, _
            ''              iDirection:=PMParamInput, _
            ''              iDatatype:=PMInteger)
            '
            '        If (m_lError <> PMTrue) Then
            '            AddInputParam = PMFalse
            '            Exit Function
            '        End If

            m_lError = .Parameters.Add(sName:="is_sys_admin_group", vValue:=CStr(oPMUserGroup.IsSysAdminGroup), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' CTAF 181200 - Not used so removed

            '        m_lError = .Parameters.Add( _
            ''              sName:="is_supervisor", _
            ''              vValue:=oPMUserGroup.IsSupervisor, _
            ''              iDirection:=PMParamInput, _
            ''              iDatatype:=PMInteger)
            '
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
    ' ***************************************************************** '
    'Developer Guide No.101
    'Private Function ValidateParameters(Optional ByRef vUserOrGroup As gPMConstants.PMEReturnCode = 0, Optional ByRef vPMUserGroupId As Object = Nothing, Optional ByRef vUserGroupCode As Object = Nothing, Optional ByRef vUserGroupDescription As Object = Nothing, Optional ByRef vIsDeleted As Byte = 0, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vIncluded As Object = Nothing, Optional ByRef vIsSysAdminGroup As Object = Nothing, Optional ByRef vIsSupervisor As Object = Nothing) As Integer
    Private Function ValidateParameters(Optional ByRef vUserOrGroup As Object = Nothing, Optional ByRef vPMUserGroupId As Object = Nothing, Optional ByRef vUserGroupCode As Object = Nothing, Optional ByRef vUserGroupDescription As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vIncluded As Object = Nothing, Optional ByRef vIsSysAdminGroup As Object = Nothing, Optional ByRef vIsSupervisor As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue


        If Not Informations.IsNothing(vUserOrGroup) Then

        End If


        If Not Informations.IsNothing(vPMUserGroupId) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vPMUserGroupId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                ValidateParameters(gPMConstants.PMEReturnCode.PMFalse)

                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateParameters vPMUserGroupId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateParameters", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If
        End If


        If Not Informations.IsNothing(vUserGroupCode) Then

        End If


        If Not Informations.IsNothing(vUserGroupDescription) Then

        End If


        If Not Informations.IsNothing(vIsDeleted) Then
            If vIsDeleted <> 0 And vIsDeleted <> 1 Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateParameters vIsDeleted Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateParameters", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If
        End If


        If Not Informations.IsNothing(vEffectiveDate) Then
            If Not Informations.IsDate(vEffectiveDate) Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateParameters vEffectiveDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateParameters", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If
        End If


        If Not Informations.IsNothing(vIncluded) Then

        End If


        If Not Informations.IsNothing(vIsSysAdminGroup) Then

        End If


        If Not Informations.IsNothing(vIsSupervisor) Then

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DefaultMissingProperties (Private)
    '
    ' Description: Checks that all mandatory paramaters have been
    '               supplied and sets defaults for the non mandatory ones
    '
    ' ***************************************************************** '
    'Developer Guide No.101
    'Private Function DefaultMissingParameters(Optional ByRef vUserOrGroup As String = "", Optional ByRef vPMUserGroupId As Byte = 0, Optional ByRef vUserGroupCode As Object = Nothing, Optional ByRef vUserGroupDescription As Object = Nothing, Optional ByRef vIsDeleted As Byte = 0, Optional ByRef vEffectiveDate As Date = #12/30/1899#, Optional ByRef vIncluded As Byte = 0, Optional ByRef vIsSysAdminGroup As Byte = 0, Optional ByRef vIsSupervisor As Byte = 0) As Integer
    Private Function DefaultMissingParameters(Optional ByRef vUserOrGroup As Object = Nothing, Optional ByRef vPMUserGroupId As Object = Nothing, Optional ByRef vUserGroupCode As Object = Nothing, Optional ByRef vUserGroupDescription As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vIncluded As Object = Nothing, Optional ByRef vIsSysAdminGroup As Object = Nothing, Optional ByRef vIsSupervisor As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'set defaults for any properties which have not been supplied

        If Informations.IsNothing(vUserOrGroup) Then
            vUserOrGroup = "group"
        End If


        If Informations.IsNothing(vPMUserGroupId) Then
            vPMUserGroupId = 1
        End If


        If Informations.IsNothing(vIsDeleted) Then
            vIsDeleted = 0
        End If


        If Informations.IsNothing(vEffectiveDate) Then
            vEffectiveDate = DateTime.Now
        End If


        If Informations.IsNothing(vIncluded) Then
            vIncluded = 0
        End If


        If Informations.IsNothing(vIsSysAdminGroup) Then
            vIsSysAdminGroup = 0
        End If


        If Informations.IsNothing(vIsSupervisor) Then
            vIsSupervisor = 0
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: MandatoryParameterCheck (Private)
    '
    ' Description: Checks that all mandatory paramaters have been
    '               supplied
    '
    ' ***************************************************************** '

    Private Function MandatoryParameterCheck(Optional ByRef vUserOrGroup As Object = Nothing, Optional ByRef vPMUserGroupId As Object = Nothing, Optional ByRef vUserGroupCode As Object = Nothing, Optional ByRef vUserGroupDescription As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vIncluded As Object = Nothing, Optional ByRef vIsSysAdminGroup As Object = Nothing, Optional ByRef vIsSupervisor As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'check that all mandatory parameters have been supplied

        If Informations.IsNothing(vUserGroupCode) Or Informations.IsNothing(vUserGroupDescription) Then

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Mandatory Property Was Not Supplied", vApp:=ACApp, vClass:=ACClass, vMethod:="MandatoryParameterCheck", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

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
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSysAdminStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSysAdminStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
End Class
