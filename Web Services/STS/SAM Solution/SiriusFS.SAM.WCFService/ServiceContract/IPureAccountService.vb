Imports SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.WCF

<ServiceContract(Namespace:="PureService")>
Public Interface IPureAccountService
    ''' <summary>
    ''' To Add Agent Receipt
    ''' </summary>
    ''' <param name="AddAgentReceiptRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
   <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddAgentReceipt(ByVal AddAgentReceiptRequest As AddAgentReceiptRequestType) As AddAgentReceiptResponseType

    ''' <summary>  
    '''  This method calls core implementation layer and creates manual journal transactions.
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.AddJournalRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.AddJournalResponseType</returns>  
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddJournal(ByVal oRequest As AddJournalRequestType) As AddJournalResponseType

    ''' <summary>
    '''This method passes the AddWriteOffRequestType request from web service layer to core implementation layer.
    '''It returns back the result from core implementation layer to web service layer.
    '''</summary>
    '''<param name="oRequest" type="SiriusFS.SAM.SFI.SAMForInsuranceV2.AddWriteOffRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.AddWriteOffResponseType</returns>
    '''<remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddWriteOff(ByVal oRequest As AddWriteOffRequestType) As AddWriteOffResponseType

    ''' <summary>
    ''' TO CreatePaymentCashListWithItems details provided in request
    ''' </summary>
    ''' <param name="CreatePaymentCashListWithItemsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function CreatePaymentCashListWithItems(ByVal CreatePaymentCashListWithItemsRequest As CreatePaymentCashListWithItemsRequestType) As CreatePaymentCashListWithItemsResponseType

    ''' <summary>
    ''' TO CreateReceiptCashListWithItems details provided in request
    ''' </summary>
    ''' <param name="CreateReceiptCashListWithItemsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function CreateReceiptCashListWithItems(ByVal CreateReceiptCashListWithItemsRequest As CreateReceiptCashListWithItemsRequestType) As CreateReceiptCashListWithItemsResponseType

    ''' <summary>  
    ''' This method is used to find accounts in claim payment
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.FindAccountsRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.FindAccountsResponseType</returns>  
    <OperationContract(Name:="FindAccounts")>
     <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindAccounts(ByVal oRequest As FindAccountsRequestType) As FindAccountsResponseType

    ''' <summary>
    ''' TO FindCashListReceipts details provided in request
    ''' </summary>
    ''' <param name="FindCashListReceiptsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function FindCashListReceipts(ByVal FindCashListReceiptsRequest As FindCashListReceiptsRequestType) As FindCashListReceiptsResponseType


    ''' <summary>
    ''' To get Party shortname from Account ID 
    ''' </summary>
    ''' <param name="oGetAccountShortCodeFromPartyRequest"></param>
    ''' <param name="v_iPartyCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetAccountShortCodeFromParty(ByVal oGetAccountShortCodeFromPartyRequest As GetAccountShortCodeFromPartyRequestType, ByVal v_iPartyCnt As Integer) As String


    ''' <summary>
    ''' TO Add a TaskGroup with details provided in request
    ''' </summary>
    ''' <param name="GetAccountBalanceRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetAccountBalance(ByVal GetAccountBalanceRequest As GetAccountBalanceRequestType) As GetAccountBalanceResponseType

    ''' <summary>
    ''' TO Get Account details 
    ''' </summary>
    ''' <param name="GetAccountDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetAccountDetails")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetAccountDetails(ByVal GetAccountDetailsRequest As GetAccountDetailsRequestType) As GetAccountDetailsResponseType

    ''' <summary>
    ''' TO Get Account Period with details provided in request
    ''' </summary>
    ''' <param name="GetAccountingPeriodRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetAccountingPeriod(ByVal GetAccountingPeriodRequest As GetAccountingPeriodRequestType) As GetAccountingPeriodResponseType


    ''' <summary>
    ''' This web services method is used to Get Allocation Details
    ''' </summary>
    ''' <param name="oGetAllocationDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetAllocationDetails(ByVal oGetAllocationDetailsRequest As GetAllocationDetailsRequestType) As GetAllocationDetailsResponseType

    ''' <summary>
    ''' GetBalancesAndUnallocatedCredits
    ''' </summary>
    ''' <param name="GetBalancesAndUnallocatedCreditsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetBalancesAndUnallocatedCredits(ByVal GetBalancesAndUnallocatedCreditsRequest As GetBalancesAndUnallocatedCreditsRequestType) As GetBalancesAndUnallocatedCreditsResponseType

    ''' <summary>
    ''' TO Get bank account details provided in request
    ''' </summary>
    ''' <param name="GetBankAccountsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetBankAccounts(ByVal GetBankAccountsRequest As GetBankAccountsRequestType) As GetBankAccountsResponseType

    ''' <summary>
    ''' GetCurrencyExchangeRates
    ''' </summary>
    ''' <param name="oGetCurrencyExchangeRatesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetCurrencyExchangeRates")>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetCurrencyExchangeRates(ByVal oGetCurrencyExchangeRatesRequest As GetCurrencyExchangeRatesRequestType) As GetCurrencyExchangeRatesResponseType

    ''' <summary>
    ''' TO Get Insurer Payments details provided in request
    ''' </summary>
    ''' <param name="GetInsurerPaymentsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetInsurerPayments(ByVal GetInsurerPaymentsRequest As GetInsurerPaymentsRequestType) As GetInsurerPaymentsResponseType



    ''' <summary>
    ''' TO Get Period details provided in request
    ''' </summary>
    ''' <param name="GetPeriodRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetPeriod(ByVal GetPeriodRequest As GetPeriodRequestType) As GetPeriodResponseType

    ''' <summary>
    ''' This Web method is used to get all the transaction details
    ''' </summary>
    ''' <param name="oGetTransactionDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
   <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetTransactionDetails(ByVal oGetTransactionDetailsRequest As GetTransactionDetailsRequestType) As GetTransactionDetailsResponseType

    ''' <summary>
    ''' TO MarkUnmarkTransaction details provided in request
    ''' </summary>
    ''' <param name="MarkUnmarkTransactionRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function MarkUnmarkTransaction(ByVal MarkUnmarkTransactionRequest As MarkUnmarkTransactionRequestType) As MarkUnmarkTransactionResponseType

    ''' <summary>
    ''' TO PostDocument details provided in request 
    ''' </summary>
    ''' <param name="PostDocumentrequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function PostDocument(ByVal PostDocumentRequest As PostDocumentRequestType) As PostDocumentResponseType

    ''' <summary>
    ''' This web services method is used to Add Address.
    '''  </summary>
    '''<param name = "ReverseAllocationRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function ReverseAllocation(ByVal ReverseAllocationRequest As ReverseAllocationRequestType) As ReverseAllocationResponseType

    ''' <summary>
    ''' TO Update Allocation details provided in request
    ''' </summary>
    ''' <param name="UpdateAllocationRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateAllocation(ByVal UpdateAllocationRequest As UpdateAllocationRequestType) As UpdateAllocationResponseType

    ''' <summary>
    ''' To Add a Bank Guarantee with details provided in request
    ''' </summary>
    ''' <param name="oAddBankGuaranteeRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddBankGuarantee(ByVal oAddBankGuaranteeRequest As AddBankGuaranteeRequestType) As AddBankGuaranteeResponseType

    ''' <summary>
    ''' To Add a Cash Deposit with details provided in request
    ''' </summary>
    ''' <param name="oAddCashDepositRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddCashDeposit(ByVal oAddCashDepositRequest As AddCashDepositRequestType) As AddCashDepositResponseType

    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="FindBankRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.FindBankResponseType</returns>  
    '''<remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindBank(ByVal oRequest As FindBankRequestType) As FindBankResponseType

    ''' <summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="FindBankGuaranteeRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.FindBankGuaranteeResponseType</returns>  
    '''<remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindBankGuarantee(ByVal oRequest As FindBankGuaranteeRequestType) As FindBankGuaranteeResponseType

    ''' <summary>
    ''' TO FindCashDeposit details provided in request
    ''' </summary>
    ''' <param name="FindCashDepositRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindCashDeposit(ByVal FindCashDepositRequest As FindCashDepositRequestType) As FindCashDepositResponseType

    ''' <summary>
    ''' GetBankGuarantee
    ''' </summary>
    ''' <param name="oGetBankGuaranteeRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetBankGuarantee(ByVal oGetBankGuaranteeRequest As GetBankGuaranteeRequestType) As GetBankGuaranteeResponseType

    ''' <summary>
    ''' TO GetCashDeposit details provided in request
    ''' </summary>
    ''' <param name="GetCashDepositRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetCashDeposit(ByVal GetCashDepositRequest As GetCashDepositRequestType) As GetCashDepositResponseType

    ''' <summary>
    ''' TO GetLinkedCashDeposits details provided in request
    ''' </summary>
    ''' <param name="GetLinkedCashDepositsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetLinkedCashDeposits(ByVal GetLinkedCashDepositsRequest As GetLinkedCashDepositsRequestType) As GetLinkedCashDepositsResponseType

    ''' <summary>
    ''' TO GetNextCashDepositRef details provided in request
    ''' </summary>
    ''' <param name="GetNextCashDepositRefRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetNextCashDepositRef(ByVal GetNextCashDepositRefRequest As GetNextCashDepositRefRequestType) As GetNextCashDepositRefResponseType

    ''' <summary>
    ''' TO GetPaymentCashListDetails details provided in request
    ''' </summary>
    ''' <param name="GetPaymentCashListDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetPaymentCashListDetails(ByVal GetPaymentCashListDetailsRequest As GetPaymentCashListDetailsRequestType) As GetPaymentCashListDetailsResponseType


    ''' <summary>
    ''' TO GetPaymentCashListItemDetails details provided in request
    ''' </summary>
    ''' <param name="GetPaymentCashListItemDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetPaymentCashListItemDetails(ByVal GetPaymentCashListItemDetailsRequest As GetPaymentCashListItemDetailsRequestType) As GetPaymentCashListItemDetailsResponseType

    ''' <summary>
    ''' TO GetPaymentCashListItems details provided in request
    ''' </summary>
    ''' <param name="GetPaymentCashListItemsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetPaymentCashListItems(ByVal GetPaymentCashListItemsRequest As GetPaymentCashListItemsRequestType) As GetPaymentCashListItemsResponseType

    ''' <summary>
    ''' TO GetReceiptCashListDetails details provided in request
    ''' </summary>
    ''' <param name="GetReceiptCashListDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetReceiptCashListDetails(ByVal GetReceiptCashListDetailsRequest As GetReceiptCashListDetailsRequestType) As GetReceiptCashListDetailsResponseType

    ''' <summary>
    ''' TO GetReceiptCashListItemDetails details provided in request
    ''' </summary>
    ''' <param name="GetReceiptCashListItemDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetReceiptCashListItemDetails(ByVal GetReceiptCashListItemDetailsRequest As GetReceiptCashListItemDetailsRequestType) As GetReceiptCashListItemDetailsResponseType

    ''' <summary>
    ''' TO GetReceiptCashListItems details provided in request
    ''' </summary>
    ''' <param name="GetReceiptCashListItemsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetReceiptCashListItems(ByVal GetReceiptCashListItemsRequest As GetReceiptCashListItemsRequestType) As GetReceiptCashListItemsResponseType

    ''' <summary>
    ''' TO Update Bank Guarantee details provided in request
    ''' </summary>
    ''' <param name="UpdateBankGuaranteeRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateBankGuarantee(ByVal UpdateBankGuaranteeRequest As UpdateBankGuaranteeRequestType) As UpdateBankGuaranteeResponseType


    ''' <summary>
    ''' TO Update Bank Guarantee Conditionally details provided in request
    ''' </summary>
    ''' <param name="UpdateBankGuaranteeConditionallyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateBankGuaranteeConditionally(ByVal UpdateBankGuaranteeConditionallyRequest As UpdateBankGuaranteeConditionallyRequestType) As UpdateBankGuaranteeConditionallyResponseType

    ''' <summary>
    ''' TO Update Cash Deposit details provided in request
    ''' </summary>
    ''' <param name="UpdateCashDepositRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateCashDeposit(ByVal UpdateCashDepositRequest As UpdateCashDepositRequestType) As UpdateCashDepositResponseType

    ''' <summary>
    ''' TO Update Receipt Media Type Status details provided in request
    ''' </summary>
    ''' <param name="UpdateReceiptMediaTypeStatusRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateReceiptMediaTypeStatus(ByVal UpdateReceiptMediaTypeStatusRequest As UpdateReceiptMediaTypeStatusRequestType) As UpdateReceiptMediaTypeStatusResponseType

    ''' <summary>
    ''' TO Validate Bank Account Number details provided in request 
    ''' </summary>
    ''' <param name="ValidateBankAccountNumberRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function ValidateBankAccountNumber(ByVal ValidateBankAccountNumberRequest As ValidateBankAccountNumberRequestType) As ValidateBankAccountNumberResponseType

    ''' <summary>
    ''' To search partied for given search criteria
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="FindParty")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindParty(ByVal oRequest As FindPartyRequestType) As FindPartyResponseType

    ''' <summary>
    ''' This web services method is used to Get Party Bank Details.
    '''  </summary>
    '''<param name = "GetPartyBankDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetPartyBankDetailsAccount")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetPartyBankDetails(ByVal GetPartyBankDetailsRequest As GetPartyBankDetailsRequestType) As GetPartyBankDetailsResponseType


    ''' <summary>
    ''' This web services method is used to Get Address.
    '''  </summary>
    '''<param name = "GetAddressRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetAddressAccount")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetAddress(ByVal GetAddressRequest As GetAddressRequestType) As GetAddressResponseType

    ''' <summary>
    ''' GetCurrenciesByBranch
    ''' </summary>
    ''' <param name="GetCurrenciesByBranchRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetCurrenciesByBranchAccount")>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetCurrenciesByBranch(ByVal GetCurrenciesByBranchRequest As GetCurrenciesByBranchRequestType) As GetCurrenciesByBranchResponseType

    ''' <summary>
    ''' TO Get List  details provided in request
    ''' </summary>
    ''' <param name="GetListRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetListAccount")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetList(ByVal GetListRequest As GetListRequestType) As GetListResponseType

    ''' <summary>
    ''' TO Get List  details from an ICCS SPU provided in request
    ''' </summary>
    ''' <param name="GetListRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetListSPUICCSAccount")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetListSPUICCS(ByVal GetListRequest As GetListRequestType) As GetListResponseType

    ''' <summary>
    ''' To get logged-in userdetail
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetUserDetailsAccount")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetUserDetails(ByVal oRequest As GetUserDetailsRequestType) As GetUserDetailsResponseType

    ''' <summary>
    ''' TO Get User Group Users details provided in request
    ''' </summary>
    ''' <param name="GetUserGroupUsersRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetUserGroupUsersAccount")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetUserGroupUsers(ByVal GetUserGroupUsersRequest As GetUserGroupUsersRequestType) As GetUserGroupUsersResponseType

    ''' <summary>
    ''' TO Get User Groups details provided in request 
    ''' </summary>
    ''' <param name="GetUserGroupsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetUserGroupsAccount")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetUserGroups(ByVal GetUserGroupsRequest As GetUserGroupsRequestType) As GetUserGroupsResponseType

    ''' <summary>
    ''' This Web method is used to get all the work manager scheduled tasks
    ''' </summary>
    ''' <param name="GetWorkManagerScheduledTasksRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetWorkManagerScheduledTasksAccount")>
      <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetWorkManagerScheduledTasks(ByVal GetWorkManagerScheduledTasksRequest As GetWorkManagerScheduledTasksRequestType) As GetWorkManagerScheduledTasksResponseType

    ''' <summary>
    ''' TO Get Option Setting details provided in request
    ''' </summary>
    ''' <param name="GetOptionSettingRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetOptionSettingAccount")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetOptionSetting(ByVal GetOptionSettingRequest As GetOptionSettingRequestType) As GetOptionSettingResponseType


    ''' <summary>
    ''' This web services method is used to Add Address.
    '''  </summary>
    '''<param name = "GetUserAuthorityValueRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetUserAuthorityValueAccount")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetUserAuthorityValue(ByVal GetUserAuthorityValueRequest As GetUserAuthorityValueRequestType) As GetUserAuthorityValueResponseType

    ''' <summary>
    ''' FindPaymentDetails
    ''' </summary>
    ''' <param name="FindPaymentDetailsRequest"></param>
    ''' <remarks></remarks>
    <OperationContract(Name:="FindPaymentDetails")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindPaymentDetails(ByVal FindPaymentDetailsRequest As FindPaymentDetailsRequestType) As FindPaymentDetailsResponseType

    ''' <summary>
    ''' CancelPayment
    ''' </summary>
    ''' <param name="CancelPaymentRequest"></param>
    ''' <remarks></remarks>
    <OperationContract(Name:="CancelPayment")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Sub CancelPayment(ByVal CancelPaymentRequest As CancelPaymentRequestType)

    ''' <summary>
    ''' FindReceiptDetails
    ''' </summary>
    ''' <param name="FindReceiptDetailsRequest"></param>
    ''' <remarks></remarks>
    <OperationContract(Name:="FindReceiptDetails")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindReceiptDetails(ByVal FindReceiptDetailsRequest As FindReceiptDetailsRequestType) As FindReceiptDetailsResponseType

    ''' <summary>
    ''' CancelReceipt
    ''' </summary>
    ''' <param name="CancelReceiptRequest"></param>
    ''' <remarks></remarks>
    <OperationContract(Name:="CancelReceipt")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function CancelReceipt(ByVal CancelReceiptRequest As CancelReceiptRequestType) As CancelReceiptResponseType

    ''' <summary>
    ''' GetPaymentTypeCashListItem
    ''' </summary>
    ''' <param name="oGetPaymentTypeCashListItemRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetPaymentTypeCashListItem")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetPaymentTypeCashListItem(ByVal oGetPaymentTypeCashListItemRequest As GetPaymentTypeCashListItemRequestType) As GetPaymentTypeCashListItemResponseType

    ''' <summary>
    ''' TO Get Policy Transaction details provided in request
    ''' </summary>
    ''' <param name="FindPolicyTransactionGroupedRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindPolicyTransactionGrouped(ByVal FindPolicyTransactionGroupedRequest As FindPolicyTransactionGroupedRequestType) As FindPolicyTransactionGroupedResponseType

    ''' <summary>
    ''' This Web method is used to get all the transaction details
    ''' </summary>
    ''' <param name="oGetTransactionDetailsExRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
   <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetTransactionDetailsEx(ByVal oGetTransactionDetailsExRequest As GetTransactionDetailsExRequestType) As GetTransactionDetailsExResponseType

    ''' <summary>
    ''' This Web method is used to get all the Policy transaction details
    ''' </summary>
    ''' <param name="GetPolicyTransactionDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
   <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetPolicyTransactionDetails(ByVal GetPolicyTransactionDetailsRequest As GetPolicyTransactionDetailsRequestType) As GetPolicyTransactionDetailsResponseType

    ''' <summary>
    ''' This Web method is used to reserve transaction details
    ''' </summary>
    ''' <param name="oReverseAllocationBatchRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
   <FaultContract(GetType(SAMMethodResponseData))> _
    Function ReverseAllocationBatch(ByVal oReverseAllocationBatchRequest As ReverseAllocationBatchRequestType) As ReverseAllocationBatchResponseType


    ''' <summary>
    ''' This Web method is used to Get Search Transaction selected column
    ''' </summary>
    ''' <param name="oGetUserPreferredColumnListRequestType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetUserPreferredColumnList(ByVal oGetUserPreferredColumnListRequestType As GetUserPreferredColumnListRequestType) As GetUserPreferredColumnListResponseType

    ''' <summary>
    ''' This Web method is used to Update Search Transaction selected column
    ''' </summary>
    ''' <param name="oUpdateUserPreferredColumnListRequestType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function UpdateUserPreferredColumnList(ByRef oUpdateUserPreferredColumnListRequestType As UpdateUserPreferredColumnListRequestType) As UpdateUserPreferredColumnListResponseType

    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetListofUnapprovedPayment(ByVal oGetListofUnapprovedPaymentRequestType As GetListofUnapprovedPaymentRequestType) As GetListofUnapprovedPaymentResponseType

    ''' <summary>
    ''' This Web method is used to Update Search Transaction selected column
    ''' </summary>
    ''' <param name="oUpdateAuthorizationCommentRequestType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function UpdateAuthorizationComment(ByRef oUpdateAuthorizationCommentRequestType As UpdateAuthorizationCommentRequestType) As UpdateAuthorizationCommentResponseType

    ''' <summary>
    ''' This web method is used to get the comments from CashlistItems
    ''' </summary>
    ''' <param name="oGetAuthorizationCommentRequestType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetAuthorizationComment(ByRef oGetAuthorizationCommentRequestType As GetAuthorizationCommentRequestType) As GetAuthorizationCommentResponseType

    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetListofManualJournalTransactions(ByVal oGetListofManualJournalTransactionsRequestType As GetListofManualJournalTransactionsRequestType) As GetListofManualJournalTransactionsResponseType
    
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetListOfManualJournalTransactionMaster(ByVal oGetListOfManualJournalTransactionMasterRequestType As GetListOfManualJournalTransactionMasterRequestType) As GetListOfManualJournalTransactionMasterResponseType

    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetListOfManualJournalTransactionDetails(ByVal oGetListofManualJournalTransactionDetailsRequestType As GetListOfManualJournalTransactionDetailsRequestType) As GetListOfManualJournalTransactionDetailsResponseType

    ''' <summary>
    ''' This Web method is used to Update Search Transaction selected column
    ''' </summary>
    ''' <param name="oUpdateManualJournalApproversCommentRequestType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function UpdateManualJournalApproversComment(ByRef oUpdateManualJournalApproversCommentRequestType As UpdateManualJournalApproversCommentRequestType) As UpdateManualJournalApproversCommentResponseType

    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function ValidateAuthorizationSteps(ByRef oValidateAuthorizationStepsRequestType As ValidateAuthorizationStepsRequestType) As ValidateAuthorizationStepsResponseType
End Interface
