Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 06-10-2008
	'
	' Description Main module of the Component
	'
	' Edit History:Saurabh
	' ***************************************************************** '
	
	
	
	Public Const ACApp As String = "bSIRClientTransClientSel"
	'
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Enum ENPolicy
		InsuranceFileId = 0
		SourceID = 1
		InsuranceFileCnt = 2
		InsuranceRef = 3
		InsuranceFolderCode = 4
		TypeCode = 5
		InsuredName = 6
		shortname = 7
		PartyId = 8
		PartySourceId = 9
		RenewalDate = 10
		InsuranceHolderCnt = 11
		InsuranceFolderCnt = 12
		ProductId = 13
		ProductCode = 14
		Description = 15
		AgentCnt = 16
		DateCreated = 17
		Status = 18
		AgentName = 19
		Premium = 20
		PolicyTypeId = 21
		PolicyType = 22
		GeminiPolicyStatus = 23
		TypeDesc = 24
		NoOfClaims = 26
		AnniversaryCopy = 27
		EventDesciption = 28
	End Enum
End Module