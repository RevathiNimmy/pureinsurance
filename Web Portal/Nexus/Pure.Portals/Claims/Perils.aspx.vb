Imports NexusProvider.SAMForInsurance
Imports Nexus.Utils
Imports Nexus.Library
Imports System.Configuration.ConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports System.Xml.XmlReader
Imports System.Xml.XPath
Imports System.Xml
Imports CMS.Library
Namespace Nexus

    Partial Class Framework_Perils
        Inherits BaseClaim

        ''' <summary>
        ''' On click of the Tab on perils page
        ''' </summary>
        ''' <param name="Path"></param>
        ''' <remarks></remarks>
        Protected Sub TabIndex_TabClicked(ByVal Path As String) Handles TabIndex.TabClicked
            Response.Redirect(Path, False)
        End Sub

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            ucProgressBar.OverviewStyle = "complete"
            ucProgressBar.PerilsStyle = "in-progress"
            ucProgressBar.ReinsuranceStyle = "incomplete"
            ucProgressBar.SummaryStyle = "incomplete"
            ucProgressBar.CompleteStyle = "incomplete"
        End Sub
    End Class

End Namespace
