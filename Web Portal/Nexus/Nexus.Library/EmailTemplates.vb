Imports System.Configuration

Namespace Config

    Public Class EmailTemplates : Inherits ConfigurationElementCollection

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
                Return "EmailTemplate"
            End Get
        End Property

        Public ReadOnly Property EmailTemplate(ByVal index As Integer) As EmailTemplate
            Get
                Return CType(MyBase.BaseGet(index), EmailTemplate)
            End Get
        End Property

        Public ReadOnly Property EmailTemplate(ByVal ID As String) As EmailTemplate
            Get
                Return MyBase.BaseGet(ID)
            End Get
        End Property

        Public Function GetKey(ByVal index As Integer) As String
            Return CStr(MyBase.BaseGetKey(index))
        End Function

        Protected Overloads Overrides Function CreateNewElement() As System.Configuration.ConfigurationElement
            Return New EmailTemplate
        End Function

        Protected Overrides Function GetElementKey(ByVal element As System.Configuration.ConfigurationElement) As Object
            Return CType(element, EmailTemplate).ID & "_" & CType(element, EmailTemplate).ProductCode
        End Function

    End Class

End Namespace