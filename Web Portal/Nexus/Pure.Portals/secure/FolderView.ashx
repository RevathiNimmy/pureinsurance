<%@ WebHandler Language="VB" Class="FolderView" %>

Imports System
Imports System.Web

Public Class FolderView : Implements IHttpHandler, IRequiresSessionState
    
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "text/plain"
        context.Response.Write("Hello World")
        HttpContext.Current.Session.Add("somesessionvalue", " this is a test")
        context.Response.Write(HttpContext.Current.Session("somesessionvalue"))
        context.Response.Write(HttpContext.Current.User.Identity.Name.ToString)
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class