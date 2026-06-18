
Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class OpenClaim_5_CoinsuranceBreakDown
    Inherits System.Web.UI.Page
    Dim StartDate As Date

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'create user token from credentials
        'normally the credentials would come from the log in
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        'create the request and response objects
        Dim oGetClaimCoinsurerRequestType As New GetClaimCoinsurerRequestType
        Dim oGetClaimCoinsurerResponseType As New GetClaimCoinsurerResponseType

        'set up request object with some values
        With oGetClaimCoinsurerRequestType
            .ClaimKey = (Session("ClaimKey"))
            .BranchCode = "HeadOff"
        End With

        Try
              StartDate = Date.Now
            oGetClaimCoinsurerResponseType = oSAM.GetClaimCoinsurer(oGetClaimCoinsurerRequestType)
            WriteToLog(Session, "6_CoinsuranceBreakDown.aspx", "SAMForInsuranceV2", "GetClaimCoinsurer", StartDate, Date.Now)
            With oGetClaimCoinsurerResponseType
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                End If
            End With

            'output dataset to the screen to show results
            CROutput.DataSource = oGetClaimCoinsurerResponseType.Coinsurers
            CROutput.DataBind()

            lblOutput.Visible = True
            lblOutput.Text = "Coinsurance BreakDown Details. "

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
