'Imports NUnit.Framework

'<TestFixture()> _
'Public Class ClaimMTA
'    Inherits BaseTest

'#Region "Private Declarations"

'    Private m_oTestData As New TestData

'    Private Enum enumMissingData
'        None
'        BranchCode
'        ClaimKey
'    End Enum

'    Private Enum enumInvalidLookup
'        None
'        BranchCode
'    End Enum

'    Private Enum enumSTSBusinessError
'        None
'        InvalidBranchCode
'        InvalidClaimKey
'    End Enum

'#End Region

'#Region "Setup Preconditions"

'#End Region

'#Region "Private Test Methods"

'    Private Function ClaimMTATest( _
'        Optional ByVal eMissingData As enumMissingData = enumMissingData.None, _
'        Optional ByVal eInvalidLookup As enumInvalidLookup = enumInvalidLookup.None, _
'        Optional ByVal eSTSBusinessError As enumSTSBusinessError = enumSTSBusinessError.None, _
'        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None) As ProxyWS.ClaimMTAResponseType

'        Dim oRequest As New ProxyWS.ClaimMTARequestType
'        Dim oResponse As ProxyWS.ClaimMTAResponseType = Nothing
'        Dim nLookupError As Integer = 102
'        Dim nBusinessError As Integer = 224

'        Try

'            oRequest.BranchCode = m_oTestData.ClaimMTABranchCode  '"HeadOff"
'            oRequest.SubBranchCode = "HeadOff"
'            oRequest.InsuranceFileKey = 110 'm_oTestData.ClaimMTAInsuranceFileKey
'            oRequest.InsuranceFolderKey = 66 'm_oTestData.ClaimMTAInsuranceFolderKey
'            'objClaimMTARequestType.QuoteTimeStamp =  ''m_oTestData.cl
'            oRequest.RiskDescription = m_oTestData.ClaimMTARiskDescription '"Homeowners"
'            oRequest.RiskKey = 122 ' m_oTestData.ClaimMTARiskKey '
'            oRequest.ScreenCode = m_oTestData.ClaimMTAScreenCode '"HOMEOWNERS"
'            'oRequest.XMLDataSet = m_oTestData.ClaimMTAXMLDataSet 'TransformDatasetSAMtoPB(ClaimMTARequest.XMLDataSet)

'            SetWSETestCaseScenario(nWSETestCaseScenario)
'            oResponse = oProxy.ClaimMTA(oRequest)
'            With oResponse

'                If eMissingData <> enumMissingData.None Then
'                    ' Missing Data tests
'                    SAMTest.AssertCallFailedWithErrors(oResponse, 1)
'                    SAMTest.AssertErrorInvalidData(oResponse, 0, 100, "Mandatory " & eMissingData.ToString & " is missing")
'                ElseIf eInvalidLookup <> enumInvalidLookup.None Then
'                    ' Invalid Lookup tests
'                    SAMTest.AssertCallFailedWithErrors(oResponse, 1)
'                    SAMTest.AssertErrorInvalidData(oResponse, 0, nLookupError, eInvalidLookup.ToString & " is invalid")
'                ElseIf eSTSBusinessError <> enumSTSBusinessError.None Then
'                    ' Business Rule tests
'                    SAMTest.AssertCallFailedWithErrors(oResponse, 1)
'                    SAMTest.AssertErrorBusinessRule(oResponse, 0, nBusinessError)
'                Else
'                    ' Success Tests
'                    SAMTest.AssertCallSucceeded(oResponse)
'                    'Assert.IsNotNull(.Policies, "No Result Set returned")
'                    'TODO: additional assert test
'                End If
'            End With

'        Catch ex As AssertionException
'            Throw
'        Catch ex As SoapException
'            WSETest.HandleException(ex, nWSETestCaseScenario)
'        Catch ex As Exception
'            WSETest.HandleException(ex)
'        Finally
'            oRequest = Nothing
'            oResponse = Nothing
'        End Try

'        Return oResponse

'    End Function

'    Public Sub TestResponse()

'    End Sub

'    Public Function SupportMethod(ByVal ClaimId As Integer, _
'                                    ByVal BranchCode As String) As ProxyWS.ClaimMTAResponseType
'        Return Success(ClaimId, BranchCode)
'    End Function

'#End Region

'#Region "Success"

'    <Test()> _
'    Public Sub Success()
'        ClaimMTATest()
'    End Sub

'    Public Function Success(ByVal ClaimId As Integer, _
'                            ByVal BranchCode As String) As ProxyWS.ClaimMTAResponseType
'        Return ClaimMTATest() ' TODO: Gaurav Arora - what to do with ClaimId and BranchCode?
'    End Function

'#End Region

'#Region "Missing Data"

'    <Test()> _
'    Public Sub Missing_BranchCode()
'        ClaimMTATest(eMissingData:=enumMissingData.BranchCode)
'    End Sub
'    <Test()> _
'    Public Sub Missing_ClaimKey()
'        ClaimMTATest(eMissingData:=enumMissingData.ClaimKey)
'    End Sub

'#End Region

'#Region "Data Scenario"

'#End Region

'#Region "Invalid Lookup"

'    <Test()> _
'    Public Sub InvalidData_BranchCode()
'        ClaimMTATest(eInvalidLookup:=enumInvalidLookup.BranchCode)
'    End Sub

'#End Region

'#Region "STS Business Rules"

'#End Region

'#Region "WSE Security"

'    <Test()> _
'    Public Sub WSESecurity_MissingSecurity()
'        ClaimMTATest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
'    End Sub

'    <Test()> _
'    Public Sub WSESecurity_InvalidUserName()
'        ClaimMTATest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
'    End Sub

'    <Test()> _
'    Public Sub WSESecurity_InvalidPassword()
'        ClaimMTATest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
'    End Sub

'    <Test()> _
'    Public Sub WSESecurity_InvalidTaskCode()
'        ClaimMTATest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
'    End Sub

'#End Region

'End Class
