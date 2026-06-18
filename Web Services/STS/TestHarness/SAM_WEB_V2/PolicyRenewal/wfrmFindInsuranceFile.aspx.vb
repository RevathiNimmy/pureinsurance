Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class Lookup_Screens_FindInsuranceFile
    Inherits System.Web.UI.Page
      Dim StartDate As Date
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
            StartDate = Date.Now
            oFindPolicyResponse = oSAM.FindPolicy(oFindPolicyRequest)
             WriteToLog(Session, "wfrmFindInsuranceFile.aspx", "SAMForInsuranceV2", "FindPolicy", StartDate, Date.Now)
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Process") IsNot Nothing Then
            If (Session("Process").ToString = "RENSEL") Then
                rblPolicyType.Items.FindByText("Policy").Selected = True
                rblPolicyType.Enabled = False

            ElseIf (Session("Process").ToString = "REN") Then
                rblPolicyType.Items.FindByText("Renewal").Selected = True
                rblPolicyType.Enabled = False
            End If
        End If
        txtInsuranceRef.Attributes.Add("ReadOnly", "True")
    End Sub

    Protected Sub gvSearchResult_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSearchResult.SelectedIndexChanged
        Session("InsuranceFileKey") = Convert.ToInt32(gvSearchResult.SelectedDataKey.Values(0))
        txtInsuranceRef.Text = gvSearchResult.SelectedDataKey.Values(1).ToString
    End Sub
End Class
