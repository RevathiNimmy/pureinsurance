Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2

Partial Class MTA_List_Policy
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (Not IsPostBack) Then
            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

            'set up the proxy object
            Dim oSAM As New SAMForInsuranceV2
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")

            Dim oGetPartySummaryRequest As New GetPartySummaryRequestType
            Dim oGetPartySummaryResponseType As GetPartySummaryResponseType

            With oGetPartySummaryRequest
                .BranchCode = "HEADOFF"
                .PartyKey = Convert.ToInt32(Session("PARTYKEY"))
            End With

            Try
                oGetPartySummaryResponseType = oSAM.GetPartySummary(oGetPartySummaryRequest)

                With oGetPartySummaryResponseType
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Response.Write(GetMessageFromSamError(.Errors))
                    Else

                        txtClientCode.Text = .Policies(0).PartyShortName
                        'Session("PartyName") = .Policies(0).PartyShortName

                        Dim oGetPartySummaryRow() As BaseGetPartySummaryResponseTypeRow

                        For PolicyCount As Integer = 0 To oGetPartySummaryResponseType.Policies.Length - 1
                            If oGetPartySummaryResponseType.Policies(PolicyCount).InsuranceFileTypeCode.Trim = "POLICY" And oGetPartySummaryResponseType.Policies(PolicyCount).PolicyStatus = "Cancelled" Then
                                If oGetPartySummaryRow Is Nothing Then
                                    ReDim Preserve oGetPartySummaryRow(0)
                                Else
                                    ReDim Preserve oGetPartySummaryRow(oGetPartySummaryRow.Length)
                                End If

                                oGetPartySummaryRow(oGetPartySummaryRow.Length - 1) = New BaseGetPartySummaryResponseTypeRow
                                oGetPartySummaryRow(oGetPartySummaryRow.Length - 1) = oGetPartySummaryResponseType.Policies(PolicyCount)

                            End If

                        Next
                        gvResult.DataSource = oGetPartySummaryRow
                        gvResult.DataBind()
                        Session("Policies") = oGetPartySummaryRow

                    End If
                End With
            Catch os As SamResponseException
                'should do some error handling here. Just output error for now
                Response.Write("An error occured calling SAM:<br>" & os.Message)
            Catch oe As Exception
                'should do some error handling here. Just output error for now
                Response.Write("An error occured:<br>" & oe.Message)
            Finally
                'clean
            End Try
        End If
    End Sub

    'Protected Sub gvResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvResult.RowCommand
    '    If (e.CommandName.Equals("Select")) Then
    '        Session("InsuranceFolderkey") = e.CommandArgument

    '        Dim oBaseGetPartySummaryResponseTypeRow() As BaseGetPartySummaryResponseTypeRow
    '        oBaseGetPartySummaryResponseTypeRow = DirectCast(Session("Policies"), BaseGetPartySummaryResponseTypeRow())
    '        Session("InsuranceFileKey") = oBaseGetPartySummaryResponseTypeRow(e.CommandSource.).InsuranceFileKey
    '        Session("SelectedPolicy") = oBaseGetPartySummaryResponseTypeRow(gvResult.SelectedIndex)
    '        Response.Redirect("ListPolicyVersions.aspx")
    '    End If
    'End Sub

    Protected Sub gvResult_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvResult.SelectedIndexChanged
        Dim oBaseGetPartySummaryResponseTypeRow() As BaseGetPartySummaryResponseTypeRow
        oBaseGetPartySummaryResponseTypeRow = DirectCast(Session("Policies"), BaseGetPartySummaryResponseTypeRow())
        Session("InsuranceFileKey") = oBaseGetPartySummaryResponseTypeRow(gvResult.SelectedIndex).InsuranceFileKey
        Session("InsuranceFolderKey") = oBaseGetPartySummaryResponseTypeRow(gvResult.SelectedIndex).InsuranceFolderKey
        'ravi

        Session("SelectedPolicy") = oBaseGetPartySummaryResponseTypeRow(gvResult.SelectedIndex)
        Response.Redirect("ListPolicyVersions.aspx")
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("Find Party.aspx")
    End Sub
End Class
