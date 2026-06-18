Imports System.Net.Http
Imports System.Net
Imports System.Text
Imports System.Threading.Tasks
Imports Newtonsoft.Json

Public Class ApiClient
    Private ReadOnly _httpClient As HttpClient
    Private ReadOnly _baseUrl As String
    Private ReadOnly _tokenManager As TokenManager
    Private _authToken As String

    Public Sub New(baseUrl As String)
        _baseUrl = baseUrl.TrimEnd("/"c)
        _httpClient = New HttpClient()
        _tokenManager = New TokenManager()
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        ServicePointManager.ServerCertificateValidationCallback = Function(sender, cert, chain, errors) True
    End Sub

    Public Async Function PostAsync(Of T)(endpoint As String, data As Object) As Task(Of T)
        Await EnsureValidTokenAsync()
        Dim settings As New JsonSerializerSettings With {
            .NullValueHandling = NullValueHandling.Ignore
        }
        Dim json = JsonConvert.SerializeObject(data, settings)
        Dim content = New StringContent(json, Encoding.UTF8, "application/json")
        Dim url = $"{_baseUrl}/{endpoint.TrimStart("/"c)}"
        Using response = Await _httpClient.PostAsync(url, content).ConfigureAwait(False)
            response.EnsureSuccessStatusCode()
            Dim responseContent = Await response.Content.ReadAsStringAsync().ConfigureAwait(False)
            Dim deserializeSettings As New JsonSerializerSettings With {
                .NullValueHandling = NullValueHandling.Include,
                .MissingMemberHandling = MissingMemberHandling.Ignore
            }
            Return JsonConvert.DeserializeObject(Of T)(responseContent, deserializeSettings)
        End Using
    End Function

    Private Async Function EnsureValidTokenAsync() As Task
        Dim validToken = Await _tokenManager.GetValidTokenAsync().ConfigureAwait(False)
        If validToken <> _authToken Then
            _authToken = validToken
            _httpClient.DefaultRequestHeaders.Authorization = New System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken)
        End If
    End Function

    Public Sub Dispose()
        _httpClient?.Dispose()
        _tokenManager?.Dispose()
    End Sub
End Class
