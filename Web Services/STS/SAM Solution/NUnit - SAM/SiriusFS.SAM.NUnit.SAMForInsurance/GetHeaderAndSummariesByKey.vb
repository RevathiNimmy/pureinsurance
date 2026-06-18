Imports NUnit.Framework

<TestFixture()> _
Public Class GetHeaderAndSummariesByKey
    Inherits BaseTest

#Region "Private Declarations"

    Private m_nInsuranceFolderCnt As Integer
    Private m_nInsuranceFileCnt As Integer
    Private m_btQuoteTimeStamp As Byte() = kanEmptyTimeStamp
    Private m_Risks() As ProxyWS.BaseGetHeaderAndSummariesResponseTypeRow

    Private m_oTestData As New TestData

    Private Enum enumMissingData
        None
        'AgentKey
        BranchCode
        InsuranceFileKey
    End Enum

    Private Enum enumInvalidLookup
        None
        BranchCode
    End Enum

    Private Enum enumSTSBusinessError
        None
        'InvAgentKey
        InvInsuranceFileKey
    End Enum

#End Region

#Region "Setup Preconditions"

    Private Sub AddQuote()

        Dim oAddQuote As New AddQuote
        oAddQuote.SupportMethod(m_nInsuranceFileCnt, m_nInsuranceFolderCnt, "", m_btQuoteTimeStamp)

    End Sub

#End Region

#Region "Private Test Methods"

    Private Sub GetHeaderAndSummariesByKeyTest( _
        Optional ByVal eMissingData As enumMissingData = enumMissingData.None, _
        Optional ByVal eInvalidLookup As enumInvalidLookup = enumInvalidLookup.None, _
        Optional ByVal eSTSBusinessError As enumSTSBusinessError = enumSTSBusinessError.None, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.GetHeaderAndSummariesByKeyRequestType
        Dim oResponse As ProxyWS.GetHeaderAndSummariesByKeyResponseType
        Dim nLookupError As Integer = 102
        Dim nBusinessError As Integer = 224

        ' If an Insurance File was specified in the test data, find that one,
        ' else create a new one
        If m_nInsuranceFileCnt = 0 Then
            AddQuote()
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
                If eMissingData <> enumMissingData.InsuranceFileKey Then
                    If eSTSBusinessError = enumSTSBusinessError.InvInsuranceFileKey Then
                        .InsuranceFileKey = m_oTestData.InvalidCnt
                    Else
                        .InsuranceFileKey = m_nInsuranceFileCnt
                    End If
                End If
            End With

            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.GetHeaderAndSummariesByKey(oRequest)
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
                    Assert.AreNotEqual(0, .InsuranceFolderKey, "No details returned")
                    'TODO: additional assert test
                    m_btQuoteTimeStamp = .QuoteTimeStamp
                    m_nInsuranceFolderCnt = .InsuranceFolderKey
                    m_Risks = .Risks
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

    Public Overloads Sub SupportMethod(ByRef r_nInsuranceFolderCnt As Integer, ByRef r_nInsuranceFileCnt As Integer, ByRef r_btQuoteTimeStamp() As Byte)

        Call SupportMethod(r_nInsuranceFolderCnt, r_nInsuranceFileCnt, r_btQuoteTimeStamp, Nothing)

    End Sub

    Public Overloads Sub SupportMethod(ByRef r_nInsuranceFolderCnt As Integer, ByRef r_nInsuranceFileCnt As Integer, ByRef r_btQuoteTimeStamp() As Byte, ByRef r_Risks() As ProxyWS.BaseGetHeaderAndSummariesResponseTypeRow)

        m_nInsuranceFileCnt = r_nInsuranceFileCnt
        m_nInsuranceFolderCnt = r_nInsuranceFolderCnt

        GetHeaderAndSummariesByKeyTest()

        r_nInsuranceFolderCnt = m_nInsuranceFolderCnt
        r_nInsuranceFileCnt = m_nInsuranceFileCnt
        r_btQuoteTimeStamp = m_btQuoteTimeStamp
        r_Risks = m_Risks

    End Sub

    <Test()> _
    Public Sub Success()
        GetHeaderAndSummariesByKeyTest()
    End Sub

#End Region

#Region "Missing Data"

    <Test()> _
    Public Sub InvalidData_Missing_BranchCode()
        GetHeaderAndSummariesByKeyTest(eMissingData:=enumMissingData.BranchCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_InsuranceFileKey()
        GetHeaderAndSummariesByKeyTest(eMissingData:=enumMissingData.InsuranceFileKey)
    End Sub

#End Region

#Region "Invalid Lookup"

    <Test()> _
    Public Sub InvalidData_BranchCode()
        GetHeaderAndSummariesByKeyTest(eInvalidLookup:=enumInvalidLookup.BranchCode)
    End Sub

#End Region

#Region "STS Business Rules"

    <Test()> _
    Public Sub STSBusiness_InsFolderCnt()
        GetHeaderAndSummariesByKeyTest(eSTSBusinessError:=enumSTSBusinessError.InvInsuranceFileKey)
    End Sub

#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        GetHeaderAndSummariesByKeyTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        GetHeaderAndSummariesByKeyTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        GetHeaderAndSummariesByKeyTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        GetHeaderAndSummariesByKeyTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class
