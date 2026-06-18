Imports System.Configuration

Namespace Config

    Public Class Currency : Inherits ConfigurationElement

        Private cpCode As ConfigurationProperty
        Private cpDisplay As ConfigurationProperty
        Private cpSymbol As ConfigurationProperty
        Private cpFormatString As ConfigurationProperty

        Private cpcProperties As ConfigurationPropertyCollection

        Public Sub New()

            cpcProperties = New ConfigurationPropertyCollection

            cpCode = New ConfigurationProperty("Code", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)

            cpDisplay = New ConfigurationProperty("Display", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)

            cpSymbol = New ConfigurationProperty("Symbol", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)

            cpFormatString = New ConfigurationProperty("FormatString", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)

            cpcProperties.Add(cpCode)
            cpcProperties.Add(cpDisplay)
            cpcProperties.Add(cpSymbol)
            cpcProperties.Add(cpFormatString)

        End Sub

        Public ReadOnly Property Code() As String
            Get
                Return CStr(MyBase.Item(cpCode))
            End Get
        End Property

        Public ReadOnly Property Display() As String
            Get
                Return CStr(MyBase.Item(cpDisplay))
            End Get
        End Property

        Public ReadOnly Property Symbol() As String
            Get
                Return CStr(MyBase.Item(cpSymbol))
            End Get
        End Property

        Public ReadOnly Property FormatString() As String
            Get
                Return CStr(MyBase.Item(cpFormatString))
            End Get
        End Property

        Protected Overrides ReadOnly Property Properties() As System.Configuration.ConfigurationPropertyCollection
            Get
                Return cpcProperties
            End Get
        End Property

    End Class

End Namespace
