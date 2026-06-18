Imports Nexus.Utils
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Web.UI.TemplateControl

Namespace Frontend

    Public Class clsCMSPage
        Inherits System.Web.UI.Page

        Protected SelectedContent As SiteMap.SiteMapContent

        Protected Overridable Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

            Functions.SetTheme(Page)

            If LCase(appsettings("CMS")) = "full" Then
                SelectedContent = Functions.GetFrontEndPage(CInt(Request("sitemap_id")), CInt(Request("archive_id")), Request("preview"), Request("guid"))
            Else
                SelectedContent = New SiteMap.SiteMapContent
            End If

        End Sub

        Protected Overridable Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'VALIDATE RESOURCE FILE EXISTENCE
            Dim sTemp As String = System.Web.HttpContext.Current.Request.ServerVariables("SCRIPT_NAME").ToString()
            Dim iPos As Integer = sTemp.LastIndexOf("/")
            Dim sResourceName As String = sTemp.Substring(iPos + 1)

            If System.IO.File.Exists(Server.MapPath("App_LocalResources/" + sResourceName + ".resx")) Then
                'VALIDATE RESOURCE KEY NAME
                If GetLocalResourceObject("PageTitle") IsNot Nothing Then
                    Me.Title = GetLocalResourceObject("PageTitle")
                End If
            End If


            If SelectedContent.IsValid Then

                Dim MetaTags As New MetaTags
                MetaTags.Keywords = CStr(SelectedContent.Element("Keywords"))
                MetaTags.Description = CStr(SelectedContent.Element("Description"))
                MetaTags.LastModified = SelectedContent.TimeStamp

                CType(Master, CMSMasterPage).MetaTags = MetaTags.HTML
                CType(Master, CMSMasterPage).Title = SelectedContent.Element("Title")

            End If

        End Sub

        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)

            For Each control As System.Web.UI.Control In Page.Header.Controls

                Dim link As System.Web.UI.HtmlControls.HtmlLink
                Dim script As System.Web.UI.HtmlControls.HtmlControl

                Select Case control.GetType.Name
                    Case "HtmlLink"
                        link = CType(control, System.Web.UI.HtmlControls.HtmlLink)
                        If (Not link Is Nothing) And link.Href.StartsWith("~/") Then

                            If (Request.ApplicationPath = "/") Then
                                link.Href = link.Href.Substring(1)
                            Else
                                link.Href = Request.ApplicationPath + "/" + link.Href.Substring("~/".Length)
                            End If

                        End If
                    Case "HtmlControl"
                        script = CType(control, System.Web.UI.HtmlControls.HtmlControl)
                        If (Not script Is Nothing) And script.Attributes("src").StartsWith("~/") Then

                            If (Request.ApplicationPath = "/") Then
                                script.Attributes("src") = script.Attributes("src").Substring(1)
                            Else
                                script.Attributes("src") = Request.ApplicationPath + "/" + script.Attributes("src").Substring("~/".Length)
                            End If

                        End If
                End Select
            Next

            CheckPageControls(Page.Form.Controls)

            MyBase.Render(writer)
        End Sub

        Private Sub CheckPageControls(ByVal PageControls As System.Web.UI.ControlCollection)
            For Each control As System.Web.UI.Control In PageControls

                If control.HasControls() Then
                    CheckPageControls(control.Controls)
                End If
                Dim image As System.Web.UI.HtmlControls.HtmlImage

                Select control.GetType.Name
                    Case "HtmlImage"
                        image = CType(control, System.Web.UI.HtmlControls.HtmlImage)
                        If (Not image Is Nothing) And image.Src.StartsWith("~/") Then

                            If (Request.ApplicationPath = "/") Then
                                image.Src = image.Src.Substring(1)
                            Else
                                image.Src = Request.ApplicationPath + "/" + image.Src.Substring("~/".Length)
                            End If

                        End If
                End Select
            Next
        End Sub

    End Class

End Namespace
