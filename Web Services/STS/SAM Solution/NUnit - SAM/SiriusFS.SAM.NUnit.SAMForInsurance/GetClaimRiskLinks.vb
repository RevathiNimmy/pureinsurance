Option Strict On

Imports NUnit.Framework

<TestFixture()> _
Public Class GetClaimRiskLinks
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

        ' Missing Data
        MissingDataBranchCode
        MissingDataInsuranceFileKey
        MissingDataRiskKey

        ' Invalid Lookups
        InvalidLookupBranchCode

        ' Invalid Data
        InvalidDataInsuranceFileKey
        InvalidDataRiskKey

    End Enum

#End Region

#Region "Call ProxyWS Method"

    Private Sub GetClaimRiskLinksTest( _
        Optional ByVal TestCase As TestCaseScenario = TestCaseScenario.None, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.GetClaimRiskLinksRequestType
        Dim oResponse As ProxyWS.GetClaimRiskLinksResponseType

        Dim oTDClaim As OpenClaimXMLStructure.cClaim
        oTDClaim = m_oTestData.OpenClaim.Claim

        Try

            With oRequest

                .BranchCode = m_oTestData.BranchCode
                .InsuranceFileKey = oTDClaim.InsurancefileKey
                .RiskKey = oTDClaim.RiskKey

                ' reset fields based on selected test case
                ProcessTestCases(oRequest, TestCase)

                SetWSETestCaseScenario(nWSETestCaseScenario)
                oResponse = oProxy.GetClaimRiskLinks(oRequest)

                With oResponse

                    ' all these test cases should work without error
                    If TestCase = TestCaseScenario.None Then

                        SAMTest.AssertCallSucceeded(oResponse)

                    ElseIf TestCase = TestCaseScenario.InvalidLookupBranchCode Then

                        ProcessInvalidLookup(oResponse, TestCase)

                    ElseIf TestCase = TestCaseScenario.MissingDataBranchCode OrElse _
                        TestCase = TestCaseScenario.MissingDataInsuranceFileKey OrElse _
                        TestCase = TestCaseScenario.MissingDataRiskKey Then

                        ProcessMissingData(oResponse, TestCase)

                    ElseIf TestCase = TestCaseScenario.InvalidDataInsuranceFileKey OrElse _
                        TestCase = TestCaseScenario.InvalidDataRiskKey Then

                        ProcessInvalidData(oResponse, TestCase)

                    End If

                End With
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

    Public Sub SupportMethod(ByVal r_lBaseClaimKey As Integer)
        Success()
    End Sub

#Region "SetupTestCaseScenarios"

    ''' <summary>
    ''' Setup Test Case Scenarios which return invalid lookup errors
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>

#Region "Setup Invalid Lookups Test Cases"

    Private Sub InvalidLookupBranchCode(ByVal oRequest As ProxyWS.GetClaimRiskLinksRequestType)
        oRequest.BranchCode = kInvalidLookupValue
    End Sub

#End Region

    ''' <summary>
    ''' Setup Test Case Scenarios which return missing data errors
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>

#Region "Setup Missing Data Test Cases"

    Private Sub MissingDataBranchCode(ByVal oRequest As ProxyWS.GetClaimRiskLinksRequestType)
        oRequest.BranchCode = String.Empty
    End Sub
    Private Sub MissingDataInsuranceFileKey(ByVal oRequest As ProxyWS.GetClaimRiskLinksRequestType)
        oRequest.InsuranceFileKey = 0
    End Sub
    Private Sub MissingDataRiskKey(ByVal oRequest As ProxyWS.GetClaimRiskLinksRequestType)
        oRequest.RiskKey = 0
    End Sub

#End Region

#Region "Setup Invalid Data Test Cases"

    Private Sub InvalidDataInsuranceFileKey(ByVal oRequest As ProxyWS.GetClaimRiskLinksRequestType)
        oRequest.InsuranceFileKey = 999999
    End Sub
    Private Sub InvalidDataRiskKey(ByVal oRequest As ProxyWS.GetClaimRiskLinksRequestType)
        oRequest.RiskKey = 1
    End Sub

#End Region

#End Region

#Region "Test Case Response Handlers"

    Private Sub ProcessTestCases(ByVal oRequest As ProxyWS.GetClaimRiskLinksRequestType, ByVal TestCase As TestCaseScenario)

        Select Case TestCase

            ' Invalid lookup data scenarios
            Case TestCaseScenario.InvalidLookupBranchCode
                InvalidLookupBranchCode(oRequest)

                ' Missing Data
            Case TestCaseScenario.MissingDataBranchCode
                MissingDataBranchCode(oRequest)
            Case TestCaseScenario.MissingDataInsuranceFileKey
                MissingDataInsuranceFileKey(oRequest)
            Case TestCaseScenario.MissingDataRiskKey
                MissingDataRiskKey(oRequest)

                ' Invalid Data
            Case TestCaseScenario.InvalidDataInsuranceFileKey
                InvalidDataInsuranceFileKey(oRequest)
            Case TestCaseScenario.InvalidDataRiskKey
                InvalidDataRiskKey(oRequest)

            Case Else
                ' do nothing
        End Select

    End Sub

    Private Sub ProcessInvalidData(ByVal oResponse As ProxyWS.BaseResponseType, ByVal TestCase As TestCaseScenario)

        ' raise an error if no error was returned
        SAMTest.AssertCallFailedWithErrors(oResponse, 1)

        ' assign the error object
        Dim oError As ProxyWS.SAMErrorInvalidData = SAMTest.AssertErrorInvalidData(oResponse, 0)

        ' raise an error if an invalid lookup table is being used (field name holds the lookup table name)
        Select Case TestCase
            Case TestCaseScenario.InvalidDataInsuranceFileKey, TestCaseScenario.InvalidDataRiskKey
                Assert.AreEqual(SAMConstants.SAMInvalidData.ValidationRulesFailed, oError.Code, kInvalidCodeReturned)
                Assert.AreEqual("Risk Key - Insurance File Key link is invalid", oError.Description, kInvalidDescriptionReturned)
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
            Case TestCaseScenario.MissingDataInsuranceFileKey
                Assert.AreEqual("InsuranceFileKey", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
            Case TestCaseScenario.MissingDataRiskKey
                Assert.AreEqual("RiskKey", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
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
        GetClaimRiskLinksTest()
        'TestCaseAllInvalidLookups()
        'TestCaseAllMissingData()
        'TestCaseAllInvalidData()
    End Sub

#End Region

#Region "NUNIT Invalid Lookup Test Cases"

    <Test()> _
    Public Sub InvalidLookupBranchCode()
        GetClaimRiskLinksTest(TestCase:=TestCaseScenario.InvalidLookupBranchCode)
    End Sub

    Private Sub TestCaseAllInvalidLookups()
        InvalidLookupBranchCode()
    End Sub

#End Region

#Region "NUNIT Missing Data Test Cases"

    <Test()> _
    Public Sub MissingDataBranchCode()
        GetClaimRiskLinksTest(TestCase:=TestCaseScenario.MissingDataBranchCode)
    End Sub

    <Test()> _
    Public Sub MissingDataInsuranceFileKey()
        GetClaimRiskLinksTest(TestCase:=TestCaseScenario.MissingDataInsuranceFileKey)
    End Sub

    <Test()> _
    Public Sub MissingDataRiskKey()
        GetClaimRiskLinksTest(TestCase:=TestCaseScenario.MissingDataRiskKey)
    End Sub

    <Ignore()> _
    Public Sub TestCaseAllMissingData()
        MissingDataBranchCode()
        MissingDataInsuranceFileKey()
        MissingDataRiskKey()
    End Sub

#End Region

#Region "NUNIT Invalid Data Test Cases"

    <Test()> _
        Public Sub InvalidDataInsuranceFileKey()
        GetClaimRiskLinksTest(TestCase:=TestCaseScenario.InvalidDataInsuranceFileKey)
    End Sub

    <Test()> _
        Public Sub InvalidDataRiskKey()
        GetClaimRiskLinksTest(TestCase:=TestCaseScenario.InvalidDataInsuranceFileKey)
    End Sub

    <Ignore()> _
    Public Sub TestCaseAllInvalidData()
        InvalidDataInsuranceFileKey()
        InvalidDataRiskKey()
    End Sub

#End Region

#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        GetClaimRiskLinksTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        GetClaimRiskLinksTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        GetClaimRiskLinksTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        GetClaimRiskLinksTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class
