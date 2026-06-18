Imports CMS.Library.Frontend
Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports System.Web.Configuration
Imports Nexus.Library
Imports Nexus.Utils
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports System.Security.Cryptography

Namespace Nexus
    Partial Class MasterPages_External_Modal : Inherits CMS.Library.Frontend.CMSMasterPage

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
        End Sub

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
            originalFileName = "bootstrap.min.js"

            hashedFileName = String.Format("~/js/libs/bootstrap/{0}", originalFileName)
            ' reference the new name of the file 
            ltrBootstrapTag.Text = "<script src='" + ResolveClientUrl(hashedFileName) + "' type='text/javascript'></script>"

            'To open a thickbox inside thickbox
            Page.Header.DataBind()

            'This will register a function for showing updatepanel errors as alert
            If ScriptManager.GetCurrent(Me.Page) IsNot Nothing Then
                If Not (Page.ClientScript.IsStartupScriptRegistered("AddEndRequestHandler")) Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "AddEndRequestHandler", "Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandlerForUpdatePanel);", True)
                End If
            End If
        End Sub

        Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
            Dim validatorOverrideScripts As String = ("<script src='" & (ResolveClientUrl("~/App_Themes/External/js/validators.js") & "' type='text/javascript'></script>"))
            Me.Page.ClientScript.RegisterStartupScript(Me.GetType, "ValidatorOverrideScripts", validatorOverrideScripts, False)
            MyBase.Render(writer)
        End Sub
    End Class

End Namespace

