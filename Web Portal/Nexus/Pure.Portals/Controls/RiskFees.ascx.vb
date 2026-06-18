Imports Nexus.Utils
Imports Nexus.Library
Imports Nexus.Constants.Session
Imports Nexus.Constants
Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Namespace Nexus

    Partial Class Controls_RiskFees
        Inherits System.Web.UI.UserControl

        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim bIsInBackDatedMode As Boolean = False

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


            Dim oQuote As NexusProvider.Quote
            Dim iInsuranceFilekey As Integer
            Dim oHeaderandRisk As NexusProvider.HeaderAndRisk
            Dim sRisk As String
            Try
                bIsInBackDatedMode = IIf(Session(CNBaseInsuranceFileKey) IsNot Nothing AndAlso Session(CNBaseInsuranceFileKey) <> Session(CNInsuranceFileKey), True, False)
                oQuote = Session(CNQuote)
                iInsuranceFilekey = oQuote.InsuranceFileKey
                sRisk = Request.QueryString("Riskkey")
                If sRisk Is Nothing Then
                    sRisk = oQuote.Risks(Session(CNCurrentRiskKey)).Key
                End If

                If sRisk IsNot Nothing AndAlso oQuote.Risks.Count > 0 Then
                        oHeaderandRisk = oWebService.GetHeaderAndRiskFeesByKey(iInsuranceFilekey, CType(sRisk, Integer))
                        If oHeaderandRisk.RiskFees IsNot Nothing And oHeaderandRisk.RiskFees.Count > 0 Then
                            grdvRiskFees.DataSource = oHeaderandRisk.RiskFees
                            grdvRiskFees.DataBind()
                        Else
                            grdvRiskFees.DataSource = Nothing
                            grdvRiskFees.DataBind()
                        End If
                    Else
                        grdvRiskFees.DataSource = Nothing
                    grdvRiskFees.DataBind()
                End If

                'Catch ex As Exception
            Finally
            End Try

        End Sub

        Protected Sub grdvRiskFees_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvRiskFees.Load
            If grdvRiskFees.PageCount = 1 Then
                grdvRiskFees.AllowPaging = False
            End If
        End Sub



        Protected Sub grdvRiskFees_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvRiskFees.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim oProduct As Config.Product = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()).Products.GetProductByCode(oQuote.ProductCode)
                Dim oEditLink As LinkButton = e.Row.FindControl("hypRiskFeeEdit")
                Dim oRiskFee As NexusProvider.Fee = CType(e.Row.DataItem, NexusProvider.Fee)
                Dim chkIncludeInInstallment As CheckBox = CType(e.Row.FindControl("chkIncludeInInstallment"), CheckBox)
                Dim chkSpreadAcrossInstallment As CheckBox = CType(e.Row.FindControl("chkSpreadAcrossInstallment"), CheckBox)
                Dim lblRate As Label = e.Row.FindControl("lblRate")

                chkIncludeInInstallment.Checked = oRiskFee.IncludeInInstallment
                chkSpreadAcrossInstallment.Checked = oRiskFee.SpreadAcrossInstallment

                If lblRate IsNot Nothing Then
                    If oRiskFee.IsValue = True Then
                        lblRate.Text = String.Format("{0:N2}", oRiskFee.Rate)
                    Else
                        lblRate.Text = String.Format("{0:N2}%", oRiskFee.Rate)
                    End If
                End If

                If oProduct.AllowEditFees And Request.QueryString("Riskkey") Is Nothing And bIsInBackDatedMode = False Then
                    If Session(CNMode) IsNot Nothing Then
                        If CType(Session(CNMode), Mode) = Mode.View Or CType(Session(CNMode), Mode) = Mode.Review Then
                            oEditLink.Visible = False
                        Else
                            oEditLink.Visible = True
                            If HttpContext.Current.Session.IsCookieless Then
                                oEditLink.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/EditFee.aspx?Type=Risk&FeeKey=" & oRiskFee.RiskFeeKey & _
                                    "&IsValue=" & oRiskFee.IsValue & "&Rate=" & oRiskFee.Rate & "&IsProRated=" & oRiskFee.IsProRated & "&ProRataRate=" & oRiskFee.ProRataRate & _
                                    "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                            Else
                                oEditLink.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "Modal/EditFee.aspx?Type=Risk&FeeKey=" & oRiskFee.RiskFeeKey & "&IsValue=" & oRiskFee.IsValue & _
                                    "&Rate=" & oRiskFee.Rate & "&IsProRated=" & oRiskFee.IsProRated & "&ProRataRate=" & oRiskFee.ProRataRate & _
                                    "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                            End If
                        End If
                    Else
                        oEditLink.Visible = True

                        If HttpContext.Current.Session.IsCookieless Then
                            oEditLink.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/EditFee.aspx?Type=Risk&FeeKey=" & oRiskFee.RiskFeeKey & _
                                "&IsValue=" & oRiskFee.IsValue & "&Rate=" & oRiskFee.Rate & "&IsProRated=" & oRiskFee.IsProRated & "&ProRataRate=" & oRiskFee.ProRataRate & _
                                "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                        Else
                            oEditLink.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "Modal/EditFee.aspx?Type=Risk&FeeKey=" & oRiskFee.RiskFeeKey & "&IsValue=" & oRiskFee.IsValue & _
                                "&Rate=" & oRiskFee.Rate & "&IsProRated=" & oRiskFee.IsProRated & "&ProRataRate=" & oRiskFee.ProRataRate & _
                                "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                        End If
                    End If
                Else
                    oEditLink.Visible = False
                End If


            End If
        End Sub

    End Class

End Namespace
