Imports System.Configuration

Namespace Config
    Public Class FormatStrings : Inherits ConfigurationElementCollection
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
                Return "FormatString"
            End Get
        End Property

        Public ReadOnly Property FormatString(ByVal DataType As String) As FormatString
            Get
                Return CType(MyBase.BaseGet(DataType), FormatString)
            End Get
        End Property


        Protected Overloads Overrides Function CreateNewElement() As System.Configuration.ConfigurationElement
            Return New FormatString
        End Function

        Protected Overrides Function GetElementKey(ByVal element As System.Configuration.ConfigurationElement) As Object
            Return CType(element, FormatString).DataType
        End Function
    End Class

End Namespace
