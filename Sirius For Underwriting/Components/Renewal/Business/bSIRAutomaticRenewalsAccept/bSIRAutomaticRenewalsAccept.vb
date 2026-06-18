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
	' Module Name: MainModule
	'
	' Date:  02/09/2000
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bSIRAutomaticRenewalsAccept"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	
	
	
	Public Const ACIRenewalStatusId As Integer = 0
	Public Const ACIRenewalProduct As Integer = 1
	Public Const ACIRenewalInsuranceHolder As Integer = 2
	Public Const ACIRenewalShortname As Integer = 3
	Public Const ACIRenewalPartyType As Integer = 4
	Public Const ACIRenewalLivePolicy As Integer = 5
	Public Const ACIRenewalPolicyCnt As Integer = 6
	Public Const ACIRenewalPolicy As Integer = 7
	Public Const ACIRenewalInsuranceFolder As Integer = 8
	Public Const ACIRenewalInsuranceStructID As Integer = 9
	Public Const ACIRenewalStatusTypeId As Integer = 10
	Public Const ACIRenewalStatusType As Integer = 11
	Public Const ACIRenewalCriticalDate As Integer = 12
	'RWH(04/10/2000) Extras added for Accept process.
	Public Const ACIRenewalLivePolicyCnt As Integer = 13
	Public Const ACIRenewalCoverStartDate As Integer = 14
	Public Const ACIRenewalExpiryDate As Integer = 15
	Public Const ACIRenewalAgentCnt As Integer = 16
	Public Const ACIRenewalProductId As Integer = 17
	'RWH(05/02/2001) Added renewal_date.
	Public Const ACIRenewalDate As Integer = 18
	
	'Thinh Nguyen 20/03/2002 (start)
	Public Const ACIRenewalLeadAgentCode As Integer = 19
	Public Const ACIRenewalAccHandlerCode As Integer = 20
	Public Const ACIRenewalSourceCode As Integer = 21
	
	Public Const ACRenewalAutomaticAcceptFailureDelStored As Boolean = True
	Public Const ACRenewalAutomaticAcceptFailureDelName As String = "RenewalAutomaticAcceptFailureDel"
	Public Const ACRenewalAutomaticAcceptFailureDelSQL As String = "{call spe_renewal_automatic_accept_failure_del}"
	
	Public Const ACRenewalAutomaticAcceptFailureAddStored As Boolean = True
	Public Const ACRenewalAutomaticAcceptFailureAddName As String = "RenewalAutomaticAcceptFailureAdd"
	Public Const ACRenewalAutomaticAcceptFailureAddSQL As String = "{call spe_renewal_automatic_accept_failure_Add(?,?)}"
    Public Const ACDOCTypeDebitNote = 3
    Public Const ACDocTypeSchedule = 4
    Public Const ACDocTypeCertificate = 5
	
	
	Sub Main_Renamed()
		
		
	End Sub
End Module