Imports System.Configuration

Namespace Config

    Public Class Reports : Inherits ConfigurationElementCollection

        Private cpLocation As ConfigurationProperty
        Private cpExportLocation As ConfigurationProperty
        Private cpProperties As ConfigurationPropertyCollection

        Public Sub New()

            cpProperties = New ConfigurationPropertyCollection

            cpLocation = New ConfigurationProperty("Location", GetType(String), Nothing,
                         ConfigurationPropertyOptions.IsRequired)

            cpExportLocation = New ConfigurationProperty("ExportLocation", GetType(String), Nothing,
                         ConfigurationPropertyOptions.IsRequired)


            cpProperties.Add(cpLocation)
            cpProperties.Add(cpExportLocation)


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
                Return "Report"
            End Get
        End Property

        Public ReadOnly Property Report(ByVal index As Integer) As Report
            Get
                Return CType(MyBase.BaseGet(index), Report)
            End Get
        End Property

        Public ReadOnly Property Report(ByVal Name As String) As Report
            Get
                Return CType(MyBase.BaseGet(Name), Report)
            End Get
        End Property

        Public Function GetKey(ByVal index As Integer) As String
            Return CStr(MyBase.BaseGetKey(index))
        End Function

        Protected Overloads Overrides Function CreateNewElement() As System.Configuration.ConfigurationElement
            Return New Report
        End Function

        Protected Overrides Function GetElementKey(ByVal element As System.Configuration.ConfigurationElement) As Object
            Return CType(element, Report).Name
        End Function

        Public ReadOnly Property Location() As String
            Get
                Return CStr(MyBase.Item(cpLocation))
            End Get
        End Property

        Public ReadOnly Property ExportLocation() As String
            Get
                Return CStr(MyBase.Item(cpExportLocation))
            End Get
        End Property

    End Class

End Namespace
