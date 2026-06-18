Public Class BaseClaimProcessPerilType
    Public Property TypeCode As String
    Public Property Description As String
    Public Property Reserve As List(Of BaseClaimProcessPerilReserveType)
    Public Property Recovery As List(Of BaseClaimProcessPerilRecoveryType)
End Class
