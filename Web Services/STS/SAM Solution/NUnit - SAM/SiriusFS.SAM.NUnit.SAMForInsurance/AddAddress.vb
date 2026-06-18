Imports NUnit.Framework

<TestFixture()> _
Public Class AddAddress
    Inherits BaseTest

#Region "Private Declarations"

    Private m_nAddressCnt As Integer
    Private m_oTestData As New TestData

    Private Enum enumMissingData
        AddressLine1
        'PostCode
        CountryCode
        None
    End Enum

    Private Enum enumSTSBusinessRule
        None
        BOFailed
    End Enum

#End Region

#Region "Private Methods"

    Private Sub AddAddressTest( _
        Optional ByVal eMissingData As enumMissingData = enumMissingData.None, _
        Optional ByVal eSTSBusinessRule As enumSTSBusinessRule = enumSTSBusinessRule.None, _
        Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.AddAddressRequestType
        Dim oResponse As ProxyWS.AddAddressResponseType
        Dim nSAMErrorCodeExpected As Integer

        Try
            ' Set the specific condition on the input class

            With oRequest
                If eMissingData <> enumMissingData.AddressLine1 Then
                    .AddressLine1 = m_oTestData.AddressLine1
                End If
                .AddressLine2 = m_oTestData.AddressLine2
                .AddressLine3 = m_oTestData.AddressLine3
                .AddressLine4 = m_oTestData.AddressLine4
                .AddressTypeCode = m_oTestData.AddressTypeCode
                If eMissingData <> enumMissingData.CountryCode Then
                    .CountryCode = m_oTestData.CountryCode
                End If
                'If eMissingData <> enumMissingData.PostCode Then
                If eSTSBusinessRule = enumSTSBusinessRule.BOFailed Then
                    ' Create a string that is too long for the DB table field
                    .PostCode = New String("x", 100)
                    nSAMErrorCodeExpected = 252
                Else
                    ' Ensure the countrycode requires a postcode for this test
                    .PostCode = m_oTestData.PostCode
                End If
                'End If
            End With

            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.AddAddress(oRequest)

            With oResponse
                If eMissingData <> enumMissingData.None Then
                    ' Missing Data tests
                    SAMTest.AssertCallFailedWithErrors(oResponse, 1)
                    SAMTest.AssertErrorInvalidData(oResponse, 0, 100, "Mandatory " & eMissingData.ToString & " is missing")
                ElseIf eSTSBusinessRule <> enumSTSBusinessRule.None Then
                    ' STS Business Rule test
                    SAMTest.AssertCallFailedWithErrors(oResponse, 1)
                    SAMTest.AssertErrorBusinessRule(oResponse, 0, nSAMErrorCodeExpected)
                Else
                    ' Success Tests
                    SAMTest.AssertCallSucceeded(oResponse)
                    Console.WriteLine("AddAddress.Success: Address Count = " & CStr(oResponse.AddressKey))
                    Assert.Greater(oResponse.AddressKey, 0, "Returned Party Count invalid.")
                    m_nAddressCnt = .AddressKey
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

#Region "Success"

    Public Sub SupportMethod(ByRef r_nAddressCnt As Integer)

        AddAddressTest()
        r_nAddressCnt = m_nAddressCnt

    End Sub

    <Test()> _
    Public Sub Success()
        AddAddressTest()
    End Sub

#End Region

#Region "Missing Data"

    <Test()> _
    Public Sub InvalidData_Missing_AddressLine1()
        AddAddressTest(enumMissingData.AddressLine1)
    End Sub

    <Test()> _
    Public Sub InvalidData_Missing_CountryCode()
        AddAddressTest(enumMissingData.CountryCode)
    End Sub

#End Region

#Region "STS Business Rules"

    <Test()> _
    Public Sub STSRule_BOFailed()
        AddAddressTest(enumMissingData.None, enumSTSBusinessRule.BOFailed)
    End Sub

#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        AddAddressTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        AddAddressTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        AddAddressTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        AddAddressTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class
