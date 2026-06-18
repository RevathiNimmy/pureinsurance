Imports System
Imports System.Configuration
Imports System.Web
Imports System.Web.Services.Protocols
Imports System.Xml
Imports SiriusFS.SAM.Structure
Imports SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2.WCF
Imports Sirius.Architecture.ExceptionHandling
Imports Sirius.Architecture.ExceptionHandling.Handler
Imports SiriusFS.SAM.CoreImplementation
Imports Sirius.Architecture.Data
Imports Sirius.Architecture.Utility
Imports Sirius.Architecture.Configuration.Database
Imports System.Xml.Serialization
Imports System.Linq
Imports Sirius.Architecture.Security
Imports System.Runtime.Remoting.Contexts
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling


''' <summary>
''' A class for common shared functions
''' </summary>
''' <remarks></remarks>
Public Class CommonFunctions

    ''' <summary>
    ''' To convert from Internal UserGroupList to WCF Service UserGroupList
    ''' </summary>
    ''' <param name="oUserGroupList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceUserGroupsList(ByVal oUserGroupList As SFI.SAMForInsuranceV2.BaseGetUserDetailsResponseTypeUserGroupsRow) As BaseGetUserDetailsResponseTypeRow

        Dim oServiceUserGroupList As New BaseGetUserDetailsResponseTypeRow

        If oUserGroupList IsNot Nothing Then
            oServiceUserGroupList.Code = oUserGroupList.Code
            oServiceUserGroupList.Description = oUserGroupList.Description
            oServiceUserGroupList.IsAssociated = oUserGroupList.IsAssociated
            oServiceUserGroupList.IsSupervisor = oUserGroupList.IsSupervisor
            oServiceUserGroupList.IsSystemAdmin = oUserGroupList.IsSystemAdmin
            oServiceUserGroupList.UserGroupKey = oUserGroupList.UserGroupKey
        End If

        Return oServiceUserGroupList

    End Function


    ''' <summary>
    ''' To convert from internal Source List to WCF Service Source List
    ''' </summary>
    ''' <param name="oSourceList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceSourceList(ByVal oSourceList As BaseImplementationTypes.BaseBranchType) As BaseBranchType

        Dim oServiceSourceList As BaseBranchType = New BaseBranchType

        If oSourceList IsNot Nothing Then
            oServiceSourceList.BranchCode = oSourceList.BranchCode
            oServiceSourceList.Description = oSourceList.Description
        End If

        Return oServiceSourceList

    End Function

    ''' <summary>
    ''' To convert from Internal AddressWithContactType to WCF Service AddressWithContactType
    ''' </summary>
    ''' <param name="oImplementation"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceBaseAddressWithContactsType(ByVal oImplementation As BaseImplementationTypes.BaseAddressType) As BaseAddressWithContactsType

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
                oService.CountryCode = oActualImplementation.CountryCode
                oService.PostCode = oActualImplementation.PostCode

                If oActualImplementation.Contacts IsNot Nothing Then
                    oService.Contacts = oActualImplementation.Contacts.ToList().ConvertAll( _
                                New Converter(Of BaseImplementationTypes.BaseContactType, BaseContactType) _
                                (AddressOf CommonFunctions.ToServiceContactType))
                End If
            Else
                oService.AddressLine1 = oImplementation.AddressLine1
                oService.AddressLine2 = oImplementation.AddressLine2
                oService.AddressLine3 = oImplementation.AddressLine3
                oService.AddressLine4 = oImplementation.AddressLine4
                oService.AddressTypeCode = CType([Enum].ToObject(GetType(BaseImplementationTypes.AddressTypeType), oImplementation.AddressTypeCode), AddressTypeType)
                oService.CountryCode = oImplementation.CountryCode
                oService.PostCode = oImplementation.PostCode
                oService.Contacts = Nothing
            End If
        End If

        Return oService

    End Function

    ''' <summary>
    ''' To convert from Internal ContactType to WCF Service ContactType
    ''' </summary>
    ''' <param name="oContact"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceContactType(ByVal oContact As BaseImplementationTypes.BaseContactType) As BaseContactType

        Dim oServiceContact As BaseContactType = New BaseContactType

        If oContact IsNot Nothing Then

            oServiceContact.AreaCode = oContact.AreaCode
            oServiceContact.Description = oContact.Description
            oServiceContact.ContactDetail = New BaseContactDetailType
            oServiceContact.ContactDetail.Item = oContact.ContactDetail.Item
            oServiceContact.ContactDetail.ItemElementName = CType([Enum].ToObject(GetType(BaseImplementationTypes.ItemChoiceType), oContact.ContactDetail.ItemElementName), ItemChoiceType)
            If oContact.ContactTypeCode <> BaseImplementationTypes.ContactTypeType.OTHER Then
                oServiceContact.ContactTypeCode = CType([Enum].ToObject(GetType(BaseImplementationTypes.ContactTypeType), oContact.ContactTypeCode), ContactTypeType)
            Else
                oServiceContact.ContactTypeCode = ContactTypeType.OTHER
                oServiceContact.OtherContactTypeCode = Trim(oContact.OtherContactTypeCode)
            End If
            oServiceContact.Extension = oContact.Extension
        End If

        Return oServiceContact

    End Function

    ''' <summary>
    ''' TO convert from InternalAssociateType to WCF Service AssociateType
    ''' </summary>
    ''' <param name="oAssociate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceAssociateType( _
            ByVal oAssociate As BaseImplementationTypes.BaseAssociateType) As BaseAssociateType

        Dim oServiceAssociate As BaseAssociateType = New BaseAssociateType

        If oAssociate IsNot Nothing Then

            oServiceAssociate.ClientKey = oAssociate.ClientKey
            oServiceAssociate.AssociateKey = oAssociate.AssociateKey
            oServiceAssociate.RelationshipCode = oAssociate.RelationshipCode
            oServiceAssociate.RelationshipDescription = oAssociate.RelationshipDescription

            oServiceAssociate.AssociateCode = oAssociate.AssociateCode
            oServiceAssociate.AssociateName = oAssociate.AssociateName

        End If

        Return oServiceAssociate

    End Function

    ''' <summary>
    ''' To convert from Internal ConvictionType to WCF Service ConvictionType
    ''' </summary>
    ''' <param name="oConviction"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceConvictionType( _
            ByVal oConviction As BaseImplementationTypes.BaseConvictionType) As BaseConvictionType

        Dim oServiceConviction As BaseConvictionType = New BaseConvictionType

        If oConviction IsNot Nothing Then

            oServiceConviction.AlcoholLevel = oConviction.AlcoholLevel
            oServiceConviction.AlcoholLevelSpecified = oConviction.AlcoholLevelSpecified
            oServiceConviction.AlcoholMeasurementMethod = oConviction.AlcoholMeasurementMethod
            oServiceConviction.ConvictionKey = oConviction.ConvictionKey
            oServiceConviction.Date = oConviction.Date
            oServiceConviction.Description = oConviction.Description
            oServiceConviction.DrivingLicensePenaltyPoints = oConviction.DrivingLicensePenaltyPoints
            oServiceConviction.DrivingLicensePenaltyPointsSpecified = oConviction.DrivingLicensePenaltyPointsSpecified
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

    ''' <summary>
    ''' To convert from internal LoyaltyScheme to WCF service LoyaltyScheme
    ''' </summary>
    ''' <param name="oLoyaltyScheme"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceLoyaltyScheme( _
                ByVal oLoyaltyScheme As BaseImplementationTypes.BaseClientSharedDataTypeLoyaltyScheme) As BaseClientSharedDataTypeLoyaltyScheme

        Dim oServiceLoyaltyScheme As BaseClientSharedDataTypeLoyaltyScheme = New BaseClientSharedDataTypeLoyaltyScheme

        If oLoyaltyScheme IsNot Nothing Then

            oServiceLoyaltyScheme.LoyaltySchemeKey = oLoyaltyScheme.LoyaltySchemeKey
            oServiceLoyaltyScheme.LoyaltySchemeCode = oLoyaltyScheme.LoyaltySchemeCode
            oServiceLoyaltyScheme.MembershipNumber = oLoyaltyScheme.MembershipNumber
            oServiceLoyaltyScheme.OtherReference = oLoyaltyScheme.OtherReference
            oServiceLoyaltyScheme.StartDate = oLoyaltyScheme.StartDate
            oServiceLoyaltyScheme.EndDate = oLoyaltyScheme.EndDate
            oServiceLoyaltyScheme.EndDateSpecified = oLoyaltyScheme.EndDateSpecified
            oServiceLoyaltyScheme.MainMember = oLoyaltyScheme.MainMember
            oServiceLoyaltyScheme.Active = oLoyaltyScheme.Active
            oServiceLoyaltyScheme.ActiveSpecified = oLoyaltyScheme.ActiveSpecified

        End If

        Return oServiceLoyaltyScheme

    End Function

    ''' <summary>
    ''' To convert from internal ProspectPolicies to WCF service ProspectPolicies
    ''' </summary>
    ''' <param name="oProspectPolicies"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceProspectPolicies( _
                ByVal oProspectPolicies As BaseImplementationTypes.BaseClientSharedDataTypeProspectPolicies) As BaseClientSharedDataTypeProspectPolicies

        Dim oServiceProspectPolicies As BaseClientSharedDataTypeProspectPolicies = New BaseClientSharedDataTypeProspectPolicies

        If oProspectPolicies IsNot Nothing Then

            oServiceProspectPolicies.ProspectPolicyKey = oProspectPolicies.ProspectPolicyKey
            oServiceProspectPolicies.ProspectTypeCode = oProspectPolicies.ProspectTypeCode
            oServiceProspectPolicies.RenewalDate = oProspectPolicies.RenewalDate
            oServiceProspectPolicies.RenewalDateSpecified = oProspectPolicies.RenewalDateSpecified
            oServiceProspectPolicies.TimesQuoted = oProspectPolicies.TimesQuoted
            oServiceProspectPolicies.TimesQuotedSpecified = oProspectPolicies.TimesQuotedSpecified
            oServiceProspectPolicies.TargetPremium = oProspectPolicies.TargetPremium
            oServiceProspectPolicies.TargetPremiumSpecified = oProspectPolicies.TargetPremiumSpecified

        End If

        Return oServiceProspectPolicies

    End Function

    ''' <summary>
    ''' To convert from Internal Accident to WCF service Accident
    ''' </summary>
    ''' <param name="oAccident"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceAccident(ByVal oAccident As BaseImplementationTypes.BasePartyOTHERTypeAccident) As BasePartyOTHERTypeAccident

        Dim oServiceAccident As BasePartyOTHERTypeAccident = New BasePartyOTHERTypeAccident

        If oAccident IsNot Nothing Then

            oServiceAccident.AccidentKey = oAccident.AccidentKey
            oServiceAccident.Date = oAccident.Date
            oServiceAccident.Description = oAccident.Description
            oServiceAccident.IsAtFault = oAccident.IsAtFault

        End If

        Return oServiceAccident

    End Function

    ''' <summary>
    ''' To convert from inetrnal SupplierBusiness to WCF Service SupplierBusiness
    ''' </summary>
    ''' <param name="oSupplierBusiness"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceSupplierBusiness(ByVal oSupplierBusiness As BaseImplementationTypes.BasePartyOTHERTypeSupplierBusiness) As BasePartyOTHERTypeSupplierBusiness

        Dim oServiceSupplierBusiness As BasePartyOTHERTypeSupplierBusiness = New BasePartyOTHERTypeSupplierBusiness

        If oSupplierBusiness IsNot Nothing Then

            oServiceSupplierBusiness.BusinessCode = oSupplierBusiness.BusinessCode
            oServiceSupplierBusiness.SpecialityCode = oSupplierBusiness.SpecialityCode

        End If

        Return oServiceSupplierBusiness

    End Function

    ''' <summary>
    ''' To convert from internal BaseClientSharedDataTypeLifeStyle to WCF service BaseClientSharedDataTypeLifeStyle
    ''' </summary>
    ''' <param name="oImplementation"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceBaseClientSharedDataTypeLifeStyle( _
            ByVal oImplementation As BaseImplementationTypes.BasePartyPCTypeLifestyle) As BasePartyPCTypeLifestyle

        Dim oService As New BasePartyPCTypeLifestyle

        If oImplementation IsNot Nothing Then

            oService.LifestyleKey = oImplementation.LifestyleKey
            oService.Name = oImplementation.Name
            oService.DateOfBirth = oImplementation.DateOfBirth
            oService.DateOfBirthSpecified = oImplementation.DateOfBirthSpecified
            oService.CategoryCode = oImplementation.CategoryCode
            If oImplementation.GenderCode = BaseImplementationTypes.GenderCodeType.Male Then
                oService.GenderCode = GenderCodeType.M
            Else
                oService.GenderCode = GenderCodeType.F
            End If
            oService.GenderCodeSpecified = oImplementation.GenderCodeSpecified
            oService.OccupationCode = oImplementation.OccupationCode
            oService.SecOccupationCode = oImplementation.SecOccupationCode
            oService.Smoker = oImplementation.Smoker
            oService.SmokerSpecified = oImplementation.SmokerSpecified

        End If

        Return oService

    End Function

    ''' <summary>
    ''' To convert from internal PartyList to WCF service PartyList
    ''' </summary>
    ''' <param name="oPartyList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServicePartyList(ByVal oPartyList As SFI.SAMForInsuranceV2.BaseFindPartyResponseTypePartiesRow) As BaseFindPartyResponseTypeRow

        Dim oServicePartyList As BaseFindPartyResponseTypeRow = New BaseFindPartyResponseTypeRow

        If oPartyList IsNot Nothing Then
            oServicePartyList.AddressLine1 = oPartyList.AddressLine1
            oServicePartyList.AddressLine2 = oPartyList.AddressLine2
            oServicePartyList.AgentKey = oPartyList.AgentKey
            oServicePartyList.AgentKeySpecified = oPartyList.AgentKeySpecified
            oServicePartyList.AgentType = oPartyList.AgentType
            oServicePartyList.AllowConsolidatedCommission = oPartyList.AllowConsolidatedCommission
            oServicePartyList.ContactTelephoneNumber = oPartyList.ContactTelephoneNumber
            oServicePartyList.DateOfBirth = oPartyList.DateOfBirth
            oServicePartyList.DateOfBirthSpecified = oPartyList.DateOfBirthSpecified
            oServicePartyList.FileCode = oPartyList.FileCode
            oServicePartyList.IsProspect = oPartyList.IsProspect
            oServicePartyList.IsRIBroker = oPartyList.IsRIBroker
            oServicePartyList.Name = oPartyList.Name
            oServicePartyList.PartyKey = oPartyList.PartyKey
            oServicePartyList.PartySourceDescription = oPartyList.PartySourceDescription
            oServicePartyList.PartySourceId = oPartyList.PartySourceId
            oServicePartyList.PostCode = oPartyList.PostCode
            oServicePartyList.ReinsuranceType = oPartyList.ReinsuranceType
            oServicePartyList.ResolvedName = oPartyList.ResolvedName
            oServicePartyList.ShortName = oPartyList.ShortName
            oServicePartyList.Status = oPartyList.Status
            oServicePartyList.SwiftLink = oPartyList.SwiftLink
            oServicePartyList.Type = oPartyList.Type
        End If

        Return oServicePartyList

    End Function

    ''' <summary>
    ''' To convert from WCF Service BaseConvictionTypes to Internal BaseConvictionTypes
    ''' </summary>
    ''' <param name="msgPartyConvictions"></param>
    ''' <param name="impPartyConvictions"></param>
    ''' <remarks></remarks>
    Public Shared Sub ToBaseImpBaseConvictionTypes(ByRef msgPartyConvictions As List(Of BaseConvictionType), _
                                                    ByRef impPartyConvictions As List(Of BaseImplementationTypes.BaseConvictionType))

        If (msgPartyConvictions) IsNot Nothing Then

            Dim msgConviction As BaseConvictionType
            Dim impConviction As BaseImplementationTypes.BaseConvictionType

            For iCnt As Integer = 0 To msgPartyConvictions.Count - 1

                msgConviction = msgPartyConvictions(iCnt)
                impConviction = New BaseImplementationTypes.BaseConvictionType

                impConviction.AlcoholLevel = msgConviction.AlcoholLevel
                impConviction.AlcoholLevelSpecified = msgConviction.AlcoholLevelSpecified
                impConviction.AlcoholMeasurementMethod = msgConviction.AlcoholMeasurementMethod
                impConviction.Date = msgConviction.Date
                impConviction.Description = msgConviction.Description
                impConviction.DrivingLicensePenaltyPoints = msgConviction.DrivingLicensePenaltyPoints
                impConviction.DrivingLicensePenaltyPointsSpecified = msgConviction.DrivingLicensePenaltyPointsSpecified
                impConviction.FineAmount = msgConviction.FineAmount
                impConviction.FineAmountSpecified = msgConviction.FineAmountSpecified
                impConviction.SentenceDescription = msgConviction.SentenceDescription
                impConviction.SentenceDuration = msgConviction.SentenceDuration
                impConviction.SentenceDurationSpecified = msgConviction.SentenceDurationSpecified
                impConviction.SentenceDurationQualifier = msgConviction.SentenceDurationQualifier
                impConviction.SentenceEffectiveDate = msgConviction.SentenceEffectiveDate
                impConviction.SentenceEffectiveDateSpecified = msgConviction.SentenceEffectiveDateSpecified
                impConviction.SentenceTypeCode = msgConviction.SentenceTypeCode
                impConviction.StatusCode = msgConviction.StatusCode
                impConviction.TypeCode = msgConviction.TypeCode

                impPartyConvictions.Add(impConviction)

            Next iCnt

        End If

    End Sub

    ''' <summary>
    ''' To convert from WCF Service BaseAssociateType to internal BaseAssociateType
    ''' </summary>
    ''' <param name="msgAssociate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToBaseImpBaseAssociateType(ByVal msgAssociate As SFI.SAMForInsuranceV2.WCF.BaseAssociateType) As BaseImplementationTypes.BaseAssociateType

        Dim impAssociate As BaseImplementationTypes.BaseAssociateType = New BaseImplementationTypes.BaseAssociateType

        impAssociate.ClientKey = msgAssociate.ClientKey
        impAssociate.AssociateKey = msgAssociate.AssociateKey
        impAssociate.RelationshipCode = msgAssociate.RelationshipCode
        impAssociate.RelationshipDescription = msgAssociate.RelationshipDescription

        Return impAssociate

    End Function

    ''' <summary>
    ''' To convert from WCF Service BaseClientSharedDataTypeLoyaltyScheme to internal BaseClientSharedDataTypeLoyaltyScheme
    ''' </summary>
    ''' <param name="oService"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToBaseImpBaseClientSharedDataTypeLoyaltyScheme( _
                ByVal oService As BaseClientSharedDataTypeLoyaltyScheme) As BaseImplementationTypes.BaseClientSharedDataTypeLoyaltyScheme

        Dim oImplementation As New BaseImplementationTypes.BaseClientSharedDataTypeLoyaltyScheme

        If oService IsNot Nothing Then

            oImplementation.LoyaltySchemeKey = oService.LoyaltySchemeKey
            oImplementation.LoyaltySchemeCode = oService.LoyaltySchemeCode
            oImplementation.MembershipNumber = oService.MembershipNumber
            oImplementation.OtherReference = oService.OtherReference
            oImplementation.StartDate = oService.StartDate
            oImplementation.EndDate = oService.EndDate
            oImplementation.EndDateSpecified = oService.EndDateSpecified
            oImplementation.MainMember = oService.MainMember
            oImplementation.Active = oService.Active
            oImplementation.ActiveSpecified = oService.ActiveSpecified

        End If

        Return oImplementation

    End Function

    ''' <summary>
    ''' To convert from WCF Service BaseClientSharedDataTypeProspectPolicies to internal BaseClientSharedDataTypeProspectPolicies
    ''' </summary>
    ''' <param name="oService"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToBaseImpBaseClientSharedDataTypeProspectPolicies( _
                ByVal oService As BaseClientSharedDataTypeProspectPolicies) As BaseImplementationTypes.BaseClientSharedDataTypeProspectPolicies

        Dim oImplementation As New BaseImplementationTypes.BaseClientSharedDataTypeProspectPolicies

        If oService IsNot Nothing Then

            oImplementation.ProspectPolicyKey = oService.ProspectPolicyKey
            oImplementation.ProspectTypeCode = oService.ProspectTypeCode
            oImplementation.RenewalDate = oService.RenewalDate
            oImplementation.RenewalDateSpecified = oService.RenewalDateSpecified
            oImplementation.TimesQuoted = oService.TimesQuoted
            oImplementation.TimesQuotedSpecified = oService.TimesQuotedSpecified
            oImplementation.TargetPremium = oService.TargetPremium
            oImplementation.TargetPremiumSpecified = oService.TargetPremiumSpecified

        End If

        Return oImplementation

    End Function

    ''' <summary>
    ''' To convert from WCF service BaseClientSharedDataTypeLifeStyle to internal BaseClientSharedDataTypeLifeStyle
    ''' </summary>
    ''' <param name="oService"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToBaseImpBaseClientSharedDataTypeLifeStyle( _
            ByVal oService As BasePartyPCTypeLifestyle) As BaseImplementationTypes.BasePartyPCTypeLifestyle

        Dim oImplementation As New BaseImplementationTypes.BasePartyPCTypeLifestyle

        If oService IsNot Nothing Then

            oImplementation.LifestyleKey = oService.LifestyleKey
            oImplementation.Name = oService.Name
            oImplementation.DateOfBirth = oService.DateOfBirth
            oImplementation.DateOfBirthSpecified = oService.DateOfBirthSpecified
            oImplementation.CategoryCode = oService.CategoryCode
            If oService.GenderCode = GenderCodeType.M Then
                oImplementation.GenderCode = BaseImplementationTypes.GenderCodeType.Male
            ElseIf oService.GenderCode = GenderCodeType.F Then
                oImplementation.GenderCode = BaseImplementationTypes.GenderCodeType.Female
            Else
                oImplementation.GenderCode = BaseImplementationTypes.GenderCodeType.NotApplicable
            End If
            oImplementation.GenderCodeSpecified = oService.GenderCodeSpecified
            oImplementation.OccupationCode = oService.OccupationCode
            oImplementation.SecOccupationCode = oService.SecOccupationCode
            oImplementation.Smoker = oService.Smoker
            oImplementation.SmokerSpecified = oService.SmokerSpecified

        End If

        Return oImplementation

    End Function

    ''' <summary>
    ''' To convert from WCF service BaseConvictionType to internal BaseConvictionType
    ''' </summary>
    ''' <param name="msgConviction"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToBaseImpBaseConvictionType( _
            ByVal msgConviction As BaseConvictionType) As BaseImplementationTypes.BaseConvictionType

        Dim impConviction As BaseImplementationTypes.BaseConvictionType = New BaseImplementationTypes.BaseConvictionType

        If msgConviction IsNot Nothing Then

            impConviction.AlcoholLevel = msgConviction.AlcoholLevel
            impConviction.AlcoholLevelSpecified = msgConviction.AlcoholLevelSpecified
            impConviction.AlcoholMeasurementMethod = msgConviction.AlcoholMeasurementMethod
            impConviction.ConvictionKey = msgConviction.ConvictionKey
            impConviction.Date = msgConviction.Date
            impConviction.Description = msgConviction.Description
            impConviction.DrivingLicensePenaltyPoints = msgConviction.DrivingLicensePenaltyPoints
            impConviction.DrivingLicensePenaltyPointsSpecified = msgConviction.DrivingLicensePenaltyPointsSpecified
            impConviction.FineAmount = msgConviction.FineAmount
            impConviction.FineAmountSpecified = msgConviction.FineAmountSpecified
            impConviction.SentenceDescription = msgConviction.SentenceDescription
            impConviction.SentenceDuration = msgConviction.SentenceDuration
            impConviction.SentenceDurationQualifier = msgConviction.SentenceDurationQualifier
            impConviction.SentenceDurationSpecified = msgConviction.SentenceDurationSpecified
            impConviction.SentenceEffectiveDate = msgConviction.SentenceEffectiveDate
            impConviction.SentenceEffectiveDateSpecified = msgConviction.SentenceEffectiveDateSpecified
            impConviction.SentenceTypeCode = msgConviction.SentenceTypeCode
            impConviction.StatusCode = msgConviction.StatusCode
            impConviction.TypeCode = msgConviction.TypeCode

        End If

        Return impConviction

    End Function

    ''' <summary>
    ''' TO check task authority for user
    ''' </summary>
    ''' <param name="sTaskCode"></param>
    ''' <param name="iUserKey"></param>
    ''' <remarks></remarks>
    Public Shared Sub CheckAuthority(ByVal sTaskCode As String, Optional ByVal iUserKey As Integer = 0)

        Try
            Using con As SiriusConnection = SiriusConnection.FromSirius()

                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAN_PMWrk_Task_check")

                    cmd.AddInParameter("@user_id", SqlDbType.SmallInt).Value = iUserKey
                    cmd.AddInParameter("@task_code", SqlDbType.NChar, 10).Value = sTaskCode
                    cmd.AddOutParameter("@exists", SqlDbType.Bit)

                    Dim ds As DataSet = con.ExecuteDataSet(cmd, "PMWrk_Task_check")

                    If IsDBNull(cmd.Parameters("@exists").Value) Then
                        Throw New AuthorisationException(sTaskCode)
                    Else
                        Dim bExists As Boolean = Cast.ToBoolean(cmd.Parameters("@exists").Value, False)
                        If bExists = False Then
                            Throw New AuthorisationException("CheckAuthority")
                        End If
                    End If

                End Using
            End Using

        Catch ex As Exception
            BusinessLayerLastResort(ex)
        End Try

    End Sub

    ''' <summary>
    ''' To get user detail
    ''' </summary>
    ''' <param name="o_sUserName"></param>
    ''' <param name="o_iAgentKey"></param>
    ''' <param name="o_iUserId"></param>
    ''' <remarks></remarks>
    Public Shared Sub GetIdentity(ByRef o_sUserName As String, ByRef o_iAgentKey As Integer, ByRef o_iUserId As Integer)

        Dim UserID As Integer = 0
        Dim LanguageID As Integer = 0
        Dim PartyCnt As Integer = 0
        Dim SAMErrorCollection As New SAMErrorCollection
        Try
            Using con As SiriusConnection = SiriusConnection.FromSirius()

                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_PMUser_check")

                    cmd.AddInParameter("@username", SqlDbType.NVarChar, 255).Value = Cast.NullIfDefault(o_sUserName)
                    cmd.AddOutParameter("@password", SqlDbType.NVarChar, 30)
                    cmd.AddOutParameter("@user_id", SqlDbType.SmallInt)
                    cmd.AddOutParameter("@language_id", SqlDbType.SmallInt)
                    cmd.AddOutParameter("@email_address", SqlDbType.NVarChar, 255)
                    cmd.AddOutParameter("@party_cnt", SqlDbType.Int)
                    cmd.AddOutParameter("@party_type_code", SqlDbType.NChar, 10)

                    Dim ds As DataSet = con.ExecuteDataSet(cmd, "UserDetails")

                    If IsDBNull(cmd.Parameters("@user_id").Value) Then
                        Throw New AuthorisationException(o_sUserName)
                    Else
                        o_iUserId = Cast.ToInt32(cmd.Parameters("@user_id").Value, 0)
                        o_iAgentKey = Cast.ToInt32(cmd.Parameters("@party_cnt").Value, 0)

                        If SiriusUserTableAccess.GetValueAsString(o_iAgentKey, "party_type_code", String.Empty).Trim <> PartyType.Agent Then
                            o_iAgentKey = 0
                        End If
                    End If

                End Using

            End Using
        Catch ex As Exception
            BusinessLayerLastResort(ex)
        End Try
    End Sub

    ''' <summary>
    ''' To convert from WCF service BasePartyOTHERTypeAccident to internal BasePartyOTHERTypeAccident
    ''' </summary>
    ''' <param name="oService"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToBaseImpBasePartyOTHERTypeAccident(ByVal oService As BasePartyOTHERTypeAccident) As BaseImplementationTypes.BasePartyOTHERTypeAccident

        Dim oImplementation As New BaseImplementationTypes.BasePartyOTHERTypeAccident

        If oService IsNot Nothing Then

            oImplementation.AccidentKey = oService.AccidentKey
            oImplementation.Date = oService.Date
            oImplementation.Description = oService.Description
            oImplementation.IsAtFault = oService.IsAtFault

        End If

        Return oImplementation

    End Function

    ''' <summary>
    ''' To convert from WCF service BasePartyOTHERTypeSupplierBusiness to internal 
    ''' </summary>
    ''' <param name="oService"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToBaseImpBasePartyOTHERTypeSupplierBusiness(
        ByVal oService As BasePartyOTHERTypeSupplierBusiness) As BaseImplementationTypes.BasePartyOTHERTypeSupplierBusiness

        Dim oImplementation As New BaseImplementationTypes.BasePartyOTHERTypeSupplierBusiness

        If oService IsNot Nothing Then

            oImplementation.BusinessCode = oService.BusinessCode
            oImplementation.SpecialityCode = oService.SpecialityCode

        End If

        Return oImplementation

    End Function

    ''' <summary>
    ''' To convert from WCF service to internal BaseAddressWithContactsType
    ''' </summary>
    ''' <param name="oService"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToBaseImpBaseAddressWithContactsType( _
            ByVal oService As BaseAddressWithContactsType) As BaseImplementationTypes.BaseAddressWithContactsType

        Dim oImplementation As New BaseImplementationTypes.BaseAddressWithContactsType

        If oService IsNot Nothing Then

            oImplementation.AddressLine1 = oService.AddressLine1
            oImplementation.AddressLine2 = oService.AddressLine2
            oImplementation.AddressLine3 = oService.AddressLine3
            oImplementation.AddressLine4 = oService.AddressLine4
            oImplementation.AddressTypeCode = CType([Enum].ToObject(GetType(AddressTypeType), oService.AddressTypeCode), BaseImplementationTypes.AddressTypeType)
            oImplementation.CountryCode = oService.CountryCode
            oImplementation.PostCode = oService.PostCode

            If oService.Contacts IsNot Nothing Then
                oImplementation.Contacts = Array.ConvertAll(oService.Contacts.ToArray(), _
                            New Converter(Of BaseContactType, BaseImplementationTypes.BaseContactType) _
                            (AddressOf ToBaseImpBaseContactType))
            End If

        End If

        Return oImplementation

    End Function

    ''' <summary>
    ''' To convert from WCF service BaseContactType to inernal BaseContactType
    ''' </summary>
    ''' <param name="msgContact"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToBaseImpBaseContactType(ByVal msgContact As BaseContactType) As BaseImplementationTypes.BaseContactType

        Dim impContact As BaseImplementationTypes.BaseContactType = New BaseImplementationTypes.BaseContactType

        If msgContact IsNot Nothing Then

            impContact.AreaCode = Trim(msgContact.AreaCode)
            impContact.Description = Trim(msgContact.Description)
            impContact.ContactDetail = New BaseImplementationTypes.BaseContactDetailType
            impContact.ContactDetail.Item = Trim(msgContact.ContactDetail.Item)
            If msgContact.ContactTypeCode <> BaseImplementationTypes.ContactTypeType.OTHER Then
                impContact.ContactTypeCode = CType([Enum].ToObject(GetType(ContactTypeType), msgContact.ContactTypeCode), BaseImplementationTypes.ContactTypeType)
            Else
                impContact.ContactTypeCode = BaseImplementationTypes.ContactTypeType.OTHER
                impContact.OtherContactTypeCode = Trim(msgContact.OtherContactTypeCode)
            End If

            impContact.Extension = Trim(msgContact.Extension)

        End If
        Return impContact

    End Function

    ''' <summary>
    ''' To convert from internal SAMError to WCF service SAMError
    ''' </summary>
    ''' <param name="oImplementation"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceSAMErrorType(ByVal oImplementation As BaseImplementationTypes.SAMError) As SFI.SAMForInsuranceV2.WCF.SAMError

        Select Case oImplementation.GetType()

            Case GetType(BaseImplementationTypes.SAMErrorBusinessRule)

                Dim oActualImplementation As BaseImplementationTypes.SAMErrorBusinessRule = CType(oImplementation, BaseImplementationTypes.SAMErrorBusinessRule)
                Dim oService As New SFI.SAMForInsuranceV2.WCF.SAMErrorBusinessRule
                oService.Code = oActualImplementation.Code
                oService.Description = oActualImplementation.Description
                oService.Detail = oActualImplementation.Detail
                Return oService

            Case GetType(BaseImplementationTypes.SAMErrorInvalidData)

                Dim oActualImplementation As BaseImplementationTypes.SAMErrorInvalidData = CType(oImplementation, BaseImplementationTypes.SAMErrorInvalidData)
                Dim oService As New SFI.SAMForInsuranceV2.WCF.SAMErrorInvalidData
                oService.Code = oActualImplementation.Code
                oService.Description = oActualImplementation.Description
                oService.FieldName = oActualImplementation.FieldName
                oService.Reason = oActualImplementation.Reason
                oService.SuppliedValue = oActualImplementation.SuppliedValue
                Return oService

            Case Else

                Dim oActualImplementation As BaseImplementationTypes.SAMErrorFatal = CType(oImplementation, BaseImplementationTypes.SAMErrorFatal)
                Dim oService As New SFI.SAMForInsuranceV2.WCF.SAMErrorFatal
                oService.Type = oActualImplementation.Type
                Return oService

        End Select

    End Function
    ''' <summary>
    ''' To return logged in user name from security token
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetUserName() As String
        'definition ned to be written after security implementation
        Return "sirius"

    End Function

    ''' <summary>
    ''' To throw fault contract to WCF service client
    ''' </summary>
    ''' <param name="ex"></param>
    ''' <remarks></remarks>
    Public Shared Sub BusinessLayerLastResort(ByVal ex As Exception)

        'Need to uncomment below line after getting a compatible function in error handler dll
        'Handler.BusinessLayerLastResort(ex, HttpContext.Current)
        Dim oException As New SFI.SAMForInsuranceV2.WCF.SAMMethodResponseData
        Dim oErrors As New List(Of SFI.SAMForInsuranceV2.WCF.SAMError)
        Dim oError As New SFI.SAMForInsuranceV2.WCF.WCFUnhandledError
        Dim handlingInstanceID As Guid

        handlingInstanceID = Guid.NewGuid()
        oError.Reason = SAMErrorCode.GeneralFailure
        oError.Description = ex.Message.ToString()
        oError.Detail = ex.StackTrace.ToString()
        oErrors.Add(oError)
        oException.HandlingInstanceID = handlingInstanceID
        oException.Errors = oErrors

        Throw New FaultException(Of SFI.SAMForInsuranceV2.WCF.SAMMethodResponseData)(oException, ex.Message.ToString())

    End Sub

    ''' <summary>
    ''' To set Errors collection in WCF service response
    ''' </summary>
    ''' <param name="oImpResponse"></param>
    ''' <param name="oResponse"></param>
    ''' <param name="ex"></param>
    ''' <remarks></remarks>
    Public Shared Sub BusinessLayerBoundary(ByVal oImpResponse As Object, ByRef oResponse As Object, ByVal ex As Exception)

        Handler.BusinessLayerBoundary(ex, oImpResponse)

        oResponse.Errors = CType(oImpResponse.Errors, BaseImplementationTypes.SAMError()).ToList().ConvertAll( _
                        New Converter(Of BaseImplementationTypes.SAMError, SFI.SAMForInsuranceV2.WCF.SAMError) _
                        (AddressOf ToServiceSAMErrorType))

    End Sub

    Public Shared Function ToServiceTaxesAndFeesTypeList(ByVal taxesAndFees As BaseImplementationTypes.BaseTaxesAndFeesType) As BaseTaxesAndFeesType

        Dim serviceTaxesAndFees As New BaseTaxesAndFeesType
        If taxesAndFees IsNot Nothing Then
            Dim oActualImplementation As BaseImplementationTypes.BaseTaxesAndFeesType
            oActualImplementation = TryCast(taxesAndFees, BaseImplementationTypes.BaseTaxesAndFeesType)

            If taxesAndFees.Fees IsNot Nothing Then

                serviceTaxesAndFees.Fees = oActualImplementation.Fees.ToList().ConvertAll( _
                                     New Converter(Of BaseImplementationTypes.BaseFeesType, BaseFeesType) _
                                                              (AddressOf CommonFunctions.ToServiceFeesType))
            End If

            If taxesAndFees.Taxes IsNot Nothing Then
                serviceTaxesAndFees.Taxes = oActualImplementation.Taxes.ToList().ConvertAll( _
                                   New Converter(Of BaseImplementationTypes.BaseTaxesType, BaseTaxesType) _
                                                            (AddressOf CommonFunctions.ToServiceTaxesType))
            End If

        End If

        Return serviceTaxesAndFees

    End Function

    Public Shared Function ToServiceFeesType(ByVal fees As BaseImplementationTypes.BaseFeesType) As BaseFeesType

        Dim serviceFees As BaseFeesType = New BaseFeesType

        If fees IsNot Nothing Then
            serviceFees.Amount = fees.Amount
            serviceFees.Description = fees.Description
        End If

        Return serviceFees

    End Function

    Public Shared Function ToServiceTaxesType(ByVal taxes As BaseImplementationTypes.BaseTaxesType) As BaseTaxesType

        Dim serviceTaxes As BaseTaxesType = New BaseTaxesType

        If taxes IsNot Nothing Then
            serviceTaxes.Amount = taxes.Amount
            serviceTaxes.Description = taxes.Description
        End If

        Return serviceTaxes
    End Function

    Public Shared Function ToServiceFindPolicy(ByVal oInsuranceFileDetailsRow As SFI.SAMForInsuranceV2.BaseFindPolicyResponseTypeInsuranceFileDetailsRow) As BaseFindPolicyResponseTypeRow

        Dim oServiceFindPolicy As BaseFindPolicyResponseTypeRow = New BaseFindPolicyResponseTypeRow

        If oInsuranceFileDetailsRow IsNot Nothing Then
            oServiceFindPolicy.ClientName = oInsuranceFileDetailsRow.ClientName
            oServiceFindPolicy.ClientShortName = oInsuranceFileDetailsRow.ClientShortName
            oServiceFindPolicy.CreatedDate = oInsuranceFileDetailsRow.CreatedDate
            oServiceFindPolicy.InsuranceFileKey = oInsuranceFileDetailsRow.InsuranceFileKey
            oServiceFindPolicy.InsuranceFileType = oInsuranceFileDetailsRow.InsuranceFileType
            oServiceFindPolicy.InsuranceFolderKey = oInsuranceFileDetailsRow.InsuranceFolderKey
            oServiceFindPolicy.InsuranceRef = oInsuranceFileDetailsRow.InsuranceRef
            oServiceFindPolicy.LastModifiedDate = oInsuranceFileDetailsRow.LastModifiedDate
            oServiceFindPolicy.PartyKey = oInsuranceFileDetailsRow.PartyKey
            oServiceFindPolicy.ProductCode = oInsuranceFileDetailsRow.ProductCode
            oServiceFindPolicy.ProductDescription = oInsuranceFileDetailsRow.ProductDescription
            oServiceFindPolicy.RiskIndex = oInsuranceFileDetailsRow.RiskIndex
            oServiceFindPolicy.Status = oInsuranceFileDetailsRow.Status
            oServiceFindPolicy.Value = oInsuranceFileDetailsRow.Value
        End If

        Return oServiceFindPolicy

    End Function

    Public Shared Function ToBaseImpBaseBankReceiptType( _
             ByVal oService As BaseBankReceiptType) As BaseImplementationTypes.BaseBankReceiptType

        Dim oImplementation As New BaseImplementationTypes.BaseBankReceiptType

        If oService IsNot Nothing Then

            oImplementation.BankCode = oService.BankCode
            oImplementation.ChequeDate = oService.ChequeDate
            oImplementation.PayerName = oService.PayerName
            oImplementation.BankBranch = oService.BankBranch
            oImplementation.BankLocation = oService.BankLocation
            oImplementation.ChequeClearingTypeCode = oService.ChequeClearingTypeCode
            oImplementation.ChequeDate = oService.ChequeDate
            oImplementation.ChequeTypeCode = oService.ChequeTypeCode

        End If

        Return oImplementation

    End Function

    Public Shared Function ToServiceFindAccount(ByVal oFindAccountDetailsRow As SFI.SAMForInsuranceV2.BaseFindAccountsResponseTypeAccountsRow) As BaseFindAccountsResponseTypeRow

        Dim oServiceFindAccount As BaseFindAccountsResponseTypeRow = New BaseFindAccountsResponseTypeRow

        If oFindAccountDetailsRow IsNot Nothing Then
            oServiceFindAccount.AccountBalance = oFindAccountDetailsRow.AccountBalance
            oServiceFindAccount.AccountKey = oFindAccountDetailsRow.AccountKey
            oServiceFindAccount.AccountName = oFindAccountDetailsRow.AccountName
            oServiceFindAccount.AccountStatus = oFindAccountDetailsRow.AccountStatus
            oServiceFindAccount.AccountStatusKey = oFindAccountDetailsRow.AccountStatusKey
            oServiceFindAccount.AccountTypeCode = oFindAccountDetailsRow.AccountTypeCode
            oServiceFindAccount.AccountTypeKey = oFindAccountDetailsRow.AccountTypeKey
            oServiceFindAccount.AddressLine1 = oFindAccountDetailsRow.AddressLine1
            oServiceFindAccount.CompanyKey = oFindAccountDetailsRow.CompanyKey
            oServiceFindAccount.ContactName = oFindAccountDetailsRow.ContactName
            oServiceFindAccount.CurrencyCode = oFindAccountDetailsRow.CurrencyCode
            oServiceFindAccount.CurrencyId = oFindAccountDetailsRow.CurrencyId
            oServiceFindAccount.FullKey = oFindAccountDetailsRow.FullKey
            oServiceFindAccount.LedgerCode = oFindAccountDetailsRow.LedgerCode
            oServiceFindAccount.LedgerKey = oFindAccountDetailsRow.LedgerKey
            oServiceFindAccount.NominalAccountKey = oFindAccountDetailsRow.NominalAccountKey
            oServiceFindAccount.PartyKey = oFindAccountDetailsRow.PartyKey
            oServiceFindAccount.PersonalClientForename = oFindAccountDetailsRow.PersonalClientForename
            oServiceFindAccount.ShortCode = oFindAccountDetailsRow.ShortCode
            oServiceFindAccount.SourceCode = oFindAccountDetailsRow.SourceCode
            oServiceFindAccount.SourceId = oFindAccountDetailsRow.SourceId
        End If

        Return oServiceFindAccount

    End Function

    Public Shared Function ToServiceFindBank(ByVal oFindBankDetailsRow As SFI.SAMForInsuranceV2.BaseFindBankResponseTypeBankRow) As BaseFindBankResponseTypeRow

        Dim oServiceFindBank As BaseFindBankResponseTypeRow = New BaseFindBankResponseTypeRow

        If oFindBankDetailsRow IsNot Nothing Then
            oServiceFindBank.BankAddress = oFindBankDetailsRow.BankAddress
            oServiceFindBank.BankKey = oFindBankDetailsRow.BankKey
            oServiceFindBank.BankName = oFindBankDetailsRow.BankName
            oServiceFindBank.BarnchCode = oFindBankDetailsRow.BarnchCode
            oServiceFindBank.Code = oFindBankDetailsRow.Code
            oServiceFindBank.HeadOffice = oFindBankDetailsRow.HeadOffice

        End If

        Return oServiceFindBank

    End Function

    Public Shared Function ToServiceFindCoverNoteBooks(ByVal oFindCoverNoteBooks As SFI.SAMForInsuranceV2.BaseFindCoverNoteBooksResponseTypeFindCoverNoteBooksRow) As BaseFindCoverNoteBooksResponseTypeRow

        Dim oServiceFindCoverNoteBooks As BaseFindCoverNoteBooksResponseTypeRow = New BaseFindCoverNoteBooksResponseTypeRow

        If oFindCoverNoteBooks IsNot Nothing Then
            oServiceFindCoverNoteBooks.AgentKey = oFindCoverNoteBooks.AgentKey
            oServiceFindCoverNoteBooks.AgentName = oFindCoverNoteBooks.AgentName
            oServiceFindCoverNoteBooks.BookNumber = oFindCoverNoteBooks.BookNumber
            oServiceFindCoverNoteBooks.CoverNoteBookKey = oFindCoverNoteBooks.CoverNoteBookKey
            oServiceFindCoverNoteBooks.CoverNoteBranchDescription = oFindCoverNoteBooks.CoverNoteBranchDescription
            oServiceFindCoverNoteBooks.CoverNoteBranchKey = oFindCoverNoteBooks.CoverNoteBranchKey
            oServiceFindCoverNoteBooks.CoverNoteStatusDescription = oFindCoverNoteBooks.CoverNoteStatusDescription
            oServiceFindCoverNoteBooks.CoverNoteStatusKey = oFindCoverNoteBooks.CoverNoteStatusKey
            oServiceFindCoverNoteBooks.DateCreated = oFindCoverNoteBooks.DateCreated
            oServiceFindCoverNoteBooks.EffectiveDate = oFindCoverNoteBooks.EffectiveDate
            oServiceFindCoverNoteBooks.EndNumber = oFindCoverNoteBooks.EndNumber
            oServiceFindCoverNoteBooks.LastUpdated = oFindCoverNoteBooks.LastUpdated
            oServiceFindCoverNoteBooks.StartNumber = oFindCoverNoteBooks.StartNumber
        End If

        Return oServiceFindCoverNoteBooks

    End Function

    ''' <summary>
    ''' To convert from internal BankGaurantee to WCF service BankGaurantee
    ''' </summary>
    ''' <param name="oBankGaurantee"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServicUpdateBankGuaranteeList(ByVal oBankGaurantee As SFI.SAMForInsuranceV2.BaseUpdateBankGuaranteeResponseTypeBankGuaranteeRow) As BaseUpdateBankGuaranteeResponseTypeRow

        Dim oServiceBankGaurantee As BaseUpdateBankGuaranteeResponseTypeRow = New BaseUpdateBankGuaranteeResponseTypeRow

        If oBankGaurantee IsNot Nothing Then
            oServiceBankGaurantee.BGKey = oBankGaurantee.BGKey
            oServiceBankGaurantee.BGTimeStamp = oBankGaurantee.BGTimeStamp
        End If

        Return oServiceBankGaurantee

    End Function
    Public Shared Function ToServiceMultiplePoliciesList(ByVal oMultiplePolicies As BaseImplementationTypes.BaseTransactResponseTypePolicyMultiplePolicies) As BaseTransactResponseTypePolicyMultiplePolicies

        Dim serviceMultiplePolicies As New BaseTransactResponseTypePolicyMultiplePolicies
        If serviceMultiplePolicies IsNot Nothing Then
            serviceMultiplePolicies.PremiumDueGross = oMultiplePolicies.PremiumDueGross
            serviceMultiplePolicies.PolicyRef = oMultiplePolicies.PolicyRef
            serviceMultiplePolicies.PremiumDueNet = oMultiplePolicies.PremiumDueNet
            serviceMultiplePolicies.PremiumDueTax = oMultiplePolicies.PremiumDueTax
            serviceMultiplePolicies.TotalAnnualTax = oMultiplePolicies.TotalAnnualTax
            serviceMultiplePolicies.CommissionAmount = oMultiplePolicies.CommissionAmount
            serviceMultiplePolicies.DocumentComment = oMultiplePolicies.DocumentComment
            serviceMultiplePolicies.AutoGeneratedPlanRef = oMultiplePolicies.AutoGeneratedPlanRef

        End If
        Return serviceMultiplePolicies

    End Function
    Public Shared Function ToServiceGetRiskByProductList(ByVal oGetRiskByProduct As SFI.SAMForInsuranceV2.BaseGetRiskByProductResponseTypeRisksRow) As BaseGetRiskByProductResponseTypeRow

        Dim serviceGetRiskByProduct As New BaseGetRiskByProductResponseTypeRow
        If oGetRiskByProduct IsNot Nothing Then
            serviceGetRiskByProduct.DataModelCode = oGetRiskByProduct.DataModelCode
            serviceGetRiskByProduct.Description = oGetRiskByProduct.Description
            serviceGetRiskByProduct.RiskTypeCode = oGetRiskByProduct.RiskTypeCode
            serviceGetRiskByProduct.RiskTypeKey = oGetRiskByProduct.RiskTypeKey
            serviceGetRiskByProduct.ScreenCode = oGetRiskByProduct.ScreenCode
            serviceGetRiskByProduct.ScreenKey = oGetRiskByProduct.ScreenKey
        End If
        Return serviceGetRiskByProduct

    End Function

    Public Shared Function ToServiceGetRatingSectionByRiskTypeList(ByVal oGetRatingSectionByRiskType As SFI.SAMForInsuranceV2.BaseGetRatingSectionByRiskTypeResponseTypeRatingSectionsRow) As BaseGetRatingSectionByRiskTypeResponseTypeRow

        Dim serviceGetRatingSectionByRiskType As New BaseGetRatingSectionByRiskTypeResponseTypeRow
        If oGetRatingSectionByRiskType IsNot Nothing Then

            serviceGetRatingSectionByRiskType.CountryID =
                serviceGetRatingSectionByRiskType.CurrencyID = oGetRatingSectionByRiskType.CurrencyID
            serviceGetRatingSectionByRiskType.Description = oGetRatingSectionByRiskType.Description
            serviceGetRatingSectionByRiskType.EarningPatternID = oGetRatingSectionByRiskType.EarningPatternID
            serviceGetRatingSectionByRiskType.Rate = oGetRatingSectionByRiskType.Rate
            serviceGetRatingSectionByRiskType.RateTypeID = oGetRatingSectionByRiskType.RateTypeID
            serviceGetRatingSectionByRiskType.RatingSectionCode = oGetRatingSectionByRiskType.RatingSectionCode
            serviceGetRatingSectionByRiskType.RatingSectionId = oGetRatingSectionByRiskType.RatingSectionId
            serviceGetRatingSectionByRiskType.StateID = oGetRatingSectionByRiskType.StateID
        End If
        Return serviceGetRatingSectionByRiskType

    End Function

    ''' <summary>
    ''' To convert from WCF Service oProductService to Internal oProductBase
    ''' </summary>
    ''' <param name="oProductService"></param>
    ''' <param name="oProductBase"></param>
    ''' <remarks></remarks>
    Public Shared Sub ToBaseImpBaseGetQuotesMarkedForCollection(ByRef oProductService As List(Of BaseGetQuotesMarkedForCollectionRequestTypeProducts), _
                                                    ByRef oProductBase As List(Of BaseImplementationTypes.BaseGetQuotesMarkedForCollectionRequestTypeProducts))

        If (oProductService) IsNot Nothing Then

            Dim msgProduct As BaseGetQuotesMarkedForCollectionRequestTypeProducts
            Dim impProduct As BaseImplementationTypes.BaseGetQuotesMarkedForCollectionRequestTypeProducts

            For iCnt As Integer = 0 To oProductService.Count - 1

                msgProduct = oProductService(iCnt)
                impProduct = New BaseImplementationTypes.BaseGetQuotesMarkedForCollectionRequestTypeProducts
                impProduct.ProductCode = msgProduct.ProductCode

                oProductBase.Add(impProduct)

            Next iCnt

        End If

    End Sub

    Public Shared Function ToServiceGetQuotesMarkedForCollectionList(ByVal oGetQuotesMarkedForCollection As SFI.SAMForInsuranceV2.BaseGetQuotesMarkedForCollectionResponseTypeMarkedQuotesRow) As BaseGetQuotesMarkedForCollectionResponseTypeRow

        Dim serviceGetQuotesMarkedForCollection As New BaseGetQuotesMarkedForCollectionResponseTypeRow
        If oGetQuotesMarkedForCollection IsNot Nothing Then
            With oGetQuotesMarkedForCollection
                serviceGetQuotesMarkedForCollection.AgentCommission = .AgentCommission
                serviceGetQuotesMarkedForCollection.AgentKey = .AgentKey
                serviceGetQuotesMarkedForCollection.AgentName = .AgentName
                serviceGetQuotesMarkedForCollection.AgentTypeCode = .AgentTypeCode
                serviceGetQuotesMarkedForCollection.BranchCode = .BranchCode
                serviceGetQuotesMarkedForCollection.BranchKey = .BranchKey
                serviceGetQuotesMarkedForCollection.CurrencyCode = .CurrencyCode
                serviceGetQuotesMarkedForCollection.CurrencyKey = .CurrencyKey
                serviceGetQuotesMarkedForCollection.InsuranceFileKey = .InsuranceFileKey
                serviceGetQuotesMarkedForCollection.InsuranceFileRef = .InsuranceFileRef
                serviceGetQuotesMarkedForCollection.InsuranceFileTypeCode = .InsuranceFileTypeCode
                serviceGetQuotesMarkedForCollection.PartyKey = .PartyKey
                serviceGetQuotesMarkedForCollection.PartyName = .PartyName
                serviceGetQuotesMarkedForCollection.Premium = .Premium
                serviceGetQuotesMarkedForCollection.ProductCode = .ProductCode
                serviceGetQuotesMarkedForCollection.ProductKey = .ProductKey
            End With

        End If
        Return serviceGetQuotesMarkedForCollection

    End Function

    Public Shared Function ToServiceGetProductRiskEventsList(ByVal oGetProductRiskEvents As SFI.SAMForInsuranceV2.BaseGetProductRiskEventsResponseTypeEventsRow) As BaseGetProductRiskEventsResponseTypeRow

        Dim serviceGetProductRiskEvents As New BaseGetProductRiskEventsResponseTypeRow
        If oGetProductRiskEvents IsNot Nothing Then
            With oGetProductRiskEvents
                serviceGetProductRiskEvents.EventCode = .EventCode
                serviceGetProductRiskEvents.EventDescription = .EventDescription
                serviceGetProductRiskEvents.EventKey = .EventKey
            End With
        End If
        Return serviceGetProductRiskEvents

    End Function

    Public Shared Function ToServiceGetInstalmentQuotesList(ByVal oGetInstalmentQuotes As SFI.SAMForInsuranceV2.BaseGetInstalmentQuotesResponseTypeQuotesRow) As BaseGetInstalmentQuotesResponseTypeRow

        Dim serviceGetInstalmentQuotes As New BaseGetInstalmentQuotesResponseTypeRow
        If oGetInstalmentQuotes IsNot Nothing Then
            With oGetInstalmentQuotes
                serviceGetInstalmentQuotes.AgentCnt = .AgentCnt
                serviceGetInstalmentQuotes.AgentRef = .AgentRef
                serviceGetInstalmentQuotes.AlignTo = .AlignTo
                serviceGetInstalmentQuotes.AprRate = .AprRate
                serviceGetInstalmentQuotes.BankAddressMandatory = .BankAddressMandatory
                serviceGetInstalmentQuotes.BankNameMandatory = .BankNameMandatory
                serviceGetInstalmentQuotes.BranchCodeMandatory = .BranchCodeMandatory
                serviceGetInstalmentQuotes.BranchNameMandatory = .BranchNameMandatory
                serviceGetInstalmentQuotes.BrokerID = .BrokerID
                serviceGetInstalmentQuotes.BrokerURL = .BrokerURL
                serviceGetInstalmentQuotes.ClaimDebtID = .ClaimDebtID
                serviceGetInstalmentQuotes.CompanyName = .CompanyName
                serviceGetInstalmentQuotes.CompanyNo = .CompanyNo
                serviceGetInstalmentQuotes.DaysDelay = .DaysDelay
                serviceGetInstalmentQuotes.DepositAmount = .DepositAmount
                serviceGetInstalmentQuotes.DepositAsInstalment = .DepositAsInstalment
                serviceGetInstalmentQuotes.FinanceCharge = .FinanceCharge
                serviceGetInstalmentQuotes.FirstInstalmentAmount = .FirstInstalmentAmount
                serviceGetInstalmentQuotes.FirstInstalmentDate = .FirstInstalmentDate
                serviceGetInstalmentQuotes.FrequencyAmount = .FrequencyAmount
                serviceGetInstalmentQuotes.FrequencyDescription = .FrequencyDescription
                serviceGetInstalmentQuotes.FrequencyID = .FrequencyID
                serviceGetInstalmentQuotes.FrequencyPeriod = .FrequencyPeriod
                serviceGetInstalmentQuotes.FrequencyPerYear = .FrequencyPerYear
                serviceGetInstalmentQuotes.HighlightCell = .HighlightCell
                serviceGetInstalmentQuotes.InstalmentsToPay = .InstalmentsToPay
                serviceGetInstalmentQuotes.InterestAmount = .InterestAmount
                serviceGetInstalmentQuotes.InterestRate = .InterestRate
                serviceGetInstalmentQuotes.LastInstalmentAmount = .LastInstalmentAmount
                serviceGetInstalmentQuotes.LastInstalmentDate = .LastInstalmentDate
                serviceGetInstalmentQuotes.MediaTypeDescription = .MediaTypeDescription
                serviceGetInstalmentQuotes.MediaTypeID = .MediaTypeID
                serviceGetInstalmentQuotes.MediaTypeValidation = .MediaTypeValidation
                serviceGetInstalmentQuotes.MinMTA = .MinMTA
                serviceGetInstalmentQuotes.NextInstalmentDate = .NextInstalmentDate
                serviceGetInstalmentQuotes.OriginalAmount = .OriginalAmount
                serviceGetInstalmentQuotes.OriginalOtherInstalmentAmount = .OriginalOtherInstalmentAmount
                serviceGetInstalmentQuotes.OriginalRate = .OriginalRate
                serviceGetInstalmentQuotes.OtherInstalmentAmount = .OtherInstalmentAmount
                serviceGetInstalmentQuotes.Password = .Password
                serviceGetInstalmentQuotes.PFRF_ID = .PFRF_ID
                serviceGetInstalmentQuotes.ProductClass = .ProductClass
                serviceGetInstalmentQuotes.ProductCode = .ProductCode
                serviceGetInstalmentQuotes.ProtectionAmount = .ProtectionAmount
                serviceGetInstalmentQuotes.ProviderCode = .ProviderCode
                serviceGetInstalmentQuotes.Ref = .Ref
                serviceGetInstalmentQuotes.RefundType = .RefundType
                serviceGetInstalmentQuotes.SchemeName = .SchemeName
                serviceGetInstalmentQuotes.SchemeNo = .SchemeNo
                serviceGetInstalmentQuotes.SchemeTypeCode = .SchemeTypeCode
                serviceGetInstalmentQuotes.SchemeVersion = .SchemeVersion
                serviceGetInstalmentQuotes.StartLimit = .StartLimit
                serviceGetInstalmentQuotes.TaxAmount = .TaxAmount
                serviceGetInstalmentQuotes.Terms = .Terms
                serviceGetInstalmentQuotes.Timeout = .Timeout
                serviceGetInstalmentQuotes.TotalAmountInput = .TotalAmountInput
                serviceGetInstalmentQuotes.TotalInstalmentsAmount = .TotalInstalmentsAmount
                serviceGetInstalmentQuotes.UserID = .UserID
                serviceGetInstalmentQuotes.Username = .Username

            End With
        End If
        Return serviceGetInstalmentQuotes

    End Function

    Public Shared Function ToServiceGetDefaultRiskClausesList(ByVal oGetDefaultRiskClauses As BaseImplementationTypes.BaseGetDefaultRiskClausesResponseTypeDocumentsRow) As BaseGetDefaultRiskClausesResponseTypeRow

        Dim serviceGetDefaultRiskClauses As New BaseGetDefaultRiskClausesResponseTypeRow
        If oGetDefaultRiskClauses IsNot Nothing Then
            With oGetDefaultRiskClauses
                serviceGetDefaultRiskClauses.Code = .Code
                serviceGetDefaultRiskClauses.Description = .Description
            End With
        End If
        Return serviceGetDefaultRiskClauses

    End Function

    Public Shared Function ToServiceGetBackdatedMTARiskVersionsList(ByVal oGetBackdatedMTARiskVersions As SFI.SAMForInsuranceV2.BaseAddBackDatedMTAQuoteResponseTypeBackdatedTransactionsRow) As BaseAddBackDatedMTAQuoteResponseTypeRow

        Dim serviceGetBackdatedMTARiskVersions As New BaseAddBackDatedMTAQuoteResponseTypeRow
        If oGetBackdatedMTARiskVersions IsNot Nothing Then
            With oGetBackdatedMTARiskVersions
                oGetBackdatedMTARiskVersions.CoverEndDate = .CoverEndDate
                oGetBackdatedMTARiskVersions.CoverStartDate = .CoverStartDate
                oGetBackdatedMTARiskVersions.CurrencyCode = .CurrencyCode
                oGetBackdatedMTARiskVersions.GISScreenID = .GISScreenID
                oGetBackdatedMTARiskVersions.InsuranceFileCnt = .InsuranceFileCnt
                oGetBackdatedMTARiskVersions.InsuranceFolderCnt = .InsuranceFolderCnt
                oGetBackdatedMTARiskVersions.MTAPremium = .MTAPremium
                oGetBackdatedMTARiskVersions.PartyCnt = .PartyCnt
                oGetBackdatedMTARiskVersions.PartyShortname = .PartyShortname
                oGetBackdatedMTARiskVersions.PolicyType = .PolicyType
                oGetBackdatedMTARiskVersions.PolicyVersion = .PolicyVersion
                oGetBackdatedMTARiskVersions.ProductID = .ProductID
                oGetBackdatedMTARiskVersions.QuoteStatus = .QuoteStatus
                oGetBackdatedMTARiskVersions.RiskCnt = .RiskCnt
                oGetBackdatedMTARiskVersions.RiskDescription = .RiskDescription
                oGetBackdatedMTARiskVersions.RiskTypeDescription = .RiskTypeDescription
                oGetBackdatedMTARiskVersions.RiskTypeID = .RiskTypeID
                oGetBackdatedMTARiskVersions.Status = .Status
            End With
        End If
        Return serviceGetBackdatedMTARiskVersions

    End Function

    Public Shared Sub ToBaseImpBaseBindQuoteCreditTransactions(ByRef oCreditTransactionsService As List(Of BaseBindQuoteRequestTypeCreditTransactionsRow), _
                                                   ByRef oCreditTransactionsBase As List(Of BaseImplementationTypes.BaseBindQuoteRequestTypeCreditTransactionsRow))

        If (oCreditTransactionsService) IsNot Nothing Then

            Dim msgCreditTransactions As BaseBindQuoteRequestTypeCreditTransactionsRow
            Dim impCreditTransactions As BaseImplementationTypes.BaseBindQuoteRequestTypeCreditTransactionsRow

            For iCnt As Integer = 0 To oCreditTransactionsService.Count - 1

                msgCreditTransactions = oCreditTransactionsService(iCnt)
                impCreditTransactions = New BaseImplementationTypes.BaseBindQuoteRequestTypeCreditTransactionsRow
                impCreditTransactions.AccountKey = msgCreditTransactions.AccountKey
                impCreditTransactions.Amount = msgCreditTransactions.Amount
                impCreditTransactions.CollectionDate = msgCreditTransactions.CollectionDate
                impCreditTransactions.TransDetailKey = msgCreditTransactions.TransDetailKey

                oCreditTransactionsBase.Add(impCreditTransactions)

            Next iCnt

        End If

    End Sub

    Public Shared Function ToServiceWarningTypeList(ByVal oServiceWarningType As BaseImplementationTypes.BaseGeneralWarningResponseType) As BaseGeneralWarningResponseType

        Dim serviceServiceWarningType As New BaseGeneralWarningResponseType
        If oServiceWarningType IsNot Nothing Then
            With oServiceWarningType
                serviceServiceWarningType.Code = .Code
                serviceServiceWarningType.Description = .Description
            End With
        End If
        Return serviceServiceWarningType

    End Function

    Public Shared Sub ToBaseImpBaseUpdateTaxes(ByRef oUpdateTaxesService As List(Of BaseUpdateTaxesRequestTypeRow), _
                                                   ByRef oUpdateTaxesBase As List(Of BaseImplementationTypes.BaseUpdateTaxesRequestTypeRow))

        If (oUpdateTaxesService) IsNot Nothing Then

            Dim msgoUpdateTaxes As BaseUpdateTaxesRequestTypeRow
            Dim impoUpdateTaxes As BaseImplementationTypes.BaseUpdateTaxesRequestTypeRow

            For iCnt As Integer = 0 To oUpdateTaxesService.Count - 1

                msgoUpdateTaxes = oUpdateTaxesService(iCnt)

                impoUpdateTaxes = New BaseImplementationTypes.BaseUpdateTaxesRequestTypeRow
                impoUpdateTaxes.IsEdited = impoUpdateTaxes.IsEdited
                impoUpdateTaxes.IsValue = impoUpdateTaxes.IsValue
                impoUpdateTaxes.TaxCalculationKey = impoUpdateTaxes.TaxCalculationKey
                impoUpdateTaxes.TaxPercentage = impoUpdateTaxes.TaxPercentage
                impoUpdateTaxes.TaxValue = impoUpdateTaxes.TaxValue

                oUpdateTaxesBase.Add(impoUpdateTaxes)

            Next iCnt

        End If


    End Sub

    Public Shared Function ToServiceGetDocumentDefaultsList(ByVal oGetDocumentDefaultsList As BaseImplementationTypes.BaseGetDocumentDefaultsResponseTypeDocumentTemplates) As BaseGetDocumentDefaultsResponseTypeDocumentTemplates
        Dim oServiceGetDocumentDefaultList As New BaseGetDocumentDefaultsResponseTypeDocumentTemplates
        If (oGetDocumentDefaultsList) IsNot Nothing Then
            With oGetDocumentDefaultsList
                oServiceGetDocumentDefaultList.DocumentGroupCode = .DocumentGroupCode
                oServiceGetDocumentDefaultList.DocumentGroupDescription = .DocumentGroupDescription
                oServiceGetDocumentDefaultList.DocumentGroupID = .DocumentGroupID
                oServiceGetDocumentDefaultList.DocumentSubGroupCode = .DocumentSubGroupCode
                oServiceGetDocumentDefaultList.DocumentSubGroupDescription = .DocumentSubGroupDescription
                oServiceGetDocumentDefaultList.DocumentSubGroupID = .DocumentSubGroupID
                oServiceGetDocumentDefaultList.DocumentTemplateCode = .DocumentTemplateCode
                oServiceGetDocumentDefaultList.DocumentTemplateDescription = .DocumentTemplateDescription
                oServiceGetDocumentDefaultList.DocumentTemplateID = .DocumentTemplateID
                oServiceGetDocumentDefaultList.InternalOnly = .InternalOnly
                oServiceGetDocumentDefaultList.SelectedByDefault = .SelectedByDefault

            End With
        End If

        Return oServiceGetDocumentDefaultList

    End Function

    Public Shared Function ToServiceGetCoverNoteBook(ByVal oGetCoverNoteBook As SFI.SAMForInsuranceV2.BaseGetCoverNoteBookResponseTypeCoverNoteBookProductsRow) As BaseGetCoverNoteBookResponseTypeRow
        Dim oServiceGetCoverNoteBook As New BaseGetCoverNoteBookResponseTypeRow
        If (oGetCoverNoteBook) IsNot Nothing Then
            oServiceGetCoverNoteBook.Chosen = oGetCoverNoteBook.Chosen
            oServiceGetCoverNoteBook.Description = oGetCoverNoteBook.Chosen
            oServiceGetCoverNoteBook.ProductCode = oGetCoverNoteBook.Chosen
            oServiceGetCoverNoteBook.ProductKey = oGetCoverNoteBook.Chosen
        End If

        Return oServiceGetCoverNoteBook

    End Function


    Public Shared Function ToServiceGetCoverNoteSheets(ByVal oGetCoverNoteSheets As SFI.SAMForInsuranceV2.BaseGetCoverNoteBookResponseTypeCoverNoteSheetsRow) As BaseGetCoverNoteBookResponseTypeRow1
        Dim oServiceGetCoverNoteSheets As New BaseGetCoverNoteBookResponseTypeRow1
        If (oGetCoverNoteSheets) IsNot Nothing Then
            With oServiceGetCoverNoteSheets
                oServiceGetCoverNoteSheets.AgentName = oGetCoverNoteSheets.AgentName
                oServiceGetCoverNoteSheets.BranchName = oGetCoverNoteSheets.BranchName
                oServiceGetCoverNoteSheets.CoverNoteSheetKey = oGetCoverNoteSheets.CoverNoteSheetKey
                oServiceGetCoverNoteSheets.CoverNoteSheetNumber = oGetCoverNoteSheets.CoverNoteSheetNumber
                oServiceGetCoverNoteSheets.CoverNoteSheetStatusCode = oGetCoverNoteSheets.CoverNoteSheetStatusCode
                oServiceGetCoverNoteSheets.CoverNoteSheetStatusDescription = oGetCoverNoteSheets.CoverNoteSheetStatusDescription
                oServiceGetCoverNoteSheets.CoverNoteSheetStatusKey = oGetCoverNoteSheets.CoverNoteSheetStatusKey
                oServiceGetCoverNoteSheets.CoverNoteSheetStatusKeySpecified = oGetCoverNoteSheets.CoverNoteSheetStatusKeySpecified
                oServiceGetCoverNoteSheets.CustomerName = oGetCoverNoteSheets.CustomerName
                oServiceGetCoverNoteSheets.DateImported = oGetCoverNoteSheets.DateImported
                oServiceGetCoverNoteSheets.PolicyNumber = oGetCoverNoteSheets.PolicyNumber
            End With

        End If

        Return oServiceGetCoverNoteSheets

    End Function


    Public Shared Function ToServiceGenerateDocumentsForEvent(ByVal oGenerateDocumentsForEvent As BaseImplementationTypes.BaseGenerateDocumentsForEventResponseDocument) As BaseGenerateDocumentsForEventResponseDocument
        Dim oServiceGenerateDocumentsForEvent As New BaseGenerateDocumentsForEventResponseDocument
        If (oGenerateDocumentsForEvent) IsNot Nothing Then
            With oGenerateDocumentsForEvent



                oServiceGenerateDocumentsForEvent.DocumentTemplateCode = oGenerateDocumentsForEvent.DocumentTemplateCode
                oServiceGenerateDocumentsForEvent.DocumentTemplateDescription = oGenerateDocumentsForEvent.DocumentTemplateDescription

            End With

        End If

        Return oServiceGenerateDocumentsForEvent

    End Function




    Public Shared Function ToServiceFindUsers(ByVal oFindUsers As SFI.SAMForInsuranceV2.BaseFindUsersResponseTypeUsersRow) As BaseFindUsersResponseTypeRow

        Dim oServiceFindUsers As BaseFindUsersResponseTypeRow = New BaseFindUsersResponseTypeRow

        If oFindUsers IsNot Nothing Then
            oServiceFindUsers.EffectiveDate = oFindUsers.EffectiveDate
            oServiceFindUsers.FullName = oFindUsers.FullName
            oServiceFindUsers.UserId = oFindUsers.UserId
            oServiceFindUsers.UserName = oFindUsers.UserName
        End If

        Return oServiceFindUsers

    End Function

    Public Shared Function ToServiceFindDocumentTemplates(ByVal oFindDocumentTemplates As SFI.SAMForInsuranceV2.BaseFindDocumentTemplatesResponseTypeDocumentTemplatesRow) As BaseFindDocumentTemplatesResponseTypeRow

        Dim oServiceFindDocumentTemplates As BaseFindDocumentTemplatesResponseTypeRow = New BaseFindDocumentTemplatesResponseTypeRow

        If oFindDocumentTemplates IsNot Nothing Then
            oServiceFindDocumentTemplates.Code = oFindDocumentTemplates.Code
            oServiceFindDocumentTemplates.Description = oFindDocumentTemplates.Description
            oServiceFindDocumentTemplates.DocumentTemplateKey = oFindDocumentTemplates.DocumentTemplateKey
            oServiceFindDocumentTemplates.EffectiveDate = oFindDocumentTemplates.EffectiveDate
            oServiceFindDocumentTemplates.Type = oFindDocumentTemplates.Type
        End If

        Return oServiceFindDocumentTemplates

    End Function

    Public Shared Function ToServiceGetClaimCoinsurer(ByVal oGetClaimCoinsurer As SFI.SAMForInsuranceV2.BaseGetClaimCoinsurerResponseTypeCoinsurersRow) As BaseGetClaimCoinsurerResponseTypeRow

        Dim oServiceGetClaimCoinsurer As BaseGetClaimCoinsurerResponseTypeRow = New BaseGetClaimCoinsurerResponseTypeRow

        If oGetClaimCoinsurer IsNot Nothing Then
            oServiceGetClaimCoinsurer.Name = oGetClaimCoinsurer.Name
            oServiceGetClaimCoinsurer.PartyKey = oGetClaimCoinsurer.Name
            oServiceGetClaimCoinsurer.Share = oGetClaimCoinsurer.Name
            oServiceGetClaimCoinsurer.ShareValue = oGetClaimCoinsurer.Name
        End If

        Return oServiceGetClaimCoinsurer

    End Function

    Public Shared Function ToServiceGetVersionsForClaim(ByVal oGetVersionsForClaim As SFI.SAMForInsuranceV2.BaseGetVersionsForClaimResponseTypeVersionsRow) As BaseGetVersionsForClaimResponseTypeRow

        Dim oServiceGetVersionsForClaim As BaseGetVersionsForClaimResponseTypeRow = New BaseGetVersionsForClaimResponseTypeRow

        If oGetVersionsForClaim IsNot Nothing Then
            oServiceGetVersionsForClaim.claim_number = oGetVersionsForClaim.claim_number
            oServiceGetVersionsForClaim.ClaimDescription = oGetVersionsForClaim.ClaimDescription
            oServiceGetVersionsForClaim.ClaimKey = oGetVersionsForClaim.ClaimKey
            oServiceGetVersionsForClaim.client_short_name = oGetVersionsForClaim.client_short_name
            oServiceGetVersionsForClaim.CurrentReserve = oGetVersionsForClaim.CurrentReserve
            oServiceGetVersionsForClaim.InsuranceFileKey = oGetVersionsForClaim.InsuranceFileKey
            oServiceGetVersionsForClaim.InsuranceFolderKey = oGetVersionsForClaim.InsuranceFolderKey
            oServiceGetVersionsForClaim.InsuranceHolderShortName = oGetVersionsForClaim.InsuranceHolderShortName
            oServiceGetVersionsForClaim.InsuranceRef = oGetVersionsForClaim.InsuranceRef
            oServiceGetVersionsForClaim.loss_from_date = oGetVersionsForClaim.loss_from_date
            oServiceGetVersionsForClaim.LossCurrency = oGetVersionsForClaim.LossCurrency
            oServiceGetVersionsForClaim.PolicyCurrency = oGetVersionsForClaim.PolicyCurrency
            oServiceGetVersionsForClaim.RiskKey = oGetVersionsForClaim.RiskKey
            oServiceGetVersionsForClaim.ThisPayment = oGetVersionsForClaim.ThisPayment
            oServiceGetVersionsForClaim.ThisRevision = oGetVersionsForClaim.ThisRevision
            oServiceGetVersionsForClaim.ThisSalvageRecovery = oGetVersionsForClaim.ThisSalvageRecovery
            oServiceGetVersionsForClaim.ThisThirdPartyRecovery = oGetVersionsForClaim.ThisThirdPartyRecovery
            oServiceGetVersionsForClaim.TotalIncurred = oGetVersionsForClaim.TotalIncurred
            oServiceGetVersionsForClaim.TotalPaid = oGetVersionsForClaim.TotalPaid
            oServiceGetVersionsForClaim.TransactionDate = oGetVersionsForClaim.TransactionDate
            oServiceGetVersionsForClaim.TransactionType = oGetVersionsForClaim.TransactionType
            oServiceGetVersionsForClaim.User = oGetVersionsForClaim.User
            oServiceGetVersionsForClaim.Version = oGetVersionsForClaim.Version
            oServiceGetVersionsForClaim.VersionDescription = oGetVersionsForClaim.VersionDescription
        End If

        Return oServiceGetVersionsForClaim

    End Function

    Public Shared Function ToServiceGetUnallocatedClaimPayment(ByVal oGetUnallocatedClaimPayment As SFI.SAMForInsuranceV2.BaseGetUnallocatedClaimPaymentsResponseTypeUnallocatedClaimPaymentsRow) As BaseGetUnallocatedClaimPaymentsResponseTypeRow

        Dim oServiceGetUnallocatedClaimPayment As BaseGetUnallocatedClaimPaymentsResponseTypeRow = New BaseGetUnallocatedClaimPaymentsResponseTypeRow

        If oGetUnallocatedClaimPayment IsNot Nothing Then
            oServiceGetUnallocatedClaimPayment.AccountAmount = oGetUnallocatedClaimPayment.AccountAmount
            oServiceGetUnallocatedClaimPayment.AccountCurrencyKey = oGetUnallocatedClaimPayment.AccountCurrencyKey
            oServiceGetUnallocatedClaimPayment.AccountKey = oGetUnallocatedClaimPayment.AccountKey
            oServiceGetUnallocatedClaimPayment.AccountName = oGetUnallocatedClaimPayment.AccountName
            oServiceGetUnallocatedClaimPayment.Amount = oGetUnallocatedClaimPayment.Amount
            oServiceGetUnallocatedClaimPayment.AmountCurrencyKey = oGetUnallocatedClaimPayment.AmountCurrencyKey
            oServiceGetUnallocatedClaimPayment.BaseClaimPaymentKey = oGetUnallocatedClaimPayment.BaseClaimPaymentKey
            oServiceGetUnallocatedClaimPayment.BaseCurrencyDescription = oGetUnallocatedClaimPayment.BaseCurrencyDescription
            oServiceGetUnallocatedClaimPayment.BaseCurrencyFormatString = oGetUnallocatedClaimPayment.BaseCurrencyFormatString
            oServiceGetUnallocatedClaimPayment.ClaimNumber = oGetUnallocatedClaimPayment.ClaimNumber
            oServiceGetUnallocatedClaimPayment.CurrencyAmount = oGetUnallocatedClaimPayment.CurrencyAmount
            oServiceGetUnallocatedClaimPayment.CurrencyDescription = oGetUnallocatedClaimPayment.CurrencyDescription
            oServiceGetUnallocatedClaimPayment.CurrencyFormatString = oGetUnallocatedClaimPayment.CurrencyFormatString
            oServiceGetUnallocatedClaimPayment.CurrencyKey = oGetUnallocatedClaimPayment.CurrencyKey
            oServiceGetUnallocatedClaimPayment.DateOfPayment = oGetUnallocatedClaimPayment.DateOfPayment
            oServiceGetUnallocatedClaimPayment.DocumentComment = oGetUnallocatedClaimPayment.DocumentComment
            oServiceGetUnallocatedClaimPayment.DocumentDate = oGetUnallocatedClaimPayment.DocumentDate
            oServiceGetUnallocatedClaimPayment.DocumentKey = oGetUnallocatedClaimPayment.DocumentKey
            oServiceGetUnallocatedClaimPayment.DocumentRef = oGetUnallocatedClaimPayment.DocumentRef
            oServiceGetUnallocatedClaimPayment.MaxClaimPaymentKey = oGetUnallocatedClaimPayment.MaxClaimPaymentKey
            oServiceGetUnallocatedClaimPayment.PayeeMediaTypeKey = oGetUnallocatedClaimPayment.PayeeMediaTypeKey

        End If

        Return oServiceGetUnallocatedClaimPayment

    End Function

    Public Shared Sub ToBaseImpBaseUpdateAgents(ByRef oUpdateAgentsService As List(Of BaseUpdateSubAgentsRequestTypeSubAgentsRow), _
                                                   ByRef oUpdateAgentsBase As List(Of BaseImplementationTypes.BaseUpdateSubAgentsRequestTypeSubAgentsRow))

        If (oUpdateAgentsService) IsNot Nothing Then

            Dim msgoUpdateAgents As BaseUpdateSubAgentsRequestTypeSubAgentsRow
            Dim impoUpdateAgents As BaseImplementationTypes.BaseUpdateSubAgentsRequestTypeSubAgentsRow

            For iCnt As Integer = 0 To oUpdateAgentsService.Count - 1

                msgoUpdateAgents = oUpdateAgentsService(iCnt)

                impoUpdateAgents = New BaseImplementationTypes.BaseUpdateSubAgentsRequestTypeSubAgentsRow
                impoUpdateAgents.Amount = msgoUpdateAgents.Amount
                impoUpdateAgents.PartyKey = impoUpdateAgents.PartyKey
                impoUpdateAgents.Percentage = impoUpdateAgents.Percentage

                oUpdateAgentsBase.Add(impoUpdateAgents)

            Next iCnt

        End If

    End Sub
    Public Shared Function ToServiceUpdateCashDepositList(ByVal oServiceeUpdateCashDepositType As BaseImplementationTypes.BaseUpdateCashDepositResponseTypeCashDepositRow) As BaseUpdateCashDepositResponseTypeRow

        Dim serviceeUpdateCashDeposit As New BaseUpdateCashDepositResponseTypeRow
        If oServiceeUpdateCashDepositType IsNot Nothing Then
            With oServiceeUpdateCashDepositType
                serviceeUpdateCashDeposit.CashDepositKey = .CashDepositKey
                serviceeUpdateCashDeposit.CashDepositRef = .CashDepositRef
                serviceeUpdateCashDeposit.CDTimeStamp = .CDTimeStamp
            End With
        End If
        Return serviceeUpdateCashDeposit
    End Function
    Public Shared Function ToServiceCheckUnpaidPremiumResponseTypeTransactionsList(ByVal oServiceCheckUnpaidPremiumResponseTypeTransactionsType As SFI.SAMForInsuranceV2.BaseCheckUnpaidPremiumResponseTypeTransactionsRow) As BaseCheckUnpaidPremiumResponseTypeRow

        Dim ServiceCheckUnpaidPremiumResponseTypeTransactionsType As New BaseCheckUnpaidPremiumResponseTypeRow
        If oServiceCheckUnpaidPremiumResponseTypeTransactionsType IsNot Nothing Then
            With oServiceCheckUnpaidPremiumResponseTypeTransactionsType
                ServiceCheckUnpaidPremiumResponseTypeTransactionsType.amount = .Amount
                ServiceCheckUnpaidPremiumResponseTypeTransactionsType.BranchCode = .BranchCode
                ServiceCheckUnpaidPremiumResponseTypeTransactionsType.BranchDescription = .BranchDescription
                ServiceCheckUnpaidPremiumResponseTypeTransactionsType.document_date = .DocumentDate
                ServiceCheckUnpaidPremiumResponseTypeTransactionsType.document_ref = .DocumentRef
                ServiceCheckUnpaidPremiumResponseTypeTransactionsType.document_type = .DocumentType
                ServiceCheckUnpaidPremiumResponseTypeTransactionsType.outstanding = .OutstandingAmount
                ServiceCheckUnpaidPremiumResponseTypeTransactionsType.short_code = .AccountCode
            End With
        End If
        Return ServiceCheckUnpaidPremiumResponseTypeTransactionsType
    End Function
    Public Shared Function ToServiceBaseClientDataImportResponseTypeAccountsDocuments(ByVal oImpAccountsDocuments As BaseImplementationTypes.BaseClientDataImportResponseTypeAccountsDocuments) As BaseClientDataImportResponseTypeAccountsDocuments
        Dim msgAccountsDocuments As BaseClientDataImportResponseTypeAccountsDocuments = New BaseClientDataImportResponseTypeAccountsDocuments

        If oImpAccountsDocuments IsNot Nothing Then
            msgAccountsDocuments.DocumentRef = oImpAccountsDocuments.DocumentRef
        End If
        Return msgAccountsDocuments

    End Function

    Public Shared Function ToServiceBaseClientDataImportResponseTypePolicyVersion(ByVal oImpPolicyVersion As BaseImplementationTypes.BaseClientDataImportResponseTypePolicyVersion) As BaseClientDataImportResponseTypePolicyVersion

        Dim msgPolicyVersion As BaseClientDataImportResponseTypePolicyVersion = New BaseClientDataImportResponseTypePolicyVersion

        If oImpPolicyVersion IsNot Nothing Then

            msgPolicyVersion.InsuranceFileKey = oImpPolicyVersion.InsuranceFileKey
            msgPolicyVersion.InsuranceFolderKey = oImpPolicyVersion.InsuranceFolderKey
            msgPolicyVersion.SAMStagingPolicyKey = oImpPolicyVersion.SAMStagingPolicyKey

            ' Process the Risks structure
            If IsArray(oImpPolicyVersion.Risks) = True Then
                msgPolicyVersion.Risks = oImpPolicyVersion.Risks.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseClientDataImportResponseTypePolicyVersionRisks, BaseClientDataImportResponseTypePolicyVersionRisks)(AddressOf CommonFunctions.ToServiceBaseClientDataImportResponseTypePolicyVersionRisks))
            End If

            ' Process the Risks structure
            If oImpPolicyVersion.Claim IsNot Nothing Then
                msgPolicyVersion.Claim = oImpPolicyVersion.Claim.ToList().ConvertAll(New Converter(Of BaseImplementationTypes.BaseClientDataImportResponseTypePolicyVersionClaim, BaseClientDataImportResponseTypePolicyVersionClaim)(AddressOf CommonFunctions.ToServiceBaseClientDataImportResponseTypePolicyVersionClaim))
            End If

        End If

        Return msgPolicyVersion

    End Function

    Public Shared Function ToServiceBaseClientDataImportResponseTypePolicyVersionRisks(ByVal oImpRisk As BaseImplementationTypes.BaseClientDataImportResponseTypePolicyVersionRisks) As BaseClientDataImportResponseTypePolicyVersionRisks

        Dim msgRisk As BaseClientDataImportResponseTypePolicyVersionRisks = New BaseClientDataImportResponseTypePolicyVersionRisks

        If oImpRisk IsNot Nothing Then

            msgRisk.RiskFolderKey = oImpRisk.RiskFolderKey
            msgRisk.RiskKey = oImpRisk.RiskKey
            msgRisk.SAMStagingRiskKey = oImpRisk.SAMStagingRiskKey

        End If

        Return msgRisk

    End Function

    Public Shared Function ToServiceBaseClientDataImportResponseTypePolicyVersionClaim( _
    ByVal ConvertFrom As BaseImplementationTypes.BaseClientDataImportResponseTypePolicyVersionClaim) _
        As BaseClientDataImportResponseTypePolicyVersionClaim

        Dim ConvertTo As BaseClientDataImportResponseTypePolicyVersionClaim = Nothing

        If ConvertFrom IsNot Nothing Then

            ConvertTo = New BaseClientDataImportResponseTypePolicyVersionClaim

            ConvertTo.ClaimKey = ConvertFrom.ClaimKey
            ConvertTo.SAMStagingClaimKey = ConvertFrom.SAMStagingClaimKey

        End If

        Return ConvertTo

    End Function

    Public Shared Function ToBaseImpBaseAddressType(ByVal msgAddress As BaseAddressWithContactsType) As BaseImplementationTypes.BaseAddressWithContactsType

        Dim impAddress As BaseImplementationTypes.BaseAddressWithContactsType = New BaseImplementationTypes.BaseAddressWithContactsType

        If msgAddress IsNot Nothing Then

            impAddress.AddressLine1 = msgAddress.AddressLine1
            impAddress.AddressLine2 = msgAddress.AddressLine2
            impAddress.AddressLine3 = msgAddress.AddressLine3
            impAddress.AddressLine4 = msgAddress.AddressLine4
            impAddress.AddressTypeCode = CType([Enum].ToObject(GetType(AddressTypeType), msgAddress.AddressTypeCode), BaseImplementationTypes.AddressTypeType)
            impAddress.CountryCode = msgAddress.CountryCode
            impAddress.PostCode = msgAddress.PostCode

            If msgAddress.Contacts IsNot Nothing Then

                impAddress.Contacts = Array.ConvertAll(msgAddress.Contacts.ToArray, New Converter(Of BaseContactType, BaseImplementationTypes.BaseContactType) _
                                                    (AddressOf CommonFunctions.ToBaseImpBaseContactType))
            End If

        End If

        Return impAddress

    End Function

    Public Shared Function ToServiceCreatePaymentCashListWithItemsList(ByVal oCreatePaymentCashListWithItems As BaseImplementationTypes.BaseCreatePaymentCashListWithItemsResponseTypeCashListItem) As BaseCreatePaymentCashListWithItemsResponseTypeCashListItem

        Dim ServiceCreatePaymentCashListWithItems As BaseCreatePaymentCashListWithItemsResponseTypeCashListItem = Nothing
        If oCreatePaymentCashListWithItems IsNot Nothing Then
            ServiceCreatePaymentCashListWithItems = New BaseCreatePaymentCashListWithItemsResponseTypeCashListItem
            ServiceCreatePaymentCashListWithItems.CashListItemKey = oCreatePaymentCashListWithItems.CashListItemKey
            ServiceCreatePaymentCashListWithItems.TransDetailKey = oCreatePaymentCashListWithItems.TransDetailKey
        End If

        Return ServiceCreatePaymentCashListWithItems

    End Function

    Public Shared Function ToServiceCreateReceiptCashListItemList(ByVal oCreateReceiptCashListItem As BaseImplementationTypes.BaseCreateReceiptCashListItemResponseTypeCashListItem) As BaseCreateReceiptCashListItemResponseTypeCashListItem

        Dim ServiceCreateReceiptCashListItem As BaseCreateReceiptCashListItemResponseTypeCashListItem = Nothing
        If oCreateReceiptCashListItem IsNot Nothing Then
            ServiceCreateReceiptCashListItem = New BaseCreateReceiptCashListItemResponseTypeCashListItem
            ServiceCreateReceiptCashListItem.CashListItemKey = oCreateReceiptCashListItem.CashListItemKey
            ServiceCreateReceiptCashListItem.TransDetailKey = oCreateReceiptCashListItem.TransDetailKey
        End If

        Return ServiceCreateReceiptCashListItem

    End Function

    Public Shared Function ToServiceCreateReceiptCashListWithItemsList(ByVal oCreateReceiptCashListWithItems As BaseImplementationTypes.BaseCreateReceiptCashListWithItemsResponseTypeCashListItem) As BaseCreateReceiptCashListWithItemsResponseTypeCashListItem

        Dim ServicCreateReceiptCashListWithItems As BaseCreateReceiptCashListWithItemsResponseTypeCashListItem = Nothing
        If oCreateReceiptCashListWithItems IsNot Nothing Then
            ServicCreateReceiptCashListWithItems = New BaseCreateReceiptCashListWithItemsResponseTypeCashListItem
            ServicCreateReceiptCashListWithItems.CashListItemKey = oCreateReceiptCashListWithItems.CashListItemKey
            ServicCreateReceiptCashListWithItems.TransDetailKey = oCreateReceiptCashListWithItems.TransDetailKey
        End If

        Return ServicCreateReceiptCashListWithItems

    End Function

    Public Shared Function ToServiceFindCashDepositList(ByVal oFindCashDeposit As BaseImplementationTypes.BaseCashDepositItemType) As BaseCashDepositItemType

        Dim ServicFindCashDeposit As BaseCashDepositItemType = Nothing
        If oFindCashDeposit IsNot Nothing Then
            With oFindCashDeposit
                ServicFindCashDeposit = New BaseCashDepositItemType

                ServicFindCashDeposit.AccountKey = .AccountKey
                ServicFindCashDeposit.Amount = .Amount
                ServicFindCashDeposit.AvailableBalance = .AvailableBalance
                ServicFindCashDeposit.BankName = .BankName
                ServicFindCashDeposit.Branch = .Branch
                ServicFindCashDeposit.CashDepositKey = .CashDepositKey
                ServicFindCashDeposit.CashDepositRef = .CashDepositRef
                ServicFindCashDeposit.IsDeleted = .IsDeleted
                ServicFindCashDeposit.IsSinglePolicy = .IsSinglePolicy
                ServicFindCashDeposit.PartyKey = .PartyKey
                ServicFindCashDeposit.PartyName = .PartyName
                ServicFindCashDeposit.Product = .Product
                ServicFindCashDeposit.PartyCode = .PartyCode
            End With

        End If

        Return ServicFindCashDeposit

    End Function
    Public Shared Function ToServiceFindCashListReceiptsList(ByVal oFindCashListReceipts As SFI.SAMForInsuranceV2.BaseFindCashListReceiptsResponseTypeCashListItemsRow) As BaseFindCashListReceiptsResponseTypeRow

        Dim ServicFindCashListReceipts As BaseFindCashListReceiptsResponseTypeRow = Nothing
        If oFindCashListReceipts IsNot Nothing Then
            With oFindCashListReceipts
                ServicFindCashListReceipts = New BaseFindCashListReceiptsResponseTypeRow
                ServicFindCashListReceipts.BranchDescription = .BranchDescription
                ServicFindCashListReceipts.CashListItemKey = .CashListItemKey
                ServicFindCashListReceipts.ClientCode = .ClientCode
                ServicFindCashListReceipts.ClientName = .ClientName
                ServicFindCashListReceipts.CurrentStatus = .CurrentStatus
                ServicFindCashListReceipts.DocumentRef = .DocumentRef
                ServicFindCashListReceipts.DrawnBankName = .DrawnBankName
                ServicFindCashListReceipts.InsuranceFileKey = .InsuranceFileKey
                ServicFindCashListReceipts.MediaReference = .MediaReference
                ServicFindCashListReceipts.MediaTypeCode = .MediaTypeCode
                ServicFindCashListReceipts.MediaTypeDescription = .MediaTypeDescription
                ServicFindCashListReceipts.MediaTypeKey = .MediaTypeKey
                ServicFindCashListReceipts.MediaTypeStatusCode = .MediaTypeStatusCode
                ServicFindCashListReceipts.MediaTypeStatusDescription = .MediaTypeStatusDescription
                ServicFindCashListReceipts.MediaTypeStatusKey = .MediaTypeStatusKey
                ServicFindCashListReceipts.PolicyNumber = .PolicyNumber

            End With

        End If

        Return ServicFindCashListReceipts

    End Function
    Public Shared Function ToServiceGetReceiptCashListItemsList(ByVal oGetReceiptCashListItems As SFI.SAMForInsuranceV2.BaseGetReceiptCashListItemsResponseTypeReceiptCashListItemsRow) As BaseGetReceiptCashListItemsResponseTypeRow

        Dim ServiceGetReceiptCashListItems As BaseGetReceiptCashListItemsResponseTypeRow = Nothing
        If oGetReceiptCashListItems IsNot Nothing Then
            With oGetReceiptCashListItems
                ServiceGetReceiptCashListItems = New BaseGetReceiptCashListItemsResponseTypeRow
                ServiceGetReceiptCashListItems.AccountShortCode = .AccountShortCode
                ServiceGetReceiptCashListItems.Amount = .Amount
                ServiceGetReceiptCashListItems.CashListItemKey = .CashListItemKey
                ServiceGetReceiptCashListItems.Letter = .Letter
                ServiceGetReceiptCashListItems.MediaReference = .MediaReference
                ServiceGetReceiptCashListItems.MediaType = .MediaType
                ServiceGetReceiptCashListItems.Status = .Status
            End With

        End If

        Return ServiceGetReceiptCashListItems

    End Function

    Public Shared Function ToServiceAgentCommissionList(ByVal oAgentCommission As SFI.SAMForInsuranceV2.BaseAgentCommissionResponseTypeAgentCommissionRow) As BaseAgentCommissionResponseTypeRow

        Dim ServicAgentCommission As BaseAgentCommissionResponseTypeRow = Nothing
        If oAgentCommission IsNot Nothing Then
            With oAgentCommission
                ServicAgentCommission = New BaseAgentCommissionResponseTypeRow
                ServicAgentCommission.Agent = .Agent
                ServicAgentCommission.AgentType = .AgentType
                ServicAgentCommission.CommissionBand = .CommissionBand
                ServicAgentCommission.CommissionRate = .CommissionRate
                ServicAgentCommission.CommissionValue = .CommissionValue
                ServicAgentCommission.IsLeadAgent = .IsLeadAgent
                ServicAgentCommission.IsValue = .IsValue
                ServicAgentCommission.MaximumRate = .MaximumRate
                ServicAgentCommission.Premium = .Premium
                ServicAgentCommission.RiskType = .RiskType
                ServicAgentCommission.TaxGroup = .TaxGroup
                ServicAgentCommission.TaxValue = .TaxValue
                ServicAgentCommission.TaxGroupDescription = .TaxGroupDescription
            End With

        End If

        Return ServicAgentCommission

    End Function

    Public Shared Function ToServiceBaseUserDetailsTypeList(ByVal oUserDetails As BaseImplementationTypes.BaseUserDetailsType) As BaseUserDetailsType

        Dim ServicUserDetails As BaseUserDetailsType = Nothing
        If oUserDetails IsNot Nothing Then
            With oUserDetails
                ServicUserDetails = New BaseUserDetailsType()
                ServicUserDetails.EmailAddress = .EmailAddress
                ServicUserDetails.UserKey = .UserKey
                ServicUserDetails.FullName = .FullName
                ServicUserDetails.UserName = .UserName
                ServicUserDetails.EffectiveDate = .EffectiveDate
            End With

        End If

        Return ServicUserDetails

    End Function

    Public Shared Function ToServiceProductList(ByVal oProduct As BaseImplementationTypes.BaseGetCashDepositResponseTypeProducts) As BaseGetCashDepositResponseTypeProducts

        Dim ServiceProduct As BaseGetCashDepositResponseTypeProducts = Nothing
        If oProduct IsNot Nothing Then
            With oProduct
                ServiceProduct = New BaseGetCashDepositResponseTypeProducts
                ServiceProduct.Description = .Description
                ServiceProduct.ProductCode = .ProductCode
                ServiceProduct.ProductKey = .ProductKey
            End With

        End If

        Return ServiceProduct

    End Function

    Public Shared Function ToServiceAgentProductList(ByVal oProduct As SFI.SAMForInsuranceV2.BaseGetProductByAgentResponseTypeProductsRow) As BaseGetProductByAgentResponseTypeRow

        Dim ServiceProduct As BaseGetProductByAgentResponseTypeRow = Nothing
        If oProduct IsNot Nothing Then
            With oProduct
                ServiceProduct = New BaseGetProductByAgentResponseTypeRow
                ServiceProduct.Description = .Description
                ServiceProduct.ProductCode = .ProductCode
                ServiceProduct.ProductKey = .ProductKey
                ServiceProduct.BlockNumber = .BlockNumber
                ServiceProduct.ConsolidatedLeadAgentCommission = .ConsolidatedLeadAgentCommission
                ServiceProduct.ConsolidatedSubAgentCommission = .ConsolidatedSubAgentCommission
                ServiceProduct.SchemeAgencyRef = .SchemeAgencyRef
            End With

        End If

        Return ServiceProduct

    End Function

    Public Shared Function ToServiceBranchList(ByVal oBranch As BaseImplementationTypes.BaseGetCashDepositResponseTypeBranches) As BaseGetCashDepositResponseTypeBranches

        Dim ServiceBranch As BaseGetCashDepositResponseTypeBranches = Nothing
        If oBranch IsNot Nothing Then
            With oBranch
                ServiceBranch = New BaseGetCashDepositResponseTypeBranches
                ServiceBranch.Description = .Description
                ServiceBranch.BranchCode = .BranchCode
                ServiceBranch.BranchKey = .BranchKey
            End With

        End If

        Return ServiceBranch

    End Function

    Public Shared Function ToServiceCashDepositPoliciesList(ByVal oCashDepositPolicies As SFI.SAMForInsuranceV2.BaseGetCashDepositsForPolicyResponseTypeCashDepositPoliciesRow) As BaseGetCashDepositsForPolicyResponseTypeCashDepositPoliciesRow

        Dim ServiceCashDepositPolicies As BaseGetCashDepositsForPolicyResponseTypeCashDepositPoliciesRow = Nothing
        If oCashDepositPolicies IsNot Nothing Then
            With oCashDepositPolicies
                ServiceCashDepositPolicies = New BaseGetCashDepositsForPolicyResponseTypeCashDepositPoliciesRow
                ServiceCashDepositPolicies.AccountKey = .AccountKey
                ServiceCashDepositPolicies.Amount = .Amount
                ServiceCashDepositPolicies.AvailableBalance = .AvailableBalance
                ServiceCashDepositPolicies.CashDepositKey = .CashDepositKey
                ServiceCashDepositPolicies.CashDepositRef = .CashDepositRef
                ServiceCashDepositPolicies.DateCreated = .DateCreated
                ServiceCashDepositPolicies.PartyKey = .PartyKey

            End With

        End If

        Return ServiceCashDepositPolicies

    End Function

    Public Shared Function ToServiceGetClaimReinsuranceArrangementLinesList(ByVal oReinsuranceArrangementLines As SFI.SAMForInsuranceV2.BaseGetClaimReinsuranceArrangementLinesResponseTypeReinsuranceArrangementLinesRow) As BaseGetClaimReinsuranceArrangementLinesResponseTypeRow

        Dim ServiceReinsuranceArrangementLines As BaseGetClaimReinsuranceArrangementLinesResponseTypeRow = Nothing
        If oReinsuranceArrangementLines IsNot Nothing Then
            With oReinsuranceArrangementLines
                ServiceReinsuranceArrangementLines = New BaseGetClaimReinsuranceArrangementLinesResponseTypeRow
                ServiceReinsuranceArrangementLines.Agreement = .Agreement
                ServiceReinsuranceArrangementLines.Balance = .Balance
                ServiceReinsuranceArrangementLines.DefaultPerc = .DefaultPerc
                ServiceReinsuranceArrangementLines.IsObligatory = .IsObligatory
                ServiceReinsuranceArrangementLines.Name = .Name
                ServiceReinsuranceArrangementLines.PaymentToDate = .PaymentToDate
                ServiceReinsuranceArrangementLines.ReserveToDate = .ReserveToDate
                ServiceReinsuranceArrangementLines.SumInsured = .SumInsured
                ServiceReinsuranceArrangementLines.ThisPayment = .ThisPayment
                ServiceReinsuranceArrangementLines.ThisPerc = .ThisPerc
                ServiceReinsuranceArrangementLines.ThisReserve = .ThisReserve
            End With

        End If

        Return ServiceReinsuranceArrangementLines

    End Function

    Public Shared Function ToServiceGetClaimReinsuranceArrangementsList(ByVal oReinsuranceArrangementLines As SFI.SAMForInsuranceV2.BaseGetClaimReinsuranceArrangementsResponseTypeReinsuranceArrangementsRow) As BaseGetClaimReinsuranceArrangementsResponseTypeRow

        Dim ServiceReinsuranceArrangementLines As BaseGetClaimReinsuranceArrangementsResponseTypeRow = Nothing
        If oReinsuranceArrangementLines IsNot Nothing Then
            With oReinsuranceArrangementLines
                ServiceReinsuranceArrangementLines = New BaseGetClaimReinsuranceArrangementsResponseTypeRow
                ServiceReinsuranceArrangementLines.ArrangementId = .ArrangementId
                ServiceReinsuranceArrangementLines.Balance = .Balance
                ServiceReinsuranceArrangementLines.BandId = .BandId
                ServiceReinsuranceArrangementLines.PaymentToDate = .PaymentToDate
                ServiceReinsuranceArrangementLines.ReserveToDate = .ReserveToDate
                ServiceReinsuranceArrangementLines.SumInsured = .SumInsured
                ServiceReinsuranceArrangementLines.ReserveToDate = .ReserveToDate
                ServiceReinsuranceArrangementLines.ThisPayment = .ThisPayment
                ServiceReinsuranceArrangementLines.ThisPayment = .ThisPayment
                ServiceReinsuranceArrangementLines.ThisReserve = .ThisReserve
            End With

        End If

        Return ServiceReinsuranceArrangementLines

    End Function
    Public Shared Function ToServiceGetRiskReinsuranceArrangementsList(ByVal oReinsuranceArrangementLines As SFI.SAMForInsuranceV2.BaseGetRiskReinsuranceArrangementsResponseTypeArrangementsRow) As BaseGetRiskReinsuranceArrangementsResponseTypeRow

        Dim ServiceReinsuranceArrangementLines As BaseGetRiskReinsuranceArrangementsResponseTypeRow = Nothing
        If oReinsuranceArrangementLines IsNot Nothing Then
            With oReinsuranceArrangementLines
                ServiceReinsuranceArrangementLines = New BaseGetRiskReinsuranceArrangementsResponseTypeRow
                ServiceReinsuranceArrangementLines.ArrangementId = .ArrangementId
                ServiceReinsuranceArrangementLines.BandId = .BandId
                ServiceReinsuranceArrangementLines.FACPremiumType = .FACPremiumType
                ServiceReinsuranceArrangementLines.IsModified = .IsModified
                ServiceReinsuranceArrangementLines.IsOriginal = .IsOriginal
                ServiceReinsuranceArrangementLines.ModelId = .ModelId
                ServiceReinsuranceArrangementLines.Premium = .Premium
                ServiceReinsuranceArrangementLines.SumInsured = .SumInsured
            End With

        End If

        Return ServiceReinsuranceArrangementLines

    End Function
    Public Shared Function ToServiceGetClaimReinsuranceBandsList(ByVal oReinsuranceArrangementLines As SFI.SAMForInsuranceV2.BaseGetClaimReinsuranceBandsResponseTypeReinsuranceBandsRow) As BaseGetClaimReinsuranceBandsResponseTypeRow

        Dim ServiceReinsuranceArrangementLines As BaseGetClaimReinsuranceBandsResponseTypeRow = Nothing
        If oReinsuranceArrangementLines IsNot Nothing Then
            With oReinsuranceArrangementLines
                ServiceReinsuranceArrangementLines = New BaseGetClaimReinsuranceBandsResponseTypeRow
                ServiceReinsuranceArrangementLines.Band = .Band
                ServiceReinsuranceArrangementLines.BandId = .BandId
            End With
        End If

        Return ServiceReinsuranceArrangementLines

    End Function

    Public Shared Function ToServiceGetRiskReinsuranceBandsList(ByVal oReinsuranceArrangementLines As SFI.SAMForInsuranceV2.BaseGetRiskReinsuranceBandsResponseTypeReinsuranceBandsRow) As BaseGetRiskReinsuranceBandsResponseTypeRow

        Dim ServiceReinsuranceArrangementLines As BaseGetRiskReinsuranceBandsResponseTypeRow = Nothing
        If oReinsuranceArrangementLines IsNot Nothing Then
            With oReinsuranceArrangementLines
                ServiceReinsuranceArrangementLines = New BaseGetRiskReinsuranceBandsResponseTypeRow
                ServiceReinsuranceArrangementLines.Band = .Band
                ServiceReinsuranceArrangementLines.BandKey = .BandKey
            End With
        End If

        Return ServiceReinsuranceArrangementLines

    End Function
    Public Shared Function ToServiceSubAgentsList(ByVal oSubAgent As SFI.SAMForInsuranceV2.BaseGetSubAgentsResponseTypeSubAgentsRow) As BaseGetSubAgentsResponseTypeRow

        Dim ServiceSubAgent As BaseGetSubAgentsResponseTypeRow = Nothing
        If oSubAgent IsNot Nothing Then
            With oSubAgent
                ServiceSubAgent = New BaseGetSubAgentsResponseTypeRow
                ServiceSubAgent.Amount = .Amount
                ServiceSubAgent.Code = .Code
                ServiceSubAgent.Name = .Name
                ServiceSubAgent.PartyKey = .PartyKey
                ServiceSubAgent.Percentage = .Percentage
            End With
        End If

        Return ServiceSubAgent

    End Function



    Public Shared Function ToServiceGetPaymentCashListItemsList(ByVal oGetPaymentCashListItems As SFI.SAMForInsuranceV2.BaseGetPaymentCashListItemsResponseTypePaymentCashListItemsRow) As BaseGetPaymentCashListItemsResponseTypeRow

        Dim ServiceGetPaymentCashListItems As BaseGetPaymentCashListItemsResponseTypeRow = Nothing
        If oGetPaymentCashListItems IsNot Nothing Then
            With oGetPaymentCashListItems
                ServiceGetPaymentCashListItems = New BaseGetPaymentCashListItemsResponseTypeRow
                ServiceGetPaymentCashListItems.AccountShortCode = .AccountShortCode
                ServiceGetPaymentCashListItems.Amount = .Amount
                ServiceGetPaymentCashListItems.CashListItemKey = .CashListItemKey
                ServiceGetPaymentCashListItems.Letter = .Letter
                ServiceGetPaymentCashListItems.MediaReference = .MediaReference
                ServiceGetPaymentCashListItems.MediaType = .MediaType
                ServiceGetPaymentCashListItems.Status = .Status
            End With

        End If

        Return ServiceGetPaymentCashListItems

    End Function

    Public Shared Function ToServiceGetRecoveryReinsuranceList(ByVal oGetRecoveryReinsurance As SFI.SAMForInsuranceV2.BaseGetRecoveryReinsuranceResponseTypeReinsurancesRow) As BaseGetRecoveryReinsuranceResponseTypeRow

        Dim ServiceGetRecoveryReinsurance As BaseGetRecoveryReinsuranceResponseTypeRow = Nothing
        If oGetRecoveryReinsurance IsNot Nothing Then
            With oGetRecoveryReinsurance
                ServiceGetRecoveryReinsurance = New BaseGetRecoveryReinsuranceResponseTypeRow
                ServiceGetRecoveryReinsurance.PartyKey = .PartyKey
                ServiceGetRecoveryReinsurance.RecoveryKey = .RecoveryKey
                ServiceGetRecoveryReinsurance.RecoveryToDate = .RecoveryToDate
                ServiceGetRecoveryReinsurance.RecoveryType = .RecoveryType
                ServiceGetRecoveryReinsurance.Reinsurer = .Reinsurer
            End With

        End If

        Return ServiceGetRecoveryReinsurance

    End Function

    Public Shared Function ToServiceGetRiskReinsuranceArrangementLinesList(ByVal oReinsuranceArrangementLines As SFI.SAMForInsuranceV2.BaseGetRiskReinsuranceArrangementLinesResponseTypeArrangementLinesRow) As BaseGetRiskReinsuranceArrangementLinesResponseTypeRow

        Dim ServiceReinsuranceArrangementLines As BaseGetRiskReinsuranceArrangementLinesResponseTypeRow = Nothing
        If oReinsuranceArrangementLines IsNot Nothing Then
            With oReinsuranceArrangementLines
                ServiceReinsuranceArrangementLines = New BaseGetRiskReinsuranceArrangementLinesResponseTypeRow
                ServiceReinsuranceArrangementLines.Agreement = .Agreement
                ServiceReinsuranceArrangementLines.DefaultPerc = .DefaultPerc
                ServiceReinsuranceArrangementLines.IsObligatory = .IsObligatory
                ServiceReinsuranceArrangementLines.Name = .Name
                ServiceReinsuranceArrangementLines.SumInsured = .SumInsured
                ServiceReinsuranceArrangementLines.ThisPerc = .ThisPerc
                ServiceReinsuranceArrangementLines.Commission = .Commission
                ServiceReinsuranceArrangementLines.CommissionPerc = .CommissionPerc
                ServiceReinsuranceArrangementLines.CommissionTax = .CommissionTax
                ServiceReinsuranceArrangementLines.Premium = .Premium
                ServiceReinsuranceArrangementLines.Tax = .Tax
                ServiceReinsuranceArrangementLines.ThisPerc = .ThisPerc
            End With

        End If

        Return ServiceReinsuranceArrangementLines

    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oBankGuarantee"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceBankGuaranteeList( _
                         ByVal oBankGuarantee As BaseImplementationTypes.BaseAddBankGuaranteeResponseTypeBankGuaranteeRow) As BaseAddBankGuaranteeResponseTypeRow

        Dim oServiceBankGuarantee As New BaseAddBankGuaranteeResponseTypeRow

        If oBankGuarantee IsNot Nothing Then

            oServiceBankGuarantee.BGKey = oBankGuarantee.BGKey
            oServiceBankGuarantee.BGRef = oBankGuarantee.BGRef
            oServiceBankGuarantee.BGTimeStamp = oBankGuarantee.BGTimeStamp

        End If

        Return oServiceBankGuarantee

    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oCashDeposit"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceCashDepositList( _
                         ByVal oCashDeposit As BaseImplementationTypes.BaseAddCashDepositResponseTypeCashDepositRow) As BaseAddCashDepositResponseTypeRow

        Dim oServiceCashDeposit As New BaseAddCashDepositResponseTypeRow

        If oCashDeposit IsNot Nothing Then

            oServiceCashDeposit.CashDepositKey = oCashDeposit.CashDepositKey
            oServiceCashDeposit.CashDepositRef = oCashDeposit.CashDepositRef
            oServiceCashDeposit.CDTimeStamp = oCashDeposit.CDTimeStamp

        End If

        Return oServiceCashDeposit

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oPartyBGPolicyDetailsList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServicePartyBGPolicyDetailsList(ByVal oPartyBGPolicyDetailsList As SFI.SAMForInsuranceV2.BaseGetPoliciesOnBankGuaranteeByKeyResponseTypePartyBGPolicyDetailsRow) As BaseGetPoliciesOnBankGuaranteeByKeyResponseTypeRow

        Dim oServiceBGPolicyDetailsList As BaseGetPoliciesOnBankGuaranteeByKeyResponseTypeRow = New BaseGetPoliciesOnBankGuaranteeByKeyResponseTypeRow()

        If oPartyBGPolicyDetailsList IsNot Nothing Then
            oServiceBGPolicyDetailsList.AgentCode = oPartyBGPolicyDetailsList.AgentCode
            oServiceBGPolicyDetailsList.BranchDesc = oPartyBGPolicyDetailsList.BranchDesc
            oServiceBGPolicyDetailsList.ClientCode = oPartyBGPolicyDetailsList.ClientCode
            oServiceBGPolicyDetailsList.ClientName = oPartyBGPolicyDetailsList.ClientName
            oServiceBGPolicyDetailsList.CoverEndDate = oPartyBGPolicyDetailsList.CoverEndDate
            oServiceBGPolicyDetailsList.CoverStartDate = oPartyBGPolicyDetailsList.CoverStartDate
            oServiceBGPolicyDetailsList.InsuranceFileKey = oPartyBGPolicyDetailsList.InsuranceFileKey
            oServiceBGPolicyDetailsList.PolicyRef = oPartyBGPolicyDetailsList.PolicyRef
            oServiceBGPolicyDetailsList.PremiumAmount = oPartyBGPolicyDetailsList.PremiumAmount
            oServiceBGPolicyDetailsList.ProductDesc = oPartyBGPolicyDetailsList.ProductDesc
        End If

        Return oServiceBGPolicyDetailsList

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oPartyBGPolicyDetailsList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServicePartyBGPolicyForReceiptResposeDetailsList(ByVal oPartyBGPolicyDetailsList As SFI.SAMForInsuranceV2.BaseGetPoliciesOnBankGuaranteeForReceiptResponseTypePartyBGPolicyDetailsRow) As BaseGetPoliciesOnBankGuaranteeForReceiptResponseTypeRow

        Dim oServiceBGPolicyDetailsList As BaseGetPoliciesOnBankGuaranteeForReceiptResponseTypeRow = New BaseGetPoliciesOnBankGuaranteeForReceiptResponseTypeRow()

        If oPartyBGPolicyDetailsList IsNot Nothing Then
            oServiceBGPolicyDetailsList.BankGuaranteeRef = oPartyBGPolicyDetailsList.BankGuaranteeRef
            oServiceBGPolicyDetailsList.BankName = oPartyBGPolicyDetailsList.BankName
            oServiceBGPolicyDetailsList.BankNameKey = oPartyBGPolicyDetailsList.BankNameKey
            oServiceBGPolicyDetailsList.BGDueDate = oPartyBGPolicyDetailsList.BGDueDate
            oServiceBGPolicyDetailsList.BGKey = oPartyBGPolicyDetailsList.BGKey
            oServiceBGPolicyDetailsList.BranchCode = oPartyBGPolicyDetailsList.BranchCode
            oServiceBGPolicyDetailsList.BranchDesc = oPartyBGPolicyDetailsList.BranchDesc
            oServiceBGPolicyDetailsList.CoverEndDate = oPartyBGPolicyDetailsList.CoverEndDate
            oServiceBGPolicyDetailsList.CoverStartDate = oPartyBGPolicyDetailsList.CoverStartDate
            oServiceBGPolicyDetailsList.OutstandingPolicyAmt = oPartyBGPolicyDetailsList.OutstandingPolicyAmt
            oServiceBGPolicyDetailsList.PolicyKey = oPartyBGPolicyDetailsList.PolicyKey
            oServiceBGPolicyDetailsList.PolicyRef = oPartyBGPolicyDetailsList.PolicyRef
            oServiceBGPolicyDetailsList.PremiumAmount = oPartyBGPolicyDetailsList.PremiumAmount
            oServiceBGPolicyDetailsList.ProductCode = oPartyBGPolicyDetailsList.ProductCode
            oServiceBGPolicyDetailsList.ProductDesc = oPartyBGPolicyDetailsList.ProductDesc
        End If

        Return oServiceBGPolicyDetailsList

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oRatingDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceRatingDetailsList( _
                         ByVal oRatingDetails As SFI.SAMForInsuranceV2.BaseGetRatingDetailsResponseTypeRatingDetailsRow) As BaseGetRatingDetailsResponseTypeRow

        Dim oServiceRatingDetails As New BaseGetRatingDetailsResponseTypeRow

        If oRatingDetails IsNot Nothing Then

            oServiceRatingDetails.AnnualPremium = oRatingDetails.AnnualPremium
            oServiceRatingDetails.AnnualRate = oRatingDetails.AnnualRate
            oServiceRatingDetails.CalculatedPremium = oRatingDetails.CalculatedPremium
            oServiceRatingDetails.Country = oRatingDetails.Country
            oServiceRatingDetails.CountryCode = oRatingDetails.CountryCode
            oServiceRatingDetails.CountryId = oRatingDetails.CountryId
            oServiceRatingDetails.CurrencyCode = oRatingDetails.CurrencyCode
            oServiceRatingDetails.CurrencyId = oRatingDetails.CurrencyId
            oServiceRatingDetails.EarningPattern = oRatingDetails.EarningPattern
            oServiceRatingDetails.EarningPatternCode = oRatingDetails.EarningPatternCode
            oServiceRatingDetails.EarningPatternId = oRatingDetails.EarningPatternId
            oServiceRatingDetails.IsAmended = oRatingDetails.IsAmended
            oServiceRatingDetails.OriginalFlag = oRatingDetails.OriginalFlag
            oServiceRatingDetails.OverrideReason = oRatingDetails.OverrideReason
            oServiceRatingDetails.PolicySectionType = oRatingDetails.PolicySectionType
            oServiceRatingDetails.PolicySectionTypeId = oRatingDetails.PolicySectionTypeId
            oServiceRatingDetails.RateType = oRatingDetails.RateType
            oServiceRatingDetails.RateTypeId = oRatingDetails.RateTypeId
            oServiceRatingDetails.RatingSectionId = oRatingDetails.RatingSectionId
            oServiceRatingDetails.RatingSectionType = oRatingDetails.RatingSectionType
            oServiceRatingDetails.RatingSectionTypeCode = oRatingDetails.RatingSectionTypeCode
            oServiceRatingDetails.RatingSectionTypeId = oRatingDetails.RatingSectionTypeId
            oServiceRatingDetails.RatingTypeCode = oRatingDetails.RatingTypeCode
            oServiceRatingDetails.State = oRatingDetails.State
            oServiceRatingDetails.StateCode = oRatingDetails.StateCode
            oServiceRatingDetails.StateId = oRatingDetails.StateId
            oServiceRatingDetails.SumInsured = oRatingDetails.SumInsured
            oServiceRatingDetails.ThisPremium = oRatingDetails.ThisPremium

        End If

        Return oServiceRatingDetails

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oSectionList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceSectionList( _
                         ByVal oSectionList As SFI.SAMForInsuranceV2.BaseGetRatingSectionTypesResponseTypeRatingSectionTypesRow) As BaseGetRatingSectionTypesResponseTypeRow

        Dim oServiceSection As New BaseGetRatingSectionTypesResponseTypeRow

        If oSectionList IsNot Nothing Then

            oServiceSection.CountryCode = oSectionList.CountryCode
            oServiceSection.CountryId = oSectionList.CountryId
            oServiceSection.CurrencyCode = oSectionList.CurrencyCode
            oServiceSection.CurrencyId = oSectionList.CurrencyId
            oServiceSection.Description = oSectionList.Description
            oServiceSection.EarningPatternCode = oSectionList.EarningPatternCode
            oServiceSection.Rate = oSectionList.Rate
            oServiceSection.RateTypeCode = oSectionList.RateTypeCode
            oServiceSection.RateTypeId = oSectionList.RateTypeId
            oServiceSection.RatingSectionTypeCode = oSectionList.RatingSectionTypeCode
            oServiceSection.RatingSectionTypeId = oSectionList.RatingSectionTypeId
            oServiceSection.StateCode = oSectionList.StateCode
            oServiceSection.StateId = oSectionList.StateId

        End If

        Return oServiceSection

    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oReferredPaymentList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceReferredPaymentList( _
                         ByVal oReferredPaymentList As SFI.SAMForInsuranceV2.BaseGetReferredPaymentsResponseTypeCashListItemsRow) As BaseGetReferredPaymentsResponseTypeRow

        Dim oServiceReferredPaymentList As New BaseGetReferredPaymentsResponseTypeRow

        If oReferredPaymentList IsNot Nothing Then

            oServiceReferredPaymentList.ClaimKey = oReferredPaymentList.ClaimKey
            oServiceReferredPaymentList.ClaimNumber = oReferredPaymentList.ClaimNumber
            oServiceReferredPaymentList.ClaimPaymentKey = oReferredPaymentList.ClaimPaymentKey
            oServiceReferredPaymentList.ClientName = oReferredPaymentList.ClientName
            oServiceReferredPaymentList.CreatedBy = oReferredPaymentList.CreatedBy

            oServiceReferredPaymentList.PaymentAmount = oReferredPaymentList.PaymentAmount
            oServiceReferredPaymentList.PaymentDate = oReferredPaymentList.PaymentDate
            oServiceReferredPaymentList.PaymentDate = oReferredPaymentList.PaymentDate

            oServiceReferredPaymentList.Status = oReferredPaymentList.Status

        End If

        Return oServiceReferredPaymentList

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetSharepointFielList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceSharepointFileList(ByVal oGetSharepointFileList As BaseImplementationTypes.BaseGetSharepointFileListResponseTypeItemList) As ArrayOfBaseGetSharepointFileListResponseTypeItemListBaseGetSharepointFileListResponseTypeItemList
        Dim oServiceGetSharepointFileList As New ArrayOfBaseGetSharepointFileListResponseTypeItemListBaseGetSharepointFileListResponseTypeItemList
        If (oGetSharepointFileList) IsNot Nothing Then
            With oGetSharepointFileList

                oServiceGetSharepointFileList.CreatedDate = .CreatedDate
                oServiceGetSharepointFileList.DocumentTemplateGroup = .DocumentTemplateGroup
                oServiceGetSharepointFileList.DocumentTemplateSubGroup = .DocumentTemplateSubGroup
                oServiceGetSharepointFileList.Filename = .Filename
                oServiceGetSharepointFileList.InternalOnly = .InternalOnly
                oServiceGetSharepointFileList.ItemType = .ItemType
                oServiceGetSharepointFileList.LastModifiedDate = .LastModifiedDate
                oServiceGetSharepointFileList.PureUser = .PureUser
                oServiceGetSharepointFileList.Title = .Title
                oServiceGetSharepointFileList.URL = .URL

            End With
        End If

        Return oServiceGetSharepointFileList

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetTransactionDetailsList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceTransactionDetailsList(ByVal oGetTransactionDetailsList As SFI.SAMForInsuranceV2.BaseGetTransactionDetailsResponseTypeTransactionsRow) As BaseGetTransactionDetailsResponseTypeRow
        Dim oServiceTransactionDetailsList As New BaseGetTransactionDetailsResponseTypeRow
        If (oGetTransactionDetailsList) IsNot Nothing Then
            With oGetTransactionDetailsList

                oServiceTransactionDetailsList.AccountCode = .AccountCode
                oServiceTransactionDetailsList.Accountkey = .Accountkey
                oServiceTransactionDetailsList.AllocationTimeStamp = .AllocationTimeStamp
                oServiceTransactionDetailsList.AltRef = .AltRef
                oServiceTransactionDetailsList.Amount = .Amount
                oServiceTransactionDetailsList.Currency = .Currency
                oServiceTransactionDetailsList.CurrencyDiff = .CurrencyDiff
                oServiceTransactionDetailsList.DocRef = .DocRef
                oServiceTransactionDetailsList.EffectiveDate = .EffectiveDate
                oServiceTransactionDetailsList.MediaRef = .MediaRef
                oServiceTransactionDetailsList.MediaType = .MediaType
                oServiceTransactionDetailsList.OutstandingAmount = .OutstandingAmount
                oServiceTransactionDetailsList.TaxBand = .TaxBand
                oServiceTransactionDetailsList.TransactionCurrenciesAmount = .TransactionCurrenciesAmount
                oServiceTransactionDetailsList.TransactionCurrency = .TransactionCurrency
                oServiceTransactionDetailsList.TransDate = .TransDate
                oServiceTransactionDetailsList.TransDetailKey = .TransDetailKey

            End With
        End If

        Return oServiceTransactionDetailsList

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetPrimaryCausesList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServicePrimaryCausesList(ByVal oGetPrimaryCausesList As SFI.SAMForInsuranceV2.BaseGetValidPrimaryCausesResponseTypePrimaryCausesRow) As BaseGetValidPrimaryCausesResponseTypeRow
        Dim oServicePrimaryCausesList As New BaseGetValidPrimaryCausesResponseTypeRow
        If (oGetPrimaryCausesList) IsNot Nothing Then
            With oGetPrimaryCausesList

                oServicePrimaryCausesList.code = .code
                oServicePrimaryCausesList.description = .description
                oServicePrimaryCausesList.primary_cause_id = .primary_cause_id

            End With
        End If

        Return oServicePrimaryCausesList

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oGetWorkManagerScheduledTasksList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToServiceWorkManagerScheduledTasksList(ByVal oGetWorkManagerScheduledTasksList As SFI.SAMForInsuranceV2.BaseGetWorkManagerScheduledTasksResponseTypeTasksRow) As BaseGetWorkManagerScheduledTasksResponseTypeRow
        Dim oServiceWorkManagerScheduledTasksList As New BaseGetWorkManagerScheduledTasksResponseTypeRow
        If (oGetWorkManagerScheduledTasksList) IsNot Nothing Then
            With oGetWorkManagerScheduledTasksList

                oServiceWorkManagerScheduledTasksList.Customer = .Customer
                oServiceWorkManagerScheduledTasksList.Description = .Description
                oServiceWorkManagerScheduledTasksList.DueDate = .DueDate
                oServiceWorkManagerScheduledTasksList.PartyKey = .PartyKey
                oServiceWorkManagerScheduledTasksList.PartyName = .PartyName
                oServiceWorkManagerScheduledTasksList.TaskGroupKey = .TaskGroupKey
                oServiceWorkManagerScheduledTasksList.TaskInstanceKey = .TaskInstanceKey
                oServiceWorkManagerScheduledTasksList.TaskKey = .TaskKey
                oServiceWorkManagerScheduledTasksList.TaskStatusKey = .TaskStatusKey
                oServiceWorkManagerScheduledTasksList.Type = .Type
                oServiceWorkManagerScheduledTasksList.Urgent = .Urgent
                oServiceWorkManagerScheduledTasksList.UserCode = .UserCode
                oServiceWorkManagerScheduledTasksList.UserGroupCode = .UserGroupCode
                oServiceWorkManagerScheduledTasksList.UserGroupDescription = .UserGroupDescription
                oServiceWorkManagerScheduledTasksList.UserGroupKey = .UserGroupKey
                oServiceWorkManagerScheduledTasksList.UserKey = .UserKey

            End With
        End If

        Return oServiceWorkManagerScheduledTasksList

    End Function


    Public Shared Function ToServiceAccountBalanceList(ByVal oAccountBalanceList As SFI.SAMForInsuranceV2.BaseGetAccountBalanceResponseTypeAccountBalanceRow) As BaseGetAccountBalanceResponseTypeRow

        Dim oServiceUserGroupList As New BaseGetAccountBalanceResponseTypeRow

        If oAccountBalanceList IsNot Nothing Then
            oServiceUserGroupList.SumAmount = oAccountBalanceList.SumAmount
            oServiceUserGroupList.CurrencyCode = oAccountBalanceList.CurrencyCode
            oServiceUserGroupList.Overdraft = oAccountBalanceList.Overdraft
            oServiceUserGroupList.FloatBalance = oAccountBalanceList.FloatBalance
        End If

        Return oServiceUserGroupList

    End Function

    Public Shared Function ToServiceGetFinancePlanDetailsInstalmentList(ByVal oGetFinancePlanDetailsInstalment As BaseImplementationTypes.BaseGetFinancePlanDetailsResponseTypeInstalmentsRow) As BaseGetFinancePlanDetailsResponseTypeRow

        Dim oServiceGetFinancePlanDetailsInstalmentList As New BaseGetFinancePlanDetailsResponseTypeRow

        If oGetFinancePlanDetailsInstalment IsNot Nothing Then

            With oGetFinancePlanDetailsInstalment

                oServiceGetFinancePlanDetailsInstalmentList.Amount = .Amount
                oServiceGetFinancePlanDetailsInstalmentList.DueDate = .DueDate
                oServiceGetFinancePlanDetailsInstalmentList.InstalmentNumber = .InstalmentNumber
                oServiceGetFinancePlanDetailsInstalmentList.PaymentDate = .PaymentDate
                oServiceGetFinancePlanDetailsInstalmentList.PaymentDateSpecified = .PaymentDateSpecified
                oServiceGetFinancePlanDetailsInstalmentList.Reason = .Reason
                oServiceGetFinancePlanDetailsInstalmentList.Status = .Status

            End With

        End If

        Return oServiceGetFinancePlanDetailsInstalmentList

    End Function

    Public Shared Function ToServiceGetAccountDetailsTransactionsList(ByVal oGetAccountDetailsTransactions As BaseImplementationTypes.BaseGetAccountDetailsResponseTypeTransactionsRow) As BaseGetAccountDetailsResponseTypeRow

        Dim oServiceGetAccountDetailsTransactionsList As New BaseGetAccountDetailsResponseTypeRow

        If oGetAccountDetailsTransactions IsNot Nothing Then

            With oGetAccountDetailsTransactions

                oServiceGetAccountDetailsTransactionsList.Account = .Account
                oServiceGetAccountDetailsTransactionsList.AccountAmount = .AccountAmount
                oServiceGetAccountDetailsTransactionsList.AccountCurrencyCode = .AccountCurrencyCode
                oServiceGetAccountDetailsTransactionsList.Accountkey = .Accountkey
                oServiceGetAccountDetailsTransactionsList.AccountOutStandingAmount = .AccountOutStandingAmount
                oServiceGetAccountDetailsTransactionsList.AltRef = .AltRef
                oServiceGetAccountDetailsTransactionsList.Amount = .Amount
                oServiceGetAccountDetailsTransactionsList.BalanceType = .BalanceType
                oServiceGetAccountDetailsTransactionsList.BaseCurrencyCode = .BaseCurrencyCode
                oServiceGetAccountDetailsTransactionsList.BGRef = .BGRef
                oServiceGetAccountDetailsTransactionsList.BranchKey = .BranchKey
                oServiceGetAccountDetailsTransactionsList.Client = .Client
                oServiceGetAccountDetailsTransactionsList.ClientCode = .ClientCode
                oServiceGetAccountDetailsTransactionsList.CurrencyAmount = .CurrencyAmount
                oServiceGetAccountDetailsTransactionsList.CurrencyCode = .CurrencyCode
                oServiceGetAccountDetailsTransactionsList.DocRef = .DocRef
                oServiceGetAccountDetailsTransactionsList.DocTypeDescription = .DocTypeDescription
                oServiceGetAccountDetailsTransactionsList.DocTypeId = .DocTypeId
                oServiceGetAccountDetailsTransactionsList.DocumentComment = .DocumentComment
                oServiceGetAccountDetailsTransactionsList.DocumentGroupCode = .DocumentGroupCode
                oServiceGetAccountDetailsTransactionsList.DocumentGroupId = .DocumentGroupId
                oServiceGetAccountDetailsTransactionsList.DocumentTypeCode = .DocumentTypeCode
                oServiceGetAccountDetailsTransactionsList.DueDate = .DueDate
                oServiceGetAccountDetailsTransactionsList.EffectiveDate = .EffectiveDate
                oServiceGetAccountDetailsTransactionsList.InstalmentCollection = .InstalmentCollection
                oServiceGetAccountDetailsTransactionsList.MediaRef = .MediaRef
                oServiceGetAccountDetailsTransactionsList.MediaType = .MediaType
                oServiceGetAccountDetailsTransactionsList.OperatorName = .OperatorName
                oServiceGetAccountDetailsTransactionsList.OutstandingAmount = .OutstandingAmount
                oServiceGetAccountDetailsTransactionsList.OutStandingCurrencyAmount = .OutStandingCurrencyAmount
                oServiceGetAccountDetailsTransactionsList.PaidDate = .PaidDate
                oServiceGetAccountDetailsTransactionsList.PayeeName = .PayeeName
                oServiceGetAccountDetailsTransactionsList.Period = .Period
                oServiceGetAccountDetailsTransactionsList.PrimarySettled = .PrimarySettled
                oServiceGetAccountDetailsTransactionsList.Reference = .Reference
                oServiceGetAccountDetailsTransactionsList.TransDate = .TransDate
                oServiceGetAccountDetailsTransactionsList.TransDetailKey = .TransDetailKey
                oServiceGetAccountDetailsTransactionsList.UnderwritingYear = .UnderwritingYear

            End With

        End If

        Return oServiceGetAccountDetailsTransactionsList

    End Function

    Public Shared Function ToServiceGetAccountingPeriodList(ByVal oGetAccountingPeriod As SFI.SAMForInsuranceV2.BaseGetAccountingPeriodResponseTypePeriodRow) As BaseGetAccountingPeriodResponseTypeRow

        Dim oServiceGetAccountingPeriodList As New BaseGetAccountingPeriodResponseTypeRow

        If oGetAccountingPeriod IsNot Nothing Then

            With oGetAccountingPeriod
                oServiceGetAccountingPeriodList.PeriodEndDate = .PeriodEndDate
                oServiceGetAccountingPeriodList.PeriodKey = .PeriodKey
                oServiceGetAccountingPeriodList.PeriodName = .PeriodName
                oServiceGetAccountingPeriodList.YearName = .YearName

            End With

        End If

        Return oServiceGetAccountingPeriodList

    End Function

    Public Shared Function ToServiceGetBankAccountsList(ByVal oGetBankAccounts As SFI.SAMForInsuranceV2.BaseGetBankAccountsResponseTypeBankAccountsRow) As BaseGetBankAccountsResponseTypeRow

        Dim oServiceGetBankAccountsList As New BaseGetBankAccountsResponseTypeRow

        If oGetBankAccounts IsNot Nothing Then

            With oGetBankAccounts
                oServiceGetBankAccountsList.BankAccountKey = .BankAccountKey
                oServiceGetBankAccountsList.BankAccountName = .BankAccountName
                oServiceGetBankAccountsList.BankAccountNumber = .BankAccountNumber
                oServiceGetBankAccountsList.Code = .Code
                oServiceGetBankAccountsList.CurrencyCode = .CurrencyCode
                oServiceGetBankAccountsList.CurrencyKey = .CurrencyKey
                oServiceGetBankAccountsList.Description = .Description
                oServiceGetBankAccountsList.EffectiveDate = .EffectiveDate
                oServiceGetBankAccountsList.IsDeleted = .IsDeleted

            End With

        End If

        Return oServiceGetBankAccountsList

    End Function

    Public Shared Function ToServiceGetClaimPaymentTaxGroupsList(ByVal oGetClaimPaymentTaxGroups As SFI.SAMForInsuranceV2.BaseGetClaimPaymentTaxGroupsResponseTypeTaxGroupRow) As BaseGetClaimPaymentTaxGroupsResponseTypeRow

        Dim oServiceGetClaimPaymentTaxGroupsList As New BaseGetClaimPaymentTaxGroupsResponseTypeRow

        If oGetClaimPaymentTaxGroups IsNot Nothing Then

            With oGetClaimPaymentTaxGroups
                oServiceGetClaimPaymentTaxGroupsList.Code = .Code
                oServiceGetClaimPaymentTaxGroupsList.Description = .Description
                oServiceGetClaimPaymentTaxGroupsList.IsWithholdingTax = .IsWithholdingTax
            End With

        End If

        Return oServiceGetClaimPaymentTaxGroupsList

    End Function

    Public Shared Function ToServiceGetEventNoteList(ByVal oGetEventNote As SFI.SAMForInsuranceV2.BaseGetEventNoteTypeResponseTypeEventTypesRow) As BaseGetEventNoteTypeResponseTypeRow

        Dim oServiceGetEventNoteList As New BaseGetEventNoteTypeResponseTypeRow

        If oGetEventNote IsNot Nothing Then

            With oGetEventNote
                oServiceGetEventNoteList.EventTypeCode = .EventTypeCode
                oServiceGetEventNoteList.EventTypeDescription = .EventTypeDescription
                oServiceGetEventNoteList.EventTypeKey = .EventTypeKey
            End With

        End If

        Return oServiceGetEventNoteList

    End Function

    Public Shared Function ToServiceGetFinancePlansList(ByVal oGetFinancePlans As SFI.SAMForInsuranceV2.BaseGetFinancePlansResponseTypeFinancePlansRow) As BaseGetFinancePlansResponseTypeRow

        Dim oServiceGetFinancePlansList As New BaseGetFinancePlansResponseTypeRow

        If oGetFinancePlans IsNot Nothing Then

            With oGetFinancePlans
                oServiceGetFinancePlansList.AccountNumber = .AccountNumber
                oServiceGetFinancePlansList.Amount = .Amount
                oServiceGetFinancePlansList.FinancePlanKey = .FinancePlanKey
                oServiceGetFinancePlansList.FinancePlanKey = .FinancePlanVersion
                oServiceGetFinancePlansList.FinanceProvider = .FinanceProvider
                oServiceGetFinancePlansList.Frequency = .Frequency
                oServiceGetFinancePlansList.InsuranceRef = .InsuranceRef
                oServiceGetFinancePlansList.NextDueDate = .NextDueDate
                oServiceGetFinancePlansList.PlanReference = .PlanReference
                oServiceGetFinancePlansList.RemainingInstalments = .RemainingInstalments
                oServiceGetFinancePlansList.Status = .Status
            End With

        End If

        Return oServiceGetFinancePlansList

    End Function

    Public Shared Function ToServiceGetInsurerPaymentsList(ByVal oGetFinancePlans As SFI.SAMForInsuranceV2.BaseGetInsurerPaymentsResponseTypeInsurerPaymentsRow) As BaseGetInsurerPaymentsResponseTypeRow

        Dim oServiceGetInsurerPaymentsList As New BaseGetInsurerPaymentsResponseTypeRow

        If oGetFinancePlans IsNot Nothing Then

            With oGetFinancePlans
                oServiceGetInsurerPaymentsList.AccountAmount = .AccountAmount
                oServiceGetInsurerPaymentsList.AccountBaseRate = .AccountBaseRate
                oServiceGetInsurerPaymentsList.AccountCurrencyCode = .AccountCurrencyCode
                oServiceGetInsurerPaymentsList.AccountCurrencyId = .AccountCurrencyId
                oServiceGetInsurerPaymentsList.AccountingDate = .AccountingDate
                oServiceGetInsurerPaymentsList.AlternateReference = .AlternateReference
                oServiceGetInsurerPaymentsList.BranchCode = .BranchCode
                oServiceGetInsurerPaymentsList.ClientOutstanding = .ClientOutstanding
                oServiceGetInsurerPaymentsList.ClientOutstandingAccountAmount = .ClientOutstandingAccountAmount
                oServiceGetInsurerPaymentsList.CompanyId = .CompanyId
                oServiceGetInsurerPaymentsList.ConsolidateBinder = .ConsolidateBinder
                oServiceGetInsurerPaymentsList.CurrencyAmount = .CurrencyAmount
                oServiceGetInsurerPaymentsList.CurrencyBaseRate = .CurrencyBaseRate
                oServiceGetInsurerPaymentsList.CurrencyCode = .CurrencyCode
                oServiceGetInsurerPaymentsList.CurrencyId = .CurrencyId
                oServiceGetInsurerPaymentsList.DocumentId = .DocumentId
                oServiceGetInsurerPaymentsList.DocumentRef = .DocumentRef
                oServiceGetInsurerPaymentsList.EffectiveDate = .EffectiveDate
                oServiceGetInsurerPaymentsList.FullyPaidAccountAmount = .FullyPaidAccountAmount
                oServiceGetInsurerPaymentsList.FullyPaidAmount = .FullyPaidAmount
                oServiceGetInsurerPaymentsList.InsurerRef = .InsurerRef
                oServiceGetInsurerPaymentsList.MarkedAccountAmount = .MarkedAccountAmount
                oServiceGetInsurerPaymentsList.MarkedAmount = .MarkedAmount
                oServiceGetInsurerPaymentsList.Month = .Month
                oServiceGetInsurerPaymentsList.PaidAccountAmount = .PaidAccountAmount
                oServiceGetInsurerPaymentsList.PaidAmount = .PaidAmount
                oServiceGetInsurerPaymentsList.PeriodName = .PeriodName
                oServiceGetInsurerPaymentsList.ResolvedName = .ResolvedName
                oServiceGetInsurerPaymentsList.ShortName = .ShortName
                oServiceGetInsurerPaymentsList.Spare = .Spare
                oServiceGetInsurerPaymentsList.TransdetailId = .TransdetailId
                oServiceGetInsurerPaymentsList.YearName = .YearName
            End With

        End If

        Return oServiceGetInsurerPaymentsList

    End Function

    Public Shared Function ToServiceGetListResponseList(ByVal oGetList As SFI.SAMForInsuranceV2.BaseGetListResponseTypeListRow) As BaseGetListResponseTypeRow

        Dim oServiceGetListResponseList As New BaseGetListResponseTypeRow

        If oGetList IsNot Nothing Then

            With oGetList
                oServiceGetListResponseList.Code = .Code
                oServiceGetListResponseList.Description = .Description
                oServiceGetListResponseList.EffectiveDate = .EffectiveDate
                oServiceGetListResponseList.IsDeleted = .IsDeleted
                oServiceGetListResponseList.Key = .Key
                oServiceGetListResponseList.ParentKey = .ParentKey
                oServiceGetListResponseList.ParentKeySpecified = .ParentKeySpecified
            End With
        End If

        Return oServiceGetListResponseList

    End Function

    Public Shared Function ToServiceGetMIDFileDetailsPoliciesList(ByVal oGetMIDFileDetails As SFI.SAMForInsuranceV2.BaseGetMIDFileDetailsResponseTypePoliciesRow) As BaseGetMIDFileDetailsResponseTypeRow

        Dim oServiceGetMIDFileDetailsPoliciesList As New BaseGetMIDFileDetailsResponseTypeRow

        If oGetMIDFileDetails IsNot Nothing Then

            With oGetMIDFileDetails
                oServiceGetMIDFileDetailsPoliciesList.BatchKey = .BatchKey
                oServiceGetMIDFileDetailsPoliciesList.BatchRef = .BatchRef
                oServiceGetMIDFileDetailsPoliciesList.ExpectedPPPC = .ExpectedPPPC
                oServiceGetMIDFileDetailsPoliciesList.InsuranceFileKey = .InsuranceFileKey
                oServiceGetMIDFileDetailsPoliciesList.InsuranceFileRef = .InsuranceFileRef
                oServiceGetMIDFileDetailsPoliciesList.MidPolicyKey = .MidPolicyKey
                oServiceGetMIDFileDetailsPoliciesList.MidPolicyKey = .MidPolicyStatusCode
                oServiceGetMIDFileDetailsPoliciesList.MidPolicyStatusCode = .PPPC
                oServiceGetMIDFileDetailsPoliciesList.MidPolicyStatusCode = .RejectErrorCodes
                oServiceGetMIDFileDetailsPoliciesList.MidPolicyStatusCode = .RejectReference
                oServiceGetMIDFileDetailsPoliciesList.StatusCode = .StatusCode
                oServiceGetMIDFileDetailsPoliciesList.UpdateType = .UpdateType
                If .Vehicles IsNot Nothing Then

                    oServiceGetMIDFileDetailsPoliciesList.Vehicles = .Vehicles.Row.ToList().ConvertAll( _
                            New Converter(Of SFI.SAMForInsuranceV2.BaseGetMIDFileDetailsResponseTypePoliciesRowVehiclesRow, BaseGetMIDFileDetailsResponseTypeRowRow)(AddressOf CommonFunctions.ToServiceGetMIDFileDetailsVehiclesList))
                End If

            End With
        End If

        Return oServiceGetMIDFileDetailsPoliciesList

    End Function

    Public Shared Function ToServiceGetMIDFileDetailsVehiclesList(ByVal oGetMIDFileDetails As SFI.SAMForInsuranceV2.BaseGetMIDFileDetailsResponseTypePoliciesRowVehiclesRow) As BaseGetMIDFileDetailsResponseTypeRowRow

        Dim oServiceGetMIDFileDetailsVehiclesList As New BaseGetMIDFileDetailsResponseTypeRowRow

        If oGetMIDFileDetails IsNot Nothing Then

            With oGetMIDFileDetails
                oServiceGetMIDFileDetailsVehiclesList.IsForeignReg = .IsForeignReg
                oServiceGetMIDFileDetailsVehiclesList.IsTradeReg = .IsTradeReg
                oServiceGetMIDFileDetailsVehiclesList.Make = .Make
                oServiceGetMIDFileDetailsVehiclesList.MIDPolicyKey = .MIDPolicyKey
                oServiceGetMIDFileDetailsVehiclesList.MIDVehicleKey = .MIDVehicleKey
                oServiceGetMIDFileDetailsVehiclesList.Model = .Model
                oServiceGetMIDFileDetailsVehiclesList.OffDate = .OffDate
                oServiceGetMIDFileDetailsVehiclesList.OnDate = .OnDate
                oServiceGetMIDFileDetailsVehiclesList.Registration = .Registration
                oServiceGetMIDFileDetailsVehiclesList.RejectErrorCodes = .RejectErrorCodes
                oServiceGetMIDFileDetailsVehiclesList.RejectReference = .RejectReference
                oServiceGetMIDFileDetailsVehiclesList.StatusCode = .StatusCode
                oServiceGetMIDFileDetailsVehiclesList.UpdateType = .UpdateType

            End With
        End If

        Return oServiceGetMIDFileDetailsVehiclesList

    End Function

    Public Shared Function ToServiceGetMIDFilesMIDFilesList(ByVal oGetMIDFilesMIDFiles As SFI.SAMForInsuranceV2.BaseGetMIDFilesResponseTypeMIDFilesRow) As BaseGetMIDFilesResponseTypeRow

        Dim oServiceGetMIDFilesMIDFilesList As New BaseGetMIDFilesResponseTypeRow

        If oGetMIDFilesMIDFiles IsNot Nothing Then

            With oGetMIDFilesMIDFiles
                oServiceGetMIDFilesMIDFilesList.DateGenerated = .DateGenerated
                oServiceGetMIDFilesMIDFilesList.FileName = .FileName
                oServiceGetMIDFilesMIDFilesList.FileSequenceNumber = .FileSequenceNumber
                oServiceGetMIDFilesMIDFilesList.MIDFileKey = .MIDFileKey
                oServiceGetMIDFilesMIDFilesList.MIDFileKeySpecified = .MIDFileKeySpecified
                oServiceGetMIDFilesMIDFilesList.StatusDescription = .StatusDescription

            End With
        End If

        Return oServiceGetMIDFilesMIDFilesList

    End Function

    Public Shared Function ToServiceGetPeriodList(ByVal oGetPeriod As SFI.SAMForInsuranceV2.BaseGetPeriodResponseTypePeriodsRow) As BaseGetPeriodResponseTypeRow

        Dim oServiceGetPeriodList As New BaseGetPeriodResponseTypeRow

        If oGetPeriod IsNot Nothing Then

            With oGetPeriod
                oServiceGetPeriodList.AllocationIndicator = .AllocationIndicator
                oServiceGetPeriodList.PeriodID = .PeriodID
                oServiceGetPeriodList.PeriodName = .PeriodName
                oServiceGetPeriodList.YearName = .YearName

            End With
        End If

        Return oServiceGetPeriodList

    End Function

    Public Shared Function ToServiceGetPoliciesForRenewalSelectionList(ByVal oGetPoliciesForRenewalSelection As SFI.SAMForInsuranceV2.BaseGetPoliciesForRenewalSelectionResponseTypePoliciesRow) As BaseGetPoliciesForRenewalSelectionResponseTypeRow

        Dim oServiceGetPoliciesForRenewalSelectionList As New BaseGetPoliciesForRenewalSelectionResponseTypeRow

        If oGetPoliciesForRenewalSelection IsNot Nothing Then

            With oGetPoliciesForRenewalSelection
                oServiceGetPoliciesForRenewalSelectionList.AnniversaryCopy = .AnniversaryCopy
                oServiceGetPoliciesForRenewalSelectionList.Client = .Client
                oServiceGetPoliciesForRenewalSelectionList.ClientCode = .ClientCode
                oServiceGetPoliciesForRenewalSelectionList.CoverEndDate = .CoverEndDate
                oServiceGetPoliciesForRenewalSelectionList.CoverStartDate = .CoverStartDate
                oServiceGetPoliciesForRenewalSelectionList.InsuranceFileKey = .InsuranceFileKey
                oServiceGetPoliciesForRenewalSelectionList.InsuranceFileRef = .InsuranceFileRef
                oServiceGetPoliciesForRenewalSelectionList.InsuranceFolderKey = .InsuranceFolderKey
                oServiceGetPoliciesForRenewalSelectionList.IsAutoRenewable = .IsAutoRenewable
                oServiceGetPoliciesForRenewalSelectionList.IsClosed = .IsClosed
                oServiceGetPoliciesForRenewalSelectionList.IsInTransferMode = .IsInTransferMode
                oServiceGetPoliciesForRenewalSelectionList.IsTrueMonthlyPolicy = .IsTrueMonthlyPolicy
                oServiceGetPoliciesForRenewalSelectionList.LeadAgent = .LeadAgent
                oServiceGetPoliciesForRenewalSelectionList.LeadAgentKey = .LeadAgentKey
                oServiceGetPoliciesForRenewalSelectionList.PartyKey = .PartyKey
                oServiceGetPoliciesForRenewalSelectionList.ProductDescription = .ProductDescription
                oServiceGetPoliciesForRenewalSelectionList.ProductKey = .ProductKey
                oServiceGetPoliciesForRenewalSelectionList.RenewalCount = .RenewalCount
                oServiceGetPoliciesForRenewalSelectionList.RenewalDate = .RenewalDate

            End With
        End If

        Return oServiceGetPoliciesForRenewalSelectionList

    End Function

    Public Shared Function ToServiceGetPoliciesInRenewalList(ByVal oGetPoliciesInRenewal As SFI.SAMForInsuranceV2.BaseGetPoliciesInRenewalResponseTypePoliciesRow) As BaseGetPoliciesInRenewalResponseTypeRow

        Dim oServiceGetPoliciesInRenewalList As New BaseGetPoliciesInRenewalResponseTypeRow

        If oGetPoliciesInRenewal IsNot Nothing Then

            With oGetPoliciesInRenewal
                oServiceGetPoliciesInRenewalList.AccHandler = .AccHandler
                oServiceGetPoliciesInRenewalList.AnniversaryCopy = .AnniversaryCopy
                oServiceGetPoliciesInRenewalList.BranchCode = .BranchCode
                oServiceGetPoliciesInRenewalList.ClaimIndicator = .ClaimIndicator
                oServiceGetPoliciesInRenewalList.CoverEndDate = .CoverEndDate
                oServiceGetPoliciesInRenewalList.CoverStartDate = .CoverStartDate
                oServiceGetPoliciesInRenewalList.InsuranceFileKey = .InsuranceFileKey
                oServiceGetPoliciesInRenewalList.InsuranceFileRef = .InsuranceFileRef
                oServiceGetPoliciesInRenewalList.InsuranceFileStatusDescription = .InsuranceFileStatusDescription
                oServiceGetPoliciesInRenewalList.InsuranceFileTypeDescription = .InsuranceFileTypeDescription
                oServiceGetPoliciesInRenewalList.InsuranceFolderKey = .InsuranceFolderKey
                oServiceGetPoliciesInRenewalList.IsClosed = .IsClosed
                oServiceGetPoliciesInRenewalList.IsTrueMonthlyPolicy = .IsTrueMonthlyPolicy
                oServiceGetPoliciesInRenewalList.LeadAgent = .LeadAgent
                oServiceGetPoliciesInRenewalList.LeadAgentKey = .LeadAgentKey
                oServiceGetPoliciesInRenewalList.PartyKey = .PartyKey
                oServiceGetPoliciesInRenewalList.PartyName = .PartyName
                oServiceGetPoliciesInRenewalList.ProductCode = .ProductCode
                oServiceGetPoliciesInRenewalList.ProductDescription = .ProductDescription
                oServiceGetPoliciesInRenewalList.RenewalDate = .RenewalDate
                oServiceGetPoliciesInRenewalList.RenewalPremium = .RenewalPremium
                oServiceGetPoliciesInRenewalList.RenewalStatusKey = .RenewalStatusKey
                oServiceGetPoliciesInRenewalList.RenewalStatusTypeCode = .RenewalStatusTypeCode
                oServiceGetPoliciesInRenewalList.RenewalStatusTypeDescription = .RenewalStatusTypeDescription

            End With
        End If

        Return oServiceGetPoliciesInRenewalList

    End Function


    Public Shared Function ToServiceGetTaskGroupTasksList(ByVal oGetTaskGroupTasks As SFI.SAMForInsuranceV2.BaseGetTaskGroupTasksResponseTypeTaskGroupTasksRow) As BaseGetTaskGroupTasksResponseTypeRow

        Dim oServiceGetTaskGroupTasksList As New BaseGetTaskGroupTasksResponseTypeRow

        If oGetTaskGroupTasks IsNot Nothing Then

            With oGetTaskGroupTasks
                oServiceGetTaskGroupTasksList.Description = .Description
                oServiceGetTaskGroupTasksList.DisplayIcon = .DisplayIcon
                oServiceGetTaskGroupTasksList.EffectiveDate = .EffectiveDate
                oServiceGetTaskGroupTasksList.IsAvailable = .IsAvailable
                oServiceGetTaskGroupTasksList.IsDeleted = .IsDeleted
                oServiceGetTaskGroupTasksList.IsIncluded = .IsIncluded
                oServiceGetTaskGroupTasksList.IsViewOnly = .IsViewOnly
                oServiceGetTaskGroupTasksList.Name = .Name
                oServiceGetTaskGroupTasksList.TaskCategoryKey = .TaskCategoryKey
                oServiceGetTaskGroupTasksList.TaskKey = .TaskKey

            End With
        End If

        Return oServiceGetTaskGroupTasksList

    End Function

    Public Shared Function ToServiceGetTaskGroupsList(ByVal oGetTaskGroups As SFI.SAMForInsuranceV2.BaseGetTaskGroupsResponseTypeTaskGroupsRow) As BaseGetTaskGroupsResponseTypeRow

        Dim oServiceGetTaskGroupsList As New BaseGetTaskGroupsResponseTypeRow

        If oGetTaskGroups IsNot Nothing Then

            With oGetTaskGroups
                oServiceGetTaskGroupsList.Description = .CaptionID
                oServiceGetTaskGroupsList.Code = .Code
                oServiceGetTaskGroupsList.Description = .Description
                oServiceGetTaskGroupsList.EffectiveDate = .EffectiveDate
                oServiceGetTaskGroupsList.IsDeleted = .IsDeleted
                oServiceGetTaskGroupsList.TaskGroupCategoryKey = .TaskGroupCategoryKey
                oServiceGetTaskGroupsList.TaskGroupKey = .TaskGroupKey

            End With
        End If

        Return oServiceGetTaskGroupsList

    End Function

    Public Shared Function ToServiceGetTaxGroupsForClaimsList(ByVal oGetTaxGroupsForClaims As SFI.SAMForInsuranceV2.BaseGetTaxGroupsForClaimsResponseTypeTaxGroupsRow) As BaseGetTaxGroupsForClaimsResponseTypeRow

        Dim oServiceGetTaxGroupsForClaimsList As New BaseGetTaxGroupsForClaimsResponseTypeRow

        If oGetTaxGroupsForClaims IsNot Nothing Then

            With oGetTaxGroupsForClaims
                oServiceGetTaxGroupsForClaimsList.AdvanceTaxScript = .AdvanceTaxScript
                oServiceGetTaxGroupsForClaimsList.Code = .Code
                oServiceGetTaxGroupsForClaimsList.Description = .Description
                oServiceGetTaxGroupsForClaimsList.IsWithHoldingTax = .IsWithHoldingTax
                oServiceGetTaxGroupsForClaimsList.TaxGroupKey = .TaxGroupKey

            End With
        End If

        Return oServiceGetTaxGroupsForClaimsList

    End Function

    Public Shared Function ToServiceGetUserGroupTaskGroupsList(ByVal oGetUserGroupTaskGroups As SFI.SAMForInsuranceV2.BaseGetUserGroupTaskGroupsResponseTypeTaskGroupsRow) As BaseGetUserGroupTaskGroupsResponseTypeRow

        Dim oServiceGetUserGroupTaskGroupsList As New BaseGetUserGroupTaskGroupsResponseTypeRow

        If oGetUserGroupTaskGroups IsNot Nothing Then

            With oGetUserGroupTaskGroups
                oServiceGetUserGroupTaskGroupsList.Code = .Code
                oServiceGetUserGroupTaskGroupsList.Description = .Description
                oServiceGetUserGroupTaskGroupsList.DisplaySequence = .DisplaySequence
                oServiceGetUserGroupTaskGroupsList.EffectiveDate = .EffectiveDate
                oServiceGetUserGroupTaskGroupsList.IsDeleted = .IsDeleted
                oServiceGetUserGroupTaskGroupsList.IsIncluded = .IsIncluded
                oServiceGetUserGroupTaskGroupsList.TaskGroupKey = .TaskGroupKey
            End With
        End If

        Return oServiceGetUserGroupTaskGroupsList

    End Function

    Public Shared Function ToServiceGetUserGroupUsersList(ByVal oGetUserGroupUsers As SFI.SAMForInsuranceV2.BaseGetUserGroupUsersResponseTypeUserGroupUsersRow) As BaseGetUserGroupUsersResponseTypeRow

        Dim oServiceGetUserGroupUsersList As New BaseGetUserGroupUsersResponseTypeRow

        If oGetUserGroupUsers IsNot Nothing Then

            With oGetUserGroupUsers
                oServiceGetUserGroupUsersList.EmailAddress = .EmailAddress
                oServiceGetUserGroupUsersList.Name = .Name
                oServiceGetUserGroupUsersList.UserKey = .UserKey

            End With
        End If

        Return oServiceGetUserGroupUsersList

    End Function

    Public Shared Function ToServiceGetUserGroupsList(ByVal oGetUserGroups As SFI.SAMForInsuranceV2.BaseGetUserGroupsResponseTypeUserGroupsRow) As BaseGetUserGroupsResponseTypeRow

        Dim oServiceGetUserGroupsList As New BaseGetUserGroupsResponseTypeRow

        If oGetUserGroups IsNot Nothing Then

            With oGetUserGroups
                oServiceGetUserGroupsList.Code = .Code
                oServiceGetUserGroupsList.Description = .Description
                oServiceGetUserGroupsList.EffectiveDate = .EffectiveDate
                oServiceGetUserGroupsList.IsDeleted = .IsDeleted
                oServiceGetUserGroupsList.IsSystemAdmin = .IsSystemAdmin
                oServiceGetUserGroupsList.UserGroupKey = .UserGroupKey
            End With
        End If

        Return oServiceGetUserGroupsList

    End Function

    Public Shared Function ToServiceGetUserGroupsbyTaskList(ByVal oGetUserGroupsbyTask As SFI.SAMForInsuranceV2.BaseGetUserGroupsbyTaskResponseTypeUserGroupsRow) As BaseGetUserGroupsbyTaskResponseTypeRow

        Dim oServiceGetUserGroupsbyTaskList As New BaseGetUserGroupsbyTaskResponseTypeRow

        If oGetUserGroupsbyTask IsNot Nothing Then

            With oGetUserGroupsbyTask
                oServiceGetUserGroupsbyTaskList.UserGroupCode = .UserGroupCode
                oServiceGetUserGroupsbyTaskList.UserGroupDescription = .UserGroupDescription
                oServiceGetUserGroupsbyTaskList.UserGroupKey = .UserGroupKey
            End With
        End If

        Return oServiceGetUserGroupsbyTaskList

    End Function

    Public Shared Function ToServiceValidateBankAccountNumberList(ByVal oValidateBankAccountNumber As SFI.SAMForInsuranceV2.BaseValidateBankAccountNumberResponseTypeValidationDetailsRow) As BaseValidateBankAccountNumberResponseTypeRow

        Dim oServiceValidateBankAccountNumberList As New BaseValidateBankAccountNumberResponseTypeRow

        If oValidateBankAccountNumber IsNot Nothing Then

            With oValidateBankAccountNumber
                oServiceValidateBankAccountNumberList.AddressLine1 = .AddressLine1
                oServiceValidateBankAccountNumberList.AddressLine2 = .AddressLine2
                oServiceValidateBankAccountNumberList.AddressLine3 = .AddressLine3
                oServiceValidateBankAccountNumberList.AddressLine4 = .AddressLine4
                oServiceValidateBankAccountNumberList.BankName = .BankName
                oServiceValidateBankAccountNumberList.IsValid = .IsValid
                oServiceValidateBankAccountNumberList.IsValidationOverridable = .IsValidationOverridable
                oServiceValidateBankAccountNumberList.IsValidationOverridableSpecified = .IsValidationOverridableSpecified
                oServiceValidateBankAccountNumberList.IsValidSpecified = .IsValidSpecified
                oServiceValidateBankAccountNumberList.PostalCode = .PostalCode
                oServiceValidateBankAccountNumberList.ValidationMessageDataset = .ValidationMessageDataset
            End With
        End If

        Return oServiceValidateBankAccountNumberList

    End Function

    Public Shared Function ToBaseImpBasePostDocumentRequestType(ByVal oServicePostDocumentRequestType As BasePostDocumentRequestType) As SAMForInsuranceV2ImplementationTypes.PostDocumentRequestType
        Dim oImpPostDocumentRequestType As New SAMForInsuranceV2ImplementationTypes.PostDocumentRequestType

        If oServicePostDocumentRequestType IsNot Nothing Then

            oImpPostDocumentRequestType.BranchCode = oServicePostDocumentRequestType.BranchCode
            oImpPostDocumentRequestType.DocumentType = CType([Enum].ToObject(GetType(DocumentTypeType), oServicePostDocumentRequestType.DocumentType), BaseImplementationTypes.DocumentTypeType)
            'oImpPostDocumentRequestType.DocumentType = oServicePostDocumentRequestType.DocumentType
            oImpPostDocumentRequestType.Comment = oServicePostDocumentRequestType.Comment
            oImpPostDocumentRequestType.DocumentReference = oServicePostDocumentRequestType.DocumentReference
            oImpPostDocumentRequestType.DocumentTypeCode = oServicePostDocumentRequestType.DocumentTypeCode
            oImpPostDocumentRequestType.SAMStagingPolicyKey = oServicePostDocumentRequestType.SAMStagingPolicyKey

            If oServicePostDocumentRequestType.Transactions IsNot Nothing Then

                oImpPostDocumentRequestType.Transactions = Array.ConvertAll( _
                                                            oServicePostDocumentRequestType.Transactions.ToArray(), _
                                                            New Converter(Of BaseTransactionType,  _
                                                                BaseImplementationTypes.BaseTransactionType) _
                                                                (AddressOf ToBaseImpBaseTransactionType))

            End If
        End If

        Return oImpPostDocumentRequestType

    End Function

    Public Shared Function ToBaseImpBaseTransactionType(ByVal oServiceTransactionType As BaseTransactionType) As BaseImplementationTypes.BaseTransactionType
        Dim oImpTransactionType As BaseImplementationTypes.BaseTransactionType = New BaseImplementationTypes.BaseTransactionType

        If oServiceTransactionType IsNot Nothing Then

            oImpTransactionType.AccountCode = oServiceTransactionType.AccountCode
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


    Public Shared Function ToServiceGetRecoveryCoinsuranceList(ByVal oGetRecoveryCoinsurance As SFI.SAMForInsuranceV2.BaseGetRecoveryCoinsuranceResponseTypeCoinsurancesRow) As BaseGetRecoveryCoinsuranceResponseTypeCoinsurancesRow

        Dim oServiceGetRecoveryCoinsuranceList As New BaseGetRecoveryCoinsuranceResponseTypeCoinsurancesRow

        If oGetRecoveryCoinsurance IsNot Nothing Then

            With oGetRecoveryCoinsurance
                oServiceGetRecoveryCoinsuranceList.Coinsurer = .Coinsurer
                oServiceGetRecoveryCoinsuranceList.PartyKey = .PartyKey
                oServiceGetRecoveryCoinsuranceList.RecoveryKey = .RecoveryKey
                oServiceGetRecoveryCoinsuranceList.RecoveryToDate = .RecoveryToDate
                oServiceGetRecoveryCoinsuranceList.RecoveryType = .RecoveryType
                oServiceGetRecoveryCoinsuranceList.SharePercent = .SharePercent
            End With
        End If

        Return oServiceGetRecoveryCoinsuranceList

    End Function

    Public Shared Function ToServiceGetWmTaskList(ByVal oGetWmTask As SFI.SAMForInsuranceV2.BaseGetWmTaskResponseTypeKeyDataRow) As BaseGetWmTaskResponseTypeRow

        Dim oServiceGetWmTaskList As New BaseGetWmTaskResponseTypeRow

        If oGetWmTask IsNot Nothing Then

            With oGetWmTask
                oServiceGetWmTaskList.KeyName = .KeyName
                oServiceGetWmTaskList.KeyValue = .KeyValue

            End With
        End If

        Return oServiceGetWmTaskList

    End Function

    Public Shared Function ToServiceGetWmTaskLogList(ByVal oGetWmTaskLog As SFI.SAMForInsuranceV2.BaseGetWmTaskLogResponseTypeTaskLogRow) As BaseGetWmTaskLogResponseTypeTaskLogRow

        Dim oServiceGetWmTaskLogList As New BaseGetWmTaskLogResponseTypeTaskLogRow

        If oGetWmTaskLog IsNot Nothing Then

            With oGetWmTaskLog
                oServiceGetWmTaskLogList.CreatedByKey = .CreatedByKey
                oServiceGetWmTaskLogList.DateCreated = .DateCreated
                oServiceGetWmTaskLogList.LogText = .LogText
                oServiceGetWmTaskLogList.TaskInstanceKey = .TaskInstanceKey
                oServiceGetWmTaskLogList.UserName = .UserName

            End With
        End If

        Return oServiceGetWmTaskLogList

    End Function

    Public Shared Function ToServiceGetClaimRiskLinksResponseTypePerilType(ByVal oImpPerilType As BaseImplementationTypes.BaseGetClaimRiskLinksResponseTypePerilType) As BaseGetClaimRiskLinksResponseTypePerilType

        Dim oServicePerilType As New BaseGetClaimRiskLinksResponseTypePerilType

        If oImpPerilType IsNot Nothing Then

            oServicePerilType.Code = oImpPerilType.Code
            oServicePerilType.Description = oImpPerilType.Description
            oServicePerilType.SumInsured = oImpPerilType.SumInsured


            If oImpPerilType.ReserveType IsNot Nothing Then

                oServicePerilType.ReserveType = oImpPerilType.ReserveType.ToList().ConvertAll( _
                        New Converter(Of SFI.SAMForInsuranceV2.BaseGetClaimRiskLinksResponseTypePerilTypeReserveType, BaseGetClaimRiskLinksResponseTypePerilTypeReserveType)(AddressOf CommonFunctions.ToServiceGetClaimPerilReserveType))
            End If

            If oImpPerilType.RecoveryType IsNot Nothing Then

                oServicePerilType.RecoveryType = oImpPerilType.RecoveryType.ToList().ConvertAll( _
                        New Converter(Of BaseImplementationTypes.BaseGetClaimRiskLinksResponseTypePerilTypeRecoveryType, BaseGetClaimRiskLinksResponseTypePerilTypeRecoveryType)(AddressOf CommonFunctions.ToServiceGetClaimPerilRecoveryType))
            End If

        End If

        Return oServicePerilType

    End Function

    Public Shared Function ToServiceGetClaimPerilReserveType(ByVal msgClaimPerilReserveType As SFI.SAMForInsuranceV2.BaseGetClaimRiskLinksResponseTypePerilTypeReserveType) As BaseGetClaimRiskLinksResponseTypePerilTypeReserveType
        Dim impClaimPerilReserveType As BaseGetClaimRiskLinksResponseTypePerilTypeReserveType = New BaseGetClaimRiskLinksResponseTypePerilTypeReserveType

        If msgClaimPerilReserveType IsNot Nothing Then

            impClaimPerilReserveType.Code = msgClaimPerilReserveType.Code
            impClaimPerilReserveType.Description = msgClaimPerilReserveType.Description

        End If

        Return impClaimPerilReserveType

    End Function

    Public Shared Function ToServiceGetClaimPerilRecoveryType(ByVal oImpPerilTypeRecoveryType As BaseImplementationTypes.BaseGetClaimRiskLinksResponseTypePerilTypeRecoveryType) As BaseGetClaimRiskLinksResponseTypePerilTypeRecoveryType

        Dim oServicePerilTypeRecoveryType As New BaseGetClaimRiskLinksResponseTypePerilTypeRecoveryType

        If oImpPerilTypeRecoveryType IsNot Nothing Then

            oServicePerilTypeRecoveryType.Code = oImpPerilTypeRecoveryType.Code
            oServicePerilTypeRecoveryType.Description = oImpPerilTypeRecoveryType.Description
            oServicePerilTypeRecoveryType.IsSalvage = oImpPerilTypeRecoveryType.IsSalvage

        End If

        Return oServicePerilTypeRecoveryType

    End Function

    Public Shared Function ToServiceGetClaimSummaryPerilTotals(ByVal oGetClaimPerilTotals As SFI.SAMForInsuranceV2.BaseGetClaimPerilSummaryResponseTypePerilTotalsRow) As BaseGetClaimPerilSummaryResponseTypeRow

        Dim oServiceGetClaimPerilTotals As BaseGetClaimPerilSummaryResponseTypeRow = New BaseGetClaimPerilSummaryResponseTypeRow

        If oGetClaimPerilTotals IsNot Nothing Then
            oServiceGetClaimPerilTotals.Average = oGetClaimPerilTotals.Average
            oServiceGetClaimPerilTotals.CurrentReserve = oGetClaimPerilTotals.CurrentReserve
            oServiceGetClaimPerilTotals.Description = oGetClaimPerilTotals.Description
            oServiceGetClaimPerilTotals.InitialReserve = oGetClaimPerilTotals.InitialReserve
            oServiceGetClaimPerilTotals.PaidAmount = oGetClaimPerilTotals.PaidAmount
            oServiceGetClaimPerilTotals.RevisedReserve = oGetClaimPerilTotals.RevisedReserve
            oServiceGetClaimPerilTotals.SumInsured = oGetClaimPerilTotals.SumInsured
        End If

        Return oServiceGetClaimPerilTotals

    End Function


    Public Shared Function ToServiceGetClaimSummaryRecoveryPerils(ByVal oGetClaimPerilRecoveryPerils As SFI.SAMForInsuranceV2.BaseGetClaimPerilSummaryResponseTypeTPRecoveryPerilsRow) As BaseGetClaimPerilSummaryResponseTypeRow1

        Dim oServiceGetClaimPerilRecoveryPerils As BaseGetClaimPerilSummaryResponseTypeRow1 = New BaseGetClaimPerilSummaryResponseTypeRow1

        If oGetClaimPerilRecoveryPerils IsNot Nothing Then
            oServiceGetClaimPerilRecoveryPerils.CurrentRecovery = oGetClaimPerilRecoveryPerils.CurrentRecovery
            oServiceGetClaimPerilRecoveryPerils.Description = oGetClaimPerilRecoveryPerils.Description
            oServiceGetClaimPerilRecoveryPerils.InitialRecovery = oGetClaimPerilRecoveryPerils.InitialRecovery
            oServiceGetClaimPerilRecoveryPerils.ReceiptedAmount = oGetClaimPerilRecoveryPerils.ReceiptedAmount
            oServiceGetClaimPerilRecoveryPerils.RevisedRecovery = oGetClaimPerilRecoveryPerils.RevisedRecovery
        End If

        Return oServiceGetClaimPerilRecoveryPerils

    End Function

    Public Shared Function ToServiceGetClaimSummarySalvageRecoveryPerils(ByVal oGetClaimSummarySalvageRecoveryPerils As SFI.SAMForInsuranceV2.BaseGetClaimPerilSummaryResponseTypeSalvageRecoveryPerilsRow) As BaseGetClaimPerilSummaryResponseTypeRow2

        Dim oServiceGetClaimSummarySalvageRecoveryPerils As BaseGetClaimPerilSummaryResponseTypeRow2 = New BaseGetClaimPerilSummaryResponseTypeRow2

        If oGetClaimSummarySalvageRecoveryPerils IsNot Nothing Then
            oServiceGetClaimSummarySalvageRecoveryPerils.CurrentRecovery = oGetClaimSummarySalvageRecoveryPerils.CurrentRecovery
            oServiceGetClaimSummarySalvageRecoveryPerils.Description = oGetClaimSummarySalvageRecoveryPerils.Description
            oServiceGetClaimSummarySalvageRecoveryPerils.InitialRecovery = oGetClaimSummarySalvageRecoveryPerils.InitialRecovery
            oServiceGetClaimSummarySalvageRecoveryPerils.ReceiptedAmount = oGetClaimSummarySalvageRecoveryPerils.ReceiptedAmount
            oServiceGetClaimSummarySalvageRecoveryPerils.RevisedRecovery = oGetClaimSummarySalvageRecoveryPerils.RevisedRecovery
        End If

        Return oServiceGetClaimSummarySalvageRecoveryPerils

    End Function

    Public Shared Function ToServiceGetClaimSummaryPerils(ByVal oGetClaimSummaryPerils As SFI.SAMForInsuranceV2.BaseGetClaimPerilSummaryResponseTypeReserveTypePerilsRow) As BaseGetClaimPerilSummaryResponseTypeReserveTypeRow

        Dim oServiceGetClaimSummaryPerils As BaseGetClaimPerilSummaryResponseTypeReserveTypeRow = New BaseGetClaimPerilSummaryResponseTypeReserveTypeRow

        If oGetClaimSummaryPerils IsNot Nothing Then
            oServiceGetClaimSummaryPerils.Average = oGetClaimSummaryPerils.Average
            oServiceGetClaimSummaryPerils.CurrentReserve = oGetClaimSummaryPerils.CurrentReserve
            oServiceGetClaimSummaryPerils.Description = oGetClaimSummaryPerils.Description
            oServiceGetClaimSummaryPerils.InitialReserve = oGetClaimSummaryPerils.InitialReserve
            oServiceGetClaimSummaryPerils.PaidAmount = oGetClaimSummaryPerils.PaidAmount
            oServiceGetClaimSummaryPerils.RevisedReserve = oGetClaimSummaryPerils.RevisedReserve
            oServiceGetClaimSummaryPerils.SumInsured = oGetClaimSummaryPerils.SumInsured
        End If

        Return oServiceGetClaimSummaryPerils

    End Function

    Public Shared Function ToBaseImpBaseClaimReceiptItemType(ByVal oService As BaseClaimReceiptItemType) As BaseImplementationTypes.BaseClaimReceiptItemType

        Dim oImplementation As BaseImplementationTypes.BaseClaimReceiptItemType = New BaseImplementationTypes.BaseClaimReceiptItemType

        If oService IsNot Nothing Then

            oImplementation.BaseRecoveryKey = oService.BaseRecoveryKey
            oImplementation.ReceiptAmount = oService.ReceiptAmount
            oImplementation.TaxGroupCode = oService.TaxGroupCode

        End If

        Return oImplementation

    End Function

    Public Shared Function ToServiceBaseGetClaimPaymentTaxesResponseTypePaymentType( _
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

    Public Shared Function ToServiceBaseClaimPerilReservePaymentType( _
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

    Public Shared Function ToServiceBaseClaimPaymentTaxItemType( _
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


    ''' <summary>
    ''' This method converts an error object of type SiriusFS.SAM.Structure.BaseImplementationTypes.SAMError to SiriusFS.
    ''' </summary>
    ''' <param name="oError">Error object of type SiriusFS.SAM.Structure.BaseImplementationTypes.SAMError</param>
    ''' <returns>Error object of type Sirius.Architecture.ExceptionHandling.SAMError</returns>
    ''' <remarks></remarks>

    Public Shared Function ConvertToSFIV2SAMError(ByVal oError As BaseImplementationTypes.SAMError) As SFI.SAMForInsuranceV2.WCF.SAMError

        'Dim oReturnError As New List(Of SFI.SAMForInsuranceV2.WCF.SAMError)
        Select Case oError.GetType()
            Case GetType(BaseImplementationTypes.SAMErrorInvalidData)
                Dim oSError As SiriusFS.SAM.Structure.BaseImplementationTypes.SAMErrorInvalidData = CType(oError, BaseImplementationTypes.SAMErrorInvalidData)
                Dim oDError As New SFI.SAMForInsuranceV2.WCF.SAMErrorInvalidData
                oDError.Code = oSError.Code
                oDError.Description = oSError.Description
                oDError.FieldName = oSError.FieldName
                oDError.SuppliedValue = oSError.SuppliedValue
                Return oDError
            Case GetType(BaseImplementationTypes.SAMErrorBusinessRule)
                Dim oSError As SiriusFS.SAM.Structure.BaseImplementationTypes.SAMErrorBusinessRule = CType(oError, BaseImplementationTypes.SAMErrorBusinessRule)
                Dim oDError As New SFI.SAMForInsuranceV2.WCF.SAMErrorBusinessRule
                oDError.Code = oSError.Code
                oDError.Description = oSError.Description
                oDError.Detail = oSError.Detail
                Return oDError
            Case GetType(SiriusFS.SAM.Structure.BaseImplementationTypes.SAMErrorFatal)
                Dim osError As SiriusFS.SAM.Structure.BaseImplementationTypes.SAMErrorFatal = CType(oError, BaseImplementationTypes.SAMErrorFatal)
                Dim oDError As New SFI.SAMForInsuranceV2.WCF.SAMErrorFatal
                oDError.Type = osError.Type
                Return oDError
        End Select
    End Function


    Public Shared Function ToServicePolicyVersionList(ByVal oPolicyVersionList As SFI.SAMForInsuranceV2.BaseGetAllPolicyVersionsResponseTypePoliciesRow) As BaseGetAllPolicyVersionsResponseTypeRow

        Dim oServicePolicyVersionList As New BaseGetAllPolicyVersionsResponseTypeRow

        If oPolicyVersionList IsNot Nothing Then
            With oServicePolicyVersionList
                .PolicyVersion = oPolicyVersionList.PolicyVersion
                .InsuranceFolderKey = oPolicyVersionList.InsuranceFolderKey
                .insuranceFileKey = oPolicyVersionList.insuranceFileKey
                .InsuranceHolderKey = oPolicyVersionList.InsuranceHolderKey
                .PolicyTypeCode = oPolicyVersionList.PolicyTypeCode
                .PolicyRef = oPolicyVersionList.PolicyRef
                .InsuranceFileTypeDesc = oPolicyVersionList.InsuranceFileTypeDesc
                .ProductDesc = oPolicyVersionList.ProductDesc
                .RenewalDate = oPolicyVersionList.RenewalDate
                .PartyShortName = oPolicyVersionList.PartyShortName
                .Premium = oPolicyVersionList.Premium
                .PremiumSpecified = oPolicyVersionList.PremiumSpecified
                .InsuranceFileTypeCode = oPolicyVersionList.InsuranceFileTypeCode
                .PremiumSpecified = oPolicyVersionList.PremiumSpecified
                .InsuranceFileTypeKey = oPolicyVersionList.InsuranceFileTypeKey
                .CoverStartDate = oPolicyVersionList.CoverStartDate
                .ExpiryDate = oPolicyVersionList.ExpiryDate
                .QuoteExpiryDate = oPolicyVersionList.QuoteExpiryDate
                .EventDesc = oPolicyVersionList.EventDesc
                .TaxAmount = oPolicyVersionList.TaxAmount
                .TaxAmountSpecified = oPolicyVersionList.TaxAmountSpecified
                .GracePeriod = oPolicyVersionList.GracePeriod
                .ProductCode = oPolicyVersionList.ProductCode
                .PaymentMethod = oPolicyVersionList.PaymentMethod
                .InstalmentPlanStatus = oPolicyVersionList.InstalmentPlanStatus
                .PreviousVersionInstalmentPlanStatus = oPolicyVersionList.PreviousVersionInstalmentPlanStatus
                .AlternativeRef = oPolicyVersionList.AlternativeRef
                .PolicyStatus = oPolicyVersionList.PolicyStatus
                .LapseCancelDate = oPolicyVersionList.LapseCancelDate
                .InsuredPersons = oPolicyVersionList.InsuredPersons
                .Regarding = oPolicyVersionList.Regarding
                .Currency = oPolicyVersionList.Currency
                .Intermediary = oPolicyVersionList.Intermediary
                .TransactionDate = oPolicyVersionList.TransactionDate
            End With
        End If
        Return oServicePolicyVersionList

    End Function

    Public Shared Function ToServiceClaimPartyDetailsList(ByVal oClaimPartyDetailsList As SFI.SAMForInsuranceV2.BaseGetClaimPartyDetailsResponseTypePartyDetailsRow) As BaseGetClaimPartyDetailsResponseTypeRow

        Dim oServiceClaimPartyDetailsList As New BaseGetClaimPartyDetailsResponseTypeRow

        If oClaimPartyDetailsList IsNot Nothing Then
            With oServiceClaimPartyDetailsList
                .ResolvedName = oClaimPartyDetailsList.ResolvedName
                .ShortName = oClaimPartyDetailsList.ShortName
                .Address1 = oClaimPartyDetailsList.Address1
                .Address2 = oClaimPartyDetailsList.Address2
                .Address3 = oClaimPartyDetailsList.Address3
                .Address4 = oClaimPartyDetailsList.Address4
                .PostalCode = oClaimPartyDetailsList.PostalCode
                .TelHome = oClaimPartyDetailsList.TelHome
                .TelOff = oClaimPartyDetailsList.TelOff
                .Fax = oClaimPartyDetailsList.Fax
                .Mobile = oClaimPartyDetailsList.Mobile
                .EMail = oClaimPartyDetailsList.EMail
                .PartyKey = oClaimPartyDetailsList.PartyKey
                .CountryKey = oClaimPartyDetailsList.CountryKey
                .AddressKey = oClaimPartyDetailsList.AddressKey
            End With
        End If
        Return oServiceClaimPartyDetailsList

    End Function

    Public Shared Function ToServicePartyPoliciesList(ByVal oPartyPoliciesList As SFI.SAMForInsuranceV2.BaseGetPartyPoliciesResponseTypePartyPoliciesRow) As BaseGetPartyPoliciesResponseTypeRow

        Dim oServicePartyPoliciesList As New BaseGetPartyPoliciesResponseTypeRow

        If oPartyPoliciesList IsNot Nothing Then
            With oServicePartyPoliciesList
                .InsuranceFileKey = oPartyPoliciesList.InsuranceFileKey
                .InsuranceFileSourceKey = oPartyPoliciesList.InsuranceFileSourceKey
                .InsuranceRef = oPartyPoliciesList.InsuranceRef
                .LastTransDesc = oPartyPoliciesList.LastTransDesc
                .TypeCode = oPartyPoliciesList.TypeCode
                .RenewalDate = oPartyPoliciesList.RenewalDate
                .InsuranceHolderKey = oPartyPoliciesList.InsuranceHolderKey
                .InsuranceFolderKey = oPartyPoliciesList.InsuranceFolderKey
                .ProductKey = oPartyPoliciesList.ProductKey
                .InsuranceFolderKey = oPartyPoliciesList.InsuranceFolderKey
                .ProductCode = oPartyPoliciesList.ProductCode
                .LeadAgentKey = oPartyPoliciesList.LeadAgentKey
                .LeadAgentCode = oPartyPoliciesList.LeadAgentCode
                .DateCreated = oPartyPoliciesList.DateCreated
                .StatusCode = oPartyPoliciesList.StatusCode
                .ThisPremium = oPartyPoliciesList.ThisPremium
                .PolicyTypeKey = oPartyPoliciesList.PolicyTypeKey
                .PolicyTypeCode = oPartyPoliciesList.PolicyTypeCode
                .PolicyTypeDesc = oPartyPoliciesList.PolicyTypeDesc
                .InsuranceDesc = oPartyPoliciesList.InsuranceDesc
                .OpenPolicyClaims = oPartyPoliciesList.OpenPolicyClaims
                .ClosePolicyClaims = oPartyPoliciesList.ClosePolicyClaims
                .CoverStartDate = oPartyPoliciesList.CoverStartDate
                .ExpiryDate = oPartyPoliciesList.ExpiryDate
                .CoverStartDate = oPartyPoliciesList.CoverStartDate
                .MarkedForCollection = oPartyPoliciesList.MarkedForCollection
                .BaseInsuranceFolderKey = oPartyPoliciesList.BaseInsuranceFolderKey
                .QuoteStatusKey = oPartyPoliciesList.QuoteStatusKey
                .QuoteVersion = oPartyPoliciesList.QuoteVersion
                .RenewedVersion = oPartyPoliciesList.RenewedVersion
            End With
        End If
        Return oServicePartyPoliciesList

    End Function

    Public Shared Function ToServicePartySummaryList(ByVal oPartySummaryList As SFI.SAMForInsuranceV2.BaseGetPartySummaryResponseTypePoliciesRow) As BaseGetPartySummaryResponseTypeRow

        Dim oServicePartySummaryList As New BaseGetPartySummaryResponseTypeRow

        If oPartySummaryList IsNot Nothing Then
            With oServicePartySummaryList
                .InsuranceFileId = oPartySummaryList.InsuranceFileId
                .BranchKey = oPartySummaryList.BranchKey
                .BranchCode = oPartySummaryList.BranchCode
                .InsuranceFileKey = oPartySummaryList.InsuranceFileKey
                .PolicyRef = oPartySummaryList.PolicyRef
                .InsuranceFolderKey = oPartySummaryList.InsuranceFolderKey
                .PolicyTypeId = oPartySummaryList.PolicyTypeId
                .PolicyTypeIdSpecified = oPartySummaryList.PolicyTypeIdSpecified
                .LeadInsurerKey = oPartySummaryList.LeadInsurerKey
                .LeadInsurerKeySpecified = oPartySummaryList.LeadInsurerKeySpecified
                .DateIssued = oPartySummaryList.DateIssued

                .DateIssuedSpecified = oPartySummaryList.DateIssuedSpecified
                .CoverStartDate = oPartySummaryList.CoverStartDate

                .CoverStartDateSpecified = oPartySummaryList.CoverStartDateSpecified

                .ExpiryDate = oPartySummaryList.ExpiryDate

                .ExpiryDateSpecified = oPartySummaryList.ExpiryDateSpecified

                .RenewalDate = oPartySummaryList.RenewalDate

                .RenewalDateSpecified = oPartySummaryList.RenewalDateSpecified

                .InsuredKey = oPartySummaryList.InsuredKey

                .InsuredKeySpecified = oPartySummaryList.InsuredKeySpecified

                .ProductKey = oPartySummaryList.ProductKey

                .LeadAgentKey = oPartySummaryList.LeadAgentKey

                .LeadAgentKeySpecified = oPartySummaryList.LeadAgentKey

                .ThisPremium = oPartySummaryList.ThisPremium

                .ThisPremiumSpecified = oPartySummaryList.ThisPremiumSpecified

                .AnnualPremium = oPartySummaryList.AnnualPremium

                .AnnualPremiumSpecified = oPartySummaryList.AnnualPremiumSpecified

                .NetPremium = oPartySummaryList.NetPremium

                .NetPremiumSpecified = oPartySummaryList.NetPremiumSpecified

                .TaxAmount = oPartySummaryList.TaxAmount

                .TaxAmountSpecified = oPartySummaryList.TaxAmountSpecified

                .GeminiPolicyStatus = oPartySummaryList.GeminiPolicyStatus

                .GeminiPolicyStatusSpecified = oPartySummaryList.GeminiPolicyStatusSpecified


                .PartyShortName = oPartySummaryList.PartyShortName

                .ProductCode = oPartySummaryList.ProductCode

                .ProductDesc = oPartySummaryList.ProductDesc

                .InsuranceFileTypeCode = oPartySummaryList.InsuranceFileTypeCode

                .PolicyStatusCode = oPartySummaryList.PolicyStatusCode

                .InsurerShortName = oPartySummaryList.InsurerShortName

                .InsurerName = oPartySummaryList.InsurerName

                .AgentShortName = oPartySummaryList.AgentShortName

                .PolicyTypeCode = oPartySummaryList.PolicyTypeCode

                .PolicyTypeDesc = oPartySummaryList.PolicyTypeDesc

                .CurrencyCode = oPartySummaryList.CurrencyCode

                .AlternativeRef = oPartySummaryList.AlternativeRef

                .Regarding = oPartySummaryList.Regarding

                .PolicyStatus = oPartySummaryList.PolicyStatus

                .RiskTypeDescription = oPartySummaryList.RiskTypeDescription

                .EventDescription = oPartySummaryList.EventDescription

                .IsCurrent = oPartySummaryList.IsCurrent

                .MarkedForCollection = oPartySummaryList.MarkedForCollection

                .BaseInsuranceFolderKey = oPartySummaryList.BaseInsuranceFolderKey

                .QuoteStatusKey = oPartySummaryList.QuoteStatusKey

                .QuoteVersion = oPartySummaryList.QuoteVersion

                .QuoteExpiryDate = oPartySummaryList.QuoteExpiryDate

                .RenewedVersion = oPartySummaryList.RenewedVersion

                .RiskStatus = oPartySummaryList.RiskStatus
            End With
        End If
        Return oServicePartySummaryList

    End Function

    Public Shared Function ToServicePolicyBankGuaranteeList(ByVal oPolicyBankGuaranteeList As SFI.SAMForInsuranceV2.BaseGetPolicyBankGuaranteeResponseTypeBankGuaranteeRow) As BaseGetPolicyBankGuaranteeResponseTypeRow

        Dim oServicePolicyBankGuaranteeList As New BaseGetPolicyBankGuaranteeResponseTypeRow

        If oPolicyBankGuaranteeList IsNot Nothing Then
            With oServicePolicyBankGuaranteeList
                .BGKey = oPolicyBankGuaranteeList.BGKey
                .BankNameKey = oPolicyBankGuaranteeList.BankNameKey

                .BankNameKey = oPolicyBankGuaranteeList.BankNameKey

                .bankName = oPolicyBankGuaranteeList.BankName

                .BankGuaranteeRef = oPolicyBankGuaranteeList.BankGuaranteeRef

                .BGLimit = oPolicyBankGuaranteeList.BGLimit

                .AvailableBalance = oPolicyBankGuaranteeList.AvailableBalance

                .ExpiryDate = oPolicyBankGuaranteeList.ExpiryDate

                .ClientShortName = oPolicyBankGuaranteeList.ClientShortName

                .ClientName = oPolicyBankGuaranteeList.ClientName

                .DueDate = oPolicyBankGuaranteeList.DueDate
            End With
        End If
        Return oServicePolicyBankGuaranteeList

    End Function

    Public Shared Function ToServicePolicyDetailsForBouncedReceiptList(ByVal oPolicyDetailsForBouncedReceiptList As SFI.SAMForInsuranceV2.BaseGetPolicyDetailsForBouncedReceiptResponseTypePoliciesRow) As BaseGetPolicyDetailsForBouncedReceiptResponseTypeRow

        Dim oServicePolicyDetailsForBouncedReceiptList As New BaseGetPolicyDetailsForBouncedReceiptResponseTypeRow

        If oPolicyDetailsForBouncedReceiptList IsNot Nothing Then
            With oServicePolicyDetailsForBouncedReceiptList
                .DocumentRef = oPolicyDetailsForBouncedReceiptList.DocumentRef

                .DocumentRef = oPolicyDetailsForBouncedReceiptList.DocumentRef
                .InsuranceFileRef = oPolicyDetailsForBouncedReceiptList.InsuranceFileRef

                .AccountShortcode = oPolicyDetailsForBouncedReceiptList.AccountShortcode

                .PartyShortcode = oPolicyDetailsForBouncedReceiptList.PartyShortcode

                .PartyName = oPolicyDetailsForBouncedReceiptList.PartyName

                .PartyType = oPolicyDetailsForBouncedReceiptList.PartyType

                .InsuredShortcode = oPolicyDetailsForBouncedReceiptList.InsuredShortcode

                .InsuredName = oPolicyDetailsForBouncedReceiptList.InsuredName

                .InsuredKey = oPolicyDetailsForBouncedReceiptList.InsuredKey

                .InsuranceFileKey = oPolicyDetailsForBouncedReceiptList.InsuranceFileKey

                .GrossPremium = oPolicyDetailsForBouncedReceiptList.GrossPremium

                .InceptionDate = oPolicyDetailsForBouncedReceiptList.InceptionDate

                .CoverStartDate = oPolicyDetailsForBouncedReceiptList.CoverStartDate

                .CoverEndDate = oPolicyDetailsForBouncedReceiptList.CoverEndDate
            End With
        End If
        Return oServicePolicyDetailsForBouncedReceiptList

    End Function

    Public Shared Function ToServiceStandardPolicyWordingsList(ByVal oStandardPolicyWordingsList As SFI.SAMForInsuranceV2.BaseGetStandardPolicyWordingsResponseTypeDocumentTemplatesRow) As BaseGetStandardPolicyWordingsResponseTypeRow

        Dim oServiceStandardPolicyWordingsList As New BaseGetStandardPolicyWordingsResponseTypeRow

        If oStandardPolicyWordingsList IsNot Nothing Then
            With oServiceStandardPolicyWordingsList
                .DocumentTemplateId = oStandardPolicyWordingsList.DocumentTemplateId

                .Code = oStandardPolicyWordingsList.Code

                .Description = oStandardPolicyWordingsList.Description
            End With
        End If
        Return oServiceStandardPolicyWordingsList

    End Function

End Class
