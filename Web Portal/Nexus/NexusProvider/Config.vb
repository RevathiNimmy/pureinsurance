Imports System.Configuration

Public Class Config : Inherits ConfigurationSection

    Private cpDefaultProvider As ConfigurationProperty
    Private cpProviders As ConfigurationProperty

    Private cpcProperties As ConfigurationPropertyCollection

    Public Sub New()

        cpcProperties = New ConfigurationPropertyCollection

        cpDefaultProvider = New ConfigurationProperty("DefaultProvider", GetType(String), Nothing)
        cpProviders = New ConfigurationProperty("Providers", GetType(ProviderSettingsCollection), Nothing)

        cpcProperties.Add(cpDefaultProvider)
        cpcProperties.Add(cpProviders)

    End Sub

    Protected Overrides ReadOnly Property Properties() As System.Configuration.ConfigurationPropertyCollection
        Get
            Return cpcProperties
        End Get
    End Property

    Public ReadOnly Property Providers() As ProviderSettingsCollection
        Get
            Return CType(MyBase.Item("Providers"), ProviderSettingsCollection)
        End Get
    End Property

    Public ReadOnly Property DefaultProvider() As String
        Get
            Return CStr(MyBase.Item(cpDefaultProvider))
        End Get
    End Property

End Class