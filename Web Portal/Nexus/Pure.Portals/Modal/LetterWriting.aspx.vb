Imports System.Configuration.ConfigurationManager
Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session


Namespace Nexus
    Partial Class Modal_LetterWriting
        Inherits System.Web.UI.Page


        Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
            ClientScript.GetPostBackEventReference(Me, String.Empty)

            If HttpContext.Current.Session.IsCookieless Then
                btnFindDocumentTemplate.OnClientClick = "tb_show(null ,'../Modal/FindDocumentTemplates.aspx?modal=true&KeepThis=true&FromPage=BG&TB_iframe=true&height=500&width=750' , null);return false;"
            Else
                btnFindDocumentTemplate.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "Modal/FindDocumentTemplates.aspx?modal=true&KeepThis=true&FromPage=BG&TB_iframe=true&height=500&width=750' , null);return false;"
            End If

            If Request("__EVENTTARGET") = "TemplateCode" Then
                Session(CNTemplateCode) = Request("__EVENTARGUMENT")
            End If
        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

    End Class
End Namespace

