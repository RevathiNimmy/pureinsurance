Imports System.Configuration

Namespace Config
    Public Class Tasks : Inherits ConfigurationElementCollection
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
                Return "Task"
            End Get
        End Property
        Public ReadOnly Property Task(ByVal index As Integer) As Task
            Get
                Return CType(MyBase.BaseGet(index), Task)
            End Get
        End Property

        Public ReadOnly Property Task(ByVal Name As String) As Task
            Get
                Return CType(MyBase.BaseGet(Name), Task)
            End Get
        End Property
        Public Function GetKey(ByVal index As Integer) As String
            Return CStr(MyBase.BaseGetKey(index))
        End Function

        Protected Overloads Overrides Function CreateNewElement() As System.Configuration.ConfigurationElement
            Return New Task
        End Function

        Protected Overrides Function GetElementKey(ByVal element As System.Configuration.ConfigurationElement) As Object
            Return CType(element, Task).Name
        End Function
    End Class
End Namespace
