Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Library
Imports CMS.Library
Imports System.Data
Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Xml
Imports Nexus.Utils
Imports Nexus
Namespace Nexus

    Partial Class Controls_PolicyFees
        Inherits System.Web.UI.UserControl

        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim cTotalAmount As Double = 0.0

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        End Sub

        Protected Sub grdvPolicyFees_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvPolicyFees.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim oProduct As Config.Product = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()).Products.GetProductByCode(oQuote.ProductCode)
                Dim oEditLink As LinkButton = e.Row.FindControl("hypPolicyFeeEdit")
                Dim oPolicyFee As NexusProvider.Fee = CType(e.Row.DataItem, NexusProvider.Fee)
                Dim lblRate As Label = e.Row.FindControl("lblRate")

                If lblRate IsNot Nothing Then
                    If oPolicyFee.IsValue = True Then
                        lblRate.Text = String.Format("{0:N2}", oPolicyFee.Rate)
                    Else
                        lblRate.Text = String.Format("{0:N2}%", oPolicyFee.Rate)
                    End If
                End If
                If oProduct.AllowEditFees Then
                    If Session(CNMode) IsNot Nothing Then
                        If CType(Session(CNMode), Mode) = Mode.View Or CType(Session(CNMode), Mode) = Mode.Review Then
                            oEditLink.Visible = False
                        Else
                            oEditLink.Visible = True
                            If HttpContext.Current.Session.IsCookieless Then
                                oEditLink.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/EditFee.aspx?Type=Policy&FeeKey=" & oPolicyFee.PolicyFeeKey & _
                                    "&IsValue=" & oPolicyFee.IsValue & "&Rate=" & oPolicyFee.Rate & "&IsProRated=" & oPolicyFee.IsProRated & "&ProRataRate=" & oPolicyFee.ProRataRate & _
                                    "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                            Else
                                oEditLink.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "Modal/EditFee.aspx?Type=Policy&FeeKey=" & oPolicyFee.PolicyFeeKey & "&IsValue=" & oPolicyFee.IsValue & _
                                    "&Rate=" & oPolicyFee.Rate & "&IsProRated=" & oPolicyFee.IsProRated & "&ProRataRate=" & oPolicyFee.ProRataRate & _
                                    "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                            End If

                        End If
                    Else
                        oEditLink.Visible = True
                        If HttpContext.Current.Session.IsCookieless Then
                            oEditLink.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/EditFee.aspx?Type=Policy&FeeKey=" & oPolicyFee.PolicyFeeKey & _
                                "&IsValue=" & oPolicyFee.IsValue & "&Rate=" & oPolicyFee.Rate & "&IsProRated=" & oPolicyFee.IsProRated & "&ProRataRate=" & oPolicyFee.ProRataRate & _
                                "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                        Else
                            oEditLink.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "Modal/EditFee.aspx?Type=Policy&FeeKey=" & oPolicyFee.PolicyFeeKey & "&IsValue=" & oPolicyFee.IsValue & _
                                "&Rate=" & oPolicyFee.Rate & "&IsProRated=" & oPolicyFee.IsProRated & "&ProRataRate=" & oPolicyFee.ProRataRate & _
                                "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                        End If
                    End If
                Else
                    oEditLink.Visible = False
                End If
            End If
        End Sub

        ''' <summary>
        ''' Logic moved from Page_Load to PreRender as Page_Load for a control use to execute before other event of parent page
        ''' While we need to load this after updating the quote from premium display
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
            Dim oQuote As NexusProvider.Quote
            Dim iInsuranceFilekey As Integer
            Dim InsuranceFileTypeCode As String = ""

            Try

                oQuote = Session(CNQuote)
                If oQuote IsNot Nothing Then

                    iInsuranceFilekey = oQuote.InsuranceFileKey
                    oQuote = oWebService.GetHeaderAndPolicyFeesByKey(iInsuranceFilekey)

                    oQuote.InsuranceFileTypeCode = InsuranceFileTypeCode
                    If oQuote.PolicyFees IsNot Nothing And oQuote.PolicyFees.Count > 0 Then

                        grdvPolicyFees.DataSource = oQuote.PolicyFees
                        grdvPolicyFees.DataBind()
                        'this is used to calculate the total Fee amount in the Quote.
                        Dim count As Integer
                        For count = 0 To oQuote.PolicyFees.Count - 1
                            cTotalAmount = oQuote.PolicyFees(count).TotalAmount + cTotalAmount
                        Next
                        lblFeeValue.Text = New Money(cTotalAmount, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
                    Else
                        policyfees_control.Visible = False
                        FeeAmountPanel.Visible = False

                        grdvPolicyFees.DataSource = Nothing
                        grdvPolicyFees.DataBind()
                    End If
                End If

            Finally

            End Try
        End Sub

    End Class

End Namespace
