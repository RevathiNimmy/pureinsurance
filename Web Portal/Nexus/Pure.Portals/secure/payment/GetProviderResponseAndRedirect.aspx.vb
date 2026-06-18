Imports CMS.library
Imports Nexus.Utils
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Security.Authentication


Namespace Nexus

    Partial Class secure_payment_GetProviderResponseAndRedirect : Inherits Frontend.clsCMSPage


        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            
            Dim obasepament As New BasePayment
            Dim oBankDetails As New NexusProvider.BankCollection
            Dim oBank As New NexusProvider.Bank
            Dim oWebService As NexusProvider.ProviderBase
            

            obasepament.GetProviderResponse()
            Dim oPaymentHubDetails As NexusProvider.PaymentHubDetails = CType(HttpContext.Current.Session(CNPaymentHubDetails), NexusProvider.PaymentHubDetails)

            If oPaymentHubDetails.ResultDescription = PaymentHub.ResultDescription.Authorised Then
                Dim oAccountSearchCr As NexusProvider.AccountSearchCriteria
                Dim oAccountColl As NexusProvider.AccountSearchResultCollection
                Dim oQuote As NexusProvider.Quote
                Dim oCreditCard As New NexusProvider.CreditCard
                oWebService = New NexusProvider.ProviderManager().Provider

                oCreditCard.Number = oPaymentHubDetails.CardNumber
                oCreditCard.AuthCode = oPaymentHubDetails.IntegrationToken
                oCreditCard.ExpiryDate = IIf(oPaymentHubDetails.CardExpiry.IndexOf("/") = -1 AndAlso oPaymentHubDetails.CardExpiry.Length = 4, oPaymentHubDetails.CardExpiry.Substring(0, 2) & "/" & oPaymentHubDetails.CardExpiry.Substring(2, 2), oPaymentHubDetails.CardExpiry)
                oCreditCard.TransactionCode = oPaymentHubDetails.TransactionID
                oCreditCard.PartyBankKey = oPaymentHubDetails.PartyBankKey
                oCreditCard.TrackingNumber = oPaymentHubDetails.TokenID
				oCreditCard.VIAPaymentHub = True

                oBank.CreditCard = oCreditCard
                oAccountSearchCr = New NexusProvider.AccountSearchCriteria
                If Session(CNQuote) IsNot Nothing Then

                    oQuote = Session(CNQuote)
                    oAccountSearchCr.ShortCode = oQuote.ClientCode
                    oAccountColl = oWebService.FindAccounts(oAccountSearchCr)

                    If oAccountColl IsNot Nothing AndAlso oAccountColl.Count > 0 Then
                        oBank.AccountHolderName = oAccountColl(0).ContactName
                    Else
                        oBank.AccountHolderName = oQuote.ClientCode
                    End If

                    oBank.BankPaymentTypeCode = "ANY"
                    oBank.IsBankItem = False
                    oBank.AccountType = "Payment Hub"
                    oBank.AccountKey = oAccountColl(0).AccountKey
                    oBankDetails.Add(oBank)
                    oBankDetails(0).CreditCard.NameOnCreditCard = oBank.AccountHolderName
                    oBankDetails(0).TaskMode = NexusProvider.Bank.Mode.Add
                    oWebService.AddPartyBankDetails(oQuote.PartyKey, oBankDetails, oQuote.BranchCode)
                    oBankDetails(0).CreditCard.PartyBankKey = oBankDetails(0).PartyBankKey
                    Session(CNCardDetails) = oBankDetails(0).CreditCard
                    Session(CNPaid) = True
                Else

                    oAccountSearchCr.ShortCode = oPaymentHubDetails.PartyCode
                    oAccountColl = oWebService.FindAccounts(oAccountSearchCr)
                    If oAccountColl IsNot Nothing AndAlso oAccountColl.Count > 0 Then
                        oBank.AccountHolderName = oAccountColl(0).ContactName
                    End If
                    oBank.BankPaymentTypeCode = "ANY"
                    oBank.IsBankItem = False
                    oBank.AccountType = "Payment Hub"
                    oBank.AccountKey = oAccountColl(0).AccountKey

                    oBankDetails.Add(oBank)
                    oBankDetails(0).CreditCard.NameOnCreditCard = oBank.AccountHolderName
                    oBankDetails(0).TaskMode = NexusProvider.Bank.Mode.Add
                    oWebService.AddPartyBankDetails(oPaymentHubDetails.PartyKey, oBankDetails, Session(CNTransBranchCode))
                    oBankDetails(0).CreditCard.PartyBankKey = oBankDetails(0).PartyBankKey
                End If
            End If
            Dim CashListItemIndex As Integer = oPaymentHubDetails.CashListItemIndex
            If Session(CNCashListItem) IsNot Nothing AndAlso CashListItemIndex >= 0 Then
                Dim oReceiptCashListCollection As New NexusProvider.ReceiptCashListCollection
                Dim oReceiptCashListItems As NexusProvider.ReceiptCashListItemType = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType)
                oReceiptCashListItems.ReceiptItems(CashListItemIndex).PaymentHubDetails = oPaymentHubDetails
            End If

            'oPaymentHubDetails.

            If oPaymentHubDetails.ReturnURL.Contains("TransactionConfirmation") AndAlso oPaymentHubDetails.ResultCode = "0" Then
                Session(CNPaid) = True
            End If

            Response.Redirect(oPaymentHubDetails.ReturnURL)


        End Sub
        Protected Shadows Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            If Request("modal") = "true" Then
                CMS.Library.Frontend.Functions.SetTheme(Page, ConfigurationManager.AppSettings("ModalPageTemplate"))
            End If

        End Sub




    End Class

End Namespace