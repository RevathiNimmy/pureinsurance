Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports Nexus.Library
Imports System.Web.Configuration
Imports Nexus.Utils
Imports System.Web.HttpContext
Imports Nexus.Constants.Session
Imports Nexus.Constants

Namespace Nexus
    Partial Class Modal_PolicySummary
        Inherits System.Web.UI.Page
        Dim oWebService As NexusProvider.ProviderBase = Nothing
        Private oMaster As ContentPlaceHolder
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                Dim oMaster As ContentPlaceHolder
                Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oProducts As Config.Products = CType(WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Products
                Dim oAgentSettings As NexusProvider.AgentSettings = Nothing

                Dim oRiskTypes As NexusProvider.RiskType = Session(CNRiskType)
                oWebService = New NexusProvider.ProviderManager().Provider
                oMaster = GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName)

                Me.POLICYHEADER__PRODUCT.Text = oQuote.ProductCode
                Me.POLICYHEADER__AGENTCODE.Text = oQuote.AgentCode
               
                'Population of Cover Dates
                With oQuote
                    POLICYHEADER__COVERSTARTDATE.Text = .CoverStartDate
                    POLICYHEADER__COVERENDDATE.Text = .CoverEndDate
                    POLICYHEADER__INCEPTION.Text = .InceptionDate
                    POLICYHEADER__INCEPTIONTPI.Text = .InceptionTPI
                    POLICYHEADER__RENEWAL.Text = .RenewalDate
                    POLICYHEADER__QUOTEEXPIRYDATE.Text = .QuoteExpiryDate
                    POLICYHEADER__PROPOSALDATE.Text = .ProposalDate

                    POLICYHEADER__ALTERNATEREF.Text = .AlternativeRef
                    POLICYHEADER__ANALYSISCODE.Text = .AnalysisCode
                End With

                With oQuote
                    Me.POLICYHEADER__INSUREDNAME.Text = .InsuredName
                    Me.POLICYHEADER__POLICYNUMBER.Text = .InsuranceFileRef
                    Me.POLICYHEADER__BUSINESSTYPE.Text = .BusinessTypeCode
                    Me.POLICYHEADER__BRANCH.Text = .BranchCode
                    Me.POLICYHEADER__SUBBRANCH.Text = .SubBranchCode
                    Me.POLICYHEADER__CURRENCY.Text = Trim(.CurrencyCode)
                    Me.POLICYHEADER__HANDLER.Text = .AccountHandlerName
                    Me.POLICYHEADER__REGARDING.Text = .Regarding
                    Me.POLICYHEADER__QUOTE.Checked = .QuoteIsLocked
                    Me.POLICYHEADER__HCEXPIRY.Text = .HCExpiryDate
                    Me.POLICYHEADER__DEDUCTIBLES.Text = .PolicyDeductible
                    Me.POLICYHEADER__UNDERWRITINGYEAR.Text = .UnderwritingYear
                    Me.POLICYHEADER__POLICYLIMITS.Text = .PolicyLimits
                    oWebService = New NexusProvider.ProviderManager().Provider
                    oWebService.GetHeaderAndRisksByKey(oQuote)
                    txtNetTotal.Text = oQuote.NetTotal.ToString(Format("0.00"))
                    txtFeeTotal.Text = oQuote.FeeTotal.ToString(Format("0.00"))
                    txtTaxTotal.Text = oQuote.TaxTotal.ToString(Format("0.00"))
                    txtGrossTotal.Text = oQuote.GrossTotal.ToString(Format("0.00"))
                End With

                Dim oAllowPolicyClientAssociationsOptionSettings As NexusProvider.OptionTypeSetting
                oAllowPolicyClientAssociationsOptionSettings = CType(ViewState("AllowPolicyClientAssociationsOptionSettings"), NexusProvider.OptionTypeSetting)
                oAllowPolicyClientAssociationsOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.AllowPolicyClientAssociations)

                If oAllowPolicyClientAssociationsOptionSettings.OptionValue = "1" Then
                    Dim oGetPolicyAssociateCollection As NexusProvider.PolicyAssociateCollection = New NexusProvider.PolicyAssociateCollection

                    oMaster = GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName)
                    Dim hLnk As HyperLink = oMaster.FindControl("hypPolicyAssociate")
                    oGetPolicyAssociateCollection = oWebService.GetPolicyAssociates(oQuote.InsuranceFileKey, oQuote.InsuranceFolderKey, Nothing)
                    rptrPolicyAssociate.DataSource = oGetPolicyAssociateCollection
                    rptrPolicyAssociate.DataBind()
               
                End If
            End If
        End Sub

        Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
        End Sub
        Protected Sub rptrPolicyAssociate_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptrPolicyAssociate.ItemDataBound
            If (e.Item.ItemType.ToString().ToLower() = "item" OrElse e.Item.ItemType.ToString().ToLower() = "alternatingitem") Then
                lbPolicyAssociates.Items.Add(CType(e.Item.DataItem, NexusProvider.PolicyAssociate).PartyName.ToString)
                lbPolicyAssociates.DataBind()
            End If
        End Sub
    End Class
End Namespace
