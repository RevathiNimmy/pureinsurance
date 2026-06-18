Option Strict Off
Option Explicit On
Imports System.Globalization
Imports System.Text
Imports System.Xml
Imports System.IO
Imports System.Security.Cryptography
Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports Microsoft.Practices.EnterpriseLibrary.Caching.Expirations
Imports System.Data
Imports System.Data.SqlClient


<Serializable()>
<System.Runtime.InteropServices.ProgId("Database_NET.Database")>
Public Class Database


#Region "Private Constants"
    Private Const ACClass As String = "Database"
    Private Const kScope As DataProtectionScope = DataProtectionScope.LocalMachine
#End Region

#Region "Private Variables"

    Private m_sSiriusUsername As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iLogLevel As Integer

    Private m_bConnectionPooling As Boolean
    Private m_sUsername As String = ""

    ' Records Collection (Read Only)
    Private m_oRecords As dPMDAO.Records
    ' Parameters Collection (Read/Write)
    Private m_oParameters As dPMDAO.Parameters
    Private m_lQueryTimeout As Integer
    ' What version of PMDAO are we supporting
    Private m_lPMDAOVersionMode As Integer

    Private m_oCon As SqlConnection
    Private m_oCmd As SqlCommand

    Private m_sDSN As String = ""
    ' Transaction Nesting Level (Private)
    Private m_iTransactionNestLevel As Integer
    Private m_lReturn As Integer
    Private m_sConnectString As String = ""
    Private m_bDebugMode As Boolean
    Private m_sSQLTrace As String = ""
    Private m_Transaction As SqlTransaction
    Dim filePath As String
    Dim fileStream As FileStream
    Dim streamWriter As StreamWriter
    Private m_sCachePath As String
    Private m_bEnvironmentCreated As Boolean
#End Region
#Region "Public Variables"
    Public Shared iCache As ICacheManager
    Public ConnectionString As String

#End Region


#Region "Public Properties"


    Public ReadOnly Property Records() As dPMDAO.Records
        Get
            Return m_oRecords
        End Get
    End Property

    Public Property Parameters() As dPMDAO.Parameters
        Get
            Return m_oParameters
        End Get
        Set(ByVal Value As dPMDAO.Parameters)
            m_oParameters = Value
        End Set
    End Property


    Public Property CurrentDSN() As String
        Get
            ' Is there a Current Connection
            If m_oCon Is Nothing Then
                If m_bConnectionPooling Then
                    Return m_sDSN
                Else
                    ' No, so return empty string
                    Return ""
                End If
            Else
                ' Yes, so return DSN
                Return m_sDSN
            End If
        End Get
        Set(ByVal Value As String)
            m_sDSN = Value.Trim()
        End Set
    End Property

    Public Property QueryTimeout() As Integer
        Get
            Return m_lQueryTimeout
        End Get
        Set(ByVal Value As Integer)

            ' Note1: Setting this to zero will allow queries to wait
            '        indefinitely which is not a good idea, so we will
            '        no allow it. Use maximum allowed.
            If Value = 0 Then
                Value = ACMaxQueryTimeout
            End If

            If Value > ACMaxQueryTimeout Then
                Value = ACMaxQueryTimeout
            End If

            If Value < ACMinQueryTimeout Then
                Value = ACMinQueryTimeout
            End If

            ' Set the Query Timeout property
            m_lQueryTimeout = Value
        End Set
    End Property

    Public Property PMDAOVersionMode() As String
        Get
            Return CStr(m_lPMDAOVersionMode)
        End Get
        Set(ByVal Value As String)
            ' Anything other than an explicit setting of 2 is mode 3 i.e. Current
            Select Case Value
                Case CStr(2)
                    m_lPMDAOVersionMode = 2
                Case CStr(3)
                    m_lPMDAOVersionMode = 3
                Case CStr(4)
                    m_lPMDAOVersionMode = 4
                Case CStr(5)
                    m_lPMDAOVersionMode = 5
                Case Else
                    m_lPMDAOVersionMode = 5
            End Select
        End Set
    End Property

    Public ReadOnly Property AppVersion() As String
        Get

            Return "" 'CStr(My.Application.Info.Version.Major) & "." & CStr(My.Application.Info.Version.Minor) & "." & CStr(My.Application.Info.Version.Revision)
        End Get
    End Property

    ' Expose the transaction count to aid debugging of nested transaction problems.
    Public ReadOnly Property TransactionNestLevel() As Integer
        Get
            Return m_iTransactionNestLevel
        End Get
    End Property
#End Region

#Region "Private Properties"
    Private Property DebugMode() As Boolean
        Get
            Return m_bDebugMode
        End Get
        Set(ByVal Value As Boolean)
            m_bDebugMode = Value
        End Set
    End Property
#End Region

    ' ***************************************************************** '
    ' Name: OpenDatabase
    '
    ' Description: Open the Sirius Database
    '
    ' Parameters : DSN - Registered ODBC DataSource
    '              Database - Default Database to use once connected.
    '              Username - Recognised user name for the database
    '              Password - Password associated with the username
    ' ***************************************************************** '
#Region "Public Methods"
    Public Function OpenDatabase(ByRef sSiriusUsername As String, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef sCallingAppName As String, Optional ByRef vUsername As String = "", Optional ByRef vPassword As String = "", Optional ByRef vDSN As String = "", Optional ByRef vDatabase As String = "") As Integer
        Dim result As Integer = 0
        Dim sDSN, sDatabase As String
        Dim bKnownDSN As Boolean
        Dim sUsername As String = ""
        Dim sPassword As String = ""
        Dim sProvider As String
        Dim sServer As String
        Dim bTrusted As Boolean
        Dim sSystemName As String = ""
        Dim sContent(1) As String
        Dim sCacheFilename As String = ""
        Dim sFilePath As String = ""

        Const ConnectionStringFrame As String = "Server={server};Database={database};Integrated Security=False; User ID={loginid}; Password={loginpassword}"
        Const ConnectionStringFrameWindowsAuthentication As String = "Server={server};Database={database};Integrated Security=SSPI;"


        Try
            result = PMConstants.PMEReturnCode.PMTrue

            m_sSiriusUsername = sSiriusUsername
            m_iSourceID = iSourceID
            m_iLanguageID = iLanguageID
            m_sCallingAppName = sCallingAppName

            ' Create Environment
            m_lReturn = CreateEnvironment()

            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CloseDatabase()

            ' Update the Username and Password Fields (if supplied)
            If Not String.IsNullOrWhiteSpace(vUsername) Then
                m_sUsername = vUsername
            Else
                m_sUsername = ""
                vUsername = ""
            End If

            'If Not Information.Nothing(vPassword) Then
            '    m_sPassword = vPassword
            'Else
            '    m_sPassword = ""
            '    vPassword = ""
            'End If

            ' If DSN not supplied set to empty string
            If String.IsNullOrWhiteSpace(vDSN) Then
                vDSN = ""
            End If

            ' If Database not supplied set to empty string
            If String.IsNullOrWhiteSpace(vDatabase) Then
                vDatabase = ""
            End If

            sDSN = vDSN.Trim()
            sDatabase = vDatabase.Trim()

            m_lReturn = CheckDSN(r_sDSN:=sDSN, r_sDatabase:=sDatabase, r_bKnownDSN:=bKnownDSN, r_sUserNameUsedToConnect:=sUsername, r_sUserPassUsedToConnect:=sPassword)

            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If

            If Not bKnownDSN Then
                ' No, so use the supplied username & password
                sUsername = vUsername.Trim()
                sPassword = vPassword.Trim()
            End If

            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If
            Dim sConnectionStringKey As String = "BOConnectionString"

            If Not iCache Is Nothing AndAlso iCache.Contains(sConnectionStringKey) AndAlso Not String.IsNullOrEmpty(Convert.ToString(iCache.GetData(sConnectionStringKey))) Then
                m_sConnectString = iCache.GetData(sConnectionStringKey)
            End If

            If m_sConnectString = "" Then


                ' Get the UDL data
                sProvider = CStr(GetConnectionDetails("Provider"))
                sServer = CStr(GetConnectionDetails("Server"))
                sDatabase = CStr(GetConnectionDetails("Database"))
                If GetConnectionDetails("Trusted") Is String.Empty Then
                    bTrusted = False
                Else
                    bTrusted = CBool(GetConnectionDetails("Trusted"))
                End If
                If sProvider = "" Or sServer = "" Or sDatabase = "" Then
                    LogDatabaseError(sSiriusUsername:=m_sSiriusUsername, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, bConnectionPooling:=m_bConnectionPooling, iType:=PMConstants.PMELogLevel.PMLogError, sMsg:="Failed to open database '" & sDSN & "', unable to find database parameter in registry.", vApp:="dPMDAO", vClass:="Database", vMethod:="OpenDatabase")
                    Return PMConstants.PMEReturnCode.PMFalse
                End If
                ' m_lReturn = PMFunctions.GetSystemName(sSystemName)

                ' RFC190603 - Release Connection for connection pooling
                ' Open a Connection Using local data and registry settings
                If bTrusted Then
                    m_sConnectString = ConnectionStringFrameWindowsAuthentication
                Else
                    m_sConnectString = ConnectionStringFrame
                End If

                ' Replace the placeholders with the correct values
                m_sConnectString = m_sConnectString.Replace("{server}", sServer).Replace("{database}", sDatabase)


                If bTrusted Then
                    'Do Nothing
                Else

                    m_sConnectString = m_sConnectString.Replace("{loginid}", sUsername)
                    m_sConnectString = m_sConnectString.Replace("{loginpassword}", sPassword)

                End If
                sCacheFilename = sConnectionStringKey
                'Add Connection String to Cache
                ' Add them to the Cache
                sFilePath = m_sCachePath + sCacheFilename + ".xml"

                If Not System.IO.File.Exists(sFilePath) Then
                    Dim fileIO As FileStream
                    fileIO = File.Create(sFilePath)
                    fileIO.Close()
                    File.WriteAllLines(sFilePath, sContent)
                End If
                '
                ' Sirius Cache Controller

                If Not iCache Is Nothing Then
                    'iCache.Add(sKey, vResultArray, CacheItemPriority.NotRemovable, Nothing, New FileDependency(m_sCachePath + m_sCacheFilename))
                    iCache.Add(sConnectionStringKey, m_sConnectString, CacheItemPriority.Normal, Nothing, New FileDependency(sFilePath))
                End If

            End If

            ' Open the connection
            m_lReturn = CheckDBConnection()
            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set the Current Data Source
            CurrentDSN = sDSN
            ConnectionString = m_sConnectString

            ' Refresh Parameters Collection (Read/Write)
            m_oParameters = New dPMDAO.Parameters()
            m_oParameters.SetGlobalData(sSiriusUsername:=m_sSiriusUsername, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, sCallingAppName:=m_sCallingAppName, iLogLevel:=m_iLogLevel, bConnectionPooling:=m_bConnectionPooling)

            ' Refresh Records Collection
            m_oRecords = New dPMDAO.Records()

            ' Initialise Transaction Nest Level
            m_iTransactionNestLevel = ACTransNestLevelStart

            m_lReturn = CloseDBConnection()
            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If

            Return result
        Catch excep As System.Exception
            CurrentDSN = ""

            ' Error Section.
            result = PMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogDatabaseError(sSiriusUsername:=m_sSiriusUsername, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, bConnectionPooling:=m_bConnectionPooling, iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to open database", vApp:="dPMDAO", vClass:="Database", vMethod:="OpenDatabase", vErrDesc:=excep.Message, vErrSource:=excep.Source, oCon:=m_oCon)

            Return result
        End Try
    End Function
#End Region

    ' ***************************************************************** '
    ' Name: CloseDatabase
    '
    ' Description: Close the Sirius Database
    '
    '
    ' ***************************************************************** '
    Public Function CloseDatabase() As Integer

        Dim result As Integer = 0
        Dim sMsg As String

        Try
            result = PMConstants.PMEReturnCode.PMTrue
            ' Do we have a Connection
            If m_oCon Is Nothing Then
                ' No, so just exit
                Return result
            End If

            ' Is there an outstanding Transaction
            If m_iTransactionNestLevel > ACTransNestLevelStart Then
                ' Yes, there is an outstanding transaction

                sMsg = "***********************************************************************************" & StringCHR() & StringCHR10()
                sMsg = sMsg & "WARNING Database Connection is being closed with an outstanding Transaction." & StringCHR() & StringCHR10()
                sMsg = sMsg & "The Transaction will be rolled back and the updates will be lost." & StringCHR() & StringCHR10()
                sMsg = sMsg & "To see what work is being lost, switch on Architecture Debug Mode" & StringCHR() & StringCHR10()
                sMsg = sMsg & "HKEY_LOCAL_MACHINE\SOFTWARE\PM\SiriusArchitecture\Common\ArchitectureInDebug = 1" & StringCHR() & StringCHR10()
                sMsg = sMsg & "then re-run the Transaction." & StringCHR() & StringCHR10()
                LogDatabaseMessage(iType:=PMConstants.PMELogLevel.PMLogError, sMsg:=sMsg, vApp:=ACApp, vClass:="Database")

                ' Roll It back
                m_iTransactionNestLevel = ACTransNestLevelStart + 1
                m_lReturn = SQLRollbackTrans()
            End If

            ' The SQLRollbackTrans above may have already closed the
            ' connection (If running in COM+ and therefore using connection pooling).
            If Not (m_oCon Is Nothing) Then
                If m_oCon.State <> ConnectionState.Closed Then
                    ' Close the Current Connection
                    m_oCon.Close()
                End If
            End If

            ' Release the Reference
            m_oCon = Nothing
            m_oCmd = Nothing

            ' Reset the Current DSN
            CurrentDSN = ""

            ' Clear Parameters and Records
            m_oRecords = Nothing
            m_oParameters = Nothing

            m_iTransactionNestLevel = ACTransNestLevelStart

            Return result
        Catch excep As System.Exception
            ' Error Section.
            result = PMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogDatabaseError(sSiriusUsername:=m_sSiriusUsername, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, bConnectionPooling:=m_bConnectionPooling, iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="Close Database Failed", vApp:="dPMDAO", vClass:="Database", vMethod:="CloseDatabase", vErrDesc:=excep.Message, vErrSource:=excep.Source, oCon:=m_oCon)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: SQLSelect
    '
    ' Description: Perform the given select statement on the database.
    '
    ' Note       : The bAsync Optional parameter has been
    '              added for future expandability without (hopefully)
    '              breaking version compatibility. It does nothing in
    '              this version.
    ' ***************************************************************** '
    Public Function SQLSelect(ByRef sSQL As String, ByRef sSQLName As String, ByRef bStoredProcedure As Boolean, Optional ByRef lNumberRecords As Integer = 0, Optional ByRef vResultArray As Object = Nothing, Optional ByRef bNoPrepare As Boolean = False, Optional ByRef bKeepQuery As Boolean = False, Optional ByRef bAsync As Boolean = False, Optional ByRef bKeepNulls As Boolean = False) As Integer
        Dim result As Integer = 0
        Try

            Return SelectRecords(sSQL:=sSQL, sSQLName:=sSQLName, bStoredProcedure:=bStoredProcedure, bSelectText:=False, lNumberRecords:=lNumberRecords, vResultArray:=vResultArray, bNoPrepare:=bNoPrepare, bKeepQuery:=bKeepQuery, bAsync:=bAsync, bKeepNulls:=bKeepNulls)
        Catch excep As System.Exception
            ' Error Section.
            result = PMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogDatabaseMessage(iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="SQLSelect Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SQLSelect", vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SQLAction
    '
    ' Description: Execute the Action SQL Statement.
    '
    '
    ' ***************************************************************** '
    Public Function SQLAction(ByRef sSQL As String, ByRef sSQLName As String, ByRef bStoredProcedure As Boolean, Optional ByRef lRecordsAffected As Integer = 0, Optional ByRef bNoPrepare As Boolean = False, Optional ByRef bKeepQuery As Boolean = False, Optional ByRef bAsync As Boolean = False) As Integer
        Dim result As Integer = 0
        Dim sMergedSQL As String = ""
        Dim sDebugSQL As String = ""

        Try
            result = PMConstants.PMEReturnCode.PMTrue

            ' Check the Current Connection
            m_lReturn = CheckDBConnection()
            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If

            ' Validate the params
            If sSQL.Length = 0 Then
                PMFunctions.RaiseError("Validate Params", "sSQL Paramater is an EMPTY String, a valid Stored Procedure or SQL command MUST be supplied.", PMConstants.PMELogLevel.PMLogError)
            End If

            ' Get a Reference to the Command
            m_oCmd = m_oParameters.ADOCommand

            ' Set the Command Active Connection
            m_oCmd.Connection = m_oCon

            ' Set the Command Type
            m_oCmd.CommandType = CommandType.Text

            ' Set the QueryTimouet
            m_oCmd.CommandTimeout = QueryTimeout

            m_oCmd.Transaction = m_Transaction

            ' Is SQL a Stored procedure
            If bStoredProcedure Then
                ' Are we calling a sproc using the NON ODBC style.
                If Not sSQL.Trim().StartsWith("{") Then
                    ' Looks like we have just been given the sproc name so set the ADO Command Type to Stored Proc
                    m_oCmd.CommandType = CommandType.StoredProcedure
                End If

                m_oCmd.CommandText = sSQL
            Else
                ' Merge parameter values with SQL string
                m_lReturn = MergeParameters(sInSQL:=sSQL, sOutSQL:=sMergedSQL)

                If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                    Return PMConstants.PMEReturnCode.PMFalse
                End If

                ' Update the Queries SQL String
                m_oCmd.CommandText = sMergedSQL
            End If
            If DebugMode Then
                If m_iTransactionNestLevel > ACTransNestLevelStart Then
                    m_sSQLTrace = m_sSQLTrace & m_oCmd.CommandText & StringCHR() & StringCHR10()
                End If
            End If
            lRecordsAffected = m_oCmd.ExecuteNonQuery()

            ' Reset the Active Connection
            m_oCmd.Connection = Nothing

            m_oCmd = Nothing

            ' RFC190603 - Release Connection for connection pooling
            m_lReturn = CloseDBConnection()
            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If

            Return result
        Catch excep As System.Exception
            Dim sErrNum = "", sErrDesc, sErrSource As String

            ''sErrNum = CStr(Information.Err().Number)
            sErrDesc = excep.Message
            sErrSource = excep.Source

            ' Error Section.
            result = PMConstants.PMEReturnCode.PMError

            If Not (m_oCmd Is Nothing) Then
                sDebugSQL = sDebugSQL & "ADO CommandText : " & m_oCmd.CommandText & StringCHR() & StringCHR10()
            End If

            If DebugMode Then
                sDebugSQL = sDebugSQL & "SQL Supplied    : " & sSQL & StringCHR() & StringCHR10()
            End If

            ' Log Error Message
            LogDatabaseError(sSiriusUsername:=m_sSiriusUsername, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, bConnectionPooling:=m_bConnectionPooling, iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:=sDebugSQL, vApp:="dPMDAO", vClass:="Database", vMethod:="SQLAction", vErrNo:=sErrNum, vErrDesc:=sErrDesc, vErrSource:=sErrSource, oCon:=m_oCon, oCmd:=m_oCmd, lError:=result)
            If Not m_oCon Is Nothing AndAlso m_oCon.State = ConnectionState.Open AndAlso m_iTransactionNestLevel = 0 Then
                m_oCon.Close()
            End If
            Return result
        Finally
            If Not m_oCon Is Nothing AndAlso m_oCon.State = ConnectionState.Open AndAlso m_iTransactionNestLevel = 0 Then
                m_oCon.Close()
            End If

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SQLSelectTextField
    '
    ' Description: Returns Text Data returned by a Select.
    '
    ' NOTE: The text field MUST be returned in the first field of the
    ' first record of the first recordset.
    '
    ' History: 24/04/2001 RFC - Created.
    '
    ' ***************************************************************** '
    Public Function SQLSelectTextField(ByRef sSQL As String, ByRef sSQLName As String, ByRef bStoredProcedure As Boolean, ByRef sTextData As String) As Integer
        Dim result As Integer = 0
        Try
            Return SelectRecords(sSQL:=sSQL, sSQLName:=sSQLName, bStoredProcedure:=bStoredProcedure, bSelectText:=True, vResultArray:=sTextData)
        Catch excep As System.Exception
            result = PMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogDatabaseMessage(iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="SQLSelectTextField Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SQLSelectTextField", vErrDesc:=excep.Message)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: SQLBeginTrans
    '
    ' Description: Starts a Transaction.
    '
    '
    ' ***************************************************************** '
    Public Function SQLBeginTrans() As Integer
        Dim result As Integer = 0
        Try
            result = PMConstants.PMEReturnCode.PMTrue

            ' Check the Current Connection
            m_lReturn = CheckDBConnection()
            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If

            ' Have we already started a transaction
            If m_iTransactionNestLevel > ACTransNestLevelStart Then

                ' Yes, Increment the nesting level
                m_iTransactionNestLevel += 1

                If DebugMode Then
                    m_sSQLTrace = m_sSQLTrace & "BEGIN TRANS (LOGICAL)" & StringCHR() & StringCHR10()
                End If
            Else
                m_Transaction = m_oCon.BeginTransaction
                ' Set the Nest Level to Start plus one
                m_iTransactionNestLevel = ACTransNestLevelStart + 1

                If DebugMode Then
                    m_sSQLTrace = "BEGIN TRANS (PHYSICAL)" & StringCHR() & StringCHR10()
                End If

            End If
            Return result
        Catch excep As System.Exception
            ' Error Section.
            result = PMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogDatabaseError(sSiriusUsername:=m_sSiriusUsername, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, bConnectionPooling:=m_bConnectionPooling, iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="Begin Transaction Call Failed", vApp:="dPMDAO", vClass:="Database", vMethod:="SQLBeginTrans", vErrDesc:=excep.Message, vErrSource:=excep.Source, oCon:=m_oCon)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SQLCommitTrans
    '
    ' Description: Commits a Transaction on the database.
    '
    '
    ' ***************************************************************** '
    Public Function SQLCommitTrans() As Integer
        Dim result As Integer = 0
        Try

            result = PMConstants.PMEReturnCode.PMTrue

            ' Check the Current Connection
            m_lReturn = CheckDBConnection()
            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If

            ' Is there a transaction to started
            If m_iTransactionNestLevel <= ACTransNestLevelStart Then
                ' No, just exit.
                Return result
            End If

            ' Decrement the Transaction Nest Level
            m_iTransactionNestLevel -= 1

            ' If we are at the Start Transaction Nest Level
            If m_iTransactionNestLevel = ACTransNestLevelStart Then

                If DebugMode Then
                    m_sSQLTrace = m_sSQLTrace & "COMMIT TRANS (PHYSICAL)" & StringCHR() & StringCHR10()
                    ' We have committed the transaction so clear the trace (we dont report the trace on a commit at the moment.)
                    m_sSQLTrace = ""
                End If

                ' Commit the Transaction
                m_Transaction.Commit()
            Else
                If DebugMode Then
                    m_sSQLTrace = m_sSQLTrace & "COMMIT TRANS (LOGICAL)" & StringCHR() & StringCHR10()
                End If
            End If

            ' RFC190603 - Release Connection for connection pooling
            m_lReturn = CloseDBConnection()
            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If

            Return result
        Catch excep As System.Exception
            ' Error Section.
            result = PMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogDatabaseError(sSiriusUsername:=m_sSiriusUsername, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, bConnectionPooling:=m_bConnectionPooling, iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="Commit Transaction Call Failed", vApp:="dPMDAO", vClass:="Database", vMethod:="SQLCommitTrans", vErrDesc:=excep.Message, vErrSource:=excep.Source, oCon:=m_oCon)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SQLRollbackTrans
    '
    ' Description: Commits a Transaction on the database.
    '
    '
    ' ***************************************************************** '
    Public Function SQLRollbackTrans() As Integer
        Dim result As Integer = 0
        Dim sMsg As String = ""

        Try

            result = PMConstants.PMEReturnCode.PMTrue

            ' Check the Current Connection
            m_lReturn = CheckDBConnection()
            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If

            ' Is there a transaction to started
            If m_iTransactionNestLevel <= ACTransNestLevelStart Then
                ' No, just exit.
                Return result
            End If

            ' Decrement the Transaction Nest Level
            m_iTransactionNestLevel -= 1

            ' If we are at the Start Transaction Nest Level
            If m_iTransactionNestLevel = ACTransNestLevelStart Then
                If DebugMode Then
                    m_sSQLTrace = m_sSQLTrace & "ROLLBACK TRANS (PHYSICAL)" & StringCHR() & StringCHR10()
                    sMsg = "***********************************************************************************" & StringCHR() & StringCHR10()
                    sMsg = sMsg & "WARNING Transaction is being ROLLED BACK. Transaction Trace is as follows..." & StringCHR() & StringCHR10()
                    sMsg = sMsg & m_sSQLTrace & StringCHR() & StringCHR10()
                    LogDatabaseMessage(iType:=PMConstants.PMELogLevel.PMLogError, sMsg:=sMsg, vApp:=ACApp, vClass:="Database")
                    m_sSQLTrace = ""
                End If

                ' Roll Back the Transaction
                m_Transaction.Rollback()
            Else
                If DebugMode Then
                    m_sSQLTrace = m_sSQLTrace & "ROLLBACK TRANS (LOGICAL)" & StringCHR() & StringCHR10()
                End If
            End If

            m_lReturn = CloseDBConnection()
            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If

            Return result
        Catch excep As System.Exception
            ' This is a work around to trap the ODBC Driver Error
            ' caused by an RDO RollbackTrans.



            result = PMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogDatabaseError(sSiriusUsername:=m_sSiriusUsername, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, bConnectionPooling:=m_bConnectionPooling, iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="Rollback Transaction Call Failed", vApp:="dPMDAO", vClass:="Database", vMethod:="SQLRollbackTrans", vErrDesc:=excep.Message, vErrSource:=excep.Source, oCon:=m_oCon)

            Return result
        End Try
    End Function

    ' RFC24/04/01 Add support for SQLSelectTextField
    ' ***************************************************************** '
    ' Name: SelectRecords
    '
    ' Description: Perform the given select statement on the database.
    '
    ' Note       : The bAsync Optional parameter has been
    '              added for future expandability without (hopefully)
    '              breaking version compatibility. It does nothing in
    '              this version.
    ' ***************************************************************** '
    Private Function SelectRecords(ByRef sSQL As String, ByRef sSQLName As String, ByRef bStoredProcedure As Boolean, ByRef bSelectText As Boolean, Optional ByRef lNumberRecords As Integer = 0, Optional ByRef vResultArray As Object = Nothing, Optional ByRef bNoPrepare As Boolean = False, Optional ByRef bKeepQuery As Boolean = False, Optional ByRef bAsync As Boolean = False, Optional ByRef bKeepNulls As Boolean = False) As Integer
        Dim result As Integer = 0
        Dim oRec As DataSet
        Dim sMergedSQL As String = ""
        Dim sDebugSQL As String = ""

        Try

            result = PMConstants.PMEReturnCode.PMTrue

            ' Check the Current Connection
            m_lReturn = CheckDBConnection()
            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If

            ' Validate the params
            If sSQL.Length = 0 Then
                PMFunctions.RaiseError("Validate Params", "sSQL Paramater is an EMPTY String, a valid Stored Procedure or SQL command MUST be supplied.", PMConstants.PMELogLevel.PMLogError)
            End If

            ' Clear the Result Set
            m_oRecords = Nothing

            ' Get a Reference to the Command
            m_oCmd = m_oParameters.ADOCommand

            ' Set the Command Active Connection
            m_oCmd.Connection = m_oCon

            ' Set the Command Type
            m_oCmd.CommandType = CommandType.Text

            ' Set the QueryTimouet
            m_oCmd.CommandTimeout = QueryTimeout

            m_oCmd.Transaction = m_Transaction

            ' Set Limit based on NumberRecords parameter

            Select Case lNumberRecords
                ' PMDAO Default
                Case 0
                    ' Set the Query Limit to the PMDAO Default
                    'lNumberRecords = ACDefaultMaxRows
                    lNumberRecords = ACMaxMaxRows
                    ' All Records
                Case PMConstants.PMAllRecords
                    ' Set the Query Limit to get All records
                    lNumberRecords = ACMaxMaxRows

                    ' Invalid Setting
                Case Is < PMConstants.PMAllRecords
                    ' Set the Query Limit to the PMDAO Default
                    lNumberRecords = ACDefaultMaxRows

                    ' Specified Number Of Records
                Case Else
            End Select

            ' Is SQL a Stored procedure
            If bStoredProcedure Then
                ' Are we calling a sproc using the NON ODBC style.
                If Not sSQL.Trim().StartsWith("{") Then
                    ' Looks like we have just been given the sproc name so set the ADO Command Type to Stored Proc
                    m_oCmd.CommandType = CommandType.StoredProcedure
                End If

                ' Set the Command Text to be the sp call
                m_oCmd.CommandText = sSQL
            Else
                ' Merge parameter values with SQL string
                m_lReturn = MergeParameters(sInSQL:=sSQL, sOutSQL:=sMergedSQL)

                If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                    Return PMConstants.PMEReturnCode.PMFalse
                End If

                ' Set the Command Text to the SQL String
                m_oCmd.CommandText = sMergedSQL
            End If

            If DebugMode Then
                If m_iTransactionNestLevel > ACTransNestLevelStart Then
                    m_sSQLTrace = m_sSQLTrace & m_oCmd.CommandText & StringCHR() & StringCHR10()
                End If
            End If

            ' Create Recordset
            'Modified because the following line was not setting the adapter object's command object properly
            'the parameters count was coming 0
            'Dim adap As SqlDataAdapter = New SqlDataAdapter(m_oCmd.CommandText, m_oCmd.Connection)
            Dim adap As SqlDataAdapter = New SqlDataAdapter()
            adap.SelectCommand = m_oCmd
            oRec = New DataSet("dsl")
            adap.Fill(oRec)

            If bSelectText Then
                result = GetTextField(oRecordset:=oRec, vTextData:=vResultArray)
            Else
                ' Get the Query Results into PMDAO Records OR Variant Array
                result = GetQueryResults(oRecordset:=oRec, lNumberRecords:=lNumberRecords, bKeepNulls:=bKeepNulls, vResultArray:=vResultArray)
            End If
            ' Reset the Active Connection
            m_oCmd.Connection = Nothing

            ' Close and Release Command
            m_oCmd = Nothing

            oRec = Nothing

            ' RFC190603 - Release Connection for connection pooling
            m_lReturn = CloseDBConnection()
            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If

            Return result
        Catch excep As System.Exception
            Dim sErrNum = "", sErrDesc, sErrSource As String

            ''sErrNum = CStr(Information.Err().Number)
            sErrDesc = excep.Message
            sErrSource = excep.Source

            ' Error Section.
            result = PMConstants.PMEReturnCode.PMError

            If Not (m_oCmd Is Nothing) Then
                sDebugSQL = sDebugSQL & "ADO CommandText : " & m_oCmd.CommandText & StringCHR() & StringCHR10()
            End If

            If DebugMode Then
                sDebugSQL = sDebugSQL & "SQL Supplied    : " & sSQL & StringCHR() & StringCHR10()
            End If

            ' Log Error Message
            LogDatabaseError(sSiriusUsername:=m_sSiriusUsername, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, bConnectionPooling:=m_bConnectionPooling, iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:=sDebugSQL, vApp:="dPMDAO", vClass:="Database", vMethod:="SelectRecords", vErrNo:=sErrNum, vErrDesc:=sErrDesc, vErrSource:=sErrSource, oCon:=m_oCon, oCmd:=m_oCmd, lError:=result)
            If Not m_oCon Is Nothing AndAlso m_oCon.State = ConnectionState.Open AndAlso m_iTransactionNestLevel = 0 Then
                m_oCon.Close()
            End If
            Return result
        Finally
            If Not m_oCon Is Nothing AndAlso m_oCon.State = ConnectionState.Open AndAlso m_iTransactionNestLevel = 0 Then
                m_oCon.Close()
            End If
        End Try
    End Function

    Private Function GetTextField(ByRef oRecordset As DataSet, ByRef vTextData As Object) As Integer
        Dim result As Integer = PMConstants.PMEReturnCode.PMTrue

        Try
            vTextData = String.Empty

            ' No Records
            If (oRecordset Is Nothing) Then
                Return result
            End If
            If (oRecordset.Tables.Count = 0) Then
                Return result
            End If
            If (oRecordset.Tables(0) Is Nothing) Then
                Return result
            End If
            If oRecordset.Tables(0).Rows.Count < 1 Then
                Return result
            End If

            For Each dr As DataRow In oRecordset.Tables(0).Rows

                If oRecordset.Tables(0).Columns.Contains("xml_data") Then
                    If dr("xml_data").ToString().Length <= 0 Then
                        Return result
                    End If
                    vTextData += dr("xml_data")
                Else
                    If dr(0).ToString().Length <= 0 Then
                        Return result
                    End If
                    vTextData += dr(0)
                End If

            Next dr
        Catch ex As Exception

            result = PMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogDatabaseMessage(PMConstants.PMELogLevel.PMLogOnError, "GetTextField Failed", ACApp, ACClass, "GetTextField", ex.Message)

            Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: GetQueryResults
    '
    ' Description: For each result set from the query, populate the
    '              variant array or PMDAO recordset.
    '
    '
    ' ***************************************************************** '
    Public Function GetQueryResults(ByRef oRecordset As DataSet, ByRef lNumberRecords As Integer, ByRef bKeepNulls As Boolean, Optional ByRef vResultArray As Object = Nothing) As Integer
        Dim result As Integer = 0
        Dim lRecordCount, lTempArrayRowSize, lTempArrayColSize, lStartRow, lStartTempRow As Integer
        Dim vTempArray(,) As Object
        Dim lGetRowsChunkSize As Integer
        Dim bMoreRecords As Boolean

        Try

            result = PMConstants.PMEReturnCode.PMTrue

            ' Are we using the Result Array
            If Not (vResultArray Is Nothing) Then
                ' Yes - clear (This should release any memory used previously)
                vResultArray = Nothing
            End If

            lRecordCount = 0

            bMoreRecords = True

            For Each table As DataTable In oRecordset.Tables
                If ((table.Rows.Count) < 1) Then
                    ' NO Records
                Else
                    ' Are we populating the Result Array
                    'If Information.IsArray(vResultArray) OrElse (vResultArray <> -1) Then
                    vTempArray = Nothing
                    'DefaultPropHelper.SetDefaultProperty(vTempArray, "")

                    ' Work Out how many rows to get
                    lGetRowsChunkSize = (lNumberRecords - lRecordCount)

                    Dim vArrayTemp(,) As Object

                    'Added by Deepak Sharma on 4/13/2010 11:40:30 AM refer developer guide no. 104 (Guide)
                    'Start' 
                    If oRecordset.Tables.Count = 0 Then
                        Return PMConstants.PMEReturnCode.PMTrue
                    End If
                    'End'

                    If lGetRowsChunkSize > (table.Rows.Count) Then
                        lGetRowsChunkSize = table.Rows.Count
                    End If
                    Dim cols As Integer = table.Columns.Count - 1
                    Dim rows As Integer = lGetRowsChunkSize - 1
                    Dim ar(table.Rows.Count - 1) As DataRow

                    ' Use CopyTo to populate a 1D array of DataRows                    
                    table.Rows.CopyTo(ar, 0)

                    ReDim vArrayTemp(cols, rows)

                    Dim nullValue As Object = Nothing

                    If Not bKeepNulls Then
                        nullValue = ""
                    End If

                    For colCnt As Integer = 0 To cols
                        For rowCnt As Integer = 0 To rows
                            'developer guide no.93
                            If (ar(rowCnt)(colCnt)) Is DBNull.Value Then
                                vArrayTemp(colCnt, rowCnt) = nullValue
                            Else
                                vArrayTemp(colCnt, rowCnt) = ar(rowCnt)(colCnt).ToString().TrimEnd()
                            End If

                            If Not bKeepNulls Then
                                vArrayTemp(colCnt, rowCnt) = vArrayTemp(colCnt, rowCnt) & ""

                            End If

                        Next
                    Next

                    vTempArray = vArrayTemp

                    ' Is this the first Result Set to be added to the Result Array
                    If vResultArray Is Nothing OrElse (Not (vResultArray.GetType().IsArray)) Then

                        ' Yes - first results - Assign Temporary Array
                        vResultArray = vTempArray

                    Else
                        ' Get the Starting position before we resize the result array
                        lStartRow = vResultArray.GetUpperBound(1) + 1

                        ' How many rows in the Temp Array
                        lTempArrayRowSize = vTempArray.GetUpperBound(1)

                        ' Get the Start Position (Lower bound) of the Temporary array
                        lStartTempRow = vTempArray.GetLowerBound(1)

                        ' Is we are using base 0 increase the row size by one
                        If lStartTempRow = 0 Then
                            lTempArrayRowSize += 1
                        End If

                        ' Get the No of columns in the temp array
                        lTempArrayColSize = vTempArray.GetUpperBound(0)

                        ' Resize the Result Array
                        ReDim Preserve vResultArray(vResultArray.GetUpperBound(0), vResultArray.GetUpperBound(1) + (lTempArrayRowSize))

                        ' Loop round adding the results from the Temp Array
                        For lRow As Integer = lStartRow To vResultArray.GetUpperBound(1)
                            For lCol As Integer = vResultArray.GetLowerBound(0) To vResultArray.GetUpperBound(0)
                                ' Is there fewer columns in the Temp Array
                                If lCol > lTempArrayColSize Then
                                    ' Fewer columns
                                    ' Do we want Null values
                                    If Not bKeepNulls Then
                                        ' No Nulls - Set to empty string
                                        vResultArray(lCol, lRow) = ""
                                    Else

                                        vResultArray(lCol, lRow) = DBNull.Value
                                    End If
                                Else
                                    ' Do we want to remove Nulls
                                    If Not bKeepNulls Then
                                        ' Yes - Remove
                                        vResultArray(lCol, lRow) = CStr(vTempArray(lCol, lStartTempRow)) & ""
                                    Else
                                        ' No - Do not remove
                                        vResultArray(lCol, lRow) = vTempArray(lCol, lStartTempRow)
                                    End If
                                End If
                            Next lCol
                            lStartTempRow += 1
                        Next lRow

                    End If

                    m_oRecords = New dPMDAO.Records
                    m_lReturn = m_oRecords.Initialise(oRecordset, bKeepNulls)

                    If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                        Return PMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            Next
            ' Release any memory used by the temporary array
            vTempArray = Nothing

            ' Set the Actual number of Records returned
            ' If we are using the Array
            If Not (vResultArray Is Nothing) Then
                ' If it is an Array
                If vResultArray.GetType().IsArray Then
                    ' Return based on the number of Array Rows (+1 due to base 0)
                    '' lNumberRecords = UBound(vResultArray, 2) + 1
                    Dim a As Array = TryCast(vResultArray, Array)
                    lNumberRecords = a.GetUpperBound(1) + 1
                Else
                    ' No array so no rows
                    lNumberRecords = 0
                End If
            Else
                ' If there were no records returned
                If m_oRecords Is Nothing Then
                    ' Create an Empty Records Class
                    'm_oRecords = UpgradeSupport.CompServerHelper.Create(Of dPMDAO.Records)(m_oRecords)
                    m_oRecords = New dPMDAO.Records()

                    m_lReturn = m_oRecords.Initialise(Nothing, True)
                End If

                ' Return the PMDAO Record Count
                lNumberRecords = m_oRecords.Count()
            End If

            Return result
        Catch excep As System.Exception
            ' Error Section.
            result = PMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogDatabaseMessage(iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQueryResults failed", vApp:="dPMDAO", vClass:="Database", vMethod:="GetQueryResults", vErrDesc:=excep.Message)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: MergeParameters
    '
    ' Description: Merges the parameters collection with an SQL string.
    '
    '
    ' ***************************************************************** '
    Private Function MergeParameters(ByRef sInSQL As String, ByRef sOutSQL As String) As Integer
        Dim result As Integer = 0
        Dim lStringLength As Integer

        Dim iParamStart, iParamEnd, iStartFrom As Integer
        Dim sParam As String = ""
        Dim vParamValue As Object
        Dim sDateDelimiter, sDateFormat, sErrMsg As String

        Try
            result = PMConstants.PMEReturnCode.PMTrue
            If Parameters.Count() < 1 Then
                sOutSQL = sInSQL
                Return result
            End If

            sDateDelimiter = ACDefaultDateDelimiter
            sDateFormat = ACDefaultDateFormat

            ' Get the length of the string
            lStringLength = sInSQL.Length

            iStartFrom = 0

            Do Until iStartFrom > lStringLength
                ' Find Postition of Start Parameter Delimiter
                '' iParamStart = Strings.InStr(iStartFrom, sInSQL, PMConstants.PMStartDelimiter, PMConstants.PMEStringCompareType.PMStringCompare)
                iParamStart = sInSQL.IndexOf(PMConstants.PMStartDelimiter, iStartFrom)
                ' If it is not found then we have finished
                If iParamStart = -1 Then

                    ' Did we start from the beginning
                    If iStartFrom = 1 Then
                        ' Yes, Assign the whole string
                        sOutSQL = sInSQL
                    Else
                        ' Mo, Assign what is left of the string
                        sOutSQL = sOutSQL & sInSQL.Substring(iStartFrom, lStringLength - iStartFrom)
                    End If

                    Exit Do

                End If

                ' Find Postition of End Parameter Delimiter
                ''iParamEnd = Strings.InStr(iParamStart, sInSQL, PMConstants.PMEndDelimiter, PMConstants.PMEStringCompareType.PMStringCompare)

                iParamEnd = sInSQL.IndexOf(PMConstants.PMEndDelimiter, iStartFrom)

                ' If it is not found then we have finished
                If iParamEnd = -1 Then
                    Exit Do
                End If

                ' Build up String
                sOutSQL = sOutSQL & sInSQL.Substring(iStartFrom, iParamStart - iStartFrom)

                ' Get Parameter Name
                sParam = sInSQL.Substring(iParamStart + 1, iParamEnd - iParamStart - 1)

                ' Get Parameter Value
                vParamValue = m_oParameters.Item(sParam).Value

                If Convert.IsDBNull(vParamValue) Or (vParamValue Is Nothing) Then
                    sOutSQL = sOutSQL & "Null"
                Else
                    ' String Parameter Value formatted according to Data Type
                    Select Case m_oParameters.Item(sParam).DbType
                        ' Strings
                        Case DbType.String
                            sOutSQL = sOutSQL & "'" & CStr(vParamValue) & "'"
                            ' Dates
                        Case DbType.DateTime
                            sOutSQL = sOutSQL & sDateDelimiter & String.Format(sDateFormat, vParamValue) & sDateDelimiter
                        Case DbType.Binary
                            sOutSQL = sOutSQL & HexString(vParamValue)
                        Case DbType.Object
                            sOutSQL = sOutSQL & CStr(vParamValue)
                        Case Else
                            sOutSQL = sOutSQL & CDbl(vParamValue)
                    End Select
                End If
                ' Set new start position to be after parameter
                iStartFrom = iParamEnd + 1
            Loop

            ' Need to now delete the Parameters from the ADO Params Collection
            Parameters.DeleteAll()

            Return result
        Catch excep As System.Exception
            ' Error Section.
            result = PMConstants.PMEReturnCode.PMError

            sErrMsg = "Failed to Merge Parameter {" & sParam & "}" & StringCHR() & StringCHR10()
            sErrMsg = sErrMsg & "VB Error = " + (excep.Message & StringCHR() & StringCHR10())
            sErrMsg = sErrMsg & "Input SQL = " & sInSQL & StringCHR() & StringCHR10()

            ' Log Error Message
            LogDatabaseError(sSiriusUsername:=m_sSiriusUsername, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, bConnectionPooling:=m_bConnectionPooling, iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrMsg, vApp:="dPMDAO", vClass:="Database", vMethod:="MergeParameters", vErrDesc:=excep.Message, oCon:=m_oCon, oCmd:=m_oParameters.ADOCommand)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: HexString
    '
    ' Description: Takes a variant array containing the byte values
    '              of a number and converts it into a HEX string
    '              in the correct syntax for the DB.
    '
    ' ***************************************************************** '
    Private Function HexString(ByRef vByteArray As Object) As String
        Dim result As New StringBuilder
        result.Append(String.Empty)
        Dim sHexChar As String = ""

        Try
            ' Start Hex string with the DB Hex identifier
            result = New StringBuilder(PMConstants.PMDBHexPrefix)

            For iSub As Integer = 0 To vByteArray.GetUpperBound(0)
                ' Convert byte to hex
                sHexChar = CInt(vByteArray(iSub)).ToString("X")

                ' Pad with leading zero if required
                If sHexChar.Length < 2 Then
                    sHexChar = "0" & sHexChar
                End If

                ' Append to String
                result.Append(sHexChar)

            Next iSub

            Return result.ToString()
        Catch excep As System.Exception
            result = New StringBuilder("")

            ' Log Error Message
            LogDatabaseMessage(iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="HexString failed", vApp:="dPMDAO", vClass:="Database", vMethod:="HexString", vErrDesc:=excep.Message)

            Return result.ToString()
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CreateEnvironment
    '
    ' Description: Creates the Initial Environment in which ALL
    '              connections are created.
    '
    ' ***************************************************************** '
    Private Function CreateEnvironment() As Integer
        Dim result As Integer = 0
        Dim sQueryTimeout As String = ""
        Dim sCompatibleMode As String = ""

        Try
            result = PMConstants.PMEReturnCode.PMTrue
            If m_bEnvironmentCreated Then
                Return result
            End If


            ' Create New Parameters Collection
            m_oParameters = New dPMDAO.Parameters()
            m_oParameters.SetGlobalData(sSiriusUsername:=m_sSiriusUsername, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, sCallingAppName:=m_sCallingAppName, iLogLevel:=m_iLogLevel, bConnectionPooling:=m_bConnectionPooling)

            ' Get the PMDAO Version Mode
            PMDAOVersionMode = 2

            sQueryTimeout = GetConnectionDetails(PMConstants.PMRegKeyQueryTimeoutSeconds)
            ' Default the Query Timeout property
            Dim dbNumericTemp As Double
            If Double.TryParse(sQueryTimeout, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                QueryTimeout = CInt(sQueryTimeout)
            Else
                QueryTimeout = ACDefaultQueryTimeout
            End If

            m_bConnectionPooling = False
            ' RFC060803 - Trace the SQL Call whilst in a transaction so we can report
            '             what work has been lost when we rollback a transaction.

            Dim sDebugMode As String = GetConnectionDetails(PMConstants.PMRegKeyArchitectureInDebug)
            If sDebugMode = "1" Then
                DebugMode = True
            Else
                DebugMode = False
            End If

            m_sSQLTrace = ""

            Return result

        Catch excep As System.Exception
            result = PMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogDatabaseMessage(iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateEnvironment failed", vApp:="dPMDAO", vClass:="Database", vMethod:="CreateEnvironment", vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CloseEnvironment
    '
    ' Description: Closes the Environment and ALL connections.
    '
    ' ***************************************************************** '
    Private Function CloseEnvironment() As Integer
        Dim result As Integer = 0
        Try
            result = PMConstants.PMEReturnCode.PMTrue

            ' Clear parameters
            m_oParameters = Nothing
            ' Clear Records
            m_oRecords = Nothing
            m_bEnvironmentCreated = False
            Return result
        Catch excep As System.Exception
            ' Error Section

            result = PMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogDatabaseMessage(iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="CloseEnvironment failed", vApp:="dPMDAO", vClass:="Database", vMethod:="CloseEnvironment", vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    ' RFC190603 - Release Connection for connection pooling
    ' ***************************************************************** '
    ' Name: CheckDBConnection
    '
    ' Description: Checks that we have a current DB Connection.
    '
    ' ***************************************************************** '
    Private Function CheckDBConnection() As Integer
        Dim result As Integer = 0
        Try
            result = PMConstants.PMEReturnCode.PMTrue

            ' Is there a Current Connection
            If m_oCon Is Nothing Then
                ' Create a New Connection
                m_oCon = New SqlConnection()
            End If

            If m_oCon.State = ConnectionState.Closed Then
                m_oCon.ConnectionString = m_sConnectString
                Try
                    m_oCon.Open()
                Catch ex As InvalidOperationException
                    If ex.Message = "Handle is not initialized." Then
                        m_oCon.Open()
                    End If
                End Try
            End If
            Return result

        Catch excep As System.Exception
            ' Error Section 
            result = PMConstants.PMEReturnCode.PMError
            ' Log Error Message
            LogDatabaseError(m_sSiriusUsername, m_sCallingAppName, m_iSourceID, m_iLanguageID, m_bConnectionPooling, PMConstants.PMELogLevel.PMLogOnError, "CheckDBConnection failed", ACApp, ACClass, "CheckDBConnection", "Incorrect_Connection_String", excep.Message, excep.Source, m_oCon)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CloseDBConnection
    '
    ' Description: If there are no outstanding transactions on this
    '              connection, close it so that it can be return to the
    '              connection pool and re-used.
    '
    ' ***************************************************************** '
    Private Function CloseDBConnection() As Integer

        Dim result As Integer = 0
        Try

            result = PMConstants.PMEReturnCode.PMTrue

            If Not m_oCon Is Nothing Then
                If m_iTransactionNestLevel > ACTransNestLevelStart Then
                    ' Yes, there is an outstanding transaction

                    ' So we cannot close the connection, just exit
                    Return result
                End If
            End If

            ' Close the Current Connection
            m_oCon.Close()

            ' Release the Reference
            m_oCon = Nothing

            Return result
        Catch excep As System.Exception
            ' Error Section
            result = PMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogDatabaseError(sSiriusUsername:=m_sSiriusUsername, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, bConnectionPooling:=m_bConnectionPooling, iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="CloseDBConnection failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CloseDBConnection", vErrDesc:=excep.Message, oCon:=m_oCon)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: CheckDSN
    '
    ' Description: Checks the DSN Name supplied.
    '
    '
    ' ***************************************************************** '
    Private Function CheckDSN(ByRef r_sDSN As String, ByRef r_sDatabase As String, ByRef r_bKnownDSN As Boolean, ByRef r_sUserNameUsedToConnect As String, ByRef r_sUserPassUsedToConnect As String) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim sLoginId As String = ""
        Dim sPassword As String = ""

        Try

            ' RFC11031998 - Allow for other DSN's other than the PMDAO known ones.
            ' Assume we know the DSN until proven otherwise.
            r_bKnownDSN = True

            ' RAM20050414 - Changes made to support Swift Database.
            '               Default to Sirius Login Name, and Password

            nResult = GetUserAndPassword(sLoginId, sPassword)

            r_sUserNameUsedToConnect = sLoginId
            r_sUserPassUsedToConnect = sPassword

            ' Which DSN to open
            Select Case r_sDSN.Trim()
                Case "", "GIS", PMConstants.PMGeminiDSN, PMConstants.PMGeminiIIDSN, PMConstants.PMClaimsDSN, PMConstants.PMSiriusSolutionsDSN, PMConstants.PMSiriusUnderwritingDSN, PMConstants.PMSiriusBrokingDSN, PMConstants.PMSiriusArchitectureDSN, PMConstants.PMSiriusDSN, PMConstants.PMOrionDSN, PMConstants.PMDocumasterDSN
                    ' Is we after one of the Sirius Databases, use the combined Sirius Solutions
                    r_sDSN = PMConstants.PMSiriusSolutionsDSN
                    r_sDatabase = PMConstants.PMSiriusSolutionsDatabase
                Case PMConstants.PMVoyagerDSN
                    ' Voyager
                    r_sDSN = PMConstants.PMVoyagerDSN
                    r_sDatabase = PMConstants.PMVoyagerDatabase
                Case PMConstants.PMMercuryDSN
                    ' Mercury
                    r_sDSN = PMConstants.PMMercuryDSN
                    r_sDatabase = PMConstants.PMMercuryDatabase
                Case PMConstants.PMDocumasterV2DSN
                    ' Documasterv2
                    r_sDSN = PMConstants.PMDocumasterV2DSN
                    r_sDatabase = PMConstants.PMDocumasterV2Database
                Case PMConstants.PMDocumasterScanDSN
                    ' DocumasterScan
                    r_sDSN = PMConstants.PMDocumasterScanDSN
                    r_sDatabase = PMConstants.PMDocumasterScanDatabase
                Case PMConstants.PMNirvanaDSN
                    ' Nirvana
                    r_sDSN = PMConstants.PMNirvanaDSN
                    r_sDatabase = PMConstants.PMNirvanaDatabase
                    'JSB 15/09/03
                Case PMConstants.PMMediquoteDSN
                    ' Mediquote
                    r_sDSN = PMConstants.PMMediquoteDSN
                    r_sDatabase = PMConstants.PMMediquoteDatabase
                Case PMConstants.PMSwiftDSN
                    ' Swift
                    r_sDSN = PMConstants.PMSwiftDSN
                    r_sDatabase = PMConstants.PMSwiftDatabase
                    r_sUserNameUsedToConnect = ACDefaultSwiftUser ' Swift Database Login Name
                    r_sUserPassUsedToConnect = ACDefaultSwiftPassword ' Swift Database Login Password
                Case Else
                    ' Unknown DSN
                    r_bKnownDSN = False

                    r_sUserNameUsedToConnect = "" ' Reset
                    r_sUserPassUsedToConnect = "" ' Reset
            End Select

            Return nResult

        Catch excep As System.Exception
            ' Error Section
            nResult = PMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogDatabaseMessage(iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckDSN failed", vApp:="dPMDAO", vClass:="Database", vMethod:="CheckDSN", vErrDesc:=excep.Message, vErrNo:="Incorrect_Connection_String")

            Return nResult
        End Try
    End Function
    ' PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

        Try
            m_bEnvironmentCreated = False
            m_lReturn = CreateEnvironment()
            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception("Invalid Pure Configuration")
            End If
            m_bEnvironmentCreated = True
            Try
                iCache = CacheFactory.GetCacheManager("PureCache")
            Catch ex As Exception
            End Try

            m_sCachePath = GetConnectionDetails(PMConstants.PMRegKeyCachePath)

            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception("Invalid Pure Configuration")
            End If

            'If Right(m_sCachePath, 1) <> "\" Then
            '    m_sCachePath += "\"
            'End If

        Catch excep As System.Exception
            LogDatabaseMessage(iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the database class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrDesc:=excep.Message, vErrNo:="Incorrect_Connection_String")
            Exit Sub
        End Try
    End Sub

    Protected Overrides Sub Finalize()
        Try
            m_lReturn = CloseEnvironment()
        Catch excep As System.Exception
            LogDatabaseMessage(iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the database class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Terminate", vErrDesc:=excep.Message)

            Exit Sub
        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: ExecuteNonQuery
    ' THIS METHOD SHOULD NOT BE CALLED FROM VB6 CODE!
    '
    ' Description: Construct and execute a SQL command in a stateless manner
    ' and return the number of rows affected.
    '
    ' This method is intended to be called from .NET code. It deliberately
    ' ignores the Sirius standard error handling in order to ensure that
    ' errors get thrown up to the .NET runtime.
    '
    ' History:
    '   CDH 22062006 created
    ' ***************************************************************** '

    Public Function ExecuteNonQuery(ByVal command As SqlCommand) As Integer
        OpenConnection(command)
        Dim rowsAffected As Integer = command.ExecuteNonQuery()
        CloseConnection(command)
        Return rowsAffected
    End Function

    ' ***************************************************************** '
    ' Name: ExecuteScalar
    ' THIS METHOD SHOULD NOT BE CALLED FROM VB6 CODE!
    '
    ' Description: Construct and execute a SQL command in a stateless manner
    ' and return the value in row 1 column 1 of the resultset. Return Null
    ' if no open resultset was returned from the command.
    '
    ' This method is intended to be called from .NET code. It deliberately
    ' ignores the Sirius standard error handling in order to ensure that
    ' errors get thrown up to the .NET runtime.
    '
    ' History:
    '   CDH 31082006 created
    ' ***************************************************************** '

    Public Function ExecuteScalar(ByVal command As SqlCommand) As Object
        OpenConnection(command)
        Dim value As Object = command.ExecuteScalar()
        CloseConnection(command)
        Return value
    End Function


    ''' <summary>
    '''  Construct and execute a SQL command in a stateless manner and return a disconnected resultset.
    ''' </summary>
    ''' <param name="command"></param>
    ''' <param name="dtResults"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ExecuteDataTable(ByVal command As SqlCommand, ByVal dtResults As DataTable) As Integer
        Dim nReturn As Integer = PMEReturnCode.PMTrue

        Try
            Dim adapter As SqlDataAdapter = New SqlDataAdapter()
            OpenConnection(command)
            adapter.SelectCommand = command
            Dim rowsAffected As Integer = adapter.Fill(dtResults)
            CloseConnection(command)

        Catch excep As Exception

            Dim sErrNum = "", sErrDesc, sErrSource As String
            Dim sDebugSQL As String = ""
            ''sErrNum = CStr(Information.Err().Number)
            sErrDesc = excep.Message
            sErrSource = excep.Source

            ' Error Section.
            nReturn = PMEReturnCode.PMError

            If Not (m_oCmd Is Nothing) Then
                sDebugSQL = sDebugSQL & "ADO CommandText : " & m_oCmd.CommandText & StringCHR() & StringCHR10()
            End If


            ' Log Error Message
            LogDatabaseError(sSiriusUsername:=m_sSiriusUsername, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, bConnectionPooling:=m_bConnectionPooling, iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:=sDebugSQL, vApp:="dPMDAO", vClass:="Database", vMethod:="SelectRecords", vErrNo:=sErrNum, vErrDesc:=sErrDesc, vErrSource:=sErrSource, oCon:=m_oCon, oCmd:=m_oCmd)
            If Not m_oCon Is Nothing AndAlso m_oCon.State = ConnectionState.Open AndAlso m_iTransactionNestLevel = 0 Then
                m_oCon.Close()
            End If
            Return nReturn
        Finally
            If Not m_oCon Is Nothing AndAlso m_oCon.State = ConnectionState.Open AndAlso m_iTransactionNestLevel = 0 Then
                m_oCon.Close()
            End If
        End Try
        Return nReturn
    End Function

    Public Function ExecuteDataTable(ByVal command As SqlCommand, ByVal adapter As SqlDataAdapter, ByVal results As DataTable) As Integer
        OpenConnection(command)
        adapter.SelectCommand = command
        Dim rowsAffected As Integer = adapter.Fill(results)
        CloseConnection(command)
        Return rowsAffected
    End Function

    Public Function ExecuteDataSet(ByVal command As SqlCommand, ByVal adapter As SqlDataAdapter, ByVal results As DataSet, ByVal tableName As String) As Integer
        OpenConnection(command)
        adapter.SelectCommand = command
        Dim rowsAffected As Integer = adapter.Fill(results, tableName)
        CloseConnection(command)
        Return rowsAffected
    End Function

    Public Function ExecuteList(Of T)(ByVal command As SqlCommand, ByVal convert As Converter(Of IDataRecord, T)) As Generic.List(Of T)
        OpenConnection(command)
        Dim results As New Generic.List(Of T)
        Using reader As SqlDataReader = command.ExecuteReader(CommandBehavior.SingleResult)
            While reader.Read()
                results.Add(convert(reader))
            End While
        End Using
        CloseConnection(command)
        Return results
    End Function


    Public Function ExecuteXmlText(ByVal command As SqlCommand) As String
        OpenConnection(command)
        Dim results As String = Nothing
        Using reader As XmlReader = command.ExecuteXmlReader()
            If reader.Read() Then
                results = reader.ReadOuterXml()
            End If
        End Using
        CloseConnection(command)
        Return results
    End Function

    Public Function ExecuteXPathDocument(ByVal command As SqlCommand) As XPath.XPathDocument
        OpenConnection(command)
        Dim results As XPath.XPathDocument
        Using reader As XmlReader = command.ExecuteXmlReader()
            results = New XPath.XPathDocument(reader)
        End Using
        CloseConnection(command)
        Return results
    End Function

    ' ***************************************************************** '
    ' Name: OpenConnection
    ' THIS METHOD SHOULD NOT BE CALLED FROM VB6 CODE!
    '
    ' Description: Open the database connection before executing an ADODB.Command
    ' object in a manner that is compatible with standard dPMDAO code.
    '
    ' This is a helper method for the public .NET methods. It deliberately
    ' ignores the Sirius standard error handling in order to ensure that
    ' errors get thrown up to the .NET runtime.
    '
    ' History:
    '   CDH 22062006 created
    ' ***************************************************************** '

    Private Sub OpenConnection(ByVal oCommand As SqlCommand)
        ' Open the connection.
        m_lReturn = CheckDBConnection()
        If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
            '' Throw New System.Exception((Constants.vbObjectError + m_lReturn).ToString() + ", dPMDAO.Database.OpenConnection, Cannot open the database connection.")
        End If

        ' Attach the connection and transaction to the command.
        oCommand.Connection = m_oCon
        oCommand.Transaction = m_Transaction
        oCommand.CommandTimeout = QueryTimeout
        ' Trace the SQL Call whilst in a transaction so we can report
        ' what work has been lost when we rollback a transaction.
        If DebugMode Then
            If m_iTransactionNestLevel > ACTransNestLevelStart Then
                m_sSQLTrace = m_sSQLTrace & oCommand.CommandText & StringCHR() & StringCHR10()
            End If
        End If
    End Sub

    ' ***************************************************************** '
    ' Name: CloseConnection
    ' THIS METHOD SHOULD NOT BE CALLED FROM VB6 CODE!
    '
    ' Description: Close the database connection after executing an ADODB.Command
    ' object in a manner that is compatible with standard dPMDAO code.
    '
    ' This is a helper method for the public .NET methods. It deliberately
    ' ignores the Sirius standard error handling in order to ensure that
    ' errors get thrown up to the .NET runtime.
    '
    ' History:
    '   CDH 22062006 created
    ' ***************************************************************** '
    Private Sub CloseConnection(ByVal oCommand As SqlCommand)
        oCommand.Connection = Nothing
        oCommand.Transaction = Nothing

        ' Close the connection.
        m_lReturn = CloseDBConnection()
        If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception((m_lReturn).ToString() + ", dPMDAO.Database.CloseConnection, Cannot close the database connection.")
        End If
    End Sub

    ' ***************************************************************** '
    ' Name: BatchSQLSelect
    '
    ' Description: executes SQL and returns disconnected recordset
    '
    ' History:
    '   RDC 10012003 created
    ' ***************************************************************** '
    Public Function BatchSQLSelect(ByVal sSQL As String, ByRef oRecordset As DataSet) As Integer
        Dim result As Integer = 0
        Dim sMergedSQL As String = ""

        Try
            result = PMConstants.PMEReturnCode.PMFalse

            ' Check the Current Connection
            m_lReturn = CheckDBConnection()
            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = MergeParameters(sSQL, sMergedSQL)

            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If DebugMode Then
                If m_iTransactionNestLevel > ACTransNestLevelStart Then
                    m_sSQLTrace = m_sSQLTrace & sMergedSQL & StringCHR() & StringCHR10()
                End If
            End If

            'Open the Recordset for Disconected Environment Variables
            Dim com As New SqlCommand
            com.Connection = m_oCon
            com.CommandText = sMergedSQL
            Dim adap As SqlDataAdapter = New SqlDataAdapter(com.CommandText, com.Connection)
            oRecordset = New DataSet("dsl")
            adap.Fill(oRecordset)

            m_lReturn = CloseDBConnection()
            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If


            Return PMConstants.PMEReturnCode.PMTrue
        Catch excep As System.Exception
            result = PMConstants.PMEReturnCode.PMError

            LogDatabaseError(sSiriusUsername:=m_sSiriusUsername, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, bConnectionPooling:=m_bConnectionPooling, iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to execute BatchSQLSelect", vApp:="dPMDAO", vClass:="Database", vMethod:="BatchSQLSelect", vErrDesc:=excep.Message, vErrSource:=excep.Source, oCon:=m_oCon, lError:=result)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BatchUpdate
    '
    ' Description: reconnects and updates a disconnected recordset
    '
    ' History:
    '   RDC 10012003 created
    ' ***************************************************************** '
    Public Function BatchUpdate(ByRef oRecordset As DataSet) As Integer
        Dim result As Integer = 0
        Try
            result = PMConstants.PMEReturnCode.PMFalse

            ' Check the Current Connection
            m_lReturn = CheckDBConnection()
            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If

            If DebugMode Then
                If m_iTransactionNestLevel > ACTransNestLevelStart Then
                    m_sSQLTrace = m_sSQLTrace & "UPDATE BATCH" & StringCHR() & StringCHR10()
                End If
            End If

            m_lReturn = CloseDBConnection()
            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If

            Return PMConstants.PMEReturnCode.PMTrue
        Catch excep As System.Exception
            result = PMConstants.PMEReturnCode.PMError

            LogDatabaseError(sSiriusUsername:=m_sSiriusUsername, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, bConnectionPooling:=m_bConnectionPooling, iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to execute BatchUpdate", vApp:="dPMDAO", vClass:="Database", vMethod:="BatchUpdate", vErrDesc:=excep.Message, vErrSource:=excep.Source, oCon:=m_oCon, lError:=result)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SQLSelectForXML
    '
    ' Description: return SQL query as XML DOM document
    '
    ' History:
    ' RDC 20012003 created
    ' RAM20041011 : Set the QueryTimeout property to the ADO Command Object
    ' ***************************************************************** '
    Public Function SQLSelectForXML(ByVal sSQL As String, ByVal bStoredProcedure As Boolean, ByRef oXMLDOM As XmlDocument) As Integer
        Dim result As Integer = 0
        Dim sSQLout As String = ""
        Dim oCmd As SqlCommand = Nothing
        Dim sDebugSQL As String = ""
        Dim oRec As DataSet

        Try
            result = PMConstants.PMEReturnCode.PMFalse
            ' Check the current connection
            m_lReturn = CheckDBConnection()

            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If

            oCmd = m_oParameters.ADOCommand
            With oCmd
                .Connection = m_oCon
                If bStoredProcedure Then
                    ' Stored proc
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = sSQL
                Else
                    ' text
                    ' Merge parameter values with SQL string
                    m_lReturn = MergeParameters(sSQL, sSQLout)

                    If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                        ' merge failed
                        Return PMConstants.PMEReturnCode.PMFalse
                    End If

                    .CommandType = CommandType.Text
                    .CommandText = sSQLout
                End If

                If DebugMode Then
                    If m_iTransactionNestLevel > ACTransNestLevelStart Then
                        m_sSQLTrace = m_sSQLTrace & oCmd.CommandText & StringCHR() & StringCHR10()
                    End If
                End If

                .CommandTimeout = QueryTimeout
                .Transaction = m_Transaction
            End With
            Dim adap As SqlDataAdapter = New SqlDataAdapter()
            adap.SelectCommand = oCmd
            oRec = New DataSet("dsl")
            adap.Fill(oRec)

            oXMLDOM.LoadXml(oRec.GetXml())

            oXMLDOM.LoadXml(oXMLDOM.InnerText)

            oCmd = Nothing
            oRec = Nothing
            m_lReturn = CloseDBConnection()
            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If

            Return PMConstants.PMEReturnCode.PMTrue
        Catch excep As System.Exception
            result = PMConstants.PMEReturnCode.PMError

            If Not (oCmd Is Nothing) Then
                sDebugSQL = oCmd.CommandText
            Else
                sDebugSQL = sSQL
            End If

            LogDatabaseError(sSiriusUsername:=m_sSiriusUsername, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, bConnectionPooling:=m_bConnectionPooling, iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to execute SQLSelectForXML SQL= " & sDebugSQL, vApp:="dPMDAO", vClass:="Database", vMethod:="SQLSelectForXML", vErrDesc:=excep.Message, vErrSource:=excep.Source, oCon:=m_oCon, oCmd:=oCmd, lError:=result)

            Return result
        Finally
            m_lReturn = CloseDBConnection()
        End Try
    End Function
    Public Function ExecuteDataSet(ByVal sSQL As String,
                                   ByVal sSQLName As String,
                                   ByVal bStoredProcedure As Boolean,
                                   ByRef oRecordset As DataSet) As Integer
        Dim result As Integer = 0
        Dim sSQLout As String = ""
        Dim oCmd As SqlCommand = Nothing
        Dim sDebugSQL As String = ""


        Try
            result = PMConstants.PMEReturnCode.PMFalse
            ' Check the current connection
            m_lReturn = CheckDBConnection()

            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If

            oCmd = m_oParameters.ADOCommand
            With oCmd
                .Connection = m_oCon
                If bStoredProcedure Then
                    ' Stored proc
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = sSQL
                Else
                    ' text
                    ' Merge parameter values with SQL string
                    m_lReturn = MergeParameters(sSQL, sSQLout)

                    If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                        ' merge failed
                        Return PMConstants.PMEReturnCode.PMFalse
                    End If

                    .CommandType = CommandType.Text
                    .CommandText = sSQLout
                End If

                If DebugMode Then
                    If m_iTransactionNestLevel > ACTransNestLevelStart Then
                        m_sSQLTrace = m_sSQLTrace & oCmd.CommandText & StringCHR() & StringCHR10()
                    End If
                End If

                .CommandTimeout = QueryTimeout
                .Transaction = m_Transaction
            End With

            Dim adap As SqlDataAdapter = New SqlDataAdapter()
            adap.SelectCommand = oCmd
            oRecordset = New DataSet("dsl")
            adap.Fill(oRecordset)

            oCmd = Nothing
            m_lReturn = CloseDBConnection()
            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If

            Return PMConstants.PMEReturnCode.PMTrue
        Catch excep As System.Exception
            result = PMConstants.PMEReturnCode.PMError

            If Not (oCmd Is Nothing) Then
                sDebugSQL = oCmd.CommandText
            Else
                sDebugSQL = sSQL
            End If

            LogDatabaseError(sSiriusUsername:=m_sSiriusUsername, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, bConnectionPooling:=m_bConnectionPooling, iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to execute SQLSelectForXMLNew SQL= " & sDebugSQL, vApp:="dPMDAO", vClass:="Database", vMethod:="SQLSelectForXMLNew", vErrDesc:=excep.Message, vErrSource:=excep.Source, oCon:=m_oCon, oCmd:=oCmd, lError:=result)

            Return result
        End Try
    End Function

    ''' <summary>
    ''' Execute the SQL on database and return a data table with results 
    ''' </summary>
    ''' <param name="sSQL"></param>
    ''' <param name="sSQLName"></param>
    ''' <param name="bStoredProcedure"></param>
    ''' <param name="oRecordset"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ExecuteDataTable(ByVal sSQL As String,
                                   ByVal sSQLName As String,
                                   ByVal bStoredProcedure As Boolean,
                                   ByRef oRecordset As DataTable) As Integer

        Dim nResult As Integer = PMConstants.PMEReturnCode.PMFalse
        Dim sSQLout As String = String.Empty
        Dim oCmd As SqlCommand = Nothing
        Dim sDebugSQL As String = String.Empty


        Try
            nResult = PMConstants.PMEReturnCode.PMFalse
            ' Check the current connection
            nResult = CheckDBConnection()

            If nResult <> PMConstants.PMEReturnCode.PMTrue Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If

            oCmd = m_oParameters.ADOCommand
            With oCmd
                .Connection = m_oCon
                If bStoredProcedure Then
                    ' Stored proc
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = sSQL
                Else
                    ' text
                    ' Merge parameter values with SQL string
                    nResult = MergeParameters(sSQL, sSQLout)

                    If nResult <> PMConstants.PMEReturnCode.PMTrue Then
                        ' merge failed
                        Return PMConstants.PMEReturnCode.PMFalse
                    End If

                    .CommandType = CommandType.Text
                    .CommandText = sSQLout
                End If

                If DebugMode Then
                    If m_iTransactionNestLevel > ACTransNestLevelStart Then
                        m_sSQLTrace = m_sSQLTrace & oCmd.CommandText & StringCHR() & StringCHR10()
                    End If
                End If

                .CommandTimeout = QueryTimeout
                .Transaction = m_Transaction
            End With
            Dim adap As SqlDataAdapter = New SqlDataAdapter()
            adap.SelectCommand = oCmd
            If oRecordset Is Nothing Then
                oRecordset = New DataTable()
            End If
            adap.Fill(oRecordset)


            oCmd = Nothing
            nResult = CloseDBConnection()
            If nResult <> PMConstants.PMEReturnCode.PMTrue Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult
        Catch ex As System.Exception
            nResult = PMConstants.PMEReturnCode.PMError

            If Not (oCmd Is Nothing) Then
                sDebugSQL = oCmd.CommandText
            Else
                sDebugSQL = sSQL
            End If

            LogDatabaseError(sSiriusUsername:=m_sSiriusUsername, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, bConnectionPooling:=m_bConnectionPooling, iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to execute ExecuteDataTable. SQL= " & sDebugSQL, vApp:="dPMDAO", vClass:="Database", vMethod:="SQLSelectForXMLNew", vErrDesc:=ex.Message, vErrSource:=ex.Source, oCon:=m_oCon, oCmd:=oCmd, lError:=nResult)

            Return nResult
        End Try
    End Function

    Public Function SQLSelectForXML(ByVal sSQL As String, ByVal bStoredProcedure As Boolean, ByRef oRecordset As DataSet) As Integer
        Dim result As Integer = 0
        Dim sSQLout As String = ""
        Dim oCmd As SqlCommand = Nothing
        Dim sDebugSQL As String = ""


        Try
            result = PMConstants.PMEReturnCode.PMFalse
            ' Check the current connection
            m_lReturn = CheckDBConnection()

            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If

            oCmd = m_oParameters.ADOCommand
            With oCmd
                .Connection = m_oCon
                If bStoredProcedure Then
                    ' Stored proc
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = sSQL
                Else
                    ' text
                    ' Merge parameter values with SQL string
                    m_lReturn = MergeParameters(sSQL, sSQLout)

                    If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                        ' merge failed
                        Return PMConstants.PMEReturnCode.PMFalse
                    End If

                    .CommandType = CommandType.Text
                    .CommandText = sSQLout
                End If

                If DebugMode Then
                    If m_iTransactionNestLevel > ACTransNestLevelStart Then
                        m_sSQLTrace = m_sSQLTrace & oCmd.CommandText & StringCHR() & StringCHR10()
                    End If
                End If

                .CommandTimeout = QueryTimeout
                .Transaction = m_Transaction
            End With
            Dim adap As SqlDataAdapter = New SqlDataAdapter()
            adap.SelectCommand = oCmd
            oRecordset = New DataSet("dsl")
            adap.Fill(oRecordset)

            oCmd = Nothing
            m_lReturn = CloseDBConnection()
            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If

            Return PMConstants.PMEReturnCode.PMTrue
        Catch excep As System.Exception
            result = PMConstants.PMEReturnCode.PMError

            If Not (oCmd Is Nothing) Then
                sDebugSQL = oCmd.CommandText
            Else
                sDebugSQL = sSQL
            End If

            LogDatabaseError(sSiriusUsername:=m_sSiriusUsername, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, bConnectionPooling:=m_bConnectionPooling, iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to execute SQLSelectForXMLNew SQL= " & sDebugSQL, vApp:="dPMDAO", vClass:="Database", vMethod:="SQLSelectForXMLNew", vErrDesc:=excep.Message, vErrSource:=excep.Source, oCon:=m_oCon, oCmd:=oCmd, lError:=result)

            Return result
        Finally
            m_lReturn = CloseDBConnection()
        End Try
    End Function
    Public Function SQLSelectForXMLNew(ByVal sSQL As String, ByVal bStoredProcedure As Boolean) As Integer
        Dim result As Integer = 0
        Dim sSQLout As String = ""
        Dim oCmd As SqlCommand = Nothing
        Dim sDebugSQL As String = ""
        Dim oRec As DataSet

        Try
            result = PMConstants.PMEReturnCode.PMFalse
            ' Check the current connection
            m_lReturn = CheckDBConnection()

            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If

            oCmd = m_oParameters.ADOCommand
            With oCmd
                .Connection = m_oCon
                If bStoredProcedure Then
                    ' Stored proc
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = sSQL
                Else
                    ' text
                    ' Merge parameter values with SQL string
                    m_lReturn = MergeParameters(sSQL, sSQLout)

                    If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                        ' merge failed
                        Return PMConstants.PMEReturnCode.PMFalse
                    End If

                    .CommandType = CommandType.Text
                    .CommandText = sSQLout
                End If

                If DebugMode Then
                    If m_iTransactionNestLevel > ACTransNestLevelStart Then
                        m_sSQLTrace = m_sSQLTrace & oCmd.CommandText & StringCHR() & StringCHR10()
                    End If
                End If

                .CommandTimeout = QueryTimeout
                .Transaction = m_Transaction
            End With
            Dim adap As SqlDataAdapter = New SqlDataAdapter()
            adap.SelectCommand = oCmd
            oRec = New DataSet("dsl")
            adap.Fill(oRec)

            For Each dr As DataRow In oRec.Tables(0).Rows
                OpenFile()
                streamWriter.Write(dr(0).ToString())
                CloseFile()
            Next

            oRec.Dispose()
            oCmd = Nothing
            oRec = Nothing

            m_lReturn = CloseDBConnection()
            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If

            Return PMConstants.PMEReturnCode.PMTrue
        Catch excep As System.Exception
            result = PMConstants.PMEReturnCode.PMError

            If Not (oCmd Is Nothing) Then
                sDebugSQL = oCmd.CommandText
            Else
                sDebugSQL = sSQL
            End If

            LogDatabaseError(sSiriusUsername:=m_sSiriusUsername, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, bConnectionPooling:=m_bConnectionPooling, iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to execute SQLSelectForXMLNew SQL= " & sDebugSQL, vApp:="dPMDAO", vClass:="Database", vMethod:="SQLSelectForXMLNew", vErrDesc:=excep.Message, vErrSource:=excep.Source, oCon:=m_oCon, oCmd:=oCmd, lError:=result)

            Return result
        Finally
            m_lReturn = CloseDBConnection()
        End Try
    End Function

    Public Sub OpenFile()
        Dim strPath As String = ""
        ''strPath = My.Application.Info.DirectoryPath + "xmlTemp.xml"
        If System.IO.File.Exists(strPath) Then
            fileStream = New FileStream(strPath, FileMode.Append, FileAccess.Write)
        Else
            fileStream = New FileStream(strPath, FileMode.Create, FileAccess.Write)
        End If
        streamWriter = New StreamWriter(fileStream)
    End Sub

    Public Sub CloseFile()
        streamWriter.Close()
        fileStream.Close()
    End Sub

#Region "Private Methods"
    ''' <summary>
    ''' Get the User Name and Password to connect to the DataBase
    ''' Password is Stored in Encrypted Form in the Registry. 
    ''' </summary>
    ''' <param name="o_sUserName"></param>
    ''' <param name="o_sPassword"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetUserAndPassword(ByRef o_sUserName As String, ByRef o_sPassword As String) As Integer

        Dim nReturn As Integer = PMEReturnCode.PMTrue
        Try

            Dim sLoginId As String = ""
            sLoginId = GetConnectionDetails(PMSQLLoginId)


            Dim sPasswordSecure As String = ""
            sPasswordSecure = GetConnectionDetails(PMSQLLoginPassword)

            Dim aKeys As Byte()
            aKeys = Encoding.ASCII.GetBytes(PMEncryptionEntropy)

            o_sUserName = Decrypt(sLoginId, aKeys)
            o_sPassword = Decrypt(sPasswordSecure, aKeys)

            Return nReturn
        Catch ex As Exception
            Throw New ApplicationException("GetUserAndPassword Failed", ex)
        End Try

        Return nReturn
    End Function
    ''' <summary>
    ''' Decrypt the Encrypted LoginIdPassword based on the Cipher
    ''' </summary>
    ''' <param name="sCipher"></param>
    ''' <param name="aKeys"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function Decrypt(sCipher As String, aKeys As Byte()) As String

        If sCipher = "" Then
            Return ""
        End If
        If sCipher Is Nothing Then
            Throw New ArgumentNullException("sCipher")
        End If

        'parse base64 string
        Dim aData As Byte() = Convert.FromBase64String(sCipher)

        ''decrypt data
        Dim aDecrypted As Byte() = ProtectedData.Unprotect(aData, aKeys, kScope)
        Return Encoding.Unicode.GetString(aDecrypted)
    End Function
#End Region
    Public Function SQLActionForPIE(ByRef sSQL As String, ByRef sSQLName As String, ByRef bStoredProcedure As Boolean, Optional ByRef lRecordsAffected As Integer = 0, Optional ByRef bNoPrepare As Boolean = False, Optional ByRef bKeepQuery As Boolean = False, Optional ByRef bAsync As Boolean = False) As Integer
        Dim result As Integer = 0
        Dim sMergedSQL As String = ""
        Dim sDebugSQL As String = ""

        Try
            result = PMConstants.PMEReturnCode.PMTrue

            ' Check the Current Connection
            m_lReturn = CheckDBConnection()
            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If

            ' Validate the params
            If sSQL.Length = 0 Then
                PMFunctions.RaiseError("Validate Params", "sSQL Paramater is an EMPTY String, a valid Stored Procedure or SQL command MUST be supplied.", PMConstants.PMELogLevel.PMLogError)
            End If

            ' Get a Reference to the Command
            m_oCmd = m_oParameters.ADOCommand

            ' Set the Command Active Connection
            m_oCmd.Connection = m_oCon

            ' Set the Command Type
            m_oCmd.CommandType = CommandType.Text

            ' Set the QueryTimouet
            m_oCmd.CommandTimeout = QueryTimeout

            m_oCmd.Transaction = m_Transaction
            m_oCmd.CommandText = "SET QUOTED_IDENTIFIER OFF"
            lRecordsAffected = m_oCmd.ExecuteNonQuery()
            ' Is SQL a Stored procedure
            If bStoredProcedure Then
                ' Are we calling a sproc using the NON ODBC style.
                If Not sSQL.Trim().StartsWith("{") Then
                    ' Looks like we have just been given the sproc name so set the ADO Command Type to Stored Proc
                    m_oCmd.CommandType = CommandType.StoredProcedure
                End If

                m_oCmd.CommandText = sSQL
            Else
                ' Merge parameter values with SQL string
                m_lReturn = MergeParameters(sInSQL:=sSQL, sOutSQL:=sMergedSQL)

                If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                    Return PMConstants.PMEReturnCode.PMFalse
                End If

                ' Update the Queries SQL String
                m_oCmd.CommandText = sMergedSQL
            End If
            If DebugMode Then
                If m_iTransactionNestLevel > ACTransNestLevelStart Then
                    m_sSQLTrace = m_sSQLTrace & m_oCmd.CommandText & StringCHR() & StringCHR10()
                End If
            End If
            lRecordsAffected = m_oCmd.ExecuteNonQuery()

            ' Reset the Active Connection
            m_oCmd.Connection = Nothing

            m_oCmd = Nothing

            ' RFC190603 - Release Connection for connection pooling
            m_lReturn = CloseDBConnection()
            If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If

            Return result
        Catch excep As System.Exception
            Dim sErrNum = "", sErrDesc, sErrSource As String

            ''sErrNum = CStr(Information.Err().Number)
            sErrDesc = excep.Message
            sErrSource = excep.Source

            ' Error Section.
            result = PMConstants.PMEReturnCode.PMError

            If Not (m_oCmd Is Nothing) Then
                sDebugSQL = sDebugSQL & "ADO CommandText : " & m_oCmd.CommandText & StringCHR() & StringCHR10()
            End If

            If DebugMode Then
                sDebugSQL = sDebugSQL & "SQL Supplied    : " & sSQL & StringCHR() & StringCHR10()
            End If

            ' Log Error Message
            LogDatabaseError(sSiriusUsername:=m_sSiriusUsername, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, bConnectionPooling:=m_bConnectionPooling, iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:=sDebugSQL, vApp:="dPMDAO", vClass:="Database", vMethod:="SQLAction", vErrNo:=sErrNum, vErrDesc:=sErrDesc, vErrSource:=sErrSource, oCon:=m_oCon, oCmd:=m_oCmd, lError:=result)
            If Not m_oCon Is Nothing AndAlso m_oCon.State = ConnectionState.Open AndAlso m_iTransactionNestLevel = 0 Then
                m_oCon.Close()
            End If
            Return result
        End Try
    End Function

    Public Function SetValues(keyString As String, settingName As String, values As String, Optional ByVal UserName As String = "", Optional MachineName As String = "") As String
        Dim result As Integer = 0
        result = PMConstants.PMEReturnCode.PMTrue
        If settingName.Equals("SQLLogin") OrElse settingName.Equals("SecureKey") OrElse settingName.Equals("UserLogLevel") OrElse settingName.Equals("EventLogMessaging") OrElse settingName.Equals("LogFileName") Then
            UpdateConnectionDetail(settingName, values)
            Return result
        End If
        m_lReturn = OpenDatabase("", 1, 1, "")
        If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If
        Parameters.Clear()

        result = Parameters.Add(sName:="KeyPath", vValue:=CStr(keyString), iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
        result = Parameters.Add(sName:="KeyName", vValue:=CStr(settingName), iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
        result = Parameters.Add(sName:="KeyData", vValue:=CStr(values), iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
        result = Parameters.Add(sName:="UserName", vValue:=CStr(UserName), iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
        result = Parameters.Add(sName:="MachineName", vValue:=CStr(MachineName), iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
        result = SQLAction(sSQL:="spu_pm_Add_or_Update_Registry", sSQLName:="spu_pm_Add_or_Update_Registry", bStoredProcedure:=True)

        If result <> PMEReturnCode.PMTrue Then
            Return result
        End If

        Return result
    End Function


    Public Function GetValues(keyString As String, settingName As String, ByRef KeyValue As String, Optional ByVal UserName As String = "", Optional ByVal MachineName As String = "") As Integer
        Dim result As Integer = 0
        result = PMConstants.PMEReturnCode.PMTrue
        Dim sSettingName As String = settingName.Replace(" ", "")
        Dim settingNames As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase) From {
                    "PMDIR", "PureReleaseVersion", "Servicerelease", "CachePath", "UserLogLevel", "EventLogMessaging", "LogFileName", "SQLLogin", "SecureKey", "Trusted"}

        If keyString.Contains("Databases\Pure") OrElse settingNames.Contains(sSettingName) Then
            KeyValue = GetConnectionDetails(sSettingName)
            Return result
        End If

        Const CACHE_KEY As String = "RegistrySettingsCache"
        KeyValue = String.Empty
        Dim sContent(1) As String

        ' Try get cache
        Dim RegistrySetting As Dictionary(Of String, String) = Nothing

        If Not iCache Is Nothing AndAlso iCache.Contains(CACHE_KEY) AndAlso Not String.IsNullOrEmpty(Convert.ToString(iCache.GetData(CACHE_KEY))) Then
            RegistrySetting = TryCast(iCache.GetData(CACHE_KEY), Dictionary(Of String, String))
        End If

        ' Load from DB if cache is empty
        If RegistrySetting Is Nothing Then

            result = LoadRegistrySettingsFromDb(Registry:=RegistrySetting)
            If result <> PMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            Dim sFilePath As String = m_sCachePath + "RegistrySettingsCache.xml"
            If Not System.IO.File.Exists(sFilePath) Then
                Dim fileIO As FileStream
                fileIO = File.Create(sFilePath)
                fileIO.Close()
                File.WriteAllLines(sFilePath, sContent)
            End If
            '
            ' Sirius Cache Controller
            If Not iCache Is Nothing Then
                iCache.Add(CACHE_KEY, RegistrySetting, CacheItemPriority.Normal, Nothing, New FileDependency(sFilePath))
            End If
            ' End If

        End If

        ' Build composite lookup key
        Dim lookupKey As String
        If Not String.IsNullOrEmpty(UserName) Then
            lookupKey = $"{keyString}|{settingName}|U:{UserName}"
        Else
            lookupKey = $"{keyString}|{settingName}|M:{MachineName}"
        End If

        ' Lookup
        If RegistrySetting.ContainsKey(lookupKey) Then
            KeyValue = RegistrySetting(lookupKey)
            Return PMConstants.PMEReturnCode.PMTrue
        End If

        Return PMConstants.PMEReturnCode.PMTrue


    End Function

    Public Function GetConnectionDetails(AttributeName As String) As String

        Dim assemblyDir As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
        Dim PMDIR As String = System.IO.Path.GetPathRoot(assemblyDir)
        If AttributeName.Equals("PMDIR") Then
            Return PMDIR
        ElseIf AttributeName.Equals("CachePath") Then
            Return PMDIR & "Pure\Cache\"
        End If
        Dim xmldoc As New XmlDocument
        Dim xmlFileName = PMDIR & "\Pure\PureConfiguration.xml" 'ConfigurationManager.AppSettings["RegistryPath"];
        ' read your XML
        Try

            Dim xmlContent = File.ReadAllText(xmlFileName)
            If xmldoc Is Nothing Then
                xmldoc = New XmlDocument()
            End If
            xmldoc.LoadXml(xmlContent)
            Dim node = xmldoc.SelectSingleNode("Pure")
            If node IsNot Nothing AndAlso node.Attributes(AttributeName) IsNot Nothing Then
                Try
                    Return node.Attributes(AttributeName).Value
                Catch
                    Return ""
                End Try
            End If

            'LogDatabaseMessage(iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="GetConnectionDetails failed", vApp:="dPMDAO", vClass:="Database", vMethod:="GetConnectionDetails", vErrNo:="Incorrect_Connection_String", vErrDesc:=node.Attributes(AttributeName).Value)
            Return ""
        Catch excep As Exception

            LogDatabaseMessage(iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="GetConnectionDetails failed", vApp:="dPMDAO", vClass:="Database", vMethod:="GetConnectionDetails", vErrNo:="Incorrect_Connection_String", vErrDesc:=excep.Message)
            Return ""
        End Try

    End Function

    Public Sub UpdateConnectionDetail(AttributeName As String, NewValue As String)

        Dim assemblyDir As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
        Dim PMDIR As String = System.IO.Path.GetPathRoot(assemblyDir)

        Dim xmlFileName = PMDIR & "\Pure\PureConfiguration.xml"

        If Not File.Exists(xmlFileName) Then
            Throw New FileNotFoundException("XML file not found.", xmlFileName)
        End If

        Dim xmldoc As New XmlDocument()
        xmldoc.Load(xmlFileName)

        ' Select the "Pure" node
        Dim node = xmldoc.SelectSingleNode("Pure")
        If node IsNot Nothing Then
            ' Check if the attribute exists
            If node.Attributes(AttributeName) IsNot Nothing Then
                node.Attributes(AttributeName).Value = NewValue
            Else
                ' Add the attribute if it doesn't exist
                Dim attr As XmlAttribute = xmldoc.CreateAttribute(AttributeName)
                attr.Value = NewValue
                node.Attributes.Append(attr)
            End If

            ' Save changes to the file
            xmldoc.Save(xmlFileName)
        Else
            Throw New Exception("The 'Pure' node was not found in the XML file.")
        End If
    End Sub

    Private Function LoadRegistrySettingsFromDb(ByRef Registry As Dictionary(Of String, String)) As Integer
        Dim result As Integer
        Dim vResultArray As Object = Nothing
        Registry = New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
        m_lReturn = OpenDatabase("", 1, 1, "")
        If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        Parameters.Clear()

        result = Parameters.Add(sName:="UserName", vValue:=Environment.UserName, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
        result = Parameters.Add(sName:="MachineName", vValue:=Environment.MachineName, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
        result = SQLSelect(sSQL:="spu_pm_get_registry_setting", sSQLName:="LoadRegistrySettings", bStoredProcedure:=True, vResultArray:=vResultArray, lNumberRecords:=PMConstants.PMAllRecords)

        If result = PMConstants.PMEReturnCode.PMTrue AndAlso vResultArray IsNot Nothing AndAlso vResultArray.GetType().IsArray Then
            For r As Integer = 0 To UBound(vResultArray, 2)
                Dim keyPath = CStr(vResultArray(0, r))
                Dim keyName = CStr(vResultArray(1, r))
                Dim keyData = CStr(vResultArray(2, r))
                Dim user = If(vResultArray(3, r), "")
                Dim machine = If(vResultArray(4, r), "")

                Dim userKey = $"{keyPath}|{keyName}|U:{user}"
                Dim machineKey = $"{keyPath}|{keyName}|M:{machine}"

                If Not Registry.ContainsKey(userKey) AndAlso user <> "" Then Registry(userKey) = keyData
                If Not Registry.ContainsKey(machineKey) AndAlso machine <> "" Then Registry(machineKey) = keyData
            Next
        End If

        Return PMConstants.PMEReturnCode.PMTrue
    End Function

    Private Function ACDefaultUser() As String
        Throw New NotImplementedException
    End Function

    Private Function ACDefaultPassword() As String
        Throw New NotImplementedException
    End Function

    Private Function StringCHR() As String
        Return ""
    End Function
    Private Function StringCHR10() As String
        Return ""
    End Function
End Class
