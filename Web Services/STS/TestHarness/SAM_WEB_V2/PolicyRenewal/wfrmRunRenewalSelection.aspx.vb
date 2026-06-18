Imports Microsoft.Web.Services3.Security.Tokens
Partial Class PolicyRenewal_wfrmRunRenewalSelection
    Inherits System.Web.UI.Page
     Dim StartDate As Date
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblOutput.Text = ""
        If (Not IsPostBack) Then
            'Get the sytem option and ask for confirmation.
            lblConfirmMessage.Text = GetMessageForConfirmation()
            If Not String.IsNullOrEmpty(lblConfirmMessage.Text) Then
                lblConfirmMessage.Text = lblConfirmMessage.Text + "<br/><br/>" + "Do you wish to proceed?"
                pnlConfirm.Visible = True
                pnlFilterRenewalSelection.Visible = False
            Else
                pnlConfirm.Visible = False
                pnlFilterRenewalSelection.Visible = True
            End If

            calStartDate.Visible = chkStartDate.Checked
            calStartDate.SelectedDate = Today.Date
            calCompareDate.SelectedDate = Today.Date
            PopulateBranchCode()
            PopulateProductCode()
        End If
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
            WriteToLog(Session, "wfrmRunRenewalSelection.aspx", "SAMForInsuranceV2", "GetOptionSetting", StartDate, Date.Now)
            'oGenerateInviteResponseType.

            With oGetOptionSettingResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                End If
                strMessage = oGetOptionSettingResponse.OptionValue

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

    Private Sub PopulateBranchCode()
        ddlBranchCode.Items.Clear()
        ddlBranchCode.Items.Add(New ListItem("", 0))
        'create user token from credentials
        'normally the credentials would come from the log in
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        'create the request and response objects
        Dim oGetListRequestType As New GetListRequestType
        Dim oGetListResponseType As New GetListResponseType

        'set up request object with some values
        With oGetListRequestType
            .ListCode = "Source"
            .ListType = STSListType.PMLookup
            .BranchCode = "HeadOff"
        End With

        Try
             StartDate = Date.Now
            oGetListResponseType = oSAM.GetList(oGetListRequestType)
            WriteToLog(Session, "wfrmRunRenewalSelection.aspx", "SAMForInsuranceV2", "GetList", StartDate, Date.Now)

            With oGetListResponseType
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                End If
                ddlBranchCode.DataSource = oGetListResponseType.List
                ddlBranchCode.DataTextField = "Description"
                ddlBranchCode.DataValueField = "Code"
                ddlBranchCode.DataBind()
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
    End Sub

    Private Sub PopulateProductCode()
        ddlProductCode.Items.Clear()
        ddlProductCode.Items.Add(New ListItem("All", 0))
        'create user token from credentials
        'normally the credentials would come from the log in
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        'create the request and response objects
        Dim oGetListRequestType As New GetListRequestType
        Dim oGetListResponseType As New GetListResponseType

        'set up request object with some values
        With oGetListRequestType
            .ListCode = "Product"
            .ListType = STSListType.PMLookup
            .BranchCode = "HeadOff"
        End With

        Try 
            StartDate = Date.Now
            oGetListResponseType = oSAM.GetList(oGetListRequestType)
            WriteToLog(Session, "wfrmRunRenewalSelection.aspx", "SAMForInsuranceV2", "GetList", StartDate, Date.Now)

            With oGetListResponseType
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                End If
                ddlProductCode.DataSource = oGetListResponseType.List
                ddlProductCode.DataTextField = "Description"
                ddlProductCode.DataValueField = "Code"
                ddlProductCode.DataBind()
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
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("~/UIIC_DEMO/HomePage.aspx")
    End Sub

    Protected Sub chkStartDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkStartDate.CheckedChanged
        calStartDate.Visible = chkStartDate.Checked
    End Sub
    Private Function GetPoliciesForRenewalSelection() As BaseGetPoliciesForRenewalSelectionResponseType
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oRequest As New GetPoliciesForRenewalSelectionRequestType
        Dim oResponse As New GetPoliciesForRenewalSelectionResponseType

        With oRequest
            .BranchCode = ddlBranchCode.SelectedValue.Trim
            .ProductCode = ddlProductCode.SelectedValue.Trim
            .CompareDate = calCompareDate.SelectedDate
            If (chkStartDate.Checked) Then
                .StartDate = calStartDate.SelectedDate
                .StartDateSpecified = True
            End If
        End With
         StartDate = Date.Now
        oResponse = oSAM.GetPoliciesForRenewalSelection(oRequest)
          WriteToLog(Session, "wfrmRunRenewalSelection.aspx", "SAMForInsuranceV2", "GetPoliciesForRenewalSelection", StartDate, Date.Now)

         
        With oResponse
            If Not (.Errors) Is Nothing Then
                'errors returned, so throw an exception
                Throw New SamResponseException(.Errors)
            End If
        End With
        Return oResponse
    End Function

    Private Sub RunRenewalSelection(ByVal oRequest As GetPoliciesForRenewalSelectionResponseType)
        If oRequest.Policies IsNot Nothing AndAlso oRequest.Policies.Length > 0 Then
            For Each oPolicy As BaseGetPoliciesForRenewalSelectionResponseTypeRow In oRequest.Policies
                RunRenewalSelectionByPolicy(oPolicy.InsuranceFileKey)
            Next
            lblOutput.Text = "Process completed successfully"
        Else
            lblOutput.Text = "No policies found for the given selection criteria"
        End If
    End Sub
    Private Sub RunRenewalSelectionByPolicy(ByVal iInsuranceFileKey As Integer)
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        'create the request and response objects
        Dim oRunRenewalSelectionByPolicyRequest As New RunRenewalSelectionByPolicyRequestType
        Dim oRunRenewalSelectionByPolicyResponse As New RunRenewalSelectionByPolicyResponseType


        oRunRenewalSelectionByPolicyRequest.BranchCode = "HeadOff"
        oRunRenewalSelectionByPolicyRequest.InsuranceFileKey = iInsuranceFileKey
         StartDate = Date.Now
        oRunRenewalSelectionByPolicyResponse = oSAM.RunRenewalSelectionByPolicy(oRunRenewalSelectionByPolicyRequest)
        WriteToLog(Session, "wfrmRunRenewalSelection.aspx", "SAMForInsuranceV2", "RunRenewalSelectionByPolicy", StartDate, Date.Now)

        'oGenerateInviteResponseType.

        With oRunRenewalSelectionByPolicyResponse
            If Not (.Errors) Is Nothing Then
                'errors returned, so throw an exception
                Throw New SamResponseException(.Errors)
            End If
        End With
    End Sub

    Protected Sub btnYes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnYes.Click
        pnlConfirm.Visible = False
        pnlFilterRenewalSelection.Visible = True
    End Sub

    Protected Sub btnNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNo.Click
        Response.Redirect("~/UIIC_DEMO/HomePage.aspx")
    End Sub

    Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Try
            RunRenewalSelection(GetPoliciesForRenewalSelection())
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
    End Sub
End Class
