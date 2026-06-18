Option Strict On
Option Explicit On

Imports SiriusFS.SAM.CoreImplementation
Imports Microsoft.ApplicationBlocks.ExceptionManagement
Imports SiriusFS.SAM.Structure
Imports SiriusFS.SAM.Structure.STSErrorPublisher
Imports SiriusFS.SAM.Structure.SAMForInsuranceImplementationTypes
Imports SiriusFS.SAM.Structure.BaseImplementationTypes

Friend Class SAMForInsuranceBusiness

    Private Enum enumTypeOfPackage ' public?
        SAMForInsurancePackage
        AgentsPackage
        AnonymousPackage
        CustomersPackage
        MessagingPackage
        ReportingPackage
        OtherPackage
    End Enum

    ''' <summary>   
    ''' This method will be an intermediator for WebMethod and business Layer
    '''</summary>
    '''<param name="oRequest"> It is an object of an class GetDefaultRiskClausesRequestType></param>    
    '''<remarks></remarks>
    Public Function GetDefaultRiskClauses(ByVal oRequest As GetDefaultRiskClausesRequestType) As GetDefaultRiskClausesResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New GetDefaultRiskClausesResponseType

        oResponse = DirectCast(oSAMBusiness.GetDefaultRiskClauses(oRequest), GetDefaultRiskClausesResponseType)

        Return oResponse

    End Function

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

        If oRequest.RunDefaultRules Then
            'RUN DEFAULT RULES (ADD)
            'Declare Implementation Type for Running Default Rules
            Dim oRulesIn As New SiriusFS.SAM.Structure.BaseImplementationTypes.BaseRunDefaultRulesAddRequestType
            Dim oRulesOut As SiriusFS.SAM.Structure.BaseImplementationTypes.BaseRunDefaultRulesAddResponseType
            'Set Inputs In
            oRulesIn.BranchCode = oRequest.BranchCode
            oRulesIn.ScreenCode = oRequest.ScreenCode
            oRulesIn.DataModelCode = oRequest.DataModelCode
            oRulesIn.XMLDataSet = oResponse.XMLDataSet 'Take the output from previous calls
            'Run Code
            oRulesOut = oSAMBusiness.RunDefaultRulesAdd(oRulesIn)
            'Set Outputs Back
            oResponse.XMLDataSet = oRulesOut.XMLDataSet
            If Not (oRulesOut.STSError Is Nothing) Then
                oResponse.STSError = oRulesOut.STSError
                Return oResponse
            End If
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

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim STSError As New STSErrorPublisher
        Const ACMethodName As String = "ChangePassword"
        Dim oResponse As New ChangePasswordResponseType

        ' Exit if there are any missing parameters
        If STSError.HasErrors Then
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            Return oResponse
        End If

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

    Public Function GetPoliciesInRenewal(ByVal oRequest As GetPoliciesInRenewalRequestType) As GetPoliciesInRenewalResponseType
        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        'Const ACMethodName As String = "GetPoliciesInRenewal"
        Dim oResponse As New GetPoliciesInRenewalResponseType
        oResponse = DirectCast(oSAMBusiness.GetPoliciesInRenewal(oRequest), GetPoliciesInRenewalResponseType)
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

    Public Function FindInsuranceFileForClaims(ByVal oRequest As FindInsuranceFileForClaimsRequestType) As FindInsuranceFileForClaimsResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Dim oResponse As New FindInsuranceFileForClaimsResponseType

        oResponse = DirectCast(oSAMBusiness.FindInsuranceFileForClaims(oRequest), FindInsuranceFileForClaimsResponseType)

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

    Public Function GetValidPrimaryCauses(ByVal oRequest As GetValidPrimaryCausesRequestType) As GetValidPrimaryCausesResponseType

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)
        Const ACMethodName As String = "GetValidPrimaryCauses"
        Dim oResponse As New GetValidPrimaryCausesResponseType

        oResponse = DirectCast(oSAMBusiness.GetValidPrimaryCauses(oRequest), GetValidPrimaryCausesResponseType)

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

    Public Function GetRenewalStatus(ByVal request As GetRenewalStatusRequestType) As GetRenewalStatusResponseType
        Dim samBusiness As New CoreSAMBusiness(request.BranchCode)
        Dim response As New GetRenewalStatusResponseType
        response = DirectCast(samBusiness.GetRenewalStatus(request), GetRenewalStatusResponseType)
        Return response
    End Function
    Public Function ReplacePartyContact(ByVal request As ReplacePartyContactRequestType) As ReplacePartyContactResponseType
        Dim samBusiness As New CoreSAMBusiness(request.BranchCode)
        Dim response As New ReplacePartyContactResponseType
        response = DirectCast(samBusiness.ReplacePartyContact(request), ReplacePartyContactResponseType)
        Return response
    End Function
    Public Function GetExistingInstalmentPlanPaymentDetails(ByVal request As GetExistingInstalmentPlanPaymentDetailsRequestType) As GetExistingInstalmentPlanPaymentDetailsResponseType
        Dim samBusiness As New CoreSAMBusiness(request.BranchCode)
        Dim response As New GetExistingInstalmentPlanPaymentDetailsResponseType
        response = DirectCast(samBusiness.GetExistingInstalmentPlanPaymentDetails(request), GetExistingInstalmentPlanPaymentDetailsResponseType)
        Return response
    End Function
    Public Function GenerateInvite(ByVal request As GenerateInviteRequestType) As GenerateInviteResponseType
        Dim samBusiness As New CoreSAMBusiness(request.BranchCode)
        Dim response As New GenerateInviteResponseType
        response = DirectCast(samBusiness.GenerateInvite(request), GenerateInviteResponseType)
        Return response
    End Function
    Public Function LapseRenewal(ByVal request As LapseRenewalRequestType) As LapseRenewalResponseType
        Dim samBusiness As New CoreSAMBusiness(request.BranchCode)
        Dim response As New LapseRenewalResponseType
        response = DirectCast(samBusiness.LapseRenewal(request), LapseRenewalResponseType)
        Return response
    End Function
    Public Sub RunRenewalSelection(ByVal oRequest As RunRenewalSelectionRequestType)

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)

        oSAMBusiness.RunRenewalSelection(oRequest)

    End Sub
    Public Sub RunRenewalInvitation(ByVal oRequest As RunRenewalInviteRequestType)

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)

        oSAMBusiness.RunRenewalInvitation(oRequest)

    End Sub
    Public Sub RunRenewalAccept(ByVal oRequest As RunRenewalAcceptRequestType)

        Dim oSAMBusiness As New CoreSAMBusiness(oRequest.BranchCode)

        oSAMBusiness.RunRenewalAccept(oRequest)

    End Sub

    Public Function PolicyDataImport(ByVal PolicyDataImportRequest As PolicyDataImportRequestType) As PolicyDataImportResponseType

        'Const ACMethodName As String = "PolicyDataImport"

        ' Declare the Response object
        Dim response As New PolicyDataImportResponseType

        ' Declare the Core SAM business object
        Dim oBusiness As New CoreSAMBusiness

        response = DirectCast(oBusiness.PolicyDataImport(PolicyDataImportRequest), PolicyDataImportResponseType)

        Return response

    End Function

    Public Function DocumentDataImport(ByVal DocumentDataImportRequest As DocumentDataImportRequestType) As PostDocumentResponseType

        'Const ACMethodName As String = "DocumentDataImport"

        ' Declare the Response object
        Dim response As New PostDocumentResponseType

        ' Declare the Core SAM business object
        Dim oBusiness As New CoreSAMBusiness

        response = DirectCast(oBusiness.DocumentDataImport(DocumentDataImportRequest), PostDocumentResponseType)

        Return response

    End Function

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

    ' End (Sriram P) - (Tech Spec WR19 - Cover Note Functionality.doc)section 8.2.3.9

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
        ' Dim oResponse As SiriusFS.SAM.Structure.BaseImplementationTypes.STSErrorType = Nothing
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

End Class

