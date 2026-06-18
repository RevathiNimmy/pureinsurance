Imports NUnit.Framework

<TestFixture()> _
Public Class ClientDataImport
    Inherits BaseTest
    Private Const kInvalidLookupValue As String = "000GARBAGE"
    Private Const kInvalidLookupFieldMessage As String = "Invalid Lookup Field Name Returned"
    Private Const kIncorrectMissingMandatoryFieldMessage As String = "Incorrect Missing Mandatory Field Returned"
    Private Const kInvalidCodeReturned As String = "Invalid Error Code Returned"
    Private Const kInvalidDescriptionReturned As String = "Invalid Error Description Returned"
    Private Const kInvalidLookupValueMessage As String = "Invalid Lookup Value Returned"

#Region "Private Declarations"

    Private m_lBaseClaimId As Integer

    Private Enum TestCaseScenario

        ' Non Error Scenarios
        None

        SuccessWithPartyPC
        SuccessWithPartyCC

        AllNonMandatoryFieldsMissing

        ' Busines Error scenarios : - some of these cannot be tested
        BusinessErrorEndDateBeforeStartDate
        BusinessErrorTooFewTransactions
        BusinessErrorTransactionAmountsDontBalance

        ' Busines Warning scenarios

        ' Missing Data
        MissingDataBranchCode
        MissingDataClient
        MissingDataPolicy 
        MissingDataCompanyName
        MissingDataForename
        MissingDataSurname 
        MissingDataTitle 
        MissingDataInitials 
        MissingDataAddress 
        MissingDataAddressLine1 
        MissingDataPostCode 
        MissingDataCountryCode 
        MissingDataContact 
        MissingDataContactType 
        MissingDataContactItem 
        MissingDataContactDetail 
        MissingDataCoverStartDate
        MissingDataCoverEndDate 
        MissingDataDescription 
        MissingDataProductCode 
        MissingDataRisks 
        MissingDataRiskTypeCode 
        MissingDataScreenCode 
        MissingDataRiskDescription 
        MissingDataDataModelCode 
        MissingDataAddressTypeCode
        MissingDataPolicyVersion
        MissingDataPolicyStatusCode
        MissingDataDocumentTypeCode
        MissingDataAccountsTransactions
        MissingDataAccountsAccountCode

        ' Invalid Lookups
        InvalidLookupBranchCode
        InvalidLookupDataModelCode
        InvalidLookupScreenCode
        InvalidLookupRiskTypeCode
        InvalidLookupProductCode
        InvalidLookupGenderCode
        InvalidLookupOccupationCode
        InvalidLookupEmployersBusinessCode
        InvalidLookupBusinessCode
        InvalidLookupPolicyStatusCode
        InvalidLookupAccountsAccountCode
        InvalidDataAccountsAmount

    End Enum

#End Region

#Region "Call ProxyWS Method"

    Private Sub ClientDataImportTest( _
        Optional ByVal TestCase As TestCaseScenario = TestCaseScenario.None, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        'TODO: REPLACE REQUEST AND RESPONSE NAMES WITH VALID ONES WHEN USING FOR REAL
        Dim oRequest As New ProxyWS.ClientDataImportRequestType
        Dim oResponse As ProxyWS.ClientDataImportResponseType

        Dim m_oTestData As New TestData

        ' get test data object for this call
        Dim oTD As ClientDataImportTestDataXMLStruture
        oTD = m_oTestData.ClientDataImport

        Try

            With oRequest
                .AgentKey = oTD.AgentKey
                .AgentKeySpecified = True
                .BranchCode = oTD.BranchCode
                .AccountDocuments = oTD.ListOfAccountDocuments.ToArray
                .PolicyVersion = oTD.ListOfPolicyVersion.ToArray

                If TestCase = TestCaseScenario.SuccessWithPartyCC Then
                    .Item = oTD.oPartyCC
                ElseIf TestCase = TestCaseScenario.SuccessWithPartyPC Then
                    .Item = oTD.oPartyPC
                ElseIf TestCase = TestCaseScenario.MissingDataCompanyName OrElse TestCase = TestCaseScenario.InvalidLookupBusinessCode Then
                    .Item = oTD.oPartyCC
                Else ' Default to PC for all other test conditions
                    .Item = oTD.oPartyPC
                End If

            End With

            ' reset fields based on selected test case
            ProcessTestCases(oRequest, TestCase)
            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.ClientDataImport(oRequest)

            With oResponse

                ' all these test cases should work without error
                If TestCase = TestCaseScenario.None OrElse _
                    TestCase = TestCaseScenario.SuccessWithPartyCC OrElse _
                    TestCase = TestCaseScenario.SuccessWithPartyPC OrElse _
                    TestCase = TestCaseScenario.AllNonMandatoryFieldsMissing Then

                    SAMTest.AssertCallSucceeded(oResponse)

                ElseIf TestCase = TestCaseScenario.BusinessErrorEndDateBeforeStartDate OrElse _
                    TestCase = TestCaseScenario.BusinessErrorTooFewTransactions OrElse _
                    TestCase = TestCaseScenario.BusinessErrorTransactionAmountsDontBalance Then

                    ProcessBusinessErrors(oResponse, TestCase)

                ElseIf TestCase = TestCaseScenario.InvalidLookupBranchCode OrElse _
                    TestCase = TestCaseScenario.InvalidLookupDataModelCode OrElse _
                    TestCase = TestCaseScenario.InvalidLookupScreenCode OrElse _
                    TestCase = TestCaseScenario.InvalidLookupRiskTypeCode OrElse _
                    TestCase = TestCaseScenario.InvalidLookupProductCode OrElse _
                    TestCase = TestCaseScenario.BusinessErrorEndDateBeforeStartDate OrElse _
                    TestCase = TestCaseScenario.InvalidLookupGenderCode OrElse _
                    TestCase = TestCaseScenario.InvalidLookupOccupationCode OrElse _
                    TestCase = TestCaseScenario.InvalidLookupEmployersBusinessCode OrElse _
                    TestCase = TestCaseScenario.InvalidLookupBusinessCode OrElse _
                    TestCase = TestCaseScenario.InvalidLookupPolicyStatusCode OrElse _
                    TestCase = TestCaseScenario.InvalidLookupAccountsAccountCode OrElse _
                    TestCase = TestCaseScenario.InvalidDataAccountsAmount Then

                    ProcessInvalidLookup(oResponse, TestCase)

                ElseIf TestCase = TestCaseScenario.MissingDataBranchCode OrElse _
                    TestCase = TestCaseScenario.MissingDataClient OrElse _
                    TestCase = TestCaseScenario.MissingDataPolicy OrElse _
                    TestCase = TestCaseScenario.MissingDataCompanyName OrElse _
                    TestCase = TestCaseScenario.MissingDataForename OrElse _
                    TestCase = TestCaseScenario.MissingDataSurname OrElse _
                    TestCase = TestCaseScenario.MissingDataTitle OrElse _
                    TestCase = TestCaseScenario.MissingDataInitials OrElse _
                    TestCase = TestCaseScenario.MissingDataAddress OrElse _
                    TestCase = TestCaseScenario.MissingDataAddressLine1 OrElse _
                    TestCase = TestCaseScenario.MissingDataPostCode OrElse _
                    TestCase = TestCaseScenario.MissingDataCountryCode OrElse _
                    TestCase = TestCaseScenario.MissingDataContact OrElse _
                    TestCase = TestCaseScenario.MissingDataContactType OrElse _
                    TestCase = TestCaseScenario.MissingDataContactItem OrElse _
                    TestCase = TestCaseScenario.MissingDataContactDetail OrElse _
                    TestCase = TestCaseScenario.MissingDataCoverStartDate OrElse _
                    TestCase = TestCaseScenario.MissingDataCoverEndDate OrElse _
                    TestCase = TestCaseScenario.MissingDataDescription OrElse _
                    TestCase = TestCaseScenario.MissingDataProductCode OrElse _
                    TestCase = TestCaseScenario.MissingDataRisks OrElse _
                    TestCase = TestCaseScenario.MissingDataRiskTypeCode OrElse _
                    TestCase = TestCaseScenario.MissingDataScreenCode OrElse _
                    TestCase = TestCaseScenario.MissingDataRiskDescription OrElse _
                    TestCase = TestCaseScenario.MissingDataDataModelCode OrElse _
                    TestCase = TestCaseScenario.MissingDataAddressTypeCode OrElse _
                    TestCase = TestCaseScenario.MissingDataPolicyVersion OrElse _
                    TestCase = TestCaseScenario.MissingDataPolicyStatusCode OrElse _
                    TestCase = TestCaseScenario.MissingDataDocumentTypeCode OrElse _
                    TestCase = TestCaseScenario.MissingDataAccountsTransactions OrElse _
                    TestCase = TestCaseScenario.MissingDataAccountsAccountCode Then

                    ProcessMissingData(oResponse, TestCase)

                Else

                    Assert.Fail("No Testcase condition was tested")

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
            'oResponse = Nothing
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


    Private Sub AllNonMandatoryFieldsMissing(ByVal oRequest As ProxyWS.ClientDataImportRequestType)


    End Sub

#End Region

    ''' <summary>
    ''' Setup Test Case Scenarios which return invalid lookup errors
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>

#Region "Setup Invalid Lookups Test Cases"

    Private Sub InvalidLookupBranchCode(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        oRequest.BranchCode = kInvalidLookupValue
    End Sub


    Private Sub InvalidLookupRiskTypeCode(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oPolicy() As SAMForInsurance.ProxyWS.BaseQuoteRiskMsgType = oRequest.PolicyVersion
        If oPolicy IsNot Nothing Then
            If oPolicy(0) IsNot Nothing Then
                If oPolicy(0).Risks IsNot Nothing Then
                    oPolicy(0).Risks(0).RiskTypeCode = kInvalidLookupValue
                End If
            End If
        End If
    End Sub

    Private Sub InvalidLookupScreenCode(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oPolicy() As SAMForInsurance.ProxyWS.BaseQuoteRiskMsgType = oRequest.PolicyVersion
        If oPolicy IsNot Nothing Then
            If oPolicy(0) IsNot Nothing Then
                If oPolicy(0).Risks IsNot Nothing Then
                    oPolicy(0).Risks(0).ScreenCode = kInvalidLookupValue
                End If
            End If
        End If
    End Sub

    Private Sub InvalidLookupDataModelCode(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oPolicy() As SAMForInsurance.ProxyWS.BaseQuoteRiskMsgType = oRequest.PolicyVersion
        If oPolicy IsNot Nothing Then
            If oPolicy(0) IsNot Nothing Then
                If oPolicy(0).Risks IsNot Nothing Then
                    oPolicy(0).Risks(0).DataModelCode = kInvalidLookupValue
                End If
            End If
        End If
    End Sub

    Private Sub InvalidLookupProductCode(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oPolicy() As SAMForInsurance.ProxyWS.BaseQuoteRiskMsgType = oRequest.PolicyVersion
        If oPolicy IsNot Nothing Then
            If oPolicy(0) IsNot Nothing Then
                oPolicy(0).ProductCode = kInvalidLookupValue
            End If
        End If
    End Sub

    Private Sub InvalidLookupGenderCode(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        If oRequest.Item.GetType Is GetType(SAMForInsurance.ProxyWS.BasePartyPCType) Then
            Dim oPartyPC As SAMForInsurance.ProxyWS.BasePartyPCType = oRequest.Item
            If oPartyPC IsNot Nothing Then
                oPartyPC.GenderCode = kInvalidLookupValue
            End If
        End If
    End Sub
        
    Private Sub InvalidLookupOccupationCode(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        If oRequest.Item.GetType Is GetType(SAMForInsurance.ProxyWS.BasePartyPCType) Then
            Dim oPartyPC As SAMForInsurance.ProxyWS.BasePartyPCType = oRequest.Item
            If oPartyPC IsNot Nothing Then
                oPartyPC.OccupationCode = kInvalidLookupValue
            End If
        End If
    End Sub
        
    Private Sub InvalidLookupEmployersBusinessCode(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        If oRequest.Item.GetType Is GetType(SAMForInsurance.ProxyWS.BasePartyPCType) Then
            Dim oPartyPC As SAMForInsurance.ProxyWS.BasePartyPCType = oRequest.Item
            If oPartyPC IsNot Nothing Then
                oPartyPC.EmployersBusinessCode = kInvalidLookupValue
            End If
        End If
    End Sub
    Private Sub InvalidLookupBusinessCode(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        If oRequest.Item.GetType Is GetType(SAMForInsurance.ProxyWS.BasePartyCCType) Then
            Dim oPartyCC As SAMForInsurance.ProxyWS.BasePartyCCType = oRequest.Item
            If oPartyCC IsNot Nothing Then
                oPartyCC.BusinessCode = kInvalidLookupValue
            End If
        End If
    End Sub
    Private Sub InvalidLookupPolicyStatusCode(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oPolicy() As SAMForInsurance.ProxyWS.BaseQuoteRiskMsgType = oRequest.PolicyVersion
        If oPolicy IsNot Nothing Then
            If oPolicy(0) IsNot Nothing Then
                oPolicy(0).PolicyStatusCode = kInvalidLookupValue
            End If
        End If
    End Sub
    Private Sub InvalidLookupAccountsAccountCode(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oAccounts() As SAMForInsurance.ProxyWS.BasePostDocumentRequestType = oRequest.AccountDocuments
        If oAccounts IsNot Nothing Then
            If oAccounts(0) IsNot Nothing Then
                Dim oTransactions() As SAMForInsurance.ProxyWS.BaseTransactionType = oAccounts(0).Transactions
                If oTransactions IsNot Nothing Then
                    If oTransactions(0) IsNot Nothing Then
                        oTransactions(0).AccountCode = kInvalidLookupValue
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub InvalidDataAccountsAmount(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oAccounts() As SAMForInsurance.ProxyWS.BasePostDocumentRequestType = oRequest.AccountDocuments
        If oAccounts IsNot Nothing Then
            If oAccounts(0) IsNot Nothing Then
                Dim oTransactions() As SAMForInsurance.ProxyWS.BaseTransactionType = oAccounts(0).Transactions
                If oTransactions IsNot Nothing Then
                    If oTransactions(0) IsNot Nothing Then
                        oTransactions(0).Amount = 0
                    End If
                End If
            End If
        End If
    End Sub

#End Region

    ''' <summary>
    ''' Setup Test Case Scenarios which return missing data errors
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>

#Region "Setup Missing Data Test Cases"

    Private Sub MissingDataBranchCode(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        oRequest.BranchCode = String.Empty
    End Sub

    Private Sub MissingDataClient(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        oRequest.Item = Nothing
    End Sub

    Private Sub MissingDataPolicy (ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        oRequest.PolicyVersion = Nothing
    End Sub

    Private Sub MissingDataCompanyName(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        If oRequest.Item.GetType Is GetType(SAMForInsurance.ProxyWS.BasePartyCCType) Then
            Dim oPartyCC As SAMForInsurance.ProxyWS.BasePartyCCType = oRequest.Item
            If oPartyCC IsNot Nothing Then
                oPartyCC.CompanyName = String.Empty
            End If
        End If
    End Sub

    Private Sub MissingDataForename(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        If oRequest.Item.GetType Is GetType(SAMForInsurance.ProxyWS.BasePartyPCType) Then
            Dim oPartyPC As SAMForInsurance.ProxyWS.BasePartyPCType = oRequest.Item
            If oPartyPC IsNot Nothing Then
                oPartyPC.Forename = String.Empty
            End If
        End If
    End Sub

    Private Sub MissingDataSurname (ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        If oRequest.Item.GetType Is GetType(SAMForInsurance.ProxyWS.BasePartyPCType) Then
            Dim oPartyPC As SAMForInsurance.ProxyWS.BasePartyPCType = oRequest.Item
            If oPartyPC IsNot Nothing Then
                oPartyPC.Surname = String.Empty
            End If
        End If
    End Sub

    Private Sub MissingDataTitle (ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        If oRequest.Item.GetType Is GetType(SAMForInsurance.ProxyWS.BasePartyPCType) Then
            Dim oPartyPC As SAMForInsurance.ProxyWS.BasePartyPCType = oRequest.Item
            If oPartyPC IsNot Nothing Then
                oPartyPC.Title = String.Empty
            End If
        End If
    End Sub

    Private Sub MissingDataInitials (ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        If oRequest.Item.GetType Is GetType(SAMForInsurance.ProxyWS.BasePartyPCType) Then
            Dim oPartyPC As SAMForInsurance.ProxyWS.BasePartyPCType = oRequest.Item
            If oPartyPC IsNot Nothing Then
                oPartyPC.Initials = String.Empty
            End If
        End If
    End Sub

    Private Sub MissingDataAddress (ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oParty As SAMForInsurance.ProxyWS.BasePartyType = oRequest.Item
        If oParty IsNot Nothing Then
            oParty.Addresses = Nothing
        End If
    End Sub

    Private Sub MissingDataAddressLine1 (ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oParty As SAMForInsurance.ProxyWS.BasePartyType = oRequest.Item
        If oParty IsNot Nothing Then
            If oParty.Addresses IsNot Nothing Then
                oParty.Addresses(0).AddressLine1 = String.Empty
            End If
        End If
    End Sub

    Private Sub MissingDataPostCode (ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oParty As SAMForInsurance.ProxyWS.BasePartyType = oRequest.Item
        If oParty IsNot Nothing Then
            If oParty.Addresses IsNot Nothing Then
                oParty.Addresses(0).PostCode = String.Empty
            End If
        End If
    End Sub

    Private Sub MissingDataCountryCode (ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oParty As SAMForInsurance.ProxyWS.BasePartyType = oRequest.Item
        If oParty IsNot Nothing Then
            If oParty.Addresses IsNot Nothing Then
                oParty.Addresses(0).CountryCode = String.Empty
            End If
        End If
    End Sub

    Private Sub MissingDataContact (ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oParty As SAMForInsurance.ProxyWS.BasePartyType = oRequest.Item
        If oParty IsNot Nothing Then
            oParty.Contacts = Nothing
        End If
    End Sub

    Private Sub MissingDataContactType (ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oParty As SAMForInsurance.ProxyWS.BasePartyType = oRequest.Item
        If oParty IsNot Nothing Then
            If oParty.Contacts IsNot Nothing Then
                oParty.Contacts(0).ContactTypeCode = Nothing
            End If
        End If
    End Sub

    Private Sub MissingDataContactItem (ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oParty As SAMForInsurance.ProxyWS.BasePartyType = oRequest.Item
        If oParty IsNot Nothing Then
            If oParty.Contacts IsNot Nothing Then
                If oParty.Contacts(0).ContactDetail IsNot Nothing Then
                    oParty.Contacts(0).ContactDetail.Item = Nothing
                End If
            End If
        End If
    End Sub

    Private Sub MissingDataContactDetail (ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oParty As SAMForInsurance.ProxyWS.BasePartyType = oRequest.Item
        If oParty IsNot Nothing Then
            If oParty.Contacts IsNot Nothing Then
                oParty.Contacts(0).ContactDetail.Item = ""
            End If
        End If
    End Sub

    Private Sub MissingDataCoverStartDate(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oPolicy() As SAMForInsurance.ProxyWS.BaseQuoteRiskMsgType = oRequest.PolicyVersion
        If oPolicy IsNot Nothing Then
            If oPolicy(0) IsNot Nothing Then
                oPolicy(0).CoverStartDate = Nothing
            End If
        End If
    End Sub

    Private Sub MissingDataCoverEndDate(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oPolicy() As SAMForInsurance.ProxyWS.BaseQuoteRiskMsgType = oRequest.PolicyVersion
        If oPolicy IsNot Nothing Then
            If oPolicy(0) IsNot Nothing Then
                oPolicy(0).CoverEndDate = Nothing
            End If
        End If
    End Sub

    Private Sub MissingDataDescription(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oPolicy() As SAMForInsurance.ProxyWS.BaseQuoteRiskMsgType = oRequest.PolicyVersion
        If oPolicy IsNot Nothing Then
            If oPolicy(0) IsNot Nothing Then
                oPolicy(0).Description = String.Empty
            End If
        End If
    End Sub

    Private Sub MissingDataProductCode(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oPolicy() As SAMForInsurance.ProxyWS.BaseQuoteRiskMsgType = oRequest.PolicyVersion
        If oPolicy IsNot Nothing Then
            If oPolicy(0) IsNot Nothing Then
                oPolicy(0).ProductCode = String.Empty
            End If
        End If
    End Sub

    Private Sub MissingDataRisks(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oPolicy() As SAMForInsurance.ProxyWS.BaseQuoteRiskMsgType = oRequest.PolicyVersion
        If oPolicy IsNot Nothing Then
            If oPolicy(0) IsNot Nothing Then
                oPolicy(0).Risks = Nothing
            End If
        End If
    End Sub

    Private Sub MissingDataRiskTypeCode(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oPolicy() As SAMForInsurance.ProxyWS.BaseQuoteRiskMsgType = oRequest.PolicyVersion
        If oPolicy IsNot Nothing Then
            If oPolicy(0) IsNot Nothing Then
                If oPolicy(0).Risks IsNot Nothing Then
                    oPolicy(0).Risks(0).RiskTypeCode = String.Empty
                End If
            End If
        End If
    End Sub
    Private Sub MissingDataScreenCode(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oPolicy() As SAMForInsurance.ProxyWS.BaseQuoteRiskMsgType = oRequest.PolicyVersion
        If oPolicy IsNot Nothing Then
            If oPolicy(0) IsNot Nothing Then
                If oPolicy(0).Risks IsNot Nothing Then
                    oPolicy(0).Risks(0).ScreenCode = String.Empty
                End If
            End If
        End If
    End Sub
    Private Sub MissingDataRiskDescription(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oPolicy() As SAMForInsurance.ProxyWS.BaseQuoteRiskMsgType = oRequest.PolicyVersion
        If oPolicy IsNot Nothing Then
            If oPolicy(0) IsNot Nothing Then
                If oPolicy(0).Risks IsNot Nothing Then
                    oPolicy(0).Risks(0).RiskDescription = String.Empty
                End If
            End If
        End If
    End Sub
    Private Sub MissingDataDataModelCode(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oPolicy() As SAMForInsurance.ProxyWS.BaseQuoteRiskMsgType = oRequest.PolicyVersion
        If oPolicy IsNot Nothing Then
            If oPolicy(0) IsNot Nothing Then
                If oPolicy(0).Risks IsNot Nothing Then
                    oPolicy(0).Risks(0).DataModelCode = String.Empty
                End If
            End If
        End If
    End Sub
    Private Sub MissingDataAddressTypeCode(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oParty As SAMForInsurance.ProxyWS.BasePartyType = oRequest.Item
        If oParty IsNot Nothing Then
            If oParty.Addresses IsNot Nothing Then
                oParty.Addresses(0).AddressTypeCode = Nothing
            End If
        End If
    End Sub
    Private Sub MissingDataPolicyVersion(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oPolicy() As SAMForInsurance.ProxyWS.BaseQuoteRiskMsgType = oRequest.PolicyVersion
        If oPolicy IsNot Nothing Then
            If oPolicy(0) IsNot Nothing Then
                oPolicy(0).PolicyVersion = 0
            End If
        End If
    End Sub
    Private Sub MissingDataPolicyStatusCode(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oPolicy() As SAMForInsurance.ProxyWS.BaseQuoteRiskMsgType = oRequest.PolicyVersion
        If oPolicy IsNot Nothing Then
            If oPolicy(0) IsNot Nothing Then
                oPolicy(0).PolicyStatusCode = String.Empty
            End If
        End If
    End Sub
    Private Sub MissingDataAccountsTransactions(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oAccounts() As SAMForInsurance.ProxyWS.BasePostDocumentRequestType = oRequest.AccountDocuments
        If oAccounts IsNot Nothing Then
            If oAccounts(0) IsNot Nothing Then
                oAccounts(0).Transactions = Nothing
            End If
        End If
    End Sub
    Private Sub MissingDataDocumentTypeCode(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oAccounts() As SAMForInsurance.ProxyWS.BasePostDocumentRequestType = oRequest.AccountDocuments
        If oAccounts IsNot Nothing Then
            If oAccounts(0) IsNot Nothing Then
                oAccounts(0).DocumentType = Nothing
            End If
        End If
    End Sub
    Private Sub MissingDataAccountsAccountCode(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oAccounts() As SAMForInsurance.ProxyWS.BasePostDocumentRequestType = oRequest.AccountDocuments
        If oAccounts IsNot Nothing Then
            If oAccounts(0) IsNot Nothing Then
                Dim oTransactions() As SAMForInsurance.ProxyWS.BaseTransactionType = oAccounts(0).Transactions
                If oTransactions IsNot Nothing Then
                    If oTransactions(0) IsNot Nothing Then
                        oTransactions(0).AccountCode = String.Empty
                    End If
                End If
            End If
        End If
    End Sub
#End Region

    ''' <summary>
    ''' Setup Test Case Scenarios which return business warnings
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>

#Region "Setup Business Warnings Test Cases"

    Private Sub BusinessWarningEXAMPLE(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        '    oRequest.Claim.LossFromDate = DateAdd(DateInterval.Day, 1, Date.Today)
    End Sub

#End Region

    ''' <summary>
    ''' Setup Test Case Scenarios which return business errrors
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <remarks></remarks>
#Region "Setup Business Errors Test Cases"

    Private Sub BusinessErrorEXAMPLE(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        '    oRequest.Claim.LossFromDate = DateAdd(DateInterval.Day, 1, Date.Today)
    End Sub

    Private Sub BusinessErrorEndDateBeforeStartDate(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oPolicy() As SAMForInsurance.ProxyWS.BaseQuoteRiskMsgType = oRequest.PolicyVersion
        If oPolicy IsNot Nothing Then
            If oPolicy(0) IsNot Nothing Then
                oPolicy(0).CoverEndDate = oPolicy(0).CoverStartDate.AddDays(-1)
            End If
        End If
    End Sub

    Private Sub BusinessErrorTooFewTransactions(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oAccounts() As SAMForInsurance.ProxyWS.BasePostDocumentRequestType = oRequest.AccountDocuments
        If oAccounts IsNot Nothing Then
            If oAccounts(0) IsNot Nothing Then
                Dim oTransactions() As SAMForInsurance.ProxyWS.BaseTransactionType = oAccounts(0).Transactions
                If oTransactions IsNot Nothing Then
                    Array.Resize(oTransactions, 1)
                    oAccounts(0).Transactions = oTransactions
                End If
            End If
        End If
    End Sub

    Private Sub BusinessErrorTransactionAmountsDontBalance(ByVal oRequest As ProxyWS.ClientDataImportRequestType)
        Dim oAccounts() As SAMForInsurance.ProxyWS.BasePostDocumentRequestType = oRequest.AccountDocuments
        If oAccounts IsNot Nothing Then
            If oAccounts(0) IsNot Nothing Then
                Dim oTransactions() As SAMForInsurance.ProxyWS.BaseTransactionType = oAccounts(0).Transactions
                If oTransactions IsNot Nothing Then
                    If oTransactions(0) IsNot Nothing Then
                        oTransactions(0).Amount = 5000
                    End If
                End If
            End If
        End If
    End Sub

#End Region

#End Region

#Region "Test Case Response Handlers"

    Private Sub ProcessTestCases(ByVal oRequest As ProxyWS.ClientDataImportRequestType, ByVal TestCase As TestCaseScenario)

        Select Case TestCase

            ' error free scenarios
            Case TestCaseScenario.None, _
                    TestCaseScenario.SuccessWithPartyPC, _
                    TestCaseScenario.SuccessWithPartyCC

            Case TestCaseScenario.AllNonMandatoryFieldsMissing
                AllNonMandatoryFieldsMissing(oRequest)

                ' Business Rule Errors
            Case TestCaseScenario.BusinessErrorEndDateBeforeStartDate
                BusinessErrorEndDateBeforeStartDate(oRequest)
            Case TestCaseScenario.BusinessErrorTooFewTransactions
                BusinessErrorTooFewTransactions(oRequest)
            Case TestCaseScenario.BusinessErrorTransactionAmountsDontBalance
                BusinessErrorTransactionAmountsDontBalance(oRequest)

                ' invalid lookup data scenarios
            Case TestCaseScenario.InvalidLookupBranchCode
                InvalidLookupBranchCode(oRequest)
            Case TestCaseScenario.InvalidLookupDataModelCode
                InvalidLookupDataModelCode(oRequest)
            Case TestCaseScenario.InvalidLookupScreenCode
                InvalidLookupScreenCode(oRequest)
            Case TestCaseScenario.InvalidLookupRiskTypeCode
                InvalidLookupRiskTypeCode(oRequest)
            Case TestCaseScenario.InvalidLookupProductCode
                InvalidLookupProductCode(oRequest)
            Case TestCaseScenario.InvalidLookupGenderCode
                InvalidLookupGenderCode(oRequest)
            Case TestCaseScenario.InvalidLookupOccupationCode
                InvalidLookupOccupationCode(oRequest)
            Case TestCaseScenario.InvalidLookupEmployersBusinessCode
                InvalidLookupEmployersBusinessCode(oRequest)
            Case TestCaseScenario.InvalidLookupBusinessCode
                InvalidLookupBusinessCode(oRequest)
            Case TestCaseScenario.InvalidLookupPolicyStatusCode
                InvalidLookupPolicyStatusCode(oRequest)
            Case TestCaseScenario.InvalidLookupAccountsAccountCode
                InvalidLookupAccountsAccountCode(oRequest)
            Case TestCaseScenario.InvalidDataAccountsAmount
                InvalidDataAccountsAmount(oRequest)

                ' Missing Data
            Case TestCaseScenario.MissingDataBranchCode
                MissingDataBranchCode(oRequest)
            Case TestCaseScenario.MissingDataClient
                MissingDataClient(oRequest)
            Case TestCaseScenario.MissingDataPolicy
                MissingDataPolicy(oRequest)
            Case TestCaseScenario.MissingDataCompanyName
                MissingDataCompanyName(oRequest)
            Case TestCaseScenario.MissingDataForename
                MissingDataForename(oRequest)
            Case TestCaseScenario.MissingDataSurname
                MissingDataSurname(oRequest)
            Case TestCaseScenario.MissingDataTitle
                MissingDataTitle(oRequest)
            Case TestCaseScenario.MissingDataInitials
                MissingDataInitials(oRequest)
            Case TestCaseScenario.MissingDataAddress
                MissingDataAddress(oRequest)
            Case TestCaseScenario.MissingDataAddressLine1
                MissingDataAddressLine1(oRequest)
            Case TestCaseScenario.MissingDataPostCode
                MissingDataPostCode(oRequest)
            Case TestCaseScenario.MissingDataCountryCode
                MissingDataCountryCode(oRequest)
            Case TestCaseScenario.MissingDataContact
                MissingDataContact(oRequest)
            Case TestCaseScenario.MissingDataContactType
                MissingDataContactType(oRequest)
            Case TestCaseScenario.MissingDataContactItem
                MissingDataContactItem(oRequest)
            Case TestCaseScenario.MissingDataContactDetail
                MissingDataContactDetail(oRequest)
            Case TestCaseScenario.MissingDataCoverStartDate
                MissingDataCoverStartDate(oRequest)
            Case TestCaseScenario.MissingDataCoverEndDate
                MissingDataCoverEndDate(oRequest)
            Case TestCaseScenario.MissingDataDescription
                MissingDataDescription(oRequest)
            Case TestCaseScenario.MissingDataProductCode
                MissingDataProductCode(oRequest)
            Case TestCaseScenario.MissingDataRisks
                MissingDataRisks(oRequest)
            Case TestCaseScenario.MissingDataRiskTypeCode
                MissingDataRiskTypeCode(oRequest)
            Case TestCaseScenario.MissingDataScreenCode
                MissingDataScreenCode(oRequest)
            Case TestCaseScenario.MissingDataRiskDescription
                MissingDataRiskDescription(oRequest)
            Case TestCaseScenario.MissingDataDataModelCode
                MissingDataDataModelCode(oRequest)
            Case TestCaseScenario.MissingDataAddressTypeCode
                MissingDataAddressTypeCode(oRequest)
            Case TestCaseScenario.MissingDataPolicyVersion
                MissingDataPolicyVersion(oRequest)
            Case TestCaseScenario.MissingDataPolicyStatusCode
                MissingDataPolicyStatusCode(oRequest)
            Case TestCaseScenario.MissingDataDocumentTypeCode
                MissingDataDocumentTypeCode(oRequest)
            Case TestCaseScenario.MissingDataAccountsTransactions
                MissingDataAccountsTransactions(oRequest)
            Case TestCaseScenario.MissingDataAccountsAccountCode
                MissingDataAccountsAccountCode(oRequest)

            Case Else
                Assert.Fail("No test case scenario has been configured")

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
            Case TestCaseScenario.BusinessErrorEndDateBeforeStartDate
                Assert.AreEqual(SAMConstants.SAMErrorCodes.CoverEndDateIsBeforeCoverStartDate, oError.Code, kInvalidCodeReturned)
                Assert.AreEqual("Cover End date is before Cover Start date", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.BusinessErrorTooFewTransactions
                Assert.AreEqual(SAMConstants.SAMBusinessErrors.LessThanTwoTransactionsInTransactionArray, oError.Code, kInvalidCodeReturned)
                Assert.AreEqual(SAMConstants.SAMBusinessErrors.LessThanTwoTransactionsInTransactionArray.ToString(), oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.BusinessErrorTransactionAmountsDontBalance
                Assert.AreEqual(SAMConstants.SAMBusinessErrors.TransactionAmountsDoNotBalance, oError.Code, kInvalidCodeReturned)
                Assert.AreEqual(SAMConstants.SAMBusinessErrors.TransactionAmountsDoNotBalance.ToString(), oError.Description, kInvalidDescriptionReturned)
            Case Else
                Assert.Fail("No BusinessRuleError condition was tested")
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
        '        Assert.AreEqual(SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString(), oError.Description, kInvalidDescriptionReturned)

        ' raise an error if the supplied value is not the one specified
        'Assert.AreEqual(kInvalidLookupValue, oError.SuppliedValue, kInvalidLookupValueMessage)

        ' raise an error if an invalid lookup table is being used (field name holds the lookup table name)
        Select Case TestCase
            Case TestCaseScenario.MissingDataBranchCode
                Assert.AreEqual("BranchCode", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual(SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString(), oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.MissingDataClient
                Assert.AreEqual("Party", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual(SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString(), oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.MissingDataPolicy
                Assert.AreEqual("PolicyVersion", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual(SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString(), oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.MissingDataCompanyName
                Assert.AreEqual("CompanyName", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual("Mandatory CompanyName is missing", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.MissingDataForename
                Assert.AreEqual("Forename", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual("Mandatory Forename is missing", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.MissingDataSurname
                Assert.AreEqual("Surname", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual("Mandatory Surname is missing", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.MissingDataTitle
                Assert.AreEqual("Title", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual("Mandatory Title is missing", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.MissingDataInitials
                Assert.AreEqual("Initials", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual("Mandatory Initials is missing", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.MissingDataAddress
                Assert.AreEqual("Address", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual("Mandatory Address is missing", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.MissingDataAddressLine1
                Assert.AreEqual("AddressLine1", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual("Mandatory AddressLine1 is missing", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.MissingDataPostCode
                Assert.AreEqual("PostCode", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual("Mandatory PostCode is missing", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.MissingDataCountryCode
                Assert.AreEqual("CountryCode", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual("Mandatory CountryCode is missing", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.MissingDataContact
                Assert.AreEqual("Contact", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual("Mandatory Contact is missing", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.MissingDataContactType
                Assert.AreEqual("ContactType", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual("Mandatory ContactType is missing", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.MissingDataContactItem
                Assert.AreEqual("ContactItem", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual("Mandatory ContactItem is missing", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.MissingDataContactDetail
                Assert.AreEqual("ContactItem", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual("Mandatory ContactItem is missing", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.MissingDataCoverStartDate
                Assert.AreEqual("CoverStartDate", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual("Mandatory CoverStartDate is missing", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.MissingDataCoverEndDate
                Assert.AreEqual("CoverEndDate", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual("Mandatory CoverEndDate is missing", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.MissingDataDescription
                Assert.AreEqual("Description", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual("Mandatory Description is missing", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.MissingDataProductCode
                Assert.AreEqual("ProductCode", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual("Mandatory ProductCode is missing", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.MissingDataRisks
                Assert.AreEqual("Risks", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual(SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString(), oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.MissingDataRiskTypeCode
                Assert.AreEqual("RiskTypeCode", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual("Mandatory RiskTypeCode is missing", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.MissingDataScreenCode
                Assert.AreEqual("ScreenCode", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual("Mandatory ScreenCode is missing", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.MissingDataRiskDescription
                Assert.AreEqual("RiskDescription", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual("Mandatory RiskDescription is missing", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.MissingDataDataModelCode
                Assert.AreEqual("DataModelCode", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual("Mandatory DataModelCode is missing", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.MissingDataAddressTypeCode
                Assert.AreEqual("AddressTypeCode", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual("Mandatory AddressTypeCode is missing", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.MissingDataPolicyVersion
                Assert.AreEqual("PolicyVersion", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual(SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString(), oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.MissingDataPolicyStatusCode
                Assert.AreEqual("PolicyStatusCode", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual(SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString(), oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.MissingDataDocumentTypeCode
                Assert.AreEqual("DocumentTypeCode", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual("Mandatory DocumentTypeCode is missing", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.MissingDataAccountsTransactions
                Assert.AreEqual("Transactions", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual(SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString(), oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.MissingDataAccountsAccountCode
                Assert.AreEqual("AccountCode", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual(SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString(), oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.InvalidDataAccountsAmount
                Assert.AreEqual("AccountsAmount", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual("Mandatory AccountsAmount is missing", oError.Description, kInvalidDescriptionReturned)
            Case Else
                Assert.Fail("No MissingData condition was tested")
        End Select

    End Sub

    Private Sub ProcessInvalidLookup(ByVal oResponse As ProxyWS.BaseResponseType, ByVal TestCase As TestCaseScenario)

        ' raise an error if no error was returned
        SAMTest.AssertCallFailedWithErrors(oResponse, 1)

        ' assign the error object
        Dim oError As ProxyWS.SAMErrorInvalidData = SAMTest.AssertErrorInvalidData(oResponse, 0)

        ' raise an errror if the error code (id) doesnt match the expected value
        If TestCase = TestCaseScenario.InvalidLookupBranchCode Then
            Assert.AreEqual(SAMConstants.SAMInvalidData.BranchCodeInvalid, oError.Code, kInvalidCodeReturned)
        ElseIf TestCase = TestCaseScenario.InvalidDataAccountsAmount Then
            Assert.AreEqual(SAMConstants.SAMInvalidData.TransactionAmountIsZero, oError.Code, kInvalidCodeReturned)
        Else
        Assert.AreEqual(SAMConstants.SAMInvalidData.InvalidLookupListValue, oError.Code, kInvalidCodeReturned)
        End If

        ' raise an error if the error description doesnt not match the expected value
        'Assert.AreEqual(SAMConstants.SAMInvalidData.InvalidLookupListValue.ToString(), Mid(oError.Description.ToString, 1, Len(SAMConstants.SAMInvalidData.InvalidLookupListValue.ToString)), kInvalidDescriptionReturned)

        ' raise an error if the supplied value is not the one specified
        If TestCase = TestCaseScenario.InvalidDataAccountsAmount Then
            Assert.AreEqual("0", oError.SuppliedValue, kInvalidLookupValueMessage)
        Else
            Assert.AreEqual(kInvalidLookupValue, oError.SuppliedValue, kInvalidLookupValueMessage)
        End If

        ' raise an error if an invalid lookup table is being used (field name holds the lookup table name)
        Select Case TestCase
            Case TestCaseScenario.InvalidLookupBranchCode
                Assert.AreEqual("BranchCode", oError.FieldName, kInvalidLookupFieldMessage)
                Assert.AreEqual("BranchCode is invalid", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.InvalidLookupDataModelCode
                Assert.AreEqual("DataModelCode", oError.FieldName, kInvalidLookupFieldMessage)
                Assert.AreEqual("DataModelCode is invalid", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.InvalidLookupScreenCode
                Assert.AreEqual("ScreenCode", oError.FieldName, kInvalidLookupFieldMessage)
                Assert.AreEqual("ScreenCode is invalid", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.InvalidLookupRiskTypeCode
                Assert.AreEqual("RiskTypeCode", oError.FieldName, kInvalidLookupFieldMessage)
                Assert.AreEqual("RiskTypeCode is invalid", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.InvalidLookupProductCode
                Assert.AreEqual("ProductCode", oError.FieldName, kInvalidLookupFieldMessage)
                Assert.AreEqual("ProductCode is invalid", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.InvalidLookupGenderCode
                Assert.AreEqual("GenderCode", oError.FieldName, kInvalidLookupFieldMessage)
                Assert.AreEqual("GenderCode is invalid", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.InvalidLookupOccupationCode
                Assert.AreEqual("OccupationCode", oError.FieldName, kInvalidLookupFieldMessage)
                Assert.AreEqual("OccupationCode is invalid", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.InvalidLookupEmployersBusinessCode
                Assert.AreEqual("EmployersBusinessCode", oError.FieldName, kInvalidLookupFieldMessage)
                Assert.AreEqual("EmployersBusinessCode is invalid", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.InvalidLookupBusinessCode
                Assert.AreEqual("BusinessCode", oError.FieldName, kInvalidLookupFieldMessage)
                Assert.AreEqual("BusinessCode is invalid", oError.Description, kInvalidDescriptionReturned)
            Case TestCaseScenario.InvalidLookupPolicyStatusCode
                Assert.AreEqual("PolicyStatusCode", oError.FieldName, kInvalidLookupFieldMessage)
                Assert.AreEqual(SAMConstants.SAMInvalidData.InvalidLookupListValue.ToString(), Mid(oError.Description.ToString, 1, Len(SAMConstants.SAMInvalidData.InvalidLookupListValue.ToString)), kInvalidDescriptionReturned)
            Case TestCaseScenario.InvalidLookupAccountsAccountCode
                Assert.AreEqual("AccountCode", oError.FieldName, kInvalidLookupFieldMessage)
                Assert.AreEqual(SAMConstants.SAMInvalidData.InvalidLookupListValue.ToString(), Mid(oError.Description.ToString, 1, Len(SAMConstants.SAMInvalidData.InvalidLookupListValue.ToString)), kInvalidDescriptionReturned)
            Case TestCaseScenario.InvalidDataAccountsAmount
                Assert.AreEqual("Amount", oError.FieldName, kIncorrectMissingMandatoryFieldMessage)
                Assert.AreEqual(SAMConstants.SAMInvalidData.TransactionAmountIsZero.ToString, oError.Description, kInvalidDescriptionReturned)
            Case Else
                Assert.Fail("No InvalidLookup condition was tested")
        End Select

    End Sub

#End Region

#Region "NUNIT Test Scenarios"

#Region "NUNIT Success Test Case"

    <Test()> _
    Public Sub Success()
        ClientDataImportTest(TestCaseScenario.SuccessWithPartyCC)
        ClientDataImportTest(TestCaseScenario.SuccessWithPartyPC)
        'TestCaseAllInvalidLookups()
        'TestCaseAllMissingData()
        'TestCaseAllBusinessWarnings()
        'TestCaseAllBusinessErrors()

    End Sub

#End Region

#Region "NUNIT Non Error Raising Scenarios"

    <Test()> _
    Public Sub AllNonMandatoryFieldsMissing()
        ClientDataImportTest(TestCase:=TestCaseScenario.AllNonMandatoryFieldsMissing)
    End Sub


#End Region

#Region "NUNIT Invalid Lookup Test Cases"

    <Test()> _
    Public Sub InvalidLookupBranchCode()
        ClientDataImportTest(TestCase:=TestCaseScenario.InvalidLookupBranchCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupDataModelCode()
        ClientDataImportTest(TestCase:=TestCaseScenario.InvalidLookupDataModelCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupScreenCode()
        ClientDataImportTest(TestCase:=TestCaseScenario.InvalidLookupScreenCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupRiskTypeCode()
        ClientDataImportTest(TestCase:=TestCaseScenario.InvalidLookupRiskTypeCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupProductCode()
        ClientDataImportTest(TestCase:=TestCaseScenario.InvalidLookupProductCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupGenderCode()
        ClientDataImportTest(TestCase:=TestCaseScenario.InvalidLookupGenderCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupOccupationCode()
        ClientDataImportTest(TestCase:=TestCaseScenario.InvalidLookupOccupationCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupEmployersBusinessCode()
        ClientDataImportTest(TestCase:=TestCaseScenario.InvalidLookupEmployersBusinessCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupBusinessCode()
        ClientDataImportTest(TestCase:=TestCaseScenario.InvalidLookupBusinessCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupPolicyStatusCode()
        ClientDataImportTest(TestCase:=TestCaseScenario.InvalidLookupPolicyStatusCode)
    End Sub

    <Test()> _
    Public Sub InvalidLookupAccountsAccountCode()
        ClientDataImportTest(TestCase:=TestCaseScenario.InvalidLookupAccountsAccountCode)
    End Sub

    <Test()> _
    Public Sub InvalidDataAccountsAmount()
        ClientDataImportTest(TestCase:=TestCaseScenario.InvalidDataAccountsAmount)
    End Sub

    Private Sub TestCaseAllInvalidLookups()

        InvalidLookupBranchCode()

    End Sub

#End Region

#Region "NUNIT Missing Data Test Cases"

    <Test()> _
    Public Sub MissingDataBranchCode()
        ClientDataImportTest(TestCase:=TestCaseScenario.MissingDataBranchCode)
    End Sub

    <Test()> _
    Public Sub MissingDataClient()
        ClientDataImportTest(TestCase:=TestCaseScenario.MissingDataClient)
    End Sub

    <Test()> _
    Public Sub MissingDataPolicy()
        ClientDataImportTest(TestCase:=TestCaseScenario.MissingDataPolicy)
    End Sub

    <Test()> _
    Public Sub MissingDataCompanyName()
        ClientDataImportTest(TestCase:=TestCaseScenario.MissingDataCompanyName)
    End Sub

    <Test()> _
    Public Sub MissingDataForename()
        ClientDataImportTest(TestCase:=TestCaseScenario.MissingDataForename)
    End Sub

    <Test()> _
    Public Sub MissingDataSurname()
        ClientDataImportTest(TestCase:=TestCaseScenario.MissingDataSurname)
    End Sub

    <Test()> _
    Public Sub MissingDataTitle()
        ClientDataImportTest(TestCase:=TestCaseScenario.MissingDataTitle)
    End Sub

    <Test()> _
    Public Sub MissingDataInitials()
        ClientDataImportTest(TestCase:=TestCaseScenario.MissingDataInitials)
    End Sub

    <Test()> _
    Public Sub MissingDataAddress()
        ClientDataImportTest(TestCase:=TestCaseScenario.MissingDataAddress)
    End Sub

    <Test()> _
    Public Sub MissingDataAddressLine1()
        ClientDataImportTest(TestCase:=TestCaseScenario.MissingDataAddressLine1)
    End Sub

    <Test()> _
    Public Sub MissingDataPostCode()
        ClientDataImportTest(TestCase:=TestCaseScenario.MissingDataPostCode)
    End Sub

    <Test()> _
    Public Sub MissingDataCountryCode()
        ClientDataImportTest(TestCase:=TestCaseScenario.MissingDataCountryCode)
    End Sub

    <Test()> _
    Public Sub MissingDataContactItem()
        ClientDataImportTest(TestCase:=TestCaseScenario.MissingDataContactItem)
    End Sub

    <Test()> _
    Public Sub MissingDataContactDetail()
        ClientDataImportTest(TestCase:=TestCaseScenario.MissingDataContactDetail)
    End Sub

    <Test()> _
    Public Sub MissingDataCoverStartDate()
        ClientDataImportTest(TestCase:=TestCaseScenario.MissingDataCoverStartDate)
    End Sub

    <Test()> _
    Public Sub MissingDataCoverEndDate()
        ClientDataImportTest(TestCase:=TestCaseScenario.MissingDataCoverEndDate)
    End Sub

    <Test()> _
    Public Sub MissingDataProductCode()
        ClientDataImportTest(TestCase:=TestCaseScenario.MissingDataProductCode)
    End Sub

    <Test()> _
    Public Sub MissingDataRisks()
        ClientDataImportTest(TestCase:=TestCaseScenario.MissingDataRisks)
    End Sub

    <Test()> _
    Public Sub MissingDataRiskTypeCode()
        ClientDataImportTest(TestCase:=TestCaseScenario.MissingDataRiskTypeCode)
    End Sub

    <Test()> _
    Public Sub MissingDataScreenCode()
        ClientDataImportTest(TestCase:=TestCaseScenario.MissingDataScreenCode)
    End Sub

    <Test()> _
    Public Sub MissingDataRiskDescription()
        ClientDataImportTest(TestCase:=TestCaseScenario.MissingDataRiskDescription)
    End Sub

    <Test()> _
    Public Sub MissingDataDataModelCode()
        ClientDataImportTest(TestCase:=TestCaseScenario.MissingDataDataModelCode)
    End Sub

    <Test()> _
    Public Sub MissingDataPolicyVersion()
        ClientDataImportTest(TestCase:=TestCaseScenario.MissingDataPolicyVersion)
    End Sub

    <Test()> _
    Public Sub MissingDataPolicyStatusCode()
        ClientDataImportTest(TestCase:=TestCaseScenario.MissingDataPolicyStatusCode)
    End Sub

    <Test()> _
    Public Sub MissingDataAccountsTransactions()
        ClientDataImportTest(TestCase:=TestCaseScenario.MissingDataAccountsTransactions)
    End Sub

    <Test()> _
    Public Sub MissingDataAccountsAccountCode()
        ClientDataImportTest(TestCase:=TestCaseScenario.MissingDataAccountsAccountCode)
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

    <Test()> _
    Public Sub BusinessErrorEndDateBeforeStartDate()
        ClientDataImportTest(TestCase:=TestCaseScenario.BusinessErrorEndDateBeforeStartDate)
    End Sub

    <Test()> _
    Public Sub BusinessErrorTooFewTransactions()
        ClientDataImportTest(TestCase:=TestCaseScenario.BusinessErrorTooFewTransactions)
    End Sub
    <Test()> _
    Public Sub BusinessErrorTransactionAmountsDontBalance()
        ClientDataImportTest(TestCase:=TestCaseScenario.BusinessErrorTransactionAmountsDontBalance)
    End Sub

    Private Sub TestCaseAllBusinessErrors()
        'BusinessErrorDuplicateClaimExists()
    End Sub

#End Region

#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        ClientDataImportTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        ClientDataImportTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        ClientDataImportTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        ClientDataImportTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class

