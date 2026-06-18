Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Utils
Imports System.Xml
Imports System.Xml.XPath
Imports System.Globalization.CultureInfo
Namespace Nexus

    Partial Class MOTOR_VEHICLEDETAILSQQ : Inherits BaseRisk

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            SetPageProgress(3)

            'If Not IsPostBack Then
            '    VEHDET__OF_LIABILITY_COVER.Checked = False
            'End If
        End Sub

        Public Overrides Sub PostDataSetWrite()
        End Sub

        Public Overrides Sub PreDataSetWrite()

        End Sub

        Protected Sub VEHDET__CLAS_USE_SelectedIndexChange(sender As Object, e As EventArgs) Handles VEHDET__CLAS_USE.SelectedIndexChange

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "SetRate();", True)
        End Sub
    End Class
	
End Namespace
		