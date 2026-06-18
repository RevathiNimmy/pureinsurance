<%@ WebHandler Language="VB" Class="FileHandler" %>

Imports System
Imports System.Web
Imports System.Net
Imports System.IO

Public Class FileHandler : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim pathAndFilename As String = context.Request("filename")
        Dim filename As String = System.IO.Path.GetFileName(pathAndFilename)
        context.Response.ClearContent()
        Dim webClient As WebClient = New WebClient()

        Using stream As Stream = webClient.OpenRead(pathAndFilename)
            Dim data1 As Byte() = New Byte(stream.Length - 1) {}
            stream.Read(data1, 0, data1.Length)
            context.Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", filename))
            context.Response.BinaryWrite(data1)
            context.Response.Flush()
            context.Response.SuppressContent = True
            context.ApplicationInstance.CompleteRequest()
        End Using
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class