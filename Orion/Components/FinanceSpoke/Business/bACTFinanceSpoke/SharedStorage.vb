Option Strict Off
Option Explicit On
Imports System
Friend NotInheritable Class SharedStorage 
	
	Public Debtor As String = ""
	Public AgreementExists As String = ""
	Public Agreement As String = ""
	Public ClaimNumber As String = ""
	Public Amount As String = ""
	Public DebtorExists As String = ""
	Public ClaimExists As String = ""
	Public DebtorCount As String = ""
	Public AgreementCount As String = ""
	Public UnpaidAmount As String = ""
	Public PostDebt As Object
	Public Account As Object
End Class
