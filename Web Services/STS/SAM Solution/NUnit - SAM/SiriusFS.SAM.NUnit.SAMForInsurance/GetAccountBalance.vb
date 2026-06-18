Imports NUnit.Framework

<TestFixture()> _
Public Class GetAccountBalance
    Inherits BaseTest

#Region "Private Declarations"

    Private m_nAddressCnt As Integer
    Private m_oTestData As New TestData

    Private Enum enumMissingData
        None
        'AgentKey
        BranchCode
        AddressKey
    End Enum

    Private Enum enumInvalidLookup
        None
        BranchCode
    End Enum

    Private Enum enumSTSBusinessError
        None
        'InvalidAgentKey
    End Enum

#End Region

#Region "Setup Preconditions"

    Private Sub AddAddress()

        Dim oAddress As New AddAddress
        oAddress.SupportMethod(m_nAddressCnt)

    End Sub

#End Region

#Region "Private Test Methods"

    Private Sub GetAccountBalanceTest( _
        Optional ByVal eMissingData As enumMissingData = enumMissingData.None, _
        Optional ByVal eInvalidLookup As enumInvalidLookup = enumInvalidLookup.None, _
        Optional ByVal eSTSBusinessError As enumSTSBusinessError = enumSTSBusinessError.None, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.GetAccountBalanceRequestType
        Dim oResponse As ProxyWS.GetAccountBalanceResponseType
        Dim nLookupError As Integer = 102
        Dim nBusinessError As Integer = 274

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
            End With

            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.GetAccountBalance(oRequest)
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
                    'TODO: additional assert test
                    SAMTest.AssertCallSucceededWithResults(oResponse, oResponse.AccountBalance)
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
        GetAccountBalanceTest()
    End Sub

#End Region

#Region "Missing Data"

    <Test()> _
    Public Sub InvalidData_Missing_BranchCode()
        GetAccountBalanceTest(eMissingData:=enumMissingData.BranchCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_AddressKey()
        GetAccountBalanceTest(eMissingData:=enumMissingData.AddressKey)
    End Sub

#End Region

#Region "Invalid Lookup"

    <Test()> _
    Public Sub InvalidData_BranchCode()
        GetAccountBalanceTest(eInvalidLookup:=enumInvalidLookup.BranchCode)
    End Sub

#End Region

#Region "STS Business Rule"

#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        GetAccountBalanceTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        GetAccountBalanceTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        GetAccountBalanceTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        GetAccountBalanceTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class
