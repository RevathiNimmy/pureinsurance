Imports NUnit.Framework

<TestFixture()> _
Public Class PayClaim
    Inherits BaseTest

#Region "Private Declarations"

    Private Const kInvalidLookupFieldMessage As String = "Invalid Lookup Field Name Returned"

    Private Enum enumTestCaseScenario
        None

        '------------------------- MISSING DATA -------------------------
        AllNonMandatoryFieldsMissing
        MissingCurrencyCode
        MissingTaxGroupCode
        MissingBranchCode
        MissingBaseClaimKey
        MissingBaseClaimPerilKey
        MissingPartyKey
        MissingBasereserveKey

        NoReserves

        '------------------------- INVALID DATA -------------------------
        InvalidBranchCode
        InvalidPercentges
        InvalidBaseClaimKey
        InvalidBaseClaimPerilKey
        InvalidBaseReserveKey
        InvalidTaxGroupCode
        InvalidPartyKey
        InvalidCurrencyCode

        '------------------------- BUSINESS RULE ERRORS -------------------------
        DefferedReinsurance
        InfoOnlyClaims
        AdvanceTaxScript
        RequiredBankAccountDetails

        'MultiplePerilsMultiplePayments
        'MultiplePerilsMultipleReceipts
        'MultiplePerilsNoPayments
        'MultiplePerilsNoReceipts

        'SingleReserve
        'MulitpleReserve
        ExcessReserve
        ReverseExcess
    End Enum

    'Private Enum enumMissingData
    '    None
    '    BranchCode
    '    BaseClaimKey
    '    CurrencyCode
    '    PartyKey
    '    TaxGroupCode
    '    BaseClaimPerilKey
    'End Enum

    'Private Enum enumInvalidLookup
    '    None
    '    InvalidBranchCode
    '    InvalidPercentges
    '    InvalidBaseClaimKey
    '    InvalidBaseClaimPerilKey
    '    InvalidBaseReserveKey
    '    InvalidTaxGroupCode
    '    InvalidPartyKey
    '    InvalidCurrencyCode

    '    ' Does the payment processes if we have no reserve attached

    'End Enum

    'Private Enum enumSTSBusinessError
    '    None
    '    InvalidBranchCode
    '    InvalidClaimKey
    'End Enum

    Private m_oTestData As New TestData

#End Region

#Region "Setup Preconditions"

#End Region

#Region "Private Test Methods"

    Private Sub ProcessTestCases(ByVal oRequest As ProxyWS.PayClaimRequestType, ByVal TestCase As enumTestCaseScenario)

        Select Case TestCase
            Case enumTestCaseScenario.AllNonMandatoryFieldsMissing
                AllNonMandatoryFieldsMissing(oRequest)
            Case enumTestCaseScenario.MissingBaseClaimKey
                MissingBaseClaimKey(oRequest)
            Case enumTestCaseScenario.missingBaseClaimPerilKey
                MissingBaseClaimPerilKey(oRequest)
            Case enumTestCaseScenario.missingBaseReserveKey
                MissingBaseReserveKey(oRequest)
            Case enumTestCaseScenario.MissingBranchCode
                MissingBranchCode(oRequest)
            Case enumTestCaseScenario.MissingCurrencyCode
                MissingCurrencyCode(oRequest)
            Case enumTestCaseScenario.MissingPartyKey
                MissingpartyKey(oRequest)
            Case enumTestCaseScenario.NoReserves
                NoReserves(oRequest)
            Case enumTestCaseScenario.NoReserves
                NoReserves(oRequest)

            Case enumTestCaseScenario.InvalidBaseClaimKey
                InvalidBaseClaimKey(oRequest)
            Case enumTestCaseScenario.InvalidBaseClaimPerilKey
                InvalidBaseClaimPerilKey(oRequest)
            Case enumTestCaseScenario.InvalidBaseReserveKey
                InvalidBaseReserveKey(oRequest)
            Case enumTestCaseScenario.InvalidBranchCode
                InvalidBranchCode(oRequest)
            Case enumTestCaseScenario.InvalidCurrencyCode
                InvalidCurrencyCode(oRequest)
            Case enumTestCaseScenario.InvalidPartyKey
                InvalidpartyKey(oRequest)
            Case enumTestCaseScenario.InvalidPercentges
                Invalidpercentages(oRequest)
            Case enumTestCaseScenario.InvalidTaxGroupCode
                Invalidtaxgroupcode(oRequest)

            Case enumTestCaseScenario.DefferedReinsurance
                DefferedReinsurance(oRequest)
            Case enumTestCaseScenario.InfoOnlyClaims
                InfoOnlyClaims(oRequest)
            Case enumTestCaseScenario.RequiredBankAccountDetails
                BankValidation(oRequest)
            Case enumTestCaseScenario.AdvanceTaxScript
                AdvanceTaxScript(oRequest)
            Case enumTestCaseScenario.ReverseExcess
                AdvanceTaxScript(oRequest)

            Case Else
        End Select

    End Sub

#Region "Requisite Functions for Test Cases (Invalid Data)"

    Private Sub InvalidBranchCode(ByVal oPayClaimRequest As ProxyWS.PayClaimRequestType)
        oPayClaimRequest.BranchCode = "202"
    End Sub

    Private Sub InvalidBaseClaimKey(ByVal oPayClaimRequest As ProxyWS.PayClaimRequestType)
        oPayClaimRequest.ClaimPayment.BaseClaimKey = 8888888
    End Sub

    Private Sub InvalidBaseClaimPerilKey(ByVal oPayClaimRequest As ProxyWS.PayClaimRequestType)
        oPayClaimRequest.ClaimPayment.BaseClaimPerilKey = 9999999
    End Sub

    Private Sub InvalidBaseReserveKey(ByVal oPayClaimRequest As ProxyWS.PayClaimRequestType)
        For cnt As Integer = 0 To oPayClaimRequest.ClaimPayment.ClaimPaymentItem.GetUpperBound(0)
            oPayClaimRequest.ClaimPayment.ClaimPaymentItem(cnt).BaseReserveKey = cnt * 7777
        Next
    End Sub

    Private Sub InvalidCurrencyCode(ByVal oPayClaimRequest As ProxyWS.PayClaimRequestType)
        oPayClaimRequest.ClaimPayment.CurrencyCode = "Fame"
    End Sub

    Private Sub InvalidpartyKey(ByVal oPayClaimRequest As ProxyWS.PayClaimRequestType)
        oPayClaimRequest.ClaimPayment.PartyKey = 8989
    End Sub

    Private Sub InvalidTaxGroupCode(ByVal oPayClaimRequest As ProxyWS.PayClaimRequestType)
        For cnt As Integer = 0 To oPayClaimRequest.ClaimPayment.ClaimPaymentItem.GetUpperBound(0)
            oPayClaimRequest.ClaimPayment.ClaimPaymentItem(cnt).TaxGroupCode = "VISION"
        Next
    End Sub

    Private Sub InvalidPercentages(ByVal oPayClaimRequest As ProxyWS.PayClaimRequestType)
        oPayClaimRequest.ClaimPayment.AdvancedTaxDetails.InsuredPercentage = 110
        oPayClaimRequest.ClaimPayment.AdvancedTaxDetails.PayeePercentage = -10
        oPayClaimRequest.ClaimPayment.AdvancedTaxDetails.SafeHarbourPercentage = 500
    End Sub

#End Region

#Region "Requisite Functions for Test Cases (Missing Data)"

    Private Sub AllNonMandatoryFieldsMissing(ByVal oPayClaimRequest As ProxyWS.PayClaimRequestType)
        oPayClaimRequest.ClaimPayment.AdvancedTaxDetails.InsuranceTaxNumber = ""
        oPayClaimRequest.ClaimPayment.AdvancedTaxDetails.InsuredDomiciled = False
        oPayClaimRequest.ClaimPayment.AdvancedTaxDetails.InsuredPercentage = 0D
        oPayClaimRequest.ClaimPayment.AdvancedTaxDetails.IsSettlement = False
        oPayClaimRequest.ClaimPayment.AdvancedTaxDetails.IsTaxExempt = False
        oPayClaimRequest.ClaimPayment.AdvancedTaxDetails.IsWHTExempt = False
        oPayClaimRequest.ClaimPayment.AdvancedTaxDetails.PayeeDomiciled = False
        oPayClaimRequest.ClaimPayment.AdvancedTaxDetails.PayeePercentage = 0D
        oPayClaimRequest.ClaimPayment.AdvancedTaxDetails.PayeeTaxNumber = ""

        oPayClaimRequest.ClaimPayment.ClaimVersionDescription = ""

        oPayClaimRequest.ClaimPayment.Payee.Address.AddressLine1 = ""
        oPayClaimRequest.ClaimPayment.Payee.Address.AddressLine2 = ""
        oPayClaimRequest.ClaimPayment.Payee.Address.AddressLine3 = ""
        oPayClaimRequest.ClaimPayment.Payee.Address.AddressLine4 = ""
        oPayClaimRequest.ClaimPayment.Payee.Address.AddressTypeCode = ""
        oPayClaimRequest.ClaimPayment.Payee.Address.CountryCode = ""
        oPayClaimRequest.ClaimPayment.Payee.Address.PostCode = ""

    End Sub

    Private Sub MissingBranchCode(ByVal oPayClaimRequest As ProxyWS.PayClaimRequestType)
        oPayClaimRequest.BranchCode = Nothing
    End Sub

    Private Sub MissingBaseClaimKey(ByVal oPayClaimRequest As ProxyWS.PayClaimRequestType)
        oPayClaimRequest.ClaimPayment.BaseClaimKey = Nothing
    End Sub

    Private Sub MissingBaseClaimPerilKey(ByVal oPayClaimRequest As ProxyWS.PayClaimRequestType)
        oPayClaimRequest.ClaimPayment.BaseClaimPerilKey = Nothing
    End Sub

    Private Sub MissingBaseReserveKey(ByVal oPayClaimRequest As ProxyWS.PayClaimRequestType)
        For cnt As Integer = 0 To oPayClaimRequest.ClaimPayment.ClaimPaymentItem.GetUpperBound(0)
            oPayClaimRequest.ClaimPayment.ClaimPaymentItem(cnt).BaseReserveKey = Nothing
        Next
    End Sub

    Private Sub MissingCurrencyCode(ByVal oPayClaimRequest As ProxyWS.PayClaimRequestType)
        oPayClaimRequest.ClaimPayment.CurrencyCode = Nothing
    End Sub

    Private Sub MissingpartyKey(ByVal oPayClaimRequest As ProxyWS.PayClaimRequestType)
        oPayClaimRequest.ClaimPayment.PartyKey = Nothing
    End Sub

    Private Sub NoReserves(ByVal oPayClaimRequest As ProxyWS.PayClaimRequestType)
        oPayClaimRequest.ClaimPayment.ClaimPaymentItem = Nothing
    End Sub

#End Region

#Region "Business Rule Error"

    Private Sub DefferedReinsurance(ByVal oPayClaimRequest As ProxyWS.PayClaimRequestType)
        oPayClaimRequest.ClaimPayment.BaseClaimKey = 16
    End Sub

    Private Sub InfoOnlyClaims(ByVal oPayClaimRequest As ProxyWS.PayClaimRequestType)
        oPayClaimRequest.ClaimPayment.BaseClaimKey = 71
    End Sub

    Private Sub AdvanceTaxScript(ByVal oPayClaimRequest As ProxyWS.PayClaimRequestType)
        oPayClaimRequest.ClaimPayment.ClaimPaymentItem = Nothing
    End Sub

    Private Sub BankValidation(ByVal oPayClaimRequest As ProxyWS.PayClaimRequestType)
        oPayClaimRequest.ClaimPayment.Payee.MediaTypeCode = "EFT"
    End Sub

#End Region

#Region "Other Conditions"

    Private Sub ReverseExcess(ByVal oPayClaimRequest As ProxyWS.PayClaimRequestType)
        For cnt As Integer = 0 To oPayClaimRequest.ClaimPayment.ClaimPaymentItem.GetUpperBound(0)
            oPayClaimRequest.ClaimPayment.ClaimPaymentItem(cnt).ReverseExcess = True
        Next
        'oPayClaimRequest.ClaimPayment.ClaimPaymentItem
    End Sub

#End Region

    Private Sub PayClaimTest( _
        Optional ByVal TestCases As enumTestCaseScenario = enumTestCaseScenario.None, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.PayClaimRequestType
        Dim oResponse As ProxyWS.PayClaimResponseType
        Dim nLookupError As Integer = 102
        Dim nBusinessError As Integer = 224

        Try
            With oRequest

                Dim oGetClaimDetailsResponse As ProxyWS.GetClaimDetailsResponseType = nothing

                ' Call Get Claim Details Subroutine to get the time stamp
                '----------------------
                Dim oGetClaimDetails As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimDetails
                oGetClaimDetails.SupportMethod(oGetClaimDetailsResponse)
                '----------------------

                .TimeStamp = oGetClaimDetailsResponse.TimeStamp
                .BranchCode = m_oTestData.PayClaim.BranchCode
                .ClaimPayment = New ProxyWS.BaseClaimPaymentType

                '.ClaimPayment.BaseClaimKey = m_oTestData.PayClaim.BaseClaimKey
                '.ClaimPayment.BaseClaimPerilKey = m_oTestData.PayClaim.BaseClaimPerilKey
                .ClaimPayment.BaseClaimKey = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.BaseClaimKey

                ' just use the first claim peril 
                .ClaimPayment.BaseClaimPerilKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(0).BaseClaimPerilKey

                .ClaimPayment.ClaimVersionDescription = m_oTestData.PayClaim.ClaimVersionDescription
                .ClaimPayment.CurrencyCode = m_oTestData.PayClaim.CurrencyCode
                .ClaimPayment.PartyKey = m_oTestData.PayClaim.PartyKey
                .ClaimPayment.PaymentPartyType = m_oTestData.PayClaim.PaymentPartyType

                .ClaimPayment.Payee = New ProxyWS.BaseClaimPayeeType
                .ClaimPayment.Payee.BankCode = m_oTestData.PayClaim.oPayee.BankCode
                .ClaimPayment.Payee.BankName = m_oTestData.PayClaim.oPayee.BankName
                .ClaimPayment.Payee.BankNumber = m_oTestData.PayClaim.oPayee.BankNumber
                .ClaimPayment.Payee.Comments = m_oTestData.PayClaim.oPayee.Comments
                .ClaimPayment.Payee.MediaReference = m_oTestData.PayClaim.oPayee.MediaReference
                .ClaimPayment.Payee.MediaTypeCode = m_oTestData.PayClaim.oPayee.MediaTypeCode
                .ClaimPayment.Payee.Name = m_oTestData.PayClaim.oPayee.Name
                .ClaimPayment.Payee.TheirReference = m_oTestData.PayClaim.oPayee.TheirReference

                .ClaimPayment.Payee.Address = Nothing
                '.ClaimPayment.Payee.Address = New ProxyWS.BaseAddressType
                '.ClaimPayment.Payee.Address.AddressLine1 = m_oTestData.PayClaim.oPayee.oAddress.Line1
                '.ClaimPayment.Payee.Address.AddressLine2 = m_oTestData.PayClaim.oPayee.oAddress.Line2
                '.ClaimPayment.Payee.Address.AddressLine3 = m_oTestData.PayClaim.oPayee.oAddress.Line3
                '.ClaimPayment.Payee.Address.AddressLine4 = m_oTestData.PayClaim.oPayee.oAddress.Line4
                '.ClaimPayment.Payee.Address.AddressTypeCode = ProxyWS.AddressTypeType.Item3131001
                '.ClaimPayment.Payee.Address.CountryCode = m_oTestData.PayClaim.oPayee.oAddress.CountryCode
                '.ClaimPayment.Payee.Address.PostCode = m_oTestData.PayClaim.oPayee.oAddress.PostCode

                .ClaimPayment.AdvancedTaxDetails = Nothing

                '.ClaimPayment.AdvancedTaxDetails = New ProxyWS.BaseClaimPaymentAdvancedTaxDetailsType
                '.ClaimPayment.AdvancedTaxDetails.InsuranceTaxNumber = m_oTestData.PayClaim.oAdvanceTaxDetails.InsuranceTaxNumber
                '.ClaimPayment.AdvancedTaxDetails.InsuredDomiciled = m_oTestData.PayClaim.oAdvanceTaxDetails.InsuredDomiciled
                '.ClaimPayment.AdvancedTaxDetails.InsuredPercentage = m_oTestData.PayClaim.oAdvanceTaxDetails.InsuredPercentage
                '.ClaimPayment.AdvancedTaxDetails.IsSettlement = m_oTestData.PayClaim.oAdvanceTaxDetails.IsSettlement
                '.ClaimPayment.AdvancedTaxDetails.IsTaxExempt = m_oTestData.PayClaim.oAdvanceTaxDetails.IsTaxExempt
                '.ClaimPayment.AdvancedTaxDetails.IsWHTExempt = m_oTestData.PayClaim.oAdvanceTaxDetails.IsWHTExempt
                '.ClaimPayment.AdvancedTaxDetails.PayeeDomiciled = m_oTestData.PayClaim.oAdvanceTaxDetails.PayeeDomiciled
                '.ClaimPayment.AdvancedTaxDetails.PayeePercentage = m_oTestData.PayClaim.oAdvanceTaxDetails.PayeePercentage
                '.ClaimPayment.AdvancedTaxDetails.PayeeTaxNumber = m_oTestData.PayClaim.oAdvanceTaxDetails.PayeeTaxNumber
                '.ClaimPayment.AdvancedTaxDetails.SafeHarbourCode = m_oTestData.PayClaim.oAdvanceTaxDetails.SafeHarbourCode
                '.ClaimPayment.AdvancedTaxDetails.SafeHarbourPercentage = m_oTestData.PayClaim.oAdvanceTaxDetails.SafeHarbourPercentage


                Dim ListOfPaymentItems As New List(Of ProxyWS.BaseClaimPaymentItemType)

                For ReservesCnt As Integer = 0 To oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(0).Reserve.Length - 1

                    Dim PaymentItem As New ProxyWS.BaseClaimPaymentItemType

                    If oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(0).Reserve(ReservesCnt).TypeCode.ToUpper <> "EXCESS" Then

                        PaymentItem.BaseReserveKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(0).Reserve(ReservesCnt).BaseReserveKey
                        PaymentItem.PaymentAmount = 100
                        PaymentItem.ReverseExcess = False
                        PaymentItem.TaxGroupCode = "PGROUP"

                    Else

                        PaymentItem.BaseReserveKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(0).Reserve(ReservesCnt).BaseReserveKey
                        PaymentItem.PaymentAmount = -100
                        PaymentItem.ReverseExcess = False

                    End If

                    ListOfPaymentItems.Add(PaymentItem)

                Next

                .ClaimPayment.ClaimPaymentItem = ListOfPaymentItems.ToArray()

            End With

            ' reset fields based on selected test case
            ProcessTestCases(oRequest, TestCases)

            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.PayClaim(oRequest)

            With oResponse

                ' all these test cases should work without error
                If TestCases = enumTestCaseScenario.None OrElse _
                    TestCases = enumTestCaseScenario.AllNonMandatoryFieldsMissing OrElse _
                    TestCases = enumTestCaseScenario.NoReserves OrElse _
                    TestCases = enumTestCaseScenario.InvalidTaxGroupCode OrElse _
                    TestCases = enumTestCaseScenario.ReverseExcess OrElse _
                    TestCases = enumTestCaseScenario.ExcessReserve Then

                    SAMTest.AssertCallSucceeded(oResponse)

                ElseIf TestCases = enumTestCaseScenario.InvalidBaseClaimKey OrElse _
                     TestCases = enumTestCaseScenario.InvalidBaseClaimPerilKey OrElse _
                     TestCases = enumTestCaseScenario.InvalidBaseReserveKey OrElse _
                     TestCases = enumTestCaseScenario.InvalidBranchCode OrElse _
                     TestCases = enumTestCaseScenario.InvalidCurrencyCode OrElse _
                     TestCases = enumTestCaseScenario.InvalidPartyKey OrElse _
                     TestCases = enumTestCaseScenario.InvalidPercentges OrElse _
                     TestCases = enumTestCaseScenario.InvalidTaxGroupCode Then

                    ProcessInvalidLookup(oResponse, TestCases)

                ElseIf TestCases = enumTestCaseScenario.MissingBaseClaimKey OrElse _
                    TestCases = enumTestCaseScenario.MissingBaseClaimPerilKey OrElse _
                    TestCases = enumTestCaseScenario.MissingBasereserveKey OrElse _
                    TestCases = enumTestCaseScenario.MissingBranchCode OrElse _
                    TestCases = enumTestCaseScenario.MissingCurrencyCode OrElse _
                    TestCases = enumTestCaseScenario.MissingPartyKey OrElse _
                    TestCases = enumTestCaseScenario.MissingTaxGroupCode Then

                    ProcessMissingData(oResponse, TestCases)

                ElseIf TestCases = enumTestCaseScenario.DefferedReinsurance OrElse _
                    TestCases = enumTestCaseScenario.InfoOnlyClaims OrElse _
                    TestCases = enumTestCaseScenario.RequiredBankAccountDetails OrElse _
                    TestCases = enumTestCaseScenario.AdvanceTaxScript Then

                    ProcessBusinessErrors(oResponse, TestCases)
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

    Private Sub ProcessInvalidLookup(ByVal oResponse As ProxyWS.BaseResponseType, ByVal TestCase As enumTestCaseScenario)

        ' raise an error if no error was returned
        SAMTest.AssertCallFailedWithErrors(oResponse, 1)

        ' assign the error object
        Dim oError As ProxyWS.SAMErrorInvalidData = SAMTest.AssertErrorInvalidData(oResponse, 0)

        ' raise an errror if the error code (id) doesnt match the expected value
        'Assert.AreEqual(SAMConstants.SAMInvalidData.InvalidLookupListValue, oError.Code, kInvalidCodeReturned)

        ' raise an error if the error description doesnt not match the expected value
        'Assert.AreEqual(SAMConstants.SAMInvalidData.InvalidLookupListValue.ToString(), Mid(oError.Description.ToString, 1, Len(SAMConstants.SAMInvalidData.InvalidLookupListValue.ToString)), kInvalidDescriptionReturned)

        ' raise an error if an invalid lookup table is being used (field name holds the lookup table name)
        Select Case TestCase
            Case enumTestCaseScenario.InvalidBaseClaimKey
                Assert.AreEqual("BaseClaimKey", oError.FieldName, kInvalidLookupFieldMessage)
            Case enumTestCaseScenario.InvalidBaseClaimPerilKey
                Assert.AreEqual("BaseClaimPerilKey", oError.FieldName, kInvalidLookupFieldMessage)
            Case enumTestCaseScenario.InvalidBaseReserveKey
                Assert.AreEqual("BaseReserveKey", oError.FieldName, kInvalidLookupFieldMessage)
            Case enumTestCaseScenario.InvalidBranchCode
                Assert.AreEqual("BranchCode", oError.FieldName, kInvalidLookupFieldMessage)
            Case enumTestCaseScenario.InvalidCurrencyCode
                Assert.AreEqual("CurrencyCode", oError.FieldName, kInvalidLookupFieldMessage)
            Case enumTestCaseScenario.InvalidPartyKey
                Assert.AreEqual("PartyKey", oError.FieldName, kInvalidLookupFieldMessage)
            Case enumTestCaseScenario.InvalidPercentges
                Assert.AreEqual("InvalidPercentages", oError.FieldName, kInvalidLookupFieldMessage)
            Case enumTestCaseScenario.InvalidTaxGroupCode
                Assert.AreEqual("TaxGroupCode", oError.FieldName, kInvalidLookupFieldMessage)
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
        'Assert.AreEqual(SAMConstants.SAMInvalidData.InvalidLookupListValue.ToString(), Mid(oError.Description.ToString, 1, Len(SAMConstants.SAMInvalidData.InvalidLookupListValue.ToString)), kInvalidDescriptionReturned)

        ' raise an error if an invalid lookup table is being used (field name holds the lookup table name)
        Select Case TestCase
            Case enumTestCaseScenario.MissingBaseClaimKey
                Assert.AreEqual("BaseClaimKey", oError.FieldName, kInvalidLookupFieldMessage)
            Case enumTestCaseScenario.MissingBaseClaimPerilKey
                Assert.AreEqual("BaseClaimPerilKey", oError.FieldName, kInvalidLookupFieldMessage)
            Case enumTestCaseScenario.MissingBasereserveKey
                Assert.AreEqual("BaseReserveKey", oError.FieldName, kInvalidLookupFieldMessage)
            Case enumTestCaseScenario.MissingBranchCode
                Assert.AreEqual("BranchCode", oError.FieldName, kInvalidLookupFieldMessage)
            Case enumTestCaseScenario.MissingCurrencyCode
                Assert.AreEqual("CurrencyCode", oError.FieldName, kInvalidLookupFieldMessage)
        End Select

    End Sub

    Private Sub ProcessBusinessErrors(ByVal oResponse As ProxyWS.BaseResponseType, ByVal TestCase As enumTestCaseScenario)

        ' raise an error if no error was returned
        SAMTest.AssertCallFailedWithErrors(oResponse, 1)

        ' assign the error object
        Dim oError As ProxyWS.SAMErrorBusinessRule = SAMTest.AssertErrorBusinessRule(oResponse, 0)

        ' raise an error if an invalid lookup table is being used (field name holds the lookup table name)
        Select Case TestCase
            Case enumTestCaseScenario.AdvanceTaxScript
                'Assert.AreEqual(SAMConstants.SAMBusinessErrors, oError.Code, kInvalidCodeReturned)
                'Assert.AreEqual(SAMConstants.SAMBusinessErrors..ToString(), oError.Description, kInvalidDescriptionReturned)
            Case enumTestCaseScenario.DefferedReinsurance
                'Assert.AreEqual(SAMConstants.SAMBusinessErrors.LossFromDataInFuture, oError.Code, kInvalidCodeReturned)
                'Assert.AreEqual(SAMConstants.SAMBusinessErrors.LossFromDataInFuture.ToString(), oError.Description, kInvalidDescriptionReturned)
            Case enumTestCaseScenario.InfoOnlyClaims
                'Assert.AreEqual(SAMConstants.SAMBusinessErrors.LossFromDateAfterReportedDate, oError.Code, kInvalidCodeReturned)
                'Assert.AreEqual(SAMConstants.SAMBusinessErrors.LossFromDateAfterReportedDate.ToString(), oError.Description, kInvalidDescriptionReturned)
            Case enumTestCaseScenario.RequiredBankAccountDetails
                'Assert.AreEqual(SAMConstants.SAMBusinessErrors.LossFromDateAfterLossToDate, oError.Code, kInvalidCodeReturned)
                'Assert.AreEqual(SAMConstants.SAMBusinessErrors.LossFromDateAfterLossToDate.ToString(), oError.Description, kInvalidDescriptionReturned)
        End Select

    End Sub

#Region "Success"

    <Test()> _
    Public Sub Success()
        PayClaimTest()
    End Sub

#End Region

#Region "Missing Data"

    <Test()> _
    Public Sub Missing_AllNonMandatoryFields()
        PayClaimTest(TestCases:=enumTestCaseScenario.AllNonMandatoryFieldsMissing)
    End Sub

    <Test()> _
    Public Sub Missing_CurrencyCode()
        PayClaimTest(TestCases:=enumTestCaseScenario.MissingCurrencyCode)
    End Sub

    <Test()> _
    Public Sub Missing_PartyKey()
        PayClaimTest(TestCases:=enumTestCaseScenario.MissingPartyKey)
    End Sub

    <Test()> _
    Public Sub Massing_TaxGroupCode()
        PayClaimTest(TestCases:=enumTestCaseScenario.MissingTaxGroupCode)
    End Sub

    <Test()> _
    Public Sub Missing_BranchCode()
        PayClaimTest(TestCases:=enumTestCaseScenario.MissingBranchCode)
    End Sub

    <Test()> _
    Public Sub Missing_ClaimKey()
        PayClaimTest(TestCases:=enumTestCaseScenario.MissingBaseClaimKey)
    End Sub

    <Test()> _
    Public Sub Missing_ClaimPerilKey()
        PayClaimTest(TestCases:=enumTestCaseScenario.MissingBaseClaimPerilKey)
    End Sub

    <Test()> _
    Public Sub Missing_TaxGroupCode()
        PayClaimTest(TestCases:=enumTestCaseScenario.MissingTaxGroupCode)
    End Sub

    <Test()> _
    Public Sub MissingData_NoReserves()
        PayClaimTest(TestCases:=enumTestCaseScenario.NoReserves)
    End Sub

#End Region

#Region "Public Properties"

    'Public Property ReverseExcess() As Boolean
    '    Get

    '    End Get
    '    Set(ByVal value As Boolean)

    '    End Set

    'End Property

#End Region

#Region "Invalid Lookup"

    '<Test()> _
    Public Sub InvalidData_BranchCode()
        PayClaimTest(TestCases:=enumTestCaseScenario.InvalidBranchCode)
    End Sub

    <Test()> _
    Public Sub InvalidData_BaseClaimKey()
        PayClaimTest(TestCases:=enumTestCaseScenario.InvalidBaseClaimKey)
    End Sub

    <Test()> _
    Public Sub InvalidData_PartyKey()
        PayClaimTest(TestCases:=enumTestCaseScenario.InvalidPartyKey)
    End Sub

    <Test()> _
    Public Sub InvalidData_BaseClaimPerilKey()
        PayClaimTest(TestCases:=enumTestCaseScenario.InvalidBaseClaimPerilKey)
    End Sub

    <Test()> _
    Public Sub InvalidData_CurrencyCode()
        PayClaimTest(TestCases:=enumTestCaseScenario.InvalidCurrencyCode)
    End Sub

    <Test()> _
    Public Sub InvalidData_BaseReserveKey()
        PayClaimTest(TestCases:=enumTestCaseScenario.InvalidBaseReserveKey)
    End Sub

    <Test()> _
    Public Sub InvalidData_TaxGroupCode()
        PayClaimTest(TestCases:=enumTestCaseScenario.InvalidTaxGroupCode)
    End Sub

    <Test()> _
    Public Sub InvalidData_Percentages()
        PayClaimTest(TestCases:=enumTestCaseScenario.InvalidPercentges)
    End Sub

#End Region

#Region "STS Business Rules"

    '<Test()> _
    'Public Sub STSBusiness_InvalidClaimKey()
    '    GetClaimDetailsTest(eSTSBusinessError:=enumSTSBusinessError.ClaimKey)
    'End Sub
    '<Test()> _
    'Public Sub STSBusiness_InvalidBranchCode()
    '    GetClaimDetailsTest(eSTSBusinessError:=enumSTSBusinessError.BranchCode)
    'End Sub

#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        PayClaimTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        PayClaimTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        PayClaimTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        PayClaimTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class

#Region "Working Fine Data"

'Dim aoPaymentItem(1) As ProxyWS.BaseClaimPaymentItemType
'aoPaymentItem(0) = New ProxyWS.BaseClaimPaymentItemType

'aoPaymentItem(0).BaseReserveKey = "602" ' "671"
'aoPaymentItem(0).PaymentAmount = 10000
'aoPaymentItem(0).TaxGroupCode = "PGROUP"

'aoPaymentItem(1) = New ProxyWS.BaseClaimPaymentItemType

'aoPaymentItem(1).BaseReserveKey = "603"
'aoPaymentItem(1).PaymentAmount = 20000
'aoPaymentItem(1).TaxGroupCode = "PGROUP"

'aoPaymentItem(1).ReverseExcess = True

'objPayClaimRequestType.BranchCode = "HeadOff"
'objPayClaimRequestType.ClaimPayment = New ProxyWS.BaseClaimPaymentType
'objPayClaimRequestType.ClaimPayment.BaseClaimKey = 254 '278
'objPayClaimRequestType.ClaimPayment.BaseClaimPerilKey = 351 '377

'objPayClaimRequestType.ClaimPayment.ClaimVersionDescription = "Test"
''objPayClaimRequestType.ClaimPayment.Comments = "Test"
'objPayClaimRequestType.ClaimPayment.CurrencyCode = "EUR"
'objPayClaimRequestType.ClaimPayment.PartyKey = 1
'objPayClaimRequestType.ClaimPayment.PaymentPartyType = ProxyWS.ClaimPaymentPartyTypeType.PARTY

'objPayClaimRequestType.ClaimPayment.Payee = New ProxyWS.BaseClaimPayeeType
'objPayClaimRequestType.ClaimPayment.Payee.BankCode = 1
'objPayClaimRequestType.ClaimPayment.Payee.BankName = "KBI"
'objPayClaimRequestType.ClaimPayment.Payee.BankNumber = "22"
'objPayClaimRequestType.ClaimPayment.Payee.MediaReference = "Cheque"
'objPayClaimRequestType.ClaimPayment.Payee.MediaTypeCode = "BD"
'objPayClaimRequestType.ClaimPayment.Payee.Name = "Gaurav"
'objPayClaimRequestType.ClaimPayment.Payee.TheirReference = "Nothing"
'objPayClaimRequestType.ClaimPayment.Payee.Comments = "Test"

'objPayClaimRequestType.ClaimPayment.Payee.Address = New ProxyWS.BaseAddressType
'objPayClaimRequestType.ClaimPayment.Payee.Address.AddressLine1 = vbNull
'objPayClaimRequestType.ClaimPayment.Payee.Address.AddressLine2 = vbNull
'objPayClaimRequestType.ClaimPayment.Payee.Address.AddressLine3 = vbNull
'objPayClaimRequestType.ClaimPayment.Payee.Address.AddressLine4 = vbNull
'objPayClaimRequestType.ClaimPayment.Payee.Address.AddressTypeCode = ProxyWS.AddressTypeType.Item3131001
'objPayClaimRequestType.ClaimPayment.Payee.Address.CountryCode = "GBR"
'objPayClaimRequestType.ClaimPayment.Payee.Address.PostCode = ""

'objPayClaimRequestType.ClaimPayment.AdvancedTaxDetails = New ProxyWS.BaseClaimPaymentAdvancedTaxDetailsType
'objPayClaimRequestType.ClaimPayment.AdvancedTaxDetails.InsuranceTaxNumber = vbNull
'objPayClaimRequestType.ClaimPayment.AdvancedTaxDetails.InsuredDomiciled = vbNull
'objPayClaimRequestType.ClaimPayment.AdvancedTaxDetails.InsuredPercentage = vbNull
'objPayClaimRequestType.ClaimPayment.AdvancedTaxDetails.IsSettlement = True
'objPayClaimRequestType.ClaimPayment.AdvancedTaxDetails.IsTaxExempt = False
'objPayClaimRequestType.ClaimPayment.AdvancedTaxDetails.IsWHTExempt = False
'objPayClaimRequestType.ClaimPayment.AdvancedTaxDetails.PayeeDomiciled = True
'objPayClaimRequestType.ClaimPayment.AdvancedTaxDetails.PayeePercentage = 0

'objPayClaimRequestType.ClaimPayment.AdvancedTaxDetails.PayeeTaxNumber = vbNull

'objPayClaimRequestType.ClaimPayment.AdvancedTaxDetails.SafeHarbourCode = "T" 'vbNull
'objPayClaimRequestType.ClaimPayment.AdvancedTaxDetails.SafeHarbourPercentage = vbNull

'Dim aoPaymentItem(1) As ProxyWS.BaseClaimPaymentItemType
'aoPaymentItem(0) = New ProxyWS.BaseClaimPaymentItemType

'aoPaymentItem(0).BaseReserveKey = "602" ' "671"
'aoPaymentItem(0).PaymentAmount = 10000
'aoPaymentItem(0).TaxGroupCode = "PGROUP"

'aoPaymentItem(1) = New ProxyWS.BaseClaimPaymentItemType

'aoPaymentItem(1).BaseReserveKey = "603"
'aoPaymentItem(1).PaymentAmount = 20000
'aoPaymentItem(1).TaxGroupCode = "PGROUP"

'aoPaymentItem(1).ReverseExcess = True

#End Region

