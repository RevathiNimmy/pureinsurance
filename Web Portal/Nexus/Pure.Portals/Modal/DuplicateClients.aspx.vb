Imports CMS.Library
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports Nexus.Utils
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class Modal_DuplicateClients
        Inherits System.Web.UI.Page
        Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub grdvSearchResults_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvSearchResults.RowCommand
            If e.CommandName = "Select" Then
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                Session(CNPartyKey) = e.CommandArgument
                oParty = oWebService.GetParty(e.CommandArgument)
                Session(CNParty) = oParty
                Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "Refresh") & ";"
                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "ParentPostBack", PostBackStr, True)
                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
            End If
        End Sub
        Protected Sub grdvSearchResults_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvSearchResults.RowDataBound

            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
                Dim oHyperLink As LinkButton = CType(e.Row.Cells(6).FindControl("lnkDetails"), LinkButton)

                Select Case True
                    Case TypeOf e.Row.DataItem Is NexusProvider.PersonalParty
                        'NOTE - this will need to be changed to give each row a unique id
                        'this needs to be matched in markup for the menu (id="Menu_<%# Eval("UserName") %>")
                        e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.PersonalParty).UserName)

                        Dim oItem As NexusProvider.PersonalParty = CType(e.Row.DataItem, NexusProvider.PersonalParty)
                        Dim oAddress As NexusProvider.Address = oItem.Addresses(NexusProvider.AddressType.CorrespondenceAddress)

                        If oAddress IsNot Nothing Then
                            If oAddress.PostCode.Trim().Length = 0 Then
                                CType(e.Row.FindControl("ltPostcode"), Literal).Text = "&nbsp;"
                            Else
                                CType(e.Row.FindControl("ltPostcode"), Literal).Text = oAddress.PostCode
                            End If
                        End If

                        CType(e.Row.FindControl("ltCustomerType"), Literal).Text = "Personal"

                        oHyperLink.CommandName = "Select"
                        oHyperLink.CommandArgument = CType(e.Row.DataItem, NexusProvider.BaseParty).Key
                        'oHyperLink.Text = GetLocalResourceObject("lbl_select").ToString()
                        'oHyperLink.PostBackUrl = "~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oItem.Key & "&Code=" & oItem.UserName & ""

                    Case TypeOf e.Row.DataItem Is NexusProvider.CorporateParty
                        'NOTE - this will need to be changed to give each row a unique id
                        'this needs to be matched in markup for the menu (id="Menu_<%# Eval("UserName") %>")
                        e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.CorporateParty).UserName)

                        Dim oItem As NexusProvider.CorporateParty = CType(e.Row.DataItem, NexusProvider.CorporateParty)
                        Dim oAddress As NexusProvider.Address = oItem.Addresses(NexusProvider.AddressType.CorrespondenceAddress)

                        If oAddress IsNot Nothing Then
                            If oAddress.PostCode.Trim().Length = 0 Then
                                CType(e.Row.FindControl("ltPostCode"), Literal).Text = "&nbsp;"
                            Else
                                CType(e.Row.FindControl("ltPostCode"), Literal).Text = oAddress.PostCode
                            End If
                        End If

                        CType(e.Row.FindControl("ltCustomerType"), Literal).Text = "Corporate"

                        oHyperLink.CommandName = "Select"
                        oHyperLink.CommandArgument = CType(e.Row.DataItem, NexusProvider.BaseParty).Key
                        'oHyperLink.Text = GetLocalResourceObject("lbl_select").ToString()
                        'oHyperLink.PostBackUrl = "~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oItem.Key & "&Code=" & oItem.UserName & ""
                    Case Else
                End Select
            End If

        End Sub
        Protected Sub grdvSearchResults_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvSearchResults.PageIndexChanging
            grdvSearchResults.PageIndex = e.NewPageIndex
            grdvSearchResults.DataSource = CType(Session(CNSearchData), NexusProvider.PartyCollection)
            grdvSearchResults.DataBind()
            
        End Sub
        Protected Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
            grdvSearchResults.DataSource = CType(Session(CNSearchData), NexusProvider.PartyCollection)
            grdvSearchResults.DataBind()
        End Sub
        
        Protected Sub btnIgnore_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIgnore.Click
            Session.Remove(CNPartyKey)
            Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "Ignore") & ";"
            Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
        End Sub
    End Class
End Namespace