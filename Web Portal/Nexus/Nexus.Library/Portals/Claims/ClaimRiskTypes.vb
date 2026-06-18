Imports System.Configuration

Namespace Config

    Public Class ClaimRiskTypes : Inherits ConfigurationElementCollection


        Private cpcProperties As ConfigurationPropertyCollection
        Private cpDefaultFolder As ConfigurationProperty

        Public Sub New()
            cpcProperties = New ConfigurationPropertyCollection
            cpDefaultFolder = New ConfigurationProperty("DefaultFolder", GetType(String), Nothing, _
                           ConfigurationPropertyOptions.IsRequired)

            cpcProperties.Add(cpDefaultFolder)
        End Sub
        Public ReadOnly Property DefaultFolder() As String
            Get
                Return CStr(MyBase.Item(cpDefaultFolder))
            End Get
        End Property
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

        Public ReadOnly Property RiskType(ByVal index As Integer) As ClaimRiskTypes
            Get
                Return CType(MyBase.BaseGet(index), ClaimRiskTypes)
            End Get
        End Property

        Public ReadOnly Property RiskType(ByVal Code As String) As ClaimRiskType
            Get
                Return MyBase.BaseGet(Code)
            End Get
        End Property

        Public Function GetKey(ByVal index As Integer) As String
            Return CStr(MyBase.BaseGetKey(index))
        End Function

        Protected Overloads Overrides Function CreateNewElement() As System.Configuration.ConfigurationElement
            Return New ClaimRiskType
        End Function

        Protected Overrides Function GetElementKey(ByVal element As System.Configuration.ConfigurationElement) As Object
            Return CType(element, ClaimRiskType).Code
        End Function


    End Class

End Namespace

