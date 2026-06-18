Imports System.Configuration
Imports Nexus
Namespace Config

    Public Class FileType : Inherits ConfigurationElement

        Private cpcProperties As ConfigurationPropertyCollection
        Private cpExtension As ConfigurationProperty
        Private cpDisplay As ConfigurationProperty
        Private cpCssClass As ConfigurationProperty
        Private cpContentType As ConfigurationProperty
        Private cpDocType As ConfigurationProperty
        Private cpRoles As ConfigurationProperty

        Public Sub New()

            cpcProperties = New ConfigurationPropertyCollection

            cpExtension = New ConfigurationProperty("Extension", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)

            cpDisplay = New ConfigurationProperty("Display", GetType(String), Nothing, _
               ConfigurationPropertyOptions.IsRequired)

            cpCssClass = New ConfigurationProperty("CssClass", GetType(String), Nothing, _
              ConfigurationPropertyOptions.IsRequired)

            cpContentType = New ConfigurationProperty("ContentType", GetType(String), Nothing, _
             ConfigurationPropertyOptions.IsRequired)

            cpDocType = New ConfigurationProperty("DocType", GetType(String), Nothing, _
            ConfigurationPropertyOptions.IsRequired)

            cpcProperties.Add(cpExtension)
            cpcProperties.Add(cpDisplay)
            cpcProperties.Add(cpCssClass)
            cpcProperties.Add(cpContentType)
            cpcProperties.Add(cpDocType)

        End Sub
        Protected Overrides ReadOnly Property Properties() As System.Configuration.ConfigurationPropertyCollection
            Get
                Return cpcProperties
            End Get
        End Property

        Public ReadOnly Property Extension() As String
            Get
                Return CStr(MyBase.Item(cpExtension))
            End Get
        End Property

        Public ReadOnly Property Display() As String
            Get
                Return CStr(MyBase.Item(cpDisplay))
            End Get
        End Property

        Public ReadOnly Property CssClass() As String
            Get
                Return CStr(MyBase.Item(cpCssClass))
            End Get
        End Property

        Public ReadOnly Property ContentType() As String
            Get
                Return CStr(MyBase.Item(cpContentType))
            End Get
        End Property

        Public ReadOnly Property DocType() As String
            Get
                Return CStr(MyBase.Item(cpDocType))
            End Get
        End Property
    End Class
End Namespace
