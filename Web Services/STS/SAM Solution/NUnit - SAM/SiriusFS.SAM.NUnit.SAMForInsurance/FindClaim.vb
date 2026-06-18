Imports NUnit.Framework

<TestFixture()> _
Public Class FindClaim
    Inherits BaseTest

#Region "Private Declarations"

    Private m_oTestData As New TestData

    Private Enum enumMissingData
        None
        BranchCode
    End Enum

    Private Enum enumInvalidLookup
        None
        BranchCode
        InvalidLossDateFrom
        InvalidLossDateTo
    End Enum

    Private Enum enumSTSBusinessError
        None
        InvalidLossDateDifference
    End Enum

#End Region

#Region "Private Methods"

    Private Sub FindClaimTest( _
        Optional ByVal eMissingData As enumMissingData = enumMissingData.None, _
        Optional ByVal eInvalidLookup As enumInvalidLookup = enumInvalidLookup.None, _
        Optional ByVal eSTSBusinessError As enumSTSBusinessError = enumSTSBusinessError.None, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.FindClaimRequestType()
        Dim oResponse As ProxyWS.FindClaimResponseType
        Dim nLookupError As Integer = 102
        Dim nBusinessError As Integer = 274

        Try
            With oRequest
                If eMissingData <> enumMissingData.BranchCode Then
                    If eInvalidLookup = enumInvalidLookup.BranchCode Then
                        .BranchCode = m_oTestData.InvalidLookupCode
                        nLookupError = 210
                    Else
                        .BranchCode = m_oTestData.FindClaimBranchCode
                    End If
                End If
                .ClaimNumber = m_oTestData.FindClaimClaimNumber
                .InsuranceFileRef = m_oTestData.FindClaimInsuranceFileRef
                .ClientShortName = m_oTestData.FindClaimClientShortName
                If eInvalidLookup <> enumInvalidLookup.InvalidLossDateFrom Then
                    .LossDateFrom = m_oTestData.FindClaimLossDateFrom
                    .LossDateFromSpecified = (m_oTestData.FindClaimLossDateFrom <> New Date)
                Else
                    .LossDateFromSpecified = True
                End If

                If eInvalidLookup <> enumInvalidLookup.InvalidLossDateTo Then
                    .LossDateTo = m_oTestData.FindClaimLossDateTo
                    .LossDateToSpecified = (m_oTestData.FindClaimLossDateTo <> New Date)
                Else
                    .LossDateToSpecified = True
                End If

                If eSTSBusinessError = enumSTSBusinessError.InvalidLossDateDifference Then
                    .LossDateFrom = New Date(2000, 12, 12)
                    .LossDateFromSpecified = True

                    .LossDateTo = New Date(1999, 12, 12)
                    .LossDateToSpecified = True
                End If

                .RiskIndex = m_oTestData.FindClaimRiskIndex



            End With

            oRequest.BranchCode = "HEADOFF"
            oRequest.ClaimNumber = Nothing
            oRequest.ClientShortName = Nothing
            oRequest.InsuranceFileRef = "HEADOPOLPR00074"
            oRequest.LossDateFrom = "2006-10-08"
            oRequest.LossDateFromSpecified = False
            oRequest.LossDateTo = "2006-10-10"
            oRequest.LossDateToSpecified = False
            oRequest.RiskIndex = Nothing

            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.FindClaim(oRequest)
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
                    SAMTest.AssertCallSucceededWithResults(oResponse, oResponse.Claims)
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
        FindClaimTest()
    End Sub

#End Region

#Region "Missing Data"

    <Test()> _
    Public Sub InvalidData_Missing_BranchCode()
        FindClaimTest(eMissingData:=enumMissingData.BranchCode)
    End Sub

#End Region

#Region "Invalid Lookup"

    <Test()> _
    Public Sub InvalidData_BranchCode()
        FindClaimTest(eInvalidLookup:=enumInvalidLookup.BranchCode)
    End Sub

#End Region

#Region "STS Business Rule"

    <Test()> _
    Public Sub InvalidData_LossDateFrom()
        FindClaimTest(eInvalidLookup:=enumInvalidLookup.InvalidLossDateFrom)
    End Sub

    <Test()> _
    Public Sub InvalidData_LossDateTo()
        FindClaimTest(eInvalidLookup:=enumInvalidLookup.InvalidLossDateTo)
    End Sub

    <Test()> _
    Public Sub STSBusinessRule_InvalidLossDateDifference()
        FindClaimTest(eSTSBusinessError:=enumSTSBusinessError.InvalidLossDateDifference)
    End Sub

#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        FindClaimTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        FindClaimTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        FindClaimTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        FindClaimTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class
