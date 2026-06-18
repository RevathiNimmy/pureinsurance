Imports System.Configuration

Public Class ProviderCollection : Inherits Provider.ProviderCollection

    Default Public Shadows ReadOnly Property Item(ByVal name As String) As NexusProvider.ProviderBase
        Get
            Return CType(MyBase.Item(name), NexusProvider.ProviderBase)
        End Get
    End Property

    Public Overrides Sub Add(ByVal provider As System.Configuration.Provider.ProviderBase)

        If provider Is Nothing Then
            Throw New ArgumentNullException("The provider parameter cannot be null.")
        End If

        If Not TypeOf provider Is NexusProvider.ProviderBase Then
            Throw New ArgumentException("The provider parameter must be of type NexusProvider.ProviderBase")
        End If

        MyBase.Add(provider)

    End Sub

    Public Shadows Sub CopyTo(ByVal array() As NexusProvider.ProviderBase, ByVal index As Integer)
        MyBase.CopyTo(array, index)
    End Sub

End Class