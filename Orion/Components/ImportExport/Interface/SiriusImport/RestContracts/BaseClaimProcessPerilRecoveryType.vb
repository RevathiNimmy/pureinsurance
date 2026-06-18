Public Class BaseClaimProcessPerilRecoveryType
    Public Property TypeCode As String
    Public Property Amount As Decimal
    Public Property RecoveryPartyTypeCode As String
    Public Property RecoveryPartyCode As String
    Public Property RecoveryPartyKey As Integer
    Public Property RecoveryPartyTypeKey As Integer
    Public Property IsSalvageRecovery As Boolean
    Public Property taxGroupCode As String
    Public Property RecoveryAmountSpecified As Boolean
    Public Property RecoveryDetails As BaseClaimProcessReceiptDetailsType
    Public Property isReceiptToDate As Boolean
    Public Property isRecoverToDate As Boolean
    Public Property RecoveryAmount As Decimal
End Class
