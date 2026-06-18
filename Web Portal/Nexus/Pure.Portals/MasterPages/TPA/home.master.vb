Imports CMS.Library.Frontend
Imports System.IO
Imports NexusProvider
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Security.Cryptography

Namespace Nexus
    Partial Class MasterPages_demo_home : Inherits CMSMasterPage

        Public Function GenerateHashFileName(fileName As String) As String
            Dim returnHashString As String
            ' Use SHA256 to hash the filename
            Using sha256 As SHA256 = SHA256.Create()
                Dim sourceBytes As Byte() = Encoding.UTF8.GetBytes(fileName)
                Dim hashBytes As Byte() = sha256.ComputeHash(sourceBytes)
                Dim sb As New StringBuilder()
                For Each b As Byte In hashBytes
                    sb.Append(b.ToString("x2")) ' Convert to hex
                Next
                returnHashString = sb.ToString().Substring(0, 8) ' Return hashed filename
                Return returnHashString

            End Using
        End Function
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim originalFileName As String
            Dim hashedFileName As String

            originalFileName = "jquery.min.js"

            ' Get hashed filename
            hashedFileName = String.Format("~/js/libs/jquery/{0}", originalFileName)
            ' Dynamically reference the hashed file in the page
            ltrScriptTag.Text = "<script src='" + ResolveClientUrl(hashedFileName) + "' type='text/javascript'></script>"
            
            'If My.User.IsAuthenticated Then
            '    Response.Redirect("~/main.aspx")
            'End If
            Page.Header.DataBind()
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            'DH - debug purposes only
            'Controls.Add(New LiteralControl(ErrorFormatter.GetSessionAsHtml()))
        End Sub
    End Class
End Namespace

