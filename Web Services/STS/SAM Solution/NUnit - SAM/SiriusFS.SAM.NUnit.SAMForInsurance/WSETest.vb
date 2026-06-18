Option Strict On

''' <summary>
''' Methods for administering WSE security tests in all other test classes.
''' </summary>
Friend NotInheritable Class WSETest

#Region "Constructors"

    Private Sub New()
        ' This class cannot be instantiated.
    End Sub

#End Region

#Region "Public Shared Methods"

    ''' <summary>
    ''' Call this method in the <c>Catch ex As SoapException</c> block.
    ''' </summary>
    ''' <param name="ex">The exception that was caught.</param>
    ''' <param name="nWSETestCaseScenario">The security test that you want to perform.</param>
    Public Shared Sub HandleException(ByVal ex As SoapException, ByVal nWSETestCaseScenario As WSETestCaseScenario)

        Select Case nWSETestCaseScenario
            Case WSETestCaseScenario.InvalidUserName, WSETestCaseScenario.InvalidPassword
                ' We are expecting an "AuthenticationException" error.
                Assert.IsTrue(IsAuthenticationException(ex), "An ""AuthenticationException"" error was expected. " & GetDiagnosticMessage(ex))
            Case WSETestCaseScenario.InvalidTaskCode
                ' We are expecting an "AuthorisationException" error.
                Assert.IsTrue(IsAuthorisationException(ex), "An ""AuthorisationException"" error was expected. " & GetDiagnosticMessage(ex))
            Case WSETestCaseScenario.MissingSecurity
                ' We are expecting a "UsernameToken not present" error.
                Assert.IsTrue(IsUsernameTokenNotPresent(ex), "A ""UsernameToken not present"" error was expected. " & GetDiagnosticMessage(ex))
            Case Else 'Security.None
                ' We are not expecting any exception.
                Assert.Fail(GetDiagnosticMessage(ex))
        End Select

    End Sub

    ''' <summary>
    ''' Call this method in the <c>Catch ex As Exception</c> block.
    ''' </summary>
    ''' <param name="ex">The exception that was caught.</param>
    Public Shared Sub HandleException(ByVal ex As Exception)

        ' We are not expecting any exception.
        Assert.Fail(GetDiagnosticMessage(ex))

    End Sub

#End Region

#Region "Private Shared Methods"

    Private Shared Function IsAuthenticationException(ByVal ex As SoapException) As Boolean

        ' This is thrown from Sirius code within the Microsoft WSE framework, so it is
        ' embedded in a SoapHeaderException. We must parse the message text to detect it.
        Return _
            ex.Message.StartsWith("Microsoft.Web.Services3.Security.SecurityFault: ", StringComparison.InvariantCulture) AndAlso _
            ex.Message.IndexOf("Sirius.Architecture.Security.AuthenticationException", StringComparison.InvariantCulture) <> -1

    End Function

    Private Shared Function IsAuthorisationException(ByVal ex As SoapException) As Boolean

        ' This is thrown from the Sirius business layer last resort exception handler,
        ' so it is encoded as a SoapException with a populated Detail node. We must
        ' parse the elements of the Detail node to detect it.
        Return _
            ex.Code.Name = "Server" AndAlso _
            ex.Detail IsNot Nothing AndAlso _
            ex.Detail.HasChildNodes AndAlso _
            ex.Detail.ChildNodes(0).Name = "ExceptionType" AndAlso _
            ex.Detail.ChildNodes(0).HasChildNodes AndAlso _
            ex.Detail.ChildNodes(0).ChildNodes(0).Value = "Sirius.Architecture.Security.AuthorisationException"

    End Function

    Private Shared Function IsUsernameTokenNotPresent(ByVal ex As SoapException) As Boolean

        ' This is thrown from the Microsoft WSE framework, so it is embedded in a
        ' SoapHeaderException. We must parse the message text to detect it.
        Return _
            ex.Message.StartsWith("Microsoft.Web.Services3.Security.SecurityFault: UsernameToken is expected but not present", StringComparison.InvariantCulture)

    End Function

    Private Shared Function GetDiagnosticMessage(ByVal ex As Exception) As String

        Dim sMessage As String = "Unexpected exception:" & vbCrLf & _
            "Type = """ & ex.GetType().FullName & """" & vbCrLf & _
            "Message = """ & ex.Message & """"

        Dim exSoap As SoapException = TryCast(ex, SoapException)
        Dim exSql As System.Data.SqlClient.SqlException = TryCast(ex, System.Data.SqlClient.SqlException)

        If exSoap IsNot Nothing Then
            sMessage &= GetDiagnosticMessage(exSoap)
        ElseIf exSql IsNot Nothing Then
            sMessage &= GetDiagnosticMessage(exSql)
        End If

        Return sMessage

    End Function

    Private Shared Function GetDiagnosticMessage(ByVal ex As SoapException) As String

        Dim sMessage As String = String.Empty

        If ex.Detail IsNot Nothing Then
            sMessage &= vbCrLf & "Detail = """ & ex.Detail.InnerXml & """"
        Else
            sMessage &= vbCrLf & "Detail = Nothing"
        End If

        Return sMessage

    End Function

    Private Shared Function GetDiagnosticMessage(ByVal ex As System.Data.SqlClient.SqlException) As String

        Dim sMessage As String = String.Empty

        If ex.Errors IsNot Nothing Then
            For iError As Integer = 0 To ex.Errors.Count - 1
                sMessage &= vbCrLf & "Error(" & iError & ") = " & GetDiagnosticMessage(ex.Errors(iError))
            Next
        Else
            sMessage &= vbCrLf & "Errors.Count = 0"
        End If

        Return sMessage

    End Function

    Private Shared Function GetDiagnosticMessage(ByVal oError As System.Data.SqlClient.SqlError) As String

        Return "<" & oError.ToString() & ">"

    End Function

#End Region

End Class
