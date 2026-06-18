Option Strict Off
Option Explicit On
'Developer Guide No. 129
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Utilities_NET.Utilities")>
Public NotInheritable Class Utilities
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Utilities
    '
    ' Date: 24th November 1998
    '
    ' Description: Creatable Utilities class which contains all the
    '              methods, business rules required to manipulate
    '              a PMUserGroup.
    '
    ' Edit History:
    ' DAK01091999 - Addition of 2 new functions to check whether
    '               a user is a member of a group.
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
    Private Const ACClass As String = "Utilities"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lError As gPMConstants.PMEReturnCode

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture

        End Get
    End Property

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

            '    Set oComponentServices = Nothing

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
    ' Name: IsUserSupervisorOf Group
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function IsUserSupervisorOfGroup(ByVal v_iUserID As Integer, ByVal v_lGroupId As Integer, ByRef r_bIsSupervisor As Boolean) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the user_id parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(v_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the group_id parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="pmuser_group_id", vValue:=CStr(v_lGroupId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACIsSupervisorSQL, sSQLName:=ACIsSupervisorName, bStoredProcedure:=ACIsSupervisorStored, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            If lRecordCount = 0 Then
                r_bIsSupervisor = False
                Return result
            End If

            ' RDC 21062002 fields now zero-based
            r_bIsSupervisor = (m_oDatabase.Records.Item(1).Fields()(0) = 1)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsUserSupervisorOfGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsUserSupervisorOfGroup", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: IsUserAdministrator
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function IsUserAdministrator(ByVal v_iUserID As Integer, ByRef r_bIsAdministrator As Boolean) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the user_id parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(v_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the effective_date parameter (INPUT)
            'Developer Guide No. 40
            m_lError = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACIsSystemAdministratorSQL, sSQLName:=ACIsSystemAdministratorName, bStoredProcedure:=ACIsSystemAdministratorStored, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            If lRecordCount = 0 Then
                r_bIsAdministrator = False
                Return result
            End If

            ' RDC 21062002 fields now zero-based
            'NIIT - Replaced with the Migrated code 1144 
            'r_bIsAdministrator = (m_oDatabase.Records.Item(1).Fields()(0) > 0)
            r_bIsAdministrator = (CInt(m_oDatabase.Records.Item(1 - 1).Fields.Item(0)) > 0)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsUserAdministrator Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsUserAdministrator", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetGroupsSupervisedByUser
    '
    ' Description: Returns All of the Groups that the supplied User
    '              is a Supervisor Of.
    '
    ' ***************************************************************** '
    'Developer Guide No. 17
    Public Function GetGroupsSupervisedByUser(ByVal v_iUserID As Integer, ByRef r_vSupervisedGroups(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lError = .Parameters.Add(sName:="user_id", vValue:=CStr(v_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'Developer Guide No. 40
                m_lError = .Parameters.Add(sName:="effective_date", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lError = .SQLSelect(sSQL:=ACUserSupervisesGroupsSQL, sSQLName:=ACUserSupervisesGroupsName, bStoredProcedure:=ACUserSupervisesGroupsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vSupervisedGroups)
                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not Informations.IsArray(r_vSupervisedGroups) Then
                    r_vSupervisedGroups = Nothing
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetGroupsSupervisedByUserFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGroupsSupervisedByUser", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: IsUserIdMemberOf Group
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function IsUserIdMemberOfGroup(ByVal v_iUserID As Integer, ByVal v_sGroupCode As String, ByRef r_bUserIsMember As Boolean) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            r_bUserIsMember = False
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the user_id parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(v_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the group_id parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="group_code", vValue:=v_sGroupCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACIsMemberIdSQL, sSQLName:=ACIsMemberIdName, bStoredProcedure:=ACIsMemberIdStored, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            If lRecordCount > 0 Then
                r_bUserIsMember = True
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsUserIdMemberOfGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsUserIdMemberOfGroup", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: IsUserNameMemberOf Group
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function IsUserNameMemberOfGroup(ByVal v_sUserName As String, ByVal v_sGroupCode As String, ByRef r_bUserIsMember As Boolean) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            r_bUserIsMember = False
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the user_Name parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="username", vValue:=v_sUserName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the group_Name parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="group_code", vValue:=v_sGroupCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACIsMemberNameSQL, sSQLName:=ACIsMemberNameName, bStoredProcedure:=ACIsMemberNameStored, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            If lRecordCount > 0 Then
                r_bUserIsMember = True
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsUserNameMemberOfGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsUserNameMemberOfGroup", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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

End Class
