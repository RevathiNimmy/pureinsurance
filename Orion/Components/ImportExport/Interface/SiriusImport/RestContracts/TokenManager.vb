Imports System.Configuration
Imports System.Net.Http
Imports System.Text
Imports System.Threading.Tasks
Imports Newtonsoft.Json

Public Class TokenManager
    Private ReadOnly _clientId As String
    Private ReadOnly _clientSecret As String
    Private ReadOnly _tokenUrl As String
    Private ReadOnly _httpClient As HttpClient
    Private _currentToken As TokenResponse
    Private _tokenExpiry As DateTime

    Public Sub New()
        _clientId = ConfigurationManager.AppSettings("ClientId")
        _clientSecret = ConfigurationManager.AppSettings("ClientSecret")
        _tokenUrl = ConfigurationManager.AppSettings("TokenUrl")
        _httpClient = New HttpClient()
    End Sub

    Public Async Function GetValidTokenAsync() As Task(Of String)
        If _currentToken Is Nothing OrElse DateTime.UtcNow >= _tokenExpiry Then
            If _currentToken IsNot Nothing AndAlso Not String.IsNullOrEmpty(_currentToken.refresh_token) Then
                _currentToken = Await RefreshTokenAsync(_currentToken.refresh_token).ConfigureAwait(False)
            Else
                Dim accessToken = Await GetTokenAsync().ConfigureAwait(False)
                _currentToken = New TokenResponse With {.access_token = accessToken}
            End If
            _tokenExpiry = DateTime.UtcNow.AddSeconds(If(_currentToken.expires_in > 0, _currentToken.expires_in - 60, 3540))
        End If
        Return _currentToken.access_token
    End Function

    Public Async Function GetTokenAsync() As Task(Of String)
        Using client As New HttpClient()
            Dim form As New Dictionary(Of String, String) From {
                {"grant_type", "client_credentials"},
                {"client_id", _clientId},
                {"client_secret", _clientSecret},
                {"scope", "openid"}
            }
            Using content As New FormUrlEncodedContent(form)
                Dim response As HttpResponseMessage = Await client.PostAsync(_tokenUrl, content).ConfigureAwait(False)
                Dim responseBody As String = Await response.Content.ReadAsStringAsync().ConfigureAwait(False)
                If Not response.IsSuccessStatusCode Then
                    Throw New Exception($"Token request failed: {response.StatusCode}{Environment.NewLine}{responseBody}")
                End If
                Return JsonConvert.DeserializeObject(Of TokenResponse)(responseBody).access_token
            End Using
        End Using
    End Function

    Private Async Function RefreshTokenAsync(refreshToken As String) As Task(Of TokenResponse)
        Dim requestBody = $"grant_type=refresh_token&client_id={Uri.EscapeDataString(_clientId)}&client_secret={Uri.EscapeDataString(_clientSecret)}&refresh_token={Uri.EscapeDataString(refreshToken)}"
        Dim content = New StringContent(requestBody, Encoding.UTF8, "application/x-www-form-urlencoded")
        Dim response = Await _httpClient.PostAsync(_tokenUrl, content).ConfigureAwait(False)
        response.EnsureSuccessStatusCode()
        Dim responseContent = Await response.Content.ReadAsStringAsync().ConfigureAwait(False)
        Return JsonConvert.DeserializeObject(Of TokenResponse)(responseContent)
    End Function

    Public Sub Dispose()
        _httpClient?.Dispose()
    End Sub
End Class
