Imports NUnit.Framework

<TestFixture()> _
Public Class UpdateQuote
    Inherits BaseTest

#Region " Private Declarations "

    Private m_nInsuranceFolderCnt As Integer
    Private m_nInsuranceFileCnt As Integer
    Private m_btQuoteTimeStamp() As Byte

    Private m_oTestData As New TestData

    Private Enum enumMissingData
        None
        'AgentKey
        BranchCode
        InsuranceFolderKey
        InsuranceFileKey
        CoverStartDate
        CoverEndDate
        QuoteTimeStamp
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
    End Enum

#End Region

#Region " Setup Preconditions "

    Private Sub AddQuote()

        Dim oAddQuote As New AddQuote
        oAddQuote.SupportMethod(m_nInsuranceFileCnt, m_nInsuranceFolderCnt, "", m_btQuoteTimeStamp)

    End Sub

    Private Sub GetHeaderAndSummariesByKey()

        Dim oGetHeader As New GetHeaderAndSummariesByKey
        oGetHeader.SupportMethod(m_nInsuranceFolderCnt, m_nInsuranceFileCnt, m_btQuoteTimeStamp)

    End Sub

#End Region

#Region " Private Test Methods "

    Private Sub UpdateQuoteTest( _
        Optional ByVal eMissingData As enumMissingData = enumMissingData.None, _
        Optional ByVal eInvalidLookup As enumInvalidLookup = enumInvalidLookup.None, _
        Optional ByVal eSTSBusinessError As enumSTSBusinessError = enumSTSBusinessError.None, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.UpdateQuoteRequestType
        Dim oResponse As ProxyWS.UpdateQuoteResponseType
        Dim nLookupError As Integer = 102
        Dim nBusinessError As Integer = 224

        ' No Insurance File Cnt was provided in the test data so create a brand
        ' new risk
        If m_oTestData.InsuranceFileCnt = 0 Then
            AddQuote()
        Else
            ' Use the provided Insurance File Cnt to get the header (support
            ' method shared the test data)
            GetHeaderAndSummariesByKey()
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
                If eMissingData <> enumMissingData.InsuranceFolderKey Then

                    If eSTSBusinessError = enumSTSBusinessError.InvInsuranceFolderKey Then
                        .InsuranceFolderKey = m_oTestData.InvalidCnt
                    Else
                        .InsuranceFolderKey = m_nInsuranceFolderCnt
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
                If eMissingData <> enumMissingData.QuoteTimeStamp Then
                    .QuoteTimeStamp = m_btQuoteTimeStamp
                End If
                If eMissingData <> enumMissingData.CoverEndDate Then
                    .CoverEndDate = m_oTestData.CoverEndDate
                End If
                If eMissingData <> enumMissingData.CoverStartDate Then
                    .CoverStartDate = m_oTestData.CoverStartDate
                End If
                .Description = m_oTestData.QuoteDescription
                .AnalysisCode = m_oTestData.UpdateAnalysisCode
            End With

            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.UpdateQuote(oRequest)
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
                    Assert.IsNotNull(oResponse.QuoteTimeStamp, "No QuoteTimeStamp Returned")
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

#Region " Success "

    <Test()> _
    Public Sub Success()
        UpdateQuoteTest()
    End Sub

#End Region

#Region " Missing Data "

    <Test()> _
    Public Sub InvalidData_Missing_BranchCode()
        UpdateQuoteTest(eMissingData:=enumMissingData.BranchCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_InsuranceFileKey()
        UpdateQuoteTest(eMissingData:=enumMissingData.InsuranceFileKey)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_InsuranceFolderKey()
        UpdateQuoteTest(eMissingData:=enumMissingData.InsuranceFolderKey)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_QuoteTimeStamp()
        UpdateQuoteTest(eMissingData:=enumMissingData.QuoteTimeStamp)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_CoverEndDate()
        UpdateQuoteTest(eMissingData:=enumMissingData.CoverEndDate)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_CoverStartDate()
        UpdateQuoteTest(eMissingData:=enumMissingData.CoverStartDate)
    End Sub

#End Region

#Region " Invalid Lookup "

    <Test()> _
    Public Sub InvalidData_BranchCode()
        UpdateQuoteTest(eInvalidLookup:=enumInvalidLookup.BranchCode)
    End Sub

#End Region

#Region " STS Business Rules "

    <Test()> _
    Public Sub STSBusiness_InsFolderCnt()
        UpdateQuoteTest(eSTSBusinessError:=enumSTSBusinessError.InvInsuranceFolderKey)
    End Sub
    <Test()> _
    Public Sub STSBusiness_InsFileCnt()
        UpdateQuoteTest(eSTSBusinessError:=enumSTSBusinessError.InvInsuranceFileKey)
    End Sub
    <Test()> _
    Public Sub STSBusiness_InsFileFolder()
        UpdateQuoteTest(eSTSBusinessError:=enumSTSBusinessError.InvInsuranceFileFolder)
    End Sub

#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        UpdateQuoteTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        UpdateQuoteTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        UpdateQuoteTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        UpdateQuoteTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class
