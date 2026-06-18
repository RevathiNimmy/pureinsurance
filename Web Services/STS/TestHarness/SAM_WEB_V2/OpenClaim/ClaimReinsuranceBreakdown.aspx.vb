Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Imports System.Data
Imports System.Xml.Serialization
Imports System.Xml
Partial Class ClaimReinsuranceBreakdown
    Inherits System.Web.UI.Page
    Dim StartDate As Date
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Not IsPostBack Then
        loadReinsuranceBand()
        loadReinsuranceArrangements()
        MultiView1.ActiveViewIndex = 0
        ' End If
    End Sub
    Private Sub loadReinsuranceBand()
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oGetClaimReinsuranceBandsRequest As New GetClaimReinsuranceBandsRequestType
        Dim oGetClaimReinsuranceBandsResponse As New GetClaimReinsuranceBandsResponseType

        oGetClaimReinsuranceBandsRequest.BranchCode = "HeadOff"
        oGetClaimReinsuranceBandsRequest.ClaimId = Session("ClaimKey")

         StartDate = Date.Now
        oGetClaimReinsuranceBandsResponse = oSAM.GetClaimReinsuranceBands(oGetClaimReinsuranceBandsRequest)
        WriteToLog(Session, "ClaimReinsuranceBreakdown.aspx", "SAMForInsuranceV2", "GetClaimReinsuranceBands", StartDate, Date.Now)

        With oGetClaimReinsuranceBandsResponse
            If Not (.Errors) Is Nothing Then
                Throw New SamResponseException(.Errors)
            Else
                ddlReinsuranceBand.DataSource = .ReinsuranceBands
                ddlReinsuranceBand.DataTextField = "Band"
                ddlReinsuranceBand.DataValueField = "BandId"
                ddlReinsuranceBand.DataBind()
            End If
        End With

    End Sub

    Private Sub loadReinsuranceArrangements()
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oGetClaimReinsuranceArrangementsRequest As New GetClaimReinsuranceArrangementsRequestType
        Dim oGetClaimReinsuranceArrangementsResponse As New GetClaimReinsuranceArrangementsResponseType
        Dim iMode As Integer = 0
        oGetClaimReinsuranceArrangementsRequest.BranchCode = "HeadOff"
        oGetClaimReinsuranceArrangementsRequest.ClaimId = Session("ClaimKey")
        oGetClaimReinsuranceArrangementsRequest.Mode = iMode
        oGetClaimReinsuranceArrangementsRequest.ModeSpecified = True

         StartDate = Date.Now
        oGetClaimReinsuranceArrangementsResponse = oSAM.GetClaimReinsuranceArrangements(oGetClaimReinsuranceArrangementsRequest)
         WriteToLog(Session, "ClaimReinsuranceBreakdown.aspx", "SAMForInsuranceV2", "GetClaimReinsuranceArrangements", StartDate, Date.Now)

        If Not (oGetClaimReinsuranceArrangementsResponse.Errors) Is Nothing Then
            Throw New SamResponseException(oGetClaimReinsuranceArrangementsResponse.Errors)
        Else
            gvReinsuranceArrangements.DataSource = oGetClaimReinsuranceArrangementsResponse.ReinsuranceArrangements
            gvReinsuranceArrangements.DataBind()

            If (oGetClaimReinsuranceArrangementsResponse.ReinsuranceArrangements.Length > 0) Then
                Dim iArrangementID As Integer
                iArrangementID = oGetClaimReinsuranceArrangementsResponse.ReinsuranceArrangements(0).ArrangementId
                loadReinsuranceArrangementsLines(iArrangementID, iMode)
            End If
        End If
    End Sub

    Private Sub loadReinsuranceArrangementsLines(ByVal iArrangementID As Integer, ByVal iMode As Integer)
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oGetClaimReinsuranceArrangementLinesRequest As New GetClaimReinsuranceArrangementLinesRequestType
        Dim oGetClaimReinsuranceArrangementLinesResponse As New GetClaimReinsuranceArrangementLinesResponseType

        oGetClaimReinsuranceArrangementLinesRequest.BranchCode = "HeadOff"
        oGetClaimReinsuranceArrangementLinesRequest.ArrangementId = iArrangementID
        oGetClaimReinsuranceArrangementLinesRequest.Mode = iMode
        oGetClaimReinsuranceArrangementLinesRequest.ModeSpecified = True

        oGetClaimReinsuranceArrangementLinesRequest.ClaimId = Session("ClaimKey")

         StartDate = Date.Now
        oGetClaimReinsuranceArrangementLinesResponse = oSAM.GetClaimReinsuranceArrangementLines(oGetClaimReinsuranceArrangementLinesRequest)
        WriteToLog(Session, "ClaimReinsuranceBreakdown.aspx", "SAMForInsuranceV2", "GetClaimReinsuranceArrangementLines", StartDate, Date.Now)

        With oGetClaimReinsuranceArrangementLinesResponse
            If Not (.Errors) Is Nothing Then
                Throw New SamResponseException(.Errors)
            Else
                gvArrangementLines.DataSource = oGetClaimReinsuranceArrangementLinesResponse.ReinsuranceArrangementLines
                gvArrangementLines.DataBind()
            End If
        End With
    End Sub

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Me.Dispose()
        Response.Write("<script>window.close();</script>")
    End Sub
End Class
