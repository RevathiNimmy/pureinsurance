Imports NUnit.Framework
Imports System.Xml
Imports System.Xml.Serialization

<TestFixture()> _
Public Class GetInstalmentQuotes
    Inherits BaseTest

#Region " Private Declarations "

    Private m_nInsuranceFileCnt As Integer
    Private m_dblPremiumDueGross As Double
    Private m_nRiskCnt As Integer
    Private m_vQuoteArray() As ProxyWS.BaseGetInstalmentQuotesResponseTypeRow

    Private m_oTestData As New TestData

    Private Enum TestCaseScenario

        ' Non Error Scenarios
        None
        MTA

    End Enum


    Private Enum enumMissingData
        None
        BranchCode
        InsuranceFileKey
    End Enum

    Private Enum enumInvalidLookup
        None
        BranchCode
    End Enum

    Private Enum enumSTSBusinessError
        None
        InvalidInsuranceFileCnt
    End Enum

#End Region

#Region " Setup Preconditions "

    Private Sub UpdateRisk(ByVal nTestCaseScenario As TestCaseScenario)

        Dim oUpdateRisk As New UpdateRisk
        If nTestCaseScenario = TestCaseScenario.None Then
            oUpdateRisk.SupportMethod(m_nInsuranceFileCnt, m_nRiskCnt, m_dblPremiumDueGross)
        Else
            oUpdateRisk.SupportMethodMTAWithInstalments(m_nInsuranceFileCnt, m_nRiskCnt, m_dblPremiumDueGross)
        End If

    End Sub

#End Region

#Region " Private Test Methods "

    Private Sub GetInstalmentQuotesTest( _
                    Optional ByVal eMissingData As enumMissingData = enumMissingData.None, _
                    Optional ByVal eInvalidLookup As enumInvalidLookup = enumInvalidLookup.None, _
                    Optional ByVal eSTSBusinessError As enumSTSBusinessError = enumSTSBusinessError.None, _
                    Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None, _
                    Optional ByVal nTestCaseScenario As TestCaseScenario = TestCaseScenario.None)

        Dim oRequest As New ProxyWS.GetInstalmentQuotesRequestType
        Dim oResponse As ProxyWS.GetInstalmentQuotesResponseType
        Dim nLookupError As String = "102"
        Dim nBusinessError As String = "224"

        ' Add the Party/Quote/Risk and update the risk
        UpdateRisk(nTestCaseScenario)

        Try
            With oRequest
                If eMissingData <> enumMissingData.BranchCode Then
                    If eInvalidLookup = enumInvalidLookup.BranchCode Then
                        .BranchCode = m_oTestData.InvalidLookupCode
                        nLookupError = "210"
                    Else
                        .BranchCode = m_oTestData.BranchCode
                    End If
                End If
                If eMissingData <> enumMissingData.InsuranceFileKey Then
                    If eSTSBusinessError <> enumSTSBusinessError.InvalidInsuranceFileCnt Then
                        .InsuranceFileKey = m_nInsuranceFileCnt
                    Else
                        .InsuranceFileKey = m_oTestData.InvalidCnt
                    End If
                End If
                .AmountToFinance = m_dblPremiumDueGross
                .StartDate = Now
                .EndDate = Now.AddYears(1)
                .MonthDay = 1
                .PreferredDate = Now.AddDays(7)
                .QuoteDate = Now
                .PaymentProtection = False
                .WeekDay = 1
                .OverrideInterestRate = -1
                .OverrideRate = -1
            End With


            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.GetInstalmentQuotes(oRequest)
            With oResponse
                If eMissingData <> enumMissingData.None Then
                    ' Missing Data tests
                    SAMTest.AssertCallFailedWithErrors(oResponse, 1)
                    SAMTest.AssertErrorInvalidData(oResponse, 0, 100, "Mandatory " & eMissingData.ToString & " is missing")
                ElseIf eInvalidLookup <> enumInvalidLookup.None Then
                    ' Invalid Lookup tests
                    SAMTest.AssertCallFailedWithErrors(oResponse, 1)
                    SAMTest.AssertErrorInvalidData(oResponse, 0, nLookupError, eInvalidLookup.ToString & " is invalid")
                ElseIf eSTSBusinessError <> enumSTSBusinessError.None Then
                    ' Business Rule tests
                    SAMTest.AssertCallFailedWithErrors(oResponse, 1)
                    SAMTest.AssertErrorBusinessRule(oResponse, 0, nBusinessError)
                Else
                    ' Success Tests
                    SAMTest.AssertCallSucceeded(oResponse)
                    SAMTest.AssertCallSucceededWithResults(oResponse, oResponse.Quotes)
                    m_vQuoteArray = oResponse.Quotes
                End If
            End With

        Catch soapex As System.Web.Services.Protocols.SoapException
            If soapex.Detail.InnerXml <> "" Then
                Assert.Fail(soapex.Detail.InnerXml)
            Else
                Assert.Fail(soapex.Message)
            End If
        Catch ex As Exception
            Assert.Fail(ex.Message)
        Finally
            oRequest = Nothing
            oResponse = Nothing
        End Try

    End Sub

#End Region

#Region " Success "

    Public Sub SupportMethod(ByRef r_nInsuranceFileCnt As Integer, ByRef r_dblPremiumDueGross As Double, ByRef r_vQuoteArray As System.Array)

        Success()
        r_nInsuranceFileCnt = m_nInsuranceFileCnt
        r_dblPremiumDueGross = m_dblPremiumDueGross
        r_vQuoteArray = m_vQuoteArray

    End Sub

    <Test()> _
    Public Sub Success()
        GetInstalmentQuotesTest()
    End Sub

    <Test()> _
    Public Sub SuccessWithMTA()
        GetInstalmentQuotesTest(nTestCaseScenario:=TestCaseScenario.MTA)
    End Sub

#End Region

#Region " Missing Data "

    <Test()> _
    Public Sub InvalidData_Missing_BranchCode()
        GetInstalmentQuotesTest(eMissingData:=enumMissingData.BranchCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_InsuranceFileKey()
        GetInstalmentQuotesTest(eMissingData:=enumMissingData.InsuranceFileKey)
    End Sub

#End Region

#Region " Invalid Lookup "

    <Test()> _
    Public Sub InvalidData_BranchCode()
        GetInstalmentQuotesTest(eInvalidLookup:=enumInvalidLookup.BranchCode)
    End Sub

#End Region

#Region " STS Business Rules "

    Public Sub STSBusiness_InsFileCnt()
        GetInstalmentQuotesTest(eSTSBusinessError:=enumSTSBusinessError.InvalidInsuranceFileCnt)
    End Sub
#End Region

End Class
