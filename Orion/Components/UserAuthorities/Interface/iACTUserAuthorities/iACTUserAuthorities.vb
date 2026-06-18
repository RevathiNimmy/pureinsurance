Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 14/02/2000
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iACTUserAuthorities"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACAuthoritiesTitle As Integer = 102
	Public Const ACAuthoritiesTabTitle1 As Integer = 120
	
	Public Const ACAccessCaption As Integer = 103
	Public Const ACUnrestrictedEnquiryCaption As Integer = 104
	Public Const ACUnrestrictedUpdateCaption As Integer = 105
	Public Const ACOverrideDateCaption As Integer = 106
	Public Const ACOverrideRateCaption As Integer = 107
	
	Public Const ACAuthorityCaption As Integer = 108
	Public Const ACRefundAuthorityCaption As Integer = 109
	Public Const ACTransferAuthorityRateCaption As Integer = 110
	
	Public Const ACPaymentsCaption As Integer = 111
	Public Const ACPaymentsAuthorityCaption As Integer = 112
	Public Const ACCurrencyCaption As Integer = 113
	Public Const ACAmountCaption As Integer = 114
	
	Public Const ACWriteOffsCaption As Integer = 115
	Public Const ACAllocationWriteOffsCaption As Integer = 116
	Public Const ACDebtWriteOffsCaption As Integer = 117
	
	Public Const ACClaimPaymentsCaption As Integer = 118
	Public Const ACClaimPaymentsAuthorityCaption As Integer = 119
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	' Menus
	
	' Array positions
	Public Const ACArrayUserID As Integer = 0
	Public Const ACArrayCanWriteOff As Integer = 1
	Public Const ACArrayWriteOffAmount As Integer = 2
	Public Const ACArrayUserName As Integer = 3
	Public Const ACArrayUnrestrictedEnquiry As Integer = 4
	Public Const ACArrayUnrestrictedUpdate As Integer = 5
	Public Const ACArrayFeeDiscount As Integer = 6
	Public Const ACArrayHasTransWriteOff As Integer = 7
	Public Const ACArrayTransWriteOffAmount As Integer = 8
	Public Const ACArrayHasRefundAuthority As Integer = 9
	Public Const ACArrayHasTransferAuthority As Integer = 10
	Public Const ACArrayHasPaymentsAuthority As Integer = 11
	Public Const ACArrayPaymentsAmount As Integer = 12
	Public Const ACArrayHasClaimPaymentsAuthority As Integer = 13
	Public Const ACArrayClaimPaymentsAmount As Integer = 14
	Public Const ACArrayOverrideDate As Integer = 15
	Public Const ACArrayOverrideRate As Integer = 16
	Public Const ACArrayOverridePrePolicyDate As Integer = 17
	Public Const ACArrayOverridePrePolicyRate As Integer = 18
	Public Const ACArrayWriteOffCurrencyID As Integer = 19
	Public Const ACArrayTransWriteOffCurrencyID As Integer = 20
	Public Const ACArrayPaymentsCurrencyID As Integer = 21
	Public Const ACArrayClaimPaymentsCurrencyID As Integer = 22
	
	' This should = the last number in the array elements
	Public Const ACArrayNumberOfElements As Integer = 22
	
	
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	Sub Main_Renamed()
		
	End Sub
End Module