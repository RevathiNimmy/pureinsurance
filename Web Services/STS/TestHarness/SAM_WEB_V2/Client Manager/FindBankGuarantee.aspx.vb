Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class BankGuarantee_FindBankGuarantee
    Inherits System.Web.UI.Page

    Protected Sub btnFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFind.Click
        'create user token from credentials
        'normally the credentials would come from the log in
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        'create the request and response objects
        Dim oFindBankGauranteeRequest As New FindBankGuaranteeRequestType
        Dim oFindBankGauranteeResponse As New FindBankGuaranteeResponseType

        'set up request object with some values
        With oFindBankGauranteeRequest

            If Not (String.IsNullOrEmpty(txtClient.Text)) Then
                .PartyCode = txtClient.Text
            End If
            If Not (String.IsNullOrEmpty(txtBankGuaranteeRef.Text)) Then
                .BankGuaranteeRef = txtBankGuaranteeRef.Text.Trim()

            End If
            If Not (String.IsNullOrEmpty(txtBankName.Text)) Then
                .BankNameCode = txtBankName.Text
            End If
            If Not (String.IsNullOrEmpty(txtInsuranceFile.Text)) Then
                .InsuranceRef = txtInsuranceFile.Text
            End If
            'If Not (txtClient.Text.Trim().Length = 0) Then
            '    '.PartyKeySpecified = True
            '    '.PartyKey = Int32.Parse(txtClient.Text.Trim())
            'End If
            'If Not (txtBankGuaranteeRef.Text.Trim().Length = 0) Then
            'End If
            'If Not (txtAgent.Text.Trim().Length = 0) Then
            '    '.AgentKeySpecified = True
            '    '.AgentKey = Int32.Parse(txtAgent.Text.Trim())
            '    .AgentCode = txtAgent.Text
            'End If
            'If Not (txtBankName.Text.Trim().Length = 0) Then
            '    .BankNameCode = txtBankName.Text.Trim()
            'End If
            'If Not (txtInsuranceFile.Text.Trim().Length = 0) Then
            '    '.InsuranceFileKeySpecified = True
            '    '.InsuranceFileKey = Int32.Parse(txtInsuranceFile.Text.Trim())
            'End If

            If Not (txtbgstatus.Text.Trim().Length = 0) Then

                .BGStatusCode = txtbgstatus.Text.Trim()
            End If
            .BranchCode = "HeadOff"
        End With

        Try
            oFindBankGauranteeResponse = oSAM.FindBankGuarantee(oFindBankGauranteeRequest)

            With oFindBankGauranteeResponse
                If (.Errors) Is Nothing Then
                    gvBankGuarantees.DataSource = .BankGuarantee
                    gvBankGuarantees.DataBind()
                Else
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
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
