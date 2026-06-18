Imports NUnit.Framework

<TestFixture()> _
Public Class GetRatingDetails
    Inherits BaseTest

#Region "Private Declarations"

    Private m_oTestData As New TestData

    Private Enum enumPartyType
        PartyPC
        PartyCC
    End Enum

    Private Enum enumInvalidData
        RK_DataModelCode
        RK_ScreenCode
        RK_RiskTypeCode
        PO_ProductCode
        PO_BranchCode
        PO_StartDateInPast
        PO_EndDateBeforeStartDate
        DS_DataModelCode
        DS_BranchCode
        None
    End Enum

    Private Enum enumMandDataMissing
        NB_BranchCode = 0
        NB_Client = 1
        NB_Policy = 2
        NB_AgentKey = 3
        CC_BranchCode = 100
        CC_TradingName = 101
        CC_CompanyName = 102
        PC_BranchCode = 200
        PC_Forename = 201
        PC_Surname = 202
        PC_DateOfBirth = 203
        PC_GenderCode = 204
        PC_Title = 205
        AD_Address = 300
        AD_AddressLine1 = 301
        AD_PostCode = 302
        CN_Contact = 400
        CN_ContactType = 401
        CN_ContactItem = 402
        CN_ContactDetail = 403
        PO_BranchCode = 500
        PO_CoverStartDate = 501
        PO_CoverEndDate = 502
        PO_Description = 503
        PO_ProductCode = 504
        PO_Risks = 505
        PO_InsuranceFileKey = 506
        RK_RiskTypeCode = 601
        RK_ScreenCode = 602
        RK_RiskDescription = 603
        RK_DataModelCode = 604
        RK_RiskKey = 605
        DS_DataModelCode = 700
        DS_BranchCode = 701

        None = 10000
    End Enum

#End Region

#Region "Private Methods"

    Private Sub GetRatingDetailsTest( _
        Optional ByVal eMissingField As enumMandDataMissing = enumMandDataMissing.None, _
        Optional ByVal eInvalidData As enumInvalidData = enumInvalidData.None, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.GetRatingDetailsRequestType
        Dim oResponse As ProxyWS.GetRatingDetailsResponseType
        Dim riskCnt As Integer = m_oTestData.RiskCnt
        Dim insuranceFileCnt As Integer = m_oTestData.InsuranceFileCnt

        Try
            ' Set the specific condition on the input class
            If eMissingField <> enumMandDataMissing.DS_BranchCode Then
                If eInvalidData = enumInvalidData.DS_BranchCode Then
                    oRequest.BranchCode = "INVALID"
                Else
                    oRequest.BranchCode = m_oTestData.branchcode
                End If
            End If

            If riskCnt = 0 Then
                Dim oUpdateRisk As New UpdateRisk
                Dim grossPremium As Double = 0D
                oUpdateRisk.SupportMethod(insuranceFileCnt, riskCnt, grossPremium)
            End If

            oRequest.InsuranceFileKey = insuranceFileCnt
            oRequest.RiskKey = riskCnt

            ' Set the specific condition on the input class
            If eMissingField = enumMandDataMissing.PO_InsuranceFileKey Then
                oRequest.InsuranceFileKey = 0
            End If

            ' Set the specific condition on the input class
            If eMissingField = enumMandDataMissing.RK_RiskKey Then
                oRequest.RiskKey = 0
            End If

            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.GetRatingDetails(oRequest)

            With oResponse
                If eMissingField <> enumMandDataMissing.None Then
                    ' Missing Mandatory field tests
                    SAMTest.AssertCallFailedWithErrors(oResponse, 1)
                    SAMTest.AssertErrorInvalidData(oResponse, 0, 100, "Mandatory " & Mid(eMissingField.ToString, 4) & " is missing")
                ElseIf eInvalidData <> enumInvalidData.None Then
                    ' Invalid data tests
                    SAMTest.AssertCallFailedWithErrors(oResponse, 1)
                    Dim oError As ProxyWS.SAMErrorInvalidData = SAMTest.AssertErrorInvalidData(oResponse, 0)
                    Select Case eInvalidData
                        Case enumInvalidData.DS_DataModelCode
                            Assert.AreEqual(102, oError.Code, "Unexpected Invalid Data error: " & oError.Description)
                            Assert.AreEqual(Mid(eInvalidData.ToString, 4) & " is invalid", oError.Description, "Unexpected description for error 100: " & oError.Description)
                        Case enumInvalidData.DS_BranchCode
                            Assert.AreEqual(210, oError.Code, "Unexpected Invalid Data error: " & oError.Description)
                    End Select
                Else
                    ' Success tests
                    SAMTest.AssertCallSucceeded(oResponse)
                    SAMTest.AssertCallSucceededWithResults(oResponse, oResponse.RatingDetails)
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
        GetRatingDetailsTest()
    End Sub

#End Region

#Region "Invalid Data"

    <Test()> _
    Public Sub InvalidData_DS_BranchCode()
        GetRatingDetailsTest(enumMandDataMissing.None, enumInvalidData.DS_BranchCode)
    End Sub

    'Summary of exceptions
    '100    Mandatory input parameter missing
    'o  Branch
    'o  Sub branch
    'o  Gender
    'o  Data of birth
    '101    Invalid date format
    'o  Date of birth
    '102    Invalid lookup list value
    'o  Sub branch
    'o  Gender
    '110  Invalid Lookup list value
    'o  Branch

#End Region

#Region "Mandatory Data Missing"

    <Test()> _
    Public Sub InvalidData_Missing_DS_Branch_Code()
        GetRatingDetailsTest(enumMandDataMissing.DS_BranchCode, enumInvalidData.None)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_PO_InsuranceFileKey()
        GetRatingDetailsTest(enumMandDataMissing.PO_InsuranceFileKey, enumInvalidData.None)
    End Sub

    <Test()> _
    Public Sub InvalidData_Missing_RK_RiskKey()
        GetRatingDetailsTest(enumMandDataMissing.RK_RiskKey, enumInvalidData.None)
    End Sub

#End Region

#Region "Invalid Format"

#End Region

#Region "Invalid List Value"

#End Region

#Region "STS Business Rules"

#End Region

#Region "Sirius Back Office"

#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        GetRatingDetailsTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        GetRatingDetailsTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        GetRatingDetailsTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        GetRatingDetailsTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class
