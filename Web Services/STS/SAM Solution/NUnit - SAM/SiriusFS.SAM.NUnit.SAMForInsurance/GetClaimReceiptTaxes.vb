Imports NUnit.Framework

<TestFixture()> _
Public Class GetClaimreceiptTaxes
    Inherits BaseTest

#Region "Private Declarations"

    ' Private m_nTransactionType As String = "CO"
    'Private m_btPartyTimeStamp() As Byte

    Private m_oTestData As New TestData
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
        MissingBaserecoveryKey

        NoRecovery

        '------------------------- INVALID DATA -------------------------
        InvalidBranchCode
        InvalidPercentges
        InvalidBaseClaimKey
        InvalidBaseClaimPerilKey
        InvalidBaseRecoveryKey
        InvalidTaxGroupCode
        InvalidPartyKey
        InvalidCurrencyCode

    End Enum

#End Region

#Region "Setup Preconditions"

#End Region

#Region "Private Test Methods"

    Private Sub ProcessClaimReceiptTest( _
        Optional ByVal TestCases As enumTestCaseScenario = enumTestCaseScenario.None, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.GetClaimReceiptTaxesRequestType
        Dim oResponse As ProxyWS.GetClaimReceiptTaxesResponseType
        Dim nLookupError As Integer = 102
        Dim nBusinessError As Integer = 224

        Try
            With oRequest

                Dim oGetClaimDetailsResponse As ProxyWS.GetClaimDetailsResponseType = Nothing

                ' Call Get Claim Details Subroutine to get the time stamp
                '----------------------
                Dim oGetClaimDetails As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimDetails
                oGetClaimDetails.SupportMethod(oGetClaimDetailsResponse)
                '----------------------

                .TimeStamp = oGetClaimDetailsResponse.TimeStamp
                .BranchCode = m_oTestData.PayClaim.BranchCode
                .ClaimReceipt = New ProxyWS.BaseClaimReceiptType

                .ClaimReceipt.BaseClaimKey = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.BaseClaimKey

                ' just use the first claim peril 
                .ClaimReceipt.BaseClaimPerilKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(0).BaseClaimPerilKey

                .ClaimReceipt.ClaimVersionDescription = m_oTestData.PayClaim.ClaimVersionDescription
                .ClaimReceipt.CurrencyCode = m_oTestData.PayClaim.CurrencyCode
                .ClaimReceipt.PartyKey = m_oTestData.PayClaim.PartyKey
                .ClaimReceipt.ReceiptPartyType = m_oTestData.PayClaim.PaymentPartyType

                .ClaimReceipt.Payee = New ProxyWS.BaseClaimPayeeType
                .ClaimReceipt.Payee.BankCode = m_oTestData.PayClaim.oPayee.BankCode
                .ClaimReceipt.Payee.BankName = m_oTestData.PayClaim.oPayee.BankName
                .ClaimReceipt.Payee.BankNumber = m_oTestData.PayClaim.oPayee.BankNumber
                .ClaimReceipt.Payee.Comments = m_oTestData.PayClaim.oPayee.Comments
                .ClaimReceipt.Payee.MediaReference = m_oTestData.PayClaim.oPayee.MediaReference
                .ClaimReceipt.Payee.MediaTypeCode = m_oTestData.PayClaim.oPayee.MediaTypeCode
                .ClaimReceipt.Payee.Name = m_oTestData.PayClaim.oPayee.Name
                .ClaimReceipt.Payee.TheirReference = m_oTestData.PayClaim.oPayee.TheirReference

                .ClaimReceipt.Payee.Address = Nothing
                '.ClaimReceipt.Payee.Address = New ProxyWS.BaseAddressType
                '.ClaimReceipt.Payee.Address.AddressLine1 = "PayeeAddressLine1"
                '.ClaimReceipt.Payee.Address.AddressLine2 = "PayeeAddessLine2"
                '.ClaimReceipt.Payee.Address.AddressLine3 = "PayeeAddessLine3"
                '.ClaimReceipt.Payee.Address.AddressLine4 = "PayeeAddessLine4"
                '.ClaimReceipt.Payee.Address.AddressTypeCode = ProxyWS.AddressTypeType.Item3131001
                '.ClaimReceipt.Payee.Address.CountryCode = m_oTestData.PayClaim.oPayee.oAddress.CountryCode
                '.ClaimReceipt.Payee.Address.PostCode = "B12 8BQ"
                '.ClaimReceipt.AdvancedTaxDetails = Nothing

                .ClaimReceipt.AdvancedTaxDetails = Nothing
                '.ClaimReceipt.AdvancedTaxDetails = New ProxyWS.BaseClaimReceiptAdvancedTaxDetailsType
                '.ClaimReceipt.AdvancedTaxDetails.InsuredDomiciled = True
                '.ClaimReceipt.AdvancedTaxDetails.InsuredDomiciledSpecified = True
                '.ClaimReceipt.AdvancedTaxDetails.InsuredPercentage = 4
                '.ClaimReceipt.AdvancedTaxDetails.InsuredPercentageSpecified = True
                '.ClaimReceipt.AdvancedTaxDetails.InsuredTaxNumber = "TAXNUM1234"
                '.ClaimReceipt.AdvancedTaxDetails.IsSettlement = False
                '.ClaimReceipt.AdvancedTaxDetails.IsSettlementSpecified = True
                '.ClaimReceipt.AdvancedTaxDetails.IsTaxExempt = False
                '.ClaimReceipt.AdvancedTaxDetails.IsTaxExemptSpecified = True
                '.ClaimReceipt.AdvancedTaxDetails.ReceivableTaxPercentage = 5
                '.ClaimReceipt.AdvancedTaxDetails.ReceivableTaxPercentageSpecified = True

                Dim aoReceiptItem(oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(0).Recovery.Length - 1) As ProxyWS.BaseClaimReceiptItemType

                For RecoveryCnt As Integer = 0 To oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(0).Recovery.Length - 1
                    aoReceiptItem(RecoveryCnt) = New ProxyWS.BaseClaimReceiptItemType
                    aoReceiptItem(RecoveryCnt).BaseRecoveryKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(0).Recovery(RecoveryCnt).BaseRecoveryKey

                    If oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(0).Recovery(RecoveryCnt) IsNot Nothing AndAlso _
                             oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(0).Recovery(RecoveryCnt).InitialRecovery <> 0 Then

                        If m_oTestData.PayClaim.oPaymentItems IsNot Nothing AndAlso _
                           m_oTestData.PayClaim.oPaymentItems.Count - 1 >= RecoveryCnt Then

                            aoReceiptItem(RecoveryCnt).ReceiptAmount = 500 ' m_oTestData.PayClaim.oPaymentItems(RecoveryCnt).PaymentAmount
                            aoReceiptItem(RecoveryCnt).TaxGroupCode = m_oTestData.PayClaim.oPaymentItems(RecoveryCnt).TaxGroupCode

                        Else
                            aoReceiptItem(RecoveryCnt).ReceiptAmount = 100
                            aoReceiptItem(RecoveryCnt).TaxGroupCode = "PGROUP"
                        End If

                    Else
                        aoReceiptItem(RecoveryCnt).ReceiptAmount = 0
                        aoReceiptItem(RecoveryCnt).TaxGroupCode = String.Empty
                    End If

                Next

                .ClaimReceipt.ReceiptItem = aoReceiptItem

            End With

            ProcessTestCases(oRequest, TestCases)
            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.GetClaimReceiptTaxes(oRequest)

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

    Private Sub ProcessTestCases(ByVal oRequest As ProxyWS.ClaimReceiptRequestType, ByVal TestCase As enumTestCaseScenario)

        Select Case TestCase
            Case enumTestCaseScenario.AllNonMandatoryFieldsMissing
                AllNonMandatoryFieldsMissing(oRequest)
            Case enumTestCaseScenario.MissingBaseClaimKey
                MissingBaseClaimKey(oRequest)
            Case enumTestCaseScenario.MissingBaseClaimPerilKey
                MissingBaseClaimPerilKey(oRequest)
            Case enumTestCaseScenario.MissingBaserecoveryKey
                MissingBaseRecoveryKey(oRequest)
            Case enumTestCaseScenario.MissingBranchCode
                MissingBranchCode(oRequest)
            Case enumTestCaseScenario.MissingCurrencyCode
                MissingCurrencyCode(oRequest)
            Case enumTestCaseScenario.MissingPartyKey
                MissingpartyKey(oRequest)
            Case enumTestCaseScenario.NoRecovery
                NoRecovery(oRequest)
            Case enumTestCaseScenario.InvalidBaseClaimKey
                InvalidBaseClaimKey(oRequest)
            Case enumTestCaseScenario.InvalidBaseClaimPerilKey
                InvalidBaseClaimPerilKey(oRequest)
            Case enumTestCaseScenario.InvalidBaseRecoveryKey
                InvalidBaseReserveKey(oRequest)
            Case enumTestCaseScenario.InvalidBranchCode
                InvalidBranchCode(oRequest)
            Case enumTestCaseScenario.InvalidCurrencyCode
                InvalidCurrencyCode(oRequest)
            Case enumTestCaseScenario.InvalidPartyKey
                InvalidpartyKey(oRequest)
            Case enumTestCaseScenario.InvalidPercentges
                InvalidPercentages(oRequest)
            Case enumTestCaseScenario.InvalidTaxGroupCode
                InvalidTaxGroupCode(oRequest)

            Case Else
        End Select

    End Sub

#End Region

#Region "Success"

    <Test()> _
    Public Sub Success()
        ProcessClaimReceiptTest()
    End Sub

#End Region

#Region "Requisite Functions for Test Cases (Missing Data)"

    Private Sub AllNonMandatoryFieldsMissing(ByVal oClaimReceiptRequest As ProxyWS.ClaimReceiptRequestType)

        oClaimReceiptRequest.ClaimReceipt.AdvancedTaxDetails.InsuredTaxNumber = ""
        oClaimReceiptRequest.ClaimReceipt.AdvancedTaxDetails.InsuredDomiciled = False
        oClaimReceiptRequest.ClaimReceipt.AdvancedTaxDetails.InsuredPercentage = 0
        oClaimReceiptRequest.ClaimReceipt.AdvancedTaxDetails.IsSettlement = False
        oClaimReceiptRequest.ClaimReceipt.AdvancedTaxDetails.IsTaxExempt = False
        oClaimReceiptRequest.ClaimReceipt.AdvancedTaxDetails.InsuredDomiciled = False
        oClaimReceiptRequest.ClaimReceipt.AdvancedTaxDetails.InsuredPercentage = 0
        oClaimReceiptRequest.ClaimReceipt.AdvancedTaxDetails.InsuredTaxNumber = ""
        oClaimReceiptRequest.ClaimReceipt.ClaimVersionDescription = ""
        oClaimReceiptRequest.ClaimReceipt.Payee.Address.AddressLine1 = ""
        oClaimReceiptRequest.ClaimReceipt.Payee.Address.AddressLine2 = ""
        oClaimReceiptRequest.ClaimReceipt.Payee.Address.AddressLine3 = ""
        oClaimReceiptRequest.ClaimReceipt.Payee.Address.AddressLine4 = ""
        oClaimReceiptRequest.ClaimReceipt.Payee.Address.AddressTypeCode = 0
        oClaimReceiptRequest.ClaimReceipt.Payee.Address.CountryCode = ""
        oClaimReceiptRequest.ClaimReceipt.Payee.Address.PostCode = ""

    End Sub

    Private Sub MissingBranchCode(ByVal oClaimReceiptRequest As ProxyWS.ClaimReceiptRequestType)
        oClaimReceiptRequest.BranchCode = ""
    End Sub

    Private Sub MissingBaseClaimKey(ByVal oClaimReceiptRequest As ProxyWS.ClaimReceiptRequestType)
        oClaimReceiptRequest.ClaimReceipt.BaseClaimKey = Nothing
    End Sub

    Private Sub MissingBaseClaimPerilKey(ByVal oClaimReceiptRequest As ProxyWS.ClaimReceiptRequestType)
        oClaimReceiptRequest.ClaimReceipt.BaseClaimPerilKey = Nothing
    End Sub
   _
    Private Sub MissingBaseRecoveryKey(ByVal oClaimReceiptRequest As ProxyWS.ClaimReceiptRequestType)
        For cnt As Integer = 0 To oClaimReceiptRequest.ClaimReceipt.ReceiptItem.GetUpperBound(0)
            oClaimReceiptRequest.ClaimReceipt.ReceiptItem(cnt).BaseRecoveryKey = Nothing
        Next
    End Sub

    Private Sub MissingCurrencyCode(ByVal oClaimReceiptRequest As ProxyWS.ClaimReceiptRequestType)
        oClaimReceiptRequest.ClaimReceipt.CurrencyCode = Nothing
    End Sub

    Private Sub MissingpartyKey(ByVal oClaimReceiptRequest As ProxyWS.ClaimReceiptRequestType)
        oClaimReceiptRequest.ClaimReceipt.PartyKey = Nothing
    End Sub

    Private Sub NoRecovery(ByVal oClaimReceiptRequest As ProxyWS.ClaimReceiptRequestType)
        oClaimReceiptRequest.ClaimReceipt.ReceiptItem = Nothing
    End Sub

#End Region

#Region "Requisite Functions for Test Cases (Invalid Data)"

    Private Sub InvalidBranchCode(ByVal oClaimReceiptRequest As ProxyWS.ClaimReceiptRequestType)
        oClaimReceiptRequest.BranchCode = "202"
    End Sub

    Private Sub InvalidBaseClaimKey(ByVal oClaimReceiptRequest As ProxyWS.ClaimReceiptRequestType)
        oClaimReceiptRequest.ClaimReceipt.BaseClaimKey = 4444444444
    End Sub

    Private Sub InvalidBaseClaimPerilKey(ByVal oClaimReceiptRequest As ProxyWS.ClaimReceiptRequestType)
        oClaimReceiptRequest.ClaimReceipt.BaseClaimPerilKey = "1000009"
    End Sub

    Private Sub InvalidBaseReserveKey(ByVal oClaimReceiptRequest As ProxyWS.ClaimReceiptRequestType)
        For cnt As Integer = 0 To oClaimReceiptRequest.ClaimReceipt.ReceiptItem.GetUpperBound(0)
            oClaimReceiptRequest.ClaimReceipt.ReceiptItem(cnt).BaseRecoveryKey = cnt * 7777
        Next
    End Sub

    Private Sub InvalidCurrencyCode(ByVal oClaimReceiptRequest As ProxyWS.ClaimReceiptRequestType)
        oClaimReceiptRequest.ClaimReceipt.CurrencyCode = "Test"
    End Sub

    Private Sub InvalidpartyKey(ByVal oClaimReceiptRequest As ProxyWS.ClaimReceiptRequestType)
        oClaimReceiptRequest.ClaimReceipt.PartyKey = 570000
    End Sub

    Private Sub InvalidTaxGroupCode(ByVal oClaimReceiptRequest As ProxyWS.ClaimReceiptRequestType)
        For cnt As Integer = 0 To oClaimReceiptRequest.ClaimReceipt.ReceiptItem.GetUpperBound(0)
            oClaimReceiptRequest.ClaimReceipt.ReceiptItem(cnt).TaxGroupCode = "GMTV"
        Next
    End Sub

    Private Sub InvalidPercentages(ByVal oClaimReceiptRequest As ProxyWS.ClaimReceiptRequestType)
        oClaimReceiptRequest.ClaimReceipt.AdvancedTaxDetails.InsuredPercentage = 101
    End Sub

#End Region

#Region "Missing Data"

    <Test()> _
    Public Sub Missing_AllNonMandatoryFields()
        ProcessClaimReceiptTest(TestCases:=enumTestCaseScenario.AllNonMandatoryFieldsMissing)
    End Sub

    <Test()> _
    Public Sub Missing_CurrencyCode()
        ProcessClaimReceiptTest(TestCases:=enumTestCaseScenario.MissingCurrencyCode)
    End Sub

    <Test()> _
    Public Sub Missing_PartyKey()
        ProcessClaimReceiptTest(TestCases:=enumTestCaseScenario.MissingPartyKey)
    End Sub

    <Test()> _
    Public Sub Massing_TaxGroupCode()
        ProcessClaimReceiptTest(TestCases:=enumTestCaseScenario.MissingTaxGroupCode)
    End Sub

    <Test()> _
    Public Sub Missing_BranchCode()
        ProcessClaimReceiptTest(TestCases:=enumTestCaseScenario.MissingBranchCode)
    End Sub

    <Test()> _
    Public Sub Missing_ClaimKey()
        ProcessClaimReceiptTest(TestCases:=enumTestCaseScenario.MissingBaseClaimKey)
    End Sub

    <Test()> _
    Public Sub Missing_ClaimPerilKey()
        ProcessClaimReceiptTest(TestCases:=enumTestCaseScenario.MissingBaseClaimPerilKey)
    End Sub

    <Test()> _
    Public Sub Missing_TaxGroupCode()
        ProcessClaimReceiptTest(TestCases:=enumTestCaseScenario.MissingTaxGroupCode)
    End Sub

    <Test()> _
    Public Sub MissingData_NoRecovery()
        ProcessClaimReceiptTest(TestCases:=enumTestCaseScenario.NoRecovery)
    End Sub

#End Region

#Region "Invalid Lookup"

    <Test()> _
    Public Sub InvalidData_BranchCode()
        ProcessClaimReceiptTest(TestCases:=enumTestCaseScenario.InvalidBranchCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_BaseClaimKey()
        ProcessClaimReceiptTest(TestCases:=enumTestCaseScenario.InvalidBaseClaimKey)
    End Sub
    <Test()> _
    Public Sub InvalidData_BaseClaimPerilKey()
        ProcessClaimReceiptTest(TestCases:=enumTestCaseScenario.InvalidBaseClaimPerilKey)
    End Sub
    <Test()> _
    Public Sub InvalidData_BaseRecoveryKey()
        ProcessClaimReceiptTest(TestCases:=enumTestCaseScenario.InvalidBaseRecoveryKey)
    End Sub
    <Test()> _
    Public Sub InvalidData_CurrencyCode()
        ProcessClaimReceiptTest(TestCases:=enumTestCaseScenario.InvalidCurrencyCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_partyKey()
        ProcessClaimReceiptTest(TestCases:=enumTestCaseScenario.InvalidPartyKey)
    End Sub
    <Test()> _
    Public Sub InvalidData_TaxGroupCode()
        ProcessClaimReceiptTest(TestCases:=enumTestCaseScenario.InvalidTaxGroupCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_Percentages()
        ProcessClaimReceiptTest(TestCases:=enumTestCaseScenario.InvalidPercentges)
    End Sub

#End Region

#Region "STS Business Rules"

    <Test()> _
    Public Sub STSBusiness_InvalidClaimKey()
        'GenerateClaimsDocumentsTest(eSTSBusinessError:=enumSTSBusinessError.InvalidClaimKey)
    End Sub

#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        ProcessClaimReceiptTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        ProcessClaimReceiptTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        ProcessClaimReceiptTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        ProcessClaimReceiptTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class
