Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports System.Timers
Imports System.Net
Imports System.Xml
Imports System.Xml.Linq
Imports Ssp.PureInsuranceRestAPIHandler.BaseClasses
Imports Ssp.PureInsuranceRestAPIHandler
Imports System.Linq
''' <summary>
''' The Windows service class.
''' </summary>
Public Class Service

#Region "Private Variables"

    Private _info As New BackgroundJobInfo
    Private _threads() As Thread
    Private _extraThreadCount As Integer
    Private _threadShutdownTimeout As TimeSpan
    Private _userName As String
    Private _timer As Timer
#End Region

#Region "Protected Methods"

    Protected Overrides Sub OnStart(ByVal args() As String)

        '#If DEBUG Then
        '        ' Break into the debugger.
        '         Debugger.Break()
        '#End If

        Try
            InitaliseSettings()

            BackgroundJobProcess.StopProcess = False

            ReDim _threads(0 To _extraThreadCount) ' one mandatory thread plus extra threads as per the app.config file

            Dim logEntry As New LogEntry
            logEntry.Message = My.Resources.OnStartInfo
            logEntry.Severity = TraceEventType.Information
            logEntry.Categories.Add("NonDebug")
            logEntry.ExtendedProperties.Add("Version", Assembly.GetExecutingAssembly.GetName.Version)
            logEntry.ExtendedProperties.Add("Message Batch Size", _info.MessageBatchSize)
            logEntry.ExtendedProperties.Add("Additional Threads", _extraThreadCount)
            logEntry.ExtendedProperties.Add("User Name", _userName)
            Logger.Write(logEntry)

            For i As Integer = 0 To _threads.Length - 1
                Dim process As New BackgroundJobProcess(_info)
                _threads(i) = New Thread(AddressOf process.StartProcessing)
                _threads(i).Name = i.ToString()
                _threads(i).Start()
            Next

        Catch ex As Exception
            ' We must have a handler here because it seems that throwing an exception out of this method
            ' does abort the service, but with no error dialog or any other trace of what went wrong.
            If ExceptionPolicy.HandleException(ex, "Log and Absorb") Then
                Throw
            End If
            Me.Stop()
        End Try

    End Sub

    Protected Overrides Sub OnStop()

        BackgroundJobProcess.StopProcess = True

        If _threads IsNot Nothing Then
            For i As Integer = 0 To _threads.Length - 1
                _threads(i).Join(_threadShutdownTimeout)
            Next
        End If

        Dim logEntry As New LogEntry
        logEntry.Message = My.Resources.OnStopInfo
        logEntry.Severity = TraceEventType.Information
        logEntry.Categories.Add("NonDebug")
        Logger.Write(logEntry)

    End Sub

#End Region

#Region "Private Methods - Read Config Data"

    Private Sub InitaliseSettings()

        Dim database As New Database

        _info.Source = System.Environment.MachineName.ToString

        '_userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name

        _userName = ConfigurationManager.AppSettings("SiriusLoginUserName")
        If _userName.Length = 0 Then
            _userName = SiriusUserDefaults.Username
        End If

        _info.Database = database
        _extraThreadCount = XmlSafeConvert.ToInt32(ConfigurationManager.AppSettings("AdditionalNumberOfThreads"), 0)
        _threadShutdownTimeout = TimeSpan.FromSeconds(XmlSafeConvert.ToInt32(ConfigurationManager.AppSettings("ThreadShutdownTimeout"), 20))
        _info.MessageBatchSize = 1  ' Default to 1
        _info.PollingDelay = TimeSpan.FromMilliseconds(XmlSafeConvert.ToInt32(ConfigurationManager.AppSettings("PollingFrequency"), 10000))
        _info.RetryDelay = TimeSpan.FromHours(XmlSafeConvert.ToInt32(ConfigurationManager.AppSettings("PolicyUpdateRetryFrequency"), 1))
        BackgroundJobProcess.ServiceRetryLimitForEXWRKITEM = XmlSafeConvert.ToInt32(ConfigurationManager.AppSettings("EXWRKITEM"), 0)
        BackgroundJobProcess.WaitMiuntesBeforeRetry = XmlSafeConvert.ToInt32(ConfigurationManager.AppSettings("WaitMiuntesBeforeRetry"), 0)

        If TimeSpan.FromMinutes(XmlSafeConvert.ToInt32(ConfigurationManager.AppSettings("WCFPingAfter"))).TotalMilliseconds > 0 Then
            _timer = New Timer
            AddHandler _timer.Elapsed, New ElapsedEventHandler(AddressOf Elapsed_Timer)
            _timer.Interval = TimeSpan.FromMinutes(XmlSafeConvert.ToInt32(ConfigurationManager.AppSettings("WCFPingAfter"))).TotalMilliseconds
            _timer.Enabled = True
        End If
    End Sub

    Protected Sub Elapsed_Timer(sender As Object, e As System.Timers.ElapsedEventArgs)
        ReloadWebService()
    End Sub

    Private Sub ReloadWebService()

        Dim m_sURL As String = ConfigurationManager.AppSettings("ApiEndpoint")

        Try
            Dim request As New LoadServiceQuery()
            request.LoginUserName = _userName
            request.CallingApp = "PureWindowsService"
            ApiClient._tokenModel = GetApiTokendetails()
            Dim returnValue As Integer = ApiClient.DeserializeJson(Of Integer)(CStr(ApiClient.Get($"/messaging/loadService", request)))
        Catch
        End Try
    End Sub
    Private Function GetApiTokendetails() As TokenModel
        Dim apiTokenDetails As TokenModel = New TokenModel()
        Dim tokenUrl As String = ConfigurationManager.AppSettings("TokenUrl")
        apiTokenDetails = GenerateToken.GetJwtTokenForBatchProcess(ConfigurationManager.AppSettings("ClientId"), tokenUrl)
        Dim address As String = ConfigurationManager.AppSettings("ApiEndpoint")
        If address.EndsWith("/") Then
            address = address.Substring(0, address.Length - 1)
        End If
        apiTokenDetails.ApiBaseUrl = address
        apiTokenDetails.TokenUrl = tokenUrl
        Return apiTokenDetails
    End Function
#End Region

End Class
