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
Imports System.Xml
Imports System.IO

Partial Public Class PureService
    Implements IPurePolicyService
    Implements IPureCoreService

    Private Shared ReadOnly m_Sellock As New Object
    Private Shared ReadOnly m_Invlock As New Object
    Private Shared ReadOnly m_Acclock As New Object

    Public Function AddBackDatedMTAQuote(ByVal oAddBackDatedMTAQuoteRequest As AddBackDatedMTAQuoteRequestType) As AddBackDatedMTAQuoteResponseType Implements IPurePolicyService.AddBackDatedMTAQuote

        Try

            Dim sUserName As String = oAddBackDatedMTAQuoteRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMAMtaQte", iUserId)
            CommonFunctions.CheckSecurityToken(oAddBackDatedMTAQuoteRequest.WCFSecurityToken)

            Dim oResponse As New AddBackDatedMTAQuoteResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oAddBackDatedMTAQuoteRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.AddBackDatedMTAQuoteRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.AddBackDatedMTAQuoteResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oAddBackDatedMTAQuoteRequest.BranchCode
            oImpRequest.EffectiveDate = oAddBackDatedMTAQuoteRequest.EffectiveDate
            oImpRequest.InsuranceFileKey = oAddBackDatedMTAQuoteRequest.InsuranceFileKey
            oImpRequest.InsuranceFolderKey = oAddBackDatedMTAQuoteRequest.InsuranceFolderKey
            oImpRequest.PartyCnt = oAddBackDatedMTAQuoteRequest.PartyCnt
            oImpRequest.TransactionType = CType([Enum].ToObject(GetType(TransactionType), oAddBackDatedMTAQuoteRequest.TransactionType), BaseImplementationTypes.TransactionType)

            oImpRequest.IsInteractive = oAddBackDatedMTAQuoteRequest.IsInteractive
            oImpRequest.WCFSecurityToken = If(oAddBackDatedMTAQuoteRequest.WCFSecurityToken.Length > 0, oAddBackDatedMTAQuoteRequest.WCFSecurityToken, "WCFSecurityToken")


            Try
                ' Call the implementation method
                oImpResponse = oBusiness.AddBackDatedMTAQuote(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    'oResponse.BackdatedTransactions = SAMFunc.GetDeserializedValues(Of List(Of BaseAddBackDatedMTAQuoteResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseAddBackDatedMTAQuoteResponseTypeBackdatedTransactions", sConvertToTypeName:="BaseAddBackDatedMTAQuoteResponseTypeRow")
                    If oImpRequest.TransactionType = BaseImplementationTypes.TransactionType.MTC Then

                        oResponse.BackdatedTransactions = DataTabletoList_AddBackDatedMTCQuote(oImpResponse.ResultData.Tables(0))
                    Else

                        oResponse.BackdatedTransactions = DataTabletoList_AddBackDatedMTAQuote(oImpResponse.ResultData.Tables(0))

                    End If

                End If
                oResponse.FailureReason = oImpResponse.FailureReason

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oAddBackDatedMTAQuoteRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oAddBackDatedMTAQuoteRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="AddMtaQuoteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddMtaQuote(ByVal AddMtaQuoteRequest As AddMtaQuoteRequestType) As AddMtaQuoteResponseType Implements IPurePolicyService.AddMtaQuote
        Try

            Dim sUserName As String = AddMtaQuoteRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMAMtaQte", iUserId)
            CommonFunctions.CheckSecurityToken(AddMtaQuoteRequest.WCFSecurityToken)
            Dim msgResponse As New AddMtaQuoteResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, AddMtaQuoteRequest.BranchCode)

            ' Implementation structures
            Dim impRequest As New SAMForInsuranceV2ImplementationTypes.AddMtaQuoteRequestType
            Dim impResponse As SAMForInsuranceV2ImplementationTypes.AddMtaQuoteResponseType = Nothing


            ' Convert the incoming interface structures into the implementation structures
            impRequest.BranchCode = AddMtaQuoteRequest.BranchCode
            impRequest.EffectiveDate = AddMtaQuoteRequest.EffectiveDate
            impRequest.ExpiryDate = AddMtaQuoteRequest.ExpiryDate
            impRequest.InsuranceFileKey = AddMtaQuoteRequest.InsuranceFileKey
            impRequest.MtaReason = AddMtaQuoteRequest.MtaReason
            impRequest.TypeOfMta = AddMtaQuoteRequest.TypeOfMta

            impRequest.InsuredName = AddMtaQuoteRequest.InsuredName
            impRequest.PolicyKey = AddMtaQuoteRequest.PolicyKey
            impRequest.Regarding = AddMtaQuoteRequest.Regarding
            impRequest.AlternateReference = AddMtaQuoteRequest.AlternateReference
            impRequest.PolicyStatusCode = AddMtaQuoteRequest.PolicyStatusCode
            impRequest.AnalysisCode = AddMtaQuoteRequest.AnalysisCode
            impRequest.BusinessTypeCode = AddMtaQuoteRequest.BusinessTypeCode
            impRequest.IssueDate = AddMtaQuoteRequest.IssueDate

            impRequest.ProposalDate = AddMtaQuoteRequest.ProposalDate
            impRequest.FrequencyCode = AddMtaQuoteRequest.FrequencyCode
            impRequest.LTUExpiryDate = AddMtaQuoteRequest.LTUExpiryDate
            impRequest.StopReasonCode = AddMtaQuoteRequest.StopReasonCode
            impRequest.RenewalMethodCode = AddMtaQuoteRequest.RenewalMethodCode
            impRequest.LapseCancelReasonCode = AddMtaQuoteRequest.LapseCancelReasonCode
            impRequest.LapseCancelDate = AddMtaQuoteRequest.LapseCancelDate
            impRequest.ReferredAtRenewal = AddMtaQuoteRequest.ReferredAtRenewal
            impRequest.ReferredOnMTA = AddMtaQuoteRequest.ReferredOnMTA
            impRequest.PolicyStyleCode = AddMtaQuoteRequest.PolicyStyleCode

            impRequest.IssueDateSpecified = AddMtaQuoteRequest.IssueDateSpecified
            impRequest.LapseCancelDateSpecified = AddMtaQuoteRequest.LapseCancelDateSpecified
            impRequest.LTUExpiryDateSpecified = AddMtaQuoteRequest.LTUExpiryDateSpecified
            impRequest.ReferredOnMTASpecified = AddMtaQuoteRequest.ReferredOnMTASpecified
            impRequest.ReferredAtRenewalSpecified = AddMtaQuoteRequest.ReferredAtRenewalSpecified
            impRequest.ProposalDateSpecified = AddMtaQuoteRequest.ProposalDateSpecified
            'Added on 01-sep-2008 as per the discusiion with gaurav
            impRequest.AccountHandlerCnt = AddMtaQuoteRequest.AccountHandlerCnt
            impRequest.AccountHandlerCntSpecified = AddMtaQuoteRequest.AccountHandlerCntSpecified

            impRequest.IsReinstatement = AddMtaQuoteRequest.IsReinstatement

            'Added  Request object TransactionType (SAM Gap done by Vijayakumar as per discussed with Gaurav on 06-Nov-2008)
            impRequest.TransactionType = CType([Enum].ToObject(GetType(TransactionType), AddMtaQuoteRequest.TransactionType), BaseImplementationTypes.TransactionType)
            impRequest.QuoteExpiryDateSpecified = AddMtaQuoteRequest.QuoteExpiryDateSpecified
            impRequest.QuoteExpiryDate = AddMtaQuoteRequest.QuoteExpiryDate
            impRequest.CreatedById = iUserId

            impRequest.PutOnNextInstalmentRenewal = AddMtaQuoteRequest.PutOnNextInstalmentRenewal
            impRequest.PutOnNextInstalmentRenewalSpecified = AddMtaQuoteRequest.PutOnNextInstalmentRenewalSpecified
            impRequest.RenewalDayNo = AddMtaQuoteRequest.RenewalDayNo
            impRequest.RenewalDayNoSpecified = AddMtaQuoteRequest.RenewalDayNoSpecified
            impRequest.RenewalDate = AddMtaQuoteRequest.RenewalDate
            impRequest.RenewalDateSpecified = AddMtaQuoteRequest.RenewalDateSpecified
            impRequest.CoInsurancePlacement = AddMtaQuoteRequest.CoInsurancePlacement
            impRequest.CorrespondenceType = AddMtaQuoteRequest.CorrespondenceType
            impRequest.DefaultPreferredCorrespondence = AddMtaQuoteRequest.DefaultPreferredCorrespondence
            impRequest.IsAgentReceiveCorrespondence = AddMtaQuoteRequest.IsAgentReceiveCorrespondence

            impRequest.AnniversaryDate = AddMtaQuoteRequest.AnniversaryDate
            impRequest.AnniversaryDateSpecified = AddMtaQuoteRequest.AnniversaryDateSpecified
            impRequest.OldPolicyNumber = AddMtaQuoteRequest.OldPolicyNumber

            Try
                impResponse = oBusiness.AddMtaQuote(impRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(impResponse.STSError)
                msgResponse.InsuranceFileKey = impResponse.InsuranceFileKey
                msgResponse.QuoteExpiryDate = impResponse.QuoteExpiryDate
                msgResponse.QuoteTimeStamp = impResponse.QuoteTimeStamp
                msgResponse.CanBeAddedToPfPlan = impResponse.CanBeAddedToPfPlan

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(impResponse, msgResponse, ex, CommonFunctions.CreateDictionary(AddMtaQuoteRequest))
            End Try
            Return msgResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(AddMtaQuoteRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' This is webservice method for AddPayNowReceipt
    '''<param name="AddPayNowReceiptRequest" type="AddPayNowReceiptRequestType"></param>   
    '''</summary>
    '''<returns>AddPayNowReceiptResponseType</returns>
    '''<remarks></remarks>  
    Public Function AddPayNowReceipt(ByVal AddPayNowReceiptRequest As AddPayNowReceiptRequestType) As AddPayNowReceiptResponseType Implements IPurePolicyService.AddPayNowReceipt

        Try

            Dim sUserName As String = AddPayNowReceiptRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMADAGRPT", iUserId)
            CommonFunctions.CheckSecurityToken(AddPayNowReceiptRequest.WCFSecurityToken)
            Dim oResponse As New AddPayNowReceiptResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, AddPayNowReceiptRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.AddPayNowReceiptRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.AddPayNowReceiptResponseType = Nothing

            oImpRequest.BranchCode = AddPayNowReceiptRequest.BranchCode
            oImpRequest.PartyKey = AddPayNowReceiptRequest.PartyKey
            Dim oImpReceipt As New BaseImplementationTypes.BaseReceiptType
            With oImpReceipt
                .BankAccountName = AddPayNowReceiptRequest.Receipt.BankAccountName
                .CurrencyCode = AddPayNowReceiptRequest.Receipt.CurrencyCode
                .Address1 = AddPayNowReceiptRequest.Receipt.Address1
                .Address2 = AddPayNowReceiptRequest.Receipt.Address2
                .Address3 = AddPayNowReceiptRequest.Receipt.Address3
                .Address4 = AddPayNowReceiptRequest.Receipt.Address4
                .Amount = AddPayNowReceiptRequest.Receipt.Amount
                .CashListRef = AddPayNowReceiptRequest.Receipt.CashListRef
                .CCAuthCode = AddPayNowReceiptRequest.Receipt.CCAuthCode
                .CCCustomer = AddPayNowReceiptRequest.Receipt.CCCustomer
                .CCExpiryDate = AddPayNowReceiptRequest.Receipt.CCExpiryDate
                .CCIssue = AddPayNowReceiptRequest.Receipt.CCIssue
                .CCManualAuthCode = AddPayNowReceiptRequest.Receipt.CCManualAuthCode
                .CCName = AddPayNowReceiptRequest.Receipt.CCName
                .CCNumber = AddPayNowReceiptRequest.Receipt.CCNumber
                .CCPin = AddPayNowReceiptRequest.Receipt.CCPin
                .CCStartDate = AddPayNowReceiptRequest.Receipt.CCStartDate
                .CCTransactionCode = AddPayNowReceiptRequest.Receipt.CCTransactionCode
                .ChequeDate = AddPayNowReceiptRequest.Receipt.ChequeDate
                .ChequeDateSpecified = AddPayNowReceiptRequest.Receipt.ChequeDateSpecified
                .ChequeName = AddPayNowReceiptRequest.Receipt.ChequeName
                .ContactName = AddPayNowReceiptRequest.Receipt.ContactName
                .CountryCode = AddPayNowReceiptRequest.Receipt.CountryCode
                .MediaReference = AddPayNowReceiptRequest.Receipt.MediaReference
                .MediaTypeCode = AddPayNowReceiptRequest.Receipt.MediaTypeCode
                .MediaTypeIssuerCode = AddPayNowReceiptRequest.Receipt.MediaTypeIssuerCode
                .OurReference = AddPayNowReceiptRequest.Receipt.OurReference
                .PostalCode = AddPayNowReceiptRequest.Receipt.PostalCode
                .ReceiptTypeCode = AddPayNowReceiptRequest.Receipt.ReceiptTypeCode
                .TheirReference = AddPayNowReceiptRequest.Receipt.TheirReference
                .TransactionDate = AddPayNowReceiptRequest.Receipt.TransactionDate

                .CollectionDateSpecified = AddPayNowReceiptRequest.Receipt.CollectionDateSpecified
                If (.CollectionDateSpecified = AddPayNowReceiptRequest.Receipt.CollectionDateSpecified) Then
                    .CollectionDate = AddPayNowReceiptRequest.Receipt.CollectionDate
                End If
                .Comments = AddPayNowReceiptRequest.Receipt.Comments

                .CCTypeCode = AddPayNowReceiptRequest.Receipt.CCTypeCode
                .CCCashListItemBankCode = AddPayNowReceiptRequest.Receipt.CCCashListItemBankCode
                .CCTransactionSlipNumber = AddPayNowReceiptRequest.Receipt.CCTransactionSlipNumber
                If (AddPayNowReceiptRequest.Receipt.Bank IsNot Nothing) Then
                    .Bank = CommonFunctions.ToBaseImpBaseBankReceiptType(AddPayNowReceiptRequest.Receipt.Bank)
                End If

            End With
            ' assign the implementation receipt to the request
            oImpRequest.Receipt = oImpReceipt

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.AddPayNowReceipt(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(AddPayNowReceiptRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(AddPayNowReceiptRequest))
            Return Nothing
        End Try

    End Function

    Public Function AddQuoteV2(ByVal AddQuoteV2Request As AddQuoteV2RequestType) As AddQuoteV2ResponseType Implements IPurePolicyService.AddQuoteV2

        Try

            Dim sUserName As String = AddQuoteV2Request.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMAQuot", iUserId)
            CommonFunctions.CheckSecurityToken(AddQuoteV2Request.WCFSecurityToken)
            Dim oResponse As New AddQuoteV2ResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, AddQuoteV2Request.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.AddQuoteV2RequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.AddQuoteV2ResponseType = Nothing

            oImpRequest.AgentKeySpecified = AddQuoteV2Request.AgentKeySpecified
            oImpRequest.AgentKey = AddQuoteV2Request.AgentKey

            oImpRequest.CoverStartDate = AddQuoteV2Request.CoverStartDate
            oImpRequest.CoverEndDate = AddQuoteV2Request.CoverEndDate
            oImpRequest.ProductCode = AddQuoteV2Request.ProductCode
            oImpRequest.Description = AddQuoteV2Request.Description
            oImpRequest.QuoteRef = AddQuoteV2Request.QuoteRef
            oImpRequest.InsuredName = AddQuoteV2Request.InsuredName
            oImpRequest.CurrencyCode = AddQuoteV2Request.CurrencyCode
            oImpRequest.AnalysisCode = AddQuoteV2Request.AnalysisCode
            oImpRequest.AlternativeRef = AddQuoteV2Request.AlternativeRef

            oImpRequest.BranchCode = AddQuoteV2Request.BranchCode
            oImpRequest.PartyKey = AddQuoteV2Request.PartyKey
            oImpRequest.SubBranchCode = AddQuoteV2Request.SubBranchCode
            oImpRequest.ConsolidatedLeadAgentCommissionSpecified = AddQuoteV2Request.ConsolidatedLeadAgentCommissionSpecified
            oImpRequest.ConsolidatedLeadAgentCommission = AddQuoteV2Request.ConsolidatedLeadAgentCommission
            oImpRequest.ConsolidatedSubAgentCommissionSpecified = AddQuoteV2Request.ConsolidatedSubAgentCommissionSpecified
            oImpRequest.ConsolidatedSubAgentCommission = AddQuoteV2Request.ConsolidatedSubAgentCommission
            oImpRequest.CoverNoteBookNumber = AddQuoteV2Request.CoverNoteBookNumber
            oImpRequest.CoverNoteSheetNumberSpecified = AddQuoteV2Request.CoverNoteSheetNumberSpecified
            oImpRequest.CoverNoteSheetNumber = AddQuoteV2Request.CoverNoteSheetNumber
            oImpRequest.BusinessTypeCode = AddQuoteV2Request.BusinessTypeCode
            oImpRequest.QuoteExpiryDate = AddQuoteV2Request.QuoteExpiryDate
            oImpRequest.HandlerCode = AddQuoteV2Request.HandlerCode
            oImpRequest.Regarding = AddQuoteV2Request.Regarding
            oImpRequest.PolicyStatusCode = AddQuoteV2Request.PolicyStatusCode
            oImpRequest.InceptionDate = AddQuoteV2Request.InceptionDate
            oImpRequest.RenewalDate = AddQuoteV2Request.RenewalDate
            oImpRequest.InceptionTPI = AddQuoteV2Request.InceptionTPI
            oImpRequest.IssuedDateSpecified = AddQuoteV2Request.IssuedDateSpecified
            oImpRequest.IssuedDate = AddQuoteV2Request.IssuedDate
            oImpRequest.ProposalDateSpecified = AddQuoteV2Request.ProposalDateSpecified
            oImpRequest.ProposalDate = AddQuoteV2Request.ProposalDate
            oImpRequest.FrequencyCode = AddQuoteV2Request.FrequencyCode
            oImpRequest.RenewalMethodCode = AddQuoteV2Request.RenewalMethodCode
            oImpRequest.LapseCancelReasonCode = AddQuoteV2Request.LapseCancelReasonCode
            oImpRequest.LTUExpiryDateSpecified = AddQuoteV2Request.LTUExpiryDateSpecified
            oImpRequest.LTUExpiryDate = AddQuoteV2Request.LTUExpiryDate
            oImpRequest.StopReasonCode = AddQuoteV2Request.StopReasonCode
            oImpRequest.LapseCancelDateSpecified = AddQuoteV2Request.LapseCancelDateSpecified
            oImpRequest.LapseCancelDateSpecified = AddQuoteV2Request.LapseCancelDateSpecified
            oImpRequest.LapseCancelDate = AddQuoteV2Request.LapseCancelDate
            oImpRequest.RenewalCountSpecified = AddQuoteV2Request.RenewalCountSpecified
            oImpRequest.RenewalCount = AddQuoteV2Request.RenewalCount
            oImpRequest.ReferredAtRenewalSpecified = AddQuoteV2Request.ReferredAtRenewalSpecified
            oImpRequest.ReferredAtRenewal = AddQuoteV2Request.ReferredAtRenewal
            oImpRequest.ReferredAtMTASpecified = AddQuoteV2Request.ReferredAtMTASpecified
            oImpRequest.ReferredAtMTA = AddQuoteV2Request.ReferredAtMTA
            oImpRequest.AccountHandlerCnt = AddQuoteV2Request.AccountHandlerCnt
            oImpRequest.AccountHandlerCntSpecified = AddQuoteV2Request.AccountHandlerCntSpecified
            oImpRequest.PutOnNextInstalmentRenewal = AddQuoteV2Request.PutOnNextInstalmentRenewal
            oImpRequest.PutOnNextInstalmentRenewalSpecified = AddQuoteV2Request.PutOnNextInstalmentRenewalSpecified
            oImpRequest.RenewalDayNo = AddQuoteV2Request.RenewalDayNo
            oImpRequest.RenewalDayNoSpecified = AddQuoteV2Request.RenewalDayNoSpecified
            oImpRequest.IsMarketPlacePolicy = AddQuoteV2Request.IsMarketPlacePolicy
            oImpRequest.CoInsurancePlacement = AddQuoteV2Request.CoInsurancePlacement

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.AddQuoteV2(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                ' Retrieve the values from the implementation response structure
                oResponse.InsuranceFileKey = oImpResponse.InsuranceFileKey
                oResponse.InsuranceFileRef = oImpResponse.InsuranceFileRef
                oResponse.InsuranceFolderKey = oImpResponse.InsuranceFolderKey
                oResponse.InsuranceFileTypeCode = "QUOTE"
                oResponse.QuoteExpiryDate = oImpResponse.QuoteExpiryDate
                oResponse.QuoteTimeStamp = oImpResponse.QuoteTimeStamp
                'Wpr53 : Add has_mandatory_risk to Return in response of type Boolean
                oResponse.IsMandatoryRisk = oImpResponse.IsMandatoryRisk
                oResponse.IsMandatoryRiskSpecified = oImpResponse.IsMandatoryRiskSpecified
                oResponse.RiskKey = oImpResponse.RiskKey
                oResponse.RiskFolderKey = oImpResponse.RiskFolderKey
                oResponse.XMLDataSet = oImpResponse.XMLDataSet

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(AddQuoteV2Request))
            End Try
            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(AddQuoteV2Request))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="AddQuoteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddQuote(ByVal AddQuoteRequest As AddQuoteRequestType) As AddQuoteResponseType Implements IPurePolicyService.AddQuote

        Try

            Dim sUserName As String = AddQuoteRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMAQuot", iUserId)
            CommonFunctions.CheckSecurityToken(AddQuoteRequest.WCFSecurityToken)
            Dim oResponse As New AddQuoteResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, AddQuoteRequest.BranchCode)
            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.AddQuoteRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.AddQuoteResponseType = Nothing

            oImpRequest.AgentKeySpecified = AddQuoteRequest.AgentKeySpecified
            oImpRequest.AgentKey = AddQuoteRequest.AgentKey

            If iAgentKey <> 0 And AddQuoteRequest.AgentKeySpecified = False Then
                oImpRequest.AgentKeySpecified = True
                oImpRequest.AgentKey = iAgentKey
            End If

            oImpRequest.BranchCode = AddQuoteRequest.BranchCode
            oImpRequest.CoverEndDate = AddQuoteRequest.CoverEndDate
            oImpRequest.CoverStartDate = AddQuoteRequest.CoverStartDate
            oImpRequest.Description = AddQuoteRequest.Description
            oImpRequest.InsuredName = AddQuoteRequest.InsuredName
            oImpRequest.InsuredParties = AddQuoteRequest.InsuredParties
            oImpRequest.PartyKey = AddQuoteRequest.PartyKey
            oImpRequest.ProductCode = AddQuoteRequest.ProductCode
            oImpRequest.QuoteRef = AddQuoteRequest.QuoteRef
            oImpRequest.SubBranchCode = AddQuoteRequest.SubBranchCode
            oImpRequest.UserName = sUserName
            oImpRequest.CurrencyCode = AddQuoteRequest.CurrencyCode
            oImpRequest.AnalysisCode = AddQuoteRequest.AnalysisCode
            oImpRequest.ConsolidatedLeadAgentCommission = AddQuoteRequest.ConsolidatedLeadAgentCommission
            oImpRequest.ConsolidatedLeadAgentCommissionSpecified = AddQuoteRequest.ConsolidatedLeadAgentCommissionSpecified
            oImpRequest.ConsolidatedSubAgentCommission = AddQuoteRequest.ConsolidatedSubAgentCommission
            oImpRequest.ConsolidatedSubAgentCommissionSpecified = AddQuoteRequest.ConsolidatedSubAgentCommissionSpecified
            oImpRequest.CoverNoteBookNumber = AddQuoteRequest.CoverNoteBookNumber
            oImpRequest.CoverNoteSheetNumber = AddQuoteRequest.CoverNoteSheetNumber
            oImpRequest.CoverNoteSheetNumberSpecified = AddQuoteRequest.CoverNoteSheetNumberSpecified
            oImpRequest.AlternateReference = AddQuoteRequest.AlternativeRef
            oImpRequest.CoInsurancePlacement = AddQuoteRequest.CoInsurancePlacement
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.AddQuote(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.InsuranceFileKey = oImpResponse.InsuranceFileKey
                oResponse.InsuranceFileRef = oImpResponse.InsuranceFileRef
                oResponse.InsuranceFolderKey = oImpResponse.InsuranceFolderKey
                oResponse.QuoteExpiryDate = oImpResponse.QuoteExpiryDate
                oResponse.QuoteTimeStamp = oImpResponse.QuoteTimeStamp

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(AddQuoteRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(AddQuoteRequest))
            Return Nothing
        End Try

    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="AddRiskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddRisk(ByVal AddRiskRequest As AddRiskRequestType) As AddRiskResponseType Implements IPurePolicyService.AddRisk

        Try

            Dim sUserName As String = AddRiskRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMARsk", iUserId)
            CommonFunctions.CheckSecurityToken(AddRiskRequest.WCFSecurityToken)
            Dim oResponse As New AddRiskResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, AddRiskRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.AddRiskRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.AddRiskResponseType = Nothing

            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = AddRiskRequest.BranchCode
            oImpRequest.DataModelCode = AddRiskRequest.DataModelCode
            oImpRequest.InsuranceFileKey = AddRiskRequest.InsuranceFileKey
            oImpRequest.InsuranceFolderKey = AddRiskRequest.InsuranceFolderKey
            oImpRequest.ProductCode = AddRiskRequest.ProductCode
            oImpRequest.QuoteTimeStamp = AddRiskRequest.QuoteTimeStamp
            oImpRequest.RiskDescription = AddRiskRequest.RiskDescription
            oImpRequest.RiskTypeCode = AddRiskRequest.RiskTypeCode
            oImpRequest.RunDefaultRules = AddRiskRequest.RunDefaultRules
            oImpRequest.ScreenCode = AddRiskRequest.ScreenCode
            oImpRequest.SubBranchCode = AddRiskRequest.SubBranchCode
            oImpRequest.UserName = sUserName
            oImpRequest.XMLDataSet = SAMFunc.TransformDatasetSAMtoPB(AddRiskRequest.XMLDataSet)

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.AddRisk(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.RiskFolderKey = oImpResponse.RiskFolderKey
                oResponse.RiskKey = oImpResponse.RiskKey
                oResponse.XMLDataSet = SAMFunc.TransformDatasetPBtoSAM(oImpResponse.XMLDataSet, sCalledVia:="AddRisk", iRiskTypeID:=oImpResponse.RiskTypeId, iSourceID:=oImpResponse.BranchID)
                oResponse.QuoteTimeStamp = oImpResponse.QuoteTimeStamp

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(AddRiskRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(AddRiskRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="BindQuoteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function BindQuote(ByVal BindQuoteRequest As BindQuoteRequestType) As BindQuoteResponseType Implements IPurePolicyService.BindQuote

        Try
            Dim sUserName As String = BindQuoteRequest.LoginUserName
            Dim iUserId As Int32 = 0
            Dim iAgentKey As Int32 = 0

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMBQuote", iUserId)
            CommonFunctions.CheckSecurityToken(BindQuoteRequest.WCFSecurityToken)
            Dim oResponse As New BindQuoteResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, BindQuoteRequest.BranchCode)
            Dim oErrorBusinessRule As New SFI.SAMForInsuranceV2.SAMErrorBusinessRule
            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.BindQuoteRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.BindQuoteResponseType = Nothing


            oImpRequest.AgentKey = iAgentKey

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = BindQuoteRequest.BranchCode
            oImpRequest.InsuranceFileKey = BindQuoteRequest.InsuranceFileKey
            oImpRequest.PaymentMethod = CType(BindQuoteRequest.PaymentMethod, BaseImplementationTypes.PaymentMethodType)
            oImpRequest.PaymentMethodSpecified = BindQuoteRequest.PaymentMethodSpecified
            oImpRequest.TransactionType = BindQuoteRequest.TransactionType

            'Begin WPR36A
            oImpRequest.InstalmentType = CType([Enum].ToObject(GetType(InstalmentType), BindQuoteRequest.InstalmentType), BaseImplementationTypes.InstalmentType)
            oImpRequest.InstalmentTypeSpecified = BindQuoteRequest.InstalmentTypeSpecified
            'End WPR36A

            If BindQuoteRequest.AcceptRenewalSpecified Then
                oImpRequest.AcceptRenewal = BindQuoteRequest.AcceptRenewal
            End If
            'Deviation from spec: quote time stamp is also assigned to implementation request type
            oImpRequest.QuoteTimeStamp = BindQuoteRequest.QuoteTimeStamp


            oImpRequest.PayTrueMonthlyPolicyMTAPremiumOnRenewalSpecified = BindQuoteRequest.PayTrueMonthlyPolicyMTAPremiumOnRenewalSpecified
            If oImpRequest.PayTrueMonthlyPolicyMTAPremiumOnRenewalSpecified Then
                oImpRequest.PayTrueMonthlyPolicyMTAPremiumOnRenewal = BindQuoteRequest.PayTrueMonthlyPolicyMTAPremiumOnRenewal
            Else
                oImpRequest.PayTrueMonthlyPolicyMTAPremiumOnRenewal = False
            End If

            If BindQuoteRequest.CoverStartDateSpecified Then
                oImpRequest.CoverStartDate = BindQuoteRequest.CoverStartDate
            End If

            oImpRequest.DebitAgainst = CType(BindQuoteRequest.DebitAgainst, BaseImplementationTypes.DebitAgainstType)
            oImpRequest.DebitAgainstSpecified = BindQuoteRequest.DebitAgainstSpecified

            oImpRequest.DebitAgainstAccount = CType(BindQuoteRequest.DebitAgainstAccount, BaseImplementationTypes.DebitAgainstAccountType)
            oImpRequest.DebitAgainstAccountSpecified = BindQuoteRequest.DebitAgainstAccountSpecified

            If BindQuoteRequest.SelectedCashDeposit IsNot Nothing Then
                oImpRequest.SelectedCashDeposit = New BaseImplementationTypes.BaseSelectedCashDepositType
                oImpRequest.SelectedCashDeposit.CashDepositRef = BindQuoteRequest.SelectedCashDeposit.CashDepositRef
            End If

            oImpRequest.IsBackdatedMTA = BindQuoteRequest.IsBackdatedMTA

            With BindQuoteRequest
                If .CreditTransactions IsNot Nothing Then
                    oImpRequest.CreditTransactions = CommonFunctions.ToBaseImpBaseBindQuoteCreditTransactions(.CreditTransactions.ToList())
                End If
            End With
            ' default transaction type to new business if not specified
            If String.IsNullOrEmpty(oImpRequest.TransactionType) Then
                oImpRequest.TransactionType = "NB"
            End If

            If BindQuoteRequest.PayNowDetails IsNot Nothing Then

                oImpRequest.PayNowDetails = New BaseImplementationTypes.BaseReceiptType
                oImpRequest.PayNowDetails.Address1 = BindQuoteRequest.PayNowDetails.Address1
                oImpRequest.PayNowDetails.Address2 = BindQuoteRequest.PayNowDetails.Address2
                oImpRequest.PayNowDetails.Address3 = BindQuoteRequest.PayNowDetails.Address3
                oImpRequest.PayNowDetails.Address4 = BindQuoteRequest.PayNowDetails.Address4
                oImpRequest.PayNowDetails.Amount = BindQuoteRequest.PayNowDetails.Amount
                oImpRequest.PayNowDetails.BankAccountName = BindQuoteRequest.PayNowDetails.BankAccountName
                oImpRequest.PayNowDetails.CashListRef = BindQuoteRequest.PayNowDetails.CashListRef
                oImpRequest.PayNowDetails.CCAuthCode = BindQuoteRequest.PayNowDetails.CCAuthCode
                oImpRequest.PayNowDetails.CCCustomer = BindQuoteRequest.PayNowDetails.CCCustomer
                oImpRequest.PayNowDetails.CCExpiryDate = BindQuoteRequest.PayNowDetails.CCExpiryDate
                oImpRequest.PayNowDetails.CCIssue = BindQuoteRequest.PayNowDetails.CCIssue
                oImpRequest.PayNowDetails.CCManualAuthCode = BindQuoteRequest.PayNowDetails.CCManualAuthCode
                oImpRequest.PayNowDetails.CCName = BindQuoteRequest.PayNowDetails.CCName
                oImpRequest.PayNowDetails.CCNumber = BindQuoteRequest.PayNowDetails.CCNumber
                oImpRequest.PayNowDetails.CCPin = BindQuoteRequest.PayNowDetails.CCPin
                oImpRequest.PayNowDetails.CCStartDate = BindQuoteRequest.PayNowDetails.CCStartDate
                oImpRequest.PayNowDetails.CCTransactionCode = BindQuoteRequest.PayNowDetails.CCTransactionCode
                oImpRequest.PayNowDetails.CCTrackingNumber = BindQuoteRequest.PayNowDetails.CCTrackingNumber
                oImpRequest.PayNowDetails.CCInsuranceFileCnt = BindQuoteRequest.InsuranceFileKey


                oImpRequest.PayNowDetails.CCTypeCode = BindQuoteRequest.PayNowDetails.CCTypeCode
                oImpRequest.PayNowDetails.CCCashListItemBankCode = BindQuoteRequest.PayNowDetails.CCCashListItemBankCode
                oImpRequest.PayNowDetails.CCTransactionSlipNumber = BindQuoteRequest.PayNowDetails.CCTransactionSlipNumber
                oImpRequest.PayNowDetails.ChequeDateSpecified = BindQuoteRequest.PayNowDetails.ChequeDateSpecified
                If oImpRequest.PayNowDetails.ChequeDateSpecified Then
                    oImpRequest.PayNowDetails.ChequeDate = BindQuoteRequest.PayNowDetails.ChequeDate
                End If

                oImpRequest.PayNowDetails.ChequeName = BindQuoteRequest.PayNowDetails.ChequeName
                oImpRequest.PayNowDetails.ContactName = BindQuoteRequest.PayNowDetails.ContactName
                oImpRequest.PayNowDetails.CountryCode = BindQuoteRequest.PayNowDetails.CountryCode
                oImpRequest.PayNowDetails.CurrencyCode = BindQuoteRequest.PayNowDetails.CurrencyCode
                oImpRequest.PayNowDetails.MediaReference = BindQuoteRequest.PayNowDetails.MediaReference
                oImpRequest.PayNowDetails.MediaTypeCode = BindQuoteRequest.PayNowDetails.MediaTypeCode
                oImpRequest.PayNowDetails.MediaTypeIssuerCode = BindQuoteRequest.PayNowDetails.MediaTypeIssuerCode
                oImpRequest.PayNowDetails.OurReference = BindQuoteRequest.PayNowDetails.OurReference
                oImpRequest.PayNowDetails.PostalCode = BindQuoteRequest.PayNowDetails.PostalCode
                oImpRequest.PayNowDetails.ReceiptTypeCode = BindQuoteRequest.PayNowDetails.ReceiptTypeCode
                oImpRequest.PayNowDetails.TheirReference = BindQuoteRequest.PayNowDetails.TheirReference
                oImpRequest.PayNowDetails.TransactionDate = BindQuoteRequest.PayNowDetails.TransactionDate
                oImpRequest.PayNowDetails.SubbranchCode = BindQuoteRequest.PayNowDetails.SubbranchCode

                oImpRequest.PayNowDetails.CollectionDateSpecified = BindQuoteRequest.PayNowDetails.CollectionDateSpecified
                If oImpRequest.PayNowDetails.CollectionDateSpecified Then
                    oImpRequest.PayNowDetails.CollectionDate = BindQuoteRequest.PayNowDetails.CollectionDate
                End If
                oImpRequest.PayNowDetails.Comments = BindQuoteRequest.PayNowDetails.Comments

                oImpRequest.PayNowDetails.PartyBankKey = BindQuoteRequest.PayNowDetails.PartyBankKey

                oImpRequest.PayNowDetails.PaymentStatusCode = BindQuoteRequest.PayNowDetails.PaymentStatusCode
                oImpRequest.PayNowDetails.PaymentTypeCode = BindQuoteRequest.PayNowDetails.PaymentTypeCode
                oImpRequest.PayNowDetails.AllocationStatusCode = BindQuoteRequest.PayNowDetails.AllocationStatusCode
                If (BindQuoteRequest.PayNowDetails.Bank IsNot Nothing) Then
                    oImpRequest.PayNowDetails.Bank = CommonFunctions.ToBaseImpBaseBankReceiptType(BindQuoteRequest.PayNowDetails.Bank)
                End If

            End If

            If BindQuoteRequest.PayNowPaymentDetails IsNot Nothing AndAlso Not BindQuoteRequest.NoTrans Then

                oImpRequest.PayNowPaymentDetails = New BaseImplementationTypes.BasePaymentType
                oImpRequest.PayNowPaymentDetails.InsuranceFileRef = BindQuoteRequest.PayNowPaymentDetails.InsuranceFileRef
                oImpRequest.PayNowPaymentDetails.CashListKey = BindQuoteRequest.PayNowPaymentDetails.CashListKey
                oImpRequest.PayNowPaymentDetails.CashListItemKey = BindQuoteRequest.PayNowPaymentDetails.CashListItemKey
                oImpRequest.PayNowPaymentDetails.TransDetailKey = BindQuoteRequest.PayNowPaymentDetails.TransDetailKey
                oImpRequest.PayNowPaymentDetails.PaymentAccountID = BindQuoteRequest.PayNowPaymentDetails.PaymentAccountID
                oImpRequest.PayNowPaymentDetails.PaymentTypeCode = BindQuoteRequest.PayNowPaymentDetails.PaymentTypeCode
                oImpRequest.PayNowPaymentDetails.MediaTypeCode = BindQuoteRequest.PayNowPaymentDetails.MediaTypeCode
                oImpRequest.PayNowPaymentDetails.MediaReference = BindQuoteRequest.PayNowPaymentDetails.MediaReference
                oImpRequest.PayNowPaymentDetails.OurReference = BindQuoteRequest.PayNowPaymentDetails.OurReference
                oImpRequest.PayNowPaymentDetails.TheirReference = BindQuoteRequest.PayNowPaymentDetails.TheirReference
                oImpRequest.PayNowPaymentDetails.SubbranchCode = BindQuoteRequest.PayNowPaymentDetails.SubbranchCode

            End If

            If BindQuoteRequest.PayNowPaymentDetails IsNot Nothing AndAlso BindQuoteRequest.NoTrans Then
                oImpRequest.PayNowPaymentDetails = New BaseImplementationTypes.BasePaymentType
            End If

            If BindQuoteRequest.PayNowPaymentDetails IsNot Nothing AndAlso BindQuoteRequest.NoTrans Then
                oImpRequest.PayNowPaymentDetails = New BaseImplementationTypes.BasePaymentType
            End If

            oImpRequest.SelectedInstalmentQuoteSpecified = False

            If BindQuoteRequest.SelectedInstalmentQuote IsNot Nothing Then
                oImpRequest.SelectedInstalmentQuoteSpecified = True
                oImpRequest.BankAccountName = BindQuoteRequest.SelectedInstalmentQuote.BankAccountName
                oImpRequest.BankAccountNo = BindQuoteRequest.SelectedInstalmentQuote.BankAccountNo
                oImpRequest.BankSortCode = BindQuoteRequest.SelectedInstalmentQuote.BankSortCode
                oImpRequest.BIC = BindQuoteRequest.SelectedInstalmentQuote.BIC
                oImpRequest.IBAN = BindQuoteRequest.SelectedInstalmentQuote.IBAN
                oImpRequest.BankAddress = New BaseImplementationTypes.BaseAddressType
                If BindQuoteRequest.SelectedInstalmentQuote IsNot Nothing Then
                    If BindQuoteRequest.SelectedInstalmentQuote.BankAddress IsNot Nothing Then
                        oImpRequest.BankAddress.AddressLine1 = BindQuoteRequest.SelectedInstalmentQuote.BankAddress.AddressLine1
                        oImpRequest.BankAddress.AddressLine2 = BindQuoteRequest.SelectedInstalmentQuote.BankAddress.AddressLine2
                        oImpRequest.BankAddress.AddressLine3 = BindQuoteRequest.SelectedInstalmentQuote.BankAddress.AddressLine3
                        oImpRequest.BankAddress.AddressLine4 = BindQuoteRequest.SelectedInstalmentQuote.BankAddress.AddressLine4
                        oImpRequest.BankAddress.AddressTypeCode = CType(BindQuoteRequest.SelectedInstalmentQuote.BankAddress.AddressTypeCode, BaseImplementationTypes.AddressTypeType)
                        oImpRequest.BankAddress.CountryCode = BindQuoteRequest.SelectedInstalmentQuote.BankAddress.CountryCode
                        oImpRequest.BankAddress.PostCode = BindQuoteRequest.SelectedInstalmentQuote.BankAddress.PostCode
                    End If
                    oImpRequest.BankAreaCode = BindQuoteRequest.SelectedInstalmentQuote.BankAreaCode
                    oImpRequest.BankBranch = BindQuoteRequest.SelectedInstalmentQuote.BankBranch
                    oImpRequest.BankExtn = BindQuoteRequest.SelectedInstalmentQuote.BankExtn
                    oImpRequest.BankFax = BindQuoteRequest.SelectedInstalmentQuote.BankFax
                    oImpRequest.BankFaxCode = BindQuoteRequest.SelectedInstalmentQuote.BankFaxCode
                    oImpRequest.BankName = BindQuoteRequest.SelectedInstalmentQuote.BankName
                    oImpRequest.BankPhone = BindQuoteRequest.SelectedInstalmentQuote.BankPhone
                    oImpRequest.SelectedSchemeNo = BindQuoteRequest.SelectedInstalmentQuote.SelectedSchemeNo
                    oImpRequest.SelectedSchemeVersion = BindQuoteRequest.SelectedInstalmentQuote.SelectedSchemeVersion
                    oImpRequest.QuoteDate = BindQuoteRequest.SelectedInstalmentQuote.QuoteDate.Date
                    oImpRequest.StartDate = BindQuoteRequest.SelectedInstalmentQuote.StartDate.Date
                    oImpRequest.EndDate = BindQuoteRequest.SelectedInstalmentQuote.EndDate.Date
                    oImpRequest.PreferredDate = BindQuoteRequest.SelectedInstalmentQuote.PreferredDate.Date
                    oImpRequest.MonthDay = BindQuoteRequest.SelectedInstalmentQuote.MonthDay
                    oImpRequest.WeekDay = BindQuoteRequest.SelectedInstalmentQuote.WeekDay
                    oImpRequest.AmountToFinance = BindQuoteRequest.SelectedInstalmentQuote.AmountToFinance
                    oImpRequest.PaymentProtection = BindQuoteRequest.SelectedInstalmentQuote.PaymentProtection
                    oImpRequest.OverrideRate = BindQuoteRequest.SelectedInstalmentQuote.OverrideRate
                    oImpRequest.OverrideInterestRate = BindQuoteRequest.SelectedInstalmentQuote.OverrideInterestRate
                    oImpRequest.AmountPaid = BindQuoteRequest.SelectedInstalmentQuote.AmountPaid
                    oImpRequest.PFRF_ID = BindQuoteRequest.SelectedInstalmentQuote.PFRF_ID
                    oImpRequest.IsUseTransactionCurrency = BindQuoteRequest.SelectedInstalmentQuote.IsUseTransactionCurrency
                    oImpRequest.PartyBankKey = BindQuoteRequest.SelectedInstalmentQuote.PartyBankKey


                    ' if the payment method was credit card then also retrieve any credit card details 
                    ' passed in the request
                    If oImpRequest.PaymentMethodSpecified AndAlso
                                BindQuoteRequest.SelectedInstalmentQuote.CreditCard IsNot Nothing Then

                        oImpRequest.CreditCard = New BaseImplementationTypes.BaseCreditCardType

                        oImpRequest.CreditCard.ExpiryDate = BindQuoteRequest.SelectedInstalmentQuote.CreditCard.ExpiryDate
                        oImpRequest.CreditCard.Issue = BindQuoteRequest.SelectedInstalmentQuote.CreditCard.Issue
                        oImpRequest.CreditCard.NameOnCreditCard = BindQuoteRequest.SelectedInstalmentQuote.CreditCard.NameOnCreditCard
                        oImpRequest.CreditCard.Number = BindQuoteRequest.SelectedInstalmentQuote.CreditCard.Number
                        oImpRequest.CreditCard.Pin = BindQuoteRequest.SelectedInstalmentQuote.CreditCard.Pin
                        oImpRequest.CreditCard.StartDate = BindQuoteRequest.SelectedInstalmentQuote.CreditCard.StartDate
                        oImpRequest.CreditCard.TypeCode = BindQuoteRequest.SelectedInstalmentQuote.CreditCard.TypeCode
                        oImpRequest.CreditCard.AuthCode = BindQuoteRequest.SelectedInstalmentQuote.CreditCard.AuthCode
                        oImpRequest.CreditCard.TrackingNumber = BindQuoteRequest.SelectedInstalmentQuote.CreditCard.TrackingNumber
                        oImpRequest.CreditCard.AccountType = BindQuoteRequest.SelectedInstalmentQuote.CreditCard.AccountType
                        oImpRequest.CreditCard.PartyBankKey = BindQuoteRequest.SelectedInstalmentQuote.CreditCard.PartyBankKey
                        oImpRequest.CreditCard.VIAPaymentHub = BindQuoteRequest.SelectedInstalmentQuote.CreditCard.VIAPaymentHub

                        ' retrieve the credit card cardholder details if provided
                        oImpRequest.CreditCard.CardHolder = New BaseImplementationTypes.BaseCreditCardTypeCardHolder
                        If BindQuoteRequest.SelectedInstalmentQuote.CreditCard.CardHolder IsNot Nothing Then
                            oImpRequest.CreditCard.CardHolder.Name = BindQuoteRequest.SelectedInstalmentQuote.CreditCard.CardHolder.Name
                            oImpRequest.CreditCard.CardHolder.AddressLine1 = BindQuoteRequest.SelectedInstalmentQuote.CreditCard.CardHolder.AddressLine1
                            oImpRequest.CreditCard.CardHolder.AddressLine2 = BindQuoteRequest.SelectedInstalmentQuote.CreditCard.CardHolder.AddressLine2
                            oImpRequest.CreditCard.CardHolder.AddressLine3 = BindQuoteRequest.SelectedInstalmentQuote.CreditCard.CardHolder.AddressLine3
                            oImpRequest.CreditCard.CardHolder.AddressLine4 = BindQuoteRequest.SelectedInstalmentQuote.CreditCard.CardHolder.AddressLine4
                            oImpRequest.CreditCard.CardHolder.CountryCode = BindQuoteRequest.SelectedInstalmentQuote.CreditCard.CardHolder.CountryCode
                            oImpRequest.CreditCard.CardHolder.PostCode = BindQuoteRequest.SelectedInstalmentQuote.CreditCard.CardHolder.PostCode
                        End If

                    End If

                End If
            End If

            If BindQuoteRequest.PaymentMethodSpecified AndAlso
                       BindQuoteRequest.PaymentMethod =
                       BaseImplementationTypes.PaymentMethodType.BankGuarantee Then
                If BindQuoteRequest.BankGuaranteeDetails IsNot Nothing Then
                    oImpRequest.BankGuaranteeDetails = New BaseImplementationTypes.BaseBankGuaranteePaymentType
                    oImpRequest.BankGuaranteeDetails.BGKey = BindQuoteRequest.BankGuaranteeDetails.BGKey
                End If
            End If

            oImpRequest.WritePolicy = BindQuoteRequest.WritePolicy
            oImpRequest.OverriddenPolicyNumber = BindQuoteRequest.OverriddenPolicyNumber
            oImpRequest.PayNegativePremiumMTABalanceSpecified = BindQuoteRequest.PayNegativePremiumMTABalanceSpecified
            oImpRequest.PayNegativePremiumMTABalance = BindQuoteRequest.PayNegativePremiumMTABalance
            oImpRequest.NoTrans = BindQuoteRequest.NoTrans
            oImpRequest.IsCallingFromSAM = True
            If BindQuoteRequest.SelectedInstalmentQuote IsNot Nothing Then
                oImpRequest.InstDepositAmount = BindQuoteRequest.SelectedInstalmentQuote.InstDepositAmount
            End If
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.BindQuote(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Return details
                oResponse.Policy = New BaseTransactResponseTypePolicy

                If oImpResponse.Policy IsNot Nothing Then

                    oResponse.Policy.CommissionAmount = oImpResponse.Policy.CommissionAmount
                    oResponse.Policy.PolicyRef = oImpResponse.Policy.PolicyRef
                    oResponse.Policy.PremiumDueGross = oImpResponse.Policy.PremiumDueGross
                    oResponse.Policy.PremiumDueNet = oImpResponse.Policy.PremiumDueNet
                    oResponse.Policy.PremiumDueTax = oImpResponse.Policy.PremiumDueTax
                    oResponse.Policy.TotalAnnualTax = oImpResponse.Policy.TotalAnnualTax
                    oResponse.Policy.DocumentComment = oImpResponse.Policy.DocumentComment
                    oResponse.Policy.AutoGeneratedPlanRef = oImpResponse.Policy.AutoGeneratedPlanRef
                    oResponse.Policy.DepositTransDetailID = oImpResponse.Policy.DepositTransDetailID
                    'Note:For MTA and MTC, Multiple policy details will be returned

                    If oImpResponse.Policy.MultiplePolicies IsNot Nothing Then
                        oResponse.Policy.MultiplePolicies = oImpResponse.Policy.MultiplePolicies.ToList().ConvertAll(
                            New Converter(Of BaseImplementationTypes.BaseTransactResponseTypePolicyMultiplePolicies, BaseTransactResponseTypePolicyMultiplePolicies) _
                                                            (AddressOf CommonFunctions.ToServiceMultiplePoliciesList))

                    End If

                    oResponse.Policy.PolicyLevelTaxesAndFees = CommonFunctions.ToServiceTaxesAndFeesTypeList(oImpResponse.Policy.PolicyLevelTaxesAndFees)

                End If
                If oImpResponse.Warnings IsNot Nothing AndAlso oImpResponse.Warnings.Count > 0 Then
                    oResponse.Warnings = oImpResponse.Warnings.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseGeneralWarningResponseType, BaseGeneralWarningResponseType)(AddressOf CommonFunctions.ToServiceWarningTypeList))
                End If


            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(BindQuoteRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(BindQuoteRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oCopyRiskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CopyRisk(ByVal oCopyRiskRequest As CopyRiskRequestType) As CopyRiskResponseType Implements IPureClaimService.CopyRisk

        Try
            'Assign appropriate key


            Dim sUserName As String = oCopyRiskRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMCRsk", iUserId)
            CommonFunctions.CheckSecurityToken(oCopyRiskRequest.WCFSecurityToken)
            Dim oResponse As New CopyRiskResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oCopyRiskRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.CopyRiskRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.CopyRiskResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oCopyRiskRequest.BranchCode
            oImpRequest.CopyType = CType([Enum].ToObject(GetType(CopyRiskType), oCopyRiskRequest.CopyType), BaseImplementationTypes.CopyRiskType)
            oImpRequest.InsuranceFileKey = oCopyRiskRequest.InsuranceFileKey
            oImpRequest.InsuranceFolderKey = oCopyRiskRequest.InsuranceFolderKey
            oImpRequest.RiskKey = oCopyRiskRequest.RiskKey
            oImpRequest.RiskNumber = oCopyRiskRequest.RiskNumber

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.CopyRisk(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                oResponse.QuoteTimeStamp = oImpResponse.QuoteTimeStamp
                oResponse.RiskKey = oImpResponse.RiskKey

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oCopyRiskRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oCopyRiskRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oCopyQuoteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CopyQuote(ByVal oCopyQuoteRequest As CopyQuoteRequestType) As CopyQuoteResponseType Implements IPurePolicyService.CopyQuote

        Try

            Dim sUserName As String = oCopyQuoteRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMCOPYQTE", iUserId)
            CommonFunctions.CheckSecurityToken(oCopyQuoteRequest.WCFSecurityToken)
            Dim oResponse As New CopyQuoteResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oCopyQuoteRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.CopyQuoteRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.CopyQuoteResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oCopyQuoteRequest.BranchCode
            oImpRequest.InsuranceFileKey = oCopyQuoteRequest.InsuranceFileKey
            oImpRequest.CloneLivePolicy = oCopyQuoteRequest.CloneLivePolicy

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.CopyQuote(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.InsuranceRef = oImpResponse.InsuranceRef
                oResponse.InsuranceFileKey = oImpResponse.InsuranceFileKey
                oResponse.InsuranceFolderKey = oImpResponse.InsuranceFolderKey
                oResponse.BaseInsuranceFolderKey = oImpResponse.BaseInsuranceFolderKey
                oResponse.QuoteVersion = oImpResponse.QuoteVersion
                oResponse.QuoteStatusKey = CType([Enum].ToObject(GetType(BaseImplementationTypes.QuoteStatusType), oImpResponse.QuoteStatusKey), QuoteStatusType)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oCopyQuoteRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oCopyQuoteRequest))
            Return Nothing
        End Try

    End Function
    Public Function CloneQuoteFromLivePolicy(ByVal oCopyQuoteRequest As CopyQuoteRequestType) As CopyQuoteResponseType Implements IPurePolicyService.CloneQuoteFromLivePolicy
        Try

            Dim sUserName As String = oCopyQuoteRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMCOPYQTE", iUserId)
            CommonFunctions.CheckSecurityToken(oCopyQuoteRequest.WCFSecurityToken)
            Dim oResponse As New CopyQuoteResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oCopyQuoteRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.CopyQuoteRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.CopyQuoteResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oCopyQuoteRequest.BranchCode
            oImpRequest.InsuranceFileKey = oCopyQuoteRequest.InsuranceFileKey
            oImpRequest.CloneLivePolicy = oCopyQuoteRequest.CloneLivePolicy

            Try
                ' Call the implementation method
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.InsuranceRef = oImpResponse.InsuranceRef
                oResponse.InsuranceFileKey = oImpResponse.InsuranceFileKey
                oResponse.InsuranceFolderKey = oImpResponse.InsuranceFolderKey
                oResponse.BaseInsuranceFolderKey = oImpResponse.BaseInsuranceFolderKey
                oResponse.QuoteVersion = oImpResponse.QuoteVersion
                oResponse.QuoteStatusKey = CType([Enum].ToObject(GetType(BaseImplementationTypes.QuoteStatusType), oImpResponse.QuoteStatusKey), QuoteStatusType)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oCopyQuoteRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oCopyQuoteRequest))
            Return Nothing
        End Try

    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oDeletePolicyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeletePolicy(ByVal oDeletePolicyRequest As DeletePolicyRequestType) As DeletePolicyResponseType Implements IPurePolicyService.DeletePolicy

        Try

            Dim sUserName As String = oDeletePolicyRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMDELPOL", iUserId)
            CommonFunctions.CheckSecurityToken(oDeletePolicyRequest.WCFSecurityToken)
            Dim oResponse As New DeletePolicyResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oDeletePolicyRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.DeletePolicyRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.DeletePolicyResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oDeletePolicyRequest.BranchCode
            oImpRequest.InsuranceFileKey = oDeletePolicyRequest.InsuranceFileKey

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.DeletePolicy(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oDeletePolicyRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oDeletePolicyRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DeleteRenewalRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeleteRenewal(ByVal DeleteRenewalRequest As DeleteRenewalRequestType) _
     As DeleteRenewalResponseType Implements IPurePolicyService.DeleteRenewal

        Try

            Dim sUserName As String = DeleteRenewalRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMPRDEL", iUserId)
            CommonFunctions.CheckSecurityToken(DeleteRenewalRequest.WCFSecurityToken)
            Dim oResponse As New DeleteRenewalResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, DeleteRenewalRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.DeleteRenewalRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.DeleteRenewalResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = DeleteRenewalRequest.BranchCode
            oImpRequest.InsuranceFileKey = DeleteRenewalRequest.InsuranceFileKey
            oImpRequest.QuoteTimeStamp = DeleteRenewalRequest.QuoteTimeStamp
            oImpRequest.CreatedById = iUserId
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.DeleteRenewal(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(DeleteRenewalRequest))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(DeleteRenewalRequest))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DeleteRiskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeleteRisk(ByVal DeleteRiskRequest As DeleteRiskRequestType) As DeleteRiskResponseType Implements IPureClaimService.DeleteRisk

        Try

            Dim sUserName As String = DeleteRiskRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMDRsk", iUserId)
            CommonFunctions.CheckSecurityToken(DeleteRiskRequest.WCFSecurityToken)
            Dim oResponse As New DeleteRiskResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, DeleteRiskRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.DeleteRiskRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.DeleteRiskResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = DeleteRiskRequest.BranchCode
            oImpRequest.QuoteTimeStamp = DeleteRiskRequest.QuoteTimeStamp
            oImpRequest.InsuranceFolderKey = DeleteRiskRequest.InsuranceFolderKey
            oImpRequest.InsuranceFileKey = DeleteRiskRequest.InsuranceFileKey
            oImpRequest.RiskKey = DeleteRiskRequest.RiskKey
            oImpRequest.UserName = sUserName
            oImpRequest.TransactionType = CType([Enum].ToObject(GetType(TransactionType), DeleteRiskRequest.TransactionType), BaseImplementationTypes.TransactionType)

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.DeleteRisk(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.QuoteTimeStamp = oImpResponse.QuoteTimeStamp

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(DeleteRiskRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(DeleteRiskRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oFindPolicyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindPolicy(ByVal oFindPolicyRequest As FindPolicyRequestType) As FindPolicyResponseType Implements IPurePolicyService.FindPolicy

        Try

            Dim sUserName As String = oFindPolicyRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMFindPol", iUserId)
            CommonFunctions.CheckSecurityToken(oFindPolicyRequest.WCFSecurityToken)

            Dim oResponse As New FindPolicyResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oFindPolicyRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.FindPolicyRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.FindPolicyResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oFindPolicyRequest.BranchCode
            oImpRequest.InsuranceRef = oFindPolicyRequest.InsuranceRef
            oImpRequest.ClientShortName = oFindPolicyRequest.ClientShortName
            oImpRequest.QuoteType = oFindPolicyRequest.QuoteType.ToString
            oImpRequest.QuoteTypeSpecified = oFindPolicyRequest.QuoteTypeSpecified
            oImpRequest.RiskIndex = oFindPolicyRequest.RiskIndex
            oImpRequest.ShowLapsedOnly = oFindPolicyRequest.ShowLapsedOnly
            oImpRequest.ShowLapsedOnlySpecified = oFindPolicyRequest.ShowLapsedOnlySpecified
            oImpRequest.ShowCancelledForEvents = oFindPolicyRequest.ShowCancelledForEvents
            oImpRequest.RetrieveAssociates = oFindPolicyRequest.RetrieveAssociates
            oImpRequest.AgentKey = iAgentKey

            oImpRequest.MaxRowsToFetchSpecified = oFindPolicyRequest.MaxRowsToFetchSpecified
            If oFindPolicyRequest.MaxRowsToFetchSpecified Then
                oImpRequest.MaxRowsToFetch = oFindPolicyRequest.MaxRowsToFetch
            Else
                oImpRequest.MaxRowsToFetch = -1
            End If
            oImpRequest.WCFSecurityToken = If(oFindPolicyRequest.WCFSecurityToken.Length > 0, oFindPolicyRequest.WCFSecurityToken, "WCFSecurityToken")

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.FindPolicy(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                'oResponse.InsuranceFileDetails = SAMFunc.GetDeserializedValues(Of List(Of BaseFindPolicyResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseFindPolicyResponseTypeInsuranceFileDetails", sConvertToTypeName:="BaseFindPolicyResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.InsuranceFileDetails = DataTabletoList_FindPolicy(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oFindPolicyRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oFindPolicyRequest))
            Return Nothing
        End Try
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="GenerateInviteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GenerateInvite(ByVal GenerateInviteRequest As GenerateInviteRequestType) As GenerateInviteResponseType Implements IPurePolicyService.GenerateInvite
        Try

            Dim sUserName As String = GenerateInviteRequest.LoginUserName
            Dim agentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, agentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGenDoc", iUserId)
            CommonFunctions.CheckSecurityToken(GenerateInviteRequest.WCFSecurityToken)

            Dim response As New GenerateInviteResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GenerateInviteRequest.BranchCode)
            ' Implementation structures
            Dim impRequest As New SAMForInsuranceV2ImplementationTypes.GenerateInviteRequestType
            Dim impResponse As SAMForInsuranceV2ImplementationTypes.GenerateInviteResponseType = Nothing
            ' Pass the values to the implementation request structure
            impRequest.AgentKey = agentKey
            impRequest.BranchCode = GenerateInviteRequest.BranchCode
            impRequest.InsuranceFileKey = GenerateInviteRequest.InsuranceFileKey
            impRequest.OutputAsHTML = GenerateInviteRequest.OutputAsHTML
            impRequest.OutputAsPDF = GenerateInviteRequest.OutputAsPDF
            impRequest.QuoteTimeStamp = GenerateInviteRequest.QuoteTimeStamp
            impRequest.SpoolDocumentOnly = GenerateInviteRequest.SpoolDocumentOnly
            impRequest.SpoolDocumentOnlySpecified = GenerateInviteRequest.SpoolDocumentOnlySpecified
            impRequest.UserId = iUserId
            Try
                ' Call the implementation method
                impResponse = oBusiness.GenerateInvite(impRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(impResponse.STSError)
                ' Retrieve the values from the implementation response structure into Actual Response
                If Not impResponse Is Nothing Then
                    response.MergedFilePath = impResponse.MergedFilePath
                    response.QuoteTimeStamp = impResponse.QuoteTimeStamp
                    response.SpooledZipFile = impResponse.SpooledZipFile
                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(impResponse, response, ex, CommonFunctions.CreateDictionary(GenerateInviteRequest))
            End Try
            Return response
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GenerateInviteRequest))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' This   web service  method  for GetAgentDetailsForPolicy
    '''<param name="oGetAgentDetailsForPolicyRequest" type="GetAgentDetailsForPolicyRequestType"></param>   
    '''<returns>GetAgentDetailsForPolicyResponseType</returns>
    '''</summary>
    '''<remarks></remarks> 
    Public Function GetAgentDetailsForPolicy(ByVal oGetAgentDetailsForPolicyRequest As GetAgentDetailsForPolicyRequestType) As GetAgentDetailsForPolicyResponseType Implements IPurePolicyService.GetAgentDetailsForPolicy
        Try

            Dim sUserName As String = oGetAgentDetailsForPolicyRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGClmDet", iUserId)
            CommonFunctions.CheckSecurityToken(oGetAgentDetailsForPolicyRequest.WCFSecurityToken)
            Dim oResponse As New GetAgentDetailsForPolicyResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetAgentDetailsForPolicyRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetAgentDetailsForPolicyRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetAgentDetailsForPolicyResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetAgentDetailsForPolicyRequest.BranchCode
            oImpRequest.InsuranceFileKey = oGetAgentDetailsForPolicyRequest.InsuranceFileKey

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetAgentDetailsForPolicy(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.Address1 = oImpResponse.Address1
                oResponse.Address2 = oImpResponse.Address2
                oResponse.Address3 = oImpResponse.Address3
                oResponse.Address4 = oImpResponse.Address4
                oResponse.AddressKey = oImpResponse.AddressKey
                oResponse.AddressUsageTypeKey = oImpResponse.AddressUsageTypeKey
                oResponse.AreaCode = oImpResponse.AreaCode
                oResponse.Code = oImpResponse.Code
                oResponse.ContactTypeKey = oImpResponse.ContactTypeKey
                oResponse.CountryKey = oImpResponse.CountryKey
                oResponse.Description = oImpResponse.Description
                oResponse.Extension = oImpResponse.Extension
                oResponse.Name = oImpResponse.Name
                oResponse.Number = oImpResponse.Number
                oResponse.PostalCode = oImpResponse.PostalCode
                oResponse.Shortname = oImpResponse.Shortname
                If oImpResponse.Addresses IsNot Nothing AndAlso oImpResponse.Addresses.Count > 0 Then

                    oResponse.Addresses = oImpResponse.Addresses.ToList().ConvertAll(
                                    New Converter(Of BaseImplementationTypes.BaseAddressType, BaseAddressType)(AddressOf CommonFunctions.ToServiceBaseAddressWithContactsType))

                End If
                ' get the contacts
                If oImpResponse.Contacts IsNot Nothing AndAlso oImpResponse.Contacts.Count > 0 Then

                    oResponse.Contacts = oImpResponse.Contacts.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseContactType, BaseContactType)(AddressOf CommonFunctions.ToServiceContactType))

                End If
            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try
            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetAgentDetailsForPolicyRequest))
            Return Nothing
        End Try
    End Function


    ''' <summary>
    ''' GetAllPolicyVersions
    ''' </summary>
    ''' <param name="GetAllPolicyVersionsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAllPolicyVersions(ByVal GetAllPolicyVersionsRequest As GetAllPolicyVersionsRequestType) As GetAllPolicyVersionsResponseType Implements IPurePolicyService.GetAllPolicyVersions, IPureClaimService.GetAllPolicyVersions
        Try

            Dim sUserName As String = GetAllPolicyVersionsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGAPolVs", iUserId)
            CommonFunctions.CheckSecurityToken(GetAllPolicyVersionsRequest.WCFSecurityToken)

            Dim oResponse As New GetAllPolicyVersionsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetAllPolicyVersionsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetAllPolicyVersionsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetAllPolicyVersionsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = GetAllPolicyVersionsRequest.BranchCode
            oImpRequest.InsuranceFolderKey = GetAllPolicyVersionsRequest.InsuranceFolderKey
            oImpRequest.UserName = sUserName
            If GetAllPolicyVersionsRequest.RetrieveAssociates Then
                oImpRequest.RetrieveAssociates = GetAllPolicyVersionsRequest.RetrieveAssociates
            Else
                oImpRequest.RetrieveAssociates = False
            End If
            oImpRequest.WCFSecurityToken = If(GetAllPolicyVersionsRequest.WCFSecurityToken.Length > 0, GetAllPolicyVersionsRequest.WCFSecurityToken, "WCFSecurityToken")


            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetAllPolicyVersions(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    'oResponse.Policies = SAMFunc.GetDeserializedValues(Of List(Of BaseGetAllPolicyVersionsResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetAllPolicyVersionsResponseTypePolicies", sConvertToTypeName:="BaseGetAllPolicyVersionsResponseTypeRow")
                    oResponse.Policies = DataTabletoList_GetAllPolicyVersions(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetAllPolicyVersionsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetAllPolicyVersionsRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' To get backdated versions for a policy during backdated MTA
    ''' </summary>
    ''' <param name="oGetBackdatedMTAPolicyVersionsRequestType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetBackdatedMTAPolicyVersions(ByVal oGetBackdatedMTAPolicyVersionsRequestType As GetHeaderAndSummariesByKeyRequestType) As AddBackDatedMTAQuoteResponseType Implements IPurePolicyService.GetBackdatedMTAPolicyVersions

        Try

            Dim sUserName As String = oGetBackdatedMTAPolicyVersionsRequestType.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMAMtaQte", iUserId)
            CommonFunctions.CheckSecurityToken(oGetBackdatedMTAPolicyVersionsRequestType.WCFSecurityToken)
            Dim oResponse As New AddBackDatedMTAQuoteResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetBackdatedMTAPolicyVersionsRequestType.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetHeaderAndSummariesByKeyRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.AddBackDatedMTAQuoteResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetBackdatedMTAPolicyVersionsRequestType.BranchCode
            oImpRequest.InsuranceFileKey = oGetBackdatedMTAPolicyVersionsRequestType.InsuranceFileKey

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetBackdatedMTAPolicyVersions(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                'oResponse.BackdatedTransactions = SAMFunc.GetDeserializedValues(Of List(Of BaseAddBackDatedMTAQuoteResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseAddBackDatedMTAQuoteResponseTypeBackdatedTransactions", sConvertToTypeName:="BaseAddBackDatedMTAQuoteResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.BackdatedTransactions = DataTabletoList_AddBackDatedMTAQuote(oImpResponse.ResultData.Tables(0))
                End If
                oResponse.FailureReason = oImpResponse.FailureReason
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetBackdatedMTAPolicyVersionsRequestType))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetBackdatedMTAPolicyVersionsRequestType))
            Return Nothing
        End Try

    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetCashDepositsForPolicyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCashDepositsForPolicy(ByVal oGetCashDepositsForPolicyRequest As GetCashDepositsForPolicyRequestType) As GetCashDepositsForPolicyResponseType Implements IPurePolicyService.GetCashDepositsForPolicy

        Try

            Dim sUserName As String = oGetCashDepositsForPolicyRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGETPCDS", iUserId)
            CommonFunctions.CheckSecurityToken(oGetCashDepositsForPolicyRequest.WCFSecurityToken)
            Dim oResponse As New GetCashDepositsForPolicyResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetCashDepositsForPolicyRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetCashDepositsForPolicyRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetCashDepositsForPolicyResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetCashDepositsForPolicyRequest.BranchCode
            oImpRequest.InsuranceFileKey = oGetCashDepositsForPolicyRequest.InsuranceFileKey
            If (oGetCashDepositsForPolicyRequest.GetCDsOf = CDPartyType.Agent) Then
                oImpRequest.GetCDsOf = BaseImplementationTypes.CDPartyType.Agent
            ElseIf (oGetCashDepositsForPolicyRequest.GetCDsOf = CDPartyType.Client) Then
                oImpRequest.GetCDsOf = BaseImplementationTypes.CDPartyType.Client
            End If
            oImpRequest.WCFSecurityToken = If(oGetCashDepositsForPolicyRequest.WCFSecurityToken.Length > 0, oGetCashDepositsForPolicyRequest.WCFSecurityToken, "WCFSecurityToken")

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetCashDepositsForPolicy(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.CashDepositPolicies = SAMFunc.GetDeserializedValues(Of List(Of BaseGetCashDepositsForPolicyResponseTypeCashDepositPoliciesRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetCashDepositsForPolicyResponseTypeCashDepositPolicies", sConvertToTypeName:="BaseGetCashDepositsForPolicyResponseTypeCashDepositPoliciesRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.CashDepositPolicies = DataTabletoList_GetCashDepositsForPolicyCashDepositPolicies(oImpResponse.ResultData.Tables(0))
                End If

                If oImpResponse.Warnings IsNot Nothing Then
                    If IsArray(oImpResponse.Warnings) Then
                        oResponse.Warnings = oImpResponse.Warnings.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseGeneralWarningResponseType, BaseGeneralWarningResponseType)(AddressOf CommonFunctions.ToServiceWarningTypeList))
                    End If
                End If

                oResponse.AgentType = oImpResponse.AgentType

                oResponse.CDTimeStamp = oImpResponse.CDTimeStamp
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetCashDepositsForPolicyRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetCashDepositsForPolicyRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' This is webservice method for  GetHeaderAndAgentCommissionByKey
    '''<param name="GetHeaderAndAgentCommissionByKeyRequest" type="GetHeaderAndAgentCommissionByKeyRequestType"></param>   
    '''</summary>
    '''<remarks></remarks>  

    Public Function GetHeaderAndAgentCommissionByKey(ByVal GetHeaderAndAgentCommissionByKeyRequest As GetHeaderAndAgentCommissionByKeyRequestType) As GetHeaderAndAgentCommissionByKeyResponseType Implements IPurePolicyService.GetHeaderAndAgentCommissionByKey

        Try

            Dim sUserName As String = GetHeaderAndAgentCommissionByKeyRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHRKEY", iUserId)
            CommonFunctions.CheckSecurityToken(GetHeaderAndAgentCommissionByKeyRequest.WCFSecurityToken)
            Dim oResponse As New GetHeaderAndAgentCommissionByKeyResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetHeaderAndAgentCommissionByKeyRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetHeaderAndAgentCommissionByKeyRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetHeaderAndAgentCommissionByKeyResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetHeaderAndAgentCommissionByKeyRequest.BranchCode
            oImpRequest.InsuranceFileKey = GetHeaderAndAgentCommissionByKeyRequest.InsuranceFileKey
            oImpRequest.WCFSecurityToken = If(GetHeaderAndAgentCommissionByKeyRequest.WCFSecurityToken.Length > 0, GetHeaderAndAgentCommissionByKeyRequest.WCFSecurityToken, "WCFSecurityToken")

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetHeaderAndAgentCommissionByKey(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.InsuranceFileKey = oImpRequest.InsuranceFileKey
                oResponse.InsuranceFileRef = oImpResponse.InsuranceFileRef
                oResponse.ClientCode = oImpResponse.ClientCode
                oResponse.Agent = oImpResponse.Agent
                oResponse.InceptionDate = oImpResponse.InceptionDate
                oResponse.CoverStartDate = oImpResponse.CoverStartDate
                oResponse.ExpiryDate = oImpResponse.ExpiryDate
                oResponse.Currency = oImpResponse.Currency
                oResponse.TotalCommissionLeadAgent = oImpResponse.TotalCommissionLeadAgent
                oResponse.TotalTaxLeadAgent = oImpResponse.TotalTaxLeadAgent
                oResponse.TotalNetPremiumLeadAgent = oImpResponse.TotalNetPremiumLeadAgent
                oResponse.TotalCommissionSubAgent = oImpResponse.TotalCommissionSubAgent
                oResponse.TotalTaxSubAgent = oImpResponse.TotalTaxSubAgent
                oResponse.TotalNetPremiumSubAgent = oImpResponse.TotalNetPremiumSubAgent

                ' oResponse.AgentCommission = SAMFunc.GetDeserializedValues(Of List(Of BaseGetHeaderAndAgentCommissionByKeyResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetHeaderAndAgentCommissionByKeyResponseTypeAgentCommission", sConvertToTypeName:="BaseGetHeaderAndAgentCommissionByKeyResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.AgentCommission = DataTabletoList_GetHeaderAndAgentCommissionByKey(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetHeaderAndAgentCommissionByKeyRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetHeaderAndAgentCommissionByKeyRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="GetHeaderAndPolicyFeesByKeyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetHeaderAndPolicyFeesByKey(ByVal GetHeaderAndPolicyFeesByKeyRequest As GetHeaderAndPolicyFeesByKeyRequestType) As GetHeaderAndPolicyFeesByKeyResponseType Implements IPurePolicyService.GetHeaderAndPolicyFeesByKey

        Try

            Dim sUserName As String = GetHeaderAndPolicyFeesByKeyRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHPKey", iUserId)
            CommonFunctions.CheckSecurityToken(GetHeaderAndPolicyFeesByKeyRequest.WCFSecurityToken)
            Dim oResponse As New GetHeaderAndPolicyFeesByKeyResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetHeaderAndPolicyFeesByKeyRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetHeaderAndPolicyFeesByKeyRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetHeaderAndPolicyFeesByKeyResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetHeaderAndPolicyFeesByKeyRequest.BranchCode
            oImpRequest.InsuranceFileKey = GetHeaderAndPolicyFeesByKeyRequest.InsuranceFileKey
            oImpRequest.WCFSecurityToken = If(GetHeaderAndPolicyFeesByKeyRequest.WCFSecurityToken.Length > 0, GetHeaderAndPolicyFeesByKeyRequest.WCFSecurityToken, "WCFSecurityToken")

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetHeaderAndPolicyFeesByKey(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.InsuranceFileKey = oImpResponse.InsuranceFileKey
                oResponse.ClientCode = oImpResponse.ClientCode
                oResponse.InsuranceFileRef = oImpResponse.InsuranceFileRef
                oResponse.Agent = oImpResponse.Agent
                oResponse.InceptionDate = oImpResponse.InceptionDate
                oResponse.CoverStartDate = oImpResponse.CoverStartDate
                oResponse.ExpiryDate = oImpResponse.ExpiryDate
                oResponse.Currency = oImpResponse.Currency
                oResponse.TotalRiskFeesEligibleForFinancing = oImpResponse.TotalRiskFeesEligibleForFinancing
                oResponse.TotalRiskFeesExcludedFromFinancing = oImpResponse.TotalRiskFeesExcludedFromFinancing
                oResponse.TotalRiskFees = oImpResponse.TotalRiskFees
                oResponse.TotalPolicyFeesEligibleForFinancing = oImpResponse.TotalPolicyFeesEligibleForFinancing
                oResponse.TotalPolicyFeesExcludedFromFinancing = oImpResponse.TotalPolicyFeesExcludedFromFinancing
                oResponse.TotalPolicyFees = oImpResponse.TotalPolicyFees
                'oResponse.PolicyFees = SAMFunc.GetDeserializedValues(Of List(Of BaseGetHeaderAndPolicyFeesByKeyResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetHeaderAndPolicyFeesByKeyResponseTypePolicyFees", sConvertToTypeName:="BaseGetHeaderAndPolicyFeesByKeyResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.PolicyFees = DataTabletoList_GetHeaderAndPolicyFeesByKey(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetHeaderAndPolicyFeesByKeyRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetHeaderAndPolicyFeesByKeyRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    '''This function will get the insuranceFilekey as an input and it will be passed to 
    ''' SAMForInsuranceV2Bussiness file 
    '''</summary>
    '''<param name="oGetHeaderAndPolicyTaxByKeyRequest" type="GetHeaderAndPolicyTaxByKeyRequestType"></param>    
    '''<remarks></remarks>
    Public Function GetHeaderAndPolicyTaxByKey(ByVal oGetHeaderAndPolicyTaxByKeyRequest As GetHeaderAndPolicyTaxByKeyRequestType) As GetHeaderAndPolicyTaxByKeyResponseType Implements IPurePolicyService.GetHeaderAndPolicyTaxByKey

        Try

            Dim sUserName As String = oGetHeaderAndPolicyTaxByKeyRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHRKEY", iUserId)
            CommonFunctions.CheckSecurityToken(oGetHeaderAndPolicyTaxByKeyRequest.WCFSecurityToken)
            Dim oResponse As New GetHeaderAndPolicyTaxByKeyResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetHeaderAndPolicyTaxByKeyRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetHeaderAndPolicyTaxByKeyRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetHeaderAndPolicyTaxByKeyResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetHeaderAndPolicyTaxByKeyRequest.BranchCode
            oImpRequest.InsuranceFileKey = oGetHeaderAndPolicyTaxByKeyRequest.InsuranceFileKey
            oImpRequest.WCFSecurityToken = If(oGetHeaderAndPolicyTaxByKeyRequest.WCFSecurityToken.Length > 0, oGetHeaderAndPolicyTaxByKeyRequest.WCFSecurityToken, "WCFSecurityToken")

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetHeaderAndPolicyTaxByKey(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure  
                oResponse.InsuranceFileKey = oImpResponse.InsuranceFileKey
                oResponse.ClientCode = oImpResponse.ClientCode
                oResponse.InsuranceFileRef = oImpResponse.InsuranceFileRef
                oResponse.Agent = oImpResponse.Agent
                oResponse.InceptionDate = oImpResponse.InceptionDate
                oResponse.CoverStartDate = oImpResponse.CoverStartDate
                oResponse.ExpiryDate = oImpResponse.ExpiryDate
                oResponse.Currency = oImpResponse.Currency
                oResponse.NetTotal = oImpResponse.NetTotal
                oResponse.TaxTotal = oImpResponse.TaxTotal
                oResponse.FeeTotal = oImpResponse.FeeTotal
                oResponse.GrossTotal = oImpResponse.GrossTotal
                oResponse.TotalRiskTaxEligibleForFinancing = oImpResponse.TotalRiskTaxEligibleForFinancing
                oResponse.TotalRiskTaxExcludedFromFinancing = oImpResponse.TotalRiskTaxExcludedFromFinancing
                oResponse.TotalRiskTax = oImpResponse.TotalRiskTax
                oResponse.TotalPolicyTaxEligibleForFinancing = oImpResponse.TotalPolicyTaxEligibleForFinancing
                oResponse.TotalPolicyTaxExcludedFromFinancing = oImpResponse.TotalPolicyTaxExcludedFromFinancing
                oResponse.TotalPolicyTax = oImpResponse.TotalPolicyTax
                oResponse.TotalCommissionLeadAgent = oImpResponse.TotalCommissionLeadAgent
                oResponse.TotalTaxLeadAgent = oImpResponse.TotalTaxLeadAgent
                'oResponse.PolicyTaxes = SAMFunc.GetDeserializedValues(Of List(Of BaseGetHeaderAndPolicyTaxByKeyResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetHeaderAndPolicyTaxByKeyResponseTypePolicyTaxes", sConvertToTypeName:="BaseGetHeaderAndPolicyTaxByKeyResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.PolicyTaxes = DataTabletoList_GetHeaderAndPolicyTaxByKey(oImpResponse.ResultData.Tables(0))
                End If

                oResponse.NetSumInsured = oImpResponse.NetSumInsured

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetHeaderAndPolicyTaxByKeyRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetHeaderAndPolicyTaxByKeyRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    '''This function will get the  insuranceFilekey and RiskKey as an input and it will be passed to 
    ''' SAMForInsuranceV2Bussiness file
    '''</summary>
    '''<param name="GetHeaderAndRiskFeesByKeyRequest" type="GetHeaderAndRiskFeesByKeyResponseType"></param>   
    '''<remarks></remarks>
    Public Function GetHeaderAndRiskFeesByKey(ByVal GetHeaderAndRiskFeesByKeyRequest As GetHeaderAndRiskFeesByKeyRequestType) As GetHeaderAndRiskFeesByKeyResponseType Implements IPurePolicyService.GetHeaderAndRiskFeesByKey

        Try

            Dim sUserName As String = GetHeaderAndRiskFeesByKeyRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHRKEY", iUserId)
            CommonFunctions.CheckSecurityToken(GetHeaderAndRiskFeesByKeyRequest.WCFSecurityToken)
            Dim oResponse As New GetHeaderAndRiskFeesByKeyResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetHeaderAndRiskFeesByKeyRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetHeaderAndRiskFeesByKeyRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetHeaderAndRiskFeesByKeyResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetHeaderAndRiskFeesByKeyRequest.BranchCode
            oImpRequest.InsuranceFileKey = GetHeaderAndRiskFeesByKeyRequest.InsuranceFileKey
            oImpRequest.RiskKey = GetHeaderAndRiskFeesByKeyRequest.RiskKey
            oImpRequest.WCFSecurityToken = If(GetHeaderAndRiskFeesByKeyRequest.WCFSecurityToken.Length > 0, GetHeaderAndRiskFeesByKeyRequest.WCFSecurityToken, "WCFSecurityToken")


            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetHeaderAndRiskFeesByKey(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure               
                oResponse.InsuranceFileKey = oImpResponse.InsuranceFileKey
                oResponse.ClientCode = oImpResponse.ClientCode
                oResponse.InsuranceFileRef = oImpResponse.InsuranceFileRef
                oResponse.Currency = oImpResponse.Currency

                'oResponse.RiskFees = SAMFunc.GetDeserializedValues(Of List(Of BaseGetHeaderAndRiskFeesByKeyResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetHeaderAndRiskFeesByKeyResponseTypeRiskFees", sConvertToTypeName:="BaseGetHeaderAndRiskFeesByKeyResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.RiskFees = DataTabletoList_GetHeaderAndRiskFeesByKey(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetHeaderAndRiskFeesByKeyRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetHeaderAndRiskFeesByKeyRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    '''This is the "WebMethod" which will get the inputs "BranchCode , "InsuranceFileKey" and "RiskKey" 
    '''from front End
    '''</summary>
    '''<remarks></remarks> 
    Public Function GetHeaderAndRiskTaxByKey(ByVal GetHeaderAndRiskTaxByKeyRequest As GetHeaderAndRiskTaxByKeyRequestType) As GetHeaderAndRiskTaxByKeyResponseType Implements IPurePolicyService.GetHeaderAndRiskTaxByKey

        Try

            Dim sUserName As String = GetHeaderAndRiskTaxByKeyRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHRKEY", iUserId)
            CommonFunctions.CheckSecurityToken(GetHeaderAndRiskTaxByKeyRequest.WCFSecurityToken)
            Dim oResponse As New GetHeaderAndRiskTaxByKeyResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetHeaderAndRiskTaxByKeyRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetHeaderAndRiskTaxByKeyRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetHeaderAndRiskTaxByKeyResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetHeaderAndRiskTaxByKeyRequest.BranchCode
            oImpRequest.InsuranceFileKey = GetHeaderAndRiskTaxByKeyRequest.InsuranceFileKey
            oImpRequest.RiskKey = GetHeaderAndRiskTaxByKeyRequest.RiskKey
            oImpRequest.WCFSecurityToken = If(GetHeaderAndRiskTaxByKeyRequest.WCFSecurityToken.Length > 0, GetHeaderAndRiskTaxByKeyRequest.WCFSecurityToken, "WCFSecurityToken")

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetHeaderAndRiskTaxByKey(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure  
                oResponse.InsuranceFileKey = oImpResponse.InsuranceFileKey
                oResponse.ClientCode = oImpResponse.ClientCode
                oResponse.InsuranceFileRef = oImpResponse.InsuranceFileRef
                oResponse.Currency = oImpResponse.Currency
                'oResponse.RiskTaxes = SAMFunc.GetDeserializedValues(Of List(Of BaseGetHeaderAndRiskTaxByKeyResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetHeaderAndRiskTaxByKeyResponseTypeRiskTaxes", sConvertToTypeName:="BaseGetHeaderAndRiskTaxByKeyResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.RiskTaxes = DataTabletoList_GetHeaderAndRiskTaxByKey(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetHeaderAndRiskTaxByKeyRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetHeaderAndRiskTaxByKeyRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>  
    ''' Retrieves risk details for a policy.
    ''' </summary>  
    ''' <param name="GetHeaderAndRisksByKeyRequest">An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetHeaderAndRisksByKeyRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetHeaderAndRisksByKeyResponseType</returns>  
    Public Function GetHeaderAndRisksByKey(ByVal GetHeaderAndRisksByKeyRequest As GetHeaderAndRisksByKeyRequestType) As GetHeaderAndRisksByKeyResponseType Implements IPurePolicyService.GetHeaderAndRisksByKey
        Try

            Dim sUserName As String = GetHeaderAndRisksByKeyRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHRKEY", iUserId)
            CommonFunctions.CheckSecurityToken(GetHeaderAndRisksByKeyRequest.WCFSecurityToken)
            Dim oResponse As New GetHeaderAndRisksByKeyResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetHeaderAndRisksByKeyRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetHeaderAndRisksByKeyRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetHeaderAndRisksByKeyResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetHeaderAndRisksByKeyRequest.BranchCode
            oImpRequest.InsuranceFileKey = GetHeaderAndRisksByKeyRequest.InsuranceFileKey

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetHeaderAndRisksByKey(oImpRequest)

                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                Dim oXmlOverride As XmlAttributeOverrides = New XmlAttributeOverrides()
                Dim oXmlAttributes As XmlAttributes = New XmlAttributes()
                oXmlAttributes.Xmlns = False

                oXmlOverride.Add(GetType(SFI.SAMForInsuranceV2.BaseGetHeaderAndRisksByKeyResponseTypeRisks), oXmlAttributes)
                oXmlOverride.Add(GetType(SFI.SAMForInsuranceV2.BaseGetHeaderAndRisksByKeyResponseTypeRisksRow), oXmlAttributes)
                With oResponse
                    .InsuranceFileKey = oImpResponse.InsuranceFileKey
                    .ClientCode = oImpResponse.ClientCode
                    .InsuranceFileRef = oImpResponse.InsuranceFileRef
                    .Agent = oImpResponse.Agent
                    .InceptionDate = oImpResponse.InceptionDate
                    .CoverStartDate = oImpResponse.CoverStartDate
                    .ExpiryDate = oImpResponse.ExpiryDate
                    .Currency = oImpResponse.Currency
                    .NetTotal = oImpResponse.NetTotal
                    .TaxTotal = oImpResponse.TaxTotal
                    .FeeTotal = oImpResponse.FeeTotal
                    .GrossTotal = oImpResponse.GrossTotal
                    .TotalSumInsured = oImpResponse.TotalSumInsured
                    .TaxRateUnRounded = oImpResponse.TaxRateUnRounded
                    .CorrespondenceType = oImpResponse.CorrespondenceType
                    .DefaultPreferredCorrespondence = oImpResponse.DefaultPreferredCorrespondence
                    .IsAgentReceiveCorrespondence = oImpResponse.IsAgentReceiveCorrespondence
                    .MailContacts = oImpResponse.MailContacts
                    .Is_Auto_Rated = oImpResponse.Is_Auto_Rated
                End With

                If oImpResponse.Risks IsNot Nothing AndAlso oImpResponse.Risks.Row.Length > 0 Then
                    oResponse.Risks = oImpResponse.Risks.Row().ToList().ConvertAll(
                        New Converter(Of BaseImplementationTypes.BaseGetHeaderAndRisksByKeyResponseTypeRisksRow, BaseGetHeaderAndRisksByKeyResponseTypeRow)(AddressOf CommonFunctions.ToServiceHeaderAndRisksByKeyResponseTypeRisksList))
                End If

                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetHeaderAndRisksByKeyRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetHeaderAndRisksByKeyRequest))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Get Header And Summaries By Key
    ''' </summary>
    ''' <param name="GetHeaderAndSummariesByKeyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetHeaderAndSummariesByKey(ByVal GetHeaderAndSummariesByKeyRequest As GetHeaderAndSummariesByKeyRequestType) As GetHeaderAndSummariesByKeyResponseType Implements IPurePolicyService.GetHeaderAndSummariesByKey, IPureClaimService.GetHeaderAndSummariesByKey

        Try

            Dim sUserName As String = GetHeaderAndSummariesByKeyRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHSKey", iUserId)
            CommonFunctions.CheckSecurityToken(GetHeaderAndSummariesByKeyRequest.WCFSecurityToken)
            Dim oResponse As New GetHeaderAndSummariesByKeyResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetHeaderAndSummariesByKeyRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetHeaderAndSummariesByKeyRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetHeaderAndSummariesByKeyResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = GetHeaderAndSummariesByKeyRequest.BranchCode
            oImpRequest.InsuranceFileKey = GetHeaderAndSummariesByKeyRequest.InsuranceFileKey
            oImpRequest.IncludeGISRetroactiveDate = GetHeaderAndSummariesByKeyRequest.IncludeGISRetroactiveDate
            oImpRequest.UserName = sUserName
            oImpRequest.PreChangeInsuranceFileKey = GetHeaderAndSummariesByKeyRequest.PreChangeInsuranceFileKey
            oImpRequest.WCFSecurityToken = If(GetHeaderAndSummariesByKeyRequest.WCFSecurityToken.Length > 0, GetHeaderAndSummariesByKeyRequest.WCFSecurityToken, "WCFSecurityToken")
            oImpRequest.ExclusiveLock = GetHeaderAndSummariesByKeyRequest.ExclusiveLock
            oImpRequest.SessionValue = GetHeaderAndSummariesByKeyRequest.SessionValue

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetHeaderAndSummariesByKey(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.CoverEndDate = oImpResponse.CoverEndDate
                oResponse.CoverStartDate = oImpResponse.CoverStartDate
                oResponse.Description = oImpResponse.Description
                oResponse.InceptionDate = oImpResponse.InceptionDate
                oResponse.InsuranceFileKey = oImpResponse.InsuranceFileKey
                oResponse.InsuranceFileRef = oImpResponse.InsuranceFileRef
                oResponse.InsuranceFileStatusCode = oImpResponse.InsuranceFileStatusCode
                oResponse.InsuranceFileTypeCode = oImpResponse.InsuranceFileTypeCode
                oResponse.InsuranceFileVersion = oImpResponse.InsuranceFileVersion
                oResponse.InsuranceFolderKey = oImpResponse.InsuranceFolderKey

                oResponse.OldPolicyNumber = oImpResponse.OldPolicyNumber
                If oImpResponse.ResultDataInsuredParties IsNot Nothing AndAlso oImpResponse.ResultDataInsuredParties.Tables(0) IsNot Nothing Then
                    oResponse.InsuredParties = DataTabletoList_GetHeaderAndSummariesByKey(oImpResponse.ResultDataInsuredParties.Tables(0))
                End If

                oResponse.PartyKey = oImpResponse.PartyKey
                oResponse.PaymentMethodCode = oImpResponse.PaymentMethodCode
                oResponse.ProductCode = oImpResponse.ProductCode
                oResponse.QuoteExpiryDate = oImpResponse.QuoteExpiryDate
                oResponse.QuoteIsLocked = oImpResponse.QuoteIsLocked
                oResponse.QuoteTimeStamp = oImpResponse.QuoteTimeStamp

                'oResponse.Risks = SAMFunc.GetDeserializedValues(Of List(Of BaseGetHeaderAndSummariesResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetHeaderAndSummariesResponseTypeRisks", sConvertToTypeName:="BaseGetHeaderAndSummariesResponseTypeRow")
                If oImpResponse.ResultDataRisk IsNot Nothing AndAlso oImpResponse.ResultDataRisk.Tables(0) IsNot Nothing Then
                    oResponse.Risks = DataTabletoList_GetHeaderAndSummaries(oImpResponse.ResultDataRisk.Tables(0))
                End If

                oResponse.SubBranchCode = oImpResponse.SubBranchCode
                oResponse.ConsolidatedLeadAgentCommission = oImpResponse.ConsolidatedLeadAgentCommission
                oResponse.ConsolidatedSubAgentCommission = oImpResponse.ConsolidatedSubAgentCommission
                oResponse.PolicyLevelTaxesAndFees = CommonFunctions.ToServiceTaxesAndFeesType(oImpResponse.PolicyLevelTaxesAndFees)
                oResponse.AlternativeRef = oImpResponse.AlternativeRef
                oResponse.LeadAgentKey = oImpResponse.LeadAgentKey
                oResponse.LeadAgent = oImpResponse.LeadAgent
                oResponse.LeadAgentCode = oImpResponse.LeadAgentCode
                oResponse.InsuredName = oImpResponse.InsuredName
                oResponse.ProductName = oImpResponse.ProductName
                oResponse.BranchCode = oImpResponse.BranchCode
                oResponse.BranchName = oImpResponse.BranchName
                oResponse.Regarding = oImpResponse.Regarding
                oResponse.PolicyStatusCode = oImpResponse.PolicyStatusCode
                oResponse.AnalysisCode = oImpResponse.AnalysisCode
                oResponse.BusinessTypeCode = oImpResponse.BusinessTypeCode
                oResponse.InceptionDate = oImpResponse.InceptionDate
                oResponse.RenewalDate = oImpResponse.RenewalDate
                oResponse.PolicyTypeCode = oImpResponse.PolicyTypeCode
                oResponse.InceptionTPI = oImpResponse.InceptionTPI
                oResponse.IssueDate = oImpResponse.IssueDate
                oResponse.ProposalDate = oImpResponse.ProposalDate
                oResponse.QuoteExpiryDate = oImpResponse.QuoteExpiryDate
                oResponse.RenewalFrequencyCode = oImpResponse.RenewalFrequencyCode
                oResponse.LTUExpiryDate = oImpResponse.LTUExpiryDate
                oResponse.StopReasonCode = oImpResponse.StopReasonCode
                oResponse.RenewedCount = oImpResponse.RenewedCount
                oResponse.RenewalMethodCode = oImpResponse.RenewalMethodCode
                oResponse.LapsedReasonCode = oImpResponse.LapsedReasonCode
                oResponse.LapseDate = oImpResponse.LapseDate
                oResponse.ReferredAtRenewal = oImpResponse.ReferredAtRenewal
                oResponse.ReferredOnMTA = oImpResponse.ReferredOnMTA
                oResponse.StandardPolicyWordingCode = oImpResponse.StandardPolicyWordingCode
                oResponse.StandardPolicyDescription = oImpResponse.StandardPolicyDescription
                oResponse.PolicyStyleCode = oImpResponse.PolicyStyleCode
                oResponse.LapseDateSpecified = oImpResponse.LapseDateSpecified
                oResponse.IssueDateSpecified = oImpResponse.IssueDateSpecified
                oResponse.LTUExpiryDateSpecified = oImpResponse.LTUExpiryDateSpecified
                oResponse.ProposalDateSpecified = oImpResponse.ProposalDateSpecified
                oResponse.CurrencyCode = oImpResponse.CurrencyCode
                oResponse.AccountHandler = oImpResponse.AccountHandler
                oResponse.AccountHandlerCode = oImpResponse.AccountHandlerCode
                oResponse.AccountHandlerCnt = oImpResponse.AccountHandlerCnt
                oResponse.AccountHandlerCntSpecified = oImpResponse.AccountHandlerCntSpecified

                oResponse.RenewalStatusTypeCode = oImpResponse.RenewalStatusTypeCode
                oResponse.RenewalStatusTypeDesc = oImpResponse.RenewalStatusTypeDescription

                oResponse.HCExpiryDate = oImpResponse.HCExpiryDate
                oResponse.PolicyDeductible = oImpResponse.PolicyDeductible
                oResponse.PolicyLimits = oImpResponse.PolicyLimits
                oResponse.UnderwritingYear = oImpResponse.UnderwritingYear
                oResponse.UnderwritingYearId = oImpResponse.UnderwritingYearId

                oResponse.MarkedForCollection = oImpResponse.MarkedForCollection

                oResponse.PutOnNextMTAInstallmentRenewal = oImpResponse.PutOnNextMTAInstallmentRenewal
                oResponse.AnniversaryCopy = oImpResponse.AnniversaryCopy
                oResponse.RenewalDayNo = oImpResponse.RenewalDayNo

                oResponse.QuoteVersion = oImpResponse.QuoteVersion
                oResponse.BaseInsuranceFolderKey = oImpResponse.BaseInsuranceFolderKey
                oResponse.QuoteStatusKey = CType([Enum].ToObject(GetType(BaseImplementationTypes.QuoteStatusType), oImpResponse.QuoteStatusKey), QuoteStatusType)

                oResponse.ContactuserKey = oImpResponse.ContactuserKey
                oResponse.ContactUserName = oImpResponse.ContactUserName
                oResponse.IsDeletedContactuser = oImpResponse.IsDeletedContactuser
                oResponse.ContactUserFullName = oImpResponse.ContactUserFullName
                oResponse.ContactUserEmail = oImpResponse.ContactUserEmail
                oResponse.IsMigratedPolicy = oImpResponse.IsMigratedPolicy
                oResponse.IsPolicyInAnnualRenewal = oImpResponse.IsPolicyInAnnualRenewal
                oResponse.IsPolicyInRenewal = oImpResponse.IsPolicyInRenewal
                oResponse.CollectionFrequency = oImpResponse.collectionFrequency
                oResponse.PaymentTerms = oImpResponse.paymentTerms
                oResponse.IsValidAnniversaryToAccept = oImpResponse.IsValidAnniversaryToAccept
                oResponse.CoInsurancePlacement = oImpResponse.CoInsurancePlacement
                oResponse.DefaultPaymentMethod = oImpResponse.DefaultPaymentMethod
                oResponse.DefaultInstalmentPlan = oImpResponse.DefaultInstalmentPlan
                oResponse.DefaultInstalmentPlanVersion = oImpResponse.DefaultInstalmentPlanVersion
                oResponse.DefaultSchemeNumber = oImpResponse.DefaultSchemeNumber
                oResponse.DefaultSchemeVersion = oImpResponse.DefaultSchemeVersion
                oResponse.OriginalInsuranceFileKey = oImpResponse.OriginalInsuranceFileKey
                oResponse.OriginalPremiumFinanceCnt = oImpResponse.OriginalPremiumFinanceCnt
                oResponse.OriginalPremFinanceVersion = oImpResponse.OriginalPremFinanceVersion
                oResponse.ActivePlans = oImpResponse.ActivePlans

                oResponse.BaseCurrencyID = oImpResponse.BaseCurrencyID
                oResponse.TransCurrencyID = oImpResponse.TransCurrencyID
                oResponse.CorrespondenceType = oImpResponse.CorrespondenceType
                oResponse.DefaultPreferredCorrespondence = oImpResponse.DefaultPreferredCorrespondence
                oResponse.IsAgentReceiveCorrespondence = oImpResponse.IsAgentReceiveCorrespondence

                oResponse.IsMarketPlacePolicy = oImpResponse.IsMarketPlacePolicy

                oResponse.DefaultPaymentMethod = oImpResponse.DefaultPaymentMethod
                oResponse.DefaultInstalmentPlan = oImpResponse.DefaultInstalmentPlan
                oResponse.DefaultInstalmentPlanVersion = oImpResponse.DefaultInstalmentPlanVersion
                oResponse.DefaultSchemeNumber = oImpResponse.DefaultSchemeNumber
                oResponse.DefaultSchemeVersion = oImpResponse.DefaultSchemeVersion

                oResponse.AnniversaryDate = oImpResponse.AnniversaryDate
                oResponse.AnniversaryDateSpecified = oImpResponse.AnniversaryDateSpecified
                oResponse.CoverNoteBookNumber = oImpResponse.CoverNoteBookNumber
                oResponse.CoverNoteSheetNumber = oImpResponse.CoverNoteSheetNumber
                oResponse.AssignedDate = oImpResponse.AssignedDate
                oResponse.ActivePlan = oImpResponse.ActivePlan
                oResponse.SavedPreferredDate = oImpResponse.SavedPreferredDate
                oResponse.SavedDayInMonth = oImpResponse.SavedDayInMonth
                oResponse.OriginalInsuranceFileKey = oImpResponse.OriginalInsuranceFileKey
                oResponse.OriginalPremFinanceVersion = oImpResponse.OriginalPremFinanceVersion
                oResponse.OriginalPremiumFinanceCnt = oImpResponse.OriginalPremiumFinanceCnt
                oResponse.Frequency = oImpResponse.Frequency
                oResponse.MtaReasonCode = oImpResponse.MtaReasonCode
                oResponse.MtaReasonId = oImpResponse.MtaReasonId
                oResponse.DefaultPFRF_Id = oImpResponse.DefaultPFRF_ID
                oResponse.SenderEmail = oImpResponse.SenderEmail
                oResponse.ReceiverEmail = oImpResponse.ReceiverEmail
          Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetHeaderAndSummariesByKeyRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetHeaderAndSummariesByKeyRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="GetHeaderAndSummariesByRefRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetHeaderAndSummariesByRef(ByVal GetHeaderAndSummariesByRefRequest As GetHeaderAndSummariesByRefRequestType) As GetHeaderAndSummariesByRefResponseType Implements IPurePolicyService.GetHeaderAndSummariesByRef

        Try

            Dim sUserName As String = GetHeaderAndSummariesByRefRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHSRef", iUserId)
            CommonFunctions.CheckSecurityToken(GetHeaderAndSummariesByRefRequest.WCFSecurityToken)
            Dim oResponse As New GetHeaderAndSummariesByRefResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetHeaderAndSummariesByRefRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetHeaderAndSummariesByRefRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetHeaderAndSummariesByRefResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = GetHeaderAndSummariesByRefRequest.BranchCode
            oImpRequest.InsuranceRef = GetHeaderAndSummariesByRefRequest.InsuranceRef
            oImpRequest.UserName = sUserName
            oImpRequest.WCFSecurityToken = If(GetHeaderAndSummariesByRefRequest.WCFSecurityToken.Length > 0, GetHeaderAndSummariesByRefRequest.WCFSecurityToken, "WCFSecurityToken")


            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetHeaderAndSummariesByRef(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.CoverEndDate = oImpResponse.CoverEndDate
                oResponse.CoverStartDate = oImpResponse.CoverStartDate
                oResponse.Description = oImpResponse.Description
                oResponse.InceptionDate = oImpResponse.InceptionDate
                oResponse.InsuranceFileKey = oImpResponse.InsuranceFileKey
                oResponse.InsuranceFileRef = oImpResponse.InsuranceFileRef
                oResponse.InsuranceFileStatusCode = oImpResponse.InsuranceFileStatusCode
                oResponse.InsuranceFileTypeCode = oImpResponse.InsuranceFileTypeCode
                oResponse.InsuranceFileVersion = oImpResponse.InsuranceFileVersion
                oResponse.InsuranceFolderKey = oImpResponse.InsuranceFolderKey

                'oResponse.InsuredParties = SAMFunc.GetDeserializedValues(Of List(Of GetHeaderAndSummariesByRefResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="GetHeaderAndSummariesByRefResponseTypeInsuredParties", sConvertToTypeName:="GetHeaderAndSummariesByRefResponseTypeRow")
                If oImpResponse.ResultDataInsuredParties IsNot Nothing AndAlso oImpResponse.ResultDataInsuredParties.Tables(0) IsNot Nothing Then
                    oResponse.InsuredParties = DataTabletoList_GetHeaderAndSummariesByRef(oImpResponse.ResultDataInsuredParties.Tables(0))
                End If

                oResponse.PartyKey = oImpResponse.PartyKey
                oResponse.PaymentMethodCode = oImpResponse.PaymentMethodCode
                oResponse.ProductCode = oImpResponse.ProductCode
                oResponse.QuoteExpiryDate = oImpResponse.QuoteExpiryDate
                oResponse.QuoteIsLocked = oImpResponse.QuoteIsLocked
                oResponse.QuoteTimeStamp = oImpResponse.QuoteTimeStamp

                'oResponse.Risks = SAMFunc.GetDeserializedValues(Of List(Of BaseGetHeaderAndSummariesResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetHeaderAndSummariesResponseTypeRisks", sConvertToTypeName:="BaseGetHeaderAndSummariesResponseTypeRow")
                If oImpResponse.ResultDataRisk IsNot Nothing AndAlso oImpResponse.ResultDataRisk.Tables(0) IsNot Nothing Then
                    oResponse.Risks = DataTabletoList_GetHeaderAndSummaries(oImpResponse.ResultDataRisk.Tables(0))
                End If

                oResponse.SubBranchCode = oImpResponse.SubBranchCode
                oResponse.ConsolidatedLeadAgentCommission = oImpResponse.ConsolidatedLeadAgentCommission
                oResponse.ConsolidatedSubAgentCommission = oImpResponse.ConsolidatedSubAgentCommission
                oResponse.PolicyLevelTaxesAndFees = CommonFunctions.ToServiceTaxesAndFeesType(oImpResponse.PolicyLevelTaxesAndFees)
                oResponse.AlternativeRef = oImpResponse.AlternativeRef

                ' Tech Spec PGR 8.8 Renewals ---------------------------------
                oResponse.RenewalStatusTypeCode = oImpResponse.RenewalStatusTypeCode
                oResponse.RenewalStatusTypeDesc = oImpResponse.RenewalStatusTypeDescription
                ' End --------------------------------------------------------

                oResponse.LeadAgentKey = oImpResponse.LeadAgentKey
                oResponse.LeadAgent = oImpResponse.LeadAgent
                oResponse.LeadAgentCode = oImpResponse.LeadAgentCode


                oResponse.InsuredName = oImpResponse.InsuredName
                oResponse.ProductName = oImpResponse.ProductName
                oResponse.BranchCode = oImpResponse.BranchCode
                oResponse.Regarding = oImpResponse.Regarding
                oResponse.PolicyStatusCode = oImpResponse.PolicyStatusCode

                oResponse.AnalysisCode = oImpResponse.AnalysisCode
                oResponse.BusinessTypeCode = oImpResponse.BusinessTypeCode
                oResponse.InceptionDate = oImpResponse.InceptionDate
                oResponse.RenewalDate = oImpResponse.RenewalDate
                oResponse.PolicyTypeCode = oImpResponse.PolicyTypeCode

                oResponse.InceptionTPI = oImpResponse.InceptionTPI
                oResponse.IssueDate = oImpResponse.IssueDate
                oResponse.ProposalDate = oImpResponse.ProposalDate
                oResponse.QuoteExpiryDate = oImpResponse.QuoteExpiryDate
                oResponse.RenewalFrequencyCode = oImpResponse.RenewalFrequencyCode

                oResponse.LTUExpiryDate = oImpResponse.LTUExpiryDate
                oResponse.StopReasonCode = oImpResponse.StopReasonCode
                oResponse.RenewedCount = oImpResponse.RenewedCount
                oResponse.RenewalMethodCode = oImpResponse.RenewalMethodCode
                oResponse.LapsedReasonCode = oImpResponse.LapsedReasonCode
                oResponse.LapseDate = oImpResponse.LapseDate
                oResponse.ReferredAtRenewal = oImpResponse.ReferredAtRenewal
                oResponse.ReferredOnMTA = oImpResponse.ReferredOnMTA
                oResponse.StandardPolicyWordingCode = oImpResponse.StandardPolicyWordingCode
                oResponse.StandardPolicyDescription = oImpResponse.StandardPolicyDescription

                oResponse.PolicyStyleCode = oImpResponse.PolicyStyleCode
                oResponse.LapseDateSpecified = oImpResponse.LapseDateSpecified
                oResponse.IssueDateSpecified = oImpResponse.IssueDateSpecified
                oResponse.LTUExpiryDateSpecified = oImpResponse.LTUExpiryDateSpecified
                oResponse.ProposalDateSpecified = oImpResponse.ProposalDateSpecified

                oResponse.CurrencyCode = oImpResponse.CurrencyCode
                oResponse.AccountHandler = oImpResponse.AccountHandler
                oResponse.AccountHandlerCnt = oImpResponse.AccountHandlerCnt
                oResponse.AccountHandlerCntSpecified = oImpResponse.AccountHandlerCntSpecified


                '(SUGAN) BEGIN - PN 63717
                oResponse.HCExpiryDate = oImpResponse.HCExpiryDate
                oResponse.PolicyDeductible = oImpResponse.PolicyDeductible
                oResponse.PolicyLimits = oImpResponse.PolicyLimits
                oResponse.UnderwritingYear = oImpResponse.UnderwritingYear

                oResponse.MarkedForCollection = oImpResponse.MarkedForCollection
                'Begin WPR36
                oResponse.PutOnNextMTAInstallmentRenewal = oImpResponse.PutOnNextMTAInstallmentRenewal
                oResponse.AnniversaryCopy = oImpResponse.AnniversaryCopy
                oResponse.RenewalDayNo = oImpResponse.RenewalDayNo
                'End WPR36
                'WPR73-74
                oResponse.ContactuserKey = oImpResponse.ContactuserKey
                oResponse.ContactUserName = oImpResponse.ContactUserName
                oResponse.IsDeletedContactuser = oImpResponse.IsDeletedContactuser
                oResponse.ContactUserFullName = oImpResponse.ContactUserFullName
                oResponse.ContactUserEmail = oImpResponse.ContactUserEmail

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetHeaderAndSummariesByRefRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetHeaderAndSummariesByRefRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="GetInstalmentQuotesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetInstalmentQuotes(ByVal GetInstalmentQuotesRequest As GetInstalmentQuotesRequestType) As GetInstalmentQuotesResponseType Implements IPurePolicyService.GetInstalmentQuotes

        Try

            Dim sUserName As String = GetInstalmentQuotesRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGInQuot", iUserId)
            CommonFunctions.CheckSecurityToken(GetInstalmentQuotesRequest.WCFSecurityToken)
            Dim oResponse As New GetInstalmentQuotesResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetInstalmentQuotesRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetInstalmentQuotesRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetInstalmentQuotesResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = GetInstalmentQuotesRequest.BranchCode
            oImpRequest.UserName = sUserName
            oImpRequest.InsuranceFileKey = GetInstalmentQuotesRequest.InsuranceFileKey
            oImpRequest.QuoteDate = GetInstalmentQuotesRequest.QuoteDate
            oImpRequest.StartDate = GetInstalmentQuotesRequest.StartDate
            oImpRequest.EndDate = GetInstalmentQuotesRequest.EndDate
            oImpRequest.PreferredDate = GetInstalmentQuotesRequest.PreferredDate
            oImpRequest.MonthDay = GetInstalmentQuotesRequest.MonthDay
            oImpRequest.WeekDay = GetInstalmentQuotesRequest.WeekDay
            oImpRequest.AmountToFinance = GetInstalmentQuotesRequest.AmountToFinance
            oImpRequest.PaymentProtection = GetInstalmentQuotesRequest.PaymentProtection
            oImpRequest.OverrideRate = GetInstalmentQuotesRequest.OverrideRate
            oImpRequest.OverrideInterestRate = GetInstalmentQuotesRequest.OverrideInterestRate
            'Begin WPR36A
            oImpRequest.InstalmentType = CType([Enum].ToObject(GetType(InstalmentType), GetInstalmentQuotesRequest.InstalmentType), BaseImplementationTypes.InstalmentType)
            oImpRequest.InstalmentTypeSpecified = GetInstalmentQuotesRequest.InstalmentTypeSpecified
            oImpRequest.WCFSecurityToken = If(GetInstalmentQuotesRequest.WCFSecurityToken.Length > 0, GetInstalmentQuotesRequest.WCFSecurityToken, "WCFSecurityToken")
            'wpr10
            oImpRequest.IsUseTransactionCurrency = GetInstalmentQuotesRequest.IsUseTransactionCurrency
            'End WPR36A
            oImpRequest.OverrideDepositAmount = GetInstalmentQuotesRequest.OverrideDepositAmount
            oImpRequest.PFTransaction = Nothing
            oImpRequest.ProcessPFMode = GetInstalmentQuotesRequest.ProcessPFMode
            oImpRequest.OverrideCommission = GetInstalmentQuotesRequest.OverrideCommission
            oImpRequest.PFPremFinanceKey = GetInstalmentQuotesRequest.PFPremFinanceKey
            oImpRequest.PFPremFinanceVersion = GetInstalmentQuotesRequest.PFPremFinanceVersion
            oImpRequest.PreferredInstalmentDueDateonly = GetInstalmentQuotesRequest.PreferredInstalmentDueDateonly

            If GetInstalmentQuotesRequest.PFTransaction IsNot Nothing Then
                ReDim oImpRequest.PFTransaction(GetInstalmentQuotesRequest.PFTransaction.GetUpperBound(0))
                For iCntItems As Integer = GetInstalmentQuotesRequest.PFTransaction.GetLowerBound(0) To _
                    GetInstalmentQuotesRequest.PFTransaction.GetUpperBound(0)

                    ReDim Preserve oImpRequest.PFTransaction(iCntItems)
                    oImpRequest.PFTransaction(iCntItems) = New BaseImplementationTypes.BasePremiumFinancePlanTransactionsType
                    oImpRequest.PFTransaction(iCntItems).InsuranceFileKey = GetInstalmentQuotesRequest.PFTransaction(iCntItems).InsuranceFileKey
                    oImpRequest.PFTransaction(iCntItems).InsuranceRefIndex = GetInstalmentQuotesRequest.PFTransaction(iCntItems).InsuranceRefIndex
                    oImpRequest.PFTransaction(iCntItems).PFTransactionKey = GetInstalmentQuotesRequest.PFTransaction(iCntItems).PFTransactionKey
                    oImpRequest.PFTransaction(iCntItems).Amount = GetInstalmentQuotesRequest.PFTransaction(iCntItems).Amount
                    oImpRequest.PFTransaction(iCntItems).TransDetailKey = GetInstalmentQuotesRequest.PFTransaction(iCntItems).TransDetailKey
                    oImpRequest.PFTransaction(iCntItems).Spare = GetInstalmentQuotesRequest.PFTransaction(iCntItems).Spare
                    oImpRequest.PFTransaction(iCntItems).OutstandingAmount = GetInstalmentQuotesRequest.PFTransaction(iCntItems).OutstandingAmount
                Next
            End If
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetInstalmentQuotes(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.Quotes = SAMFunc.GetDeserializedValues(Of List(Of BaseGetInstalmentQuotesResponseTypeRow))(elmResultDataSet:=oImpResponse.Quotes, sFromTypeName:="BaseGetInstalmentQuotesResponseTypeQuotes", sConvertToTypeName:="BaseGetInstalmentQuotesResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Quotes = DataTabletoList_GetInstalmentQuotes(oImpResponse.ResultData.Tables(0))
                End If
                oResponse.PreferredInstalmentDueDate = oImpResponse.PreferredInstalmentDueDate

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetInstalmentQuotesRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetInstalmentQuotesRequest))
            Return Nothing
        End Try

    End Function

    '''<summary>
    ''' This method is used to get policies ready for renewal
    '''</summary>
    '''<param name="GetPoliciesForRenewalSelectionRequest" type="SFI.SAMForInsuranceV2.GetPoliciesForRenewalSelectionRequestType"></param>   
    '''<returns>SFI.SAMForInsuranceV2.GetPoliciesForRenewalSelectionResponseType</returns>
    '''<remarks></remarks>
    Public Function GetPoliciesForRenewalSelection(ByVal GetPoliciesForRenewalSelectionRequest As GetPoliciesForRenewalSelectionRequestType) As GetPoliciesForRenewalSelectionResponseType Implements IPurePolicyService.GetPoliciesForRenewalSelection
        Try
            'TODO - Check authority

            Dim sUserName As String = GetPoliciesForRenewalSelectionRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGPFR", iUserId)
            CommonFunctions.CheckSecurityToken(GetPoliciesForRenewalSelectionRequest.WCFSecurityToken)

            Dim oResponse As New GetPoliciesForRenewalSelectionResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetPoliciesForRenewalSelectionRequest.BranchCode)
            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetPoliciesForRenewalSelectionRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetPoliciesForRenewalSelectionResponseType = Nothing

            oImpRequest.BranchCode = GetPoliciesForRenewalSelectionRequest.BranchCode
            oImpRequest.ProductCode = GetPoliciesForRenewalSelectionRequest.ProductCode
            oImpRequest.CompareDate = GetPoliciesForRenewalSelectionRequest.CompareDate
            If (oImpRequest.StartDateSpecified) Then
                oImpRequest.StartDate = GetPoliciesForRenewalSelectionRequest.StartDate
            End If
            oImpRequest.WCFSecurityToken = If(GetPoliciesForRenewalSelectionRequest.WCFSecurityToken.Length > 0, GetPoliciesForRenewalSelectionRequest.WCFSecurityToken, "WCFSecurityToken")

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetPoliciesForRenewalSelection(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.Policies = SAMFunc.GetDeserializedValues(Of List(Of BaseGetPoliciesForRenewalSelectionResponseTypeRow))(elmResultDataSet:=oImpResponse.Policies, sFromTypeName:="BaseGetPoliciesForRenewalSelectionResponseTypePolicies", sConvertToTypeName:="BaseGetPoliciesForRenewalSelectionResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Policies = DataTabletoList_GetPoliciesForRenewalSelection(oImpResponse.ResultData.Tables(0))
                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetPoliciesForRenewalSelectionRequest))
            End Try
            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetPoliciesForRenewalSelectionRequest))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="GetPoliciesInRenewalRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPoliciesInRenewal(ByVal GetPoliciesInRenewalRequest As GetPoliciesInRenewalRequestType) As GetPoliciesInRenewalResponseType Implements IPurePolicyService.GetPoliciesInRenewal
        Try
            'Check authority

            Dim sUserName As String = GetPoliciesInRenewalRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGPIR", iUserId)
            CommonFunctions.CheckSecurityToken(GetPoliciesInRenewalRequest.WCFSecurityToken)

            Dim oResponse As New GetPoliciesInRenewalResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetPoliciesInRenewalRequest.BranchCode)
            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetPoliciesInRenewalRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetPoliciesInRenewalResponseType = Nothing
            ' Pass the values to the implementation request structure
            If GetPoliciesInRenewalRequest.AgentKeySpecified = True Then
                oImpRequest.AgentKey = GetPoliciesInRenewalRequest.AgentKey
            Else
                oImpRequest.AgentKey = iAgentKey
            End If
            oImpRequest.BranchCode = GetPoliciesInRenewalRequest.BranchCode
            oImpRequest.PartyKey = GetPoliciesInRenewalRequest.PartyKey
            oImpRequest.DirectOnly = GetPoliciesInRenewalRequest.DirectOnly
            oImpRequest.ForAccept = GetPoliciesInRenewalRequest.ForAccept
            oImpRequest.ProductCode = GetPoliciesInRenewalRequest.ProductCode
            oImpRequest.RenewalDate = GetPoliciesInRenewalRequest.RenewalDate
            oImpRequest.UserName = sUserName
            'WPR73-74
            oImpRequest.InsuranceRef = GetPoliciesInRenewalRequest.InsuranceRef
            'oImpRequest.InsuranceRefSpecified = GetPoliciesInRenewalRequest.InsuranceRefSpecified
            oImpRequest.SearchType = CType([Enum].ToObject(GetType(ContactUserSearchType), GetPoliciesInRenewalRequest.SearchType), BaseImplementationTypes.ContactUserSearchType)
            oImpRequest.RetrieveAssociates = GetPoliciesInRenewalRequest.RetrieveAssociates

            oImpRequest.WCFSecurityToken = If(GetPoliciesInRenewalRequest.WCFSecurityToken.Length > 0, GetPoliciesInRenewalRequest.WCFSecurityToken, "WCFSecurityToken")

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetPoliciesInRenewal(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    'oResponse.Policies = SAMFunc.GetDeserializedValues(Of List(Of BaseGetPoliciesInRenewalResponseTypeRow))(elmResultDataSet:=oImpResponse.Policies, sFromTypeName:="BaseGetPoliciesInRenewalResponseTypePolicies", sConvertToTypeName:="BaseGetPoliciesInRenewalResponseTypeRow")
                    oResponse.Policies = DataTabletoList_GetPoliciesInRenewal(oImpResponse.ResultData.Tables(0))
                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetPoliciesInRenewalRequest))
            End Try
            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetPoliciesInRenewalRequest))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' This is webservice method for GetPoliciesOnBankGuaranteeForReceipt
    '''<param name="oGetPoliciesOnBankGuaranteeForReceiptRequest" type="GetPoliciesOnBankGuaranteeForReceiptRequestType"></param>   
    '''<returns>GetPoliciesOnBankGuaranteeForReceiptResponseType</returns>
    '''</summary>
    '''<remarks></remarks> 
    Public Function GetPoliciesOnBankGuaranteeByKey(ByVal oGetPoliciesOnBankGuaranteeByKeyRequest As GetPoliciesOnBankGuaranteeByKeyRequestType) As GetPoliciesOnBankGuaranteeByKeyResponseType Implements IPurePolicyService.GetPoliciesOnBankGuaranteeByKey
        Try

            Dim sUserName As String = oGetPoliciesOnBankGuaranteeByKeyRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMBGPol", iUserId)
            CommonFunctions.CheckSecurityToken(oGetPoliciesOnBankGuaranteeByKeyRequest.WCFSecurityToken)
            Dim oResponse As New GetPoliciesOnBankGuaranteeByKeyResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetPoliciesOnBankGuaranteeByKeyRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetPoliciesOnBankGuaranteeByKeyRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetPoliciesOnBankGuaranteeByKeyResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetPoliciesOnBankGuaranteeByKeyRequest.BranchCode
            oImpRequest.BGKey = oGetPoliciesOnBankGuaranteeByKeyRequest.BGKey
            oImpRequest.WCFSecurityToken = If(oGetPoliciesOnBankGuaranteeByKeyRequest.WCFSecurityToken.Length > 0, oGetPoliciesOnBankGuaranteeByKeyRequest.WCFSecurityToken, "WCFSecurityToken")

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetPoliciesOnBankGuaranteeByKey(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.PartyBGPolicyDetails = SAMFunc.GetDeserializedValues(Of List(Of BaseGetPoliciesOnBankGuaranteeByKeyResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultBGPolicyDetailsDataset, sFromTypeName:="BaseGetPoliciesOnBankGuaranteeByKeyResponseTypePartyBGPolicyDetails", sConvertToTypeName:="BaseGetPoliciesOnBankGuaranteeByKeyResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.PartyBGPolicyDetails = DataTabletoList_GetPoliciesOnBankGuaranteeByKey(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetPoliciesOnBankGuaranteeByKeyRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetPoliciesOnBankGuaranteeByKeyRequest))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' This is webservice method for GetPoliciesOnBankGuaranteeForReceipt
    '''<param name="oGetPoliciesOnBankGuaranteeForReceiptRequest" type="GetPoliciesOnBankGuaranteeForReceiptRequestType"></param>   
    '''<returns>GetPoliciesOnBankGuaranteeForReceiptResponseType</returns>
    '''</summary>
    '''<remarks></remarks> 
    Public Function GetPoliciesOnBankGuaranteeForReceipt(ByVal oGetPoliciesOnBankGuaranteeForReceiptRequest As GetPoliciesOnBankGuaranteeForReceiptRequestType) As GetPoliciesOnBankGuaranteeForReceiptResponseType Implements IPurePolicyService.GetPoliciesOnBankGuaranteeForReceipt
        Try

            Dim sUserName As String = oGetPoliciesOnBankGuaranteeForReceiptRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMRBGGET", iUserId)
            CommonFunctions.CheckSecurityToken(oGetPoliciesOnBankGuaranteeForReceiptRequest.WCFSecurityToken)
            Dim oResponse As New GetPoliciesOnBankGuaranteeForReceiptResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetPoliciesOnBankGuaranteeForReceiptRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetPoliciesOnBankGuaranteeForReceiptRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetPoliciesOnBankGuaranteeForReceiptResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetPoliciesOnBankGuaranteeForReceiptRequest.BranchCode

            oImpRequest.AccountKey = oGetPoliciesOnBankGuaranteeForReceiptRequest.AccountKey

            oImpRequest.GetPoliciesFor = CType([Enum].ToObject(GetType(BGGetPoliciesActionType), oGetPoliciesOnBankGuaranteeForReceiptRequest.GetPoliciesFor), BaseImplementationTypes.BGGetPoliciesActionType)
            oImpRequest.PartyKey = oGetPoliciesOnBankGuaranteeForReceiptRequest.PartyKey
            oImpRequest.WCFSecurityToken = If(oGetPoliciesOnBankGuaranteeForReceiptRequest.WCFSecurityToken.Length > 0, oGetPoliciesOnBankGuaranteeForReceiptRequest.WCFSecurityToken, "WCFSecurityToken")
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetPoliciesOnBankGuaranteeForReceipt(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.PartyCode = oImpResponse.PartyCode
                oResponse.ResolvedName = oImpResponse.ResolvedName
                'oResponse.PartyBGPolicyDetails = SAMFunc.GetDeserializedValues(Of List(Of BaseGetPoliciesOnBankGuaranteeForReceiptResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultBGPolicyDetailsDataset, sFromTypeName:="BaseGetPoliciesOnBankGuaranteeForReceiptResponseTypePartyBGPolicyDetails", sConvertToTypeName:="BaseGetPoliciesOnBankGuaranteeForReceiptResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.PartyBGPolicyDetails = DataTabletoList_GetPoliciesOnBankGuaranteeForReceipt(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetPoliciesOnBankGuaranteeForReceiptRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetPoliciesOnBankGuaranteeForReceiptRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetPolicyBankGuaranteeRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPolicyBankGuarantee(ByVal oGetPolicyBankGuaranteeRequest As GetPolicyBankGuaranteeRequestType) As GetPolicyBankGuaranteeResponseType Implements IPurePolicyService.GetPolicyBankGuarantee

        Try

            Dim sUserName As String = oGetPolicyBankGuaranteeRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMPBGGET", iUserId)
            CommonFunctions.CheckSecurityToken(oGetPolicyBankGuaranteeRequest.WCFSecurityToken)
            Dim oResponse As New GetPolicyBankGuaranteeResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetPolicyBankGuaranteeRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetPolicyBankGuaranteeRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetPolicyBankGuaranteeResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetPolicyBankGuaranteeRequest.BranchCode
            oImpRequest.InsuranceFileKey = oGetPolicyBankGuaranteeRequest.InsuranceFileKey
            If (oGetPolicyBankGuaranteeRequest.GetBGsOf = BGPartyType.Agent) Then
                oImpRequest.GetBGsOf = BaseImplementationTypes.BGPartyType.Agent
            ElseIf (oGetPolicyBankGuaranteeRequest.GetBGsOf = BGPartyType.Client) Then
                oImpRequest.GetBGsOf = BaseImplementationTypes.BGPartyType.Client
            End If
            oImpRequest.WCFSecurityToken = If(oGetPolicyBankGuaranteeRequest.WCFSecurityToken.Length > 0, oGetPolicyBankGuaranteeRequest.WCFSecurityToken, "WCFSecurityToken")

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetPolicyBankGuarantee(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.BankGuarantee = SAMFunc.GetDeserializedValues(Of List(Of BaseGetPolicyBankGuaranteeResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetPolicyBankGuaranteeResponseTypeBankGuarantee", sConvertToTypeName:="BaseGetPolicyBankGuaranteeResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.BankGuarantee = DataTabletoList_GetPolicyBankGuarantee(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetPolicyBankGuaranteeRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetPolicyBankGuaranteeRequest))
            Return Nothing
        End Try


    End Function

    Public Function GetQuotesMarkedForCollection(ByVal oGetQuotesMarkedForCollectionRequest As GetQuotesMarkedForCollectionRequestType) As GetQuotesMarkedForCollectionResponseType Implements IPurePolicyService.GetQuotesMarkedForCollection

        Try
            Dim sUserName As String = oGetQuotesMarkedForCollectionRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHSKey", iUserId)
            CommonFunctions.CheckSecurityToken(oGetQuotesMarkedForCollectionRequest.WCFSecurityToken)

            Dim oResponse As New GetQuotesMarkedForCollectionResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetQuotesMarkedForCollectionRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetQuotesMarkedForCollectionRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetQuotesMarkedForCollectionResponseType = Nothing

            oImpRequest.BranchCode = oGetQuotesMarkedForCollectionRequest.BranchCode
            oImpRequest.AgentKey = oGetQuotesMarkedForCollectionRequest.AgentKey
            oImpRequest.AgentKeySpecified = oGetQuotesMarkedForCollectionRequest.AgentKeySpecified

            oImpRequest.DirectBusinessOnly = oGetQuotesMarkedForCollectionRequest.DirectBusinessOnly
            oImpRequest.DirectBusinessOnlySpecified = oGetQuotesMarkedForCollectionRequest.DirectBusinessOnlySpecified
            oImpRequest.InsuranceFileKey = oGetQuotesMarkedForCollectionRequest.InsuranceFileKey
            oImpRequest.InsuranceFileKeySpecified = oGetQuotesMarkedForCollectionRequest.InsuranceFileKeySpecified
            oImpRequest.PartyKey = oGetQuotesMarkedForCollectionRequest.PartyKey
            oImpRequest.PartyKeySpecified = oGetQuotesMarkedForCollectionRequest.PartyKeySpecified
            oImpRequest.SearchDateFrom = oGetQuotesMarkedForCollectionRequest.SearchDateFrom
            oImpRequest.SearchDateFromSpecified = oGetQuotesMarkedForCollectionRequest.SearchDateFromSpecified
            oImpRequest.SearchDateTo = oGetQuotesMarkedForCollectionRequest.SearchDateTo
            oImpRequest.SearchDateToSpecified = oGetQuotesMarkedForCollectionRequest.SearchDateToSpecified
            oImpRequest.WCFSecurityToken = If(oGetQuotesMarkedForCollectionRequest.WCFSecurityToken.Length > 0, oGetQuotesMarkedForCollectionRequest.WCFSecurityToken, "WCFSecurityToken")

            Dim lCount As Integer

            If oGetQuotesMarkedForCollectionRequest.Products IsNot Nothing Then
                For Each product As BaseGetQuotesMarkedForCollectionRequestTypeProducts In oGetQuotesMarkedForCollectionRequest.Products
                    ReDim Preserve oImpRequest.Products(lCount)
                    oImpRequest.Products(lCount) = New BaseImplementationTypes.BaseGetQuotesMarkedForCollectionRequestTypeProducts
                    oImpRequest.Products(lCount).ProductCode = product.ProductCode
                    lCount += 1
                Next
            End If

            With oGetQuotesMarkedForCollectionRequest
                If .Products IsNot Nothing Then
                    CommonFunctions.ToBaseImpBaseGetQuotesMarkedForCollection(.Products.ToList(), oImpRequest.Products.ToList())
                End If
            End With

            ' Call the implementation method
            Try
                oImpResponse = oBusiness.GetQuotesMarkedForCollection(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.MarkedQuotes = SAMFunc.GetDeserializedValues(Of List(Of BaseGetQuotesMarkedForCollectionResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataSet, sFromTypeName:="BaseGetQuotesMarkedForCollectionResponseTypeMarkedQuotes", sConvertToTypeName:="BaseGetQuotesMarkedForCollectionResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.MarkedQuotes = DataTabletoList_GetQuotesMarkedForCollection(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetQuotesMarkedForCollectionRequest))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetQuotesMarkedForCollectionRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' This is webservice method definition for fetching the rating details
    ''' </summary>
    ''' <param name="GetRatingDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRatingDetails(ByVal GetRatingDetailsRequest As GetRatingDetailsRequestType) As GetRatingDetailsResponseType Implements IPurePolicyService.GetRatingDetails

        Try

            Dim sUserName As String = GetRatingDetailsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGRatDet", iUserId)
            CommonFunctions.CheckSecurityToken(GetRatingDetailsRequest.WCFSecurityToken)
            Dim oResponse As New GetRatingDetailsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetRatingDetailsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetRatingDetailsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetRatingDetailsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = GetRatingDetailsRequest.BranchCode
            oImpRequest.InsuranceFileKey = GetRatingDetailsRequest.InsuranceFileKey
            oImpRequest.RiskKey = GetRatingDetailsRequest.RiskKey
            oImpRequest.UserName = sUserName
            oImpRequest.WCFSecurityToken = If(GetRatingDetailsRequest.WCFSecurityToken.Length > 0, GetRatingDetailsRequest.WCFSecurityToken, "WCFSecurityToken")

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetRatingDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.RatingDetails = SAMFunc.GetDeserializedValues(Of List(Of BaseGetRatingDetailsResponseTypeRow))(elmResultDataSet:=oImpResponse.RatingDetails, sFromTypeName:="BaseGetRatingDetailsResponseTypeRatingDetails", sConvertToTypeName:="BaseGetRatingDetailsResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.RatingDetails = DataTabletoList_GetRatingDetails(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetRatingDetailsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetRatingDetailsRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="GetRenewalStatusRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRenewalStatus(ByVal GetRenewalStatusRequest As GetRenewalStatusRequestType) As GetRenewalStatusResponseType Implements IPurePolicyService.GetRenewalStatus

        Try
            Dim sUserName As String = GetRenewalStatusRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMFIFFCLM", iUserId)
            CommonFunctions.CheckSecurityToken(GetRenewalStatusRequest.WCFSecurityToken)
            Dim impRequest As New SAMForInsuranceV2ImplementationTypes.GetRenewalStatusRequestType
            Dim impResponse As SAMForInsuranceV2ImplementationTypes.GetRenewalStatusResponseType = Nothing

            Dim response As New GetRenewalStatusResponseType
            Dim business As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetRenewalStatusRequest.BranchCode)
            ' Implementation structures

            impRequest.AgentKey = iAgentKey
            impRequest.UserName = sUserName
            impRequest.BranchCode = GetRenewalStatusRequest.BranchCode
            impRequest.InsuranceFileKey = GetRenewalStatusRequest.InsuranceFileKey
            Try
                ' Call the implementation method
                impResponse = business.GetRenewalStatus(impRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(impResponse.STSError)
                ' Retrieve the values from the implementation response structure into Actual Response
                response.CriticalDate = impResponse.CriticalDate
                response.DateCreated = impResponse.DateCreated
                response.DateInvitePrinted = impResponse.DateInvitePrinted
                response.EmailSent = impResponse.EmailSent
                response.EmailSentDate = impResponse.EmailSentDate
                response.InsuranceHolderKey = impResponse.InsuranceHolderKey
                response.IsInvitePrinted = impResponse.IsInvitePrinted
                response.LeadAgentKey = impResponse.LeadAgentKey
                response.OriginalInsuranceFileKey = impResponse.OriginalInsuranceFileKey
                response.ProductCode = impResponse.ProductCode
                response.RenewalExceptionNotes = impResponse.RenewalExceptionNotes
                response.RenewalExceptionReasonCode = impResponse.RenewalExceptionReasonCode
                response.RenewalExceptionReasonDescription = impResponse.RenewalExceptionReasonDescription
                response.RenewalStatusKey = impResponse.RenewalStatusKey
                response.RenewalStatusTypeCode = impResponse.RenewalStatusTypeCode
                response.RenewalStatusTypeDescription = impResponse.RenewalStatusTypeDescription
                response.IsDuplicateRenewalExists = impResponse.IsDuplicateRenewal
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(impResponse, response, ex, CommonFunctions.CreateDictionary(GetRenewalStatusRequest))
            End Try
            Return response
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetRenewalStatusRequest))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="GetRiskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRisk(ByVal GetRiskRequest As GetRiskRequestType) As GetRiskResponseType Implements IPurePolicyService.GetRisk

        Try

            Dim sUserName As String = GetRiskRequest.LoginUserName
            Dim nAgentKey As Integer = 0
            Dim nUserId As Integer = 0

            CommonFunctions.GetIdentity(sUserName, nAgentKey, nUserId)
            CommonFunctions.CheckAuthority("SAMGRsk", nUserId)
            CommonFunctions.CheckSecurityToken(GetRiskRequest.WCFSecurityToken)
            Dim oResponse As New GetRiskResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetRiskRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetRiskRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetRiskResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = nAgentKey
            oImpRequest.BranchCode = GetRiskRequest.BranchCode
            oImpRequest.InsuranceFileKey = GetRiskRequest.InsuranceFileKey
            oImpRequest.InsuranceFolderKey = GetRiskRequest.InsuranceFolderKey
            oImpRequest.QuoteTimeStamp = GetRiskRequest.QuoteTimeStamp
            oImpRequest.RiskKey = GetRiskRequest.RiskKey
            oImpRequest.UserName = sUserName
            oImpRequest.IgnoreLocking = GetRiskRequest.IgnoreLocking
            oImpRequest.riskLinkStatusFlag = GetRiskRequest.riskLinkStatusFlag
            oImpRequest.isForEdit = GetRiskRequest.isForEdit

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.RetrieveRisk(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.CommissionAmount = oImpResponse.CommissionAmount
                oResponse.PremiumDueGross = oImpResponse.PremiumDueGross
                oResponse.PremiumDueNet = oImpResponse.PremiumDueNet
                oResponse.PremiumDueTax = oImpResponse.PremiumDueTax
                oResponse.QuoteTimeStamp = oImpResponse.QuoteTimeStamp
                oResponse.TotalAnnualTax = oImpResponse.TotalAnnualTax
                oResponse.proRataRate = oImpResponse.proRataRate
                oResponse.XMLDataSet = SAMFunc.TransformDatasetPBtoSAM(oImpResponse.XMLDataSet, sCalledVia:="GetRisk")
                oResponse.PolicyLevelTaxesAndFees = CommonFunctions.ToServiceTaxesAndFeesTypeList(oImpResponse.PolicyLevelTaxesAndFees)
                oResponse.RiskLevelTaxesAndFees = CommonFunctions.ToServiceTaxesAndFeesTypeList(oImpResponse.RiskLevelTaxesAndFees)
                oResponse.RiskKey = oImpResponse.riskKey
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetRiskRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetRiskRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="GetStandardPolicyWordingsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetStandardPolicyWordings(ByVal GetStandardPolicyWordingsRequest As GetStandardPolicyWordingsRequestType) As GetStandardPolicyWordingsResponseType Implements IPurePolicyService.GetStandardPolicyWordings

        Try
            Dim sUserName As String = GetStandardPolicyWordingsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMAQuot", iUserId)
            CommonFunctions.CheckSecurityToken(GetStandardPolicyWordingsRequest.WCFSecurityToken)
            Dim oResponse As New GetStandardPolicyWordingsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetStandardPolicyWordingsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetStandardPolicyWordingsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetStandardPolicyWordingsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetStandardPolicyWordingsRequest.BranchCode
            oImpRequest.InsuranceFileKey = GetStandardPolicyWordingsRequest.InsuranceFileKey
            oImpRequest.GetFreshPolicyStandardWording = GetStandardPolicyWordingsRequest.GetFreshPolicyStandardWording
            oImpRequest.WCFSecurityToken = If(GetStandardPolicyWordingsRequest.WCFSecurityToken.Length > 0, GetStandardPolicyWordingsRequest.WCFSecurityToken, "WCFSecurityToken")

            Try

                'Call the implementation method
                oImpResponse = oBusiness.GetStandardPolicyWordings(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                'oResponse.DocumentTemplates = SAMFunc.GetDeserializedValues(Of List(Of BaseGetStandardPolicyWordingsResponseTypeRow))(elmResultDataSet:=oImpResponse.PolicyWordingsDataset, sFromTypeName:="BaseGetStandardPolicyWordingsResponseTypeDocumentTemplates", sConvertToTypeName:="BaseGetStandardPolicyWordingsResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.DocumentTemplates = DataTabletoList_GetStandardPolicyWordings(oImpResponse.ResultData.Tables(0))
                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetStandardPolicyWordingsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetStandardPolicyWordingsRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' This Web method is used to get the standard wording template
    ''' </summary>
    ''' <param name="oGetStandardWordingTemplateRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetStandardWordingTemplate(ByVal oGetStandardWordingTemplateRequest As GetStandardWordingTemplateRequestType) As GetStandardWordingTemplateResponseType Implements IPureCoreService.GetStandardWordingTemplate

        Try

            Dim sUserName As String = oGetStandardWordingTemplateRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New GetStandardWordingTemplateResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetStandardWordingTemplateRequest.BranchCode)
            Dim iCount As Integer = 0
            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetStandardWordingTemplateRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetStandardWordingTemplateResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetStandardWordingTemplateRequest.BranchCode
            oImpRequest.DocumentTemplateCode = oGetStandardWordingTemplateRequest.DocumentTemplateCode.Trim
            oImpRequest.DocumentTemplateKey = oGetStandardWordingTemplateRequest.DocumentTemplateKey
            oImpRequest.DocumentTemplateKeySpecified = oGetStandardWordingTemplateRequest.DocumentTemplateKeySpecified

            If oGetStandardWordingTemplateRequest.DocumentTemplateFormatSpecified Then
                oImpRequest.DocumentTemplateFormatSpecified = True
                oImpRequest.DocumentTemplateFormat = CType([Enum].ToObject(GetType(DocumentFormatType), oGetStandardWordingTemplateRequest.DocumentTemplateFormat), BaseImplementationTypes.DocumentFormatType)
            End If

            oImpRequest.IsTXTextControlEnabled = oGetStandardWordingTemplateRequest.IsTXTextControlEnabled
            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetStandardWordingTemplate(oImpRequest)

                ' Return back the Byte array to front End
                oResponse.DocumentTemplate = oImpResponse.DocumentTemplate
                oResponse.MergedFilePath = oImpResponse.MergedFilePath

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetStandardWordingTemplateRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetStandardWordingTemplateRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>  
    ''' This web services method is used to Get Taxes.
    ''' </summary>  
    ''' <param name="GetTaxesRequest">An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetTaxesRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetTaxesResponseType</returns>  
    Public Function GetTaxes(ByVal GetTaxesRequest As GetTaxesRequestType) As GetTaxesResponseType Implements IPurePolicyService.GetTaxes


        Try


            Dim sUserName As String = GetTaxesRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGETTAX", iUserId)
            CommonFunctions.CheckSecurityToken(GetTaxesRequest.WCFSecurityToken)
            Dim oResponse As New GetTaxesResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetTaxesRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetTaxesRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetTaxesResponseType = Nothing

            ' Pass the values to the implementation request structure            
            oImpRequest.BranchCode = GetTaxesRequest.BranchCode
            oImpRequest.InsuranceFileKey = GetTaxesRequest.InsuranceFileKey
            oImpRequest.WCFSecurityToken = If(GetTaxesRequest.WCFSecurityToken.Length > 0, GetTaxesRequest.WCFSecurityToken, "WCFSecurityToken")

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetTaxes(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.Row = SAMFunc.GetDeserializedValues(Of List(Of BaseGetTaxesResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetTaxesResponseType", sConvertToTypeName:="BaseGetTaxesResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Row = DataTabletoList_GetTaxes(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetTaxesRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetTaxesRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="LapseRenewalRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function LapseRenewal(ByVal LapseRenewalRequest As LapseRenewalRequestType) As LapseRenewalResponseType Implements IPurePolicyService.LapseRenewal
        Try

            Dim sUserName As String = LapseRenewalRequest.LoginUserName
            Dim agentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, agentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMFIFFCLM", iUserId)
            CommonFunctions.CheckSecurityToken(LapseRenewalRequest.WCFSecurityToken)
            Dim response As New LapseRenewalResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, LapseRenewalRequest.BranchCode)
            ' Implementation structures
            Dim impRequest As New SAMForInsuranceV2ImplementationTypes.LapseRenewalRequestType
            Dim impResponse As SAMForInsuranceV2ImplementationTypes.LapseRenewalResponseType = Nothing
            ' Pass the values to the implementation request structure
            impRequest.AgentKey = agentKey
            impRequest.BranchCode = LapseRenewalRequest.BranchCode
            impRequest.InsuranceFileKey = LapseRenewalRequest.InsuranceFileKey
            impRequest.QuoteTimeStamp = LapseRenewalRequest.QuoteTimeStamp
            impRequest.LapseReasonCode = LapseRenewalRequest.LapseReasonCode
            Try
                ' Call the implementation method
                impResponse = oBusiness.LapseRenewal(impRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(impResponse.STSError)
                If Not impResponse Is Nothing Then
                    response.QuoteTimeStamp = impResponse.QuoteTimeStamp
                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(impResponse, response, ex, CommonFunctions.CreateDictionary(LapseRenewalRequest))
            End Try
            Return response
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(LapseRenewalRequest))
            Return Nothing
        End Try
    End Function


    Public Sub RunRenewalAccept(ByVal RunRenewalAcceptRequest As RunRenewalAcceptRequestType) Implements IPurePolicyService.RunRenewalAccept
        Try

            Dim sUserName As String = RunRenewalAcceptRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHRKey", iUserId)
            CommonFunctions.CheckSecurityToken(RunRenewalAcceptRequest.WCFSecurityToken)
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, RunRenewalAcceptRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.RunRenewalAcceptRequestType

            ' Pass the values to the implementation request structure
            oImpRequest.InsuranceFileKey = RunRenewalAcceptRequest.InsuranceFileKey
            oImpRequest.BatchRenewalJobKey = RunRenewalAcceptRequest.BatchRenewalJobKey

            ' Removed RenewalDocsDestination field from request type and added 
            ' RecordsCount,GUID as specified in "Renewal Changes.doc" sent by Rahul
            oImpRequest.RecordsCount = RunRenewalAcceptRequest.RecordsCount
            oImpRequest.GUID = RunRenewalAcceptRequest.GUID

            ' Call the implementation method
            oBusiness.RunRenewalAccept(oImpRequest)

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(RunRenewalAcceptRequest))
        End Try
    End Sub


    Public Sub RunRenewalSelection(ByVal RunRenewalSelectionRequest As RunRenewalSelectionRequestType) Implements IPurePolicyService.RunRenewalSelection

        Try
            Dim sUserName As String = RunRenewalSelectionRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHRKey", iUserId)
            CommonFunctions.CheckSecurityToken(RunRenewalSelectionRequest.WCFSecurityToken)
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, RunRenewalSelectionRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.RunRenewalSelectionRequestType

            ' Pass the values to the implementation request structure
            oImpRequest.InsuranceFileKey = RunRenewalSelectionRequest.InsuranceFileKey
            oImpRequest.BatchRenewalJobKey = RunRenewalSelectionRequest.BatchRenewalJobKey

            ' Removed IsProcessStart, GenerateReport, ReportSortOrder fields from request type and added
            ' RecordsCount,GUID as specified in "Renewal Changes.doc" sent by Rahul 
            oImpRequest.RecordsCount = RunRenewalSelectionRequest.RecordsCount
            oImpRequest.GUID = RunRenewalSelectionRequest.GUID

            ' Call the implementation method
            oBusiness.RunRenewalSelection(oImpRequest)

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(RunRenewalSelectionRequest))
        End Try

    End Sub
    Public Function RunRenewalSelectionByPolicy(ByVal RunRenewalSelectionByPolicyRequest As RunRenewalSelectionByPolicyRequestType) As RunRenewalSelectionByPolicyResponseType Implements IPurePolicyService.RunRenewalSelectionByPolicy

        Try
            Dim sUserName As String = RunRenewalSelectionByPolicyRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMPRSBP", iUserId)
            CommonFunctions.CheckSecurityToken(RunRenewalSelectionByPolicyRequest.WCFSecurityToken)
            Dim oResponse As New RunRenewalSelectionByPolicyResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, RunRenewalSelectionByPolicyRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.RunRenewalSelectionByPolicyRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.RunRenewalSelectionByPolicyResponseType

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = RunRenewalSelectionByPolicyRequest.BranchCode
            oImpRequest.InsuranceFileKey = RunRenewalSelectionByPolicyRequest.InsuranceFileKey
            oImpRequest.SetRenewalInviteAsSent = RunRenewalSelectionByPolicyRequest.SetRenewalInviteAsSent
            oImpRequest.CreatedById = iUserId

            Try
                ' Call the implementation method
                oImpResponse = New SAMForInsuranceV2ImplementationTypes.RunRenewalSelectionByPolicyResponseType
                oImpResponse = oBusiness.RunRenewalSelectionByPolicy(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                oResponse.RenewalInsuranceFileKey = oImpResponse.RenewalInsuranceFileKey
                oResponse.QuoteTimeStamp = oImpResponse.QuoteTimeStamp

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(RunRenewalSelectionByPolicyRequest))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(RunRenewalSelectionByPolicyRequest))
            Return Nothing
        End Try
    End Function

    Public Function RunValidationRules(ByVal RunValidationRulesRequest As RunValidationRulesRequestType) As RunValidationRulesResponseType Implements IPurePolicyService.RunValidationRules, IPureClaimService.RunValidationRules

        Try
            Dim sUserName As String = RunValidationRulesRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMRDfRulE", iUserId)
            CommonFunctions.CheckSecurityToken(RunValidationRulesRequest.WCFSecurityToken)

            Dim oResponse As New RunValidationRulesResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, RunValidationRulesRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.RunValidationRulesRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.RunValidationRulesResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = RunValidationRulesRequest.BranchCode
            oImpRequest.ScreenCode = RunValidationRulesRequest.ScreenCode
            oImpRequest.XMLDataSet = SAMFunc.TransformDatasetSAMtoPB(RunValidationRulesRequest.XMLDataSet)

            oImpRequest.ClaimKeySpecified = RunValidationRulesRequest.ClaimKeySpecified
            oImpRequest.ClaimPerilKeySpecified = RunValidationRulesRequest.ClaimPerilKeySpecified

            If RunValidationRulesRequest.ClaimKeySpecified Then
                oImpRequest.ClaimKey = RunValidationRulesRequest.ClaimKey
            End If

            If RunValidationRulesRequest.ClaimPerilKeySpecified Then
                oImpRequest.ClaimPerilKey = RunValidationRulesRequest.ClaimPerilKey
            End If
            oImpRequest.TransactionType = RunValidationRulesRequest.TransactionType
            oImpRequest.SkipSaveToDB = RunValidationRulesRequest.SkipSaveToDB

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.RunValidationRules(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.XMLDataset = SAMFunc.TransformDatasetPBtoSAM(oImpResponse.XMLDataset, Nothing, RunValidationRulesRequest.SkipSaveToDB, sCalledVia:="SKIP")

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(RunValidationRulesRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(RunValidationRulesRequest))
            Return Nothing
        End Try

    End Function

    Public Function SaveRisk(ByVal SaveRiskRequest As SaveRiskRequestType) As SaveRiskResponseType Implements IPurePolicyService.SaveRisk

        Try


            Dim sUserName As String = SaveRiskRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMURsk", iUserId)
            CommonFunctions.CheckSecurityToken(SaveRiskRequest.WCFSecurityToken)
            Dim oResponse As New SaveRiskResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, SaveRiskRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.SaveRiskRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.SaveRiskResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = SaveRiskRequest.BranchCode
            oImpRequest.InsuranceFileKey = SaveRiskRequest.InsuranceFileKey
            oImpRequest.InsuranceFolderKey = SaveRiskRequest.InsuranceFolderKey
            oImpRequest.QuoteTimeStamp = SaveRiskRequest.QuoteTimeStamp
            oImpRequest.RiskKey = SaveRiskRequest.RiskKey
            oImpRequest.XMLDataSet = SAMFunc.TransformDatasetSAMtoPB(SaveRiskRequest.XMLDataSet)

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.SaveRisk(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.QuoteTimeStamp = oImpResponse.QuoteTimeStamp
                oResponse.XMLDataSet = SAMFunc.TransformDatasetPBtoSAM(oImpResponse.XMLDataSet, sCalledVia:="SaveRisk", iRiskTypeID:=oImpResponse.RiskTypeID, iSourceID:=oImpResponse.BranchID, bRemoveAllSW:=oImpRequest.RemoveAllSW)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(SaveRiskRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(SaveRiskRequest))
            Return Nothing
        End Try

    End Function


    Public Function UpdateAgentCommission(ByVal oUpdateAgentCommissionRequest As UpdateAgentCommissionRequestType) As UpdateAgentCommissionResponseType Implements IPurePolicyService.UpdateAgentCommission
        Try

            Dim sUserName As String = oUpdateAgentCommissionRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMUPAGCOM", iUserId)
            CommonFunctions.CheckSecurityToken(oUpdateAgentCommissionRequest.WCFSecurityToken)

            Dim oResponse As New UpdateAgentCommissionResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oUpdateAgentCommissionRequest.BranchCode)
            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateAgentCommissionRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateAgentCommissionResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oUpdateAgentCommissionRequest.BranchCode
            oImpRequest.InsuranceFileKey = oUpdateAgentCommissionRequest.InsuranceFileKey
            If oUpdateAgentCommissionRequest.AgentCommission IsNot Nothing AndAlso oUpdateAgentCommissionRequest.AgentCommission.Count > 0 Then
                Dim iLength As Int32 = oUpdateAgentCommissionRequest.AgentCommission.Count
                oImpRequest.AgentCommission = New BaseImplementationTypes.BaseUpdateAgentCommissionRequestTypeAgentCommission
                For icount As Integer = 0 To iLength - 1
                    ReDim Preserve oImpRequest.AgentCommission.Row(icount)
                    oImpRequest.AgentCommission.Row(icount) = New BaseImplementationTypes.BaseUpdateAgentCommissionRequestTypeAgentCommissionRow
                    oImpRequest.AgentCommission.Row(icount).Agent = oUpdateAgentCommissionRequest.AgentCommission(icount).Agent
                    oImpRequest.AgentCommission.Row(icount).CalculatedCommissionValue = oUpdateAgentCommissionRequest.AgentCommission(icount).CalculatedCommissionValue
                    oImpRequest.AgentCommission.Row(icount).CalculatedCommissionValueSpecified = oUpdateAgentCommissionRequest.AgentCommission(icount).CalculatedCommissionValueSpecified
                    oImpRequest.AgentCommission.Row(icount).CommissionBand = oUpdateAgentCommissionRequest.AgentCommission(icount).CommissionBand
                    oImpRequest.AgentCommission.Row(icount).CommissionRate = oUpdateAgentCommissionRequest.AgentCommission(icount).CommissionRate
                    oImpRequest.AgentCommission.Row(icount).CommissionValue = oUpdateAgentCommissionRequest.AgentCommission(icount).CommissionValue
                    oImpRequest.AgentCommission.Row(icount).IsAmended = oUpdateAgentCommissionRequest.AgentCommission(icount).IsAmended
                    oImpRequest.AgentCommission.Row(icount).IsLeadAgent = oUpdateAgentCommissionRequest.AgentCommission(icount).IsLeadAgent
                    oImpRequest.AgentCommission.Row(icount).OverRideReason = oUpdateAgentCommissionRequest.AgentCommission(icount).OverRideReason
                    oImpRequest.AgentCommission.Row(icount).Premium = oUpdateAgentCommissionRequest.AgentCommission(icount).Premium
                    oImpRequest.AgentCommission.Row(icount).RiskType = oUpdateAgentCommissionRequest.AgentCommission(icount).RiskType
                    oImpRequest.AgentCommission.Row(icount).TaxGroupCode = oUpdateAgentCommissionRequest.AgentCommission(icount).TaxGroupCode
                    oImpRequest.AgentCommission.Row(icount).IsValue = oUpdateAgentCommissionRequest.AgentCommission(icount).IsValue
                    oImpRequest.AgentCommission.Row(icount).MaximumRate = oUpdateAgentCommissionRequest.AgentCommission(icount).MaximumRate
                    oImpRequest.AgentCommission.Row(icount).IsTaxAmended = oUpdateAgentCommissionRequest.AgentCommission(icount).IsTaxAmended
                    oImpRequest.AgentCommission.Row(icount).AmendedTaxValue = oUpdateAgentCommissionRequest.AgentCommission(icount).AmendedTaxValue
                    oImpRequest.AgentCommission.Row(icount).PerilType = oUpdateAgentCommissionRequest.AgentCommission(icount).PerilType
                Next
            End If
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateAgentCommission(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                If oImpResponse.AgentCommission IsNot Nothing AndAlso oImpResponse.AgentCommission.Row IsNot Nothing Then
                    With oResponse
                        .AgentCommission = oImpResponse.AgentCommission.Row.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseAgentCommissionResponseTypeAgentCommissionRow, BaseAgentCommissionResponseTypeRow)(AddressOf CommonFunctions.ToServiceAgentCommissionList))
                    End With
                End If

                ' Retrieve the values from the implementation response structure
                oResponse.LeadAgentNet = oImpResponse.LeadAgentNet
                oResponse.LeadAgentTotalCommission = oImpResponse.LeadAgentTotalCommission
                oResponse.LeadAgentTotalTax = oImpResponse.LeadAgentTotalTax
                oResponse.SubAgentNet = oImpResponse.SubAgentNet
                oResponse.SubAgentTotalCommission = oImpResponse.SubAgentTotalCommission
                oResponse.SubAgentTotalTax = oImpResponse.SubAgentTotalTax
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdateAgentCommissionRequest))
            End Try
            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateAgentCommissionRequest))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' UpdateQuoteV2
    ''' </summary>
    ''' <param name="oUpdateQuoteV2Request"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateQuoteV2(ByVal oUpdateQuoteV2Request As UpdateQuoteV2RequestType) As UpdateQuoteV2ResponseType Implements IPurePolicyService.UpdateQuoteV2

        Try

            Dim sUserName As String = oUpdateQuoteV2Request.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMAQuot", iUserId)
            CommonFunctions.CheckSecurityToken(oUpdateQuoteV2Request.WCFSecurityToken)
            Dim oResponse As New UpdateQuoteV2ResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oUpdateQuoteV2Request.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateQuoteV2RequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateQuoteV2ResponseType = Nothing

            With oUpdateQuoteV2Request
                oImpRequest.BranchCode = .BranchCode
                oImpRequest.CoverStartDate = .CoverStartDate
                oImpRequest.CoverEndDate = .CoverEndDate
                oImpRequest.ProductCode = .ProductCode
                oImpRequest.Description = .Description
                oImpRequest.QuoteRef = .QuoteRef
                oImpRequest.InsuredName = .InsuredName
                oImpRequest.CurrencyCode = .CurrencyCode
                oImpRequest.AgentKeySpecified = oUpdateQuoteV2Request.AgentKeySpecified
                If iAgentKey <> 0 And .AgentKeySpecified = False Then
                    oImpRequest.AgentKeySpecified = True
                    oImpRequest.AgentKey = iAgentKey
                Else
                    oImpRequest.AgentKey = .AgentKey
                    oImpRequest.AgentKeySpecified = True
                End If
                oImpRequest.AnalysisCode = .AnalysisCode
                oImpRequest.AlternativeRef = .AlternativeRef
                oImpRequest.Timestamp = .Timestamp
                oImpRequest.InsuranceFolderKey = .InsuranceFolderKey
                oImpRequest.InsuranceFileKey = .InsuranceFileKey
                oImpRequest.PartyKey = .PartyKey
                oImpRequest.SubBranchCode = .SubBranchCode
                oImpRequest.ConsolidatedLeadAgentCommissionSpecified = .ConsolidatedLeadAgentCommissionSpecified
                oImpRequest.ConsolidatedLeadAgentCommission = .ConsolidatedLeadAgentCommission
                oImpRequest.ConsolidatedSubAgentCommissionSpecified = .ConsolidatedSubAgentCommissionSpecified
                oImpRequest.ConsolidatedSubAgentCommission = .ConsolidatedSubAgentCommission
                oImpRequest.CoverNoteBookNumber = .CoverNoteBookNumber
                oImpRequest.CoverNoteSheetNumberSpecified = .CoverNoteSheetNumberSpecified
                oImpRequest.CoverNoteSheetNumber = .CoverNoteSheetNumber
                oImpRequest.BusinessTypeCode = .BusinessTypeCode
                oImpRequest.QuoteExpiryDate = .QuoteExpiryDate
                oImpRequest.HandlerCode = .HandlerCode
                oImpRequest.Regarding = .Regarding
                oImpRequest.PolicyStatusCode = .PolicyStatusCode
                oImpRequest.InceptionDate = .InceptionDate
                oImpRequest.RenewalDate = .RenewalDate
                oImpRequest.InceptionTPI = .InceptionTPI
                oImpRequest.IssuedDateSpecified = .IssuedDateSpecified
                If Not IsDate(.IssuedDate) OrElse .IssuedDate = DateTime.MinValue Then
                    .IssuedDate = #12/29/1899#
                End If
                oImpRequest.IssuedDate = .IssuedDate
                oImpRequest.ProposalDateSpecified = .ProposalDateSpecified
                oImpRequest.ProposalDate = .ProposalDate
                oImpRequest.FrequencyCode = .FrequencyCode
                oImpRequest.RenewalMethodCode = .RenewalMethodCode
                oImpRequest.LapseCancelReasonCode = .LapseCancelReasonCode
                oImpRequest.LTUExpiryDateSpecified = .LTUExpiryDateSpecified
                If Not IsDate(.LTUExpiryDate) OrElse .LTUExpiryDate = DateTime.MinValue Then
                    .LTUExpiryDate = #12/29/1899#
                End If
                oImpRequest.LTUExpiryDate = .LTUExpiryDate
                oImpRequest.StopReasonCode = .StopReasonCode
                oImpRequest.LapseCancelDateSpecified = .LapseCancelDateSpecified
                If Not IsDate(.LapseCancelDate) OrElse .LapseCancelDate = DateTime.MinValue Then
                    .LapseCancelDate = #12/29/1899#
                End If
                oImpRequest.LapseCancelDate = .LapseCancelDate
                oImpRequest.ReferredAtRenewalSpecified = .ReferredAtRenewalSpecified
                oImpRequest.ReferredAtRenewal = .ReferredAtRenewal
                oImpRequest.ReferredAtMTASpecified = .ReferredAtMTASpecified
                oImpRequest.ReferredAtMTA = .ReferredAtMTA
                oImpRequest.PaymentMethod = .PaymentMethod
                oImpRequest.MarkedForCollection = .MarkedForCollection
                oImpRequest.MarkedForCollectionSpecified = .MarkedForCollectionSpecified
                If Not IsDate(.MarkedDate) OrElse .MarkedDate = DateTime.MinValue Then
                    .MarkedDate = #12/29/1899#
                End If
                oImpRequest.MarkedDate = .MarkedDate
                oImpRequest.MarkedDateSpecified = .MarkedDateSpecified
                oImpRequest.PutOnNextInstalmentRenewal = .PutOnNextInstalmentRenewal
                oImpRequest.PutOnNextInstalmentRenewalSpecified = .PutOnNextInstalmentRenewalSpecified
                oImpRequest.RenewalDayNo = .RenewalDayNo
                oImpRequest.RenewalDayNoSpecified = .RenewalDayNoSpecified
                oImpRequest.ContactuserKey = .ContactuserKey
                oImpRequest.ContactuserKeySpecified = .ContactuserKeySpecified
                oImpRequest.UnderwritingYearId = .UnderwritingYearId
                oImpRequest.UnderwritingYearIdSpecified = .UnderwritingYearIdSpecified
                oImpRequest.ContactuserKey = .ContactuserKey
                oImpRequest.ContactuserKeySpecified = .ContactuserKeySpecified
                oImpRequest.IsMarketPlacePolicy = .IsMarketPlacePolicy
                oImpRequest.collectionFrequency = .CollectionFrequency
                oImpRequest.collectionFrequencySpecified = .CollectionFrequencySpecified
                oImpRequest.paymentTerms = .PaymentTerms
                oImpRequest.paymentTermsSpecified = .PaymentTermsSpecified
                oImpRequest.CoInsurancePlacement = .CoInsurancePlacement
                oImpRequest.CorrespondenceType = .CorrespondenceType
                oImpRequest.DefaultPreferredCorrespondence = .DefaultPreferredCorrespondence
                oImpRequest.IsAgentReceiveCorrespondence = .IsAgentReceiveCorrespondence
                oImpRequest.AnniversaryDate = .AnniversaryDate
                oImpRequest.AnniversaryDateSpecified = .AnniversaryDateSpecified
                oImpRequest.OldPolicyNumber = .OldPolicyNumber
                oImpRequest.SenderEmail = .SenderEmail
                oImpRequest.ReceiverEmail = .ReceiverEmail
            End With

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateQuoteV2(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.TimeStamp = oImpResponse.TimeStamp

                oResponse.InsuranceFileRef = oImpResponse.InsuranceFileRef
                oResponse.QuoteVersion = oImpResponse.QuoteVersion
                oResponse.BaseInsuranceFolderKey = oImpResponse.BaseInsuranceFolderKey
                oResponse.QuoteStatusKey = CType([Enum].ToObject(GetType(BaseImplementationTypes.QuoteStatusType), oImpResponse.QuoteStatusKey), QuoteStatusType)
                oResponse.QuoteExpiryDate = oImpResponse.QuoteExpiryDate
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdateQuoteV2Request))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateQuoteV2Request))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oUpdateQuoteStatusRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateQuoteStatus(ByVal oUpdateQuoteStatusRequest As UpdateQuoteStatusRequestType) As UpdateQuoteStatusResponseType Implements IPurePolicyService.UpdateQuoteStatus

        Try

            Dim sUserName As String = oUpdateQuoteStatusRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMUPDQST", iUserId)
            CommonFunctions.CheckSecurityToken(oUpdateQuoteStatusRequest.WCFSecurityToken)
            Dim oResponse As New UpdateQuoteStatusResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oUpdateQuoteStatusRequest.BranchCode)
            Dim iCount As Integer = 0
            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateQuoteStatusRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateQuoteStatusResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oUpdateQuoteStatusRequest.BranchCode
            oImpRequest.InsuranceFileKey = oUpdateQuoteStatusRequest.InsuranceFileKey
            oImpRequest.QuoteStatusKey = CType([Enum].ToObject(GetType(QuoteStatusType), oUpdateQuoteStatusRequest.QuoteStatusKey), BaseImplementationTypes.QuoteStatusType)

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateQuoteStatus(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.InsuranceFileKey = oImpResponse.InsuranceFileKey
                oResponse.QuoteStatusKey = CType([Enum].ToObject(GetType(BaseImplementationTypes.QuoteStatusType), oImpResponse.QuoteStatusKey), QuoteStatusType)


            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdateQuoteStatusRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateQuoteStatusRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oUpdateQuotePaymentMethodRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateQuotePaymentMethod(ByVal oUpdateQuotePaymentMethodRequest As UpdateQuotePaymentMethodRequestType) As UpdateQuotePaymentMethodResponseType Implements IPureCoreService.UpdateQuotePaymentMethod

        Try

            Dim sUserName As String = oUpdateQuotePaymentMethodRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMUPAYM", iUserId)
            CommonFunctions.CheckSecurityToken(oUpdateQuotePaymentMethodRequest.WCFSecurityToken)
            Dim oUpdateQuotePaymentMethodResponse As New UpdateQuotePaymentMethodResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness()

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateQuotePaymentMethodRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateQuotePaymentMethodResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oUpdateQuotePaymentMethodRequest.BranchCode
            oImpRequest.InsuranceFileKey = oUpdateQuotePaymentMethodRequest.InsuranceFileKey
            oImpRequest.PaymentMethod = CType(oUpdateQuotePaymentMethodRequest.PaymentMethod, BaseImplementationTypes.PaymentMethodType)
            oImpRequest.QuoteTimeStamp = oUpdateQuotePaymentMethodRequest.QuoteTimeStamp

            If oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote IsNot Nothing Then
                oImpRequest.SelectedInstalmentQuote = New BaseImplementationTypes.BaseSelectedInstalmentQuoteType
                oImpRequest.SelectedInstalmentQuote.BankAccountName = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.BankAccountName
                oImpRequest.SelectedInstalmentQuote.BankAccountNo = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.BankAccountNo
                oImpRequest.SelectedInstalmentQuote.BankSortCode = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.BankSortCode
                oImpRequest.SelectedInstalmentQuote.BIC = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.BIC
                oImpRequest.SelectedInstalmentQuote.IBAN = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.IBAN
                If oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote IsNot Nothing Then
                    If oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.BankAddress IsNot Nothing Then
                        oImpRequest.SelectedInstalmentQuote.BankAddress = New BaseImplementationTypes.BaseAddressType
                        oImpRequest.SelectedInstalmentQuote.BankAddress.AddressLine1 = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.BankAddress.AddressLine1
                        oImpRequest.SelectedInstalmentQuote.BankAddress.AddressLine2 = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.BankAddress.AddressLine2
                        oImpRequest.SelectedInstalmentQuote.BankAddress.AddressLine3 = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.BankAddress.AddressLine3
                        oImpRequest.SelectedInstalmentQuote.BankAddress.AddressLine4 = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.BankAddress.AddressLine4
                        oImpRequest.SelectedInstalmentQuote.BankAddress.AddressTypeCode = CType(oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.BankAddress.AddressTypeCode, BaseImplementationTypes.AddressTypeType)
                        oImpRequest.SelectedInstalmentQuote.BankAddress.CountryCode = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.BankAddress.CountryCode
                        oImpRequest.SelectedInstalmentQuote.BankAddress.PostCode = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.BankAddress.PostCode
                    End If
                    oImpRequest.SelectedInstalmentQuote.BankAreaCode = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.BankAreaCode
                    oImpRequest.SelectedInstalmentQuote.BankBranch = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.BankBranch
                    oImpRequest.SelectedInstalmentQuote.BankExtn = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.BankExtn
                    oImpRequest.SelectedInstalmentQuote.BankFax = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.BankFax
                    oImpRequest.SelectedInstalmentQuote.BankFaxCode = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.BankFaxCode
                    oImpRequest.SelectedInstalmentQuote.BankName = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.BankName
                    oImpRequest.SelectedInstalmentQuote.BankPhone = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.BankPhone
                    oImpRequest.SelectedInstalmentQuote.SelectedSchemeNo = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.SelectedSchemeNo
                    oImpRequest.SelectedInstalmentQuote.SelectedSchemeVersion = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.SelectedSchemeVersion
                    oImpRequest.SelectedInstalmentQuote.QuoteDate = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.QuoteDate.Date
                    oImpRequest.SelectedInstalmentQuote.StartDate = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.StartDate.Date
                    oImpRequest.SelectedInstalmentQuote.EndDate = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.EndDate.Date
                    oImpRequest.SelectedInstalmentQuote.PreferredDate = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.PreferredDate.Date
                    oImpRequest.SelectedInstalmentQuote.MonthDay = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.MonthDay
                    oImpRequest.SelectedInstalmentQuote.WeekDay = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.WeekDay
                    oImpRequest.SelectedInstalmentQuote.AmountToFinance = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.AmountToFinance
                    oImpRequest.SelectedInstalmentQuote.PaymentProtection = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.PaymentProtection
                    oImpRequest.SelectedInstalmentQuote.OverrideRate = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.OverrideRate
                    oImpRequest.SelectedInstalmentQuote.OverrideInterestRate = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.OverrideInterestRate
                    oImpRequest.SelectedInstalmentQuote.AmountPaid = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.AmountPaid
                    oImpRequest.SelectedInstalmentQuote.PFRF_ID = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.PFRF_ID

                    If oImpRequest.PaymentMethod = BaseImplementationTypes.PaymentMethodType.CreditCard AndAlso
                                oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.CreditCard IsNot Nothing Then

                        oImpRequest.SelectedInstalmentQuote.CreditCard = New BaseImplementationTypes.BaseCreditCardType

                        oImpRequest.SelectedInstalmentQuote.CreditCard.ExpiryDate = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.CreditCard.ExpiryDate
                        oImpRequest.SelectedInstalmentQuote.CreditCard.Issue = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.CreditCard.Issue
                        oImpRequest.SelectedInstalmentQuote.CreditCard.NameOnCreditCard = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.CreditCard.NameOnCreditCard
                        oImpRequest.SelectedInstalmentQuote.CreditCard.Number = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.CreditCard.Number
                        oImpRequest.SelectedInstalmentQuote.CreditCard.Pin = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.CreditCard.Pin
                        oImpRequest.SelectedInstalmentQuote.CreditCard.StartDate = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.CreditCard.StartDate
                        oImpRequest.SelectedInstalmentQuote.CreditCard.TypeCode = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.CreditCard.TypeCode
                        oImpRequest.SelectedInstalmentQuote.CreditCard.AuthCode = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.CreditCard.AuthCode
                        oImpRequest.SelectedInstalmentQuote.CreditCard.TrackingNumber = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.CreditCard.TrackingNumber
                        oImpRequest.SelectedInstalmentQuote.CreditCard.AccountType = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.CreditCard.AccountType
                        oImpRequest.SelectedInstalmentQuote.CreditCard.PartyBankKey = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.CreditCard.PartyBankKey
                        ' retrieve the credit card cardholder details if provided
                        If oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.CreditCard.CardHolder IsNot Nothing Then
                            oImpRequest.SelectedInstalmentQuote.CreditCard.CardHolder = New BaseImplementationTypes.BaseCreditCardTypeCardHolder
                            oImpRequest.SelectedInstalmentQuote.CreditCard.CardHolder.Name = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.CreditCard.CardHolder.Name
                            oImpRequest.SelectedInstalmentQuote.CreditCard.CardHolder.AddressLine1 = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.CreditCard.CardHolder.AddressLine1
                            oImpRequest.SelectedInstalmentQuote.CreditCard.CardHolder.AddressLine2 = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.CreditCard.CardHolder.AddressLine2
                            oImpRequest.SelectedInstalmentQuote.CreditCard.CardHolder.AddressLine3 = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.CreditCard.CardHolder.AddressLine3
                            oImpRequest.SelectedInstalmentQuote.CreditCard.CardHolder.AddressLine4 = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.CreditCard.CardHolder.AddressLine4
                            oImpRequest.SelectedInstalmentQuote.CreditCard.CardHolder.CountryCode = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.CreditCard.CardHolder.CountryCode
                            oImpRequest.SelectedInstalmentQuote.CreditCard.CardHolder.PostCode = oUpdateQuotePaymentMethodRequest.SelectedInstalmentQuote.CreditCard.CardHolder.PostCode
                        End If

                    End If

                End If
            End If

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.UpdateQuotePaymentMethod(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oUpdateQuotePaymentMethodResponse.QuoteTimeStamp = oImpResponse.QuoteTimeStamp

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oUpdateQuotePaymentMethodResponse, ex, CommonFunctions.CreateDictionary(oUpdateQuotePaymentMethodRequest))
            End Try

            Return oUpdateQuotePaymentMethodResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateQuotePaymentMethodRequest))
            Return Nothing
        End Try
    End Function

    Public Function UpdateQuote(ByVal UpdateQuoteRequest As UpdateQuoteRequestType) As UpdateQuoteResponseType Implements IPurePolicyService.UpdateQuote

        Try

            Dim sUserName As String = UpdateQuoteRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMUQuot", iUserId)
            CommonFunctions.CheckSecurityToken(UpdateQuoteRequest.WCFSecurityToken)
            Dim oResponse As New UpdateQuoteResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, UpdateQuoteRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateQuoteRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateQuoteResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = UpdateQuoteRequest.BranchCode
            oImpRequest.CoverEndDate = UpdateQuoteRequest.CoverEndDate
            oImpRequest.CoverStartDate = UpdateQuoteRequest.CoverStartDate
            oImpRequest.Description = UpdateQuoteRequest.Description
            oImpRequest.InsuranceFileKey = UpdateQuoteRequest.InsuranceFileKey
            oImpRequest.InsuranceFolderKey = UpdateQuoteRequest.InsuranceFolderKey
            oImpRequest.InsuredParties = UpdateQuoteRequest.InsuredParties
            oImpRequest.QuoteTimeStamp = UpdateQuoteRequest.QuoteTimeStamp
            oImpRequest.UserName = sUserName
            oImpRequest.AnalysisCode = UpdateQuoteRequest.AnalysisCode
            oImpRequest.ConsolidatedLeadAgentCommission = UpdateQuoteRequest.ConsolidatedLeadAgentCommission
            oImpRequest.ConsolidatedLeadAgentCommissionSpecified = UpdateQuoteRequest.ConsolidatedLeadAgentCommissionSpecified
            oImpRequest.ConsolidatedSubAgentCommission = UpdateQuoteRequest.ConsolidatedSubAgentCommission
            oImpRequest.ConsolidatedSubAgentCommissionSpecified = UpdateQuoteRequest.ConsolidatedSubAgentCommissionSpecified
            oImpRequest.CoverNoteBookNumber = UpdateQuoteRequest.CoverNoteBookNumber
            oImpRequest.CoverNoteSheetNumber = UpdateQuoteRequest.CoverNoteSheetNumber
            oImpRequest.CoverNoteSheetNumberSpecified = UpdateQuoteRequest.CoverNoteSheetNumberSpecified
            oImpRequest.AlternativeRef = UpdateQuoteRequest.AlternativeRef

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateQuote(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.QuoteTimeStamp = oImpResponse.QuoteTimeStamp

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(UpdateQuoteRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(UpdateQuoteRequest))
            Return Nothing
        End Try

    End Function

    Public Function UpdateRatingSections(ByVal UpdateRatingDetailsRequest As UpdateRatingDetailsRequestType) As UpdateRatingDetailsResponseType Implements IPurePolicyService.UpdateRatingSections

        Try

            Dim sUserName As String = UpdateRatingDetailsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMURatSec", iUserId)
            CommonFunctions.CheckSecurityToken(UpdateRatingDetailsRequest.WCFSecurityToken)
            Dim oResponse As New UpdateRatingDetailsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, UpdateRatingDetailsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateRatingDetailsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateRatingDetailsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = UpdateRatingDetailsRequest.BranchCode
            oImpRequest.InsuranceFileKey = UpdateRatingDetailsRequest.InsuranceFileKey
            oImpRequest.RiskKey = UpdateRatingDetailsRequest.RiskKey
            oImpRequest.TransactionType = UpdateRatingDetailsRequest.TransactionType
            oImpRequest.TimeStamp = UpdateRatingDetailsRequest.TimeStamp

            If UpdateRatingDetailsRequest.RatingDetails IsNot Nothing Then
                If UpdateRatingDetailsRequest.RatingDetails.Count <> 0 Then
                    Dim iLength As Int32 = UpdateRatingDetailsRequest.RatingDetails.Count
                    oImpRequest.RatingDetails = New BaseImplementationTypes.BaseUpdateRatingDetailsRequestTypeRatingDetails
                    For icount As Integer = 0 To iLength - 1
                        ReDim Preserve oImpRequest.RatingDetails.Row(icount)
                        oImpRequest.RatingDetails.Row(icount) = New BaseImplementationTypes.BaseUpdateRatingDetailsRequestTypeRatingDetailsRow
                        oImpRequest.RatingDetails.Row(icount).RatingSectionTypeCode = UpdateRatingDetailsRequest.RatingDetails(icount).RatingSectionTypeCode
                        oImpRequest.RatingDetails.Row(icount).EarningPatternCode = UpdateRatingDetailsRequest.RatingDetails(icount).EarningPatternCode
                        oImpRequest.RatingDetails.Row(icount).RateTypeCode = UpdateRatingDetailsRequest.RatingDetails(icount).RateTypeCode
                        oImpRequest.RatingDetails.Row(icount).AnnualRate = UpdateRatingDetailsRequest.RatingDetails(icount).AnnualRate
                        oImpRequest.RatingDetails.Row(icount).SumInsured = UpdateRatingDetailsRequest.RatingDetails(icount).SumInsured
                        oImpRequest.RatingDetails.Row(icount).AnnualPremium = UpdateRatingDetailsRequest.RatingDetails(icount).AnnualPremium
                        oImpRequest.RatingDetails.Row(icount).ThisPremium = UpdateRatingDetailsRequest.RatingDetails(icount).ThisPremium
                        oImpRequest.RatingDetails.Row(icount).CountryCode = UpdateRatingDetailsRequest.RatingDetails(icount).CountryCode
                        oImpRequest.RatingDetails.Row(icount).StateCode = UpdateRatingDetailsRequest.RatingDetails(icount).StateCode
                        oImpRequest.RatingDetails.Row(icount).CurrencyCode = UpdateRatingDetailsRequest.RatingDetails(icount).CurrencyCode
                        oImpRequest.RatingDetails.Row(icount).OverrideReason = UpdateRatingDetailsRequest.RatingDetails(icount).OverrideReason
                        oImpRequest.RatingDetails.Row(icount).OriginalFlag = UpdateRatingDetailsRequest.RatingDetails(icount).OriginalFlag
                        oImpRequest.RatingDetails.Row(icount).IsAmended = UpdateRatingDetailsRequest.RatingDetails(icount).IsAmended
                        oImpRequest.RatingDetails.Row(icount).CalculatedPremium = UpdateRatingDetailsRequest.RatingDetails(icount).CalculatedPremium
                    Next
                End If
            End If
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateRatingDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.TimeStamp = oImpResponse.TimeStamp

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(UpdateRatingDetailsRequest))
            End Try
            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(UpdateRatingDetailsRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="UpdateRenewalStatusRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateRenewalStatus(ByVal UpdateRenewalStatusRequest As UpdateRenewalStatusRequestType) _
      As UpdateRenewalStatusResponseType Implements IPurePolicyService.UpdateRenewalStatus

        Try
            'As per discussion with Rahul, the key for CheckAuthority is SAMPRURS


            Dim sUserName As String = UpdateRenewalStatusRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMPRURS", iUserId)
            CommonFunctions.CheckSecurityToken(UpdateRenewalStatusRequest.WCFSecurityToken)
            Dim oResponse As New UpdateRenewalStatusResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, UpdateRenewalStatusRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateRenewalStatusRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateRenewalStatusResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = UpdateRenewalStatusRequest.BranchCode
            oImpRequest.InsuranceFileKey = UpdateRenewalStatusRequest.InsuranceFileKey
            oImpRequest.RenewalStatusCode = UpdateRenewalStatusRequest.RenewalStatusCode
            oImpRequest.QuoteTimeStamp = UpdateRenewalStatusRequest.QuoteTimeStamp
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateRenewalStatus(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.QuoteTimeStamp = oImpResponse.QuoteTimeStamp
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(UpdateRenewalStatusRequest))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(UpdateRenewalStatusRequest))
            Return Nothing
        End Try
    End Function

    ''' <summary>  
    ''' This web services method is used to Update Risk Selection.
    ''' </summary>  
    ''' <param name="oUpdateRiskSelectionRequest">An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.UpdateRiskSelectionRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.UpdateRiskSelectionResponseType</returns>  
    Public Function UpdateRiskSelection(ByVal oUpdateRiskSelectionRequest As UpdateRiskSelectionRequestType) As UpdateRiskSelectionResponseType Implements IPurePolicyService.UpdateRiskSelection

        Try
            Dim sUserName As String = oUpdateRiskSelectionRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMURsk", iUserId)
            CommonFunctions.CheckSecurityToken(oUpdateRiskSelectionRequest.WCFSecurityToken)

            Dim oResponse As New UpdateRiskSelectionResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oUpdateRiskSelectionRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateRiskSelectionRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateRiskSelectionResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oUpdateRiskSelectionRequest.BranchCode
            oImpRequest.InsuranceFileKey = oUpdateRiskSelectionRequest.InsuranceFileKey
            oImpRequest.InsuranceFolderKey = oUpdateRiskSelectionRequest.InsuranceFolderKey
            oImpRequest.TransactionType = oUpdateRiskSelectionRequest.TransactionType.ToString

            oImpRequest.IsSelected = oUpdateRiskSelectionRequest.IsSelected
            oImpRequest.RiskKey = oUpdateRiskSelectionRequest.RiskKey
            oImpRequest.TimeStamp = oUpdateRiskSelectionRequest.TimeStamp

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateRiskSelection(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.TimeStamp = oImpResponse.TimeStamp

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdateRiskSelectionRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateRiskSelectionRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="UpdateRiskRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateRisk(ByVal UpdateRiskRequest As UpdateRiskRequestType) As UpdateRiskResponseType Implements IPurePolicyService.UpdateRisk

        Try

            Dim sUserName As String = UpdateRiskRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            Dim bIgnoreErrorMessage As Boolean = False

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMURsk", iUserId)
            CommonFunctions.CheckSecurityToken(UpdateRiskRequest.WCFSecurityToken)
            Dim oResponse As New UpdateRiskResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, UpdateRiskRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateRiskRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateRiskResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = UpdateRiskRequest.BranchCode
            oImpRequest.InsuranceFileKey = UpdateRiskRequest.InsuranceFileKey
            oImpRequest.InsuranceFolderKey = UpdateRiskRequest.InsuranceFolderKey
            oImpRequest.QuoteTimeStamp = UpdateRiskRequest.QuoteTimeStamp
            oImpRequest.RiskDescription = UpdateRiskRequest.RiskDescription
            oImpRequest.RiskKey = UpdateRiskRequest.RiskKey
            oImpRequest.ScreenCode = UpdateRiskRequest.ScreenCode
            oImpRequest.SubBranchCode = UpdateRiskRequest.SubBranchCode
            oImpRequest.UserName = sUserName
            oImpRequest.XMLDataSet = SAMFunc.TransformDatasetSAMtoPB(UpdateRiskRequest.XMLDataSet)
            oImpRequest.TransactionType = UpdateRiskRequest.TransactionType

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateRisk(oImpRequest)

                'XML Can be updated in error cases
                oResponse.XMLDataSet = SAMFunc.TransformDatasetPBtoSAM(oImpResponse.XMLDataSet)


                If oImpResponse IsNot Nothing Then
                    If oImpResponse.STSError IsNot Nothing Then
                        If oImpResponse.STSError.STSBusinessRule IsNot Nothing Then
                            If (oImpResponse.STSError.STSBusinessRule.Code = Convert.ToString(STSErrorPublisher.STSErrorCodes.ValidationRulesReferred) OrElse
                            oImpResponse.STSError.STSBusinessRule.Code = Convert.ToString(STSErrorPublisher.STSErrorCodes.ValidationRulesDeclined) OrElse
                            oImpResponse.STSError.STSBusinessRule.Code = Convert.ToString(STSErrorPublisher.STSErrorCodes.UALRulesReferred) OrElse
                            oImpResponse.STSError.STSBusinessRule.Code = Convert.ToString(STSErrorPublisher.STSErrorCodes.UALRulesDeclined) OrElse
                            oImpResponse.STSError.STSBusinessRule.Code = Convert.ToString(STSErrorPublisher.STSErrorCodes.RatingRulesReferred) OrElse
                            oImpResponse.STSError.STSBusinessRule.Code = Convert.ToString(STSErrorPublisher.STSErrorCodes.RatingRulesDeclined)) AndAlso UpdateRiskRequest.IgnoreErrorMessage Then
                                bIgnoreErrorMessage = True
                            End If
                            If oImpResponse.STSError.STSBusinessRule IsNot Nothing Then
                                If Not (oImpResponse.STSError.STSBusinessRule.Code <> Convert.ToString(SAMErrorCode.ValidationRulesReferred) AndAlso
                                oImpResponse.STSError.STSBusinessRule.Code <> Convert.ToString(SAMErrorCode.ValidationRulesDeclined) AndAlso
                                oImpResponse.STSError.STSBusinessRule.Code <> Convert.ToString(SAMErrorCode.UALRulesReferred) AndAlso
                                oImpResponse.STSError.STSBusinessRule.Code <> Convert.ToString(SAMErrorCode.UALRulesDeclined) AndAlso
                                oImpResponse.STSError.STSBusinessRule.Code <> Convert.ToString(SAMErrorCode.RatingRulesReferred) AndAlso
                            oImpResponse.STSError.STSBusinessRule.Code <> Convert.ToString(SAMErrorCode.RatingRulesDeclined)) Then
                                    oResponse.XMLDataSet = SAMFunc.TransformDatasetPBtoSAM(oImpResponse.XMLDataSet)
                                    oResponse.QuoteTimeStamp = oImpResponse.QuoteTimeStamp
                                    If Not bIgnoreErrorMessage Then
                                        SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
                ' Retrieve the values from the implementation response structure
                oResponse.CommissionAmount = oImpResponse.CommissionAmount
                oResponse.PremiumDueGross = oImpResponse.PremiumDueGross
                oResponse.PremiumDueNet = oImpResponse.PremiumDueNet
                oResponse.PremiumDueTax = oImpResponse.PremiumDueTax
                oResponse.QuoteTimeStamp = oImpResponse.QuoteTimeStamp
                oResponse.TotalAnnualTax = oImpResponse.TotalAnnualTax
                oResponse.PolicyLevelTax = oImpResponse.PolicyLevelTax
                oResponse.PolicyLevelTaxSpecified = True
                oResponse.PolicyLevelFees = oImpResponse.PolicyLevelFees
                oResponse.PolicyLevelFeesSpecified = True
                oResponse.ProRataRate = oImpResponse.ProRataRate

                oResponse.XMLDataSet = SAMFunc.TransformDatasetPBtoSAM(oImpResponse.XMLDataSet, sCalledVia:="UpdateRisk")
                oResponse.PolicyLevelTaxesAndFees = CommonFunctions.ToServiceTaxesAndFeesTypeList(oImpResponse.PolicyLevelTaxesAndFees)
                oResponse.RiskLevelTaxesAndFees = CommonFunctions.ToServiceTaxesAndFeesTypeList(oImpResponse.RiskLevelTaxesAndFees)
                oResponse.ReturnPremiumMoreThanBilled = oImpResponse.ReturnPremiumMoreThanBilled
                If oImpResponse IsNot Nothing Then
                    If oImpResponse.STSError IsNot Nothing Then
                        If oImpResponse.STSError.STSBusinessRule IsNot Nothing Then
                            If oImpResponse.STSError.STSBusinessRule.Code <> Convert.ToString(SAMErrorCode.ValidationRulesReferred) OrElse
                            oImpResponse.STSError.STSBusinessRule.Code <> Convert.ToString(SAMErrorCode.ValidationRulesDeclined) OrElse
                            oImpResponse.STSError.STSBusinessRule.Code <> Convert.ToString(SAMErrorCode.UALRulesReferred) OrElse
                            oImpResponse.STSError.STSBusinessRule.Code <> Convert.ToString(SAMErrorCode.UALRulesDeclined) OrElse
                            oImpResponse.STSError.STSBusinessRule.Code <> Convert.ToString(SAMErrorCode.RatingRulesReferred) OrElse
                            oImpResponse.STSError.STSBusinessRule.Code <> Convert.ToString(SAMErrorCode.RatingRulesDeclined) Then
                                If Not bIgnoreErrorMessage Then
                                    SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                                End If
                            End If
                        End If
                    End If
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(UpdateRiskRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(UpdateRiskRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' To Update the taxes
    ''' </summary>
    ''' <param name="UpdateTaxesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateTaxes(ByVal UpdateTaxesRequest As UpdateTaxesRequestType) As UpdateTaxesResponseType Implements IPurePolicyService.UpdateTaxes

        Try

            Dim sUserName As String = UpdateTaxesRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer


            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMUPDTAX", iUserId)
            CommonFunctions.CheckSecurityToken(UpdateTaxesRequest.WCFSecurityToken)
            Dim oResponse As New UpdateTaxesResponseType
            Dim oRequest As New UpdateTaxesRequestType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, UpdateTaxesRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateTaxesRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateTaxesResponseType = Nothing

            Dim iCount As Integer = 0
            oImpRequest.BranchCode = UpdateTaxesRequest.BranchCode
            If (UpdateTaxesRequest IsNot Nothing) AndAlso (UpdateTaxesRequest.Row IsNot Nothing) AndAlso (UpdateTaxesRequest.Row.Count <> 0) Then
                With UpdateTaxesRequest
                    If .Row IsNot Nothing Then
                        oImpRequest.Row = CommonFunctions.ToBaseImpBaseUpdateTaxes(.Row.ToList())
                    End If
                End With
            End If

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateTaxes(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                'Write back response values if required
                With oResponse

                End With
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(UpdateTaxesRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(UpdateTaxesRequest))
            Return Nothing
        End Try

    End Function

    '''<summary>
    ''' This method is used to get CoInsurance default values
    '''</summary>
    '''<param name="GetCoinsuranceDefaultsRequest" type="GetCoinsuranceDefaultsRequestType"></param>   
    '''<returns>GetCoinsuranceDefaultsResponseType</returns>
    '''<remarks></remarks> 

    Public Function GetCoinsuranceDefaults(ByVal GetCoinsuranceDefaultsRequest As GetCoinsuranceDefaultsRequestType) As GetCoinsuranceDefaultsResponseType Implements IPurePolicyService.GetCoinsuranceDefaults

        Try

            Dim sUserName As String = GetCoinsuranceDefaultsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGCD", iUserId)
            CommonFunctions.CheckSecurityToken(GetCoinsuranceDefaultsRequest.WCFSecurityToken)
            Dim oResponse As New GetCoinsuranceDefaultsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetCoinsuranceDefaultsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetCoinsuranceDefaultsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetCoinsuranceDefaultsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetCoinsuranceDefaultsRequest.BranchCode
            oImpRequest.WCFSecurityToken = If(GetCoinsuranceDefaultsRequest.WCFSecurityToken.Length > 0, GetCoinsuranceDefaultsRequest.WCFSecurityToken, "WCFSecurityToken")


            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetCoinsuranceDefaults(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.Defaults = SAMFunc.GetDeserializedValues(Of List(Of BaseGetCoinsuranceDefaultsResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetCoinsuranceDefaultsResponseTypeDefaults", sConvertToTypeName:="BaseGetCoinsuranceDefaultsResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Defaults = DataTabletoList_GetCoinsuranceDefaults(oImpResponse.ResultData.Tables(0))
                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetCoinsuranceDefaultsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetCoinsuranceDefaultsRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' This method is used to get CoInsurance details (arrangement and values)
    '''</summary>
    '''<param name="GetCoinsuranceValuesRequest" type="GetCoinsuranceValuesRequestType"></param>
    '''<returns>GetCoinsuranceValuesResponseType</returns>
    '''<remarks></remarks>

    Public Function GetCoinsuranceValues(ByVal GetCoinsuranceValuesRequest As GetCoinsuranceValuesRequestType) As GetCoinsuranceValuesResponseType Implements IPurePolicyService.GetCoinsuranceValues

        Try

            Dim sUserName As String = GetCoinsuranceValuesRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGCVs", iUserId)
            CommonFunctions.CheckSecurityToken(GetCoinsuranceValuesRequest.WCFSecurityToken)
            Dim oResponse As New GetCoinsuranceValuesResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetCoinsuranceValuesRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetCoinsuranceValuesRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetCoinsuranceValuesResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetCoinsuranceValuesRequest.BranchCode
            oImpRequest.InsuranceFileKey = GetCoinsuranceValuesRequest.InsuranceFileKey
            oImpRequest.WCFSecurityToken = If(GetCoinsuranceValuesRequest.WCFSecurityToken.Length > 0, GetCoinsuranceValuesRequest.WCFSecurityToken, "WCFSecurityToken")

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetCoinsuranceValues(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.IsRecovered = oImpResponse.IsRecovered
                oResponse.IsSurcharged = oImpResponse.IsSurcharged
                oResponse.DefaultId = oImpResponse.DefaultId

                'oResponse.CoInsurers = SAMFunc.GetDeserializedValues(Of List(Of BaseGetCoinsuranceValuesResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetCoinsuranceValuesResponseTypeCoInsurers", sConvertToTypeName:="BaseGetCoinsuranceValuesResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.CoInsurers = DataTabletoList_GetCoinsuranceValues(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetCoinsuranceValuesRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetCoinsuranceValuesRequest))
            Return Nothing
        End Try

    End Function

    Public Function UpdateFinancePlanDetails(ByVal oUpdateFinancePlanDetailsRequest As UpdateFinancePlanDetailsRequestType) As UpdateFinancePlanDetailsResponseType Implements IPureCoreService.UpdateFinancePlanDetails

        Try

            Dim sUserName As String = oUpdateFinancePlanDetailsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMUpdFP", iUserId)
            CommonFunctions.CheckSecurityToken(oUpdateFinancePlanDetailsRequest.WCFSecurityToken)
            Dim oUpdateFinancePlanDetailsResponse As New UpdateFinancePlanDetailsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness()

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateFinancePlanDetailsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateFinancePlanDetailsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oUpdateFinancePlanDetailsRequest.BranchCode
            oImpRequest.FinancePlanKey = oUpdateFinancePlanDetailsRequest.FinancePlanKey
            oImpRequest.FinancePlanVersion = oUpdateFinancePlanDetailsRequest.FinancePlanVersion

            If oUpdateFinancePlanDetailsRequest.CreditCard IsNot Nothing Then
                oImpRequest.CreditCard = New BaseImplementationTypes.BaseCreditCardType
                oImpRequest.CreditCard.ExpiryDate = oUpdateFinancePlanDetailsRequest.CreditCard.ExpiryDate
                oImpRequest.CreditCard.Issue = oUpdateFinancePlanDetailsRequest.CreditCard.Issue
                oImpRequest.CreditCard.NameOnCreditCard = oUpdateFinancePlanDetailsRequest.CreditCard.NameOnCreditCard
                oImpRequest.CreditCard.Number = oUpdateFinancePlanDetailsRequest.CreditCard.Number
                oImpRequest.CreditCard.Pin = oUpdateFinancePlanDetailsRequest.CreditCard.Pin
                oImpRequest.CreditCard.StartDate = oUpdateFinancePlanDetailsRequest.CreditCard.StartDate
                oImpRequest.CreditCard.TypeCode = oUpdateFinancePlanDetailsRequest.CreditCard.TypeCode

                oImpRequest.CreditCard.TrackingNumber = oUpdateFinancePlanDetailsRequest.CreditCard.TrackingNumber
                oImpRequest.CreditCard.PartyBankKey = oUpdateFinancePlanDetailsRequest.CreditCard.PartyBankKey


                If oUpdateFinancePlanDetailsRequest.CreditCard.CardHolder IsNot Nothing Then
                    oImpRequest.CreditCard.CardHolder = New BaseImplementationTypes.BaseCreditCardTypeCardHolder
                    oImpRequest.CreditCard.CardHolder.AddressLine1 = oUpdateFinancePlanDetailsRequest.CreditCard.CardHolder.AddressLine1
                    oImpRequest.CreditCard.CardHolder.AddressLine2 = oUpdateFinancePlanDetailsRequest.CreditCard.CardHolder.AddressLine2
                    oImpRequest.CreditCard.CardHolder.AddressLine3 = oUpdateFinancePlanDetailsRequest.CreditCard.CardHolder.AddressLine3
                    oImpRequest.CreditCard.CardHolder.AddressLine4 = oUpdateFinancePlanDetailsRequest.CreditCard.CardHolder.AddressLine4
                    oImpRequest.CreditCard.CardHolder.CountryCode = oUpdateFinancePlanDetailsRequest.CreditCard.CardHolder.CountryCode
                    oImpRequest.CreditCard.CardHolder.Name = oUpdateFinancePlanDetailsRequest.CreditCard.CardHolder.Name
                    oImpRequest.CreditCard.CardHolder.PostCode = oUpdateFinancePlanDetailsRequest.CreditCard.CardHolder.PostCode
                End If
            End If

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.UpdateFinancePlanDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oUpdateFinancePlanDetailsResponse, ex, CommonFunctions.CreateDictionary(oUpdateFinancePlanDetailsRequest))
            End Try

            Return oUpdateFinancePlanDetailsResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateFinancePlanDetailsRequest))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="GetProductRiskEventsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetProductRiskEvents(ByVal GetProductRiskEventsRequest As GetProductRiskEventsRequestType) As GetProductRiskEventsResponseType Implements IPureCoreService.GetProductRiskEvents, IPurePolicyService.GetProductRiskEvents, IPureClaimService.GetProductRiskEvents

        Try

            Dim sUserName As String = GetProductRiskEventsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGPROV", iUserId)
            CommonFunctions.CheckSecurityToken(GetProductRiskEventsRequest.WCFSecurityToken)

            Dim oResponse As New GetProductRiskEventsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness()
            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetProductRiskEventsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetProductRiskEventsResponseType = Nothing

            oImpRequest.BranchCode = GetProductRiskEventsRequest.BranchCode
            oImpRequest.ProductCode = GetProductRiskEventsRequest.ProductCode
            oImpRequest.ProductCodeSpecified = GetProductRiskEventsRequest.ProductCodeSpecified
            oImpRequest.InsuranceFileKey = GetProductRiskEventsRequest.InsuranceFileKey
            oImpRequest.InsuranceFileKeySpecified = GetProductRiskEventsRequest.InsuranceFileKeySpecified
            oImpRequest.EventType = DirectCast(GetProductRiskEventsRequest.EventType, BaseImplementationTypes.ProductEventActionType)
            oImpRequest.WCFSecurityToken = If(GetProductRiskEventsRequest.WCFSecurityToken.Length > 0, GetProductRiskEventsRequest.WCFSecurityToken, "WCFSecurityToken")

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetProductRiskEvents(oImpRequest)
                oImpResponse = oBusiness.GetProductRiskEvents(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.Events = SAMFunc.GetDeserializedValues(Of List(Of BaseGetProductRiskEventsResponseTypeRow))(elmResultDataSet:=oImpResponse.Events, sFromTypeName:="BaseGetProductRiskEventsResponseTypeEvents", sConvertToTypeName:="BaseGetProductRiskEventsResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Events = DataTabletoList_GetProductRiskEvents(oImpResponse.ResultData.Tables(0))
                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetProductRiskEventsRequest))
            End Try
            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetProductRiskEventsRequest))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oUpdateAgentsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateSubAgents(ByVal oUpdateAgentsRequest As UpdateSubAgentsRequestType) As UpdateSubAgentsResponseType Implements IPurePolicyService.UpdateSubAgents

        Try

            Dim sUserName As String = oUpdateAgentsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMAQuot", iUserId)
            CommonFunctions.CheckSecurityToken(oUpdateAgentsRequest.WCFSecurityToken)
            Dim oResponse As New UpdateSubAgentsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oUpdateAgentsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateSubAgentsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateSubAgentsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.TimeStamp = oUpdateAgentsRequest.TimeStamp
            oImpRequest.BranchCode = oUpdateAgentsRequest.BranchCode
            oImpRequest.InsuranceFileKey = oUpdateAgentsRequest.InsuranceFileKey

            If (oUpdateAgentsRequest IsNot Nothing) AndAlso (oUpdateAgentsRequest.SubAgents IsNot Nothing) AndAlso (oUpdateAgentsRequest.SubAgents.Count <> 0) Then
                With oUpdateAgentsRequest
                    If .SubAgents IsNot Nothing Then
                        oImpRequest.SubAgents = CommonFunctions.ToBaseImpBaseUpdateAgents(.SubAgents.ToList())
                    End If
                End With
            End If

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateSubAgents(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.TimeStamp = oImpResponse.TimeStamp

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdateAgentsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateAgentsRequest))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetAgentCommissionRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAgentCommission(ByVal oGetAgentCommissionRequest As GetAgentCommissionRequestType) As GetAgentCommissionResponseType Implements IPurePolicyService.GetAgentCommission
        Try

            Dim sUserName As String = oGetAgentCommissionRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGEAGCOM", iUserId)
            CommonFunctions.CheckSecurityToken(oGetAgentCommissionRequest.WCFSecurityToken)

            Dim oResponse As New GetAgentCommissionResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetAgentCommissionRequest.BranchCode)
            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetAgentCommissionRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetAgentCommissionResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetAgentCommissionRequest.BranchCode
            oImpRequest.InsuranceFileKey = oGetAgentCommissionRequest.InsuranceFileKey
            oImpRequest.RiskType = oGetAgentCommissionRequest.RiskType
            oImpRequest.CommissionBand = oGetAgentCommissionRequest.CommissionBand
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetAgentCommission(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)


                If oImpResponse.AgentCommission IsNot Nothing AndAlso oImpResponse.AgentCommission.Row IsNot Nothing Then
                    oResponse.AgentCommission = oImpResponse.AgentCommission.Row.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseAgentCommissionResponseTypeAgentCommissionRow, BaseAgentCommissionResponseTypeRow) _
                                                                                                     (AddressOf CommonFunctions.ToServiceAgentCommissionList))
                End If

                ' Retrieve the values from the implementation response structure
                oResponse.LeadAgentNet = oImpResponse.LeadAgentNet
                oResponse.LeadAgentTotalCommission = oImpResponse.LeadAgentTotalCommission
                oResponse.LeadAgentTotalTax = oImpResponse.LeadAgentTotalTax
                oResponse.SubAgentNet = oImpResponse.SubAgentNet
                oResponse.SubAgentTotalCommission = oImpResponse.SubAgentTotalCommission
                oResponse.SubAgentTotalTax = oImpResponse.SubAgentTotalTax
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetAgentCommissionRequest))
            End Try
            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetAgentCommissionRequest))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' This is webservice method for  Get FinancePlan Details
    '''<param name="oGetFinancePlanDetailsRequest" type="GetFinancePlanDetailsRequestType"></param>   
    ''' <returns>GetFinancePlanDetailsResponseType</returns>  
    '''</summary>
    '''<remarks></remarks>  
    Public Function GetFinancePlanDetails(ByVal oGetFinancePlanDetailsRequest As GetFinancePlanDetailsRequestType) As GetFinancePlanDetailsResponseType Implements IPurePolicyService.GetFinancePlanDetails

        Try

            Dim sUserName As String = oGetFinancePlanDetailsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer


            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMFP", iUserId)
            CommonFunctions.CheckSecurityToken(oGetFinancePlanDetailsRequest.WCFSecurityToken)
            Dim oGetFinancePlanDetailsResponse As New GetFinancePlanDetailsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetFinancePlanDetailsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetFinancePlanDetailsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetFinancePlanDetailsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetFinancePlanDetailsRequest.BranchCode
            oImpRequest.InsuranceFileKey = oGetFinancePlanDetailsRequest.InsuranceFileKey
            oImpRequest.FinancePlanKey = oGetFinancePlanDetailsRequest.FinancePlanKey
            oImpRequest.FinancePlanVersion = oGetFinancePlanDetailsRequest.FinancePlanVersion

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetFinancePlanDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                oGetFinancePlanDetailsResponse.FinacePlanKey = oImpResponse.FinacePlanKey
                oGetFinancePlanDetailsResponse.FinacePlanVersion = oImpResponse.FinacePlanVersion
                oGetFinancePlanDetailsResponse.Deposit = oImpResponse.Deposit
                oGetFinancePlanDetailsResponse.EndDate = oImpResponse.EndDate
                oGetFinancePlanDetailsResponse.FinancedAmount = oImpResponse.FinancedAmount
                oGetFinancePlanDetailsResponse.InterestRate = oImpResponse.InterestRate
                oGetFinancePlanDetailsResponse.SchemeName = oImpResponse.SchemeName
                oGetFinancePlanDetailsResponse.StartDate = oImpResponse.StartDate
                oGetFinancePlanDetailsResponse.Status = CType([Enum].ToObject(GetType(BaseImplementationTypes.FinancePlanStatus), oImpResponse.Status), FinancePlanStatus)
                oGetFinancePlanDetailsResponse.Taxes = oImpResponse.Taxes
                oGetFinancePlanDetailsResponse.TotalAmount = oImpResponse.TotalAmount
                ' Assign all the newly added parameter from oImpResponse to oGetFinancePlanDetailsResponse Type
                oGetFinancePlanDetailsResponse.APRRate = oImpResponse.APRRate
                oGetFinancePlanDetailsResponse.MediaType = oImpResponse.MediaType
                oGetFinancePlanDetailsResponse.NoOfInstalments = oImpResponse.NoOfInstalments
                oGetFinancePlanDetailsResponse.FirstInstalmentDate = oImpResponse.FirstInstalmentDate
                oGetFinancePlanDetailsResponse.NextInstalmentDate = oImpResponse.NextInstalmentDate
                oGetFinancePlanDetailsResponse.LastInstalmentDate = oImpResponse.LastInstalmentDate
                oGetFinancePlanDetailsResponse.FirstInstalmentAmount = oImpResponse.FirstInstalmentAmount
                oGetFinancePlanDetailsResponse.OtherInstalmentAmount = oImpResponse.OtherInstalmentAmount
                oGetFinancePlanDetailsResponse.AdminCharge = oImpResponse.AdminCharge
                oGetFinancePlanDetailsResponse.ProtectionCharge = oImpResponse.ProtectionCharge
                oGetFinancePlanDetailsResponse.InterestAmount = oImpResponse.InterestAmount
                oGetFinancePlanDetailsResponse.PartyBankKey = oImpResponse.PartyBankKey
                oGetFinancePlanDetailsResponse.BankName = oImpResponse.BankName
                oGetFinancePlanDetailsResponse.BankAddress1 = oImpResponse.BankAddress1
                oGetFinancePlanDetailsResponse.BankBranchName = oImpResponse.BankBranchName
                oGetFinancePlanDetailsResponse.BankBranchCode = oImpResponse.BankBranchCode
                oGetFinancePlanDetailsResponse.BankAccountName = oImpResponse.BankAccountName
                oGetFinancePlanDetailsResponse.BankAccountNumber = oImpResponse.BankAccountNumber
                oGetFinancePlanDetailsResponse.BankAccountType = oImpResponse.BankAccountType
                oGetFinancePlanDetailsResponse.Frequency = oImpResponse.Frequency
                oGetFinancePlanDetailsResponse.PaymentMethod = oImpResponse.PaymentMethod
                oGetFinancePlanDetailsResponse.BIC = oImpResponse.BIC
                oGetFinancePlanDetailsResponse.IBAN = oImpResponse.IBAN
                oGetFinancePlanDetailsResponse.DayOfWeekOrMonth = oImpResponse.DayOfWeekOrMonth
                oGetFinancePlanDetailsResponse.PlanReference = oImpResponse.PlanReference
                Select Case CType([Enum].ToObject(GetType(BaseImplementationTypes.FinancePlanStatus), oImpResponse.Status), FinancePlanStatus)
                    Case FinancePlanStatus.Item000
                        oGetFinancePlanDetailsResponse.StatusDescription = StatusDescription.None.ToString()
                    Case FinancePlanStatus.Item010
                        oGetFinancePlanDetailsResponse.StatusDescription = StatusDescription.Saved.ToString()
                    Case FinancePlanStatus.Item011
                        oGetFinancePlanDetailsResponse.StatusDescription = StatusDescription.Updated.ToString()
                    Case FinancePlanStatus.Item012
                        oGetFinancePlanDetailsResponse.StatusDescription = StatusDescription.QuotePrinted.ToString()
                    Case FinancePlanStatus.Item040
                        oGetFinancePlanDetailsResponse.StatusDescription = StatusDescription.Live.ToString()
                    Case FinancePlanStatus.Item140
                        oGetFinancePlanDetailsResponse.StatusDescription = StatusDescription.OnHold.ToString()
                    Case FinancePlanStatus.Item900
                        oGetFinancePlanDetailsResponse.StatusDescription = StatusDescription.Completed.ToString()
                    Case FinancePlanStatus.Item990
                        oGetFinancePlanDetailsResponse.StatusDescription = StatusDescription.Superseded.ToString()
                    Case FinancePlanStatus.Item999
                        oGetFinancePlanDetailsResponse.StatusDescription = StatusDescription.Cancelled.ToString()
                End Select

                If oImpResponse.CreditCard IsNot Nothing Then
                    Dim oResponseCreditCard As New BaseCreditCardType
                    Dim oResponseCreditCardTypeCardHolder As New BaseCreditCardTypeCardHolder
                    Dim oCreditCard As SiriusFS.SAM.Structure.BaseImplementationTypes.BaseCreditCardType = oImpResponse.CreditCard
                    If oImpResponse.CreditCard.CardHolder IsNot Nothing Then

                        oResponseCreditCardTypeCardHolder.AddressLine1 = oCreditCard.CardHolder.AddressLine1
                        oResponseCreditCardTypeCardHolder.AddressLine2 = oCreditCard.CardHolder.AddressLine2
                        oResponseCreditCardTypeCardHolder.AddressLine3 = oCreditCard.CardHolder.AddressLine3
                        oResponseCreditCardTypeCardHolder.AddressLine4 = oCreditCard.CardHolder.AddressLine4
                        oResponseCreditCardTypeCardHolder.CountryCode = oCreditCard.CardHolder.CountryCode
                        oResponseCreditCardTypeCardHolder.Name = oCreditCard.CardHolder.Name
                        oResponseCreditCardTypeCardHolder.PostCode = oCreditCard.CardHolder.PostCode

                        oResponseCreditCard.CardHolder = oResponseCreditCardTypeCardHolder
                    End If
                    oResponseCreditCard.ExpiryDate = oCreditCard.ExpiryDate
                    oResponseCreditCard.Issue = oCreditCard.Issue
                    oResponseCreditCard.NameOnCreditCard = oCreditCard.NameOnCreditCard
                    oResponseCreditCard.Number = oCreditCard.Number
                    oResponseCreditCard.Pin = oCreditCard.Pin
                    oResponseCreditCard.StartDate = oCreditCard.StartDate
                    oResponseCreditCard.TypeCode = oCreditCard.TypeCode
                    oResponseCreditCard.AccountType = oCreditCard.AccountType
                    oResponseCreditCard.AuthCode = oCreditCard.AuthCode
                    oResponseCreditCard.ManualAuthCode = oCreditCard.ManualAuthCode
                    oResponseCreditCard.TrackingNumber = oCreditCard.TrackingNumber
                    oResponseCreditCard.TransactionCode = oCreditCard.TransactionCode
                    oResponseCreditCard.PartyBankKey = oCreditCard.PartyBankKey

                    oGetFinancePlanDetailsResponse.CreditCard = oResponseCreditCard

                End If

                If (oImpResponse.Instalments IsNot Nothing) Then
                    If (oImpResponse.Instalments.Row IsNot Nothing) Then

                        oGetFinancePlanDetailsResponse.Instalments = oImpResponse.Instalments.Row.ToList().ConvertAll(
                            New Converter(Of BaseImplementationTypes.BaseGetFinancePlanDetailsResponseTypeInstalmentsRow, BaseGetFinancePlanDetailsResponseTypeRow)(AddressOf CommonFunctions.ToServiceGetFinancePlanDetailsInstalmentList))

                    End If
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oGetFinancePlanDetailsResponse, ex, CommonFunctions.CreateDictionary(oGetFinancePlanDetailsRequest))
            End Try

            Return oGetFinancePlanDetailsResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetFinancePlanDetailsRequest))
            Return Nothing

        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="GetExistingInstalmentPlanPaymentDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetExistingInstalmentPlanPaymentDetails(ByVal GetExistingInstalmentPlanPaymentDetailsRequest As GetExistingInstalmentPlanPaymentDetailsRequestType) As GetExistingInstalmentPlanPaymentDetailsResponseType Implements IPurePolicyService.GetExistingInstalmentPlanPaymentDetails
        Try

            Dim sUserName As String = GetExistingInstalmentPlanPaymentDetailsRequest.LoginUserName
            Dim agentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, agentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMFIFFCLM", iUserId)
            CommonFunctions.CheckSecurityToken(GetExistingInstalmentPlanPaymentDetailsRequest.WCFSecurityToken)

            Dim response As New GetExistingInstalmentPlanPaymentDetailsResponseType
            Dim business As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetExistingInstalmentPlanPaymentDetailsRequest.BranchCode)
            ' Implementation structures
            Dim impRequest As New SAMForInsuranceV2ImplementationTypes.GetExistingInstalmentPlanPaymentDetailsRequestType
            Dim impResponse As SAMForInsuranceV2ImplementationTypes.GetExistingInstalmentPlanPaymentDetailsResponseType = Nothing
            impRequest.AgentKey = agentKey
            impRequest.UserName = sUserName
            impRequest.BranchCode = GetExistingInstalmentPlanPaymentDetailsRequest.BranchCode
            impRequest.InsuranceFileKey = GetExistingInstalmentPlanPaymentDetailsRequest.InsuranceFileKey
            Try
                ' Call the implementation method
                impResponse = business.GetExistingInstalmentPlanPaymentDetails(impRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(impResponse.STSError)
                If Not impResponse Is Nothing Then
                    ' Retrieve the values from the implementation response structure into Actual Response
                    response.BankAccountName = impResponse.BankAccountName
                    response.BankAccountNo = impResponse.BankAccountNo
                    response.BankAreaCode = impResponse.BankAreaCode
                    response.BankBranch = impResponse.BankBranch
                    response.BankExtn = impResponse.BankExtn
                    response.BankFax = impResponse.BankFax
                    response.BankFaxCode = impResponse.BankFaxCode
                    response.BankName = impResponse.BankName
                    response.BankPhone = impResponse.BankPhone
                    response.BankSortCode = impResponse.BankSortCode
                    response.BIC = impResponse.BIC
                    response.IBAN = impResponse.IBAN
                    If Not impResponse.BankAddress Is Nothing Then
                        response.BankAddress = New BaseAddressType
                        response.BankAddress.AddressLine1 = impResponse.BankAddress.AddressLine1
                        response.BankAddress.AddressLine2 = impResponse.BankAddress.AddressLine2
                        response.BankAddress.AddressLine3 = impResponse.BankAddress.AddressLine3
                        response.BankAddress.AddressLine4 = impResponse.BankAddress.AddressLine4
                        response.BankAddress.CountryCode = impResponse.BankAddress.CountryCode
                        response.BankAddress.PostCode = impResponse.BankAddress.PostCode
                    End If
                    If Not impResponse.CreditCard Is Nothing Then
                        response.CreditCard = New BaseCreditCardType
                        response.CreditCard.Number = impResponse.CreditCard.Number
                        response.CreditCard.Pin = impResponse.CreditCard.Pin
                        response.CreditCard.StartDate = impResponse.CreditCard.StartDate
                        response.CreditCard.TypeCode = impResponse.CreditCard.TypeCode
                        response.CreditCard.ExpiryDate = impResponse.CreditCard.ExpiryDate
                        response.CreditCard.Issue = impResponse.CreditCard.Issue
                        response.CreditCard.NameOnCreditCard = impResponse.CreditCard.NameOnCreditCard
                        If Not impResponse.CreditCard.CardHolder Is Nothing Then
                            response.CreditCard.CardHolder = New BaseCreditCardTypeCardHolder
                            response.CreditCard.CardHolder.AddressLine1 = impResponse.CreditCard.CardHolder.AddressLine1
                            response.CreditCard.CardHolder.AddressLine2 = impResponse.CreditCard.CardHolder.AddressLine2
                            response.CreditCard.CardHolder.AddressLine3 = impResponse.CreditCard.CardHolder.AddressLine3
                            response.CreditCard.CardHolder.AddressLine4 = impResponse.CreditCard.CardHolder.AddressLine4
                            response.CreditCard.CardHolder.CountryCode = impResponse.CreditCard.CardHolder.CountryCode
                            response.CreditCard.CardHolder.Name = impResponse.CreditCard.CardHolder.Name
                            response.CreditCard.CardHolder.PostCode = impResponse.CreditCard.CardHolder.PostCode
                        End If
                    End If
                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(impResponse, response, ex, CommonFunctions.CreateDictionary(GetExistingInstalmentPlanPaymentDetailsRequest))
            End Try
            Return response
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetExistingInstalmentPlanPaymentDetailsRequest))
            Return Nothing
        End Try
    End Function

    ''' <summary>  
    ''' This web services is used to Get MID File Details.
    ''' </summary>  
    ''' <param name="oGetMIDFileDetailsRequest">An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetMIDFileDetailsRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetMIDFileDetailsResponseType</returns>  
    Public Function GetMIDFileDetails(ByVal oGetMIDFileDetailsRequest As GetMIDFileDetailsRequestType) As GetMIDFileDetailsResponseType Implements IPureCoreService.GetMIDFileDetails

        Try

            Dim sUserName As String = oGetMIDFileDetailsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMMIDDET", iUserId)
            CommonFunctions.CheckSecurityToken(oGetMIDFileDetailsRequest.WCFSecurityToken)
            Dim oResponse As New GetMIDFileDetailsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetMIDFileDetailsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetMIDFileDetailsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetMIDFileDetailsResponseType = Nothing
            oImpRequest.BranchCode = oGetMIDFileDetailsRequest.BranchCode
            oImpRequest.MIDFileKey = oGetMIDFileDetailsRequest.MIDFileKey
            oImpRequest.FailuresOnly = oGetMIDFileDetailsRequest.FailuresOnly

            Try
                'Call the implementation method
                oImpResponse = oBusiness.GetMIDFileDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.FileSequenceNumber = oImpResponse.FileSequenceNumber
                oResponse.FailuresOnly = oImpResponse.FailuresOnly

                If (oImpResponse.Policies IsNot Nothing) Then
                    If (oImpResponse.Policies.Row IsNot Nothing) Then
                        oResponse.Policies = oImpResponse.Policies.Row.ToList().ConvertAll(
                            New Converter(Of BaseImplementationTypes.BaseGetMIDFileDetailsResponseTypePoliciesRow, BaseGetMIDFileDetailsResponseTypeRow)(AddressOf CommonFunctions.ToServiceGetMIDFileDetailsPoliciesList))
                    End If
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetMIDFileDetailsRequest))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetMIDFileDetailsRequest))
            Return Nothing
        End Try
    End Function

    ''' <summary>  
    ''' This web services is used to Get MID Files.
    ''' </summary>  
    ''' <param name="oGetMIDFilesRequest">An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetMIDFilesRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetMIDFilesResponseType</returns>  
    Public Function GetMIDFiles(ByVal oGetMIDFilesRequest As GetMIDFilesRequestType) As GetMIDFilesResponseType Implements IPureCoreService.GetMIDFiles

        Try

            Dim sUserName As String = oGetMIDFilesRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMMID", iUserId)
            CommonFunctions.CheckSecurityToken(oGetMIDFilesRequest.WCFSecurityToken)
            Dim oResponse As New GetMIDFilesResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetMIDFilesRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetMIDFilesRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetMIDFilesResponseType = Nothing
            oImpRequest.BranchCode = oGetMIDFilesRequest.BranchCode
            oImpRequest.StartDate = oGetMIDFilesRequest.StartDate
            oImpRequest.EndDate = oGetMIDFilesRequest.EndDate
            oImpRequest.MIDFileKey = oGetMIDFilesRequest.MIDFileKey
            oImpRequest.FailuresOnly = oGetMIDFilesRequest.FailuresOnly
            Try
                'Call the implementation method
                oImpResponse = oBusiness.GetMIDFiles(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                If (oImpResponse.MIDFiles IsNot Nothing) Then
                    If (oImpResponse.MIDFiles.Row IsNot Nothing) Then
                        oResponse.MIDFiles = oImpResponse.MIDFiles.Row.ToList().ConvertAll(
                            New Converter(Of BaseImplementationTypes.BaseGetMIDFilesResponseTypeMIDFilesRow, BaseGetMIDFilesResponseTypeRow)(AddressOf CommonFunctions.ToServiceGetMIDFilesMIDFilesList))
                    End If
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetMIDFilesRequest))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetMIDFilesRequest))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetDocumentListRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDocumentList(ByVal oGetDocumentListRequest As GetDocumentListRequestType) As GetDocumentListResponseType Implements IPureCoreService.GetDocumentList

        Try

            Dim sUserName As String = oGetDocumentListRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGDocLst", iUserId)
            CommonFunctions.CheckSecurityToken(oGetDocumentListRequest.WCFSecurityToken)

            Dim oResponse As GetDocumentListResponseType = New GetDocumentListResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetDocumentListRequest.BranchCode)

            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetDocumentListRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetDocumentListResponseType = Nothing

            oImpRequest.InsuranceFolderKey = oGetDocumentListRequest.InsuranceFolderKey

            Try
                oImpResponse = oBusiness.GetDocumentList(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                If oImpResponse.Documents IsNot Nothing AndAlso oImpResponse.Documents.Count > 0 Then
                    oResponse.Documents = oImpResponse.Documents.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseDocumentType, BaseDocumentType)(AddressOf CommonFunctions.ToServiceDocumentTypeList))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetDocumentListRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetDocumentListRequest))
            Return Nothing
        End Try

    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetPolicyStatusForMediaTypeStatusRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPolicyStatusForMediaTypeStatus(ByVal oGetPolicyStatusForMediaTypeStatusRequest As GetPolicyStatusForMediaTypeStatusRequestType) As GetPolicyStatusForMediaTypeStatusResponseType Implements IPurePolicyService.GetPolicyStatusForMediaTypeStatus

        Try

            Dim sUserName As String = oGetPolicyStatusForMediaTypeStatusRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGETPSFM", iUserId)
            CommonFunctions.CheckSecurityToken(oGetPolicyStatusForMediaTypeStatusRequest.WCFSecurityToken)
            Dim oResponse As New GetPolicyStatusForMediaTypeStatusResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetPolicyStatusForMediaTypeStatusRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetPolicyStatusForMediaTypeStatusRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetPolicyStatusForMediaTypeStatusResponseType = Nothing

            oImpRequest.BranchCode = oGetPolicyStatusForMediaTypeStatusRequest.BranchCode
            oImpRequest.InsuranceFileKey = oGetPolicyStatusForMediaTypeStatusRequest.InsuranceFileKey
            oImpRequest.LossDate = oGetPolicyStatusForMediaTypeStatusRequest.LossDate
            oImpRequest.LossDateSpecified = oGetPolicyStatusForMediaTypeStatusRequest.LossDateSpecified

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetPolicyStatusForMediaTypeStatus(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                ' Populate the Response Type

                oResponse.IsClaimPaymentInitiated = oImpResponse.IsClaimPaymentInitiated
                oResponse.IsClaimPaymentInitiatedOnLossDate = oImpResponse.IsClaimPaymentInitiatedOnLossDate
                oResponse.IsPolicyCanceled = oImpResponse.IsPolicyCanceled
                oResponse.IsUnclearedCashListExists = oImpResponse.IsUnclearedCashListExists

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetPolicyStatusForMediaTypeStatusRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetPolicyStatusForMediaTypeStatusRequest))
            Return Nothing
        End Try

    End Function

#Region "Web Method - UpdateFeeRequest"
    Public Function UpdateFee(ByVal oUpdateFeeRequest As UpdateFeeRequestType) As UpdateFeeResponseType Implements IPurePolicyService.UpdateFee

        Try

            Dim sUserName As String = oUpdateFeeRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMUPDFEE", iUserId)
            CommonFunctions.CheckSecurityToken(oUpdateFeeRequest.WCFSecurityToken)

            Dim oResponse As New UpdateFeeResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oUpdateFeeRequest.BranchCode)

            'Implementation structure
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateFeeRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateFeeResponseType

            'Pass the values to the implementation request structure
            oImpRequest.BranchCode = oUpdateFeeRequest.BranchCode

            oImpRequest.IsValue = oUpdateFeeRequest.IsValue
            oImpRequest.FeeKey = oUpdateFeeRequest.FeeKey
            oImpRequest.Rate = oUpdateFeeRequest.Rate

            ' India -Pass all the other values to the request structure.

            Try
                'Call the implementation method
                oImpResponse = oBusiness.UpdateFee(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdateFeeRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateFeeRequest))
            Return Nothing
        End Try

    End Function


#End Region

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oUpdateStandardWordingTemplateRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateStandardWordingTemplate(ByVal oUpdateStandardWordingTemplateRequest As UpdateStandardWordingTemplateRequestType) As UpdateStandardWordingTemplateResponseType Implements IPureCoreService.UpdateStandardWordingTemplate

        Try

            Dim sUserName As String = oUpdateStandardWordingTemplateRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New UpdateStandardWordingTemplateResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oUpdateStandardWordingTemplateRequest.BranchCode)
            Dim iCount As Integer = 0
            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateStandardWordingTemplateRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateStandardWordingTemplateResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oUpdateStandardWordingTemplateRequest.BranchCode
            oImpRequest.DocumentTemplateCode = oUpdateStandardWordingTemplateRequest.DocumentTemplateCode.Trim
            oImpRequest.DocumentTemplateKey = oUpdateStandardWordingTemplateRequest.DocumentTemplateKey
            oImpRequest.DocumentTemplateKeySpecified = oUpdateStandardWordingTemplateRequest.DocumentTemplateKeySpecified
            oImpRequest.DocumentTemplate = oUpdateStandardWordingTemplateRequest.DocumentTemplate
            If oUpdateStandardWordingTemplateRequest.DocumentTemplateFormatSpecified Then
                oImpRequest.DocumentTemplateFormat = CType([Enum].ToObject(GetType(DocumentFormatType), oUpdateStandardWordingTemplateRequest.DocumentTemplateFormat), BaseImplementationTypes.DocumentFormatType)
                oImpRequest.DocumentTemplateFormatSpecified = True
            End If

            oImpRequest.IsTXTextControlEnabled = oUpdateStandardWordingTemplateRequest.IsTXTextControlEnabled
            Try

            ' Call the implementation method
            oImpResponse = oBusiness.UpdateStandardWordingTemplate(oImpRequest)

                ' Return back the Byte array to front End
                oResponse.NewDocumentTemplateCode = oImpResponse.NewDocumentTemplateCode.Trim
                oResponse.NewDocumentTemplateKey = oImpResponse.NewDocumentTemplateKey

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdateStandardWordingTemplateRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateStandardWordingTemplateRequest))
            Return Nothing
        End Try

    End Function


    Public Function GetRiskReinsuranceArrangementLines(ByVal GetRiskReinsuranceArrangementLinesRequest As GetRiskReinsuranceArrangementLinesRequestType) As GetRiskReinsuranceArrangementLinesResponseType Implements IPurePolicyService.GetRiskReinsuranceArrangementLines
        Try
            Dim sUserName As String = GetRiskReinsuranceArrangementLinesRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHRKEY", iUserId)
            CommonFunctions.CheckSecurityToken(GetRiskReinsuranceArrangementLinesRequest.WCFSecurityToken)
            Dim oResponse As New GetRiskReinsuranceArrangementLinesResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetRiskReinsuranceArrangementLinesRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetRiskReinsuranceArrangementLinesRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetRiskReinsuranceArrangementLinesResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetRiskReinsuranceArrangementLinesRequest.BranchCode
            oImpRequest.ArrangementId = GetRiskReinsuranceArrangementLinesRequest.ArrangementId
            oImpRequest.WCFSecurityToken = If(GetRiskReinsuranceArrangementLinesRequest.WCFSecurityToken.Length > 0, GetRiskReinsuranceArrangementLinesRequest.WCFSecurityToken, "WCFSecurityToken")

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetRiskReinsuranceArrangementLines(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.ArrangementLines = SAMFunc.GetDeserializedValues(Of List(Of BaseGetRiskReinsuranceArrangementLinesResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetRiskReinsuranceArrangementLinesResponseTypeArrangementLines", sConvertToTypeName:="BaseGetRiskReinsuranceArrangementLinesResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.ArrangementLines = DataTabletoList_GetRiskReinsuranceArrangementLines(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetRiskReinsuranceArrangementLinesRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetRiskReinsuranceArrangementLinesRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetRiskReinsuranceArrangements(ByVal GetRiskReinsuranceArrangementsRequest As GetRiskReinsuranceArrangementsRequestType) As GetRiskReinsuranceArrangementsResponseType Implements IPurePolicyService.GetRiskReinsuranceArrangements
        Try
            Dim sUserName As String = GetRiskReinsuranceArrangementsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGRIA", iUserId)
            CommonFunctions.CheckSecurityToken(GetRiskReinsuranceArrangementsRequest.WCFSecurityToken)
            Dim oResponse As New GetRiskReinsuranceArrangementsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetRiskReinsuranceArrangementsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetRiskReinsuranceArrangementsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetRiskReinsuranceArrangementsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetRiskReinsuranceArrangementsRequest.BranchCode
            oImpRequest.RiskKey = GetRiskReinsuranceArrangementsRequest.RiskKey
            oImpRequest.WCFSecurityToken = If(GetRiskReinsuranceArrangementsRequest.WCFSecurityToken.Length > 0, GetRiskReinsuranceArrangementsRequest.WCFSecurityToken, "WCFSecurityToken")


            oImpRequest.RIVersionID = GetRiskReinsuranceArrangementsRequest.RIVersionID
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetRiskReinsuranceArrangements(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.Arrangements = SAMFunc.GetDeserializedValues(Of List(Of BaseGetRiskReinsuranceArrangementsResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetRiskReinsuranceArrangementsResponseTypeArrangements", sConvertToTypeName:="BaseGetRiskReinsuranceArrangementsResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Arrangements = DataTabletoList_GetRiskReinsuranceArrangements(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetRiskReinsuranceArrangementsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetRiskReinsuranceArrangementsRequest))
            Return Nothing
        End Try
    End Function

    Public Function GetRiskReinsuranceBands(ByVal GetRiskReinsuranceBandsRequest As GetRiskReinsuranceBandsRequestType) As GetRiskReinsuranceBandsResponseType Implements IPurePolicyService.GetRiskReinsuranceBands


        Try
            Dim sUserName As String = GetRiskReinsuranceBandsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHRKEY", iUserId)
            CommonFunctions.CheckSecurityToken(GetRiskReinsuranceBandsRequest.WCFSecurityToken)
            Dim oResponse As New GetRiskReinsuranceBandsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetRiskReinsuranceBandsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetRiskReinsuranceBandsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetRiskReinsuranceBandsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetRiskReinsuranceBandsRequest.BranchCode
            oImpRequest.RiskKey = GetRiskReinsuranceBandsRequest.RiskKey
            oImpRequest.WCFSecurityToken = If(GetRiskReinsuranceBandsRequest.WCFSecurityToken.Length > 0, GetRiskReinsuranceBandsRequest.WCFSecurityToken, "WCFSecurityToken")
            oImpRequest.RIVersionID = GetRiskReinsuranceBandsRequest.RIVersionID
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetRiskReinsuranceBands(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.ReinsuranceBands = SAMFunc.GetDeserializedValues(Of List(Of BaseGetRiskReinsuranceBandsResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetRiskReinsuranceBandsResponseTypeReinsuranceBands", sConvertToTypeName:="BaseGetRiskReinsuranceBandsResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.ReinsuranceBands = DataTabletoList_GetRiskReinsuranceBands(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetRiskReinsuranceBandsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetRiskReinsuranceBandsRequest))
            Return Nothing
        End Try
    End Function


    Public Function GetRiskReinsuranceArrangementLinesRI2007(ByVal GetRiskReinsuranceArrangementLinesRI2007Request As GetRiskReinsuranceArrangementLinesRI2007RequestType) As GetRiskReinsuranceArrangementLinesRI2007ResponseType Implements IPurePolicyService.GetRiskReinsuranceArrangementLinesRI2007
        Try

            Dim sUserName As String = GetRiskReinsuranceArrangementLinesRI2007Request.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckSecurityToken(GetRiskReinsuranceArrangementLinesRI2007Request.WCFSecurityToken)
            Dim oResponse As New GetRiskReinsuranceArrangementLinesRI2007ResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetRiskReinsuranceArrangementLinesRI2007Request.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetRiskReinsuranceArrangementLinesRI2007RequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetRiskReinsuranceArrangementLinesRI2007ResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetRiskReinsuranceArrangementLinesRI2007Request.BranchCode
            oImpRequest.ArrangementKey = GetRiskReinsuranceArrangementLinesRI2007Request.ArrangementKey
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetRiskReinsuranceArrangementLinesRI2007(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                If (oImpResponse.ArrangementLines IsNot Nothing) Then
                    oResponse.ArrangementLines = oImpResponse.ArrangementLines.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseRiskRIArrangementLineType, BaseRiskRIArrangementLineType)(AddressOf CommonFunctions.ToServiceRiskRIArrangementLineTypeList))
                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetRiskReinsuranceArrangementLinesRI2007Request))
            End Try
            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetRiskReinsuranceArrangementLinesRI2007Request))
            Return Nothing
        End Try
    End Function

    Public Function GetRIModelDetails(ByVal GetRIModelDetailsRequest As GetRIModelDetailsRequestType) As GetRIModelDetailsResponseType Implements IPurePolicyService.GetRIModelDetails
        Try

            Dim sUserName As String = GetRIModelDetailsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGRIMOD", iUserId)
            CommonFunctions.CheckSecurityToken(GetRIModelDetailsRequest.WCFSecurityToken)

            Dim oResponse As New GetRIModelDetailsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetRIModelDetailsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetRIModelDetailsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetRIModelDetailsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetRIModelDetailsRequest.BranchCode
            oImpRequest.RIModelCode = GetRIModelDetailsRequest.RIModelCode
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetRIModelDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Map to the Reponse object to send RI Lines back to the Front End
                With oResponse
                    .RIModelKey = oImpResponse.RIModelKey
                    .Code = oImpResponse.Code
                    .Description = oImpResponse.Description
                    .EffectiveDate = oImpResponse.EffectiveDate
                    .CurrencyCode = oImpResponse.CurrencyCode
                    .ExpiryDate = oImpResponse.ExpiryDate
                    .FACPremiums = oImpResponse.FACPremiums
                    .RIModelType = oImpResponse.RIModelType
                    .ClaimAllocationType = oImpResponse.ClaimAllocationType
                End With
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetRIModelDetailsRequest))
            End Try
            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetRIModelDetailsRequest))
            Return Nothing
        End Try
    End Function


    Public Function GetRIModelLineDetails(ByVal oGetRIModelLineDetailsRequest As GetRIModelLineDetailsRequestType) As GetRIModelLineDetailsResponseType Implements IPurePolicyService.GetRIModelLineDetails

        Try

            Dim sUserName As String = oGetRIModelLineDetailsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGRIMOD", iUserId)
            CommonFunctions.CheckSecurityToken(oGetRIModelLineDetailsRequest.WCFSecurityToken)

            Dim oResponse As New GetRIModelLineDetailsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetRIModelLineDetailsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetRIModelLineDetailsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetRIModelLineDetailsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetRIModelLineDetailsRequest.BranchCode
            oImpRequest.RIModelCode = oGetRIModelLineDetailsRequest.RIModelCode
            oImpRequest.WCFSecurityToken = If(oGetRIModelLineDetailsRequest.WCFSecurityToken.Length > 0, oGetRIModelLineDetailsRequest.WCFSecurityToken, "WCFSecurityToken")
            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetRIModelLineDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.Lines = SAMFunc.GetDeserializedValues(Of List(Of BaseGetRIModelLineDetailsResponseTypeLinesRow))(elmResultDataSet:=oImpResponse.Lines, sFromTypeName:="BaseGetRIModelLineDetailsResponseTypeLines", sConvertToTypeName:="BaseGetRIModelLineDetailsResponseTypeLinesRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Lines = DataTabletoList_GetRIModelLineDetailsLines(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetRIModelLineDetailsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetRIModelLineDetailsRequest))
            Return Nothing
        End Try

    End Function


    Public Function FindReinsurer(ByVal oFindReinsurerRequest As FindReinsurerRequestType) As FindReinsurerResponseType Implements IPurePolicyService.FindReinsurer

        Try

            Dim sUserName As String = oFindReinsurerRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMFINDRI", iUserId)
            CommonFunctions.CheckSecurityToken(oFindReinsurerRequest.WCFSecurityToken)

            Dim oResponse As New FindReinsurerResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oFindReinsurerRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.FindReinsurerRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.FindReinsurerResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oFindReinsurerRequest.BranchCode
            oImpRequest.FileCode = oFindReinsurerRequest.FileCode
            oImpRequest.IsBroker = oFindReinsurerRequest.IsBroker
            oImpRequest.IsBrokerSpecified = oFindReinsurerRequest.IsBrokerSpecified
            oImpRequest.IsRetained = oFindReinsurerRequest.IsRetained
            oImpRequest.IsRetainedSpecified = oFindReinsurerRequest.IsRetainedSpecified
            oImpRequest.RICode = oFindReinsurerRequest.RICode
            oImpRequest.RIName = oFindReinsurerRequest.RIName
            oImpRequest.RITypeCode = oFindReinsurerRequest.RITypeCode
            oImpRequest.IsFAX = oFindReinsurerRequest.IsFAX
            oImpRequest.IsFAXSpecified = oFindReinsurerRequest.IsFAXSpecified
            oImpRequest.IncludeClosedBranches = oFindReinsurerRequest.IncludeClosedBranches
            oImpRequest.WCFSecurityToken = If(oFindReinsurerRequest.WCFSecurityToken.Length > 0, oFindReinsurerRequest.WCFSecurityToken, "WCFSecurityToken")
            Try

                ' Call the implementation method
                oImpResponse = oBusiness.FindReinsurer(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.Reinsurers = SAMFunc.GetDeserializedValues(Of List(Of BaseFindReinsurerResponseTypeReinsurersRow))(elmResultDataSet:=oImpResponse.Reinsurers, sFromTypeName:="BaseFindReinsurerResponseTypeReinsurers", sConvertToTypeName:="BaseFindReinsurerResponseTypeReinsurersRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Reinsurers = DataTabletoList_FindReinsurerReinsurers(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oFindReinsurerRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oFindReinsurerRequest))
            Return Nothing
        End Try

    End Function


    Public Function GetTreatyPartyDetails(ByVal oGetTreatyPartyDetailsRequest As GetTreatyPartyDetailsRequestType) As GetTreatyPartyDetailsResponseType Implements IPurePolicyService.GetTreatyPartyDetails

        Try

            Dim sUserName As String = oGetTreatyPartyDetailsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGTtyPty", iUserId)
            CommonFunctions.CheckSecurityToken(oGetTreatyPartyDetailsRequest.WCFSecurityToken)

            Dim oResponse As New GetTreatyPartyDetailsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetTreatyPartyDetailsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetTreatyPartyDetailsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetTreatyPartyDetailsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetTreatyPartyDetailsRequest.BranchCode
            oImpRequest.TreatyCode = oGetTreatyPartyDetailsRequest.TreatyCode
            oImpRequest.WCFSecurityToken = If(oGetTreatyPartyDetailsRequest.WCFSecurityToken.Length > 0, oGetTreatyPartyDetailsRequest.WCFSecurityToken, "WCFSecurityToken")
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetTreatyPartyDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.Parties = SAMFunc.GetDeserializedValues(Of List(Of BaseGetTreatyPartyDetailsResponseTypePartiesRow))(elmResultDataSet:=oImpResponse.Parties, sFromTypeName:="BaseGetTreatyPartyDetailsResponseTypeParties", sConvertToTypeName:="BaseGetTreatyPartyDetailsResponseTypePartiesRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Parties = DataTabletoList_GetTreatyPartyDetailsParties(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetTreatyPartyDetailsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetTreatyPartyDetailsRequest))
            Return Nothing
        End Try

    End Function

    Public Function UpdateArrangementLinesRI2007(ByVal UpdateArrangementLinesRI2007Request As UpdateArrangementLinesRI2007RequestType) As UpdateArrangementLinesRI2007ResponseType Implements IPurePolicyService.UpdateArrangementLinesRI2007
        Try

            Dim sUserName As String = UpdateArrangementLinesRI2007Request.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMURI07", iUserId)
            CommonFunctions.CheckSecurityToken(UpdateArrangementLinesRI2007Request.WCFSecurityToken)

            Dim oResponse As New UpdateArrangementLinesRI2007ResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, UpdateArrangementLinesRI2007Request.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateArrangementLinesRI2007RequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateArrangementLinesRI2007ResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = UpdateArrangementLinesRI2007Request.BranchCode
            If (UpdateArrangementLinesRI2007Request.RIArrangementLines IsNot Nothing) Then
                With UpdateArrangementLinesRI2007Request
                    ReDim oImpRequest.RIArrangementLines(.RIArrangementLines.Count - 1)
                    For iCount As Integer = 0 To .RIArrangementLines.Count - 1
                        oImpRequest.RIArrangementLines(iCount) = New BaseImplementationTypes.BaseRiskRIArrangementLineType
                        oImpRequest.RIArrangementLines(iCount).RIArrangementKey = .RIArrangementLines(iCount).RIArrangementKey
                        oImpRequest.RIArrangementLines(iCount).RIArrangementLineKey = .RIArrangementLines(iCount).RIArrangementLineKey
                        oImpRequest.RIArrangementLines(iCount).RIPlacement = .RIArrangementLines(iCount).RIPlacement
                        oImpRequest.RIArrangementLines(iCount).RIName = .RIArrangementLines(iCount).RIName
                        oImpRequest.RIArrangementLines(iCount).Retained = .RIArrangementLines(iCount).Retained
                        oImpRequest.RIArrangementLines(iCount).DefaultSharePercent = .RIArrangementLines(iCount).DefaultSharePercent / 100

                        oImpRequest.RIArrangementLines(iCount).ThisSharePercent = .RIArrangementLines(iCount).ThisSharePercent / 100
                        oImpRequest.RIArrangementLines(iCount).LowerLimit = .RIArrangementLines(iCount).LowerLimit
                        oImpRequest.RIArrangementLines(iCount).LowerLimitSpecified = .RIArrangementLines(iCount).LowerLimitSpecified

                        oImpRequest.RIArrangementLines(iCount).PartyKeySpecified = .RIArrangementLines(iCount).PartyKeySpecified
                        oImpRequest.RIArrangementLines(iCount).PremiumTaxSpecified = .RIArrangementLines(iCount).PremiumTaxSpecified
                        oImpRequest.RIArrangementLines(iCount).RetainedSpecified = .RIArrangementLines(iCount).RetainedSpecified

                        oImpRequest.RIArrangementLines(iCount).LineLimit = .RIArrangementLines(iCount).LineLimit
                        oImpRequest.RIArrangementLines(iCount).SumInsured = .RIArrangementLines(iCount).SumInsured
                        oImpRequest.RIArrangementLines(iCount).PremiumValue = .RIArrangementLines(iCount).PremiumValue
                        oImpRequest.RIArrangementLines(iCount).PremiumTax = .RIArrangementLines(iCount).PremiumTax
                        oImpRequest.RIArrangementLines(iCount).CommissionTax = .RIArrangementLines(iCount).CommissionTax
                        oImpRequest.RIArrangementLines(iCount).CommissionTaxSpecified = .RIArrangementLines(iCount).CommissionTaxSpecified
                        oImpRequest.RIArrangementLines(iCount).CommissionPercent = .RIArrangementLines(iCount).CommissionPercent / 100
                        oImpRequest.RIArrangementLines(iCount).CommissionValue = .RIArrangementLines(iCount).CommissionValue
                        oImpRequest.RIArrangementLines(iCount).AgreementCode = .RIArrangementLines(iCount).AgreementCode
                        oImpRequest.RIArrangementLines(iCount).IsDomiciledForTax = .RIArrangementLines(iCount).IsDomiciledForTax
                        oImpRequest.RIArrangementLines(iCount).Grouping = .RIArrangementLines(iCount).Grouping
                        oImpRequest.RIArrangementLines(iCount).GroupingSpecified = .RIArrangementLines(iCount).GroupingSpecified
                        oImpRequest.RIArrangementLines(iCount).IsRIBroker = .RIArrangementLines(iCount).IsRIBroker
                        oImpRequest.RIArrangementLines(iCount).ReinsuranceTypeCode = .RIArrangementLines(iCount).ReinsuranceTypeCode
                        oImpRequest.RIArrangementLines(iCount).TreatyCode = .RIArrangementLines(iCount).TreatyCode
                        oImpRequest.RIArrangementLines(iCount).PartyKey = .RIArrangementLines(iCount).PartyKey
                        oImpRequest.RIArrangementLines(iCount).Priority = .RIArrangementLines(iCount).Priority
                        oImpRequest.RIArrangementLines(iCount).NumberOfLines = .RIArrangementLines(iCount).NumberOfLines
                        oImpRequest.RIArrangementLines(iCount).PremiumPercent = .RIArrangementLines(iCount).PremiumPercent / 100
                        oImpRequest.RIArrangementLines(iCount).ParticipationPercent = .RIArrangementLines(iCount).ParticipationPercent
                        oImpRequest.RIArrangementLines(iCount).ParticipationPercentSpecified = .RIArrangementLines(iCount).ParticipationPercentSpecified
                        oImpRequest.RIArrangementLines(iCount).IsCommissionModified = .RIArrangementLines(iCount).IsCommissionModified
                        oImpRequest.RIArrangementLines(iCount).CedePremiumOnly = .RIArrangementLines(iCount).CedePremiumOnly
                        oImpRequest.RIArrangementLines(iCount).Type = .RIArrangementLines(iCount).Type
                        oImpRequest.RIArrangementLines(iCount).ActionType = CType(.RIArrangementLines(iCount).ActionType, BaseImplementationTypes.RowAction)
                        If (.RIArrangementLines(iCount).BrokerParticipants IsNot Nothing) Then
                            ReDim oImpRequest.RIArrangementLines(iCount).BrokerParticipants(.RIArrangementLines(iCount).BrokerParticipants.Count - 1)
                            For iCount1 As Integer = 0 To .RIArrangementLines(iCount).BrokerParticipants.Count - 1
                                oImpRequest.RIArrangementLines(iCount).BrokerParticipants(iCount1) = New BaseImplementationTypes.BaseBrokerParticipants
                                oImpRequest.RIArrangementLines(iCount).BrokerParticipants(iCount1).PartyKey = .RIArrangementLines(iCount).BrokerParticipants(iCount1).PartyKey
                                oImpRequest.RIArrangementLines(iCount).BrokerParticipants(iCount1).PartyCode = .RIArrangementLines(iCount).BrokerParticipants(iCount1).PartyCode
                                oImpRequest.RIArrangementLines(iCount).BrokerParticipants(iCount1).PartyName = .RIArrangementLines(iCount).BrokerParticipants(iCount1).PartyName
                                oImpRequest.RIArrangementLines(iCount).BrokerParticipants(iCount1).ParticpationPercentage = .RIArrangementLines(iCount).BrokerParticipants(iCount1).ParticpationPercentage
                            Next
                        End If

                        If (.RIArrangementLines(iCount).FAXParticipants IsNot Nothing) Then
                            ReDim oImpRequest.RIArrangementLines(iCount).FAXParticipants(.RIArrangementLines(iCount).FAXParticipants.Count - 1)
                            For iCount1 As Integer = 0 To .RIArrangementLines(iCount).FAXParticipants.Count - 1
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1) = New BaseImplementationTypes.BaseFAXParticipants
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).RIArrangementLineKey = .RIArrangementLines(iCount).FAXParticipants(iCount1).RIArrangementLineKey
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).PartyKey = .RIArrangementLines(iCount).FAXParticipants(iCount1).PartyKey
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).PartyCode = .RIArrangementLines(iCount).FAXParticipants(iCount1).PartyCode
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).PartyName = .RIArrangementLines(iCount).FAXParticipants(iCount1).PartyName
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).AccountType = .RIArrangementLines(iCount).FAXParticipants(iCount1).AccountType
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).ParticpationPercentage = .RIArrangementLines(iCount).FAXParticipants(iCount1).ParticpationPercentage
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).CommissionTaxSpecified = .RIArrangementLines(iCount).FAXParticipants(iCount1).CommissionTaxSpecified
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).PremiumTaxSpecified = .RIArrangementLines(iCount).FAXParticipants(iCount1).PremiumTaxSpecified
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).SumInsured = .RIArrangementLines(iCount).FAXParticipants(iCount1).SumInsured
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).PremiumValue = .RIArrangementLines(iCount).FAXParticipants(iCount1).PremiumValue
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).PremiumTax = .RIArrangementLines(iCount).FAXParticipants(iCount1).PremiumTax
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).CommissionPercent = .RIArrangementLines(iCount).FAXParticipants(iCount1).CommissionPercent / 100
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).CommissionTax = .RIArrangementLines(iCount).FAXParticipants(iCount1).CommissionTax
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).CommissionValue = .RIArrangementLines(iCount).FAXParticipants(iCount1).CommissionValue
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).AgreementCode = .RIArrangementLines(iCount).FAXParticipants(iCount1).AgreementCode
                                oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).ActionType = CType(.RIArrangementLines(iCount).FAXParticipants(iCount1).ActionType, BaseImplementationTypes.RowAction)
                                If (.RIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants IsNot Nothing) Then
                                    ReDim oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(.RIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants.Count - 1)
                                    For iCount2 As Integer = 0 To .RIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants.Count - 1
                                        oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2) = New BaseImplementationTypes.BaseBrokerParticipants
                                        oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).PartyKey = .RIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).PartyKey
                                        oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).PartyCode = .RIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).PartyCode
                                        oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).PartyName = .RIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).PartyName
                                        oImpRequest.RIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).ParticpationPercentage = .RIArrangementLines(iCount).FAXParticipants(iCount1).BrokerParticipants(iCount2).ParticpationPercentage
                                    Next
                                End If
                            Next
                        End If
                    Next
                End With
            End If

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateArrangementLinesRI2007(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(UpdateArrangementLinesRI2007Request))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(UpdateArrangementLinesRI2007Request))
            Return Nothing
        End Try
    End Function

    Public Function CalculateRITax(ByVal CalculateRITaxRequest As CalculateRITaxRequestType) As CalculateRITaxResponseType Implements IPurePolicyService.CalculateRITax
        Try

            Dim sUserName As String = CalculateRITaxRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMURI07", iUserId)
            CommonFunctions.CheckSecurityToken(CalculateRITaxRequest.WCFSecurityToken)
            Dim oResponse As New CalculateRITaxResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, CalculateRITaxRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.CalculateRITaxRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.CalculateRITaxResponseType = Nothing

            oImpRequest.BranchCode = CalculateRITaxRequest.BranchCode
            oImpRequest.InsuranceFileKey = CalculateRITaxRequest.InsuranceFileKey
            oImpRequest.RiskKey = CalculateRITaxRequest.RiskKey
            oImpRequest.PartyKey = CalculateRITaxRequest.PartyKey
            oImpRequest.RIArrangementLineKey = CalculateRITaxRequest.RIArrangementLineKey
            oImpRequest.RIArrangementLineKeySpecified = CalculateRITaxRequest.RIArrangementLineKeySpecified
            oImpRequest.Premium = CalculateRITaxRequest.Premium
            oImpRequest.Commission = CalculateRITaxRequest.Commission

            Try
                ' Call the implementation method

                oImpResponse = oBusiness.CalculateRITax(oImpRequest)
                oResponse.CommissionTax = oImpResponse.CommissionTax
                oResponse.PremiumTax = oImpResponse.PremiumTax
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(CalculateRITaxRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(CalculateRITaxRequest))
            Return Nothing
        End Try

    End Function


    Public Function UpdateRiskStatus(ByVal oUpdateRiskStatusRequest As UpdateRiskStatusRequestType) As UpdateRiskStatusResponseType Implements IPurePolicyService.UpdateRiskStatus

        Try

            Dim sUserName As String = oUpdateRiskStatusRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMURsk", iUserId)
            CommonFunctions.CheckSecurityToken(oUpdateRiskStatusRequest.WCFSecurityToken)
            Dim oResponse As New UpdateRiskStatusResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oUpdateRiskStatusRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateRiskStatusRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateRiskStatusResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oUpdateRiskStatusRequest.BranchCode
            If oUpdateRiskStatusRequest.InsuranceFileKeySpecified Then
                oImpRequest.InsuranceFileKey = oUpdateRiskStatusRequest.InsuranceFileKey
            End If
            If oUpdateRiskStatusRequest.RiskKeySpecified Then
                oImpRequest.RiskKey = oUpdateRiskStatusRequest.RiskKey
            End If
            oImpRequest.RiskStatusCode = oUpdateRiskStatusRequest.RiskStatusCode.ToString
            If oUpdateRiskStatusRequest.RiskInceptionDateSpecified Then
                oImpRequest.RiskInceptionDateSpecified = True
                oImpRequest.RiskInceptionDate = oUpdateRiskStatusRequest.RiskInceptionDate
            End If

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateRiskStatus(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdateRiskStatusRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateRiskStatusRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetRiskByProduct(ByVal GetRiskByProductRequest As GetRiskByProductRequestType) As GetRiskByProductResponseType Implements IPurePolicyService.GetRiskByProduct

        Try


            Dim sUserName As String = GetRiskByProductRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGRskPro", iUserId)
            CommonFunctions.CheckSecurityToken(GetRiskByProductRequest.WCFSecurityToken)
            Dim oResponse As New GetRiskByProductResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetRiskByProductRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetRiskByProductRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetRiskByProductResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = GetRiskByProductRequest.BranchCode
            oImpRequest.ProductCode = GetRiskByProductRequest.ProductCode
            oImpRequest.UserName = sUserName
            oImpRequest.WCFSecurityToken = If(GetRiskByProductRequest.WCFSecurityToken.Length > 0, GetRiskByProductRequest.WCFSecurityToken, "WCFSecurityToken")
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetRiskByProduct(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' oResponse.Risks = SAMFunc.GetDeserializedValues(Of List(Of BaseGetRiskByProductResponseTypeRow))(elmResultDataSet:=oImpResponse.Risks, sFromTypeName:="BaseGetRiskByProductResponseTypeRisks", sConvertToTypeName:="BaseGetRiskByProductResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Risks = DataTabletoList_GetRiskByProduct(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetRiskByProductRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetRiskByProductRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetSubAgents(ByVal oGetSubAgentsRequest As GetSubAgentsRequestType) As GetSubAgentsResponseType Implements IPurePolicyService.GetSubAgents

        Try


            Dim sUserName As String = oGetSubAgentsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMAQuot", iUserId)
            CommonFunctions.CheckSecurityToken(oGetSubAgentsRequest.WCFSecurityToken)
            Dim oResponse As New GetSubAgentsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetSubAgentsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetSubAgentsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetSubAgentsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetSubAgentsRequest.BranchCode
            oImpRequest.InsuranceFileKey = oGetSubAgentsRequest.InsuranceFileKey
            oImpRequest.WCFSecurityToken = If(oGetSubAgentsRequest.WCFSecurityToken.Length > 0, oGetSubAgentsRequest.WCFSecurityToken, "WCFSecurityToken")
            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetSubAgents(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'Deserialize the result sets
                'oResponse.SubAgents = SAMFunc.GetDeserializedValues(Of List(Of BaseGetSubAgentsResponseTypeRow))(elmResultDataSet:=oImpResponse.SubAgentsDataset, sFromTypeName:="BaseGetSubAgentsResponseTypeSubAgents", sConvertToTypeName:="BaseGetSubAgentsResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.SubAgents = DataTabletoList_GetSubAgents(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetSubAgentsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetSubAgentsRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' RunRenewalInvitation
    ''' </summary>
    ''' <param name="RunRenewalInviteRequest"></param>
    ''' <remarks></remarks>
    Public Sub RunRenewalInvitation(ByVal RunRenewalInviteRequest As RunRenewalInviteRequestType) Implements IPurePolicyService.RunRenewalInvitation

        Try
            Dim sUserName As String = RunRenewalInviteRequest.LoginUserName
            Dim nAgentKey As Integer = 0
            Dim nUserId As Integer = 0

            CommonFunctions.GetIdentity(sUserName, nAgentKey, nUserId)
            CommonFunctions.CheckAuthority("SAMGHRKey", nUserId)
            CommonFunctions.CheckSecurityToken(RunRenewalInviteRequest.WCFSecurityToken)

            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, RunRenewalInviteRequest.BranchCode)


            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.RunRenewalInviteRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.RunBatchRenewalResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.InsuranceFileKey = RunRenewalInviteRequest.InsuranceFileKey
            oImpRequest.BatchRenewalJobKey = RunRenewalInviteRequest.BatchRenewalJobKey

            ' Removed RenewalDocsDestination field from request type and added 
            ' RecordsCount,GUID as specified in "Renewal Changes.doc" sent by Rahul
            oImpRequest.RecordsCount = RunRenewalInviteRequest.RecordsCount
            oImpRequest.GUID = RunRenewalInviteRequest.GUID

            ' Call the implementation method
            oBusiness.RunRenewalInvitation(oImpRequest)

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(RunRenewalInviteRequest))
        End Try
    End Sub


    ''' <summary>
    ''' CreatePostingsForReinsurance
    ''' </summary>
    ''' <param name="oCreatePostingsForReinsuranceRequest"></param>
    ''' <remarks></remarks>
    Public Function CreatePostingsForReinsurance(ByVal oCreatePostingsForReinsuranceRequest As CreatePostingsForReinsuranceRequestType) As CreatePostingsForReinsuranceResponseType Implements IPurePolicyService.CreatePostingsForReinsurance
        Try
            Dim sUserName As String = oCreatePostingsForReinsuranceRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHRKey", iUserId)
            CommonFunctions.CheckSecurityToken(oCreatePostingsForReinsuranceRequest.WCFSecurityToken)


            Dim oResponse As New CreatePostingsForReinsuranceResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oCreatePostingsForReinsuranceRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.CreatePostingsForReinsuranceRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.CreatePostingsForReinsuranceResponseType = Nothing

            oImpRequest.BranchCode = oCreatePostingsForReinsuranceRequest.BranchCode
            oImpRequest.insuranceFileKey = oCreatePostingsForReinsuranceRequest.insuranceFileKey
            oImpRequest.ProcessType = oCreatePostingsForReinsuranceRequest.ProcessType
            oImpRequest.TransactionDate = oCreatePostingsForReinsuranceRequest.TransactionDate
            oImpRequest.LoginUserName = oCreatePostingsForReinsuranceRequest.LoginUserName
            oImpRequest.TimeStamp = oCreatePostingsForReinsuranceRequest.TimeStamp
            ' Pass the values to the implementation request structure

            ' Call the implementation method
            Try
                oImpResponse = oBusiness.CreatePostingsForReinsurance(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oCreatePostingsForReinsuranceRequest))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oCreatePostingsForReinsuranceRequest))
            Return Nothing
        End Try

    End Function
    ''' <summary>
    ''' UpdateRIAmendmentStatus
    ''' </summary>
    ''' <param name="oUpdateRIAmendmentStatusRequest"></param>
    ''' <remarks></remarks>
    Public Function UpdateRIAmendmentStatus(ByVal oUpdateRIAmendmentStatusRequest As UpdateRIAmendmentStatusRequestType) As UpdateRIAmendmentStatusResponseType Implements IPurePolicyService.UpdateRIAmendmentStatus
        Try
            Dim sUserName As String = oUpdateRIAmendmentStatusRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHRKey", iUserId)
            CommonFunctions.CheckSecurityToken(oUpdateRIAmendmentStatusRequest.WCFSecurityToken)

            Dim oResponse As New UpdateRIAmendmentStatusResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oUpdateRIAmendmentStatusRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateRIAmendmentStatusRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateRIAmendmentStatusResponseType = Nothing

            oImpRequest.BranchCode = oUpdateRIAmendmentStatusRequest.BranchCode
            oImpRequest.insuranceFileKey = oUpdateRIAmendmentStatusRequest.insuranceFileKey
            oImpRequest.ProcessType = oUpdateRIAmendmentStatusRequest.ProcessType
            oImpRequest.Status = oUpdateRIAmendmentStatusRequest.Status
            oImpRequest.LoginUserName = oUpdateRIAmendmentStatusRequest.LoginUserName
            oImpRequest.TimeStamp = oUpdateRIAmendmentStatusRequest.TimeStamp
            ' Pass the values to the implementation request structure

            ' Call the implementation method
            Try
                oImpResponse = oBusiness.UpdateRIAmendmentStatus(oImpRequest)
                oResponse.TimeStamp = oImpResponse.TimeStamp
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdateRIAmendmentStatusRequest))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateRIAmendmentStatusRequest))
            Return Nothing
        End Try

    End Function
    ''' <summary>
    ''' RecalculateRIForPortfolioTransfer
    ''' </summary>
    ''' <param name="oRecalculateRIForPortfolioTransferRequest"></param>
    ''' <remarks></remarks>
    Public Function RecalculateRIForPortfolioTransfer(ByVal oRecalculateRIForPortfolioTransferRequest As RecalculateRIForPortfolioTransferRequestType) As RecalculateRIForPortfolioTransferResponseType Implements IPurePolicyService.RecalculateRIForPortfolioTransfer
        Try
            Dim sUserName As String = oRecalculateRIForPortfolioTransferRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHRKey", iUserId)
            CommonFunctions.CheckSecurityToken(oRecalculateRIForPortfolioTransferRequest.WCFSecurityToken)

            Dim oResponse As New RecalculateRIForPortfolioTransferResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oRecalculateRIForPortfolioTransferRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.RecalculateRIForPortfolioTransferRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.RecalculateRIForPortfolioTransferResponseType = Nothing

            oImpRequest.BranchCode = oRecalculateRIForPortfolioTransferRequest.BranchCode
            oImpRequest.InsuranceFileKey = oRecalculateRIForPortfolioTransferRequest.InsuranceFileKey
            oImpRequest.TransactionDate = oRecalculateRIForPortfolioTransferRequest.TransactionDate
            oImpRequest.LoginUserName = oRecalculateRIForPortfolioTransferRequest.LoginUserName
            oImpRequest.TimeStamp = oRecalculateRIForPortfolioTransferRequest.TimeStamp
            ' Pass the values to the implementation request structure

            ' Call the implementation method
            Try
                oImpResponse = oBusiness.RecalculateRIForPortfolioTransfer(oImpRequest)
                oResponse.TimeStamp = oImpResponse.TimeStamp
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oRecalculateRIForPortfolioTransferRequest))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oRecalculateRIForPortfolioTransferRequest))
            Return Nothing
        End Try

    End Function
    ''' <summary>
    ''' RecalculateRIForCloneTransfer
    ''' </summary>
    ''' <param name="oRecalculateRIForCloneTransferRequest"></param>
    ''' <remarks></remarks>
    Public Function RecalculateRIForCloneTransfer(ByVal oRecalculateRIForCloneTransferRequest As RecalculateRIForCloneTransferRequestType) As RecalculateRIForCloneTransferResponseType Implements IPurePolicyService.RecalculateRIForCloneTransfer
        Try
            Dim sUserName As String = oRecalculateRIForCloneTransferRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHRKey", iUserId)
            CommonFunctions.CheckSecurityToken(oRecalculateRIForCloneTransferRequest.WCFSecurityToken)

            Dim oResponse As New RecalculateRIForCloneTransferResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oRecalculateRIForCloneTransferRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.RecalculateRIForCloneTransferRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.RecalculateRIForCloneTransferResponseType = Nothing

            oImpRequest.BranchCode = oRecalculateRIForCloneTransferRequest.BranchCode
            oImpRequest.InsuranceFileKey = oRecalculateRIForCloneTransferRequest.InsuranceFileKey
            oImpRequest.LoginUserName = oRecalculateRIForCloneTransferRequest.LoginUserName
            oImpRequest.TransactionDate = oRecalculateRIForCloneTransferRequest.TransactionDate
            oImpRequest.TimeStamp = oRecalculateRIForCloneTransferRequest.TimeStamp
            ' Pass the values to the implementation request structure

            ' Call the implementation method
            Try
                oImpResponse = oBusiness.RecalculateRIForCloneTransfer(oImpRequest)
                oResponse.TimeStamp = oImpResponse.TimeStamp
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oRecalculateRIForCloneTransferRequest))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oRecalculateRIForCloneTransferRequest))
            Return Nothing
        End Try

    End Function
    ''' <summary>
    ''' GetPTPoliciesForAmend
    ''' </summary>
    ''' <param name="oGetPTPoliciesForAmendRequest"></param>
    ''' <remarks></remarks>
    Public Function GetPTPoliciesForAmend(ByVal oGetPTPoliciesForAmendRequest As GetPTPoliciesForAmendRequestType) As GetPTPoliciesForAmendResponseType Implements IPurePolicyService.GetPTPoliciesForAmend
        Try
            Dim sUserName As String = oGetPTPoliciesForAmendRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHRKey", iUserId)
            CommonFunctions.CheckSecurityToken(oGetPTPoliciesForAmendRequest.WCFSecurityToken)

            Dim oResponse As New GetPTPoliciesForAmendResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetPTPoliciesForAmendRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetPTPoliciesForAmendRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetPTPoliciesForAmendResponseType = Nothing

            oImpRequest.BranchCode = oGetPTPoliciesForAmendRequest.BranchCode
            oImpRequest.policyNumber = oGetPTPoliciesForAmendRequest.PolicyNumber
            oImpRequest.LoginUserName = oGetPTPoliciesForAmendRequest.LoginUserName
            ' Pass the values to the implementation request structure

            ' Call the implementation method
            Try
                oImpResponse = oBusiness.GetPTPoliciesForAmend(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)


                If oImpResponse.ResultDataset IsNot Nothing Then

                    oResponse.Policies = SAMFunc.GetDeserializedValues(Of List(Of BaseGetPTPoliciesForAmendResponseTypePoliciesRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetPTPoliciesForAmendResponseTypePolicies", sConvertToTypeName:="BaseGetPTPoliciesForAmendResponseTypePoliciesRow")

                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetPTPoliciesForAmendRequest))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetPTPoliciesForAmendRequest))
            Return Nothing
        End Try

    End Function
    ''' <summary>
    ''' GetClonePoliciesForAmend
    ''' </summary>
    ''' <param name="oGetClonePoliciesForAmendRequest"></param>
    ''' <remarks></remarks>   
    Public Function GetClonePoliciesForAmend(ByVal oGetClonePoliciesForAmendRequest As GetClonePoliciesForAmendRequestType) As GetClonePoliciesForAmendResponseType Implements IPurePolicyService.GetClonePoliciesForAmend
        Try
            Dim sUserName As String = oGetClonePoliciesForAmendRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHRKey", iUserId)
            CommonFunctions.CheckSecurityToken(oGetClonePoliciesForAmendRequest.WCFSecurityToken)


            Dim oResponse As New GetClonePoliciesForAmendResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetClonePoliciesForAmendRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetClonePoliciesForAmendRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetClonePoliciesForAmendResponseType = Nothing

            oImpRequest.BranchCode = oGetClonePoliciesForAmendRequest.BranchCode
            oImpRequest.policyNumber = oGetClonePoliciesForAmendRequest.policyNumber
            oImpRequest.LoginUserName = oGetClonePoliciesForAmendRequest.LoginUserName
            ' Pass the values to the implementation request structure

            ' Call the implementation method
            Try
                oImpResponse = oBusiness.GetClonePoliciesForAmend(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)


                If oImpResponse.ResultDataset IsNot Nothing Then

                    oResponse.Policies = SAMFunc.GetDeserializedValues(Of List(Of BaseGetClonePoliciesForAmendResponseTypePoliciesRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetClonePoliciesForAmendResponseTypePolicies", sConvertToTypeName:="BaseGetClonePoliciesForAmendResponseTypePoliciesRow")

                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetClonePoliciesForAmendRequest))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetClonePoliciesForAmendRequest))
            Return Nothing
        End Try

    End Function
    ''' <summary>
    ''' IsPendingTransfer
    ''' </summary>
    ''' <param name="oPendingTransferRequest"></param>
    ''' <remarks></remarks>   
    Public Function IsPendingTransfer(ByVal oPendingTransferRequest As IsPendingTransferRequestType) As IsPendingTransferResponseType Implements IPurePolicyService.IsPendingTransfer
        Try
            Dim sUserName As String = oPendingTransferRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHRKey", iUserId)
            CommonFunctions.CheckSecurityToken(oPendingTransferRequest.WCFSecurityToken)

            Dim oResponse As New IsPendingTransferResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oPendingTransferRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.IsPendingTransferRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.IsPendingTransferResponseType = Nothing

            oImpRequest.BranchCode = oPendingTransferRequest.BranchCode
            oImpRequest.InsuranceFileKey = oPendingTransferRequest.InsuranceFileKey
            oImpRequest.PolicyNumber = oPendingTransferRequest.PolicyNumber
            oImpRequest.LoginUserName = oPendingTransferRequest.LoginUserName

            ' Pass the values to the implementation request structure

            ' Call the implementation method
            Try
                oImpResponse = oBusiness.IsPendingTransfer(oImpRequest)

                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.IsPendingCloneTransfer = oImpResponse.IsPendingCloneTransfer
                oResponse.IsPendingPortfolioTransfer = oImpResponse.IsPendingPortfolioTransfer
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oPendingTransferRequest))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oPendingTransferRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' IsAnniversaryDateEditable
    ''' </summary>
    ''' <param name="oAnniversaryDateEditableRequest"></param>
    ''' <remarks></remarks>   
    Public Function IsAnniversaryDateEditable(ByVal oAnniversaryDateEditableRequest As IsAnniversaryDateEditableRequestType) As IsAnniversaryDateEditableResponseType Implements IPurePolicyService.IsAnniversaryDateEditable
        Try
            Dim sUserName As String = oAnniversaryDateEditableRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHRKey", iUserId)
            CommonFunctions.CheckSecurityToken(oAnniversaryDateEditableRequest.WCFSecurityToken)

            Dim oResponse As New IsAnniversaryDateEditableResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oAnniversaryDateEditableRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.IsAnniversaryDateEditableRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.IsAnniversaryDateEditableResponseType = Nothing

            oImpRequest.BranchCode = oAnniversaryDateEditableRequest.BranchCode
            oImpRequest.InsuranceFileKey = oAnniversaryDateEditableRequest.InsuranceFileKey
            oImpRequest.LoginUserName = oAnniversaryDateEditableRequest.LoginUserName

            ' Pass the values to the implementation request structure

            ' Call the implementation method
            Try
                oImpResponse = oBusiness.IsAnniversaryDateEditable(oImpRequest)

                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                oResponse.IsAnniversaryDateEditable = oImpResponse.IsAnniversaryDateEditable

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oAnniversaryDateEditableRequest))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oAnniversaryDateEditableRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' GetRIVersion
    ''' </summary>
    ''' <param name="oGetRIVersionRequest"></param>
    ''' <remarks></remarks>   
    Public Function GetRIVersion(ByVal oGetRIVersionRequest As GetRIVersionRequestType) As GetRIVersionResponseType Implements IPurePolicyService.GetRIVersion
        Try
            Dim sUserName As String = oGetRIVersionRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHRKey", iUserId)
            CommonFunctions.CheckSecurityToken(oGetRIVersionRequest.WCFSecurityToken)

            Dim oResponse As New GetRIVersionResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetRIVersionRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetRIVersionRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetRIVersionResponseType = Nothing

            oImpRequest.BranchCode = oGetRIVersionRequest.BranchCode
            oImpRequest.RiskCnt = oGetRIVersionRequest.RiskCnt
            oImpRequest.LoginUserName = oGetRIVersionRequest.LoginUserName

            ' Pass the values to the implementation request structure
            ' Call the implementation method
            Try
                oImpResponse = oBusiness.GetRIVersion(oImpRequest)

                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                If oImpResponse.ResultDataset IsNot Nothing Then

                    oResponse.ResultData = SAMFunc.GetDeserializedValues(Of List(Of BaseGetRIVersionResponseTypeRIVersionsRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetRIVersionResponseTypeRIVersions", sConvertToTypeName:="BaseGetRIVersionResponseTypeRIVersionsRow")

                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetRIVersionRequest))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetRIVersionRequest))
            Return Nothing
        End Try

    End Function
    ''' <summary>
    ''' RunPortfolioTransfer
    ''' </summary>
    ''' <param name="request"></param>
    ''' <remarks></remarks>   
    Public Function RunPortfolioTransfer(ByVal request As RunPortfolioTransferRequestType) As RunPortfolioTransferResponseType Implements IPurePolicyService.RunPortfolioTransfer

        Try
            Dim sUserName As String = request.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHRKey", iUserId)
            CommonFunctions.CheckSecurityToken(request.WCFSecurityToken)

            Dim oResponse As New RunPortfolioTransferResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, request.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.RunPortfolioTransferRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.RunPortfolioTransferResponseType = Nothing


            oImpRequest.BranchCode = request.BranchCode
            oImpRequest.InsuranceFileKey = request.InsuranceFileKey
            oImpRequest.InsuranceFileType = request.InsuranceFileType
            oImpRequest.ClaimKey = request.ClaimKey
            oImpRequest.ProductKey = request.ProductKey
            oImpRequest.TransferDate = request.TransferDate
            oImpRequest.StartDate = request.StartDate
            oImpRequest.EndDate = request.EndDate
            oImpRequest.InceptionDate = request.InceptionDate
            oImpRequest.SkipPostings = request.SkipPostings
            oImpRequest.LoginUserName = request.LoginUserName

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.RunPortfolioTransfer(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(request))
            End Try

            oResponse.IsFailed = oImpResponse.IsFailed

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(request))
            Return Nothing
        End Try
    End Function


    ''' <summary>
    ''' RunCloneRework
    ''' </summary>
    ''' <param name="request"></param>
    ''' <remarks></remarks>   
    Public Function RunCloneRework(ByVal request As RunCloneReworkRequestType) As RunCloneReworkResponseType Implements IPurePolicyService.RunCloneRework

        Try
            Dim sUserName As String = request.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGHRKey", iUserId)
            CommonFunctions.CheckSecurityToken(request.WCFSecurityToken)

            Dim oResponse As New RunCloneReworkResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, request.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.RunCloneReworkRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.RunCloneReworkResponseType = Nothing

            oImpRequest.BranchCode = request.BranchCode
            oImpRequest.InsuranceFileKey = request.InsuranceFileKey
            oImpRequest.InsuranceFileType = request.InsuranceFileType
            oImpRequest.RiskKey = request.RiskKey
            oImpRequest.ClaimKey = request.ClaimKey
            oImpRequest.IsFailed = request.IsFailed
            oImpRequest.LoginUserName = request.LoginUserName

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.RunCloneRework(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(request))
            End Try

            oResponse.IsFailed = oImpResponse.IsFailed

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(request))
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' RunBatchRenewal
    ''' </summary>
    ''' <param name="request"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RunBatchRenewal(ByVal request As RunBatchRenewalRequestType) As RunBatchRenewalResponseType Implements IPurePolicyService.RunBatchRenewal

        Try
            Dim sUserName As String = request.LoginUserName
            Dim nAgentKey As Integer = 0
            Dim nUserId As Integer = 0
            CommonFunctions.GetIdentity(sUserName, nAgentKey, nUserId)
            CommonFunctions.CheckAuthority("SAMGHRKey", nUserId)
            CommonFunctions.CheckSecurityToken(request.WCFSecurityToken)
            Dim oResponse As New RunBatchRenewalResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, request.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.RunBatchRenewalRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.RunBatchRenewalResponseType = Nothing

            oImpRequest.BatchId = request.batchId
            oImpRequest.BranchCode = request.BranchCode
            oImpRequest.InsuranceFolderKey = request.insuranceFolderKey
            oImpRequest.LoginUserName = sUserName

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.RunBatchRenewal(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(request))
            End Try

            oResponse.insuranceFileKey = oImpResponse.insuranceFileKey
            oResponse.message = oImpResponse.message
            oResponse.result = oImpResponse.result

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(request))
        End Try
    End Function




#Region "RunRenewalSelectionSync"

    Public Function RunRenewalSelectionSync(ByVal RunRenewalSelectionSyncRequest As RunRenewalSelectionSyncRequestType) Implements IPurePolicyService.RunRenewalSelectionSync
        SyncLock m_Sellock

            Dim oResponse As New RunRenewalSelectionSyncResponseType
            Try

                Dim sUserName As String = String.Empty
                Dim iAgentKey As Integer
                Dim iUserId As Integer

                CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
                CommonFunctions.CheckAuthority("SAMGHRKey", iUserId)
                CommonFunctions.CheckSecurityToken(RunRenewalSelectionSyncRequest.WCFSecurityToken)
                Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, RunRenewalSelectionSyncRequest.BranchCode)
                ' Implementation structures
                Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.RunRenewalSelectionSyncRequestType
                Dim oImpResponse As New SAMForInsuranceV2ImplementationTypes.RunRenewalSelectionSyncResponseType

                ' Pass the values to the implementation request structure
                oImpRequest.InsuranceFileKey = RunRenewalSelectionSyncRequest.InsuranceFileKey
                oImpRequest.BatchRenewalJobKey = RunRenewalSelectionSyncRequest.BatchRenewalJobKey

                ' Removed IsProcessStart, GenerateReport, ReportSortOrder fields from request type and added
                ' RecordsCount,GUID as specified in "Renewal Changes.doc" sent by Rahul 
                oImpRequest.RecordsCount = RunRenewalSelectionSyncRequest.RecordsCount
                oImpRequest.GUID = RunRenewalSelectionSyncRequest.GUID

                ' Call the implementation method
                oBusiness.RunRenewalSelection(oImpRequest)
                oImpResponse.IsProcessed = True
                oResponse.IsProcessed = oImpResponse.IsProcessed
                Return oResponse
            Catch ex As Exception
                CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(RunRenewalSelectionSyncRequest))
                oResponse.IsProcessed = False
                Return oResponse
            End Try
        End SyncLock
    End Function
#End Region

#Region " RunRenewalInvitationSync "

    Public Function RunRenewalInvitationSync(ByVal RunRenewalInviteSyncRequest As RunRenewalInviteSyncRequestType) Implements IPurePolicyService.RunRenewalInvitationSync
        SyncLock m_Invlock

            Dim oResponse As New RunRenewalInviteSyncResponseType

            Try

                Dim sUserName As String = String.Empty
                Dim iAgentKey As Integer
                Dim iUserId As Integer
                CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
                CommonFunctions.CheckAuthority("SAMGHRKey", iUserId)
                Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, RunRenewalInviteSyncRequest.BranchCode)

                ' Implementation structures
                Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.RunRenewalInviteSyncRequestType
                Dim oImpResponse As New SAMForInsuranceV2ImplementationTypes.RunRenewalInviteSyncResponseType

                ' Pass the values to the implementation request structure
                oImpRequest.InsuranceFileKey = RunRenewalInviteSyncRequest.InsuranceFileKey
                oImpRequest.BatchRenewalJobKey = RunRenewalInviteSyncRequest.BatchRenewalJobKey

                ' Removed RenewalDocsDestination field from request type and added 
                ' RecordsCount,GUID as specified in "Renewal Changes.doc" sent by Rahul
                oImpRequest.RecordsCount = RunRenewalInviteSyncRequest.RecordsCount
                oImpRequest.GUID = RunRenewalInviteSyncRequest.GUID

                ' Call the implementation method
                oBusiness.RunRenewalInvitation(oImpRequest)
                oImpResponse.IsProcessed = True
                oResponse.IsProcessed = oImpResponse.IsProcessed
                Return oResponse
            Catch ex As Exception
                CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(RunRenewalInviteSyncRequest))
                oResponse.IsProcessed = False
                Return oResponse
            End Try
        End SyncLock
    End Function
#End Region

#Region " RunRenewalAcceptSync "

    Public Function RunRenewalAcceptSync(ByVal RunRenewalAcceptSyncRequest As RunRenewalAcceptSyncRequestType) Implements IPurePolicyService.RunRenewalAcceptSync
        SyncLock m_Acclock

            Dim oResponse As New RunRenewalAcceptSyncResponseType

            Try

                Dim sUserName As String = String.Empty
                Dim iAgentKey As Integer
                Dim iUserId As Integer

                CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
                CommonFunctions.CheckAuthority("SAMGHRKey", iUserId)

                Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, RunRenewalAcceptSyncRequest.BranchCode)

                ' Implementation structures
                Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.RunRenewalAcceptSyncRequestType
                Dim oImpResponse As New SAMForInsuranceV2ImplementationTypes.RunRenewalAcceptSyncResponseType

                ' Pass the values to the implementation request structure
                oImpRequest.InsuranceFileKey = RunRenewalAcceptSyncRequest.InsuranceFileKey
                oImpRequest.BatchRenewalJobKey = RunRenewalAcceptSyncRequest.BatchRenewalJobKey

                ' Removed RenewalDocsDestination field from request type and added 
                ' RecordsCount,GUID as specified in "Renewal Changes.doc" sent by Rahul
                oImpRequest.RecordsCount = RunRenewalAcceptSyncRequest.RecordsCount
                oImpRequest.GUID = RunRenewalAcceptSyncRequest.GUID

                ' Call the implementation method
                oBusiness.RunRenewalAccept(oImpRequest)
                oImpResponse.IsProcessed = True
                oResponse.IsProcessed = oImpResponse.IsProcessed
                Return oResponse

            Catch ex As Exception
                CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(RunRenewalAcceptSyncRequest))
                oResponse.IsProcessed = False
                Return oResponse
            End Try
        End SyncLock
    End Function
#End Region


    ''' <summary>
    ''' To update RI arrange lines
    ''' </summary>
    ''' <param name="UpdateArrangementLinesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateArrangementLines(ByVal UpdateArrangementLinesRequest As UpdateArrangementLinesRequestType) As UpdateArrangementLinesResponseType Implements IPurePolicyService.UpdateArrangementLines
        Try

            Dim sUserName As String = UpdateArrangementLinesRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMURI07", iUserId)
            CommonFunctions.CheckSecurityToken(UpdateArrangementLinesRequest.WCFSecurityToken)

            Dim oResponse As New UpdateArrangementLinesResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, UpdateArrangementLinesRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateArrangementLinesRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateArrangementLinesResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = UpdateArrangementLinesRequest.BranchCode
            If (UpdateArrangementLinesRequest.RIArrangementLines IsNot Nothing) Then
                With UpdateArrangementLinesRequest
                    ReDim oImpRequest.RIArrangementLines(.RIArrangementLines.Count - 1)
                    For iCount As Integer = 0 To .RIArrangementLines.Count - 1
                        oImpRequest.RIArrangementLines(iCount) = New BaseImplementationTypes.BaseRiskRIArrangementLineType
                        oImpRequest.RIArrangementLines(iCount).RIArrangementKey = .RIArrangementLines(iCount).RIArrangementKey
                        oImpRequest.RIArrangementLines(iCount).RIArrangementLineKey = .RIArrangementLines(iCount).RIArrangementLineKey
                        oImpRequest.RIArrangementLines(iCount).RIPlacement = .RIArrangementLines(iCount).RIPlacement
                        oImpRequest.RIArrangementLines(iCount).RIName = .RIArrangementLines(iCount).RIName
                        oImpRequest.RIArrangementLines(iCount).Retained = .RIArrangementLines(iCount).Retained
                        oImpRequest.RIArrangementLines(iCount).DefaultSharePercent = .RIArrangementLines(iCount).DefaultSharePercent / 100

                        oImpRequest.RIArrangementLines(iCount).ThisSharePercent = .RIArrangementLines(iCount).ThisSharePercent / 100
                        oImpRequest.RIArrangementLines(iCount).LowerLimit = .RIArrangementLines(iCount).LowerLimit
                        oImpRequest.RIArrangementLines(iCount).LowerLimitSpecified = .RIArrangementLines(iCount).LowerLimitSpecified

                        oImpRequest.RIArrangementLines(iCount).PartyKey = .RIArrangementLines(iCount).PartyKey
                        oImpRequest.RIArrangementLines(iCount).PartyKeySpecified = .RIArrangementLines(iCount).PartyKeySpecified
                        oImpRequest.RIArrangementLines(iCount).PremiumTaxSpecified = .RIArrangementLines(iCount).PremiumTaxSpecified
                        oImpRequest.RIArrangementLines(iCount).RetainedSpecified = .RIArrangementLines(iCount).RetainedSpecified

                        oImpRequest.RIArrangementLines(iCount).LineLimit = .RIArrangementLines(iCount).LineLimit
                        oImpRequest.RIArrangementLines(iCount).SumInsured = .RIArrangementLines(iCount).SumInsured
                        oImpRequest.RIArrangementLines(iCount).PremiumValue = .RIArrangementLines(iCount).PremiumValue
                        oImpRequest.RIArrangementLines(iCount).PremiumTax = .RIArrangementLines(iCount).PremiumTax
                        oImpRequest.RIArrangementLines(iCount).CommissionTax = .RIArrangementLines(iCount).CommissionTax
                        oImpRequest.RIArrangementLines(iCount).CommissionTaxSpecified = .RIArrangementLines(iCount).CommissionTaxSpecified
                        oImpRequest.RIArrangementLines(iCount).CommissionPercent = .RIArrangementLines(iCount).CommissionPercent / 100
                        oImpRequest.RIArrangementLines(iCount).CommissionValue = .RIArrangementLines(iCount).CommissionValue
                        oImpRequest.RIArrangementLines(iCount).AgreementCode = .RIArrangementLines(iCount).AgreementCode
                        oImpRequest.RIArrangementLines(iCount).IsDomiciledForTax = .RIArrangementLines(iCount).IsDomiciledForTax
                        oImpRequest.RIArrangementLines(iCount).ReinsuranceTypeCode = .RIArrangementLines(iCount).ReinsuranceTypeCode
                        oImpRequest.RIArrangementLines(iCount).TreatyCode = .RIArrangementLines(iCount).TreatyCode

                        oImpRequest.RIArrangementLines(iCount).Priority = .RIArrangementLines(iCount).Priority
                        oImpRequest.RIArrangementLines(iCount).NumberOfLines = .RIArrangementLines(iCount).NumberOfLines
                        oImpRequest.RIArrangementLines(iCount).PremiumPercent = .RIArrangementLines(iCount).PremiumPercent / 100
                        oImpRequest.RIArrangementLines(iCount).ParticipationPercent = .RIArrangementLines(iCount).ParticipationPercent
                        oImpRequest.RIArrangementLines(iCount).ParticipationPercentSpecified = .RIArrangementLines(iCount).ParticipationPercentSpecified
                        oImpRequest.RIArrangementLines(iCount).IsCommissionModified = .RIArrangementLines(iCount).IsCommissionModified
                        oImpRequest.RIArrangementLines(iCount).CedePremiumOnly = .RIArrangementLines(iCount).CedePremiumOnly
                        oImpRequest.RIArrangementLines(iCount).Type = .RIArrangementLines(iCount).Type
                        oImpRequest.RIArrangementLines(iCount).RiOverrideReasonId = .RIArrangementLines(iCount).RiOverrideReasonId
                        oImpRequest.RIArrangementLines(iCount).ActionType = CType(.RIArrangementLines(iCount).ActionType, BaseImplementationTypes.RowAction)
                    Next
                End With
            End If

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateArrangementLines(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(UpdateArrangementLinesRequest))
            Return Nothing
        End Try
    End Function

#Region "GetHeaderAndSummariesPFPlanByKey"

    ''' <summary>
    ''' Get Instalment policy details
    ''' </summary>
    ''' <param name="r_oGetHeaderAndSummariesPFPlanByKeyRequestType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetHeaderAndSummariesPFPlanByKey(ByVal r_oGetHeaderAndSummariesPFPlanByKeyRequestType As GetHeaderAndSummariesPFPlanByKeyRequestType) As GetHeaderAndSummariesPFPlanByKeyResponseType Implements IPurePolicyService.GetHeaderAndSummariesPFPlanByKey

        Dim sUserName As String = r_oGetHeaderAndSummariesPFPlanByKeyRequestType.LoginUserName
        Dim iAgentKey As Integer
        Dim iUserId As Integer

        Try

            Dim oResponse As New GetHeaderAndSummariesPFPlanByKeyResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, r_oGetHeaderAndSummariesPFPlanByKeyRequestType.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetHeaderAndSummariesPFPlanByKeyRequestType
            Dim oImpResponse As New SAMForInsuranceV2ImplementationTypes.GetHeaderAndSummariesPFPlanByKeyResponseType

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGETPF", iUserId)
            CommonFunctions.CheckSecurityToken(r_oGetHeaderAndSummariesPFPlanByKeyRequestType.WCFSecurityToken)

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = r_oGetHeaderAndSummariesPFPlanByKeyRequestType.BranchCode
            oImpRequest.PFPremiumFinanceKey = r_oGetHeaderAndSummariesPFPlanByKeyRequestType.PFPremiumFinanceKey
            oImpRequest.PFPremiumFinanceVersionKey = r_oGetHeaderAndSummariesPFPlanByKeyRequestType.PFPremiumFinanceVersionKey
            oImpRequest.DocumentRef = r_oGetHeaderAndSummariesPFPlanByKeyRequestType.DocumentRef
            oImpRequest.UserID = r_oGetHeaderAndSummariesPFPlanByKeyRequestType.UserID
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetHeaderAndSummariesPFPlanByKey(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'Setting the ImpResponse To Portal Response(start)
                If oImpResponse.PremiumFinanceDetails IsNot Nothing AndAlso oImpResponse.PremiumFinanceDetails.PFPremiumFinanceKey > 0 Then
                    oResponse.PremiumFinanceDetails = New SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.WCF.BasePremiumFinancePlanDetailsType
                    With oResponse.PremiumFinanceDetails
                        .PFPremiumFinanceKey = oImpResponse.PremiumFinanceDetails.PFPremiumFinanceKey
                        .PFPremiumFinanceVersionKey = oImpResponse.PremiumFinanceDetails.PFPremiumFinanceVersionKey
                        .PFSchemeName = oImpResponse.PremiumFinanceDetails.PFSchemeName
                        .StartDate = oImpResponse.PremiumFinanceDetails.StartDate
                        .EndDate = oImpResponse.PremiumFinanceDetails.EndDate
                        .ProductClass = oImpResponse.PremiumFinanceDetails.ProductClass
                        .TransType = oImpResponse.PremiumFinanceDetails.TransType
                        .FinanceAmount = oImpResponse.PremiumFinanceDetails.FinanceAmount
                        .APR = oImpResponse.PremiumFinanceDetails.APR
                        .InterestRate = oImpResponse.PremiumFinanceDetails.InterestRate
                        .DaysDelay = oImpResponse.PremiumFinanceDetails.DaysDelay
                        .NoOfInstallments = oImpResponse.PremiumFinanceDetails.NoOfInstallments
                        .FirstInstallment = oImpResponse.PremiumFinanceDetails.FirstInstallment
                        .OtherInstallments = oImpResponse.PremiumFinanceDetails.OtherInstallments
                        .CostOfProtection = oImpResponse.PremiumFinanceDetails.CostOfProtection
                        .Deposit = oImpResponse.PremiumFinanceDetails.Deposit
                        .NetAmount = oImpResponse.PremiumFinanceDetails.NetAmount
                        .TotalCost = oImpResponse.PremiumFinanceDetails.TotalCost
                        .InterestCost = oImpResponse.PremiumFinanceDetails.InterestCost
                        .MinFinanceChrge = oImpResponse.PremiumFinanceDetails.MinFinanceChrge
                        .PayProtection = oImpResponse.PremiumFinanceDetails.PayProtection
                        .ClientName = oImpResponse.PremiumFinanceDetails.ClientName
                        .ClientAddress1 = oImpResponse.PremiumFinanceDetails.ClientAddress1
                        .ClientAddress2 = oImpResponse.PremiumFinanceDetails.ClientAddress2
                        .ClientAddress3 = oImpResponse.PremiumFinanceDetails.ClientAddress3
                        .ClientAddress4 = oImpResponse.PremiumFinanceDetails.ClientAddress4
                        .ClientTown = oImpResponse.PremiumFinanceDetails.ClientTown
                        .ClientPcode = oImpResponse.PremiumFinanceDetails.ClientPcode
                        .ClientCountry = oImpResponse.PremiumFinanceDetails.ClientCountry
                        .ClientAreaCode = oImpResponse.PremiumFinanceDetails.ClientAreaCode
                        .ClientPhoneNo = oImpResponse.PremiumFinanceDetails.ClientPhoneNo
                        .ClientExtension = oImpResponse.PremiumFinanceDetails.ClientExtension
                        .ClientFaxAreaCode = oImpResponse.PremiumFinanceDetails.ClientFaxAreaCode
                        .ClientFaxNo = oImpResponse.PremiumFinanceDetails.ClientFaxNo
                        .BankName = oImpResponse.PremiumFinanceDetails.BankName
                        .BankSortCode = oImpResponse.PremiumFinanceDetails.BankSortCode
                        .BankAccountNo = oImpResponse.PremiumFinanceDetails.BankAccountNo
                        .BankAccountName = oImpResponse.PremiumFinanceDetails.BankAccountName
                        .BankBranch = oImpResponse.PremiumFinanceDetails.BankBranch
                        .BankAddress1 = oImpResponse.PremiumFinanceDetails.BankAddress1
                        .BankAddress2 = oImpResponse.PremiumFinanceDetails.BankAddress2
                        .BankAddress3 = oImpResponse.PremiumFinanceDetails.BankAddress3
                        .BankTown = oImpResponse.PremiumFinanceDetails.BankTown
                        .BankRegion = oImpResponse.PremiumFinanceDetails.BankRegion
                        .BankPostCode = oImpResponse.PremiumFinanceDetails.BankPostCode
                        .BankCountry = oImpResponse.PremiumFinanceDetails.BankCountry
                        .BankAreaCode = oImpResponse.PremiumFinanceDetails.BankAreaCode
                        .BankPhoneNo = oImpResponse.PremiumFinanceDetails.BankPhoneNo
                        .BankExtension = oImpResponse.PremiumFinanceDetails.BankExtension
                        .BankFaxAreaCode = oImpResponse.PremiumFinanceDetails.BankFaxAreaCode
                        .BankFaxNo = oImpResponse.PremiumFinanceDetails.BankFaxNo
                        .StatusInd = CType(oImpResponse.PremiumFinanceDetails.StatusInd, FinancePlanStatus)
                        .ClientCode = oImpResponse.PremiumFinanceDetails.ClientCode
                        .AutoGeneratedPlanRef = oImpResponse.PremiumFinanceDetails.AutoGeneratedPlanRef
                        .FinanceCollatedPlanRef = oImpResponse.PremiumFinanceDetails.FinanceCollatedPlanRef
                        .InterestFree = oImpResponse.PremiumFinanceDetails.InterestFree
                        .IsQuote = oImpResponse.PremiumFinanceDetails.IsQuote
                        .PlanTransactionKey = oImpResponse.PremiumFinanceDetails.PlanTransactionKey
                        .InsuranceFileKey = oImpResponse.PremiumFinanceDetails.InsuranceFileKey
                        .PFRFKEY = oImpResponse.PremiumFinanceDetails.PFRFKEY
                        .PFFrequencyCode = oImpResponse.PremiumFinanceDetails.PFFrequencyCode
                        .BankCountryKey = oImpResponse.PremiumFinanceDetails.BankCountryKey
                        .ClientCountryKey = oImpResponse.PremiumFinanceDetails.ClientCountryKey
                        .FirstInstalmentDate = oImpResponse.PremiumFinanceDetails.FirstInstalmentDate
                        .NextInstalmentDate = oImpResponse.PremiumFinanceDetails.NextInstalmentDate
                        .LastInstalmentDate = oImpResponse.PremiumFinanceDetails.LastInstalmentDate
                        .TaxCost = oImpResponse.PremiumFinanceDetails.TaxCost
                        .MediaTypeCode = oImpResponse.PremiumFinanceDetails.MediaTypeCode
                        .CCNumber = oImpResponse.PremiumFinanceDetails.CCNumber
                        .CCExpiryDate = oImpResponse.PremiumFinanceDetails.CCExpiryDate
                        .CCStartDate = oImpResponse.PremiumFinanceDetails.CCStartDate
                        .CCIssue = oImpResponse.PremiumFinanceDetails.CCIssue
                        .CCPin = oImpResponse.PremiumFinanceDetails.CCPin
                        .MediaTypeValidationCode = oImpResponse.PremiumFinanceDetails.MediaTypeValidationCode
                        .bank_name_mandatory = oImpResponse.PremiumFinanceDetails.bank_name_mandatory
                        .IsBankAddressMandatory = oImpResponse.PremiumFinanceDetails.IsBankAddressMandatory
                        .IsBranchNameMandatory = oImpResponse.PremiumFinanceDetails.IsBranchNameMandatory
                        .IsBranchCodeMandatory = oImpResponse.PremiumFinanceDetails.IsBranchCodeMandatory
                        .CommissionTransdetailKey = oImpResponse.PremiumFinanceDetails.CommissionTransdetailKey
                        .PFFrequencyPeriod = oImpResponse.PremiumFinanceDetails.PFFrequencyPeriod
                        .CommissionTransdetailKey = oImpResponse.PremiumFinanceDetails.CommissionTransdetailKey
                        .PFFrequencyAmount = oImpResponse.PremiumFinanceDetails.PFFrequencyAmount
                        .PFFrequencyKey = oImpResponse.PremiumFinanceDetails.PFFrequencyKey
                        .SourceKey = oImpResponse.PremiumFinanceDetails.SourceKey
                        .ProductKey = oImpResponse.PremiumFinanceDetails.ProductKey
                        .PFSchemeTypeCode = oImpResponse.PremiumFinanceDetails.PFSchemeTypeCode
                        .FinanceFee = oImpResponse.PremiumFinanceDetails.FinanceFee
                        .DayOfWeekOrMonth = oImpResponse.PremiumFinanceDetails.DayOfWeekOrMonth
                        .PaymentMethod = oImpResponse.PremiumFinanceDetails.PaymentMethod
                        .PFFrequencyDesc = oImpResponse.PremiumFinanceDetails.PFFrequencyDesc
                        .SchemePrintType = oImpResponse.PremiumFinanceDetails.SchemePrintType
                        .OriginalAmount = oImpResponse.PremiumFinanceDetails.OriginalAmount
                        .LastInstalment = oImpResponse.PremiumFinanceDetails.LastInstalment
                        .ClaimDebtId = oImpResponse.PremiumFinanceDetails.ClaimDebtId
                        .DateCreated = oImpResponse.PremiumFinanceDetails.DateCreated
                        .DateModified = oImpResponse.PremiumFinanceDetails.DateModified
                        .DateConfirmed = oImpResponse.PremiumFinanceDetails.DateConfirmed
                        .DateReview = oImpResponse.PremiumFinanceDetails.DateReview
                        .IsViaThirdParty = oImpResponse.PremiumFinanceDetails.IsViaThirdParty
                        .IsDepositAsInstalment = oImpResponse.PremiumFinanceDetails.IsDepositAsInstalment
                        .PFRFMnemonic = oImpResponse.PremiumFinanceDetails.PFRFMnemonic
                        .CoverStartDate = oImpResponse.PremiumFinanceDetails.CoverStartDate
                        .ExpiryDate = oImpResponse.PremiumFinanceDetails.ExpiryDate
                        .Terms = oImpResponse.PremiumFinanceDetails.Terms
                        .OriginalRate = oImpResponse.PremiumFinanceDetails.OriginalRate
                        .IsCardHolder = oImpResponse.PremiumFinanceDetails.IsCardHolder
                        .CardHolderName = oImpResponse.PremiumFinanceDetails.CardHolderName
                        .CardHolderAddress1 = oImpResponse.PremiumFinanceDetails.CardHolderAddress1
                        .CardHolderAddress2 = oImpResponse.PremiumFinanceDetails.CardHolderAddress2
                        .CardHolderAddress3 = oImpResponse.PremiumFinanceDetails.CardHolderAddress3
                        .CardHolderAddress4 = oImpResponse.PremiumFinanceDetails.CardHolderAddress4
                        .CardHolderPostCode = oImpResponse.PremiumFinanceDetails.CardHolderPostCode
                        .CardType = oImpResponse.PremiumFinanceDetails.CardType
                        .IsProviderCollectDeposit = oImpResponse.PremiumFinanceDetails.IsProviderCollectDeposit
                        .CardHolderPostCode = oImpResponse.PremiumFinanceDetails.CardHolderPostCode
                        .CardHolderPostCode = oImpResponse.PremiumFinanceDetails.CardHolderPostCode
                        .DateBankDetailsChanged = oImpResponse.PremiumFinanceDetails.DateBankDetailsChanged
                        .IsDDCancelled = oImpResponse.PremiumFinanceDetails.IsDDCancelled
                        .IsCCCancelled = oImpResponse.PremiumFinanceDetails.IsCCCancelled
                        .IsPaperlessDD = oImpResponse.PremiumFinanceDetails.IsPaperlessDD
                        .SchemeType = oImpResponse.PremiumFinanceDetails.SchemeType
                        .SchemeCode = oImpResponse.PremiumFinanceDetails.SchemeCode
                        .PartyBankKey = oImpResponse.PremiumFinanceDetails.PartyBankKey
                        .PFPremiumFinanceCancelReason = oImpResponse.PremiumFinanceDetails.PFPremiumFinanceCancelReason
                        .IsCancelPolicyRun = oImpResponse.PremiumFinanceDetails.IsCancelPolicyRun
                        .IsFinanceNetCommission = oImpResponse.PremiumFinanceDetails.IsFinanceNetCommission
                        .IsDepositOnOtherMediaType = oImpResponse.PremiumFinanceDetails.IsDepositOnOtherMediaType
                        .SettlementAmount = oImpResponse.PremiumFinanceDetails.SettlementAmount
                        .DepostiCCTrackingNumber = oImpResponse.PremiumFinanceDetails.DepostiCCTrackingNumber
                        .Authcode = oImpResponse.PremiumFinanceDetails.DepostiCCTrackingNumber
                        .CanUserEditInstallmentStatusYN = oImpResponse.PremiumFinanceDetails.CanUserEditInstallmentStatusYN
                        .Receipt_Difference_Option = oImpResponse.PremiumFinanceDetails.Receipt_Difference_Option
                    End With
                End If

                'Setting the ImpResponse of PremiumFinanceDetails To Portal Response PremiumFinanceDetails

                'Setting the ImpResponse of BasePremiumFinancePlanTransactionsType To Portal Response 
                If oImpResponse.Transactions IsNot Nothing Then
                    Dim iCntTransaction As Integer = 0
                    Dim iuBoundTransaction As Integer = oImpResponse.Transactions.GetUpperBound(0)
                    Dim ilBoundTransaction As Integer = oImpResponse.Transactions.GetLowerBound(0)
                    If oImpResponse.Transactions.Length > 0 Then
                        ReDim oResponse.Transactions(iuBoundTransaction)
                        For iCntTransaction = ilBoundTransaction To iuBoundTransaction
                            oResponse.Transactions(iCntTransaction) = New BasePremiumFinancePlanTransactionsType
                            With oResponse.Transactions(iCntTransaction)
                                .PFTransactionKey = oImpResponse.Transactions(iCntTransaction).PFTransactionKey
                                .InsuranceRefIndex = oImpResponse.Transactions(iCntTransaction).InsuranceRefIndex
                                .Amount = oImpResponse.Transactions(iCntTransaction).Amount
                                .InsuranceFileKey = oImpResponse.Transactions(iCntTransaction).InsuranceFileKey
                                .TransDetailKey = oImpResponse.Transactions(iCntTransaction).TransDetailKey
                                .DocRef = oImpResponse.Transactions(iCntTransaction).DocRef
                                .AltRef = oImpResponse.Transactions(iCntTransaction).AltRef
                                .EffectiveDate = oImpResponse.Transactions(iCntTransaction).EffectiveDate
                                .TransDate = oImpResponse.Transactions(iCntTransaction).TransDate
                                .MediaType = oImpResponse.Transactions(iCntTransaction).MediaType
                                .Amount = oImpResponse.Transactions(iCntTransaction).Amount
                                .OutstandingAmount = oImpResponse.Transactions(iCntTransaction).OutstandingAmount
                                .MediaRef = oImpResponse.Transactions(iCntTransaction).MediaRef
                                .Accountkey = oImpResponse.Transactions(iCntTransaction).Accountkey
                                .AccountCode = oImpResponse.Transactions(iCntTransaction).AccountCode
                                .Currency = oImpResponse.Transactions(iCntTransaction).Currency
                                .TaxBand = oImpResponse.Transactions(iCntTransaction).TaxBand
                                .TransactionCurrenciesAmount = oImpResponse.Transactions(iCntTransaction).TransactionCurrenciesAmount
                                .TransactionCurrency = oImpResponse.Transactions(iCntTransaction).TransactionCurrency
                                .TransactionCurrencyCode = oImpResponse.Transactions(iCntTransaction).TransactionCurrencyCode
                                .CurrencyDiff = oImpResponse.Transactions(iCntTransaction).CurrencyDiff
                                .SourceID = oImpResponse.Transactions(iCntTransaction).SourceID
                                .PeriodName = oImpResponse.Transactions(iCntTransaction).PeriodName
                                .DocType = oImpResponse.Transactions(iCntTransaction).DocType
                                .DoctypeGroup = oImpResponse.Transactions(iCntTransaction).DoctypeGroup
                                .InsuranceRef = oImpResponse.Transactions(iCntTransaction).InsuranceRef
                                .Spare = oImpResponse.Transactions(iCntTransaction).Spare
                                .DocTypeID = oImpResponse.Transactions(iCntTransaction).DocTypeID
                                .PrimarySettled = oImpResponse.Transactions(iCntTransaction).PrimarySettled
                            End With
                        Next
                    End If
                End If
                'Setting the ImpResponse of BasePremiumFinancePlanTransactionsType To Portal Response [Start]
                If oImpResponse.PFHistory IsNot Nothing Then
                    Dim iuPfHistory As Integer = oImpResponse.PFHistory.GetUpperBound(0)
                    Dim ilPfHistory As Integer = oImpResponse.PFHistory.GetLowerBound(0)
                    Dim iCntPfHistory As Integer = 0

                    If oImpResponse.PFHistory.Length > 0 Then
                        ReDim oResponse.PFHistory(iuPfHistory)
                        For iCntPfHistory = ilPfHistory To iuPfHistory
                            oResponse.PFHistory(iCntPfHistory) = New BasePremiumFinancePlanHistoryType
                            With oResponse.PFHistory(iCntPfHistory)
                                .PFPremiumFinanceKey = oImpResponse.PFHistory(iCntPfHistory).PFPremiumFinanceKey
                                .PFPremiumFinanceVersionKey = oImpResponse.PFHistory(iCntPfHistory).PFPremiumFinanceVersionKey
                                .StartDate = oImpResponse.PFHistory(iCntPfHistory).StartDate
                                .FinanceAmount = oImpResponse.PFHistory(iCntPfHistory).FinanceAmount
                                .TotalCost = oImpResponse.PFHistory(iCntPfHistory).TotalCost
                                .StatusInd = CType(oImpResponse.PFHistory(iCntPfHistory).StatusInd, FinancePlanStatus)
                                .AutoGeneratedPlanRef = oImpResponse.PFHistory(iCntPfHistory).AutoGeneratedPlanRef
                            End With
                        Next
                    End If
                End If

                'Setting the ImpResponse of BasePremiumFinancePlanTransactionsType To Portal Response [End]

                'Setting the ImpResponse of BankHistory e To Portal Response [Start]
                If oImpResponse.PFBankHistory IsNot Nothing Then
                    Dim iuPfBankHistory As Integer = oImpResponse.PFBankHistory.GetUpperBound(0)
                    Dim ilPfBankHistory As Integer = oImpResponse.PFBankHistory.GetLowerBound(0)
                    Dim iCntPfBankHistory As Integer = 0

                    If oImpResponse.PFBankHistory.Length > 0 Then
                        ReDim oResponse.PFBankHistory(iuPfBankHistory)
                        For iCntPfBankHistory = ilPfBankHistory To iuPfBankHistory
                            oResponse.PFBankHistory(iCntPfBankHistory) = New BasePremiumFinancePlanBankHistoryType
                            With oResponse.PFBankHistory(iCntPfBankHistory)
                                .MediatypeValidationCode = oImpResponse.PFBankHistory(iCntPfBankHistory).MediatypeValidationCode
                                .ActionCode = oImpResponse.PFBankHistory(iCntPfBankHistory).ActionCode
                                .BankAccountName = oImpResponse.PFBankHistory(iCntPfBankHistory).BankAccountName
                                .BankSortCode = oImpResponse.PFBankHistory(iCntPfBankHistory).BankSortCode
                                .BankAccountNo = oImpResponse.PFBankHistory(iCntPfBankHistory).BankAccountNo
                                .BankName = oImpResponse.PFBankHistory(iCntPfBankHistory).BankName
                                .BankBranch = oImpResponse.PFBankHistory(iCntPfBankHistory).BankBranch
                                .BankAddress1 = oImpResponse.PFBankHistory(iCntPfBankHistory).BankAddress1
                                .BankAddress2 = oImpResponse.PFBankHistory(iCntPfBankHistory).BankAddress2
                                .BankAddress3 = oImpResponse.PFBankHistory(iCntPfBankHistory).BankAddress3
                                .BankTown = oImpResponse.PFBankHistory(iCntPfBankHistory).BankTown
                                .BankPostCode = oImpResponse.PFBankHistory(iCntPfBankHistory).BankPostCode
                                .BankRegion = oImpResponse.PFBankHistory(iCntPfBankHistory).BankRegion
                                .BankCountry = oImpResponse.PFBankHistory(iCntPfBankHistory).BankCountry
                                .BankAreaCode = oImpResponse.PFBankHistory(iCntPfBankHistory).BankAreaCode
                                .BankPhoneNo = oImpResponse.PFBankHistory(iCntPfBankHistory).BankPhoneNo
                                .BankExtension = oImpResponse.PFBankHistory(iCntPfBankHistory).BankExtension
                                .BankFaxAreaCode = oImpResponse.PFBankHistory(iCntPfBankHistory).BankFaxAreaCode
                                .BankFaxNo = oImpResponse.PFBankHistory(iCntPfBankHistory).BankFaxNo
                                .CCNumber = oImpResponse.PFBankHistory(iCntPfBankHistory).CCNumber
                                .CCExpiry_date = oImpResponse.PFBankHistory(iCntPfBankHistory).CCExpiry_date
                                .CCStartDate = oImpResponse.PFBankHistory(iCntPfBankHistory).CCStartDate
                                .CCIssue = oImpResponse.PFBankHistory(iCntPfBankHistory).CCIssue
                                .CCPin = oImpResponse.PFBankHistory(iCntPfBankHistory).CCPin
                                .CardHolderName = oImpResponse.PFBankHistory(iCntPfBankHistory).CardHolderName
                                .CardHolderAddress1 = oImpResponse.PFBankHistory(iCntPfBankHistory).CardHolderAddress1
                                .CardHolderAddress2 = oImpResponse.PFBankHistory(iCntPfBankHistory).CardHolderAddress2
                                .CardHolderAddress3 = oImpResponse.PFBankHistory(iCntPfBankHistory).CardHolderAddress3
                                .CardHolderAddress4 = oImpResponse.PFBankHistory(iCntPfBankHistory).CardHolderAddress4
                                .CardHolderPostCode = oImpResponse.PFBankHistory(iCntPfBankHistory).CardHolderPostCode
                                .DateModified = oImpResponse.PFBankHistory(iCntPfBankHistory).DateModified
                                .UserName = oImpResponse.PFBankHistory(iCntPfBankHistory).UserName
                                .PaperDD = oImpResponse.PFBankHistory(iCntPfBankHistory).PaperDD
                                .PaymentType = oImpResponse.PFBankHistory(iCntPfBankHistory).PaymentType
                                .AccountType = oImpResponse.PFBankHistory(iCntPfBankHistory).AccountType
                            End With
                        Next
                    End If
                End If

                'Setting the ImpResponse of BankHistory e To Portal Response [End]

                'Setting the ImpResponse of PFInstallmente To Portal Response [Start]
                If oImpResponse.Installements IsNot Nothing Then
                    Dim iuPfInstallements As Integer = oImpResponse.Installements.GetUpperBound(0)
                    Dim ilPfInstallements As Integer = oImpResponse.Installements.GetLowerBound(0)
                    Dim iCntPfInstallements As Integer = 0
                    If oImpResponse.Installements.Length > 0 Then
                        ReDim oResponse.Installements(iuPfInstallements)

                        For iCntPfInstallements = ilPfInstallements To iuPfInstallements
                            oResponse.Installements(iCntPfInstallements) = New BasePremiumFinancePlanInstalmentsType

                            With oResponse.Installements(iCntPfInstallements)
                                .PFInstalmentsKey = oImpResponse.Installements(iCntPfInstallements).PFInstalmentsKey
                                .InstalmentNumber = oImpResponse.Installements(iCntPfInstallements).InstalmentNumber
                                .DueDate = oImpResponse.Installements(iCntPfInstallements).DueDate
                                .Fee = oImpResponse.Installements(iCntPfInstallements).Fee
                                .Amount = oImpResponse.Installements(iCntPfInstallements).Amount
                                .TransactionDescription = oImpResponse.Installements(iCntPfInstallements).TransactionDescription
                                .StatusDescription = oImpResponse.Installements(iCntPfInstallements).StatusDescription
                                .BatchRef = oImpResponse.Installements(iCntPfInstallements).BatchRef
                                .ExportDate = oImpResponse.Installements(iCntPfInstallements).ExportDate
                                .PostedDate = oImpResponse.Installements(iCntPfInstallements).PostedDate
                                .PFTransactionKey = oImpResponse.Installements(iCntPfInstallements).PFTransactionKey
                                .Tax = oImpResponse.Installements(iCntPfInstallements).Tax
                                .Commission = oImpResponse.Installements(iCntPfInstallements).Commission
                                .InstalmentReason = oImpResponse.Installements(iCntPfInstallements).InstalmentReason
                                .InstalmentReasonCode = oImpResponse.Installements(iCntPfInstallements).InstalmentReasonCode
                                .StatusCode = oImpResponse.Installements(iCntPfInstallements).StatusCode
                                .CurrencyDesc = oImpResponse.Installements(iCntPfInstallements).CurrencyDesc
                                If Not oImpResponse.Installements(iCntPfInstallements).History Is Nothing Then
                                    Dim ilBoundhistory As Integer = oImpResponse.Installements(iCntPfInstallements).History.GetLowerBound(0)
                                    Dim iuBoundhistory As Integer = oImpResponse.Installements(iCntPfInstallements).History.GetUpperBound(0)
                                    Dim iCntInshistory As Integer = 0
                                    If oImpResponse.Installements(iCntPfInstallements).History.Length > 0 Then
                                        oResponse.Installements(iCntPfInstallements).History = Nothing
                                        ReDim oResponse.Installements(iCntPfInstallements).History(iuBoundhistory)

                                        For iCntInshistory = ilBoundhistory To iuBoundhistory
                                            oResponse.Installements(iCntPfInstallements).History(iCntInshistory) = New BasePremiumFinancePlanInstalmentsHistoryType

                                            With oResponse.Installements(iCntPfInstallements).History(iCntInshistory)
                                                .PostedDate = oImpResponse.Installements(iCntPfInstallements).History(iCntInshistory).PostedDate
                                                .PFIStatusDescription = oImpResponse.Installements(iCntPfInstallements).History(iCntInshistory).PFIStatusDescription
                                                .PFIResultDescription = oImpResponse.Installements(iCntPfInstallements).History(iCntInshistory).PFIResultDescription
                                                .PFIResultCode = oImpResponse.Installements(iCntPfInstallements).History(iCntInshistory).PFIResultCode
                                            End With

                                        Next
                                    End If
                                End If

                            End With

                        Next
                    End If
                End If
                'Setting the ImpResponse of PFInstallmente To Portal Response [End]

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            Finally
                oImpRequest = Nothing
                oBusiness = Nothing
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(r_oGetHeaderAndSummariesPFPlanByKeyRequestType))
            Return Nothing
        End Try

    End Function
#End Region
#Region "UpdateInstalmentStatus"

    ''' <summary>
    ''' Update status 
    ''' </summary>
    ''' <param name="r_oUpdateInstalmentStatusRequestType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateInstalmentStatus(ByVal r_oUpdateInstalmentStatusRequestType As UpdateInstalmentStatusRequestType) As UpdateInstalmentStatusResponseType Implements IPurePolicyService.UpdateInstalmentStatus
        Dim sUserName As String = r_oUpdateInstalmentStatusRequestType.LoginUserName
        Dim iAgentKey As Integer
        Dim iUserId As Integer

        Dim oResponse As New UpdateInstalmentStatusResponseType
        Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, r_oUpdateInstalmentStatusRequestType.BranchCode)

        ' Implementation structures
        Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateInstalmentStatusRequestType
        Dim oImpResponse As New SAMForInsuranceV2ImplementationTypes.UpdateInstalmentStatusResponseType

        Try
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMUPDIS", iUserId)
            CommonFunctions.CheckSecurityToken(r_oUpdateInstalmentStatusRequestType.WCFSecurityToken)

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = r_oUpdateInstalmentStatusRequestType.BranchCode
            oImpRequest.PFInstalmentKey = r_oUpdateInstalmentStatusRequestType.PFInstalmentKey
            oImpRequest.PFIStatusCode = r_oUpdateInstalmentStatusRequestType.PFIStatusCode

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateInstalmentStatus(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(r_oUpdateInstalmentStatusRequestType))
            Return Nothing
        End Try
        Return oResponse
    End Function
#End Region
    ''' <summary>
    ''' Cancel instalment policy
    ''' </summary>
    ''' <param name="oCancelPremiumFinancePlanRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CancelPremiumFinancePlan(ByVal oCancelPremiumFinancePlanRequest As CancelPremiumFinancePlanRequestType) As CancelPremiumFinancePlanResponseType Implements IPurePolicyService.CancelPremiumFinancePlan


        Dim sUserName As String = oCancelPremiumFinancePlanRequest.LoginUserName
        Dim iAgentKey As Integer
        Dim iUserId As Integer

        CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
        CommonFunctions.CheckAuthority("SAMCANPFPL", iUserId)
        CommonFunctions.CheckSecurityToken(oCancelPremiumFinancePlanRequest.WCFSecurityToken)

        Dim oResponse As New CancelPremiumFinancePlanResponseType
        Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oCancelPremiumFinancePlanRequest.BranchCode)

        Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.CancelPremiumFinancePlanRequestType
        Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.CancelPremiumFinancePlanResponseType = Nothing

        Try

            oImpRequest.BranchCode = oCancelPremiumFinancePlanRequest.BranchCode
            oImpRequest.PFPremiumFinanceKey = oCancelPremiumFinancePlanRequest.PFPremiumFinanceKey
            oImpRequest.PFPremiumFinanceVersionKey = oCancelPremiumFinancePlanRequest.PFPremiumFinanceVersionKey
            oImpRequest.ReasonCode = oCancelPremiumFinancePlanRequest.ReasonCode
            oImpRequest.RequestType = oCancelPremiumFinancePlanRequest.RequestType

            Try
                oImpResponse = oBusiness.CancelPremiumFinancePlan(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.DebitTransdetailKey = oImpResponse.DebitTransdetailKey
                oResponse.Warnings = oImpResponse.Warnings
                oResponse.PFPolicies = New List(Of BaseCancelPremiumFinancePlanResponseTypePolicies)
                If oImpResponse.PFPolicies IsNot Nothing Then
                    For i = 0 To oImpResponse.PFPolicies.Count - 1
                        Dim oBaseCancelPremiumFinancePlanResponseTypePolicies As BaseCancelPremiumFinancePlanResponseTypePolicies = New BaseCancelPremiumFinancePlanResponseTypePolicies
                        oBaseCancelPremiumFinancePlanResponseTypePolicies.CalculatedLapsedDate = oImpResponse.PFPolicies(i).CalculatedLapsedDate
                        oBaseCancelPremiumFinancePlanResponseTypePolicies.InsuranceFileKey = oImpResponse.PFPolicies(i).InsuranceFileKey
                        oResponse.PFPolicies.Add(oBaseCancelPremiumFinancePlanResponseTypePolicies)
                    Next
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oCancelPremiumFinancePlanRequest))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' This method is used for Canceling Premium Finance Policies
    ''' </summary>
    ''' <param name="oCancelPFPoliciesRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CancelPFPolicies(ByVal oCancelPFPoliciesRequest As CancelPFPoliciesRequestType) As CancelPFPoliciesResponseType Implements IPurePolicyService.CancelPFPolicies
        Dim sUserName As String = oCancelPFPoliciesRequest.LoginUserName
        Dim nAgentKey As Integer
        Dim nUserId As Integer

        CommonFunctions.GetIdentity(sUserName, nAgentKey, nUserId)
        CommonFunctions.CheckAuthority("SAMCANPFPO", nUserId)
        CommonFunctions.CheckSecurityToken(oCancelPFPoliciesRequest.WCFSecurityToken)

        Dim oResponse As New CancelPFPoliciesResponseType
        Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oCancelPFPoliciesRequest.BranchCode)

        Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.CancelPFPoliciesRequestType
        Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.CancelPFPoliciesResponseType = Nothing

        Try
            oImpRequest.BranchCode = oCancelPFPoliciesRequest.BranchCode
            oImpRequest.LapsedReasonCode = oCancelPFPoliciesRequest.LapsedReasonCode
            oImpRequest.PFPremiumFinanceKey = oCancelPFPoliciesRequest.PFPremiumFinanceKey
            oImpRequest.PFPremiumFinanceVersionKey = oCancelPFPoliciesRequest.PFPremiumFinanceVersionKey
            oImpRequest.PolicyLapsedDate = oCancelPFPoliciesRequest.PolicyLapsedDate
            oImpRequest.SpoolDoc = oCancelPFPoliciesRequest.SpoolDoc
            oImpRequest.WriteOff = oCancelPFPoliciesRequest.WriteOff

            Try
                oImpResponse = oBusiness.CancelPFPolicies(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oCancelPFPoliciesRequest))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' This method is used for Processing Premium Finance Plan
    ''' </summary>
    ''' <param name="oProcessPfPlanRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessPfPlan(ByVal oProcessPfPlanRequest As ProcessPFPlanRequestType) As ProcessPFPlanResponseType Implements IPurePolicyService.ProcessPfPlan
        Dim sUserName As String = oProcessPfPlanRequest.LoginUserName
        Dim nAgentKey As Integer
        Dim nUserId As Integer

        CommonFunctions.GetIdentity(sUserName, nAgentKey, nUserId)
        CommonFunctions.CheckAuthority("SAMPROPFP", nUserId)
        CommonFunctions.CheckSecurityToken(oProcessPfPlanRequest.WCFSecurityToken)

        Dim oResponse As New ProcessPFPlanResponseType
        Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oProcessPfPlanRequest.BranchCode)

        Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.ProcessPFPlanRequestType
        Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.ProcessPFPlanResponseType

        Try
            oImpRequest.PFPremFinanceKey = oProcessPfPlanRequest.PFPremFinanceKey
            oImpRequest.PFPremFinanceVersion = oProcessPfPlanRequest.PFPremFinanceVersion
            oImpRequest.BranchCode = oProcessPfPlanRequest.BranchCode
            oImpRequest.PartyCode = oProcessPfPlanRequest.PartyCode
            oImpRequest.TransType = CType([Enum].ToObject(GetType(ProcessPFPlanType), oProcessPfPlanRequest.TransType), BaseImplementationTypes.ProcessPFPlanType)
            oImpRequest.SaveOnly = oProcessPfPlanRequest.SaveOnly
            oImpRequest.Type = CType([Enum].ToObject(GetType(InstalmentType), oProcessPfPlanRequest.Type), BaseImplementationTypes.InstalmentType)

            If oProcessPfPlanRequest.PFBankDetails IsNot Nothing Then
                oImpRequest.PFBankDetails = New BaseImplementationTypes.BasePFBankDetails
                oImpRequest.PFBankDetails.BankAccountName = oProcessPfPlanRequest.PFBankDetails.BankAccountName
                oImpRequest.PFBankDetails.BankAccountNo = oProcessPfPlanRequest.PFBankDetails.BankAccountNo

                If oProcessPfPlanRequest.PFBankDetails.BankAddress IsNot Nothing Then
                    oImpRequest.PFBankDetails.BankAddress = New BaseImplementationTypes.BaseAddressType
                    oImpRequest.PFBankDetails.BankAddress.AddressLine1 = oProcessPfPlanRequest.PFBankDetails.BankAddress.AddressLine1
                    oImpRequest.PFBankDetails.BankAddress.AddressLine2 = oProcessPfPlanRequest.PFBankDetails.BankAddress.AddressLine2
                    oImpRequest.PFBankDetails.BankAddress.AddressLine3 = oProcessPfPlanRequest.PFBankDetails.BankAddress.AddressLine3
                    oImpRequest.PFBankDetails.BankAddress.AddressLine4 = oProcessPfPlanRequest.PFBankDetails.BankAddress.AddressLine4
                    oImpRequest.PFBankDetails.BankAddress.AddressTypeCode = CType(oProcessPfPlanRequest.PFBankDetails.BankAddress.AddressTypeCode, BaseImplementationTypes.AddressTypeType)
                    oImpRequest.PFBankDetails.BankAddress.CountryCode = oProcessPfPlanRequest.PFBankDetails.BankAddress.CountryCode
                    oImpRequest.PFBankDetails.BankAddress.PostCode = oProcessPfPlanRequest.PFBankDetails.BankAddress.PostCode
                End If ' End of oProcessPFPlanRequest.PFBankDetails.BankAddress


                oImpRequest.PFBankDetails.BankAreaCode = oProcessPfPlanRequest.PFBankDetails.BankAreaCode
                oImpRequest.PFBankDetails.BankBranch = oProcessPfPlanRequest.PFBankDetails.BankBranch
                oImpRequest.PFBankDetails.BankExtn = oProcessPfPlanRequest.PFBankDetails.BankExtn
                oImpRequest.PFBankDetails.BankFax = oProcessPfPlanRequest.PFBankDetails.BankFax
                oImpRequest.PFBankDetails.BankFaxCode = oProcessPfPlanRequest.PFBankDetails.BankFaxCode
                oImpRequest.PFBankDetails.BankName = oProcessPfPlanRequest.PFBankDetails.BankName
                oImpRequest.PFBankDetails.BankPhone = oProcessPfPlanRequest.PFBankDetails.BankPhone
                oImpRequest.PFBankDetails.BankSortCode = oProcessPfPlanRequest.PFBankDetails.BankSortCode
                oImpRequest.PFBankDetails.PartyBankKey = oProcessPfPlanRequest.PFBankDetails.PartyBankKey
            End If ' End of oProcessPFPlanRequest.PFBankDetails

            If oProcessPfPlanRequest.PFCreditCardDetails IsNot Nothing Then
                oImpRequest.PFCreditCardDetails = New BaseImplementationTypes.BaseCreditCardType
                oImpRequest.PFCreditCardDetails.AccountType = oProcessPfPlanRequest.PFCreditCardDetails.AccountType
                oImpRequest.PFCreditCardDetails.AuthCode = oProcessPfPlanRequest.PFCreditCardDetails.AuthCode

                If oProcessPfPlanRequest.PFCreditCardDetails.CardHolder IsNot Nothing Then
                    oImpRequest.PFCreditCardDetails.CardHolder = New BaseImplementationTypes.BaseCreditCardTypeCardHolder
                    oImpRequest.PFCreditCardDetails.CardHolder.AddressLine1 = oProcessPfPlanRequest.PFCreditCardDetails.CardHolder.AddressLine1
                    oImpRequest.PFCreditCardDetails.CardHolder.AddressLine2 = oProcessPfPlanRequest.PFCreditCardDetails.CardHolder.AddressLine2
                    oImpRequest.PFCreditCardDetails.CardHolder.AddressLine3 = oProcessPfPlanRequest.PFCreditCardDetails.CardHolder.AddressLine3
                    oImpRequest.PFCreditCardDetails.CardHolder.AddressLine4 = oProcessPfPlanRequest.PFCreditCardDetails.CardHolder.AddressLine4
                    oImpRequest.PFCreditCardDetails.CardHolder.CountryCode = oProcessPfPlanRequest.PFCreditCardDetails.CardHolder.CountryCode
                    oImpRequest.PFCreditCardDetails.CardHolder.Name = oProcessPfPlanRequest.PFCreditCardDetails.CardHolder.Name
                    oImpRequest.PFCreditCardDetails.CardHolder.PostCode = oProcessPfPlanRequest.PFCreditCardDetails.CardHolder.PostCode
                End If ' End of oProcessPFPlanRequest.PFCreditCardDetails.CardHolder


                oImpRequest.PFCreditCardDetails.CashListItemBankCode = oProcessPfPlanRequest.PFCreditCardDetails.CashListItemBankCode
                oImpRequest.PFCreditCardDetails.CustomerPresent = oProcessPfPlanRequest.PFCreditCardDetails.CustomerPresent
                oImpRequest.PFCreditCardDetails.ExpiryDate = oProcessPfPlanRequest.PFCreditCardDetails.ExpiryDate
                oImpRequest.PFCreditCardDetails.IsRegisteredCardHolder = oProcessPfPlanRequest.PFCreditCardDetails.IsRegisteredCardHolder
                oImpRequest.PFCreditCardDetails.Issue = oProcessPfPlanRequest.PFCreditCardDetails.Issue
                oImpRequest.PFCreditCardDetails.ManualAuthCode = oProcessPfPlanRequest.PFCreditCardDetails.ManualAuthCode
                oImpRequest.PFCreditCardDetails.NameOnCreditCard = oProcessPfPlanRequest.PFCreditCardDetails.NameOnCreditCard
                oImpRequest.PFCreditCardDetails.Number = oProcessPfPlanRequest.PFCreditCardDetails.Number
                oImpRequest.PFCreditCardDetails.PartyBankKey = oProcessPfPlanRequest.PFCreditCardDetails.PartyBankKey
                oImpRequest.PFCreditCardDetails.Pin = oProcessPfPlanRequest.PFCreditCardDetails.Pin
                oImpRequest.PFCreditCardDetails.StartDate = oProcessPfPlanRequest.PFCreditCardDetails.StartDate
                oImpRequest.PFCreditCardDetails.TrackingNumber = oProcessPfPlanRequest.PFCreditCardDetails.TrackingNumber
                oImpRequest.PFCreditCardDetails.TransactionCode = oProcessPfPlanRequest.PFCreditCardDetails.TransactionCode
                oImpRequest.PFCreditCardDetails.TransactionSlipNumber = oProcessPfPlanRequest.PFCreditCardDetails.TransactionSlipNumber
                oImpRequest.PFCreditCardDetails.TypeCode = oProcessPfPlanRequest.PFCreditCardDetails.TypeCode
                oImpRequest.PFCreditCardDetails.VIAPaymentHub = oProcessPfPlanRequest.PFCreditCardDetails.VIAPaymentHub
            End If ' End of oProcessPFPlanRequest.PFCreditCardDetail


            If oProcessPfPlanRequest.PFQuote IsNot Nothing Then
                oImpRequest.PFQuote = New BaseImplementationTypes.BasePremiumFinanceDetails


                If oProcessPfPlanRequest.PFQuote.PFBankDetails IsNot Nothing Then
                    oImpRequest.PFQuote.PFBankDetails = New BaseImplementationTypes.BasePFBankDetails
                    oImpRequest.PFQuote.PFBankDetails.BankAccountName = oProcessPfPlanRequest.PFQuote.PFBankDetails.BankAccountName
                    oImpRequest.PFQuote.PFBankDetails.BankAccountNo = oProcessPfPlanRequest.PFQuote.PFBankDetails.BankAccountNo

                    If oImpRequest.PFQuote.PFBankDetails.BankAddress IsNot Nothing Then
                        oImpRequest.PFQuote.PFBankDetails.BankAddress = New BaseImplementationTypes.BaseAddressType
                        oImpRequest.PFQuote.PFBankDetails.BankAddress.AddressLine1 = oProcessPfPlanRequest.PFQuote.PFBankDetails.BankAddress.AddressLine1
                        oImpRequest.PFQuote.PFBankDetails.BankAddress.AddressLine2 = oProcessPfPlanRequest.PFQuote.PFBankDetails.BankAddress.AddressLine2
                        oImpRequest.PFQuote.PFBankDetails.BankAddress.AddressLine3 = oProcessPfPlanRequest.PFQuote.PFBankDetails.BankAddress.AddressLine3
                        oImpRequest.PFQuote.PFBankDetails.BankAddress.AddressLine4 = oProcessPfPlanRequest.PFQuote.PFBankDetails.BankAddress.AddressLine4
                        oImpRequest.PFQuote.PFBankDetails.BankAddress.AddressTypeCode = CType(oProcessPfPlanRequest.PFQuote.PFBankDetails.BankAddress.AddressTypeCode, BaseImplementationTypes.AddressTypeType)
                        oImpRequest.PFQuote.PFBankDetails.BankAddress.CountryCode = oProcessPfPlanRequest.PFQuote.PFBankDetails.BankAddress.CountryCode
                        oImpRequest.PFQuote.PFBankDetails.BankAddress.PostCode = oProcessPfPlanRequest.PFQuote.PFBankDetails.BankAddress.PostCode
                    End If ' End of oImpRequest.PFQuote.PFBankDetails.BankAddress


                    oImpRequest.PFQuote.PFBankDetails.BankAreaCode = oProcessPfPlanRequest.PFQuote.PFBankDetails.BankAreaCode
                    oImpRequest.PFQuote.PFBankDetails.BankBranch = oProcessPfPlanRequest.PFQuote.PFBankDetails.BankBranch
                    oImpRequest.PFQuote.PFBankDetails.BankExtn = oProcessPfPlanRequest.PFQuote.PFBankDetails.BankExtn
                    oImpRequest.PFQuote.PFBankDetails.BankFax = oProcessPfPlanRequest.PFQuote.PFBankDetails.BankFax
                    oImpRequest.PFQuote.PFBankDetails.BankFaxCode = oProcessPfPlanRequest.PFQuote.PFBankDetails.BankFaxCode
                    oImpRequest.PFQuote.PFBankDetails.BankName = oProcessPfPlanRequest.PFQuote.PFBankDetails.BankName
                    oImpRequest.PFQuote.PFBankDetails.BankPhone = oProcessPfPlanRequest.PFQuote.PFBankDetails.BankPhone
                    oImpRequest.PFQuote.PFBankDetails.BankSortCode = oProcessPfPlanRequest.PFQuote.PFBankDetails.BankSortCode
                    oImpRequest.PFQuote.PFBankDetails.PartyBankKey = oProcessPfPlanRequest.PFQuote.PFBankDetails.PartyBankKey


                End If ' End of oProcessPFPlanRequest.PFQuote.PFBankDetails


                oImpRequest.PFQuote.PFPremiumFinanceKey = oProcessPfPlanRequest.PFQuote.PFPremiumFinanceKey
                oImpRequest.PFQuote.PFPremiumFinanceVersionKey = oProcessPfPlanRequest.PFQuote.PFPremiumFinanceVersionKey
                oImpRequest.PFQuote.PFSchemeName = oProcessPfPlanRequest.PFQuote.PFSchemeName
                oImpRequest.PFQuote.QuoteDate = oProcessPfPlanRequest.PFQuote.QuoteDate
                oImpRequest.PFQuote.StartDate = oProcessPfPlanRequest.PFQuote.StartDate
                oImpRequest.PFQuote.EndDate = oProcessPfPlanRequest.PFQuote.EndDate
                oImpRequest.PFQuote.PreferredDate = oProcessPfPlanRequest.PFQuote.PreferredDate
                oImpRequest.PFQuote.ProductClass = oProcessPfPlanRequest.PFQuote.ProductClass
                oImpRequest.PFQuote.TransType = oProcessPfPlanRequest.PFQuote.TransType
                oImpRequest.PFQuote.FinanceAmount = oProcessPfPlanRequest.PFQuote.FinanceAmount
                oImpRequest.PFQuote.APR = oProcessPfPlanRequest.PFQuote.APR
                oImpRequest.PFQuote.InterestRate = oProcessPfPlanRequest.PFQuote.InterestRate
                oImpRequest.PFQuote.DaysDelay = oProcessPfPlanRequest.PFQuote.DaysDelay
                oImpRequest.PFQuote.NoOfInstallments = oProcessPfPlanRequest.PFQuote.NoOfInstallments
                oImpRequest.PFQuote.FirstInstallment = oProcessPfPlanRequest.PFQuote.FirstInstallment
                oImpRequest.PFQuote.OtherInstallments = oProcessPfPlanRequest.PFQuote.OtherInstallments
                oImpRequest.PFQuote.CostOfProtection = oProcessPfPlanRequest.PFQuote.CostOfProtection
                oImpRequest.PFQuote.Deposit = oProcessPfPlanRequest.PFQuote.Deposit
                oImpRequest.PFQuote.NetAmount = oProcessPfPlanRequest.PFQuote.NetAmount
                oImpRequest.PFQuote.TotalCost = oProcessPfPlanRequest.PFQuote.TotalCost
                oImpRequest.PFQuote.InterestCost = oProcessPfPlanRequest.PFQuote.InterestCost
                oImpRequest.PFQuote.MinFinanceChrge = oProcessPfPlanRequest.PFQuote.MinFinanceChrge
                oImpRequest.PFQuote.PaymentProtection = oProcessPfPlanRequest.PFQuote.PaymentProtection
                oImpRequest.PFQuote.OverrideInterestRate = oProcessPfPlanRequest.PFQuote.OverrideInterestRate
                oImpRequest.PFQuote.OverrideCommission = oProcessPfPlanRequest.PFQuote.OverrideCommission
                oImpRequest.PFQuote.ClientName = oProcessPfPlanRequest.PFQuote.ClientName
                oImpRequest.PFQuote.ClientAddress1 = oProcessPfPlanRequest.PFQuote.ClientAddress1
                oImpRequest.PFQuote.ClientAddress2 = oProcessPfPlanRequest.PFQuote.ClientAddress2
                oImpRequest.PFQuote.ClientAddress3 = oProcessPfPlanRequest.PFQuote.ClientAddress3
                oImpRequest.PFQuote.ClientAddress4 = oProcessPfPlanRequest.PFQuote.ClientAddress4
                oImpRequest.PFQuote.ClientTown = oProcessPfPlanRequest.PFQuote.ClientTown
                oImpRequest.PFQuote.ClientPcode = oProcessPfPlanRequest.PFQuote.ClientPcode
                oImpRequest.PFQuote.ClientCountry = oProcessPfPlanRequest.PFQuote.ClientCountry
                oImpRequest.PFQuote.ClientAreaCode = oProcessPfPlanRequest.PFQuote.ClientAreaCode
                oImpRequest.PFQuote.ClientPhoneNo = oProcessPfPlanRequest.PFQuote.ClientPhoneNo
                oImpRequest.PFQuote.ClientExtension = oProcessPfPlanRequest.PFQuote.ClientExtension
                oImpRequest.PFQuote.ClientFaxAreaCode = oProcessPfPlanRequest.PFQuote.ClientFaxAreaCode
                oImpRequest.PFQuote.ClientFaxNo = oProcessPfPlanRequest.PFQuote.ClientFaxNo
                oImpRequest.PFQuote.BankName = oProcessPfPlanRequest.PFQuote.BankName
                oImpRequest.PFQuote.BankSortCode = oProcessPfPlanRequest.PFQuote.BankSortCode
                oImpRequest.PFQuote.BankAccountNo = oProcessPfPlanRequest.PFQuote.BankAccountNo
                oImpRequest.PFQuote.BankAccountName = oProcessPfPlanRequest.PFQuote.BankAccountName
                oImpRequest.PFQuote.BankBranch = oProcessPfPlanRequest.PFQuote.BankBranch
                oImpRequest.PFQuote.BankAddress1 = oProcessPfPlanRequest.PFQuote.BankAddress1
                oImpRequest.PFQuote.BankAddress2 = oProcessPfPlanRequest.PFQuote.BankAddress2
                oImpRequest.PFQuote.BankAddress3 = oProcessPfPlanRequest.PFQuote.BankAddress3
                oImpRequest.PFQuote.BankAddress4 = oProcessPfPlanRequest.PFQuote.BankAddress4
                oImpRequest.PFQuote.BankTown = oProcessPfPlanRequest.PFQuote.BankTown
                oImpRequest.PFQuote.BankRegion = oProcessPfPlanRequest.PFQuote.BankRegion
                oImpRequest.PFQuote.BankPostCode = oProcessPfPlanRequest.PFQuote.BankPostCode
                oImpRequest.PFQuote.BankCountry = oProcessPfPlanRequest.PFQuote.BankCountry
                oImpRequest.PFQuote.BankAreaCode = oProcessPfPlanRequest.PFQuote.BankAreaCode
                oImpRequest.PFQuote.BankPhoneNo = oProcessPfPlanRequest.PFQuote.BankPhoneNo
                oImpRequest.PFQuote.BankExtension = oProcessPfPlanRequest.PFQuote.BankExtension
                oImpRequest.PFQuote.BankFaxAreaCode = oProcessPfPlanRequest.PFQuote.BankFaxAreaCode
                oImpRequest.PFQuote.BankFaxNo = oProcessPfPlanRequest.PFQuote.BankFaxNo
                oImpRequest.PFQuote.StatusInd = CType([Enum].ToObject(GetType(FinancePlanStatus), oProcessPfPlanRequest.PFQuote.StatusInd), BaseImplementationTypes.FinancePlanStatus)
                oImpRequest.PFQuote.ClientCode = oProcessPfPlanRequest.PFQuote.ClientCode
                oImpRequest.PFQuote.AutoGeneratedPlanRef = oProcessPfPlanRequest.PFQuote.AutoGeneratedPlanRef
                oImpRequest.PFQuote.FinanceCollatedPlanRef = oProcessPfPlanRequest.PFQuote.FinanceCollatedPlanRef
                oImpRequest.PFQuote.InterestFree = oProcessPfPlanRequest.PFQuote.InterestFree
                oImpRequest.PFQuote.IsQuote = oProcessPfPlanRequest.PFQuote.IsQuote
                oImpRequest.PFQuote.PlanTransactionKey = oProcessPfPlanRequest.PFQuote.PlanTransactionKey
                oImpRequest.PFQuote.InsuranceFileKey = oProcessPfPlanRequest.PFQuote.InsuranceFileKey
                oImpRequest.PFQuote.PFRFKEY = oProcessPfPlanRequest.PFQuote.PFRFKEY
                oImpRequest.PFQuote.PFFrequencyCode = oProcessPfPlanRequest.PFQuote.PFFrequencyCode
                oImpRequest.PFQuote.BankCountryKey = oProcessPfPlanRequest.PFQuote.BankCountryKey
                oImpRequest.PFQuote.ClientCountryKey = oProcessPfPlanRequest.PFQuote.ClientCountryKey
                oImpRequest.PFQuote.FirstInstalmentDate = oProcessPfPlanRequest.PFQuote.FirstInstalmentDate
                oImpRequest.PFQuote.NextInstalmentDate = oProcessPfPlanRequest.PFQuote.NextInstalmentDate
                oImpRequest.PFQuote.LastInstalmentDate = oProcessPfPlanRequest.PFQuote.LastInstalmentDate
                oImpRequest.PFQuote.TaxCost = oProcessPfPlanRequest.PFQuote.TaxCost
                oImpRequest.PFQuote.MediaTypeCode = oProcessPfPlanRequest.PFQuote.MediaTypeCode
                oImpRequest.PFQuote.CCNumber = oProcessPfPlanRequest.PFQuote.CCNumber
                oImpRequest.PFQuote.CCExpiryDate = oProcessPfPlanRequest.PFQuote.CCExpiryDate
                oImpRequest.PFQuote.CCStartDate = oProcessPfPlanRequest.PFQuote.CCStartDate
                oImpRequest.PFQuote.CCIssue = oProcessPfPlanRequest.PFQuote.CCIssue
                oImpRequest.PFQuote.CCPin = oProcessPfPlanRequest.PFQuote.CCPin
                oImpRequest.PFQuote.MediaTypeValidationCode = oProcessPfPlanRequest.PFQuote.MediaTypeValidationCode
                oImpRequest.PFQuote.bank_name_mandatory = oProcessPfPlanRequest.PFQuote.bank_name_mandatory
                oImpRequest.PFQuote.IsBankAddressMandatory = oProcessPfPlanRequest.PFQuote.IsBankAddressMandatory
                oImpRequest.PFQuote.IsBranchNameMandatory = oProcessPfPlanRequest.PFQuote.IsBranchNameMandatory
                oImpRequest.PFQuote.IsBranchCodeMandatory = oProcessPfPlanRequest.PFQuote.IsBranchCodeMandatory
                oImpRequest.PFQuote.CommissionTransdetailKey = oProcessPfPlanRequest.PFQuote.CommissionTransdetailKey
                oImpRequest.PFQuote.PFFrequencyPeriod = oProcessPfPlanRequest.PFQuote.PFFrequencyPeriod
                oImpRequest.PFQuote.PFFrequencyAmount = oProcessPfPlanRequest.PFQuote.PFFrequencyAmount
                oImpRequest.PFQuote.PFFrequencyKey = oProcessPfPlanRequest.PFQuote.PFFrequencyKey
                oImpRequest.PFQuote.SourceKey = oProcessPfPlanRequest.PFQuote.SourceKey
                oImpRequest.PFQuote.ProductKey = oProcessPfPlanRequest.PFQuote.ProductKey
                oImpRequest.PFQuote.PFSchemeTypeCode = oProcessPfPlanRequest.PFQuote.PFSchemeTypeCode
                oImpRequest.PFQuote.FinanceFee = oProcessPfPlanRequest.PFQuote.FinanceFee
                oImpRequest.PFQuote.DayOfWeekOrMonth = oProcessPfPlanRequest.PFQuote.DayOfWeekOrMonth
                oImpRequest.PFQuote.PaymentMethod = oProcessPfPlanRequest.PFQuote.PaymentMethod
                oImpRequest.PFQuote.PFFrequencyDesc = oProcessPfPlanRequest.PFQuote.PFFrequencyDesc
                oImpRequest.PFQuote.SchemePrintType = oProcessPfPlanRequest.PFQuote.SchemePrintType
                oImpRequest.PFQuote.OriginalAmount = oProcessPfPlanRequest.PFQuote.OriginalAmount
                oImpRequest.PFQuote.LastInstalment = oProcessPfPlanRequest.PFQuote.LastInstalment
                oImpRequest.PFQuote.ClaimDebtId = oProcessPfPlanRequest.PFQuote.ClaimDebtId
                oImpRequest.PFQuote.DateCreated = oProcessPfPlanRequest.PFQuote.DateCreated
                oImpRequest.PFQuote.DateModified = oProcessPfPlanRequest.PFQuote.DateModified
                oImpRequest.PFQuote.DateConfirmed = oProcessPfPlanRequest.PFQuote.DateConfirmed
                oImpRequest.PFQuote.DateReview = oProcessPfPlanRequest.PFQuote.DateReview
                oImpRequest.PFQuote.IsViaThirdParty = oProcessPfPlanRequest.PFQuote.IsViaThirdParty
                oImpRequest.PFQuote.IsDepositAsInstalment = oProcessPfPlanRequest.PFQuote.IsDepositAsInstalment
                oImpRequest.PFQuote.PFRFMnemonic = oProcessPfPlanRequest.PFQuote.PFRFMnemonic
                oImpRequest.PFQuote.CoverStartDate = oProcessPfPlanRequest.PFQuote.CoverStartDate
                oImpRequest.PFQuote.ExpiryDate = oProcessPfPlanRequest.PFQuote.ExpiryDate
                oImpRequest.PFQuote.Terms = oProcessPfPlanRequest.PFQuote.Terms
                oImpRequest.PFQuote.OriginalRate = oProcessPfPlanRequest.PFQuote.OriginalRate
                oImpRequest.PFQuote.IsCardHolder = oProcessPfPlanRequest.PFQuote.IsCardHolder
                oImpRequest.PFQuote.CardHolderName = oProcessPfPlanRequest.PFQuote.CardHolderName
                oImpRequest.PFQuote.CardHolderAddress1 = oProcessPfPlanRequest.PFQuote.CardHolderAddress1
                oImpRequest.PFQuote.CardHolderAddress2 = oProcessPfPlanRequest.PFQuote.CardHolderAddress2
                oImpRequest.PFQuote.CardHolderAddress3 = oProcessPfPlanRequest.PFQuote.CardHolderAddress3
                oImpRequest.PFQuote.CardHolderAddress4 = oProcessPfPlanRequest.PFQuote.CardHolderAddress4
                oImpRequest.PFQuote.CardHolderPostCode = oProcessPfPlanRequest.PFQuote.CardHolderPostCode
                oImpRequest.PFQuote.CardType = oProcessPfPlanRequest.PFQuote.CardType
                oImpRequest.PFQuote.IsProviderCollectDeposit = oProcessPfPlanRequest.PFQuote.IsProviderCollectDeposit
                oImpRequest.PFQuote.DateBankDetailsChanged = oProcessPfPlanRequest.PFQuote.DateBankDetailsChanged
                oImpRequest.PFQuote.IsDDCancelled = oProcessPfPlanRequest.PFQuote.IsDDCancelled
                oImpRequest.PFQuote.IsCCCancelled = oProcessPfPlanRequest.PFQuote.IsCCCancelled
                oImpRequest.PFQuote.IsPaperlessDD = oProcessPfPlanRequest.PFQuote.IsPaperlessDD
                oImpRequest.PFQuote.SchemeType = oProcessPfPlanRequest.PFQuote.SchemeType
                oImpRequest.PFQuote.SchemeCode = oProcessPfPlanRequest.PFQuote.SchemeCode
                oImpRequest.PFQuote.PartyBankKey = oProcessPfPlanRequest.PFQuote.PartyBankKey
                oImpRequest.PFQuote.PFPremiumFinanceCancelReason = oProcessPfPlanRequest.PFQuote.PFPremiumFinanceCancelReason
                oImpRequest.PFQuote.IsCancelPolicyRun = oProcessPfPlanRequest.PFQuote.IsCancelPolicyRun
                oImpRequest.PFQuote.IsFinanceNetCommission = oProcessPfPlanRequest.PFQuote.IsFinanceNetCommission
                oImpRequest.PFQuote.IsDepositOnOtherMediaType = oProcessPfPlanRequest.PFQuote.IsDepositOnOtherMediaType
                oImpRequest.PFQuote.SelectedSchemeNo = oProcessPfPlanRequest.PFQuote.SelectedSchemeNo
                oImpRequest.PFQuote.SelectedSchemeVersion = oProcessPfPlanRequest.PFQuote.SelectedSchemeVersion
                oImpRequest.PFQuote.ProcessPFMode = oProcessPfPlanRequest.PFQuote.ProcessPFMode
                oImpRequest.PFQuote.DepositCCTrackingNumber = oProcessPfPlanRequest.PFQuote.DepositCCTrackingNumber
            End If ' End of oProcessPFPlanRequest.PFQuote.PFBankDetails


            Dim oPFTransactions(0) As BaseImplementationTypes.BaseProcessPFPlanRequestTrans

            If oProcessPfPlanRequest.PFTransaction IsNot Nothing AndAlso oProcessPfPlanRequest.PFTransaction.Count > 0 Then
                For nTransactionCount As Integer = 0 To oProcessPfPlanRequest.PFTransaction.Count - 1
                    ReDim Preserve oPFTransactions(nTransactionCount)
                    oPFTransactions(nTransactionCount) = New BaseImplementationTypes.BaseProcessPFPlanRequestTrans
                    oPFTransactions(nTransactionCount).DocumentTypeId = oProcessPfPlanRequest.PFTransaction(nTransactionCount).DocumentTypeId
                    oPFTransactions(nTransactionCount).InsuranceFileKey = oProcessPfPlanRequest.PFTransaction(nTransactionCount).InsuranceFileKey
                    oPFTransactions(nTransactionCount).OutstandingAmount = oProcessPfPlanRequest.PFTransaction(nTransactionCount).OutstandingAmount
                    oPFTransactions(nTransactionCount).PolicyRef = oProcessPfPlanRequest.PFTransaction(nTransactionCount).PolicyRef
                    oPFTransactions(nTransactionCount).Spare = oProcessPfPlanRequest.PFTransaction(nTransactionCount).Spare
                    oPFTransactions(nTransactionCount).TransdetailKey = oProcessPfPlanRequest.PFTransaction(nTransactionCount).TransdetailKey
                    oPFTransactions(nTransactionCount).AccountId = oProcessPfPlanRequest.PFTransaction(nTransactionCount).AccountId

                Next

                oImpRequest.PFTransaction = oPFTransactions
            End If



            Try
                oImpResponse = oBusiness.ProcessPFPlan(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.DepositTransdetailKey = oImpResponse.DepositTransdetailKey

                Dim oErrors() As SFI.SAMForInsuranceV2.WCF.SAMError = Nothing
                Dim bHasErrors As Boolean = False
                If oImpResponse.Errors IsNot Nothing Then
                    bHasErrors = True

                    ReDim oErrors(oImpResponse.Errors.GetUpperBound(0))
                    For cntEIndex As Integer = 0 To oResponse.Errors.Count - 1
                        oErrors(cntEIndex) = CommonFunctions.ConvertToSFIV2SAMError(oImpResponse.Errors(cntEIndex))
                        oResponse.Errors.Add(oErrors(cntEIndex))
                    Next
                    oErrors = Nothing
                End If

                oResponse.HandlingInstanceID = oImpResponse.HandlingInstanceID
                oResponse.PFPremFinanceKey = oImpResponse.PFPremFinanceKey
                oResponse.PFPremFinanceVersion = oImpResponse.PFPremFinanceVersion
                oResponse.Warnings = oImpResponse.Warnings


            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oProcessPfPlanRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' Get plan info
    ''' </summary>
    ''' <param name="oGetFinancePlanInformationRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFinancePlanInformation(ByVal oGetFinancePlanInformationRequest As GetFinancePlanInformationRequestType) As GetFinancePlanInformationResponseType Implements IPurePolicyService.GetFinancePlanInformation

        Try
            Dim sUserName As String = oGetFinancePlanInformationRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMFP", iUserId)
            CommonFunctions.CheckSecurityToken(oGetFinancePlanInformationRequest.WCFSecurityToken)

            Dim oResponse As New GetFinancePlanInformationResponseType
            Dim oBusiness As New CoreSAMBusiness(sUserName, oGetFinancePlanInformationRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetFinancePlanInformationRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetFinancePlanInformationResponseType = Nothing

            oImpRequest.BranchCode = oGetFinancePlanInformationRequest.BranchCode
            oImpRequest.InsuranceFileKey = oGetFinancePlanInformationRequest.InsuranceFileKey

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetFinancePlanInformation(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                With oResponse
                    .PremiumFinanceKey = oImpResponse.PremiumFinanceKey
                    .PremiumFinanceVersion = oImpResponse.PremiumFinanceVersion
                    .ProductCode = oImpResponse.ProductCode
                    .OriginalInsuranceFileKey = oImpResponse.OriginalInsuranceFileKey
                End With

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetFinancePlanInformationRequest))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oUpdateMarketplacePolicyStatusRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateMarketplacePolicyStatus(ByVal oUpdateMarketplacePolicyStatusRequest As UpdateMarketplacePolicyStatusRequestType) As UpdateMarketplacePolicyStatusResponseType Implements IPurePolicyService.UpdateMarketplacePolicyStatus

        Try

            Dim sUserName As String = oUpdateMarketplacePolicyStatusRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckSecurityToken(oUpdateMarketplacePolicyStatusRequest.WCFSecurityToken)
            Dim oResponse As New UpdateMarketplacePolicyStatusResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oUpdateMarketplacePolicyStatusRequest.BranchCode)
            Dim iCount As Integer = 0
            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateMarketplacePolicyStatusRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateMarketplacePolicyStatusResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oUpdateMarketplacePolicyStatusRequest.BranchCode
            oImpRequest.InsuranceFileKey = oUpdateMarketplacePolicyStatusRequest.InsuranceFileKey
            oImpRequest.IsMarketPlacePolicy = oUpdateMarketplacePolicyStatusRequest.IsMarketPlacePolicy

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateMarketplacePolicyStatus(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdateMarketplacePolicyStatusRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateMarketplacePolicyStatusRequest))
            Return Nothing
        End Try

    End Function

    Public Function FindPoliciesByRiskindex(ByVal oFindPoliciesByRiskindexRequest As FindPoliciesByRiskIndexRequestType) As FindPoliciesByRiskIndexResponseType Implements IPurePolicyService.FindPoliciesByRiskindex

        Try

            Dim sUserName As String = oFindPoliciesByRiskindexRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            'CommonFunctions.CheckAuthority("SAMAQuot", iUserId)
            CommonFunctions.CheckSecurityToken(oFindPoliciesByRiskindexRequest.WCFSecurityToken)
            Dim oResponse As New FindPoliciesByRiskIndexResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oFindPoliciesByRiskindexRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.FindPoliciesByRiskIndexRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.FindPoliciesByRiskIndexResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oFindPoliciesByRiskindexRequest.BranchCode
            oImpRequest.PartyKey = oFindPoliciesByRiskindexRequest.PartyKey
            oImpRequest.RiskIndex = oFindPoliciesByRiskindexRequest.RiskIndex
            oImpRequest.WCFSecurityToken = If(oFindPoliciesByRiskindexRequest.WCFSecurityToken.Length > 0, oFindPoliciesByRiskindexRequest.WCFSecurityToken, "WCFSecurityToken")
            Try

                ' Call the implementation method
                oImpResponse = oBusiness.FindPoliciesByRiskIndex(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                If oImpResponse.InsuranceFolderKeys IsNot Nothing AndAlso Not String.IsNullOrEmpty(oImpResponse.InsuranceFolderKeys) Then
                    oResponse.InsuranceFolderKeys = oImpResponse.InsuranceFolderKeys
                    If oImpResponse.InsuranceFileKeys IsNot Nothing AndAlso Not String.IsNullOrEmpty(oImpResponse.InsuranceFileKeys) Then
                        oResponse.InsuranceFileKeys = oImpResponse.InsuranceFileKeys
                        If oImpResponse.RiskKeys IsNot Nothing AndAlso Not String.IsNullOrEmpty(oImpResponse.RiskKeys) Then
                            oResponse.RiskKeys = oImpResponse.RiskKeys
                        End If
                    End If
                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex)
            Return Nothing
        End Try

    End Function

#Region "Cancel Quote"
    ''' <summary>
    ''' To cancel (delete) the MTA Quote
    ''' </summary>
    ''' <param name="oCancelQuoteRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CancelQuote(ByVal oCancelQuoteRequest As CancelQuoteRequestType) As CancelQuoteResponseType Implements IPurePolicyService.CancelQuote

        Try
            Dim sUserName As String = oCancelQuoteRequest.LoginUserName
            Dim nAgentKey As Integer = 0
            Dim nUserId As Integer = 0
            CommonFunctions.GetIdentity(sUserName, nAgentKey, nUserId)
            CommonFunctions.CheckAuthority("SAMAQuot", nUserId)
            CommonFunctions.CheckSecurityToken(oCancelQuoteRequest.WCFSecurityToken)
            Dim oResponse As New CancelQuoteResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oCancelQuoteRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.CancelQuoteRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.CancelQuoteResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.LoginUserName = oCancelQuoteRequest.LoginUserName
            oImpRequest.BranchCode = oCancelQuoteRequest.BranchCode
            oImpRequest.InsuranceFileKey = oCancelQuoteRequest.InsuranceFileKey
            oImpRequest.TimeStamp = oCancelQuoteRequest.TimeStamp

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.CancelQuote(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oCancelQuoteRequest))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oCancelQuoteRequest))
        End Try

    End Function

#End Region

#Region "CheckPendingOOSVersions"
    ''' <summary>
    ''' This method will return boolean flag depending on pending OOS version on policy
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckPendingOOSVersions(ByVal oRequest As CheckPendingOOSVersionsRequestType) As CheckPendingOOSVersionsResponseType Implements IPurePolicyService.CheckPendingOOSVersions

        Try
            Dim sUserName As String = oRequest.LoginUserName
            Dim nAgentKey As Integer = 0
            Dim nUserId As Integer = 0
            CommonFunctions.GetIdentity(sUserName, nAgentKey, nUserId)
            CommonFunctions.CheckSecurityToken(oRequest.WCFSecurityToken)
            Dim oResponse As New CheckPendingOOSVersionsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.CheckPendingOOSVersionsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.CheckPendingOOSVersionsResponseType = Nothing

            oImpRequest.BranchCode = oRequest.BranchCode
            oImpRequest.InsuranceFileKey = oRequest.InsuranceFileKey
            oImpRequest.InsuranceFolderKey = oRequest.InsuranceFolderKey
            oImpRequest.LoginUserName = oRequest.LoginUserName

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.CheckPendingOOSVersions(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex)
            End Try

            ' Retrieve the values from the implementation response structure
            If oImpResponse.ResultData IsNot Nothing Then
                ' Retrieve the values from the implementation response structure
                oResponse.ResultData = DataTabletoList_CheckPendingOOSVersions(oImpResponse.ResultData)
            End If
            Return oResponse


            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex)
            Return Nothing
        End Try

    End Function
#End Region

    ''' <summary>
    ''' To find latest policy versions
    ''' </summary>
    ''' <param name="oFindPolicyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindLatestPolicyVersions(ByVal oFindPolicyRequest As FindLatestPolicyVersionsRequestType) As FindLatestPolicyVersionsResponseType Implements IPurePolicyService.FindLatestPolicyVersions
        Try

            Dim sUserName As String = oFindPolicyRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMFindPol", iUserId)
            CommonFunctions.CheckSecurityToken(oFindPolicyRequest.WCFSecurityToken)

            Dim oResponse As New FindLatestPolicyVersionsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oFindPolicyRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.FindLatestPolicyVersionsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.FindLatestPolicyVersionsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oFindPolicyRequest.BranchCode
            oImpRequest.RecordTypeSpecified = oFindPolicyRequest.RecordTypeSpecified
            oImpRequest.RecordType = oFindPolicyRequest.RecordType.ToString()
            oImpRequest.ProductCode = oFindPolicyRequest.ProductCode
            oImpRequest.InsuranceRef = oFindPolicyRequest.InsuranceRef
            oImpRequest.AgentKey = oFindPolicyRequest.AgentKey
            oImpRequest.InsuredName = oFindPolicyRequest.InsuredName
            oImpRequest.QuoteORLiveDateSpecified = oFindPolicyRequest.QuoteORLiveDateSpecified
            oImpRequest.QuoteORLiveDate = oFindPolicyRequest.QuoteORLiveDate
            oImpRequest.CoverStartDateSpecified = oFindPolicyRequest.CoverStartDateSpecified
            oImpRequest.CoverStartDate = oFindPolicyRequest.CoverStartDate
            oImpRequest.MaxRowsToFetchSpecified = oFindPolicyRequest.MaxRowsToFetchSpecified
            If (oImpRequest.RetrieveAssociates AndAlso oImpRequest.InsuranceRef <> "" AndAlso oImpRequest.InsuranceRef.Contains("%")) = False Then
                oImpRequest.RetrieveAssociates = True
            Else
                oImpRequest.RetrieveAssociates = False
            End If

            If oFindPolicyRequest.MaxRowsToFetchSpecified Then
                oImpRequest.MaxRowsToFetch = oFindPolicyRequest.MaxRowsToFetch
            Else
                oImpRequest.MaxRowsToFetch = -1
            End If
            oImpRequest.WCFSecurityToken = If(oFindPolicyRequest.WCFSecurityToken.Length > 0, oFindPolicyRequest.WCFSecurityToken, "WCFSecurityToken")
            oImpRequest.RiskIndex = oFindPolicyRequest.RiskIndex
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.FindLatestPolicyVersions(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.InsuranceFileDetails = DataTabletoList_FindLatestPolicyVersions(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oFindPolicyRequest))
            Return Nothing
        End Try
    End Function

    Public Function CheckDocumentTemplateExists(ByVal oGetDocumentTemplateStatusRequest As GetDocumentTemplateStatusRequestType) As GetDocumentTemplateStatusResponseType Implements IPurePolicyService.CheckDocumentTemplateExists

        Try

            Dim sUserName As String = oGetDocumentTemplateStatusRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            'CommonFunctions.CheckAuthority("SAMAQuot", iUserId)
            CommonFunctions.CheckSecurityToken(oGetDocumentTemplateStatusRequest.WCFSecurityToken)
            Dim oResponse As New GetDocumentTemplateStatusResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetDocumentTemplateStatusRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetDocumentTemplateStatusRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetDocumentTemplateStatusResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetDocumentTemplateStatusRequest.BranchCode
            oImpRequest.DocumentTemplateKey = oGetDocumentTemplateStatusRequest.DocumentTemplateKey

            oImpRequest.WCFSecurityToken = If(oGetDocumentTemplateStatusRequest.WCFSecurityToken.Length > 0, oGetDocumentTemplateStatusRequest.WCFSecurityToken, "WCFSecurityToken")
            Try

                ' Call the implementation method
                oImpResponse = oBusiness.CheckDocumentTemplateExists(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                If oImpResponse IsNot Nothing Then
                    oResponse.DocumentTemplateStatus = oImpResponse.DocumentTemplateStatus

                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex)
            Return Nothing
        End Try

    End Function

    Public Function GetInstalmentSettlementAmount(ByVal oGetInstalmentSettlementAmountRequest As GetInstalmentSettlementAmountRequestType) As GetInstalmentSettlementAmountResponseType Implements IPurePolicyService.GetInstalmentSettlementAmount

        Try

            Dim sUserName As String = oGetInstalmentSettlementAmountRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            'CommonFunctions.CheckAuthority("SAMAQuot", iUserId)
            CommonFunctions.CheckSecurityToken(oGetInstalmentSettlementAmountRequest.WCFSecurityToken)
            Dim oResponse As New GetInstalmentSettlementAmountResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetInstalmentSettlementAmountRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetInstalmentSettlementAmountRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetInstalmentSettlementAmountResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetInstalmentSettlementAmountRequest.BranchCode
            oImpRequest.nInsuranceFileKey = oGetInstalmentSettlementAmountRequest.nInsuranceFileKey

            oImpRequest.WCFSecurityToken = If(oGetInstalmentSettlementAmountRequest.WCFSecurityToken.Length > 0, oGetInstalmentSettlementAmountRequest.WCFSecurityToken, "WCFSecurityToken")
            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetInstalmentSettlementAmount(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                If oImpResponse IsNot Nothing Then
                    oResponse.dInstalmentSettlementAmount = oImpResponse.dInstalmentSettlementAmount
                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetInstalmentSettlementAmountRequest))
            Return Nothing
        End Try
    End Function


    Public Function CancelMTAQuote(ByVal oCancelMTAQuoteRequest As CancelMTAQuoteRequestType) As CancelMTAQuoteResponseType Implements IPurePolicyService.CancelMTAQuote
        Try

            Dim sUserName As String = oCancelMTAQuoteRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMACMQ", iUserId)
            CommonFunctions.CheckSecurityToken(oCancelMTAQuoteRequest.WCFSecurityToken)


            Dim oResponse As New CancelMTAQuoteResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oCancelMTAQuoteRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.CancelMTAQuoteRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.CancelMTAQuoteResponseType = Nothing

            oImpRequest.BranchCode = oCancelMTAQuoteRequest.BranchCode
            oImpRequest.InsuranceFileKey = oCancelMTAQuoteRequest.InsuranceFileKey
            Try

                ' Call the implementation method
                oImpResponse = oBusiness.CancelMTAQuote(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex)
            End Try
            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oCancelMTAQuoteRequest))
            Return Nothing
        End Try
    End Function

    Public Function SavePremiumFinanceDetails(ByVal oSavePremiumFinanceDetailsRequest As SavePremiumFinanceDetailsRequestType) As SavePremiumFinanceDetailsResponseType Implements IPurePolicyService.SavePremiumFinanceDetails
        Try
            Dim sUserName As String = oSavePremiumFinanceDetailsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckSecurityToken(oSavePremiumFinanceDetailsRequest.WCFSecurityToken)

            Dim oResponse As New SavePremiumFinanceDetailsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oSavePremiumFinanceDetailsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.SavePremiumFinanceDetailsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.SavePremiumFinanceDetailsResponseType = Nothing
            Dim oGetInstalmentQuotesRequest As New GetInstalmentQuotesRequestType
            Dim oGetInstalmentQuotesResponse As New GetInstalmentQuotesResponseType
            With oGetInstalmentQuotesRequest
                .LoginUserName = sUserName
                .WCFSecurityToken = oSavePremiumFinanceDetailsRequest.WCFSecurityToken
                .AmountToFinance = oSavePremiumFinanceDetailsRequest.AmountToFinance
                .BranchCode = oSavePremiumFinanceDetailsRequest.BranchCode
                .EndDate = oSavePremiumFinanceDetailsRequest.EndDate
                .InsuranceFileKey = oSavePremiumFinanceDetailsRequest.InsuranceFileKey
                .MonthDay = oSavePremiumFinanceDetailsRequest.MonthDay
                .PaymentProtection = oSavePremiumFinanceDetailsRequest.PaymentProtection
                .PreferredDate = oSavePremiumFinanceDetailsRequest.PreferredDate
                .QuoteDate = oSavePremiumFinanceDetailsRequest.QuoteDate
                .StartDate = oSavePremiumFinanceDetailsRequest.StartDate
                .WeekDay = oSavePremiumFinanceDetailsRequest.WeekDay
                .OverrideCommission = oSavePremiumFinanceDetailsRequest.OverrideCommission
                .OverrideDepositAmount = oSavePremiumFinanceDetailsRequest.OverrideDepositAmount
                .OverrideInterestRate = oSavePremiumFinanceDetailsRequest.OverrideInterestRate
                .OverrideRate = oSavePremiumFinanceDetailsRequest.OverrideRate

            End With
            With oImpRequest
                ' Pass the values to the implementation request structure
                oImpRequest.BranchCode = oSavePremiumFinanceDetailsRequest.BranchCode
                oImpRequest.InsuranceFileKey = oSavePremiumFinanceDetailsRequest.InsuranceFileKey
                oImpRequest.SchemeNo = oSavePremiumFinanceDetailsRequest.SchemeNo
                oImpRequest.SchemeVersion = oSavePremiumFinanceDetailsRequest.SchemeVersion
                oImpRequest.PFRF_ID = oSavePremiumFinanceDetailsRequest.PFRF_ID
                oImpRequest.MonthDay = oSavePremiumFinanceDetailsRequest.MonthDay
                oImpRequest.AmountToFinance = oSavePremiumFinanceDetailsRequest.AmountToFinance
                oImpRequest.NetAmount = oSavePremiumFinanceDetailsRequest.NetAmount
                oImpRequest.Deposit = oSavePremiumFinanceDetailsRequest.Deposit
                oImpRequest.PreferredDate = oSavePremiumFinanceDetailsRequest.PreferredDate
                oImpRequest.WeekDay = oSavePremiumFinanceDetailsRequest.WeekDay
                oImpRequest.BankAccountName = oSavePremiumFinanceDetailsRequest.BankAccountName
                oImpRequest.BankAccountNo = oSavePremiumFinanceDetailsRequest.BankAccountNo
                oImpRequest.BankSortCode = oSavePremiumFinanceDetailsRequest.BankSortCode
                oImpRequest.PartyBankKey = oSavePremiumFinanceDetailsRequest.PartyBankKey
                oImpRequest.BankAreaCode = oSavePremiumFinanceDetailsRequest.BankAreaCode
                oImpRequest.BankBranch = oSavePremiumFinanceDetailsRequest.BankBranch
                oImpRequest.BankExtn = oSavePremiumFinanceDetailsRequest.BankExtn
                oImpRequest.BankFax = oSavePremiumFinanceDetailsRequest.BankFax
                oImpRequest.BankFaxCode = oSavePremiumFinanceDetailsRequest.BankFaxCode
                oImpRequest.BankName = oSavePremiumFinanceDetailsRequest.BankName
                oImpRequest.BankPhone = oSavePremiumFinanceDetailsRequest.BankPhone
                oImpRequest.EndDate = oSavePremiumFinanceDetailsRequest.EndDate
                oImpRequest.OverrideRate = oSavePremiumFinanceDetailsRequest.OverrideRate
                oImpRequest.OverrideInterestRate = oSavePremiumFinanceDetailsRequest.OverrideInterestRate
                oImpRequest.OverrideCommission = oSavePremiumFinanceDetailsRequest.OverrideCommission
                .StartDate = oSavePremiumFinanceDetailsRequest.StartDate
                .EndDate = oSavePremiumFinanceDetailsRequest.EndDate
                .WeekDay = oSavePremiumFinanceDetailsRequest.WeekDay
                .MonthDay = oSavePremiumFinanceDetailsRequest.MonthDay
                .NetAmount = oSavePremiumFinanceDetailsRequest.NetAmount

                'THIS INFORMATION IS RETRIVED FROM SELECTED INSTALMENT QUOTE
                oGetInstalmentQuotesResponse = GetInstalmentQuotes(oGetInstalmentQuotesRequest)

                Dim oInstalmentQuoteSelected As BaseGetInstalmentQuotesResponseTypeRow = Nothing
                For Each oInstalmentQuote As BaseGetInstalmentQuotesResponseTypeRow In oGetInstalmentQuotesResponse.Quotes
                    If oInstalmentQuote.SchemeNo = oSavePremiumFinanceDetailsRequest.SchemeNo AndAlso oInstalmentQuote.SchemeVersion = oSavePremiumFinanceDetailsRequest.SchemeVersion Then
                        oInstalmentQuoteSelected = oInstalmentQuote
                        Exit For
                    End If
                Next

                If oSavePremiumFinanceDetailsRequest.PreferredDate = DateTime.MinValue Then
                    .FirstInstalmentDate = oInstalmentQuoteSelected.FirstInstalmentDate
                Else
                    .FirstInstalmentDate = oSavePremiumFinanceDetailsRequest.PreferredDate
                End If

                .NextInstalmentDate = oInstalmentQuoteSelected.NextInstalmentDate
                .LastInstalmentDate = oInstalmentQuoteSelected.LastInstalmentDate
                .DaysDelay = oInstalmentQuoteSelected.DaysDelay
                .NoOfInstallments = oInstalmentQuoteSelected.InstalmentsToPay

                'THIS IS IN CASE OF ANNUAL PLAN -SPECIAL CASE
                If .NextInstalmentDate = .LastInstalmentDate AndAlso .NoOfInstallments = 1 Then
                    .NextInstalmentDate = .FirstInstalmentDate
                    .LastInstalmentDate = .FirstInstalmentDate
                    .WeekDay = .FirstInstalmentDate.Day
                    .MonthDay = .FirstInstalmentDate.Day
                End If

                .FirstInstallment = oInstalmentQuoteSelected.FirstInstalmentAmount
                .OtherInstallments = oInstalmentQuoteSelected.OtherInstalmentAmount
                .Deposit = oInstalmentQuoteSelected.DepositAmount
                .FinanceFee = oInstalmentQuoteSelected.FinanceCharge
                .OriginalAmount = oInstalmentQuoteSelected.OriginalAmount
                .CostOfProtection = oInstalmentQuoteSelected.ProtectionAmount
                .AmountToFinance = oInstalmentQuoteSelected.TotalInstalmentsAmount
                .TotalCost = oInstalmentQuoteSelected.TotalAmountInput





                oImpRequest.OverrideDepositAmount = oSavePremiumFinanceDetailsRequest.OverrideDepositAmount
                If oSavePremiumFinanceDetailsRequest.CreditCard IsNot Nothing Then
                    .CreditCard = New BaseImplementationTypes.BaseCreditCardType()
                    .CreditCard.NameOnCreditCard = oSavePremiumFinanceDetailsRequest.CreditCard.NameOnCreditCard
                    .CreditCard.Number = oSavePremiumFinanceDetailsRequest.CreditCard.Number
                    .CreditCard.ExpiryDate = oSavePremiumFinanceDetailsRequest.CreditCard.ExpiryDate
                    .CreditCard.StartDate = oSavePremiumFinanceDetailsRequest.CreditCard.StartDate
                    .CreditCard.Issue = oSavePremiumFinanceDetailsRequest.CreditCard.Issue
                    .CreditCard.Pin = oSavePremiumFinanceDetailsRequest.CreditCard.Pin
                    .CreditCard.TypeCode = oSavePremiumFinanceDetailsRequest.CreditCard.TypeCode
                    .CreditCard.AuthCode = oSavePremiumFinanceDetailsRequest.CreditCard.AuthCode
                    .CreditCard.PartyBankKey = oSavePremiumFinanceDetailsRequest.CreditCard.PartyBankKey
                    If oSavePremiumFinanceDetailsRequest.CreditCard.CardHolder IsNot Nothing Then
                        .CreditCard.CardHolder = New BaseImplementationTypes.BaseCreditCardTypeCardHolder()
                        .CreditCard.CardHolder.Name = oSavePremiumFinanceDetailsRequest.CreditCard.CardHolder.Name
                        .CreditCard.CardHolder.AddressLine1 = oSavePremiumFinanceDetailsRequest.CreditCard.CardHolder.AddressLine1
                        .CreditCard.CardHolder.AddressLine2 = oSavePremiumFinanceDetailsRequest.CreditCard.CardHolder.AddressLine2
                        .CreditCard.CardHolder.AddressLine3 = oSavePremiumFinanceDetailsRequest.CreditCard.CardHolder.AddressLine3
                        .CreditCard.CardHolder.AddressLine4 = oSavePremiumFinanceDetailsRequest.CreditCard.CardHolder.AddressLine4
                        .CreditCard.CardHolder.PostCode = oSavePremiumFinanceDetailsRequest.CreditCard.CardHolder.PostCode
                    End If
                End If
            End With
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.SavePremiumFinanceDetails(oImpRequest)
                If oImpResponse IsNot Nothing Then
                    SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                End If
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oSavePremiumFinanceDetailsRequest))
            End Try

            Return oResponse
            Catch ex As Exception
                CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oSavePremiumFinanceDetailsRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' UpdatePolicyAssociates
    ''' </summary>
    ''' <param name="oUpdatePolicyAssociatesStatusRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdatePolicyAssociates(ByVal oUpdatePolicyAssociatesStatusRequest As UpdatePolicyAssociatesRequestType) As UpdatePolicyAssociatesResponsType Implements IPurePolicyService.UpdatePolicyAssociates

        Try

            Dim sUserName As String = oUpdatePolicyAssociatesStatusRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckSecurityToken(oUpdatePolicyAssociatesStatusRequest.WCFSecurityToken)
            Dim oResponse As New UpdatePolicyAssociatesResponsType

            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oUpdatePolicyAssociatesStatusRequest.BranchCode)
            Dim iCount As Integer = 0
            ' Implementation structures

            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdatePolicyAssociatesRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdatePolicyAssociatesResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oUpdatePolicyAssociatesStatusRequest.BranchCode
            oImpRequest.TimeStamp = oUpdatePolicyAssociatesStatusRequest.TimeStamp

            If oUpdatePolicyAssociatesStatusRequest.Associates IsNot Nothing Then

                'Temporary objects to hold the Policy Associates
                Dim oPolicyAssociates() As SiriusFS.SAM.Structure.BaseImplementationTypes.BasePolicyAssociatesType = Nothing
                Dim oPolicyAssociatesItem As SiriusFS.SAM.Structure.BaseImplementationTypes.BasePolicyAssociatesType

                ReDim oPolicyAssociates(oUpdatePolicyAssociatesStatusRequest.Associates.GetUpperBound(0))

                For iCntIndex As Integer = 0 To oUpdatePolicyAssociatesStatusRequest.Associates.GetUpperBound(0)
                    oPolicyAssociatesItem = New SiriusFS.SAM.Structure.BaseImplementationTypes.BasePolicyAssociatesType

                    If Not oUpdatePolicyAssociatesStatusRequest.Associates(iCntIndex) Is Nothing AndAlso oUpdatePolicyAssociatesStatusRequest.Associates(iCntIndex).PartyKey <> 0 Then

                        With oUpdatePolicyAssociatesStatusRequest.Associates(iCntIndex)
                            oPolicyAssociatesItem.RowKey = .RowKey
                            oPolicyAssociatesItem.InsuranceFileAssociatesKey = .InsuranceFileAssociatesKey
                            oPolicyAssociatesItem.InsuranceFileKey = .InsuranceFileKey
                            oPolicyAssociatesItem.InsuranceFolderCnt = .InsuranceFolderCnt
                            oPolicyAssociatesItem.PartyKey = .PartyKey
                            oPolicyAssociatesItem.AssociationTypeKey = .AssociationTypeKey
                            oPolicyAssociatesItem.DateAttached = .DateAttached
                            oPolicyAssociatesItem.DateAttachedSpecified = .DateAttachedSpecified
                            oPolicyAssociatesItem.DateRemovedSpecified = .DateRemovedSpecified
                            oPolicyAssociatesItem.IsDeletedSpecified = .IsDeletedSpecified
                            oPolicyAssociatesItem.AssociationDetail = .AssociationDetail
                            oPolicyAssociatesItem.ActionType = CType([Enum].ToObject(GetType(RowAction), .ActionType), BaseImplementationTypes.RowAction)

                            If oPolicyAssociatesItem.ActionType = BaseImplementationTypes.RowAction.AddRow Then
                                oPolicyAssociatesItem.IsAddUnConfirmed = True
                                oPolicyAssociatesItem.IsDelUnConfirmed = .IsDelUnConfirmed
                            ElseIf oPolicyAssociatesItem.ActionType = BaseImplementationTypes.RowAction.DeleteRow Then
                                oPolicyAssociatesItem.IsDelUnConfirmed = True
                                oPolicyAssociatesItem.IsAddUnConfirmed = .IsAddUnConfirmed
                            End If

                            If oPolicyAssociatesItem.DateRemovedSpecified Then
                                oPolicyAssociatesItem.DateRemoved = .DateRemoved
                            Else
                                oPolicyAssociatesItem.DateRemoved = Nothing

                            End If

                            If oPolicyAssociatesItem.IsDeletedSpecified Then
                                oPolicyAssociatesItem.IsDeleted = .IsDeleted
                            Else
                                oPolicyAssociatesItem.IsDeleted = False
                            End If

                        End With
                        oPolicyAssociates(iCntIndex) = oPolicyAssociatesItem
                    End If
                Next
                oImpRequest.Associates = oPolicyAssociates
                oImpRequest.SkipPolicyTypeCheck = oUpdatePolicyAssociatesStatusRequest.SkipPolicyTypeCheck
            End If

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdatePolicyAssociates(oImpRequest)
                If Not oImpResponse Is Nothing Then
                    oResponse.TimeStamp = oImpResponse.TimeStamp
                End If
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdatePolicyAssociatesStatusRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetPolicyAssociatesStatusRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPolicyAssociates(ByVal oGetPolicyAssociatesStatusRequest As GetPolicyAssociatesRequestType) As GetPolicyAssociatesResponsType Implements IPurePolicyService.GetPolicyAssociates

        Try

            Dim sUserName As String = oGetPolicyAssociatesStatusRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckSecurityToken(oGetPolicyAssociatesStatusRequest.WCFSecurityToken)

            Dim oResponse As New GetPolicyAssociatesResponsType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetPolicyAssociatesStatusRequest.BranchCode)
            Dim iCount As Integer = 0
            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetPolicyAssociatesRequestType

            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetPolicyAssociatesResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.InsuranceFileKey = oGetPolicyAssociatesStatusRequest.InsuranceFileKey
            oImpRequest.BranchCode = oGetPolicyAssociatesStatusRequest.BranchCode
            oImpRequest.InsuranceFolderCnt = oGetPolicyAssociatesStatusRequest.InsuranceFolderCnt


            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetPolicyAssociates(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                If Not oImpResponse Is Nothing Then
                    With oImpResponse
                        oResponse.AllocationTimeStamp = .AllocationTimeStamp

                        'Setting the ImpResponse of BasePremiumFinancePlanTransactionsType To Portal Response 
                        If oImpResponse.AssociatesRow IsNot Nothing Then
                            Dim iCntAssociates As Integer = 0
                            Dim iuBoundAssociates As Integer = oImpResponse.AssociatesRow.GetUpperBound(0)
                            Dim ilBoundAssociates As Integer = oImpResponse.AssociatesRow.GetLowerBound(0)

                            If oImpResponse.AssociatesRow.Length > 0 Then

                                ReDim oResponse.AssociatesRow(iuBoundAssociates)

                                For iCntAssociates = ilBoundAssociates To iuBoundAssociates

                                    oResponse.AssociatesRow(iCntAssociates) = New BaseGetPolicyAssociatesResponseTypeAssociatesRow
                                    With oResponse.AssociatesRow(iCntAssociates)
                                        .PartyCode = oImpResponse.AssociatesRow(iCntAssociates).PartyCode
                                        .PartyName = oImpResponse.AssociatesRow(iCntAssociates).PartyName
                                        .PartyType = CType(oImpResponse.AssociatesRow(iCntAssociates).PartyType, PartyTypeType)

                                        'AssociatesDetails
                                        oResponse.AssociatesRow(iCntAssociates).Associates = New BasePolicyAssociatesType
                                        .Associates.RowKey = oImpResponse.AssociatesRow(iCntAssociates).Associates.RowKey
                                        .Associates.InsuranceFileAssociatesKey = oImpResponse.AssociatesRow(iCntAssociates).Associates.InsuranceFileAssociatesKey
                                        .Associates.InsuranceFolderCnt = oImpResponse.AssociatesRow(iCntAssociates).Associates.InsuranceFolderCnt
                                        .Associates.PartyKey = oImpResponse.AssociatesRow(iCntAssociates).Associates.PartyKey
                                        .Associates.InsuranceFileKey = oImpResponse.AssociatesRow(iCntAssociates).Associates.InsuranceFileKey
                                        .Associates.AssociationTypeKey = oImpResponse.AssociatesRow(iCntAssociates).Associates.AssociationTypeKey
                                        .Associates.DateAttachedSpecified = True
                                        .Associates.DateAttached = oImpResponse.AssociatesRow(iCntAssociates).Associates.DateAttached
                                        .Associates.DateRemoved = oImpResponse.AssociatesRow(iCntAssociates).Associates.DateRemoved
                                        .Associates.DateRemovedSpecified = True
                                        .Associates.IsDeletedSpecified = True
                                        .Associates.IsDeleted = oImpResponse.AssociatesRow(iCntAssociates).Associates.IsDeleted
                                        .Associates.AssociationDetail = oImpResponse.AssociatesRow(iCntAssociates).Associates.AssociationDetail
                                        .Associates.ActionType = CType(oImpResponse.AssociatesRow(iCntAssociates).Associates.ActionType, RowAction)
                                        .Associates.IsDelUnConfirmed = oImpResponse.AssociatesRow(iCntAssociates).Associates.IsDelUnConfirmed
                                        .Associates.IsAddUnConfirmed = oImpResponse.AssociatesRow(iCntAssociates).Associates.IsAddUnConfirmed

                                        'Party Address Setting
                                        oResponse.AssociatesRow(iCntAssociates).Address = New BaseAddressType
                                        .Address.AddressLine1 = oImpResponse.AssociatesRow(iCntAssociates).Address.AddressLine1
                                        .Address.AddressLine2 = oImpResponse.AssociatesRow(iCntAssociates).Address.AddressLine2
                                        .Address.AddressLine3 = oImpResponse.AssociatesRow(iCntAssociates).Address.AddressLine3
                                        .Address.AddressLine4 = oImpResponse.AssociatesRow(iCntAssociates).Address.AddressLine4
                                        .Address.AddressTypeCode = CType(oImpResponse.AssociatesRow(iCntAssociates).Address.AddressTypeCode, AddressTypeType)
                                        .Address.CountryCode = oImpResponse.AssociatesRow(iCntAssociates).Address.CountryCode
                                        .Address.PostCode = oImpResponse.AssociatesRow(iCntAssociates).Address.PostCode
                                    End With
                                Next
                            End If
                        End If

                    End With
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex)
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetPolicyAssociatesStatusRequest))
            Return Nothing
        End Try

    End Function

    Public Function ReverseCollectedInstalment(ByVal oReverseCollectedInstalmentRequestType As ReverseCollectedInstalmentRequestType) As ReverseCollectedInstalmentResponseType Implements IPurePolicyService.ReverseCollectedInstalment
        Dim sUserName As String = oReverseCollectedInstalmentRequestType.LoginUserName
        Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oReverseCollectedInstalmentRequestType.BranchCode)
        Try
            Dim nUserId As Integer
            CommonFunctions.GetIdentity(sUserName, 0, nUserId)
            CommonFunctions.CheckSecurityToken(oReverseCollectedInstalmentRequestType.WCFSecurityToken)

            Dim oResponse As New ReverseCollectedInstalmentResponseType


            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.ReverseCollectedInstalmentRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.ReverseCollectedInstalmentResponseType = Nothing
            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oReverseCollectedInstalmentRequestType.BranchCode
            oImpRequest.PFInstalmentId = oReverseCollectedInstalmentRequestType.PFInstalmentId
            oImpRequest.PFPlanStatusInd = oReverseCollectedInstalmentRequestType.PFPlanStatusInd
            ' Call the implementation method
            Try

                oImpResponse = oBusiness.ReverseCollectedInstalment(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex)
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oReverseCollectedInstalmentRequestType))
            Return Nothing
        Finally
            oBusiness = Nothing
        End Try
    End Function

    Public Function ExecutePRERuleset(ByVal oExecutePRERulesetRequest As ExecutePRERulesetRequestType) As ExecutePRERulesetResponseType Implements IPurePolicyService.ExecutePRERuleset

        Try

            Dim sUserName As String = oExecutePRERulesetRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            Dim bIgnoreErrorMessage As Boolean = False

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMURsk", iUserId)
            CommonFunctions.CheckSecurityToken(oExecutePRERulesetRequest.WCFSecurityToken)
            Dim oResponse As New ExecutePRERulesetResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oExecutePRERulesetRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.ExecutePRERulesetRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.ExecutePRERulesetResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = oExecutePRERulesetRequest.BranchCode
            oImpRequest.InsuranceFileKey = oExecutePRERulesetRequest.InsuranceFileKey
            oImpRequest.InsuranceFolderKey = oExecutePRERulesetRequest.InsuranceFolderKey
            oImpRequest.QuoteTimeStamp = oExecutePRERulesetRequest.QuoteTimeStamp
            oImpRequest.RiskDescription = oExecutePRERulesetRequest.RiskDescription
            oImpRequest.RiskKey = oExecutePRERulesetRequest.RiskKey
            oImpRequest.ScreenCode = oExecutePRERulesetRequest.ScreenCode
            oImpRequest.SubBranchCode = oExecutePRERulesetRequest.SubBranchCode
            oImpRequest.UserName = sUserName
            oImpRequest.XMLDataSet = SAMFunc.TransformDatasetSAMtoPB(oExecutePRERulesetRequest.XMLDataSet)
            oImpRequest.TransactionType = oExecutePRERulesetRequest.TransactionType
            oImpRequest.PRERuleAssemblyName = oExecutePRERulesetRequest.PRERuleAssemblyName
            oImpRequest.RunPrePRERule = oExecutePRERulesetRequest.RunPrePRERule
            oImpRequest.RunPostPRERule = oExecutePRERulesetRequest.RunPostPRERule
            oImpRequest.CoverStartDate = oExecutePRERulesetRequest.CoverStartDate

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.ExecutePRERuleset(oImpRequest)
                oResponse.QuoteTimeStamp = oImpResponse.QuoteTimeStamp
                'XML Can be updated in error cases
                oResponse.XMLDataSet = SAMFunc.TransformDatasetPBtoSAM(oImpResponse.XMLDataSet, sCalledVia:="ExecutePRERuleset")

                If oImpResponse IsNot Nothing Then
                    If oImpResponse.STSError IsNot Nothing Then
                        If oImpResponse.STSError.STSBusinessRule IsNot Nothing Then
                            If (oImpResponse.STSError.STSBusinessRule.Code = Convert.ToString(STSErrorPublisher.STSErrorCodes.ValidationRulesReferred) OrElse
                            oImpResponse.STSError.STSBusinessRule.Code = Convert.ToString(STSErrorPublisher.STSErrorCodes.ValidationRulesDeclined) OrElse
                            oImpResponse.STSError.STSBusinessRule.Code = Convert.ToString(STSErrorPublisher.STSErrorCodes.UALRulesReferred) OrElse
                            oImpResponse.STSError.STSBusinessRule.Code = Convert.ToString(STSErrorPublisher.STSErrorCodes.UALRulesDeclined) OrElse
                            oImpResponse.STSError.STSBusinessRule.Code = Convert.ToString(STSErrorPublisher.STSErrorCodes.RatingRulesReferred) OrElse
                            oImpResponse.STSError.STSBusinessRule.Code = Convert.ToString(STSErrorPublisher.STSErrorCodes.RatingRulesDeclined)) AndAlso oExecutePRERulesetRequest.IgnoreErrorMessage Then
                                bIgnoreErrorMessage = True
                            End If

                        End If
                    End If
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oExecutePRERulesetRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oExecutePRERulesetRequest))
            Return Nothing
        End Try

    End Function

#Region "Update Policy Payment Method"
    ''' <summary>
    ''' Update Policy Payment Method
    ''' </summary>
    ''' <param name="oUpdatePolicyPaymentMethodRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdatePolicyPaymentMethod(ByVal oUpdatePolicyPaymentMethodRequest As UpdatePolicyPaymentMethodRequestType) As UpdatePolicyPaymentMethodResponseType Implements IPurePolicyService.UpdatePolicyPaymentMethod

        Try

            Dim sUserName As String = oUpdatePolicyPaymentMethodRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMUPAYM", iUserId)
            CommonFunctions.CheckSecurityToken(oUpdatePolicyPaymentMethodRequest.WCFSecurityToken)
            Dim oUpdatePolicyPaymentMethodResponse As New UpdatePolicyPaymentMethodResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness()

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdatePolicyPaymentMethodRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdatePolicyPaymentMethodResponseType = Nothing

            ' Pass the values to the implementation request structure
            With oUpdatePolicyPaymentMethodRequest
                oImpRequest.BranchCode = .BranchCode
                oImpRequest.InsuranceFileKey = .InsuranceFileKey
                oImpRequest.PolicyPaymentMethod = .PolicyPaymentMethod
                oImpRequest.QuoteTimeStamp = .QuoteTimeStamp
            End With

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.UpdatePolicyPaymentMethod(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oUpdatePolicyPaymentMethodResponse.QuoteTimeStamp = oImpResponse.QuoteTimeStamp

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oUpdatePolicyPaymentMethodResponse, ex)
            End Try

            Return oUpdatePolicyPaymentMethodResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex)
            Return Nothing
        End Try
    End Function
#End Region
    ''' <summary>
    ''' GetRITreatyPartyDetailsWithTax
    ''' </summary>
    ''' <param name="oGetRITreatyPartyDetailsWithTaxRequest"></param>
    ''' <returns></returns>
    Public Function GetRITreatyPartyDetailsWithTax(ByVal oGetRITreatyPartyDetailsWithTaxRequest As GetRITreatyPartyDetailsWithTaxRequestType) As GetRITreatyPartyDetailsWithTaxResponseType Implements IPurePolicyService.GetRITreatyPartyDetailsWithTax

        Try

            Dim sUserName As String = oGetRITreatyPartyDetailsWithTaxRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGTtyPty", iUserId)
            CommonFunctions.CheckSecurityToken(oGetRITreatyPartyDetailsWithTaxRequest.WCFSecurityToken)

            Dim oResponse As New GetRITreatyPartyDetailsWithTaxResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetRITreatyPartyDetailsWithTaxRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetRITreatyPartyDetailsWithTaxRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetRITreatyPartyDetailsWithTaxResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetRITreatyPartyDetailsWithTaxRequest.BranchCode
            oImpRequest.InsuranceFileID = oGetRITreatyPartyDetailsWithTaxRequest.InsuranceFileID
            oImpRequest.RiskID = oGetRITreatyPartyDetailsWithTaxRequest.RiskID
            oImpRequest.RIArrangementLineID = oGetRITreatyPartyDetailsWithTaxRequest.RIArrangementLineID
            oImpRequest.TreatyID = oGetRITreatyPartyDetailsWithTaxRequest.TreatyID
            oImpRequest.Premium = oGetRITreatyPartyDetailsWithTaxRequest.Premium
            oImpRequest.PremiumTransType = oGetRITreatyPartyDetailsWithTaxRequest.PremiumTransType
            oImpRequest.Commission = oGetRITreatyPartyDetailsWithTaxRequest.Commission
            oImpRequest.CommissionTransType = oGetRITreatyPartyDetailsWithTaxRequest.CommissionTransType
            oImpRequest.TreatyCode = oGetRITreatyPartyDetailsWithTaxRequest.TreatyCode
            oImpRequest.IgnoreTreatyDetails = oGetRITreatyPartyDetailsWithTaxRequest.IgnoreTreatyDetails
            oImpRequest.IgnoreTax = oGetRITreatyPartyDetailsWithTaxRequest.IgnoreTax

            oImpRequest.WCFSecurityToken = If(oGetRITreatyPartyDetailsWithTaxRequest.WCFSecurityToken.Length > 0, oGetRITreatyPartyDetailsWithTaxRequest.WCFSecurityToken, "WCFSecurityToken")
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetRITreatyPartyDetailsWithTax(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'oResponse.Parties = SAMFunc.GetDeserializedValues(Of List(Of BaseGetTreatyPartyDetailsResponseTypePartiesRow))(elmResultDataSet:=oImpResponse.Parties, sFromTypeName:="BaseGetTreatyPartyDetailsResponseTypeParties", sConvertToTypeName:="BaseGetTreatyPartyDetailsResponseTypePartiesRow")
                If oImpResponse IsNot Nothing Then
                    oResponse.CommissionTax = oImpResponse.CommissionTax
                    oResponse.PremiumTax = oImpResponse.PremiumTax
                    oResponse.CommissionPercent = oImpResponse.CommissionPercent
                    oResponse.IsRetained = oImpResponse.IsRetained
                    oResponse.AgreementCode = oImpResponse.AgreementCode
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetRITreatyPartyDetailsWithTaxRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetRITreatyPartyDetailsWithTaxRequest))
            Return Nothing
        End Try

    End Function

    Public Function UpdateRiOverrideReasonInRiArrangement(ByVal oUpdateRiOverrideReasonInRiArrangementRequest As UpdateRiOverrideReasonInRiArrangementRequestType) As UpdateRiOverrideReasonInRiArrangementResponseType Implements IPurePolicyService.UpdateRiOverrideReasonInRiArrangement

        Try

            Dim sUserName As String = oUpdateRiOverrideReasonInRiArrangementRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGTtyPty", iUserId)
            CommonFunctions.CheckSecurityToken(oUpdateRiOverrideReasonInRiArrangementRequest.WCFSecurityToken)

            Dim oResponse As New UpdateRiOverrideReasonInRiArrangementResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oUpdateRiOverrideReasonInRiArrangementRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateRiOverrideReasonInRiArrangementRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateRiOverrideReasonInRiArrangementResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oUpdateRiOverrideReasonInRiArrangementRequest.BranchCode
            oImpRequest.RiArrangementId = oUpdateRiOverrideReasonInRiArrangementRequest.RiArrangementId
            oImpRequest.RiOverrideReasonId = oUpdateRiOverrideReasonInRiArrangementRequest.RiOverrideReasonId
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateRiOverrideReasonInRiArrangement(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdateRiOverrideReasonInRiArrangementRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateRiOverrideReasonInRiArrangementRequest))
            Return Nothing
        End Try

    End Function
    ''' <summary>
    ''' GetRIPropTreaties
    ''' </summary>
    ''' <param name="oGetRIPropTreatiesRequest"></param>
    Public Function GetRIPropTreaties(ByVal oGetRIPropTreatiesRequest As GetRIPropTreatiesRequestType) As GetRIPropTreatiesResponseType Implements IPurePolicyService.GetRIPropTreaties

        Try

            Dim sUserName As String = oGetRIPropTreatiesRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGTtyPty", iUserId)
            CommonFunctions.CheckSecurityToken(oGetRIPropTreatiesRequest.WCFSecurityToken)

            Dim oResponse As New GetRIPropTreatiesResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetRIPropTreatiesRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetRIPropTreatiesRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetRIPropTreatiesResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetRIPropTreatiesRequest.BranchCode
            oImpRequest.WCFSecurityToken = If(oGetRIPropTreatiesRequest.WCFSecurityToken.Length > 0, oGetRIPropTreatiesRequest.WCFSecurityToken, "WCFSecurityToken")
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetRIPropTreaties(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Treaties = DataTabletoList_GetRIPropTreaties(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetRIPropTreatiesRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetRIPropTreatiesRequest))
            Return Nothing
        End Try

    End Function
#Region "UpdateInstalmentDetails"
    ''' <summary>
    ''' Update Instalment Details
    ''' </summary>
    ''' <param name="oUpdateInstalmentDetailsRequestType"></param>
    ''' <returns></returns>
    Public Function UpdateInstalmentDetails(ByVal oUpdateInstalmentDetailsRequestType As UpdateInstalmentDetailsRequestType) As UpdateInstalmentDetailsResponseType Implements IPurePolicyService.UpdateInstalmentDetails
        Dim sUserName As String = oUpdateInstalmentDetailsRequestType.LoginUserName
        Dim iAgentKey As Integer
        Dim iUserId As Integer
        CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
        CommonFunctions.CheckAuthority("SAMUPDIS", iUserId)
        CommonFunctions.CheckSecurityToken(oUpdateInstalmentDetailsRequestType.WCFSecurityToken)

        Dim oResponse As New UpdateInstalmentDetailsResponseType
        Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oUpdateInstalmentDetailsRequestType.BranchCode)

        ' Implementation structures
        Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdateInstalmentDetailsRequestType
        Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdateInstalmentDetailsResponseType = Nothing

        Try

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oUpdateInstalmentDetailsRequestType.BranchCode
            oImpRequest.FinancialPlanKey = oUpdateInstalmentDetailsRequestType.FinancialPlanKey
            oImpRequest.FinancialPlanVersion = oUpdateInstalmentDetailsRequestType.FinancialPlanVersion
            oImpRequest.DueDate = oUpdateInstalmentDetailsRequestType.DueDate
            oImpRequest.InstalmentNo = oUpdateInstalmentDetailsRequestType.InstalmentNo

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateInstalmentDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdateInstalmentDetailsRequestType))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdateInstalmentDetailsRequestType))
            Return Nothing
        End Try

    End Function
#End Region

End Class
