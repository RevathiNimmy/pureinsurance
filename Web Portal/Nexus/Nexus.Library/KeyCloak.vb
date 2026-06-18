Imports System.Configuration

Namespace Config

	Public Class KeyCloak : Inherits ConfigurationElement

		Private cpClientID As ConfigurationProperty
		Private cpClientSecret As ConfigurationProperty
		Private cpRedirectUri As ConfigurationProperty
		Private cpAuthority As ConfigurationProperty
		Private cpLogoutRedirectUri As ConfigurationProperty

		Private cpcProperties As ConfigurationPropertyCollection

		Public Sub New()

			cpcProperties = New ConfigurationPropertyCollection

			cpClientID = New ConfigurationProperty("ClientID", GetType(String), Nothing,
				ConfigurationPropertyOptions.IsRequired)

			cpClientSecret = New ConfigurationProperty("ClientSecret", GetType(String), Nothing,
				ConfigurationPropertyOptions.IsRequired)

			cpRedirectUri = New ConfigurationProperty("RedirectUri", GetType(String), Nothing,
				ConfigurationPropertyOptions.IsRequired)

			cpAuthority = New ConfigurationProperty("Authority", GetType(String), Nothing,
				ConfigurationPropertyOptions.IsRequired)

			cpLogoutRedirectUri = New ConfigurationProperty("LogoutRedirectUri", GetType(String), Nothing,
				ConfigurationPropertyOptions.IsRequired)

			cpcProperties.Add(cpClientID)
			cpcProperties.Add(cpClientSecret)
			cpcProperties.Add(cpRedirectUri)
			cpcProperties.Add(cpAuthority)
			cpcProperties.Add(cpLogoutRedirectUri)

		End Sub

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

		Public ReadOnly Property RedirectUri() As String
			Get
				Return CStr(MyBase.Item(cpRedirectUri))
			End Get
		End Property

		Public ReadOnly Property Authority() As String
			Get
				Return CStr(MyBase.Item(cpAuthority))
			End Get
		End Property

		Public ReadOnly Property LogoutRedirectUri() As String
			Get
				Return CStr(MyBase.Item(cpLogoutRedirectUri))
			End Get
		End Property

		Protected Overrides ReadOnly Property Properties() As System.Configuration.ConfigurationPropertyCollection
			Get
				Return cpcProperties
			End Get
		End Property

	End Class

End Namespace
