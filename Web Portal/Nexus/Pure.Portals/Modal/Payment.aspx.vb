Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports CMS.library
Imports Nexus.Utils
Imports System.Web.HttpContext
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class Modal_Payment
        Inherits Frontend.clsCMSPage

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not IsPostBack Then
                Dim dPaymentAmount As Decimal = Request.QueryString("PAY")
                If dPaymentAmount < 0 Then
                    txtEnterPaymentAmount.Text = dPaymentAmount * -1
                Else
                    txtEnterPaymentAmount.Text = dPaymentAmount
                End If
            End If

        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
            If (Page.IsValid) Then

                Dim dAmount As Decimal = Request.QueryString("PAY")
                If (Not IsNumeric(txtEnterPaymentAmount.Text) OrElse String.IsNullOrEmpty(txtEnterPaymentAmount.Text.Trim)) Then
                    rvPartPayRange.Visible = True
                    rvPartPayRange.Text = GetLocalResourceObject("PartPayErr").ToString()
                    Exit Sub
                End If
                Dim txtPartPayVal As String = txtEnterPaymentAmount.Text
                rvPartPayRange.Text = ""
                Dim dPartPayAmount As Decimal = IIf(IsNumeric(txtPartPayVal), Convert.ToDecimal(txtPartPayVal), 0.00)

                If Math.Abs(dPartPayAmount) > Math.Abs(dAmount) Then
                    rvPartPayRange.Visible = True
                    rvPartPayRange.Text = GetLocalResourceObject("PartPayAmountValid_Err").ToString()
                    Exit Sub
                Else
                    Mark()
                End If
            End If
        End Sub
       
        Protected Sub Mark()
            Try
                Dim oMarkunmark As NexusProvider.MarkUnmarkTransaction
                Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim sCurrencyCode As String = CType(Request("CC"), String)
                Dim iTransactionkey As Integer = CType(Request("TK"), Integer)
                Dim bMark As Boolean = Request.QueryString("Mark")
                Dim dPaymentAmount As Decimal = Request.QueryString("PAY")
                If dPaymentAmount < 0 Then
                    dPaymentAmount = -CDec(txtEnterPaymentAmount.Text.Trim)
                Else
                    dPaymentAmount = CDec(txtEnterPaymentAmount.Text.Trim)
                End If

                If bMark = True Then
                    oMarkunmark = New NexusProvider.MarkUnmarkTransaction
                    oMarkunmark.CurrencyCode = sCurrencyCode.Trim()
                    oMarkunmark.TransactionKey = iTransactionkey
                    oMarkunmark.PaymentAmount = "0.00"
                    oMarkunmark.MarkStatus = NexusProvider.MarkStatusType.UnMark
                    oWebservice.MarkUnmarkTransaction(oMarkunmark)
                Else
                    oMarkunmark = New NexusProvider.MarkUnmarkTransaction
                End If
                oMarkunmark.CurrencyCode = sCurrencyCode.Trim()
                oMarkunmark.TransactionKey = iTransactionkey
                If Not txtEnterPaymentAmount.Text = "0.00" AndAlso Not txtEnterPaymentAmount.Text.Trim.Length = 0 Then
                    oMarkunmark.PaymentAmount = dPaymentAmount
                    oMarkunmark.MarkStatus = NexusProvider.MarkStatusType.Mark
                    oWebservice.MarkUnmarkTransaction(oMarkunmark)
                End If

            Finally
                txtEnterPaymentAmount.Text = String.Empty
            End Try

            'add javascript to call script in parent page which will close modal dialog
            Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "RefreshGrid") & ";"
            Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)

        End Sub


        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            ' code to close the screen on thickbox implementation 
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
        End Sub
    End Class

End Namespace
