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
    Partial Class MasterPages_External_main : Inherits CMSMasterPage
        Dim RestrictedClientdetailbox1URLs, RestrictedClientdetailbox2URLs, RestrictedPolicyRefCleanURL As String

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

            Page.Header.DataBind()

            'This will register a function for showing updatepanel errors as alert
            If ScriptManager.GetCurrent(Me.Page) IsNot Nothing Then
                If Not (Page.ClientScript.IsStartupScriptRegistered("AddEndRequestHandler")) Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "AddEndRequestHandler", "Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandlerForUpdatePanel);", True)
                End If
            End If
            Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
            If oUserDetails IsNot Nothing Then
                ' Added to override defualt portal id as per user type       
                If oUserDetails.Key = 0 Then
                    'set PortalID for internal portal & app_themes
                    HttpContext.Current.Session("PortalID") = 1
                Else
                    'set PortalID  for Broker portal
                    If oUserDetails.PartyType = "AG" Then
                        HttpContext.Current.Session("PortalID") = 2
                    Else
                        HttpContext.Current.Session("PortalID") = 3 ' If user is of Third Party Type 
                    End If
                End If
                HttpContext.Current.Cache.Insert("PortalID", Session("PortalID"), Nothing, Now.AddHours(4), TimeSpan.Zero)

                'If Session portalid set first time and agent has more than 1 branches then need to reload page to set themes
                Dim sAgentStartPage As String = CType(GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).AgentStartPage
                If Request.UrlReferrer Is Nothing AndAlso Session("HasThemesSet") <> 1 Then
                    If Request.CurrentExecutionFilePath.ToUpper.Contains("/SELECTBRANCH.ASPX") Then
                        Session("HasThemesSet") = 1
                        Response.Redirect("~/SelectBranch.aspx", False)
                    Else
                        Session("HasThemesSet") = 1
                        Response.Redirect(sAgentStartPage, False)
                    End If
                End If

                If Not Request.CurrentExecutionFilePath.ToUpper.Contains("/SELECTBRANCH.ASPX") Then
                    If Session(CNBranchCode) Is Nothing Then
                        Response.Redirect("~/SelectBranch.aspx", False)
                    End If
                End If
            End If
        End Sub



        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            'Remove comment below for debugging
            'Controls.Add(New LiteralControl(NexusProvider.ErrorFormatter.GetSessionAsHtml()))
        End Sub

        Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
            Dim validatorOverrideScripts As String = ("<script src='" & (ResolveClientUrl("~/App_Themes/External/js/validators.js") & "' type='text/javascript'></script>"))
            Me.Page.ClientScript.RegisterStartupScript(Me.GetType, "ValidatorOverrideScripts", validatorOverrideScripts, False)
            MyBase.Render(writer)
        End Sub



    End Class
End Namespace

