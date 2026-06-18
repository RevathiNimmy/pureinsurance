Imports System.Configuration

Namespace Config
    Public Class FileTypes : Inherits ConfigurationElementCollection
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
                Return "FileType"
            End Get
        End Property
        Public ReadOnly Property FileType(ByVal index As Integer) As FileType
            Get
                Return CType(MyBase.BaseGet(index), FileType)
            End Get
        End Property

        Public ReadOnly Property FileType(ByVal DocType As String) As FileType
            Get
                Return CType(MyBase.BaseGet(DocType), FileType)
            End Get
        End Property
        Public Function GetKey(ByVal index As Integer) As String
            Return CStr(MyBase.BaseGetKey(index))
        End Function

        Protected Overloads Overrides Function CreateNewElement() As System.Configuration.ConfigurationElement
            Return New FileType
        End Function

        Protected Overrides Function GetElementKey(ByVal element As System.Configuration.ConfigurationElement) As Object
            Return CType(element, FileType).DocType
        End Function
    End Class
End Namespace
