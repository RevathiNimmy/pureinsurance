Imports NUnit.Framework

<TestFixture()> _
Public Class GenerateClaimsDocuments
    Inherits BaseTest

#Region "Private Declarations"

    Private m_oTestData As New TestData

    Private Enum enumTestCaseScenario
        None

        '------------------------- MISSING DATA -------------------------
        MissingBranchCode
        MissingClaimKey
        MssingTransactionType

        '------------------------- INVALID DATA -------------------------
        InvalidBranchCode
        InvalidClaimKey
        InvalidTransactionType

    End Enum

#End Region

#Region "Private Test Methods"

    Private Sub GenerateClaimsDocumentsTest( _
        Optional ByVal TestCases As enumTestCaseScenario = enumTestCaseScenario.None, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.GenerateClaimsDocumentsRequestType
        Dim oResponse As ProxyWS.GenerateClaimsDocumentsResponseType
        Dim nLookupError As Integer = 102
        Dim nBusinessError As Integer = 224

        Try
            With oRequest
                .BranchCode = m_oTestData.BranchCode
                .ClaimKey = m_oTestData.Generateclaim(0).ClaimKey
                .Mode = m_oTestData.Generateclaim(0).Mode
                .TransactionType = m_oTestData.Generateclaim(0).TransactionType
                .ParameterXML = 1
                .OutputAsHTML = m_oTestData.Generateclaim(0).OutputAsHTML

            End With
            '-----------------TEST PROCESS----------------------------------------
            ProcessTestCases(oRequest, TestCases)
            '----------------MAIN PROCEDURE---------------------------------------
            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.GenerateClaimsDocuments(oRequest)

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

    Private Sub ProcessTestCases(ByVal oRequest As ProxyWS.GenerateClaimsDocumentsRequestType, ByVal TestCase As enumTestCaseScenario)

        Select Case TestCase
            Case enumTestCaseScenario.MissingClaimKey
                MissingClaimKey(oRequest)
            Case enumTestCaseScenario.MssingTransactionType
                MissingTransactionType(oRequest)
            Case enumTestCaseScenario.MissingBranchCode
                MissingBranchCode(oRequest)
            Case enumTestCaseScenario.InvalidBranchCode
                InvalidBranchCode(oRequest)
            Case enumTestCaseScenario.InvalidClaimKey
                InvalidClaimKey(oRequest)
            Case enumTestCaseScenario.InvalidTransactionType
                InvalidTransactionType(oRequest)

            Case Else
        End Select

    End Sub

#End Region

#Region "Success"

    <Test()> _
    Public Sub Success()
        GenerateClaimsDocumentsTest()
    End Sub

#End Region

#Region "Requisite Functions for Test Cases (Missing Data)"

    Private Sub MissingClaimKey(ByVal oGenerateClaimsDocumentsRequest As ProxyWS.GenerateClaimsDocumentsRequestType)
        oGenerateClaimsDocumentsRequest.ClaimKey = Nothing
    End Sub
    Private Sub MissingTransactionType(ByVal oGenerateClaimsDocumentsRequest As ProxyWS.GenerateClaimsDocumentsRequestType)
        oGenerateClaimsDocumentsRequest.TransactionType = Nothing
    End Sub
    Private Sub MissingBranchCode(ByVal oGenerateClaimsDocumentsRequest As ProxyWS.GenerateClaimsDocumentsRequestType)
        oGenerateClaimsDocumentsRequest.BranchCode = Nothing
    End Sub

#End Region

#Region "Requisite Functions for Test Cases (Invalid Data)"

    Private Sub InvalidBranchCode(ByVal oGenerateClaimsDocumentsRequest As ProxyWS.GenerateClaimsDocumentsRequestType)
        oGenerateClaimsDocumentsRequest.BranchCode = "202"
    End Sub

    Private Sub InvalidClaimKey(ByVal oGenerateClaimsDocumentsRequest As ProxyWS.GenerateClaimsDocumentsRequestType)
        oGenerateClaimsDocumentsRequest.ClaimKey = 0
    End Sub

    Private Sub InvalidTransactionType(ByVal oGenerateClaimsDocumentsRequest As ProxyWS.GenerateClaimsDocumentsRequestType)
        oGenerateClaimsDocumentsRequest.TransactionType = "Nothing"
    End Sub

#End Region

#Region "Invalid Lookup"

    <Test()> _
    Public Sub InvalidData_BranchCode()
        GenerateClaimsDocumentsTest(TestCases:=enumTestCaseScenario.InvalidBranchCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_ClaimKey()
        GenerateClaimsDocumentsTest(TestCases:=enumTestCaseScenario.InvalidClaimKey)
    End Sub
    <Test()> _
    Public Sub InvalidData_TransactionType()
        GenerateClaimsDocumentsTest(TestCases:=enumTestCaseScenario.InvalidTransactionType)
    End Sub

#End Region

#Region "Missing Data"

    <Test()> _
    Public Sub MissingData_BranchCode()
        GenerateClaimsDocumentsTest(TestCases:=enumTestCaseScenario.MissingBranchCode)
    End Sub
    <Test()> _
    Public Sub MissingData_ClaimKey()
        GenerateClaimsDocumentsTest(TestCases:=enumTestCaseScenario.MissingClaimKey)
    End Sub
    <Test()> _
    Public Sub MissingData_TransactionType()
        GenerateClaimsDocumentsTest(TestCases:=enumTestCaseScenario.MssingTransactionType)
    End Sub

#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        GenerateClaimsDocumentsTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        GenerateClaimsDocumentsTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        GenerateClaimsDocumentsTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        GenerateClaimsDocumentsTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class
