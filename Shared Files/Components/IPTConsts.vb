Option Strict Off
Option Explicit On
Imports System
<System.Runtime.InteropServices.ProgId("IPTConsts_NET.IPTConsts")> _
 Public Module IPTConsts
	' ***************************************************************** '
	' Class Name: IPTConst
	'
	' Date: 23/12/1997
	'
	' Description: Contains the Field Array positional constants for
	'              the IPTLookup Business objects
	'
	' Edit History:
	' ***************************************************************** '
	
	' {* GENERATED CODE (Begin) *}
	
	' Constant used to Dimension Field Array
	' Constant used to Dimension Field Array
	'Public Const ACFieldArraySize = 3
	
	' Rate Field Array positional constants
	'Public Const ACIPTRateCompany = 0
	'Public Const ACIPTRateRisk = 1
	'Public Const ACIPTRateEffectiveDate = 2
	'Public Const ACIPTRateRate = 3
	
	' Extras Field Array positional constants
	'Public Const ACIPTExtrasCompany = 0
	'Public Const ACIPTExtrasAccount = 1
	'Public Const ACIPTExtrasEffectiveDate = 2
	'Public Const ACIPTExtrasRate = 3
	'Public Const ACIPTExtrasAmount = 4
	
	'Exempt post codes
	Public Const ACIPTExemptPostCode1 As String = "CI"
	Public Const ACIPTExemptPostCode2 As String = "IM"
	Public Const ACIPTExemptPostCode3 As String = "GY"
	Public Const ACIPTExemptPostCode4 As String = "JE"
	
	Public Const PMExemptArea As Integer = 1
	Public Const PMZeroRated As Integer = 2
	Public Const PMNoRiskRecord As Integer = 3
	
	Public Const PMIPTTableLookup As Integer = 0
	Public Const PMIPTExtrasLookup As Integer = 1
	
	Public Const PMReverseCalculation As String = "R"
	
	Public Const PMIPTThreshDate As Integer = 1
	
	' {* GENERATED CODE (End) *}
End Module