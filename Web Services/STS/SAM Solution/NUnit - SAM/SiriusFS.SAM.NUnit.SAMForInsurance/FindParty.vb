Imports NUnit.Framework

<TestFixture()> _
Public Class FindParty
    Inherits BaseTest

#Region " Private Declarations "

    Private m_oTestData As New TestData

    Private Enum enumMissingData
        None
        'AgentKey
        BranchCode
    End Enum

    Private Enum enumInvalidLookup
        None
        BranchCode
        PartyType
    End Enum

    Private Enum enumSTSBusinessError
        None
        'InvalidAgentKey
    End Enum

#End Region

#Region " Private Methods "

    Private Sub FindPartyTest( _
        Optional ByVal eMissingData As enumMissingData = enumMissingData.None, _
        Optional ByVal eInvalidLookup As enumInvalidLookup = enumInvalidLookup.None, _
        Optional ByVal eSTSBusinessError As enumSTSBusinessError = enumSTSBusinessError.None, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.FindPartyRequestType
        Dim oResponse As ProxyWS.FindPartyResponseType
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
                .AddressLine1 = m_oTestData.FPAddressLine1
                .AddressLine2 = m_oTestData.FPAddressLine2
                .AddressLine3 = m_oTestData.FPAddressLine3
                .AddressLine4 = m_oTestData.FPAddressLine4
                .AlternativeId = m_oTestData.FPAlternativeId
                .AreaCode = m_oTestData.FPAreaCode
                .DateOfBirth = m_oTestData.FPDateOfBirth
                .DateOfBirthSpecified = (m_oTestData.FPDateOfBirth <> New Date)
                .Firstname = m_oTestData.FPFirstName
                .Name = m_oTestData.FPName
                If eInvalidLookup = enumInvalidLookup.PartyType Then
                    .PartyType = 0
                Else
                    .PartyType = m_oTestData.FPPartyType
                    .PartyTypeSpecified = True
                End If
                .PolicyRef = m_oTestData.FPPolicyRef
                .PostCode = m_oTestData.FPPostCode
                .RiskIndex = m_oTestData.FPRiskIndex
                .Shortname = m_oTestData.FPShortName
                .TelephoneNumber = m_oTestData.FPTelephoneNumber

            End With

            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.FindParty(oRequest)
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
                    SAMTest.AssertCallSucceededWithResults(oResponse, oResponse.Parties)
                    'Dim oXML As New Xml.XmlDocument
                    'oXML.LoadXml(.ResultDataset.OuterXml.ToString)

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
        FindPartyTest()
    End Sub

#End Region

#Region " Missing Data "

    <Test()> _
    Public Sub InvalidData_Missing_BranchCode()
        FindPartyTest(eMissingData:=enumMissingData.BranchCode)
    End Sub

#End Region

#Region " Invalid Lookup "

    <Test()> _
    Public Sub InvalidData_BranchCode()
        FindPartyTest(eInvalidLookup:=enumInvalidLookup.BranchCode)
    End Sub
    '<Test()> _
    'Public Sub InvalidData_PartyType()
    '    FindPartyTest(eInvalidLookup:=enumInvalidLookup.PartyType)
    'End Sub

#End Region

#Region " STS Business Rule "

#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        FindPartyTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        FindPartyTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        FindPartyTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        FindPartyTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class
