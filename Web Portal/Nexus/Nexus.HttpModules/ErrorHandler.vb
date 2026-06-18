Imports System.Web
Imports System.Web.Configuration
Imports System.Web.HttpContext
Imports Nexus.Library.Config
Imports NexusProvider
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports System.Diagnostics


    ''' <summary>
    ''' HTTP module to handle any unhandled exceptions and display full debug information in browser
    ''' when in debug mode or email the information to an email address defined within the Nexus config
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ErrorHandler : Implements IHttpModule

        Public Sub ErrorHandler()

        End Sub

        Public Sub Dispose() Implements IHttpModule.Dispose

        End Sub

        Public Sub Init(ByVal app As HttpApplication) Implements IHttpModule.Init
            'Initialize, define eventhandlers
            AddHandler app.Error, AddressOf ApplicationError
        End Sub

        ''' <summary>
        ''' Handles the ApplicationError event, producing string containing all required debug information,
        ''' If debugging is enabled in web config this string is formatted as HTML and output to screen
        ''' Debug information is logged according to the logging configuration found in web.config
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub ApplicationError(ByVal sender As Object, ByVal e As EventArgs)

            Dim oCompilationSection As CompilationSection = CType(System.Web.Configuration.WebConfigurationManager.GetSection("system.web/compilation"), CompilationSection)

            If oCompilationSection.Debug Then
                'Debugging enabled - create HTML formatted error message for display on screen to allow easy debugging
                'output error message to screen for easy debugging
                Current.Response.Clear()
                Current.Response.Write(ErrorFormatter.FormatErrorAsHtml(Current.Server.GetLastError()))


                'log the error details according to the logging configuration
                Logger.Write(ErrorFormatter.FormatErrorAsText(Current.Server.GetLastError()), "Unhandled Exception", 1, 1, TraceEventType.Critical)
                
            If Current.Server.GetLastError() IsNot Nothing AndAlso Current.Server.GetLastError().ToString.Contains("potentially dangerous Request.Path") Then
             Current.Response.Redirect(String.Format(WebConfigurationManager.AppSettings("WebRoot") & "PotentiallyDangerous.aspx?aspxerrorpath={0}&ERef={1}", HttpUtility.UrlEncode(Current.Request.RawUrl), Guid.NewGuid().ToString()), True)
             End If
                'Clear the errors, to stop the default exception being displayed,
                'this will also stop the exception being logged in the event viewer,
                'but we don't need it anyway as we're in debug.
                Current.ClearError()
                Current.Response.End()
            Else
                'log the error details according to the logging configuration
                Logger.Write(ErrorFormatter.FormatErrorAsText(Current.Server.GetLastError()), Category.CriticalError, Priority.Highest, 1, TraceEventType.Critical)
            End If
        End Sub
    End Class
