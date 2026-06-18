Option Strict On
Option Explicit On

#Region " Imports"

Imports Microsoft.ApplicationBlocks.ExceptionManagement
Imports SiriusFS.SAM.CoreImplementation
Imports SiriusFS.SAM.Structure.STSErrorPublisher
Imports SiriusFS.SAM.Structure
Imports SiriusFS.SAM.Structure.SAMForInsuranceV2ImplementationTypes
Imports STSCoreStruct = SiriusFS.SAM.Structure.Core
Imports SiriusFS.SAM.Structure.BaseImplementationTypes
Imports SharedFiles.gPMConstants
#End Region

Public Class SAMForInsuranceV2Business

#Region " Declarations"

    'Public Enum enumTypeOfPackage ' public?
    '    SAMForInsuranceV2Package
    '    AgentsPackage
    '    AnonymousPackage
    '    CustomersPackage
    '    MessagingPackage
    '    ReportingPackage
    '    OtherPackage
    'End Enum

#End Region

#Region " Public Methods"

#Region "GetCashDepositsForPolicy"
    'Start Prakash - WPR85 Parelleling
    Public Function GetCashDepositsForPolicy(ByVal oRequest As GetCashDepositsForPolicyRequestType) As GetCashDepositsForPolicyResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetCashDepositsForPolicyResponseType

        oResponse = DirectCast(oSAMBusiness.GetCashDepositsForPolicy(oRequest), GetCashDepositsForPolicyResponseType)

        Return oResponse

    End Function
    'End Prakash - WPR85 Parelleling
#End Region
    Public Function ClaimReceipt(ByVal oRequest As ClaimReceiptRequestType, ByVal oTax As Int16) As ClaimReceiptResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New ClaimReceiptResponseType

        oResponse = DirectCast(oSAMBusiness.ClaimReceipt(oRequest), ClaimReceiptResponseType)

        Return oResponse

    End Function

    Public Function GetClaimReceiptTaxes(ByVal oRequest As GetClaimReceiptTaxesRequestType) As GetClaimReceiptTaxesResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetClaimReceiptTaxesResponseType

        oResponse = DirectCast(oSAMBusiness.ClaimReceipt(oRequest), GetClaimReceiptTaxesResponseType)

        Return oResponse

    End Function
    Public Function AddAddress(ByVal oRequest As AddAddressRequestType) As AddAddressResponseType

        Dim oSAMBusiness As New CoreSAMBusiness()
        Dim oResponse As New AddAddressResponseType

        oResponse = DirectCast(oSAMBusiness.AddAddress(oRequest), AddAddressResponseType)

        Return oResponse

    End Function
    Public Function AddMtaQuote(ByVal AddMtaQuoteRequest As AddMtaQuoteRequestType) As AddMtaQuoteResponseType

        Dim oSAMBusiness As New CoreSAMBusiness
        Dim oResponse As New AddMtaQuoteResponseType

        oResponse = DirectCast(oSAMBusiness.AddMtaQuote(AddMtaQuoteRequest), AddMtaQuoteResponseType)

        Return oResponse

    End Function
    Public Function AddParty(ByVal oRequest As AddPartyRequestType) As AddPartyResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New AddPartyResponseType

        oResponse = DirectCast(oSAMBusiness.AddParty(oRequest), AddPartyResponseType)

        Return oResponse

    End Function

    Public Function AddQuote(ByVal oRequest As AddQuoteRequestType) As AddQuoteResponseType

        Const ACMethodName As String = "AddQuote"

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New AddQuoteResponseType

        Dim STSError As New STSErrorPublisher

        ' exit if there are any missing parameters
        If STSError.HasErrors Then
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            Return oResponse
        End If

        oResponse = DirectCast(oSAMBusiness.AddQuote(oRequest), AddQuoteResponseType)

        Return oResponse

    End Function

    Public Function AddRisk(ByVal oRequest As AddRiskRequestType) As AddRiskResponseType

        'Const ACMethodName As String = "AddRisk"
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New AddRiskResponseType

        ' Add the risk
        oResponse = DirectCast(oSAMBusiness.AddRisk(oRequest), AddRiskResponseType)

        'Set Outputs Back
        If oResponse.STSError Is Nothing = False Then
            Return oResponse
        End If

        Return oResponse

    End Function

    Public Function BindQuote(ByVal oRequest As BindQuoteRequestType) As BindQuoteResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim STSError As New STSErrorPublisher
        Const ACMethodName As String = "BindQuote"
        Dim oResponse As New BindQuoteResponseType

        ' Exit if there are any missing parameters
        If STSError.HasErrors Then
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            Return oResponse
        End If

        oResponse = DirectCast(oSAMBusiness.BindQuote(oRequest), BindQuoteResponseType)

        Return oResponse

    End Function

    Public Function ChangePassword(ByVal oRequest As ChangePasswordRequestType) As ChangePasswordResponseType
        Dim oSAMBusiness As New CoreSAMBusiness
        Dim oResponse As New ChangePasswordResponseType

        oResponse = DirectCast(oSAMBusiness.ChangePassword(oRequest), ChangePasswordResponseType)

        Return oResponse
    End Function

    Public Function CreateWMTask(ByVal oRequest As CreateWmTaskRequestType) As CreateWmTaskResponseType

        Dim oSAMBusiness As New CoreSAMBusiness
        Dim oResponse As New CreateWmTaskResponseType

        oResponse = DirectCast(oSAMBusiness.CreateWmTask(oRequest), CreateWmTaskResponseType)

        Return oResponse

    End Function

    Public Function DeleteRisk(ByVal oRequest As DeleteRiskRequestType) As DeleteRiskResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        'Const ACMethodName As String = "DeleteRisk"
        Dim oResponse As New DeleteRiskResponseType

        oResponse = DirectCast(oSAMBusiness.DeleteRisk(oRequest), DeleteRiskResponseType)

        Return oResponse

    End Function

    Public Function FindControlSearch(ByVal oRequest As FindControlSearchRequestType) As FindControlSearchResponseType

        'Const ACMethodName As String = "FindControlSearchRequest"
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New FindControlSearchResponseType

        oResponse = DirectCast(oSAMBusiness.FindControlSearch(oRequest), FindControlSearchResponseType)
        Return oResponse

    End Function

    Public Function FindParty(ByVal oRequest As FindPartyRequestType) As FindPartyResponseType

        'Const ACMethodName As String = "FindPartyRequest"
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New FindPartyResponseType

        oResponse = DirectCast(oSAMBusiness.FindParty(oRequest), FindPartyResponseType)
        Return oResponse

    End Function

    Public Function ForgottenPassword(ByVal oRequest As ForgottenPasswordRequestType) As ForgottenPasswordResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New ForgottenPasswordResponseType



        oResponse = DirectCast(oSAMBusiness.ForgottenPassword(oRequest), ForgottenPasswordResponseType)
        Return oResponse

    End Function

    Public Function GetClaimDetails(ByVal oRequest As GetClaimDetailsRequestType) As GetClaimDetailsResponseType

        'Const ACMethodName As String = "GetClaimDetails"
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetClaimDetailsResponseType

        ' Validate the data in here 



        oResponse = DirectCast(oSAMBusiness.GetClaimDetails(oRequest), GetClaimDetailsResponseType)


        Return oResponse

    End Function

    Public Function PayClaim(ByVal oRequest As PayClaimRequestType) As PayClaimResponseType

        'Const ACMethodName As String = "GetClaimDetails"
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New PayClaimResponseType

        oResponse = DirectCast(oSAMBusiness.PayClaim(oRequest), PayClaimResponseType)

        Return oResponse

    End Function

    Public Function GetClaimpaymentTaxes(ByVal oRequest As GetClaimPaymentTaxesRequestType) As GetClaimPaymentTaxesResponseType

        'Const ACMethodName As String = "GetClaimDetails"
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetClaimPaymentTaxesResponseType

        oResponse = DirectCast(oSAMBusiness.PayClaim(oRequest), GetClaimPaymentTaxesResponseType)

        Return oResponse

    End Function

    Public Function GetClaimRisk(ByVal oRequest As GetClaimRiskRequestType) As GetClaimRiskResponseType

        'Const ACMethodName As String = "GetClaimRisk"
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetClaimRiskResponseType

        oResponse = DirectCast(oSAMBusiness.GetClaimRisk(oRequest), GetClaimRiskResponseType)

        Return oResponse

    End Function

    Public Function GenerateClaimsDocuments(ByVal oRequest As GenerateClaimsDocumentsRequestType) As GenerateClaimsDocumentsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Const ACMethodName As String = "GenerateClaimsDocuments"
        Dim oResponse As New GenerateClaimsDocumentsResponseType

        oResponse.STSError = ClaimKeyChecks(oRequest.ClaimKey, ACMethodName)
        If oResponse.STSError Is Nothing = False Then
            Return oResponse
        End If

        oResponse = DirectCast(oSAMBusiness.GenerateClaimsDocuments(oRequest), GenerateClaimsDocumentsResponseType)

        Return oResponse

    End Function

    Public Function GenerateDocument(ByVal oRequest As GenerateDocumentRequestType) As GenerateDocumentResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        'Const ACMethodName As String = "GenerateDocument"
        Dim oResponse As New GenerateDocumentResponseType

        oResponse = DirectCast(oSAMBusiness.GenerateDocument(oRequest), GenerateDocumentResponseType)

        Return oResponse

    End Function

    Public Function GetAccountBalance(ByVal oRequest As GetAccountBalanceRequestType) As GetAccountBalanceResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        'Const ACMethodName As String = "GetAccountBalance"
        Dim oResponse As New GetAccountBalanceResponseType

        oResponse = DirectCast(oSAMBusiness.GetAccountBalance(oRequest), GetAccountBalanceResponseType)

        Return oResponse

    End Function

    Public Function GetAddress(ByVal oRequest As GetAddressRequestType) As GetAddressResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        'Const ACMethodName As String = "GetAddress"
        Dim oResponse As New GetAddressResponseType

        oResponse = DirectCast(oSAMBusiness.GetAddress(oRequest), GetAddressResponseType)

        Return oResponse

    End Function

    Public Function GetAllPolicyVersions(ByVal oRequest As GetAllPolicyVersionsRequestType) As GetAllPolicyVersionsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)

        'Const ACMethodName As String = "GetAllPolicyVersions"
        Dim oResponse As New GetAllPolicyVersionsResponseType

        oResponse = DirectCast(oSAMBusiness.GetAllPolicyVersions(oRequest), GetAllPolicyVersionsResponseType)

        Return oResponse

    End Function

    Public Function GetCurrenciesByBranch(ByVal oRequest As GetCurrenciesByBranchRequestType) As GetCurrenciesByBranchResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        'Const ACMethodName As String = "GetCurrenciesByBranch"
        Dim oResponse As New GetCurrenciesByBranchResponseType

        oResponse = DirectCast(oSAMBusiness.GetCurrenciesByBranch(oRequest), GetCurrenciesByBranchResponseType)

        Return oResponse

    End Function


#Region " GetDatasetSchema"

    Public Function GetDatasetSchema(ByVal oRequest As SiriusFS.SAM.Structure.BaseImplementationTypes.BaseGetDatasetSchemaRequestType) As SiriusFS.SAM.Structure.BaseImplementationTypes.BaseGetDatasetSchemaResponseType

        Dim oGetDatasetSchemaResponse As BaseImplementationTypes.BaseGetDatasetSchemaResponseType
        ' Declare the Response object
        Dim oResponse As New SiriusFS.SAM.Structure.BaseImplementationTypes.BaseGetDatasetSchemaResponseType

        ' Declare the Core SAM business object
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)

        ' Process the GetDatasetSchema by calling the implementation method 
        oGetDatasetSchemaResponse = oSAMBusiness.GetDatasetSchema(oRequest)

        oResponse.DatasetSchema = oGetDatasetSchemaResponse.DatasetSchema
        oResponse.STSError = oGetDatasetSchemaResponse.STSError

        Return oResponse

    End Function

#End Region

#Region " GetDatasetDefinition"

    Public Function GetDatasetDefinition(ByVal GetDatasetDefinitionRequest As GetDatasetDefinitionRequestType) As GetDatasetDefinitionResponseType

        'Const ACMethodName As String = "GetDatasetDefinition"

        Dim oGetDatasetDefinitionResponse As BaseImplementationTypes.BaseGetDatasetDefinitionResponseType
        ' Declare the Response object
        Dim oResponse As New GetDatasetDefinitionResponseType

        ' Declare the Core SAM business object
        Dim oSAMBusiness As New CoreSAMBusiness()

        ' Process the GetDatasetDefinition by calling the implementation method 
        oGetDatasetDefinitionResponse = oSAMBusiness.GetDatasetDefinition(GetDatasetDefinitionRequest)

        oResponse.XMLDatasetDefinition = oGetDatasetDefinitionResponse.XMLDatasetDefinition
        oResponse.STSError = oGetDatasetDefinitionResponse.STSError

        Return oResponse

    End Function

#End Region

    Public Function GetHeaderAndSummariesByKey(ByVal oRequest As GetHeaderAndSummariesByKeyRequestType) As GetHeaderAndSummariesByKeyResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetHeaderAndSummariesByKeyResponseType

        oResponse = DirectCast(oSAMBusiness.GetHeaderAndSummariesByKey(oRequest), GetHeaderAndSummariesByKeyResponseType)

        Return oResponse

    End Function

    Public Function GetHeaderAndSummariesByRef(ByVal oRequest As GetHeaderAndSummariesByRefRequestType) As GetHeaderAndSummariesByRefResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        'Const ACMethodName As String = "GetHeaderAndSummariesByRef"
        Dim oResponse As New GetHeaderAndSummariesByRefResponseType

        oResponse = DirectCast(oSAMBusiness.GetHeaderAndSummariesByRef(oRequest), GetHeaderAndSummariesByRefResponseType)

        Return oResponse

    End Function

    Public Function GetInstalmentQuotes(ByVal oRequest As GetInstalmentQuotesRequestType) As GetInstalmentQuotesResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        'Const ACMethodName As String = "GetInstalmentQuotes"
        Dim oResponse As New GetInstalmentQuotesResponseType

        oResponse = DirectCast(oSAMBusiness.GetInstalmentQuotes(oRequest), GetInstalmentQuotesResponseType)

        Return oResponse

    End Function

    Public Function GetList(ByVal oRequest As GetListRequestType) As GetListResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        'Const ACMethodName As String = "GetList"
        Dim oResponse As New GetListResponseType

        oResponse = DirectCast(oSAMBusiness.GetList(oRequest), GetListResponseType)

        Return oResponse

    End Function

    Public Function GetOptionSetting(ByVal oRequest As GetOptionSettingRequestType) As GetOptionSettingResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetOptionSettingResponseType

        oResponse = DirectCast(oSAMBusiness.GetOptionSetting(oRequest), GetOptionSettingResponseType)

        Return oResponse

    End Function

    Public Function GetParty(ByVal oRequest As GetPartyRequestType) As GetPartyResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        'Const ACMethodName As String = "GetParty"
        Dim oResponse As New GetPartyResponseType

        oResponse = DirectCast(oSAMBusiness.GetParty(oRequest), GetPartyResponseType)

        Return oResponse

    End Function

    Public Function GetPartySummary(ByVal oRequest As GetPartySummaryRequestType) As GetPartySummaryResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        'Const ACMethodName As String = "GetPartySummary"
        Dim oResponse As New GetPartySummaryResponseType

        oResponse = DirectCast(oSAMBusiness.GetPartySummary(oRequest), GetPartySummaryResponseType)

        Return oResponse

    End Function

    Public Function GetRatingDetails(ByVal oRequest As GetRatingDetailsRequestType) As GetRatingDetailsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        'Const ACMethodName As String = "GetRatingDetails"
        Dim oResponse As New GetRatingDetailsResponseType

        oResponse = DirectCast(oSAMBusiness.GetRatingDetails(oRequest), GetRatingDetailsResponseType)

        Return oResponse

    End Function

    Public Function GetRisk(ByVal oRequest As GetRiskRequestType) As GetRiskResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        'Const ACMethodName As String = "GetRisk"
        Dim oResponse As New GetRiskResponseType

        oResponse = DirectCast(oSAMBusiness.RetrieveRisk(oRequest), GetRiskResponseType)

        Return oResponse

    End Function

    Public Function GetProductByAgent(ByVal oRequest As GetProductByAgentRequestType) As GetProductByAgentResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        'Const ACMethodName As String = "GetProductByAgent"
        Dim oResponse As New GetProductByAgentResponseType

        oResponse = DirectCast(oSAMBusiness.GetProductByAgent(oRequest), GetProductByAgentResponseType)

        Return oResponse

    End Function

    Public Function GetRiskByProduct(ByVal oRequest As GetRiskByProductRequestType) As GetRiskByProductResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        'Const ACMethodName As String = "GetRiskByProduct"
        Dim oResponse As New GetRiskByProductResponseType

        oResponse = DirectCast(oSAMBusiness.GetRiskByProduct(oRequest), GetRiskByProductResponseType)

        Return oResponse

    End Function

    Public Function GetUserDetails(ByVal oRequest As GetUserDetailsRequestType) As BaseImplementationTypes.BaseGetUserDetailsResponseType

        'Const ACMethodName As String = "GetUserDetails"

        Dim oSAMBusiness As New CoreSAMBusiness()
        Dim oResponse As New GetUserDetailsResponseType

        oResponse = DirectCast(oSAMBusiness.GetUserDetails(oRequest), GetUserDetailsResponseType)

        Return oResponse

    End Function

    Public Function RunDefaultRulesAdd(ByVal oRequest As RunDefaultRulesAddRequestType) As RunDefaultRulesAddResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        'Const ACMethodName As String = "RunDefaultRulesAdd"
        Dim oResponse As New RunDefaultRulesAddResponseType

        oResponse = DirectCast(oSAMBusiness.RunDefaultRulesAdd(oRequest), RunDefaultRulesAddResponseType)

        Return oResponse

    End Function

    Public Function RunDefaultRulesEdit(ByVal oRequest As RunDefaultRulesEditRequestType) As RunDefaultRulesEditResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        'Const ACMethodName As String = "RunDefaultRulesEdit"
        Dim oResponse As New RunDefaultRulesEditResponseType

        oResponse = DirectCast(oSAMBusiness.RunDefaultRulesEdit(oRequest), RunDefaultRulesEditResponseType)

        Return oResponse

    End Function

    Public Function UpdateClaimRisk(ByVal oRequest As UpdateClaimRiskRequestType) As UpdateClaimRiskResponseType

        'Const ACMethodName As String = "UpdateClaimRisk"

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New UpdateClaimRiskResponseType

        oResponse = DirectCast(oSAMBusiness.UpdateClaimRisk(oRequest), UpdateClaimRiskResponseType)

        Return oResponse

    End Function

    Public Function UpdateParty(ByVal oRequest As UpdatePartyRequestType) As UpdatePartyResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        'Const ACMethodName As String = "UpdateParty"
        Dim oResponse As New UpdatePartyResponseType

        oResponse = DirectCast(oSAMBusiness.UpdateParty(oRequest), UpdatePartyResponseType)

        Return oResponse

    End Function

    Public Function UpdateQuote(ByVal oRequest As UpdateQuoteRequestType) As UpdateQuoteResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        'Const ACMethodName As String = "UpdateQuote"
        Dim oResponse As New UpdateQuoteResponseType

        oResponse = DirectCast(oSAMBusiness.UpdateQuote(oRequest), UpdateQuoteResponseType)

        Return oResponse

    End Function

    Public Function UpdateRisk(ByVal oRequest As UpdateRiskRequestType) As UpdateRiskResponseType

        'Const ACMethodName As String = "UpdateRisk"

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New UpdateRiskResponseType

        oResponse = DirectCast(oSAMBusiness.UpdateRisk(oRequest), UpdateRiskResponseType)

        Return oResponse

    End Function

    Public Function FindClaim(ByVal oREquest As FindClaimRequestType) As FindClaimResponseType

        'Const ACMethodName As String = "FindClaim"
        Dim oSAMBusiness As New CoreSAMBusiness(oREquest.BranchCode)
        Dim oResponse As New FindClaimResponseType

        oResponse = DirectCast(oSAMBusiness.FindClaim(oREquest), FindClaimResponseType)
        Return oResponse

    End Function

    Public Function OpenClaim(ByVal oRequest As OpenClaimRequestType) As OpenClaimResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New OpenClaimResponseType

        oResponse = DirectCast(oSAMBusiness.OpenClaim(oRequest), OpenClaimResponseType)

        Return oResponse

    End Function

    Public Function MaintainClaim(ByVal oRequest As MaintainClaimRequestType) As MaintainClaimResponseType

        'Const ACMethodName As String = "MaintainClaim"

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New MaintainClaimResponseType

        oResponse = DirectCast(oSAMBusiness.MaintainClaim(oRequest), MaintainClaimResponseType)

        Return oResponse

    End Function

    Public Function GetClaimRiskLinks(ByVal oRequest As GetClaimRiskLinksRequestType) As GetClaimRiskLinksResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetClaimRiskLinksResponseType

        oResponse = DirectCast(oSAMBusiness.GetClaimRiskLinks(oRequest), GetClaimRiskLinksResponseType)

        Return oResponse

    End Function

    Public Function RunValidationRules(ByVal oRequest As RunValidationRulesRequestType) As RunValidationRulesResponseType

        'Const ACMethodName As String = "RunValidationRules"

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New RunValidationRulesResponseType

        oResponse = DirectCast(oSAMBusiness.RunValidationRules(oRequest), RunValidationRulesResponseType)

        Return oResponse
    End Function

    Public Function AddAgentReceipt(ByVal oRequest As AddAgentReceiptRequestType) As AddAgentReceiptResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New AddAgentReceiptResponseType

        oResponse = DirectCast(oSAMBusiness.AddAgentReceipt(oRequest), AddAgentReceiptResponseType)

        Return oResponse
    End Function

#Region "AddPayNowReceipt"
    'Start (PraveenGora) - (Tech Spec - UIICWR23 - New Business - AddPayNowReceipt.doc) - (7.1.2.7)
    ''' <summary>
    ''' This is webservice method for AddPayNowReceipt
    '''<param name="oRequest" type="AddPayNowReceiptRequestType"></param>   
    '''</summary>
    '''<returns>AddPayNowReceiptResponseType</returns>
    '''<remarks></remarks>
    Public Function AddPayNowReceipt(ByVal oRequest As AddPayNowReceiptRequestType) As AddPayNowReceiptResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New AddPayNowReceiptResponseType

        oResponse = DirectCast(oSAMBusiness.AddPayNowReceipt(oRequest), AddPayNowReceiptResponseType)

        Return oResponse
    End Function
    'End (PraveenGora) - (Tech Spec - UIICWR23 - New Business - AddPayNowReceipt.doc) - (7.1.2.7)
#End Region


#Region " Client Data Import"
    ' ***************************************************************** '
    ' Name: ClientDataImport
    '
    ' Description: This method is the implementation of the ClientDataImport 
    '              Web Method on the Messaging service
    '
    ' ***************************************************************** '
    Public Function ClientDataImport(ByVal ClientDataImportRequest As ClientDataImportRequestType) As ClientDataImportResponseType

        'Const ACMethodName As String = "ClientDataImport"

        ' Declare the Response object
        Dim oResponse As New ClientDataImportResponseType

        ' Declare the Core SAM business object
        Dim oBusiness As New CoreSAMBusiness(ClientDataImportRequest.BranchCode)

        oResponse = DirectCast(oBusiness.ClientDataImport(ClientDataImportRequest), ClientDataImportResponseType)

        Return oResponse

    End Function
#End Region

    ' ***************************************************************** '
    ' Name: ClaimDataImport
    '
    ' Description: This method is the implementation of the ClaimDataImport 
    '              Web Method on the Messaging service
    '
    ' ***************************************************************** '
    Public Function ClaimDataImport(ByVal ClaimDataImportRequest As ClaimDataImportRequestType) As ClaimDataImportResponseType

        'Const ACMethodName As String = "ClientDataImport"

        ' Declare the Response object
        Dim response As New ClaimDataImportResponseType

        ' Declare the Core SAM business object
        Dim business As New CoreImplementation.DataTransfer.General

        response = DirectCast(business.ClaimDataImport(ClaimDataImportRequest), ClaimDataImportResponseType)

        Return response

    End Function

    Public Function PostDocument(ByVal oRequest As PostDocumentRequestType) As PostDocumentResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New PostDocumentResponseType

        oResponse = DirectCast(oSAMBusiness.PostDocument(oRequest), PostDocumentResponseType)

        Return oResponse
    End Function

#Region "Public Method-CreateReceiptCashListItem"
    'Start (Arul Stephen) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Create Receipt Cash List Item.doc) -(7.1.4.6.1)
    ''' <summary>
    '''This  method  will become a communicator between the service layer and core implementation layer
    '''</summary>
    '''<param name="oCreateReceiptCashListItemRequest">An object of the class CreateReceiptCashListItemRequestType</param>
    '''<remarks></remarks> 
    Public Function CreateReceiptCashListItem(ByVal oCreateReceiptCashListItemRequest As CreateReceiptCashListItemRequestType) As CreateReceiptCashListItemResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oCreateReceiptCashListItemRequest.BranchCode)
        Dim oResponse As New CreateReceiptCashListItemResponseType

        oResponse = DirectCast(oSAMBusiness.CreateReceiptCashListItem(oCreateReceiptCashListItemRequest), CreateReceiptCashListItemResponseType)

        Return oResponse

    End Function
    'End ((Arul Stephen) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Create Receipt Cash List Item.doc) -(7.1.4.6.1)
#End Region


#Region "Public Method-CreateReceiptCashListWithItems"
    'Start (Arul Stephen) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Create Receipt Cash List .doc) -(7.1.4.7)
    ''' <summary>
    '''This  method  will become a communicator between the service layer and core implementation layer
    '''</summary>
    '''<param name="oRequest">An object of the class GetPolicyBankGuaranteeRequestType</param>
    '''<remarks></remarks> 
    Public Function CreateReceiptCashListWithItems(ByVal oCreateReceiptCashListWithItemsRequest As CreateReceiptCashListWithItemsRequestType) As CreateReceiptCashListWithItemsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oCreateReceiptCashListWithItemsRequest.BranchCode)
        Dim oResponse As New CreateReceiptCashListWithItemsResponseType

        oResponse = DirectCast(oSAMBusiness.CreateReceiptCashListWithItems(oCreateReceiptCashListWithItemsRequest), CreateReceiptCashListWithItemsResponseType)

        Return oResponse

    End Function
    '(Arul Stephen) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Create Receipt Cash List .doc) -(7.1.4.7)
#End Region

#Region "Public Method-GetPolicyBankGuarantee"
    'Start (Arul Stephen A) - (Tech Spec - UIIC WR6 - Get Bank Guarantee.doc) - (7.1.5.6)
    ''' <summary>
    '''This  method  will become a communicator between the service layer and core implementation layer
    '''</summary>
    '''<param name="oRequest">An object of the class GetPolicyBankGuaranteeRequestType</param>
    '''<remarks></remarks> 
    Public Function GetPolicyBankGuarantee(ByVal oRequest As GetPolicyBankGuaranteeRequestType) As GetPolicyBankGuaranteeResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetPolicyBankGuaranteeResponseType

        oResponse = DirectCast(oSAMBusiness.GetPolicyBankGuarantee(oRequest), GetPolicyBankGuaranteeResponseType)

        Return oResponse

    End Function
    'End (Arul Stephen A) - (Tech Spec - UIIC WR6 - Get Bank Guarantee.doc) - (7.1.5.6)
#End Region
#Region "Public Method-AddBankGuarantee"
    'Start (Arul Stephen A) - (Tech Spec - UIIC WR6 - Add Bank Guarantee.doc) - (7.1.5.6)
    ''' <summary>
    '''This  method  will become a communicator between the service layer and core implementation layer
    '''</summary>
    '''<param name="oRequest">An object of the class AddBankGuaranteeRequestType</param>
    '''<remarks></remarks> 
    Public Function AddBankGuarantee(ByVal oRequest As AddBankGuaranteeRequestType) As AddBankGuaranteeResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New AddBankGuaranteeResponseType

        oResponse = DirectCast(oSAMBusiness.AddBankGuarantee(oRequest), AddBankGuaranteeResponseType)

        Return oResponse

    End Function
    'End (Arul Stephen A) - (Tech Spec - UIIC WR6 - Add Bank Guarantee.doc) - (7.1.5.6)
#End Region


#Region "Public Method-GetBankGuarantee"
    'Start (Arul Stephen A) - (Tech Spec - UIIC WR6 - Get Bank Guarantee.doc) - (7.1.4.7)
    ''' <summary>
    '''This web method  will become a communicator between the service layer and core implementation layer
    '''</summary>
    '''<param name="oRequest">An object of the class GetBankGuaranteeRequestType</param>
    '''<remarks></remarks> 
    Public Function GetBankGuarantee(ByVal oRequest As GetBankGuaranteeRequestType) As GetBankGuaranteeResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetBankGuaranteeResponseType

        oResponse = DirectCast(oSAMBusiness.GetBankGuarantee(oRequest), GetBankGuaranteeResponseType)

        Return oResponse

    End Function
    'End (Arul Stephen A) - (Tech Spec - UIIC WR6 - Get Bank Guarantee.doc) - (7.1.4.7)
#End Region

#Region "FindBankGuarantee"
    'Start (Girija chokkalingam) -  (Tech Spec - UIIC WR6 Bank Guarantee – Find Bank Guarantee.doc) - (7.1.4.6)
    ''' <summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="FindBankGuaranteeRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.FindBankGuaranteeResponseType</returns>  
    '''<remarks></remarks>
    Public Function FindBankGuarantee(ByVal oRequest As FindBankGuaranteeRequestType) As FindBankGuaranteeResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New FindBankGuaranteeResponseType

        oResponse = DirectCast(oSAMBusiness.FindBankGuarantee(oRequest), FindBankGuaranteeResponseType)

        Return oResponse

    End Function

    'End (Girija chokkalingam) -  (Tech Spec - UIIC WR6 Bank Guarantee – Find Bank Guarantee.doc) - (7.1.4.6)
#End Region
#Region " Public Class - BaseFindBankRequestType "
    'Start (Ravikumar Pasupuleti)-(Tech Spec - UIICWR6 - Find Bank.doc) - (7.1.4.6)
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="FindBankRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.FindBankResponseType</returns>  
    '''<remarks></remarks>

    Public Function FindBank(ByVal oRequest As FindBankRequestType) As FindBankResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New FindBankResponseType

        oResponse = DirectCast(oSAMBusiness.FindBank(oRequest), FindBankResponseType)

        Return oResponse

    End Function
    'End (Ravikumar Pasupuleti)-(Tech Spec - UIICWR6 - Find Bank.doc) - (7.1.4.6)
#End Region

#Region " UpdateBankGuarantee "
    'Start (Sankar)-(Tech Spec - UIICWR6 - Update Bank Guarantee.doc)-(7.1.3.7)
    ''' <summary>  
    '''  This method calls core implementation layer and updates Bank Guarantee.
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.UpdateBankGuaranteeRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.UpdateBankGuaranteeResponseType</returns>  
    Public Function UpdateBankGuarantee(ByVal oRequest As UpdateBankGuaranteeRequestType) As UpdateBankGuaranteeResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New UpdateBankGuaranteeResponseType
        oResponse = DirectCast(oSAMBusiness.UpdateBankGuarantee(oRequest), UpdateBankGuaranteeResponseType)
        Return oResponse
    End Function
    'End (Sankar)-(Tech Spec - UIICWR6 - Update Bank Guarantee.doc)-(7.1.3.7)
#End Region

#Region "Public Method-GetHeaderAndRiskTaxByKey"
    'Start (Arul Stephen)-(Tech Spec - UIICWR27 - MTA - Risk Premium - Get Risk Taxes.doc)-(7.1.3.9)
    ''' <summary>
    '''This function will call the  "CoreImplementation" to accomplish the functionality
    '''</summary>
    '''<remarks></remarks> 
    Public Function GetHeaderAndRiskTaxByKey(ByVal oRequest As GetHeaderAndRiskTaxByKeyRequestType) As GetHeaderAndRiskTaxByKeyResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetHeaderAndRiskTaxByKeyResponseType

        oResponse = DirectCast(oSAMBusiness.GetHeaderAndRiskTaxByKey(oRequest), GetHeaderAndRiskTaxByKeyResponseType)

        Return oResponse

    End Function
    'End (Arul Stephen)-(Tech Spec - UIICWR27 - MTA - Risk Premium - Get Risk Taxes.doc)-(7.1.3.9)
#End Region
#Region "Public Method-GetHeaderAndPolicyTaxByKey"
    'Start(Arul Stephen)-(Tech Spec - UIICWR50 - MTC - List Risks - Policy Tax.doc)-(7.1.3.9)
    ''' <summary>
    '''This function will call the CoreSAMBusiness to implement the required functionality.
    '''</summary>
    '''<param name="oRequest" type="GetHeaderAndPolicyTaxByKeyRequestType"></param>    
    '''<remarks></remarks>
    Public Function GetHeaderAndPolicyTaxByKey(ByVal oRequest As GetHeaderAndPolicyTaxByKeyRequestType) As GetHeaderAndPolicyTaxByKeyResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetHeaderAndPolicyTaxByKeyResponseType

        oResponse = DirectCast(oSAMBusiness.GetHeaderAndPolicyTaxByKey(oRequest), GetHeaderAndPolicyTaxByKeyResponseType)

        Return oResponse

    End Function
    'End(Arul Stephen)-(Tech Spec - UIICWR50 - MTC - List Risks - Policy Tax.doc)-(7.1.3.9)
#End Region
#Region "Public Method-GetVersionsForClaim"
    'Start (Arul Stephen)-(Tech Spec - UIIC WR26 View Claims – Claim Versions)-(7.1.2.2.2)
    ''' <summary>
    ''' This funciton will call the CoreSAMBusines Function
    '''</summary>
    ''' <param name="oRequest" type="GetVersionsForClaimRequestType"></param>
    '''<remarks></remarks>
    Public Function GetVersionsForClaim(ByVal oRequest As GetVersionsForClaimRequestType) As GetVersionsForClaimResponseType

        Dim oCoreImplemtentation As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetVersionsForClaimResponseType

        oResponse = DirectCast(oCoreImplemtentation.GetVersionsForClaim(oRequest), GetVersionsForClaimResponseType)

        Return oResponse
    End Function
    'End (Arul Stephen)-(Tech Spec - UIIC WR26 View Claims – Claim Versions)-(7.1.2.2.2)
#End Region
    Public Function FindInsuranceFileForClaims(ByVal oRequest As FindInsuranceFileForClaimsRequestType) As FindInsuranceFileForClaimsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New FindInsuranceFileForClaimsResponseType

        oResponse = DirectCast(oSAMBusiness.FindInsuranceFileForClaimsVersion2(oRequest), FindInsuranceFileForClaimsResponseType)
        Return oResponse
    End Function

    Public Function SaveRisk(ByVal oRequest As SaveRiskRequestType) As SaveRiskResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New SaveRiskResponseType

        oResponse = DirectCast(oSAMBusiness.SaveRisk(oRequest), SaveRiskResponseType)

        Return oResponse
    End Function

    Public Function GetClaimPaymentTaxGroups(ByVal oRequest As GetClaimPaymentTaxGroupsRequestType) As GetClaimPaymentTaxGroupsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetClaimPaymentTaxGroupsResponseType

        oResponse = DirectCast(oSAMBusiness.GetClaimPaymentTaxGroups(oRequest), GetClaimPaymentTaxGroupsResponseType)

        Return oResponse
    End Function

    Public Function GetClaimReceiptTaxGroups(ByVal oRequest As GetClaimReceiptTaxGroupsRequestType) As GetClaimReceiptTaxGroupsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetClaimReceiptTaxGroupsResponseType

        oResponse = DirectCast(oSAMBusiness.GetClaimReceiptTaxGroups(oRequest), GetClaimReceiptTaxGroupsResponseType)

        Return oResponse
    End Function

    Public Function UpdatePartyRisk(ByVal oRequest As UpdatePartyRiskRequestType) As UpdatePartyRiskResponseType

        Dim coreSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim coreXBuilderParty As New CoreImplementation.XBuilder.Party(coreSAMBusiness._SiriusUser)

        Dim oResponse As New UpdatePartyRiskResponseType

        oResponse = DirectCast(coreXBuilderParty.UpdateDataSet(oRequest), UpdatePartyRiskResponseType)

        Return oResponse
    End Function

    Public Function GetDocument(ByVal GetDocumentInput As GetDocumentRequestType) As GetDocumentResponseType

        Dim oBusiness As New CoreSAMBusiness
        Dim oResponse As New GetDocumentResponseType

        oResponse = DirectCast(oBusiness.GetDocument(GetDocumentInput), GetDocumentResponseType)

        Return oResponse

    End Function

    Public Function GetDocumentList(ByVal GetDocumentListRequest As GetDocumentListRequestType) As GetDocumentListResponseType

        Dim oSamBusiness As New CoreSAMBusiness
        Dim oResponse As New GetDocumentListResponseType

        oResponse = DirectCast(oSamBusiness.GetDocumentList(GetDocumentListRequest), GetDocumentListResponseType)
        Return oResponse

    End Function

#Region " Get Header And Risks By Key "

    'Start (Sankar)-(Tech Spec - UIICWR50 - MTC - List Risks - Risks.doc)-(7.1.4.7)
    ''' <summary>  
    ''' Retrieves risk details for a policy. 
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.GetHeaderAndRisksByKeyRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.GetHeaderAndRisksByKeyResponseType</returns>  
    Public Function GetHeaderAndRisksByKey(ByVal oRequest As GetHeaderAndRisksByKeyRequestType) As GetHeaderAndRisksByKeyResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetHeaderAndRisksByKeyResponseType

        oResponse = DirectCast(oSAMBusiness.GetHeaderAndRisksByKey(oRequest), GetHeaderAndRisksByKeyResponseType)

        Return oResponse

    End Function
    'End (Sankar)-(Tech Spec - UIICWR50 - MTC - List Risks - Risks.doc)-(7.1.4.7)
#End Region

#Region " Get Reserve Reinsurance Recoveries "

    'Start (Sankar) - (Tech Spec - UIIC WR24 - OpenClaim - Reserve Process - Reinsurance - Recoveries.doc) - (7.1.2.2.2)
    ''' <summary>  
    '''  Retrieves reserve reinsurance recoveries details.
    ''' </summary>  
    ''' <param name="request">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.GetRecoveryReinsuranceRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.GetRecoveryReinsuranceResponseType</returns>  
    Public Function GetRecoveryReinsurance(ByVal request As GetRecoveryReinsuranceRequestType) As GetRecoveryReinsuranceResponseType

        Dim oCoreImplemtentation As New CoreSAMBusiness(request.BranchCode)
        Dim oResponse As New GetRecoveryReinsuranceResponseType

        oResponse = DirectCast(oCoreImplemtentation.GetRecoveryReinsurance(request), GetRecoveryReinsuranceResponseType)

        Return oResponse
    End Function
    'End (Sankar) - (Tech Spec - UIIC WR24 - OpenClaim - Reserve Process - Reinsurance - Recoveries.doc) - (7.1.2.2.2)
#End Region

#Region "Get Header And Policy Fees By Key"
    'Start (PraveenGora)-(Tech Spec - UIICWR50 - MTC - List Risks - Policy Fees.doc)-(7.1.4.7)
    ''' <summary>
    '''This function will call the CoreSAMBusiness to implement the required functionality.
    '''</summary>
    '''<param name="GetHeaderAndPolicyFeesByKeyRequestType" type="GetHeaderAndPolicyFeesByKeyResponseType"></param>   
    '''<remarks></remarks>  
    Public Function GetHeaderAndPolicyFeesByKey(ByVal oRequest As GetHeaderAndPolicyFeesByKeyRequestType) As GetHeaderAndPolicyFeesByKeyResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetHeaderAndPolicyFeesByKeyResponseType

        oResponse = DirectCast(oSAMBusiness.GetHeaderAndPolicyFeesByKey(oRequest), GetHeaderAndPolicyFeesByKeyResponseType)

        Return oResponse

    End Function
    'End (PraveenGora)-(Tech Spec - UIICWR50 - MTC - List Risks - Policy Fees.doc)-(7.1.4.7)

#End Region

#Region "Get Header And Risk Fees By Key"
    'Start (PraveenGora)-(Tech Spec - UIICWR27 - MTA - Risk Premium - Get Risk Fees.doc)-(7.1.4.7)
    ''' <summary>
    '''This function will call the CoreSAMBusiness to implement the required functionality.
    '''</summary>
    '''<param name="GetHeaderAndPolicyFeesByKeyRequestType" type="GetHeaderAndPolicyFeesByKeyResponseType"></param>   
    '''<remarks></remarks>  
    Public Function GetHeaderAndRiskFeesByKey(ByVal oRequest As GetHeaderAndRiskFeesByKeyRequestType) As GetHeaderAndRiskFeesByKeyResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetHeaderAndRiskFeesByKeyResponseType

        oResponse = DirectCast(oSAMBusiness.GetHeaderAndRiskFeesByKey(oRequest), GetHeaderAndRiskFeesByKeyResponseType)

        Return oResponse

    End Function
    'End (PraveenGora)-(Tech Spec - UIICWR27 - MTA - Risk Premium - Get Risk Fees.doc)-(7.1.4.7)

#End Region

#Region "Get Allocation Details"
    'Start (Praveen Gora) - (Tech Spec - WR42A_SAM_Party_Enquiry-GetAllocationDetails.doc) - (7.1.3.9)
    ''' <summary>
    '''This function will call the CoreSAMBusiness to implement the required functionality.
    '''</summary>
    '''<param name="GetAllocationDetailsRequestType" type="GetAllocationDetailsResponseType"></param>   
    '''<remarks></remarks>  
    Public Function GetAllocationDetails(ByVal oRequest As GetAllocationDetailsRequestType) As GetAllocationDetailsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetAllocationDetailsResponseType

        oResponse = DirectCast(oSAMBusiness.GetAllocationDetails(oRequest), GetAllocationDetailsResponseType)

        Return oResponse

    End Function
    'End (Praveen Gora) - (Tech Spec - WR42A_SAM_Party_Enquiry-GetAllocationDetails.doc) - (7.1.3.9)
#End Region

#Region "GetClaimPerilSummary"
    'Start (Prakash C Varghese) - (Tech Spec - UIIC WR25 - MaintainClaim - RiskRelatedDetails - Financials.doc) - (10.2.1)

    ''' <summary>  
    ''' Retrieves financial details for a claim. This is a sub functionality of Maintain Claim process. 
    ''' </summary>  
    ''' <param name="GetClaimPerilSummaryRequest">An object of type SiriusFS.SAM.Structure.SAMForInsurancev2ImplementationTypes.GetClaimPerilSummaryRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.Structure.SAMForInsurancev2ImplementationTypes.GetClaimPerilSummaryResponseType</returns>  
    Public Function GetClaimPerilSummary(ByVal GetClaimPerilSummaryRequest As GetClaimPerilSummaryRequestType) As GetClaimPerilSummaryResponseType

        Dim oCoreBusiness As New CoreSAMBusiness(GetClaimPerilSummaryRequest.BranchCode)
        Dim oResponse As New GetClaimPerilSummaryResponseType

        oResponse = DirectCast(oCoreBusiness.GetClaimPerilSummary(GetClaimPerilSummaryRequest), GetClaimPerilSummaryResponseType)

        Return oResponse
    End Function
    'End (Prakash C Varghese) - (Tech Spec - UIIC WR25 - MaintainClaim - RiskRelatedDetails - Financials.doc) - (10.2.1)
#End Region

#Region "GetClaimCoinsurer"
    'Start (Prakash C Varghese) - (Tech Spec - UIIC WR24 - OpenClaim - Reserve Process - Coinsurer Breakdown.doc) - (7.1.4.2.2)

    ''' <summary>
    ''' Gets the coinsurance breakdown details for a claim. 
    ''' </summary>
    ''' <param name="oRequest">Object of SiriusFS.SAM.Structure.SAMForInsuranceV2ImplementationTypes.GetClaimCoinsurerRequestType class</param>
    ''' <returns >An object of type SiriusFS.SAM.Structure.SAMForInsuranceV2ImplementationTypes.GetClaimCoinsurerResponseType</returns>
    ''' <remarks></remarks>
    Public Function GetClaimCoinsurer(ByVal oRequest As GetClaimCoinsurerRequestType) As GetClaimCoinsurerResponseType

        Dim oCoreImplemtentation As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetClaimCoinsurerResponseType

        oResponse = DirectCast(oCoreImplemtentation.GetClaimCoinsurer(oRequest), GetClaimCoinsurerResponseType)

        Return oResponse
    End Function
    'End (Prakash C Varghese) - (Tech Spec - UIIC WR24 - OpenClaim - Reserve Process - Coinsurer Breakdown.doc) - (7.1.4.2.2)
#End Region
#Region "Get Header And AgentCommission By Key"
    'Start (Vijayakumar Ramasamy)-(Tech Spec - UIICWR50 - MTC - List Risks - Agent Commission.doc)-(7.1.3.9)
    ''' <summary>
    '''GetHeaderAndAgentCommissionByKey is Business logic method  
    '''</summary>
    '''<remarks></remarks>   

    Public Function GetHeaderAndAgentCommissionByKey(ByVal oRequest As GetHeaderAndAgentCommissionByKeyRequestType) As GetHeaderAndAgentCommissionByKeyResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetHeaderAndAgentCommissionByKeyResponseType

        oResponse = DirectCast(oSAMBusiness.GetHeaderAndAgentCommissionByKey(oRequest), GetHeaderAndAgentCommissionByKeyResponseType)

        Return oResponse

    End Function
    'End (Vijayakumar Ramasamy)-(Tech Spec - UIICWR50 - MTC - List Risks - Agent Commission.doc)-(7.1.3.9)
#End Region
#Region "Update Rating Details"
    'Start (Vijayakumar Ramasamy)-(Tech Spec - UIICWR50 - MTC - Risk Premium Details - Update Rating Sections.doc)-(7.1.4.7)
    ''' <summary>
    '''UpdateRatingDetails is Business logic method  
    '''</summary>
    '''<remarks></remarks>
    Public Function UpdateRatingDetails(ByVal oRequest As UpdateRatingDetailsRequestType) As UpdateRatingDetailsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New UpdateRatingDetailsResponseType

        oResponse = DirectCast(oSAMBusiness.UpdateRatingDetails(oRequest), UpdateRatingDetailsResponseType)

        Return oResponse

    End Function

    'End (Vijayakumar Ramasamy)-(Tech Spec - UIICWR50 - MTC - Risk Premium Details - Update Rating Sections.doc)-(7.1.4.7)
#End Region
#Region "Get WorkManager Task Log"
    'Start (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR33 - Work Manager - Task  Log.doc)-(7.1.4.7)
    ''' <summary>
    ''' This is Business logic method for  Get WorkManager Task Log
    '''<param name="oRequest" type="GetWmTaskLogRequestType"></param>   
    '''</summary>
    '''<remarks></remarks>  
    Public Function GetWmTaskLog(ByVal oRequest As GetWmTaskLogRequestType) As GetWmTaskLogResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetWmTaskLogResponseType

        oResponse = DirectCast(oSAMBusiness.GetWmTaskLog(oRequest), GetWmTaskLogResponseType)

        Return oResponse

    End Function
    'End (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR33 - Work Manager - Task  Log.doc)-(7.1.4.7)
#End Region
#Region "Add WorkManager Task Log"
    'Start (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR33 - Work Manager - Task  Log.doc)-(7.2.4.6)
    ''' <summary>
    ''' This is Business logic for  Add WorkManager Task Log
    '''<param name="oRequest" type="AddWmTaskLogRequestType"></param>   
    '''</summary>
    '''<remarks></remarks>  

    Public Function AddWmTaskLog(ByVal oRequest As AddWmTaskLogRequestType) As AddWmTaskLogResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New AddWmTaskLogResponseType

        oResponse = DirectCast(oSAMBusiness.AddWmTaskLog(oRequest), AddWmTaskLogResponseType)

        Return oResponse

    End Function
    'End (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR33 - Work Manager - Task  Log.doc)-(7.2.4.6)
#End Region
#Region "Get Balances And Unallocated Credits"
    'Start (Vijayakumar Ramasamy)-(Tech Spec - UIICWR23 - New Business - Pre Payment Functionality.doc)-(7.1.3.9)
    ''' <summary>
    ''' This is business logic layer for GetBalancesAndUnallocatedCredits
    '''<param name="oRequest" type="GetBalancesAndUnallocatedCreditsRequestType"></param>   
    '''</summary>
    '''<remarks></remarks>  


    Public Function GetBalancesAndUnallocatedCredits(ByVal oRequest As GetBalancesAndUnallocatedCreditsRequestType) As GetBalancesAndUnallocatedCreditsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetBalancesAndUnallocatedCreditsResponseType

        oResponse = DirectCast(oSAMBusiness.GetBalancesAndUnallocatedCredits(oRequest), GetBalancesAndUnallocatedCreditsResponseType)

        Return oResponse

    End Function

    'End (Vijayakumar Ramasamy)-(Tech Spec - UIICWR23 - New Business - Pre Payment Functionality.doc)-(7.1.3.9)
#End Region
#Region "AddQuoteV2"
    'Start (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR22 – Capture Quote Details – Add Quote.doc)-(7.1.5.1)
    '''<summary>
    ''' This method Business logic method for AddQuoteV2
    '''</summary>
    '''<param name="oRequest" type="AddQuoteV2RequestType"></param>   
    '''<returns>AddQuoteV2ResponseType</returns>
    '''<remarks></remarks> 

    Public Function AddQuoteV2(ByVal oRequest As AddQuoteV2RequestType) As AddQuoteV2ResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New AddQuoteV2ResponseType

        oResponse = DirectCast(oSAMBusiness.AddQuoteV2(oRequest), AddQuoteV2ResponseType)

        Return oResponse

    End Function
    'End (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR22 – Capture Quote Details – Add Quote.doc)-(7.1.5.1)
#End Region
#Region "AddTaskGroup"
    'Start (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR01 - User Access - Add Task Group.doc)-(7.1.5.7)
    '''<summary>
    ''' This method Business logic method for AddTaskGroup
    '''</summary>
    '''<param name="oRequest" type="AddTaskGroupRequestType"></param>   
    '''<returns>AddTaskGroupResponseType</returns>
    '''<remarks></remarks> 
    Public Function AddTaskGroup(ByVal oRequest As AddTaskGroupRequestType) As AddTaskGroupResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New AddTaskGroupResponseType

        oResponse = DirectCast(oSAMBusiness.AddTaskGroup(oRequest), AddTaskGroupResponseType)

        Return oResponse

    End Function
    'End (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR01 - User Access - Add Task Group.doc)-(7.1.5.7)
#End Region

#Region "GetCoverNoteBook"
    'Start (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR53 - Cover Note Maintenance - Get Cover Note Book Details.doc)-(7.1.4.7)
    '''<summary>
    ''' This method Business logic method for AddTaskGroup
    '''</summary>
    '''<param name="oRequest" type="GetCoverNoteBookRequestType"></param>   
    '''<returns>GetCoverNoteBookResponseType</returns>
    '''<remarks></remarks>
    Public Function GetCoverNoteBook(ByVal oRequest As GetCoverNoteBookRequestType) As GetCoverNoteBookResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetCoverNoteBookResponseType

        oResponse = DirectCast(oSAMBusiness.GetCoverNoteBook(oRequest), GetCoverNoteBookResponseType)

        Return oResponse

    End Function
    'Start (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR53 - Cover Note Maintenance - Get Cover Note Book Details.doc)-(7.1.4.7)
#End Region
#Region "Get Policies On BankGuarantee For Receipt"
    'Start (Vijayakumar Ramasamy)-(Tech Spec - UIICWR6 - Get Policies on Bank Guarantee For Receipt.doc)-(7.1.5.6)
    ''' <summary>
    ''' This is Business method for GetPoliciesOnBankGuaranteeForReceiptForReceipt
    '''<param name="oGetPoliciesOnBankGuaranteeForReceiptForReceiptRequest" type="GetPoliciesOnBankGuaranteeForReceiptForReceiptRequestType"></param>   
    '''<returns>GetPoliciesOnBankGuaranteeForReceiptResponseType</returns>
    '''</summary>
    '''<remarks></remarks> 
    Public Function GetPoliciesOnBankGuaranteeForReceipt(ByVal oRequest As GetPoliciesOnBankGuaranteeForReceiptRequestType) As GetPoliciesOnBankGuaranteeForReceiptResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetPoliciesOnBankGuaranteeForReceiptResponseType

        oResponse = DirectCast(oSAMBusiness.GetPoliciesOnBankGuaranteeForReceipt(oRequest), GetPoliciesOnBankGuaranteeForReceiptResponseType)

        Return oResponse

    End Function
    'End (Vijayakumar Ramasamy)-(Tech Spec - UIICWR6 - Get Policies on Bank Guarantee For Receipt.doc)-(7.1.5.6)
#End Region
#Region "Get Policies On BankGuarantee By Key"
    'Start (Vijayakumar Ramasamy)-(Tech Spec - UIICWR6 - Get Policies on Bank Guarantee By Key.doc)-(7.1.5.6)
    ''' <summary>
    ''' This is Business method for GetPoliciesOnBankGuarantee By Key
    '''<param name="oRequest" type="GetPoliciesOnBankGuaranteeByKeyRequestType"></param>   
    '''<returns>GetPoliciesOnBankGuaranteeByKeyResponseType</returns>
    '''</summary>
    '''<remarks></remarks> 
    Public Function GetPoliciesOnBankGuaranteeByKey(ByVal oRequest As GetPoliciesOnBankGuaranteeByKeyRequestType) As GetPoliciesOnBankGuaranteeByKeyResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetPoliciesOnBankGuaranteeByKeyResponseType

        oResponse = DirectCast(oSAMBusiness.GetPoliciesOnBankGuaranteeByKey(oRequest), GetPoliciesOnBankGuaranteeByKeyResponseType)

        Return oResponse

    End Function
    'End (Vijayakumar Ramasamy)-(Tech Spec - UIICWR6 - Get Policies on Bank Guarantee By Key.doc)-(7.1.5.6)
#End Region
#Region "GetUserGroupsbyTask"
    'Start (Vijayakumar Ramasamy)-(Tech Spec - Get User Groups By Task Group.doc)-(7.1.4.7)
    ''' <summary>
    ''' This   business method logic for Get UserGroups by Task
    '''<param name="oRequest" type="GetUserGroupsbyTaskRequestType"></param>   
    '''<returns>GetUserGroupsbyTaskResponseType</returns>
    '''</summary>
    '''<remarks></remarks> 
    Public Function GetUserGroupsbyTask(ByVal oRequest As GetUserGroupsbyTaskRequestType) As GetUserGroupsbyTaskResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetUserGroupsbyTaskResponseType

        oResponse = DirectCast(oSAMBusiness.GetUserGroupsbyTask(oRequest), GetUserGroupsbyTaskResponseType)

        Return oResponse

    End Function
    'End (Vijayakumar Ramasamy)-(Tech Spec - Get User Groups By Task Group.doc)-(7.1.4.7)
#End Region

#Region "GetAgentDetailsForPolicy"
    'Start (Vijayakumar Ramasamy)-(Tech Specs - Batch3_SAM_GetAgentDetailsForClaims.doc)-(7.1.4.7)
    ''' <summary>
    ''' This   business method logic for GetAgentDetailsForPolicy
    '''<param name="oRequest" type="GetAgentDetailsForPolicyRequestType"></param>   
    '''<returns>GetAgentDetailsForPolicyResponseType</returns>
    '''</summary>
    '''<remarks></remarks> 
    Public Function GetAgentDetailsForPolicy(ByVal oRequest As GetAgentDetailsForPolicyRequestType) As GetAgentDetailsForPolicyResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetAgentDetailsForPolicyResponseType
        oResponse = DirectCast(oSAMBusiness.GetAgentDetailsForPolicy(oRequest), GetAgentDetailsForPolicyResponseType)
        Return oResponse
    End Function
    'End (Vijayakumar Ramasamy)-(Tech Specs - Batch3_SAM_GetAgentDetailsForClaims.doc)-(7.1.4.7)
#End Region
#Region "GetTaxGroupsForClaims"
    'Start (Vijayakumar Ramasamy)-(Tech Specs - Batch3_SAM_GetTaxGroupForClaims.doc)-(7.1.4.7)
    ''' <summary>
    ''' This   business method logic for GetAgentDetailsForPolicy
    '''<param name="oRequest" type="GetTaxGroupsForClaimsRequestType"></param>   
    '''<returns>GetTaxGroupsForClaimsResponseType</returns>
    '''</summary>
    '''<remarks></remarks> 
    Public Function GetTaxGroupsForClaims(ByVal oRequest As GetTaxGroupsForClaimsRequestType) As GetTaxGroupsForClaimsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetTaxGroupsForClaimsResponseType
        oResponse = DirectCast(oSAMBusiness.GetTaxGroupsForClaims(oRequest), GetTaxGroupsForClaimsResponseType)
        Return oResponse
    End Function
    'End (Vijayakumar Ramasamy)-(Tech Specs - Batch3_SAM_GetTaxGroupForClaims.doc)-(7.1.4.7)
#End Region

#Region "MarkUnmarkTransaction"
    'Start (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR63 - Insurer payments - Mark & Unmark.doc)-(7.1.3.7)
    ''' <summary>
    ''' This   business method logic for MarkUnmarkTransaction
    '''<param name="oRequest" type="MarkUnmarkTransactionRequestType"></param>   
    '''<returns>MarkUnmarkTransactionResponseType</returns>
    '''</summary>
    '''<remarks></remarks> 
    Public Function MarkUnmarkTransaction(ByVal oRequest As MarkUnmarkTransactionRequestType) As MarkUnmarkTransactionResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New MarkUnmarkTransactionResponseType

        oResponse = DirectCast(oSAMBusiness.MarkUnmarkTransaction(oRequest), MarkUnmarkTransactionResponseType)

        Return oResponse

    End Function
    'End (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR63 - Insurer payments - Mark & Unmark.doc)-(7.1.3.7)
#End Region
#Region " Get Claim Reinsurance Bands "

    'Start (Sankar)-(Tech Spec - UIIC WR25 - Maintain Claims - Reinsurance Breakdown.doc)-(7.1.3.7)
    ''' <summary>  
    ''' Retrieves Reinsurance Band details for a claim. 
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.GetClaimReinsuranceBandsRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.GetClaimReinsuranceBandsResponseType</returns>  
    Public Function GetClaimReinsuranceBands(ByVal oRequest As GetClaimReinsuranceBandsRequestType) As GetClaimReinsuranceBandsResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetClaimReinsuranceBandsResponseType

        oResponse = DirectCast(oSAMBusiness.GetClaimReinsuranceBands(oRequest), GetClaimReinsuranceBandsResponseType)
        Return oResponse
    End Function
    'End (Sankar)-(Tech Spec - UIIC WR25 - Maintain Claims - Reinsurance Breakdown.doc)-(7.1.3.7)
#End Region

#Region " Get Claim Reinsurance Arrangements "

    'Start (Sankar)-(Tech Spec - UIIC WR25 - Maintain Claims - Reinsurance Breakdown.doc)-(7.2.3.7)
    ''' <summary>  
    ''' Retrieves Reinsurance Arrangement details for a claim. 
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.GetClaimReinsuranceArrangementsRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.GetClaimReinsuranceArrangementsResponseType</returns>  
    Public Function GetClaimReinsuranceArrangements(ByVal oRequest As GetClaimReinsuranceArrangementsRequestType) As GetClaimReinsuranceArrangementsResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetClaimReinsuranceArrangementsResponseType

        oResponse = DirectCast(oSAMBusiness.GetClaimReinsuranceArrangements(oRequest), GetClaimReinsuranceArrangementsResponseType)
        Return oResponse
    End Function
    'End (Sankar)-(Tech Spec - UIIC WR25 - Maintain Claims - Reinsurance Breakdown.doc)-(7.2.3.7)
#End Region

#Region " Get Claim Reinsurance Arrangement Lines "

    'Start (Sankar)-(Tech Spec - UIIC WR25 - Maintain Claims - Reinsurance Breakdown.doc)-(7.3.3.7)
    ''' <summary>  
    ''' Retrieves Reinsurance Arrangement Lines details for a claim. 
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.GetClaimReinsuranceArrangementLinesRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.GetClaimReinsuranceArrangementLinesResponseType</returns>  
    Public Function GetClaimReinsuranceArrangementLines(ByVal oRequest As GetClaimReinsuranceArrangementLinesRequestType) As GetClaimReinsuranceArrangementLinesResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetClaimReinsuranceArrangementLinesResponseType

        oResponse = DirectCast(oSAMBusiness.GetClaimReinsuranceArrangementLines(oRequest), GetClaimReinsuranceArrangementLinesResponseType)
        Return oResponse
    End Function
    'End (Sankar)-(Tech Spec - UIIC WR25 - Maintain Claims - Reinsurance Breakdown.doc)-(7.3.3.7)
#End Region

#Region " Get Risk Reinsurance Bands "
    'Start (Sankar)-(Tech Spec - UIIC WR22 - Capture Quote Details - Reinsurance Details.doc)-(7.1.5.7)
    ''' <summary>  
    ''' Returns Reinsurance Band details to the Service Layer.
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.GetRiskReinsuranceBandsRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.GetRiskReinsuranceBandsResponseType</returns>  
    Public Function GetRiskReinsuranceBands(ByVal oRequest As GetRiskReinsuranceBandsRequestType) As GetRiskReinsuranceBandsResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetRiskReinsuranceBandsResponseType
        oResponse = DirectCast(oSAMBusiness.GetRiskReinsuranceBands(oRequest), GetRiskReinsuranceBandsResponseType)
        Return oResponse
    End Function
    'End (Sankar)-(Tech Spec - UIIC WR22 - Capture Quote Details - Reinsurance Details.doc)-(7.1.5.7)
#End Region

#Region " Get Risk Reinsurance Arrangements "
    'Start (Sankar)-(Tech Spec - UIIC WR22 - Capture Quote Details - Reinsurance Details.doc)-(7.2.4.7)
    ''' <summary>  
    ''' Returns Reinsurance Arrangement details to the Service Layer. 
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.GetRiskReinsuranceArrangementsRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.GetRiskReinsuranceArrangementsResponseType</returns>  
    Public Function GetRiskReinsuranceArrangements(ByVal oRequest As GetRiskReinsuranceArrangementsRequestType) As GetRiskReinsuranceArrangementsResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetRiskReinsuranceArrangementsResponseType
        oResponse = DirectCast(oSAMBusiness.GetRiskReinsuranceArrangements(oRequest), GetRiskReinsuranceArrangementsResponseType)
        Return oResponse
    End Function
    'End (Sankar)-(Tech Spec - UIIC WR22 - Capture Quote Details - Reinsurance Details.doc)-(7.2.4.7)
#End Region

#Region " Get Risk Reinsurance Arrangement Lines "
    'Start (Sankar)-(Tech Spec - UIIC WR22 - Capture Quote Details - Reinsurance Details.doc)-(7.3.5.7)
    ''' <summary>  
    ''' Returns Reinsurance Arrangement Lines details to the Service Layer. 
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.GetRiskReinsuranceArrangementLinesRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.GetRiskReinsuranceArrangementLinesResponseType</returns>  
    Public Function GetRiskReinsuranceArrangementLines(ByVal oRequest As GetRiskReinsuranceArrangementLinesRequestType) As GetRiskReinsuranceArrangementLinesResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetRiskReinsuranceArrangementLinesResponseType
        oResponse = DirectCast(oSAMBusiness.GetRiskReinsuranceArrangementLines(oRequest), GetRiskReinsuranceArrangementLinesResponseType)
        Return oResponse
    End Function
    'End (Sankar)-(Tech Spec - UIIC WR22 - Capture Quote Details - Reinsurance Details.doc)-(7.3.5.7)
#End Region

#Region " DeleteWmTask "
    'Start (Sankar)-(Tech Spec - UIIC WR33 - Work Manager - Delete Task.doc)-(7.1.4.7)
    ''' <summary>  
    '''  This method calls core implementation layer and deletes a task based on TaskInstanceKey.
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.DeleteWmTaskRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.DeleteWmTaskResponseType</returns>  
    Public Function DeleteWmTask(ByVal oRequest As DeleteWmTaskRequestType) As DeleteWmTaskResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New DeleteWmTaskResponseType
        oResponse = DirectCast(oSAMBusiness.DeleteWmTask(oRequest), DeleteWmTaskResponseType)
        Return oResponse
    End Function
    'End (Sankar)-(Tech Spec - UIIC WR33 - Work Manager - Delete Task.doc)-(7.1.4.7)
#End Region

#Region " UpdateWmTask "
    'Start (Sankar)-(Tech Spec - UIIC WR33 - Work Manager - Update Task.doc)-(7.2.4.6)
    ''' <summary>  
    '''  This method calls core implementation layer and updates a task.
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.UpdateWmTaskRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.UpdateWmTaskResponseType</returns>  
    Public Function UpdateWmTask(ByVal oRequest As UpdateWmTaskRequestType) As UpdateWmTaskResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New UpdateWmTaskResponseType
        oResponse = DirectCast(oSAMBusiness.UpdateWmTask(oRequest), UpdateWmTaskResponseType)
        Return oResponse
    End Function
    'End (Sankar)-(Tech Spec - UIIC WR33 - Work Manager - Update Task.doc)-(7.2.4.6)
#End Region

#Region " ReAssignMultipleWmTasks "
    'Start (PraveenGora) - (Tech Spec - UIIC WR33 - Work Manager -  ReAssign Multiple Tasks.doc) - (7.1.4.6)
    ''' <summary>  
    '''  This method calls core implementation layer and updates a task.
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.ReAssignMultipleWmTasksRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.ReAssignMultipleWmTasksResponseType</returns>  

    Public Function ReAssignMultipleWmTasks(ByVal oRequest As ReAssignMultipleWmTasksRequestType) As ReAssignMultipleWmTasksResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New ReAssignMultipleWmTasksResponseType

        oResponse = DirectCast(oSAMBusiness.ReAssignMultipleWmTasks(oRequest), ReAssignMultipleWmTasksResponseType)

        Return oResponse

    End Function
    'End (PraveenGora) - (Tech Spec - UIIC WR33 - Work Manager -  ReAssign Multiple Tasks.doc) - (7.1.4.6)
#End Region

#Region " DeleteUndeleteUserGroup "
    'Start (PraveenGora) - (Tech Spec - UIIC WR01 - User Access - Delete Undelete User Group.doc) - (7.1.5.7) 
    ''' <summary>  
    '''  This method calls core implementation layer and deletes an User group.
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.DeleteUndeleteUserGroupRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.DeleteUndeleteUserGroupResponseType</returns>  
    Public Function DeleteUndeleteUserGroup(ByVal oRequest As DeleteUndeleteUserGroupRequestType) As DeleteUndeleteUserGroupResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New DeleteUndeleteUserGroupResponseType

        oResponse = DirectCast(oSAMBusiness.DeleteUndeleteUserGroup(oRequest), DeleteUndeleteUserGroupResponseType)

        Return oResponse

    End Function
    'End (PraveenGora) - (Tech Spec - UIIC WR01 - User Access - Delete Undelete User Group.doc) - (7.1.5.7)
#End Region

#Region " UpdateTaskGroups "
    'Start (PraveenGora) - (Tech Spec - UIIC WR01 - User Access - Update Task Groups.doc) - (7.1.5.7)   
    ''' <summary>  
    '''  This method calls core implementation layer and updated user groups.
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.UpdateTaskGroupsRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.UpdateTaskGroupsResponseType</returns>  
    Public Function UpdateTaskGroups(ByVal oRequest As UpdateTaskGroupsRequestType) As UpdateTaskGroupsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New UpdateTaskGroupsResponseType

        oResponse = DirectCast(oSAMBusiness.UpdateTaskGroups(oRequest), UpdateTaskGroupsResponseType)

        Return oResponse

    End Function
    'End (PraveenGora) - (Tech Spec - UIIC WR01 - User Access - Update Task Groups.doc) - (7.1.5.7) 
#End Region

#Region "Get Coinsurance Values"
    'Start (Vivek Athalye) - (Tech Spec - UIIC WR22 - Capture Quote Details - Co-Insurance.doc) - (7.1.5.7)
    ''' <summary>
    ''' This method is used to get CoInsurance details (arrangement and values)
    '''</summary>
    '''<param name="oRequest" type="GetCoinsuranceValuesRequestType"></param>   
    '''<returns>GetCoinsuranceValuesResponseType</returns>
    '''<remarks></remarks> 
    Public Function GetCoinsuranceValues(ByVal oRequest As GetCoinsuranceValuesRequestType) As GetCoinsuranceValuesResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetCoinsuranceValuesResponseType

        oResponse = DirectCast(oSAMBusiness.GetCoinsuranceValues(oRequest), GetCoinsuranceValuesResponseType)

        Return oResponse

    End Function
    'End (Vivek Athalye) - (Tech Spec - UIIC WR22 - Capture Quote Details - Co-Insurance.doc) - (7.1.5.7)

#End Region

#Region "Update Coinsurance Values"
    'Start (Vivek Athalye) - (Tech Spec - UIIC WR22 - Capture Quote Details - Co-Insurance.doc) - (7.3.5.7)
    ''' <summary>
    ''' This method is used to update CoInsurance details (arrangement and values)
    '''</summary>
    '''<param name="oRequest" type="UpdateCoinsuranceValuesRequestType"></param>   
    '''<returns>UpdateCoinsuranceValuesResponseType</returns>
    '''<remarks></remarks>
    Public Function UpdateCoinsuranceValues(ByVal oRequest As UpdateCoinsuranceValuesRequestType) As UpdateCoinsuranceValuesResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New UpdateCoinsuranceValuesResponseType

        oResponse = DirectCast(oSAMBusiness.UpdateCoinsuranceValues(oRequest), UpdateCoinsuranceValuesResponseType)

        Return oResponse

    End Function
    'End (Vivek Athalye) - (Tech Spec - UIIC WR22 - Capture Quote Details - Co-Insurance.doc) - (7.3.5.7)

#End Region

#Region "Get Coinsurance Defaults"
    'Start (Vivek Athalye) - (Tech Spec - UIIC WR22 - Capture Quote Details - Co-Insurance.doc) - (7.2.5.5)
    '''<summary>
    ''' This method is used to get CoInsurance default values
    '''</summary>
    '''<param name="oRequest" type="GetCoinsuranceDefaultsRequestType"></param>   
    '''<returns>GetCoinsuranceDefaultsResponseType</returns>
    '''<remarks></remarks> 
    Public Function GetCoinsuranceDefaults(ByVal oRequest As GetCoinsuranceDefaultsRequestType) As GetCoinsuranceDefaultsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetCoinsuranceDefaultsResponseType

        oResponse = DirectCast(oSAMBusiness.GetCoinsuranceDefaults(oRequest), GetCoinsuranceDefaultsResponseType)

        Return oResponse

    End Function
    'End (Vivek Athalye) - (Tech Spec - UIIC WR22 - Capture Quote Details - Co-Insurance.doc) - (7.2.5.5)
#End Region

#Region " UpdateQuoteV2 "
    'Start (Sankar)-(Tech Spec - UIIC WR22 - Capture Quote Details - Update Quote.doc)-(7.1.5.1)
    ''' <summary>  
    '''  This method calls core implementation layer and updates a Quote.
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.UpdateQuoteV2RequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.UpdateQuoteV2ResponseType</returns>  
    Public Function UpdateQuoteV2(ByVal oRequest As UpdateQuoteV2RequestType) As UpdateQuoteV2ResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New UpdateQuoteV2ResponseType
        oResponse = DirectCast(oSAMBusiness.UpdateQuoteV2(oRequest), UpdateQuoteV2ResponseType)
        Return oResponse
    End Function
    'End (Sankar)-(Tech Spec - UIIC WR22 - Capture Quote Details - Update Quote.doc)-(7.1.5.1)
#End Region

#Region " GetUserGroupUsers "

    'Start (Sankar)-(Tech Spec - UIIC WR01 - User Access - Get User Group Users.doc)-(7.1.5.7)
    ''' <summary>  
    '''  This method calls GetUserGroupUsers of core implementation layer and fetches user group's users.
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.GetUserGroupUsersRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.GetUserGroupUsersResponseType</returns>  
    Public Function GetUserGroupUsers(ByVal oRequest As GetUserGroupUsersRequestType) As GetUserGroupUsersResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetUserGroupUsersResponseType

        oResponse = DirectCast(oSAMBusiness.GetUserGroupUsers(oRequest), GetUserGroupUsersResponseType)

        Return oResponse

    End Function
    'End (Sankar)-(Tech Spec - UIIC WR01 - User Access - Get User Group Users.doc)-(7.1.5.7)
#End Region

#Region "GetAccountDetails"
    'Start (Vivek Athalye) - (Tech Specs - WR42A_SAM_Party_Enquiry-GetAccountDetails.doc) - (7.1.4.7)
    '''<summary>
    ''' This method is used to get Account Details
    '''</summary>
    '''<param name="oRequest" type="GetAccountDetailsRequestType"></param>   
    '''<returns>GetAccountDetailsResponseType</returns>
    '''<remarks></remarks> 
    Public Function GetAccountDetails(ByVal oRequest As GetAccountDetailsRequestType) As GetAccountDetailsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetAccountDetailsResponseType

        oResponse = DirectCast(oSAMBusiness.GetAccountDetails(oRequest), GetAccountDetailsResponseType)

        Return oResponse

    End Function
    'End (Vivek Athalye) - (Tech Specs - WR42A_SAM_Party_Enquiry-GetAccountDetails.doc) - (7.1.4.7)
#End Region

#Region " FindCoverNoteBooks "
    'Start (Sankar)-(Tech Spec - UIIC WR53 - Cover Note Maintenance - Find Cover Note Book.doc)-(7.1.4.7)
    ''' <summary>  
    '''  This method calls the core implementation layer to find Cover Note Books.   
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.FindCoverNoteBooksRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.FindCoverNoteBooksResponseType</returns>  
    Public Function FindCoverNoteBooks(ByVal oRequest As FindCoverNoteBooksRequestType) As FindCoverNoteBooksResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New FindCoverNoteBooksResponseType
        oResponse = DirectCast(oSAMBusiness.FindCoverNoteBooks(oRequest), FindCoverNoteBooksResponseType)
        Return oResponse
    End Function
    'End (Sankar)-(Tech Spec - UIIC WR53 - Cover Note Maintenance - Find Cover Note Book.doc)-(7.1.4.7)
#End Region

#Region " Update Allocation "
    'Start (Sankar)-(Tech Specs - UIICWR61 Cash Cheque Payment - UpdateAllocation)-(7.1.4.7)
    ''' <summary>  
    '''  This method calls the core implementation layer to update allocation in cash cheque payment.   
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.UpdateAllocationRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.UpdateAllocationResponseType</returns>  
    Public Function UpdateAllocation(ByVal oRequest As UpdateAllocationRequestType) As UpdateAllocationResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New UpdateAllocationResponseType

        oResponse = DirectCast(oSAMBusiness.UpdateAllocation(oRequest), UpdateAllocationResponseType)

        Return oResponse
    End Function
    'End (Sankar)-(Tech Specs - UIICWR61 Cash Cheque Payment - UpdateAllocation)-(7.1.4.7)
#End Region

#End Region

#Region " Private methods"
    ' Untested code!!!!!!!!!!!!!
    ' TODO test and finish
    'Private Function ArrayToBaseBranchType(ByVal oSourceList As Object) As BaseImplementationTypes.BaseBranchType()

    '    Dim vSourceListBBT() As BaseImplementationTypes.BaseBranchType
    '    Dim vSourceListArray(0, 0) As Array
    '    Dim iRowFrom As Int16
    '    Dim iRowTo As Int16
    '    Dim iRow As Int16

    '    ' If we do not have an Array then return Nothing
    '    If vSourceListArray Is Nothing Then
    '        Return Nothing
    '    End If

    '    ' More to do here, need to trap if there are no results, therefore No Array
    '    Try
    '        If oSourceList.GetType Is GetType(System.Array) Then
    '            Return Nothing
    '        End If
    '    Catch ex As Exception
    '        Return Nothing
    '    End Try

    '    iRowFrom = oSourceList.GetLowerBound(1)
    '    iRowTo = oSourceList.GetUpperBound(1)

    '    ' Populate the Table
    '    For iRow = iRowFrom To iRowTo
    '        ' Create a New Row            
    '        ReDim Preserve vSourceListBBT(iRow)
    '        vSourceListBBT(iRow) = New BaseImplementationTypes.BaseBranchType
    '        ' Get rid of trailing spaces. PN27820.
    '        vSourceListBBT(iRow).BranchCode = oSourceList(1, iRow).ToString.Trim
    '        vSourceListBBT(iRow).Description = oSourceList(2, iRow).ToString.Trim
    '    Next

    '    Return vSourceListBBT

    'End Function

    Private Function ClaimKeyChecks(ByVal lClaimKey As Integer, ByVal sMethodName As String) As SiriusFS.SAM.Structure.BaseImplementationTypes.STSErrorType

        Dim oCoreBusiness As New CoreBusiness
        Dim oSTSError As New STSErrorPublisher
        Dim oResponse As SiriusFS.SAM.Structure.BaseImplementationTypes.STSErrorType = Nothing
        Using con As SiriusConnection = SiriusConnection.FromAny(SAMFunc.ConnectionString)


            If lClaimKey = 0 Then
                oSTSError.AddInvalidField("ClaimKey", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "ClaimKey"), "")
                oSTSError.SetContext(oResponse, HttpContext.Current.Request.Url.ToString(), sMethodName, "Mandatory field validation", True)
                Return oResponse
            End If

            If oCoreBusiness.CheckClaimKey(con, lClaimKey) = False Then
                oSTSError = New STSErrorPublisher(STSErrorCodes.AgentRecordNotFound, "Claim Key validation failed", "The Claim does not exist for key: " & lClaimKey)
                oSTSError.SetContext(oResponse, HttpContext.Current.Request.Url.ToString(), sMethodName, "Key Validation", True)
                Return oResponse
            End If
        End Using
        Return oResponse

    End Function



#End Region

#Region "V2 Public Methods"
    '''<summary>
    '''Tech Spec - UIIC WR24 - OpenClaim - Check Unpaid Premium.doc,7.1.2.2.2
    '''Developed By: Jigar Pandya On: 09 May 2008
    '''</summary>
    '''<param name="oRequest">CheckUnpaidPremiumRequestType</param>
    '''<remarks></remarks>
    Public Function CheckUnpaidPremium(ByVal oRequest As CheckUnpaidPremiumRequestType) As CheckUnpaidPremiumResponseType
        Dim coreImplemtentation As New CoreSAMBusiness(oRequest.BranchCode)
        Dim response As New CheckUnpaidPremiumResponseType

        response = DirectCast(coreImplemtentation.CheckUnpaidPremium(oRequest), CheckUnpaidPremiumResponseType)
        Return response
    End Function
#End Region

#Region "Public Method-GetRecoveryCoinsurance"
    'Start(Sriram P)-(Tech Spec - UIIC WR24 - OpenClaim - Reserve Process - Coinsurance - Recoveries.doc)-(7.1.2.2.2)
    ''' <summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="GetRecoveryCoinsuranceRequestType"></param>
    '''<remarks></remarks>

    Public Function GetRecoveryCoinsurance(ByVal oRequest As GetRecoveryCoinsuranceRequestType) As GetRecoveryCoinsuranceResponseType

        Dim oCoreImplemtentation As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetRecoveryCoinsuranceResponseType

        oResponse = DirectCast(oCoreImplemtentation.GetRecoveryCoinsurance(oRequest), GetRecoveryCoinsuranceResponseType)

        Return oResponse
    End Function
    'End(Sriram P)-(Tech Spec - UIIC WR24 - OpenClaim - Reserve Process - Coinsurance - Recoveries.doc)-(7.1.2.2.2)
#End Region
#Region "Public Method-GetWmTask"
    'Start(Sriram P)-(Tech Spec - UIIC WR33 - Work Manager - View Task.doc)-(7.1.4.7)
    ''' <summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="GetWmTaskRequestType"></param>
    '''<remarks></remarks>
    Public Function GetWmTask(ByVal oRequest As GetWmTaskRequestType) As GetWmTaskResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetWmTaskResponseType

        oResponse = DirectCast(oSAMBusiness.GetWmTask(oRequest), GetWmTaskResponseType)

        Return oResponse

    End Function
    'End (Sriram P)-(Tech Spec - UIIC WR33 - Work Manager - View Task.doc)-(7.1.4.7)
#End Region
#Region "GetSubAgents"
    'Start (Girija chokkalingam) - (Tech Spec - UIIC WR22 – Capture Quote Details – Sub Agents.doc) - (7.1.5.7)    ''' <summary>
    '''<summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="GetSubAgentsRequestType"></param>
    '''<remarks></remarks>
    Public Function GetSubAgents(ByVal oRequest As GetSubAgentsRequestType) As GetSubAgentsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetSubAgentsResponseType
        oResponse = DirectCast(oSAMBusiness.GetSubAgents(oRequest), GetSubAgentsResponseType)
        Return oResponse

    End Function
    'End Girija chokkalingam) - (Tech Spec - UIIC WR22 – Capture Quote Details – Sub Agents.doc) - (7.1.5.7)  
#End Region
#Region "UpdateSubAgents"
    'Start (Girija chokkalingam) - (Tech Spec - UIIC WR22 – Capture Quote Details – Sub Agents.doc) - (7.2.5.7)
    ''' <summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="UpdateSubAgentsRequestType"></param>
    '''<remarks></remarks>
    Public Function UpdateSubAgents(ByVal oRequest As UpdateSubAgentsRequestType) As UpdateSubAgentsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New UpdateSubAgentsResponseType
        oResponse = DirectCast(oSAMBusiness.UpdateSubAgents(oRequest), UpdateSubAgentsResponseType)
        Return oResponse

    End Function
    'End (Girija chokkalingam) - (Tech Spec - UIIC WR22 – Capture Quote Details – Sub Agents.doc) - (7.2.5.7)  
#End Region
#Region "GetStandardPolicyWordings"
    'Start (Girija chokkalingam) - (Tech Spec - UIIC WR22 – Capture Quote Details – Policy Standard Wordings.doc) - (7.1.5.7)
    '''<summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="GetStandardPolicyWordingsRequestType"></param>
    '''<remarks></remarks>
    Public Function GetStandardPolicyWordings(ByVal oRequest As GetStandardPolicyWordingsRequestType) As GetStandardPolicyWordingsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetStandardPolicyWordingsResponseType

        oResponse = DirectCast(oSAMBusiness.GetStandardPolicyWordings(oRequest), GetStandardPolicyWordingsResponseType)

        Return oResponse

    End Function

    'End (Girija chokkalingam) - (Tech Spec - UIIC WR22 – Capture Quote Details – Policy Standard Wordings.doc) - (7.2.5.7)
#End Region
#Region "UpdateStandardPolicyWordings"
    'Start (Girija chokkalingam) - (Tech Spec - UIIC WR22 – Capture Quote Details – Policy Standard Wordings.doc) - (7.2.5.7)
    '''<summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="UpdateStandardPolicyWordingsRequestType"></param>
    '''<remarks></remarks>
    Public Function UpdateStandardPolicyWordings(ByVal oRequest As UpdateStandardPolicyWordingsRequestType) As UpdateStandardPolicyWordingsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New UpdateStandardPolicyWordingsResponseType

        oResponse = DirectCast(oSAMBusiness.UpdateStandardPolicyWordings(oRequest), UpdateStandardPolicyWordingsResponseType)

        Return oResponse

    End Function

    'End (Girija chokkalingam) - (Tech Spec - UIIC WR22 – Capture Quote Details – Policy Standard Wordings.doc) - (7.1.5.7)
#End Region
#Region "FindDocumentTemplates"
    'Start (Girija chokkalingam) - (Tech Spec - UIIC WR22 – Capture Quote Details – Policy Standard Wordings.doc) - (7.3.5.7)
    '''<summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="FindDocumentTemplatesRequestType"></param>
    '''<remarks></remarks>
    Public Function FindDocumentTemplates(ByVal oRequest As FindDocumentTemplatesRequestType) As FindDocumentTemplatesResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New FindDocumentTemplatesResponseType

        oResponse = DirectCast(oSAMBusiness.FindDocumentTemplates(oRequest), FindDocumentTemplatesResponseType)

        Return oResponse

    End Function

    'End (Girija chokkalingam) - (Tech Spec - UIIC WR22 – Capture Quote Details – Policy Standard Wordings.doc) - (7.3.5.7)
#End Region
#Region "Public Method-GetEventDetails"
    'Start(Sriram P)-(Tech Specs - WR42A_SAM_Party_Enquiry - GetEventDetails.doc)-(7.1.4.7)
    ''' <summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="GetEventDetailsRequestType"></param>
    '''<remarks></remarks>
    Public Function GetEventDetails(ByVal oRequest As GetEventDetailsRequestType) As GetEventDetailsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetEventDetailsResponseType

        oResponse = DirectCast(oSAMBusiness.GetEventDetails(oRequest), GetEventDetailsResponseType)

        Return oResponse

    End Function
    'End(Sriram P)-(Tech Specs - WR42A_SAM_Party_Enquiry - GetEventDetails.doc)-(7.1.4.7)
#End Region
#Region "Public Method-AddEvent"
    'Start(Sriram P)-(Tech Specs - WR42A_SAM_Party_Enquiry - AddEvent.doc)-(7.1.4.7)
    ''' <summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="AddEventRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.AddEventResponseType</returns>  
    '''<remarks></remarks>
    Public Function AddEvent(ByVal oRequest As AddEventRequestType) As AddEventResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New AddEventResponseType
        oResponse = DirectCast(oSAMBusiness.AddEvent(oRequest), AddEventResponseType)
        Return oResponse

    End Function
    'End(Sriram P)-(Tech Specs - WR42A_SAM_Party_Enquiry - AddEvent.doc)-(7.1.4.7)

#End Region
#Region "Public Method-AddEventNote"
    'Start(Sriram P)-(Tech Specs - WR42A_SAM_Party_Enquiry - AddEventNote.doc)-(7.1.4.7)
    ''' <summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="AddEventNoteRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.AddEventNoteResponseType</returns>  
    '''<remarks></remarks>
    Public Function AddEventNote(ByVal oRequest As AddEventNoteRequestType) As AddEventNoteResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New AddEventNoteResponseType

        oResponse = DirectCast(oSAMBusiness.AddEventNote(oRequest), AddEventNoteResponseType)

        Return oResponse

    End Function

    'End(Sriram P)-(Tech Specs - WR42A_SAM_Party_Enquiry - AddEventNote.doc)-(7.1.4.7)

#End Region
#Region "GetEventNote"
    'Start (Girija chokkalingam) - (Tech Spec - UIIC WR42A_SAM_Party_Enquiry_GetEventNote.doc) - (7.1.4.7)
    ''' <summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="GetEventNoteRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetEventNoteResponseType</returns>  
    '''<remarks></remarks>
    Public Function GetEventNote(ByVal oRequest As GetEventNoteRequestType) As GetEventNoteResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetEventNoteResponseType

        oResponse = DirectCast(oSAMBusiness.GetEventNote(oRequest), GetEventNoteResponseType)

        Return oResponse

    End Function

    'End (Girija chokkalingam) - (Tech Spec - UIIC WR42A_SAM_Party_Enquiry_GetEventNote.doc) - (7.1.4.7)
#End Region
#Region "GetRatingSectionTypes"
    'Start(Sriram P)-(Tech Spec - UIIC WR22 – Capture Quote Details – Get Rating Section Types.doc)-(7.1.3.9)
    ''' <summary>
    '''This function will pass the request object along GetRatingSectionTypes method in coreSAMBusiness
    '''</summary>
    '''<param name="oRequest" type="GetRatingSectionTypesRequestType"></param>  
    '''<returns> object of type GetRatingSectionTypesResponseType </returns>
    '''<remarks></remarks> 
    Public Function GetRatingSectionTypes(ByVal oRequest As GetRatingSectionTypesRequestType) As GetRatingSectionTypesResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetRatingSectionTypesResponseType

        oResponse = DirectCast(oSAMBusiness.GetRatingSectionTypes(oRequest), GetRatingSectionTypesResponseType)

        Return oResponse

    End Function
    'End(Sriram P)-(Tech Spec - UIIC WR22 – Capture Quote Details – Get Rating Section Types.doc)-(7.1.3.9)
#End Region
#Region " AddUserGroup "
    'Start (Prakash C Varghese) - (Tech Spec - UIIC WR01 - User Access - Add User Group.doc) - (7.1.5.7)
    ''' <summary> 
    ''' Adds a new user group and retrieves the newly added user group id. 
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.Structure.SAMForInsuranceV2ImplementationTypes.AddUserGroupRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.Structure.SAMForInsuranceV2ImplementationTypes.AddUserGroupResponseType</returns>  
    Public Function AddUserGroup(ByVal oRequest As AddUserGroupRequestType) As AddUserGroupResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New AddUserGroupResponseType

        oResponse = DirectCast(oSAMBusiness.AddUserGroup(oRequest), AddUserGroupResponseType)

        Return oResponse

    End Function
    'End (Prakash C Varghese) - (Tech Spec - UIIC WR01 - User Access - Add User Group.doc) - (7.1.5.7)
#End Region
#Region "Public Method-UpdateUserGroup"
    'Start(Sriram P)-(Tech Spec - UIIC WR01 - User Access - Update User Group.doc)-(7.1.5.7)
    ''' <summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="UpdateUserGroupRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.UpdateUserGroupResponseType</returns>  
    '''<remarks></remarks>
    Public Function UpdateUserGroup(ByVal oRequest As UpdateUserGroupRequestType) As UpdateUserGroupResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New UpdateUserGroupResponseType

        oResponse = DirectCast(oSAMBusiness.UpdateUserGroup(oRequest), UpdateUserGroupResponseType)

        Return oResponse

    End Function
    'End(Sriram P)-(Tech Spec - UIIC WR01 - User Access - Update User Group.doc)-(7.1.5.7)

#End Region
#Region "UpdateUserGroupUsers"
    'Start (Girija chokkalingam) - (Tech Spec - UIIC WR01 – User Access – Update User Group Users.doc) - (7.1.5.7)
    ''' <summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness- UserAccess  layer
    '''</summary>
    '''<param name="oRequest" type="UpdateUserGroupUsersRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.UpdateUserGroupUsersResponseType</returns>  
    '''<remarks></remarks>
    Public Function UpdateUserGroupUsers(ByVal oRequest As UpdateUserGroupUsersRequestType) As UpdateUserGroupUsersResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New UpdateUserGroupUsersResponseType

        oResponse = DirectCast(oSAMBusiness.UpdateUserGroupUsers(oRequest), UpdateUserGroupUsersResponseType)

        Return oResponse

    End Function

    'End (Girija chokkalingam) - (Tech Spec - UIIC WR01 – User Access – Update User Group Users.doc) - (7.1.5.7)
#End Region
#Region "UpdateTaskGroupTasks"
    'Start (Girija chokkalingam) - (Tech Spec - UIIC WR01 - User Access - Update Task Group Tasks.doc) - (7.1.5.7)
    ''' <summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness- UserAccess  layer
    '''</summary>
    '''<param name="oRequest" type="UpdateTaskGroupTasksRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.UpdateTaskGroupTasksResponseType</returns>  
    '''<remarks></remarks>
    Public Function UpdateTaskGroupTasks(ByVal oRequest As UpdateTaskGroupTasksRequestType) As UpdateTaskGroupTasksResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New UpdateTaskGroupTasksResponseType

        oResponse = DirectCast(oSAMBusiness.UpdateTaskGroupTasks(oRequest), UpdateTaskGroupTasksResponseType)

        Return oResponse

    End Function
    'End (Girija chokkalingam) - (Tech Spec - UIIC WR01 - User Access - Update Task Group Tasks.doc) - (7.1.5.7)
#End Region
#Region "GetUserGroups"

    '(Start (Ravikumar Pasupuleti)-(Tech Spec - UIIC WR01 - User Access - Get User Groups)-(7.1.5.7)
    '''<summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="GetUserGroupsRequestType"></param>
    '''<remarks></remarks>
    Public Function GetUserGroups(ByVal oRequest As GetUserGroupsRequestType) As GetUserGroupsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetUserGroupsResponseType
        oResponse = DirectCast(oSAMBusiness.GetUserGroups(oRequest), GetUserGroupsResponseType)
        Return oResponse
    End Function

    'End (Ravikumar Pasupuleti)-(Tech Spec - UIIC WR33 - Work Manager - Get Tasks.doc)-(7.1.5.7)
#End Region
#Region "Public Method-GetTaskGroups"
    'Start(Sriram P)-(Tech Spec - UIIC WR01 - User Access - Update User Group.doc)-(7.1.5.7)
    ''' <summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="GetTaskGroupsRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetTaskGroupsResponseType</returns>  
    '''<remarks></remarks>
    Public Function GetTaskGroups(ByVal oRequest As GetTaskGroupsRequestType) As GetTaskGroupsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetTaskGroupsResponseType

        oResponse = DirectCast(oSAMBusiness.GetTaskGroups(oRequest), GetTaskGroupsResponseType)

        Return oResponse

    End Function

    'End(Sriram P)-(Tech Spec - UIIC WR01 - User Access - Update User Group.doc)-(7.1.5.7)

#End Region
#Region "Public Method-GetTaskGroupTasks"
    'Start(Sriram P)-(Tech Spec - UIIC WR01 - User Access - Get Task Group Tasks.doc)-(7.1.5.7)
    ''' <summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="GetTaskGroupTasksRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetTaskGroupTasksResponseType</returns>  
    '''<remarks></remarks>
    Public Function GetTaskGroupTasks(ByVal oRequest As GetTaskGroupTasksRequestType) As GetTaskGroupTasksResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetTaskGroupTasksResponseType

        oResponse = DirectCast(oSAMBusiness.GetTaskGroupTasks(oRequest), GetTaskGroupTasksResponseType)

        Return oResponse

    End Function


    'End(Sriram P)-(Tech Spec - UIIC WR01 - User Access - Get Task Group Tasks.doc)-(7.1.5.7)

#End Region
#Region "GetWorkManagerScheduledTasks"

    'Start(Ravikumar Pasupuleti)-(Tech Spec - UIIC WR33 - Work Manager - Get Tasks.doc)-(7.1.4.8)
    '''<summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="GetWorkManagerScheduledTasks"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetWorkManagerScheduledTasksResponseType</returns>  
    '''<remarks></remarks>

    Public Function GetWorkManagerScheduledTasks(ByVal oRequest As GetWorkManagerScheduledTasksRequestType) As GetWorkManagerScheduledTasksResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetWorkManagerScheduledTasksResponseType
        oResponse = DirectCast(oSAMBusiness.GetWorkManagerScheduledTasks(oRequest), GetWorkManagerScheduledTasksResponseType)

        Return oResponse
    End Function

    'End (Ravikumar Pasupuleti)-(Tech Spec - UIIC WR33 - Work Manager - Get Tasks.doc)-(7.1.4.8)
#End Region
#Region "Public Method-GetUserGroupTaskGroups"
    'Start(Sriram P)-(Tech Spec - UIIC WR28 - User Access - Get User Group Task Groups.doc)-(7.1.5.8)
    ''' <summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="GetUserGroupTaskGroupsRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetUserGroupTaskGroupsResponseType</returns>  
    '''<remarks></remarks>
    Public Function GetUserGroupTaskGroups(ByVal oRequest As GetUserGroupTaskGroupsRequestType) As GetUserGroupTaskGroupsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetUserGroupTaskGroupsResponseType

        oResponse = DirectCast(oSAMBusiness.GetUserGroupTaskGroups(oRequest), GetUserGroupTaskGroupsResponseType)

        Return oResponse

    End Function
    'End(Sriram P)-(Tech Spec - UIIC WR28 - User Access - Get User Group Task Groups.doc)-(7.1.5.8)

#End Region
#Region "Public Method-AddCoverNoteBook"
    'Start(Sriram P)-(Tech Spec - UIIC WR53 - Cover Note Maintenance - Add Cover Note Book.doc)-(7.1.4.7)
    ''' <summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="AddCoverNoteBookRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.AddCoverNoteBookResponseType</returns>  
    '''<remarks></remarks>
    Public Function AddCoverNoteBook(ByVal oRequest As AddCoverNoteBookRequestType) As AddCoverNoteBookResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New AddCoverNoteBookResponseType

        oResponse = DirectCast(oSAMBusiness.AddCoverNoteBook(oRequest), AddCoverNoteBookResponseType)

        Return oResponse

    End Function


    'End(Sriram P)-(Tech Spec - UIIC WR53 - Cover Note Maintenance - Add Cover Note Book.doc)-(7.1.4.7)

#End Region
#Region "Public Method-GetReferredPayments"
    'Start(Sriram P)-(Tech Spec - UIIC WR60 - Authorise Payment - Get Referred Payments.doc)-(7.1.5.7)
    ''' <summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="GetReferredPaymentsRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetReferredPaymentsResponseType</returns>  
    '''<remarks></remarks>
    Public Function GetReferredPayments(ByVal oRequest As GetReferredPaymentsRequestType) As GetReferredPaymentsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetReferredPaymentsResponseType

        oResponse = DirectCast(oSAMBusiness.GetReferredPayments(oRequest), GetReferredPaymentsResponseType)

        Return oResponse

    End Function

    'End(Sriram P)-(Tech Spec - UIIC WR60 - Authorise Payment - Get Referred Payments.doc)-(7.1.5.7)

#End Region
#Region "DeleteCoverNoteSheet"
    'Start (Girija chokkalingam) - (WR53 – Cover Note Maintenance – Delete Cover Note Sheet .doc) - (7.1.4.7)    
    '''<summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="DeleteCoverNoteSheetRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.BaseImplementationTypes.BaseDeleteCoverNoteSheetResponseType</returns>
    '''<remarks></remarks>
    Public Function DeleteCoverNoteSheet(ByVal oRequest As DeleteCoverNoteSheetRequestType) As DeleteCoverNoteSheetResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New DeleteCoverNoteSheetResponseType

        oResponse = DirectCast(oSAMBusiness.DeleteCoverNoteSheet(oRequest), DeleteCoverNoteSheetResponseType)

        Return oResponse

    End Function
#End Region
#Region " UpdateCoverNoteBook "
    ''' <summary>  
    ''' This method is used to update Cover Note Book
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.UpdateCoverNoteBookRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.UpdateCoverNoteBookResponseType</returns>  
    Public Function UpdateCoverNoteBook(ByVal oRequest As UpdateCoverNoteBookRequestType) As UpdateCoverNoteBookResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New UpdateCoverNoteBookResponseType

        oResponse = DirectCast(oSAMBusiness.UpdateCoverNoteBook(oRequest), UpdateCoverNoteBookResponseType)

        Return oResponse

    End Function
#End Region
#Region " UpdateCoverNoteSheet "
    ''' <summary>  
    ''' This method is used to update Cover Note Sheet
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.UpdateCoverNoteSheetRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.UpdateCoverNoteSheetResponseType</returns>  
    Public Function UpdateCoverNoteSheet(ByVal oRequest As UpdateCoverNoteSheetRequestType) As UpdateCoverNoteSheetResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New UpdateCoverNoteSheetResponseType

        oResponse = DirectCast(oSAMBusiness.UpdateCoverNoteSheet(oRequest), UpdateCoverNoteSheetResponseType)

        Return oResponse
    End Function
#End Region
#Region "AddCoverNoteSheet"
    ''' <summary>
    '''This method passes the AddCoverNoteSheetRequestType request from web service layer to core implementation layer.
    '''It returns back the result from core implementation layer to web service layer.
    '''</summary>
    '''<param name="oRequest" type="SiriusFS.SAM.SFI.SAMForInsuranceV2.AddCoverNoteSheetRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.AddCoverNoteSheetResponseType</returns>
    '''<remarks></remarks>
    Public Function AddCoverNoteSheet(ByVal oRequest As AddCoverNoteSheetRequestType) As AddCoverNoteSheetResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New AddCoverNoteSheetResponseType

        oResponse = DirectCast(oSAMBusiness.AddCoverNoteSheet(oRequest), AddCoverNoteSheetResponseType)

        Return oResponse
    End Function
#End Region
#Region " GetUnallocatedClaimPayments "
    ''' <summary>  
    ''' This method is used to get unallocated claim payments
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.GetUnallocatedClaimPaymentsRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.GetUnallocatedClaimPaymentsResponseType</returns>  
    Public Function GetUnallocatedClaimPayments(ByVal oRequest As GetUnallocatedClaimPaymentsRequestType) As GetUnallocatedClaimPaymentsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetUnallocatedClaimPaymentsResponseType

        oResponse = DirectCast(oSAMBusiness.GetUnallocatedClaimPayments(oRequest), GetUnallocatedClaimPaymentsResponseType)

        Return oResponse

    End Function
#End Region
#Region "UpdateBankGuaranteeConditionally"
    '''<summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="UpdateBankGuaranteeConditionallyRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.UpdateBankGuaranteeConditionallyResponseType</returns>  
    '''<remarks></remarks>
    Public Function UpdateBankGuaranteeConditionally(ByVal oRequest As UpdateBankGuaranteeConditionallyRequestType) As UpdateBankGuaranteeConditionallyResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New UpdateBankGuaranteeConditionallyResponseType

        oResponse = DirectCast(oSAMBusiness.UpdateBankGuaranteeConditionally(oRequest), UpdateBankGuaranteeConditionallyResponseType)

        Return oResponse
    End Function
#End Region

#Region "Public Method-GetPaymentCashListItems"
    ''' <summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="GetPaymentCashListItemsRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetPaymentCashListItemsResponseType</returns>  
    '''<remarks></remarks>
    Public Function GetPaymentCashListItems(ByVal oRequest As GetPaymentCashListItemsRequestType) As GetPaymentCashListItemsResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetPaymentCashListItemsResponseType

        oResponse = DirectCast(oSAMBusiness.GetPaymentCashListItems(oRequest), GetPaymentCashListItemsResponseType)

        Return oResponse
    End Function
#End Region
#Region "Public Method-GetReceiptCashListDetails"
    ''' <summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="GetReceiptCashListDetailsRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetReceiptCashListDetailsResponseType</returns>  
    '''<remarks></remarks>
    Public Function GetReceiptCashListDetails(ByVal oRequest As GetReceiptCashListDetailsRequestType) As GetReceiptCashListDetailsResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetReceiptCashListDetailsResponseType

        oResponse = DirectCast(oSAMBusiness.GetReceiptCashListDetails(oRequest), GetReceiptCashListDetailsResponseType)

        Return oResponse
    End Function
#End Region

#Region "GetCoverNote"
    '''<summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="GetCoverNoteSheetRequestType"></param>
    ''<remarks></remarks>
    Public Function GetCoverNoteSheet(ByVal oRequest As GetCoverNoteSheetRequestType) As GetCoverNoteSheetResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetCoverNoteSheetResponseType

        oResponse = DirectCast(oSAMBusiness.GetCoverNoteSheet(oRequest), GetCoverNoteSheetResponseType)

        Return oResponse
    End Function
#End Region
#Region " FindAccounts "
    ''' <summary>  
    ''' This method is used to find accounts in claim payment
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.FindAccountsRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.FindAccountsResponseType</returns>  
    Public Function FindAccounts(ByVal oRequest As FindAccountsRequestType) As FindAccountsResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New FindAccountsResponseType

        oResponse = DirectCast(oSAMBusiness.FindAccounts(oRequest), FindAccountsResponseType)

        Return oResponse
    End Function
#End Region
#Region " GetPaymentCashListDetails "
    ''' <summary>  
    ''' This method is used to get payment cash list details
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.GetPaymentCashListDetailsRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.GetPaymentCashListDetailsResponseType</returns>  
    Public Function GetPaymentCashListDetails(ByVal oRequest As GetPaymentCashListDetailsRequestType) As GetPaymentCashListDetailsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetPaymentCashListDetailsResponseType

        oResponse = DirectCast(oSAMBusiness.GetPaymentCashListDetails(oRequest), GetPaymentCashListDetailsResponseType)

        Return oResponse
    End Function
#End Region
#Region " GetPaymentCashListItemDetails "
    ''' <summary>  
    ''' This method is used to get payment cash list item details
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.GetPaymentCashListItemDetailsRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.GetPaymentCashListItemDetailsResponseType</returns>  
    Public Function GetPaymentCashListItemDetails(ByVal oRequest As GetPaymentCashListItemDetailsRequestType) As GetPaymentCashListItemDetailsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetPaymentCashListItemDetailsResponseType

        oResponse = DirectCast(oSAMBusiness.GetPaymentCashListItemDetails(oRequest), GetPaymentCashListItemDetailsResponseType)

        Return oResponse
    End Function
#End Region
#Region "GetReceiptCashListItems "
    ''' <summary> 
    ''' To get receipt cash list items
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.Structure.SAMForInsuranceV2ImplementationTypes.GetReceiptCashListItemsRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.Structure.SAMForInsuranceV2ImplementationTypes.GetReceiptCashListItemsResponseType</returns>  
    Public Function GetReceiptCashListItems(ByVal oRequest As GetReceiptCashListItemsRequestType) As GetReceiptCashListItemsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetReceiptCashListItemsResponseType

        oResponse = DirectCast(oSAMBusiness.GetReceiptCashListItems(oRequest), GetReceiptCashListItemsResponseType)

        Return oResponse
    End Function
#End Region

#Region "ApproveCashListItem"
    '''<summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="ApproveCashListItemRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.ApproveCashListItemResponseType</returns>
    '''<remarks></remarks>
    Public Function ApproveCashListItem(ByVal oRequest As ApproveCashListItemRequestType) As ApproveCashListItemResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New ApproveCashListItemResponseType

        oResponse = DirectCast(oSAMBusiness.ApproveCashListItem(oRequest), ApproveCashListItemResponseType)

        Return oResponse
    End Function
#End Region
#Region " PolicyRenewalSection "

#Region " GetPoliciesInRenewal "
    Public Function GetPoliciesInRenewal(ByVal oRequest As GetPoliciesInRenewalRequestType) As GetPoliciesInRenewalResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        'Const ACMethodName As String = "GetPoliciesInRenewal"
        Dim oResponse As New GetPoliciesInRenewalResponseType
        oResponse = DirectCast(oSAMBusiness.GetPoliciesInRenewal(oRequest), GetPoliciesInRenewalResponseType)
        Return oResponse

    End Function
#End Region

#Region " GetRenewalStatus "
    Public Function GetRenewalStatus(ByVal request As GetRenewalStatusRequestType) As GetRenewalStatusResponseType
        Dim samBusiness As New CoreSAMBusiness(request.BranchCode)
        Dim response As New GetRenewalStatusResponseType
        response = DirectCast(samBusiness.GetRenewalStatus(request), GetRenewalStatusResponseType)
        Return response
    End Function
#End Region

#Region " ReplacePartyContact "
    Public Function ReplacePartyContact(ByVal request As ReplacePartyContactRequestType) As ReplacePartyContactResponseType
        Dim samBusiness As New CoreSAMBusiness(request.BranchCode)
        Dim response As New ReplacePartyContactResponseType
        response = DirectCast(samBusiness.ReplacePartyContact(request), ReplacePartyContactResponseType)
        Return response
    End Function
#End Region

#Region " GetExistingInstalmentPlanPaymentDetails "
    Public Function GetExistingInstalmentPlanPaymentDetails(ByVal request As GetExistingInstalmentPlanPaymentDetailsRequestType) As GetExistingInstalmentPlanPaymentDetailsResponseType
        Dim samBusiness As New CoreSAMBusiness(request.BranchCode)
        Dim response As New GetExistingInstalmentPlanPaymentDetailsResponseType
        response = DirectCast(samBusiness.GetExistingInstalmentPlanPaymentDetails(request), GetExistingInstalmentPlanPaymentDetailsResponseType)
        Return response
    End Function
#End Region

#Region " GenerateInvite "
    Public Function GenerateInvite(ByVal request As GenerateInviteRequestType) As GenerateInviteResponseType
        Dim samBusiness As New CoreSAMBusiness(request.BranchCode)
        Dim response As New GenerateInviteResponseType
        response = DirectCast(samBusiness.GenerateInvite(request), GenerateInviteResponseType)
        Return response
    End Function
#End Region

#Region " LapseRenewal "
    Public Function LapseRenewal(ByVal request As LapseRenewalRequestType) As LapseRenewalResponseType
        Dim samBusiness As New CoreSAMBusiness(request.BranchCode)
        Dim response As New LapseRenewalResponseType
        response = DirectCast(samBusiness.LapseRenewal(request), LapseRenewalResponseType)
        Return response
    End Function
#End Region

#Region " RunRenewalSelection "
    Public Sub RunRenewalSelection(ByVal oRequest As RunRenewalSelectionRequestType)

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)

        oSAMBusiness.RunRenewalSelection(oRequest)

    End Sub
#End Region

#Region " RunRenewalInvitation "
    Public Sub RunRenewalInvitation(ByVal oRequest As RunRenewalInviteRequestType)

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)

        oSAMBusiness.RunRenewalInvitation(oRequest)

    End Sub
#End Region

#Region " RunRenewalAccept "
    Public Sub RunRenewalAccept(ByVal oRequest As RunRenewalAcceptRequestType)

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)

        oSAMBusiness.RunRenewalAccept(oRequest)

    End Sub
#End Region

#Region " RunRenewalSelectionByPolicy "
    '''<summary>
    ''' This method is the adapter method between RunRenewalSelectionByPolicy methods in Web Service layer and Core implementation layer
    '''</summary>
    '''<param name="oRunRenewalSelectionByPolicyRequest" type="SAMForInsuranceV2ImplementationTypes.RunRenewalSelectionByPolicyRequestType"></param>   
    '''<returns>SAMForInsuranceV2ImplementationTypes.RunRenewalSelectionByPolicyResponseType</returns>
    '''<remarks></remarks>
    Public Function RunRenewalSelectionByPolicy(ByVal oRunRenewalSelectionByPolicyRequest As RunRenewalSelectionByPolicyRequestType) As RunRenewalSelectionByPolicyResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRunRenewalSelectionByPolicyRequest.BranchCode)
        Dim oResponse As New RunRenewalSelectionByPolicyResponseType

        oResponse = DirectCast(oSAMBusiness.RunRenewalSelectionByPolicy(oRunRenewalSelectionByPolicyRequest), RunRenewalSelectionByPolicyResponseType)

        Return oResponse
    End Function
#End Region

#Region " DeleteRenewal "
    '''<summary>
    ''' This method is the adapter method between DeleteRenewal methods in Web Service layer and Core implementation layer
    '''</summary>
    '''<param name="oDeleteRenewalRequest" type="SAMForInsuranceV2ImplementationTypes.DeleteRenewalRequestType"></param>   
    '''<returns>SAMForInsuranceV2ImplementationTypes.DeleteRenewalResponseType</returns>
    '''<remarks></remarks>
    Public Function DeleteRenewal(ByVal oDeleteRenewalRequest As DeleteRenewalRequestType) As DeleteRenewalResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oDeleteRenewalRequest.BranchCode)
        Dim oResponse As New DeleteRenewalResponseType

        oResponse = DirectCast(oSAMBusiness.DeleteRenewal(oDeleteRenewalRequest), DeleteRenewalResponseType)

        Return oResponse
    End Function
#End Region

#Region " UpdateRenewalStatus "
    '''<summary>
    ''' This method is the adapter method between UpdateRenewal methods in Web Service layer and Core implementation layer
    '''</summary>
    '''<param name="oUpdateRenewalStatusRequest" type="SAMForInsuranceV2ImplementationTypes.UpdateRenewalStatusRequestType"></param>   
    '''<returns>SAMForInsuranceV2ImplementationTypes.UpdateRenewalStatusResponseType</returns>
    '''<remarks></remarks>
    Public Function UpdateRenewalStatus(ByVal oUpdateRenewalStatusRequest As UpdateRenewalStatusRequestType) As UpdateRenewalStatusResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oUpdateRenewalStatusRequest.BranchCode)
        Dim oResponse As New UpdateRenewalStatusResponseType

        oResponse = DirectCast(oSAMBusiness.UpdateRenewalStatus(oUpdateRenewalStatusRequest), UpdateRenewalStatusResponseType)

        Return oResponse
    End Function
#End Region
#Region " GetPoliciesForRenewalSelection "
    '''<summary>
    ''' This method is the adapter method between GetPoliciesForRenewalSelection methods in Web Service layer and Core implementation layer
    '''</summary>
    '''<param name="oRequest" type="SAMForInsuranceV2ImplementationTypes.GetPoliciesForRenewalSelectionRequestType"></param>   
    '''<returns>SAMForInsuranceV2ImplementationTypes.GetPoliciesForRenewalSelectionResponseType</returns>
    '''<remarks></remarks>
    Public Function GetPoliciesForRenewalSelection(ByVal oRequest As GetPoliciesForRenewalSelectionRequestType) As GetPoliciesForRenewalSelectionResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetPoliciesForRenewalSelectionResponseType

        oResponse = DirectCast(oSAMBusiness.GetPoliciesForRenewalSelection(oRequest), GetPoliciesForRenewalSelectionResponseType)

        Return oResponse
    End Function
#End Region
#End Region

#Region "Public Method-UpdateRiskSelection"
    ''' <summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="UpdateRiskSelectionRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2ImplementationTypes.UpdateRiskSelectionResponseType</returns>  
    '''<remarks></remarks>
    Public Function UpdateRiskSelection(ByVal oRequest As UpdateRiskSelectionRequestType) As UpdateRiskSelectionResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New UpdateRiskSelectionResponseType

        oResponse = DirectCast(oSAMBusiness.UpdateRiskSelection(oRequest), UpdateRiskSelectionResponseType)

        Return oResponse

    End Function
#End Region
#Region "FindPolicy"
    ''' <summary>  
    ''' Find the Policy
    ''' </summary>  
    ''' <param name="oRequest">An object of type FindPolicyRequestType</param>  
    Public Function FindPolicy(ByVal oRequest As FindPolicyRequestType) As FindPolicyResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New FindPolicyResponseType

        oResponse = DirectCast(oSAMBusiness.FindPolicy(oRequest), FindPolicyResponseType)

        Return oResponse

    End Function
#End Region
#Region "CreatePaymentCashListWithItems"
    'Start (Saurabh Agrawal) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Create Payment Cash List) - (7.1.4.6)  
    ''' <summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="CreatePaymentCashListWithItemsRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2ImplementationTypes.CreatePaymentCashListWithItemsResponseType</returns>  
    '''<remarks></remarks>
    Public Function CreatePaymentCashListWithItems(ByVal oRequest As CreatePaymentCashListWithItemsRequestType) As CreatePaymentCashListWithItemsResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New CreatePaymentCashListWithItemsResponseType

        oResponse = DirectCast(oSAMBusiness.CreatePaymentCashListWithItems(oRequest), CreatePaymentCashListWithItemsResponseType)

        Return oResponse
    End Function
#End Region
#Region "CreatePaymentCashListItem"
    ''' <summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="CreatePaymentCashListItemRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2ImplementationTypes.CreatePaymentCashListItemResponseType</returns>  
    '''<remarks></remarks>

    Public Function CreatePaymentCashListItem(ByVal oRequest As CreatePaymentCashListItemRequestType) As CreatePaymentCashListItemResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New CreatePaymentCashListItemResponseType

        oResponse = DirectCast(oSAMBusiness.CreatePaymentCashListItem(oRequest), CreatePaymentCashListItemResponseType)

        Return oResponse
    End Function
#End Region

#Region "GetTransactionDetails"
    ''' <summary>
    '''This method passes the GetTransactionDetailsRequestType request from web service layer to core implementation layer.
    '''It returns back the result from core implementation layer to web service layer.
    '''</summary>
    '''<param name="oRequest" type="SiriusFS.SAM.SFI.SAMForInsuranceV2.GetTransactionDetailsRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetTransactionDetailsResponseType</returns>
    '''<remarks></remarks>

    Public Function GetTransactionDetails(ByVal oRequest As GetTransactionDetailsRequestType) As GetTransactionDetailsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetTransactionDetailsResponseType

        oResponse = DirectCast(oSAMBusiness.GetTransactionDetails(oRequest), GetTransactionDetailsResponseType)

        Return oResponse
    End Function
#End Region
#Region "GetCurrencyExchangeRates"
    ''' <summary>
    '''This method passes the GetCurrencyExchangeRatesRequestType request from web service layer to core implementation layer.
    '''It returns back the result from core implementation layer to web service layer.
    '''</summary>
    '''<param name="oRequest" type="SiriusFS.SAM.SFI.SAMForInsuranceV2.GetCurrencyExchangeRatesRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetCurrencyExchangeRatesResponseType</returns>
    '''<remarks></remarks>
    Public Function GetCurrencyExchangeRates(ByVal oRequest As GetCurrencyExchangeRatesRequestType) As GetCurrencyExchangeRatesResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetCurrencyExchangeRatesResponseType

        oResponse = DirectCast(oSAMBusiness.GetCurrencyExchangeRates(oRequest), GetCurrencyExchangeRatesResponseType)

        Return oResponse
    End Function
#End Region

#Region "GetCurrencyToCurrencyExchangeRate"
    ''' <summary>
    '''This method passes the GetCurrencyToCurrencyExchangeRateRequestType request from web service layer to core implementation layer.
    '''It returns back the result from core implementation layer to web service layer.
    '''</summary>
    '''<param name="oRequest" type="SiriusFS.SAM.SFI.SAMForInsuranceV2.GetCurrencyToCurrencyExchangeRateRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetCurrencyToCurrencyExchangeRateResponseType</returns>
    '''<remarks></remarks>
    Public Function GetCurrencyToCurrencyExchangeRate(ByVal oRequest As GetCurrencyToCurrencyExchangeRateRequestType) As GetCurrencyToCurrencyExchangeRateResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetCurrencyToCurrencyExchangeRateResponseType

        oResponse = DirectCast(oSAMBusiness.GetCurrencyToCurrencyExchangesRate(oRequest), GetCurrencyToCurrencyExchangeRateResponseType)

        Return oResponse
    End Function
#End Region
#Region "GetReceiptCashListItem"
    ''' <summary>
    ''' Get a cashlist item for CashlistReceipt
    ''' </summary>
    '''<param name="oRequest" type ="GetReceiptCashListItemDetailsRequestType"></param>
    '''<returns>"oResponse" type ="GetReceiptCashListItemDetailsResponseType"</returns>
    '''</summary>
    Public Function GetReceiptCashListItemDetails(ByVal oRequest As GetReceiptCashListItemDetailsRequestType) As GetReceiptCashListItemDetailsResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetReceiptCashListItemDetailsResponseType

        oResponse = DirectCast(oSAMBusiness.GetReceiptCashListItemDetails(oRequest), GetReceiptCashListItemDetailsResponseType)

        Return oResponse
    End Function
#End Region

#Region " GetValidPrimaryCauses "
    '''<summary>
    ''' This method is the adapter method between GetValidPrimaryCauses methods in Web Service layer and Core implementation layer
    '''</summary>
    '''<param name="oRequest" type="SAMForInsuranceV2ImplementationTypes.GetValidPrimaryCausesRequestType"></param>   
    '''<returns>SAMForInsuranceV2ImplementationTypes.GetValidPrimaryCausesResponseType</returns>
    '''<remarks></remarks>
    Public Function GetValidPrimaryCauses(ByVal oRequest As GetValidPrimaryCausesRequestType) As GetValidPrimaryCausesResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetValidPrimaryCausesResponseType

        oResponse = DirectCast(oSAMBusiness.GetValidPrimaryCauses(oRequest), GetValidPrimaryCausesResponseType)

        Return oResponse
    End Function
#End Region

#Region "CopyRisk"
    ''' <summary>
    ''' Copy Risk for Quote
    ''' </summary>
    '''<param name="oRequest" type ="CopyRiskRequestType"></param>
    '''<returns>"oResponse" type ="CopyRiskResponseType"</returns>
    '''</summary>
    Public Function CopyRisk(ByVal oRequest As CopyRiskRequestType) As CopyRiskResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New CopyRiskResponseType

        oResponse = DirectCast(oSAMBusiness.CopyRisk(oRequest), CopyRiskResponseType)

        Return oResponse
    End Function
#End Region

#Region "FindUsers"
    ''' <summary>
    '''This method passes the FindUsersRequestType request from web service layer to core implementation layer.
    '''It returns back the result from core implementation layer to web service layer.
    '''</summary>
    '''<param name="oRequest" type="SiriusFS.SAM.SFI.SAMForInsuranceV2.FindUsersRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.FindUsersResponseType</returns>
    '''<remarks></remarks>
    Public Function FindUsers(ByVal oRequest As FindUsersRequestType) As FindUsersResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New FindUsersResponseType

        oResponse = DirectCast(oSAMBusiness.FindUsers(oRequest), FindUsersResponseType)

        Return oResponse
    End Function
#End Region

#Region "GetInsurerPayments"
    ''' <summary>
    '''This method passes the GetInsurerPaymentsRequestType request from web service layer to core implementation layer.
    '''It returns back the result from core implementation layer to web service layer.
    '''</summary>
    '''<param name="oRequest" type="SiriusFS.SAM.SFI.SAMForInsuranceV2.GetInsurerPaymentsRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetInsurerPaymentsResponseType</returns>
    '''<remarks></remarks>
    Public Function GetInsurerPayments(ByVal oRequest As GetInsurerPaymentsRequestType) As GetInsurerPaymentsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetInsurerPaymentsResponseType

        oResponse = DirectCast(oSAMBusiness.GetInsurerPayments(oRequest), GetInsurerPaymentsResponseType)

        Return oResponse
    End Function
#End Region

#Region "AddWriteOff"
    ''' <summary>
    '''This method passes the AddWriteOffRequestType request from web service layer to core implementation layer.
    '''It returns back the result from core implementation layer to web service layer.
    '''</summary>
    '''<param name="oRequest" type="SiriusFS.SAM.SFI.SAMForInsuranceV2.AddWriteOffRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.AddWriteOffResponseType</returns>
    '''<remarks></remarks>
    Public Function AddWriteOff(ByVal oRequest As AddWriteOffRequestType) As AddWriteOffResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New AddWriteOffResponseType

        oResponse = DirectCast(oSAMBusiness.AddWriteOff(oRequest), AddWriteOffResponseType)

        Return oResponse
    End Function
#End Region

#Region "Public method GetProductRiskOptionValue"
    ''' <summary>
    '''This method passes the GetProductRiskOptionValue request from web service layer to core implementation layer.
    '''It returns back the result from core implementation layer to web service layer.
    '''</summary>
    '''<param name="oRequest" type="SiriusFS.SAM.SFI.SAMForInsuranceV2.ProductRiskOptionValueRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.ProductRiskOptionValueResponseType</returns>
    '''<remarks></remarks>
    Public Function GetProductRiskOptionValue(ByVal oRequest As ProductRiskOptionValueRequestType) As ProductRiskOptionValueResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New ProductRiskOptionValueResponseType

        oResponse = DirectCast(oSAMBusiness.GetProductRiskOptionValue(oRequest), ProductRiskOptionValueResponseType)

        Return oResponse
    End Function
#End Region



#Region "Get User Authority Value"
    ''' <summary>
    ''' This is webservice method for  GetUserAuthorityValue
    '''<param name="oRequest" type="GetUserAuthorityValueRequestType"></param>   
    ''' <returns>GetUserAuthorityValueResponseType</returns>  
    '''</summary>
    '''<remarks></remarks>  
    Public Function GetUserAuthorityValue(ByVal oRequest As GetUserAuthorityValueRequestType) As GetUserAuthorityValueResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetUserAuthorityValueResponseType
        oResponse = DirectCast(oSAMBusiness.GetUserAuthorityValue(oRequest), GetUserAuthorityValueResponseType)
        Return oResponse
    End Function
#End Region

#Region "AuthoriseClaimPayment"
    '''<summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="AuthoriseClaimPaymentRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.AuthoriseClaimPaymentResponseType</returns>  
    '''<remarks></remarks>
    Public Function AuthoriseClaimPayment(ByVal oRequest As AuthoriseClaimPaymentRequestType) As AuthoriseClaimPaymentResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New AuthoriseClaimPaymentResponseType

        oResponse = DirectCast(oSAMBusiness.AuthoriseClaimPayment(oRequest), AuthoriseClaimPaymentResponseType)

        Return oResponse
    End Function
#End Region

#Region "CalculateTaxForClaims"
    ''' <summary>
    '''This method passes the CalculateTaxForClaims request from web service layer to core implementation layer.
    '''It returns back the result from core implementation layer to web service layer.
    '''</summary>
    '''<param name="oRequest" type="SiriusFS.SAM.SFI.SAMForInsuranceV2.CalculateTaxForClaimsRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.CalculateTaxForClaimsResponseType</returns>
    '''<remarks></remarks>
    Public Function CalculateTaxForClaims(ByVal oRequest As CalculateTaxForClaimsRequestType) As CalculateTaxForClaimsResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New CalculateTaxForClaimsResponseType

        oResponse = DirectCast(oSAMBusiness.CalculateTaxforClaims(oRequest), CalculateTaxForClaimsResponseType)

        Return oResponse
    End Function
#End Region

#Region "Public method AttachCoverNote"
    ''' <summary>
    '''This method passes the AttachCoverNote request from web service layer to core implementation layer.
    '''It returns back the result from core implementation layer to web service layer.
    '''</summary>
    '''<param name="oRequest" type="SiriusFS.SAM.SFI.SAMForInsurance.AttachCoverNoteRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsurance.AttachCoverNoteResponseType</returns>
    '''<remarks></remarks>

    Public Function AttachCoverNote(ByVal oRequest As AttachCoverNoteRequestType) As AttachCoverNoteResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oSTSError As New STSErrorPublisher
        Dim oResponse As New AttachCoverNoteResponseType
        Const ACMethodName As String = "AttachCoverNote"


        If oRequest.ProcessType <> CoverNoteProcessType.Attach And oRequest.ProcessType <> CoverNoteProcessType.Dettach Then
            oSTSError.AddInvalidField("ProcessType", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(STSErrorPublisher.MandatoryInputMissing, "ProcessType"), "")
        End If

        ' exit if there are any missing parameters
        If oSTSError.HasErrors Then
            oSTSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            Return oResponse
        End If
        oResponse = DirectCast(oSAMBusiness.AttachCoverNote(oRequest), AttachCoverNoteResponseType)

        Return oResponse
    End Function
#End Region
#Region "Public method GetNumberingSchemeNo"
    ''' <summary>
    '''This method passes the GetNumberingSchemeNo request from web service layer to core implementation layer.
    '''It returns back the result from core implementation layer to web service layer.
    '''</summary>
    '''<param name="oRequest" type="SiriusFS.SAM.SFI.SAMForInsurance.GetNumberingSchemeNoRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsurance.GetNumberingSchemeNoResponseType</returns>
    '''<remarks></remarks>

    Public Function GetNumberingSchemeNo(ByVal oRequest As GetNumberingSchemeNoRequestType) As GetNumberingSchemeNoResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oSTSError As New STSErrorPublisher
        ' Dim oResponse As SiriusFS.SAM.Structure.BaseImplementationTypes.STSErrorType = Nothing
        Dim oResponse As New GetNumberingSchemeNoResponseType
        Const ACMethodName As String = "GetNumberingSchemeNo"


        If oRequest.SchemeType <> NumberingSchemeType.CoverNote And oRequest.SchemeType <> NumberingSchemeType.Policy And oRequest.SchemeType <> NumberingSchemeType.Quote Then
            oSTSError.AddInvalidField("SchemeType", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(STSErrorPublisher.MandatoryInputMissing, "SchemeType"), "")
        End If

        ' exit if there are any missing parameters
        If oSTSError.HasErrors Then
            oSTSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            Return oResponse
        End If
        oResponse = DirectCast(oSAMBusiness.GetNumberingSchemeNo(oRequest), GetNumberingSchemeNoResponseType)

        Return oResponse

    End Function

    'End (Sriram P) - (Tech Spec WR19 - Cover Note Functionality.doc)section 8.5.7.1
#End Region

#Region "GetFinancePlans"
    '''<summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="GetFinancePlansRequestType"></param>   
    '''<returns>GetFinancePlansResponseType</returns>
    '''<remarks></remarks>

    Public Function GetFinancePlans(ByVal oRequest As GetFinancePlansRequestType) As GetFinancePlansResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetFinancePlansResponseType

        oResponse = DirectCast(oSAMBusiness.GetFinancePlans(oRequest),  _
                            GetFinancePlansResponseType)

        Return oResponse
    End Function
#End Region
#Region "GetFinancePlanDetails"
    '''<summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="GetFinancePlanDetailsRequestType"></param>   
    '''<returns>GetFinancePlanDetailsResponseType</returns>
    '''<remarks></remarks>
    Public Function GetFinancePlanDetails(ByVal oRequest As GetFinancePlanDetailsRequestType) As GetFinancePlanDetailsResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetFinancePlanDetailsResponseType

        oResponse = DirectCast(oSAMBusiness.GetFinancePlanDetails(oRequest),  _
                    GetFinancePlanDetailsResponseType)

        Return oResponse
    End Function
#End Region

#Region " GetProductClaimsWorkflowOptions "
    '''<summary>
    ''' This method is the adapter method between GetProductClaimsWorkflowOptions methods in Web Service layer and Core implementation layer
    '''</summary>
    '''<param name="oRequest" type="SAMForInsuranceV2ImplementationTypes.GetProductClaimsWorkflowOptionsRequestType"></param>   
    '''<returns>SAMForInsuranceV2ImplementationTypes.GetProductClaimsWorkflowOptionsResponseType</returns>
    '''<remarks></remarks>
    Public Function GetProductClaimsWorkflowOptions(ByVal oRequest As GetProductClaimsWorkflowOptionsRequestType) As GetProductClaimsWorkflowOptionsResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetProductClaimsWorkflowOptionsResponseType

        oResponse = DirectCast(oSAMBusiness.GetProductClaimsWorkflowOptions(oRequest), GetProductClaimsWorkflowOptionsResponseType)

        Return oResponse
    End Function
#End Region


#Region "GetAgentSettings"
    Public Function GetAgentSettings(ByVal oRequest As GetAgentSettingsRequestType) As GetAgentSettingsResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetAgentSettingsResponseType
        oResponse = DirectCast(oSAMBusiness.GetAgentSettings(oRequest), GetAgentSettingsResponseType)
        Return oResponse
    End Function
#End Region

#Region "UpdateFinancePlanDetails"
    '''<summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="UpdateFinancePlanDetailsRequestType"></param>   
    '''<returns>UpdateFinancePlanDetailsResponseType</returns>
    '''<remarks></remarks>
    Public Function UpdateFinancePlanDetails(ByVal oRequest As UpdateFinancePlanDetailsRequestType) As UpdateFinancePlanDetailsResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New UpdateFinancePlanDetailsResponseType

        oResponse = DirectCast(oSAMBusiness.UpdateFinancePlanDetails(oRequest),  _
                                UpdateFinancePlanDetailsResponseType)

        Return oResponse
    End Function
#End Region


#Region "GetAccountBalanceByAccountCode"
    '''<summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="GetAccountBalanceByAccountCodeRequestType"></param>   
    '''<returns>GetAccountBalanceByAccountCodeResponseType</returns>
    '''<remarks></remarks>
    Public Function GetAccountBalanceByAccountCode(ByVal oRequest As GetAccountBalanceByAccountCodeRequestType) As GetAccountBalanceByAccountCodeResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetAccountBalanceByAccountCodeResponseType

        oResponse = DirectCast(oSAMBusiness.GetAccountBalanceByAccountCode(oRequest),  _
      GetAccountBalanceByAccountCodeResponseType)

        Return oResponse
    End Function
#End Region
#Region "GetMediaType"
    '''<summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="GetMediaTypeRequestType"></param>   
    '''<returns>GetMediaTypeResponseType</returns>
    '''<remarks></remarks>
    Public Function GetMediaType(ByVal oRequest As GetMediaTypeRequestType) As GetMediaTypeResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetMediaTypeResponseType

        oResponse = DirectCast(oSAMBusiness.GetMediaType(oRequest),  _
                    GetMediaTypeResponseType)

        Return oResponse
    End Function
#End Region

#Region "GetRatingSectionbyRiskType"
    ''' <summary>  
    ''' Get Rating Sections By Risk Type
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.BaseImplementationTypes.BaseGetRatingSectionbyRiskTypeRequestType</param>  
    Public Function GetRatingSectionByRiskType(ByVal oRequest As BaseGetRatingSectionByRiskTypeRequestType) As GetRatingSectionByRiskTypeResponseType


        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetRatingSectionByRiskTypeResponseType

        oResponse = DirectCast(oSAMBusiness.GetRatingSectionByRiskType(oRequest),  _
                                                GetRatingSectionByRiskTypeResponseType)

        Return oResponse
    End Function
#End Region

#Region "GetProductRiskEvents"
    Public Function GetProductRiskEvents(ByVal oRequest As GetProductRiskEventsRequestType) As GetProductRiskEventsResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetProductRiskEventsResponseType
        oResponse = DirectCast(oSAMBusiness.GetProductRiskEvents(oRequest), GetProductRiskEventsResponseType)
        Return oResponse
    End Function
#End Region

#Region "GetEventNoteType"
    Public Function GetEventNoteType(ByVal oRequest As GetEventNoteTypeRequestType) As GetEventNoteTypeResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetEventNoteTypeResponseType
        oResponse = DirectCast(oSAMBusiness.GetEventNoteType(oRequest), GetEventNoteTypeResponseType)
        Return oResponse
    End Function
#End Region

#Region "GetAccountingPeriod"
    '''<summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="GetMediaTypeRequestType"></param>   
    '''<returns>GetMediaTypeResponseType</returns>
    '''<remarks></remarks>
    Public Function GetAccountingPeriod(ByVal oRequest As GetAccountingPeriodRequestType) As GetAccountingPeriodResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetAccountingPeriodResponseType

        oResponse = DirectCast(oSAMBusiness.GetAccountingPeriod(oRequest),  _
            GetAccountingPeriodResponseType)

        Return oResponse
    End Function
#End Region


#Region "GetClaimPartyDetails"
    '''<summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="GetMediaTypeRequestType"></param>   
    '''<returns>GetMediaTypeResponseType</returns>
    '''<remarks></remarks>
    Public Function GetClaimPartyDetails(ByVal oRequest As GetClaimPartyDetailsRequestType) As GetClaimPartyDetailsResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetClaimPartyDetailsResponseType

        oResponse = DirectCast(oSAMBusiness.GetClaimPartyDetails(oRequest),  _
            GetClaimPartyDetailsResponseType)

        Return oResponse
    End Function
#End Region

#Region "GetPartyPolicies"
    '''<summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="GetPartyPoliciesRequestType"></param>   
    '''<returns>GetPartyPoliciesResponseType</returns>
    '''<remarks></remarks>
    Public Function GetPartyPolicies(ByVal oRequest As GetPartyPoliciesRequestType) As GetPartyPoliciesResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetPartyPoliciesResponseType

        oResponse = DirectCast(oSAMBusiness.GetPartyPolicies(oRequest), GetPartyPoliciesResponseType)

        Return oResponse
    End Function
#End Region

#Region "GetBankAccounts"
    Public Function GetBankAccounts(ByVal oRequest As GetBankAccountsRequestType) As GetBankAccountsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetBankAccountsResponseType

        oResponse = DirectCast(oSAMBusiness.GetBankAccounts(oRequest), GetBankAccountsResponseType)

        Return oResponse

    End Function
#End Region
#Region " GetPartyBankDetails "
    '''<summary>
    ''' This method is the adapter method between GetPartyBankDetails methods in Web Service layer and Core implementation layer
    '''</summary>
    '''<param name="oRequest" type="SAMForInsuranceV2ImplementationTypes.GetPartyBankDetailsRequestType"></param>   
    '''<returns>SAMForInsuranceV2ImplementationTypes.GetPartyBankDetailssResponseType</returns>
    '''<remarks></remarks>
    Public Function GetPartyBankDetails(ByVal oRequest As GetPartyBankDetailsRequestType) As GetPartyBankDetailsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)

        Dim oResponse As New GetPartyBankDetailsResponseType

        oResponse = DirectCast(oSAMBusiness.GetPartyBankDetails(oRequest), GetPartyBankDetailsResponseType)

        Return oResponse

    End Function
#End Region

#Region " AddPartyBankDetails "
    '''<summary>
    ''' This method is the adapter method between AddPartyBankDetails methods in Web Service layer and Core implementation layer
    '''</summary>
    '''<param name="oRequest" type="SAMForInsuranceV2ImplementationTypes.AddPartyBankDetailsRequestType"></param>   
    '''<returns>SAMForInsuranceV2ImplementationTypes.AddPartyBankDetailssResponseType</returns>
    '''<remarks></remarks>
    Public Function AddPartyBankDetails(ByVal oRequest As AddPartyBankDetailsRequestType) As AddPartyBankDetailsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)

        Dim oResponse As New AddPartyBankDetailsResponseType

        oResponse = DirectCast(oSAMBusiness.AddPartyBankDetails(oRequest), AddPartyBankDetailsResponseType)

        Return oResponse

    End Function
#End Region
#Region "UpdatePartyBankDetails"
    'Start (Bushra) - (Tech WPR 12 -UpdatePartyBankDetails)
    ''' <summary>  
    ''' This method is the adapter method between UpdatePartyBankDetails methods in Web Service layer and Core implementation layer
    ''' </summary>  
    ''' <param name="oRequest">An object of type UpdatePartyBankDetailsRequestType</param>  
    Public Function UpdatePartyBankDetails(ByVal oRequest As UpdatePartyBankDetailsRequestType) As UpdatePartyBankDetailsResponseType

        Dim oSAMBusiness As CoreSAMBusiness
        oSAMBusiness = New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New UpdatePartyBankDetailsResponseType
        oResponse = DirectCast(oSAMBusiness.UpdatePartyBankDetails(oRequest), UpdatePartyBankDetailsResponseType)

        Return oResponse

    End Function
    'End (Bushra) - (Tech WPR 12 -UpdatePartyBankDetails)
#End Region

#Region "DeletePartyBankDetails"
    'Start (Bushra) - (Tech WPR 12 -DeletePartyBankDetails)
    ''' <summary>  
    ''' DeletePartyBankDetailsRequestType
    ''' </summary>  
    ''' <param name="oRequest">An object of type DeletePartyBankDetailsRequestType</param>  
    Public Function DeletepartyBank(ByVal oRequest As DeletePartyBankDetailsRequestType) As DeletePartyBankDetailsResponseType

        Dim oSAMBusiness As CoreSAMBusiness
        oSAMBusiness = New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New DeletePartyBankDetailsResponseType

        oResponse = DirectCast(oSAMBusiness.DeletePartyBank(oRequest), DeletePartyBankDetailsResponseType)

        Return oResponse

    End Function
    'End (Bushra) - (Tech WPR 12 -UpdatePartyBankDetails)
#End Region

#Region "ActivatePartyBankDetails"
    'Start (Bushra) - (Tech WPR 12 -ActivatePartyBankDetails)
    ''' <summary>  
    ''' ActivatePartyBankDetails
    ''' </summary>  
    ''' <param name="oRequest">An object of type ActivatePartyBankDetailsRequestType</param>  
    Public Function ActivatePartyBankDetails(ByVal oRequest As ActivatePartyBankRequestType) As ActivatePartyBankResponseType

        Dim oSAMBusiness As CoreSAMBusiness
        Dim oResponse As New ActivatePartyBankResponseType
        oSAMBusiness = New CoreSAMBusiness(oRequest.BranchCode)
        oResponse = DirectCast(oSAMBusiness.ActivatePartyBank(oRequest), ActivatePartyBankResponseType)

        Return oResponse

    End Function
    'End (Bushra) - (Tech WPR 12 -ActivatePartyBankDetails)
#End Region

#Region "ValidateBankAccountNumber"
    'Start (Bushra) - (Tech WPR 12 -ValidateBankAccountNumber )
    ''' <summary>  
    ''' ValidateBankAccountNumber 
    ''' </summary>  
    ''' <param name="oRequest">An object of type ValidateBankAccountNumberRequestType</param>  
    Public Function ValidateBankAccountNumber(ByVal oRequest As ValidateBankAccountNumberRequestType) As ValidateBankAccountNumberResponseType

        Dim oSAMBusiness As CoreSAMBusiness
        oSAMBusiness = New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New ValidateBankAccountNumberResponseType

        oResponse = DirectCast(oSAMBusiness.ValidateBankAccountNumber(oRequest), ValidateBankAccountNumberResponseType)

        Return oResponse

    End Function
    'End (Bushra) - (Tech WPR 12 -ActivatePartyBankDetails)
#End Region

#Region " GetAllLivePolicyVersionAmounts "
    '''<summary>
    ''' This method is the adapter method between GetAllLivePolicyVersionAmounts methods in Web Service layer and Core implementation layer
    '''</summary>
    '''<param name="oRequest" type="SAMForInsuranceV2ImplementationTypes.GetAllLivePolicyVersionAmountsRequestType"></param>   
    '''<returns>SAMForInsuranceV2ImplementationTypes.GetAllLivePolicyVersionAmountsResponseType</returns>
    '''<remarks></remarks>
    Public Function GetAllLivePolicyVersionAmounts(ByVal oRequest As GetAllLivePolicyVersionAmountsRequestType) As GetAllLivePolicyVersionAmountsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)

        Dim oResponse As New GetAllLivePolicyVersionAmountsResponseType

        oResponse = DirectCast(oSAMBusiness.GetAllLivePolicyVersionAmounts(oRequest), GetAllLivePolicyVersionAmountsResponseType)

        Return oResponse

    End Function
#End Region
#Region "Public Method-GetPolicyStatusForMediaTypeStatus"
    ''' <summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="GetPolicyStatusForMediaTypeStatusRequestType"></param>   
    '''<returns>GetPolicyStatusForMediaTypeStatusResponseType</returns>
    '''<remarks></remarks>
    Public Function GetPolicyStatusForMediaTypeStatus(ByVal oRequest As GetPolicyStatusForMediaTypeStatusRequestType) As GetPolicyStatusForMediaTypeStatusResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetPolicyStatusForMediaTypeStatusResponseType

        oResponse = DirectCast(oSAMBusiness.GetPolicyStatusForMediaTypeStatus(oRequest), GetPolicyStatusForMediaTypeStatusResponseType)

        Return oResponse
    End Function
#End Region

#Region "Public Method-FindCashListReceipts"
    ''' <summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="FindCashListReceiptsRequestType"></param>   
    '''<returns>FindCashListReceiptsResponseType</returns>
    '''<remarks></remarks>
    Public Function FindCashListReceipts(ByVal oRequest As FindCashListReceiptsRequestType) As FindCashListReceiptsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New FindCashListReceiptsResponseType

        oResponse = DirectCast(oSAMBusiness.FindCashListReceipts(oRequest), FindCashListReceiptsResponseType)
        Return oResponse

    End Function
#End Region

#Region "Public Method-UpdateReceiptMediaTypeStatus"
    ''' <summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="UpdateReceiptMediaTypeStatusRequestType"></param>   
    '''<returns>UpdateReceiptMediaTypeStatusResponseType</returns>
    '''<remarks></remarks>
    Public Function UpdateReceiptMediaTypeStatus(ByVal oRequest As UpdateReceiptMediaTypeStatusRequestType) As UpdateReceiptMediaTypeStatusResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New UpdateReceiptMediaTypeStatusResponseType

        oResponse = DirectCast(oSAMBusiness.UpdateReceiptMediaTypeStatus(oRequest), UpdateReceiptMediaTypeStatusResponseType)

        Return oResponse
    End Function
#End Region

    Public Function UpdateQuotePaymentMethod(ByVal oRequest As UpdateQuotePaymentMethodRequestType) As UpdateQuotePaymentMethodResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)

        Dim oResponse As New UpdateQuotePaymentMethodResponseType

        oResponse = DirectCast(oSAMBusiness.UpdateQuotePaymentMethod(oRequest), UpdateQuotePaymentMethodResponseType)

        Return oResponse

    End Function
    Public Function GenerateDocumentsForEvent(ByVal oRequest As GenerateDocumentsForEventRequestType) As GenerateDocumentsForEventResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GenerateDocumentsForEventResponseType

        oResponse = DirectCast(oSAMBusiness.GenerateDocumentsForEvent(oRequest), GenerateDocumentsForEventResponseType)

        Return oResponse
    End Function

#Region "Public Method-GetAgentCommission"
    ''' <summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="GetAgentCommissionRequestType"></param>   
    '''<returns>GetAgentCommissionResponseType</returns>
    '''<remarks></remarks>
    Public Function GetAgentCommission(ByVal oRequest As GetAgentCommissionRequestType) As GetAgentCommissionResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetAgentCommissionResponseType

        oResponse = DirectCast(oSAMBusiness.GetAgentCommission(oRequest), GetAgentCommissionResponseType)

        Return oResponse

    End Function
#End Region

#Region "Public Method-UpdateAgentCommission"
    ''' <summary>
    '''This method pass the request object got from Web method to the CoreSAMBusiness layer
    '''</summary>
    '''<param name="oRequest" type="UpdateAgentCommissionRequestType"></param>   
    '''<returns>UpdateAgentCommissionResponseType</returns>
    '''<remarks></remarks>

    Public Function UpdateAgentCommission(ByVal oRequest As UpdateAgentCommissionRequestType) As UpdateAgentCommissionResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New UpdateAgentCommissionResponseType

        oResponse = DirectCast(oSAMBusiness.UpdateAgentCommission(oRequest), UpdateAgentCommissionResponseType)

        Return oResponse

    End Function
#End Region
    'Start  - Prakash - WPR85 Parelleling
    Public Function AddCashDeposit(ByVal oRequest As AddCashDepositRequestType) As AddCashDepositResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New AddCashDepositResponseType

        oResponse = DirectCast(oSAMBusiness.AddCashDeposit(oRequest), AddCashDepositResponseType)

        Return oResponse

    End Function

    Public Function UpdateCashDeposit(ByVal oRequest As UpdateCashDepositRequestType) As UpdateCashDepositResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New UpdateCashDepositResponseType

        oResponse = DirectCast(oSAMBusiness.UpdateCashDeposit(oRequest), UpdateCashDepositResponseType)

        Return oResponse

    End Function

    Public Function FindCashDeposit(ByVal oRequest As FindCashDepositRequestType) As FindCashDepositResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New FindCashDepositResponseType

        oResponse = DirectCast(oSAMBusiness.FindCashDeposit(oRequest), FindCashDepositResponseType)

        Return oResponse

    End Function

    Public Function GetLinkedCashDeposits(ByVal oRequest As GetLinkedCashDepositsRequestType) As GetLinkedCashDepositsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetLinkedCashDepositsResponseType

        oResponse = DirectCast(oSAMBusiness.GetLinkedCashDeposits(oRequest), GetLinkedCashDepositsResponseType)

        Return oResponse

    End Function

    Public Function GetCashDeposit(ByVal oRequest As GetCashDepositRequestType) As GetCashDepositResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetCashDepositResponseType

        oResponse = DirectCast(oSAMBusiness.GetCashDeposit(oRequest), GetCashDepositResponseType)

        Return oResponse

    End Function

    Public Function GetNextCashDepositRef(ByVal oRequest As GetNextCashDepositRefRequestType) As GetNextCashDepositRefResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetNextCashDepositRefResponseType

        oResponse = DirectCast(oSAMBusiness.GetNextCashDepositRef(oRequest), GetNextCashDepositRefResponseType)

        Return oResponse

    End Function

    Public Function GetBalanceForCD(ByVal oRequest As GetBalanceForCDRequestType) As GetBalanceForCDResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetBalanceForCDResponseType

        oResponse = DirectCast(oSAMBusiness.GetBalanceForCD(oRequest), GetBalanceForCDResponseType)

        Return oResponse

    End Function

#Region "GetPolicyDetailsForBouncedReceipt"
    Public Function GetPolicyDetailsForBouncedReceipt(ByVal oRequest As GetPolicyDetailsForBouncedReceiptRequestType) As GetPolicyDetailsForBouncedReceiptResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetPolicyDetailsForBouncedReceiptResponseType

        oResponse = DirectCast(oSAMBusiness.GetPolicyDetailsForBouncedReceipt(oRequest), GetPolicyDetailsForBouncedReceiptResponseType)

        Return oResponse

    End Function
#End Region
    'End  - Prakash - WPR85 Parelleling





#Region " AddJournal "
    ''' <summary>  
    '''  This method calls core implementation layer and creates manual journal transactions.
    ''' </summary>  
    ''' <param name="oRequest">An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.AddJournalRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SAMForInsuranceV2ImplementationTypes.AddJournalResponseType</returns>  
    Public Function AddJournal(ByVal oRequest As AddJournalRequestType) As AddJournalResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New AddJournalResponseType

        oResponse = DirectCast(oSAMBusiness.AddJournal(oRequest), AddJournalResponseType)

        Return oResponse

    End Function
    'End - (Sankar) - (Tech Spec - WR96 - UIIC SAM Manual Journal.doc)
#End Region


#Region "GetQuotesMarkedForCollection"
    Public Function GetQuotesMarkedForCollection(ByVal oRequest As GetQuotesMarkedForCollectionRequestType) As GetQuotesMarkedForCollectionResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetQuotesMarkedForCollectionResponseType

        oResponse = DirectCast(oSAMBusiness.GetQuotesMarkedForCollection(oRequest), GetQuotesMarkedForCollectionResponseType)

        Return oResponse

    End Function
#End Region


#Region "UpdateClaimReservesOrPayments"
    Public Function UpdateClaimReservesOrPayments(ByVal oRequest As UpdateClaimReservesOrPaymentsRequestType) As UpdateClaimReservesOrPaymentsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New UpdateClaimReservesOrPaymentsResponseType

        oResponse = DirectCast(oSAMBusiness.UpdateClaimReservesOrPayments(oRequest), UpdateClaimReservesOrPaymentsResponseType)

        Return oResponse

    End Function
#End Region

#Region "BindClaim"
    Public Function BindClaim(ByVal oRequest As BindClaimRequestType) As BindClaimResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New BindClaimResponseType

        oResponse = DirectCast(oSAMBusiness.BindClaim(oRequest), BindClaimResponseType)

        Return oResponse

    End Function
#End Region

#Region "GetAgentCommissionTax"
    Public Function GetAgentCommissionTax(ByVal oRequest As GetAgentCommissionTaxRequestType) As GetAgentCommissionTaxResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetAgentCommissionTaxResponseType

        oResponse = DirectCast(oSAMBusiness.GetAgentCommissionTax(oRequest), GetAgentCommissionTaxResponseType)

        Return oResponse

    End Function
#End Region
#Region "GETBROKERSUMMARY"
    ''' <summary>  
    ''' GetBrokerSummary
    ''' </summary>  
    ''' <param name="oRequest">An object of type GetBrokerSummaryRequestType</param>  
    Public Function GetBrokerSummary(ByVal oRequest As GetBrokerSummaryRequestType) As GetBrokerSummaryResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetBrokerSummaryResponseType

        oResponse = DirectCast(oSAMBusiness.GetBrokerSummary(oRequest), GetBrokerSummaryResponseType)

        Return oResponse

    End Function
    'End (Sandeep Kumar) - (Tech Spec - WPR13 -GetBrokerSummary)-
#End Region

    Public Function TransferQuote(ByVal oRequest As TransferQuoteRequestType) As TransferQuoteResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As TransferQuoteResponseType

        oResponse = DirectCast(oSAMBusiness.TransferQuote(oRequest), TransferQuoteResponseType)

        Return oResponse

    End Function

    Public Function GetDocumentDefaults(ByVal oRequest As GetDocumentDefaultsRequestType) As GetDocumentDefaultsResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetDocumentDefaultsResponseType

        oResponse = DirectCast(oSAMBusiness.GetDocumentDefaults(oRequest), GetDocumentDefaultsResponseType)

        Return oResponse
    End Function

    Public Function GetSharepointFileList(ByVal oRequest As GetSharepointFileListRequestType) As GetSharepointFileListResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetSharepointFileListResponseType

        oResponse = DirectCast(oSAMBusiness.GetSharepointFileList(oRequest), GetSharepointFileListResponseType)

        Return oResponse
    End Function

    Public Function GetStandardWordingTemplate(ByVal oRequest As GetStandardWordingTemplateRequestType) As GetStandardWordingTemplateResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetStandardWordingTemplateResponseType

        oResponse = DirectCast(oSAMBusiness.GetStandardWordingTemplate(oRequest), GetStandardWordingTemplateResponseType)

        Return oResponse

    End Function

    Public Function UpdateStandardWordingTemplate(ByVal oRequest As UpdateStandardWordingTemplateRequestType) As UpdateStandardWordingTemplateResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New UpdateStandardWordingTemplateResponseType

        oResponse = DirectCast(oSAMBusiness.UpdateStandardWordingTemplate(oRequest), UpdateStandardWordingTemplateResponseType)

        Return oResponse

    End Function

    Public Function CreateBackgroundJob(ByVal oRequest As CreateBackgroundJobRequestType) As CreateBackgroundJobResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As CreateBackgroundJobResponseType

        oResponse = DirectCast(oSAMBusiness.CreateBackgroundJob(oRequest), CreateBackgroundJobResponseType)

        Return oResponse

    End Function
    Public Function CopyQuote(ByVal oRequest As CopyQuoteRequestType) As CopyQuoteResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New CopyQuoteResponseType

        oResponse = DirectCast(oSAMBusiness.CopyQuote(oRequest), CopyQuoteResponseType)

        Return oResponse

    End Function

    Public Function UpdateQuoteStatus(ByVal oRequest As UpdateQuoteStatusRequestType) As UpdateQuoteStatusResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New UpdateQuoteStatusResponseType

        oResponse = DirectCast(oSAMBusiness.UpdateQuoteStatus(oRequest), UpdateQuoteStatusResponseType)

        Return oResponse

    End Function

    Public Function DeletePolicy(ByVal oRequest As DeletePolicyRequestType) As DeletePolicyResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New DeletePolicyResponseType

        oResponse = DirectCast(oSAMBusiness.DeletePolicy(oRequest), DeletePolicyResponseType)

        Return oResponse

    End Function

    Public Function GetPeriod(ByVal oRequest As GetPeriodRequestType) As GetPeriodResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetPeriodResponseType

        oResponse = DirectCast(oSAMBusiness.GetPeriod(oRequest), GetPeriodResponseType)

        Return oResponse

    End Function
    'WPR 33-75 ADDED
#Region "AddBackDatedMTAQuote"

    Public Function AddBackDatedMTAQuote(ByVal oRequest As AddBackDatedMTAQuoteRequestType) As AddBackDatedMTAQuoteResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New AddBackDatedMTAQuoteResponseType

        oResponse = DirectCast(oSAMBusiness.AddBackDatedMTAQuote(oRequest), AddBackDatedMTAQuoteResponseType)

        Return oResponse

    End Function
#End Region


#Region "GetBackdatedMTARiskVersions"

    Public Function GetBackdatedMTARiskVersions(ByVal oRequest As GetHeaderAndSummariesByKeyRequestType) As AddBackDatedMTAQuoteResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New AddBackDatedMTAQuoteResponseType

        oResponse = DirectCast(oSAMBusiness.GetBackdatedMTARiskVersions(oRequest), AddBackDatedMTAQuoteResponseType)

        Return oResponse

    End Function

#End Region
    'archana Code END
#Region "DeleteBackDatedVersions"

    Public Function DeleteBackDatedVersions(ByVal oRequest As DeleteBackDatedVersionsRequestType) As DeleteBackDatedVersionsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New DeleteBackDatedVersionsResponseType

        oResponse = DirectCast(oSAMBusiness.DeleteBackDatedVersions(oRequest), DeleteBackDatedVersionsResponseType)

        Return oResponse

    End Function

#End Region
    'WPR 33-75 END

#Region " GetReport"
    Public Function GetReport(ByVal oRequest As GetReportRequestType) As GetReportResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetReportResponseType

        oResponse = DirectCast(oSAMBusiness.GetReport(oRequest), GetReportResponseType)

        Return oResponse

    End Function
#End Region

    Public Function GetTaxes(ByVal oRequest As GetTaxesRequestType) As GetTaxesResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As GetTaxesResponseType

        oResponse = DirectCast(oSAMBusiness.GetTaxes(oRequest), GetTaxesResponseType)

        Return oResponse
    End Function
    Public Function UpdateTaxes(ByVal oRequest As UpdateTaxesRequestType) As UpdateTaxesResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As UpdateTaxesResponseType

        oResponse = DirectCast(oSAMBusiness.UpdateTaxes(oRequest), UpdateTaxesResponseType)

        Return oResponse
    End Function
#Region "ReRate All Risks"
    'Start - (Jai Prakash) - (WPR60_ReRate_All_Transaction_Risks-Enhancement)
    ''' <summary>
    '''This function will call the CoreSAMBusiness to implement the required functionality.
    '''</summary>
    '''<param name="ReRateAllRisksRequestType" type="ReRateAllRisksResponseType"></param>   
    '''<remarks></remarks>  
    Public Function ReRateAllRisks(ByVal oRequest As ReRateAllRisksRequestType) As ReRateAllRisksResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New ReRateAllRisksResponseType

        oResponse = DirectCast(oSAMBusiness.ReRateAllRisks(oRequest), ReRateAllRisksResponseType)

        Return oResponse
    End Function
    'End - (Jai Prakash) - (WPR60_ReRate_All_Transaction_Risks-Enhancement)
#End Region
#Region "FindAddress"
    ' Start (Deepak) - (Tech WPR032 -FindAddress)-
    ''' <summary>  
    ''' FindAddress
    ''' </summary>  
    ''' <param name="oRequest">An object of type FindAddressRequestType</param>  
    Public Function FindAddress(ByVal oRequest As FindAddressRequestType) As FindAddressResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New FindAddressResponseType

        oResponse = DirectCast(oSAMBusiness.FindAddress(oRequest), FindAddressResponseType)

        Return oResponse

    End Function

    Public Function ProcessClaim(ByVal oRequest As BaseClaimProcessRequestType) As BaseClaimProcessResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As BaseClaimProcessResponseType

        oResponse = DirectCast(oSAMBusiness.ProcessClaim(oRequest), BaseClaimProcessResponseType)

        Return oResponse
    End Function

#End Region

    'WPR14-MID
    Public Function GetMIDFiles(ByVal oRequest As GetMIDFilesRequestType) As GetMIDFilesResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetMIDFilesResponseType

        oResponse = DirectCast(oSAMBusiness.GetMIDFiles(oRequest), GetMIDFilesResponseType)

        Return oResponse

    End Function

    Public Function GetMIDFileDetails(ByVal oRequest As GetMIDFileDetailsRequestType) As GetMIDFileDetailsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetMIDFileDetailsResponseType

        oResponse = DirectCast(oSAMBusiness.GetMIDFileDetails(oRequest), GetMIDFileDetailsResponseType)

        Return oResponse

    End Function
    'END WPR14-MID

#Region "AddDocumentToDocumaster"

    Public Function AddDocumentToDocumaster(ByVal oRequest As AddDocumentToDocumasterRequestType) As AddDocumentToDocumasterResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New AddDocumentToDocumasterResponseType

        oResponse = DirectCast(oSAMBusiness.AddDocumentToDocumaster(oRequest), AddDocumentToDocumasterResponseType)

        Return oResponse

    End Function


    ' End (Ravikumar Pasupuleti)-(Tech Spec - UIIC WR63 - Insurer payments - AddWriteOff.doc) - (7.1.4.7)
#End Region
#Region "GetDMEFolder"
    Public Function GetDMEFolder(ByVal oRequest As GetDMEFolderRequestType) As GetDMEFolderResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetDMEFolderResponseType

        oResponse = DirectCast(oSAMBusiness.GetDMEFolder(oRequest), GetDMEFolderResponseType)

        Return oResponse
    End Function
#End Region

#Region "FindDMEDocuments"
    Public Function FindDMEDocuments(ByVal oRequest As FindDMEDocumentsRequestType) As FindDMEDocumentsResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New FindDMEDocumentsResponseType

        oResponse = DirectCast(oSAMBusiness.FindDMEDocuments(oRequest), FindDMEDocumentsResponseType)

        Return oResponse
    End Function
#End Region

    Public Function ProcessPaymentAndBindQuote(ByVal oRequest As ProcessPaymentAndBindQuoteRequestType) As ProcessPaymentAndBindQuoteResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New ProcessPaymentAndBindQuoteResponseType

        oResponse = DirectCast(oSAMBusiness.ProcessPaymentAndBindQuote(oRequest), ProcessPaymentAndBindQuoteResponseType)

        Return oResponse

    End Function
#Region "Function - UpdateFee"
    Public Function UpdateFee(ByVal oRequest As UpdateFeeRequestType) As UpdateFeeResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New UpdateFeeResponseType

        oResponse = DirectCast(oSAMBusiness.UpdateFee(oRequest), UpdateFeeResponseType)

        Return oResponse
    End Function
#End Region
    Public Function GetProductsForUserBranch(ByVal oRequest As GetProductsForUserBranchRequestType) As GetProductsForUserBranchResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Const ACMethodName As String = "GetProductsForUserBranch"
        Dim oResponse As New GetProductsForUserBranchResponseType

        oResponse = DirectCast(oSAMBusiness.GetProductsForUserBranch(oRequest), GetProductsForUserBranchResponseType)

        Return oResponse

    End Function


    Public Function ClearCache(ByVal oRequest As ClearCacheRequestType) As ClearCacheResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New SAMForInsuranceV2ImplementationTypes.ClearCacheResponseType

        oResponse = DirectCast(oSAMBusiness.ClearCache(oRequest), ClearCacheResponseType)

        Return oResponse
    End Function


    '''<summary>
    ''' This method is the adapter method between UpdateRiskStatus methods in Web Service layer and Core implementation layer
    '''</summary>
    '''<param name="oUpdateRiskStatusRequest" type="SAMForInsuranceV2ImplementationTypes.UpdateRiskStatusRequestType"></param>   
    '''<returns>SAMForInsuranceV2ImplementationTypes.UpdateRiskStatusResponseType</returns>
    '''<remarks></remarks>
    Public Function UpdateRiskStatus(ByVal oUpdateRiskStatusRequest As UpdateRiskStatusRequestType) As UpdateRiskStatusResponseType
        Dim oSAMBusiness As CoreSAMBusiness
        Dim oResponse As New UpdateRiskStatusResponseType

        oSAMBusiness = New CoreSAMBusiness(oUpdateRiskStatusRequest.BranchCode)

        oResponse = DirectCast(oSAMBusiness.UpdateRiskStatus(oUpdateRiskStatusRequest), UpdateRiskStatusResponseType)

        Return oResponse
    End Function

#Region "GetTaskOnKeys"
    Public Function GetTaskOnKeys(ByVal oRequest As GetTaskOnKeysRequestType) As GetTaskOnKeysResponseType
        Dim oSAMBusiness As New CoreSAMBusiness
        Dim oResponse As New GetTaskOnKeysResponseType
        oResponse = DirectCast(oSAMBusiness.GetTaskOnKeys(oRequest), GetTaskOnKeysResponseType)
        Return oResponse
    End Function
#End Region

#Region "UpdateTaskStatus"
    Public Function UpdateTaskStatus(ByVal oRequest As UpdateTaskStatusRequestType) As UpdateTaskStatusResponseType
        Dim oSAMBusiness As New CoreSAMBusiness
        Dim oResponse As New UpdateTaskStatusResponseType
        oResponse = DirectCast(oSAMBusiness.UpdateTaskStatus(oRequest), UpdateTaskStatusResponseType)
        Return oResponse
    End Function
#End Region
#Region "UpdateRiskStatus"
    Public Function UpdateRiskStatus(ByVal oRequest As UpdateRiskStatusRequestType) As UpdateRiskStatusResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New UpdateRiskStatusResponseType

        oResponse = DirectCast(oSAMBusiness.UpdateRiskStatus(oRequest), UpdateRiskStatusResponseType)

        Return oResponse

    End Function
#End Region
End Class

