Imports NUnit.Framework

<TestFixture()> _
Public Class UpdateRisk
    Inherits BaseTest

#Region "Private Declarations"

    Private m_nPartyCnt As Integer
    Private m_nInsuranceFolderCnt As Integer
    Private m_nInsuranceFileCnt As Integer
    Private m_btQuoteTimeStamp() As Byte
    Private m_sRiskDataXML As String
    Private m_nRiskCnt As Integer
    Private m_oTestData As New TestData
    Private m_dGrossPremium As Double

    Private Enum TestCaseScenario

        ' Non Error Scenarios
        None
        MTA
        MTAWithInstalments

    End Enum

    Private Enum enumMissingData
        'AgentKey
        BranchCode
        InsuranceFileKey
        InsuranceFolderKey
        RiskKey
        XMLDataSet
        ScreenCode
        RiskDescription
        QuoteTimeStamp
        None
    End Enum

    Private Enum enumInvalidLookup
        None
        BranchCode
        ScreenCode
    End Enum

    Private Enum enumSTSBusinessError
        None
        'InvalidAgentKey
        InvInsuranceFolderKey
        InvInsuranceFileKey
        InvInsuranceFileFolder
        InvRiskKey
        InvInsuranceFileRisk
    End Enum

#End Region

#Region "Setup preconditions"

    Private Sub AddRisk()

        Dim oAddRisk As New AddRisk
        oAddRisk.SupportMethod(m_nInsuranceFileCnt, _
                                m_nInsuranceFolderCnt, _
                                m_nRiskCnt, _
                                m_sRiskDataXML, _
                                m_btQuoteTimeStamp)

    End Sub

    Private Sub AddMTAQuote(ByVal withInstalments As Boolean)

        Dim AddMTAQuote As New AddMTAQuote
        AddMTAQuote.SupportMethod(m_nInsuranceFileCnt, _
                                  m_btQuoteTimeStamp, _
                                  withInstalments)


        Dim risks() As ProxyWS.BaseGetHeaderAndSummariesResponseTypeRow = Nothing
        Dim risk As ProxyWS.BaseGetHeaderAndSummariesResponseTypeRow = Nothing


        Dim GetHeaderAndSummaries As New GetHeaderAndSummariesByKey
        GetHeaderAndSummaries.SupportMethod(m_nInsuranceFolderCnt, _
                                m_nInsuranceFileCnt, _
                                m_btQuoteTimeStamp, _
                                risks)

        If IsArray(risks) AndAlso risks.Length > 0 Then
            risk = risks(0)
            m_nRiskCnt = risk.RiskKey
            Dim GetRisk As New GetRisk
            GetRisk.SupportMethod(m_nRiskCnt, m_nInsuranceFolderCnt, m_nInsuranceFileCnt, m_btQuoteTimeStamp, m_sRiskDataXML)
        End If

    End Sub


#End Region

#Region "Success"

    Public Overloads Sub SupportMethod(ByRef r_nInsuranceFileCnt As Integer)
        Success()
        r_nInsuranceFileCnt = m_nInsuranceFileCnt
    End Sub

    Public Overloads Sub SupportMethod(ByRef r_nInsuranceFileCnt As Integer, ByRef riskCnt As Integer, ByRef grossPremium As Double)
        Success()
        r_nInsuranceFileCnt = m_nInsuranceFileCnt
        riskCnt = m_nRiskCnt
        grossPremium = m_dGrossPremium
    End Sub

    Public Overloads Sub SupportMethodMTAWithInstalments(ByRef r_nInsuranceFileCnt As Integer, ByRef riskCnt As Integer, ByRef grossPremium As Double)
        SuccessWithMTAAndInstalments()
        r_nInsuranceFileCnt = m_nInsuranceFileCnt
        riskCnt = m_nRiskCnt
        grossPremium = m_dGrossPremium
    End Sub

    Private Sub SuccessTest( _
        Optional ByVal nTestcaseScenario As TestCaseScenario = TestCaseScenario.None, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.UpdateRiskRequestType
        Dim oResponse As ProxyWS.UpdateRiskResponseType

        Try

            If nTestcaseScenario = TestCaseScenario.None Then
                AddRisk()
            ElseIf nTestcaseScenario = TestCaseScenario.MTA Then
                AddMTAQuote(False)
            ElseIf nTestcaseScenario = TestCaseScenario.MTAWithInstalments Then
                AddMTAQuote(True)
            End If

            Dim xmlDoc As New System.Xml.XmlDocument
            Dim oElementToAddOrUpdate As XmlNode
            Dim oElementToAddOrUpdateAdded As XmlNode
            Dim newAttribute As System.Xml.XmlAttribute
            Dim nNextOINumber As Integer
            Dim bNewElement As Boolean

            ' Read in the XML created in the AddRisk
            xmlDoc.LoadXml(m_sRiskDataXML)

            ' Get the NextOINumber and increment it
            nNextOINumber = xmlDoc.SelectSingleNode("DATA_SET") _
                .Attributes("NextOINumber").Value

            nNextOINumber = nNextOINumber + 1


            Dim xmlDatasetElementsToAdd As XMLElementToAdd()
            If nTestcaseScenario = TestCaseScenario.None Then
                xmlDatasetElementsToAdd = m_oTestData.XMLDataSetElementsToAdd
            Else
                xmlDatasetElementsToAdd = m_oTestData.XMLDataSetMTAElementsToAdd
                oRequest.TransactionType = "MTA"
            End If

            ' Create the new test elements
            For Each xmlDatasetElement As XMLElementToAdd In xmlDatasetElementsToAdd

                ' Check if the element already exists
                oElementToAddOrUpdate = xmlDoc.SelectSingleNode("DATA_SET") _
                                    .SelectSingleNode("RISK_OBJECTS") _
                                    .SelectSingleNode(m_oTestData.DataModelCode & "_POLICY_BINDER") _
                                    .SelectSingleNode(xmlDatasetElement.ElementName)
                ' If not, create it
                If oElementToAddOrUpdate Is Nothing Then
                    oElementToAddOrUpdate = xmlDoc.CreateNode(XmlNodeType.Element, xmlDatasetElement.ElementName, "")

                    ' Add the common Object Instance (OI) and Update
                    ' Status (US) attributes
                    newAttribute = xmlDoc.CreateAttribute("OI")
                    newAttribute.Value = "OI" & nNextOINumber.ToString
                    oElementToAddOrUpdate.Attributes.Append(newAttribute)
                    nNextOINumber += 1

                    newAttribute = xmlDoc.CreateAttribute("US")
                    newAttribute.Value = "1"
                    oElementToAddOrUpdate.Attributes.Append(newAttribute)
                    bNewElement = True
                Else
                    ' Update the Update
                    ' Status (US) attribute
                    oElementToAddOrUpdate.Attributes("US").Value = "2"
                    bNewElement = False
                End If

                ' Add specific attributes for this test
                For Each XMLAttribute As XMLAttributeToAdd In xmlDatasetElement.Attributes
                    If InStr(XMLAttribute.AttributeName, "ADDRESS_CNT") > 0 Then
                        Dim addressElement As System.Xml.XmlNode = xmlDoc.CreateNode(XmlNodeType.Element, XMLAttribute.AttributeName, "")

                        newAttribute = xmlDoc.CreateAttribute("US")
                        newAttribute.Value = "1"
                        addressElement.Attributes.Append(newAttribute)

                        newAttribute = xmlDoc.CreateAttribute("ADDRESS_LINE1")
                        newAttribute.Value = "2500 The Crescent"
                        addressElement.Attributes.Append(newAttribute)

                        newAttribute = xmlDoc.CreateAttribute("ADDRESS_LINE2")
                        newAttribute.Value = "Birmingham Business Park"
                        addressElement.Attributes.Append(newAttribute)

                        newAttribute = xmlDoc.CreateAttribute("ADDRESS_LINE3")
                        newAttribute.Value = "Solihull"
                        addressElement.Attributes.Append(newAttribute)

                        newAttribute = xmlDoc.CreateAttribute("ADDRESS_LINE4")
                        newAttribute.Value = "West Midlands"
                        addressElement.Attributes.Append(newAttribute)

                        newAttribute = xmlDoc.CreateAttribute("POSTCODE")
                        newAttribute.Value = "B37 7YE"
                        addressElement.Attributes.Append(newAttribute)

                        newAttribute = xmlDoc.CreateAttribute("COUNTRYCODE")
                        newAttribute.Value = "GBR"
                        addressElement.Attributes.Append(newAttribute)

                        oElementToAddOrUpdate.AppendChild(addressElement)

                    ElseIf (oElementToAddOrUpdate.Attributes(XMLAttribute.AttributeName) Is Nothing) = False Then
                        oElementToAddOrUpdate.Attributes(XMLAttribute.AttributeName).Value = XMLAttribute.AttributeValue
                    Else
                        newAttribute = xmlDoc.CreateAttribute(XMLAttribute.AttributeName)
                        newAttribute.Value = XMLAttribute.AttributeValue
                        oElementToAddOrUpdate.Attributes.Append(newAttribute)
                    End If
                Next

                If bNewElement Then
                    ' Append the new element node to the XML under the POLICY BINDER
                    oElementToAddOrUpdateAdded = xmlDoc.SelectSingleNode("DATA_SET") _
                        .SelectSingleNode("RISK_OBJECTS") _
                        .SelectSingleNode(m_oTestData.DataModelCode & "_POLICY_BINDER") _
                        .AppendChild(oElementToAddOrUpdate)
                End If

            Next

            ' Write back the next OI number to the DATA_SET node
            nNextOINumber -= 1
            xmlDoc.SelectSingleNode("DATA_SET") _
                .Attributes("NextOINumber").Value = nNextOINumber

            With oRequest
                .BranchCode = m_oTestData.BranchCode
                .SubBranchCode = m_oTestData.SubBranch
                .InsuranceFileKey = m_nInsuranceFileCnt
                .InsuranceFolderKey = m_nInsuranceFolderCnt
                .RiskKey = m_nRiskCnt
                .RiskDescription = m_oTestData.RiskDescription
                .ScreenCode = m_oTestData.ScreenCode
                .XMLDataSet = xmlDoc.OuterXml
                .QuoteTimeStamp = m_btQuoteTimeStamp
            End With

            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.UpdateRisk(oRequest)

            SAMTest.AssertCallSucceeded(oResponse)

            m_sRiskDataXML = oResponse.XMLDataSet
            m_dGrossPremium = oResponse.PremiumDueGross

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

    <Test()> _
    Public Sub Success()
        SuccessTest()
    End Sub

    <Test()> _
    Public Sub SuccessWithMTA()
        'SuccessTest()
        SuccessTest(nTestcaseScenario:=TestCaseScenario.MTA)
    End Sub

    <Test()> _
    Public Sub SuccessWithMTAAndInstalments()
        'SuccessTest()
        SuccessTest(nTestcaseScenario:=TestCaseScenario.MTAWithInstalments)
    End Sub

#End Region

#Region "Invalid Data"

#Region "Mandatory Data Missing"

    Private Sub MissingDataTest(ByVal eMissingData As enumMissingData)

        Dim oRequest As New ProxyWS.UpdateRiskRequestType
        Dim oResponse As ProxyWS.UpdateRiskResponseType

        Try

            With oRequest
                If eMissingData <> enumMissingData.BranchCode Then
                    .BranchCode = "DATA"
                End If
                If eMissingData <> enumMissingData.InsuranceFileKey Then
                    .InsuranceFileKey = 1
                End If
                If eMissingData <> enumMissingData.InsuranceFolderKey Then
                    .InsuranceFolderKey = 1
                End If
                If eMissingData <> enumMissingData.RiskKey Then
                    .RiskKey = 1
                End If
                If eMissingData <> enumMissingData.XMLDataSet Then
                    .XMLDataSet = m_oTestData.RiskDataXML
                End If
                If eMissingData <> enumMissingData.QuoteTimeStamp Then
                    .QuoteTimeStamp = kanEmptyTimeStamp
                End If
                If eMissingData <> enumMissingData.RiskDescription Then
                    .RiskDescription = "DATA"
                End If
                If eMissingData <> enumMissingData.ScreenCode Then
                    .ScreenCode = "DATA"
                End If
            End With

            SetWSETestCaseScenario(WSETestCaseScenario.None)
            oResponse = oProxy.UpdateRisk(oRequest)

            With oResponse
                SAMTest.AssertCallFailedWithErrors(oResponse, 1)
                SAMTest.AssertErrorInvalidData(oResponse, 0, 100, "Mandatory " & eMissingData.ToString & " is missing")
            End With

        Catch ex As AssertionException
            Throw
        Catch ex As SoapException
            WSETest.HandleException(ex, WSETestCaseScenario.None)
        Catch ex As Exception
            WSETest.HandleException(ex)
        Finally
            oRequest = Nothing
            oResponse = Nothing
        End Try

    End Sub

    <Test()> _
    Public Sub InvalidData_Missing_BranchCode()
        MissingDataTest(enumMissingData.BranchCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_InsuranceFileKey()
        MissingDataTest(enumMissingData.InsuranceFileKey)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_InsuranceFolderkey()
        MissingDataTest(enumMissingData.InsuranceFolderKey)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_RiskKey()
        MissingDataTest(enumMissingData.RiskKey)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_XMLDataSet()
        MissingDataTest(enumMissingData.XMLDataSet)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_QuoteTimeStamp()
        MissingDataTest(enumMissingData.QuoteTimeStamp)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_RiskDescription()
        MissingDataTest(enumMissingData.RiskDescription)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_ScreenCode()
        MissingDataTest(enumMissingData.ScreenCode)
    End Sub

#End Region

#Region "Invalid Format"

#End Region

#Region "Invalid List Value"

    Private Sub InvalidLookupTest(ByVal eInvalidLookup As enumInvalidLookup)

        Dim oRequest As New ProxyWS.UpdateRiskRequestType
        Dim oResponse As ProxyWS.UpdateRiskResponseType
        Dim nLookupError As Integer = 102

        AddRisk()

        Try

            With oRequest
                If eInvalidLookup = enumInvalidLookup.BranchCode Then
                    .BranchCode = m_oTestData.InvalidLookupCode
                    nLookupError = 210
                Else
                    .BranchCode = m_oTestData.BranchCode
                End If
                .SubBranchCode = m_oTestData.SubBranch
                .InsuranceFileKey = m_nInsuranceFileCnt
                .InsuranceFolderKey = m_nInsuranceFolderCnt
                .RiskKey = m_nRiskCnt
                .RiskDescription = m_oTestData.RiskDescription
                If eInvalidLookup = enumInvalidLookup.ScreenCode Then
                    .ScreenCode = m_oTestData.InvalidLookupCode
                Else
                    .ScreenCode = m_oTestData.ScreenCode
                End If

                .XMLDataSet = m_sRiskDataXML
                .QuoteTimeStamp = m_btQuoteTimeStamp
            End With

            SetWSETestCaseScenario(WSETestCaseScenario.None)
            oResponse = oProxy.UpdateRisk(oRequest)

            With oResponse
                SAMTest.AssertCallFailedWithErrors(oResponse, 1)
                SAMTest.AssertErrorInvalidData(oResponse, 0, nLookupError, eInvalidLookup.ToString & " is invalid")
            End With

        Catch ex As AssertionException
            Throw
        Catch ex As SoapException
            WSETest.HandleException(ex, WSETestCaseScenario.None)
        Catch ex As Exception
            WSETest.HandleException(ex)
        Finally
            oRequest = Nothing
            oResponse = Nothing
        End Try

    End Sub

    <Test()> _
    Public Sub InvalidData_BranchCode()
        InvalidLookupTest(enumInvalidLookup.BranchCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_ScreenCode()
        InvalidLookupTest(enumInvalidLookup.ScreenCode)
    End Sub

#End Region

#End Region

#Region "STS Business Rules"

    Private Sub STSBusinessRulesTest(ByVal eSTSBusinessError As enumSTSBusinessError)

        Dim oRequest As New ProxyWS.UpdateRiskRequestType
        Dim oResponse As ProxyWS.UpdateRiskResponseType
        Dim nBusinessError As Integer = 224

        Try

            AddRisk()

            With oRequest
                .BranchCode = m_oTestData.BranchCode
                Select Case eSTSBusinessError
                    Case enumSTSBusinessError.InvInsuranceFileFolder
                        .InsuranceFileKey = m_nInsuranceFileCnt
                        .InsuranceFolderKey = m_oTestData.InvalidInsFileFolderCnt
                        .RiskKey = m_nRiskCnt
                        nBusinessError = 212
                    Case enumSTSBusinessError.InvInsuranceFileKey
                        .InsuranceFileKey = m_oTestData.InvalidCnt
                        .InsuranceFolderKey = m_nInsuranceFolderCnt
                        .RiskKey = m_nRiskCnt
                    Case enumSTSBusinessError.InvInsuranceFileRisk
                        .InsuranceFileKey = m_nInsuranceFileCnt
                        .InsuranceFolderKey = m_nInsuranceFolderCnt
                        .RiskKey = m_oTestData.InvalidInsFileRiskCnt
                        nBusinessError = 219
                    Case enumSTSBusinessError.InvInsuranceFolderKey
                        .InsuranceFileKey = m_nInsuranceFileCnt
                        .InsuranceFolderKey = m_oTestData.InvalidCnt
                        .RiskKey = m_nRiskCnt
                    Case enumSTSBusinessError.InvRiskKey
                        .InsuranceFileKey = m_nInsuranceFileCnt
                        .InsuranceFolderKey = m_nInsuranceFolderCnt
                        .RiskKey = m_oTestData.InvalidCnt
                        nBusinessError = 229
                End Select
                .XMLDataSet = m_oTestData.RiskDataXML
                .QuoteTimeStamp = kanEmptyTimeStamp
                .RiskDescription = m_oTestData.RiskDescription
                .ScreenCode = m_oTestData.ScreenCode
            End With

            SetWSETestCaseScenario(WSETestCaseScenario.None)
            oResponse = oProxy.UpdateRisk(oRequest)

            With oResponse
                ' Business Rule tests
                SAMTest.AssertCallFailedWithErrors(oResponse, 1)
                SAMTest.AssertErrorBusinessRule(oResponse, 0, nBusinessError)
            End With

        Catch ex As AssertionException
            Throw
        Catch ex As SoapException
            WSETest.HandleException(ex, WSETestCaseScenario.None)
        Catch ex As Exception
            WSETest.HandleException(ex)
        Finally
            oRequest = Nothing
            oResponse = Nothing
        End Try
    End Sub

    <Test(), Ignore("To be implemented")> _
    Public Sub STSBusinessRules_ValidationRulesReferred()

    End Sub
    <Test(), Ignore("To be implemented")> _
    Public Sub STSBusinessRules_ValidationRulesDeclined()

    End Sub
    <Test(), Ignore("To be implemented")> _
    Public Sub STSBusinessRules_UALRulesReferred()

    End Sub
    <Test(), Ignore("To be implemented")> _
    Public Sub STSBusinessRules_UALRulesDeclined()

    End Sub
    <Test(), Ignore("To be implemented")> _
    Public Sub STSBusinessRules_RatingRulesReferred()

    End Sub
    <Test(), Ignore("To be implemented")> _
    Public Sub STSBusinessRules_RatingRulesDeclined()

    End Sub

    <Test()> _
    Public Sub STSBusiness_InsFolderCnt()
        STSBusinessRulesTest(eSTSBusinessError:=enumSTSBusinessError.InvInsuranceFolderKey)
    End Sub
    <Test()> _
    Public Sub STSBusiness_InsFileCnt()
        STSBusinessRulesTest(eSTSBusinessError:=enumSTSBusinessError.InvInsuranceFileKey)
    End Sub
    <Test()> _
    Public Sub STSBusiness_RiskCnt()
        STSBusinessRulesTest(eSTSBusinessError:=enumSTSBusinessError.InvRiskKey)
    End Sub
    <Test()> _
    Public Sub STSBusiness_InsFileFolder()
        STSBusinessRulesTest(eSTSBusinessError:=enumSTSBusinessError.InvInsuranceFileFolder)
    End Sub
    <Test()> _
    Public Sub STSBusiness_InsFileRisk()
        STSBusinessRulesTest(eSTSBusinessError:=enumSTSBusinessError.InvInsuranceFileRisk)
    End Sub

#End Region

#Region "Sirius Back Office"

#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        SuccessTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        SuccessTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        SuccessTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        SuccessTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class
