Imports System.Configuration
Imports Nexus
Namespace Config


    Public Class FormatString : Inherits ConfigurationElement
        Private cpcProperties As ConfigurationPropertyCollection
        Private cpDataType As ConfigurationProperty
        Private cpDataFormatString As ConfigurationProperty
        Private cpHeaderStyleCssClass As ConfigurationProperty
        Private cpItemStyleCssClass As ConfigurationProperty

        Public Sub New()

            cpcProperties = New ConfigurationPropertyCollection

            cpDataType = New ConfigurationProperty("DataType", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)

            cpDataFormatString = New ConfigurationProperty("DataFormatString", GetType(String), Nothing, _
               ConfigurationPropertyOptions.IsRequired)

            cpHeaderStyleCssClass = New ConfigurationProperty("HeaderStyleCssClass", GetType(String), Nothing, _
              ConfigurationPropertyOptions.IsRequired)

            cpItemStyleCssClass = New ConfigurationProperty("ItemStyleCssClass", GetType(String), Nothing, _
              ConfigurationPropertyOptions.IsRequired)

            cpcProperties.Add(cpDataType)
            cpcProperties.Add(cpDataFormatString)
            cpcProperties.Add(cpHeaderStyleCssClass)
            cpcProperties.Add(cpItemStyleCssClass)

        End Sub
        Protected Overrides ReadOnly Property Properties() As System.Configuration.ConfigurationPropertyCollection
            Get
                Return cpcProperties
            End Get
        End Property

        Public ReadOnly Property DataType() As String
            Get
                Return CStr(MyBase.Item(cpDataType))
            End Get
        End Property

        Public ReadOnly Property DataFormatString() As String
            Get
                Return CStr(MyBase.Item(cpDataFormatString))
            End Get
        End Property

        Public ReadOnly Property HeaderStyleCssClass() As String
            Get
                Return CStr(MyBase.Item(cpHeaderStyleCssClass))
            End Get
        End Property
        Public ReadOnly Property ItemStyleCssClass() As String
            Get
                Return CStr(MyBase.Item(cpItemStyleCssClass))
            End Get
        End Property
    End Class
End Namespace
