Imports System.Configuration
Imports Nexus
Namespace Config

    Public Class Task : Inherits ConfigurationElement

        Private cpcProperties As ConfigurationPropertyCollection
        Private cpTaskName As ConfigurationProperty
        Private cpRoles As ConfigurationProperty

        Public Sub New()

            cpcProperties = New ConfigurationPropertyCollection

            cpTaskName = New ConfigurationProperty("Name", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)

            cpRoles = New ConfigurationProperty("Role", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)
            cpcProperties.Add(cpTaskName)
            cpcProperties.Add(cpRoles)
        End Sub
        Protected Overrides ReadOnly Property Properties() As System.Configuration.ConfigurationPropertyCollection
            Get
                Return cpcProperties
            End Get
        End Property

        Public ReadOnly Property Name() As String
            Get
                Return CStr(MyBase.Item(cpTaskName))
            End Get
        End Property

        Public ReadOnly Property Role() As String
            Get
                Return CStr(MyBase.Item(cpRoles))
            End Get
        End Property

    End Class
End Namespace
