Public Class PolicyProcessCommand
    Inherits BaseRequestType
    ' From BaseRequestType
    Public Property BasePartyPCType As BasePartyPCType
    Public Property BasePartyCCType As BasePartyCCType
    ' From PolicyProcessCommandBase
    Public Property AgentCode As String
    Public Property ClientCodeSpecified As Boolean
    Public Property ClientID As Integer
    Public Property CurrencyCode As CurrencyType?
    Public Property CurrencyCodeSpecified As Boolean
    Public Property Policy As BaseQuoteRiskMsgType
    Public Property UpdateParty As Boolean
    ' PolicyProcessCommand addition
End Class
