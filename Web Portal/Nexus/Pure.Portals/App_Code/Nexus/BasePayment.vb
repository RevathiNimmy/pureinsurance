Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Constants.Constant
Imports System.Net
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports System.IO
Imports System.Xml

Namespace Nexus

    ''' <summary>
    ''' </summary>
    ''' <remarks>DH - Can someone how knows what this does add comments?</remarks>
    Public Class BasePayment : Inherits Frontend.clsCMSPage

        Private Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'write code to check that this page (i.e the current page) is in the payment configuration for the current portal
            Dim paymentTypes As New Config.PaymentTypes
            paymentTypes = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID).PaymentTypes
            'If Request.Url.AbsolutePath.ToLower().IndexOf(paymentTypes.PaymentType(Session(CNSelectedPaymentIndex)).Url.Remove(0, 2).ToLower()) = -1 Then
            '    Throw New Exception("Payment type not supported")
            'End If

        End Sub

        Public Sub SetPaymentTakenAndRedirect()
            'set appropriate session values here to indicate payment taken and then redirect to end page
            Session(CNPaid) = True
            Response.Redirect("~/secure/TransactionConfirmation.aspx", False)
        End Sub

#Region "Paymeent HUb"


        Private Function InitializePaymentHubIntegrationService() As PaymentHub.IntegrationService
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolTypeExtensions.Tls12 Or SecurityProtocolType.Ssl3
            Dim oPaymentHub As New PaymentHub.IntegrationService
            'Write definition for other security things after getting Payment Hub security implemented

            Dim oSecurityValue As New PaymentHub.Security
            Dim oPaymentHubConfig As NexusProvider.PaymentHubConfig = GetPaymentHubConfig()

            oSecurityValue.SystemUsername = oPaymentHubConfig.SystemUserName
            oSecurityValue.SystemPassword = oPaymentHubConfig.Password
            oSecurityValue.SystemID = 2

            oPaymentHub.SecurityValue = oSecurityValue
            oPaymentHub.Url = oPaymentHubConfig.PaymentHubServiceUrl
            Return oPaymentHub
        End Function

        Private Function InitializePaymentHubClientHeader() As PaymentHub.RequestHeader
            Dim oRequestHeader As New PaymentHub.RequestHeader
            Dim oClientHeader As New PaymentHub.ClientHeader
            Dim oPaymentHubConfig As NexusProvider.PaymentHubConfig = GetPaymentHubConfig()
            oClientHeader.ClientName = oPaymentHubConfig.ClientName
            oClientHeader.BrokerSCID = oPaymentHubConfig.BrokerSCID
            oRequestHeader.ClientHeader = oClientHeader
            Return oRequestHeader
        End Function

        Private Function InitializeAccountUpdaterRequestHeader() As PaymentHub.AccountUpdaterRequestHeader
            Dim oAccountUpdaterRequestHeader As New PaymentHub.AccountUpdaterRequestHeader
            Dim oPaymentHubConfig As NexusProvider.PaymentHubConfig = GetPaymentHubConfig()

            oAccountUpdaterRequestHeader.BrokerSCID = oPaymentHubConfig.BrokerSCID
            Return oAccountUpdaterRequestHeader
        End Function

        Private Function PopulateCustomerDetails(oPaymentItem As NexusProvider.PaymentHubDetails) As PaymentHub.Customer
            Dim oCustomer As New PaymentHub.Customer
            oCustomer.Address = New PaymentHub.CustomerAddress
            oCustomer.Basket = New PaymentHub.CustomerBasket
            oCustomer.DeliveryAddress = New PaymentHub.DeliveryAddress


            oCustomer.Address.Address1 = oPaymentItem.CustomerDetails.Address1
            oCustomer.Address.Address2 = oPaymentItem.CustomerDetails.Address2
            oCustomer.Address.Town = oPaymentItem.CustomerDetails.Town
            oCustomer.Address.County = oPaymentItem.CustomerDetails.County
            oCustomer.Address.Country = oPaymentItem.CustomerDetails.Country
            oCustomer.Address.Postcode = oPaymentItem.CustomerDetails.Postcode

            ''Basket
            oCustomer.Basket.ShippingAmount = 0
            oCustomer.Basket.TotalAmount = oPaymentItem.TransactionAmount
            oCustomer.Basket.VATAmount = 0

            ''DeliveryAddress

            oCustomer.DeliveryAddress.Address1 = oPaymentItem.CustomerDetails.Address1
            oCustomer.DeliveryAddress.Address2 = oPaymentItem.CustomerDetails.Address2
            oCustomer.DeliveryAddress.Town = oPaymentItem.CustomerDetails.Town
            oCustomer.DeliveryAddress.County = oPaymentItem.CustomerDetails.County
            oCustomer.DeliveryAddress.Country = oPaymentItem.CustomerDetails.Country
            oCustomer.DeliveryAddress.Postcode = oPaymentItem.CustomerDetails.Postcode
            oCustomer.DeliveryEdit = False

            ''EMail
            oCustomer.Email = oPaymentItem.CustomerDetails.Email
            oCustomer.Firstname = oPaymentItem.CustomerDetails.Firstname
            oCustomer.Lastname = oPaymentItem.CustomerDetails.Lastname
            Return oCustomer

        End Function

        Public Function GetPayPageURL() As String

            Dim oPaymentHub As PaymentHub.IntegrationService
            Dim oRequest As PaymentHub.TransactionData
            Dim oResponce As PaymentHub.TransactionResponse
            Dim oRequestHeader As PaymentHub.RequestHeader
            Dim sURL As String = ""
            Dim oPaymentHubConfig As NexusProvider.PaymentHubConfig = GetPaymentHubConfig()

            Dim oToken As PaymentHub.Token
            Dim oPaymentItem As NexusProvider.PaymentHubDetails = Session(CNPaymentHubDetails)
            Dim sbLogMessage As StringBuilder
            Try
                sbLogMessage = New StringBuilder
                oRequest = New PaymentHub.TransactionData
                oResponce = New PaymentHub.TransactionResponse
                oToken = New PaymentHub.Token
                oPaymentHub = InitializePaymentHubIntegrationService()
                oRequestHeader = InitializePaymentHubClientHeader()


                ''MessageHeader
                Dim oMessageHeader As New PaymentHub.MessageHeader
                oMessageHeader.TransactionID = Guid.NewGuid().ToString()
                oMessageHeader.MessageDateTime = System.DateTime.Now
                oRequestHeader.MessageHeader = oMessageHeader


                oPaymentItem.TransactionID = oMessageHeader.TransactionID
                oPaymentItem.RequestType = oPaymentItem.RequestType
                ''ClientHeader

                oRequest.Header = oRequestHeader

                Dim oTransactionDataBody As New PaymentHub.TransactionDataBody
                Dim oPaymentProviderData As New PaymentHub.PaymentProviderData

                With oPaymentProviderData
                    .RequestType = oPaymentItem.RequestType
                    .JavascriptEnabled = PaymentHub.JavascriptEnabled.Y
                    Dim oMerchant As New PaymentHub.Merchant
                    oMerchant.MerchantID = oPaymentHubConfig.MerchantID
                    If oPaymentHubConfig.SystemPasscode IsNot Nothing AndAlso oPaymentHubConfig.SystemPasscode <> "" Then
                        oMerchant.SystemPassCode = oPaymentHubConfig.SystemPasscode
                    End If
                    If oPaymentHubConfig.SystemGUID IsNot Nothing AndAlso oPaymentHubConfig.SystemGUID <> "" Then
                        oMerchant.SystemGUID = oPaymentHubConfig.SystemGUID
                    End If
                    If oPaymentHubConfig.AccountPassCode IsNot Nothing And oMerchant IsNot Nothing Then
                        oMerchant.AccountPassCode = oPaymentHubConfig.AccountPassCode
                    End If
                    .Merchant = oMerchant
                    Dim sReturnURL As String = "secure/payment/GetProviderResponseAndRedirect.aspx"
                    .ReturnURL = String.Format("{0}://{1}{2}{3}{4}{5}",
                                                              Context.Request.Url.Scheme,
                                                              Context.Request.Url.Host,
                                                              IIf(Context.Request.Url.Port = 80 Or Context.Request.Url.Port = 443, String.Empty, ":" + Context.Request.Url.Port.ToString),
                                                              IIf(HttpContext.Current.Session.IsCookieless, "(S(" & Session.SessionID.ToString() + "))", String.Empty),
                                                              AppSettings("WebRoot"),
                                                              sReturnURL)
                    Dim oTemplate As New PaymentHub.Template
                    If oPaymentHubConfig.LanguageTemplateID IsNot Nothing AndAlso oPaymentHubConfig.LanguageTemplateID.Trim() <> "" AndAlso oPaymentHubConfig.MerchantTemplateID IsNot Nothing AndAlso oPaymentHubConfig.MerchantTemplateID.Trim() <> "" Then
                        oTemplate.LanguageTemplateID = Convert.ToInt32(oPaymentHubConfig.LanguageTemplateID)
                        oTemplate.MerchantTemplateID = Convert.ToInt32(oPaymentHubConfig.MerchantTemplateID)
                        .Template = oTemplate
                    End If

                    If oPaymentHubConfig.AccountID IsNot Nothing Then
                        .AccountID = oPaymentHubConfig.AccountID
                    End If

                    If oPaymentHubConfig.CaptureMethod IsNot Nothing AndAlso oPaymentHubConfig.CaptureMethod <> "" Then
                        .CaptureMethod = oPaymentHubConfig.CaptureMethod
                    End If

                    .Customer = PopulateCustomerDetails(oPaymentItem)

                    If oPaymentItem.RequestType = PaymentHub.RequestType.TokenRegistration Then
                        .Description = "Card Registeration"
                    Else
                        .Description = "Payment"
                    End If
                    .ProcessingIdentifier = PaymentHub.ProcessingIdentifier.AuthAndCharge
                    .RegisterToken = True
                    .ShowOrderConfirmation = False
                    .HideDeliveryDetails = True
                    .HidePaymentResultSuccess = True
                    .IsRecurring = False


                    If oPaymentHubConfig IsNot Nothing AndAlso oPaymentHubConfig.Customer IsNot Nothing AndAlso oPaymentHubConfig.Customer.Trim() <> "" Then
                        Dim CustomerSpecificDatas() As String
                        Dim CustomerSpecificDataKeyValue() As String

                        Dim oPaymentProviderSpecificData As PaymentHub.SpecificData
                        CustomerSpecificDatas = oPaymentHubConfig.Customer.Split("|")
                        Dim oPaymentProviderSpecificDatas(CustomerSpecificDatas.Length - 1) As PaymentHub.SpecificData
                        For custspeccfic_index As Integer = 0 To CustomerSpecificDatas.Length - 1
                            oPaymentProviderSpecificData = New PaymentHub.SpecificData
                            CustomerSpecificDataKeyValue = CustomerSpecificDatas(custspeccfic_index).Split(",")
                            If CustomerSpecificDataKeyValue.Length = 2 Then
                                oPaymentProviderSpecificData.Key = CustomerSpecificDataKeyValue(0).Trim()
                                oPaymentProviderSpecificData.Value = CustomerSpecificDataKeyValue(1).Trim()
                            End If
                            oPaymentProviderSpecificDatas(custspeccfic_index) = oPaymentProviderSpecificData
                        Next
                        .PaymentProviderSpecificData = oPaymentProviderSpecificDatas
                    End If
                    oToken.TokenExpirationDate = Date.Now.AddYears(5).ToString("ddMMyyyy")
                    .Token = oToken
                    If oPaymentItem.RequestType = PaymentHub.RequestType.TokenRegistration Then
                        .TransactionValue = 0.0
                    Else
                        .TransactionValue = oPaymentItem.TransactionAmount
                    End If

                    .TransactionCurrencyCode = GetCurrencyId(oPaymentItem.TransactionCurrency)
                End With
                oTransactionDataBody.PaymentProviderData = oPaymentProviderData
                oRequest.Body = oTransactionDataBody

                If Logger.IsLoggingEnabled Then
                    Dim swRequest = New StringWriter()
                    Dim writerRequest As New System.Xml.Serialization.XmlSerializer(GetType(PaymentHub.TransactionData))
                    writerRequest.Serialize(New XmlTextWriter(swRequest), oRequest)
                    sbLogMessage.AppendLine("GetPayPageURL" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine(swRequest.ToString() & vbCrLf)
                End If
                oResponce = oPaymentHub.GetPayPageURL(oRequest)
                If Logger.IsLoggingEnabled Then
                    Dim swResponse = New StringWriter()
                    Dim writerResponse = New System.Xml.Serialization.XmlSerializer(GetType(PaymentHub.TransactionResponse))
                    writerResponse.Serialize(New XmlTextWriter(swResponse), oResponce)
                    sbLogMessage.AppendLine("Output:" & vbCrLf)
                    sbLogMessage.AppendLine(swResponse.ToString() & vbCrLf)
                End If
                If sbLogMessage.Length > 0 Then
                    LogMessageEntry(sbLogMessage)
                End If

                With oResponce
                    If .Header.ErrorHeader IsNot Nothing AndAlso .Header.ErrorHeader.ErrorCode.Trim <> "0" Then
                    Else
                        oPaymentItem.IntegrationToken = .Body.IntegrationToken
                        sURL = .Body.URL
                    End If
                End With
            Catch ex As Exception

            Finally
                Session(CNPaymentHubDetails) = oPaymentItem
                oPaymentHub = Nothing
                oPaymentHub = Nothing
                oRequest = Nothing
                oResponce = Nothing
                oRequestHeader = Nothing
                oPaymentHubConfig = Nothing
                oToken = Nothing
                sbLogMessage = Nothing
            End Try
            Return sURL
        End Function

        Public Function GetProviderResponse() As NexusProvider.PaymentHubDetails

            Dim oPaymentHub As PaymentHub.IntegrationService
            Dim oRequest As PaymentHub.ProviderResponseRequest
            Dim oResponce As PaymentHub.ProviderResponse
            Dim oRequestHeader As PaymentHub.RequestHeader
            Dim oPaymentHubDetails As NexusProvider.PaymentHubDetails
            Dim sbLogMessage As StringBuilder
            Try
                sbLogMessage = New StringBuilder
                oRequest = New PaymentHub.ProviderResponseRequest()
                oResponce = New PaymentHub.ProviderResponse()
                oPaymentHub = InitializePaymentHubIntegrationService()
                oRequestHeader = InitializePaymentHubClientHeader()
                oPaymentHubDetails = CType(HttpContext.Current.Session(CNPaymentHubDetails), NexusProvider.PaymentHubDetails)
                ''MessageHeader
                Dim oMessageHeader As New PaymentHub.MessageHeader
                oMessageHeader.TransactionID = oPaymentHubDetails.TransactionID
                oMessageHeader.MessageDateTime = System.DateTime.Now
                oRequestHeader.MessageHeader = oMessageHeader

                oRequest.Header = oRequestHeader

                Dim oProviderResponseBody As New PaymentHub.ProviderResponseRequestBody
                oProviderResponseBody.IntegrationToken = oPaymentHubDetails.IntegrationToken


                oRequest.Body = oProviderResponseBody

                If Logger.IsLoggingEnabled Then
                    Dim swRequest = New StringWriter()
                    Dim writerRequest As New System.Xml.Serialization.XmlSerializer(GetType(PaymentHub.ProviderResponseRequest))
                    writerRequest.Serialize(New XmlTextWriter(swRequest), oRequest)
                    sbLogMessage.AppendLine("GetProviderResponse executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine(swRequest.ToString() & vbCrLf)
                End If

                oResponce = oPaymentHub.GetProviderResponse(oRequest)

                If Logger.IsLoggingEnabled Then
                    Dim swResponse = New StringWriter()
                    Dim writerResponse = New System.Xml.Serialization.XmlSerializer(GetType(PaymentHub.ProviderResponse))
                    writerResponse.Serialize(New XmlTextWriter(swResponse), oResponce)
                    sbLogMessage.AppendLine("Output:" & vbCrLf)
                    sbLogMessage.AppendLine(swResponse.ToString() & vbCrLf)
                End If
                If sbLogMessage.Length > 0 Then
                    LogMessageEntry(sbLogMessage)
                End If

                With oResponce
                    If .Header.ErrorHeader IsNot Nothing AndAlso .Header.ErrorHeader.ErrorCode.Trim <> "0" Then
                        'Process the error object if errors, and throw as a single exception
                        'Throw New NexusException(.Header.ErrorHeader)
                    Else


                        If .Body.PaymentProviderResponseData.TransactionResult = PaymentHub.TransactionResult.Success Then

                            If .Body.PaymentProviderResponseData.ResultDescription = PaymentHub.ResultDescription.Authorised Then
                                'TokenID not availabe for world pay
                                If oResponce.Body.PaymentProviderResponseData.TokenID IsNot Nothing Then
                                    Session("TokenID") = oResponce.Body.PaymentProviderResponseData.TokenID
                                End If
                                Session(CNPaid) = True

                                With .Body.PaymentProviderResponseData
                                    oPaymentHubDetails.AuthCode = .AuthCode
                                    oPaymentHubDetails.CardExpiry = .CardExpiry
                                    If oPaymentHubDetails.CardExpiry.Contains("-") Then
                                        Dim cardexpiryformat(1) As String
                                        cardexpiryformat = oPaymentHubDetails.CardExpiry.Split("-")
                                        If cardexpiryformat(1).Length = 4 Then
                                            cardexpiryformat(1) = cardexpiryformat(1).Substring(2, 2)
                                        End If
                                        oPaymentHubDetails.CardExpiry = cardexpiryformat(0) & "/" & cardexpiryformat(1)
                                    End If

                                    oPaymentHubDetails.CardNumber = .PAN ' IIf(.PAN <> "", .PAN, .SchemeName)

                                    If oPaymentHubDetails.CardNumber Is Nothing OrElse oPaymentHubDetails.CardNumber.Trim() = "" Then
                                        oPaymentHubDetails.CardNumber = "4263 9700 0000 5262"
                                    End If
                                    oPaymentHubDetails.ProviderResponseMessage = .ProviderResponseCode
                                    oPaymentHubDetails.ResultCode = .ResultCode
                                    oPaymentHubDetails.ResultDescription = .ResultDescription
                                    oPaymentHubDetails.SchemeName = .SchemeName
                                    oPaymentHubDetails.TokenExpirationDate = .TokenExpirationDate
                                    oPaymentHubDetails.TokenID = .TokenID
                                    If .TransactionAmount > 0 Then
                                        oPaymentHubDetails.TransactionAmount = .TransactionAmount
                                    End If
                                    Dim oBankDetails As New NexusProvider.BankCollection
                                    Dim oBank As New NexusProvider.Bank
                                End With
                            Else
                                '' Failed 
                                Session(CNPaid) = False
                                oPaymentHubDetails.ResultDescription = PaymentHub.ResultDescription.IncorrectCardDetailsEntered
                            End If
                        Else
                            Session(CNPaid) = False
                            oPaymentHubDetails.ResultDescription = PaymentHub.ResultDescription.IncorrectCardDetailsEntered
                        End If
                    End If
                End With
            Catch
            Finally
                Session(CNPaymentHubDetails) = oPaymentHubDetails
                oPaymentHub = Nothing
                oRequest = Nothing
                oResponce = Nothing
                oRequestHeader = Nothing
                oPaymentHubDetails = Nothing
                sbLogMessage = Nothing
            End Try

            Return oPaymentHubDetails


        End Function

        Private Function GetCurrencyId(CurrencyCode As String) As Integer
            Dim CurrencyId As Integer = 0

            If HttpContext.Current.Cache("CurrencyCode" & "_" & CurrencyCode) Is Nothing Then

                Dim doc As System.Xml.XmlDocument = New System.Xml.XmlDocument()
                doc.Load(CStr(HttpContext.Current.Request.PhysicalApplicationPath & "xml\list_one_Currencies.xml"))
                Try
                    CurrencyId = doc.SelectSingleNode("ISO_4217/CcyTbl/CcyNtry[Ccy='" & CurrencyCode & "']/CcyNbr").InnerText
                    HttpContext.Current.Cache.Insert("CurrencyCode" & "_" & CurrencyCode, CurrencyId,
                                                        Nothing, Now.AddHours(24), TimeSpan.Zero)
                Catch ex As Exception

                End Try
            Else
                CurrencyId = CType(HttpContext.Current.Cache("CurrencyCode" & "_" & CurrencyCode), Integer)

            End If
            Return CurrencyId
        End Function

        Sub ProcessPurchase(ByRef oPaymentHubDetails As NexusProvider.PaymentHubDetails)

            Dim oPaymentHub As PaymentHub.IntegrationService

            Dim oRequest As New PaymentHub.TransactionData
            Dim oResponce As New PaymentHub.ProviderResponse
            Dim oRequestHeader As New PaymentHub.RequestHeader
            Dim oCustomer As PaymentHub.Customer
            Dim oPaymentHubConfig As NexusProvider.PaymentHubConfig = GetPaymentHubConfig()

            Dim sbLogMessage As StringBuilder
            Try
                sbLogMessage = New StringBuilder
                oPaymentHub = InitializePaymentHubIntegrationService()
                oRequestHeader = InitializePaymentHubClientHeader()

                oCustomer = New PaymentHub.Customer
                ''MessageHeader
                Dim oMessageHeader As New PaymentHub.MessageHeader
                oMessageHeader.TransactionID = Guid.NewGuid().ToString()
                oMessageHeader.MessageDateTime = System.DateTime.Now
                oRequestHeader.MessageHeader = oMessageHeader

                oRequest.Header = oRequestHeader

                ''ClientHeader
                Dim oTransactionDataBody As New PaymentHub.TransactionDataBody
                Dim oPaymentProviderData As New PaymentHub.PaymentProviderData

                With oPaymentProviderData
                    .IntegrationToken = oPaymentHubDetails.IntegrationToken
                    .RequestType = PaymentHub.RequestType.Purchase

                    Dim oMerchant As New PaymentHub.Merchant

                    oMerchant.MerchantID = oPaymentHubConfig.MerchantID
                    If oPaymentHubConfig.SystemPasscode IsNot Nothing Then
                        oMerchant.SystemPassCode = oPaymentHubConfig.SystemPasscode
                    End If
                    If oPaymentHubConfig.SystemGUID IsNot Nothing Then
                        oMerchant.SystemGUID = oPaymentHubConfig.SystemGUID
                    End If
                    If oPaymentHubConfig.AccountPassCode IsNot Nothing And oMerchant IsNot Nothing Then
                        oMerchant.AccountPassCode = oPaymentHubConfig.AccountPassCode
                    End If
                    .Merchant = oMerchant

                    If oPaymentHubConfig.AccountID IsNot Nothing Then
                        .AccountID = oPaymentHubConfig.AccountID
                    End If

                    If oPaymentHubConfig.CaptureMethod IsNot Nothing Then
                        .CaptureMethod = oPaymentHubConfig.CaptureMethod
                    End If
                    Dim oTemplate As New PaymentHub.Template
                    If oPaymentHubConfig.LanguageTemplateID IsNot Nothing AndAlso oPaymentHubConfig.LanguageTemplateID.Trim() <> "" AndAlso oPaymentHubConfig.MerchantTemplateID IsNot Nothing AndAlso oPaymentHubConfig.MerchantTemplateID.Trim() <> "" Then
                        oTemplate.LanguageTemplateID = Convert.ToInt32(oPaymentHubConfig.LanguageTemplateID)
                        oTemplate.MerchantTemplateID = Convert.ToInt32(oPaymentHubConfig.MerchantTemplateID)
                        .Template = oTemplate
                    End If
                    .Customer = PopulateCustomerDetails(oPaymentHubDetails)
                    .Description = "Payment"
                    .ProcessingIdentifier = PaymentHub.ProcessingIdentifier.AuthAndCharge
                    .HideDeliveryDetails = True
                    .HidePaymentResultSuccess = True
                    .IsRecurring = False
                    Dim oToken As New PaymentHub.Token

                    oToken.TokenId = oPaymentHubDetails.TokenID
                    .Token = oToken
                    .TransactionValue = oPaymentHubDetails.TransactionAmount
                    .TransactionCurrencyCode = GetCurrencyId(oPaymentHubDetails.TransactionCurrency)


                    If oPaymentHubConfig.TransactionIPAddress IsNot Nothing Then
                        .TransactionIPAddress = oPaymentHubConfig.TransactionIPAddress
                    End If
                End With
                oTransactionDataBody.PaymentProviderData = oPaymentProviderData
                oRequest.Body = oTransactionDataBody

                If Logger.IsLoggingEnabled Then
                    Dim swRequest = New StringWriter()
                    Dim writerRequest As New System.Xml.Serialization.XmlSerializer(GetType(PaymentHub.TransactionData))
                    writerRequest.Serialize(New XmlTextWriter(swRequest), oRequest)
                    sbLogMessage.AppendLine("ProcessPurchase executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine(swRequest.ToString() & vbCrLf)
                End If



                oResponce = oPaymentHub.ProcessPurchase(oRequest)

                If Logger.IsLoggingEnabled Then
                    Dim swResponse = New StringWriter()
                    Dim writerResponse = New System.Xml.Serialization.XmlSerializer(GetType(PaymentHub.ProviderResponse))
                    writerResponse.Serialize(New XmlTextWriter(swResponse), oResponce)
                    sbLogMessage.AppendLine("Output:" & vbCrLf)
                    sbLogMessage.AppendLine(swResponse.ToString() & vbCrLf)
                End If
                If sbLogMessage.Length > 0 Then
                    LogMessageEntry(sbLogMessage)
                End If

                With oResponce
                    If .Header.ErrorHeader IsNot Nothing AndAlso .Header.ErrorHeader.ErrorCode.Trim <> "0" Then
                        oPaymentHubDetails.ErrorCode = .Header.ErrorHeader.ErrorCode
                        oPaymentHubDetails.ErrorDescription = .Header.ErrorHeader.ErrorDescription
                    Else
                        With .Body.PaymentProviderResponseData

                            If .TransactionResult = PaymentHub.TransactionResult.Success Then
                                oPaymentHubDetails.AuthCode = .AuthCode
                                oPaymentHubDetails.CardExpiry = .CardExpiry
                                oPaymentHubDetails.CardNumber = .PAN
                                oPaymentHubDetails.ProviderResponseMessage = .ProviderResponseCode
                                oPaymentHubDetails.ResultCode = .ResultCode
                                oPaymentHubDetails.ResultDescription = .ResultDescription
                                oPaymentHubDetails.SchemeName = .SchemeName
                                oPaymentHubDetails.TokenExpirationDate = .TokenExpirationDate
                                oPaymentHubDetails.TokenID = .TokenID
                                oPaymentHubDetails.TransactionAmount = .TransactionAmount
                            Else
                                '' Failed 
                                oPaymentHubDetails.ResultCode = .ResultCode
                                oPaymentHubDetails.ResultDescription = .ResultDescription

                            End If
                        End With
                    End If
                End With
            Catch ex As Exception
            Finally
                sbLogMessage = Nothing
            End Try
        End Sub

        Sub ProcessRefund(ByRef oPaymentHubDetails As NexusProvider.PaymentHubDetails)

            Dim oPaymentHub As PaymentHub.IntegrationService

            Dim oPaymentHubConfig As NexusProvider.PaymentHubConfig = GetPaymentHubConfig()
            Dim oRequest As New PaymentHub.TransactionData
            Dim oResponce As New PaymentHub.ProviderResponse
            Dim oRequestHeader As New PaymentHub.RequestHeader
            Dim oCustomer As PaymentHub.Customer

            Dim sbLogMessage As StringBuilder
            Try
                sbLogMessage = New StringBuilder
                oPaymentHub = InitializePaymentHubIntegrationService()
                oRequestHeader = InitializePaymentHubClientHeader()

                oCustomer = New PaymentHub.Customer
                ''MessageHeader
                Dim oMessageHeader As New PaymentHub.MessageHeader
                oMessageHeader.TransactionID = Guid.NewGuid().ToString()
                oMessageHeader.MessageDateTime = System.DateTime.Now
                oRequestHeader.MessageHeader = oMessageHeader

                oRequest.Header = oRequestHeader

                ''ClientHeader
                Dim oTransactionDataBody As New PaymentHub.TransactionDataBody
                Dim oPaymentProviderData As New PaymentHub.PaymentProviderData

                With oPaymentProviderData
                    .IntegrationToken = oPaymentHubDetails.IntegrationToken
                    .RequestType = PaymentHub.RequestType.Refund

                    Dim oMerchant As New PaymentHub.Merchant

                    oMerchant.MerchantID = oPaymentHubConfig.MerchantID
                    If oPaymentHubConfig.SystemPasscode IsNot Nothing Then
                        oMerchant.SystemPassCode = oPaymentHubConfig.SystemPasscode
                    End If
                    If oPaymentHubConfig.SystemGUID IsNot Nothing Then
                        oMerchant.SystemGUID = oPaymentHubConfig.SystemGUID
                    End If
                    If oPaymentHubConfig.AccountPassCode IsNot Nothing And oMerchant IsNot Nothing Then
                        oMerchant.AccountPassCode = oPaymentHubConfig.AccountPassCode
                    End If
                    .Merchant = oMerchant

                    If oPaymentHubConfig.AccountID IsNot Nothing Then
                        .AccountID = oPaymentHubConfig.AccountID
                    End If

                    If oPaymentHubConfig.CaptureMethod IsNot Nothing Then
                        .CaptureMethod = oPaymentHubConfig.CaptureMethod
                    End If
                    Dim oTemplate As New PaymentHub.Template
                    If oPaymentHubConfig.LanguageTemplateID IsNot Nothing AndAlso oPaymentHubConfig.LanguageTemplateID.Trim() <> "" AndAlso oPaymentHubConfig.MerchantTemplateID IsNot Nothing AndAlso oPaymentHubConfig.MerchantTemplateID.Trim() <> "" Then
                        oTemplate.LanguageTemplateID = Convert.ToInt32(oPaymentHubConfig.LanguageTemplateID)
                        oTemplate.MerchantTemplateID = Convert.ToInt32(oPaymentHubConfig.MerchantTemplateID)
                        .Template = oTemplate
                    End If
                    .Customer = PopulateCustomerDetails(oPaymentHubDetails)
                    .Description = "Payment"
                    .ProcessingIdentifier = PaymentHub.ProcessingIdentifier.AuthAndCharge
                    .HideDeliveryDetails = True
                    .HidePaymentResultSuccess = True
                    .IsRecurring = False
                    Dim oToken As New PaymentHub.Token

                    oToken.TokenId = oPaymentHubDetails.TokenID
                    .Token = oToken
                    .TransactionValue = oPaymentHubDetails.TransactionAmount
                    .TransactionCurrencyCode = GetCurrencyId(oPaymentHubDetails.TransactionCurrency)

                    If oPaymentHubConfig.TransactionIPAddress IsNot Nothing Then
                        .TransactionIPAddress = oPaymentHubConfig.TransactionIPAddress
                    End If
                End With
                oTransactionDataBody.PaymentProviderData = oPaymentProviderData
                oRequest.Body = oTransactionDataBody

                If Logger.IsLoggingEnabled Then
                    Dim swRequest = New StringWriter()
                    Dim writerRequest As New System.Xml.Serialization.XmlSerializer(GetType(PaymentHub.TransactionData))
                    writerRequest.Serialize(New XmlTextWriter(swRequest), oRequest)
                    sbLogMessage.AppendLine("ProcessRefund executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine(swRequest.ToString() & vbCrLf)
                End If



                oResponce = oPaymentHub.ProcessRefund(oRequest)

                If Logger.IsLoggingEnabled Then
                    Dim swResponse = New StringWriter()
                    Dim writerResponse = New System.Xml.Serialization.XmlSerializer(GetType(PaymentHub.ProviderResponse))
                    writerResponse.Serialize(New XmlTextWriter(swResponse), oResponce)
                    sbLogMessage.AppendLine("Output:" & vbCrLf)
                    sbLogMessage.AppendLine(swResponse.ToString() & vbCrLf)
                End If

                With oResponce
                    If .Header.ErrorHeader IsNot Nothing AndAlso .Header.ErrorHeader.ErrorCode.Trim <> "0" Then
                        oPaymentHubDetails.ErrorCode = .Header.ErrorHeader.ErrorCode
                        oPaymentHubDetails.ErrorDescription = .Header.ErrorHeader.ErrorDescription
                    Else
                        With .Body.PaymentProviderResponseData

                            If .TransactionResult = PaymentHub.TransactionResult.Success Then
                                oPaymentHubDetails.AuthCode = .AuthCode
                                oPaymentHubDetails.CardExpiry = .CardExpiry
                                oPaymentHubDetails.CardNumber = .PAN
                                oPaymentHubDetails.ProviderResponseMessage = .ProviderResponseCode
                                oPaymentHubDetails.ResultCode = .ResultCode
                                oPaymentHubDetails.ResultDescription = .ResultDescription
                                oPaymentHubDetails.SchemeName = .SchemeName
                                oPaymentHubDetails.TokenExpirationDate = .TokenExpirationDate
                                oPaymentHubDetails.TokenID = .TokenID
                                oPaymentHubDetails.TransactionAmount = .TransactionAmount
                            Else
                                '' Failed 
                                oPaymentHubDetails.ResultCode = .ResultCode
                                oPaymentHubDetails.ResultDescription = .ResultDescription

                            End If
                        End With
                    End If
                End With
            Catch ex As Exception
            Finally
                sbLogMessage = Nothing
            End Try
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sbLogMessage"></param>
        ''' <param name="v_Category"></param>
        ''' <param name="v_Priority"></param>
        ''' <param name="v_Severity"></param>
        ''' <remarks></remarks>
        Private Sub LogMessageEntry(ByVal sbLogMessage As StringBuilder, Optional ByVal v_Category As String = NexusProvider.Category.General,
                                    Optional ByVal v_Priority As Integer = NexusProvider.Priority.Normal, Optional ByVal v_Severity As System.Diagnostics.TraceEventType = System.Diagnostics.TraceEventType.Verbose)

            Dim logEntry As New LogEntry()
            logEntry.Categories.Clear()
            logEntry.Categories.Add(v_Category)
            logEntry.Priority = v_Priority
            logEntry.Severity = v_Severity
            logEntry.Message = sbLogMessage.ToString

            Logger.Write(logEntry)

        End Sub
#End Region
    End Class

End Namespace
