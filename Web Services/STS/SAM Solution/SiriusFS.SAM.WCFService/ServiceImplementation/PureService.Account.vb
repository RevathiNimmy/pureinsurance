Imports SiriusFS.SAM.Structure
Imports SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.WCF
Imports Internal = SiriusFS.SAM.Structure.BaseImplementationTypes
Imports Sirius.Architecture.ExceptionHandling
Imports Sirius.Architecture.ExceptionHandling.Handler
Imports SiriusFS.SAM.CoreImplementation
Imports Sirius.Architecture.Data
Imports Sirius.Architecture.Utility
Imports Sirius.Architecture.Configuration.Database
Imports System.Xml.Serialization
Imports System.Linq
Imports Sirius.Architecture.Security
Imports System.ServiceModel.Activation
Imports System.Web.Services.Protocols

Partial Public Class PureService
    Implements IPureAccountService
    ''' <summary>  
    ''' This web services is used to create manual journal.
    ''' </summary>  
    ''' <param name="oAddJournalRequest">An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.AddJournalRequestType</param>  
    ''' <remarks></remarks>
    Public Function AddJournal(ByVal oAddJournalRequest As AddJournalRequestType) As AddJournalResponseType Implements IPureAccountService.AddJournal
        Try

            Dim sUserName As String = oAddJournalRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            if oAddJournalRequest.ManualJournalId >0
                CommonFunctions.CheckAuthority("SAMMJAUT", iUserId)
                Else
                CommonFunctions.CheckAuthority("SAMGPty", iUserId)
            End If
            
            CommonFunctions.CheckSecurityToken(oAddJournalRequest.WCFSecurityToken)
            Dim oResponse As New AddJournalResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oAddJournalRequest.BranchCode)
            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.AddJournalRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.AddJournalResponseType = Nothing



            Dim iCount As Integer = 0
            oImpRequest.BranchCode = oAddJournalRequest.BranchCode
            oImpRequest.JournalTypeCode = oAddJournalRequest.JournalTypeCode
            oImpRequest.JournalBranchCode = oAddJournalRequest.JournalBranchCode
            oImpRequest.JournalDate = oAddJournalRequest.JournalDate
            oImpRequest.JournalDateSpecified = oAddJournalRequest.JournalDateSpecified
            oImpRequest.Comment = oAddJournalRequest.Comment
            oImpRequest.JournalSubBranchCode = oAddJournalRequest.JournalSubBranchCode
            oImpRequest.Is_reffered = oAddJournalRequest.Is_reffered
            oImpRequest.ManualJournalId = oAddJournalRequest.ManualJournalId
            oImpRequest.Approved = oAddJournalRequest.Approved
            If (oAddJournalRequest.RecurringDetails IsNot Nothing) Then
                oImpRequest.RecurringDetails = New BaseImplementationTypes.BaseAddJournalRecurringDetails
                oImpRequest.RecurringDetails.Occurs = oAddJournalRequest.RecurringDetails.Occurs
                oImpRequest.RecurringDetails.PerPeriodOnDay = oAddJournalRequest.RecurringDetails.PerPeriodOnDay
                oImpRequest.RecurringDetails.PerPeriodOnDaySpecified = oAddJournalRequest.RecurringDetails.PerPeriodOnDaySpecified
                oImpRequest.RecurringDetails.PerPeriodOnMonth = oAddJournalRequest.RecurringDetails.PerPeriodOnMonth
                oImpRequest.RecurringDetails.PerPeriodOnMonthSpecified = oAddJournalRequest.RecurringDetails.PerPeriodOnMonthSpecified
                oImpRequest.RecurringDetails.PerQuarterOnDay = oAddJournalRequest.RecurringDetails.PerQuarterOnDay
                oImpRequest.RecurringDetails.PerQuarterOnDaySpecified = oAddJournalRequest.RecurringDetails.PerQuarterOnDaySpecified
            End If
            If (oAddJournalRequest.ReversalDetails IsNot Nothing) Then
                oImpRequest.ReversalDetails = New BaseImplementationTypes.BaseAddJournalReversalDetails
                oImpRequest.ReversalDetails.ReversesOn = oAddJournalRequest.ReversalDetails.ReversesOn
            End If

            If (oAddJournalRequest.Transactions IsNot Nothing) Then
                If (oAddJournalRequest.Transactions.Count > 0) Then
                    ReDim Preserve oImpRequest.Transactions(oAddJournalRequest.Transactions.Count - 1)
                    For iCount = 0 To oAddJournalRequest.Transactions.Count - 1
                        With oAddJournalRequest
                            oImpRequest.Transactions(iCount) = New BaseImplementationTypes.BaseAddJournalTransaction
                            oImpRequest.Transactions(iCount).AccountCode = .Transactions(iCount).AccountCode
                            oImpRequest.Transactions(iCount).CurrencyCode = .Transactions(iCount).CurrencyCode
                            oImpRequest.Transactions(iCount).Amount = .Transactions(iCount).Amount
                            oImpRequest.Transactions(iCount).AltReference = .Transactions(iCount).AltReference
                            oImpRequest.Transactions(iCount).Comment = .Transactions(iCount).Comment
                            oImpRequest.Transactions(iCount).UnderwritingYear = .Transactions(iCount).UnderwritingYear
                            oImpRequest.Transactions(iCount).CostCentreCode = .Transactions(iCount).CostCentreCode
                            oImpRequest.Transactions(iCount).InsuranceRef = .Transactions(iCount).InsuranceRef
                            oImpRequest.Transactions(iCount).PurchaseOrderNumber = .Transactions(iCount).PurchaseOrderNumber
                            oImpRequest.Transactions(iCount).PurchaseInvoiceNumber = .Transactions(iCount).PurchaseInvoiceNumber
                            oImpRequest.Transactions(iCount).ManualJournalDetailId = .Transactions(iCount).ManualJournalDetailId
                        End With
                    Next iCount
                End If
            End If

            Try
                'Call the implementation method
                oImpResponse = oBusiness.AddJournal(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                If (oImpResponse.DocumentRef.Length > 0) Then
                    oResponse.DocumentRef = oImpResponse.DocumentRef.ToList()
                Else
                    oResponse.DocumentRef = Nothing
                End If
                oResponse.ManualJournalId = oImpResponse.ManualJournalId


            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oAddJournalRequest))
            End Try

            Return oResponse

        Catch ex As Exception

            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oAddJournalRequest))
            Return Nothing
        End Try
    End Function

    ''' <summary>  
    ''' This web services method is used to Get Insurer Payments
    ''' </summary>  
    ''' <param name="oAddWriteOffRequest">An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.AddWriteOffRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.AddWriteOffResponseType</returns>  
    Public Function AddWriteOff(ByVal oAddWriteOffRequest As AddWriteOffRequestType) As AddWriteOffResponseType Implements IPureAccountService.AddWriteOff

        Try

            Dim sUserName As String = oAddWriteOffRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New AddWriteOffResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oAddWriteOffRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.AddWriteOffRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.AddWriteOffResponseType = Nothing

            ' Pass the values to the implementation request 
            oImpRequest.BranchCode = oAddWriteOffRequest.BranchCode
            oImpRequest.DocumentKey = oAddWriteOffRequest.DocumentKey
            oImpRequest.AccountKey = oAddWriteOffRequest.AccountKey
            oImpRequest.WriteOffAmount = oAddWriteOffRequest.WriteOffAmount

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.AddWriteOff(oImpRequest)

                oResponse.TransactionKey = oImpResponse.TransactionKey
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oAddWriteOffRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oAddWriteOffRequest))
            Return Nothing
        End Try

    End Function

    Public Function ApproveCashListItem(ByVal oApproveCashListItemRequest As ApproveCashListItemRequestType) As ApproveCashListItemResponseType Implements IPureClaimService.ApproveCashListItem

        Try

            Dim sUserName As String = oApproveCashListItemRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMCLIApr", iUserId)
            CommonFunctions.CheckSecurityToken(oApproveCashListItemRequest.WCFSecurityToken)
            Dim oResponse As New ApproveCashListItemResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oApproveCashListItemRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.ApproveCashListItemRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.ApproveCashListItemResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oApproveCashListItemRequest.BranchCode
            oImpRequest.CashListItemKey = oApproveCashListItemRequest.CashListItemKey
            oImpRequest.Comments = oApproveCashListItemRequest.Comments
            oImpRequest.Declined = oApproveCashListItemRequest.Declined
            oImpRequest.TimeStamp = oApproveCashListItemRequest.TimeStamp
            oImpRequest.CheckValidationOnly = oApproveCashListItemRequest.CheckValidationOnly

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.ApproveCashListItem(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'Set the response values here
                oResponse.TimeStamp = oImpResponse.TimeStamp

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oApproveCashListItemRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oApproveCashListItemRequest))
            Return Nothing
        End Try

    End Function

    Public Function CreatePaymentCashListItem(ByVal oCreatePaymentCashListItemRequest As CreatePaymentCashListItemRequestType) As CreatePaymentCashListItemResponseType Implements IPureClaimService.CreatePaymentCashListItem

        Try

            Dim sUserName As String = oCreatePaymentCashListItemRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMCPCLI", iUserId)
            CommonFunctions.CheckSecurityToken(oCreatePaymentCashListItemRequest.WCFSecurityToken)
            Dim oResponse As New CreatePaymentCashListItemResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oCreatePaymentCashListItemRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.CreatePaymentCashListItemRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.CreatePaymentCashListItemResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oCreatePaymentCashListItemRequest.BranchCode
            oImpRequest.CashListKey = oCreatePaymentCashListItemRequest.CashListKey

            With oCreatePaymentCashListItemRequest
                If .PaymentItem IsNot Nothing Then
                    Dim oPaymentItem() As BaseImplementationTypes.BasePaymentCashListItemType
                    ReDim oPaymentItem(.PaymentItem.Count - 1)
                    Dim oItem As BaseImplementationTypes.BasePaymentCashListItemType
                    For cntItems As Integer = 0 To .PaymentItem.Count - 1
                        If .PaymentItem(cntItems) IsNot Nothing Then
                            oItem = New BaseImplementationTypes.BasePaymentCashListItemType
                            With .PaymentItem(cntItems)
                                oItem.StatusCode = .StatusCode
                                oItem.TypeCode = .TypeCode
                                oItem.TransactionDate = .TransactionDate
                                oItem.MediaTypeCode = .MediaTypeCode
                                oItem.MediaReference = .MediaReference
                                oItem.OurReference = .OurReference
                                oItem.TheirReference = .TheirReference
                                oItem.AccountShortCode = .AccountShortCode
                                oItem.AllocationStatusCode = .AllocationStatusCode
                                oItem.Amount = .Amount
                                oItem.ContactName = .ContactName
                                oItem.FurtherDetails = .FurtherDetails

                                oItem.IsProduceDocument = .IsProduceDocument
                                oItem.BankReference = .BankReference

                                'Address Details
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

                                'Bank Details
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

                                If .Policies IsNot Nothing Then
                                    Dim icount As Integer = 0
                                    For Each pol As BasePaymentCashListItemTypePolicies In .Policies
                                        ReDim Preserve oItem.Policies(.Policies.Count - 1)
                                        oItem.Policies(icount) = New BaseImplementationTypes.BasePaymentCashListItemTypePolicies
                                        oItem.Policies(icount).AmountTobeAllocated = pol.AmountTobeAllocated
                                        oItem.Policies(icount).DocumentRef = pol.DocumentRef
                                        oItem.Policies(icount).InsuranceFileKey = pol.InsuranceFileKey
                                        oItem.Policies(icount).IsCurrencyWriteOff = pol.IsCurrencyWriteOff
                                        oItem.Policies(icount).WriteOffAmount = pol.WriteOffAmount
                                        oItem.Policies(icount).WriteOffReasonKey = pol.WriteOffReasonKey
                                        icount = icount + 1
                                    Next
                                End If

                                'Credit Card Details
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
                    oImpRequest.PaymentItem = oPaymentItem
                End If

            End With

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.CreatePaymentCashListItem(oImpRequest)

                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.CashListItemKey = oImpResponse.CashListItemKey.ToList()

                oResponse.TransDetailKey = oImpResponse.TransDetailKey.ToList()


            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oCreatePaymentCashListItemRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oCreatePaymentCashListItemRequest))
            Return Nothing
        End Try

    End Function

    Public Function CreatePaymentCashListWithItems(ByVal oCreatePaymentCashListWithItemsRequest As CreatePaymentCashListWithItemsRequestType) As CreatePaymentCashListWithItemsResponseType Implements IPureAccountService.CreatePaymentCashListWithItems

        Try

            Dim sUserName As String = oCreatePaymentCashListWithItemsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMCPCL", iUserId)
            CommonFunctions.CheckSecurityToken(oCreatePaymentCashListWithItemsRequest.WCFSecurityToken)
            Dim oResponse As New CreatePaymentCashListWithItemsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oCreatePaymentCashListWithItemsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.CreatePaymentCashListWithItemsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.CreatePaymentCashListWithItemsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oCreatePaymentCashListWithItemsRequest.BranchCode

            Dim oCashList As New BaseImplementationTypes.BasePaymentCashListType
            With oCreatePaymentCashListWithItemsRequest.PaymentCashList
                oCashList.BranchCode = oCreatePaymentCashListWithItemsRequest.BranchCode
                oCashList.Reference = .Reference
                oCashList.TypeCode = .TypeCode
                oCashList.BankAccountCode = .BankAccountCode
                oCashList.BankAccountName = .bankAccountName
                oCashList.CurrencyCode = .CurrencyCode
                oCashList.ListDate = .ListDate
                oCashList.StatusCode = .StatusCode
                oCashList.BankAccountKey = .BankAccountKey
                oCashList.CashListKey = .cashListKey
                oCashList.SubBranchCode = .SubBranchCode
                'Payment items
                If .PaymentItem IsNot Nothing Then
                    Dim oPaymentItem() As BaseImplementationTypes.BasePaymentCashListItemType
                    ReDim oPaymentItem(.PaymentItem.Count - 1)
                    Dim oItem As BaseImplementationTypes.BasePaymentCashListItemType
                    For cntItems As Integer = 0 To .PaymentItem.Count - 1
                        If .PaymentItem(cntItems) IsNot Nothing Then
                            oItem = New BaseImplementationTypes.BasePaymentCashListItemType
                            With .PaymentItem(cntItems)
                                oItem.StatusCode = .StatusCode
                                oItem.TypeCode = .TypeCode
                                oItem.TransactionDate = .TransactionDate
                                oItem.MediaTypeCode = .MediaTypeCode
                                oItem.MediaReference = .MediaReference
                                oItem.OurReference = .OurReference
                                oItem.TheirReference = .TheirReference
                                oItem.AccountShortCode = .AccountShortCode
                                oItem.AllocationStatusCode = .AllocationStatusCode
                                oItem.Amount = .Amount
                                oItem.Amount = .Amount
                                oItem.Amount_Tendered = .Amount_Tendered
                                oItem.Original_Amount = .Original_Amount
                                oItem.Collection_Date = .Collection_Date
                                oItem.IsProduceDocument = .IsProduceDocument
                                oItem.BankReference = .BankReference
                                oItem.ContactName = .ContactName
                                oItem.FurtherDetails = .FurtherDetails
                                If .AllocationDetails IsNot Nothing Then
                                    oItem.AllocationDetails = New BaseImplementationTypes.BaseAllocationType
                                    oItem.AllocationDetails.AutoAllocate = .AllocationDetails.AutoAllocate
                                End If
                                oItem.SkipPosting = .SkipPosting
                                oItem.TaxBandCode = .TaxBandCode
                                oItem.TaxAmount = .TaxAmount
                                oItem.CashListItemKey = .CashListItemKey
                                oItem.IsViaBulkClaimPayment = .IsViaBulkClaimPayment
                                'Address Details
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

                                'Bank Details
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
                                If .Policies IsNot Nothing Then
                                    Dim icount As Integer = 0
                                    For Each pol As BasePaymentCashListItemTypePolicies In .Policies
                                        ReDim Preserve oItem.Policies(.Policies.Count - 1)

                                        oItem.Policies(icount) = New BaseImplementationTypes.BasePaymentCashListItemTypePolicies
                                        oItem.Policies(icount).AmountTobeAllocated = pol.AmountTobeAllocated
                                        oItem.Policies(icount).DocumentRef = pol.DocumentRef
                                        oItem.Policies(icount).InsuranceFileKey = pol.InsuranceFileKey
                                        oItem.Policies(icount).IsCurrencyWriteOff = pol.IsCurrencyWriteOff
                                        oItem.Policies(icount).WriteOffAmount = pol.WriteOffAmount
                                        oItem.Policies(icount).WriteOffReasonKey = pol.WriteOffReasonKey
                                        icount = icount + 1
                                    Next
                                End If

                                If .Policies IsNot Nothing Then
                                    Dim icount As Integer = 0
                                    For Each pol As BasePaymentCashListItemTypePolicies In .Policies
                                        ReDim Preserve oItem.Policies(.Policies.Count - 1)

                                        oItem.Policies(icount) = New BaseImplementationTypes.BasePaymentCashListItemTypePolicies
                                        oItem.Policies(icount).AmountTobeAllocated = pol.AmountTobeAllocated
                                        oItem.Policies(icount).DocumentRef = pol.DocumentRef
                                        oItem.Policies(icount).InsuranceFileKey = pol.InsuranceFileKey
                                        oItem.Policies(icount).IsCurrencyWriteOff = pol.IsCurrencyWriteOff
                                        oItem.Policies(icount).WriteOffAmount = pol.WriteOffAmount
                                        oItem.Policies(icount).WriteOffReasonKey = pol.WriteOffReasonKey
                                        icount = icount + 1
                                    Next
                                End If

                                'Credit Card Details
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

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.CreatePaymentCashListWithItems(oImpRequest)

                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                oResponse.CashListKey = oImpResponse.CashListKey
                If (oImpResponse IsNot Nothing) AndAlso oImpResponse.CashListItem IsNot Nothing AndAlso oImpResponse.CashListItem.Count > 0 Then
                    oResponse.CashListKey = oImpResponse.CashListKey
                    oResponse.CashListItem = oImpResponse.CashListItem.ToList().ConvertAll(
                        New Converter(Of BaseImplementationTypes.BaseCreatePaymentCashListWithItemsResponseTypeCashListItem, BaseCreatePaymentCashListWithItemsResponseTypeCashListItem) _
                        (AddressOf CommonFunctions.ToServiceCreatePaymentCashListWithItemsList))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oCreatePaymentCashListWithItemsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oCreatePaymentCashListWithItemsRequest))
            Return Nothing
        End Try

    End Function


    Public Function CreateReceiptCashListItem(ByVal oCreateReceiptCashListItemsRequest As CreateReceiptCashListItemRequestType) As CreateReceiptCashListItemResponseType Implements IPureClaimService.CreateReceiptCashListItem

        Try

            Dim sUserName As String = oCreateReceiptCashListItemsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMCRCLI", iUserId)
            CommonFunctions.CheckSecurityToken(oCreateReceiptCashListItemsRequest.WCFSecurityToken)
            Dim oResponse As New CreateReceiptCashListItemResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oCreateReceiptCashListItemsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.CreateReceiptCashListItemRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.CreateReceiptCashListItemResponseType = Nothing

            ' Pass the values to the implementation request structure
            If (oCreateReceiptCashListItemsRequest IsNot Nothing) Then
                oImpRequest.BranchCode = oCreateReceiptCashListItemsRequest.BranchCode
                oImpRequest.CashListKey = oCreateReceiptCashListItemsRequest.CashListKey
                Dim iRecordCount As Integer = 0
                If (oCreateReceiptCashListItemsRequest.ReceiptCashListItem IsNot Nothing) Then
                    For Each ReceiptItem As BaseReceiptCashListItemType In oCreateReceiptCashListItemsRequest.ReceiptCashListItem
                        With oCreateReceiptCashListItemsRequest.ReceiptCashListItem(iRecordCount)
                            ReDim Preserve oImpRequest.ReceiptCashListItem(iRecordCount)
                            oImpRequest.ReceiptCashListItem(iRecordCount) = New BaseImplementationTypes.BaseReceiptCashListItemType
                            oImpRequest.ReceiptCashListItem(iRecordCount).AccountShortCode = .AccountShortCode
                            oImpRequest.ReceiptCashListItem(iRecordCount).AllocationStatusCode = .AllocationStatusCode
                            oImpRequest.ReceiptCashListItem(iRecordCount).Amount = .Amount

                            oImpRequest.ReceiptCashListItem(iRecordCount).IsProduceDocument = .IsProduceDocument
                            oImpRequest.ReceiptCashListItem(iRecordCount).BankReference = .BankReference

                            oImpRequest.ReceiptCashListItem(iRecordCount).ContactName = .ContactName
                            oImpRequest.ReceiptCashListItem(iRecordCount).MediaReference = .MediaReference
                            oImpRequest.ReceiptCashListItem(iRecordCount).MediaTypeCode = .MediaTypeCode
                            oImpRequest.ReceiptCashListItem(iRecordCount).OurReference = .OurReference
                            oImpRequest.ReceiptCashListItem(iRecordCount).StatusCode = .StatusCode
                            oImpRequest.ReceiptCashListItem(iRecordCount).TheirReference = .TheirReference
                            oImpRequest.ReceiptCashListItem(iRecordCount).TransactionDate = .TransactionDate
                            oImpRequest.ReceiptCashListItem(iRecordCount).TypeCode = .TypeCode

                            Dim iInstallmentCount As Integer = 0
                            Dim iInstallmentDetailCount As Integer = 0
                            If oCreateReceiptCashListItemsRequest.ReceiptCashListItem(iRecordCount).InstalmentPlanDetails IsNot Nothing Then
                                For iInstallmentCount = 0 To oCreateReceiptCashListItemsRequest.ReceiptCashListItem(iRecordCount).InstalmentPlanDetails.Count - 1
                                    With oCreateReceiptCashListItemsRequest.ReceiptCashListItem(iRecordCount).InstalmentPlanDetails(iInstallmentCount)
                                        ReDim Preserve oImpRequest.ReceiptCashListItem(iRecordCount).InstalmentPlanDetails(iInstallmentCount)
                                        oImpRequest.ReceiptCashListItem(iRecordCount).InstalmentPlanDetails(iInstallmentCount) = New BaseImplementationTypes.BaseCoreCashListItemTypeInstalmentPlanDetails
                                        oImpRequest.ReceiptCashListItem(iRecordCount).InstalmentPlanDetails(iInstallmentCount).FinancePlanKey = .FinancePlanKey
                                        oImpRequest.ReceiptCashListItem(iRecordCount).InstalmentPlanDetails(iInstallmentCount).FinancePlanVersion = .FinancePlanVersion
                                        If oCreateReceiptCashListItemsRequest.ReceiptCashListItem(iRecordCount).InstalmentPlanDetails(iInstallmentCount).InstalmentDetails IsNot Nothing Then
                                            For iInstallmentDetailCount = 0 To oCreateReceiptCashListItemsRequest.ReceiptCashListItem(iRecordCount).InstalmentPlanDetails(iInstallmentCount).InstalmentDetails.Count - 1
                                                With oCreateReceiptCashListItemsRequest.ReceiptCashListItem(iRecordCount).InstalmentPlanDetails(iInstallmentCount).InstalmentDetails(iInstallmentDetailCount)
                                                    ReDim Preserve oImpRequest.ReceiptCashListItem(iRecordCount).InstalmentPlanDetails(iInstallmentCount).InstalmentDetails(iInstallmentDetailCount)
                                                    oImpRequest.ReceiptCashListItem(iRecordCount).InstalmentPlanDetails(iInstallmentCount).InstalmentDetails(iInstallmentDetailCount) = New BaseImplementationTypes.BaseCoreCashListItemTypeInstalmentPlanDetailsInstalmentDetails
                                                    oImpRequest.ReceiptCashListItem(iRecordCount).InstalmentPlanDetails(iInstallmentCount).InstalmentDetails(iInstallmentDetailCount).Amount = .Amount
                                                    oImpRequest.ReceiptCashListItem(iRecordCount).InstalmentPlanDetails(iInstallmentCount).InstalmentDetails(iInstallmentDetailCount).InstalmentNumber = .InstalmentNumber
                                                End With

                                            Next
                                        End If
                                    End With

                                Next
                            End If

                            If (.CreditCard IsNot Nothing) Then
                                oImpRequest.ReceiptCashListItem(iRecordCount).CreditCard = New BaseImplementationTypes.BaseCreditCardType
                                With .CreditCard
                                    oImpRequest.ReceiptCashListItem(iRecordCount).CreditCard.AuthCode = .AuthCode
                                    oImpRequest.ReceiptCashListItem(iRecordCount).CreditCard.CustomerPresent = .CustomerPresent
                                    oImpRequest.ReceiptCashListItem(iRecordCount).CreditCard.ExpiryDate = .ExpiryDate
                                    oImpRequest.ReceiptCashListItem(iRecordCount).CreditCard.Issue = .Issue
                                    oImpRequest.ReceiptCashListItem(iRecordCount).CreditCard.ManualAuthCode = .ManualAuthCode
                                    oImpRequest.ReceiptCashListItem(iRecordCount).CreditCard.NameOnCreditCard = .NameOnCreditCard
                                    oImpRequest.ReceiptCashListItem(iRecordCount).CreditCard.Number = .Number
                                    oImpRequest.ReceiptCashListItem(iRecordCount).CreditCard.Pin = .Pin
                                    oImpRequest.ReceiptCashListItem(iRecordCount).CreditCard.StartDate = .StartDate
                                    oImpRequest.ReceiptCashListItem(iRecordCount).CreditCard.TransactionCode = .TransactionCode
                                    oImpRequest.ReceiptCashListItem(iRecordCount).CreditCard.TypeCode = .TypeCode

                                    oImpRequest.ReceiptCashListItem(iRecordCount).CreditCard.TrackingNumber = .TrackingNumber

                                    oImpRequest.ReceiptCashListItem(iRecordCount).CreditCard.PartyBankKey = .PartyBankKey

                                    If (.CardHolder IsNot Nothing) Then
                                        oImpRequest.ReceiptCashListItem(iRecordCount).CreditCard.CardHolder = New BaseImplementationTypes.BaseCreditCardTypeCardHolder
                                        With .CardHolder
                                            oImpRequest.ReceiptCashListItem(iRecordCount).CreditCard.CardHolder.AddressLine1 = .AddressLine1
                                            oImpRequest.ReceiptCashListItem(iRecordCount).CreditCard.CardHolder.AddressLine2 = .AddressLine2
                                            oImpRequest.ReceiptCashListItem(iRecordCount).CreditCard.CardHolder.AddressLine3 = .AddressLine3
                                            oImpRequest.ReceiptCashListItem(iRecordCount).CreditCard.CardHolder.AddressLine4 = .AddressLine4
                                            oImpRequest.ReceiptCashListItem(iRecordCount).CreditCard.CardHolder.CountryCode = .CountryCode
                                            oImpRequest.ReceiptCashListItem(iRecordCount).CreditCard.CardHolder.Name = .Name
                                            oImpRequest.ReceiptCashListItem(iRecordCount).CreditCard.CardHolder.PostCode = .PostCode
                                        End With
                                    End If
                                End With
                            End If
                            If (.Bank IsNot Nothing) Then
                                oImpRequest.ReceiptCashListItem(iRecordCount).Bank = New BaseImplementationTypes.BaseBankReceiptType
                                With .Bank
                                    oImpRequest.ReceiptCashListItem(iRecordCount).Bank.BankCode = .BankCode
                                    oImpRequest.ReceiptCashListItem(iRecordCount).Bank.ChequeDate = .ChequeDate
                                    oImpRequest.ReceiptCashListItem(iRecordCount).Bank.PayerName = .PayerName

                                    oImpRequest.ReceiptCashListItem(iRecordCount).Bank.PartyBankKey = .PartyBankKey

                                End With
                            End If
                            If (.Policies IsNot Nothing) Then
                                Dim icount As Integer = 0
                                For Each pol As BaseReceiptCashListItemTypePolicies In .Policies
                                    ReDim Preserve oImpRequest.ReceiptCashListItem(iRecordCount).Policies(icount)
                                    oImpRequest.ReceiptCashListItem(iRecordCount).Policies(icount) = New BaseImplementationTypes.BaseReceiptCashListItemTypePolicies
                                    oImpRequest.ReceiptCashListItem(iRecordCount).Policies(icount).AmountTobeAllocated = pol.AmountTobeAllocated
                                    oImpRequest.ReceiptCashListItem(iRecordCount).Policies(icount).BGKey = pol.BGKey
                                    oImpRequest.ReceiptCashListItem(iRecordCount).Policies(icount).DocumentRef = pol.DocumentRef
                                    oImpRequest.ReceiptCashListItem(iRecordCount).Policies(icount).InsuranceFileKey = pol.InsuranceFileKey
                                    oImpRequest.ReceiptCashListItem(iRecordCount).Policies(icount).IsCurrencyWriteOff = pol.IsCurrencyWriteOff
                                    oImpRequest.ReceiptCashListItem(iRecordCount).Policies(icount).WriteOffAmount = pol.WriteOffAmount
                                    oImpRequest.ReceiptCashListItem(iRecordCount).Policies(icount).WriteOffReasonKey = pol.WriteOffReasonKey
                                    icount = icount + 1
                                Next

                            End If
                            If (.ContactAddress IsNot Nothing) Then
                                oImpRequest.ReceiptCashListItem(iRecordCount).ContactAddress = New BaseImplementationTypes.BaseSimpleAddressType
                                With .ContactAddress
                                    oImpRequest.ReceiptCashListItem(iRecordCount).ContactAddress.AddressLine1 = .AddressLine1
                                    oImpRequest.ReceiptCashListItem(iRecordCount).ContactAddress.AddressLine2 = .AddressLine2
                                    oImpRequest.ReceiptCashListItem(iRecordCount).ContactAddress.AddressLine3 = .AddressLine3
                                    oImpRequest.ReceiptCashListItem(iRecordCount).ContactAddress.AddressLine4 = .AddressLine4
                                    oImpRequest.ReceiptCashListItem(iRecordCount).ContactAddress.CountryCode = .CountryCode
                                    oImpRequest.ReceiptCashListItem(iRecordCount).ContactAddress.PostCode = .PostCode
                                End With
                                oImpRequest.ReceiptCashListItem(iRecordCount).FurtherDetails = .FurtherDetails
                            End If
                            iRecordCount += 1
                        End With
                    Next
                End If
            End If
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.CreateReceiptCashListItem(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                If oImpResponse IsNot Nothing AndAlso oImpResponse.CashListItem IsNot Nothing AndAlso oImpResponse.CashListItem.Count > 0 Then
                    oResponse.CashListItem = oImpResponse.CashListItem.ToList().ConvertAll(
                        New Converter(Of BaseImplementationTypes.BaseCreateReceiptCashListItemResponseTypeCashListItem, BaseCreateReceiptCashListItemResponseTypeCashListItem) _
                        (AddressOf CommonFunctions.ToServiceCreateReceiptCashListItemList))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oCreateReceiptCashListItemsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oCreateReceiptCashListItemsRequest))
            Return Nothing
        End Try

    End Function

    Public Function CreateReceiptCashListWithItems(ByVal oCreateReceiptCashListWithItemsRequest As CreateReceiptCashListWithItemsRequestType) As CreateReceiptCashListWithItemsResponseType Implements IPureAccountService.CreateReceiptCashListWithItems, IPureClaimService.CreateReceiptCashListWithItems
        Try

            Dim sUserName As String = oCreateReceiptCashListWithItemsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMCRCL", iUserId)
            CommonFunctions.CheckSecurityToken(oCreateReceiptCashListWithItemsRequest.WCFSecurityToken)
            Dim oResponse As New CreateReceiptCashListWithItemsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oCreateReceiptCashListWithItemsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.CreateReceiptCashListWithItemsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.CreateReceiptCashListWithItemsResponseType = Nothing

            ' Pass the values to the implementation request structure
            If (oCreateReceiptCashListWithItemsRequest IsNot Nothing) Then
                oImpRequest.BranchCode = oCreateReceiptCashListWithItemsRequest.BranchCode
                If (oCreateReceiptCashListWithItemsRequest.ReceiptCashList IsNot Nothing) Then
                    oImpRequest.ReceiptCashList = New BaseImplementationTypes.BaseReceiptCashListType
                    With oCreateReceiptCashListWithItemsRequest.ReceiptCashList
                        oImpRequest.ReceiptCashList.BankAccountCode = .BankAccountCode
                        oImpRequest.ReceiptCashList.CurrencyCode = .CurrencyCode
                        oImpRequest.ReceiptCashList.ListDate = .ListDate
                        oImpRequest.ReceiptCashList.StatusCode = .StatusCode
                        oImpRequest.ReceiptCashList.TypeCode = .TypeCode
                        oImpRequest.ReceiptCashList.Reference = .Reference
                        oImpRequest.ReceiptCashList.BankAccountKey = .BankAccountKey
                        oImpRequest.ReceiptCashList.SubBranchCode = .SubBranchCode
                    End With
                End If
                Dim iRecordCount As Integer = 0
                If (oCreateReceiptCashListWithItemsRequest.ReceiptCashList.ReceiptItem IsNot Nothing) Then
                    For Each ReceiptItem As BaseReceiptCashListItemType In oCreateReceiptCashListWithItemsRequest.ReceiptCashList.ReceiptItem
                        With oCreateReceiptCashListWithItemsRequest.ReceiptCashList.ReceiptItem(iRecordCount)
                            ReDim Preserve oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount)
                            oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount) = New BaseImplementationTypes.BaseReceiptCashListItemType
                            oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).AccountShortCode = .AccountShortCode
                            oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).AllocationStatusCode = .AllocationStatusCode
                            oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).Amount = .Amount
                            oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).Amount_Tendered = .Amount_Tendered
                            oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).Original_Amount = .Original_Amount

                            oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).IsProduceDocument = .IsProduceDocument
                            oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).BankReference = .BankReference

                            oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).ContactName = .ContactName
                            oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).MediaReference = .MediaReference
                            oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).MediaTypeCode = .MediaTypeCode
                            oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).OurReference = .OurReference
                            oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).StatusCode = .StatusCode
                            oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).TheirReference = .TheirReference
                            oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).TransactionDate = .TransactionDate
                            oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).TypeCode = .TypeCode

                            If .AllocationDetails IsNot Nothing Then
                                oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).AllocationDetails = New BaseImplementationTypes.BaseAllocationType
                                oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).AllocationDetails.AutoAllocate = .AllocationDetails.AutoAllocate
                            End If

                            Dim iInstallmentCount As Integer = 0
                            Dim iInstallmentDetailCount As Integer = 0
                            If oCreateReceiptCashListWithItemsRequest.ReceiptCashList.ReceiptItem(iRecordCount).InstalmentPlanDetails IsNot Nothing Then

                                For iInstallmentCount = 0 To oCreateReceiptCashListWithItemsRequest.ReceiptCashList.ReceiptItem(iRecordCount).InstalmentPlanDetails.Count - 1
                                    With oCreateReceiptCashListWithItemsRequest.ReceiptCashList.ReceiptItem(iRecordCount).InstalmentPlanDetails(iInstallmentCount)
                                        ReDim Preserve oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).InstalmentPlanDetails(iInstallmentCount)
                                        oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).InstalmentPlanDetails(iInstallmentCount) = New BaseImplementationTypes.BaseCoreCashListItemTypeInstalmentPlanDetails
                                        oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).InstalmentPlanDetails(iInstallmentCount).FinancePlanKey = .FinancePlanKey
                                        oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).InstalmentPlanDetails(iInstallmentCount).FinancePlanVersion = .FinancePlanVersion
                                        If oCreateReceiptCashListWithItemsRequest.ReceiptCashList.ReceiptItem(iRecordCount).InstalmentPlanDetails(iInstallmentCount).InstalmentDetails IsNot Nothing Then

                                            For iInstallmentDetailCount = 0 To oCreateReceiptCashListWithItemsRequest.ReceiptCashList.ReceiptItem(iRecordCount).InstalmentPlanDetails(iInstallmentCount).InstalmentDetails.Count - 1
                                                With oCreateReceiptCashListWithItemsRequest.ReceiptCashList.ReceiptItem(iRecordCount).InstalmentPlanDetails(iInstallmentCount).InstalmentDetails(iInstallmentDetailCount)
                                                    ReDim Preserve oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).InstalmentPlanDetails(iInstallmentCount).InstalmentDetails(iInstallmentDetailCount)
                                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).InstalmentPlanDetails(iInstallmentCount).InstalmentDetails(iInstallmentDetailCount) = New BaseImplementationTypes.BaseCoreCashListItemTypeInstalmentPlanDetailsInstalmentDetails
                                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).InstalmentPlanDetails(iInstallmentCount).InstalmentDetails(iInstallmentDetailCount).Amount = .Amount
                                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).InstalmentPlanDetails(iInstallmentCount).InstalmentDetails(iInstallmentDetailCount).InstalmentNumber = .InstalmentNumber
                                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).InstalmentPlanDetails(iInstallmentCount).InstalmentDetails(iInstallmentDetailCount).IsPartialPayment = .IsPartialPayment
                                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).InstalmentPlanDetails(iInstallmentCount).InstalmentDetails(iInstallmentDetailCount).IsWriteOffPayment = .IsWriteOffPayment
                                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).InstalmentPlanDetails(iInstallmentCount).InstalmentDetails(iInstallmentDetailCount).WriteOffReasonID = .WriteOffReasonID
                                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).InstalmentPlanDetails(iInstallmentCount).InstalmentDetails(iInstallmentDetailCount).OverPaymentWriteOffAmount = .OverPaymentWriteOffAmount
                                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).InstalmentPlanDetails(iInstallmentCount).InstalmentDetails(iInstallmentDetailCount).ActualAmount = .ActualAmount
                                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).InstalmentPlanDetails(iInstallmentCount).InstalmentDetails(iInstallmentDetailCount).iPFInstalmentID = .iPFInstalmentID
                                                End With

                                            Next
                                        End If
                                    End With

                                Next
                            End If

                            If .Policies IsNot Nothing Then
                                Dim icount As Integer = 0
                                For Each pol As BaseReceiptCashListItemTypePolicies In .Policies
                                    ReDim Preserve oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).Policies(.Policies.Count - 1)

                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).Policies(icount) = New BaseImplementationTypes.BaseReceiptCashListItemTypePolicies
                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).Policies(icount).AmountTobeAllocated = pol.AmountTobeAllocated '.Policies(icount).AmountTobeAllocated
                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).Policies(icount).BGKey = pol.BGKey
                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).Policies(icount).DocumentRef = pol.DocumentRef
                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).Policies(icount).InsuranceFileKey = pol.InsuranceFileKey
                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).Policies(icount).IsCurrencyWriteOff = pol.IsCurrencyWriteOff
                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).Policies(icount).WriteOffAmount = pol.WriteOffAmount
                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).Policies(icount).WriteOffReasonKey = pol.WriteOffReasonKey
                                    icount = icount + 1
                                Next
                            End If

                            If (.CreditCard IsNot Nothing) Then
                                oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).CreditCard = New BaseImplementationTypes.BaseCreditCardType
                                With .CreditCard
                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).CreditCard.AuthCode = .AuthCode
                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).CreditCard.CustomerPresent = .CustomerPresent
                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).CreditCard.ExpiryDate = .ExpiryDate
                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).CreditCard.Issue = .Issue
                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).CreditCard.ManualAuthCode = .ManualAuthCode
                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).CreditCard.NameOnCreditCard = .NameOnCreditCard
                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).CreditCard.Number = .Number
                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).CreditCard.Pin = .Pin
                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).CreditCard.StartDate = .StartDate
                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).CreditCard.TransactionCode = .TransactionCode
                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).CreditCard.TypeCode = .TypeCode

                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).CreditCard.TrackingNumber = .TrackingNumber

                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).CreditCard.PartyBankKey = .PartyBankKey

                                    If (.CardHolder IsNot Nothing) Then
                                        With .CardHolder
                                            oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).CreditCard.CardHolder = New BaseImplementationTypes.BaseCreditCardTypeCardHolder
                                            oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).CreditCard.CardHolder.AddressLine1 = .AddressLine1
                                            oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).CreditCard.CardHolder.AddressLine2 = .AddressLine2
                                            oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).CreditCard.CardHolder.AddressLine3 = .AddressLine3
                                            oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).CreditCard.CardHolder.AddressLine4 = .AddressLine4
                                            oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).CreditCard.CardHolder.CountryCode = .CountryCode
                                            oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).CreditCard.CardHolder.Name = .Name
                                            oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).CreditCard.CardHolder.PostCode = .PostCode
                                        End With
                                    End If
                                End With
                            End If
                            If (.Bank IsNot Nothing) Then
                                oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).Bank = New BaseImplementationTypes.BaseBankReceiptType
                                With .Bank
                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).Bank.BankCode = .BankCode
                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).Bank.ChequeDate = .ChequeDate
                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).Bank.PayerName = .PayerName

                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).Bank.PartyBankKey = .PartyBankKey

                                End With
                            End If
                            If (.ContactAddress IsNot Nothing) Then
                                oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).ContactAddress = New BaseImplementationTypes.BaseSimpleAddressType
                                With .ContactAddress
                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).ContactAddress.AddressLine1 = .AddressLine1
                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).ContactAddress.AddressLine2 = .AddressLine2
                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).ContactAddress.AddressLine3 = .AddressLine3
                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).ContactAddress.AddressLine4 = .AddressLine4
                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).ContactAddress.CountryCode = .CountryCode
                                    oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).ContactAddress.PostCode = .PostCode
                                End With
                                oImpRequest.ReceiptCashList.ReceiptItem(iRecordCount).FurtherDetails = .FurtherDetails
                            End If
                        End With
                        iRecordCount += 1
                    Next
                End If

            End If
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.CreateReceiptCashListWithItems(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                If oImpResponse IsNot Nothing AndAlso oImpResponse.CashListItem IsNot Nothing AndAlso oImpResponse.CashListItem.Count > 0 Then
                    oResponse.CashListKey = oImpResponse.CashListKey
                    oResponse.CashListItem = oImpResponse.CashListItem.ToList().ConvertAll(
                        New Converter(Of BaseImplementationTypes.BaseCreateReceiptCashListWithItemsResponseTypeCashListItem, BaseCreateReceiptCashListWithItemsResponseTypeCashListItem) _
                        (AddressOf CommonFunctions.ToServiceCreateReceiptCashListWithItemsList))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oCreateReceiptCashListWithItemsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oCreateReceiptCashListWithItemsRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>  
    ''' This method is used to find accounts in claim payments
    '''<param name="FindAccountsRequest" type="FindAccountsRequestType"></param>   
    '''<returns>FindAccountsResponseType</returns>
    '''<remarks></remarks>
    ''' </summary> 
    Public Function FindAccounts(ByVal FindAccountsRequest As FindAccountsRequestType) As FindAccountsResponseType Implements IPureAccountService.FindAccounts, IPureClaimService.FindAccounts, IPurePolicyService.FindAccounts

        Try

            Dim sUserName As String = FindAccountsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMPClm", iUserId)
            CommonFunctions.CheckSecurityToken(FindAccountsRequest.WCFSecurityToken)
            Dim oResponse As New FindAccountsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, FindAccountsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.FindAccountsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.FindAccountsResponseType = Nothing

            ' Pass the values to the implementation request structure, i.e.
            oImpRequest.BranchCode = FindAccountsRequest.BranchCode
            oImpRequest.LedgerCode = FindAccountsRequest.LedgerCode
            oImpRequest.AccountName = FindAccountsRequest.AccountName
            oImpRequest.AccountTypeCode = FindAccountsRequest.AccountTypeCode
            oImpRequest.ShortCode = FindAccountsRequest.ShortCode
            oImpRequest.InsuranceRef = FindAccountsRequest.InsuranceRef
            oImpRequest.OperatorKeySpecified = FindAccountsRequest.OperatorKeySpecified
            oImpRequest.OperatorKey = FindAccountsRequest.OperatorKey
            oImpRequest.PurchaseInvoiceNo = FindAccountsRequest.PurchaseInvoiceNo
            oImpRequest.PurchaseOrderNo = FindAccountsRequest.PurchaseOrderNo
            oImpRequest.Spare = FindAccountsRequest.Spare
            oImpRequest.ShowDeletedSpecified = FindAccountsRequest.ShowDeletedSpecified
            oImpRequest.ShowDeleted = FindAccountsRequest.ShowDeleted
            oImpRequest.ShowBalanceSpecified = FindAccountsRequest.ShowBalanceSpecified
            oImpRequest.ShowBalance = FindAccountsRequest.ShowBalance
            oImpRequest.OnlyUpdatableAccountsSpecified = FindAccountsRequest.OnlyUpdatableAccountsSpecified
            oImpRequest.OnlyUpdatableAccounts = FindAccountsRequest.OnlyUpdatableAccounts
            oImpRequest.IncludeInsurerAgents = FindAccountsRequest.IncludeInsurerAgents
            oImpRequest.ExcludeInsurerAgents = FindAccountsRequest.ExcludeInsurerAgents

            oImpRequest.MaxRowsToFetchSpecified = FindAccountsRequest.MaxRowsToFetchSpecified
            If FindAccountsRequest.MaxRowsToFetchSpecified Then
                oImpRequest.MaxRowsToFetch = FindAccountsRequest.MaxRowsToFetch
            Else
                oImpRequest.MaxRowsToFetch = -1
            End If
            oImpRequest.BrokerCnt = iAgentKey

            oImpRequest.WCFSecurityToken = If(FindAccountsRequest.WCFSecurityToken.Length > 0, FindAccountsRequest.WCFSecurityToken, "WCFSecuritytoken")

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.FindAccounts(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.Accounts = SAMFunc.GetDeserializedValues(Of List(Of BaseFindAccountsResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseFindAccountsResponseTypeAccounts", sConvertToTypeName:="BaseFindAccountsResponseTypeRow")

                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Accounts = DataTabletoList_FindAccounts(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(FindAccountsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(FindAccountsRequest))
            Return Nothing
        End Try

    End Function

    Public Function FindCashDeposit(ByVal oFindCashDepositRequest As FindCashDepositRequestType) As FindCashDepositResponseType Implements IPureAccountService.FindCashDeposit

        Try
            Dim sUserName As String = oFindCashDepositRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMFCD", iUserId)
            CommonFunctions.CheckSecurityToken(oFindCashDepositRequest.WCFSecurityToken)
            Dim oResponse As New FindCashDepositResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oFindCashDepositRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.FindCashDepositRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.FindCashDepositResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oFindCashDepositRequest.BranchCode
            oImpRequest.PartyCode = oFindCashDepositRequest.PartyCode
            oImpRequest.CashDepositRef = oFindCashDepositRequest.CashDepositRef
            oImpRequest.BankCode = oFindCashDepositRequest.BankCode
            oImpRequest.MaxRowsToFetch = oFindCashDepositRequest.MaxRowsToFetch
            oImpRequest.MaxRowsToFetchSpecified = oFindCashDepositRequest.MaxRowsToFetchSpecified

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.FindCashDeposit(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                If (oImpResponse IsNot Nothing) AndAlso oImpResponse.CashDeposit IsNot Nothing AndAlso oImpResponse.CashDeposit.Count > 0 Then
                    oResponse.CashDeposit = oImpResponse.CashDeposit.ToList().ConvertAll(
                        New Converter(Of BaseImplementationTypes.BaseCashDepositItemType, BaseCashDepositItemType)(AddressOf CommonFunctions.ToServiceFindCashDepositList))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oFindCashDepositRequest))
            End Try
            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oFindCashDepositRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetAccountShortCodeFromParty(ByVal oGetAccountShortCodeFromPartyRequest As GetAccountShortCodeFromPartyRequestType, ByVal v_iPartyCnt As Integer) As String Implements IPureAccountService.GetAccountShortCodeFromParty
        Try
            Dim sUserName As String = oGetAccountShortCodeFromPartyRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMCASHPAY", iUserId)
            CommonFunctions.CheckSecurityToken(oGetAccountShortCodeFromPartyRequest.WCFSecurityToken)
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetAccountShortCodeFromPartyRequest.BranchCode)
            Dim sAccountShortCode As String = ""


            ' Call the implementation method
            sAccountShortCode = oBusiness.GetAccountShortCodeFromParty(v_iPartyCnt)

            Return sAccountShortCode

        Catch ex As Exception

            Return ""
        End Try
    End Function


    Public Function FindCashListReceipts(ByVal oFindCashListReceiptsRequest As FindCashListReceiptsRequestType) As FindCashListReceiptsResponseType Implements IPureAccountService.FindCashListReceipts

        Try

            Dim sUserName As String = oFindCashListReceiptsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMFINDCAS", iUserId)
            CommonFunctions.CheckSecurityToken(oFindCashListReceiptsRequest.WCFSecurityToken)
            Dim oResponse As New FindCashListReceiptsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oFindCashListReceiptsRequest.BranchCode)
            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.FindCashListReceiptsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.FindCashListReceiptsResponseType = Nothing

            oImpRequest.BranchCode = oFindCashListReceiptsRequest.BranchCode
            oImpRequest.BankAccountCode = oFindCashListReceiptsRequest.BankAccountCode
            oImpRequest.CollectionDateFromSpecified = oFindCashListReceiptsRequest.CollectionDateFromSpecified
            oImpRequest.CollectionDateFrom = oFindCashListReceiptsRequest.CollectionDateFrom
            oImpRequest.CollectionDateTo = oFindCashListReceiptsRequest.CollectionDateTo
            oImpRequest.CollectionDateToSpecified = oFindCashListReceiptsRequest.CollectionDateToSpecified
            oImpRequest.DocumentRef = oFindCashListReceiptsRequest.DocumentRef
            oImpRequest.DrawnBankCode = oFindCashListReceiptsRequest.DrawnBankCode
            oImpRequest.InsuranceRef = oFindCashListReceiptsRequest.InsuranceRef
            oImpRequest.MaxRowsToFetch = oFindCashListReceiptsRequest.MaxRowsToFetch
            oImpRequest.MaxRowsToFetchSpecified = oFindCashListReceiptsRequest.MaxRowsToFetchSpecified
            oImpRequest.MediaReference = oFindCashListReceiptsRequest.MediaReference
            oImpRequest.MediaTypeStatusCode = oFindCashListReceiptsRequest.MediaTypeStatusCode
            oImpRequest.PartyKey = oFindCashListReceiptsRequest.PartyKey
            oImpRequest.PartyKeySpecified = oFindCashListReceiptsRequest.PartyKeySpecified
            oImpRequest.WCFSecurityToken = If(oFindCashListReceiptsRequest.WCFSecurityToken.Length > 0, oFindCashListReceiptsRequest.WCFSecurityToken, "WCFSecurityToken")
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.FindCashListReceipts(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.CashListItems = SAMFunc.GetDeserializedValues(Of List(Of BaseFindCashListReceiptsResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataSet, sFromTypeName:="BaseFindCashListReceiptsResponseTypeCashListItems", sConvertToTypeName:="BaseFindCashListReceiptsResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.CashListItems = DataTabletoList_FindCashListReceipts(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oFindCashListReceiptsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oFindCashListReceiptsRequest))
            Return Nothing
        End Try
    End Function

    Public Function GetAccountBalance(ByVal GetAccountBalanceRequest As GetAccountBalanceRequestType) As GetAccountBalanceResponseType Implements IPureAccountService.GetAccountBalance

        Try

            Dim sUserName As String = GetAccountBalanceRequest.LoginUserName
            Dim iAgentKey As Integer = 0
            Dim iUserId As Integer = 0

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGClmLFR", iUserId)
            CommonFunctions.CheckSecurityToken(GetAccountBalanceRequest.WCFSecurityToken)
            Dim oResponse As New GetAccountBalanceResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetAccountBalanceRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetAccountBalanceRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetAccountBalanceResponseType = Nothing



            oImpRequest.BranchCode = GetAccountBalanceRequest.BranchCode
            oImpRequest.PartyKey = iAgentKey
            oImpRequest.WCFSecurityToken = If(GetAccountBalanceRequest.WCFSecurityToken.Length > 0, GetAccountBalanceRequest.WCFSecurityToken, "WCFSecurityToken")
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetAccountBalance(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.AccountBalance = SAMFunc.GetDeserializedValues(Of List(Of BaseGetAccountBalanceResponseTypeRow))(elmResultDataSet:=oImpResponse.AccountBalance, sFromTypeName:="BaseGetAccountBalanceResponseTypeAccountBalance", sConvertToTypeName:="BaseGetAccountBalanceResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.AccountBalance = DataTabletoList_GetAccountBalance(oImpResponse.ResultData.Tables(0))
                End If


            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetAccountBalanceRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetAccountBalanceRequest))
            Return Nothing
        End Try

    End Function


    '''<summary>
    ''' This method is used to get Account Details
    '''</summary>
    '''<param name="GetAccountDetailsRequest" type="GetAccountDetailsRequestType"></param>   
    '''<returns>GetAccountDetailsResponseType</returns>
    '''<remarks></remarks> 
    Public Function GetAccountDetails(ByVal GetAccountDetailsRequest As GetAccountDetailsRequestType) As GetAccountDetailsResponseType Implements IPureAccountService.GetAccountDetails, IPurePartyService.GetAccountDetails, IPurePolicyService.GetAccountDetails

        Try

            Dim sUserName As String = GetAccountDetailsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMACCTDET", iUserId)
            CommonFunctions.CheckSecurityToken(GetAccountDetailsRequest.WCFSecurityToken)
            Dim oResponse As New GetAccountDetailsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetAccountDetailsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetAccountDetailsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetAccountDetailsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetAccountDetailsRequest.BranchCode

            oImpRequest.PartyCntSpecified = GetAccountDetailsRequest.PartyCntSpecified
            oImpRequest.PartyCnt = GetAccountDetailsRequest.PartyCnt
            oImpRequest.AccountKeySpecified = GetAccountDetailsRequest.AccountKeySpecified
            oImpRequest.AccountKey = GetAccountDetailsRequest.AccountKey

            oImpRequest.DocumentRef = GetAccountDetailsRequest.DocumentRef
            oImpRequest.CurrencyCode = GetAccountDetailsRequest.CurrencyCode

            oImpRequest.CurrencyAmount = GetAccountDetailsRequest.CurrencyAmount
            oImpRequest.CurrencyAmountSpecified = GetAccountDetailsRequest.CurrencyAmountSpecified
            oImpRequest.Tolerance = GetAccountDetailsRequest.Tolerance
            oImpRequest.ToleranceSpecified = GetAccountDetailsRequest.ToleranceSpecified
            oImpRequest.DocTypeGroupCode = GetAccountDetailsRequest.DocTypeGroupCode

            oImpRequest.DocumentTypeCode = GetAccountDetailsRequest.DocumentTypeCode

            oImpRequest.PeriodKey = GetAccountDetailsRequest.PeriodKey
            oImpRequest.PeriodKeySpecified = GetAccountDetailsRequest.PeriodKeySpecified
            oImpRequest.DateFrom = GetAccountDetailsRequest.DateFrom
            oImpRequest.DateFromSpecified = GetAccountDetailsRequest.DateFromSpecified
            oImpRequest.DateTo = GetAccountDetailsRequest.DateTo
            oImpRequest.DateToSpecified = GetAccountDetailsRequest.DateToSpecified
            oImpRequest.InsuranceRef = GetAccountDetailsRequest.InsuranceRef
            oImpRequest.Username = GetAccountDetailsRequest.Username
            oImpRequest.PurchaseInvoiceNo = GetAccountDetailsRequest.PurchaseInvoiceNo
            oImpRequest.PurchaseOrderNo = GetAccountDetailsRequest.PurchaseOrderNo
            oImpRequest.Department = GetAccountDetailsRequest.Department
            oImpRequest.Spare = GetAccountDetailsRequest.Spare
            oImpRequest.OutstandingOnly = GetAccountDetailsRequest.OutstandingOnly
            oImpRequest.OutstandingOnlySpecified = GetAccountDetailsRequest.OutstandingOnlySpecified
            oImpRequest.IsNewPF = GetAccountDetailsRequest.IsNewPF
            oImpRequest.IsNewPFSpecified = GetAccountDetailsRequest.IsNewPFSpecified
            oImpRequest.InsuredAccountKey = GetAccountDetailsRequest.InsuredAccountKey
            oImpRequest.InsuredAccountKeySpecified = GetAccountDetailsRequest.InsuredAccountKeySpecified
            oImpRequest.Rollup = GetAccountDetailsRequest.Rollup
            oImpRequest.RollupSpecified = GetAccountDetailsRequest.RollupSpecified
            oImpRequest.CashListKey = GetAccountDetailsRequest.CashListKey
            oImpRequest.CashListKeySpecified = GetAccountDetailsRequest.CashListKeySpecified
            oImpRequest.OrderBySpare = GetAccountDetailsRequest.OrderBySpare
            oImpRequest.OrderBySpareSpecified = GetAccountDetailsRequest.OrderBySpareSpecified
            oImpRequest.DocumentKey = GetAccountDetailsRequest.DocumentKey
            oImpRequest.DocumentKeySpecified = GetAccountDetailsRequest.DocumentKeySpecified
            oImpRequest.FinancePlanKey = GetAccountDetailsRequest.FinancePlanKey
            oImpRequest.FinancePlanKeySpecified = GetAccountDetailsRequest.FinancePlanKeySpecified
            oImpRequest.UnderwritingYearKey = GetAccountDetailsRequest.UnderwritingYearKey
            oImpRequest.UnderwritingYearKeySpecified = GetAccountDetailsRequest.UnderwritingYearKeySpecified
            oImpRequest.SourceArray = GetAccountDetailsRequest.SourceArray
            oImpRequest.TransDetailKeys = GetAccountDetailsRequest.TransDetailKeys
            oImpRequest.Display500 = GetAccountDetailsRequest.Display500
            oImpRequest.Display500Specified = GetAccountDetailsRequest.Display500Specified
            oImpRequest.AltReference = GetAccountDetailsRequest.AltReference
            oImpRequest.IncludeReversedTran = GetAccountDetailsRequest.IncludeReversedTran
            oImpRequest.IncludeReversedTranSpecified = GetAccountDetailsRequest.IncludeReversedTranSpecified
            oImpRequest.BGRef = GetAccountDetailsRequest.BGRef
            oImpRequest.CashListKey = GetAccountDetailsRequest.CashListKey
            oImpRequest.IsSplitReceipt = GetAccountDetailsRequest.IsSplitReceipt
            oImpRequest.IsLead = GetAccountDetailsRequest.IsLeadAgent
            oImpRequest.AgentCnt = GetAccountDetailsRequest.nAgentCnt
            oImpRequest.Insurance_file_cnt = GetAccountDetailsRequest.Insurance_file_cnt
            oImpRequest.Insurance_folder_cnt = GetAccountDetailsRequest.Insurance_folder_cnt

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetAccountDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.AccountName = oImpResponse.AccountName
                oResponse.ContactName = oImpResponse.ContactName
                oResponse.PhoneAreaCode = oImpResponse.PhoneAreaCode
                oResponse.PhoneExtension = oImpResponse.PhoneExtension
                oResponse.PhoneNumber = oImpResponse.PhoneNumber
                oResponse.AccountStatus = oImpResponse.AccountStatus
                oResponse.AccountBalance = oImpResponse.AccountBalance

                If (oImpResponse.Transactions IsNot Nothing) Then
                    If (oImpResponse.Transactions.Row IsNot Nothing) AndAlso oImpResponse.Transactions.Row.Count > 0 Then
                        oResponse.Transactions = oImpResponse.Transactions.Row.ToList().ConvertAll(
                            New Converter(Of BaseImplementationTypes.BaseGetAccountDetailsResponseTypeTransactionsRow, BaseGetAccountDetailsResponseTypeRow)(AddressOf CommonFunctions.ToServiceGetAccountDetailsTransactionsList))
                    End If
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetAccountDetailsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetAccountDetailsRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetAccountingPeriod(ByVal GetAccountingPeriodRequest As GetAccountingPeriodRequestType) As GetAccountingPeriodResponseType Implements IPureAccountService.GetAccountingPeriod
        Try
            Dim sUserName As String = GetAccountingPeriodRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMACCTDET", iUserId)
            CommonFunctions.CheckSecurityToken(GetAccountingPeriodRequest.WCFSecurityToken)
            Dim oResponse As New GetAccountingPeriodResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetAccountingPeriodRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetAccountingPeriodRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetAccountingPeriodResponseType = Nothing

            oImpRequest.BranchCode = GetAccountingPeriodRequest.BranchCode
            oImpRequest.DateInPeriod = GetAccountingPeriodRequest.DateInPeriod
            If GetAccountingPeriodRequest.SubBranchCodeSpecified1 Then
                oImpRequest.SubBranchCode = GetAccountingPeriodRequest.SubBranchCode
            End If
            oImpRequest.WCFSecurityToken = If(GetAccountingPeriodRequest.WCFSecurityToken.Length > 0, GetAccountingPeriodRequest.WCFSecurityToken, "WCFSecurityToken")
            Try
                oImpResponse = oBusiness.GetAccountingPeriod(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.Period = SAMFunc.GetDeserializedValues(Of List(Of BaseGetAccountingPeriodResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataSet, sFromTypeName:="BaseGetAccountingPeriodResponseTypePeriod", sConvertToTypeName:="BaseGetAccountingPeriodResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Period = DataTabletoList_GetAccountingPeriod(oImpResponse.ResultData.Tables(0))
                End If


            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetAccountingPeriodRequest))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetAccountingPeriodRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetAgentCommissionTax(ByVal GetAgentCommissionTaxRequest As GetAgentCommissionTaxRequestType) As GetAgentCommissionTaxResponseType Implements IPurePolicyService.GetAgentCommissionTax

        Try

            Dim sUserName As String = GetAgentCommissionTaxRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGAGCOTX", iUserId)
            CommonFunctions.CheckSecurityToken(GetAgentCommissionTaxRequest.WCFSecurityToken)
            Dim oResponse As New GetAgentCommissionTaxResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetAgentCommissionTaxRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetAgentCommissionTaxRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetAgentCommissionTaxResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.InsuranceFileKey = GetAgentCommissionTaxRequest.InsuranceFileKey
            oImpRequest.TaxGroupCode = GetAgentCommissionTaxRequest.TaxGroupCode
            oImpRequest.BranchCode = GetAgentCommissionTaxRequest.BranchCode
            oImpRequest.CurrencyCode = GetAgentCommissionTaxRequest.CurrencyCode
            oImpRequest.AgentCommissionAmount = GetAgentCommissionTaxRequest.AgentCommissionAmount

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetAgentCommissionTax(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.TaxCurrencyAmount = oImpResponse.TaxCurrencyAmount
                oResponse.TaxBaseAmount = oImpResponse.TaxBaseAmount
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetAgentCommissionTaxRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetAgentCommissionTaxRequest))
            Return Nothing
        End Try

    End Function
    Public Function GetRenewalAmountToFinance(ByVal GetRenewalAmountToFinanceRequest As GetRenewalAmountToFinanceRequestType) As GetRenewalAmountToFinanceResponseType Implements IPurePolicyService.GetRenewalAmountToFinance

        Try

            Dim sUserName As String = GetRenewalAmountToFinanceRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGAGCOTX", iUserId)
            CommonFunctions.CheckSecurityToken(GetRenewalAmountToFinanceRequest.WCFSecurityToken)
            Dim oResponse As New GetRenewalAmountToFinanceResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetRenewalAmountToFinanceRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetRenewalAmountToFinanceRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetRenewalAmountToFinanceResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.InsuranceFileKey = GetRenewalAmountToFinanceRequest.InsuranceFileKey
            oImpRequest.BranchCode = GetRenewalAmountToFinanceRequest.BranchCode


            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetRenewalAmountToFinance(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.RenewalAmount = oImpResponse.RenewalAmount

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetRenewalAmountToFinanceRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetRenewalAmountToFinanceRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    '''This function will get the  TransactionDetailKey as an input and it will be passed to 
    ''' SAMForInsuranceV2Business file
    '''</summary>
    '''<param name="GetHeaderAndPolicyFeesByKeyRequestType" type="GetHeaderAndPolicyFeesByKeyResponseType"></param>   
    '''<remarks></remarks>
    ''' </summary>
    Public Function GetAllocationDetails(ByVal oGetAllocationDetailsRequest As GetAllocationDetailsRequestType) As GetAllocationDetailsResponseType Implements IPureAccountService.GetAllocationDetails

        Try

            Dim sUserName As String = oGetAllocationDetailsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMACCTDET", iUserId)
            CommonFunctions.CheckSecurityToken(oGetAllocationDetailsRequest.WCFSecurityToken)
            Dim oResponse As New GetAllocationDetailsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetAllocationDetailsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetAllocationDetailsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetAllocationDetailsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetAllocationDetailsRequest.BranchCode
            oImpRequest.TransDetailKey = oGetAllocationDetailsRequest.TransDetailKey
            oImpRequest.WCFSecurityToken = If(oGetAllocationDetailsRequest.WCFSecurityToken.Length > 0, oGetAllocationDetailsRequest.WCFSecurityToken, "WCFSecurityToken")
            oImpRequest.IncludeExtended = oGetAllocationDetailsRequest.IncludeExtended

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetAllocationDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.Row = SAMFunc.GetDeserializedValues(Of List(Of BaseGetAllocationDetailsResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetAllocationDetailsResponseType", sConvertToTypeName:="BaseGetAllocationDetailsResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Row = DataTabletoList_GetAllocationDetails(oImpResponse.ResultData.Tables(0))
                End If


            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetAllocationDetailsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetAllocationDetailsRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' This is webservice method for GetBalancesAndUnallocatedCredits
    '''<param name="GetBalancesAndUnallocatedCreditsRequest" type="GetBalancesAndUnallocatedCreditsRequestType"></param>   
    '''</summary>
    '''<remarks></remarks>  

    Public Function GetBalancesAndUnallocatedCredits(ByVal GetBalancesAndUnallocatedCreditsRequest As GetBalancesAndUnallocatedCreditsRequestType) As GetBalancesAndUnallocatedCreditsResponseType Implements IPureAccountService.GetBalancesAndUnallocatedCredits

        Try

            Dim sUserName As String = GetBalancesAndUnallocatedCreditsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMBQuote", iUserId)
            CommonFunctions.CheckSecurityToken(GetBalancesAndUnallocatedCreditsRequest.WCFSecurityToken)
            Dim oResponse As New GetBalancesAndUnallocatedCreditsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetBalancesAndUnallocatedCreditsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetBalancesAndUnallocatedCreditsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetBalancesAndUnallocatedCreditsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetBalancesAndUnallocatedCreditsRequest.BranchCode
            oImpRequest.InsuranceFileKey = GetBalancesAndUnallocatedCreditsRequest.InsuranceFileKey
            oImpRequest.WCFSecurityToken = If(GetBalancesAndUnallocatedCreditsRequest.WCFSecurityToken.Length > 0, GetBalancesAndUnallocatedCreditsRequest.WCFSecurityToken, "WCFSecurityToken")

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetBalancesAndUnallocatedCredits(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.InsuranceRef = oImpResponse.InsuranceRef
                oResponse.ClientKey = oImpResponse.ClientKey
                oResponse.AgentKey = oImpResponse.AgentKey
                oResponse.AgentType = oImpResponse.AgentType
                oResponse.IsFloatBalanceAccount = oImpResponse.IsFloatBalanceAccount
                oResponse.IsOverDraftAccount = oImpResponse.IsOverDraftAccount
                oResponse.FloatBalanceLimit = oImpResponse.FloatBalanceLimit
                oResponse.OverDraftLimit = oImpResponse.OverDraftLimit
                oResponse.OverDraftExpiry = oImpResponse.OverDraftExpiry
                oResponse.AccountBalance = oImpResponse.AccountBalance

                'oResponse.UnallocatedCreditsForAgents = SAMFunc.GetDeserializedValues(Of List(Of BaseGetBalancesAndUnallocatedCreditsResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDatasetForAgents, sFromTypeName:="BaseGetBalancesAndUnallocatedCreditsResponseTypeUnallocatedCreditsForAgents", sConvertToTypeName:="BaseGetBalancesAndUnallocatedCreditsResponseTypeRow")

                If oImpResponse.ResultDataForAgent IsNot Nothing AndAlso oImpResponse.ResultDataForAgent.Tables(0) IsNot Nothing Then
                    oResponse.UnallocatedCreditsForAgents = DataTabletoList_GetBalancesAndUnallocatedCredits(oImpResponse.ResultDataForAgent.Tables(0))
                End If


                'oResponse.UnallocatedCreditsForClients = SAMFunc.GetDeserializedValues(Of List(Of BaseGetBalancesAndUnallocatedCreditsResponseTypeRow1))(elmResultDataSet:=oImpResponse.ResultDatasetForClients, sFromTypeName:="BaseGetBalancesAndUnallocatedCreditsResponseTypeUnallocatedCreditsForClients", sConvertToTypeName:="BaseGetBalancesAndUnallocatedCreditsResponseTypeRow1")
                If oImpResponse.ResultDataForClient IsNot Nothing AndAlso oImpResponse.ResultDataForClient.Tables(0) IsNot Nothing Then
                    oResponse.UnallocatedCreditsForClients = DataTabletoList_GetBalancesAndUnallocatedCredits1(oImpResponse.ResultDataForClient.Tables(0))
                End If


            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetBalancesAndUnallocatedCreditsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetBalancesAndUnallocatedCreditsRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetPaymentCashListDetails(ByVal oGetPaymentCashListDetailsRequest As GetPaymentCashListDetailsRequestType) As GetPaymentCashListDetailsResponseType Implements IPureAccountService.GetPaymentCashListDetails

        Try

            Dim sUserName As String = oGetPaymentCashListDetailsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMCASHPAY", iUserId)
            CommonFunctions.CheckSecurityToken(oGetPaymentCashListDetailsRequest.WCFSecurityToken)
            Dim oResponse As New GetPaymentCashListDetailsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetPaymentCashListDetailsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetPaymentCashListDetailsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetPaymentCashListDetailsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetPaymentCashListDetailsRequest.BranchCode
            oImpRequest.CashListKey = oGetPaymentCashListDetailsRequest.CashListKey

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetPaymentCashListDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                'India : Set the response values here

                oResponse.PaymentCashList = New BasePaymentCashListType
                oResponse.PaymentCashList.TypeCode = oImpResponse.PaymentCashList.TypeCode
                oResponse.PaymentCashList.ListDate = oImpResponse.PaymentCashList.ListDate
                oResponse.PaymentCashList.BankAccountCode = oImpResponse.PaymentCashList.BankAccountCode
                oResponse.PaymentCashList.CurrencyCode = oImpResponse.PaymentCashList.CurrencyCode
                oResponse.PaymentCashList.Reference = oImpResponse.PaymentCashList.Reference
                oResponse.PaymentCashList.StatusCode = oImpResponse.PaymentCashList.StatusCode

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetPaymentCashListDetailsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetPaymentCashListDetailsRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetPaymentCashListItemDetails(ByVal oGetPaymentCashListItemDetailsRequest As GetPaymentCashListItemDetailsRequestType) As GetPaymentCashListItemDetailsResponseType Implements IPureAccountService.GetPaymentCashListItemDetails

        Try

            Dim sUserName As String = oGetPaymentCashListItemDetailsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMCASHPAY", iUserId)
            CommonFunctions.CheckSecurityToken(oGetPaymentCashListItemDetailsRequest.WCFSecurityToken)
            Dim oResponse As New GetPaymentCashListItemDetailsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetPaymentCashListItemDetailsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetPaymentCashListItemDetailsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetPaymentCashListItemDetailsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetPaymentCashListItemDetailsRequest.BranchCode
            oImpRequest.CashListItemKey = oGetPaymentCashListItemDetailsRequest.CashListItemKey

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetPaymentCashListItemDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'India : Set the response values here
                If oImpResponse.CashListPayment IsNot Nothing Then
                    oResponse.CashListPayment = New BasePaymentCashListItemType

                    oResponse.CashListPayment.IsProduceDocument = oImpResponse.CashListPayment.IsProduceDocument

                    oResponse.CashListPayment.MediaTypeCode = oImpResponse.CashListPayment.MediaTypeCode
                    oResponse.CashListPayment.TransactionDate = oImpResponse.CashListPayment.TransactionDate
                    oResponse.CashListPayment.AccountShortCode = oImpResponse.CashListPayment.AccountShortCode
                    oResponse.CashListPayment.Amount = oImpResponse.CashListPayment.Amount
                    oResponse.CashListPayment.AllocationStatusCode = oImpResponse.CashListPayment.AllocationStatusCode
                    oResponse.CashListPayment.MediaReference = oImpResponse.CashListPayment.MediaReference
                    oResponse.CashListPayment.OurReference = oImpResponse.CashListPayment.OurReference
                    oResponse.CashListPayment.TheirReference = oImpResponse.CashListPayment.TheirReference
                    oResponse.CashListPayment.ContactName = oImpResponse.CashListPayment.ContactName
                    oResponse.CashListPayment.FurtherDetails = oImpResponse.CashListPayment.FurtherDetails
                    oResponse.CashListPayment.TypeCode = oImpResponse.CashListPayment.TypeCode
                    oResponse.CashListPayment.StatusCode = oImpResponse.CashListPayment.StatusCode
                    oResponse.CashListPayment.UserId = oImpResponse.CashListPayment.UserId
                    oResponse.CashListPayment.UserName = oImpResponse.CashListPayment.UserName

                    oResponse.CashListPayment.Bank = New BaseBankPaymentType
                    oResponse.CashListPayment.Bank.AccountCode = oImpResponse.CashListPayment.Bank.AccountCode
                    oResponse.CashListPayment.Bank.BranchCode = oImpResponse.CashListPayment.Bank.BranchCode
                    oResponse.CashListPayment.Bank.ExpiryDate = oImpResponse.CashListPayment.Bank.ExpiryDate
                    oResponse.CashListPayment.Bank.PayeeName = oImpResponse.CashListPayment.Bank.PayeeName
                    oResponse.CashListPayment.Bank.Reference1 = oImpResponse.CashListPayment.Bank.Reference1
                    oResponse.CashListPayment.Bank.Reference2 = oImpResponse.CashListPayment.Bank.Reference2
                    oResponse.CashListPayment.Bank.BIC = oImpResponse.CashListPayment.Bank.BIC
                    oResponse.CashListPayment.Bank.IBAN = oImpResponse.CashListPayment.Bank.IBAN

                    oResponse.CashListPayment.ContactAddress = New BaseSimpleAddressType
                    oResponse.CashListPayment.ContactAddress.AddressLine1 = oImpResponse.CashListPayment.ContactAddress.AddressLine1
                    oResponse.CashListPayment.ContactAddress.AddressLine2 = oImpResponse.CashListPayment.ContactAddress.AddressLine2
                    oResponse.CashListPayment.ContactAddress.AddressLine3 = oImpResponse.CashListPayment.ContactAddress.AddressLine3
                    oResponse.CashListPayment.ContactAddress.AddressLine4 = oImpResponse.CashListPayment.ContactAddress.AddressLine4
                    oResponse.CashListPayment.ContactAddress.PostCode = oImpResponse.CashListPayment.ContactAddress.PostCode
                    oResponse.CashListPayment.ContactAddress.CountryCode = oImpResponse.CashListPayment.ContactAddress.CountryCode

                    oResponse.CashListPayment.CreditCard = New BaseCreditCardType
                    oResponse.CashListPayment.CreditCard.Number = oImpResponse.CashListPayment.CreditCard.Number
                    oResponse.CashListPayment.CreditCard.ExpiryDate = oImpResponse.CashListPayment.CreditCard.ExpiryDate
                    oResponse.CashListPayment.CreditCard.StartDate = oImpResponse.CashListPayment.CreditCard.StartDate
                    oResponse.CashListPayment.CreditCard.AuthCode = oImpResponse.CashListPayment.CreditCard.AuthCode
                    oResponse.CashListPayment.CreditCard.ManualAuthCode = oImpResponse.CashListPayment.CreditCard.ManualAuthCode
                    oResponse.CashListPayment.CreditCard.NameOnCreditCard = oImpResponse.CashListPayment.CreditCard.NameOnCreditCard
                    oResponse.CashListPayment.CreditCard.Issue = oImpResponse.CashListPayment.CreditCard.Issue
                    oResponse.CashListPayment.CreditCard.Pin = oImpResponse.CashListPayment.CreditCard.Pin
                    oResponse.CashListPayment.CreditCard.TransactionCode = oImpResponse.CashListPayment.CreditCard.TransactionCode
                    oResponse.CashListPayment.CreditCard.CustomerPresent = oImpResponse.CashListPayment.CreditCard.CustomerPresent
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetPaymentCashListItemDetailsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetPaymentCashListItemDetailsRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetPaymentCashListItems(ByVal oGetPaymentCashListItemsRequest As GetPaymentCashListItemsRequestType) As GetPaymentCashListItemsResponseType Implements IPureAccountService.GetPaymentCashListItems

        Try

            Dim sUserName As String = oGetPaymentCashListItemsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMCASHPAY", iUserId)
            CommonFunctions.CheckSecurityToken(oGetPaymentCashListItemsRequest.WCFSecurityToken)
            Dim oResponse As New GetPaymentCashListItemsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetPaymentCashListItemsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetPaymentCashListItemsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetPaymentCashListItemsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetPaymentCashListItemsRequest.BranchCode
            oImpRequest.CashListKey = oGetPaymentCashListItemsRequest.CashListKey
            oImpRequest.WCFSecurityToken = If(oGetPaymentCashListItemsRequest.WCFSecurityToken.Length > 0, oGetPaymentCashListItemsRequest.WCFSecurityToken, "WCFSecurityToken")
            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetPaymentCashListItems(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.PaymentCashListItems = SAMFunc.GetDeserializedValues(Of List(Of BaseGetPaymentCashListItemsResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetPaymentCashListItemsResponseTypePaymentCashListItems", sConvertToTypeName:="BaseGetPaymentCashListItemsResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.PaymentCashListItems = DataTabletoList_GetPaymentCashListItems(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetPaymentCashListItemsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetPaymentCashListItemsRequest))
            Return Nothing
        End Try
    End Function

    Public Function GetReceiptCashListDetails(ByVal oGetReceiptCashListDetailsRequest As GetReceiptCashListDetailsRequestType) As GetReceiptCashListDetailsResponseType Implements IPureAccountService.GetReceiptCashListDetails

        Try

            Dim sUserName As String = oGetReceiptCashListDetailsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMCASHREC", iUserId)
            CommonFunctions.CheckSecurityToken(oGetReceiptCashListDetailsRequest.WCFSecurityToken)
            Dim oResponse As New GetReceiptCashListDetailsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetReceiptCashListDetailsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetReceiptCashListDetailsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetReceiptCashListDetailsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetReceiptCashListDetailsRequest.BranchCode
            oImpRequest.CashListKey = oGetReceiptCashListDetailsRequest.CashListKey

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetReceiptCashListDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                If oImpResponse.ReceiptCashList IsNot Nothing Then
                    oResponse.ReceiptCashList = New BaseReceiptCashListType
                    oResponse.ReceiptCashList.TypeCode = oImpResponse.ReceiptCashList.TypeCode
                    oResponse.ReceiptCashList.ListDate = oImpResponse.ReceiptCashList.ListDate
                    oResponse.ReceiptCashList.BankAccountCode = oImpResponse.ReceiptCashList.BankAccountCode
                    oResponse.ReceiptCashList.CurrencyCode = oImpResponse.ReceiptCashList.CurrencyCode
                    oResponse.ReceiptCashList.Reference = oImpResponse.ReceiptCashList.Reference
                    oResponse.ReceiptCashList.StatusCode = oImpResponse.ReceiptCashList.StatusCode

                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetReceiptCashListDetailsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetReceiptCashListDetailsRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetReceiptCashListItemDetails(ByVal oGetReceiptCashListItemDetailsRequest As GetReceiptCashListItemDetailsRequestType) As GetReceiptCashListItemDetailsResponseType Implements IPureAccountService.GetReceiptCashListItemDetails

        Try

            Dim sUserName As String = oGetReceiptCashListItemDetailsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGRefP", iUserId)
            CommonFunctions.CheckSecurityToken(oGetReceiptCashListItemDetailsRequest.WCFSecurityToken)
            Dim oResponse As New GetReceiptCashListItemDetailsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetReceiptCashListItemDetailsRequest.BranchCode)

            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetReceiptCashListItemDetailsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetReceiptCashListItemDetailsResponseType = Nothing

            oImpRequest.BranchCode = oGetReceiptCashListItemDetailsRequest.BranchCode
            oImpRequest.CashListItemKey = oGetReceiptCashListItemDetailsRequest.CashListItemKey

            Try

                oImpResponse = oBusiness.GetReceiptCashListItemDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                oResponse.CashListReceipt = New BaseReceiptCashListItemType
                If oImpResponse.CashListReceipt IsNot Nothing Then
                    With oResponse.CashListReceipt
                        .AccountShortCode = oImpResponse.CashListReceipt.AccountShortCode
                        .AllocationStatusCode = oImpResponse.CashListReceipt.AllocationStatusCode
                        .Amount = oImpResponse.CashListReceipt.Amount
                        .Bank = New BaseBankReceiptType
                        .Bank.BankCode = oImpResponse.CashListReceipt.Bank.BankCode
                        .Bank.ChequeDate = oImpResponse.CashListReceipt.Bank.ChequeDate
                        .Bank.PayerName = oImpResponse.CashListReceipt.Bank.PayerName

                        .ContactAddress = New BaseSimpleAddressType
                        .ContactAddress.AddressLine1 = oImpResponse.CashListReceipt.ContactAddress.AddressLine1
                        .ContactAddress.AddressLine2 = oImpResponse.CashListReceipt.ContactAddress.AddressLine2
                        .ContactAddress.AddressLine3 = oImpResponse.CashListReceipt.ContactAddress.AddressLine3
                        .ContactAddress.AddressLine4 = oImpResponse.CashListReceipt.ContactAddress.AddressLine4
                        .ContactAddress.CountryCode = oImpResponse.CashListReceipt.ContactAddress.CountryCode
                        .ContactAddress.PostCode = oImpResponse.CashListReceipt.ContactAddress.PostCode
                        .ContactName = oImpResponse.CashListReceipt.ContactName

                        .CreditCard = New BaseCreditCardType
                        .CreditCard.AuthCode = oImpResponse.CashListReceipt.CreditCard.AuthCode
                        .CreditCard.CustomerPresent = oImpResponse.CashListReceipt.CreditCard.CustomerPresent
                        .CreditCard.ExpiryDate = oImpResponse.CashListReceipt.CreditCard.ExpiryDate
                        .CreditCard.Issue = oImpResponse.CashListReceipt.CreditCard.Issue
                        .CreditCard.ManualAuthCode = oImpResponse.CashListReceipt.CreditCard.ManualAuthCode
                        .CreditCard.NameOnCreditCard = oImpResponse.CashListReceipt.CreditCard.NameOnCreditCard
                        .CreditCard.Number = oImpResponse.CashListReceipt.CreditCard.Number
                        .CreditCard.Pin = oImpResponse.CashListReceipt.CreditCard.Pin
                        .CreditCard.StartDate = oImpResponse.CashListReceipt.CreditCard.StartDate
                        .CreditCard.TransactionCode = oImpResponse.CashListReceipt.CreditCard.TransactionCode
                        .CreditCard.TypeCode = oImpResponse.CashListReceipt.CreditCard.TypeCode

                        .FurtherDetails = oImpResponse.CashListReceipt.FurtherDetails
                        .MediaReference = oImpResponse.CashListReceipt.MediaReference
                        .MediaTypeCode = oImpResponse.CashListReceipt.MediaTypeCode
                        .OurReference = oImpResponse.CashListReceipt.OurReference
                        .StatusCode = oImpResponse.CashListReceipt.StatusCode
                        .TheirReference = oImpResponse.CashListReceipt.TheirReference
                        .TransactionDate = oImpResponse.CashListReceipt.TransactionDate
                        .TypeCode = oImpResponse.CashListReceipt.TypeCode

                        .IsProduceDocument = oImpResponse.CashListReceipt.IsProduceDocument

                    End With
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetReceiptCashListItemDetailsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetReceiptCashListItemDetailsRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetReceiptCashListItems(ByVal GetReceiptCashListItemsRequest As GetReceiptCashListItemsRequestType) As GetReceiptCashListItemsResponseType Implements IPureAccountService.GetReceiptCashListItems

        Try

            Dim sUserName As String = GetReceiptCashListItemsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMCASHREC", iUserId)
            CommonFunctions.CheckSecurityToken(GetReceiptCashListItemsRequest.WCFSecurityToken)
            Dim oResponse As New GetReceiptCashListItemsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetReceiptCashListItemsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetReceiptCashListItemsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetReceiptCashListItemsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetReceiptCashListItemsRequest.BranchCode
            oImpRequest.CashListKey = GetReceiptCashListItemsRequest.CashListKey
            oImpRequest.WCFSecurityToken = If(GetReceiptCashListItemsRequest.WCFSecurityToken.Length > 0, GetReceiptCashListItemsRequest.WCFSecurityToken, "WCFSecurityToken")
            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetReceiptCashListItems(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.ReceiptCashListItems = SAMFunc.GetDeserializedValues(Of List(Of BaseGetReceiptCashListItemsResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetReceiptCashListItemsResponseTypeReceiptCashListItems", sConvertToTypeName:="BaseGetReceiptCashListItemsResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.ReceiptCashListItems = DataTabletoList_GetReceiptCashListItems(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetReceiptCashListItemsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetReceiptCashListItemsRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>  
    ''' This web services method is used to Get Transaction Details
    ''' </summary>  
    ''' <param name="oGetTransactionDetailsRequest">An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetTransactionDetailsRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetTransactionDetailsResponseType</returns>  
    Public Function GetTransactionDetails(ByVal oGetTransactionDetailsRequest As GetTransactionDetailsRequestType) As GetTransactionDetailsResponseType Implements IPureAccountService.GetTransactionDetails

        Try

            Dim sUserName As String = oGetTransactionDetailsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGTRAKey", iUserId)
            CommonFunctions.CheckSecurityToken(oGetTransactionDetailsRequest.WCFSecurityToken)
            Dim oResponse As New GetTransactionDetailsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetTransactionDetailsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetTransactionDetailsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetTransactionDetailsResponseType = Nothing

            ' Pass the values to the implementation request structure and do remember to map the specified fields also
            With oGetTransactionDetailsRequest
                oImpRequest.BranchCode = .BranchCode
                oImpRequest.AccountKey = .AccountKey
                oImpRequest.AccountKeySpecified = .AccountKeySpecified
                oImpRequest.InsuranceRef = .InsuranceRef
                oImpRequest.AccountingDate = .AccountingDate
                oImpRequest.IsNewPF = .IsNewPF
                oImpRequest.IsOutstandingOnly = .IsOutstandingOnly
                oImpRequest.ShortCode = .ShortCode
                oImpRequest.IncludeReversedTran = .IncludeReversedTran

                If oGetTransactionDetailsRequest.Allocation IsNot Nothing AndAlso oGetTransactionDetailsRequest.Allocation.Count > 0 Then
                    oImpRequest.Allocation = New BaseImplementationTypes.BaseGetTransactionDetailsRequestTypeAllocation
                    For iCount As Integer = 0 To oGetTransactionDetailsRequest.Allocation.Count - 1
                        ReDim Preserve oImpRequest.Allocation.Row(iCount)
                        oImpRequest.Allocation.Row(iCount) = New BaseImplementationTypes.BaseGetTransactionDetailsRequestTypeAllocationRow
                        oImpRequest.Allocation.Row(iCount).AllocationTransDetailKey = .Allocation.Item(iCount).AllocationTransDetailKey
                    Next
                End If
                oImpRequest.WCFSecurityToken = If(.WCFSecurityToken.Length > 0, .WCFSecurityToken, "WCFSecurityToken")

            End With

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetTransactionDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.Transactions = SAMFunc.GetDeserializedValues(Of List(Of BaseGetTransactionDetailsResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetTransactionDetailsResponseTypeTransactions", sConvertToTypeName:="BaseGetTransactionDetailsResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Transactions = DataTabletoList_GetTransactionDetails(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetTransactionDetailsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetTransactionDetailsRequest))
            Return Nothing
        End Try

    End Function

    Public Function MarkUnmarkTransaction(ByVal oMarkUnmarkTransactionRequest As MarkUnmarkTransactionRequestType) As MarkUnmarkTransactionResponseType Implements IPureAccountService.MarkUnmarkTransaction
        Try
            Dim sUserName As String = oMarkUnmarkTransactionRequest.LoginUserName
            Dim iAgentKey As Integer = 0
            Dim iUserId As Integer = 0

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New MarkUnmarkTransactionResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oMarkUnmarkTransactionRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.MarkUnmarkTransactionRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.MarkUnmarkTransactionResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oMarkUnmarkTransactionRequest.BranchCode
            oImpRequest.CurrencyCode = oMarkUnmarkTransactionRequest.CurrencyCode
            oImpRequest.MarkStatus = CType([Enum].ToObject(GetType(MarkStatusType), oMarkUnmarkTransactionRequest.MarkStatus), BaseImplementationTypes.MarkStatusType)
            oImpRequest.PaymentAmount = oMarkUnmarkTransactionRequest.PaymentAmount
            oImpRequest.TransactionKey = oMarkUnmarkTransactionRequest.TransactionKey
            Try
                oImpResponse = oBusiness.MarkUnmarkTransaction(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oMarkUnmarkTransactionRequest))
            End Try
            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oMarkUnmarkTransactionRequest))
            Return Nothing
        End Try
    End Function

    Public Function PostDocument(ByVal PostDocumentRequest As PostDocumentRequestType) As PostDocumentResponseType Implements IPureAccountService.PostDocument

        Try

            Dim sUserName As String = PostDocumentRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMPOSTDOC", iUserId)
            CommonFunctions.CheckSecurityToken(PostDocumentRequest.WCFSecurityToken)
            Dim oResponse As New PostDocumentResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, PostDocumentRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.PostDocumentRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.PostDocumentResponseType = Nothing

            oImpRequest = CommonFunctions.ToBaseImpBasePostDocumentRequestType(PostDocumentRequest)

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.PostDocument(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.DocumentRef = oImpResponse.DocumentRef

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(PostDocumentRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(PostDocumentRequest))
            Return Nothing
        End Try

    End Function
    ''' <summary>
    ''' UpdateAllocation
    ''' </summary>
    ''' <param name="oUpdateAllocationRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateAllocation(ByVal oUpdateAllocationRequest As UpdateAllocationRequestType) As UpdateAllocationResponseType Implements IPureAccountService.UpdateAllocation
        Try

            Dim sUserName As String = oUpdateAllocationRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMCPUA", iUserId)
            CommonFunctions.CheckSecurityToken(oUpdateAllocationRequest.WCFSecurityToken)
            Dim oResponse As New UpdateAllocationResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oUpdateAllocationRequest.BranchCode)

            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateAllocationRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateAllocationResponseType = Nothing

            Dim iCount As Integer = 0
            With oUpdateAllocationRequest
                oImpRequest.BranchCode = .BranchCode
                oImpRequest.AccountKey = .AccountKey
                oImpRequest.TransdetailKey = .TransdetailKey
                oImpRequest.Amount = .Amount
                oImpRequest.CashListItemKey = .CashListItemKey
                oImpRequest.TransdetailExKeySpecified = .TransdetailExKeyFieldSpecified
                oImpRequest.TransdetailExKey = .TransdetailExKey
                If Not (.Allocation Is Nothing) Then
                    Dim oAllocation As BaseUpdateAllocationRequestTypeAllocation
                    ReDim oImpRequest.Allocation(.Allocation.Count - 1)
                    For Each oAllocation In .Allocation
                        oImpRequest.Allocation(iCount) = New BaseImplementationTypes.BaseUpdateAllocationRequestTypeAllocation
                        oImpRequest.Allocation(iCount).AllocationTransdetailKey = oAllocation.AllocationTransdetailKey
                        oImpRequest.Allocation(iCount).AllocationAmount = oAllocation.AllocationAmount
                        oImpRequest.Allocation(iCount).AllocationAmountSpecified = oAllocation.AllocationAmountSpecified
                        oImpRequest.Allocation(iCount).AllocationTimeStamp = oAllocation.AllocationTimeStamp
                        oImpRequest.Allocation(iCount).AllocationTransdetailExKeySpecified = oAllocation.AllocationTransdetailExKeySpecified
                        oImpRequest.Allocation(iCount).AllocationTransdetailExKey = oAllocation.AllocationTransdetailExKey
                        oImpRequest.Allocation(iCount).WriteOffAmount = oAllocation.WriteOffAmount

                        iCount = iCount + 1
                    Next
                End If
                oImpRequest.WriteOffAmount = .WriteOffAmount
                oImpRequest.WriteOffAmountSpecified = .WriteOffAmountSpecified
                oImpRequest.WriteOffReason = .WriteOffReason
                oImpRequest.WriteOffReasonSpecified = .WriteOffReasonSpecified
                oImpRequest.CurrencyDiff = .CurrencyDiff
                oImpRequest.CurrencyDiffSpecified = .CurrencyDiffSpecified
            End With

            Try
                oImpResponse = oBusiness.UpdateAllocation(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                oResponse.AllocationStatus = oImpResponse.AllocationStatus
                oResponse.AllocationBatchId = oImpResponse.AllocationBatchId
                oResponse.AllocationId = oImpResponse.AllocationId
                oResponse.IsWrittenOff = oImpResponse.IsWrittenOff
                oResponse.WriteOffAllocationId = oImpResponse.WriteOffAllocationId
                oResponse.WriteOffTransdetailId = oImpResponse.WriteOffTransdetailId

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdateAllocationRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateAllocationRequest))
            Return Nothing
        End Try
    End Function

    Public Function UpdateReceiptMediaTypeStatus(ByVal oUpdateReceiptMediaTypeStatusRequest As UpdateReceiptMediaTypeStatusRequestType) As UpdateReceiptMediaTypeStatusResponseType Implements IPureAccountService.UpdateReceiptMediaTypeStatus

        Try

            Dim sUserName As String = oUpdateReceiptMediaTypeStatusRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMUPRPTMT", iUserId)
            CommonFunctions.CheckSecurityToken(oUpdateReceiptMediaTypeStatusRequest.WCFSecurityToken)
            Dim oResponse As New UpdateReceiptMediaTypeStatusResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oUpdateReceiptMediaTypeStatusRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateReceiptMediaTypeStatusRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateReceiptMediaTypeStatusResponseType = Nothing

            oImpRequest.BranchCode = oUpdateReceiptMediaTypeStatusRequest.BranchCode

            If oUpdateReceiptMediaTypeStatusRequest.CashListItems IsNot Nothing AndAlso oUpdateReceiptMediaTypeStatusRequest.CashListItems.Count <> 0 Then
                Dim iLength As Int32 = oUpdateReceiptMediaTypeStatusRequest.CashListItems.Count
                oImpRequest.CashListItems = New BaseImplementationTypes.BaseUpdateReceiptMediaTypeStatusRequestTypeCashListItems

                For icount As Integer = 0 To iLength - 1
                    ReDim Preserve oImpRequest.CashListItems.Row(icount)
                    oImpRequest.CashListItems.Row(icount) = New BaseImplementationTypes.BaseUpdateReceiptMediaTypeStatusRequestTypeCashListItemsRow
                    oImpRequest.CashListItems.Row(icount).CashListItemKey = oUpdateReceiptMediaTypeStatusRequest.CashListItems(icount).CashListItemKey
                    oImpRequest.CashListItems.Row(icount).Comments = oUpdateReceiptMediaTypeStatusRequest.CashListItems(icount).Comments
                    oImpRequest.CashListItems.Row(icount).DocumentRef = oUpdateReceiptMediaTypeStatusRequest.CashListItems(icount).DocumentRef
                    oImpRequest.CashListItems.Row(icount).InsuranceFileKey = oUpdateReceiptMediaTypeStatusRequest.CashListItems(icount).InsuranceFileKey
                    oImpRequest.CashListItems.Row(icount).MediaTypeCode = oUpdateReceiptMediaTypeStatusRequest.CashListItems(icount).MediaTypeCode
                    oImpRequest.CashListItems.Row(icount).MediaTypeStatusCode = oUpdateReceiptMediaTypeStatusRequest.CashListItems(icount).MediaTypeStatusCode
                    oImpRequest.CashListItems.Row(icount).ModifiedDate = oUpdateReceiptMediaTypeStatusRequest.CashListItems(icount).ModifiedDate
                Next
            End If

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateReceiptMediaTypeStatus(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdateReceiptMediaTypeStatusRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateReceiptMediaTypeStatusRequest))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' To get the Added Agent receipt response
    ''' </summary>
    ''' <param name="AddAgentReceiptRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddAgentReceipt(ByVal AddAgentReceiptRequest As AddAgentReceiptRequestType) As AddAgentReceiptResponseType Implements IPureAccountService.AddAgentReceipt

        Try
            Dim sUserName As String = AddAgentReceiptRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMADAGRPT", iUserId)
            CommonFunctions.CheckSecurityToken(AddAgentReceiptRequest.WCFSecurityToken)
            Dim oResponse As New AddAgentReceiptResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, AddAgentReceiptRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.AddAgentReceiptRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.AddAgentReceiptResponseType = Nothing

            oImpRequest.BranchCode = AddAgentReceiptRequest.BranchCode

            Dim oImpReceipt As New BaseImplementationTypes.BaseReceiptType

            oImpReceipt.AgentKey = iAgentKey
            oImpReceipt.BankAccountName = AddAgentReceiptRequest.Receipt.BankAccountName
            oImpReceipt.CurrencyCode = AddAgentReceiptRequest.Receipt.CurrencyCode
            oImpReceipt.Address1 = AddAgentReceiptRequest.Receipt.Address1
            oImpReceipt.Address2 = AddAgentReceiptRequest.Receipt.Address2
            oImpReceipt.Address3 = AddAgentReceiptRequest.Receipt.Address3
            oImpReceipt.Address4 = AddAgentReceiptRequest.Receipt.Address4
            oImpReceipt.Amount = AddAgentReceiptRequest.Receipt.Amount
            oImpReceipt.CashListRef = AddAgentReceiptRequest.Receipt.CashListRef
            oImpReceipt.CCAuthCode = AddAgentReceiptRequest.Receipt.CCAuthCode
            oImpReceipt.CCCustomer = AddAgentReceiptRequest.Receipt.CCCustomer
            oImpReceipt.CCExpiryDate = AddAgentReceiptRequest.Receipt.CCExpiryDate
            oImpReceipt.CCIssue = AddAgentReceiptRequest.Receipt.CCIssue
            oImpReceipt.CCManualAuthCode = AddAgentReceiptRequest.Receipt.CCManualAuthCode
            oImpReceipt.CCName = AddAgentReceiptRequest.Receipt.CCName
            oImpReceipt.CCNumber = AddAgentReceiptRequest.Receipt.CCNumber
            oImpReceipt.CCPin = AddAgentReceiptRequest.Receipt.CCPin
            oImpReceipt.CCStartDate = AddAgentReceiptRequest.Receipt.CCStartDate
            oImpReceipt.CCTransactionCode = AddAgentReceiptRequest.Receipt.CCTransactionCode
            oImpReceipt.ChequeDate = AddAgentReceiptRequest.Receipt.ChequeDate
            oImpReceipt.ChequeDateSpecified = AddAgentReceiptRequest.Receipt.ChequeDateSpecified
            oImpReceipt.ChequeName = AddAgentReceiptRequest.Receipt.ChequeName
            oImpReceipt.ContactName = AddAgentReceiptRequest.Receipt.ContactName
            oImpReceipt.CountryCode = AddAgentReceiptRequest.Receipt.CountryCode
            oImpReceipt.MediaReference = AddAgentReceiptRequest.Receipt.MediaReference
            oImpReceipt.MediaTypeCode = AddAgentReceiptRequest.Receipt.MediaTypeCode
            oImpReceipt.MediaTypeIssuerCode = AddAgentReceiptRequest.Receipt.MediaTypeIssuerCode
            oImpReceipt.OurReference = AddAgentReceiptRequest.Receipt.OurReference
            oImpReceipt.PostalCode = AddAgentReceiptRequest.Receipt.PostalCode
            oImpReceipt.ReceiptTypeCode = AddAgentReceiptRequest.Receipt.ReceiptTypeCode
            oImpReceipt.TheirReference = AddAgentReceiptRequest.Receipt.TheirReference
            oImpReceipt.TransactionDate = AddAgentReceiptRequest.Receipt.TransactionDate

            oImpReceipt.CollectionDateSpecified = AddAgentReceiptRequest.Receipt.CollectionDateSpecified
            If (oImpReceipt.CollectionDateSpecified = AddAgentReceiptRequest.Receipt.CollectionDateSpecified) Then
                oImpReceipt.CollectionDate = AddAgentReceiptRequest.Receipt.CollectionDate
            End If
            oImpReceipt.Comments = AddAgentReceiptRequest.Receipt.Comments

            ' assign the implementation receipt to the request
            oImpRequest.Receipt = oImpReceipt

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.AddAgentReceipt(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(AddAgentReceiptRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(AddAgentReceiptRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>  
    ''' This web services method is used to Get Insurer Payments
    ''' </summary>  
    ''' <param name="oGetInsurerPaymentsRequest">An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetInsurerPaymentsRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetInsurerPaymentsResponseType</returns>  
    Public Function GetInsurerPayments(ByVal oGetInsurerPaymentsRequest As GetInsurerPaymentsRequestType) As GetInsurerPaymentsResponseType Implements IPureAccountService.GetInsurerPayments

        Try

            Dim sUserName As String = oGetInsurerPaymentsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New GetInsurerPaymentsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetInsurerPaymentsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetInsurerPaymentsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetInsurerPaymentsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetInsurerPaymentsRequest.BranchCode
            oImpRequest.AccountKey = oGetInsurerPaymentsRequest.AccountKey
            oImpRequest.AlternateReference = oGetInsurerPaymentsRequest.AlternateReference
            oImpRequest.PolicyNumber = oGetInsurerPaymentsRequest.PolicyNumber

            oImpRequest.DateByTransaction = CType(oGetInsurerPaymentsRequest.DateByTransaction, BaseImplementationTypes.InsurerPaymentsDateByType)
            oImpRequest.DateByTransactionSpecified = oGetInsurerPaymentsRequest.DateByTransactionSpecified
            oImpRequest.DateTo = oGetInsurerPaymentsRequest.DateTo
            oImpRequest.DateToSpecified = oGetInsurerPaymentsRequest.DateToSpecified
            oImpRequest.InsurerPaymentBranchCode = oGetInsurerPaymentsRequest.InsurerPaymentBranchCode
            oImpRequest.MarkedStatus = CType(oGetInsurerPaymentsRequest.MarkedStatus, BaseImplementationTypes.InsurerPaymentsMarkedStatus)
            oImpRequest.MarkedStatusSpecified = oGetInsurerPaymentsRequest.MarkedStatusSpecified
            oImpRequest.Month = CType(oGetInsurerPaymentsRequest.Month, BaseImplementationTypes.Month)
            oImpRequest.MonthSpecified = oGetInsurerPaymentsRequest.MonthSpecified
            oImpRequest.YearName = oGetInsurerPaymentsRequest.YearName
            oImpRequest.PeriodName = oGetInsurerPaymentsRequest.PeriodName
            oImpRequest.CurrencyCode = oGetInsurerPaymentsRequest.CurrencyCode
            oImpRequest.DateFrom = oGetInsurerPaymentsRequest.DateFrom
            oImpRequest.Reference = oGetInsurerPaymentsRequest.Reference
            oImpRequest.GrossAgent = oGetInsurerPaymentsRequest.GrossAgent
            oImpRequest.MediaType = oGetInsurerPaymentsRequest.MediaType
            oImpRequest.DateFromSpecified = oGetInsurerPaymentsRequest.DateFromSpecified
            oImpRequest.WCFSecurityToken = If(oGetInsurerPaymentsRequest.WCFSecurityToken.Length > 0, oGetInsurerPaymentsRequest.WCFSecurityToken, "WCFSecurityToken")

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetInsurerPayments(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.InsurerPayments = SAMFunc.GetDeserializedValues(Of List(Of BaseGetInsurerPaymentsResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetInsurerPaymentsResponseTypeInsurerPayments", sConvertToTypeName:="BaseGetInsurerPaymentsResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.InsurerPayments = DataTabletoList_GetInsurerPayments(oImpResponse.ResultData.Tables(0))
                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetInsurerPaymentsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetInsurerPaymentsRequest))
            Return Nothing
        End Try

    End Function

    Public Function ReverseAllocation(ByVal oReverseAllocationRequest As ReverseAllocationRequestType) As ReverseAllocationResponseType Implements IPureAccountService.ReverseAllocation

        Try
            Dim sUserName As String = oReverseAllocationRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMREVALOC", iUserId)
            CommonFunctions.CheckSecurityToken(oReverseAllocationRequest.WCFSecurityToken)

            Dim oResponse As New ReverseAllocationResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oReverseAllocationRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.ReverseAllocationRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.ReverseAllocationResponseType = Nothing

            ' Pass the values to the implementation request structure

            oImpRequest.BranchCode = oReverseAllocationRequest.BranchCode
            oImpRequest.TransDetailKey = oReverseAllocationRequest.TransDetailKey
            oImpRequest.AllocationKey = oReverseAllocationRequest.AllocationKey
            oImpRequest.IgnoreWarnings = oReverseAllocationRequest.IgnoreWarnings

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.ReverseAllocation(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                oResponse.Warnings = oImpResponse.Warnings
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oReverseAllocationRequest))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oReverseAllocationRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetBankAccounts(ByVal oGetBankAccountsRequest As GetBankAccountsRequestType) As GetBankAccountsResponseType Implements IPureAccountService.GetBankAccounts, IPureClaimService.GetBankAccounts
        Try
            Dim sUserName As String = oGetBankAccountsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGClmDet", iUserId)
            CommonFunctions.CheckSecurityToken(oGetBankAccountsRequest.WCFSecurityToken)

            Dim oResponse As New GetBankAccountsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetBankAccountsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetBankAccountsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetBankAccountsResponseType = Nothing

            oImpRequest.BranchCode = oGetBankAccountsRequest.BranchCode
            oImpRequest.BankAccountKey = oGetBankAccountsRequest.BankAccountKey
            oImpRequest.BankAccountKeySpecified = oGetBankAccountsRequest.BankAccountKeySpecified
            oImpRequest.WCFSecurityToken = If(oGetBankAccountsRequest.WCFSecurityToken.Length > 0, oGetBankAccountsRequest.WCFSecurityToken, "WCFSecurityToken")

            ' Pass the values to the implementation request structure

            ' Call the implementation method
            Try
                oImpResponse = oBusiness.GetBankAccounts(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.BankAccounts = SAMFunc.GetDeserializedValues(Of List(Of BaseGetBankAccountsResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataSet, sFromTypeName:="BaseGetBankAccountsResponseTypeBankAccounts", sConvertToTypeName:="BaseGetBankAccountsResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.BankAccounts = DataTabletoList_GetBankAccounts(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetBankAccountsRequest))
            Return Nothing
        End Try

    End Function


    ''' <summary>
    ''' FindPaymentDetails
    ''' </summary>
    ''' <param name="oFindPaymentDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindPaymentDetails(ByVal oFindPaymentDetailsRequest As FindPaymentDetailsRequestType) As FindPaymentDetailsResponseType Implements IPureAccountService.FindPaymentDetails

        Dim iAgentKey As Integer
        Dim iUserId As Integer
        Dim sUserName As String
        Dim oResponse As New FindPaymentDetailsResponseType
        Dim oBusiness As CoreSAMBusiness
        Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.FindPaymentDetailsRequestType
        Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.FindPaymentDetailsResponseType = Nothing

        Try
            sUserName = oFindPaymentDetailsRequest.LoginUserName
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMFINDPMT", iUserId)
            CommonFunctions.CheckSecurityToken(oFindPaymentDetailsRequest.WCFSecurityToken)
            oBusiness = New CoreSAMBusiness(sUserName, oFindPaymentDetailsRequest.BranchCode)

            ' Implementation structures
            With oFindPaymentDetailsRequest
                oImpRequest.BranchCode = .BranchCode
                oImpRequest.ClientCode = .ClientCode
                oImpRequest.ClientAccountNumber = .ClientAccountNumber
                oImpRequest.PayeeName = .PayeeName
                oImpRequest.PaymentStatus = .PaymentStatus
                oImpRequest.BatchReference = .BatchReference
                oImpRequest.BankAccount = .BankAccount
                oImpRequest.PolicyClaimNumber = .PolicyClaimNumber
                oImpRequest.MaxRowsToFetch = .MaxRowsToFetch
                oImpRequest.MaxRowsToFetchSpecified = .MaxRowsToFetchSpecified
                oImpRequest.PaymentType = .PaymentType
                oImpRequest.MediaType = .MediaType
                oImpRequest.MediaReferenceFrom = .MediaReferenceFrom
                oImpRequest.MediaReferenceTo = .MediaReferenceTo
                oImpRequest.AmountRangeFrom = .AmountRangeFrom
                oImpRequest.AmountrangeTo = .AmountrangeTo
                oImpRequest.DateFrom = .DateFrom
                oImpRequest.DateTo = .DateTo
                oImpRequest.ShowOnlyOutStanding = .ShowOnlyOutStanding
                oImpRequest.PaymentBranch = .PaymentBranch
            End With

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.FindPaymentDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.PaymentDetails = SAMFunc.GetDeserializedValues(Of List(Of BaseFindPaymentDetailsResponseTypePaymentDetails))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseFindPaymentDetailsResponseTypePaymentDetails", sConvertToTypeName:="BaseFindPaymentDetailsResponseTypePaymentDetails")

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oFindPaymentDetailsRequest))
        End Try
    End Function

    ''' <summary>
    ''' CancelPayment
    ''' </summary>
    ''' <param name="oCancelPaymentRequest"></param>
    ''' <remarks></remarks>
    Public Sub CancelPayment(ByVal oCancelPaymentRequest As CancelPaymentRequestType) Implements IPureAccountService.CancelPayment

        Try
            Dim sUserName As String = oCancelPaymentRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMCANPMT", iUserId)
            CommonFunctions.CheckSecurityToken(oCancelPaymentRequest.WCFSecurityToken)



            Dim oResponse As New CancelPaymentResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oCancelPaymentRequest.BranchCode)
            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.CancelPaymentRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.CancelPaymentResponseType = Nothing

            With oCancelPaymentRequest
                oImpRequest.BranchCode = .BranchCode
                oImpRequest.TransDetailKey = .TransDetailKey
                oImpRequest.ReverseReasonKey = .ReverseReasonKey
                oImpRequest.CashListItemKey = .CashListItemKey
                oImpRequest.InsuranceFileKey = .InsuranceFileKey
            End With


            Try
                ' Call the implementation method
                oImpResponse = oBusiness.CancelPayment(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oCancelPaymentRequest))
        End Try
    End Sub

    ''' <summary>
    ''' FindReceiptDetails
    ''' </summary>
    ''' <param name="oFindReceiptDetailsRequest"></param>
    ''' <remarks></remarks>
    Public Function FindReceiptDetails(ByVal oFindReceiptDetailsRequest As FindReceiptDetailsRequestType) As FindReceiptDetailsResponseType Implements IPureAccountService.FindReceiptDetails

        Try
            Dim sUserName As String = oFindReceiptDetailsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMFINDRCT", iUserId)
            CommonFunctions.CheckSecurityToken(oFindReceiptDetailsRequest.WCFSecurityToken)



            Dim oResponse As New FindReceiptDetailsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oFindReceiptDetailsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.FindReceiptDetailsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.FindReceiptDetailsResponseType = Nothing

            With oFindReceiptDetailsRequest
                oImpRequest.BranchCode = .BranchCode
                oImpRequest.ClientCode = .ClientCode
                oImpRequest.ClientAccountNumber = .ClientAccountNumber
                oImpRequest.PayeeName = .PayeeName
                oImpRequest.ReceiptStatus = .ReceiptStatus
                oImpRequest.BatchReference = .BatchReference
                oImpRequest.BankAccount = .BankAccount
                oImpRequest.PolicyClaimNumber = .PolicyClaimNumber
                oImpRequest.MaxRowsToFetch = .MaxRowsToFetch
                oImpRequest.MaxRowsToFetchSpecified = .MaxRowsToFetchSpecified
                oImpRequest.MediaType = .MediaType
                oImpRequest.MediaReferenceFrom = .MediaReferenceFrom
                oImpRequest.MediaReferenceTo = .MediaReferenceTo
                oImpRequest.AmountRangeFrom = .AmountRangeFrom
                oImpRequest.AmountRangeFromSpecified = .AmountRangeFromSpecified
                oImpRequest.AmountrangeTo = .AmountrangeTo
                oImpRequest.AmountrangeToSpecified = .AmountrangeToSpecified
                oImpRequest.DateFrom = .DateFrom
                oImpRequest.DateFromSpecified = .DateFromSpecified
                oImpRequest.DateTo = .DateTo
                oImpRequest.DateToSpecified = .DateToSpecified
                oImpRequest.ShowOnlyOutStanding = .ShowOnlyOutStanding
                oImpRequest.ReceiptBranch = .ReceiptBranch
                oImpRequest.DocumentReference = .DocumentReference
            End With

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.FindReceiptDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.ReceiptDetails = SAMFunc.GetDeserializedValues(Of List(Of BaseFindReceiptDetailsResponseTypeReceiptDetails))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseFindReceiptDetailsResponseTypeReceiptDetails", sConvertToTypeName:="BaseFindReceiptDetailsResponseTypeReceiptDetails")

                ' Retrieve the values from the implementation response structure


            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oFindReceiptDetailsRequest))
        End Try
    End Function

    ''' <summary>
    ''' CancelReceipt
    ''' </summary>
    ''' <param name="CancelReceiptRequest"></param>
    ''' <remarks></remarks>
    Public Function CancelReceipt(ByVal oCancelReceiptRequest As CancelReceiptRequestType) As CancelReceiptResponseType Implements IPureAccountService.CancelReceipt
        Try
            Dim sUserName As String = oCancelReceiptRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMCANRCT", iUserId)
            CommonFunctions.CheckSecurityToken(oCancelReceiptRequest.WCFSecurityToken)


            Dim oResponse As New CancelReceiptResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oCancelReceiptRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.CancelReceiptRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.CancelReceiptResponseType = Nothing

            With oCancelReceiptRequest
                oImpRequest.BranchCode = .BranchCode
                oImpRequest.TransDetailKey = .TransDetailKey
                oImpRequest.ReverseReasonKey = .ReverseReasonKey
                oImpRequest.CashListItemKey = .CashListItemKey
                oImpRequest.ReverseReasonCode = .ReverseReasonCode
            End With

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.CancelReceipt(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oCancelReceiptRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oCancelReceiptRequest))
            Return Nothing

        End Try
    End Function

    ''' <summary>
    ''' GetPaymentTypeCashListItem
    ''' </summary>
    ''' <param name="oGetPaymentTypeCashListItemRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPaymentTypeCashListItem(ByVal oGetPaymentTypeCashListItemRequest As GetPaymentTypeCashListItemRequestType) As GetPaymentTypeCashListItemResponseType Implements IPureAccountService.GetPaymentTypeCashListItem
        Try
            Dim sUserName As String = oGetPaymentTypeCashListItemRequest.LoginUserName
            Dim nAgentKey As Integer
            Dim nUserId As Integer

            CommonFunctions.GetIdentity(sUserName, nAgentKey, nUserId)
            CommonFunctions.CheckAuthority("SAMGPCLDET", nUserId)
            CommonFunctions.CheckSecurityToken(oGetPaymentTypeCashListItemRequest.WCFSecurityToken)

            Dim oResponse As GetPaymentTypeCashListItemResponseType = Nothing
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetPaymentTypeCashListItemRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetPaymentTypeCashListItemRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetPaymentTypeCashListItemResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.LoginUserName = oGetPaymentTypeCashListItemRequest.LoginUserName
            oImpRequest.BranchCode = oGetPaymentTypeCashListItemRequest.BranchCode
            oImpRequest.CashListItemKey = oGetPaymentTypeCashListItemRequest.CashListItemKey

            Try
                oResponse = New GetPaymentTypeCashListItemResponseType
                oResponse.CashList = New List(Of BasePaymentCashListType)
                ' Call the implementation method
                oImpResponse = oBusiness.GetPaymentTypeCashListItem(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                If oImpResponse.CashList IsNot Nothing Then
                    Dim oCashList As New BasePaymentCashListType
                    oCashList.BankAccountCode = oImpResponse.CashList(0).BankAccountCode
                    oCashList.CurrencyCode = oImpResponse.CashList(0).CurrencyCode
                    oCashList.Reference = oImpResponse.CashList(0).Reference
                    oCashList.StatusCode = oImpResponse.CashList(0).StatusCode
                    oCashList.TypeCode = oImpResponse.CashList(0).TypeCode
                    oCashList.ListDate = oImpResponse.CashList(0).ListDate
                    oCashList.BankAccountKey = oImpResponse.CashList(0).BankAccountKey
                    Dim oPaymentItem As BasePaymentCashListItemType

                    With oCashList
                        .PaymentItem = New List(Of BasePaymentCashListItemType)

                        For iCount As Integer = 0 To oImpResponse.CashList(0).PaymentItem.Length - 1
                            oPaymentItem = New BasePaymentCashListItemType

                            oPaymentItem.AccountShortCode = oImpResponse.CashList(0).PaymentItem(iCount).AccountShortCode
                            oPaymentItem.AllocationStatusCode = oImpResponse.CashList(0).PaymentItem(iCount).AllocationStatusCode
                            oPaymentItem.Amount = oImpResponse.CashList(0).PaymentItem(iCount).Amount
                            oPaymentItem.AllocationStatusCode = oImpResponse.CashList(0).PaymentItem(iCount).AllocationStatusCode
                            oPaymentItem.StatusCode = oImpResponse.CashList(0).PaymentItem(iCount).StatusCode
                            oPaymentItem.CashListItemKey = oImpResponse.CashList(0).PaymentItem(iCount).CashListItemKey
                            oPaymentItem.CashListItemKeySpecified = True
                            oPaymentItem.ContactName = oImpResponse.CashList(0).PaymentItem(iCount).ContactName
                            oPaymentItem.MediaReference = oImpResponse.CashList(0).PaymentItem(iCount).MediaReference
                            oPaymentItem.MediaTypeCode = oImpResponse.CashList(0).PaymentItem(iCount).MediaTypeCode
                            oPaymentItem.OurReference = oImpResponse.CashList(0).PaymentItem(iCount).OurReference
                            oPaymentItem.TheirReference = oImpResponse.CashList(0).PaymentItem(iCount).TheirReference
                            oPaymentItem.TransactionDate = oImpResponse.CashList(0).PaymentItem(iCount).TransactionDate
                            oPaymentItem.FurtherDetails = oImpResponse.CashList(0).PaymentItem(iCount).FurtherDetails
                            oPaymentItem.TypeCode = oImpResponse.CashList(0).PaymentItem(iCount).TypeCode

                            .PaymentItem.Add(oPaymentItem)
                            Dim oBank As New BaseBankPaymentType

                            oBank.AccountCode = oImpResponse.CashList(0).PaymentItem(iCount).Bank.AccountCode
                            oBank.BranchCode = oImpResponse.CashList(0).PaymentItem(iCount).Bank.BranchCode
                            oBank.ExpiryDate = oImpResponse.CashList(0).PaymentItem(iCount).Bank.ExpiryDate
                            oBank.ExpiryDateSpecified = oImpResponse.CashList(0).PaymentItem(iCount).Bank.ExpiryDateSpecified
                            oBank.PartyBankKey = oImpResponse.CashList(0).PaymentItem(iCount).Bank.PartyBankKey
                            oBank.PayeeName = oImpResponse.CashList(0).PaymentItem(iCount).Bank.PayeeName
                            oBank.Reference1 = oImpResponse.CashList(0).PaymentItem(iCount).Bank.Reference1
                            oBank.Reference2 = oImpResponse.CashList(0).PaymentItem(iCount).Bank.Reference2
                            oBank.BIC = oImpResponse.CashList(0).PaymentItem(iCount).Bank.BIC
                            oBank.IBAN = oImpResponse.CashList(0).PaymentItem(iCount).Bank.IBAN
                            .PaymentItem(iCount).Bank = oBank

                            Dim oAddress As New BaseSimpleAddressType
                            oAddress.AddressLine1 = oImpResponse.CashList(0).PaymentItem(iCount).ContactAddress.AddressLine1
                            oAddress.AddressLine2 = oImpResponse.CashList(0).PaymentItem(iCount).ContactAddress.AddressLine2
                            oAddress.AddressLine3 = oImpResponse.CashList(0).PaymentItem(iCount).ContactAddress.AddressLine3
                            oAddress.AddressLine4 = oImpResponse.CashList(0).PaymentItem(iCount).ContactAddress.AddressLine4
                            oAddress.CountryCode = oImpResponse.CashList(0).PaymentItem(iCount).ContactAddress.CountryCode
                            oAddress.PostCode = oImpResponse.CashList(0).PaymentItem(iCount).ContactAddress.PostCode
                            .PaymentItem(iCount).ContactAddress = oAddress


                            Dim oCreditCard As New BaseCreditCardType
                            oCreditCard.Number = oImpResponse.CashList(0).PaymentItem(iCount).CreditCard.Number
                            oCreditCard.ExpiryDate = oImpResponse.CashList(0).PaymentItem(iCount).CreditCard.ExpiryDate
                            oCreditCard.StartDate = oImpResponse.CashList(0).PaymentItem(iCount).CreditCard.StartDate
                            oCreditCard.Issue = oImpResponse.CashList(0).PaymentItem(iCount).CreditCard.Issue
                            oCreditCard.Pin = oImpResponse.CashList(0).PaymentItem(iCount).CreditCard.Pin
                            oCreditCard.AuthCode = oImpResponse.CashList(0).PaymentItem(iCount).CreditCard.AuthCode
                            oCreditCard.NameOnCreditCard = oImpResponse.CashList(0).PaymentItem(iCount).CreditCard.NameOnCreditCard
                            oCreditCard.ManualAuthCode = oImpResponse.CashList(0).PaymentItem(iCount).CreditCard.ManualAuthCode
                            oCreditCard.TransactionCode = oImpResponse.CashList(0).PaymentItem(iCount).CreditCard.TransactionCode
                            oCreditCard.CustomerPresent = oImpResponse.CashList(0).PaymentItem(iCount).CreditCard.CustomerPresent
                            .PaymentItem(iCount).CreditCard = oCreditCard

                        Next
                    End With
                    oResponse.CashList.add(oCashList)
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetPaymentTypeCashListItemRequest))
            Finally
                oBusiness = Nothing
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetPaymentTypeCashListItemRequest))
            Return Nothing
        End Try

    End Function
#Region "GetTransactionDetailsEx"

    ''' <summary>  
    ''' This web services method is used to Get Transaction Details
    ''' </summary>  
    ''' <param name="oGetTransactionDetailsExRequest">An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetTransactionDetailsExRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetTransactionDetailsExResponseType</returns>  

    Public Function GetTransactionDetailsEx(ByVal oGetTransactionDetailsExRequest As GetTransactionDetailsExRequestType) As GetTransactionDetailsExResponseType Implements IPureAccountService.GetTransactionDetailsEx

        Try

            Dim sUserName As String = oGetTransactionDetailsExRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGTRAKey", iUserId)
            CommonFunctions.CheckSecurityToken(oGetTransactionDetailsExRequest.WCFSecurityToken)
            Dim oResponse As New GetTransactionDetailsExResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetTransactionDetailsExRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetTransactionDetailsExRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetTransactionDetailsExResponseType = Nothing

            ' Pass the values to the implementation request structure and do remember to map the specified fields also
            With oGetTransactionDetailsExRequest
                oImpRequest.BranchCode = .BranchCode

                If oGetTransactionDetailsExRequest.Allocation IsNot Nothing AndAlso oGetTransactionDetailsExRequest.Allocation.Count > 0 Then
                    oImpRequest.Allocation = New BaseImplementationTypes.BaseGetTransactionDetailsExRequestTypeAllocation
                    For iCount As Integer = 0 To oGetTransactionDetailsExRequest.Allocation.Count - 1
                        ReDim Preserve oImpRequest.Allocation.Row(iCount)
                        oImpRequest.Allocation.Row(iCount) = New BaseImplementationTypes.BaseGetTransactionDetailsExRequestTypeAllocationRow
                        oImpRequest.Allocation.Row(iCount).AllocationTransDetailKey = .Allocation(iCount).AllocationTransDetailKey
                    Next
                End If
            End With

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetTransactionDetailsEx(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                If (oImpResponse.Transactions IsNot Nothing) Then
                    Dim oTransaction As BaseGetTransactionDetailsExResponseTypeTransactions
                    oResponse.Transactions = New List(Of BaseGetTransactionDetailsExResponseTypeTransactions)

                    For iCount As Integer = 0 To oImpResponse.Transactions.Length - 1
                        oTransaction = New BaseGetTransactionDetailsExResponseTypeTransactions
                        ' oResponse.Transactions(iCount) = New BaseGetTransactionDetailsExResponseTypeTransactions
                        oTransaction.AccountCode = oImpResponse.Transactions(iCount).AccountCode
                        oTransaction.Amount = oImpResponse.Transactions(iCount).Amount
                        oTransaction.Outstanding_Amount = oImpResponse.Transactions(iCount).Outstanding_Amount
                        oTransaction.CurrencyCode = oImpResponse.Transactions(iCount).CurrencyCode
                        oTransaction.DocumentReference = oImpResponse.Transactions(iCount).DocumentReference
                        oTransaction.TransDetailKey = oImpResponse.Transactions(iCount).TransDetailKey
                        oTransaction.AllocationTimeStamp = oImpResponse.Transactions(iCount).AllocationTimeStamp
                        oTransaction.Extended = New List(Of BaseGetTransactionDetailsExResponseTypeTransactionsExtended)
                        If (oImpResponse.Transactions(iCount).Extended IsNot Nothing) Then
                            Dim oTransactionEx As BaseGetTransactionDetailsExResponseTypeTransactionsExtended
                            For iCnt As Integer = 0 To oImpResponse.Transactions(iCount).Extended.Length - 1
                                oTransactionEx = New BaseGetTransactionDetailsExResponseTypeTransactionsExtended
                                oTransactionEx.DueDate = oImpResponse.Transactions(iCount).Extended(iCnt).DueDate
                                oTransactionEx.DueAmount = oImpResponse.Transactions(iCount).Extended(iCnt).DueAmount
                                oTransactionEx.ExtendedId = oImpResponse.Transactions(iCount).Extended(iCnt).ExtendedId
                                oTransactionEx.OutstandingAmount = oImpResponse.Transactions(iCount).Extended(iCnt).OutstandingAmount
                                oTransaction.Extended.Add(oTransactionEx)
                            Next iCnt
                        End If
                        oResponse.Transactions.Add(oTransaction)
                    Next iCount
                End If

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetTransactionDetailsExRequest))
            Return Nothing
        End Try

    End Function

#End Region

#Region "FindPolicyTransactionGrouped"

    ''' <summary>  
    ''' This web services method is used to Find Policy Transactions
    ''' </summary>  
    ''' <param name="oFindPolicyTransactionGroupedRequest">An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.FindPolicyTransactionGroupedRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.FindPolicyTransactionGroupedResponseType</returns>  

    Public Function FindPolicyTransactionGrouped(ByVal oFindPolicyTransactionGroupedRequest As FindPolicyTransactionGroupedRequestType) As FindPolicyTransactionGroupedResponseType Implements IPureAccountService.FindPolicyTransactionGrouped
        Try
            Dim sUserName As String = oFindPolicyTransactionGroupedRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            'CheckAuthority("SAMUPDCRP")
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMUPDCRP", iUserId)
            CommonFunctions.CheckSecurityToken(oFindPolicyTransactionGroupedRequest.WCFSecurityToken)
            Dim oResponse As New FindPolicyTransactionGroupedResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oFindPolicyTransactionGroupedRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.FindPolicyTransactionGroupedRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.FindPolicyTransactionGroupedResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oFindPolicyTransactionGroupedRequest.BranchCode
            oImpRequest.AgentCode = oFindPolicyTransactionGroupedRequest.AgentCode
            oImpRequest.AgentCodeSpecified = oFindPolicyTransactionGroupedRequest.AgentCodeSpecified
            oImpRequest.ClientCode = oFindPolicyTransactionGroupedRequest.ClientCode
            oImpRequest.ClientCodeSpecified = oFindPolicyTransactionGroupedRequest.ClientCodeSpecified
            oImpRequest.DueDate = oFindPolicyTransactionGroupedRequest.DueDate
            oImpRequest.DueDateSpecified = oFindPolicyTransactionGroupedRequest.DueDateSpecified
            oImpRequest.EffectiveFromDate = oFindPolicyTransactionGroupedRequest.EffectiveFromDate
            oImpRequest.EffectiveFromDateSpecified = oFindPolicyTransactionGroupedRequest.EffectiveFromDateSpecified
            oImpRequest.EffectiveToDate = oFindPolicyTransactionGroupedRequest.EffectiveToDate
            oImpRequest.EffectiveToDateSpecified = oFindPolicyTransactionGroupedRequest.EffectiveToDateSpecified
            oImpRequest.OnlyOutstanding = oFindPolicyTransactionGroupedRequest.OnlyOutstanding
            oImpRequest.PolicyReference = oFindPolicyTransactionGroupedRequest.PolicyReference
            oImpRequest.PolicyReferenceSpecified = oFindPolicyTransactionGroupedRequest.PolicyReferenceSpecified

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.FindPolicyTransactionGrouped(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                If Not oImpResponse.Policies Is Nothing Then
                    Dim cntPolicies As Integer
                    Dim cntPoliciesFee As Integer

                    Dim uBoundPolicies As Integer = oImpResponse.Policies.GetUpperBound(0)
                    Dim lBoundPolicies As Integer = oImpResponse.Policies.GetLowerBound(0)

                    'ReDim oResponse.Policies(uBoundPolicies)
                    oResponse.Policies = New List(Of BaseFindPolicyTransactionGroupedResponseTypePolicies)
                    Dim oPolicies As New BaseFindPolicyTransactionGroupedResponseTypePolicies

                    For cntPolicies = lBoundPolicies To uBoundPolicies
                        With oImpResponse.Policies(cntPolicies)
                            oPolicies = New BaseFindPolicyTransactionGroupedResponseTypePolicies
                            oPolicies.ClientCode = oImpResponse.Policies(cntPolicies).ClientCode
                            oPolicies.ClientName = oImpResponse.Policies(cntPolicies).ClientName
                            oPolicies.Commission = oImpResponse.Policies(cntPolicies).Commission
                            oPolicies.CommissionOS = oImpResponse.Policies(cntPolicies).CommissionOS
                            oPolicies.CommissionTax = oImpResponse.Policies(cntPolicies).CommissionTax
                            oPolicies.CommissionTaxOS = oImpResponse.Policies(cntPolicies).CommissionTaxOS
                            oPolicies.PolicyCurrency = oImpResponse.Policies(cntPolicies).PolicyCurrency
                            oPolicies.PolicyFolderId = oImpResponse.Policies(cntPolicies).PolicyFolderId
                            oPolicies.PolicyNumber = oImpResponse.Policies(cntPolicies).PolicyNumber
                            oPolicies.Premium = oImpResponse.Policies(cntPolicies).Premium
                            oPolicies.PremiumOS = oImpResponse.Policies(cntPolicies).PremiumOS
                            oPolicies.PremiumTax = oImpResponse.Policies(cntPolicies).PremiumTax
                            oPolicies.PremiumTaxOS = oImpResponse.Policies(cntPolicies).PremiumTaxOS
                            If Not oImpResponse.Policies(cntPolicies).Fees Is Nothing Then
                                Dim lBoundPoliciesFee As Integer = oImpResponse.Policies(cntPolicies).Fees.GetLowerBound(0)
                                Dim uBoundPoliciesFee As Integer = oImpResponse.Policies(cntPolicies).Fees.GetUpperBound(0)
                                Dim oFees As BaseFindPolicyTransactionGroupedResponseTypePoliciesFees
                                oPolicies.Fees = New List(Of BaseFindPolicyTransactionGroupedResponseTypePoliciesFees)
                                For cntPoliciesFee = lBoundPoliciesFee To uBoundPoliciesFee
                                    oFees = New BaseFindPolicyTransactionGroupedResponseTypePoliciesFees
                                    oFees.FeeAmount = oImpResponse.Policies(cntPolicies).Fees(cntPoliciesFee).FeeAmount
                                    oFees.FeeAmountOS = oImpResponse.Policies(cntPolicies).Fees(cntPoliciesFee).FeeAmountOS
                                    oFees.FeeTaxAmount = oImpResponse.Policies(cntPolicies).Fees(cntPoliciesFee).FeeTaxAmount
                                    oFees.FeeTaxAmountOS = oImpResponse.Policies(cntPolicies).Fees(cntPoliciesFee).FeeTaxAmountOS
                                    oFees.FeeType = oImpResponse.Policies(cntPolicies).Fees(cntPoliciesFee).FeeType
                                    oPolicies.Fees.Add(oFees)
                                Next
                            End If
                            oResponse.Policies.Add(oPolicies)
                        End With
                    Next
                End If

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oFindPolicyTransactionGroupedRequest))
            Return Nothing
        End Try

    End Function

#End Region


#Region "GetPolicyTransactionDetails"
    ''' <summary>
    ''' GetPolicyTransactionDetails
    ''' </summary>
    ''' <param name="oGetPolicyTransactionDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPolicyTransactionDetails(ByVal oGetPolicyTransactionDetailsRequest As GetPolicyTransactionDetailsRequestType) As GetPolicyTransactionDetailsResponseType Implements IPureAccountService.GetPolicyTransactionDetails

        Try
            'CheckAuthority("")
            Dim sUserName As String = oGetPolicyTransactionDetailsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMUPDCRP", iUserId)
            CommonFunctions.CheckSecurityToken(oGetPolicyTransactionDetailsRequest.WCFSecurityToken)

            Dim oResponse As New GetPolicyTransactionDetailsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetPolicyTransactionDetailsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetPolicyTransactionDetailsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetPolicyTransactionDetailsResponseType = Nothing

            oImpRequest.BranchCode = oGetPolicyTransactionDetailsRequest.BranchCode
            oImpRequest.InsuranceFolderKey = oGetPolicyTransactionDetailsRequest.InsuranceFolderKey
            oImpRequest.OnlyOutstanding = oGetPolicyTransactionDetailsRequest.OnlyOutstanding
            oImpRequest.DueByDate = oGetPolicyTransactionDetailsRequest.DueByDate
            oImpRequest.DueByDateSpecified = oGetPolicyTransactionDetailsRequest.DueByDateSpecified

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetPolicyTransactionDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                If oImpResponse IsNot Nothing AndAlso oImpResponse.Transactions IsNot Nothing Then
                    oResponse.Transactions = oImpResponse.Transactions.ToList().ConvertAll(
                            New Converter(Of BaseImplementationTypes.BaseGetPolicyTransactionDetailsResponseTypeTransactions, BaseGetPolicyTransactionDetailsResponseTypeTransactions)(AddressOf ToServiceGetPolicyTransactionDetailsResponseTypeTransactions))
                End If
            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetPolicyTransactionDetailsRequest))
            Return Nothing
        End Try

    End Function


    Private Function ToServiceGetPolicyTransactionDetailsResponseTypeTransactions(ByVal oImpTransactions As BaseImplementationTypes.BaseGetPolicyTransactionDetailsResponseTypeTransactions) As BaseGetPolicyTransactionDetailsResponseTypeTransactions

        Dim oServiceTransactions As New BaseGetPolicyTransactionDetailsResponseTypeTransactions

        If oImpTransactions IsNot Nothing Then

            oServiceTransactions.DocumentReference = oImpTransactions.DocumentReference
            oServiceTransactions.DocumentType = oImpTransactions.DocumentType
            oServiceTransactions.EffectiveDate = oImpTransactions.EffectiveDate

            If oImpTransactions.Extended IsNot Nothing Then
                oServiceTransactions.Extended = oImpTransactions.Extended.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseGetPolicyTransactionDetailsResponseTypeTransactionsExtended, 
                                                                                                             BaseGetPolicyTransactionDetailsResponseTypeTransactionsExtended)(AddressOf ToServiceGetPolicyTransactionDetailsResponseTypeTransactionsExtended))

            End If

        End If

        Return oServiceTransactions

    End Function

    Private Function ToServiceGetPolicyTransactionDetailsResponseTypeTransactionsExtended(ByVal oImpTransactionsExtended As BaseImplementationTypes.BaseGetPolicyTransactionDetailsResponseTypeTransactionsExtended) As BaseGetPolicyTransactionDetailsResponseTypeTransactionsExtended

        Dim oServiceTransactionsExtended As New BaseGetPolicyTransactionDetailsResponseTypeTransactionsExtended

        If oImpTransactionsExtended IsNot Nothing Then

            oServiceTransactionsExtended.DueDate = oImpTransactionsExtended.DueDate

            oServiceTransactionsExtended.Premium = oImpTransactionsExtended.Premium
            oServiceTransactionsExtended.PremiumOutstanding = oImpTransactionsExtended.PremiumOutstanding
            oServiceTransactionsExtended.PremiumTransDetailId = oImpTransactionsExtended.PremiumTransDetailId
            oServiceTransactionsExtended.PremiumTransDetailExtendedId = oImpTransactionsExtended.PremiumTransDetailExtendedId
            oServiceTransactionsExtended.PremiumAllocationTimeStamp = oImpTransactionsExtended.PremiumAllocationTimeStamp

            oServiceTransactionsExtended.PremiumTax = oImpTransactionsExtended.PremiumTax
            oServiceTransactionsExtended.PremiumTaxOutstanding = oImpTransactionsExtended.PremiumTaxOutstanding
            oServiceTransactionsExtended.PremiumTaxTransDetailId = oImpTransactionsExtended.PremiumTaxTransDetailId
            oServiceTransactionsExtended.PremiumTaxTransDetailExtendedId = oImpTransactionsExtended.PremiumTaxTransDetailExtendedId
            oServiceTransactionsExtended.PremiumTaxAllocationTimeStamp = oImpTransactionsExtended.PremiumTaxAllocationTimeStamp

            oServiceTransactionsExtended.Commission = oImpTransactionsExtended.Commission
            oServiceTransactionsExtended.CommissionOutstanding = oImpTransactionsExtended.CommissionOutstanding
            oServiceTransactionsExtended.CommissionTransDetailId = oImpTransactionsExtended.CommissionTransDetailId
            oServiceTransactionsExtended.CommissionTransDetailExtendedId = oImpTransactionsExtended.CommissionTransDetailExtendedId
            oServiceTransactionsExtended.CommissionAllocationTimeStamp = oImpTransactionsExtended.CommissionAllocationTimeStamp

            oServiceTransactionsExtended.CommissionTax = oImpTransactionsExtended.CommissionTax
            oServiceTransactionsExtended.CommissionTaxOutstanding = oImpTransactionsExtended.CommissionTaxOutstanding
            oServiceTransactionsExtended.CommissionTaxTransDetailId = oImpTransactionsExtended.CommissionTaxTransDetailId
            oServiceTransactionsExtended.CommissionTaxTransDetailExtendedId = oImpTransactionsExtended.CommissionTaxTransDetailExtendedId
            oServiceTransactionsExtended.CommissionTaxAllocationTimeStamp = oImpTransactionsExtended.CommissionTaxAllocationTimeStamp

            If oImpTransactionsExtended.Fees IsNot Nothing Then

                oServiceTransactionsExtended.Fees = oImpTransactionsExtended.Fees.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseGetPolicyTransactionDetailsResponseTypeTransactionsExtendedFees, 
                                                                                                            BaseGetPolicyTransactionDetailsResponseTypeTransactionsExtendedFees)(AddressOf ToServiceGetPolicyTransactionDetailsResponseTypeTransactionsExtendedFees))

            End If

        End If

        Return oServiceTransactionsExtended

    End Function

    Private Function ToServiceGetPolicyTransactionDetailsResponseTypeTransactionsExtendedFees(ByVal oImpTransactionsExtendedFees As BaseImplementationTypes.BaseGetPolicyTransactionDetailsResponseTypeTransactionsExtendedFees) As BaseGetPolicyTransactionDetailsResponseTypeTransactionsExtendedFees

        Dim oServiceTransactionsExtendedFees As New BaseGetPolicyTransactionDetailsResponseTypeTransactionsExtendedFees

        If oImpTransactionsExtendedFees IsNot Nothing Then

            oServiceTransactionsExtendedFees.Amount = oImpTransactionsExtendedFees.Amount
            oServiceTransactionsExtendedFees.AmountOutstanding = oImpTransactionsExtendedFees.AmountOutstanding
            oServiceTransactionsExtendedFees.FeeTransDetailId = oImpTransactionsExtendedFees.FeeTransDetailId
            oServiceTransactionsExtendedFees.FeeTransDetailExtendedId = oImpTransactionsExtendedFees.FeeTransDetailExtendedId
            oServiceTransactionsExtendedFees.FeeTaxTransDetailId = oImpTransactionsExtendedFees.FeeTaxTransDetailId
            oServiceTransactionsExtendedFees.FeeType = oImpTransactionsExtendedFees.FeeType
            oServiceTransactionsExtendedFees.TaxAmount = oImpTransactionsExtendedFees.TaxAmount
            oServiceTransactionsExtendedFees.TaxAmountOutstanding = oImpTransactionsExtendedFees.TaxAmountOutstanding
            oServiceTransactionsExtendedFees.FeeTaxTransDetailExtendedId = oImpTransactionsExtendedFees.FeeTaxTransDetailExtendedId
            oServiceTransactionsExtendedFees.FeeAllocationTimeStamp = oImpTransactionsExtendedFees.FeeAllocationTimeStamp
            oServiceTransactionsExtendedFees.FeeTaxAllocationTimeStamp = oImpTransactionsExtendedFees.FeeTaxAllocationTimeStamp

        End If

        Return oServiceTransactionsExtendedFees

    End Function
#End Region

#Region "Reverse Allocation  Batch"
    ''' <summary>
    '''This function will reverse allocation Batch based on allocation key and transdetail key 
    '''</summary>
    Public Function ReverseAllocationBatch(ByVal oReverseAllocationBatchRequest As ReverseAllocationBatchRequestType) As ReverseAllocationBatchResponseType Implements IPureAccountService.ReverseAllocationBatch

        Try

            Dim sUserName As String = oReverseAllocationBatchRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMREVALOB", iUserId)
            CommonFunctions.CheckSecurityToken(oReverseAllocationBatchRequest.WCFSecurityToken)

            Dim oResponse As New ReverseAllocationBatchResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oReverseAllocationBatchRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.ReverseAllocationBatchRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.ReverseAllocationBatchResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oReverseAllocationBatchRequest.BranchCode
            oImpRequest.AllocationBatchKey = oReverseAllocationBatchRequest.AllocationBatchKey
            oImpRequest.IgnoreWarnings = oReverseAllocationBatchRequest.IgnoreWarnings

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.ReverseAllocationBatch(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            oResponse.Warnings = oImpResponse.Warnings

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oReverseAllocationBatchRequest))
            Return Nothing
        End Try

    End Function

#End Region

#Region "Search Transaction Selected Column"
    ''' <summary>
    ''' This function will Get the selected column data based on User Name.
    ''' </summary>
    ''' <param name="oGetUserPreferredColumnListRequestType"></param>
    ''' <returns></returns>
    Public Function GetUserPreferredColumnList(ByVal oGetUserPreferredColumnListRequestType As GetUserPreferredColumnListRequestType) As GetUserPreferredColumnListResponseType Implements IPureAccountService.GetUserPreferredColumnList
        Try
            Dim sUserName As String = oGetUserPreferredColumnListRequestType.LoginUserName
            Dim BranchCode As String = oGetUserPreferredColumnListRequestType.BranchCode
            Dim oResponse = New GetUserPreferredColumnListResponseType
            Dim oImpRequest As New BaseImplementationTypes.BaseGetUserPreferredColumnListRequestType
            Dim oImpResponse As New BaseImplementationTypes.BaseGetUserPreferredColumnListResponseType
            Dim nAgentKey As Integer
            Dim nUserId As Integer

            CommonFunctions.GetIdentity(sUserName, nAgentKey, nUserId)
            CommonFunctions.CheckAuthority("FINDTXN", nUserId)
            CommonFunctions.CheckSecurityToken(oGetUserPreferredColumnListRequestType.WCFSecurityToken)

            oImpRequest.BranchCode = oGetUserPreferredColumnListRequestType.BranchCode
            oImpRequest.LoginUserName = oGetUserPreferredColumnListRequestType.LoginUserName
            oImpRequest.InterfaceName = oGetUserPreferredColumnListRequestType.InterfaceName


            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, BranchCode)

            Try
                oImpResponse = oBusiness.GetUserPreferredColumnList(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            With oResponse
                .ColumnList = oImpResponse.ColumnList

            End With

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetUserPreferredColumnListRequestType))
            Return Nothing

        End Try

    End Function
#End Region

#Region "Update Search Transaction Selected Column"
    ''' <summary>
    ''' This function will Set or Update the selected column data based on User Name.
    ''' </summary>
    ''' <param name="oUpdateUserPreferredColumnListRequestType"></param>
    ''' <returns></returns>
    Public Function UpdateUserPreferredColumnList(ByRef oUpdateUserPreferredColumnListRequestType As UpdateUserPreferredColumnListRequestType) As UpdateUserPreferredColumnListResponseType Implements IPureAccountService.UpdateUserPreferredColumnList
        Try
            Dim sUserName As String = oUpdateUserPreferredColumnListRequestType.LoginUserName
            Dim BranchCode As String = oUpdateUserPreferredColumnListRequestType.BranchCode
            Dim oResponse As UpdateUserPreferredColumnListResponseType

            oResponse = New UpdateUserPreferredColumnListResponseType

            Dim oImpResponse As New BaseImplementationTypes.BaseUpdateUserPreferredColumnListResponseType
            Dim oImpRequest As New BaseImplementationTypes.BaseUpdateUserPreferredColumnListRequestType

            sUserName = oUpdateUserPreferredColumnListRequestType.LoginUserName
            Dim nAgentKey As Integer
            Dim nUserId As Integer

            CommonFunctions.GetIdentity(sUserName, nAgentKey, nUserId)
            CommonFunctions.CheckAuthority("FINDTXN", nUserId)
            CommonFunctions.CheckSecurityToken(oUpdateUserPreferredColumnListRequestType.WCFSecurityToken)

            With oUpdateUserPreferredColumnListRequestType
                oImpRequest.BranchCode = .BranchCode
                oImpRequest.LoginUserName = .LoginUserName
                oImpRequest.WCFSecurityToken = .WCFSecurityToken
                oImpRequest.ColumnList = .ColumnList
                oImpRequest.InterfaceName = .InterfaceName
                oImpRequest.UserName = .UserName

            End With

            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, BranchCode)
            Try
                oImpResponse = oBusiness.UpdateUserPreferredColumnList(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdateUserPreferredColumnListRequestType))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateUserPreferredColumnListRequestType))
            Return Nothing
        End Try

    End Function
#End Region

    ''' <summary>
    ''' Get list of unapproved payments.
    ''' </summary>
    ''' <param name="oGetListofUnapprovedPaymentRequestType"></param>
    ''' <returns></returns>
    Public Function GetListofUnapprovedPayment(ByVal oGetListofUnapprovedPaymentRequestType As GetListofUnapprovedPaymentRequestType) As GetListofUnapprovedPaymentResponseType Implements IPureAccountService.GetListofUnapprovedPayment
        Try
            Dim sUserName As String = oGetListofUnapprovedPaymentRequestType.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckSecurityToken(oGetListofUnapprovedPaymentRequestType.WCFSecurityToken)
            Dim oResponse As GetListofUnapprovedPaymentResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetListofUnapprovedPaymentRequestType.BranchCode)
            oResponse = New GetListofUnapprovedPaymentResponseType
            ' Implementation structures
            Dim oImpRequest As New BaseImplementationTypes.BaseGetListofUnapprovedPaymentRequestType
            Dim oImpResponse As New BaseImplementationTypes.BaseGetListofUnapprovedPaymentResponseType

            ' Pass the values to the implementation request structure
            With oGetListofUnapprovedPaymentRequestType
                oImpRequest.AssignedTo = .AssignedTo
                oImpRequest.CashListItemKey = .CashListItemKey
                oImpRequest.Branch = .Branch
                oImpRequest.CreatedBy = .CreatedBy
                oImpRequest.DateFrom = .DateFrom
                oImpRequest.DateTo = .DateTo
                oImpRequest.PayeeName = .PayeeName
                oImpRequest.ShowAllOtherPayments = .ShowAllOtherPayments
                oImpRequest.PaymentType = .PaymentType
            End With

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetListofUnapprovedPayment(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.Errors)
                'oResponse.CashListItems = SAMFunc.GetDeserializedValues(Of List(Of BaseGetReferredPaymentsResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetReferredPaymentsResponseTypeCashListItems", sConvertToTypeName:="BaseGetReferredPaymentsResponseTypeRow")
                If oImpResponse.ResultDataset IsNot Nothing AndAlso oImpResponse.ResultDataset.Tables(0) IsNot Nothing Then
                    oResponse.ListofUnapprovedPayment = DataTabletoList_ListofUnapprovedPayment(oImpResponse.ResultDataset.Tables(0))
                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetListofUnapprovedPaymentRequestType))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetListofUnapprovedPaymentRequestType))
            Return Nothing
        End Try
    End Function


    Public Function UpdateAuthorizationComment(ByRef oUpdateAuthorizationCommentRequestType As UpdateAuthorizationCommentRequestType) As UpdateAuthorizationCommentResponseType Implements IPureAccountService.UpdateAuthorizationComment
        Try
            Dim sUserName As String = oUpdateAuthorizationCommentRequestType.LoginUserName
            Dim BranchCode As String = oUpdateAuthorizationCommentRequestType.BranchCode
            Dim oResponse As UpdateAuthorizationCommentResponseType

            oResponse = New UpdateAuthorizationCommentResponseType

            Dim oImpResponse As New BaseImplementationTypes.BaseUpdateAuthorizationCommentResponseType
            Dim oImpRequest As New BaseImplementationTypes.BaseUpdateAuthorizationCommentRequestType

            oImpRequest.LoginUserName = oUpdateAuthorizationCommentRequestType.LoginUserName
            oImpRequest.WCFSecurityToken = oUpdateAuthorizationCommentRequestType.WCFSecurityToken
            oImpRequest.BranchCode = oUpdateAuthorizationCommentRequestType.BranchCode

            sUserName = oUpdateAuthorizationCommentRequestType.LoginUserName
            Dim nAgentKey As Integer
            Dim nUserId As Integer

            CommonFunctions.GetIdentity(sUserName, nAgentKey, nUserId)
            CommonFunctions.CheckAuthority("FINDTXN", nUserId)
            CommonFunctions.CheckSecurityToken(oUpdateAuthorizationCommentRequestType.WCFSecurityToken)

            With oUpdateAuthorizationCommentRequestType
                oImpRequest.CashListItem_id = .CashListItem_id
                oImpRequest.Comment = .Comment + " - " + DateTime.Now + " - " + .LoginUserName

            End With
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, BranchCode)

            Try
                oImpResponse = oBusiness.UpdateAuthorizationComment(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdateAuthorizationCommentRequestType))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateAuthorizationCommentRequestType))
            Return Nothing
        End Try
    End Function

    Public Function GetAuthorizationComment(ByRef oGetAuthorizationCommentRequestType As GetAuthorizationCommentRequestType) As GetAuthorizationCommentResponseType Implements IPureAccountService.GetAuthorizationComment
        Try
            Dim sUserName As String = oGetAuthorizationCommentRequestType.LoginUserName
            Dim BranchCode As String = oGetAuthorizationCommentRequestType.BranchCode
            Dim oResponse As GetAuthorizationCommentResponseType

            oResponse = New GetAuthorizationCommentResponseType

            Dim oImpResponse As New BaseImplementationTypes.BaseGetAuthorizationCommentResponseType
            Dim oImpRequest As New BaseImplementationTypes.BaseGetAuthorizationCommentRequestType

            oImpRequest.LoginUserName = oGetAuthorizationCommentRequestType.LoginUserName
            oImpRequest.WCFSecurityToken = oGetAuthorizationCommentRequestType.WCFSecurityToken
            oImpRequest.BranchCode = oGetAuthorizationCommentRequestType.BranchCode

            sUserName = oGetAuthorizationCommentRequestType.LoginUserName
            Dim nAgentKey As Integer
            Dim nUserId As Integer

            CommonFunctions.GetIdentity(sUserName, nAgentKey, nUserId)
            CommonFunctions.CheckAuthority("FINDTXN", nUserId)
            CommonFunctions.CheckSecurityToken(oGetAuthorizationCommentRequestType.WCFSecurityToken)

            With oGetAuthorizationCommentRequestType
                oImpRequest.CashListItem_id = .CashListItem_id

            End With
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, BranchCode)

            Try
                oImpResponse = oBusiness.GetAuthorizationComment(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetAuthorizationCommentRequestType))
            End Try

            With oResponse
                .Authorization_Comment = oImpResponse.Authorization_Comment
            End With

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetAuthorizationCommentRequestType))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' Get list of unapproved payments.
    ''' </summary>
    ''' <param name="oGetListofManualJournalTransactionsRequestType"></param>
    ''' <returns></returns>
    Public Function GetListofManualJournalTransactions(ByVal oGetListofManualJournalTransactionsRequestType As GetListofManualJournalTransactionsRequestType) As GetListofManualJournalTransactionsResponseType Implements IPureAccountService.GetListofManualJournalTransactions
        Try
            Dim sUserName As String = oGetListofManualJournalTransactionsRequestType.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckSecurityToken(oGetListofManualJournalTransactionsRequestType.WCFSecurityToken)
            Dim oResponse As GetListofManualJournalTransactionsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetListofManualJournalTransactionsRequestType.BranchCode)
            oResponse = New GetListofManualJournalTransactionsResponseType
            ' Implementation structures
            Dim oImpRequest As New BaseImplementationTypes.BaseGetListofManualJournalTransactionsRequestType
            Dim oImpResponse As New BaseImplementationTypes.BaseGetListofManualJournalTransactionsResponseType

            ' Pass the values to the implementation request structure
            With oGetListofManualJournalTransactionsRequestType
                oImpRequest.AccountCode = .AccountCode
                oImpRequest.JournalTypeCode = .JournalTypeCode
                oImpRequest.DateFrom = .DateFrom
                oImpRequest.DateTo = .DateTo

            End With

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetListofManualJournalTransactions(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.Errors)
                'oResponse.CashListItems = SAMFunc.GetDeserializedValues(Of List(Of BaseGetReferredPaymentsResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetReferredPaymentsResponseTypeCashListItems", sConvertToTypeName:="BaseGetReferredPaymentsResponseTypeRow")
                If oImpResponse.ResultDataset IsNot Nothing AndAlso oImpResponse.ResultDataset.Tables(0) IsNot Nothing Then
                    oResponse.ListofManualJournalTransactions = DataTabletoList_ListofManualJournalTransactions(oImpResponse.ResultDataset.Tables(0))
                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetListofManualJournalTransactionsRequestType))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetListofManualJournalTransactionsRequestType))
            Return Nothing
        End Try
    End Function


    Public Function UpdateManualJournalApproversComment(ByRef oUpdateManualJournalApproversCommentRequestType As UpdateManualJournalApproversCommentRequestType) As UpdateManualJournalApproversCommentResponseType Implements IPureAccountService.UpdateManualJournalApproversComment
        Try
            Dim sUserName As String = oUpdateManualJournalApproversCommentRequestType.LoginUserName
            Dim BranchCode As String = oUpdateManualJournalApproversCommentRequestType.BranchCode
            Dim oResponse As UpdateManualJournalApproversCommentResponseType

            oResponse = New UpdateManualJournalApproversCommentResponseType

            Dim oImpResponse As New BaseImplementationTypes.BaseUpdateManualJournalApproversCommentResponseType
            Dim oImpRequest As New BaseImplementationTypes.BaseUpdateManualJournalApproversCommentRequestType

            oImpRequest.LoginUserName = oUpdateManualJournalApproversCommentRequestType.LoginUserName
            oImpRequest.WCFSecurityToken = oUpdateManualJournalApproversCommentRequestType.WCFSecurityToken
            oImpRequest.BranchCode = oUpdateManualJournalApproversCommentRequestType.BranchCode

            sUserName = oUpdateManualJournalApproversCommentRequestType.LoginUserName
            Dim nAgentKey As Integer
            Dim nUserId As Integer

            CommonFunctions.GetIdentity(sUserName, nAgentKey, nUserId)
            '  CommonFunctions.CheckAuthority("FINDTXN", nUserId)
            CommonFunctions.CheckSecurityToken(oUpdateManualJournalApproversCommentRequestType.WCFSecurityToken)

            With oUpdateManualJournalApproversCommentRequestType
                oImpRequest.ManualJournalId = .ManualJournalId
                oImpRequest.Comment = .Comment + " - " + DateTime.Now + " - " + .LoginUserName

            End With
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, BranchCode)

            Try
                oImpResponse = oBusiness.UpdateManualJournalApproversComment(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdateManualJournalApproversCommentRequestType))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateManualJournalApproversCommentRequestType))
            Return Nothing
        End Try
    End Function


    ''' <summary>
    ''' Get list to fill filter controls
    ''' </summary>
    ''' <param name="oGetListOfManualJournalTransactionMasterRequestType"></param>
    ''' <returns></returns>
    Public Function GetListOfManualJournalTransactionMaster(ByVal oGetListOfManualJournalTransactionMasterRequestType As GetListOfManualJournalTransactionMasterRequestType) As GetListOfManualJournalTransactionMasterResponseType Implements IPureAccountService.GetListOfManualJournalTransactionMaster
        Try
            Dim sUserName As String = oGetListOfManualJournalTransactionMasterRequestType.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckSecurityToken(oGetListOfManualJournalTransactionMasterRequestType.WCFSecurityToken)
            Dim oResponse As GetListOfManualJournalTransactionMasterResponseType
            Dim oBusiness As New CoreSAMBusiness()
            oResponse = New GetListOfManualJournalTransactionMasterResponseType
            ' Implementation structures
            Dim oImpRequest As New BaseImplementationTypes.BaseGetListOfManualJournalTransactionMasterRequestType
            Dim oImpResponse As New BaseImplementationTypes.BaseGetListOfManualJournalTransactionMasterResponseType

            oImpRequest.LoginUserName = oGetListOfManualJournalTransactionMasterRequestType.LoginUserName
            oImpRequest.WCFSecurityToken = oGetListOfManualJournalTransactionMasterRequestType.WCFSecurityToken
            oImpRequest.BranchCode = oGetListOfManualJournalTransactionMasterRequestType.BranchCode
            ' Pass the values to the implementation request structure
            With oGetListOfManualJournalTransactionMasterRequestType
                oImpRequest.ManualJournalId = .ManualJournalId

            End With

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetListofManualJournalTransactionMaster(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.Errors)
                'oResponse.CashListItems = SAMFunc.GetDeserializedValues(Of List(Of BaseGetReferredPaymentsResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetReferredPaymentsResponseTypeCashListItems", sConvertToTypeName:="BaseGetReferredPaymentsResponseTypeRow")
                If oImpResponse.ResultMasterDataSet IsNot Nothing AndAlso oImpResponse.ResultMasterDataSet.Tables(0) IsNot Nothing Then
                    oResponse.ListofManualJournalTransactionMaster = DataTabletoList_ListofManualJournalTransactionsMaster(oImpResponse.ResultMasterDataSet.Tables(0))
                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetListOfManualJournalTransactionMasterRequestType))
            End Try
            'With oResponse
            '    .
            'End With
            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetListOfManualJournalTransactionMasterRequestType))
            Return Nothing
        End Try
    End Function


    ''' <summary>
    ''' Get list of to fill detailed Transaction
    ''' </summary>
    ''' <param name="oGetListOfManualJournalTransactionDetailsRequestType"></param>
    ''' <returns></returns>
    Public Function GetListOfManualJournalTransactionDetails(ByVal oGetListOfManualJournalTransactionDetailsRequestType As GetListOfManualJournalTransactionDetailsRequestType) As GetListOfManualJournalTransactionDetailsResponseType Implements IPureAccountService.GetListOfManualJournalTransactionDetails
        Try
            Dim sUserName As String = oGetListOfManualJournalTransactionDetailsRequestType.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckSecurityToken(oGetListOfManualJournalTransactionDetailsRequestType.WCFSecurityToken)
            Dim oResponse As GetListOfManualJournalTransactionDetailsResponseType
            Dim oBusiness As New CoreSAMBusiness(sUserName, oGetListOfManualJournalTransactionDetailsRequestType.BranchCode)
            oResponse = New GetListOfManualJournalTransactionDetailsResponseType
            ' Implementation structures
            Dim oImpRequest As New BaseImplementationTypes.BaseGetListOfManualJournalTransactionDetailsRequestType
            Dim oImpResponse As New BaseImplementationTypes.BaseGetListOfManualJournalTransactionDetailsResponseType

            oImpRequest.LoginUserName = oGetListOfManualJournalTransactionDetailsRequestType.LoginUserName
            oImpRequest.WCFSecurityToken = oGetListOfManualJournalTransactionDetailsRequestType.WCFSecurityToken
            oImpRequest.BranchCode = oGetListOfManualJournalTransactionDetailsRequestType.BranchCode

            ' Pass the values to the implementation request structure
            With oGetListOfManualJournalTransactionDetailsRequestType
                oImpRequest.ManualJournalId = .ManualJournalId

            End With

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetListofManualJournalTransactionDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.Errors)
                'oResponse.CashListItems = SAMFunc.GetDeserializedValues(Of List(Of BaseGetReferredPaymentsResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetReferredPaymentsResponseTypeCashListItems", sConvertToTypeName:="BaseGetReferredPaymentsResponseTypeRow")
                If oImpResponse.ResultDetailDataSet IsNot Nothing AndAlso oImpResponse.ResultDetailDataSet.Tables(0) IsNot Nothing Then
                    oResponse.ListofManualJournalTransactionDetails = DataTabletoList_ListofManualJournalTransactionsDetails(oImpResponse.ResultDetailDataSet.Tables(0))
                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetListOfManualJournalTransactionDetailsRequestType))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetListOfManualJournalTransactionDetailsRequestType))
            Return Nothing
        End Try
    End Function


    Public Function ValidateAuthorizationSteps(ByRef oValidateAuthorizationStepsRequestType As ValidateAuthorizationStepsRequestType) As ValidateAuthorizationStepsResponseType Implements IPureAccountService.ValidateAuthorizationSteps
        Try
            Dim sUserName As String = oValidateAuthorizationStepsRequestType.LoginUserName
            Dim BranchCode As String = oValidateAuthorizationStepsRequestType.BranchCode
            Dim oResponse As ValidateAuthorizationStepsResponseType

            oResponse = New ValidateAuthorizationStepsResponseType

            Dim oImpResponse As New BaseImplementationTypes.BaseValidateAuthorizationStepsResponseType
            Dim oImpRequest As New BaseImplementationTypes.BaseValidateAuthorizationStepsRequestType

            oImpRequest.LoginUserName = oValidateAuthorizationStepsRequestType.LoginUserName
            oImpRequest.WCFSecurityToken = oValidateAuthorizationStepsRequestType.WCFSecurityToken
            oImpRequest.BranchCode = oValidateAuthorizationStepsRequestType.BranchCode

            sUserName = oValidateAuthorizationStepsRequestType.LoginUserName
            Dim nAgentKey As Integer
            Dim nUserId As Integer

            CommonFunctions.GetIdentity(sUserName, nAgentKey, nUserId)
            CommonFunctions.CheckAuthority("SAMADJNL", nUserId)
            CommonFunctions.CheckSecurityToken(oValidateAuthorizationStepsRequestType.WCFSecurityToken)

            With oValidateAuthorizationStepsRequestType
                oImpRequest.ManualJournalId = .ManualJournalId
                oImpRequest.IsApproved = .IsApproved

            End With
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, BranchCode)

            Try
                oImpResponse = DirectCast(oBusiness.ValidateAuthorizationSteps(oImpRequest), Internal.BaseValidateAuthorizationStepsResponseType)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.ValidateAuthorizationSteps = DataTabletoList_ValidateAuthorizationSteps(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oImpRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oValidateAuthorizationStepsRequestType))
            Return Nothing
        End Try

    End Function
End Class
