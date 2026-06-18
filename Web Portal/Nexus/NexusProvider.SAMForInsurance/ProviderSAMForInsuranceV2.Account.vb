Imports System.Text
Imports System.Web.HttpContext
Imports System.Web.UI
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports SSP.PureInsuranceRestAPIHandler
Imports SSP.PureInsuranceRestAPIHandler.BaseClasses
Imports SSP.PureInsuranceRestAPIHandler.Enums
Partial Public Class ProviderSAMForInsuranceV2

    ''' <summary>
    ''' This method is used in the pay now functionality.
    ''' This is an old method for adding receipt for Agent attached with current user.
    ''' </summary>
    ''' <param name="v_PartyKey"></param>
    ''' <param name="r_oReceiptType"></param>
    ''' <param name="v_sBranchCode"></param>
    Public Overrides Sub AddAgentReceipt(ByVal v_PartyKey As Integer,
                                                ByRef r_oReceiptType As ReceiptType,
                                                Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock

            Dim oAddReceiptRequest As AddAgentReceiptCommand
            Dim oAddReceiptResponse As AddAgentReceiptCommandResponse
            Dim sbLogMessage As StringBuilder

            Try
                oAddReceiptRequest = New AddAgentReceiptCommand
                oAddReceiptResponse = New AddAgentReceiptCommandResponse
                sbLogMessage = New StringBuilder

                ''To add response type

                With oAddReceiptRequest
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    'if the passed parameter v_sBranchCode is empty
                    If v_PartyKey > 0 Then
                        .PartyKey = v_PartyKey
                    Else
                        Throw New ArgumentNullException("PartyKey")
                    End If

                    If r_oReceiptType.TransactionDate = DateTime.MinValue Then
                        Throw New ArgumentNullException("Quote.Risk.CoverStartDate")
                    Else
                        .Receipt.TransactionDate = r_oReceiptType.TransactionDate
                    End If

                    If r_oReceiptType.Amount > 0.0 Then
                        .Receipt.Amount = r_oReceiptType.Amount
                    Else
                        Throw New ArgumentNullException("ReceiptType.Amount")
                    End If
                    .Receipt.Address1 = r_oReceiptType.Address1
                    .Receipt.Address2 = r_oReceiptType.Address2
                    .Receipt.Address3 = r_oReceiptType.Address3
                    .Receipt.Address4 = r_oReceiptType.Address4
                    .Receipt.BankAccountName = r_oReceiptType.BankAccountName
                    .Receipt.CashListRef = r_oReceiptType.CashListRef
                    .Receipt.CCAuthCode = r_oReceiptType.CCAuthCode
                    .Receipt.CCCustomer = r_oReceiptType.CCCustomer
                    .Receipt.CCExpiryDate = r_oReceiptType.CCExpiryDate
                    .Receipt.CCIssue = r_oReceiptType.CCIssue
                    .Receipt.CCManualAuthCode = r_oReceiptType.CCManualAuthCode
                    .Receipt.CCName = r_oReceiptType.CCName
                    .Receipt.CCNumber = r_oReceiptType.CCNumber
                    .Receipt.CCPin = r_oReceiptType.CCPin
                    .Receipt.CCStartDate = r_oReceiptType.CCStartDate
                    .Receipt.CCTransactionCode = r_oReceiptType.CCTransactionCode

                    If r_oReceiptType.ChequeDate <> Date.MinValue Then
                        .Receipt.ChequeDateSpecified = True
                        .Receipt.ChequeDate = r_oReceiptType.ChequeDate
                    Else
                        .Receipt.ChequeDateSpecified = False
                    End If

                    .Receipt.ChequeName = r_oReceiptType.ChequeName

                    If r_oReceiptType.CollectionDate <> Date.MinValue Then
                        .Receipt.CollectionDateSpecified = True
                        .Receipt.CollectionDate = r_oReceiptType.CollectionDate
                    Else
                        .Receipt.ChequeDateSpecified = False
                    End If

                    .Receipt.Comments = r_oReceiptType.Comments
                    .Receipt.ContactName = r_oReceiptType.ContactName
                    .Receipt.CountryCode = r_oReceiptType.CountryCode
                    .Receipt.CurrencyCode = r_oReceiptType.CurrencyCode
                    .Receipt.MediaReference = r_oReceiptType.MediaReference
                    .Receipt.MediaTypeCode = r_oReceiptType.MediaTypeCode
                    .Receipt.MediaTypeIssuerCode = r_oReceiptType.MediaTypeIssuerCode
                    .Receipt.OurReference = r_oReceiptType.OurReference
                    .Receipt.ReceiptTypeCode = r_oReceiptType.ReceiptTypeCode
                    .Receipt.PostalCode = r_oReceiptType.PostalCode
                    .Receipt.TheirReference = r_oReceiptType.TheirReference

                End With
                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Post(ApiMethods.AddAgentReceipt, oAddReceiptRequest)
                    oAddReceiptResponse = ApiClient.DeserializeJson(Of AddAgentReceiptCommandResponse)(result)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("AddReceipt executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    '  sbLogMessage.AppendLine("r_oReceiptType = " & r_oReceiptType.Print("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

            Catch ex As Exception
                Throw
            Finally
                oAddReceiptRequest = Nothing
                oAddReceiptResponse = Nothing
            End Try


        End SyncLock

    End Sub

    Public Overrides Sub AddJournal(ByVal oManualJournal As NexusProvider.ManualJournal,
                                                  Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock

            Dim oAddJournalRequestType As BaseAddJournalRequestType
            Dim oAddJournalResponseType As AddJournalCommandResponse
            Dim oReversalDetails As BaseAddJournalReversalDetails
            Dim oRecurringDetails As BaseAddJournalRecurringDetails
            Dim oTransaction As BaseAddJournalTransaction
            Try
                oAddJournalRequestType = New BaseAddJournalRequestType
                oAddJournalResponseType = New AddJournalCommandResponse
                oReversalDetails = New BaseAddJournalReversalDetails
                oRecurringDetails = New BaseAddJournalRecurringDetails


                With oAddJournalRequestType
                    'if the passed parameter v_sBranchCode is empty 
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    .JournalBranchCode = oManualJournal.JournalBranchCode
                    .Comment = oManualJournal.JournalComment
                    If Not oManualJournal.JournalDateSpecified Then
                        .JournalDateSpecified = False
                    Else
                        .JournalDate = oManualJournal.JournalDate
                        .JournalDateSpecified = True
                    End If

                    .is_reffered = oManualJournal.Is_Reffered

                    'if document type is Accrual


                    .JournalTypeCode = oManualJournal.JournalTypeCode
                    .JournalSubBranchCode = oManualJournal.JournalSubBranchCode
                    If oManualJournal.JournalTypeCode = "ACC" Or oManualJournal.JournalTypeCode = "RVJ" Or oManualJournal.JournalTypeCode = "PPT" Then

                        oReversalDetails.ReversesOn = oManualJournal.JournalReverseDate
                        .ReversalDetails = oReversalDetails
                    End If

                    If oManualJournal.JournalTypeCode = "RCJ" Or oManualJournal.JournalTypeCode = "DPJ" Then

                        'oReversalDetails.ReversesOn = oManualJournal.JournalReverseDate
                        '.ReversalDetails = oReversalDetails


                        If oManualJournal.RecurringPerPeriodOnDayBool Then
                            oRecurringDetails.PerPeriodOnDay = oManualJournal.RecurringPerPeriodOnDay
                            oRecurringDetails.PerPeriodOnDaySpecified = True
                        ElseIf oManualJournal.RecurringPerMonthOnDayBool Then
                            oRecurringDetails.PerPeriodOnMonth = oManualJournal.RecurringPerMonthOnDay
                            oRecurringDetails.PerPeriodOnMonthSpecified = True
                        ElseIf oManualJournal.RecurringPerQuarterOnDayBool Then
                            oRecurringDetails.PerQuarterOnDay = oManualJournal.RecurringPerQuarterOnDay
                            oRecurringDetails.PerQuarterOnDaySpecified = True
                        End If
                        oRecurringDetails.Occurs = oManualJournal.RecurringOccurs

                        .RecurringDetails = oRecurringDetails

                    End If

                    .Transactions = New List(Of BaseAddJournalTransaction)
                    For iCount As Integer = 0 To oManualJournal.ManualJournalItemCollection.Count - 1
                        oTransaction = New BaseAddJournalTransaction
                        oTransaction.AccountCode = oManualJournal.ManualJournalItemCollection(iCount).AccountKey
                        oTransaction.AltReference = oManualJournal.ManualJournalItemCollection(iCount).AltReference
                        oTransaction.Amount = oManualJournal.ManualJournalItemCollection(iCount).Amount
                        oTransaction.Comment = oManualJournal.ManualJournalItemCollection(iCount).Comment
                        oTransaction.CostCentreCode = oManualJournal.ManualJournalItemCollection(iCount).CostCentreCode
                        oTransaction.CurrencyCode = oManualJournal.ManualJournalItemCollection(iCount).CurrencyTypeCode
                        oTransaction.InsuranceRef = oManualJournal.ManualJournalItemCollection(iCount).InsuranceRef
                        oTransaction.PurchaseInvoiceNumber = oManualJournal.ManualJournalItemCollection(iCount).PurchaseInvoiceNumber
                        oTransaction.PurchaseOrderNumber = oManualJournal.ManualJournalItemCollection(iCount).PurchaseOrderNumber
                        oTransaction.UnderwritingYear = oManualJournal.ManualJournalItemCollection(iCount).UnderwritingYearCode
                        oTransaction.ManualJournalDetailId = oManualJournal.ManualJournalItemCollection(iCount).ManualJournalDetailId
                        .Transactions.Add(oTransaction)
                    Next
                    .ManualJournalId = oManualJournal.ManualJournalId
                    .isApproved = oManualJournal.Approved

                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Post(ApiMethods.AddJournal, oAddJournalRequestType)
                    oAddJournalResponseType = ApiClient.DeserializeJson(Of AddJournalCommandResponse)(result)
                End Using

                With oAddJournalResponseType

                    If (.DocumentRef IsNot Nothing AndAlso .DocumentRef.Length > 0) Then
                        If Not String.IsNullOrEmpty(.DocumentRef(0)) Then
                            oManualJournal.ManualJournalDocumentRef = .DocumentRef(0).ToString()
                        End If
                    End If
                    oManualJournal.ManualJournalId = .ManualJournalId

                End With
                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oAddJournalResponseType = Nothing
                oAddJournalRequestType = Nothing

            End Try

        End SyncLock
    End Sub

    Public Overrides Function AddWriteoff(ByVal v_oWriteoff As Writeoff,
                                        ByVal v_iTransactionKey As Integer,
                                        Optional ByVal v_sBranchCode As String = Nothing) As Integer
        SyncLock oLock
            Dim oAddWriteOffRequest As BaseAddWriteOffRequestType
            Dim oAddWriteOffResponse As BaseAddWriteOffResponseType
            Dim sbLogMessage As StringBuilder

            Try
                oAddWriteOffRequest = New BaseAddWriteOffRequestType
                oAddWriteOffResponse = New BaseAddWriteOffResponseType
                sbLogMessage = New StringBuilder

                With oAddWriteOffRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                        If v_oWriteoff.AccountKey > 0 Then
                            .AccountKey = v_oWriteoff.AccountKey
                        Else
                            Throw New ArgumentNullException("AddWriteOff.AccountKey")
                        End If

                        If v_oWriteoff.DocumentKey > 0 Then
                            .DocumentKey = v_oWriteoff.DocumentKey
                        Else
                            Throw New ArgumentNullException("AddWriteOff.DocumentKey")
                        End If

                        If v_oWriteoff.WriteOffAmount <> 0 Then
                            .WriteOffAmount = v_oWriteoff.WriteOffAmount
                        Else
                            Throw New ArgumentNullException("AddWriteOff.WriteOffAmount")
                        End If


                    End If
                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Post(ApiMethods.AddWriteOff, oAddWriteOffRequest)
                    oAddWriteOffResponse = ApiClient.DeserializeJson(Of AddWriteOffCommandResponse)(result)
                End Using

                With oAddWriteOffResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        v_iTransactionKey = .TransactionKey
                    End If

                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("AddWriteoff executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_oWriteoff = " & v_oWriteoff.Print.Replace("<br/>", vbCrLf))
                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If
                Return 1
            Catch ex As Exception
                Throw
            Finally
                oAddWriteOffRequest = Nothing
                oAddWriteOffResponse = Nothing
            End Try
        End SyncLock
    End Function

    ''' <summary>
    ''' This method is used to add cash list with items for payment.
    ''' </summary>
    ''' <param name="v_oPaymentCashListItem"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub CreatePaymentCashListWithItems(ByRef v_oPaymentCashListItem As PaymentCashListItemType,
                                                                 Optional ByVal v_sBranchCode As String = Nothing,
                                                                 Optional ByVal v_alTransDetailCollection As ArrayList = Nothing)

        SyncLock oLock

            Dim oCreatePaymentCashListWithItemsRequest As CreatePaymentCashListWithItemsCommand 'Request Type
            Dim oCreatePaymentCashListWithItemsResponse As CreatePaymentCashListWithItemsCommandResponse 'Response Type
            Dim oPaymentItem As BaseClasses.BasePaymentCashListItemType
            Dim oPayment As PaymentCashList
            Dim oPaymentCashListCollection As PaymentCashListCollection
            Dim sbLogMessage As StringBuilder

            Try
                oCreatePaymentCashListWithItemsRequest = New CreatePaymentCashListWithItemsCommand
                oCreatePaymentCashListWithItemsResponse = New CreatePaymentCashListWithItemsCommandResponse
                oPaymentCashListCollection = New PaymentCashListCollection
                sbLogMessage = New StringBuilder


                With oCreatePaymentCashListWithItemsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    .PaymentCashList = New BaseClasses.BasePaymentCashListType()
                    .PaymentCashList.TypeCode = v_oPaymentCashListItem.CoreCashList.TypeCode
                    .PaymentCashList.StatusCode = v_oPaymentCashListItem.CoreCashList.StatusCode
                    .PaymentCashList.ListDate = v_oPaymentCashListItem.CoreCashList.ListDate
                    .PaymentCashList.BankAccountCode = v_oPaymentCashListItem.CoreCashList.BankAccountCode
                    .PaymentCashList.CurrencyCode = v_oPaymentCashListItem.CoreCashList.CurrencyCode
                    .PaymentCashList.Reference = ""
                    .PaymentCashList.BankAccountName = v_oPaymentCashListItem.CoreCashList.BankAccountName
                    .PaymentCashList.BankAccountKey = v_oPaymentCashListItem.CoreCashList.BankAccountKey
                    .PaymentCashList.CashListKey = v_oPaymentCashListItem.CoreCashList.CashListKey
                    .PaymentCashList.SubBranchCode = v_oPaymentCashListItem.CoreCashList.SubBranchCode
                    .PaymentCashList.PaymentItem = New List(Of BaseClasses.BasePaymentCashListItemType)

                    For iItems As Integer = 0 To v_oPaymentCashListItem.PaymentItems.Count - 1
                        oPaymentItem = New BaseClasses.BasePaymentCashListItemType()
                        oPaymentItem.CashListItemKey = v_oPaymentCashListItem.PaymentItems(iItems).CashListItemKey
                        oPaymentItem.AccountShortCode = v_oPaymentCashListItem.PaymentItems(iItems).AccountShortCode
                        oPaymentItem.AllocationStatusCode = v_oPaymentCashListItem.PaymentItems(iItems).AllocationStatusCode
                        oPaymentItem.Amount = v_oPaymentCashListItem.PaymentItems(iItems).Amount
                        oPaymentItem.SkipPosting = v_oPaymentCashListItem.PaymentItems(iItems).SkipPosting
                        oPaymentItem.Amount_Tendered = v_oPaymentCashListItem.PaymentItems(iItems).Amount_tendered
                        oPaymentItem.Original_Amount = v_oPaymentCashListItem.PaymentItems(iItems).Original_amount
                        oPaymentItem.AccountBaseDate = v_oPaymentCashListItem.PaymentItems(iItems).AccountBaseDate
                        oPaymentItem.AccountBaseXrate = v_oPaymentCashListItem.PaymentItems(iItems).AccountBaseXrate
                        oPaymentItem.CurrencyBaseDate = v_oPaymentCashListItem.PaymentItems(iItems).CurrencyBaseDate
                        oPaymentItem.CurrencyBaseXrate = v_oPaymentCashListItem.PaymentItems(iItems).CurrencyBaseXrate
                        oPaymentItem.SystemBaseDate = v_oPaymentCashListItem.PaymentItems(iItems).SystemBaseDate
                        oPaymentItem.SystemBaseXrate = v_oPaymentCashListItem.PaymentItems(iItems).SystemBaseXrate
                        oPaymentItem.OverrideReason = v_oPaymentCashListItem.PaymentItems(iItems).OverrideReason
                        oPaymentItem.ContactAddress = New BaseClasses.BaseSimpleAddressType
                        oPaymentItem.ContactAddress.AddressLine1 = v_oPaymentCashListItem.PaymentItems(iItems).Address.Address1
                        oPaymentItem.ContactAddress.AddressLine2 = v_oPaymentCashListItem.PaymentItems(iItems).Address.Address2
                        oPaymentItem.ContactAddress.AddressLine3 = v_oPaymentCashListItem.PaymentItems(iItems).Address.Address3
                        oPaymentItem.ContactAddress.AddressLine4 = v_oPaymentCashListItem.PaymentItems(iItems).Address.Address4
                        oPaymentItem.ContactAddress.PostCode = v_oPaymentCashListItem.PaymentItems(iItems).Address.PostCode
                        oPaymentItem.ContactAddress.CountryCode = v_oPaymentCashListItem.PaymentItems(iItems).Address.CountryCode

                        If oPaymentItem.AllocationDetails Is Nothing And v_oPaymentCashListItem.AutoAllocateIfAble Then
                            oPaymentItem.AllocationDetails = New BaseClasses.BaseAllocationType()
                            oPaymentItem.AllocationDetails.AutoAllocate = v_oPaymentCashListItem.AutoAllocateIfAble
                        End If
                        If v_oPaymentCashListItem.PaymentItems(iItems).Bank IsNot Nothing Then
                            If v_oPaymentCashListItem.PaymentItems(iItems).Bank.AccountCode IsNot Nothing Then
                                If v_oPaymentCashListItem.PaymentItems(iItems).Bank.AccountCode.Trim.Length <> 0 Then
                                    oPaymentItem.Bank = New BaseClasses.BaseBankPaymentType
                                    oPaymentItem.Bank.AccountCode = v_oPaymentCashListItem.PaymentItems(iItems).Bank.AccountCode
                                    oPaymentItem.Bank.BranchCode = v_oPaymentCashListItem.PaymentItems(iItems).Bank.BranchCode

                                    If v_oPaymentCashListItem.PaymentItems(iItems).Bank.ExpiryDate <> Date.MinValue Then
                                        oPaymentItem.Bank.ExpiryDate = v_oPaymentCashListItem.PaymentItems(iItems).Bank.ExpiryDate
                                        oPaymentItem.Bank.ExpiryDateSpecified = True
                                    Else
                                        oPaymentItem.Bank.ExpiryDateSpecified = False
                                    End If

                                    oPaymentItem.Bank.PayeeName = v_oPaymentCashListItem.PaymentItems(iItems).Bank.PayeeName
                                    oPaymentItem.Bank.Reference1 = v_oPaymentCashListItem.PaymentItems(iItems).Bank.Reference1
                                    oPaymentItem.Bank.Reference2 = v_oPaymentCashListItem.PaymentItems(iItems).Bank.Reference2
                                    oPaymentItem.BankReference = v_oPaymentCashListItem.PaymentItems(iItems).BankReference
                                    oPaymentItem.Bank.BIC = v_oPaymentCashListItem.PaymentItems(iItems).Bank.BIC
                                    oPaymentItem.Bank.IBAN = v_oPaymentCashListItem.PaymentItems(iItems).Bank.IBAN
                                    oPaymentItem.Bank.PartyBankKey = v_oPaymentCashListItem.PaymentItems(iItems).Bank.PartyBankKey
                                End If
                            End If
                        End If
                        If v_oPaymentCashListItem.PaymentItems(iItems).CreditCard.Number IsNot Nothing Then
                            oPaymentItem.CreditCard = New BaseClasses.BaseCreditCardType
                            oPaymentItem.CreditCard.AuthCode = v_oPaymentCashListItem.PaymentItems(iItems).CreditCard.AuthCode
                            oPaymentItem.CreditCard.CardHolder = New BaseClasses.BaseCreditCardTypeCardHolder
                            oPaymentItem.CreditCard.CardHolder.AddressLine1 = v_oPaymentCashListItem.PaymentItems(iItems).CreditCard.CardHolder.Address1
                            oPaymentItem.CreditCard.CardHolder.AddressLine2 = v_oPaymentCashListItem.PaymentItems(iItems).CreditCard.CardHolder.Address2
                            oPaymentItem.CreditCard.CardHolder.AddressLine3 = v_oPaymentCashListItem.PaymentItems(iItems).CreditCard.CardHolder.Address3
                            oPaymentItem.CreditCard.CardHolder.AddressLine4 = v_oPaymentCashListItem.PaymentItems(iItems).CreditCard.CardHolder.Address4
                            oPaymentItem.CreditCard.CardHolder.CountryCode = v_oPaymentCashListItem.PaymentItems(iItems).CreditCard.CardHolder.CountryCode
                            oPaymentItem.CreditCard.CardHolder.Name = v_oPaymentCashListItem.PaymentItems(iItems).CreditCard.CardHolder.Name
                            oPaymentItem.CreditCard.CardHolder.PostCode = v_oPaymentCashListItem.PaymentItems(iItems).CreditCard.CardHolder.PostCode
                            oPaymentItem.CreditCard.CustomerPresent = v_oPaymentCashListItem.PaymentItems(iItems).CreditCard.CustomerPresent
                            oPaymentItem.CreditCard.ExpiryDate = v_oPaymentCashListItem.PaymentItems(iItems).CreditCard.ExpiryDate
                            oPaymentItem.CreditCard.Issue = v_oPaymentCashListItem.PaymentItems(iItems).CreditCard.Issue
                            oPaymentItem.CreditCard.ManualAuthCode = v_oPaymentCashListItem.PaymentItems(iItems).CreditCard.ManualAuthCode
                            oPaymentItem.CreditCard.NameOnCreditCard = v_oPaymentCashListItem.PaymentItems(iItems).CreditCard.NameOnCreditCard
                            oPaymentItem.CreditCard.Number = v_oPaymentCashListItem.PaymentItems(iItems).CreditCard.Number
                            oPaymentItem.CreditCard.Pin = v_oPaymentCashListItem.PaymentItems(iItems).CreditCard.Pin
                            oPaymentItem.CreditCard.StartDate = v_oPaymentCashListItem.PaymentItems(iItems).CreditCard.StartDate
                            oPaymentItem.CreditCard.TransactionCode = v_oPaymentCashListItem.PaymentItems(iItems).CreditCard.TransactionCode
                            oPaymentItem.CreditCard.TypeCode = v_oPaymentCashListItem.PaymentItems(iItems).CreditCard.TypeCode
                            oPaymentItem.CreditCard.CashListItemBankCode = v_oPaymentCashListItem.PaymentItems(iItems).CreditCard.CCIssueBank
                        End If

                        oPaymentItem.ContactName = v_oPaymentCashListItem.PaymentItems(iItems).ContactName
                        oPaymentItem.FurtherDetails = v_oPaymentCashListItem.PaymentItems(iItems).FurtherDetails
                        oPaymentItem.IsProduceDocument = v_oPaymentCashListItem.PaymentItems(iItems).IsProduceDocument
                        oPaymentItem.MediaReference = v_oPaymentCashListItem.PaymentItems(iItems).MediaReference
                        oPaymentItem.MediaTypeCode = v_oPaymentCashListItem.PaymentItems(iItems).MediaTypeCode
                        oPaymentItem.OurReference = v_oPaymentCashListItem.PaymentItems(iItems).OurReference
                        oPaymentItem.StatusCode = v_oPaymentCashListItem.PaymentItems(iItems).StatusCode
                        oPaymentItem.TheirReference = v_oPaymentCashListItem.PaymentItems(iItems).TheirReference
                        oPaymentItem.TransactionDate = v_oPaymentCashListItem.PaymentItems(iItems).TransactionDate
                        oPaymentItem.Collection_Date = v_oPaymentCashListItem.PaymentItems(iItems).Collection_Date
                        oPaymentItem.TypeCode = v_oPaymentCashListItem.PaymentItems(iItems).TypeCode
                        If v_alTransDetailCollection IsNot Nothing AndAlso v_alTransDetailCollection.Count > 0 Then
                            oPaymentItem.MarkedTransKeys = New List(Of Integer)
                            For Each oPair As Pair In v_alTransDetailCollection
                                oPaymentItem.MarkedTransKeys.Add(oPair.First)
                            Next
                        End If
                        ' oPaymentItem.IsViaBulkClaimPayment = v_oPaymentCashListItem.PaymentItems(iItems).IsViaBulkClaimPayment sandeep
                        .PaymentCashList.PaymentItem.Add(oPaymentItem)
                    Next

                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Post(ApiMethods.CreatePaymentCashListWithItems, oCreatePaymentCashListWithItemsRequest)
                    oCreatePaymentCashListWithItemsResponse = ApiClient.DeserializeJson(Of CreatePaymentCashListWithItemsCommandResponse)(result)
                End Using

                ' Disposing the SAM's object
                With oCreatePaymentCashListWithItemsResponse
                    If .Errors IsNot Nothing AndAlso .Errors.Count > 1 Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If

                    If .CashListItem IsNot Nothing AndAlso .CashListItem.Count > 0 Then

                        For Each oPaymentItems As BaseClasses.BaseCreatePaymentCashListWithItemsResponseTypeCashListItem In .CashListItem
                            oPayment = New PaymentCashList
                            oPayment.AccountShortCode = oPaymentItems.AccountShortCode
                            oPayment.CashListItemKey = oPaymentItems.CashListItemKey
                            oPayment.TransDetailKey = oPaymentItems.TransDetailKey
                            oPayment.DocumentCode = oPaymentItems.DocumentCode
                            oPayment.DocumentRef = oPaymentItems.DocumentRef
                            oPayment.AutoAllocatePaymentSuccessful = oPaymentItems.AutoAllocatePaymentSuccessful
                            oPaymentCashListCollection.Add(oPayment)
                        Next

                    End If

                    v_oPaymentCashListItem.PaymentCashList = oPaymentCashListCollection

                    v_oPaymentCashListItem.CashListKey = .CashListKey
                    ' v_oPaymentCashListItem. = .TransDetailKey

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("CreatePaymentCashListWithItems executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output : " & vbCrLf)
                    sbLogMessage.AppendLine(v_oPaymentCashListItem.Print().Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

            Catch ex As Exception
                Throw
            Finally

                oCreatePaymentCashListWithItemsRequest = Nothing
                oCreatePaymentCashListWithItemsResponse = Nothing
            End Try

        End SyncLock
    End Sub

    ''' <summary>
    ''' This method is used to add cash list with items for receipt.
    ''' </summary>
    ''' <param name="r_oReceiptCashListItem"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function CreateReceiptcashListWithItem(ByRef r_oReceiptCashListItem As ReceiptCashListItemType,
                                                    Optional ByVal v_sBranchCode As String = Nothing) As ReceiptCashListCollection

        SyncLock oLock

            Dim oCreateReceiptCashListWithItemsRequest As CreateReceiptCashListWithItemsCommand 'Request Type
            Dim oCreateReceiptCashListWithItemsResponse As CreateReceiptCashListWithItemsCommandResponse  'Response Type
            Dim oReceiptItem As BaseClasses.BaseReceiptCashListItemType
            Dim oPolicies As BaseClasses.BaseReceiptCashListItemTypePolicies
            Dim oInstalmentPlanDetails As BaseClasses.BaseInstalmentPlanDetailsType
            Dim oReceipt As ReceiptCashList = Nothing
            Dim oReceiptCollection As ReceiptCashListCollection
            'Dim s As Integer = .CashListItem(0).CashListItemKey
            Dim sbLogMessage As StringBuilder

            Try
                oCreateReceiptCashListWithItemsRequest = New CreateReceiptCashListWithItemsCommand
                oCreateReceiptCashListWithItemsResponse = New CreateReceiptCashListWithItemsCommandResponse

                sbLogMessage = New StringBuilder


                With oCreateReceiptCashListWithItemsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .ReceiptCashList = New BaseClasses.BaseReceiptCashListType()
                    .ReceiptCashList.BankAccountCode = r_oReceiptCashListItem.CoreCashList.BankAccountCode  'BaseReceiptCashListType
                    .ReceiptCashList.CurrencyCode = r_oReceiptCashListItem.CoreCashList.CurrencyCode
                    .ReceiptCashList.ListDate = r_oReceiptCashListItem.CoreCashList.ListDate
                    .ReceiptCashList.Reference = "" 'r_oReceiptCashListItem.CoreCashList.Reference
                    .ReceiptCashList.StatusCode = r_oReceiptCashListItem.CoreCashList.StatusCode
                    .ReceiptCashList.TypeCode = r_oReceiptCashListItem.CoreCashList.TypeCode
                    .ReceiptCashList.BankAccountKey = r_oReceiptCashListItem.CoreCashList.BankAccountKey
                    .ReceiptCashList.SubBranchCode = r_oReceiptCashListItem.CoreCashList.SubBranchCode
                    .ReceiptCashList.ReceiptItem = New List(Of BaseClasses.BaseReceiptCashListItemType)
                    For iItems As Integer = 0 To r_oReceiptCashListItem.ReceiptItems.Count - 1

                        If r_oReceiptCashListItem.ReceiptItems(iItems).MediaTypeCode <> "OCP" OrElse (r_oReceiptCashListItem.ReceiptItems(iItems).MediaTypeCode = "OCP" AndAlso r_oReceiptCashListItem.ReceiptItems(iItems).AllocationStatusCode = "Payment Captured") Then


                            oReceiptItem = New BaseReceiptCashListItemType()

                            oReceiptItem.AccountShortCode = r_oReceiptCashListItem.ReceiptItems(iItems).AccountShortCode
                            oReceiptItem.AllocationStatusCode = r_oReceiptCashListItem.ReceiptItems(iItems).AllocationStatusCode
                            oReceiptItem.Amount = r_oReceiptCashListItem.ReceiptItems(iItems).Amount
                            oReceiptItem.Amount_Tendered = r_oReceiptCashListItem.ReceiptItems(iItems).Amount_tendered
                            oReceiptItem.Original_Amount = r_oReceiptCashListItem.ReceiptItems(iItems).Original_amount
                            oReceiptItem.ContactAddress = New BaseSimpleAddressType
                            oReceiptItem.ContactAddress.AddressLine1 = r_oReceiptCashListItem.ReceiptItems(iItems).Address.Address1
                            oReceiptItem.ContactAddress.AddressLine2 = r_oReceiptCashListItem.ReceiptItems(iItems).Address.Address2
                            oReceiptItem.ContactAddress.AddressLine3 = r_oReceiptCashListItem.ReceiptItems(iItems).Address.Address3
                            oReceiptItem.ContactAddress.AddressLine4 = r_oReceiptCashListItem.ReceiptItems(iItems).Address.Address4
                            oReceiptItem.ContactAddress.PostCode = r_oReceiptCashListItem.ReceiptItems(iItems).Address.PostCode
                            oReceiptItem.ContactAddress.CountryCode = r_oReceiptCashListItem.ReceiptItems(iItems).Address.CountryCode
                            oReceiptItem.AccountBaseDate = r_oReceiptCashListItem.ReceiptItems(iItems).AccountBaseDate
                            oReceiptItem.AccountBaseXrate = r_oReceiptCashListItem.ReceiptItems(iItems).AccountBaseXrate
                            oReceiptItem.CurrencyBaseDate = r_oReceiptCashListItem.ReceiptItems(iItems).CurrencyBaseDate
                            oReceiptItem.CurrencyBaseXrate = r_oReceiptCashListItem.ReceiptItems(iItems).CurrencyBaseXrate
                            oReceiptItem.SystemBaseDate = r_oReceiptCashListItem.ReceiptItems(iItems).SystemBaseDate
                            oReceiptItem.SystemBaseXrate = r_oReceiptCashListItem.ReceiptItems(iItems).SystemBaseXrate
                            oReceiptItem.OverrideReason = r_oReceiptCashListItem.ReceiptItems(iItems).OverrideReason
                            If r_oReceiptCashListItem.ReceiptItems(iItems).Bank.PayeeName <> "" Then
                                oReceiptItem.Bank = New BaseBankReceiptType
                                oReceiptItem.Bank.BankCode = r_oReceiptCashListItem.ReceiptItems(iItems).Bank.Code
                                oReceiptItem.Bank.ChequeDate = r_oReceiptCashListItem.ReceiptItems(iItems).Bank.ChequeDate
                                oReceiptItem.Bank.PayerName = r_oReceiptCashListItem.ReceiptItems(iItems).Bank.PayeeName
                                oReceiptItem.Bank.BankLocation = r_oReceiptCashListItem.ReceiptItems(iItems).Bank.DraweeBankLocation
                                oReceiptItem.Bank.BankBranch = r_oReceiptCashListItem.ReceiptItems(iItems).Bank.DraweeBankBranch
                                oReceiptItem.Bank.ChequeClearingTypeCode = r_oReceiptCashListItem.ReceiptItems(iItems).Bank.ChequeClearingType
                                oReceiptItem.Bank.ChequeTypeCode = r_oReceiptCashListItem.ReceiptItems(iItems).Bank.ChequeType
                                If Not String.IsNullOrEmpty(r_oReceiptCashListItem.ReceiptItems(iItems).Bank.InstrumentNumber) Then
                                    oReceiptItem.Bank.ChequeTypeCode = r_oReceiptCashListItem.ReceiptItems(iItems).Bank.InstrumentNumber
                                Else
                                    oReceiptItem.MediaReference = r_oReceiptCashListItem.ReceiptItems(iItems).MediaReference
                                End If
                            Else
                                oReceiptItem.MediaReference = r_oReceiptCashListItem.ReceiptItems(iItems).MediaReference
                            End If
                            If r_oReceiptCashListItem.ReceiptItems(iItems).CreditCard.Number IsNot Nothing Then
                                oReceiptItem.CreditCard = New BaseCreditCardType
                                oReceiptItem.CreditCard.AuthCode = r_oReceiptCashListItem.ReceiptItems(iItems).CreditCard.AuthCode
                                oReceiptItem.CreditCard.CustomerPresent = r_oReceiptCashListItem.ReceiptItems(iItems).CreditCard.CustomerPresent
                                oReceiptItem.CreditCard.ExpiryDate = r_oReceiptCashListItem.ReceiptItems(iItems).CreditCard.ExpiryDate
                                oReceiptItem.CreditCard.Issue = r_oReceiptCashListItem.ReceiptItems(iItems).CreditCard.Issue
                                oReceiptItem.CreditCard.ManualAuthCode = r_oReceiptCashListItem.ReceiptItems(iItems).CreditCard.ManualAuthCode
                                oReceiptItem.CreditCard.NameOnCreditCard = r_oReceiptCashListItem.ReceiptItems(iItems).CreditCard.NameOnCreditCard
                                oReceiptItem.CreditCard.Number = r_oReceiptCashListItem.ReceiptItems(iItems).CreditCard.Number
                                oReceiptItem.CreditCard.Pin = r_oReceiptCashListItem.ReceiptItems(iItems).CreditCard.Pin
                                oReceiptItem.CreditCard.StartDate = r_oReceiptCashListItem.ReceiptItems(iItems).CreditCard.StartDate
                                oReceiptItem.CreditCard.TransactionCode = r_oReceiptCashListItem.ReceiptItems(iItems).CreditCard.TransactionCode
                                oReceiptItem.CreditCard.TypeCode = r_oReceiptCashListItem.ReceiptItems(iItems).CreditCard.TypeCode
                                oReceiptItem.CreditCard.CashListItemBankCode = r_oReceiptCashListItem.ReceiptItems(iItems).CreditCard.CCIssueBank
                            End If
                            Dim oInstalmentDetails As BaseInstalmentPlanDetailsInstalmentDetails
                            If r_oReceiptCashListItem.InstalmentPlanCollection IsNot Nothing AndAlso r_oReceiptCashListItem.InstalmentPlanCollection.Count > 0 AndAlso r_oReceiptCashListItem.ReceiptItems.Item(iItems).TypeCode = "INST" AndAlso r_oReceiptCashListItem.InstalmentPlanCollection.Count > 0 Then
                                oReceiptItem.InstalmentPlanDetails = New List(Of BaseInstalmentPlanDetailsType)
                                For iCount As Integer = 0 To r_oReceiptCashListItem.InstalmentPlanCollection.Count - 1
                                    If r_oReceiptCashListItem.InstalmentPlanCollection(iCount).InstalmentDetails IsNot Nothing Then
                                        oInstalmentPlanDetails = New BaseInstalmentPlanDetailsType
                                        oInstalmentPlanDetails.FinancePlanKey = r_oReceiptCashListItem.InstalmentPlanCollection(iCount).FinancePlanKey
                                        oInstalmentPlanDetails.FinancePlanVersion = r_oReceiptCashListItem.InstalmentPlanCollection(iCount).FinancePlanVersion
                                        oInstalmentPlanDetails.InstalmentDetails = New List(Of BaseInstalmentPlanDetailsInstalmentDetails)

                                        oInstalmentDetails = New BaseInstalmentPlanDetailsInstalmentDetails
                                        oInstalmentDetails.PFInstalmentID = r_oReceiptCashListItem.InstalmentPlanCollection(iCount).InstalmentDetails.PFInstalmentsKey
                                        oInstalmentDetails.Amount = r_oReceiptCashListItem.InstalmentPlanCollection(iCount).InstalmentDetails.Amount
                                        oInstalmentDetails.InstalmentNumber = r_oReceiptCashListItem.InstalmentPlanCollection(iCount).InstalmentDetails.InstalmentNumber
                                        oInstalmentDetails.IsPartialPayment = r_oReceiptCashListItem.InstalmentPlanCollection(iCount).InstalmentDetails.IsPartialPayment
                                        oInstalmentDetails.IsWriteOffPayment = r_oReceiptCashListItem.InstalmentPlanCollection(iCount).InstalmentDetails.IsWriteOffPayment
                                        oInstalmentDetails.WriteOffReasonID = r_oReceiptCashListItem.InstalmentPlanCollection(iCount).InstalmentDetails.WriteOffReasonID
                                        oInstalmentDetails.OverPaymentWriteOffAmount = r_oReceiptCashListItem.InstalmentPlanCollection(iCount).InstalmentDetails.OverPaymentWriteOffAmount
                                        oInstalmentDetails.ActualAmount = r_oReceiptCashListItem.InstalmentPlanCollection(iCount).InstalmentDetails.ActualAmount
                                        oInstalmentPlanDetails.InstalmentDetails.Add(oInstalmentDetails)
                                        oReceiptItem.InstalmentPlanDetails.Add(oInstalmentPlanDetails)
                                    End If
                                Next
                            End If

                            If oReceiptItem.AllocationDetails Is Nothing And r_oReceiptCashListItem.AutoAllocateIfAble Then
                                oReceiptItem.AllocationDetails = New BaseAllocationType()
                                oReceiptItem.AllocationDetails.AutoAllocate = r_oReceiptCashListItem.AutoAllocateIfAble
                            End If

                            If r_oReceiptCashListItem.ReceiptItems(iItems).Policies IsNot Nothing Then
                                If r_oReceiptCashListItem.ReceiptItems(iItems).Policies.Count > 0 Then
                                    oReceiptItem.Policies = New List(Of BaseReceiptCashListItemTypePolicies)
                                    For iCount As Integer = 0 To r_oReceiptCashListItem.ReceiptItems(iItems).Policies.Count - 1
                                        oPolicies = New BaseReceiptCashListItemTypePolicies
                                        oPolicies.AmountTobeAllocated = r_oReceiptCashListItem.ReceiptItems(iItems).Policies(iCount).AmountTobeAllocated
                                        oPolicies.BGKey = r_oReceiptCashListItem.ReceiptItems(iItems).Policies(iCount).BGKey
                                        oPolicies.InsuranceFileKey = r_oReceiptCashListItem.ReceiptItems(iItems).Policies(iCount).InsuranceFileKey
                                        oReceiptItem.Policies.Add(oPolicies)
                                    Next
                                End If
                            End If
                            oReceiptItem.ContactName = r_oReceiptCashListItem.ReceiptItems(iItems).ContactName
                            oReceiptItem.FurtherDetails = r_oReceiptCashListItem.ReceiptItems(iItems).FurtherDetails
                            oReceiptItem.IsProduceDocument = r_oReceiptCashListItem.ReceiptItems(iItems).IsProduceDocument
                            oReceiptItem.MediaTypeCode = r_oReceiptCashListItem.ReceiptItems(iItems).MediaTypeCode
                            oReceiptItem.OurReference = r_oReceiptCashListItem.ReceiptItems(iItems).OurReference
                            oReceiptItem.StatusCode = "ADD" ' r_oReceiptCashListItem.ReceiptItems(iItems).StatusCode '"ADD" '
                            oReceiptItem.BankReference = r_oReceiptCashListItem.ReceiptItems(iItems).BankReference
                            oReceiptItem.TheirReference = r_oReceiptCashListItem.ReceiptItems(iItems).TheirReference
                            oReceiptItem.TransactionDate = r_oReceiptCashListItem.ReceiptItems(iItems).TransactionDate
                            oReceiptItem.TypeCode = r_oReceiptCashListItem.ReceiptItems(iItems).TypeCode '"STD" '
                            If oReceiptItem.TypeCode.Trim.ToUpper = "BGDEPT" Then
                                oReceiptItem.AllocationType = r_oReceiptCashListItem.AllocationType
                                oReceiptItem.AllocationTypeSpecified = True
                            End If
                            'oReceiptItem.AutoAllocatePaymentSuccessful = r_oReceiptCashListItem.ReceiptItems(iItems).AutoAllocatePaymentSuccessful
                            .ReceiptCashList.ReceiptItem.Add(oReceiptItem)
                        End If
                    Next
                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Post(ApiMethods.CreateReceiptCashListWithItems, oCreateReceiptCashListWithItemsRequest)
                    oCreateReceiptCashListWithItemsResponse = ApiClient.DeserializeJson(Of CreateReceiptCashListWithItemsCommandResponse)(result)
                End Using

                oReceiptCollection = New ReceiptCashListCollection

                With oCreateReceiptCashListWithItemsResponse
                    If .Errors IsNot Nothing AndAlso .Errors.Count > 0 Then
                        'Process the error object if errors, and throw as a single exception CashListItem
                        Throw New NexusException(.Errors)
                    End If


                    ' For Each oReceiptItem As BaseCreateReceiptCashListWithItemsResponseTypeCashListItem In .CashListItem
                    If .CashListItem IsNot Nothing AndAlso .CashListItem.Count > 0 Then
                        For i As Integer = 0 To .CashListItem.Count - 1
                            oReceipt = New ReceiptCashList
                            oReceipt.CashListKey = .CashListKey
                            oReceipt.AccountShortCode = .CashListItem(i).AccountShortCode
                            oReceipt.CashListItemKey = .CashListItem(i).CashListItemKey
                            oReceipt.TransDetailKey = .CashListItem(i).TransDetailKey
                            oReceipt.DocumentCode = .CashListItem(i).DocumentCode
                            oReceipt.DocumentRef = .CashListItem(i).DocumentRef
                            ' oReceipt.AutoAllocatePaymentSuccessful = .CashListItem(i).AutoAllocatePaymentSuccessful
                            oReceiptCollection.Add(oReceipt)
                        Next
                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("CreateReceiptCashListWithItems executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If
                    sbLogMessage.AppendLine("Output : " & vbCrLf)
                    sbLogMessage.AppendLine(oReceiptCollection.Print().Replace("<br />", vbCrLf))
                    LogMessageEntry(sbLogMessage)
                End If
                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oCreateReceiptCashListWithItemsRequest = Nothing
                oCreateReceiptCashListWithItemsResponse = Nothing
            End Try


            Return oReceiptCollection
        End SyncLock

    End Function


    ''' <summary>
    ''' To find list of accounts depending upon the search criteria.
    ''' </summary>
    ''' <param name="v_oAccountSearchCriteria"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function FindAccounts(ByVal v_oAccountSearchCriteria As AccountSearchCriteria,
                                        Optional ByVal v_sBranchCode As String = Nothing) As AccountSearchResultCollection
        SyncLock oLock

            Dim oFindAccountsRequest As FindAccountsQuery
            Dim oFindAccountsResponse As FindAccountsQueryResponse
            Dim oAccountSearchResult As AccountSearchResultCollection = Nothing
            Dim oNewAccountSearchResult As AccountSearchResult
            Dim sbLogMessage As StringBuilder

            Try
                oFindAccountsRequest = New FindAccountsQuery
                oFindAccountsResponse = New FindAccountsQueryResponse
                sbLogMessage = New StringBuilder

                With oFindAccountsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        .BranchCode = "HeadOff"
                    Else
                        'Use the branch code in session 
                        .BranchCode = sBranchCode
                    End If

                    'Else
                    'use the passed parameter v_sBranchCode
                    '.BranchCode = v_sBranchCode
                    .AccountName = v_oAccountSearchCriteria.AccountName
                    .AccountTypeCode = v_oAccountSearchCriteria.AccountTypeCode
                    .ExcludeInsurerAgents = v_oAccountSearchCriteria.ExcludeInsurerAgents
                    .IncludeInsurerAgents = v_oAccountSearchCriteria.IncludeInsurerAgents
                    .InsuranceRef = v_oAccountSearchCriteria.InsuranceRef
                    .LedgerCode = v_oAccountSearchCriteria.LedgerCode

                    If v_oAccountSearchCriteria.OnlyUpdatableAccounts > 0 Then

                        .OnlyUpdatableAccounts = v_oAccountSearchCriteria.OnlyUpdatableAccounts
                        ' .OnlyUpdatableAccountsSpecified = True sandeep
                    Else
                        ' .OnlyUpdatableAccountsSpecified = False sandeep
                    End If

                    If v_oAccountSearchCriteria.OperatorKey > 0 Then
                        .OperatorKey = v_oAccountSearchCriteria.OperatorKey
                        .OperatorKeySpecified = True
                    Else
                        .OperatorKeySpecified = False
                    End If

                    .PurchaseInvoiceNo = v_oAccountSearchCriteria.PurchaseInvoiceNo
                    .PurchaseOrderNo = v_oAccountSearchCriteria.PurchaseOrderNo
                    .ShortCode = v_oAccountSearchCriteria.ShortCode
                    .Spare = Nothing

                    If v_oAccountSearchCriteria.ShowBalance > 0 Then
                        .ShowBalance = v_oAccountSearchCriteria.ShowBalance
                        ' .ShowBalanceSpecified = True sandeep
                    Else
                        ' .ShowBalanceSpecified = False

                    End If

                    If v_oAccountSearchCriteria.ShowDeleted > 0 Then
                        .ShowDeleted = v_oAccountSearchCriteria.ShowDeleted
                        ' .ShowDeletedSpecified = True andeep
                    Else
                        ' .ShowDeletedSpecified = False
                    End If

                    If v_oAccountSearchCriteria.ShowDeletedSpecified Then
                        .ShowDeleted = v_oAccountSearchCriteria.ShowDeleted
                        ' .ShowDeletedSpecified = v_oAccountSearchCriteria.ShowDeletedSpecified sandeep
                    Else
                        ' .ShowDeletedSpecified = v_oAccountSearchCriteria.ShowDeletedSpecified
                    End If

                    If v_oAccountSearchCriteria.ShowBalanceSpecified Then
                        .ShowBalance = v_oAccountSearchCriteria.ShowBalance
                        '.ShowBalanceSpecified = v_oAccountSearchCriteria.ShowBalanceSpecified sandeep
                    Else
                        ' .ShowBalanceSpecified = v_oAccountSearchCriteria.ShowBalanceSpecified
                    End If

                    If v_oAccountSearchCriteria.MaxRowsToFetch > 0 Then
                        .MaxRowsToFetch = v_oAccountSearchCriteria.MaxRowsToFetch
                        .MaxRowsToFetchSpecified = True
                    Else
                        .MaxRowsToFetchSpecified = False
                    End If
                    If Not String.IsNullOrEmpty(v_oAccountSearchCriteria.Spare) Then
                        .Spare = v_oAccountSearchCriteria.Spare
                    End If

                End With
                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.FindAccount, oFindAccountsRequest)
                    oFindAccountsResponse = ApiClient.DeserializeJson(Of FindAccountsQueryResponse)(result)
                End Using

                With oFindAccountsResponse.FindAccountsResponse

                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        If .Accounts IsNot Nothing AndAlso .Accounts.Count > 0 Then

                            oAccountSearchResult = New AccountSearchResultCollection

                            For Each oFindAccount As BaseFindAccountsResponseTypeRow In .Accounts

                                oNewAccountSearchResult = New AccountSearchResult()

                                With oNewAccountSearchResult
                                    .AccountBalance = oFindAccount.AccountBalance
                                    .AccountKey = oFindAccount.AccountKey
                                    .AccountName = oFindAccount.AccountName
                                    .AccountStatus = oFindAccount.AccountStatus
                                    .AccountStatusKey = oFindAccount.AccountStatusKey
                                    .AccountTypeCode = oFindAccount.AccountTypeCode
                                    .AccountTypeKey = oFindAccount.AccountTypeKey
                                    .AddressLine1 = oFindAccount.AddressLine1
                                    .CompanyKey = oFindAccount.CompanyKey
                                    .ContactName = oFindAccount.ContactName
                                    .FullKey = oFindAccount.FullKey
                                    .LedgerCode = oFindAccount.LedgerCode
                                    .LedgerKey = oFindAccount.LedgerKey
                                    .NominalAccountKey = oFindAccount.NominalAccountKey
                                    .PersonalClientForename = oFindAccount.PersonalClientForename
                                    .ShortCode = oFindAccount.ShortCode
                                    .PartyKey = oFindAccount.PartyKey
                                    .CurrencyCode = oFindAccount.CurrencyCode
                                    .SourceID = oFindAccount.SourceId
                                    .SourceCode = oFindAccount.SourceCode
                                    .IsGrossAgent = oFindAccount.IsGrossAgent
                                End With

                                oAccountSearchResult.Add(oNewAccountSearchResult)

                            Next

                        End If
                    End If

                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("FindAccounts executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_oAccountSearchCriteria = " & v_oAccountSearchCriteria.Print.Replace("<br/>", vbCrLf))
                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If
                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally

                oFindAccountsRequest = Nothing
                oFindAccountsResponse = Nothing
            End Try


            Return oAccountSearchResult

        End SyncLock
    End Function

    Public Overrides Function GetAccountBalance(Optional ByVal v_sBranchCode As String = Nothing) As AccountBalancecollection

        SyncLock oLock

            Dim oGetAccountBalanceRequest As GetAccountBalanceQuery
            Dim oGetAccountBalanceResponse As GetAccountBalanceQueryResponse
            Dim oAccountBalanceCollection As AccountBalancecollection
            Dim oAccountBalance As AccountBalance
            Dim sbLogMessage As StringBuilder

            Try
                oGetAccountBalanceRequest = New GetAccountBalanceQuery
                oGetAccountBalanceResponse = New GetAccountBalanceQueryResponse
                sbLogMessage = New StringBuilder

                With oGetAccountBalanceRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetAccountBalance, oGetAccountBalanceRequest)
                    oGetAccountBalanceResponse = ApiClient.DeserializeJson(Of GetAccountBalanceQueryResponse)(result)
                End Using

                oAccountBalanceCollection = New AccountBalancecollection

                With oGetAccountBalanceResponse
                    If .AccountBalance IsNot Nothing AndAlso .AccountBalance.Count > 0 Then
                        For Each oAccountB As BaseGetAccountBalanceResponseTypeRow In .AccountBalance
                            oAccountBalance = New AccountBalance

                            oAccountBalance.CurrencyCode = oAccountB.CurrencyCode
                            oAccountBalance.FloatBalance = oAccountB.FloatBalance
                            oAccountBalance.Overdraft = oAccountB.Overdraft
                            oAccountBalance.SumAmount = oAccountB.SumAmount

                            oAccountBalanceCollection.Add(oAccountBalance)
                        Next
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("GetAccountBalance executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Output:" & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally

                oGetAccountBalanceRequest = Nothing
                oGetAccountBalanceResponse = Nothing
            End Try

            Return oAccountBalanceCollection

        End SyncLock

    End Function

    ''' <summary>
    ''' To get the account details of the selected party.
    ''' </summary>
    ''' <param name="v_oAccountDetails"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetAccountDetails(ByVal v_oAccountDetails As AccountDetails,
                                     Optional ByVal v_sBranchCode As String = Nothing) As AccountDetailsDefaults
        SyncLock oLock


            Dim oGetAccountDetailsRequest As GetAccountDetailsQuery
            Dim oGetAccountDetailsResponse As GetAccountDetailsQueryResponse
            Dim oAccountDefaults As NexusProvider.AccountDetailsDefaults
            Dim oAccountDetails As AccountDetails
            Dim sbLogMessage As StringBuilder

            Try

                oGetAccountDetailsRequest = New GetAccountDetailsQuery
                oGetAccountDetailsResponse = New GetAccountDetailsQueryResponse
                oAccountDefaults = New NexusProvider.AccountDetailsDefaults
                sbLogMessage = New StringBuilder

                With oGetAccountDetailsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    If Not IsNothing(Current.Session(Nexus.Constants.CNAgentDetails)) Then
                        .AgentCnt = CType(Current.Session(Nexus.Constants.CNAgentDetails), NexusProvider.UserDetails).Key
                        .AgentKey = CType(Current.Session(Nexus.Constants.CNAgentDetails), NexusProvider.UserDetails).Key
                        .AgentCode = CType(Current.Session(Nexus.Constants.CNAgentDetails), NexusProvider.UserDetails).PartyCode
                    End If
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    If v_oAccountDetails.PartyCnt > 0 Then
                        .PartyCnt = v_oAccountDetails.PartyCnt
                        .PartyCntSpecified = True
                    End If

                    If v_oAccountDetails.AccountKey > 0 Then
                        .AccountKey = v_oAccountDetails.AccountKey
                        .AccountKeySpecified = True
                    End If
                    .DocumentRef = v_oAccountDetails.DocumentRef
                    .CurrencyCode = v_oAccountDetails.CurrencyCode

                    If v_oAccountDetails.CurrencyAmount <> 0 Then
                        .CurrencyAmount = v_oAccountDetails.CurrencyAmount
                        .CurrencyAmountSpecified = True
                    End If

                    If v_oAccountDetails.Tolerance > 0 Then
                        .Tolerance = v_oAccountDetails.Tolerance
                        .ToleranceSpecified = True
                    Else
                        .ToleranceSpecified = False
                    End If

                    .DocTypeGroupCode = v_oAccountDetails.DocTypeGroupCode
                    .DocumentTypeCode = v_oAccountDetails.DocumentTypeCode
                    If v_oAccountDetails.PeriodKey > 0 Then
                        .PeriodKey = v_oAccountDetails.PeriodKey
                        .PeriodKeySpecified = True
                    End If

                    If v_oAccountDetails.DateFrom <> Date.MinValue Then
                        .DateFrom = v_oAccountDetails.DateFrom
                        .DateFromSpecified = True
                    End If

                    If v_oAccountDetails.DateTo <> Date.MinValue Then
                        .DateTo = v_oAccountDetails.DateTo
                        .DateToSpecified = True
                    End If

                    .InsuranceRef = v_oAccountDetails.InsuranceRef
                    .Username = v_oAccountDetails.Username
                    .PurchaseInvoiceNo = v_oAccountDetails.PurchaseInvoiceNo
                    .PurchaseOrderNo = v_oAccountDetails.PurchaseOrderNo
                    .Department = v_oAccountDetails.Department
                    If Not String.IsNullOrEmpty(v_oAccountDetails.Spare) Then
                        .Spare = v_oAccountDetails.Spare
                    End If
                    '.MediaRef = v_oAccountDetails.MediaRef

                    If v_oAccountDetails.OutstandingOnly = True Then
                        .OutstandingOnly = v_oAccountDetails.OutstandingOnly
                        .OutstandingOnlySpecified = True
                        .OutstandingOnlySpecified1 = True
                    End If

                    If v_oAccountDetails.IsNewPF = True Then
                        .IsNewPFSpecified1 = True
                        .IsNewPFSpecified = True
                        .IsNewPF = v_oAccountDetails.IsNewPF
                    End If

                    If v_oAccountDetails.Display500 = True Then
                        .Display500Specified = True
                        .Display500 = v_oAccountDetails.Display500
                    End If

                    If v_oAccountDetails.IncludeReversedTran = True Then
                        .IncludeReversedTran = v_oAccountDetails.IncludeReversedTran
                        .IncludeReversedTranSpecified = True
                    End If

                    If v_oAccountDetails.InsuredAccountKey > 0 Then
                        .InsuredAccountKey = v_oAccountDetails.InsuredAccountKey
                        .InsuredAccountKeySpecified = True
                    End If

                    .Rollup = v_oAccountDetails.Rollup
                    .RollupSpecified = v_oAccountDetails.Rollup

                    If v_oAccountDetails.CashListKey > 0 Then
                        .CashListKey = v_oAccountDetails.CashListKey
                        .CashListKeySpecified = True
                    End If

                    .OrderBySpare = v_oAccountDetails.OrderBySpare
                    .OrderBySpareSpecified = v_oAccountDetails.OrderBySpare

                    If v_oAccountDetails.DocumentKey > 0 Then
                        .DocumentKey = v_oAccountDetails.DocumentKey
                        .DocumentKeySpecified = True
                    End If

                    If v_oAccountDetails.FinancePlanKey > 0 Then
                        .FinancePlanKey = v_oAccountDetails.FinancePlanKey
                        .FinancePlanKeySpecified = True
                    End If

                    If v_oAccountDetails.UnderwritingYearKey > 0 Then
                        .UnderwritingYearKey = v_oAccountDetails.UnderwritingYearKey
                        .UnderwritingYearKeySpecified = True
                    End If

                    If Not v_oAccountDetails.DueDateFrom Is Nothing Then
                        .DueDateFrom = v_oAccountDetails.DueDateFrom
                        .DueDateFromSpecified = True
                    End If

                    If Not v_oAccountDetails.DueDateTo Is Nothing Then
                        .DueDateTo = v_oAccountDetails.DueDateTo
                        .DueDateToSpecified = True
                    End If

                    .SourceArray = v_oAccountDetails.SourceArray
                    .TransDetailKeys = v_oAccountDetails.TransDetailKeys
                    .Display500 = v_oAccountDetails.Display500
                    .Display500Specified = v_oAccountDetails.Display500
                    .AltReference = v_oAccountDetails.AltReference
                    .IncludeReversedTran = v_oAccountDetails.IncludeReversedTran
                    .IncludeReversedTranSpecified = v_oAccountDetails.IncludeReversedTran
                    .BGRef = v_oAccountDetails.BGRef
                    '' .Insurance_file_cnt = v_oAccountDetails.Insurance_filecnt sandeep
                    '' .Insurance_folder_cnt = v_oAccountDetails.Insurance_foldercnt

                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()

                    Dim result As String = ApiClient.Get(ApiMethods.GetAccountDetails, oGetAccountDetailsRequest)
                    oGetAccountDetailsResponse = ApiClient.DeserializeJson(Of GetAccountDetailsQueryResponse)(result)
                End Using

                With oGetAccountDetailsResponse.GetAccountDetailsResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        oAccountDefaults.AccountBalance = .AccountBalance
                        oAccountDefaults.AccountName = .AccountName
                        oAccountDefaults.ContactName = .ContactName
                        oAccountDefaults.PhoneExtension = .PhoneExtension
                        oAccountDefaults.PhoneAreaCode = .PhoneAreaCode
                        oAccountDefaults.PhoneNumber = .PhoneNumber
                        oAccountDefaults.AccountStatus = .AccountStatus
                        oAccountDefaults.TransactionCurrencyOutStandingBalance = .TransactionCurrencyOSBalance

                        If .Transactions IsNot Nothing AndAlso .Transactions.Count > 0 Then

                            For Each oAccountDetailsRow As BaseGetAccountDetailsResponseTypeTransactions In .Transactions
                                oAccountDetails = New AccountDetails()
                                oAccountDetails.TransDetailKeys = oAccountDetailsRow.TransDetailKey
                                oAccountDetails.BranchKey = oAccountDetailsRow.BranchKey
                                oAccountDetails.Account = oAccountDetailsRow.Account
                                oAccountDetails.DocRef = oAccountDetailsRow.DocRef
                                oAccountDetails.AltRef = oAccountDetailsRow.AltRef
                                oAccountDetails.EffectiveDate = oAccountDetailsRow.EffectiveDate
                                oAccountDetails.TransDate = oAccountDetailsRow.TransDate
                                oAccountDetails.MediaType = oAccountDetailsRow.MediaType
                                oAccountDetails.Amount = oAccountDetailsRow.Amount
                                oAccountDetails.PrimarySettled = oAccountDetailsRow.PrimarySettled
                                oAccountDetails.OutstandingAmount = oAccountDetailsRow.OutstandingAmount
                                If oAccountDetails.OutstandingAmount.Equals(oAccountDetails.Amount) Then
                                    oAccountDetails.PaidDate = String.Empty
                                Else
                                    oAccountDetails.PaidDate = oAccountDetailsRow.PaidDate.ToShortDateString
                                End If
                                oAccountDetails.DocTypeId = oAccountDetailsRow.DocTypeId
                                oAccountDetails.DocumentTypeCode = oAccountDetailsRow.DocumentTypeCode
                                oAccountDetails.Reference = oAccountDetailsRow.Reference
                                oAccountDetails.OperatorName = oAccountDetailsRow.OperatorName
                                oAccountDetails.Period = oAccountDetailsRow.Period
                                oAccountDetails.DocumentGroupId = oAccountDetailsRow.DocumentGroupId
                                oAccountDetails.DocTypeGroupCode = oAccountDetailsRow.DocumentGroupCode
                                oAccountDetails.Client = oAccountDetailsRow.Client
                                oAccountDetails.ClientCode = oAccountDetailsRow.ClientCode
                                oAccountDetails.MediaRef = oAccountDetailsRow.MediaRef
                                oAccountDetails.AccountKey = oAccountDetailsRow.Accountkey
                                oAccountDetails.PayeeName = oAccountDetailsRow.PayeeName
                                oAccountDetails.UnderwritingYear = oAccountDetailsRow.UnderwritingYear
                                oAccountDetails.AccountOutStandingAmount = oAccountDetailsRow.AccountOutStandingAmount
                                oAccountDetails.CurrencyAmount = oAccountDetailsRow.CurrencyAmount
                                oAccountDetails.OutStandingCurrencyAmount = oAccountDetailsRow.OutStandingCurrencyAmount
                                oAccountDetails.BGRef = oAccountDetailsRow.BGRef
                                oAccountDetails.MediaRef = oAccountDetailsRow.MediaRef
                                oAccountDetails.CurrencyCode = oAccountDetailsRow.CurrencyCode
                                oAccountDetails.AccountCurrencyCode = oAccountDetailsRow.AccountCurrencyCode
                                oAccountDetails.AccountAmount = oAccountDetailsRow.AccountAmount
                                oAccountDetails.BalanceType = oAccountDetailsRow.BalanceType
                                oAccountDetails.BaseCurrencyCode = oAccountDetailsRow.BaseCurrencyCode
                                oAccountDetails.CashListKey = oAccountDetailsRow.CashListKey
                                oAccountDetails.IsSplitReceipt = oAccountDetailsRow.IsSplitReceipt
                                oAccountDetails.IsLeadAgent = oAccountDetailsRow.IsLeadAgent
                                If oAccountDetailsRow.DueDate <> Date.MinValue Then
                                    oAccountDetails.DueDate = oAccountDetailsRow.DueDate
                                End If
                                oAccountDetails.CashListItemKey = oAccountDetailsRow.CashListItemKey
                                oAccountDetails.BankAccountID = oAccountDetailsRow.BankAccountID
                                oAccountDetails.PartyCnt = oAccountDetailsRow.PartyCnt
                                oAccountDetails.FinancePlanKey = oAccountDetailsRow.FinancePlanKey
                                oAccountDetails.FinancePlanVersion = oAccountDetailsRow.FinancePlanVersion
                                oAccountDetails.FinancePlanStatus = oAccountDetailsRow.FinancePlanStatus
                                oAccountDetails.Insurance_filecnt = oAccountDetailsRow.InsuranceFileCnt
                                oAccountDetails.Insurance_foldercnt = oAccountDetailsRow.InsuranceFolderCnt
                                oAccountDefaults.AccountDetails.Add(oAccountDetails)
                            Next
                        End If
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetAccountDetails executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    'sbLogMessage.AppendLine("v_iAddressKey = " & v_iAddressKey.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If
                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)
                'FaultErrorHandler(ex) ' handling fault error messages 
            Catch ex As Exception
                Throw
            Finally

                oGetAccountDetailsRequest = Nothing
                oGetAccountDetailsResponse = Nothing
            End Try

            Return oAccountDefaults

        End SyncLock
    End Function

    ''' <summary>
    ''' To get party shortname from Account ID.
    ''' </summary>
    ''' <param name="v_iPartyCnt"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetAccountShortCodeFromParty(ByVal v_iPartyCnt As Integer,
                                      Optional ByVal v_sBranchCode As String = Nothing) As String
        SyncLock oLock


            Dim oGetAccountShortCodeFromPartyRequest As New GetAccountShortCodeFromPartyQuery
            Dim oGetAccountShortCodeFromPartyQueryResponse As New GetAccountShortCodeFromPartyQueryResponse
            Dim sbLogMessage As StringBuilder
            Dim sAccountShortCode As String = Nothing
            Try

                sbLogMessage = New StringBuilder

                oGetAccountShortCodeFromPartyRequest.LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                'if the passed parameter v_sBranchCode is empty
                If String.IsNullOrEmpty(v_sBranchCode) Then
                    'if the branch code is NOT in session 
                    If String.IsNullOrEmpty(sBranchCode) Then
                        'Use the default branch code
                        oGetAccountShortCodeFromPartyRequest.BranchCode = sDefaultBranchCode
                    Else
                        'Use the branch code in session 
                        oGetAccountShortCodeFromPartyRequest.BranchCode = sBranchCode
                    End If
                Else
                    'use the passed parameter v_sBranchCode
                    oGetAccountShortCodeFromPartyRequest.BranchCode = v_sBranchCode
                End If

                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetAccountShortCodeFromParty, oGetAccountShortCodeFromPartyRequest)
                    oGetAccountShortCodeFromPartyQueryResponse = ApiClient.DeserializeJson(Of GetAccountShortCodeFromPartyQueryResponse)(result)
                End Using
                Return oGetAccountShortCodeFromPartyQueryResponse.GetAccountShortCodeFromPartyResponse.AccountShortCode
            Catch ex As Exception
                Return ""
            End Try
        End SyncLock
    End Function

    Public Overrides Function GetAccountingPeriod(Optional ByVal v_sBranchCode As String = Nothing) As LookupListCollection
        SyncLock oLock

            Dim oBaseGetAccountingPeriodRequest As GetAccountingPeriodQuery   ' Request Type
            Dim oBaseGetAccountingPeriodResponse As GetAccountingPeriodQueryResponse    ' Response Type
            Dim ollCollection As NexusProvider.LookupListCollection
            Dim olllist As LookupListItem
            Try

                oBaseGetAccountingPeriodRequest = New GetAccountingPeriodQuery
                oBaseGetAccountingPeriodResponse = New GetAccountingPeriodQueryResponse
                ollCollection = New NexusProvider.LookupListCollection
                olllist = New LookupListItem

                With oBaseGetAccountingPeriodRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    'if the passed parameter v_sBranchCode is empty 
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If

                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    .DateInPeriod = Now.Date
                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetAccountingPeriod, oBaseGetAccountingPeriodRequest)
                    oBaseGetAccountingPeriodResponse = ApiClient.DeserializeJson(Of GetAccountingPeriodQueryResponse)(result)
                End Using

                With oBaseGetAccountingPeriodResponse
                    If .Period IsNot Nothing AndAlso .Period.Count > 0 Then
                        For Each oPeriod As BaseGetAccountingPeriodResponseTypeRow In .Period

                            olllist.Code = oPeriod.PeriodName
                            olllist.Description = oPeriod.YearName & " : " & oPeriod.PeriodName
                            olllist.Key = oPeriod.PeriodKey
                            ollCollection.Add(olllist)
                        Next
                    End If
                End With

            Catch ex As Exception
                Throw
            Finally

                oBaseGetAccountingPeriodRequest = Nothing
                oBaseGetAccountingPeriodResponse = Nothing
            End Try
            Return ollCollection
        End SyncLock
    End Function

    Public Overrides Function GetAllocationDetails(ByVal v_iTransDetailKey As Integer,
                                            Optional ByVal v_sBranchCode As String = Nothing) As AllocationDetailsCollections

        SyncLock oLock


            Dim oGetAllocationDetailsRequest As GetAllocationDetailsQuery 'Request Type
            Dim oGetAllocationDetailsResponse As GetAllocationDetailsQueryResponse 'Response Type
            Dim oAllocationDetailsCollection As AllocationDetailsCollections = Nothing
            Dim oNewAllocationDetails As AllocationDetails
            Dim sbLogMessage As StringBuilder

            Try

                oGetAllocationDetailsRequest = New GetAllocationDetailsQuery
                oGetAllocationDetailsResponse = New GetAllocationDetailsQueryResponse
                sbLogMessage = New StringBuilder

                With oGetAllocationDetailsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    'if the passed parameter v_iTransDetailKey is empty
                    If v_iTransDetailKey > 0 Then
                        .TransDetailKey = v_iTransDetailKey
                    Else
                        Throw New ArgumentNullException("v_iTransDetailKey")
                    End If
                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetAllocationDetails, oGetAllocationDetailsRequest)
                    oGetAllocationDetailsResponse = ApiClient.DeserializeJson(Of GetAllocationDetailsQueryResponse)(result)
                End Using

                With oGetAllocationDetailsResponse
                    If .Row IsNot Nothing AndAlso .Row.Count > 0 Then

                        oAllocationDetailsCollection = New AllocationDetailsCollections
                        For Each oAllocationDetails As BaseGetAllocationDetailsResponseTypeRow In .Row
                            oNewAllocationDetails = New AllocationDetails

                            With oNewAllocationDetails
                                .DocRef = oAllocationDetails.DocRef
                                .TransDate = oAllocationDetails.TransDate
                                .AllocatedDate = oAllocationDetails.AllocatedDate
                                .AllocatedAmount = oAllocationDetails.AllocatedAmount
                                .OriginalAmount = oAllocationDetails.OriginalAmount
                                .WriteOffAmount = oAllocationDetails.WriteOffAmount
                                .DocType = oAllocationDetails.DocType
                                .InsuranceRef = oAllocationDetails.InsuranceRef
                                .Account = oAllocationDetails.Account
                                .User = oAllocationDetails.User
                                .AllocationKey = oAllocationDetails.AllocationKey
                                .TransdetailKey = oAllocationDetails.TransDetailKey
                            End With

                            oAllocationDetailsCollection.Add(oNewAllocationDetails)

                        Next
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetAllocationDetails executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iTransDetailKey = " & v_iTransDetailKey.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If
                    If Not IsNothing(oAllocationDetailsCollection) Then
                        sbLogMessage.AppendLine("Returned " & oAllocationDetailsCollection.Print.ToString & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally

                oGetAllocationDetailsRequest = Nothing
                oGetAllocationDetailsResponse = Nothing
            End Try

            Return oAllocationDetailsCollection

        End SyncLock

    End Function

    Public Overrides Function GetBalancesAndUnallocatedCredits(ByVal v_iInsuranceFileKey As Integer,
                                                    Optional ByVal v_sBranchCode As String = Nothing) As BalancesAndUnallocatedCredits

        SyncLock oLock

            Dim oGetBalancesAndUnallocatedCreditsRequest As GetBalancesAndUnallocatedCreditsQuery
            Dim oGetBalancesAndUnallocatedCreditsResponse As GetBalancesAndUnallocatedCreditsQueryResponse
            Dim oBalancesAndUnallocatedCredits As BalancesAndUnallocatedCredits = Nothing
            Dim oUnallocatedCreditsForAgent As UnallocatedCredit = Nothing
            Dim oUnallocatedCreditsForClient As UnallocatedCredit = Nothing
            Dim sbLogMessage As StringBuilder

            Try

                oGetBalancesAndUnallocatedCreditsRequest = New GetBalancesAndUnallocatedCreditsQuery
                oGetBalancesAndUnallocatedCreditsResponse = New GetBalancesAndUnallocatedCreditsQueryResponse
                sbLogMessage = New StringBuilder

                With oGetBalancesAndUnallocatedCreditsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    If v_iInsuranceFileKey > 0 Then
                        .InsuranceFileKey = v_iInsuranceFileKey
                    Else
                        Throw New ArgumentException("BalancesAndUnallocatedCredits.InsuranceFileKey")
                    End If
                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetBalancesAndUnallocatedCredits, oGetBalancesAndUnallocatedCreditsRequest)
                    oGetBalancesAndUnallocatedCreditsResponse = ApiClient.DeserializeJson(Of GetBalancesAndUnallocatedCreditsQueryResponse)(result)
                End Using

                With oGetBalancesAndUnallocatedCreditsResponse.GetBalancesAndUnallocatedCreditsResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If
                    oBalancesAndUnallocatedCredits = New BalancesAndUnallocatedCredits()

                    oBalancesAndUnallocatedCredits.AccountBalance = .AccountBalance
                    oBalancesAndUnallocatedCredits.AgentKey = .AgentKey
                    oBalancesAndUnallocatedCredits.AgentType = .AgentType
                    oBalancesAndUnallocatedCredits.ClientKey = .ClientKey
                    oBalancesAndUnallocatedCredits.FloatBalanceLimit = .FloatBalanceLimit
                    oBalancesAndUnallocatedCredits.InsuranceRef = .InsuranceRef
                    oBalancesAndUnallocatedCredits.IsFloatBalanceAccount = .IsFloatBalanceAccount
                    oBalancesAndUnallocatedCredits.IsOverDraftAccount = .IsOverDraftAccount
                    oBalancesAndUnallocatedCredits.OverDraftExpiry = .OverDraftExpiry
                    oBalancesAndUnallocatedCredits.OverDraftLimit = .OverDraftLimit

                    If .UnallocatedCreditsForAgents IsNot Nothing AndAlso .UnallocatedCreditsForAgents.Count > 0 Then
                        For Each oBaseoUnallocatedCreditsForAgent As BaseGetBalancesAndUnallocatedCreditsResponseTypeRow In .UnallocatedCreditsForAgents
                            oUnallocatedCreditsForAgent = New UnallocatedCredit()
                            With oUnallocatedCreditsForAgent
                                .AccountKey = oBaseoUnallocatedCreditsForAgent.AccountKey
                                .Amount = oBaseoUnallocatedCreditsForAgent.Amount
                                If oBaseoUnallocatedCreditsForAgent.CollectionDate <> Date.MinValue Then
                                    .CollectionDate = oBaseoUnallocatedCreditsForAgent.CollectionDate
                                    .CollectionDateSpecified = True
                                Else
                                    .CollectionDateSpecified = False
                                End If
                                .DocumentRef = oBaseoUnallocatedCreditsForAgent.DocumentRef
                                .MediaType = oBaseoUnallocatedCreditsForAgent.MediaType
                                .Reference = oBaseoUnallocatedCreditsForAgent.Reference
                                .TransDetailKey = oBaseoUnallocatedCreditsForAgent.TransDetailKey
                            End With
                            oBalancesAndUnallocatedCredits.UnallocatedCreditsForAgents.Add(oUnallocatedCreditsForAgent)
                        Next
                    End If

                    If .UnallocatedCreditsForClients IsNot Nothing AndAlso .UnallocatedCreditsForClients.Count > 0 Then
                        For Each oBaseoUnallocatedCreditsForClient As BaseGetBalancesAndUnallocatedCreditsResponseTypeRow1 In .UnallocatedCreditsForClients
                            oUnallocatedCreditsForClient = New UnallocatedCredit()
                            With oUnallocatedCreditsForClient
                                .AccountKey = oBaseoUnallocatedCreditsForClient.AccountKey
                                .Amount = oBaseoUnallocatedCreditsForClient.Amount
                                .CollectionDate = oBaseoUnallocatedCreditsForClient.CollectionDate
                                .CollectionDateSpecified = oBaseoUnallocatedCreditsForClient.CollectionDateSpecified
                                .DocumentRef = oBaseoUnallocatedCreditsForClient.DocumentRef
                                .MediaType = oBaseoUnallocatedCreditsForClient.MediaType
                                .Reference = oBaseoUnallocatedCreditsForClient.Reference
                                .TransDetailKey = oBaseoUnallocatedCreditsForClient.TransDetailKey
                            End With
                            oBalancesAndUnallocatedCredits.UnallocatedCreditsForClients.Add(oUnallocatedCreditsForClient)
                        Next
                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetUserGroups executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If IsNothing(v_iInsuranceFileKey) Then
                        sbLogMessage.AppendLine("Insurance File Key : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Insurance File Key : " & v_iInsuranceFileKey.ToString & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output : " & vbCrLf)
                    sbLogMessage.AppendLine(oBalancesAndUnallocatedCredits.Print().Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally

                oGetBalancesAndUnallocatedCreditsRequest = Nothing
                oGetBalancesAndUnallocatedCreditsResponse = Nothing
            End Try

            Return oBalancesAndUnallocatedCredits

        End SyncLock
    End Function

    Public Overrides Function GetBankAccounts(Optional ByVal v_iBankAccountKey As Integer = 0,
                                      Optional ByVal v_sBranchCode As String = Nothing) As AccountDetailsCollection
        SyncLock oLock

            Dim oGetBankAccountsRequest As GetBankAccountsQuery
            Dim oGetBankAccountsResponse As GetBankAccountsQueryResponse
            Dim oAccountSearchResult As AccountDetailsCollection
            Dim oNewAccountSearchResult As AccountDetails
            Dim sbLogMessage As StringBuilder

            Try

                oGetBankAccountsRequest = New GetBankAccountsQuery
                oGetBankAccountsResponse = New GetBankAccountsQueryResponse
                oAccountSearchResult = New AccountDetailsCollection
                sbLogMessage = New StringBuilder

                With oGetBankAccountsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    If v_iBankAccountKey > 0 Then
                        .BankAccountKey = v_iBankAccountKey
                        .BankAccountKeySpecified = True
                    Else
                        .BankAccountKeySpecified = False
                    End If
                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetBankAccounts, oGetBankAccountsRequest)
                    oGetBankAccountsResponse = ApiClient.DeserializeJson(Of GetBankAccountsQueryResponse)(result)
                End Using

                With oGetBankAccountsResponse
                    If .BankAccounts IsNot Nothing AndAlso .BankAccounts.Count > 0 Then

                        For Each oFindAccount As BaseGetBankAccountsResponseTypeRow In .BankAccounts

                            oNewAccountSearchResult = New AccountDetails()

                            With oNewAccountSearchResult
                                .BankAccountName = oFindAccount.BankAccountName
                                .AccountKey = oFindAccount.BankAccountKey
                                .Account = oFindAccount.BankAccountNumber
                                .Code = oFindAccount.Code
                                .CurrencyCode = oFindAccount.CurrencyCode
                                .CurrencyId = oFindAccount.CurrencyKey
                                .Description = oFindAccount.Description
                                .EffectiveDate = oFindAccount.EffectiveDate

                            End With

                            oAccountSearchResult.Add(oNewAccountSearchResult)

                        Next
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("FindAccounts executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    'sbLogMessage.AppendLine("v_oAccountSearchCriteria = " & v_oAccountSearchCriteria.Print.Replace("<br/>", vbCrLf))
                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally

                oGetBankAccountsRequest = Nothing
                oGetBankAccountsResponse = Nothing
            End Try


            Return oAccountSearchResult

        End SyncLock
    End Function

    Public Overrides Function GetCurrencyExchangeRates(ByVal v_oCurrency As Currency,
                                         Optional ByVal v_sBranchCode As String = Nothing,
                                         Optional ByVal v_iClaimKey As Integer = Nothing) As Currency
        SyncLock oLock

            Dim oGetCurrencyExchangeRatesRequest As GetCurrencyExchangeRatesQuery
            Dim oGetCurrencyExchangeRatesResponse As GetCurrencyExchangeRatesQueryResponse
            Dim oCurrencyExchangeRateType As Currency
            Dim sbLogMessage As StringBuilder

            Try

                oGetCurrencyExchangeRatesRequest = New GetCurrencyExchangeRatesQuery
                oGetCurrencyExchangeRatesResponse = New GetCurrencyExchangeRatesQueryResponse
                oCurrencyExchangeRateType = New Currency
                sbLogMessage = New StringBuilder

                With oGetCurrencyExchangeRatesRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .AccountCode = v_oCurrency.AccountCode
                    .TransactionCurrencyCode = v_oCurrency.TransactionCurrencyCode

                    If v_oCurrency.CurrencyAmountUnRounded > 0 Then
                        .CurrencyAmountUnRounded = v_oCurrency.CurrencyAmountUnRounded
                        .CurrencyAmountUnRoundedSpecified = True
                    Else
                        .CurrencyAmountUnRoundedSpecified = False
                    End If
                    .ClaimKey = v_iClaimKey 'Issue126
                    .Mode = v_oCurrency.Mode
                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetCurrencyExchangeRates, oGetCurrencyExchangeRatesRequest)
                    oGetCurrencyExchangeRatesResponse = ApiClient.DeserializeJson(Of GetCurrencyExchangeRatesQueryResponse)(result)
                End Using

                With oGetCurrencyExchangeRatesResponse

                    If .CurrencyRates IsNot Nothing Then
                        v_oCurrency.TransactionCurrencyKey = .CurrencyRates.TransactionCurrencyKey
                        v_oCurrency.TransactionCurrencyDesc = .CurrencyRates.TransactionCurrencyDesc
                        v_oCurrency.TransactionCurrencyRate = .CurrencyRates.TransactionCurrrencyRate
                        v_oCurrency.ExchangeRateOverrideReasonKey = .CurrencyRates.ExchangeRateOverrideReasonKey
                        v_oCurrency.BaseCurrencyKey = .CurrencyRates.BaseCurrencyKey
                        v_oCurrency.BaseCurrencyDesc = .CurrencyRates.BaseCurrencyDesc
                        v_oCurrency.BaseCurrencyRate = .CurrencyRates.BaseCurrencyRate
                        v_oCurrency.BaseCurrencyDate = .CurrencyRates.BaseCurrencyDate
                        v_oCurrency.AccountCurrencyKey = .CurrencyRates.AccountCurrencyKey
                        v_oCurrency.AccountCurrencyDesc = .CurrencyRates.AccountCurrencyDesc
                        v_oCurrency.AccountCurrencyRate = .CurrencyRates.AccountCurrencyRate
                        v_oCurrency.AccountCurrencyDate = .CurrencyRates.AccountCurrencyDate
                        v_oCurrency.SystemCurrrencyRate = .CurrencyRates.SystemCurrrencyRate
                        v_oCurrency.SystemCurrencyDate = .CurrencyRates.SystemCurrencyDate
                        v_oCurrency.SystemCurrencyKey = .CurrencyRates.SystemCurrencyKey
                    End If

                    v_oCurrency.BaseAmount = .BaseAmount
                    v_oCurrency.BaseAmountUnrounded = .BaseAmountUnrounded
                    v_oCurrency.AccountAmount = .AccountAmount
                    v_oCurrency.AccountAmountUnrounded = .AccountAmountUnrounded
                    v_oCurrency.SystemAmount = .SystemAmount
                    v_oCurrency.SystemAmountUnrounded = .SystemAmountUnrounded

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetCurrencyExchangeRates executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & v_oCurrency.ToString & " results" & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally

                oGetCurrencyExchangeRatesRequest = Nothing
                oGetCurrencyExchangeRatesResponse = Nothing
            End Try


            Return v_oCurrency

        End SyncLock
    End Function

    Public Overrides Function GetInsurerPayments(ByVal v_oAccountDetails As AccountDetails,
                                  Optional ByVal v_sBranchCode As String = Nothing) As AccountDetailsCollection

        SyncLock oLock

            Dim oGetInsurerPaymentsRequest As GetInsurerPaymentsQuery
            Dim oGetInsurerPaymentsResponse As GetInsurerPaymentsQueryResponse
            Dim oAccountDetailsCollection As AccountDetailsCollection
            Dim oAccountDetails As AccountDetails
            Dim sbLogMessage As StringBuilder

            Try

                oGetInsurerPaymentsRequest = New GetInsurerPaymentsQuery
                oGetInsurerPaymentsResponse = New GetInsurerPaymentsQueryResponse
                oAccountDetailsCollection = New AccountDetailsCollection
                sbLogMessage = New StringBuilder

                With oGetInsurerPaymentsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .AccountKey = v_oAccountDetails.AccountKey
                    If v_oAccountDetails.DateTo <> Date.MinValue Then
                        .DateTo = v_oAccountDetails.DateTo
                        ' .DateToSpecified = True sandeep
                    Else
                        ' .DateToSpecified = False
                    End If


                    .DateByTransaction = v_oAccountDetails.DateByTransaction
                    ' .DateByTransactionSpecified = True sandeep

                    Select Case v_oAccountDetails.MarkedStatus
                        Case NexusProvider.InsurerPaymentsMarkedStatus.Any
                            .MarkedStatus = InsurerPaymentsMarkedStatus.Any
                           ' .MarkedStatusSpecified = True
                        Case NexusProvider.InsurerPaymentsMarkedStatus.No
                            .MarkedStatus = InsurerPaymentsMarkedStatus.No
                           ' .MarkedStatusSpecified = True
                        Case NexusProvider.InsurerPaymentsMarkedStatus.Yes
                            .MarkedStatus = InsurerPaymentsMarkedStatus.Yes
                            ' .MarkedStatusSpecified = True
                    End Select
                    .Month = v_oAccountDetails.Month
                    ' .MonthSpecified = True
                    .AlternateReference = v_oAccountDetails.AlternateReference
                    .PolicyNumber = v_oAccountDetails.PolicyNumber
                    .InsurerPaymentBranchCode = v_oAccountDetails.InsurerPaymentBranchCode
                    'WPR48
                    .CurrencyCode = v_oAccountDetails.CurrencyCode
                    If (v_oAccountDetails.PeriodName IsNot Nothing) Then
                        .PeriodName = v_oAccountDetails.PeriodName
                    Else
                        .PeriodName = ""
                    End If
                    If (.PeriodName.Trim() = "") Then
                        .PeriodName = Nothing
                    End If
                    .YearName = v_oAccountDetails.YearName
                    If v_oAccountDetails.DateFrom <> Date.MinValue Then
                        .DateFrom = v_oAccountDetails.DateFrom
                        '  .DateFromSpecified = True
                    Else
                        ' .DateFromSpecified = False
                    End If
                    .Reference = v_oAccountDetails.Reference
                    .GrossAgent = v_oAccountDetails.GrossAgent
                    .ExcludePendingAuth = v_oAccountDetails.ExcludePendingAuth
                    .OnlyPendingAuth = v_oAccountDetails.OnlyPendingAuth
                    .MediaType = v_oAccountDetails.MediaType
                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetInsurerPayments, oGetInsurerPaymentsRequest)
                    oGetInsurerPaymentsResponse = ApiClient.DeserializeJson(Of GetInsurerPaymentsQueryResponse)(result)
                End Using

                With oGetInsurerPaymentsResponse
                    If .InsurerPayments IsNot Nothing AndAlso .InsurerPayments.Count > 0 Then

                        For Each oAccountDetailsRow As BaseGetInsurerPaymentsResponseTypeRow In .InsurerPayments
                            oAccountDetails = New AccountDetails()

                            oAccountDetails.DocumentId = oAccountDetailsRow.DocumentId
                            oAccountDetails.DocumentRef = oAccountDetailsRow.DocumentRef
                            oAccountDetails.InsurerRef = oAccountDetailsRow.InsurerRef
                            oAccountDetails.FullyPaidAmount = oAccountDetailsRow.FullyPaidAmount
                            oAccountDetails.ClientOutstanding = oAccountDetailsRow.ClientOutstanding
                            oAccountDetails.ConsolidateBinder = oAccountDetailsRow.ConsolidateBinder
                            oAccountDetails.ShortName = oAccountDetailsRow.ShortName
                            oAccountDetails.ResolvedName = oAccountDetailsRow.ResolvedName
                            oAccountDetails.TransdetailId = oAccountDetailsRow.TransdetailId
                            oAccountDetails.CompanyId = oAccountDetailsRow.CompanyId
                            oAccountDetails.AccountingDate = oAccountDetailsRow.AccountingDate
                            oAccountDetails.CurrencyAmount = oAccountDetailsRow.CurrencyAmount
                            oAccountDetails.CurrencyId = oAccountDetailsRow.CurrencyId
                            oAccountDetails.CurrencyCode = oAccountDetailsRow.CurrencyCode
                            oAccountDetails.CurrencyBaseRate = oAccountDetailsRow.CurrencyBaseRate
                            oAccountDetails.MarkedAmount = oAccountDetailsRow.MarkedAmount
                            oAccountDetails.PaidAmount = oAccountDetailsRow.PaidAmount
                            oAccountDetails.Spare = oAccountDetailsRow.Spare
                            oAccountDetails.PeriodName = oAccountDetailsRow.PeriodName
                            oAccountDetails.Month = oAccountDetailsRow.Month
                            oAccountDetails.AccountCurrencyId = oAccountDetailsRow.AccountCurrencyId
                            oAccountDetails.AccountCurrencyCode = oAccountDetailsRow.AccountCurrencyCode
                            oAccountDetails.AccountBaseRate = oAccountDetailsRow.AccountBaseRate
                            oAccountDetails.FullyPaidAccountAmount = oAccountDetailsRow.FullyPaidAccountAmount
                            oAccountDetails.ClientOutstandingAccountAmount = oAccountDetailsRow.ClientOutstandingAccountAmount
                            oAccountDetails.AccountAmount = oAccountDetailsRow.AccountAmount
                            oAccountDetails.MarkedAccountAmount = oAccountDetailsRow.MarkedAccountAmount
                            oAccountDetails.PaidAccountAmount = oAccountDetailsRow.PaidAccountAmount
                            oAccountDetails.AlternateReference = oAccountDetailsRow.AlternateReference
                            oAccountDetails.EffectiveDate = oAccountDetailsRow.EffectiveDate
                            oAccountDetails.BranchCode = oAccountDetailsRow.BranchCode
                            'WPR48
                            oAccountDetails.YearName = oAccountDetailsRow.YearName
                            oAccountDetails.DueDate = oAccountDetailsRow.DueDate
                            oAccountDetailsCollection.Add(oAccountDetails)
                            oAccountDetails = Nothing

                        Next

                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetInsurerPayments executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    'sbLogMessage.AppendLine("v_iAddressKey = " & v_iAddressKey.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally

                oGetInsurerPaymentsRequest = Nothing
                oGetInsurerPaymentsResponse = Nothing
            End Try



            Return oAccountDetailsCollection

        End SyncLock
    End Function

    '''' <summary>
    '''' (WPR48)This method is used to retrieve all periods from the period table, with an optional input parameter to indicate whether to retrieve an indicator that all payments for this period have been allocated or not
    '''' </summary>
    '''' <returns></returns>
    '''' <remarks></remarks>
    Public Overrides Function GetPeriod(Optional ByVal v_bGetPaymentsAllocated As Boolean = False,
                                   Optional ByVal v_sBranchCode As String = Nothing) As PeriodCollection

        SyncLock oLock

            Dim oGetPeriodsRequestType As GetPeriodQuery
            Dim oGetPeriodsResponseType As GetPeriodQueryResponse
            Dim oPeriodCollection As PeriodCollection = Nothing
            Dim oAllocationPeriod As AllocationPeriod
            Try

                oGetPeriodsRequestType = New GetPeriodQuery
                oGetPeriodsResponseType = New GetPeriodQueryResponse

                With oGetPeriodsRequestType
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    'if the passed parameter v_sBranchCode is empty 
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .GetPaymentsAllocated = v_bGetPaymentsAllocated

                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetPeriods, oGetPeriodsRequestType)
                    oGetPeriodsResponseType = ApiClient.DeserializeJson(Of GetPeriodQueryResponse)(result)
                End Using

                With oGetPeriodsResponseType
                    oPeriodCollection = New PeriodCollection

                    If .Periods IsNot Nothing AndAlso .Periods.Count > 0 Then
                        For Each oPeriod As BaseGetPeriodResponseTypeRow In .Periods
                            oAllocationPeriod = New AllocationPeriod()
                            With oAllocationPeriod
                                .PeriodID = oPeriod.PeriodID
                                .PeriodName = oPeriod.PeriodName
                                .YearName = oPeriod.YearName
                            End With
                            oPeriodCollection.Add(oAllocationPeriod)
                        Next
                    End If

                End With

            Catch ex As Exception
                Throw
            Finally

                oGetPeriodsRequestType = Nothing
                oGetPeriodsResponseType = Nothing

            End Try

            Return oPeriodCollection
        End SyncLock
    End Function

    Public Overrides Function GetTransactionDetails(ByVal v_iAccountKey As Integer,
                                  ByVal v_oAllocationDetailsCollections As AllocationDetailsCollections,
                                  Optional ByVal sShortCode As String = Nothing,
                                  Optional ByVal dtToDate As String = Nothing,
                                  Optional ByVal sInsuranceRef As String = Nothing,
                                  Optional ByVal bIsNewPF As Boolean = False,
                                  Optional ByVal v_sBranchCode As String = Nothing) As AllocationDetailsCollections

        SyncLock oLock

            Dim oGetTransactionDetailsRequest As GetTransactionDetailsQuery
            Dim oGetTransactionDetailsResponse As GetTransactionDetailsQueryResponse
            Dim oAllocationDetails As BaseGetTransactionDetailsRequestTypeRow
            Dim i As Integer
            Dim oTransactionCollection As AllocationDetailsCollections
            Dim oTransaction As AllocationDetails
            Dim sbLogMessage As StringBuilder

            Try

                oGetTransactionDetailsRequest = New GetTransactionDetailsQuery
                oGetTransactionDetailsResponse = New GetTransactionDetailsQueryResponse
                oTransactionCollection = New AllocationDetailsCollections
                sbLogMessage = New StringBuilder

                With oGetTransactionDetailsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    oGetTransactionDetailsRequest.Allocation = New List(Of BaseGetTransactionDetailsRequestTypeRow)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    If v_iAccountKey > 0 Then
                        .AccountKey = v_iAccountKey
                        .AccountKeySpecified = True
                    End If

                    If sShortCode IsNot Nothing Then
                        .ShortCode = sShortCode
                        .AccountKeySpecified = False
                    End If

                    If dtToDate IsNot Nothing Then
                        .AccountingDate = dtToDate
                    End If

                    .IsNewPF = bIsNewPF
                    If bIsNewPF Then
                        .IsOutstandingOnly = True
                    End If
                    If sInsuranceRef IsNot Nothing Then
                        .InsuranceRef = sInsuranceRef
                    End If

                    '.Allocation = New List(Of BaseGetTransactionDetailsRequestTypeRow)
                    If v_oAllocationDetailsCollections IsNot Nothing AndAlso v_oAllocationDetailsCollections.Count > 0 Then
                        For i = 0 To (v_oAllocationDetailsCollections.Count - 1)
                            oAllocationDetails = New BaseGetTransactionDetailsRequestTypeRow
                            oAllocationDetails.AllocationTransDetailKey = v_oAllocationDetailsCollections(i).TransdetailKey
                            .Allocation.Add(oAllocationDetails)
                        Next
                    End If
                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Post(ApiMethods.GetTransactionDetails, oGetTransactionDetailsRequest)
                    oGetTransactionDetailsResponse = ApiClient.DeserializeJson(Of GetTransactionDetailsQueryResponse)(result)
                End Using

                With oGetTransactionDetailsResponse
                    oTransaction = New AllocationDetails()
                    If .GetTransactionDetailsResponse.Transactions IsNot Nothing AndAlso .GetTransactionDetailsResponse.Transactions.Count > 0 Then
                        For Each oTransactionRow As BaseGetTransactionDetailsResponseTypeRow In .GetTransactionDetailsResponse.Transactions
                            oTransaction = New AllocationDetails()
                            With oTransactionRow
                                oTransaction.TransdetailKey = .TransDetailKey
                                oTransaction.DocRef = .DocRef
                                oTransaction.AltRef = .AltRef
                                oTransaction.EffectiveDate = .EffectiveDate
                                oTransaction.TransDate = .TransDate
                                oTransaction.MediaType = .MediaType
                                oTransaction.Amount = .Amount
                                oTransaction.OutStandingamount = .OutstandingAmount
                                oTransaction.MediaRef = .MediaRef
                                oTransaction.AccountKey = .Accountkey
                                oTransaction.AccountCode = .AccountCode
                                oTransaction.AllocationTimeStamp = .AllocationTimeStamp
                                oTransaction.Currency = .Currency
                                oTransaction.CurrencyCode = .CurrencyCode
                                oTransaction.CurrencyDiff = .CurrencyDiff
                                oTransaction.TaxBand = .TaxBand
                                oTransaction.TransactionCurrency = .TransactionCurrency
                                oTransaction.TransactionCurrencyCode = .TransactionCurrencyCode
                                oTransaction.TransactionCurrencyAmount = .TransactionCurrenciesAmount
                                oTransaction.InsuranceFileKey = .InsuranceFileCnt
                                oTransaction.InsuranceRef = .InsuranceRef
                                oTransaction.DocumentTypeID = .DocTypeID
                                oTransaction.Spare = .Spare
                                oTransaction.DocType = .DocType
                                oTransaction.Period = .PeriodName
                                oTransaction.PrimarySettled = .PrimarySettled
                                oTransaction.DocGroup = .DoctypeGroup
                                oTransaction.SourceID = .SourceID
                            End With
                            oTransactionCollection.Add(oTransaction)
                        Next
                    End If
                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetTransactionDetails executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    'sbLogMessage.AppendLine("v_iAddressKey = " & v_iAddressKey.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally

                oGetTransactionDetailsRequest = Nothing
                oGetTransactionDetailsResponse = Nothing
            End Try


            Return oTransactionCollection
        End SyncLock
    End Function


    ''' <summary>
    ''' Get Claim Recovery (CLR) transactions eligible for instalment plan creation.
    ''' </summary>
    Public Overrides Function GetClaimRecoveryTransactions(
                                  Optional ByVal sShortCode As String = Nothing,
                                  Optional ByVal sClaimNumber As String = Nothing,
                                  Optional ByVal v_sBranchCode As String = Nothing) As FinancePlanTransactionsCollection

        SyncLock oLock
            Dim oTransactionsCollection As New FinancePlanTransactionsCollection
            Dim oTransaction As FinancePlanTransactions

            Try
                Dim oRequest As New GetClaimRecoveryTransactionsQuery
                With oRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        If String.IsNullOrEmpty(sBranchCode) Then
                            .BranchCode = sDefaultBranchCode
                        Else
                            .BranchCode = sBranchCode
                        End If
                    Else
                        .BranchCode = v_sBranchCode
                    End If
                    .ShortCode = sShortCode
                    .ClaimNumber = sClaimNumber
                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetClaimRecoveryTransactions, oRequest)
                    Dim oResponse = ApiClient.DeserializeJson(Of GetClaimRecoveryTransactionsQueryResponse)(result)

                    If oResponse IsNot Nothing AndAlso oResponse.Transactions IsNot Nothing Then
                        For Each oItem As ClaimRecoveryTransactionResponseItem In oResponse.Transactions
                            oTransaction = New FinancePlanTransactions()
                            oTransaction.PFTransactionKey = oItem.TransDetailKey
                            oTransaction.DocRef = oItem.DocumentReference
                            oTransaction.Amount = oItem.TransactionAmount
                            oTransaction.OutStandingamount = oItem.OutstandingAmount
                            oTransaction.TransDate = oItem.TransactionDate
                            oTransaction.TransactionCurrencyCode = oItem.CurrencyCode
                            oTransaction.InsuranceFileKey = oItem.InsuranceFileKey
                            oTransaction.DocumentTypeId = CInt(oItem.DocumentType)
                            oTransaction.DocType = oItem.DocumentType
                            oTransaction.InsuranceRefIndex = oItem.ClaimNumber
                            oTransactionsCollection.Add(oTransaction)
                        Next
                    End If
                End Using

            Catch ex As Exception
                Throw
            End Try

            Return oTransactionsCollection
        End SyncLock
    End Function


    Public Overrides Sub MarkUnmarkTransaction(ByRef v_oMarkUnmarkTransaction As MarkUnmarkTransaction,
                                 Optional ByVal v_sBranchCode As String = Nothing)


        SyncLock oLock


            Dim oMarkUnmarkTransactionRequest As MarkUnmarkTransactionCommand
            Dim oMarkUnmarkTransactionResponse As MarkUnmarkTransactionCommandResponse
            Dim sbLogMessage As StringBuilder

            Try

                oMarkUnmarkTransactionRequest = New MarkUnmarkTransactionCommand
                oMarkUnmarkTransactionResponse = New MarkUnmarkTransactionCommandResponse
                sbLogMessage = New StringBuilder

                With oMarkUnmarkTransactionRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .TransactionKey = v_oMarkUnmarkTransaction.TransactionKey
                    .CurrencyCode = v_oMarkUnmarkTransaction.CurrencyCode
                    .PaymentAmount = v_oMarkUnmarkTransaction.PaymentAmount

                    Select Case v_oMarkUnmarkTransaction.MarkStatus
                        Case MarkStatusType.Mark
                            .MarkStatus = MarkStatusType.Mark
                        Case MarkStatusType.UnMark
                            .MarkStatus = MarkStatusType.UnMark
                    End Select

                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Post(ApiMethods.MarkUnmarkTransaction, oMarkUnmarkTransactionRequest)
                    oMarkUnmarkTransactionResponse = ApiClient.DeserializeJson(Of MarkUnmarkTransactionCommandResponse)(result)
                End Using

                With oMarkUnmarkTransactionResponse.GetMarkUnmarkTransaction
                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)

                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("MarkUnmarkTransaction executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    'sbLogMessage.AppendLine("v_iAddressKey = " & v_iAddressKey.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    'sbLogMessage.AppendLine("Returned " & oAddress.Print.Replace("<br />", vbCrLf) & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally

            End Try


        End SyncLock
    End Sub

    Public Overrides Sub PostDocument(ByVal v_oDocumentTypeType As DocumentTypeTypes,
                                           ByVal v_oTransactions As TransactionCollection,
                                           ByVal v_sComment As String,
                                           ByVal v_sDocumentTypeCode As String,
                                           ByRef v_sDocumentReference As String,
                                           ByVal v_iSAMStagingPolicyKey As Integer,
                                           Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock

            Dim oPostDocumentRequest As PostDocumentCommand ' Request Type
            Dim oPostDocumentResponse As PostDocumentCommandResponse  ' Response Type
            Dim oTransactionType(v_oTransactions.Count) As BaseTransactionType
            Dim i As Integer
            Dim sbLogMessage As StringBuilder

            Try

                oPostDocumentRequest = New PostDocumentCommand
                oPostDocumentResponse = New PostDocumentCommandResponse
                sbLogMessage = New StringBuilder

                With oPostDocumentRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    If Not IsNothing(v_oDocumentTypeType) Then
                        .DocumentType = DocumentTypeType.JN
                    Else
                        Throw New ArgumentNullException("v_oDocumentTypeType")
                    End If
                    For i = 0 To v_oTransactions.Count - 1
                        oTransactionType(i) = New BaseTransactionType
                        oTransactionType(i).AccountCode = v_oTransactions(i).AccountCode
                        oTransactionType(i).Amount = v_oTransactions(i).Amount
                        oTransactionType(i).UnderwritingYearCode = v_oTransactions(i).UnderwritingYearCode
                        oTransactionType(i).Comment = v_oTransactions(i).Comment
                        oTransactionType(i).TransactionDate = v_oTransactions(i).TransactionDate
                        oTransactionType(i).PeriodID = v_oTransactions(i).PeriodID
                        oTransactionType(i).Username = v_oTransactions(i).UserName
                        oTransactionType(i).Reference = v_oTransactions(i).Reference

                    Next
                    .Comment = v_sComment
                    .DocumentTypeCode = v_sDocumentTypeCode
                    .DocumentReference = v_sDocumentReference
                    .SAMStagingPolicyKey = v_iSAMStagingPolicyKey

                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Post(ApiMethods.PostDocument, oPostDocumentRequest)
                    oPostDocumentResponse = ApiClient.DeserializeJson(Of PostDocumentCommandResponse)(result)
                End Using

                With oPostDocumentResponse
                    v_sDocumentReference = .DocumentRef
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("PostDocument executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_oTransactions = " & v_oTransactions.Print.Replace("<br />", vbCrLf))
                    sbLogMessage.AppendLine("v_sComment = " & v_sComment.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_sDocumentTypeCode = " & v_sDocumentTypeCode.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_sDocumentReference = " & v_sDocumentReference.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_iSAMStagingPolicyKey = " & v_iSAMStagingPolicyKey.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally

                oPostDocumentRequest = Nothing
                oPostDocumentResponse = Nothing
            End Try


        End SyncLock

    End Sub

    ''' <summary>
    ''' This method is used to reverse the transaction allocation.
    ''' </summary>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_bIgnoreWarnings" ></param>
    ''' <param name="v_iAllocationKey" ></param>
    ''' <param name="v_iTransDetailKey" ></param>
    ''' <returns>WarningCollection</returns>
    ''' <remarks></remarks>
    Public Overrides Function ReverseAllocation(ByVal v_iTransDetailKey As Integer,
                                                ByVal v_iAllocationKey As Integer,
                                                Optional ByVal v_bIgnoreWarnings As Boolean = True,
                                                Optional ByVal v_sBranchCode As String = Nothing) As String

        SyncLock oLock

            Dim oReverseAllocationRequest As ReverseAllocationCommand
            Dim oReverseAllocationResponse As ReverseAllocationCommandResponse
            Dim sWarning As String = String.Empty
            Try

                oReverseAllocationRequest = New ReverseAllocationCommand
                oReverseAllocationResponse = New ReverseAllocationCommandResponse

                With oReverseAllocationRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    If v_iAllocationKey <> 0 Then
                        .AllocationKey = v_iAllocationKey
                    Else
                        Throw New ArgumentException("AllocationKey")
                    End If
                    If v_iTransDetailKey <> 0 Then
                        .TransDetailKey = v_iTransDetailKey
                    Else
                        Throw New ArgumentException("TransDetailKey")
                    End If
                    .IgnoreWarnings = v_bIgnoreWarnings

                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Post(ApiMethods.ReverseAllocation, oReverseAllocationRequest)
                    oReverseAllocationResponse = ApiClient.DeserializeJson(Of ReverseAllocationCommandResponse)(result)
                End Using

                With oReverseAllocationResponse.ReverseAllocationResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        If Not (String.IsNullOrEmpty(.Warnings)) Then
                            sWarning = CStr(.Warnings)
                        End If
                    End If
                End With

            Catch ex As Exception
                Throw
            Finally

                oReverseAllocationRequest = Nothing
                oReverseAllocationResponse = Nothing

            End Try
            Return sWarning
        End SyncLock
    End Function

    Public Overrides Function UpdateAllocation(ByVal v_oAllocationDetails As AllocationDetails,
                                               Optional ByVal v_sBranchCode As String = Nothing) As Boolean
        SyncLock oLock

            Dim oUpdateAllocationRequest As UpdateAllocationCommand
            Dim oUpdateAllocationResponse As UpdateAllocationCommandResponse
            Dim oAllocation As AllocationCollection
            Dim iAllocation As Integer = 0
            Dim oAllocationDetails As BaseUpdateAllocationRequestTypeAllocation
            Dim sbLogMessage As StringBuilder

            Try

                oUpdateAllocationRequest = New UpdateAllocationCommand
                oUpdateAllocationResponse = New UpdateAllocationCommandResponse
                oAllocation = New AllocationCollection
                sbLogMessage = New StringBuilder


                With oUpdateAllocationRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .AccountKey = v_oAllocationDetails.AccountKey
                    .TransdetailKey = v_oAllocationDetails.TransdetailKey
                    .Amount = v_oAllocationDetails.Amount
                    .CashListItemKey = v_oAllocationDetails.CashListItemKey


                    If v_oAllocationDetails.Allocation.Count > 0 Then
                        .Allocation = New List(Of BaseUpdateAllocationRequestTypeAllocation)
                        For i As Integer = 0 To v_oAllocationDetails.Allocation.Count - 1
                            oAllocationDetails = New BaseUpdateAllocationRequestTypeAllocation
                            'oAllocationDetails.AllocationTransdetailKey = v_oAllocationDetails.Allocation(i).AllocationTransdetailKey
                            'oAllocationDetails.AllocationAmount = v_oAllocationDetails.Allocation(i).AllocationAmount
                            'oAllocationDetails.AllocationTimeStamp = v_oAllocationDetails.Allocation(i).AllocationTimeStamp

                            oAllocationDetails.AllocationTransdetailKey = v_oAllocationDetails.Allocation.Item(i).AllocationTransdetailKey
                            oAllocationDetails.AllocationAmount = v_oAllocationDetails.Allocation.Item(i).AllocationAmount
                            oAllocationDetails.AllocationTimeStamp = v_oAllocationDetails.Allocation.Item(i).AllocationTimeStamp
                            oAllocationDetails.AllocationAmountSpecified = True
                            .Allocation.Add(oAllocationDetails)
                        Next

                    End If
                    'For Each oNewAllocation As BaseUpdateAllocationRequestTypeAllocation In oAllocation
                    '    .Allocation.SetValue(oNewAllocation, iAllocation)
                    '    iAllocation = iAllocation + 1
                    'Next
                    If v_oAllocationDetails.WriteOffAmount <> 0 Then
                        .WriteOffAmountSpecified = True
                        .WriteOffAmount = v_oAllocationDetails.WriteOffAmount
                    End If

                    If v_oAllocationDetails.WriteOffReason <> 0 Then
                        .WriteOffReasonSpecified = True
                        .WriteOffReason = v_oAllocationDetails.WriteOffReason
                    End If

                    If v_oAllocationDetails.CurrencyDiff <> 0 Then
                        .CurrencyDiffSpecified = True
                        .CurrencyDiff = v_oAllocationDetails.CurrencyDiff
                    End If

                    Using trace As New Tracer(Category.Trace)
                        ApiClient._tokenModel = GetApiTokendetails()
                        Dim result As String = ApiClient.Put(ApiMethods.UpdateAllocation, oUpdateAllocationRequest)
                        oUpdateAllocationResponse = ApiClient.DeserializeJson(Of UpdateAllocationCommandResponse)(result)
                    End Using

                    'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                End With
                With oUpdateAllocationResponse.UpdateAllocationResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        v_oAllocationDetails.AllocationStatus = .AllocationStatus
                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("UpadteAllocation executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    ' sbLogMessage.AppendLine("v_oAllocationDetails = " & v_oAllocationDetails.Print.Replace("<br/>", vbCrLf))
                    sbLogMessage.AppendLine("v_oAllocationDetails = " & v_oAllocationDetails.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally

                oUpdateAllocationRequest = Nothing
                oUpdateAllocationResponse = Nothing
            End Try


            'Returns Boolean(Allocation Status)
            Return v_oAllocationDetails.AllocationStatus
        End SyncLock
    End Function

    ''' <summary>
    ''' This method  will add the BankGuarantee Details
    ''' </summary>
    ''' <param name="r_oBankCollection"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns>Bank Guarantee Collection</returns>
    ''' <remarks></remarks>
    Public Overrides Function AddBankGuarantee(ByVal r_oBankCollection As BankGuaranteeCollection,
                  Optional ByVal v_sBranchCode As String = Nothing) As BankGuaranteeCollection
        SyncLock oLock



            Return New BankGuaranteeCollection
        End SyncLock
    End Function

    Public Overrides Function AddCashDeposit(ByVal oCDColl As NexusProvider.CashDepositCollection,
                                             Optional ByVal v_sBranchCode As String = Nothing) As CashDepositCollection

        SyncLock oLock


            Return New CashDepositCollection

        End SyncLock
    End Function


    ''' <summary>
    ''' To find bank details depending upon the search criteria.
    ''' </summary>
    ''' <param name="v_oBankSearchCriteria"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function FindBank(ByVal v_oBankSearchCriteria As BankSearchCriteria,
                                       Optional ByVal v_sBranchCode As String = Nothing) As BankCollection
        SyncLock oLock

            Dim oFindBankRequest As FindBankCommand
            Dim oFindBankResponse As FindBankCommandResponse
            Dim oBankCollection As BankCollection = New BankCollection()
            Dim oBank As Bank = Nothing
            Dim sbLogMessage As StringBuilder

            Try

                oFindBankRequest = New FindBankCommand
                oFindBankResponse = New FindBankCommandResponse
                sbLogMessage = New StringBuilder

                With oFindBankRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .BankName = v_oBankSearchCriteria.BankName
                    .ShortCode = v_oBankSearchCriteria.ShortCode

                    If v_oBankSearchCriteria.MaxRowsToFetch > 0 Then
                        .MaxRowsToFetch = v_oBankSearchCriteria.MaxRowsToFetch
                        .MaxRowsToFetchSpecified = True
                    Else
                        .MaxRowsToFetchSpecified = False
                    End If

                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Post(ApiMethods.FindBank, oFindBankRequest)
                    oFindBankResponse = ApiClient.DeserializeJson(Of FindBankCommandResponse)(result)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                With oFindBankResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)

                    End If

                    If .Bank IsNot Nothing AndAlso .Bank.Count > 0 Then

                        For Each oBaseBank As BaseFindBankResponseTypeRow In .Bank
                            oBank = New Bank
                            oBank.BankKey = oBaseBank.BankKey
                            oBank.BankName = oBaseBank.BankName
                            oBank.BranchCode = oBaseBank.BranchCode
                            oBank.BankAddress = oBaseBank.BankAddress
                            oBank.Code = oBaseBank.Code
                            oBank.HeadOffice = oBaseBank.HeadOffice
                            oBankCollection.Add(oBank)
                        Next

                        If oBankCollection.Count = 0 Then
                            oBankCollection = Nothing
                        End If
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("FindBank executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_oBankSearchCriteria = " & v_oBankSearchCriteria.Print())


                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally

                oFindBankRequest = Nothing
                oFindBankResponse = Nothing
            End Try


            Return oBankCollection

        End SyncLock

    End Function


    Public Overrides Function FindBankGuarantee(ByVal v_oBankGuarantee As BankGuarantee,
                                     Optional ByVal v_sBranchCode As String = Nothing) As BankGuaranteeCollection


        SyncLock oLock




            Return New BankGuaranteeCollection

        End SyncLock
    End Function

    Public Overrides Function FindCashDeposit(ByVal v_sPartyCode As String,
                                              ByVal v_sCashDepositRef As String,
                                              ByVal v_sBankCode As String,
                                              Optional ByVal v_iMaxRowsToFetch As Integer = 0,
                                              Optional ByVal v_sBranchCode As String = Nothing) As CashDepositCollection

        SyncLock oLock

            Return New CashDepositCollection

        End SyncLock
    End Function
    Public Overrides Function GetLinkedCashDeposits(ByVal v_sPartyCode As String,
                              Optional ByVal v_sBranchCode As String = Nothing) As CashDepositCollection
        SyncLock oLock

            Return New CashDepositCollection

        End SyncLock
    End Function

    Public Overrides Function GetNextCashDepositRef(ByVal v_sPartyCode As String,
                                                    Optional ByVal v_sBranchCode As String = Nothing) As CashDeposit

        SyncLock oLock

            Return New CashDeposit
        End SyncLock

    End Function

    Public Overrides Function GetPaymentCashListDetails(ByVal v_iCashListKey As Integer,
                                     Optional ByVal v_sBranchCode As String = Nothing) As PaymentCashList

        SyncLock oLock


            Dim oGetPaymentCashListDetailsRequest As GetPaymentCashListDetailsQuery
            Dim oGetPaymentCashListDetailsResponse As GetPaymentCashListDetailsQueryResponse
            Dim oPaymentCashList As PaymentCashList
            Dim oPaymentCashListItemType As PaymentCashListItemType
            Dim sbLogMessage As StringBuilder

            Try

                oGetPaymentCashListDetailsRequest = New GetPaymentCashListDetailsQuery
                oGetPaymentCashListDetailsResponse = New GetPaymentCashListDetailsQueryResponse
                oPaymentCashList = New PaymentCashList
                sbLogMessage = New StringBuilder

                With oGetPaymentCashListDetailsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .CashListKey = v_iCashListKey

                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetPaymentCashListDetails, oGetPaymentCashListDetailsRequest)
                    oGetPaymentCashListDetailsResponse = ApiClient.DeserializeJson(Of GetPaymentCashListDetailsQueryResponse)(result)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.
                With oGetPaymentCashListDetailsResponse.GetPaymentCashListDetailsResponse

                    oPaymentCashList.BankAccountCode = .PaymentCashList.BankAccountCode
                    oPaymentCashList.CurrencyCode = .PaymentCashList.CurrencyCode
                    oPaymentCashList.ListDate = .PaymentCashList.ListDate
                    oPaymentCashList.Reference = .PaymentCashList.Reference
                    oPaymentCashList.StatusCode = .PaymentCashList.StatusCode
                    oPaymentCashList.TypeCode = .PaymentCashList.TypeCode

                    If .PaymentCashList.PaymentItem IsNot Nothing AndAlso .PaymentCashList.PaymentItem.Count > 0 Then
                        For Each oPymentCashList As BasePaymentCashListItemType In .PaymentCashList.PaymentItem

                            oPaymentCashListItemType = New PaymentCashListItemType

                            oPaymentCashListItemType.AccountShortCode = oPymentCashList.AccountShortCode
                            oPaymentCashListItemType.AllocationStatusCode = oPymentCashList.AllocationStatusCode
                            oPaymentCashListItemType.Amount = oPymentCashList.Amount

                            oPaymentCashListItemType.BankPaymentType.AccountCode = oPymentCashList.Bank.AccountCode
                            oPaymentCashListItemType.BankPaymentType.BranchCode = oPymentCashList.Bank.BranchCode
                            oPaymentCashListItemType.BankPaymentType.ExpiryDate = oPymentCashList.Bank.ExpiryDate
                            oPaymentCashListItemType.BankPaymentType.PayeeName = oPymentCashList.Bank.PayeeName
                            oPaymentCashListItemType.BankPaymentType.Reference1 = oPymentCashList.Bank.Reference1
                            oPaymentCashListItemType.BankPaymentType.Reference2 = oPymentCashList.Bank.Reference2
                            oPaymentCashListItemType.BankPaymentType.BIC = oPaymentCashList.Bank.BIC
                            oPaymentCashListItemType.BankPaymentType.IBAN = oPaymentCashList.Bank.IBAN

                            oPaymentCashListItemType.BankReference = oPymentCashList.BankReference

                            oPaymentCashListItemType.ContactAddress.Address1 = oPymentCashList.ContactAddress.AddressLine1
                            oPaymentCashListItemType.ContactAddress.Address2 = oPymentCashList.ContactAddress.AddressLine2
                            oPaymentCashListItemType.ContactAddress.Address3 = oPymentCashList.ContactAddress.AddressLine3
                            oPaymentCashListItemType.ContactAddress.Address4 = oPymentCashList.ContactAddress.AddressLine4
                            oPaymentCashListItemType.ContactAddress.CountryCode = oPymentCashList.ContactAddress.CountryCode
                            oPaymentCashListItemType.ContactAddress.PostCode = oPymentCashList.ContactAddress.PostCode
                            oPaymentCashListItemType.ContactName = oPymentCashList.ContactName

                            oPaymentCashListItemType.CreditCard.AuthCode = oPymentCashList.CreditCard.AuthCode

                            oPaymentCashListItemType.CreditCard.CardHolder.Address1 = oPymentCashList.CreditCard.CardHolder.AddressLine1
                            oPaymentCashListItemType.CreditCard.CardHolder.Address2 = oPymentCashList.CreditCard.CardHolder.AddressLine2
                            oPaymentCashListItemType.CreditCard.CardHolder.Address3 = oPymentCashList.CreditCard.CardHolder.AddressLine3
                            oPaymentCashListItemType.CreditCard.CardHolder.Address4 = oPymentCashList.CreditCard.CardHolder.AddressLine4
                            oPaymentCashListItemType.CreditCard.CardHolder.CountryCode = oPymentCashList.CreditCard.CardHolder.CountryCode
                            oPaymentCashListItemType.CreditCard.CardHolder.Name = oPymentCashList.CreditCard.CardHolder.Name
                            oPaymentCashListItemType.CreditCard.CardHolder.PostCode = oPymentCashList.CreditCard.CardHolder.PostCode

                            oPaymentCashListItemType.CreditCard.CustomerPresent = oPymentCashList.CreditCard.CustomerPresent
                            oPaymentCashListItemType.CreditCard.ExpiryDate = oPymentCashList.CreditCard.ExpiryDate
                            oPaymentCashListItemType.CreditCard.Issue = oPymentCashList.CreditCard.Issue
                            oPaymentCashListItemType.CreditCard.ManualAuthCode = oPymentCashList.CreditCard.ManualAuthCode
                            oPaymentCashListItemType.CreditCard.NameOnCreditCard = oPymentCashList.CreditCard.NameOnCreditCard
                            oPaymentCashListItemType.CreditCard.Number = oPymentCashList.CreditCard.Number
                            oPaymentCashListItemType.CreditCard.Pin = oPymentCashList.CreditCard.Pin
                            oPaymentCashListItemType.CreditCard.StartDate = oPymentCashList.CreditCard.StartDate
                            oPaymentCashListItemType.CreditCard.TransactionCode = oPymentCashList.CreditCard.TransactionCode
                            oPaymentCashListItemType.CreditCard.TypeCode = oPymentCashList.CreditCard.TypeCode

                            oPaymentCashListItemType.FurtherDetails = oPymentCashList.FurtherDetails
                            oPaymentCashListItemType.IsProduceDocument = oPymentCashList.IsProduceDocument
                            oPaymentCashListItemType.MediaReference = oPymentCashList.MediaReference
                            oPaymentCashListItemType.MediaTypeCode = oPymentCashList.MediaTypeCode
                            oPaymentCashListItemType.OurReference = oPymentCashList.OurReference
                            oPaymentCashListItemType.StatusCode = oPymentCashList.StatusCode
                            oPaymentCashListItemType.TheirReference = oPymentCashList.TheirReference
                            oPaymentCashListItemType.TransactionDate = oPymentCashList.TransactionDate
                            oPaymentCashListItemType.TypeCode = oPymentCashList.TypeCode


                            oPaymentCashList.PaymentCashListItemType.Add(oPaymentCashListItemType)
                        Next
                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetPaymentCashListDetails executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    'sbLogMessage.AppendLine("r_oQuote = " & r_oQuote.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally

                oGetPaymentCashListDetailsRequest = Nothing
                oGetPaymentCashListDetailsResponse = Nothing
            End Try

            Return oPaymentCashList

        End SyncLock

    End Function

    Public Overrides Function GetPaymentCashListItemDetails(ByVal v_iCashListItemKey As Integer,
                                 Optional ByVal v_sBranchCode As String = Nothing) As PaymentCashListItemType

        SyncLock oLock

            Dim oGetPaymentCashListItemDetailsRequest As GetPaymentCashListItemDetailsQuery
            Dim oGetPaymentCashListItemDetailsResponse As GetPaymentCashListItemDetailsQueryResponse
            Dim oPymentCashList As PaymentCashListItemType
            Dim sbLogMessage As StringBuilder

            Try

                oGetPaymentCashListItemDetailsRequest = New GetPaymentCashListItemDetailsQuery
                oGetPaymentCashListItemDetailsResponse = New GetPaymentCashListItemDetailsQueryResponse
                sbLogMessage = New StringBuilder

                With oGetPaymentCashListItemDetailsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .CashListItemKey = v_iCashListItemKey

                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetPaymentCashListItemDetails, oGetPaymentCashListItemDetailsRequest)
                    oGetPaymentCashListItemDetailsResponse = ApiClient.DeserializeJson(Of GetPaymentCashListItemDetailsQueryResponse)(result)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.
                With oGetPaymentCashListItemDetailsResponse.GetPaymentCashListItemDetailsResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else

                        oPymentCashList = New PaymentCashListItemType

                        oPymentCashList.AccountShortCode = .CashListPayment.AccountShortCode
                        oPymentCashList.AllocationStatusCode = .CashListPayment.AllocationStatusCode
                        oPymentCashList.Amount = .CashListPayment.Amount
                        If .CashListPayment.Bank IsNot Nothing Then

                            oPymentCashList.BankPaymentType.AccountCode = .CashListPayment.Bank.AccountCode
                            oPymentCashList.BankPaymentType.BranchCode = .CashListPayment.Bank.BranchCode
                            oPymentCashList.BankPaymentType.ExpiryDate = .CashListPayment.Bank.ExpiryDate
                            oPymentCashList.BankPaymentType.PayeeName = .CashListPayment.Bank.PayeeName
                            oPymentCashList.BankPaymentType.Reference1 = .CashListPayment.Bank.Reference1
                            oPymentCashList.BankPaymentType.Reference2 = .CashListPayment.Bank.Reference2
                            oPymentCashList.BankPaymentType.BIC = .CashListPayment.Bank.BIC
                            oPymentCashList.BankPaymentType.IBAN = .CashListPayment.Bank.IBAN

                        End If

                        oPymentCashList.BankReference = .CashListPayment.BankReference

                        If .CashListPayment.ContactAddress IsNot Nothing Then

                            oPymentCashList.ContactAddress.Address1 = .CashListPayment.ContactAddress.AddressLine1
                            oPymentCashList.ContactAddress.Address2 = .CashListPayment.ContactAddress.AddressLine2
                            oPymentCashList.ContactAddress.Address3 = .CashListPayment.ContactAddress.AddressLine3
                            oPymentCashList.ContactAddress.Address4 = .CashListPayment.ContactAddress.AddressLine4
                            oPymentCashList.ContactAddress.CountryCode = .CashListPayment.ContactAddress.CountryCode
                            oPymentCashList.ContactAddress.PostCode = .CashListPayment.ContactAddress.PostCode
                            oPymentCashList.ContactName = .CashListPayment.ContactName
                        End If

                        If .CashListPayment.CreditCard IsNot Nothing Then

                            oPymentCashList.CreditCard.AuthCode = .CashListPayment.CreditCard.AuthCode
                            If .CashListPayment.CreditCard.CardHolder IsNot Nothing Then

                                oPymentCashList.CreditCard.CardHolder.Address1 = .CashListPayment.CreditCard.CardHolder.AddressLine1
                                oPymentCashList.CreditCard.CardHolder.Address2 = .CashListPayment.CreditCard.CardHolder.AddressLine2
                                oPymentCashList.CreditCard.CardHolder.Address3 = .CashListPayment.CreditCard.CardHolder.AddressLine3
                                oPymentCashList.CreditCard.CardHolder.Address4 = .CashListPayment.CreditCard.CardHolder.AddressLine4
                                oPymentCashList.CreditCard.CardHolder.CountryCode = .CashListPayment.CreditCard.CardHolder.CountryCode
                                oPymentCashList.CreditCard.CardHolder.Name = .CashListPayment.CreditCard.CardHolder.Name
                                oPymentCashList.CreditCard.CardHolder.PostCode = .CashListPayment.CreditCard.CardHolder.PostCode

                            End If

                            oPymentCashList.CreditCard.CustomerPresent = .CashListPayment.CreditCard.CustomerPresent
                            oPymentCashList.CreditCard.ExpiryDate = .CashListPayment.CreditCard.ExpiryDate
                            oPymentCashList.CreditCard.Issue = .CashListPayment.CreditCard.Issue
                            oPymentCashList.CreditCard.ManualAuthCode = .CashListPayment.CreditCard.ManualAuthCode
                            oPymentCashList.CreditCard.NameOnCreditCard = .CashListPayment.CreditCard.NameOnCreditCard
                            oPymentCashList.CreditCard.Number = .CashListPayment.CreditCard.Number
                            oPymentCashList.CreditCard.Pin = .CashListPayment.CreditCard.Pin
                            oPymentCashList.CreditCard.StartDate = .CashListPayment.CreditCard.StartDate
                            oPymentCashList.CreditCard.TransactionCode = .CashListPayment.CreditCard.TransactionCode
                            oPymentCashList.CreditCard.TypeCode = .CashListPayment.CreditCard.TypeCode
                        End If

                        oPymentCashList.FurtherDetails = .CashListPayment.FurtherDetails
                        oPymentCashList.IsProduceDocument = .CashListPayment.IsProduceDocument
                        oPymentCashList.MediaReference = .CashListPayment.MediaReference
                        oPymentCashList.MediaTypeCode = .CashListPayment.MediaTypeCode
                        oPymentCashList.OurReference = .CashListPayment.OurReference
                        oPymentCashList.StatusCode = .CashListPayment.StatusCode
                        oPymentCashList.TheirReference = .CashListPayment.TheirReference
                        oPymentCashList.TransactionDate = .CashListPayment.TransactionDate
                        oPymentCashList.TypeCode = .CashListPayment.TypeCode
                        oPymentCashList.UserName = .CashListPayment.UserName
                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetPaymentCashListItemDetails executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    'sbLogMessage.AppendLine("r_oQuote = " & r_oQuote.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally

                oGetPaymentCashListItemDetailsRequest = Nothing
                oGetPaymentCashListItemDetailsResponse = Nothing
            End Try


            Return oPymentCashList
        End SyncLock

    End Function

    Public Overrides Function GetPaymentCashListItems(ByVal v_iCashListKey As Integer,
                                Optional ByVal v_sBranchCode As String = Nothing) As PaymentCashListItemTypeCollection

        SyncLock oLock


            Dim oGetPaymentCashListItemsRequest As GetPaymentCashListItemsQuery
            Dim oGetPaymentCashListItemsResponse As GetPaymentCashListItemsQueryResponse
            Dim oPaymentCollection As PaymentCashListItemTypeCollection
            Dim oPaymentCashListItemType As PaymentCashListItemType
            Dim sbLogMessage As StringBuilder

            Try

                oGetPaymentCashListItemsRequest = New GetPaymentCashListItemsQuery
                oGetPaymentCashListItemsResponse = New GetPaymentCashListItemsQueryResponse
                oPaymentCollection = New PaymentCashListItemTypeCollection
                sbLogMessage = New StringBuilder

                With oGetPaymentCashListItemsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .CashListKey = v_iCashListKey

                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetPaymentCashListItems, oGetPaymentCashListItemsRequest)
                    oGetPaymentCashListItemsResponse = ApiClient.DeserializeJson(Of GetPaymentCashListItemsQueryResponse)(result)
                End Using
                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                With oGetPaymentCashListItemsResponse.GetPaymentCashListItemsResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else

                        If .PaymentCashListItems IsNot Nothing AndAlso .PaymentCashListItems.Count > 0 Then
                            For Each oPaymentCashList As BaseGetPaymentCashListItemsResponseTypeRow In .PaymentCashListItems

                                oPaymentCashListItemType = New PaymentCashListItemType()

                                oPaymentCashListItemType.CashListKey = oPaymentCashList.CashListItemKey
                                oPaymentCashListItemType.MediaReference = oPaymentCashList.MediaReference
                                oPaymentCashListItemType.MediaTypeCode = oPaymentCashList.MediaType
                                oPaymentCashListItemType.Amount = oPaymentCashList.Amount
                                oPaymentCashListItemType.AccountShortCode = oPaymentCashList.AccountShortCode
                                oPaymentCashListItemType.StatusCode = oPaymentCashList.Status
                                oPaymentCashListItemType.Letter = oPaymentCashList.Letter

                                oPaymentCollection.Add(oPaymentCashListItemType)

                            Next
                        End If
                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetPaymentCashListItems executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    'sbLogMessage.AppendLine("r_oQuote = " & r_oQuote.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally

                oGetPaymentCashListItemsRequest = Nothing
                oGetPaymentCashListItemsResponse = Nothing
            End Try

            Return oPaymentCollection

        End SyncLock

    End Function

    Public Overrides Function GetReceiptCashListDetails(ByVal v_iCashListKey As Integer,
                                  Optional ByVal v_sBranchCode As String = Nothing) As ReceiptCashListCollection


        SyncLock oLock


            Dim oGetReceiptCashListDetailsRequest As GetReceiptCashListDetailsQuery
            Dim oGetReceiptCashListDetailsResponse As GetReceiptCashListDetailsQueryResponse
            Dim oReceiptCashList As ReceiptCashList
            Dim oReceiptCashListCollection As ReceiptCashListCollection
            Dim oBaseReceiptCashListItemTypePolicies As ReceiptCashListItemTypePolicies
            Dim sbLogMessage As StringBuilder

            Try

                oGetReceiptCashListDetailsRequest = New GetReceiptCashListDetailsQuery
                oGetReceiptCashListDetailsResponse = New GetReceiptCashListDetailsQueryResponse
                oReceiptCashListCollection = New ReceiptCashListCollection
                sbLogMessage = New StringBuilder

                With oGetReceiptCashListDetailsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .CashListKey = v_iCashListKey

                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetReceiptCashListDetails, oGetReceiptCashListDetailsRequest)
                    oGetReceiptCashListDetailsResponse = ApiClient.DeserializeJson(Of GetReceiptCashListDetailsQueryResponse)(result)
                End Using

                With oGetReceiptCashListDetailsResponse.GetReceiptCashListDetailsResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else

                        If .ReceiptCashList.ReceiptItem IsNot Nothing AndAlso .ReceiptCashList.ReceiptItem.Count > 0 Then
                            For Each oReceiptCashListRow As BaseReceiptCashListItemType In .ReceiptCashList.ReceiptItem

                                oReceiptCashList = New ReceiptCashList

                                oReceiptCashList.TypeCode = oReceiptCashListRow.TypeCode
                                oReceiptCashList.StatusCode = oReceiptCashListRow.StatusCode

                                oReceiptCashList.CreditCard.AuthCode = oReceiptCashListRow.CreditCard.AuthCode
                                oReceiptCashList.CreditCard.CreditCardType_CardHolder.Address1 = oReceiptCashListRow.CreditCard.CardHolder.AddressLine1
                                oReceiptCashList.CreditCard.CreditCardType_CardHolder.Address2 = oReceiptCashListRow.CreditCard.CardHolder.AddressLine2
                                oReceiptCashList.CreditCard.CreditCardType_CardHolder.Address3 = oReceiptCashListRow.CreditCard.CardHolder.AddressLine3
                                oReceiptCashList.CreditCard.CreditCardType_CardHolder.Address4 = oReceiptCashListRow.CreditCard.CardHolder.AddressLine4
                                oReceiptCashList.CreditCard.CreditCardType_CardHolder.CountryCode = oReceiptCashListRow.CreditCard.CardHolder.CountryCode
                                oReceiptCashList.CreditCard.CreditCardType_CardHolder.Name = oReceiptCashListRow.CreditCard.CardHolder.Name
                                oReceiptCashList.CreditCard.CreditCardType_CardHolder.PostCode = oReceiptCashListRow.CreditCard.CardHolder.PostCode

                                oReceiptCashList.CreditCard.CustomerPresent = oReceiptCashListRow.CreditCard.CustomerPresent
                                oReceiptCashList.CreditCard.ExpiryDate = oReceiptCashListRow.CreditCard.ExpiryDate
                                oReceiptCashList.CreditCard.Issue = oReceiptCashListRow.CreditCard.Issue
                                oReceiptCashList.CreditCard.ManualAuthCode = oReceiptCashListRow.CreditCard.ManualAuthCode
                                oReceiptCashList.CreditCard.NameOnCreditCard = oReceiptCashListRow.CreditCard.NameOnCreditCard
                                oReceiptCashList.CreditCard.Number = oReceiptCashListRow.CreditCard.Number
                                oReceiptCashList.CreditCard.Pin = oReceiptCashListRow.CreditCard.Pin
                                oReceiptCashList.CreditCard.StartDate = oReceiptCashListRow.CreditCard.StartDate
                                oReceiptCashList.CreditCard.TransactionCode = oReceiptCashListRow.CreditCard.TransactionCode
                                oReceiptCashList.CreditCard.TypeCode = oReceiptCashListRow.CreditCard.TypeCode

                                oReceiptCashList.Bank.BankCode = oReceiptCashListRow.Bank.BankCode
                                oReceiptCashList.Bank.ChequeDate = oReceiptCashListRow.Bank.ChequeDate
                                oReceiptCashList.Bank.PayerName = oReceiptCashListRow.Bank.PayerName

                                oReceiptCashList.AllocationType = oReceiptCashListRow.AllocationType

                                If oReceiptCashListRow.Policies IsNot Nothing AndAlso oReceiptCashListRow.Policies.Count > 0 Then
                                    For Each oPolicy As BaseReceiptCashListItemTypePolicies In oReceiptCashListRow.Policies
                                        oBaseReceiptCashListItemTypePolicies = New ReceiptCashListItemTypePolicies

                                        oBaseReceiptCashListItemTypePolicies.InsuranceFileKey = oPolicy.InsuranceFileKey
                                        oBaseReceiptCashListItemTypePolicies.DocumentRef = oPolicy.DocumentRef
                                        oBaseReceiptCashListItemTypePolicies.WriteOffReasonKey = oPolicy.WriteOffReasonKey
                                        oBaseReceiptCashListItemTypePolicies.WriteOffAmount = oPolicy.WriteOffAmount
                                        oBaseReceiptCashListItemTypePolicies.IsCurrencyWriteOff = oPolicy.IsCurrencyWriteOff
                                        oBaseReceiptCashListItemTypePolicies.AmountTobeAllocated = oPolicy.AmountTobeAllocated
                                        oBaseReceiptCashListItemTypePolicies.BGKey = oPolicy.BGKey

                                        oReceiptCashList.Policies.Add(oBaseReceiptCashListItemTypePolicies)
                                    Next
                                End If
                                oReceiptCashListCollection.Add(oReceiptCashList)
                            Next
                        End If

                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetReceiptCashListDetails executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    'sbLogMessage.AppendLine("v_iAddressKey = " & v_iAddressKey.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally

                oGetReceiptCashListDetailsRequest = Nothing
                oGetReceiptCashListDetailsResponse = Nothing
            End Try

            Return oReceiptCashListCollection

        End SyncLock
    End Function


    Public Overrides Function GetReceiptCashListItemDetails(ByVal v_iCashListItemKey As Integer,
                                  Optional ByVal v_sBranchCode As String = Nothing) As ReceiptCashList


        SyncLock oLock


            Dim oGetReceiptCashListItemDetailsRequest As GetReceiptCashListItemDetailsQuery
            Dim oGetReceiptCashListItemDetailsResponse As GetReceiptCashListItemDetailsQueryResponse
            Dim oReceiptCashList As ReceiptCashList
            Dim oBaseReceiptCashListItemTypePolicies As ReceiptCashListItemTypePolicies
            Dim sbLogMessage As StringBuilder

            Try

                oGetReceiptCashListItemDetailsRequest = New GetReceiptCashListItemDetailsQuery
                oGetReceiptCashListItemDetailsResponse = New GetReceiptCashListItemDetailsQueryResponse
                sbLogMessage = New StringBuilder

                With oGetReceiptCashListItemDetailsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .CashListItemKey = v_iCashListItemKey


                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetReceiptCashListItemDetails, oGetReceiptCashListItemDetailsRequest)
                    oGetReceiptCashListItemDetailsResponse = ApiClient.DeserializeJson(Of GetReceiptCashListItemDetailsQueryResponse)(result)
                End Using

                With oGetReceiptCashListItemDetailsResponse.GetReceiptCashListItemDetailsResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else

                        oReceiptCashList = New ReceiptCashList

                        oReceiptCashList.TypeCode = .CashListReceipt.TypeCode
                        oReceiptCashList.StatusCode = .CashListReceipt.StatusCode

                        oReceiptCashList.CreditCard.AuthCode = .CashListReceipt.CreditCard.AuthCode
                        oReceiptCashList.CreditCard.CreditCardType_CardHolder.Address1 = .CashListReceipt.CreditCard.CardHolder.AddressLine1
                        oReceiptCashList.CreditCard.CreditCardType_CardHolder.Address2 = .CashListReceipt.CreditCard.CardHolder.AddressLine2
                        oReceiptCashList.CreditCard.CreditCardType_CardHolder.Address3 = .CashListReceipt.CreditCard.CardHolder.AddressLine3
                        oReceiptCashList.CreditCard.CreditCardType_CardHolder.Address4 = .CashListReceipt.CreditCard.CardHolder.AddressLine4
                        oReceiptCashList.CreditCard.CreditCardType_CardHolder.CountryCode = .CashListReceipt.CreditCard.CardHolder.CountryCode
                        oReceiptCashList.CreditCard.CreditCardType_CardHolder.Name = .CashListReceipt.CreditCard.CardHolder.Name
                        oReceiptCashList.CreditCard.CreditCardType_CardHolder.PostCode = .CashListReceipt.CreditCard.CardHolder.PostCode

                        oReceiptCashList.CreditCard.CustomerPresent = .CashListReceipt.CreditCard.CustomerPresent
                        oReceiptCashList.CreditCard.ExpiryDate = .CashListReceipt.CreditCard.ExpiryDate
                        oReceiptCashList.CreditCard.Issue = .CashListReceipt.CreditCard.Issue
                        oReceiptCashList.CreditCard.ManualAuthCode = .CashListReceipt.CreditCard.ManualAuthCode
                        oReceiptCashList.CreditCard.NameOnCreditCard = .CashListReceipt.CreditCard.NameOnCreditCard
                        oReceiptCashList.CreditCard.Number = .CashListReceipt.CreditCard.Number
                        oReceiptCashList.CreditCard.Pin = .CashListReceipt.CreditCard.Pin
                        oReceiptCashList.CreditCard.StartDate = .CashListReceipt.CreditCard.StartDate
                        oReceiptCashList.CreditCard.TransactionCode = .CashListReceipt.CreditCard.TransactionCode
                        oReceiptCashList.CreditCard.TypeCode = .CashListReceipt.CreditCard.TypeCode

                        oReceiptCashList.Bank.BankCode = .CashListReceipt.Bank.BankCode
                        oReceiptCashList.Bank.ChequeDate = .CashListReceipt.Bank.ChequeDate
                        oReceiptCashList.Bank.PayerName = .CashListReceipt.Bank.PayerName

                        oReceiptCashList.AllocationType = .CashListReceipt.AllocationType

                        If .CashListReceipt.Policies IsNot Nothing AndAlso .CashListReceipt.Policies.Count > 0 Then
                            For Each oPolicy As BaseReceiptCashListItemTypePolicies In .CashListReceipt.Policies
                                oBaseReceiptCashListItemTypePolicies = New ReceiptCashListItemTypePolicies
                                oBaseReceiptCashListItemTypePolicies.InsuranceFileKey = oPolicy.InsuranceFileKey
                                oBaseReceiptCashListItemTypePolicies.DocumentRef = oPolicy.DocumentRef
                                oBaseReceiptCashListItemTypePolicies.WriteOffReasonKey = oPolicy.WriteOffReasonKey
                                oBaseReceiptCashListItemTypePolicies.WriteOffAmount = oPolicy.WriteOffAmount
                                oBaseReceiptCashListItemTypePolicies.IsCurrencyWriteOff = oPolicy.IsCurrencyWriteOff
                                oBaseReceiptCashListItemTypePolicies.AmountTobeAllocated = oPolicy.AmountTobeAllocated
                                oBaseReceiptCashListItemTypePolicies.BGKey = oPolicy.BGKey

                                oReceiptCashList.Policies.Add(oBaseReceiptCashListItemTypePolicies)
                            Next
                        End If

                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetReceiptCashListItemDetails executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    'sbLogMessage.AppendLine("v_iAddressKey = " & v_iAddressKey.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally

                oGetReceiptCashListItemDetailsRequest = Nothing
                oGetReceiptCashListItemDetailsResponse = Nothing
            End Try

            Return oReceiptCashList

        End SyncLock
    End Function

    Public Overrides Function GetReceiptCashListItems(ByVal v_iCashListKey As Integer,
                                  Optional ByVal v_sBranchCode As String = Nothing) As ReceiptCashListItemsCollection


        SyncLock oLock


            Dim oGetReceiptCashListItemsRequest As GetReceiptCashListItemsQuery
            Dim oGetReceiptCashListItemsResponse As GetReceiptCashListItemsQueryResponse
            Dim oReceiptCashListItems As ReceiptCashListItems
            Dim oReceiptCashListItemsCollection As ReceiptCashListItemsCollection
            Dim sbLogMessage As StringBuilder

            Try

                oGetReceiptCashListItemsRequest = New GetReceiptCashListItemsQuery
                oGetReceiptCashListItemsResponse = New GetReceiptCashListItemsQueryResponse
                oReceiptCashListItemsCollection = New ReceiptCashListItemsCollection
                sbLogMessage = New StringBuilder


                With oGetReceiptCashListItemsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .CashListKey = v_iCashListKey


                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetReceiptCashListItems, oGetReceiptCashListItemsRequest)
                    oGetReceiptCashListItemsResponse = ApiClient.DeserializeJson(Of GetReceiptCashListItemsQueryResponse)(result)
                End Using

                With oGetReceiptCashListItemsResponse.GetReceiptCashListItemsResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else

                        For Each oReceiptCashList As BaseGetReceiptCashListItemsResponseTypeRow In .ReceiptCashListItems
                            oReceiptCashListItems = New ReceiptCashListItems()

                            oReceiptCashListItems.CashListItemKey = oReceiptCashList.CashListItemKey
                            oReceiptCashListItems.MediaReference = oReceiptCashList.MediaReference
                            oReceiptCashListItems.MediaType = oReceiptCashList.MediaType
                            oReceiptCashListItems.Amount = oReceiptCashList.Amount
                            oReceiptCashListItems.AccountShortCode = oReceiptCashList.AccountShortCode
                            oReceiptCashListItems.Status = oReceiptCashList.Status
                            oReceiptCashListItems.Letter = oReceiptCashList.Letter

                            oReceiptCashListItemsCollection.Add(oReceiptCashListItems)
                        Next


                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetReceiptCashListItems executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    'sbLogMessage.AppendLine("v_iAddressKey = " & v_iAddressKey.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally

                oGetReceiptCashListItemsRequest = Nothing
                oGetReceiptCashListItemsResponse = Nothing
            End Try

            Return oReceiptCashListItemsCollection

        End SyncLock
    End Function


    Public Overrides Sub ManageBankDetails(ByVal v_iPartyKey As Integer,
                                     ByVal vPartyBankDetails As BankCollection,
                                     Optional ByVal v_sBranchCode As String = Nothing)

        Dim iCount As Integer
        Dim oAddBankCollection As New BankCollection
        Dim oUpdateBankCollection As New BankCollection
        Dim oDeleteBankCollection As New BankCollection
        Dim oActivateBankCollection As New BankCollection


        For iCount = 0 To vPartyBankDetails.Count - 1
            If vPartyBankDetails(iCount).TaskMode = NexusProvider.Bank.Mode.Add Then
                oAddBankCollection.Add(vPartyBankDetails(iCount))
            ElseIf vPartyBankDetails(iCount).TaskMode = NexusProvider.Bank.Mode.Edit Or vPartyBankDetails(iCount).TaskMode = NexusProvider.Bank.Mode.EditAndMakeActive Then
                oUpdateBankCollection.Add(vPartyBankDetails(iCount))
                If vPartyBankDetails(iCount).TaskMode = NexusProvider.Bank.Mode.EditAndMakeActive Then
                    oActivateBankCollection.Add(vPartyBankDetails(iCount))
                End If
            ElseIf vPartyBankDetails(iCount).TaskMode = NexusProvider.Bank.Mode.Delete Then
                oDeleteBankCollection.Add(vPartyBankDetails(iCount))
            ElseIf vPartyBankDetails(iCount).TaskMode = NexusProvider.Bank.Mode.MakeActive Or vPartyBankDetails(iCount).TaskMode = NexusProvider.Bank.Mode.MakeInactive Then
                oActivateBankCollection.Add(vPartyBankDetails(iCount))
            End If
        Next

        'Add
        If oAddBankCollection IsNot Nothing AndAlso oAddBankCollection.Count > 0 Then
            AddPartyBankDetails(v_iPartyKey, oAddBankCollection, v_sBranchCode)
        End If

        'Update
        If oUpdateBankCollection IsNot Nothing AndAlso oUpdateBankCollection.Count > 0 Then
            UpdatePartyBankDetails(v_iPartyKey, oUpdateBankCollection, v_sBranchCode)
        End If

        'Delete
        If oDeleteBankCollection IsNot Nothing AndAlso oDeleteBankCollection.Count > 0 Then
            DeletePartyBankDetails(v_iPartyKey, oDeleteBankCollection, v_sBranchCode)
        End If

        'Activate or Deactivate
        If oActivateBankCollection IsNot Nothing AndAlso oActivateBankCollection.Count > 0 Then
            ActivatePartyBankDetails(v_iPartyKey, oActivateBankCollection, v_sBranchCode)
        End If

    End Sub
    ''' <summary>
    ''' This method  will add the BankGuarantee Details
    ''' </summary>
    ''' <param name="oBankCollection"></param>
    ''' <param name="oBranchCollection"></param>
    ''' <param name="oProductCollection"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    Public Overrides Function UpdateBankGuarantee(ByVal oBankCollection As BankGuaranteeCollection,
                    ByVal oBranchCollection As BranchCollection,
                    ByRef oProductCollection As ProductCollection,
                    Optional ByVal v_sBranchCode As String = Nothing) As BankGuaranteeCollection





        Return New BankGuaranteeCollection 'Returning BankGuarantee Collection
    End Function

    Public Overrides Sub UpdateBankGuaranteeConditionally(ByVal oBankCollection As BankGuaranteeCollection,
                                     Optional ByVal v_sBranchCode As String = Nothing)




    End Sub

    Public Overrides Function UpdateCashDeposit(ByVal oCDColl As NexusProvider.CashDepositCollection,
                                          Optional ByVal v_sBranchCode As String = Nothing) As CashDepositCollection

        SyncLock oLock



            Return New CashDepositCollection

        End SyncLock
    End Function


    Public Overrides Sub UpdateReceiptMediaTypeStatus(ByVal oCashListReceipts As CashListReceipts,
                                              Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock


            Dim oUpdateReceiptMediaTypeStatusRequestType As UpdateReceiptMediaTypeStatusCommand
            Dim oUpdateReceiptMediaTypeStatusResponseType As UpdateReceiptMediaTypeStatusCommandResponse
            Dim oCashListItems As BaseUpdateReceiptMediaTypeStatusRequestTypeRow
            Dim oCashListReceiptsCollection As CashListReceipts
            Try

                oUpdateReceiptMediaTypeStatusRequestType = New UpdateReceiptMediaTypeStatusCommand
                oUpdateReceiptMediaTypeStatusResponseType = New UpdateReceiptMediaTypeStatusCommandResponse
                oCashListReceiptsCollection = New CashListReceipts


                With oUpdateReceiptMediaTypeStatusRequestType
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    'if the passed parameter v_sBranchCode is empty 
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If


                    .CashListItems = New List(Of BaseUpdateReceiptMediaTypeStatusRequestTypeRow)
                    For i As Integer = 0 To oCashListReceipts.Count - 1
                        oCashListItems = New BaseUpdateReceiptMediaTypeStatusRequestTypeRow
                        With oCashListItems
                            .CashListItemKey = oCashListReceipts(i).CashListItemKey
                            .Comments = oCashListReceipts(i).Comments
                            .DocumentRef = oCashListReceipts(i).DocumentRef
                            .InsuranceFileKey = oCashListReceipts(i).InsuranceFileKey
                            .MediaTypeCode = oCashListReceipts(i).MediaTypeCode
                            .MediaTypeStatusCode = oCashListReceipts(i).MediaTypeStatusCode
                            .ModifiedDate = oCashListReceipts(i).ModifiedDate
                        End With
                        .CashListItems.Add(oCashListItems)
                    Next
                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Put(ApiMethods.UpdateReceiptMediaTypeStatus, oUpdateReceiptMediaTypeStatusRequestType)
                    oUpdateReceiptMediaTypeStatusResponseType = ApiClient.DeserializeJson(Of UpdateReceiptMediaTypeStatusCommandResponse)(result)
                End Using

            Catch ex As Exception
                Throw
            Finally

                oUpdateReceiptMediaTypeStatusRequestType = Nothing
                oUpdateReceiptMediaTypeStatusResponseType = Nothing

            End Try
        End SyncLock
    End Sub

    Public Overrides Function ValidateBankAccountNumber(ByVal v_iBankMediaID As Integer,
                                                        ByVal v_iBankCountryID As Integer,
                                                        Optional ByVal v_sAccountNumber As String = Nothing,
                                                        Optional ByVal v_sBankMediaCode As String = Nothing,
                                                        Optional ByVal v_sBranchCode As String = Nothing,
                                                        Optional ByVal sBIC As String = Nothing,
                                                        Optional ByVal sIBAN As String = Nothing,
                                                        Optional ByVal sBankName As String = Nothing) As ValidationDetailsCollection
        SyncLock oLock


            Dim oValidateBankAccountNumberRequest As ValidateBankAccountNumberCommand
            Dim oValidateBankAccountNumberResponse As ValidateBankAccountNumberCommandResponse
            Dim v_oValidationDetailsCollection As ValidationDetailsCollection
            Dim v_oValidationDetail As ValidationDetails
            Dim sbLogMessage As StringBuilder

            Try

                oValidateBankAccountNumberRequest = New ValidateBankAccountNumberCommand
                oValidateBankAccountNumberResponse = New ValidateBankAccountNumberCommandResponse
                v_oValidationDetailsCollection = New ValidationDetailsCollection
                sbLogMessage = New StringBuilder


                With oValidateBankAccountNumberRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .AccountNumber = v_sAccountNumber 'Branchcode|AccountNo
                    .BankCountryKey = v_iBankCountryID ' BankCountryID in the previous build
                    .BankMediaCode = v_sBankMediaCode
                    .BankMediaKey = v_iBankMediaID ' BankMediaID in the previous Build
                    .BIC = sBIC
                    .IBAN = sIBAN
                    .BankName = sBankName
                End With
                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Post(ApiMethods.ValidateBankAccountNumber, oValidateBankAccountNumberRequest)
                    oValidateBankAccountNumberResponse = ApiClient.DeserializeJson(Of ValidateBankAccountNumberCommandResponse)(result)
                End Using

                With oValidateBankAccountNumberResponse

                    If .ValidationDetails IsNot Nothing Then

                        For Each v_oValidationDetails As BaseValidateBankAccountNumberResponseTypeRow In .ValidationDetails
                            v_oValidationDetail = New ValidationDetails()
                            v_oValidationDetail.BankName = v_oValidationDetails.BankName
                            v_oValidationDetail.IsValid = v_oValidationDetails.IsValid
                            v_oValidationDetail.ValidationMessageDataset = v_oValidationDetails.ValidationMessageDataset
                            v_oValidationDetail.AddressLine1 = v_oValidationDetails.AddressLine1
                            v_oValidationDetail.AddressLine2 = v_oValidationDetails.AddressLine2
                            v_oValidationDetail.AddressLine3 = v_oValidationDetails.AddressLine3
                            v_oValidationDetail.AddressLine4 = v_oValidationDetails.AddressLine4
                            v_oValidationDetail.PostalCode = v_oValidationDetails.PostalCode
                            v_oValidationDetailsCollection.Add(v_oValidationDetail)
                        Next

                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("ValidateBankAccountNumber executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    'sbLogMessage.AppendLine("r_oAddress = " & r_oAddress.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally

                oValidateBankAccountNumberRequest = Nothing
                oValidateBankAccountNumberResponse = Nothing
            End Try


            Return v_oValidationDetailsCollection

        End SyncLock
    End Function

    ''' <summary>
    ''' To get the bank guarantee details for the bank guarantee reference.
    ''' </summary>
    ''' <param name="r_BankGuarantee"></param>
    ''' <param name="v_iKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub GetBankGuarantee(ByRef r_BankGuarantee As BankGuarantee,
                                               ByVal v_iKey As Integer,
                                               Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock


        End SyncLock

    End Sub

    'Start WPR 85 & WPR 12
#Region "WPR85 & WPR12"
    Public Overrides Function GetCashDeposit(ByVal v_sPartyCode As String,
                                             ByVal v_sCashDepositRef As String,
                                             Optional ByVal v_sBranchCode As String = Nothing) As CashDeposit

        SyncLock oLock


            Return New CashDeposit

        End SyncLock
    End Function

#End Region 'End WPR 85 & WPR 12


    Public Overrides Function FindCashListReceipts(ByVal oCashListReceipt As CashListReceipt,
                                              Optional ByVal v_sBranchCode As String = Nothing) As CashListReceipts

        SyncLock oLock


            Dim oFindCashListReceiptsRequestType As FindCashListReceiptsQuery
            Dim oFindCashListReceiptsResponseType As FindCashListReceiptsQueryResponse
            Dim oCashListReceiptsCollection As CashListReceipts
            Dim iRowID As Integer = 10
            Dim oNewReceipt As CashListReceipt
            Try

                oFindCashListReceiptsRequestType = New FindCashListReceiptsQuery
                oFindCashListReceiptsResponseType = New FindCashListReceiptsQueryResponse

                With oFindCashListReceiptsRequestType
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty 
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If

                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    If String.IsNullOrEmpty(oCashListReceipt.BankAccountCode) Then
                        .BankAccountCode = Nothing
                    Else
                        .BankAccountCode = oCashListReceipt.BankAccountCode
                    End If

                    .CollectionDateFrom = oCashListReceipt.CollectionDateFrom
                    .CollectionDateFromFieldSpecified = oCashListReceipt.CollectionDateFromSpecified
                    .CollectionDateTo = oCashListReceipt.CollectionDateTo
                    .CollectionDateToFieldSpecified = oCashListReceipt.CollectionDateToSpecified

                    If String.IsNullOrEmpty(oCashListReceipt.DocumentRef) Then
                        .DocumentRef = Nothing
                    Else
                        .DocumentRef = oCashListReceipt.DocumentRef
                    End If

                    If String.IsNullOrEmpty(oCashListReceipt.DrawnBankCode) Then
                        .DrawnBankCode = Nothing
                    Else
                        .DrawnBankCode = oCashListReceipt.DrawnBankCode
                    End If

                    If String.IsNullOrEmpty(oCashListReceipt.InsuranceRef) Then
                        .InsuranceRef = Nothing
                    Else
                        .InsuranceRef = oCashListReceipt.InsuranceRef
                    End If

                    If String.IsNullOrEmpty(oCashListReceipt.MediaReference) Then
                        .MediaReference = Nothing
                    Else
                        .MediaReference = oCashListReceipt.MediaReference
                    End If

                    If String.IsNullOrEmpty(oCashListReceipt.MediaTypeStatusCode) Then
                        .MediaTypeStatusCode = Nothing
                    Else
                        .MediaTypeStatusCode = oCashListReceipt.MediaTypeStatusCode
                    End If

                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.FindCashListReceipts, oFindCashListReceiptsRequestType)
                    oFindCashListReceiptsResponseType = ApiClient.DeserializeJson(Of FindCashListReceiptsQueryResponse)(result)
                End Using

                With oFindCashListReceiptsResponseType

                    oCashListReceiptsCollection = New CashListReceipts

                    If .CashListItems IsNot Nothing AndAlso .CashListItems.Count > 0 Then

                        For Each oReceipt As BaseFindCashListReceiptsResponseTypeRow In .CashListItems
                            oNewReceipt = New CashListReceipt
                            With oReceipt
                                iRowID += 2
                                oNewReceipt.CashListReceiptRowID = iRowID
                                oNewReceipt.BranchDescription = .BranchDescription
                                oNewReceipt.CashListItemKey = .CashListItemKey
                                oNewReceipt.ClientCode = .ClientCode
                                oNewReceipt.ClientName = .ClientName
                                oNewReceipt.DocumentRef = .DocumentRef
                                oNewReceipt.DrawnBankName = .DrawnBankName
                                oNewReceipt.InsuranceFileKey = .InsuranceFileKey
                                oNewReceipt.MediaReference = .MediaReference
                                oNewReceipt.MediaTypeDescription = .MediaTypeDescription
                                oNewReceipt.MediaTypeKey = .MediaTypeKey
                                oNewReceipt.MediaTypeStatusDescription = .MediaTypeStatusDescription
                                oNewReceipt.MediaTypeStatusKey = .MediaTypeStatusKey
                                oNewReceipt.MediaTypeStatusCode = .MediaTypeStatusCode
                                oNewReceipt.PolicyNumber = .PolicyNumber
                                oNewReceipt.MediaTypeCode = .MediaTypeCode
                                oNewReceipt.CurrentStatus = .CurrentStatus
                            End With
                            oCashListReceiptsCollection.Add(oNewReceipt)
                        Next
                    End If

                End With

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally

                oFindCashListReceiptsRequestType = Nothing
                oFindCashListReceiptsRequestType = Nothing
            End Try

            Return oCashListReceiptsCollection
        End SyncLock
    End Function

    ''' <summary>
    ''' This method is used to get the cash list item details for payment.
    ''' </summary>
    ''' <param name="v_nCashListItemKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetPaymentTypeCashListItem(ByVal v_nCashListItemKey As Integer,
                                  Optional ByVal v_sBranchCode As String = Nothing) As NexusProvider.PaymentCashListItemTypeCollection


        SyncLock oLock

            Dim oGetPaymentTypeCashListItemRequest As GetPaymentTypeCashListItemQuery
            Dim oGetPaymentTypeCashListItemResponse As GetPaymentTypeCashListItemQueryResponse

            Dim oPaymentCashListItemsCollection As NexusProvider.PaymentCashListItemTypeCollection
            Dim oPaymentCashList As NexusProvider.PaymentCashListItemType
            Dim oPaymentCashListItem As NexusProvider.PaymentItems
            Try

                oGetPaymentTypeCashListItemRequest = New GetPaymentTypeCashListItemQuery
                oGetPaymentTypeCashListItemResponse = New GetPaymentTypeCashListItemQueryResponse

                With oGetPaymentTypeCashListItemRequest

                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .CashListItemKey = v_nCashListItemKey
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetPaymentTypeCashListItem, oGetPaymentTypeCashListItemRequest)
                    oGetPaymentTypeCashListItemResponse = ApiClient.DeserializeJson(Of GetPaymentTypeCashListItemQueryResponse)(result)
                End Using

                oPaymentCashListItemsCollection = New NexusProvider.PaymentCashListItemTypeCollection
                With oGetPaymentTypeCashListItemResponse

                    oPaymentCashListItemsCollection = New NexusProvider.PaymentCashListItemTypeCollection

                    For Each oCashList As BasePaymentCashListType In .CashList
                        oPaymentCashList = New NexusProvider.PaymentCashListItemType
                        oPaymentCashList.CoreCashList.BankAccountCode = oCashList.BankAccountCode
                        oPaymentCashList.CoreCashList.CurrencyCode = oCashList.CurrencyCode
                        oPaymentCashList.CoreCashList.ListDate = oCashList.ListDate
                        oPaymentCashList.CoreCashList.Reference = oCashList.Reference
                        oPaymentCashList.CoreCashList.StatusCode = oCashList.StatusCode
                        oPaymentCashList.CoreCashList.TypeCode = oCashList.TypeCode
                        oPaymentCashList.CoreCashList.BankAccountKey = oCashList.BankAccountKey

                        For Each oCashListItem As BasePaymentCashListItemType In oCashList.PaymentItem
                            oPaymentCashListItem = New NexusProvider.PaymentItems

                            oPaymentCashListItem.AccountShortCode = oCashListItem.AccountShortCode
                            If oCashListItem.ContactAddress IsNot Nothing Then
                                oPaymentCashListItem.Address = New NexusProvider.Address
                                oPaymentCashListItem.Address.Address1 = oCashListItem.ContactAddress.AddressLine1
                                oPaymentCashListItem.Address.Address2 = oCashListItem.ContactAddress.AddressLine2
                                oPaymentCashListItem.Address.Address3 = oCashListItem.ContactAddress.AddressLine3
                                oPaymentCashListItem.Address.Address4 = oCashListItem.ContactAddress.AddressLine4
                                oPaymentCashListItem.Address.CountryCode = oCashListItem.ContactAddress.CountryCode
                                oPaymentCashListItem.Address.PostCode = oCashListItem.ContactAddress.PostCode
                            End If
                            oPaymentCashListItem.AllocationStatusCode = oCashListItem.AllocationStatusCode
                            oPaymentCashListItem.Amount = oCashListItem.Amount

                            If oCashListItem.Bank IsNot Nothing Then
                                oPaymentCashListItem.Bank = New NexusProvider.Bank
                                oPaymentCashListItem.Bank.AccountCode = oCashListItem.Bank.AccountCode
                                oPaymentCashListItem.Bank.BranchCode = oCashListItem.Bank.BranchCode
                                oPaymentCashListItem.Bank.ExpiryDate = oCashListItem.Bank.ExpiryDate
                                oPaymentCashListItem.Bank.ExpiryDateSpecified = oCashListItem.Bank.ExpiryDateSpecified
                                oPaymentCashListItem.Bank.PartyBankKey = oCashListItem.Bank.PartyBankKey
                                oPaymentCashListItem.Bank.PayeeName = oCashListItem.Bank.PayeeName
                                oPaymentCashListItem.Bank.Reference1 = oCashListItem.Bank.Reference1
                                oPaymentCashListItem.Bank.Reference2 = oCashListItem.Bank.Reference2
                                oPaymentCashListItem.Bank.BIC = oCashListItem.Bank.BIC
                                oPaymentCashListItem.Bank.IBAN = oCashListItem.Bank.IBAN
                            End If
                            oPaymentCashListItem.BankReference = oCashListItem.BankReference
                            oPaymentCashListItem.CashListItemKey = oCashListItem.CashListItemKey
                            oPaymentCashListItem.ContactName = oCashListItem.ContactName
                            If oCashListItem.CreditCard IsNot Nothing AndAlso Not String.IsNullOrEmpty(oCashListItem.CreditCard.Number) Then
                                oPaymentCashListItem.CreditCard = New NexusProvider.CreditCard
                                If oCashListItem.CreditCard.CardHolder IsNot Nothing Then
                                    oPaymentCashListItem.CreditCard.Address1 = oCashListItem.CreditCard.CardHolder.AddressLine1
                                    oPaymentCashListItem.CreditCard.Address2 = oCashListItem.CreditCard.CardHolder.AddressLine2
                                    oPaymentCashListItem.CreditCard.Address3 = oCashListItem.CreditCard.CardHolder.AddressLine3
                                    oPaymentCashListItem.CreditCard.Address4 = oCashListItem.CreditCard.CardHolder.AddressLine4
                                    oPaymentCashListItem.CreditCard.CountryCode = oCashListItem.CreditCard.CardHolder.CountryCode
                                    oPaymentCashListItem.CreditCard.Name = oCashListItem.CreditCard.CardHolder.Name
                                    oPaymentCashListItem.CreditCard.PostCode = oCashListItem.CreditCard.CardHolder.PostCode
                                End If
                                oPaymentCashListItem.CreditCard.AuthCode = oCashListItem.CreditCard.AuthCode
                                oPaymentCashListItem.CreditCard.CCIssue = oCashListItem.CreditCard.Issue
                                oPaymentCashListItem.CreditCard.CCPin = oCashListItem.CreditCard.Pin
                                oPaymentCashListItem.CreditCard.CCSlipNumber = oCashListItem.CreditCard.TransactionSlipNumber
                                oPaymentCashListItem.CreditCard.ExpiryDate = oCashListItem.CreditCard.ExpiryDate
                                oPaymentCashListItem.CreditCard.Issue = oCashListItem.CreditCard.Issue
                                oPaymentCashListItem.CreditCard.Key = oCashListItem.CreditCard.TrackingNumber
                                oPaymentCashListItem.CreditCard.ManualAuthCode = oCashListItem.CreditCard.ManualAuthCode
                                oPaymentCashListItem.CreditCard.NameOnCreditCard = oCashListItem.CreditCard.NameOnCreditCard
                                oPaymentCashListItem.CreditCard.Number = oCashListItem.CreditCard.Number
                                oPaymentCashListItem.CreditCard.Pin = oCashListItem.CreditCard.Pin
                                oPaymentCashListItem.CreditCard.StartDate = oCashListItem.CreditCard.StartDate
                                oPaymentCashListItem.CreditCard.TransactionCode = oCashListItem.CreditCard.TransactionCode
                                oPaymentCashListItem.CreditCard.TypeCode = oCashListItem.CreditCard.TypeCode
                            End If
                            oPaymentCashListItem.FurtherDetails = oCashListItem.FurtherDetails
                            oPaymentCashListItem.IsProduceDocument = oCashListItem.IsProduceDocument
                            oPaymentCashListItem.MediaReference = oCashListItem.MediaReference
                            oPaymentCashListItem.MediaTypeCode = oCashListItem.MediaTypeCode
                            oPaymentCashListItem.OurReference = oCashListItem.OurReference
                            oPaymentCashListItem.SkipPosting = oCashListItem.SkipPosting
                            oPaymentCashListItem.StatusCode = oCashListItem.StatusCode
                            oPaymentCashListItem.TaxAmount = oCashListItem.TaxAmount
                            oPaymentCashListItem.TaxBandCode = oCashListItem.TaxBandCode
                            oPaymentCashListItem.TaxBandKey = oCashListItem.TaxBandKey
                            oPaymentCashListItem.TheirReference = oCashListItem.TheirReference
                            oPaymentCashListItem.TransactionDate = oCashListItem.TransactionDate
                            oPaymentCashListItem.TypeCode = oCashListItem.TypeCode

                            oPaymentCashList.PaymentItems.Add(oPaymentCashListItem)
                        Next

                        oPaymentCashListItemsCollection.Add(oPaymentCashList)
                    Next
                End With

                If Logger.IsLoggingEnabled Then
                    Dim sbLogMessage As New StringBuilder
                    sbLogMessage.AppendLine("GetPaymentTypeCashListItem executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If
                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally

                oGetPaymentTypeCashListItemRequest = Nothing
                oGetPaymentTypeCashListItemResponse = Nothing
                oPaymentCashList = Nothing
                oPaymentCashListItem = Nothing
            End Try

            Return oPaymentCashListItemsCollection

        End SyncLock
    End Function

    Public Overrides Sub ReverseCashListItem(ByVal v_TransdetailKey As Integer,
                                          ByVal v_CashListItemKey As Integer, ByVal v_ReverseReasonCode As String,
                                          Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock

            Dim oCancelReceiptRequestType As CancelReceiptCommand
            Dim oCancelReceiptResponseType As CancelReceiptCommandResponse

            Try

                oCancelReceiptRequestType = New CancelReceiptCommand
                oCancelReceiptResponseType = New CancelReceiptCommandResponse

                With oCancelReceiptRequestType
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .CashListItemKey = v_CashListItemKey
                    .TransDetailKey = v_TransdetailKey
                    .ReverseReasonCode = v_ReverseReasonCode
                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Post(ApiMethods.AddParty, oCancelReceiptRequestType)
                    oCancelReceiptResponseType = ApiClient.DeserializeJson(Of CancelReceiptCommandResponse)(result)
                End Using

                With oCancelReceiptResponseType
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If

                End With
                If Logger.IsLoggingEnabled Then
                    Dim sbLogMessage As StringBuilder
                    sbLogMessage = New StringBuilder

                    sbLogMessage.AppendLine("CancelReceipt executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If
                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally

                oCancelReceiptRequestType = Nothing
                oCancelReceiptResponseType = Nothing
            End Try


        End SyncLock
    End Sub

    ''' <summary>
    ''' This method changes the status of the Risk
    ''' </summary>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="v_iRiskKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub UpdateRiskStatus(ByVal v_iInsuranceFileKey As Integer,
                                     ByVal v_iRiskKey As Integer, ByVal v_RiskStatusType As NexusProvider.RiskStatusType,
                                      Optional ByVal v_sBranchCode As String = Nothing, Optional ByVal v_dtRiskInceptionDate As DateTime = Nothing,
                                          Optional ByRef o_nTimeStamp As Byte() = Nothing)
        SyncLock oLock

            Dim oUpdateRiskStatusRequest As UpdateRiskStatusCommand
            Dim oUpdateRiskStatusResponse As UpdateRiskStatusCommandResponse
            Dim sbLogMessage As StringBuilder

            Try


                oUpdateRiskStatusRequest = New UpdateRiskStatusCommand
                oUpdateRiskStatusResponse = New UpdateRiskStatusCommandResponse
                sbLogMessage = New StringBuilder

                With oUpdateRiskStatusRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    ' if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        ' if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            '  Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            ' Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        ' use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    If v_iInsuranceFileKey > 0 Then
                        .InsuranceFileKey = v_iInsuranceFileKey
                        .InsuranceFileKeySpecified = True
                    Else
                        .InsuranceFileKeySpecified = False
                    End If

                    If v_iRiskKey > 0 Then
                        .RiskKey = v_iRiskKey
                        .RiskKeySpecified = True
                        If v_dtRiskInceptionDate <> DateTime.MinValue Then
                            .RiskInceptionDateSpecified = True
                            .RiskInceptionDate = v_dtRiskInceptionDate
                        Else
                            .RiskInceptionDateSpecified = False
                        End If

                    Else
                        .RiskKeySpecified = False
                    End If
                    .RiskStatusCode = v_RiskStatusType

                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Put(ApiMethods.UpdateRiskStatus, oUpdateRiskStatusRequest)
                    oUpdateRiskStatusResponse = ApiClient.DeserializeJson(Of UpdateRiskStatusCommandResponse)(result)
                End Using
                If oUpdateRiskStatusResponse.UpdateRiskStatusResponse IsNot Nothing Then
                    With oUpdateRiskStatusResponse.UpdateRiskStatusResponse
                        If .Errors IsNot Nothing Then
                            'Process the error object if errors, and throw as a single exception
                            Throw New NexusException(.Errors)
                        End If
                    End With
                End If


                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally

                oUpdateRiskStatusResponse = Nothing
                oUpdateRiskStatusRequest = Nothing
            End Try


        End SyncLock
    End Sub

    ''' <summary>
    ''' This method will get the search transaction selected column
    ''' </summary>
    ''' <param name="sDefaultBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Function GetUserPreferredColumnList(Optional ByVal sDefaultBranchCode As String = Nothing, Optional ByVal v_sInterfaceName As String = Nothing) As UserPreferredColumnList
        SyncLock oLock

            Dim oGetUserPreferredColumnListRequestType As GetUserPreferredColumnListQuery
            Dim oGetUserPreferredColumnListResponseType As GetUserPreferredColumnListQueryResponse
            Dim oUserPreferredColumns As UserPreferredColumnList
            Try


                oUserPreferredColumns = New UserPreferredColumnList()
                oGetUserPreferredColumnListRequestType = New GetUserPreferredColumnListQuery
                oGetUserPreferredColumnListResponseType = New GetUserPreferredColumnListQueryResponse

                With oGetUserPreferredColumnListRequestType
                    .LoginUserName = Current.Session(CNLoginName)
                    .UserName = Current.Session(CNLoginName)
                    .InterfaceName = v_sInterfaceName
                    .UserName = Current.Session(CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(sDefaultBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = sDefaultBranchCode
                    End If
                End With
                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetUserPreferredColumnList, oGetUserPreferredColumnListRequestType)
                    oGetUserPreferredColumnListResponseType = ApiClient.DeserializeJson(Of GetUserPreferredColumnListQueryResponse)(result)
                End Using

                With oGetUserPreferredColumnListResponseType
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If
                End With
                If (oGetUserPreferredColumnListResponseType.ColumnList <> "") Then
                    With oUserPreferredColumns
                        .ColumnList = oGetUserPreferredColumnListResponseType.ColumnList
                    End With
                End If
                Return oUserPreferredColumns
            Catch ex As Exception
                Throw
            Finally

                oGetUserPreferredColumnListRequestType = Nothing
                oGetUserPreferredColumnListResponseType = Nothing
                oUserPreferredColumns = Nothing
            End Try
        End SyncLock
    End Function

    ''' <summary>
    ''' This method will update the search transaction selected column
    ''' </summary>
    ''' <param name="oUserPreferredColumns"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub UpdateUserPreferredColumnList(ByRef oUserPreferredColumns As UserPreferredColumnList, Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock

            Dim oUpdateUserPreferredColumnListRequestType As UpdateUserPreferredColumnListCommand
            Dim oUpdateUserPreferredColumnListResponseType As UpdateUserPreferredColumnListCommandResponse
            Try

                oUpdateUserPreferredColumnListRequestType = New UpdateUserPreferredColumnListCommand
                oUpdateUserPreferredColumnListResponseType = New UpdateUserPreferredColumnListCommandResponse

                oUpdateUserPreferredColumnListRequestType.UserName = Current.Session(CNLoginName)

                If String.IsNullOrEmpty(v_sBranchCode) Then
                    'if the branch code is NOT in session 
                    If String.IsNullOrEmpty(sBranchCode) Then
                        'Use the default branch code
                        oUpdateUserPreferredColumnListRequestType.BranchCode = v_sBranchCode
                    Else
                        'Use the branch code in session 
                        oUpdateUserPreferredColumnListRequestType.BranchCode = sBranchCode
                    End If
                Else
                    'use the passed parameter v_sBranchCode
                    oUpdateUserPreferredColumnListRequestType.BranchCode = v_sBranchCode
                End If

                oUpdateUserPreferredColumnListRequestType.InterfaceName = oUserPreferredColumns.InterfaceName
                oUpdateUserPreferredColumnListRequestType.ColumnList = oUserPreferredColumns.ColumnList
                'oUpdateUserPreferredColumnListRequestType.UserName = oUserPreferredColumns.UserName

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Put(ApiMethods.UpdateUserPreferredColumnList, oUpdateUserPreferredColumnListRequestType)
                    oUpdateUserPreferredColumnListResponseType = ApiClient.DeserializeJson(Of UpdateUserPreferredColumnListCommandResponse)(result)
                End Using

                With oUpdateUserPreferredColumnListResponseType
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If
                End With
            Catch ex As Exception
                Throw
            Finally

                oUpdateUserPreferredColumnListRequestType = Nothing
                oUpdateUserPreferredColumnListResponseType = Nothing
            End Try
        End SyncLock
    End Sub

    ''' <summary>
    ''' This method is used to retrieve the referred payments.
    ''' </summary>
    ''' <param name="v_oAuthorisedPaymentList"></param>
    ''' <returns></returns>
    Public Overrides Function GetListofUnapprovedPayment(Optional ByVal v_oAuthorisedPaymentList As NexusProvider.AutorisedPaymentRequestType = Nothing) As NexusProvider.AuthorisedPaymentCollection
        SyncLock oLock

            Dim oGetListofUnapprovedPaymentRequestType As GetListofUnapprovedPaymentQuery  'Request Type
            Dim oGetListofUnapprovedPaymentResponseType As GetListofUnapprovedPaymentQueryResponse 'Response Type

            Dim oAuthorisedPaymentList As AuthorisedPaymentList
            Dim oAuthorisedPaymentCollection = New NexusProvider.AuthorisedPaymentCollection
            Dim sbLogMessage As StringBuilder

            Try

                oGetListofUnapprovedPaymentRequestType = New GetListofUnapprovedPaymentQuery
                oGetListofUnapprovedPaymentResponseType = New GetListofUnapprovedPaymentQueryResponse
                oAuthorisedPaymentList = New AuthorisedPaymentList
                sbLogMessage = New StringBuilder

                With oGetListofUnapprovedPaymentRequestType
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    .Branch = v_oAuthorisedPaymentList.Branch
                    .AssignedTo = v_oAuthorisedPaymentList.AssignedTo
                    .CashListItemKey = v_oAuthorisedPaymentList.CashListItemKey
                    .CreatedBy = v_oAuthorisedPaymentList.CreatedBy
                    .DateFrom = v_oAuthorisedPaymentList.DateFrom
                    .DateTo = v_oAuthorisedPaymentList.DateTo
                    .PayeeName = v_oAuthorisedPaymentList.PayeeName
                    .PaymentType = v_oAuthorisedPaymentList.PaymentType
                    .ShowAllOtherPayments = v_oAuthorisedPaymentList.ShowAllOtherPayments
                    .BranchCode = v_oAuthorisedPaymentList.Branch

                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetListofUnapprovedPayment, oGetListofUnapprovedPaymentRequestType)
                    oGetListofUnapprovedPaymentResponseType = ApiClient.DeserializeJson(Of GetListofUnapprovedPaymentQueryResponse)(result)
                End Using

                With oGetListofUnapprovedPaymentResponseType.GetListofUnapprovedPaymentResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        If .ListofUnapprovedPayment IsNot Nothing AndAlso .ListofUnapprovedPayment.Count > 0 Then
                            For Each oCashListItemTypeRow As BaseGetListofUnapprovedPaymentResponseRowType In .ListofUnapprovedPayment
                                oAuthorisedPaymentList = New AuthorisedPaymentList
                                With oAuthorisedPaymentList
                                    .Amount = oCashListItemTypeRow.Amount
                                    .Assignedto = oCashListItemTypeRow.Assignedto
                                    .BankAccount = oCashListItemTypeRow.BankAccount
                                    .BaseCurrencyAmount = oCashListItemTypeRow.BaseCurrencyAmount
                                    .Branch = oCashListItemTypeRow.Branch
                                    .ClaimRef = oCashListItemTypeRow.ClaimRef
                                    .CreatedBy = oCashListItemTypeRow.CreatedBy
                                    .Currency = oCashListItemTypeRow.Currency
                                    .DateAssigned = If(oCashListItemTypeRow.DateAssigned = DateTime.MinValue, "", oCashListItemTypeRow.DateAssigned.ToString("d"))
                                    .MediaRef = oCashListItemTypeRow.MediaRef
                                    .MediaType = oCashListItemTypeRow.MediaType
                                    .PayeeAccountName = oCashListItemTypeRow.PayeeAccountName
                                    .PaymentType = oCashListItemTypeRow.PaymentType
                                    .PolicyRef = oCashListItemTypeRow.PolicyRef
                                    .TransactionDate = oCashListItemTypeRow.TransactionDate.Date
                                    .CashListId = oCashListItemTypeRow.CashListId
                                    .CashListItemId = oCashListItemTypeRow.CashListItemId
                                    .Status = oCashListItemTypeRow.Status
                                End With
                                oAuthorisedPaymentCollection.Add(oAuthorisedPaymentList)
                            Next
                        End If
                    End If

                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetReferredPayments executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("Output : " & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally

                oGetListofUnapprovedPaymentRequestType = Nothing
                oGetListofUnapprovedPaymentResponseType = Nothing
            End Try

            Return oAuthorisedPaymentCollection
        End SyncLock

    End Function


    ''' <summary>
    ''' This method is used to update the comment provided while payment authorization
    ''' </summary>
    ''' <param name="oUpdateAuthorizationComment"></param>
    ''' <param name="sDefaultBranchCode"></param>
    Public Overrides Sub UpdateAuthorizationComment(ByRef oUpdateAuthorizationComment As UpdateAuthorizationComment, Optional ByVal sDefaultBranchCode As String = Nothing)
        SyncLock oLock

            Dim oUpdateAuthorizationCommentRequestType As UpdateAuthorizationCommentCommand
            Dim oUpdateAuthorizationCommentResponseType As UpdateAuthorizationCommentCommandResponse
            Try

                oUpdateAuthorizationCommentRequestType = New UpdateAuthorizationCommentCommand()
                oUpdateAuthorizationCommentResponseType = New UpdateAuthorizationCommentCommandResponse()

                oUpdateAuthorizationCommentRequestType.LoginUserName = Current.Session(CNLoginName)

                If String.IsNullOrEmpty(sDefaultBranchCode) Then
                    'if the branch code is NOT in session 
                    If String.IsNullOrEmpty(sBranchCode) Then
                        'Use the default branch code
                        oUpdateAuthorizationCommentRequestType.BranchCode = sDefaultBranchCode
                    Else
                        'Use the branch code in session 
                        oUpdateAuthorizationCommentRequestType.BranchCode = sBranchCode
                    End If
                Else
                    'use the passed parameter v_sBranchCode
                    oUpdateAuthorizationCommentRequestType.BranchCode = sDefaultBranchCode
                End If

                With oUpdateAuthorizationCommentRequestType
                    .CashListItemId = oUpdateAuthorizationComment.CashListItem_id
                    .Comment = oUpdateAuthorizationComment.Comment
                End With

                oUpdateAuthorizationCommentRequestType.LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Put(ApiMethods.UpdateAuthorizationComment, oUpdateAuthorizationCommentRequestType)
                    oUpdateAuthorizationCommentResponseType = ApiClient.DeserializeJson(Of UpdateAuthorizationCommentCommandResponse)(result)
                End Using

                With oUpdateAuthorizationCommentResponseType
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If
                End With
            Catch ex As Exception
                Throw
            Finally

                oUpdateAuthorizationCommentRequestType = Nothing
                oUpdateAuthorizationCommentResponseType = Nothing
            End Try
        End SyncLock

    End Sub
    ''' <summary>
    ''' This method is used to get comments from the database that already exists
    ''' </summary>
    ''' <param name="sDefaultBranchCode"></param>
    ''' <param name="CashListItem_id"></param>
    ''' <returns></returns>
    Public Overrides Function GetAuthorizationComment(ByVal CashListItem_Id As Integer, Optional ByVal sDefaultBranchCode As String = Nothing) As GetAuthorizationComment
        SyncLock oLock

            Dim oGetAuthorizationCommentRequestType As GetAuthorizationCommentQuery
            Dim oGetAuthorizationCommentResponseType As GetAuthorizationCommentQueryResponse
            Dim oGetAuthorizationComment As GetAuthorizationComment
            Try

                oGetAuthorizationComment = New GetAuthorizationComment()
                oGetAuthorizationCommentRequestType = New GetAuthorizationCommentQuery
                oGetAuthorizationCommentResponseType = New GetAuthorizationCommentQueryResponse

                With oGetAuthorizationCommentRequestType
                    .LoginUserName = Current.Session(CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(sDefaultBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = sDefaultBranchCode
                    End If
                    If (CashListItem_Id <> 0) Then
                        oGetAuthorizationCommentRequestType.CashListItem_id = CashListItem_Id
                    End If
                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetAuthorizationComment, oGetAuthorizationCommentRequestType)
                    oGetAuthorizationCommentResponseType = ApiClient.DeserializeJson(Of GetAuthorizationCommentQueryResponse)(result)
                End Using

                If (oGetAuthorizationCommentResponseType.Authorization_Comment <> "") Then
                    With oGetAuthorizationComment
                        .Authorization_Comment = oGetAuthorizationCommentResponseType.Authorization_Comment
                    End With
                End If
                With oGetAuthorizationCommentResponseType
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If
                End With
                Return oGetAuthorizationComment
            Catch ex As Exception
                Throw
            Finally

                oGetAuthorizationCommentRequestType = Nothing
                oGetAuthorizationCommentResponseType = Nothing
                'oGetAuthorizationComment = Nothing
            End Try
        End SyncLock
    End Function

    ''' <summary>
    ''' This method is used to retrieve the referred payments.
    ''' </summary>
    ''' <param name="v_oManualJournalTransactionsList"></param>
    ''' <returns></returns>
    Public Overrides Function GetListofManualJournalTransactions(Optional ByVal v_oManualJournalTransactionsList As NexusProvider.AuthorisedManualJournalTransactionRequestType = Nothing) As NexusProvider.ManualJournalTransactionsCollection 'AuthorisedPaymentCollection
        SyncLock oLock


            Dim oGetListofManualJournalTransactionsRequestType As GetListOfManualJournalTransactionsQuery  'Request Type
            Dim oGetListofManualJournalTransactionsResponseType As GetListOfManualJournalTransactionsQueryResponse 'Response Type

            Dim oAuthorisedManualJournalTransactionsList As AuthorisedManualJournalTransactionsList
            Dim oManualJournalTransactionsCollection = New NexusProvider.ManualJournalTransactionsCollection
            Dim sbLogMessage As StringBuilder

            Try

                oGetListofManualJournalTransactionsRequestType = New GetListOfManualJournalTransactionsQuery
                oGetListofManualJournalTransactionsResponseType = New GetListOfManualJournalTransactionsQueryResponse
                oAuthorisedManualJournalTransactionsList = New AuthorisedManualJournalTransactionsList
                sbLogMessage = New StringBuilder

                With oGetListofManualJournalTransactionsRequestType
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    .AccountCode = v_oManualJournalTransactionsList.AccountCode
                    If v_oManualJournalTransactionsList.JournalTypeCode <> "All" Then
                        .JournalTypeCode = v_oManualJournalTransactionsList.JournalTypeCode
                    End If
                    .DateFrom = v_oManualJournalTransactionsList.DateFrom
                    .DateTo = v_oManualJournalTransactionsList.DateTo
                    If String.IsNullOrEmpty(sBranchCode) Then
                        'Use the default branch code
                        .BranchCode = sDefaultBranchCode
                    Else
                        'Use the branch code in session 
                        .BranchCode = sBranchCode
                    End If

                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetListOfManualJournalTransactions, oGetListofManualJournalTransactionsRequestType)
                    oGetListofManualJournalTransactionsResponseType = ApiClient.DeserializeJson(Of GetListOfManualJournalTransactionsQueryResponse)(result)
                End Using

                With oGetListofManualJournalTransactionsResponseType
                    If .ManualJournalTransactions IsNot Nothing AndAlso .ManualJournalTransactions.Count > 0 Then
                        For Each oJournalItemTypeRow As BaseGetListOfManualJournalTransactionsResponse In .ManualJournalTransactions
                            oAuthorisedManualJournalTransactionsList = New AuthorisedManualJournalTransactionsList
                            With oAuthorisedManualJournalTransactionsList
                                .Amount = oJournalItemTypeRow.Amount
                                .AccountCode = oJournalItemTypeRow.AccountCode
                                .AlternateRef = oJournalItemTypeRow.AlternateRef
                                .BaseAmount = oJournalItemTypeRow.BaseAmountRate
                                .Comment = oJournalItemTypeRow.Comment
                                .CostCenterId = oJournalItemTypeRow.CostCenterId
                                .Currency = oJournalItemTypeRow.Currency
                                .CurrencyRate = oJournalItemTypeRow.CurrencyRate
                                .CurrencyCode = oJournalItemTypeRow.CurrencyCode
                                .InsuranceRef = oJournalItemTypeRow.InsuranceRef
                                .ManualJournalId = oJournalItemTypeRow.ManualJournalId
                                .PurchaseInvoiceNumber = oJournalItemTypeRow.PurchaseInvoiceNumber
                                .PurchaseOrderNumber = oJournalItemTypeRow.PurchaseOrderNumber
                                .UnderwritingYearId = oJournalItemTypeRow.UnderwritingYearId
                                .CreatedBy = oJournalItemTypeRow.CreatedBy
                                .CreatedDate = oJournalItemTypeRow.CreatedDate.ToShortDateString()
                                .Status = oJournalItemTypeRow.Status
                            End With
                            oManualJournalTransactionsCollection.Add(oAuthorisedManualJournalTransactionsList)
                        Next
                    End If
                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetReferredPayments executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("Output : " & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally

                oGetListofManualJournalTransactionsRequestType = Nothing
                oGetListofManualJournalTransactionsResponseType = Nothing
            End Try

            Return oManualJournalTransactionsCollection
        End SyncLock

    End Function

    ''' <summary>
    ''' This method is used to retrieve the referred payments.
    ''' </summary>
    ''' <param name="manualJournalId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetListOfManualJournalTransactionApprovalMaster(ByVal manualJournalId As Integer, Optional ByVal v_sBranchCode As String = Nothing) As NexusProvider.ManualJournalTransactionApprovalMasterCollection 'As NexusProvider.ManualJournalTransactionApprovalRequestType = Nothing) As NexusProvider.ManualJournalTransactionApprovalMasterCollection
        SyncLock oLock

            Dim oGetListofManualJournalTransactionsRequestType As GetListOfManualJournalTransactionMasterQueryBase  'Request Type
            Dim oGetListofManualJournalTransactionsResponseType As GetListOfManualJournalTransactionMasterQueryResponse 'Response Type

            Dim oAuthorisedManualJournalTransactionsList As ManualJournalTransactionsMasterList
            Dim oManualJournalTransactionsCollection = New NexusProvider.ManualJournalTransactionApprovalMasterCollection
            Dim sbLogMessage As StringBuilder

            Try

                oGetListofManualJournalTransactionsRequestType = New GetListOfManualJournalTransactionMasterQueryBase
                oGetListofManualJournalTransactionsResponseType = New GetListOfManualJournalTransactionMasterQueryResponse
                oAuthorisedManualJournalTransactionsList = New ManualJournalTransactionsMasterList
                sbLogMessage = New StringBuilder

                With oGetListofManualJournalTransactionsRequestType
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty 
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If
                    .ManualJournalId = manualJournalId
                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetListOfManualJournalTransactionMaster, oGetListofManualJournalTransactionsRequestType)
                    oGetListofManualJournalTransactionsResponseType = ApiClient.DeserializeJson(Of GetListOfManualJournalTransactionMasterQueryResponse)(result)
                End Using

                With oGetListofManualJournalTransactionsResponseType
                    If .Masters IsNot Nothing AndAlso .Masters.Count > 0 Then
                        For Each oJournalItemTypeRow As BaseGetListOfManualJournalTransactionMasterResponse In .Masters
                            oAuthorisedManualJournalTransactionsList = New ManualJournalTransactionsMasterList
                            With oAuthorisedManualJournalTransactionsList
                                .Branch = oJournalItemTypeRow.Branch
                                .AuthorisationComment = oJournalItemTypeRow.AuthorisationComment
                                .Comment = oJournalItemTypeRow.Comment
                                .CreatedDate = oJournalItemTypeRow.CreatedDate
                                .DocumentType = oJournalItemTypeRow.DocumentType
                                .IsReferred = oJournalItemTypeRow.IsReferred
                                .PerMonthOnDay = oJournalItemTypeRow.PerMonthOnDay
                                .PerPeriodOnDay = oJournalItemTypeRow.PerPeriodOnDay
                                .PerQuarterOnDay = oJournalItemTypeRow.PerQuarterOnDay
                                .RecurringOccurs = oJournalItemTypeRow.RecurringOccurs
                                .ReversesOn = oJournalItemTypeRow.ReversesOn
                            End With
                            oManualJournalTransactionsCollection.Add(oAuthorisedManualJournalTransactionsList)
                        Next
                    End If

                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetReferredPayments executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("Output : " & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally

                oGetListofManualJournalTransactionsRequestType = Nothing
                oGetListofManualJournalTransactionsResponseType = Nothing
            End Try

            Return oManualJournalTransactionsCollection
        End SyncLock

    End Function
    ''' <summary>
    ''' This method is used to retrieve the referred payments.
    ''' </summary>
    ''' <param name="manualJournalId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetListOfManualJournalTransactionApprovalDetails(ByVal manualJournalId As Integer, Optional ByVal v_sBranchCode As String = Nothing) As NexusProvider.ManualJournalItemCollection
        SyncLock oLock

            Dim oGetListofManualJournalTransactionsRequestType As GetListOfManualJournalTransactionDetailsQuery  'Request Type
            Dim oGetListofManualJournalTransactionsResponseType As GetListOfManualJournalTransactionDetailsQueryResponse 'Response Type

            Dim oAuthorisedManualJournalTransactionsList As ManualJournalItem
            Dim oManualJournalTransactionsCollection = New NexusProvider.ManualJournalItemCollection
            Dim sbLogMessage As StringBuilder

            Try
                oGetListofManualJournalTransactionsRequestType = New GetListOfManualJournalTransactionDetailsQuery
                oGetListofManualJournalTransactionsResponseType = New GetListOfManualJournalTransactionDetailsQueryResponse
                oAuthorisedManualJournalTransactionsList = New ManualJournalItem
                sbLogMessage = New StringBuilder

                With oGetListofManualJournalTransactionsRequestType
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty 
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If
                    .ManualJournalId = manualJournalId

                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetListOfManualJournalTransactionDetails, oGetListofManualJournalTransactionsRequestType)
                    oGetListofManualJournalTransactionsResponseType = ApiClient.DeserializeJson(Of GetListOfManualJournalTransactionDetailsQueryResponse)(result)
                End Using

                With oGetListofManualJournalTransactionsResponseType

                    If .ManualJournalTransactionDetails IsNot Nothing AndAlso .ManualJournalTransactionDetails.Count > 0 Then
                        For Each oJournalItemTypeRow As BaseGetListOfManualJournalTransactionDetailsResponse In .ManualJournalTransactionDetails
                            oAuthorisedManualJournalTransactionsList = New ManualJournalItem
                            With oAuthorisedManualJournalTransactionsList
                                .ManualJournalDetailId = oJournalItemTypeRow.ManualJournalDetailId
                                .AccountKey = oJournalItemTypeRow.AccountCode
                                .Amount = oJournalItemTypeRow.Amount
                                .AccountName = oJournalItemTypeRow.AccountCode
                                .AltReference = oJournalItemTypeRow.AlternateRef
                                .BaseAmount = oJournalItemTypeRow.BaseAmount
                                .Comment = oJournalItemTypeRow.Comment
                                .CostCentreDescription = oJournalItemTypeRow.CostCentreDescription
                                .CurrencyTypeCode = oJournalItemTypeRow.CurrencyCode
                                .CurrencyTypeDescription = oJournalItemTypeRow.CurrencyTypeDescription
                                .CurrencyRate = oJournalItemTypeRow.CurrencyRate
                                .InsuranceRef = oJournalItemTypeRow.InsuranceRef
                                .PurchaseInvoiceNumber = oJournalItemTypeRow.PurchaseInvoiceNumber
                                .PurchaseOrderNumber = oJournalItemTypeRow.PurchaseOrderNumber
                                .UnderwritingYearDescription = oJournalItemTypeRow.UnderwritingYearDescription
                            End With
                            oManualJournalTransactionsCollection.Add(oAuthorisedManualJournalTransactionsList)
                        Next
                    End If

                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetReferredPayments executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("Output : " & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetListofManualJournalTransactionsRequestType = Nothing
                oGetListofManualJournalTransactionsResponseType = Nothing
            End Try

            Return oManualJournalTransactionsCollection
        End SyncLock

    End Function

    ''' This method is used to update the comment provided while payment authorization
    ''' <param name="oUpdateManualJournalApproversComment"></param>
    ''' <param name="sDefaultBranchCode"></param>
    Public Overrides Sub UpdateManualJournalApproversComment(ByRef oUpdateManualJournalApproversComment As UpdateManualJournalApproversComment, Optional ByVal sDefaultBranchCode As String = Nothing)
        SyncLock oLock
            Dim oUpdateManualJournalApproversCommentRequestType As UpdateManualJournalApproversCommentCommand
            Dim oUpdateManualJournalApproversCommentResponseType As UpdateManualJournalApproversCommentCommandResponse
            Try
                oUpdateManualJournalApproversCommentRequestType = New UpdateManualJournalApproversCommentCommand()
                oUpdateManualJournalApproversCommentResponseType = New UpdateManualJournalApproversCommentCommandResponse()

                oUpdateManualJournalApproversCommentRequestType.LoginUserName = Current.Session(CNLoginName)

                If String.IsNullOrEmpty(sDefaultBranchCode) Then
                    'if the branch code is NOT in session 
                    If String.IsNullOrEmpty(sBranchCode) Then
                        'Use the default branch code
                        oUpdateManualJournalApproversCommentRequestType.BranchCode = sDefaultBranchCode
                    Else
                        'Use the branch code in session 
                        oUpdateManualJournalApproversCommentRequestType.BranchCode = sBranchCode
                    End If
                Else
                    'use the passed parameter v_sBranchCode
                    oUpdateManualJournalApproversCommentRequestType.BranchCode = sDefaultBranchCode
                End If

                With oUpdateManualJournalApproversCommentRequestType
                    .ManualJournalId = oUpdateManualJournalApproversComment.ManualJournalId
                    .Comment = oUpdateManualJournalApproversComment.Comment
                End With

                oUpdateManualJournalApproversCommentRequestType.LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Post(ApiMethods.UpdateManualJournalApproversComment, oUpdateManualJournalApproversCommentRequestType)
                    oUpdateManualJournalApproversCommentResponseType = ApiClient.DeserializeJson(Of UpdateManualJournalApproversCommentCommandResponse)(result)
                End Using
                With oUpdateManualJournalApproversCommentResponseType
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If
                End With
            Catch ex As Exception
                Throw
            Finally
                oUpdateManualJournalApproversCommentRequestType = Nothing
                oUpdateManualJournalApproversCommentResponseType = Nothing
            End Try
        End SyncLock

    End Sub
    Public Overrides Function ValidateAuthorizationSteps(ByVal oManualJournal As NexusProvider.ManualJournal,
                                               Optional ByVal v_sBranchCode As String = Nothing) As ManualJournalCollection

        SyncLock oLock

            Dim oValidateAuthorizationStepsRequestType As ValidateAuthorizationStepsCommand
            Dim oValidateAuthorizationSteps As ValidateAuthorizationStepsCommandResponse
            Dim oManualJournalCollection As ManualJournalCollection = Nothing
            Dim sbLogMessage As StringBuilder
            Try
                oValidateAuthorizationStepsRequestType = New ValidateAuthorizationStepsCommand
                oValidateAuthorizationSteps = New ValidateAuthorizationStepsCommandResponse
                sbLogMessage = New StringBuilder

                With oValidateAuthorizationStepsRequestType
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty 
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .ManualJournalId = oManualJournal.ManualJournalId
                    .IsApproved = oManualJournal.Approved
                End With

                oManualJournalCollection = New ManualJournalCollection
                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Post(ApiMethods.ValidateAuthorizationSteps, oValidateAuthorizationStepsRequestType)
                    oValidateAuthorizationSteps = ApiClient.DeserializeJson(Of ValidateAuthorizationStepsCommandResponse)(result)
                End Using
                With oValidateAuthorizationSteps  'With Response Type
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        'WorkManager Response
                        'Fetching from the  WorkManager Response Collection 
                        If .ValidateAuthorizationSteps IsNot Nothing AndAlso .ValidateAuthorizationSteps.Count > 0 Then
                            For Each oValidateAuthorizationrow As BaseValidateAuthorizationStepsResponseType In .ValidateAuthorizationSteps
                                oManualJournal = New NexusProvider.ManualJournal
                                oManualJournal.CurrentStep = oValidateAuthorizationrow.CurrentStep
                                oManualJournal.IsLastStep = oValidateAuthorizationrow.IsLastStep
                                oManualJournal.ValidationMessage = oValidateAuthorizationrow.ValidationMessage
                                oManualJournal.PMUserGroup = oValidateAuthorizationrow.PMUserGroup
                                oManualJournal.JournalAmount = oValidateAuthorizationrow.JournalAmount
                                oManualJournalCollection.Add(oManualJournal)
                            Next
                        End If
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("oManualJournal executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & oManualJournal.Print() & vbCrLf)

                    sbLogMessage.AppendLine("Returned " & oManualJournalCollection.Print() & "results" & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oValidateAuthorizationStepsRequestType = Nothing
            End Try


            Return oManualJournalCollection  'Returning Audit Trail Module Collection
        End SyncLock

    End Function

End Class
