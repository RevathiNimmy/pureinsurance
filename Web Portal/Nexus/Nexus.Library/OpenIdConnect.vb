Imports System.Configuration

Namespace Config

	Public Class OpenIdConnect : Inherits ConfigurationElement

		Private cpTenantID As ConfigurationProperty
		Private cpClientID As ConfigurationProperty
		Private cpClientSecret As ConfigurationProperty
		Private cpRedirectUriChallenge As ConfigurationProperty
		Private cpAADInstance As ConfigurationProperty
		Private cpAADLogoutUrl As ConfigurationProperty

		Private cpcProperties As ConfigurationPropertyCollection

		Public Sub New()

			cpcProperties = New ConfigurationPropertyCollection

			cpTenantID = New ConfigurationProperty("TenantID", GetType(String), Nothing,
				ConfigurationPropertyOptions.IsRequired)

			cpClientID = New ConfigurationProperty("ClientID", GetType(String), Nothing,
				ConfigurationPropertyOptions.IsRequired)

			cpClientSecret = New ConfigurationProperty("ClientSecret", GetType(String), Nothing,
				ConfigurationPropertyOptions.IsRequired)

			cpRedirectUriChallenge = New ConfigurationProperty("RedirectUriChallenge", GetType(String), Nothing,
				ConfigurationPropertyOptions.IsRequired)

			cpAADInstance = New ConfigurationProperty("AADInstance", GetType(String), "https://login.microsoftonline.com")

			cpAADLogoutUrl = New ConfigurationProperty("AADLogoutUrl", GetType(String), "https://login.microsoftonline.com/common/oauth2/logout?post_logout_redirect_uri=")

			cpcProperties.Add(cpTenantID)
			cpcProperties.Add(cpClientID)
			cpcProperties.Add(cpClientSecret)
			cpcProperties.Add(cpRedirectUriChallenge)
			cpcProperties.Add(cpAADInstance)
			cpcProperties.Add(cpAADLogoutUrl)

		End Sub

		Public ReadOnly Property TenantID() As String
			Get
				Return CStr(MyBase.Item(cpTenantID))
			End Get
		End Property

		Public ReadOnly Property ClientID() As String
			Get
				Return CStr(MyBase.Item(cpClientID))
			End Get
		End Property

		Public ReadOnly Property ClientSecret() As String
			Get
				Return CStr(MyBase.Item(cpClientSecret))
			End Get
		End Property

		Public ReadOnly Property RedirectUriChallenge() As String
			Get
				Return CStr(MyBase.Item(cpRedirectUriChallenge))
			End Get
		End Property

		Public ReadOnly Property AADInstance() As String
			Get
				Return CStr(MyBase.Item(cpAADInstance))
			End Get
		End Property

		Public ReadOnly Property AADLogoutUrl() As String
			Get
				Return CStr(MyBase.Item(cpAADLogoutUrl))
			End Get
		End Property

		Protected Overrides ReadOnly Property Properties() As System.Configuration.ConfigurationPropertyCollection
			Get
				Return cpcProperties
			End Get
		End Property

	End Class

End Namespace
