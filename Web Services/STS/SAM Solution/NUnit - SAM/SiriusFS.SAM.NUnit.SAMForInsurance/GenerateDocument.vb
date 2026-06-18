Imports NUnit.Framework

<TestFixture()> _
Public Class GenerateDocument
    Inherits BaseTest

#Region "Private Declarations"

    Private m_nPartyCnt As Integer
    Private m_nInsuranceFileCnt As Integer
    Private m_nInsuranceFolderCnt As Integer
    Private m_btPartyTimeStamp() As Byte

    Private m_oTestData As New TestData

    Private Enum enumMissingData
        None
        'AgentKey
        BranchCode
        DocumentTemplateCode
        PartyKey
        InsuranceFileKey
        InsuranceFolderKey
    End Enum

    Private Enum enumInvalidLookup
        None
        BranchCode
    End Enum

    Private Enum enumSTSBusinessError
        None
        'InvalidAgentKey
        InvInsuranceFolderKey
        InvInsuranceFileKey
        InvInsuranceFileFolder
    End Enum

#End Region

#Region "Setup Preconditions"

    Private Sub AddQuote()

        Dim oAddQuote As New AddQuote
        oAddQuote.SupportMethod(m_nInsuranceFileCnt, m_nInsuranceFolderCnt, "", m_btPartyTimeStamp, m_nPartyCnt)

    End Sub

#End Region

#Region "Private Test Methods"

    Private Sub GenerateDocumentTest( _
        Optional ByVal eMissingData As enumMissingData = enumMissingData.None, _
        Optional ByVal eInvalidLookup As enumInvalidLookup = enumInvalidLookup.None, _
        Optional ByVal eSTSBusinessError As enumSTSBusinessError = enumSTSBusinessError.None, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.GenerateDocumentRequestType
        Dim oResponse As ProxyWS.GenerateDocumentResponseType
        Dim nLookupError As Integer = 102
        Dim nBusinessError As Integer = 224

        AddQuote()

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
                ' Insurance Folder
                If eMissingData <> enumMissingData.InsuranceFolderKey Then

                    If eSTSBusinessError = enumSTSBusinessError.InvInsuranceFolderKey Then
                        .InsuranceFolderKey = m_oTestData.InvalidCnt
                    Else
                        .InsuranceFolderKey = m_nInsuranceFolderCnt
                    End If
                End If
                ' Insurance File
                If eMissingData <> enumMissingData.InsuranceFileKey Then
                    Select Case eSTSBusinessError
                        Case enumSTSBusinessError.InvInsuranceFileKey
                            .InsuranceFileKey = m_oTestData.InvalidCnt
                        Case enumSTSBusinessError.InvInsuranceFileFolder
                            .InsuranceFolderKey = m_oTestData.InvalidInsFileFolderCnt
                            .InsuranceFileKey = m_nInsuranceFileCnt
                            nBusinessError = 212
                        Case Else
                            .InsuranceFileKey = m_nInsuranceFileCnt
                    End Select
                End If

                If eMissingData <> enumMissingData.DocumentTemplateCode Then
                    .DocumentTemplateCode = m_oTestData.DocumentTemplateCode
                End If
                .Mode = m_oTestData.GenerateDocumentMode
                .OutputAsHTML = m_oTestData.GenerateDocumentOutputAsHTML
                .ParameterXML = m_oTestData.GenerateDocumentParameterXML
            End With

            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.GenerateDocument(oRequest)
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
                    Assert.AreNotEqual("", .MergedFilePath, "No MergedFilePath returned")
                    Assert.Greater(.SpooledZipFile.Length, 0, "No SpooledZipFile returned")
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
        GenerateDocumentTest()
    End Sub

#End Region

#Region "Missing Data"

    <Test()> _
    Public Sub InvalidData_Missing_BranchCode()
        GenerateDocumentTest(eMissingData:=enumMissingData.BranchCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_PartyKey()
        GenerateDocumentTest(eMissingData:=enumMissingData.PartyKey)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_DocumentTemplateCode()
        GenerateDocumentTest(eMissingData:=enumMissingData.DocumentTemplateCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_InsuranceFileKey()
        GenerateDocumentTest(eMissingData:=enumMissingData.InsuranceFileKey)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_InsuranceFolderKey()
        GenerateDocumentTest(eMissingData:=enumMissingData.InsuranceFolderKey)
    End Sub

#End Region

#Region "Invalid Lookup"

    <Test()> _
    Public Sub InvalidData_BranchCode()
        GenerateDocumentTest(eInvalidLookup:=enumInvalidLookup.BranchCode)
    End Sub

#End Region

#Region "STS Business Rules"

    <Test()> _
    Public Sub STSBusiness_InsFolderCnt()
        GenerateDocumentTest(eSTSBusinessError:=enumSTSBusinessError.InvInsuranceFolderKey)
    End Sub
    <Test()> _
    Public Sub STSBusiness_InsFileCnt()
        GenerateDocumentTest(eSTSBusinessError:=enumSTSBusinessError.InvInsuranceFileKey)
    End Sub
    <Test()> _
    Public Sub STSBusiness_InsFileFolder()
        GenerateDocumentTest(eSTSBusinessError:=enumSTSBusinessError.InvInsuranceFileFolder)
    End Sub

#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        GenerateDocumentTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        GenerateDocumentTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        GenerateDocumentTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        GenerateDocumentTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class
