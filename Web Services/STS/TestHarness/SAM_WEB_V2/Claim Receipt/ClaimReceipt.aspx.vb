Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class Claim_Payment_PayClaim
    Inherits System.Web.UI.Page
    Dim StartDate As Date
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oClaimReceiptRequest As New ClaimReceiptRequestType
        Dim oClaimReceiptResponse As New ClaimReceiptResponseType
        oClaimReceiptRequest = DirectCast(Session("ClaimReceiptRequest"), ClaimReceiptRequestType)




        oClaimReceiptRequest.TimeStamp = Session("TimeStamp")

        Try
            StartDate = Date.Now
            oClaimReceiptResponse = oSAM.ClaimReceipt(oClaimReceiptRequest)
            WriteToLog(Session, "ClaimReceipt.aspx", "SAMForInsuranceV2", "ClaimReceipt", StartDate, Date.Now)

            With oClaimReceiptResponse
                If Not (.Errors) Is Nothing Then

                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                Else
                    Response.Write(.ClaimNumber)
                    Session("ClaimNumber") = .ClaimNumber
                    Session("ClaimKey") = .ClaimKey
                    ''Response.Redirect("ClaimReinsuranceBreakdown.aspx")
                End If
            End With

        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            Response.Write("An error occured calling SAM:<br>" & os.Message)

        Catch oe As Exception
            'should do some error handling here. Just output error for now
            Response.Write("An error occured:<br>" & oe.Message)

        Finally
            'clean up any objects here
        End Try



    End Sub
End Class
