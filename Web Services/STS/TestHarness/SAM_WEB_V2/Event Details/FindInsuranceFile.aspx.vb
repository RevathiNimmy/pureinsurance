Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class Lookup_Screens_FindInsuranceFile
    Inherits System.Web.UI.Page
    Dim oFindInsurenceFileForClaimResponse As New FindInsuranceFileForClaimsResponseType


    Protected Sub mnuInsuranceFile_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles mnuInsuranceFile.MenuItemClick
        mvFindInsuranceFile.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub

    Protected Sub btnFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFind.Click
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oFindInsurenceFileRequest As New FindPolicyRequestType
        Dim oFindInsurenceFileResponse As New FindPolicyResponseType

        Try


            With oFindInsurenceFileRequest
                .BranchCode = "HeadOff"
                .InsuranceRef = txtinsuranceref.Text.Trim()
                .RiskIndex = txtriskindex.Text.Trim()
                .ClientShortName = txtshortname.Text.Trim()
                If (rdtype.SelectedIndex = 0) Then
                    ' .QuoteType = ""
                    .QuoteTypeSpecified = False
                ElseIf (rdtype.SelectedIndex = 1) Then
                    .QuoteType = InsuranceFileType.QUOTE
                    .QuoteTypeSpecified = True
                ElseIf (rdtype.SelectedIndex = 2) Then
                    .QuoteType = InsuranceFileType.MTAQUOTE
                    .QuoteTypeSpecified = True
                ElseIf (rdtype.SelectedIndex = 3) Then
                    .QuoteType = InsuranceFileType.POLICY
                    .QuoteTypeSpecified = True
                Else

                    .QuoteType = InsuranceFileType.RENEWAL
                    .QuoteTypeSpecified = True
                End If
                .ShowLapsedOnly = False
                .ShowLapsedOnlySpecified = False


            End With
            oFindInsurenceFileResponse = oSAM.FindPolicy(oFindInsurenceFileRequest)
            Session("InsuranceFileResponse") = oFindInsurenceFileResponse
            With oFindInsurenceFileForClaimResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                Else
                    gvSearchResult.DataSource = oFindInsurenceFileResponse.InsuranceFileDetails
                    gvSearchResult.DataBind()



                End If


            End With
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub gvSearchResult_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSearchResult.SelectedIndexChanged
        Dim oFindPolicyResponse As New FindPolicyResponseType
        oFindPolicyResponse = DirectCast(Session("InsuranceFileResponse"), FindPolicyResponseType)
        txtinsuranceref.Text = oFindPolicyResponse.InsuranceFileDetails(gvSearchResult.SelectedIndex).InsuranceRef
        'Session("InsuranceFileKey") = oFindPolicyResponse.InsuranceFileDetails(gvSearchResult.SelectedIndex).InsuranceFileKey
        hfinsurancekey.Value = oFindPolicyResponse.InsuranceFileDetails(gvSearchResult.SelectedIndex).InsuranceFileKey

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtshortname.Text = Session("clientcode")
        txtshortname.Enabled = False
        mvFindInsuranceFile.ActiveViewIndex = 0
        rdtype.SelectedIndex = 0

    End Sub
    
    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click

    End Sub
End Class
