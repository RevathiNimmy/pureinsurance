Imports System
Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Tcp
Imports System.Runtime.Remoting.Lifetime
Imports SharedFiles
Friend Class Program
    Shared Sub Main(ByVal args As String())
        Try
            Const portnumberconstant = 65535
            LifetimeServices.LeaseTime = TimeSpan.FromMinutes(10)
            LifetimeServices.LeaseManagerPollTime = TimeSpan.FromMinutes(10)
            LifetimeServices.RenewOnCallTime = TimeSpan.FromMinutes(5)
            LifetimeServices.SponsorshipTimeout = TimeSpan.FromMinutes(10)
            Dim iSessionId As Integer = Process.GetCurrentProcess.SessionId
            Dim provider As New BinaryServerFormatterSinkProvider
            provider.TypeFilterLevel = Runtime.Serialization.Formatters.TypeFilterLevel.Full
            Dim props As New Hashtable
            props.Add("port", (portnumberconstant - iSessionId))
            ChannelServices.RegisterChannel(New TcpChannel(props, Nothing, provider), False)
            'Dim m_ologonManager = New iLogonManager.LogonManager
            'Dim ch As TcpChannel = New TcpChannel(8085)
            'ChannelServices.RegisterChannel(ch, False)
            RemotingConfiguration.RegisterWellKnownServiceType(GetType(iLogonManager.LogonManager), "SSP", WellKnownObjectMode.Singleton)
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError.PMLogDebug4, sMsg:="Port id is " & "localhost:" & Trim(iSessionId), vApp:=ACApp, vClass:=ACClass)

            Threading.Thread.Sleep(Threading.Timeout.Infinite)
        Catch Ex As Exception
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start ILogonServer", vApp:=ACApp, vClass:=ACClass, vMethod:="Main", excep:=Ex)
        End Try
    End Sub

End Class
