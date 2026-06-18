Imports NUnit.Framework

<TestFixture()> _
Public Class RunDefaultRulesAdd
    Inherits BaseTest

#Region "Private Declarations"

    Private m_nPartyCnt As Integer
    Private m_nInsuranceFolderCnt As Integer
    Private m_nInsuranceFileCnt As Integer
    Private m_btQuoteTimeStamp() As Byte
    Private m_sRiskDataXML As String
    Private m_nRiskCnt As Integer
    Private m_oTestData As New TestData

    Private Enum enumMissingData
        AgentKey
        BranchCode
        ScreenCode
        XMLDataSet
        None
    End Enum

    Private Enum enumInvalidLookup
        None
        BranchCode
        ScreenCode
    End Enum

    Private Enum enumSTSBusinessError
        None
        InvalidAgentKey
    End Enum

#End Region

#Region "Setup preconditions"

    Private Sub AddRisk()

        Dim oAddRisk As New AddRisk
        oAddRisk.SupportMethod(m_nInsuranceFileCnt, _
                                m_nInsuranceFolderCnt, _
                                m_nRiskCnt, _
                                m_sRiskDataXML, _
                                m_btQuoteTimeStamp, _
                                False)

    End Sub

#End Region

#Region "Private Test method"

    Private Sub RunDefaultRulesAddTest( _
        Optional ByVal eMissingData As enumMissingData = enumMissingData.None, _
        Optional ByVal eInvalidLookup As enumInvalidLookup = enumInvalidLookup.None, _
        Optional ByVal eSTSBusinessError As enumSTSBusinessError = enumSTSBusinessError.None, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.RunDefaultRulesAddRequestType
        Dim oResponse As ProxyWS.RunDefaultRulesAddResponseType
        Dim nLookupError As Integer = 102
        Dim nBusinessError As Integer = 274

        Try
            If eMissingData = enumMissingData.None And _
               eInvalidLookup = enumInvalidLookup.None Then
                AddRisk()
            Else
                m_sRiskDataXML = m_oTestData.RiskDataXML
            End If

            With oRequest
                If eMissingData <> enumMissingData.BranchCode Then
                    If eInvalidLookup <> enumInvalidLookup.BranchCode Then
                        .BranchCode = m_oTestData.BranchCode
                    Else
                        .BranchCode = m_oTestData.InvalidLookupCode
                        nLookupError = 210
                    End If
                End If
                If eMissingData <> enumMissingData.ScreenCode Then
                    If eInvalidLookup <> enumInvalidLookup.ScreenCode Then
                        .ScreenCode = m_oTestData.ScreenCode
                    Else
                        .ScreenCode = m_oTestData.InvalidLookupCode
                    End If
                End If
                If eMissingData <> enumMissingData.XMLDataSet Then
                    .XMLDataSet = m_sRiskDataXML
                End If

            End With

            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.RunDefaultRulesAdd(oRequest)

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
        RunDefaultRulesAddTest()
    End Sub

#End Region

#Region "Invalid Data"

#Region "Mandatory Data Missing"

    <Test()> _
    Public Sub InvalidData_Missing_BranchCode()
        RunDefaultRulesAddTest(eMissingData:=enumMissingData.BranchCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_ScreenCode()
        RunDefaultRulesAddTest(eMissingData:=enumMissingData.ScreenCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_XMLDataSet()
        RunDefaultRulesAddTest(eMissingData:=enumMissingData.XMLDataSet)
    End Sub

#End Region

#Region "Invalid Format"

#End Region

#Region "Invalid List Value"

    <Test()> _
    Public Sub InvalidData_BranchCode()
        RunDefaultRulesAddTest(eInvalidLookup:=enumInvalidLookup.BranchCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_ScreenCode()
        RunDefaultRulesAddTest(eInvalidLookup:=enumInvalidLookup.ScreenCode)
    End Sub

#End Region

#End Region

#Region "STS Business Rules"

    <Test(), Ignore("To be implemented")> _
     Public Sub STSBusinessRules_ValidationRulesReferred()

    End Sub
    <Test(), Ignore("To be implemented")> _
    Public Sub STSBusinessRules_ValidationRulesDeclined()

    End Sub
    <Test(), Ignore("To be implemented")> _
    Public Sub STSBusinessRules_UALRulesReferred()

    End Sub
    <Test(), Ignore("To be implemented")> _
    Public Sub STSBusinessRules_UALRulesDeclined()

    End Sub
    <Test(), Ignore("To be implemented")> _
    Public Sub STSBusinessRules_RatingRulesReferred()

    End Sub
    <Test(), Ignore("To be implemented")> _
    Public Sub STSBusinessRules_RatingRulesDeclined()

    End Sub

#End Region

#Region "Sirius Back Office"

#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        RunDefaultRulesAddTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        RunDefaultRulesAddTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        RunDefaultRulesAddTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        RunDefaultRulesAddTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class
