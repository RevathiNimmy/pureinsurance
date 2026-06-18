Imports Nexus.Library
Imports CMS.library
Imports Nexus.Utils
Imports System.Web.HttpContext
Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class secure_agent_FindReinsurer
        Inherits BaseFindParty

        Private oParty As New NexusProvider.PartyCollection


        Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

            Session(CNSearchType) = PartyType.INS
            FindParty()

        End Sub

        'Protected Sub grdvSearchResults_RowCommand1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvSearchResults.RowCommand
        '    Dim sResolvedName As String
        '    Dim sKey As String
        '    Dim sAgentCode As String
        '    For i As Integer = 0 To oParty.Count - 1
        '        If e.CommandArgument = oParty(i).ResolvedName Then
        '            sResolvedName = oParty(i).ResolvedName
        '            sKey = oParty(i).Key
        '            sAgentCode = oParty(i).UserName
        '            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.setReInsurer('" + sResolvedName + "','" + sKey + "','" + sAgentCode + "');", True)

        '        End If
        '    Next
        'End Sub

        Protected Sub grdvSearchResults_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvSearchResults.RowDataBound


            If e.Row.RowType = DataControlRowType.DataRow Then


                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("UserName") %>")
                Select Case True
                    Case TypeOf e.Row.DataItem Is NexusProvider.PersonalParty

                        e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.PersonalParty).UserName)

                        Dim oItem As NexusProvider.PersonalParty = CType(e.Row.DataItem, NexusProvider.PersonalParty)
                        oParty.Add(oItem)
                        Dim oAddress As NexusProvider.Address = oItem.Addresses(NexusProvider.AddressType.CorrespondenceAddress)
                        If oAddress IsNot Nothing Then

                            If oAddress.Address1 Is Nothing Then
                                CType(e.Row.FindControl("ltAddressLine1"), Literal).Text = "&nbsp;"
                            Else
                                CType(e.Row.FindControl("ltAddressLine1"), Literal).Text = oAddress.Address1
                            End If

                            If oAddress.Address2 Is Nothing Then
                                CType(e.Row.FindControl("ltAddressLine2"), Literal).Text = "&nbsp;"
                            Else
                                CType(e.Row.FindControl("ltAddressLine2"), Literal).Text = oAddress.Address2
                            End If

                        End If

                    Case TypeOf e.Row.DataItem Is NexusProvider.CorporateParty
                        e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.CorporateParty).UserName)

                        Dim oItem As NexusProvider.CorporateParty = CType(e.Row.DataItem, NexusProvider.CorporateParty)
                        oParty.Add(oItem)
                        Dim oAddress As NexusProvider.Address = oItem.Addresses(NexusProvider.AddressType.CorrespondenceAddress)

                        If oAddress.Address1 Is Nothing Then
                            CType(e.Row.FindControl("ltAddressLine1"), Literal).Text = "&nbsp;"
                        Else
                            CType(e.Row.FindControl("ltAddressLine1"), Literal).Text = oAddress.Address1
                        End If

                        If oAddress.Address2 Is Nothing Then
                            CType(e.Row.FindControl("ltAddressLine2"), Literal).Text = "&nbsp;"
                        Else
                            CType(e.Row.FindControl("ltAddressLine2"), Literal).Text = oAddress.Address2
                        End If

                End Select

            End If

        End Sub

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If IsPostBack Then
                grdvSearchResults.DataSource = Session(CNSearchData)
                grdvSearchResults.DataBind()
            End If
        End Sub
        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            grdvSearchResults.Visible = False
            Session.Remove(CNSearchData)
            txtReinsurerCode.Text = String.Empty
            txtName.Text = String.Empty
            txtFileCode.Text = String.Empty
            chkIncludeClosedBranches.Checked = False
        End Sub
    End Class

End Namespace