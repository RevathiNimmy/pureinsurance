Imports NUnit.Framework

<TestFixture()> _
Public Class GetPartySummary
    Inherits BaseTest

#Region "Private Declarations"

    Private m_nPartyCnt As Integer
    Private m_btPartyTimeStamp() As Byte

    Private m_oTestData As New TestData

    Private Enum enumMissingData
        None
        'AgentKey
        BranchCode
        PartyKey
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

    Private Sub AddParty()

        Dim oAddParty As New AddParty
        oAddParty.SupportMethod(m_nPartyCnt, m_btPartyTimeStamp)

    End Sub

#End Region

#Region "Private Test Methods"

    Private Sub GetPartySummaryTest( _
        Optional ByVal eMissingData As enumMissingData = enumMissingData.None, _
        Optional ByVal eInvalidLookup As enumInvalidLookup = enumInvalidLookup.None, _
        Optional ByVal eSTSBusinessError As enumSTSBusinessError = enumSTSBusinessError.None, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.GetPartySummaryRequestType
        Dim oResponse As ProxyWS.GetPartySummaryResponseType
        Dim nLookupError As Integer = 102
        Dim nBusinessError As Integer = 274

        ' If an PartyCnt was specified in the test data, find that one,
        ' else create a new one
        If m_oTestData.PartyCnt = 0 Then
            AddParty()
        Else
            m_nPartyCnt = m_oTestData.PartyCnt
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
                If eMissingData <> enumMissingData.PartyKey Then
                    .PartyKey = m_nPartyCnt
                End If
            End With

            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.GetPartySummary(oRequest)
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
                    Assert.IsNotNull(.Item, "No details returned")
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
        GetPartySummaryTest()
    End Sub

#End Region

#Region "Missing Data"

    <Test()> _
    Public Sub InvalidData_Missing_BranchCode()
        GetPartySummaryTest(eMissingData:=enumMissingData.BranchCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_PartyKey()
        GetPartySummaryTest(eMissingData:=enumMissingData.PartyKey)
    End Sub

#End Region

#Region "Invalid Lookup"

    <Test()> _
    Public Sub InvalidData_BranchCode()
        GetPartySummaryTest(eInvalidLookup:=enumInvalidLookup.BranchCode)
    End Sub

#End Region

#Region "STS Business Rules"

#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        GetPartySummaryTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        GetPartySummaryTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        GetPartySummaryTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        GetPartySummaryTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class
