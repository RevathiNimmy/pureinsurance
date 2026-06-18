'Imports Microsoft.VisualBasic

'Namespace System.Security.Authentication
'    Class SslProtocolsExtensions
'        Private Sub New()
'        End Sub
'        Public Const Tls12 As SslProtocols = DirectCast(&HC00, SslProtocols)
'        Public Const Tls11 As SslProtocols = DirectCast(&H300, SslProtocols)
'    End Class
'End Namespace
Namespace System.Security.Authentication
    Module SslProtocolsExtensions
        Public Const Tls12 As SslProtocols = CType(&HC00, SslProtocols)
        Public Const Tls11 As SslProtocols = CType(&H300, SslProtocols)
    End Module
End Namespace
