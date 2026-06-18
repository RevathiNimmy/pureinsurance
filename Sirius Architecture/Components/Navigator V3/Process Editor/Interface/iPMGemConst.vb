Option Strict Off
Option Explicit On
Imports System
Module RemoveAfterSA14
	
	'Process Modes:
	
	' RDC 13062002 constants now available in gPMLibraries
	'Global Const PMProcessModeFull = 101
	'Global Const PMProcessModePostQuote = 102
	'Global Const PMProcessModeSpecific = 103
	'Global Const PMProcessModeStartAtQuote = 104
	'Global Const PMProcessModeDefault = 105
	'Global Const PMProcessModeReview = 106
	'Global Const PMProcessModeCancellations = 107
	'Global Const PMProcessModeClaims = 108
	'Global Const PMProcessModeOverride = 109
	
	Public Const PMTransactionTypeNB As String = "G_NB"
	Public Const PMTransactionTypeMTA As String = "G_MTA"
	Public Const PMTransactionTypeRenewals As String = "G_RENEW"
End Module