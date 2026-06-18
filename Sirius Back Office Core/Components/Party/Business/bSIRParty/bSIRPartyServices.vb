Option Strict Off
Option Explicit On
'Developer Guide No. 129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Services_NET.Services")> _
Public NotInheritable Class Services
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Services
    '
    ' Date: 19/10/1998
    '
    ' Description: Provides public SIRParty entity services.
    '
    ' Edit History:
    ' TF250700 - Added Email address property
    ' SP231198 - Need to get a contact for when Orion uses services.
    ' SP071298 - Need to get the address details.
    ' SP050199 - Add properties for other party types.
    ' TF170700 - Add properties for Net Data
    ' RAW 21/02/2003 : ISS2379 : added Agent Group party type
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 27/11/2003
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Object
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Services"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag
    Private m_bCloseDatabase As Boolean

    ' Instance of Business class
    Private m_oSIRParty As Business

    ' Instance of Solution Specific class
    Private m_oSolutionParty As Object
    'EK 24/11/99
    Private m_oParty As Object
    'EK 14/11/99
    'Instance of Component Services
    'Instance of Address Business Class
    Private m_oAddress As bSIRAddress.Business
    'Instance of Contact Business Class
    Private m_oContact As bSIRContact.Business
    ''
    ' Error Code
    Private m_lReturn As Integer

    ' Primary Keys to work with
    Private m_lPartyCnt As Object

    ' DataBase Attributes for Party
    'Developer Guide No. 101
    Private m_vPartyTypeID As Object
    Private m_vIsAlsoAgent As Object
    'Developer Guide No. 101
    Private m_vPartyStructureID As Object
    Private m_vSourceID As Object
    'Developer Guide No. 101
    Private m_vSubBranchId As Object
    Private m_vPartyID As Object
    'Developer Guide No 101
    Private m_vShortName As Object
    Private m_vName As Object
    Private m_vResolvedName As Object
    'Developer Guide No. 101
    Private m_vCurrencyID As Object

    'MSS200901 - Added for merge
    Private m_vCurrencyCode As String = ""
    'MSS200901 - Merge end
    Private m_vLanguageID As Object
    Private m_vCollectTypeID As Object
    Private m_vAccumTreatmentTypeID As Object
    Private m_vStatsTreatmentTypeID As Object
    Private m_vPartyCategoryId As Object
    Private m_vAgentCnt As Object
    Private m_vConsultantCnt As Object
    Private m_vCreatedByID As Object
    Private m_vDateCreated As Object
    Private m_vLastModified As Object
    Private m_vModifiedByID As Object
    'Developer Guide No. 101
    Private m_vPaymentMethodCode As Object
    'Developer Guide No. 101
    Private m_vPaymentTermCode As Object
    Private m_vCreditCardCode As Object
    'Developer Guide No. 101
    Private m_vFileCode As Object
    Private m_vABCCount As Object
    Private m_vStatements As Object
    Private m_vReminderTypeId As Object
    Private m_vRenewals As Object
    Private m_vStatus As Object
    Private m_vLastACtionType As Object
    Private m_vIsTravelAgent As Object
    Private m_vIsProspect As Object
    Private m_vIsDeleted As Object
    Private m_vABICodeOn406 As Object
    Private m_vABICodeOn81 As Object
    Private m_vABICodeList As Object
    Private m_vAreaId As Object
    Private m_vServiceLevelId As Object
    Private m_vInvariantKey As Object
    Private m_vRecordStatus As Object
    Private m_vCCJS As Object
    Private m_vUserDefinedDataId As Object
    Private m_vSeasonalGiftID As Object
    'DC 28/06/00
    Private m_vCorrespondenceTypeId As Object
    'FSA Phase III
    Private m_vSwiftPartyId As Object
    Private m_vLoyaltyNumber As Object
    Private m_vAlternativeIdentifier As Object
    Private m_vMarketingSegmentInd As Object
    Private m_vTradingName As Object
    Private m_vTobLetter As Object
    Private m_vTpsInd As Object
    Private m_vEmpsInd As Object
    'FSA Phase IIIEnd

    ' Additional Properties from Lookups
    Private m_vPartyType As String = ""
    'MSS200901 - Added for merge
    Private m_vRenewalStopCodeId As Object
    'MSS200901 - Merge end
    ' Database Attributes for PartyPC
    'Developer Guide No. 101
    Private m_vPartyTitleCode As Object
    'Developer Guide No. 101
    Private m_vForeName As Object
    'Developer Guide No. 101
    Private m_vInitials As Object
    Private m_vEmploymentStatusCode As Object
    Private m_vEmployerCnt As Object
    Private m_vEmployerBusiness As Object
    Private m_vSecondaryEmploymentStatusC As Object
    Private m_vSecondaryEmployerBusiness As Object
    Private m_vMaritalStatusCode As Object
    Private m_vNumberOfChildren As Object
    Private m_vNationalityId As Object
    'MSS200901 - Added for merge
    Private m_vNationalityCode As String = ""
    'MSS200901 - Merge end
    Private m_vCountryOfOriginCode As Object
    Private m_vSeasonal_gift_Id As Object
    Private m_vMailshot As Object
    Private m_vIsPetOwner As Object
    Private m_vAccomodationTypeCode As Object

    ' Additional Properties from Lookups for PartyPC
    Private m_vPartyTitle As Object
    Private m_vPartyOccupation As Object

    ' Database Attributes for Party Lifestyle
    Private m_vPartyLifestyleID As Object
    Private m_vCategory As Object
    Private m_vOccupationCode As Object
    'Developer Guide No 101
    Private m_vDateOfBirth As Object
    Private m_vGenderCode As Object
    Private m_vSecondaryOccupationCode As Object
    Private m_vIsSmoker As Object
    Private m_bPartyLifestyleChanged As Boolean

    ' Database Attributes for PartyGC
    Private m_vPartyGroupTypeID As Object
    Private m_vIsRegisteredCharity As Object

    ' Additional Properties from Lookups for PartyGC
    Private m_vPartyGroupType As String = ""

    ' Database Attributes for PartyCC
    Private m_vCompanyReg As Object
    Private m_vTradingSinceDate As Object
    Private m_vPartyBusinessID As Object
    Private m_vPArtyTradeId As Object

    ' Database Attributes for PartyAH & PartyCO
    Private m_vDepartmentId As Object


    ' Additional Properties from Lookups for PartyCC
    Private m_vPartyTrade As Object

    ' Database Attributes for PartyAG
    Private m_vPartyAgentOriginID As Object
    'DC 27/07/00
    Private m_vPartyAgentTypeID As Object
    Private m_vAgencyAgreementDate As Object
    Private m_vAgencyNextReviewDate As Object
    Private m_vIsBranch As Object
    Private m_vIsHeadOffice As Object
    Private m_vDefaultCommissionPercent As Object
    Private m_vPaymentMethod As Object

    Private m_vContactPerson As Object
    Private m_vFirstName As Object

    ' Additional Properties from Lookups for PartyAG
    'Developer Guide No.17
    Private m_vPartyAgentOrigin As Object


    ' RAW 21/02/2003 : ISS2379 : added
    ' Database Attributes for PartyAGG
    Private m_vIsGroupActive As Object

    ' Additional Properties from Lookups for PartyAGG
    ' none
    ' RAW 21/02/2003 : ISS2379 : end

    'Attributes for Insurer
    Private m_vAgencyNumber As Object
    Private m_vBinderIndicator As Object
    Private m_vReportIndicator As Object
    Private m_vIsReinsurer As Object
    Private m_vReInsuranceType As Object
    Private m_vIsReInsuranceDebitCreditNo As Object
    Private m_vDefaultCommRate As Object

    ' Database Attributes for PartyNetData
    Private m_vPassword As Object
    Private m_vMothersMaidenName As Object
    Private m_vTPIntroducerCode As Object
    Private m_vTPUserCode As Object
    Private m_vMemorableDate As Object
    Private m_vAQuestion As Object
    Private m_vTheAnswer As Object
    'RJG 09/06/2000 - Add UserID and Curr Ins Renewal date for PartyNetData
    Private m_vUserID As Object
    Private m_vCurrInsRenewalDate As Object

    ' TF190700 - Dirty flag to indicate Net Data changed
    Private m_bNetDataChanged As Boolean

    ' TF301000 - Database Attributes for PartyFP (Finance Provider)
    Private m_vFinanceProviderNumber As Object
    Private m_vMailboxNumber As Object
    'DC100402
    Private m_vRefNumber As Object
    Private m_vExternalId As Object
    Private m_vRegNumber As Object
    Private m_vPartyStatus As Object
    Private m_vLicenseTypeId As Object
    Private m_vLicenseNumber As Object
    ' Address Properties
    Private m_vAddressCnt As Integer
    Private m_vAddress1 As String = ""
    Private m_vAddress2 As String = ""
    Private m_vAddress3 As String = ""
    Private m_vAddress4 As String = ""
    Private m_vPostalCode As String = ""
    Private m_sAddress5 As String = ""
    Private m_sAddress6 As String = ""
    Private m_sAddress7 As String = ""
    Private m_sAddress8 As String = ""
    Private m_sAddress9 As String = ""
    Private m_sAddress10 As String = ""
    'eck130601
    Private m_vCountryId As String = ""
    ' TF250700
    Private m_vEmailAddress As String = ""

    ' Contact Properties
    Private m_vContactArray As Object
    Private m_vContactCnt As Object
    Private m_vAreaCode As String = ""
    Private m_vNumber As String = ""
    Private m_vExtension As String = ""
    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business

    'Extras
    Private m_sSolutionCode As String = ""

    'RAM20021016 : NRMA Changes - Sirius Process No 126 - Start
    Private m_sPreferredContactType As String = ""
    Private m_sPreferredContactDetail As String = ""
    Private m_vAllContactsArray As Object
    Private m_vConvictionsArray As Object
    'RAM20021016 : NRMA Changes - Sirius Process No 126 - End

    'Do not confuse this party type (ie Pers. Client, Corporate Client, Agent etc) with
    'the other party type (ie Nursing Home, School etc)
    Private m_sSolutionPartyType As String = ""
    ' PRIVATE Data Members (End)

    Private m_vEmployerBusinessCode As Object
    Public ReadOnly Property PMProductFamily() As Object
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property


    Public Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPartyCnt = Value

            'SP041298 - This is done by calling app.
            '    m_lReturn& = GetDetails()

        End Set
    End Property


    Public Property ContactPerson() As String
        Get
            Return m_vContactPerson
        End Get
        Set(ByVal Value As String)

            m_vContactPerson = CStr(Value)
        End Set
    End Property


    Public Property FirstName() As String
        Get
            Return m_vFirstName
        End Get
        Set(ByVal Value As String)

            m_vFirstName = CStr(Value)
        End Set
    End Property
    'Developer Guide No. 101
    Public Property PartyTypeID() As Object
        Get

            Return m_vPartyTypeID

        End Get
        Set(ByVal Value As Object)


            m_vPartyTypeID = Value

        End Set
    End Property
    Public Property RecordStatus() As Object
        Get
            'JAS(CMG) 05/09/02
            Return m_vRecordStatus

        End Get
        Set(ByVal Value As Object)
            'JAS(CMG) 05/09/02


            m_vRecordStatus = Value

        End Set
    End Property
    'Developer Guide No. 101
    Public Property PartyStructureID() As Object
        Get

            Return m_vPartyStructureID

        End Get
        'Set(ByVal Value As Integer)
        Set(ByVal Value As Object)


            m_vPartyStructureID = Value

        End Set
    End Property
    Public Property SourceID() As Object
        Get

            Return m_vSourceID

        End Get
        Set(ByVal Value As Object)



            m_vSourceID = Value

        End Set
    End Property
    'Public Property SubBranchId() As Integer
    Public Property SubBranchId() As Object
        Get

            Return m_vSubBranchId

        End Get
        Set(ByVal Value As Object)


            m_vSubBranchId = Value

        End Set
    End Property
    Public Property PartyID() As Object
        Get

            Return m_vPartyID

        End Get
        Set(ByVal Value As Object)



            m_vPartyID = Value

        End Set
    End Property
    'Developer Guide No 101
    Public Property Shortname() As Object
        Get

            Return m_vShortName

        End Get
        Set(ByVal Value As Object)


            m_vShortName = Value

        End Set
    End Property

    Public Property Name() As Object
        Get

            Return m_vName

        End Get
        Set(ByVal Value As Object)


            m_vName = Value

        End Set
    End Property
    Public Property ResolvedName() As Object
        Get

            Return m_vResolvedName

        End Get
        Set(ByVal Value As Object)


            m_vResolvedName = Value

        End Set
    End Property
    'Ends
    Public Property CurrencyID() As Object
        Get

            Return m_vCurrencyID

        End Get
        Set(ByVal Value As Object)


            m_vCurrencyID = Value

        End Set
    End Property

    Public Property Address5() As String
        Get
            Return m_sAddress5
        End Get
        Set(ByVal Value As String)
            m_sAddress5 = CStr(Value)
        End Set
    End Property
    Public Property Address6() As String
        Get
            Return m_sAddress6
        End Get
        Set(ByVal Value As String)
            m_sAddress6 = CStr(Value)
        End Set
    End Property
    Public Property Address7() As String
        Get
            Return m_sAddress7
        End Get
        Set(ByVal Value As String)
            m_sAddress7 = CStr(Value)
        End Set
    End Property
    Public Property Address8() As String
        Get
            Return m_sAddress8
        End Get
        Set(ByVal Value As String)
            m_sAddress8 = CStr(Value)
        End Set
    End Property
    Public Property Address9() As String
        Get
            Return m_sAddress9
        End Get
        Set(ByVal Value As String)
            m_sAddress9 = CStr(Value)
        End Set
    End Property
    Public Property Address10() As String
        Get
            Return m_sAddress10
        End Get
        Set(ByVal Value As String)
            m_sAddress10 = CStr(Value)
        End Set
    End Property

    'MSS200901 -Added for Merge
    Public Property CurrencyCode() As String
        Get

            Return m_vCurrencyCode

        End Get
        Set(ByVal Value As String)

            Dim lID As Integer

            'If description this has changed ned to get new look up ID for it

            If CStr(Value).Trim() <> m_vCurrencyCode.Trim() Then


                'Developer Guide No. 98
                m_lReturn = GetLookUpID(sTable:=gSIRLibrary.SIRLookupCurrency, sField:="code", sDescription:=Value, sIDColumnName:="currency_id", lIDValue:=lID)

                'update the member ID
                m_vCurrencyID = lID


                m_vCurrencyCode = CStr(Value)

            End If

            'update the area ID
            '    m_vCurrencyID = lID

        End Set
    End Property

    Public WriteOnly Property RenewalStopCodeId() As Object
        Set(ByVal Value As Object)

            m_vRenewalStopCodeId = Value

        End Set
    End Property

    'MSS200901 - Merge end
    Public Property LanguageID() As Object
        Get

            Return m_vLanguageID

        End Get
        Set(ByVal Value As Object)



            m_vLanguageID = Value

        End Set
    End Property
    Public Property CollectTypeID() As Object
        Get

            Return m_vCollectTypeID

        End Get
        Set(ByVal Value As Object)



            m_vCollectTypeID = Value

        End Set
    End Property
    Public Property AccumTreatmentTypeID() As Object
        Get

            Return m_vAccumTreatmentTypeID

        End Get
        Set(ByVal Value As Object)



            m_vAccumTreatmentTypeID = Value

        End Set
    End Property
    Public Property StatsTreatmentTypeID() As Object
        Get

            Return m_vStatsTreatmentTypeID

        End Get
        Set(ByVal Value As Object)



            m_vStatsTreatmentTypeID = Value

        End Set
    End Property

    Public Property AgentCnt() As Integer
        Get

            Return m_vAgentCnt

        End Get
        Set(ByVal Value As Integer)


            m_vAgentCnt = CInt(Value)

        End Set
    End Property
    Public Property CreatedByID() As Object
        Get

            Return m_vCreatedByID

        End Get
        Set(ByVal Value As Object)



            m_vCreatedByID = Value

        End Set
    End Property
    Public Property DateCreated() As Object
        Get

            Return m_vDateCreated

        End Get
        Set(ByVal Value As Object)


            m_vDateCreated = Value

        End Set
    End Property
    Public Property LastModified() As Object
        Get

            Return m_vLastModified

        End Get
        Set(ByVal Value As Object)


            m_vLastModified = Value

        End Set
    End Property
    Public Property ModifiedByID() As Object
        Get

            Return m_vModifiedByID

        End Get
        Set(ByVal Value As Object)



            m_vModifiedByID = Value

        End Set
    End Property
    'Developer Guide No. 101
    Public Property PartyTitleCode() As Object
        Get

            Return m_vPartyTitleCode

        End Get
        Set(ByVal Value As Object)


            m_vPartyTitleCode = Value

        End Set
    End Property
    Public WriteOnly Property EmploymentStatusCode() As Object
        Set(ByVal Value As Object)

            m_vEmploymentStatusCode = Value

        End Set
    End Property
    Public WriteOnly Property EmployerBusinessCode() As Object
        Set(ByVal Value As Object)

            m_vEmployerBusinessCode = Value

        End Set
    End Property

    'ek 24/11/99
    Public Property DepartmentId() As Object
        Get

            Return m_vDepartmentId

        End Get
        Set(ByVal Value As Object)



            m_vDepartmentId = Value

        End Set
    End Property
    ' TF200700
    Public Property PartyLifestyleID() As Object
        Get

            Return m_vPartyLifestyleID

        End Get
        Set(ByVal Value As Object)


            m_vPartyLifestyleID = Value
            m_bPartyLifestyleChanged = True

        End Set
    End Property

    Public Property Category() As Object
        Get

            Return m_vCategory

        End Get
        Set(ByVal Value As Object)



            m_vCategory = Value
            m_bPartyLifestyleChanged = True

        End Set
    End Property

    'Developer Guide No 101
    Public Property DateOfBirth() As Object
        Get

            Return m_vDateOfBirth

        End Get
        Set(ByVal Value As Object)
            m_vDateOfBirth = Value
            m_bPartyLifestyleChanged = True

        End Set
    End Property

    Public Property GenderCode() As Object
        Get

            Return m_vGenderCode

        End Get
        Set(ByVal Value As Object)


            m_vGenderCode = Value
            m_bPartyLifestyleChanged = True

        End Set
    End Property

    Public Property OccupationCode() As Object
        Get

            Return m_vOccupationCode

        End Get
        Set(ByVal Value As Object)


            m_vOccupationCode = Value
            m_bPartyLifestyleChanged = True

        End Set
    End Property

    Public Property SecondaryOccupationCode() As Object
        Get

            Return m_vSecondaryOccupationCode

        End Get
        Set(ByVal Value As Object)



            m_vSecondaryOccupationCode = Value
            m_bPartyLifestyleChanged = True

        End Set
    End Property

    Public Property IsSmoker() As Object
        Get

            Return m_vIsSmoker

        End Get
        Set(ByVal Value As Object)



            m_vIsSmoker = Value
            m_bPartyLifestyleChanged = True

        End Set
    End Property

    'sj 11/11/99 - start
    'sj 11/11/99 - end

    Public Property MaritalStatusCode() As Object
        Get

            Return m_vMaritalStatusCode

        End Get
        Set(ByVal Value As Object)


            m_vMaritalStatusCode = Value

        End Set
    End Property
    Public Property NumberOfChildren() As Object
        Get

            Return m_vNumberOfChildren

        End Get
        Set(ByVal Value As Object)


            m_vNumberOfChildren = Value

        End Set
    End Property
    'Public Property Forename() As Integer
    Public Property Forename() As Object
        Get

            Return m_vForeName

        End Get
        Set(ByVal Value As Object)


            m_vForeName = Value

        End Set
    End Property
    'Public Property Initials() As Integer
    Public Property Initials() As Object
        Get

            Return m_vInitials

        End Get
        Set(ByVal Value As Object)


            m_vInitials = Value

        End Set
    End Property


    Public Property PartyGroupTypeID() As Object
        Get

            Return m_vPartyGroupTypeID

        End Get
        Set(ByVal Value As Object)


            m_vPartyGroupTypeID = Value

        End Set
    End Property
    Public Property IsRegisteredCharity() As Object
        Get

            Return m_vIsRegisteredCharity

        End Get
        Set(ByVal Value As Object)



            m_vIsRegisteredCharity = Value

        End Set
    End Property
    Public Property PartyBusinessId() As Object
        Get

            Return m_vPartyBusinessID

        End Get
        Set(ByVal Value As Object)



            m_vPartyBusinessID = Value

        End Set
    End Property
    Public Property CompanyReg() As Object
        Get

            Return m_vCompanyReg

        End Get
        Set(ByVal Value As Object)



            m_vCompanyReg = Value

        End Set
    End Property
    Public Property TradingSinceDate() As Object
        Get

            Return m_vTradingSinceDate

        End Get
        Set(ByVal Value As Object)



            m_vTradingSinceDate = Value

        End Set
    End Property
    Public Property PartyTradeID() As Object
        Get

            Return m_vPArtyTradeId

        End Get
        Set(ByVal Value As Object)



            m_vPArtyTradeId = Value

        End Set
    End Property

    Public Property PartyAgentOriginID() As Object
        Get

            Return m_vPartyAgentOriginID

        End Get
        Set(ByVal Value As Object)


            m_vPartyAgentOriginID = Value

        End Set
    End Property
    'DC 27/07/00
    'DC 27/07/00
    Public Property PartyAgentTypeID() As Object
        Get

            Return m_vPartyAgentTypeID

        End Get
        Set(ByVal Value As Object)



            m_vPartyAgentTypeID = Value

        End Set
    End Property
    Public Property AgencyAgreementDate() As Object
        Get

            Return m_vAgencyAgreementDate

        End Get
        Set(ByVal Value As Object)



            m_vAgencyAgreementDate = Value

        End Set
    End Property
    Public Property AgencyNextReviewDate() As Object
        Get

            Return m_vAgencyNextReviewDate

        End Get
        Set(ByVal Value As Object)



            m_vAgencyNextReviewDate = Value

        End Set
    End Property
    Public WriteOnly Property IsTravelAgent() As Object
        Set(ByVal Value As Object)



            m_vIsTravelAgent = Value
        End Set
    End Property
    Public WriteOnly Property IsProspect() As Object
        Set(ByVal Value As Object)

            m_vIsProspect = Value
        End Set
    End Property
    Public Property IsBranch() As Object
        Get

            Return m_vIsBranch

        End Get
        Set(ByVal Value As Object)



            m_vIsBranch = Value

        End Set
    End Property
    Public Property IsHeadOffice() As Object
        Get

            Return m_vIsHeadOffice

        End Get
        Set(ByVal Value As Object)



            m_vIsHeadOffice = Value

        End Set
    End Property

    Public Property PaymentMethod() As Object
        Get
            Return m_vPaymentMethod
        End Get
        Set(ByVal Value As Object)


            m_vPaymentMethod = Value
        End Set
    End Property


    ' RAW 21/02/2003 : ISS2379 : added
    Public Property IsGroupActive() As Object
        Get

            Return m_vIsGroupActive

        End Get
        Set(ByVal Value As Object)



            m_vIsGroupActive = Value

        End Set
    End Property
    ' RAW 21/02/2003 : ISS2379 : end
    'contact
    Public Property ContactCnt() As Object
        Get

            Return m_vContactCnt

        End Get
        Set(ByVal Value As Object)



            m_vContactCnt = Value

        End Set
    End Property
    'DC221200
    'consultant
    Public Property ConsultantCnt() As Integer
        Get

            Return m_vConsultantCnt

        End Get
        Set(ByVal Value As Integer)


            m_vConsultantCnt = CInt(Value)

        End Set
    End Property
    'Address
    Public Property AddressCnt() As Integer
        Get

            Return m_vAddressCnt

        End Get
        Set(ByVal Value As Integer)


            m_vAddressCnt = CInt(Value)

        End Set
    End Property
    Public Property Address1() As String
        Get

            Return m_vAddress1

        End Get
        Set(ByVal Value As String)


            m_vAddress1 = CStr(Value)

        End Set
    End Property
    Public Property Address2() As String
        Get

            Return m_vAddress2

        End Get
        Set(ByVal Value As String)


            m_vAddress2 = CStr(Value)

        End Set
    End Property
    Public Property Address3() As String
        Get

            Return m_vAddress3

        End Get
        Set(ByVal Value As String)


            m_vAddress3 = CStr(Value)

        End Set
    End Property
    Public Property Address4() As String
        Get

            Return m_vAddress4

        End Get
        Set(ByVal Value As String)


            m_vAddress4 = CStr(Value)

        End Set
    End Property
    Public Property PostalCode() As String
        Get

            Return m_vPostalCode

        End Get
        Set(ByVal Value As String)


            m_vPostalCode = CStr(Value)

        End Set
    End Property
    Public Property CountryId() As String
        Get

            Return m_vCountryId

        End Get
        Set(ByVal Value As String)


            m_vCountryId = CStr(Value)

        End Set
    End Property

    Public Property ContactArray() As Object
        Get

            Return m_vContactArray

        End Get
        Set(ByVal Value As Object)



            m_vContactArray = Value

        End Set
    End Property

    ' TF250700
    Public Property EMailAddress() As String
        Get

            Return m_vEmailAddress

        End Get
        Set(ByVal Value As String)


            m_vEmailAddress = CStr(Value)

        End Set
    End Property

    Public WriteOnly Property PaymentTermCode() As Object
        Set(ByVal Value As Object)

            m_vPaymentTermCode = Value

        End Set
    End Property
    'Developer Guide No. 101
    Public WriteOnly Property PaymentMethodCode() As Object
        Set(ByVal Value As Object)

            m_vPaymentMethodCode = Value

        End Set
    End Property
    'Developer Guide No. 101
    Public WriteOnly Property FileCode() As Object
        Set(ByVal Value As Object)

            m_vFileCode = Value

        End Set
    End Property
    Public WriteOnly Property ABCCount() As Object
        Set(ByVal Value As Object)

            m_vABCCount = Value

        End Set
    End Property
    Public WriteOnly Property Statements() As Object
        Set(ByVal Value As Object)

            m_vStatements = Value

        End Set
    End Property
    Public WriteOnly Property ReminderTypeId() As Object
        Set(ByVal Value As Object)

            m_vReminderTypeId = Value

        End Set
    End Property
    Public WriteOnly Property Renewals() As Object
        Set(ByVal Value As Object)

            m_vRenewals = Value

        End Set
    End Property
    Public WriteOnly Property Status() As Object
        Set(ByVal Value As Object)

            m_vStatus = Value

        End Set
    End Property
    Public WriteOnly Property InvariantKey() As Object
        Set(ByVal Value As Object)
            'DC 08/10/99 added in

            m_vInvariantKey = Value

        End Set
    End Property
    Public WriteOnly Property LastActionType() As Object
        Set(ByVal Value As Object)

            m_vLastACtionType = Value

        End Set
    End Property
    Public ReadOnly Property SolutionPartyType() As String
        Get

            Return m_sSolutionPartyType

        End Get
    End Property

    'MSS200901 - Added for merge

    Public Property AreaId() As Object
        Get

            Return m_vAreaId

        End Get
        Set(ByVal Value As Object)


            m_vAreaId = Value
        End Set
    End Property

    Public Property NationalityID() As Object
        Get

            Return m_vNationalityId

        End Get
        Set(ByVal Value As Object)


            m_vNationalityId = Value

        End Set
    End Property

    Public Property NationalityCode() As String
        Get

            Return m_vNationalityCode

        End Get
        Set(ByVal Value As String)

            Dim lID As Integer

            'If description this has changed ned to get new look up ID for it

            If CStr(Value).Trim() <> m_vNationalityCode.Trim() Then


                'Developer Guide No. 98
                m_lReturn = GetLookUpID(sTable:=gSIRLibrary.SIRLookupNationality, sField:="code", sDescription:=Value, sIDColumnName:="Nationality_id", lIDValue:=lID)

                'update the member ID
                m_vNationalityId = lID


                m_vNationalityCode = CStr(Value)

            End If

            'update the area ID
            '    m_vNationalityId = lID

        End Set
    End Property
    'MSS200901 - Merge End

    Public Property AreaCode() As String
        Get

            Return m_vAreaCode

        End Get
        Set(ByVal Value As String)

            Dim lID As Integer

            'If description this has changed ned to get new look up ID for it

            If CStr(Value).Trim() <> m_vAreaCode.Trim() Then


                'Developer Guide No. 98
                m_lReturn = GetLookUpID(sTable:=gSIRLibrary.SIRLookupArea, sField:="code", sDescription:=Value, sIDColumnName:="area_id", lIDValue:=lID)

                'update the member ID
                m_vAreaId = lID


                m_vAreaCode = CStr(Value)

            End If

            'update the area ID
            m_vAreaId = lID

        End Set
    End Property
    Public WriteOnly Property ABICodeList() As Object
        Set(ByVal Value As Object)



            m_vABICodeList = Value

        End Set
    End Property
    Public WriteOnly Property ABICodeon406() As Object
        Set(ByVal Value As Object)



            m_vABICodeOn406 = Value

        End Set
    End Property
    Public WriteOnly Property ABICodeon81() As Object
        Set(ByVal Value As Object)



            m_vABICodeOn81 = Value

        End Set
    End Property

    Public ReadOnly Property Number() As String
        Get

            Return m_vNumber

        End Get
    End Property
    Public Property Extension() As String
        Get

            Return m_vExtension

        End Get
        Set(ByVal Value As String)


            m_vExtension = CStr(Value)

        End Set
    End Property
    'eck010600
    Public Property SeasonalGiftID() As Object
        Get

            Return m_vSeasonalGiftID

        End Get
        Set(ByVal Value As Object)


            m_vSeasonalGiftID = Value

        End Set
    End Property
    'DC 28/06/00
    'DC 28/06/00
    Public Property CorrespondenceTypeId() As Object
        Get

            Return m_vCorrespondenceTypeId

        End Get
        Set(ByVal Value As Object)


            m_vCorrespondenceTypeId = Value

        End Set
    End Property
    'FSA Phase III
    Public Property SwiftPartyId() As Object
        Get

            Return m_vSwiftPartyId

        End Get
        Set(ByVal Value As Object)


            m_vSwiftPartyId = Value

        End Set
    End Property
    Public Property LoyaltyNumber() As Object
        Get

            Return m_vLoyaltyNumber

        End Get
        Set(ByVal Value As Object)


            m_vLoyaltyNumber = Value

        End Set
    End Property
    Public Property AlternativeIdentifier() As Object
        Get

            Return m_vAlternativeIdentifier

        End Get
        Set(ByVal Value As Object)


            m_vAlternativeIdentifier = Value

        End Set
    End Property
    Public Property MarketingSegmentInd() As Object
        Get

            Return m_vMarketingSegmentInd

        End Get
        Set(ByVal Value As Object)


            m_vMarketingSegmentInd = Value

        End Set
    End Property
    Public Property TradingName() As Object
        Get

            Return m_vTradingName

        End Get
        Set(ByVal Value As Object)


            m_vTradingName = Value
        End Set
    End Property
    Public Property TobLetter() As Object
        Get

            Return m_vTobLetter

        End Get
        Set(ByVal Value As Object)


            m_vTobLetter = Value

        End Set
    End Property
    Public Property TpsInd() As Object
        Get

            Return m_vTpsInd

        End Get
        Set(ByVal Value As Object)



            m_vTpsInd = Value

        End Set
    End Property
    Public Property Mailshot() As Object
        Get

            Return m_vMailshot

        End Get
        Set(ByVal Value As Object)



            m_vMailshot = Value

        End Set
    End Property
    Public Property EmpsInd() As Object
        Get

            Return m_vEmpsInd

        End Get
        Set(ByVal Value As Object)



            m_vEmpsInd = Value

        End Set
    End Property
    'FSA Phase III End
    'Look ups
    Public Property PartyType() As String
        Get

            Return m_vPartyType

        End Get
        Set(ByVal Value As String)

            Dim lID As Integer

            'If description this has changed ned to get new look up ID for it

            If CStr(Value).Trim() <> m_vPartyType.Trim() Then


                'Developer Guide No. 98
                m_lReturn = GetLookUpID(sTable:=gSIRLibrary.SIRLookupPartyType, sField:="code", sDescription:=Value, sIDColumnName:="party_type_id", lIDValue:=lID)

                'update the member ID
                m_vPartyTypeID = lID


                m_vPartyType = CStr(Value)

            End If

            'update the member ID
            '    m_vPartyTypeID = lID


        End Set
    End Property
    Public Property PartyTitle() As Object
        Get

            Return m_vPartyTitle

        End Get
        Set(ByVal Value As Object)



            'If description this has changed ned to get new look up ID for it
            '    If (Trim$(sPartyTitle) <> Trim$(m_vPartyTitle)) Then
            '
            '        m_lReturn& = GetLookUpID(sTable:=SIRLookupPartyTitle, _
            ''                                sField:="description", _
            ''                                sDescription:=CStr(sPartyTitle), _
            ''                                sIDColumnName:="party_title_id", _
            ''                                lIDValue:=lID)
            '
            '        'update the member ID
            ' '       m_vPartyTitleCode = lID



            m_vPartyTitle = Value


            '    End If

            'update the member ID
            '    m_vPartyTitleCode = lID


        End Set
    End Property
    'Public Property Let PartyOccupation(sPartyOccupation As Variant)
    '
    'Dim lID As Long
    '
    '    'If description this has changed ned to get new look up ID for it
    '    If (Trim$(sPartyOccupation) <> Trim$(m_vPartyOccupation)) Then
    '
    '        m_lReturn& = GetLookUpID(sTable:=SIRLookupPartyOccupation, _
    ''                                sField:="description", _
    ''                                sDescription:=CStr(sPartyOccupation), _
    ''                                sIDColumnName:="party_occupation_id", _
    ''                                lIDValue:=lID)
    '
    '        'update the member ID
    '        m_vPartyOccupationID = lID
    '
    '        m_vPartyOccupation = sPartyOccupation
    '
    '    End If
    '
    '    'update the member ID
    ''    m_vPartyOccupationID = lID
    '
    '
    'End Property
    'Public Property Get PartyOccupation() As Variant
    '
    '    PartyOccupation = m_vPartyOccupation
    '
    'End Property
    Public Property PartyGroupType() As String
        Get

            Return m_vPartyGroupType

        End Get
        Set(ByVal Value As String)

            Dim lID As Integer

            'If description this has changed need to get new look up ID for it

            If CStr(Value).Trim() <> m_vPartyGroupType.Trim() Then


                'Developer Guide No. 98
                m_lReturn = GetLookUpID(sTable:=gSIRLibrary.SIRLookupPartyGroupType, sField:="description", sDescription:=Value, sIDColumnName:="party_group_type_id", lIDValue:=lID)

                'update the member ID
                m_vPartyGroupTypeID = lID


                m_vPartyGroupType = CStr(Value)

            End If

            'update the member ID
            '    m_vPartyGroupTypeID = lID


        End Set
    End Property
    Public Property PartyTrade() As Object
        Get

            Return m_vPartyTrade

        End Get
        Set(ByVal Value As Object)



            'If description this has changed need to get new look up ID for it
            '    If (Trim$(sPartyTrade) <> Trim$(m_vPartyTrade)) Then

            '        m_lReturn& = GetLookUpID(sTable:=SIRLookupPartyBusiness, _
            'sField:="description", _
            'sDescription:=CStr(sPartyTrade), _
            'sIDColumnName:="party_business_id", _
            'lIDValue:=lID)

            'update the member ID
            '        m_vPartyBusinessID = lID



            m_vPartyTrade = Value

            '    End If

            'update the member ID
            '    m_vPartyTradeID = lID


        End Set
    End Property
    Public Property PartyAgentOrigin() As String
        Get

            Return m_vPartyAgentOrigin

        End Get
        Set(ByVal Value As String)

            Dim lID As Integer

            'If description this has changed need to get new look up ID for it

            If CStr(Value).Trim() <> m_vPartyAgentOrigin.Trim() Then


                'Developer Guide No. 98
                m_lReturn = GetLookUpID(sTable:=gSIRLibrary.SIRLookupPartyAgentOrigin, sField:="description", sDescription:=Value, sIDColumnName:="party_agent_origin_id", lIDValue:=lID)

                'update the member ID
                m_vPartyAgentOriginID = lID


                m_vPartyAgentOrigin = CStr(Value)

            End If

            'update the member ID
            '    m_vPartyAgentOriginID = lID


        End Set
    End Property

    Public Property Password() As Object
        Get
            Return m_vPassword
        End Get
        Set(ByVal Value As Object)


            m_vPassword = Value
            m_bNetDataChanged = True
        End Set
    End Property

    Public Property MothersMaidenName() As Object
        Get
            Return m_vMothersMaidenName
        End Get
        Set(ByVal Value As Object)


            m_vMothersMaidenName = Value
            m_bNetDataChanged = True
        End Set
    End Property

    Public Property TPIntroducerCode() As Object
        Get
            Return m_vTPIntroducerCode
        End Get
        Set(ByVal Value As Object)


            m_vTPIntroducerCode = Value
            m_bNetDataChanged = True
        End Set
    End Property

    Public Property TPUserCode() As Object
        Get
            Return m_vTPUserCode
        End Get
        Set(ByVal Value As Object)


            m_vTPUserCode = Value
            m_bNetDataChanged = True
        End Set
    End Property

    Public Property MemorableDate() As Object
        Get
            Return m_vMemorableDate
        End Get
        Set(ByVal Value As Object)


            m_vMemorableDate = Value
            m_bNetDataChanged = True
        End Set
    End Property

    Public Property AQuestion() As Object
        Get
            Return m_vAQuestion
        End Get
        Set(ByVal Value As Object)


            m_vAQuestion = Value
            m_bNetDataChanged = True
        End Set
    End Property

    Public Property TheAnswer() As Object
        Get
            Return m_vTheAnswer
        End Get
        Set(ByVal Value As Object)


            m_vTheAnswer = Value
            m_bNetDataChanged = True
        End Set
    End Property


    Public Property CurrInsRenewalDate() As Object
        Get
            Return m_vCurrInsRenewalDate
        End Get
        Set(ByVal Value As Object)


            m_vCurrInsRenewalDate = Value
            m_bNetDataChanged = True
        End Set
    End Property


    Public Property UserID() As Object
        Get
            Return m_vUserID
        End Get
        Set(ByVal Value As Object)


            m_vUserID = Value
            m_bNetDataChanged = True
        End Set
    End Property

    ' TF301000 - Additional properties for PartyFP

    Public Property FinanceProviderNumber() As Object
        Get
            Return m_vFinanceProviderNumber
        End Get
        Set(ByVal Value As Object)


            m_vFinanceProviderNumber = Value
        End Set
    End Property


    Public Property MailboxNumber() As Object
        Get
            Return m_vMailboxNumber
        End Get
        Set(ByVal Value As Object)


            m_vMailboxNumber = Value
        End Set
    End Property
    'DC190402 -start

    Public Property RefNumber() As Object
        Get
            Return m_vRefNumber
        End Get
        Set(ByVal Value As Object)


            m_vRefNumber = Value
        End Set
    End Property


    Public Property RegNumber() As Object
        Get
            Return m_vRegNumber
        End Get
        Set(ByVal Value As Object)


            m_vRegNumber = Value
        End Set
    End Property


    Public Property ExternalId() As Object
        Get
            Return m_vExternalId
        End Get
        Set(ByVal Value As Object)


            m_vExternalId = Value
        End Set
    End Property


    Public Property LicenseTypeId() As Object
        Get
            Return m_vLicenseTypeId
        End Get
        Set(ByVal Value As Object)


            m_vLicenseTypeId = Value
        End Set
    End Property


    Public Property LicenseNumber() As Object
        Get
            Return m_vLicenseNumber
        End Get
        Set(ByVal Value As Object)


            m_vLicenseNumber = Value
        End Set
    End Property


    Public Property PartyStatus() As Object
        Get
            Return m_vPartyStatus
        End Get
        Set(ByVal Value As Object)


            m_vPartyStatus = Value
        End Set
    End Property
    'DC100402 -End

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'RAM20021016 : NRMA Changes - Sirius Process No 126 - Start
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public ReadOnly Property PreferredContactType() As String
        Get
            Return m_sPreferredContactType
        End Get
    End Property

    Public ReadOnly Property PreferredContactDetail() As String
        Get
            Return m_sPreferredContactDetail
        End Get
    End Property

    Public ReadOnly Property AllContactsArray() As Object
        Get
            Return m_vAllContactsArray
        End Get
    End Property

    Public ReadOnly Property ConvictionsArray() As Object
        Get
            Return m_vConvictionsArray
        End Get
    End Property


    ' RAW 21/02/2003 : ISS2379 : added
    Private ReadOnly Property SolutionPartyObjectName() As String
        Get


            Select Case m_sSolutionPartyType
                Case gSIRLibrary.SIRPartyTypePersonalClient, gSIRLibrary.SIRPartyTypeCorporateClient, gSIRLibrary.SIRPartyTypeGroupClient, gSIRLibrary.SIRPartyTypeAccountHandler, gSIRLibrary.SIRPartyTypeInsurer, gSIRLibrary.SIRPartyTypeAgent, gSIRLibrary.SIRPartyTypeAgentGroup, gSIRLibrary.SIRPartyTypeNetClient, gSIRLibrary.SIRPartyTypeFinanceProvider

                    ' Use the full party type code to identify the correct party component to use.
                    ' eg use PartyAG for AG, use PartyAGG for AGG etc
                    Return "b" & m_sSolutionCode & _
                    "Party" & m_sSolutionPartyType & _
                    ".Business"

                Case gSIRLibrary.SIRPartyTypeConsultant
                    ' Use PartyAH
                    Return "b" & m_sSolutionCode & _
                    "PartyAH" & _
                    ".Business"

                Case Else

                    If (m_sSolutionPartyType.Substring(0, 2)) = gSIRLibrary.SIRPartyTypeOther Then
                        ' This only tests the first 2 characters
                        ' Use PartyOT for all Types that begin with OT
                        Return "b" & m_sSolutionCode & _
                        "Party" & m_sSolutionPartyType.Substring(0, 2) & _
                                                                 ".Business"
                    Else
                        Return ""
                    End If
            End Select
        End Get
    End Property
    ' RAW 21/02/2003 : ISS2379 : end


    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'RAM20021016 : NRMA Changes - Sirius Process No 126 - End
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


    ' TF200700 - Replaced by new Login()
    '' ***************************************************************** '
    '' Name: Login (Public)
    ''
    '' Description: Gets the required PartyCnt.  Used only for NC Clients
    ''
    '' ***************************************************************** '
    'Public Function Login() As Long
    '
    '    On Error GoTo Err_Login
    '
    '    Login = PMTrue
    '
    '    Set m_oSolutionParty = CreateObject("bSIRPartyNC.Business")
    '
    '    m_lReturn& = m_oSolutionParty.Initialise( _
    ''            sUsername:=m_sUsername$, _
    ''            sPassword:=m_sPassword$, _
    ''            iUserID:=m_iUserID%, _
    ''            iSourceID:=m_iSourceID%, _
    ''            iLanguageID:=m_iLanguageID%, _
    ''            iCurrencyID:=m_iCurrencyID%, _
    ''            iLogLevel:=m_iLogLevel%, _
    ''            sCallingAppName:=m_sCallingAppName$, _
    ''            vDatabase:=m_oDatabase)
    '
    '    m_lReturn& = m_oSolutionParty.Login(sUserID:=UserID)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        Login = PMFalse
    '        Exit Function
    '    End If
    '
    '    PartyCnt = m_oSolutionParty.PartyCnt
    '
    '    Set m_oSolutionParty = Nothing
    '
    '    Exit Function
    '
    '
    'Err_Login:
    '
    '    Login = PMError
    '
    '    LogMessage m_sUsername, _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="Login Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="Login", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Private Function BeginTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Private Function CommitTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Private Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        'EK Create Private copy

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            'EK 22/9/90
            'SD 02/08/2002

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)
            'EK 22/9/99 - terminate at the end

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create instance of Party Business class
            m_oSIRParty = New Business()

            m_lReturn = m_oSIRParty.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=sCallingAppName, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Create PM Lookup Business Object
            m_oLookup = New BPMLOOKUP.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

            m_vCountryId = CStr(0)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If m_oSIRParty IsNot Nothing Then
                    m_oSIRParty.Dispose()
                    m_oSIRParty = Nothing
                End If
                If m_oSolutionParty IsNot Nothing Then
                    m_oSolutionParty.Dispose()
                    m_oSolutionParty = Nothing
                End If
                If m_oLookup IsNot Nothing Then
                    m_oLookup.Dispose()
                    m_oLookup = Nothing
                End If
                If m_oParty IsNot Nothing Then
                    m_oParty.Dispose()
                    m_oParty = Nothing
                End If
                If m_oAddress IsNot Nothing Then
                    m_oAddress.Dispose()
                    m_oAddress = Nothing
                End If
                If m_oContact IsNot Nothing Then
                    m_oContact.Dispose()
                    m_oContact = Nothing
                End If
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required Party entity properties.
    '
    ' Edit History  :
    ' RAM20021016   : NRMA Changes - Sirius Process No 126
    ' ***************************************************************** '
    Public Function GetDetails() As Integer

        Dim result As Integer = 0
        Dim vTableArray As Object = Nothing
        Dim vResultArray As Object = Nothing
        Dim sSQL As String = ""

        'Dim sShortName As String
        'Dim sResolvedName As String

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' RAM20021016   : NRMA Changes - Sirius Process No 126 - Start
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim sErrorMessage As String = String.Empty
        Dim sPreferredEMailAddress As String = String.Empty
        Dim sPreferredFAXAreaCode As String = String.Empty
        Dim sPreferredFAXNumber As String = String.Empty
        Dim sPreferredFAXExtn As String = String.Empty
        Dim sPreferredContactDetail As String = String.Empty ' Final Value
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' RAM20021016   : NRMA Changes - Sirius Process No 126 - Start
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check for Party identity
            m_lReturn = GetIdentity()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get Solution specific details
            m_lReturn = GetSolutionParty()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Call solution/party specific
            Select Case m_sSolutionCode
                Case gSIRLibrary.SIRCoreSolution

                    m_lReturn = m_oSolutionParty.GetDetails(vPartyCnt:=ToSafeInteger(m_lPartyCnt))

                    If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                        Return gPMConstants.PMEReturnCode.PMNotFound
                    ElseIf (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' RAW 21/02/2003 : ISS2379 : added message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details for PartyCnt " & m_lPartyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                        m_oSolutionParty.Dispose()
                        Return result
                    End If
                    'MSS200901 - Changed for merge. Should be OK with SFORB

                    'RWH(23/07/01) Use 2 left most chars of Party Type only to cater for
                    'other party types.
                    ' RAW 21/02/2003 : ISS2379 : moved test against 1st 2 character into case else

                    Select Case m_sSolutionPartyType
                        'Personal Client
                        Case gSIRLibrary.SIRPartyTypePersonalClient, gSIRLibrary.SIRPartyTypeNoneInsuredClient
                            'SP231198

                            'sj 11/11/99 - start
                            'Added NumberOfChildren, DateOfBirth, MaritalStatus & Gender
                            'sj 11/11/99 - end

                            m_lReturn = m_oSolutionParty.GetNext(vPartyCnt:=m_lPartyCnt, vPartyTitleCode:=m_vPartyTitleCode, vForeName:=m_vForeName, vInitials:=m_vInitials, vSourceID:=m_vSourceID, vPartyID:=m_vPartyID, vCurrencyId:=m_vCurrencyID, vShortname:=m_vShortName, vName:=m_vName, vAgentCnt:=ToSafeInteger(m_vAgentCnt), vNumberOfChildren:=m_vNumberOfChildren, vDateOfBirth:=m_vDateOfBirth, vMaritalStatusCode:=m_vMaritalStatusCode, vAreaId:=m_vAreaId, vGender:=m_vGenderCode, vResolvedName:=m_vResolvedName, vSubBranchId:=m_vSubBranchId)
                            'MSS200901 - Added extra parameters above for UW

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse

                                m_oSolutionParty.Dispose()
                                Return result
                            End If

                            'Get Lookups

                            m_lReturn = m_oSolutionParty.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=vTableArray, iLanguageID:=m_iLanguageID, vResultArray:=vResultArray)

                            'PWF 22/08/2002 - Lookup details not set in bSIRPartyPC
                            ' Removed from here to prevent logged error.
                            'm_lReturn& = GetLookupDetails( _
                            'sLookupTable:=SIRLookupPartyTitle, _
                            'vTableArray:=vTableArray, _
                            'vResultArray:=vResultArray, _
                            'vResult:=m_vPartyTitle)

                            'm_lReturn& = GetLookupDetails( _
                            'sLookupTable:=SIRLookupPartyOccupation, _
                            'vTableArray:=vTableArray, _
                            'vResultArray:=vResultArray, _
                            'vResult:=m_vPartyOccupation)

                        Case gSIRLibrary.SIRPartyTypeCorporateClient

                            'SP050199
                            'EK 10/09/99 These Parameters don't match

                            m_lReturn = m_oSolutionParty.GetNext(vPartyCnt:=m_lPartyCnt, vPartyBusinessId:=m_vPartyBusinessID, vCompanyReg:=m_vCompanyReg, vTradingSinceDate:=m_vTradingSinceDate, vSourceID:=m_vSourceID, vPartyID:=m_vPartyID, vCurrencyId:=m_vCurrencyID, vShortname:=m_vShortName, vName:=m_vName, vAreaId:=m_vAreaId, vAgentCnt:=m_vAgentCnt)
                            'MSS200901 - Added AreaID for merge

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse

                                m_oSolutionParty.Dispose()
                                Return result
                            End If

                            'MSS200901 - Added for UW
                            m_vResolvedName = m_vName
                            'MSS200901 - Merge End

                            'Get Lookups

                            m_lReturn = m_oSolutionParty.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=vTableArray, iLanguageID:=m_iLanguageID, vResultArray:=vResultArray)

                            'PWF 22/08/2002 - Lookup details not set in bSIRPartyCC
                            ' Removed from here to prevent logged error.
                            'm_lReturn& = GetLookupDetails( _
                            'sLookupTable:=SIRLookupPartyTrade, _
                            'vTableArray:=vTableArray, _
                            'vResultArray:=vResultArray, _
                            'vResult:=m_vPartyTrade)

                        Case gSIRLibrary.SIRPartyTypeGroupClient
                            'SP050199

                            m_lReturn = m_oSolutionParty.GetNext(vPartyCnt:=m_lPartyCnt, vPartyGroupTypeID:=m_vPartyGroupTypeID, vIsRegisteredCharity:=m_vIsRegisteredCharity, vSourceID:=m_vSourceID, vPartyID:=m_vPartyID, vCurrencyId:=m_vCurrencyID, vShortname:=m_vShortName, vName:=m_vName, vAreaId:=m_vAreaId, vResolvedName:=m_vResolvedName, vAgentCnt:=m_vAgentCnt)
                            'MSS200901 - Added AreaID for merge

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse

                                m_oSolutionParty.Dispose()
                                Return result
                            End If

                            'Get Lookups

                            m_lReturn = m_oSolutionParty.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=vTableArray, iLanguageID:=m_iLanguageID, vResultArray:=vResultArray)


                            'PWF 22/08/2002 - Lookup details not set in bSIRPartyGC
                            ' Removed from here to prevent logged error.
                            'm_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupPartyGroupType, vTableArray:=vTableArray, vResultArray:=vResultArray, vResult:=m_vPartyGroupType)


                        Case gSIRLibrary.SIRPartyTypeAgent

                            'SP050199
                            'DC 27/07/00 Added Party Agent Type ID
                            ' RDC 25102002 added SubBranchID

                            m_lReturn = m_oSolutionParty.GetNext(vPartyCnt:=m_lPartyCnt, vPartyAgentTypeID:=m_vPartyAgentTypeID, vPartyAgentOriginID:=m_vPartyAgentOriginID, vAgencyAgreementDate:=m_vAgencyAgreementDate, vAgencyNextReviewDate:=m_vAgencyNextReviewDate, vIsBranch:=m_vIsBranch, vIsHeadOffice:=m_vIsHeadOffice, vSourceID:=m_vSourceID, vPartyID:=m_vPartyID, vCurrencyId:=m_vCurrencyID, vShortname:=m_vShortName, vName:=m_vName, vAgentCnt:=m_vAgentCnt, vSubBranch:=m_vSubBranchId, vPaymentMethod:=m_vPaymentMethod, vContactPerson:=m_vContactPerson, vFirstName:=m_vFirstName)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse

                                m_oSolutionParty.Dispose()
                                Return result
                            End If

                            'Get Lookups

                            m_lReturn = m_oSolutionParty.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=vTableArray, iLanguageID:=m_iLanguageID, vResultArray:=vResultArray)



                            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupPartyAgentOrigin, vTableArray:=vTableArray, vResultArray:=vResultArray, vResult:=m_vPartyAgentOrigin)


                            ' RAW 21/02/2003 : ISS2379 : added Agent Group
                        Case gSIRLibrary.SIRPartyTypeAgentGroup

                            ' Agent Group


                            m_lReturn = m_oSolutionParty.GetNext(vPartyCnt:=m_lPartyCnt, vShortname:=m_vShortName, vName:=m_vName, vPartyAgentBranch:=m_vSourceID, vActive:=m_vIsGroupActive)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse

                                m_oSolutionParty.Dispose()
                                Return result
                            End If


                            'ECK 02/08/99 Add Insurer to list
                        Case gSIRLibrary.SIRPartyTypeInsurer

                            m_lReturn = m_oSolutionParty.GetNext(vPartyCnt:=m_lPartyCnt, vSourceID:=m_vSourceID, vPartyID:=m_vPartyID, vCurrencyId:=m_vCurrencyID, vShortname:=m_vShortName, vName:=m_vName, vSubBranchId:=m_vSubBranchId)

                            'ECK 02/08/99 Add Account Handler to list
                            'ECK 25/11/99 This extremely suspect - coding of bPartyAH makes
                            ' it impossible to get all party details in one hit , it should
                            'be recoded when there is time
                            'PN18406 Add Executive Handler
                            'PN67180 Add Account Executive
                        Case gSIRLibrary.SIRPartyTypeAccountHandler, gSIRLibrary.SIRPartyTypeExecutiveHandler

                            m_lReturn = m_oSolutionParty.GetNext(vPartyCnt:=m_lPartyCnt, vSourceID:=m_vSourceID, vPartyID:=m_vPartyID, vCurrencyId:=m_vCurrencyID, vShortname:=m_vShortName, vName:=m_vName)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse

                                m_oSolutionParty.Dispose()
                                Return result
                            End If

                            'ECK 9/11/99 Add Odds to list
                            'ECK 27/1/00 Add Commission Account to List
                        Case gSIRLibrary.SIRPartyTypeFee, gSIRLibrary.SIRPartyTypeExtra, gSIRLibrary.SIRPartyTypeDiscount, gSIRLibrary.SIRPartyTypeCommission

                            m_lReturn = m_oSolutionParty.GetNext(vPartyCnt:=m_lPartyCnt, vSourceID:=m_vSourceID, vPartyID:=m_vPartyID, vCurrencyId:=m_vCurrencyID, vShortname:=m_vShortName, vName:=m_vName)

                            ' TF250700 - NetClient now replaced by PartyNetData
                            ''Tomo260500 Add Net Client to list
                            '                Case SIRPartyTypeNetClient
                            '                'RJG 09/06/2000 Added Shortname, Name, DOB, UserID and Curr Ins Renewal date to GetNext call
                            '                    m_lReturn& = m_oSolutionParty.GetNext( _
                            ''                                        vPartyCnt:=m_lPartyCnt, _
                            ''                                        vPassword:=m_vPassword, _
                            ''                                        vMothersMaidenName:=m_vMothersMaidenName, _
                            ''                                        vTPIntroducerCode:=m_vTPIntroducerCode, _
                            ''                                        vTPUserCode:=m_vTPUserCode, _
                            ''                                        vMemorableDate:=m_vMemorableDate, _
                            ''                                        vAQuestion:=m_vAQuestion, _
                            ''                                        vTheAnswer:=m_vTheAnswer, _
                            ''                                        vShortname:=m_vShortName, _
                            ''                                        vName:=m_vName, _
                            ''                                        vDateOfBirth:=m_vDateOfBirth, _
                            ''                                        vUserID:=m_vUserID, _
                            ''                                        vCurrInsRenewalDate:=m_vCurrInsRenewalDate)
                            ' TF301000
                        Case gSIRLibrary.SIRPartyTypeFinanceProvider

                            m_lReturn = m_oSolutionParty.GetNext(vPartyCnt:=m_lPartyCnt, vSourceID:=m_vSourceID, vPartyID:=m_vPartyID, vCurrencyId:=m_vCurrencyID, vShortname:=m_vShortName, vName:=m_vName, vFinanceProviderNumber:=m_vFinanceProviderNumber, vAgencyNumber:=m_vAgencyNumber, vMailboxNumber:=m_vMailboxNumber)




                            'PN-67185
                            'Error Log is generating as this condition
                            'is not mentioned in select case statement
                            'and case else statement is executing
                            'which printing error log file
                        Case gSIRLibrary.SIRPartyTypeConsultant
                            Return result


                        Case Else

                            'MSS200901 - Added for merge
                            'RWH(23/07/01) Add Other Party Types
                            ' RAW 21/02/2003 : ISS2379 : moved test for 'OT' to within case else
                            If (m_sSolutionPartyType.Substring(0, 2)) = gSIRLibrary.SIRPartyTypeOther Then


                                m_lReturn = m_oSolutionParty.GetNext(vPartyCnt:=m_lPartyCnt, vPartyID:=m_vPartyID, vShortname:=m_vShortName, vName:=m_vName, vCurrencyId:=m_vCurrencyID, vDateOfBirth:=m_vDateOfBirth, vGender:=m_vGenderCode, vLicenseTypeId:=m_vLicenseTypeId, vLicenseNumber:=m_vLicenseNumber, vStatus:=m_vStatus, vRegNumber:=m_vRegNumber, vSourceID:=m_vSourceID, vSubBranchId:=m_vSubBranchId)
                                'MSS200901 - Merge End

                            Else

                                result = gPMConstants.PMEReturnCode.PMFalse

                                m_oSolutionParty.Dispose()
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Party type '" & m_sSolutionPartyType & "' is unrecognised", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                Return result

                            End If

                    End Select

                    'case "<solution code>"
                    'case "<solution code>"
                    ' etc .. (solution specific stuff)
                Case Else

                    result = gPMConstants.PMEReturnCode.PMFalse

                    m_oSolutionParty.Dispose()
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Solution Code '" & m_sSolutionCode & "' is unrecognised", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result

            End Select

            'TF170700 Get Generic Net Data
            m_lReturn = GetNetData()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' TF180700 - OK if no Net Data found
                ' Can be validated by the calling APP
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNetData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If

            'TF200700 Get Generic Party Lifestyle data
            m_lReturn = GetLifestyleData()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' OK if no Net Data found
                ' Can be validated by the calling APP
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLifestyleData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If


            'EK 22/10/99 We dont keep address/contact details for account handler
            'DC100402 changed check for first two chars
            Select Case m_sSolutionPartyType.Substring(0, 2)
                'PN12255
                Case gSIRLibrary.SIRPartyTypeAccountHandler, gSIRLibrary.SIRPartyTypeExecutiveHandler
                    Return result
                    'EK 9/11/99 EK Don't keep address details for this lot either
                    'DC 22/05/00 Don't keep address details for Commission Accounts
                    'Case SIRPartyTypeFee, SIRPartyTypeExtra, SIRPartyTypeDiscount,
                Case gSIRLibrary.SIRPartyTypeCommission, gSIRLibrary.SIRPartyTypeNoneInsuredClient
                    Return result

                Case Else
                    'SP071298 - Need to get the address details
                    m_lReturn = GetAddressDetails()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                            Select Case m_sSolutionPartyType.Substring(0, 2)
                                Case gSIRLibrary.SIRPartyTypeFee, gSIRLibrary.SIRPartyTypeExtra, gSIRLibrary.SIRPartyTypeDiscount
                                    'Perfectly valid as these types might not have a main address.
                                    'Still exit the function though.
                                Case Else
                                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Could not find main address for party.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")
                                    result = gPMConstants.PMEReturnCode.PMFalse
                            End Select
                        Else
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If

                        Return result
                    End If

                    ' TF250700 - This call loses overwrites contact_type_id with description
                    '            m_lReturn = m_oSIRParty.GetContactDetails(PartyCnt, ContactArray)
                    '            If (m_lReturn& <> PMTrue) Then
                    '                GetDetails = PMFalse
                    '                Exit Function
                    '            End If

                    ' TF250700 - Net Client now replaced by PartyNetData
                    'RJG 12/06/2000 - Get contact array details for e-mail address
                    '            If m_sSolutionPartyType = SIRPartyTypeNetClient Then

                    ' TF250700 - Use this to get email address
                    sSQL = "SELECT number" &
                           " FROM Contact C," &
                           " Party_Contact_Usage U," &
                           " Contact_Type T" &
                           " WHERE U.party_cnt = " & CStr(m_lPartyCnt) &
                           " AND C.contact_cnt = U.contact_cnt" &
                           " AND T.contact_type_id = C.contact_type_id" &
                           " AND T.code = '" & gSIRLibrary.SIREmailContactCode & "'"

                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GETAMAINCONTACT", bStoredProcedure:=False, vResultArray:=vResultArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Set the email details from the array
                    If Informations.IsArray(vResultArray) Then

                        m_vEmailAddress = CStr(vResultArray(0, 0)).Trim()
                    End If


                    ' TF250700 - NOTE this bit only returns the main address & tel.
                    'SP231198 - Need to get a contact for when Orion uses services
                    sSQL = "SELECT contact_type.code, area_code, number, extension, '' " &
                           "FROM contact, address, contact_address_usage, party_address_usage, " &
                           "address_usage_type, contact_type WHERE " &
                           "party_cnt = " & CStr(m_lPartyCnt) & " AND " &
                           "party_address_usage.address_cnt = address.address_cnt AND " &
                           "party_address_usage.address_usage_type_id = address_usage_type.address_usage_type_id AND " &
                           "address_usage_type.code = '" & gSIRLibrary.SIRMainAddressABICode & "' AND " &
                           "contact_address_usage.address_cnt = address.address_cnt AND " &
                           "contact.contact_cnt = contact_address_usage.contact_cnt AND " &
                           "contact.contact_type_id = contact_type.contact_type_id AND " &
                           "contact_type.code = '" & gSIRLibrary.SIRTelephoneContactCode & "'"
                    '            End If

                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GETAMAINCONTACT", bStoredProcedure:=False, vResultArray:=vResultArray)


                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Set the contact details from the array
                    If Informations.IsArray(vResultArray) Then

                        m_vAreaCode = CStr(vResultArray(1, 0)).Trim()

                        m_vNumber = CStr(vResultArray(2, 0)).Trim()

                        m_vExtension = CStr(vResultArray(3, 0)).Trim()
                    End If



                    ContactArray = vResultArray

                    vResultArray = Nothing

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    ' RAM20021016 : NRMA Changes - Sirius Process No 126 - Start
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    ' Preferred Contact Derails
                    m_sPreferredContactType = ""
                    m_sPreferredContactDetail = ""

                    m_lReturn = m_oSIRParty.GetPreferredContact(v_lPartyCnt:=m_lPartyCnt, r_sContactType:=m_sPreferredContactType, r_sErrorMessage:=sErrorMessage, r_sEmailAddress:=sPreferredEMailAddress, r_sFaxCode:=sPreferredFAXAreaCode, r_sFaxNumber:=sPreferredFAXNumber, r_sFaxExt:=sPreferredFAXExtn)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        ' Log Error!!!

                        'MKW310703 PN5794 Remove Function Returning false as can be ignored.
                        ' (i.e. perfered contact of fax and no fax number).
                        'GetDetails = PMFalse

                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPreferredContact Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                        'MKW310703 PN5794
                        'Exit Function
                    End If

                    If sErrorMessage.Trim().Length = 0 Then
                        ' no error
                        ' Trim Just In Case
                        'Developer Guide No. 131
                        If Not sPreferredEMailAddress Is Nothing Then
                            sPreferredEMailAddress = sPreferredEMailAddress.Trim()
                        End If
                        If Not sPreferredFAXNumber Is Nothing Then
                            sPreferredFAXNumber = sPreferredFAXNumber.Trim()
                        End If
                        If Not sPreferredFAXExtn Is Nothing Then
                            sPreferredFAXExtn = sPreferredFAXExtn.Trim()
                        End If

                        ' Which type of correspondence do they prefer? FAX or EMAIL

                        sPreferredContactDetail = ""


                        Select Case m_sPreferredContactType
                            Case "FAX" ' Fax
                                ' Append the Area Code, Number and Extn if Any

                                ' Append area code if any
                                If sPreferredFAXAreaCode.Length > 0 Then
                                    sPreferredContactDetail = sPreferredContactDetail & sPreferredFAXAreaCode & " "
                                End If

                                sPreferredContactDetail = sPreferredContactDetail & sPreferredFAXNumber

                                ' Append extn number if any
                                If sPreferredFAXExtn.Length > 0 Then
                                    sPreferredContactDetail = sPreferredContactDetail & " Extn. " & sPreferredFAXExtn
                                End If

                            Case "E-MAIL" ' E-Mail
                                ' Return the E-Mail address
                                sPreferredContactDetail = sPreferredEMailAddress
                        End Select

                    End If
                    ' Return the Value
                    m_sPreferredContactDetail = sPreferredContactDetail

                    ' Get All Contact Details


                    'Developer Guide No. 17
                    m_vAllContactsArray = Nothing

                    sSQL = ""
                    sSQL = sSQL & "SELECT T.Code as contact_type, c.area_code, c.number, c.extension, c.description "
                    sSQL = sSQL & "FROM   Contact C, "
                    sSQL = sSQL & "       Party_Contact_Usage U, "
                    sSQL = sSQL & "       Contact_Type T "
                    sSQL = sSQL & "WHERE  U.party_cnt = " & CStr(m_lPartyCnt) & " "
                    sSQL = sSQL & "AND    C.contact_cnt = U.contact_cnt "
                    sSQL = sSQL & "AND    T.contact_type_id = C.contact_type_id"

                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAllContacts", bStoredProcedure:=False, vResultArray:=vResultArray)


                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Set the contact details from the array
                    If Informations.IsArray(vResultArray) Then


                        m_vAllContactsArray = vResultArray
                    End If

                    vResultArray = Nothing

                    ' Fetch the Conviction Details


                    'Developer Guide No. 17
                    m_vConvictionsArray = Nothing

                    sSQL = ""

                    'Modifying the inline query to make it compatible with SQL server 2005

                    sSQL = sSQL & "SELECT conviction_date, code, description, sentence_code, "
                    sSQL = sSQL & "       sentence_description, sentence_duration, "
                    sSQL = sSQL & "       sentence_duration_qualifier, sentence_effective_date "
                    sSQL = sSQL & " FROM   party P"
                    sSQL = sSQL & " LEFT OUTER JOIN party_conviction PC"
                    sSQL = sSQL & "       ON PC.party_cnt = P.party_cnt"
                    sSQL = sSQL & " WHERE  P.party_cnt = " & CStr(m_lPartyCnt)

                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAllConvictions", bStoredProcedure:=False, vResultArray:=vResultArray)


                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Set the contact details from the array
                    If Informations.IsArray(vResultArray) Then


                        m_vConvictionsArray = vResultArray
                    End If

                    vResultArray = Nothing

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    ' RAM20021016 : NRMA Changes - Sirius Process No 126 - End
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            End Select

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateParty
    '
    ' Description: Update Party using supplied properties.
    '
    ' ***************************************************************** '
    Public Function UpdateParty() As Integer

        Dim result As Integer = 0
        Try

            '**** START CHANGES - Changed By: AAB  - Changed On: 11-Oct-2002 11:40   ****
            '**** Added this for UpdateContacts
            Dim sSolutionPartyTypePrefix As String = ""
            '****   END CHANGES - Changed By: AAB  - Changed On: 11-Oct-2002 11:40   ****

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check for Party identity
            m_lReturn = GetIdentity()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Start Transaction
            m_lReturn = BeginTrans()

            ' Update Solution specific details
            m_lReturn = GetSolutionParty()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSolutionPartyTypePrefix = m_sSolutionPartyType.Substring(0, 2)

            'Call solution/party specific
            Select Case m_sSolutionCode
                Case gSIRLibrary.SIRCoreSolution
                    'DC100402 changed check for first two chars
                    ' RAW 21/02/2003 : ISS2379 : changed to test full string

                    Select Case sSolutionPartyTypePrefix
                        Case gSIRLibrary.SIRPartyTypePersonalClient

                            'RDT 18/9/2005 - Added ResolvedName parameter

                            m_lReturn = m_oSolutionParty.EditUpdate(lRow:=1, vPartyCnt:=ToSafeInteger(m_lPartyCnt), vPartyTitleCode:=m_vPartyTitleCode, vForeName:=m_vForeName, vInitials:=m_vInitials, vShortname:=m_vShortName, vName:=m_vName, vAgentCnt:=ToSafeInteger(m_vAgentCnt), vPartyLifestyleID:=m_vPartyLifestyleID, vPartyLifestyleName:=m_vName, vCategory:=m_vCategory, vOccupationCode:=m_vOccupationCode, vDateOfBirth:=m_vDateOfBirth, vGender:=m_vGenderCode, vSecondaryOccupationCode:=m_vSecondaryOccupationCode, vIsSmoker:=m_vIsSmoker, vResolved:=m_vResolvedName, vFileCode:=m_vFileCode, vAlternativeIdentifier:=m_vAlternativeIdentifier, vEmployerBusiness:=m_vEmployerBusinessCode, vEmploymentStatusCode:=m_vEmploymentStatusCode, vMaritalStatusCode:=m_vMaritalStatusCode)

                            'MSS200901 - Added extra details for UW

                        Case gSIRLibrary.SIRPartyTypeAgent

                            'DC 27/07/00 Added Party Agent Type ID

                            m_lReturn = m_oSolutionParty.EditUpdate(lRow:=1, vPartyCnt:=ToSafeInteger(m_lPartyCnt), vPartyAgentTypeID:=m_vPartyAgentTypeID, vPartyAgentOriginID:=m_vPartyAgentOriginID, vAgencyAgreementDate:=m_vAgencyAgreementDate, vAgencyNextReviewDate:=m_vAgencyNextReviewDate, vIsBranch:=m_vIsBranch, vIsHeadOffice:=m_vIsHeadOffice, vShortname:=m_vShortName, vName:=m_vName, vAgentCnt:=m_vAgentCnt, vPaymentMethod:=m_vPaymentMethod)

                            ' RAW 21/02/2003 : ISS2379 : added Agent Group
                        Case gSIRLibrary.SIRPartyTypeAgentGroup

                            ' Agent Group


                            m_lReturn = m_oSolutionParty.EditUpdate(lRow:=1, vPartyCnt:=ToSafeInteger(m_lPartyCnt), vShortname:=m_vShortName, vName:=m_vName, vPartyAgentBranch:=m_vSourceID, vActive:=m_vIsGroupActive)

                        Case gSIRLibrary.SIRPartyTypeGroupClient

                            'SP050199

                            m_lReturn = m_oSolutionParty.EditUpdate(lRow:=1, vPartyCnt:=ToSafeInteger(m_lPartyCnt), vPartyGroupTypeID:=m_vPartyGroupTypeID, vIsRegisteredCharity:=m_vIsRegisteredCharity, vShortname:=m_vShortName, vName:=m_vName, vResolvedName:=m_vResolvedName, vAgentCnt:=ToSafeInteger(m_vAgentCnt))

                        Case gSIRLibrary.SIRPartyTypeCorporateClient

                            'MSS200901 - UW uses vPartyTradeID instead of vPartyBusinessId
                            ' Left as is. If this causes problems, will need to update business
                            'SP050199

                            m_lReturn = m_oSolutionParty.EditUpdate(lRow:=1, vPartyCnt:=ToSafeInteger(m_lPartyCnt), vPartyBusinessId:=m_vPartyBusinessID, vCompanyReg:=m_vCompanyReg, vTradingSinceDate:=m_vTradingSinceDate, vShortname:=m_vShortName, vName:=m_vName, vResolvedName:=m_vResolvedName, vAgentCnt:=ToSafeInteger(m_vAgentCnt), vFileCode:=m_vFileCode, vAlternativeIdentifier:=m_vAlternativeIdentifier)

                            ' TF301000
                        Case gSIRLibrary.SIRPartyTypeFinanceProvider

                            m_lReturn = m_oSolutionParty.EditUpdate(lRow:=1, vPartyCnt:=ToSafeInteger(m_lPartyCnt), vFinanceProviderNumber:=m_vFinanceProviderNumber, vAgencyNumber:=m_vAgencyNumber, vMailboxNumber:=m_vMailboxNumber, vShortname:=m_vShortName, vName:=m_vName, vCurrencyId:=m_vCurrencyID)

                            ' TF250700 - NetClient now replaced by PartyNetData
                            '                Case SIRPartyTypeNetClient
                            '
                            '                    m_lReturn& = m_oSolutionParty.EditUpdate( _
                            ''                                        lRow:=1, _
                            ''                                        vPartyCnt:=m_lPartyCnt, _
                            ''                                        vPassword:=m_vPassword, _
                            ''                                        vMothersMaidenName:=m_vMothersMaidenName, _
                            ''                                        vTPIntroducerCode:=m_vTPIntroducerCode, _
                            ''                                        vTPUserCode:=m_vTPUserCode, _
                            ''                                        vMemorableDate:=m_vMemorableDate, _
                            ''                                        vAQuestion:=m_vAQuestion, _
                            ''                                        vTheAnswer:=m_vTheAnswer, _
                            ''                                        vUserID:=m_vUserID, _
                            ''                                        vCurrInsRenewalDate:=m_vCurrInsRenewalDate, _
                            ''                                        vName:=m_vName, _
                            ''                                        vShortname:=m_vShortName, _
                            ''                                        vDateOfBirth:=m_vDateOfBirth)


                        Case gSIRLibrary.SIRPartyTypeOther
                            ' do nothing

                        Case Else
                            result = gPMConstants.PMEReturnCode.PMFalse

                            m_oSolutionParty.Dispose()

                            ' ensure the transaction is actually rolled
                            ' back when the party is not known
                            m_oDatabase.SQLRollbackTrans()

                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Party type '" & m_sSolutionPartyType & "' is unrecognised", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                            Return result

                    End Select

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        m_lReturn = RollbackTrans()

                        m_oSolutionParty.Dispose()
                        Return result
                    End If


                    m_lReturn = m_oSolutionParty.Update()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        m_lReturn = RollbackTrans()

                        m_oSolutionParty.Dispose()
                        Return result
                    End If

                    'case "<solution code>"
                    'case "<solution code>"
                    'etc for specific solutions

                Case Else
                    result = gPMConstants.PMEReturnCode.PMFalse

                    m_oSolutionParty.Dispose()
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Solution Code '" & m_sSolutionCode & "' is unrecognised", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result

            End Select

            ' TF170700 Update Generic Net Data
            ' Only if NetData properties changed
            If m_bNetDataChanged Then
                m_lReturn = UpdateNetData()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateNetData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If

            'MSS200901 - Used UW code as this has an extra check....if causes problems will need
            ' to be changed
            ' TF200700 Update Generic Lifestyle Data
            ' Only if PartyLifestyle properties changed
            If m_bPartyLifestyleChanged Then
                'And ONLY if we're not a personal client
                'As PartyPC.Business handles it, and we can't stop it doing it there else
                'we shag the data when called from Client Manager
                If m_sSolutionPartyType = gSIRLibrary.SIRPartyTypePersonalClient Then
                    m_lReturn = UpdateLifestyleData()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateLifestyleData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If
                End If
            End If
            'MSS200901 - Merge End

            'UpdateAddress
            m_lReturn = UpdateAddress()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = RollbackTrans()

                m_oSolutionParty.Dispose()
                Return result
            End If


            '**** START CHANGES - Changed By: AAB  - Changed On: 10-Oct-2002 10:51   ****
            '**** Added this to update contact for AOL
            'UPDATE CONTACTS
            If Informations.IsArray(ContactArray) Then
                'Update Contacts
                m_lReturn = UpdateContacts(ContactArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = RollbackTrans()

                    m_oSolutionParty.Dispose()
                    Return result
                End If
            End If
            '****   END CHANGES - Changed By: AAB  - Changed On: 10-Oct-2002 10:51   ****

            ' Commit Transaction
            m_lReturn = CommitTrans()

            Return result

        Catch excep As System.Exception




            ' Error
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function UpdateContacts(ByVal vContactsArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim Contact_Cnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = BeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Delete the old Contacts

            For i As Integer = vContactsArray.GetLowerBound(1) To vContactsArray.GetUpperBound(1)

                'If the first element is not empty

                If Not Object.Equals(vContactsArray(0, i), Nothing) Then

                    If CStr(vContactsArray(0, i)).Trim() <> "" Then
                        'Get the contact_Cnt based on it being phone or e-mail.

                        sSQL = "SELECT C.contact_cnt" &
                               " FROM Contact C," &
                               " Party_Contact_Usage U," &
                               " Contact_Type T" &
                               " WHERE U.party_cnt = " & CStr(m_lPartyCnt) &
                               " AND C.contact_cnt = U.contact_cnt" &
                               " AND T.contact_type_id = C.contact_type_id" &
                               " AND T.code = '" & CStr(vContactsArray(0, i)).Trim() & "'"

                        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GETEMAILCONTACTCNT", bStoredProcedure:=False, vResultArray:=vResultArray)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        'Set the Contact_cnt  from the array
                        If Informations.IsArray(vResultArray) Then

                            Contact_Cnt = CInt(vResultArray(0, 0))
                        Else
                            'set contact_cnt to Zero
                            Contact_Cnt = 0
                        End If

                        'Delete the contact based on whether it is Phone or E-mail
                        sSQL = "DELETE from party_contact_usage WHERE " &
                               "contact_cnt = " & CStr(Contact_Cnt) & " AND " &
                               "party_cnt = " & CStr(m_lPartyCnt)

                        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="DELPARTYCONS", bStoredProcedure:=False)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = RollbackTrans()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                End If
            Next i

            'Create Business object.
            ' RDC 25102002 removed reference to gPMComponentServices component - it's a BAS module now

            m_oContact = New bsircontact.Business
            m_lReturn = m_oContact.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise the bSIRContact Business", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateContacts", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMError
            End If


            For i As Integer = vContactsArray.GetLowerBound(1) To vContactsArray.GetUpperBound(1)

                If Not Object.Equals(vContactsArray(0, i), Nothing) Then

                    If CStr(vContactsArray(0, i)).Trim() <> "" Then
                        m_lReturn = CreateContact(vType:=vContactsArray(0, i), vAreaCode:=vContactsArray(1, i), vNumber:=vContactsArray(2, i), vExtension:=vContactsArray(3, i), vDescription:=vContactsArray(4, i))
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            m_lReturn = RollbackTrans()

                            m_oContact.Dispose()
                            Return result
                        End If
                    End If
                End If
            Next i

            'Commit Transaction
            m_lReturn = CommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = RollbackTrans()

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateContactsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateContacts", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'EK 22/9/99 New Method
    ' ***************************************************************** '
    ' Name: CreateParty
    '
    ' Component Services object
    ' Description: Create Party using supplied properties.
    '
    ' ***************************************************************** '
    Public Function CreateParty() As Integer
        Dim Catch_Renamed As Boolean = False

        Dim result As Integer = 0
        Const kMethodName As String = "CreateParty"

        Dim bTransStarted As Boolean
        Dim lPartyTypeId As Integer
        Dim vFieldArray As Object

        Try
            Catch_Renamed = True



            result = gPMConstants.PMEReturnCode.PMTrue


            If Not Object.Equals(m_vSourceID, Nothing) Then

                m_iSourceID = CInt(m_vSourceID)
            End If

            bTransStarted = False

            'Start Transaction
            m_lReturn = BeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("BeginTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            bTransStarted = True

            m_sSolutionCode = "SIR"
            m_sSolutionPartyType = m_vPartyType

            'Create instance of solution specific party business class
            If SolutionPartyObjectName <> "" Then
                '  m_oSolutionParty = Activator.CreateInstance(System.Reflection.Assembly.GetAssembly(Type.GetType(SolutionPartyObjectName + "," + SolutionPartyObjectName.Substring(0, SolutionPartyObjectName.LastIndexOf(".")))).FullName, SolutionPartyObjectName).Unwrap()



                result = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oSolutionParty, v_sClassName:=SolutionPartyObjectName, v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    Dim r_sMessage As String = "Failed to create an instance of m_oSolutionParty.Initialise"
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="m_oSolutionParty.Initialise", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If

            'If no specific party business class then use the standard one
            Select Case m_sSolutionPartyType.Substring(0, 2)
                Case "EX", "FE", "DI", "AH", "CO", "CM"
                    ' m_oParty = Activator.CreateInstance(System.Reflection.Assembly.GetAssembly(Type.GetType("b" & m_sSolutionCode & "Party.Business" + "," + ("b" & m_sSolutionCode & "Party.Business").Substring(0, ("b" & m_sSolutionCode & "Party.Business").LastIndexOf(".")))).FullName, "b" & m_sSolutionCode & "Party.Business").Unwrap()

                    result = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oParty, v_sClassName:="b" & m_sSolutionCode & "Party.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
                    If result <> gPMConstants.PMEReturnCode.PMTrue Then
                        Dim r_sMessage As String = "Failed to create an instance of m_oSolutionParty.Initialise"
                        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="m_oSolutionParty.Initialise", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

            End Select

            'Call solution/party specific
            If m_sSolutionCode = gSIRLibrary.SIRCoreSolution Then

                Select Case m_sSolutionPartyType
                    Case gSIRLibrary.SIRPartyTypePersonalClient

                        'Create array and populate with all of the parameters
                        vFieldArray = Array.CreateInstance(GetType(Object), New Integer() {AC_PARTYPC_INDEXUPPER - AC_PARTYPC_INDEXLOWER + 1}, New Integer() {AC_PARTYPC_INDEXLOWER})


                        vFieldArray(AC_PARTYPC_PARTYCNT) = m_lPartyCnt

                        vFieldArray(AC_PARTYPC_PARTYTITLECODE) = m_vPartyTitleCode

                        vFieldArray(AC_PARTYPC_RENEWALSTOPCODEID) = m_vRenewalStopCodeId

                        vFieldArray(AC_PARTYPC_FORENAME) = m_vForeName

                        vFieldArray(AC_PARTYPC_NATIONALITYID) = m_vNationalityId

                        vFieldArray(AC_PARTYPC_INITIALS) = m_vInitials

                        vFieldArray(AC_PARTYPC_PAYMENTMETHODCODE) = m_vPaymentMethodCode

                        vFieldArray(AC_PARTYPC_MARITALSTATUSCODE) = m_vMaritalStatusCode

                        vFieldArray(AC_PARTYPC_NUMBERCHILDREN) = m_vNumberOfChildren

                        vFieldArray(AC_PARTYPC_OCCUPATIONCODE) = m_vOccupationCode

                        vFieldArray(AC_PARTYPC_DATEOFBIRTH) = m_vDateOfBirth

                        vFieldArray(AC_PARTYPC_GENDER) = m_vGenderCode

                        vFieldArray(AC_PARTYPC_SHORTNAME) = m_vShortName

                        vFieldArray(AC_PARTYPC_NAME) = m_vName

                        vFieldArray(AC_PARTYPC_PAYMENTTERMCODE) = m_vPaymentTermCode

                        vFieldArray(AC_PARTYPC_FILECODE) = m_vFileCode

                        vFieldArray(AC_PARTYPC_ABCCOUNT) = m_vABCCount

                        vFieldArray(AC_PARTYPC_STATEMENTS) = m_vStatements

                        vFieldArray(AC_PARTYPC_REMINDERTYPEID) = m_vReminderTypeId

                        vFieldArray(AC_PARTYPC_RENEWALS) = m_vRenewals

                        vFieldArray(AC_PARTYPC_STATUS) = m_vStatus

                        vFieldArray(AC_PARTYPC_CURRENCYID) = m_vCurrencyID

                        vFieldArray(AC_PARTYPC_LASTMODIFIED) = m_vLastModified

                        vFieldArray(AC_PARTYPC_LASTACTIONTYPE) = m_vLastACtionType

                        vFieldArray(AC_PARTYPC_RESOLVED) = m_vResolvedName

                        vFieldArray(AC_PARTYPC_DATECREATED) = m_vDateCreated

                        vFieldArray(AC_PARTYPC_AREAID) = m_vAreaId

                        vFieldArray(AC_PARTYPC_ISPROSPECT) = m_vIsProspect

                        vFieldArray(AC_PARTYPC_LIFESTYLENAME) = m_vResolvedName

                        vFieldArray(AC_PARTYPC_AGENTCNT) = m_vAgentCnt

                        vFieldArray(AC_PARTYPC_CONSULTANTCNT) = m_vConsultantCnt

                        vFieldArray(AC_PARTYPC_INVARIANTKEY) = m_vInvariantKey

                        vFieldArray(AC_PARTYPC_SEASONALGIFTID) = m_vSeasonalGiftID

                        vFieldArray(AC_PARTYPC_CORRESPONDENCETYPEID) = m_vCorrespondenceTypeId

                        vFieldArray(AC_PARTYPC_RENEWALSTOPCODEID) = m_vRenewalStopCodeId

                        vFieldArray(AC_PARTYPC_SWIFTPARTYID) = m_vSwiftPartyId

                        vFieldArray(AC_PARTYPC_LOYALTYNUMBER) = m_vLoyaltyNumber

                        vFieldArray(AC_PARTYPC_ALTERNATIVEIDENTIFIER) = m_vAlternativeIdentifier

                        vFieldArray(AC_PARTYPC_MARKETINGSEGMENTIND) = m_vMarketingSegmentInd

                        vFieldArray(AC_PARTYPC_TRADINGNAME) = m_vTradingName

                        vFieldArray(AC_PARTYPC_SUBBRANCHID) = m_vSubBranchId

                        vFieldArray(AC_PARTYPC_TOBLETTER) = m_vTobLetter

                        vFieldArray(AC_PARTYPC_EMPLOYERBUSINESS) = m_vEmployerBusinessCode

                        vFieldArray(AC_PARTYPC_EMPLOYMENTSTATUSCODE) = m_vEmploymentStatusCode

                        'Add the party using the populated array

                        m_lReturn = m_oSolutionParty.DirectAdd(vFieldArray)

                        'Retrieve the party_cnt

                        m_lPartyCnt = CInt(vFieldArray(AC_PARTYPC_PARTYCNT))

                    Case gSIRLibrary.SIRPartyTypeAgent


                        m_lReturn = m_oSolutionParty.DirectAdd(vPartyCnt:=m_lPartyCnt, vPartyAgentTypeID:=m_vPartyAgentTypeID, vShortname:=m_vShortName, vName:=m_vName, vTradingName:=m_vName, vAgencyAccountNumber:=1, vPaymentTermCode:=m_vPaymentTermCode, vFileCode:=m_vFileCode, vABCCount:=m_vABCCount, vCurrencyId:=m_vCurrencyID, vLastModified:=m_vLastModified, vLAstActionType:=m_vLastACtionType, vResolvedName:=m_vResolvedName, vDateCreated:=m_vDateCreated, vInvariantKey:=m_vInvariantKey)

                    Case gSIRLibrary.SIRPartyTypeAgentGroup


                        m_lReturn = m_oSolutionParty.DirectAdd(vPartyCnt:=m_lPartyCnt, vShortname:=m_vShortName, vName:=m_vName, vGroupActive:=m_vIsGroupActive, vLastModified:=m_vLastModified, vLAstActionType:=m_vLastACtionType, vDateCreated:=m_vDateCreated, vInvariantKey:=m_vInvariantKey)

                    Case gSIRLibrary.SIRPartyTypeGroupClient


                        m_lReturn = m_oSolutionParty.DirectAdd(vPartyCnt:=m_lPartyCnt, vPartyGroupTypeID:=m_vPartyGroupTypeID, vIsRegisteredCharity:=m_vIsRegisteredCharity, vShortname:=m_vShortName, vName:=m_vName, vConsultantCnt:=ToSafeInteger(m_vConsultantCnt), vResolvedName:=m_vResolvedName, vAgentCnt:=ToSafeInteger(m_vAgentCnt), vSeasonalGiftID:=m_vSeasonalGiftID, vCorrespondenceTypeId:=m_vCorrespondenceTypeId, vInvariantKey:=m_vInvariantKey, vRenewalStopCodeId:=m_vRenewalStopCodeId, vSwiftPartyId:=m_vSwiftPartyId, vLoyaltyNumber:=m_vLoyaltyNumber, vAlternativeIdentifier:=m_vAlternativeIdentifier, vMarketingSegmentInd:=m_vMarketingSegmentInd, vTradingName:=m_vTradingName, vSubBranchId:=m_vSubBranchId, vTobLetter:=m_vTobLetter, vTpsInd:=m_vTpsInd, vMailshot:=m_vMailshot, vEmpsInd:=m_vEmpsInd)

                    Case gSIRLibrary.SIRPartyTypeCorporateClient


                        m_lReturn = m_oSolutionParty.DirectAdd(vPartyCnt:=m_lPartyCnt, vPartyBusinessId:=m_vPartyBusinessID, vCompanyReg:=m_vCompanyReg, vTradingSinceDate:=m_vTradingSinceDate, vPaymentMethodCode:=m_vPaymentMethodCode, vShortname:=m_vShortName, vName:=m_vName, vRenewalStopCodeId:=m_vRenewalStopCodeId, vPaymentTermCode:=m_vPaymentTermCode, vFileCode:=m_vFileCode, vABCCount:=m_vABCCount, vStatements:=m_vStatements, vReminderTypeId:=m_vReminderTypeId, vRenewals:=m_vRenewals, vStatus:=m_vStatus, vCurrencyId:=m_vCurrencyID, vLastModified:=m_vLastModified, vLAstActionType:=m_vLastACtionType, vResolvedName:=m_vResolvedName, vDateCreated:=m_vDateCreated, vAreaId:=m_vAreaId, vConsultantCnt:=m_vConsultantCnt, vIsProspect:=m_vIsProspect, vAgentCnt:=m_vAgentCnt, vSeasonalGiftID:=m_vSeasonalGiftID, vCorrespondenceTypeId:=m_vCorrespondenceTypeId, vInvariantKey:=m_vInvariantKey, vRecordStatus:=m_vRecordStatus, vSwiftPartyId:=m_vSwiftPartyId, vLoyaltyNumber:=m_vLoyaltyNumber, vAlternativeIdentifier:=m_vAlternativeIdentifier, vMarketingSegmentInd:=m_vMarketingSegmentInd, vTradingName:=m_vTradingName, vSubBranchId:=m_vSubBranchId, vTobLetter:=m_vTobLetter)

                    Case gSIRLibrary.SIRPartyTypeInsurer


                        m_lReturn = m_oSolutionParty.DirectAdd(vPartyCnt:=m_lPartyCnt, vAgencyNumber:=m_vAgencyNumber, vBinderIndicator:=m_vBinderIndicator, vReportIndicator:=m_vReportIndicator, vIsReinsurer:=0, vReInsuranceType:=0, vIsReInsuranceDebitCreditNo:=0, vDefaultCommRate:=10, vShortname:=m_vShortName, vName:=m_vName, vResolvedName:=m_vName, vCurrencyId:=m_vCurrencyID, vPaymentTermCode:=m_vPaymentTermCode, vFileCode:=m_vFileCode, vABCCount:=m_vABCCount, vABICodeOn406:=m_vABICodeOn406, vABICodeOn81:=m_vABICodeOn81, vABICodeList:=m_vABICodeList, vLastModified:=m_vLastModified, vLAstActionType:=m_vLastACtionType, vDateCreated:=m_vDateCreated, vInvariantKey:=m_vInvariantKey)

                    Case gSIRLibrary.SIRPartyTypeExtra

                        m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupPartyType, v_sCode:=gSIRLibrary.SIRPartyTypeExtra, v_dtEffectiveDate:=DateTime.Now, r_lID:=ToSafeInteger(lPartyTypeId))


                        m_lReturn = m_oParty.DirectAdd(vPartyCnt:=m_lPartyCnt, vPartyTypeID:=ToSafeInteger(lPartyTypeId), vShortname:=m_vShortName, vName:=m_vName, vPaymentTermCode:=m_vPaymentTermCode, vFileCode:=m_vFileCode, vABCCount:=m_vABCCount, vStatements:=m_vStatements, vReminderTypeId:=m_vReminderTypeId, vRenewals:=m_vRenewals, vStatus:=m_vStatus, vCurrencyId:=m_vCurrencyID, vLastModified:=m_vLastModified, vLAstActionType:=m_vLastACtionType, vResolvedName:=m_vResolvedName, vDateCreated:=m_vDateCreated, vAreaId:=m_vAreaId, vIsProspect:=m_vIsProspect, vAgentCnt:=ToSafeInteger(m_vAgentCnt))

                    Case gSIRLibrary.SIRPartyTypeDiscount

                        m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupPartyType, v_sCode:=gSIRLibrary.SIRPartyTypeDiscount, v_dtEffectiveDate:=DateTime.Now, r_lID:=ToSafeInteger(lPartyTypeId))


                        m_lReturn = m_oParty.DirectAdd(vPartyCnt:=m_lPartyCnt, vPartyTypeID:=ToSafeInteger(lPartyTypeId), vShortname:=m_vShortName, vName:=m_vName, vPaymentTermCode:=m_vPaymentTermCode, vFileCode:=m_vFileCode, vABCCount:=m_vABCCount, vStatements:=m_vStatements, vReminderTypeId:=m_vReminderTypeId, vRenewals:=m_vRenewals, vStatus:=m_vStatus, vCurrencyId:=m_vCurrencyID, vLastModified:=m_vLastModified, vLAstActionType:=m_vLastACtionType, vResolvedName:=m_vResolvedName, vDateCreated:=m_vDateCreated, vAreaId:=m_vAreaId, vIsProspect:=m_vIsProspect, vAgentCnt:=ToSafeInteger(m_vAgentCnt))

                    Case gSIRLibrary.SIRPartyTypeFee

                        m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupPartyType, v_sCode:=gSIRLibrary.SIRPartyTypeFee, v_dtEffectiveDate:=DateTime.Now, r_lID:=ToSafeInteger(lPartyTypeId))


                        m_lReturn = m_oParty.DirectAdd(vPartyCnt:=m_lPartyCnt, vPartyTypeID:=ToSafeInteger(lPartyTypeId), vShortname:=m_vShortName, vName:=m_vName, vPaymentTermCode:=m_vPaymentTermCode, vFileCode:=m_vFileCode, vABCCount:=m_vABCCount, vStatements:=m_vStatements, vReminderTypeId:=m_vReminderTypeId, vRenewals:=m_vRenewals, vStatus:=m_vStatus, vCurrencyId:=m_vCurrencyID, vLastModified:=m_vLastModified, vLAstActionType:=m_vLastACtionType, vResolvedName:=m_vResolvedName, vDateCreated:=m_vDateCreated, vAreaId:=m_vAreaId, vIsProspect:=m_vIsProspect, vAgentCnt:=ToSafeInteger(m_vAgentCnt))

                    Case gSIRLibrary.SIRPartyTypeAccountHandler

                        m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupPartyType, v_sCode:=gSIRLibrary.SIRPartyTypeAccountHandler, v_dtEffectiveDate:=DateTime.Now, r_lID:=ToSafeInteger(lPartyTypeId))


                        m_lReturn = m_oParty.DirectAdd(vPartyCnt:=m_lPartyCnt, vPartyTypeID:=ToSafeInteger(lPartyTypeId), vShortname:=m_vShortName, vName:=m_vName, vPaymentTermCode:=m_vPaymentTermCode, vFileCode:=m_vFileCode, vABCCount:=m_vABCCount, vStatements:=m_vStatements, vReminderTypeId:=m_vReminderTypeId, vRenewals:=m_vRenewals, vStatus:=m_vStatus, vCurrencyId:=m_vCurrencyID, vLastModified:=m_vLastModified, vLAstActionType:=m_vLastACtionType, vResolvedName:=m_vResolvedName, vDateCreated:=m_vDateCreated, vAreaId:=m_vAreaId, vIsProspect:=m_vIsProspect, vAgentCnt:=ToSafeInteger(m_vAgentCnt), vInvariantKey:=m_vInvariantKey)


                        m_lPartyCnt = m_oParty.PartyCnt


                        m_oSolutionParty.HandlerType = "AH"


                        m_lReturn = m_oSolutionParty.DirectAdd(vPartyCnt:=m_lPartyCnt, vForeName:=m_vForeName, vInitials:=m_vInitials, vDepartmentId:=m_vDepartmentId, vPartyTitleCode:=m_vPartyTitleCode, vResolvedName:=m_vResolvedName)

                    Case gSIRLibrary.SIRPartyTypeConsultant

                        m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupPartyType, v_sCode:=gSIRLibrary.SIRPartyTypeConsultant, v_dtEffectiveDate:=DateTime.Now, r_lID:=ToSafeInteger(lPartyTypeId))


                        m_lReturn = m_oParty.DirectAdd(vPartyCnt:=m_lPartyCnt, vPartyTypeID:=ToSafeInteger(lPartyTypeId), vShortname:=m_vShortName, vName:=m_vName, vPaymentTermCode:=m_vPaymentTermCode, vFileCode:=m_vFileCode, vABCCount:=m_vABCCount, vStatements:=m_vStatements, vReminderTypeId:=m_vReminderTypeId, vRenewals:=m_vRenewals, vStatus:=m_vStatus, vCurrencyId:=m_vCurrencyID, vLastModified:=m_vLastModified, vLAstActionType:=m_vLastACtionType, vResolvedName:=m_vResolvedName, vDateCreated:=m_vDateCreated, vAreaId:=m_vAreaId, vIsProspect:=m_vIsProspect, vAgentCnt:=ToSafeInteger(m_vAgentCnt))


                        m_lPartyCnt = m_oParty.PartyCnt


                        m_oSolutionParty.HandlerType = "CO"


                        m_lReturn = m_oSolutionParty.DirectAdd(vPartyCnt:=m_lPartyCnt, vForeName:=m_vForeName, vInitials:=m_vInitials, vDepartmentId:=m_vDepartmentId, vPartyTitleCode:=m_vPartyTitleCode, vResolvedName:=m_vResolvedName)

                    Case gSIRLibrary.SIRPartyTypeCommission

                        m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupPartyType, v_sCode:=gSIRLibrary.SIRPartyTypeCommission, v_dtEffectiveDate:=DateTime.Now, r_lID:=ToSafeInteger(lPartyTypeId))


                        m_lReturn = m_oParty.DirectAdd(vPartyCnt:=m_lPartyCnt, vPartyTypeID:=ToSafeInteger(lPartyTypeId), vShortname:=m_vShortName, vName:=m_vName, vResolvedName:=m_vResolvedName, vInvariantKey:=m_vInvariantKey)

                    Case gSIRLibrary.SIRPartyTypeFinanceProvider


                        m_lReturn = m_oSolutionParty.DirectAdd(vPartyCnt:=m_lPartyCnt, vFinanceProviderNumber:=m_vFinanceProviderNumber, vAgencyNumber:=m_vAgencyNumber, vMailboxNumber:=m_vMailboxNumber)

                    Case Else

                        If m_sSolutionPartyType.Substring(0, 2) = gSIRLibrary.SIRPartyTypeOther Then

                            m_lReturn = m_oSolutionParty.DirectAdd(vPartyCnt:=m_lPartyCnt, vPartyTypeID:=m_vPartyTypeID, vDateOfBirth:=m_vDateOfBirth, vGender:=m_vGenderCode, vLicenseTypeId:=m_vLicenseTypeId, vLicenseNumber:=m_vLicenseNumber, vPartyStatus:=m_vPartyStatus, vReg_Number:=m_vRegNumber, vExternalId:=m_vExternalId, vReferenceNumber:=m_vRefNumber, vShortname:=m_vShortName, vName:=m_vName, vCurrencyId:=m_vCurrencyID, vStatus:=m_vStatus, vLastModified:=m_vLastModified, vDateCreated:=m_vDateCreated)
                        End If
                End Select

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oSolutionParty.DirectAdd", "m_sSolutionPartyType = " & m_sSolutionPartyType, gPMConstants.PMELogLevel.PMLogError)
                End If

            Else
                gPMFunctions.RaiseError("Error", "Solution Code '" & m_sSolutionCode & "' is unrecognised", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Create Generic Net Data if NetData properties have changed
            If m_bNetDataChanged Then
                m_lReturn = CreateNetData()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("CreateNetData", "Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            'Create Generic Lifestyle data if PartyLifestyle properties have changed and not a commission account
            If m_bPartyLifestyleChanged And Not (m_sSolutionPartyType = gSIRLibrary.SIRPartyTypeCommission) AndAlso
             m_sSolutionPartyType.Substring(0, 2) <> gSIRLibrary.SIRPartyTypeOther Then
                m_lReturn = CreateLifestyleData()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("CreateLifestyleData", "Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            'Create address object

            m_oAddress = New bSIRAddress.Business
            m_lReturn = m_oAddress.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("gPMComponentServices.CreateBusinessObject", "v_sClassName:=bSIRAddress.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Do not create address and contacts for commission accounts
            If m_sSolutionPartyType <> gSIRLibrary.SIRPartyTypeCommission Then

                m_lReturn = CreateAddress()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("CreateAddress", "Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                If Informations.IsArray(ContactArray) Then

                    'Create contact object

                    m_oContact = New bsircontact.Business
                    m_lReturn = m_oContact.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("gPMComponentServices.CreateBusinessObject", "v_sClassName:=bSIRContact.Business", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    For i As Integer = ContactArray.GetLowerBound(1) To ContactArray.GetUpperBound(1)

                        'Ensure only elements thats have something there are processed

                        If Not Object.Equals(ContactArray(0, i), Nothing) Then

                            m_lReturn = CreateContact(vType:=ContactArray(0, i), vAreaCode:=ContactArray(1, i), vNumber:=ContactArray(2, i), vExtension:=ContactArray(3, i), vDescription:=ContactArray(4, i))

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("CreateContact", "Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If

                        End If

                    Next i

                End If
            End If

            m_lReturn = CommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CommitTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If m_sSolutionPartyType = gSIRLibrary.SIRPartyTypeCorporateClient Or
                m_sSolutionPartyType = gSIRLibrary.SIRPartyTypePersonalClient Or
                m_sSolutionPartyType = gSIRLibrary.SIRPartyTypeGroupClient Then
                'Generate a default Sharepoint folder (if Sharepoint is enabled)

                Dim Sharepoint As bSIRSharepoint.Business
                Sharepoint = New bSIRSharepoint.Business
                Sharepoint.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

                Sharepoint.GenerateDefaultPath(m_lPartyCnt, 0, 0, 0)
            End If

            Return result

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If

            GoTo Finally_Renamed

            If Catch_Renamed Then


                ' DO Not Call any functions before here or the error will be lost
                bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

                ' If you want to rollback a transaction or something, do it here
                If bTransStarted Then
                    m_lReturn = RollbackTrans()
                End If

            End If
Finally_Renamed:
        End Try
    End Function

    ' TF200700
    ' Copied from dSIRPartyNC (Now replaced by SIRPartyNetData)

    Public Function Login() As Integer

        'RJG 09/06/2000 - Calls Login SP and tries to match a UserID and Return a PartyCnt

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase


                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Add the required INPUT parameters
                ' TF200700
                '         m_lReturn& = AddInputLoginParam()
                m_lReturn = .Parameters.Add(sName:="userid", vValue:=UserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add PrimaryKey as OUTPUT parameters
                ' TF200700
                '        m_lReturn& = AddKeyOutputParam()
                m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACLoginSQL, sSQLName:=ACLoginName, bStoredProcedure:=ACLoginStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Get the Party_Cnt for the matching UserID
                ' TF200700
                PartyCnt = .Parameters.Item("party_cnt").Value
                '        m_lReturn& = GetNewPrimaryKeyID()

                '        If (m_lReturn& <> PMTrue) Then
                '            Login = PMFalse
                '            Exit Function
                '        End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Login Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Login", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Create Address from SAM
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateAddress() As Integer
        Dim vResultArray(,) As Object = Nothing
        Dim sSQL As String = ""
        Dim lAddressUsageType As Integer


        'RDT 18/9/2005 - Added CountryCode parameter


        m_lReturn = m_oAddress.DirectAdd(vAddress1:=m_vAddress1, vAddress2:=m_vAddress2,
                                         vAddress3:=m_vAddress3, vAddress4:=m_vAddress4,
                                         vPostalCode:=m_vPostalCode, vCountryID:=m_vCountryId,
                                         sAddress5:=m_sAddress5, sAddress6:=m_sAddress6,
                                         sAddress7:=m_sAddress7, sAddress8:=m_sAddress8,
                                         sAddress9:=m_sAddress9, sAddress10:=m_sAddress10)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        AddressCnt = m_oAddress.AddressCnt

        sSQL = "SELECT address_usage_type_id  FROM address_usage_type WHERE description = 'Correspondence Address' "

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetCorrespondenceId", bStoredProcedure:=False, vResultArray:=vResultArray)

        If Informations.IsArray(vResultArray) Then

            lAddressUsageType = CInt(vResultArray(0, 0))
        Else
            m_lReturn = RollbackTrans()
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        sSQL = "INSERT INTO party_address_usage " &
               "(address_cnt, " &
               "party_cnt, " &
               "address_usage_type_id) " &
               "VALUES (" &
               m_vAddressCnt & ", " &
               m_lPartyCnt & ", " &
                   lAddressUsageType & ")"

        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="AddAddressUsage", bStoredProcedure:=False)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = RollbackTrans()

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return m_lReturn

    End Function

    Private Function CreateContact(ByRef vType As Object, ByRef vAreaCode As Object, ByRef vNumber As Object, ByRef vExtension As Object, ByRef vDescription As Object) As Integer
        'MSS200901 - Added Description for UW.
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim vTypeId As Object
        Dim vResultArray(,) As Object = Nothing
        Dim sSQL As String = ""




        result = gPMConstants.PMEReturnCode.PMTrue



        sSQL = "SELECT Contact_type_id  FROM Contact_type WHERE code = '" & CStr(vType) & "'"

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetContactTypeId", bStoredProcedure:=False, vResultArray:=vResultArray)

        If Informations.IsArray(vResultArray) Then


            vTypeId = vResultArray(0, 0)
        Else
            m_lReturn = RollbackTrans()
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oContact.DirectAdd(vContactTypeID:=vTypeId, vDescription:=vDescription, vAreaCode:=vAreaCode, vNumber:=vNumber, vExtension:=vExtension)
        'MSS200901 - Added description for UW
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If



        ContactCnt = m_oContact.ContactCnt

        'Add Party Contact Usage


        sSQL = "INSERT INTO party_Contact_usage " &
               "(party_cnt, " &
               "contact_cnt) " &
               "VALUES (" &
               m_lPartyCnt & ", " &
               CStr(m_vContactCnt) & ")"

        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="AddContactUsage", bStoredProcedure:=False)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = RollbackTrans()

            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'Add Address Contact Usage


        sSQL = "INSERT INTO Contact_Address_usage " &
               "(contact_cnt, " &
               "address_cnt) " &
               "VALUES (" &
               CStr(m_vContactCnt) & ", " &
               m_vAddressCnt & ")"

        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="AddContactAddressUsage", bStoredProcedure:=False)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = RollbackTrans()

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetAddressDetails
    '
    ' Description: Gets the address details for the partys main address
    ' (ie the correspondence address)
    '
    ' ***************************************************************** '
    Private Function GetAddressDetails() As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim sSQL As String = ""
        Dim lID, lAddressCnt As Integer
        Dim oAddress As bSIRAddress.Business




        result = gPMConstants.PMEReturnCode.PMTrue

        'First get the address_usage_type_id for the
        'main address from lookup table
        m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookUpAddressUsageType, v_sCode:=gSIRLibrary.SIRMainAddressABICode, v_dtEffectiveDate:=DateTime.Now, r_lID:=lID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Get the address cnt for this parties main address
        sSQL = "SELECT address_cnt FROM party_address_usage " &
               "WHERE party_cnt = " & CStr(m_lPartyCnt) & " AND " &
               "address_usage_type_id = " & CStr(lID)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GETAUTID", bStoredProcedure:=False, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(vResultArray) Then
            'Wrong - there should be a main address
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        'save address cnt

        lAddressCnt = CInt(vResultArray(0, 0))

        'MSS200901 - Added from UW
        m_vAddressCnt = lAddressCnt
        'MSS200901 - Merge End

        'Get address business
        'Developer Guide No. 

        oAddress = New bSIRAddress.Business
        m_lReturn = oAddress.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:="bSIRParty.Services", vDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Now get address details

        m_lReturn = oAddress.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)


        oAddress.AddressCnt = lAddressCnt

        'MKW140503 PN4070 1.6.9 to 1.8.6 Catchup START
        'AMH 130503 Added to populate global variable for calling component.
        m_vAddressCnt = lAddressCnt
        'MKW140503 PN4070 1.6.9 to 1.8.6 Catchup END


        m_lReturn = oAddress.GetDetails(vAddressCnt:=lAddressCnt)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'eck130601 return countryId

        m_lReturn = oAddress.GetNext(vAddress1:=m_vAddress1, vAddress2:=m_vAddress2, vAddress3:=m_vAddress3, vAddress4:=m_vAddress4, vPostalCode:=m_vPostalCode, vCountryID:=m_vCountryId)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Done

        oAddress.Dispose()

        oAddress = Nothing


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetNetData
    '
    ' Description: Gets the corresponding Net Data
    '
    ' ***************************************************************** '
    Private Function GetNetData() As Integer

        Dim result As Integer = 0
        Dim oNetData As bSIRPartyNetData.Business




        result = gPMConstants.PMEReturnCode.PMTrue

        'Get Net Data business


        oNetData = New bSIRPartyNetData.Business
        m_lReturn = oNetData.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:="bSIRParty.Services", vDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Now get Net Data
        With oNetData

            ' Set primary key & get details

            .PartyCnt = m_lPartyCnt

            m_lReturn = .GetDetails(vPartyCnt:=m_lPartyCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' TF180700 - OK if no Net Data present
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    Return gPMConstants.PMEReturnCode.PMNotFound
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            m_lReturn = .GetNext(vPassword:=m_vPassword, vMothersMaidenName:=m_vMothersMaidenName, vTpIntroducerCode:=m_vTPIntroducerCode, vTpUserCode:=m_vTPUserCode, vMemorableDate:=m_vMemorableDate, vAQuestion:=m_vAQuestion, vTheAnswer:=m_vTheAnswer, vUserid:=m_vUserID, vCurrentInsRenewalDate:=m_vCurrInsRenewalDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            .Dispose()
        End With

        oNetData = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateNetData
    '
    ' Description: Updates the corresponding Net Data
    '
    ' ***************************************************************** '
    Private Function UpdateNetData() As Integer

        Dim result As Integer = 0
        Dim oNetData As bSIRPartyNetData.Business




        result = gPMConstants.PMEReturnCode.PMTrue

        'Get Net Data business


        oNetData = New bSIRPartyNetData.Business
        m_lReturn = oNetData.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:="bSIRParty.Services", vDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Now get Net Data
        With oNetData

            ' Check if new record needs creating

            ' Set primary key & get details

            .PartyCnt = m_lPartyCnt

            m_lReturn = .GetDetails(vPartyCnt:=m_lPartyCnt)

            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMNotFound
                    ' Add new record

                    m_lReturn = .SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)


                    m_lReturn = .DirectAdd(vPartyCnt:=m_lPartyCnt, vPassword:=m_vPassword, vMothersMaidenName:=m_vMothersMaidenName, vTpIntroducerCode:=m_vTPIntroducerCode, vTpUserCode:=m_vTPUserCode, vMemorableDate:=m_vMemorableDate, vAQuestion:=m_vAQuestion, vTheAnswer:=m_vTheAnswer, vUserid:=m_vUserID, vCurrentInsRenewalDate:=m_vCurrInsRenewalDate)


                    m_lReturn = .SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        .Dispose()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Update record

                    m_lReturn = .SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)


                    m_lReturn = .EditUpdate(lRow:=1, vPassword:=m_vPassword, vMothersMaidenName:=m_vMothersMaidenName, vTpIntroducerCode:=m_vTPIntroducerCode, vTpUserCode:=m_vTPUserCode, vMemorableDate:=m_vMemorableDate, vAQuestion:=m_vAQuestion, vTheAnswer:=m_vTheAnswer)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        .Dispose()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = .Update()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        .Dispose()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case Else
                    ' Problem

                    .Dispose()
                    Return gPMConstants.PMEReturnCode.PMFalse
            End Select


            .Dispose()
        End With

        oNetData = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CreateNetData
    '
    ' Description: Creates a corresponding Net Data record
    '
    ' ***************************************************************** '
    Private Function CreateNetData() As Integer

        Dim result As Integer = 0
        Dim oNetData As bSIRPartyNetData.Business




        result = gPMConstants.PMEReturnCode.PMTrue

        'Get Net Data business


        oNetData = New bSIRPartyNetData.Business
        m_lReturn = oNetData.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:="bSIRParty.Services", vDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Now get Net Data
        With oNetData


            m_lReturn = .SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            ' Add details

            m_lReturn = .DirectAdd(vPartyCnt:=m_lPartyCnt, vPassword:=m_vPassword, vMothersMaidenName:=m_vMothersMaidenName, vTpIntroducerCode:=m_vTPIntroducerCode, vTpUserCode:=m_vTPUserCode, vMemorableDate:=m_vMemorableDate, vAQuestion:=m_vAQuestion, vTheAnswer:=m_vTheAnswer, vUserid:=m_vUserID, vCurrentInsRenewalDate:=m_vCurrInsRenewalDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            .Dispose()
        End With

        oNetData = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetLifestyleData
    '
    ' Description: Gets the corresponding Party Lifestyle data
    '
    ' ***************************************************************** '
    Private Function GetLifestyleData() As Integer

        Dim result As Integer = 0
        Dim oLifestyle As bSIRPartyLifestyle.Business




        result = gPMConstants.PMEReturnCode.PMTrue

        'Get Party Lifestyle business


        oLifestyle = New bSIRPartyLifestyle.Business
        m_lReturn = oLifestyle.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:="bSIRParty.Services", vDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Now get Party Lifestyle Data
        With oLifestyle

            ' Set primary key & get details

            .PartyCnt = m_lPartyCnt

            .PartyLifestyleID = m_vPartyLifestyleID

            ' Temporary fix!

            If String.IsNullOrEmpty(m_vPartyLifestyleID) Then
                m_vPartyLifestyleID = 1
            End If


            m_lReturn = .GetDetails(vPartyCnt:=m_lPartyCnt, vPartyLifestyleID:=m_vPartyLifestyleID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' TF260900 - Found bug in lifestyle.business
                'GetLifestyleData = PMTrue
                'Exit Function

                ' TF180700 - OK if no Party Lifestyle data present
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then

                    Return gPMConstants.PMEReturnCode.PMNotFound
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' TF250700 - Don't get Name as it may be empty
            ' and will overwrite main property
            'm_lReturn = .GetNext( _
            'vName:=m_vName, _
            'vCategory:=m_vCategory, _
            'vOccupationCode:=m_vOccupationCode, _
            'vDateOfBirth:=m_vDateOfBirth, _
            'vGenderCode:=m_vGenderCode, _
            'vSecondaryOccupationCode:=m_vSecondaryOccupationCode, _
            'vIsSmoker:=m_vIsSmoker)


            m_lReturn = .GetNext(vCategory:=m_vCategory, vOccupationCode:=m_vOccupationCode, vDateOfBirth:=m_vDateOfBirth, vGenderCode:=m_vGenderCode, vSecondaryOccupationCode:=m_vSecondaryOccupationCode, vIsSmoker:=m_vIsSmoker)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            .Dispose()
        End With

        oLifestyle = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateLifestyleData
    '
    ' Description: Updates the corresponding Party Lifestyle data
    '
    ' ***************************************************************** '
    Private Function UpdateLifestyleData() As Integer

        Dim result As Integer = 0
        Dim oLifestyle As bSIRPartyLifestyle.Business




        result = gPMConstants.PMEReturnCode.PMTrue

        ' TF250700 - PartyPC already does this
        If m_sSolutionPartyType = gSIRLibrary.SIRPartyTypePersonalClient Then
            Return result
        End If

        'Get Party Lifestyle business


        oLifestyle = New bSIRPartyLifestyle.Business
        m_lReturn = oLifestyle.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:="bSIRParty.Services", vDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Now get Party Lifestyle
        With oLifestyle

            ' Check if new record needs creating

            ' Set primary key & get details

            .PartyCnt = m_lPartyCnt

            .PartyLifestyleID = m_vPartyLifestyleID

            m_lReturn = .GetDetails(vPartyCnt:=m_lPartyCnt, vPartyLifestyleID:=m_vPartyLifestyleID)

            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMNotFound
                    ' Add new record

                    m_lReturn = .SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)


                    m_lReturn = .DirectAdd(vPartyCnt:=m_lPartyCnt, vPartyLifestyleID:=m_vPartyLifestyleID, vName:=m_vName, vCategory:=m_vCategory, vOccupationCode:=m_vOccupationCode, vDateOfBirth:=m_vDateOfBirth, vGenderCode:=m_vGenderCode, vSecondaryOccupationCode:=m_vSecondaryOccupationCode, vIsSmoker:=m_vIsSmoker)


                    m_lReturn = .SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        .Dispose()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Update record

                    m_lReturn = .SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)


                    m_lReturn = .EditUpdate(lRow:=1, vOccupationCode:=m_vOccupationCode, vDateOfBirth:=m_vDateOfBirth, vGenderCode:=m_vGenderCode, vSecondaryOccupationCode:=m_vSecondaryOccupationCode, vIsSmoker:=m_vIsSmoker)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        .Dispose()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = .Update()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        .Dispose()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case Else
                    ' Problem

                    .Dispose()
                    Return gPMConstants.PMEReturnCode.PMFalse
            End Select


            .Dispose()
        End With

        oLifestyle = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CreateLifestyleData
    '
    ' Description: Creates a corresponding Party Lifestyle record
    '
    ' ***************************************************************** '
    Private Function CreateLifestyleData() As Integer

        Dim result As Integer = 0
        Dim oLifestyle As bSIRPartyLifestyle.Business




        result = gPMConstants.PMEReturnCode.PMTrue

        ' TF250700 - PartyPC already does this
        If m_sSolutionPartyType = gSIRLibrary.SIRPartyTypePersonalClient Then
            Return result
        End If

        'Get Party Lifestyle business


        oLifestyle = New bSIRPartyLifestyle.Business
        m_lReturn = oLifestyle.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:="bSIRParty.Services", vDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Now get Party Lifestyle Data
        With oLifestyle


            m_lReturn = .SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            ' Add details

            m_lReturn = .DirectAdd(vPartyCnt:=m_lPartyCnt, vPartyLifestyleID:=m_vPartyLifestyleID, vName:=m_vName, vCategory:=m_vCategory, vOccupationCode:=m_vOccupationCode, vDateOfBirth:=m_vDateOfBirth, vGenderCode:=m_vGenderCode, vSecondaryOccupationCode:=m_vSecondaryOccupationCode, vIsSmoker:=m_vIsSmoker)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            .Dispose()
        End With

        oLifestyle = Nothing

        Return result

    End Function


    Private Function UpdateAddress() As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim aoResultArray(,) As Object = Nothing
        Dim sSQL As String = ""
        Dim nID, nAddressCnt, nNewAddressCnt As Integer
        Dim oAddress As bSIRAddress.Business
        Dim sAddress1 As String = String.Empty
        Dim sAddress2 As String = String.Empty
        Dim sAddress3 As String = String.Empty
        Dim sAddress4 As String = String.Empty
        Dim sPostalCode As String = String.Empty
        Dim sAddress5 As String = ""
        Dim sAddress6 As String = ""
        Dim sAddress7 As String = ""
        Dim sAddress8 As String = ""
        Dim sAddress9 As String = ""
        Dim sAddress10 As String = ""
        Dim nCountryId As Integer

        'First get the address_usage_type_id for the
        'main address from lookup table
        m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookUpAddressUsageType, v_sCode:=gSIRLibrary.SIRMainAddressABICode, v_dtEffectiveDate:=DateTime.Now, r_lID:=nID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Get the address cnt for this parties main address
        sSQL = "SELECT address_cnt FROM party_address_usage " &
               "WHERE party_cnt = " & CStr(m_lPartyCnt) & " AND " &
               "address_usage_type_id = " & CStr(nID)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GETAUTID", bStoredProcedure:=False, vResultArray:=aoResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(aoResultArray) Then
            'Wrong - there should be a main address
            nResult = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Could not find main address for party.", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAddress")

            Return nResult
        End If
        nAddressCnt = CInt(aoResultArray(0, 0))
        'Get address business
        oAddress = New bSIRAddress.Business
        m_lReturn = oAddress.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:="bSIRParty.Services", vDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'First get existing address values
        m_lReturn = oAddress.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
        oAddress.AddressCnt = nAddressCnt
        m_lReturn = oAddress.GetDetails(vAddressCnt:=nAddressCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_lReturn = oAddress.GetNext(vAddress1:=sAddress1, vAddress2:=sAddress2,
                                        vAddress3:=sAddress3, vAddress4:=sAddress4,
                                        vPostalCode:=sPostalCode, vCountryID:=nCountryId,
                                        sAddress5:=sAddress5, sAddress6:=sAddress6,
                                        sAddress7:=sAddress7, sAddress8:=sAddress8,
                                        sAddress9:=sAddress9, sAddress10:=sAddress10)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Check to see if address has changed
        If (sAddress1.Trim() <> m_vAddress1.Trim()) Or
           (sAddress2.Trim() <> m_vAddress2.Trim()) Or
           (sAddress3.Trim() <> m_vAddress3.Trim()) Or
           (sAddress4.Trim() <> m_vAddress4.Trim()) Or
           (sPostalCode.Trim() <> m_vPostalCode.Trim()) Or
           (CStr(nCountryId).Trim() <> m_vCountryId.Trim() Or
            (sAddress5.Trim() <> m_sAddress5.Trim()) Or
            (sAddress6.Trim() <> m_sAddress6.Trim()) Or
            (sAddress7.Trim() <> m_sAddress7.Trim()) Or
            (sAddress8.Trim() <> m_sAddress8.Trim()) Or
            (sAddress9.Trim() <> m_sAddress9.Trim()) Or
            (sAddress10.Trim() <> m_sAddress10.Trim())) Then

            'It has, so check to see if this address is used by other parties
            sSQL = "SELECT party_cnt FROM party_address_usage WHERE " &
                   "party_cnt <> " & CStr(m_lPartyCnt) & " AND " &
                   "address_cnt = " & CStr(nAddressCnt)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GETOTHERADD", bStoredProcedure:=False, vResultArray:=aoResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(aoResultArray) Then
                'No other parties use this address so we'll
                'just update it with the new address details

                m_lReturn = oAddress.EditUpdate(lRow:=1,
                                                  vAddress1:=m_vAddress1,
                                                  vAddress2:=m_vAddress2,
                                                  vAddress3:=m_vAddress3,
                                                  vAddress4:=m_vAddress4,
                                                  vPostalCode:=m_vPostalCode, vCountryID:=m_vCountryId,
                                                  sAddress5:=m_sAddress5,
                                                  sAddress6:=m_sAddress6,
                                                  sAddress7:=m_sAddress7,
                                                  sAddress8:=m_sAddress8,
                                                  sAddress9:=m_sAddress9,
                                                  sAddress10:=m_sAddress10)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = oAddress.Update()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                'Other parties use this address so we'll
                'add a new address with the new details and
                'update this parties address_usage to the new
                'one

                're-initialise the address object as we are
                'going to do an add

                m_lReturn = oAddress.Initialise(sCallingAppName:="bSIRParty.Services", sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, vDatabase:=m_oDatabase)
                'First get existing address values

                m_lReturn = oAddress.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                oAddress.AddressCnt = 0
                m_lReturn = oAddress.EditAdd(lRow:=1, vAddress1:=m_vAddress1, vAddress2:=m_vAddress2, vAddress3:=m_vAddress3, vAddress4:=m_vAddress4, vPostalCode:=m_vPostalCode, vCountryID:=m_vCountryId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = oAddress.Update()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                nNewAddressCnt = oAddress.AddressCnt

                'update the party_address usage
                sSQL = "UPDATE party_address_usage SET " &
                       "address_cnt = " & CStr(nNewAddressCnt) & " WHERE " &
                       "party_cnt = " & CStr(m_lPartyCnt) & " AND " &
                       "address_cnt = " & CStr(nAddressCnt)

                m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="UPDATEPARTYADD", bStoredProcedure:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

        End If
        oAddress.Dispose()
        oAddress = Nothing
        Return nResult
    End Function


    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the variable passed.
    '
    ' ***************************************************************** '
    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef vTableArray(,) As Object, ByRef vResultArray(,) As Object, ByRef vResult As Object) As Integer

        Dim result As Integer = 0
        Dim m_vLookupValues, m_vLookupDetails As Object

        Dim lRow As Integer
        Dim bFoundMatch As Boolean

        ' Lookup value contants.
        Const ACValueTableName As Integer = 0
        'Const ACValueID As Integer = 1  ''Unused Variable by Rachana
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3

        ' Lookup detail contants.
        'Const ACDetailKey As Integer = 0    ''Unused Variable by Rachana
        Const ACDetailDesc As Integer = 1



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the standard parameters to local variables

        m_vLookupValues = vTableArray

        m_vLookupDetails = vResultArray

        ' Get the lookup values.

        bFoundMatch = False

        If m_vLookupValues Is Nothing Then
            Return result

        Else

            For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
                ' Check for a match of the table name.

                If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
                    ' Found a match
                    bFoundMatch = True
                    Exit For
                End If
            Next lRow

            ' Check if there has been a table match.
            If Not bFoundMatch Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")

                Return result
            End If
        End If
        ' Using the lookup values, populate the result parameter with
        ' the details from the lookup details array.




        For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)


            vResult = m_vLookupDetails(ACDetailDesc, lCntr)
        Next lCntr

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetLookUpID
    '
    ' Description: Return the look up id for a given lookup table and
    ' lookup description.
    '
    ' ***************************************************************** '
    Private Function GetLookUpID(ByRef sTable As String, ByRef sField As String, ByRef sDescription As String, ByRef sIDColumnName As String, ByRef lIDValue As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing




        result = gPMConstants.PMEReturnCode.PMTrue

        sSQL = "SELECT " & sIDColumnName & " FROM " & sTable &
               " WHERE " & sField.Trim() & " = '" & sDescription.Trim() & "'"

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GETLOOKUPID", bStoredProcedure:=False, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'should have got a match else log error
        If Not Informations.IsArray(vResultArray) Then
            result = gPMConstants.PMEReturnCode.PMFalse

            If sDescription.Trim() <> "" Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Lookup match." & Strings.ChrW(10).ToString() &
                                   "Table = '" & sTable & "'" & Strings.ChrW(10).ToString() &
                                   "Description='" & sDescription & "'", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookUpID", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            End If

            Return result
        End If

        'Return the value

        lIDValue = CInt(vResultArray(0, 0))

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetSolutionParty
    '
    ' Description: Ensure SolutionCode has been provided & create object.
    '
    ' ***************************************************************** '
    Private Function GetSolutionParty() As Integer

        Dim result As Integer = 0
        Dim sTmp As String = ""
        Dim vResultArray(,) As Object
        Dim bFound As Boolean
        Dim sSQL As String = ""




        result = gPMConstants.PMEReturnCode.PMTrue

        If Not (m_oSolutionParty Is Nothing) Then
            Return result
        End If

        ' Ensure StructureID is set
        m_lReturn = GetStructureID()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Determine what solution is

        'Developer Guide No. 17
        vResultArray = Nothing
        Dim vTabArray(3, 0) As Object


        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = "party_structure"

        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = m_vPartyStructureID

        ' Get the Lookup items
        m_lReturn = m_oLookup.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=vTabArray, iLanguageID:=m_iLanguageID, dtEffectiveDate:=DateTime.Now, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Get the solution code

        m_sSolutionCode = CStr(vResultArray(2, 0)).Trim()
        'ECK 02/08/99 PMB use Core Business components
        If m_sSolutionCode = "PMB" Then
            m_sSolutionCode = "SIR"
        End If

        'Now get party type, depending on solution
        bFound = False
        m_sSolutionPartyType = ""

        Select Case m_sSolutionCode
            Case gSIRLibrary.SIRCoreSolution
                'ECK 02/08/99 Added Insurers to the list
                'What type of party/client is it
                '            sSQL$ = "SELECT party_cnt, '" & SIRPartyTypePersonalClient & _
                ''                    "' FROM party_personal_client WHERE party_cnt = " & m_lPartyCnt & _
                ''                    "UNION SELECT party_cnt, '" & SIRPartyTypeAgent & _
                ''                    "' FROM party_agent WHERE party_cnt = " & m_lPartyCnt & _
                ''                    "UNION SELECT party_cnt, '" & SIRPartyTypeCorporateClient & _
                ''                    "' FROM party_corporate_client WHERE party_cnt = " & m_lPartyCnt & _
                ''                    "UNION SELECT party_cnt, '" & SIRPartyTypeGroupClient & _
                ''                    "' FROM party_group_client WHERE party_cnt = " & m_lPartyCnt & _
                ''                    "UNION SELECT party_cnt, '" & SIRPartyTypeInsurer & _
                ''                    "' FROM party_insurer WHERE party_cnt = " & m_lPartyCnt
                'EK 27/9/99 Party Type now held on the party
                sSQL = "SELECT code FROM party_type T, party P WHERE T.party_type_id = P.party_type_id AND P.party_cnt = " & m_lPartyCnt
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="ISPC", bStoredProcedure:=False, vResultArray:=vResultArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not Informations.IsArray(vResultArray) Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unknown Party Type", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSolutionParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If


                m_sSolutionPartyType = CStr(vResultArray(0, 0)).Trim()
                'PN13636
                PartyType = m_sSolutionPartyType

                'case "<Solution Code>"
                'case "<Solution Code>" etc ie add in other solutions here

            Case Else
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to determine solution", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSolutionParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result

        End Select

        'should have a party type now
        If m_sSolutionPartyType = "" Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to determine party type (ie PC, GC, CC, Agent etc)" &
                               Strings.ChrW(10).ToString() & "Solution = " & m_sSolutionCode, vApp:=ACApp, vClass:=ACClass, vMethod:="GetSolutionParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Create instance of Solution Specific Party Business class
        'EK 27/9/99
        'EK 27/1/00 Add Commission Account
        'MSS200901 - Added from UW. Commented out SFORB
        'RWH(23/07/01) Only check left 2 chars to cater for Other Party Types.
        Select Case m_sSolutionPartyType.Substring(0, 2)
            'Select Case m_sSolutionPartyType
            'sj 17/08/2002 - start
            'Case "EX", "FE", "DI", "AH", "CO", "CM", "OT"
            'PN18406 Add Executive Handler Code
            Case "EX", "FE", "DI", "AH", "CO", "CM", "HC"
                'sj 17/08/2002 - end
                'm_oSolutionParty = Activator.CreateInstance(System.Reflection.Assembly.GetAssembly(Type.GetType("b" & m_sSolutionCode & "Party.Business" + "," + ("b" & m_sSolutionCode & "Party.Business").Substring(0, ("b" & m_sSolutionCode & "Party.Business").LastIndexOf(".")))).FullName, "b" & m_sSolutionCode & "Party.Business").Unwrap()

                result = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oSolutionParty, v_sClassName:="b" & m_sSolutionCode & "Party.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    Dim r_sMessage As String = "Failed to create an instance of m_oSolutionParty.Initialise"
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="m_oSolutionParty.Initialise", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                '    Case "CO"
                '        Set m_oSolutionParty = CreateObject("b" & m_sSolutionCode & "PartyAH.Business")
            Case Else
                ' RAW 21/02/2003 : ISS2379 : replaced with call to SolutionPartyObjectName
                '        Set m_oSolutionParty = CreateObject("b" & m_sSolutionCode & "Party" & Left$(m_sSolutionPartyType, 2) & ".Business")
                If SolutionPartyObjectName <> "" Then
                    'm_oSolutionParty = Activator.CreateInstance(System.Reflection.Assembly.GetAssembly(Type.GetType(SolutionPartyObjectName + "," + SolutionPartyObjectName.Substring(0, SolutionPartyObjectName.LastIndexOf(".")))).FullName, SolutionPartyObjectName).Unwrap()
                    result = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oSolutionParty, v_sClassName:=SolutionPartyObjectName, v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
                    If result <> gPMConstants.PMEReturnCode.PMTrue Then
                        Dim r_sMessage As String = "Failed to create an instance of m_oSolutionParty.Initialise"
                        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="bSirpartyServices.GetSolutionParty", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to determine SolutionPartyObjectName for party type " & m_sSolutionPartyType, vApp:=ACApp, vClass:=ACClass, vMethod:="GetSolutionParty")
                    Return result
                End If
                ' RAW 21/02/2003 : ISS2379 : end
        End Select

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetIdentity
    '
    ' Description: Get Party from supplied value or IDs.
    '
    ' ***************************************************************** '
    Private Function GetIdentity() As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        If m_lPartyCnt < 1 Then

            If (Object.Equals(m_vSourceID, Nothing)) Or (Object.Equals(m_vPartyID, Nothing)) Then

                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Insufficient Identity Supplied", vApp:=ACApp, vClass:=ACClass, vMethod:="GetIdentity ", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            Else
                With m_oDatabase

                    sSQL = "SELECT party_cnt, party_structure_id FROM party"

                    sSQL = sSQL & " WHERE source_id=" & CStr(m_vSourceID)

                    sSQL = sSQL & " AND party_id=" & CStr(m_vPartyID)

                    m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GETPARTYCNT", bStoredProcedure:=False)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Developer Guide 111
                    m_lPartyCnt = gPMFunctions.NullToLong(.Records.Item(0).Fields()("party_cnt"))
                    m_vPartyStructureID = gPMFunctions.NullToLong(.Records.Item(0).Fields()("party_structure_id"))
                End With
            End If
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetStructureID
    '
    ' Description: Get PartyStructureID for current partyCnt
    '
    ' ***************************************************************** '
    Private Function GetStructureID() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' exit if already set
        If m_vPartyStructureID >= 1 Then
            Return result
        End If

        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(m_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set parameter 'party_cnt' =" & m_lPartyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetStructureID", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            m_lReturn = .SQLSelect(sSQL:=ACGetPartyStructureIDSQL, sSQLName:=ACGetPartyStructureIDName, bStoredProcedure:=ACGetPartyStructureIDStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process " & ACGetPartyStructureIDSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="GetStructureID", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            If .Records.Count() < 1 Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Party Structure for party_cnt" & m_lPartyCnt & " not found.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStructureID", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            'Developer Guide No 111
            m_vPartyStructureID = gPMFunctions.NullToLong(.Records.Item(0).Fields()("party_structure_id"))

        End With

        Return result

    End Function
End Class

