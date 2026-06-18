Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class CheckUnPaidPremium
    Inherits System.Web.UI.Page
    Dim oOpenClaimRequestType As New OpenClaimRequestType
    Dim UserToken As UsernameToken
    Dim oSAM As New SAMForInsuranceV2
    Dim StartDate As Date

    Protected Sub btnCheckUnPaidPremium_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        oOpenClaimRequestType = DirectCast(Session("OpenClaimRequestType"), OpenClaimRequestType)
        If Not oOpenClaimRequestType.Claim.InfoOnly Then
            Response.Redirect("4_Perils.aspx")
        Else
            Response.Redirect("OpenClaim.aspx")
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'create user token from credentials
        'normally the credentials would come from the log in
        UserToken = GetUserToken("sirius", "sirius")

        'set up the proxy object

        'Dim oOpenClaimSelectedPolicy As New BaseFindInsuranceFileResponseTypeRow
        'oOpenClaimSelectedPolicy = DirectCast(Session("OpenClaimSelectedPolicy"), BaseFindInsuranceFileResponseTypeRow)


        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        'create the request and response objects
        Dim oCheckUnpaidPremiumRequestType As New CheckUnpaidPremiumRequestType
        Dim oCheckUnpaidPremiumResponseType As New CheckUnpaidPremiumResponseType

        'set up request object with some values
        Dim oOpenClaimSelectedPolicy As New BaseFindInsuranceFileResponseTypeRow
        oOpenClaimSelectedPolicy = DirectCast(Session("OpenClaimSelectedPolicy"), BaseFindInsuranceFileResponseTypeRow)
        lblPolicyNo.Text = oOpenClaimSelectedPolicy.InsuranceRef
        lblClaimDate.Text = Session("ClaimDate")

        With oCheckUnpaidPremiumRequestType

            .InsuranceRef = oOpenClaimSelectedPolicy.InsuranceRef
            .BranchCode = "HeadOff"
        End With

        Try
             StartDate = Date.Now
            oCheckUnpaidPremiumResponseType = oSAM.CheckUnpaidPremium(oCheckUnpaidPremiumRequestType)
            WriteToLog(Session, "3_CheckUnPaidPremium.aspx", "SAMForInsuranceV2", "CheckUnpaidPremium", StartDate, Date.Now)

            With oCheckUnpaidPremiumResponseType
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                End If
            End With

            lblOutput.Visible = True
            If Not oCheckUnpaidPremiumResponseType.Transactions Is Nothing Then
                gvTransactions.DataSource = oCheckUnpaidPremiumResponseType.Transactions
                gvTransactions.DataBind()
                lblOutput.Text = "Transactions found : " & oCheckUnpaidPremiumResponseType.Transactions.Length
            End If

            'add to session
            Session("CheckUnpaidPremiumResponse") = oCheckUnpaidPremiumResponseType
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
