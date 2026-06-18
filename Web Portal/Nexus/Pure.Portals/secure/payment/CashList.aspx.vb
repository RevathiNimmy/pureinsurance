
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants.Session
Namespace Nexus
    Partial Class secure_payment_CashList : Inherits CMS.Library.Frontend.clsCMSPage

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not IsPostBack Then
                If Request.UrlReferrer IsNot Nothing Then
                    Dim sParentPage As String = Request.UrlReferrer.AbsoluteUri
                    If Not sParentPage.Contains("FinancePlanDetails.aspx") Then
                        Session(CNDebitTransDetailkey) = Nothing
                    End If
                End If
            End If

        End Sub
        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            Dim sMode As String = Request.QueryString("Mode")

            'Note : The Session are used into cashlist.aspx and internal Controls instead of clearing all session , session should be clear while required code paralleled from 4.0 , Racti
            '(23 Sep 2015)

            'If Not IsPostBack AndAlso Not String.IsNullOrEmpty(sMode) Then
            '    'Cleaning of the session values
            '    ClearQuote()
            '    ClearClaims()
            '    ClearHeader()
            'End If

            'WPR48 
            If sMode = "IP" Then '
                If Session(CNTransInMultiCurr) = "Yes" Then
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", _
           "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){alert('The transactions that you have selected for payment have different transaction currencies.');});</script>")
                End If
            End If
        End Sub
      
    End Class
End Namespace