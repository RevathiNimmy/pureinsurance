Option Strict Off
Option Explicit On
Imports System
Public Module MSecurityConstants
	' Module:   Constants that cannot be exposed as COM enumerations
	' Shared:   Yes
	' Needs:    Nothing
	'
	' THIS MODULE MUST *NEVER* BE PUBLICLY EXPOSED AS A COM INTERFACE.
	' IF THAT WAS DONE, THEN A CUSTOMER OR COMPETITOR WOULD HAVE
	' EASY ACCESS TO OUR ENCRYPTION ROUTINES. INSTEAD, THE CLASS
	' MUST ALWAYS BE SHARED IN CODE THROUGH SOURCESAFE.
	'
	
	'***********************************************************
	' Bespoke Flags
	'***********************************************************
	
	' Recognised bespoke flags.
	Public Const knBFUnknown As Integer = 0
	Public Const knBFNormal As Integer = 1
	Public Const knBF3Sixty As Integer = 2
	Public Const knBFAdvisoryBrokerage As Integer = 3
	Public Const knBFAlliedDunbar As Integer = 4
	Public Const knBFBellLawrieInvMan As Integer = 5
	Public Const knBFBerkeleyWodehouse As Integer = 6
	Public Const knBFEurosure As Integer = 7
	Public Const knBFFirstTrust As Integer = 8
	Public Const knBFHandscombes As Integer = 9
	Public Const knBFJohnstonCambell As Integer = 10
	Public Const knBFLiverpoolVictoria As Integer = 11
	Public Const knBFMarshMcLennan As Integer = 12
	Public Const knBFMichaelDavey As Integer = 13
	Public Const knBFMoorgateHouse As Integer = 14
	Public Const knBFPointonYork As Integer = 15
	Public Const knBFTorquilClark As Integer = 16
	Public Const knBFWilliamMercer As Integer = 17
	Public Const knBFJardineLloydThompson As Integer = 18
	Public Const knBFPannellKerrForster As Integer = 19
	Public Const knBFBerryBirchNoble As Integer = 20
	Public Const knBFRARossborough As Integer = 21
	Public Const knBFRoyalBank As Integer = 22
	Public Const knBFCooperativeBank As Integer = 23
	
	'***********************************************************
	' Module Security Values
	'***********************************************************
	
	' Recognised IDs from the IntModules table.
	Public Const knProductSwift As Integer = 165
	Public Const knProductJigsaw As Integer = 167 ' backward compatibility only
	
	Public Const knModulePersonalClients As Integer = 10005
	Public Const knModuleCorporateClients As Integer = 10004
	Public Const knModuleProposals As Integer = 10000
	Public Const knModuleInvestment1 As Integer = 10002
	Public Const knModuleInvestment2 As Integer = 10003
	Public Const knModuleCommission As Integer = 10001
	Public Const knModuleFees As Integer = 10006
	Public Const knModuleDirectSales As Integer = 10007
	Public Const knModuleClientMoney As Integer = 10008
	Public Const knModuleProvider As Integer = 10009
	
	' These are no longer stored in the IntModules table,
	' but they ARE used elsewhere in the code to indicate
	' which application you are in.
	Public Const knApplicationSwiftClientManager As Integer = 1001
	Public Const knApplicationSwiftSystemManager As Integer = 1000
	Public Const knApplicationSwiftFactFind As Integer = 1005
	Public Const knApplicationSwiftPlanners As Integer = 1004
	Public Const knApplicationSwiftQuery As Integer = 1003
	Public Const knApplicationSwiftCTPLink As Integer = 1002
	Public Const knApplicationS4BClientManager As Integer = 1100
	Public Const knApplicationJigsaw As Integer = 1004 ' backward compatibility only
	Public Const knApplicationDataImport As Integer = 1006
	Public Const knApplicationBriefcase As Integer = 1007
	Public Const knApplicationReportManager As Integer = 1008
	Public Const knApplicationTransactionControl As Integer = 1009
	
	' Recognised Statuses from the IntModules table.
	Public Const knModuleStatusInactive As Integer = 0
	Public Const knModuleStatusOnTrial As Integer = 1
	Public Const knModuleStatusActive As Integer = 2
	
	' Module array indices. These are used:
	' * as indexes into arrays of module statuses
	' * as indexes into control arrays that edit module statuses
	' * to calculate registration passwords
	Public Const kiModuleInvalid As Integer = -1
	Public Const kiModulePersonalClients As Integer = 0
	Public Const kiModuleCorporateClients As Integer = 5
	Public Const kiModuleProposals As Integer = 1
	Public Const kiModuleInvestment1 As Integer = 3
	Public Const kiModuleInvestment2 As Integer = 4
	Public Const kiModuleCommission As Integer = 2
	Public Const kiModuleFees As Integer = 6
	Public Const kiModuleDirectSales As Integer = 7
	Public Const kiModuleClientMoney As Integer = 8
	Public Const kiModuleProvider As Integer = 9
	
	Public Const kiModuleFirst As Integer = 0
	Public Const kiModuleLast As Integer = 9
	
	' Constants shared between applications responsible for encoding
	' and decoding registration passwords.
	Public Const knModChNoChange As Integer = 0
	Public Const knModChActive As Integer = 1
	Public Const knModChOnTrial1Year As Integer = 2
	Public Const knModChOnTrial1Month As Integer = 3
	Public Const knModChOnTrial2Weeks As Integer = 4
	Public Const knModChInactive As Integer = 5
	
	'***********************************************************
	' User Security Values
	'***********************************************************
	
	' User access rights. WARNING: Once defined, the actual values
	' must NEVER be changed because they are used as indexes into
	' the array of flags and also bit masks for the database
	' structure.
	' Data Version 30
	Public Const knUAIsManager As Integer = 0
	Public Const knUAIsWritable As Integer = 1
	Public Const knUARecordAudit As Integer = 2
	Public Const knUAAuditTrailDialog As Integer = 3
	Public Const knUACommissionModule As Integer = 4
	' Added in Data Version 33
	Public Const knUANewStatement As Integer = 5
	Public Const knUAEditStatement As Integer = 6
	Public Const knUADeleteStatement As Integer = 7
	Public Const knUAProcessStatement As Integer = 8
	Public Const knUARecoverStatement As Integer = 9
	Public Const knUATrialMonthEnd As Integer = 10
	Public Const knUAFullMonthEnd As Integer = 11
	Public Const knUAYearEnd As Integer = 12
	Public Const knUAInaccurateCommReceipts As Integer = 13
	Public Const knUAAdminMenu As Integer = 14
	Public Const knUAOptionsMenuTop As Integer = 15
	Public Const knUAOptionsMenuBottom As Integer = 16
	' Added in Data Version 34
	Public Const knUAManageStartOfDay As Integer = 17
	Public Const knUAReportCommissions As Integer = 18
	Public Const knUANewClient As Integer = 19
	Public Const knUAEditClient As Integer = 20
	Public Const knUADeleteClient As Integer = 21
	Public Const knUANewPolicy As Integer = 22
	Public Const knUAEditPolicy As Integer = 23
	Public Const knUADeletePolicy As Integer = 24
	Public Const knUAPolicyEditCommission As Integer = 25 ' on = do nothing / off = disable all fields when editing existing policy
	Public Const knUARunSwiftAccounts As Integer = 26
	' Added in Data Version 36
	Public Const knUAPolicyOverrideCommission As Integer = 27 ' on = enable allow unrestricted editing button / off = disable it
	' Added in Data Version 39
	Public Const knUAReportNewBusiness As Integer = 28
	' Added in Data Version 46
	Public Const knUAKeyFeaturesOverride As Integer = 29 ' on = enable override common code / off = disable it
	' Added in Data Version 49
	Public Const knUAPolicyCommsNotesEditOnly As Integer = 30 ' on = enable all fields / off = enable notes field only [BESPOKE TO ADVISORY & BROKERAGE]
	' Added in Data Version 53
	Public Const knUAKeyFeatOverrideNoReason As Integer = 31 '
	
	' All additions from now on must not clash with Swift 5.
	' To achieve this, add the same constant to both Swift 5
	' and Swift 6, then update the function
	' Admin.frmEditUser.UsesCheckBox() to skip over the
	' constants that you are not actually using in your version
	' of Swift. This MUST be adhered to at all costs.
	' You can tell which constants belong to your product by
	' looking at the version comment. Swift 5 versions
	' start with 5, and Swift 6 versions start with 6.
	
	' Added in 6.0.2.2 DV19
	Public Const knUAClientSalaryHistoryViewSIPP As Integer = 32
	Public Const knUAClientSalaryHistoryViewGroup As Integer = 33
	Public Const knUAClientSalaryHistoryViewSSAS As Integer = 34
	Public Const knUAClientSalaryHistoryAddEditSIPP As Integer = 35
	Public Const knUAClientSalaryHistoryAddEditGroup As Integer = 36
	Public Const knUAClientSalaryHistoryAddEditSSAS As Integer = 37
	Public Const knUAClientSalaryHistoryDeleteSIPP As Integer = 38
	Public Const knUAClientSalaryHistoryDeleteGroup As Integer = 39
	Public Const knUAClientSalaryHistoryDeleteSSAS As Integer = 40
	' Added in 5.4.0 DV61
	Public Const knUANewClientMoneyAccount As Integer = 41 ' A&B only
	' Added in 6.0.4.1 DV30
	Public Const knUAViewAgentComplianceDetails As Integer = 42
	' Added in 5.5.1 DV69
	Public Const knUAIncludeInAccessUpdate As Integer = 43
	Public Const knUAGrantRevokeCommissionAccess As Integer = 44
	Public Const knUANewBusinessComplianceChecks As Integer = 45
	Public Const knUAAssignWorkflowItems As Integer = 46
	Public Const knUADeleteTracking As Integer = 47
	' Added in 6.2.2 DV41
	Public Const knUAProvider As Integer = 48
	' Added in 6.3 DV44
	Public Const knUANonMoneyTransCommission As Integer = 49
	Public Const knUAValuationTransaction As Integer = 50
	Public Const knUACanPutPoliciesOnRisk As Integer = 51
	Public Const knUAViewAllWorkflowGroupsAndItems As Integer = 52
	Public Const knUAAssignWorkflowItemsTeam As Integer = 53
	' Added for 6.4 DV46
	Public Const knUAAssignClientsToTeams As Integer = 54
	Public Const knUAUpdateFundPriceValuationTransaction As Integer = 55
	Public Const knUAUserDisabled As Integer = 56
	' Added for 6.4 DV47 SR2
	Public Const knUAReportEditPolicyFilters As Integer = 57
	Public Const knUAReportEditShowAndHide As Integer = 58
	
	' THIS MUST BE UPDATED WHENEVER YOU ADD A NEW VALUE ABOVE.
	Public Const knUAFirst As Integer = 0
	Public Const knUALast As Integer = 58
	
	' User fee precision values.
	Public Const knUFFeeInvalid As Integer = -1
	Public Const knUFFeeHour As Integer = 0
	Public Const knUFFeeMinute As Integer = 1
	Public Const knUFFeeSecond As Integer = 2
	Public Const knUFFee5Minutes As Integer = 3
	Public Const knUFFee6Minutes As Integer = 4
	Public Const knUFFee10Minutes As Integer = 5
	Public Const knUFFee12Minutes As Integer = 6
	Public Const knUFFee15Minutes As Integer = 7
	Public Const knUFFee30Minutes As Integer = 8
	
	' User fee rounding direction values.
	Public Const knUFFeeNearest As Integer = 0
	Public Const knUFFeeUp As Integer = 1
	Public Const knUFFeeDown As Integer = 2
	
	'***********************************************************
	' Miscellaneous
	'***********************************************************
	
	Public Const ksConsultMessage As String = "Please consult your System Administrator for assistance."
	
	Public Function SecIsProduct(ByVal lID As Integer) As Boolean
		
		Select Case lID
			Case 100 To 999
				Return True
			Case Else
				Return False
		End Select
		
	End Function
	
	Public Function SecIsApplication(ByVal lID As Integer) As Boolean
		
		Select Case lID
			Case 1000 To 9999
				Return True
			Case Else
				Return False
		End Select
		
	End Function
	
	Public Function TransModuleIDToIndex(ByVal lID As Integer) As Integer
		
		Select Case lID
			Case knModulePersonalClients : Return kiModulePersonalClients
			Case knModuleCorporateClients : Return kiModuleCorporateClients
			Case knModuleProposals : Return kiModuleProposals
			Case knModuleInvestment1 : Return kiModuleInvestment1
			Case knModuleInvestment2 : Return kiModuleInvestment2
			Case knModuleCommission : Return kiModuleCommission
			Case knModuleFees : Return kiModuleFees
			Case knModuleDirectSales : Return kiModuleDirectSales
			Case knModuleClientMoney : Return kiModuleClientMoney
			Case knModuleProvider : Return kiModuleProvider
			Case Else : Return kiModuleInvalid
		End Select
		
	End Function
	
	Public Function TransModuleIndexToID(ByVal iIndex As Integer) As Integer
		
		Select Case iIndex
			Case kiModulePersonalClients : Return knModulePersonalClients
			Case kiModuleCorporateClients : Return knModuleCorporateClients
			Case kiModuleProposals : Return knModuleProposals
			Case kiModuleInvestment1 : Return knModuleInvestment1
			Case kiModuleInvestment2 : Return knModuleInvestment2
			Case kiModuleCommission : Return knModuleCommission
			Case kiModuleFees : Return knModuleFees
			Case kiModuleDirectSales : Return knModuleDirectSales
			Case kiModuleClientMoney : Return knModuleClientMoney
			Case kiModuleProvider : Return knModuleProvider
			Case Else : Return 0
		End Select
		
	End Function
End Module