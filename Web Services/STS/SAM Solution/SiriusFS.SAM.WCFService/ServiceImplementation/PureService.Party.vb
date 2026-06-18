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
    ''' <summary>
    ''' To search parties for given search criteria
    ''' </summary>
    ''' <param name="FindPartyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindParty(ByVal FindPartyRequest As FindPartyRequestType) As FindPartyResponseType Implements IPurePartyService.FindParty, IPureAccountService.FindParty, IPureClaimService.FindParty, IPurePolicyService.FindParty

        Try
            Dim sUserName As String = FindPartyRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMFPty", iUserId)
            CommonFunctions.CheckSecurityToken(FindPartyRequest.WCFSecurityToken)
            Dim oResponse As New FindPartyResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, FindPartyRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.FindPartyRequestType
            Dim oImpResponse As New SAMForInsuranceV2ImplementationTypes.FindPartyResponseType

            ' Pass the values to the implementation request structure
            oImpRequest.AddressLine1 = FindPartyRequest.AddressLine1
            oImpRequest.AddressLine2 = FindPartyRequest.AddressLine2
            oImpRequest.AddressLine3 = FindPartyRequest.AddressLine3
            oImpRequest.AddressLine4 = FindPartyRequest.AddressLine4

            oImpRequest.AddressLine5 = FindPartyRequest.AddressLine5
            oImpRequest.AddressLine6 = FindPartyRequest.AddressLine6
            oImpRequest.AddressLine7 = FindPartyRequest.AddressLine7
            oImpRequest.AddressLine8 = FindPartyRequest.AddressLine8
            oImpRequest.AddressLine9 = FindPartyRequest.AddressLine9
            oImpRequest.AddressLine10 = FindPartyRequest.AddressLine10



            oImpRequest.AgentKey = iAgentKey
            oImpRequest.AlternativeId = FindPartyRequest.AlternativeId
            oImpRequest.AreaCode = FindPartyRequest.AreaCode
            oImpRequest.BranchCode = FindPartyRequest.BranchCode
            oImpRequest.DateOfBirth = FindPartyRequest.DateOfBirth
            oImpRequest.DateOfBirthSpecified = FindPartyRequest.DateOfBirthSpecified
            oImpRequest.Firstname = Trim(FindPartyRequest.Firstname)
            oImpRequest.Name = Trim(FindPartyRequest.Name)
            oImpRequest.IsAnySelected = FindPartyRequest.IsAnySelected

            oImpRequest.PartyTypeSpecified = FindPartyRequest.PartyTypeSpecified
            oImpRequest.LoginUserName = FindPartyRequest.LoginUserName
            If FindPartyRequest.PartyTypeSpecified = True Then
                If FindPartyRequest.PartyType <> PartyTypeType.OTOTHERPARTY Then
                    oImpRequest.PartyType = [Enum].GetName(GetType(PartyTypeType), FindPartyRequest.PartyType)
                Else
                    oImpRequest.PartyType = "OTOTHERPARTY"
                    oImpRequest.OtherPartyTypeCode = FindPartyRequest.OtherPartyTypeCode
                End If
            Else
                oImpRequest.PartyType = String.Empty
            End If

            If (FindPartyRequest.PartyIndex IsNot Nothing AndAlso FindPartyRequest.PartyIndex.Trim.Length > 0) OrElse
                   (FindPartyRequest.RiskIndex IsNot Nothing AndAlso FindPartyRequest.RiskIndex.Trim.Length > 0) Then
                oImpRequest.PartyTypeSpecified = FindPartyRequest.PartyTypeSpecified
            End If

            oImpRequest.PolicyRef = FindPartyRequest.PolicyRef
            oImpRequest.PostCode = FindPartyRequest.PostCode
            oImpRequest.RiskRequestdex = FindPartyRequest.RiskIndex
            oImpRequest.PartyIndex = FindPartyRequest.PartyIndex
            oImpRequest.Shortname = Trim(FindPartyRequest.Shortname)
            oImpRequest.TelephoneNumber = FindPartyRequest.TelephoneNumber
            oImpRequest.FileCode = FindPartyRequest.FileCode
            oImpRequest.ClaimNumber = FindPartyRequest.ClaimNumber
            oImpRequest.IncludeClosedBranches = FindPartyRequest.IncludeClosedBranches
            oImpRequest.Status = FindPartyRequest.Status
            oImpRequest.ClaimsRiskIndex = FindPartyRequest.ClaimsRiskIndex

            oImpRequest.AgentTypeSpecified = FindPartyRequest.AgentTypeSpecified
            If FindPartyRequest.AgentTypeSpecified = True Then
                oImpRequest.AgentType = CType([Enum].ToObject(GetType(PartyAgentType), FindPartyRequest.AgentType), BaseImplementationTypes.PartyAgentType)
            End If
            oImpRequest.SupressSubAgentsSpecified = FindPartyRequest.SupressSubAgentsSpecified
            If oImpRequest.SupressSubAgentsSpecified = True Then
                oImpRequest.SupressSubAgents = FindPartyRequest.SupressSubAgents
            End If
            oImpRequest.PartySourceId = FindPartyRequest.PartySourceId
            oImpRequest.TransactionType = FindPartyRequest.TransactionType

            oImpRequest.MaxRowsToFetchSpecified = FindPartyRequest.MaxRowsToFetchSpecified
            If FindPartyRequest.MaxRowsToFetchSpecified Then
                oImpRequest.MaxRowsToFetch = FindPartyRequest.MaxRowsToFetch
            Else
                oImpRequest.MaxRowsToFetch = -1
            End If
            If FindPartyRequest.CaseNumberSpecified Then
                oImpRequest.CaseNumber = FindPartyRequest.CaseNumber
            End If
            oImpRequest.WCFSecurityToken = If(FindPartyRequest.WCFSecurityToken.Length > 0, FindPartyRequest.WCFSecurityToken, "WCFSecurityToken")

            If FindPartyRequest.AgentGroup <> "" Then
                oImpRequest.AgentGroup = FindPartyRequest.AgentGroup
            End If

            If FindPartyRequest.SearchType Is Nothing Then
                oImpRequest.SearchType = ""
            Else
                oImpRequest.SearchType = FindPartyRequest.SearchType
            End If
            oImpRequest.IncludeAgent = FindPartyRequest.IncludeAgent

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.FindParty(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Parties = DataTabletoList_FindParty(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(FindPartyRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(FindPartyRequest))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' To Add a new Party
    ''' </summary>
    ''' <param name="AddPartyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddParty(ByVal AddPartyRequest As AddPartyRequestType) As AddPartyResponseType Implements IPurePartyService.AddParty
        Try
            Dim sUserName As String = AddPartyRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMAPty", iUserId)
            CommonFunctions.CheckSecurityToken(AddPartyRequest.WCFSecurityToken)
            Dim oResponse As New AddPartyResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, AddPartyRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.AddPartyRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.AddPartyResponseType = Nothing

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
                Dim impConviction As New BaseImplementationTypes.BaseConvictionType
                Dim msgAddress As BaseAddressType
                Dim msgContact As BaseContactType

                ' Check the type of the party object to see if it is Personal or Corporate
                If AddPartyRequest.Item.GetType Is GetType(BasePartyPCType) Then

                    ' Process personal client
                    Dim msgParty As BasePartyPCType = DirectCast(AddPartyRequest.Item, BasePartyPCType)
                    Dim impPartyPC As New BaseImplementationTypes.BasePartyPCType

                    Dim impPCClientDetail As New BaseImplementationTypes.BaseClientSharedDataType
                    Dim impPCAssociate(0) As BaseImplementationTypes.BaseAssociateType
                    Dim impPCLoyaltyScheme() As BaseImplementationTypes.BaseClientSharedDataTypeLoyaltyScheme = Nothing
                    Dim impPCProspectPolicies() As BaseImplementationTypes.BaseClientSharedDataTypeProspectPolicies = Nothing
                    Dim impPCLifeStyle() As BaseImplementationTypes.BasePartyPCTypeLifestyle = Nothing
                    Dim impPcConvictionsDetail() As BaseImplementationTypes.BaseConvictionType = Nothing

                    impPartyPC.Forename = Trim(msgParty.Forename)
                    impPartyPC.Surname = Trim(msgParty.Surname)
                    impPartyPC.Initials = Trim(msgParty.Initials)
                    impPartyPC.Title = msgParty.Title
                    impPartyPC.DateOfBirth = msgParty.DateOfBirth
                    impPartyPC.GenderCode = msgParty.GenderCode

                    impPartyPC.MaritalStatusCode = CType([Enum].ToObject(GetType(MaritalStatusCodeType), msgParty.MaritalStatusCode), BaseImplementationTypes.MaritalStatusCodeType)
                    impPartyPC.MaritalStatusCodeSpecified = msgParty.MaritalStatusCodeSpecified
                    impPartyPC.OccupationCode = msgParty.OccupationCode
                    impPartyPC.EmploymentStatusCode = CType([Enum].ToObject(GetType(EmploymentStatusCodeType), msgParty.EmploymentStatusCode), BaseImplementationTypes.EmploymentStatusCodeType)
                    impPartyPC.EmployersBusinessCode = msgParty.EmployersBusinessCode
                    impPartyPC.AlternativeId = msgParty.AlternativeId
                    impPartyPC.TradingName = Trim(msgParty.TradingName)
                    impPartyPC.SecOccupationCode = msgParty.SecOccupationCode
                    impPartyPC.SecEmployersBusinessCode = msgParty.SecEmployersBusinessCode
                    impPartyPC.SecEmploymentStatusCode = CType([Enum].ToObject(GetType(EmploymentStatusCodeType), msgParty.SecEmploymentStatusCode), BaseImplementationTypes.EmploymentStatusCodeType)
                    impPartyPC.SecEmploymentStatusCodeSpecified = msgParty.SecEmploymentStatusCodeSpecified
                    impPartyPC.NationalityCode = msgParty.NationalityCode
                    impPartyPC.AccommodationCode = msgParty.AccommodationCode
                    impPartyPC.Salutation = msgParty.Salutation
                    impPartyPC.TPS = msgParty.TPS
                    impPartyPC.TPSSpecified = msgParty.TPSSpecified
                    impPartyPC.MPS = msgParty.MPS
                    impPartyPC.MPSSpecified = msgParty.MPSSpecified
                    impPartyPC.eMPS = msgParty.eMPS
                    impPartyPC.eMPSSpecified = msgParty.eMPSSpecified
                    impPartyPC.Source = msgParty.Source
                    impPartyPC.PetOwner = msgParty.PetOwner
                    impPartyPC.PetOwnerSpecified = msgParty.PetOwnerSpecified

                    impPartyPC.AccountExecutiveCode = msgParty.AccountExecutiveCode
                    impPartyPC.AccountExecutive = msgParty.AccountExecutive

                    If msgParty.ClientDetail IsNot Nothing Then
                        impPCClientDetail = New BaseImplementationTypes.BaseClientSharedDataType
                        impPartyPC.ClientDetail = impPCClientDetail

                        impPCClientDetail.ServiceLevelCode = msgParty.ClientDetail.ServiceLevelCode
                        impPCClientDetail.AreaCode = msgParty.ClientDetail.AreaCode
                        impPCClientDetail.LeadAgentKey = msgParty.ClientDetail.LeadAgentKey
                        impPCClientDetail.LeadAgentKeySpecified = msgParty.ClientDetail.LeadAgentKeySpecified
                        impPCClientDetail.IsProspect = msgParty.ClientDetail.IsProspect
                        impPCClientDetail.IsProspectSpecified = msgParty.ClientDetail.IsProspectSpecified
                        impPCClientDetail.IsAgent = msgParty.ClientDetail.IsAgent
                        impPCClientDetail.IsAgentSpecified = msgParty.ClientDetail.IsAgentSpecified
                        impPCClientDetail.CorrespondenceCode = msgParty.ClientDetail.CorrespondenceCode
                        impPCClientDetail.PaymentCode = msgParty.ClientDetail.PaymentCode
                        impPCClientDetail.ReminderCode = msgParty.ClientDetail.ReminderCode
                        impPCClientDetail.PaymentTermCode = msgParty.ClientDetail.PaymentTermCode
                        impPCClientDetail.RenewalStopCode = msgParty.ClientDetail.RenewalStopCode
                        impPCClientDetail.LoyaltyNumber = msgParty.ClientDetail.LoyaltyNumber
                        impPCClientDetail.SeasonalGiftCode = msgParty.ClientDetail.SeasonalGiftCode
                        impPCClientDetail.CountyCourtJudgments = msgParty.ClientDetail.CountyCourtJudgments
                        impPCClientDetail.CountyCourtJudgmentsSpecified = msgParty.ClientDetail.CountyCourtJudgmentsSpecified
                        impPCClientDetail.AgentReference = msgParty.ClientDetail.AgentReference
                        impPCClientDetail.CurrentIntermediaryKey = msgParty.ClientDetail.CurrentIntermediaryKey
                        impPCClientDetail.CurrentIntermediaryKeySpecified = msgParty.ClientDetail.CurrentIntermediaryKeySpecified
                        impPCClientDetail.StrengthCode = msgParty.ClientDetail.StrengthCode
                        impPCClientDetail.StatusCode = msgParty.ClientDetail.StatusCode
                        impPCClientDetail.PreviousInsurerKey = msgParty.ClientDetail.PreviousInsurerKey
                        impPCClientDetail.PreviousInsurerKeySpecified = msgParty.ClientDetail.PreviousInsurerKeySpecified
                        impPCClientDetail.PreviousBrokerKey = msgParty.ClientDetail.PreviousBrokerKey
                        impPCClientDetail.PreviousBrokerKeySpecified = msgParty.ClientDetail.PreviousBrokerKeySpecified


                        If (msgParty.ClientDetail.Associates) IsNot Nothing Then
                            For cntAsoc As Integer = 0 To _
                                                     msgParty.ClientDetail.Associates.Count - 1

                                ReDim Preserve impPCAssociate(cntAsoc)
                                impPCAssociate(cntAsoc) = New BaseImplementationTypes.BaseAssociateType
                                impPCAssociate(cntAsoc).ClientKey = msgParty.ClientDetail.Associates(cntAsoc).ClientKey
                                impPCAssociate(cntAsoc).AssociateKey = msgParty.ClientDetail.Associates(cntAsoc).AssociateKey
                                impPCAssociate(cntAsoc).RelationshipCode = msgParty.ClientDetail.Associates(cntAsoc).RelationshipCode
                                impPCAssociate(cntAsoc).RelationshipDescription = msgParty.ClientDetail.Associates(cntAsoc).RelationshipDescription
                            Next
                            impPartyPC.ClientDetail.Associates = impPCAssociate
                        End If

                        If msgParty.ClientDetail.LoyaltyScheme IsNot Nothing Then
                            For cntLS As Integer = 0 To _
                                msgParty.ClientDetail.LoyaltyScheme.Count - 1

                                ReDim Preserve impPCLoyaltyScheme(cntLS)
                                impPCLoyaltyScheme(cntLS) = New BaseImplementationTypes.BaseClientSharedDataTypeLoyaltyScheme ' Vivek: typo in Tech Spec
                                impPCLoyaltyScheme(cntLS).LoyaltySchemeKey = msgParty.ClientDetail.LoyaltyScheme(cntLS).LoyaltySchemeKey
                                impPCLoyaltyScheme(cntLS).LoyaltySchemeCode = msgParty.ClientDetail.LoyaltyScheme(cntLS).LoyaltySchemeCode
                                impPCLoyaltyScheme(cntLS).MembershipNumber = msgParty.ClientDetail.LoyaltyScheme(cntLS).MembershipNumber
                                impPCLoyaltyScheme(cntLS).OtherReference = msgParty.ClientDetail.LoyaltyScheme(cntLS).OtherReference
                                impPCLoyaltyScheme(cntLS).StartDate = msgParty.ClientDetail.LoyaltyScheme(cntLS).StartDate
                                impPCLoyaltyScheme(cntLS).EndDate = msgParty.ClientDetail.LoyaltyScheme(cntLS).EndDate
                                impPCLoyaltyScheme(cntLS).EndDateSpecified = msgParty.ClientDetail.LoyaltyScheme(cntLS).EndDateSpecified
                                impPCLoyaltyScheme(cntLS).MainMember = msgParty.ClientDetail.LoyaltyScheme(cntLS).MainMember
                                impPCLoyaltyScheme(cntLS).Active = msgParty.ClientDetail.LoyaltyScheme(cntLS).Active
                                impPCLoyaltyScheme(cntLS).ActiveSpecified = msgParty.ClientDetail.LoyaltyScheme(cntLS).ActiveSpecified
                            Next
                            impPartyPC.ClientDetail.LoyaltyScheme = impPCLoyaltyScheme
                        End If

                        If msgParty.ClientDetail.ProspectPolicies IsNot Nothing Then
                            For cntPP As Integer = 0 To _
                                msgParty.ClientDetail.ProspectPolicies.Count - 1

                                ReDim Preserve impPCProspectPolicies(cntPP)
                                impPCProspectPolicies(cntPP) = New BaseImplementationTypes.BaseClientSharedDataTypeProspectPolicies
                                impPCProspectPolicies(cntPP).ProspectPolicyKey = msgParty.ClientDetail.ProspectPolicies(cntPP).ProspectPolicyKey
                                impPCProspectPolicies(cntPP).ProspectTypeCode = msgParty.ClientDetail.ProspectPolicies(cntPP).ProspectTypeCode
                                impPCProspectPolicies(cntPP).RenewalDate = msgParty.ClientDetail.ProspectPolicies(cntPP).RenewalDate
                                impPCProspectPolicies(cntPP).RenewalDateSpecified = msgParty.ClientDetail.ProspectPolicies(cntPP).RenewalDateSpecified
                                impPCProspectPolicies(cntPP).TimesQuoted = msgParty.ClientDetail.ProspectPolicies(cntPP).TimesQuoted
                                impPCProspectPolicies(cntPP).TimesQuotedSpecified = msgParty.ClientDetail.ProspectPolicies(cntPP).TimesQuotedSpecified
                                impPCProspectPolicies(cntPP).TargetPremium = msgParty.ClientDetail.ProspectPolicies(cntPP).TargetPremium
                                impPCProspectPolicies(cntPP).TargetPremiumSpecified = msgParty.ClientDetail.ProspectPolicies(cntPP).TargetPremiumSpecified
                            Next
                            impPartyPC.ClientDetail.ProspectPolicies = impPCProspectPolicies
                        End If

                        If msgParty.Lifestyle IsNot Nothing Then
                            For cntLife As Integer = 0 To msgParty.Lifestyle.Count - 1

                                ReDim Preserve impPCLifeStyle(cntLife)
                                impPCLifeStyle(cntLife) = New BaseImplementationTypes.BasePartyPCTypeLifestyle
                                impPCLifeStyle(cntLife).LifestyleKey = msgParty.Lifestyle(cntLife).LifestyleKey
                                impPCLifeStyle(cntLife).Name = msgParty.Lifestyle(cntLife).Name
                                impPCLifeStyle(cntLife).DateOfBirth = msgParty.Lifestyle(cntLife).DateOfBirth
                                impPCLifeStyle(cntLife).DateOfBirthSpecified = msgParty.Lifestyle(cntLife).DateOfBirthSpecified
                                impPCLifeStyle(cntLife).CategoryCode = msgParty.Lifestyle(cntLife).CategoryCode
                                If msgParty.Lifestyle(cntLife).GenderCode = GenderCodeType.M Then
                                    impPCLifeStyle(cntLife).GenderCode = BaseImplementationTypes.GenderCodeType.Male
                                ElseIf msgParty.Lifestyle(cntLife).GenderCode = GenderCodeType.F Then
                                    impPCLifeStyle(cntLife).GenderCode = BaseImplementationTypes.GenderCodeType.Female
                                Else
                                    impPCLifeStyle(cntLife).GenderCode = BaseImplementationTypes.GenderCodeType.NotApplicable
                                End If
                                impPCLifeStyle(cntLife).GenderCodeSpecified = msgParty.Lifestyle(cntLife).GenderCodeSpecified
                                impPCLifeStyle(cntLife).OccupationCode = msgParty.Lifestyle(cntLife).OccupationCode
                                impPCLifeStyle(cntLife).SecOccupationCode = msgParty.Lifestyle(cntLife).SecOccupationCode
                                impPCLifeStyle(cntLife).Smoker = msgParty.Lifestyle(cntLife).Smoker
                                impPCLifeStyle(cntLife).SmokerSpecified = msgParty.Lifestyle(cntLife).SmokerSpecified

                            Next
                            impPartyPC.Lifestyle = impPCLifeStyle
                        End If

                        If msgParty.ClientDetail.Convictions IsNot Nothing Then
                            For iConvictions As Integer = 0 To msgParty.ClientDetail.Convictions.Count - 1
                                ReDim Preserve impPcConvictionsDetail(iConvictions)

                                impPcConvictionsDetail(iConvictions) = New BaseImplementationTypes.BaseConvictionType
                                impPcConvictionsDetail(iConvictions).AlcoholLevel = msgParty.ClientDetail.Convictions(iConvictions).AlcoholLevel
                                impPcConvictionsDetail(iConvictions).AlcoholLevelSpecified = msgParty.ClientDetail.Convictions(iConvictions).AlcoholLevelSpecified
                                impPcConvictionsDetail(iConvictions).AlcoholMeasurementMethod = msgParty.ClientDetail.Convictions(iConvictions).AlcoholMeasurementMethod
                                impPcConvictionsDetail(iConvictions).Date = msgParty.ClientDetail.Convictions(iConvictions).Date
                                impPcConvictionsDetail(iConvictions).Description = msgParty.ClientDetail.Convictions(iConvictions).Description
                                impPcConvictionsDetail(iConvictions).DrivingLicensePenaltyPoints = msgParty.ClientDetail.Convictions(iConvictions).DrivingLicensePenaltyPoints
                                impPcConvictionsDetail(iConvictions).DrivingLicensePenaltyPointsSpecified = msgParty.ClientDetail.Convictions(iConvictions).DrivingLicensePenaltyPointsSpecified
                                impPcConvictionsDetail(iConvictions).FineAmount = msgParty.ClientDetail.Convictions(iConvictions).FineAmount
                                impPcConvictionsDetail(iConvictions).FineAmountSpecified = msgParty.ClientDetail.Convictions(iConvictions).FineAmountSpecified
                                impPcConvictionsDetail(iConvictions).SentenceDescription = msgParty.ClientDetail.Convictions(iConvictions).SentenceDescription
                                impPcConvictionsDetail(iConvictions).SentenceDuration = msgParty.ClientDetail.Convictions(iConvictions).SentenceDuration
                                impPcConvictionsDetail(iConvictions).SentenceDurationSpecified = msgParty.ClientDetail.Convictions(iConvictions).SentenceDurationSpecified
                                impPcConvictionsDetail(iConvictions).SentenceDurationQualifier = msgParty.ClientDetail.Convictions(iConvictions).SentenceDurationQualifier
                                impPcConvictionsDetail(iConvictions).SentenceEffectiveDate = msgParty.ClientDetail.Convictions(iConvictions).SentenceEffectiveDate
                                impPcConvictionsDetail(iConvictions).SentenceEffectiveDateSpecified = msgParty.ClientDetail.Convictions(iConvictions).SentenceEffectiveDateSpecified
                                impPcConvictionsDetail(iConvictions).SentenceTypeCode = msgParty.ClientDetail.Convictions(iConvictions).SentenceTypeCode
                                impPcConvictionsDetail(iConvictions).StatusCode = msgParty.ClientDetail.Convictions(iConvictions).StatusCode
                                impPcConvictionsDetail(iConvictions).TypeCode = msgParty.ClientDetail.Convictions(iConvictions).TypeCode
                            Next

                            impPartyPC.ClientDetail.Convictions = impPcConvictionsDetail
                            'CommonFunctions.ToBaseImpBaseConvictionTypes(msgParty.ClientDetail.Convictions.ToList(), impPartyPC.ClientDetail.Convictions.ToList())
                        End If
                    End If


                    impParty = impPartyPC

                ElseIf AddPartyRequest.Item.GetType Is GetType(BasePartyCCType) Then

                    ' Process corporate client
                    Dim msgParty As BasePartyCCType = DirectCast(AddPartyRequest.Item, BasePartyCCType)
                    Dim impPartyCC As New BaseImplementationTypes.BasePartyCCType

                    Dim impCCClientDetail As New BaseImplementationTypes.BaseClientSharedDataType
                    Dim impCCAssociate(0) As BaseImplementationTypes.BaseAssociateType ' Vivek: differing with Tech Spec : declaring as array
                    Dim impCCLoyaltyScheme() As BaseImplementationTypes.BaseClientSharedDataTypeLoyaltyScheme = Nothing
                    Dim impCCProspectPolicies() As BaseImplementationTypes.BaseClientSharedDataTypeProspectPolicies = Nothing
                    Dim impCcConvictionsDetail() As BaseImplementationTypes.BaseConvictionType = Nothing

                    impPartyCC.AlternativeId = msgParty.AlternativeId
                    impPartyCC.BranchCode = msgParty.BranchCode
                    impPartyCC.CompanyName = Trim(msgParty.CompanyName)
                    impPartyCC.BusinessCode = msgParty.BusinessCode
                    impPartyCC.MainContact = msgParty.MainContact
                    impPartyCC.NumberOfEmployees = msgParty.NumberOfEmployees
                    impPartyCC.NumberOfOffices = msgParty.NumberOfOffices
                    impPartyCC.NumberOfOfficesSpecified = msgParty.NumberOfOfficesSpecified
                    impPartyCC.AccountExecutive = msgParty.AccountExecutive
                    impPartyCC.AccountExecutiveCode = msgParty.AccountExecutiveCode
                    impPartyCC.CompanyReg = msgParty.CompanyReg
                    impPartyCC.TradeCode = msgParty.TradeCode
                    impPartyCC.SICCode = msgParty.SICCode
                    impPartyCC.TradingSince = msgParty.TradingSince
                    impPartyCC.TradingSinceSpecified = msgParty.TradingSinceSpecified
                    impPartyCC.WageRoll = msgParty.WageRoll
                    impPartyCC.WageRollSpecified = msgParty.WageRollSpecified
                    impPartyCC.TurnoverCode = msgParty.TurnoverCode
                    impPartyCC.FinancialYear = msgParty.FinancialYear
                    impPartyCC.FinancialYearSpecified = msgParty.FinancialYearSpecified
                    impPartyCC.Salutation = msgParty.Salutation
                    impPartyCC.TPS = msgParty.TPS
                    impPartyCC.TPSSpecified = msgParty.TPSSpecified
                    impPartyCC.MPS = msgParty.MPS
                    impPartyCC.MPSSpecified = msgParty.MPSSpecified
                    impPartyCC.eMPS = msgParty.eMPS
                    impPartyCC.eMPSSpecified = msgParty.eMPSSpecified
                    impPartyCC.Source = msgParty.Source

                    If msgParty.ClientDetail IsNot Nothing Then
                        impCCClientDetail = New BaseImplementationTypes.BaseClientSharedDataType
                        impPartyCC.ClientDetail = impCCClientDetail

                        impCCClientDetail.ServiceLevelCode = msgParty.ClientDetail.ServiceLevelCode
                        impCCClientDetail.AreaCode = msgParty.ClientDetail.AreaCode
                        impCCClientDetail.LeadAgentKey = msgParty.ClientDetail.LeadAgentKey
                        impCCClientDetail.LeadAgentKeySpecified = msgParty.ClientDetail.LeadAgentKeySpecified
                        impCCClientDetail.IsProspect = msgParty.ClientDetail.IsProspect
                        impCCClientDetail.IsProspectSpecified = msgParty.ClientDetail.IsProspectSpecified
                        impCCClientDetail.IsAgent = msgParty.ClientDetail.IsAgent
                        impCCClientDetail.IsAgentSpecified = msgParty.ClientDetail.IsAgentSpecified
                        impCCClientDetail.CorrespondenceCode = msgParty.ClientDetail.CorrespondenceCode
                        impCCClientDetail.PaymentCode = msgParty.ClientDetail.PaymentCode
                        impCCClientDetail.ReminderCode = msgParty.ClientDetail.ReminderCode
                        impCCClientDetail.PaymentTermCode = msgParty.ClientDetail.PaymentTermCode
                        impCCClientDetail.RenewalStopCode = msgParty.ClientDetail.RenewalStopCode
                        impCCClientDetail.LoyaltyNumber = msgParty.ClientDetail.LoyaltyNumber
                        impCCClientDetail.SeasonalGiftCode = msgParty.ClientDetail.SeasonalGiftCode
                        impCCClientDetail.CountyCourtJudgments = msgParty.ClientDetail.CountyCourtJudgments
                        impCCClientDetail.CountyCourtJudgmentsSpecified = msgParty.ClientDetail.CountyCourtJudgmentsSpecified
                        impCCClientDetail.AgentReference = msgParty.ClientDetail.AgentReference
                        impCCClientDetail.CurrentIntermediaryKey = msgParty.ClientDetail.CurrentIntermediaryKey
                        impCCClientDetail.CurrentIntermediaryKeySpecified = msgParty.ClientDetail.CurrentIntermediaryKeySpecified
                        impCCClientDetail.StrengthCode = msgParty.ClientDetail.StrengthCode
                        impCCClientDetail.StatusCode = msgParty.ClientDetail.StatusCode
                        impCCClientDetail.PreviousInsurerKey = msgParty.ClientDetail.PreviousInsurerKey
                        impCCClientDetail.PreviousInsurerKeySpecified = msgParty.ClientDetail.PreviousInsurerKeySpecified
                        impCCClientDetail.PreviousBrokerKey = msgParty.ClientDetail.PreviousBrokerKey
                        impCCClientDetail.PreviousBrokerKeySpecified = msgParty.ClientDetail.PreviousBrokerKeySpecified

                        If (msgParty.ClientDetail.Associates) IsNot Nothing Then
                            For cntAsoc As Integer = 0 To msgParty.ClientDetail.Associates.Count - 1

                                ReDim Preserve impCCAssociate(cntAsoc)
                                impCCAssociate(cntAsoc) = New BaseImplementationTypes.BaseAssociateType
                                impCCAssociate(cntAsoc).ClientKey = msgParty.ClientDetail.Associates(cntAsoc).ClientKey
                                impCCAssociate(cntAsoc).AssociateKey = msgParty.ClientDetail.Associates(cntAsoc).AssociateKey
                                impCCAssociate(cntAsoc).RelationshipCode = msgParty.ClientDetail.Associates(cntAsoc).RelationshipCode
                                impCCAssociate(cntAsoc).RelationshipDescription = msgParty.ClientDetail.Associates(cntAsoc).RelationshipDescription
                            Next
                            impPartyCC.ClientDetail.Associates = impCCAssociate
                        End If

                        If msgParty.ClientDetail.LoyaltyScheme IsNot Nothing Then
                            For cntLS As Integer = 0 To _
                                msgParty.ClientDetail.LoyaltyScheme.Count - 1

                                ReDim Preserve impCCLoyaltyScheme(cntLS)
                                impCCLoyaltyScheme(cntLS) = New BaseImplementationTypes.BaseClientSharedDataTypeLoyaltyScheme ' Vivek: typo in Tech Spec
                                impCCLoyaltyScheme(cntLS).LoyaltySchemeKey = msgParty.ClientDetail.LoyaltyScheme(cntLS).LoyaltySchemeKey
                                impCCLoyaltyScheme(cntLS).LoyaltySchemeCode = msgParty.ClientDetail.LoyaltyScheme(cntLS).LoyaltySchemeCode
                                impCCLoyaltyScheme(cntLS).MembershipNumber = msgParty.ClientDetail.LoyaltyScheme(cntLS).MembershipNumber
                                impCCLoyaltyScheme(cntLS).OtherReference = msgParty.ClientDetail.LoyaltyScheme(cntLS).OtherReference
                                impCCLoyaltyScheme(cntLS).StartDate = msgParty.ClientDetail.LoyaltyScheme(cntLS).StartDate
                                impCCLoyaltyScheme(cntLS).EndDate = msgParty.ClientDetail.LoyaltyScheme(cntLS).EndDate
                                impCCLoyaltyScheme(cntLS).EndDateSpecified = msgParty.ClientDetail.LoyaltyScheme(cntLS).EndDateSpecified
                                impCCLoyaltyScheme(cntLS).MainMember = msgParty.ClientDetail.LoyaltyScheme(cntLS).MainMember
                                impCCLoyaltyScheme(cntLS).Active = msgParty.ClientDetail.LoyaltyScheme(cntLS).Active
                                impCCLoyaltyScheme(cntLS).ActiveSpecified = msgParty.ClientDetail.LoyaltyScheme(cntLS).ActiveSpecified
                            Next
                            impPartyCC.ClientDetail.LoyaltyScheme = impCCLoyaltyScheme
                        End If

                        If msgParty.ClientDetail.ProspectPolicies IsNot Nothing Then
                            For cntPP As Integer = 0 To _
                                msgParty.ClientDetail.ProspectPolicies.Count - 1
                                ReDim Preserve impCCProspectPolicies(cntPP)
                                impCCProspectPolicies(cntPP) = New BaseImplementationTypes.BaseClientSharedDataTypeProspectPolicies
                                impCCProspectPolicies(cntPP).ProspectPolicyKey = msgParty.ClientDetail.ProspectPolicies(cntPP).ProspectPolicyKey
                                impCCProspectPolicies(cntPP).ProspectTypeCode = msgParty.ClientDetail.ProspectPolicies(cntPP).ProspectTypeCode
                                impCCProspectPolicies(cntPP).RenewalDate = msgParty.ClientDetail.ProspectPolicies(cntPP).RenewalDate
                                impCCProspectPolicies(cntPP).RenewalDateSpecified = msgParty.ClientDetail.ProspectPolicies(cntPP).RenewalDateSpecified
                                impCCProspectPolicies(cntPP).TimesQuoted = msgParty.ClientDetail.ProspectPolicies(cntPP).TimesQuoted
                                impCCProspectPolicies(cntPP).TimesQuotedSpecified = msgParty.ClientDetail.ProspectPolicies(cntPP).TimesQuotedSpecified
                                impCCProspectPolicies(cntPP).TargetPremium = msgParty.ClientDetail.ProspectPolicies(cntPP).TargetPremium
                                impCCProspectPolicies(cntPP).TargetPremiumSpecified = msgParty.ClientDetail.ProspectPolicies(cntPP).TargetPremiumSpecified
                            Next
                            impPartyCC.ClientDetail.ProspectPolicies = impCCProspectPolicies
                        End If

                        If msgParty.ClientDetail.Convictions IsNot Nothing Then
                            For iConvictions As Integer = 0 To msgParty.ClientDetail.Convictions.Count - 1
                                ReDim Preserve impCcConvictionsDetail(iConvictions)

                                impCcConvictionsDetail(iConvictions) = New BaseImplementationTypes.BaseConvictionType
                                impCcConvictionsDetail(iConvictions).AlcoholLevel = msgParty.ClientDetail.Convictions(iConvictions).AlcoholLevel
                                impCcConvictionsDetail(iConvictions).AlcoholLevelSpecified = msgParty.ClientDetail.Convictions(iConvictions).AlcoholLevelSpecified
                                impCcConvictionsDetail(iConvictions).AlcoholMeasurementMethod = msgParty.ClientDetail.Convictions(iConvictions).AlcoholMeasurementMethod
                                impCcConvictionsDetail(iConvictions).Date = msgParty.ClientDetail.Convictions(iConvictions).Date
                                impCcConvictionsDetail(iConvictions).Description = msgParty.ClientDetail.Convictions(iConvictions).Description
                                impCcConvictionsDetail(iConvictions).DrivingLicensePenaltyPoints = msgParty.ClientDetail.Convictions(iConvictions).DrivingLicensePenaltyPoints
                                impCcConvictionsDetail(iConvictions).DrivingLicensePenaltyPointsSpecified = msgParty.ClientDetail.Convictions(iConvictions).DrivingLicensePenaltyPointsSpecified
                                impCcConvictionsDetail(iConvictions).FineAmount = msgParty.ClientDetail.Convictions(iConvictions).FineAmount
                                impCcConvictionsDetail(iConvictions).FineAmountSpecified = msgParty.ClientDetail.Convictions(iConvictions).FineAmountSpecified
                                impCcConvictionsDetail(iConvictions).SentenceDescription = msgParty.ClientDetail.Convictions(iConvictions).SentenceDescription
                                impCcConvictionsDetail(iConvictions).SentenceDuration = msgParty.ClientDetail.Convictions(iConvictions).SentenceDuration
                                impCcConvictionsDetail(iConvictions).SentenceDurationSpecified = msgParty.ClientDetail.Convictions(iConvictions).SentenceDurationSpecified
                                impCcConvictionsDetail(iConvictions).SentenceDurationQualifier = msgParty.ClientDetail.Convictions(iConvictions).SentenceDurationQualifier
                                impCcConvictionsDetail(iConvictions).SentenceEffectiveDate = msgParty.ClientDetail.Convictions(iConvictions).SentenceEffectiveDate
                                impCcConvictionsDetail(iConvictions).SentenceEffectiveDateSpecified = msgParty.ClientDetail.Convictions(iConvictions).SentenceEffectiveDateSpecified
                                impCcConvictionsDetail(iConvictions).SentenceTypeCode = msgParty.ClientDetail.Convictions(iConvictions).SentenceTypeCode
                                impCcConvictionsDetail(iConvictions).StatusCode = msgParty.ClientDetail.Convictions(iConvictions).StatusCode
                                impCcConvictionsDetail(iConvictions).TypeCode = msgParty.ClientDetail.Convictions(iConvictions).TypeCode
                            Next

                            impPartyCC.ClientDetail.Convictions = impCcConvictionsDetail
                            'CommonFunctions.ToBaseImpBaseConvictionTypes(msgParty.ClientDetail.Convictions.ToList(), impPartyCC.ClientDetail.Convictions.ToList())

                        End If

                    End If


                    impParty = impPartyCC

                ElseIf AddPartyRequest.Item.GetType Is GetType(BasePartyOTHERType) Then

                    Dim objPartyOther As BasePartyOTHERType = DirectCast(AddPartyRequest.Item, BasePartyOTHERType)
                    Dim impPartyOther As New BaseImplementationTypes.BasePartyOTHERType
                    Dim impOTConvictionsDetail As BaseImplementationTypes.BaseConvictionType() = Nothing

                    If objPartyOther.Convictions IsNot Nothing AndAlso objPartyOther.Convictions.Count > 0 Then
                        For iConvictions As Integer = 0 To objPartyOther.Convictions.Count - 1
                            ReDim Preserve impOTConvictionsDetail(iConvictions)

                            impOTConvictionsDetail(iConvictions) = New BaseImplementationTypes.BaseConvictionType
                            impOTConvictionsDetail(iConvictions).AlcoholLevel = objPartyOther.Convictions(iConvictions).AlcoholLevel
                            impOTConvictionsDetail(iConvictions).AlcoholLevelSpecified = objPartyOther.Convictions(iConvictions).AlcoholLevelSpecified
                            impOTConvictionsDetail(iConvictions).AlcoholMeasurementMethod = objPartyOther.Convictions(iConvictions).AlcoholMeasurementMethod
                            impOTConvictionsDetail(iConvictions).Date = objPartyOther.Convictions(iConvictions).Date
                            impOTConvictionsDetail(iConvictions).Description = objPartyOther.Convictions(iConvictions).Description
                            impOTConvictionsDetail(iConvictions).DrivingLicensePenaltyPoints = objPartyOther.Convictions(iConvictions).DrivingLicensePenaltyPoints
                            impOTConvictionsDetail(iConvictions).DrivingLicensePenaltyPointsSpecified = objPartyOther.Convictions(iConvictions).DrivingLicensePenaltyPointsSpecified
                            impOTConvictionsDetail(iConvictions).FineAmount = objPartyOther.Convictions(iConvictions).FineAmount
                            impOTConvictionsDetail(iConvictions).FineAmountSpecified = objPartyOther.Convictions(iConvictions).FineAmountSpecified
                            impOTConvictionsDetail(iConvictions).SentenceDescription = objPartyOther.Convictions(iConvictions).SentenceDescription
                            impOTConvictionsDetail(iConvictions).SentenceDuration = objPartyOther.Convictions(iConvictions).SentenceDuration
                            impOTConvictionsDetail(iConvictions).SentenceDurationSpecified = objPartyOther.Convictions(iConvictions).SentenceDurationSpecified
                            impOTConvictionsDetail(iConvictions).SentenceDurationQualifier = objPartyOther.Convictions(iConvictions).SentenceDurationQualifier
                            impOTConvictionsDetail(iConvictions).SentenceEffectiveDate = objPartyOther.Convictions(iConvictions).SentenceEffectiveDate
                            impOTConvictionsDetail(iConvictions).SentenceEffectiveDateSpecified = objPartyOther.Convictions(iConvictions).SentenceEffectiveDateSpecified
                            impOTConvictionsDetail(iConvictions).SentenceTypeCode = objPartyOther.Convictions(iConvictions).SentenceTypeCode
                            impOTConvictionsDetail(iConvictions).StatusCode = objPartyOther.Convictions(iConvictions).StatusCode
                            impOTConvictionsDetail(iConvictions).TypeCode = objPartyOther.Convictions(iConvictions).TypeCode
                        Next

                        impPartyOther.Convictions = impOTConvictionsDetail
                        'CommonFunctions.ToBaseImpBaseConvictionTypes(objPartyOther.Convictions.ToList(), impPartyOther.Convictions.ToList())
                    End If

                    Dim impPartyAccident As BaseImplementationTypes.BasePartyOTHERTypeAccident() = Nothing
                    If objPartyOther.Accident IsNot Nothing Then
                        For cntAcci As Integer = 0 To objPartyOther.Accident.Count - 1

                            ReDim Preserve impPartyAccident(cntAcci)
                            impPartyAccident(cntAcci) = New BaseImplementationTypes.BasePartyOTHERTypeAccident
                            impPartyAccident(cntAcci).Date = objPartyOther.Accident(cntAcci).Date
                            impPartyAccident(cntAcci).Description = objPartyOther.Accident(cntAcci).Description
                            impPartyAccident(cntAcci).IsAtFault = objPartyOther.Accident(cntAcci).IsAtFault
                        Next
                        impPartyOther.Accident = impPartyAccident
                    End If
                    Dim impSuppBusiness As BaseImplementationTypes.BasePartyOTHERTypeSupplierBusiness() = Nothing
                    If objPartyOther.SupplierBusiness IsNot Nothing Then
                        For cntBusiness As Integer = 0 To objPartyOther.SupplierBusiness.Count - 1

                            ReDim Preserve impSuppBusiness(cntBusiness)
                            impSuppBusiness(cntBusiness) = New BaseImplementationTypes.BasePartyOTHERTypeSupplierBusiness
                            impSuppBusiness(cntBusiness).BusinessCode = objPartyOther.SupplierBusiness(cntBusiness).BusinessCode
                            impSuppBusiness(cntBusiness).SpecialityCode = objPartyOther.SupplierBusiness(cntBusiness).SpecialityCode

                        Next
                        impPartyOther.SupplierBusiness = impSuppBusiness
                    End If
                    Dim impBranches As BaseImplementationTypes.BasePartyOTHERTypeBranch() = Nothing
                    If objPartyOther.Branches IsNot Nothing Then
                        For cntBranch As Integer = 0 To objPartyOther.Branches.Count - 1

                            ReDim Preserve impBranches(cntBranch)
                            impBranches(cntBranch) = New BaseImplementationTypes.BasePartyOTHERTypeBranch
                            impBranches(cntBranch).BranchId = objPartyOther.Branches(cntBranch).BranchId
                            impBranches(cntBranch).Description = objPartyOther.Branches(cntBranch).Description
                        Next
                        impPartyOther.Branches = impBranches
                    End If

                    impPartyOther.ActiveIndicator = objPartyOther.ActiveIndicator
                    impPartyOther.AfterHoursIndicator = objPartyOther.AfterHoursIndicator
                    impPartyOther.Code = objPartyOther.Code
                    impPartyOther.IsTPASettleDirectly = objPartyOther.ISTPASettleDirectly
                    impPartyOther.DateOfBirth = objPartyOther.DateOfBirth
                    impPartyOther.DriverStatusCode = objPartyOther.DriverStatusCode
                    impPartyOther.Gender = objPartyOther.Gender

                    impPartyOther.LicenseNumber = objPartyOther.LicenseNumber
                    impPartyOther.LicenseTypeCode = objPartyOther.LicenseTypeCode
                    impPartyOther.Name = objPartyOther.Name

                    impPartyOther.PriorityIndicator = objPartyOther.PriorityIndicator
                    impPartyOther.RegNumber = objPartyOther.RegNumber
                    impPartyOther.TypeCode = objPartyOther.TypeCode

                    impParty = DirectCast(impPartyOther, BaseImplementationTypes.BasePartyType)

                    oImpRequest.Party = impParty

                End If

                ' Common party fields
                impParty.TPIntroducer = AddPartyRequest.Item.TPIntroducer
                impParty.TPUserCode = AddPartyRequest.Item.TPUserCode
                impParty.BranchCode = AddPartyRequest.Item.BranchCode
                impParty.XMLDataset = AddPartyRequest.Item.XMLDataset

                impParty.Currency = AddPartyRequest.Item.Currency
                impParty.SubBranchCode = AddPartyRequest.Item.SubBranchCode
                impParty.DomiciledForTaxSpecified = AddPartyRequest.Item.DomiciledForTaxSpecified
                impParty.DomiciledForTax = AddPartyRequest.Item.DomiciledForTax
                impParty.TaxExemptSpecified = AddPartyRequest.Item.TaxExemptSpecified
                impParty.TaxExempt = AddPartyRequest.Item.TaxExempt
                impParty.TaxNumber = AddPartyRequest.Item.TaxNumber
                impParty.TaxPercentageSpecified = AddPartyRequest.Item.TaxPercentageSpecified
                impParty.TaxPercentage = AddPartyRequest.Item.TaxPercentage
                impParty.FileCode = AddPartyRequest.Item.FileCode


                If (AddPartyRequest.Item.Addresses) IsNot Nothing Then
                    ReDim impParty.Addresses(AddPartyRequest.Item.Addresses.Count - 1)
                    For iCnt As Integer = 0 To AddPartyRequest.Item.Addresses.Count - 1
                        msgAddress = AddPartyRequest.Item.Addresses(iCnt)
                        impAddress = New BaseImplementationTypes.BaseAddressWithContactsType

                        impAddress.AddressLine1 = msgAddress.AddressLine1
                        impAddress.AddressLine2 = msgAddress.AddressLine2
                        impAddress.AddressLine3 = msgAddress.AddressLine3
                        impAddress.AddressLine4 = msgAddress.AddressLine4
                        impAddress.AddressLine5 = msgAddress.AddressLine5
                        impAddress.AddressLine6 = msgAddress.AddressLine6
                        impAddress.AddressLine7 = msgAddress.AddressLine7
                        impAddress.AddressLine8 = msgAddress.AddressLine8
                        impAddress.AddressLine9 = msgAddress.AddressLine9
                        impAddress.AddressLine10 = msgAddress.AddressLine10


                        impAddress.AddressTypeCode = CType([Enum].ToObject(GetType(AddressTypeType), msgAddress.AddressTypeCode), BaseImplementationTypes.AddressTypeType)

                        impAddress.CountryCode = msgAddress.CountryCode
                        impAddress.PostCode = msgAddress.PostCode
                        If Not (AddPartyRequest.Item.Addresses(iCnt%).Contacts) Is Nothing Then
                            ReDim impAddress.Contacts((AddPartyRequest.Item.Addresses(iCnt%).Contacts.Count))
                            Dim oContact As New BaseImplementationTypes.BaseContactType

                            For icntContacts As Integer = 0 To AddPartyRequest.Item.Addresses(iCnt%).Contacts.Count - 1
                                Dim AssignContact As BaseContactType = AddPartyRequest.Item.Addresses(iCnt%).Contacts(icntContacts)
                                oContact.AreaCode = Cast.ToString(AssignContact.AreaCode)
                                oContact.Description = AssignContact.Description

                                oContact.ContactDetail = New BaseImplementationTypes.BaseContactDetailType

                                oContact.ContactDetail.ItemElementName = CType([Enum].ToObject(GetType(ItemChoiceType), AssignContact.ContactDetail.ItemElementName), BaseImplementationTypes.ItemChoiceType)

                                oContact.ContactDetail.Item = AssignContact.ContactDetail.Item
                                oContact.ContactTypeCode = CType([Enum].ToObject(GetType(ContactTypeType), AssignContact.ContactTypeCode), BaseImplementationTypes.ContactTypeType)

                                impAddress.Contacts(icntContacts) = New BaseImplementationTypes.BaseContactType
                                impAddress.Contacts(icntContacts) = oContact
                            Next
                        End If
                        impParty.Addresses(iCnt) = impAddress
                    Next iCnt%
                End If

                ' Process the Contact structure

                If AddPartyRequest.Item.Contacts IsNot Nothing AndAlso AddPartyRequest.Item.Contacts.Count > 0 Then
                    Dim oImpartyContact(AddPartyRequest.Item.Contacts.Count) As BaseImplementationTypes.BaseContactType
                    For iCnt As Integer = 0 To AddPartyRequest.Item.Contacts.Count - 1
                        If AddPartyRequest.Item.Contacts(iCnt%) IsNot Nothing Then
                            msgContact = AddPartyRequest.Item.Contacts(iCnt%)
                            impContact = New BaseImplementationTypes.BaseContactType
                            impContact.AreaCode = msgContact.AreaCode
                            impContact.Description = msgContact.Description
                            impContact.Extension = msgContact.Extension
                            impContact.ContactDetail = New BaseImplementationTypes.BaseContactDetailType
                            impContact.ContactDetail.Item = msgContact.ContactDetail.Item
                            If msgContact.ContactTypeCode <> ContactTypeType.OTHER Then
                                impContact.ContactTypeCode = CType([Enum].ToObject(GetType(ContactTypeType), msgContact.ContactTypeCode), BaseImplementationTypes.ContactTypeType)
                            Else
                                impContact.ContactTypeCode = BaseImplementationTypes.ContactTypeType.OTHER
                                impContact.OtherContactTypeCode = msgContact.OtherContactTypeCode
                            End If
                            ReDim Preserve oImpartyContact(iCnt)
                            oImpartyContact(iCnt) = impContact
                        End If
                    Next iCnt

                    impParty.Contacts = oImpartyContact

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
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(AddPartyRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(AddPartyRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' To Get party detail for given party id
    ''' </summary>
    ''' <param name="GetPartyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetParty(ByVal GetPartyRequest As GetPartyRequestType) As GetPartyResponseType Implements IPurePartyService.GetParty, IPurePolicyService.GetParty, IPureClaimService.GetParty

        Try
            Dim sUserName As String = GetPartyRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            Dim oResponse As New GetPartyResponseType

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGPty", iUserId)
            CommonFunctions.CheckSecurityToken(GetPartyRequest.WCFSecurityToken)
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetPartyRequest.BranchCode)
            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetPartyRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetPartyResponseType = Nothing



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
                Dim oParty As New BasePartyType

                If oImpResponse.Item IsNot Nothing Then

                    If oImpResponse.Item.GetType Is GetType(SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyPCType) Then
                        oParty = New BasePartyPCType
                    ElseIf oImpResponse.Item.GetType Is GetType(SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyCCType) Then
                        oParty = New BasePartyCCType
                    ElseIf oImpResponse.Item.GetType Is GetType(SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyOTHERType) Then
                        oParty = New BasePartyOTHERType
                    End If

                    ' get the addresses
                    If oImpResponse.Item.Addresses IsNot Nothing AndAlso oImpResponse.Item.Addresses.Count > 0 Then

                        oParty.Addresses = oImpResponse.Item.Addresses.ToList().ConvertAll(
                                New Converter(Of BaseImplementationTypes.BaseAddressType, BaseAddressWithContactsType) _
                                (AddressOf CommonFunctions.ToServiceBaseAddressWithContactsType))

                    End If

                    ' get the contacts
                    If oImpResponse.Item.Contacts IsNot Nothing AndAlso oImpResponse.Item.Contacts.Count > 0 Then
                        oParty.Contacts = oImpResponse.Item.Contacts.ToList().ConvertAll(
                                New Converter(Of BaseImplementationTypes.BaseContactType, BaseContactType) _
                                (AddressOf CommonFunctions.ToServiceContactType))

                    End If

                    oParty.BranchCode = oImpResponse.Item.BranchCode
                    oParty.SubBranchCode = oImpResponse.Item.SubBranchCode
                    oParty.TPIntroducer = oImpResponse.Item.TPIntroducer
                    oParty.TPUserCode = oImpResponse.Item.TPUserCode
                    oParty.XMLDataset = oImpResponse.Item.XMLDataset
                    oResponse.PartyTimestamp = oImpResponse.PartyTimestamp
                    oResponse.NoofPolicies = oImpResponse.NoofPolicies
                    oResponse.NoofOpenClaims = oImpResponse.NoofOpenClaims
                    oResponse.NoofClosedClaims = oImpResponse.NoofClosedClaims
                    oResponse.Item = oParty

                    If oImpResponse.Item.GetType Is GetType(SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyPCType) Then
                        Dim oResponseParty As New BasePartyPCType
                        Dim oPartyPC As New SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyPCType
                        oPartyPC = DirectCast(oImpResponse.Item, SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyPCType)
                        oResponseParty = DirectCast(oResponse.Item, BasePartyPCType)
                        oResponseParty.AlternativeId = oPartyPC.AlternativeId
                        oResponseParty.DateOfBirth = oPartyPC.DateOfBirth
                        oResponseParty.DateOfBirthSpecified = oPartyPC.DateOfBirthSpecified
                        oResponseParty.EmployersBusinessCode = oPartyPC.EmployersBusinessCode
                        oResponseParty.EmploymentStatusCode = CType([Enum].ToObject(GetType(BaseImplementationTypes.EmploymentStatusCodeType), oPartyPC.EmploymentStatusCode), EmploymentStatusCodeType)
                        oResponseParty.EmploymentStatusCodeSpecified = oPartyPC.EmploymentStatusCodeSpecified
                        oResponseParty.Forename = oPartyPC.Forename
                        oResponseParty.GenderCode = oPartyPC.GenderCode
                        oResponseParty.Initials = oPartyPC.Initials
                        oResponseParty.MaritalStatusCode = CType([Enum].ToObject(GetType(BaseImplementationTypes.MaritalStatusCodeType), oPartyPC.MaritalStatusCode), MaritalStatusCodeType)
                        oResponseParty.MaritalStatusCodeSpecified = oPartyPC.MaritalStatusCodeSpecified
                        oResponseParty.OccupationCode = oPartyPC.OccupationCode
                        oResponseParty.Surname = oPartyPC.Surname
                        oResponseParty.Title = oPartyPC.Title
                        oResponseParty.FileCode = oPartyPC.FileCode
                        oResponseParty.AccountExecutive = oPartyPC.AccountExecutive
                        oResponseParty.AccountExecutiveCode = oPartyPC.AccountExecutiveCode
                        oResponseParty.TradingName = oPartyPC.TradingName
                        oResponseParty.Currency = oPartyPC.Currency
                        oResponseParty.SecOccupationCode = oPartyPC.SecOccupationCode
                        oResponseParty.SecEmployersBusinessCode = oPartyPC.SecEmployersBusinessCode
                        oResponseParty.SecEmploymentStatusCode = CType([Enum].ToObject(GetType(BaseImplementationTypes.EmploymentStatusCodeType), oPartyPC.SecEmploymentStatusCode), EmploymentStatusCodeType)
                        oResponseParty.SecEmploymentStatusCodeSpecified = oPartyPC.SecEmploymentStatusCodeSpecified
                        oResponseParty.NationalityCode = oPartyPC.NationalityCode
                        oResponseParty.AccommodationCode = oPartyPC.AccommodationCode
                        oResponseParty.Salutation = oPartyPC.Salutation
                        oResponseParty.TPS = oPartyPC.TPS
                        oResponseParty.TPSSpecified = oPartyPC.TPSSpecified
                        oResponseParty.MPS = oPartyPC.MPS
                        oResponseParty.MPSSpecified = oPartyPC.MPSSpecified
                        oResponseParty.eMPS = oPartyPC.eMPS
                        oResponseParty.eMPSSpecified = oPartyPC.eMPSSpecified
                        oResponseParty.Source = oPartyPC.Source
                        oResponseParty.PetOwner = oPartyPC.PetOwner
                        oResponseParty.PetOwnerSpecified = oPartyPC.PetOwnerSpecified

                        oResponseParty.TaxNumber = oPartyPC.TaxNumber
                        oResponseParty.DomiciledForTax = oPartyPC.DomiciledForTax
                        oResponseParty.DomiciledForTaxSpecified = oPartyPC.DomiciledForTaxSpecified
                        oResponseParty.TaxExempt = oPartyPC.TaxExempt
                        oResponseParty.TaxExemptSpecified = oPartyPC.TaxExemptSpecified
                        oResponseParty.TaxPercentage = oPartyPC.TaxPercentage
                        oResponseParty.TaxPercentageSpecified = oPartyPC.TaxPercentageSpecified

                        If oPartyPC.Lifestyle IsNot Nothing AndAlso oPartyPC.Lifestyle.Count > 0 Then
                            oResponseParty.Lifestyle = oPartyPC.Lifestyle.ToList().ConvertAll(
                                New Converter(Of BaseImplementationTypes.BasePartyPCTypeLifestyle, BasePartyPCTypeLifestyle) _
                                (AddressOf CommonFunctions.ToServiceBaseClientSharedDataTypeLifeStyle))
                        End If

                        If oPartyPC.ClientDetail IsNot Nothing Then
                            Dim oResponseClientDetail As New BaseClientSharedDataType
                            oResponseParty.ClientDetail = oResponseClientDetail
                            Dim oClientDetail As SiriusFS.SAM.Structure.BaseImplementationTypes.BaseClientSharedDataType = oPartyPC.ClientDetail
                            oResponseClientDetail.ShortName = oClientDetail.ShortName
                            oResponseClientDetail.ResolvedName = oClientDetail.ResolvedName
                            oResponseClientDetail.ServiceLevelCode = oClientDetail.ServiceLevelCode
                            oResponseClientDetail.AreaCode = oClientDetail.AreaCode
                            oResponseClientDetail.LeadAgentKey = oClientDetail.LeadAgentKey
                            oResponseClientDetail.LeadAgentKeySpecified = oClientDetail.LeadAgentKeySpecified
                            oResponseClientDetail.IsProspect = oClientDetail.IsProspect
                            oResponseClientDetail.IsProspectSpecified = oClientDetail.IsProspectSpecified
                            oResponseClientDetail.IsAgent = oClientDetail.IsAgent
                            oResponseClientDetail.IsAgentSpecified = oClientDetail.IsAgentSpecified
                            oResponseClientDetail.CorrespondenceCode = oClientDetail.CorrespondenceCode
                            oResponseClientDetail.PaymentCode = oClientDetail.PaymentCode
                            oResponseClientDetail.ReminderCode = oClientDetail.ReminderCode
                            oResponseClientDetail.PaymentTermCode = oClientDetail.PaymentTermCode
                            oResponseClientDetail.RenewalStopCode = oClientDetail.RenewalStopCode
                            oResponseClientDetail.LoyaltyNumber = oClientDetail.LoyaltyNumber
                            oResponseClientDetail.SeasonalGiftCode = oClientDetail.SeasonalGiftCode
                            oResponseClientDetail.CountyCourtJudgments = oClientDetail.CountyCourtJudgments
                            oResponseClientDetail.CountyCourtJudgmentsSpecified = oClientDetail.CountyCourtJudgmentsSpecified
                            oResponseClientDetail.AgentReference = oClientDetail.AgentReference
                            oResponseClientDetail.CurrentIntermediaryKey = oClientDetail.CurrentIntermediaryKey
                            oResponseClientDetail.CurrentIntermediaryKeySpecified = oClientDetail.CurrentIntermediaryKeySpecified
                            oResponseClientDetail.StrengthCode = oClientDetail.StrengthCode
                            oResponseClientDetail.StatusCode = oClientDetail.StatusCode
                            oResponseClientDetail.PreviousInsurerKey = oClientDetail.PreviousInsurerKey
                            oResponseClientDetail.PreviousInsurerKeySpecified = oClientDetail.PreviousInsurerKeySpecified
                            oResponseClientDetail.PreviousBrokerKey = oClientDetail.PreviousBrokerKey
                            oResponseClientDetail.PreviousBrokerKeySpecified = oClientDetail.PreviousBrokerKeySpecified
                            oResponseClientDetail.CurrentIntermediaryName = oClientDetail.CurrentIntermediaryName
                            oResponseClientDetail.PreviousInsurerCode = oClientDetail.PreviousInsurerCode
                            oResponseClientDetail.PreviousInsurerName = oClientDetail.PreviousInsurerName
                            oResponseClientDetail.PreviousBrokerCode = oClientDetail.PreviousBrokerCode
                            oResponseClientDetail.PreviousBrokerName = oClientDetail.PreviousBrokerName
                            oResponseClientDetail.LeadAgentCode = oClientDetail.LeadAgentCode
                            oResponseClientDetail.LeadAgentName = oClientDetail.LeadAgentName
                            oResponseClientDetail.AccountBalance = oClientDetail.AccountBalance
                            oResponseClientDetail.YearToDateTurnover = oClientDetail.YearToDateTurnover
                            oResponseClientDetail.LastYearTurnover = oClientDetail.LastYearTurnover
                            oResponseClientDetail.AccountBalanceSpecified = oClientDetail.AccountBalanceSpecified
                            oResponseClientDetail.YearToDateTurnoverSpecified = oClientDetail.YearToDateTurnoverSpecified
                            oResponseClientDetail.LastYearTurnoverSpecified = oClientDetail.LastYearTurnoverSpecified

                            If oClientDetail.Associates IsNot Nothing AndAlso oClientDetail.Associates.Count > 0 Then

                                oResponseClientDetail.Associates = oClientDetail.Associates.ToList().ConvertAll(
                                    New Converter(Of BaseImplementationTypes.BaseAssociateType, BaseAssociateType) _
                                    (AddressOf CommonFunctions.ToServiceAssociateType))

                            End If

                            If oClientDetail.Convictions IsNot Nothing AndAlso oClientDetail.Convictions.Count > 0 Then

                                oResponseClientDetail.Convictions = oClientDetail.Convictions.ToList().ConvertAll(
                                    New Converter(Of BaseImplementationTypes.BaseConvictionType, BaseConvictionType) _
                                    (AddressOf CommonFunctions.ToServiceConvictionType))

                            End If

                            If oClientDetail.LoyaltyScheme IsNot Nothing AndAlso oClientDetail.LoyaltyScheme.Count > 0 Then

                                oResponseClientDetail.LoyaltyScheme = oClientDetail.LoyaltyScheme.ToList().ConvertAll(
                                New Converter(Of BaseImplementationTypes.BaseClientSharedDataTypeLoyaltyScheme, BaseClientSharedDataTypeLoyaltyScheme) _
                                (AddressOf CommonFunctions.ToServiceLoyaltyScheme))

                            End If

                            If oClientDetail.ProspectPolicies IsNot Nothing AndAlso oClientDetail.ProspectPolicies.Count > 0 Then

                                oResponseClientDetail.ProspectPolicies = oClientDetail.ProspectPolicies.ToList().ConvertAll(
                                    New Converter(Of BaseImplementationTypes.BaseClientSharedDataTypeProspectPolicies, BaseClientSharedDataTypeProspectPolicies) _
                                    (AddressOf CommonFunctions.ToServiceProspectPolicies))

                            End If
                        End If

                        oResponse.Item = oResponseParty
                    ElseIf oImpResponse.Item.GetType Is GetType(SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyCCType) Then
                        Dim oResponseParty As New BasePartyCCType
                        Dim oPartyCC As New SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyCCType
                        oPartyCC = DirectCast(oImpResponse.Item, SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyCCType)
                        oResponseParty = DirectCast(oResponse.Item, BasePartyCCType)

                        oResponseParty.AlternativeId = oPartyCC.AlternativeId

                        oResponseParty.BusinessCode = oPartyCC.BusinessCode
                        oResponseParty.CompanyName = oPartyCC.CompanyName
                        oResponseParty.MainContact = oPartyCC.MainContact
                        oResponseParty.FileCode = oPartyCC.FileCode
                        oResponseParty.AccountExecutive = oPartyCC.AccountExecutive
                        oResponseParty.AccountExecutiveCode = oPartyCC.AccountExecutiveCode
                        oResponseParty.NumberOfEmployees = oPartyCC.NumberOfEmployees
                        oResponseParty.NumberOfOffices = oPartyCC.NumberOfOffices
                        oResponseParty.NumberOfOfficesSpecified = oPartyCC.NumberOfOfficesSpecified
                        oResponseParty.CompanyReg = oPartyCC.CompanyReg
                        oResponseParty.TradeCode = oPartyCC.TradeCode
                        oResponseParty.SICCode = oPartyCC.SICCode
                        oResponseParty.TradingSince = oPartyCC.TradingSince
                        oResponseParty.TradingSinceSpecified = oPartyCC.TradingSinceSpecified
                        oResponseParty.WageRoll = oPartyCC.WageRoll
                        oResponseParty.WageRollSpecified = oPartyCC.WageRollSpecified
                        oResponseParty.TurnoverCode = oPartyCC.TurnoverCode
                        oResponseParty.FinancialYear = oPartyCC.FinancialYear
                        oResponseParty.FinancialYearSpecified = oPartyCC.FinancialYearSpecified
                        oResponseParty.Salutation = oPartyCC.Salutation
                        oResponseParty.TPS = oPartyCC.TPS
                        oResponseParty.TPSSpecified = oPartyCC.TPSSpecified
                        oResponseParty.MPS = oPartyCC.MPS
                        oResponseParty.MPSSpecified = oPartyCC.MPSSpecified
                        oResponseParty.eMPS = oPartyCC.eMPS
                        oResponseParty.eMPSSpecified = oPartyCC.eMPSSpecified
                        oResponseParty.Source = oPartyCC.Source
                        oResponseParty.Currency = oPartyCC.Currency
                        oResponseParty.TaxNumber = oPartyCC.TaxNumber
                        oResponseParty.DomiciledForTax = oPartyCC.DomiciledForTax
                        oResponseParty.DomiciledForTaxSpecified = oPartyCC.DomiciledForTaxSpecified
                        oResponseParty.TaxExempt = oPartyCC.TaxExempt
                        oResponseParty.TaxExemptSpecified = oPartyCC.TaxExemptSpecified
                        oResponseParty.TaxPercentage = oPartyCC.TaxPercentage
                        oResponseParty.TaxPercentageSpecified = oPartyCC.TaxPercentageSpecified

                        If oPartyCC.ClientDetail IsNot Nothing Then
                            Dim oResponseClientDetail As New BaseClientSharedDataType
                            oResponseParty.ClientDetail = oResponseClientDetail
                            Dim oClientDetail As SiriusFS.SAM.Structure.BaseImplementationTypes.BaseClientSharedDataType = oPartyCC.ClientDetail
                            oResponseClientDetail.ShortName = oClientDetail.ShortName
                            oResponseClientDetail.ResolvedName = oClientDetail.ResolvedName
                            oResponseClientDetail.ServiceLevelCode = oClientDetail.ServiceLevelCode
                            oResponseClientDetail.AreaCode = oClientDetail.AreaCode
                            oResponseClientDetail.LeadAgentKey = oClientDetail.LeadAgentKey
                            oResponseClientDetail.LeadAgentKeySpecified = oClientDetail.LeadAgentKeySpecified
                            oResponseClientDetail.IsProspect = oClientDetail.IsProspect
                            oResponseClientDetail.IsProspectSpecified = oClientDetail.IsProspectSpecified
                            oResponseClientDetail.IsAgent = oClientDetail.IsAgent
                            oResponseClientDetail.IsAgentSpecified = oClientDetail.IsAgentSpecified
                            oResponseClientDetail.CorrespondenceCode = oClientDetail.CorrespondenceCode
                            oResponseClientDetail.PaymentCode = oClientDetail.PaymentCode
                            oResponseClientDetail.ReminderCode = oClientDetail.ReminderCode
                            oResponseClientDetail.PaymentTermCode = oClientDetail.PaymentTermCode
                            oResponseClientDetail.RenewalStopCode = oClientDetail.RenewalStopCode
                            oResponseClientDetail.LoyaltyNumber = oClientDetail.LoyaltyNumber
                            oResponseClientDetail.SeasonalGiftCode = oClientDetail.SeasonalGiftCode
                            oResponseClientDetail.CountyCourtJudgments = oClientDetail.CountyCourtJudgments
                            oResponseClientDetail.CountyCourtJudgmentsSpecified = oClientDetail.CountyCourtJudgmentsSpecified
                            oResponseClientDetail.AgentReference = oClientDetail.AgentReference
                            oResponseClientDetail.CurrentIntermediaryKey = oClientDetail.CurrentIntermediaryKey
                            oResponseClientDetail.CurrentIntermediaryKeySpecified = oClientDetail.CurrentIntermediaryKeySpecified
                            oResponseClientDetail.StrengthCode = oClientDetail.StrengthCode
                            oResponseClientDetail.StatusCode = oClientDetail.StatusCode
                            oResponseClientDetail.PreviousInsurerKey = oClientDetail.PreviousInsurerKey
                            oResponseClientDetail.PreviousInsurerKeySpecified = oClientDetail.PreviousInsurerKeySpecified
                            oResponseClientDetail.PreviousBrokerKey = oClientDetail.PreviousBrokerKey
                            oResponseClientDetail.PreviousBrokerKeySpecified = oClientDetail.PreviousBrokerKeySpecified
                            oResponseClientDetail.CurrentIntermediaryName = oClientDetail.CurrentIntermediaryName
                            oResponseClientDetail.PreviousInsurerCode = oClientDetail.PreviousInsurerCode
                            oResponseClientDetail.PreviousInsurerName = oClientDetail.PreviousInsurerName
                            oResponseClientDetail.PreviousBrokerCode = oClientDetail.PreviousBrokerCode
                            oResponseClientDetail.PreviousBrokerName = oClientDetail.PreviousBrokerName
                            oResponseClientDetail.LeadAgentCode = oClientDetail.LeadAgentCode
                            oResponseClientDetail.LeadAgentName = oClientDetail.LeadAgentName
                            oResponseClientDetail.AccountBalance = oClientDetail.AccountBalance
                            oResponseClientDetail.YearToDateTurnover = oClientDetail.YearToDateTurnover
                            oResponseClientDetail.LastYearTurnover = oClientDetail.LastYearTurnover
                            oResponseClientDetail.AccountBalanceSpecified = oClientDetail.AccountBalanceSpecified
                            oResponseClientDetail.YearToDateTurnoverSpecified = oClientDetail.YearToDateTurnoverSpecified
                            oResponseClientDetail.LastYearTurnoverSpecified = oClientDetail.LastYearTurnoverSpecified

                            If oClientDetail.Associates IsNot Nothing AndAlso oClientDetail.Associates.Count > 0 Then

                                oResponseClientDetail.Associates = oClientDetail.Associates.ToList().ConvertAll(
                                    New Converter(Of BaseImplementationTypes.BaseAssociateType, BaseAssociateType) _
                                    (AddressOf CommonFunctions.ToServiceAssociateType))

                            End If

                            If oClientDetail.Convictions IsNot Nothing AndAlso oClientDetail.Convictions.Count > 0 Then

                                oResponseClientDetail.Convictions = oClientDetail.Convictions.ToList().ConvertAll(
                                    New Converter(Of BaseImplementationTypes.BaseConvictionType, BaseConvictionType) _
                                    (AddressOf CommonFunctions.ToServiceConvictionType))

                            End If

                            If oClientDetail.LoyaltyScheme IsNot Nothing AndAlso oClientDetail.LoyaltyScheme.Count > 0 Then

                                oResponseClientDetail.LoyaltyScheme = oClientDetail.LoyaltyScheme.ToList().ConvertAll(
                                New Converter(Of BaseImplementationTypes.BaseClientSharedDataTypeLoyaltyScheme, BaseClientSharedDataTypeLoyaltyScheme) _
                                (AddressOf CommonFunctions.ToServiceLoyaltyScheme))

                            End If

                            If oClientDetail.ProspectPolicies IsNot Nothing AndAlso oClientDetail.ProspectPolicies.Count > 0 Then

                                oResponseClientDetail.ProspectPolicies = oClientDetail.ProspectPolicies.ToList().ConvertAll(
                                    New Converter(Of BaseImplementationTypes.BaseClientSharedDataTypeProspectPolicies, BaseClientSharedDataTypeProspectPolicies) _
                                    (AddressOf CommonFunctions.ToServiceProspectPolicies))

                            End If
                        End If

                        oResponse.Item = oResponseParty
                    ElseIf oImpResponse.Item.GetType Is GetType(SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyOTHERType) Then
                        PopulateOtherPartyReponse(oImpResponse, oResponse)
                    End If

                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetPartyRequest))
            End Try

            Return oResponse

        Catch ex As Exception

            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetPartyRequest))
            Return Nothing

        End Try

    End Function

    ''' To update a party with the details given in request
    ''' </summary>
    ''' <param name="UpdatePartyRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateParty(ByVal UpdatePartyRequest As UpdatePartyRequestType) As UpdatePartyResponseType Implements IPurePartyService.UpdateParty, IPurePolicyService.UpdateParty

        Try
            Dim sUserName As String = UpdatePartyRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMUPty", iUserId)
            CommonFunctions.CheckSecurityToken(UpdatePartyRequest.WCFSecurityToken)
            Dim oResponse As New UpdatePartyResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, UpdatePartyRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdatePartyRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdatePartyResponseType = Nothing

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

                    impPartyPC.Currency = msgParty.Currency
                    impPartyPC.SubBranchCode = msgParty.SubBranchCode
                    impPartyPC.BranchCode = msgParty.BranchCode
                    impPartyPC.AccountExecutiveCode = msgParty.AccountExecutiveCode
                    impPartyPC.Forename = msgParty.Forename
                    impPartyPC.Surname = msgParty.Surname
                    impPartyPC.Initials = msgParty.Initials
                    impPartyPC.Title = msgParty.Title
                    impPartyPC.DateOfBirth = msgParty.DateOfBirth
                    impPartyPC.GenderCode = msgParty.GenderCode
                    impPartyPC.MaritalStatusCode = CType([Enum].ToObject(GetType(MaritalStatusCodeType), msgParty.MaritalStatusCode), BaseImplementationTypes.MaritalStatusCodeType)
                    impPartyPC.MaritalStatusCodeSpecified = msgParty.MaritalStatusCodeSpecified
                    impPartyPC.OccupationCode = msgParty.OccupationCode
                    impPartyPC.EmploymentStatusCode = CType([Enum].ToObject(GetType(EmploymentStatusCodeType), msgParty.EmploymentStatusCode), BaseImplementationTypes.EmploymentStatusCodeType)
                    impPartyPC.EmploymentStatusCodeSpecified = msgParty.EmploymentStatusCodeSpecified
                    impPartyPC.EmployersBusinessCode = msgParty.EmployersBusinessCode
                    impPartyPC.AlternativeId = msgParty.AlternativeId
                    impPartyPC.FileCode = msgParty.FileCode
                    impPartyPC.TradingName = msgParty.TradingName
                    impPartyPC.SecOccupationCode = msgParty.SecOccupationCode
                    impPartyPC.SecEmployersBusinessCode = msgParty.SecEmployersBusinessCode
                    impPartyPC.SecEmploymentStatusCode = CType([Enum].ToObject(GetType(EmploymentStatusCodeType), msgParty.SecEmploymentStatusCode), BaseImplementationTypes.EmploymentStatusCodeType)
                    impPartyPC.SecEmploymentStatusCodeSpecified = msgParty.SecEmploymentStatusCodeSpecified
                    impPartyPC.NationalityCode = msgParty.NationalityCode
                    impPartyPC.AccommodationCode = msgParty.AccommodationCode
                    impPartyPC.Salutation = msgParty.Salutation
                    impPartyPC.TPS = msgParty.TPS
                    impPartyPC.TPSSpecified = msgParty.TPSSpecified
                    impPartyPC.MPS = msgParty.MPS
                    impPartyPC.MPSSpecified = msgParty.MPSSpecified
                    impPartyPC.eMPS = msgParty.eMPS
                    impPartyPC.eMPSSpecified = msgParty.eMPSSpecified
                    impPartyPC.Source = msgParty.Source
                    impPartyPC.PetOwner = msgParty.PetOwner
                    impPartyPC.PetOwnerSpecified = msgParty.PetOwnerSpecified

                    If msgParty.ClientDetail IsNot Nothing Then
                        Dim impPCClientDetail As BaseImplementationTypes.BaseClientSharedDataType
                        impPCClientDetail = New BaseImplementationTypes.BaseClientSharedDataType
                        impPartyPC.ClientDetail = impPCClientDetail
                        impPCClientDetail.ServiceLevelCode = msgParty.ClientDetail.ServiceLevelCode
                        impPCClientDetail.AreaCode = msgParty.ClientDetail.AreaCode
                        impPCClientDetail.LeadAgentKey = msgParty.ClientDetail.LeadAgentKey
                        impPCClientDetail.LeadAgentKeySpecified = msgParty.ClientDetail.LeadAgentKeySpecified
                        impPCClientDetail.IsProspect = msgParty.ClientDetail.IsProspect
                        impPCClientDetail.IsProspectSpecified = msgParty.ClientDetail.IsProspectSpecified
                        impPCClientDetail.IsAgent = msgParty.ClientDetail.IsAgent
                        impPCClientDetail.IsAgentSpecified = msgParty.ClientDetail.IsAgentSpecified
                        impPCClientDetail.CorrespondenceCode = msgParty.ClientDetail.CorrespondenceCode
                        impPCClientDetail.PaymentCode = msgParty.ClientDetail.PaymentCode
                        impPCClientDetail.ReminderCode = msgParty.ClientDetail.ReminderCode
                        impPCClientDetail.PaymentTermCode = msgParty.ClientDetail.PaymentTermCode
                        impPCClientDetail.RenewalStopCode = msgParty.ClientDetail.RenewalStopCode
                        impPCClientDetail.LoyaltyNumber = msgParty.ClientDetail.LoyaltyNumber
                        impPCClientDetail.SeasonalGiftCode = msgParty.ClientDetail.SeasonalGiftCode
                        impPCClientDetail.CountyCourtJudgments = msgParty.ClientDetail.CountyCourtJudgments
                        impPCClientDetail.CountyCourtJudgmentsSpecified = msgParty.ClientDetail.CountyCourtJudgmentsSpecified
                        impPCClientDetail.AgentReference = msgParty.ClientDetail.AgentReference
                        impPCClientDetail.CurrentIntermediaryKey = msgParty.ClientDetail.CurrentIntermediaryKey
                        impPCClientDetail.CurrentIntermediaryKeySpecified = msgParty.ClientDetail.CurrentIntermediaryKeySpecified
                        impPCClientDetail.StrengthCode = msgParty.ClientDetail.StrengthCode
                        impPCClientDetail.StatusCode = msgParty.ClientDetail.StatusCode
                        impPCClientDetail.PreviousInsurerKey = msgParty.ClientDetail.PreviousInsurerKey
                        impPCClientDetail.PreviousInsurerKeySpecified = msgParty.ClientDetail.PreviousInsurerKeySpecified
                        impPCClientDetail.PreviousBrokerKey = msgParty.ClientDetail.PreviousBrokerKey
                        impPCClientDetail.PreviousBrokerKeySpecified = msgParty.ClientDetail.PreviousBrokerKeySpecified

                        If (msgParty.ClientDetail.Associates) IsNot Nothing AndAlso (msgParty.ClientDetail.Associates.Count > 0) Then
                            impPartyPC.ClientDetail.Associates = Array.ConvertAll(msgParty.ClientDetail.Associates.ToArray(),
                                                        New Converter(Of BaseAssociateType,
                                                        BaseImplementationTypes.BaseAssociateType) _
                                                        (AddressOf CommonFunctions.ToBaseImpBaseAssociateType))
                        End If

                        If msgParty.ClientDetail.LoyaltyScheme IsNot Nothing AndAlso (msgParty.ClientDetail.LoyaltyScheme.Count > 0) Then
                            impPartyPC.ClientDetail.LoyaltyScheme = Array.ConvertAll(msgParty.ClientDetail.LoyaltyScheme.ToArray(),
                                                        New Converter(Of BaseClientSharedDataTypeLoyaltyScheme,
                                                        BaseImplementationTypes.BaseClientSharedDataTypeLoyaltyScheme) _
                                                        (AddressOf CommonFunctions.ToBaseImpBaseClientSharedDataTypeLoyaltyScheme))
                        End If

                        If msgParty.ClientDetail.ProspectPolicies IsNot Nothing AndAlso (msgParty.ClientDetail.ProspectPolicies.Count > 0) Then
                            impPartyPC.ClientDetail.ProspectPolicies = Array.ConvertAll(msgParty.ClientDetail.ProspectPolicies.ToArray(),
                                                        New Converter(Of BaseClientSharedDataTypeProspectPolicies,
                                                        BaseImplementationTypes.BaseClientSharedDataTypeProspectPolicies) _
                                                        (AddressOf CommonFunctions.ToBaseImpBaseClientSharedDataTypeProspectPolicies))
                        End If

                        If msgParty.Lifestyle IsNot Nothing AndAlso (msgParty.Lifestyle.Count > 0) Then
                            impPartyPC.Lifestyle = Array.ConvertAll(msgParty.Lifestyle.ToArray(),
                                                        New Converter(Of BasePartyPCTypeLifestyle,
                                                        BaseImplementationTypes.BasePartyPCTypeLifestyle) _
                                                        (AddressOf CommonFunctions.ToBaseImpBaseClientSharedDataTypeLifeStyle))
                        End If

                        If (msgParty.ClientDetail.Convictions) IsNot Nothing AndAlso (msgParty.ClientDetail.Convictions.Count > 0) Then
                            impPartyPC.ClientDetail.Convictions = Array.ConvertAll(msgParty.ClientDetail.Convictions.ToArray(),
                                                        New Converter(Of BaseConvictionType,
                                                        BaseImplementationTypes.BaseConvictionType) _
                                                        (AddressOf CommonFunctions.ToBaseImpBaseConvictionType))
                        End If
                    End If

                    impParty = impPartyPC

                ElseIf UpdatePartyRequest.Item.GetType Is GetType(BasePartyCCType) Then

                    ' Process corporate client
                    Dim msgParty As BasePartyCCType = DirectCast(UpdatePartyRequest.Item, BasePartyCCType)
                    Dim impPartyCC As New BaseImplementationTypes.BasePartyCCType

                    impPartyCC.Currency = msgParty.Currency
                    impPartyCC.AccountExecutiveCode = msgParty.AccountExecutiveCode

                    impPartyCC.NumberOfEmployees = msgParty.NumberOfEmployees
                    impPartyCC.NumberOfOffices = msgParty.NumberOfOffices
                    impPartyCC.NumberOfOfficesSpecified = msgParty.NumberOfOfficesSpecified
                    impPartyCC.SubBranchCode = msgParty.SubBranchCode
                    impPartyCC.MainContact = msgParty.MainContact

                    impPartyCC.AlternativeId = msgParty.AlternativeId
                    impPartyCC.BranchCode = msgParty.BranchCode
                    impPartyCC.CompanyName = msgParty.CompanyName
                    impPartyCC.BusinessCode = msgParty.BusinessCode
                    impPartyCC.FileCode = msgParty.FileCode

                    impPartyCC.CompanyReg = msgParty.CompanyReg
                    impPartyCC.TradeCode = msgParty.TradeCode
                    impPartyCC.SICCode = msgParty.SICCode
                    impPartyCC.TradingSince = msgParty.TradingSince
                    impPartyCC.TradingSinceSpecified = msgParty.TradingSinceSpecified
                    impPartyCC.WageRoll = msgParty.WageRoll
                    impPartyCC.WageRollSpecified = msgParty.WageRollSpecified
                    impPartyCC.TurnoverCode = msgParty.TurnoverCode
                    impPartyCC.FinancialYear = msgParty.FinancialYear
                    impPartyCC.FinancialYearSpecified = msgParty.FinancialYearSpecified
                    impPartyCC.Salutation = msgParty.Salutation
                    impPartyCC.TPS = msgParty.TPS
                    impPartyCC.TPSSpecified = msgParty.TPSSpecified
                    impPartyCC.MPS = msgParty.MPS
                    impPartyCC.MPSSpecified = msgParty.MPSSpecified
                    impPartyCC.eMPS = msgParty.eMPS
                    impPartyCC.eMPSSpecified = msgParty.eMPSSpecified
                    impPartyCC.Source = msgParty.Source

                    If msgParty.ClientDetail IsNot Nothing Then
                        Dim impCCClientDetail As BaseImplementationTypes.BaseClientSharedDataType
                        impCCClientDetail = New BaseImplementationTypes.BaseClientSharedDataType
                        impPartyCC.ClientDetail = impCCClientDetail
                        impCCClientDetail.ServiceLevelCode = msgParty.ClientDetail.ServiceLevelCode
                        impCCClientDetail.AreaCode = msgParty.ClientDetail.AreaCode
                        impCCClientDetail.LeadAgentKey = msgParty.ClientDetail.LeadAgentKey
                        impCCClientDetail.LeadAgentKeySpecified = msgParty.ClientDetail.LeadAgentKeySpecified
                        impCCClientDetail.IsProspect = msgParty.ClientDetail.IsProspect
                        impCCClientDetail.IsProspectSpecified = msgParty.ClientDetail.IsProspectSpecified
                        impCCClientDetail.IsAgent = msgParty.ClientDetail.IsAgent
                        impCCClientDetail.IsAgentSpecified = msgParty.ClientDetail.IsAgentSpecified
                        impCCClientDetail.CorrespondenceCode = msgParty.ClientDetail.CorrespondenceCode
                        impCCClientDetail.PaymentCode = msgParty.ClientDetail.PaymentCode
                        impCCClientDetail.ReminderCode = msgParty.ClientDetail.ReminderCode
                        impCCClientDetail.PaymentTermCode = msgParty.ClientDetail.PaymentTermCode
                        impCCClientDetail.RenewalStopCode = msgParty.ClientDetail.RenewalStopCode
                        impCCClientDetail.LoyaltyNumber = msgParty.ClientDetail.LoyaltyNumber
                        impCCClientDetail.SeasonalGiftCode = msgParty.ClientDetail.SeasonalGiftCode
                        impCCClientDetail.CountyCourtJudgments = msgParty.ClientDetail.CountyCourtJudgments
                        impCCClientDetail.CountyCourtJudgmentsSpecified = msgParty.ClientDetail.CountyCourtJudgmentsSpecified
                        impCCClientDetail.AgentReference = msgParty.ClientDetail.AgentReference
                        impCCClientDetail.CurrentIntermediaryKey = msgParty.ClientDetail.CurrentIntermediaryKey
                        impCCClientDetail.CurrentIntermediaryKeySpecified = msgParty.ClientDetail.CurrentIntermediaryKeySpecified
                        impCCClientDetail.StrengthCode = msgParty.ClientDetail.StrengthCode
                        impCCClientDetail.StatusCode = msgParty.ClientDetail.StatusCode
                        impCCClientDetail.PreviousInsurerKey = msgParty.ClientDetail.PreviousInsurerKey
                        impCCClientDetail.PreviousInsurerKeySpecified = msgParty.ClientDetail.PreviousInsurerKeySpecified
                        impCCClientDetail.PreviousBrokerKey = msgParty.ClientDetail.PreviousBrokerKey
                        impCCClientDetail.PreviousBrokerKeySpecified = msgParty.ClientDetail.PreviousBrokerKeySpecified

                        If (msgParty.ClientDetail.Associates) IsNot Nothing AndAlso msgParty.ClientDetail.Associates.Count > 0 Then
                            impPartyCC.ClientDetail.Associates = Array.ConvertAll(msgParty.ClientDetail.Associates.ToArray(),
                                                        New Converter(Of BaseAssociateType,
                                                        BaseImplementationTypes.BaseAssociateType) _
                                                        (AddressOf CommonFunctions.ToBaseImpBaseAssociateType))
                        End If

                        If msgParty.ClientDetail.LoyaltyScheme IsNot Nothing AndAlso msgParty.ClientDetail.LoyaltyScheme.Count > 0 Then
                            impPartyCC.ClientDetail.LoyaltyScheme = Array.ConvertAll(msgParty.ClientDetail.LoyaltyScheme.ToArray(),
                                                        New Converter(Of BaseClientSharedDataTypeLoyaltyScheme,
                                                        BaseImplementationTypes.BaseClientSharedDataTypeLoyaltyScheme) _
                                                        (AddressOf CommonFunctions.ToBaseImpBaseClientSharedDataTypeLoyaltyScheme))
                        End If

                        If msgParty.ClientDetail.ProspectPolicies IsNot Nothing AndAlso msgParty.ClientDetail.ProspectPolicies.Count > 0 Then
                            impPartyCC.ClientDetail.ProspectPolicies = Array.ConvertAll(msgParty.ClientDetail.ProspectPolicies.ToArray(),
                                                        New Converter(Of BaseClientSharedDataTypeProspectPolicies,
                                                        BaseImplementationTypes.BaseClientSharedDataTypeProspectPolicies) _
                                                        (AddressOf CommonFunctions.ToBaseImpBaseClientSharedDataTypeProspectPolicies))
                        End If

                        If (msgParty.ClientDetail.Convictions) IsNot Nothing AndAlso msgParty.ClientDetail.Convictions.Count > 0 Then
                            impPartyCC.ClientDetail.Convictions = Array.ConvertAll(msgParty.ClientDetail.Convictions.ToArray(),
                                                        New Converter(Of BaseConvictionType,
                                                        BaseImplementationTypes.BaseConvictionType) _
                                                        (AddressOf CommonFunctions.ToBaseImpBaseConvictionType))
                        End If
                    End If


                    impParty = impPartyCC
                ElseIf UpdatePartyRequest.Item.GetType Is GetType(BasePartyOTHERType) Then

                    Dim objPartyOther As BasePartyOTHERType = DirectCast(UpdatePartyRequest.Item, BasePartyOTHERType)
                    Dim impPartyOther As New BaseImplementationTypes.BasePartyOTHERType

                    If objPartyOther.Accident IsNot Nothing AndAlso objPartyOther.Accident.Count > 0 Then
                        impPartyOther.Accident = Array.ConvertAll(objPartyOther.Accident.ToArray(),
                                                    New Converter(Of BasePartyOTHERTypeAccident,
                                                    BaseImplementationTypes.BasePartyOTHERTypeAccident) _
                                                    (AddressOf CommonFunctions.ToBaseImpBasePartyOTHERTypeAccident))

                    End If

                    If objPartyOther.SupplierBusiness IsNot Nothing AndAlso objPartyOther.SupplierBusiness.Count > 0 Then
                        impPartyOther.SupplierBusiness = Array.ConvertAll(objPartyOther.SupplierBusiness.ToArray(),
                                                    New Converter(Of BasePartyOTHERTypeSupplierBusiness,
                                                    BaseImplementationTypes.BasePartyOTHERTypeSupplierBusiness) _
                                                    (AddressOf CommonFunctions.ToBaseImpBasePartyOTHERTypeSupplierBusiness))

                    End If

                    If objPartyOther.Branches IsNot Nothing AndAlso objPartyOther.Branches.Count > 0 Then
                        impPartyOther.Branches = Array.ConvertAll(objPartyOther.Branches.ToArray(),
                                                                  New Converter(Of BasePartyOTHERTypeBranch,
                                                                  BaseImplementationTypes.BasePartyOTHERTypeBranch) _
                                                    (AddressOf CommonFunctions.ToBaseImpBasePartyOTHERTypeBranch))

                    End If

                    impPartyOther.ActiveIndicator = objPartyOther.ActiveIndicator
                    impPartyOther.AfterHoursIndicator = objPartyOther.AfterHoursIndicator
                    impPartyOther.Code = objPartyOther.Code
                    impPartyOther.IsTPASettleDirectly = objPartyOther.ISTPASettleDirectly

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
                    impPartyOther.BranchCode = objPartyOther.BranchCode
                    impPartyOther.SubBranchCode = objPartyOther.SubBranchCode
                    impPartyOther.Currency = objPartyOther.Currency
                    impParty = DirectCast(impPartyOther, BaseImplementationTypes.BasePartyType)
                    impParty.BranchCode = UpdatePartyRequest.Item.BranchCode

                    If (objPartyOther.Convictions) IsNot Nothing AndAlso (objPartyOther.Convictions.Count > 0) Then
                        impPartyOther.Convictions = Array.ConvertAll(objPartyOther.Convictions.ToArray(),
                                                    New Converter(Of BaseConvictionType,
                                                    BaseImplementationTypes.BaseConvictionType) _
                                                    (AddressOf CommonFunctions.ToBaseImpBaseConvictionType))
                    End If

                    oImpRequest.Party = impParty

                End If

                If UpdatePartyRequest.Item.Addresses IsNot Nothing AndAlso UpdatePartyRequest.Item.Addresses.Count > 0 Then
                    impParty.Addresses = Array.ConvertAll(UpdatePartyRequest.Item.Addresses.ToArray(),
                                                New Converter(Of BaseAddressWithContactsType,
                                                BaseImplementationTypes.BaseAddressWithContactsType) _
                                                (AddressOf CommonFunctions.ToBaseImpBaseAddressWithContactsType))
                End If

                If UpdatePartyRequest.Item.Contacts IsNot Nothing AndAlso UpdatePartyRequest.Item.Contacts.Count > 0 Then
                    impParty.Contacts = Array.ConvertAll(UpdatePartyRequest.Item.Contacts.ToArray(),
                                                New Converter(Of BaseContactType,
                                                BaseImplementationTypes.BaseContactType) _
                                                (AddressOf CommonFunctions.ToBaseImpBaseContactType))
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
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(UpdatePartyRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(UpdatePartyRequest))
            Return Nothing
        End Try

    End Function


    ''' <summary>
    ''' To populate the other party response
    ''' </summary>
    ''' <param name="oImpResponse"></param>
    ''' <param name="oResponse"></param>
    ''' <remarks></remarks>
    Private Sub PopulateOtherPartyReponse(ByVal oImpResponse As SAMForInsuranceV2ImplementationTypes.GetPartyResponseType,
                                          ByVal oResponse As GetPartyResponseType)

        Dim oResponseParty As New BasePartyOTHERType
        Dim oPartyOther As New SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyOTHERType

        oPartyOther = DirectCast(oImpResponse.Item, SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyOTHERType)
        oResponseParty = DirectCast(oResponse.Item, BasePartyOTHERType)

        If oPartyOther.Accident IsNot Nothing Then
            oResponseParty.Accident = oPartyOther.Accident.ToList().ConvertAll(
                                    New Converter(Of BaseImplementationTypes.BasePartyOTHERTypeAccident, BasePartyOTHERTypeAccident) _
                                    (AddressOf CommonFunctions.ToServiceAccident))
        End If

        If oPartyOther.Convictions IsNot Nothing Then
            oResponseParty.Convictions = oPartyOther.Convictions.ToList().ConvertAll(
                                    New Converter(Of BaseImplementationTypes.BaseConvictionType, BaseConvictionType) _
                                    (AddressOf CommonFunctions.ToServiceConvictionType))
        End If

        If oPartyOther.SupplierBusiness IsNot Nothing Then
            oResponseParty.SupplierBusiness = oPartyOther.SupplierBusiness.ToList().ConvertAll(
                                    New Converter(Of BaseImplementationTypes.BasePartyOTHERTypeSupplierBusiness, BasePartyOTHERTypeSupplierBusiness) _
                                    (AddressOf CommonFunctions.ToServiceSupplierBusiness))
        End If

        If oPartyOther.Branches IsNot Nothing Then
            oResponseParty.Branches = oPartyOther.Branches.ToList().ConvertAll(
                                    New Converter(Of BaseImplementationTypes.BasePartyOTHERTypeBranch, BasePartyOTHERTypeBranch) _
                                    (AddressOf CommonFunctions.ToServiceBranch))
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
        oResponseParty.FileCode = oPartyOther.FileCode
        If oPartyOther.IsTPASettleDirectly <> "" Then
            oResponseParty.ISTPASettleDirectly = oPartyOther.IsTPASettleDirectly
        End If

    End Sub

    ''' <summary>  
    ''' This web services method is used to GetBrokerSummary.
    ''' </summary>  
    ''' <param name="GetBrokerSummaryRequest">An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetBrokerSummaryRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.GetBrokerSummaryResponseType</returns>  
    ''' </summary>  
    Public Function GetBrokerSummary(ByVal oGetBrokerSummaryRequest As GetBrokerSummaryRequestType) As GetBrokerSummaryResponseType Implements IPurePolicyService.GetBrokerSummary

        Try

            Dim sUserName As String = oGetBrokerSummaryRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGETBS", iUserId)
            CommonFunctions.CheckSecurityToken(oGetBrokerSummaryRequest.WCFSecurityToken)
            Dim oResponse As New GetBrokerSummaryResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetBrokerSummaryRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetBrokerSummaryRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetBrokerSummaryResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.BranchCode = oGetBrokerSummaryRequest.BranchCode
            If oGetBrokerSummaryRequest.AgentKey > 0 Then
                oImpRequest.AgentKey = oGetBrokerSummaryRequest.AgentKey
            Else
                oImpRequest.AgentKey = iAgentKey
            End If
            oImpRequest.InsuranceRef = oGetBrokerSummaryRequest.InsuranceRef
            oImpRequest.ProductCode = oGetBrokerSummaryRequest.ProductCode
            oImpRequest.InsuredName = oGetBrokerSummaryRequest.InsuredName
            oImpRequest.RecordType = oGetBrokerSummaryRequest.RecordType.ToString
            oImpRequest.RecordTypeSpecified = oGetBrokerSummaryRequest.RecordTypeSpecified
            oImpRequest.ShowUserOnly = oGetBrokerSummaryRequest.ShowUserOnly
            oImpRequest.ShowUserOnlySpecified = oGetBrokerSummaryRequest.ShowUserOnlySpecified
            If oGetBrokerSummaryRequest.MaxRowsToFetchSpecified Then
                oImpRequest.MaxRowsToFetch = oGetBrokerSummaryRequest.MaxRowsToFetch
            Else
                oImpRequest.MaxRowsToFetch = -1
            End If
            oImpRequest.CoverStartDate = oGetBrokerSummaryRequest.CoverStartDate
            oImpRequest.CoverStartDateSpecified = oGetBrokerSummaryRequest.CoverStartDateSpecified
            oImpRequest.QuoteORLiveDate = oGetBrokerSummaryRequest.QuoteORLiveDate
            oImpRequest.QuoteORLiveDateSpecified = oGetBrokerSummaryRequest.QuoteORLiveDateSpecified
            oImpRequest.UserKey = oGetBrokerSummaryRequest.UserKey
            oImpRequest.UserKeySpecified = oGetBrokerSummaryRequest.UserKeySpecified
            oImpRequest.RetrieveAssociates = oGetBrokerSummaryRequest.retrieveAssociates

            oImpRequest.WCFSecurityToken = If(oGetBrokerSummaryRequest.WCFSecurityToken.Length > 0, oGetBrokerSummaryRequest.WCFSecurityToken, "WCFSecurityToken")
            oImpRequest.RiskIndex = oGetBrokerSummaryRequest.RiskIndex
            oImpRequest.FilterQuotes = oGetBrokerSummaryRequest.FilterQuotes
            oImpRequest.FilterPolicies = oGetBrokerSummaryRequest.FilterPolicies
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetBrokerSummary(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                'oResponse.InsuranceFileDetails = SAMFunc.GetDeserializedValues(Of List(Of BaseGetBrokerSummaryResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataset, sFromTypeName:="BaseGetBrokerSummaryResponseTypeInsuranceFileDetails", sConvertToTypeName:="BaseGetBrokerSummaryResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.InsuranceFileDetails = DataTabletoList_GetBrokerSummary(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetBrokerSummaryRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetBrokerSummaryRequest))
            Return Nothing
        End Try

    End Function

#Region "Private Method"

    Private Sub ClaimPaymentIn(ByVal oImpClaimPayment As BaseImplementationTypes.BaseClaimPaymentType, ByVal oClaimPayment As BaseClaimPaymentType, ByVal sBranchCode As String, Optional ByVal bCopyCashList As Boolean = False)

        oImpClaimPayment.BaseClaimKey = oClaimPayment.BaseClaimKey
        oImpClaimPayment.BaseClaimPerilKey = oClaimPayment.BaseClaimPerilKey
        oImpClaimPayment.ClaimVersionDescription = oClaimPayment.ClaimVersionDescription
        oImpClaimPayment.CurrencyCode = oClaimPayment.CurrencyCode
        oImpClaimPayment.PartyKey = oClaimPayment.PartyKey
        oImpClaimPayment.PaymentPartyType = CType([Enum].ToObject(GetType(ClaimPaymentPartyTypeType), oClaimPayment.PaymentPartyType), BaseImplementationTypes.ClaimPaymentPartyTypeType)
        oImpClaimPayment.CloseClaimOnZeroReserveRecoveryBalance = oClaimPayment.CloseClaimOnZeroReserveRecoveryBalance
        oImpClaimPayment.closeClaimOnFinalPayment = oClaimPayment.closeClaimOnFinalPayment
        oImpClaimPayment.PaymentOnly = oClaimPayment.PaymentOnly
        oImpClaimPayment.ClientKey = oClaimPayment.ClientKey
        oImpClaimPayment.TransactionDate = Date.Now()
        oImpClaimPayment.OurRef = oClaimPayment.OurRef
        oImpClaimPayment.UltimatePayee = oClaimPayment.UltimatePayee
        oImpClaimPayment.IsExGratia = oClaimPayment.IsExGratia

        If oClaimPayment.ClaimPaymentTaxItems IsNot Nothing Then

            oImpClaimPayment.ClaimPaymentTaxItems = Array.ConvertAll(oClaimPayment.ClaimPaymentTaxItems.ToArray(),
                                                       New Converter(Of BaseClaimPaymentTaxItemType,
                                                       BaseImplementationTypes.BaseClaimPaymentTaxItemType) _
                                                       (AddressOf CommonFunctions.ToBaseClaimPaymentTaxItemType))

        End If

        If oClaimPayment.AdvancedTaxDetails IsNot Nothing Then

            oImpClaimPayment.AdvancedTaxDetails = New BaseImplementationTypes.BaseClaimPaymentAdvancedTaxDetailsType
            oImpClaimPayment.AdvancedTaxDetails.InsuredDomiciled = oClaimPayment.AdvancedTaxDetails.InsuredDomiciled
            oImpClaimPayment.AdvancedTaxDetails.InsuredPercentage = oClaimPayment.AdvancedTaxDetails.InsuredPercentage
            oImpClaimPayment.AdvancedTaxDetails.IsSettlement = oClaimPayment.AdvancedTaxDetails.IsSettlement
            oImpClaimPayment.AdvancedTaxDetails.IsTaxExempt = oClaimPayment.AdvancedTaxDetails.IsTaxExempt
            oImpClaimPayment.AdvancedTaxDetails.IsWHTExempt = oClaimPayment.AdvancedTaxDetails.IsWHTExempt
            oImpClaimPayment.AdvancedTaxDetails.InsuranceTaxNumber = oClaimPayment.AdvancedTaxDetails.InsuranceTaxNumber
            oImpClaimPayment.AdvancedTaxDetails.PayeeDomiciled = oClaimPayment.AdvancedTaxDetails.PayeeDomiciled
            oImpClaimPayment.AdvancedTaxDetails.PayeePercentage = oClaimPayment.AdvancedTaxDetails.PayeePercentage
            oImpClaimPayment.AdvancedTaxDetails.PayeeTaxNumber = oClaimPayment.AdvancedTaxDetails.PayeeTaxNumber
            oImpClaimPayment.AdvancedTaxDetails.SafeHarbourCode = oClaimPayment.AdvancedTaxDetails.SafeHarbourCode
            oImpClaimPayment.AdvancedTaxDetails.SafeHarbourPercentage = oClaimPayment.AdvancedTaxDetails.SafeHarbourPercentage
            oImpClaimPayment.AdvancedTaxDetails.PaymentTo = oClaimPayment.AdvancedTaxDetails.PaymentTo
            oImpClaimPayment.AdvancedTaxDetails.PayeeName = oClaimPayment.AdvancedTaxDetails.PayeeName

        End If

        If oClaimPayment.Payee IsNot Nothing Then

            oImpClaimPayment.Payee = New BaseImplementationTypes.BaseClaimPayeeType
            oImpClaimPayment.Payee.BankCode = oClaimPayment.Payee.BankCode
            oImpClaimPayment.Payee.BankName = oClaimPayment.Payee.BankName
            oImpClaimPayment.Payee.BankNumber = oClaimPayment.Payee.BankNumber
            oImpClaimPayment.Payee.MediaReference = oClaimPayment.Payee.MediaReference
            oImpClaimPayment.Payee.MediaTypeCode = oClaimPayment.Payee.MediaTypeCode
            oImpClaimPayment.Payee.Name = GetPaymentName(oClaimPayment.Payee.Name)
            oImpClaimPayment.Payee.TheirReference = oClaimPayment.Payee.TheirReference
            oImpClaimPayment.Payee.Comments = oClaimPayment.Payee.Comments
            oImpClaimPayment.Payee.PartyBankKey = oClaimPayment.Payee.PartyBankKey
            oImpClaimPayment.Payee.BIC = oClaimPayment.Payee.BIC
            oImpClaimPayment.Payee.IBAN = oClaimPayment.Payee.IBAN
            oImpClaimPayment.Payee.AccountType = oClaimPayment.Payee.AccountType
            If oClaimPayment.Payee.Address IsNot Nothing Then
                oImpClaimPayment.Payee.Address = New BaseImplementationTypes.BaseAddressType
                oImpClaimPayment.Payee.Address.AddressLine1 = oClaimPayment.Payee.Address.AddressLine1
                oImpClaimPayment.Payee.Address.AddressLine2 = oClaimPayment.Payee.Address.AddressLine2
                oImpClaimPayment.Payee.Address.AddressLine3 = oClaimPayment.Payee.Address.AddressLine3
                oImpClaimPayment.Payee.Address.AddressLine4 = oClaimPayment.Payee.Address.AddressLine4
                oImpClaimPayment.Payee.Address.AddressTypeCode = CType([Enum].ToObject(GetType(AddressTypeType), oClaimPayment.Payee.Address.AddressTypeCode), BaseImplementationTypes.AddressTypeType)
                oImpClaimPayment.Payee.Address.PostCode = oClaimPayment.Payee.Address.PostCode
                oImpClaimPayment.Payee.Address.CountryCode = oClaimPayment.Payee.Address.CountryCode
            End If
        End If


        If oClaimPayment.ClaimPaymentItem IsNot Nothing Then

            oImpClaimPayment.ClaimPaymentItem = Array.ConvertAll(oClaimPayment.ClaimPaymentItem.ToArray(),
                                                       New Converter(Of BaseClaimPaymentItemType,
                                                       BaseImplementationTypes.BaseClaimPaymentItemType) _
                                                       (AddressOf CommonFunctions.ToBaseClaimPaymentItemType))

        End If
        If bCopyCashList = True Then
            If oClaimPayment.CashList IsNot Nothing Then
                Dim oCashList As New BaseImplementationTypes.BasePaymentCashListType
                With oClaimPayment.CashList
                    oCashList.BranchCode = sBranchCode
                    oCashList.Reference = .Reference
                    oCashList.TypeCode = .TypeCode
                    oCashList.BankAccountCode = .BankAccountCode

                    oCashList.CurrencyCode = .CurrencyCode

                    oCashList.ListDate = .ListDate
                    oCashList.StatusCode = .StatusCode
                    oCashList.SubBranchCode = .SubBranchCode
                    'Payment items
                    If .PaymentItem IsNot Nothing Then
                        oCashList.PaymentItem = Array.ConvertAll(.PaymentItem.ToArray(),
                                                                                New Converter(Of BasePaymentCashListItemType,
                                                                                BaseImplementationTypes.BasePaymentCashListItemType) _
                                                                                (AddressOf CommonFunctions.ToBasePaymentCashListItemType))

                    End If
                End With
                oImpClaimPayment.CashList = oCashList
            End If
        End If

    End Sub

    Private Sub ClaimDataImportIn(
    ByVal service As ClaimDataImportRequestType,
    ByVal implementation As SAMForInsuranceV2ImplementationTypes.ClaimDataImportRequestType)

        implementation.BranchCode = service.BranchCode
        implementation.Claim = CommonFunctions.ToBaseImpBaseCDTClaimType(service.Claim)

        ' claim versioning mode - default to false ( partial versioning )
        implementation.UseFullClaimVersioningSpecified = True
        If service.UseFullClaimVersioningSpecified Then
            implementation.UseFullClaimVersioning = service.UseFullClaimVersioning
        Else
            implementation.UseFullClaimVersioning = False
        End If

    End Sub

    Private Sub ClaimDataImportOut(
    ByVal service As ClaimDataImportResponseType,
    ByVal implementation As SAMForInsuranceV2ImplementationTypes.ClaimDataImportResponseType)

    End Sub

    Private Sub ClaimReceiptIn(
    ByVal oImpRequest As SAMForInsuranceV2ImplementationTypes.ClaimReceiptRequestType,
    ByVal oRequest As ClaimReceiptRequestType)

        ' claim receipt details

        oImpRequest.BranchCode = oRequest.BranchCode
        oImpRequest.TimeStamp = oRequest.TimeStamp
        oImpRequest.GetSavedTaxOfPeril = oRequest.GetSavedTaxOfPeril
        oImpRequest.PostTransaction = oRequest.PostTransaction

        If oRequest.ClaimReceipt IsNot Nothing Then
            oImpRequest.ClaimReceipt = New BaseImplementationTypes.BaseClaimReceiptType
            oImpRequest.ClaimReceipt.DoNotCreateClaimVersionOnSalvageReceipt = oRequest.ClaimReceipt.DoNotCreateClaimVersionOnSalvageReceipt

            oImpRequest.ClaimReceipt.BaseClaimKey = CInt(oRequest.ClaimReceipt.BaseClaimKey)
            oImpRequest.ClaimReceipt.BaseClaimPerilKey = CInt(Cast.ToString(oRequest.ClaimReceipt.BaseClaimPerilKey, "0"))
            oImpRequest.ClaimReceipt.ClaimVersionDescription = oRequest.ClaimReceipt.ClaimVersionDescription
            oImpRequest.ClaimReceipt.CurrencyCode = oRequest.ClaimReceipt.CurrencyCode
            oImpRequest.ClaimReceipt.PartyKey = oRequest.ClaimReceipt.PartyKey

            oImpRequest.ClaimReceipt.ReceiptPartyType = CType([Enum].ToObject(GetType(ClaimReceiptPartyTypeType), oRequest.ClaimReceipt.ReceiptPartyType), BaseImplementationTypes.ClaimReceiptPartyTypeType)

            oImpRequest.ClaimReceipt.IsSalvageRecovery = oRequest.ClaimReceipt.IsSalvageRecovery

            oImpRequest.ClaimReceipt.ClaimId = oRequest.ClaimReceipt.ClaimKey
            oImpRequest.ClaimReceipt.ClaimPerilId = oRequest.ClaimReceipt.ClaimPerilKey

            oImpRequest.ClaimReceipt.TransactionDate = Date.Now()
            oImpRequest.CloseClaimOnZeroReserveRecoveryBalance = oRequest.CloseClaimOnZeroReserveRecoveryBalance
            ' claim receipt items
            If oRequest.ClaimReceipt.ReceiptItem IsNot Nothing Then
                oImpRequest.ClaimReceipt.ReceiptItem = Array.ConvertAll(oRequest.ClaimReceipt.ReceiptItem.ToArray,
                                                            New Converter(Of BaseClaimReceiptItemType,
                                                            BaseImplementationTypes.BaseClaimReceiptItemType) _
                                                            (AddressOf CommonFunctions.ToBaseImpBaseClaimReceiptItemType))
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
            If oRequest.ClaimReceipt.Payee IsNot Nothing Then
                oImpRequest.ClaimReceipt.Payee = New BaseImplementationTypes.BaseClaimPayeeType
                oImpRequest.ClaimReceipt.Payee.BankCode = oRequest.ClaimReceipt.Payee.BankCode
                oImpRequest.ClaimReceipt.Payee.BankName = oRequest.ClaimReceipt.Payee.BankName
                oImpRequest.ClaimReceipt.Payee.BankNumber = oRequest.ClaimReceipt.Payee.BankNumber
                oImpRequest.ClaimReceipt.Payee.MediaReference = oRequest.ClaimReceipt.Payee.MediaReference
                oImpRequest.ClaimReceipt.Payee.MediaTypeCode = oRequest.ClaimReceipt.Payee.MediaTypeCode
                oImpRequest.ClaimReceipt.Payee.Name = oRequest.ClaimReceipt.Payee.Name
                oImpRequest.ClaimReceipt.Payee.Comments = oRequest.ClaimReceipt.Payee.Comments
                oImpRequest.ClaimReceipt.Payee.TheirReference = oRequest.ClaimReceipt.Payee.TheirReference
                oImpRequest.ClaimReceipt.Payee.BIC = oRequest.ClaimReceipt.Payee.BIC
                oImpRequest.ClaimReceipt.Payee.IBAN = oRequest.ClaimReceipt.Payee.IBAN

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
            End If

        ElseIf oRequest.ClaimReceiptCollection IsNot Nothing Then
            oImpRequest.ClaimReceiptCollection = New List(Of Internal.BaseClaimReceiptType)
            For ReceiptItemCount As Integer = 0 To oRequest.ClaimReceiptCollection.Count - 1
                Dim ClaimReceipt As New BaseImplementationTypes.BaseClaimReceiptType
                ClaimReceipt.DoNotCreateClaimVersionOnSalvageReceipt = oRequest.ClaimReceiptCollection(ReceiptItemCount).DoNotCreateClaimVersionOnSalvageReceipt
                ClaimReceipt.BaseClaimKey = CInt(oRequest.ClaimReceiptCollection(ReceiptItemCount).BaseClaimKey)
                ClaimReceipt.BaseClaimPerilKey = CInt(Cast.ToString(oRequest.ClaimReceiptCollection(ReceiptItemCount).BaseClaimPerilKey, "0"))
                ClaimReceipt.ClaimVersionDescription = oRequest.ClaimReceiptCollection(ReceiptItemCount).ClaimVersionDescription
                ClaimReceipt.CurrencyCode = oRequest.ClaimReceiptCollection(ReceiptItemCount).CurrencyCode
                ClaimReceipt.PartyKey = oRequest.ClaimReceiptCollection(ReceiptItemCount).PartyKey

                ClaimReceipt.ReceiptPartyType = CType([Enum].ToObject(GetType(ClaimReceiptPartyTypeType), oRequest.ClaimReceiptCollection(ReceiptItemCount).ReceiptPartyType), BaseImplementationTypes.ClaimReceiptPartyTypeType)

                ClaimReceipt.IsSalvageRecovery = oRequest.ClaimReceiptCollection(ReceiptItemCount).IsSalvageRecovery

                ClaimReceipt.ClaimId = oRequest.ClaimReceiptCollection(ReceiptItemCount).ClaimKey
                ClaimReceipt.ClaimPerilId = oRequest.ClaimReceiptCollection(ReceiptItemCount).ClaimPerilKey

                ClaimReceipt.TransactionDate = Date.Now()
                oImpRequest.CloseClaimOnZeroReserveRecoveryBalance = oRequest.CloseClaimOnZeroReserveRecoveryBalance
                ' claim receipt items
                If oRequest.ClaimReceiptCollection(ReceiptItemCount).ReceiptItem IsNot Nothing Then
                    ClaimReceipt.ReceiptItem = Array.ConvertAll(oRequest.ClaimReceiptCollection(ReceiptItemCount).ReceiptItem.ToArray,
                                                                New Converter(Of BaseClaimReceiptItemType,
                                                                BaseImplementationTypes.BaseClaimReceiptItemType) _
                                                                (AddressOf CommonFunctions.ToBaseImpBaseClaimReceiptItemType))
                End If

                ' advanced tax options
                If oRequest.ClaimReceiptCollection(ReceiptItemCount).AdvancedTaxDetails IsNot Nothing Then
                    ClaimReceipt.AdvancedTaxDetails = New BaseImplementationTypes.BaseClaimReceiptAdvancedTaxDetailsType
                    ClaimReceipt.AdvancedTaxDetails.InsuredDomiciled = oRequest.ClaimReceiptCollection(ReceiptItemCount).AdvancedTaxDetails.InsuredDomiciled
                    ClaimReceipt.AdvancedTaxDetails.InsuredPercentage = oRequest.ClaimReceiptCollection(ReceiptItemCount).AdvancedTaxDetails.InsuredPercentage
                    ClaimReceipt.AdvancedTaxDetails.IsSettlement = oRequest.ClaimReceiptCollection(ReceiptItemCount).AdvancedTaxDetails.IsSettlement
                    ClaimReceipt.AdvancedTaxDetails.IsTaxExempt = oRequest.ClaimReceiptCollection(ReceiptItemCount).AdvancedTaxDetails.IsTaxExempt
                    ClaimReceipt.AdvancedTaxDetails.ReceivableTaxPercentage = oRequest.ClaimReceiptCollection(ReceiptItemCount).AdvancedTaxDetails.ReceivableTaxPercentage
                    ClaimReceipt.AdvancedTaxDetails.InsuredTaxNumber = oRequest.ClaimReceiptCollection(ReceiptItemCount).AdvancedTaxDetails.InsuredTaxNumber
                End If

                ' payee
                If oRequest.ClaimReceiptCollection(ReceiptItemCount).Payee IsNot Nothing Then
                    ClaimReceipt.Payee = New BaseImplementationTypes.BaseClaimPayeeType
                    ClaimReceipt.Payee.BankCode = oRequest.ClaimReceiptCollection(ReceiptItemCount).Payee.BankCode
                    ClaimReceipt.Payee.BankName = oRequest.ClaimReceiptCollection(ReceiptItemCount).Payee.BankName
                    ClaimReceipt.Payee.BankNumber = oRequest.ClaimReceiptCollection(ReceiptItemCount).Payee.BankNumber
                    ClaimReceipt.Payee.MediaReference = oRequest.ClaimReceiptCollection(ReceiptItemCount).Payee.MediaReference
                    ClaimReceipt.Payee.MediaTypeCode = oRequest.ClaimReceiptCollection(ReceiptItemCount).Payee.MediaTypeCode
                    ClaimReceipt.Payee.Name = oRequest.ClaimReceiptCollection(ReceiptItemCount).Payee.Name
                    ClaimReceipt.Payee.Comments = oRequest.ClaimReceiptCollection(ReceiptItemCount).Payee.Comments
                    ClaimReceipt.Payee.TheirReference = oRequest.ClaimReceiptCollection(ReceiptItemCount).Payee.TheirReference
                    ClaimReceipt.Payee.BIC = oRequest.ClaimReceiptCollection(ReceiptItemCount).Payee.BIC
                    ClaimReceipt.Payee.IBAN = oRequest.ClaimReceiptCollection(ReceiptItemCount).Payee.IBAN

                    ' payee address
                    If oRequest.ClaimReceiptCollection(ReceiptItemCount).Payee.Address IsNot Nothing Then
                        ClaimReceipt.Payee.Address = New BaseImplementationTypes.BaseAddressType
                        ClaimReceipt.Payee.Address.AddressLine1 = oRequest.ClaimReceiptCollection(ReceiptItemCount).Payee.Address.AddressLine1
                        ClaimReceipt.Payee.Address.AddressLine2 = oRequest.ClaimReceiptCollection(ReceiptItemCount).Payee.Address.AddressLine2
                        ClaimReceipt.Payee.Address.AddressLine3 = oRequest.ClaimReceiptCollection(ReceiptItemCount).Payee.Address.AddressLine3
                        ClaimReceipt.Payee.Address.AddressLine4 = oRequest.ClaimReceiptCollection(ReceiptItemCount).Payee.Address.AddressLine4

                        ClaimReceipt.Payee.Address.AddressTypeCode = CType([Enum].ToObject(GetType(AddressTypeType), oRequest.ClaimReceiptCollection(ReceiptItemCount).Payee.Address.AddressTypeCode), BaseImplementationTypes.AddressTypeType)
                        ClaimReceipt.Payee.Address.PostCode = oRequest.ClaimReceiptCollection(ReceiptItemCount).Payee.Address.PostCode
                        ClaimReceipt.Payee.Address.CountryCode = oRequest.ClaimReceiptCollection(ReceiptItemCount).Payee.Address.CountryCode
                    End If
                End If
                oImpRequest.ClaimReceiptCollection.Add(ClaimReceipt)
            Next
        End If
    End Sub

    Private Sub PayClaimIn(ByVal oImpRequest As SAMForInsuranceV2ImplementationTypes.PayClaimRequestType, ByVal oRequest As PayClaimRequestType, Optional ByVal bSkipTaxItemExpansion As Boolean = False)

        oImpRequest.TimeStamp = oRequest.TimeStamp
        oImpRequest.BranchCode = oRequest.BranchCode
        oImpRequest.ClaimPayment = New BaseImplementationTypes.BaseClaimPaymentType
        oImpRequest.ClaimPayment.BaseClaimKey = oRequest.ClaimPayment.BaseClaimKey
        oImpRequest.ClaimPayment.BaseClaimPerilKey = oRequest.ClaimPayment.BaseClaimPerilKey
        oImpRequest.ClaimPayment.ClaimVersionDescription = oRequest.ClaimPayment.ClaimVersionDescription
        oImpRequest.ClaimPayment.CurrencyCode = oRequest.ClaimPayment.CurrencyCode
        oImpRequest.ClaimPayment.PartyKey = oRequest.ClaimPayment.PartyKey
        oImpRequest.ClaimPayment.PaymentPartyType = CType([Enum].ToObject(GetType(ClaimPaymentPartyTypeType), oRequest.ClaimPayment.PaymentPartyType), BaseImplementationTypes.ClaimPaymentPartyTypeType)
        oImpRequest.ClaimPayment.CloseClaimOnZeroReserveRecoveryBalance = oRequest.ClaimPayment.CloseClaimOnZeroReserveRecoveryBalance

        oImpRequest.ClaimPayment.PaymentOnly = oRequest.ClaimPayment.PaymentOnly
        oImpRequest.ClaimPayment.OurRef = oRequest.ClaimPayment.OurRef
        oImpRequest.ClaimPayment.UltimatePayee = oRequest.ClaimPayment.UltimatePayee
        oImpRequest.ClaimPayment.SkipTaxItemExpansion = bSkipTaxItemExpansion

        oImpRequest.ClaimPayment.TransactionDate = Date.Now()
        oImpRequest.ClaimPayment.ViewMode = oRequest.ClaimPayment.ViewMode
        oImpRequest.ClaimPayment.IsExGratia = oRequest.ClaimPayment.IsExGratia

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
            oImpRequest.ClaimPayment.AdvancedTaxDetails.PaymentToCode = oRequest.ClaimPayment.AdvancedTaxDetails.PaymentToCode
            oImpRequest.ClaimPayment.AdvancedTaxDetails.PaymentTo = oRequest.ClaimPayment.AdvancedTaxDetails.PaymentTo
        End If

        If oRequest.ClaimPayment.Payee IsNot Nothing Then


            oImpRequest.ClaimPayment.Payee = New BaseImplementationTypes.BaseClaimPayeeType
            oImpRequest.ClaimPayment.Payee.BankCode = oRequest.ClaimPayment.Payee.BankCode
            oImpRequest.ClaimPayment.Payee.BankName = oRequest.ClaimPayment.Payee.BankName
            oImpRequest.ClaimPayment.Payee.BankNumber = oRequest.ClaimPayment.Payee.BankNumber
            oImpRequest.ClaimPayment.Payee.MediaReference = oRequest.ClaimPayment.Payee.MediaReference
            oImpRequest.ClaimPayment.Payee.MediaTypeCode = oRequest.ClaimPayment.Payee.MediaTypeCode
            oImpRequest.ClaimPayment.Payee.Name = oRequest.ClaimPayment.Payee.Name
            oImpRequest.ClaimPayment.Payee.TheirReference = oRequest.ClaimPayment.Payee.TheirReference
            oImpRequest.ClaimPayment.Payee.Comments = oRequest.ClaimPayment.Payee.Comments
            oImpRequest.ClaimPayment.Payee.PartyBankKey = oRequest.ClaimPayment.Payee.PartyBankKey
            oImpRequest.ClaimPayment.Payee.BIC = oRequest.ClaimPayment.Payee.BIC
            oImpRequest.ClaimPayment.Payee.IBAN = oRequest.ClaimPayment.Payee.IBAN

            oImpRequest.ClaimPayment.Payee.Chequedate = oRequest.ClaimPayment.Payee.Chequedate

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

        End If

        If oRequest.ClaimPayment.ClaimPaymentItem IsNot Nothing Then

            oImpRequest.ClaimPayment.ClaimPaymentItem = Array.ConvertAll(oRequest.ClaimPayment.ClaimPaymentItem.ToArray(),
                                                      New Converter(Of BaseClaimPaymentItemType,
                                                      BaseImplementationTypes.BaseClaimPaymentItemType) _
                                                      (AddressOf CommonFunctions.ToBaseClaimPaymentItemType))
        End If

        If oRequest.ClaimPayment.CashList IsNot Nothing Then
            Dim oCashList As New BaseImplementationTypes.BasePaymentCashListType
            With oRequest.ClaimPayment.CashList
                oCashList.BranchCode = oRequest.BranchCode
                oCashList.Reference = .Reference
                oCashList.TypeCode = .TypeCode
                oCashList.BankAccountCode = .BankAccountCode

                oCashList.CurrencyCode = .CurrencyCode

                oCashList.ListDate = .ListDate
                oCashList.StatusCode = .StatusCode

                'Payment items
                If .PaymentItem IsNot Nothing Then
                    oCashList.PaymentItem = Array.ConvertAll(.PaymentItem.ToArray(),
                                                                               New Converter(Of BasePaymentCashListItemType,
                                                                               BaseImplementationTypes.BasePaymentCashListItemType) _
                                                                               (AddressOf CommonFunctions.ToBasePaymentCashListItemType))


                End If
            End With
            oImpRequest.ClaimPayment.CashList = oCashList
        End If
        oImpRequest.GetSavedTaxOfPeril = oRequest.GetSavedTaxOfPeril
    End Sub

#End Region


    ''' <summary>  
    ''' To Add a Party Bank Details
    ''' </summary>
    ''' <param name=" AddPartyBankDetailsRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddPartyBankDetails(ByVal AddPartyBankDetailsRequest As AddPartyBankDetailsRequestType) As AddPartyBankDetailsResponseType Implements IPurePartyService.AddPartyBankDetails

        Try

            Dim sUserName As String = AddPartyBankDetailsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMAPBD", iUserId)
            CommonFunctions.CheckSecurityToken(AddPartyBankDetailsRequest.WCFSecurityToken)

            Dim oResponse As New AddPartyBankDetailsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, AddPartyBankDetailsRequest.BranchCode)

            'Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.AddPartyBankDetailsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.AddPartyBankDetailsResponseType = Nothing

            'Pass the values to the implementation request structure
            With AddPartyBankDetailsRequest
                oImpRequest.BranchCode = .BranchCode
                oImpRequest.PartyKey = .PartyKey
                oImpRequest.TimeStamp = .TimeStamp

                'Party bank items
                If .PartyBankDetails IsNot Nothing Then
                    'Temporary objects to hold the party bank details
                    Dim oPartyBankDetails() As SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyBankType = Nothing
                    Dim oPartyBankItem As SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyBankType = Nothing
                    Dim oBank As SiriusFS.SAM.Structure.BaseImplementationTypes.BaseBankType = Nothing
                    Dim oCreditCard As SiriusFS.SAM.Structure.BaseImplementationTypes.BaseCreditCardType = Nothing
                    Dim oBankAddress As SiriusFS.SAM.Structure.BaseImplementationTypes.BaseSimpleAddressType = Nothing
                    Dim oCardHolder As SiriusFS.SAM.Structure.BaseImplementationTypes.BaseCreditCardTypeCardHolder = Nothing

                    ReDim oPartyBankDetails(.PartyBankDetails.Count - 1)

                    For cntIndex As Integer = 0 To .PartyBankDetails.Count - 1
                        oPartyBankItem = New SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyBankType
                        With .PartyBankDetails(cntIndex)
                            oPartyBankItem.PartyBankKey = .PartyBankKey
                            oPartyBankItem.BankPaymentTypeCode = .BankPaymentTypeCode
                            oPartyBankItem.AccountKey = .AccountKey
                            oPartyBankItem.AccountHolderName = .AccountHolderName
                            oPartyBankItem.AccountType = .AccountType
                            oPartyBankItem.IsBankItem = .IsBankItem
                            oPartyBankItem.RowKey = .RowKey

                            If .Bank IsNot Nothing Then
                                oBank = New SiriusFS.SAM.Structure.BaseImplementationTypes.BaseBankType
                                With .Bank
                                    oBank.AccountNumber = .AccountNumber
                                    oBank.BankCode = .BankCode
                                    oBank.Branch = .Branch
                                    oBank.BranchCode = .BranchCode
                                    oBank.BIC = .BIC
                                    oBank.IBAN = .IBAN

                                    'Bank Address Details
                                    If .BankAddress IsNot Nothing Then
                                        oBankAddress = New SiriusFS.SAM.Structure.BaseImplementationTypes.BaseSimpleAddressType
                                        With .BankAddress
                                            oBankAddress.AddressLine1 = .AddressLine1
                                            oBankAddress.AddressLine2 = .AddressLine2
                                            oBankAddress.AddressLine3 = .AddressLine3
                                            oBankAddress.AddressLine4 = .AddressLine4
                                            oBankAddress.PostCode = .PostCode
                                            oBankAddress.CountryCode = .CountryCode
                                        End With
                                        oBank.BankAddress = oBankAddress
                                        oBankAddress = Nothing
                                    End If
                                End With
                                oPartyBankItem.Bank = oBank
                                oBank = Nothing
                            End If

                            If .CreditCard IsNot Nothing Then
                                oCreditCard = New SiriusFS.SAM.Structure.BaseImplementationTypes.BaseCreditCardType
                                With .CreditCard
                                    oCreditCard.Number = .Number
                                    oCreditCard.StartDate = .StartDate
                                    oCreditCard.ExpiryDate = .ExpiryDate
                                    oCreditCard.NameOnCreditCard = .NameOnCreditCard
                                    oCreditCard.Issue = .Issue
                                    oCreditCard.Pin = .Pin
                                    oCreditCard.IsRegisteredCardHolder = .IsRegisteredCardHolder
                                    oCreditCard.ManualAuthCode = .ManualAuthCode
                                    oCreditCard.TrackingNumber = .TrackingNumber
                                    oCreditCard.IsDefaultCreditCard = .IsDefaultCreditCard

                                    'CardHolder Address Details
                                    If .CardHolder IsNot Nothing Then
                                        oCardHolder = New SiriusFS.SAM.Structure.BaseImplementationTypes.BaseCreditCardTypeCardHolder
                                        With .CardHolder
                                            oCardHolder.AddressLine1 = .AddressLine1
                                            oCardHolder.AddressLine2 = .AddressLine2
                                            oCardHolder.AddressLine3 = .AddressLine3
                                            oCardHolder.AddressLine4 = .AddressLine4
                                            oCardHolder.PostCode = .PostCode
                                            oCardHolder.CountryCode = .CountryCode
                                        End With
                                        oCreditCard.CardHolder = oCardHolder
                                        oCardHolder = Nothing
                                    End If
                                End With
                                oPartyBankItem.CreditCard = oCreditCard
                                oCreditCard = Nothing
                            End If
                        End With
                        oPartyBankDetails(cntIndex) = oPartyBankItem
                        oPartyBankItem = Nothing
                    Next
                    oImpRequest.PartyBankDetails = oPartyBankDetails
                End If
            End With

            Try
                'Call the implementation method
                oImpResponse = oBusiness.AddPartyBankDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'Get values from implementation layer
                Dim bHasErrors As Boolean = False
                With oImpResponse
                    oResponse.TimeStamp = .TimeStamp
                    If .PartyBankStatus IsNot Nothing AndAlso .PartyBankStatus.Count > 0 Then
                        oResponse.PartyBankStatus = .PartyBankStatus.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseAddPartyBankStatusType, BaseAddPartyBankStatusType)(AddressOf CommonFunctions.ToServiceAddPartyBankStatusList))
                        For cntIndex As Integer = 0 To oResponse.PartyBankStatus.Count - 1
                            If oResponse.PartyBankStatus(cntIndex).Errors IsNot Nothing Then
                                bHasErrors = True
                                Exit For
                            End If
                        Next
                    End If
                End With

                If bHasErrors Then
                    Dim oErrorCollection As SAMErrorCollection
                    oErrorCollection = New SAMErrorCollection
                    oErrorCollection.AddBusinessRule(SAMErrorCode.GeneralFailure, "Errors occurred during operation. Please check individual items to get more details.")
                    oErrorCollection.CheckForErrors()
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(AddPartyBankDetailsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(AddPartyBankDetailsRequest))
            Return Nothing
        End Try

    End Function

    ''' <summary>  
    ''' This web services method is used to delete party bank deatils through nexus using party bank key.
    ''' </summary>  
    ''' <param name="DeletePartyBankDetails"></param>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.BaseDeletePartyBankDetailsRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.BaseDeletePartyBankDetailsRequestType</returns>  

    Public Function DeletePartyBankDetails(ByVal oDeletePartyBankDetailsRequest As DeletePartyBankDetailsRequestType) As DeletePartyBankDetailsResponseType Implements IPurePartyService.DeletePartyBankDetails

        Try
            Dim oResponse As New DeletePartyBankDetailsResponseType


            Dim sUserName As String = oDeletePartyBankDetailsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMDPBD", iUserId)
            CommonFunctions.CheckSecurityToken(oDeletePartyBankDetailsRequest.WCFSecurityToken)
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oDeletePartyBankDetailsRequest.BranchCode)
            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.DeletePartyBankDetailsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.DeletePartyBankDetailsResponseType = Nothing

            With oDeletePartyBankDetailsRequest

                oImpRequest.BranchCode = .BranchCode
                oImpRequest.PartyKey = .PartyKey

                'Party bank items
                If .PartBankDetails IsNot Nothing Then
                    Dim iLength As Int32 = .PartBankDetails.Count
                    If iLength > 0 Then
                        oImpRequest.PartBankDetails = New BaseImplementationTypes.BaseDeletePartyBankDetailsRequestTypePartBankDetails
                        For icount As Integer = 0 To iLength - 1
                            ReDim Preserve oImpRequest.PartBankDetails.Row(icount)
                            oImpRequest.PartBankDetails.Row(icount) = New BaseImplementationTypes.BaseDeletePartyBankDetailsRequestTypePartBankDetailsRow
                            oImpRequest.PartBankDetails.Row(icount).PartyBankKey = .PartBankDetails(icount).PartyBankKey
                        Next
                    End If
                End If

            End With
            Try
                'Call the implementation method
                oImpResponse = oBusiness.DeletePartyBank(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oDeletePartyBankDetailsRequest))
            End Try
            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oDeletePartyBankDetailsRequest))
            Return Nothing
        End Try
    End Function

    Public Function ActivatePartyBankDetails(ByVal oActivatePartyBankRequestType As ActivatePartyBankRequestType) As ActivatePartyBankResponseType Implements IPurePartyService.ActivatePartyBankDetails

        Try

            Dim sUserName As String = oActivatePartyBankRequestType.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMAVPBD", iUserId)
            CommonFunctions.CheckSecurityToken(oActivatePartyBankRequestType.WCFSecurityToken)
            Dim oResponse As New ActivatePartyBankResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oActivatePartyBankRequestType.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.ActivatePartyBankRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.ActivatePartyBankResponseType = Nothing

            'Passing values in request
            With oActivatePartyBankRequestType
                oImpRequest.BranchCode = .BranchCode
                oImpRequest.PartyKey = .PartyKey
                'Party bank items
                If .PartBankDetails IsNot Nothing Then
                    Dim iLength As Int32 = .PartBankDetails.Count
                    If iLength > 0 Then
                        oImpRequest.PartBankDetails = New BaseImplementationTypes.BaseActivatePartyBankRequestTypePartBankDetails
                        For icount As Integer = 0 To iLength - 1
                            ReDim Preserve oImpRequest.PartBankDetails.Row(icount)
                            oImpRequest.PartBankDetails.Row(icount) = New BaseImplementationTypes.BaseActivatePartyBankRequestTypePartBankDetailsRow
                            oImpRequest.PartBankDetails.Row(icount).PartyBankKey = .PartBankDetails(icount).PartyBankKey
                            oImpRequest.PartBankDetails.Row(icount).MakeActive = .PartBankDetails(icount).MakeActive
                        Next
                    End If
                End If
            End With

            Try
                'Call the implementation method
                oImpResponse = oBusiness.ActivatePartyBank(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oActivatePartyBankRequestType))
            End Try
            Return oResponse

        Catch ex As Exception

            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oActivatePartyBankRequestType))
            Return Nothing
        End Try
    End Function

    ''' <summary>  
    ''' Retrieves details of a party bank item or all Party bank items for a specified party.
    ''' </summary>  
    ''' <param name="GetPartyBankDetailsRequest">An object of type SiriusFS.SAM.Structure.SFI.SAMForInsurancev2.GetPartyBankDetailsRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.Structure.SFI.SAMForInsurancev2.GetPartyBankDetailsResponseType</returns>  

    Public Function GetPartyBankDetails(ByVal GetPartyBankDetailsRequest As GetPartyBankDetailsRequestType) As GetPartyBankDetailsResponseType Implements IPurePartyService.GetPartyBankDetails, IPureAccountService.GetPartyBankDetails, IPureClaimService.GetPartyBankDetails, IPurePolicyService.GetPartyBankDetails

        Try

            Dim sUserName As String = GetPartyBankDetailsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGPBD", iUserId)
            CommonFunctions.CheckSecurityToken(GetPartyBankDetailsRequest.WCFSecurityToken)
            Dim oResponse As New GetPartyBankDetailsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetPartyBankDetailsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetPartyBankDetailsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetPartyBankDetailsResponseType = Nothing

            ' Pass the values to the implementation request structure
            With GetPartyBankDetailsRequest
                oImpRequest.BranchCode = .BranchCode
                oImpRequest.PartyKey = .PartyKey
                oImpRequest.AccountKey = .AccountKey
                oImpRequest.PartyBankKey = .PartyBankKey
                oImpRequest.IncludeHistory = .IncludeHistory
                oImpRequest.IncludeLastTransactedPartyBankKey = .IncludeLastTransactedPartyBankKey
                oImpRequest.TransactionKey = .TransactionKey
                oImpRequest.LastTransactionType = CType([Enum].ToObject(GetType(LastTransactionTypeWithPartyBank), .LastTransactionType), BaseImplementationTypes.LastTransactionTypeWithPartyBank)
                oImpRequest.BankPaymentTypeCode = .BankPaymentTypeCode
                oImpRequest.ISBank = .ISBank
            End With

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetPartyBankDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'Get values from implementation layer
                With oImpResponse
                    oResponse.IsExternalCreditCardHandling = .IsExternalCreditCardHandling
                    oResponse.LastTransactedPartyBankKey = .LastTransactedPartyBankKey
                    oResponse.TimeStamp = .TimeStamp

                    If .PartyBankDetails IsNot Nothing Then
                        oResponse.PartyBankDetails = .PartyBankDetails.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BasePartyBankType, BasePartyBankType)(AddressOf CommonFunctions.ToServicePartyBankTypeList))
                    End If
                End With
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetPartyBankDetailsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetPartyBankDetailsRequest))
            Return Nothing
        End Try

    End Function
    Public Function GetPartyPolicies(ByVal oGetPartyPoliciesRequest As GetPartyPoliciesRequestType) As GetPartyPoliciesResponseType Implements IPurePartyService.GetPartyPolicies, IPurePolicyService.GetPartyPolicies
        Try
            Dim sUserName As String = oGetPartyPoliciesRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGClmDet", iUserId)
            CommonFunctions.CheckSecurityToken(oGetPartyPoliciesRequest.WCFSecurityToken)
            Dim oResponse As New GetPartyPoliciesResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetPartyPoliciesRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetPartyPoliciesRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetPartyPoliciesResponseType = Nothing

            oImpRequest.BranchCode = oGetPartyPoliciesRequest.BranchCode
            oImpRequest.PartyCode = oGetPartyPoliciesRequest.PartyCode
            If oGetPartyPoliciesRequest.RetrieveAssociates Then
                oImpRequest.RetrieveAssociates = Convert.ToBoolean(oGetPartyPoliciesRequest.RetrieveAssociates)
            Else
                oImpRequest.RetrieveAssociates = False
            End If
            oImpRequest.WCFSecurityToken = If(oGetPartyPoliciesRequest.WCFSecurityToken.Length > 0, oGetPartyPoliciesRequest.WCFSecurityToken, "WCFSecurityToken")
            ' Pass the values to the implementation request structure

            ' Call the implementation method
            Try
                oImpResponse = oBusiness.GetPartyPolicies(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                oResponse.PartyName = oImpResponse.PartyName
                oResponse.PartyCode = oImpResponse.PartyCode
                oResponse.PartyKey = oImpResponse.PartyKey
                oResponse.SourceKey = oImpResponse.SourceKey

                'oResponse.PartyPolicies = SAMFunc.GetDeserializedValues(Of List(Of BaseGetPartyPoliciesResponseTypeRow))(elmResultDataSet:=oImpResponse.ResultDataSet, sFromTypeName:="BaseGetPartyPoliciesResponseTypePartyPolicies", sConvertToTypeName:="BaseGetPartyPoliciesResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.PartyPolicies = DataTabletoList_GetPartyPolicies(oImpResponse.ResultData.Tables(0))
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oGetPartyPoliciesRequest))
            End Try

            Return oResponse
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oGetPartyPoliciesRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetPartySummary(ByVal GetPartySummaryRequest As GetPartySummaryRequestType) As GetPartySummaryResponseType Implements IPurePartyService.GetPartySummary

        Try

            Dim sUserName As String = GetPartySummaryRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGPtySum", iUserId)
            CommonFunctions.CheckSecurityToken(GetPartySummaryRequest.WCFSecurityToken)
            Dim oResponse As New GetPartySummaryResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetPartySummaryRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetPartySummaryRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetPartySummaryResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = GetPartySummaryRequest.BranchCode
            oImpRequest.PartyKey = GetPartySummaryRequest.PartyKey
            oImpRequest.UserName = sUserName
            If GetPartySummaryRequest.RetrieveAssociates Then
                oImpRequest.RetrieveAssociates = GetPartySummaryRequest.RetrieveAssociates
            Else
                oImpRequest.RetrieveAssociates = False
            End If
            oImpRequest.WCFSecurityToken = If(GetPartySummaryRequest.WCFSecurityToken.Length > 0, GetPartySummaryRequest.WCFSecurityToken, "WCFSecurityToken")
            Try

                ' Call the implementation method
                oImpResponse = oBusiness.GetPartySummary(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                'oResponse.Policies = SAMFunc.GetDeserializedValues(Of List(Of BaseGetPartySummaryResponseTypeRow))(elmResultDataSet:=oImpResponse.InsuranceFileDataset, sFromTypeName:="BaseGetPartySummaryResponseTypePolicies", sConvertToTypeName:="BaseGetPartySummaryResponseTypeRow")
                If oImpResponse.ResultData IsNot Nothing AndAlso oImpResponse.ResultData.Tables(0) IsNot Nothing Then
                    oResponse.Policies = DataTabletoList_GetPartySummary(oImpResponse.ResultData.Tables(0))
                End If
                oResponse.PartyTimestamp = oImpResponse.PartyTimestamp

                Dim oParty As New BasePartyType
                If oImpResponse.Item.GetType Is GetType(SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyPCType) Then
                    oParty = New BasePartyPCType
                ElseIf oImpResponse.Item.GetType Is GetType(SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyCCType) Then
                    oParty = New BasePartyCCType
                End If

                ' get the addresses
                If oImpResponse.Item.Addresses IsNot Nothing Then

                    oParty.Addresses = oImpResponse.Item.Addresses.ToList().ConvertAll(
                            New Converter(Of BaseImplementationTypes.BaseAddressType, BaseAddressWithContactsType) _
                            (AddressOf CommonFunctions.ToServiceBaseAddressWithContactsType))

                End If

                ' get the contacts
                If oImpResponse.Item.Contacts IsNot Nothing Then
                    oParty.Contacts = oImpResponse.Item.Contacts.ToList().ConvertAll(
                            New Converter(Of BaseImplementationTypes.BaseContactType, BaseContactType) _
                            (AddressOf CommonFunctions.ToServiceContactType))

                End If

                oParty.BranchCode = oImpResponse.Item.BranchCode
                oParty.TPIntroducer = oImpResponse.Item.TPIntroducer
                oParty.TPUserCode = oImpResponse.Item.TPUserCode
                oResponse.PartyTimestamp = oImpResponse.PartyTimestamp
                oResponse.Item = oParty

                If oImpResponse.Item.GetType Is GetType(SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyPCType) Then
                    Dim oResponseParty As New BasePartyPCType
                    Dim oPartyPC As New SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyPCType
                    oPartyPC = DirectCast(oImpResponse.Item, SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyPCType)
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
                    oResponse.Item.Addresses = oParty.Addresses 'Addresses are added 
                    oResponse.Item.Contacts = oParty.Contacts 'Contacts are added 
                ElseIf oImpResponse.Item.GetType Is GetType(SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyCCType) Then
                    Dim oResponseParty As New BasePartyCCType
                    Dim oPartyCC As New SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyCCType
                    oPartyCC = DirectCast(oImpResponse.Item, SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyCCType)
                    oResponseParty.BusinessCode = oPartyCC.BusinessCode
                    oResponseParty.CompanyName = oPartyCC.CompanyName
                    oResponse.Item = oResponseParty
                    oResponse.Item.Addresses = oParty.Addresses 'Addresses are added 
                    oResponse.Item.Contacts = oParty.Contacts 'Contacts are added 
                End If

            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetPartySummaryRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetPartySummaryRequest))
            Return Nothing
        End Try

    End Function


    Public Function ReplacePartyContact(ByVal ReplacePartyContactRequest As ReplacePartyContactRequestType) As ReplacePartyContactResponseType Implements IPurePartyService.ReplacePartyContact
        Try

            Dim sUserName As String = ReplacePartyContactRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMFIFFCLM", iUserId)
            CommonFunctions.CheckSecurityToken(ReplacePartyContactRequest.WCFSecurityToken)
            Dim response As New ReplacePartyContactResponseType
            Dim business As CoreSAMBusiness = New CoreSAMBusiness(sUserName, ReplacePartyContactRequest.BranchCode)
            ' Implementation structures
            Dim impRequest As New SAMForInsuranceV2ImplementationTypes.ReplacePartyContactRequestType
            Dim impResponse As SAMForInsuranceV2ImplementationTypes.ReplacePartyContactResponseType = Nothing
            impRequest.AgentKey = iAgentKey
            impRequest.UserName = sUserName
            impRequest.BranchCode = ReplacePartyContactRequest.BranchCode
            impRequest.UserId = iUserId
            impRequest.Contacts = Array.ConvertAll(ReplacePartyContactRequest.Contacts.ToArray(),
                                        New Converter(Of BaseContactType,
                                        BaseImplementationTypes.BaseContactType) _
                                        (AddressOf CommonFunctions.ToBaseImpBaseContactType))

            impRequest.PartyKey = ReplacePartyContactRequest.PartyKey
            Try
                ' Call the implementation method
                impResponse = business.ReplacePartyContact(impRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(impResponse.STSError)
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(impResponse, response, ex, CommonFunctions.CreateDictionary(ReplacePartyContactRequest))
            End Try
            Return response
        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(ReplacePartyContactRequest))
            Return Nothing
        End Try
        Return Nothing
    End Function


    Private Sub UpdatePartyRiskIn(
  ByVal service As UpdatePartyRiskRequestType,
  ByVal implementation As SAMForInsuranceV2ImplementationTypes.UpdatePartyRiskRequestType)

        implementation.BranchCode = service.BranchCode
        implementation.PartyKey = service.PartyKey
        implementation.TimeStamp = service.TimeStamp
        implementation.XMLDataSet = service.XMLDataSet

    End Sub

    Private Sub UpdatePartyRiskOut(
ByVal service As UpdatePartyRiskResponseType,
ByVal implementation As SAMForInsuranceV2ImplementationTypes.UpdatePartyRiskResponseType)

        service.TimeStamp = implementation.TimeStamp
        service.XMLDataSet = implementation.XMLDataSet

    End Sub
    ''' <summary>  
    ''' This web services method is used to get address.
    ''' </summary>  
    ''' <param name="DeletePartyBankDetails"></param>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.BaseDeletePartyBankDetailsRequestType</param>  
    ''' <returns>An object of type SiriusFS.SAM.SFI.SAMForInsuranceV2.BaseDeletePartyBankDetailsRequestType</returns>  

    Public Function GetAddress(ByVal GetAddressRequest As GetAddressRequestType) As GetAddressResponseType Implements IPurePartyService.GetAddress, IPurePolicyService.GetAddress, IPureAccountService.GetAddress

        Try

            Dim sUserName As String = GetAddressRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMGAddr", iUserId)
            CommonFunctions.CheckSecurityToken(GetAddressRequest.WCFSecurityToken)

            Dim oResponse As New GetAddressResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetAddressRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetAddressRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetAddressResponseType = Nothing

            ' Pass the values to the implementation request structure
            oImpRequest.AddressKey = GetAddressRequest.AddressKey
            oImpRequest.AgentKey = iAgentKey
            oImpRequest.BranchCode = GetAddressRequest.BranchCode
            oImpRequest.UserName = sUserName
            oImpRequest.PartyKey = GetAddressRequest.PartyKey
            oImpRequest.PartyKeySpecified = GetAddressRequest.PartyKeySpecified

            Try
                ' Call the implementation method
                oImpResponse = oBusiness.GetAddress(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                ' Retrieve the values from the implementation response structure
                Dim oAddress As New BaseAddressType
                oAddress.AddressLine1 = oImpResponse.Address.AddressLine1
                oAddress.AddressLine2 = oImpResponse.Address.AddressLine2
                oAddress.AddressLine3 = oImpResponse.Address.AddressLine3
                oAddress.AddressLine4 = oImpResponse.Address.AddressLine4
                oAddress.AddressLine5 = oImpResponse.Address.AddressLine5
                oAddress.AddressLine6 = oImpResponse.Address.AddressLine6
                oAddress.AddressLine7 = oImpResponse.Address.AddressLine7
                oAddress.AddressLine8 = oImpResponse.Address.AddressLine8
                oAddress.AddressLine9 = oImpResponse.Address.AddressLine9
                oAddress.AddressLine10 = oImpResponse.Address.AddressLine10

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
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetAddressRequest))
            Return Nothing
        End Try

    End Function

    Public Function UpdatePartyBankDetails(ByVal oUpdatePartyBankDetailsRequest As UpdatePartyBankDetailsRequestType) As UpdatePartyBankDetailsResponseType Implements IPurePartyService.UpdatePartyBankDetails

        Try
            Dim sUserName As String = oUpdatePartyBankDetailsRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer

            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckAuthority("SAMUPBD", iUserId)
            CommonFunctions.CheckSecurityToken(oUpdatePartyBankDetailsRequest.WCFSecurityToken)

            Dim oResponse As New UpdatePartyBankDetailsResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oUpdatePartyBankDetailsRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.UpdatePartyBankDetailsRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.UpdatePartyBankDetailsResponseType = Nothing

            With oUpdatePartyBankDetailsRequest
                ' Pass the values to the implementation request structure
                oImpRequest.BranchCode = .BranchCode
                oImpRequest.TimeStamp = .TimeStamp
                'Party bank items
                If .PartyBankDetails IsNot Nothing Then
                    'Temporary objects to hold the party bank details
                    Dim oPartyBankDetails() As SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyBankType = Nothing
                    Dim oPartyBankItem As SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyBankType = Nothing
                    Dim oBankAddress As SiriusFS.SAM.Structure.BaseImplementationTypes.BaseSimpleAddressType = Nothing
                    Dim oBank As SiriusFS.SAM.Structure.BaseImplementationTypes.BaseBankType = Nothing
                    Dim oCreditCard As SiriusFS.SAM.Structure.BaseImplementationTypes.BaseCreditCardType = Nothing
                    Dim oCardHolder As SiriusFS.SAM.Structure.BaseImplementationTypes.BaseCreditCardTypeCardHolder = Nothing
                    Dim oHistory As SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyBankHistoryType = Nothing

                    ReDim oPartyBankDetails(.PartyBankDetails.Count - 1)
                    For cntIndex As Integer = 0 To .PartyBankDetails.Count - 1
                        oPartyBankItem = New SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyBankType
                        With .PartyBankDetails(cntIndex)
                            oPartyBankItem.PartyBankKey = .PartyBankKey
                            oPartyBankItem.BankPaymentTypeCode = .BankPaymentTypeCode
                            oPartyBankItem.AccountKey = .AccountKey
                            oPartyBankItem.AccountHolderName = .AccountHolderName
                            oPartyBankItem.AccountType = .AccountType
                            oPartyBankItem.IsBankItem = .IsBankItem
                            oPartyBankItem.RowKey = .RowKey
                            If .Bank IsNot Nothing Then
                                oBank = New SiriusFS.SAM.Structure.BaseImplementationTypes.BaseBankType
                                With .Bank
                                    oBank.AccountNumber = .AccountNumber
                                    oBank.BankCode = .BankCode
                                    oBank.Branch = .Branch
                                    oBank.BranchCode = .BranchCode
                                    oBank.BIC = .BIC
                                    oBank.IBAN = .IBAN

                                    'Bank Address Details
                                    If .BankAddress IsNot Nothing Then
                                        oBankAddress = New SiriusFS.SAM.Structure.BaseImplementationTypes.BaseSimpleAddressType
                                        With .BankAddress
                                            oBankAddress.AddressLine1 = .AddressLine1
                                            oBankAddress.AddressLine2 = .AddressLine2
                                            oBankAddress.AddressLine3 = .AddressLine3
                                            oBankAddress.AddressLine4 = .AddressLine4
                                            oBankAddress.PostCode = .PostCode
                                            oBankAddress.CountryCode = .CountryCode
                                        End With
                                        oBank.BankAddress = oBankAddress
                                    End If
                                End With
                                oPartyBankItem.Bank = oBank
                            End If

                            If .History IsNot Nothing Then
                                oHistory = New SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyBankHistoryType
                                With .History(cntIndex)
                                    oHistory.AccountHolderName = .AccountHolderName
                                    oHistory.AccountNumber = .AccountNumber
                                    oHistory.AccountType = .AccountType
                                    oHistory.ActionCode = .ActionCode
                                    oHistory.BankBranchCode = .BankBranchCode
                                    oHistory.BankName = .BankName
                                    oHistory.DateModified = .DateModified
                                    oHistory.PartyBankKey = .PartyBankKey
                                    oHistory.PostCode = .PostCode
                                    oHistory.StreetName = .StreetName
                                    oHistory.UserName = .UserName
                                End With
                                ReDim oPartyBankItem.History(cntIndex)
                                oPartyBankItem.History(cntIndex) = New SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyBankHistoryType
                                oPartyBankItem.History(cntIndex) = oHistory
                            End If

                            If .CreditCard IsNot Nothing Then
                                oCreditCard = New SiriusFS.SAM.Structure.BaseImplementationTypes.BaseCreditCardType
                                With .CreditCard
                                    oCreditCard.Number = .Number
                                    oCreditCard.StartDate = .StartDate
                                    oCreditCard.ExpiryDate = .ExpiryDate
                                    oCreditCard.NameOnCreditCard = .NameOnCreditCard
                                    oCreditCard.Issue = .Issue
                                    oCreditCard.Pin = .Pin
                                    oCreditCard.IsRegisteredCardHolder = .IsRegisteredCardHolder
                                    oCreditCard.ManualAuthCode = .ManualAuthCode
                                    oCreditCard.TrackingNumber = .TrackingNumber
                                    oCreditCard.IsDefaultCreditCard = .IsDefaultCreditCard

                                    'CardHolder Address Details
                                    If .CardHolder IsNot Nothing Then
                                        oCardHolder = New SiriusFS.SAM.Structure.BaseImplementationTypes.BaseCreditCardTypeCardHolder
                                        With .CardHolder
                                            oCardHolder.AddressLine1 = .AddressLine1
                                            oCardHolder.AddressLine2 = .AddressLine2
                                            oCardHolder.AddressLine3 = .AddressLine3
                                            oCardHolder.AddressLine4 = .AddressLine4
                                            oCardHolder.PostCode = .PostCode
                                            oCardHolder.CountryCode = .CountryCode
                                        End With
                                        oCreditCard.CardHolder = oCardHolder
                                    End If
                                End With
                                oPartyBankItem.CreditCard = oCreditCard
                            End If

                        End With
                        oPartyBankDetails(cntIndex) = oPartyBankItem
                    Next
                    oImpRequest.PartyBankDetails = oPartyBankDetails
                End If
            End With

            Try
                'Call the implementation method
                oImpResponse = oBusiness.UpdatePartyBankDetails(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'Get values from implementation layer
                With oImpResponse
                    oResponse.TimeStamp = .TimeStamp
                    Dim bHasErrors As Boolean = False
                    If .PartyBankStatus IsNot Nothing Then
                        If .PartyBankStatus IsNot Nothing AndAlso .PartyBankStatus.Count > 0 Then
                            oResponse.PartyBankStatus = .PartyBankStatus.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseUpdatePartyBankStatusType, BaseUpdatePartyBankStatusType)(AddressOf CommonFunctions.ToServiceUpdatePartyBankStatusList))
                            For cntIndex As Integer = 0 To oResponse.PartyBankStatus.Count - 1
                                If oResponse.PartyBankStatus(cntIndex).Errors IsNot Nothing Then
                                    bHasErrors = True
                                    Exit For
                                End If
                            Next
                        End If
                    End If
                    If bHasErrors Then
                        Dim oErrorCollection As SAMErrorCollection
                        oErrorCollection = New SAMErrorCollection
                        oErrorCollection.AddBusinessRule(SAMErrorCode.GeneralFailure, "Errors occurred during operation. Please check individual items to get more details.")
                        oErrorCollection.CheckForErrors()
                    End If
                End With
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(oUpdatePartyBankDetailsRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(oUpdatePartyBankDetailsRequest))
            Return Nothing
        End Try

    End Function

    Public Function GetClientDataExtract(ByVal GetClientDataExtractRequest As GetClientDataExtractRequestType) As GetClientDataExtractResponseType Implements IPurePartyService.GetClientDataExtract
        Try
            Dim sUserName As String = GetClientDataExtractRequest.LoginUserName
            Dim nAgentKey As Integer
            Dim nUserId As Integer

            CommonFunctions.GetIdentity(sUserName, nAgentKey, nUserId)
            CommonFunctions.CheckAuthority("SAMGUsrDet", nUserId)
            CommonFunctions.CheckSecurityToken(GetClientDataExtractRequest.WCFSecurityToken)

            Dim oResponse As New GetClientDataExtractResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, GetClientDataExtractRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetClientDataExtractRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetClientDataExtractResponseType = Nothing

            With GetClientDataExtractRequest
                oImpRequest.PartyCnt = .PartyCnt
                oImpRequest.FilePassword = .FilePassword
                oImpRequest.BranchCode = .BranchCode
            End With

            Try
                'Call the implementation method
                oImpResponse = oBusiness.GetClientDataExtract(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)

                'Get values from implementation layer
                With oImpResponse
                    oResponse.ClientDataFile = .ClientDataFile
                End With
            Catch ex As Exception
                CommonFunctions.BusinessLayerBoundary(oImpResponse, oResponse, ex, CommonFunctions.CreateDictionary(GetClientDataExtractRequest))
            End Try

            Return oResponse

        Catch ex As Exception
            CommonFunctions.BusinessLayerLastResort(ex, CommonFunctions.CreateDictionary(GetClientDataExtractRequest))
            Return Nothing
        End Try

    End Function
    Public Function CheckRetainedCoInsurerExists(ByVal oGetRetainedCoInsurerRequest As GetRetainedCoInsurerRequestType) As GetRetainedCoInsurerResponseType Implements IPurePartyService.CheckRetainedCoInsurerExists

        Try

            Dim sUserName As String = oGetRetainedCoInsurerRequest.LoginUserName
            Dim iAgentKey As Integer
            Dim iUserId As Integer
            CommonFunctions.GetIdentity(sUserName, iAgentKey, iUserId)
            CommonFunctions.CheckSecurityToken(oGetRetainedCoInsurerRequest.WCFSecurityToken)
            Dim oResponse As New GetRetainedCoInsurerResponseType
            Dim oBusiness As CoreSAMBusiness = New CoreSAMBusiness(sUserName, oGetRetainedCoInsurerRequest.BranchCode)

            ' Implementation structures
            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.GetRetainedCoInsurerRequestType
            Dim oImpResponse As SAMForInsuranceV2ImplementationTypes.GetRetainedCoInsurerResponseType = Nothing

            oImpRequest.CoInsurerKeys = oGetRetainedCoInsurerRequest.CoInsurerKeys
            Try
                ' Call the implementation method
                oImpResponse = oBusiness.CheckRetainedCoInsurerExists(oImpRequest)
                SAMErrorCollection.CheckForErrorsFromSTS(oImpResponse.STSError)
                If oImpResponse IsNot Nothing Then
                    oResponse.IsRetainedExists = oImpResponse.IsRetainedExists
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
End Class


