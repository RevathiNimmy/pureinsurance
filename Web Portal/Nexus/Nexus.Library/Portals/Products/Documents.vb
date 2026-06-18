Imports System.Configuration

Namespace Config

    Public Class Documents : Inherits ConfigurationElementCollection

        'Private cpDirectoryName As ConfigurationProperty
        Private cpLocation As ConfigurationProperty
        Private cpProperties As ConfigurationPropertyCollection

        Public Sub New()

            cpProperties = New ConfigurationPropertyCollection

            'cpDirectoryName = New ConfigurationProperty("DirectoryName", GetType(String), Nothing, _
            '    ConfigurationPropertyOptions.IsRequired)

            cpLocation = New ConfigurationProperty("Location", GetType(String), Nothing, _
                          ConfigurationPropertyOptions.IsRequired)
            cpProperties.Add(cpLocation)

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
                Return "DocTemplate"
            End Get
        End Property

        Public ReadOnly Property DocTemplate(ByVal index As Integer) As DocTemplate
            Get
                Return CType(MyBase.BaseGet(index), DocTemplate)
            End Get
        End Property

        Public ReadOnly Property DocTemplate(ByVal Name As String) As DocTemplate
            Get
                Return MyBase.BaseGet(Name)
            End Get
        End Property

        Public Function GetKey(ByVal index As Integer) As String
            Return CStr(MyBase.BaseGetKey(index))
        End Function

        Protected Overloads Overrides Function CreateNewElement() As System.Configuration.ConfigurationElement
            Return New DocTemplate
        End Function

        Protected Overrides Function GetElementKey(ByVal element As System.Configuration.ConfigurationElement) As Object
            Return CType(element, DocTemplate).Name
        End Function

        Public ReadOnly Property Location() As String
            Get
                Return CStr(MyBase.Item(cpLocation))
            End Get
        End Property
        'Public ReadOnly Property DirectoryName() As String
        '    Get
        '        Return CStr(MyBase.Item(cpDirectoryName))
        '    End Get
        'End Property

    End Class

End Namespace