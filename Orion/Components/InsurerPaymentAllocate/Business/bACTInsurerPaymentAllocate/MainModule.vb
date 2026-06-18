Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Edit History :
	'
	' RAW 13/01/2003 : PS187 : replaced hard-coded sql that deleted from
	'                          TransMatch with stored procedure
	'****************************************************************** '
	
	' This App
	Public Const ACApp As String = "bACTInsurerPaymentAllocation"
	
	' This Class
	Private Const ACClass As String = "MainModule"
	
	' Return value
	Private m_lReturn As Integer
	
	' Username.
	
	' Password.
	
	' User ID
	
	' Calling Application
	
	' Source ID
	
	' Language ID
	
	' Log Level
	
	' Currency ID
	
	'System Options
	Public Const ACCurrencyDifferenceCrebitAccount As Integer = 150
	Public Const ACCurrencyDifferenceDebitAccount As Integer = 151
	Public Const ACWriteOffDebtorAccount As Integer = 152
	Public Const ACWriteOffCrebitorAccount As Integer = 153
	
    ' SQL
    'developer guide no.30
    Public Const ACInsurerPaymentsSQL As String = "spu_ACT_Do_InsurerPayments"
	Public Const ACInsurerPaymentsName As String = "InsurerPayments"
	Public Const ACInsurerPaymentsStored As Boolean = True
	
    ' RAW 13/01/2003: PS187: added
    'developer guide no.39
    Public Const ACDeleteMatchPaymentSQL As String = "spu_ACT_Delete_TransMatch"
	Public Const ACDeleteMatchPaymentName As String = "DeleteMatchPayment"
	Public Const ACDeleteMatchPaymentStored As Boolean = True
	' RAW 13/01/2003: PS187: end
	
	Public Const ACSubAllocationDetailID As Integer = 0
	Public Const ACSubCashlistitemID As Integer = 1
	Public Const ACSubAllocationID As Integer = 2
	Public Const ACSubOriginalCurrency As Integer = 3
	Public Const ACSubTransdetailID As Integer = 4
	Public Const ACSubDocumenttypeID As Integer = 5
	Public Const ACSubAccountingDate As Integer = 6
	Public Const ACSubOriginalDate As Integer = 7
	Public Const ACSubAllocateToBase As Integer = 8
	Public Const ACSubOrigBaseAmount As Integer = 9
	Public Const ACSubOrigCcyAmount As Integer = 10
	Public Const ACSubOrigXrate As Integer = 11
	Public Const ACSubEffectiveXrate As Integer = 12
	Public Const ACSubOsBaseAmount As Integer = 13
	Public Const ACSubOsCcyAmount As Integer = 14
	Public Const ACSubAllocBaseAmount As Integer = 15
	Public Const ACSubAllocCcyAmount As Integer = 16
	Public Const ACSubFullyMatched As Integer = 17
	Public Const ACSubWriteOff As Integer = 18
	Public Const ACSubNewOsCcyAmount As Integer = 19
	Public Const ACSubNewOsBaseAmount As Integer = 20
	Public Const ACSubLossGainAmount As Integer = 21
	Public Const ACSubIsPrimary As Integer = 22
	Public Const ACSubDocumentRef As Integer = 23
	Public Const ACSubAllocBaseAmountUnrounded As Integer = 24
	Public Const ACSubWriteOffReasonID As Integer = 25
	Public Const ACSubWriteOffCode As Integer = 26
	Public Const ACSubWriteOffReason As Integer = 27
	Public Const ACSubWriteOffAmount As Integer = 28
	
	Public Const ACAccountingDateColumn As Integer = 1
	Public Const ACDocumentTypeColumn As Integer = 2
	Public Const ACDocumentRefColumn As Integer = 3
	Public Const ACOriginalCurrencyColumn As Integer = 4
	Public Const ACOrigXrateColumn As Integer = 5
	Public Const ACEffectiveXrateColumn As Integer = 6
	Public Const ACCommentColumn As Integer = 7
	Public Const ACOrigCCYColumn As Integer = 8
	Public Const ACOrigBaseColumn As Integer = 9
	Public Const ACOSCCYColumn As Integer = 10
	Public Const ACOSBaseColumn As Integer = 11
	Public Const ACAllocCCYColumn As Integer = 12
	Public Const ACAllocBaseColumn As Integer = 13
	Public Const ACNewOSCCYColumn As Integer = 14
	Public Const ACNewOSBaseColumn As Integer = 15
	Public Const ACWriteOffColumn As Integer = 16
	Public Const ACWriteOffReasonColumn As Integer = 17
	
	' Columns in the Transaction ListView
	Public Const ACIAccountingDate As Integer = 0
	Public Const ACIDocumentTypeId As Integer = 1
	Public Const ACIDocumentRef As Integer = 2
	'Public Const ACIPeriodName = 2
	Public Const ACICurrency As Integer = 3
	Public Const ACICurrencyAmount As Integer = 4
	Public Const ACIBaseAmount As Integer = 5
	'Public Const ACIDocTypeGroupId = 5
	Public Const ACIProject As Integer = 6
	Public Const ACIContract As Integer = 7
	Public Const ACIProduct As Integer = 8
	Public Const ACIDepartment As Integer = 9
	Public Const ACIAgent As Integer = 10
	Public Const ACIClient As Integer = 11
	Public Const ACIAccountShortCode As Integer = 12
	Public Const ACIAccountId As Integer = 13
	Public Const ACICurrencyId As Integer = 14
	Public Const ACITransDetailId As Integer = 15
End Module