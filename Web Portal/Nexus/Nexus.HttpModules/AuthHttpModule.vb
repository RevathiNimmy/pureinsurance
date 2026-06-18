Imports Microsoft.IdentityModel.Tokens
Imports Newtonsoft.Json
Imports System.IdentityModel.Tokens.Jwt
Imports System.Security.Principal
Imports System.Threading
Imports System.Configuration.ConfigurationManager
Imports System.Net

Public Class AuthHttpModule : Implements IHttpModule

    Private Const Realm As String = "Pure-Insurance-Main"

    Private Const JWT_HEADER As String = "X-Amzn-Oidc-Data"
    Private Const JWT_Accesstoken As String = "X-Amzn-Oidc-Accesstoken"
    Private Const JWT_HealthChecker As String = "ELB-HealthChecker"

    Public Sub Init(ByVal context As HttpApplication) Implements IHttpModule.Init
        AddHandler context.AuthenticateRequest, AddressOf OnApplicationAuthenticateRequest
        AddHandler context.EndRequest, AddressOf OnApplicationEndRequest
    End Sub

    Public Sub Dispose() Implements IHttpModule.Dispose
        ' nothing to do
    End Sub

    Private Shared Sub OnApplicationAuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)

        Try

            ' The load balancer adds the following HTTP headers
            ' x-amzn-oidc-accesstoken = The access token from the token endpoint, in plain text.
            ' x-amzn-oidc-identity = The subject field (sub) from the user info endpoint, in plain text.
            ' x-amzn-oidc-data = The User claims, in JSON web tokens (JWT) format.


            Dim request = HttpContext.Current.Request

            If Not request.Headers.ToString.Contains(JWT_HealthChecker) Then
                If Not request.Headers.ToString.Contains(JWT_HEADER) Then
                    HttpContext.Current.Response.StatusCode = HttpStatusCode.Unauthorized
                Else
                    Dim authHeader As String = request.Headers(JWT_Accesstoken)
                    Dim securityKeys = GetSecurityKeysAsync()

                    Dim tokenValidationParameters As TokenValidationParameters = New TokenValidationParameters With {
                            .ValidateIssuer = True,
                            .ValidIssuer = AppSettings("AuthDomain").ToString,
                            .ValidateAudience = False,
                            .ValidateLifetime = True,
                            .NameClaimType = "preferred_username",
                            .IssuerSigningKeyResolver = Function(s, securityToken, identifier, parameters) securityKeys
                            }

                    Dim validatedToken As Microsoft.IdentityModel.Tokens.SecurityToken
                    Dim handler As JwtSecurityTokenHandler = New JwtSecurityTokenHandler()

                    validatedToken = Nothing
                    Dim jwtSecurityToken As JwtSecurityToken = handler.ReadJwtToken(authHeader)

                    Dim principal = handler.ValidateToken(jwtSecurityToken.RawData, tokenValidationParameters, validatedToken)


                    SetPrincipal(New GenericPrincipal(principal.Identity, Nothing))

                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Shared Sub OnApplicationEndRequest(ByVal sender As Object, ByVal e As EventArgs)
        Dim response = HttpContext.Current.Response

        If response.StatusCode = HttpStatusCode.Unauthorized Then
            response.Headers.Add("WWW-Authenticate", String.Format("Basic realm=""{0}""", Realm))
        End If
    End Sub

    Private Shared Function GetSecurityKeysAsync() As IEnumerable(Of SecurityKey)
        Dim securityKeys As String = Environment.GetEnvironmentVariable(AppSettings("KeyCloakSecurityKey").ToString)
        Dim securityKey = JsonConvert.DeserializeObject(Of List(Of JsonWebKey))(securityKeys)

        Return securityKey
    End Function
    Private Shared Sub SetPrincipal(ByVal principal As IPrincipal)
        Thread.CurrentPrincipal = principal

        If HttpContext.Current IsNot Nothing Then
            HttpContext.Current.User = principal
        End If
    End Sub
End Class
