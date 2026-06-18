Module MainModule

    Private m_bIsHelp As Boolean = False
    Private m_sSharePointOnlineURL As String = String.Empty
    Private m_sSharePointLibrary As String = String.Empty
    Private m_sUserId As String = String.Empty
    Private m_sPassword As String = String.Empty
    Private m_sClientId As String = String.Empty
    Private m_sTenantId As String = String.Empty
    Private m_cArgs As System.Collections.ObjectModel.Collection(Of String) = Nothing

    Sub Main()
        Try

            ProcessCommandLine()

            Dim oSharePointValidate = New BusinessSharepointOnlineValidate()
            Dim sResponse As String = String.Empty
            oSharePointValidate.ValidateSharePointURL(m_sSharePointOnlineURL,
                                                      m_sSharePointLibrary,
                                                      m_sUserId, m_sPassword, sResponse, sAppClientId:=m_sClientId, sSharepointTenantId:=m_sTenantId)
            Console.Write(sResponse)
        Catch ex As Exception
            Console.Write(ex.Message)
        End Try

    End Sub

    ''' <summary>
    ''' Process command line for flags and commands
    ''' </summary>
    Private Sub ProcessCommandLine()

        m_cArgs = New System.Collections.ObjectModel.Collection(Of String)()

        For Each sArg As String In My.Application.CommandLineArgs

            Select Case sArg.ToUpper.Split("=")(0)
                Case "URL"
                    m_sSharePointOnlineURL = sArg.Split("=")(1)
                Case "LIBRARY"
                    m_sSharePointLibrary = sArg.Split("=")(1)
                Case "USERID"
                    m_sUserId = sArg.Split("=")(1)
                Case "PASSWORD"
                    m_sPassword = sArg.Split("=")(1)
                Case "APPCLIENTID"
                    m_sClientId = sArg.Split("=")(1)
                Case "SHAREPOINTTENANTID"
                    m_sTenantId = sArg.Split("=")(1)
            End Select

        Next

    End Sub

End Module
