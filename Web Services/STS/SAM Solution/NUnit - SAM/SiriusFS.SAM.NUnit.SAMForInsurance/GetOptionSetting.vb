Imports NUnit.Framework

<TestFixture()> _
Public Class GetOptionSetting
    Inherits BaseTest

#Region "Private Declarations"

    Private Const kInvalidLookupFieldMessage As String = "Invalid Lookup Field Name Returned"

    Private Enum enumTestCaseScenario
        None

        '------------------------- MISSING DATA -------------------------
        AllNonMandatoryFieldsMissing
        MissingBranchCode
        MissingBaseOptionNumber
        MissingBaseOptionType

        '------------------------- INVALID DATA -------------------------
        InvalidBranchCode
        InvalidBaseOptionNumber
        InvalidBaseOptionType

    End Enum

    Private m_oTestData As New TestData

#End Region

#Region "Setup Preconditions"

#End Region

#Region "Private Test Methods"

    Private Sub ProcessTestCases(ByVal oRequest As ProxyWS.GetOptionSettingRequestType, ByVal TestCase As enumTestCaseScenario)

        Select Case TestCase
            Case enumTestCaseScenario.AllNonMandatoryFieldsMissing
                AllNonMandatoryFieldsMissing(oRequest)
            Case enumTestCaseScenario.MissingBranchCode
                MissingBranchCode(oRequest)
            Case enumTestCaseScenario.MissingBaseOptionNumber
                MissingOptionNumber(oRequest)
            Case enumTestCaseScenario.MissingBaseOptionType
                MissingOptionType(oRequest)

            Case enumTestCaseScenario.InvalidBranchCode
                InvalidBranchCode(oRequest)
            Case enumTestCaseScenario.InvalidBaseOptionNumber
                InvalidBaseOptionNumber(oRequest)
            Case enumTestCaseScenario.InvalidBaseOptionType
                InvalidBaseOptionType(oRequest)

            Case Else
        End Select

    End Sub

#Region "Requisite Functions for Test Cases (Invalid Data)"

    Private Sub InvalidBranchCode(ByVal oGetOptionSettingRequest As ProxyWS.GetOptionSettingRequestType)
        oGetOptionSettingRequest.BranchCode = "202"
    End Sub

    Private Sub InvalidBaseOptionNumber(ByVal oGetOptionSettingRequest As ProxyWS.GetOptionSettingRequestType)
        oGetOptionSettingRequest.OptionNumber = 8888888
    End Sub

    Private Sub InvalidBaseOptionType(ByVal oGetOptionSettingRequest As ProxyWS.GetOptionSettingRequestType)
        oGetOptionSettingRequest.OptionType = 3
    End Sub

#End Region

#Region "Requisite Functions for Test Cases (Missing Data)"

    Private Sub AllNonMandatoryFieldsMissing(ByVal oGetOptionSettingRequest As ProxyWS.GetOptionSettingRequestType)

    End Sub

    Private Sub MissingBranchCode(ByVal oGetOptionSettingRequest As ProxyWS.GetOptionSettingRequestType)
        oGetOptionSettingRequest.BranchCode = Nothing
    End Sub

    Private Sub MissingOptionNumber(ByVal oGetOptionSettingRequest As ProxyWS.GetOptionSettingRequestType)
        oGetOptionSettingRequest.OptionNumber = Nothing
    End Sub

    Private Sub MissingOptionType(ByVal oGetOptionSettingRequest As ProxyWS.GetOptionSettingRequestType)
        oGetOptionSettingRequest.OptionType = Nothing
    End Sub

#End Region

#Region "Business Rule Error"

#End Region

#Region "Other Conditions"

#End Region

    Private Sub GetOptionSettingTest( _
        Optional ByVal TestCases As enumTestCaseScenario = enumTestCaseScenario.None, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.GetOptionSettingRequestType
        Dim oResponse As ProxyWS.GetOptionSettingResponseType
        Dim nLookupError As Integer = 102
        Dim nBusinessError As Integer = 224

        Try

            oRequest.BranchCode = m_oTestData.BranchCode
            oRequest.OptionNumber = m_oTestData.GetOptionSettingNumber
            oRequest.OptionType = m_oTestData.GetOptionSettingType

            ' reset fields based on selected test case
            ProcessTestCases(oRequest, TestCases)

            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.GetOptionSetting(oRequest)

            With oResponse

                ' all these test cases should work without error
                If TestCases = enumTestCaseScenario.None OrElse _
                    TestCases = enumTestCaseScenario.AllNonMandatoryFieldsMissing Then

                    SAMTest.AssertCallSucceeded(oResponse)

                ElseIf TestCases = enumTestCaseScenario.InvalidBranchCode OrElse _
                     TestCases = enumTestCaseScenario.InvalidBaseOptionNumber OrElse _
                     TestCases = enumTestCaseScenario.InvalidBaseOptionType Then

                    ProcessInvalidData(oResponse, TestCases)

                ElseIf TestCases = enumTestCaseScenario.MissingBaseOptionNumber OrElse _
                    TestCases = enumTestCaseScenario.MissingBaseOptionType OrElse _
                    TestCases = enumTestCaseScenario.MissingBranchCode Then

                    ProcessMissingData(oResponse, TestCases)

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

    Private Sub ProcessInvalidData(ByVal oResponse As ProxyWS.BaseResponseType, ByVal TestCase As enumTestCaseScenario)

        ' raise an error if no error was returned
        SAMTest.AssertCallFailedWithErrors(oResponse, 1)

        ' assign the error object
        Dim oError As ProxyWS.SAMErrorInvalidData = SAMTest.AssertErrorInvalidData(oResponse, 0)

        ' raise an errror if the error code (id) doesnt match the expected value
        'Assert.AreEqual(SAMConstants.SAMInvalidData.InvalidLookupListValue, oError.Code, kInvalidCodeReturned)

        ' raise an error if the error description doesnt not match the expected value
        'Assert.AreEqual(SAMConstants.SAMInvalidData.InvalidLookupListValue.ToString(), oError.Description, kInvalidDescriptionReturned)

        ' raise an error if an invalid lookup table is being used (field name holds the lookup table name)
        Select Case TestCase
            Case enumTestCaseScenario.InvalidBranchCode
                Assert.AreEqual("BranchCode", oError.FieldName, kInvalidLookupFieldMessage)
            Case enumTestCaseScenario.InvalidBaseOptionType
                Assert.AreEqual("BaseOptionType", oError.FieldName, kInvalidLookupFieldMessage)
            Case enumTestCaseScenario.InvalidBaseOptionNumber
                Assert.AreEqual("BaseOptionNumber", oError.FieldName, kInvalidLookupFieldMessage)
        End Select

    End Sub

    Private Sub ProcessMissingData(ByVal oResponse As ProxyWS.BaseResponseType, ByVal TestCase As enumTestCaseScenario)

        ' raise an error if no error was returned
        SAMTest.AssertCallFailedWithErrors(oResponse, 1)

        ' assign the error object
        Dim oError As ProxyWS.SAMErrorInvalidData = SAMTest.AssertErrorInvalidData(oResponse, 0)

        ' raise an errror if the error code (id) doesnt match the expected value
        'Assert.AreEqual(SAMConstants.SAMInvalidData.InvalidLookupListValue, oError.Code, kInvalidCodeReturned)

        ' raise an error if the error description doesnt not match the expected value
        'Assert.AreEqual(SAMConstants.SAMInvalidData.InvalidLookupListValue.ToString(), oError.Description, kInvalidDescriptionReturned)

        ' raise an error if an invalid lookup table is being used (field name holds the lookup table name)
        Select Case TestCase
            Case enumTestCaseScenario.MissingBaseOptionNumber
                Assert.AreEqual("BaseOptionNumber", oError.FieldName, kInvalidLookupFieldMessage)
            Case enumTestCaseScenario.MissingBaseOptionType
                Assert.AreEqual("BaseOptionType", oError.FieldName, kInvalidLookupFieldMessage)
            Case enumTestCaseScenario.MissingBranchCode
                Assert.AreEqual("BranchCode", oError.FieldName, kInvalidLookupFieldMessage)
        End Select

    End Sub

    Private Sub ProcessBusinessErrors(ByVal oResponse As ProxyWS.BaseResponseType, ByVal TestCase As enumTestCaseScenario)

        ' raise an error if no error was returned
        SAMTest.AssertCallFailedWithErrors(oResponse, 1)

        ' assign the error object
        Dim oError As ProxyWS.SAMErrorBusinessRule = SAMTest.AssertErrorBusinessRule(oResponse, 0)

        ' raise an error if an invalid lookup table is being used (field name holds the lookup table name)
        Select Case TestCase
       
        End Select

    End Sub

#Region "Success"

    <Test()> _
    Public Sub Success()
        GetOptionSettingTest()
    End Sub

#End Region

#Region "Missing Data"

    <Test()> _
    Public Sub Missing_AllNonMandatoryFields()
        GetOptionSettingTest(TestCases:=enumTestCaseScenario.AllNonMandatoryFieldsMissing)
    End Sub

    <Test()> _
    Public Sub Missing_BaseOptionNumber()
        GetOptionSettingTest(TestCases:=enumTestCaseScenario.MissingBaseOptionNumber)
    End Sub

    <Test()> _
    Public Sub Missing_BaseOptionType()
        GetOptionSettingTest(TestCases:=enumTestCaseScenario.MissingBaseOptionType)
    End Sub

    <Test()> _
    Public Sub Missing_BranchCode()
        GetOptionSettingTest(TestCases:=enumTestCaseScenario.MissingBranchCode)
    End Sub

#End Region

#Region "Public Properties"


#End Region

#Region "Invalid Lookup"

    <Test()> _
    Public Sub InvalidData_BranchCode()
        GetOptionSettingTest(TestCases:=enumTestCaseScenario.InvalidBranchCode)
    End Sub

    <Test()> _
    Public Sub InvalidData_BaseOptionNumber()
        GetOptionSettingTest(TestCases:=enumTestCaseScenario.InvalidBaseOptionNumber)
    End Sub

    <Test()> _
    Public Sub InvalidData_BaseOptionType()
        GetOptionSettingTest(TestCases:=enumTestCaseScenario.InvalidBaseOptionType)
    End Sub

#End Region

#Region "STS Business Rules"


#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        GetOptionSettingTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        GetOptionSettingTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        GetOptionSettingTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        GetOptionSettingTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class

#Region "Working Fine Data"


#End Region

