Imports SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.WCF

<ServiceContract()>
Public Interface IPureCoreService
    ''' <summary>
    ''' To Add a Cover Notebook with details provided in request
    ''' </summary>
    ''' <param name="oAddCoverNoteBookRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddCoverNoteBook(ByVal oAddCoverNoteBookRequest As AddCoverNoteBookRequestType) As AddCoverNoteBookResponseType

    ''' <summary>
    ''' To Add a Cover NoteSheet with details provided in request
    ''' </summary>
    ''' <param name="oAddCoverNoteSheetRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddCoverNoteSheet(ByVal oAddCoverNoteSheetRequest As AddCoverNoteSheetRequestType) As AddCoverNoteSheetResponseType

    ''' <summary>
    ''' To Add a Documents to Documaster with details provided in request
    ''' </summary>
    ''' <param name="AddDocumentToDocumasterRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddDocumentToDocumaster(ByVal AddDocumentToDocumasterRequest As AddDocumentToDocumasterRequestType) As AddDocumentToDocumasterResponseType

    ''' <summary>
    ''' This Web method is used to add the Event Notes details by passing the Request parameters as request type objects
    ''' and also the response object event key and EventPublicTextKey is being returned .
    '''</summary>
    '''<param name="oAddEventNoteRequest" type="AddEventNoteRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.AddEventNoteResponseType</returns>  
    '''<remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddEventNote(ByVal oAddEventNoteRequest As AddEventNoteRequestType) As AddEventNoteResponseType

    ''' <summary>
    ''' TO Add a TaskGroup with details provided in request
    ''' </summary>
    ''' <param name="AddTaskGroupRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddTaskGroup(ByVal AddTaskGroupRequest As AddTaskGroupRequestType) As AddTaskGroupResponseType

    ''' <summary>
    ''' TO Add a UserGroup with details provided in request
    ''' </summary>
    ''' <param name="AddUserGroupRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddUserGroup(ByVal AddUserGroupRequest As AddUserGroupRequestType) As AddUserGroupResponseType

    'Start (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR33 - Work Manager - Task  Log.doc)-(7.2.4.6)
    ''' <summary>
    ''' This is Business logic for  Add WorkManager Task Log
    '''<param name="oRequest" type="AddWmTaskLogRequestType"></param>   
    '''</summary>
    '''<remarks></remarks>  
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddWmTaskLog(ByVal oRequest As AddWmTaskLogRequestType) As AddWmTaskLogResponseType

    ''' <summary>
    ''' TO CreateBackgroundJob details provided in request
    ''' </summary>
    ''' <param name="CreateBackgroundJobRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function CreateBackgroundJob(ByVal CreateBackgroundJobRequest As CreateBackgroundJobRequestType) As CreateBackgroundJobResponseType

    ''' <summary>
    ''' TO CreateWmTask details provided in request 
    ''' </summary>
    ''' <param name="CreateWmTaskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function CreateWmTask(ByVal CreateWmTaskRequest As CreateWmTaskRequestType) As CreateWmTaskResponseType

    ''' <summary>
    ''' TO Delete Cover Note Sheet details provided in request 
    ''' </summary>
    ''' <param name="DeleteCoverNoteSheetRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function DeleteCoverNoteSheet(ByVal DeleteCoverNoteSheetRequest As DeleteCoverNoteSheetRequestType) As DeleteCoverNoteSheetResponseType

    ''' <summary>
    ''' TO Add a TaskGroup with details provided in request
    ''' </summary>
    ''' <param name="DeleteUndeleteUserGroupRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function DeleteUndeleteUserGroup(ByVal DeleteUndeleteUserGroupRequest As DeleteUndeleteUserGroupRequestType) As DeleteUndeleteUserGroupResponseType

    ''' <summary>
    ''' TO DeleteWmTask provided in request
    ''' </summary>
    ''' <param name="DeleteWmTaskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function DeleteWmTask(ByVal DeleteWmTaskRequest As DeleteWmTaskRequestType) As DeleteWmTaskResponseType

    ''' <summary>
    ''' This web services method is used to Find Address.
    '''  </summary>
    '''<param name = "FindAddressRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindAddress(ByVal FindAddressRequest As FindAddressRequestType) As FindAddressResponseType

    <OperationContract>
<FaultContract(GetType(SAMMethodResponseData))> _
    Function FindControlSearch(ByVal oRequest As FindControlSearchRequestType) As FindControlSearchResponseType


    ''' <summary>  
    '''  This method calls the core implementation layer to find Cover Note Books.   
    ''' </summary>  
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindCoverNoteBooks(ByVal oRequest As FindCoverNoteBooksRequestType) As FindCoverNoteBooksResponseType


    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindDMEDocuments(ByVal oRequest As FindDMEDocumentsRequestType) As FindDMEDocumentsResponseType

    ''' <summary>
    '''This method passes the FindUsersRequestType request from web service layer to core implementation layer.
    '''It returns back the result from core implementation layer to web service layer.
    '''</summary>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function FindUsers(ByVal oRequest As FindUsersRequestType) As FindUsersResponseType

    ''' <summary>
    ''' </summary>
    ''' <param name="GenerateDocumentRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GenerateDocument(ByVal GenerateDocumentRequest As GenerateDocumentRequestType) As GenerateDocumentResponseType

    ''' <summary>
    ''' TO GetAgentSettings details provided in request
    ''' </summary>
    ''' <param name="GetAgentSettingsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetAgentSettings(ByVal GetAgentSettingsRequest As GetAgentSettingsRequestType) As GetAgentSettingsResponseType

    ''' <summary>
    ''' TO Get Cover Note Book provided in request
    ''' </summary>
    ''' <param name="GetCoverNoteBookRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetCoverNoteBook(ByVal GetCoverNoteBookRequest As GetCoverNoteBookRequestType) As GetCoverNoteBookResponseType


    ''' <summary>
    ''' TO Get Cover Note Sheet provided in request
    ''' </summary>
    ''' <param name="GetCoverNoteSheetRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetCoverNoteSheet(ByVal GetCoverNoteSheetRequest As GetCoverNoteSheetRequestType) As GetCoverNoteSheetResponseType

    ''' <summary>
    ''' GetCurrencyToCurrencyExchangeRate
    ''' </summary>
    ''' <param name="oGetCurrencyToCurrencyExchangeRateRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetCurrencyToCurrencyExchangeRate(ByVal oGetCurrencyToCurrencyExchangeRateRequest As GetCurrencyToCurrencyExchangeRateRequestType) As GetCurrencyToCurrencyExchangeRateResponseType

    ''' <summary>
    ''' GetDatasetSchema
    ''' </summary>
    ''' <param name="GetDatasetSchemaRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetDatasetSchema(ByVal GetDatasetSchemaRequest As GetDatasetSchemaRequestType) As GetDatasetSchemaResponseType


    ''' TO get document provided in request
    ''' </summary>
    ''' <param name="GetDocumentRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetDocument(ByVal GetDocumentRequest As GetDocumentRequestType) As GetDocumentResponseType


    ''' <summary>
    ''' TO get document defaults provided in request
    ''' </summary>
    ''' <param name="GetDocumentDefaultsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetDocumentDefaults(ByVal GetDocumentDefaultsRequest As GetDocumentDefaultsRequestType) As GetDocumentDefaultsResponseType

    ''' <summary>
    ''' TO Get Document List details provided in request 
    ''' </summary>
    ''' <param name="GetDocumentListRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetDocumentList(ByVal GetDocumentListRequest As GetDocumentListRequestType) As GetDocumentListResponseType

    ''' <summary>
    ''' GetEventNote
    ''' </summary>
    ''' <param name="oGetEventNoteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetEventNote(ByVal oGetEventNoteRequest As GetEventNoteRequestType) As GetEventNoteResponseType

    ''' <summary>
    ''' TO Get MIDFile details provided in request
    ''' </summary>
    ''' <param name="GetMIDFileDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetMIDFileDetails(ByVal GetMIDFileDetailsRequest As GetMIDFileDetailsRequestType) As GetMIDFileDetailsResponseType


    ''' <summary>
    ''' TO Get MID Files details provided in request
    ''' </summary>
    ''' <param name="GetMIDFilesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetMIDFiles(ByVal GetMIDFilesRequest As GetMIDFilesRequestType) As GetMIDFilesResponseType

    ''' <summary>
    ''' This Web method is used to get the report
    ''' </summary>
    ''' <param name="oGetReportRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
   <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetReport(ByVal oGetReportRequest As GetReportRequestType) As GetReportResponseType

    ''' <summary>
    ''' This Web method is used to get the sharepoint file list
    ''' </summary>
    ''' <param name="GetSharepointFileListRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
   <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetSharepointFileList(ByVal GetSharepointFileListRequest As GetSharepointFileListRequestType) As GetSharepointFileListResponseType

    ''' <summary>
    ''' This Web method is used to get the standard wording template
    ''' </summary>
    ''' <param name="oGetStandardWordingTemplateRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
   <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetStandardWordingTemplate(ByVal oGetStandardWordingTemplateRequest As GetStandardWordingTemplateRequestType) As GetStandardWordingTemplateResponseType

    ''' <summary>
    ''' TO Get Task Groups details provided in request
    ''' </summary>
    ''' <param name="GetTaskGroupsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetTaskGroups(ByVal GetTaskGroupsRequest As GetTaskGroupsRequestType) As GetTaskGroupsResponseType


    ''' <summary>
    ''' TO Get Task Group Tasks details provided in request
    ''' </summary>
    ''' <param name="GetTaskGroupTasksRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetTaskGroupTasks(ByVal GetTaskGroupTasksRequest As GetTaskGroupTasksRequestType) As GetTaskGroupTasksResponseType

    ''' <summary>
    ''' TO Get User Groups by Task details provided in request 
    ''' </summary>
    ''' <param name="GetUserGroupsbyTaskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetUserGroupsbyTask(ByVal GetUserGroupsbyTaskRequest As GetUserGroupsbyTaskRequestType) As GetUserGroupsbyTaskResponseType


    ''' <summary>
    ''' TO Get User Group Task Groups details provided in request
    ''' </summary>
    ''' <param name="GetUserGroupTaskGroupsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetUserGroupTaskGroups(ByVal GetUserGroupTaskGroupsRequest As GetUserGroupTaskGroupsRequestType) As GetUserGroupTaskGroupsResponseType


    ''' <summary>
    ''' TO Get Wm Task details provided in request 
    ''' </summary>
    ''' <param name="GetWmTaskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetWmTask(ByVal GetWmTaskRequest As GetWmTaskRequestType) As GetWmTaskResponseType


    ''' <summary>
    ''' TO Get Wm Task Log details provided in request 
    ''' </summary>
    ''' <param name="GetWmTaskLogRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetWmTaskLog(ByVal GetWmTaskLogRequest As GetWmTaskLogRequestType) As GetWmTaskLogResponseType

    ''' <summary>
    ''' TO ReAssignMultipleWmTasks details provided in request 
    ''' </summary>
    ''' <param name="ReAssignMultipleWmTasksrequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function ReAssignMultipleWmTasks(ByVal ReAssignMultipleWmTasksRequest As ReAssignMultipleWmTasksRequestType) As ReAssignMultipleWmTasksResponseType

    ''' <summary>
    ''' TO UpdateCoverNoteBook details provided in request 
    ''' </summary>
    ''' <param name="UpdateCoverNoteBookRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateCoverNoteBook(ByVal UpdateCoverNoteBookRequest As UpdateCoverNoteBookRequestType) As UpdateCoverNoteBookResponseType

    ''' <summary>
    ''' TO UpdateCoverNoteSheet details provided in request 
    ''' </summary>
    ''' <param name="UpdateCoverNoteSheetRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateCoverNoteSheet(ByVal UpdateCoverNoteSheetRequest As UpdateCoverNoteSheetRequestType) As UpdateCoverNoteSheetResponseType

    ''' <summary>
    ''' TO Update Standard WordingTemplate details provided in request
    ''' </summary>
    ''' <param name="UpdateStandardWordingTemplateRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateStandardWordingTemplate(ByVal UpdateStandardWordingTemplateRequest As UpdateStandardWordingTemplateRequestType) As UpdateStandardWordingTemplateResponseType


    ''' <summary>
    ''' TO Update Task Groups details provided in request 
    ''' </summary>
    ''' <param name="UpdateTaskGroupsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateTaskGroups(ByVal UpdateTaskGroupsRequest As UpdateTaskGroupsRequestType) As UpdateTaskGroupsResponseType

    ''' <summary>
    ''' TO Update Task Group Tasks provided in request 
    ''' </summary>
    ''' <param name="UpdateTaskGroupTasksRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateTaskGroupTasks(ByVal UpdateTaskGroupTasksRequest As UpdateTaskGroupTasksRequestType) As UpdateTaskGroupTasksResponseType


    ''' <summary>
    ''' TO Update User Group provided in request 
    ''' </summary>
    ''' <param name="UpdateUserGroupRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateUserGroup(ByVal UpdateUserGroupRequest As UpdateUserGroupRequestType) As UpdateUserGroupResponseType

    ''' <summary>
    ''' TO Update User Group Users details provided in request 
    ''' </summary>
    ''' <param name="UpdateUserGroupUsersRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateUserGroupUsers(ByVal UpdateUserGroupUsersRequest As UpdateUserGroupUsersRequestType) As UpdateUserGroupUsersResponseType

    ''' <summary>
    ''' TO Update Wm Taskr details provided in request 
    ''' </summary>
    ''' <param name="UpdateWmTaskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateWmTask(ByVal UpdateWmTaskRequest As UpdateWmTaskRequestType) As UpdateWmTaskResponseType



    ''' <summary>
    ''' TO Get Account Balance By AccountCode
    ''' </summary>
    ''' <param name="GetAccountBalanceByAccountCodeRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetAccountBalanceByAccountCode(ByVal GetAccountBalanceByAccountCodeRequest As GetAccountBalanceByAccountCodeRequestType) As GetAccountBalanceByAccountCodeResponseType

    ''' <summary>
    ''' TO get ClaimDataImport
    ''' </summary>
    ''' <param name="ClaimDataImportRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function ClaimDataImport(ByVal ClaimDataImportRequest As ClaimDataImportRequestType) As ClaimDataImportResponseType

    ''' <summary>
    ''' TO ProcessClaim
    ''' </summary>
    ''' <param name="ClaimProcessRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function ProcessClaim(ByVal ClaimProcessRequest As BaseClaimProcessRequestType) As BaseClaimProcessResponseType


    ''' <summary>
    ''' TO GetCaseDetails
    ''' </summary>
    ''' <param name="GetCaseDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetCaseDetails(ByVal GetCaseDetailsRequest As GetCaseDetailsRequestType) As GetCaseDetailsResponseType


    ''' <summary>
    ''' TO GetCaseDetails
    ''' </summary>
    ''' <param name="SettleAllClaimPaymentsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function SettleAllClaimPayments(ByVal SettleAllClaimPaymentsRequest As SettleAllClaimPaymentsRequestType) As SettleAllClaimPaymentsResponseType

    ''' <summary>
    ''' TO Get Numbering Scheme Nod
    ''' </summary>
    ''' <param name="GetNumberingSchemeNoRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetNumberingSchemeNo(ByVal GetNumberingSchemeNoRequest As GetNumberingSchemeNoRequestType) As GetNumberingSchemeNoResponseType


    ''' <summary>
    ''' TO Get Default Risk Clauses
    ''' </summary>
    ''' <param name="GetDefaultRiskClausesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetDefaultRiskClauses(ByVal GetDefaultRiskClausesRequest As GetDefaultRiskClausesRequestType) As GetDefaultRiskClausesResponseType


    ''' <summary>
    ''' TO Get EventNote Type
    ''' </summary>
    ''' <param name="GetEventNoteTypeRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetEventNoteType(ByVal GetEventNoteTypeRequest As GetEventNoteTypeRequestType) As GetEventNoteTypeResponseType


    ''' <summary>
    ''' TO Get Finance Plans
    ''' </summary>
    ''' <param name="GetFinancePlansRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetFinancePlans(ByVal GetFinancePlansRequest As GetFinancePlansRequestType) As GetFinancePlansResponseType

    ''' <summary>
    ''' TO Get Media Type
    ''' </summary>
    ''' <param name="GetMediaTypeRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetMediaType(ByVal GetMediaTypeRequest As GetMediaTypeRequestType) As GetMediaTypeResponseType


    ''' <summary>
    ''' TO Clear Cache
    ''' </summary>
    ''' <param name="ClearCacheRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function ClearCache(ByVal ClearCacheRequest As ClearCacheRequestType) As ClearCacheResponseType


    ''' <summary>
    '''  Client Data Import
    ''' </summary>
    ''' <param name="ClientDataImportRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function ClientDataImport(ByVal ClientDataImportRequest As ClientDataImportRequestType) As ClientDataImportResponseType

    ''' <summary>
    ''' Update Party Risk Request
    ''' </summary>
    ''' <param name="UpdatePartyRiskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdatePartyRisk(ByVal UpdatePartyRiskRequest As UpdatePartyRiskRequestType) As UpdatePartyRiskResponseType


    ''' <summary>
    ''' Delete BackDated Versions
    ''' </summary>
    ''' <param name="DeleteBackDatedVersionsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function DeleteBackDatedVersions(ByVal DeleteBackDatedVersionsRequest As DeleteBackDatedVersionsRequestType) As DeleteBackDatedVersionsResponseType


    ''' <summary>
    ''' Generate Documents For Event
    ''' </summary>
    ''' <param name="GenerateDocumentsForEventRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GenerateDocumentsForEvent(ByVal GenerateDocumentsForEventRequest As GenerateDocumentsForEventRequestType) As GenerateDocumentsForEventResponseType


    ''' <summary>
    ''' Get All Liv ePolicy Version Amounts
    ''' </summary>
    ''' <param name="GetAllLivePolicyVersionAmountsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetAllLivePolicyVersionAmounts(ByVal GetAllLivePolicyVersionAmountsRequest As GetAllLivePolicyVersionAmountsRequestType) As GetAllLivePolicyVersionAmountsResponseType

    ''' <summary>
    ''' Get Balance For CD
    ''' </summary>
    ''' <param name="GetBalanceForCDRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetBalanceForCD(ByVal GetBalanceForCDRequest As GetBalanceForCDRequestType) As GetBalanceForCDResponseType


    ''' <summary>
    ''' Process Payment And Bind Quote
    ''' </summary>
    ''' <param name="ProcessPaymentAndBindQuoteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function ProcessPaymentAndBindQuote(ByVal ProcessPaymentAndBindQuoteRequest As ProcessPaymentAndBindQuoteRequestType) As ProcessPaymentAndBindQuoteResponseType


    ''' <summary>
    ''' TransferQuote
    ''' </summary>
    ''' <param name="TransferQuoteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function TransferQuote(ByVal TransferQuoteRequest As TransferQuoteRequestType) As TransferQuoteResponseType


    ''' <summary>
    ''' Update Coinsurance Values
    ''' </summary>
    ''' <param name="UpdateCoinsuranceValuesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateCoinsuranceValues(ByVal UpdateCoinsuranceValuesRequest As UpdateCoinsuranceValuesRequestType) As UpdateCoinsuranceValuesResponseType


    ''' <summary>
    ''' Update Coinsurance Values
    ''' </summary>
    ''' <param name="UpdateQuotePaymentMethodRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateQuotePaymentMethod(ByVal UpdateQuotePaymentMethodRequest As UpdateQuotePaymentMethodRequestType) As UpdateQuotePaymentMethodResponseType


    ''' <summary>
    ''' Update Finance Plan Details
    ''' </summary>
    ''' <param name="UpdateFinancePlanDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateFinancePlanDetails(ByVal UpdateFinancePlanDetailsRequest As UpdateFinancePlanDetailsRequestType) As UpdateFinancePlanDetailsResponseType


    ''' <summary>
    ''' Update Finance Plan Details
    ''' </summary>
    ''' <param name="GetPolicyDetailsForBouncedReceiptRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetPolicyDetailsForBouncedReceipt(ByVal GetPolicyDetailsForBouncedReceiptRequest As GetPolicyDetailsForBouncedReceiptRequestType) As GetPolicyDetailsForBouncedReceiptResponseType

    ''' <summary>
    ''' To Add an Event with details provided in request
    ''' </summary>
    ''' <param name="oAddEventRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="AddEvent")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function AddEvent(ByVal oAddEventRequest As AddEventRequestType) As AddEventResponseType

    ''' <summary>
    ''' GetCurrenciesByBranch
    ''' </summary>
    ''' <param name="GetCurrenciesByBranchRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetCurrenciesByBranch")>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetCurrenciesByBranch(ByVal GetCurrenciesByBranchRequest As GetCurrenciesByBranchRequestType) As GetCurrenciesByBranchResponseType

    ''' <summary>
    ''' TO Get List  details provided in request
    ''' </summary>
    ''' <param name="GetListRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetList(ByVal GetListRequest As GetListRequestType) As GetListResponseType

    ''' <summary>
    ''' TO Get List  details from an ICCS SPU provided in request
    ''' </summary>
    ''' <param name="GetListRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetListSPUICCS(ByVal GetListRequest As GetListRequestType) As GetListResponseType

    ''' <summary>
    ''' GetDMEFolder
    ''' </summary>
    ''' <param name="GetDMEFolderRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetDMEFolder(ByVal GetDMEFolderRequest As GetDMEFolderRequestType) As GetDMEFolderResponseType

    ''' <summary>
    ''' GetDatasetDefinition
    ''' </summary>
    ''' <param name="GetDatasetDefinitionRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetDatasetDefinition")>
  <FaultContract(GetType(SAMMethodResponseData))>
    Function GetDatasetDefinition(ByVal GetDatasetDefinitionRequest As GetDatasetDefinitionRequestType) As GetDatasetDefinitionResponseType

    ''' <summary>
    ''' TO GetProductRiskOptionValue details provided in request
    ''' </summary>
    ''' <param name="GetProductRiskOptionValueRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetProductRiskOptionValue(ByVal GetProductRiskOptionValueRequest As ProductRiskOptionValueRequestType) As ProductRiskOptionValueResponseType

    ''' <summary>
    ''' To get logged-in userdetail
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()> _
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetUserDetails(ByVal oRequest As GetUserDetailsRequestType) As GetUserDetailsResponseType

    ''' <summary>
    ''' TO Get User Group Users details provided in request
    ''' </summary>
    ''' <param name="GetUserGroupUsersRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetUserGroupUsers(ByVal GetUserGroupUsersRequest As GetUserGroupUsersRequestType) As GetUserGroupUsersResponseType

    ''' <summary>
    ''' TO Get User Groups details provided in request 
    ''' </summary>
    ''' <param name="GetUserGroupsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetUserGroups(ByVal GetUserGroupsRequest As GetUserGroupsRequestType) As GetUserGroupsResponseType

    ''' <summary>
    ''' This Web method is used to get all the work manager scheduled tasks
    ''' </summary>
    ''' <param name="GetWorkManagerScheduledTasksRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
      <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetWorkManagerScheduledTasks(ByVal GetWorkManagerScheduledTasksRequest As GetWorkManagerScheduledTasksRequestType) As GetWorkManagerScheduledTasksResponseType

    ''' <summary>
    ''' GetAuditTrailUser
    ''' </summary>
    ''' <param name="oGetAuditTrailUserRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetAuditTrailUser(ByVal oGetAuditTrailUserRequest As GetAuditTrailUserRequestType) As GetAuditTrailUserResponseType

    ''' <summary>
    ''' GetAuditTrailModule
    ''' </summary>
    ''' <param name="oGetAuditTrailModuleRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetAuditTrailModule(ByVal oGetAuditTrailModuleRequest As GetAuditTrailModuleRequestType) As GetAuditTrailModuleResponseType
    ''' <summary>
    ''' GetAuditTrail
    ''' </summary>
    ''' <param name="oGetAuditTrailRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetAuditTrailDetails(ByVal oGetAuditTrailRequest As GetAuditTrailRequestType) As GetAuditTrailResponseType

    ''' <summary>
    ''' GetEventDetails
    ''' </summary>
    ''' <param name="oGetEventDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function GetEventDetails(ByVal oGetEventDetailsRequest As GetEventDetailsRequestType) As GetEventDetailsResponseType

    ''' <summary>
    ''' TO Get Option Setting details provided in request
    ''' </summary>
    ''' <param name="GetOptionSettingRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetOptionSetting(ByVal GetOptionSettingRequest As GetOptionSettingRequestType) As GetOptionSettingResponseType

    ''' <summary>
    ''' This web services method is used to Add Address.
    '''  </summary>
    '''<param name = "GetUserAuthorityValueRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetUserAuthorityValue(ByVal GetUserAuthorityValueRequest As GetUserAuthorityValueRequestType) As GetUserAuthorityValueResponseType

    ''' <summary>
    ''' This Web method is used to run the default rules files explicitly
    ''' </summary>
    ''' <param name="RunDefaultRulesAddRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
      <FaultContract(GetType(SAMMethodResponseData))> _
    Function RunDefaultRulesAdd(ByVal RunDefaultRulesAddRequest As RunDefaultRulesAddRequestType) As RunDefaultRulesAddResponseType

    ''' <summary>
    ''' This web services method is used to Run Default Rules Edit.
    '''  </summary>
    '''<param name = "RunDefaultRulesEditRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function RunDefaultRulesEdit(ByVal RunDefaultRulesEditRequest As RunDefaultRulesEditRequestType) As RunDefaultRulesEditResponseType

    ''' <summary>
    ''' TO update Taxes details provided in request
    ''' </summary>
    ''' <param name="GetProductRiskEventsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetProductRiskEvents(ByVal GetProductRiskEventsRequest As GetProductRiskEventsRequestType) As GetProductRiskEventsResponseType

    ''' <summary>
    ''' TO prdouct details for a user branch
    ''' </summary>
    ''' <param name="oGetProductsForUserBranch"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="GetProductsForUserBranch")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetProductsForUserBranch(ByVal oGetProductsForUserBranch As GetProductsForUserBranchRequestType) As GetProductsForUserBranchResponseType

    ''' <summary>
    ''' DOCUMENT IMPORT VIA DTU2
    ''' </summary>
    ''' <param name="oDocumentDataImportRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract(Name:="DocumentDataImport")>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function DocumentDataImport(ByVal oDocumentDataImportRequest As DocumentDataImportRequestType) As DocumentDataImportResponseType

    ''' <summary>
    ''' GetTaskOnKeys
    ''' </summary>
    ''' <param name="oGetTaskOnKeys"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetTaskOnKeys(ByVal oGetTaskOnKeys As GetTaskOnKeysRequestType) As GetTaskOnKeysResponseType

    ''' <summary>
    ''' UpdateTaskStatus
    ''' </summary>
    ''' <param name="oUpdateTaskStatus"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function UpdateTaskStatus(ByVal oUpdateTaskStatus As UpdateTaskStatusRequestType) As UpdateTaskStatusResponseType

    ''' <summary>
    ''' MaintainLock
    ''' </summary>
    ''' <param name="oMaintainLockRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function MaintainLock(ByVal oMaintainLockRequest As MaintainLockRequestType) As MaintainLockResponseType

    ''' <summary>
    ''' GetLockDetails
    ''' </summary>
    ''' <param name="oGetLockDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
   <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetLockDetails(ByVal oGetLockDetailsRequest As GetLockDetailsRequestType) As GetLockDetailsResponseType
    ''' <summary>
    ''' GetPolicyOutstandingAmount
    ''' </summary>
    ''' <param name="oGetPolicyOutstandingAmount"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetPolicyOutstandingAmount(ByVal oGetPolicyOutstandingAmount As GetPolicyOutstandingAmountRequestType) As GetPolicyOutstandingAmountResponseType
    ''' <summary>
    ''' GetPaymentHubSystemOptions
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))> _
    Function GetPaymentHubSystemOptions(ByVal oGetPaymentHubSystemOptionsRequest As GetPaymentHubSystemOptionsRequestType) As GetPaymentHubSystemOptionsResponseType

    ''' <summary>  
    ''' Find the Policy
    ''' </summary>  
    ''' <param name="oRequest">An object of type ImportLookupRequestType</param>  
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function ProcessList(ByVal oRequest As ProcessListRequestType) As ProcessListResponseType

    ''' <summary>
    ''' Execute any stored procedure in Pure database
    ''' </summary>
    ''' <param name="oCallNamedStoredProcedureRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    <FaultContract(GetType(SAMMethodResponseData))>
    Function CallNamedStoredProcedure(ByVal oCallNamedStoredProcedureRequest As CallNamedStoredProcedureRequestType) As CallNamedStoredProcedureResponseType

End Interface
