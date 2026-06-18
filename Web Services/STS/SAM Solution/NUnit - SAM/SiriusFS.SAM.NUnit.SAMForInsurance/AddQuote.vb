Imports NUnit.Framework

<TestFixture()> _
Public Class AddQuote
    Inherits BaseTest

#Region " Private Declarations "

    Private m_nPartyCnt As Integer
    Private m_nInsuranceFileCnt As Integer
    Private m_nInsuranceFolderCnt As Integer
    Private m_sInsuranceFileRef As String
    Private m_bQuoteTimeStamp() As Byte
    Private m_bPartyTimeStamp() As Byte

    Private m_oTestData As New TestData

    Private Enum enumMissingData
        None
        AgentKey
        BranchCode
        CoverEndDate
        CoverStartDate
        Description
        PartyKey
        ProductCode
    End Enum

    Private Enum enumInvalidLookup
        None
        BranchCode
        SubBranchCode
        ProductCode
    End Enum

#End Region

#Region " Private Setup Preconditions "

    Private Sub AddParty()

        Dim oAddParty As New AddParty
        oAddParty.SupportMethod(m_nPartyCnt, m_bPartyTimeStamp)

    End Sub

    Private Sub AddParameters(ByRef oAddQuoteRequest As ProxyWS.AddQuoteRequestType)

        With oAddQuoteRequest
            .AgentKey = m_oTestData.AgentKey
            .AgentKeySpecified = (m_oTestData.AgentKey <> 0)
            .BranchCode = m_oTestData.BranchCode
            .SubBranchCode = m_oTestData.SubBranch
            .CoverEndDate = m_oTestData.CoverEndDate
            .CoverStartDate = m_oTestData.CoverStartDate
            .Description = m_oTestData.QuoteDescription
            .PartyKey = m_nPartyCnt
            .ProductCode = m_oTestData.ProductCode
            .AnalysisCode = m_oTestData.AnalysisCode
            .QuoteRef = m_oTestData.NewInsuranceFileRef & m_nPartyCnt.ToString
            .CurrencyCode = "GBP"
        End With

    End Sub

    Private Sub InvalidDataMissingTest(ByVal eMissingData As enumMissingData)

        Dim oRequest As New ProxyWS.AddQuoteRequestType
        Dim oResponse As ProxyWS.AddQuoteResponseType

        Try
            ' Set the specific condition on the input class
            AddParty()
            Assert.Greater(m_nPartyCnt, 0, "Class initialisation failed to create an anonymous customer to create the Quote against.")
            AddParameters(oRequest)

            ' Clear the missing value
            With oRequest
                Select Case eMissingData
                    Case enumMissingData.AgentKey
                        .AgentKey = Nothing
                    Case enumMissingData.BranchCode
                        .BranchCode = Nothing
                    Case enumMissingData.CoverEndDate
                        .CoverEndDate = Nothing
                    Case enumMissingData.CoverStartDate
                        .CoverStartDate = Nothing
                    Case enumMissingData.Description
                        .Description = Nothing
                    Case enumMissingData.PartyKey
                        .PartyKey = 0
                    Case enumMissingData.ProductCode
                        .ProductCode = Nothing
                End Select

            End With

            SetWSETestCaseScenario(WSETestCaseScenario.None)
            oResponse = oProxy.AddQuote(oRequest)

            With oResponse
                ' Now let check for the error we're expecting
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

    Private Sub InvalidLookupTest(ByVal eInvalidLookup As enumInvalidLookup)

        Dim oRequest As New ProxyWS.AddQuoteRequestType
        Dim oResponse As ProxyWS.AddQuoteResponseType
        Dim nError As Integer = 102
        Dim numberOfErrors As Integer = 1

        Try
            ' Set the specific condition on the input class
            AddParty()
            Assert.Greater(m_nPartyCnt, 0, "Class initialisation failed to create an anonymous customer to create the Quote against.")
            AddParameters(oRequest)

            With oRequest
                Select Case eInvalidLookup
                    Case enumInvalidLookup.BranchCode
                        .BranchCode = m_oTestData.InvalidLookupCode
                        nError = 210
                        numberOfErrors = 2
                    Case enumInvalidLookup.ProductCode
                        .ProductCode = m_oTestData.InvalidLookupCode
                    Case enumInvalidLookup.SubBranchCode
                        .SubBranchCode = m_oTestData.InvalidLookupCode
                End Select
            End With

            SetWSETestCaseScenario(WSETestCaseScenario.None)
            oResponse = oProxy.AddQuote(oRequest)
            With oResponse

                ' First lets check that the output class properties have not been set as we're expecting an error to have been thrown

                Assert.AreEqual(0, .InsuranceFileKey, "Unexpected returned value of InsuranceFileKey.")
                Assert.IsNull(.InsuranceFileRef, "Unexpected returned value of InsuranceFileRef.")
                Assert.AreEqual(0, .InsuranceFolderKey, "Unexpected returned value of InsuranceFolderKey.")
                Assert.AreEqual(kdtEmptyDateTime, .QuoteExpiryDate, "Unexpected returned value of QuoteExpiryDate.")
                Assert.IsNull(.QuoteTimeStamp, "Unexpected returned value of QuoteTimeStamp.")

                ' Now let check for the error we're expecting

                SAMTest.AssertCallFailedWithErrors(oResponse, numberOfErrors)
                SAMTest.AssertErrorInvalidData(oResponse, 0, nError, eInvalidLookup.ToString & " is invalid")
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

#End Region

#Region "Success"

    Public Sub SupportMethod(ByRef r_nInsuranceFileCnt As Integer, _
                            ByRef r_nInsuranceFolderCnt As Integer, _
                            ByRef r_sInsuranceFileRef As String, _
                            ByRef r_bQuoteTimeStamp() As Byte, _
                            Optional ByRef r_nPartyCnt As Integer = 0)
        Success()
        r_nInsuranceFileCnt = m_nInsuranceFileCnt
        r_nInsuranceFolderCnt = m_nInsuranceFolderCnt
        r_sInsuranceFileRef = m_sInsuranceFileRef
        r_bQuoteTimeStamp = m_bQuoteTimeStamp
        r_nPartyCnt = m_nPartyCnt

    End Sub

    Private Sub SuccessTest( _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.AddQuoteRequestType
        Dim oResponse As ProxyWS.AddQuoteResponseType

        Try
            ' Add a new party
            AddParty()
            Assert.Greater(m_nPartyCnt, 0, "Class initialisation failed to create an anonymous customer to create the Quote against.")

            ' Set the specific condition on the input class
            AddParameters(oRequest)

            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.AddQuote(oRequest)
            With oResponse
                SAMTest.AssertCallSucceeded(oResponse)
                ' Now validate that the returned data is valid
                Console.WriteLine("AddQuote.Success_AddQuote: Insurance File Count = " & CStr(.InsuranceFileKey))
                Assert.Greater(.InsuranceFileKey, 0, "Returned Insurance File Count invalid. See Console tab for value returned.")
                'Assert.AreEqual(17, .InsuranceFileRef.TrimEnd.Length, "Insurance File Reference invalid: length" & .InsuranceFileRef.TrimEnd)
                Console.WriteLine("AddQuote.Success_AddQuote: Insurance Folder Count = " & CStr(.InsuranceFolderKey))
                Assert.Greater(.InsuranceFolderKey, 0, "Returned Insurance Folder Count invalid. See Console tab for value returned.")
                ' TODO: Anyone - Why is this assertion testing that a date variable is a valid date? I have commented it out for now.
                '    Assert.IsTrue(IsDate(.QuoteExpiryDate), "Returned QuoteExpiryDate cannot be converted to a date data type.")
                m_nInsuranceFileCnt = .InsuranceFileKey
                m_nInsuranceFolderCnt = .InsuranceFolderKey
                m_sInsuranceFileRef = .InsuranceFileRef
                m_bQuoteTimeStamp = .QuoteTimeStamp

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

    <Test()> _
    Public Sub Success()
        SuccessTest()
    End Sub

#End Region

#Region "Invalid Data"

#Region "Mandatory Data Missing"

    <Test()> _
    Public Sub InvalidData_Missing_AgentKey()
        InvalidDataMissingTest(enumMissingData.AgentKey)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_BranchCode()
        InvalidDataMissingTest(enumMissingData.BranchCode)
    End Sub

    <Test()> _
    Public Sub InvalidData_Missing_CoverEndDate()
        InvalidDataMissingTest(enumMissingData.CoverEndDate)
    End Sub

    <Test()> _
    Public Sub InvalidData_Missing_CoverStartDate()
        InvalidDataMissingTest(enumMissingData.CoverStartDate)
    End Sub

    <Test()> _
    Public Sub InvalidData_Missing_PartyKey()
        InvalidDataMissingTest(enumMissingData.PartyKey)
    End Sub

    <Test()> _
    Public Sub InvalidData_Missing_ProductCode()
        InvalidDataMissingTest(enumMissingData.ProductCode)
    End Sub

#End Region

#Region "Invalid Format"

#End Region

#Region "Invalid List Value"

    <Test()> _
    Public Sub InvalidData_LookupListValue_BranchCode()
        InvalidLookupTest(enumInvalidLookup.BranchCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_LookupListValue_ProductCode()
        InvalidLookupTest(enumInvalidLookup.ProductCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_LookupListValue_SubBranchCode()
        InvalidLookupTest(enumInvalidLookup.SubBranchCode)
    End Sub

#End Region

#End Region

#Region "STS Business Rules"

    
    Public Sub STSBusinessRules_CoverStartDateRequestTypePast()

        Dim oRequest As New ProxyWS.AddQuoteRequestType
        Dim oResponse As ProxyWS.AddQuoteResponseType

        Try
            ' Set the specific condition on the input class
            AddParty()
            Assert.Greater(m_nPartyCnt, 0, "Class initialisation failed to create an anonymous customer to create the Quote against.")

            AddParameters(oRequest)
            oRequest.CoverStartDate = DateAdd(DateInterval.Year, -1, Date.Today)

            SetWSETestCaseScenario(WSETestCaseScenario.None)
            oResponse = oProxy.AddQuote(oRequest)

            With oResponse
                ' First lets check that the output class properties have not been set as we're expecting an error to have been thrown
                Assert.AreEqual(0, .InsuranceFileKey, "Unexpected returned value of InsuranceFileKey.")
                Assert.AreEqual("", .InsuranceFileRef, "Unexpected returned value of InsuranceFileRef.")
                Assert.AreEqual(0, .InsuranceFolderKey, "Unexpected returned value of InsuranceFolderKey.")
                Assert.AreEqual(kdtEmptyDateTime, .QuoteExpiryDate, "Unexpected returned value of QuoteExpiryDate.")
                Assert.IsNull(.QuoteTimeStamp, "Unexpected returned value of QuoteTimeStamp.")

                ' Now let check for the error we're expecting
                SAMTest.AssertCallFailedWithErrors(oResponse, 1)
                SAMTest.AssertErrorBusinessRule(oResponse, 0, 221, "Cover Start date is in the past")
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
    Public Sub STSBusinessRules_CoverEndDateBeforeStartDate()

        Dim oRequest As New ProxyWS.AddQuoteRequestType
        Dim oResponse As ProxyWS.AddQuoteResponseType

        Try
            ' Set the specific condition on the input class
            AddParty()
            Assert.Greater(m_nPartyCnt, 0, "Class initialisation failed to create an anonymous customer to create the Quote against.")

            AddParameters(oRequest)
            oRequest.CoverEndDate = DateAdd(DateInterval.Year, -1, m_oTestData.CoverStartDate)

            SetWSETestCaseScenario(WSETestCaseScenario.None)
            oResponse = oProxy.AddQuote(oRequest)

            With oResponse
                ' First lets check that the output class properties have not been set as we're expecting an error to have been thrown
                Assert.AreEqual(0, .InsuranceFileKey, "Unexpected returned value of InsuranceFileKey.")
                Assert.AreEqual("", .InsuranceFileRef, "Unexpected returned value of InsuranceFileRef.")
                Assert.AreEqual(0, .InsuranceFolderKey, "Unexpected returned value of InsuranceFolderKey.")
                Assert.AreEqual(kdtEmptyDateTime, .QuoteExpiryDate, "Unexpected returned value of QuoteExpiryDate.")
                Assert.IsNull(.QuoteTimeStamp, "Unexpected returned value of QuoteTimeStamp.")

                ' Now let check for the error we're expecting
                SAMTest.AssertCallFailedWithErrors(oResponse, 1)
                SAMTest.AssertErrorBusinessRule(oResponse, 0, 222, "Cover End date is before Cover Start date")
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
    Public Sub STSBusinessRules_InvalidAgentKey()

        Dim oRequest As New ProxyWS.AddQuoteRequestType
        Dim oResponse As ProxyWS.AddQuoteResponseType

        Try
            ' Set the specific condition on the input class
            AddParty()
            Assert.Greater(m_nPartyCnt, 0, "Class initialisation failed to create an anonymous customer to create the Quote against.")

            AddParameters(oRequest)
            oRequest.AgentKey = m_oTestData.InvalidCnt

            SetWSETestCaseScenario(WSETestCaseScenario.None)
            oResponse = oProxy.AddQuote(oRequest)

            With oResponse
                ' Now let check for the error we're expecting
                SAMTest.AssertCallFailedWithErrors(oResponse, 1)
                SAMTest.AssertErrorBusinessRule(oResponse, 0, 274)
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
