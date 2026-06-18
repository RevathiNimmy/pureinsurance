Option Strict Off
Option Explicit On
Imports System
'developer guide no. 129
Imports SharedFiles
Module MainModule
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iSIRPolicyCashDeposit"
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	
	
	' Public source and language ID's from the
    ' Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iUserID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As bSIRCashDeposit.Business
	
	
	Public Const kRegKeyConstLvwSelect As Integer = 100
	Public Const kRegKeyConstLvwCDNo As Integer = 101
	Public Const kRegKeyConstLvwAvailableBal As Integer = 102
	Public Const kRegKeyConstLvwDateCreated As Integer = 103
	
	'Start WPR85
	Public Const kCashDepositColHIndexSelect As Integer = 0
	Public Const kCashDepositColHIndexCDRef As Integer = 1
	Public Const kCashDepositColHIndexBalance As Integer = 2
	Public Const kCashDepositColHIndexCreatedDate As Integer = 3
	Public Const kCashDepositColHIndexAccountID As Integer = 4
	
	Public Enum ENCashDeposit
		CDID = 0
		AccountID = 1
		PartyID = 2
		CDRef = 3
		Amount = 4
		Balance = 5
		DateCreated = 6
		PartyCode = 7
		PartyName = 8
	End Enum
	
	Public Enum ENPolicyDetails
		InsuranceFileCnt = 0
		InsuranceFolderCnt = 1
		ProductId = 2
		BusinessType = 3
		PartyCnt = 4
		PartyCode = 5
		PartyName = 6
		AgentCnt = 7
		AgentCode = 8
		AgentName = 9
		AgentType = 10
	End Enum
	
	
	Public Enum ENBGPartyType
		Client
		agent
	End Enum
End Module