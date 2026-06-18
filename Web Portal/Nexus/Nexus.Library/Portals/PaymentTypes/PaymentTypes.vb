Imports System.Configuration

Namespace Config

    Public Class PaymentTypes : Inherits ConfigurationElementCollection

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
                Return "PaymentType"
            End Get
        End Property

        Public ReadOnly Property PaymentType(ByVal index As Integer) As PaymentType
            Get
                Return CType(MyBase.BaseGet(index), PaymentType)
            End Get
        End Property

        Public ReadOnly Property PaymentType(ByVal Name As String) As PaymentType
            Get
                Return CType(MyBase.BaseGet(Name), PaymentType)
            End Get
        End Property

        Public Function GetKey(ByVal index As Integer) As String
            Return CStr(MyBase.BaseGetKey(index))
        End Function

        Protected Overloads Overrides Function CreateNewElement() As System.Configuration.ConfigurationElement
            Return New PaymentType
        End Function

        Protected Overrides Function GetElementKey(ByVal element As System.Configuration.ConfigurationElement) As Object
            Return CType(element, PaymentType).Name
        End Function

    End Class

End Namespace