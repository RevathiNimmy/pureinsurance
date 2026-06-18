Imports System.Configuration

Namespace Configuration

    ''' <summary>
    ''' The class that holds onto each element returned by the configuration manager.
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class InstanceElement
        Inherits ConfigurationElement

        <ConfigurationProperty("instanceId", DefaultValue:="", IsKey:=True, IsRequired:=True)>
    Public Property InstanceId() As String
        Get
            Return CStr(Me("instanceId"))
        End Get
        Set(ByVal value As String)
            Me("instanceId") = value
        End Set
    End Property

    <ConfigurationProperty("threadLimit", DefaultValue:="8", IsKey:=False, IsRequired:=False)>
    Public Property ThreadLimit() As Integer
        Get
            Return CInt(Me("threadLimit"))
        End Get
        Set(ByVal value As Integer)
            Me("threadLimit") = value
        End Set
    End Property

    <ConfigurationProperty("logFile", DefaultValue:="", IsKey:=False, IsRequired:=True)>
    Public Property LogFile() As String
        Get
            Return CStr(Me("logFile"))
        End Get
        Set(ByVal value As String)
            Me("logFile") = value
        End Set
    End Property
    End Class

End Namespace