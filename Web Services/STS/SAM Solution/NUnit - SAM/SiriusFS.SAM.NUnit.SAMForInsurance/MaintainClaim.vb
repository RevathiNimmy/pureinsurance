Imports NUnit.Framework

<TestFixture()> _
Public Class MaintainClaim
    Inherits BaseTest

#Region "Private Declarations"

    Private Const kInvalidLookupValue As String = "000GARBAGE"
    Private Const kInvalidLookupFieldMessage As String = "Invalid Lookup Field Name Returned"
    Private Const kIncorrectMissingMandatoryFieldMessage As String = "Incorrect Missing Mandatory Field Returned"
    Private Const kInvalidCodeReturned As String = "Invalid Error Code Returned"
    Private Const kInvalidDescriptionReturned As String = "Invalid Error Description Returned"
    Private Const kInvalidLookupValueMessage As String = "Invalid Lookup Value Returned"

    Private m_oTestData As New TestData

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
        'InvalidLookupCurrencyCode
        'InvalidLookupUnderwritingYearCode

        ' Missing Data
        MissingDataBaseClaimKey
        MissingDataBranchCode
        MissingDataClientAddressLine1
        MissingDataClientCountryCode
        MissingDataClientTaxRegistrationNumber
        MissingDataInsurerAddressLine1
        MissingDataInsurerCountryCode
        MissingDataInsurerContactDetailItem
        MissingDataDescription
        MissingDataProgressStatusCode
        MissingDataPrimaryCauseCode
        MissingDataHandlerCode
        MissingDataClaimPerilTypeCode
        MissingDataClaimPerilReserveTypeCode
        MissingDataClaimPerilRecoveryTypeCode
        ''MissingDataInsuranceFileKey
        'MissingDataRiskKey

        ' Busines Warning scenarios
        BusinessWarningInfoOnlyClaimDataTruncated
        BusinessWarningLossFromDateAfterPolicyEndDate
        BusinessWarningLossFromDateBeforePolicyStartDate
        BusinessWarningRiskIsDeferred
        BusinessWarningPolicyIsVoid
        BusinessWarningPolicyIsDifferent
        BusinessWarningLossDateChanged

        'Busines Error scenarios : - some of these cannot be tested
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

    End Enum

#End Region

#Region "Private Methods"

    Public Sub SupportMethod(ByRef BaseClaimKey As Integer, ByRef ClaimKey As Integer, ByRef BranchCode As String, ByRef TimeStamp As Byte())

        IsCalledFromSupportMethod = True

        m_lBaseClaimId = BaseClaimKey
        m_lClaimid = ClaimKey
        m_sBranchCode = BranchCode
        m_TimeStamp = TimeStamp

        MaintainClaimTest(TestCaseScenario.None)

        BaseClaimKey = m_lBaseClaimId
        ClaimKey = m_lClaimid
        BranchCode = m_sBranchCode
        TimeStamp = m_TimeStamp

    End Sub

    Private IsCalledFromSupportMethod As Boolean = False
    Private m_lClaimid As Integer
    Private m_sBranchCode As String
    Private m_lBaseClaimId As Integer
    Private m_TimeStamp As Byte()

    Private Sub GetClaimDetails( _
    ByRef r_lClaimid As Integer, _
    ByRef r_lBaseClaimid As Integer, _
    ByRef r_sBranchCode As String, _
    ByRef r_TimeStamp As Byte())

        Dim oGetClaimDetails As New GetClaimDetails

        oGetClaimDetails.SupportMethod(r_lClaimid, r_sBranchCode, r_lBaseClaimid, r_TimeStamp)

    End Sub

    Private Sub MaintainClaimTest( _
        Optional ByVal TestCase As TestCaseScenario = TestCaseScenario.None, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.MaintainClaimRequestType
        Dim oResponse As ProxyWS.MaintainClaimResponseType
        Dim oClaimRequest As New ProxyWS.BaseClaimMaintainRequestType
        Dim oClaim As New ProxyWS.BaseClaimMaintainType
        Dim oClaimPeril() As ProxyWS.BaseClaimPerilMaintainType = Nothing
        Dim oRecovery() As ProxyWS.BaseClaimPerilRecoveryType = Nothing
        Dim oReserve() As ProxyWS.BaseClaimPerilReserveType = Nothing
        Dim oClient As ProxyWS.BaseClaimPartyClientType = Nothing
        Dim oInsurer As ProxyWS.BaseClaimPartyInsurerType = Nothing
        Dim oClientContact(0 To 4) As ProxyWS.BaseContactType
        Dim oInsurerContact(0 To 4) As ProxyWS.BaseContactType
        Dim oContactDetail As New ProxyWS.BaseContactDetailType

        Dim nLookupError As Integer = 102
        Dim nBusinessError As Integer = 274

        Dim oTDClaim As MaintainClaimXMLStructure.cClaim = Nothing
        oTDClaim = m_oTestData.MaintainClaim.Claim

        Try

            If Not IsCalledFromSupportMethod Then
                ' get the claim details
                GetClaimDetails(m_lClaimid, m_lBaseClaimId, m_sBranchCode, m_TimeStamp)
            End If

            With oRequest

                '.BranchCode = oTDClaim.BranchCode
                .BranchCode = m_sBranchCode
                .TimeStamp = m_TimeStamp

                '***************** CLIENT CONTACTS  ****************************
                Array.Resize(oClientContact, oTDClaim.Client.Contacts.Count)
                Dim iClientContact As Integer = 0
                For Each oCContact As MaintainClaimXMLStructure.cContact In oTDClaim.Client.Contacts
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

                '***************** INSURER CONTACTS  ****************************
                Array.Resize(oInsurerContact, oTDClaim.Insurer.Contacts.Count)
                Dim iInsurerContact As Integer = 0
                For Each oCContact As MaintainClaimXMLStructure.cContact In oTDClaim.Insurer.Contacts
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
                For Each oCClaimPeril As MaintainClaimXMLStructure.cClaimPeril In oTDClaim.ClaimPerils

                    '***************** RECOVERY **************************************
                    Dim iRecovery As Integer = 0
                    Array.Resize(oRecovery, oCClaimPeril.Recovery.Count)
                    For Each oCRecovery As MaintainClaimXMLStructure.cRecovery In oCClaimPeril.Recovery
                        oRecovery(iRecovery) = New ProxyWS.BaseClaimPerilRecoveryType
                        oRecovery(iRecovery).RevisionAmount = oCRecovery.RevisionAmount
                        oRecovery(iRecovery).TypeCode = oCRecovery.TypeCode
                        iRecovery += 1
                    Next

                    '***************** RESERVES **************************************
                    Dim iReserve As Integer = 0
                    Array.Resize(oReserve, oCClaimPeril.Reserve.Count)
                    For Each oCReserve As MaintainClaimXMLStructure.cReserve In oCClaimPeril.Reserve
                        oReserve(iReserve) = New ProxyWS.BaseClaimPerilReserveType
                        oReserve(iReserve).RevisionAmount = oCReserve.RevisionAmount
                        oReserve(iReserve).TypeCode = oCReserve.TypeCode
                        iReserve += 1
                    Next

                    '***************** CLAIM PERIL DETAILS ****************************
                    oClaimPeril(iClaimPeril) = New ProxyWS.BaseClaimPerilMaintainType
                    oClaimPeril(iClaimPeril).Recovery = oRecovery
                    oClaimPeril(iClaimPeril).Reserve = oReserve
                    oClaimPeril(iClaimPeril).TypeCode = oCClaimPeril.TypeCode
                    oClaimPeril(iClaimPeril).Description = oCClaimPeril.Description
                    oClaimPeril(iClaimPeril).BaseClaimPerilKey = oCClaimPeril.BaseClaimPerilKey
                    oClaimPeril(iClaimPeril).BaseClaimPerilKeySpecified = True

                    iClaimPeril += 1

                Next

                '***************** CLAIM **********************************************
                oClaim.ClaimPeril = oClaimPeril
                oClaim.CatastropheCode = oTDClaim.CatastropheCode
                oClaim.ClaimVersionDescription = oTDClaim.ClaimVersionDescription
                oClaim.Comments = oTDClaim.Comments
                oClaim.Description = oTDClaim.Description
                oClaim.HandlerCode = oTDClaim.HandlerCode
                oClaim.InfoOnly = oTDClaim.InfoOnly
                oClaim.LikelyClaim = oTDClaim.LikelyClaim
                oClaim.Location = oTDClaim.Location
                oClaim.LossFromDate = oTDClaim.LossFromDate
                oClaim.LossToDate = oTDClaim.LossToDate
                oClaim.LossToDateSpecified = True
                oClaim.PrimaryCauseCode = oTDClaim.PrimaryCauseCode
                oClaim.ProgressStatusCode = oTDClaim.ProgressStatusCode
                oClaim.ReportedDate = oTDClaim.ReportedDate
                oClaim.SecondaryCauseCode = oTDClaim.SecondaryCauseCode
                oClaim.TownCode = oTDClaim.TownCode
                oClaim.UserDefFldACode = oTDClaim.UserDefFldACode
                oClaim.UserDefFldBCode = oTDClaim.UserDefFldBCode
                oClaim.UserDefFldCCode = oTDClaim.UserDefFldCCode
                oClaim.UserDefFldDCode = oTDClaim.UserDefFldDCode
                oClaim.UserDefFldECode = oTDClaim.UserDefFldECode
                oClaim.ExternalHandler = oTDClaim.ExternalHandler
                oClaim.IgnoreWarnings = oTDClaim.IgnoreWarnings


                'oClaim.BaseClaimKey = oTDClaim.BaseClaimKey
                oClaim.BaseClaimKey = m_lBaseClaimId

                .Claim = oClaim

            End With

            ' reset fields based on selected test case
            ProcessTestCases(oRequest, TestCase)

            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.MaintainClaim(oRequest)

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
                    m_lClaimid = oResponse.ClaimKey
                    m_sBranchCode = oRequest.BranchCode
                    m_TimeStamp = oResponse.TimeStamp

                ElseIf TestCase = TestCaseScenario.InvalidLookupBranchCode OrElse _
                     TestCase = TestCaseScenario.InvalidLookupCatastropheCode OrElse _
                     TestCase = TestCaseScenario.InvalidLookupClientCountryCode OrElse _
                     TestCase = TestCaseScenario.InvalidLookupHandlerCode OrElse _
                     TestCase = TestCaseScenario.InvalidLookupInsurerCountryCode OrElse _
                     TestCase = TestCaseScenario.InvalidLookupPerilTypeCode OrElse _
                     TestCase = TestCaseScenario.InvalidLookupPrimaryCauseCode OrElse _
                     TestCase = TestCaseScenario.InvalidLookupProgressStatusCode OrElse _
                     TestCase = TestCaseScenario.InvalidLookupRecoveryTypeCode OrElse _
                     TestCase = TestCaseScenario.InvalidLookupReserveTypeCode OrElse _
                     TestCase = TestCaseScenario.InvalidLookupSecondaryCauseCode OrElse _
                     TestCase = TestCaseScenario.InvalidLookupTownCode OrElse _
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
                    TestCase = TestCaseScenario.MissingDataClaimPerilTypeCode OrElse _
                    TestCase = TestCaseScenario.MissingDataClaimPerilReserveTypeCode OrElse _
                    TestCase = TestCaseScenario.MissingDataClaimPerilRecoveryTypeCode OrElse _
                    TestCase = TestCaseScenario.MissingDataBaseClaimKey Then

                    ProcessMissingData(oResponse, TestCase)

                ElseIf TestCase = TestCaseScenario.BusinessErrorLossFromDataInFuture OrElse _
                    TestCase = TestCaseScenario.BusinessErrorLossFromDateAfterReportedDate OrElse _
                    TestCase = TestCaseScenario.BusinessErrorLossFromDateAfterLossToDate OrElse _
                    TestCase = TestCaseScenario.BusinessErrorReportedDateInFuture Then

                    ProcessBusinessErrors(oResponse, TestCase)

                ElseIf TestCase = TestCaseScenario.BusinessWarningInfoOnlyClaimDataTruncated OrElse _
                    TestCase = TestCaseScenario.BusinessWarningLossFromDateAfterPolicyEndDate OrElse _
                    TestCase = TestCaseScenario.BusinessWarningLossFromDateBeforePolicyStartDate OrElse _
                    TestCase = TestCaseScenario.BusinessWarningRiskIsDeferred OrElse _
                    TestCase = TestCaseScenario.BusinessWarningPolicyIsVoid OrElse _
                    TestCase = TestCaseScenario.BusinessWarningPolicyIsDifferent OrElse _
                    TestCase = TestCaseScenario.BusinessWarningLossDateChanged Then

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

#Region "SetupTestCaseScenarios"

    ''' <summary>
    ''' Setup Test Case Scenarios which should not raise errors
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>
    '''

#Region "Setup NonErrorScenarios Test Cases"

    Private Sub NoClient(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.Client = Nothing
    End Sub

    Private Sub NoInsurer(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.Insurer = Nothing
    End Sub

    Private Sub NoClaimPeril(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.ClaimPeril = Nothing
    End Sub

    Private Sub NoAddress(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.Client.Address = Nothing
        oRequest.Claim.Insurer.Address = Nothing
    End Sub

    Private Sub NoContacts(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.Client.Contact = Nothing
        oRequest.Claim.Insurer.Contact = Nothing
    End Sub

    Private Sub NoReserves(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        For Each oClaimPeril As ProxyWS.BaseClaimPerilType In oRequest.Claim.ClaimPeril
            oClaimPeril.Reserve = Nothing
        Next
    End Sub

    Private Sub NoRecoveries(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        For Each oClaimPeril As ProxyWS.BaseClaimPerilType In oRequest.Claim.ClaimPeril
            oClaimPeril.Recovery = Nothing
        Next
    End Sub

    Private Sub AllNonMandatoryFieldsMissing(ByVal oRequest As ProxyWS.MaintainClaimRequestType)

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

    Private Sub InfoOnlyClaimNoClaimPeril(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
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

    Private Sub InvalidLookupBranchCode(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.BranchCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupClientCountryCode(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.Client.Address.CountryCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupInsurerCountryCode(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.Insurer.Address.CountryCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupProgressStatusCode(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.ProgressStatusCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupPrimaryCauseCode(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.PrimaryCauseCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupHandlerCode(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.HandlerCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupSecondaryCauseCode(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.SecondaryCauseCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupCatastropheCode(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.CatastropheCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupTownCode(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.TownCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupUserDefFldACode(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.UserDefFldACode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupUserDefFldBCode(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.UserDefFldBCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupUserDefFldCCode(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.UserDefFldCCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupUserDefFldDCode(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.UserDefFldDCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupUserDefFldECode(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.UserDefFldECode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupPerilTypeCode(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.ClaimPeril(0).TypeCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupReserveTypeCode(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.ClaimPeril(0).Reserve(0).TypeCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupRecoveryTypeCode(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.ClaimPeril(0).Recovery(0).TypeCode = kInvalidLookupValue
    End Sub

#End Region

    ''' <summary>
    ''' Setup Test Case Scenarios which return missing data errors
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>

#Region "Setup Missing Data Test Cases"

    Private Sub MissingDataBranchCode(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.BranchCode = String.Empty
    End Sub
    Private Sub MissingDataClientAddressLine1(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.Client.Address.AddressLine1 = String.Empty
    End Sub
    Private Sub MissingDataClientCountryCode(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.Client.Address.CountryCode = String.Empty
    End Sub
    Private Sub MissingDataClientTaxRegistrationNumber(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.Client.TaxRegistered = True
        oRequest.Claim.Client.TaxRegistrationNumber = String.Empty
    End Sub
    Private Sub MissingDataInsurerAddressLine1(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.Insurer.Address.AddressLine1 = String.Empty
    End Sub
    Private Sub MissingDataInsurerCountryCode(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.Insurer.Address.CountryCode = String.Empty
    End Sub
    Private Sub MissingDataInsurerContactDetailItem(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.Insurer.Contact(0).ContactDetail.Item = String.Empty
    End Sub
    Private Sub MissingDataDescription(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.Description = String.Empty
    End Sub
    Private Sub MissingDataProgressStatusCode(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.ProgressStatusCode = String.Empty
    End Sub
    Private Sub MissingDataPrimaryCauseCode(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.PrimaryCauseCode = String.Empty
    End Sub
    Private Sub MissingDataLossFromDate(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.LossToDateSpecified = False
    End Sub
    Private Sub MissingDataHandlerCode(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.HandlerCode = String.Empty
    End Sub
    Private Sub MissingDataClaimPerilTypeCode(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.ClaimPeril(0).TypeCode = String.Empty
    End Sub
    Private Sub MissingDataClaimPerilReserveTypeCode(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.ClaimPeril(0).Reserve(0).TypeCode = String.Empty
    End Sub
    Private Sub MissingDataClaimPerilRecoveryTypeCode(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.ClaimPeril(0).Recovery(0).TypeCode = String.Empty
    End Sub
    Private Sub MissingDataBaseClaimKey(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.BaseClaimKey = 0
    End Sub

#End Region

    ''' <summary>
    ''' Setup Test Case Scenarios which return business warnings
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>

#Region "Setup Business Warnings Test Cases"

    Private Sub BusinessWarningInfoOnlyClaimDataTruncated(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.InfoOnly = True
    End Sub
    Private Sub BusinessWarningLossFromDateAfterPolicyEndDate(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        Dim oTDClaim As MaintainClaimXMLStructure.cClaim
        oTDClaim = m_oTestData.MaintainClaim.Claim
        oRequest.Claim.BaseClaimKey = oTDClaim.PolicyWithExpiryDateBeforeClaimLossDateBaseClaimKey
        oRequest.Claim.LossFromDate = oTDClaim.PolicyWithExpiryDateBeforeClaimLossDateClaimDate
        oRequest.Claim.LossToDate = oTDClaim.PolicyWithExpiryDateBeforeClaimLossDateClaimDate
        oRequest.Claim.ReportedDate = oTDClaim.PolicyWithExpiryDateBeforeClaimLossDateClaimDate
    End Sub
    Private Sub BusinessWarningLossFromDateBeforePolicyStartDate(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        Dim oTDClaim As MaintainClaimXMLStructure.cClaim
        oTDClaim = m_oTestData.MaintainClaim.Claim
        oRequest.Claim.BaseClaimKey = oTDClaim.PolicyWithStartDateAfterClaimLossDateBaseClaimKey
        oRequest.Claim.LossFromDate = oTDClaim.PolicyWithStartDateAfterClaimLossDateClaimDate
        oRequest.Claim.LossToDate = oTDClaim.PolicyWithStartDateAfterClaimLossDateClaimDate
        oRequest.Claim.ReportedDate = oTDClaim.PolicyWithStartDateAfterClaimLossDateClaimDate
    End Sub
    Private Sub BusinessWarningRiskIsDeferred(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        Dim oTDClaim As MaintainClaimXMLStructure.cClaim
        oTDClaim = m_oTestData.MaintainClaim.Claim
        oRequest.Claim.BaseClaimKey = oTDClaim.RiskDeferredBaseClaimKey
        oRequest.Claim.LossFromDate = oTDClaim.RiskDeferredClaimDate
        oRequest.Claim.LossToDate = oTDClaim.RiskDeferredClaimDate
        oRequest.Claim.ReportedDate = oTDClaim.RiskDeferredClaimDate
    End Sub
    Private Sub BusinessWarningPolicyIsVoid(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        Dim oTDClaim As MaintainClaimXMLStructure.cClaim
        oTDClaim = m_oTestData.MaintainClaim.Claim
        oRequest.Claim.BaseClaimKey = oTDClaim.PolicyVoidedBaseClaimKey
        oRequest.Claim.LossFromDate = oTDClaim.PolicyVoidedClaimDate
        oRequest.Claim.LossToDate = oTDClaim.PolicyVoidedClaimDate
        oRequest.Claim.ReportedDate = oTDClaim.PolicyVoidedClaimDate
    End Sub
    Private Sub BusinessWarningPolicyIsDifferent(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
    End Sub

    Private Sub BusinessWarningLossDateChanged(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.LossFromDate = DateAdd(DateInterval.Day, -1, oRequest.Claim.LossFromDate)
    End Sub

#End Region

    ''' <summary>
    ''' Setup Test Case Scenarios which return business errrors
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>

#Region "Setup Business Errors Test Cases"

    Private Sub BusinessErrorDuplicateClaimExists(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        'Dim oTDClaim As OpenClaimXMLStructure.cClaim
        'oTDClaim = m_oTestData.OpenClaim.Claim
        'oRequest.Claim.InsuranceFileKey = oTDClaim.DuplicateClaimInsuranceFileCnt
        'oRequest.Claim.RiskKey = oTDClaim.DuplicateClaimRiskCnt
        'oRequest.Claim.LossFromDate = oTDClaim.DuplicateClaimLossFromDate
        'oTDClaim = Nothing
    End Sub
    Private Sub BusinessErrorLossFromDataInFuture(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.LossFromDate = DateAdd(DateInterval.Day, 1, Date.Today)
    End Sub
    Private Sub BusinessErrorLossFromDateAfterReportedDate(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.ReportedDate = DateAdd(DateInterval.Day, -1, oRequest.Claim.LossFromDate)
    End Sub
    Private Sub BusinessErrorLossFromDateAfterLossToDate(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.LossToDate = DateAdd(DateInterval.Day, -1, oRequest.Claim.LossFromDate)
    End Sub
    Private Sub BusinessErrorReportedDateInFuture(ByVal oRequest As ProxyWS.MaintainClaimRequestType)
        oRequest.Claim.ReportedDate = DateAdd(DateInterval.Day, 1, Date.Today)
    End Sub

#End Region

#End Region

#Region "Test Case Response Handlers"

    Private Sub ProcessTestCases(ByVal oRequest As ProxyWS.MaintainClaimRequestType, ByVal TestCase As TestCaseScenario)

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
            Case TestCaseScenario.MissingDataClaimPerilTypeCode
                MissingDataClaimPerilTypeCode(oRequest)
            Case TestCaseScenario.MissingDataClaimPerilReserveTypeCode
                MissingDataClaimPerilReserveTypeCode(oRequest)
            Case TestCaseScenario.MissingDataClaimPerilRecoveryTypeCode
                MissingDataClaimPerilRecoveryTypeCode(oRequest)
            Case TestCaseScenario.MissingDataBaseClaimKey
                MissingDataBaseClaimKey(oRequest)

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
            Case TestCaseScenario.BusinessWarningLossDateChanged
                BusinessWarningLossDateChanged(oRequest)

                ' Business Errrors
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

        Const kInvalidDescriptionReturned As String = "Invalid Warning Description Returned"

        ' raise an error if no warnings was returned
        SAMTest.AssertCallSucceedededWithWarnings(oResponse)

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
            Case TestCaseScenario.MissingDataClaimPerilTypeCode
                Assert.AreEqual("ClaimPerilTypeCode", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
            Case TestCaseScenario.MissingDataClaimPerilReserveTypeCode
                Assert.AreEqual("ReserveTypeCode", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
            Case TestCaseScenario.MissingDataClaimPerilRecoveryTypeCode
                Assert.AreEqual("RecoveryTypeCode", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
            Case TestCaseScenario.MissingDataBaseClaimKey
                Assert.AreEqual("BaseClaimKey", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
        End Select

    End Sub

    Private Sub ProcessInvalidLookup(ByVal oResponse As ProxyWS.BaseResponseType, ByVal TestCase As TestCaseScenario)

        ' raise an error if no error was returned
        SAMTest.AssertCallFailedWithErrors(oResponse, 1)

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

        MaintainClaimTest()
        'TestCaseAllNonErrorScenarios()
        'TestCaseAllInvalidLookups()
        'TestCaseAllMissingData()
        'TestCaseAllBusinessWarnings()
        'TestCaseAllBusinessErrors()

    End Sub

#End Region

#Region "NUNIT Non Error Raising Scenarios"

    <Test()> _
    Public Sub AllNonMandatoryFieldsMissing()
        MaintainClaimTest(TestCase:=TestCaseScenario.AllNonMandatoryFieldsMissing)
    End Sub
    <Test()> _
    Public Sub NoClientInformationPassed()
        MaintainClaimTest(TestCase:=TestCaseScenario.NoClient)
    End Sub
    <Test()> _
    Public Sub NoInsurerInformationPassed()
        MaintainClaimTest(TestCase:=TestCaseScenario.NoInsurer)
    End Sub
    <Test()> _
    Public Sub NoClaimPerilInformationPassed()
        MaintainClaimTest(TestCase:=TestCaseScenario.NoClaimPeril)
    End Sub
    <Test()> _
    Public Sub NoContactInformationPassed()
        MaintainClaimTest(TestCase:=TestCaseScenario.NoContacts)
    End Sub
    <Test()> _
    Public Sub NoReservesInformationPassed()
        MaintainClaimTest(TestCase:=TestCaseScenario.NoReserves)
    End Sub
    <Test()> _
    Public Sub NoRecoveriesInformationPassed()
        MaintainClaimTest(TestCase:=TestCaseScenario.NoRecoveries)
    End Sub

    Private Sub TestCaseAllNonErrorScenarios()
        AllNonMandatoryFieldsMissing()
        NoClientInformationPassed()
        NoInsurerInformationPassed()
        NoClaimPerilInformationPassed()
        NoContactInformationPassed()
        NoReservesInformationPassed()
        NoRecoveriesInformationPassed()
    End Sub

#End Region

#Region "NUNIT Invalid Lookup Test Cases"

    <Test()> _
    Public Sub InvalidLookupBranchCode()
        MaintainClaimTest(TestCase:=TestCaseScenario.InvalidLookupBranchCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupClientCountryCode()
        MaintainClaimTest(TestCase:=TestCaseScenario.InvalidLookupClientCountryCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupInsurerCountryCode()
        MaintainClaimTest(TestCase:=TestCaseScenario.InvalidLookupInsurerCountryCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupProgressStatusCode()
        MaintainClaimTest(TestCase:=TestCaseScenario.InvalidLookupProgressStatusCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupPrimaryCauseCode()
        MaintainClaimTest(TestCase:=TestCaseScenario.InvalidLookupPrimaryCauseCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupHandlerCode()
        MaintainClaimTest(TestCase:=TestCaseScenario.InvalidLookupHandlerCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupSecondaryCauseCode()
        MaintainClaimTest(TestCase:=TestCaseScenario.InvalidLookupSecondaryCauseCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupCatastropheCode()
        MaintainClaimTest(TestCase:=TestCaseScenario.InvalidLookupCatastropheCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupTownCode()
        MaintainClaimTest(TestCase:=TestCaseScenario.InvalidLookupTownCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupUserDefFldACode()
        MaintainClaimTest(TestCase:=TestCaseScenario.InvalidLookupUserDefFldACode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupUserDefFldBCode()
        MaintainClaimTest(TestCase:=TestCaseScenario.InvalidLookupUserDefFldBCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupUserDefFldCCode()
        MaintainClaimTest(TestCase:=TestCaseScenario.InvalidLookupUserDefFldCCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupUserDefFldDCode()
        MaintainClaimTest(TestCase:=TestCaseScenario.InvalidLookupUserDefFldDCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupUserDefFldECode()
        MaintainClaimTest(TestCase:=TestCaseScenario.InvalidLookupUserDefFldECode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupPerilTypeCode()
        MaintainClaimTest(TestCase:=TestCaseScenario.InvalidLookupPerilTypeCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupReserveTypeCode()
        MaintainClaimTest(TestCase:=TestCaseScenario.InvalidLookupReserveTypeCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupRecoveryTypeCode()
        MaintainClaimTest(TestCase:=TestCaseScenario.InvalidLookupRecoveryTypeCode)
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

    End Sub

#End Region

#Region "NUNIT Missing Data Test Cases"

    <Test()> _
    Public Sub MissingDataBranchCode()
        MaintainClaimTest(TestCase:=TestCaseScenario.MissingDataBranchCode)
    End Sub

    <Test()> _
    Public Sub MissingDataBaseClaimKey()
        MaintainClaimTest(TestCase:=TestCaseScenario.MissingDataBaseClaimKey)
    End Sub

    <Test()> _
    Public Sub MissingDataClientAddressLine1()
        MaintainClaimTest(TestCase:=TestCaseScenario.MissingDataClientAddressLine1)
    End Sub

    <Test()> _
    Public Sub MissingDataClientCountryCode()
        MaintainClaimTest(TestCase:=TestCaseScenario.MissingDataClientCountryCode)
    End Sub

    <Test()> _
    Public Sub MissingDataClientTaxRegistrationNumber()
        MaintainClaimTest(TestCase:=TestCaseScenario.MissingDataClientTaxRegistrationNumber)
    End Sub

    <Test()> _
    Public Sub MissingDataInsurerAddressLine1()
        MaintainClaimTest(TestCase:=TestCaseScenario.MissingDataInsurerAddressLine1)
    End Sub

    <Test()> _
    Public Sub MissingDataInsurerCountryCode()
        MaintainClaimTest(TestCase:=TestCaseScenario.MissingDataInsurerCountryCode)
    End Sub

    <Test()> _
    Public Sub MissingDataDescription()
        MaintainClaimTest(TestCase:=TestCaseScenario.MissingDataDescription)
    End Sub

    <Test()> _
    Public Sub MissingDataProgressStatusCode()
        MaintainClaimTest(TestCase:=TestCaseScenario.MissingDataProgressStatusCode)
    End Sub

    <Test()> _
    Public Sub MissingDataPrimaryCauseCode()
        MaintainClaimTest(TestCase:=TestCaseScenario.MissingDataPrimaryCauseCode)
    End Sub

    <Test()> _
    Public Sub MissingDataHandlerCode()
        MaintainClaimTest(TestCase:=TestCaseScenario.MissingDataHandlerCode)
    End Sub

    <Test()> _
    Public Sub MissingDataClaimPerilTypeCode()
        MaintainClaimTest(TestCase:=TestCaseScenario.MissingDataClaimPerilTypeCode)
    End Sub

    <Test()> _
    Public Sub MissingDataClaimPerilReserveTypeCode()
        MaintainClaimTest(TestCase:=TestCaseScenario.MissingDataClaimPerilReserveTypeCode)
    End Sub

    <Test()> _
    Public Sub MissingDataClaimPerilRecoveryTypeCode()
        MaintainClaimTest(TestCase:=TestCaseScenario.MissingDataClaimPerilRecoveryTypeCode)
    End Sub

    <Ignore()> _
    Public Sub TestCaseAllMissingData()
        MissingDataBranchCode()
        MissingDataClientAddressLine1()
        MissingDataClientCountryCode()
        MissingDataClientTaxRegistrationNumber()
        MissingDataInsurerAddressLine1()
        MissingDataInsurerCountryCode()
        MissingDataDescription()
        MissingDataProgressStatusCode()
        MissingDataPrimaryCauseCode()
        MissingDataHandlerCode()
        MissingDataClaimPerilTypeCode()
        MissingDataClaimPerilReserveTypeCode()
        MissingDataClaimPerilRecoveryTypeCode()
    End Sub

#End Region

#Region "NUNIT Business Warnings Test Cases"

    <Test()> _
    Public Sub BusinessWarningInfoOnlyClaimDataTruncated()
        MaintainClaimTest(TestCase:=TestCaseScenario.BusinessWarningInfoOnlyClaimDataTruncated)
    End Sub

    <Test()> _
    Public Sub BusinessWarningLossFromDateAfterPolicyEndDate()
        MaintainClaimTest(TestCase:=TestCaseScenario.BusinessWarningLossFromDateAfterPolicyEndDate)
    End Sub

    <Test()> _
    Public Sub BusinessWarningLossFromDateBeforePolicyStartDate()
        MaintainClaimTest(TestCase:=TestCaseScenario.BusinessWarningLossFromDateBeforePolicyStartDate)
    End Sub

    <Test()> _
    Public Sub BusinessWarningRiskIsDeferred()
        MaintainClaimTest(TestCase:=TestCaseScenario.BusinessWarningRiskIsDeferred)
    End Sub

    <Test()> _
    Public Sub BusinessWarningPolicyIsVoid()
        MaintainClaimTest(TestCase:=TestCaseScenario.BusinessWarningPolicyIsVoid)
    End Sub

    <Test()> _
    Public Sub BusinessWarningPolicyIsDifferent()
        MaintainClaimTest(TestCase:=TestCaseScenario.BusinessWarningPolicyIsDifferent)
    End Sub

    <Test()> _
    Public Sub BusinessWarningLossDateChanged()
        MaintainClaimTest(TestCase:=TestCaseScenario.BusinessWarningLossDateChanged)
    End Sub

    <Ignore()> _
    Public Sub TestCaseAllBusinessWarnings()
        BusinessWarningInfoOnlyClaimDataTruncated()
        BusinessWarningLossFromDateAfterPolicyEndDate()
        BusinessWarningLossFromDateBeforePolicyStartDate()
        BusinessWarningRiskIsDeferred()
        BusinessWarningPolicyIsVoid()
        BusinessWarningLossDateChanged()
        'BusinessWarningPolicyIsDifferent() ' TODO : MEVANS Determine how to setup a testcase to test this scenario
    End Sub

#End Region

#Region "NUNIT Business Error Test Cases"

    <Test()> _
    Public Sub BusinessErrorLossFromDataInFuture()
        MaintainClaimTest(TestCase:=TestCaseScenario.BusinessErrorLossFromDataInFuture)
    End Sub
    <Test()> _
    Public Sub BusinessErrorLossFromDateAfterReportedDate()
        MaintainClaimTest(TestCase:=TestCaseScenario.BusinessErrorLossFromDateAfterReportedDate)
    End Sub
    <Test()> _
    Public Sub BusinessErrorLossFromDateAfterLossToDate()
        MaintainClaimTest(TestCase:=TestCaseScenario.BusinessErrorLossFromDateAfterLossToDate)
    End Sub
    <Test()> _
    Public Sub BusinessErrorReportedDateInFuture()
        MaintainClaimTest(TestCase:=TestCaseScenario.BusinessErrorReportedDateInFuture)
    End Sub
    Private Sub TestCaseAllBusinessErrors()
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
        MaintainClaimTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        MaintainClaimTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        MaintainClaimTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        MaintainClaimTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class
