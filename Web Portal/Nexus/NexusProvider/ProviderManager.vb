Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager

Public Class ProviderManager

    Private oDefaultProvider As NexusProvider.ProviderBase
    Private oProviderCollection As NexusProvider.ProviderCollection

    Public Sub New()

        Dim oNexusConfig As Config = CType(GetSection("NexusProvider.Config"), Config)

        oProviderCollection = New NexusProvider.ProviderCollection
        ProvidersHelper.InstantiateProviders(oNexusConfig.Providers, oProviderCollection, GetType(NexusProvider.ProviderBase))

        oDefaultProvider = oProviderCollection(oNexusConfig.DefaultProvider)

    End Sub

    Public ReadOnly Property Provider() As NexusProvider.ProviderBase
        Get
            Return oDefaultProvider
        End Get
    End Property

    Public ReadOnly Property ProviderCollection() As NexusProvider.ProviderCollection
        Get
            Return oProviderCollection
        End Get
    End Property

End Class
