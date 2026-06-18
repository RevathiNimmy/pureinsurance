Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	
	' ***************************************************************** '
	' Class Name: BusinessSQL
	' ***************************************************************** '
    ' Table: Treaty
    ' Delete
	Public Const ACDeleteTreatyStored As Boolean = True
	Public Const ACDeleteTreatyName As String = "DeleteTreaty"
	Public Const ACDeleteTreatySQL As String = "spu_Treaty_del"
	
	' Insert
	Public Const ACInsertTreatyStored As Boolean = True
	Public Const ACInsertTreatyName As String = "InsertTreaty"
	Public Const ACInsertTreatySQL As String = "spu_Treaty_add"
	
	' Select
	Public Const ACSelectTreatyStored As Boolean = True
	Public Const ACSelectTreatyName As String = "SelectTreaty"
	Public Const ACSelectTreatySQL As String = "spu_Treaty_saa"

    ' Update
	Public Const ACUpdateTreatyStored As Boolean = True
	Public Const ACUpdateTreatyName As String = "InsertTreaty"
	Public Const ACUpdateTreatySQL As String = "spu_Treaty_upd"

	' Table: Treaty Party
	' Delete
	Public Const ACDeleteTreatyPartyStored As Boolean = True
	Public Const ACDeleteTreatyPartyName As String = "DeleteTreatyParty"
	Public Const ACDeleteTreatyPartySQL As String = "spu_Treaty_Party_del"
	
	' Insert
	Public Const ACInsertTreatyPartyStored As Boolean = True
	Public Const ACInsertTreatyPartyName As String = "InsertTreatyParty"
	Public Const ACInsertTreatyPartySQL As String = "spu_Treaty_Party_add"
	
	' Select
	Public Const ACSelectTreatyPartyStored As Boolean = True
	Public Const ACSelectTreatyPartyName As String = "SelectTreatyParty"
	Public Const ACSelectTreatyPartySQL As String = "spu_Treaty_Party_saa"

	' Table: Party Insurer
	' Select
	Public Const ACSelectPartyInsurerStored As Boolean = True
	Public Const ACSelectPartyInsurerName As String = "SelectParty"
	Public Const ACSelectPartyInsurerSQL As String = "spe_Party_Insurer_sel"
	
	'Get treaty Effective Period
	
	Public Const ACGetTreatyEffectivePeriodStored As Boolean = True
	Public Const ACGetTreatyEffectivePeriodSQL As String = "Spu_Treaty_EffectivePeriod_sel"
    Public Const ACGetTreatyEffectivePeriodName As String = "GetTreatyEffectivePeriod"

    'E005
    Public Const ACSelectTreatyPartyBrokerParticipantStored As Boolean = True
    Public Const ACSelectTreatyPartyBrokerParticipantName As String = "SelectTreatyPartyBrokerParticipant"
    Public Const ACSelectTreatyPartyBrokerParticipantSQL As String = "spu_Treaty_Party_BrokerParticipants_saa"

    Public Const ACInsertTreatyPartyBrokerParticipantStored As Boolean = True
    Public Const ACInsertTreatyPartyBrokerParticipantName As String = "InsertTreatyPartyBrokerParticipant"
    Public Const ACInsertTreatyPartyBrokerParticipantSQL As String = "spu_Treaty_PartyBrokerParticipant_add"

    Public Const ACDeleteTreatyPartyBrokerParticipantStored As Boolean = True
    Public Const ACDeleteTreatyPartyBrokerParticipantName As String = "DeleteTreatyPartyBrokerParticipant"
    Public Const ACDeleteTreatyPartyBrokerParticipantSQL As String = "spu_Treaty_PartyBrokerParticipant_del"
End Module