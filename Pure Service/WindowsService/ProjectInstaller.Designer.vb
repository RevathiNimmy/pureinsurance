<System.ComponentModel.RunInstaller(True)> Partial Class ProjectInstaller
    Inherits System.Configuration.Install.Installer

    'Installer overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.serviceInstaller = New System.ServiceProcess.ServiceInstaller
        Me.serviceProcessInstaller = New System.ServiceProcess.ServiceProcessInstaller
        Me.eventLogInstaller = New System.Diagnostics.EventLogInstaller
        '
        'serviceInstaller
        '
        Me.serviceInstaller.Description = "The SSP Pure Insurance Windows Service that runs background jobs."
        Me.serviceInstaller.DisplayName = "SSP Pure Windows Service"
        Me.serviceInstaller.ServiceName = "PureWindowsService"
        '
        'serviceProcessInstaller
        '
        Me.serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem
        Me.serviceProcessInstaller.Password = Nothing
        Me.serviceProcessInstaller.Username = Nothing
        '
        'eventLogInstaller
        '
        Me.eventLogInstaller.CategoryCount = 0
        Me.eventLogInstaller.CategoryResourceFile = Nothing
        Me.eventLogInstaller.Log = "Application"
        Me.eventLogInstaller.MessageResourceFile = Nothing
        Me.eventLogInstaller.ParameterResourceFile = Nothing
        Me.eventLogInstaller.Source = "PureWindowsService"
        Me.eventLogInstaller.UninstallAction = System.Configuration.Install.UninstallAction.NoAction
        '
        'ProjectInstaller
        '
        Me.Installers.AddRange(New System.Configuration.Install.Installer() {Me.serviceInstaller, Me.serviceProcessInstaller, Me.eventLogInstaller})

    End Sub
    Friend WithEvents serviceInstaller As System.ServiceProcess.ServiceInstaller
    Friend WithEvents serviceProcessInstaller As System.ServiceProcess.ServiceProcessInstaller
    Friend WithEvents eventLogInstaller As System.Diagnostics.EventLogInstaller

End Class
