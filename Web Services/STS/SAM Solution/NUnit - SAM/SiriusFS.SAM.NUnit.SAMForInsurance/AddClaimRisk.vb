Imports NUnit.Framework

<TestFixture()> _
Public Class AddClaimRisk
    Inherits BaseTest

#Region "Private Declarations"

    'Private m_nInsuranceFolderCnt As Integer
    'Private m_nInsuranceFileCnt As Integer

    Private m_oTestData As New TestData
    Private m_sXMLDataset As String
    Private m_btTimestamp As Byte()
    Private m_iClaimKey As Integer

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

#End Region

#Region "Private Test Methods"

    Private m_lClaimid As Integer
    Private m_sBranchCode As String
    Private m_lBaseClaimId As Integer
    Private m_TimeStamp As Byte()

    Private Sub GetClaimDetails( _
    ByRef r_lClaimid As Integer, _
    ByRef r_lBaseClaimid As Integer, _
    ByRef r_sBranchCode As String, _
    ByRef r_TimeStamp As Byte())

        Dim oGetClaimDetails As New GetClaimDetails

        oGetClaimDetails.SupportMethod(r_lClaimid, r_sBranchCode, r_lBaseClaimid, r_TimeStamp)

    End Sub

    Private Function AddClaimRiskTest( _
        Optional ByVal ClaimId As Integer = 0, _
        Optional ByVal BranchCode As String = "", _
        Optional ByVal eMissingData As enumMissingData = enumMissingData.None, _
        Optional ByVal eInvalidLookup As enumInvalidLookup = enumInvalidLookup.None, _
        Optional ByVal eSTSBusinessError As enumSTSBusinessError = enumSTSBusinessError.None, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None) As ProxyWS.AddClaimRiskResponseType

        Dim oRequest As New ProxyWS.AddClaimRiskRequestType
        Dim oResponse As ProxyWS.AddClaimRiskResponseType = Nothing
        Dim nLookupError As Integer = 102
        Dim nBusinessError As Integer = 224

        ' Test cases
        ' Claim Key - 21
        '           - 121
        '           - 110
        '           - 17

        ' If an Insurance Folder was specified in the test data, find that one,
        ' else create a new one
        'If m_oTestData.InsuranceFolderCnt = 0 Then
        '    AddQuote()
        'Else
        '    m_nInsuranceFolderCnt = m_oTestData.InsuranceFolderCnt
        'End If

        ' get the claim details
        GetClaimDetails(m_lClaimid, m_lBaseClaimId, m_sBranchCode, m_TimeStamp)


        Try
            With oRequest
                .BaseClaimKey = m_lBaseClaimId
                .BranchCode = m_sBranchCode
                .TimeStamp = m_TimeStamp


                'If eMissingData <> enumMissingData.ClaimKey Then
                '    If eSTSBusinessError <> enumSTSBusinessError.InvalidClaimKey Then
                '        If ClaimId = 0 Then
                '            Dim oOpenClaim As New OpenClaim
                '            oOpenClaim.SupportMethod(r_lBaseClaimKey:=.BaseClaimKey, _
                '                                     r_lClaimKey:=0, _
                '                                     r_sBranchCode:="")
                '            m_iClaimKey = .BaseClaimKey
                '        Else
                '            .BaseClaimKey = ClaimId
                '        End If
                '    Else
                '        .BaseClaimKey = m_oTestData.InvalidCnt
                '        nBusinessError = 274
                '    End If
                'End If
                'If eMissingData <> enumMissingData.BranchCode Then
                '    If eInvalidLookup = enumInvalidLookup.BranchCode Then
                '        .BranchCode = m_oTestData.InvalidLookupCode
                '        nLookupError = 210
                '    Else
                '        If BranchCode = "" Then
                '            .BranchCode = m_oTestData.GetClaimRiskBranchCode
                '        Else
                '            .BranchCode = BranchCode
                '        End If
                '    End If
                'End If
            End With

            'If eDataScenario = enumDataScenario.MultiplePerils Then
            'ElseIf eDataScenario = enumDataScenario.MultiplePerilsMultiplePayments Then
            'ElseIf eDataScenario = enumDataScenario.MultiplePerilsMultipleReceipts Then
            'ElseIf eDataScenario = enumDataScenario.MultiplePerilsNoPayments Then
            'ElseIf eDataScenario = enumDataScenario.MultiplePerilsNoReceipts Then
            'ElseIf eDataScenario = enumDataScenario.MultipleRecoveries Then
            'ElseIf eDataScenario = enumDataScenario.MultipleReserves Then
            'ElseIf eDataScenario = enumDataScenario.NoInsurerAttached Then
            'ElseIf eDataScenario = enumDataScenario.NoRecoveries Then
            'ElseIf eDataScenario = enumDataScenario.NoReserves Then

            'End If

            'If ClaimId = 0 And BranchCode = "" Then
            '    objGetClaimRequestType.BranchCode = m_oTestData.AddClaimRiskBranchCode
            '    objGetClaimRequestType.ClaimKey = m_oTestData.AddClaimRiskClaimKey
            'Else
            '    objGetClaimRequestType.BranchCode = BranchCode
            '    objGetClaimRequestType.ClaimKey = ClaimId
            'End If

            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.AddClaimRisk(oRequest)
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
                    'Assert.IsNotNull(.Policies, "No Result Set returned")
                    'TODO: additional assert test
                End If

            End With

            m_sXMLDataset = oResponse.XMLDataSet
            m_btTimestamp = oResponse.TimeStamp

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

#End Region

#Region "Support Method"

    Public Sub SupportMethod(ByVal BranchCode As String, _
                                  ByRef r_iClaimId As Integer, _
                                  ByRef r_sXMLDataset As String, _
                                  ByRef r_btTimestamp As Byte())

        r_iClaimId = 0

        Dim AddClaimRiskResponseType As ProxyWS.AddClaimRiskResponseType
        AddClaimRiskResponseType = Success(r_iClaimId, _
                                           BranchCode)
        r_sXMLDataset = m_sXMLDataset
        r_btTimestamp = m_btTimestamp
        r_iClaimId = m_iClaimKey

    End Sub

#End Region

#Region "Success"

    <Test()> _
    Public Sub Success()
        AddClaimRiskTest()
    End Sub

    Public Function Success(ByVal ClaimId As Integer, _
                            ByVal BranchCode As String) As ProxyWS.AddClaimRiskResponseType
        Return AddClaimRiskTest(ClaimId, BranchCode)
    End Function

#End Region

#Region "Missing Data"

    <Test()> _
    Public Sub Missing_BranchCode()
        AddClaimRiskTest(eMissingData:=enumMissingData.BranchCode)
    End Sub
    <Test()> _
    Public Sub Missing_ClaimKey()
        AddClaimRiskTest(eMissingData:=enumMissingData.ClaimKey)
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
        AddClaimRiskTest(eInvalidLookup:=enumInvalidLookup.BranchCode)
    End Sub

#End Region

#Region "STS Business Rules"

    '<Test()> _
    'Public Sub STSBusiness_InvalidClaimKey()
    '    AddClaimRiskTest(eSTSBusinessError:=enumSTSBusinessError.ClaimKey)
    'End Sub
    '<Test()> _
    'Public Sub STSBusiness_InvalidBranchCode()
    '    AddClaimRiskTest(eSTSBusinessError:=enumSTSBusinessError.BranchCode)
    'End Sub

#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        AddClaimRiskTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        AddClaimRiskTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        AddClaimRiskTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        AddClaimRiskTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class
