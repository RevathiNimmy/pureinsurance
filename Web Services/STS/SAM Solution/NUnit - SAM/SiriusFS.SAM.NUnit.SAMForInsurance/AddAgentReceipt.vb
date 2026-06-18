Imports NUnit.Framework

<TestFixture()> _
Public Class AddAgentReceipt
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

    Private Enum TestCaseScenario

        ' Non Error Scenarios
        None
        AllNonMandatoryFieldsMissing

        ' Busines Error scenarios : - some of these cannot be tested

        ' Busines Warning scenarios

        ' Missing Data
        MissingDataBranchCode
        MissingDataBankAccountName
        MissingDataCurrencyCode
        MissingDataReceiptTypeCode
        MissingDataMediaTypeCode
        MissingDataAmount

        ' Invalid Lookups
        InvalidLookupBankAccountName
        InvalidLookupBranchCode
        InvalidLookupCountryCode
        InvalidLookupCurrencyCode
        InvalidLookupMediaTypeCode
        InvalidLookupMediaTypeIssuer
        InvalidLookupReceiptTypeCode

    End Enum

#End Region

#Region "Call ProxyWS Method"

    Private Sub AddAgentReceiptTest( _
        Optional ByVal TestCase As TestCaseScenario = TestCaseScenario.None, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.AddAgentReceiptRequestType
        Dim oResponse As ProxyWS.AddAgentReceiptResponseType

        Dim oTDAgentReceipt As AddAgentReceiptXMLStructure
        oTDAgentReceipt = m_oTestData.AgentReceipt


        Try

            With oRequest

                .BranchCode = m_oTestData.BranchCode

                .Receipt = New ProxyWS.BaseReceiptType

                .Receipt.Address1 = oTDAgentReceipt.Address1
                .Receipt.Address2 = oTDAgentReceipt.Address2
                .Receipt.Address3 = oTDAgentReceipt.Address3
                .Receipt.Address4 = oTDAgentReceipt.Address4
                .Receipt.Amount = oTDAgentReceipt.Amount
                .Receipt.BankAccountName = oTDAgentReceipt.BankAccountName
                .Receipt.CashListRef = oTDAgentReceipt.CashListRef
                .Receipt.CCAuthCode = oTDAgentReceipt.CCAuthCode
                .Receipt.CCCustomer = oTDAgentReceipt.CCCustomer
                .Receipt.CCExpiryDate = oTDAgentReceipt.CCExpiryDate
                .Receipt.CCIssue = oTDAgentReceipt.CCIssue
                .Receipt.CCManualAuthCode = oTDAgentReceipt.CCManualAuthCode
                .Receipt.CCName = oTDAgentReceipt.CCName
                .Receipt.CCNumber = oTDAgentReceipt.CCNumber
                .Receipt.CCPin = oTDAgentReceipt.CCPin
                .Receipt.CCStartDate = oTDAgentReceipt.CCStartDate
                .Receipt.CCTransactionCode = oTDAgentReceipt.CCTransactionCode
                .Receipt.ChequeDate = oTDAgentReceipt.ChequeDate
                .Receipt.ChequeDateSpecified = True
                .Receipt.ChequeName = oTDAgentReceipt.ChequeName
                .Receipt.ContactName = oTDAgentReceipt.ContactName
                .Receipt.CountryCode = oTDAgentReceipt.CountryCode
                .Receipt.CurrencyCode = oTDAgentReceipt.CurrencyCode
                .Receipt.MediaReference = oTDAgentReceipt.MediaReference
                .Receipt.MediaTypeCode = oTDAgentReceipt.MediaTypeCode
                .Receipt.MediaTypeIssuerCode = oTDAgentReceipt.MediaTypeIssuerCode
                .Receipt.OurReference = oTDAgentReceipt.OurReference
                .Receipt.PostalCode = oTDAgentReceipt.PostalCode
                .Receipt.ReceiptTypeCode = oTDAgentReceipt.ReceiptTypeCode
                .Receipt.TheirReference = oTDAgentReceipt.TheirReference
                .Receipt.TransactionDate = oTDAgentReceipt.TransactionDate

            End With

            ' reset fields based on selected test case
            ProcessTestCases(oRequest, TestCase)

            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.AddAgentReceipt(oRequest)

            With oResponse

                ' all these test cases should work without error
                If TestCase = TestCaseScenario.None Then

                    SAMTest.AssertCallSucceeded(oResponse)

                ElseIf TestCase = TestCaseScenario.InvalidLookupBranchCode OrElse _
                        TestCase = TestCaseScenario.InvalidLookupBankAccountName OrElse _
                        TestCase = TestCaseScenario.InvalidLookupCountryCode OrElse _
                        TestCase = TestCaseScenario.InvalidLookupCurrencyCode OrElse _
                        TestCase = TestCaseScenario.InvalidLookupMediaTypeCode OrElse _
                        TestCase = TestCaseScenario.InvalidLookupMediaTypeIssuer OrElse _
                        TestCase = TestCaseScenario.InvalidLookupReceiptTypeCode Then

                    ProcessInvalidLookup(oResponse, TestCase)

                ElseIf TestCase = TestCaseScenario.MissingDataBranchCode OrElse _
                        TestCase = TestCaseScenario.MissingDataAmount OrElse _
                        TestCase = TestCaseScenario.MissingDataBankAccountName OrElse _
                        TestCase = TestCaseScenario.MissingDataCurrencyCode OrElse _
                        TestCase = TestCaseScenario.MissingDataMediaTypeCode OrElse _
                        TestCase = TestCaseScenario.MissingDataReceiptTypeCode Then

                    ProcessMissingData(oResponse, TestCase)

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

    Private Sub AllNonMandatoryFieldsMissing(ByVal oRequest As ProxyWS.AddAgentReceiptRequestType)

        With oRequest
            .Receipt.Address1 = String.Empty
            .Receipt.Address2 = String.Empty
            .Receipt.Address3 = String.Empty
            .Receipt.Address4 = String.Empty
            .Receipt.CashListRef = String.Empty
            .Receipt.CCAuthCode = String.Empty
            .Receipt.CCCustomer = String.Empty
            .Receipt.CCExpiryDate = String.Empty
            .Receipt.CCIssue = String.Empty
            .Receipt.CCManualAuthCode = String.Empty
            .Receipt.CCName = String.Empty
            .Receipt.CCNumber = String.Empty
            .Receipt.CCPin = String.Empty
            .Receipt.CCStartDate = String.Empty
            .Receipt.CCTransactionCode = String.Empty
            .Receipt.ChequeDate = Date.Today
            .Receipt.ChequeDateSpecified = False
            .Receipt.ChequeName = String.Empty
            .Receipt.ContactName = String.Empty
            .Receipt.CountryCode = String.Empty
            .Receipt.MediaReference = String.Empty
            .Receipt.MediaTypeIssuerCode = String.Empty
            .Receipt.OurReference = String.Empty
            .Receipt.PostalCode = String.Empty
            .Receipt.TheirReference = String.Empty
        End With

    End Sub

#End Region

    ''' <summary>
    ''' Setup Test Case Scenarios which return invalid lookup errors
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>

#Region "Setup Invalid Lookups Test Cases"

    Private Sub InvalidLookupBankAccountName(ByVal oRequest As ProxyWS.AddAgentReceiptRequestType)
        oRequest.Receipt.BankAccountName = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupBranchCode(ByVal oRequest As ProxyWS.AddAgentReceiptRequestType)
        oRequest.BranchCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupCountryCode(ByVal oRequest As ProxyWS.AddAgentReceiptRequestType)
        oRequest.Receipt.CountryCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupCurrencyCode(ByVal oRequest As ProxyWS.AddAgentReceiptRequestType)
        oRequest.Receipt.CurrencyCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupMediaTypeCode(ByVal oRequest As ProxyWS.AddAgentReceiptRequestType)
        oRequest.Receipt.MediaTypeCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupMediaTypeIssuer(ByVal oRequest As ProxyWS.AddAgentReceiptRequestType)
        oRequest.Receipt.MediaTypeIssuerCode = kInvalidLookupValue
    End Sub
    Private Sub InvalidLookupReceiptTypeCode(ByVal oRequest As ProxyWS.AddAgentReceiptRequestType)
        oRequest.Receipt.ReceiptTypeCode = kInvalidLookupValue
    End Sub

#End Region

    ''' <summary>
    ''' Setup Test Case Scenarios which return missing data errors
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>

#Region "Setup Missing Data Test Cases"

    Private Sub MissingDataBranchCode(ByVal oRequest As ProxyWS.AddAgentReceiptRequestType)
        oRequest.BranchCode = String.Empty
    End Sub
    Private Sub MissingDataBankAccountName(ByVal oRequest As ProxyWS.AddAgentReceiptRequestType)
        oRequest.Receipt.BankAccountName = String.Empty
    End Sub
    Private Sub MissingDataAmount(ByVal oRequest As ProxyWS.AddAgentReceiptRequestType)
        oRequest.Receipt.Amount = 0
    End Sub
    Private Sub MissingDataCurrencyCode(ByVal oRequest As ProxyWS.AddAgentReceiptRequestType)
        oRequest.Receipt.CurrencyCode = String.Empty
    End Sub
    Private Sub MissingDataMediaTypeCode(ByVal oRequest As ProxyWS.AddAgentReceiptRequestType)
        oRequest.Receipt.MediaTypeCode = String.Empty
    End Sub
    Private Sub MissingDataReceiptTypeCode(ByVal oRequest As ProxyWS.AddAgentReceiptRequestType)
        oRequest.Receipt.ReceiptTypeCode = String.Empty
    End Sub

#End Region

    ''' <summary>
    ''' Setup Test Case Scenarios which return business warnings
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>

#Region "Setup Business Warnings Test Cases"

    Private Sub BusinessWarningEXAMPLE(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        '    oRequest.Claim.LossFromDate = DateAdd(DateInterval.Day, 1, Date.Today)
    End Sub

#End Region

    ''' <summary>
    ''' Setup Test Case Scenarios which return business errrors
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>

#Region "Setup Business Errors Test Cases"

    Private Sub BusinessErrorEXAMPLE(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        '    oRequest.Claim.LossFromDate = DateAdd(DateInterval.Day, 1, Date.Today)
    End Sub

#End Region

#End Region

#Region "Test Case Response Handlers"

    Private Sub ProcessTestCases(ByVal oRequest As ProxyWS.AddAgentReceiptRequestType, ByVal TestCase As TestCaseScenario)

        Select Case TestCase

            ' error free scenarios
            Case TestCaseScenario.AllNonMandatoryFieldsMissing
                AllNonMandatoryFieldsMissing(oRequest)

                ' invalid lookup data scenarios
            Case TestCaseScenario.InvalidLookupBankAccountName
                InvalidLookupBankAccountName(oRequest)
            Case TestCaseScenario.InvalidLookupBranchCode
                InvalidLookupBranchCode(oRequest)
            Case TestCaseScenario.InvalidLookupCountryCode
                InvalidLookupCountryCode(oRequest)
            Case TestCaseScenario.InvalidLookupCurrencyCode
                InvalidLookupCurrencyCode(oRequest)
            Case TestCaseScenario.InvalidLookupMediaTypeCode
                InvalidLookupMediaTypeCode(oRequest)
            Case TestCaseScenario.InvalidLookupMediaTypeIssuer
                InvalidLookupMediaTypeIssuer(oRequest)
            Case TestCaseScenario.InvalidLookupReceiptTypeCode
                InvalidLookupReceiptTypeCode(oRequest)

                ' Missing Data
            Case TestCaseScenario.MissingDataAmount
                MissingDataAmount(oRequest)
            Case TestCaseScenario.MissingDataBankAccountName
                MissingDataBankAccountName(oRequest)
            Case TestCaseScenario.MissingDataBranchCode
                MissingDataBranchCode(oRequest)
            Case TestCaseScenario.MissingDataCurrencyCode
                MissingDataCurrencyCode(oRequest)
            Case TestCaseScenario.MissingDataMediaTypeCode
                MissingDataMediaTypeCode(oRequest)
            Case TestCaseScenario.MissingDataReceiptTypeCode
                MissingDataReceiptTypeCode(oRequest)

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
            'Case TestCaseScenario.BusinessErrorDuplicateClaimExists
            '    Assert.AreEqual(SAMConstants.SAMBusinessErrors.DuplicateClaimExists, oError.Code, kInvalidCodeReturned)
            '    Assert.AreEqual(SAMConstants.SAMBusinessErrors.DuplicateClaimExists.ToString(), oError.Description, kInvalidDescriptionReturned)
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
            'Case TestCaseScenario.BusinessWarningInfoOnlyClaimDataTruncated
            '    Assert.AreEqual(SAMConstants.SAMBusinessWarnings.InfoOnlyClaimDataTruncated, oWarning.Code, kInvalidCodeReturned)
            '    Assert.AreEqual(SAMConstants.SAMBusinessWarnings.InfoOnlyClaimDataTruncated.ToString(), oWarning.Description, kInvalidDescriptionReturned)
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
            Case TestCaseScenario.MissingDataBankAccountName
                Assert.AreEqual("BankAccountName", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
            Case TestCaseScenario.MissingDataCurrencyCode
                Assert.AreEqual("CurrencyCode", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
            Case TestCaseScenario.MissingDataReceiptTypeCode
                Assert.AreEqual("ReceiptTypeCode", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
            Case TestCaseScenario.MissingDataMediaTypeCode
                Assert.AreEqual("MediaTypeCode", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
            Case TestCaseScenario.MissingDataAmount
                Assert.AreEqual("Amount", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
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
        Assert.AreEqual(SAMConstants.SAMInvalidData.InvalidLookupListValue.ToString(), oError.Description, kInvalidDescriptionReturned)

        ' raise an error if the supplied value is not the one specified
        Assert.AreEqual(kInvalidLookupValue, oError.SuppliedValue, kInvalidLookupValueMessage)

        ' raise an error if an invalid lookup table is being used (field name holds the lookup table name)
        Select Case TestCase
            Case TestCaseScenario.InvalidLookupBankAccountName
                Assert.AreEqual("BankAccountName", oError.FieldName, kInvalidLookupFieldMessage)
            Case TestCaseScenario.InvalidLookupBranchCode
                Assert.AreEqual("BranchCode", oError.FieldName, kInvalidLookupFieldMessage)
            Case TestCaseScenario.InvalidLookupCountryCode
                Assert.AreEqual("CountryCode", oError.FieldName, kInvalidLookupFieldMessage)
            Case TestCaseScenario.InvalidLookupCurrencyCode
                Assert.AreEqual("CurrencyCode", oError.FieldName, kInvalidLookupFieldMessage)
            Case TestCaseScenario.InvalidLookupMediaTypeCode
                Assert.AreEqual("MediaTypeCode", oError.FieldName, kInvalidLookupFieldMessage)
            Case TestCaseScenario.InvalidLookupMediaTypeIssuer
                Assert.AreEqual("MediaTypeIssuerCode", oError.FieldName, kInvalidLookupFieldMessage)
            Case TestCaseScenario.InvalidLookupReceiptTypeCode
                Assert.AreEqual("ReceiptTypeCode", oError.FieldName, kInvalidLookupFieldMessage)
        End Select

    End Sub

#End Region

#Region "NUNIT Test Scenarios"

#Region "NUNIT Success Test Case"

    <Test()> _
    Public Sub Success()

        AddAgentReceiptTest()
        'TestCaseAllInvalidLookups()
        'TestCaseAllMissingData()
        'TestCaseAllBusinessWarnings()
        'TestCaseAllBusinessErrors()

    End Sub

#End Region

#Region "NUNIT Non Error Raising Scenarios"

    <Test()> _
    Public Sub AllNonMandatoryFieldsMissing()
        AddAgentReceiptTest(TestCase:=TestCaseScenario.AllNonMandatoryFieldsMissing)
    End Sub


#End Region

#Region "NUNIT Invalid Lookup Test Cases"

    <Test()> _
    Public Sub InvalidLookupBankAccountName()
        AddAgentReceiptTest(TestCase:=TestCaseScenario.InvalidLookupBankAccountName)
    End Sub
    <Test()> _
    Public Sub InvalidLookupBranchCode()
        AddAgentReceiptTest(TestCase:=TestCaseScenario.InvalidLookupBranchCode)
    End Sub
    <Test()> _
    Public Sub InvalidLookupCountryCode()
        AddAgentReceiptTest(TestCase:=TestCaseScenario.InvalidLookupCountryCode)
    End Sub
    <Test()> _
    Public Sub InvalidLookupCurrencyCode()
        AddAgentReceiptTest(TestCase:=TestCaseScenario.InvalidLookupCurrencyCode)
    End Sub
    <Test()> _
    Public Sub InvalidLookupMediaTypeCode()
        AddAgentReceiptTest(TestCase:=TestCaseScenario.InvalidLookupMediaTypeCode)
    End Sub
    <Test()> _
    Public Sub InvalidLookupMediaTypeIssuer()
        AddAgentReceiptTest(TestCase:=TestCaseScenario.InvalidLookupMediaTypeIssuer)
    End Sub
    <Test()> _
    Public Sub InvalidLookupReceiptTypeCode()
        AddAgentReceiptTest(TestCase:=TestCaseScenario.InvalidLookupReceiptTypeCode)
    End Sub

    Private Sub TestCaseAllInvalidLookups()
        InvalidLookupBankAccountName()
        InvalidLookupBranchCode()
        InvalidLookupCountryCode()
        InvalidLookupCurrencyCode()
        InvalidLookupMediaTypeCode()
        InvalidLookupMediaTypeIssuer()
        InvalidLookupReceiptTypeCode()
    End Sub

#End Region

#Region "NUNIT Missing Data Test Cases"

    <Test()> _
    Public Sub MissingDataAmount()
        AddAgentReceiptTest(TestCase:=TestCaseScenario.MissingDataAmount)
    End Sub
    <Test()> _
    Public Sub MissingDataBranchCode()
        AddAgentReceiptTest(TestCase:=TestCaseScenario.MissingDataBranchCode)
    End Sub
    <Test()> _
    Public Sub MissingDataBankAccountName()
        AddAgentReceiptTest(TestCase:=TestCaseScenario.MissingDataBankAccountName)
    End Sub
    <Test()> _
    Public Sub MissingDataCurrencyCode()
        AddAgentReceiptTest(TestCase:=TestCaseScenario.MissingDataCurrencyCode)
    End Sub
    <Test()> _
    Public Sub MissingDataMediaTypeCode()
        AddAgentReceiptTest(TestCase:=TestCaseScenario.MissingDataMediaTypeCode)
    End Sub
    <Test()> _
    Public Sub MissingDataReceiptTypeCode()
        AddAgentReceiptTest(TestCase:=TestCaseScenario.MissingDataReceiptTypeCode)
    End Sub

    <Ignore()> _
    Public Sub TestCaseAllMissingData()
        MissingDataBranchCode()
        MissingDataBankAccountName()
        MissingDataCurrencyCode()
        MissingDataReceiptTypeCode()
        MissingDataMediaTypeCode()
        MissingDataAmount()
    End Sub



#End Region

#Region "NUNIT Business Warnings Test Cases"

    '<Test()> _
    'Public Sub BusinessWarningInfoOnlyClaimDataTruncated()
    '    OpenClaimTest(TestCase:=TestCaseScenario.BusinessWarningInfoOnlyClaimDataTruncated)
    'End Sub

    <Ignore()> _
    Public Sub TestCaseAllBusinessWarnings()
        'BusinessWarningInfoOnlyClaimDataTruncated()
    End Sub

#End Region

#Region "NUNIT Business Error Test Cases"

    '<Test()> _
    'Public Sub BusinessErrorDuplicateClaimExists()
    '    OpenClaimTest(TestCase:=TestCaseScenario.BusinessErrorDuplicateClaimExists)
    'End Sub

    Private Sub TestCaseAllBusinessErrors()
        'BusinessErrorDuplicateClaimExists()
    End Sub

#End Region

#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        AddAgentReceiptTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        AddAgentReceiptTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        AddAgentReceiptTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        AddAgentReceiptTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class