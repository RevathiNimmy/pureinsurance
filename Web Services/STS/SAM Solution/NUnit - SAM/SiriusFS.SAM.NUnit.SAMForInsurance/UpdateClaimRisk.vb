Imports NUnit.Framework

<TestFixture()> _
Public Class UpdateClaimRisk
    Inherits BaseTest

#Region " Private Declarations "

    Private m_iClaimKey As Integer
    Private m_btQuoteTimeStamp() As Byte
    Private m_sRiskDataXML As String
    Private m_oTestData As New TestData

    Private Enum enumMissingData
        BranchCode
        BaseClaimKey
        XMLDataSet
        TimeStamp
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
        InvBaseClaimKey
        InvTimeStamp
    End Enum

#End Region

#Region "Setup preconditions"


#End Region

#Region "Success"

    Public Sub SupportMethod(ByRef r_nInsuranceFileCnt As Integer)

        Success()

    End Sub

    Private Sub SuccessTest( _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.UpdateClaimRiskRequestType
        Dim oResponse As ProxyWS.UpdateClaimRiskResponseType

        Try

            Dim xmlDoc As New System.Xml.XmlDocument
            Dim oElementToAddOrUpdate As XmlNode
            Dim oElementToAddOrUpdateAdded As XmlNode
            Dim newAttribute As System.Xml.XmlAttribute
            Dim nNextOINumber As Integer
            Dim bNewElement As Boolean

            Dim oAddClaimRisk As New AddClaimRisk

            oAddClaimRisk.SupportMethod(m_oTestData.BranchCode, m_iClaimKey, m_sRiskDataXML, m_btQuoteTimeStamp)

            ' Read in the XML created in the AddRisk
            xmlDoc.LoadXml(m_sRiskDataXML)

            ' Get the NextOINumber and increment it
            nNextOINumber = xmlDoc.SelectSingleNode("DATA_SET") _
                .Attributes("NextOINumber").Value

            nNextOINumber = nNextOINumber + 1

            ' Create the new test elements
            For iElementCnt As Integer = m_oTestData.XMLDataSetClaimsElementsToAdd.GetLowerBound(0) To m_oTestData.XMLDataSetClaimsElementsToAdd.GetUpperBound(0)

                ' Check if the element already exists
                oElementToAddOrUpdate = xmlDoc.SelectSingleNode("DATA_SET") _
                                    .SelectSingleNode("RISK_OBJECTS") _
                                    .SelectSingleNode(m_oTestData.ClaimDataModelCode & "_POLICY_BINDER") _
                                    .SelectSingleNode(m_oTestData.XMLDataSetClaimsElementsToAdd(iElementCnt).ElementName)
                ' If not, create it
                If oElementToAddOrUpdate Is Nothing Then
                    oElementToAddOrUpdate = xmlDoc.CreateNode(XmlNodeType.Element, m_oTestData.XMLDataSetClaimsElementsToAdd(iElementCnt).ElementName, "")

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
                    If oElementToAddOrUpdate.Attributes("OI") IsNot Nothing And oElementToAddOrUpdate.Attributes("US").Value <> "1" Then
                        oElementToAddOrUpdate.Attributes("US").Value = "2"
                        bNewElement = False
                    End If

                End If

                ' Add specific attributes for this test
                For iAttrCnt As Integer = m_oTestData.XMLDataSetClaimsElementsToAdd(iElementCnt).Attributes.GetLowerBound(0) To m_oTestData.XMLDataSetClaimsElementsToAdd(iElementCnt).Attributes.GetUpperBound(0)
                    newAttribute = xmlDoc.CreateAttribute(m_oTestData.XMLDataSetClaimsElementsToAdd(iElementCnt).Attributes(iAttrCnt).AttributeName)
                    newAttribute.Value = m_oTestData.XMLDataSetClaimsElementsToAdd(iElementCnt).Attributes(iAttrCnt).AttributeValue
                    oElementToAddOrUpdate.Attributes.Append(newAttribute)
                Next

                If bNewElement Then
                    ' Append the new element node to the XML under the POLICY BINDER
                    oElementToAddOrUpdateAdded = xmlDoc.SelectSingleNode("DATA_SET") _
                        .SelectSingleNode("RISK_OBJECTS") _
                        .SelectSingleNode(m_oTestData.ClaimDataModelCode & "_POLICY_BINDER") _
                        .AppendChild(oElementToAddOrUpdate)
                End If

            Next

            ' Write back the next OI number to the DATA_SET node
            nNextOINumber -= 1
            xmlDoc.SelectSingleNode("DATA_SET") _
                .Attributes("NextOINumber").Value = nNextOINumber

            With oRequest
                .BranchCode = m_oTestData.BranchCode
                .BaseClaimKey = m_iClaimKey
                .XMLDataSet = xmlDoc.OuterXml
                .TimeStamp = m_btQuoteTimeStamp
            End With

            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.UpdateClaimRisk(oRequest)

            SAMTest.AssertCallSucceeded(oResponse)

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

#End Region

#Region "Invalid Data"

#Region "Mandatory Data Missing"

    Private Sub MissingDataTest(ByVal eMissingData As enumMissingData)

        Dim oRequest As New ProxyWS.UpdateClaimRiskRequestType
        Dim oResponse As ProxyWS.UpdateClaimRiskResponseType
        Dim bQuoteTimeStamp() As Byte = {0, 0, 0, 0, 0, 0, 0, 0}
        Try

            With oRequest
                If eMissingData <> enumMissingData.BranchCode Then
                    .BranchCode = "DATA"
                End If
                If eMissingData <> enumMissingData.BaseClaimKey Then
                    .BaseClaimKey = m_iClaimKey
                End If
                If eMissingData <> enumMissingData.XMLDataSet Then
                    .XMLDataSet = m_oTestData.RiskDataXML
                End If
                If eMissingData <> enumMissingData.TimeStamp Then
                    .TimeStamp = bQuoteTimeStamp
                End If
            End With

            SetWSETestCaseScenario(WSETestCaseScenario.None)
            oResponse = oProxy.UpdateClaimRisk(oRequest)

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
    Public Sub InvalidData_Missing_XMLDataSet()
        MissingDataTest(enumMissingData.XMLDataSet)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_TimeStamp()
        MissingDataTest(enumMissingData.TimeStamp)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_BaseClaimKey()
        MissingDataTest(enumMissingData.BaseClaimKey)
    End Sub

#End Region

#Region "Invalid Format"

#End Region

#Region "Invalid List Value"

    Private Sub InvalidLookupTest(ByVal eInvalidLookup As enumInvalidLookup)

        Dim oRequest As New ProxyWS.UpdateClaimRiskRequestType
        Dim oResponse As ProxyWS.UpdateClaimRiskResponseType
        Dim nLookupError As Integer = 102

        Try

            With oRequest
                If eInvalidLookup = enumInvalidLookup.BranchCode Then
                    .BranchCode = m_oTestData.InvalidLookupCode
                    nLookupError = 210
                Else
                    .BranchCode = m_oTestData.BranchCode
                End If

                .XMLDataSet = m_sRiskDataXML
                .TimeStamp = m_btQuoteTimeStamp
            End With

            SetWSETestCaseScenario(WSETestCaseScenario.None)
            oResponse = oProxy.UpdateClaimRisk(oRequest)

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

#Region " STS Business Rules "

    Private Sub STSBusinessRulesTest(ByVal eSTSBusinessError As enumSTSBusinessError)

        Dim oRequest As New ProxyWS.UpdateClaimRiskRequestType
        Dim oResponse As ProxyWS.UpdateClaimRiskResponseType
        Dim bQuoteTimeStamp() As Byte = {0, 0, 0, 0, 0, 0, 0, 0}
        Dim nBusinessError As Integer = 224
        Try

            With oRequest
                .BranchCode = m_oTestData.BranchCode
                Select Case eSTSBusinessError
                End Select
                .XMLDataSet = m_oTestData.RiskDataXML
                .TimeStamp = bQuoteTimeStamp
            End With

            SetWSETestCaseScenario(WSETestCaseScenario.None)
            oResponse = oProxy.UpdateClaimRisk(oRequest)

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
    Public Sub STSBusiness_BaseClaimKey()
        STSBusinessRulesTest(eSTSBusinessError:=enumSTSBusinessError.InvBaseClaimKey)
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
