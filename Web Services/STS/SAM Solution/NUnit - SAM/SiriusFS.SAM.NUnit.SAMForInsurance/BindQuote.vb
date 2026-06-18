Imports NUnit.Framework

<TestFixture()> _
Public Class BindQuote
    Inherits BaseTest

#Region " Private Declarations "

    Private m_nInsuranceFileCnt As Integer
    Private m_dblPremiumDueGross As Double
    Private m_nRiskCnt As Integer
    Private m_vQuoteArray() As Object 'Remove - ProxyWS.BaseGetInstalmentQuotesResponseTypeRow

    Private m_oTestData As New TestData

    Private Enum enumMissingData
        None
        BranchCode
        InsuranceFileKey
    End Enum

    Private Enum enumInvalidLookup
        None
        BranchCode
    End Enum

    Private Enum enumSTSBusinessError
        None
        InvalidInsuranceFileCnt
    End Enum

#End Region

#Region " Setup Preconditions "

    Private Sub UpdateRisk()

        Dim oUpdateRisk As New UpdateRisk
        oUpdateRisk.SupportMethod(m_nInsuranceFileCnt, m_nRiskCnt, m_dblPremiumDueGross)

    End Sub

    Private Sub GetInstalmentQuotes()

        Dim oGetInstalmentQuotes As New GetInstalmentQuotes
        oGetInstalmentQuotes.SupportMethod(m_nInsuranceFileCnt, m_dblPremiumDueGross, m_vQuoteArray)

    End Sub

#End Region

#Region " Private Test Methods "

    Private Sub BindQuoteTest( _
                    Optional ByVal eMissingData As enumMissingData = enumMissingData.None, _
                    Optional ByVal eInvalidLookup As enumInvalidLookup = enumInvalidLookup.None, _
                    Optional ByVal eSTSBusinessError As enumSTSBusinessError = enumSTSBusinessError.None, _
                    Optional ByVal blWithInstalments As Boolean = False, _
                    Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.BindQuoteRequestType
        Dim oResponse As ProxyWS.BindQuoteResponseType
        Dim nLookupError As Integer = 102
        Dim nBusinessError As Integer = 224

        If blWithInstalments = True Then
            GetInstalmentQuotes()
        Else
            ' Add the Party/Quote/Risk and update the risk
            UpdateRisk()
        End If

        Try
            With oRequest
                If eMissingData <> enumMissingData.BranchCode Then
                    If eInvalidLookup = enumInvalidLookup.BranchCode Then
                        .BranchCode = m_oTestData.InvalidLookupCode
                        nLookupError = 210
                    Else
                        .BranchCode = m_oTestData.BranchCode
                    End If
                End If
                If eMissingData <> enumMissingData.InsuranceFileKey Then
                    If eSTSBusinessError <> enumSTSBusinessError.InvalidInsuranceFileCnt Then
                        .InsuranceFileKey = m_nInsuranceFileCnt
                    Else
                        .InsuranceFileKey = m_oTestData.InvalidCnt
                    End If
                End If
                If blWithInstalments = True Then
                    If IsArray(m_vQuoteArray) = True Then
                        .PaymentMethod = ProxyWS.PaymentMethodType.AgentCollection
                        .PaymentMethodSpecified = True
                        .SelectedInstalmentQuote = New ProxyWS.BaseSelectedInstalmentQuoteType
                        With .SelectedInstalmentQuote
                            .SelectedSchemeNo = m_vQuoteArray(0).SchemeNo
                            .SelectedSchemeVersion = m_vQuoteArray(0).SchemeVersion
                            .PFRF_ID = m_vQuoteArray(0).PFRF_ID
                            .AmountToFinance = m_dblPremiumDueGross
                            .StartDate = Now
                            .EndDate = Now.AddYears(1)
                            .MonthDay = 1
                            .PreferredDate = Now.AddDays(7)
                            .QuoteDate = Now
                            .PaymentProtection = False
                            .WeekDay = 1
                            .OverrideInterestRate = -1
                            .OverrideRate = -1
                            .BankAccountName = "Fred"
                            .BankAccountNo = "123456789"
                            .BankSortCode = "456789"
                            .BankAddress = New ProxyWS.BaseAddressType
                            .BankAddress.AddressLine1 = "AddressLine1"
                            .BankAddress.AddressLine2 = "AddressLine2"
                            .BankAddress.AddressLine3 = "AddressLine3"
                            .BankAddress.AddressLine4 = "AddressLine4"
                            .BankAddress.CountryCode = "GBR"
                            .BankAddress.PostCode = "B61 0EN"
                            .BankAreaCode = "0121"
                            .BankBranch = "Kings Heath"
                            .BankExtn = "789"
                            .BankFax = "299 2929"
                            .BankFaxCode = "0121"
                            .BankName = "Natwest"
                            .BankPhone = "822 8282"
                        End With
                    End If
                End If
            End With

            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.BindQuote(oRequest)
            With oResponse

                If eMissingData <> enumMissingData.None Then
                    ' Missing Data tests
                    SAMTest.AssertCallFailedWithErrors(oResponse, 1)
                    SAMTest.AssertErrorInvalidData(oResponse, 0, 100, "Mandatory " & eMissingData.ToString & " is missing")
                ElseIf eInvalidLookup <> enumInvalidLookup.None Then
                    ' Invalid Lookup tests
                    SAMTest.AssertCallFailedWithErrors(oResponse, 1)
                    SAMTest.AssertErrorInvalidData(oResponse, 0, nLookupError, eInvalidLookup.ToString & " is invalid")
                ElseIf eSTSBusinessError <> enumSTSBusinessError.None Then
                    ' Business Rule tests
                    SAMTest.AssertCallFailedWithErrors(oResponse, 1)
                    SAMTest.AssertErrorBusinessRule(oResponse, 0, nBusinessError)
                Else
                    ' Success Tests
                    SAMTest.AssertCallSucceeded(oResponse)
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

#Region " Success "

    Public Sub SupportMethod(ByRef r_nInsuranceFileCnt As Integer)
        Success()
        r_nInsuranceFileCnt = m_nInsuranceFileCnt
    End Sub

    Public Sub SupportMethodWithInstalments(ByRef r_nInsuranceFileCnt As Integer)
        Success_Instalments()
        r_nInsuranceFileCnt = m_nInsuranceFileCnt
    End Sub

    <Test()> _
    Public Sub Success()
        BindQuoteTest()
    End Sub

#End Region

#Region " Success With Instalments"

    <Test()> _
    Public Sub Success_Instalments()
        BindQuoteTest(blWithInstalments:=True)
    End Sub

#End Region

#Region " Missing Data "

    <Test()> _
    Public Sub InvalidData_Missing_BranchCode()
        BindQuoteTest(eMissingData:=enumMissingData.BranchCode)
    End Sub
    <Test()> _
    Public Sub InvalidData_Missing_InsuranceFileKey()
        BindQuoteTest(eMissingData:=enumMissingData.InsuranceFileKey)
    End Sub

#End Region

#Region " Invalid Lookup "

    <Test()> _
    Public Sub InvalidData_BranchCode()
        BindQuoteTest(eInvalidLookup:=enumInvalidLookup.BranchCode)
    End Sub

#End Region

#Region " STS Business Rules "

    <Test()> _
    Public Sub STSBusiness_InsFileCnt()
        BindQuoteTest(eSTSBusinessError:=enumSTSBusinessError.InvalidInsuranceFileCnt)
    End Sub
#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        BindQuoteTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        BindQuoteTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        BindQuoteTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        BindQuoteTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class

