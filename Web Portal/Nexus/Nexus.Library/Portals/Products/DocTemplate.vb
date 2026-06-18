Imports System.Configuration

Namespace Config

    Public Class DocTemplate : Inherits ConfigurationElement

        Private cpName As ConfigurationProperty
        Private cpFileName As ConfigurationProperty
        Private cpTagFileName As ConfigurationProperty
        Private cpTagFileFormat As ConfigurationProperty
        Private cpCode As ConfigurationProperty

        Private cpcProperties As ConfigurationPropertyCollection

        Public Sub New()

            cpcProperties = New ConfigurationPropertyCollection

            cpName = New ConfigurationProperty("Name", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)
            cpFileName = New ConfigurationProperty("FileName", GetType(String), Nothing, _
               ConfigurationPropertyOptions.IsRequired)
            cpTagFileName = New ConfigurationProperty("TagFileName", GetType(String), Nothing)
            cpTagFileFormat = New ConfigurationProperty("TagFileFormat", GetType(String), Nothing)
            cpCode = New ConfigurationProperty("Code", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)

            cpcProperties.Add(cpName)
            cpcProperties.Add(cpFileName)
            cpcProperties.Add(cpTagFileName)
            cpcProperties.Add(cpTagFileFormat)
            cpcProperties.Add(cpCode)

        End Sub

        Public ReadOnly Property Name() As String
            Get
                Return CStr(MyBase.Item(cpName))
            End Get
        End Property
        Public ReadOnly Property FileName() As String
            Get
                Return CStr(MyBase.Item(cpFileName))
            End Get
        End Property
        Public ReadOnly Property TagFileName() As String
            Get
                Return CStr(MyBase.Item(cpTagFileName))
            End Get
        End Property
        Public ReadOnly Property TagFileFormat() As String
            Get
                Return CStr(MyBase.Item(cpTagFileFormat))
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