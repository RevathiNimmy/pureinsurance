Imports System.Configuration

Namespace Config

    Public Class PaymentProviderValue : Inherits ConfigurationElement

        Private cpKey As ConfigurationProperty
        Private cpValue As ConfigurationProperty

        Private cpcProperties As ConfigurationPropertyCollection

        Public Sub New()

            cpcProperties = New ConfigurationPropertyCollection

            cpKey = New ConfigurationProperty("Key", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)

            cpValue = New ConfigurationProperty("Value", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)

            cpcProperties.Add(cpKey)
            cpcProperties.Add(cpValue)

        End Sub

        Public ReadOnly Property Key() As String
            Get
                Return CStr(MyBase.Item(cpKey))
            End Get
        End Property

        Public ReadOnly Property Value() As String
            Get
                Return CStr(MyBase.Item(cpValue))
            End Get
        End Property

        Protected Overrides ReadOnly Property Properties() As System.Configuration.ConfigurationPropertyCollection
            Get
                Return cpcProperties
            End Get
        End Property

    End Class

End Namespace