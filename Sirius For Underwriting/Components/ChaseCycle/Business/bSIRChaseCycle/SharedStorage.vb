Option Strict Off
Option Explicit On
Imports System
<System.Runtime.InteropServices.ProgId("SharedStorage_NET.SharedStorage")> _
Public NotInheritable Class SharedStorage 
	' ***************************************************************** '
	' Class Name: SharedStorage
	'
    '
	'
    ' Date:01/03/2013
	'
	' Description: Private class that is created from the Business class
	'              and passed to the VB script rule.
	'              Used to hold values that are accessible from the VB
	'              ScriptControl.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' naming convention - although we are implementing these as public variables
    ' rather than property procedures , have used
	' property procedure type namings rather than variable type namings.
	' VB will create the property procedures pair
	' behind the scenes anyway for the type library.
	
	Public BusinessType As String = ""
	Public ServiceLevel As String = ""
    Public Credit As Decimal
	Public CanAutoCancel As Boolean
	Public PolicyVersion As Integer
	Public ClaimRegistered As Boolean
	Public PartiallyPaid As Boolean
	Public AutoCancel As Boolean
	Public RelaxationPeriod As Integer
    Public ENPolicyCancellationCriteria As PolicyCancellationDateCriteria
    Public Used_Instalment_Due_Date As Boolean
	
	
	Public Enum PolicyCancellationDateCriteria
		InstallmentFailureDate
		SystemDate
		PremiumReceivedDate
		PolicyStartDate
	End Enum
End Class