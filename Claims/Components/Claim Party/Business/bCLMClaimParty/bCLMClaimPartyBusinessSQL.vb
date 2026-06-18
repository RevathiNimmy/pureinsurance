Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: BusinessSQL
	'
	' Date: 05/05/1999
	'
	' Description: Contains the SQL Statements required by the
	'              bCLMClaimParty.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select All Lookup Details SQL
	Public Const ACGetClaimPartyClaimStored As Boolean = True
	Public Const ACGetClaimPartyClaimName As String = "GetClaimPartyClaim"
	Public Const ACGetClaimPartyClaimSQL As String = "spu_claim_party_link_saa"
	
	' Delete All Lookup Details SQL
	Public Const ACDeleteClaimPartyClaimStored As Boolean = True
	Public Const ACDeleteClaimPartyClaimName As String = "DeleteClaimPartyClaim"
	Public Const ACDeleteClaimPartyClaimSQL As String = "spu_claim_party_link_dar"
	
	'DC010403 ISS3139
	' Insert All Lookup Details SQL
	Public Const ACInsertClaimPartyClaimStored As Boolean = True
	Public Const ACInsertClaimPartyClaimName As String = "InsertClaimPartyClaim"
	Public Const ACInsertClaimPartyClaimSQL As String = "{spu_claim_party_link_add}"
	
	' Select All Lookup Details SQL
	Public Const ACGetWorkClaimPartyClaimStored As Boolean = True
	Public Const ACGetWorkClaimPartyClaimName As String = "GetWorkClaimPartyClaim"
	Public Const ACGetWorkClaimPartyClaimSQL As String = "{call spu_work_claim_party_link_saa (?,?)}"
	
	' Delete All Lookup Details SQL
	Public Const ACDeleteWorkClaimPartyClaimStored As Boolean = True
	Public Const ACDeleteWorkClaimPartyClaimName As String = "DeleteWorkClaimPartyClaim"
	Public Const ACDeleteWorkClaimPartyClaimSQL As String = "{call spu_work_claim_party_link_dar (?,?)}"
	
	' Insert All Lookup Details SQL
	Public Const ACInsertWorkClaimPartyClaimStored As Boolean = True
	Public Const ACInsertWorkClaimPartyClaimName As String = "InsertWorkClaimPartyClaim"
	Public Const ACInsertWorkClaimPartyClaimSQL As String = "{call spu_work_claim_party_link_add (?,?)}"
	
	'DC050402 rejigged the following to determine difference between broking and undweriting
	' Select Single Party Details SQL
	Public Const ACGetWorkSinglePartyClaimStored As Boolean = True
	Public Const ACGetWorkSinglePartyClaimName As String = "GetSinglePartyClaim"
	Public Const ACGetWorkSinglePartyClaimSQL As String = "{call spu_work_single_party_claim_sel (?)}"
	
	' Select Single Party Details SQL
	Public Const ACGetSinglePartyClaimStored As Boolean = True
	Public Const ACGetSinglePartyClaimName As String = "GetSinglePartyClaim"
	Public Const ACGetSinglePartyClaimSQL As String = "{call spu_single_party_claim_sel (?)}"
	
	Public Const ACGetClientPolicyDetailsStored As Boolean = True
	Public Const ACGetClientPolicyDetailsName As String = "GetClientPolicyDetails"
	Public Const ACGetClientPolicyDetailsSQL As String = "spu_get_client_policy_details"
End Module