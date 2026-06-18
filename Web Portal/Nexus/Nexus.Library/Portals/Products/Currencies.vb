Imports System.Configuration

Namespace Config

    Public Class Currencies : Inherits ConfigurationElementCollection

        Private cpProperties As ConfigurationPropertyCollection

        Public Sub New()

            cpProperties = New ConfigurationPropertyCollection

        End Sub

        Protected Overrides ReadOnly Property Properties() As System.Configuration.ConfigurationPropertyCollection
            Get
                Return cpProperties
            End Get
        End Property

        Public Overrides ReadOnly Property CollectionType() As System.Configuration.ConfigurationElementCollectionType
            Get
                Return ConfigurationElementCollectionType.BasicMap
            End Get
        End Property

        Protected Overrides ReadOnly Property ElementName() As String
            Get
                Return "Currency"
            End Get
        End Property

        Public ReadOnly Property Currency(ByVal index As Integer) As Currency
            Get
                Return CType(MyBase.BaseGet(index), Currency)
            End Get
        End Property

        Public ReadOnly Property Currency(ByVal Code As String) As Currency
            Get
                Return MyBase.BaseGet(Code)
            End Get
        End Property

        Public Function GetKey(ByVal index As Integer) As String
            Return CStr(MyBase.BaseGetKey(index))
        End Function

        Protected Overloads Overrides Function CreateNewElement() As System.Configuration.ConfigurationElement
            Return New Currency
        End Function

        Protected Overrides Function GetElementKey(ByVal element As System.Configuration.ConfigurationElement) As Object
            Return CType(element, Currency).Code
        End Function

    End Class

End Namespace
