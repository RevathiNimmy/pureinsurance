Option Strict On

Imports SiriusFS.SAM.Structure
Imports SiriusFS.SAM.Structure.SFI.SAMForInsurance
Imports SiriusFS.SAM.Structure.STSErrorPublisher
Imports SiriusFS.SAM.CoreImplementation
Imports SiriusFS.SAM.CoreImplementation.InternalSAMConstants
Imports Sirius.Architecture.Configuration.Database


<WebService(Namespace:="http://www.siriusfs.com/SFI/SAM/SAMForInsurance/20060627")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Policy("SAMServerPolicy")> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class SAMForInsurance
    Inherits System.Web.Services.WebService

    Private Const ServiceNamespace As String = "http://www.siriusfs.com/SFI/SAM/SAMForInsurance/20060627"
    Private Const m_sDefaultNameSpace As String = "http://www.siriusfs.com/SFI/SAM/BaseTypes/20060627"

    ''' <summary>
    ''' This webmethod will get the RiskTypeCode and CurrentBranchCode to get the Clauses that are attahced with those two
    '''</summary>
    '''<param name="GetDefaultRiskClausesRequest"> It is an object of an class GetDefaultRiskClausesRequestType></param>    
    '''<remarks></remarks>
    <WebMethod()> _
   <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetDefaultRiskClauses(<XmlElement(elementName:="GetDefaultRiskClausesRequest", Namespace:=ServiceNamespace)> ByVal GetDefaultRiskClausesRequest As GetDefaultRiskClausesRequestType) As <XmlElement(elementName:="GetDefaultRiskClausesResponse", Namespace:=ServiceNamespace)> GetDefaultRiskClausesResponseType

        Try
            CheckAuthority("SAMARsk")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New GetDefaultRiskClausesResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetDefaultRiskClausesRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetDefaultRiskClausesResponseType = Nothing

            oImpRequest.BranchCode = GetDefaultRiskClausesRequest.BranchCode
            oImpRequest.CurrentBranchCode = GetDefaultRiskClausesRequest.CurrentBranchCode
            oImpRequest.RiskTypeCode = GetDefaultRiskClausesRequest.RiskTypeCode
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetDefaultRiskClauses(oImpRequest)

                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                'Retrieve the values from the implementation response structure
                If (oImpResponse IsNot Nothing AndAlso oImpResponse.Documents IsNot Nothing AndAlso oImpResponse.Documents.Row IsNot Nothing) Then
                    oResponse.Documents = New BaseGetDefaultRiskClausesResponseTypeDocuments
                    For iResponseCount As Integer = 0 To oImpResponse.Documents.Row.Length - 1
                        ReDim Preserve oResponse.Documents.Row(iResponseCount)
                        oResponse.Documents.Row(iResponseCount) = New BaseGetDefaultRiskClausesResponseTypeDocumentsRow
                        oResponse.Documents.Row(iResponseCount).Code = oImpResponse.Documents.Row(iResponseCount).Code
                        oResponse.Documents.Row(iResponseCount).Description = oImpResponse.Documents.Row(iResponseCount).Description
                    Next
                End If
            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try
    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function AddAddress(<XmlElement(elementName:="AddAddressRequest", Namespace:=ServiceNamespace)> ByVal AddAddressRequest As AddAddressRequestType) As <XmlElement(elementName:="AddAddressResponse", Namespace:=ServiceNamespace)> AddAddressResponseType

        Try
            CheckAuthority("SAMAAddr")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New AddAddressResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.AddAddressRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.AddAddressResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AddressLine1 = AddAddressRequest.AddressLine1
            oImpRequest.AddressLine2 = AddAddressRequest.AddressLine2
            oImpRequest.AddressLine3 = AddAddressRequest.AddressLine3
            oImpRequest.AddressLine4 = AddAddressRequest.AddressLine4
            oImpRequest.AddressTypeCode = CType(AddAddressRequest.AddressTypeCode, BaseImplementationTypes.AddressTypeType)
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.CountryCode = AddAddressRequest.CountryCode
            oImpRequest.PostCode = AddAddressRequest.PostCode
            oImpRequest.UserName = sUserName

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.AddAddress(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.AddressKey = oImpResponse.AddressKey

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function AddClaimRisk(<XmlElement(elementName:="AddClaimRiskRequest", Namespace:=ServiceNamespace)> ByVal AddClaimRiskRequest As AddClaimRiskRequestType) As <XmlElement(elementName:="AddClaimRiskResponse", Namespace:=ServiceNamespace)> AddClaimRiskResponseType

        Try

            CheckAuthority("SAMGClmRsk")

            Dim oResponse As New AddClaimRiskResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetClaimRiskRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetClaimRiskResponseType = Nothing

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

                Handler.BusinessLayerBoundary(ex, oResponse)

            End Try

            Return oResponse

        Catch ex As Exception

            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing

        End Try

    End Function

    <WebMethod()> _
   <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function AddParty(<XmlElement(elementName:="AddPartyRequest", Namespace:=ServiceNamespace)> ByVal AddPartyRequest As AddPartyRequestType) As <XmlElement(elementName:="AddPartyResponse", Namespace:=ServiceNamespace)> AddPartyResponseType

        Try
            CheckAuthority("SAMAPty")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim iLBnd As Integer
            Dim iUBnd As Integer

            Dim oResponse As New AddPartyResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.AddPartyRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.AddPartyResponseType = Nothing

            ' Convert the incoming interface structures into the implementation structures
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = AddPartyRequest.BranchCode
            oImpRequest.SubBranchCode = AddPartyRequest.SubBranchCode
            oImpRequest.UserName = sUserName

            ' Process the Party structure.  We 1st need to check the party type of the incoming message
            If AddPartyRequest.Item IsNot Nothing Then

                Dim impParty As New BaseImplementationTypes.BasePartyType
                Dim impAddress As New BaseImplementationTypes.BaseAddressWithContactsType
                Dim impContact As New BaseImplementationTypes.BaseContactType

                Dim msgAddress As BaseAddressType
                Dim msgContact As BaseContactType

                ' Check the type of the party object to see if it is Personal or Corporate
                If AddPartyRequest.Item.GetType Is GetType(BasePartyPCType) Then

                    ' Process personal client
                    Dim msgParty As BasePartyPCType = DirectCast(AddPartyRequest.Item, BasePartyPCType)
                    Dim impPartyPC As New BaseImplementationTypes.BasePartyPCType

                    impPartyPC.Forename = msgParty.Forename
                    impPartyPC.Surname = msgParty.Surname
                    impPartyPC.Initials = msgParty.Initials
                    impPartyPC.Title = msgParty.Title
                    impPartyPC.DateOfBirth = msgParty.DateOfBirth
                    impPartyPC.GenderCode = msgParty.GenderCode

                    impPartyPC.MaritalStatusCode = CType([Enum].ToObject(GetType(MaritalStatusCodeType), msgParty.MaritalStatusCode), BaseImplementationTypes.MaritalStatusCodeType)

                    'impPartyPC.MaritalStatusCode = msgParty.MaritalStatusCode
                    impPartyPC.OccupationCode = msgParty.OccupationCode
                    impPartyPC.EmploymentStatusCode = CType([Enum].ToObject(GetType(EmploymentStatusCodeType), msgParty.EmploymentStatusCode), BaseImplementationTypes.EmploymentStatusCodeType)
                    'impPartyPC.EmploymentStatusCode = msgParty.EmploymentStatusCode
                    impPartyPC.EmployersBusinessCode = msgParty.EmployersBusinessCode
                    impPartyPC.AlternativeId = msgParty.AlternativeId
                    impPartyPC.FileCode = msgParty.FileCode
                    impParty = impPartyPC

                ElseIf AddPartyRequest.Item.GetType Is GetType(BasePartyCCType) Then

                    ' Process corporate client
                    Dim msgParty As BasePartyCCType = DirectCast(AddPartyRequest.Item, BasePartyCCType)
                    Dim impPartyCC As New BaseImplementationTypes.BasePartyCCType

                    impPartyCC.CompanyName = msgParty.CompanyName
                    impPartyCC.BusinessCode = msgParty.BusinessCode
                    impPartyCC.MainContact = msgParty.MainContact
                    impPartyCC.NumberOfEmployees = msgParty.NumberOfEmployees.ToString
                    impPartyCC.NumberOfOffices = msgParty.NumberOfOffices
                    impPartyCC.FileCode = msgParty.FileCode
                    impParty = impPartyCC

                ElseIf AddPartyRequest.Item.GetType Is GetType(BasePartyOTHERType) Then

                    Dim objPartyOther As BasePartyOTHERType = DirectCast(AddPartyRequest.Item, BasePartyOTHERType)
                    Dim impPartyOther As New BaseImplementationTypes.BasePartyOTHERType
                    Dim impPartyConviction() As BaseImplementationTypes.BasePartyOTHERTypeConviction = Nothing

                    If objPartyOther.Conviction IsNot Nothing Then
                        For cntConv As Integer = objPartyOther.Conviction.GetLowerBound(0) To _
                                                        objPartyOther.Conviction.GetUpperBound(0)

                            ReDim Preserve impPartyConviction(cntConv)
                            impPartyConviction(cntConv) = New BaseImplementationTypes.BasePartyOTHERTypeConviction
                            impPartyConviction(cntConv).AlcoholLevel = objPartyOther.Conviction(cntConv).AlcoholLevel
                            impPartyConviction(cntConv).AlcoholMeasurementMethod = objPartyOther.Conviction(cntConv).AlcoholMeasurementMethod
                            impPartyConviction(cntConv).Date = objPartyOther.Conviction(cntConv).Date
                            impPartyConviction(cntConv).Description = objPartyOther.Conviction(cntConv).Description

                            impPartyConviction(cntConv).DrivingLicencePenaltyPoints = objPartyOther.Conviction(cntConv).DrivingLicencePenaltyPoints
                            impPartyConviction(cntConv).FineAmount = objPartyOther.Conviction(cntConv).FineAmount

                            impPartyConviction(cntConv).SentenceDescription = objPartyOther.Conviction(cntConv).SentenceDescription
                            impPartyConviction(cntConv).SentenceDuration = objPartyOther.Conviction(cntConv).SentenceDuration

                            impPartyConviction(cntConv).SentenceDurationQualifier = objPartyOther.Conviction(cntConv).SentenceDurationQualifier
                            impPartyConviction(cntConv).SentenceEffectiveDate = objPartyOther.Conviction(cntConv).SentenceEffectiveDate
                            impPartyConviction(cntConv).SentenceTypeCode = objPartyOther.Conviction(cntConv).SentenceTypeCode
                            impPartyConviction(cntConv).StatusCode = objPartyOther.Conviction(cntConv).StatusCode
                            impPartyConviction(cntConv).TypeCode = objPartyOther.Conviction(cntConv).TypeCode

                        Next
                        impPartyOther.Conviction = impPartyConviction
                    End If
                    Dim impPartyAccident() As BaseImplementationTypes.BasePartyOTHERTypeAccident = Nothing
                    If Not objPartyOther.Accident Is Nothing Then
                        For cntAcci As Integer = objPartyOther.Accident.GetLowerBound(0) To _
                                                    objPartyOther.Accident.GetUpperBound(0)

                            ReDim Preserve impPartyAccident(cntAcci)
                            impPartyAccident(cntAcci) = New BaseImplementationTypes.BasePartyOTHERTypeAccident
                            impPartyAccident(cntAcci).Date = objPartyOther.Accident(cntAcci).Date
                            impPartyAccident(cntAcci).Description = objPartyOther.Accident(cntAcci).Description
                            impPartyAccident(cntAcci).IsAtFault = objPartyOther.Accident(cntAcci).IsAtFault
                        Next
                        impPartyOther.Accident = impPartyAccident
                    End If
                    Dim impSuppBusiness() As BaseImplementationTypes.BasePartyOTHERTypeSupplierBusiness = Nothing
                    If Not objPartyOther.SupplierBusiness Is Nothing Then
                        For cntBusi As Integer = objPartyOther.SupplierBusiness.GetLowerBound(0) To _
                                                    objPartyOther.SupplierBusiness.GetUpperBound(0)

                            ReDim Preserve impSuppBusiness(cntBusi)
                            impSuppBusiness(cntBusi) = New BaseImplementationTypes.BasePartyOTHERTypeSupplierBusiness
                            impSuppBusiness(cntBusi).BusinessCode = objPartyOther.SupplierBusiness(cntBusi).BusinessCode
                            impSuppBusiness(cntBusi).SpecialityCode = objPartyOther.SupplierBusiness(cntBusi).SpecialityCode

                        Next
                        impPartyOther.SupplierBusiness = impSuppBusiness
                    End If
                    impPartyOther.ActiveIndicator = objPartyOther.ActiveIndicator
                    impPartyOther.AfterHoursIndicator = objPartyOther.AfterHoursIndicator
                    impPartyOther.Code = objPartyOther.Code

                    impPartyOther.DateOfBirth = objPartyOther.DateOfBirth
                    impPartyOther.DriverStatusCode = objPartyOther.DriverStatusCode
                    impPartyOther.Gender = objPartyOther.Gender

                    impPartyOther.LicenseNumber = objPartyOther.LicenseNumber
                    impPartyOther.LicenseTypeCode = objPartyOther.LicenseTypeCode
                    impPartyOther.Name = objPartyOther.Name

                    impPartyOther.PriorityIndicator = objPartyOther.PriorityIndicator
                    impPartyOther.RegNumber = objPartyOther.RegNumber
                    impPartyOther.TypeCode = objPartyOther.TypeCode
                    impPartyOther.FileCode = objPartyOther.FileCode

                    impParty = DirectCast(impPartyOther, BaseImplementationTypes.BasePartyType)

                    oImpRequest.Party = impParty

                End If

                ' Common party fields
                impParty.TPIntroducer = AddPartyRequest.Item.TPIntroducer
                impParty.TPUserCode = AddPartyRequest.Item.TPUserCode
                impParty.BranchCode = AddPartyRequest.Item.BranchCode
                impParty.XMLDataset = AddPartyRequest.Item.XMLDataset
                impParty.Currency = AddPartyRequest.Item.Currency

                ' Process the address structure
                If (AddPartyRequest.Item.Addresses) IsNot Nothing Then

                    iLBnd% = AddPartyRequest.Item.Addresses.GetLowerBound(0)
                    iUBnd% = AddPartyRequest.Item.Addresses.GetUpperBound(0)

                    ReDim impParty.Addresses(iUBnd%)

                    For iCnt As Integer = iLBnd% To iUBnd%
                        msgAddress = AddPartyRequest.Item.Addresses(iCnt%)
                        impAddress = New BaseImplementationTypes.BaseAddressWithContactsType

                        impAddress.AddressLine1 = msgAddress.AddressLine1
                        impAddress.AddressLine2 = msgAddress.AddressLine2
                        impAddress.AddressLine3 = msgAddress.AddressLine3
                        impAddress.AddressLine4 = msgAddress.AddressLine4
                        impAddress.AddressTypeCode = CType([Enum].ToObject(GetType(AddressTypeType), msgAddress.AddressTypeCode), BaseImplementationTypes.AddressTypeType)
                        'impAddress.AddressTypeCode = msgAddress.AddressTypeCode
                        impAddress.CountryCode = msgAddress.CountryCode
                        impAddress.PostCode = msgAddress.PostCode
                        If Not (AddPartyRequest.Item.Addresses(iCnt%).Contacts) Is Nothing Then
                            Dim lBoundContacts As Integer = AddPartyRequest.Item.Addresses(iCnt%).Contacts.GetLowerBound(0)
                            Dim uBoundContacts As Integer = AddPartyRequest.Item.Addresses(iCnt%).Contacts.GetUpperBound(0)
                            ReDim impAddress.Contacts(uBoundContacts)
                            Dim oContact As New BaseImplementationTypes.BaseContactType

                            For icntContacts As Integer = lBoundContacts To uBoundContacts
                                Dim AssignContact As BaseContactType = AddPartyRequest.Item.Addresses(iCnt%).Contacts(icntContacts)
                                oContact.AreaCode = Cast.ToString(AssignContact.AreaCode)

                                oContact.ContactDetail = New BaseImplementationTypes.BaseContactDetailType

                                oContact.ContactDetail.ItemElementName = CType([Enum].ToObject(GetType(ItemChoiceType), AssignContact.ContactDetail.ItemElementName), BaseImplementationTypes.ItemChoiceType)
                                'oContact.ContactDetail.ItemElementName = AssignContact.ContactDetail.ItemElementName
                                oContact.ContactDetail.Item = AssignContact.ContactDetail.Item
                                oContact.ContactTypeCode = CType([Enum].ToObject(GetType(ContactTypeType), AssignContact.ContactTypeCode), BaseImplementationTypes.ContactTypeType)
                                'oContact.ContactTypeCode = AssignContact.ContactTypeCode

                                impAddress.Contacts(icntContacts) = New BaseImplementationTypes.BaseContactType
                                impAddress.Contacts(icntContacts) = oContact
                            Next
                        End If
                        impParty.Addresses(iCnt%) = impAddress
                    Next iCnt%
                End If

                ' Process the Contact structure

                If IsArray(AddPartyRequest.Item.Contacts) Then

                    iLBnd% = AddPartyRequest.Item.Contacts.GetLowerBound(0)
                    iUBnd% = AddPartyRequest.Item.Contacts.GetUpperBound(0)

                    ReDim impParty.Contacts(iUBnd%)

                    For iCnt As Integer = iLBnd% To iUBnd%
                        msgContact = AddPartyRequest.Item.Contacts(iCnt%)
                        impContact = New BaseImplementationTypes.BaseContactType

                        impContact.AreaCode = msgContact.AreaCode
                        impContact.ContactDetail = New BaseImplementationTypes.BaseContactDetailType
                        impContact.ContactDetail.Item = msgContact.ContactDetail.Item

                        impContact.ContactTypeCode = CType([Enum].ToObject(GetType(ContactTypeType), msgContact.ContactTypeCode), BaseImplementationTypes.ContactTypeType)
                        'impContact.ContactTypeCode = msgContact.ContactTypeCode

                        impParty.Contacts(iCnt%) = impContact
                    Next iCnt%

                End If

                oImpRequest.Party = impParty

            End If

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.AddParty(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.PartyKey = oImpResponse.PartyKey
                oResponse.PartyTimestamp = oImpResponse.PartyTimestamp
                oResponse.ResolvedName = oImpResponse.ResolvedName
                oResponse.Shortname = oImpResponse.Shortname
                oResponse.XMLDataset = oImpResponse.XMLDataset

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function AddQuote(<XmlElement(elementName:="AddQuoteRequest", Namespace:=ServiceNamespace)> ByVal AddQuoteRequest As AddQuoteRequestType) As <XmlElement(elementName:="AddQuoteResponse", Namespace:=ServiceNamespace)> AddQuoteResponseType

        Try
            CheckAuthority("SAMAQuot")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New AddQuoteResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.AddQuoteRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.AddQuoteResponseType = Nothing

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
            oImpRequest.LapsedDate = AddQuoteRequest.LapsedDate
            oImpRequest.LapsedDateSpecified = AddQuoteRequest.LapsedDateSpecified
            oImpRequest.LapsedReasonCode = AddQuoteRequest.LapsedReasonCode
            oImpRequest.LapsedReasonDescription = AddQuoteRequest.LapsedReasonDescription
            oImpRequest.InceptionDateSpecified = True
            oImpRequest.InceptionDateTPISpecified = True
            oImpRequest.InceptionDate = AddQuoteRequest.CoverStartDate
            oImpRequest.InceptionDateTPI = AddQuoteRequest.CoverStartDate

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
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function AddRisk(<XmlElement(elementName:="AddRiskRequest", Namespace:=ServiceNamespace)> ByVal AddRiskRequest As AddRiskRequestType) As <XmlElement(elementName:="AddRiskResponse", Namespace:=ServiceNamespace)> AddRiskResponseType

        Try
            CheckAuthority("SAMARsk")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New AddRiskResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.AddRiskRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.AddRiskResponseType = Nothing

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
                oResponse.XMLDataSet = SAMFunc.TransformDatasetPBtoSAM(oImpResponse.XMLDataSet)
                oResponse.QuoteTimeStamp = oImpResponse.QuoteTimeStamp

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function BindQuote(<XmlElement(elementName:="BindQuoteRequest", Namespace:=ServiceNamespace)> ByVal BindQuoteRequest As BindQuoteRequestType) As <XmlElement(elementName:="BindQuoteResponse", Namespace:=ServiceNamespace)> BindQuoteResponseType

        Try
            CheckAuthority("SAMBQuote")

            Dim oResponse As New BindQuoteResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.BindQuoteRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.BindQuoteResponseType = Nothing

            Dim sUsername As String = String.Empty
            Dim iUserId As Int32 = 0
            Dim iAgentKey As Int32 = 0

            GetIdentity(sUsername, iAgentKey, iUserId)
            oImpRequest.AgentKey = iAgentKey

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = BindQuoteRequest.BranchCode
            oImpRequest.InsuranceFileKey = BindQuoteRequest.InsuranceFileKey
            oImpRequest.PaymentMethod = CType(BindQuoteRequest.PaymentMethod, BaseImplementationTypes.PaymentMethodType)
            oImpRequest.PaymentMethodSpecified = BindQuoteRequest.PaymentMethodSpecified
            oImpRequest.TransactionType = BindQuoteRequest.TransactionType

            oImpRequest.AcceptRenewal = False
            If BindQuoteRequest.AcceptRenewalSpecified Then
                oImpRequest.AcceptRenewal = BindQuoteRequest.AcceptRenewal
            End If
            oImpRequest.PayTrueMonthlyPolicyMTAPremiumOnRenewalSpecified = BindQuoteRequest.PayTrueMonthlyPolicyMTAPremiumOnRenewalSpecified
            If oImpRequest.PayTrueMonthlyPolicyMTAPremiumOnRenewalSpecified Then
                oImpRequest.PayTrueMonthlyPolicyMTAPremiumOnRenewal = BindQuoteRequest.PayTrueMonthlyPolicyMTAPremiumOnRenewal
            Else
                oImpRequest.PayTrueMonthlyPolicyMTAPremiumOnRenewal = False
            End If

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

            End If

            oImpRequest.SelectedInstalmentQuoteSpecified = False

            If BindQuoteRequest.SelectedInstalmentQuote IsNot Nothing Then
                oImpRequest.SelectedInstalmentQuoteSpecified = True
                oImpRequest.BankAccountName = BindQuoteRequest.SelectedInstalmentQuote.BankAccountName
                oImpRequest.BankAccountNo = BindQuoteRequest.SelectedInstalmentQuote.BankAccountNo
                oImpRequest.BankSortCode = BindQuoteRequest.SelectedInstalmentQuote.BankSortCode
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

                    ' if the payment method was credit card then also retrieve any credit card details 
                    ' passed in the request
                    If oImpRequest.PaymentMethodSpecified AndAlso _
                        oImpRequest.PaymentMethod = BaseImplementationTypes.PaymentMethodType.CreditCard AndAlso _
                                BindQuoteRequest.SelectedInstalmentQuote.CreditCard IsNot Nothing Then

                        oImpRequest.CreditCard = New BaseImplementationTypes.BaseCreditCardType

                        oImpRequest.CreditCard.ExpiryDate = BindQuoteRequest.SelectedInstalmentQuote.CreditCard.ExpiryDate
                        oImpRequest.CreditCard.Issue = BindQuoteRequest.SelectedInstalmentQuote.CreditCard.Issue
                        oImpRequest.CreditCard.NameOnCreditCard = BindQuoteRequest.SelectedInstalmentQuote.CreditCard.NameOnCreditCard
                        oImpRequest.CreditCard.Number = BindQuoteRequest.SelectedInstalmentQuote.CreditCard.Number
                        oImpRequest.CreditCard.Pin = BindQuoteRequest.SelectedInstalmentQuote.CreditCard.Pin
                        oImpRequest.CreditCard.StartDate = BindQuoteRequest.SelectedInstalmentQuote.CreditCard.StartDate
                        oImpRequest.CreditCard.TypeCode = BindQuoteRequest.SelectedInstalmentQuote.CreditCard.TypeCode

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

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.BindQuote(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Return details
                oResponse.Policy = New BaseTransactResponseTypePolicy
                oResponse.Policy.CommissionAmount = oImpResponse.Policy.CommissionAmount
                oResponse.Policy.PolicyRef = oImpResponse.Policy.PolicyRef
                oResponse.Policy.PremiumDueGross = oImpResponse.Policy.PremiumDueGross
                oResponse.Policy.PremiumDueNet = oImpResponse.Policy.PremiumDueNet
                oResponse.Policy.PremiumDueTax = oImpResponse.Policy.PremiumDueTax
                oResponse.Policy.TotalAnnualTax = oImpResponse.Policy.TotalAnnualTax
                oResponse.Policy.PolicyLevelTaxesAndFees = ToServiceTaxesAndFeesType(oImpResponse.Policy.PolicyLevelTaxesAndFees)

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function ChangePassword(<XmlElement(elementName:="ChangePasswordRequest", Namespace:=ServiceNamespace)> ByVal ChangePasswordRequest As ChangePasswordRequestType) As <XmlElement(elementName:="ChangePasswordResponse", Namespace:=ServiceNamespace)> ChangePasswordResponseType

        Try
            CheckAuthority("SAMChgPass")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New ChangePasswordResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.ChangePasswordRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.ChangePasswordResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = ChangePasswordRequest.BranchCode
            oImpRequest.NewPassword = ChangePasswordRequest.NewPassword
            oImpRequest.Password = ChangePasswordRequest.Password
            oImpRequest.UserName = sUserName

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.ChangePassword(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function CreateWmTask(<XmlElement(elementName:="CreateWmTaskRequest", Namespace:=ServiceNamespace)> ByVal CreateWmTaskRequest As CreateWmTaskRequestType) As <XmlElement(elementName:="CreateWmTaskResponse", Namespace:=ServiceNamespace)> CreateWmTaskResponseType

        Try
            CheckAuthority("SAMWmTask")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New CreateWmTaskResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.CreateWmTaskRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.CreateWmTaskResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AllocationUser = CreateWmTaskRequest.AllocationUser
            oImpRequest.AllocationUserGroup = CreateWmTaskRequest.AllocationUserGroup
            oImpRequest.BranchCode = CreateWmTaskRequest.BranchCode
            oImpRequest.Client = CreateWmTaskRequest.Client
            oImpRequest.Description = CreateWmTaskRequest.Description
            oImpRequest.DueDateTime = CreateWmTaskRequest.DueDateTime
            oImpRequest.IsComplete = CreateWmTaskRequest.IsComplete
            oImpRequest.IsUrgent = CreateWmTaskRequest.IsUrgent
            oImpRequest.Task = CreateWmTaskRequest.Task
            oImpRequest.TaskGroup = CreateWmTaskRequest.TaskGroup
            oImpRequest.UserId = iUserId

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.CreateWMTask(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function DeleteRisk(<XmlElement(elementName:="DeleteRiskRequest", Namespace:=ServiceNamespace)> ByVal DeleteRiskRequest As DeleteRiskRequestType) As <XmlElement(elementName:="DeleteRiskResponse", Namespace:=ServiceNamespace)> DeleteRiskResponseType

        Try
            CheckAuthority("SAMDRsk")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New DeleteRiskResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.DeleteRiskRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.DeleteRiskResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = DeleteRiskRequest.BranchCode
            oImpRequest.QuoteTimeStamp = DeleteRiskRequest.QuoteTimeStamp
            oImpRequest.InsuranceFolderKey = DeleteRiskRequest.InsuranceFolderKey
            oImpRequest.InsuranceFileKey = DeleteRiskRequest.InsuranceFileKey
            oImpRequest.RiskKey = DeleteRiskRequest.RiskKey
            oImpRequest.UserName = sUserName

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.DeleteRisk(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.QuoteTimeStamp = oImpResponse.QuoteTimeStamp

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function FindControlSearch(<XmlElement(elementName:="FindControlSearchRequest", Namespace:=ServiceNamespace)> ByVal FindControlSearchRequest As FindControlSearchRequestType) As <XmlElement(elementName:="FindControlSearchResponse", Namespace:=ServiceNamespace)> FindControlSearchResponseType

        Try
            CheckAuthority("SAMFCtlSrc")

            Dim oResponse As New FindControlSearchResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.FindControlSearchRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.FindControlSearchResponseType = Nothing

            ' Pass the values to the implementation request structure

            oImpRequest.BranchCode = FindControlSearchRequest.BranchCode
            oImpRequest.FindControlKey = FindControlSearchRequest.FindControlKey
            oImpRequest.BranchCode = FindControlSearchRequest.BranchCode

            Dim lCntSearch As Integer
            Dim lLBndSearch As Integer = FindControlSearchRequest.SearchCriteria.GetLowerBound(0)
            Dim lUBndSearch As Integer = FindControlSearchRequest.SearchCriteria.GetUpperBound(0)

            ReDim oImpRequest.SearchCriteria(lUBndSearch)

            For lCntSearch = lLBndSearch To lUBndSearch
                oImpRequest.SearchCriteria(lCntSearch) = New BaseImplementationTypes.BaseSearchCriteriaType
                oImpRequest.SearchCriteria(lCntSearch).ObjectName = FindControlSearchRequest.SearchCriteria(lCntSearch).ObjectName
                oImpRequest.SearchCriteria(lCntSearch).PropertyName = FindControlSearchRequest.SearchCriteria(lCntSearch).PropertyName
                oImpRequest.SearchCriteria(lCntSearch).Value = FindControlSearchRequest.SearchCriteria(lCntSearch).Value
            Next lCntSearch

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.FindControlSearch(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.Matches = oImpResponse.Matches

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function FindParty(<XmlElement(elementName:="FindPartyRequest", Namespace:=ServiceNamespace)> ByVal FindPartyRequest As FindPartyRequestType) As <XmlElement(elementName:="FindPartyResponse", Namespace:=ServiceNamespace)> FindPartyResponseType

        Try
            CheckAuthority("SAMFPty")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New FindPartyResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.FindPartyRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.FindPartyResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AddressLine1 = FindPartyRequest.AddressLine1
            oImpRequest.AddressLine2 = FindPartyRequest.AddressLine2
            oImpRequest.AddressLine3 = FindPartyRequest.AddressLine3
            oImpRequest.AddressLine4 = FindPartyRequest.AddressLine4
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.AlternativeId = FindPartyRequest.AlternativeId
            oImpRequest.AreaCode = FindPartyRequest.AreaCode
            oImpRequest.BranchCode = FindPartyRequest.BranchCode
            oImpRequest.DateOfBirth = FindPartyRequest.DateOfBirth
            oImpRequest.DateOfBirthSpecified = FindPartyRequest.DateOfBirthSpecified
            oImpRequest.Firstname = FindPartyRequest.Firstname
            oImpRequest.Name = FindPartyRequest.Name
            If FindPartyRequest.PartyTypeSpecified = True Then
                oImpRequest.PartyType = [Enum].GetName(GetType(PartyTypeType), FindPartyRequest.PartyType)
            Else
                oImpRequest.PartyType = String.Empty
            End If
            oImpRequest.PolicyRef = FindPartyRequest.PolicyRef
            oImpRequest.PostCode = FindPartyRequest.PostCode
            oImpRequest.RiskRequestdex = FindPartyRequest.RiskIndex
            oImpRequest.Shortname = FindPartyRequest.Shortname
            oImpRequest.TelephoneNumber = FindPartyRequest.TelephoneNumber
            oImpRequest.UserName = sUserName
            oImpRequest.FileCode = FindPartyRequest.FileCode

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.FindParty(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)


                ' Deserialize the XML from the implementation resultdataset into
                ' the correct messaging format
                Dim oResultDataSet As BaseFindPartyResponseTypeParties = Nothing
                Dim oResultDataSetObject As Object = Nothing
                Dim oXMLSerializer As New Serialization.XmlSerializer(GetType(BaseFindPartyResponseTypeParties), m_sDefaultNameSpace)
                If oImpResponse.ResultDataset IsNot Nothing Then
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=oImpResponse.ResultDataset.OuterXml, oXMLSerializer:=oXMLSerializer, r_oResultDataSet:=oResultDataSetObject, sDefaultNameSpace:=m_sDefaultNameSpace)
                    oResultDataSet = DirectCast(oResultDataSetObject, BaseFindPartyResponseTypeParties)
                End If
                ' Retrieve the values from the implementation response structure
                oResponse.Parties = oResultDataSet

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function ForgottenPassword(<XmlElement(elementName:="ForgottenPasswordRequest", Namespace:=ServiceNamespace)> ByVal ForgottenPasswordRequest As ForgottenPasswordRequestType) As <XmlElement(elementName:="ForgottenPasswordResponse", Namespace:=ServiceNamespace)> ForgottenPasswordResponseType

        Try
            CheckAuthority("SAMFrgtPwd")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New ForgottenPasswordResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.ForgottenPasswordRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.ForgottenPasswordResponseType = Nothing

            ' Pass the values to the implementation request structure

            oImpRequest.UserName = ForgottenPasswordRequest.Username
            oImpRequest.BranchCode = ForgottenPasswordRequest.BranchCode

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.ForgottenPassword(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GenerateDocument(<XmlElement(elementName:="GenerateDocumentRequest", Namespace:=ServiceNamespace)> ByVal GenerateDocumentRequest As GenerateDocumentRequestType) As <XmlElement(elementName:="GenerateDocumentResponse", Namespace:=ServiceNamespace)> GenerateDocumentResponseType

        Try
            CheckAuthority("SAMGenDoc")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New GenerateDocumentResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GenerateDocumentRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GenerateDocumentResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = GenerateDocumentRequest.BranchCode
            oImpRequest.DocumentTemplateCode = GenerateDocumentRequest.DocumentTemplateCode
            oImpRequest.InsuranceFileKey = GenerateDocumentRequest.InsuranceFileKey
            oImpRequest.InsuranceFolderKey = GenerateDocumentRequest.InsuranceFolderKey
            oImpRequest.Mode = 4
            oImpRequest.OutputAsHTML = GenerateDocumentRequest.OutputAsHTML
            oImpRequest.OutputAsPDF = GenerateDocumentRequest.OutputAsPDF
            oImpRequest.ParameterXML = GenerateDocumentRequest.ParameterXML
            oImpRequest.PartyKey = GenerateDocumentRequest.PartyKey
            oImpRequest.UserName = sUserName
            oImpRequest.ClaimKey = GenerateDocumentRequest.ClaimKey

            If GenerateDocumentRequest.SpoolDocumentOnlySpecified = True Then
                oImpRequest.SpoolDocumentOnly = GenerateDocumentRequest.SpoolDocumentOnly
            Else
                oImpRequest.SpoolDocumentOnly = False
            End If

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GenerateDocument(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                If oImpRequest.SpoolDocumentOnly = False Then
                    ' Retrieve the values from the implementation response structure
                    oResponse.MergedFilePath = oImpResponse.MergedFilePath
                    oResponse.SpooledZipFile = oImpResponse.SpooledZipFile
                End If

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GenerateClaimsDocuments(<XmlElement(elementName:="GenerateClaimsDocumentsRequest", Namespace:=ServiceNamespace)> ByVal GenerateClaimsDocumentsRequest As GenerateClaimsDocumentsRequestType) As <XmlElement(elementName:="GenerateClaimsDocumentsResponse", Namespace:=ServiceNamespace)> GenerateClaimsDocumentsResponseType

        Try
            CheckAuthority("SAMGenCDoc")

            Dim oResponse As New GenerateClaimsDocumentsResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GenerateClaimsDocumentsRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GenerateClaimsDocumentsResponseType = Nothing
            Dim iCount As Integer

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

                oResponse.Documents = New BaseGenerateClaimsDocumentsResponseTypeDocuments
                ReDim oResponse.Documents.Row(oImpResponse.Documents.Row.GetUpperBound(0))

                For iCount = oImpResponse.Documents.Row.GetLowerBound(0) To oImpResponse.Documents.Row.GetUpperBound(0)
                    oResponse.Documents.Row(iCount) = New BaseGenerateClaimsDocumentsResponseTypeDocumentsRow
                    oResponse.Documents.Row(iCount).DocumentDescription = oImpResponse.Documents.Row(iCount).DocumentDescription
                    oResponse.Documents.Row(iCount).DocumentName = oImpResponse.Documents.Row(iCount).DocumentName
                Next

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing

        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetClaimReceiptTaxes(<XmlElement(elementName:="GetClaimReceiptTaxesRequest", Namespace:=ServiceNamespace)> ByVal GetClaimReceiptTaxesRequest As GetClaimReceiptTaxesRequestType) As <XmlElement(elementName:="GetClaimReceiptTaxesResponse", Namespace:=ServiceNamespace)> GetClaimReceiptTaxesResponseType

        Try
            CheckAuthority("SAMGClmRT")

            Dim oResponse As New GetClaimReceiptTaxesResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetClaimReceiptTaxesRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetClaimReceiptTaxesResponseType = Nothing

            ClaimReceiptIn(oImpRequest, GetClaimReceiptTaxesRequest)

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetClaimReceiptTaxes(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.ReceiptToLossExchangeRate = oImpResponse.ReceiptToLossExchangeRate

                If oImpResponse.ReceiptItems IsNot Nothing Then
                    ' Retrieve the values from the implementation response structure
                    ReDim oResponse.ReceiptItems(oImpResponse.ReceiptItems.GetUpperBound(0))
                    For cntItems As Integer = oImpResponse.ReceiptItems.GetLowerBound(0) To oImpResponse.ReceiptItems.GetUpperBound(0)
                        oResponse.ReceiptItems(cntItems) = New BaseGetClaimReceiptTaxesResponseTypeReceiptItems

                        oResponse.ReceiptItems(cntItems).BaseRecoveryKey = oImpResponse.ReceiptItems(cntItems).BaseRecoveryKey
                        oResponse.ReceiptItems(cntItems).ReceiptAmount = Decimal.Round(oImpResponse.ReceiptItems(cntItems).ReceiptAmount * oImpResponse.ReceiptToLossExchangeRate, 2)
                        oResponse.ReceiptItems(cntItems).TaxGroupCode = oImpResponse.ReceiptItems(cntItems).TaxGroupCode
                        oResponse.ReceiptItems(cntItems).TaxAmount = Decimal.Round(oImpResponse.ReceiptItems(cntItems).TaxAmount * oImpResponse.ReceiptToLossExchangeRate, 2)

                    Next
                End If

                If oImpResponse.Recoveries IsNot Nothing Then
                    ReDim oResponse.Recoveries(oImpResponse.Recoveries.GetUpperBound(0))
                    For cntItems As Integer = oImpResponse.Recoveries.GetLowerBound(0) To oImpResponse.Recoveries.GetUpperBound(0)
                        oResponse.Recoveries(cntItems) = New BaseClaimPerilRecoveryReceiptType
                        oResponse.Recoveries(cntItems).BaseRecoveryKey = oImpResponse.Recoveries(cntItems).BaseRecoveryKey
                        oResponse.Recoveries(cntItems).TypeCode = oImpResponse.Recoveries(cntItems).TypeCode
                        oResponse.Recoveries(cntItems).TotalRecoveryAmount = Decimal.Round(oImpResponse.Recoveries(cntItems).TotalRecoveryAmount * oImpResponse.ReceiptToLossExchangeRate, 2)
                        oResponse.Recoveries(cntItems).TotalReceiptAmount = Decimal.Round(oImpResponse.Recoveries(cntItems).TotalReceiptAmount * oImpResponse.ReceiptToLossExchangeRate, 2)
                        oResponse.Recoveries(cntItems).ThisReceiptINCLTaxAmount = Decimal.Round(oImpResponse.Recoveries(cntItems).ThisReceiptINCLTaxAmount * oImpResponse.ReceiptToLossExchangeRate, 2)
                        oResponse.Recoveries(cntItems).ThisReceiptTaxAmount = Decimal.Round(oImpResponse.Recoveries(cntItems).ThisReceiptTaxAmount * oImpResponse.ReceiptToLossExchangeRate, 2)
                        oResponse.Recoveries(cntItems).ThisReceiptNetAmount = Decimal.Round(oImpResponse.Recoveries(cntItems).ThisReceiptNetAmount * oImpResponse.ReceiptToLossExchangeRate, 2)
                        oResponse.Recoveries(cntItems).BalanceAmount = Decimal.Round(oImpResponse.Recoveries(cntItems).BalanceAmount * oImpResponse.ReceiptToLossExchangeRate, 2)
                    Next
                End If

                If oImpResponse.TaxItems IsNot Nothing Then
                    ReDim oResponse.TaxItems(oImpResponse.TaxItems.GetUpperBound(0))
                    For cntItems As Integer = oImpResponse.TaxItems.GetLowerBound(0) To oImpResponse.TaxItems.GetUpperBound(0)
                        oResponse.TaxItems(cntItems) = New BaseClaimReceiptTaxItemType
                        oResponse.TaxItems(cntItems).Amount = Decimal.Round(oImpResponse.TaxItems(cntItems).Amount * oImpResponse.ReceiptToLossExchangeRate, 2)
                        oResponse.TaxItems(cntItems).Percentage = oImpResponse.TaxItems(cntItems).Percentage
                        oResponse.TaxItems(cntItems).RecoveryType = oImpResponse.TaxItems(cntItems).RecoveryType
                        oResponse.TaxItems(cntItems).TaxBandCode = oImpResponse.TaxItems(cntItems).TaxBandCode
                        oResponse.TaxItems(cntItems).TaxGroupCode = oImpResponse.TaxItems(cntItems).TaxGroupCode
                    Next
                End If

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function ClaimReceipt(<XmlElement(elementName:="ClaimReceiptRequest", Namespace:=ServiceNamespace)> ByVal ClaimReceiptRequest As ClaimReceiptRequestType) As <XmlElement(elementName:="ClaimReceiptResponse", Namespace:=ServiceNamespace)> ClaimReceiptResponseType

        Try
            CheckAuthority("SAMClmRec")

            Dim oResponse As New ClaimReceiptResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            'Dim bReceipt As Boolean
            Dim oTax As Int16 = 0

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.ClaimReceiptRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.ClaimReceiptResponseType = Nothing

            ClaimReceiptIn(oImpRequest, ClaimReceiptRequest)

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.ClaimReceipt(oImpRequest, oTax)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'Retrieve values
                oResponse.BaseClaimKey = oImpResponse.BaseClaimKey
                oResponse.ClaimKey = oImpResponse.ClaimKey
                oResponse.ClaimNumber = oImpResponse.ClaimNumber
                oResponse.Version = oImpResponse.Version

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    Private Sub ClaimReceiptIn( _
    ByVal oImpRequest As SAMForInsuranceImplementationTypes.ClaimReceiptRequestType, _
    ByVal oRequest As ClaimReceiptRequestType)

        ' claim receipt details

        oImpRequest.BranchCode = oRequest.BranchCode
        oImpRequest.ClaimReceipt = New BaseImplementationTypes.BaseClaimReceiptType
        oImpRequest.ClaimReceipt.BaseClaimKey = CInt(oRequest.ClaimReceipt.BaseClaimKey)
        oImpRequest.ClaimReceipt.BaseClaimPerilKey = CInt(Cast.ToString(oRequest.ClaimReceipt.BaseClaimPerilKey, "0"))
        oImpRequest.ClaimReceipt.ClaimVersionDescription = oRequest.ClaimReceipt.ClaimVersionDescription
        oImpRequest.ClaimReceipt.CurrencyCode = oRequest.ClaimReceipt.CurrencyCode
        oImpRequest.ClaimReceipt.PartyKey = oRequest.ClaimReceipt.PartyKey

        oImpRequest.ClaimReceipt.ReceiptPartyType = CType([Enum].ToObject(GetType(ClaimReceiptPartyTypeType), oRequest.ClaimReceipt.ReceiptPartyType), BaseImplementationTypes.ClaimReceiptPartyTypeType)
        'oImpRequest.ClaimReceipt.ReceiptPartyType = oRequest.ClaimReceipt.ReceiptPartyType

        ' for now we only support third party recoveries 
        ' in time this value should be set by the request
        oImpRequest.ClaimReceipt.IsSalvageRecovery = False

        ' for now we only support receipts processed today
        ' but for the data transfer this needs to process
        ' receipts made historicially
        oImpRequest.ClaimReceipt.TransactionDate = Date.Now()

        ' claim receipt items
        If oRequest.ClaimReceipt.ReceiptItem IsNot Nothing Then
            oImpRequest.ClaimReceipt.ReceiptItem = Array.ConvertAll(oRequest.ClaimReceipt.ReceiptItem, _
                                                        New Converter(Of BaseClaimReceiptItemType,  _
                                                        BaseImplementationTypes.BaseClaimReceiptItemType) _
                                                        (AddressOf ToBaseImpBaseClaimReceiptItemType))
        End If

        ' advanced tax options
        If oRequest.ClaimReceipt.AdvancedTaxDetails IsNot Nothing Then
            oImpRequest.ClaimReceipt.AdvancedTaxDetails = New BaseImplementationTypes.BaseClaimReceiptAdvancedTaxDetailsType
            oImpRequest.ClaimReceipt.AdvancedTaxDetails.InsuredDomiciled = oRequest.ClaimReceipt.AdvancedTaxDetails.InsuredDomiciled
            oImpRequest.ClaimReceipt.AdvancedTaxDetails.InsuredPercentage = oRequest.ClaimReceipt.AdvancedTaxDetails.InsuredPercentage
            oImpRequest.ClaimReceipt.AdvancedTaxDetails.IsSettlement = oRequest.ClaimReceipt.AdvancedTaxDetails.IsSettlement
            oImpRequest.ClaimReceipt.AdvancedTaxDetails.IsTaxExempt = oRequest.ClaimReceipt.AdvancedTaxDetails.IsTaxExempt
            oImpRequest.ClaimReceipt.AdvancedTaxDetails.ReceivableTaxPercentage = oRequest.ClaimReceipt.AdvancedTaxDetails.ReceivableTaxPercentage
            oImpRequest.ClaimReceipt.AdvancedTaxDetails.InsuredTaxNumber = oRequest.ClaimReceipt.AdvancedTaxDetails.InsuredTaxNumber
        End If

        ' payee
        oImpRequest.ClaimReceipt.Payee = New BaseImplementationTypes.BaseClaimPayeeType
        oImpRequest.ClaimReceipt.Payee.BankCode = oRequest.ClaimReceipt.Payee.BankCode
        oImpRequest.ClaimReceipt.Payee.BankName = oRequest.ClaimReceipt.Payee.BankName
        oImpRequest.ClaimReceipt.Payee.BankNumber = oRequest.ClaimReceipt.Payee.BankNumber
        oImpRequest.ClaimReceipt.Payee.MediaReference = oRequest.ClaimReceipt.Payee.MediaReference
        oImpRequest.ClaimReceipt.Payee.MediaTypeCode = oRequest.ClaimReceipt.Payee.MediaTypeCode
        oImpRequest.ClaimReceipt.Payee.Name = oRequest.ClaimReceipt.Payee.Name
        oImpRequest.ClaimReceipt.Payee.Comments = oRequest.ClaimReceipt.Payee.Comments
        oImpRequest.ClaimReceipt.Payee.TheirReference = oRequest.ClaimReceipt.Payee.TheirReference

        ' payee address
        If oRequest.ClaimReceipt.Payee.Address IsNot Nothing Then
            oImpRequest.ClaimReceipt.Payee.Address = New BaseImplementationTypes.BaseAddressType
            oImpRequest.ClaimReceipt.Payee.Address.AddressLine1 = oRequest.ClaimReceipt.Payee.Address.AddressLine1
            oImpRequest.ClaimReceipt.Payee.Address.AddressLine2 = oRequest.ClaimReceipt.Payee.Address.AddressLine2
            oImpRequest.ClaimReceipt.Payee.Address.AddressLine3 = oRequest.ClaimReceipt.Payee.Address.AddressLine3
            oImpRequest.ClaimReceipt.Payee.Address.AddressLine4 = oRequest.ClaimReceipt.Payee.Address.AddressLine4

            oImpRequest.ClaimReceipt.Payee.Address.AddressTypeCode = CType([Enum].ToObject(GetType(AddressTypeType), oRequest.ClaimReceipt.Payee.Address.AddressTypeCode), BaseImplementationTypes.AddressTypeType)
            'oImpRequest.ClaimReceipt.Payee.Address.AddressTypeCode = oRequest.ClaimReceipt.Payee.Address.AddressTypeCode
            oImpRequest.ClaimReceipt.Payee.Address.PostCode = oRequest.ClaimReceipt.Payee.Address.PostCode
            oImpRequest.ClaimReceipt.Payee.Address.CountryCode = oRequest.ClaimReceipt.Payee.Address.CountryCode
        End If

    End Sub

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function ClientDataImport(<XmlElement(elementName:="ClientDataImportRequest", Namespace:=ServiceNamespace)> ByVal ClientDataImportRequest As ClientDataImportRequestType) As <XmlElement(elementName:="ClientDataImportResponse", Namespace:=ServiceNamespace)> ClientDataImportResponseType

        Try
            CheckAuthority("SAMCliData")

            Dim msgResponse As New ClientDataImportResponseType

            ' Implementation structures
            Dim impRequest As New SAMForInsuranceImplementationTypes.ClientDataImportRequestType
            Dim impResponse As SAMForInsuranceImplementationTypes.ClientDataImportResponseType

            ' Convert the incoming interface structures into the implementation structures
            If ClientDataImportRequest.AgentKeySpecified Then
                impRequest.AgentKey = ClientDataImportRequest.AgentKey
            End If

            If ClientDataImportRequest.SiriusPartyKeySpecified Then
                impRequest.SiriusPartyKey = ClientDataImportRequest.SiriusPartyKey
                impRequest.SiriusPartyKeySpecified = True
            End If

            impRequest.BranchCode = ClientDataImportRequest.BranchCode

            ' Process the Party structure.  We 1st need to check the party type of the incoming message
            If (ClientDataImportRequest.Item Is Nothing) = False Then

                Dim impParty As New BaseImplementationTypes.BasePartyType
                Dim impAddress As New BaseImplementationTypes.BaseAddressWithContactsType
                Dim impContact As New BaseImplementationTypes.BaseContactType

                ' Check the type of the party object to see if it is Personal or Corporate
                If ClientDataImportRequest.Item.GetType Is GetType(BasePartyPCType) Then

                    ' Process personal client
                    Dim msgParty As BasePartyPCType = DirectCast(ClientDataImportRequest.Item, BasePartyPCType)
                    Dim impPartyPC As New BaseImplementationTypes.BasePartyPCType

                    impPartyPC.Forename = msgParty.Forename
                    impPartyPC.Surname = msgParty.Surname
                    impPartyPC.Initials = msgParty.Initials
                    impPartyPC.Title = msgParty.Title
                    impPartyPC.DateOfBirth = msgParty.DateOfBirth
                    impPartyPC.GenderCode = msgParty.GenderCode
                    impPartyPC.MaritalStatusCode = CType([Enum].ToObject(GetType(MaritalStatusCodeType), msgParty.MaritalStatusCode), BaseImplementationTypes.MaritalStatusCodeType)
                    'impPartyPC.MaritalStatusCode = msgParty.MaritalStatusCode
                    impPartyPC.OccupationCode = msgParty.OccupationCode
                    impPartyPC.EmploymentStatusCode = CType([Enum].ToObject(GetType(EmploymentStatusCodeType), msgParty.EmploymentStatusCode), BaseImplementationTypes.EmploymentStatusCodeType)
                    'impPartyPC.EmploymentStatusCode = msgParty.EmploymentStatusCode
                    impPartyPC.EmployersBusinessCode = msgParty.EmployersBusinessCode
                    impPartyPC.AlternativeId = msgParty.AlternativeId

                    impParty = impPartyPC

                Else

                    ' Process corporate client
                    Dim msgParty As BasePartyCCType = DirectCast(ClientDataImportRequest.Item, BasePartyCCType)
                    Dim impPartyCC As New BaseImplementationTypes.BasePartyCCType

                    impPartyCC.CompanyName = msgParty.CompanyName
                    impPartyCC.BusinessCode = msgParty.BusinessCode
                    impPartyCC.MainContact = msgParty.MainContact
                    impPartyCC.NumberOfEmployees = msgParty.NumberOfEmployees.ToString
                    impPartyCC.NumberOfOffices = msgParty.NumberOfOffices

                    impParty = impPartyCC

                End If

                ' Common to PC and CC
                impParty.BranchCode = ClientDataImportRequest.Item.BranchCode
                impParty.Currency = ClientDataImportRequest.Item.Currency
                impParty.TPUserCode = ClientDataImportRequest.Item.TPUserCode
                impParty.TPIntroducer = ClientDataImportRequest.Item.TPIntroducer
                impParty.AccountExecutive = ClientDataImportRequest.Item.AccountExecutive
                impParty.DomiciledForTax = ClientDataImportRequest.Item.DomiciledForTax
                impParty.TaxExempt = ClientDataImportRequest.Item.TaxExempt
                impParty.TaxNumber = ClientDataImportRequest.Item.TaxNumber
                impParty.TaxPercentage = ClientDataImportRequest.Item.TaxPercentage
                impParty.XMLDataset = ClientDataImportRequest.Item.XMLDataset
                impParty.FileCode = ClientDataImportRequest.Item.FileCode

                ' Process the address structure
                If IsArray(ClientDataImportRequest.Item.Addresses) = True Then

                    impParty.Addresses = Array.ConvertAll(ClientDataImportRequest.Item.Addresses, _
                                            New Converter(Of BaseAddressWithContactsType,  _
                                            BaseImplementationTypes.BaseAddressWithContactsType) _
                                            (AddressOf ToBaseImpBaseAddressType))

                End If

                ' Process the Contact structure
                If IsArray(ClientDataImportRequest.Item.Contacts) = True Then

                    impParty.Contacts = Array.ConvertAll(ClientDataImportRequest.Item.Contacts, _
                                        New Converter(Of BaseContactType,  _
                                        BaseImplementationTypes.BaseContactType) _
                                        (AddressOf ToBaseImpBaseContactType))

                End If

                impRequest.Party = impParty

            End If

            Dim oBusiness As New SAMForInsuranceBusiness

            Try

                'Call into the buisiness layer
                impResponse = oBusiness.ClientDataImport(impRequest)

                SAMErrorCollection.CheckForErrorsFromSTS(impResponse.STSError)

                msgResponse.PartyKey = impResponse.PartyKey

            Catch ex As Exception

                Handler.BusinessLayerBoundary(ex, msgResponse)
            End Try

            Return msgResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetAccountBalance(<XmlElement(elementName:="GetAccountBalanceRequest", Namespace:=ServiceNamespace)> ByVal GetAccountBalanceRequest As GetAccountBalanceRequestType) As <XmlElement(elementName:="GetAccountBalanceResponse", Namespace:=ServiceNamespace)> GetAccountBalanceResponseType

        Try
            CheckAuthority("SAMGClmLFR")

            Dim oResponse As New GetAccountBalanceResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetAccountBalanceRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetAccountBalanceResponseType = Nothing

            Dim sUsername As String = String.Empty
            Dim iAgentKey As Integer = 0
            Dim iUserId As Integer = 0

            GetIdentity(sUsername, iAgentKey, iUserId)

            oImpRequest.BranchCode = GetAccountBalanceRequest.BranchCode
            oImpRequest.PartyKey = iAgentKey

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetAccountBalance(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)



                ' Deserialize the XML from the implementation resultdataset into
                ' the correct messaging format
                If oImpResponse.AccountBalance IsNot Nothing Then
                    Dim oResultDataSet As BaseGetAccountBalanceResponseTypeAccountBalance = Nothing
                    Dim oResultDataSetObject As Object = Nothing
                    Dim oXMLSerializer As New Serialization.XmlSerializer(GetType(BaseGetAccountBalanceResponseTypeAccountBalance), m_sDefaultNameSpace)
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=oImpResponse.AccountBalance.OuterXml, oXMLSerializer:=oXMLSerializer, r_oResultDataSet:=oResultDataSetObject, sDefaultNameSpace:=m_sDefaultNameSpace)
                    oResultDataSet = DirectCast(oResultDataSetObject, BaseGetAccountBalanceResponseTypeAccountBalance)
                    ' Retrieve the values from the implementation response structure
                    oResponse.AccountBalance = oResultDataSet

                End If

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetAddress(<XmlElement(elementName:="GetAddressRequest", Namespace:=ServiceNamespace)> ByVal GetAddressRequest As GetAddressRequestType) As <XmlElement(elementName:="GetAddressResponse", Namespace:=ServiceNamespace)> GetAddressResponseType

        Try
            CheckAuthority("SAMGAddr")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New GetAddressResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetAddressRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetAddressResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AddressKey = GetAddressRequest.AddressKey
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = GetAddressRequest.BranchCode
            oImpRequest.UserName = sUserName

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetAddress(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                Dim oAddress As New SiriusFS.SAM.Structure.SFI.SAMForInsurance.BaseAddressType
                oAddress.AddressLine1 = oImpResponse.Address.AddressLine1
                oAddress.AddressLine2 = oImpResponse.Address.AddressLine2
                oAddress.AddressLine3 = oImpResponse.Address.AddressLine3
                oAddress.AddressLine4 = oImpResponse.Address.AddressLine4

                oAddress.AddressTypeCode = CType([Enum].ToObject(GetType(BaseImplementationTypes.AddressTypeType), oImpResponse.Address.AddressTypeCode), AddressTypeType)
                'oAddress.AddressTypeCode = oImpResponse.Address.AddressTypeCode
                oAddress.CountryCode = oImpResponse.Address.CountryCode
                oAddress.PostCode = oImpResponse.Address.PostCode
                oResponse.Address = oAddress

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetAllPolicyVersions(<XmlElement(elementName:="GetAllPolicyVersionsRequest", Namespace:=ServiceNamespace)> ByVal GetAllPolicyVersionsRequest As GetAllPolicyVersionsRequestType) As <XmlElement(elementName:="GetAllPolicyVersionsResponse", Namespace:=ServiceNamespace)> GetAllPolicyVersionsResponseType

        Try
            CheckAuthority("SAMGAPolVs")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New GetAllPolicyVersionsResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetAllPolicyVersionsRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetAllPolicyVersionsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = GetAllPolicyVersionsRequest.BranchCode
            oImpRequest.InsuranceFolderKey = GetAllPolicyVersionsRequest.InsuranceFolderKey
            oImpRequest.UserName = sUserName

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetAllPolicyVersions(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)


                ' Deserialize the XML from the implementation resultdataset into
                ' the correct messaging format
                Dim oResultDataSet As BaseGetAllPolicyVersionsResponseTypePolicies = Nothing
                Dim oResultDataSetObject As Object = Nothing
                Dim oXMLSerializer As New Serialization.XmlSerializer(GetType(BaseGetAllPolicyVersionsResponseTypePolicies), m_sDefaultNameSpace)
                If oImpResponse.ResultDataset IsNot Nothing Then
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=oImpResponse.ResultDataset.OuterXml, oXMLSerializer:=oXMLSerializer, r_oResultDataSet:=oResultDataSetObject, sDefaultNameSpace:=m_sDefaultNameSpace)
                    oResultDataSet = DirectCast(oResultDataSetObject, BaseGetAllPolicyVersionsResponseTypePolicies)
                End If

                ' Retrieve the values from the implementation response structure
                oResponse.Policies = oResultDataSet

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetCurrenciesByBranch(<XmlElement(elementName:="GetCurrenciesByBranchRequest", Namespace:=ServiceNamespace)> ByVal GetCurrenciesByBranchRequest As GetCurrenciesByBranchRequestType) As <XmlElement(elementName:="GetCurrenciesByBranchResponse", Namespace:=ServiceNamespace)> GetCurrenciesByBranchResponseType

        Try
            CheckAuthority("SAMGCurByB")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New GetCurrenciesByBranchResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetCurrenciesByBranchRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetCurrenciesByBranchResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = GetCurrenciesByBranchRequest.BranchCode
            oImpRequest.UserName = sUserName

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetCurrenciesByBranch(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)


                ' Deserialize the XML from the implementation resultdataset into
                ' the correct messaging format
                Dim oResultDataSet As BaseGetCurrenciesByBranchResponseTypeCurrencies = Nothing
                Dim oResultDataSetObject As Object = Nothing

                If oImpResponse.Currencies IsNot Nothing Then
                    Dim oXMLSerializer As New Serialization.XmlSerializer(GetType(BaseGetCurrenciesByBranchResponseTypeCurrencies), m_sDefaultNameSpace)
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=oImpResponse.Currencies.OuterXml, oXMLSerializer:=oXMLSerializer, r_oResultDataSet:=oResultDataSetObject, sDefaultNameSpace:=m_sDefaultNameSpace)
                    oResultDataSet = DirectCast(oResultDataSetObject, BaseGetCurrenciesByBranchResponseTypeCurrencies)
                End If

                ' Retrieve the values from the implementation response structure
                oResponse.Currencies = oResultDataSet

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetDatasetDefinition(<XmlElement(elementName:="GetDatasetDefinitionRequest", Namespace:=ServiceNamespace)> ByVal GetDatasetDefinitionRequest As GetDatasetDefinitionRequestType) As <XmlElement(elementName:="GetDatasetDefinitionResponse", Namespace:=ServiceNamespace)> GetDatasetDefinitionResponseType

        Try
            CheckAuthority("SAMGDSDef")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New GetDatasetDefinitionResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetDatasetDefinitionRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetDatasetDefinitionResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = GetDatasetDefinitionRequest.BranchCode
            oImpRequest.UserName = sUserName
            oImpRequest.DataModelCode = GetDatasetDefinitionRequest.DataModelCode

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetDatasetDefinition(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.XMLDatasetDefinition = oImpResponse.XMLDatasetDefinition

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetDatasetSchema(<XmlElement(elementName:="GetDatasetSchemaRequest", Namespace:=ServiceNamespace)> ByVal GetDatasetSchemaRequest As GetDatasetSchemaRequestType) As <XmlElement(elementName:="GetDatasetSchemaResponse", Namespace:=ServiceNamespace)> GetDatasetSchemaResponseType

        Try
            CheckAuthority("SAMGDSSch")

            Dim oResponse As New GetDatasetSchemaResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            Dim oImpRequest As New BaseImplementationTypes.BaseGetDatasetSchemaRequestType
            Dim oImpResponse As BaseImplementationTypes.BaseGetDatasetSchemaResponseType = Nothing

            oImpRequest.BranchCode = GetDatasetSchemaRequest.BranchCode
            oImpRequest.DataModelCode = GetDatasetSchemaRequest.DataModelCode

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetDatasetSchema(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Return details
                oResponse.DatasetSchema = oImpResponse.DatasetSchema

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetHeaderAndSummariesByKey(<XmlElement(elementName:="GetHeaderAndSummariesByKeyRequest", Namespace:=ServiceNamespace)> ByVal GetHeaderAndSummariesByKeyRequest As GetHeaderAndSummariesByKeyRequestType) As <XmlElement(elementName:="GetHeaderAndSummariesByKeyResponse", Namespace:=ServiceNamespace)> GetHeaderAndSummariesByKeyResponseType

        Try
            CheckAuthority("SAMGHSKey")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New GetHeaderAndSummariesByKeyResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetHeaderAndSummariesByKeyRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetHeaderAndSummariesByKeyResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = GetHeaderAndSummariesByKeyRequest.BranchCode
            oImpRequest.InsuranceFileKey = GetHeaderAndSummariesByKeyRequest.InsuranceFileKey
            oImpRequest.UserName = sUserName

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetHeaderAndSummariesByKey(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Deserialize the Risks XML from the implementation resultdataset into
                ' the correct messaging format
                Dim oRisksDataSet As BaseGetHeaderAndSummariesResponseTypeRisks = Nothing
                Dim oRisksDataSetObject As Object = Nothing

                If oImpResponse.ResultDataset IsNot Nothing Then
                    Dim oXMLSerializerRisks As New Serialization.XmlSerializer(GetType(BaseGetHeaderAndSummariesResponseTypeRisks), m_sDefaultNameSpace)
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=oImpResponse.ResultDataset.OuterXml, oXMLSerializer:=oXMLSerializerRisks, r_oResultDataSet:=oRisksDataSetObject, sDefaultNameSpace:=m_sDefaultNameSpace)
                    oRisksDataSet = DirectCast(oRisksDataSetObject, BaseGetHeaderAndSummariesResponseTypeRisks)
                End If


                ' Deserialize the Insured Parties XML from the implementation resultdataset into
                ' the correct messaging format
                Dim oInsuredPartiesDataSet As GetHeaderAndSummariesByKeyResponseTypeInsuredParties = Nothing
                Dim oInsuredPartiesDataSetObject As Object = Nothing

                If oImpResponse.InsuredParties IsNot Nothing Then
                    Dim oXMLSerializerIPs As New Serialization.XmlSerializer(GetType(GetHeaderAndSummariesByKeyResponseTypeInsuredParties), m_sDefaultNameSpace)
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=oImpResponse.InsuredParties.OuterXml, oXMLSerializer:=oXMLSerializerIPs, r_oResultDataSet:=oInsuredPartiesDataSetObject, sDefaultNameSpace:=m_sDefaultNameSpace)
                    oInsuredPartiesDataSet = DirectCast(oInsuredPartiesDataSetObject, GetHeaderAndSummariesByKeyResponseTypeInsuredParties)
                End If

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
                oResponse.InsuredParties = oInsuredPartiesDataSet
                oResponse.PartyKey = oImpResponse.PartyKey
                oResponse.PaymentMethodCode = oImpResponse.PaymentMethodCode
                oResponse.ProductCode = oImpResponse.ProductCode
                oResponse.QuoteExpiryDate = oImpResponse.QuoteExpiryDate
                oResponse.QuoteIsLocked = oImpResponse.QuoteIsLocked
                oResponse.QuoteTimeStamp = oImpResponse.QuoteTimeStamp
                oResponse.Risks = oRisksDataSet
                oResponse.SubBranchCode = oImpResponse.SubBranchCode
                oResponse.ConsolidatedLeadAgentCommission = oImpResponse.ConsolidatedLeadAgentCommission
                oResponse.ConsolidatedSubAgentCommission = oImpResponse.ConsolidatedSubAgentCommission
                oResponse.PolicyLevelTaxesAndFees = ToServiceTaxesAndFeesType(oImpResponse.PolicyLevelTaxesAndFees)
                oResponse.AlternativeRef = oImpResponse.AlternativeRef
                oResponse.LapsedReasonCode = oImpResponse.LapsedReasonCode
                oResponse.LapsedReasonDescription = oImpResponse.LapsedReasonDescription
                oResponse.RenewalStatusTypeCode = oImpResponse.RenewalStatusTypeCode
                oResponse.RenewalStatusTypeDescription = oImpResponse.RenewalStatusTypeDescription
                oResponse.CurrencyCode = oImpResponse.CurrencyCode
                oResponse.BranchCode = oImpResponse.BranchCode

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetHeaderAndSummariesByRef(<XmlElement(elementName:="GetHeaderAndSummariesByRefRequest", Namespace:=ServiceNamespace)> ByVal GetHeaderAndSummariesByRefRequest As GetHeaderAndSummariesByRefRequestType) As <XmlElement(elementName:="GetHeaderAndSummariesByRefResponse", Namespace:=ServiceNamespace)> GetHeaderAndSummariesByRefResponseType

        Try
            CheckAuthority("SAMGHSRef")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New GetHeaderAndSummariesByRefResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetHeaderAndSummariesByRefRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetHeaderAndSummariesByRefResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = GetHeaderAndSummariesByRefRequest.BranchCode
            oImpRequest.InsuranceRef = GetHeaderAndSummariesByRefRequest.InsuranceRef
            oImpRequest.UserName = sUserName

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetHeaderAndSummariesByRef(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Deserialize the Risks XML from the implementation resultdataset into
                ' the correct messaging format
                Dim oRisksDataSet As BaseGetHeaderAndSummariesResponseTypeRisks = Nothing
                Dim oRisksDataSetObject As Object = Nothing
                Dim oXMLSerializerRisks As New Serialization.XmlSerializer(GetType(BaseGetHeaderAndSummariesResponseTypeRisks), m_sDefaultNameSpace)
                If oImpResponse.ResultDataset IsNot Nothing Then
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=oImpResponse.ResultDataset.OuterXml, oXMLSerializer:=oXMLSerializerRisks, r_oResultDataSet:=oRisksDataSetObject, sDefaultNameSpace:=m_sDefaultNameSpace)
                    oRisksDataSet = DirectCast(oRisksDataSetObject, BaseGetHeaderAndSummariesResponseTypeRisks)
                End If


                ' Deserialize the Insured Parties XML from the implementation resultdataset into
                ' the correct messaging format
                Dim oInsuredPartiesDataSet As GetHeaderAndSummariesByRefResponseTypeInsuredParties = Nothing
                Dim oInsuredPartiesDataSetObject As Object = Nothing

                If oImpResponse.InsuredParties IsNot Nothing Then
                    Dim oXMLSerializerIPs As New Serialization.XmlSerializer(GetType(GetHeaderAndSummariesByRefResponseTypeInsuredParties), m_sDefaultNameSpace)
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=oImpResponse.InsuredParties.OuterXml, oXMLSerializer:=oXMLSerializerIPs, r_oResultDataSet:=oInsuredPartiesDataSetObject, sDefaultNameSpace:=m_sDefaultNameSpace)
                    oInsuredPartiesDataSet = DirectCast(oInsuredPartiesDataSetObject, GetHeaderAndSummariesByRefResponseTypeInsuredParties)
                End If

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
                oResponse.InsuredParties = oInsuredPartiesDataSet
                oResponse.PartyKey = oImpResponse.PartyKey
                oResponse.PaymentMethodCode = oImpResponse.PaymentMethodCode
                oResponse.ProductCode = oImpResponse.ProductCode
                oResponse.QuoteExpiryDate = oImpResponse.QuoteExpiryDate
                oResponse.QuoteIsLocked = oImpResponse.QuoteIsLocked
                oResponse.QuoteTimeStamp = oImpResponse.QuoteTimeStamp
                oResponse.Risks = oRisksDataSet
                oResponse.SubBranchCode = oImpResponse.SubBranchCode
                oResponse.ConsolidatedLeadAgentCommission = oImpResponse.ConsolidatedLeadAgentCommission
                oResponse.ConsolidatedSubAgentCommission = oImpResponse.ConsolidatedSubAgentCommission
                oResponse.PolicyLevelTaxesAndFees = ToServiceTaxesAndFeesType(oImpResponse.PolicyLevelTaxesAndFees)
                oResponse.AlternativeRef = oImpResponse.AlternativeRef

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetInstalmentQuotes(<XmlElement(elementName:="GetInstalmentQuotesRequest", Namespace:=ServiceNamespace)> ByVal GetInstalmentQuotesRequest As GetInstalmentQuotesRequestType) As <XmlElement(elementName:="GetInstalmentQuotesResponse", Namespace:=ServiceNamespace)> GetInstalmentQuotesResponseType

        Try
            CheckAuthority("SAMGInQuot")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New GetInstalmentQuotesResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetInstalmentQuotesRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetInstalmentQuotesResponseType = Nothing

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

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetInstalmentQuotes(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Deserialize the XML from the implementation resultdataset into
                ' the correct messaging format
                Dim oResultDataSet As BaseGetInstalmentQuotesResponseTypeQuotes = Nothing
                Dim oResultDataSetObject As Object = Nothing
                'Dim o As BaseGetInstalmentQuotesResponseType
                'o.ResultDataSet.Row(0).Code
                Dim oXMLSerializer As New Serialization.XmlSerializer(GetType(BaseGetInstalmentQuotesResponseTypeQuotes), m_sDefaultNameSpace)
                If oImpResponse.Quotes IsNot Nothing Then
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=oImpResponse.Quotes.OuterXml, oXMLSerializer:=oXMLSerializer, r_oResultDataSet:=oResultDataSetObject, sDefaultNameSpace:=m_sDefaultNameSpace)
                    oResultDataSet = DirectCast(oResultDataSetObject, BaseGetInstalmentQuotesResponseTypeQuotes)
                End If

                ' Retrieve the values from the implementation response structure
                oResponse.Quotes = oResultDataSet

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetList(<XmlElement(elementName:="GetListRequest", Namespace:=ServiceNamespace)> ByVal GetListRequest As GetListRequestType) As <XmlElement(elementName:="GetListResponse", Namespace:=ServiceNamespace)> GetListResponseType

        Try
            CheckAuthority("SAMGLst")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New GetListResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetListRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetListResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = GetListRequest.BranchCode
            oImpRequest.ListCode = GetListRequest.ListCode
            oImpRequest.ListType = CType([Enum].ToObject(GetType(STSListType), GetListRequest.ListType), BaseImplementationTypes.STSListType)
            oImpRequest.UserName = sUserName

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetList(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Deserialize the XML from the implementation resultdataset into
                ' the correct messaging format
                Dim oResultDataSet As BaseGetListResponseTypeList = Nothing
                Dim oResultDataSetObject As Object = Nothing
                'Dim o As BaseGetListResponseType
                'o.ResultDataSet.Row(0).Code

                If oImpResponse.ListItems IsNot Nothing Then
                    Dim oXMLSerializer As New Serialization.XmlSerializer(GetType(BaseGetListResponseTypeList), m_sDefaultNameSpace)
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=oImpResponse.ListItems.OuterXml, oXMLSerializer:=oXMLSerializer, r_oResultDataSet:=oResultDataSetObject, sDefaultNameSpace:=m_sDefaultNameSpace)
                    oResultDataSet = DirectCast(oResultDataSetObject, BaseGetListResponseTypeList)
                End If

                ' Retrieve the values from the implementation response structure
                oResponse.List = oResultDataSet

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetPoliciesInRenewal(<XmlElement(elementName:="GetPoliciesInRenewalRequest", Namespace:=ServiceNamespace)> ByVal GetPoliciesInRenewalRequest As GetPoliciesInRenewalRequestType) As <XmlElement(elementName:="GetPoliciesInRenewalResponse", Namespace:=ServiceNamespace)> GetPoliciesInRenewalResponseType
        Try
            'TODO - Check authority
            'CheckAuthority("SAMGPIR")
            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)
            Dim oResponse As New GetPoliciesInRenewalResponseType
            Dim oBusiness As New SAMForInsuranceBusiness
            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetPoliciesInRenewalRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetPoliciesInRenewalResponseType = Nothing
            ' Pass the values to the implementation request structure
            If GetPoliciesInRenewalRequest.AgentKeySpecified = True Then
                oImpRequest.AgentKey = GetPoliciesInRenewalRequest.AgentKey
            Else
                oImpRequest.AgentKey = iAgentKey
            End If
            oImpRequest.BranchCode = GetPoliciesInRenewalRequest.BranchCode
            oImpRequest.PartyKey = GetPoliciesInRenewalRequest.PartyKey
            oImpRequest.UserName = sUserName
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetPoliciesInRenewal(oImpRequest)

                ' Deserialize the Risks XML from the implementation resultdataset into
                ' the correct messaging format
                Dim oPoliciesDataSet As BaseGetPoliciesInRenewalResponseTypePolicies = Nothing
                Dim oPoliciesDataSetObject As Object = Nothing
                Dim oXMLSerializerPolicies As New Serialization.XmlSerializer(GetType(BaseGetPoliciesInRenewalResponseTypePolicies), m_sDefaultNameSpace)
                If oImpResponse.Policies IsNot Nothing Then
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=oImpResponse.Policies.OuterXml, oXMLSerializer:=oXMLSerializerPolicies, r_oResultDataSet:=oPoliciesDataSetObject, sDefaultNameSpace:=m_sDefaultNameSpace)
                    oPoliciesDataSet = DirectCast(oPoliciesDataSetObject, BaseGetPoliciesInRenewalResponseTypePolicies)
                End If
                ' Retrieve the values from the implementation response structure
                oResponse.Policies = oPoliciesDataSet
            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try
            Return oResponse
        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try
    End Function
    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetParty(<XmlElement(elementName:="GetPartyRequest", Namespace:=ServiceNamespace)> ByVal GetPartyRequest As GetPartyRequestType) As <XmlElement(elementName:="GetPartyResponse", Namespace:=ServiceNamespace)> GetPartyResponseType

        Try
            CheckAuthority("SAMGPty")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New GetPartyResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetPartyRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetPartyResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = GetPartyRequest.BranchCode
            oImpRequest.PartyKey = GetPartyRequest.PartyKey
            oImpRequest.UserName = sUserName

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetParty(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                Dim oParty As New SiriusFS.SAM.Structure.SFI.SAMForInsurance.BasePartyType

                If oImpResponse.Item.GetType Is GetType(SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyPCType) Then
                    oParty = New SiriusFS.SAM.Structure.SFI.SAMForInsurance.BasePartyPCType
                ElseIf oImpResponse.Item.GetType Is GetType(SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyCCType) Then
                    oParty = New SiriusFS.SAM.Structure.SFI.SAMForInsurance.BasePartyCCType
                ElseIf oImpResponse.Item.GetType Is GetType(SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyOTHERType) Then
                    oParty = New SiriusFS.SAM.Structure.SFI.SAMForInsurance.BasePartyOTHERType
                End If

                ' get the addresses
                If oImpResponse.Item.Addresses IsNot Nothing Then

                    oParty.Addresses = Array.ConvertAll(oImpResponse.Item.Addresses, _
                            New Converter(Of BaseImplementationTypes.BaseAddressType,  _
                            BaseAddressWithContactsType) _
                            (AddressOf ToServiceBaseAddressWithContactsType))

                End If

                ' get the contacts
                If oImpResponse.Item.Contacts IsNot Nothing Then

                    oParty.Contacts = Array.ConvertAll(oImpResponse.Item.Contacts, _
                            New Converter(Of BaseImplementationTypes.BaseContactType,  _
                            BaseContactType) _
                            (AddressOf ToServiceContactType))

                End If

                oParty.BranchCode = oImpResponse.Item.BranchCode
                oParty.TPIntroducer = oImpResponse.Item.TPIntroducer
                oParty.TPUserCode = oImpResponse.Item.TPUserCode
                oParty.XMLDataset = oImpResponse.Item.XMLDataset
                oParty.FileCode = oImpResponse.Item.FileCode
                oResponse.PartyTimestamp = oImpResponse.PartyTimestamp
                oResponse.Item = oParty

                If oImpResponse.Item.GetType Is GetType(SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyPCType) Then
                    Dim oResponseParty As New SiriusFS.SAM.Structure.SFI.SAMForInsurance.BasePartyPCType
                    Dim oPartyPC As New SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyPCType
                    oPartyPC = DirectCast(oImpResponse.Item, SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyPCType)
                    oResponseParty = DirectCast(oResponse.Item, SiriusFS.SAM.Structure.SFI.SAMForInsurance.BasePartyPCType)
                    oResponseParty.AlternativeId = oPartyPC.AlternativeId
                    oResponseParty.DateOfBirth = oPartyPC.DateOfBirth
                    oResponseParty.DateOfBirthSpecified = oPartyPC.DateOfBirthSpecified
                    oResponseParty.EmployersBusinessCode = oPartyPC.EmployersBusinessCode
                    oResponseParty.EmploymentStatusCode = CType([Enum].ToObject(GetType(BaseImplementationTypes.EmploymentStatusCodeType), oPartyPC.EmploymentStatusCode), EmploymentStatusCodeType)
                    'oResponseParty.EmploymentStatusCode = oPartyPC.EmploymentStatusCode
                    oResponseParty.EmploymentStatusCodeSpecified = oPartyPC.EmploymentStatusCodeSpecified
                    oResponseParty.Forename = oPartyPC.Forename
                    oResponseParty.GenderCode = oPartyPC.GenderCode
                    oResponseParty.Initials = oPartyPC.Initials
                    oResponseParty.MaritalStatusCode = CType([Enum].ToObject(GetType(BaseImplementationTypes.MaritalStatusCodeType), oPartyPC.MaritalStatusCode), MaritalStatusCodeType)
                    'oResponseParty.MaritalStatusCode = oPartyPC.MaritalStatusCode
                    oResponseParty.MaritalStatusCodeSpecified = oPartyPC.MaritalStatusCodeSpecified
                    oResponseParty.OccupationCode = oPartyPC.OccupationCode
                    oResponseParty.Surname = oPartyPC.Surname
                    oResponseParty.Title = oPartyPC.Title
                    oResponse.Item = oResponseParty
                ElseIf oImpResponse.Item.GetType Is GetType(SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyCCType) Then
                    Dim oResponseParty As New SiriusFS.SAM.Structure.SFI.SAMForInsurance.BasePartyCCType
                    Dim oPartyCC As New SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyCCType
                    oPartyCC = DirectCast(oImpResponse.Item, SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyCCType)
                    oResponseParty = DirectCast(oResponse.Item, SiriusFS.SAM.Structure.SFI.SAMForInsurance.BasePartyCCType)
                    oResponseParty.BusinessCode = oPartyCC.BusinessCode
                    oResponseParty.CompanyName = oPartyCC.CompanyName
                    oResponseParty.MainContact = oPartyCC.MainContact
                    oResponse.Item = oResponseParty
                ElseIf oImpResponse.Item.GetType Is GetType(SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyOTHERType) Then
                    PopulateOtherPartyReponse(oImpResponse, oResponse)
                End If

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetPartySummary(<XmlElement(elementName:="GetPartySummaryRequest", Namespace:=ServiceNamespace)> ByVal GetPartySummaryRequest As GetPartySummaryRequestType) As <XmlElement(elementName:="GetPartySummaryResponse", Namespace:=ServiceNamespace)> GetPartySummaryResponseType

        Try
            CheckAuthority("SAMGPtySum")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New GetPartySummaryResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetPartySummaryRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetPartySummaryResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = GetPartySummaryRequest.BranchCode
            oImpRequest.PartyKey = GetPartySummaryRequest.PartyKey
            oImpRequest.UserName = sUserName

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetPartySummary(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)


                ' Deserialize the XML from the implementation resultdataset into
                ' the correct messaging format
                Dim oResultDataSet As BaseGetPartySummaryResponseTypePolicies = Nothing
                Dim oResultDataSetObject As Object = Nothing

                If oImpResponse.InsuranceFileDataset IsNot Nothing Then
                    Dim oXMLSerializer As New Serialization.XmlSerializer(GetType(BaseGetPartySummaryResponseTypePolicies), m_sDefaultNameSpace)
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=oImpResponse.InsuranceFileDataset.OuterXml, oXMLSerializer:=oXMLSerializer, r_oResultDataSet:=oResultDataSetObject, sDefaultNameSpace:=m_sDefaultNameSpace)
                    oResultDataSet = DirectCast(oResultDataSetObject, BaseGetPartySummaryResponseTypePolicies)
                End If

                ' Retrieve the values from the implementation response structure
                oResponse.Policies = oResultDataSet
                oResponse.PartyTimestamp = oImpResponse.PartyTimestamp
                Dim oParty As New SiriusFS.SAM.Structure.SFI.SAMForInsurance.BasePartyType

                If oImpResponse.Item.GetType Is GetType(SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyPCType) Then
                    oParty = New SiriusFS.SAM.Structure.SFI.SAMForInsurance.BasePartyPCType
                ElseIf oImpResponse.Item.GetType Is GetType(SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyCCType) Then
                    oParty = New SiriusFS.SAM.Structure.SFI.SAMForInsurance.BasePartyCCType
                End If

                Dim ilBnd As Integer
                Dim iuBnd As Integer
                Dim impAddress As New SiriusFS.SAM.Structure.SFI.SAMForInsurance.BaseAddressType
                Dim msgAddress As SiriusFS.SAM.Structure.BaseImplementationTypes.BaseAddressType
                If IsArray(oImpResponse.Item.Addresses) Then
                    ilBnd% = oImpResponse.Item.Addresses.GetLowerBound(0)
                    iuBnd% = oImpResponse.Item.Addresses.GetUpperBound(0)
                    ReDim oParty.Addresses(iuBnd%)
                    For iCnt As Integer = ilBnd% To iuBnd%
                        msgAddress = oImpResponse.Item.Addresses(iCnt%)
                        impAddress = New SiriusFS.SAM.Structure.SFI.SAMForInsurance.BaseAddressWithContactsType
                        impAddress.AddressLine1 = msgAddress.AddressLine1
                        impAddress.AddressLine2 = msgAddress.AddressLine2
                        impAddress.AddressLine3 = msgAddress.AddressLine3
                        impAddress.AddressLine4 = msgAddress.AddressLine4
                        impAddress.AddressTypeCode = CType([Enum].ToObject(GetType(BaseImplementationTypes.AddressTypeType), msgAddress.AddressTypeCode), AddressTypeType)
                        'impAddress.AddressTypeCode = msgAddress.AddressTypeCode
                        impAddress.CountryCode = msgAddress.CountryCode
                        impAddress.PostCode = msgAddress.PostCode
                        oParty.Addresses(iCnt) = DirectCast(impAddress, BaseAddressWithContactsType)
                    Next iCnt%
                End If

                Dim impContact As New SiriusFS.SAM.Structure.SFI.SAMForInsurance.BaseContactType
                Dim msgContact As SiriusFS.SAM.Structure.BaseImplementationTypes.BaseContactType
                If IsArray(oImpResponse.Item.Contacts) Then
                    ilBnd% = oImpResponse.Item.Contacts.GetLowerBound(0)
                    iuBnd% = oImpResponse.Item.Contacts.GetUpperBound(0)
                    ReDim oParty.Contacts(iuBnd%)
                    For iCnt As Integer = ilBnd% To iuBnd%
                        msgContact = oImpResponse.Item.Contacts(iCnt%)
                        impContact = New SiriusFS.SAM.Structure.SFI.SAMForInsurance.BaseContactType
                        impContact.AreaCode = msgContact.AreaCode
                        impContact.ContactDetail = New SiriusFS.SAM.Structure.SFI.SAMForInsurance.BaseContactDetailType
                        impContact.ContactDetail.Item = msgContact.ContactDetail.Item
                        impContact.ContactTypeCode = CType([Enum].ToObject(GetType(BaseImplementationTypes.ContactTypeType), msgContact.ContactTypeCode), ContactTypeType)
                        'impContact.ContactTypeCode = msgContact.ContactTypeCode
                        oParty.Contacts(iCnt%) = impContact
                    Next iCnt%
                End If

                oParty.BranchCode = oImpResponse.Item.BranchCode
                oParty.TPIntroducer = oImpResponse.Item.TPIntroducer
                oParty.TPUserCode = oImpResponse.Item.TPUserCode
                oResponse.PartyTimestamp = oImpResponse.PartyTimestamp
                oResponse.Item = oParty

                If oImpResponse.Item.GetType Is GetType(SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyPCType) Then
                    Dim oResponseParty As New SiriusFS.SAM.Structure.SFI.SAMForInsurance.BasePartyPCType
                    Dim oPartyPC As New SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyPCType
                    oPartyPC = DirectCast(oImpResponse.Item, SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyPCType)
                    oResponseParty = DirectCast(oResponse.Item, SiriusFS.SAM.Structure.SFI.SAMForInsurance.BasePartyPCType)
                    oResponseParty.AlternativeId = oPartyPC.AlternativeId
                    oResponseParty.DateOfBirth = oPartyPC.DateOfBirth
                    oResponseParty.EmployersBusinessCode = oPartyPC.EmployersBusinessCode
                    oResponseParty.EmploymentStatusCode = CType([Enum].ToObject(GetType(BaseImplementationTypes.EmploymentStatusCodeType), oPartyPC.EmploymentStatusCode), EmploymentStatusCodeType)
                    'oResponseParty.EmploymentStatusCode = oPartyPC.EmploymentStatusCode
                    oResponseParty.EmploymentStatusCodeSpecified = oPartyPC.EmploymentStatusCodeSpecified
                    oResponseParty.Forename = oPartyPC.Forename
                    oResponseParty.GenderCode = oPartyPC.GenderCode
                    oResponseParty.Initials = oPartyPC.Initials
                    oResponseParty.MaritalStatusCode = CType([Enum].ToObject(GetType(BaseImplementationTypes.MaritalStatusCodeType), oPartyPC.MaritalStatusCode), MaritalStatusCodeType)
                    'oResponseParty.MaritalStatusCode = oPartyPC.MaritalStatusCode
                    oResponseParty.MaritalStatusCodeSpecified = oPartyPC.MaritalStatusCodeSpecified
                    oResponseParty.OccupationCode = oPartyPC.OccupationCode
                    oResponseParty.Surname = oPartyPC.Surname
                    oResponseParty.Title = oPartyPC.Title
                    oResponse.Item = oResponseParty
                ElseIf oImpResponse.Item.GetType Is GetType(SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyCCType) Then
                    Dim oResponseParty As New SiriusFS.SAM.Structure.SFI.SAMForInsurance.BasePartyCCType
                    Dim oPartyCC As New SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyCCType
                    oPartyCC = DirectCast(oImpResponse.Item, SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyCCType)
                    oResponseParty = DirectCast(oResponse.Item, SiriusFS.SAM.Structure.SFI.SAMForInsurance.BasePartyCCType)
                    oResponseParty.BusinessCode = oPartyCC.BusinessCode
                    oResponseParty.CompanyName = oPartyCC.CompanyName
                    oResponse.Item = oResponseParty
                End If

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetProductByAgent(<XmlElement(elementName:="GetProductByAgentRequest", Namespace:=ServiceNamespace)> ByVal GetProductByAgentRequest As GetProductByAgentRequestType) As <XmlElement(elementName:="GetProductByAgentResponse", Namespace:=ServiceNamespace)> GetProductByAgentResponseType

        Try
            CheckAuthority("SAMGPrdByA")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New GetProductByAgentResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetProductByAgentRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetProductByAgentResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = GetProductByAgentRequest.BranchCode
            oImpRequest.UserName = sUserName

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetProductByAgent(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)



                ' Deserialize the XML from the implementation resultdataset into
                ' the correct messaging format
                Dim oResultDataSet As BaseGetProductByAgentResponseTypeProducts = Nothing
                Dim oResultDataSetObject As Object = Nothing
                Dim oXMLSerializer As New Serialization.XmlSerializer(GetType(BaseGetProductByAgentResponseTypeProducts), m_sDefaultNameSpace)
                If oImpResponse.Products IsNot Nothing Then
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=oImpResponse.Products.OuterXml, oXMLSerializer:=oXMLSerializer, r_oResultDataSet:=oResultDataSetObject, sDefaultNameSpace:=m_sDefaultNameSpace)
                    oResultDataSet = DirectCast(oResultDataSetObject, BaseGetProductByAgentResponseTypeProducts)
                End If

                ' Retrieve the values from the implementation response structure
                oResponse.Products = oResultDataSet

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetRiskByProduct(<XmlElement(elementName:="GetRiskByProductRequest", Namespace:=ServiceNamespace)> ByVal GetRiskByProductRequest As GetRiskByProductRequestType) As <XmlElement(elementName:="GetRiskByProductResponse", Namespace:=ServiceNamespace)> GetRiskByProductResponseType

        Try
            CheckAuthority("SAMGRskPro")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New GetRiskByProductResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetRiskByProductRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetRiskByProductResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = GetRiskByProductRequest.BranchCode
            oImpRequest.ProductCode = GetRiskByProductRequest.ProductCode
            oImpRequest.UserName = sUserName

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetRiskByProduct(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)



                ' Deserialize the XML from the implementation resultdataset into
                ' the correct messaging format
                Dim oResultDataSet As BaseGetRiskByProductResponseTypeRisks = Nothing
                Dim oResultDataSetObject As Object = Nothing

                If oImpResponse.Risks IsNot Nothing Then
                    Dim oXMLSerializer As New Serialization.XmlSerializer(GetType(BaseGetRiskByProductResponseTypeRisks), m_sDefaultNameSpace)
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=oImpResponse.Risks.OuterXml, oXMLSerializer:=oXMLSerializer, r_oResultDataSet:=oResultDataSetObject, sDefaultNameSpace:=m_sDefaultNameSpace)
                    oResultDataSet = DirectCast(oResultDataSetObject, BaseGetRiskByProductResponseTypeRisks)
                End If

                ' Retrieve the values from the implementation response structure
                oResponse.Risks = oResultDataSet

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetRatingDetails(<XmlElement(elementName:="GetRatingDetailsRequest", Namespace:=ServiceNamespace)> ByVal GetRatingDetailsRequest As GetRatingDetailsRequestType) As <XmlElement(elementName:="GetRatingDetailsResponse", Namespace:=ServiceNamespace)> GetRatingDetailsResponseType

        Try
            CheckAuthority("SAMGRatDet")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New GetRatingDetailsResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetRatingDetailsRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetRatingDetailsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = GetRatingDetailsRequest.BranchCode
            oImpRequest.InsuranceFileKey = GetRatingDetailsRequest.InsuranceFileKey
            oImpRequest.RiskKey = GetRatingDetailsRequest.RiskKey
            oImpRequest.UserName = sUserName

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetRatingDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Deserialize the XML from the implementation resultdataset into
                ' the correct messaging format
                Dim oResultDataSet As BaseGetRatingDetailsResponseTypeRatingDetails = Nothing
                Dim oResultDataSetObject As Object = Nothing

                If oImpResponse.RatingDetails IsNot Nothing Then
                    Dim oXMLSerializer As New Serialization.XmlSerializer(GetType(BaseGetRatingDetailsResponseTypeRatingDetails), m_sDefaultNameSpace)
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=oImpResponse.RatingDetails.OuterXml, oXMLSerializer:=oXMLSerializer, r_oResultDataSet:=oResultDataSetObject, sDefaultNameSpace:=m_sDefaultNameSpace)
                    oResultDataSet = DirectCast(oResultDataSetObject, BaseGetRatingDetailsResponseTypeRatingDetails)
                End If
                ' Retrieve the values from the implementation response structure
                oResponse.RatingDetails = oResultDataSet

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetRisk(<XmlElement(elementName:="GetRiskRequest", Namespace:=ServiceNamespace)> ByVal GetRiskRequest As GetRiskRequestType) As <XmlElement(elementName:="GetRiskResponse", Namespace:=ServiceNamespace)> GetRiskResponseType

        Try
            CheckAuthority("SAMGRsk")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New GetRiskResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetRiskRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetRiskResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = GetRiskRequest.BranchCode
            oImpRequest.InsuranceFileKey = GetRiskRequest.InsuranceFileKey
            oImpRequest.InsuranceFolderKey = GetRiskRequest.InsuranceFolderKey
            oImpRequest.QuoteTimeStamp = GetRiskRequest.QuoteTimeStamp
            oImpRequest.RiskKey = GetRiskRequest.RiskKey
            oImpRequest.UserName = sUserName

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetRisk(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.CommissionAmount = oImpResponse.CommissionAmount
                oResponse.PremiumDueGross = oImpResponse.PremiumDueGross
                oResponse.PremiumDueNet = oImpResponse.PremiumDueNet
                oResponse.PremiumDueTax = oImpResponse.PremiumDueTax
                oResponse.QuoteTimeStamp = oImpResponse.QuoteTimeStamp
                oResponse.TotalAnnualTax = oImpResponse.TotalAnnualTax
                oResponse.XMLDataSet = SAMFunc.TransformDatasetPBtoSAM(oImpResponse.XMLDataSet)
                oResponse.PolicyLevelTaxesAndFees = ToServiceTaxesAndFeesType(oImpResponse.PolicyLevelTaxesAndFees)
                oResponse.RiskLevelTaxesAndFees = ToServiceTaxesAndFeesType(oImpResponse.RiskLevelTaxesAndFees)

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetUserDetails(<XmlElement(elementName:="GetUserDetailsRequest", Namespace:=ServiceNamespace)> ByVal GetUserDetailsRequest As GetUserDetailsRequestType) As <XmlElement(elementName:="GetUserDetailsResponse", Namespace:=ServiceNamespace)> GetUserDetailsResponseType

        Try
            CheckAuthority("SAMGUsrDet")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New GetUserDetailsResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetUserDetailsRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetUserDetailsResponseType = Nothing

            Try

                ' Call the implementation method
                oImpRequest.Username = sUserName
                oImpRequest.UserId = iUserId
                oImpRequest.AgentKey = iAgentKey

                oImpResponse = DirectCast(oBusiness.GetUserDetails(oImpRequest), SAMForInsuranceImplementationTypes.GetUserDetailsResponseType)

                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.EmailAddress = oImpResponse.EmailAddress
                oResponse.FullUsername = oImpResponse.FullUsername
                If oImpResponse.LastLogin <> Date.MinValue Then
                    oResponse.LastLogin = oImpResponse.LastLogin
                    oResponse.LastLoginSpecified = True
                Else
                    oResponse.LastLoginSpecified = False
                End If
                If oImpResponse.PasswordChangeDate <> Date.MinValue Then
                    oResponse.PasswordChangeDate = oImpResponse.PasswordChangeDate
                    oResponse.PasswordChangeDateSpecified = True
                Else
                    oResponse.PasswordChangeDateSpecified = False
                End If
                oResponse.PartyKey = oImpResponse.PartyKey
                oResponse.PartyKeySpecified = True
                oResponse.PartyName = oImpResponse.PartyName
                oResponse.PartyType = oImpResponse.PartyType
                oResponse.ConsolidatedAgentCommission = oImpResponse.ConsolidatedAgentCommission

                oResponse.SourceList = Array.ConvertAll( _
                            oImpResponse.SourceList, _
                            New Converter(Of BaseImplementationTypes.BaseBranchType,  _
                            BaseBranchType) _
                            (AddressOf ToServiceSourceList))

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function RunDefaultRulesAdd(<XmlElement(elementName:="RunDefaultRulesAddRequest", Namespace:=ServiceNamespace)> ByVal RunDefaultRulesAddRequest As RunDefaultRulesAddRequestType) As <XmlElement(elementName:="RunDefaultRulesAddResponse", Namespace:=ServiceNamespace)> RunDefaultRulesAddResponseType

        Try
            CheckAuthority("SAMRDfRulA")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New RunDefaultRulesAddResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.RunDefaultRulesAddRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.RunDefaultRulesAddResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = RunDefaultRulesAddRequest.BranchCode
            oImpRequest.ScreenCode = RunDefaultRulesAddRequest.ScreenCode
            oImpRequest.UserName = sUserName
            oImpRequest.XMLDataSet = SAMFunc.TransformDatasetSAMtoPB(RunDefaultRulesAddRequest.XMLDataSet)

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.RunDefaultRulesAdd(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.XMLDataSet = SAMFunc.TransformDatasetPBtoSAM(oImpResponse.XMLDataSet)

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function RunDefaultRulesEdit(<XmlElement(elementName:="RunDefaultRulesEditRequest", Namespace:=ServiceNamespace)> ByVal RunDefaultRulesEditRequest As RunDefaultRulesEditRequestType) As <XmlElement(elementName:="RunDefaultRulesEditResponse", Namespace:=ServiceNamespace)> RunDefaultRulesEditResponseType

        Try
            CheckAuthority("SAMRDfRulE")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New RunDefaultRulesEditResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.RunDefaultRulesEditRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.RunDefaultRulesEditResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = RunDefaultRulesEditRequest.BranchCode
            oImpRequest.ScreenCode = RunDefaultRulesEditRequest.ScreenCode
            oImpRequest.UserName = sUserName
            oImpRequest.XMLDataSet = SAMFunc.TransformDatasetSAMtoPB(RunDefaultRulesEditRequest.XMLDataSet)
            oImpRequest.InceptionDateTPI = RunDefaultRulesEditRequest.InceptionDateTPI
            oImpRequest.ClaimKeySpecified = RunDefaultRulesEditRequest.ClaimKeySpecified
            oImpRequest.ClaimPerilKeySpecified = RunDefaultRulesEditRequest.ClaimPerilKeySpecified

            If RunDefaultRulesEditRequest.ClaimKeySpecified Then
                oImpRequest.ClaimKey = RunDefaultRulesEditRequest.ClaimKey
            End If

            If RunDefaultRulesEditRequest.ClaimPerilKeySpecified Then
                oImpRequest.ClaimPerilKey = RunDefaultRulesEditRequest.ClaimPerilKey
            End If

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.RunDefaultRulesEdit(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.XMLDataSet = SAMFunc.TransformDatasetPBtoSAM(oImpResponse.XMLDataSet)

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function UpdateQuote(<XmlElement(elementName:="UpdateQuoteRequest", Namespace:=ServiceNamespace)> ByVal UpdateQuoteRequest As UpdateQuoteRequestType) As <XmlElement(elementName:="UpdateQuoteResponse", Namespace:=ServiceNamespace)> UpdateQuoteResponseType

        Try
            CheckAuthority("SAMUQuot")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New UpdateQuoteResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.UpdateQuoteRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.UpdateQuoteResponseType = Nothing

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
            oImpRequest.CurrencyCode = UpdateQuoteRequest.CurrencyCode

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateQuote(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.QuoteTimeStamp = oImpResponse.QuoteTimeStamp

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function UpdateParty(<XmlElement(elementName:="UpdatePartyRequest", Namespace:=ServiceNamespace)> ByVal UpdatePartyRequest As UpdatePartyRequestType) As <XmlElement(elementName:="UpdatePartyResponse", Namespace:=ServiceNamespace)> UpdatePartyResponseType

        Try
            CheckAuthority("SAMUPty")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim iLBnd As Integer = 0
            Dim iUBnd As Integer = 0

            Dim oResponse As New UpdatePartyResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.UpdatePartyRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.UpdatePartyResponseType = Nothing

            ' Convert the incoming interface structures into the implementation structures
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = UpdatePartyRequest.BranchCode
            oImpRequest.SubBranchCode = UpdatePartyRequest.SubBranchCode
            oImpRequest.PartyKey = UpdatePartyRequest.PartyKey
            oImpRequest.PartyTimestamp = UpdatePartyRequest.PartyTimestamp
            oImpRequest.UserName = sUserName

            ' Process the Party structure.  We 1st need to check the party type of the incoming message
            If UpdatePartyRequest.Item IsNot Nothing Then
                Dim impParty As New BaseImplementationTypes.BasePartyType
                Dim impAddress As New BaseImplementationTypes.BaseAddressWithContactsType
                Dim impContact As New BaseImplementationTypes.BaseContactType

                Dim msgAddress As BaseAddressType = Nothing
                Dim msgContact As BaseContactType = Nothing

                ' Check the type of the party object to see if it is Personal or Corporate
                If UpdatePartyRequest.Item.GetType Is GetType(BasePartyPCType) Then

                    ' Process personal client
                    Dim msgParty As BasePartyPCType = DirectCast(UpdatePartyRequest.Item, BasePartyPCType)
                    Dim impPartyPC As New BaseImplementationTypes.BasePartyPCType

                    impPartyPC.BranchCode = msgParty.BranchCode
                    impPartyPC.Forename = msgParty.Forename
                    impPartyPC.Surname = msgParty.Surname
                    impPartyPC.Initials = msgParty.Initials
                    impPartyPC.Title = msgParty.Title
                    impPartyPC.DateOfBirth = msgParty.DateOfBirth
                    impPartyPC.GenderCode = msgParty.GenderCode
                    impPartyPC.MaritalStatusCode = CType([Enum].ToObject(GetType(MaritalStatusCodeType), msgParty.MaritalStatusCode), BaseImplementationTypes.MaritalStatusCodeType)
                    'impPartyPC.MaritalStatusCode = msgParty.MaritalStatusCode
                    impPartyPC.OccupationCode = msgParty.OccupationCode
                    impPartyPC.EmploymentStatusCode = CType([Enum].ToObject(GetType(EmploymentStatusCodeType), msgParty.EmploymentStatusCode), BaseImplementationTypes.EmploymentStatusCodeType)
                    'impPartyPC.EmploymentStatusCode = msgParty.EmploymentStatusCode
                    impPartyPC.EmployersBusinessCode = msgParty.EmployersBusinessCode
                    impPartyPC.AlternativeId = msgParty.AlternativeId
                    impPartyPC.FileCode = msgParty.FileCode
                    impParty = impPartyPC

                ElseIf UpdatePartyRequest.Item.GetType Is GetType(BasePartyCCType) Then

                    ' Process corporate client
                    Dim msgParty As BasePartyCCType = DirectCast(UpdatePartyRequest.Item, BasePartyCCType)
                    Dim impPartyCC As New BaseImplementationTypes.BasePartyCCType

                    impPartyCC.BranchCode = msgParty.BranchCode
                    impPartyCC.CompanyName = msgParty.CompanyName
                    impPartyCC.BusinessCode = msgParty.BusinessCode
                    impPartyCC.MainContact = msgParty.MainContact
                    impPartyCC.FileCode = msgParty.FileCode
                    impParty = impPartyCC
                ElseIf UpdatePartyRequest.Item.GetType Is GetType(BasePartyOTHERType) Then

                    Dim objPartyOther As BasePartyOTHERType = DirectCast(UpdatePartyRequest.Item, BasePartyOTHERType)
                    Dim impPartyOther As New BaseImplementationTypes.BasePartyOTHERType

                    ' get convictions
                    If objPartyOther.Conviction IsNot Nothing Then
                        impPartyOther.Conviction = Array.ConvertAll(objPartyOther.Conviction, _
                                                    New Converter(Of BasePartyOTHERTypeConviction,  _
                                                    BaseImplementationTypes.BasePartyOTHERTypeConviction) _
                                                    (AddressOf ToBaseImpBasePartyOTHERTypeConviction))
                    End If

                    If objPartyOther.Accident IsNot Nothing Then
                        ' get accidents
                        impPartyOther.Accident = Array.ConvertAll(objPartyOther.Accident, _
                                                    New Converter(Of BasePartyOTHERTypeAccident,  _
                                                    BaseImplementationTypes.BasePartyOTHERTypeAccident) _
                                                    (AddressOf ToBaseImpBasePartyOTHERTypeAccident))

                    End If

                    If objPartyOther.SupplierBusiness IsNot Nothing Then
                        ' get supplier business
                        impPartyOther.SupplierBusiness = Array.ConvertAll(objPartyOther.SupplierBusiness, _
                                                    New Converter(Of BasePartyOTHERTypeSupplierBusiness,  _
                                                    BaseImplementationTypes.BasePartyOTHERTypeSupplierBusiness) _
                                                    (AddressOf ToBaseImpBasePartyOTHERTypeSupplierBusiness))

                    End If

                    impPartyOther.ActiveIndicator = objPartyOther.ActiveIndicator
                    impPartyOther.AfterHoursIndicator = objPartyOther.AfterHoursIndicator
                    impPartyOther.Code = objPartyOther.Code

                    impPartyOther.DateOfBirth = objPartyOther.DateOfBirth
                    impPartyOther.DriverStatusCode = objPartyOther.DriverStatusCode
                    impPartyOther.Gender = objPartyOther.Gender

                    impPartyOther.LicenseNumber = objPartyOther.LicenseNumber
                    impPartyOther.LicenseTypeCode = objPartyOther.LicenseTypeCode
                    impPartyOther.Name = objPartyOther.Name

                    impPartyOther.PriorityIndicator = objPartyOther.PriorityIndicator
                    impPartyOther.RegNumber = objPartyOther.RegNumber
                    impPartyOther.TypeCode = objPartyOther.TypeCode
                    impPartyOther.FileCode = objPartyOther.FileCode
                    impParty = DirectCast(impPartyOther, BaseImplementationTypes.BasePartyType)

                    impParty.BranchCode = UpdatePartyRequest.Item.BranchCode

                    oImpRequest.Party = impParty

                End If

                If UpdatePartyRequest.Item.Addresses IsNot Nothing Then
                    impParty.Addresses = Array.ConvertAll(UpdatePartyRequest.Item.Addresses, _
                                                New Converter(Of BaseAddressWithContactsType,  _
                                                BaseImplementationTypes.BaseAddressWithContactsType) _
                                                (AddressOf ToBaseImpBaseAddressWithContactsType))
                End If

                If UpdatePartyRequest.Item.Contacts IsNot Nothing Then
                    impParty.Contacts = Array.ConvertAll(UpdatePartyRequest.Item.Contacts, _
                                                New Converter(Of BaseContactType,  _
                                                BaseImplementationTypes.BaseContactType) _
                                                (AddressOf ToBaseImpBaseContactType))
                End If

                oImpRequest.Party = impParty

                ' set the tax details for all parties regardless of type
                If UpdatePartyRequest.Item.DomiciledForTaxSpecified Then
                    oImpRequest.Party.DomiciledForTaxSpecified = UpdatePartyRequest.Item.DomiciledForTaxSpecified
                    oImpRequest.Party.DomiciledForTax = UpdatePartyRequest.Item.DomiciledForTax
                End If

                If UpdatePartyRequest.Item.TaxExemptSpecified Then
                    oImpRequest.Party.TaxExemptSpecified = UpdatePartyRequest.Item.TaxExemptSpecified
                    oImpRequest.Party.TaxExempt = UpdatePartyRequest.Item.TaxExempt
                End If

                If UpdatePartyRequest.Item.TaxPercentageSpecified Then
                    oImpRequest.Party.TaxPercentageSpecified = UpdatePartyRequest.Item.TaxPercentageSpecified
                    oImpRequest.Party.TaxPercentage = UpdatePartyRequest.Item.TaxPercentage
                End If

                If UpdatePartyRequest.Item.TaxNumber IsNot Nothing Then
                    oImpRequest.Party.TaxNumber = UpdatePartyRequest.Item.TaxNumber
                End If

                If UpdatePartyRequest.Item.XMLDataset IsNot Nothing Then
                    oImpRequest.Party.XMLDataset = UpdatePartyRequest.Item.XMLDataset
                End If

            End If

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateParty(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.PartyTimestamp = oImpResponse.PartyTimestamp

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function UpdateRisk(<XmlElement(elementName:="UpdateRiskRequest", Namespace:=ServiceNamespace)> ByVal UpdateRiskRequest As UpdateRiskRequestType) As <XmlElement(elementName:="UpdateRiskResponse", Namespace:=ServiceNamespace)> UpdateRiskResponseType

        Try
            CheckAuthority("SAMURsk")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New UpdateRiskResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.UpdateRiskRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.UpdateRiskResponseType = Nothing

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
                oResponse.XMLDataSet = SAMFunc.TransformDatasetPBtoSAM(oImpResponse.XMLDataSet)
                oResponse.PolicyLevelTaxesAndFees = ToServiceTaxesAndFeesType(oImpResponse.PolicyLevelTaxesAndFees)
                oResponse.RiskLevelTaxesAndFees = ToServiceTaxesAndFeesType(oImpResponse.RiskLevelTaxesAndFees)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetClaimDetails(<XmlElement(elementName:="GetClaimDetailsRequest", Namespace:=ServiceNamespace)> ByVal GetClaimRequest As GetClaimDetailsRequestType) As <XmlElement(elementName:="GetClaimDetailsResponse", Namespace:=ServiceNamespace)> GetClaimDetailsResponseType

        Try
            CheckAuthority("SAMGClmDet")

            Dim oResponse As New GetClaimDetailsResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetClaimDetailsRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetClaimDetailsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetClaimRequest.BranchCode
            oImpRequest.ClaimKey = GetClaimRequest.ClaimKey

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetClaimDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                Dim oClaimDet As New SiriusFS.SAM.Structure.SFI.SAMForInsurance.BaseGetClaimDetailsTypeClaimDetails

                With oImpResponse.ClaimDetails.ClaimDetails

                    oResponse.TimeStamp = oImpResponse.TimeStamp

                    oResponse.ClaimDetails = New SiriusFS.SAM.Structure.SFI.SAMForInsurance.BaseGetClaimDetailsType
                    oResponse.ClaimDetails.ClaimDetails = New SiriusFS.SAM.Structure.SFI.SAMForInsurance.BaseGetClaimDetailsTypeClaimDetails

                    oResponse.ClaimDetails.ClaimDetails.BaseClaimKey = oImpResponse.ClaimDetails.ClaimDetails.BaseClaimKey
                    oResponse.ClaimDetails.ClaimDetails.ClaimKey = oImpResponse.ClaimDetails.ClaimDetails.ClaimKey
                    oResponse.ClaimDetails.ClaimDetails.CatastropheCode = oImpResponse.ClaimDetails.ClaimDetails.CatastropheCode
                    oResponse.ClaimDetails.ClaimDetails.ClaimVersionDescription = .ClaimVersionDescription
                    oResponse.ClaimDetails.ClaimDetails.Comments = .Comments
                    oResponse.ClaimDetails.ClaimDetails.GisScreenCode = .GisScreenCode

                    oResponse.ClaimDetails.ClaimDetails.CurrencyCode = .CurrencyCode
                    oResponse.ClaimDetails.ClaimDetails.Description = .Description
                    oResponse.ClaimDetails.ClaimDetails.HandlerCode = .HandlerCode
                    oResponse.ClaimDetails.ClaimDetails.InfoOnly = .InfoOnly

                    oResponse.ClaimDetails.ClaimDetails.InsuranceFileKey = .InsuranceFileKey
                    oResponse.ClaimDetails.ClaimDetails.LikelyClaim = .LikelyClaim
                    oResponse.ClaimDetails.ClaimDetails.Location = .Location
                    oResponse.ClaimDetails.ClaimDetails.LossFromDate = .LossFromDate

                    oResponse.ClaimDetails.ClaimDetails.LossToDate = .LossToDate
                    oResponse.ClaimDetails.ClaimDetails.LossToDateSpecified = .LossToDateSpecified
                    oResponse.ClaimDetails.ClaimDetails.PrimaryCauseCode = .PrimaryCauseCode
                    oResponse.ClaimDetails.ClaimDetails.ProgressStatusCode = .ProgressStatusCode

                    oResponse.ClaimDetails.ClaimDetails.ReportedDate = .ReportedDate
                    oResponse.ClaimDetails.ClaimDetails.RiskKey = .RiskKey

                    oResponse.ClaimDetails.ClaimDetails.SecondaryCauseCode = .SecondaryCauseCode
                    oResponse.ClaimDetails.ClaimDetails.TownCode = .TownCode
                    oResponse.ClaimDetails.ClaimDetails.UnderwritingYearCode = .UnderwritingYearCode

                    oResponse.ClaimDetails.ClaimDetails.UserDefFldACode = .UserDefFldACode
                    oResponse.ClaimDetails.ClaimDetails.UserDefFldBCode = .UserDefFldBCode
                    oResponse.ClaimDetails.ClaimDetails.UserDefFldCCode = .UserDefFldCCode
                    oResponse.ClaimDetails.ClaimDetails.UserDefFldDCode = .UserDefFldDCode
                    oResponse.ClaimDetails.ClaimDetails.UserDefFldECode = .UserDefFldECode

                    oResponse.ClaimDetails.ClaimDetails.Client = New BaseClaimPartyClientType
                    oResponse.ClaimDetails.ClaimDetails.Client.PartyClaimNumber = .Client.PartyClaimNumber
                    oResponse.ClaimDetails.ClaimDetails.Client.TaxRegistered = .Client.TaxRegistered
                    oResponse.ClaimDetails.ClaimDetails.Client.TaxRegistrationNumber = .Client.TaxRegistrationNumber

                    'uncomment after finding the bug
                    If oImpResponse.ClaimDetails.ClaimDetails.Client.Address IsNot Nothing Then

                        oResponse.ClaimDetails.ClaimDetails.Client.Address = New BaseAddressType
                        oResponse.ClaimDetails.ClaimDetails.Client.Address.AddressLine1 = .Client.Address.AddressLine1
                        oResponse.ClaimDetails.ClaimDetails.Client.Address.AddressLine2 = .Client.Address.AddressLine2
                        oResponse.ClaimDetails.ClaimDetails.Client.Address.AddressLine3 = .Client.Address.AddressLine3
                        oResponse.ClaimDetails.ClaimDetails.Client.Address.AddressLine4 = .Client.Address.AddressLine4
                        oResponse.ClaimDetails.ClaimDetails.Client.Address.AddressTypeCode = CType([Enum].ToObject(GetType(BaseImplementationTypes.AddressTypeType), .Client.Address.AddressTypeCode), AddressTypeType)
                        'oResponse.ClaimDetails.ClaimDetails.Client.Address.AddressTypeCode = .Client.Address.AddressTypeCode
                        oResponse.ClaimDetails.ClaimDetails.Client.Address.CountryCode = .Client.Address.CountryCode
                        oResponse.ClaimDetails.ClaimDetails.Client.Address.PostCode = .Client.Address.PostCode

                    End If

                    If oImpResponse.ClaimDetails.ClaimDetails.Insurer IsNot Nothing Then

                        oResponse.ClaimDetails.ClaimDetails.Insurer = New BaseClaimPartyInsurerType
                        oResponse.ClaimDetails.ClaimDetails.Insurer.PartyClaimNumber = .Client.PartyClaimNumber

                        ' TODO : MEvans : This information is missing from the response object
                        oResponse.ClaimDetails.ClaimDetails.Insurer.ContactName = String.Empty
                        'oResponse.ClaimDetails.ClaimDetails.Insurer.ContactName = .Client.TaxRegistered

                        If oImpResponse.ClaimDetails.ClaimDetails.Insurer.Address IsNot Nothing Then
                            oResponse.ClaimDetails.ClaimDetails.Insurer.Address = New BaseAddressType
                            oResponse.ClaimDetails.ClaimDetails.Insurer.Address.AddressLine1 = .Insurer.Address.AddressLine1
                            oResponse.ClaimDetails.ClaimDetails.Insurer.Address.AddressLine2 = .Insurer.Address.AddressLine2
                            oResponse.ClaimDetails.ClaimDetails.Insurer.Address.AddressLine3 = .Insurer.Address.AddressLine3
                            oResponse.ClaimDetails.ClaimDetails.Insurer.Address.AddressLine4 = .Insurer.Address.AddressLine4
                            oResponse.ClaimDetails.ClaimDetails.Insurer.Address.AddressTypeCode = CType([Enum].ToObject(GetType(BaseImplementationTypes.AddressTypeType), .Insurer.Address.AddressTypeCode), AddressTypeType)
                            'oResponse.ClaimDetails.ClaimDetails.Insurer.Address.AddressTypeCode = .Insurer.Address.AddressTypeCode                          
                            oResponse.ClaimDetails.ClaimDetails.Insurer.Address.CountryCode = .Insurer.Address.CountryCode
                            oResponse.ClaimDetails.ClaimDetails.Insurer.Address.PostCode = .Insurer.Address.PostCode
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

                ReDim oResponse.ClaimDetails.ClaimPeril(uBoundPeril)

                With oImpResponse.ClaimDetails.ClaimPeril(cntPerils)

                    For cntPerils = lBoundPeril To uBoundPeril

                        oResponse.ClaimDetails.ClaimPeril(cntPerils) = New BaseGetClaimPerilDetailsType
                        oResponse.ClaimDetails.ClaimPeril(cntPerils).BaseClaimPerilKey = oImpResponse.ClaimDetails.ClaimPeril(cntPerils).BaseClaimPerilKey
                        oResponse.ClaimDetails.ClaimPeril(cntPerils).Comments = oImpResponse.ClaimDetails.ClaimPeril(cntPerils).Comments
                        oResponse.ClaimDetails.ClaimPeril(cntPerils).Description = oImpResponse.ClaimDetails.ClaimPeril(cntPerils).Description
                        oResponse.ClaimDetails.ClaimPeril(cntPerils).GisScreenCode = oImpResponse.ClaimDetails.ClaimPeril(cntPerils).GisScreenCode
                        oResponse.ClaimDetails.ClaimPeril(cntPerils).RIBand = oImpResponse.ClaimDetails.ClaimPeril(cntPerils).RIBand
                        oResponse.ClaimDetails.ClaimPeril(cntPerils).SumInsured = oImpResponse.ClaimDetails.ClaimPeril(cntPerils).SumInsured
                        oResponse.ClaimDetails.ClaimPeril(cntPerils).TypeCode = oImpResponse.ClaimDetails.ClaimPeril(cntPerils).TypeCode
                        oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPerilKey = oImpResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPerilKey

                        If Not oImpResponse.ClaimDetails.ClaimPeril(cntPerils).Reserve Is Nothing Then

                            Dim lBoundRes As Integer = oImpResponse.ClaimDetails.ClaimPeril(cntPerils).Reserve.GetLowerBound(0)
                            Dim uBoundRes As Integer = oImpResponse.ClaimDetails.ClaimPeril(cntPerils).Reserve.GetUpperBound(0)

                            oResponse.ClaimDetails.ClaimPeril(cntPerils).Reserve = Nothing

                            ReDim oResponse.ClaimDetails.ClaimPeril(cntPerils).Reserve(uBoundRes)

                            For cntReserves = lBoundRes To uBoundRes

                                oResponse.ClaimDetails.ClaimPeril(cntPerils).Reserve(cntReserves) = New BaseGetClaimReserveDetailsType
                                oResponse.ClaimDetails.ClaimPeril(cntPerils).Reserve(cntReserves).BaseReserveKey = oImpResponse.ClaimDetails.ClaimPeril(cntPerils).Reserve(cntReserves).BaseReserveKey
                                oResponse.ClaimDetails.ClaimPeril(cntPerils).Reserve(cntReserves).InitialReserve = oImpResponse.ClaimDetails.ClaimPeril(cntPerils).Reserve(cntReserves).InitialReserve
                                oResponse.ClaimDetails.ClaimPeril(cntPerils).Reserve(cntReserves).PaidAmount = oImpResponse.ClaimDetails.ClaimPeril(cntPerils).Reserve(cntReserves).PaidAmount
                                oResponse.ClaimDetails.ClaimPeril(cntPerils).Reserve(cntReserves).RevisedReserve = oImpResponse.ClaimDetails.ClaimPeril(cntPerils).Reserve(cntReserves).RevisedReserve
                                oResponse.ClaimDetails.ClaimPeril(cntPerils).Reserve(cntReserves).SumInsured = oImpResponse.ClaimDetails.ClaimPeril(cntPerils).Reserve(cntReserves).SumInsured
                                oResponse.ClaimDetails.ClaimPeril(cntPerils).Reserve(cntReserves).TypeCode = oImpResponse.ClaimDetails.ClaimPeril(cntPerils).Reserve(cntReserves).TypeCode

                            Next

                        End If

                        If Not .Recovery Is Nothing Then
                            Dim lBoundRec As Integer = .Recovery.GetLowerBound(0)
                            Dim uBoundRec As Integer = .Recovery.GetUpperBound(0)
                            oResponse.ClaimDetails.ClaimPeril(cntPerils).Recovery = Nothing
                            ReDim oResponse.ClaimDetails.ClaimPeril(cntPerils).Recovery(uBoundRec)

                            For cntRecoveries = lBoundRec To uBoundRec

                                oResponse.ClaimDetails.ClaimPeril(cntPerils).Recovery(cntRecoveries) = New BaseGetClaimRecoveryDetailsType

                                oResponse.ClaimDetails.ClaimPeril(cntPerils).Recovery(cntRecoveries).BaseRecoveryKey = .Recovery(cntRecoveries).BaseRecoveryKey
                                oResponse.ClaimDetails.ClaimPeril(cntPerils).Recovery(cntRecoveries).CurrencyCode = .Recovery(cntRecoveries).CurrencyCode
                                oResponse.ClaimDetails.ClaimPeril(cntPerils).Recovery(cntRecoveries).InitialRecovery = .Recovery(cntRecoveries).InitialRecovery
                                oResponse.ClaimDetails.ClaimPeril(cntPerils).Recovery(cntRecoveries).ReceiptedAmount = .Recovery(cntRecoveries).ReceiptedAmount
                                oResponse.ClaimDetails.ClaimPeril(cntPerils).Recovery(cntRecoveries).ReceiptedTaxAmount = .Recovery(cntRecoveries).ReceiptedTaxAmount
                                oResponse.ClaimDetails.ClaimPeril(cntPerils).Recovery(cntRecoveries).RevisedRecovery = .Recovery(cntRecoveries).RevisedRecovery
                                oResponse.ClaimDetails.ClaimPeril(cntPerils).Recovery(cntRecoveries).TypeCode = .Recovery(cntRecoveries).TypeCode
                            Next
                        End If

                        If Not oImpResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments Is Nothing Then
                            Dim lBoundPay As Integer = oImpResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments.GetLowerBound(0)
                            Dim uBoundPay As Integer = oImpResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments.GetUpperBound(0)
                            oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments = Nothing
                            ReDim oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(uBoundPay)
                            For cntPay = lBoundPay To uBoundPay
                                Dim obj As New SiriusFS.SAM.Structure.BaseImplementationTypes.BaseGetClaimPaymentDetailsType
                                obj = oImpResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay)
                                oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay) = New BaseGetClaimPaymentDetailsType

                                oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).BaseClaimPaymentKey = obj.BaseClaimPaymentKey
                                oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).CurrencyCode = obj.CurrencyCode
                                oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).IsReferred = obj.IsReferred
                                oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).PartyKey = obj.PartyKey
                                oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).PaymentAmount = obj.PaymentAmount
                                oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).PaymentDate = obj.PaymentDate
                                oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).PaymentPartyType = obj.PaymentPartyType

                                'oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).BaseClaimPaymentKey = .ClaimPayments(cntPay).BaseClaimPaymentKey
                                'oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).CurrencyCode = .ClaimPayments(cntPay).CurrencyCode
                                'oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).IsReferred = .ClaimPayments(cntPay).IsReferred
                                'oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).PartyKey = .ClaimPayments(cntPay).PartyKey
                                'oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).PaymentAmount = .ClaimPayments(cntPay).PaymentAmount
                                'oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).PaymentDate = .ClaimPayments(cntPay).PaymentDate
                                'oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).PaymentPartyType = .ClaimPayments(cntPay).PaymentPartyType

                                If Not obj.AdvancedTaxDetails Is Nothing Then
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).AdvancedTaxDetails = New BaseClaimPaymentAdvancedTaxDetailsType
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).AdvancedTaxDetails.InsuranceTaxNumber = obj.AdvancedTaxDetails.InsuranceTaxNumber
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).AdvancedTaxDetails.InsuredDomiciled = obj.AdvancedTaxDetails.InsuredDomiciled
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).AdvancedTaxDetails.InsuredPercentage = obj.AdvancedTaxDetails.InsuredPercentage
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).AdvancedTaxDetails.IsSettlement = obj.AdvancedTaxDetails.IsSettlement
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).AdvancedTaxDetails.IsTaxExempt = obj.AdvancedTaxDetails.IsTaxExempt
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).AdvancedTaxDetails.IsWHTExempt = obj.AdvancedTaxDetails.IsWHTExempt
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).AdvancedTaxDetails.PayeeDomiciled = obj.AdvancedTaxDetails.PayeeDomiciled

                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).AdvancedTaxDetails.PayeePercentage = obj.AdvancedTaxDetails.PayeePercentage
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).AdvancedTaxDetails.PayeeTaxNumber = obj.AdvancedTaxDetails.PayeeTaxNumber
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).AdvancedTaxDetails.SafeHarbourCode = obj.AdvancedTaxDetails.SafeHarbourCode
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).AdvancedTaxDetails.SafeHarbourPercentage = Cast.ToDecimal(obj.AdvancedTaxDetails.SafeHarbourPercentage, 0)
                                End If

                                If Not obj.Payee Is Nothing Then
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).Payee = New BaseClaimPayeeType
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).Payee.BankCode = obj.Payee.BankCode
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).Payee.BankName = obj.Payee.BankName
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).Payee.BankNumber = obj.Payee.BankNumber
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).Payee.MediaReference = obj.Payee.MediaReference
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).Payee.MediaTypeCode = obj.Payee.MediaTypeCode
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).Payee.Name = obj.Payee.Name
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).Payee.TheirReference = obj.Payee.TheirReference

                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).Payee.Address = New BaseAddressType
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).Payee.Address.AddressLine1 = obj.Payee.Address.AddressLine1
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).Payee.Address.AddressLine2 = obj.Payee.Address.AddressLine2
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).Payee.Address.AddressLine3 = obj.Payee.Address.AddressLine3
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).Payee.Address.AddressLine4 = obj.Payee.Address.AddressLine4
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).Payee.Address.AddressTypeCode = CType([Enum].ToObject(GetType(BaseImplementationTypes.AddressTypeType), obj.Payee.Address.AddressTypeCode), AddressTypeType)
                                    'oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).Payee.Address.AddressTypeCode = .ClaimPayments(cntPay).Payee.Address.AddressTypeCode
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).Payee.Address.CountryCode = obj.Payee.Address.CountryCode
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).Payee.Address.PostCode = obj.Payee.Address.PostCode
                                End If

                                If Not obj.ClaimPaymentItems Is Nothing Then
                                    Dim lBoundPayItem As Integer = obj.ClaimPaymentItems.GetLowerBound(0)
                                    Dim uBoundPayItem As Integer = obj.ClaimPaymentItems.GetUpperBound(0)
                                    ReDim oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).ClaimPaymentItems(uBoundPayItem)

                                    For cntPayItem = lBoundPayItem To uBoundPayItem
                                        oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).ClaimPaymentItems(cntPayItem) = New BaseGetClaimPaymentItemDetailsType

                                        If Not obj.ClaimPaymentItems(cntPayItem) Is Nothing Then
                                            oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).ClaimPaymentItems(cntPayItem).BaseClaimPaymentItemKey = obj.ClaimPaymentItems(cntPayItem).BaseClaimPaymentItemKey
                                            oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).ClaimPaymentItems(cntPayItem).BaseRecoveryKey = obj.ClaimPaymentItems(cntPayItem).BaseRecoveryKey
                                            oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).ClaimPaymentItems(cntPayItem).BaseReserveKey = obj.ClaimPaymentItems(cntPayItem).BaseReserveKey
                                            oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).ClaimPaymentItems(cntPayItem).PaymentAdjustment = obj.ClaimPaymentItems(cntPayItem).PaymentAdjustment
                                            oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).ClaimPaymentItems(cntPayItem).PaymentAmount = obj.ClaimPaymentItems(cntPayItem).PaymentAmount
                                            oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).ClaimPaymentItems(cntPayItem).TaxAmount = obj.ClaimPaymentItems(cntPayItem).TaxAmount
                                            oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).TaxAmount = oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).TaxAmount + obj.ClaimPaymentItems(cntPayItem).TaxAmount
                                        End If
                                    Next
                                End If
                            Next
                        End If

                        If Not .ClaimReceipts Is Nothing Then
                            Dim lBoundRece As Integer = .ClaimReceipts.GetLowerBound(0)
                            Dim uBoundRece As Integer = .ClaimReceipts.GetUpperBound(0)
                            ReDim oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(uBoundRece)
                            For cntRec = lBoundRece To uBoundRece
                                oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec) = New BaseGetClaimReceiptDetailsType
                                oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).BaseClaimReceiptKey = .ClaimReceipts(cntRec).BaseClaimReceiptKey
                                oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).CurrencyCode = .ClaimReceipts(cntRec).CurrencyCode
                                oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).PartyKey = .ClaimReceipts(cntRec).PartyKey
                                oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).ReceiptAmount = .ClaimReceipts(cntRec).ReceiptAmount
                                oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).ReceiptDate = .ClaimReceipts(cntRec).ReceiptDate
                                oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).ReceiptPartyType = .ClaimReceipts(cntRec).ReceiptPartyType

                                If Not .ClaimReceipts(cntRec).AdvancedTax Is Nothing Then
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).AdvancedTax = New BaseClaimReceiptAdvancedTaxDetailsType
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).AdvancedTax.InsuredDomiciled = .ClaimReceipts(cntRec).AdvancedTax.InsuredDomiciled
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).AdvancedTax.InsuredDomiciledSpecified = .ClaimReceipts(cntRec).AdvancedTax.InsuredDomiciledSpecified
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).AdvancedTax.InsuredPercentage = .ClaimReceipts(cntRec).AdvancedTax.InsuredPercentage
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).AdvancedTax.InsuredPercentageSpecified = .ClaimReceipts(cntRec).AdvancedTax.InsuredPercentageSpecified
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).AdvancedTax.InsuredTaxNumber = .ClaimReceipts(cntRec).AdvancedTax.InsuredTaxNumber
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).AdvancedTax.IsSettlement = .ClaimReceipts(cntRec).AdvancedTax.IsSettlement
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).AdvancedTax.IsSettlementSpecified = .ClaimReceipts(cntRec).AdvancedTax.IsSettlementSpecified

                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).AdvancedTax.IsTaxExempt = .ClaimReceipts(cntRec).AdvancedTax.IsTaxExempt
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).AdvancedTax.IsTaxExemptSpecified = .ClaimReceipts(cntRec).AdvancedTax.IsTaxExemptSpecified
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).AdvancedTax.ReceivableTaxPercentage = .ClaimReceipts(cntRec).AdvancedTax.ReceivableTaxPercentage
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).AdvancedTax.ReceivableTaxPercentageSpecified = .ClaimReceipts(cntRec).AdvancedTax.ReceivableTaxPercentageSpecified
                                End If

                                If Not .ClaimReceipts(cntRec).Payee Is Nothing Then
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).Payee = New BaseClaimPayeeType
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).Payee.BankCode = .ClaimReceipts(cntRec).Payee.BankCode
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).Payee.BankName = .ClaimReceipts(cntRec).Payee.BankName
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).Payee.BankNumber = .ClaimReceipts(cntRec).Payee.BankNumber
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).Payee.MediaReference = .ClaimReceipts(cntRec).Payee.MediaReference
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).Payee.MediaTypeCode = .ClaimReceipts(cntRec).Payee.MediaTypeCode
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).Payee.Name = .ClaimReceipts(cntRec).Payee.Name
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).Payee.TheirReference = .ClaimReceipts(cntRec).Payee.TheirReference

                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).Payee.Address = New BaseAddressType
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).Payee.Address.AddressLine1 = .ClaimReceipts(cntRec).Payee.Address.AddressLine1
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).Payee.Address.AddressLine2 = .ClaimReceipts(cntRec).Payee.Address.AddressLine2
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).Payee.Address.AddressLine3 = .ClaimReceipts(cntRec).Payee.Address.AddressLine3
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).Payee.Address.AddressLine4 = .ClaimReceipts(cntRec).Payee.Address.AddressLine4
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).Payee.Address.AddressTypeCode = CType([Enum].ToObject(GetType(BaseImplementationTypes.AddressTypeType), .ClaimReceipts(cntRec).Payee.Address.AddressTypeCode), AddressTypeType)
                                    'oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).Payee.Address.AddressTypeCode = .ClaimReceipts(cntRec).Payee.Address.AddressTypeCode
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).Payee.Address.CountryCode = .ClaimReceipts(cntRec).Payee.Address.CountryCode
                                    oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).Payee.Address.PostCode = .ClaimReceipts(cntRec).Payee.Address.PostCode
                                End If

                                If Not .ClaimReceipts(cntRec).ReceiptItem Is Nothing Then
                                    Dim lBoundRecItem As Integer = .ClaimReceipts(cntRecItem).ReceiptItem.GetLowerBound(0)
                                    Dim uBoundRecItem As Integer = .ClaimReceipts(cntRecItem).ReceiptItem.GetUpperBound(0)
                                    ReDim oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimPayments(cntPay).ClaimPaymentItems(uBoundRecItem)
                                    For cntRecItem = lBoundRecItem To uBoundRecItem

                                        oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).ReceiptItem(cntRecItem) = _
                                        New BaseGetClaimReceiptItemDetailsType

                                        oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).ReceiptItem(cntRec).BaseClaimReceiptItemKey = .ClaimReceipts(cntRec).ReceiptItem(cntRecItem).BaseClaimReceiptItemKey
                                        oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).ReceiptItem(cntRec).BaseRecoveryKey = .ClaimReceipts(cntRec).ReceiptItem(cntRecItem).BaseRecoveryKey
                                        oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).ReceiptItem(cntRec).BaseReserveKey = .ClaimReceipts(cntRec).ReceiptItem(cntRecItem).BaseReserveKey
                                        oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).ReceiptItem(cntRec).ReceiptAmount = .ClaimReceipts(cntRec).ReceiptItem(cntRecItem).ReceiptAmount
                                        oResponse.ClaimDetails.ClaimPeril(cntPerils).ClaimReceipts(cntRec).ReceiptItem(cntRec).TaxAmount = .ClaimReceipts(cntRec).ReceiptItem(cntRecItem).TaxAmount
                                    Next
                                End If
                            Next
                        End If
                    Next
                End With

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function FindClaim(<XmlElement(elementName:="FindClaimRequest", Namespace:=ServiceNamespace)> ByVal FindClaimRequest As FindClaimRequestType) As <XmlElement(elementName:="FindClaimResponse", Namespace:=ServiceNamespace)> FindClaimResponseType

        Try
            CheckAuthority("SAMFClm")

            Dim oResponse As New FindClaimResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.FindClaimRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.FindClaimResponseType = Nothing

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
            oImpRequest.AgentKey = iAgentKey

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.FindClaim(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)


                ' Deserialize the XML from the implementation resultdataset into
                ' the correct messaging format
                Dim oResultDataSet As BaseFindClaimResponseTypeClaims = Nothing
                Dim oResultDataSetObject As Object = Nothing

                If oImpResponse.ResultDataset IsNot Nothing Then
                    Dim oXMLSerializer As New Serialization.XmlSerializer(GetType(BaseFindClaimResponseTypeClaims), m_sDefaultNameSpace)
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=oImpResponse.ResultDataset.OuterXml, oXMLSerializer:=oXMLSerializer, r_oResultDataSet:=oResultDataSetObject, sDefaultNameSpace:=m_sDefaultNameSpace)
                    oResultDataSet = DirectCast(oResultDataSetObject, BaseFindClaimResponseTypeClaims)
                End If

                ' Retrieve the values from the implementation response structure
                oResponse.Claims = oResultDataSet

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function OpenClaim(<XmlElement(elementName:="OpenClaimRequest", Namespace:=ServiceNamespace)> ByVal OpenClaimRequest As OpenClaimRequestType) As <XmlElement(elementName:="OpenClaimResponse", Namespace:=ServiceNamespace)> OpenClaimResponseType

        Try
            CheckAuthority("SAMOClm")

            Dim oResponse As New OpenClaimResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.OpenClaimRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.OpenClaimResponseType = Nothing

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
            oImpClaim.UserDefFldACode = OpenClaimRequest.Claim.UserDefFldACode
            oImpClaim.UserDefFldBCode = OpenClaimRequest.Claim.UserDefFldBCode
            oImpClaim.UserDefFldCCode = OpenClaimRequest.Claim.UserDefFldCCode
            oImpClaim.UserDefFldDCode = OpenClaimRequest.Claim.UserDefFldDCode
            oImpClaim.UserDefFldECode = OpenClaimRequest.Claim.UserDefFldECode

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
                    oImpClient.Contact = Array.ConvertAll( _
                                                OpenClaimRequest.Claim.Client.Contact, _
                                                New Converter(Of BaseContactType,  _
                                                    BaseImplementationTypes.BaseContactType) _
                                                    (AddressOf ToBaseImpBaseContactType))
                End If

                oImpClient.PartyClaimNumber = OpenClaimRequest.Claim.Client.PartyClaimNumber
                oImpClient.TaxRegistered = OpenClaimRequest.Claim.Client.TaxRegistered
                oImpClient.TaxRegistrationNumber = OpenClaimRequest.Claim.Client.TaxRegistrationNumber

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
                    oImpInsurer.Contact = Array.ConvertAll( _
                                                OpenClaimRequest.Claim.Insurer.Contact, _
                                                New Converter(Of BaseContactType,  _
                                                    BaseImplementationTypes.BaseContactType) _
                                                    (AddressOf ToBaseImpBaseContactType))
                End If

                oImpInsurer.ContactName = OpenClaimRequest.Claim.Insurer.ContactName
                oImpInsurer.PartyClaimNumber = OpenClaimRequest.Claim.Insurer.PartyClaimNumber

                ' set the insured into the claim
                oImpClaim.Insurer = oImpInsurer

            End If

            ' if perils were specified in the request
            If OpenClaimRequest.Claim.ClaimPeril IsNot Nothing Then

                ' process the claim peril array
                oImpClaim.ClaimPeril = Array.ConvertAll( _
                                                OpenClaimRequest.Claim.ClaimPeril, _
                                                New Converter(Of BaseClaimPerilType,  _
                                                    BaseImplementationTypes.BaseClaimPerilType) _
                                                    (AddressOf ToBaseImpBaseClaimPerilType))

            End If

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = OpenClaimRequest.BranchCode

            oImpRequest.Claim = oImpClaim

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
                    oResponse.Warnings = Array.ConvertAll( _
                                                    oImpResponse.Warnings, _
                                                    New Converter(Of BaseImplementationTypes.BaseClaimResponseTypeWarnings,  _
                                                        BaseClaimResponseTypeWarnings) _
                                                        (AddressOf ToServiceImpBaseClaimResponseTypeWarnings))

                End If

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    '<WebMethod()> _
    '<SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    'Public Function ClaimMTA(<XmlElement(elementName:="ClaimMTARequest", Namespace:=ServiceNamespace)> ByVal ClaimMTARequest As ClaimMTARequestType) As <XmlElement(elementName:="ClaimMTAResponse", Namespace:=ServiceNamespace)> ClaimMTAResponseType

    '    Try
    '        CheckAuthority("SAMClmMTA")

    '        Dim oResponse As New ClaimMTAResponseType
    '        Dim oBusiness As New SAMForInsuranceBusiness

    '        ' Implementation structures
    '        Dim oImpRequest As New SAMForInsuranceImplementationTypes.ClaimMTARequestType
    '        Dim oImpResponse As SAMForInsuranceImplementationTypes.ClaimMTAResponseType = Nothing

    '        ' Pass the values to the implementation request structure
    '        oImpRequest.BranchCode = ClaimMTARequest.BranchCode
    '        oImpRequest.SubBranchCode = ClaimMTARequest.SubBranchCode
    '        oImpRequest.InsuranceFileKey = ClaimMTARequest.InsuranceFileKey
    '        oImpRequest.InsuranceFolderKey = ClaimMTARequest.InsuranceFolderKey
    '        oImpRequest.QuoteTimeStamp = ClaimMTARequest.QuoteTimeStamp
    '        oImpRequest.RiskDescription = ClaimMTARequest.RiskDescription
    '        oImpRequest.RiskKey = ClaimMTARequest.RiskKey
    '        oImpRequest.ScreenCode = ClaimMTARequest.ScreenCode
    '        'oImpRequest.XMLDataSet = SAMFunc.TransformDatasetSAMtoPB(ClaimMTARequest.XMLDataSet)
    '        'oImpRequest.UserName = ClaimMTARequest.UserName
    '        'oImpRequest.AgentKey = ClaimMTARequest.AgentKey

    '        Try
    '            ' Call the implementation method
    '            oImpResponse = oBusiness.ClaimMTA(oImpRequest)
    '            SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

    '            ' Retrieve the values from the implementation response structure
    '            oResponse.TimeStamp = oImpResponse.TimeStamp
    '            oResponse.InsuranceFileKey = oImpResponse.InsuranceFileKey
    '            oResponse.XMLDataSet = SAMFunc.TransformDatasetPBtoSAM(oImpResponse.XMLDataSet)
    '            oResponse.RiskKey = oImpResponse.RiskKey

    '        Catch ex As Exception
    '            Handler.BusinessLayerBoundary(ex, oResponse)
    '        End Try

    '        Return oResponse

    '    Catch ex As Exception
    '        Handler.BusinessLayerLastResort(ex, Context)
    '        Return Nothing
    '    End Try

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function MaintainClaim(<XmlElement(elementName:="MaintainClaimRequest", Namespace:=ServiceNamespace)> ByVal MaintainClaimRequest As MaintainClaimRequestType) As <XmlElement(elementName:="MaintainClaimResponse", Namespace:=ServiceNamespace)> MaintainClaimResponseType

        Try
            CheckAuthority("SAMMClm")

            Dim oResponse As New MaintainClaimResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            '' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.MaintainClaimRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.MaintainClaimResponseType = Nothing

            Dim oImpClaim As BaseImplementationTypes.BaseClaimMaintainType = New BaseImplementationTypes.BaseClaimMaintainType
            Dim oImpClient As BaseImplementationTypes.BaseClaimPartyClientType = New BaseImplementationTypes.BaseClaimPartyClientType
            Dim oImpInsurer As BaseImplementationTypes.BaseClaimPartyInsurerType = New BaseImplementationTypes.BaseClaimPartyInsurerType

            oImpClaim.IgnoreWarnings = MaintainClaimRequest.Claim.IgnoreWarnings
            oImpClaim.ExternalHandler = MaintainClaimRequest.Claim.ExternalHandler
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

                    oImpClient.Contact = Array.ConvertAll( _
                                                MaintainClaimRequest.Claim.Client.Contact, _
                                                New Converter(Of BaseContactType,  _
                                                    BaseImplementationTypes.BaseContactType) _
                                                    (AddressOf ToBaseImpBaseContactType))

                End If

                oImpClient.PartyClaimNumber = MaintainClaimRequest.Claim.Client.PartyClaimNumber
                oImpClient.TaxRegistered = MaintainClaimRequest.Claim.Client.TaxRegistered
                oImpClient.TaxRegistrationNumber = MaintainClaimRequest.Claim.Client.TaxRegistrationNumber

                ' set client into claim
                oImpClaim.Client = oImpClient
            End If

            ' if the client has been specified in the request
            If MaintainClaimRequest.Claim.Insurer IsNot Nothing Then

                '' Process the insurers addresses
                Dim oInsurerAddress As New BaseImplementationTypes.BaseAddressType

                oInsurerAddress.AddressLine1 = MaintainClaimRequest.Claim.Insurer.Address.AddressLine1
                oInsurerAddress.AddressLine2 = MaintainClaimRequest.Claim.Insurer.Address.AddressLine2
                oInsurerAddress.AddressLine3 = MaintainClaimRequest.Claim.Insurer.Address.AddressLine3
                oInsurerAddress.AddressLine4 = MaintainClaimRequest.Claim.Insurer.Address.AddressLine4

                oInsurerAddress.AddressTypeCode = CType([Enum].ToObject(GetType(AddressTypeType), MaintainClaimRequest.Claim.Insurer.Address.AddressTypeCode), BaseImplementationTypes.AddressTypeType)
                'oInsurerAddress.AddressTypeCode = MaintainClaimRequest.Claim.Insurer.Address.AddressTypeCode
                oInsurerAddress.CountryCode = MaintainClaimRequest.Claim.Insurer.Address.CountryCode
                oInsurerAddress.PostCode = MaintainClaimRequest.Claim.Insurer.Address.PostCode

                oImpInsurer.Address = oInsurerAddress

                ' if client contacts have been provided in the request
                If MaintainClaimRequest.Claim.Insurer.Contact IsNot Nothing Then

                    ' Process the insurers contacts
                    oImpInsurer.Contact = Array.ConvertAll( _
                                                MaintainClaimRequest.Claim.Insurer.Contact, _
                                                New Converter(Of BaseContactType,  _
                                                    BaseImplementationTypes.BaseContactType) _
                                                    (AddressOf ToBaseImpBaseContactType))

                End If

                oImpInsurer.ContactName = MaintainClaimRequest.Claim.Insurer.ContactName
                oImpInsurer.PartyClaimNumber = MaintainClaimRequest.Claim.Insurer.PartyClaimNumber

                ' set the insured into the claim
                oImpClaim.Insurer = oImpInsurer

            End If

            ' if perils were specified in the request
            If MaintainClaimRequest.Claim.ClaimPeril IsNot Nothing Then

                ' process the claim peril array
                oImpClaim.ClaimPeril = Array.ConvertAll( _
                                                MaintainClaimRequest.Claim.ClaimPeril, _
                                                New Converter(Of BaseClaimPerilMaintainType,  _
                                                    BaseImplementationTypes.BaseClaimPerilMaintainType) _
                                                    (AddressOf ToBaseImpBaseClaimPerilMaintainType))

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

                If oImpResponse.Warnings IsNot Nothing Then
                    oResponse.Warnings = Array.ConvertAll( _
                                                    oImpResponse.Warnings, _
                                                    New Converter(Of BaseImplementationTypes.BaseClaimResponseTypeWarnings,  _
                                                        BaseClaimResponseTypeWarnings) _
                                                        (AddressOf ToServiceImpBaseClaimResponseTypeWarnings))

                End If

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function PayClaim(<XmlElement(elementName:="PayClaimRequest", Namespace:=ServiceNamespace)> ByVal PayClaimRequest As PayClaimRequestType) As <XmlElement(elementName:="payclaimResponse", Namespace:=ServiceNamespace)> PayClaimResponseType

        ' TODO : MEvans : Rework the calculation of tax items as currently it does not correctly handle advanced tax

        Try
            CheckAuthority("SAMPClm")

            Dim oResponse As New PayClaimResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.PayClaimRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.PayClaimResponseType = Nothing

            PayClaimIn(oImpRequest, PayClaimRequest)

            Try

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

                oResponse.Warnings = Array.ConvertAll( _
                                                  oImpResponse.Warnings, _
                                                  New Converter(Of BaseImplementationTypes.BaseClaimResponseTypeWarnings,  _
                                                      BaseClaimResponseTypeWarnings) _
                                                      (AddressOf ToServiceImpBaseClaimResponseTypeWarnings))

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    Private Sub PayClaimIn(ByVal oImpRequest As SAMForInsuranceImplementationTypes.PayClaimRequestType, ByVal oRequest As PayClaimRequestType)

        oImpRequest.TimeStamp = oRequest.TimeStamp ' CHECK IN
        oImpRequest.BranchCode = oRequest.BranchCode
        oImpRequest.ClaimPayment = New BaseImplementationTypes.BaseClaimPaymentType
        oImpRequest.ClaimPayment.BaseClaimKey = oRequest.ClaimPayment.BaseClaimKey
        oImpRequest.ClaimPayment.BaseClaimPerilKey = oRequest.ClaimPayment.BaseClaimPerilKey
        oImpRequest.ClaimPayment.ClaimVersionDescription = oRequest.ClaimPayment.ClaimVersionDescription
        oImpRequest.ClaimPayment.CurrencyCode = oRequest.ClaimPayment.CurrencyCode
        oImpRequest.ClaimPayment.PartyKey = oRequest.ClaimPayment.PartyKey
        oImpRequest.ClaimPayment.PaymentPartyType = CType([Enum].ToObject(GetType(ClaimPaymentPartyTypeType), oRequest.ClaimPayment.PaymentPartyType), BaseImplementationTypes.ClaimPaymentPartyTypeType)
        'oImpRequest.ClaimPayment.PaymentPartyType = oRequest.ClaimPayment.PaymentPartyType

        ' default the transaction date to todays date
        ' the only case this will not be true is via a data transfer
        ' where this process needs to support historical payments
        oImpRequest.ClaimPayment.TransactionDate = Date.Now()

        If oRequest.ClaimPayment.AdvancedTaxDetails IsNot Nothing Then

            oImpRequest.ClaimPayment.AdvancedTaxDetails = New BaseImplementationTypes.BaseClaimPaymentAdvancedTaxDetailsType
            oImpRequest.ClaimPayment.AdvancedTaxDetails.InsuredDomiciled = oRequest.ClaimPayment.AdvancedTaxDetails.InsuredDomiciled
            oImpRequest.ClaimPayment.AdvancedTaxDetails.InsuredPercentage = oRequest.ClaimPayment.AdvancedTaxDetails.InsuredPercentage
            oImpRequest.ClaimPayment.AdvancedTaxDetails.IsSettlement = oRequest.ClaimPayment.AdvancedTaxDetails.IsSettlement
            oImpRequest.ClaimPayment.AdvancedTaxDetails.IsTaxExempt = oRequest.ClaimPayment.AdvancedTaxDetails.IsTaxExempt
            oImpRequest.ClaimPayment.AdvancedTaxDetails.IsWHTExempt = oRequest.ClaimPayment.AdvancedTaxDetails.IsWHTExempt
            oImpRequest.ClaimPayment.AdvancedTaxDetails.InsuranceTaxNumber = oRequest.ClaimPayment.AdvancedTaxDetails.InsuranceTaxNumber
            oImpRequest.ClaimPayment.AdvancedTaxDetails.PayeeDomiciled = oRequest.ClaimPayment.AdvancedTaxDetails.PayeeDomiciled
            oImpRequest.ClaimPayment.AdvancedTaxDetails.PayeePercentage = oRequest.ClaimPayment.AdvancedTaxDetails.PayeePercentage
            oImpRequest.ClaimPayment.AdvancedTaxDetails.PayeeTaxNumber = oRequest.ClaimPayment.AdvancedTaxDetails.PayeeTaxNumber
            oImpRequest.ClaimPayment.AdvancedTaxDetails.SafeHarbourCode = oRequest.ClaimPayment.AdvancedTaxDetails.SafeHarbourCode
            oImpRequest.ClaimPayment.AdvancedTaxDetails.SafeHarbourPercentage = oRequest.ClaimPayment.AdvancedTaxDetails.SafeHarbourPercentage

        End If

        oImpRequest.ClaimPayment.Payee = New BaseImplementationTypes.BaseClaimPayeeType
        oImpRequest.ClaimPayment.Payee.BankCode = oRequest.ClaimPayment.Payee.BankCode
        oImpRequest.ClaimPayment.Payee.BankName = oRequest.ClaimPayment.Payee.BankName
        oImpRequest.ClaimPayment.Payee.BankNumber = oRequest.ClaimPayment.Payee.BankNumber
        oImpRequest.ClaimPayment.Payee.MediaReference = oRequest.ClaimPayment.Payee.MediaReference
        oImpRequest.ClaimPayment.Payee.MediaTypeCode = oRequest.ClaimPayment.Payee.MediaTypeCode
        oImpRequest.ClaimPayment.Payee.Name = oRequest.ClaimPayment.Payee.Name
        oImpRequest.ClaimPayment.Payee.TheirReference = oRequest.ClaimPayment.Payee.TheirReference
        oImpRequest.ClaimPayment.Payee.Comments = oRequest.ClaimPayment.Payee.Comments

        If oRequest.ClaimPayment.Payee.Address IsNot Nothing Then
            oImpRequest.ClaimPayment.Payee.Address = New BaseImplementationTypes.BaseAddressType
            oImpRequest.ClaimPayment.Payee.Address.AddressLine1 = oRequest.ClaimPayment.Payee.Address.AddressLine1
            oImpRequest.ClaimPayment.Payee.Address.AddressLine2 = oRequest.ClaimPayment.Payee.Address.AddressLine2
            oImpRequest.ClaimPayment.Payee.Address.AddressLine3 = oRequest.ClaimPayment.Payee.Address.AddressLine3
            oImpRequest.ClaimPayment.Payee.Address.AddressLine4 = oRequest.ClaimPayment.Payee.Address.AddressLine4
            oImpRequest.ClaimPayment.Payee.Address.AddressTypeCode = CType([Enum].ToObject(GetType(AddressTypeType), oRequest.ClaimPayment.Payee.Address.AddressTypeCode), BaseImplementationTypes.AddressTypeType)
            oImpRequest.ClaimPayment.Payee.Address.PostCode = oRequest.ClaimPayment.Payee.Address.PostCode
            oImpRequest.ClaimPayment.Payee.Address.CountryCode = oRequest.ClaimPayment.Payee.Address.CountryCode
        End If

        ReDim oImpRequest.ClaimPayment.ClaimPaymentItem(oRequest.ClaimPayment.ClaimPaymentItem.GetUpperBound(0))
        For cntItems As Integer = oRequest.ClaimPayment.ClaimPaymentItem.GetLowerBound(0) To oRequest.ClaimPayment.ClaimPaymentItem.GetUpperBound(0)
            oImpRequest.ClaimPayment.ClaimPaymentItem(cntItems) = New BaseImplementationTypes.BaseClaimPaymentItemType
            oImpRequest.ClaimPayment.ClaimPaymentItem(cntItems).BaseReserveKey = oRequest.ClaimPayment.ClaimPaymentItem(cntItems).BaseReserveKey
            oImpRequest.ClaimPayment.ClaimPaymentItem(cntItems).PaymentAmount = oRequest.ClaimPayment.ClaimPaymentItem(cntItems).PaymentAmount
            oImpRequest.ClaimPayment.ClaimPaymentItem(cntItems).TaxGroupCode = oRequest.ClaimPayment.ClaimPaymentItem(cntItems).TaxGroupCode
        Next

    End Sub

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetClaimPaymentTaxes(<XmlElement(elementName:="GetClaimPaymentTaxesRequest", Namespace:=ServiceNamespace)> ByVal GetClaimPaymentTaxesRequest As GetClaimPaymentTaxesRequestType) As <XmlElement(elementName:="GetClaimPaymentTaxesResponse", Namespace:=ServiceNamespace)> GetClaimPaymentTaxesResponseType

        ' TODO : MEvans : Rework the calculation of tax items as currently it does not correctly handle advanced tax

        Try
            CheckAuthority("SAMGClmPT")

            Dim oResponse As New GetClaimPaymentTaxesResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetClaimPaymentTaxesRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetClaimPaymentTaxesResponseType = Nothing

            PayClaimIn(oImpRequest, GetClaimPaymentTaxesRequest)

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetClaimpaymentTaxes(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ReDim oResponse.Reserves(0)
                oResponse.Reserves(0) = New BaseClaimPerilReservePaymentType
                oResponse.Reserves(0).BaseReserveKey = oImpResponse.Reserves(0).BaseReserveKey
                'Retrieve values

                oResponse.PaymentItems = Array.ConvertAll( _
                                            oImpResponse.PaymentItems, _
                                            New Converter(Of BaseImplementationTypes.BaseGetClaimPaymentTaxesResponseTypePaymentItems,  _
                                                BaseGetClaimPaymentTaxesResponseTypePaymentItems) _
                                                (AddressOf ToServiceBaseGetClaimPaymentTaxesResponseTypePaymentType))

                oResponse.Reserves = Array.ConvertAll( _
                                            oImpResponse.Reserves, _
                                            New Converter(Of BaseImplementationTypes.BaseClaimPerilReservePaymentType,  _
                                                BaseClaimPerilReservePaymentType) _
                                                (AddressOf ToServiceBaseClaimPerilReservePaymentType))

                oResponse.TaxItems = Array.ConvertAll( _
                                            oImpResponse.TaxItems, _
                                            New Converter(Of BaseImplementationTypes.BaseClaimPaymentTaxItemType,  _
                                                BaseClaimPaymentTaxItemType) _
                                                (AddressOf ToServiceBaseClaimPaymentTaxItemType))

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetClaimRisk(<XmlElement(elementName:="GetClaimRiskRequest", Namespace:=ServiceNamespace)> ByVal GetClaimRiskRequest As GetClaimRiskRequestType) As <XmlElement(elementName:="GetClaimRiskResponse", Namespace:=ServiceNamespace)> GetClaimRiskResponseType

        Try
            CheckAuthority("SAMGClmRsk")

            Dim oResponse As New GetClaimRiskResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetClaimRiskRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetClaimRiskResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetClaimRiskRequest.BranchCode
            oImpRequest.BaseClaimKey = GetClaimRiskRequest.BaseClaimKey
            oImpRequest.Task = SiriusFS.SAM.Structure.SAMConstants.SAMComponentAction.PMEdit

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetClaimRisk(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                'oResponse.QuoteTimeStamp = oImpResponse.QuoteTimeStamp
                oResponse.TimeStamp = oImpResponse.TimeStamp
                oResponse.XMLDataSet = SAMFunc.TransformDatasetPBtoSAM(oImpResponse.XMLDataSet)

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetClaimRiskReadOnly(<XmlElement(elementName:="GetClaimRiskReadOnlyRequest", Namespace:=ServiceNamespace)> ByVal GetClaimRiskReadOnlyRequest As GetClaimRiskReadOnlyRequestType) As <XmlElement(elementName:="GetClaimRiskReadOnlyResponse", Namespace:=ServiceNamespace)> GetClaimRiskReadOnlyResponseType

        Try
            CheckAuthority("SAMGClmRsk")

            Dim oResponse As New GetClaimRiskReadOnlyResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetClaimRiskRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetClaimRiskResponseType

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetClaimRiskReadOnlyRequest.BranchCode
            oImpRequest.BaseClaimKey = GetClaimRiskReadOnlyRequest.BaseClaimKey
            oImpRequest.Task = SiriusFS.SAM.Structure.SAMConstants.SAMComponentAction.PMView

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetClaimRisk(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                'oResponse.QuoteTimeStamp = oImpResponse.QuoteTimeStamp
                oResponse.TimeStamp = oImpResponse.TimeStamp
                oResponse.XMLDataSet = SAMFunc.TransformDatasetPBtoSAM(oImpResponse.XMLDataSet)

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function UpdateClaimRisk(<XmlElement(elementName:="UpdateClaimRiskRequest", Namespace:=ServiceNamespace)> ByVal UpdateClaimRiskRequest As UpdateClaimRiskRequestType) As <XmlElement(elementName:="UpdateClaimRiskResponse", Namespace:=ServiceNamespace)> UpdateClaimRiskResponseType

        Try
            CheckAuthority("SAMUClmRsk")

            Dim oResponse As New UpdateClaimRiskResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.UpdateClaimRiskRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.UpdateClaimRiskResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = UpdateClaimRiskRequest.BranchCode
            oImpRequest.BaseClaimKey = UpdateClaimRiskRequest.BaseClaimKey
            oImpRequest.XMLDataSet = UpdateClaimRiskRequest.XMLDataSet
            oImpRequest.TimeStamp = UpdateClaimRiskRequest.TimeStamp

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.UpdateClaimRisk(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function RunValidationRules(<XmlElement(elementName:="RunValidationRulesRequest", Namespace:=ServiceNamespace)> ByVal RunValidationRulesRequest As RunValidationRulesRequestType) As <XmlElement(elementName:="RunValidationRulesResponse", Namespace:=ServiceNamespace)> RunValidationRulesResponseType

        Try
            CheckAuthority("SAMRDfRulE")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New RunValidationRulesResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.RunValidationRulesRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.RunValidationRulesResponseType = Nothing

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

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.RunValidationRules(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.XMLDataset = SAMFunc.TransformDatasetPBtoSAM(oImpResponse.XMLDataset)

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetClaimRiskLinks(<XmlElement(elementName:="GetClaimRiskLinksRequest", Namespace:=ServiceNamespace)> ByVal GetClaimRiskLinksRequest As GetClaimRiskLinksRequestType) As <XmlElement(elementName:="GetClaimRiskLinksResponse", Namespace:=ServiceNamespace)> GetClaimRiskLinksResponseType

        Try
            CheckAuthority("SAMGClmLFR")

            Dim oResponse As New GetClaimRiskLinksResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetClaimRiskLinksRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetClaimRiskLinksResponseType = Nothing

            oImpRequest.BranchCode = GetClaimRiskLinksRequest.BranchCode
            oImpRequest.InsuranceFileKey = GetClaimRiskLinksRequest.InsuranceFileKey
            oImpRequest.RiskKey = GetClaimRiskLinksRequest.RiskKey

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetClaimRiskLinks(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                oResponse.PerilType = Array.ConvertAll( _
                                                oImpResponse.PerilType, _
                                                New Converter(Of BaseImplementationTypes.BaseGetClaimRiskLinksResponseTypePerilType,  _
                                                    BaseGetClaimRiskLinksResponseTypePerilType) _
                                                    (AddressOf ToBaseGetClaimRiskLinksResponseTypePerilType))

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function AddAgentReceipt(<XmlElement(elementName:="AddAgentReceiptRequest", Namespace:=ServiceNamespace)> ByVal AddAgentReceiptRequest As AddAgentReceiptRequestType) As <XmlElement(elementName:="AddAgentReceiptResponse", Namespace:=ServiceNamespace)> AddAgentReceiptResponseType

        Try
            CheckAuthority("SAMADAGRPT")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New AddAgentReceiptResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.AddAgentReceiptRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.AddAgentReceiptResponseType = Nothing

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

            ' assign the implementation receipt to the request
            oImpRequest.Receipt = oImpReceipt

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.AddAgentReceipt(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function PostDocument(<XmlElement(elementName:="PostDocumentRequest", Namespace:=ServiceNamespace)> ByVal PostDocumentRequest As PostDocumentRequestType) As <XmlElement(elementName:="PostDocumentResponse", Namespace:=ServiceNamespace)> PostDocumentResponseType

        Try
            CheckAuthority("SAMPOSTDOC")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New PostDocumentResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.PostDocumentRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.PostDocumentResponseType = Nothing

            oImpRequest = ToBaseImpBasePostDocumentRequestType(PostDocumentRequest)

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.PostDocument(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.DocumentRef = oImpResponse.DocumentRef

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
       <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function FindInsuranceFileForClaims(<XmlElement(elementName:="FindInsuranceFileForClaimsRequest", Namespace:=ServiceNamespace)> ByVal FindInsuranceFileForCLaimsRequest As FindInsuranceFileForClaimsRequestType) As <XmlElement(elementName:="FindInsuranceFileForClaimsResponse", Namespace:=ServiceNamespace)> FindInsuranceFileForClaimsResponseType

        Try
            CheckAuthority("SAMFIFFCLM")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New FindInsuranceFileForClaimsResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.FindInsuranceFileForClaimsRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.FindInsuranceFileForClaimsResponseType = Nothing

            oImpRequest.BranchCode = FindInsuranceFileForCLaimsRequest.BranchCode
            oImpRequest.InsuranceFileRef = FindInsuranceFileForCLaimsRequest.InsuranceFileRef
            oImpRequest.SearchDate = FindInsuranceFileForCLaimsRequest.SearchDate
            oImpRequest.AgentKey = iAgentKey

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.FindInsuranceFileForClaims(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.InsuranceFileKey = oImpResponse.InsuranceFileKey

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function SaveRisk(<XmlElement(elementName:="SaveRiskRequest", Namespace:=ServiceNamespace)> ByVal SaveRiskRequest As SaveRiskRequestType) As <XmlElement(elementName:="SaveRiskResponse", Namespace:=ServiceNamespace)> SaveRiskResponseType

        Try
            CheckAuthority("SAMURsk")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New SaveRiskResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.SaveRiskRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.SaveRiskResponseType = Nothing

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
                oResponse.XMLDataSet = SAMFunc.TransformDatasetPBtoSAM(oImpResponse.XMLDataSet)

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetClaimPaymentTaxGroups(<XmlElement(elementName:="GetClaimPaymentTaxGroups", Namespace:=ServiceNamespace)> ByVal GetClaimPaymentTaxGroupsRequest As GetClaimPaymentTaxGroupsRequestType) As <XmlElement(elementName:="GetClaimPaymentTaxGroupsResponse", Namespace:=ServiceNamespace)> GetClaimPaymentTaxGroupsResponseType

        Try

            CheckAuthority("SAMGTCLPTG")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer = 0
            Dim iUserId As Integer = 0
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New GetClaimPaymentTaxGroupsResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures

            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetClaimPaymentTaxGroupsRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetClaimPaymentTaxGroupsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetClaimPaymentTaxGroupsRequest.BranchCode

            If GetClaimPaymentTaxGroupsRequest.AdvancedTax IsNot Nothing Then

                Dim oAdvanceTax As New BaseImplementationTypes.BaseGetClaimPaymentTaxGroupsRequestTypeAdvancedTax

                oAdvanceTax.PayeeDomiciled = GetClaimPaymentTaxGroupsRequest.AdvancedTax.PayeeDomiciled
                oAdvanceTax.PayeePercentage = GetClaimPaymentTaxGroupsRequest.AdvancedTax.PayeePercentage
                oAdvanceTax.PayeeTaxNumber = GetClaimPaymentTaxGroupsRequest.AdvancedTax.PayeeTaxNumber

                oImpRequest.AdvancedTax = oAdvanceTax

            End If

            oImpRequest.PartyKey = GetClaimPaymentTaxGroupsRequest.PartyKey
            oImpRequest.PaymentPartyType = CType([Enum].ToObject(GetType(ClaimPaymentPartyTypeType), GetClaimPaymentTaxGroupsRequest.PaymentPartyType), BaseImplementationTypes.ClaimPaymentPartyTypeType)
            'oImpRequest.PaymentPartyType = GetClaimPaymentTaxGroupsRequest.PaymentPartyType

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetClaimPaymentTaxGroups(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)


                ' Deserialize the XML from the implementation resultdataset into
                ' the correct messaging format
                Dim oResultDataSet As BaseGetClaimPaymentTaxGroupsResponseTypeTaxGroup = Nothing
                Dim oResultDataSetObject As Object = Nothing

                If oImpResponse.TaxGroup IsNot Nothing Then
                    Dim oXMLSerializer As New Serialization.XmlSerializer(GetType(BaseGetClaimPaymentTaxGroupsResponseTypeTaxGroup), m_sDefaultNameSpace)
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=oImpResponse.TaxGroup.OuterXml, oXMLSerializer:=oXMLSerializer, r_oResultDataSet:=oResultDataSetObject, sDefaultNameSpace:=m_sDefaultNameSpace)
                    oResultDataSet = DirectCast(oResultDataSetObject, BaseGetClaimPaymentTaxGroupsResponseTypeTaxGroup)
                End If

                ' Retrieve the values from the implementation response structure
                oResponse.TaxGroup = oResultDataSet

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetClaimReceiptTaxGroups(<XmlElement(elementName:="GetClaimReceiptTaxGroups", Namespace:=ServiceNamespace)> ByVal GetClaimReceiptTaxGroupsRequest As GetClaimReceiptTaxGroupsRequestType) As <XmlElement(elementName:="GetClaimReceiptTaxGroupsResponse", Namespace:=ServiceNamespace)> GetClaimReceiptTaxGroupsResponseType

        Try

            CheckAuthority("SAMGTCLRTG")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer = 0
            Dim iUserId As Integer = 0
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New GetClaimReceiptTaxGroupsResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures

            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetClaimReceiptTaxGroupsRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetClaimReceiptTaxGroupsResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetClaimReceiptTaxGroupsRequest.BranchCode
            oImpRequest.TypeCode = GetClaimReceiptTaxGroupsRequest.TypeCode

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetClaimReceiptTaxGroups(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Deserialize the XML from the implementation resultdataset into
                ' the correct messaging format
                Dim oResultDataSet As BaseGetClaimPaymentTaxGroupsResponseTypeTaxGroup = Nothing
                Dim oResultDataSetObject As Object = Nothing

                If oImpResponse.TaxGroup IsNot Nothing Then
                    Dim oXMLSerializer As New Serialization.XmlSerializer(GetType(BaseGetClaimPaymentTaxGroupsResponseTypeTaxGroup), m_sDefaultNameSpace)
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=oImpResponse.TaxGroup.OuterXml, oXMLSerializer:=oXMLSerializer, r_oResultDataSet:=oResultDataSetObject, sDefaultNameSpace:=m_sDefaultNameSpace)
                    oResultDataSet = DirectCast(oResultDataSetObject, BaseGetClaimPaymentTaxGroupsResponseTypeTaxGroup)
                End If

                ' Retrieve the values from the implementation response structure
                oResponse.TaxGroup = oResultDataSet

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function
    <WebMethod()> _
          <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetValidPrimaryCauses(<XmlElement(elementName:="GetValidPrimaryCausesRequest", Namespace:=ServiceNamespace)> ByVal GetValidPrimaryCausesRequest As GetValidPrimaryCausesRequestType) As <XmlElement(elementName:="GetValidPrimaryCausesResponse", Namespace:=ServiceNamespace)> GetValidPrimaryCausesResponseType

        Try
            '  CheckAuthority("SAMGCurByB")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New GetValidPrimaryCausesResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetValidPrimaryCausesRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetValidPrimaryCausesResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetValidPrimaryCausesRequest.BranchCode
            oImpRequest.InsuranceFileKey = GetValidPrimaryCausesRequest.InsuranceFileKey

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetValidPrimaryCauses(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Deserialize the XML from the implementation resultdataset into
                ' the correct messaging format

                Dim oResultDataSet As BaseGetValidPrimaryCausesResponseTypePrimaryCauses = Nothing
                Dim oResultDataSetObject As Object = Nothing

                If oImpResponse.PrimaryCauses IsNot Nothing Then
                    Dim oXMLSerializer As New Serialization.XmlSerializer(GetType(BaseGetValidPrimaryCausesResponseTypePrimaryCauses), m_sDefaultNameSpace)
                    SAMFunc.DeserializeImplementationDataSet(sXMLString:=oImpResponse.PrimaryCauses.OuterXml, oXMLSerializer:=oXMLSerializer, r_oResultDataSet:=oResultDataSetObject, sDefaultNameSpace:=m_sDefaultNameSpace)
                    oResultDataSet = DirectCast(oResultDataSetObject, BaseGetValidPrimaryCausesResponseTypePrimaryCauses)
                End If

                ' Retrieve the values from the implementation response structure
                oResponse.PrimaryCauses = oResultDataSet

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetDocumentList(<XmlElement(elementName:="GetDocumentListRequest", Namespace:=ServiceNamespace)> ByVal GetDocumentListRequest As GetDocumentListRequestType) As <XmlElement(elementName:="GetDocumentListResponse", Namespace:=ServiceNamespace)> GetDocumentListResponseType

        Try

            CheckAuthority("SAMGDocLst")

            Dim oResponse As GetDocumentListResponseType = New GetDocumentListResponseType
            Dim oBusiness As SAMForInsuranceBusiness = New SAMForInsuranceBusiness

            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetDocumentListRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetDocumentListResponseType = Nothing

            oImpRequest.InsuranceFolderKey = GetDocumentListRequest.InsuranceFolderKey

            Try
                oImpResponse = oBusiness.GetDocumentList(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                If IsArray(oImpResponse.Documents) Then

                    Dim iLower As Integer = oImpResponse.Documents.GetLowerBound(0)
                    Dim iUpper As Integer = oImpResponse.Documents.GetUpperBound(0)

                    ReDim oResponse.Documents(iUpper)

                    Dim impDocument As BaseImplementationTypes.BaseDocumentType

                    For iLoop As Integer = iLower To iUpper
                        impDocument = oImpResponse.Documents(iLoop)

                        oResponse.Documents(iLoop) = New BaseDocumentType
                        oResponse.Documents(iLoop).DocNum = impDocument.DocNum
                        oResponse.Documents(iLoop).DocDescription = impDocument.DocDescription
                    Next

                End If

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetDocument(<XmlElement(elementName:="GetDocumentRequest", Namespace:=ServiceNamespace)> ByVal GetDocumentRequest As GetDocumentRequestType) As <XmlElement(elementName:="GetDocumentResponse", Namespace:=ServiceNamespace)> GetDocumentResponseType
        Try
            CheckAuthority("SAMGDoc")

            Dim oResponse As New GetDocumentResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetDocumentRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetDocumentResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetDocumentRequest.BranchCode
            oImpRequest.DocNum = GetDocumentRequest.DocNum
            oImpRequest.Compress = GetDocumentRequest.Compress
            oImpRequest.ConvertPdf = GetDocumentRequest.ConvertPdf

            ' Call the implementation method
            Try
                oImpResponse = oBusiness.GetDocument(oImpRequest)

                ' Retrieve the values from the implementation response structure
                oResponse.PdfDocument = oImpResponse.PdfDocument

                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse
        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetOptionSetting(<XmlElement(elementName:="GetOptionSettingRequest", Namespace:=ServiceNamespace)> ByVal GetOptionSettingRequest As GetOptionSettingRequestType) As <XmlElement(elementName:="GetOptionSettingResponse", Namespace:=ServiceNamespace)> GetOptionSettingResponseType
        Try
            CheckAuthority("SAMGOS")

            Dim oResponse As New GetOptionSettingResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetOptionSettingRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetOptionSettingResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = GetOptionSettingRequest.BranchCode
            oImpRequest.OptionType = CType(GetOptionSettingRequest.OptionType, BaseImplementationTypes.OptionType)
            oImpRequest.OptionNumber = GetOptionSettingRequest.OptionNumber

            ' Call the implementation method
            Try
                oImpResponse = oBusiness.GetOptionSetting(oImpRequest)

                ' Retrieve the values from the implementation response structure
                oResponse.OptionValue = oImpResponse.OptionValue
                oResponse.UnderwritingType = oImpResponse.UnderwritingType
                oResponse.AccountType = oImpResponse.AccountType

                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse
        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
       <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetRenewalStatus(<XmlElement(elementName:="GetRenewalStatusRequest", Namespace:=ServiceNamespace)> ByVal GetRenewalStatusRequest As GetRenewalStatusRequestType) As <XmlElement(elementName:="GetRenewalStatusResponse", Namespace:=ServiceNamespace)> GetRenewalStatusResponseType
        Try
            CheckAuthority("SAMFIFFCLM")
            Dim userName As String = String.Empty
            Dim agentKey As Integer
            Dim userId As Integer
            GetIdentity(userName, agentKey, userId)
            Dim response As New GetRenewalStatusResponseType
            Dim business As New SAMForInsuranceBusiness
            ' Implementation structures
            Dim impRequest As New SAMForInsuranceImplementationTypes.GetRenewalStatusRequestType
            Dim impResponse As SAMForInsuranceImplementationTypes.GetRenewalStatusResponseType = Nothing
            impRequest.AgentKey = agentKey
            impRequest.UserName = userName
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
            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, response)
            End Try
            Return response
        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try
    End Function
    <WebMethod()> _
       <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function ReplacePartyContact(<XmlElement(elementName:="ReplacePartyContactRequest", Namespace:=ServiceNamespace)> ByVal ReplacePartyContactRequest As ReplacePartyContactRequestType) As <XmlElement(elementName:="ReplacePartyContactResponse", Namespace:=ServiceNamespace)> ReplacePartyContactResponseType
        Try
            CheckAuthority("SAMFIFFCLM")
            Dim userName As String = String.Empty
            Dim agentKey As Integer
            Dim userId As Integer
            GetIdentity(userName, agentKey, userId)
            Dim response As New ReplacePartyContactResponseType
            Dim business As New SAMForInsuranceBusiness
            ' Implementation structures
            Dim impRequest As New SAMForInsuranceImplementationTypes.ReplacePartyContactRequestType
            Dim impResponse As SAMForInsuranceImplementationTypes.ReplacePartyContactResponseType = Nothing
            impRequest.AgentKey = agentKey
            impRequest.UserName = userName
            impRequest.BranchCode = ReplacePartyContactRequest.BranchCode
            impRequest.UserId = userId
            impRequest.Contacts = Array.ConvertAll(ReplacePartyContactRequest.Contacts, _
                                        New Converter(Of BaseContactType,  _
                                        BaseImplementationTypes.BaseContactType) _
                                        (AddressOf ToBaseImpBaseContactType))
            impRequest.PartyKey = ReplacePartyContactRequest.PartyKey
            Try
                ' Call the implementation method
                impResponse = business.ReplacePartyContact(impRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(impResponse.STSError)
            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, response)
            End Try
            Return response
        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try
        Return Nothing
    End Function
    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetExistingInstalmentPlanPaymentDetails(<XmlElement(elementName:="GetExistingInstalmentPlanPaymentDetailsRequest", Namespace:=ServiceNamespace)> ByVal GetExistingInstalmentPlanPaymentDetailsRequest As GetExistingInstalmentPlanPaymentDetailsRequestType) As <XmlElement(elementName:="GetExistingInstalmentPlanPaymentDetailsResponse", Namespace:=ServiceNamespace)> GetExistingInstalmentPlanPaymentDetailsResponseType
        Try
            CheckAuthority("SAMFIFFCLM")
            Dim userName As String = String.Empty
            Dim agentKey As Integer
            Dim userId As Integer
            GetIdentity(userName, agentKey, userId)
            Dim response As New GetExistingInstalmentPlanPaymentDetailsResponseType
            Dim business As New SAMForInsuranceBusiness
            ' Implementation structures
            Dim impRequest As New SAMForInsuranceImplementationTypes.GetExistingInstalmentPlanPaymentDetailsRequestType
            Dim impResponse As SAMForInsuranceImplementationTypes.GetExistingInstalmentPlanPaymentDetailsResponseType = Nothing
            impRequest.AgentKey = agentKey
            impRequest.UserName = userName
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
                Handler.BusinessLayerBoundary(ex, response)
            End Try
            Return response
        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try
    End Function
    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GenerateInvite(<XmlElement(elementName:="GenerateInviteRequest", Namespace:=ServiceNamespace)> ByVal GenerateInviteRequest As GenerateInviteRequestType) As <XmlElement(elementName:="GenerateInviteResponse", Namespace:=ServiceNamespace)> GenerateInviteResponseType
        Try
            CheckAuthority("SAMGenDoc")
            Dim userName As String = String.Empty
            Dim agentKey As Integer
            Dim userId As Integer
            GetIdentity(userName, agentKey, userId)
            Dim response As New GenerateInviteResponseType
            Dim business As New SAMForInsuranceBusiness
            ' Implementation structures
            Dim impRequest As New SAMForInsuranceImplementationTypes.GenerateInviteRequestType
            Dim impResponse As SAMForInsuranceImplementationTypes.GenerateInviteResponseType = Nothing
            ' Pass the values to the implementation request structure
            impRequest.AgentKey = agentKey
            impRequest.BranchCode = GenerateInviteRequest.BranchCode
            impRequest.InsuranceFileKey = GenerateInviteRequest.InsuranceFileKey
            impRequest.OutputAsHTML = GenerateInviteRequest.OutputAsHTML
            impRequest.OutputAsPDF = GenerateInviteRequest.OutputAsPDF
            impRequest.QuoteTimeStamp = GenerateInviteRequest.QuoteTimeStamp
            impRequest.SpoolDocumentOnly = GenerateInviteRequest.SpoolDocumentOnly
            impRequest.SpoolDocumentOnlySpecified = GenerateInviteRequest.SpoolDocumentOnlySpecified
            impRequest.UserId = userId
            Try
                ' Call the implementation method
                impResponse = business.GenerateInvite(impRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(impResponse.STSError)
                ' Retrieve the values from the implementation response structure into Actual Response
                If Not impResponse Is Nothing Then
                    response.MergedFilePath = impResponse.MergedFilePath
                    response.QuoteTimeStamp = impResponse.QuoteTimeStamp
                    response.SpooledZipFile = impResponse.SpooledZipFile
                End If
            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, response)
            End Try
            Return response
        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try
    End Function
    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function LapseRenewal(<XmlElement(elementName:="LapseRenewalRequest", Namespace:=ServiceNamespace)> ByVal LapseRenewalRequest As LapseRenewalRequestType) As <XmlElement(elementName:="LapseRenewalResponse", Namespace:=ServiceNamespace)> LapseRenewalResponseType
        Try
            CheckAuthority("SAMFIFFCLM")
            Dim userName As String = String.Empty
            Dim agentKey As Integer
            Dim userId As Integer
            GetIdentity(userName, agentKey, userId)
            Dim response As New LapseRenewalResponseType
            Dim business As New SAMForInsuranceBusiness
            ' Implementation structures
            Dim impRequest As New SAMForInsuranceImplementationTypes.LapseRenewalRequestType
            Dim impResponse As SAMForInsuranceImplementationTypes.LapseRenewalResponseType = Nothing
            ' Pass the values to the implementation request structure
            impRequest.AgentKey = agentKey
            impRequest.BranchCode = LapseRenewalRequest.BranchCode
            impRequest.InsuranceFileKey = LapseRenewalRequest.InsuranceFileKey
            impRequest.QuoteTimeStamp = LapseRenewalRequest.QuoteTimeStamp
            impRequest.LapseReasonCode = LapseRenewalRequest.LapseReasonCode
            Try
                ' Call the implementation method
                impResponse = business.LapseRenewal(impRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(impResponse.STSError)
                If Not impResponse Is Nothing Then
                    response.QuoteTimeStamp = impResponse.QuoteTimeStamp
                End If
            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, response)
            End Try
            Return response
        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try
    End Function

    <WebMethod()> _
 <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare, OneWay:=True)> _
    Public Sub RunRenewalSelection(<XmlElement(elementName:="RunRenewalSelectionRequest", Namespace:=ServiceNamespace)> ByVal RunRenewalSelectionRequest As RunRenewalSelectionRequestType)

        Try
            CheckAuthority("SAMGHRKey")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.RunRenewalSelectionRequestType

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.UserName = sUserName
            oImpRequest.BranchCode = RunRenewalSelectionRequest.BranchCode
            oImpRequest.InsuranceFileKey = RunRenewalSelectionRequest.InsuranceFileKey
            oImpRequest.BatchRenewalJobKey = RunRenewalSelectionRequest.BatchRenewalJobKey
            oImpRequest.RecordsCount = RunRenewalSelectionRequest.RecordsCount
            oImpRequest.GUID = RunRenewalSelectionRequest.GUID

            ' Call the implementation method
            oBusiness.RunRenewalSelection(oImpRequest)

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
        End Try

    End Sub

    <WebMethod()> _
   <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare, OneWay:=True)> _
    Public Sub RunRenewalInvitation(<XmlElement(elementName:="RunRenewalInviteRequest", Namespace:=ServiceNamespace)> ByVal RunRenewalInviteRequest As RunRenewalInviteRequestType)

        Try
            CheckAuthority("SAMGHRKey")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.RunRenewalInviteRequestType

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.UserName = sUserName
            oImpRequest.BranchCode = RunRenewalInviteRequest.BranchCode
            oImpRequest.InsuranceFileKey = RunRenewalInviteRequest.InsuranceFileKey
            oImpRequest.BatchRenewalJobKey = RunRenewalInviteRequest.BatchRenewalJobKey
            oImpRequest.RecordsCount = RunRenewalInviteRequest.RecordsCount
            oImpRequest.GUID = RunRenewalInviteRequest.GUID

            ' Call the implementation method
            oBusiness.RunRenewalInvitation(oImpRequest)

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
        End Try

    End Sub

    <WebMethod()> _
      <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare, OneWay:=True)> _
    Public Sub RunRenewalAccept(<XmlElement(elementName:="RunRenewalAcceptRequest", Namespace:=ServiceNamespace)> ByVal RunRenewalAcceptRequest As RunRenewalAcceptRequestType)

        Try
            CheckAuthority("SAMGHRKey")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.RunRenewalAcceptRequestType

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.UserName = sUserName
            oImpRequest.BranchCode = RunRenewalAcceptRequest.BranchCode
            oImpRequest.InsuranceFileKey = RunRenewalAcceptRequest.InsuranceFileKey
            oImpRequest.BatchRenewalJobKey = RunRenewalAcceptRequest.BatchRenewalJobKey
            oImpRequest.RecordsCount = RunRenewalAcceptRequest.RecordsCount
            oImpRequest.GUID = RunRenewalAcceptRequest.GUID

            ' Call the implementation method
            oBusiness.RunRenewalAccept(oImpRequest)

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
        End Try

    End Sub

    ''' <summary>
    ''' This Web method is used to get the  Product Risk OptionValue by passing the Request parameters as request type objects
    ''' and also the response object ProductRiskOptionValue is being returned .
    '''</summary>
    '''<param name="oGetProductRiskOptionValueRequest" type="ProductRiskOptionValueRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.ProductRiskOptionValueResponseType</returns>  
    '''<remarks></remarks>

    <WebMethod()> _
            <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetProductRiskOptionValue(<XmlElement(elementName:=" GetProductRiskOptionValueRequest", Namespace:=ServiceNamespace)> ByVal oGetProductRiskOptionValueRequest As ProductRiskOptionValueRequestType) As <XmlElement(elementName:=" GetProductRiskOptionValueResponse", Namespace:=ServiceNamespace)> ProductRiskOptionValueResponseType

        Try
            'Assign appropriate key
            If oGetProductRiskOptionValueRequest.ActionType = ProductConfigActionType.ProductRiskMaintenance Then
                CheckAuthority("SAMGPROV")

            ElseIf oGetProductRiskOptionValueRequest.ActionType = ProductConfigActionType.RiskTypeMaintenance Then
                CheckAuthority("SAMGROV")
            End If

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New ProductRiskOptionValueResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.ProductRiskOptionValueRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.ProductRiskOptionValueResponseType = Nothing

            'Pass the values to the implementation request structure as below for all input parameters 
            oImpRequest.BranchCode = oGetProductRiskOptionValueRequest.BranchCode
            oImpRequest.ProducRiskOption = CType([Enum].ToObject(GetType(ProductRiskOptions), oGetProductRiskOptionValueRequest.ProducRiskOption), BaseImplementationTypes.ProductRiskOptions)
            oImpRequest.ProducRiskOptionSpecified = oGetProductRiskOptionValueRequest.ProducRiskOptionSpecified
            oImpRequest.ProductCode = oGetProductRiskOptionValueRequest.ProductCode
            oImpRequest.RiskTypeOption = CType([Enum].ToObject(GetType(RiskTypeOptions), oGetProductRiskOptionValueRequest.RiskTypeOption), BaseImplementationTypes.RiskTypeOptions)
            oImpRequest.RiskTypeOptionSpecified = oGetProductRiskOptionValueRequest.RiskTypeOptionSpecified
            oImpRequest.RiskTypeCode = oGetProductRiskOptionValueRequest.RiskTypeCode
            oImpRequest.ActionType = CType([Enum].ToObject(GetType(ProductConfigActionType), oGetProductRiskOptionValueRequest.ActionType), BaseImplementationTypes.ProductConfigActionType)

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetProductRiskOptionValue(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.ProductRiskOptionValue = oImpResponse.ProductRiskOptionValue

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' This Web method is used to Attach a cover note by passing the Request parameters as request type objects
    ''' and also the response object  is being returned .
    '''</summary>
    '''<param name="oAttachCoverNoteRequest" type="AttachCoverNoteRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsurance.AttachCoverNoteResponseType</returns>  
    '''<remarks></remarks>

    <WebMethod()> _
            <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function AttachCoverNote(<XmlElement(elementName:=" AttachCoverNoteRequest", Namespace:=ServiceNamespace)> ByVal oAttachCoverNoteRequest As AttachCoverNoteRequestType) As <XmlElement(elementName:=" AttachCoverNoteResponse", Namespace:=ServiceNamespace)> AttachCoverNoteResponseType

        Try
            'Assign appropriate key
            CheckAuthority("SAMARDCN")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New AttachCoverNoteResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.AttachCoverNoteRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.AttachCoverNoteResponseType = Nothing
            Dim oCoverNote As New BaseCoverNoteRiskItemType

            'Pass the values to the implementation request structure as below for all input parameters 
            oImpRequest.BranchCode = oAttachCoverNoteRequest.BranchCode
            oImpRequest.GenerateCoverNoteDocs = oAttachCoverNoteRequest.GenerateCoverNoteDocs
            oImpRequest.ProcessType = CType([Enum].ToObject(GetType(CoverNoteProcessType), oAttachCoverNoteRequest.ProcessType), BaseImplementationTypes.CoverNoteProcessType)
            oImpRequest.CoverNote = New BaseImplementationTypes.BaseCoverNoteRiskItemType
            oImpRequest.CoverNote.RiskKey = oAttachCoverNoteRequest.CoverNote.RiskKey
            oImpRequest.CoverNote.RiskDesc = oAttachCoverNoteRequest.CoverNote.RiskDesc
            oImpRequest.CoverNote.CoverNoteNumber = oAttachCoverNoteRequest.CoverNote.CoverNoteNumber
            oImpRequest.CoverNote.CoverNoteFrom = oAttachCoverNoteRequest.CoverNote.CoverNoteFrom
            oImpRequest.CoverNote.CoverNoteFromSpecified = oAttachCoverNoteRequest.CoverNote.CoverNoteFromSpecified
            oImpRequest.CoverNote.CoverNoteTo = oAttachCoverNoteRequest.CoverNote.CoverNoteTo
            oImpRequest.CoverNote.CoverNoteToSpecified = oAttachCoverNoteRequest.CoverNote.CoverNoteToSpecified
            oImpRequest.CoverNote.TImeStamp = oAttachCoverNoteRequest.CoverNote.TImeStamp

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.AttachCoverNote(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.CoverNoteRiskId = oImpResponse.CoverNoteRiskId
                oResponse.RiskKey = oImpResponse.RiskKey
                oResponse.TimeStamp = oImpResponse.TimeStamp

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' This Web method is used to Attach a cover note by passing the Request parameters as request type objects
    ''' and also the response object  is being returned .
    '''</summary>
    '''<param name="oGetNumberingSchemeNo" type="GetNumberingSchemeNoRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsurance.GetNumberingSchemeNoResponseType</returns>  
    '''<remarks></remarks>

    <WebMethod()> _
            <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function GetNumberingSchemeNo(<XmlElement(elementName:=" GetNumberingSchemeNoRequest", Namespace:=ServiceNamespace)> ByVal oGetNumberingSchemeNo As GetNumberingSchemeNoRequestType) As <XmlElement(elementName:=" GetNumberingSchemeNoResponse", Namespace:=ServiceNamespace)> GetNumberingSchemeNoResponseType

        Try
            'Assign appropriate key
            CheckAuthority("SAMGNCNO")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim oResponse As New GetNumberingSchemeNoResponseType
            Dim oBusiness As New SAMForInsuranceBusiness

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceImplementationTypes.GetNumberingSchemeNoRequestType
            Dim oImpResponse As SAMForInsuranceImplementationTypes.GetNumberingSchemeNoResponseType = Nothing
            Dim oCoverNote As New BaseCoverNoteRiskItemType

            'Pass the values to the implementation request structure as below for all input parameters 
            oImpRequest.BranchCode = oGetNumberingSchemeNo.BranchCode
            oImpRequest.ProductKey = oGetNumberingSchemeNo.ProductKey
            oImpRequest.AgentKey = oGetNumberingSchemeNo.AgentKey
            oImpRequest.SchemeType = CType([Enum].ToObject(GetType(NumberingSchemeType), oGetNumberingSchemeNo.SchemeType), BaseImplementationTypes.NumberingSchemeType)

            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetNumberingSchemeNo(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.FailureReason = oImpResponse.FailureReason
                oResponse.GeneratedCode = oImpResponse.GeneratedCode

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, oResponse)
            End Try

            Return oResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function

    <WebMethod()> _
        <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function PolicyDataImport(<XmlElement(elementName:="PolicyDataImportRequest", Namespace:=ServiceNamespace)> ByVal PolicyDataImportRequest As PolicyDataImportRequestType) As <XmlElement(elementName:="PolicyDataImportResponse", Namespace:=ServiceNamespace)> PolicyDataImportResponseType
        Try

            'Check user authority . . . . . . . . . . . . . . .
            CheckAuthority("SAMPolData")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer = 0
            Dim iUserId As Integer = 0
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim msgResponse As New PolicyDataImportResponseType

            ' Implementation structures
            Dim impRequest As New SAMForInsuranceImplementationTypes.PolicyDataImportRequestType
            Dim impResponse As SAMForInsuranceImplementationTypes.PolicyDataImportResponseType

            impRequest.BranchCode = PolicyDataImportRequest.BranchCode
            impRequest.PolicyVersion = ToBaseImpBaseQuoteRiskMsgType(PolicyDataImportRequest.PolicyVersion)

            ' Load the Request object for the next SAM layer . . . . . . . . 
            Dim oBusiness As New SAMForInsuranceBusiness

            Try

                ' Call the Next SAM layer . . . . . . . . . . .
                impResponse = oBusiness.PolicyDataImport(impRequest)

                SAMErrorCollection.CheckForErrorsFromSTS(impResponse.STSError)

                msgResponse = ToServiceBasePolicyDataImportResponseType(DirectCast(impResponse, BaseImplementationTypes.BasePolicyDataImportResponseType))

                ' Handle the response from the Last SAM layer . . . . . . . . . .

                'Load the response from this function . . . . . . . . . .

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, msgResponse)
            End Try

            Return msgResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing

        End Try

    End Function

    <WebMethod()> _
            <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function DocumentDataImport(<XmlElement(elementName:="DocumentDataImportRequest", Namespace:=ServiceNamespace)> ByVal DocumentDataImportRequest As DocumentDataImportRequestType) As <XmlElement(elementName:="DocumentDataImportResponse", Namespace:=ServiceNamespace)> DocumentDataImportResponseType
        Try

            'Check user authority . . . . . . . . . . . . . . .
            CheckAuthority("SAMDocData")

            Dim sUserName As String = String.Empty
            Dim iAgentKey As Integer = 0
            Dim iUserId As Integer = 0
            GetIdentity(sUserName, iAgentKey, iUserId)

            Dim msgResponse As New DocumentDataImportResponseType

            ' Implementation structures
            Dim impRequest As New SAMForInsuranceImplementationTypes.DocumentDataImportRequestType
            Dim impResponse As SAMForInsuranceImplementationTypes.PostDocumentResponseType

            impRequest.BranchCode = DocumentDataImportRequest.BranchCode
            impRequest.SAMStagingDocumentKey = DocumentDataImportRequest.SAMStagingDocumentKey
            impRequest.Document = ToBaseImpBasePostDocumentRequestType(DocumentDataImportRequest.Document)

            ' Load the Request object for the next SAM layer . . . . . . . . 
            Dim oBusiness As New SAMForInsuranceBusiness

            Try

                ' Call the Next SAM layer . . . . . . . . . . .
                impResponse = oBusiness.DocumentDataImport(impRequest)

                SAMErrorCollection.CheckForErrorsFromSTS(impResponse.STSError)

                msgResponse.HandlingInstanceID = impResponse.HandlingInstanceID

            Catch ex As Exception
                Handler.BusinessLayerBoundary(ex, msgResponse)
            End Try

            Return msgResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing

        End Try

    End Function

    Private Function ToServiceBasePolicyDataImportResponseType(ByVal oImpPolicyVersion As BaseImplementationTypes.BasePolicyDataImportResponseType) As PolicyDataImportResponseType

        Dim msgPolicyVersion As PolicyDataImportResponseType = New PolicyDataImportResponseType

        If oImpPolicyVersion IsNot Nothing Then

            msgPolicyVersion.InsuranceFileKey = oImpPolicyVersion.InsuranceFileKey
            msgPolicyVersion.InsuranceFolderKey = oImpPolicyVersion.InsuranceFolderKey
            msgPolicyVersion.SAMStagingPolicyKey = oImpPolicyVersion.SAMStagingPolicyKey

            ' Process the Risks structure
            If IsArray(oImpPolicyVersion.Risks) = True Then

                msgPolicyVersion.Risks = Array.ConvertAll(oImpPolicyVersion.Risks, _
                            New Converter(Of BaseImplementationTypes.BasePolicyDataImportResponseTypeRisks,  _
                            BasePolicyDataImportResponseTypeRisks) _
                            (AddressOf ToServiceBasePolicyDataImportResponseTypeRisks))

            End If
        End If

        Return msgPolicyVersion

    End Function

    Private Function ToServiceBasePolicyDataImportResponseTypeRisks(ByVal oImpRisk As BaseImplementationTypes.BasePolicyDataImportResponseTypeRisks) As BasePolicyDataImportResponseTypeRisks

        Dim msgRisk As BasePolicyDataImportResponseTypeRisks = New BasePolicyDataImportResponseTypeRisks

        If oImpRisk IsNot Nothing Then

            msgRisk.RiskFolderKey = oImpRisk.RiskFolderKey
            msgRisk.RiskKey = oImpRisk.RiskKey
            msgRisk.SAMStagingRiskKey = oImpRisk.SAMStagingRiskKey

        End If

        Return msgRisk

    End Function

    Private Function ToBaseImpBaseQuoteRiskMsgType(ByVal oServiceQuoteRiskMsgType As BaseQuoteRiskMsgType) As BaseImplementationTypes.BaseQuoteRiskMsgType

        Dim oImpQuoteRiskMsgType As BaseImplementationTypes.BaseQuoteRiskMsgType = New BaseImplementationTypes.BaseQuoteRiskMsgType

        If oServiceQuoteRiskMsgType IsNot Nothing Then

            oImpQuoteRiskMsgType.BranchCode = oServiceQuoteRiskMsgType.BranchCode
            oImpQuoteRiskMsgType.PartyKey = oServiceQuoteRiskMsgType.PartyKey
            oImpQuoteRiskMsgType.CoverStartDate = oServiceQuoteRiskMsgType.CoverStartDate
            oImpQuoteRiskMsgType.CoverEndDate = oServiceQuoteRiskMsgType.CoverEndDate
            oImpQuoteRiskMsgType.Description = oServiceQuoteRiskMsgType.Description
            oImpQuoteRiskMsgType.InsuredName = oServiceQuoteRiskMsgType.InsuredName
            oImpQuoteRiskMsgType.ProductCode = oServiceQuoteRiskMsgType.ProductCode
            oImpQuoteRiskMsgType.QuoteRef = oServiceQuoteRiskMsgType.QuoteRef
            oImpQuoteRiskMsgType.PolicyStatusCode = oServiceQuoteRiskMsgType.PolicyStatusCode
            oImpQuoteRiskMsgType.PolicyVersion = oServiceQuoteRiskMsgType.PolicyVersion
            oImpQuoteRiskMsgType.AgentKey = oServiceQuoteRiskMsgType.AgentKey
            oImpQuoteRiskMsgType.AgentKeySpecified = oServiceQuoteRiskMsgType.AgentKeySpecified
            oImpQuoteRiskMsgType.SAMStagingPolicyKey = oServiceQuoteRiskMsgType.SAMStagingPolicyKey
            oImpQuoteRiskMsgType.FinanceCompanyNo = oServiceQuoteRiskMsgType.FinanceCompanyNo
            oImpQuoteRiskMsgType.FinanceSchemeNo = oServiceQuoteRiskMsgType.FinanceSchemeNo
            oImpQuoteRiskMsgType.FinanceSchemeVersion = oServiceQuoteRiskMsgType.FinanceSchemeVersion
            oImpQuoteRiskMsgType.FinanceQuoteDate = oServiceQuoteRiskMsgType.FinanceQuoteDate
            oImpQuoteRiskMsgType.FinanceStartDate = oServiceQuoteRiskMsgType.FinanceStartDate
            oImpQuoteRiskMsgType.FinanceEndDate = oServiceQuoteRiskMsgType.FinanceEndDate
            oImpQuoteRiskMsgType.FinancePreferredDate = oServiceQuoteRiskMsgType.FinancePreferredDate
            oImpQuoteRiskMsgType.DayOfWeekOrMonth = oServiceQuoteRiskMsgType.DayOfWeekOrMonth
            oImpQuoteRiskMsgType.AmountToFinance = oServiceQuoteRiskMsgType.AmountToFinance
            oImpQuoteRiskMsgType.PaymentProtection = oServiceQuoteRiskMsgType.PaymentProtection
            oImpQuoteRiskMsgType.BankName = oServiceQuoteRiskMsgType.BankName
            oImpQuoteRiskMsgType.BankSortCode = oServiceQuoteRiskMsgType.BankSortCode
            oImpQuoteRiskMsgType.BankAccountNo = oServiceQuoteRiskMsgType.BankAccountNo
            oImpQuoteRiskMsgType.BankAccountName = oServiceQuoteRiskMsgType.BankAccountName
            oImpQuoteRiskMsgType.BankBranch = oServiceQuoteRiskMsgType.BankBranch
            oImpQuoteRiskMsgType.BankAddress = ToBaseImpBaseAddressType(oServiceQuoteRiskMsgType.BankAddress)
            oImpQuoteRiskMsgType.BankAreaCode = oServiceQuoteRiskMsgType.BankAreaCode
            oImpQuoteRiskMsgType.BankPhone = oServiceQuoteRiskMsgType.BankPhone
            oImpQuoteRiskMsgType.LapsedReasonCode = oServiceQuoteRiskMsgType.LapsedReasonCode
            oImpQuoteRiskMsgType.LapsedDate = oServiceQuoteRiskMsgType.LapsedDate
            oImpQuoteRiskMsgType.LapsedDateSpecified = oServiceQuoteRiskMsgType.LapsedDateSpecified
            oImpQuoteRiskMsgType.LapsedReasonDescription = oServiceQuoteRiskMsgType.LapsedReasonDescription
            oImpQuoteRiskMsgType.InceptionDate = oServiceQuoteRiskMsgType.InceptionDate
            oImpQuoteRiskMsgType.InceptionDateSpecified = oServiceQuoteRiskMsgType.InceptionDateSpecified
            oImpQuoteRiskMsgType.InceptionDateTPI = oServiceQuoteRiskMsgType.InceptionDateTPI
            oImpQuoteRiskMsgType.InceptionDateTPISpecified = oServiceQuoteRiskMsgType.InceptionDateTPISpecified
            oImpQuoteRiskMsgType.RenewalDate = oServiceQuoteRiskMsgType.RenewalDate
            oImpQuoteRiskMsgType.RenewalDateSpecified = oServiceQuoteRiskMsgType.RenewalDateSpecified
            oImpQuoteRiskMsgType.AlternateReference = oServiceQuoteRiskMsgType.AlternateReference
            oImpQuoteRiskMsgType.OldPolicyNumber = oServiceQuoteRiskMsgType.OldPolicyNumber
            oImpQuoteRiskMsgType.AccountExecutiveShortname = oServiceQuoteRiskMsgType.AccountExecutiveShortname
            oImpQuoteRiskMsgType.AnalysisCode = oServiceQuoteRiskMsgType.AnalysisCode
            oImpQuoteRiskMsgType.AccountHandlerShortname = oServiceQuoteRiskMsgType.AccountHandlerShortname
            oImpQuoteRiskMsgType.PolicyVersionTypeCode = oServiceQuoteRiskMsgType.PolicyVersionTypeCode

            ' Process the Risks structure
            If IsArray(oServiceQuoteRiskMsgType.Risks) = True Then

                oImpQuoteRiskMsgType.Risks = Array.ConvertAll(oServiceQuoteRiskMsgType.Risks, _
                            New Converter(Of BaseQuoteRiskMsgTypeRisks,  _
                            BaseImplementationTypes.BaseQuoteRiskMsgTypeRisks) _
                            (AddressOf ToBaseImpBaseRiskType))

            End If

            If oServiceQuoteRiskMsgType.Claim IsNot Nothing Then

                oImpQuoteRiskMsgType.Claim = Array.ConvertAll(oServiceQuoteRiskMsgType.Claim, _
                    New Converter(Of BaseQuoteRiskMsgTypeClaim,  _
                        BaseImplementationTypes.BaseQuoteRiskMsgTypeClaim) _
                        (AddressOf ToImplementationBaseQuoteRiskMsgTypeClaim))

            End If

        End If

        Return oImpQuoteRiskMsgType

    End Function

    Private Function ToBaseImpBaseClaimReceiptItemType(ByVal oService As BaseClaimReceiptItemType) As BaseImplementationTypes.BaseClaimReceiptItemType

        Dim oImplementation As BaseImplementationTypes.BaseClaimReceiptItemType = New BaseImplementationTypes.BaseClaimReceiptItemType

        If oService IsNot Nothing Then

            oImplementation.BaseRecoveryKey = oService.BaseRecoveryKey
            oImplementation.ReceiptAmount = oService.ReceiptAmount
            oImplementation.TaxGroupCode = oService.TaxGroupCode

            oImplementation.RecoveryPartyCode = oService.RecoveryPartyCode
            If oService.RecoveryPartyCode = "" Then
                oImplementation.RecoveryPartyCodeSpecified = False
            Else
                oImplementation.RecoveryPartyCodeSpecified = True
            End If

            If oService.RecoveryPartyTypeCode = "" Then
                oImplementation.RecoveryPartyTypeCodeSpecified = False
            Else
                oImplementation.RecoveryPartyTypeCodeSpecified = True
            End If

        End If

        Return oImplementation

    End Function

    Private Function ToImplementationBaseQuoteRiskMsgTypeClaim(ByVal Service As BaseQuoteRiskMsgTypeClaim) As BaseImplementationTypes.BaseQuoteRiskMsgTypeClaim

        Dim Implementation As New BaseImplementationTypes.BaseQuoteRiskMsgTypeClaim

        If Service IsNot Nothing Then

            Implementation.ClaimNumber = Service.ClaimNumber
            Implementation.ClaimVersionDescription = Service.ClaimVersionDescription
            Implementation.Comments = Service.Comments
            Implementation.CurrencyCode = Service.CurrencyCode
            Implementation.Description = Service.Description
            Implementation.HandlerCode = Service.HandlerCode
            Implementation.IgnoreWarnings = Service.IgnoreWarnings
            Implementation.InfoOnly = Service.InfoOnly
            Implementation.InsuranceFileKey = Service.InsuranceFileKey
            Implementation.LikelyClaim = Service.LikelyClaim
            Implementation.Location = Service.Location
            Implementation.LossFromDate = Service.LossFromDate
            Implementation.LossToDate = Service.LossToDate
            Implementation.LossToDateSpecified = Service.LossToDateSpecified
            Implementation.PrimaryCauseCode = Service.PrimaryCauseCode
            Implementation.ProgressStatusCode = Service.ProgressStatusCode
            Implementation.ReportedDate = Service.ReportedDate
            Implementation.RiskKey = Service.RiskKey
            Implementation.SamStagingClaimKey = Service.SAMStagingClaimKey
            Implementation.SecondaryCauseCode = Service.SecondaryCauseCode
            Implementation.TownCode = Service.TownCode
            Implementation.UnderwritingYearCode = Service.UnderwritingYearCode

            ' Not Implemented in this version of the data transfer 

            'Implementation.Client = Service.Client
            'Implementation.Insurer = Service.Insurer
            'Implementation.ClaimPeril = Service.ClaimPeril
            'Implementation.CatastropheCode = Service.CatastropheCode
            'Implementation.UserDefFldACode = Service.UserDefFldACode
            'Implementation.UserDefFldBCode = Service.UserDefFldBCode
            'Implementation.UserDefFldCCode = Service.UserDefFldCCode
            'Implementation.UserDefFldDCode = Service.UserDefFldDCode
            'Implementation.UserDefFldECode = Service.UserDefFldECode

        End If

        Return Implementation

    End Function

    Private Function ToBaseImpBaseRiskType(ByVal oServiceRiskType As BaseQuoteRiskMsgTypeRisks) As BaseImplementationTypes.BaseQuoteRiskMsgTypeRisks

        Dim oImpRisk As New BaseImplementationTypes.BaseQuoteRiskMsgTypeRisks

        If oServiceRiskType IsNot Nothing Then

            oImpRisk.DataModelCode = oServiceRiskType.DataModelCode
            oImpRisk.QuoteTimeStamp = oServiceRiskType.QuoteTimeStamp
            oImpRisk.RiskDescription = oServiceRiskType.RiskDescription
            oImpRisk.RiskTypeCode = oServiceRiskType.RiskTypeCode
            oImpRisk.RunDefaultRules = oServiceRiskType.RunDefaultRules
            oImpRisk.ScreenCode = oServiceRiskType.ScreenCode
            oImpRisk.XMLDataSet = oServiceRiskType.XMLDataSet
            oImpRisk.BranchCode = oServiceRiskType.BranchCode
            oImpRisk.SAMStagingRiskKey = oServiceRiskType.SAMStagingRiskKey

            ' Process the Risks Rating Sections structure
            If IsArray(oServiceRiskType.RatingSections) = True Then

                oImpRisk.RatingSections = Array.ConvertAll(oServiceRiskType.RatingSections, _
                            New Converter(Of BaseRiskRatingSectionType,  _
                            BaseImplementationTypes.BaseRiskRatingSectionType) _
                            (AddressOf ToBaseRiskRatingSectionType))

            End If

            ' Process the Risks Rating Sections structure
            If IsArray(oServiceRiskType.RIArrangement) = True Then

                oImpRisk.RIArrangement = Array.ConvertAll(oServiceRiskType.RIArrangement, _
                            New Converter(Of BaseRiskRIArrangementType,  _
                            BaseImplementationTypes.BaseRiskRIArrangementType) _
                            (AddressOf ToBaseRiskRIArrangementType))

            End If

        End If

        Return oImpRisk

    End Function

    Private Function ToBaseRiskRatingSectionType(ByVal oServiceRiskRatingSectionType As BaseRiskRatingSectionType) As BaseImplementationTypes.BaseRiskRatingSectionType

        Dim oImpRiskRatingSection As New BaseImplementationTypes.BaseRiskRatingSectionType

        If oServiceRiskRatingSectionType IsNot Nothing Then

            oImpRiskRatingSection.AnnualPremium = oServiceRiskRatingSectionType.AnnualPremium
            oImpRiskRatingSection.AnnualPremiumSpecified = oServiceRiskRatingSectionType.AnnualPremiumSpecified
            oImpRiskRatingSection.AnnualRate = oServiceRiskRatingSectionType.AnnualRate
            oImpRiskRatingSection.CountryCode = oServiceRiskRatingSectionType.CountryCode
            oImpRiskRatingSection.RateTypeCode = oServiceRiskRatingSectionType.RateTypeCode
            oImpRiskRatingSection.RatingSectionTypeCode = oServiceRiskRatingSectionType.RatingSectionTypeCode
            oImpRiskRatingSection.SequenceNumber = oServiceRiskRatingSectionType.SequenceNumber
            oImpRiskRatingSection.StateCode = oServiceRiskRatingSectionType.StateCode
            oImpRiskRatingSection.SumInsured = oServiceRiskRatingSectionType.SumInsured
            oImpRiskRatingSection.SumInsuredSpecified = oServiceRiskRatingSectionType.SumInsuredSpecified
            oImpRiskRatingSection.ThisPremium = oServiceRiskRatingSectionType.ThisPremium
            oImpRiskRatingSection.ThisPremiumSpecified = oServiceRiskRatingSectionType.ThisPremiumSpecified
            oImpRiskRatingSection.OriginalFlag = oServiceRiskRatingSectionType.OriginalFlag

        End If

        Return oImpRiskRatingSection

    End Function

    Private Function ToBaseRiskRIArrangementType(ByVal oServiceRiskRIArrangementType As BaseRiskRIArrangementType) As BaseImplementationTypes.BaseRiskRIArrangementType

        Dim oImpRiskRIArrangement As New BaseImplementationTypes.BaseRiskRIArrangementType

        If oServiceRiskRIArrangementType IsNot Nothing Then

            oImpRiskRIArrangement.Premium = oServiceRiskRIArrangementType.Premium
            oImpRiskRIArrangement.RIBandCode = oServiceRiskRIArrangementType.RIBandCode
            oImpRiskRIArrangement.RIModelCode = oServiceRiskRIArrangementType.RIModelCode
            oImpRiskRIArrangement.SumInsured = oServiceRiskRIArrangementType.SumInsured
            oImpRiskRIArrangement.OriginalFlag = oServiceRiskRIArrangementType.OriginalFlag

            ' Process the Risks Rating Sections structure
            If IsArray(oServiceRiskRIArrangementType.RIArrangementLine) = True Then

                oImpRiskRIArrangement.RIArrangementLine = Array.ConvertAll(oServiceRiskRIArrangementType.RIArrangementLine, _
                            New Converter(Of BaseRiskRIArrangementLineType,  _
                            BaseImplementationTypes.BaseRiskRIArrangementLineType) _
                            (AddressOf ToBaseRiskRIArrangementLineType))

            End If

        End If

        Return oImpRiskRIArrangement

    End Function

    Private Function ToBaseRiskRIArrangementLineType(ByVal oServiceRiskRIArrangementLineType As BaseRiskRIArrangementLineType) As BaseImplementationTypes.BaseRiskRIArrangementLineType

        Dim oImpRiskRIArrangementLine As New BaseImplementationTypes.BaseRiskRIArrangementLineType

        If oServiceRiskRIArrangementLineType IsNot Nothing Then

            oImpRiskRIArrangementLine.AgreementCode = oServiceRiskRIArrangementLineType.AgreementCode
            oImpRiskRIArrangementLine.CommissionPercent = oServiceRiskRIArrangementLineType.CommissionPercent
            oImpRiskRIArrangementLine.CommissionTax = oServiceRiskRIArrangementLineType.CommissionTax
            oImpRiskRIArrangementLine.CommissionTaxSpecified = oServiceRiskRIArrangementLineType.CommissionTaxSpecified
            oImpRiskRIArrangementLine.CommissionValue = oServiceRiskRIArrangementLineType.CommissionValue
            oImpRiskRIArrangementLine.DefaultSharePercent = oServiceRiskRIArrangementLineType.DefaultSharePercent
            oImpRiskRIArrangementLine.Grouping = oServiceRiskRIArrangementLineType.Grouping
            oImpRiskRIArrangementLine.GroupingSpecified = oServiceRiskRIArrangementLineType.GroupingSpecified
            oImpRiskRIArrangementLine.LineLimit = oServiceRiskRIArrangementLineType.LineLimit
            oImpRiskRIArrangementLine.LowerLimit = oServiceRiskRIArrangementLineType.LowerLimit
            oImpRiskRIArrangementLine.LowerLimitSpecified = oServiceRiskRIArrangementLineType.LowerLimitSpecified
            oImpRiskRIArrangementLine.NumberOfLines = oServiceRiskRIArrangementLineType.NumberOfLines
            oImpRiskRIArrangementLine.ParticipationPercent = oServiceRiskRIArrangementLineType.ParticipationPercent
            oImpRiskRIArrangementLine.ParticipationPercentSpecified = oServiceRiskRIArrangementLineType.ParticipationPercentSpecified
            oImpRiskRIArrangementLine.PartyKey = oServiceRiskRIArrangementLineType.PartyKey
            oImpRiskRIArrangementLine.PartyKeySpecified = oServiceRiskRIArrangementLineType.PartyKeySpecified
            oImpRiskRIArrangementLine.PremiumPercent = oServiceRiskRIArrangementLineType.PremiumPercent
            oImpRiskRIArrangementLine.PremiumTax = oServiceRiskRIArrangementLineType.PremiumTax
            oImpRiskRIArrangementLine.PremiumTaxSpecified = oServiceRiskRIArrangementLineType.PremiumTaxSpecified
            oImpRiskRIArrangementLine.PremiumValue = oServiceRiskRIArrangementLineType.PremiumValue
            oImpRiskRIArrangementLine.Priority = oServiceRiskRIArrangementLineType.Priority
            oImpRiskRIArrangementLine.Retained = oServiceRiskRIArrangementLineType.Retained
            oImpRiskRIArrangementLine.RetainedSpecified = oServiceRiskRIArrangementLineType.RetainedSpecified
            oImpRiskRIArrangementLine.SumInsured = oServiceRiskRIArrangementLineType.SumInsured
            oImpRiskRIArrangementLine.ThisSharePercent = oServiceRiskRIArrangementLineType.ThisSharePercent
            oImpRiskRIArrangementLine.TreatyCode = oServiceRiskRIArrangementLineType.TreatyCode
            oImpRiskRIArrangementLine.Type = oServiceRiskRIArrangementLineType.Type

        End If

        Return oImpRiskRIArrangementLine

    End Function

    Private Function ToBaseImpBasePostDocumentRequestType(ByVal oServicePostDocumentRequestType As BasePostDocumentRequestType) As SAMForInsuranceImplementationTypes.PostDocumentRequestType
        Dim oImpPostDocumentRequestType As New SAMForInsuranceImplementationTypes.PostDocumentRequestType

        If oServicePostDocumentRequestType IsNot Nothing Then

            oImpPostDocumentRequestType.BranchCode = oServicePostDocumentRequestType.BranchCode
            oImpPostDocumentRequestType.DocumentType = CType([Enum].ToObject(GetType(DocumentTypeType), oServicePostDocumentRequestType.DocumentType), BaseImplementationTypes.DocumentTypeType)
            'oImpPostDocumentRequestType.DocumentType = oServicePostDocumentRequestType.DocumentType
            oImpPostDocumentRequestType.Comment = oServicePostDocumentRequestType.Comment
            oImpPostDocumentRequestType.DocumentReference = oServicePostDocumentRequestType.DocumentReference
            oImpPostDocumentRequestType.DocumentTypeCode = oServicePostDocumentRequestType.DocumentTypeCode
            oImpPostDocumentRequestType.SAMStagingPolicyKey = oServicePostDocumentRequestType.SAMStagingPolicyKey
            oImpPostDocumentRequestType.InsuranceFileKey = oServicePostDocumentRequestType.InsuranceFileKey
            oImpPostDocumentRequestType.InsuranceFileKeySpecified = oServicePostDocumentRequestType.InsuranceFileKeySpecified

            If oServicePostDocumentRequestType.Transactions IsNot Nothing Then

                oImpPostDocumentRequestType.Transactions = Array.ConvertAll( _
                                                            oServicePostDocumentRequestType.Transactions, _
                                                            New Converter(Of BaseTransactionType,  _
                                                                BaseImplementationTypes.BaseTransactionType) _
                                                                (AddressOf ToBaseImpBaseTransactionType))

            End If
        End If

        Return oImpPostDocumentRequestType

    End Function

    Private Function ToBaseImpBaseTransactionType(ByVal oServiceTransactionType As BaseTransactionType) As BaseImplementationTypes.BaseTransactionType
        Dim oImpTransactionType As BaseImplementationTypes.BaseTransactionType = New BaseImplementationTypes.BaseTransactionType

        If oServiceTransactionType IsNot Nothing Then

            oImpTransactionType.AccountCode = oServiceTransactionType.AccountCode
            oImpTransactionType.PartyKey = oServiceTransactionType.PartyKey
            oImpTransactionType.Amount = oServiceTransactionType.Amount
            oImpTransactionType.Comment = oServiceTransactionType.Comment
            oImpTransactionType.UnderwritingYearCode = oServiceTransactionType.UnderwritingYearCode
            oImpTransactionType.Reference = oServiceTransactionType.Reference
            oImpTransactionType.TransactionDate = oServiceTransactionType.TransactionDate
            oImpTransactionType.TransactionDateSpecified = oServiceTransactionType.TransactionDateSpecified
            oImpTransactionType.Username = oServiceTransactionType.Username

        End If

        Return oImpTransactionType

    End Function

    Private Function ToBaseImpBaseClaimPerilReserveType(ByVal oImpPerilTypeReserveType As BaseImplementationTypes.BaseGetClaimRiskLinksResponseTypePerilTypeReserveType) As BaseGetClaimRiskLinksResponseTypePerilTypeReserveType
        Dim oServicePerilTypeReserveType As New BaseGetClaimRiskLinksResponseTypePerilTypeReserveType

        If oImpPerilTypeReserveType IsNot Nothing Then

            oServicePerilTypeReserveType.Code = oImpPerilTypeReserveType.Code
            oServicePerilTypeReserveType.Description = oImpPerilTypeReserveType.Description

        End If

        Return oServicePerilTypeReserveType

    End Function

    Private Function ToBaseImpBaseClaimPerilRecoveryType(ByVal oImpPerilTypeRecoveryType As BaseImplementationTypes.BaseGetClaimRiskLinksResponseTypePerilTypeRecoveryType) As BaseGetClaimRiskLinksResponseTypePerilTypeRecoveryType

        Dim oServicePerilTypeRecoveryType As New BaseGetClaimRiskLinksResponseTypePerilTypeRecoveryType

        If oImpPerilTypeRecoveryType IsNot Nothing Then

            oServicePerilTypeRecoveryType.Code = oImpPerilTypeRecoveryType.Code
            oServicePerilTypeRecoveryType.Description = oImpPerilTypeRecoveryType.Description

        End If

        Return oServicePerilTypeRecoveryType

    End Function

    Private Function ToBaseGetClaimRiskLinksResponseTypePerilType(ByVal oImpPerilType As BaseImplementationTypes.BaseGetClaimRiskLinksResponseTypePerilType) As BaseGetClaimRiskLinksResponseTypePerilType

        Dim oServicePerilType As New BaseGetClaimRiskLinksResponseTypePerilType

        If oImpPerilType IsNot Nothing Then

            oServicePerilType.Code = oImpPerilType.Code
            oServicePerilType.Description = oImpPerilType.Description
            oServicePerilType.SumInsured = oImpPerilType.SumInsured

            If oImpPerilType.ReserveType IsNot Nothing Then
                oServicePerilType.ReserveType = Array.ConvertAll( _
                                                     oImpPerilType.ReserveType, _
                                                     New Converter(Of BaseImplementationTypes.BaseGetClaimRiskLinksResponseTypePerilTypeReserveType,  _
                                                         BaseGetClaimRiskLinksResponseTypePerilTypeReserveType) _
                                                         (AddressOf ToBaseImpBaseClaimPerilReserveType))
            End If

            If oImpPerilType.RecoveryType IsNot Nothing Then
                oServicePerilType.RecoveryType = Array.ConvertAll( _
                                                     oImpPerilType.RecoveryType, _
                                                     New Converter(Of BaseImplementationTypes.BaseGetClaimRiskLinksResponseTypePerilTypeRecoveryType,  _
                                                         BaseGetClaimRiskLinksResponseTypePerilTypeRecoveryType) _
                                                         (AddressOf ToBaseImpBaseClaimPerilRecoveryType))
            End If

        End If

        Return oServicePerilType

    End Function

    Private Function ToServiceImpBaseClaimResponseTypeWarnings(ByVal oWarnings As BaseImplementationTypes.BaseClaimResponseTypeWarnings) As BaseClaimResponseTypeWarnings

        Dim oServiceImpClaimPerilRecoveryType As BaseClaimResponseTypeWarnings = New BaseClaimResponseTypeWarnings

        If oWarnings IsNot Nothing Then

            oServiceImpClaimPerilRecoveryType.Code = oWarnings.Code
            oServiceImpClaimPerilRecoveryType.Description = oWarnings.Description

        End If

        Return oServiceImpClaimPerilRecoveryType

    End Function

    Private Overloads Function ToBaseImpBaseAddressType(ByVal msgAddress As BaseAddressType) As BaseImplementationTypes.BaseAddressType

        Dim impAddress As BaseImplementationTypes.BaseAddressType = New BaseImplementationTypes.BaseAddressType

        If msgAddress IsNot Nothing Then

            impAddress.AddressLine1 = msgAddress.AddressLine1
            impAddress.AddressLine2 = msgAddress.AddressLine2
            impAddress.AddressLine3 = msgAddress.AddressLine3
            impAddress.AddressLine4 = msgAddress.AddressLine4
            impAddress.AddressTypeCode = CType([Enum].ToObject(GetType(AddressTypeType), msgAddress.AddressTypeCode), BaseImplementationTypes.AddressTypeType)
            'impAddress.AddressTypeCode = msgAddress.AddressTypeCode
            impAddress.CountryCode = msgAddress.CountryCode
            impAddress.PostCode = msgAddress.PostCode

        End If

        Return impAddress

    End Function

    Private Overloads Function ToBaseImpBaseAddressType(ByVal msgAddress As BaseAddressWithContactsType) As BaseImplementationTypes.BaseAddressWithContactsType

        Dim impAddress As BaseImplementationTypes.BaseAddressWithContactsType = New BaseImplementationTypes.BaseAddressWithContactsType

        If msgAddress IsNot Nothing Then

            impAddress.AddressLine1 = msgAddress.AddressLine1
            impAddress.AddressLine2 = msgAddress.AddressLine2
            impAddress.AddressLine3 = msgAddress.AddressLine3
            impAddress.AddressLine4 = msgAddress.AddressLine4
            impAddress.AddressTypeCode = CType([Enum].ToObject(GetType(AddressTypeType), msgAddress.AddressTypeCode), BaseImplementationTypes.AddressTypeType)
            'impAddress.AddressTypeCode = msgAddress.AddressTypeCode
            impAddress.CountryCode = msgAddress.CountryCode
            impAddress.PostCode = msgAddress.PostCode

            If msgAddress.Contacts IsNot Nothing Then

                impAddress.Contacts = Array.ConvertAll( _
                                                    msgAddress.Contacts, _
                                                    New Converter(Of BaseContactType,  _
                                                     BaseImplementationTypes.BaseContactType) _
                                                    (AddressOf ToBaseImpBaseContactType))
            End If

        End If

        Return impAddress

    End Function

    Private Function ToServiceContactType(ByVal oContact As BaseImplementationTypes.BaseContactType) As BaseContactType

        Dim oServiceContact As BaseContactType = New BaseContactType

        If oContact IsNot Nothing Then

            oServiceContact.AreaCode = oContact.AreaCode
            oServiceContact.ContactDetail = New BaseContactDetailType
            oServiceContact.ContactDetail.Item = oContact.ContactDetail.Item
            oServiceContact.ContactDetail.ItemElementName = CType([Enum].ToObject(GetType(BaseImplementationTypes.ItemChoiceType), oContact.ContactDetail.ItemElementName), ItemChoiceType)
            '            oServiceContact.ContactDetail.ItemElementName = oContact.ContactDetail.ItemElementName
            oServiceContact.ContactTypeCode = CType([Enum].ToObject(GetType(BaseImplementationTypes.ContactTypeType), oContact.ContactTypeCode), ContactTypeType)
            '            oServiceContact.ContactTypeCode = oContact.ContactTypeCode

        End If

        Return oServiceContact

    End Function

    Private Function ToBaseImpBaseContactType(ByVal msgContact As BaseContactType) As BaseImplementationTypes.BaseContactType
        Dim impContact As BaseImplementationTypes.BaseContactType = New BaseImplementationTypes.BaseContactType

        If msgContact IsNot Nothing Then

            impContact.AreaCode = msgContact.AreaCode
            impContact.ContactDetail = New BaseImplementationTypes.BaseContactDetailType
            impContact.ContactDetail.Item = msgContact.ContactDetail.Item
            impContact.ContactTypeCode = CType([Enum].ToObject(GetType(ContactTypeType), msgContact.ContactTypeCode), BaseImplementationTypes.ContactTypeType)
            'impContact.ContactTypeCode = msgContact.ContactTypeCode

        End If
        Return impContact
    End Function

    Private Function ToBaseImpBaseClaimPerilRecoveryType(ByVal msgClaimPerilRecoveryType As BaseClaimPerilRecoveryType) As BaseImplementationTypes.BaseClaimPerilRecoveryType
        Dim impClaimPerilRecoveryType As BaseImplementationTypes.BaseClaimPerilRecoveryType = New BaseImplementationTypes.BaseClaimPerilRecoveryType

        If msgClaimPerilRecoveryType IsNot Nothing Then

            impClaimPerilRecoveryType.RevisionAmount = msgClaimPerilRecoveryType.RevisionAmount
            impClaimPerilRecoveryType.TypeCode = msgClaimPerilRecoveryType.TypeCode

            impClaimPerilRecoveryType.RecoveryPartyTypeKeySpecified = msgClaimPerilRecoveryType.RecoveryPartyTypeKeySpecified
            impClaimPerilRecoveryType.RecoveryPartyKeySpecified = msgClaimPerilRecoveryType.RecoveryPartyKeySpecified
            impClaimPerilRecoveryType.RecoveryPartyKey = msgClaimPerilRecoveryType.RecoveryPartyKey
            impClaimPerilRecoveryType.RecoveryPartyTypeKey = msgClaimPerilRecoveryType.RecoveryPartyTypeKey

        End If
        Return impClaimPerilRecoveryType
    End Function

    Private Function ToBaseImpBaseClaimPerilReserveType(ByVal msgClaimPerilReserveType As BaseClaimPerilReserveType) As BaseImplementationTypes.BaseClaimPerilReserveType
        Dim impClaimPerilReserveType As BaseImplementationTypes.BaseClaimPerilReserveType = New BaseImplementationTypes.BaseClaimPerilReserveType

        If msgClaimPerilReserveType IsNot Nothing Then

            impClaimPerilReserveType.RevisionAmount = msgClaimPerilReserveType.RevisionAmount
            impClaimPerilReserveType.TypeCode = msgClaimPerilReserveType.TypeCode

        End If

        Return impClaimPerilReserveType

    End Function

    Private Function ToBaseImpBaseClaimPerilType(ByVal msgClaimPerilType As BaseClaimPerilType) As BaseImplementationTypes.BaseClaimPerilType

        Dim impClaimPerilType As BaseImplementationTypes.BaseClaimPerilType = New BaseImplementationTypes.BaseClaimPerilType

        ' if there is a claim peril in the request
        If msgClaimPerilType IsNot Nothing Then

            impClaimPerilType.Description = msgClaimPerilType.Description
            impClaimPerilType.TypeCode = msgClaimPerilType.TypeCode

            ' if reserves were specfied in the request
            If msgClaimPerilType.Reserve IsNot Nothing Then
                impClaimPerilType.Reserve = Array.ConvertAll( _
                                                     msgClaimPerilType.Reserve, _
                                                     New Converter(Of BaseClaimPerilReserveType,  _
                                                         BaseImplementationTypes.BaseClaimPerilReserveType) _
                                                         (AddressOf ToBaseImpBaseClaimPerilReserveType))
            End If

            ' if recoveries were specified in the request
            If msgClaimPerilType.Recovery IsNot Nothing Then
                impClaimPerilType.Recovery = Array.ConvertAll( _
                                                     msgClaimPerilType.Recovery, _
                                                     New Converter(Of BaseClaimPerilRecoveryType,  _
                                                         BaseImplementationTypes.BaseClaimPerilRecoveryType) _
                                                         (AddressOf ToBaseImpBaseClaimPerilRecoveryType))

            End If

        End If

        Return impClaimPerilType

    End Function

    Private Function ToBaseImpBaseClaimPerilMaintainType(ByVal msgClaimPerilType As BaseClaimPerilMaintainType) As BaseImplementationTypes.BaseClaimPerilMaintainType

        Dim impClaimPerilType As BaseImplementationTypes.BaseClaimPerilMaintainType = New BaseImplementationTypes.BaseClaimPerilMaintainType

        If msgClaimPerilType IsNot Nothing Then

            impClaimPerilType.Description = msgClaimPerilType.Description
            impClaimPerilType.TypeCode = msgClaimPerilType.TypeCode
            impClaimPerilType.BaseClaimPerilKeySpecified = msgClaimPerilType.BaseClaimPerilKeySpecified
            impClaimPerilType.BaseClaimPerilKey = msgClaimPerilType.BaseClaimPerilKey

            If msgClaimPerilType.Reserve IsNot Nothing Then
                impClaimPerilType.Reserve = Array.ConvertAll( _
                                                     msgClaimPerilType.Reserve, _
                                                     New Converter(Of BaseClaimPerilReserveType,  _
                                                         BaseImplementationTypes.BaseClaimPerilReserveType) _
                                                         (AddressOf ToBaseImpBaseClaimPerilReserveType))
            End If

            If msgClaimPerilType.Recovery IsNot Nothing Then
                impClaimPerilType.Recovery = Array.ConvertAll( _
                                                     msgClaimPerilType.Recovery, _
                                                     New Converter(Of BaseClaimPerilRecoveryType,  _
                                                         BaseImplementationTypes.BaseClaimPerilRecoveryType) _
                                                         (AddressOf ToBaseImpBaseClaimPerilRecoveryType))

            End If

        End If

        Return impClaimPerilType
    End Function

    Private Function ToServiceAccident(ByVal oAccident As BaseImplementationTypes.BasePartyOTHERTypeAccident) As BasePartyOTHERTypeAccident

        Dim oServiceAccident As BasePartyOTHERTypeAccident = New BasePartyOTHERTypeAccident

        If oAccident IsNot Nothing Then

            oServiceAccident.AccidentKey = oAccident.AccidentKey
            oServiceAccident.Date = oAccident.Date
            oServiceAccident.Description = oAccident.Description
            oServiceAccident.IsAtFault = oAccident.IsAtFault

        End If

        Return oServiceAccident

    End Function

    Private Function ToServiceSupplierBusiness(ByVal oSupplierBusiness As BaseImplementationTypes.BasePartyOTHERTypeSupplierBusiness) As BasePartyOTHERTypeSupplierBusiness

        Dim oServiceSupplierBusiness As BasePartyOTHERTypeSupplierBusiness = New BasePartyOTHERTypeSupplierBusiness

        If oSupplierBusiness IsNot Nothing Then

            oServiceSupplierBusiness.BusinessCode = oSupplierBusiness.BusinessCode
            oServiceSupplierBusiness.SpecialityCode = oSupplierBusiness.SpecialityCode

        End If

        Return oServiceSupplierBusiness

    End Function

    Private Function ToServiceConviction(ByVal oConviction As BaseImplementationTypes.BasePartyOTHERTypeConviction) As BasePartyOTHERTypeConviction

        Dim oServiceConviction As BasePartyOTHERTypeConviction = New BasePartyOTHERTypeConviction

        If oConviction IsNot Nothing Then

            oServiceConviction.AlcoholLevel = oConviction.AlcoholLevel
            oServiceConviction.AlcoholLevelSpecified = oConviction.AlcoholLevelSpecified
            oServiceConviction.AlcoholMeasurementMethod = oConviction.AlcoholMeasurementMethod
            oServiceConviction.ConvictionKey = oConviction.ConvictionKey
            oServiceConviction.Date = oConviction.Date
            oServiceConviction.Description = oConviction.Description
            oServiceConviction.DrivingLicencePenaltyPoints = oConviction.DrivingLicencePenaltyPoints
            oServiceConviction.DrivingLicencePenaltyPointsSpecified = oConviction.DrivingLicencePenaltyPointsSpecified
            oServiceConviction.FineAmount = oConviction.FineAmount
            oServiceConviction.FineAmountSpecified = oConviction.FineAmountSpecified
            oServiceConviction.SentenceDescription = oConviction.SentenceDescription
            oServiceConviction.SentenceDuration = oConviction.SentenceDuration
            oServiceConviction.SentenceDurationQualifier = oConviction.SentenceDurationQualifier
            oServiceConviction.SentenceDurationSpecified = oConviction.SentenceDurationSpecified
            oServiceConviction.SentenceEffectiveDate = oConviction.SentenceEffectiveDate
            oServiceConviction.SentenceEffectiveDateSpecified = oConviction.SentenceEffectiveDateSpecified
            oServiceConviction.SentenceTypeCode = oConviction.SentenceTypeCode
            oServiceConviction.StatusCode = oConviction.StatusCode
            oServiceConviction.TypeCode = oConviction.TypeCode

        End If

        Return oServiceConviction

    End Function

    Private Function ToServiceSourceList(ByVal oSourceList As BaseImplementationTypes.BaseBranchType) As BaseBranchType

        Dim oServiceSourceList As BaseBranchType = New BaseBranchType

        If oSourceList IsNot Nothing Then

            oServiceSourceList.BranchCode = oSourceList.BranchCode
            oServiceSourceList.Description = oSourceList.Description

        End If

        Return oServiceSourceList

    End Function

    Private Function ToServiceBaseClaimPaymentTaxItemType( _
    ByVal oImplementation As BaseImplementationTypes.BaseClaimPaymentTaxItemType) As BaseClaimPaymentTaxItemType

        Dim oService As New BaseClaimPaymentTaxItemType

        If oImplementation IsNot Nothing Then

            oService.Amount = Decimal.Round(oImplementation.Amount, 2)
            oService.Percentage = oImplementation.Percentage
            oService.ReserveType = oImplementation.ReserveType
            oService.TaxBandCode = oImplementation.TaxBandCode
            oService.TaxGroupCode = oImplementation.TaxGroupCode

        End If

        Return oService

    End Function

    Private Function ToServiceBaseClaimPerilReservePaymentType( _
    ByVal oImplementation As BaseImplementationTypes.BaseClaimPerilReservePaymentType) As BaseClaimPerilReservePaymentType

        Dim oService As New BaseClaimPerilReservePaymentType

        If oImplementation IsNot Nothing Then

            oService.BaseReserveKey = oImplementation.BaseReserveKey
            oService.CostToClaim = Decimal.Round(oImplementation.CostToClaim, 2)
            oService.CurrentReserve = Decimal.Round(oImplementation.CurrentReserve, 2)
            oService.PaidToDate = Decimal.Round(oImplementation.PaidToDate, 2)
            oService.PaidToDateTax = Decimal.Round(oImplementation.PaidToDateTax, 2)
            oService.ThisPaymentINCLTax = Decimal.Round(oImplementation.ThisPaymentINCLTax, 2)
            oService.ThisPaymentTax = Decimal.Round(oImplementation.ThisPaymentTax, 2)
            oService.TotalReserve = Decimal.Round(oImplementation.TotalReserve, 2)
            oService.TypeCode = oImplementation.TypeCode

        End If

        Return oService

    End Function

    Private Function ToServiceBaseGetClaimPaymentTaxesResponseTypePaymentType( _
    ByVal oImplementation As BaseImplementationTypes.BaseGetClaimPaymentTaxesResponseTypePaymentItems) As BaseGetClaimPaymentTaxesResponseTypePaymentItems

        Dim oService As New BaseGetClaimPaymentTaxesResponseTypePaymentItems

        If oImplementation IsNot Nothing Then

            oService.BaseReserveKey = oImplementation.BaseReserveKey
            oService.PaymentAdjustment = Decimal.Round(oImplementation.PaymentAdjustment, 2)
            oService.PaymentAmount = Decimal.Round(oImplementation.PaymentAmount, 2)
            oService.ReverseExcess = oImplementation.ReverseExcess
            oService.TaxAmount = Decimal.Round(oImplementation.TaxAmount, 2)
            oService.TaxGroupCode = oImplementation.TaxGroupCode

        End If

        Return oService

    End Function

    Private Function ToServiceBaseAddressWithContactsType( _
    ByVal oImplementation As BaseImplementationTypes.BaseAddressType) As BaseAddressWithContactsType

        Dim oService As New BaseAddressWithContactsType

        If oImplementation IsNot Nothing Then

            Dim oActualImplementation As BaseImplementationTypes.BaseAddressWithContactsType

            oActualImplementation = TryCast(oImplementation, BaseImplementationTypes.BaseAddressWithContactsType)

            If oActualImplementation IsNot Nothing Then
                oService.AddressLine1 = oActualImplementation.AddressLine1
                oService.AddressLine2 = oActualImplementation.AddressLine2
                oService.AddressLine3 = oActualImplementation.AddressLine3
                oService.AddressLine4 = oActualImplementation.AddressLine4
                oService.AddressTypeCode = CType([Enum].ToObject(GetType(BaseImplementationTypes.AddressTypeType), oActualImplementation.AddressTypeCode), AddressTypeType)
                'oService.AddressTypeCode = oActualImplementation.AddressTypeCode
                oService.CountryCode = oActualImplementation.CountryCode
                oService.PostCode = oActualImplementation.PostCode

                If oActualImplementation.Contacts IsNot Nothing Then

                    oService.Contacts = Array.ConvertAll(oActualImplementation.Contacts, _
                                New Converter(Of BaseImplementationTypes.BaseContactType,  _
                                BaseContactType) _
                                (AddressOf ToServiceContactType))

                End If

            Else
                oService.AddressLine1 = oImplementation.AddressLine1
                oService.AddressLine2 = oImplementation.AddressLine2
                oService.AddressLine3 = oImplementation.AddressLine3
                oService.AddressLine4 = oImplementation.AddressLine4
                oService.AddressTypeCode = CType([Enum].ToObject(GetType(BaseImplementationTypes.AddressTypeType), oImplementation.AddressTypeCode), AddressTypeType)
                'oService.AddressTypeCode = oImplementation.AddressTypeCode
                oService.CountryCode = oImplementation.CountryCode
                oService.PostCode = oImplementation.PostCode
                oService.Contacts = Nothing
            End If
        End If

        Return oService

    End Function

    Private Function ToBaseImpBaseAddressWithContactsType( _
            ByVal oService As BaseAddressWithContactsType) As BaseImplementationTypes.BaseAddressWithContactsType

        Dim oImplementation As New BaseImplementationTypes.BaseAddressWithContactsType

        If oService IsNot Nothing Then

            oImplementation.AddressLine1 = oService.AddressLine1
            oImplementation.AddressLine2 = oService.AddressLine2
            oImplementation.AddressLine3 = oService.AddressLine3
            oImplementation.AddressLine4 = oService.AddressLine4
            oImplementation.AddressTypeCode = CType([Enum].ToObject(GetType(AddressTypeType), oService.AddressTypeCode), BaseImplementationTypes.AddressTypeType)
            'oImplementation.AddressTypeCode = oService.AddressTypeCode
            oImplementation.CountryCode = oService.CountryCode
            oImplementation.PostCode = oService.PostCode

            If oService.Contacts IsNot Nothing Then

                oImplementation.Contacts = Array.ConvertAll(oService.Contacts, _
                            New Converter(Of BaseContactType, BaseImplementationTypes.BaseContactType) _
                            (AddressOf ToBaseImpBaseContactType))

            End If

        End If

        Return oImplementation

    End Function

    Private Function ToBaseImpBasePartyOTHERTypeConviction( _
            ByVal oService As BasePartyOTHERTypeConviction) As BaseImplementationTypes.BasePartyOTHERTypeConviction

        Dim oImplementation As New BaseImplementationTypes.BasePartyOTHERTypeConviction

        If oService IsNot Nothing Then

            oImplementation.AlcoholLevel = oService.AlcoholLevel
            oImplementation.AlcoholLevelSpecified = oService.AlcoholLevelSpecified
            oImplementation.AlcoholMeasurementMethod = oService.AlcoholMeasurementMethod
            oImplementation.ConvictionKey = oService.ConvictionKey
            oImplementation.Date = oService.Date
            oImplementation.Description = oService.Description
            oImplementation.DrivingLicencePenaltyPoints = oService.DrivingLicencePenaltyPoints
            oImplementation.DrivingLicencePenaltyPointsSpecified = oService.DrivingLicencePenaltyPointsSpecified
            oImplementation.FineAmount = oService.FineAmount
            oImplementation.FineAmountSpecified = oService.FineAmountSpecified
            oImplementation.SentenceDescription = oService.SentenceDescription
            oImplementation.SentenceDuration = oService.SentenceDuration
            oImplementation.SentenceDurationQualifier = oService.SentenceDurationQualifier
            oImplementation.SentenceDurationSpecified = oService.SentenceDurationSpecified
            oImplementation.SentenceEffectiveDate = oService.SentenceEffectiveDate
            oImplementation.SentenceEffectiveDateSpecified = oService.SentenceEffectiveDateSpecified
            oImplementation.SentenceTypeCode = oService.SentenceTypeCode
            oImplementation.StatusCode = oService.StatusCode
            oImplementation.TypeCode = oService.TypeCode

        End If

        Return oImplementation

    End Function

    Private Function ToBaseImpBasePartyOTHERTypeAccident( _
            ByVal oService As BasePartyOTHERTypeAccident) As BaseImplementationTypes.BasePartyOTHERTypeAccident

        Dim oImplementation As New BaseImplementationTypes.BasePartyOTHERTypeAccident

        If oService IsNot Nothing Then

            oImplementation.AccidentKey = oService.AccidentKey
            oImplementation.Date = oService.Date
            oImplementation.Description = oService.Description
            oImplementation.IsAtFault = oService.IsAtFault

        End If

        Return oImplementation

    End Function

    Private Function ToBaseImpBasePartyOTHERTypeSupplierBusiness( _
            ByVal oService As BasePartyOTHERTypeSupplierBusiness) As BaseImplementationTypes.BasePartyOTHERTypeSupplierBusiness

        Dim oImplementation As New BaseImplementationTypes.BasePartyOTHERTypeSupplierBusiness

        If oService IsNot Nothing Then

            oImplementation.BusinessCode = oService.BusinessCode
            oImplementation.SpecialityCode = oService.SpecialityCode

        End If

        Return oImplementation

    End Function

    Private Sub PopulateOtherPartyReponse(ByVal oImpResponse As SAMForInsuranceImplementationTypes.GetPartyResponseType, _
                                          ByVal oResponse As GetPartyResponseType)

        Dim oResponseParty As New SiriusFS.SAM.Structure.SFI.SAMForInsurance.BasePartyOTHERType
        Dim oPartyOther As New SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyOTHERType

        oPartyOther = DirectCast(oImpResponse.Item, SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyOTHERType)
        oResponseParty = DirectCast(oResponse.Item, SiriusFS.SAM.Structure.SFI.SAMForInsurance.BasePartyOTHERType)

        If oPartyOther.Accident IsNot Nothing Then
            oResponseParty.Accident = Array.ConvertAll(oPartyOther.Accident, _
                New Converter(Of BaseImplementationTypes.BasePartyOTHERTypeAccident,  _
                BasePartyOTHERTypeAccident) _
                (AddressOf ToServiceAccident))
        End If

        If oPartyOther.Conviction IsNot Nothing Then
            oResponseParty.Conviction = Array.ConvertAll(oPartyOther.Conviction, _
                New Converter(Of BaseImplementationTypes.BasePartyOTHERTypeConviction,  _
                BasePartyOTHERTypeConviction) _
                (AddressOf ToServiceConviction))
        End If

        If oPartyOther.SupplierBusiness IsNot Nothing Then
            oResponseParty.SupplierBusiness = Array.ConvertAll(oPartyOther.SupplierBusiness, _
                New Converter(Of BaseImplementationTypes.BasePartyOTHERTypeSupplierBusiness,  _
                BasePartyOTHERTypeSupplierBusiness) _
                (AddressOf ToServiceSupplierBusiness))
        End If

        oResponseParty.AccountExecutive = oPartyOther.AccountExecutive
        oResponseParty.ActiveIndicator = oPartyOther.ActiveIndicator
        oResponseParty.ActiveIndicatorSpecified = oPartyOther.ActiveIndicatorSpecified
        oResponseParty.AfterHoursIndicator = oPartyOther.AfterHoursIndicator
        oResponseParty.AfterHoursIndicatorSpecified = oPartyOther.AfterHoursIndicatorSpecified
        oResponseParty.BranchCode = oPartyOther.BranchCode
        oResponseParty.Code = oPartyOther.Code
        oResponseParty.Currency = oPartyOther.Currency
        oResponseParty.DateOfBirth = oPartyOther.DateOfBirth
        oResponseParty.DomiciledForTax = oPartyOther.DomiciledForTax
        oResponseParty.DomiciledForTaxSpecified = oPartyOther.DomiciledForTaxSpecified
        oResponseParty.DriverStatusCode = oPartyOther.DriverStatusCode
        oResponseParty.Gender = oPartyOther.Gender
        oResponseParty.LicenseNumber = oPartyOther.LicenseNumber
        oResponseParty.LicenseTypeCode = oPartyOther.LicenseTypeCode
        oResponseParty.Name = oPartyOther.Name
        oResponseParty.PriorityIndicator = oPartyOther.PriorityIndicator
        oResponseParty.PriorityIndicatorSpecified = oPartyOther.PriorityIndicatorSpecified
        oResponseParty.RegNumber = oPartyOther.RegNumber
        oResponseParty.TaxExempt = oPartyOther.TaxExempt
        oResponseParty.TaxExemptSpecified = oPartyOther.TaxExemptSpecified
        oResponseParty.TaxNumber = oPartyOther.TaxNumber
        oResponseParty.TaxPercentage = oPartyOther.TaxPercentage
        oResponseParty.TaxPercentageSpecified = oPartyOther.TaxPercentageSpecified
        oResponseParty.TPIntroducer = oPartyOther.TPIntroducer
        oResponseParty.TPUserCode = oPartyOther.TPUserCode
        oResponseParty.TypeCode = oPartyOther.TypeCode

    End Sub

    Private Function ToServiceTaxesAndFeesType(ByVal taxesAndFees As BaseImplementationTypes.BaseTaxesAndFeesType) As BaseTaxesAndFeesType

        Dim serviceTaxesAndFees As BaseTaxesAndFeesType = New BaseTaxesAndFeesType

        If taxesAndFees IsNot Nothing Then

            If taxesAndFees.Fees IsNot Nothing Then
                serviceTaxesAndFees.Fees = Array.ConvertAll(taxesAndFees.Fees, _
                                New Converter(Of BaseImplementationTypes.BaseFeesType,  _
                                              BaseFeesType) _
                                              (AddressOf ToServiceFeesType))
            End If

            If taxesAndFees.Taxes IsNot Nothing Then
                serviceTaxesAndFees.Taxes = Array.ConvertAll(taxesAndFees.Taxes, _
                                New Converter(Of BaseImplementationTypes.BaseTaxesType,  _
                                              BaseTaxesType) _
                                              (AddressOf ToServiceTaxesType))
            End If

        End If

        Return serviceTaxesAndFees

    End Function

    Private Function ToServiceFeesType(ByVal fees As BaseImplementationTypes.BaseFeesType) As BaseFeesType

        Dim serviceFees As BaseFeesType = New BaseFeesType

        If fees IsNot Nothing Then
            serviceFees.Amount = fees.Amount
            serviceFees.Description = fees.Description
        End If

        Return serviceFees

    End Function

    Private Function ToServiceTaxesType(ByVal taxes As BaseImplementationTypes.BaseTaxesType) As BaseTaxesType

        Dim serviceTaxes As BaseTaxesType = New BaseTaxesType

        If taxes IsNot Nothing Then
            serviceTaxes.Amount = taxes.Amount
            serviceTaxes.Description = taxes.Description
        End If

        Return serviceTaxes

    End Function

    'Private Function ToBaseImplementationAddresses( _
    '        ByVal oExternalAddresses() As BaseAddressType _
    '        ) As BaseImplementationTypes.BaseAddressType()

    '    Dim oBaseImplementationAddresses() As BaseImplementationTypes.BaseAddressType
    '    Dim iLBound As Integer
    '    Dim iUBound As Integer

    '    ' Process the address structure
    '    If IsArray(oExternalAddresses) Then

    '        Dim msgAddress As BaseAddressType
    '        Dim impAddress As BaseImplementationTypes.BaseAddressType

    '        iLBound = oExternalAddresses.GetLowerBound(0)
    '        iUBound = oExternalAddresses.GetUpperBound(0)

    '        ReDim oBaseImplementationAddresses(iUBound)

    '        For iCnt As Integer = iLBound To iUBound

    '            msgAddress = oExternalAddresses(iCnt)
    '            impAddress = New BaseImplementationTypes.BaseAddressType

    '            impAddress.AddressLine1 = msgAddress.AddressLine1
    '            impAddress.AddressLine2 = msgAddress.AddressLine2
    '            impAddress.AddressLine3 = msgAddress.AddressLine3
    '            impAddress.AddressLine4 = msgAddress.AddressLine4
    '            impAddress.AddressTypeCode = msgAddress.AddressTypeCode
    '            impAddress.CountryCode = msgAddress.CountryCode
    '            impAddress.PostCode = msgAddress.PostCode

    '            oBaseImplementationAddresses(iCnt) = impAddress

    '        Next iCnt

    '    End If

    '    Return oBaseImplementationAddresses

    'Private Function ToBaseImplementationContracts( _
    '        ByVal oExternalContacts() As BaseContactType _
    '        ) As BaseImplementationTypes.BaseContactType()

    '    Dim oBaseImplementationContacts() As BaseImplementationTypes.BaseContactType
    '    Dim iLBound As Integer
    '    Dim iUBound As Integer

    '    ' Process the Contact structure
    '    If IsArray(oExternalContacts) Then

    '        Dim msgContact As BaseContactType
    '        Dim impContact As BaseImplementationTypes.BaseContactType

    '        iLBound = oExternalContacts.GetLowerBound(0)
    '        iUBound = oExternalContacts.GetUpperBound(0)

    '        ReDim oBaseImplementationContacts(iUBound)

    '        For iCnt As Integer = iLBound To iUBound
    '            msgContact = oExternalContacts(iCnt)
    '            impContact = New BaseImplementationTypes.BaseContactType

    '            impContact.AreaCode = msgContact.AreaCode
    '            impContact.ContactDetail = New BaseImplementationTypes.BaseContactDetailType
    '            impContact.ContactDetail.Item = msgContact.ContactDetail.Item
    '            impContact.ContactTypeCode = msgContact.ContactTypeCode

    '            oBaseImplementationContacts(iCnt) = impContact
    '        Next iCnt

    '    End If

    'Private Function ConvertToBaseImplementationClaimPerilType( _
    '      ByVal oExternalClaimPerils() As BaseClaimPerilType _
    '      ) As BaseImplementationTypes.BaseClaimPerilType()

    '    Dim oBaseImplementationClaimPerils() As BaseImplementationTypes.BaseClaimPerilType
    '    Dim iLBound As Integer
    '    Dim iUBound As Integer

    '    ' Process the Contact structure
    '    If IsArray(oExternalClaimPerils) Then

    '        Dim msgClaimPeril As BaseClaimPerilType
    '        Dim impClaimPeril As BaseImplementationTypes.BaseClaimPerilType

    '        iLBound = oExternalClaimPerils.GetLowerBound(0)
    '        iUBound = oExternalClaimPerils.GetUpperBound(0)

    '        ReDim oBaseImplementationClaimPerils(iUBound)

    '        For iCnt As Integer = iLBound To iUBound

    '            Dim impClaimPerilRecovery As BaseImplementationTypes.BaseClaimPerilRecoveryType
    '            Dim impClaimPerilReserve As BaseImplementationTypes.BaseClaimPerilRecoveryType

    '            msgClaimPeril = oExternalClaimPerils(iCnt)
    '            impClaimPeril = New BaseImplementationTypes.BaseClaimPerilType

    '            impClaimPeril.Description = msgClaimPeril.Description
    '            impClaimPeril.TypeCode = msgClaimPeril.TypeCode

    '            'impClaimPerilRecovery = ConvertToBaseImplementationRecovery(msgClaimPeril.Recovery)
    '            'impClaimPeril.Reserve = ConvertToBaseImplementationReserve(msgClaimPeril.Recovery)

    '            oBaseImplementationClaimPerils(iCnt) = impClaimPeril
    '        Next iCnt

    '    End If

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

    ''' <summary>
    ''' Retrieve the authenticated user name and agent key (if applicable).
    ''' </summary>
    ''' <param name="o_sUserName">The authenticated user name returned.</param>
    ''' <param name="o_iAgentKey">The agent key returned, or zero if the user is not an agent.</param>
    Private Sub GetIdentity(ByRef o_sUserName As String, ByRef o_iAgentKey As Integer, ByRef o_iUserId As Integer)

        Dim principal As SiriusPrincipal = SiriusPrincipal.ToSiriusPrincipal(RequestSoapContext.Current.IdentityToken.Principal)

        Dim oIdentity As SiriusIdentity = principal.Identity

        If SiriusUserTableAccess.GetValueAsString(oIdentity.ID, "party_type_code", String.Empty).Trim = PartyType.Agent Then
            o_iAgentKey = SiriusUserTableAccess.GetValueAsInt32(oIdentity.ID, "party_cnt", 0)
        End If

        o_sUserName = oIdentity.Name
        o_iUserId = oIdentity.ID

    End Sub

    <WebMethod()> _
    <SoapDocumentMethod(parameterstyle:=SoapParameterStyle.Bare)> _
    Public Function ClaimDataImport(<XmlElement(elementName:="ClaimDataImportRequest", Namespace:=ServiceNamespace)> ByVal ClaimDataImportRequest As ClaimDataImportRequestType) As <XmlElement(elementName:="ClaimDataImportResponse", Namespace:=ServiceNamespace)> ClaimDataImportResponseType

        Try
            CheckAuthority("SAMCLMData")


            Dim serviceResponse As New ClaimDataImportResponseType

            ' Implementation structures
            Dim impRequest As New SAMForInsuranceImplementationTypes.ClaimDataImportRequestType
            Dim impResponse As SAMForInsuranceImplementationTypes.ClaimDataImportResponseType = Nothing

            ' convert the service request to the base request
            ClaimDataImportIn(ClaimDataImportRequest, impRequest)

            Dim oBusiness As New SAMForInsuranceBusiness

            Try

                ' call into the business layer
                impResponse = oBusiness.ClaimDataImport(impRequest)

                ' historical check to ensure any sts errors raised are 
                ' actually picked up
                SAMErrorCollection.CheckForErrorsFromSTS(impResponse.STSError)

            Catch ex As Exception

                Handler.BusinessLayerBoundary(ex, serviceResponse)

            End Try

            ' build service response from base response
            ClaimDataImportOut(serviceResponse, impResponse)

            Return serviceResponse

        Catch ex As Exception
            Handler.BusinessLayerLastResort(ex, Context)
            Return Nothing
        End Try

    End Function
    Private Sub ClaimDataImportOut( _
    ByVal service As ClaimDataImportResponseType, _
    ByVal implementation As SAMForInsuranceImplementationTypes.ClaimDataImportResponseType)

    End Sub

    Private Sub ClaimDataImportIn( _
    ByVal service As ClaimDataImportRequestType, _
    ByVal implementation As SAMForInsuranceImplementationTypes.ClaimDataImportRequestType)

        implementation.BranchCode = service.BranchCode
        implementation.Claim = ToBaseImpBaseCDTClaimType(service.Claim)

        ' claim versioning mode - default to false ( partial versioning )
        implementation.UseFullClaimVersioningSpecified = True
        If service.UseFullClaimVersioningSpecified Then
            implementation.UseFullClaimVersioning = service.UseFullClaimVersioning
        Else
            implementation.UseFullClaimVersioning = False
        End If

    End Sub

    Private Function ToBaseImpBaseCDTClaimType(ByVal service As BaseCDTClaimType) _
        As BaseImplementationTypes.BaseCDTClaimType

        Dim implementation As BaseImplementationTypes.BaseCDTClaimType = Nothing

        If service IsNot Nothing Then

            implementation = New BaseImplementationTypes.BaseCDTClaimType

            implementation.CatastropheCode = service.CatastropheCode
            implementation.ClaimNumber = service.ClaimNumber
            implementation.ClaimVersionDescription = service.ClaimVersionDescription
            implementation.Comments = service.Comments
            implementation.CurrencyCode = service.CurrencyCode
            implementation.Description = service.Description
            implementation.HandlerCode = service.HandlerCode
            implementation.InfoOnly = service.InfoOnly
            implementation.LikelyClaim = service.LikelyClaim
            implementation.Location = service.Location
            implementation.LossFromDate = service.LossFromDate
            implementation.LossToDate = service.LossToDate
            implementation.LossToDateSpecified = service.LossToDateSpecified
            implementation.PrimaryCauseCode = service.PrimaryCauseCode
            implementation.ProgressStatusCode = service.ProgressStatusCode
            implementation.ReportedDate = service.ReportedDate
            implementation.SAMStagingClaimKey = service.SAMStagingClaimKey
            implementation.SecondaryCauseCode = service.SecondaryCauseCode
            implementation.SiriusInsuranceFileKey = service.SiriusInsuranceFileKey
            implementation.SiriusRiskKey = service.SiriusRiskKey
            implementation.TownCode = service.TownCode
            implementation.UnderwritingYearCode = service.UnderwritingYearCode
            implementation.XMLDATASET = service.XMLDATASET

            implementation.TransactionDateSpecified = True
            If service.TransactionDateSpecified Then
                implementation.TransactionDate = service.TransactionDate
            Else
                implementation.TransactionDate = Date.Now
            End If

            implementation.VersionNoSpecified = True
            If service.VersionNoSpecified Then
                implementation.VersionNo = service.VersionNo
            Else
                implementation.VersionNo = 0
            End If

            If service.ClaimPeril IsNot Nothing Then
                implementation.ClaimPeril = Array.ConvertAll(service.ClaimPeril, _
                                        New Converter(Of BaseCDTClaimPerilType,  _
                                                BaseImplementationTypes.BaseCDTClaimPerilType) _
                                            (AddressOf ToBaseImpBaseCDTClaimPerilType))

            End If

            If service.ClaimReinsurance IsNot Nothing Then
                implementation.ClaimReinsuranceForDTU = ToBaseImpBaseCDTClaimReinsuranceType(service.ClaimReinsurance)
            End If

        End If

        Return implementation

    End Function

    Private Function ToBaseImpBaseCDTClaimPerilType(ByVal service As BaseCDTClaimPerilType) _
        As BaseImplementationTypes.BaseCDTClaimPerilType

        Dim implementation As New BaseImplementationTypes.BaseCDTClaimPerilType

        If service IsNot Nothing Then

            implementation.SAMStagingBaseClaimPerilKey = service.BaseClaimPerilKey
            implementation.SAMStagingClaimPerilKey = service.SAMStagingClaimPerilKey
            implementation.Description = service.Description
            implementation.TypeCode = service.TypeCode

            If service.ClaimPayment IsNot Nothing Then
                implementation.ClaimPayment = Array.ConvertAll(service.ClaimPayment, _
                                                    New Converter(Of BaseCDTClaimPaymentType, BaseImplementationTypes.BaseCDTClaimPaymentType) _
                                                    (AddressOf ToBaseImpBaseCDTClaimPaymentType))
            End If

            If service.ClaimReceipt IsNot Nothing Then
                implementation.ClaimReceipt = Array.ConvertAll(service.ClaimReceipt, _
                                                    New Converter(Of BaseCDTClaimReceiptType, BaseImplementationTypes.BaseCDTClaimReceiptType) _
                                                    (AddressOf ToBaseImpBaseCDTClaimReceiptType))
            End If

            If service.Recovery IsNot Nothing Then
                implementation.Recovery = Array.ConvertAll(service.Recovery, _
                                                    New Converter(Of BaseCDTRecoveryType, BaseImplementationTypes.BaseCDTRecoveryType) _
                                                    (AddressOf ToBaseImpBaseCDTRecoveryType))
            End If

            If service.Reserve IsNot Nothing Then
                implementation.Reserve = Array.ConvertAll(service.Reserve, _
                                                New Converter(Of BaseCDTReserveType, BaseImplementationTypes.BaseCDTReserveType) _
                                                    (AddressOf ToBaseImpBaseCDTReserveType))
            End If

        End If

        Return implementation

    End Function

    Private Function ToBaseImpBaseCDTReserveType(ByVal service As BaseCDTReserveType) _
        As BaseImplementationTypes.BaseCDTReserveType

        Dim implementation As New BaseImplementationTypes.BaseCDTReserveType

        If service IsNot Nothing Then
            implementation.RevisionAmount = service.RevisionAmount
            implementation.SAMStagingReserveKey = service.SAMStagingReserveKey
            implementation.TypeCode = service.TypeCode
        End If

        Return implementation

    End Function

    Private Function ToBaseImpBaseCDTRecoveryType(ByVal service As BaseCDTRecoveryType) _
            As BaseImplementationTypes.BaseCDTRecoveryType

        Dim implementation As New BaseImplementationTypes.BaseCDTRecoveryType

        If service IsNot Nothing Then

            implementation.RevisionAmount = service.RevisionAmount
            implementation.SAMStagingRecoveryKey = service.SAMStagingRecoveryKey
            implementation.TypeCode = service.TypeCode

        End If

        Return implementation

    End Function

    Private Function ToBaseImpBaseClaimPayeeType(ByVal service As BaseClaimPayeeType) As BaseImplementationTypes.BaseClaimPayeeType

        Dim implementation As New BaseImplementationTypes.BaseClaimPayeeType

        implementation.Address = ToBaseImpBaseAddressType(service.Address)
        implementation.BankCode = service.BankCode
        implementation.BankName = service.BankName
        implementation.BankNumber = service.BankNumber
        implementation.Comments = service.Comments
        implementation.MediaReference = service.MediaReference()
        implementation.MediaTypeCode = service.MediaTypeCode
        implementation.Name = service.Name
        implementation.TheirReference = service.TheirReference

        Return implementation

    End Function

    Private Function ToBaseImpBaseCDTReceiptITemType(ByVal service As BaseCDTReceiptItemType) _
        As BaseImplementationTypes.BaseCDTReceiptItemType

        Dim implementation As New BaseImplementationTypes.BaseCDTReceiptItemType

        If service IsNot Nothing Then

            implementation.ReceiptAmount = service.ReceiptAmount
            implementation.RecoveryTypeCode = service.RecoveryTypeCode
            implementation.SAMStagingClaimReceiptItemKey = service.SAMStagingClaimReceiptItemKey
            implementation.TaxGroupCode = service.TaxGroupCode

        End If

        Return implementation
    End Function

    Private Function ToBaseImpBaseCDTClaimRIArrangmentLineType(ByVal service As BaseCDTClaimRIArrangmentLineType) _
            As BaseImplementationTypes.BaseCDTClaimRIArrangmentLineType

        Dim implementation As New BaseImplementationTypes.BaseCDTClaimRIArrangmentLineType

        If service IsNot Nothing Then

            implementation.AgreementCode = service.AgreementCode
            implementation.DefaultSharePercent = service.DefaultSharePercent
            implementation.Grouping = service.Grouping
            implementation.LineLimit = service.LineLimit
            implementation.LowerLimit = service.LowerLimit
            implementation.NumberOfLines = service.NumberOfLines
            implementation.ParticipationPercent = service.ParticipationPercent
            implementation.PartyKey = service.PartyKey
            implementation.Payment = service.Payment
            implementation.Priority = service.Priority
            implementation.Recovery = service.Recovery
            implementation.Reserve = service.Reserve
            implementation.Retained = service.Retained
            implementation.Salvage = service.Salvage
            implementation.SAMStagingClaimRIArrangementKey = service.SAMStagingClaimRIArrangementKey
            implementation.SAMStagingClaimRIArrangementLineKey = service.SAMStagingClaimRIArrangementLineKey
            implementation.SumInsured = service.SumInsured
            implementation.ThisPayment = service.ThisPayment
            implementation.ThisRecovery = service.ThisRecovery
            implementation.ThisReserve = service.ThisReserve
            implementation.ThisSalvage = service.ThisSalvage
            implementation.ThisSharePercent = service.ThisSharePercent
            implementation.TreatyCode = service.TreatyCode
            implementation.Type = service.Type

        End If

        Return implementation

    End Function

    Private Function ToBaseImpCDTCLaimReinsuranceTypeRIArrangement(ByVal service() As BaseCDTClaimReinsuranceTypeClaimRIArrangement) _
            As List(Of BaseImplementationTypes.BaseCDTClaimReinsuranceTypeClaimRIArrangement)

        'Dim implementation As New BaseImplementationTypes.BaseCDTClaimReinsuranceTypeClaimRIArrangement
        Dim implementation As New List(Of BaseImplementationTypes.BaseCDTClaimReinsuranceTypeClaimRIArrangement)
        Dim implementation1 As New BaseImplementationTypes.BaseCDTClaimReinsuranceTypeClaimRIArrangement
        Dim i As Integer = 0

        If service IsNot Nothing Then

            For i = 0 To service.Length - 1
                implementation1.ClaimAllocationType = service(i).ClaimAllocationType
                implementation1.ClaimRIArrangmentLine = Array.ConvertAll(service(i).ClaimRIArrangmentLine,
                                                        New Converter(Of BaseCDTClaimRIArrangmentLineType, 
                                                         BaseImplementationTypes.BaseCDTClaimRIArrangmentLineType) _
                                                            (AddressOf ToBaseImpBaseCDTClaimRIArrangmentLineType))

                implementation1.Payment = service(i).Payment
                implementation1.Recovery = service(i).Recovery
                implementation1.Reserve = service(i).Reserve
                implementation1.RIArrangementKey = service(i).RIArrangementKey
                implementation1.RIBandCode = service(i).RIBandCode
                implementation1.RIModelCode = service(i).RIModelCode
                implementation1.Salvage = service(i).Salvage
                implementation1.SAMStagingClaimRIArrangementKey = service(i).SAMStagingClaimRIArrangementKey
                implementation1.SumInsured = service(i).SumInsured
                implementation1.ThisPayment = service(i).ThisPayment
                implementation1.ThisRecovery = service(i).ThisRecovery
                implementation1.ThisReserve = service(i).ThisReserve
                implementation1.ThisSalvage = service(i).ThisSalvage
                implementation.Add(implementation1)
            Next
        End If

        Return implementation

    End Function

    Private Function ToBaseImpBaseCDTClaimReinsuranceType(ByVal service As BaseCDTClaimReinsuranceType) _
        As BaseImplementationTypes.BaseCDTClaimReinsuranceTypeForDTU

        Dim implementation As New BaseImplementationTypes.BaseCDTClaimReinsuranceTypeForDTU

        If service IsNot Nothing Then

            implementation.ClaimRIArrangement = ToBaseImpCDTCLaimReinsuranceTypeRIArrangement(service.ClaimRIArrangement)

        End If

        Return implementation

    End Function

    Private Function ToBaseImpBaseCDTClaimReceiptType(ByVal service As BaseCDTClaimReceiptType) _
        As BaseImplementationTypes.BaseCDTClaimReceiptType

        Dim implementation As New BaseImplementationTypes.BaseCDTClaimReceiptType

        If service IsNot Nothing Then

            If service.ClaimReceiptItem IsNot Nothing Then
                implementation.ClaimReceiptItem = Array.ConvertAll(service.ClaimReceiptItem, _
                                                    New Converter(Of BaseCDTReceiptItemType,  _
                                                     BaseImplementationTypes.BaseCDTReceiptItemType) _
                                                        (AddressOf ToBaseImpBaseCDTReceiptITemType))
            End If

            If service.ClaimReinsurance IsNot Nothing Then
                implementation.ClaimReinsuranceForDTU = ToBaseImpBaseCDTClaimReinsuranceType(service.ClaimReinsurance)
            End If

            implementation.CurrencyCode = service.CurrencyCode
            implementation.PartyKey = service.PartyKey
            implementation.TransactionDate = service.TransactionDate
            implementation.TransactionDateSpecified = service.TransactionDateSpecified
            implementation.IsSalvageRecovery = service.IsSalvageRecovery

            If service.Payee IsNot Nothing Then
                implementation.Payee = ToBaseImpBaseClaimPayeeType(service.Payee)
            End If

            implementation.ReceiptPartyType = CType([Enum].ToObject(GetType(ClaimReceiptPartyTypeType), service.ReceiptPartyType), BaseImplementationTypes.ClaimReceiptPartyTypeType)

            'implementation.ReceiptPartyType = service.ReceiptPartyType
            implementation.SAMStagingClaimReceiptKey = service.SAMStagingClaimReceiptKey

        End If

        Return implementation

    End Function

    Private Function ToBaseImpBaseCDTClaimPaymentItemType(ByVal service As BaseCDTClaimPaymentItemType) _
        As BaseImplementationTypes.BaseCDTClaimPaymentItemType

        Dim implementation As New BaseImplementationTypes.BaseCDTClaimPaymentItemType

        If service IsNot Nothing Then

            implementation.PaymentAmount = service.PaymentAmount
            implementation.ReserveTypeCode = service.ReserveTypeCode
            implementation.ReverseExcess = service.ReverseExcess
            implementation.SAMStagingClaimPaymentItemKey = service.SAMStagingClaimPaymentItemKey
            implementation.TaxGroupCode = service.TaxGroupCode

        End If

        Return implementation

    End Function

    Private Function ToBaseImpBaseCDTClaimPaymentType(ByVal service As BaseCDTClaimPaymentType) _
        As BaseImplementationTypes.BaseCDTClaimPaymentType

        Dim implementation As New BaseImplementationTypes.BaseCDTClaimPaymentType

        If service.ClaimPaymentItem IsNot Nothing Then
            implementation.ClaimPaymentItem = Array.ConvertAll(service.ClaimPaymentItem, _
                                    New Converter(Of BaseCDTClaimPaymentItemType,  _
                                    BaseImplementationTypes.BaseCDTClaimPaymentItemType) _
                                        (AddressOf ToBaseImpBaseCDTClaimPaymentItemType))

            implementation.ClaimReinsuranceForDTU = ToBaseImpBaseCDTClaimReinsuranceType(service.ClaimReinsurance)
            implementation.CurrencyCode = service.CurrencyCode
            implementation.PartyKey = service.PartyKey
            implementation.TransactionDate = service.TransactionDate
            implementation.TransactionDateSpecified = service.TransactionDateSpecified

            If service.Payee IsNot Nothing Then
                implementation.Payee = ToBaseImpBaseClaimPayeeType(service.Payee)
            End If

            implementation.PaymentPartyType = CType([Enum].ToObject(GetType(ClaimPaymentPartyTypeType), service.PaymentPartyType), BaseImplementationTypes.ClaimPaymentPartyTypeType)
            implementation.SAMStagingClaimPaymentKey = service.SAMStagingClaimPaymentKey
        End If

        Return implementation

    End Function

End Class

