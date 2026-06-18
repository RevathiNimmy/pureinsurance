Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class Claim_Payment_CashList
    Inherits System.Web.UI.Page
    Dim StartDate As Date
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        If (IsPostBack = False) Then
            BuildLists(oSAM, ddlBankAccount, STSListType.PMLookup, "BankAccount", "")
            BuildLists(oSAM, ddlCurrency, STSListType.PMLookup, "Currency", "")
            BuildLists(oSAM, ddlType, STSListType.PMLookup, "CashListType", "CP")
            ddlType.Enabled = False
        End If
        'Praveen
        txtDate.Text = Today.Date
        TextBox1.Text = Session("MediaReference")
        'Praveen
        Dim oPayClaimRequest As New PayClaimRequestType


    End Sub

    Private Sub BuildLists(ByVal oSAM As SAMForInsuranceV2, ByRef objControl As DropDownList, ByVal ESTSLookup As STSListType, ByVal ListCode As String, ByVal BindValue As String)
        Dim oRequest As New GetListRequestType
        Dim oResponse As New GetListResponseType


        oRequest.BranchCode = "HeadOff"
        oRequest.ListType = STSListType.PMLookup
        oRequest.ListCode = ListCode


        Try
             StartDate = Date.Now
            oResponse = oSAM.GetList(oRequest)
            WriteToLog(Session, "CashList.aspx", "SAMForInsuranceV2", "GetList", StartDate, Date.Now)
            With oResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                Else

                    objControl.DataSource = oResponse.List
                    objControl.DataTextField = "Description"
                    objControl.DataValueField = "Code"
                    objControl.DataBind()
                    If (BindValue <> "") Then
                        objControl.SelectedValue = BindValue
                    End If


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

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click

        Dim oPayClaimRequest As New PayClaimRequestType

        oPayClaimRequest = DirectCast(Session("PayClaimRequest"), PayClaimRequestType)

        With oPayClaimRequest
            .ClaimPayment.CashList = New BasePaymentCashListType
            .ClaimPayment.CashList.BankAccountCode = ddlBankAccount.SelectedValue
            .ClaimPayment.CashList.CurrencyCode = ddlCurrency.SelectedValue
            .ClaimPayment.CashList.ListDate = txtDate.Text
            .ClaimPayment.CashList.StatusCode = "E"
            .ClaimPayment.CashList.TypeCode = ddlType.SelectedValue
            .ClaimPayment.CashList.Reference = txtReference.Text

        End With

        Session("PayClaimRequest") = oPayClaimRequest
        Session("TransactionDate") = txtDate.Text
        Response.Redirect("CashListItem.aspx")
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("CashListItem.aspx")
    End Sub
End Class
