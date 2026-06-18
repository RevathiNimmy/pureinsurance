Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	' ***************************************************************** '
	' Class Name: BusinessSQL
	'
	' Date: 08/09/1998
	'
	
	' Description: Contains the SQL Statements required by the
	'              bSirFreeFormText.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	'Constants for required stored procedures depending on data object used
    'developer guide no. 39
    Public Const ACInsuranceSQL As String = "spe_Ins_File_"
    Public Const ACClaimSQL As String = "spe_Claim_"
    Public Const ACPartySQL As String = "spe_Party_"
    Public Const ACEventSQL As String = "spe_Event_"
	'Selected key field name of required data table
	Public g_sTableKeyName As String = ""
	
	'Selected table prefix name of required data table
	Public g_sTablePrefix As String = ""
	
	' Select All SirFreeFormText SQL
	Public Const ACGetAllDetailsStored As Boolean = True
	Public Const ACGetAllDetailsName As String = "SelectAllSirFreeFormText"
	Public g_sGetAllDetailsSQL As String = ""
	
	' Check ID SQL
	Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckSirFreeFormTextID"
    'developer guide no. 39
    Public Const ACCheckIDSQL As String = "spu_check_SirFreeFormText_id"
	Public Sub SetSQL(ByRef sEntName As String, ByRef sTxtType As String)
		
        'developer guide no. 39
		If (sEntName.ToLower().Trim() = "claim") And (sTxtType.ToLower().Trim() = "private") Then
            g_sGetAllDetailsSQL = ACClaimSQL & "Private_Text_saa"
			g_sTableKeyName = "claim_cnt"
			g_sTablePrefix = "Claim"
			
		ElseIf ((sEntName.ToLower().Trim() = "claim") And (sTxtType.ToLower().Trim() = "public")) Then 
            g_sGetAllDetailsSQL = ACClaimSQL & "Public_Text_saa"
			g_sTableKeyName = "claim_cnt"
			g_sTablePrefix = "Claim"
			
		ElseIf ((sEntName.ToLower().Trim() = "policy") And (sTxtType.ToLower().Trim() = "private")) Then 
            g_sGetAllDetailsSQL = ACInsuranceSQL & "Private_Text_saa"
			g_sTableKeyName = "insurance_file_cnt"
			g_sTablePrefix = "Ins_File"
			
		ElseIf ((sEntName.ToLower().Trim() = "policy") And (sTxtType.ToLower().Trim() = "public")) Then 
            g_sGetAllDetailsSQL = ACInsuranceSQL & "Public_Text_saa"
			g_sTableKeyName = "insurance_file_cnt"
			g_sTablePrefix = "Ins_File"
			
		ElseIf ((sEntName.ToLower().Trim() = "party") And (sTxtType.ToLower().Trim() = "private")) Then 
            g_sGetAllDetailsSQL = ACPartySQL & "Private_Text_saa"
			g_sTableKeyName = "party_cnt"
			g_sTablePrefix = "Party"
			
		ElseIf ((sEntName.ToLower().Trim() = "party") And (sTxtType.ToLower().Trim() = "public")) Then 
            g_sGetAllDetailsSQL = ACPartySQL & "Public_Text_saa"
			g_sTableKeyName = "party_cnt"
			g_sTablePrefix = "Party"
		ElseIf ((sEntName.ToLower().Trim() = "event") And (sTxtType.ToLower().Trim() = "public")) Then 
            g_sGetAllDetailsSQL = ACEventSQL & "Public_Text_saa"
			g_sTableKeyName = "event_cnt"
			g_sTablePrefix = "Event"
		End If
		
	End Sub
End Module