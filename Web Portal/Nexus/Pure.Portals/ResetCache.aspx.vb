Imports System.Web

Namespace Web.BrandRisks

    Partial Class ResetCache
        Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub


        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub

#End Region

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            'Secure this page using a guid
            If Request.QueryString("guid") = Configuration.WebConfigurationManager.AppSettings("ResetCacheGUID") Then
                If IsNumeric(Request.QueryString("sitemap_id")) Then
                    'Reset cache for requested page

                    Cache.Remove("Content_" & Request.QueryString("sitemap_id"))

                    If IsNumeric(Request.QueryString("parent_id")) Then
                        'Reset Navigation for page, referenced by the parent_id
                        Cache.Remove("Nav_" & Request.QueryString("parent_id") & True)
                        Cache.Remove("Nav_" & Request.QueryString("parent_id") & False)
                    End If
                Else
                    'Reset cache for requested key
                    If Request.QueryString("key") <> "" Then
                        Cache.Remove(Request.QueryString("key"))
                    End If
                End If
            Else
                'Go Away
                Response.Redirect("default.aspx", False)
            End If
        End Sub

    End Class

End Namespace
