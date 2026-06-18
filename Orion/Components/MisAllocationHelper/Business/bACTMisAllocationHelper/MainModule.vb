Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports System
Module MainModule
	Public Const ACApp As String = "MisallocationsHelper"
	
	Public Const kcTimeout As Integer = 300
	
	'Account Columns
	Public Const ACACAccountCode As Integer = 0
	Public Const ACACLedgerName As Integer = 1
	Public Const ACACMisAllocAmount As Integer = 2
	Public Const ACACTransdetailAmount As Integer = 3
	Public Const ACACDisplayAmount As Integer = 4
	Public Const ACACAccountID As Integer = 5
	
	Public Const ksMisAllocated As String = "Mis-Allocated"
	Public Const ksUnallocatedCash As String = "Unallocated Cash"
	Public Const ksAllocatedCash As String = "Allocated Cash"
	
	'Allocation Columns
	Public Const ACAlCMatchID As Integer = 0
	Public Const ACAlCMatchDate As Integer = 1
	Public Const ACAlCMisAllocAmount As Integer = 2
	
	
	'Transaction Columns
	Public Const ACTCAccountCode As Integer = 0
	Public Const ACTCDocumentRef As Integer = 1
	Public Const ACTCCompany As Integer = 2
	Public Const ACTCSpare As Integer = 3
	Public Const ACTCCurrency As Integer = 4
	Public Const ACTCOriginalAmount As Integer = 5
	Public Const ACTCAllocatedAmount As Integer = 6
	Public Const ACTCTransMatchID As Integer = 7
	Public Const ACTCOSAmount As Integer = 8
	Public Const ACTCCurrencyAllocatedAmount As Integer = 9
	
	'AddRelated Columns
	Public Const ACARCAccountCode As Integer = 0
	Public Const ACARCDocumentRef As Integer = 1
	Public Const ACARCCompany As Integer = 2
	Public Const ACARCOriginalAmount As Integer = 4
	Public Const ACARCID As Integer = 6
	Public Const ACARCOSAmount As Integer = 7
	
	'AddType
	Public Const ACATAddRelated As Integer = 1
	Public Const ACATAddMissing As Integer = 2
	Public Const ACATAddOther As Integer = 3
	
	Public Function GeneratePassword(ByVal nSeed As Integer) As Integer
		
		Const kdMaxInt As Double = 2147483647
        'Const kdBaseDate As Double = #1/1/2000#
        Const kdBaseDate As Date = #1/1/2000#
		
		' Make sure the seed value is positive.
		Dim dSeed As Double = Math.Abs(nSeed) + 3
		
        ' Get the date as a number.
        '-- TODO check during runtime
        'Dim dDate As Double = Math.Abs(DateTime.Today.ToOADate - kdBaseDate) + 3
        Dim dDate As Double = Math.Abs(DateTime.Today.ToOADate - kdBaseDate.ToOADate) + 3
		' Get the time as a number rounded to the nearest quarter-hour.
		Dim dTime As Double = Math.Abs(Math.Floor(DateTimeHelper.Time.ToOADate * 96# + 0.5)) + 3
		
		' Throw all the values together and see what comes out.
		Dim dPassword As Double = dDate * dDate * dTime * dTime * dSeed * dSeed
		
		' Quickly cut down the exponent if it's too high.
		dPassword = CDbl(StringsHelper.Format(dPassword, "#").Substring(0, 15))
		
		' Take the modulus of the value to make sure it fits into
		' the correct range for a long integer.
		Return CInt(Math.Floor(dPassword - (Math.Floor(dPassword / kdMaxInt) * kdMaxInt)))
		
	End Function
End Module