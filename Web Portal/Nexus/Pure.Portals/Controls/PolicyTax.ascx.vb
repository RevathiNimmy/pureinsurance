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

    Partial Class Controls_PolicyTax
        Inherits System.Web.UI.UserControl

        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim oPolicytax As NexusProvider.Tax
        Dim cTotalAmount As Double = 0.0

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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
            Try

                oQuote = Session(CNQuote)
                If oQuote IsNot Nothing Then
                    iInsuranceFilekey = oQuote.InsuranceFileKey
                    oQuote = Nothing
                    oQuote = oWebService.GetHeaderAndPolicyTaxByKey(iInsuranceFilekey)



                    If oQuote.PolicyTaxes IsNot Nothing And oQuote.PolicyTaxes.Count > 0 Then

                        grdvPolicyTax.DataSource = oQuote.PolicyTaxes
                        grdvPolicyTax.DataBind()
                        'this is used to calculate the total tax amount in the Quote.
                        Dim count As Integer
                        For count = 0 To oQuote.PolicyTaxes.Count - 1
                            cTotalAmount = oQuote.PolicyTaxes(count).TaxAmount + cTotalAmount

                            If oQuote.PolicyTaxes(count).IsValue = False Then
                                grdvPolicyTax.Rows(count).Cells(5).Text = Format(oQuote.PolicyTaxes(count).Rate, "0.00") + "%"
                            Else
                                grdvPolicyTax.Rows(count).Cells(5).Text = Format(oQuote.PolicyTaxes(count).Rate, "0.00")
                            End If
                        Next
                        lblTaxValue.Text = New Money(cTotalAmount, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString

                    Else
                        policytax_control.Visible = False
                        TaxAmountPanel.Visible = False

                    End If
                End If
                'Catch ex As Exception
            Finally
                oQuote = Nothing
                oWebService = Nothing
            End Try

        End Sub

        Protected Sub grdvPolicyTax_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvPolicyTax.Load
            If grdvPolicyTax.PageCount = 1 Then
                grdvPolicyTax.AllowPaging = False
            End If
        End Sub

        Protected Sub grdvPolicyTax_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvPolicyTax.RowDataBound
           
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim chkIsNotAppliedToClient As CheckBox = CType(e.Row.FindControl("IsNotAppliedToClient"), CheckBox)
                Dim chkIncludeInInstallment As CheckBox = CType(e.Row.FindControl("IncludeInInstallment"), CheckBox)
                Dim chkSpreadAcrossInstallment As CheckBox = CType(e.Row.FindControl("SpreadAcrossInstallment"), CheckBox)
                oPolicytax = CType(e.Row.DataItem, NexusProvider.Tax)
                chkIsNotAppliedToClient.Checked = oPolicytax.IsNotAppliedToClient
                chkIncludeInInstallment.Checked = oPolicytax.IncludeinInstallment
                chkSpreadAcrossInstallment.Checked = oPolicytax.SpreadAcrossInstallment
                oPolicytax = Nothing
            End If


        End Sub
    End Class

End Namespace