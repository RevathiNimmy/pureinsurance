Imports NUnit.Framework

<TestFixture()> _
Public Class GetClaimDetails
    Inherits BaseTest

#Region "Private Declarations"

    'Private m_nInsuranceFolderCnt As Integer
    'Private m_nInsuranceFileCnt As Integer

    Private m_TimeStamp As Byte()
    Private m_lClaimId As Integer
    Private m_lBaseClaimId As Integer
    Private m_sBranchCode As String
    Private m_oResponse As ProxyWS.BaseGetClaimDetailsResponseType

    Private m_oTestData As New TestData

    Private Enum enumMissingData
        None
        BranchCode
        ClaimKey
    End Enum

    Private Enum enumInvalidLookup
        None
        BranchCode
    End Enum

    Private Enum enumSTSBusinessError
        None
        InvalidBranchCode
        InvalidClaimKey
    End Enum

    'Private Enum enumDataScenario
    '    MultiplePerils
    '    MultipleRecoveries
    '    MultipleReserves
    '    NoRecoveries
    '    NoReserves
    '    NoInsurerAttached
    '    MultiplePerilsMultiplePayments
    '    MultiplePerilsMultipleReceipts
    '    MultiplePerilsNoPayments
    '    MultiplePerilsNoReceipts
    'End Enum

#End Region

#Region "Setup Preconditions"

    Private Sub OpenClaim( _
    ByRef lBaseClaimkey As Integer, _
    ByRef lClaimKey As Integer, _
    ByRef sBranchCode As String)

        Dim oOpenClaim As New OpenClaim
        Dim TimeStamp As Byte() = Nothing
        oOpenClaim.SupportMethod(lBaseClaimkey, lClaimKey, sBranchCode, TimeStamp)
    End Sub


#End Region

#Region "Private Test Methods"

    Private Function GetClaimDetailsTest( _
        Optional ByVal ClaimId As Integer = 0, _
        Optional ByVal BranchCode As String = "", _
        Optional ByVal eMissingData As enumMissingData = enumMissingData.None, _
        Optional ByVal eInvalidLookup As enumInvalidLookup = enumInvalidLookup.None, _
        Optional ByVal eSTSBusinessError As enumSTSBusinessError = enumSTSBusinessError.None, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None) As ProxyWS.GetClaimDetailsResponseType

        Dim oRequest As New ProxyWS.GetClaimDetailsRequestType
        Dim oResponse As ProxyWS.GetClaimDetailsResponseType = Nothing
        Dim nLookupError As Integer = 102
        Dim nBusinessError As Integer = 224

        Try

            OpenClaim(m_lBaseClaimId, m_lClaimId, m_sBranchCode)

            With oRequest
                .ClaimKey = m_lClaimId
                .BranchCode = m_sBranchCode
            End With

            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.GetClaimDetails(oRequest)

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

                    m_TimeStamp = oResponse.TimeStamp()
                    m_oResponse = oResponse

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
            'objGetClaimResponseType = Nothing
        End Try

        Return oResponse

    End Function

    Public Sub TestResponse()

    End Sub

    Public Sub SupportMethod(ByRef r_oResponse As ProxyWS.GetClaimDetailsResponseType)

        Success()

        r_oResponse = m_oResponse

    End Sub

    Public Sub SupportMethod(ByRef r_lClaimId As Integer, _
                             ByRef r_sBranchCode As String, _
                             ByRef r_lBaseClaimid As Integer, _
                             ByRef r_TimeStamp As Byte())

        Success()

        r_lClaimId = m_lClaimId
        r_sBranchCode = m_sBranchCode
        r_lBaseClaimid = m_lBaseClaimId
        r_TimeStamp = m_TimeStamp

    End Sub

    Public Function SupportMethod(ByVal ClaimId As Integer, _
                                    ByVal BranchCode As String) As ProxyWS.GetClaimDetailsResponseType
        Return Success(ClaimId, BranchCode)
    End Function

#End Region

#Region "Success"

    <Test()> _
    Public Sub Success()
        GetClaimDetailsTest()
    End Sub

    Public Function Success(ByVal ClaimId As Integer, _
                            ByVal BranchCode As String) As ProxyWS.GetClaimDetailsResponseType
        Return GetClaimDetailsTest(ClaimId, BranchCode)
    End Function

#End Region

#Region "Missing Data"

    <Test()> _
    Public Sub Missing_BranchCode()
        GetClaimDetailsTest(eMissingData:=enumMissingData.BranchCode)
    End Sub
    <Test()> _
    Public Sub Missing_ClaimKey()
        GetClaimDetailsTest(eMissingData:=enumMissingData.ClaimKey)
    End Sub

#End Region

#Region "Data Scenario"

    'Public Sub MultiplePerils()

    'End Sub

    'Public Sub MultipleRecoveries()

    'End Sub
    'Public Sub MultipleReserves()

    'End Sub

    'Public Sub NoRecoveries()

    'End Sub

    'Public Sub NoReserves()

    'End Sub

    'Public Sub NoInsurerAttached()

    'End Sub

    'Public Sub MultiplePerilsMultiplePayments()

    'End Sub

    'Public Sub MultiplePerilsMultipleReceipts()

    'End Sub

    'Public Sub MultiplePerilsNoPayments()

    'End Sub

    'Public Sub MultiplePerilsNoReceipts()

    'End Sub

#End Region

#Region "Invalid Lookup"

    <Test()> _
    Public Sub InvalidData_BranchCode()
        GetClaimDetailsTest(eInvalidLookup:=enumInvalidLookup.BranchCode)
    End Sub

#End Region

#Region "STS Business Rules"

    '<Test()> _
    'Public Sub STSBusiness_InvalidClaimKey()
    '    GetClaimDetailsTest(eSTSBusinessError:=enumSTSBusinessError.ClaimKey)
    'End Sub
    '<Test()> _
    'Public Sub STSBusiness_InvalidBranchCode()
    '    GetClaimDetailsTest(eSTSBusinessError:=enumSTSBusinessError.BranchCode)
    'End Sub

#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        GetClaimDetailsTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        GetClaimDetailsTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        GetClaimDetailsTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        GetClaimDetailsTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class
