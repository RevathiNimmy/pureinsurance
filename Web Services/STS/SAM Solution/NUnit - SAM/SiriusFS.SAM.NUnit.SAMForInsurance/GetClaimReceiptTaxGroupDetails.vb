Imports NUnit.Framework

<TestFixture()> _
Public Class GetClaimReceiptTaxGroupDetails
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

        ' Busines Error scenarios : - some of these cannot be tested
        BusinessErrorDuplicateClaimExists

        ' Busines Warning scenarios
        BusinessWarningInfoOnlyClaimDataTruncated

        ' Missing Data
        MissingDataBranchCode


        ' Invalid Lookups
        InvalidLookupBranchCode

    End Enum

#End Region

#Region "Call ProxyWS Method"

    Private Sub Test( _
        Optional ByVal TestCase As TestCaseScenario = TestCaseScenario.None, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.GetClaimReceiptTaxGroupsRequestType
        Dim oResponse As ProxyWS.GetClaimReceiptTaxGroupsResponseType


        Try

            With oRequest
                .BranchCode = "HEADOFF"
                '.TypeCode = "C_SA"
            End With

            ' reset fields based on selected test case
            ProcessTestCases(oRequest, TestCase)

            SetWSETestCaseScenario(nWSETestCaseScenario)

            oResponse = oProxy.GetClaimREceiptTaxGroups(oRequest)

            With oResponse

                ' all these test cases should work without error
                If TestCase = TestCaseScenario.None OrElse _
                    TestCase = TestCaseScenario.AllNonMandatoryFieldsMissing Then

                    SAMTest.AssertCallSucceeded(oResponse)
                    SAMTest.AssertCallSucceededWithResults(oResponse, oResponse.TaxGroup)
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

    Private Sub NoClient(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        'oRequest.Claim.Client = Nothing
    End Sub

    Private Sub NoInsurer(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        ' oRequest.Claim.Insurer = Nothing
    End Sub

    Private Sub NoClaimPeril(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        'oRequest.Claim.ClaimPeril = Nothing
    End Sub

    Private Sub NoAddress(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        'oRequest.Claim.Client.Address = Nothing
        'oRequest.Claim.Insurer.Address = Nothing
    End Sub

    Private Sub NoContacts(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        ' oRequest.Claim.Client.Contact = Nothing
        ' oRequest.Claim.Insurer.Contact = Nothing
    End Sub

    Private Sub NoReserves(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        'For Each oClaimPeril As ProxyWS.BaseClaimPerilType In oRequest.Claim.ClaimPeril
        '    oClaimPeril.Reserve = Nothing
        'Next
    End Sub

    Private Sub NoRecoveries(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        'For Each oClaimPeril As ProxyWS.BaseClaimPerilType In oRequest.Claim.ClaimPeril
        '    oClaimPeril.Recovery = Nothing
        'Next
    End Sub

    Private Sub AllNonMandatoryFieldsMissing(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)

        '' clear claim
        'oRequest.Claim.CatastropheCode = ""
        'oRequest.Claim.SecondaryCauseCode = ""
        'oRequest.Claim.LossToDate = Nothing
        'oRequest.Claim.LossToDateSpecified = False
        'oRequest.Claim.Location = ""
        'oRequest.Claim.TownCode = ""
        'oRequest.Claim.UserDefFldACode = ""
        'oRequest.Claim.UserDefFldBCode = ""
        'oRequest.Claim.UserDefFldCCode = ""
        'oRequest.Claim.UserDefFldDCode = ""
        'oRequest.Claim.UserDefFldECode = ""
        'oRequest.Claim.Comments = ""
        'oRequest.Claim.ClaimVersionDescription = ""

        '' clear client
        'oRequest.Claim.Client.PartyClaimNumber = ""
        'oRequest.Claim.Client.TaxRegistrationNumber = ""
        'oRequest.Claim.Client.TaxRegistered = False

        'oRequest.Claim.Client.Address.AddressLine2 = ""
        'oRequest.Claim.Client.Address.AddressLine3 = ""
        'oRequest.Claim.Client.Address.AddressLine4 = ""
        'oRequest.Claim.Client.Address.PostCode = ""

        'For x As Integer = 0 To oRequest.Claim.Client.Contact.GetUpperBound(0)
        '    oRequest.Claim.Client.Contact(x).AreaCode = ""
        'Next

        '' clear insurer
        'oRequest.Claim.Insurer.PartyClaimNumber = ""
        'oRequest.Claim.Insurer.ContactName = ""

        'oRequest.Claim.Insurer.Address.AddressLine2 = ""
        'oRequest.Claim.Insurer.Address.AddressLine3 = ""
        'oRequest.Claim.Insurer.Address.AddressLine4 = ""
        'oRequest.Claim.Insurer.Address.PostCode = ""

        'For x As Integer = 0 To oRequest.Claim.Insurer.Contact.GetUpperBound(0)
        '    oRequest.Claim.Insurer.Contact(x).AreaCode = ""
        'Next

    End Sub

    Private Sub InfoOnlyClaimNoClaimPeril(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        'oRequest.Claim.InfoOnly = True
        'oRequest.Claim.ClaimPeril = Nothing
    End Sub

#End Region

    ''' <summary>
    ''' Setup Test Case Scenarios which return invalid lookup errors
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>

#Region "Setup Invalid Lookups Test Cases"

    Private Sub InvalidLookupBranchCode(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        oRequest.BranchCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupClientCountryCode(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        'oRequest.Claim.Client.Address.CountryCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupInsurerCountryCode(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        ' oRequest.Claim.Insurer.Address.CountryCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupProgressStatusCode(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        ' oRequest.Claim.ProgressStatusCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupPrimaryCauseCode(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        'oRequest.Claim.PrimaryCauseCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupHandlerCode(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        'oRequest.Claim.HandlerCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupSecondaryCauseCode(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        'oRequest.Claim.SecondaryCauseCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupCatastropheCode(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        'oRequest.Claim.CatastropheCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupTownCode(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        'oRequest.Claim.TownCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupUserDefFldACode(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        'oRequest.Claim.UserDefFldACode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupUserDefFldBCode(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        'oRequest.Claim.UserDefFldBCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupUserDefFldCCode(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        ' oRequest.Claim.UserDefFldCCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupUserDefFldDCode(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        ' oRequest.Claim.UserDefFldDCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupUserDefFldECode(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        'oRequest.Claim.UserDefFldECode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupPerilTypeCode(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        ' oRequest.Claim.ClaimPeril(0).TypeCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupReserveTypeCode(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        ' oRequest.Claim.ClaimPeril(0).Reserve(0).TypeCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupRecoveryTypeCode(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        'oRequest.Claim.ClaimPeril(0).Recovery(0).TypeCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupCurrencyCode(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        ' oRequest.Claim.CurrencyCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupUnderwritingYearCode(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        ' oRequest.Claim.UnderwritingYearCode = kInvalidLookupValue
    End Sub

#End Region

    ''' <summary>
    ''' Setup Test Case Scenarios which return missing data errors
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>

#Region "Setup Missing Data Test Cases"

    Private Sub MissingDataBranchCode(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        oRequest.BranchCode = String.Empty
    End Sub
    Private Sub MissingDataClientAddressLine1(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        '  oRequest.Claim.Client.Address.AddressLine1 = String.Empty
    End Sub
    Private Sub MissingDataClientCountryCode(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        ' oRequest.Claim.Client.Address.CountryCode = String.Empty
    End Sub
    Private Sub MissingDataClientContactDetailItem(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        ' oRequest.Claim.Client.Contact(0).ContactDetail.Item = String.Empty
    End Sub
    Private Sub MissingDataClientTaxRegistrationNumber(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        '  oRequest.Claim.Client.TaxRegistered = True
        '  oRequest.Claim.Client.TaxRegistrationNumber = String.Empty
    End Sub
    Private Sub MissingDataInsurerAddressLine1(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        '  oRequest.Claim.Insurer.Address.AddressLine1 = String.Empty
    End Sub
    Private Sub MissingDataInsurerCountryCode(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        ' oRequest.Claim.Insurer.Address.CountryCode = String.Empty
    End Sub
    Private Sub MissingDataInsurerContactDetailItem(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        ' oRequest.Claim.Insurer.Contact(0).ContactDetail.Item = Nothing
    End Sub
    Private Sub MissingDataDescription(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        '  oRequest.Claim.Description = String.Empty
    End Sub
    Private Sub MissingDataProgressStatusCode(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        '  oRequest.Claim.ProgressStatusCode = String.Empty
    End Sub
    Private Sub MissingDataPrimaryCauseCode(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        '  oRequest.Claim.PrimaryCauseCode = String.Empty
    End Sub
    Private Sub MissingDataLossFromDate(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        '  oRequest.Claim.LossToDateSpecified = False
    End Sub
    Private Sub MissingDataHandlerCode(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        '  oRequest.Claim.HandlerCode = String.Empty
    End Sub
    Private Sub MissingDataInsuranceFileKey(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        ' oRequest.Claim.InsuranceFileKey = 0
    End Sub
    Private Sub MissingDataRiskKey(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        ' oRequest.Claim.RiskKey = 0
    End Sub
    Private Sub MissingDataClaimPerilTypeCode(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        '  oRequest.Claim.ClaimPeril(0).TypeCode = String.Empty
    End Sub
    Private Sub MissingDataClaimPerilReserveTypeCode(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        ' oRequest.Claim.ClaimPeril(0).Reserve(0).TypeCode = String.Empty
    End Sub
    Private Sub MissingDataClaimPerilRecoveryTypeCode(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        '  oRequest.Claim.ClaimPeril(0).Recovery(0).TypeCode = String.Empty
    End Sub

#End Region

    ''' <summary>
    ''' Setup Test Case Scenarios which return business warnings
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>

#Region "Setup Business Warnings Test Cases"

    Private Sub BusinessWarningInfoOnlyClaimDataTruncated(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        ' oRequest.Claim.InfoOnly = True
    End Sub
    Private Sub BusinessWarningLossFromDateAfterPolicyEndDate(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        'Dim oTDClaim As OpenClaimXMLStructure.cClaim
        'oTDClaim = m_oTestData.OpenClaim.Claim
        'oRequest.Claim.InsuranceFileKey = oTDClaim.PolicyExpiryDateBeforeLossDateInsurancefileCnt
        'oRequest.Claim.RiskKey = oTDClaim.PolicyExpiryDateBeforeLossDateRiskCnt
        'oTDClaim = Nothing
    End Sub
    Private Sub BusinessWarningLossFromDateBeforePolicyStartDate(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        'Dim oTDClaim As OpenClaimXMLStructure.cClaim
        'oTDClaim = m_oTestData.OpenClaim.Claim
        'oRequest.Claim.InsuranceFileKey = oTDClaim.PolicyStartDateAfterLossDateInsuranceFileCnt
        'oRequest.Claim.RiskKey = oTDClaim.PolicyStartDateAfterLossDateRiskCnt
        'oTDClaim = Nothing
    End Sub
    Private Sub BusinessWarningRiskIsDeferred(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        'Dim oTDClaim As OpenClaimXMLStructure.cClaim
        'oTDClaim = m_oTestData.OpenClaim.Claim
        'oRequest.Claim.InsuranceFileKey = oTDClaim.ReinsuranceDeferredInsuranceFilecnt
        'oRequest.Claim.RiskKey = oTDClaim.ReinsuranceDeferredRiskCnt
        'oTDClaim = Nothing
    End Sub
    Private Sub BusinessWarningPolicyIsVoid(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        'Dim oTDClaim As OpenClaimXMLStructure.cClaim
        'oTDClaim = m_oTestData.OpenClaim.Claim
        'oRequest.Claim.InsuranceFileKey = oTDClaim.PolicyVoidedInsuranceFileCnt
        'oRequest.Claim.RiskKey = oTDClaim.PolicyVoidedRiskCnt
        'oTDClaim = Nothing
    End Sub
    Private Sub BusinessWarningPolicyIsDifferent(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)

    End Sub

#End Region

    ''' <summary>
    ''' Setup Test Case Scenarios which return business errrors
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>

#Region "Setup Business Errors Test Cases"

    Private Sub BusinessErrorDuplicateClaimExists(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        'Dim oTDClaim As OpenClaimXMLStructure.cClaim
        'oTDClaim = m_oTestData.OpenClaim.Claim
        'oRequest.Claim.InsuranceFileKey = oTDClaim.DuplicateClaimInsuranceFileCnt
        'oRequest.Claim.RiskKey = oTDClaim.DuplicateClaimRiskCnt
        'oRequest.Claim.LossFromDate = oTDClaim.DuplicateClaimLossFromDate
        'oTDClaim = Nothing
    End Sub
    Private Sub BusinessErrorLossFromDataInFuture(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        '   oRequest.Claim.LossFromDate = DateAdd(DateInterval.Day, 1, Date.Today)
    End Sub
    Private Sub BusinessErrorLossFromDateAfterReportedDate(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        ' oRequest.Claim.ReportedDate = DateAdd(DateInterval.Day, -1, oRequest.Claim.LossFromDate)
    End Sub
    Private Sub BusinessErrorLossFromDateAfterLossToDate(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        '  oRequest.Claim.LossToDate = DateAdd(DateInterval.Day, -1, oRequest.Claim.LossFromDate)
    End Sub
    Private Sub BusinessErrorReportedDateInFuture(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        '  oRequest.Claim.ReportedDate = DateAdd(DateInterval.Day, 1, Date.Today)
    End Sub
    Private Sub BusinessErrorInsuranceFileDetailsNotFound(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType)
        '  oRequest.Claim.InsuranceFileKey = 999999999
    End Sub

#End Region

#End Region

#Region "Test Case Response Handlers"

    Private Sub ProcessTestCases(ByVal oRequest As ProxyWS.GetClaimReceiptTaxGroupsRequestType, ByVal TestCase As TestCaseScenario)

        Select Case TestCase

            ' error free scenarios
            Case TestCaseScenario.AllNonMandatoryFieldsMissing
                AllNonMandatoryFieldsMissing(oRequest)


                ' invalid lookup data scenarios

                ' Missing Data


                ' Business Warnings


                ' Business Errrors

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
        End Select

    End Sub

#End Region

#Region "NUNIT Test Scenarios"

#Region "NUNIT Success Test Case"

    <Test()> _
    Public Sub Success()

        Test()
        'TestCaseAllInvalidLookups()
        'TestCaseAllMissingData()
        'TestCaseAllBusinessWarnings()
        'TestCaseAllBusinessErrors()

    End Sub

#End Region

#Region "NUNIT Non Error Raising Scenarios"

    <Test()> _
    Public Sub AllNonMandatoryFieldsMissing()
        Test(TestCase:=TestCaseScenario.AllNonMandatoryFieldsMissing)
    End Sub

#End Region

#Region "NUNIT Invalid Lookup Test Cases"

    Private Sub TestCaseAllInvalidLookups()


    End Sub

#End Region

#Region "NUNIT Missing Data Test Cases"


#End Region

#Region "NUNIT Business Warnings Test Cases"


#End Region

#Region "NUNIT Business Error Test Cases"


#End Region

#End Region

#Region "WSE Security"


#End Region

End Class



