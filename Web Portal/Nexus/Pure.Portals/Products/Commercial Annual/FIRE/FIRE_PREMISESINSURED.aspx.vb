Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Constants.Session
Imports Nexus.Constants.Constant
Namespace Nexus

		Partial Class FIRE_PREMISESINSURED : Inherits BaseRisk
		
		Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            SetPageProgress(3)
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)

            If Not IsPostBack And (oQuote.InsuranceFileTypeCode.Trim <> "MTAQUOTE") Then

                HideTab("fire_firefiref")
                HideTab("fire_officecontents")

            End If
		End Sub

		Public Overrides Sub PostDataSetWrite()
		End Sub

		Public Overrides Sub PreDataSetWrite()
		End Sub
		
	End Class
	
End Namespace
		