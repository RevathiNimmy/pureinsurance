Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class CurrencyExchange_CurrencyConversion
    Inherits System.Web.UI.Page
Dim oSAM As New SAMForInsuranceV2
    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        'Dim oRequest As New GetCurrencyExchangeRatesRequestType
        'Dim oResponse As New GetCurrencyExchangeRatesResponseType
        'oRequest.AccountCode = "" 'AR001
        'oRequest.BranchCode = "HeadOff"
        ''oRequest.CurrencyAmountUnRounded = ""
        'oRequest.CurrencyAmountUnRoundedSpecified = False
        'oRequest.Mode = "ALL"
        'oRequest.TransactionCurrencyCode = "" ' ddlCurrency code

        'Dim oRequest As New GetCurrencyExchangeRatesRequestType
        'Dim oResponse As New GetCurrencyExchangeRatesResponseType
        'oRequest.AccountCode = Session("ACCOUNTCODE")
        'oRequest.BranchCode = "HeadOff"
        ''oRequest.CurrencyAmountUnRounded = ""
        'oRequest.CurrencyAmountUnRoundedSpecified = False
        'oRequest.Mode = "ALL"
        'oRequest.TransactionCurrencyCode = ddlTransactionCurrency.SelectedValue
        'Session("TRANSACTIONCURRENCYCODE") = ddlTransactionCurrency.SelectedValue
        'oResponse = oSAM.GetCurrencyExchangeRates(oRequest)
        'FillingCurrency(oResponse)

    End Sub
    Private Sub BuildLists(ByVal oSAM As SAMForInsuranceV2, ByRef objControl As DropDownList, ByVal ESTSLookup As STSListType, ByVal ListCode As String)
        Dim oRequest As New GetListRequestType
        Dim oResponse As New GetListResponseType


        oRequest.BranchCode = "HeadOff"
        oRequest.ListType = ESTSLookup.PMLookup
        oRequest.ListCode = ListCode


        Try
            Dim StartDate As Date
            StartDate = Date.Now
            oResponse = oSAM.GetList(oRequest)
            WriteToLog(Session, "PolicyHeader.aspx", "SAMForInsuranceV2", "GetList", StartDate, Date.Now)
            With oResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    ' lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                Else

                    objControl.DataSource = oResponse.List
                    objControl.DataTextField = "Description"
                    objControl.DataValueField = "key"
                    objControl.DataBind()
                End If
                If ListCode = "source" Then
                    Session("Branch") = oResponse.List
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Response.CacheControl = "no-cache"
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        BuildLists(oSAM, ddlBaseCurrency, STSListType.PMLookup, "Currency")
        BuildLists(oSAM, ddlTransactionCurrency, STSListType.PMLookup, "Currency")
        BuildLists(oSAM, ddlSystemCurrency, STSListType.PMLookup, "Currency")
        btnOk.Attributes.Add("OnClick", "window.close(); return false")
        btnCancel.Attributes.Add("OnClick", "CLOSED()")

        Session("POPUP") = "SHOW"

        Dim oRequest As New GetCurrencyExchangeRatesRequestType
        Dim oResponse As New GetCurrencyExchangeRatesResponseType
        oRequest.AccountCode = Session("ACCOUNTCODE")
        oRequest.BranchCode = "HeadOff"
        'oRequest.CurrencyAmountUnRounded = ""
        oRequest.CurrencyAmountUnRoundedSpecified = False
        oRequest.Mode = "ALL"
        'ddlTransactionCurrency.SelectedValue = Session("TRANSACTIONCURRENCYCODE")
        oRequest.TransactionCurrencyCode = Session("TRANSACTIONCURRENCYCODE")
        oResponse = oSAM.GetCurrencyExchangeRates(oRequest)
        FillingCurrency(oResponse)
    End Sub
    Public Sub FillingCurrency(ByRef oResponse As GetCurrencyExchangeRatesResponseType)
        ddlTransactionCurrency.SelectedValue = oResponse.CurrencyRates.TransactionCurrencyKey
        ddlDateofExchange.Items.Clear()
        ddlDateofExchange.Items.Add(Today.Date)
        ddlDateofExchange.SelectedItem.Text = Today.Date
        ddlBaseCurrency.SelectedValue = oResponse.CurrencyRates.BaseCurrencyKey
        txtBaseCurrencyRate.Text = oResponse.CurrencyRates.BaseCurrencyRate
        ddlSystemCurrency.SelectedValue = oResponse.CurrencyRates.SystemCurrencyKey
        txtSystemCurrencyRate.Text = oResponse.CurrencyRates.BaseCurrencyRate / (oResponse.CurrencyRates.SystemCurrrencyRate)
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

    End Sub
End Class

