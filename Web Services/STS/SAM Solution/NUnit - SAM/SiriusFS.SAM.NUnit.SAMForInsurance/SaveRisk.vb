'Imports NUnit.Framework

'<TestFixture()> _
'Public Class SaveRisk
'    Inherits BaseTest

'#Region "Private Declarations"

'    Private m_nPartyCnt As Integer
'    Private m_nInsuranceFolderCnt As Integer
'    Private m_nInsuranceFileCnt As Integer
'    Private m_btQuoteTimeStamp() As Byte
'    Private m_sRiskDataXML As String
'    Private m_nRiskCnt As Integer
'    Private m_oTestData As New TestData

'    Private Enum enumMissingData
'        'AgentKey
'        BranchCode
'        InsuranceFileKey
'        InsuranceFolderKey
'        RiskKey
'        XMLDataSet
'        ScreenCode
'        RiskDescription
'        QuoteTimeStamp
'        None
'    End Enum

'    Private Enum enumInvalidLookup
'        None
'        BranchCode
'        ScreenCode
'    End Enum

'    Private Enum enumSTSBusinessError
'        None
'        'InvalidAgentKey
'        InvInsuranceFolderKey
'        InvInsuranceFileKey
'        InvInsuranceFileFolder
'        InvRiskKey
'        InvInsuranceFileRisk
'    End Enum

'#End Region

'    '#Region "Setup preconditions"

'    '    Private Sub ClaimMTA()

'    '        Dim oClaimMTA As New ClaimMTA
'    '        Dim oClaimMTAResponse As ProxyWS.ClaimMTAResponseType
'    '        'oClaimMTAResponse = oClaimMTA.SupportMethod(m_nInsuranceFolderCnt)

'    '        m_nInsuranceFileCnt = oClaimMTAResponse.InsuranceFileKey
'    '        m_nRiskCnt = oClaimMTAResponse.RiskKey
'    '        m_btQuoteTimeStamp = oClaimMTAResponse.TimeStamp
'    '        m_sRiskDataXML = oClaimMTAResponse.XMLDataSet

'    '    End Sub

'    '#End Region

'#Region "Success"

'    Public Sub SupportMethod(ByRef r_nInsuranceFileCnt As Integer)
'        Success()
'        r_nInsuranceFileCnt = m_nInsuranceFileCnt
'    End Sub

'    'Private Sub SuccessTest( _
'    '    Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

'    '    Dim oRequest As New ProxyWS.SaveRiskRequestType
'    '    Dim oResponse As ProxyWS.SaveRiskResponseType

'    '    Try
'    '        ClaimMTA()

'    '        Dim xmlDoc As New System.Xml.XmlDocument
'    '        Dim oElementToAddOrUpdate As XmlNode
'    '        Dim oElementToAddOrUpdateAdded As XmlNode
'    '        Dim newAttribute As System.Xml.XmlAttribute
'    '        Dim nNextOINumber As Integer
'    '        Dim bNewElement As Boolean

'    '        ' Read in the XML created in the AddRisk
'    '        xmlDoc.LoadXml(m_sRiskDataXML)

'    '        ' Get the NextOINumber and increment it
'    '        nNextOINumber = xmlDoc.SelectSingleNode("DATA_SET") _
'    '            .Attributes("NextOINumber").Value

'    '        nNextOINumber = nNextOINumber + 1

'    '        ' Create the new test elements
'    '        For iElementCnt As Integer = m_oTestData.XMLDataSetElementsToAdd.GetLowerBound(0) To m_oTestData.XMLDataSetElementsToAdd.GetUpperBound(0)

'    '            ' Check if the element already exists
'    '            oElementToAddOrUpdate = xmlDoc.SelectSingleNode("DATA_SET") _
'    '                                .SelectSingleNode("RISK_OBJECTS") _
'    '                                .SelectSingleNode(m_oTestData.DataModelCode & "_POLICY_BINDER") _
'    '                                .SelectSingleNode(m_oTestData.XMLDataSetElementsToAdd(iElementCnt).ElementName)
'    '            ' If not, create it
'    '            If oElementToAddOrUpdate Is Nothing Then
'    '                oElementToAddOrUpdate = xmlDoc.CreateNode(XmlNodeType.Element, m_oTestData.XMLDataSetElementsToAdd(iElementCnt).ElementName, "")

'    '                ' Add the common Object Instance (OI) and Update
'    '                ' Status (US) attributes
'    '                newAttribute = xmlDoc.CreateAttribute("OI")
'    '                newAttribute.Value = "OI" & nNextOINumber.ToString
'    '                oElementToAddOrUpdate.Attributes.Append(newAttribute)
'    '                nNextOINumber += 1

'    '                newAttribute = xmlDoc.CreateAttribute("US")
'    '                newAttribute.Value = "1"
'    '                oElementToAddOrUpdate.Attributes.Append(newAttribute)
'    '                bNewElement = True
'    '            Else
'    '                ' Update the Update
'    '                ' Status (US) attribute
'    '                oElementToAddOrUpdate.Attributes("US").Value = "2"
'    '                bNewElement = False
'    '            End If

'    '            ' Add specific attributes for this test
'    '            For iAttrCnt As Integer = m_oTestData.XMLDataSetElementsToAdd(iElementCnt).Attributes.GetLowerBound(0) To m_oTestData.XMLDataSetElementsToAdd(iElementCnt).Attributes.GetUpperBound(0)
'    '                newAttribute = xmlDoc.CreateAttribute(m_oTestData.XMLDataSetElementsToAdd(iElementCnt).Attributes(iAttrCnt).AttributeName)
'    '                newAttribute.Value = m_oTestData.XMLDataSetElementsToAdd(iElementCnt).Attributes(iAttrCnt).AttributeValue
'    '                oElementToAddOrUpdate.Attributes.Append(newAttribute)
'    '            Next

'    '            If bNewElement Then
'    '                ' Append the new element node to the XML under the POLICY BINDER
'    '                oElementToAddOrUpdateAdded = xmlDoc.SelectSingleNode("DATA_SET") _
'    '                    .SelectSingleNode("RISK_OBJECTS") _
'    '                    .SelectSingleNode(m_oTestData.DataModelCode & "_POLICY_BINDER") _
'    '                    .AppendChild(oElementToAddOrUpdate)
'    '            End If

'    '        Next

'    '        ' Write back the next OI number to the DATA_SET node
'    '        nNextOINumber -= 1
'    '        xmlDoc.SelectSingleNode("DATA_SET") _
'    '            .Attributes("NextOINumber").Value = nNextOINumber

'    '        With oRequest
'    '            .BranchCode = m_oTestData.BranchCode
'    '            .InsuranceFileKey = m_nInsuranceFileCnt
'    '            .InsuranceFolderKey = m_nInsuranceFolderCnt
'    '            .RiskKey = m_nRiskCnt
'    '            .XMLDataSet = xmlDoc.OuterXml
'    '            .QuoteTimeStamp = m_btQuoteTimeStamp
'    '        End With

'    '        SetWSETestCaseScenario(nWSETestCaseScenario)
'    '        oResponse = oProxy.SaveRisk(oRequest)

'    '        SAMTest.AssertCallSucceeded(oResponse)

'    '    Catch ex As AssertionException
'    '        Throw
'    '    Catch ex As SoapException
'    '        WSETest.HandleException(ex, nWSETestCaseScenario)
'    '    Catch ex As Exception
'    '        WSETest.HandleException(ex)
'    '    Finally
'    '        oRequest = Nothing
'    '        oResponse = Nothing
'    '    End Try

'    'End Sub

'    <Test()> _
'    Public Sub Success()
'        SuccessTest()
'    End Sub

'#End Region

'#Region "Invalid Data"

'#Region "Mandatory Data Missing"

'    Private Sub MissingDataTest(ByVal eMissingData As enumMissingData)

'        Dim oRequest As New ProxyWS.SaveRiskRequestType
'        Dim oResponse As ProxyWS.SaveRiskResponseType

'        Try

'            With oRequest
'                If eMissingData <> enumMissingData.BranchCode Then
'                    .BranchCode = "DATA"
'                End If
'                If eMissingData <> enumMissingData.InsuranceFileKey Then
'                    .InsuranceFileKey = 1
'                End If
'                If eMissingData <> enumMissingData.InsuranceFolderKey Then
'                    .InsuranceFolderKey = 1
'                End If
'                If eMissingData <> enumMissingData.RiskKey Then
'                    .RiskKey = 1
'                End If
'                If eMissingData <> enumMissingData.XMLDataSet Then
'                    .XMLDataSet = m_oTestData.RiskDataXML
'                End If
'                If eMissingData <> enumMissingData.QuoteTimeStamp Then
'                    .QuoteTimeStamp = kanEmptyTimeStamp
'                End If
'            End With

'            SetWSETestCaseScenario(WSETestCaseScenario.None)
'            oResponse = oProxy.SaveRisk(oRequest)

'            With oResponse
'                SAMTest.AssertCallFailedWithErrors(oResponse, 1)
'                SAMTest.AssertErrorInvalidData(oResponse, 0, 100, "Mandatory " & eMissingData.ToString & " is missing")
'            End With

'        Catch ex As AssertionException
'            Throw
'        Catch ex As SoapException
'            WSETest.HandleException(ex, WSETestCaseScenario.None)
'        Catch ex As Exception
'            WSETest.HandleException(ex)
'        Finally
'            oRequest = Nothing
'            oResponse = Nothing
'        End Try

'    End Sub

'    <Test()> _
'    Public Sub InvalidData_Missing_BranchCode()
'        MissingDataTest(enumMissingData.BranchCode)
'    End Sub
'    <Test()> _
'    Public Sub InvalidData_Missing_InsuranceFileKey()
'        MissingDataTest(enumMissingData.InsuranceFileKey)
'    End Sub
'    <Test()> _
'    Public Sub InvalidData_Missing_InsuranceFolderkey()
'        MissingDataTest(enumMissingData.InsuranceFolderKey)
'    End Sub
'    <Test()> _
'    Public Sub InvalidData_Missing_RiskKey()
'        MissingDataTest(enumMissingData.RiskKey)
'    End Sub
'    <Test()> _
'    Public Sub InvalidData_Missing_XMLDataSet()
'        MissingDataTest(enumMissingData.XMLDataSet)
'    End Sub
'    <Test()> _
'    Public Sub InvalidData_Missing_QuoteTimeStamp()
'        MissingDataTest(enumMissingData.QuoteTimeStamp)
'    End Sub

'#End Region

'#Region "Invalid Format"

'#End Region

'#Region "Invalid List Value"

'    Private Sub InvalidLookupTest(ByVal eInvalidLookup As enumInvalidLookup)

'        Dim oRequest As New ProxyWS.SaveRiskRequestType
'        Dim oResponse As ProxyWS.SaveRiskResponseType
'        Dim nLookupError As Integer = 102

'        ClaimMTA()

'        Try

'            With oRequest
'                If eInvalidLookup = enumInvalidLookup.BranchCode Then
'                    .BranchCode = m_oTestData.InvalidLookupCode
'                    nLookupError = 210
'                Else
'                    .BranchCode = m_oTestData.BranchCode
'                End If
'                .InsuranceFileKey = m_nInsuranceFileCnt
'                .InsuranceFolderKey = m_nInsuranceFolderCnt
'                .RiskKey = m_nRiskCnt
'                .XMLDataSet = m_sRiskDataXML
'                .QuoteTimeStamp = m_btQuoteTimeStamp
'            End With

'            SetWSETestCaseScenario(WSETestCaseScenario.None)
'            oResponse = oProxy.SaveRisk(oRequest)

'            With oResponse
'                SAMTest.AssertCallFailedWithErrors(oResponse, 1)
'                SAMTest.AssertErrorInvalidData(oResponse, 0, nLookupError, eInvalidLookup.ToString & " is invalid")
'            End With

'        Catch ex As AssertionException
'            Throw
'        Catch ex As SoapException
'            WSETest.HandleException(ex, WSETestCaseScenario.None)
'        Catch ex As Exception
'            WSETest.HandleException(ex)
'        Finally
'            oRequest = Nothing
'            oResponse = Nothing
'        End Try

'    End Sub

'    <Test()> _
'    Public Sub InvalidData_BranchCode()
'        InvalidLookupTest(enumInvalidLookup.BranchCode)
'    End Sub

'#End Region

'#End Region

'#Region "STS Business Rules"

'    Private Sub STSBusinessRulesTest(ByVal eSTSBusinessError As enumSTSBusinessError)

'        Dim oRequest As New ProxyWS.SaveRiskRequestType
'        Dim oResponse As ProxyWS.SaveRiskResponseType
'        Dim nBusinessError As Integer = 224

'        Try

'            ClaimMTA()

'            With oRequest
'                .BranchCode = m_oTestData.BranchCode
'                Select Case eSTSBusinessError
'                    Case enumSTSBusinessError.InvInsuranceFileFolder
'                        .InsuranceFileKey = m_nInsuranceFileCnt
'                        .InsuranceFolderKey = m_oTestData.InvalidInsFileFolderCnt
'                        .RiskKey = m_nRiskCnt
'                        nBusinessError = 212
'                    Case enumSTSBusinessError.InvInsuranceFileKey
'                        .InsuranceFileKey = m_oTestData.InvalidCnt
'                        .InsuranceFolderKey = m_nInsuranceFolderCnt
'                        .RiskKey = m_nRiskCnt
'                    Case enumSTSBusinessError.InvInsuranceFileRisk
'                        .InsuranceFileKey = m_nInsuranceFileCnt
'                        .InsuranceFolderKey = m_nInsuranceFolderCnt
'                        .RiskKey = m_oTestData.InvalidInsFileRiskCnt
'                        nBusinessError = 219
'                    Case enumSTSBusinessError.InvInsuranceFolderKey
'                        .InsuranceFileKey = m_nInsuranceFileCnt
'                        .InsuranceFolderKey = m_oTestData.InvalidCnt
'                        .RiskKey = m_nRiskCnt
'                    Case enumSTSBusinessError.InvRiskKey
'                        .InsuranceFileKey = m_nInsuranceFileCnt
'                        .InsuranceFolderKey = m_nInsuranceFolderCnt
'                        .RiskKey = m_oTestData.InvalidCnt
'                        nBusinessError = 229
'                End Select
'                .XMLDataSet = m_oTestData.RiskDataXML
'                .QuoteTimeStamp = kanEmptyTimeStamp
'            End With

'            SetWSETestCaseScenario(WSETestCaseScenario.None)
'            oResponse = oProxy.SaveRisk(oRequest)

'            With oResponse
'                ' Business Rule tests
'                SAMTest.AssertCallFailedWithErrors(oResponse, 1)
'                SAMTest.AssertErrorBusinessRule(oResponse, 0, nBusinessError)
'            End With

'        Catch ex As AssertionException
'            Throw
'        Catch ex As SoapException
'            WSETest.HandleException(ex, WSETestCaseScenario.None)
'        Catch ex As Exception
'            WSETest.HandleException(ex)
'        Finally
'            oRequest = Nothing
'            oResponse = Nothing
'        End Try
'    End Sub

'    <Test()> _
'    Public Sub STSBusiness_InsFolderCnt()
'        STSBusinessRulesTest(eSTSBusinessError:=enumSTSBusinessError.InvInsuranceFolderKey)
'    End Sub
'    <Test()> _
'    Public Sub STSBusiness_InsFileCnt()
'        STSBusinessRulesTest(eSTSBusinessError:=enumSTSBusinessError.InvInsuranceFileKey)
'    End Sub
'    <Test()> _
'    Public Sub STSBusiness_RiskCnt()
'        STSBusinessRulesTest(eSTSBusinessError:=enumSTSBusinessError.InvRiskKey)
'    End Sub
'    <Test()> _
'    Public Sub STSBusiness_InsFileFolder()
'        STSBusinessRulesTest(eSTSBusinessError:=enumSTSBusinessError.InvInsuranceFileFolder)
'    End Sub
'    <Test()> _
'    Public Sub STSBusiness_InsFileRisk()
'        STSBusinessRulesTest(eSTSBusinessError:=enumSTSBusinessError.InvInsuranceFileRisk)
'    End Sub

'#End Region

'#Region "Sirius Back Office"

'#End Region

'#Region "WSE Security"

'    <Test()> _
'    Public Sub WSESecurity_MissingSecurity()
'        SuccessTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
'    End Sub

'    <Test()> _
'    Public Sub WSESecurity_InvalidUserName()
'        SuccessTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
'    End Sub

'    <Test()> _
'    Public Sub WSESecurity_InvalidPassword()
'        SuccessTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
'    End Sub

'    <Test()> _
'    Public Sub WSESecurity_InvalidTaskCode()
'        SuccessTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
'    End Sub

'#End Region

'End Class
