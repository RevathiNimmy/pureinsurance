Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.MembershipProvider


Namespace Nexus

    Partial Class Controls_MultiBranch : Inherits System.Web.UI.UserControl


       
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not IsPostBack Then
                If CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches.Count > 1 Then
                    If GetLocalResourceObject("ddl_Branchlst_defaulttext").ToString().Trim.Length <> 0 Then
                        'if client has give any default value than only add
                        ddlBranchlst.Items.Insert(0, New ListItem(GetLocalResourceObject("ddl_Branchlst_defaulttext"), ""))
                    End If
                    Dim oBranchs As NexusProvider.BranchCollection = CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches
                    If oBranchs IsNot Nothing Then
                        'Sort the collection before binding
                        oBranchs.SortColumn = "Description"
                        oBranchs.SortingOrder = NexusProvider.GenericComparer.SortOrder.Ascending
                        oBranchs.Sort()
                        For Each oBranch As NexusProvider.Branch In oBranchs
                            ddlBranchlst.Items.Add(New ListItem(oBranch.Description, oBranch.Code))
                        Next
                    End If
                    If Session(CNBranchCode) IsNot Nothing Then
                        ddlBranchlst.SelectedValue = Session(CNBranchCode).ToString()
                    End If
                End If

                'To set the Focus
                Page.SetFocus(ddlBranchlst)
            End If

        End Sub

        Protected Sub btnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNext.Click

            If Request.QueryString("Mode") = "IP" Then
                Response.Redirect("~/secure/payment/cashlist.aspx?Mode=IP&BC=" + ddlBranchlst.SelectedValue, False)
            Else
                ' Need to log In
                Dim sUserName As String = Session(CNLoginName).ToString().Trim()
                Session(CNBranchCode) = ddlBranchlst.SelectedValue

                If String.IsNullOrEmpty(HttpContext.Current.Request.QueryString("ReturnUrl")) Then
                    Dim oNexusMembershipProvider As New NexusMembershipProvider
                    If CType(GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).AllowMultipleLogins = False Then
                        oNexusMembershipProvider.InsertSessionIDforUser(sUserName)
                    Else
                        FormsAuthentication.SetAuthCookie(sUserName, False)
                    End If
                    Dim sAgentStartPage As String = CType(GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).AgentStartPage
                    Response.Redirect(sAgentStartPage, False)
                Else
                    FormsAuthentication.RedirectFromLoginPage(sUserName, False)
                End If

            End If

        End Sub

    End Class

End Namespace
