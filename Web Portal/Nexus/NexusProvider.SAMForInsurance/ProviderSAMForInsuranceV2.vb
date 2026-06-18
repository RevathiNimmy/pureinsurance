Imports System.Configuration
Imports System.Web.HttpContext
Imports Nexus.Library.Config
Imports SSP.PureInsuranceRestAPIHandler
Partial Public Class ProviderSAMForInsuranceV2 : Inherits NexusProvider.ProviderBase

    Private Function GetApiTokendetails() As TokenModel
        Dim apiTokenDetails As New TokenModel()
        If Current.Session IsNot Nothing AndAlso Current.Session("access_token") IsNot Nothing Then
            ' For KeyCloak ClientID
            Dim providerSection As IdentityProvider = ConfigurationManager.GetSection("IdentityProvider")
            Dim clientId As String = providerSection.Identity("KeyCloak").Parameters("ClientID")
            Dim clientSecret As String = providerSection.Identity("KeyCloak").Parameters("ClientSecret")

            '// Or for SSO ClientID
            'String ssoClientId = providerSection.Identity["SSO"].Parameters["ClientID"];
            Dim accessToken As String = Current.Session("access_token")
            Dim refreshToken As String = Current.Session("refresh_token")
            Dim expiresIn As DateTime = Current.Session("expires_at")
            Dim tokenUrl As String = Current.Session("tokenUrl")
            Dim apiBaseUrl As String = ConfigurationManager.AppSettings("RestAPIUrl")
            With apiTokenDetails
                .AccessToken = accessToken
                .RefreshToken = refreshToken
                .ApiBaseUrl = apiBaseUrl
                .AccessTokenExpiry = expiresIn
                .TokenUrl = tokenUrl
                .ClientId = clientId
                .ClientSecret = clientSecret
            End With

            ' ApiClient will invoke this automatically after any token refresh,
            ' so SyncTokenToSession() does NOT need to be called at every API call site.
            SSP.PureInsuranceRestAPIHandler.ApiClient.OnTokenRefreshed =
                Sub(refreshedModel As SSP.PureInsuranceRestAPIHandler.TokenModel)
                    SyncTokenToSession()
                End Sub
        End If
        Return apiTokenDetails
    End Function

     ''' <summary>
    ''' back to Session so the next call does not overwrite it with stale values.
    ''' </summary>
    Private Sub SyncTokenToSession()
        If SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel IsNot Nothing AndAlso
            Not String.IsNullOrEmpty(SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel.AccessToken) Then
            Current.Session("access_token") = SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel.AccessToken
            Current.Session("refresh_token") = SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel.RefreshToken
            Current.Session("expires_at") = SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel.AccessTokenExpiry.ToString("o")
        End If
    End Sub
End Class
