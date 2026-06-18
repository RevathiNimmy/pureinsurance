Imports NUnit.Framework

<TestFixture()> _
Public Class AddMTAQuote
    Inherits BaseTest

#Region " Private Declarations "

    Private Const kInvalidLookupValue As String = "000GARBAGE"
    Private Const kInvalidLookupFieldMessage As String = "Invalid Lookup Field Name Returned"
    Private Const kIncorrectMissingMandatoryFieldMessage As String = "Incorrect Missing Mandatory Field Returned"
    Private Const kInvalidCodeReturned As String = "Invalid Error Code Returned"
    Private Const kInvalidDescriptionReturned As String = "Invalid Error Description Returned"
    Private Const kInvalidLookupValueMessage As String = "Invalid Lookup Value Returned"

    Private m_nPartyCnt As Integer
    Private m_nInsuranceFileCnt As Integer
    Private m_nInsuranceFolderCnt As Integer
    Private m_sInsuranceFileRef As String
    Private m_bQuoteTimeStamp() As Byte
    Private m_bPartyTimeStamp() As Byte
    Private m_blWithInstalments As Boolean

    Private m_oTestData As New TestData

    Private Enum TestCaseScenario
        None

        '------------------------- MISSING DATA -------------------------
        AllNonMandatoryFieldsMissing
        MissingBranchCode
        MissingEffectiveDate
        MissingExpiryDate
        MissingInsuranceFileKey
        MissingTypeOfMta
        MissingMtaReason

        '------------------------- INVALID DATA -------------------------
        InvalidBranchCode
        InvalidEffectiveDate
        InvalidExpiryDate
        InvalidInsuranceFileKey
        InvalidTypeOfMta
        InvalidMtaReason

    End Enum

#End Region

#Region " Private Setup Preconditions "

    Private Sub BindQuote()

        Dim oBindQuote As New BindQuote

        If m_blWithInstalments = True Then
            oBindQuote.SupportMethodWithInstalments(m_nInsuranceFileCnt)
        Else
            oBindQuote.SupportMethod(m_nInsuranceFileCnt)
        End If


    End Sub

    Private Sub AddParameters(ByRef oAddMTAQuoteRequest As ProxyWS.AddMtaQuoteRequestType)

        With oAddMTAQuoteRequest
            .BranchCode = m_oTestData.BranchCode
            .EffectiveDate = DateAdd(DateInterval.Day, 5, m_oTestData.CoverStartDate)
            .ExpiryDate = m_oTestData.CoverEndDate
            .InsuranceFileKey = m_nInsuranceFileCnt
            .MtaReason = "OTHER"
            .TypeOfMta = "PERMANENT"
        End With

    End Sub


    Private Sub ProcessTestCases(ByVal oRequest As ProxyWS.AddMtaQuoteRequestType, ByVal TestCase As TestCaseScenario)

        Select Case TestCase
            Case TestCaseScenario.AllNonMandatoryFieldsMissing
                AllNonMandatoryFieldsMissing(oRequest)
            Case TestCaseScenario.MissingBranchCode
                MissingBranchCode(oRequest)
            Case TestCaseScenario.MissingEffectiveDate
                MissingEffectiveDate(oRequest)
            Case TestCaseScenario.MissingExpiryDate
                MissingExpiryDate(oRequest)
            Case TestCaseScenario.MissingInsuranceFileKey
                MissingInsuranceFileKey(oRequest)
            Case TestCaseScenario.MissingMtaReason
                MissingMtaReason(oRequest)
            Case TestCaseScenario.MissingTypeOfMta
                MissingTypeOfMta(oRequest)
            Case TestCaseScenario.InvalidBranchCode
                InvalidBranchCode(oRequest)
            Case TestCaseScenario.InvalidEffectiveDate
                InvalidEffectiveDate(oRequest)
            Case TestCaseScenario.InvalidExpiryDate
                InvalidExpiryDate(oRequest)
            Case TestCaseScenario.InvalidInsuranceFileKey
                InvalidInsuranceFileKey(oRequest)
            Case TestCaseScenario.InvalidTypeOfMta
                InvalidTypeOfMta(oRequest)

            Case Else
        End Select

    End Sub

#End Region

#Region " Support Method"

    Public Overloads Sub SupportMethod(ByRef r_nInsuranceFileCnt As Integer, _
                                       ByRef r_bQuoteTimeStamp As Byte(), _
                                       ByVal v_blWithInstalments As Boolean)

        m_nInsuranceFileCnt = r_nInsuranceFileCnt
        m_blWithInstalments = v_blWithInstalments
        Success()
        r_nInsuranceFileCnt = m_nInsuranceFileCnt
        r_bQuoteTimeStamp = m_bQuoteTimeStamp


    End Sub

#End Region

#Region "Success"

    Private Sub ProcessAddMTAQuoteTest( _
        Optional ByVal TestCases As TestCaseScenario = TestCaseScenario.None, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.AddMtaQuoteRequestType
        Dim oResponse As ProxyWS.AddMtaQuoteResponseType

        Try
            ' Add a new party
            If m_nInsuranceFileCnt = 0 Then
                BindQuote()
            End If
            Assert.Greater(m_nInsuranceFileCnt, 0, "Class initialisation failed to create a live policy to create the MTA Quote against.")

            ' Set the specific condition on the input class
            AddParameters(oRequest)

            ProcessTestCases(oRequest, TestCases)
            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.AddMtaQuote(oRequest)

            With oResponse

                ' all these test cases should work without error
                If TestCases = TestCaseScenario.None OrElse _
                    TestCases = TestCaseScenario.AllNonMandatoryFieldsMissing Then

                    SAMTest.AssertCallSucceeded(oResponse)

                    m_nInsuranceFileCnt = oResponse.InsuranceFileKey
                    m_bQuoteTimeStamp = oResponse.QuoteTimeStamp

                ElseIf TestCases = TestCaseScenario.InvalidBranchCode OrElse _
                     TestCases = TestCaseScenario.InvalidEffectiveDate OrElse _
                     TestCases = TestCaseScenario.InvalidExpiryDate OrElse _
                     TestCases = TestCaseScenario.InvalidInsuranceFileKey OrElse _
                     TestCases = TestCaseScenario.InvalidTypeOfMta Then

                    ProcessInvalidData(oResponse, TestCases)

                ElseIf TestCases = TestCaseScenario.MissingBranchCode OrElse _
                    TestCases = TestCaseScenario.MissingEffectiveDate OrElse _
                    TestCases = TestCaseScenario.MissingExpiryDate OrElse _
                    TestCases = TestCaseScenario.MissingInsuranceFileKey OrElse _
                    TestCases = TestCaseScenario.MissingMtaReason OrElse _
                    TestCases = TestCaseScenario.MissingTypeOfMta Then

                    ProcessMissingData(oResponse, TestCases)

                    'ElseIf  Then

                    '    ProcessBusinessErrors(oResponse, TestCases)

                    'ElseIf  Then

                    '    ProcessBusinessWarnings(oResponse, TestCases)

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

    <Test()> _
    Public Sub Success()
        ProcessAddMTAQuoteTest()
    End Sub

#End Region

#Region "Invalid Data"

#Region "Requisite Functions for Test Cases (Missing Data)"

    Private Sub AllNonMandatoryFieldsMissing(ByVal oAddMTAQuoteRequest As ProxyWS.AddMtaQuoteRequestType)

    End Sub

    Private Sub MissingBranchCode(ByVal oAddMTAQuoteRequest As ProxyWS.AddMtaQuoteRequestType)
        oAddMTAQuoteRequest.BranchCode = ""
    End Sub

    Private Sub MissingEffectiveDate(ByVal oAddMTAQuoteRequest As ProxyWS.AddMtaQuoteRequestType)
        oAddMTAQuoteRequest.EffectiveDate = Nothing
    End Sub

    Private Sub MissingExpiryDate(ByVal oAddMTAQuoteRequest As ProxyWS.AddMtaQuoteRequestType)
        oAddMTAQuoteRequest.ExpiryDate = Nothing
    End Sub
   _
    Private Sub MissingInsuranceFileKey(ByVal oAddMTAQuoteRequest As ProxyWS.AddMtaQuoteRequestType)
        oAddMTAQuoteRequest.InsuranceFileKey = 0
    End Sub

    Private Sub MissingMtaReason(ByVal oAddMTAQuoteRequest As ProxyWS.AddMtaQuoteRequestType)
        oAddMTAQuoteRequest.MtaReason = String.Empty
    End Sub

    Private Sub MissingTypeOfMta(ByVal oAddMTAQuoteRequest As ProxyWS.AddMtaQuoteRequestType)
        oAddMTAQuoteRequest.TypeOfMta = String.Empty
    End Sub

#End Region

#Region "Requisite Functions for Test Cases (Invalid Data)"

    Private Sub InvalidBranchCode(ByVal oAddMTAQuoteRequest As ProxyWS.AddMtaQuoteRequestType)
        oAddMTAQuoteRequest.BranchCode = kInvalidLookupValue
    End Sub

    Private Sub InvalidEffectiveDate(ByVal oAddMTAQuoteRequest As ProxyWS.AddMtaQuoteRequestType)
        oAddMTAQuoteRequest.EffectiveDate = Date.MinValue
    End Sub

    Private Sub InvalidExpiryDate(ByVal oAddMTAQuoteRequest As ProxyWS.AddMtaQuoteRequestType)
        oAddMTAQuoteRequest.ExpiryDate = Date.MinValue
    End Sub

    Private Sub InvalidInsuranceFileKey(ByVal oAddMTAQuoteRequest As ProxyWS.AddMtaQuoteRequestType)
        oAddMTAQuoteRequest.InsuranceFileKey = -67
    End Sub

    Private Sub InvalidTypeOfMta(ByVal oAddMTAQuoteRequest As ProxyWS.AddMtaQuoteRequestType)
        oAddMTAQuoteRequest.TypeOfMta = "Test"
    End Sub

#End Region

#Region "Mandatory Data Missing"

    <Test()> _
    Public Sub Missing_AllNonMandatoryFields()
        ProcessAddMTAQuoteTest(TestCases:=TestCaseScenario.AllNonMandatoryFieldsMissing)
    End Sub

    <Test()> _
    Public Sub Missing_BranchCode()
        ProcessAddMTAQuoteTest(TestCases:=TestCaseScenario.MissingBranchCode)
    End Sub

    <Test()> _
    Public Sub Missing_EffectiveDate()
        ProcessAddMTAQuoteTest(TestCases:=TestCaseScenario.MissingEffectiveDate)
    End Sub

    <Test()> _
    Public Sub Missing_ExpiryDate()
        ProcessAddMTAQuoteTest(TestCases:=TestCaseScenario.MissingExpiryDate)
    End Sub

    <Test()> _
    Public Sub Missing_InsuranceFileKey()
        ProcessAddMTAQuoteTest(TestCases:=TestCaseScenario.MissingInsuranceFileKey)
    End Sub

    <Test()> _
    Public Sub Missing_MTAReason()
        ProcessAddMTAQuoteTest(TestCases:=TestCaseScenario.MissingMtaReason)
    End Sub

    <Test()> _
    Public Sub Missing_TypeOfMta()
        ProcessAddMTAQuoteTest(TestCases:=TestCaseScenario.MissingTypeOfMta)
    End Sub

#End Region

#Region "Invalid Format"

#End Region

#Region "Invalid Values"

    <Test()> _
    Public Sub invalid_BranchCode()
        ProcessAddMTAQuoteTest(TestCases:=TestCaseScenario.InvalidBranchCode)
    End Sub

    <Test()> _
    Public Sub invalid_EffectiveDate()
        ProcessAddMTAQuoteTest(TestCases:=TestCaseScenario.InvalidEffectiveDate)
    End Sub

    <Test()> _
    Public Sub invalid_ExpiryDate()
        ProcessAddMTAQuoteTest(TestCases:=TestCaseScenario.InvalidExpiryDate)
    End Sub

    <Test()> _
    Public Sub invalid_InsuranceFileKey()
        ProcessAddMTAQuoteTest(TestCases:=TestCaseScenario.InvalidInsuranceFileKey)
    End Sub

    <Test()> _
    Public Sub invalid_MTAReason()
        ProcessAddMTAQuoteTest(TestCases:=TestCaseScenario.InvalidMtaReason)
    End Sub

    <Test()> _
    Public Sub invalid_TypeOfMta()
        ProcessAddMTAQuoteTest(TestCases:=TestCaseScenario.InvalidTypeOfMta)
    End Sub

#End Region

#End Region

#Region "Test Case Response Handlers"

    Private Sub ProcessBusinessErrors(ByVal oResponse As ProxyWS.BaseResponseType, ByVal TestCase As TestCaseScenario)

        ' raise an error if no error was returned
        SAMTest.AssertCallFailedWithErrors(oResponse, 1)

        ' assign the error object
        Dim oError As ProxyWS.SAMErrorBusinessRule = SAMTest.AssertErrorBusinessRule(oResponse, 0)

        ' raise an error if an invalid lookup table is being used (field name holds the lookup table name)
        Select Case TestCase

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

        ' raise an error if an invalid lookup table is being used (field name holds the lookup table name)
        Select Case TestCase
            Case TestCaseScenario.MissingBranchCode
                Assert.AreEqual("BranchCode", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
            Case TestCaseScenario.MissingEffectiveDate
                Assert.AreEqual("EffectiveDate", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
            Case TestCaseScenario.MissingExpiryDate
                Assert.AreEqual("ExpiryDate", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
            Case TestCaseScenario.MissingInsuranceFileKey
                Assert.AreEqual("InsuranceFileKey", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
            Case TestCaseScenario.MissingMtaReason
                Assert.AreEqual("MtaReason", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
            Case TestCaseScenario.MissingTypeOfMta
                Assert.AreEqual("TypeOfMta", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
        End Select

    End Sub

    Private Sub ProcessInvalidData(ByVal oResponse As ProxyWS.BaseResponseType, ByVal TestCase As TestCaseScenario)

        ' raise an error if no error was returned
        SAMTest.AssertCallFailedWithErrors(oResponse, 1)

        ' assign the error object
        Dim oError As ProxyWS.SAMErrorInvalidData = SAMTest.AssertErrorInvalidData(oResponse, 0)

        ' raise an errror if the error code (id) doesnt match the expected value
        Assert.AreEqual(SAMConstants.SAMInvalidData.InvalidLookupListValue, oError.Code, kInvalidCodeReturned)

        ' raise an error if the error description doesnt not match the expected value
        Assert.AreEqual(SAMConstants.SAMInvalidData.InvalidLookupListValue.ToString(), oError.Description, kInvalidDescriptionReturned)

        ' raise an error if an invalid lookup table is being used (field name holds the lookup table name)
        Select Case TestCase
            Case TestCaseScenario.InvalidBranchCode
                Assert.AreEqual("BranchCode", oError.FieldName, kInvalidLookupFieldMessage)
            Case TestCaseScenario.InvalidEffectiveDate
                Assert.AreEqual("EffectiveDate", oError.FieldName, kInvalidLookupFieldMessage)
            Case TestCaseScenario.InvalidExpiryDate
                Assert.AreEqual("ExpiryDate", oError.FieldName, kInvalidLookupFieldMessage)
            Case TestCaseScenario.InvalidInsuranceFileKey
                Assert.AreEqual("InsuranceFileKey", oError.FieldName, kInvalidLookupFieldMessage)
            Case TestCaseScenario.InvalidTypeOfMta
                Assert.AreEqual("TypeOfMta", oError.FieldName, kInvalidLookupFieldMessage)
        End Select

    End Sub

#End Region

#Region "STS Business Rules"

    <Test()> _
    Public Sub STSBusinessRules_CoverEndDateBeforeStartDate()

    End Sub

#End Region

#Region "Sirius Back Office"

#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        ProcessAddMTAQuoteTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        ProcessAddMTAQuoteTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        ProcessAddMTAQuoteTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        ProcessAddMTAQuoteTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class
