Namespace System.Net
    Public Module SecurityProtocolTypeExtensions
        Public Const Tls12 As SecurityProtocolType = CType(System.Security.Authentication.SslProtocolsExtensions.Tls12, SecurityProtocolType)
        Public Const Tls11 As SecurityProtocolType = CType(System.Security.Authentication.SslProtocolsExtensions.Tls11, SecurityProtocolType)
        Public Const SystemDefault As SecurityProtocolType = CType(0, SecurityProtocolType)
    End Module
End Namespace
