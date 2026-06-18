Option Strict Off
Option Explicit On
'Developer Guide No. 129
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Lookup_NET.Lookup")>
Public NotInheritable Class Lookup
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: LookupControl
    '
    ' Date: 22nd October 1998
    '
    ' Description: This class is used by the PMUserGroupLookup control.
    '
    ' Edit History:
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
    Private Const ACClass As String = "LookupControl"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)
    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_bRestrictUserList As Boolean
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    ' RFC250398 - Product Family Property Get Added.
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

        ' RDC 13062002 CompServ replaced with BAS module
        'Dim oComponentServices As PMServerBusinessCS

        Dim result As Integer = 0
        Try
            Dim bRestrictedUsers, bMultiTreeAcc As Boolean
            Dim vValue As Byte

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

            '    m_lReturn = oComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, _
            'v_lPMProductFamily:=PMProductFamily, _
            'r_bNewInstanceCreated:=m_bCloseDatabase, _
            'r_oCheckedDatabase:=m_oDatabase, _
            'v_vDatabase:=vDatabase)

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            '    Set oComponentServices = Nothing

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' SET 18/04/2007
            m_lReturn = CType(bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, v_vBranch:=m_iSourceID, r_vUnderwriting:=CStr(vValue)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            End If

            If vValue = 1 Then
                bMultiTreeAcc = True
            End If

            m_lReturn = CType(bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiCoWorkManagerTaskRestriction, v_vBranch:=m_iSourceID, r_vUnderwriting:=CStr(vValue)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue (Restricted Client View) Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            If vValue = 1 Then
                bRestrictedUsers = True
            End If

            If bRestrictedUsers And bMultiTreeAcc Then
                m_bRestrictUserList = True
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
    ' Name: GetAllEffectiveUsers
    '
    ' Description: Return the user_id and user_name of all effective
    '              users. i.e. Those that are not deleted.
    '
    ' ***************************************************************** '
    'Developer Guide No. 17
    'Public Function GetAllEffectiveUsers(ByVal v_dtEffectiveDate As Date, ByRef r_vAllUsersArray As String) As Integer
    Public Function GetAllEffectiveUsers(ByVal v_dtEffectiveDate As Date, ByRef r_vAllUsersArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lRecords As Integer = gPMConstants.PMAllRecords
        Dim vValue As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Parameters
                .Parameters.Clear()
                'Developer Guide No. 40
                'm_lReturn = .Parameters.Add(sName:="effective_date", vValue:=DateTimeHelper.ToString(v_dtEffectiveDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMDate)
                m_lReturn = .Parameters.Add(sName:="effective_date", vValue:=v_dtEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If m_bRestrictUserList Then
                    vValue = m_iSourceID
                Else

                    vValue = Nothing
                End If

                m_lReturn = .Parameters.Add(sName:="source_id", vValue:=vValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If m_bRestrictUserList Then
                    vValue = 1
                Else

                    vValue = Nothing
                End If

                m_lReturn = .Parameters.Add(sName:="RestrictUserList", vValue:=vValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLSelect(sSQL:=ACAllUsersLookupSQL, sSQLName:=ACAllUsersLookupName, bStoredProcedure:=ACAllUsersLookupStored, vResultArray:=r_vAllUsersArray, lNumberRecords:=lRecords)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            ' If there aren't any users then just return an empty string
            If lRecords < 1 Then
                r_vAllUsersArray = Nothing
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllEffectiveUsersFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllEffectiveUsers", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetGroupEffectiveUsers
    '
    ' Description: Return the user_id and user_name of all effective
    '              users in a Group. i.e. Those that are not deleted.
    '
    ' ***************************************************************** '
    'Developer Guide No. 17
    Public Function GetGroupEffectiveUsers(ByVal v_lPMUserGroupID As Integer, ByVal v_dtEffectiveDate As Date, ByRef r_vGroupUsersArray(,) As Object) As Integer
        Return GetGroupEffectiveUsers(v_lPMUserGroupID:=v_lPMUserGroupID, v_dtEffectiveDate:=v_dtEffectiveDate, r_vGroupUsersArray:=r_vGroupUsersArray, v_iPartyCnt:=0)
    End Function

    Public Function GetGroupEffectiveUsers(ByVal v_lPMUserGroupID As Integer, ByVal v_dtEffectiveDate As Date, ByRef r_vGroupUsersArray(,) As Object, ByVal v_iPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecords As Integer = gPMConstants.PMAllRecords
        Dim vValue As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Parameters
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="pmuser_group_id", vValue:=CStr(v_lPMUserGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'Developer Guide No. 40
                'm_lReturn = .Parameters.Add(sName:="effective_date", vValue:=DateTimeHelper.ToString(v_dtEffectiveDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMDate)
                m_lReturn = .Parameters.Add(sName:="effective_date", vValue:=v_dtEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If m_bRestrictUserList Then
                    vValue = m_iSourceID
                Else

                    vValue = Nothing
                End If

                m_lReturn = .Parameters.Add(sName:="source_id", vValue:=CStr(vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If m_bRestrictUserList Then
                    vValue = 1
                Else

                    vValue = Nothing
                End If

                m_lReturn = .Parameters.Add(sName:="RestrictUserList", vValue:=CStr(vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If v_iPartyCnt <> 0 Then
                    m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=v_iPartyCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                m_lReturn = .SQLSelect(sSQL:=ACGroupUsersLookupSQL, sSQLName:=ACGroupUsersLookupName, bStoredProcedure:=ACGroupUsersLookupStored, vResultArray:=r_vGroupUsersArray, lNumberRecords:=lRecords)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            ' If there aren't any users then just return an empty string
            If lRecords < 1 Then
                r_vGroupUsersArray = Nothing
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetGroupEffectiveUsersFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGroupEffectiveUsers", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAllEffectiveGroups
    '
    ' Description: Return the group_id and caption of all effective
    '              groups. i.e. Those that are not deleted.
    '
    ' ***************************************************************** '
    'Developer Guide No. 17
    'Public Function GetAllEffectiveGroups(ByVal v_dtEffectiveDate As Date, ByRef r_vUserGroupsArray As String) As Integer
    Public Function GetAllEffectiveGroups(ByVal v_dtEffectiveDate As Date, ByRef r_vUserGroupsArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lRecords As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Parameters
                .Parameters.Clear()
                'Developer Guide No. 40
                'm_lReturn = .Parameters.Add(sName:="effective_date", vValue:=DateTimeHelper.ToString(v_dtEffectiveDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMDate)
                m_lReturn = .Parameters.Add(sName:="effective_date", vValue:=v_dtEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="language_id", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLSelect(sSQL:=ACAllUserGroupsLookupSQL, sSQLName:=ACAllUserGroupsLookupName, bStoredProcedure:=ACAllUserGroupsLookupStored, vResultArray:=r_vUserGroupsArray, lNumberRecords:=lRecords)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            ' If there aren't any users then just return an empty string
            If lRecords < 1 Then
                r_vUserGroupsArray = Nothing
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllEffectiveGroups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllEffectiveGroups", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAllEffectiveGroupsByTask
    '
    ' Description: Return the group_id and caption of all effective
    '              groups in a Task Group. i.e. Those that are not deleted.
    '
    ' ***************************************************************** '
    'Developer Guide No. 17
    'Public Function GetAllEffectiveGroupsByTask(ByVal v_lPMTaskGroupID As Integer, ByVal v_dtEffectiveDate As Date, ByRef r_vUserGroupsArray As String) As Integer
    Public Function GetAllEffectiveGroupsByTask(ByVal v_lPMTaskGroupID As Object, ByVal v_dtEffectiveDate As Object, ByRef r_vUserGroupsArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lRecords As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Parameters
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="pmwrk_task_group_id", vValue:=v_lPMTaskGroupID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'Developer Guide No. 40
                'm_lReturn = .Parameters.Add(sName:="effective_date", vValue:=DateTimeHelper.ToString(v_dtEffectiveDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMDate)
                m_lReturn = .Parameters.Add(sName:="effective_date", vValue:=v_dtEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="language_id", vValue:=m_iLanguageID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLSelect(sSQL:=ACUserGroupsByTaskLookupSQL, sSQLName:=ACUserGroupsByTaskLookupName, bStoredProcedure:=ACUserGroupsByTaskLookupStored, vResultArray:=r_vUserGroupsArray, lNumberRecords:=lRecords)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            ' If there aren't any users then just return an empty string
            If lRecords < 1 Then
                r_vUserGroupsArray = Nothing
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllEffectiveGroupsByTask Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllEffectiveGroupsByTask", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAllEffectiveGroupsByUser
    '
    ' Description: Return the group_id and caption of all effective
    '              groups a user belongs to. i.e. Those that are not deleted.
    '
    ' ***************************************************************** '
    'Developer Guide No. 17
    'Public Function GetAllEffectiveGroupsByUser(ByVal v_lPMUserID As Integer, ByVal v_dtEffectiveDate As Date, ByRef r_vUserGroupsArray As String) As Integer
    Public Function GetAllEffectiveGroupsByUser(ByVal v_lPMUserID As Integer, ByVal v_dtEffectiveDate As Date, ByRef r_vUserGroupsArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lRecords As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Parameters
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="user_id", vValue:=CStr(v_lPMUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'Developer Guide No. 40
                'm_lReturn = .Parameters.Add(sName:="effective_date", vValue:=DateTimeHelper.ToString(v_dtEffectiveDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMDate)
                m_lReturn = .Parameters.Add(sName:="effective_date", vValue:=v_dtEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="language_id", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLSelect(sSQL:=ACUserGroupsByUserLookupSQL, sSQLName:=ACUserGroupsByUserLookupName, bStoredProcedure:=ACUserGroupsByUserLookupStored, vResultArray:=r_vUserGroupsArray, lNumberRecords:=lRecords)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            ' If there aren't any users then just return an empty string
            If lRecords < 1 Then
                r_vUserGroupsArray = Nothing

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllEffectiveGroupsByUser Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllEffectiveGroupsByUser", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetUsersGroups
    '
    ' Description: Returns all the Groups that the User is a member of.
    '
    '
    ' ***************************************************************** '
    'Developer Guide No. 17
    'Public Function GetUsersGroups(ByVal v_iUserID As Integer, ByVal v_dtEffectiveDate As Date, ByRef r_vUsersGroupsArray As String) As Integer
    Public Function GetUsersGroups(ByVal v_iUserID As Integer, ByVal v_dtEffectiveDate As Date, ByRef r_vUsersGroupsArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lRecords As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="user_id", vValue:=CStr(v_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="language_id", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'Developer Guide No. 40
                'm_lReturn = .Parameters.Add(sName:="effective_date", vValue:=DateTimeHelper.ToString(v_dtEffectiveDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMDate)
                m_lReturn = .Parameters.Add(sName:="effective_date", vValue:=v_dtEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLSelect(sSQL:=ACUsersGroupsLookupSQL, sSQLName:=ACUsersGroupsLookupName, bStoredProcedure:=ACUsersGroupsLookupStored, vResultArray:=r_vUsersGroupsArray, lNumberRecords:=lRecords)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            ' If there aren't any users then just return an empty string
            If lRecords < 1 Then
                r_vUsersGroupsArray = Nothing
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUsersGroupsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUsersGroups", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

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
        'bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
