Imports System.Configuration

Namespace Config

    Public Class PerilType : Inherits ConfigurationElement

        Private cpRiskCode As ConfigurationProperty
        Private cpPath As ConfigurationProperty

        Private cpcProperties As ConfigurationPropertyCollection

        Public Sub New()

            cpcProperties = New ConfigurationPropertyCollection
            cpRiskCode = New ConfigurationProperty("Code", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)
            cpPath = New ConfigurationProperty("Folder", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)

            cpcProperties.Add(cpRiskCode)
            cpcProperties.Add(cpPath)

        End Sub

#Region "Public ReadOnly Property"

        Public ReadOnly Property Code() As String
            Get
                Return CStr(MyBase.Item(cpRiskCode))
            End Get
        End Property

        Public ReadOnly Property Folder() As String
            Get
                Return CStr(MyBase.Item(cpPath))
            End Get
        End Property
        Protected Overrides ReadOnly Property Properties() As System.Configuration.ConfigurationPropertyCollection
            Get
                Return cpcProperties
            End Get
        End Property

#End Region

    End Class

End Namespace


