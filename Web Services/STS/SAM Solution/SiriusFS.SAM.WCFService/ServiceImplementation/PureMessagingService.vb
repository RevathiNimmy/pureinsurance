Imports SiriusFS.SAM.Structure
Imports SiriusFS.SAM.Structure.SFI.Messaging.WCF
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

<AspNetCompatibilityRequirements(RequirementsMode:=AspNetCompatibilityRequirementsMode.Required)>
Partial Public Class PureMessagingService
    Implements IPureMessagingService

#Region "New Business"
    ''' <summary>
    ''' this method will run New business on the basis on request supplied from calling application
    ''' </summary>
    ''' <param name="NewBusinessRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function NewBusiness(ByVal NewBusinessRequest As NewBusinessRequestType) As NewBusinessResponseType Implements IPureMessagingService.NewBusiness

        Dim iLBnd As Integer = 0
        Dim iUBnd As Integer = 0
        Dim oMsgResponse As New NewBusinessResponseType
        Dim oMessagingBusiness As New CoreSAMBusiness()

        ' Implementation structures
        Dim oImpRequest As New MessagingImplementationTypes.NewBusinessRequestType
        Dim oImpResponse As MessagingImplementationTypes.NewBusinessResponseType = Nothing

        Try
            ' Convert the incoming interface structures into the implementation structures
            oImpRequest.AgentCode = NewBusinessRequest.AgentCode
            oImpRequest.BranchCode = NewBusinessRequest.BranchCode
            oImpRequest.CurrencyCode = CType([Enum].ToObject(GetType(CurrencyType), NewBusinessRequest.CurrencyCode), BaseImplementationTypes.CurrencyType)

            ' Process the Party structure.  We 1st need to check the party type of the incoming message
            If NewBusinessRequest.Item IsNot Nothing Then

                Dim oImpParty As New BaseImplementationTypes.BasePartyType

                ' Check the type of the party object to see if it is Personal or Corporate
                If NewBusinessRequest.Item.GetType Is GetType(BasePartyPCType) Then

                    ' Process personal client
                    Dim oMsgParty As BasePartyPCType = DirectCast(NewBusinessRequest.Item, BasePartyPCType)
                    Dim oImpPartyPC As New BaseImplementationTypes.BasePartyPCType
                    With oMsgParty
                        oImpPartyPC.Forename = .Forename
                        oImpPartyPC.Surname = .Surname
                        oImpPartyPC.Initials = .Initials
                        oImpPartyPC.Title = .Title
                        oImpPartyPC.DateOfBirth = .DateOfBirth.ToShortDateString
                        oImpPartyPC.GenderCode = .GenderCode
                        oImpPartyPC.MaritalStatusCode = CType([Enum].ToObject(GetType(MaritalStatusCodeType), .MaritalStatusCode), BaseImplementationTypes.MaritalStatusCodeType)
                        oImpPartyPC.OccupationCode = oMsgParty.OccupationCode
                        oImpPartyPC.EmploymentStatusCode = CType([Enum].ToObject(GetType(EmploymentStatusCodeType), .EmploymentStatusCode), BaseImplementationTypes.EmploymentStatusCodeType)
                        oImpPartyPC.EmployersBusinessCode = .EmployersBusinessCode
                        oImpPartyPC.AlternativeId = .AlternativeId
                        oImpPartyPC.Currency = .Currency
                        oImpParty = oImpPartyPC
                    End With
                Else

                    ' Process corporate client
                    Dim oMsgParty As BasePartyCCType = DirectCast(NewBusinessRequest.Item, BasePartyCCType)
                    Dim oImpPartyCC As New BaseImplementationTypes.BasePartyCCType

                    With oMsgParty
                        oImpPartyCC.CompanyName = .CompanyName
                        oImpPartyCC.BusinessCode = .BusinessCode
                    End With
                    oImpParty = oImpPartyCC

                End If

                ' Common to PC and CC
                oImpParty.BranchCode = NewBusinessRequest.Item.BranchCode
                oImpParty.TPUserCode = NewBusinessRequest.Item.TPUserCode
                oImpParty.TPIntroducer = NewBusinessRequest.Item.TPIntroducer

                ' Process the address structure
                If NewBusinessRequest.Item.Addresses IsNot Nothing Then
                    Dim oMsgAddress As BaseAddressType = Nothing
                    Dim oImpAddress As New BaseImplementationTypes.BaseAddressType
                    iUBnd% = NewBusinessRequest.Item.Addresses.Count - 1

                    ReDim oImpParty.Addresses(iUBnd%)

                    For iCnt As Integer = 0 To iUBnd%
                        oMsgAddress = NewBusinessRequest.Item.Addresses(iCnt%)
                        oImpAddress = New BaseImplementationTypes.BaseAddressType
                        With oMsgAddress
                            oImpAddress.AddressLine1 = .AddressLine1
                            oImpAddress.AddressLine2 = .AddressLine2
                            oImpAddress.AddressLine3 = .AddressLine3
                            oImpAddress.AddressLine4 = .AddressLine4
                            oImpAddress.AddressTypeCode = CType([Enum].ToObject(GetType(AddressTypeType), .AddressTypeCode), BaseImplementationTypes.AddressTypeType)
                            oImpAddress.CountryCode = .CountryCode
                            oImpAddress.PostCode = .PostCode

                            oImpParty.Addresses(iCnt%) = oImpAddress
                        End With
                    Next iCnt%

                End If

                ' Process the Contact structure
                If NewBusinessRequest.Item.Contacts IsNot Nothing Then
                    Dim oMsgContact As BaseContactType = Nothing
                    Dim oImpContact As New BaseImplementationTypes.BaseContactType

                    iUBnd% = NewBusinessRequest.Item.Contacts.Count - 1

                    ReDim oImpParty.Contacts(iUBnd%)

                    For iCnt As Integer = 0 To iUBnd%
                        oMsgContact = NewBusinessRequest.Item.Contacts(iCnt%)
                        oImpContact = New BaseImplementationTypes.BaseContactType
                        With oMsgContact
                            oImpContact.AreaCode = .AreaCode
                            oImpContact.ContactDetail = New BaseImplementationTypes.BaseContactDetailType
                            oImpContact.ContactDetail.Item = .ContactDetail.Item
                            oImpContact.ContactDetail.ItemElementName = CType([Enum].ToObject(GetType(ItemChoiceType), .ContactDetail.ItemElementName), BaseImplementationTypes.ItemChoiceType)
                            oImpContact.ContactTypeCode = CType([Enum].ToObject(GetType(ContactTypeType), .ContactTypeCode), BaseImplementationTypes.ContactTypeType)
                        End With
                        oImpParty.Contacts(iCnt%) = oImpContact
                    Next iCnt%

                End If

                oImpRequest.Party = oImpParty

            End If

            If NewBusinessRequest.Policy IsNot Nothing Then
                ' Process the Policy Structure
                oImpRequest.Policy = New BaseImplementationTypes.BaseQuoteRiskMsgType
                With NewBusinessRequest.Policy
                    oImpRequest.Policy.BranchCode = .BranchCode
                    oImpRequest.Policy.CoverStartDate = .CoverStartDate
                    oImpRequest.Policy.CoverEndDate = .CoverEndDate
                    oImpRequest.Policy.Description = .Description
                    oImpRequest.Policy.InsuredName = .InsuredName
                    oImpRequest.Policy.ProductCode = .ProductCode
                    oImpRequest.Policy.QuoteRef = .QuoteRef

                    If .Risks IsNot Nothing Then

                        Dim oMsgRisk As BaseRiskType
                        Dim oImpRisk As BaseImplementationTypes.BaseRiskType

                        iUBnd% = .Risks.Count - 1

                        ReDim oImpRequest.Policy.Risks(iUBnd%)

                        For iCnt As Integer = 0 To iUBnd%
                            oMsgRisk = .Risks(iCnt%)
                            oImpRisk = New BaseImplementationTypes.BaseRiskType
                            With oMsgRisk
                                oImpRisk.DataModelCode = .DataModelCode
                                oImpRisk.QuoteTimeStamp = .QuoteTimeStamp
                                oImpRisk.RiskDescription = .RiskDescription
                                oImpRisk.RiskTypeCode = .RiskTypeCode
                                oImpRisk.RunDefaultRules = .RunDefaultRules
                                oImpRisk.ScreenCode = .ScreenCode
                                oImpRisk.XMLDataSet = .XMLDataSet
                            End With
                            oImpRequest.Policy.Risks(iCnt%) = DirectCast(oImpRisk, BaseImplementationTypes.BaseQuoteRiskMsgTypeRisks)

                        Next iCnt%

                    End If
                End With
            End If

            oImpResponse = oMessagingBusiness.NewBusiness(oImpRequest)

            ' Return errors
            SAMFunc.ConvertWCFSTSError(oImpResponse.STSError, oMsgResponse.STSErrorType)

            ' Return details
            If (oMsgResponse.STSErrorType Is Nothing) And IsArray(oImpResponse.Policy) Then
                oMsgResponse.Insured = New BaseAddPartyResponseType
                oMsgResponse.Policy = New BaseNBQuoteResponseTypePolicy
                With oImpResponse
                    oMsgResponse.Insured.PartyKey = .Insured.PartyKey
                    oMsgResponse.Insured.Shortname = .Insured.Shortname
                    With oImpResponse.Policy(0)
                        oMsgResponse.Policy.PolicyID = .PolicyID
                        oMsgResponse.Policy.PremiumDueGross = .PremiumDueGross
                        oMsgResponse.Policy.PremiumDueNet = .PremiumDueNet
                        oMsgResponse.Policy.PremiumDueTax = .PremiumDueTax
                        oMsgResponse.Policy.QuoteRef = .QuoteRef
                        oMsgResponse.Policy.TotalAnnualTax = .TotalAnnualTax

                        ' Process the Risks structure
                        If IsArray(.Risks) Then
                            Dim oMsgRisk As BaseRiskResultType
                            Dim oImpRisk As BaseImplementationTypes.BaseRiskResultType

                            iLBnd% = .Risks.GetLowerBound(0)
                            iUBnd% = .Risks.GetUpperBound(0)

                            oMsgResponse.Policy.Risks.Capacity = iUBnd%

                            For iCnt As Integer = iLBnd% To iUBnd%
                                oImpRisk = .Risks(iCnt%)
                                oMsgRisk = New BaseRiskResultType
                                With oImpRisk
                                    oMsgRisk.PremiumDueGross = .PremiumDueGross
                                    oMsgRisk.PremiumDueNet = .PremiumDueNet
                                    oMsgRisk.PremiumDueTax = .PremiumDueTax
                                    oMsgRisk.TotalAnnualTax = .TotalAnnualTax
                                    oMsgRisk.RiskFolderID = .RiskFolderID
                                    oMsgRisk.RiskID = .RiskID
                                    oMsgRisk.XMLDataSet = SAMFunc.TransformDatasetPBtoSAM(.XMLDataSet)
                                End With
                                oMsgResponse.Policy.Risks(iCnt%) = oMsgRisk

                            Next iCnt%
                        End If
                    End With
                End With
            End If

        Catch SAMError As Sirius.Architecture.ExceptionHandling.SAMErrorException
            ConvertSAMErrorToSTSResponse(oMsgResponse, SAMError)
            Return oMsgResponse

        Catch exError As Exception
            Dim STSErrorFileError As New STSErrorPublisher("An error occured when attempting Add new business", exError)
            STSErrorFileError.Raise(HttpContext.Current.Request.Url.ToString(), "NewBusiness", "NewBusiness", True)

        End Try

        Return oMsgResponse

    End Function
#End Region

#Region "NewBusinessTransact"
    ''' <summary>
    ''' This method will transact New business on the basis on request supplied from calling application
    ''' </summary>
    ''' <param name="NBTransactRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function NewBusinessTransact(ByVal NBTransactRequest As NBTransactRequestType) As NBTransactResponseType Implements IPureMessagingService.NewBusinessTransact

        Dim oMsgResponse As New NBTransactResponseType

        Dim oImpRequest As New MessagingImplementationTypes.NBTransactRequestType
        Dim oImpResponse As New MessagingImplementationTypes.NBTransactResponseType

        Dim oMessagingBusiness As New CoreSAMBusiness()
        Try
            ' Move the incoming data to the implementation request structure
            oImpRequest.QuoteRef = NBTransactRequest.QuoteRef
            If NBTransactRequest.Risks IsNot Nothing Then

                Dim iUBnd As Integer = NBTransactRequest.Risks.Count - 1
                ReDim oImpRequest.Risks(iUBnd)
                For iCnt As Integer = 0 To iUBnd
                    With NBTransactRequest.Risks(iCnt)
                        oImpRequest.Risks(iCnt).BranchCode = .BranchCode
                        oImpRequest.Risks(iCnt).DataModelCode = .DataModelCode
                        oImpRequest.Risks(iCnt).QuoteTimeStamp = .QuoteTimeStamp
                        oImpRequest.Risks(iCnt).RiskDescription = .RiskDescription
                        oImpRequest.Risks(iCnt).RiskTypeCode = .RiskTypeCode
                        oImpRequest.Risks(iCnt).RunDefaultRules = .RunDefaultRules
                        oImpRequest.Risks(iCnt).ScreenCode = .ScreenCode
                        oImpRequest.Risks(iCnt).XMLDataSet = .XMLDataSet
                    End With
                Next
            End If

            ' Call the implementation method
            oImpResponse = oMessagingBusiness.NBTransact(oImpRequest)

            ' Return errors
            SAMFunc.ConvertWCFSTSError(oImpResponse.STSError, oMsgResponse.STSErrorType)

            ' Return details
            If oMsgResponse.STSErrorType Is Nothing Then
                oMsgResponse.Policy = New BaseTransactResponseTypePolicy
                With oImpResponse.Policy
                    oMsgResponse.Policy.PolicyRef = .PolicyRef
                    oMsgResponse.Policy.PremiumDueGross = .PremiumDueGross
                    oMsgResponse.Policy.PremiumDueNet = .PremiumDueNet
                    oMsgResponse.Policy.PremiumDueTax = .PremiumDueTax
                    oMsgResponse.Policy.TotalAnnualTax = .TotalAnnualTax
                    oMsgResponse.Policy.CommissionAmount = .CommissionAmount
                End With
            End If
        Catch SAMError As Sirius.Architecture.ExceptionHandling.SAMErrorException
            ConvertSAMErrorToSTSResponse(oMsgResponse, SAMError)
            Return oMsgResponse

        Catch exError As Exception
            Dim STSErrorFileError As New STSErrorPublisher("An error occured when attempting to Add New Business Transact", exError)
            STSErrorFileError.Raise(HttpContext.Current.Request.Url.ToString(), "NewBusinessTransact", "NewBusinessTransact", True)

        End Try
        Return oMsgResponse

    End Function
#End Region

#Region "GetDatasetSchema"
    ''' <summary>
    ''' this method will get dataset schema on the basis on request supplied from calling application
    ''' </summary>
    ''' <param name="oGetDatasetSchemaRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetDatasetSchema(ByVal oGetDatasetSchemaRequest As GetDatasetSchemaRequestType) As GetDatasetSchemaResponseType Implements IPureMessagingService.GetDatasetSchema

        Dim oMsgResponse As New BaseGetDatasetSchemaResponseType

        Dim oImpRequest As New BaseImplementationTypes.BaseGetDatasetSchemaRequestType
        Dim oImpResponse As New BaseImplementationTypes.BaseGetDatasetSchemaResponseType

        Dim oMessagingBusiness As New CoreSAMBusiness()

        Try
            oImpRequest.BranchCode = oGetDatasetSchemaRequest.BranchCode
            oImpRequest.DataModelCode = oGetDatasetSchemaRequest.DataModelCode

            oImpResponse = oMessagingBusiness.GetDatasetSchema(oImpRequest)

            ' Return errors
            SAMFunc.ConvertWCFSTSError(oImpResponse.STSError, oMsgResponse.STSErrorType)

            ' Return details
            If oMsgResponse.STSErrorType Is Nothing Then
                oMsgResponse.DatasetSchema = oImpResponse.DatasetSchema
            End If

        Catch SAMError As Sirius.Architecture.ExceptionHandling.SAMErrorException
            ConvertSAMErrorToSTSResponse(oMsgResponse, SAMError)
            Return oMsgResponse

        Catch exError As Exception
            Dim STSErrorFileError As New STSErrorPublisher("An error occured when attempting to Get Dataset Schema", exError)
            STSErrorFileError.Raise(HttpContext.Current.Request.Url.ToString(), "GetDatasetSchema", "GetDatasetSchema", True)

        End Try
        Return oMsgResponse

    End Function
#End Region

#Region "ProcessClaim"
    ''' <summary>
    ''' this method will process claim on the basis on request supplied from calling application
    ''' </summary>
    ''' <param name="oClaimProcessRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessClaim(ByVal oClaimProcessRequest As BaseClaimProcessRequestType) As BaseClaimProcessResponseType Implements IPureMessagingService.ProcessClaim

        Dim oMsgResponse As New BaseClaimProcessResponseType

        Dim oImpRequest As New BaseImplementationTypes.BaseClaimProcessRequestType
        Dim oImpResponse As BaseImplementationTypes.BaseClaimProcessResponseType
        Dim oMessagingBusiness As New CoreSAMBusiness()

        Try
            With oClaimProcessRequest
                oImpRequest.BranchCode = .BranchCode
                oImpRequest.Claim = New BaseImplementationTypes.BaseClaimProcessType
                With .Claim
                    oImpRequest.Claim.BaseClaimKey = .BaseClaimKey
                    oImpRequest.ClaimNumber = .ClaimNumber
                    oImpRequest.Claim.CatastropheCode = .CatastropheCode

                    oImpRequest.Claim.ClaimStatus = .ClaimStatus
                    oImpRequest.Claim.ClaimStatusDate = .ClaimStatusDate
                    oImpRequest.Claim.ClaimVersion = .ClaimVersion
                    oImpRequest.Claim.ClaimVersionDescription = .ClaimVersionDescription

                    oImpRequest.Claim.ClientEmail = .ClientEmail
                    oImpRequest.Claim.ClientFaxNo = .ClientFaxNo
                    oImpRequest.Claim.ClientMobileNo = .ClientMobileNo
                    oImpRequest.Claim.ClientName = .ClientName

                    oImpRequest.Claim.ClientTelNo = .ClientTelNo
                    oImpRequest.Claim.Comments = .Comments
                    oImpRequest.Claim.CurrencyCode = .CurrencyCode

                    oImpRequest.Claim.Description = .Description
                    oImpRequest.Claim.ExternalHandler = .ExternalHandler
                    oImpRequest.Claim.HandlerCode = .HandlerCode
                    oImpRequest.Claim.IgnoreWarnings = .IgnoreWarnings
                    oImpRequest.Claim.InfoOnly = .InfoOnly

                    oImpRequest.Claim.InsuranceFileKey = .InsuranceFileKey
                    oImpRequest.Claim.LastModifiedDate = .LastModifiedDate
                    oImpRequest.Claim.LikelyClaim = .LikelyClaim
                    oImpRequest.Claim.LossFromDate = .LossFromDate

                    If Not oImpRequest.Claim.LossToDateSpecified Then
                        oImpRequest.Claim.LossToDate = .LossFromDate
                        oImpRequest.Claim.LossToDateSpecified = True
                    Else
                        oImpRequest.Claim.LossToDate = .LossToDate
                        oImpRequest.Claim.LossToDateSpecified = .LossToDateSpecified
                    End If

                    oImpRequest.Claim.PrimaryCauseCode = .PrimaryCauseCode
                    oImpRequest.Claim.ProgressStatusCode = .ProgressStatusCode
                    oImpRequest.Claim.ReportedDate = .ReportedDate
                    oImpRequest.Claim.RiskKey = .RiskKey
                    oImpRequest.Claim.SecondaryCauseCode = .SecondaryCauseCode

                    If Not .ClaimBuilderDetail Is Nothing Then
                        ReDim oImpRequest.Claim.ClaimBuilderDetail(.ClaimBuilderDetail.Count - 1)
                        For iCount As Int32 = 0 To .ClaimBuilderDetail.Count - 1
                            With .ClaimBuilderDetail(iCount)
                                oImpRequest.Claim.ClaimBuilderDetail(iCount) = New BaseImplementationTypes.BaseClaimProcessBuilderRiskType
                                oImpRequest.Claim.ClaimBuilderDetail(iCount).ClaimBuilderData = New BaseImplementationTypes.BaseClaimProcessBuilderRiskTypeClaimBuilderData
                                oImpRequest.Claim.ClaimBuilderDetail(iCount).ClaimBuilderData.ItemName = .ClaimBuilderData.ItemName
                                oImpRequest.Claim.ClaimBuilderDetail(iCount).ClaimBuilderData.Value = .ClaimBuilderData.Value
                            End With
                        Next
                    End If
                    If Not .ClaimPeril Is Nothing Then
                        ReDim oImpRequest.Claim.ClaimPeril(.ClaimPeril.Count - 1)
                        For iCount As Int32 = 0 To .ClaimPeril.Count - 1
                            oImpRequest.Claim.ClaimPeril(iCount) = New BaseImplementationTypes.BaseClaimProcessPerilType
                            With .ClaimPeril(iCount)
                                If .Description = String.Empty Then
                                    oImpRequest.Claim.ClaimPeril(iCount).Description = .TypeCode
                                Else
                                    oImpRequest.Claim.ClaimPeril(iCount).Description = .Description
                                End If
                                If Not .Recovery Is Nothing Then
                                    ReDim oImpRequest.Claim.ClaimPeril(iCount).Recovery(.Recovery.Count - 1)
                                    For iRCount As Int32 = 0 To .Recovery.Count - 1
                                        With .Recovery(iRCount)
                                            oImpRequest.Claim.ClaimPeril(iCount).Recovery(iRCount) = New BaseImplementationTypes.BaseClaimProcessPerilRecoveryType
                                            oImpRequest.Claim.ClaimPeril(iCount).Recovery(iRCount).Amount = .Amount
                                            oImpRequest.Claim.ClaimPeril(iCount).Recovery(iRCount).RecoveryPartyCode = .RecoveryPartyCode
                                            oImpRequest.Claim.ClaimPeril(iCount).Recovery(iRCount).RecoveryPartyTypeCode = .RecoveryPartyTypeCode
                                            oImpRequest.Claim.ClaimPeril(iCount).Recovery(iRCount).TypeCode = .TypeCode
                                            If Not .RecoveryDetails Is Nothing Then
                                                With .RecoveryDetails
                                                    oImpRequest.Claim.ClaimPeril(iCount).Recovery(iRCount).RecoveryDetails = New BaseImplementationTypes.BaseClaimProcessReceiptDetailsType
                                                    oImpRequest.Claim.ClaimPeril(iCount).Recovery(iRCount).RecoveryDetails.ReceiptBankCode = .ReceiptBankCode
                                                    ' If no MediaType passed in then default to Cheque
                                                    If .ReceiptMediaTypeCode = String.Empty Then
                                                        oImpRequest.Claim.ClaimPeril(iCount).Recovery(iRCount).RecoveryDetails.ReceiptMediaTypeCode = "CQ"
                                                    Else
                                                        oImpRequest.Claim.ClaimPeril(iCount).Recovery(iRCount).RecoveryDetails.ReceiptMediaTypeCode = .ReceiptMediaTypeCode
                                                    End If
                                                    oImpRequest.Claim.ClaimPeril(iCount).Recovery(iRCount).RecoveryDetails.ReceiptMediaReference = .ReceiptMediaReference
                                                    oImpRequest.Claim.ClaimPeril(iCount).Recovery(iRCount).RecoveryDetails.ReceiptPayee = .ReceiptPayee
                                                End With
                                            End If
                                            oImpRequest.Claim.ClaimPeril(iCount).Recovery(iRCount).IsSalvageRecovery = .IsSalvageRecovery
                                            oImpRequest.Claim.ClaimPeril(iCount).Recovery(iRCount).taxGroupCode = .taxGroupCode
                                            oImpRequest.Claim.ClaimPeril(iCount).Recovery(iRCount).TypeCode = .TypeCode
                                            oImpRequest.Claim.ClaimPeril(iCount).Recovery(iRCount).RecoveryAmount = .RecoveryAmount
                                            oImpRequest.Claim.ClaimPeril(iCount).Recovery(iRCount).RecoveryAmountSpecified = .RecoveryAmountSpecified
                                        End With
                                    Next
                                End If
                                If Not .Reserve Is Nothing Then
                                    ReDim oImpRequest.Claim.ClaimPeril(iCount).Reserve(.Reserve.Count - 1)
                                    For iRSCount As Int32 = 0 To .Reserve.Count - 1
                                        With .Reserve(iRSCount)
                                            oImpRequest.Claim.ClaimPeril(iCount).Reserve(iRSCount) = New BaseImplementationTypes.BaseClaimProcessPerilReserveType
                                            oImpRequest.Claim.ClaimPeril(iCount).Reserve(iRSCount).Amount = .Amount
                                            oImpRequest.Claim.ClaimPeril(iCount).Reserve(iRSCount).PaymentAmount = .PaymentAmount
                                            oImpRequest.Claim.ClaimPeril(iCount).Reserve(iRSCount).PaymentAmountSpecified = .PaymentAmountSpecified
                                            If Not .PaymentDetails Is Nothing Then
                                                With .PaymentDetails
                                                    oImpRequest.Claim.ClaimPeril(iCount).Reserve(iRSCount).PaymentDetails = New BaseImplementationTypes.BaseClaimProcessPaymentDetailsType
                                                    oImpRequest.Claim.ClaimPeril(iCount).Reserve(iRSCount).PaymentDetails.PaymentBankCode = .PaymentBankCode
                                                    ' If no MediaType passed in then default to Cheque
                                                    If .PaymentMediaTypeCode = String.Empty Then
                                                        oImpRequest.Claim.ClaimPeril(iCount).Reserve(iRSCount).PaymentDetails.PaymentMediaTypeCode = "CQ"
                                                    Else
                                                        oImpRequest.Claim.ClaimPeril(iCount).Reserve(iRSCount).PaymentDetails.PaymentMediaTypeCode = .PaymentMediaTypeCode
                                                    End If
                                                    oImpRequest.Claim.ClaimPeril(iCount).Reserve(iRSCount).PaymentDetails.PaymentMediaReference = .PaymentMediaReference
                                                    oImpRequest.Claim.ClaimPeril(iCount).Reserve(iRSCount).PaymentDetails.PaymentPayee = .PaymentPayee
                                                    If Not String.IsNullOrEmpty(.UltimatePayee) Then
                                                        oImpRequest.Claim.ClaimPeril(iCount).Reserve(iRSCount).PaymentDetails.UltimatePayee = .UltimatePayee
                                                    End If

                                                End With
                                            End If
                                            oImpRequest.Claim.ClaimPeril(iCount).Reserve(iRSCount).ReverseExcess = .ReverseExcess
                                            oImpRequest.Claim.ClaimPeril(iCount).Reserve(iRSCount).TaxGroupCode = .TaxGroupCode
                                            oImpRequest.Claim.ClaimPeril(iCount).Reserve(iRSCount).TypeCode = .TypeCode
                                        End With
                                    Next
                                End If
                                oImpRequest.Claim.ClaimPeril(iCount).TypeCode = .TypeCode
                            End With
                        Next
                    End If
                End With
            End With
            'Calling base function
            oImpResponse = oMessagingBusiness.ProcessClaim(oImpRequest)

            If oImpResponse.Errors IsNot Nothing Then
                If (oImpResponse.Errors.Length > 0) Then
                    oMsgResponse.STSErrorType = New STSErrorType
                    For iCount As Int32 = 0 To oImpResponse.Errors.Length - 1
                        If TypeOf (oImpResponse.Errors(iCount)) Is BaseImplementationTypes.SAMErrorInvalidData Then
                            Dim SAMErr As BaseImplementationTypes.SAMErrorInvalidData = CType(oImpResponse.Errors(iCount), BaseImplementationTypes.SAMErrorInvalidData)
                            Dim STSErr As New STSErrorInvalidDataType
                            With SAMErr
                                STSErr.Code = .Code.ToString
                                STSErr.Description = .Description
                                STSErr.FieldName = .FieldName
                                STSErr.SuppliedValue = .SuppliedValue
                            End With
                            oMsgResponse.STSErrorType.InvalidData.Add(STSErr)
                        ElseIf TypeOf (oImpResponse.Errors(iCount)) Is BaseImplementationTypes.SAMErrorBusinessRule Then
                            Dim SAMErr As BaseImplementationTypes.SAMErrorBusinessRule = CType(oImpResponse.Errors(iCount), BaseImplementationTypes.SAMErrorBusinessRule)
                            Dim STSErr As New STSErrorSTSBusinessRuleType
                            With SAMErr
                                STSErr.Code = .Code.ToString
                                STSErr.Description = .Description
                                STSErr.Detail = .Detail
                            End With
                            oMsgResponse.STSErrorType.STSBusinessRule = STSErr
                        ElseIf TypeOf (oImpResponse.Errors(iCount)) Is BaseImplementationTypes.SAMErrorFatal Then
                            Dim SAMErr As BaseImplementationTypes.SAMErrorFatal = CType(oImpResponse.Errors(iCount), BaseImplementationTypes.SAMErrorFatal)
                            Dim STSErr As New STSErrorInternalExceptionType
                            STSErr.Description = SAMErr.Type
                            oMsgResponse.STSErrorType.InternalException = STSErr
                        End If
                    Next
                End If
            End If

            ' Return details
            If oMsgResponse.STSErrorType Is Nothing Then
                oMsgResponse.BaseClaimKey = oImpResponse.BaseClaimKey
                oMsgResponse.ClaimKey = oImpResponse.ClaimKey
                oMsgResponse.ClaimNumber = oImpResponse.ClaimNumber
                If Not oImpResponse.Warnings Is Nothing Then
                    oMsgResponse.Warnings.Capacity = oImpResponse.Warnings.Length
                    For iCount As Int32 = 0 To oImpResponse.Warnings.Length - 1
                        oMsgResponse.Warnings(iCount) = New BaseClaimProcessResponseTypeWarnings
                        oMsgResponse.Warnings(iCount).Code = oImpResponse.Warnings(iCount).Code
                        oMsgResponse.Warnings(iCount).Description = oImpResponse.Warnings(iCount).Description
                    Next
                End If
                oMsgResponse.ResultingStatus = oImpResponse.ResultingStatus
                oMsgResponse.TimeStamp = oImpResponse.TimeStamp
                oMsgResponse.Version = oImpResponse.Version
            End If

            Return oMsgResponse

        Catch SAMException As Sirius.Architecture.ExceptionHandling.SAMErrorException
            If SAMException.Errors IsNot Nothing Then
                If (SAMException.Errors.Count > 0) Then
                    oMsgResponse.STSErrorType = New STSErrorType
                    For iCount As Int32 = 0 To SAMException.Errors.Count - 1
                        If TypeOf (SAMException.Errors(iCount)) Is Sirius.Architecture.ExceptionHandling.SAMErrorInvalidData Then
                            Dim SAMErr As Sirius.Architecture.ExceptionHandling.SAMErrorInvalidData = CType(SAMException.Errors(iCount), Sirius.Architecture.ExceptionHandling.SAMErrorInvalidData)
                            Dim STSErr As New STSErrorInvalidDataType
                            With SAMErr
                                STSErr.Code = .Code.ToString
                                STSErr.Description = .Description
                                STSErr.FieldName = .FieldName
                                STSErr.SuppliedValue = .SuppliedValue
                            End With
                            oMsgResponse.STSErrorType.InvalidData.Add(STSErr)
                        ElseIf TypeOf (SAMException.Errors(iCount)) Is Sirius.Architecture.ExceptionHandling.SAMErrorBusinessRule Then
                            Dim SAMErr As Sirius.Architecture.ExceptionHandling.SAMErrorBusinessRule = CType(SAMException.Errors(iCount), Sirius.Architecture.ExceptionHandling.SAMErrorBusinessRule)
                            Dim STSErr As New STSErrorSTSBusinessRuleType
                            With SAMErr
                                STSErr.Code = .Code.ToString
                                STSErr.Description = .Description
                                STSErr.Detail = .Detail
                            End With
                            oMsgResponse.STSErrorType.STSBusinessRule = STSErr
                        ElseIf TypeOf (SAMException.Errors(iCount)) Is Sirius.Architecture.ExceptionHandling.SAMErrorFatal Then
                            Dim SAMErr As Sirius.Architecture.ExceptionHandling.SAMErrorFatal = CType(SAMException.Errors(iCount), Sirius.Architecture.ExceptionHandling.SAMErrorFatal)
                            Dim STSErr As New STSErrorInternalExceptionType
                            STSErr.Description = SAMErr.Type
                            oMsgResponse.STSErrorType.InternalException = STSErr
                        End If
                    Next
                End If
            End If
            Return oMsgResponse
        Catch exError As Exception
            Dim STSErr As New STSErrorInternalExceptionType
            STSErr.Description = exError.Message
            oMsgResponse.STSErrorType = New STSErrorType
            oMsgResponse.STSErrorType.InternalException = STSErr
            Return oMsgResponse
        End Try

    End Function
#End Region

#Region "Common method to convert SAM Error to STS Error"
    ''' <summary>
    ''' this method will Convert SAMError To STS Response
    ''' </summary>
    ''' <param name="oMsgResponse"></param>
    ''' <param name="SAMError"></param>
    ''' <remarks></remarks>
    Private Shared Sub ConvertSAMErrorToSTSResponse(ByVal oMsgResponse As BaseResponseType, ByVal SAMError As Sirius.Architecture.ExceptionHandling.SAMErrorException)
        If SAMError.Errors IsNot Nothing Then
            If (SAMError.Errors.Count > 0) Then
                oMsgResponse.STSErrorType = New STSErrorType
                For iCount As Int32 = 0 To SAMError.Errors.Count - 1
                    If TypeOf (SAMError.Errors(iCount)) Is SAMErrorInvalidData Then
                        Dim SAMErr As SAMErrorInvalidData = CType(SAMError.Errors(iCount), SAMErrorInvalidData)
                        Dim STSErr As New STSErrorInvalidDataType
                        With SAMErr
                            STSErr.Code = .Code.ToString
                            STSErr.Description = .Description
                            STSErr.FieldName = .FieldName
                            STSErr.SuppliedValue = .SuppliedValue
                        End With
                        If oMsgResponse.STSErrorType.InvalidData Is Nothing Then
                            Dim oInvalidData As New List(Of STSErrorInvalidDataType)
                            oMsgResponse.STSErrorType.InvalidData = oInvalidData
                            oMsgResponse.STSErrorType.InvalidData.Add(STSErr)
                        Else
                            oMsgResponse.STSErrorType.InvalidData.Add(STSErr)
                        End If
                    ElseIf TypeOf (SAMError.Errors(iCount)) Is SAMErrorBusinessRule Then
                        Dim SAMErr As SAMErrorBusinessRule = CType(SAMError.Errors(iCount), SAMErrorBusinessRule)
                        Dim STSErr As New STSErrorSTSBusinessRuleType
                        With SAMErr
                            STSErr.Code = .Code.ToString
                            STSErr.Description = .Description
                            STSErr.Detail = .Detail
                        End With
                        oMsgResponse.STSErrorType.STSBusinessRule = STSErr
                    ElseIf TypeOf (SAMError.Errors(iCount)) Is SAMErrorFatal Then
                        Dim SAMErr As SAMErrorFatal = CType(SAMError.Errors(iCount), SAMErrorFatal)
                        Dim STSErr As New STSErrorInternalExceptionType
                        STSErr.Description = SAMErr.Type
                        oMsgResponse.STSErrorType.InternalException = STSErr
                    End If
                Next
            End If
        End If
    End Sub
#End Region

#Region "Common method to convert SAM Error to STS Error"
    ''' <summary>
    ''' this method will process policy on the basis on request supplied from calling application
    ''' </summary>
    ''' <param name="PolicyProcessRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PolicyProcess(ByVal PolicyProcessRequest As PolicyProcessRequestType) As PolicyProcessResponseType Implements IPureMessagingService.PolicyProcess

        Dim iLBnd As Integer
        Dim iUBnd As Integer

        Dim oMsgResponse As New PolicyProcessResponseType
        Dim oMessagingBusiness As New CoreSAMBusiness()

        ' Implementation structures
        Dim oImpRequest As New MessagingImplementationTypes.PolicyProcessRequestType
        Dim oImpResponse As MessagingImplementationTypes.PolicyProcessResponseType = Nothing

        Try

            ' Convert the incoming interface structures into the implementation structures
            With PolicyProcessRequest
                oImpRequest.AgentCode = .AgentCode
                oImpRequest.BranchCode = .BranchCode
                oImpRequest.CurrencyCode = CType([Enum].ToObject(GetType(CurrencyType), .CurrencyCode), BaseImplementationTypes.CurrencyType)
                oImpRequest.CurrencyCodeSpecified = True
                oImpRequest.UpdateParty = .UpdateParty
                oImpRequest.ClientID = .ClientID
                oImpRequest.ClientCodeSpecified = .ClientCodeSpecified

                ' Process the Party structure.  We 1st need to check the party type of the incoming message
                If .Item IsNot Nothing Then

                    Dim oImpParty As New BaseImplementationTypes.BasePartyType
                    Dim oImpPartySharedClientDetail As New BaseImplementationTypes.BaseClientSharedDataType

                    ' Check the type of the party object to see if it is Personal or Corporate
                    If .Item.GetType Is GetType(BasePartyPCType) Then

                        ' Process personal client
                        Dim oMsgParty As BasePartyPCType = DirectCast(.Item, BasePartyPCType)
                        Dim oImpPartyPC As New BaseImplementationTypes.BasePartyPCType
                        With oMsgParty
                            oImpPartyPC.Forename = .Forename
                            oImpPartyPC.Surname = .Surname
                            oImpPartyPC.Initials = .Initials
                            oImpPartyPC.Title = .Title
                            oImpPartyPC.DateOfBirth = .DateOfBirth
                            oImpPartyPC.GenderCode = .GenderCode
                            oImpPartyPC.MaritalStatusCode = CType([Enum].ToObject(GetType(MaritalStatusCodeType), .MaritalStatusCode), BaseImplementationTypes.MaritalStatusCodeType)
                            oImpPartyPC.OccupationCode = .OccupationCode
                            oImpPartyPC.EmploymentStatusCode = CType([Enum].ToObject(GetType(EmploymentStatusCodeType), .EmploymentStatusCode), BaseImplementationTypes.EmploymentStatusCodeType)
                            oImpPartyPC.EmployersBusinessCode = .EmployersBusinessCode
                            oImpPartyPC.AlternativeId = .AlternativeId
                            oImpPartyPC.Currency = .Currency
                            oImpPartyPC.FileCode = .FileCode
                            oImpPartyPC.ClientDetail = oImpPartySharedClientDetail
                            oImpPartyPC.MaritalStatusCodeSpecified = .MaritalStatusCodeSpecified
                            oImpPartyPC.NationalityCode = .NationalityCode
                            oImpParty = oImpPartyPC
                        End With
                    Else

                        ' Process corporate client
                        Dim oMsgParty As BasePartyCCType = DirectCast(.Item, BasePartyCCType)
                        Dim oImpPartyCC As New BaseImplementationTypes.BasePartyCCType
                        With oMsgParty
                            oImpPartyCC.CompanyName = .CompanyName
                            oImpPartyCC.MainContact = .MainContact
                            oImpPartyCC.BusinessCode = .BusinessCode
                            oImpPartyCC.FileCode = .FileCode
                            oImpPartyCC.Currency = .Currency
                            oImpPartyCC.ClientDetail = oImpPartySharedClientDetail
                        End With
                        oImpParty = oImpPartyCC

                    End If

                    ' Common to PC and CC
                    oImpParty.BranchCode = .Item.BranchCode
                    oImpParty.TPUserCode = .Item.TPUserCode
                    oImpParty.TPIntroducer = .Item.TPIntroducer

                    ' Process the address structure
                    If .Item.Addresses IsNot Nothing Then
                        Dim oMsgAddress As BaseAddressType
                        Dim oImpAddress As New BaseImplementationTypes.BaseAddressType

                        iUBnd% = .Item.Addresses.Count - 1

                        ReDim oImpParty.Addresses(iUBnd%)

                        For iCnt As Integer = 0 To iUBnd%
                            oMsgAddress = .Item.Addresses(iCnt%)
                            oImpAddress = New BaseImplementationTypes.BaseAddressType
                            With oMsgAddress
                                oImpAddress.AddressLine1 = .AddressLine1
                                oImpAddress.AddressLine2 = .AddressLine2
                                oImpAddress.AddressLine3 = .AddressLine3
                                oImpAddress.AddressLine4 = .AddressLine4
                                oImpAddress.AddressTypeCode = CType([Enum].ToObject(GetType(AddressTypeType), .AddressTypeCode), BaseImplementationTypes.AddressTypeType)
                                oImpAddress.CountryCode = .CountryCode
                                oImpAddress.PostCode = .PostCode
                            End With
                            oImpParty.Addresses(iCnt%) = oImpAddress
                        Next iCnt%

                    End If

                    ' Process the Contact structure

                    If .Item.Contacts IsNot Nothing Then
                        Dim oImpContact As New BaseImplementationTypes.BaseContactType
                        Dim oMsgContact As BaseContactType

                        iUBnd% = .Item.Contacts.Count - 1

                        ReDim oImpParty.Contacts(iUBnd%)

                        For iCnt As Integer = iLBnd% To iUBnd%
                            oMsgContact = .Item.Contacts(iCnt%)
                            oImpContact = New BaseImplementationTypes.BaseContactType
                            With oMsgContact
                                oImpContact.AreaCode = .AreaCode
                                oImpContact.ContactDetail = New BaseImplementationTypes.BaseContactDetailType
                                oImpContact.ContactDetail.Item = .ContactDetail.Item
                                oImpContact.ContactDetail.ItemElementName = CType([Enum].ToObject(GetType(ItemChoiceType), .ContactDetail.ItemElementName), BaseImplementationTypes.ItemChoiceType)
                                oImpContact.ContactTypeCode = CType([Enum].ToObject(GetType(ContactTypeType), .ContactTypeCode), BaseImplementationTypes.ContactTypeType)
                            End With
                            oImpParty.Contacts(iCnt%) = oImpContact
                        Next iCnt%

                    End If

                    oImpRequest.Party = oImpParty

                End If

                If .Policy IsNot Nothing Then
                    With .Policy
                        ' Process the Policy Structure
                        oImpRequest.Policy = New BaseImplementationTypes.BaseQuoteRiskMsgType

                        oImpRequest.Policy.BranchCode = .BranchCode
                        oImpRequest.Policy.CoverStartDate = .CoverStartDate
                        oImpRequest.Policy.CoverEndDate = .CoverEndDate
                        oImpRequest.Policy.Description = .Description
                        oImpRequest.Policy.InsuredName = .InsuredName
                        oImpRequest.Policy.ProductCode = .ProductCode
                        oImpRequest.Policy.QuoteRef = .QuoteRef
                        oImpRequest.Policy.CurrencyCode = .CurrencyCode
                        oImpRequest.Policy.PolicyStatusCode = .PolicyStatusCode
                        oImpRequest.Policy.TransactionTypeCode = .TransactionTypeCode
                        oImpRequest.Policy.AlternateReference = .AlternateReference
                        oImpRequest.Policy.AlternativeRef = .AlternateReference
                        oImpRequest.Policy.NewQuoteRef = .NewQuoteRef
                        oImpRequest.Policy.CommissionRate = .CommissionRate
                        oImpRequest.Policy.CommissionValue = .CommissionValue
                        oImpRequest.Policy.TransactionDueDate = .TransactionDueDate
                        oImpRequest.Policy.OldPolicyNumber = .OldPolicyNumber
                        oImpRequest.Policy.LastTransDescription = .LastTransDescription
                        oImpRequest.Policy.AnalysisCode = .AnalysisCode
                        oImpRequest.Policy.DoNotCopyRiskAtRenSelection = .DoNotCopyRiskAtRenSelection
                        oImpRequest.Policy.DeletePolicyUnderRenewal = .DeletePolicyUnderRenewal
                        oImpRequest.Policy.PolicyProcessType = .PolicyProcessType

                        oImpRequest.Policy.CoInsurancePlacement = PolicyProcessRequest.Policy.CoInsurancePlacement
                        oImpRequest.Policy.UnderwritingYearCode = PolicyProcessRequest.Policy.UnderwritingYearCode
                        oImpRequest.Policy.SkipNumberingScheme = .SkipNumberingScheme
                        oImpRequest.Policy.PaymentTermCode = .PaymentTermCode
                        oImpRequest.Policy.CollectionFrequencyCode = .CollectionFrequencyCode
                        If Not String.IsNullOrEmpty(PolicyProcessRequest.AgentCode) Then
                            If Not String.IsNullOrEmpty(Cast.ToString(.BusinessTypeCode, "")) AndAlso
                               (.BusinessTypeCode.Trim.ToUpper = "COIN FOLL" OrElse
                               .BusinessTypeCode.Trim.ToUpper = "COIN LEAD") Then
                                oImpRequest.Policy.BusinessTypeCode = .BusinessTypeCode
                            Else
                                oImpRequest.Policy.BusinessTypeCode = "AGENCY"
                            End If
                        Else
                            oImpRequest.Policy.BusinessTypeCode = "DIRECT"
                        End If

                        If (.MTAReasonCode Is Nothing) Then
                            oImpRequest.Policy.MTAReasonCode = "OTHER"
                        Else
                            oImpRequest.Policy.MTAReasonCode = .MTAReasonCode
                        End If
                        If .Taxes IsNot Nothing Then
                            oImpRequest.Policy.Taxes = Array.ConvertAll(.Taxes.ToArray(),
                                                New Converter(Of BaseTaxesType, BaseImplementationTypes.BaseTaxesType) _
                                                (AddressOf ToBaseImpBaseTaxesAndFeesRiskType))
                        End If
                        ' Process the Risks structure
                        If .Risks IsNot Nothing AndAlso .Risks.Count >= 1 Then
                            oImpRequest.Policy.Risks = Array.ConvertAll(.Risks.ToArray(),
                                        New Converter(Of BaseRiskType, BaseImplementationTypes.BaseQuoteRiskMsgTypeRisks) _
                                        (AddressOf ToBaseImpBaseQuoteRiskMsgTypeRisks))

                        End If
                        oImpRequest.Policy.CoInsurancePlacement = PolicyProcessRequest.Policy.CoInsurancePlacement
                        oImpRequest.Policy.PartyKey = PolicyProcessRequest.Policy.PartyKey
                        ' Copy Messaging Coinsurer type to CoreSSAm Coinsurer type
                        If .CoInsurers IsNot Nothing AndAlso .CoInsurers.Count <> 0 Then
                            oImpRequest.Policy.CoInsurers = New BaseImplementationTypes.BaseUpdateCoinsuranceValuesRequestTypeCoInsurers
                            ReDim oImpRequest.Policy.CoInsurers.Row(.CoInsurers.Count - 1)
                            Dim iRowIndex As Integer = 0
                            For Each oCoInsurer As BaseCoInsurerType In .CoInsurers
                                Dim oImpRow As New BaseImplementationTypes.BaseUpdateCoinsuranceValuesRequestTypeCoInsurersRow
                                oImpRow.CoInsurerKey = oCoInsurer.InsurerKey
                                oImpRow.ArrangementRef = oCoInsurer.ArrangementRef
                                oImpRow.SharePerc = oCoInsurer.SharePerc
                                oImpRow.CommissionPerc = oCoInsurer.CommPerc
                                oImpRequest.Policy.CoInsurers.Row(iRowIndex) = oImpRow
                                iRowIndex = iRowIndex + 1
                            Next
                        End If
                        oImpRequest.Policy.IsBDXRequest = .IsBDXRequest
                    End With
                End If
            End With
            oImpResponse = oMessagingBusiness.PolicyProcess(oImpRequest)

            ' Return errors
            SAMFunc.ConvertWCFSTSError(oImpResponse.STSError, oMsgResponse.STSErrorType)

            ' Return details
            If (oMsgResponse.STSErrorType Is Nothing) And IsArray(oImpResponse.Policy) Then
                oMsgResponse.Insured = New BaseAddPartyResponseType
                oMsgResponse.Policy = New BaseNBQuoteResponseTypePolicy
                With oImpResponse
                    oMsgResponse.Insured.PartyKey = .Insured.PartyKey
                    oMsgResponse.Insured.Shortname = .Insured.Shortname
                    With oImpResponse.Policy(0)
                        oMsgResponse.Policy.PolicyID = .PolicyID
                        oMsgResponse.Policy.PremiumDueGross = .PremiumDueGross
                        oMsgResponse.Policy.PremiumDueNet = .PremiumDueNet
                        oMsgResponse.Policy.PremiumDueTax = .PremiumDueTax
                        oMsgResponse.Policy.QuoteRef = .QuoteRef
                        oMsgResponse.Policy.TotalAnnualTax = .TotalAnnualTax

                        ' Process the Risks structure
                        If .Risks IsNot Nothing AndAlso .Risks.Count >= 1 Then
                            oMsgResponse.Policy.Risks = .Risks.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseRiskResultType, BaseRiskResultType)(AddressOf ToServiceBaseRiskResultType))
                        End If
                    End With
                End With
            End If

        Catch SAMError As Sirius.Architecture.ExceptionHandling.SAMErrorException
            ConvertSAMErrorToSTSResponse(oMsgResponse, SAMError)
            Return oMsgResponse

        Catch exError As Exception
            Dim STSErrorFileError As New STSErrorPublisher("An error occured when attempting UpdateRisk", exError)
            STSErrorFileError.Raise(HttpContext.Current.Request.Url.ToString(), "PolicyProcess", "PolicyProcess", True)

        End Try
        Return oMsgResponse
    End Function

    ''' <summary>
    ''' this will convert base risk result into service type
    ''' </summary>
    ''' <param name="oImpRisk"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ToServiceBaseRiskResultType(ByVal oImpRisk As BaseImplementationTypes.BaseRiskResultType) As BaseRiskResultType

        Dim oMsgRisk As New BaseRiskResultType

        If oImpRisk IsNot Nothing Then
            With oImpRisk
                oMsgRisk.PremiumDueGross = .PremiumDueGross
                oMsgRisk.PremiumDueNet = .PremiumDueNet
                oMsgRisk.PremiumDueTax = .PremiumDueTax
                oMsgRisk.TotalAnnualTax = .TotalAnnualTax
                oMsgRisk.CommissionAmount = .CommissionAmount
                oMsgRisk.RiskFolderID = .RiskFolderID
                oMsgRisk.RiskID = .RiskID
                oMsgRisk.XMLDataSet = SAMFunc.TransformDatasetPBtoSAM(.XMLDataSet)
            End With
        End If

        Return oMsgRisk

    End Function

    ''' <summary>
    ''' This will convert input request parameters into internal type
    ''' </summary>
    ''' <param name="oMsgRisk"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ToBaseImpBaseQuoteRiskMsgTypeRisks(ByVal oMsgRisk As BaseRiskType) As BaseImplementationTypes.BaseQuoteRiskMsgTypeRisks

        Dim oImpRisk As New BaseImplementationTypes.BaseQuoteRiskMsgTypeRisks

        If oMsgRisk IsNot Nothing Then
            With oMsgRisk
                oImpRisk.DataModelCode = .DataModelCode
                oImpRisk.QuoteTimeStamp = .QuoteTimeStamp
                oImpRisk.RiskDescription = .RiskDescription
                oImpRisk.RiskTypeCode = .RiskTypeCode
                oImpRisk.RunDefaultRules = .RunDefaultRules
                oImpRisk.ScreenCode = .ScreenCode
                oImpRisk.RiskFolderKey = .RiskFolderKey
                oImpRisk.RiskFolderKeySpecified = .RiskFolderKeySpecified
                oImpRisk.OriginalRiskKey = .OriginalRiskKey
                oImpRisk.OriginalRiskKeySpecified = .OriginalRiskKeySpecified

                oImpRisk.XMLDataSet = .XMLDataSet
                ' Process the Product Builder structure
                If .ProductBuilderDetail IsNot Nothing AndAlso .ProductBuilderDetail.Count >= 1 Then
                    oImpRisk.ProductBuilderDetail = Array.ConvertAll(.ProductBuilderDetail.ToArray(),
                                New Converter(Of BaseProductBuilderRiskType, BaseImplementationTypes.BaseProductBuilderRiskType) _
                                (AddressOf ToBaseImpBaseProductBuilderRiskType))

                End If

                ' Process the Tax structure
                If .Taxes IsNot Nothing AndAlso .Taxes.Count >= 1 Then
                    ReDim oImpRisk.TaxesAndFees(0)

                    oImpRisk.TaxesAndFees(0) = New BaseImplementationTypes.BaseTaxesAndFeesType
                    oImpRisk.TaxesAndFees(0).Taxes = Array.ConvertAll(.Taxes.ToArray(),
                                New Converter(Of BaseTaxesType, BaseImplementationTypes.BaseTaxesType) _
                                (AddressOf ToBaseImpBaseTaxesAndFeesRiskType))

                End If
            End With
        End If

        Return oImpRisk

    End Function

    ''' <summary>
    ''' this will convert input request parameters into internal type
    ''' </summary>
    ''' <param name="oMsgStruct"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ToBaseImpBaseProductBuilderRiskType(ByVal oMsgStruct As BaseProductBuilderRiskType) As BaseImplementationTypes.BaseProductBuilderRiskType

        Dim oImpStruct As New BaseImplementationTypes.BaseProductBuilderRiskType

        If oMsgStruct IsNot Nothing AndAlso oMsgStruct.ProductBuilderData IsNot Nothing Then
            With oMsgStruct
                oImpStruct.ProductBuilderData = New BaseImplementationTypes.BaseProductBuilderRiskTypeProductBuilderData
                oImpStruct.ProductBuilderData.ItemName = .ProductBuilderData.ItemName
                oImpStruct.ProductBuilderData.Value = .ProductBuilderData.Value
            End With
        End If

        Return oImpStruct

    End Function

    ''' <summary>
    ''' this will convert input request parameters into internal type
    ''' </summary>
    ''' <param name="oMsgStruct"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ToBaseImpBaseTaxesAndFeesRiskType(ByVal oMsgStruct As BaseTaxesType) As BaseImplementationTypes.BaseTaxesType

        Dim oImpStruct As New BaseImplementationTypes.BaseTaxesType

        If oMsgStruct IsNot Nothing Then
            With oMsgStruct
                oImpStruct = New BaseImplementationTypes.BaseTaxesType
                oImpStruct.Amount = .Amount
                oImpStruct.TaxRate = .TaxRate
                oImpStruct.Description = .Description
                oImpStruct.TaxBandCode = .TaxBandCode
            End With
        End If

        Return oImpStruct

    End Function

    ''' <summary>
    ''' this method will process policy on the basis on request supplied from calling application
    ''' </summary>
    ''' <param name="PolicyProcessRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PolicyProcessV2(ByVal PolicyProcessRequest As PolicyProcessV2RequestType) As PolicyProcessV2ResponseType Implements IPureMessagingService.PolicyProcessV2

        Dim iLBnd As Integer
        Dim iUBnd As Integer

        Dim oMsgResponse As New PolicyProcessResponseType
        Dim oMsgResponseV2 As New PolicyProcessV2ResponseType
        Dim oMessagingBusiness As New CoreSAMBusiness()

        ' Implementation structures
        Dim oImpRequest As New MessagingImplementationTypes.PolicyProcessRequestType
        Dim oImpResponse As MessagingImplementationTypes.PolicyProcessResponseType = Nothing

        Try
            If PolicyProcessRequest.Policy.PolicyProcessType = BaseImplementationTypes.PolicyProcessTypes.GetPolicy Then

                If Not String.IsNullOrEmpty(PolicyProcessRequest.Policy.QuoteRef) Then
                    Dim getPolicyRequest As New SAMForInsuranceV2ImplementationTypes.GetHeaderAndSummariesByRefRequestType
                    Dim getPolicyReponse As New SAMForInsuranceV2ImplementationTypes.GetHeaderAndSummariesByRefResponseType

                    If PolicyProcessRequest.Policy.QuoteRef IsNot Nothing AndAlso Not String.IsNullOrEmpty(PolicyProcessRequest.Policy.QuoteRef) Then
                        getPolicyRequest.BranchCode = PolicyProcessRequest.BranchCode
                        getPolicyRequest.InsuranceRef = PolicyProcessRequest.Policy.QuoteRef
                        getPolicyRequest.SkipPolicyLevelTaxesRecalculation = True
                        getPolicyReponse = DirectCast(oMessagingBusiness.GetHeaderAndSummariesByRef(getPolicyRequest), SAMForInsuranceV2ImplementationTypes.GetHeaderAndSummariesByRefResponseType)
                        If getPolicyReponse.InsuranceFileKey <> 0 Then
                            PopulatePolicy(getPolicyReponse.InsuranceFileKey, PolicyProcessRequest.BranchCode, oMsgResponseV2)
                            If getPolicyReponse.InsuredParties IsNot Nothing AndAlso getPolicyReponse.InsuredParties.SelectSingleNode("//Row/shortname").InnerXml IsNot Nothing AndAlso Not String.IsNullOrEmpty(getPolicyReponse.InsuredParties.SelectSingleNode("//Row/shortname").InnerXml) Then
                                PopulateInsured(getPolicyReponse.InsuredParties.SelectSingleNode("//Row/shortname").InnerXml, PolicyProcessRequest.BranchCode, oMsgResponseV2)                            
                            End If
                        End If
                    End If
                    Return oMsgResponseV2
                End If
            End If
            ' Convert the incoming interface structures into the implementation structures
            With PolicyProcessRequest
                oImpRequest.AgentCode = .AgentCode
                oImpRequest.BranchCode = .BranchCode
                oImpRequest.CurrencyCode = CType([Enum].ToObject(GetType(CurrencyType), .CurrencyCode), BaseImplementationTypes.CurrencyType)
                oImpRequest.CurrencyCodeSpecified = True
                oImpRequest.UpdateParty = .UpdateParty

                ' Process the Party structure.  We 1st need to check the party type of the incoming message
                If .PersonalClient IsNot Nothing Then

                    Dim oImpParty As New BaseImplementationTypes.BasePartyPCType


                    ' Process personal client
                    Dim oMsgParty As BasePartyPCType = DirectCast(.PersonalClient, BasePartyPCType)
                    Dim oImpPartyPC As New BaseImplementationTypes.BasePartyPCType
                    With oMsgParty
                        oImpPartyPC.Forename = .Forename
                        oImpPartyPC.Surname = .Surname
                        oImpPartyPC.Initials = .Initials
                        oImpPartyPC.Title = .Title
                        oImpPartyPC.DateOfBirth = .DateOfBirth
                        oImpPartyPC.GenderCode = .GenderCode
                        oImpPartyPC.MaritalStatusCode = CType([Enum].ToObject(GetType(MaritalStatusCodeType), .MaritalStatusCode), BaseImplementationTypes.MaritalStatusCodeType)
                        oImpPartyPC.OccupationCode = .OccupationCode
                        oImpPartyPC.EmploymentStatusCode = CType([Enum].ToObject(GetType(EmploymentStatusCodeType), .EmploymentStatusCode), BaseImplementationTypes.EmploymentStatusCodeType)
                        oImpPartyPC.EmployersBusinessCode = .EmployersBusinessCode
                        oImpPartyPC.AlternativeId = .AlternativeId
                        oImpPartyPC.Currency = .Currency
                        oImpParty = oImpPartyPC
                    End With

                    ' Common to PC and CC
                    oImpParty.BranchCode = .PersonalClient.BranchCode
                    oImpParty.TPUserCode = .PersonalClient.TPUserCode
                    oImpParty.TPIntroducer = .PersonalClient.TPIntroducer

                    ' Process the address structure
                    If .PersonalClient.Addresses IsNot Nothing Then
                        Dim oMsgAddress As BaseAddressType
                        Dim oImpAddress As New BaseImplementationTypes.BaseAddressType

                        iUBnd% = .PersonalClient.Addresses.Count - 1

                        ReDim oImpParty.Addresses(iUBnd%)

                        For iCnt As Integer = 0 To iUBnd%
                            oMsgAddress = .PersonalClient.Addresses(iCnt%)
                            oImpAddress = New BaseImplementationTypes.BaseAddressType
                            With oMsgAddress
                                oImpAddress.AddressLine1 = .AddressLine1
                                oImpAddress.AddressLine2 = .AddressLine2
                                oImpAddress.AddressLine3 = .AddressLine3
                                oImpAddress.AddressLine4 = .AddressLine4
                                oImpAddress.AddressTypeCode = CType([Enum].ToObject(GetType(AddressTypeType), .AddressTypeCode), BaseImplementationTypes.AddressTypeType)
                                oImpAddress.CountryCode = .CountryCode
                                oImpAddress.PostCode = .PostCode
                            End With
                            oImpParty.Addresses(iCnt%) = oImpAddress
                        Next iCnt%

                    End If

                    ' Process the Contact structure
                    If .PersonalClient.Contacts IsNot Nothing Then
                        Dim oImpContact As New BaseImplementationTypes.BaseContactType
                        Dim oMsgContact As BaseContactType

                        iUBnd% = .PersonalClient.Contacts.Count - 1

                        ReDim oImpParty.Contacts(iUBnd%)

                        For iCnt As Integer = iLBnd% To iUBnd%
                            oMsgContact = .PersonalClient.Contacts(iCnt%)
                            oImpContact = New BaseImplementationTypes.BaseContactType
                            With oMsgContact
                                oImpContact.AreaCode = .AreaCode
                                oImpContact.ContactDetail = New BaseImplementationTypes.BaseContactDetailType
                                oImpContact.ContactDetail.Item = .ContactDetail.Item
                                oImpContact.ContactDetail.ItemElementName = CType([Enum].ToObject(GetType(ItemChoiceType), .ContactDetail.ItemElementName), BaseImplementationTypes.ItemChoiceType)
                                oImpContact.ContactTypeCode = CType([Enum].ToObject(GetType(ContactTypeType), .ContactTypeCode), BaseImplementationTypes.ContactTypeType)
                            End With
                            oImpParty.Contacts(iCnt%) = oImpContact
                        Next iCnt%

                    End If

                    oImpRequest.Party = oImpParty

                ElseIf .CorporateClient IsNot Nothing Then

                    Dim oImpParty As New BaseImplementationTypes.BasePartyCCType

                    ' Process corporate client
                    Dim oMsgParty As BasePartyCCType = DirectCast(.CorporateClient, BasePartyCCType)
                    Dim oImpPartyCC As New BaseImplementationTypes.BasePartyCCType
                    With oMsgParty
                        oImpPartyCC.CompanyName = .CompanyName
                        oImpPartyCC.MainContact = .MainContact
                        oImpPartyCC.BusinessCode = .BusinessCode
                    End With
                    oImpParty = oImpPartyCC

                    ' Common to PC and CC
                    oImpParty.BranchCode = .CorporateClient.BranchCode
                    oImpParty.TPUserCode = .CorporateClient.TPUserCode
                    oImpParty.TPIntroducer = .CorporateClient.TPIntroducer

                    ' Process the address structure
                    If .CorporateClient.Addresses IsNot Nothing Then
                        Dim oMsgAddress As BaseAddressType
                        Dim oImpAddress As New BaseImplementationTypes.BaseAddressType

                        iUBnd% = .CorporateClient.Addresses.Count - 1

                        ReDim oImpParty.Addresses(iUBnd%)

                        For iCnt As Integer = 0 To iUBnd%
                            oMsgAddress = .CorporateClient.Addresses(iCnt%)
                            oImpAddress = New BaseImplementationTypes.BaseAddressType
                            With oMsgAddress
                                oImpAddress.AddressLine1 = .AddressLine1
                                oImpAddress.AddressLine2 = .AddressLine2
                                oImpAddress.AddressLine3 = .AddressLine3
                                oImpAddress.AddressLine4 = .AddressLine4
                                oImpAddress.AddressTypeCode = CType([Enum].ToObject(GetType(AddressTypeType), .AddressTypeCode), BaseImplementationTypes.AddressTypeType)
                                oImpAddress.CountryCode = .CountryCode
                                oImpAddress.PostCode = .PostCode
                            End With
                            oImpParty.Addresses(iCnt%) = oImpAddress
                        Next iCnt%

                    End If

                    ' Process the Contact structure

                    If .CorporateClient.Contacts IsNot Nothing Then
                        Dim oImpContact As New BaseImplementationTypes.BaseContactType
                        Dim oMsgContact As BaseContactType

                        iUBnd% = .CorporateClient.Contacts.Count - 1

                        ReDim oImpParty.Contacts(iUBnd%)

                        For iCnt As Integer = iLBnd% To iUBnd%
                            oMsgContact = .CorporateClient.Contacts(iCnt%)
                            oImpContact = New BaseImplementationTypes.BaseContactType
                            With oMsgContact
                                oImpContact.AreaCode = .AreaCode
                                oImpContact.ContactDetail = New BaseImplementationTypes.BaseContactDetailType
                                oImpContact.ContactDetail.Item = .ContactDetail.Item
                                oImpContact.ContactDetail.ItemElementName = CType([Enum].ToObject(GetType(ItemChoiceType), .ContactDetail.ItemElementName), BaseImplementationTypes.ItemChoiceType)
                                oImpContact.ContactTypeCode = CType([Enum].ToObject(GetType(ContactTypeType), .ContactTypeCode), BaseImplementationTypes.ContactTypeType)
                            End With
                            oImpParty.Contacts(iCnt%) = oImpContact
                        Next iCnt%

                    End If

                    oImpRequest.Party = oImpParty

                End If

                If .Policy IsNot Nothing Then
                    With .Policy
                        ' Process the Policy Structure
                        oImpRequest.Policy = New BaseImplementationTypes.BaseQuoteRiskMsgType

                        oImpRequest.Policy.BranchCode = .BranchCode
                        oImpRequest.Policy.CoverStartDate = .CoverStartDate.ToShortDateString
                        oImpRequest.Policy.CoverEndDate = .CoverEndDate.ToShortDateString
                        oImpRequest.Policy.Description = .Description
                        oImpRequest.Policy.InsuredName = .InsuredName
                        oImpRequest.Policy.ProductCode = .ProductCode
                        oImpRequest.Policy.QuoteRef = .QuoteRef
                        oImpRequest.Policy.CurrencyCode = .CurrencyCode
                        oImpRequest.Policy.PolicyStatusCode = .PolicyStatusCode
                        oImpRequest.Policy.TransactionTypeCode = .TransactionTypeCode
                        oImpRequest.Policy.AlternateReference = .AlternateReference
                        oImpRequest.Policy.AlternativeRef = .AlternateReference
                        oImpRequest.Policy.NewQuoteRef = .NewQuoteRef
                        oImpRequest.Policy.CommissionRate = .CommissionRate
                        oImpRequest.Policy.CommissionValue = .CommissionValue
                        oImpRequest.Policy.TransactionDueDate = .TransactionDueDate.ToShortDateString
                        oImpRequest.Policy.OldPolicyNumber = .OldPolicyNumber
                        oImpRequest.Policy.LastTransDescription = .LastTransDescription
                        oImpRequest.Policy.AnalysisCode = .AnalysisCode

                        If Not String.IsNullOrEmpty(PolicyProcessRequest.AgentCode) Then
                            oImpRequest.Policy.BusinessTypeCode = "AGENCY"
                        Else
                            oImpRequest.Policy.BusinessTypeCode = "DIRECT"
                        End If

                        If (.MTAReasonCode Is Nothing) Then
                            oImpRequest.Policy.MTAReasonCode = "OTHER"
                        Else
                            oImpRequest.Policy.MTAReasonCode = .MTAReasonCode
                        End If

                        If .IsMarketPlacePolicy Then
                            oImpRequest.Policy.IsMarketPlacePolicy = .IsMarketPlacePolicy
                            oImpRequest.Policy.PolicyProcessType = .PolicyProcessType
                            oImpRequest.Policy.OverrideNetPremium = .OverrideNetPremium
                        Else
                            oImpRequest.Policy.IsMarketPlacePolicy = False
                            oImpRequest.Policy.PolicyProcessType = BaseImplementationTypes.PolicyProcessTypes.Bind
                        End If

                        If .Taxes IsNot Nothing Then
                            oImpRequest.Policy.Taxes = Array.ConvertAll(.Taxes.ToArray(),
                                                New Converter(Of BaseTaxesType, BaseImplementationTypes.BaseTaxesType) _
                                                (AddressOf ToBaseImpBaseTaxesAndFeesRiskType))
                        End If
                        ' Process the Risks structure
                        If .Risks IsNot Nothing Then
                            oImpRequest.Policy.Risks = Array.ConvertAll(.Risks.ToArray(),
                                        New Converter(Of BaseRiskType, BaseImplementationTypes.BaseQuoteRiskMsgTypeRisks) _
                                        (AddressOf ToBaseImpBaseQuoteRiskMsgTypeRisks))

                        End If
                    End With
                End If
            End With

            oImpResponse = oMessagingBusiness.PolicyProcess(oImpRequest)

            ' Return errors
            SAMFunc.ConvertWCFSTSError(oImpResponse.STSError, oMsgResponseV2.STSErrorType)
            If (oMsgResponseV2.STSErrorType Is Nothing) Then
                If IsArray(oImpResponse.Policy) Then
                    PopulateInsured(oImpResponse.Insured.Shortname, PolicyProcessRequest.BranchCode, oMsgResponseV2)
                    PopulatePolicy(oImpResponse.Policy(0).InsuranceFileKey, PolicyProcessRequest.BranchCode, oMsgResponseV2)
                End If
            ElseIf oMsgResponseV2.STSErrorType.STSBusinessRule IsNot Nothing AndAlso (oMsgResponseV2.STSErrorType.STSBusinessRule.Code.Trim.ToString = "275" Or oMsgResponseV2.STSErrorType.STSBusinessRule.Code.Trim.ToString = "276" Or oMsgResponseV2.STSErrorType.STSBusinessRule.Code.Trim.ToString = "279" Or oMsgResponseV2.STSErrorType.STSBusinessRule.Code.Trim.ToString = "280") Then
                If IsArray(oImpResponse.Policy) Then
                    PopulateInsured(oImpResponse.Insured.Shortname, PolicyProcessRequest.BranchCode, oMsgResponseV2)
                    PopulatePolicy(oImpResponse.Policy(0).InsuranceFileKey, PolicyProcessRequest.BranchCode, oMsgResponseV2)
                End If
            End If

        Catch SAMError As Sirius.Architecture.ExceptionHandling.SAMErrorException
            ConvertSAMErrorToSTSResponse(oMsgResponseV2, SAMError)
            Return oMsgResponseV2

        Catch exError As Exception
            Dim STSErrorFileError As New STSErrorPublisher("An error occured when attempting PolicyProcessV2", exError)
            STSErrorFileError.Raise(HttpContext.Current.Request.Url.ToString(), "PolicyProcessV2", "PolicyProcessV2", True)

        End Try
        Return oMsgResponseV2
    End Function

    ''' <summary>
    ''' this method to generate documents attached to a process (Quote,NB,MTAQuote,MTA)
    ''' </summary>
    ''' <param name="GenerateDocumentsV2Request"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GenerateDocumentsV2(ByVal GenerateDocumentsV2Request As GenerateDocumentsV2RequestType) As GenerateDocumentsV2ResponseType Implements IPureMessagingService.GenerateDocumentsV2

        Dim oMsgResponseV2 As New GenerateDocumentsV2ResponseType
        Dim oImpRequest As New MessagingImplementationTypes.GenerateDocumentsV2RequestType
        Dim oImpResponse As MessagingImplementationTypes.GenerateDocumentsV2ResponseType
        Dim oMessagingBusiness As New CoreSAMBusiness()
        Try
            oImpRequest.DocumentProcessType = GenerateDocumentsV2Request.DocumentProcessType
            oImpRequest.PolicyProcessType = GenerateDocumentsV2Request.PolicyProcessType
            oImpRequest.BranchCode = GenerateDocumentsV2Request.BranchCode
            oImpRequest.InsuranceFileKey = GenerateDocumentsV2Request.InsuranceFileKey
            oImpRequest.ClaimKey = GenerateDocumentsV2Request.ClaimKey

            oImpResponse = oMessagingBusiness.GenerateDocumentsV2(oImpRequest)
            SAMFunc.ConvertWCFSTSError(oImpResponse.STSError, oMsgResponseV2.STSErrorType)

            If oImpResponse IsNot Nothing AndAlso oMsgResponseV2.STSErrorType Is Nothing Then
                oMsgResponseV2.Documents = New System.Collections.Generic.List(Of BaseGenerateDocumentV2ResponseTypeDocument)
                For Each row In oImpResponse.Documents
                    Dim oDocumentRow As New BaseGenerateDocumentV2ResponseTypeDocument

                    oDocumentRow.DocumentCode = row.DocumentCode
                    oDocumentRow.DocumentDescription = row.DocumentDescription
                    oDocumentRow.MergedFilePath = row.MergedFilePath
                    oDocumentRow.SpooledZipFile = row.SpooledZipFile
                    oMsgResponseV2.Documents.Add(oDocumentRow)
                Next
            End If

        Catch SAMError As Sirius.Architecture.ExceptionHandling.SAMErrorException
            ConvertSAMErrorToSTSResponse(oMsgResponseV2, SAMError)
            Return oMsgResponseV2

        Catch exError As Exception
            Dim STSErrorFileError As New STSErrorPublisher("An error occured when attempting GenerateDocumentsV2", exError)
            STSErrorFileError.Raise(HttpContext.Current.Request.Url.ToString(), "GenerateDocumentsV2", "GenerateDocumentsV2", True)

        End Try
        Return oMsgResponseV2
    End Function

    ''' <summary>
    ''' this method will populate the response of ProcessPolicyV2 for Party
    ''' </summary>
    ''' <param name="sPartyShortName"></param>
    ''' <param name="sBranchCode"></param>
    ''' <param name="r_oResponse"></param>
    ''' <remarks></remarks>
    Private Sub PopulateInsured(ByVal sPartyShortName As String, ByVal sBranchCode As String, ByRef r_oResponse As PolicyProcessV2ResponseType)
        Dim oMessagingBusiness As New CoreSAMBusiness()
        Dim oFindPartyRequest As New BaseImplementationTypes.BaseFindPartyRequestType
        Dim oFindPartyResponse As BaseImplementationTypes.BaseFindPartyResponseType
        oFindPartyRequest.BranchCode = sBranchCode
        oFindPartyRequest.Status = "0"
        oFindPartyRequest.Shortname = sPartyShortName
        oFindPartyRequest.PartyType = "BDXIgnorePartyType"
        oFindPartyRequest.PartyTypeSpecified = True
        oFindPartyResponse = oMessagingBusiness.FindParty(oFindPartyRequest)
        If oFindPartyResponse IsNot Nothing Then
            Dim oPartyRow As System.Collections.Generic.List(Of SFI.SAMForInsuranceV2.WCF.BaseFindPartyResponseTypeRow) = SAMFunc.GetDeserializedValues(Of List(Of SFI.SAMForInsuranceV2.WCF.BaseFindPartyResponseTypeRow))(elmResultDataSet:=oFindPartyResponse.ResultDataset, sFromTypeName:="BaseFindPartyResponseTypeParties", sConvertToTypeName:="BaseFindPartyResponseTypeRow")
            If oPartyRow IsNot Nothing AndAlso oPartyRow.Count > 0 Then
                r_oResponse.Insured = New BasePolicyProcessV2ResponseTypeInsured
                r_oResponse.Insured.AddressLine1 = oPartyRow(0).AddressLine1
                r_oResponse.Insured.AddressLine2 = oPartyRow(0).AddressLine2
                r_oResponse.Insured.AgentKey = oPartyRow(0).AgentKey
                r_oResponse.Insured.AgentType = oPartyRow(0).AgentType
                r_oResponse.Insured.AllowConsolidatedCommission = oPartyRow(0).AllowConsolidatedCommission
                r_oResponse.Insured.ContactTelephoneNumber = oPartyRow(0).ContactTelephoneNumber
                r_oResponse.Insured.DateOfBirth = oPartyRow(0).DateOfBirth
                r_oResponse.Insured.FileCode = oPartyRow(0).FileCode
                r_oResponse.Insured.IsProspect = oPartyRow(0).IsProspect
                r_oResponse.Insured.IsRIBroker = oPartyRow(0).IsRIBroker
                r_oResponse.Insured.Name = oPartyRow(0).Name
                r_oResponse.Insured.PartyKey = oPartyRow(0).PartyKey
                r_oResponse.Insured.PartySourceDescription = oPartyRow(0).PartySourceDescription
                r_oResponse.Insured.PartySourceId = oPartyRow(0).PartySourceId
                r_oResponse.Insured.PostCode = oPartyRow(0).PostCode
                r_oResponse.Insured.ReinsuranceType = oPartyRow(0).ReinsuranceType
                r_oResponse.Insured.ResolvedName = oPartyRow(0).ResolvedName
                r_oResponse.Insured.ShortName = oPartyRow(0).ShortName
                r_oResponse.Insured.Status = oPartyRow(0).Status
                r_oResponse.Insured.SwiftLink = oPartyRow(0).SwiftLink
                r_oResponse.Insured.Type = oPartyRow(0).Type
            End If
        End If
    End Sub

    ''' <summary>
    ''' this method will populate the response of ProcessPolicyV2 for Policy
    ''' </summary>
    ''' <param name="nInsuranceFileKey"></param>
    ''' <param name="sBranchCode"></param>
    ''' <param name="r_oResponse"></param>
    ''' <remarks></remarks>
    Private Sub PopulatePolicy(ByVal nInsuranceFileKey As Integer, ByVal sBranchCode As String, ByRef r_oResponse As PolicyProcessV2ResponseType)
        Dim oMessagingBusiness As New CoreSAMBusiness()

        Dim oGetHeaderAndSummariesRequest As New SAMForInsuranceV2ImplementationTypes.GetHeaderAndSummariesByKeyRequestType
        Dim oGetHeaderAndSummariesResponse As SAMForInsuranceV2ImplementationTypes.GetHeaderAndSummariesByKeyResponseType

        With oGetHeaderAndSummariesRequest
            .BranchCode = sBranchCode
            .InsuranceFileKey = nInsuranceFileKey
        End With

        oGetHeaderAndSummariesResponse = oMessagingBusiness.GetHeaderAndSummariesByKey(oGetHeaderAndSummariesRequest)

        If oGetHeaderAndSummariesResponse IsNot Nothing Then
            r_oResponse.Policy = New BasePolicyProcessV2ResponseTypePolicy
            r_oResponse.Policy.AccountHandler = oGetHeaderAndSummariesResponse.AccountHandler
            r_oResponse.Policy.AccountHandlerCnt = oGetHeaderAndSummariesResponse.AccountHandlerCnt
            r_oResponse.Policy.AccountHandlerCntSpecified = oGetHeaderAndSummariesResponse.AccountHandlerCntSpecified
            r_oResponse.Policy.AccountHandlerCode = oGetHeaderAndSummariesResponse.AccountHandlerCode
            r_oResponse.Policy.AlternativeRef = oGetHeaderAndSummariesResponse.AlternativeRef
            r_oResponse.Policy.AnalysisCode = oGetHeaderAndSummariesResponse.AnalysisCode
            r_oResponse.Policy.AnniversaryCopy = oGetHeaderAndSummariesResponse.AnniversaryCopy
            r_oResponse.Policy.AnnualPremium = oGetHeaderAndSummariesResponse.AnnualPremium
            r_oResponse.Policy.AnnualPremiumSpecified = oGetHeaderAndSummariesResponse.AnnualPremiumSpecified
            r_oResponse.Policy.BaseInsuranceFolderKey = oGetHeaderAndSummariesResponse.BaseInsuranceFolderKey
            r_oResponse.Policy.BranchCode = oGetHeaderAndSummariesResponse.BranchCode
            r_oResponse.Policy.BusinessTypeCode = oGetHeaderAndSummariesResponse.BusinessTypeCode

            'fetch agent commission 
            Dim oGetAgentCommissionRequest As New BaseImplementationTypes.BaseGetAgentCommissionRequestType
            Dim oGetAgentCommissionResponse As BaseImplementationTypes.BaseGetAgentCommissionResponseType
            With oGetAgentCommissionRequest
                .BranchCode = sBranchCode
                .InsuranceFileKey = nInsuranceFileKey
            End With
            oGetAgentCommissionResponse = oMessagingBusiness.GetAgentCommission(oGetAgentCommissionRequest)

            Dim dTotalCommission As Double = 0.0
            If oGetAgentCommissionResponse IsNot Nothing Then
                With oGetAgentCommissionResponse
                    If .AgentCommission IsNot Nothing AndAlso .AgentCommission.Row IsNot Nothing AndAlso .AgentCommission.Row.Count > 0 Then
                        r_oResponse.Policy.Commission = New BasePolicyProcessV2ResponseTypeCommission
                        r_oResponse.Policy.Commission.AgentCommission = New System.Collections.Generic.List(Of BaseAgentCommissionResponseTypeRow)
                        For iRowCount As Integer = 0 To oGetAgentCommissionResponse.AgentCommission.Row.Count - 1
                            Dim oCommRow As New BaseAgentCommissionResponseTypeRow
                            oCommRow.Agent = .AgentCommission.Row(iRowCount).Agent
                            oCommRow.AgentType = .AgentCommission.Row(iRowCount).AgentType
                            oCommRow.CommissionBand = .AgentCommission.Row(iRowCount).CommissionBand
                            oCommRow.CommissionRate = .AgentCommission.Row(iRowCount).CommissionRate
                            oCommRow.CommissionValue = .AgentCommission.Row(iRowCount).CommissionValue
                            oCommRow.IsLeadAgent = .AgentCommission.Row(iRowCount).IsLeadAgent
                            oCommRow.IsValue = .AgentCommission.Row(iRowCount).IsValue
                            oCommRow.MaximumRate = .AgentCommission.Row(iRowCount).MaximumRate
                            oCommRow.Premium = .AgentCommission.Row(iRowCount).Premium
                            oCommRow.RiskType = .AgentCommission.Row(iRowCount).RiskType
                            oCommRow.TaxGroup = .AgentCommission.Row(iRowCount).TaxGroup
                            oCommRow.TaxGroupDescription = .AgentCommission.Row(iRowCount).TaxGroupDescription
                            oCommRow.TaxValue = .AgentCommission.Row(iRowCount).TaxValue
                            r_oResponse.Policy.Commission.AgentCommission.Add(oCommRow)
                        Next
                        r_oResponse.Policy.Commission.LeadAgentNet = .LeadAgentNet
                        r_oResponse.Policy.Commission.LeadAgentTotalCommission = .LeadAgentTotalCommission
                        r_oResponse.Policy.Commission.LeadAgentTotalTax = .LeadAgentTotalTax
                        r_oResponse.Policy.Commission.SubAgentNet = .SubAgentNet
                        r_oResponse.Policy.Commission.SubAgentTotalCommission = .SubAgentTotalCommission
                        r_oResponse.Policy.Commission.SubAgentTotalTax = .SubAgentTotalTax
                        dTotalCommission = .LeadAgentTotalCommission + .SubAgentTotalCommission
                    End If
                End With
            End If
            r_oResponse.Policy.TotalCommission = dTotalCommission
            r_oResponse.Policy.ConsolidatedLeadAgentCommission = oGetHeaderAndSummariesResponse.ConsolidatedLeadAgentCommission
            r_oResponse.Policy.ConsolidatedSubAgentCommission = oGetHeaderAndSummariesResponse.ConsolidatedSubAgentCommission
            r_oResponse.Policy.ContactUserEmail = oGetHeaderAndSummariesResponse.ContactUserEmail
            r_oResponse.Policy.ContactUserFullName = oGetHeaderAndSummariesResponse.ContactUserFullName
            r_oResponse.Policy.ContactuserKey = oGetHeaderAndSummariesResponse.ContactuserKey
            r_oResponse.Policy.ContactUserName = oGetHeaderAndSummariesResponse.ContactUserName
            r_oResponse.Policy.CoverEndDate = oGetHeaderAndSummariesResponse.CoverEndDate
            r_oResponse.Policy.CoverStartDate = oGetHeaderAndSummariesResponse.CoverStartDate
            r_oResponse.Policy.CurrencyCode = oGetHeaderAndSummariesResponse.CurrencyCode
            r_oResponse.Policy.Description = oGetHeaderAndSummariesResponse.Description
            'fetch the fees collection

            Dim oGetHeaderAndPolicyFeesByKeyRequest As New BaseImplementationTypes.BaseGetHeaderAndPolicyFeesByKeyRequestType
            Dim oGetHeaderAndPolicyFeesByKeyResponse As BaseImplementationTypes.BaseGetHeaderAndPolicyFeesByKeyResponseType
            oGetHeaderAndPolicyFeesByKeyRequest.BranchCode = sBranchCode
            oGetHeaderAndPolicyFeesByKeyRequest.InsuranceFileKey = nInsuranceFileKey

            oGetHeaderAndPolicyFeesByKeyResponse = oMessagingBusiness.GetHeaderAndPolicyFeesByKey(oGetHeaderAndPolicyFeesByKeyRequest)

            Dim dTotalFees As Double = 0.0
            If oGetHeaderAndPolicyFeesByKeyResponse IsNot Nothing Then
                Dim oSFIFeeResponse As New SFI.SAMForInsuranceV2.WCF.GetHeaderAndPolicyFeesByKeyResponseType
                oSFIFeeResponse.PolicyFees = SAMFunc.GetDeserializedValues(Of List(Of SFI.SAMForInsuranceV2.WCF.BaseGetHeaderAndPolicyFeesByKeyResponseTypeRow))(elmResultDataSet:=oGetHeaderAndPolicyFeesByKeyResponse.ResultDataset, sFromTypeName:="BaseGetHeaderAndPolicyFeesByKeyResponseTypePolicyFees", sConvertToTypeName:="BaseGetHeaderAndPolicyFeesByKeyResponseTypeRow")

                With oSFIFeeResponse
                    If .PolicyFees IsNot Nothing AndAlso .PolicyFees.Count > 0 Then
                        r_oResponse.Policy.Fees = New System.Collections.Generic.List(Of BasePolicyProcessV2ResponseTypePolicyFees)
                        For iRowCount As Integer = 0 To .PolicyFees.Count - 1
                            Dim oFeeRow As New BasePolicyProcessV2ResponseTypePolicyFees
                            oFeeRow.AppliedTo = .PolicyFees(iRowCount).AppliedTo
                            oFeeRow.CurrencyCode = .PolicyFees(iRowCount).CurrencyCode
                            oFeeRow.FeeAmount = .PolicyFees(iRowCount).FeeAmount
                            oFeeRow.FeeName = .PolicyFees(iRowCount).FeeName
                            oFeeRow.IncludeInInstallment = .PolicyFees(iRowCount).IncludeInInstallment
                            oFeeRow.IsValue = .PolicyFees(iRowCount).IsValue
                            oFeeRow.PolicyFeeKey = .PolicyFees(iRowCount).PolicyFeeKey
                            oFeeRow.Premium = .PolicyFees(iRowCount).Premium
                            oFeeRow.Rate = .PolicyFees(iRowCount).Rate
                            oFeeRow.SpreadAcrossInstallment = .PolicyFees(iRowCount).SpreadAcrossInstallment
                            oFeeRow.TaxAmount = .PolicyFees(iRowCount).TaxAmount
                            oFeeRow.TaxGroup = .PolicyFees(iRowCount).TaxGroup
                            oFeeRow.TotalAmount = .PolicyFees(iRowCount).TotalAmount
                            dTotalFees = dTotalFees + .PolicyFees(iRowCount).FeeAmount
                            r_oResponse.Policy.Fees.Add(oFeeRow)
                        Next
                    End If
                End With
            End If
            r_oResponse.Policy.TotalFees = dTotalFees
            r_oResponse.Policy.HasClaimLink = oGetHeaderAndSummariesResponse.HasClaimLink
            r_oResponse.Policy.HCExpiryDate = oGetHeaderAndSummariesResponse.HCExpiryDate
            r_oResponse.Policy.InceptionDt = oGetHeaderAndSummariesResponse.InceptionDt
            r_oResponse.Policy.InceptionTPI = oGetHeaderAndSummariesResponse.InceptionTPI
            r_oResponse.Policy.InsuranceFileKey = oGetHeaderAndSummariesResponse.InsuranceFileKey
            r_oResponse.Policy.InsuranceFileRef = oGetHeaderAndSummariesResponse.InsuranceFileRef
            r_oResponse.Policy.InsuranceFolderKey = oGetHeaderAndSummariesResponse.InsuranceFolderKey
            r_oResponse.Policy.InsuredName = oGetHeaderAndSummariesResponse.InsuredName
            r_oResponse.Policy.IsDeletedContactuser = oGetHeaderAndSummariesResponse.IsDeletedContactuser
            r_oResponse.Policy.IssueDate = oGetHeaderAndSummariesResponse.IssueDate
            r_oResponse.Policy.IssueDateSpecified = oGetHeaderAndSummariesResponse.IssueDateSpecified
            r_oResponse.Policy.IsValidAnniversaryToAccept = oGetHeaderAndSummariesResponse.IsValidAnniversaryToAccept
            r_oResponse.Policy.LapseDate = oGetHeaderAndSummariesResponse.LapseDate
            r_oResponse.Policy.LapseDateSpecified = oGetHeaderAndSummariesResponse.LapseDateSpecified
            r_oResponse.Policy.LapsedReasonCode = oGetHeaderAndSummariesResponse.LapsedReasonCode
            r_oResponse.Policy.LeadAgent = oGetHeaderAndSummariesResponse.LeadAgent
            r_oResponse.Policy.LeadAgentCode = oGetHeaderAndSummariesResponse.LeadAgentCode
            r_oResponse.Policy.LeadAgentKey = oGetHeaderAndSummariesResponse.LeadAgentKey
            r_oResponse.Policy.LTUExpiryDate = oGetHeaderAndSummariesResponse.LTUExpiryDate
            r_oResponse.Policy.LTUExpiryDateSpecified = oGetHeaderAndSummariesResponse.LTUExpiryDateSpecified
            r_oResponse.Policy.MarkedForCollection = oGetHeaderAndSummariesResponse.MarkedForCollection
            r_oResponse.Policy.NetPremium = oGetHeaderAndSummariesResponse.NetPremium
            r_oResponse.Policy.NetPremiumSpecified = oGetHeaderAndSummariesResponse.NetPremiumSpecified
            r_oResponse.Policy.PartyKey = oGetHeaderAndSummariesResponse.PartyKey
            r_oResponse.Policy.PolicyDeductible = oGetHeaderAndSummariesResponse.PolicyDeductible

            'r_oResponse.Policy.PolicyLevelTaxesAndFees = 
            'r_oResponse.Policy.PolicyLevelTaxesAndFees = 
            r_oResponse.Policy.PolicyLimits = oGetHeaderAndSummariesResponse.PolicyLimits
            If String.IsNullOrEmpty(oGetHeaderAndSummariesResponse.PolicyStatusCode) Then
                If Cast.ToString(oGetHeaderAndSummariesResponse.InsuranceFileTypeCode, String.Empty).ToUpper().Trim() = "QUOTE" Or Cast.ToString(oGetHeaderAndSummariesResponse.InsuranceFileTypeCode, String.Empty).ToUpper().Trim() = "MTAQUOTE" Then
                    r_oResponse.Policy.PolicyStatusCode = "QUOTE"
                ElseIf Cast.ToString(oGetHeaderAndSummariesResponse.InsuranceFileTypeCode, String.Empty).ToUpper().Trim() = "RENEWAL" Then
                    r_oResponse.Policy.PolicyStatusCode = "IN RENEWAL"
                ElseIf Cast.ToString(oGetHeaderAndSummariesResponse.InsuranceFileTypeCode, String.Empty).ToUpper().Trim() = "WRITTEN" Then
                    r_oResponse.Policy.PolicyStatusCode = "WRITTEN"
                Else
                    r_oResponse.Policy.PolicyStatusCode = "LIVE"
                End If
            Else
                Select Case Cast.ToString(oGetHeaderAndSummariesResponse.PolicyStatusCode, String.Empty).ToUpper().Trim()
                    Case "CAN"
                        r_oResponse.Policy.PolicyStatusCode = "CANCELLED"
                    Case "LAP"
                        r_oResponse.Policy.PolicyStatusCode = "LAPSED"
                    Case "REN"
                        r_oResponse.Policy.PolicyStatusCode = "UNDER RENEWAL"
                    Case "TRA"
                        r_oResponse.Policy.PolicyStatusCode = "TRANSFERRED"
                    Case "REP"
                        r_oResponse.Policy.PolicyStatusCode = "RENEWED"
                    Case "REPBDMTA"
                        r_oResponse.Policy.PolicyStatusCode = "LIVE"
                End Select
            End If
            r_oResponse.Policy.PolicyStyleCode = oGetHeaderAndSummariesResponse.PolicyStyleCode
            r_oResponse.Policy.PolicyTypeCode = oGetHeaderAndSummariesResponse.PolicyTypeCode
            r_oResponse.Policy.ProductCode = oGetHeaderAndSummariesResponse.ProductCode
            r_oResponse.Policy.ProductName = oGetHeaderAndSummariesResponse.ProductName
            r_oResponse.Policy.ProposalDate = oGetHeaderAndSummariesResponse.ProposalDate
            r_oResponse.Policy.ProposalDateSpecified = oGetHeaderAndSummariesResponse.ProposalDateSpecified
            r_oResponse.Policy.PutOnNextMTAInstallmentRenewal = oGetHeaderAndSummariesResponse.PutOnNextMTAInstallmentRenewal
            r_oResponse.Policy.QuoteExpiryDate = oGetHeaderAndSummariesResponse.QuoteExpiryDate
            r_oResponse.Policy.QuoteIsLocked = oGetHeaderAndSummariesResponse.QuoteIsLocked
            r_oResponse.Policy.QuoteStatusKey = oGetHeaderAndSummariesResponse.QuoteStatusKey
            r_oResponse.Policy.QuoteTimeStamp = oGetHeaderAndSummariesResponse.QuoteTimeStamp
            r_oResponse.Policy.QuoteVersion = oGetHeaderAndSummariesResponse.QuoteVersion
            r_oResponse.Policy.ReferredAtRenewal = oGetHeaderAndSummariesResponse.ReferredAtRenewal
            r_oResponse.Policy.ReferredOnMTA = oGetHeaderAndSummariesResponse.ReferredOnMTA
            r_oResponse.Policy.Regarding = oGetHeaderAndSummariesResponse.Regarding
            r_oResponse.Policy.RenewalDate = oGetHeaderAndSummariesResponse.RenewalDate
            r_oResponse.Policy.RenewalDayNo = oGetHeaderAndSummariesResponse.RenewalDayNo
            r_oResponse.Policy.RenewalFrequencyCode = oGetHeaderAndSummariesResponse.RenewalFrequencyCode
            r_oResponse.Policy.RenewalMethodCode = oGetHeaderAndSummariesResponse.RenewalMethodCode
            r_oResponse.Policy.RenewalStatusTypeCode = oGetHeaderAndSummariesResponse.RenewalStatusTypeCode
            r_oResponse.Policy.RenewalStatusTypeDesc = oGetHeaderAndSummariesResponse.RenewalStatusTypeDescription
            r_oResponse.Policy.RenewedCount = oGetHeaderAndSummariesResponse.RenewedCount
            'fetch policy risks
            Dim oGetHeaderAndRisksByKeyRequest As New BaseImplementationTypes.BaseGetHeaderAndRisksByKeyRequestType
            Dim oGetHeaderAndRisksByKeyResponse As BaseImplementationTypes.BaseGetHeaderAndRisksByKeyResponseType
            oGetHeaderAndRisksByKeyRequest.BranchCode = sBranchCode
            oGetHeaderAndRisksByKeyRequest.InsuranceFileKey = nInsuranceFileKey
            oGetHeaderAndRisksByKeyResponse = oMessagingBusiness.GetHeaderAndRisksByKey(oGetHeaderAndRisksByKeyRequest)

            If oGetHeaderAndRisksByKeyResponse IsNot Nothing Then
                With oGetHeaderAndRisksByKeyResponse
                    r_oResponse.Policy.GrossPremium = .GrossTotal
                    If .Risks IsNot Nothing AndAlso .Risks.Row IsNot Nothing AndAlso .Risks.Row.Count > 0 Then

                        r_oResponse.Policy.Risks = New List(Of BasePolicyProcessV2ResponseTypeRisks)
                        For iRiskCount As Integer = 0 To .Risks.Row.Count - 1
                            Dim oGetRiskRequest As New BaseImplementationTypes.BaseGetRiskRequestType
                            Dim oGetRiskResponse As BaseImplementationTypes.BaseGetRiskResponseType
                            oGetRiskRequest.BranchCode = sBranchCode
                            oGetRiskRequest.IgnoreLocking = True
                            oGetRiskRequest.InsuranceFileKey = nInsuranceFileKey
                            oGetRiskRequest.InsuranceFolderKey = oGetHeaderAndSummariesResponse.InsuranceFolderKey
                            oGetRiskRequest.QuoteTimeStamp = oGetHeaderAndSummariesResponse.QuoteTimeStamp
                            oGetRiskRequest.RiskKey = .Risks.Row(iRiskCount).RiskKey
                            oGetRiskResponse = oMessagingBusiness.RetrieveRisk(oGetRiskRequest)
                            If oGetRiskResponse IsNot Nothing Then
                                With oGetRiskResponse
                                    Dim oRiskRow As New BasePolicyProcessV2ResponseTypeRisks
                                    oRiskRow.CommissionAmount = .CommissionAmount
                                    'collection is there if we want to Segregate risk level fees 
                                    'oRiskRow.Fees = .
                                    oRiskRow.PolicyLevelTax = .PolicyLevelTax
                                    'oRiskRow.PolicyLevelTaxesAndFees = 
                                    oRiskRow.PremiumDueGross = .PremiumDueGross
                                    oRiskRow.PremiumDueNet = .PremiumDueNet
                                    oRiskRow.PremiumDueTax = .PremiumDueTax
                                    oRiskRow.proRataRate = .proRataRate
                                    oRiskRow.QuoteTimeStamp = .QuoteTimeStamp
                                    oRiskRow.RiskStatus = oGetHeaderAndRisksByKeyResponse.Risks.Row(iRiskCount).StatusCode
                                    oRiskRow.RiskFolderKey = oGetHeaderAndRisksByKeyResponse.Risks.Row(iRiskCount).RiskFolderKey
                                    'fetch rating sections
                                    Dim oGetRatingDetailsRequest As New BaseImplementationTypes.BaseGetRatingDetailsRequestType
                                    Dim oGetRatingDetailsResponse As BaseImplementationTypes.BaseGetRatingDetailsResponseType
                                    oGetRatingDetailsRequest.BranchCode = sBranchCode
                                    oGetRatingDetailsRequest.InsuranceFileKey = nInsuranceFileKey
                                    oGetRatingDetailsRequest.RiskKey = oGetHeaderAndRisksByKeyResponse.Risks.Row(iRiskCount).RiskKey
                                    oGetRatingDetailsResponse = oMessagingBusiness.GetRatingDetails(oGetRatingDetailsRequest)
                                    If oGetRatingDetailsResponse IsNot Nothing Then
                                        With oGetRatingDetailsResponse
                                            If .RatingDetails IsNot Nothing AndAlso .RatingDetails.HasChildNodes Then
                                                oRiskRow.RatingSections = New List(Of BasePolicyProcessV2ResponseTypeRatingSections)
                                                Dim oSFIRatingResponse As New SFI.SAMForInsuranceV2.WCF.BaseGetRatingDetailsResponseType
                                                oSFIRatingResponse.RatingDetails = SAMFunc.GetDeserializedValues(Of List(Of SFI.SAMForInsuranceV2.WCF.BaseGetRatingDetailsResponseTypeRow))(elmResultDataSet:=oGetRatingDetailsResponse.RatingDetails, sFromTypeName:="BaseGetRatingDetailsResponseTypeRatingDetails", sConvertToTypeName:="BaseGetRatingDetailsResponseTypeRow")

                                                For Each row In oSFIRatingResponse.RatingDetails
                                                    Dim oRatingRow As New BasePolicyProcessV2ResponseTypeRatingSections
                                                    oRatingRow.AnnualPremium = row.AnnualPremium
                                                    oRatingRow.AnnualRate = row.AnnualRate
                                                    oRatingRow.CalculatedPremium = row.CalculatedPremium
                                                    oRatingRow.Country = row.Country
                                                    oRatingRow.CountryCode = row.CountryCode
                                                    oRatingRow.CountryId = row.CountryId
                                                    oRatingRow.CurrencyCode = row.CurrencyCode
                                                    oRatingRow.CurrencyId = row.CurrencyId
                                                    oRatingRow.EarningPattern = row.EarningPattern
                                                    oRatingRow.EarningPatternCode = row.EarningPatternCode
                                                    oRatingRow.EarningPatternId = row.EarningPatternId
                                                    oRatingRow.IsAmended = row.IsAmended
                                                    oRatingRow.OriginalFlag = row.OriginalFlag
                                                    oRatingRow.OverrideReason = row.OverrideReason
                                                    oRatingRow.PolicySectionType = row.PolicySectionType
                                                    oRatingRow.PolicySectionTypeId = row.PolicySectionTypeId
                                                    oRatingRow.RateType = row.RateType
                                                    oRatingRow.RateTypeId = row.RateTypeId
                                                    oRatingRow.RatingSectionId = row.RatingSectionId
                                                    oRatingRow.RatingSectionType = row.RatingSectionType
                                                    oRatingRow.RatingSectionTypeCode = row.RatingSectionTypeCode
                                                    oRatingRow.RatingSectionTypeId = row.RatingSectionTypeId
                                                    oRatingRow.RatingTypeCode = row.RatingTypeCode
                                                    oRatingRow.State = row.State
                                                    oRatingRow.StateCode = row.StateCode
                                                    oRatingRow.StateId = row.StateId
                                                    oRatingRow.SumInsured = row.SumInsured
                                                    oRatingRow.ThisPremium = row.ThisPremium
                                                    oRiskRow.RatingSections.Add(oRatingRow)
                                                Next
                                            End If
                                        End With
                                    End If
                                    'collection is there if we want to Segregate risk level taxes  
                                    'oRiskRow.Taxes = New System.Collections.Generic.List(Of BasePolicyProcessV2ResponseTypeRiskTaxes)
                                    oRiskRow.TotalAnnualTax = .TotalAnnualTax
                                    Dim xmlDoc As New System.Xml.XmlDocument
                                    Dim sDataModelCode As String
                                    xmlDoc.LoadXml(.XMLDataSet)
                                    sDataModelCode = xmlDoc.SelectSingleNode("DATA_SET").Attributes("DataModelCode").Value
                                    oMessagingBusiness.TransformResponseListValues(.XMLDataSet, sDataModelCode)
                                    oMessagingBusiness.TransformResponseUDLValues(.XMLDataSet, sDataModelCode)
                                    oRiskRow.XMLDataSet = SAMFunc.TransformDatasetPBtoSAM(.XMLDataSet)
                                    r_oResponse.Policy.Risks.Add(oRiskRow)
                                End With
                            End If
                        Next
                    End If
                End With
            End If
            r_oResponse.Policy.StandardPolicyDescription = oGetHeaderAndSummariesResponse.StandardPolicyDescription
            r_oResponse.Policy.StandardPolicyWordingCode = oGetHeaderAndSummariesResponse.StandardPolicyWordingCode
            r_oResponse.Policy.StopReasonCode = oGetHeaderAndSummariesResponse.StopReasonCode
            r_oResponse.Policy.SubBranchCode = oGetHeaderAndSummariesResponse.SubBranchCode
            r_oResponse.Policy.TaxAmount = oGetHeaderAndSummariesResponse.TaxAmount
            r_oResponse.Policy.TaxAmountSpecified = oGetHeaderAndSummariesResponse.TaxAmountSpecified
            'fetch taxes

            Dim oGetTaxesRequest As New BaseImplementationTypes.BaseGetTaxesRequestType
            Dim oGetTaxesResponse As BaseImplementationTypes.BaseGetTaxesResponseType

            oGetTaxesRequest.BranchCode = sBranchCode
            oGetTaxesRequest.InsuranceFileKey = nInsuranceFileKey

            oGetTaxesResponse = oMessagingBusiness.GetTaxes(oGetTaxesRequest)

            If oGetTaxesResponse IsNot Nothing Then
                Dim oSFITaxResponse As New SFI.SAMForInsuranceV2.WCF.BaseGetTaxesResponseType
                oSFITaxResponse.Row = SAMFunc.GetDeserializedValues(Of List(Of SFI.SAMForInsuranceV2.WCF.BaseGetTaxesResponseTypeRow))(elmResultDataSet:=oGetTaxesResponse.ResultDataset, sFromTypeName:="BaseGetTaxesResponseType", sConvertToTypeName:="BaseGetTaxesResponseTypeRow")

                If oSFITaxResponse IsNot Nothing AndAlso oSFITaxResponse.Row IsNot Nothing AndAlso oSFITaxResponse.Row.Count > 0 Then
                    r_oResponse.Policy.Taxes = New System.Collections.Generic.List(Of BasePolicyProcessV2ResponseTypePolicyTaxes)
                    For iRowCount As Integer = 0 To oSFITaxResponse.Row.Count - 1
                        Dim oTaxRow As New BasePolicyProcessV2ResponseTypePolicyTaxes
                        oTaxRow.AgentCommissionKey = oSFITaxResponse.Row(iRowCount).AgentCommissionKey
                        oTaxRow.AllowTaxCredit = oSFITaxResponse.Row(iRowCount).AllowTaxCredit
                        oTaxRow.ApplyTaxBy = oSFITaxResponse.Row(iRowCount).ApplyTaxBy
                        oTaxRow.BaseTaxCalculationKey = oSFITaxResponse.Row(iRowCount).BaseTaxCalculationKey
                        oTaxRow.BasisValue = oSFITaxResponse.Row(iRowCount).BasisValue
                        oTaxRow.CalculationBasis = oSFITaxResponse.Row(iRowCount).CalculationBasis
                        oTaxRow.ClaimPaymentItemKey = oSFITaxResponse.Row(iRowCount).ClaimPaymentItemKey
                        oTaxRow.ClaimPaymentKey = oSFITaxResponse.Row(iRowCount).ClaimPaymentKey
                        oTaxRow.ClaimPerilKey = oSFITaxResponse.Row(iRowCount).ClaimPerilKey
                        oTaxRow.ClaimReceiptItemKey = oSFITaxResponse.Row(iRowCount).ClaimReceiptItemKey
                        oTaxRow.ClaimReceiptKey = oSFITaxResponse.Row(iRowCount).ClaimReceiptKey
                        oTaxRow.ClassOfBusinessKey = oSFITaxResponse.Row(iRowCount).ClassOfBusinessKey
                        oTaxRow.CountryKey = oSFITaxResponse.Row(iRowCount).CountryKey
                        oTaxRow.CurrencyKey = oSFITaxResponse.Row(iRowCount).CurrencyKey
                        oTaxRow.IncludeTaxInInstalments = oSFITaxResponse.Row(iRowCount).IncludeTaxInInstalments
                        oTaxRow.InsuranceSectionKey = oSFITaxResponse.Row(iRowCount).InsuranceSectionKey
                        oTaxRow.InsurerPartyKey = oSFITaxResponse.Row(iRowCount).InsurerPartyKey
                        oTaxRow.IsCommissionTax = oSFITaxResponse.Row(iRowCount).IsCommissionTax
                        oTaxRow.IsManuallyChanged = oSFITaxResponse.Row(iRowCount).IsManuallyChanged
                        oTaxRow.IsNotAppliedToClient = oSFITaxResponse.Row(iRowCount).IsNotAppliedToClient
                        oTaxRow.IsSuspended = oSFITaxResponse.Row(iRowCount).IsSuspended
                        oTaxRow.IsValue = oSFITaxResponse.Row(iRowCount).IsValue
                        oTaxRow.OriginalSumInsured = oSFITaxResponse.Row(iRowCount).OriginalSumInsured
                        oTaxRow.PfPremFinanceKey = oSFITaxResponse.Row(iRowCount).PfPremFinanceKey
                        oTaxRow.PfPremFinanceVersion = oSFITaxResponse.Row(iRowCount).PfPremFinanceVersion
                        oTaxRow.PolicyAgentsKey = oSFITaxResponse.Row(iRowCount).PolicyAgentsKey
                        oTaxRow.PolicyCoinsurersSectionKey = oSFITaxResponse.Row(iRowCount).PolicyCoinsurersSectionKey
                        oTaxRow.PolicyFeeKey = oSFITaxResponse.Row(iRowCount).PolicyFeeKey
                        oTaxRow.PolicyFeeUKey = oSFITaxResponse.Row(iRowCount).PolicyFeeUKey
                        oTaxRow.Premium = oSFITaxResponse.Row(iRowCount).Premium
                        oTaxRow.RIArrangementLineKey = oSFITaxResponse.Row(iRowCount).RIArrangementLineKey
                        oTaxRow.RIPartyKey = oSFITaxResponse.Row(iRowCount).RIPartyKey
                        oTaxRow.RiskKey = oSFITaxResponse.Row(iRowCount).RiskKey
                        oTaxRow.Sequence = oSFITaxResponse.Row(iRowCount).Sequence
                        oTaxRow.SpreadTaxAcrossInstalments = oSFITaxResponse.Row(iRowCount).SpreadTaxAcrossInstalments
                        oTaxRow.StateKey = oSFITaxResponse.Row(iRowCount).StateKey
                        oTaxRow.SumInsured = oSFITaxResponse.Row(iRowCount).SumInsured
                        oTaxRow.SumInsuredRounded = oSFITaxResponse.Row(iRowCount).SumInsuredRounded
                        oTaxRow.TaxBandCode = oSFITaxResponse.Row(iRowCount).TaxBandCode
                        oTaxRow.TaxBandDescription = oSFITaxResponse.Row(iRowCount).TaxBandDescription
                        oTaxRow.TaxBandKey = oSFITaxResponse.Row(iRowCount).TaxBandKey
                        oTaxRow.TaxBandRateDescription = oSFITaxResponse.Row(iRowCount).TaxBandRateDescription
                        oTaxRow.TaxBandRateKey = oSFITaxResponse.Row(iRowCount).TaxBandRateKey
                        oTaxRow.TaxCalculationKey = oSFITaxResponse.Row(iRowCount).TaxCalculationKey
                        oTaxRow.TaxGroupCode = oSFITaxResponse.Row(iRowCount).TaxGroupCode
                        oTaxRow.TaxGroupDescription = oSFITaxResponse.Row(iRowCount).TaxGroupDescription
                        oTaxRow.TaxGroupKey = oSFITaxResponse.Row(iRowCount).TaxGroupKey
                        oTaxRow.TaxPercentage = oSFITaxResponse.Row(iRowCount).TaxPercentage
                        oTaxRow.TaxValue = oSFITaxResponse.Row(iRowCount).TaxValue
                        oTaxRow.TransType = oSFITaxResponse.Row(iRowCount).TransType
                        oTaxRow.VersionKey = oSFITaxResponse.Row(iRowCount).VersionKey
                        r_oResponse.Policy.Taxes.Add(oTaxRow)
                    Next
                End If
            End If
            r_oResponse.Policy.ThisPremium = oGetHeaderAndSummariesResponse.ThisPremium
            r_oResponse.Policy.ThisPremiumSpecified = oGetHeaderAndSummariesResponse.ThisPremiumSpecified
            r_oResponse.Policy.UnderwritingYear = oGetHeaderAndSummariesResponse.UnderwritingYear
            r_oResponse.Policy.UnderwritingYearId = oGetHeaderAndSummariesResponse.UnderwritingYearId
        End If
    End Sub

    ''' <summary>
    ''' this method will load web service on each call by Windows service
    ''' this method we are using as a pinging method to avoide first call timeout issues
    ''' </summary>
    ''' <param name="LoadServiceRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function LoadService(ByVal LoadServiceRequest As LoadServiceRequestType) As Integer Implements IPureMessagingService.LoadService

        If LoadServiceRequest.CallingApp = "PureWindowsService" Then
            Dim oMessagingBusiness As New CoreSAMBusiness()
            Dim oGetListRequest As New SiriusFS.SAM.Structure.BaseImplementationTypes.BaseGetListRequestType
            Dim oGetListResponse As New SiriusFS.SAM.Structure.BaseImplementationTypes.BaseGetListResponseType
            oGetListRequest.BranchCode = "HeadOff"
            oGetListRequest.ListCode = "Country"
            oGetListRequest.ListType = STSListType.PMLookup
            oGetListResponse = oMessagingBusiness.GetList(oGetListRequest)
            If oGetListResponse.Errors Is Nothing Then
                Return 1
            Else
                Return 0
            End If
        Else
            Return 0
        End If
    End Function

#End Region

End Class
