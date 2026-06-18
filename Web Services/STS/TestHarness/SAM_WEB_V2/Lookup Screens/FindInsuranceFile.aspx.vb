Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class Lookup_Screens_FindInsuranceFile
    Inherits System.Web.UI.Page

    Protected Sub mnuInsuranceFile_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles mnuInsuranceFile.MenuItemClick
        mvFindInsuranceFile.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub

    Protected Sub btnFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFind.Click
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oFindPolicyRequest As New FindPolicyRequestType
        Dim oFindPolicyResponse As New FindPolicyResponseType

        With oFindPolicyRequest
            .BranchCode = "HeadOff"
            .ClientShortName = txtShortName.Text
            .InsuranceRef = txtReference.Text



            If rblPolicyType.SelectedValue = "ALL" Then
                .QuoteTypeSpecified = False
            Else
                .QuoteType = Convert.ToInt32(rblPolicyType.SelectedValue)
                .QuoteTypeSpecified = True
            End If
            .ShowLapsedOnlySpecified = False
            .RiskIndex = txtRiskIndex.Text

        End With

        Try
            oFindPolicyResponse = oSAM.FindPolicy(oFindPolicyRequest)

            With oFindPolicyResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Response.Write(GetMessageFromSamError(.Errors))
                Else
                    gvSearchResult.DataSource = .InsuranceFileDetails
                    gvSearchResult.DataBind()
                End If


            End With
        Catch ex As Exception

        End Try

    End Sub
End Class
