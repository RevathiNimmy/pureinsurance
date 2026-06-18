Imports System.Web.Configuration.WebConfigurationManager
Imports NexusProvider.LookupList
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports CMS.Library
Imports Nexus.Library
Imports NexusProvider.SAMForInsurance
Imports System.Collections.Generic
Imports Nexus
Imports System.Collections
Imports System.Web.UI.WebControls
Namespace Nexus

    Partial Class Modal_SelectTreaty : Inherits Frontend.clsCMSPage
        Dim sCode As String
        Dim sDescription As String
        Dim skey As Integer
        Private ReadOnly Property IsRI2007 As Boolean
            Get
                Return Request.QueryString("RI2007") = "ON"
            End Get
        End Property

        Private ReadOnly Property TreatyType As String
            Get
                Return If(Request.QueryString("Type"), "T")
            End Get
        End Property

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not IsPostBack Then

                Dim oWebService As NexusProvider.ProviderBase
                Dim oRIPropTreatiesCol As New NexusProvider.RIPropTreatiesCollection
                Dim nRIArrangementKey As Integer = 0
                If Request.QueryString("RIArrangementKey") IsNot Nothing Then
                    Integer.TryParse(Request.QueryString("RIArrangementKey"), nRIArrangementKey)
                End If

                oWebService = New NexusProvider.ProviderManager().Provider
                oWebService.GetRIPropTreaties(oRIPropTreatiesCol, nRIArrangementKey, TreatyType)
                oWebService = Nothing

                If oRIPropTreatiesCol IsNot Nothing AndAlso oRIPropTreatiesCol.Count > 0 Then
                    ddlRIPropTreaties.Items.Clear()
                    ddlRIPropTreaties.Items.Add(New ListItem("(Please select)", ""))
                    For Each oReinsurarerBand As NexusProvider.RIPropTreaties In oRIPropTreatiesCol
                        Dim compositeValue As String = oReinsurarerBand.TreatyId.ToString() & "|" & oReinsurarerBand.TreatyCode & "|" & oReinsurarerBand.ReinsuranceCode
                        ddlRIPropTreaties.Items.Add(New ListItem(oReinsurarerBand.TreatyDescription, compositeValue))
                    Next
                End If

            End If
            If ddlRIPropTreaties.Items IsNot Nothing And ddlRIPropTreaties.Items.Count > 0 Then

                sCode = ddlRIPropTreaties.Items(ddlRIPropTreaties.SelectedIndex).Value
                sDescription = ddlRIPropTreaties.Items(ddlRIPropTreaties.SelectedIndex).Text
                skey = "0"

            End If

            If IsRI2007 Then
                ' For RI2007, allow server-side click event
                btnCancel.Attributes.Add("onclick", "self.parent.tb_remove();return false;")
            Else
                Dim sScript As String = "self.parent.addTreaty('" + skey.ToString() + "','" + sCode + "','" + sDescription + "');"
                sScript = sScript & "self.parent.tb_remove();return false;"
                btnOk.Attributes.Add("onclick", sScript)
                btnCancel.Attributes.Add("onclick", "self.parent.tb_remove();return false;")
            End If
        End Sub

        Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
            ' PopulateTreaty()
        End Sub

        Protected Sub btnCancel_Click(sender As Object, e As EventArgs)

        End Sub

        Protected Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
            If Not Page.IsValid Then Return
            If IsRI2007 Then
                If ddlRIPropTreaties.SelectedItem IsNot Nothing Then
                    Dim compositeValue As String = ddlRIPropTreaties.SelectedValue
                    Dim parts() As String = compositeValue.Split("|"c)
                    Session("SelectedTreatyId") = Convert.ToInt32(parts(0))
                    Session("SelectedTreatyCode") = parts(1)
                    Session("SelectedReinsuranceCode") = If(parts.Length > 2, parts(2), String.Empty)
                    Session("SelectedTreatyName") = ddlRIPropTreaties.SelectedItem.Text
                    Session("SelectedTreatyType") = TreatyType
                    ClientScript.RegisterStartupScript(Me.GetType(), "close", "parent.location.reload(); parent.tb_remove();", True)
                End If
            End If
        End Sub

        Protected Sub PopulateTreaty()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

        End Sub

        Private Sub ddlRIPropTreaties_SelectedIndexChange(sender As Object, e As EventArgs) Handles ddlRIPropTreaties.SelectedIndexChanged
            sCode = ddlRIPropTreaties.SelectedItem.Value
            sDescription = ddlRIPropTreaties.SelectedItem.Text
            skey = "0"

            If Not IsRI2007 Then
                Dim sScript As String = "self.parent.addTreaty('" + skey.ToString() + "','" + sCode + "','" + sDescription + "');"
                sScript = sScript & "self.parent.tb_remove();return false;"
                btnOk.Attributes.Add("onclick", sScript)
                btnCancel.Attributes.Add("onclick", "self.parent.tb_remove();return false;")
            End If
        End Sub
    End Class

End Namespace