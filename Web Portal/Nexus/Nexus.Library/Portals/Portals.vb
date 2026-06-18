Imports System.Configuration

Namespace Config

    Public Class Portals : Inherits ConfigurationElementCollection

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
                Return "Portal"
            End Get
        End Property

        Public ReadOnly Property Portal(ByVal Name As String) As Portal
            Get
                Return MyBase.BaseGet(Name)
            End Get
        End Property

        Public Function GetKey(ByVal index As Integer) As String
            Return CStr(MyBase.BaseGetKey(index))
        End Function

        Protected Overloads Overrides Function CreateNewElement() As System.Configuration.ConfigurationElement
            Return New Portal
        End Function

        Protected Overrides Function GetElementKey(ByVal element As System.Configuration.ConfigurationElement) As Object
            Return CType(element, Portal).ID
        End Function

    End Class

End Namespace
