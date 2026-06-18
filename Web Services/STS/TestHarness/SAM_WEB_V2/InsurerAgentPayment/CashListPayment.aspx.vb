Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class Claim_Payment_CashList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        If (IsPostBack = False) Then
            BuildLists(oSAM, ddlBankAccount, STSListType.PMLookup, "BankAccount", "")
            BuildLists(oSAM, ddlCurrency, STSListType.PMLookup, "Currency", "")
            BuildLists(oSAM, ddlType, STSListType.PMLookup, "CashListType", "P")
            ' ddlType.Items.RemoveAt(1)
            ddlType.Enabled = True
            txtDate.Text = Date.Now()
            txtStatus.Text = "ENTERED"
            txtStatus.Enabled = False

        End If
        ' Dim oPayClaimRequest As New PayClaimRequestType


    End Sub

    Private Sub BuildLists(ByVal oSAM As SAMForInsuranceV2, ByRef objControl As DropDownList, ByVal ESTSLookup As STSListType, ByVal ListCode As String, ByVal BindValue As String)
        Dim oRequest As New GetListRequestType
        Dim oResponse As New GetListResponseType


        oRequest.BranchCode = "HeadOff"
        oRequest.ListType = STSListType.PMLookup
        oRequest.ListCode = ListCode


        Try
            oResponse = oSAM.GetList(oRequest)

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
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        Dim oRequest As New CreatePaymentCashListWithItemsRequestType
        Dim oResponse As New CreatePaymentCashListWithItemsResponseType


        oRequest.PaymentCashList = New BasePaymentCashListType
        oRequest.BranchCode = "HeadOff"

        With oRequest.PaymentCashList

            .BankAccountCode = ddlBankAccount.SelectedValue
            .CurrencyCode = ddlCurrency.SelectedValue
            .ListDate = txtDate.Text
            .StatusCode = "E"
            .TypeCode = ddlType.SelectedValue
            .Reference = txtReference.Text

        End With
        Session("CashList") = oRequest

        oResponse = oSAM.CreatePaymentCashListWithItems(oRequest)


        Session("CashListKey") = oResponse.CashListKey
        Response.Redirect("CashListItemsfirstPayment.aspx")

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Write("<script>self.close();</script>")
    End Sub
End Class
