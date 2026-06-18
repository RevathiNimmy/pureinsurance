Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class Claim_Payment_CashList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
            Dim oSAM As New SAMForInsuranceV2

            Dim oRisk As BaseGetHeaderAndRisksByKeyResponseTypeRow

            Dim oRequest As New AttachCoverNoteRequestType
            Dim oResponse As New AttachCoverNoteResponseType

            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")

            oRisk = DirectCast(Session("SelectedRisk"), BaseGetHeaderAndRisksByKeyResponseTypeRow)
            Dim oGetNumberingSchemeRequest As New GetNumberingSchemeNoRequestType
            Dim oGetNumberingSchemeResponse As New GetNumberingSchemeNoResponseType

            oGetNumberingSchemeRequest.BranchCode = Session("BranchCode")
            oGetNumberingSchemeRequest.SchemeType = NumberingSchemeType.CoverNote
            'JP oGetNumberingSchemeRequest.ProductCode = Session("ProductCode")
            oGetNumberingSchemeRequest.AgentKey = Session("LeadAgentKey")

            oGetNumberingSchemeResponse = oSAM.GetNumberingSchemeNo(oGetNumberingSchemeRequest)

            With oGetNumberingSchemeResponse
                If .Errors IsNot Nothing Then
                    lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                Else
                    txtRiskDescription.Text = oRisk.Description
                    txtCoverNoteNumber.Text = oGetNumberingSchemeResponse.GeneratedCode
                    txtCoverFromDate.Text = Date.Now
                    txtCoverToDate.Text = Date.Today.AddDays(Convert.ToDouble(Session("CoverNoteDefaultPeriod")))
                End If
            End With
        End If



    End Sub

    


    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click

        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2

        Dim oRequest As New AttachCoverNoteRequestType
        Dim oResponse As New AttachCoverNoteResponseType
        Dim oRisk As BaseGetHeaderAndRisksByKeyResponseTypeRow

        oRisk = DirectCast(Session("SelectedRisk"), BaseGetHeaderAndRisksByKeyResponseTypeRow)

        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Response.Write("<script>self.close();</script>")

        oRequest.BranchCode = Session("BranchCode")
        oRequest.CoverNote = New BaseCoverNoteRiskItemType
        'JP oRequest.CoverNote.CheckMandatory = True
        oRequest.CoverNote.RiskKey = oRisk.RiskKey
        oRequest.CoverNote.CoverNoteNumber = txtCoverNoteNumber.Text
        'JP oRequest.TimeStamp = Session("TimeStamp")
        oRequest.CoverNote.CoverNoteFrom = txtCoverFromDate.Text
        oRequest.CoverNote.CoverNoteFromSpecified = True
        oRequest.CoverNote.CoverNoteTo = txtCoverToDate.Text
        oRequest.CoverNote.CoverNoteToSpecified = True
        'JP oRequest.InsuranceFolderKey = Session("InsuranceFolderKey")
        oRequest.ProcessType = CoverNoteProcessType.Attach
        oRequest.GenerateCoverNoteDocs = Session("GenerateCoverNoteDocument")
        oRequest.CoverNote.RiskDesc = oRisk.Description

        oResponse = oSAM.AttachCoverNote(oRequest)

        With oResponse
            If Not .Errors Is Nothing Then
                lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
            Else
                Session("TimeStamp") = oResponse.TimeStamp
            End If


        End With
    End Sub
End Class
