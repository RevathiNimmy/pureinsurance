Imports System.Configuration
Imports Nexus

Namespace Config

    Public Class Report : Inherits ConfigurationElement

        Private cpcProperties As ConfigurationPropertyCollection
        Private cpName As ConfigurationProperty
        Private cpDisplay As ConfigurationProperty
        Private cpControl As ConfigurationProperty

        Public Sub New()

            cpcProperties = New ConfigurationPropertyCollection

            cpName = New ConfigurationProperty("Name", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)

            cpDisplay = New ConfigurationProperty("Display", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)

            cpControl = New ConfigurationProperty("Control", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)

            cpcProperties.Add(cpName)
            cpcProperties.Add(cpDisplay)
            cpcProperties.Add(cpControl)

        End Sub

        Protected Overrides ReadOnly Property Properties() As System.Configuration.ConfigurationPropertyCollection
            Get
                Return cpcProperties
            End Get
        End Property

        Public ReadOnly Property Name() As String
            Get
                Return CStr(MyBase.Item(cpName))
            End Get
        End Property

        Public ReadOnly Property Display() As String
            Get
                Return CStr(MyBase.Item(cpDisplay))
            End Get
        End Property

        Public ReadOnly Property Control() As String
            Get
                Return CStr(MyBase.Item(cpControl))
            End Get
        End Property

    End Class

End Namespace
