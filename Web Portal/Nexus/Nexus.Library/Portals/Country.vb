Imports System.Configuration

Namespace Config

    Public Class Country : Inherits ConfigurationElement

        Private cpName As ConfigurationProperty
        Private cpCode As ConfigurationProperty

        Private cpcProperties As ConfigurationPropertyCollection

        Public Sub New()

            cpcProperties = New ConfigurationPropertyCollection

            cpName = New ConfigurationProperty("Name", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)

            cpCode = New ConfigurationProperty("Code", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)

            cpcProperties.Add(cpName)
            cpcProperties.Add(cpCode)

        End Sub

        Public ReadOnly Property Name() As String
            Get
                Return CStr(MyBase.Item(cpName))
            End Get
        End Property

        Public ReadOnly Property Code() As String
            Get
                Return CStr(MyBase.Item(cpCode))
            End Get
        End Property

        Protected Overrides ReadOnly Property Properties() As System.Configuration.ConfigurationPropertyCollection
            Get
                Return cpcProperties
            End Get
        End Property

    End Class

End Namespace