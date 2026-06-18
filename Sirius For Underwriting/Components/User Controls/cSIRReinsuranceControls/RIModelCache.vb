Option Strict Off
Option Explicit On
Imports System
Friend NotInheritable Class RIModelCache 
	
	' RI Model header
	Public RIModelID As Integer
	Public Description As String = ""
	
	' Process details
    Public EffectiveDate As Date
    'developer guide no. 101
    Public ExpiryDate As Object
	Public ClaimsAllocation As Integer
	Public CurrencyDescription As String = ""
	
	' Claim XOL details
	Public ClaimXOLID As Integer
	Public ClaimXOLLimit As Decimal
	
	' Catastrophe XOL details
	Public CatXOLID As Integer
	Public CatXOLLimit As Decimal
	Public CatXOLReinstatements As Integer
	
	' RI Model Lines
	Public Lines( ,  ) As Object
End Class
