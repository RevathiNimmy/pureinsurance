Imports NUnit.Framework

<TestFixture()> _
Public Class GetRisk
    Inherits BaseTest

#Region "Private Declarations"

    Private m_nInsuranceFolderCnt As Integer
    Private m_nInsuranceFileCnt As Integer
    Private m_nRiskCnt As Integer
    Private m_btQuoteTimeStamp() As Byte
    Private m_xmlDataset As String = String.Empty

    Private m_oTestData As New TestData

    Private Enum enumMissingData
        None
        'AgentKey
        BranchCode
        InsuranceFolderKey
        InsuranceFileKey
        QuoteTimeStamp
        RiskKey
    End Enum

    Private Enum enumInvalidLookup
        None
        BranchCode
    End Enum

    Private Enum enumSTSBusinessError
        None
        'InvalidAgentKey
        InvInsuranceFolderKey
        InvInsuranceFileKey
        InvInsuranceFileFolder
        InvRiskKey
        InvInsuranceFileRisk
    End Enum

#End Region

#Region "Setup Preconditions"

    Private Sub AddRisk()

        Dim oAddRisk As New AddRisk
        oAddRisk.SupportMethod(m_nInsuranceFileCnt, m_nInsuranceFolderCnt, m_nRiskCnt, "", m_btQuoteTimeStamp)

    End Sub

    Private Sub GetHeaderAndSummariesByKey()

        Dim oGetHeader As New GetHeaderAndSummariesByKey
        oGetHeader.SupportMethod(m_nInsuranceFolderCnt, m_nInsuranceFileCnt, m_btQuoteTimeStamp)

    End Sub

#End Region

#Region "Private Test Methods"

    Private Sub GetRiskTest( _
        Optional ByVal eMissingData As enumMissingData = enumMissingData.None, _
        Optional ByVal eInvalidLookup As enumInvalidLookup = enumInvalidLookup.None, _
        Optional ByVal eSTSBusinessError As enumSTSBusinessError = enumSTSBusinessError.None, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.GetRiskRequestType
        Dim oResponse As ProxyWS.GetRiskResponseType
        Dim nLookupError As Integer = 102
        Dim nBusinessError As Integer = 224


        ' No Insurance File Cnt was provided in the test data so create a brand
        ' new risk
        If m_oTestData.InsuranceFileCnt = 0 And m_nInsuranceFileCnt = 0 Then
            AddRisk()
        Else
            ' No Risk Cnt was provided in the test data so create a brand
            ' new risk
            If m_oTestData.RiskCnt = 0 And m_nRiskCnt = 0 Then
                AddRisk()
            End If
        End If

        Try
            With oRequest
                If eMissingData <> enumMissingData.BranchCode Then
                    If eInvalidLookup = enumInvalidLookup.BranchCode Then
                        .BranchCode = m_oTestData.InvalidLookupCode
                        nLookupError = 210
                    Else
                        .BranchCode = m_oTestData.BranchCode
                    End If
                End If
                If eMissingData <> enumMissingData.QuoteTimeStamp Then
                    .QuoteTimeStamp = m_btQuoteTimeStamp
                End If

                If eMissingData <> enumMissingData.InsuranceFolderKey Then
                    If eSTSBusinessError <> enumSTSBusinessError.InvInsuranceFolderKey Then
                        .InsuranceFolderKey = m_nInsuranceFolderCnt
                    Else
                        .InsuranceFolderKey = m_oTestData.InvalidCnt
                    End If
                End If

                If eMissingData <> enumMissingData.InsuranceFileKey Then
                    Select Case eSTSBusinessError
                        Case enumSTSBusinessError.InvInsuranceFileKey
                            .InsuranceFileKey = m_oTestData.InvalidCnt
                        Case enumSTSBusinessError.InvInsuranceFileFolder
                            .InsuranceFolderKey = m_oTestData.InvalidInsFileFolderCnt
                            .InsuranceFileKey = m_nInsuranceFileCnt
                            nBusinessError = 212
                        Case Else
                            .InsuranceFileKey = m_nInsuranceFileCnt
                    End Select
                End If

                If eMissingData <> enumMissingData.RiskKey Then
                    Select Case eSTSBusinessError
                        Case enumSTSBusinessError.InvRiskKey
                            .RiskKey = m_oTestData.InvalidCnt
                            nBusinessError = 229
                        Case enumSTSBusinessError.InvInsuranceFileRisk
                            .RiskKey = m_oTestData.InvalidInsFileRiskCnt
                            nBusinessError = 219
                        Case Else
                            .RiskKey = m_nRiskCnt
                    End Select
                End If

            End With

            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.GetRisk(oRequest)
            With oResponse

                m_xmlDataset = .XMLDataSet
                m_btQuoteTimeStamp = .QuoteTimeStamp

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
                    Assert.AreNotEqual("", .XMLDataSet, "No details returned")
                    'TODO: additional assert test
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

#Region "Success"


    Public Overloads Sub SupportMethod(ByVal riskKey As Integer, ByVal insuranceFolderCnt As Integer, ByVal insuranceFileCnt As Integer, ByRef quoteTimeStamp() As Byte, ByRef xmlDataset As String)

        m_btQuoteTimeStamp = quoteTimeStamp
        m_nInsuranceFolderCnt = insuranceFolderCnt
        m_nInsuranceFileCnt = insuranceFileCnt
        m_nRiskCnt = riskKey
        GetRiskTest()
        quoteTimeStamp = m_btQuoteTimeStamp
        xmlDataset = m_xmlDataset

    End Sub

    <Test()> _
    Public Sub Success()
        GetRiskTest()
    End Sub

#End Region

#Region "Missing Data"

    <Test()> _
    Public Sub InvalidData_Missing_BranchCode()
        GetRiskTest(eMissingData:=enumMissingData.BranchCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_InsuranceFileKey()
        GetRiskTest(eMissingData:=enumMissingData.InsuranceFileKey)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_InsuranceFolderKey()
        GetRiskTest(eMissingData:=enumMissingData.InsuranceFolderKey)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_QuoteTimeStamp()
        GetRiskTest(eMissingData:=enumMissingData.QuoteTimeStamp)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_RiskKey()
        GetRiskTest(eMissingData:=enumMissingData.RiskKey)
    End Sub

#End Region

#Region "Invalid Lookup"

    <Test()> _
    Public Sub InvalidData_BranchCode()
        GetRiskTest(eInvalidLookup:=enumInvalidLookup.BranchCode)
    End Sub

#End Region

#Region "STS Business Rules"

    <Test()> _
    Public Sub STSBusiness_InsFolderCnt()
        GetRiskTest(eSTSBusinessError:=enumSTSBusinessError.InvInsuranceFolderKey)
    End Sub
    <Test()> _
    Public Sub STSBusiness_InsFileCnt()
        GetRiskTest(eSTSBusinessError:=enumSTSBusinessError.InvInsuranceFileKey)
    End Sub
    <Test()> _
    Public Sub STSBusiness_RiskCnt()
        GetRiskTest(eSTSBusinessError:=enumSTSBusinessError.InvRiskKey)
    End Sub
    <Test()> _
    Public Sub STSBusiness_InsFileFolder()
        GetRiskTest(eSTSBusinessError:=enumSTSBusinessError.InvInsuranceFileFolder)
    End Sub
    <Test()> _
    Public Sub STSBusiness_InsFileRisk()
        GetRiskTest(eSTSBusinessError:=enumSTSBusinessError.InvInsuranceFileRisk)
    End Sub

#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        GetRiskTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        GetRiskTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        GetRiskTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        GetRiskTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class
