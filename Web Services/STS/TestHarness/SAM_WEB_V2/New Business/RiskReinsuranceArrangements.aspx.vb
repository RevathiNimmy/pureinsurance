Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Imports System.Data
Imports System.Xml.Serialization
Imports System.Xml
Partial Class RiskReinsuranceArrangements
    Inherits System.Web.UI.Page
    Dim StartDate As Date
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            loadReinsuranceBand()
            loadReinsuranceArrangements()
            MultiView1.ActiveViewIndex = 0
        End If
    End Sub
    Private Sub loadReinsuranceBand()
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oGetRiskReinsuranceBandsRequest As New GetRiskReinsuranceBandsRequestType
        Dim oGetRiskReinsuranceBandsResponse As New GetRiskReinsuranceBandsResponseType

        oGetRiskReinsuranceBandsRequest.BranchCode = "HeadOff"
        oGetRiskReinsuranceBandsRequest.RiskKey = Session("RiskKey")
        StartDate = Date.Now
        oGetRiskReinsuranceBandsResponse = oSAM.GetRiskReinsuranceBands(oGetRiskReinsuranceBandsRequest)
        WriteToLog(Session, "RiskReinsuranceArrangements.aspx", "SAMForInsuranceV2", "GetRiskReinsuranceBands",StartDate, Date.Now)
        With oGetRiskReinsuranceBandsResponse
            If Not (.Errors) Is Nothing Then
                Response.Write(GetMessageFromSamError(.Errors))
            Else
                ddlReinsuranceBand.DataSource = .ReinsuranceBands
                ddlReinsuranceBand.DataTextField = "Band"
                ddlReinsuranceBand.DataValueField = "BandKey"
                ddlReinsuranceBand.DataBind()
            End If
        End With

    End Sub

    Private Sub loadReinsuranceArrangements()
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oGetRiskReinsuranceArrangementsRequest As New GetRiskReinsuranceArrangementsRequestType
        Dim oGetRiskReinsuranceArrangementsResponse As New GetRiskReinsuranceArrangementsResponseType

        oGetRiskReinsuranceArrangementsRequest.BranchCode = "HeadOff"
        oGetRiskReinsuranceArrangementsRequest.RiskKey = Session("RiskKey")
        StartDate = Date.Now
        oGetRiskReinsuranceArrangementsResponse = oSAM.GetRiskReinsuranceArrangements(oGetRiskReinsuranceArrangementsRequest)
        WriteToLog(Session, "RiskReinsuranceArrangements.aspx", "SAMForInsuranceV2", "GetRiskReinsuranceArrangements",StartDate, Date.Now)
        Dim dsArrangements, dsOriginalRI As New System.Data.DataSet
        Dim dtArrangements As New DataTable
        dtArrangements.Columns.Add("ArrangementId", GetType(String))
        dtArrangements.Columns.Add("BandId", GetType(String))
        dtArrangements.Columns.Add("ModelId", GetType(String))
        dtArrangements.Columns.Add("SumInsured", GetType(String))
        dtArrangements.Columns.Add("Premium", GetType(String))
        dtArrangements.Columns.Add("IsOriginal", GetType(String))
        dtArrangements.Columns.Add("IsModified", GetType(String))
        dtArrangements.Columns.Add("FACPremiumType", GetType(String))
        dsArrangements.Tables.Add(dtArrangements)
        dsOriginalRI = dsArrangements.Copy()
        If Not (oGetRiskReinsuranceArrangementsResponse.Errors) Is Nothing Then
            Throw New SamResponseException(oGetRiskReinsuranceArrangementsResponse.Errors)
        Else
            Dim nCounter As Integer
            Dim drArrangements As DataRow
            nCounter = oGetRiskReinsuranceArrangementsResponse.Arrangements.Length
            Dim oTempArrangements(nCounter) As GetRiskReinsuranceArrangementsResponseType
            For nCounter = 0 To oGetRiskReinsuranceArrangementsResponse.Arrangements.Length - 1
                If (Not oGetRiskReinsuranceArrangementsResponse.Arrangements(nCounter).IsOriginal) Then
                    drArrangements = dsArrangements.Tables(0).NewRow()
                    With oGetRiskReinsuranceArrangementsResponse.Arrangements(nCounter)
                        drArrangements("ArrangementId") = .ArrangementId
                        drArrangements("BandId") = .BandId
                        drArrangements("ModelId") = .ModelId
                        drArrangements("SumInsured") = .SumInsured
                        drArrangements("Premium") = .Premium
                        drArrangements("IsOriginal") = .IsOriginal
                        drArrangements("IsModified") = .IsModified
                        drArrangements("FACPremiumType") = .FACPremiumType
                        dsArrangements.Tables(0).Rows.Add(drArrangements)
                    End With
                Else
                    drArrangements = dsOriginalRI.Tables(0).NewRow()
                    With oGetRiskReinsuranceArrangementsResponse.Arrangements(nCounter)
                        drArrangements("ArrangementId") = .ArrangementId
                        drArrangements("BandId") = .BandId
                        drArrangements("ModelId") = .ModelId
                        drArrangements("SumInsured") = .SumInsured
                        drArrangements("Premium") = .Premium
                        drArrangements("IsOriginal") = .IsOriginal
                        drArrangements("IsModified") = .IsModified
                        drArrangements("FACPremiumType") = .FACPremiumType
                        dsOriginalRI.Tables(0).Rows.Add(drArrangements)
                    End With
                End If
            Next
            gvReinsuranceArrangements.DataSource = dsArrangements
            gvReinsuranceArrangements.DataBind()
            gvOriginalRI.DataSource = dsOriginalRI
            gvOriginalRI.DataBind()
            loadReinsuranceArrangementsLines(Convert.ToInt32(dsArrangements.Tables(0).Rows(0)("ArrangementId")))
        End If
    End Sub

    Private Sub loadReinsuranceArrangementsLines(ByVal iArrangementID As Integer)
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oGetRiskReinsuranceArrangementLinesRequest As New GetRiskReinsuranceArrangementLinesRequestType
        Dim oGetRiskReinsuranceArrangementLinesResponse As New GetRiskReinsuranceArrangementLinesResponseType

        oGetRiskReinsuranceArrangementLinesRequest.BranchCode = "HeadOff"
        oGetRiskReinsuranceArrangementLinesRequest.ArrangementId = iArrangementID
        StartDate = Date.Now
        oGetRiskReinsuranceArrangementLinesResponse = oSAM.GetRiskReinsuranceArrangementLines(oGetRiskReinsuranceArrangementLinesRequest)
        WriteToLog(Session, "RiskReinsuranceArrangements.aspx", "SAMForInsuranceV2", "GetRiskReinsuranceArrangementLines",StartDate, Date.Now)
        With oGetRiskReinsuranceArrangementLinesResponse
            If Not (.Errors) Is Nothing Then
                Response.Write(GetMessageFromSamError(.Errors))
            Else
                gvArrangementLines.DataSource = oGetRiskReinsuranceArrangementLinesResponse.ArrangementLines
                gvArrangementLines.DataBind()
            End If
        End With
    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Dim iSelectedIndex As Integer
        iSelectedIndex = Menu1.Items.IndexOf(Menu1.SelectedItem)
        MultiView1.ActiveViewIndex = iSelectedIndex
        If (iSelectedIndex = 0) Then

        ElseIf (iSelectedIndex = 1) Then

        End If
    End Sub

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Response.Redirect("GetListRisks_Risk.aspx")
    End Sub
End Class
