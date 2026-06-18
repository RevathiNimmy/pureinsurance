Option Strict Off
Option Explicit On
Imports System
<System.Runtime.InteropServices.ProgId("Priority_NET.Priority")> _
Public NotInheritable Class Priority 
	
	' Simple class for ri model line priority management
	Public Priority As Integer
	Public Lines As Decimal
	Public LineLimit As Decimal
	Public Share As Double
	Public LowerLimit As Decimal
	Public Ceding As Double
	'Start-(Arul Stephen)-(Tech Spec - WPR2 - Reinsurance Obligatory)
	Public bIsObligatory As Boolean
	'End-(Arul Stephen)-(Tech Spec - WPR2 - Reinsurance Obligatory)
	'Start (Sriram P)Tech Spec - Calliden WR3.2.1.2 (2) - Relax Edit of Quota Share%.doc sec(6.1.1.1)
	Public lReinsuranceType As Integer
	'End (Sriram P)Tech Spec - Calliden WR3.2.1.2 (2) - Relax Edit of Quota Share%.doc sec(6.1.1.1)
End Class
