Public Class BasePartyCCType
    Inherits BasePartyType
    Public Property CompanyName As String
    Public Property BusinessCode As String
    Public Property MainContact As String
    Public Property NumberOfOffices As Integer
    Public Property NumberOfOfficesSpecified As Boolean
    Public Property NumberOfEmployees As String
    Public Property TradeCode As String
    Public Property ClientDetail As BaseClientSharedDataType
    Public Property CompanyReg As String
    Public Property SICCode As String
    Public Property TradingSince As Date
    Public Property TradingSinceSpecified As Boolean
    Public Property WageRoll As Decimal
    Public Property WageRollSpecified As Boolean
    Public Property TurnoverCode As String
    Public Property FinancialYear As Date
    Public Property FinancialYearSpecified As Boolean
    Public Property Salutation As String
    Public Property TPS As Boolean
    Public Property TPSSpecified As Boolean
    Public Property MPS As Boolean
    Public Property MPSSpecified As Boolean
    Public Property EMPS As Boolean
    Public Property EMPSSpecified As Boolean
    Public Property Source As String
    Public Property AlternativeId As String
End Class
