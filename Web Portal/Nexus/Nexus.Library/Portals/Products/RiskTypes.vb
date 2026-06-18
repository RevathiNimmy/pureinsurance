Imports System.Configuration

Namespace Config

    Public Class RiskTypes : Inherits ConfigurationElementCollection

        Private cpcProperties As ConfigurationPropertyCollection

        Public Sub New()
            cpcProperties = New ConfigurationPropertyCollection
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
                Return "RiskType"
            End Get
        End Property

        Public ReadOnly Property RiskType(ByVal RiskCode As String) As RiskType
            Get
                Return MyBase.BaseGet(RiskCode.ToUpper())
            End Get
        End Property

        Public ReadOnly Property RiskType(ByVal index As Integer) As RiskType
            Get
                Return CType(MyBase.BaseGet(index), RiskType)
            End Get
        End Property



        Public Function GetKey(ByVal index As Integer) As String
            Return CStr(MyBase.BaseGetKey(index))
        End Function

        Protected Overloads Overrides Function CreateNewElement() As System.Configuration.ConfigurationElement
            Return New RiskType
        End Function

        Protected Overrides Function GetElementKey(ByVal element As System.Configuration.ConfigurationElement) As Object
            Return CType(element, RiskType).RiskCode.ToUpper()
        End Function


    End Class

End Namespace

