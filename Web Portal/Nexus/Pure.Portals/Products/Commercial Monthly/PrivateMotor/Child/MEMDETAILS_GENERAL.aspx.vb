Imports CMS.Library
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports System.Web.Configuration
Imports Nexus.Utils
Imports System.Web.HttpContext
Imports Nexus.Constants.Session
Imports Nexus.Constants.Constant
Namespace Nexus

		Partial Class MEMDETAILS_GENERAL : Inherits BaseRisk
		
		Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
			SetPageProgress(3)
		End Sub

		Public Overrides Sub PostDataSetWrite()
		End Sub

		Public Overrides Sub PreDataSetWrite()
		End Sub
		
	End Class
	
End Namespace
		