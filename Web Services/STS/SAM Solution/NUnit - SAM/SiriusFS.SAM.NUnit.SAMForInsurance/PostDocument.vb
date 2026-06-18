Imports NUnit.Framework

<TestFixture()> _
Public Class PostDocument
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
        BusinessErrorTooFewTransactions
        BusinessErrorTransactionAmountsDontBalance

        ' Busines Warning scenarios

        ' Missing Data
        MissingDataBranchCode
        MissingDataAccountCode

        ' Invalid Lookups
        InvalidLookupBranchCode
        InvalidLookupAccountCode
        InvalidLookupUnderwritingYearCode

    End Enum

#End Region

#Region "Call ProxyWS Method"



    Private Function ReturnAsNewArray(ByVal ListToClone As List(Of ProxyWS.BaseTransactionType)) As Array

        Dim oTransactions As New List(Of ProxyWS.BaseTransactionType)

        oTransactions.GetEnumerator()

        Dim ListEnum As IEnumerator = ListToClone.GetEnumerator()
        ListEnum.Reset()

        Dim oExistingTransaction As ProxyWS.BaseTransactionType
        While ListEnum.MoveNext()

            oExistingTransaction = ListEnum.Current()

            Dim oTransaction As New ProxyWS.BaseTransactionType

            oTransaction.AccountCode = oExistingTransaction.AccountCode
            oTransaction.Amount = oExistingTransaction.Amount
            oTransaction.Comment = oExistingTransaction.Comment
            oTransaction.UnderwritingYearCode = oExistingTransaction.UnderwritingYearCode

            oTransactions.Add(oTransaction)

        End While

        Return oTransactions.ToArray

    End Function

    Private Sub PostDocumentTest( _
        Optional ByVal TestCase As TestCaseScenario = TestCaseScenario.None, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.PostDocumentRequestType
        Dim oResponse As ProxyWS.PostDocumentResponseType

        Dim oTDPostDocument As PostDocumentXMLStructure
        oTDPostDocument = m_oTestData.PostDocument

        Try

            With oRequest
                oRequest.BranchCode = oTDPostDocument.BranchCode
                oRequest.Comment = oTDPostDocument.Comment
                oRequest.DocumentType = oTDPostDocument.DocumentType
                oRequest.Transactions = oTDPostDocument.Transactions.ToArray()

                oRequest.Transactions = ReturnAsNewArray(oTDPostDocument.Transactions)

            End With

            Debug.WriteLine(oRequest.Transactions Is oTDPostDocument.Transactions)

            ' reset fields based on selected test case
            ProcessTestCases(oRequest, TestCase)

            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.PostDocument(oRequest)

            With oResponse

                ' all these test cases should work without error
                If TestCase = TestCaseScenario.None OrElse _
                    TestCase = TestCaseScenario.AllNonMandatoryFieldsMissing Then

                    SAMTest.AssertCallSucceeded(oResponse)

                ElseIf TestCase = TestCaseScenario.InvalidLookupBranchCode OrElse _
                        TestCase = TestCaseScenario.InvalidLookupUnderwritingYearCode OrElse _
                        TestCase = TestCaseScenario.InvalidLookupAccountCode Then

                    ProcessInvalidLookup(oResponse, TestCase)

                ElseIf TestCase = TestCaseScenario.MissingDataBranchCode OrElse _
                        TestCase = TestCaseScenario.MissingDataAccountCode Then

                    ProcessMissingData(oResponse, TestCase)

                ElseIf TestCase = TestCaseScenario.BusinessErrorTooFewTransactions OrElse _
                    TestCase = TestCaseScenario.BusinessErrorTransactionAmountsDontBalance Then

                    ProcessBusinessErrors(oResponse, TestCase)

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


    Private Sub AllNonMandatoryFieldsMissing(ByVal oRequest As ProxyWS.PostDocumentRequestType)

        oRequest.Comment = Nothing
        oRequest.Transactions(0).Comment = Nothing
        oRequest.Transactions(0).UnderwritingYearCode = Nothing
        oRequest.Transactions(1).Comment = Nothing
        oRequest.Transactions(1).UnderwritingYearCode = Nothing

    End Sub

#End Region

    ''' <summary>
    ''' Setup Test Case Scenarios which return invalid lookup errors
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>

#Region "Setup Invalid Lookups Test Cases"

    Private Sub InvalidLookupBranchCode(ByVal oRequest As ProxyWS.PostDocumentRequestType)
        oRequest.BranchCode = kInvalidLookupValue
    End Sub

    Private Sub InvalidLookupAccountCode(ByVal oRequest As ProxyWS.PostDocumentRequestType)
        For Each oTransaction As ProxyWS.BaseTransactionType In oRequest.Transactions
            oTransaction.AccountCode = kInvalidLookupValue
        Next
    End Sub
    Private Sub InvalidLookupUnderwritingYearCode(ByVal oRequest As ProxyWS.PostDocumentRequestType)
        For Each oTransaction As ProxyWS.BaseTransactionType In oRequest.Transactions
            oTransaction.UnderwritingYearCode = kInvalidLookupValue
        Next
    End Sub

#End Region

    ''' <summary>
    ''' Setup Test Case Scenarios which return missing data errors
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>

#Region "Setup Missing Data Test Cases"

    Private Sub MissingDataBranchCode(ByVal oRequest As ProxyWS.PostDocumentRequestType)
        oRequest.BranchCode = String.Empty
    End Sub

    Private Sub MissingDataAccountCode(ByVal oRequest As ProxyWS.PostDocumentRequestType)
        For Each oTransaction As ProxyWS.BaseTransactionType In oRequest.Transactions
            oTransaction.AccountCode = Nothing
        Next
    End Sub

#End Region

    ''' <summary>
    ''' Setup Test Case Scenarios which return business warnings
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>

#Region "Setup Business Warnings Test Cases"

    Private Sub BusinessWarningEXAMPLE(ByVal oRequest As ProxyWS.PostDocumentRequestType)
        '    oRequest.Claim.LossFromDate = DateAdd(DateInterval.Day, 1, Date.Today)
    End Sub

#End Region

    ''' <summary>
    ''' Setup Test Case Scenarios which return business errrors
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>
#Region "Setup Business Errors Test Cases"

    Private Sub BusinessErrorTooFewTransactions(ByVal oRequest As ProxyWS.PostDocumentRequestType)
        Array.Resize(oRequest.Transactions, 1)
    End Sub

    Private Sub BusinessErrorTransactionAmountsDontBalance(ByVal oRequest As ProxyWS.PostDocumentRequestType)
        oRequest.Transactions(0).Amount = 5000
    End Sub

#End Region

#End Region

#Region "Test Case Response Handlers"

    Private Sub ProcessTestCases(ByVal oRequest As ProxyWS.PostDocumentRequestType, ByVal TestCase As TestCaseScenario)

        Select Case TestCase

            ' error free scenarios
            Case TestCaseScenario.AllNonMandatoryFieldsMissing
                AllNonMandatoryFieldsMissing(oRequest)

            Case TestCaseScenario.BusinessErrorTooFewTransactions
                BusinessErrorTooFewTransactions(oRequest)
            Case TestCaseScenario.BusinessErrorTransactionAmountsDontBalance
                BusinessErrorTransactionAmountsDontBalance(oRequest)

                ' invalid lookup data scenarios
            Case TestCaseScenario.InvalidLookupBranchCode
                InvalidLookupBranchCode(oRequest)
            Case TestCaseScenario.InvalidLookupAccountCode
                InvalidLookupAccountCode(oRequest)
            Case TestCaseScenario.InvalidLookupUnderwritingYearCode
                InvalidLookupUnderwritingYearCode(oRequest)
                ' Missing Data
            Case TestCaseScenario.MissingDataBranchCode
                MissingDataBranchCode(oRequest)
            Case TestCaseScenario.MissingDataAccountCode
                MissingDataAccountCode(oRequest)

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
            Case TestCaseScenario.BusinessErrorTooFewTransactions
                Assert.AreEqual(SAMConstants.SAMBusinessErrors.LessThanTwoTransactionsInTransactionArray, oError.Code, kInvalidCodeReturned)
                Assert.AreEqual(SAMConstants.SAMBusinessErrors.LessThanTwoTransactionsInTransactionArray.ToString(), oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.BusinessErrorTransactionAmountsDontBalance
                Assert.AreEqual(SAMConstants.SAMBusinessErrors.TransactionAmountsDoNotBalance, oError.Code, kInvalidCodeReturned)
                Assert.AreEqual(SAMConstants.SAMBusinessErrors.TransactionAmountsDoNotBalance.ToString(), oError.Description, kInvalidDescriptionReturned)
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
        'SAMTest.AssertCallFailedWithErrors(oResponse, 1)

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
            Case TestCaseScenario.MissingDataAccountCode
                Assert.AreEqual("AccountCode", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
            Case TestCaseScenario.MissingDataBranchCode
                Assert.AreEqual("BranchCode", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
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
            Case TestCaseScenario.InvalidLookupAccountCode
                Assert.AreEqual("AccountCode", oError.FieldName, kInvalidLookupFieldMessage)
            Case TestCaseScenario.InvalidLookupBranchCode
                Assert.AreEqual("BranchCode", oError.FieldName, kInvalidLookupFieldMessage)
            Case TestCaseScenario.InvalidLookupUnderwritingYearCode
                Assert.AreEqual("UnderwritingYearCode", oError.FieldName, kInvalidLookupFieldMessage)
        End Select

    End Sub

#End Region

#Region "NUNIT Test Scenarios"

#Region "NUNIT Success Test Case"

    <Test()> _
    Public Sub Success()
        PostDocumentTest()
        'AllNonMandatoryFieldsMissing()
        ''TestCaseAllInvalidLookups()
        ' TestCaseAllMissingData()
        'TestCaseAllBusinessWarnings()
        ' TestCaseAllBusinessErrors()

    End Sub

#End Region

#Region "NUNIT Non Error Raising Scenarios"

    <Test()> _
    Public Sub AllNonMandatoryFieldsMissing()
        PostDocumentTest(TestCase:=TestCaseScenario.AllNonMandatoryFieldsMissing)
    End Sub


#End Region

#Region "NUNIT Invalid Lookup Test Cases"

    <Test()> _
    Public Sub InvalidLookupBranchCode()
        PostDocumentTest(TestCase:=TestCaseScenario.InvalidLookupBranchCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupAccountCode()
        PostDocumentTest(TestCase:=TestCaseScenario.InvalidLookupAccountCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupUnderwritingYearCode()
        PostDocumentTest(TestCase:=TestCaseScenario.InvalidLookupUnderwritingYearCode)
    End Sub

    Private Sub TestCaseAllInvalidLookups()
        InvalidLookupAccountCode()
        InvalidLookupBranchCode()
        InvalidLookupUnderwritingYearCode()
    End Sub

#End Region

#Region "NUNIT Missing Data Test Cases"

    <Test()> _
    Public Sub MissingDataBranchCode()
        PostDocumentTest(TestCase:=TestCaseScenario.MissingDataBranchCode)
    End Sub
    <Test()> _
    Public Sub MissingDataAccountCode()
        PostDocumentTest(TestCase:=TestCaseScenario.MissingDataAccountCode)
    End Sub

    <Ignore()> _
    Public Sub TestCaseAllMissingData()
        MissingDataBranchCode()
        MissingDataAccountCode()
    End Sub

#End Region

#Region "NUNIT Business Warnings Test Cases"

    '<Test()> _
    'Public Sub BusinessWarningInfoOnlyClaimDataTruncated()
    '    PostDocumentTest(TestCase:=TestCaseScenario.BusinessWarningInfoOnlyClaimDataTruncated)
    'End Sub

    <Ignore()> _
    Public Sub TestCaseAllBusinessWarnings()
        'BusinessWarningInfoOnlyClaimDataTruncated()
    End Sub

#End Region

#Region "NUNIT Business Error Test Cases"

    <Test()> _
    Public Sub BusinessErrorTooFewTransactions()
        PostDocumentTest(TestCase:=TestCaseScenario.BusinessErrorTooFewTransactions)
    End Sub
    <Test()> _
    Public Sub BusinessErrorTransactionAmountsDontBalance()
        PostDocumentTest(TestCase:=TestCaseScenario.BusinessErrorTransactionAmountsDontBalance)
    End Sub

    Private Sub TestCaseAllBusinessErrors()
        BusinessErrorTooFewTransactions()
        BusinessErrorTransactionAmountsDontBalance()
    End Sub

#End Region

#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        PostDocumentTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        PostDocumentTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        PostDocumentTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        PostDocumentTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class
