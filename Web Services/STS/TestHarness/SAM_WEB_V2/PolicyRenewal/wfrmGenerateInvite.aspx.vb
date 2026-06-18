Imports Microsoft.Web.Services3.Security.Tokens
Partial Class PolicyRenewal_wfrmGenerateInvite
    Inherits System.Web.UI.Page
      Dim StartDate As Date
       Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblOutput.Text = ""
        If Not IsPostBack Then
            Session("Process") = "REN"
        End If
        txtInsuranceRef.Attributes.Add("ReadOnly", "True")
    End Sub

    Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If GetRenewalStatus(Convert.ToInt32(Session("InsuranceFileKey"))) = "AutoReview" Then
            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

            'set up the proxy object
            Dim oSAM As New SAMForInsuranceV2
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")
            'create the request and response objects
            Dim oGenerateInviteRequestType As New GenerateInviteRequestType
            Dim oGenerateInviteResponseType As New GenerateInviteResponseType
            'Use GetHeaderAndSummariesByKey to get timestamp on InsuranceFolderKey that has to be passed on request of GenerateInvite
            Dim oGetHeaderAndSummariesByKeyRequest As New GetHeaderAndSummariesByKeyRequestType
            Dim oGetHeaderAndSummariesByKeResponse As New GetHeaderAndSummariesByKeyResponseType

            oGetHeaderAndSummariesByKeyRequest.BranchCode = "HeadOff"
            oGetHeaderAndSummariesByKeyRequest.InsuranceFileKey = Convert.ToInt32(Session("InsuranceFileKey"))
            Try
                StartDate = Date.Now
                oGetHeaderAndSummariesByKeResponse = oSAM.GetHeaderAndSummariesByKey(oGetHeaderAndSummariesByKeyRequest)
                WriteToLog(Session, "wfrmGenerateInvite.aspx", "SAMForInsuranceV2", "GetHeaderAndSummariesByKey", StartDate, Date.Now)
                If Not (oGetHeaderAndSummariesByKeResponse.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(oGetHeaderAndSummariesByKeResponse.Errors)
                End If
                'If (oGetHeaderAndSummariesByKeResponse.RenewalMethodCode = "AutoReview") Then
                'set up request object with some values
                With oGenerateInviteRequestType
                    'If Session("BranchCode") IsNot Nothing Then
                    '    .BranchCode = Convert.ToString(Session("BranchCode"))
                    'Else
                    .BranchCode = "HeadOff"

                    'End If
                    .InsuranceFileKey = oGetHeaderAndSummariesByKeyRequest.InsuranceFileKey
                    .OutputAsHTML = chkOutputHTML.Checked
                    .OutputAsPDF = chkOutputPDF.Checked
                    .SpoolDocumentOnlySpecified = chkSpoolDocument.Checked
                    .QuoteTimeStamp = oGetHeaderAndSummariesByKeResponse.QuoteTimeStamp

                End With

                StartDate = Date.Now
                oGenerateInviteResponseType = oSAM.GenerateInvite(oGenerateInviteRequestType)
                WriteToLog(Session, "wfrmGenerateInvite.aspx", "SAMForInsuranceV2", "GenerateInvite", StartDate, Date.Now)

                With oGenerateInviteResponseType
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(.Errors)
                    Else
                        lblOutput.Text = "Process completed successfully"
                    End If

                End With
                'Else
                'lblOutput.Text = "Selected policy's status is not Awaiting Renewal Notice Print"
                'End If


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
            lblOutput.Text = "Renewal Status of selected policy is not Awaiting renewal notice print"
        End If

    End Sub
    Private Function GetRenewalStatus(ByVal iInsuranceFileKey As Integer) As String
        Dim strStatus As String = String.Empty
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        'create the request and response objects
        Dim oGetRenewalStatusRequest As New GetRenewalStatusRequestType
        Dim oGetRenewalStatusResponse As New GetRenewalStatusResponseType
        oGetRenewalStatusRequest.BranchCode = "HeadOff"
        oGetRenewalStatusRequest.InsuranceFileKey = iInsuranceFileKey
        Try
            StartDate = Date.Now
            oGetRenewalStatusResponse = oSAM.GetRenewalStatus(oGetRenewalStatusRequest)
            WriteToLog(Session, "wfrmGenerateInvite.aspx", "SAMForInsuranceV2", "GetRenewalStatus", StartDate, Date.Now)

            If Not (oGetRenewalStatusResponse.Errors) Is Nothing Then
                'errors returned, so throw an exception
                Throw New SamResponseException(oGetRenewalStatusResponse.Errors)
            End If
            strStatus = oGetRenewalStatusResponse.RenewalStatusTypeCode.Trim
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
        Return strStatus
    End Function

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("~/UIIC_DEMO/HomePage.aspx")
    End Sub
End Class
