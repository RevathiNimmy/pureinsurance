Imports NUnit.Framework

<TestFixture()> _
Public Class AddRisk
    Inherits BaseTest

#Region "Private Declarations"

    Private m_nPartyCnt As Integer
    Private m_nInsuranceFileCount As Integer
    Private m_nInsuranceFolderCount As Integer
    Private m_nRiskCnt As Integer
    Private m_sRiskDataSetXML As String
    Private m_bQuoteTimeStamp() As Byte

    Private m_oTestData As New TestData

    Private Enum enumMissingData
        'AgentKey
        BranchCode
        RiskTypeCode
        ScreenCode
        DataModelCode
        InsuranceFolderCnt
        InsuranceFileCnt
        ProductCode
        RiskDescription
        QuoteTimeStamp
        None
    End Enum

    Private Enum enumInvalidLookup
        None
        BranchCode
        SubBranchCode
        ProductCode
        ScreenCode
        RiskTypeCode
        DataModelCode
    End Enum

    Private Enum enumSTSBusinessError
        None
        'InvAgentKey
        InvInsuranceFolderKey
        InvInsuranceFileKey
        InvInsuranceFileFolder
    End Enum

#End Region

#Region "Private Test Method"

    Private Sub AddRiskTest( _
        Optional ByVal eMissingData As enumMissingData = enumMissingData.None, _
        Optional ByVal eInvalidLookup As enumInvalidLookup = enumInvalidLookup.None, _
        Optional ByVal eSTSBusinessError As enumSTSBusinessError = enumSTSBusinessError.None, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.AddRiskRequestType
        Dim oResponse As ProxyWS.AddRiskResponseType
        Dim nLookupError As Integer = 102
        Dim nBusinessError As Integer = 224

        Select Case eMissingData
            Case enumMissingData.None
                AddQuote()
            Case Else
                m_nInsuranceFileCount = 1
                m_nInsuranceFolderCount = 1
        End Select

        Try
            With oRequest
                ' Branch Code
                If eMissingData <> enumMissingData.BranchCode Then
                    If eInvalidLookup = enumInvalidLookup.BranchCode Then
                        .BranchCode = m_oTestData.InvalidLookupCode
                        nLookupError = 210
                    Else
                        .BranchCode = m_oTestData.BranchCode
                    End If
                End If
                ' Insurance Folder
                If eMissingData <> enumMissingData.InsuranceFolderCnt Then

                    If eSTSBusinessError = enumSTSBusinessError.InvInsuranceFolderKey Then
                        .InsuranceFolderKey = m_oTestData.InvalidCnt
                    Else
                        .InsuranceFolderKey = m_nInsuranceFolderCount
                    End If
                End If
                ' Insurance File
                If eMissingData <> enumMissingData.InsuranceFileCnt Then
                    Select Case eSTSBusinessError
                        Case enumSTSBusinessError.InvInsuranceFileKey
                            .InsuranceFileKey = m_oTestData.InvalidCnt
                        Case enumSTSBusinessError.InvInsuranceFileFolder
                            .InsuranceFolderKey = m_oTestData.InvalidInsFileFolderCnt
                            .InsuranceFileKey = m_nInsuranceFileCount
                            nBusinessError = 212
                        Case Else
                            .InsuranceFileKey = m_nInsuranceFileCount
                    End Select
                End If
                ' Data Model Code etc.
                If eMissingData <> enumMissingData.DataModelCode Then
                    If eInvalidLookup = enumInvalidLookup.DataModelCode Then
                        .DataModelCode = m_oTestData.InvalidLookupCode
                    Else
                        .DataModelCode = m_oTestData.DataModelCode
                    End If
                End If
                If eMissingData <> enumMissingData.ProductCode Then
                    If eInvalidLookup = enumInvalidLookup.ProductCode Then
                        .ProductCode = m_oTestData.InvalidLookupCode
                    Else
                        .ProductCode = m_oTestData.ProductCode
                    End If
                End If
                If eMissingData <> enumMissingData.RiskDescription Then
                    .RiskDescription = m_oTestData.RiskDescription
                End If
                If eMissingData <> enumMissingData.RiskTypeCode Then
                    If eInvalidLookup = enumInvalidLookup.RiskTypeCode Then
                        .RiskTypeCode = m_oTestData.InvalidLookupCode
                    Else
                        .RiskTypeCode = m_oTestData.RiskTypeCode
                    End If
                End If
                If eMissingData <> enumMissingData.QuoteTimeStamp Then
                    .QuoteTimeStamp = m_bQuoteTimeStamp
                End If
                If eMissingData <> enumMissingData.ScreenCode Then
                    If eInvalidLookup = enumInvalidLookup.ScreenCode Then
                        .ScreenCode = m_oTestData.InvalidLookupCode
                    Else
                        .ScreenCode = m_oTestData.ScreenCode
                    End If
                End If

                If eInvalidLookup = enumInvalidLookup.SubBranchCode Then
                    .SubBranchCode = m_oTestData.InvalidLookupCode
                    nLookupError = 210
                Else
                    .SubBranchCode = m_oTestData.SubBranch
                End If

                .RunDefaultRules = m_oTestData.RunDefaultRules
                .XMLDataSet = ""

            End With

            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.AddRisk(oRequest)
            With oResponse

                If eMissingData <> enumMissingData.None Then
                    ' Missing Data tests
                    SAMTest.AssertCallFailedWithErrors(oResponse, 1)
                    SAMTest.AssertErrorInvalidData(oResponse, 0, 100, "Mandatory " & eMissingData.ToString & " is missing")
                ElseIf eInvalidLookup <> enumInvalidLookup.None Then
                    ' Invalid Lookup tests
                    Assert.AreEqual(0, .RiskKey, "Unexpected returned value of RiskKey.")
                    SAMTest.AssertCallFailedWithErrors(oResponse, 1)
                    SAMTest.AssertErrorInvalidData(oResponse, 0, nLookupError, eInvalidLookup.ToString & " is invalid")
                ElseIf eSTSBusinessError <> enumSTSBusinessError.None Then
                    ' Business Rule tests
                    SAMTest.AssertCallFailedWithErrors(oResponse, 1)
                    SAMTest.AssertErrorBusinessRule(oResponse, 0, nBusinessError)
                Else
                    ' Success Tests
                    SAMTest.AssertCallSucceeded(oResponse)
                    m_nRiskCnt = .RiskKey
                    m_sRiskDataSetXML = .XMLDataSet
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

#Region "Setup preconditions"

    Private Sub AddQuote()

        Dim oAddQuote As New AddQuote
        oAddQuote.SupportMethod(m_nInsuranceFileCount, m_nInsuranceFolderCount, "", m_bQuoteTimeStamp)
    End Sub

#End Region

#Region "Success"

    Public Sub SupportMethod(ByRef r_nInsuranceFileCnt As Integer, _
                            ByRef r_nInsuranceFolderCnt As Integer, _
                            ByRef r_nRiskCnt As Integer, _
                            ByRef r_sRiskDataSetXML As String, _
                            ByRef r_bQuoteTimeStamp() As Byte, _
                            Optional ByVal bRunDefaultRules As Boolean = True)

        If Not bRunDefaultRules Then
            m_oTestData.RunDefaultRules = False
        End If

        Success()
        r_nInsuranceFileCnt = m_nInsuranceFileCount
        r_nInsuranceFolderCnt = m_nInsuranceFolderCount
        r_nRiskCnt = m_nRiskCnt
        r_sRiskDataSetXML = m_sRiskDataSetXML
        r_bQuoteTimeStamp = m_bQuoteTimeStamp

    End Sub

    <Test()> _
    Public Sub Success()
        AddRiskTest()
    End Sub

#End Region

#Region "Missing Mandatory Data"

    <Test()> _
    Public Sub InvalidData_Missing_DataModelCode()
        AddRiskTest(enumMissingData.DataModelCode, enumInvalidLookup.None)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_BranchCode()
        AddRiskTest(enumMissingData.BranchCode, enumInvalidLookup.None)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_InsuranceFileKey()
        AddRiskTest(enumMissingData.InsuranceFileCnt, enumInvalidLookup.None)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_InsuranceFolderKey()
        AddRiskTest(enumMissingData.InsuranceFolderCnt, enumInvalidLookup.None)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_ProductCode()
        AddRiskTest(enumMissingData.ProductCode, enumInvalidLookup.None)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_QuoteTimeStamp()
        AddRiskTest(enumMissingData.QuoteTimeStamp, enumInvalidLookup.None)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_RiskDescription()
        AddRiskTest(enumMissingData.RiskDescription, enumInvalidLookup.None)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_RiskTypeCode()
        AddRiskTest(enumMissingData.RiskTypeCode, enumInvalidLookup.None)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_ScreenCode()
        AddRiskTest(enumMissingData.ScreenCode, enumInvalidLookup.None)
    End Sub

#End Region

#Region "Invalid Lookup List"

    <Test()> _
    Public Sub InvalidData_BranchCode()
        AddRiskTest(enumMissingData.None, enumInvalidLookup.BranchCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_DataModelCode()
        AddRiskTest(enumMissingData.None, enumInvalidLookup.DataModelCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_ProductCode()
        AddRiskTest(enumMissingData.None, enumInvalidLookup.ProductCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_RiskTypeCode()
        AddRiskTest(enumMissingData.None, enumInvalidLookup.RiskTypeCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_ScreenCode()
        AddRiskTest(enumMissingData.None, enumInvalidLookup.ScreenCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_SubBranchCode()
        AddRiskTest(enumMissingData.None, enumInvalidLookup.SubBranchCode)
    End Sub

#End Region

#Region "STS Business Rules"

    <Test()> _
    Public Sub STSBusiness_InsFolderCnt()
        AddRiskTest(eSTSBusinessError:=enumSTSBusinessError.InvInsuranceFolderKey)
    End Sub
    <Test()> _
    Public Sub STSBusiness_InsFileCnt()
        AddRiskTest(eSTSBusinessError:=enumSTSBusinessError.InvInsuranceFileKey)
    End Sub
    <Test()> _
    Public Sub STSBusiness_InsFileFolder()
        AddRiskTest(eSTSBusinessError:=enumSTSBusinessError.InvInsuranceFileFolder)
    End Sub

#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        AddRiskTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        AddRiskTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        AddRiskTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        AddRiskTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class
