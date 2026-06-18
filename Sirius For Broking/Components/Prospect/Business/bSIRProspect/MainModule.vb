Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date:  27/04/1999
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bSIRProspect"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Username.
	'Public g_sUsername As String * 12
	
	' Password.
	'Public g_sPassword As String * 30
	
	' User ID
	'Public g_iUserID As Integer
	
	' Calling Application
	'Public g_sCallingAppName As String
	' Source ID
	'Public g_iSourceID As Integer
	' Language ID
	'Public g_iLanguageID As Integer
	' Currency ID
	'Public g_iCurrencyID As Integer
	' LogLevel
	'Public g_iLogLevel As Integer
	
	' Constants for the r_vPolicies array
	Public Const PMBPolicyPolicyID As Integer = 0
	Public Const PMBPolicyPolicyTypeID As Integer = 1
	Public Const PMBPolicyRenewalDate As Integer = 2
	Public Const PMBPolicyNoOfTimesQuoted As Integer = 3
	Public Const PMBPolicyTypeDescription As Integer = 4
	Public Const PMBPolicyTargetPremium As Integer = 5
	
	' Constants for the r_vCampaigns array
	Public Const PMBCampaignRecordNo As Integer = 0
	Public Const PMBCampaignCampaignID As Integer = 1
	Public Const PMBCampaignCampaignDate As Integer = 2
	Public Const PMBCampaignDescription As Integer = 3
End Module