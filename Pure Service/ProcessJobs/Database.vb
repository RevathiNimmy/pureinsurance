Imports System.Security.Cryptography
Imports Microsoft.VisualBasic
Imports SharedFiles
Imports Sirius.Architecture.Security
Imports Sirius.Architecture.Data
''' <summary>
''' This class represents one Sirius database, in order to support multiple databases on Sirius 21.
''' NOTE: The contents of this class are specific to this project and should NOT be shared.
''' </summary>
Public NotInheritable Class Database
    Implements ICloneable

#Region "Private Fields"

    Private _server As String                   ' SQL Server connection server name
    Private _database As String                 ' SQL Server connection database name
    Private _trusted As Boolean                 ' SQL Server connection trusted connection flag
    Private _connection As SiriusConnection     ' SQL Server connection object
    Private _username As String                 ' Sirius user name (if blank, integrated login is used)
    Private _password As String                 ' Sirius user password
    Private _userID As Nullable(Of Integer)     ' Sirius user ID
    Private Const kScope As DataProtectionScope = DataProtectionScope.LocalMachine
#End Region

#Region "Constructors"

    ''' <summary>
    ''' Create a new Sirius database reference.
    ''' </summary>
    Public Sub New()

        ' Create the connection using data from the registry.
        Dim sConnectionString As String = ""
        GetConnectionString(sConnectionString)
        _connection = SiriusConnection.FromAny(sConnectionString)
        _username = SiriusUserDefaults.Username
        _password = SiriusUserDefaults.Password
        _userID = SiriusUserDefaults.UserID
        _server = SiriusRegistryAccess.GetValueAsString(Registry.LocalMachine, "SOFTWARE\PM\SiriusArchitecture\Server\Databases\SiriusSolutions", "Server", String.Empty)
        _database = SiriusRegistryAccess.GetValueAsString(Registry.LocalMachine, "SOFTWARE\PM\SiriusArchitecture\Server\Databases\SiriusSolutions", "Database", String.Empty)

    End Sub

#End Region

#Region "Public Properties"

    Public ReadOnly Property Connection() As SiriusConnection
        Get
            Return _connection
        End Get
    End Property

    Public ReadOnly Property UserID() As Integer
        Get
            ' This read must be done on first use, NOT in the constructor, because the connection
            ' can only be cloned properly before it is first used due to the way that ADO.NET is designed.
            If Not _userID.HasValue Then
                If String.IsNullOrEmpty(_username) Then
                    _userID = (SiriusPrincipal.ToSiriusPrincipal(Me.Connection, Thread.CurrentPrincipal)).Identity.ID
                Else
                    _userID = (New SiriusPrincipal(Me.Connection, _username, _password)).Identity.ID
                End If
            End If
            Return _userID.Value
        End Get
    End Property

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

        Return ""

    End Function

#End Region

#Region "Friend Properties"

#End Region

#Region "Public Methods"

#End Region

#Region "Private Methods"

    ' Separate method because VB.NET does not support anonymous delegates.
    Private Function GetConnectionString(ByRef r_sConnectString As String) As Integer

        Const ConnectionStringFrame As String = "Server={server};Database={database};Integrated Security=False; User ID={loginid}; Password={loginpassword}"
        Const ConnectionStringFrameWindowsAuthentication As String = "Server={server};Database={database};Integrated Security=SSPI;"

        Dim nResult As Integer = 0
        Dim sDSN, sDatabase As String
        Dim sUsername As String = ""
        Dim sPassword As String = ""
        Dim sProvider As String
        Dim sServer As String
        Dim bTrusted As Boolean
        Dim sFilePath As String = ""

        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            nResult = GetUserAndPassword(sUsername, sPassword)

            If nResult = PMEReturnCode.PMTrue AndAlso r_sConnectString = "" Then
                sDSN = "Pure"

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
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Open a Connection Using local data and registry settings
                If bTrusted Then
                    r_sConnectString = ConnectionStringFrameWindowsAuthentication
                Else
                    r_sConnectString = ConnectionStringFrame
                End If

                ' Replace the placeholders with the correct values
                r_sConnectString = Replace(r_sConnectString, "{server}", sServer)
                r_sConnectString = Replace(r_sConnectString, "{database}", sDatabase)

                If bTrusted Then
                    'Do Nothing
                Else
                    '  m_sConnectString = m_sConnectString & "User ID=" & sUsername & ";"
                    '  m_sConnectString = m_sConnectString & "Password=" & sPassword & ";"
                    r_sConnectString = Replace(r_sConnectString, "{loginid}", sUsername)
                    r_sConnectString = Replace(r_sConnectString, "{loginpassword}", sPassword)

                End If

                _username = sUsername
                _server = sServer
                _database = sDatabase
            End If

            Return nResult
        Catch excep As System.Exception

            ' Error Section.
            nResult = gPMConstants.PMEReturnCode.PMError

            Return nResult
        End Try
    End Function
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
    Private Function Decrypt(sCipher As String, aKeys As Byte()) As String
        If sCipher = "" Then
            Return ""
        End If
        If sCipher Is Nothing Then
            Throw New ArgumentNullException("sCipher")
        End If

        'parse base64 string
        Dim aData As Byte() = Convert.FromBase64String(sCipher)

        'decrypt data
        Dim aDecrypted As Byte() = ProtectedData.Unprotect(aData, aKeys, kScope)
        Return Encoding.Unicode.GetString(aDecrypted)
    End Function
    Private Function GetDatabaseConnectItem(ByVal sDSN As String, ByVal sRegItem As String) As Object

        Try
            Dim vRegValue As Object

            vRegValue = QueryKeyValue(gpmConstants.HKEY_LOCAL_MACHINE,
                    gPMFunctions.BuildKeyString(
                    v_ePMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture,
                    v_ePMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer,
                    v_sSubKey:="\Databases\" & sDSN),
                    sRegItem)

            If TypeOf vRegValue Is String Then
                vRegValue = Trim(vRegValue)
            End If
            Return vRegValue
        Catch
            Return Nothing
        End Try

    End Function
#End Region

#Region "Public Shared Methods"

    ' Separate method because VB.NET does not support anonymous delegates.
    Public Shared Function GetInt32FromRow(ByVal row As IDataRecord) As Integer
        Return Cast.ToInt32(row(0), 0)
    End Function

    ' Separate method because VB.NET does not support anonymous delegates.
    Public Shared Function GetStringFromRow(ByVal row As IDataRecord) As String
        Return Cast.ToString(row(0))
    End Function

#End Region

#Region "ICloneable Methods"

    Public Function Clone() As Database

        Dim copy As New Database
        copy._server = Me._server
        copy._database = Me._database
        copy._username = Me._username
        copy._password = Me._password
        copy._connection = Me._connection.Clone()
        copy._userID = Me._userID
        Return copy

    End Function

    Private Function ICloneable_Clone() As Object Implements ICloneable.Clone

        Return Clone()

    End Function

#End Region

#Region "Object Methods"

    Public Overrides Function ToString() As String

        ' Return text suitable for writing to the event log.
        Dim server As String
        Dim database As String

        server = "(standard)"
        database = "(standard)"

        Return String.Format("Server = ""{0}"", Database = ""{1}"", UserName = ""{2}""", server, database, _username)

    End Function

#End Region

End Class
