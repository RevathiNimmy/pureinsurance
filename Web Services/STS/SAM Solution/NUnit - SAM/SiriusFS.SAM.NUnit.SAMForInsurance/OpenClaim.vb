Imports NUnit.Framework

<TestFixture()> _
Public Class OpenClaim
    Inherits BaseTest
    Private Const kInvalidLookupValue As String = "000GARBAGE"
    Private Const kInvalidLookupFieldMessage As String = "Invalid Lookup Field Name Returned"
    Private Const kIncorrectMissingMandatoryFieldMessage As String = "Incorrect Missing Mandatory Field Returned"
    Private Const kInvalidCodeReturned As String = "Invalid Error Code Returned"
    Private Const kInvalidDescriptionReturned As String = "Invalid Error Description Returned"
    Private Const kInvalidLookupValueMessage As String = "Invalid Lookup Value Returned"






#Region "Private Declarations"

    Private m_oTestData As New TestData
    Private m_lBaseClaimId As Integer
    Private m_lClaimId As Integer
    Private m_sBranchCode As String
    Private m_TimeStamp As Byte()

    Private Enum TestCaseScenario

        ' Non Error Scenarios
        None
        AllNonMandatoryFieldsMissing
        NoClient
        NoInsurer
        NoClaimPeril
        NoContacts
        NoReserves
        NoRecoveries
        InfoOnlyClaimNoClaimPeril

        ' Busines Error scenarios : - some of these cannot be tested
        BusinessErrorDuplicateClaimExists
        BusinessErrorLossFromDataInFuture
        BusinessErrorLossFromDateAfterReportedDate
        BusinessErrorLossFromDateAfterLossToDate
        BusinessErrorReportedDateInFuture
        BusinessErrorPolicyDataMissing
        BusinessErrorNumberingSchemeNotFound
        BusinessErrorProductsNumberingSchemesNotFound
        BusinessErrorAutoClaimNumberingDisabled
        BusinessErrorAgentRecordNotFound
        BusinessErrorInsuranceFileDetailsNotFound
        BusinessErrorAutoReinsuranceFailed
        BusinessErrorTransactionTypeNotFound
        BusinessErrorAccountsProcessingFailed
        BusinessErrorDMEFolderCreationFailed

        ' Busines Warning scenarios
        BusinessWarningInfoOnlyClaimDataTruncated
        BusinessWarningLossFromDateAfterPolicyEndDate
        BusinessWarningLossFromDateBeforePolicyStartDate
        BusinessWarningRiskIsDeferred
        BusinessWarningPolicyIsVoid
        BusinessWarningPolicyIsDifferent

        ' Missing Data
        MissingDataBranchCode
        MissingDataClientAddressLine1
        MissingDataClientCountryCode
        'MissingDataClientContactDetailItem
        MissingDataClientTaxRegistrationNumber
        MissingDataInsurerAddressLine1
        MissingDataInsurerCountryCode
        MissingDataInsurerContactDetailItem
        MissingDataDescription
        MissingDataProgressStatusCode
        MissingDataPrimaryCauseCode
        MissingDataHandlerCode
        MissingDataInsuranceFileKey
        MissingDataRiskKey
        MissingDataClaimPerilTypeCode
        MissingDataClaimPerilReserveTypeCode
        MissingDataClaimPerilRecoveryTypeCode

        ' Invalid Lookups
        InvalidLookupBranchCode
        InvalidLookupClientCountryCode
        InvalidLookupInsurerCountryCode
        InvalidLookupProgressStatusCode
        InvalidLookupPrimaryCauseCode
        InvalidLookupHandlerCode
        InvalidLookupSecondaryCauseCode
        InvalidLookupCatastropheCode
        InvalidLookupTownCode
        InvalidLookupUserDefFldACode
        InvalidLookupUserDefFldBCode
        InvalidLookupUserDefFldCCode
        InvalidLookupUserDefFldDCode
        InvalidLookupUserDefFldECode
        InvalidLookupPerilTypeCode
        InvalidLookupReserveTypeCode
        InvalidLookupRecoveryTypeCode
        InvalidLookupCurrencyCode
        InvalidLookupUnderwritingYearCode

    End Enum

#End Region

#Region "Call ProxyWS Method"

    Private Sub OpenClaimTest( _
        Optional ByVal TestCase As TestCaseScenario = TestCaseScenario.None, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.OpenClaimRequestType
        Dim oResponse As ProxyWS.OpenClaimResponseType
        Dim oClaimRequest As New ProxyWS.BaseClaimOpenRequestType
        Dim oClaim As New ProxyWS.BaseClaimOpenType
        Dim oClaimPeril() As ProxyWS.BaseClaimPerilType = Nothing
        Dim oRecovery() As ProxyWS.BaseClaimPerilRecoveryType = Nothing
        Dim oReserve() As ProxyWS.BaseClaimPerilReserveType = Nothing
        Dim oClient As ProxyWS.BaseClaimPartyClientType = Nothing
        Dim oInsurer As ProxyWS.BaseClaimPartyInsurerType = Nothing
        Dim oClientContact(0 To 4) As ProxyWS.BaseContactType
        Dim oInsurerContact(0 To 4) As ProxyWS.BaseContactType
        Dim oContactDetail As New ProxyWS.BaseContactDetailType

        Dim oTDClaim As OpenClaimXMLStructure.cClaim = Nothing
        oTDClaim = m_oTestData.OpenClaim.Claim

        Try
            With oRequest

                .BranchCode = oTDClaim.BranchCode
                m_sBranchCode = oTDClaim.BranchCode

                '***************** CLIENT CONTACTS  ****************************
                Array.Resize(oClientContact, oTDClaim.Client.Contacts.Count)
                Dim iClientContact As Integer = 0
                For Each oCContact As OpenClaimXMLStructure.cContact In oTDClaim.Client.Contacts
                    oContactDetail = New ProxyWS.BaseContactDetailType
                    oContactDetail.Item = oCContact.ContactDetail
                    If oCContact.ContactTypeCode = ProxyWS.ContactTypeType.EMAIL Then
                        oContactDetail.ItemElementName = ProxyWS.ItemChoiceType.EmailAddress
                    Else
                        oContactDetail.ItemElementName = ProxyWS.ItemChoiceType.Number
                    End If
                    oClientContact(iClientContact) = New ProxyWS.BaseContactType
                    oClientContact(iClientContact).AreaCode = oCContact.AreaCode
                    oClientContact(iClientContact).ContactDetail = oContactDetail
                    oClientContact(iClientContact).ContactTypeCode = oCContact.ContactTypeCode
                    iClientContact += 1
                Next

                '***************** CLIENT ADDRESS  ******************************
                Dim oClientAddress As ProxyWS.BaseAddressType = New ProxyWS.BaseAddressType
                oClientAddress.AddressLine1 = oTDClaim.Client.Addressline1
                oClientAddress.AddressLine2 = oTDClaim.Client.Addressline2
                oClientAddress.AddressLine3 = oTDClaim.Client.Addressline3
                oClientAddress.AddressLine4 = oTDClaim.Client.Addressline4
                oClientAddress.AddressTypeCode = oTDClaim.Client.AddressTypeCode
                oClientAddress.CountryCode = oTDClaim.Client.CountryCode
                oClientAddress.PostCode = oTDClaim.Client.PostCode

                '***************** CLIENT ***************************************
                oClient = New ProxyWS.BaseClaimPartyClientType
                oClient.Address = oClientAddress
                oClient.Contact = oClientContact
                oClient.PartyClaimNumber = oTDClaim.Client.ClaimNumber
                oClient.TaxRegistered = oTDClaim.Client.VatRegistered
                oClient.TaxRegistrationNumber = oTDClaim.Client.VatRegNumber
                oClaim.Client = oClient
                'oClaim.Client = Nothing

                '***************** INSURER CONTACTS  ****************************
                Array.Resize(oInsurerContact, oTDClaim.Insurer.Contacts.Count)
                Dim iInsurerContact As Integer = 0
                For Each oCContact As OpenClaimXMLStructure.cContact In oTDClaim.Insurer.Contacts
                    oContactDetail = New ProxyWS.BaseContactDetailType
                    oContactDetail.Item = oCContact.ContactDetail
                    If oCContact.ContactTypeCode = ProxyWS.ContactTypeType.EMAIL Then
                        oContactDetail.ItemElementName = ProxyWS.ItemChoiceType.EmailAddress
                    Else
                        oContactDetail.ItemElementName = ProxyWS.ItemChoiceType.Number
                    End If
                    oInsurerContact(iInsurerContact) = New ProxyWS.BaseContactType
                    oInsurerContact(iInsurerContact).AreaCode = oCContact.AreaCode
                    oInsurerContact(iInsurerContact).ContactDetail = oContactDetail
                    oInsurerContact(iInsurerContact).ContactTypeCode = oCContact.ContactTypeCode

                    iInsurerContact += 1
                Next

                '***************** INSURER ADDRESS  ******************************
                Dim oInsurerAddress As ProxyWS.BaseAddressType = New ProxyWS.BaseAddressType
                oInsurerAddress.AddressLine1 = oTDClaim.Insurer.Addressline1
                oInsurerAddress.AddressLine2 = oTDClaim.Insurer.Addressline2
                oInsurerAddress.AddressLine3 = oTDClaim.Insurer.Addressline3
                oInsurerAddress.AddressLine4 = oTDClaim.Insurer.Addressline4
                oInsurerAddress.AddressTypeCode = oTDClaim.Insurer.AddressTypeCode
                oInsurerAddress.CountryCode = oTDClaim.Insurer.CountryCode
                oInsurerAddress.PostCode = oTDClaim.Insurer.PostCode

                '***************** INSURER **************************************
                oInsurer = New ProxyWS.BaseClaimPartyInsurerType
                oInsurer.Address = oInsurerAddress
                oInsurer.Contact = oInsurerContact
                oInsurer.PartyClaimNumber = oTDClaim.Insurer.ClaimNumber
                oInsurer.ContactName = oTDClaim.Insurer.ContactName
                oClaim.Insurer = oInsurer

                '***************** CLAIM PERIL **************************************
                Array.Resize(oClaimPeril, oTDClaim.ClaimPerils.Count)
                Dim iClaimPeril As Integer = 0
                For Each oCClaimPeril As OpenClaimXMLStructure.cClaimPeril In oTDClaim.ClaimPerils

                    '***************** RECOVERY **************************************
                    Dim iRecovery As Integer = 0
                    Array.Resize(oRecovery, oCClaimPeril.Recovery.Count)
                    For Each oCRecovery As OpenClaimXMLStructure.cRecovery In oCClaimPeril.Recovery
                        oRecovery(iRecovery) = New ProxyWS.BaseClaimPerilRecoveryType
                        oRecovery(iRecovery).RevisionAmount = oCRecovery.RevisionAmount
                        oRecovery(iRecovery).TypeCode = oCRecovery.TypeCode
                        iRecovery += 1
                    Next

                    '***************** RESERVES **************************************
                    Dim iReserve As Integer = 0
                    Array.Resize(oReserve, oCClaimPeril.Reserve.Count)
                    For Each oCReserve As OpenClaimXMLStructure.cReserve In oCClaimPeril.Reserve
                        oReserve(iReserve) = New ProxyWS.BaseClaimPerilReserveType
                        oReserve(iReserve).RevisionAmount = oCReserve.RevisionAmount
                        oReserve(iReserve).TypeCode = oCReserve.TypeCode
                        iReserve += 1
                    Next

                    '***************** CLAIM PERIL DETAILS ****************************
                    oClaimPeril(iClaimPeril) = New ProxyWS.BaseClaimPerilType
                    oClaimPeril(iClaimPeril).Recovery = oRecovery
                    oClaimPeril(iClaimPeril).Reserve = oReserve
                    oClaimPeril(iClaimPeril).TypeCode = oCClaimPeril.TypeCode
                    oClaimPeril(iClaimPeril).Description = oCClaimPeril.Description

                    iClaimPeril += 1

                Next

                '***************** CLAIM **********************************************
                oClaim.ClaimPeril = oClaimPeril
                oClaim.CatastropheCode = oTDClaim.CatastropheCode
                oClaim.ClaimVersionDescription = oTDClaim.ClaimVersionDescription
                oClaim.Comments = oTDClaim.Comments
                oClaim.CurrencyCode = oTDClaim.CurrencyCode
                oClaim.Description = oTDClaim.Description
                oClaim.HandlerCode = oTDClaim.HandlerCode
                oClaim.InfoOnly = oTDClaim.InfoOnly
                oClaim.InsuranceFileKey = oTDClaim.InsurancefileKey
                oClaim.LikelyClaim = oTDClaim.LikelyClaim
                oClaim.Location = oTDClaim.Location
                oClaim.LossFromDate = oTDClaim.LossFromDate
                oClaim.LossToDate = oTDClaim.LossToDate
                oClaim.LossToDateSpecified = True
                oClaim.PrimaryCauseCode = oTDClaim.PrimaryCauseCode
                oClaim.ProgressStatusCode = oTDClaim.ProgressStatusCode
                oClaim.ReportedDate = oTDClaim.ReportedDate
                oClaim.RiskKey = oTDClaim.RiskKey
                oClaim.SecondaryCauseCode = oTDClaim.SecondaryCauseCode
                oClaim.TownCode = oTDClaim.TownCode
                oClaim.UnderwritingYearCode = oTDClaim.UnderwritingYearCode
                oClaim.UserDefFldACode = oTDClaim.UserDefFldACode
                oClaim.UserDefFldBCode = oTDClaim.UserDefFldBCode
                oClaim.UserDefFldCCode = oTDClaim.UserDefFldCCode
                oClaim.UserDefFldDCode = oTDClaim.UserDefFldDCode
                oClaim.UserDefFldECode = oTDClaim.UserDefFldECode
                oClaim.IgnoreWarnings = oTDClaim.IgnoreWarnings

                .Claim = oClaim

            End With

            ' reset fields based on selected test case
            ProcessTestCases(oRequest, TestCase)

            SetWSETestCaseScenario(nWSETestCaseScenario)

            oResponse = oProxy.OpenClaim(oRequest)

            With oResponse

                ' all these test cases should work without error
                If TestCase = TestCaseScenario.None OrElse _
                    TestCase = TestCaseScenario.AllNonMandatoryFieldsMissing OrElse _
                    TestCase = TestCaseScenario.NoContacts OrElse _
                    TestCase = TestCaseScenario.NoClient OrElse _
                    TestCase = TestCaseScenario.NoInsurer OrElse _
                    TestCase = TestCaseScenario.NoClaimPeril OrElse _
                    TestCase = TestCaseScenario.NoReserves OrElse _
                    TestCase = TestCaseScenario.NoRecoveries OrElse _
                    TestCase = TestCaseScenario.InfoOnlyClaimNoClaimPeril Then

                    SAMTest.AssertCallSucceeded(oResponse)

                    m_lBaseClaimId = oResponse.BaseClaimKey
                    m_lClaimId = oResponse.ClaimKey
                    m_TimeStamp = oResponse.TimeStamp

                ElseIf TestCase = TestCaseScenario.InvalidLookupBranchCode OrElse _
                     TestCase = TestCaseScenario.InvalidLookupCatastropheCode OrElse _
                     TestCase = TestCaseScenario.InvalidLookupClientCountryCode OrElse _
                     TestCase = TestCaseScenario.InvalidLookupCurrencyCode OrElse _
                     TestCase = TestCaseScenario.InvalidLookupHandlerCode OrElse _
                     TestCase = TestCaseScenario.InvalidLookupInsurerCountryCode OrElse _
                     TestCase = TestCaseScenario.InvalidLookupPerilTypeCode OrElse _
                     TestCase = TestCaseScenario.InvalidLookupPrimaryCauseCode OrElse _
                     TestCase = TestCaseScenario.InvalidLookupProgressStatusCode OrElse _
                     TestCase = TestCaseScenario.InvalidLookupRecoveryTypeCode OrElse _
                     TestCase = TestCaseScenario.InvalidLookupReserveTypeCode OrElse _
                     TestCase = TestCaseScenario.InvalidLookupSecondaryCauseCode OrElse _
                     TestCase = TestCaseScenario.InvalidLookupTownCode OrElse _
                     TestCase = TestCaseScenario.InvalidLookupUnderwritingYearCode OrElse _
                     TestCase = TestCaseScenario.InvalidLookupUserDefFldACode OrElse _
                     TestCase = TestCaseScenario.InvalidLookupUserDefFldBCode OrElse _
                     TestCase = TestCaseScenario.InvalidLookupUserDefFldCCode OrElse _
                     TestCase = TestCaseScenario.InvalidLookupUserDefFldDCode OrElse _
                     TestCase = TestCaseScenario.InvalidLookupUserDefFldECode Then

                    ProcessInvalidLookup(oResponse, TestCase)

                ElseIf TestCase = TestCaseScenario.MissingDataBranchCode OrElse _
                    TestCase = TestCaseScenario.MissingDataClientAddressLine1 OrElse _
                    TestCase = TestCaseScenario.MissingDataClientCountryCode OrElse _
                    TestCase = TestCaseScenario.MissingDataClientTaxRegistrationNumber OrElse _
                    TestCase = TestCaseScenario.MissingDataInsurerAddressLine1 OrElse _
                    TestCase = TestCaseScenario.MissingDataInsurerCountryCode OrElse _
                    TestCase = TestCaseScenario.MissingDataInsurerContactDetailItem OrElse _
                    TestCase = TestCaseScenario.MissingDataDescription OrElse _
                    TestCase = TestCaseScenario.MissingDataProgressStatusCode OrElse _
                    TestCase = TestCaseScenario.MissingDataPrimaryCauseCode OrElse _
                    TestCase = TestCaseScenario.MissingDataHandlerCode OrElse _
                    TestCase = TestCaseScenario.MissingDataInsuranceFileKey OrElse _
                    TestCase = TestCaseScenario.MissingDataRiskKey OrElse _
                    TestCase = TestCaseScenario.MissingDataClaimPerilTypeCode OrElse _
                    TestCase = TestCaseScenario.MissingDataClaimPerilReserveTypeCode OrElse _
                    TestCase = TestCaseScenario.MissingDataClaimPerilRecoveryTypeCode Then

                    ProcessMissingData(oResponse, TestCase)

                ElseIf TestCase = TestCaseScenario.BusinessErrorDuplicateClaimExists OrElse _
                    TestCase = TestCaseScenario.BusinessErrorLossFromDataInFuture OrElse _
                    TestCase = TestCaseScenario.BusinessErrorLossFromDateAfterReportedDate OrElse _
                    TestCase = TestCaseScenario.BusinessErrorLossFromDateAfterLossToDate OrElse _
                    TestCase = TestCaseScenario.BusinessErrorReportedDateInFuture Then

                    ProcessBusinessErrors(oResponse, TestCase)

                ElseIf TestCase = TestCaseScenario.BusinessWarningInfoOnlyClaimDataTruncated OrElse _
                    TestCase = TestCaseScenario.BusinessWarningLossFromDateAfterPolicyEndDate OrElse _
                    TestCase = TestCaseScenario.BusinessWarningLossFromDateBeforePolicyStartDate OrElse _
                    TestCase = TestCaseScenario.BusinessWarningRiskIsDeferred OrElse _
                    TestCase = TestCaseScenario.BusinessWarningPolicyIsVoid OrElse _
                    TestCase = TestCaseScenario.BusinessWarningPolicyIsDifferent Then

                    ProcessBusinessWarnings(oResponse, TestCase)

                End If

            End With

        Catch ex As AssertionException
            Throw
        Catch ex As SoapException
            WSETest.HandleException(ex, nWSETestCaseScenario)
        Catch ex As Exception
            WSETest.HandleException(ex)
        Finally
            oRequest = Nothing
            oResponse = Nothing
        End Try

    End Sub

#End Region

    Public Sub SupportMethod(ByRef r_lBaseClaimKey As Integer, _
                             ByRef r_lClaimKey As Integer, _
                             ByRef r_sBranchCode As String, _
                             ByRef r_TimeStamp As Byte())
        Success()
        r_lBaseClaimKey = m_lBaseClaimId
        r_lClaimKey = m_lClaimId
        r_sBranchCode = m_sBranchCode
        r_TimeStamp = m_TimeStamp
    End Sub

#Region "SetupTestCaseScenarios"

    ''' <summary>
    ''' Setup Test Case Scenarios which should not raise errors
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>
    '''

#Region "Setup NonErrorScenarios Test Cases"

    Private Sub NoClient(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.Client = Nothing
    End Sub

    Private Sub NoInsurer(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.Insurer = Nothing
    End Sub

    Private Sub NoClaimPeril(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.ClaimPeril = Nothing
    End Sub

    Private Sub NoAddress(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.Client.Address = Nothing
        oRequest.Claim.Insurer.Address = Nothing
    End Sub

    Private Sub NoContacts(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.Client.Contact = Nothing
        oRequest.Claim.Insurer.Contact = Nothing
    End Sub

    Private Sub NoReserves(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        For Each oClaimPeril As ProxyWS.BaseClaimPerilType In oRequest.Claim.ClaimPeril
            oClaimPeril.Reserve = Nothing
        Next
    End Sub

    Private Sub NoRecoveries(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        For Each oClaimPeril As ProxyWS.BaseClaimPerilType In oRequest.Claim.ClaimPeril
            oClaimPeril.Recovery = Nothing
        Next
    End Sub

    Private Sub AllNonMandatoryFieldsMissing(ByVal oRequest As ProxyWS.OpenClaimRequestType)

        ' clear claim
        oRequest.Claim.CatastropheCode = ""
        oRequest.Claim.SecondaryCauseCode = ""
        oRequest.Claim.LossToDate = Nothing
        oRequest.Claim.LossToDateSpecified = False
        oRequest.Claim.Location = ""
        oRequest.Claim.TownCode = ""
        oRequest.Claim.UserDefFldACode = ""
        oRequest.Claim.UserDefFldBCode = ""
        oRequest.Claim.UserDefFldCCode = ""
        oRequest.Claim.UserDefFldDCode = ""
        oRequest.Claim.UserDefFldECode = ""
        oRequest.Claim.Comments = ""
        oRequest.Claim.ClaimVersionDescription = ""

        ' clear client
        oRequest.Claim.Client.PartyClaimNumber = ""
        oRequest.Claim.Client.TaxRegistrationNumber = ""
        oRequest.Claim.Client.TaxRegistered = False

        oRequest.Claim.Client.Address.AddressLine2 = ""
        oRequest.Claim.Client.Address.AddressLine3 = ""
        oRequest.Claim.Client.Address.AddressLine4 = ""
        oRequest.Claim.Client.Address.PostCode = ""

        For x As Integer = 0 To oRequest.Claim.Client.Contact.GetUpperBound(0)
            oRequest.Claim.Client.Contact(x).AreaCode = ""
        Next

        ' clear insurer
        oRequest.Claim.Insurer.PartyClaimNumber = ""
        oRequest.Claim.Insurer.ContactName = ""

        oRequest.Claim.Insurer.Address.AddressLine2 = ""
        oRequest.Claim.Insurer.Address.AddressLine3 = ""
        oRequest.Claim.Insurer.Address.AddressLine4 = ""
        oRequest.Claim.Insurer.Address.PostCode = ""

        For x As Integer = 0 To oRequest.Claim.Insurer.Contact.GetUpperBound(0)
            oRequest.Claim.Insurer.Contact(x).AreaCode = ""
        Next

    End Sub

    Private Sub InfoOnlyClaimNoClaimPeril(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.InfoOnly = True
        oRequest.Claim.ClaimPeril = Nothing
    End Sub

#End Region

    ''' <summary>
    ''' Setup Test Case Scenarios which return invalid lookup errors
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>

#Region "Setup Invalid Lookups Test Cases"

    Private Sub InvalidLookupBranchCode(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.BranchCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupClientCountryCode(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.Client.Address.CountryCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupInsurerCountryCode(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.Insurer.Address.CountryCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupProgressStatusCode(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.ProgressStatusCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupPrimaryCauseCode(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.PrimaryCauseCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupHandlerCode(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.HandlerCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupSecondaryCauseCode(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.SecondaryCauseCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupCatastropheCode(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.CatastropheCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupTownCode(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.TownCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupUserDefFldACode(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.UserDefFldACode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupUserDefFldBCode(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.UserDefFldBCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupUserDefFldCCode(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.UserDefFldCCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupUserDefFldDCode(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.UserDefFldDCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupUserDefFldECode(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.UserDefFldECode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupPerilTypeCode(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.ClaimPeril(0).TypeCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupReserveTypeCode(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.ClaimPeril(0).Reserve(0).TypeCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupRecoveryTypeCode(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.ClaimPeril(0).Recovery(0).TypeCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupCurrencyCode(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.CurrencyCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupUnderwritingYearCode(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.UnderwritingYearCode = kInvalidLookupValue
    End Sub

#End Region

    ''' <summary>
    ''' Setup Test Case Scenarios which return missing data errors
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>

#Region "Setup Missing Data Test Cases"

    Private Sub MissingDataBranchCode(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.BranchCode = String.Empty
    End Sub
    Private Sub MissingDataClientAddressLine1(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.Client.Address.AddressLine1 = String.Empty
    End Sub
    Private Sub MissingDataClientCountryCode(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.Client.Address.CountryCode = String.Empty
    End Sub
    Private Sub MissingDataClientContactDetailItem(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.Client.Contact(0).ContactDetail.Item = String.Empty
    End Sub
    Private Sub MissingDataClientTaxRegistrationNumber(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.Client.TaxRegistered = True
        oRequest.Claim.Client.TaxRegistrationNumber = String.Empty
    End Sub
    Private Sub MissingDataInsurerAddressLine1(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.Insurer.Address.AddressLine1 = String.Empty
    End Sub
    Private Sub MissingDataInsurerCountryCode(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.Insurer.Address.CountryCode = String.Empty
    End Sub
    Private Sub MissingDataInsurerContactDetailItem(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.Insurer.Contact(0).ContactDetail.Item = Nothing
    End Sub
    Private Sub MissingDataDescription(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.Description = String.Empty
    End Sub
    Private Sub MissingDataProgressStatusCode(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.ProgressStatusCode = String.Empty
    End Sub
    Private Sub MissingDataPrimaryCauseCode(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.PrimaryCauseCode = String.Empty
    End Sub
    Private Sub MissingDataLossFromDate(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.LossToDateSpecified = False
    End Sub
    Private Sub MissingDataHandlerCode(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.HandlerCode = String.Empty
    End Sub
    Private Sub MissingDataInsuranceFileKey(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.InsuranceFileKey = 0
    End Sub
    Private Sub MissingDataRiskKey(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.RiskKey = 0
    End Sub
    Private Sub MissingDataClaimPerilTypeCode(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.ClaimPeril(0).TypeCode = String.Empty
    End Sub
    Private Sub MissingDataClaimPerilReserveTypeCode(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.ClaimPeril(0).Reserve(0).TypeCode = String.Empty
    End Sub
    Private Sub MissingDataClaimPerilRecoveryTypeCode(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.ClaimPeril(0).Recovery(0).TypeCode = String.Empty
    End Sub

#End Region

    ''' <summary>
    ''' Setup Test Case Scenarios which return business warnings
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>

#Region "Setup Business Warnings Test Cases"

    Private Sub BusinessWarningInfoOnlyClaimDataTruncated(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.InfoOnly = True
    End Sub
    Private Sub BusinessWarningLossFromDateAfterPolicyEndDate(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        Dim oTDClaim As OpenClaimXMLStructure.cClaim
        oTDClaim = m_oTestData.OpenClaim.Claim
        oRequest.Claim.InsuranceFileKey = oTDClaim.PolicyExpiryDateBeforeLossDateInsurancefileCnt
        oRequest.Claim.RiskKey = oTDClaim.PolicyExpiryDateBeforeLossDateRiskCnt
        oTDClaim = Nothing
    End Sub
    Private Sub BusinessWarningLossFromDateBeforePolicyStartDate(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        Dim oTDClaim As OpenClaimXMLStructure.cClaim
        oTDClaim = m_oTestData.OpenClaim.Claim
        oRequest.Claim.InsuranceFileKey = oTDClaim.PolicyStartDateAfterLossDateInsuranceFileCnt
        oRequest.Claim.RiskKey = oTDClaim.PolicyStartDateAfterLossDateRiskCnt
        oTDClaim = Nothing
    End Sub
    Private Sub BusinessWarningRiskIsDeferred(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        Dim oTDClaim As OpenClaimXMLStructure.cClaim
        oTDClaim = m_oTestData.OpenClaim.Claim
        oRequest.Claim.InsuranceFileKey = oTDClaim.ReinsuranceDeferredInsuranceFilecnt
        oRequest.Claim.RiskKey = oTDClaim.ReinsuranceDeferredRiskCnt
        oTDClaim = Nothing
    End Sub
    Private Sub BusinessWarningPolicyIsVoid(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        Dim oTDClaim As OpenClaimXMLStructure.cClaim
        oTDClaim = m_oTestData.OpenClaim.Claim
        oRequest.Claim.InsuranceFileKey = oTDClaim.PolicyVoidedInsuranceFileCnt
        oRequest.Claim.RiskKey = oTDClaim.PolicyVoidedRiskCnt
        oTDClaim = Nothing
    End Sub
    Private Sub BusinessWarningPolicyIsDifferent(ByVal oRequest As ProxyWS.OpenClaimRequestType)

    End Sub

#End Region

    ''' <summary>
    ''' Setup Test Case Scenarios which return business errrors
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>

#Region "Setup Business Errors Test Cases"

    Private Sub BusinessErrorDuplicateClaimExists(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        Dim oTDClaim As OpenClaimXMLStructure.cClaim
        oTDClaim = m_oTestData.OpenClaim.Claim
        oRequest.Claim.InsuranceFileKey = oTDClaim.DuplicateClaimInsuranceFileCnt
        oRequest.Claim.RiskKey = oTDClaim.DuplicateClaimRiskCnt
        oRequest.Claim.LossFromDate = oTDClaim.DuplicateClaimLossFromDate
        oTDClaim = Nothing
    End Sub
    Private Sub BusinessErrorLossFromDataInFuture(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.LossFromDate = DateAdd(DateInterval.Day, 1, Date.Today)
    End Sub
    Private Sub BusinessErrorLossFromDateAfterReportedDate(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.ReportedDate = DateAdd(DateInterval.Day, -1, oRequest.Claim.LossFromDate)
    End Sub
    Private Sub BusinessErrorLossFromDateAfterLossToDate(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.LossToDate = DateAdd(DateInterval.Day, -1, oRequest.Claim.LossFromDate)
    End Sub
    Private Sub BusinessErrorReportedDateInFuture(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.ReportedDate = DateAdd(DateInterval.Day, 1, Date.Today)
    End Sub
    Private Sub BusinessErrorInsuranceFileDetailsNotFound(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.Claim.InsuranceFileKey = 999999999
    End Sub

#End Region

#End Region

#Region "Test Case Response Handlers"

    Private Sub ProcessTestCases(ByVal oRequest As ProxyWS.OpenClaimRequestType, ByVal TestCase As TestCaseScenario)

        Select Case TestCase

            ' error free scenarios
            Case TestCaseScenario.AllNonMandatoryFieldsMissing
                AllNonMandatoryFieldsMissing(oRequest)
            Case TestCaseScenario.NoClient
                NoClient(oRequest)
            Case TestCaseScenario.NoInsurer
                NoInsurer(oRequest)
            Case TestCaseScenario.NoContacts
                NoContacts(oRequest)
            Case TestCaseScenario.NoClaimPeril
                NoClaimPeril(oRequest)
            Case TestCaseScenario.NoRecoveries
                NoRecoveries(oRequest)
            Case TestCaseScenario.NoReserves
                NoReserves(oRequest)
            Case TestCaseScenario.InfoOnlyClaimNoClaimPeril
                InfoOnlyClaimNoClaimPeril(oRequest)

                ' invalid lookup data scenarios
            Case TestCaseScenario.InvalidLookupBranchCode
                InvalidLookupBranchCode(oRequest)
            Case TestCaseScenario.InvalidLookupClientCountryCode
                InvalidLookupClientCountryCode(oRequest)
            Case TestCaseScenario.InvalidLookupInsurerCountryCode
                InvalidLookupInsurerCountryCode(oRequest)
            Case TestCaseScenario.InvalidLookupProgressStatusCode
                InvalidLookupProgressStatusCode(oRequest)
            Case TestCaseScenario.InvalidLookupPrimaryCauseCode
                InvalidLookupPrimaryCauseCode(oRequest)
            Case TestCaseScenario.InvalidLookupHandlerCode
                InvalidLookupHandlerCode(oRequest)
            Case TestCaseScenario.InvalidLookupSecondaryCauseCode
                InvalidLookupSecondaryCauseCode(oRequest)
            Case TestCaseScenario.InvalidLookupCatastropheCode
                InvalidLookupCatastropheCode(oRequest)
            Case TestCaseScenario.InvalidLookupTownCode
                InvalidLookupTownCode(oRequest)
            Case TestCaseScenario.InvalidLookupUserDefFldACode
                InvalidLookupUserDefFldACode(oRequest)
            Case TestCaseScenario.InvalidLookupUserDefFldBCode
                InvalidLookupUserDefFldBCode(oRequest)
            Case TestCaseScenario.InvalidLookupUserDefFldCCode
                InvalidLookupUserDefFldCCode(oRequest)
            Case TestCaseScenario.InvalidLookupUserDefFldDCode
                InvalidLookupUserDefFldDCode(oRequest)
            Case TestCaseScenario.InvalidLookupUserDefFldECode
                InvalidLookupUserDefFldECode(oRequest)
            Case TestCaseScenario.InvalidLookupPerilTypeCode
                InvalidLookupPerilTypeCode(oRequest)
            Case TestCaseScenario.InvalidLookupReserveTypeCode
                InvalidLookupReserveTypeCode(oRequest)
            Case TestCaseScenario.InvalidLookupRecoveryTypeCode
                InvalidLookupRecoveryTypeCode(oRequest)
            Case TestCaseScenario.InvalidLookupCurrencyCode
                InvalidLookupCurrencyCode(oRequest)
            Case TestCaseScenario.InvalidLookupUnderwritingYearCode
                InvalidLookupUnderwritingYearCode(oRequest)

                ' Missing Data
            Case TestCaseScenario.MissingDataBranchCode
                MissingDataBranchCode(oRequest)
            Case TestCaseScenario.MissingDataClientAddressLine1
                MissingDataClientAddressLine1(oRequest)
            Case TestCaseScenario.MissingDataClientCountryCode
                MissingDataClientCountryCode(oRequest)
            Case TestCaseScenario.MissingDataClientTaxRegistrationNumber
                MissingDataClientTaxRegistrationNumber(oRequest)
            Case TestCaseScenario.MissingDataInsurerAddressLine1
                MissingDataInsurerAddressLine1(oRequest)
            Case TestCaseScenario.MissingDataInsurerCountryCode
                MissingDataInsurerCountryCode(oRequest)
            Case TestCaseScenario.MissingDataInsurerContactDetailItem
                MissingDataInsurerContactDetailItem(oRequest)
            Case TestCaseScenario.MissingDataDescription
                MissingDataDescription(oRequest)
            Case TestCaseScenario.MissingDataProgressStatusCode
                MissingDataProgressStatusCode(oRequest)
            Case TestCaseScenario.MissingDataPrimaryCauseCode
                MissingDataPrimaryCauseCode(oRequest)
            Case TestCaseScenario.MissingDataHandlerCode
                MissingDataHandlerCode(oRequest)
            Case TestCaseScenario.MissingDataInsuranceFileKey
                MissingDataInsuranceFileKey(oRequest)
            Case TestCaseScenario.MissingDataRiskKey
                MissingDataRiskKey(oRequest)
            Case TestCaseScenario.MissingDataClaimPerilTypeCode
                MissingDataClaimPerilTypeCode(oRequest)
            Case TestCaseScenario.MissingDataClaimPerilReserveTypeCode
                MissingDataClaimPerilReserveTypeCode(oRequest)
            Case TestCaseScenario.MissingDataClaimPerilRecoveryTypeCode
                MissingDataClaimPerilRecoveryTypeCode(oRequest)

                ' Business Warnings
            Case TestCaseScenario.BusinessWarningInfoOnlyClaimDataTruncated
                BusinessWarningInfoOnlyClaimDataTruncated(oRequest)
            Case TestCaseScenario.BusinessWarningLossFromDateAfterPolicyEndDate
                BusinessWarningLossFromDateAfterPolicyEndDate(oRequest)
            Case TestCaseScenario.BusinessWarningLossFromDateBeforePolicyStartDate
                BusinessWarningLossFromDateBeforePolicyStartDate(oRequest)
            Case TestCaseScenario.BusinessWarningRiskIsDeferred
                BusinessWarningRiskIsDeferred(oRequest)
            Case TestCaseScenario.BusinessWarningPolicyIsVoid
                BusinessWarningPolicyIsVoid(oRequest)
            Case TestCaseScenario.BusinessWarningPolicyIsDifferent
                BusinessWarningPolicyIsDifferent(oRequest)

                ' Business Errrors
            Case TestCaseScenario.BusinessErrorDuplicateClaimExists
                BusinessErrorDuplicateClaimExists(oRequest)
            Case TestCaseScenario.BusinessErrorLossFromDataInFuture
                BusinessErrorLossFromDataInFuture(oRequest)
            Case TestCaseScenario.BusinessErrorLossFromDateAfterReportedDate
                BusinessErrorLossFromDateAfterReportedDate(oRequest)
            Case TestCaseScenario.BusinessErrorLossFromDateAfterLossToDate
                BusinessErrorLossFromDateAfterLossToDate(oRequest)
            Case TestCaseScenario.BusinessErrorReportedDateInFuture
                BusinessErrorReportedDateInFuture(oRequest)

            Case Else
                ' do nothing
        End Select

    End Sub

    Private Sub ProcessBusinessErrors(ByVal oResponse As ProxyWS.BaseResponseType, ByVal TestCase As TestCaseScenario)

        ' raise an error if no error was returned
        SAMTest.AssertCallFailedWithErrors(oResponse, 1)

        ' assign the error object
        Dim oError As ProxyWS.SAMErrorBusinessRule = SAMTest.AssertErrorBusinessRule(oResponse, 0)

        ' raise an error if an invalid lookup table is being used (field name holds the lookup table name)
        Select Case TestCase
            Case TestCaseScenario.BusinessErrorDuplicateClaimExists
                Assert.AreEqual(SAMConstants.SAMBusinessErrors.DuplicateClaimExists, oError.Code, kInvalidCodeReturned)
                Assert.AreEqual(SAMConstants.SAMBusinessErrors.DuplicateClaimExists.ToString(), oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.BusinessErrorLossFromDataInFuture
                Assert.AreEqual(SAMConstants.SAMBusinessErrors.LossFromDataInFuture, oError.Code, kInvalidCodeReturned)
                Assert.AreEqual(SAMConstants.SAMBusinessErrors.LossFromDataInFuture.ToString(), oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.BusinessErrorLossFromDateAfterReportedDate
                Assert.AreEqual(SAMConstants.SAMBusinessErrors.LossFromDateAfterReportedDate, oError.Code, kInvalidCodeReturned)
                Assert.AreEqual(SAMConstants.SAMBusinessErrors.LossFromDateAfterReportedDate.ToString(), oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.BusinessErrorLossFromDateAfterLossToDate
                Assert.AreEqual(SAMConstants.SAMBusinessErrors.LossFromDateAfterLossToDate, oError.Code, kInvalidCodeReturned)
                Assert.AreEqual(SAMConstants.SAMBusinessErrors.LossFromDateAfterLossToDate.ToString(), oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.BusinessErrorReportedDateInFuture
                Assert.AreEqual(SAMConstants.SAMBusinessErrors.ReportedDateInFuture, oError.Code, kInvalidCodeReturned)
                Assert.AreEqual(SAMConstants.SAMBusinessErrors.ReportedDateInFuture.ToString(), oError.Description, kInvalidDescriptionReturned)
        End Select

    End Sub

    Private Sub ProcessBusinessWarnings(ByVal oResponse As ProxyWS.BaseClaimResponseType, ByVal TestCase As TestCaseScenario)

        ' raise an error if no warnings was returned
        SAMTest.AssertCallSucceedededWithWarnings(oResponse, 1)

        ' assign the warning object
        Dim oWarning As ProxyWS.BaseClaimResponseTypeWarnings = SAMTest.AssertWarning(oResponse, 0)

        ' raise an errror if the error code (id) and description dont match the expected values]
        ' for the specified test case scenario
        Select Case TestCase
            Case TestCaseScenario.BusinessWarningInfoOnlyClaimDataTruncated
                Assert.AreEqual(SAMConstants.SAMBusinessWarnings.InfoOnlyClaimDataTruncated, oWarning.Code, kInvalidCodeReturned)
                Assert.AreEqual(SAMConstants.SAMBusinessWarnings.InfoOnlyClaimDataTruncated.ToString(), oWarning.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.BusinessWarningLossFromDateAfterPolicyEndDate
                Assert.AreEqual(SAMConstants.SAMBusinessWarnings.LossFromDateAfterPolicyEndDate, oWarning.Code, kInvalidCodeReturned)
                ' description include additional details so just compare the start to ensure it is the
                ' right type of warning being returned
                StringAssert.StartsWith(SAMConstants.SAMBusinessWarnings.LossFromDateAfterPolicyEndDate.ToString(), oWarning.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.BusinessWarningLossFromDateBeforePolicyStartDate
                Assert.AreEqual(SAMConstants.SAMBusinessWarnings.LossFromDateBeforePolicyStartDate, oWarning.Code, kInvalidCodeReturned)
                ' description include additional details so just compare the start to ensure it is the
                ' right type of warning being returned
                StringAssert.StartsWith(SAMConstants.SAMBusinessWarnings.LossFromDateBeforePolicyStartDate.ToString(), oWarning.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.BusinessWarningRiskIsDeferred
                Assert.AreEqual(SAMConstants.SAMBusinessWarnings.RiskIsDeferred, oWarning.Code, kInvalidCodeReturned)
                Assert.AreEqual(SAMConstants.SAMBusinessWarnings.RiskIsDeferred.ToString(), oWarning.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.BusinessWarningPolicyIsVoid
                Assert.AreEqual(SAMConstants.SAMBusinessWarnings.PolicyIsVoid, oWarning.Code, kInvalidCodeReturned)
                Assert.AreEqual(SAMConstants.SAMBusinessWarnings.PolicyIsVoid.ToString(), oWarning.Description, kInvalidDescriptionReturned)
                'Case TestCaseScenario.BusinessWarningPolicyIsDifferent
                '    Assert.AreEqual(SAMConstants.SAMBusinessWarnings.PolicyIsDifferent, oWarning.Code, kInvalidCodeReturned)
                '    Assert.AreEqual(SAMConstants.SAMBusinessWarnings.PolicyIsDifferent.ToString(), oWarning.Description, kInvalidDescriptionReturned)
        End Select

    End Sub

    Private Sub ProcessMissingData(ByVal oResponse As ProxyWS.BaseResponseType, ByVal TestCase As TestCaseScenario)

        ' raise an error if no error was returned
        SAMTest.AssertCallFailedWithErrors(oResponse, 1)

        ' assign the error object
        Dim oError As ProxyWS.SAMErrorInvalidData = SAMTest.AssertErrorInvalidData(oResponse, 0)

        ' raise an errror if the error code (id) doesnt match the expected value
        Assert.AreEqual(SAMConstants.SAMInvalidData.MandatoryInputMissing, oError.Code, kInvalidCodeReturned)

        ' raise an error if the error description doesnt not match the expected value
        Assert.AreEqual(SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString(), oError.Description, kInvalidDescriptionReturned)

        ' raise an error if the supplied value is not the one specified
        'Assert.AreEqual(kInvalidLookupValue, oError.SuppliedValue, kInvalidLookupValueMessage)

        ' raise an error if an invalid lookup table is being used (field name holds the lookup table name)
        Select Case TestCase
            Case TestCaseScenario.MissingDataBranchCode
                Assert.AreEqual("BranchCode", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
            Case TestCaseScenario.MissingDataClientAddressLine1, TestCaseScenario.MissingDataInsurerAddressLine1
                Assert.AreEqual("AddressLine1", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
            Case TestCaseScenario.MissingDataClientCountryCode, TestCaseScenario.MissingDataInsurerCountryCode
                Assert.AreEqual("CountryCode", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
            Case TestCaseScenario.MissingDataClientTaxRegistrationNumber
                Assert.AreEqual("TaxRegistrationNumber", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
            Case TestCaseScenario.MissingDataDescription
                Assert.AreEqual("Description", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
            Case TestCaseScenario.MissingDataProgressStatusCode
                Assert.AreEqual("ProgressStatusCode", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
            Case TestCaseScenario.MissingDataPrimaryCauseCode
                Assert.AreEqual("PrimaryCauseCode", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
            Case TestCaseScenario.MissingDataHandlerCode
                Assert.AreEqual("HandlerCode", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
            Case TestCaseScenario.MissingDataInsuranceFileKey
                Assert.AreEqual("InsuranceFileKey", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
            Case TestCaseScenario.MissingDataRiskKey
                Assert.AreEqual("RiskKey", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
            Case TestCaseScenario.MissingDataClaimPerilTypeCode
                Assert.AreEqual("ClaimPerilTypeCode", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
            Case TestCaseScenario.MissingDataClaimPerilReserveTypeCode
                Assert.AreEqual("ReserveTypeCode", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
            Case TestCaseScenario.MissingDataClaimPerilRecoveryTypeCode
                Assert.AreEqual("RecoveryTypeCode", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
        End Select

    End Sub

    Private Sub ProcessInvalidLookup(ByVal oResponse As ProxyWS.BaseResponseType, ByVal TestCase As TestCaseScenario)

        ' raise an error if no error was returned
        'SAMTest.AssertCallFailedWithErrors(oResponse, 1)

        ' assign the error object
        Dim oError As ProxyWS.SAMErrorInvalidData = SAMTest.AssertErrorInvalidData(oResponse, 0)

        ' raise an errror if the error code (id) doesnt match the expected value
        Assert.AreEqual(SAMConstants.SAMInvalidData.InvalidLookupListValue, oError.Code, kInvalidCodeReturned)

        ' raise an error if the error description doesnt not match the expected value
        Assert.AreEqual(SAMConstants.SAMInvalidData.InvalidLookupListValue.ToString(), Mid(oError.Description.ToString, 1, Len(SAMConstants.SAMInvalidData.InvalidLookupListValue.ToString)), kInvalidDescriptionReturned)

        ' raise an error if the supplied value is not the one specified
        Assert.AreEqual(kInvalidLookupValue, oError.SuppliedValue, kInvalidLookupValueMessage)

        ' raise an error if an invalid lookup table is being used (field name holds the lookup table name)
        Select Case TestCase
            Case TestCaseScenario.InvalidLookupBranchCode
                Assert.AreEqual("BranchCode", oError.FieldName, kInvalidLookupFieldMessage)
            Case TestCaseScenario.InvalidLookupCatastropheCode
                Assert.AreEqual("CatastropheCode", oError.FieldName, kInvalidLookupFieldMessage)
            Case TestCaseScenario.InvalidLookupClientCountryCode
                Assert.AreEqual("ClientCountryCode", oError.FieldName, kInvalidLookupFieldMessage)
            Case TestCaseScenario.InvalidLookupCurrencyCode
                Assert.AreEqual("CurrencyCode", oError.FieldName, kInvalidLookupFieldMessage)
            Case TestCaseScenario.InvalidLookupHandlerCode
                Assert.AreEqual("HandlerCode", oError.FieldName, kInvalidLookupFieldMessage)
            Case TestCaseScenario.InvalidLookupInsurerCountryCode
                Assert.AreEqual("InsurerCountryCode", oError.FieldName, kInvalidLookupFieldMessage)
            Case TestCaseScenario.InvalidLookupPerilTypeCode
                Assert.AreEqual("PerilTypeCode", oError.FieldName, kInvalidLookupFieldMessage)
            Case TestCaseScenario.InvalidLookupPrimaryCauseCode
                Assert.AreEqual("PrimaryCauseCode", oError.FieldName, kInvalidLookupFieldMessage)
            Case TestCaseScenario.InvalidLookupProgressStatusCode
                Assert.AreEqual("ProgressStatusCode", oError.FieldName, kInvalidLookupFieldMessage)
            Case TestCaseScenario.InvalidLookupRecoveryTypeCode
                Assert.AreEqual("RecoveryTypeCode", oError.FieldName, kInvalidLookupFieldMessage)
            Case TestCaseScenario.InvalidLookupReserveTypeCode
                Assert.AreEqual("ReserveTypeCode", oError.FieldName, kInvalidLookupFieldMessage)
            Case TestCaseScenario.InvalidLookupSecondaryCauseCode
                Assert.AreEqual("SecondaryCauseCode", oError.FieldName, kInvalidLookupFieldMessage)
            Case TestCaseScenario.InvalidLookupTownCode
                Assert.AreEqual("TownCode", oError.FieldName, kInvalidLookupFieldMessage)
            Case TestCaseScenario.InvalidLookupUnderwritingYearCode
                Assert.AreEqual("UnderwritingYearCode", oError.FieldName, kInvalidLookupFieldMessage)
            Case TestCaseScenario.InvalidLookupUserDefFldACode
                Assert.AreEqual("UserDefFldACode", oError.FieldName, kInvalidLookupFieldMessage)
            Case TestCaseScenario.InvalidLookupUserDefFldBCode
                Assert.AreEqual("UserDefFldBCode", oError.FieldName, kInvalidLookupFieldMessage)
            Case TestCaseScenario.InvalidLookupUserDefFldCCode
                Assert.AreEqual("UserDefFldCCode", oError.FieldName, kInvalidLookupFieldMessage)
            Case TestCaseScenario.InvalidLookupUserDefFldDCode
                Assert.AreEqual("UserDefFldDCode", oError.FieldName, kInvalidLookupFieldMessage)
            Case TestCaseScenario.InvalidLookupUserDefFldECode
                Assert.AreEqual("UserDefFldECode", oError.FieldName, kInvalidLookupFieldMessage)
        End Select

    End Sub

#End Region

#Region "NUNIT Test Scenarios"

#Region "NUNIT Success Test Case"

    <Test()> _
    Public Sub Success()

        OpenClaimTest()
        'TestCaseAllInvalidLookups()
        'TestCaseAllMissingData()
        'TestCaseAllBusinessWarnings()
        'TestCaseAllBusinessErrors()

    End Sub

#End Region

#Region "NUNIT Non Error Raising Scenarios"

    <Test()> _
    Public Sub AllNonMandatoryFieldsMissing()
        OpenClaimTest(TestCase:=TestCaseScenario.AllNonMandatoryFieldsMissing)
    End Sub
    <Test()> _
    Public Sub NoClientInformationPassed()
        OpenClaimTest(TestCase:=TestCaseScenario.NoClient)
    End Sub
    <Test()> _
    Public Sub NoInsurerInformationPassed()
        OpenClaimTest(TestCase:=TestCaseScenario.NoInsurer)
    End Sub
    <Test()> _
    Public Sub NoClaimPerilInformationPassed()
        OpenClaimTest(TestCase:=TestCaseScenario.NoClaimPeril)
    End Sub
    <Test()> _
    Public Sub NoContactInformationPassed()
        OpenClaimTest(TestCase:=TestCaseScenario.NoContacts)
    End Sub
    <Test()> _
    Public Sub NoReservesInformationPassed()
        OpenClaimTest(TestCase:=TestCaseScenario.NoReserves)
    End Sub
    <Test()> _
    Public Sub NoRecoveriesInformationPassed()
        OpenClaimTest(TestCase:=TestCaseScenario.NoRecoveries)
    End Sub

#End Region

#Region "NUNIT Invalid Lookup Test Cases"

    <Test()> _
    Public Sub InvalidLookupBranchCode()
        OpenClaimTest(TestCase:=TestCaseScenario.InvalidLookupBranchCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupClientCountryCode()
        OpenClaimTest(TestCase:=TestCaseScenario.InvalidLookupClientCountryCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupInsurerCountryCode()
        OpenClaimTest(TestCase:=TestCaseScenario.InvalidLookupInsurerCountryCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupProgressStatusCode()
        OpenClaimTest(TestCase:=TestCaseScenario.InvalidLookupProgressStatusCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupPrimaryCauseCode()
        OpenClaimTest(TestCase:=TestCaseScenario.InvalidLookupPrimaryCauseCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupHandlerCode()
        OpenClaimTest(TestCase:=TestCaseScenario.InvalidLookupHandlerCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupSecondaryCauseCode()
        OpenClaimTest(TestCase:=TestCaseScenario.InvalidLookupSecondaryCauseCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupCatastropheCode()
        OpenClaimTest(TestCase:=TestCaseScenario.InvalidLookupCatastropheCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupTownCode()
        OpenClaimTest(TestCase:=TestCaseScenario.InvalidLookupTownCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupUserDefFldACode()
        OpenClaimTest(TestCase:=TestCaseScenario.InvalidLookupUserDefFldACode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupUserDefFldBCode()
        OpenClaimTest(TestCase:=TestCaseScenario.InvalidLookupUserDefFldBCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupUserDefFldCCode()
        OpenClaimTest(TestCase:=TestCaseScenario.InvalidLookupUserDefFldCCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupUserDefFldDCode()
        OpenClaimTest(TestCase:=TestCaseScenario.InvalidLookupUserDefFldDCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupUserDefFldECode()
        OpenClaimTest(TestCase:=TestCaseScenario.InvalidLookupUserDefFldECode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupPerilTypeCode()
        OpenClaimTest(TestCase:=TestCaseScenario.InvalidLookupPerilTypeCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupReserveTypeCode()
        OpenClaimTest(TestCase:=TestCaseScenario.InvalidLookupReserveTypeCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupRecoveryTypeCode()
        OpenClaimTest(TestCase:=TestCaseScenario.InvalidLookupRecoveryTypeCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupCurrencyCode()
        OpenClaimTest(TestCase:=TestCaseScenario.InvalidLookupCurrencyCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupUnderwritingYearCode()
        OpenClaimTest(TestCase:=TestCaseScenario.InvalidLookupUnderwritingYearCode)
    End Sub

    Private Sub TestCaseAllInvalidLookups()

        InvalidLookupBranchCode()
        InvalidLookupClientCountryCode()
        InvalidLookupInsurerCountryCode()
        InvalidLookupProgressStatusCode()
        InvalidLookupPrimaryCauseCode()
        InvalidLookupHandlerCode()
        InvalidLookupSecondaryCauseCode()
        InvalidLookupCatastropheCode()
        InvalidLookupTownCode()
        InvalidLookupUserDefFldACode()
        InvalidLookupUserDefFldBCode()
        InvalidLookupUserDefFldCCode()
        InvalidLookupUserDefFldDCode()
        InvalidLookupUserDefFldECode()
        InvalidLookupPerilTypeCode()
        InvalidLookupReserveTypeCode()
        InvalidLookupRecoveryTypeCode()
        InvalidLookupCurrencyCode()
        InvalidLookupUnderwritingYearCode()

    End Sub

#End Region

#Region "NUNIT Missing Data Test Cases"

    <Test()> _
    Public Sub MissingDataBranchCode()
        OpenClaimTest(TestCase:=TestCaseScenario.MissingDataBranchCode)
    End Sub

    <Test()> _
    Public Sub MissingDataClientAddressLine1()
        OpenClaimTest(TestCase:=TestCaseScenario.MissingDataClientAddressLine1)
    End Sub

    <Test()> _
    Public Sub MissingDataClientCountryCode()
        OpenClaimTest(TestCase:=TestCaseScenario.MissingDataClientCountryCode)
    End Sub

    <Test()> _
    Public Sub MissingDataClientTaxRegistrationNumber()
        OpenClaimTest(TestCase:=TestCaseScenario.MissingDataClientTaxRegistrationNumber)
    End Sub

    <Test()> _
    Public Sub MissingDataInsurerAddressLine1()
        OpenClaimTest(TestCase:=TestCaseScenario.MissingDataInsurerAddressLine1)
    End Sub

    <Test()> _
    Public Sub MissingDataInsurerCountryCode()
        OpenClaimTest(TestCase:=TestCaseScenario.MissingDataInsurerCountryCode)
    End Sub

    <Test()> _
    Public Sub MissingDataInsurerContactDetailItem()
        OpenClaimTest(TestCase:=TestCaseScenario.MissingDataInsurerContactDetailItem)
    End Sub

    <Test()> _
    Public Sub MissingDataDescription()
        OpenClaimTest(TestCase:=TestCaseScenario.MissingDataDescription)
    End Sub

    <Test()> _
    Public Sub MissingDataProgressStatusCode()
        OpenClaimTest(TestCase:=TestCaseScenario.MissingDataProgressStatusCode)
    End Sub

    <Test()> _
    Public Sub MissingDataPrimaryCauseCode()
        OpenClaimTest(TestCase:=TestCaseScenario.MissingDataPrimaryCauseCode)
    End Sub

    <Test()> _
    Public Sub MissingDataHandlerCode()
        OpenClaimTest(TestCase:=TestCaseScenario.MissingDataHandlerCode)
    End Sub

    <Test()> _
    Public Sub MissingDataInsuranceFileKey()
        OpenClaimTest(TestCase:=TestCaseScenario.MissingDataInsuranceFileKey)
    End Sub

    <Test()> _
    Public Sub MissingDataRiskKey()
        OpenClaimTest(TestCase:=TestCaseScenario.MissingDataRiskKey)
    End Sub

    <Test()> _
    Public Sub MissingDataClaimPerilTypeCode()
        OpenClaimTest(TestCase:=TestCaseScenario.MissingDataClaimPerilTypeCode)
    End Sub

    <Test()> _
    Public Sub MissingDataClaimPerilReserveTypeCode()
        OpenClaimTest(TestCase:=TestCaseScenario.MissingDataClaimPerilReserveTypeCode)
    End Sub

    <Test()> _
    Public Sub MissingDataClaimPerilRecoveryTypeCode()
        OpenClaimTest(TestCase:=TestCaseScenario.MissingDataClaimPerilRecoveryTypeCode)
    End Sub

    <Ignore()> _
    Public Sub TestCaseAllMissingData()
        MissingDataBranchCode()
        MissingDataClientAddressLine1()
        MissingDataClientCountryCode()
        MissingDataClientTaxRegistrationNumber()
        MissingDataInsurerAddressLine1()
        MissingDataInsurerCountryCode()
        MissingDataInsurerContactDetailItem()
        MissingDataDescription()
        MissingDataProgressStatusCode()
        MissingDataPrimaryCauseCode()
        MissingDataHandlerCode()
        MissingDataInsuranceFileKey()
        MissingDataRiskKey()
        MissingDataClaimPerilTypeCode()
        MissingDataClaimPerilReserveTypeCode()
        MissingDataClaimPerilRecoveryTypeCode()
    End Sub

#End Region

#Region "NUNIT Business Warnings Test Cases"

    <Test()> _
    Public Sub BusinessWarningInfoOnlyClaimDataTruncated()
        OpenClaimTest(TestCase:=TestCaseScenario.BusinessWarningInfoOnlyClaimDataTruncated)
    End Sub

    <Test()> _
    Public Sub BusinessWarningLossFromDateAfterPolicyEndDate()
        OpenClaimTest(TestCase:=TestCaseScenario.BusinessWarningLossFromDateAfterPolicyEndDate)
    End Sub

    <Test()> _
    Public Sub BusinessWarningLossFromDateBeforePolicyStartDate()
        OpenClaimTest(TestCase:=TestCaseScenario.BusinessWarningLossFromDateBeforePolicyStartDate)
    End Sub

    <Test()> _
    Public Sub BusinessWarningRiskIsDeferred()
        OpenClaimTest(TestCase:=TestCaseScenario.BusinessWarningRiskIsDeferred)
    End Sub

    <Test()> _
    Public Sub BusinessWarningPolicyIsVoid()
        OpenClaimTest(TestCase:=TestCaseScenario.BusinessWarningPolicyIsVoid)
    End Sub

    <Test()> _
    Public Sub BusinessWarningPolicyIsDifferent()
        OpenClaimTest(TestCase:=TestCaseScenario.BusinessWarningPolicyIsDifferent)
    End Sub

    <Ignore()> _
    Public Sub TestCaseAllBusinessWarnings()
        BusinessWarningInfoOnlyClaimDataTruncated()
        BusinessWarningLossFromDateAfterPolicyEndDate()
        BusinessWarningLossFromDateBeforePolicyStartDate()
        BusinessWarningRiskIsDeferred()
        BusinessWarningPolicyIsVoid()
        'BusinessWarningPolicyIsDifferent() ' TODO : MEvans : Determine how to setup a testcase to test this scenario
    End Sub

#End Region

#Region "NUNIT Business Error Test Cases"

    <Test()> _
    Public Sub BusinessErrorDuplicateClaimExists()
        OpenClaimTest(TestCase:=TestCaseScenario.BusinessErrorDuplicateClaimExists)
    End Sub
    <Test()> _
    Public Sub BusinessErrorLossFromDataInFuture()
        OpenClaimTest(TestCase:=TestCaseScenario.BusinessErrorLossFromDataInFuture)
    End Sub
    <Test()> _
    Public Sub BusinessErrorLossFromDateAfterReportedDate()
        OpenClaimTest(TestCase:=TestCaseScenario.BusinessErrorLossFromDateAfterReportedDate)
    End Sub
    <Test()> _
    Public Sub BusinessErrorLossFromDateAfterLossToDate()
        OpenClaimTest(TestCase:=TestCaseScenario.BusinessErrorLossFromDateAfterLossToDate)
    End Sub
    <Test()> _
    Public Sub BusinessErrorReportedDateInFuture()
        OpenClaimTest(TestCase:=TestCaseScenario.BusinessErrorReportedDateInFuture)
    End Sub
    Private Sub TestCaseAllBusinessErrors()
        BusinessErrorDuplicateClaimExists()
        BusinessErrorLossFromDataInFuture()
        BusinessErrorLossFromDateAfterReportedDate()
        BusinessErrorLossFromDateAfterLossToDate()
        BusinessErrorReportedDateInFuture()

    End Sub

#End Region

#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        OpenClaimTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        OpenClaimTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        OpenClaimTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        OpenClaimTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class
