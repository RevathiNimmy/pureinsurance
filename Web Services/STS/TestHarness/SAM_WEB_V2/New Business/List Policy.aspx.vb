Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2

Partial Class MTA_List_Policy
    Inherits System.Web.UI.Page
    Dim StartDate As Date
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (Not IsPostBack) Then
            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

            'set up the proxy object
            Dim oSAM As New SAMForInsuranceV2
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")

            Dim oGetPartySummaryRequest As New GetPartySummaryRequestType
            Dim oGetPartySummaryResponseType As GetPartySummaryResponseType
            Dim oSelectedParty As New BaseFindPartyResponseTypeRow
            oSelectedParty = DirectCast(Session("SelectedParty"), BaseFindPartyResponseTypeRow)


            With oGetPartySummaryRequest
                .BranchCode = "HeadOff"
                .PartyKey = oSelectedParty.PartyKey
            End With

            Try
                StartDate = Date.Now
                oGetPartySummaryResponseType = oSAM.GetPartySummary(oGetPartySummaryRequest)
                WriteToLog(Session, "ListPolicy.aspx", "SAMForInsuranceV2", "GetPartySummary", StartDate, Date.Now)
                With oGetPartySummaryResponseType
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                    Else

                        txtClientCode.Text = oSelectedParty.ShortName

                        Dim oGetPartySummaryRow() As BaseGetPartySummaryResponseTypeRow

                        For PolicyCount As Integer = 0 To oGetPartySummaryResponseType.Policies.Length - 1
                            If oGetPartySummaryResponseType.Policies(PolicyCount).InsuranceFileTypeCode.Trim = "QUOTE" Then
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


    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Session("SelectedPolicy") = Nothing
        Session("ACCOUNTCODE") = txtClientCode.Text
        Response.Redirect("SelectProduct.aspx")
    End Sub


    Protected Sub gvResult_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvResult.SelectedIndexChanged
        Dim oBaseGetPartySummaryResponseTypeRow() As BaseGetPartySummaryResponseTypeRow
        oBaseGetPartySummaryResponseTypeRow = DirectCast(Session("Policies"), BaseGetPartySummaryResponseTypeRow())
        Session("InsuranceFileKey") = oBaseGetPartySummaryResponseTypeRow(gvResult.SelectedIndex).InsuranceFileKey
        Session("InsuranceFolderKey") = oBaseGetPartySummaryResponseTypeRow(gvResult.SelectedIndex).InsuranceFolderKey
        Session("SelectedPolicy") = oBaseGetPartySummaryResponseTypeRow(gvResult.SelectedIndex)
        Session("ACCOUNTCODE") = txtClientCode.Text
        Response.Redirect("PolicyHeader.aspx")
    End Sub

End Class
