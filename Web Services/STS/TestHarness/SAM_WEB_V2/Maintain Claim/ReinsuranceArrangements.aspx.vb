Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class Maintain_Claim_ReinsuranceArrangements
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            loadReinsuranceBand()
            loadReinsuranceArrangements()
            loadReinsuranceArrangementsLines()
        End If
    End Sub
    Private Sub loadReinsuranceBand()
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        'Dim oGetClaimReinsuranceBandsRequest As New GetClaimReinsuranceBandsRequestType
        'Dim oGetClaimReinsuranceBandsResponse As New GetClaimReinsuranceBandsResponseType

        ''oGetClaimReinsuranceBandsRequest.BranchCode = "HeadOff"
        ''oGetClaimReinsuranceBandsRequest.ClaimId = 741

        ''oGetClaimReinsuranceBandsResponse = oSAM.GetClaimReinsuranceBands(oGetClaimReinsuranceBandsRequest)

        ''With oGetClaimReinsuranceBandsResponse
        ''    If Not (.Errors) Is Nothing Then
        ''        Throw New SamResponseException(.Errors)
        ''    Else
        ''        ddlReinsuranceBand.DataSource = .ReinsuranceBands
        ''        ddlReinsuranceBand.DataTextField = "Band"
        ''        ddlReinsuranceBand.DataValueField = "BandId"
        ''        ddlReinsuranceBand.DataBind()
        ''    End If
        ''End With
    End Sub

    Private Sub loadReinsuranceArrangements()
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        'Dim oGetClaimReinsuranceArrangementsRequest As New GetClaimReinsuranceArrangementsRequestType
        'Dim oGetClaimReinsuranceArrangementsResponse As New GetClaimReinsuranceArrangementsResponseType

        'oGetClaimReinsuranceArrangementsRequest.BranchCode = "HeadOff"
        'oGetClaimReinsuranceArrangementsRequest.ClaimId = 741 '721
        'oGetClaimReinsuranceArrangementsRequest.ModeSpecified = True
        'oGetClaimReinsuranceArrangementsRequest.Mode = 1

        'oGetClaimReinsuranceArrangementsResponse = oSAM.GetClaimReinsuranceArrangements(oGetClaimReinsuranceArrangementsRequest)
        'With oGetClaimReinsuranceArrangementsResponse
        '    If Not (.Errors) Is Nothing Then
        '        Throw New SamResponseException(.Errors)
        '    Else
        '        gvReinsuranceArrangements.DataSource = oGetClaimReinsuranceArrangementsResponse.ReinsuranceArrangements
        '        gvReinsuranceArrangements.DataBind()
        '    End If
        'End With
    End Sub

    Private Sub loadReinsuranceArrangementsLines()
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        'Dim oGetClaimReinsuranceArrangementLinesRequest As New GetClaimReinsuranceArrangementLinesRequestType
        'Dim oGetClaimReinsuranceArrangementLinesResponse As New GetClaimReinsuranceArrangementLinesResponseType

        'oGetClaimReinsuranceArrangementLinesRequest.BranchCode = "HeadOff"
        'oGetClaimReinsuranceArrangementLinesRequest.ClaimId = 741 '721
        'oGetClaimReinsuranceArrangementLinesRequest.ArrangementId = 2689 '2667
        'oGetClaimReinsuranceArrangementLinesRequest.ModeSpecified = True
        'oGetClaimReinsuranceArrangementLinesRequest.Mode = 1

        'oGetClaimReinsuranceArrangementLinesResponse = oSAM.GetClaimReinsuranceArrangementLines(oGetClaimReinsuranceArrangementLinesRequest)
        'With oGetClaimReinsuranceArrangementLinesResponse
        '    If Not (.Errors) Is Nothing Then
        '        Throw New SamResponseException(.Errors)
        '    Else
        '        gvArrangementLines.DataSource = oGetClaimReinsuranceArrangementLinesResponse.ReinsuranceArrangementLines
        '        gvArrangementLines.DataBind()
        '    End If
        'End With
    End Sub
End Class
