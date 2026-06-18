Imports Microsoft.Web.Services3.Security.Tokens
Partial Class PolicyRenewal_wfrmRenewalSelectionByPolicy
    Inherits System.Web.UI.Page
    Dim StartDate As Date

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblOutput.Text = ""
        If (Not IsPostBack) Then
            'Get the sytem option and ask for confirmation.
            lblConfirmMessage.Text = GetMessageForConfirmation()
            pnlConfirm.Visible = True
            pnlRenewalSelection.Visible = False
            Session("Process") = "RENSEL"
        End If
        txtInsuranceRef.Attributes.Add("ReadOnly", "True")
    End Sub

    Protected Sub btnYes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnYes.Click
        pnlConfirm.Visible = False
        pnlRenewalSelection.Visible = True
    End Sub
    Private Function GetMessageForConfirmation() As String
        Dim strMessage As String = String.Empty

        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        'create the request and response objects
        Dim oGetOptionSettingRequest As New GetOptionSettingRequestType
        Dim oGetOptionSettingResponse As New GetOptionSettingResponseType

        oGetOptionSettingRequest.BranchCode = "HeadOff"
        oGetOptionSettingRequest.OptionType = OptionType.SystemOption
        oGetOptionSettingRequest.OptionNumber = 155
        Try
            StartDate = Date.Now
            oGetOptionSettingResponse = oSAM.GetOptionSetting(oGetOptionSettingRequest)
            WriteToLog(Session, "wfrmRenewalSelectionByPolicy.aspx", "SAMForInsuranceV2", "GetOptionSetting", StartDate, Date.Now)
            'oGenerateInviteResponseType.

            With oGetOptionSettingResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                End If
                strMessage = oGetOptionSettingResponse.OptionValue + "<br/><br/>" + "Do you wish to proceed?"

            End With


        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            lblOutput.Visible = True
            lblOutput.Text = "An error occured calling SAM:<br>" & os.Message
        Catch oe As Exception
            'should do some error handling here. Just output error for now
            lblOutput.Visible = True
            lblOutput.Text = "An error occured:<br>" & oe.Message
        Finally
            'clean up any objects here
        End Try
        Return strMessage
    End Function

    Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If (Session("InsuranceFileKey") IsNot Nothing) Then

            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

            'set up the proxy object
            Dim oSAM As New SAMForInsuranceV2
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")

            'create the request and response objects
            Dim oRunRenewalSelectionByPolicyRequest As New RunRenewalSelectionByPolicyRequestType
            Dim oRunRenewalSelectionByPolicyResponse As New RunRenewalSelectionByPolicyResponseType


            oRunRenewalSelectionByPolicyRequest.BranchCode = "HeadOff"
            oRunRenewalSelectionByPolicyRequest.InsuranceFileKey = Convert.ToInt32(Session("InsuranceFileKey"))
            Try
                StartDate = Date.Now
                oRunRenewalSelectionByPolicyResponse = oSAM.RunRenewalSelectionByPolicy(oRunRenewalSelectionByPolicyRequest)
                WriteToLog(Session, "wfrmRenewalSelectionByPolicy.aspx", "SAMForInsuranceV2", "RunRenewalSelectionByPolicy", StartDate, Date.Now)
                'oGenerateInviteResponseType.

                With oRunRenewalSelectionByPolicyResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(.Errors)
                    End If
                    lblOutput.Text = "Process completed successfully"
                End With


            Catch os As SamResponseException
                'should do some error handling here. Just output error for now
                lblOutput.Visible = True
                lblOutput.Text = "An error occured calling SAM:<br>" & os.Message
            Catch oe As Exception
                'should do some error handling here. Just output error for now
                lblOutput.Visible = True
                lblOutput.Text = "An error occured:<br>" & oe.Message
            Finally
                'clean up any objects here
            End Try
        Else
            lblOutput.Text = "Session values corrupted. Unable to complete the task"
        End If
    End Sub

    Protected Sub btnNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNo.Click
        Response.Redirect("~/UIIC_DEMO/HomePage.aspx")
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("~/UIIC_DEMO/HomePage.aspx")
    End Sub
End Class
