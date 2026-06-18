Option Strict Off
Option Explicit On
Imports System
Public Module gPFConst
	
	'BACS/DDM Constants
	Public Const PFTransactionCreate As Integer = 1
	Public Const PFTransactionCancel As Integer = 2
	Public Const PFTransactionFirst As Integer = 3
	Public Const PFTransactionOngoing As Integer = 4
	Public Const PFTransactionRepresent As Integer = 5
	Public Const PFTransactionLast As Integer = 6
	'AAB-06-Oct-2003 15:02 - adde for new Transaction Type
	Public Const PFTransactionDeposit As Integer = 7
	
	'Status Constants (moved to bSIRPremFinConst, required in plan maintenance)
	'Public Const PFStatusNew = 1
	'Public Const PFStatusPending = 2
	'Public Const PFStatusCollected = 3
	'Public Const PFStatusManual = 4
	'Public Const PFStatusRetrying = 5
	'Public Const PFStatusFailed = 6
	'Public Const PFStatusHold = 7
	'Public Const PFStatusWriteOff = 8
	'Public Const PFStatusTransferred = 9
	
	'filter constants for PFInstalments_saa Stored Procedure
	Public Const PFFilterNone As Integer = 0
	Public Const PFFilterDDMControl As Integer = 1
	Public Const PFFilterGetUnpaidOnly As Integer = 2
	Public Const PFFilterGetPaidOnly As Integer = 3
	
	'filter constants for Select Batch Stored Procedure
	Public Const PFFilterGetForExportList As Integer = 1
	Public Const PFFilterGetForPostingList As Integer = 2
	Public Const PFFilterGetForRecallList As Integer = 3
	Public Const PFFilterGetForExportAction As Integer = 4
	Public Const PFFilterGetForPostingAction As Integer = 5
	Public Const PFFilterGetForRecallAction As Integer = 6
	
	
	'marking constants for Mark Batch Stored Procedure
	Public Const PFMarkAsExported As Integer = 1
	Public Const PFMarkAsPosted As Integer = 2
End Module