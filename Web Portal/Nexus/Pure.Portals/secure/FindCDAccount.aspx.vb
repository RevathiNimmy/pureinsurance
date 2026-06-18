Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Data
Imports CMS.library
Imports Nexus.Library
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Utils

Namespace Nexus
    Partial Class secure_FindCDAccount : Inherits CMS.Library.Frontend.clsCMSPage

        Dim oWebService As NexusProvider.ProviderBase
        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If IsPostBack = False Then
                ClearQuote()
                ClearClaims()
                txtClient.Focus()
            End If
            If Request.QueryString("cliendcode") IsNot Nothing And Not IsPostBack Then
                'If a Client code is passed then Client code textbox should be populated and the search results should be displayed on page load
                txtClient.Text = Request.QueryString("cliendcode")
                SearchCDAccount()
            End If
        End Sub

        Protected Sub grdvCDAccount_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvCDAccount.DataBound
            If grdvCDAccount.Rows.Count = 0 Or grdvCDAccount.PageCount = 1 Then
                grdvCDAccount.AllowPaging = False
            End If
        End Sub

        Protected Sub grdvCDAccount_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvCDAccount.PageIndexChanging
            grdvCDAccount.PageIndex = e.NewPageIndex
            'ToDo: Need to change the value from Session to any other best way
            grdvCDAccount.DataSource = Session("TempCDCashDeposit")
            grdvCDAccount.DataBind()
        End Sub

        Protected Sub btnNewSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewSearch.Click
            Response.Redirect("~\secure\FindCDAccount.aspx", False)
        End Sub

        Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
            If Page.IsValid Then
                SearchCDAccount()
            End If
        End Sub

        Sub SearchCDAccount()
            Dim oCashDeposit As NexusProvider.CashDepositCollection
            Dim sCashDepositRef, sBankCode, sSelectedPartyCode As String
            Dim iMaxRowsToFetch As Integer = oPortal.MaxSearchResults

            'Client and Agent are mutually exclusive search options
            If Not String.IsNullOrEmpty(txtClient.Text) Then
                sSelectedPartyCode = txtClient.Text
                btnAgent.Enabled = False
                txtAgentCode.Enabled = False
            Else
                sSelectedPartyCode = txtAgentCode.Text.Trim
                btnClient.Enabled = False
                txtClient.Enabled = False
            End If

            sCashDepositRef = txtCDNumber.Text.Trim
            sBankCode = GISLookup_CashListItemBank.Value

            oWebService = New NexusProvider.ProviderManager().Provider
            oCashDeposit = oWebService.FindCashDeposit(sSelectedPartyCode, sCashDepositRef, sBankCode, iMaxRowsToFetch)
            Session("TempCDCashDeposit") = oCashDeposit

            grdvCDAccount.AllowPaging = True
            grdvCDAccount.DataSource = oCashDeposit
            grdvCDAccount.DataBind()
            grdvCDAccount.Visible = True
            'validate size of dataset. if 500(configured at portal level) or more results are returned then add a validation message to the screen
            If oCashDeposit.Count >= oPortal.MaxSearchResults Then
                'create a custom validator
                Dim cstMaxResults As New CustomValidator
                cstMaxResults.IsValid = False
                'look for a validation message in the page resources, but if there is not one defined add a default message
                cstMaxResults.ErrorMessage = IIf(GetLocalResourceObject("cstMaxResults") Is Nothing, "Maximum number of search results exceeded, please refine your search criteria", GetLocalResourceObject("cstMaxResults"))
                cstMaxResults.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                'add the validator to the page, this will have the effect of making the page invalid
                Page.Validators.Add(cstMaxResults)
            End If

            'cleaning up
            oCashDeposit = Nothing
            oWebService = Nothing
            sCashDepositRef = Nothing
            sBankCode = Nothing
            oNexusConfig = Nothing
            oPortal = Nothing
        End Sub

        Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
            If Page.IsValid Then
                If CheckValidAgentorClient() = False Then
                    CustVld_ValidAgentorClient.IsValid = False
                    Exit Sub
                End If

                Dim sAgentCode As String = txtAgentCode.Text.Trim
                Dim sClientCode As String = txtClient.Text.Trim

                'Selected Party Code will go the CDAccountDetails page
                If Not String.IsNullOrEmpty(sAgentCode) Then
                    Response.Redirect("../secure/CDAccountDetails.aspx?PartyCode=" & sAgentCode, False)
                Else
                    Response.Redirect("../secure/CDAccountDetails.aspx?PartyCode=" & sClientCode, False)
                End If
            End If

        End Sub

        Protected Sub CustVld_AnagentorClient_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles CustVld_AnagentorClient.ServerValidate
            'Select an Agent or Client
            Dim sAgentCode As String = txtAgentCode.Text.Trim
            Dim sClientCode As String = txtClient.Text.Trim
            If String.IsNullOrEmpty(sAgentCode) And String.IsNullOrEmpty(sClientCode) Then
                args.IsValid = False
            Else
                args.IsValid = True
            End If
        End Sub

        Private Function CheckValidAgentorClient()
            'Validation for valid Agent or Client
            'Using Replace, we are searching for exactly Client/Agent Code
            Dim sAgentCode As String = txtAgentCode.Text.Trim.Replace("%", "")
            Dim sClientCode As String = txtClient.Text.Trim.Replace("%", "")

            Dim oPartySearchCriteria As New NexusProvider.PartySearchCriteria
            Dim oPartyCollection As NexusProvider.PartyCollection

            oWebService = New NexusProvider.ProviderManager().Provider

            If Not String.IsNullOrEmpty(sClientCode) Then
                'User has enterd Client Code so validation for valid Client
                oPartySearchCriteria.ShortName = sClientCode
                oPartySearchCriteria.PartyType = "GC"
                oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.PC)
                oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.CC)
                oPartyCollection = oWebService.FindParty(oPartySearchCriteria)

                If oPartyCollection.Count = 0 Then
                    'Entered Client Code is NOT valid, so send the error message
                    Return False
                End If
            ElseIf Not String.IsNullOrEmpty(sAgentCode) Then
                'User has enterd Agent Code so validation for valid agent
                oPartySearchCriteria = New NexusProvider.PartySearchCriteria
                oPartySearchCriteria.AgentType = Nothing
                oPartySearchCriteria.ShortName = sAgentCode
                oPartySearchCriteria.PartyType = NexusProvider.PartyTypeType.AG
                oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.AG)
                oPartyCollection = oWebService.FindParty(oPartySearchCriteria)

                If oPartyCollection.Count = 0 Then
                    'Entered Agent Code is NOT valid, so send the error message
                    Return False
                End If
            Else
                'try to avoid the situation where user has entered only '%%%'
                Return False
            End If

            'cleaning up
            oWebService = Nothing
            oPartySearchCriteria = Nothing
            oPartyCollection = Nothing
            Return True
        End Function

        Protected Sub grdvCDAccount_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvCDAccount.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("CashDepositKey") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.CashDeposit).CashDepositKey)
                Dim oItem As NexusProvider.CashDeposit = CType(e.Row.DataItem, NexusProvider.CashDeposit)
                e.Row.Attributes.Add("id", oItem.CashDepositKey)
                e.Row.Cells(2).Text = New Money(oItem.AvailableBalance, oItem.CurrencyCode).Formatted 'Available Balance
                Dim oHyperLink As LinkButton = CType(e.Row.Cells(7).FindControl("lnkSelect"), LinkButton)
                oHyperLink.PostBackUrl = "../secure/CDAccountDetails.aspx?CDAccount=" & CType(e.Row.DataItem, NexusProvider.CashDeposit).CashDepositRef & "&PartyCode=" & CType(e.Row.DataItem, NexusProvider.CashDeposit).PartyCode
                oHyperLink.PostBackUrl = "../secure/CDAccountDetails.aspx?CDAccount=" & oItem.CashDepositRef & "&PartyCode=" & oItem.PartyCode

            End If
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If HttpContext.Current.Session.IsCookieless Then
                'This will populate search client modal 
                btnClient.OnClientClick = "tb_show(null ,'../secure/agent/FindClient.aspx?RequestPage=BG&modal=true&KeepThis=true&TB_iframe=true&height=500&width=700' , null);return false;"
                'This will populate search agent modal 
                btnAgent.OnClientClick = "tb_show(null ,'../Modal/FindAgent.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            Else
                'This will populate search client modal 
                btnClient.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/secure/agent/FindClient.aspx?RequestPage=BG&modal=true&KeepThis=true&TB_iframe=true&height=500&width=700' , null);return false;"
                'This will populate search agent modal 
                btnAgent.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Modal/FindAgent.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            End If
        End Sub
    End Class
End Namespace
