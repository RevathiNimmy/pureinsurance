Imports CMS.Library
'Imports System.Web.Configuration.WebConfigurationManager

Namespace Nexus

    ''' <summary>
    ''' CMS sub page
    ''' </summary>
    ''' <remarks></remarks>
    Partial Class LoggedIn : Inherits Frontend.clsCMSPage

        ''' <summary>
        ''' Display the content from the CMS
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' 
        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If SelectedContent.IsValid Then

                LblTitle.Text = Server.HtmlDecode(SelectedContent.Element("Title"))
                ltContent.Text = Server.HtmlDecode(SelectedContent.Element("Text"))

            End If
        End Sub
    End Class

End Namespace
