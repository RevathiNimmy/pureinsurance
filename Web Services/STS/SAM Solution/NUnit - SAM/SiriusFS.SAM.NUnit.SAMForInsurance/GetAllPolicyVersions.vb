Imports NUnit.Framework

<TestFixture()> _
Public Class GetAllPolicyVersions
    Inherits BaseTest

#Region "Private Declarations"

    Private m_nInsuranceFolderCnt As Integer
    Private m_nInsuranceFileCnt As Integer
    Private m_btQuoteTimeStamp As Byte() = kanEmptyTimeStamp

    Private m_oTestData As New TestData

    Private Enum enumMissingData
        None
        'AgentKey
        BranchCode
        InsuranceFolderKey
    End Enum

    Private Enum enumInvalidLookup
        None
        BranchCode
    End Enum

    Private Enum enumSTSBusinessError
        None
        'InvAgentKey
        InvInsuranceFolderKey
    End Enum

#End Region

#Region "Setup Preconditions"

    Private Sub AddQuote()

        Dim oAddQuote As New AddQuote
        oAddQuote.SupportMethod(m_nInsuranceFileCnt, m_nInsuranceFolderCnt, "", m_btQuoteTimeStamp)

    End Sub

#End Region

#Region "Private Test Methods"

    Private Sub GetAllPolicyVersionsTest( _
        Optional ByVal eMissingData As enumMissingData = enumMissingData.None, _
        Optional ByVal eInvalidLookup As enumInvalidLookup = enumInvalidLookup.None, _
        Optional ByVal eSTSBusinessError As enumSTSBusinessError = enumSTSBusinessError.None, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.GetAllPolicyVersionsRequestType
        Dim oResponse As ProxyWS.GetAllPolicyVersionsResponseType
        Dim nLookupError As Integer = 102
        Dim nBusinessError As Integer = 224

        ' If an Insurance Folder was specified in the test data, find that one,
        ' else create a new one
        If m_oTestData.InsuranceFolderCnt = 0 Then
            AddQuote()
        Else
            m_nInsuranceFolderCnt = m_oTestData.InsuranceFolderCnt
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
            End With

            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.GetAllPolicyVersions(oRequest)
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
                    SAMTest.AssertCallSucceededWithResults(oResponse, oResponse.Policies)
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

    <Test()> _
    Public Sub Success()
        GetAllPolicyVersionsTest()
    End Sub

#End Region

#Region "Missing Data"

    <Test()> _
    Public Sub InvalidData_Missing_BranchCode()
        GetAllPolicyVersionsTest(eMissingData:=enumMissingData.BranchCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_InsuranceFolderKey()
        GetAllPolicyVersionsTest(eMissingData:=enumMissingData.InsuranceFolderKey)
    End Sub

#End Region

#Region "Invalid Lookup"

    <Test()> _
    Public Sub InvalidData_BranchCode()
        GetAllPolicyVersionsTest(eInvalidLookup:=enumInvalidLookup.BranchCode)
    End Sub

#End Region

#Region "STS Business Rules"

    <Test()> _
    Public Sub STSBusiness_InsFolderCnt()
        GetAllPolicyVersionsTest(eSTSBusinessError:=enumSTSBusinessError.InvInsuranceFolderKey)
    End Sub

#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        GetAllPolicyVersionsTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        GetAllPolicyVersionsTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        GetAllPolicyVersionsTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        GetAllPolicyVersionsTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class
