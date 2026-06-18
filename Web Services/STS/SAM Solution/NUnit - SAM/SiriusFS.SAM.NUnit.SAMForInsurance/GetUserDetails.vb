Option Strict On


<TestFixture()> _
Public Class GetUserDetails

#Region "Protected Constants"

    Protected Const kdtEmptyDateTime As Date = #12:00:00 AM#
    Protected Shared ReadOnly kanEmptyTimeStamp As Byte() = {0, 0, 0, 0, 0, 0, 0, 0}

#End Region

#Region "Protected Fields"

    Protected oProxy As ProxyWS.SAMForInsuranceWse

#End Region

#Region "Constructors"

    Public Sub New()
        oProxy = New ProxyWS.SAMForInsuranceWse
        oProxy.Timeout = 1000000000
    End Sub

#End Region

#Region "Protected Methods"

    ''' <summary>
    ''' Call this method immediately before calling the actual web method.
    ''' </summary>
    ''' <param name="nWSETestCaseScenario">The security test that you want to perform.</param>
    Protected Sub SetWSETestCaseScenario(ByVal nWSETestCaseScenario As WSETestCaseScenario)

        Select Case nWSETestCaseScenario
            Case WSETestCaseScenario.None
                ' Set to valid user who has all SAM access rights.
                SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "sirius")
            Case WSETestCaseScenario.InvalidUserName
                ' Set obviously non-existent user name.
                SAMSecurity.SetSiriusClientCredential(oProxy, "3ycbwcfthg34789r3473", "sirius")
            Case WSETestCaseScenario.InvalidPassword
                ' Set invalid password.
                SAMSecurity.SetSiriusClientCredential(oProxy, "sirius", "w34cbtrw344w5w56cbtr")
            Case WSETestCaseScenario.InvalidTaskCode
                ' Set to valid user who has none of the SAM access rights.
                SAMSecurity.SetSiriusClientCredential(oProxy, "testsirius", "sirius")
            Case WSETestCaseScenario.MissingSecurity
                ' Clear previous credentials.
                oProxy = New ProxyWS.SAMForInsuranceWse
                oProxy.Timeout = 1000000000
        End Select

    End Sub

#End Region

#Region "Test Case Enumeration"

    Private Enum TestCaseScenario

        ' Non Error Scenarios
        None

    End Enum

#End Region

#Region "Main Test Method"

    Private Sub GetUserDetailsTest( _
      Optional ByVal TestCase As TestCaseScenario = TestCaseScenario.None, _
      Optional ByVal nWSETestCaseScenario As WSETestCaseScenario = WSETestCaseScenario.None)

        Dim oRequest As New ProxyWS.GetUserDetailsRequestType
        Dim oResponse As ProxyWS.GetUserDetailsResponseType

        Try

            SetWSETestCaseScenario(nWSETestCaseScenario)
            oResponse = oProxy.GetUserDetails(oRequest)

            With oResponse

                ' all these test cases should work without error
                If TestCase = TestCaseScenario.None Then
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

#Region "NUNIT Success Test Case"

    <Test()> _
    Public Sub Success()
        GetUserDetailsTest()
    End Sub

#End Region

#Region "WSE Security"

    <Test()> _
    Public Sub WSESecurity_MissingSecurity()
        GetUserDetailsTest(nWSETestCaseScenario:=WSETestCaseScenario.MissingSecurity)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidUserName()
        GetUserDetailsTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidUserName)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidPassword()
        GetUserDetailsTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidPassword)
    End Sub

    <Test()> _
    Public Sub WSESecurity_InvalidTaskCode()
        GetUserDetailsTest(nWSETestCaseScenario:=WSETestCaseScenario.InvalidTaskCode)
    End Sub

#End Region

End Class
