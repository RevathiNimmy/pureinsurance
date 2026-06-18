Imports CMS.Library
Imports System.Web.Configuration.WebConfigurationManager
Namespace Nexus
    Partial Class secure_payment_CashListItem : Inherits CMS.Library.Frontend.clsCMSPage


        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

            'TO DO - need to uncomment the code
            'Request.QueryString("Mode") = "IP" Or
            'If Request.QueryString("Mode") = "CR" Then
            '    CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
            'End If
            Session("ModeValue") = Request.QueryString("Mode")
            Session("Type") = Request.QueryString("Type")
            If Request.QueryString("Mode") = "AP" Then
                Session("ModeType") = "Payment"
            End If
        End Sub
    End Class
End Namespace