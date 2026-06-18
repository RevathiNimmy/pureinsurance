Imports System.Configuration

Namespace SamConfiguration

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

        <ConfigurationProperty("address", DefaultValue:="", IsKey:=False, IsRequired:=True)>
        Public Property Address() As String
            Get
                Return CStr(Me("address"))
            End Get
            Set(ByVal value As String)
                Me("address") = value
            End Set
        End Property

        <ConfigurationProperty("username", DefaultValue:="", IsKey:=False, IsRequired:=True)>
        Public Property UserName() As String
            Get
                Return CStr(Me("username"))
            End Get
            Set(ByVal value As String)
                Me("username") = value
            End Set
        End Property

        <ConfigurationProperty("password", DefaultValue:="", IsKey:=False, IsRequired:=True)>
        Public Property Password() As String
            Get
                Return CStr(Me("password"))
            End Get
            Set(ByVal value As String)
                Me("password") = value
            End Set
        End Property

        <ConfigurationProperty("concurrentLimit", DefaultValue:="10", IsKey:=False, IsRequired:=False)>
        Public Property ConcurrentLimit() As Integer
            Get
                Return CInt(Me("concurrentLimit"))
            End Get
            Set(ByVal value As Integer)
                Me("concurrentLimit") = value
            End Set
        End Property

        <ConfigurationProperty("timeout", DefaultValue:="100000", IsKey:=False, IsRequired:=False)>
        Public Property Timeout() As Integer
            Get
                Return CInt(Me("timeout"))
            End Get
            Set(ByVal value As Integer)
                Me("timeout") = value
            End Set
        End Property
        <ConfigurationProperty("clientId", DefaultValue:="", IsKey:=False, IsRequired:=False)>
        Public Property ClientID() As String
            Get
                Return CStr(Me("clientId"))
            End Get
            Set(ByVal value As String)
                Me("clientId") = value
            End Set
        End Property
        <ConfigurationProperty("tenantId", DefaultValue:="", IsKey:=False, IsRequired:=False)>
        Public Property TenantId() As String
            Get
                Return CStr(Me("tenantId"))
            End Get
            Set(ByVal value As String)
                Me("tenantId") = value
            End Set
        End Property
        <ConfigurationProperty("tokenUrl", DefaultValue:="", IsKey:=False, IsRequired:=False)>
        Public Property TokenUrl() As String
            Get
                Return CStr(Me("tokenUrl"))
            End Get
            Set(ByVal value As String)
                Me("tokenUrl") = value
            End Set
        End Property
    End Class

End Namespace