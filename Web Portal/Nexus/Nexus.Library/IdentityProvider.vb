Imports System.Configuration
Namespace Config

	Public Class IdentityProvider : Inherits ConfigurationSection

		Private cpDefaultIdentity As ConfigurationProperty
		Private cpExpireTimeSpan As ConfigurationProperty
		Private cpIdentity As ConfigurationProperty
		Private cpcProperties As ConfigurationPropertyCollection

		Public Sub New()

			cpcProperties = New ConfigurationPropertyCollection

			cpDefaultIdentity = New ConfigurationProperty("DefaultIdentity", GetType(String), Nothing, ConfigurationPropertyOptions.IsRequired)
			cpExpireTimeSpan = New ConfigurationProperty("ExpireTimeSpan", GetType(Integer), 30)
			cpIdentity = New ConfigurationProperty("Identity", GetType(ProviderSettingsCollection), Nothing)

			cpcProperties.Add(cpDefaultIdentity)
			cpcProperties.Add(cpExpireTimeSpan)
			cpcProperties.Add(cpIdentity)

		End Sub

		Protected Overrides ReadOnly Property Properties() As System.Configuration.ConfigurationPropertyCollection
			Get
				Return cpcProperties
			End Get
		End Property

		Public ReadOnly Property Identity() As ProviderSettingsCollection
			Get
				Return CType(MyBase.Item("Identity"), ProviderSettingsCollection)
			End Get
		End Property

		Public ReadOnly Property DefaultIdentity() As String
			Get
				Return CStr(MyBase.Item(cpDefaultIdentity))
			End Get
		End Property

		Public ReadOnly Property ExpireTimeSpan() As Integer
			Get
				Return MyBase.Item(cpExpireTimeSpan)
			End Get
		End Property

	End Class

End Namespace