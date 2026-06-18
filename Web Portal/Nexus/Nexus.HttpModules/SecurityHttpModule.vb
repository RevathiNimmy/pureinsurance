Imports System.Collections.Generic
Imports System.Text
Imports System.Web
Imports System.Text.RegularExpressions
Imports System.IO

Public Class SecurityHttpModule
    Implements IHttpModule
    Private Class RegexWithDesc
        Inherits Regex
        Private strErrorText As String

        Public ReadOnly Property ErrorText() As String
            Get
                Return strErrorText
            End Get
        End Property

        Public Sub New(regex As String, options As RegexOptions, errorText As String)
            MyBase.New(regex, options)
            strErrorText = errorText
        End Sub
    End Class

    Private strErrorMsg As String = System.Configuration.ConfigurationManager.AppSettings("SecurityViolation")
    Private strSTSUrl As String = System.Configuration.ConfigurationManager.AppSettings("STSURL")
    ' regex for default checks
    ' http://www.securityfocus.com/infocus/1768
    Shared regexOptions As RegexOptions = RegexOptions.Compiled Or RegexOptions.IgnoreCase Or RegexOptions.IgnorePatternWhitespace

    'Private regexCollection As RegexWithDesc() = New RegexWithDesc() {New RegexWithDesc("((¼|<)[^\n]+(>|¾)*)|javascript|unescape", regexOptions, "XSS 1"), New RegexWithDesc("(=)[^\n]*(\'|(\-\-)|(;))", regexOptions, "SQL 2"), New RegexWithDesc("(\')\s*(or|union|insert|delete|drop|update|create|waitfor|(declare\s+@\w+))", regexOptions, "SQL 4"), New RegexWithDesc("exec(((\s|\+)+(s|x)p\w+)|(\s@))", regexOptions, "SQL 5")}
    Private regexCollection As RegexWithDesc()

#Region "IHttpModule Members"

    Public Sub Dispose() Implements IHttpModule.Dispose
        ' nothing to do
    End Sub

    Public Sub Init(context As HttpApplication) Implements IHttpModule.Init
        AddHandler context.BeginRequest, New EventHandler(AddressOf context_BeginRequest)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub context_BeginRequest(sender As Object, e As EventArgs)
        Try
            Dim modules As HttpModuleCollection = HttpContext.Current.ApplicationInstance.Modules
            Dim authModule As IHttpModule = modules.Get("AuthHttpModule")
            If authModule Is Nothing Then
                Dim currentUrl As String = HttpContext.Current.Request.Url.ToString()
                If Not currentUrl.StartsWith(strSTSUrl) Then
                    UpdateRegex(regexCollection)
                    Dim toCheck As New List(Of String)()
                    Dim hashOrEncryptedPattern As String = "^[a-fA-F0-9]{32}$|^[a-fA-F0-9]{40}$|^[a-fA-F0-9]{64}$|^[A-Za-z0-9\-_]{40,}$"
                    Dim actionPattern As String = "^(Select|Delete)\$\d+$"


                    For Each key As String In HttpContext.Current.ApplicationInstance.Request.QueryString.AllKeys
                        toCheck.Add(HttpContext.Current.ApplicationInstance.Request(key))
                    Next
                    For Each key As String In HttpContext.Current.ApplicationInstance.Request.Form.AllKeys
                        If (key <> "ctl00$cntMainBody$txtDocumentEditor" AndAlso key <> "wresult") Then
                            toCheck.Add(HttpContext.Current.ApplicationInstance.Request.Form(key))
                        End If
                    Next
                    For Each regex As RegexWithDesc In regexCollection
                        For Each param As String In toCheck
                            Dim dp As String = HttpUtility.UrlDecode(param)
                            If RegularExpressions.Regex.IsMatch(dp, hashOrEncryptedPattern, RegexOptions.IgnoreCase) Then
                                Continue For
                            End If
                            If RegularExpressions.Regex.IsMatch(dp, actionPattern, RegexOptions.IgnoreCase) Then
                                Continue For
                            End If
                            ' Validate input unless it's from known safe values
                            If RegularExpressions.Regex.IsMatch(dp, regex.ToString, RegexOptions.IgnoreCase) AndAlso
            Not dp.Contains("AjaxControlToolkit") AndAlso
            Not dp.ToUpper.StartsWith("<FINDCONTROL>") AndAlso
             dp.Length < 1599 AndAlso
            Not dp.ToUpper.Contains("750'") AndAlso
            Not dp.ToUpper.Contains("PLEASE SELECT") Then
                                HttpContext.Current.ApplicationInstance.Response.Write(String.Format(strErrorMsg, regex.ErrorText))
                                HttpContext.Current.ApplicationInstance.CompleteRequest()
                                Return
                            End If

                        Next
                    Next
                End If
            End If
        Catch x As System.Threading.ThreadAbortException
            Throw
        Catch ex As Exception
            Dim sErrorMessage As String = ex.Message.ToString()
            HttpContext.Current.ApplicationInstance.Response.Write(String.Format(strErrorMsg, "Attack Vector Detected"))
            EventLog.WriteEntry("AttackVectorDetected", NexusProvider.ErrorFormatter.FormatErrorAsHtml(ex))
            HttpContext.Current.Response.End()
            Return
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="regexCollection"></param>
    Private Sub UpdateRegex(ByRef regexCollection As RegexWithDesc())

        Dim strCrossSiteRegex As String = System.Configuration.ConfigurationManager.AppSettings("CrossSiteRegex")
        Dim strSQL2Regex As String = System.Configuration.ConfigurationManager.AppSettings("SQL2Regex")
        Dim strSQL4Regex As String = System.Configuration.ConfigurationManager.AppSettings("SQL4Regex")
        Dim strSQL5Regex As String = System.Configuration.ConfigurationManager.AppSettings("SQL5Regex")

        If String.IsNullOrWhiteSpace(strCrossSiteRegex) Then
            strCrossSiteRegex = "((¼|<)[^\n]+(>|¾)*)|javascript|unescape"
        Else
            strCrossSiteRegex = strCrossSiteRegex.Trim
        End If
        If String.IsNullOrWhiteSpace(strSQL2Regex) Then
            strSQL2Regex = "(=)[^\n]*(\'|(\-\-)|(;))"
        Else
            strSQL2Regex = strSQL2Regex.Trim
        End If
        If String.IsNullOrWhiteSpace(strSQL4Regex) Then
            strSQL4Regex = "(\')\s*(or|union|insert|delete|drop|update|create|waitfor|(declare\s+@\w+))"
        Else
            strSQL4Regex = strSQL4Regex.Trim
        End If
        If String.IsNullOrWhiteSpace(strSQL5Regex) Then
            strSQL5Regex = "exec(((\s|\+)+(s|x)p\w+)|(\s@))"
        Else
            strSQL5Regex = strSQL5Regex.Trim
        End If

        regexCollection = New RegexWithDesc() {New RegexWithDesc(strCrossSiteRegex, regexOptions, "XSS 1"), New RegexWithDesc(strSQL2Regex, regexOptions, "SQL 2"), New RegexWithDesc(strSQL4Regex, regexOptions, "SQL 4"), New RegexWithDesc(strSQL5Regex, regexOptions, "SQL 5")}
    End Sub

#End Region
End Class
