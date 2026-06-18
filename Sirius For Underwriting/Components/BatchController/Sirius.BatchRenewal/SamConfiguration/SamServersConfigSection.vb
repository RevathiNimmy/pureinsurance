Imports System.Configuration

Namespace SamConfiguration

    ''' <summary>
    ''' The Class that will hold the XML config file data loaded into it via the configuration Manager.
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class SamServersConfigSection 
        Inherits ConfigurationSection

        <ConfigurationProperty("Instances")> _
        Public ReadOnly Property InstanceItems() As InstancesCollection
            Get
                Return CType(Me("Instances"), InstancesCollection)
            End Get
        End Property

    End Class

End Namespace
