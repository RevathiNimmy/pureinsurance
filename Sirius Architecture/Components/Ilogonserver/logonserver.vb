Imports System
Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Tcp
Imports System.Runtime.Remoting.Lifetime
Imports SharedFiles

Public Class logonserver

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Const portnumberconstant = 65535
            LifetimeServices.LeaseTime = TimeSpan.FromMinutes(10)
            LifetimeServices.LeaseManagerPollTime = TimeSpan.FromMinutes(10)
            LifetimeServices.RenewOnCallTime = TimeSpan.FromMinutes(5)
            LifetimeServices.SponsorshipTimeout = TimeSpan.FromMinutes(10)
            Dim provider As New BinaryServerFormatterSinkProvider
            provider.TypeFilterLevel = Runtime.Serialization.Formatters.TypeFilterLevel.Full
            Dim props As New Hashtable
            props.Add("port", (portnumberconstant - Process.GetCurrentProcess.SessionId))
            ChannelServices.RegisterChannel(New TcpChannel(props, Nothing, provider), False)

            'Dim m_ologonManager = New iLogonManager.LogonManager
            'Dim ch As TcpChannel = New TcpChannel(8085)
            'ChannelServices.RegisterChannel(ch, False)
            RemotingConfiguration.RegisterWellKnownServiceType(GetType(iLogonManager.LogonManager), "SSP", WellKnownObjectMode.Singleton)
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogDebug4, sMsg:="Port id is" + (portnumberconstant - Process.GetCurrentProcess.SessionId), vApp:=ACApp, vClass:=ACClass, vMethod:="", excep:=New Exception("Port id is" + (portnumberconstant - Process.GetCurrentProcess.SessionId)))
            Threading.Thread.Sleep(Threading.Timeout.Infinite)
            'Console.Write("server is ready............")
            'Console.Read()
            'Console.SetWindowSize(0, 0)
            'GC.Collect()
            'GC.WaitForPendingFinalizers()
        Catch Ex As Exception
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start ILogonServer", vApp:=ACApp, vClass:=ACClass, vMethod:="Load", excep:=Ex)
        End Try
    End Sub
End Class
