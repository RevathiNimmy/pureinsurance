Imports Nexus.Library
Imports CMS.Library
Imports System.Data
Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Xml
Imports Nexus.Utils
Imports Nexus
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class Controls_RIAmendMultiRisk : Inherits System.Web.UI.UserControl

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            oWebService.GetHeaderAndRisksByKey(oQuote)

            Dim oRiskCollection As New NexusProvider.RiskCollection
            oRiskCollection = oQuote.Risks
            If oRiskCollection.Count > 1 Then
                oRiskCollection.SortColumn = "FolderKey"
                oRiskCollection.SortObjectType = GetType(NexusProvider.Risk)
                oRiskCollection.SortingOrder = SortDirection.Ascending
                oRiskCollection.Sort()
            End If

            If oQuote.Risks.Count > gvRisk.PageSize Then
                gvRisk.AllowPaging = True
            Else
                gvRisk.AllowPaging = False
            End If

            gvRisk.DataSource = oQuote.Risks
            gvRisk.DataBind()

            Dim olblTotalPremium As Label = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("lblPremiumValue"), Label)
            Dim m_cRoundOffAmount As Decimal = CheckAndCalculateRoundOff()
            olblTotalPremium.Text = New Money(m_cRoundOffAmount, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
            Session(CNQuote) = oQuote
        End Sub


        Protected Sub gvRisk_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvRisk.PageIndexChanging
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            gvRisk.PageIndex = e.NewPageIndex
            gvRisk.DataSource = oQuote.Risks
            gvRisk.DataBind()
        End Sub

        Protected Sub gvRisk_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvRisk.RowCommand
            If Not LCase(e.CommandName).Equals("page") And Not LCase(e.CommandName).Equals("sort") Then
                If e.CommandName = "EditRI" Then
                    'Get current risk and redirect to RI Screen
                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                    Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
                    Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)

                    Dim iRiskIndex As Integer = CInt(e.CommandArgument)
                    For iIndex As Integer = 0 To oQuote.Risks.Count - 1
                        If oQuote.Risks(iIndex).Key = CInt(e.CommandArgument) Then
                            iRiskIndex = iIndex
                            Exit For
                        End If
                    Next

                    Dim oRiskType As Config.RiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(iRiskIndex).RiskTypeCode.Trim)

                    Dim sStatusFlag As String = ""
                    If oQuote IsNot Nothing AndAlso oQuote.Risks IsNot Nothing AndAlso oQuote.Risks.Count > 0 AndAlso oQuote.Risks(iRiskIndex).RiskLinkStatusFlag IsNot Nothing Then
                        sStatusFlag = oQuote.Risks(iRiskIndex).RiskLinkStatusFlag.ToString.ToUpper()
                    End If
                    sStatusFlag = IIf(Constants.CheckStatusFlags.IndexOf(sStatusFlag) = -1, Nothing, sStatusFlag)

                    Dim oRisk As New NexusProvider.RiskType

                    oRisk.DataModelCode = oRiskType.DataModelCode
                    oRisk.Name = oRiskType.Name
                    oRisk.Path = oRiskType.Path
                    oRisk.RiskCode = oRiskType.RiskCode

                    Session(CNRiskType) = oRisk
                    Session(CNRiskMode) = RiskMode.PortfolioTransferAmendment
                    Session(CNCurrentRiskKey) = iRiskIndex
                    'Get the risk data after creating a copy. A new copy of risk will created if bIsForEdit=true passed to getrisk function.
                    oWebService.GetRisk(oQuote.Risks(iRiskIndex).Key, iRiskIndex, oQuote, oQuote.BranchCode, False)
                    oWebService = Nothing
                    Session(CNQuote) = oQuote
                    Response.Redirect("RiskDetails.aspx")
                End If

            End If

        End Sub

        ''' <summary>
        ''' Change the status code to descriptive text by getting the text from the resource file.
        ''' </summary>
        ''' <param name="code">Link Status Code</param>
        ''' <returns>Link Status Description from the resource file</returns>
        ''' <remarks>Returns - if code not found.</remarks>
        Private Function GetLinkStatusText(ByVal code As String) As String

            Dim sDescription As String = Convert.ToString(GetLocalResourceObject("lbl_ChangedStatus_" & code))
            If String.IsNullOrEmpty(sDescription) Then
                sDescription = "-"
            End If
            GetLinkStatusText = sDescription

        End Function
    End Class

End Namespace
