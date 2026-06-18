Imports SiriusFS.SAM.Structure
Imports SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.WCF
Imports Internal = SiriusFS.SAM.Structure.BaseImplementationTypes
Imports Sirius.Architecture.ExceptionHandling
Imports Sirius.Architecture.ExceptionHandling.Handler
Imports SiriusFS.SAM.CoreImplementation
Imports Sirius.Architecture.Utility
Imports System.Linq


Partial Public Class PureService

    ''' <summary>  
    ''' This web services method is used to Authorise Payment.
    ''' </summary>  
    ''' <param name="oAuthoriseClaimPaymentRequest">An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.AuthoriseClaimPaymentRequest</param>  
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.AuthoriseClaimPaymentResponseType</returns>  


    Public Function AuthoriseClaimPayment(ByVal oAuthoriseClaimPaymentRequest As AuthoriseClaimPaymentRequestType) As AuthoriseClaimPaymentResponseType Implements IPureClaimService.AuthoriseClaimPayment

        Try
            'Assign appropriate key, i.e.
            'CommonFunctions.CheckAuthority("SAMGRefP")

            Dim sUserName As String = oAuthoriseClaimPaymentRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckSecurityToken(oAuthoriseClaimPaymentRequest.WCFSecurityToken)
            Dim oResponse As New AuthoriseClaimPaymentResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oAuthoriseClaimPaymentRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.AuthoriseClaimPaymentRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.AuthoriseClaimPaymentResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oAuthoriseClaimPaymentRequest.BranchCode

            oImpRequest.ClaimPaymentKey = oAuthoriseClaimPaymentRequest.ClaimPaymentKey
            oImpRequest.Comments = oAuthoriseClaimPaymentRequest.Comments
            oImpRequest.Declined = oAuthoriseClaimPaymentRequest.Declined
            oImpRequest.IsRecommended = oAuthoriseClaimPaymentRequest.IsRecommended
            oImpRequest.RecommendedBy = oAuthoriseClaimPaymentRequest.RecommendedBy
            oImpRequest.LoginUserName = oAuthoriseClaimPaymentRequest.LoginUserName
            oImpRequest.TimeStamp = oAuthoriseClaimPaymentRequest.TimeStamp
            oImpRequest.ShortCode = oAuthoriseClaimPaymentRequest.ShortCode
            oImpRequest.ExclusiveLock = oAuthoriseClaimPaymentRequest.ExclusiveLock
            oImpRequest.SessionValue = oAuthoriseClaimPaymentRequest.SessionValue

            If oAuthoriseClaimPaymentRequest.PaymentCashList IsNot Nothing Then
                CashListWithItemsRequest(oImpRequest, oAuthoriseClaimPaymentRequest)
                oImpRequest.AccountKey = oAuthoriseClaimPaymentRequest.AccountKey
                oImpRequest.AccountKeySpecified = oAuthoriseClaimPaymentRequest.AccountKeySpecified
                oImpRequest.PaymentDate = oAuthoriseClaimPaymentRequest.PaymentDate
                oImpRequest.PaymentDateSpecified = oAuthoriseClaimPaymentRequest.PaymentDateSpecified
                oImpRequest.PaymentDateTo = oAuthoriseClaimPaymentRequest.PaymentDateTo
                oImpRequest.PaymentDateToSpecified = oAuthoriseClaimPaymentRequest.PaymentDateToSpecified
                oImpRequest.SourceArray = oAuthoriseClaimPaymentRequest.SourceArray
            End If

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.AuthoriseClaimPayment(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                oResponse.ErrorMessage = oImpResponse.ErrorMessage
                oResponse.TimeStamp = oImpResponse.TimeStamp

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oAuthoriseClaimPaymentRequest))
            Finally
                oBusiness = Nothing
                oImpRequest = Nothing
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oAuthoriseClaimPaymentRequest))
            Return Nothing

        End Try

    End Function
    ''' <summary>
    ''' CashListWithItemsRequest
    ''' </summary>
    ''' <param name="oImpRequest"></param>
    ''' <param name="oCreatePaymentCashListWithItemsRequest"></param>
    ''' <remarks></remarks>
    Private Sub CashListWithItemsRequest(ByRef oImpRequest As SAMForInsuranceV2ImplementationTypes.AuthoriseClaimPaymentRequestType, ByVal oCreatePaymentCashListWithItemsRequest As BaseAuthoriseClaimPaymentRequestType)

        Dim oCashList As New BaseImplementationTypes.BasePaymentCashListType
        Try

            With oCreatePaymentCashListWithItemsRequest.PaymentCashList
                oCashList.BranchCode = oCreatePaymentCashListWithItemsRequest.BranchCode
                oCashList.Reference = .Reference
                oCashList.TypeCode = "CP"
                oCashList.BankAccountCode = .BankAccountCode
                oCashList.BankAccountName = .bankAccountName
                oCashList.CurrencyCode = .CurrencyCode
                oCashList.ListDate = .ListDate
                oCashList.StatusCode = .StatusCode
                oCashList.SubBranchCode = .SubBranchCode
                oCashList.CashListKey = oCreatePaymentCashListWithItemsRequest.PaymentCashList.cashListKey

                If .PaymentItem IsNot Nothing Then
                    Dim oPaymentItem() As BaseImplementationTypes.BasePaymentCashListItemType
                    ReDim oPaymentItem(.PaymentItem.Count - 1)
                    Dim oItem As BaseImplementationTypes.BasePaymentCashListItemType
                    For cntItems As Integer = 0 To .PaymentItem.Count - 1
                        If .PaymentItem(cntItems) IsNot Nothing Then
                            oItem = New BaseImplementationTypes.BasePaymentCashListItemType
                            With .PaymentItem(cntItems)
                                oItem.StatusCode = "ISS"
                                oItem.TypeCode = "CLP"
                                oItem.TransactionDate = .TransactionDate
                                oItem.MediaTypeCode = .MediaTypeCode
                                oItem.MediaReference = .MediaReference
                                oItem.OurReference = .OurReference
                                oItem.TheirReference = .TheirReference
                                oItem.AccountShortCode = .AccountShortCode
                                oItem.AllocationStatusCode = .AllocationStatusCode
                                oItem.Amount = .Amount
                                oItem.IsProduceDocument = .IsProduceDocument
                                oItem.BankReference = .BankReference
                                oItem.ContactName = .ContactName
                                oItem.FurtherDetails = .FurtherDetails
                                oItem.AllocationDetails = New BaseImplementationTypes.BaseAllocationType
                                oItem.AllocationDetails.AutoAllocate = False
                                oItem.SkipPosting = .SkipPosting
                                oItem.TaxBandCode = .TaxBandCode
                                oItem.TaxAmount = .TaxAmount
                                oItem.CashListItemKey = .CashListItemKey

                                If .ContactAddress IsNot Nothing Then
                                    Dim oContactAddress As New BaseImplementationTypes.BaseSimpleAddressType
                                    With .ContactAddress
                                        oContactAddress.AddressLine1 = .AddressLine1
                                        oContactAddress.AddressLine2 = .AddressLine2
                                        oContactAddress.AddressLine3 = .AddressLine3
                                        oContactAddress.AddressLine4 = .AddressLine4
                                        oContactAddress.PostCode = .PostCode
                                        oContactAddress.CountryCode = .CountryCode
                                    End With
                                    oItem.ContactAddress = oContactAddress
                                End If

                                If (.Bank IsNot Nothing) Then
                                    Dim oBank As New BaseImplementationTypes.BaseBankPaymentType
                                    With .Bank
                                        oBank.PayeeName = GetPaymentName(.PayeeName)
                                        oBank.AccountCode = .AccountCode
                                        oBank.BranchCode = .BranchCode
                                        oBank.ExpiryDateSpecified = .ExpiryDateSpecified
                                        oBank.ExpiryDate = .ExpiryDate
                                        oBank.Reference1 = .Reference1
                                        oBank.Reference2 = .Reference2
                                        oBank.PartyBankKey = .PartyBankKey
                                        oBank.BIC = .BIC
                                        oBank.IBAN = .IBAN
                                    End With
                                    oItem.Bank = oBank
                                End If

                                If (.CreditCard IsNot Nothing) Then
                                    Dim oCreditCard As New BaseImplementationTypes.BaseCreditCardType
                                    With .CreditCard
                                        oCreditCard.Number = .Number
                                        oCreditCard.NameOnCreditCard = .NameOnCreditCard
                                        oCreditCard.ExpiryDate = .ExpiryDate
                                        oCreditCard.StartDate = .StartDate
                                        oCreditCard.Issue = .Issue
                                        oCreditCard.TrackingNumber = .TrackingNumber
                                        oCreditCard.PartyBankKey = .PartyBankKey
                                    End With
                                    oItem.CreditCard = oCreditCard
                                End If
                            End With

                            oPaymentItem(cntItems) = oItem
                            oItem = Nothing
                        End If
                    Next
                    oCashList.PaymentItem = oPaymentItem
                End If
            End With
            oImpRequest.PaymentCashList = oCashList

        Catch ex As Exception

        Finally
            oCashList = Nothing
        End Try
    End Sub


#Region "GetProductDocuments"
    ''' <summary>
    ''' GetProductDocuments
    ''' </summary>
    ''' <param name="oGetProductDocumentsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetProductDocuments(ByVal oGetProductDocumentsRequest As GetProductDocumentsRequestType) As GetProductDocumentsResponseType Implements IPureClaimService.GetProductDocuments
        Try

            Dim sUserName As String = oGetProductDocumentsRequest.LoginUserName
            Dim nAgentKey As Integer
            Dim nUserId As Integer

            CommonFunctions.GetIdentity(sUserName, nAgentKey, nUserId)
            CommonFunctions.CheckAuthority("SAMDOCLINK", nUserId)
            CommonFunctions.CheckSecurityToken(oGetProductDocumentsRequest.WCFSecurityToken)

            'Dim oResponse As New 
            Dim oResponse As New GetProductDocumentsResponseType

            'Dim oBusiness As New SAMForInsuranceV2Business
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetProductDocumentsRequest.BranchCode)
            ' Implementation structures            

            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetProductDocumentsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetProductDocumentsResponseType = Nothing

            oImpRequest.LoginUserName = oGetProductDocumentsRequest.LoginUserName
            oImpRequest.BranchCode = oGetProductDocumentsRequest.BranchCode
            oImpRequest.FunctionalArea = oGetProductDocumentsRequest.FunctionalArea
            oImpRequest.ProductCode = oGetProductDocumentsRequest.ProductCode
            oImpRequest.ProcessTypeKey = oGetProductDocumentsRequest.ProcessTypeKey
            oImpRequest.DocumentTemplateKey = oGetProductDocumentsRequest.DocumentTemplateKey

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetProductDocuments(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.ProductDocuments = SAMFunc.GetDeserializedValues(Of List(Of BaseGetProductDocumentsResponseTypeProductDocumentsRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetProductDocumentsResponseTypeProductDocuments", sConvertToTypeName:="BaseGetProductDocumentsResponseTypeProductDocumentsRow")
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetProductDocumentsRequest))
            Finally
                oBusiness = Nothing
                oImpRequest = Nothing
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetProductDocumentsRequest))
            Return Nothing
        End Try
    End Function
#End Region

#Region "UpdateRecommendStatus"
    ''' <summary>
    ''' UpdateRecommendStatus
    ''' </summary>
    ''' <param name="oUpdateRecommendStatusRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateRecommendStatus(ByVal oUpdateRecommendStatusRequest As UpdateRecommendStatusRequestType) As UpdateRecommendStatusResponseType Implements IPureClaimService.UpdateRecommendStatus
        Try
            Dim sUserName As String = oUpdateRecommendStatusRequest.LoginUserName
            Dim nAgentKey As Integer
            Dim nUserId As Integer

            CommonFunctions.GetIdentity(sUserName, nAgentKey, nUserId)
            CommonFunctions.CheckAuthority("SAMCLMURS", nUserId)
            CommonFunctions.CheckSecurityToken(oUpdateRecommendStatusRequest.WCFSecurityToken)

            Dim oResponse As UpdateRecommendStatusResponseType = Nothing
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oUpdateRecommendStatusRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateRecommendStatusRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateRecommendStatusResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.LoginUserName = oUpdateRecommendStatusRequest.LoginUserName
            oImpRequest.BranchCode = oUpdateRecommendStatusRequest.BranchCode
            oImpRequest.ClaimKey = oUpdateRecommendStatusRequest.ClaimKey
            oImpRequest.Status = oUpdateRecommendStatusRequest.Status
            oImpRequest.TimeStamp() = oUpdateRecommendStatusRequest.TimeStamp()
            Try
                ' Call the implementation method
                oImpResponse = New SAMForInsuranceV2ImplementationTypes.UpdateRecommendStatusResponseType
                oResponse = New UpdateRecommendStatusResponseType
                oImpResponse = oBusiness.UpdateRecommendStatus(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                oResponse.TimeStamp() = oImpResponse.TimeStamp()

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdateRecommendStatusRequest))
            Finally
                oBusiness = Nothing
                oImpRequest = Nothing
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateRecommendStatusRequest))
            Return Nothing
        End Try

    End Function
#End Region


    Public Function BindClaim(ByVal oBindClaimRequest As BindClaimRequestType) As BindClaimResponseType Implements IPureClaimService.BindClaim

        Try
            'CheckAuthority("SAMBindCLM")

            Dim sUserName As String = oBindClaimRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckSecurityToken(oBindClaimRequest.WCFSecurityToken)
            Dim oResponse As New BindClaimResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oBindClaimRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.BindClaimRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.BindClaimResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oBindClaimRequest.BranchCode
            oImpRequest.ClaimKey = oBindClaimRequest.ClaimKey
            oImpRequest.ProcessType = oBindClaimRequest.ProcessType
            oImpRequest.TimeStamp = oBindClaimRequest.TimeStamp
            oImpRequest.BaseCaseKey = oBindClaimRequest.BaseCaseKey
            oImpRequest.SkipSaveTransaction = oBindClaimRequest.SkipSaveTransaction
            oImpRequest.CloseClaimOnZeroReserveRecoveryBalance = oBindClaimRequest.CloseClaimOnZeroReserveRecoveryBalance

            If oBindClaimRequest.ClaimPayment IsNot Nothing Then
                oImpRequest.ClaimPayment = New BaseImplementationTypes.BaseClaimPaymentType
                ClaimPaymentIn(oImpRequest.ClaimPayment, oBindClaimRequest.ClaimPayment, oBindClaimRequest.BranchCode, True)
            ElseIf (oBindClaimRequest.ClaimPerilPayment IsNot Nothing) Then
                oImpRequest.ClaimPerilPayment = New List(Of BaseImplementationTypes.BaseClaimPaymentType)
                For Each ClaimPaymentItem In oBindClaimRequest.ClaimPerilPayment
                    oImpRequest.ClaimPayment = New BaseImplementationTypes.BaseClaimPaymentType
                    ClaimPaymentIn(oImpRequest.ClaimPayment, ClaimPaymentItem, oBindClaimRequest.BranchCode, True)
                    oImpRequest.ClaimPerilPayment.Add(oImpRequest.ClaimPayment)
                    oImpRequest.ClaimPayment = Nothing
                Next
            End If

            oImpRequest.CloseClaimOnZeroReserveRecoveryBalance = oBindClaimRequest.CloseClaimOnZeroReserveRecoveryBalance
            oImpRequest.NoTrans = oBindClaimRequest.NoTrans
            oImpRequest.CloseClaimOnFinalPayment = oBindClaimRequest.CloseClaimOnFinalPayment
            Try

                ' Call the implementation method
                oImpResponse = oBusiness.BindClaim(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Map the response back the structures

                'Retrive values
                oResponse.TimeStamp = oImpResponse.TimeStamp
                oResponse.BaseClaimKey = oImpResponse.BaseClaimKey
                oResponse.ClaimKey = oImpResponse.ClaimKey
                oResponse.ClaimNumber = oImpResponse.ClaimNumber
                oResponse.creditedAccountKey = oImpResponse.creditedAccountKey
                oResponse.creditedDocumentKey = oImpResponse.creditedDocumentKey
                oResponse.creditedTransdetailKey = oImpResponse.creditedTransdetailKey
                oResponse.PaymentAuthorized = oImpResponse.PaymentAuthorized
                oResponse.ResultingStatus = oImpResponse.ResultingStatus

                If (oImpResponse.CashList IsNot Nothing) Then
                    oResponse.CashList = New BaseCashListResponseType
                    oResponse.CashList.CashListKey = oImpResponse.CashList.CashListKey
                End If

                If (oImpResponse.Warnings IsNot Nothing) Then

                    oResponse.Warnings = oImpResponse.Warnings.ToList().ConvertAll(
                         New Converter(Of BaseImplementationTypes.BaseClaimResponseTypeWarnings, BaseClaimResponseTypeWarnings)(AddressOf CommonFunctions.ToServiceClaimWarningTypeList))

                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oBindClaimRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oBindClaimRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>  
    ''' This web services method is used to Calculate Tax For Claims.
    ''' </summary>  
    ''' <param name="CalculateTaxForClaimsRequest">An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.CalculateTaxForClaimsRequest</param>  
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.CalculateTaxForClaimsResponseType</returns>  

    Public Function CalculateTaxForClaims(ByVal CalculateTaxForClaimsRequest As CalculateTaxForClaimsRequestType) As CalculateTaxForClaimsResponseType Implements IPureClaimService.CalculateTaxForClaims
        Try

            Dim sUserName As String = CalculateTaxForClaimsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMPClm", iUserId)
            CommonFunctions.CheckSecurityToken(CalculateTaxForClaimsRequest.WCFSecurityToken)
            Dim oResponse As New CalculateTaxForClaimsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, CalculateTaxForClaimsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.CalculateTaxForClaimsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.CalculateTaxForClaimsResponseType = Nothing
            With oImpRequest
                .BranchCode = CalculateTaxForClaimsRequest.BranchCode
                .Amount = CalculateTaxForClaimsRequest.Amount
                .CompanyCode = CalculateTaxForClaimsRequest.BranchCode
                .CurrencyCode = CalculateTaxForClaimsRequest.CurrencyCode
                .LossCurrencyCode = CalculateTaxForClaimsRequest.LossCurrencyCode
                .TaxGroupCode = CalculateTaxForClaimsRequest.TaxGroupCode
                .PerilId = CalculateTaxForClaimsRequest.PerilId
                .TransactionTypeCode = CalculateTaxForClaimsRequest.TransactionTypeCode
                .ReserveKey = CalculateTaxForClaimsRequest.ReserveKey
                .ReserveType = CalculateTaxForClaimsRequest.ReserveType

                .IsSalvageRecovery = CalculateTaxForClaimsRequest.IsSalvageRecovery
                '----- For ATS Calculation
                If CalculateTaxForClaimsRequest.AdvancedTaxDetails IsNot Nothing AndAlso CalculateTaxForClaimsRequest.AdvancedTaxDetails.AdvancedTaxScriptOptionOn Then
                    .AdvancedTaxDetails = New BaseImplementationTypes.BaseClaimPaymentAdvancedTaxDetailsType
                    .AdvancedTaxDetails.PaymentTo = CalculateTaxForClaimsRequest.AdvancedTaxDetails.PaymentTo
                    .AdvancedTaxDetails.InsuredDomiciled = CalculateTaxForClaimsRequest.AdvancedTaxDetails.InsuredDomiciled
                    .AdvancedTaxDetails.InsuredPercentage = CalculateTaxForClaimsRequest.AdvancedTaxDetails.InsuredPercentage
                    .AdvancedTaxDetails.IsSettlement = CalculateTaxForClaimsRequest.AdvancedTaxDetails.IsSettlement
                    .AdvancedTaxDetails.IsTaxExempt = CalculateTaxForClaimsRequest.AdvancedTaxDetails.IsTaxExempt
                    .AdvancedTaxDetails.IsWHTExempt = CalculateTaxForClaimsRequest.AdvancedTaxDetails.IsWHTExempt
                    .AdvancedTaxDetails.InsuranceTaxNumber = CalculateTaxForClaimsRequest.AdvancedTaxDetails.InsuranceTaxNumber
                    .AdvancedTaxDetails.PayeeDomiciled = CalculateTaxForClaimsRequest.AdvancedTaxDetails.PayeeDomiciled
                    .AdvancedTaxDetails.PayeePercentage = CalculateTaxForClaimsRequest.AdvancedTaxDetails.PayeePercentage
                    .AdvancedTaxDetails.PayeeTaxNumber = CalculateTaxForClaimsRequest.AdvancedTaxDetails.PayeeTaxNumber
                    .AdvancedTaxDetails.SafeHarbourCode = CalculateTaxForClaimsRequest.AdvancedTaxDetails.SafeHarbourCode
                    .AdvancedTaxDetails.SafeHarbourPercentage = CalculateTaxForClaimsRequest.AdvancedTaxDetails.SafeHarbourPercentage
                    .AdvancedTaxDetails.PayeeName = CalculateTaxForClaimsRequest.AdvancedTaxDetails.PayeeName
                    .AdvancedTaxDetails.IsExcess = CalculateTaxForClaimsRequest.AdvancedTaxDetails.IsExcess
                    .AdvancedTaxDetails.AdvancedTaxScriptOptionOn = CalculateTaxForClaimsRequest.AdvancedTaxDetails.AdvancedTaxScriptOptionOn
                End If

                If CalculateTaxForClaimsRequest.ReceiptAdvancedTaxDetails IsNot Nothing AndAlso CalculateTaxForClaimsRequest.ReceiptAdvancedTaxDetails.AdvancedTaxScriptOptionOn Then
                    .ReceiptAdvancedTaxDetails = New BaseImplementationTypes.BaseClaimReceiptAdvancedTaxDetailsType
                    .ReceiptAdvancedTaxDetails.InsuredDomiciled = CalculateTaxForClaimsRequest.ReceiptAdvancedTaxDetails.InsuredDomiciled
                    .ReceiptAdvancedTaxDetails.InsuredPercentage = CalculateTaxForClaimsRequest.ReceiptAdvancedTaxDetails.InsuredPercentage
                    .ReceiptAdvancedTaxDetails.InsuredTaxNumber = CalculateTaxForClaimsRequest.ReceiptAdvancedTaxDetails.InsuredTaxNumber
                    .ReceiptAdvancedTaxDetails.IsTaxExempt = CalculateTaxForClaimsRequest.ReceiptAdvancedTaxDetails.IsTaxExempt
                    .ReceiptAdvancedTaxDetails.ReceivableTaxPercentage = CalculateTaxForClaimsRequest.ReceiptAdvancedTaxDetails.ReceivableTaxPercentage
                    .ReceiptAdvancedTaxDetails.PayeeName = CalculateTaxForClaimsRequest.ReceiptAdvancedTaxDetails.PayeeName
                    .ReceiptAdvancedTaxDetails.AdvancedTaxScriptOptionOn = CalculateTaxForClaimsRequest.ReceiptAdvancedTaxDetails.AdvancedTaxScriptOptionOn
                End If

            End With
            Try
                ' Call the implementation method

                oImpResponse = oBusiness.CalculateTaxforClaims(oImpRequest)
                oResponse.TaxBaseAmount = oImpResponse.TaxBaseAmount
                oResponse.TaxCurrencyAmount = oImpResponse.TaxCurrencyAmount
                oResponse.TaxLossAmount = oImpResponse.TaxLossAmount
                If oImpResponse.TaxItems IsNot Nothing Then
                    oResponse.TaxItems = oImpResponse.TaxItems.ToList.ConvertAll(
                        New Converter(Of BaseImplementationTypes.BaseClaimPaymentTaxItemType, BaseClaimPaymentTaxItemType)(AddressOf CommonFunctions.ToServiceBaseClaimPaymentTaxItemType))
                End If

                If oImpResponse.ReceiptTaxItems IsNot Nothing Then
                    oResponse.ReceiptTaxItems = oImpResponse.ReceiptTaxItems.ToList.ConvertAll(
                        New Converter(Of BaseImplementationTypes.BaseClaimReceiptTaxItemType, BaseClaimReceiptTaxItemType)(AddressOf CommonFunctions.ToServiceBaseClaimReceiptTaxItemType))
                End If

                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(CalculateTaxForClaimsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(CalculateTaxForClaimsRequest))
            Return Nothing
        End Try

    End Function
    ''' <summary>
    ''' get the Unpaid policy transactions
    ''' </summary>
    ''' <param name="CheckUnpaidPremiumRequest"></param>
    ''' <returns></returns>
    Public Function CheckUnpaidPremium(ByVal CheckUnpaidPremiumRequest As CheckUnpaidPremiumRequestType) As CheckUnpaidPremiumResponseType Implements IPureClaimService.CheckUnpaidPremium

        Try
            Dim sUserName As String = CheckUnpaidPremiumRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMCHKUPPR", iUserId)
            CommonFunctions.CheckSecurityToken(CheckUnpaidPremiumRequest.WCFSecurityToken)

            Dim oResponse As New CheckUnpaidPremiumResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, CheckUnpaidPremiumRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.CheckUnpaidPremiumRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.CheckUnpaidPremiumResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = CheckUnpaidPremiumRequest.BranchCode
            oImpRequest.InsuranceRef = CheckUnpaidPremiumRequest.InsuranceRef
            oImpRequest.ClaimNumber = CheckUnpaidPremiumRequest.ClaimNumber
            oImpRequest.WCFSecurityToken = If(CheckUnpaidPremiumRequest.WCFSecurityToken.Length > 0, CheckUnpaidPremiumRequest.WCFSecurityToken, "WCFSecurityToken")

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.CheckUnpaidPremium(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.Transactions = SAMFunc.GetDeserializedValues(Of List(Of BaseCheckUnpaidPremiumResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseCheckUnpaidPremiumResponseTypeTransactions", sConvertToTypeName:="BaseCheckUnpaidPremiumResponseTypeRow")
                If oImpResponse IsNot Nothing Then
                    If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                        oResponse.Transactions = DataTabletoList_CheckUnpaidPremium(oImpResponse.ResultData.Tables(0))
                    End If
                    oResponse.InstalmentOverdue = oImpResponse.InstalmentOverdue
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(CheckUnpaidPremiumRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(CheckUnpaidPremiumRequest))
            Return Nothing
        End Try

    End Function



    Public Function ClaimReceipt(ByVal ClaimReceiptRequest As ClaimReceiptRequestType) As ClaimReceiptResponseType Implements IPureClaimService.ClaimReceipt

        Try
            Dim sUserName As String = ClaimReceiptRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMClmRec", iUserId)
            CommonFunctions.CheckSecurityToken(ClaimReceiptRequest.WCFSecurityToken)

            Dim oResponse As New ClaimReceiptResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, ClaimReceiptRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.ClaimReceiptRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.ClaimReceiptResponseType = Nothing

            ClaimReceiptIn(oImpRequest, ClaimReceiptRequest)

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.ClaimReceipt(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'Retrieve values
                oResponse.BaseClaimKey = oImpResponse.BaseClaimKey
                oResponse.ClaimKey = oImpResponse.ClaimKey
                oResponse.ClaimNumber = oImpResponse.ClaimNumber
                oResponse.Version = oImpResponse.Version
                oResponse.TimeStamp = oImpResponse.TimeStamp

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(ClaimReceiptRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(ClaimReceiptRequest))
            Return Nothing
        End Try

    End Function

    Public Function FindClaim(ByVal FindClaimRequest As FindClaimRequestType) As FindClaimResponseType Implements IPurePolicyService.FindClaim, IPurePartyService.FindClaim, IPureClaimService.FindClaim

        Try


            Dim sUserName As String = FindClaimRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMFClm", iUserId)
            CommonFunctions.CheckSecurityToken(FindClaimRequest.WCFSecurityToken)

            Dim oResponse As New FindClaimResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, FindClaimRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.FindClaimRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.FindClaimResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = FindClaimRequest.BranchCode
            oImpRequest.ClaimNumber = FindClaimRequest.ClaimNumber
            oImpRequest.InsuranceFileRef = FindClaimRequest.InsuranceFileRef
            oImpRequest.ClientShortName = FindClaimRequest.ClientShortName
            oImpRequest.LossDateFrom = FindClaimRequest.LossDateFrom
            oImpRequest.LossDateFromSpecified = FindClaimRequest.LossDateFromSpecified
            oImpRequest.LossDateTo = FindClaimRequest.LossDateTo
            oImpRequest.LossDateToSpecified = FindClaimRequest.LossDateToSpecified
            oImpRequest.RiskIndex = FindClaimRequest.RiskIndex
            oImpRequest.IncludeClosedClaim = FindClaimRequest.IncludeClosedClaim
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.MaxRowsToFetchSpecified = FindClaimRequest.MaxRowsToFetchSpecified
            oImpRequest.RiskKey = FindClaimRequest.RiskKey
            If FindClaimRequest.MaxRowsToFetchSpecified Then
                oImpRequest.MaxRowsToFetch = FindClaimRequest.MaxRowsToFetch
            Else
                oImpRequest.MaxRowsToFetch = -1
            End If
            oImpRequest.TPA = FindClaimRequest.TPA
            oImpRequest.CaseNumberSpecified = FindClaimRequest.CaseNumberSpecified
            If FindClaimRequest.CaseNumberSpecified Then
                oImpRequest.CaseNumber = FindClaimRequest.CaseNumber
            Else
                oImpRequest.CaseNumber = String.Empty
            End If
            oImpRequest.Description = FindClaimRequest.Description
            If FindClaimRequest.RetrieveAssociates Then
                oImpRequest.RetrieveAssociates = FindClaimRequest.RetrieveAssociates
            Else
                oImpRequest.RetrieveAssociates = False
            End If
            oImpRequest.WCFSecurityToken = If(FindClaimRequest.WCFSecurityToken.Length > 0, FindClaimRequest.WCFSecurityToken, "WCFSecurityToken")

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.FindClaim(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.Claims = SAMFunc.GetDeserializedValues(Of List(Of BaseFindClaimResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseFindClaimResponseTypeClaims", sConvertToTypeName:="BaseFindClaimResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Claims = DataTabletoList_FindClaim(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(FindClaimRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(FindClaimRequest))
            Return Nothing
        End Try

    End Function

    Public Function FindInsuranceFileForClaims(ByVal FindInsuranceFileForCLaimsRequest As FindInsuranceFileForClaimsRequestType) As FindInsuranceFileForClaimsResponseType Implements IPureClaimService.FindInsuranceFileForClaims
        Try


            Dim sUserName As String = FindInsuranceFileForCLaimsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMFIFFCLM", iUserId)
            CommonFunctions.CheckSecurityToken(FindInsuranceFileForCLaimsRequest.WCFSecurityToken)
            Dim oResponse As New FindInsuranceFileForClaimsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, FindInsuranceFileForCLaimsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.FindInsuranceFileForClaimsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.FindInsuranceFileForClaimsResponseType = Nothing

            oImpRequest.BranchCode = FindInsuranceFileForCLaimsRequest.BranchCode
            oImpRequest.InForceFrom = FindInsuranceFileForCLaimsRequest.InForceFrom
            oImpRequest.InForceFromSpecified = FindInsuranceFileForCLaimsRequest.InForceFromSpecified
            oImpRequest.InForceTo = FindInsuranceFileForCLaimsRequest.InForceTo
            oImpRequest.InForceToSpecified = FindInsuranceFileForCLaimsRequest.InForceToSpecified
            oImpRequest.InsuranceRef = FindInsuranceFileForCLaimsRequest.InsuranceRef
            oImpRequest.LossDate = FindInsuranceFileForCLaimsRequest.LossDate
            oImpRequest.CoverNoteSheetNumber = FindInsuranceFileForCLaimsRequest.CoverNoteSheetNumber
            oImpRequest.CoverNoteSheetNumberSpecified = FindInsuranceFileForCLaimsRequest.CoverNoteSheetNumberSpecified
            oImpRequest.RiskIndex = FindInsuranceFileForCLaimsRequest.RiskIndex
            oImpRequest.ClientShortName = FindInsuranceFileForCLaimsRequest.ClientShortName
            oImpRequest.PostCode = FindInsuranceFileForCLaimsRequest.PostCode
            oImpRequest.AgentKey = IIf(iAgentKey <> 0, iAgentKey, FindInsuranceFileForCLaimsRequest.LeadAgentKey)

            oImpRequest.MaxRowsToFetchSpecified = FindInsuranceFileForCLaimsRequest.MaxRowsToFetchSpecified
            If FindInsuranceFileForCLaimsRequest.MaxRowsToFetchSpecified Then
                oImpRequest.MaxRowsToFetch = FindInsuranceFileForCLaimsRequest.MaxRowsToFetch
            Else
                oImpRequest.MaxRowsToFetch = -1
            End If
            If FindInsuranceFileForCLaimsRequest.RetrieveAssociates Then
                oImpRequest.RetrieveAssociates = FindInsuranceFileForCLaimsRequest.RetrieveAssociates
            Else
                oImpRequest.RetrieveAssociates = False
            End If
            oImpRequest.WCFSecurityToken = If(FindInsuranceFileForCLaimsRequest.WCFSecurityToken.Length > 0, FindInsuranceFileForCLaimsRequest.WCFSecurityToken, "WCFSecurityToken")


            Try
                ' Call the implementation method
                oImpResponse = oBusiness.FindInsuranceFileForClaimsVersion2(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.InsuranceFileKey = oImpResponse.InsuranceFileKey

                'oResponse.InsuranceFileDetails = SAMFunc.GetDeserializedValues(Of List(Of BaseFindInsuranceFileResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseFindInsuranceFileResponseTypeInsuranceFileDetails", sConvertToTypeName:="BaseFindInsuranceFileResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.InsuranceFileDetails = DataTabletoList_FindInsuranceFile(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(FindInsuranceFileForCLaimsRequest))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(FindInsuranceFileForCLaimsRequest))
            Return Nothing
        End Try
    End Function

    Public Function GenerateClaimsDocuments(ByVal GenerateClaimsDocumentsRequest As GenerateClaimsDocumentsRequestType) As GenerateClaimsDocumentsResponseType Implements IPureClaimService.GenerateClaimsDocuments

        Try
            Dim sUserName As String = GenerateClaimsDocumentsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGenCDoc", iUserId)
            CommonFunctions.CheckSecurityToken(GenerateClaimsDocumentsRequest.WCFSecurityToken)

            Dim oResponse As New GenerateClaimsDocumentsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GenerateClaimsDocumentsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GenerateClaimsDocumentsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GenerateClaimsDocumentsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GenerateClaimsDocumentsRequest.BranchCode
            oImpRequest.Mode = GenerateClaimsDocumentsRequest.Mode
            oImpRequest.OutputAsHTML = GenerateClaimsDocumentsRequest.OutputAsHTML
            oImpRequest.ParameterXML = GenerateClaimsDocumentsRequest.ParameterXML
            oImpRequest.TransactionType = GenerateClaimsDocumentsRequest.TransactionType
            oImpRequest.ClaimKey = GenerateClaimsDocumentsRequest.ClaimKey

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GenerateClaimsDocuments(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                'oResponse.Documents.Row() = New BaseImplementationTypes.BaseGenerateClaimsDocumentsResponseTypeDocumentsRow
                If oImpResponse IsNot Nothing AndAlso oImpResponse.Documents IsNot Nothing AndAlso oImpResponse.Documents.Row IsNot Nothing AndAlso IsArray(oImpResponse.Documents.Row) Then
                    oResponse.Documents = oImpResponse.Documents.Row.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseGenerateClaimsDocumentsResponseTypeDocumentsRow, BaseGenerateClaimsDocumentsResponseTypeRow)(AddressOf CommonFunctions.ToServiceGenerateClaimsDocumentsList))
                End If



            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GenerateClaimsDocumentsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GenerateClaimsDocumentsRequest))
            Return Nothing

        End Try

    End Function

    Public Function GetClaimCoinsurer(ByVal GetClaimCoinsurerRequest As GetClaimCoinsurerRequestType) As GetClaimCoinsurerResponseType Implements IPureClaimService.GetClaimCoinsurer

        Try
            Dim sUserName As String = GetClaimCoinsurerRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGETCOIN", iUserId)
            CommonFunctions.CheckSecurityToken(GetClaimCoinsurerRequest.WCFSecurityToken)

            Dim oResponse As New GetClaimCoinsurerResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetClaimCoinsurerRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetClaimCoinsurerRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetClaimCoinsurerResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetClaimCoinsurerRequest.BranchCode
            oImpRequest.ClaimKey = GetClaimCoinsurerRequest.ClaimKey
            oImpRequest.WCFSecurityToken = If(GetClaimCoinsurerRequest.WCFSecurityToken.Length > 0, GetClaimCoinsurerRequest.WCFSecurityToken, "WCFSecurityToken")
            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetClaimCoinsurer(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.Coinsurers = SAMFunc.GetDeserializedValues(Of List(Of BaseGetClaimCoinsurerResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetClaimCoinsurerResponseTypeCoinsurers", sConvertToTypeName:="BaseGetClaimCoinsurerResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Coinsurers = DataTabletoList_GetClaimCoinsurer(oImpResponse.ResultData.Tables(0))
                End If

                oResponse.ClaimNumber = oImpResponse.ClaimNumber
                oResponse.TotalShare = oImpResponse.TotalShare
                oResponse.TotalCurrentShareValue = oImpResponse.TotalCurrentShareValue

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetClaimCoinsurerRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetClaimCoinsurerRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetClaimDetails(ByVal GetClaimRequest As GetClaimDetailsRequestType) As GetClaimDetailsResponseType Implements IPureClaimService.GetClaimDetails
        Try
            Dim sUserName As String = GetClaimRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGClmDet", iUserId)
            CommonFunctions.CheckSecurityToken(GetClaimRequest.WCFSecurityToken)

            Dim oResponse As New GetClaimDetailsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetClaimRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetClaimDetailsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetClaimDetailsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetClaimRequest.BranchCode
            oImpRequest.ClaimKey = GetClaimRequest.ClaimKey
            oImpRequest.ExclusiveLock = GetClaimRequest.ExclusiveLock
            oImpRequest.SessionValue = GetClaimRequest.SessionValue
            oImpRequest.FetchAllVersionAmounts = GetClaimRequest.FetchAllVersionAmounts
            oImpRequest.IsRoundingUpToFour = GetClaimRequest.IsRoundingUpToFour
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetClaimDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                With oImpResponse.ClaimDetails.ClaimDetails
                    oResponse.TimeStamp = oImpResponse.TimeStamp

                    oResponse.ClaimDetails = New BaseGetClaimDetailsType
                    oResponse.ClaimDetails.ClaimDetails = New BaseGetClaimDetailsTypeClaimDetails

                    oResponse.ClaimDetails.ClaimDetails.BaseClaimKey = oImpResponse.ClaimDetails.ClaimDetails.BaseClaimKey
                    oResponse.ClaimDetails.ClaimDetails.ClaimKey = oImpResponse.ClaimDetails.ClaimDetails.ClaimKey
                    oResponse.ClaimDetails.ClaimDetails.CatastropheCode = oImpResponse.ClaimDetails.ClaimDetails.CatastropheCode
                    oResponse.ClaimDetails.ClaimDetails.ClaimVersionDescription = .ClaimVersionDescription
                    oResponse.ClaimDetails.ClaimDetails.Comments = .Comments
                    oResponse.ClaimDetails.ClaimDetails.GisScreenCode = .GisScreenCode
                    oResponse.ClaimDetails.ClaimDetails.ClaimNumber = .ClaimNumber

                    oResponse.ClaimDetails.ClaimDetails.CurrencyCode = .CurrencyCode
                    oResponse.ClaimDetails.ClaimDetails.Description = .Description
                    oResponse.ClaimDetails.ClaimDetails.HandlerCode = .HandlerCode
                    oResponse.ClaimDetails.ClaimDetails.InfoOnly = .InfoOnly

                    oResponse.ClaimDetails.ClaimDetails.InsuranceFileKey = .InsuranceFileKey
                    oResponse.ClaimDetails.ClaimDetails.InsuranceFolderKey = .InsuranceFolderCnt
                    oResponse.ClaimDetails.ClaimDetails.LikelyClaim = .LikelyClaim
                    oResponse.ClaimDetails.ClaimDetails.Location = .Location
                    oResponse.ClaimDetails.ClaimDetails.LossFromDate = .LossFromDate

                    oResponse.ClaimDetails.ClaimDetails.LossToDate = .LossToDate
                    oResponse.ClaimDetails.ClaimDetails.LossToDateSpecified = True
                    oResponse.ClaimDetails.ClaimDetails.PrimaryCauseCode = .PrimaryCauseCode
                    oResponse.ClaimDetails.ClaimDetails.ProgressStatusCode = .ProgressStatusCode

                    oResponse.ClaimDetails.ClaimDetails.ReportedDate = .ReportedDate
                    oResponse.ClaimDetails.ClaimDetails.RiskKey = .RiskKey

                    oResponse.ClaimDetails.ClaimDetails.SecondaryCauseCode = .SecondaryCauseCode
                    oResponse.ClaimDetails.ClaimDetails.TownCode = .TownCode
                    oResponse.ClaimDetails.ClaimDetails.UnderwritingYearCode = .UnderwritingYearCode

                    oResponse.ClaimDetails.ClaimDetails.ClientEmail = .ClientEmail
                    oResponse.ClaimDetails.ClaimDetails.ClientFaxNo = .ClientFaxNo
                    oResponse.ClaimDetails.ClaimDetails.ClientMobileNo = .ClientMobileNo
                    oResponse.ClaimDetails.ClaimDetails.ClientTelNo = .ClientTelNo
                    oResponse.ClaimDetails.ClaimDetails.ClientTelNoOff = .ClientTelNoOff

                    oResponse.ClaimDetails.ClaimDetails.UserDefFldACode = .UserDefFldACode
                    oResponse.ClaimDetails.ClaimDetails.UserDefFldBCode = .UserDefFldBCode
                    oResponse.ClaimDetails.ClaimDetails.UserDefFldCCode = .UserDefFldCCode
                    oResponse.ClaimDetails.ClaimDetails.UserDefFldDCode = .UserDefFldDCode
                    oResponse.ClaimDetails.ClaimDetails.UserDefFldECode = .UserDefFldECode
                    oResponse.ClaimDetails.ClaimDetails.ClaimVersion = .ClaimVersion
                    oResponse.ClaimDetails.ClaimDetails.ClaimStatus = .ClaimStatus
                    oResponse.ClaimDetails.ClaimDetails.ClaimStatusDate = .ClaimStatusDate
                    oResponse.ClaimDetails.ClaimDetails.LastModifiedDate = .LastModifiedDate
                    oResponse.ClaimDetails.ClaimDetails.TPA = .TPA
                    oResponse.ClaimDetails.ClaimDetails.BaseCaseKey = .BaseCaseKey
                    oResponse.ClaimDetails.ClaimDetails.ClientShortName = .ClientShortName
                    oResponse.ClaimDetails.ClaimDetails.IsPolicyOutstanding = .IsPolicyOutstanding


                    oResponse.ClaimDetails.ClaimDetails.Client = New BaseClaimPartyClientType
                    oResponse.ClaimDetails.ClaimDetails.Client.PartyClaimNumber = .Client.PartyClaimNumber
                    oResponse.ClaimDetails.ClaimDetails.Client.TaxRegistered = .Client.TaxRegistered
                    oResponse.ClaimDetails.ClaimDetails.Client.TaxRegistrationNumber = .Client.TaxRegistrationNumber
                    oResponse.ClaimDetails.ClaimDetails.Client.PartyKey = .Client.PartyKey

                    oResponse.ClaimDetails.ClaimDetails.Client.PartyEmail = .Client.PartyEmail
                    oResponse.ClaimDetails.ClaimDetails.Client.PartyFaxNo = .Client.PartyFaxNo
                    oResponse.ClaimDetails.ClaimDetails.Client.PartyMobileNo = .Client.PartyMobileNo
                    oResponse.ClaimDetails.ClaimDetails.Client.PartyTelNo = .Client.PartyTelNo
                    oResponse.ClaimDetails.ClaimDetails.Client.PartyTelNoOff = .Client.PartyTelNoOff

                    If oImpResponse.ClaimDetails.ClaimDetails.Client.Address IsNot Nothing Then
                        oResponse.ClaimDetails.ClaimDetails.Client.Address = New BaseAddressType
                        oResponse.ClaimDetails.ClaimDetails.Client.Address.AddressLine1 = .Client.Address.AddressLine1
                        oResponse.ClaimDetails.ClaimDetails.Client.Address.AddressLine2 = .Client.Address.AddressLine2
                        oResponse.ClaimDetails.ClaimDetails.Client.Address.AddressLine3 = .Client.Address.AddressLine3
                        oResponse.ClaimDetails.ClaimDetails.Client.Address.AddressLine4 = .Client.Address.AddressLine4
                        oResponse.ClaimDetails.ClaimDetails.Client.Address.AddressTypeCode = CType([Enum].ToObject(GetType(BaseImplementationTypes.AddressTypeType), .Client.Address.AddressTypeCode), AddressTypeType)
                        oResponse.ClaimDetails.ClaimDetails.Client.Address.CountryCode = .Client.Address.CountryCode
                        oResponse.ClaimDetails.ClaimDetails.Client.Address.PostCode = .Client.Address.PostCode
                    End If
                    If oImpResponse.ClaimDetails.ClaimDetails.Client.Contact IsNot Nothing Then

                        oResponse.ClaimDetails.ClaimDetails.Client.Contact = oImpResponse.ClaimDetails.ClaimDetails.Client.Contact.ToList().ConvertAll(
                                                                           New Converter(Of BaseImplementationTypes.BaseContactType, BaseContactType) _
                                                                           (AddressOf CommonFunctions.ToServiceContactType))

                    End If

                    If oImpResponse.ClaimDetails.ClaimDetails.Insurer IsNot Nothing Then
                        oResponse.ClaimDetails.ClaimDetails.Insurer = New BaseClaimPartyInsurerType
                        oResponse.ClaimDetails.ClaimDetails.Insurer.PartyClaimNumber = .Insurer.PartyClaimNumber
                        oResponse.ClaimDetails.ClaimDetails.Insurer.ContactName = .Insurer.ContactName
                        oResponse.ClaimDetails.ClaimDetails.Insurer.InsurerShortName = .Insurer.InsurerShortName
                        oResponse.ClaimDetails.ClaimDetails.Insurer.InsurerName = .InsurerName
                        oResponse.ClaimDetails.ClaimDetails.Insurer.InsurerContact = .Insurer.InsurerContact
                        oResponse.ClaimDetails.ClaimDetails.Insurer.InsurerEmail = .Insurer.InsurerEmail
                        oResponse.ClaimDetails.ClaimDetails.Insurer.InsurerFaxNo = .Insurer.InsurerFaxNo
                        oResponse.ClaimDetails.ClaimDetails.Insurer.InsurerTelNo = .Insurer.InsurerTelNo


                        If oImpResponse.ClaimDetails.ClaimDetails.Insurer.Address IsNot Nothing Then
                            oResponse.ClaimDetails.ClaimDetails.Insurer.Address = New BaseAddressType
                            oResponse.ClaimDetails.ClaimDetails.Insurer.Address.AddressLine1 = .Insurer.Address.AddressLine1
                            oResponse.ClaimDetails.ClaimDetails.Insurer.Address.AddressLine2 = .Insurer.Address.AddressLine2
                            oResponse.ClaimDetails.ClaimDetails.Insurer.Address.AddressLine3 = .Insurer.Address.AddressLine3
                            oResponse.ClaimDetails.ClaimDetails.Insurer.Address.AddressLine4 = .Insurer.Address.AddressLine4
                            oResponse.ClaimDetails.ClaimDetails.Insurer.Address.CountryCode = .Insurer.Address.CountryCode
                            oResponse.ClaimDetails.ClaimDetails.Insurer.Address.PostCode = .Insurer.Address.PostCode
                            oResponse.ClaimDetails.ClaimDetails.Insurer.Address.AddressTypeCode = CType([Enum].ToObject(GetType(BaseImplementationTypes.AddressTypeType), .Insurer.Address.AddressTypeCode), AddressTypeType)
                        End If
                        If oImpResponse.ClaimDetails.ClaimDetails.Insurer.Contact IsNot Nothing Then

                            oResponse.ClaimDetails.ClaimDetails.Insurer.Contact = oImpResponse.ClaimDetails.ClaimDetails.Insurer.Contact.ToList().ConvertAll(
                                                   New Converter(Of BaseImplementationTypes.BaseContactType, BaseContactType) _
                                                   (AddressOf CommonFunctions.ToServiceContactType))
                        End If
                    End If
                End With

                Dim cntPerils As Integer
                Dim cntReserves As Integer
                Dim cntRecoveries As Integer
                Dim cntPay As Integer
                Dim cntPayItem As Integer
                Dim cntRec As Integer
                Dim cntRecItem As Integer
                Dim uBoundPeril As Integer = oImpResponse.ClaimDetails.ClaimPeril.GetUpperBound(0)
                Dim lBoundPeril As Integer = oImpResponse.ClaimDetails.ClaimPeril.GetLowerBound(0)

                'ReDim oResponse.ClaimDetails.ClaimPeril(uBoundPeril)

                If oImpResponse.ClaimDetails.ClaimPeril IsNot Nothing AndAlso oImpResponse.ClaimDetails.ClaimPeril.Count > 0 Then
                    oResponse.ClaimDetails.ClaimPeril = New List(Of BaseGetClaimPerilDetailsType)
                End If
                For cntPerils = lBoundPeril To uBoundPeril
                    Dim oCurrentClaimPerilDetails As New BaseGetClaimPerilDetailsType
                    With oImpResponse.ClaimDetails.ClaimPeril(cntPerils)


                        oCurrentClaimPerilDetails.BaseClaimPerilKey = .BaseClaimPerilKey
                        oCurrentClaimPerilDetails.Comments = .Comments
                        oCurrentClaimPerilDetails.Description = .Description
                        oCurrentClaimPerilDetails.GisScreenCode = .GisScreenCode
                        oCurrentClaimPerilDetails.RIBand = .RIBand
                        oCurrentClaimPerilDetails.SumInsured = .SumInsured
                        oCurrentClaimPerilDetails.TypeCode = .TypeCode
                        oCurrentClaimPerilDetails.ClaimPerilKey = .ClaimPerilKey

                        If Not .Reserve Is Nothing Then
                            Dim lBoundRes As Integer = .Reserve.GetLowerBound(0)
                            Dim uBoundRes As Integer = .Reserve.GetUpperBound(0)

                            oCurrentClaimPerilDetails.Reserve = New List(Of BaseGetClaimReserveDetailsType)

                            For cntReserves = lBoundRes To uBoundRes
                                Dim oCurrentReserve As New BaseGetClaimReserveDetailsType
                                oCurrentReserve.BaseReserveKey = .Reserve(cntReserves).BaseReserveKey
                                oCurrentReserve.InitialReserve = Math.Round(.Reserve(cntReserves).InitialReserve, (IIf(oImpRequest.IsRoundingUpToFour, 4, 2)))
                                oCurrentReserve.PaidAmount = Math.Round(.Reserve(cntReserves).PaidAmount, (IIf(oImpRequest.IsRoundingUpToFour, 4, 2)))
                                oCurrentReserve.RevisedReserve = Math.Round(.Reserve(cntReserves).RevisedReserve, (IIf(oImpRequest.IsRoundingUpToFour, 4, 2)))
                                oCurrentReserve.SumInsured = .Reserve(cntReserves).SumInsured
                                oCurrentReserve.TypeCode = .Reserve(cntReserves).TypeCode
                                oCurrentReserve.IsExcess = .Reserve(cntReserves).IsExcess
                                oCurrentReserve.IsIndemnity = .Reserve(cntReserves).IsIndemnity
                                oCurrentReserve.IsExpense = .Reserve(cntReserves).IsExpense
                                oCurrentReserve.CanDelete = .Reserve(cntReserves).CanDelete
                                oCurrentReserve.TypeDescription = .Reserve(cntReserves).TypeDescription
                                oCurrentReserve.GrossReserve = .Reserve(cntReserves).GrossReserve
                                oCurrentReserve.Tax = .Reserve(cntReserves).Tax
                                oCurrentReserve.RevisedGrossReserve = .Reserve(cntReserves).RevisedGrossReserve
                                oCurrentReserve.RevisedTaxReserve = .Reserve(cntReserves).RevisedTaxReserve
                                oCurrentReserve.PaidToDateTax = .Reserve(cntReserves).PaidToDateTax
                                oCurrentClaimPerilDetails.Reserve.Add(oCurrentReserve)
                                oCurrentReserve = Nothing
                            Next
                        End If

                        If Not .Recovery Is Nothing Then
                            Dim lBoundRec As Integer = .Recovery.GetLowerBound(0)
                            Dim uBoundRec As Integer = .Recovery.GetUpperBound(0)

                            oCurrentClaimPerilDetails.Recovery = New List(Of BaseGetClaimRecoveryDetailsType)

                            For cntRecoveries = lBoundRec To uBoundRec
                                Dim oCurrentRecovery As New BaseGetClaimRecoveryDetailsType
                                oCurrentRecovery.BaseRecoveryKey = .Recovery(cntRecoveries).BaseRecoveryKey
                                oCurrentRecovery.CurrencyCode = .Recovery(cntRecoveries).CurrencyCode
                                oCurrentRecovery.InitialRecovery = Math.Round(.Recovery(cntRecoveries).InitialRecovery, (IIf(oImpRequest.IsRoundingUpToFour, 4, 2)))
                                oCurrentRecovery.ReceiptedAmount = Math.Round(.Recovery(cntRecoveries).ReceiptedAmount, (IIf(oImpRequest.IsRoundingUpToFour, 4, 2)))
                                oCurrentRecovery.ReceiptedTaxAmount = Math.Round(.Recovery(cntRecoveries).ReceiptedTaxAmount, (IIf(oImpRequest.IsRoundingUpToFour, 4, 2)))
                                oCurrentRecovery.RevisedRecovery = Math.Round(.Recovery(cntRecoveries).RevisedRecovery, (IIf(oImpRequest.IsRoundingUpToFour, 4, 2)))
                                oCurrentRecovery.TypeCode = .Recovery(cntRecoveries).TypeCode
                                oCurrentRecovery.IsSalvage = .Recovery(cntRecoveries).IsSalvage
                                oCurrentRecovery.CanDelete = .Recovery(cntRecoveries).CanDelete
                                oCurrentRecovery.ClaimPerilId = .Recovery(cntRecoveries).ClaimPerilId
                                oCurrentClaimPerilDetails.Recovery.Add(oCurrentRecovery)
                                oCurrentRecovery = Nothing
                            Next
                        End If

                        If Not .ClaimPayments Is Nothing Then
                            Dim lBoundPay As Integer = .ClaimPayments.GetLowerBound(0)
                            Dim uBoundPay As Integer = .ClaimPayments.GetUpperBound(0)

                            oCurrentClaimPerilDetails.ClaimPayments = New List(Of BaseGetClaimPaymentDetailsType)

                            For cntPay = lBoundPay To uBoundPay
                                Dim oCurrentClaimPayment As New BaseGetClaimPaymentDetailsType

                                oCurrentClaimPayment.BaseClaimPaymentKey = .ClaimPayments(cntPay).BaseClaimPaymentKey
                                oCurrentClaimPayment.CurrencyCode = .ClaimPayments(cntPay).CurrencyCode
                                oCurrentClaimPayment.IsReferred = .ClaimPayments(cntPay).IsReferred
                                oCurrentClaimPayment.PartyKey = .ClaimPayments(cntPay).PartyKey
                                oCurrentClaimPayment.PaymentAmount = Math.Round(.ClaimPayments(cntPay).PaymentAmount, (IIf(oImpRequest.IsRoundingUpToFour, 4, 2)))
                                oCurrentClaimPayment.PaymentDate = .ClaimPayments(cntPay).PaymentDate
                                oCurrentClaimPayment.PaymentPartyType = .ClaimPayments(cntPay).PaymentPartyType
                                oCurrentClaimPayment.CurrencyDescription = .ClaimPayments(cntPay).CurrencyDescription
                                oCurrentClaimPayment.PartyPaidName = .ClaimPayments(cntPay).PartyPaidName
                                oCurrentClaimPayment.LossAmount = Math.Round(.ClaimPayments(cntPay).LossAmount, (IIf(oImpRequest.IsRoundingUpToFour, 4, 2)))
                                oCurrentClaimPayment.BaseAmount = Math.Round(.ClaimPayments(cntPay).BaseAmount, (IIf(oImpRequest.IsRoundingUpToFour, 4, 2)))
                                oCurrentClaimPayment.PartyPaidCode = .ClaimPayments(cntPay).PartyPaidCode
                                oCurrentClaimPayment.TaxAmount = Math.Round(.ClaimPayments(cntPay).TaxAmount, (IIf(oImpRequest.IsRoundingUpToFour, 4, 2)))
                                'WPR 33-75 ADDED
                                oCurrentClaimPayment.ClaimKey = .ClaimPayments(cntPay).ClaimKey
                                oCurrentClaimPayment.OurRef = .ClaimPayments(cntPay).OurRef
                                oCurrentClaimPayment.UltimatePayee = .ClaimPayments(cntPay).UltimatePayee
                                oCurrentClaimPayment.IsExGratia = .ClaimPayments(cntPay).IsExGratia
                                oCurrentClaimPayment.LossCurrencyCode = .ClaimPayments(cntPay).LossCurrencyCode
                                oCurrentClaimPayment.IsThisPayment = .ClaimPayments(cntPay).IsThisPayment
                                oCurrentClaimPayment.DocumentReference = .ClaimPayments(cntPay).DocumentReference
                                oCurrentClaimPayment.PaymentStatus = .ClaimPayments(cntPay).PaymentStatus
                                oCurrentClaimPayment.TheirReference = .ClaimPayments(cntPay).TheirReference
                                If Not .ClaimPayments(cntPay).AdvancedTaxDetails Is Nothing Then
                                    oCurrentClaimPayment.AdvancedTaxDetails = New BaseClaimPaymentAdvancedTaxDetailsType
                                    oCurrentClaimPayment.AdvancedTaxDetails.InsuranceTaxNumber = .ClaimPayments(cntPay).AdvancedTaxDetails.InsuranceTaxNumber
                                    oCurrentClaimPayment.AdvancedTaxDetails.InsuredDomiciled = .ClaimPayments(cntPay).AdvancedTaxDetails.InsuredDomiciled
                                    oCurrentClaimPayment.AdvancedTaxDetails.InsuredPercentage = .ClaimPayments(cntPay).AdvancedTaxDetails.InsuredPercentage
                                    oCurrentClaimPayment.AdvancedTaxDetails.IsSettlement = .ClaimPayments(cntPay).AdvancedTaxDetails.IsSettlement
                                    oCurrentClaimPayment.AdvancedTaxDetails.IsTaxExempt = .ClaimPayments(cntPay).AdvancedTaxDetails.IsTaxExempt
                                    oCurrentClaimPayment.AdvancedTaxDetails.IsWHTExempt = .ClaimPayments(cntPay).AdvancedTaxDetails.IsWHTExempt
                                    oCurrentClaimPayment.AdvancedTaxDetails.PayeeDomiciled = .ClaimPayments(cntPay).AdvancedTaxDetails.PayeeDomiciled
                                    oCurrentClaimPayment.AdvancedTaxDetails.PayeePercentage = .ClaimPayments(cntPay).AdvancedTaxDetails.PayeePercentage
                                    oCurrentClaimPayment.AdvancedTaxDetails.PayeeTaxNumber = .ClaimPayments(cntPay).AdvancedTaxDetails.PayeeTaxNumber
                                    oCurrentClaimPayment.AdvancedTaxDetails.SafeHarbourCode = .ClaimPayments(cntPay).AdvancedTaxDetails.SafeHarbourCode
                                    oCurrentClaimPayment.AdvancedTaxDetails.SafeHarbourPercentage = Cast.ToDecimal(.ClaimPayments(cntPay).AdvancedTaxDetails.SafeHarbourPercentage, 0)
                                End If

                                If Not .ClaimPayments(cntPay).Payee Is Nothing Then
                                    oCurrentClaimPayment.Payee = New BaseClaimPayeeType
                                    oCurrentClaimPayment.Payee.BankCode = .ClaimPayments(cntPay).Payee.BankCode
                                    oCurrentClaimPayment.Payee.BankName = .ClaimPayments(cntPay).Payee.BankName
                                    oCurrentClaimPayment.Payee.BankNumber = .ClaimPayments(cntPay).Payee.BankNumber
                                    oCurrentClaimPayment.Payee.MediaReference = .ClaimPayments(cntPay).Payee.MediaReference
                                    oCurrentClaimPayment.Payee.MediaTypeDesc = .ClaimPayments(cntPay).Payee.MediaTypeDesc
                                    oCurrentClaimPayment.Payee.MediaTypeCode = .ClaimPayments(cntPay).Payee.MediaTypeCode
                                    oCurrentClaimPayment.Payee.Name = .ClaimPayments(cntPay).Payee.Name
                                    oCurrentClaimPayment.Payee.TheirReference = .ClaimPayments(cntPay).Payee.TheirReference
                                    oCurrentClaimPayment.Payee.PartyBankKey = .ClaimPayments(cntPay).Payee.PartyBankKey
                                    oCurrentClaimPayment.Payee.Comments = .ClaimPayments(cntPay).Payee.Comments
                                    oCurrentClaimPayment.Payee.BIC = .ClaimPayments(cntPay).Payee.BIC
                                    oCurrentClaimPayment.Payee.IBAN = .ClaimPayments(cntPay).Payee.IBAN
                                    oCurrentClaimPayment.Payee.AccountType = .ClaimPayments(cntPay).Payee.AccountType
                                    If .ClaimPayments(cntPay).Payee.Address IsNot Nothing Then
                                        oCurrentClaimPayment.Payee.Address = New BaseAddressType
                                        oCurrentClaimPayment.Payee.Address.AddressLine1 = .ClaimPayments(cntPay).Payee.Address.AddressLine1
                                        oCurrentClaimPayment.Payee.Address.AddressLine2 = .ClaimPayments(cntPay).Payee.Address.AddressLine2
                                        oCurrentClaimPayment.Payee.Address.AddressLine3 = .ClaimPayments(cntPay).Payee.Address.AddressLine3
                                        oCurrentClaimPayment.Payee.Address.AddressLine4 = .ClaimPayments(cntPay).Payee.Address.AddressLine4
                                        oCurrentClaimPayment.Payee.Address.AddressTypeCode = CType([Enum].ToObject(GetType(BaseImplementationTypes.AddressTypeType), .ClaimPayments(cntPay).Payee.Address.AddressTypeCode), AddressTypeType)
                                        oCurrentClaimPayment.Payee.Address.CountryCode = .ClaimPayments(cntPay).Payee.Address.CountryCode
                                        oCurrentClaimPayment.Payee.Address.PostCode = .ClaimPayments(cntPay).Payee.Address.PostCode
                                    End If
                                End If
                                If Not .ClaimPayments(cntPay).ClaimPaymentItems Is Nothing Then
                                    Dim lBoundPayItem As Integer = .ClaimPayments(cntPay).ClaimPaymentItems.GetLowerBound(0)
                                    Dim uBoundPayItem As Integer = .ClaimPayments(cntPay).ClaimPaymentItems.GetUpperBound(0)

                                    oCurrentClaimPayment.ClaimPaymentItems = New List(Of BaseGetClaimPaymentItemDetailsType)

                                    For cntPayItem = lBoundPayItem To uBoundPayItem
                                        Dim oCurrentClaimPaymentItem As New BaseGetClaimPaymentItemDetailsType

                                        If Not .ClaimPayments(cntPay).ClaimPaymentItems(cntPayItem) Is Nothing Then
                                            oCurrentClaimPaymentItem.BaseClaimPaymentItemKey = .ClaimPayments(cntPay).ClaimPaymentItems(cntPayItem).BaseClaimPaymentItemKey
                                            oCurrentClaimPaymentItem.BaseRecoveryKey = .ClaimPayments(cntPay).ClaimPaymentItems(cntPayItem).BaseRecoveryKey
                                            oCurrentClaimPaymentItem.BaseReserveKey = .ClaimPayments(cntPay).ClaimPaymentItems(cntPayItem).BaseReserveKey
                                            oCurrentClaimPaymentItem.PaymentAdjustment = Math.Round(.ClaimPayments(cntPay).ClaimPaymentItems(cntPayItem).PaymentAdjustment, (IIf(oImpRequest.IsRoundingUpToFour, 4, 2)))
                                            oCurrentClaimPaymentItem.PaymentAmount = Math.Round(.ClaimPayments(cntPay).ClaimPaymentItems(cntPayItem).PaymentAmount, (IIf(oImpRequest.IsRoundingUpToFour, 4, 2)))
                                            oCurrentClaimPaymentItem.TaxAmount = Math.Round(.ClaimPayments(cntPay).ClaimPaymentItems(cntPayItem).TaxAmount, (IIf(oImpRequest.IsRoundingUpToFour, 4, 2)))
                                            oCurrentClaimPaymentItem.TotalTaxAmount = Math.Round(.ClaimPayments(cntPay).ClaimPaymentItems(cntPayItem).TotalTaxAmount, (IIf(oImpRequest.IsRoundingUpToFour, 4, 2)))
                                            oCurrentClaimPaymentItem.ReserveKey = .ClaimPayments(cntPay).ClaimPaymentItems(cntPayItem).ReserveKey
                                            oCurrentClaimPaymentItem.TaxGroupCode = .ClaimPayments(cntPay).ClaimPaymentItems(cntPayItem).TaxGroupCode
                                            oCurrentClaimPaymentItem.ThisRevision = .ClaimPayments(cntPay).ClaimPaymentItems(cntPayItem).ThisRevision
                                            oCurrentClaimPaymentItem.LossAmount = .ClaimPayments(cntPay).ClaimPaymentItems(cntPayItem).LossAmount
                                            oCurrentClaimPaymentItem.BaseAmount = .ClaimPayments(cntPay).ClaimPaymentItems(cntPayItem).BaseAmount
                                            oCurrentClaimPaymentItem.LossTaxAmount = .ClaimPayments(cntPay).ClaimPaymentItems(cntPayItem).LossTaxAmount
                                            oCurrentClaimPayment.ClaimPaymentItems.Add(oCurrentClaimPaymentItem)
                                            oCurrentClaimPaymentItem = Nothing
                                        End If
                                    Next
                                End If
                                oCurrentClaimPerilDetails.ClaimPayments.Add(oCurrentClaimPayment)
                                oCurrentClaimPayment = Nothing
                            Next


                        End If

                        If Not .ClaimReceipts Is Nothing Then
                            Dim lBoundRece As Integer = .ClaimReceipts.GetLowerBound(0)
                            Dim uBoundRece As Integer = .ClaimReceipts.GetUpperBound(0)
                            oCurrentClaimPerilDetails.ClaimReceipts = New List(Of BaseGetClaimReceiptDetailsType)

                            For cntRec = lBoundRece To uBoundRece
                                Dim oClaimReciepts As New BaseGetClaimReceiptDetailsType

                                oClaimReciepts.BaseClaimReceiptKey = .ClaimReceipts(cntRec).BaseClaimReceiptKey
                                oClaimReciepts.CurrencyCode = .ClaimReceipts(cntRec).CurrencyCode
                                oClaimReciepts.PartyKey = .ClaimReceipts(cntRec).PartyKey
                                oClaimReciepts.ReceiptAmount = Math.Round(.ClaimReceipts(cntRec).ReceiptAmount, (IIf(oImpRequest.IsRoundingUpToFour, 4, 2)))
                                oClaimReciepts.ReceiptDate = .ClaimReceipts(cntRec).ReceiptDate
                                oClaimReciepts.ReceiptPartyCode = .ClaimReceipts(cntRec).ReceiptPartyCode
                                oClaimReciepts.ClaimReceiptKey = .ClaimReceipts(cntRec).ClaimReceiptKey
                                oClaimReciepts.ReceiptPartyType = .ClaimReceipts(cntRec).ReceiptPartyType

                                If Not .ClaimReceipts(cntRec).AdvancedTax Is Nothing Then
                                    oClaimReciepts.AdvancedTax = New BaseClaimReceiptAdvancedTaxDetailsType
                                    oClaimReciepts.AdvancedTax.InsuredDomiciled = .ClaimReceipts(cntRec).AdvancedTax.InsuredDomiciled
                                    oClaimReciepts.AdvancedTax.InsuredDomiciledSpecified = .ClaimReceipts(cntRec).AdvancedTax.InsuredDomiciledSpecified
                                    oClaimReciepts.AdvancedTax.InsuredPercentage = .ClaimReceipts(cntRec).AdvancedTax.InsuredPercentage
                                    oClaimReciepts.AdvancedTax.InsuredPercentageSpecified = .ClaimReceipts(cntRec).AdvancedTax.InsuredPercentageSpecified
                                    oClaimReciepts.AdvancedTax.InsuredTaxNumber = .ClaimReceipts(cntRec).AdvancedTax.InsuredTaxNumber
                                    oClaimReciepts.AdvancedTax.IsSettlement = .ClaimReceipts(cntRec).AdvancedTax.IsSettlement
                                    oClaimReciepts.AdvancedTax.IsSettlementSpecified = .ClaimReceipts(cntRec).AdvancedTax.IsSettlementSpecified
                                    oClaimReciepts.AdvancedTax.IsTaxExempt = .ClaimReceipts(cntRec).AdvancedTax.IsTaxExempt
                                    oClaimReciepts.AdvancedTax.IsTaxExemptSpecified = .ClaimReceipts(cntRec).AdvancedTax.IsTaxExemptSpecified
                                    oClaimReciepts.AdvancedTax.ReceivableTaxPercentage = .ClaimReceipts(cntRec).AdvancedTax.ReceivableTaxPercentage
                                    oClaimReciepts.AdvancedTax.ReceivableTaxPercentageSpecified = .ClaimReceipts(cntRec).AdvancedTax.ReceivableTaxPercentageSpecified
                                End If

                                If Not .ClaimReceipts(cntRec).Payee Is Nothing Then
                                    oClaimReciepts.Payee = New BaseClaimPayeeType
                                    oClaimReciepts.Payee.BankCode = .ClaimReceipts(cntRec).Payee.BankCode
                                    oClaimReciepts.Payee.BankName = .ClaimReceipts(cntRec).Payee.BankName
                                    oClaimReciepts.Payee.BankNumber = .ClaimReceipts(cntRec).Payee.BankNumber
                                    oClaimReciepts.Payee.MediaReference = .ClaimReceipts(cntRec).Payee.MediaReference
                                    oClaimReciepts.Payee.MediaTypeCode = .ClaimReceipts(cntRec).Payee.MediaTypeCode
                                    oClaimReciepts.Payee.Name = .ClaimReceipts(cntRec).Payee.Name
                                    oClaimReciepts.Payee.TheirReference = .ClaimReceipts(cntRec).Payee.TheirReference
                                    oClaimReciepts.Payee.BIC = .ClaimReceipts(cntRec).Payee.BIC
                                    oClaimReciepts.Payee.IBAN = .ClaimReceipts(cntRec).Payee.IBAN
                                    oClaimReciepts.Payee.Comments = .ClaimReceipts(cntRec).Payee.Comments

                                    oClaimReciepts.Payee.Address = New BaseAddressType
                                    oClaimReciepts.Payee.Address.AddressLine1 = .ClaimReceipts(cntRec).Payee.Address.AddressLine1
                                    oClaimReciepts.Payee.Address.AddressLine2 = .ClaimReceipts(cntRec).Payee.Address.AddressLine2
                                    oClaimReciepts.Payee.Address.AddressLine3 = .ClaimReceipts(cntRec).Payee.Address.AddressLine3
                                    oClaimReciepts.Payee.Address.AddressLine4 = .ClaimReceipts(cntRec).Payee.Address.AddressLine4
                                    oClaimReciepts.Payee.Address.AddressTypeCode = CType([Enum].ToObject(GetType(BaseImplementationTypes.AddressTypeType), .ClaimReceipts(cntRec).Payee.Address.AddressTypeCode), AddressTypeType)
                                    oClaimReciepts.Payee.Address.CountryCode = .ClaimReceipts(cntRec).Payee.Address.CountryCode
                                    oClaimReciepts.Payee.Address.PostCode = .ClaimReceipts(cntRec).Payee.Address.PostCode
                                End If

                                If Not .ClaimReceipts(cntRec).ReceiptItem Is Nothing Then
                                    Dim lBoundRecItem As Integer = .ClaimReceipts(cntRec).ReceiptItem.GetLowerBound(0)
                                    Dim uBoundRecItem As Integer = .ClaimReceipts(cntRec).ReceiptItem.GetUpperBound(0)
                                    oClaimReciepts.ReceiptItem = New List(Of BaseGetClaimReceiptItemDetailsType)
                                    For cntRecItem = lBoundRecItem To uBoundRecItem
                                        Dim oCurrentReceiptItem As New BaseGetClaimReceiptItemDetailsType
                                        oCurrentReceiptItem.BaseClaimReceiptItemKey = .ClaimReceipts(cntRec).ReceiptItem(cntRecItem).BaseClaimReceiptItemKey
                                        oCurrentReceiptItem.BaseRecoveryKey = .ClaimReceipts(cntRec).ReceiptItem(cntRecItem).BaseRecoveryKey
                                        oCurrentReceiptItem.BaseReserveKey = .ClaimReceipts(cntRec).ReceiptItem(cntRecItem).BaseReserveKey
                                        oCurrentReceiptItem.ReceiptAmount = Math.Round(.ClaimReceipts(cntRec).ReceiptItem(cntRecItem).ReceiptAmount, (IIf(oImpRequest.IsRoundingUpToFour, 4, 2)))
                                        oCurrentReceiptItem.TaxAmount = Math.Round(.ClaimReceipts(cntRec).ReceiptItem(cntRecItem).TaxAmount, (IIf(oImpRequest.IsRoundingUpToFour, 4, 2)))
                                        oCurrentReceiptItem.ReceiptBaseAmount = Math.Round(.ClaimReceipts(cntRec).ReceiptItem(cntRecItem).ReceiptBaseAmount, (IIf(oImpRequest.IsRoundingUpToFour, 4, 2)))
                                        oCurrentReceiptItem.ReceiptLossAmount = Math.Round(.ClaimReceipts(cntRec).ReceiptItem(cntRecItem).ReceiptLossAmount, (IIf(oImpRequest.IsRoundingUpToFour, 4, 2)))


                                        oCurrentReceiptItem.RecoveryTypeCode = .ClaimReceipts(cntRec).ReceiptItem(cntRecItem).RecoveryTypeCode
                                        oCurrentReceiptItem.ClaimReceiptItemKey = .ClaimReceipts(cntRec).ReceiptItem(cntRecItem).ClaimReceiptItemKey
                                        oCurrentReceiptItem.TaxGroupCode = .ClaimReceipts(cntRec).ReceiptItem(cntRecItem).TaxGroupCode

                                        oClaimReciepts.ReceiptItem.Add(oCurrentReceiptItem)
                                        oCurrentReceiptItem = Nothing
                                    Next
                                End If
                                oCurrentClaimPerilDetails.ClaimReceipts.Add(oClaimReciepts)
                                oClaimReciepts = Nothing
                            Next

                        End If
                    End With

                    oResponse.ClaimDetails.ClaimPeril.Add(oCurrentClaimPerilDetails)
                Next

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetClaimRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetClaimRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetClaimPartyDetails(ByVal GetClaimPartyDetailsRequest As GetClaimPartyDetailsRequestType) As GetClaimPartyDetailsResponseType Implements IPureClaimService.GetClaimPartyDetails
        Try

            Dim sUserName As String = GetClaimPartyDetailsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGClmDet", iUserId)
            CommonFunctions.CheckSecurityToken(GetClaimPartyDetailsRequest.WCFSecurityToken)

            Dim oResponse As New GetClaimPartyDetailsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetClaimPartyDetailsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetClaimPartyDetailsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetClaimPartyDetailsResponseType = Nothing

            oImpRequest.BranchCode = GetClaimPartyDetailsRequest.BranchCode
            oImpRequest.InsuranceFileKey = GetClaimPartyDetailsRequest.InsuranceFileKey
            oImpRequest.WCFSecurityToken = If(GetClaimPartyDetailsRequest.WCFSecurityToken.Length > 0, GetClaimPartyDetailsRequest.WCFSecurityToken, "WCFSecurityToken")
            ' Pass the values to the implementation request structure

            ' Call the implementation method
            Try
                oImpResponse = oBusiness.GetClaimPartyDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' oResponse.PartyDetails = SAMFunc.GetDeserializedValues(Of List(Of BaseGetClaimPartyDetailsResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataSet, sFromTypeName:="BaseGetClaimPartyDetailsResponseTypePartyDetails", sConvertToTypeName:="BaseGetClaimPartyDetailsResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.PartyDetails = DataTabletoList_GetClaimPartyDetails(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetClaimPartyDetailsRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetClaimPaymentTaxGroups(ByVal oGetClaimPaymentTaxGroupsRequest As GetClaimPaymentTaxGroupsRequestType) As GetClaimPaymentTaxGroupsResponseType Implements IPureClaimService.GetClaimPaymentTaxGroups

        Try



            Dim sUserName As String = oGetClaimPaymentTaxGroupsRequest.LoginUserName
            Dim iAgentKey As Integer = 0
            Dim iUserId As Integer = 0

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGTCLPTG", iUserId)
            CommonFunctions.CheckSecurityToken(oGetClaimPaymentTaxGroupsRequest.WCFSecurityToken)

            Dim oResponse As New GetClaimPaymentTaxGroupsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetClaimPaymentTaxGroupsRequest.BranchCode)

            ' Implementation structures

            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetClaimPaymentTaxGroupsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetClaimPaymentTaxGroupsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetClaimPaymentTaxGroupsRequest.BranchCode

            If oGetClaimPaymentTaxGroupsRequest.AdvancedTax IsNot Nothing Then

                Dim oAdvanceTax As New BaseImplementationTypes.BaseGetClaimPaymentTaxGroupsRequestTypeAdvancedTax

                oAdvanceTax.PayeeDomiciled = oGetClaimPaymentTaxGroupsRequest.AdvancedTax.PayeeDomiciled
                oAdvanceTax.PayeePercentage = oGetClaimPaymentTaxGroupsRequest.AdvancedTax.PayeePercentage
                oAdvanceTax.PayeeTaxNumber = oGetClaimPaymentTaxGroupsRequest.AdvancedTax.PayeeTaxNumber

                oImpRequest.AdvancedTax = oAdvanceTax

            End If

            oImpRequest.PartyKey = oGetClaimPaymentTaxGroupsRequest.PartyKey
            oImpRequest.PaymentPartyType = CType([Enum].ToObject(GetType(ClaimPaymentPartyTypeType), oGetClaimPaymentTaxGroupsRequest.PaymentPartyType), BaseImplementationTypes.ClaimPaymentPartyTypeType)
            oImpRequest.WCFSecurityToken = If(oGetClaimPaymentTaxGroupsRequest.WCFSecurityToken.Length > 0, oGetClaimPaymentTaxGroupsRequest.WCFSecurityToken, "WCFSecurityToken")
            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetClaimPaymentTaxGroups(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.TaxGroup = SAMFunc.GetDeserializedValues(Of List(Of BaseGetClaimPaymentTaxGroupsResponseTypeRow))(elmResultDataSet:=oImpResponse.TaxGroup, sFromTypeName:="BaseGetClaimPaymentTaxGroupsResponseTypeTaxGroup", sConvertToTypeName:="BaseGetClaimPaymentTaxGroupsResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.TaxGroup = DataTabletoList_GetClaimPaymentTaxGroups(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetClaimPaymentTaxGroupsRequest))
            Return Nothing
        End Try

    End Function

    Private Sub ClaimPaymentInForATS(ByVal oImpClaimPayment As BaseImplementationTypes.BaseClaimPaymentType, ByVal oClaimPayment As BaseClaimPaymentType, ByVal sBranchCode As String)

        oImpClaimPayment.BaseClaimKey = oClaimPayment.BaseClaimKey
        oImpClaimPayment.BaseClaimPerilKey = oClaimPayment.BaseClaimPerilKey
        oImpClaimPayment.CurrencyCode = oClaimPayment.CurrencyCode
        oImpClaimPayment.PartyKey = oClaimPayment.PartyKey
        oImpClaimPayment.PaymentPartyType = CType([Enum].ToObject(GetType(ClaimPaymentPartyTypeType), oClaimPayment.PaymentPartyType), BaseImplementationTypes.ClaimPaymentPartyTypeType)
        oImpClaimPayment.PaymentOnly = oClaimPayment.PaymentOnly
        oImpClaimPayment.ClientKey = oClaimPayment.ClientKey
        oImpClaimPayment.TransactionDate = Date.Now()
        oImpClaimPayment.UltimatePayee = oClaimPayment.UltimatePayee

        If oClaimPayment.AdvancedTaxDetails IsNot Nothing Then

            oImpClaimPayment.AdvancedTaxDetails = New BaseImplementationTypes.BaseClaimPaymentAdvancedTaxDetailsType
            oImpClaimPayment.AdvancedTaxDetails.PaymentTo = oClaimPayment.AdvancedTaxDetails.PaymentTo
            oImpClaimPayment.AdvancedTaxDetails.InsuredDomiciled = oClaimPayment.AdvancedTaxDetails.InsuredDomiciled
            oImpClaimPayment.AdvancedTaxDetails.InsuredPercentage = oClaimPayment.AdvancedTaxDetails.InsuredPercentage
            oImpClaimPayment.AdvancedTaxDetails.IsSettlement = oClaimPayment.AdvancedTaxDetails.IsSettlement
            oImpClaimPayment.AdvancedTaxDetails.IsTaxExempt = oClaimPayment.AdvancedTaxDetails.IsTaxExempt
            oImpClaimPayment.AdvancedTaxDetails.IsWHTExempt = oClaimPayment.AdvancedTaxDetails.IsWHTExempt
            oImpClaimPayment.AdvancedTaxDetails.InsuranceTaxNumber = oClaimPayment.AdvancedTaxDetails.InsuranceTaxNumber
            oImpClaimPayment.AdvancedTaxDetails.PayeeDomiciled = oClaimPayment.AdvancedTaxDetails.PayeeDomiciled
            oImpClaimPayment.AdvancedTaxDetails.PayeePercentage = oClaimPayment.AdvancedTaxDetails.PayeePercentage
            oImpClaimPayment.AdvancedTaxDetails.PayeeTaxNumber = oClaimPayment.AdvancedTaxDetails.PayeeTaxNumber
            oImpClaimPayment.AdvancedTaxDetails.SafeHarbourCode = oClaimPayment.AdvancedTaxDetails.SafeHarbourCode
            oImpClaimPayment.AdvancedTaxDetails.SafeHarbourPercentage = oClaimPayment.AdvancedTaxDetails.SafeHarbourPercentage

        End If

    End Sub

    Public Function GetClaimPaymentTaxes(ByVal GetClaimPaymentTaxesRequest As GetClaimPaymentTaxesRequestType) As GetClaimPaymentTaxesResponseType Implements IPureClaimService.GetClaimPaymentTaxes

        Try
            Dim sUserName As String = GetClaimPaymentTaxesRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGClmPT", iUserId)
            CommonFunctions.CheckSecurityToken(GetClaimPaymentTaxesRequest.WCFSecurityToken)

            Dim oResponse As New GetClaimPaymentTaxesResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetClaimPaymentTaxesRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetClaimPaymentTaxesRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetClaimPaymentTaxesResponseType = Nothing

            PayClaimIn(oImpRequest, GetClaimPaymentTaxesRequest, True)

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.PayClaim(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                If oImpResponse IsNot Nothing Then

                    oResponse.Reserves = Nothing

                    If oImpResponse.PaymentItems IsNot Nothing Then

                        oResponse.PaymentItems = oImpResponse.PaymentItems.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseGetClaimPaymentTaxesResponseTypePaymentItems,
                                                        BaseGetClaimPaymentTaxesResponseTypePaymentItems) _
                                                        (AddressOf CommonFunctions.ToServiceBaseGetClaimPaymentTaxesResponseTypePaymentType))
                    End If
                    If oImpResponse.Reserves IsNot Nothing Then
                        oResponse.Reserves = oImpResponse.Reserves.ToList().ConvertAll(
                                                    New Converter(Of BaseImplementationTypes.BaseClaimPerilReservePaymentType,
                                                        BaseClaimPerilReservePaymentType) _
                                                        (AddressOf CommonFunctions.ToServiceBaseClaimPerilReservePaymentType))
                    End If
                    If oImpResponse.TaxItems IsNot Nothing Then
                        oResponse.TaxItems = oImpResponse.TaxItems.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseClaimPaymentTaxItemType,
                                                        BaseClaimPaymentTaxItemType) _
                                                        (AddressOf CommonFunctions.ToServiceBaseClaimPaymentTaxItemType))
                    End If
                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetClaimPaymentTaxesRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetClaimPaymentTaxesRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetClaimPerilSummary(ByVal GetClaimPerilSummaryRequest As GetClaimPerilSummaryRequestType) As GetClaimPerilSummaryResponseType Implements IPureClaimService.GetClaimPerilSummary
        Try

            Dim sUserName As String = GetClaimPerilSummaryRequest.LoginUserName
            Dim iAgentKey As Integer = 0
            Dim iUserId As Integer = 0

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGTCPSUM", iUserId)
            CommonFunctions.CheckSecurityToken(GetClaimPerilSummaryRequest.WCFSecurityToken)

            Dim oResponse As New GetClaimPerilSummaryResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetClaimPerilSummaryRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetClaimPerilSummaryRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetClaimPerilSummaryResponseType = Nothing

            ' Pass values to the implementation request structure
            oImpRequest.BranchCode = GetClaimPerilSummaryRequest.BranchCode
            oImpRequest.ClaimKey = GetClaimPerilSummaryRequest.ClaimKey
            oImpRequest.IncludeTotals = GetClaimPerilSummaryRequest.IncludeTotals
            oImpRequest.IncludeTPRecovery = GetClaimPerilSummaryRequest.IncludeTPRecovery
            oImpRequest.IncludeSalvageRecovery = GetClaimPerilSummaryRequest.IncludeSalvageRecovery
            oImpRequest.IncludeReserveTypes = GetClaimPerilSummaryRequest.IncludeReserveTypes
            oImpRequest.WCFSecurityToken = If(GetClaimPerilSummaryRequest.WCFSecurityToken.Length > 0, GetClaimPerilSummaryRequest.WCFSecurityToken, "WCFSecurityToken")
            ' Call the implementation method
            Try
                oImpResponse = oBusiness.GetClaimPerilSummary(oImpRequest)

                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve values from implementation structure 
                ' and load into response from this funtion 

                ' Retrieve Peril Totals
                'oResponse.PerilTotals = SAMFunc.GetDeserializedValues(Of List(Of BaseGetClaimPerilSummaryResponseTypeRow))(elmResultDataSet:=oImpResponse.PerilTotals, sFromTypeName:="BaseGetClaimPerilSummaryResponseTypePerilTotals", sConvertToTypeName:="BaseGetClaimPerilSummaryResponseTypeRow")
                If oImpResponse.ResultDataPerilTotals IsNot Nothing AndAlso oImpResponse.ResultDataPerilTotals.Tables(0) IsNot Nothing Then
                    oResponse.PerilTotals = DataTabletoList_GetClaimPerilSummary(oImpResponse.ResultDataPerilTotals.Tables(0))
                End If

                ' Retrieve TPRecovery Perils
                'oResponse.TPRecoveryPerils = SAMFunc.GetDeserializedValues(Of List(Of BaseGetClaimPerilSummaryResponseTypeRow1))(elmResultDataSet:=oImpResponse.TPRecoveryPerils, sFromTypeName:="BaseGetClaimPerilSummaryResponseTypeTPRecoveryPerils", sConvertToTypeName:="BaseGetClaimPerilSummaryResponseTypeRow1")
                If oImpResponse.ResultDataTPRecoveryPerils IsNot Nothing AndAlso oImpResponse.ResultDataTPRecoveryPerils.Tables(0) IsNot Nothing Then
                    oResponse.TPRecoveryPerils = DataTabletoList_GetClaimPerilSummary1(oImpResponse.ResultDataTPRecoveryPerils.Tables(0))
                End If

                ' Retrieve Salvage Recovery Perils
                'oResponse.SalvageRecoveryPerils = SAMFunc.GetDeserializedValues(Of List(Of BaseGetClaimPerilSummaryResponseTypeRow2))(elmResultDataSet:=oImpResponse.SalvageRecoveryPerils, sFromTypeName:="BaseGetClaimPerilSummaryResponseTypeSalvageRecoveryPerils", sConvertToTypeName:="BaseGetClaimPerilSummaryResponseTypeRow2")
                If oImpResponse.ResultDataSalvageRecoveryPerils IsNot Nothing AndAlso oImpResponse.ResultDataSalvageRecoveryPerils.Tables(0) IsNot Nothing Then
                    oResponse.SalvageRecoveryPerils = DataTabletoList_GetClaimPerilSummary2(oImpResponse.ResultDataSalvageRecoveryPerils.Tables(0))
                End If


                ' Retrieve Reserve Type Perils
                If oImpResponse.ReserveType IsNot Nothing Then
                    Dim nCount As Integer = oImpResponse.ReserveType.Length
                    Dim iCount As Integer

                    Dim oReserveTypes(0 To nCount - 1) As BaseGetClaimPerilSummaryResponseTypeReserveType
                    For iCount = 0 To nCount - 1
                        oReserveTypes(iCount) = New BaseGetClaimPerilSummaryResponseTypeReserveType
                        With oImpResponse.ReserveType(iCount)
                            oReserveTypes(iCount).Code = .Code
                            oReserveTypes(iCount).Description = .Description

                            'oReserveTypes(iCount).Perils = SAMFunc.GetDeserializedValues(Of List(Of BaseGetClaimPerilSummaryResponseTypeReserveTypeRow))(elmResultDataSet:=.Perils, sFromTypeName:="BaseGetClaimPerilSummaryResponseTypeReserveTypePerils", sConvertToTypeName:="BaseGetClaimPerilSummaryResponseTypeReserveTypeRow")
                            If oImpResponse.ResultDataPerilTotals IsNot Nothing AndAlso oImpResponse.ResultDataPerilTotals.Tables(0) IsNot Nothing Then
                                oReserveTypes(iCount).Perils = DataTabletoList_GetClaimPerilSummaryReserveType(oImpResponse.ResultDataPerilTotals.Tables(0))
                            End If

                        End With
                    Next
                    oResponse.ReserveType = oReserveTypes.ToList

                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetClaimPerilSummaryRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetClaimPerilSummaryRequest))
            Return Nothing
        End Try
    End Function

    Public Function GetClaimReceiptTaxGroups(ByVal oGetClaimReceiptTaxGroupsRequest As GetClaimReceiptTaxGroupsRequestType) As GetClaimReceiptTaxGroupsResponseType Implements IPureClaimService.GetClaimReceiptTaxGroups

        Try



            Dim sUserName As String = oGetClaimReceiptTaxGroupsRequest.LoginUserName
            Dim iAgentKey As Integer = 0
            Dim iUserId As Integer = 0

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGTCLRTG", iUserId)
            CommonFunctions.CheckSecurityToken(oGetClaimReceiptTaxGroupsRequest.WCFSecurityToken)
            Dim oResponse As New GetClaimReceiptTaxGroupsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetClaimReceiptTaxGroupsRequest.BranchCode)

            ' Implementation structures

            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetClaimReceiptTaxGroupsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetClaimReceiptTaxGroupsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetClaimReceiptTaxGroupsRequest.BranchCode
            oImpRequest.TypeCode = oGetClaimReceiptTaxGroupsRequest.TypeCode
            oImpRequest.WCFSecurityToken = If(oGetClaimReceiptTaxGroupsRequest.WCFSecurityToken.Length > 0, oGetClaimReceiptTaxGroupsRequest.WCFSecurityToken, "WCFSecurityToken")
            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetClaimReceiptTaxGroups(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.TaxGroup = SAMFunc.GetDeserializedValues(Of List(Of BaseGetClaimPaymentTaxGroupsResponseTypeRow))(elmResultDataSet:=oImpResponse.TaxGroup, sFromTypeName:="BaseGetClaimPaymentTaxGroupsResponseTypeTaxGroup", sConvertToTypeName:="BaseGetClaimPaymentTaxGroupsResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.TaxGroup = DataTabletoList_GetClaimPaymentTaxGroups(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetClaimReceiptTaxGroupsRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetClaimReceiptTaxes(ByVal GetClaimReceiptTaxesRequest As GetClaimReceiptTaxesRequestType) As GetClaimReceiptTaxesResponseType Implements IPureClaimService.GetClaimReceiptTaxes

        Try
            Dim sUserName As String = GetClaimReceiptTaxesRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGClmRT", iUserId)
            CommonFunctions.CheckSecurityToken(GetClaimReceiptTaxesRequest.WCFSecurityToken)

            Dim oResponse As New GetClaimReceiptTaxesResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetClaimReceiptTaxesRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetClaimReceiptTaxesRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetClaimReceiptTaxesResponseType = Nothing

            ClaimReceiptIn(oImpRequest, GetClaimReceiptTaxesRequest)

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.ClaimReceipt(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.ReceiptToLossExchangeRate = oImpResponse.ReceiptToLossExchangeRate
                oResponse.ReceiptItems = Nothing
                If oImpResponse.ReceiptItems IsNot Nothing Then
                    oResponse.ReceiptItems = oImpResponse.ReceiptItems.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseGetClaimReceiptTaxesResponseTypeReceiptItems, BaseGetClaimReceiptTaxesResponseTypeReceiptItems)(AddressOf CommonFunctions.ToServiceBaseGetClaimReceiptTaxesResponseTypeReceiptItemsType))
                End If

                oResponse.Recoveries = Nothing
                If oImpResponse.Recoveries IsNot Nothing Then
                    oResponse.Recoveries = oImpResponse.Recoveries.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseClaimPerilRecoveryReceiptType, BaseClaimPerilRecoveryReceiptType)(AddressOf CommonFunctions.ToServiceBaseClaimPerilRecoveryReceiptType))
                End If

                oResponse.TaxItems = Nothing
                If oImpResponse.TaxItems IsNot Nothing Then
                    oResponse.TaxItems = oImpResponse.TaxItems.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseClaimReceiptTaxItemType, BaseClaimReceiptTaxItemType)(AddressOf CommonFunctions.ToServiceClaimReceipttaxItemType))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetClaimReceiptTaxesRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetClaimReceiptTaxesRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetClaimRiskLinks(ByVal GetClaimRiskLinksRequest As GetClaimRiskLinksRequestType) As GetClaimRiskLinksResponseType Implements IPureClaimService.GetClaimRiskLinks

        Try
            Dim sUserName As String = GetClaimRiskLinksRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGClmLFR", iUserId)
            CommonFunctions.CheckSecurityToken(GetClaimRiskLinksRequest.WCFSecurityToken)

            Dim oResponse As New GetClaimRiskLinksResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetClaimRiskLinksRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetClaimRiskLinksRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetClaimRiskLinksResponseType = Nothing

            oImpRequest.BranchCode = GetClaimRiskLinksRequest.BranchCode
            oImpRequest.InsuranceFileKey = GetClaimRiskLinksRequest.InsuranceFileKey
            oImpRequest.RiskKey = GetClaimRiskLinksRequest.RiskKey

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetClaimRiskLinks(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.PerilType = oImpResponse.PerilType.ToList().ConvertAll(
                    New Converter(Of BaseImplementationTypes.BaseGetClaimRiskLinksResponseTypePerilType, BaseGetClaimRiskLinksResponseTypePerilType)(AddressOf CommonFunctions.ToServiceGetClaimRiskLinksResponseTypePerilType))

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetClaimRiskLinksRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetClaimRisk(ByVal GetClaimRiskRequest As GetClaimRiskRequestType) As GetClaimRiskResponseType Implements IPureClaimService.GetClaimRisk

        Try
            Dim sUserName As String = GetClaimRiskRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGClmRsk", iUserId)
            CommonFunctions.CheckSecurityToken(GetClaimRiskRequest.WCFSecurityToken)

            Dim oResponse As New GetClaimRiskResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetClaimRiskRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetClaimRiskRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetClaimRiskResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetClaimRiskRequest.BranchCode
            oImpRequest.BaseClaimKey = GetClaimRiskRequest.BaseClaimKey
            oImpRequest.Task = SiriusFS.SAM.Structure.SAMConstants.SAMComponentAction.PMEdit
            oImpRequest.ClaimKey = GetClaimRiskRequest.ClaimKey

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetClaimRisk(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                'oResponse.QuoteTimeStamp = oImpResponse.QuoteTimeStamp
                oResponse.TimeStamp = oImpResponse.TimeStamp
                oResponse.XMLDataSet = SAMFunc.TransformDatasetPBtoSAM(oImpResponse.XMLDataSet)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetClaimRiskRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetClaimRiskRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetClaimRiskReadOnly(ByVal GetClaimRiskReadOnlyRequest As GetClaimRiskReadOnlyRequestType) As GetClaimRiskReadOnlyResponseType Implements IPureClaimService.GetClaimRiskReadOnly

        Try
            Dim sUserName As String = GetClaimRiskReadOnlyRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGClmRsk", iUserId)
            CommonFunctions.CheckSecurityToken(GetClaimRiskReadOnlyRequest.WCFSecurityToken)

            Dim oResponse As New GetClaimRiskReadOnlyResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetClaimRiskReadOnlyRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetClaimRiskRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetClaimRiskResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetClaimRiskReadOnlyRequest.BranchCode
            oImpRequest.BaseClaimKey = GetClaimRiskReadOnlyRequest.BaseClaimKey
            oImpRequest.Task = SiriusFS.SAM.Structure.SAMConstants.SAMComponentAction.PMView
            oImpRequest.IgnoreIsDirty = GetClaimRiskReadOnlyRequest.IgnoreIsDirty

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetClaimRisk(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                'oResponse.QuoteTimeStamp = oImpResponse.QuoteTimeStamp
                oResponse.TimeStamp = oImpResponse.TimeStamp
                oResponse.XMLDataSet = SAMFunc.TransformDatasetPBtoSAM(oImpResponse.XMLDataSet)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetClaimRiskReadOnlyRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetClaimRiskReadOnlyRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' This Web method is used to get the Coinsurance Recoveries details by passing the ClaimPerilKey and IsSalvage as request tye objects
    ''' and also the response object is being passed to the correct messaging format.
    '''</summary>
    '''<param name="oGetRecoveryCoinsuranceRequest" type="GetRecoveryCoinsuranceRequestType"></param>   
    '''<remarks></remarks>

    Public Function GetRecoveryCoinsurance(ByVal oGetRecoveryCoinsuranceRequest As GetRecoveryCoinsuranceRequestType) As GetRecoveryCoinsuranceResponseType Implements IPureClaimService.GetRecoveryCoinsurance

        Try
            Dim sUserName As String = oGetRecoveryCoinsuranceRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGRCOINS", iUserId)
            CommonFunctions.CheckSecurityToken(oGetRecoveryCoinsuranceRequest.WCFSecurityToken)

            Dim oResponse As New GetRecoveryCoinsuranceResponseType
            Dim oServiceImplementation As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetRecoveryCoinsuranceRequest.BranchCode)

            ' Implementation structures
            Dim oImplementationRequest As New SAMForInsuranceV2ImplementationTypes.GetRecoveryCoinsuranceRequestType
            Dim oImplementationResponse As SAMForInsuranceV2ImplementationTypes.GetRecoveryCoinsuranceResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImplementationRequest.BranchCode = oGetRecoveryCoinsuranceRequest.BranchCode
            oImplementationRequest.ClaimPerilKey = oGetRecoveryCoinsuranceRequest.ClaimPerilKey
            oImplementationRequest.IsSalvage = oGetRecoveryCoinsuranceRequest.IsSalvage
            oImplementationRequest.WCFSecurityToken = If(oGetRecoveryCoinsuranceRequest.WCFSecurityToken.Length > 0, oGetRecoveryCoinsuranceRequest.WCFSecurityToken, "WCFSecurityToken")
            Try

                ' Call the implementation method
                oImplementationResponse = oServiceImplementation.GetRecoveryCoinsurance(oImplementationRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImplementationResponse.STSError)

                ' oResponse.Coinsurances = SAMFunc.GetDeserializedValues(Of List(Of BaseGetRecoveryCoinsuranceResponseTypeCoinsurancesRow))(elmResultDataSet:=oImplementationResponse.ResultDataset, sFromTypeName:="BaseGetRecoveryCoinsuranceResponseTypeCoinsurances", sConvertToTypeName:="BaseGetRecoveryCoinsuranceResponseTypeCoinsurancesRow")
                If oImplementationResponse.ResultData IsNot Nothing AndAlso oImplementationResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Coinsurances = DataTabletoList_GetRecoveryCoinsuranceCoinsurances(oImplementationResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetRecoveryCoinsuranceRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' This Web method is used to get all the aithorise payment list by paddding branchcode as request type objects and getting the result as XML
    '''</summary>
    '''<param name="oGetReferredPaymentsRequest" type="GetReferredPaymentsRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetReferredPaymentsResponseType</returns>  
    '''<remarks></remarks>

    Public Function GetReferredPayments(ByVal oGetReferredPaymentsRequest As GetReferredPaymentsRequestType) As GetReferredPaymentsResponseType Implements IPureClaimService.GetReferredPayments

        Try
            'Assign appropriate key, i.e.
            'CheckAuthority("SAMGRefP")'told by rahul not to use check authority

            Dim sUserName As String = oGetReferredPaymentsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckSecurityToken(oGetReferredPaymentsRequest.WCFSecurityToken)
            Dim oResponse As New GetReferredPaymentsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetReferredPaymentsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetReferredPaymentsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetReferredPaymentsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetReferredPaymentsRequest.BranchCode

            oImpRequest.ClaimNumber = oGetReferredPaymentsRequest.ClaimNumber
            oImpRequest.PolicyNumber = oGetReferredPaymentsRequest.PolicyNumber
            oImpRequest.PartyKey = oGetReferredPaymentsRequest.PartyKey
            oImpRequest.PartyKeySpecified = oGetReferredPaymentsRequest.PartyKeySpecified
            oImpRequest.DateOfPayment = oGetReferredPaymentsRequest.DateOfPayment
            oImpRequest.DateOfPaymentSpecified = oGetReferredPaymentsRequest.DateOfPaymentSpecified
            If oGetReferredPaymentsRequest.UserCode IsNot Nothing Then
                oImpRequest.UserCode = oGetReferredPaymentsRequest.UserCode
            End If
            oImpRequest.ReferredPaymentsBranchCode = oGetReferredPaymentsRequest.ReferredPaymentsBranchCode
            oImpRequest.ClientShortName = oGetReferredPaymentsRequest.ClientShortName
            oImpRequest.CaseNumber = oGetReferredPaymentsRequest.CaseNumber
            oImpRequest.PayeeName = oGetReferredPaymentsRequest.PayeeName
            oImpRequest.WCFSecurityToken = If(oGetReferredPaymentsRequest.WCFSecurityToken.Length > 0, oGetReferredPaymentsRequest.WCFSecurityToken, "WCFSecurityToken")
            oImpRequest.IsReferredForRecommendation = oGetReferredPaymentsRequest.IsReferredForRecommendation
            oImpRequest.RecommendedBy = oGetReferredPaymentsRequest.RecommendedBy
            oImpRequest.CurrencyCode = oGetReferredPaymentsRequest.CurrencyCode
            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetReferredPayments(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                'oResponse.CashListItems = SAMFunc.GetDeserializedValues(Of List(Of BaseGetReferredPaymentsResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetReferredPaymentsResponseTypeCashListItems", sConvertToTypeName:="BaseGetReferredPaymentsResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.CashListItems = DataTabletoList_GetReferredPayments(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetReferredPaymentsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetReferredPaymentsRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' This   web service  method  for GetTaxGroupsForClaims
    '''<param name="oGetTaxGroupsForClaimsRequest" type="GetTaxGroupsForClaimsRequestType"></param>   
    '''<returns>GetTaxGroupsForClaimsResponseType</returns>
    '''</summary>
    '''<remarks></remarks> 
    Public Function GetTaxGroupsForClaims(ByVal oGetTaxGroupsForClaimsRequest As GetTaxGroupsForClaimsRequestType) As GetTaxGroupsForClaimsResponseType Implements IPureClaimService.GetTaxGroupsForClaims

        Try

            Dim sUserName As String = oGetTaxGroupsForClaimsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGClmDet", iUserId)
            CommonFunctions.CheckSecurityToken(oGetTaxGroupsForClaimsRequest.WCFSecurityToken)
            Dim oResponse As New GetTaxGroupsForClaimsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetTaxGroupsForClaimsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetTaxGroupsForClaimsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetTaxGroupsForClaimsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetTaxGroupsForClaimsRequest.BranchCode
            oImpRequest.Is_withholding_tax = oGetTaxGroupsForClaimsRequest.Is_withholding_tax
            oImpRequest.TransactionTypeCode = oGetTaxGroupsForClaimsRequest.TransactionTypeCode
            oImpRequest.WCFSecurityToken = If(oGetTaxGroupsForClaimsRequest.WCFSecurityToken.Length > 0, oGetTaxGroupsForClaimsRequest.WCFSecurityToken, "WCFSecurityToken")
            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetTaxGroupsForClaims(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                ' oResponse.TaxGroups = SAMFunc.GetDeserializedValues(Of List(Of BaseGetTaxGroupsForClaimsResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetTaxGroupsForClaimsResponseTypeTaxGroups", sConvertToTypeName:="BaseGetTaxGroupsForClaimsResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.TaxGroups = DataTabletoList_GetTaxGroupsForClaims(oImpResponse.ResultData.Tables(0))
                End If
            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetTaxGroupsForClaimsRequest))
            Return Nothing
        End Try
    End Function

    Public Function GetUnallocatedClaimPayment(ByVal GetUnallocatedClaimPaymentsRequest As GetUnallocatedClaimPaymentsRequestType) As GetUnallocatedClaimPaymentsResponseType Implements IPureClaimService.GetUnallocatedClaimPayments

        Try

            Dim sUserName As String = GetUnallocatedClaimPaymentsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMPClm", iUserId)
            CommonFunctions.CheckSecurityToken(GetUnallocatedClaimPaymentsRequest.WCFSecurityToken)
            Dim oResponse As New GetUnallocatedClaimPaymentsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetUnallocatedClaimPaymentsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetUnallocatedClaimPaymentsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetUnallocatedClaimPaymentsResponseType = Nothing

            ' Pass the values to the implementation request structure, i.e.
            oImpRequest.BranchCode = GetUnallocatedClaimPaymentsRequest.BranchCode
            oImpRequest.AccountKeySpecified = GetUnallocatedClaimPaymentsRequest.AccountKeySpecified
            oImpRequest.AccountKey = GetUnallocatedClaimPaymentsRequest.AccountKey
            oImpRequest.PaymentDateSpecified = GetUnallocatedClaimPaymentsRequest.PaymentDateSpecified
            oImpRequest.PaymentDate = GetUnallocatedClaimPaymentsRequest.PaymentDate
            oImpRequest.ShortCodeSpecified = True
            oImpRequest.ShortCode = GetUnallocatedClaimPaymentsRequest.ShortCode
            oImpRequest.PaymentDateToSpecified = GetUnallocatedClaimPaymentsRequest.PaymentDateToSpecified
            oImpRequest.PaymentDateTo = GetUnallocatedClaimPaymentsRequest.PaymentDateTo
            oImpRequest.WCFSecurityToken = If(GetUnallocatedClaimPaymentsRequest.WCFSecurityToken.Length > 0, GetUnallocatedClaimPaymentsRequest.WCFSecurityToken, "WCFSecurityToken")
            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetUnallocatedClaimPayments(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.UnallocatedClaimPayments = SAMFunc.GetDeserializedValues(Of List(Of BaseGetUnallocatedClaimPaymentsResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetUnallocatedClaimPaymentsResponseTypeUnallocatedClaimPayments", sConvertToTypeName:="BaseGetUnallocatedClaimPaymentsResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.UnallocatedClaimPayments = DataTabletoList_GetUnallocatedClaimPayments(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetUnallocatedClaimPaymentsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetUnallocatedClaimPaymentsRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetValidPrimaryCauses(ByVal GetValidPrimaryCausesRequest As GetValidPrimaryCausesRequestType) As GetValidPrimaryCausesResponseType Implements IPureClaimService.GetValidPrimaryCauses

        Try

            Dim sUserName As String = GetValidPrimaryCausesRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGCurByB", iUserId)
            CommonFunctions.CheckSecurityToken(GetValidPrimaryCausesRequest.WCFSecurityToken)
            Dim oResponse As New GetValidPrimaryCausesResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetValidPrimaryCausesRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetValidPrimaryCausesRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetValidPrimaryCausesResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = GetValidPrimaryCausesRequest.BranchCode
            oImpRequest.InsuranceFileKey = GetValidPrimaryCausesRequest.InsuranceFileKey
            oImpRequest.UserName = sUserName
            oImpRequest.Mode = GetValidPrimaryCausesRequest.Mode
            oImpRequest.ModeSpecified = GetValidPrimaryCausesRequest.ModeSpecified
            oImpRequest.WCFSecurityToken = If(GetValidPrimaryCausesRequest.WCFSecurityToken.Length > 0, GetValidPrimaryCausesRequest.WCFSecurityToken, "WCFSecurityToken")
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetValidPrimaryCauses(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.PrimaryCauses = SAMFunc.GetDeserializedValues(Of List(Of BaseGetValidPrimaryCausesResponseTypeRow))(elmResultDataSet:=oImpResponse.PrimaryCauses, sFromTypeName:="BaseGetValidPrimaryCausesResponseTypePrimaryCauses", sConvertToTypeName:="BaseGetValidPrimaryCausesResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.PrimaryCauses = DataTabletoList_GetValidPrimaryCauses(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetValidPrimaryCausesRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetValidPrimaryCausesRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetVersionsForClaim(ByVal GetVersionsForClaimRequest As GetVersionsForClaimRequestType) As GetVersionsForClaimResponseType Implements IPureClaimService.GetVersionsForClaim

        Try
            Dim sUserName As String = GetVersionsForClaimRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGETCLVR", iUserId)
            CommonFunctions.CheckSecurityToken(GetVersionsForClaimRequest.WCFSecurityToken)

            Dim oResponse As New GetVersionsForClaimResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetVersionsForClaimRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetVersionsForClaimRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetVersionsForClaimResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetVersionsForClaimRequest.BranchCode
            oImpRequest.ClaimKey = GetVersionsForClaimRequest.Claim_Number
            oImpRequest.WCFSecurityToken = If(GetVersionsForClaimRequest.WCFSecurityToken.Length > 0, GetVersionsForClaimRequest.WCFSecurityToken, "WCFSecurityToken")
            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetVersionsForClaim(oImpRequest)
                oResponse.IsPreviouslyLocked = oImpResponse.IsPreviouslyLocked

                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' oResponse.Versions = SAMFunc.GetDeserializedValues(Of List(Of BaseGetVersionsForClaimResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetVersionsForClaimResponseTypeVersions", sConvertToTypeName:="BaseGetVersionsForClaimResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Versions = DataTabletoList_GetVersionsForClaim(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetVersionsForClaimRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetVersionsForClaimRequest))
            Return Nothing
        End Try

    End Function

    Public Function MaintainClaim(ByVal MaintainClaimRequest As MaintainClaimRequestType) As MaintainClaimResponseType Implements IPureClaimService.MaintainClaim

        Try
            Dim sUserName As String = MaintainClaimRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMMClm", iUserId)
            CommonFunctions.CheckSecurityToken(MaintainClaimRequest.WCFSecurityToken)

            Dim oResponse As New MaintainClaimResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, MaintainClaimRequest.BranchCode)

            '' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.MaintainClaimRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.MaintainClaimResponseType = Nothing

            Dim oImpClaim As BaseImplementationTypes.BaseClaimMaintainType = New BaseImplementationTypes.BaseClaimMaintainType
            Dim oImpClient As BaseImplementationTypes.BaseClaimPartyClientType = New BaseImplementationTypes.BaseClaimPartyClientType
            Dim oImpInsurer As BaseImplementationTypes.BaseClaimPartyInsurerType = New BaseImplementationTypes.BaseClaimPartyInsurerType

            oImpClaim.IgnoreWarnings = MaintainClaimRequest.Claim.IgnoreWarnings
            oImpClaim.ExternalHandler = MaintainClaimRequest.Claim.ExternalHandler

            oImpClaim.CloseClaimOnZeroReserveRecoveryBalance = MaintainClaimRequest.Claim.CloseClaimOnZeroReserveRecoveryBalance

            oImpClaim.BaseClaimKey = MaintainClaimRequest.Claim.BaseClaimKey
            oImpClaim.CatastropheCode = MaintainClaimRequest.Claim.CatastropheCode
            oImpClaim.ClaimVersionDescription = MaintainClaimRequest.Claim.ClaimVersionDescription
            oImpClaim.Comments = MaintainClaimRequest.Claim.Comments
            oImpClaim.Description = MaintainClaimRequest.Claim.Description
            oImpClaim.HandlerCode = MaintainClaimRequest.Claim.HandlerCode
            oImpClaim.InfoOnly = MaintainClaimRequest.Claim.InfoOnly
            oImpClaim.LikelyClaim = MaintainClaimRequest.Claim.LikelyClaim
            oImpClaim.Location = MaintainClaimRequest.Claim.Location
            oImpClaim.LossFromDate = MaintainClaimRequest.Claim.LossFromDate

            oImpClaim.LossToDateSpecified = True

            If MaintainClaimRequest.Claim.LossToDateSpecified Then
                oImpClaim.LossToDate = MaintainClaimRequest.Claim.LossToDate
            Else
                oImpClaim.LossToDate = MaintainClaimRequest.Claim.LossFromDate
            End If

            oImpClaim.PrimaryCauseCode = MaintainClaimRequest.Claim.PrimaryCauseCode
            oImpClaim.ProgressStatusCode = MaintainClaimRequest.Claim.ProgressStatusCode
            oImpClaim.ReportedDate = MaintainClaimRequest.Claim.ReportedDate
            oImpClaim.SecondaryCauseCode = MaintainClaimRequest.Claim.SecondaryCauseCode
            oImpClaim.TownCode = MaintainClaimRequest.Claim.TownCode
            oImpClaim.UserDefFldACode = MaintainClaimRequest.Claim.UserDefFldACode
            oImpClaim.UserDefFldBCode = MaintainClaimRequest.Claim.UserDefFldBCode
            oImpClaim.UserDefFldCCode = MaintainClaimRequest.Claim.UserDefFldCCode
            oImpClaim.UserDefFldDCode = MaintainClaimRequest.Claim.UserDefFldDCode
            oImpClaim.UserDefFldECode = MaintainClaimRequest.Claim.UserDefFldECode
            oImpClaim.ClientEmail = MaintainClaimRequest.Claim.ClientEmail
            oImpClaim.ClientFaxNo = MaintainClaimRequest.Claim.ClientFaxNo
            oImpClaim.ClientMobileNo = MaintainClaimRequest.Claim.ClientMobileNo
            oImpClaim.ClientTelNo = MaintainClaimRequest.Claim.ClientTelNo
            oImpClaim.ClientTelNoOff = MaintainClaimRequest.Claim.ClientTelNoOff
            oImpClaim.ReserveOnly = MaintainClaimRequest.Claim.ReserveOnly
            oImpClaim.TPA = MaintainClaimRequest.Claim.TPA

            ' if the client has been specified in the request
            If MaintainClaimRequest.Claim.Client IsNot Nothing Then

                ' there is only ever 1 client per claim so no need to get the bounds
                oImpClient = New BaseImplementationTypes.BaseClaimPartyClientType

                Dim oClientAddress As New BaseImplementationTypes.BaseAddressType

                oClientAddress.AddressLine1 = MaintainClaimRequest.Claim.Client.Address.AddressLine1
                oClientAddress.AddressLine2 = MaintainClaimRequest.Claim.Client.Address.AddressLine2
                oClientAddress.AddressLine3 = MaintainClaimRequest.Claim.Client.Address.AddressLine3
                oClientAddress.AddressLine4 = MaintainClaimRequest.Claim.Client.Address.AddressLine4
                oClientAddress.AddressTypeCode = CType([Enum].ToObject(GetType(AddressTypeType), MaintainClaimRequest.Claim.Client.Address.AddressTypeCode), BaseImplementationTypes.AddressTypeType)
                'oClientAddress.AddressTypeCode = MaintainClaimRequest.Claim.Client.Address.AddressTypeCode
                oClientAddress.CountryCode = MaintainClaimRequest.Claim.Client.Address.CountryCode
                oClientAddress.PostCode = MaintainClaimRequest.Claim.Client.Address.PostCode

                oImpClient.Address = oClientAddress

                ' if client contacts have been provided in the request
                If MaintainClaimRequest.Claim.Client.Contact IsNot Nothing Then

                    oImpClient.Contact = Array.ConvertAll(
                                                MaintainClaimRequest.Claim.Client.Contact.ToArray(),
                                                New Converter(Of BaseContactType,
                                                    BaseImplementationTypes.BaseContactType) _
                                                    (AddressOf CommonFunctions.ToBaseImpBaseContactType))

                End If

                oImpClient.PartyClaimNumber = MaintainClaimRequest.Claim.Client.PartyClaimNumber
                oImpClient.TaxRegistered = MaintainClaimRequest.Claim.Client.TaxRegistered
                oImpClient.TaxRegistrationNumber = MaintainClaimRequest.Claim.Client.TaxRegistrationNumber
                oImpClient.PartyEmail = MaintainClaimRequest.Claim.Client.PartyEmail
                oImpClient.PartyFaxNo = MaintainClaimRequest.Claim.Client.PartyFaxNo
                oImpClient.PartyMobileNo = MaintainClaimRequest.Claim.Client.PartyMobileNo
                oImpClient.PartyTelNo = MaintainClaimRequest.Claim.Client.PartyTelNo
                oImpClient.PartyTelNoOff = MaintainClaimRequest.Claim.Client.PartyTelNoOff


                ' set client into claim
                oImpClaim.Client = oImpClient
            End If

            ' if the client has been specified in the request
            If MaintainClaimRequest.Claim.Insurer IsNot Nothing Then

                '' Process the insurers addresses
                Dim oInsurerAddress As New BaseImplementationTypes.BaseAddressType
                If MaintainClaimRequest.Claim.Insurer.Address IsNot Nothing Then
                    oInsurerAddress.AddressLine1 = MaintainClaimRequest.Claim.Insurer.Address.AddressLine1
                    oInsurerAddress.AddressLine2 = MaintainClaimRequest.Claim.Insurer.Address.AddressLine2
                    oInsurerAddress.AddressLine3 = MaintainClaimRequest.Claim.Insurer.Address.AddressLine3
                    oInsurerAddress.AddressLine4 = MaintainClaimRequest.Claim.Insurer.Address.AddressLine4

                    oInsurerAddress.AddressTypeCode = CType([Enum].ToObject(GetType(AddressTypeType), MaintainClaimRequest.Claim.Insurer.Address.AddressTypeCode), BaseImplementationTypes.AddressTypeType)
                    'oInsurerAddress.AddressTypeCode = MaintainClaimRequest.Claim.Insurer.Address.AddressTypeCode
                    oInsurerAddress.CountryCode = MaintainClaimRequest.Claim.Insurer.Address.CountryCode
                    oInsurerAddress.PostCode = MaintainClaimRequest.Claim.Insurer.Address.PostCode

                    oImpInsurer.Address = oInsurerAddress
                End If
                ' if client contacts have been provided in the request
                If MaintainClaimRequest.Claim.Insurer.Contact IsNot Nothing Then

                    ' Process the insurers contacts
                    oImpInsurer.Contact = Array.ConvertAll(
                                                MaintainClaimRequest.Claim.Insurer.Contact.ToArray,
                                                New Converter(Of BaseContactType,
                                                    BaseImplementationTypes.BaseContactType) _
                                                    (AddressOf CommonFunctions.ToBaseImpBaseContactType))

                End If

                oImpInsurer.ContactName = MaintainClaimRequest.Claim.Insurer.ContactName
                oImpInsurer.PartyClaimNumber = MaintainClaimRequest.Claim.Insurer.PartyClaimNumber
                oImpInsurer.InsurerContact = MaintainClaimRequest.Claim.Insurer.InsurerContact
                oImpInsurer.InsurerEmail = MaintainClaimRequest.Claim.Insurer.InsurerEmail
                oImpInsurer.InsurerFaxNo = MaintainClaimRequest.Claim.Insurer.InsurerFaxNo
                oImpInsurer.InsurerTelNo = MaintainClaimRequest.Claim.Insurer.InsurerTelNo


                ' set the insured into the claim
                oImpClaim.Insurer = oImpInsurer

            End If

            ' if perils were specified in the request
            If MaintainClaimRequest.Claim.ClaimPeril IsNot Nothing Then

                ' process the claim peril array
                oImpClaim.ClaimPeril = Array.ConvertAll(
                                                MaintainClaimRequest.Claim.ClaimPeril.ToArray,
                                                New Converter(Of BaseClaimPerilMaintainType,
                                                    BaseImplementationTypes.BaseClaimPerilMaintainType) _
                                                    (AddressOf CommonFunctions.ToBaseImpBaseClaimPerilMaintainType))

            End If

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = MaintainClaimRequest.BranchCode
            oImpRequest.TimeStamp = MaintainClaimRequest.TimeStamp

            oImpRequest.Claim = oImpClaim

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.MaintainClaim(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.BaseClaimKey = oImpResponse.BaseClaimKey
                oResponse.ClaimKey = oImpResponse.ClaimKey
                oResponse.ClaimNumber = oImpResponse.ClaimNumber
                oResponse.Version = oImpResponse.Version
                oResponse.TimeStamp = oImpResponse.TimeStamp

                oResponse.ResultingStatus = oImpResponse.ResultingStatus

                If oImpResponse.Warnings IsNot Nothing Then
                    oResponse.Warnings = oImpResponse.Warnings.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseClaimResponseTypeWarnings, BaseClaimResponseTypeWarnings)(AddressOf CommonFunctions.ToServiceImpBaseClaimResponseTypeWarnings))

                    Array.ConvertAll(oImpResponse.Warnings.ToArray,
                                                                        New Converter(Of BaseImplementationTypes.BaseClaimResponseTypeWarnings,
                                                                            BaseClaimResponseTypeWarnings) _
                                                                            (AddressOf CommonFunctions.ToServiceImpBaseClaimResponseTypeWarnings))

                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(MaintainClaimRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(MaintainClaimRequest))
            Return Nothing
        End Try

    End Function

    Public Function OpenClaim(ByVal OpenClaimRequest As OpenClaimRequestType) As OpenClaimResponseType Implements IPureClaimService.OpenClaim

        Try

            Dim sUserName As String = OpenClaimRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMOClm", iUserId)
            CommonFunctions.CheckSecurityToken(OpenClaimRequest.WCFSecurityToken)
            Dim oResponse As New OpenClaimResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, OpenClaimRequest.BranchCode)



            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.OpenClaimRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.OpenClaimResponseType = Nothing

            Dim oImpClaim As BaseImplementationTypes.BaseClaimOpenType = New BaseImplementationTypes.BaseClaimOpenType
            Dim oImpClient As BaseImplementationTypes.BaseClaimPartyClientType = New BaseImplementationTypes.BaseClaimPartyClientType
            Dim oImpInsurer As BaseImplementationTypes.BaseClaimPartyInsurerType = New BaseImplementationTypes.BaseClaimPartyInsurerType

            ' default version id for open claim version to 1
            oImpClaim.IgnoreWarnings = OpenClaimRequest.Claim.IgnoreWarnings
            oImpClaim.CatastropheCode = OpenClaimRequest.Claim.CatastropheCode
            oImpClaim.ClaimVersionDescription = OpenClaimRequest.Claim.ClaimVersionDescription
            oImpClaim.Comments = OpenClaimRequest.Claim.Comments
            oImpClaim.CurrencyCode = OpenClaimRequest.Claim.CurrencyCode
            oImpClaim.Description = OpenClaimRequest.Claim.Description
            oImpClaim.HandlerCode = OpenClaimRequest.Claim.HandlerCode
            oImpClaim.InfoOnly = OpenClaimRequest.Claim.InfoOnly
            oImpClaim.InsuranceFileKey = OpenClaimRequest.Claim.InsuranceFileKey
            oImpClaim.LikelyClaim = OpenClaimRequest.Claim.LikelyClaim
            oImpClaim.Location = OpenClaimRequest.Claim.Location
            oImpClaim.LossFromDate = OpenClaimRequest.Claim.LossFromDate
            oImpClaim.DuplicateClaimOverrideUserName = OpenClaimRequest.Claim.DuplicateClaimOverrideUserName
            oImpClaim.DuplicateClaimOverrideUserPassword = OpenClaimRequest.Claim.DuplicateClaimOverrideUserPassword

            oImpClaim.LossToDateSpecified = True

            If OpenClaimRequest.Claim.LossToDateSpecified Then
                oImpClaim.LossToDate = OpenClaimRequest.Claim.LossToDate
            Else
                oImpClaim.LossToDate = OpenClaimRequest.Claim.LossFromDate
            End If

            oImpClaim.PrimaryCauseCode = OpenClaimRequest.Claim.PrimaryCauseCode
            oImpClaim.ProgressStatusCode = OpenClaimRequest.Claim.ProgressStatusCode
            oImpClaim.ReportedDate = OpenClaimRequest.Claim.ReportedDate
            oImpClaim.RiskKey = OpenClaimRequest.Claim.RiskKey
            oImpClaim.SecondaryCauseCode = OpenClaimRequest.Claim.SecondaryCauseCode
            oImpClaim.TownCode = OpenClaimRequest.Claim.TownCode
            oImpClaim.UnderwritingYearCode = OpenClaimRequest.Claim.UnderwritingYearCode
            oImpClaim.ClientEmail = OpenClaimRequest.Claim.ClientEmail
            oImpClaim.ClientFaxNo = OpenClaimRequest.Claim.ClientFaxNo
            oImpClaim.ClientMobileNo = OpenClaimRequest.Claim.ClientMobileNo
            oImpClaim.ClientTelNo = OpenClaimRequest.Claim.ClientTelNo
            oImpClaim.ClientTelNoOff = OpenClaimRequest.Claim.ClientTelNoOff
            oImpClaim.UserDefFldACode = OpenClaimRequest.Claim.UserDefFldACode
            oImpClaim.UserDefFldBCode = OpenClaimRequest.Claim.UserDefFldBCode
            oImpClaim.UserDefFldCCode = OpenClaimRequest.Claim.UserDefFldCCode
            oImpClaim.UserDefFldDCode = OpenClaimRequest.Claim.UserDefFldDCode
            oImpClaim.UserDefFldECode = OpenClaimRequest.Claim.UserDefFldECode
            oImpClaim.ReserveOnly = OpenClaimRequest.Claim.ReserveOnly
            oImpClaim.TPA = OpenClaimRequest.Claim.TPA

            ' if the client has been specified in the request
            If OpenClaimRequest.Claim.Client IsNot Nothing Then

                Dim oClientAddress As New BaseImplementationTypes.BaseAddressType

                oClientAddress.AddressLine1 = OpenClaimRequest.Claim.Client.Address.AddressLine1
                oClientAddress.AddressLine2 = OpenClaimRequest.Claim.Client.Address.AddressLine2
                oClientAddress.AddressLine3 = OpenClaimRequest.Claim.Client.Address.AddressLine3
                oClientAddress.AddressLine4 = OpenClaimRequest.Claim.Client.Address.AddressLine4

                oClientAddress.AddressTypeCode = CType([Enum].ToObject(GetType(AddressTypeType), OpenClaimRequest.Claim.Client.Address.AddressTypeCode), BaseImplementationTypes.AddressTypeType)
                'oClientAddress.AddressTypeCode = OpenClaimRequest.Claim.Client.Address.AddressTypeCode
                oClientAddress.CountryCode = OpenClaimRequest.Claim.Client.Address.CountryCode
                oClientAddress.PostCode = OpenClaimRequest.Claim.Client.Address.PostCode

                oImpClient.Address = oClientAddress

                ' if client contacts have been provided in the request
                If OpenClaimRequest.Claim.Client.Contact IsNot Nothing Then
                    oImpClient.Contact = Array.ConvertAll(
                                                OpenClaimRequest.Claim.Client.Contact.ToArray,
                                                New Converter(Of BaseContactType,
                                                    BaseImplementationTypes.BaseContactType) _
                                                    (AddressOf CommonFunctions.ToBaseImpBaseContactType))
                End If

                oImpClient.PartyClaimNumber = OpenClaimRequest.Claim.Client.PartyClaimNumber
                oImpClient.TaxRegistered = OpenClaimRequest.Claim.Client.TaxRegistered
                oImpClient.TaxRegistrationNumber = OpenClaimRequest.Claim.Client.TaxRegistrationNumber
                oImpClient.PartyEmail = OpenClaimRequest.Claim.Client.PartyEmail
                oImpClient.PartyFaxNo = OpenClaimRequest.Claim.Client.PartyFaxNo
                oImpClient.PartyMobileNo = OpenClaimRequest.Claim.Client.PartyMobileNo
                oImpClient.PartyTelNo = OpenClaimRequest.Claim.Client.PartyTelNo
                oImpClient.PartyTelNoOff = OpenClaimRequest.Claim.Client.PartyTelNoOff


                ' set client into claim
                oImpClaim.Client = oImpClient

            End If

            ' if insurer details were passed in the request
            If OpenClaimRequest.Claim.Insurer IsNot Nothing Then

                Dim oInsurerAddress As New BaseImplementationTypes.BaseAddressType

                oInsurerAddress.AddressLine1 = OpenClaimRequest.Claim.Insurer.Address.AddressLine1
                oInsurerAddress.AddressLine2 = OpenClaimRequest.Claim.Insurer.Address.AddressLine2
                oInsurerAddress.AddressLine3 = OpenClaimRequest.Claim.Insurer.Address.AddressLine3
                oInsurerAddress.AddressLine4 = OpenClaimRequest.Claim.Insurer.Address.AddressLine4

                oInsurerAddress.AddressTypeCode = CType([Enum].ToObject(GetType(AddressTypeType), OpenClaimRequest.Claim.Insurer.Address.AddressTypeCode), BaseImplementationTypes.AddressTypeType)
                'oInsurerAddress.AddressTypeCode = OpenClaimRequest.Claim.Insurer.Address.AddressTypeCode
                oInsurerAddress.CountryCode = OpenClaimRequest.Claim.Insurer.Address.CountryCode
                oInsurerAddress.PostCode = OpenClaimRequest.Claim.Insurer.Address.PostCode

                oImpInsurer.Address = oInsurerAddress

                ' if insurer contacts have been provided in request
                If OpenClaimRequest.Claim.Insurer.Contact IsNot Nothing Then
                    ' Process the insurers contacts
                    oImpInsurer.Contact = Array.ConvertAll(
                                                OpenClaimRequest.Claim.Insurer.Contact.ToArray,
                                                New Converter(Of BaseContactType,
                                                    BaseImplementationTypes.BaseContactType) _
                                                    (AddressOf CommonFunctions.ToBaseImpBaseContactType))
                End If

                oImpInsurer.ContactName = OpenClaimRequest.Claim.Insurer.ContactName
                oImpInsurer.PartyClaimNumber = OpenClaimRequest.Claim.Insurer.PartyClaimNumber
                oImpInsurer.InsurerContact = OpenClaimRequest.Claim.Insurer.InsurerContact
                oImpInsurer.InsurerEmail = OpenClaimRequest.Claim.Insurer.InsurerEmail
                oImpInsurer.InsurerFaxNo = OpenClaimRequest.Claim.Insurer.InsurerFaxNo
                oImpInsurer.InsurerTelNo = OpenClaimRequest.Claim.Insurer.InsurerTelNo


                ' set the insured into the claim
                oImpClaim.Insurer = oImpInsurer

            End If

            ' if perils were specified in the request
            If OpenClaimRequest.Claim.ClaimPeril IsNot Nothing Then

                ' process the claim peril array
                oImpClaim.ClaimPeril = Array.ConvertAll(
                                                OpenClaimRequest.Claim.ClaimPeril.ToArray,
                                                New Converter(Of BaseClaimPerilType,
                                                    BaseImplementationTypes.BaseClaimPerilType) _
                                                    (AddressOf CommonFunctions.ToBaseImpBaseClaimPerilType))

            End If

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = OpenClaimRequest.BranchCode

            oImpRequest.Claim = oImpClaim
            oImpRequest.UserName = sUserName

            oImpRequest.Claim.ClaimNumber = OpenClaimRequest.Claim.ClaimNumber
            oImpRequest.Claim.BaseCaseKey = OpenClaimRequest.Claim.BaseCaseKey


            Try
                ' Call the implementation method
                oImpResponse = oBusiness.OpenClaim(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.BaseClaimKey = oImpResponse.BaseClaimKey
                oResponse.ClaimKey = oImpResponse.ClaimKey
                oResponse.ClaimNumber = oImpResponse.ClaimNumber
                oResponse.Version = oImpResponse.Version
                oResponse.TimeStamp = oImpResponse.TimeStamp

                If oImpResponse.Warnings IsNot Nothing Then
                    oResponse.Warnings = oImpResponse.Warnings.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseClaimResponseTypeWarnings, BaseClaimResponseTypeWarnings)(AddressOf CommonFunctions.ToServiceImpBaseClaimResponseTypeWarnings))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(OpenClaimRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(OpenClaimRequest))
            Return Nothing
        End Try

    End Function

    Public Function PayClaim(ByVal PayClaimRequest As PayClaimRequestType) As PayClaimResponseType Implements IPureClaimService.PayClaim

        ' TODO : MEvans : Rework the calculation of tax items as currently it does not correctly handle advanced tax

        Try
            Dim sUserName As String = PayClaimRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMPClm", iUserId)
            CommonFunctions.CheckSecurityToken(PayClaimRequest.WCFSecurityToken)

            Dim oResponse As New PayClaimResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, PayClaimRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.PayClaimRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.PayClaimResponseType = Nothing

            If PayClaimRequest.ClaimPayment IsNot Nothing Then
                PayClaimIn(oImpRequest, PayClaimRequest)
                oImpRequest.ClaimPayment.ClientKey = PayClaimRequest.ClaimPayment.ClientKey
            ElseIf (PayClaimRequest.ClaimPerilPayment IsNot Nothing) Then
                oImpRequest.ClaimPerilPayment = New List(Of BaseImplementationTypes.BaseClaimPaymentType)
                For Each ClaimPaymentItem In PayClaimRequest.ClaimPerilPayment
                    oImpRequest.ClaimPayment = New BaseImplementationTypes.BaseClaimPaymentType
                    PayClaimIn(oImpRequest, PayClaimRequest)
                    oImpRequest.ClaimPayment.ClientKey = PayClaimRequest.ClaimPayment.ClientKey
                    oImpRequest.ClaimPerilPayment.Add(oImpRequest.ClaimPayment)
                    oImpRequest.ClaimPayment = Nothing
                Next
            End If

            Try
                oImpResponse = New SAMForInsuranceV2ImplementationTypes.PayClaimResponseType
                ' Call the implementation method
                oImpResponse = oBusiness.PayClaim(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'Retrive values
                oResponse.TimeStamp = oImpResponse.TimeStamp
                oResponse.BaseClaimKey = oImpResponse.BaseClaimKey
                oResponse.ClaimKey = oImpResponse.ClaimKey
                oResponse.ClaimNumber = oImpResponse.ClaimNumber
                oResponse.creditedAccountKey = oImpResponse.creditedAccountKey
                oResponse.creditedDocumentKey = oImpResponse.creditedDocumentKey
                oResponse.creditedTransdetailKey = oImpResponse.creditedTransdetailKey

                oResponse.ResultingStatus = oImpResponse.ResultingStatus

                If (oResponse.CashList IsNot Nothing) Then
                    oResponse.CashList = New BaseCashListResponseType
                    oResponse.CashList.CashListKey = oImpResponse.CashList.CashListKey
                End If

                If (oImpResponse.Warnings IsNot Nothing) Then
                    oResponse.Warnings = oImpResponse.Warnings.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseClaimResponseTypeWarnings, BaseClaimResponseTypeWarnings)(AddressOf CommonFunctions.ToServiceImpBaseClaimResponseTypeWarnings))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(PayClaimRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(PayClaimRequest))
            Return Nothing
        End Try

    End Function


    Public Function UpdateClaimReservesOrPayments(ByVal oUpdateClaimReservesOrPaymentsRequest As UpdateClaimReservesOrPaymentsRequestType) As UpdateClaimReservesOrPaymentsResponseType Implements IPureClaimService.UpdateClaimReservesOrPayments
        Try

            'CheckAuthority("SAMUPDCRP")

            Dim sUserName As String = oUpdateClaimReservesOrPaymentsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckSecurityToken(oUpdateClaimReservesOrPaymentsRequest.WCFSecurityToken)
            Dim oResponse As New UpdateClaimReservesOrPaymentsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oUpdateClaimReservesOrPaymentsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateClaimReservesOrPaymentsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateClaimReservesOrPaymentsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oUpdateClaimReservesOrPaymentsRequest.BranchCode
            oImpRequest.ClaimKey = oUpdateClaimReservesOrPaymentsRequest.ClaimKey
            oImpRequest.ProcessType = oUpdateClaimReservesOrPaymentsRequest.ProcessType
            oImpRequest.TimeStamp = oUpdateClaimReservesOrPaymentsRequest.TimeStamp

            ' Map peril details to implementation request structure
            ' if perils were specified in the request
            If oUpdateClaimReservesOrPaymentsRequest.ClaimPeril IsNot Nothing Then

                ' process the claim peril array
                oImpRequest.ClaimPeril = Array.ConvertAll(
                                                oUpdateClaimReservesOrPaymentsRequest.ClaimPeril.ToArray,
                                                New Converter(Of BaseClaimPerilType,
                                                    BaseImplementationTypes.BaseClaimPerilType) _
                                                    (AddressOf CommonFunctions.ToBaseImpBaseClaimPerilType))

            End If

            ' Map payment details to implementation request structure
            ' if payments were specified in the request
            If oUpdateClaimReservesOrPaymentsRequest.ClaimPayment IsNot Nothing Then
                oImpRequest.ClaimPayment = New BaseImplementationTypes.BaseClaimPaymentType
                ClaimPaymentIn(oImpRequest.ClaimPayment, oUpdateClaimReservesOrPaymentsRequest.ClaimPayment, "", False)
            End If
            ' Call the implementation method
            Try
                oImpResponse = New SAMForInsuranceV2ImplementationTypes.UpdateClaimReservesOrPaymentsResponseType
                oImpResponse = oBusiness.UpdateClaimReservesOrPayments(oImpRequest)

                ' Map the response back to the outer structure

                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.TimeStamp = oImpResponse.TimeStamp
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdateClaimReservesOrPaymentsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateClaimReservesOrPaymentsRequest))
            Return Nothing
        End Try
    End Function

    Public Function UpdateClaimRisk(ByVal UpdateClaimRiskRequest As UpdateClaimRiskRequestType) As UpdateClaimRiskResponseType Implements IPureClaimService.UpdateClaimRisk

        Try
            Dim sUserName As String = UpdateClaimRiskRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMUClmRsk", iUserId)
            CommonFunctions.CheckSecurityToken(UpdateClaimRiskRequest.WCFSecurityToken)

            Dim oResponse As New UpdateClaimRiskResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, UpdateClaimRiskRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateClaimRiskRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateClaimRiskResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = UpdateClaimRiskRequest.BranchCode
            oImpRequest.BaseClaimKey = UpdateClaimRiskRequest.BaseClaimKey
            oImpRequest.XMLDataSet = UpdateClaimRiskRequest.XMLDataSet
            oImpRequest.TimeStamp = UpdateClaimRiskRequest.TimeStamp

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateClaimRisk(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.TimeStamp = oImpResponse.TimeStamp

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(UpdateClaimRiskRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(UpdateClaimRiskRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="AddClaimRiskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddClaimRisk(ByVal AddClaimRiskRequest As AddClaimRiskRequestType) As AddClaimRiskResponseType Implements IPureClaimService.AddClaimRisk

        Try

            Dim sUserName As String = AddClaimRiskRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGClmRsk", iUserId)
            CommonFunctions.CheckSecurityToken(AddClaimRiskRequest.WCFSecurityToken)

            Dim oResponse As New AddClaimRiskResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, AddClaimRiskRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetClaimRiskRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetClaimRiskResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = AddClaimRiskRequest.BranchCode
            oImpRequest.BaseClaimKey = AddClaimRiskRequest.BaseClaimKey
            oImpRequest.Task = SAMConstants.SAMComponentAction.PMAdd
            oImpRequest.TimeStamp = AddClaimRiskRequest.TimeStamp

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetClaimRisk(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                'oResponse.QuoteTimeStamp = oImpResponse.QuoteTimeStamp
                oResponse.TimeStamp = oImpResponse.TimeStamp
                oResponse.XMLDataSet = SAMFunc.TransformDatasetPBtoSAM(oImpResponse.XMLDataSet)

            Catch ex As Exception

                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(AddClaimRiskRequest))

            End Try

            Return oResponse

        Catch ex As Exception

            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(AddClaimRiskRequest))
            Return Nothing

        End Try

    End Function

#Region "Find Case"
    Public Function FindCase(ByVal oFindCaseRequest As FindCaseRequestType) As FindCaseResponseType Implements IPureClaimService.FindCase

        Try

            Dim sUserName As String = oFindCaseRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMFCASE", iUserId)
            CommonFunctions.CheckSecurityToken(oFindCaseRequest.WCFSecurityToken)
            Dim oResponse As New FindCaseResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oFindCaseRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.FindCaseRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.FindCaseResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oFindCaseRequest.BranchCode
            oImpRequest.CaseNumber = oFindCaseRequest.CaseNumber
            oImpRequest.CaseOpenDateSpecified = oFindCaseRequest.CaseOpenDateSpecified
            oImpRequest.CaseOpenDate = oFindCaseRequest.CaseOpenDate
            oImpRequest.ClaimNumber = oFindCaseRequest.ClaimNumber
            oImpRequest.ProgressStatusCode = oFindCaseRequest.ProgressStatusCode
            oImpRequest.RiskIndex = oFindCaseRequest.RiskIndex
            oImpRequest.RiskType = oFindCaseRequest.RiskType
            If oFindCaseRequest.MaxRowsToFetchSpecified Then
                oImpRequest.MaxRowsToFetch = oFindCaseRequest.MaxRowsToFetch
            Else
                oImpRequest.MaxRowsToFetch = -1
            End If
            oImpRequest.WCFSecurityToken = If(oFindCaseRequest.WCFSecurityToken.Length > 0, oFindCaseRequest.WCFSecurityToken, "WCFSecurityToken")
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.FindCase(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' oResponse.CaseDetails = SAMFunc.GetDeserializedValues(Of List(Of BaseFindCaseResponseTypeCaseDetailsRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseFindCaseResponseTypeCaseDetails", sConvertToTypeName:="BaseFindCaseResponseTypeCaseDetailsRow")
                If oImpResponse.ResultData IsNot Nothing Then
                    oResponse.CaseDetails = DataTabletoList_FindCaseCaseDetails(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oFindCaseRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oFindCaseRequest))
            Return Nothing
        End Try

    End Function
#End Region

#Region "Close Case"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oCloseCaseRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CloseCase(ByVal oCloseCaseRequest As CloseCaseRequestType) As CloseCaseResponseType Implements IPureClaimService.CloseCase

        Try


            Dim sUserName As String = oCloseCaseRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMCCASE", iUserId)
            CommonFunctions.CheckSecurityToken(oCloseCaseRequest.WCFSecurityToken)

            Dim oResponse As New CloseCaseResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oCloseCaseRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.CloseCaseRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.CloseCaseResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oCloseCaseRequest.BranchCode
            oImpRequest.CaseKey = oCloseCaseRequest.CaseKey
            oImpRequest.BaseCaseKey = oCloseCaseRequest.BaseCaseKey
            oImpRequest.EventDescription = oCloseCaseRequest.EventDescription
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.CloseCase(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oCloseCaseRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oCloseCaseRequest))
            Return Nothing
        End Try

    End Function
#End Region
#Region "Save Case"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oSaveCaseRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SaveCase(ByVal oSaveCaseRequest As SaveCaseRequestType) As SaveCaseResponseType Implements IPureClaimService.SaveCase

        Try



            Dim sUserName As String = oSaveCaseRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMSCASE", iUserId)
            CommonFunctions.CheckSecurityToken(oSaveCaseRequest.WCFSecurityToken)
            Dim oResponse As New SaveCaseResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(oSaveCaseRequest.LoginUserName, oSaveCaseRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.SaveCaseRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.SaveCaseResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oSaveCaseRequest.BranchCode
            oImpRequest.CaseKey = oSaveCaseRequest.CaseKey
            oImpRequest.CaseKeySpecified = oSaveCaseRequest.CaseKeySpecified
            oImpRequest.CaseNumber = oSaveCaseRequest.CaseNumber
            oImpRequest.Analyst = oSaveCaseRequest.Analyst
            oImpRequest.Assistant = oSaveCaseRequest.Assistant
            oImpRequest.CaseOpenDate = oSaveCaseRequest.CaseOpenDate
            oImpRequest.ProgressStatusCode = oSaveCaseRequest.ProgressStatusCode
            oImpRequest.EventDescription = oSaveCaseRequest.EventDescription
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.SaveCase(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.CaseKey = oImpResponse.CaseKey
                oResponse.BaseCaseKey = oImpResponse.BaseCaseKey
                oResponse.CaseNumber = oImpResponse.CaseNumber
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oSaveCaseRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oSaveCaseRequest))
            Return Nothing
        End Try

    End Function
#End Region

#Region "CaseLinkUnlink"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oCaseLinkUnlinkRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CaseLinkUnlink(ByVal oCaseLinkUnlinkRequest As CaseLinkUnLinkRequestType) As CaseLinkUnLinkResponseType Implements IPureClaimService.CaseLinkUnlink

        Try


            Dim sUserName As String = oCaseLinkUnlinkRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMCASELU", iUserId)
            CommonFunctions.CheckSecurityToken(oCaseLinkUnlinkRequest.WCFSecurityToken)

            Dim oResponse As New CaseLinkUnLinkResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oCaseLinkUnlinkRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.CaseLinkUnLinkRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.CaseLinkUnLinkResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oCaseLinkUnlinkRequest.BranchCode
            oImpRequest.BaseCaseKey = oCaseLinkUnlinkRequest.BaseCaseKey
            oImpRequest.ClaimKey = oCaseLinkUnlinkRequest.ClaimKey
            oImpRequest.IsLinked = oCaseLinkUnlinkRequest.IsLinked
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.CaseLinkUnlink(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                If oImpResponse.Warnings IsNot Nothing Then
                    oResponse.Warnings = New List(Of BaseGeneralWarningResponseType)
                    Dim oWarning As New BaseGeneralWarningResponseType
                    oWarning.Code = oImpResponse.Warnings(0).Code
                    oWarning.Description = oImpResponse.Warnings(0).Description
                    oResponse.Warnings.Add(oWarning)
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oCaseLinkUnlinkRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oCaseLinkUnlinkRequest))
            Return Nothing
        End Try

    End Function
#End Region



    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oAddCashClaimLinkRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddCashClaimLink(ByVal oAddCashClaimLinkRequest As AddCashClaimLinkRequestType) As AddCashClaimLinkResponseType Implements IPureClaimService.AddCashClaimLink

        Try



            Dim sUserName As String = oAddCashClaimLinkRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            'CommonFunctions.CheckAuthority("SAMACCLINK", iUserId)
            CommonFunctions.CheckSecurityToken(oAddCashClaimLinkRequest.WCFSecurityToken)
            Dim oResponse As New AddCashClaimLinkResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oAddCashClaimLinkRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.AddCashClaimLinkRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.AddCashClaimLinkResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oAddCashClaimLinkRequest.BranchCode
            oImpRequest.ClaimPaymentKey = oAddCashClaimLinkRequest.ClaimPaymentKey
            oImpRequest.CashListItemKey = oAddCashClaimLinkRequest.CashListItemKey
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.AddCashClaimLink(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oAddCashClaimLinkRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oAddCashClaimLinkRequest))
            Return Nothing
        End Try

    End Function
#Region "GetCashClaimLink"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetCashClaimLinkRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCashClaimLink(ByVal oGetCashClaimLinkRequest As GetCashClaimLinkRequestType) As GetCashClaimLinkResponseType Implements IPureClaimService.GetCashClaimLink

        Try



            Dim sUserName As String = oGetCashClaimLinkRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGCCLINK", iUserId)
            CommonFunctions.CheckSecurityToken(oGetCashClaimLinkRequest.WCFSecurityToken)
            Dim oResponse As New GetCashClaimLinkResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetCashClaimLinkRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetCashClaimLinkRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetCashClaimLinkResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetCashClaimLinkRequest.BranchCode
            oImpRequest.ClaimPaymentKey = oGetCashClaimLinkRequest.ClaimPaymentKey

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetCashClaimLink(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.Amount = oImpResponse.Amount
                oResponse.BranchCode = oImpResponse.BranchCode
                oResponse.CashListItemKey = oImpResponse.CashListItemKey
                oResponse.CashListKey = oImpResponse.CashListKey
                oResponse.CurrencyCode = oImpResponse.CurrencyCode
                oResponse.MediaTypeCode = oImpResponse.MediaTypeCode

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetCashClaimLinkRequest))
            Return Nothing
        End Try

    End Function
#End Region

#Region "GetTaxTypesAndBands"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetTaxTypesAndBandsRequest"></param>
    ''' <returns></returns>
    Public Function GetTaxTypesAndBands(ByVal oGetTaxTypesAndBandsRequest As GetTaxTypesAndBandsRequestType) As GetTaxTypesAndBandsResponseType Implements IPureClaimService.GetTaxTypesAndBand

        Try

            Dim sUserName As String = oGetTaxTypesAndBandsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGCCLINK", iUserId)
            CommonFunctions.CheckSecurityToken(oGetTaxTypesAndBandsRequest.WCFSecurityToken)
            Dim oResponse As New GetTaxTypesAndBandsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetTaxTypesAndBandsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetTaxTypesAndBandsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetTaxTypesAndBandsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetTaxTypesAndBandsRequest.BranchCode
            oImpRequest.TaxGroupId = oGetTaxTypesAndBandsRequest.TaxGroupId

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetTaxTypesAndBands(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.BankAccounts = SAMFunc.GetDeserializedValues(Of List(Of BaseGetBankAccountsResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataSet, sFromTypeName:="BaseGetBankAccountsResponseTypeBankAccounts", sConvertToTypeName:="BaseGetBankAccountsResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Taxes = DataTabletoList_GetTaxeTypesAndBands(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetTaxTypesAndBandsRequest))
            Return Nothing
        End Try

    End Function
#End Region

    Public Function GetClaimReinsuranceArrangementLines(ByVal GetClaimReinsuranceArrangementLinesRequest As GetClaimReinsuranceArrangementLinesRequestType) As GetClaimReinsuranceArrangementLinesResponseType Implements IPureClaimService.GetClaimReinsuranceArrangementLines
        Try
            Dim sUserName As String = GetClaimReinsuranceArrangementLinesRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHRKEY", iUserId)
            CommonFunctions.CheckSecurityToken(GetClaimReinsuranceArrangementLinesRequest.WCFSecurityToken)
            Dim oResponse As New GetClaimReinsuranceArrangementLinesResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetClaimReinsuranceArrangementLinesRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetClaimReinsuranceArrangementLinesRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetClaimReinsuranceArrangementLinesResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetClaimReinsuranceArrangementLinesRequest.BranchCode
            oImpRequest.ClaimId = GetClaimReinsuranceArrangementLinesRequest.ClaimId
            oImpRequest.ArrangementId = GetClaimReinsuranceArrangementLinesRequest.ArrangementId

            oImpRequest.ModeSpecified = GetClaimReinsuranceArrangementLinesRequest.ModeSpecified
            If (oImpRequest.ModeSpecified) Then
                oImpRequest.Mode = GetClaimReinsuranceArrangementLinesRequest.Mode
            End If
            oImpRequest.WCFSecurityToken = If(GetClaimReinsuranceArrangementLinesRequest.WCFSecurityToken.Length > 0, GetClaimReinsuranceArrangementLinesRequest.WCFSecurityToken, "WCFSecurityToken")

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetClaimReinsuranceArrangementLines(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' oResponse.ReinsuranceArrangementLines = SAMFunc.GetDeserializedValues(Of List(Of BaseGetClaimReinsuranceArrangementLinesResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetClaimReinsuranceArrangementLinesResponseTypeReinsuranceArrangementLines", sConvertToTypeName:="BaseGetClaimReinsuranceArrangementLinesResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.ReinsuranceArrangementLines = DataTabletoList_GetClaimReinsuranceArrangementLines(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetClaimReinsuranceArrangementLinesRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetClaimReinsuranceArrangementLinesRequest))
            Return Nothing
        End Try
    End Function


    Public Function GetClaimReinsuranceArrangements(ByVal GetClaimReinsuranceArrangementsRequest As GetClaimReinsuranceArrangementsRequestType) As GetClaimReinsuranceArrangementsResponseType Implements IPureClaimService.GetClaimReinsuranceArrangements

        Try
            Dim sUserName As String = GetClaimReinsuranceArrangementsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHRKEY", iUserId)
            CommonFunctions.CheckSecurityToken(GetClaimReinsuranceArrangementsRequest.WCFSecurityToken)
            Dim oResponse As New GetClaimReinsuranceArrangementsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetClaimReinsuranceArrangementsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetClaimReinsuranceArrangementsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetClaimReinsuranceArrangementsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetClaimReinsuranceArrangementsRequest.BranchCode
            oImpRequest.ClaimId = GetClaimReinsuranceArrangementsRequest.ClaimId

            oImpRequest.ModeSpecified = GetClaimReinsuranceArrangementsRequest.ModeSpecified
            If (GetClaimReinsuranceArrangementsRequest.ModeSpecified) Then
                oImpRequest.Mode = GetClaimReinsuranceArrangementsRequest.Mode
            End If
            oImpRequest.WCFSecurityToken = If(GetClaimReinsuranceArrangementsRequest.WCFSecurityToken.Length > 0, GetClaimReinsuranceArrangementsRequest.WCFSecurityToken, "WCFSecurityToken")
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetClaimReinsuranceArrangements(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                'oResponse.ReinsuranceArrangements = SAMFunc.GetDeserializedValues(Of List(Of BaseGetClaimReinsuranceArrangementsResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetClaimReinsuranceArrangementsResponseTypeReinsuranceArrangements", sConvertToTypeName:="BaseGetClaimReinsuranceArrangementsResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.ReinsuranceArrangements = DataTabletoList_GetClaimReinsuranceArrangements(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetClaimReinsuranceArrangementsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetClaimReinsuranceArrangementsRequest))
            Return Nothing
        End Try
    End Function


    Public Function GetClaimReinsuranceBands(ByVal GetClaimReinsuranceBandsRequest As GetClaimReinsuranceBandsRequestType) As GetClaimReinsuranceBandsResponseType Implements IPureClaimService.GetClaimReinsuranceBands

        Try
            Dim sUserName As String = GetClaimReinsuranceBandsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHRKEY", iUserId)
            CommonFunctions.CheckSecurityToken(GetClaimReinsuranceBandsRequest.WCFSecurityToken)
            Dim oResponse As New GetClaimReinsuranceBandsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetClaimReinsuranceBandsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetClaimReinsuranceBandsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetClaimReinsuranceBandsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetClaimReinsuranceBandsRequest.BranchCode
            oImpRequest.ClaimId = GetClaimReinsuranceBandsRequest.ClaimId
            oImpRequest.WCFSecurityToken = If(GetClaimReinsuranceBandsRequest.WCFSecurityToken.Length > 0, GetClaimReinsuranceBandsRequest.WCFSecurityToken, "WCFSecurityToken")
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetClaimReinsuranceBands(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.ReinsuranceBands = SAMFunc.GetDeserializedValues(Of List(Of BaseGetClaimReinsuranceBandsResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetClaimReinsuranceBandsResponseTypeReinsuranceBands", sConvertToTypeName:="BaseGetClaimReinsuranceBandsResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.ReinsuranceBands = DataTabletoList_GetClaimReinsuranceBands(oImpResponse.ResultData.Tables(0))
                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetClaimReinsuranceBandsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetClaimReinsuranceBandsRequest))
            Return Nothing
        End Try
    End Function

    Public Function GetRecoveryReinsurance(ByVal GetRecoveryReinsuranceRequest As GetRecoveryReinsuranceRequestType) As GetRecoveryReinsuranceResponseType Implements IPureClaimService.GetRecoveryReinsurance

        Try
            Dim sUserName As String = GetRecoveryReinsuranceRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGRREINS", iUserId)
            CommonFunctions.CheckSecurityToken(GetRecoveryReinsuranceRequest.WCFSecurityToken)

            Dim oResponse As New GetRecoveryReinsuranceResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetRecoveryReinsuranceRequest.BranchCode)

            ' Implementation structures
            Dim oImplementationRequest As New SAMForInsuranceV2ImplementationTypes.GetRecoveryReinsuranceRequestType
            Dim oImplementationResponse As SAMForInsuranceV2ImplementationTypes.GetRecoveryReinsuranceResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImplementationRequest.BranchCode = GetRecoveryReinsuranceRequest.BranchCode
            oImplementationRequest.ClaimPerilKey = GetRecoveryReinsuranceRequest.ClaimPerilKey
            oImplementationRequest.IsSalvage = GetRecoveryReinsuranceRequest.IsSalvage
            oImplementationRequest.WCFSecurityToken = If(GetRecoveryReinsuranceRequest.WCFSecurityToken.Length > 0, GetRecoveryReinsuranceRequest.WCFSecurityToken, "WCFSecurityToken")
            Try
                ' Call the implementation method
                oImplementationResponse = oBusiness.GetRecoveryReinsurance(oImplementationRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImplementationResponse.STSError)

                'oResponse.Reinsurances = SAMFunc.GetDeserializedValues(Of List(Of BaseGetRecoveryReinsuranceResponseTypeRow))(elmResultDataSet:=oImplementationResponse.ResultDataset, sFromTypeName:="BaseGetRecoveryReinsuranceResponseTypeReinsurances", sConvertToTypeName:="BaseGetRecoveryReinsuranceResponseTypeRow")
                If oImplementationResponse.ResultData IsNot Nothing AndAlso oImplementationResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Reinsurances = DataTabletoList_GetRecoveryReinsurance(oImplementationResponse.ResultData.Tables(0))
                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImplementationResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetRecoveryReinsuranceRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetRecoveryReinsuranceRequest))
            Return Nothing
        End Try

    End Function


    Public Function GetClaimRIArrangementLinesRI2007(ByVal GetClaimRIArrangementLinesRI2007Request As GetClaimRIArrangementLinesRI2007RequestType) As GetClaimRIArrangementLinesRI2007ResponseType Implements IPureClaimService.GetClaimRIArrangementLinesRI2007
        Try

            Dim sUserName As String = GetClaimRIArrangementLinesRI2007Request.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGCLRI07", iUserId)
            CommonFunctions.CheckSecurityToken(GetClaimRIArrangementLinesRI2007Request.WCFSecurityToken)

            Dim oResponse As New GetClaimRIArrangementLinesRI2007ResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetClaimRIArrangementLinesRI2007Request.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetClaimRIArrangementLinesRI2007RequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetClaimRIArrangementLinesRI2007ResponseType '= Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetClaimRIArrangementLinesRI2007Request.BranchCode
            oImpRequest.ClaimKey = GetClaimRIArrangementLinesRI2007Request.ClaimKey
            oImpRequest.ArrangementKey = GetClaimRIArrangementLinesRI2007Request.ArrangementKey
            oImpRequest.ModeSpecified = GetClaimRIArrangementLinesRI2007Request.ModeSpecified
            If (GetClaimRIArrangementLinesRI2007Request.ModeSpecified) Then
                oImpRequest.Mode = GetClaimRIArrangementLinesRI2007Request.Mode
            End If
            oImpRequest.IsRecoverySpecified = GetClaimRIArrangementLinesRI2007Request.IsRecoverySpecified
            If (GetClaimRIArrangementLinesRI2007Request.IsRecoverySpecified) Then
                oImpRequest.IsRecovery = GetClaimRIArrangementLinesRI2007Request.IsRecovery
            End If
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetClaimRIArrangementLinesRI2007(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                If (oImpResponse.RIArrangementLines IsNot Nothing) Then
                    oResponse.RIArrangementLines = oImpResponse.RIArrangementLines.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseClaimRiskRIArrangementLineType, BaseClaimRiskRIArrangementLineType)(AddressOf CommonFunctions.ToServiceClaimRiskRIArrangementLineTypeList))
                End If
            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try
            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetClaimRIArrangementLinesRI2007Request))
            Return Nothing
        End Try
    End Function

    Public Function UpdateClaimRIArrangementLinesRI2007(ByVal UpdateClaimRIArrangementLinesRI2007Request As UpdateClaimRIArrangementLinesRI2007RequestType) As UpdateClaimRIArrangementLinesRI2007ResponseType Implements IPureClaimService.UpdateClaimRIArrangementLinesRI2007
        Try

            Dim sUserName As String = UpdateClaimRIArrangementLinesRI2007Request.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMUCLRI07", iUserId)
            CommonFunctions.CheckSecurityToken(UpdateClaimRIArrangementLinesRI2007Request.WCFSecurityToken)
            Dim oResponse As New UpdateClaimRIArrangementLinesRI2007ResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, UpdateClaimRIArrangementLinesRI2007Request.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateClaimRIArrangementLinesRI2007RequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateClaimRIArrangementLinesRI2007ResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = UpdateClaimRIArrangementLinesRI2007Request.BranchCode
            If (UpdateClaimRIArrangementLinesRI2007Request.ClaimRIArrangementLines IsNot Nothing) Then
                With UpdateClaimRIArrangementLinesRI2007Request
                    ReDim oImpRequest.ClaimRIArrangementLines(.ClaimRIArrangementLines.Count - 1)
                    For iCount As Integer = 0 To .ClaimRIArrangementLines.Count - 1
                        oImpRequest.ClaimRIArrangementLines(iCount) = New BaseImplementationTypes.BaseClaimRiskRIArrangementLineType
                        oImpRequest.ClaimRIArrangementLines(iCount).ActionType = CType(.ClaimRIArrangementLines(iCount).ActionType, BaseImplementationTypes.RowAction)
                        oImpRequest.ClaimRIArrangementLines(iCount).RIArrangementKey = .ClaimRIArrangementLines(iCount).RIArrangementKey
                        oImpRequest.ClaimRIArrangementLines(iCount).RIArrangementLineKey = .ClaimRIArrangementLines(iCount).RIArrangementLineKey
                        oImpRequest.ClaimRIArrangementLines(iCount).RIPlacement = .ClaimRIArrangementLines(iCount).RIPlacement
                        oImpRequest.ClaimRIArrangementLines(iCount).RIName = .ClaimRIArrangementLines(iCount).RIName
                        oImpRequest.ClaimRIArrangementLines(iCount).Type = .ClaimRIArrangementLines(iCount).Type
                        oImpRequest.ClaimRIArrangementLines(iCount).Retained = .ClaimRIArrangementLines(iCount).Retained / 100
                        oImpRequest.ClaimRIArrangementLines(iCount).DefaultSharePercent = .ClaimRIArrangementLines(iCount).DefaultSharePercent
                        oImpRequest.ClaimRIArrangementLines(iCount).ThisSharePercent = .ClaimRIArrangementLines(iCount).ThisSharePercent
                        oImpRequest.ClaimRIArrangementLines(iCount).LowerLimit = .ClaimRIArrangementLines(iCount).LowerLimit
                        oImpRequest.ClaimRIArrangementLines(iCount).LineLimit = .ClaimRIArrangementLines(iCount).LineLimit
                        oImpRequest.ClaimRIArrangementLines(iCount).AgreementCode = .ClaimRIArrangementLines(iCount).AgreementCode
                        oImpRequest.ClaimRIArrangementLines(iCount).IsDomiciledForTax = .ClaimRIArrangementLines(iCount).IsDomiciledForTax
                        oImpRequest.ClaimRIArrangementLines(iCount).Grouping = .ClaimRIArrangementLines(iCount).Grouping
                        oImpRequest.ClaimRIArrangementLines(iCount).IsRIBroker = .ClaimRIArrangementLines(iCount).IsRIBroker
                        oImpRequest.ClaimRIArrangementLines(iCount).ReinsuranceTypeCode = .ClaimRIArrangementLines(iCount).ReinsuranceTypeCode
                        oImpRequest.ClaimRIArrangementLines(iCount).TreatyCode = .ClaimRIArrangementLines(iCount).TreatyCode
                        oImpRequest.ClaimRIArrangementLines(iCount).PartyKey = .ClaimRIArrangementLines(iCount).PartyKey
                        oImpRequest.ClaimRIArrangementLines(iCount).Priority = .ClaimRIArrangementLines(iCount).Priority
                        oImpRequest.ClaimRIArrangementLines(iCount).NumberOfLines = .ClaimRIArrangementLines(iCount).NumberOfLines
                        oImpRequest.ClaimRIArrangementLines(iCount).CedePremiumOnly = .ClaimRIArrangementLines(iCount).CedePremiumOnly
                        oImpRequest.ClaimRIArrangementLines(iCount).SumInsured = .ClaimRIArrangementLines(iCount).SumInsured
                        oImpRequest.ClaimRIArrangementLines(iCount).Incurred = .ClaimRIArrangementLines(iCount).Incurred
                        oImpRequest.ClaimRIArrangementLines(iCount).ReserveToDate = .ClaimRIArrangementLines(iCount).ReserveToDate
                        oImpRequest.ClaimRIArrangementLines(iCount).ThisReserve = .ClaimRIArrangementLines(iCount).ThisReserve
                        oImpRequest.ClaimRIArrangementLines(iCount).PaymentToDate = .ClaimRIArrangementLines(iCount).PaymentToDate
                        oImpRequest.ClaimRIArrangementLines(iCount).ThisPayment = .ClaimRIArrangementLines(iCount).ThisPayment
                        oImpRequest.ClaimRIArrangementLines(iCount).RecoverToDate = .ClaimRIArrangementLines(iCount).RecoverToDate
                        oImpRequest.ClaimRIArrangementLines(iCount).Balance = .ClaimRIArrangementLines(iCount).Balance

                        If (.ClaimRIArrangementLines(iCount).BrokerParticipants IsNot Nothing) Then
                            ReDim oImpRequest.ClaimRIArrangementLines(iCount).BrokerParticipants(.ClaimRIArrangementLines(iCount).BrokerParticipants.Count - 1)
                            For iCount1 As Integer = 0 To .ClaimRIArrangementLines(iCount).BrokerParticipants.Count - 1
                                oImpRequest.ClaimRIArrangementLines(iCount).BrokerParticipants(iCount1) = New BaseImplementationTypes.BaseBrokerParticipants
                                oImpRequest.ClaimRIArrangementLines(iCount).BrokerParticipants(iCount1).PartyKey = .ClaimRIArrangementLines(iCount).BrokerParticipants(iCount1).PartyKey
                                oImpRequest.ClaimRIArrangementLines(iCount).BrokerParticipants(iCount1).PartyCode = .ClaimRIArrangementLines(iCount).BrokerParticipants(iCount1).PartyCode
                                oImpRequest.ClaimRIArrangementLines(iCount).BrokerParticipants(iCount1).PartyName = .ClaimRIArrangementLines(iCount).BrokerParticipants(iCount1).PartyName
                                oImpRequest.ClaimRIArrangementLines(iCount).BrokerParticipants(iCount1).ParticpationPercentage = .ClaimRIArrangementLines(iCount).BrokerParticipants(iCount1).ParticpationPercentage
                            Next
                        End If

                        If (.ClaimRIArrangementLines(iCount).FAXParticipants IsNot Nothing) Then
                            ReDim oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(.ClaimRIArrangementLines(iCount).FAXParticipants.Count - 1)
                            For iCount1 As Integer = 0 To .ClaimRIArrangementLines(iCount).FAXParticipants.Count - 1
                                oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1) = New BaseImplementationTypes.BaseClaimFAXParticipants
                                oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).ActionType = CType(.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).ActionType, BaseImplementationTypes.RowAction)
                                oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).RIArrangementLineKey = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).RIArrangementLineKey
                                oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).PartyKey = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).PartyKey
                                oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).PartyCode = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).PartyCode
                                oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).PartyName = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).PartyName
                                oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).AccountType = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).AccountType
                                oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).ParticpationPercentage = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).ParticpationPercentage
                                oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).SumInsured = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).SumInsured
                                oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).AgreementCode = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).AgreementCode
                                oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).ReserveToDate = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).ReserveToDate
                                oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).ThisReserve = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).ThisReserve
                                oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).PaymentToDate = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).PaymentToDate
                                oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).ThisPayment = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).ThisPayment
                                If (.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants IsNot Nothing) Then
                                    ReDim oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants.Count - 1)
                                    For iCount2 As Integer = 0 To .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants.Count - 1
                                        oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2) = New BaseImplementationTypes.BaseBrokerParticipants
                                        oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).PartyKey = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).PartyKey
                                        oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).PartyCode = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).PartyCode
                                        oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).PartyName = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).PartyName
                                        oImpRequest.ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).ParticpationPercentage = .ClaimRIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).ParticpationPercentage
                                    Next
                                End If
                            Next
                        End If
                    Next
                End With
            End If

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateClaimRIArrangementLinesRI2007(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(UpdateClaimRIArrangementLinesRI2007Request))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(UpdateClaimRIArrangementLinesRI2007Request))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' WPR 85 - To generate a cash list
    ''' </summary>
    ''' <param name="oGenerateCashListRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GenerateCashList(ByVal oGenerateCashListRequest As GenerateCashListRequestType) As GenerateCashListResponseType Implements IPureClaimService.GenerateCashList

        Try

            Dim sUserName As String = oGenerateCashListRequest.LoginUserName
            Dim nAgentKey As Integer
            Dim nUserId As Integer

            CommonFunctions.GetIdentity(sUserName, nAgentKey, nUserId)
            CommonFunctions.CheckAuthority("SAMGENCALI", nUserId)
            CommonFunctions.CheckSecurityToken(oGenerateCashListRequest.WCFSecurityToken)

            Dim oResponse As New GenerateCashListResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGenerateCashListRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New BaseImplementationTypes.BaseGenerateCashListRequestType
            Dim oImpResponse As BaseImplementationTypes.BaseGenerateCashListResponseType = Nothing

            oImpRequest.ClaimId = oGenerateCashListRequest.ClaimId

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GenerateCashList(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGenerateCashListRequest))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' WPR 85- To get default bank account for currency 
    ''' </summary>
    ''' <param name="oGetDefaultBankAccountWithCurrencyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDefaultBankAccountWithCurrency(ByVal oGetDefaultBankAccountWithCurrencyRequest As GetDefaultBankAccountWithCurrencyRequestType) As GetDefaultBankAccountWithCurrencyResponseType Implements IPureClaimService.GetDefaultBankAccountWithCurrency
        Try
            Dim sUserName As String = oGetDefaultBankAccountWithCurrencyRequest.LoginUserName
            Dim nAgentKey As Integer
            Dim nUserId As Integer

            CommonFunctions.GetIdentity(sUserName, nAgentKey, nUserId)
            CommonFunctions.CheckAuthority("SAMDEFBAWC", nUserId)
            CommonFunctions.CheckSecurityToken(oGetDefaultBankAccountWithCurrencyRequest.WCFSecurityToken)

            Dim oResponse As New GetDefaultBankAccountWithCurrencyResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetDefaultBankAccountWithCurrencyRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetDefaultBankAccountWithCurrencyRequestType

            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetDefaultBankAccountWithCurrencyResponseType = Nothing

            oImpRequest.BranchCode = oGetDefaultBankAccountWithCurrencyRequest.BranchCode
            oImpRequest.ProductCode = oGetDefaultBankAccountWithCurrencyRequest.ProductCode
            oImpRequest.MediaTypeID = oGetDefaultBankAccountWithCurrencyRequest.MediaTypeID
            oImpRequest.CashListTypeID = oGetDefaultBankAccountWithCurrencyRequest.CashListTypeID

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetDefaultBankAccountWithCurrency(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                If oImpResponse.ResultDataset IsNot Nothing Then
                    oResponse.Results = SAMFunc.GetDeserializedValues(Of List(Of BaseGetDefaultBankAccountWithCurrencyResponseTypeResultsRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetDefaultBankAccountWithCurrencyResponseTypeResults", sConvertToTypeName:="BaseGetDefaultBankAccountWithCurrencyResponseTypeResultsRow")
                End If

            Catch ex As Exception
                BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetDefaultBankAccountWithCurrencyRequest))
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' CheckReserveRecovery
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckReserveRecovery(ByVal oRequest As CheckReserveRecoveryRequestType) As CheckReserveRecoveryResponseType Implements IPureClaimService.CheckReserveRecovery

        Try
            Dim sUserName As String = oRequest.LoginUserName
            Dim nAgentKey As Integer
            Dim nUserId As Integer

            CommonFunctions.GetIdentity(sUserName, nAgentKey, nUserId)
            CommonFunctions.CheckSecurityToken(oRequest.WCFSecurityToken)

            Dim oResponse As New CheckReserveRecoveryResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.CheckReserveRecoveryRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.CheckReserveRecoveryResponseType = Nothing

            oImpRequest.BranchCode = oRequest.BranchCode
            oImpRequest.ClaimKey = oRequest.ClaimKey

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.CheckReserveRecovery(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                oResponse.CurrentReserve = oImpResponse.CurrentReserve
                oResponse.CurrentRecovery = oImpResponse.CurrentRecovery
                oResponse.ClaimStatus = oImpResponse.ClaimStatus

                Return oResponse

            Catch ex As Exception
                BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sValue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPaymentName(ByVal sValue As String) As String
        Dim sReturn As String = sValue
        If Not String.IsNullOrEmpty(sValue) AndAlso Trim(Convert.ToString(sValue)) <> "" AndAlso Len(sValue) > 60 Then
            sReturn = Left(sValue, 60)
        End If
        Return sReturn
    End Function
#Region "Delete Abandon Claim"
    ''' <summary>
    ''' This method is used to delete the anandon claims
    ''' </summary>
    ''' <param name="oDeletAbandonClaimRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeleteAbandonClaim(ByVal oDeletAbandonClaimRequest As DeleteAbandonClaimRequestType) As DeleteAbandonClaimResponseType Implements IPureClaimService.DeleteAbandonClaim

        Try
            Dim sUserName As String = oDeletAbandonClaimRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMCLMD", iUserId)
            CommonFunctions.CheckSecurityToken(oDeletAbandonClaimRequest.WCFSecurityToken)
            Dim oResponse As New DeleteAbandonClaimResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oDeletAbandonClaimRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.DeleteAbandonClaimRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.DeleteAbandonClaimResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oDeletAbandonClaimRequest.BranchCode
            oImpRequest.ClaimKey = oDeletAbandonClaimRequest.ClaimKey

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.DeletAbandonClaim(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oDeletAbandonClaimRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oDeletAbandonClaimRequest))
            Return Nothing
        End Try

    End Function
#End Region
End Class
