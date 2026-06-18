Imports System
Imports System.ComponentModel
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI

<DefaultProperty("Text"), ToolboxData("<{0}:SessionTimeoutControl runat=server></{0}:SessionTimeoutControl>")> _
Public Class SessionTimeoutControl
    Inherits Control

    Private sRedirectUrl As String

    <Bindable(True), Category("Appearance"), DefaultValue("")> _
    Public Property RedirectUrl() As String
        Get
            Return sRedirectUrl
        End Get

        Set(ByVal value As String)
            sRedirectUrl = value
        End Set
    End Property

    'Protected Overrides Sub OnInit(ByVal e As EventArgs)

    '    If sRedirectUrl Is Nothing Then
    '        Throw New InvalidOperationException("RedirectUrl Property Not Set.")
    '    End If

    '    If Not (Context.Session Is Nothing) Then

    '        If Context.Session.IsNewSession Then
    '            'check for New Session

    '            Dim sCookieHeader As String = Page.Request.Headers("Cookie")

    '            If sCookieHeader IsNot Nothing AndAlso sCookieHeader.IndexOf("ASP.NET_SessionId") >= 0 Then

    '                Dim sRequestedPageURL As String = Page.Request.Url.Segments(Page.Request.Url.Segments.Length - 1).ToString
    '                Dim sPageswithoutAuthentication As String

    '                'these are the pages which user can access without Authentication
    '                'Currently i can see only single page(SelectBranch.aspx) which an agent can access without Authentication
    '                'if need, we can add more in future in the list seperated by ','
    '                sPageswithoutAuthentication = "SelectBranch.aspx"

    '                If Page.Request.IsAuthenticated Or sRequestedPageURL.Contains(sPageswithoutAuthentication) Then
    '                    'if user was authenticated then only redirect him(IsAuthenticated will help us to avoid the check for pages like Login.aspx, ForgotPassword.aspx or Register.aspx)

    '                    ' FormsAuthentication.SignOut()

    '                    Page.Response.Redirect(sRedirectUrl)

    '                End If

    '            End If

    '        End If
    '    End If

    'End Sub
    Protected Overrides Sub OnLoad(ByVal e As EventArgs)
        Page.Response.AppendHeader("Refresh", Convert.ToString((Context.Session.Timeout * 60)) + "; URL=" + ResolveUrl("~/SessionExpired.aspx"))
    End Sub

End Class