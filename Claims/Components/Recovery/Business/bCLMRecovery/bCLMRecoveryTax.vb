Option Strict Off
Option Explicit On
Imports System
<System.Runtime.InteropServices.ProgId("CLMRecoveryTax_NET.CLMRecoveryTax")> _
Public NotInheritable Class CLMRecoveryTax 
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "CLMRecoveryTax"
	
	
	' ***************************************************************** '
	'                        PUBLIC PROPERTIES
	' ***************************************************************** '
	Public TaxTypeCode As String = ""
	Public TaxTotal As Decimal
End Class
