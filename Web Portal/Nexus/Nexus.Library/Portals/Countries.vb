Imports System.Configuration

Namespace Config

    Public Class Countries : Inherits ConfigurationElementCollection

        Private cpDefaultCountryCode As ConfigurationProperty
        Private cpDefaultCountryKey As ConfigurationProperty

        Private cpcProperties As ConfigurationPropertyCollection

        Public Sub New()

            cpcProperties = New ConfigurationPropertyCollection

            cpDefaultCountryCode = New ConfigurationProperty("DefaultCountryCode", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)

            cpDefaultCountryKey = New ConfigurationProperty("DefaultCountryKey", GetType(String), Nothing, _
               ConfigurationPropertyOptions.IsRequired)

            cpcProperties.Add(cpDefaultCountryCode)
            cpcProperties.Add(cpDefaultCountryKey)

        End Sub

        Protected Overrides ReadOnly Property Properties() As System.Configuration.ConfigurationPropertyCollection
            Get
                Return cpcProperties
            End Get
        End Property

        Public Overrides ReadOnly Property CollectionType() As System.Configuration.ConfigurationElementCollectionType
            Get
                Return ConfigurationElementCollectionType.BasicMap
            End Get
        End Property

        Protected Overrides ReadOnly Property ElementName() As String
            Get
                Return "Country"
            End Get
        End Property

        Public ReadOnly Property Country(ByVal index As Integer) As Country
            Get
                Return CType(MyBase.BaseGet(index), Country)
            End Get
        End Property

        Public ReadOnly Property Country(ByVal Name As String) As Country
            Get
                Return MyBase.BaseGet(Name)
            End Get
        End Property

        Public Function GetKey(ByVal index As Integer) As String
            Return CStr(MyBase.BaseGetKey(index))
        End Function

        Protected Overloads Overrides Function CreateNewElement() As System.Configuration.ConfigurationElement
            Return New Country
        End Function

        Protected Overrides Function GetElementKey(ByVal element As System.Configuration.ConfigurationElement) As Object
            Return CType(element, Country).Code
        End Function

        Public ReadOnly Property DefaultCountryCode() As String
            Get
                Return CStr(MyBase.Item(cpDefaultCountryCode))
            End Get
        End Property

        Public ReadOnly Property DefaultCountryKey() As String
            Get
                Return CStr(MyBase.Item(cpDefaultCountryKey))
            End Get
        End Property

    End Class

End Namespace