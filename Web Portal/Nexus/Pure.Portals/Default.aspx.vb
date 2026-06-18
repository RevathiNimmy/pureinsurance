Imports CMS.Library
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants.Session
Imports Nexus.Library.Config

Namespace Nexus

	''' <summary>
	''' CMS homepage
	''' </summary>
	Partial Class _Default : Inherits Frontend.clsCMSPage

		''' <summary>
		''' Set the site theme and ensure the homepage template is used irrelevant of any setting the CMS.
		''' Retrieve the content for the CMS for the homepage
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Overrides Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

			Frontend.Functions.SetTheme(Page, AppSettings("HomePageTemplate"))

			If LCase(AppSettings("CMS")) = "full" Then
				Dim iSiteMapID As Integer = SiteMap.Functions.ValidateParentChild(0, SiteMap.Functions.GetHomeLabel)
				SelectedContent = Frontend.Functions.GetFrontEndPage(iSiteMapID,
															Request("archive_id"), Request("preview"), Request("guid"))
			Else
				SelectedContent = New SiteMap.SiteMapContent
			End If

		End Sub

		''' <summary>
		''' Display the page content from the CMS for the homepage
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

			If SelectedContent.IsValid Then

				'lblTitle.Text = SelectedContent.Element("Title")
				ltContent.Text = SelectedContent.Element("Text")

			End If

			Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
			Dim bEnableBranchSelectionAtLogin As Boolean = False

			Dim providerSection As IdentityProvider = CType(ConfigurationManager.GetSection("IdentityProvider"), IdentityProvider)
			Dim identityProvider = providerSection.DefaultIdentity.ToUpper
			If Equals(identityProvider, "SSO") OrElse Equals(identityProvider, "KEYCLOAK") Then
				HttpContext.Current.Response.Redirect("~/Login.aspx")
			ElseIf CType(HttpContext.Current.Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches.Count > 1 Then
				Dim oOptionType As New NexusProvider.OptionTypeSetting
				oOptionType = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, 37)
				If oOptionType.OptionValue = "1" Then
					bEnableBranchSelectionAtLogin = True
				End If
			End If

			If CType(HttpContext.Current.Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches.Count > 1 And bEnableBranchSelectionAtLogin = True Then
				'if an agent has more than 1 branches and option set to force a choice to 
				'be made on login then go to select branch
				HttpContext.Current.Response.Redirect("~/SelectBranch.aspx")
			Else
				'set first branch as default
				HttpContext.Current.Session(CNBranchCode) = CType(HttpContext.Current.Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches(0).Code
				'redirect to start page
				Dim sAgentStartPage As String = CType(GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).AgentStartPage
				HttpContext.Current.Response.Redirect(sAgentStartPage)
			End If
		End Sub

	End Class

End Namespace
