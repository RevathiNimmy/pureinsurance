Imports System.Configuration

Namespace SamConfiguration

    ''' <summary>
    ''' The collection class that will store the list of each element/item 
    ''' that is returned back from the configuration manager.
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class InstancesCollection 
        Inherits ConfigurationElementCollection

        Protected Overrides Function CreateNewElement() As ConfigurationElement
            Return New InstanceElement()
        End Function

        Protected Overrides Function GetElementKey(ByVal element As ConfigurationElement) As [Object]
            Return CType(element, InstanceElement).InstanceId
        End Function

        Default Public Shadows Property Item(ByVal index As Integer) As InstanceElement
            Get
                Return CType(BaseGet(index), InstanceElement)
            End Get

            Set(ByVal value As InstanceElement)
                If Not (BaseGet(index) Is Nothing) Then
                    BaseRemoveAt(index)
                End If
                BaseAdd(index, value)
            End Set
        End Property

    End Class

End Namespace