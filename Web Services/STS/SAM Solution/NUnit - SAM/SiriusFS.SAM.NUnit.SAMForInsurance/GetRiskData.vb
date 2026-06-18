Imports NUnit.Framework
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Data.SqlDbType

Public Class GetRiskData
    Inherits BaseTest
    Private Const kInvalidLookupValue As String = "000GARBAGE"
    Private Const kInvalidLookupFieldMessage As String = "Invalid Lookup Field Name Returned"
    Private Const kIncorrectMissingMandatoryFieldMessage As String = "Incorrect Missing Mandatory Field Returned"
    Private Const kInvalidCodeReturned As String = "Invalid Error Code Returned"
    Private Const kInvalidDescriptionReturned As String = "Invalid Error Description Returned"
    Private Const kInvalidLookupValueMessage As String = "Invalid Lookup Value Returned"

#Region "Private Declarations"

    Private m_oTestData As New TestData
    Private m_lBaseClaimId As Integer

    Private Enum TestCaseScenario

        ' Non Error Scenarios
        None
        AllNonMandatoryFieldsMissing

        ' Busines Error scenarios : - some of these cannot be tested

        ' Busines Warning scenarios

        ' Missing Data
        MissingDataBranchCode

        ' Invalid Lookups
        InvalidLookupBranchCode

    End Enum

#End Region

#Region "Call ProxyWS Method"

    Private Sub GETRISKS( _
        Optional ByVal TestCase As TestCaseScenario = TestCaseScenario.None, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        'TODO: REPLACE REQUEST AND RESPONSE NAMES WITH VALID ONES WHEN USING FOR REAL
        Dim oRequest As New ProxyWS.GetRiskRequestType
        Dim oResponse As ProxyWS.GetRiskResponseType

        Try
            Dim con As New SqlConnection("Data Source=S4IDevServer;Database=SAMStaging;Integrated Security=SSPI;")
            Dim queryString As String = "SELECT risk_key, policy_key, IFILE.INSURANCE_fOLDER_CNT ," & _
                                         " source.code from SAMStaging..Risk " & _
                                         " inner JOIN SIRIUS..INSURANCE_FILE IFILE on " & _
                                         " SAMSTAGING..RISK.POLICY_KEY = IFILE.INSURANCE_fILE_CNT " & _
                                        " inner join sirius..source source on " & _
                                        " source.source_id = ifile.source_id "
            Dim da As SqlDataAdapter = New SqlDataAdapter(queryString, con)
            Dim ds As DataSet = New DataSet

            da.Fill(ds, "Policies")

            Dim dt As DataTable = Nothing
            If ds IsNot Nothing Then
                If ds.Tables.Count > 0 Then
                    dt = ds.Tables(0)
                End If
            End If

            For Each dr As DataRow In dt.Rows
                oRequest.InsuranceFileKey = dr.Item("policy_key")
                oRequest.InsuranceFolderKey = dr.Item("insurance_folder_cnt")
                oRequest.BranchCode = dr.Item("code").ToString.Trim
                oRequest.RiskKey = dr.Item("risk_key")

                SetWSETestCaseScenario(nWSETestCaseScenario)
                oResponse = oProxy.GetRisk(oRequest)

                With oResponse

                    Dim cmd As New SqlCommand
                    cmd.CommandText = "Update Risk Set XMLDATASET = """ & oResponse.XMLDataSet & """) where risk_key = " & oRequest.RiskKey
                    cmd.CommandType = CommandType.Text
                    cmd.Connection = con
                    cmd.ExecuteNonQuery()

                End With

            Next

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

#Region "SetupTestCaseScenarios"

    ''' <summary>
    ''' Setup Test Case Scenarios which should not raise errors
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>
    '''

#Region "Setup NonErrorScenarios Test Cases"


    Private Sub AllNonMandatoryFieldsMissing(ByVal oRequest As ProxyWS.OpenClaimRequestType)


    End Sub

#End Region

    ''' <summary>
    ''' Setup Test Case Scenarios which return invalid lookup errors
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>

#Region "Setup Invalid Lookups Test Cases"

    Private Sub InvalidLookupBranchCode(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.BranchCode = kInvalidLookupValue
    End Sub

#End Region

    ''' <summary>
    ''' Setup Test Case Scenarios which return missing data errors
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>

#Region "Setup Missing Data Test Cases"

    Private Sub MissingDataBranchCode(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        oRequest.BranchCode = String.Empty
    End Sub

#End Region

    ''' <summary>
    ''' Setup Test Case Scenarios which return business warnings
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>

#Region "Setup Business Warnings Test Cases"

    Private Sub BusinessWarningEXAMPLE(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        '    oRequest.Claim.LossFromDate = DateAdd(DateInterval.Day, 1, Date.Today)
    End Sub

#End Region

    ''' <summary>
    ''' Setup Test Case Scenarios which return business errrors
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>
#Region "Setup Business Errors Test Cases"

    Private Sub BusinessErrorEXAMPLE(ByVal oRequest As ProxyWS.OpenClaimRequestType)
        '    oRequest.Claim.LossFromDate = DateAdd(DateInterval.Day, 1, Date.Today)
    End Sub

#End Region

#End Region

#Region "Test Case Response Handlers"

    Private Sub ProcessTestCases(ByVal oRequest As ProxyWS.OpenClaimRequestType, ByVal TestCase As TestCaseScenario)

        Select Case TestCase

            ' error free scenarios
            Case TestCaseScenario.AllNonMandatoryFieldsMissing
                AllNonMandatoryFieldsMissing(oRequest)

                ' invalid lookup data scenarios
            Case TestCaseScenario.InvalidLookupBranchCode
                InvalidLookupBranchCode(oRequest)

                ' Missing Data
            Case TestCaseScenario.MissingDataBranchCode
                MissingDataBranchCode(oRequest)


            Case Else
                ' do nothing
        End Select

    End Sub

    Private Sub ProcessBusinessErrors(ByVal oResponse As ProxyWS.BaseResponseType, ByVal TestCase As TestCaseScenario)

        ' raise an error if no error was returned
        SAMTest.AssertCallFailedWithErrors(oResponse, 1)

        ' assign the error object
        Dim oError As ProxyWS.SAMErrorBusinessRule = SAMTest.AssertErrorBusinessRule(oResponse, 0)

        ' raise an error if an invalid lookup table is being used (field name holds the lookup table name)
        Select Case TestCase
            'Case TestCaseScenario.BusinessErrorDuplicateClaimExists
            '    Assert.AreEqual(SAMConstants.SAMBusinessErrors.DuplicateClaimExists, oError.Code, kInvalidCodeReturned)
            '    Assert.AreEqual(SAMConstants.SAMBusinessErrors.DuplicateClaimExists.ToString(), oError.Description, kInvalidDescriptionReturned)
        End Select

    End Sub

    Private Sub ProcessBusinessWarnings(ByVal oResponse As ProxyWS.BaseClaimResponseType, ByVal TestCase As TestCaseScenario)

        ' raise an error if no warnings was returned
        SAMTest.AssertCallSucceedededWithWarnings(oResponse, 1)

        ' assign the warning object
        Dim oWarning As ProxyWS.BaseClaimResponseTypeWarnings = SAMTest.AssertWarning(oResponse, 0)

        ' raise an errror if the error code (id) and description dont match the expected values]
        ' for the specified test case scenario
        Select Case TestCase
            'Case TestCaseScenario.BusinessWarningInfoOnlyClaimDataTruncated
            '    Assert.AreEqual(SAMConstants.SAMBusinessWarnings.InfoOnlyClaimDataTruncated, oWarning.Code, kInvalidCodeReturned)
            '    Assert.AreEqual(SAMConstants.SAMBusinessWarnings.InfoOnlyClaimDataTruncated.ToString(), oWarning.Description, kInvalidDescriptionReturned)
        End Select

    End Sub

    Private Sub ProcessMissingData(ByVal oResponse As ProxyWS.BaseResponseType, ByVal TestCase As TestCaseScenario)

        ' raise an error if no error was returned
        SAMTest.AssertCallFailedWithErrors(oResponse, 1)

        ' assign the error object
        Dim oError As ProxyWS.SAMErrorInvalidData = SAMTest.AssertErrorInvalidData(oResponse, 0)

        ' raise an errror if the error code (id) doesnt match the expected value
        Assert.AreEqual(SAMConstants.SAMInvalidData.MandatoryInputMissing, oError.Code, kInvalidCodeReturned)

        ' raise an error if the error description doesnt not match the expected value
        Assert.AreEqual(SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString(), oError.Description, kInvalidDescriptionReturned)

        ' raise an error if the supplied value is not the one specified
        'Assert.AreEqual(kInvalidLookupValue, oError.SuppliedValue, kInvalidLookupValueMessage)

        ' raise an error if an invalid lookup table is being used (field name holds the lookup table name)
        Select Case TestCase
            Case TestCaseScenario.MissingDataBranchCode
                Assert.AreEqual("BranchCode", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
        End Select

    End Sub

    Private Sub ProcessInvalidLookup(ByVal oResponse As ProxyWS.BaseResponseType, ByVal TestCase As TestCaseScenario)

        ' raise an error if no error was returned
        SAMTest.AssertCallFailedWithErrors(oResponse, 1)

        ' assign the error object
        Dim oError As ProxyWS.SAMErrorInvalidData = SAMTest.AssertErrorInvalidData(oResponse, 0)

        ' raise an errror if the error code (id) doesnt match the expected value
        Assert.AreEqual(SAMConstants.SAMInvalidData.InvalidLookupListValue, oError.Code, kInvalidCodeReturned)

        ' raise an error if the error description doesnt not match the expected value
        Assert.AreEqual(SAMConstants.SAMInvalidData.InvalidLookupListValue.ToString(), Mid(oError.Description.ToString, 1, Len(SAMConstants.SAMInvalidData.InvalidLookupListValue.ToString)), kInvalidDescriptionReturned)

        ' raise an error if the supplied value is not the one specified
        Assert.AreEqual(kInvalidLookupValue, oError.SuppliedValue, kInvalidLookupValueMessage)

        ' raise an error if an invalid lookup table is being used (field name holds the lookup table name)
        Select Case TestCase
            Case TestCaseScenario.InvalidLookupBranchCode
                Assert.AreEqual("BranchCode", oError.FieldName, kInvalidLookupFieldMessage)
        End Select

    End Sub

#End Region

#Region "NUNIT Test Scenarios"

#Region "NUNIT Success Test Case"

    <Test()> _
    Public Sub Success()

        GETRISKS()
        'TestCaseAllInvalidLookups()
        'TestCaseAllMissingData()
        'TestCaseAllBusinessWarnings()
        'TestCaseAllBusinessErrors()

    End Sub

#End Region

#Region "NUNIT Non Error Raising Scenarios"

    <Test()> _
    Public Sub AllNonMandatoryFieldsMissing()
        ' 'TemplateTestCaseTest(TestCase:=TestCaseScenario.AllNonMandatoryFieldsMissing)
    End Sub


#End Region

#Region "NUNIT Invalid Lookup Test Cases"

    <Test()> _
    Public Sub InvalidLookupBranchCode()
        'TemplateTestCaseTest(TestCase:=TestCaseScenario.InvalidLookupBranchCode)
    End Sub

    Private Sub TestCaseAllInvalidLookups()

        InvalidLookupBranchCode()

    End Sub

#End Region

#Region "NUNIT Missing Data Test Cases"

    <Test()> _
    Public Sub MissingDataBranchCode()
        'TemplateTestCaseTest(TestCase:=TestCaseScenario.MissingDataBranchCode)
    End Sub

    <Ignore()> _
    Public Sub TestCaseAllMissingData()
        MissingDataBranchCode()
    End Sub

#End Region

#Region "NUNIT Business Warnings Test Cases"

    '<Test()> _
    'Public Sub BusinessWarningInfoOnlyClaimDataTruncated()
    '    OpenClaimTest(TestCase:=TestCaseScenario.BusinessWarningInfoOnlyClaimDataTruncated)
    'End Sub

    <Ignore()> _
    Public Sub TestCaseAllBusinessWarnings()
        'BusinessWarningInfoOnlyClaimDataTruncated()
    End Sub

#End Region

#Region "NUNIT Business Error Test Cases"

    '<Test()> _
    'Public Sub BusinessErrorDuplicateClaimExists()
    '    OpenClaimTest(TestCase:=TestCaseScenario.BusinessErrorDuplicateClaimExists)
    'End Sub

    Private Sub TestCaseAllBusinessErrors()
        'BusinessErrorDuplicateClaimExists()
    End Sub

#End Region

#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        'TemplateTestCaseTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        'TemplateTestCaseTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        'TemplateTestCaseTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        'TemplateTestCaseTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class
