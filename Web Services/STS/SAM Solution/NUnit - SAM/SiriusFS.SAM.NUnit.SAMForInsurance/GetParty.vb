Imports NUnit.Framework

<TestFixture()> _
Public Class GetParty
    Inherits BaseTest

#Region "Private Declarations"

    Private m_nPartyCnt As Integer
    Private m_btPartyTimeStamp() As Byte

    Private m_oTestData As New TestData

    Private Enum enumMissingData
        None
        AgentKey
        BranchCode
        PartyKey
    End Enum

    Private Enum enumInvalidLookup
        None
        BranchCode
    End Enum

    Private Enum enumSTSBusinessError
        None
        InvalidAgentKey
    End Enum

#End Region

#Region "Setup Preconditions"

    Private Sub AddParty()

        Dim oAddParty As New AddParty
        oAddParty.SupportMethod(m_nPartyCnt, m_btPartyTimeStamp)

    End Sub

#End Region

#Region "Private Test Methods"

    Private Sub GetPartyTest( _
        Optional ByVal eMissingData As enumMissingData = enumMissingData.None, _
        Optional ByVal eInvalidLookup As enumInvalidLookup = enumInvalidLookup.None, _
        Optional ByVal eSTSBusinessError As enumSTSBusinessError = enumSTSBusinessError.None, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.GetPartyRequestType
        Dim oResponse As ProxyWS.GetPartyResponseType
        Dim nLookupError As Integer = 102
        Dim nBusinessError As Integer = 274

        ' If an PartyCnt was specified in the test data, find that one,
        ' else create a new one
        If m_oTestData.PartyCnt = 0 And m_nPartyCnt = 0 Then
            AddParty()
        Else
            If m_nPartyCnt = 0 Then
                m_nPartyCnt = m_oTestData.PartyCnt
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
                    If eMissingData <> enumMissingData.PartyKey Then
                        .PartyKey = m_nPartyCnt
                    End If
                End With

                SetWSETestCaseScenario(nWSETestCaseScenario)
                oResponse = oProxy.GetParty(oRequest)
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
                        m_btPartyTimeStamp = .PartyTimestamp
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

    Public Sub SupportMethod(ByRef r_btPartyTimeStamp() As Byte)
        GetPartyTest()
        r_btPartyTimeStamp = m_btPartyTimeStamp
    End Sub

    Public Sub SupportMethod(ByVal PartyKey As Integer, _
    ByRef r_btPartyTimeStamp() As Byte)
        m_nPartyCnt = PartyKey
        GetPartyTest()
        r_btPartyTimeStamp = m_btPartyTimeStamp
    End Sub


    <Test()> _
    Public Sub Success()
        GetPartyTest()
    End Sub

#End Region

#Region "Missing Data"

    <Test()> _
    Public Sub InvalidData_Missing_BranchCode()
        GetPartyTest(eMissingData:=enumMissingData.BranchCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_PartyKey()
        GetPartyTest(eMissingData:=enumMissingData.PartyKey)
    End Sub

#End Region

#Region "Invalid Lookup"

    <Test()> _
    Public Sub InvalidData_BranchCode()
        GetPartyTest(eInvalidLookup:=enumInvalidLookup.BranchCode)
    End Sub

#End Region

#Region "STS Business Rules"


#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        GetPartyTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        GetPartyTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        GetPartyTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        GetPartyTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class
