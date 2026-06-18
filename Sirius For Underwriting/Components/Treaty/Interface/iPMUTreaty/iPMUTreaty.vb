Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	' ***************************************************************** '
	
	' Main public constant for all functions to identify which application this is.
	Public Const ACApp As String = "iPMUTreatyParty"
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	' ***************************************************************** '
	'                          TREATY ARRAY
	' ***************************************************************** '
	Public Enum TreatyEnum
		DBTTreatyID
		DBTCode
		DBTDescription
		DBTIsDeleted
		DBTEffectiveDate
		DBTExpiryDate
		DBTAgreementCode
		DBTReinsuranceTypeID
		DBTReinsuranceType
		DBTReplacesTreatyID
		DBTReplacesTreaty
		DBTReplacedEffectiveDate
		DBTReplacedByTreatyID
		DBTTreatyLimit
		DBTCurrencyID
		DBTReinstatements
		DBTReinsuranceCode
	End Enum
	Public DBTMax As TreatyEnum = TreatyEnum.DBTReinsuranceCode

	' ***************************************************************** '
	'                       TREATY PARTY ARRAY
	' ***************************************************************** '
	Public Enum TreatyPartyEnum
		DBTPTreatyPartyID
		DBTPPartyCnt
		DBTPResolvedName
		DBTPTreatyID
		DBTPSharePercent
		DBTPCommissionPercent
		DBTPDomiciledForTax
		DBTPTaxGroupID
        DBTPTaxGroup
        'E016
        DBTPIsReinsurerApproved
        DBTPRowID
    End Enum

    Public DBTPMax As TreatyPartyEnum = TreatyPartyEnum.DBTPRowID

    'E005
    Public Enum TreatyPartyBrokerParticipantEnum
        DBTPBPParticipantonTreatyID
        DBTPBPTreatyID
        DBTPBPTreatyTartyID
        DBTPBPassociatedPartyCnt
        DBTPBPPartyCnt
        DBTPBPParticipantPercent
        DBTPBPShortCode
        DBTPBPName
        RowID
    End Enum
    Public DBTPBPMax As TreatyPartyBrokerParticipantEnum = TreatyPartyBrokerParticipantEnum.DBTPBPName

    'E005
	
	' ***************************************************************** '
	'                        GLOBAL VARIABLES
	' ***************************************************************** '
    ' Public source and language ID's from the Object Manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer

    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
End Module