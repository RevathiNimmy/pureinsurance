Option Strict Off
Option Explicit On
'developer guide no. 129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("User_NET.User")> _
Public NotInheritable Class User
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: User
    '
    ' Date: 01/07/1997
    '
    ' Description: Creatable User class which contains all the
    '              methods, Form rules required to manipulate
    '              a PMLock.
    '
    ' Edit History: Original created 01/07/1997 TF
    '               M3 Keys & Constants updated 01/08/1997
    '               PW091105 - changes required for SAM solution
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
    Private Const ACClass As String = "User"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Status members
    Private m_sProcessStatus As New StringsHelper.FixedLengthString(2)
    Private m_sMapStatus As New StringsHelper.FixedLengthString(2)
    Private m_sStepStatus As New StringsHelper.FixedLengthString(2)
    ' PRIVATE Data Members (End)

    ' ***************************************************************** '
    ' Name: CheckTSAndLock (Public)
    '
    ' Description: Checks the timestamp for the supplied key
    '              and locks the entity if it matches.
    ' ***************************************************************** '
    Public Function CheckTSAndLock(ByVal v_sKeyName As String, ByVal v_lKeyValue As Integer, ByVal v_iUserID As Integer, ByVal v_vTimestamp As Object, ByRef r_sCurrentlyLockedBy As String, ByRef r_bTimestampMatches As Boolean) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer
        Dim iMatches As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            With m_oDatabase

                ' Add the Output Params
                m_lReturn = .Parameters.Add(sName:="tstamp_matches", vValue:=CStr(iMatches), idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Add the Input Params
                m_lReturn = .Parameters.Add(sName:="lock_name", vValue:=v_sKeyName.Trim(), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="lock_value", vValue:=CStr(v_lKeyValue), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = .Parameters.Add(sName:="tstamp", vValue:=v_vTimestamp, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBinary)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            ' Check the timestamp
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCheckTSSQL, sSQLName:=ACCheckTSName, bStoredProcedure:=ACCheckTSStored, lRecordsAffected:=lRecordsAffected)

            ' Database error encountered
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If


            ' Did it match?
            If (m_oDatabase.Parameters.Item("tstamp_matches").Value) = gPMConstants.PMEReturnCode.PMFalse Then
                ' Didn't match, so return and tell the caller that it didn't
                r_bTimestampMatches = False
                Return gPMConstants.PMEReturnCode.PMRecordChanged
            End If

            ' It matches, so lock
            r_bTimestampMatches = True
            m_lReturn = CType(LockKey(sKeyName:=v_sKeyName, vKeyValue:=v_lKeyValue, iUserID:=v_iUserID, sCurrentlyLockedBy:=r_sCurrentlyLockedBy), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If (r_sCurrentlyLockedBy <> "") And (r_sCurrentlyLockedBy <> "ERROR") Then
                    ' Locked, by another user
                    Return gPMConstants.PMEReturnCode.PMRecordInUse
                Else
                    ' Some other sort of error
                    Return m_lReturn
                End If
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckTSAndLock Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckTSAndLock", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLastUnlockTimestamp (Public)
    '
    ' Description: Returns the LastUnlockTimestamp for the supplied record
    '              and details of who currently has it locked (if anybody).
    ' ***************************************************************** '
    Public Function GetLastUnlockTimestamp(ByVal v_sKeyName As String, ByVal v_lKeyValue As Integer, ByRef r_vTimestamp As Object, ByRef r_bCurrentlyLocked As Boolean, ByRef r_iLockedByUserId As Object, ByRef r_sLockedByUser As String) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            With m_oDatabase

                'Add the Input Params
                m_lReturn = .Parameters.Add(sName:="lock_name", vValue:=v_sKeyName.Trim(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="lock_value", vValue:=CStr(v_lKeyValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            ' Check the timestamp
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTSSQL, sSQLName:=ACGetTSName, bStoredProcedure:=ACGetTSStored)

            ' Database error encountered
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' return the timestamp

            r_vTimestamp = m_oDatabase.Records.Item(0).Fields()("tstamp")

            ' Is it already locked

            If Convert.IsDBNull(m_oDatabase.Records.Item(0).Fields()("currently_locked_by_id")) Or Informations.IsNothing(m_oDatabase.Records.Item(0).Fields()("currently_locked_by_id")) Then

                ' No
                r_bCurrentlyLocked = False
                r_iLockedByUserId = 0
                r_sLockedByUser = ""

            Else

                ' Yes, so return the details of who has it locked.
                r_bCurrentlyLocked = True
                r_iLockedByUserId = m_oDatabase.Records.Item(0).Fields()("currently_locked_by_id")
                r_sLockedByUser = m_oDatabase.Records.Item(0).Fields()("currently_locked_by")

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLastUnlockTimestamp Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLastUnlockTimestamp", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UnlockAndGetTS (Public)
    '
    ' Description: Checks the timestamp for the supplied key
    '              and locks the entity if it matches.
    ' ***************************************************************** '
    Public Function UnlockAndGetTS(ByVal v_sKeyName As String, ByVal v_lKeyValue As Integer, ByVal v_iUserID As Integer, ByRef r_vTimestamp As Object) As Integer

        Dim result As Integer = 0
        Dim bCurrentlyLocked As Boolean
        Dim iLockedByUserID As Integer
        Dim sLockedByUser As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Unlock
            m_lReturn = CType(UnLockKey(sKeyName:=v_sKeyName, vKeyValue:=v_lKeyValue, iUserID:=v_iUserID, bIgnoreNoLock:=False), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Return NotFound if we can't unlock it.
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Get the timestamp
            m_lReturn = CType(GetLastUnlockTimestamp(v_sKeyName:=v_sKeyName, v_lKeyValue:=v_lKeyValue, r_vTimestamp:=r_vTimestamp, r_bCurrentlyLocked:=bCurrentlyLocked, r_iLockedByUserId:=iLockedByUserID, r_sLockedByUser:=sLockedByUser), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockAndGetTS Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockAndGetTS", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function




    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture
        End Get
    End Property

    Public ReadOnly Property StepStatus() As String
        Get

            ' Standard Property.

            ' Return the Steps Status
            Return m_sStepStatus.Value

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

            ' Set UserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            '    Set oComponentServices = New PMServerBusinessCS

            ' Check that we have the right Database for our
            ' product Family
            '    m_lReturn& = oComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, _
            'v_lPMProductFamily:=PMProductFamily, _
            'r_bNewInstanceCreated:=m_bCloseDatabase, _
            'r_oCheckedDatabase:=m_oDatabase, _
            'v_vDatabase:=vDatabase)

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            '    Set oComponentServices = Nothing

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Initialise the Status settings
            m_sProcessStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sMapStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sStepStatus.Value = gPMConstants.PMNavStatusUnknown


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
    Public Function Terminate() As Integer

        Dim result As Integer = 0
        Static bTerminated As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If we have already Terminated then exit

            If bTerminated Then
                Return result
            Else
                bTerminated = True
            End If



            ' If this class opened the database, close it
            If m_bCloseDatabase Then
                ' Close the Database
                m_lReturn = m_oDatabase.CloseDatabase()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Release reference to PM Data Access Object
            m_oDatabase = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Terminate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Terminate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function
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
    ' Name: LockKey (Public)
    '
    ' Description: Add lock record to PMLock table
    '
    ' ***************************************************************** '
    Public Function LockKey(ByVal sKeyName As String, ByVal vKeyValue As Object, ByVal iUserID As Integer, ByRef sCurrentlyLockedBy As String) As Integer
        Return LockKey(sKeyName:=sKeyName, vKeyValue:=vKeyValue, iUserID:=iUserID, sCurrentlyLockedBy:=sCurrentlyLockedBy, v_bOtherUserOnly:=True, vKey2Value:=0, IsSystemLock:=False)
    End Function

    Public Function LockKey(ByVal sKeyName As String, ByVal vKeyValue As Object, ByVal iUserID As Integer, ByRef sCurrentlyLockedBy As String, ByVal v_bOtherUserOnly As Boolean) As Integer
        Return LockKey(sKeyName:=sKeyName, vKeyValue:=vKeyValue, iUserID:=iUserID, sCurrentlyLockedBy:=sCurrentlyLockedBy, v_bOtherUserOnly:=v_bOtherUserOnly, vKey2Value:=0, IsSystemLock:=False)
    End Function

    Public Function LockKey(ByVal sKeyName As String, ByVal vKeyValue As Object, ByVal iUserID As Integer, ByRef sCurrentlyLockedBy As String, ByVal v_bOtherUserOnly As Boolean, ByVal vKey2Value As Byte) As Integer
        Return LockKey(sKeyName:=sKeyName, vKeyValue:=vKeyValue, iUserID:=iUserID, sCurrentlyLockedBy:=sCurrentlyLockedBy, v_bOtherUserOnly:=v_bOtherUserOnly, vKey2Value:=vKey2Value, IsSystemLock:=False)
    End Function

    Public Function LockKey(ByVal sKeyName As String, ByVal vKeyValue As Object, ByVal iUserID As Integer, ByRef sCurrentlyLockedBy As String, ByVal v_bOtherUserOnly As Boolean, ByVal vKey2Value As Byte, ByVal IsSystemLock As Boolean) As Integer

        Dim nResult As Integer = 0
        Dim nRecordsAffected As Integer
        Dim nCurrentlyLockedByID As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            With m_oDatabase

                ' Add the Output Params
                m_lReturn = .Parameters.Add(sName:="locked_by", vValue:=sCurrentlyLockedBy, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .Parameters.Add(sName:="locked_by_id", vValue:=nCurrentlyLockedByID, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'Add the Input Params
                m_lReturn = .Parameters.Add(sName:="key_name", vValue:=sKeyName.Trim(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = .Parameters.Add(sName:="key_value", vValue:=CStr(vKeyValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="user_id", vValue:=CStr(iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If v_bOtherUserOnly Then
                    'Devlopment Work For Insurer Payment Locking
                    m_lReturn = .Parameters.Add(sName:="key2_value", vValue:=CStr(vKey2Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                If IsSystemLock = True Then
                    m_lReturn = .Parameters.Add(sName:="is_system_lock",
                                                vValue:=CStr(1),
                                                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                iDataType:=gPMConstants.PMEDataType.PMInteger)
                Else
                    m_lReturn = .Parameters.Add(sName:="is_system_lock",
                                                vValue:=CStr(0),
                                                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                iDataType:=gPMConstants.PMEDataType.PMInteger)
                End If

            End With

            If v_bOtherUserOnly Then
                ' Execute Add PMLock SQL Statement
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddPMLockSQL,
                                                  sSQLName:=ACAddPMLockName,
                                                  bStoredProcedure:=ACAddPMLockStored,
                                                  lRecordsAffected:=nRecordsAffected)
            Else
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddPMLockSQL2,
                                                  sSQLName:=ACAddPMLockName2,
                                                  bStoredProcedure:=ACAddPMLockStored2,
                                                  lRecordsAffected:=nRecordsAffected)
            End If

            ' Database error encountered
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMError
                ' Don't exit yet as need to check for
                ' failure on Primary Key Constraint
                ' Exit Function
            End If

            ' Check value of return parameter
            If Convert.IsDBNull(m_oDatabase.Parameters.Item("locked_by").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("locked_by").Value) Then
                ' Lock was successful
                sCurrentlyLockedBy = ""
                Return gPMConstants.PMEReturnCode.PMTrue
            Else
                sCurrentlyLockedBy = m_oDatabase.Parameters.Item("locked_by").Value
                nCurrentlyLockedByID = m_oDatabase.Parameters.Item("locked_by_id").Value
                'If iUserID = nCurrentlyLockedByID Then
                '    Return gPMConstants.PMEReturnCode.PMTrue
                'End If
                Select Case sCurrentlyLockedBy
                    Case Is = "ERROR"
                        ' Database error encountered
                        Return gPMConstants.PMEReturnCode.PMError
                        ' User already holds lock
                    Case Else
                        Return gPMConstants.PMEReturnCode.PMFalse
                End Select
            End If

        Catch ex As Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockKey Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockKey", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
            Return nResult
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: UnLockKey (Public)
    '
    ' Description: Remove lock record from PMLock table
    '
    ' ***************************************************************** '
    Public Function UnLockKey(ByVal sKeyName As String, ByVal vKeyValue As Object, ByVal iUserID As Integer) As Integer
        Return UnLockKey(sKeyName:=sKeyName, vKeyValue:=vKeyValue, iUserID:=iUserID, bIgnoreNoLock:=True, vKey2Value:=0)
    End Function

    Public Function UnLockKey(ByVal sKeyName As String, ByVal vKeyValue As Object, ByVal iUserID As Integer, ByVal bIgnoreNoLock As Boolean) As Integer
        Return UnLockKey(sKeyName:=sKeyName, vKeyValue:=vKeyValue, iUserID:=iUserID, bIgnoreNoLock:=bIgnoreNoLock, vKey2Value:=0)
    End Function

    Public Function UnLockKey(ByVal sKeyName As String, ByVal vKeyValue As Object, ByVal iUserID As Integer, ByRef vKey2Value As Byte) As Integer
        Return UnLockKey(sKeyName:=sKeyName, vKeyValue:=vKeyValue, iUserID:=iUserID, bIgnoreNoLock:=True, vKey2Value:=vKey2Value)
    End Function

    Public Function UnLockKey(ByVal sKeyName As String, ByVal vKeyValue As Object, ByVal iUserID As Integer, ByVal bIgnoreNoLock As Boolean, ByRef vKey2Value As Byte) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            With m_oDatabase

                'Add the Input Params
                m_lReturn = .Parameters.Add(sName:="key_name", vValue:=sKeyName.Trim(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = .Parameters.Add(sName:="key_value", vValue:=CStr(vKeyValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="user_id", vValue:=CStr(iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Devlopment Work For Insurer Payment Locking
                If bIgnoreNoLock Then
                    m_lReturn = .Parameters.Add(sName:="key2_value", vValue:=CStr(vKey2Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Else
                    m_lReturn = .Parameters.Add(sName:="key2_value", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

            End With

            ' Execute Delete PMLock SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeletePMLockSQL, sSQLName:=ACDeletePMLockName, bStoredProcedure:=ACDeletePMLockStored, lRecordsAffected:=lRecordsAffected)

            ' Database error encountered
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' Set return value
            Select Case lRecordsAffected
                ' Lock removed successfully
                Case Is = 1
                    result = gPMConstants.PMEReturnCode.PMTrue
                    ' No lock removed
                Case Is = 0
                    ' PW091105 - Add optional param to prohibit error
                    ' when unlocking and the lock has gone...
                    If Not bIgnoreNoLock Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If
                    ' Database error encountered
                Case Else
                    result = gPMConstants.PMEReturnCode.PMError
            End Select


            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnLockKey Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnLockKey", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetStatus (Standard Method)
    '
    ' Description: Set the Process, Map and Step status.
    ' Note:        A Property Get is provided for the Step Status only
    '              as this is the only one which this component can
    '              alter directly.
    ' ***************************************************************** '
    Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Start)

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (BeginTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function BeginTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLBeginTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CommitTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CommitTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLCommitTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (RollbackTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function RollbackTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLRollbackTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
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


    Public Function UnLockKey(ByVal sKeyName As String,
                                  ByVal vKeyValue As Object,
                                  ByVal bDeleteSystemLock As Boolean) As Integer

        Dim nResult As Integer = 0
        Dim nRecordsAffected As Integer

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()

            With m_oDatabase


                m_lReturn = .Parameters.Add(sName:="key_name",
                                            vValue:=sKeyName,
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = .Parameters.Add(sName:="key_value",
                                            vValue:=CStr(vKeyValue),
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If bDeleteSystemLock = True Then
                    m_lReturn = .Parameters.Add(sName:="Delete_System_lock",
                                                vValue:=CStr(1),
                                                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End With

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeletePMLockKeySQL,
                                              sSQLName:=ACDeletePMLockKeyName,
                                              bStoredProcedure:=ACDeletePMLockKeyStored,
                                              lRecordsAffected:=nRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' Set return value
            Select Case nRecordsAffected
                ' Lock removed successfully
                Case Is = 1
                    nResult = gPMConstants.PMEReturnCode.PMTrue
                    ' No lock removed
                Case Is = 0
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    ' Database error encountered
                Case Else
                    nResult = gPMConstants.PMEReturnCode.PMError
            End Select

            Return nResult

        Catch ex As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnLockKey Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnLockKey", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)

            Return nResult

        End Try

    End Function



End Class