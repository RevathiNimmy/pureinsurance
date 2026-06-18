Imports CMS.Library
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports Nexus.Utils
Imports System.Web.Configuration
Imports System.Exception
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session


Namespace Nexus
    Partial Class secure_CoInsurance : Inherits Frontend.clsCMSPage
        Dim Total As Double = 0.0

        Protected Sub drgCoInsurance_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        End Sub
        Protected Sub BindCoInsuranceData()
            Total = 0.0
            Dim CoInsurers As NexusProvider.CoInsurersCollections = CType(Session.Item("CoInsurance"), NexusProvider.CoInsurersCollections)
            drgCoInsurance.DataSource = CoInsurers
            drgCoInsurance.DataBind()
            hidTotalShare.Value = Total
            lblTotalShare.Text = Total.ToString("f2") & "%" 'Total.ToString
        End Sub
        Protected Sub drgCoInsurance_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles drgCoInsurance.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("CoInsurerKey") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.CoInsurers).CoInsurerKey)

                Dim hypEdit As LinkButton = e.Row.FindControl("hypCoInsuranceEdit")
                If HttpContext.Current.Session.IsCookieless Then
                    hypEdit.OnClientClick = "tb_show(null ,& " & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/CoInsurance.aspx?PostbackTo=" & UpdCoInsurer.ClientID.ToString & "&CoInsuranceID=" & e.Row.RowIndex & "&modal=true&KeepThis=true&TB_iframe=true&height=600&width=750' , null);return false;"
                Else
                    hypEdit.OnClientClick = "tb_show(null , '../Modal/CoInsurance.aspx?PostbackTo=" & UpdCoInsurer.ClientID.ToString & "&CoInsuranceID=" & e.Row.RowIndex & "&modal=true&KeepThis=true&TB_iframe=true&height=600&width=750' , null);return false;"
                End If
                Total = Total + CType(e.Row.DataItem, NexusProvider.CoInsurers).SharePerc
            End If
        End Sub
        Protected Sub drgCoInsurance_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles drgCoInsurance.RowDeleting
            CType(Session.Item("CoInsurance"), NexusProvider.CoInsurersCollections).Remove(e.RowIndex)
            BindCoInsuranceData()
        End Sub
        Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            chkIsRecovered.Checked = True
            chkIsSurcharged.Checked = True
            chkIsRecovered.Enabled = False
            chkIsSurcharged.Enabled = False
            Session(CNCoInsurancePage) = True
            If HttpContext.Current.Session.IsCookieless Then
                hypCoInsurance.OnClientClick = "tb_show(null ,  '" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/CoInsurance.aspx?PostbackTo=" & UpdCoInsurer.ClientID.ToString & "&modal=true&KeepThis=true&TB_iframe=true&height=600&width=750' , null);return false;"
            Else
                hypCoInsurance.OnClientClick = "tb_show(null , '../Modal/CoInsurance.aspx?PostbackTo=" & UpdCoInsurer.ClientID.ToString & "&modal=true&KeepThis=true&TB_iframe=true&height=600&width=750' , null);return false;"
            End If

            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            If Session("CoInsurance") Is Nothing Then
                Session.Add("CoInsurance", New NexusProvider.CoInsurersCollections)
            End If
            If Not IsPostBack Then
                Dim InsuranceFileKey As Integer = oQuote.InsuranceFileKey
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oresponse As NexusProvider.CoinsuranceDefaults
                oresponse = oWebService.GetCoInsuranceValues(InsuranceFileKey)
                drgCoInsurance.DataSource = oresponse.CoInsurer
                drgCoInsurance.DataBind()
                Session("CoInsurance") = oresponse.CoInsurer
                BindCoInsuranceData()
                Total = 0.0
                If oresponse.CoInsurer IsNot Nothing Then
                    For iCount As Integer = 0 To oresponse.CoInsurer.Count - 1
                        Total += oresponse.CoInsurer(iCount).SharePerc
                    Next
                    hidTotalShare.Value = Total
                    lblTotalShare.Text = Total.ToString("f2") & "%" 'Total.ToString
                End If
            Else
                'Updation of the values
                BindCoInsuranceData()
            End If
            'If Request("__EVENTARGUMENT") = "Refresh" Then
            '    BindCoInsuranceData()
            'End If
        End Sub


        Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
            If Page.IsValid Then

                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                If Session(CNMode) <> Mode.View Then
                    If Val(hidTotalShare.Value) = "100.00" OrElse oQuote.BusinessTypeCode = "COIN FOLL" Then

                        Dim oCoInsurers As NexusProvider.CoInsurersCollections = CType(Session.Item("CoInsurance"), NexusProvider.CoInsurersCollections)
                        Dim InsuranceFileKey As Integer = CType(Session.Item(CNQuote), NexusProvider.Quote).InsuranceFileKey
                        Dim IsRecovered As Boolean = True
                        Dim IsSurcharged As Boolean = True
                        Dim TimeStamp As Byte() = CType(Session.Item(CNQuote), NexusProvider.Quote).TimeStamp

                        oWebService.UpdateCoInsuranceValues(InsuranceFileKey, True, True, TimeStamp, oCoInsurers, 1)
                        ' Session("TimeStamp") = TimeStamp
                        CType(Session(CNQuote), NexusProvider.Quote).TimeStamp = TimeStamp
                    End If
                End If

                If Session(CNQuoteMode) = QuoteMode.ReQuote Then
                    If Session(CNMTAType) IsNot Nothing Then
                        Session(CNQuoteMode) = QuoteMode.MTAQuote
                    Else
                        Session(CNQuoteMode) = QuoteMode.FullQuote
                    End If

                    Try
                        Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                        If oUserDetails IsNot Nothing Then
                            oWebService.UpdateQuotev2(oQuote, oQuote.BranchCode, oQuote.SubBranchCode, oUserDetails.Key)
                        Else
                            oWebService.UpdateQuotev2(oQuote, oQuote.BranchCode, oQuote.SubBranchCode)
                        End If

                    Finally
                        oWebService = Nothing
                    End Try
                    Response.Redirect("~/secure/RiskDetails.aspx", False)
                Else
                    Response.Redirect(Session("NextPage"), False)
                End If
            End If
        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        End Sub

        Protected Sub cusValidShare_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cusValidShare.ServerValidate
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            If (Val(hidTotalShare.Value) > "100.00") Then
                args.IsValid = False
                cusValidShare.ErrorMessage = GetLocalResourceObject("Err_InvalidOverShare")
            ElseIf Val(hidTotalShare.Value) < "100.00" Then
                args.IsValid = False
                cusValidShare.ErrorMessage = GetLocalResourceObject("Err_InvalidUnderShare")
            End If
            If (oQuote.BusinessTypeCode = "COIN FOLL") AndAlso (Val(hidTotalShare.Value) <= "100.00") AndAlso (Val(hidTotalShare.Value) > "0.00") Then
                args.IsValid = True
                If Val(hidTotalShare.Value) < "100.00" Then
                    args.IsValid = False
                    cusValidShare.ErrorMessage = GetLocalResourceObject("Err_InvalidUnderShare")
                ElseIf Val(hidTotalShare.Value) > "100.00" Then
                    args.IsValid = False
                    cusValidShare.ErrorMessage = GetLocalResourceObject("Err_InvalidOverShare")
                ElseIf Val(hidTotalShare.Value) = "100.00" Then
                    args.IsValid = True
                End If
            End If


        End Sub
        Protected Sub cusIsRetained_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cusIsRetained.ServerValidate
            Dim CoInsurers As NexusProvider.CoInsurersCollections = CType(Session.Item("CoInsurance"), NexusProvider.CoInsurersCollections)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim bIsRetained As Boolean
            bIsRetained = oWebService.CheckRetainedCoInsurerExists(CoInsurers)
            If bIsRetained = False Then
                args.IsValid = False
                cusIsRetained.ErrorMessage = GetLocalResourceObject("Err_NoRetained")
            End If
        End Sub
    End Class
End Namespace
