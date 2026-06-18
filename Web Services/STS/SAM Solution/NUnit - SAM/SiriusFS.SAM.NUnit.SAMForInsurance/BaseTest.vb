Option Strict On

''' <summary>
''' Base class for all the NUnit test classes in this project.
''' </summary>
Public Class BaseTest

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

End Class
