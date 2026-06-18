Option Strict On

Imports SiriusFS.SAM
Imports SiriusFS.SAM.CoreImplementation
Imports SiriusFS.SAM.Structure
Imports SiriusFS.SAM.Structure.SFI
Imports SiriusFS.SAM.Structure.STSErrorPublisher
Imports SiriusFS.SAM.ServiceAgent.SAMFunc

<WebService(Namespace:="http://www.siriusfs.com/SFI/SAM/MessagingTypes/20050929")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
Public Class MessagingServiceForUnderwriting

    Inherits System.Web.Services.WebService

    Private Const ServiceNamespace As String = "http://www.siriusfs.com/SFI/SAM/MessagingTypes/20050929"

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function NewBusiness(<XmlElement(elementName:="NewBusinessRequest", Namespace:=ServiceNamespace)> ByVal NewBusinessRequest As MessagingTypes.NewBusinessRequestType) As <XmlElement(elementName:="NewBusinessResponse", Namespace:=ServiceNamespace)> MessagingTypes.NewBusinessResponseType

        Dim iLBnd As Integer
        Dim iUBnd As Integer

        Dim msgResponse As New MessagingTypes.NewBusinessResponseType

        Dim oMessagingBusiness As New MessagingBusiness

        ' Implementation structures
        Dim impRequest As New MessagingImplementationTypes.NewBusinessRequestType
        Dim impResponse As MessagingImplementationTypes.NewBusinessResponseType = Nothing

        ' Convert the incoming interface structures into the implementation structures
        impRequest.AgentCode = NewBusinessRequest.AgentCode
        impRequest.BranchCode = NewBusinessRequest.BranchCode
        impRequest.CurrencyCode = CType([Enum].ToObject(GetType(MessagingTypes.CurrencyType), NewBusinessRequest.CurrencyCode), BaseImplementationTypes.CurrencyType)
        'impRequest.CurrencyCode = NewBusinessRequest.CurrencyCode

        ' Process the Party structure.  We 1st need to check the party type of the incoming message
        If (NewBusinessRequest.Item Is Nothing) = False Then

            Dim impParty As New BaseImplementationTypes.BasePartyType
            Dim impAddress As New BaseImplementationTypes.BaseAddressType
            Dim impContact As New BaseImplementationTypes.BaseContactType

            Dim msgAddress As MessagingTypes.BaseAddressType
            Dim msgContact As MessagingTypes.BaseContactType

            ' Check the type of the party object to see if it is Personal or Corporate
            If NewBusinessRequest.Item.GetType Is GetType(MessagingTypes.BasePartyPCType) Then

                ' Process personal client
                Dim msgParty As MessagingTypes.BasePartyPCType = DirectCast(NewBusinessRequest.Item, MessagingTypes.BasePartyPCType)
                Dim impPartyPC As New BaseImplementationTypes.BasePartyPCType

                impPartyPC.Forename = msgParty.Forename
                impPartyPC.Surname = msgParty.Surname
                impPartyPC.Initials = msgParty.Initials
                impPartyPC.Title = msgParty.Title
                impPartyPC.DateOfBirth = msgParty.DateOfBirth
                impPartyPC.GenderCode = msgParty.GenderCode
                impPartyPC.MaritalStatusCode = CType([Enum].ToObject(GetType(MessagingTypes.MaritalStatusCodeType), msgParty.MaritalStatusCode), BaseImplementationTypes.MaritalStatusCodeType)
                'impPartyPC.MaritalStatusCode = msgParty.MaritalStatusCode
                impPartyPC.OccupationCode = msgParty.OccupationCode
                impPartyPC.EmploymentStatusCode = CType([Enum].ToObject(GetType(MessagingTypes.EmploymentStatusCodeType), msgParty.EmploymentStatusCode), BaseImplementationTypes.EmploymentStatusCodeType)
                'impPartyPC.EmploymentStatusCode = msgParty.EmploymentStatusCode
                impPartyPC.EmployersBusinessCode = msgParty.EmployersBusinessCode
                impPartyPC.AlternativeId = msgParty.AlternativeId
                impPartyPC.Currency = msgParty.Currency
                impParty = impPartyPC

            Else

                ' Process corporate client
                Dim msgParty As MessagingTypes.BasePartyCCType = DirectCast(NewBusinessRequest.Item, MessagingTypes.BasePartyCCType)
                Dim impPartyCC As New BaseImplementationTypes.BasePartyCCType

                impPartyCC.CompanyName = msgParty.CompanyName
                impPartyCC.BusinessCode = msgParty.BusinessCode

                impParty = impPartyCC

            End If

            ' Common to PC and CC
            impParty.BranchCode = NewBusinessRequest.Item.BranchCode
            impParty.TPUserCode = NewBusinessRequest.Item.TPUserCode
            impParty.TPIntroducer = NewBusinessRequest.Item.TPIntroducer

            ' Process the address structure
            If IsArray(NewBusinessRequest.Item.Addresses) = True Then

                iLBnd% = NewBusinessRequest.Item.Addresses.GetLowerBound(0)
                iUBnd% = NewBusinessRequest.Item.Addresses.GetUpperBound(0)

                ReDim impParty.Addresses(iUBnd%)

                For iCnt As Integer = iLBnd% To iUBnd%
                    msgAddress = NewBusinessRequest.Item.Addresses(iCnt%)
                    impAddress = New BaseImplementationTypes.BaseAddressType

                    impAddress.AddressLine1 = msgAddress.AddressLine1
                    impAddress.AddressLine2 = msgAddress.AddressLine2
                    impAddress.AddressLine3 = msgAddress.AddressLine3
                    impAddress.AddressLine4 = msgAddress.AddressLine4
                    impAddress.AddressTypeCode = CType([Enum].ToObject(GetType(MessagingTypes.AddressTypeType), msgAddress.AddressTypeCode), BaseImplementationTypes.AddressTypeType)
                    'impAddress.AddressTypeCode = msgAddress.AddressTypeCode
                    impAddress.CountryCode = msgAddress.CountryCode
                    impAddress.PostCode = msgAddress.PostCode

                    impParty.Addresses(iCnt%) = impAddress
                Next iCnt%

            End If

            ' Process the Contact structure
            If IsArray(NewBusinessRequest.Item.Contacts) = True Then

                iLBnd% = NewBusinessRequest.Item.Contacts.GetLowerBound(0)
                iUBnd% = NewBusinessRequest.Item.Contacts.GetUpperBound(0)

                ReDim impParty.Contacts(iUBnd%)

                For iCnt As Integer = iLBnd% To iUBnd%
                    msgContact = NewBusinessRequest.Item.Contacts(iCnt%)
                    impContact = New BaseImplementationTypes.BaseContactType

                    impContact.AreaCode = msgContact.AreaCode
                    impContact.ContactDetail = New BaseImplementationTypes.BaseContactDetailType
                    impContact.ContactDetail.Item = msgContact.ContactDetail.Item
                    impContact.ContactDetail.ItemElementName = CType([Enum].ToObject(GetType(MessagingTypes.ItemChoiceType), msgContact.ContactDetail.ItemElementName), BaseImplementationTypes.ItemChoiceType)
                    'impContact.ContactDetail.ItemElementName = msgContact.ContactDetail.ItemElementName
                    impContact.ContactTypeCode = CType([Enum].ToObject(GetType(MessagingTypes.ContactTypeType), msgContact.ContactTypeCode), BaseImplementationTypes.ContactTypeType)
                    'impContact.ContactTypeCode = msgContact.ContactTypeCode

                    impParty.Contacts(iCnt%) = impContact
                Next iCnt%

            End If

            impRequest.Party = impParty

        End If

        If NewBusinessRequest.Policy Is Nothing = False Then
            ' Process the Policy Structure
            impRequest.Policy = New BaseImplementationTypes.BaseQuoteRiskMsgType

            impRequest.Policy.BranchCode = NewBusinessRequest.Policy.BranchCode
            impRequest.Policy.CoverStartDate = NewBusinessRequest.Policy.CoverStartDate
            impRequest.Policy.CoverEndDate = NewBusinessRequest.Policy.CoverEndDate
            impRequest.Policy.Description = NewBusinessRequest.Policy.Description
            impRequest.Policy.InsuredName = NewBusinessRequest.Policy.InsuredName
            impRequest.Policy.ProductCode = NewBusinessRequest.Policy.ProductCode
            impRequest.Policy.QuoteRef = NewBusinessRequest.Policy.QuoteRef

            ' Process the Risks structure
            If IsArray(NewBusinessRequest.Policy.Risks) = True Then

                Dim msgRisk As MessagingTypes.BaseRiskType
                Dim impRisk As BaseImplementationTypes.BaseRiskType

                iLBnd% = NewBusinessRequest.Policy.Risks.GetLowerBound(0)
                iUBnd% = NewBusinessRequest.Policy.Risks.GetUpperBound(0)

                ReDim impRequest.Policy.Risks(iUBnd%)

                For iCnt As Integer = iLBnd% To iUBnd%
                    msgRisk = NewBusinessRequest.Policy.Risks(iCnt%)
                    impRisk = New BaseImplementationTypes.BaseRiskType

                    impRisk.DataModelCode = msgRisk.DataModelCode
                    impRisk.QuoteTimeStamp = msgRisk.QuoteTimeStamp
                    impRisk.RiskDescription = msgRisk.RiskDescription
                    impRisk.RiskTypeCode = msgRisk.RiskTypeCode
                    impRisk.RunDefaultRules = msgRisk.RunDefaultRules
                    impRisk.ScreenCode = msgRisk.ScreenCode
                    impRisk.XMLDataSet = msgRisk.XMLDataSet

                    impRequest.Policy.Risks(iCnt%) = DirectCast(impRisk, BaseImplementationTypes.BaseQuoteRiskMsgTypeRisks)

                Next iCnt%

            End If
        End If

        Try
            impResponse = oMessagingBusiness.NewBusiness(impRequest)
        Catch ex As ApplicationException

        End Try

        ' Return errors
        SAMFunc.ConvertSTSError(impResponse.STSError, msgResponse.STSErrorType)

        ' Return details
        If (msgResponse.STSErrorType Is Nothing) And IsArray(impResponse.Policy) Then
            msgResponse.Insured = New MessagingTypes.BaseAddPartyResponseType
            msgResponse.Policy = New MessagingTypes.BaseNBQuoteResponseTypePolicy

            msgResponse.Insured.PartyKey = impResponse.Insured.PartyKey
            msgResponse.Insured.Shortname = impResponse.Insured.Shortname
            msgResponse.Policy.PolicyID = impResponse.Policy(0).PolicyID
            msgResponse.Policy.PremiumDueGross = impResponse.Policy(0).PremiumDueGross
            msgResponse.Policy.PremiumDueNet = impResponse.Policy(0).PremiumDueNet
            msgResponse.Policy.PremiumDueTax = impResponse.Policy(0).PremiumDueTax
            msgResponse.Policy.QuoteRef = impResponse.Policy(0).QuoteRef
            msgResponse.Policy.TotalAnnualTax = impResponse.Policy(0).TotalAnnualTax

            ' Process the Risks structure
            If IsArray(impResponse.Policy(0).Risks) = True Then
                Dim msgRisk As MessagingTypes.BaseRiskResultType
                Dim impRisk As BaseImplementationTypes.BaseRiskResultType

                iLBnd% = impResponse.Policy(0).Risks.GetLowerBound(0)
                iUBnd% = impResponse.Policy(0).Risks.GetUpperBound(0)

                ReDim msgResponse.Policy.Risks(iUBnd%)

                For iCnt As Integer = iLBnd% To iUBnd%
                    impRisk = impResponse.Policy(0).Risks(iCnt%)
                    msgRisk = New MessagingTypes.BaseRiskResultType

                    msgRisk.PremiumDueGross = impRisk.PremiumDueGross
                    msgRisk.PremiumDueNet = impRisk.PremiumDueNet
                    msgRisk.PremiumDueTax = impRisk.PremiumDueTax
                    msgRisk.TotalAnnualTax = impRisk.TotalAnnualTax
                    msgRisk.RiskFolderID = impRisk.RiskFolderID
                    msgRisk.RiskID = impRisk.RiskID
                    msgRisk.XMLDataSet = SAMFunc.TransformDatasetPBtoSAM(impRisk.XMLDataSet)

                    msgResponse.Policy.Risks(iCnt%) = msgRisk

                Next iCnt%
            End If
        End If

        Return msgResponse

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function NewBusinessTransact(<XmlElement(elementName:="NBTransactRequest", Namespace:=ServiceNamespace)> ByVal NBTransactRequest As MessagingTypes.NBTransactRequestType) As <XmlElement(elementName:="NBTransactResponse", Namespace:=ServiceNamespace)> MessagingTypes.NBTransactResponseType

        Dim msgResponse As New MessagingTypes.NBTransactResponseType

        Dim oImpRequest As New MessagingImplementationTypes.NBTransactRequestType
        Dim oImpResponse As MessagingImplementationTypes.NBTransactResponseType

        Dim oMessagingBusiness As New MessagingBusiness

        ' Move the incoming data to the implementation request structure
        oImpRequest.QuoteRef = NBTransactRequest.QuoteRef
        If IsArray(NBTransactRequest.Risks) Then
            Dim iLBnd As Integer = NBTransactRequest.Risks.GetLowerBound(0)
            Dim iUbnd As Integer = NBTransactRequest.Risks.GetUpperBound(0)
            ReDim oImpRequest.Risks(iUbnd)
            For iCnt As Integer = iLBnd To iUbnd
                oImpRequest.Risks(iCnt).BranchCode = NBTransactRequest.Risks(iCnt).BranchCode
                oImpRequest.Risks(iCnt).DataModelCode = NBTransactRequest.Risks(iCnt).DataModelCode
                oImpRequest.Risks(iCnt).QuoteTimeStamp = NBTransactRequest.Risks(iCnt).QuoteTimeStamp
                oImpRequest.Risks(iCnt).RiskDescription = NBTransactRequest.Risks(iCnt).RiskDescription
                oImpRequest.Risks(iCnt).RiskTypeCode = NBTransactRequest.Risks(iCnt).RiskTypeCode
                oImpRequest.Risks(iCnt).RunDefaultRules = NBTransactRequest.Risks(iCnt).RunDefaultRules
                oImpRequest.Risks(iCnt).ScreenCode = NBTransactRequest.Risks(iCnt).ScreenCode
                oImpRequest.Risks(iCnt).XMLDataSet = NBTransactRequest.Risks(iCnt).XMLDataSet
            Next
        End If

        ' Call the implementation method
        oImpResponse = oMessagingBusiness.NBTransact(oImpRequest)

        ' Return errors
        SAMFunc.ConvertSTSError(oImpResponse.STSError, msgResponse.STSErrorType)

        ' Return details
        If msgResponse.STSErrorType Is Nothing Then
            msgResponse.Policy = New MessagingTypes.BaseTransactResponseTypePolicy
            msgResponse.Policy.PolicyRef = oImpResponse.Policy.PolicyRef
            msgResponse.Policy.PremiumDueGross = oImpResponse.Policy.PremiumDueGross
            msgResponse.Policy.PremiumDueNet = oImpResponse.Policy.PremiumDueNet
            msgResponse.Policy.PremiumDueTax = oImpResponse.Policy.PremiumDueTax
            msgResponse.Policy.TotalAnnualTax = oImpResponse.Policy.TotalAnnualTax
            msgResponse.Policy.CommissionAmount = oImpResponse.Policy.CommissionAmount
        End If

        Return msgResponse

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetDatasetSchema(<XmlElement(elementName:="GetDatasetSchemaRequest", Namespace:=ServiceNamespace)> ByVal GetDatasetSchemaRequest As MessagingTypes.BaseGetDatasetSchemaRequestType) As <XmlElement(elementName:="GetDatasetSchemaResponse", Namespace:=ServiceNamespace)> MessagingTypes.BaseGetDatasetSchemaResponseType

        Dim msgResponse As New MessagingTypes.BaseGetDatasetSchemaResponseType

        Dim oImpRequest As New BaseImplementationTypes.BaseGetDatasetSchemaRequestType
        Dim oImpResponse As BaseImplementationTypes.BaseGetDatasetSchemaResponseType

        Dim oMessagingBusiness As New MessagingBusiness

        oImpRequest.BranchCode = GetDatasetSchemaRequest.BranchCode
        oImpRequest.DataModelCode = GetDatasetSchemaRequest.DataModelCode

        oImpResponse = oMessagingBusiness.GetDatasetSchema(oImpRequest)

        ' Return errors
        SAMFunc.ConvertSTSError(oImpResponse.STSError, msgResponse.STSErrorType)

        ' Return details
        If msgResponse.STSErrorType Is Nothing Then
            msgResponse.DatasetSchema = oImpResponse.DatasetSchema
        End If

        Return msgResponse

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function ProcessClaim(<XmlElement(elementName:="ClaimProcessRequest", Namespace:=ServiceNamespace)> ByVal oClaimProcessRequest As MessagingTypes.BaseClaimProcessRequestType) As <XmlElement(elementName:="ClaimProcessResponse", Namespace:=ServiceNamespace)> MessagingTypes.BaseClaimProcessResponseType

        'CheckAuthority("SAMPROCLM")

        Dim msgResponse As New MessagingTypes.BaseClaimProcessResponseType

        Dim oImpRequest As New BaseImplementationTypes.BaseClaimProcessRequestType
        Dim oImpResponse As BaseImplementationTypes.BaseClaimProcessResponseType
        Dim oMessagingBusiness As New MessagingBusiness

        Try
            oImpRequest.BranchCode = oClaimProcessRequest.BranchCode
            oImpRequest.Claim = New BaseImplementationTypes.BaseClaimProcessType
            oImpRequest.Claim.BaseClaimKey = oClaimProcessRequest.Claim.BaseClaimKey
            oImpRequest.ClaimNumber = oClaimProcessRequest.Claim.ClaimNumber
            oImpRequest.Claim.CatastropheCode = oClaimProcessRequest.Claim.CatastropheCode

            oImpRequest.Claim.ClaimStatus = oClaimProcessRequest.Claim.ClaimStatus
            oImpRequest.Claim.ClaimStatusDate = oClaimProcessRequest.Claim.ClaimStatusDate
            oImpRequest.Claim.ClaimVersion = oClaimProcessRequest.Claim.ClaimVersion
            oImpRequest.Claim.ClaimVersionDescription = oClaimProcessRequest.Claim.ClaimVersionDescription

            oImpRequest.Claim.ClientEmail = oClaimProcessRequest.Claim.ClientEmail
            oImpRequest.Claim.ClientFaxNo = oClaimProcessRequest.Claim.ClientFaxNo
            oImpRequest.Claim.ClientMobileNo = oClaimProcessRequest.Claim.ClientMobileNo
            oImpRequest.Claim.ClientName = oClaimProcessRequest.Claim.ClientName

            oImpRequest.Claim.ClientTelNo = oClaimProcessRequest.Claim.ClientTelNo
            oImpRequest.Claim.Comments = oClaimProcessRequest.Claim.Comments
            oImpRequest.Claim.CurrencyCode = oClaimProcessRequest.Claim.CurrencyCode

            oImpRequest.Claim.Description = oClaimProcessRequest.Claim.Description
            oImpRequest.Claim.ExternalHandler = oClaimProcessRequest.Claim.ExternalHandler
            oImpRequest.Claim.HandlerCode = oClaimProcessRequest.Claim.HandlerCode
            oImpRequest.Claim.IgnoreWarnings = oClaimProcessRequest.Claim.IgnoreWarnings
            oImpRequest.Claim.InfoOnly = oClaimProcessRequest.Claim.InfoOnly

            oImpRequest.Claim.InsuranceFileKey = oClaimProcessRequest.Claim.InsuranceFileKey
            oImpRequest.Claim.LastModifiedDate = oClaimProcessRequest.Claim.LastModifiedDate
            oImpRequest.Claim.LikelyClaim = oClaimProcessRequest.Claim.LikelyClaim
            oImpRequest.Claim.LossFromDate = oClaimProcessRequest.Claim.LossFromDate

            If oImpRequest.Claim.LossToDateSpecified = False Then
                oImpRequest.Claim.LossToDate = oClaimProcessRequest.Claim.LossFromDate
                oImpRequest.Claim.LossToDateSpecified = True
            Else
                oImpRequest.Claim.LossToDate = oClaimProcessRequest.Claim.LossToDate
                oImpRequest.Claim.LossToDateSpecified = oClaimProcessRequest.Claim.LossToDateSpecified
            End If

            oImpRequest.Claim.PrimaryCauseCode = oClaimProcessRequest.Claim.PrimaryCauseCode
            oImpRequest.Claim.ProgressStatusCode = oClaimProcessRequest.Claim.ProgressStatusCode
            oImpRequest.Claim.ReportedDate = oClaimProcessRequest.Claim.ReportedDate
            oImpRequest.Claim.RiskKey = oClaimProcessRequest.Claim.RiskKey
            oImpRequest.Claim.SecondaryCauseCode = oClaimProcessRequest.Claim.SecondaryCauseCode

            If Not oClaimProcessRequest.Claim.ClaimBuilderDetail Is Nothing Then
                ReDim oImpRequest.Claim.ClaimBuilderDetail(oClaimProcessRequest.Claim.ClaimBuilderDetail.Length - 1)
                For lCount As Int32 = 0 To oClaimProcessRequest.Claim.ClaimBuilderDetail.Length - 1
                    oImpRequest.Claim.ClaimBuilderDetail(lCount) = New BaseImplementationTypes.BaseClaimProcessBuilderRiskType
                    oImpRequest.Claim.ClaimBuilderDetail(lCount).ClaimBuilderData = New BaseImplementationTypes.BaseClaimProcessBuilderRiskTypeClaimBuilderData
                    oImpRequest.Claim.ClaimBuilderDetail(lCount).ClaimBuilderData.ItemName = oClaimProcessRequest.Claim.ClaimBuilderDetail(lCount).ClaimBuilderData.ItemName
                    oImpRequest.Claim.ClaimBuilderDetail(lCount).ClaimBuilderData.Value = oClaimProcessRequest.Claim.ClaimBuilderDetail(lCount).ClaimBuilderData.Value
                Next
            End If
            If Not oClaimProcessRequest.Claim.ClaimPeril Is Nothing Then
                ReDim oImpRequest.Claim.ClaimPeril(oClaimProcessRequest.Claim.ClaimPeril.Length - 1)
                For lCount As Int32 = 0 To oClaimProcessRequest.Claim.ClaimPeril.Length - 1
                    oImpRequest.Claim.ClaimPeril(lCount) = New BaseImplementationTypes.BaseClaimProcessPerilType

                    If oClaimProcessRequest.Claim.ClaimPeril(lCount).Description = String.Empty Then
                        oImpRequest.Claim.ClaimPeril(lCount).Description = oClaimProcessRequest.Claim.ClaimPeril(lCount).TypeCode
                    Else
                        oImpRequest.Claim.ClaimPeril(lCount).Description = oClaimProcessRequest.Claim.ClaimPeril(lCount).Description
                    End If
                    If Not oClaimProcessRequest.Claim.ClaimPeril(lCount).Recovery Is Nothing Then
                        ReDim oImpRequest.Claim.ClaimPeril(lCount).Recovery(oClaimProcessRequest.Claim.ClaimPeril(lCount).Recovery.Length - 1)
                        For lRCount As Int32 = 0 To oClaimProcessRequest.Claim.ClaimPeril(lCount).Recovery.Length - 1
                            oImpRequest.Claim.ClaimPeril(lCount).Recovery(lRCount) = New BaseImplementationTypes.BaseClaimProcessPerilRecoveryType
                            oImpRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).Amount = oClaimProcessRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).Amount
                            oImpRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).RecoveryPartyCode = oClaimProcessRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).RecoveryPartyCode
                            oImpRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).RecoveryPartyTypeCode = oClaimProcessRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).RecoveryPartyTypeCode
                            oImpRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).TypeCode = oClaimProcessRequest.Claim.ClaimPeril(lCount).Recovery(lRCount).TypeCode
                        Next
                    End If
                    If Not oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve Is Nothing Then
                        ReDim oImpRequest.Claim.ClaimPeril(lCount).Reserve(oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve.Length - 1)
                        For lRSCount As Int32 = 0 To oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve.Length - 1
                            oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount) = New BaseImplementationTypes.BaseClaimProcessPerilReserveType
                            oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).Amount = oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).Amount
                            oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentAmount = oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentAmount
                            oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentAmountSpecified = oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentAmountSpecified
                            If Not oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails Is Nothing Then
                                oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails = New BaseImplementationTypes.BaseClaimProcessPaymentDetailsType
                                oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.PaymentBankCode = oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.PaymentBankCode
                                ' If no MediaType passed in then default to Cheque
                                If oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.PaymentMediaTypeCode = String.Empty Then
                                    oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.PaymentMediaTypeCode = "CQ"
                                Else
                                    oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.PaymentMediaTypeCode = oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.PaymentMediaTypeCode
                                End If
                                oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.PaymentMediaReference = oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.PaymentMediaReference
                                oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.PaymentPayee = oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).PaymentDetails.PaymentPayee
                            End If
                            oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).ReverseExcess = oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).ReverseExcess
                            oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).TaxGroupCode = oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).TaxGroupCode
                            oImpRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).TypeCode = oClaimProcessRequest.Claim.ClaimPeril(lCount).Reserve(lRSCount).TypeCode
                        Next
                    End If
                    oImpRequest.Claim.ClaimPeril(lCount).TypeCode = oClaimProcessRequest.Claim.ClaimPeril(lCount).TypeCode
                Next
            End If
            'Calling base function
            oImpResponse = oMessagingBusiness.ProcessClaim(oImpRequest)

            If oImpResponse.Errors Is Nothing = False Then
                If (oImpResponse.Errors.Length > 0) Then
                    msgResponse.STSErrorType = New MessagingTypes.STSErrorType
                    For lCount As Int32 = 0 To oImpResponse.Errors.Length - 1
                        If TypeOf (oImpResponse.Errors(lCount)) Is BaseImplementationTypes.SAMErrorInvalidData Then
                            Dim SAMErr As BaseImplementationTypes.SAMErrorInvalidData = CType(oImpResponse.Errors(lCount), BaseImplementationTypes.SAMErrorInvalidData)
                            Dim STSErr As New MessagingTypes.STSErrorInvalidDataType
                            STSErr.Code = SAMErr.Code.ToString
                            STSErr.Description = SAMErr.Description
                            STSErr.FieldName = SAMErr.FieldName
                            STSErr.SuppliedValue = SAMErr.SuppliedValue
                            If msgResponse.STSErrorType.InvalidData Is Nothing Then
                                ReDim msgResponse.STSErrorType.InvalidData(0)
                            Else
                                ReDim Preserve msgResponse.STSErrorType.InvalidData(msgResponse.STSErrorType.InvalidData.Length)
                            End If
                            msgResponse.STSErrorType.InvalidData(msgResponse.STSErrorType.InvalidData.Length - 1) = STSErr
                        ElseIf TypeOf (oImpResponse.Errors(lCount)) Is BaseImplementationTypes.SAMErrorBusinessRule Then
                            Dim SAMErr As BaseImplementationTypes.SAMErrorBusinessRule = CType(oImpResponse.Errors(lCount), BaseImplementationTypes.SAMErrorBusinessRule)
                            Dim STSErr As New MessagingTypes.STSErrorSTSBusinessRuleType
                            STSErr.Code = SAMErr.Code.ToString
                            STSErr.Description = SAMErr.Description
                            STSErr.Detail = SAMErr.Detail
                            msgResponse.STSErrorType.STSBusinessRule = STSErr
                        ElseIf TypeOf (oImpResponse.Errors(lCount)) Is BaseImplementationTypes.SAMErrorFatal Then
                            Dim SAMErr As BaseImplementationTypes.SAMErrorFatal = CType(oImpResponse.Errors(lCount), BaseImplementationTypes.SAMErrorFatal)
                            Dim STSErr As New MessagingTypes.STSErrorInternalExceptionType
                            STSErr.Description = SAMErr.Type
                            msgResponse.STSErrorType.InternalException = STSErr
                        End If
                    Next
                End If
            End If

            ' Return details
            If msgResponse.STSErrorType Is Nothing Then
                msgResponse.BaseClaimKey = oImpResponse.BaseClaimKey
                msgResponse.ClaimKey = oImpResponse.ClaimKey
                msgResponse.ClaimNumber = oImpResponse.ClaimNumber
                If Not oImpResponse.Warnings Is Nothing Then
                    ReDim msgResponse.Warnings(oImpResponse.Warnings.Length - 1)
                    For lCount As Int32 = 0 To oImpResponse.Warnings.Length - 1
                        msgResponse.Warnings(lCount) = New MessagingTypes.BaseClaimProcessResponseTypeWarnings
                        msgResponse.Warnings(lCount).Code = oImpResponse.Warnings(lCount).Code
                        msgResponse.Warnings(lCount).Description = oImpResponse.Warnings(lCount).Description
                    Next
                End If
                msgResponse.ResultingStatus = oImpResponse.ResultingStatus
                msgResponse.TimeStamp = oImpResponse.TimeStamp
                msgResponse.Version = oImpResponse.Version
            End If

            Return msgResponse

        Catch SAMException As Sirius.Architecture.ExceptionHandling.SAMErrorException
            If SAMException.Errors Is Nothing = False Then
                If (SAMException.Errors.Count > 0) Then
                    msgResponse.STSErrorType = New MessagingTypes.STSErrorType
                    For lCount As Int32 = 0 To SAMException.Errors.Count - 1
                        If TypeOf (SAMException.Errors(lCount)) Is Sirius.Architecture.ExceptionHandling.SAMErrorInvalidData Then
                            Dim SAMErr As Sirius.Architecture.ExceptionHandling.SAMErrorInvalidData = CType(SAMException.Errors(lCount), Sirius.Architecture.ExceptionHandling.SAMErrorInvalidData)
                            Dim STSErr As New MessagingTypes.STSErrorInvalidDataType
                            STSErr.Code = SAMErr.Code.ToString
                            STSErr.Description = SAMErr.Description
                            STSErr.FieldName = SAMErr.FieldName
                            STSErr.SuppliedValue = SAMErr.SuppliedValue
                            If msgResponse.STSErrorType.InvalidData Is Nothing Then
                                ReDim msgResponse.STSErrorType.InvalidData(0)
                            Else
                                ReDim Preserve msgResponse.STSErrorType.InvalidData(msgResponse.STSErrorType.InvalidData.Length)
                            End If
                            msgResponse.STSErrorType.InvalidData(msgResponse.STSErrorType.InvalidData.Length - 1) = STSErr
                        ElseIf TypeOf (SAMException.Errors(lCount)) Is Sirius.Architecture.ExceptionHandling.SAMErrorBusinessRule Then
                            Dim SAMErr As Sirius.Architecture.ExceptionHandling.SAMErrorBusinessRule = CType(SAMException.Errors(lCount), Sirius.Architecture.ExceptionHandling.SAMErrorBusinessRule)
                            Dim STSErr As New MessagingTypes.STSErrorSTSBusinessRuleType
                            STSErr.Code = SAMErr.Code.ToString
                            STSErr.Description = SAMErr.Description
                            STSErr.Detail = SAMErr.Detail
                            msgResponse.STSErrorType.STSBusinessRule = STSErr
                        ElseIf TypeOf (SAMException.Errors(lCount)) Is Sirius.Architecture.ExceptionHandling.SAMErrorFatal Then
                            Dim SAMErr As Sirius.Architecture.ExceptionHandling.SAMErrorFatal = CType(SAMException.Errors(lCount), Sirius.Architecture.ExceptionHandling.SAMErrorFatal)
                            Dim STSErr As New MessagingTypes.STSErrorInternalExceptionType
                            STSErr.Description = SAMErr.Type
                            msgResponse.STSErrorType.InternalException = STSErr
                        End If
                    Next
                End If
            End If
            Return msgResponse
        Catch ex As Exception
            Dim STSErr As New MessagingTypes.STSErrorInternalExceptionType
            STSErr.Description = ex.Message
            msgResponse.STSErrorType = New MessagingTypes.STSErrorType
            msgResponse.STSErrorType.InternalException = STSErr
            Return msgResponse
        End Try

    End Function

    ''' <summary>
    ''' Check whether the current user has the authority to perform a task.
    ''' </summary>
    ''' <param name="sTaskCode">The PMWrk_Task code to check for.</param>
    ''' <exception cref="AuthorisationException">The user does not have the authority.</exception>
    Private Sub CheckAuthority(ByVal sTaskCode As String)

        Dim principal As SiriusPrincipal = SiriusPrincipal.ToSiriusPrincipal(RequestSoapContext.Current.IdentityToken.Principal)

        If Not principal.IsInRole(sTaskCode) Then
            Throw New AuthorisationException(sTaskCode)
        End If

    End Sub

    Private Shared Sub ConvertSAMErrorToSTSResponse(ByVal msgResponse As MessagingTypes.BaseResponseType, ByVal SAMError As Sirius.Architecture.ExceptionHandling.SAMErrorException)
        If SAMError.Errors Is Nothing = False Then
            If (SAMError.Errors.Count > 0) Then
                msgResponse.STSErrorType = New MessagingTypes.STSErrorType
                For lCount As Int32 = 0 To SAMError.Errors.Count - 1
                    If TypeOf (SAMError.Errors(lCount)) Is SAMErrorInvalidData Then
                        Dim SAMErr As SAMErrorInvalidData = CType(SAMError.Errors(lCount), SAMErrorInvalidData)
                        Dim STSErr As New MessagingTypes.STSErrorInvalidDataType
                        STSErr.Code = SAMErr.Code.ToString
                        STSErr.Description = SAMErr.Description
                        STSErr.FieldName = SAMErr.FieldName
                        STSErr.SuppliedValue = SAMErr.SuppliedValue
                        If msgResponse.STSErrorType.InvalidData Is Nothing Then
                            ReDim msgResponse.STSErrorType.InvalidData(0)
                        Else
                            ReDim Preserve msgResponse.STSErrorType.InvalidData(msgResponse.STSErrorType.InvalidData.Length)
                        End If
                        msgResponse.STSErrorType.InvalidData(msgResponse.STSErrorType.InvalidData.Length - 1) = STSErr
                    ElseIf TypeOf (SAMError.Errors(lCount)) Is SAMErrorBusinessRule Then
                        Dim SAMErr As SAMErrorBusinessRule = CType(SAMError.Errors(lCount), SAMErrorBusinessRule)
                        Dim STSErr As New MessagingTypes.STSErrorSTSBusinessRuleType
                        STSErr.Code = SAMErr.Code.ToString
                        STSErr.Description = SAMErr.Description
                        STSErr.Detail = SAMErr.Detail
                        msgResponse.STSErrorType.STSBusinessRule = STSErr
                    ElseIf TypeOf (SAMError.Errors(lCount)) Is SAMErrorFatal Then
                        Dim SAMErr As SAMErrorFatal = CType(SAMError.Errors(lCount), SAMErrorFatal)
                        Dim STSErr As New MessagingTypes.STSErrorInternalExceptionType
                        STSErr.Description = SAMErr.Type
                        msgResponse.STSErrorType.InternalException = STSErr
                    End If
                Next
            End If
        End If
    End Sub

    <WebMethod()> _
<SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function PolicyProcess(<XmlElement(elementName:="PolicyProcessRequest", Namespace:=ServiceNamespace)> ByVal PolicyProcessRequest As MessagingTypes.PolicyProcessRequestType) As <XmlElement(elementName:="PolicyProcessResponse", Namespace:=ServiceNamespace)> MessagingTypes.PolicyProcessResponseType

        Dim iLBnd As Integer
        Dim iUBnd As Integer

        Dim msgResponse As New MessagingTypes.PolicyProcessResponseType

        Dim oMessagingBusiness As New MessagingBusiness

        ' Implementation structures
        Dim impRequest As New MessagingImplementationTypes.PolicyProcessRequestType

        Dim impResponse As MessagingImplementationTypes.PolicyProcessResponseType = Nothing

        Try

            ' Convert the incoming interface structures into the implementation structures
            impRequest.AgentCode = PolicyProcessRequest.AgentCode
            impRequest.BranchCode = PolicyProcessRequest.BranchCode
            impRequest.CurrencyCode = CType([Enum].ToObject(GetType(MessagingTypes.CurrencyType), PolicyProcessRequest.CurrencyCode), BaseImplementationTypes.CurrencyType)
            impRequest.CurrencyCodeSpecified = True
            impRequest.UpdateParty = PolicyProcessRequest.UpdateParty

            ' Process the Party structure.  We 1st need to check the party type of the incoming message
            If (PolicyProcessRequest.Item Is Nothing) = False Then

                Dim impParty As New BaseImplementationTypes.BasePartyType
                Dim impAddress As New BaseImplementationTypes.BaseAddressType
                Dim impContact As New BaseImplementationTypes.BaseContactType

                Dim msgAddress As MessagingTypes.BaseAddressType
                Dim msgContact As MessagingTypes.BaseContactType

                ' Check the type of the party object to see if it is Personal or Corporate
                If PolicyProcessRequest.Item.GetType Is GetType(MessagingTypes.BasePartyPCType) Then

                    ' Process personal client
                    Dim msgParty As MessagingTypes.BasePartyPCType = DirectCast(PolicyProcessRequest.Item, MessagingTypes.BasePartyPCType)
                    Dim impPartyPC As New BaseImplementationTypes.BasePartyPCType

                    impPartyPC.Forename = msgParty.Forename
                    impPartyPC.Surname = msgParty.Surname
                    impPartyPC.Initials = msgParty.Initials
                    impPartyPC.Title = msgParty.Title
                    impPartyPC.DateOfBirth = msgParty.DateOfBirth
                    impPartyPC.GenderCode = msgParty.GenderCode
                    impPartyPC.MaritalStatusCode = CType([Enum].ToObject(GetType(MessagingTypes.MaritalStatusCodeType), msgParty.MaritalStatusCode), BaseImplementationTypes.MaritalStatusCodeType)
                    'impPartyPC.MaritalStatusCode = msgParty.MaritalStatusCode
                    impPartyPC.OccupationCode = msgParty.OccupationCode
                    impPartyPC.EmploymentStatusCode = CType([Enum].ToObject(GetType(MessagingTypes.EmploymentStatusCodeType), msgParty.EmploymentStatusCode), BaseImplementationTypes.EmploymentStatusCodeType)
                    'impPartyPC.EmploymentStatusCode = msgParty.EmploymentStatusCode
                    impPartyPC.EmployersBusinessCode = msgParty.EmployersBusinessCode
                    impPartyPC.AlternativeId = msgParty.AlternativeId
                    impPartyPC.Currency = msgParty.Currency
                    impParty = impPartyPC

                Else

                    ' Process corporate client
                    Dim msgParty As MessagingTypes.BasePartyCCType = DirectCast(PolicyProcessRequest.Item, MessagingTypes.BasePartyCCType)
                    Dim impPartyCC As New BaseImplementationTypes.BasePartyCCType

                    impPartyCC.CompanyName = msgParty.CompanyName
                    impPartyCC.MainContact = msgParty.MainContact
                    impPartyCC.BusinessCode = msgParty.BusinessCode

                    impParty = impPartyCC

                End If

                ' Common to PC and CC
                impParty.BranchCode = PolicyProcessRequest.Item.BranchCode
                impParty.TPUserCode = PolicyProcessRequest.Item.TPUserCode
                impParty.TPIntroducer = PolicyProcessRequest.Item.TPIntroducer


                ' Process the address structure
                If IsArray(PolicyProcessRequest.Item.Addresses) = True Then

                    iLBnd% = PolicyProcessRequest.Item.Addresses.GetLowerBound(0)
                    iUBnd% = PolicyProcessRequest.Item.Addresses.GetUpperBound(0)

                    ReDim impParty.Addresses(iUBnd%)

                    For iCnt As Integer = iLBnd% To iUBnd%
                        msgAddress = PolicyProcessRequest.Item.Addresses(iCnt%)
                        impAddress = New BaseImplementationTypes.BaseAddressType

                        impAddress.AddressLine1 = msgAddress.AddressLine1
                        impAddress.AddressLine2 = msgAddress.AddressLine2
                        impAddress.AddressLine3 = msgAddress.AddressLine3
                        impAddress.AddressLine4 = msgAddress.AddressLine4
                        impAddress.AddressTypeCode = CType([Enum].ToObject(GetType(MessagingTypes.AddressTypeType), msgAddress.AddressTypeCode), BaseImplementationTypes.AddressTypeType)
                        'impAddress.AddressTypeCode = msgAddress.AddressTypeCode
                        impAddress.CountryCode = msgAddress.CountryCode
                        impAddress.PostCode = msgAddress.PostCode

                        impParty.Addresses(iCnt%) = impAddress
                    Next iCnt%

                End If

                ' Process the Contact structure
                If IsArray(PolicyProcessRequest.Item.Contacts) = True Then

                    iLBnd% = PolicyProcessRequest.Item.Contacts.GetLowerBound(0)
                    iUBnd% = PolicyProcessRequest.Item.Contacts.GetUpperBound(0)

                    ReDim impParty.Contacts(iUBnd%)

                    For iCnt As Integer = iLBnd% To iUBnd%
                        msgContact = PolicyProcessRequest.Item.Contacts(iCnt%)
                        impContact = New BaseImplementationTypes.BaseContactType

                        impContact.AreaCode = msgContact.AreaCode
                        impContact.ContactDetail = New BaseImplementationTypes.BaseContactDetailType
                        impContact.ContactDetail.Item = msgContact.ContactDetail.Item
                        impContact.ContactDetail.ItemElementName = CType([Enum].ToObject(GetType(MessagingTypes.ItemChoiceType), msgContact.ContactDetail.ItemElementName), BaseImplementationTypes.ItemChoiceType)
                        'impContact.ContactDetail.ItemElementName = msgContact.ContactDetail.ItemElementName
                        impContact.ContactTypeCode = CType([Enum].ToObject(GetType(MessagingTypes.ContactTypeType), msgContact.ContactTypeCode), BaseImplementationTypes.ContactTypeType)
                        'impContact.ContactTypeCode = msgContact.ContactTypeCode

                        impParty.Contacts(iCnt%) = impContact
                    Next iCnt%

                End If

                impRequest.Party = impParty

            End If

            If PolicyProcessRequest.Policy Is Nothing = False Then
                ' Process the Policy Structure
                impRequest.Policy = New BaseImplementationTypes.BaseQuoteRiskMsgType

                impRequest.Policy.BranchCode = PolicyProcessRequest.Policy.BranchCode
                impRequest.Policy.CoverStartDate = PolicyProcessRequest.Policy.CoverStartDate
                impRequest.Policy.CoverEndDate = PolicyProcessRequest.Policy.CoverEndDate
                impRequest.Policy.Description = PolicyProcessRequest.Policy.Description
                impRequest.Policy.InsuredName = PolicyProcessRequest.Policy.InsuredName
                impRequest.Policy.ProductCode = PolicyProcessRequest.Policy.ProductCode
                impRequest.Policy.QuoteRef = PolicyProcessRequest.Policy.QuoteRef
                impRequest.Policy.CurrencyCode = PolicyProcessRequest.Policy.CurrencyCode
                impRequest.Policy.PolicyStatusCode = PolicyProcessRequest.Policy.PolicyStatusCode
                impRequest.Policy.TransactionTypeCode = PolicyProcessRequest.Policy.TransactionTypeCode
                impRequest.Policy.AlternateReference = PolicyProcessRequest.Policy.AlternateReference
                impRequest.Policy.AlternativeRef = PolicyProcessRequest.Policy.AlternateReference
                impRequest.Policy.NewQuoteRef = PolicyProcessRequest.Policy.NewQuoteRef
                impRequest.Policy.CommissionRate = PolicyProcessRequest.Policy.CommissionRate
                impRequest.Policy.CommissionValue = PolicyProcessRequest.Policy.CommissionValue
                impRequest.Policy.TransactionDueDate = PolicyProcessRequest.Policy.TransactionDueDate
                impRequest.Policy.OldPolicyNumber = PolicyProcessRequest.Policy.OldPolicyNumber
                impRequest.Policy.LastTransDescription = PolicyProcessRequest.Policy.LastTransDescription
                impRequest.Policy.AnalysisCode = PolicyProcessRequest.Policy.AnalysisCode
                impRequest.Policy.BusinessTypeCode = IIf(ToSafeString(PolicyProcessRequest.AgentCode, "") <> "", "AGENCY", "DIRECT").ToString
                If (PolicyProcessRequest.Policy.MTAReasonCode Is Nothing) Then
                    impRequest.Policy.MTAReasonCode = "OTHER"
                Else
                    impRequest.Policy.MTAReasonCode = PolicyProcessRequest.Policy.MTAReasonCode
                End If
                impRequest.Policy.UnderwritingYearCode = PolicyProcessRequest.Policy.UnderwritingYearCode

                impRequest.Policy.DoNotCopyRiskAtRenSelection = PolicyProcessRequest.Policy.DoNotCopyRiskAtRenSelection
                impRequest.Policy.DeletePolicyUnderRenewal = PolicyProcessRequest.Policy.DeletePolicyUnderRenewal

                impRequest.Policy.PartyKey = PolicyProcessRequest.Policy.PartyKey


                If IsArray(PolicyProcessRequest.Policy.Taxes) = True Then
                    impRequest.Policy.Taxes = Array.ConvertAll(PolicyProcessRequest.Policy.Taxes, _
                                        New Converter(Of MessagingTypes.BaseTaxesType, BaseImplementationTypes.BaseTaxesType) _
                                        (AddressOf ToBaseImpBaseTaxesAndFeesRiskType))
                End If
                ' Process the Risks structure
                If IsArray(PolicyProcessRequest.Policy.Risks) = True Then

                    impRequest.Policy.Risks = Array.ConvertAll(PolicyProcessRequest.Policy.Risks, _
                                New Converter(Of MessagingTypes.BaseRiskType, BaseImplementationTypes.BaseQuoteRiskMsgTypeRisks) _
                                (AddressOf ToBaseImpBaseQuoteRiskMsgTypeRisks))

                End If
            End If

            impResponse = oMessagingBusiness.PolicyProcess(impRequest)

            ' Return errors
            SAMFunc.ConvertSTSError(impResponse.STSError, msgResponse.STSErrorType)

            ' Return details
            If (msgResponse.STSErrorType Is Nothing) And IsArray(impResponse.Policy) Then
                msgResponse.Insured = New MessagingTypes.BaseAddPartyResponseType
                msgResponse.Policy = New MessagingTypes.BaseNBQuoteResponseTypePolicy
                If Not impResponse.Insured Is Nothing Then
                    msgResponse.Insured.PartyKey = impResponse.Insured.PartyKey
                    msgResponse.Insured.Shortname = impResponse.Insured.Shortname
                End If
                msgResponse.Policy.PolicyID = impResponse.Policy(0).PolicyID
                msgResponse.Policy.PremiumDueGross = impResponse.Policy(0).PremiumDueGross
                msgResponse.Policy.PremiumDueNet = impResponse.Policy(0).PremiumDueNet
                msgResponse.Policy.PremiumDueTax = impResponse.Policy(0).PremiumDueTax
                msgResponse.Policy.QuoteRef = impResponse.Policy(0).QuoteRef
                msgResponse.Policy.TotalAnnualTax = impResponse.Policy(0).TotalAnnualTax

                ' Process the Risks structure
                If IsArray(impResponse.Policy(0).Risks) = True Then

                    msgResponse.Policy.Risks = Array.ConvertAll(impResponse.Policy(0).Risks, _
                                New Converter(Of BaseImplementationTypes.BaseRiskResultType, MessagingTypes.BaseRiskResultType) _
                                (AddressOf ToServiceBaseRiskResultType))

                End If
            End If

            Return msgResponse

        Catch SAMError As Sirius.Architecture.ExceptionHandling.SAMErrorException

            ConvertSAMErrorToSTSResponse(msgResponse, SAMError)

            Return msgResponse

        Catch ex As Exception
            Dim STSErrorFileError As New STSErrorPublisher("An error occured when processing policy.", ex)
            STSErrorFileError.Raise(HttpContext.Current.Request.Url.ToString(), "PolicyProcess", "PolicyProcess", True)

        End Try

        Return msgResponse

    End Function

    Private Function ToServiceBaseRiskResultType(ByVal impRisk As BaseImplementationTypes.BaseRiskResultType) As MessagingTypes.BaseRiskResultType

        Dim msgRisk As New MessagingTypes.BaseRiskResultType

        If msgRisk IsNot Nothing Then

            msgRisk.PremiumDueGross = impRisk.PremiumDueGross
            msgRisk.PremiumDueNet = impRisk.PremiumDueNet
            msgRisk.PremiumDueTax = impRisk.PremiumDueTax
            msgRisk.TotalAnnualTax = impRisk.TotalAnnualTax
            msgRisk.CommissionAmount = impRisk.CommissionAmount
            msgRisk.RiskFolderID = impRisk.RiskFolderID
            msgRisk.RiskID = impRisk.RiskID
            msgRisk.XMLDataSet = SAMFunc.TransformDatasetPBtoSAM(impRisk.XMLDataSet)

        End If

        Return msgRisk

    End Function

    Private Function ToBaseImpBaseQuoteRiskMsgTypeRisks(ByVal msgRisk As MessagingTypes.BaseRiskType) As BaseImplementationTypes.BaseQuoteRiskMsgTypeRisks

        Dim impRisk As New BaseImplementationTypes.BaseQuoteRiskMsgTypeRisks

        If msgRisk IsNot Nothing Then

            impRisk.DataModelCode = msgRisk.DataModelCode
            impRisk.QuoteTimeStamp = msgRisk.QuoteTimeStamp
            impRisk.RiskDescription = msgRisk.RiskDescription
            impRisk.RiskTypeCode = msgRisk.RiskTypeCode
            impRisk.RunDefaultRules = msgRisk.RunDefaultRules
            impRisk.ScreenCode = msgRisk.ScreenCode
            impRisk.RiskFolderKey = msgRisk.RiskFolderKey
            impRisk.RiskFolderKeySpecified = msgRisk.RiskFolderKeySpecified
            impRisk.XMLDataSet = msgRisk.XMLDataSet
            ' Process the Product Builder structure
            If IsArray(msgRisk.ProductBuilderDetail) = True Then

                impRisk.ProductBuilderDetail = Array.ConvertAll(msgRisk.ProductBuilderDetail, _
                            New Converter(Of MessagingTypes.BaseProductBuilderRiskType, BaseImplementationTypes.BaseProductBuilderRiskType) _
                            (AddressOf ToBaseImpBaseProductBuilderRiskType))

            End If

            ' Process the Tax structure
            If IsArray(msgRisk.Taxes) = True Then

                ReDim impRisk.TaxesAndFees(0)

                impRisk.TaxesAndFees(0) = New BaseImplementationTypes.BaseTaxesAndFeesType

                impRisk.TaxesAndFees(0).Taxes = Array.ConvertAll(msgRisk.Taxes, _
                            New Converter(Of MessagingTypes.BaseTaxesType, BaseImplementationTypes.BaseTaxesType) _
                            (AddressOf ToBaseImpBaseTaxesAndFeesRiskType))

            End If

        End If

        Return impRisk

    End Function

    Private Function ToBaseImpBaseProductBuilderRiskType(ByVal msgStruct As MessagingTypes.BaseProductBuilderRiskType) As BaseImplementationTypes.BaseProductBuilderRiskType

        Dim impStruct As New BaseImplementationTypes.BaseProductBuilderRiskType

        If msgStruct IsNot Nothing AndAlso msgStruct.ProductBuilderData IsNot Nothing Then

            impStruct.ProductBuilderData = New BaseImplementationTypes.BaseProductBuilderRiskTypeProductBuilderData
            impStruct.ProductBuilderData.ItemName = msgStruct.ProductBuilderData.ItemName
            impStruct.ProductBuilderData.Value = msgStruct.ProductBuilderData.Value

        End If

        Return impStruct

    End Function

    Private Function ToBaseImpBaseTaxesAndFeesRiskType(ByVal msgStruct As MessagingTypes.BaseTaxesType) As BaseImplementationTypes.BaseTaxesType

        Dim impStruct As New BaseImplementationTypes.BaseTaxesType

        If msgStruct IsNot Nothing Then

            impStruct = New BaseImplementationTypes.BaseTaxesType
            impStruct.Amount = msgStruct.Amount
            impStruct.TaxRate = msgStruct.TaxRate
            impStruct.Description = msgStruct.Description
            impStruct.TaxBandCode = msgStruct.TaxBandCode

        End If

        Return impStruct

    End Function
End Class

