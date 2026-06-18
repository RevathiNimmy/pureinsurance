Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
Module gSIRLibrary
	' ***************************************************************** '
	' Module Name: gSIRLibrary
	'
	' Date: 18th June 2002
	'
	' Description: Created to store all functions
	'
	' Edit History:
	'   18062002 SJP - Converted all classes into BAS file
	'   19062002 sj  - Move NavKey names to PMNavKeyConst.bas
	' RAW 14/02/2003 : ISS2153 : Added constants for LedgerTypeCodes
	' RAW 20/02/2003 : ISS2332 : added constants for Tax Processing Mode
	' RAW 06/06/2003 : CR23 : added constants for PremiumOverrideEventLevel system option
	' CLG 01/09/2004 : RFC71 Added product option to allow spool AND archive of documents to be selected in document link
	' ***************************************************************** '
	
	
	' ******************************************************'
	'   CONSTANTS - Hidden Options
	' ******************************************************'
	Private Const ACClass As String = "gSIRLibrary" ' RAM20020827 : Changed it to Private
	
	Public SIROPTMasterOptions(5, gPMConstants.SIRHiddenOptions.SIROPTMasterOptionsLastEntry) As Object
	
	
	' ***************************************************************** '
	' * Date Type constants                                           * '
	' ***************************************************************** '
	Public Enum SIREDateType
		' Date Type Return constants
		sireValidDate = 0
		sireNullDate = 1
		sireInvalidDate = 2
	End Enum
	
	' ***************************************************************** '
	' LedgerType Constants
	' ***************************************************************** '
	Public Const g_ksNominalLedgerTypeCode As String = "G"
	Public Const g_ksClientLedgerTypeCode As String = "D"
	Public Const g_ksPurchaseLedgerTypeCode As String = "C"
	Public Const g_ksInsurerLedgerTypeCode As String = "I"
	Public Const g_ksAgentLedgerTypeCode As String = "A"
	Public Const g_ksPremiumFinanceLedgerTypeCode As String = "R"
	Public Const g_ksFeeLedgerTypeCode As String = "F"
	Public Const g_ksDiscountLedgerTypeCode As String = "S"
	Public Const g_ksCommissionLedgerTypeCode As String = "O"
	Public Const g_ksSubAgentLedgerTypeCode As String = "B"
	Public Const g_ksOtherPartyRecLedgerTypeCode As String = "X"
	Public Const g_ksOtherPartyPayLedgerTypeCode As String = "Y"
	Public Const g_ksIntroducerLedgerTypeCode As String = "T"
	
	' ***************************************************************** '
	' Sirius/Orion Link Constants - TF160498
	' ***************************************************************** '
	Public Const SIRACTSalesLedgerShortName As String = "S"
	Public Const SIRACTPurchaseLedgerShortName As String = "P"
	
	' ***************************************************************** '
	' Sirius/Orion Link Constants - EK16/11/99
	' ***************************************************************** '
	Public Const SIRACTInsurerLedgerShortName As String = "I"
	Public Const SIRACTAgentLedgerShortName As String = "A"
	Public Const SIRACTFeeLedgerShortName As String = "F"
	Public Const SIRACTCommissionLedgerShortName As String = "C"
	Public Const SIRACTDiscountLedgerShortName As String = "D"
	Public Const SIRACTPremiumFinanceLedgerShortName As String = "R"
	Public Const SIRACTSubAgentLedgerShortName As String = "U"
	Public Const SIRACTNominalLedgerShortName As String = "N"
	Public Const SIRACTOtherPartySingleCharCode As String = "O"
	Public Const SIRACTOtherPartyRecLedgerShortName As String = "OR"
	Public Const SIRACTOtherPartyPayLedgerShortName As String = "OP"
	Public Const SIRACTIntroducerLedgerShortname As String = "T"
	Public Const SIRACTSalesLedgerMappingCode As String = "Sales Ledger"
	Public Const SIRACTPurchaseLedgerMappingCode As String = "Purchase Ledger"
	
	
	' ***************************************************************** '
	' Sirius/Orion Link Constants - EK16/11/99
	' ***************************************************************** '
	Public Const SIRACTClientLedgerMappingCode As String = "Client Ledger"
	Public Const SIRACTInsurerLedgerMappingCode As String = "Insurer Ledger"
	Public Const SIRACTAgentLedgerMappingCode As String = "Agent Ledger"
	Public Const SIRACTFeeLedgerMappingCode As String = "Fee Receivable"
	Public Const SIRACTCommissionLedgerMappingCode As String = "Commission Ledger"
	Public Const SIRACTDiscountLedgerMappingCode As String = "Discount Ledger"
	Public Const SIRACTPremiumFinanceLedgerMappingCode As String = "Premium Finance"
	Public Const SIRACTSubAgentLedgerMappingCode As String = "Sub Agent Ledger"
	Public Const SIRACTNominalLedgerMappingCode As String = "Nominal Ledger"
	Public Const SIRACTIntroducerLedgerMappingCode As String = "Introducer Ledger"
	
	' ***************************************************************** '
	' Sirius/Orion General Ledger Constants - TF280498
	' ***************************************************************** '
	Public Const SIRACTGLCodeIncome As String = "INCOME"
	Public Const SIRACTGLCodeWrittenPremium As String = "PREMIUMIN"
	Public Const SIRACTGLCodeCoinsuredCommission As String = "CICOMMIN"
	Public Const SIRACTGLCodeRITreatyCommission As String = "RITRCOMIN"
	Public Const SIRACTGLCodeRIOtherCommission As String = "RIOTHCOMIN"
	Public Const SIRACTGLCodeFeesAndCharges As String = "FEESCHRGES"
	Public Const SIRACTGLCodeCoinsuredClaimsInc As String = "CICLAIMINC"
	Public Const SIRACTGLCodeRITreatyClaimInc As String = "RITRCLINC"
	Public Const SIRACTGLCodeRIOtherClaimInc As String = "RIOTHCLINC"
	Public Const SIRACTGLCodeLeadCommission As String = "COMMOUT"
	Public Const SIRACTGLCodeSubAgentCommission As String = "SUBCOMMOUT"
	Public Const SIRACTGLCodeCoinsuredPremium As String = "CIPREMOUT"
	Public Const SIRACTGLCodeRITreatyPremium As String = "RITRPROUT"
	Public Const SIRACTGLCodeRIOtherPremium As String = "RIOTHPROUT"
	Public Const SIRACTGLCodeGrossClaimIncurred As String = "GRCLAIMINC"
	Public Const SIRACTGLCodeCurrentAssets As String = "ASSETS"
	Public Const SIRACTGLCodeSalesLedger As String = "SALESLEDGR"
	Public Const SIRACTGLCodeCoinsuredClaimReserve As String = "CICLAIMRES"
	Public Const SIRACTGLCodeRITreatyClaimReserve As String = "RITRCLRES"
	Public Const SIRACTGLCodeRIOtherClaimReserve As String = "RIOTHCLRES"
	Public Const SIRACTGLCodeGrossClaimPayment As String = "GRCLAIMRES"
	Public Const SIRACTGLCodeCurrentLiabilities As String = "LIABILTIES"
	Public Const SIRACTGLCodePurchaseLedger As String = "PURCHLEDGR"
	Public Const SIRACTGLCodeTax As String = "TAXCLEARG"
	Public Const SIRACTGLCodeGrossClaimReserve As String = "GRCLAIMSRES"
	Public Const SIRACTGLCodeCoinsuredClaimPayment As String = "CICLAIMRES"
	Public Const SIRACTGLCodeRITreatyClaimPayment As String = "RITRCLRES"
	Public Const SIRACTGLCodeRIOtherClaimPayment As String = "RIOTHCLRES"
	
	' ***************************************************************** '
	' Navigator Display Mode for Sirius M4 - TF220498
	' ***************************************************************** '
	Public Const SIRDisplaySinglePane As Integer = 2
	'TF071298
	Public Const SIREntityNamePolicy As String = "POLICY"
	Public Const SIREntityNameParty As String = "PARTY"
	Public Const SIREntityNameClaim As String = "CLAIM"
	Public Const SIREntityNameAccount As String = "ACCOUNT"
	
	' ***************************************************************** '
	' * Document Status constants                                     * '
	' ***************************************************************** '
	Public Enum SIREDocumentStatus
		SIRReady = 1
		SIRLocalPrint = 2
		SIRCollated = 3
		SIRBulkPrint = 4
		SIRDeleted = 5
		SIRFailed = 99
	End Enum
	
	' ***************************************************************** '
	' * Lookup Types                                                  * '
	' ***************************************************************** '
	Public Enum SIRELookupTypes
		' Lookup type constants
		SIRLookupTypeLong = 0
		SIRLookupTypeShort = 1
	End Enum
	
	' ***************************************************************** '
	' * LongFind constants                                            * '
	' ***************************************************************** '
	Public Enum SIRELongFind
		' LongFind constants
		SIRLongFindAll = 0
	End Enum
	
	' ***************************************************************** '
	' * Accumulation constants                                        * '
	' ***************************************************************** '
	Public Enum SIREAccumulationArrayColPos
		' Accumulation constants
		SIRAccumID = 0
		SIRAccumCode = 1
		SIRAccumQuickCode = 2
		SIRAccumCaption = 3
		SIRAccumParentID = 4
		SIRAccumTimeStamp = 5
		SIRAccumStatus = 6
	End Enum
	
	' ***************************************************************** '
	' * Commissions constants                                         * '
	' ***************************************************************** '
	Public Enum SIRECommissionBasis
		' Commission Basis 1/2
		SIRLeadCommission = 1
		SIRSubCommission = 2
	End Enum
	
	Public Enum SIRECommissionBandArrayColPosition
		' Column Positions for Commission Band Array
		SIRCommissionBand = 0
		SIRCommissionPremium = 1
		SIRCommissionPercent = 2
		SIRCommissionValue = 3
		SIRCommissionBandID = 4
	End Enum
	
	' ***************************************************************** '
	' * Tax constants                                                 * '
	' ***************************************************************** '
	Public Enum SIRETaxBasis
		' Tax Basis 0/1/2/3
		SIRTaxBasisZero = 0
		SIRTaxBasisOne = 1
		SIRTaxBasisTwo = 2
		SIRTaxBasisThree = 3
	End Enum
	
	Public Enum SIRETaxBandArrayColPosition
		' Column Positions for Tax Band Array
		SIRTaxBand = 0
		SIRTaxPremium = 1
		SIRTaxPercent = 2
		SIRTaxValue = 3
		SIRTaxBandID = 4
		SIRTaxTypeID = 5
		SIRTaxTypeBandID = 6
		SIRTaxValueID = 7
	End Enum
	
	' ***************************************************************** '
	' * Extras constants                                              * '
	' ***************************************************************** '
	Public Enum SIREExtraArrayColPosition
		' Column Positions for the Extra Array
		SIRExtraName = 0
		SIRExtraType = 1
		SIRExtraPercent = 2
		SIRExtraValue = 3
		SIRExtraTaxed = 4
		SIRExtraCommissioned = 5
		SIRExtraCode = 6
	End Enum
	
	' ***************************************************************** '
	' * Reinsurance constants                                         * '
	' ***************************************************************** '
	Public Enum SIRERILinesArrayColPosition
		' Column Positions for RI Arrangement Lines Array
		SIRRITreatyCode = 0
		SIRRIDefaultSharePercent = 1
		SIRRIThisSharePercent = 2
		SIRRISumInsured = 3
		SIRRIPremiumValue = 4
		SIRRIPremiumTax = 5
		SIRRICommissionPercent = 6
		SIRRICommissionValue = 7
		SIRRICommissionTax = 8
		SIRRIAgreementCode = 9
		SIRRIMethod = 10
		SIRRIArrangementLineID = 11
		SIRRIFacArrangementSummaryID = 12
		'sj 19/06/2002 - start
		'Added from SirConst
		SIRRIOriginalFlag = 13
		'sj 19/06/2002 - end
		' Peter Finney - FAC premium do not need to be proportional to SI anymore!
		SIRRIPremiumPercent = 14
		' Tracy Richards - Pass around the claims ri method
		SIRRIClaimRIMethod = 15
	End Enum
	
	' PWF 20/06/2003 - Moved max definition to enumeration location
	Public Const SIRRIMax As Integer = 15
	
	Public Enum SIRERIFACArrayColPosition
		' Column Positions for RI FAC Arrangement Array
		SIRFacPartyShortName = 0
		SIRFacBidSharePercent = 1
		SIRFacThisSharePercent = 2
		SIRFacSumInsured = 3
		SIRFacPremiumValue = 4
		SIRFacPremiumtax = 5
		SIRFacCommissionPercent = 6
		SIRFacCommissionValue = 7
		SIRFacCommissionTax = 8
		SIRFacAgreementCode = 9
		SIRFacPartyCnt = 10
		' Peter Finney - FAC premium do not need to be proportional to SI anymore!
		SIRFacPremiumPercent = 11
	End Enum
	
	' Constants for Claims Reinsurance Treaty Array
	Public Enum SIRCRILinesArrayColPosition
		SIRCRITreatyCode = 0
		SIRCRIDefaultShare = 1
		SIRCRIThisShare = 2
		SIRCRISumInsured = 3
		SIRCRIReserve = 4
		SIRCRIPayment = 5
		SIRCRIThisReserve = 6
		SIRCRIThisPayment = 7
		SIRCRIAgreementCode = 8
		SIRCRIMethod = 9
		SIRCRIArrangementLineID = 10
		SIRCRIFacArrSummaryID = 11
	End Enum
	
	Public Const SIRCRIMax As Integer = 11
	
	Public Enum SIRFACLinesArrayColPosition
		SIRCFacPartyName = 0
		SIRCFacBidShare = 1
		SIRCFacThisShare = 2
		SIRCFacSumInsured = 3
		SIRCFacReserve = 4
		SIRCFacPayment = 5
		SIRCFacThisReserve = 6
		SIRCFacThisPayment = 7
		SIRCFacAgreementCode = 8
		SIRCFacPartyCnt = 9
	End Enum
	
	' ***************************************************************** '
	' * Rounding 4dp                                                  * '
	' ***************************************************************** '
	Public Enum SIRERounding
		' Rounding Correction Factor for Four Decimal Places
		SIR4DPRoundFactor = CInt(0.000049)
	End Enum
	
	' PH250298 - Add archive file types
	' ***************************************************************** '
	' * Archive file types                                            * '
	' ***************************************************************** '
	Public Enum SIREArcFileTypes
		' File types
		sireArcFileTypeQuote = 0
		sireArcFileTypeInitialLive = 1
		sireArcFileTypeMTAQuote = 2
		sireArcFileTypeMTALive = 3
	End Enum
	
	' ShortFind constants
	Public Const SIRShortFindAgent As String = "Agent"
	Public Const SIRShortFindInsurer As String = "Insurer"
	Public Const SIRShortFindProduct As String = "Product"
	Public Const SIRShortFindTransactionType As String = "Transaction_Type"
	Public Const SIRShortFindRiskType As String = "Risk_Type"
	Public Const SIRShortFindLapsedReason As String = "Lapsed_Reason"
	
	Public Const SIRSystemLowDate As Date = #1/1/1900#
	Public Const SIRSystemHighDate As Date = #12/31/2099#
	
	' Accumulations
	Public Const SIRAccumCodeDelimeter As String = "-"
	
	' Renewals
	Public Const SIRRenPreProcAutoRenew As String = "PreProcAut"
	Public Const SIRRenPreProcManualRenew As String = "PreProcMan"
	Public Const SIRRenScrutinyList As String = "ScrutList"
	Public Const SIRRenCheckList As String = "CheckList"
	Public Const SIRRenMakeUp As String = "MakeUp"
	Public Const SIRRenMakeUpNotReferred As String = "MUNotRef"
	Public Const SIRRenMakeUpReferred As String = "MURef"
	Public Const SIRRenReferral As String = "Referral"
	Public Const SIRRenNoticePrint As String = "NoticPrt"
	Public Const SIRRenNoticeRePrint As String = "NoticRePrt"
	Public Const SIRRenFACNoticePrint As String = "FACNoticPr"
	Public Const SIRRenUpdate As String = "Update"
	Public Const SIRRenLapse As String = "Lapse"
	Public Const SIRRenDelete As String = "Delete"
	Public Const SIRRenFastTrackExisting As String = "FTExisting"
	Public Const SIRRenFastTrackNew As String = "FTNew"
	Public Const SIRRenReMakeUp As String = "ReMakeUp"
	
	' FAC Summary Line Code
	Public Const SIRFacSummaryCode As String = "FAC"
	
	' Table names (Lookup Tables)
	Public Const SIRLookupEventType As String = "event_type"
	Public Const SIRLookupEventRepeatType As String = "repeat_type"
	Public Const SIRLookupProduct As String = "product"
	Public Const SIRLookupCollectionFrom As String = "collection_from"
	'   SJP 18/6/02 - commented as already in SIRConst
	'Public Const SIRLookupRenewalFrequency = "renewal_frequency"
	
	'TF230998
	Public Const SIRLookupSource As String = "Source"
	'   SJP 18/6/02 commented as name conflicts with SIRConst
	'Public Const SIRLookupBranch = "branch"
	'SP140998
	Public Const SIRLookupPartyTitle As String = "party_title"
	'SP170998
	Public Const SIRLookupPartyOccupation As String = "party_occupation"
	'SP091198
	Public Const SIRLookUpAddressUsageType As String = "address_usage_type"
	Public Const SIRLookupBusinessType As String = "business_type"
	Public Const SIRLookupSectionRateType As String = "rate_type"
	Public Const SIRLookupPartyType As String = "party_type"
	Public Const SIRLookupContactType As String = "contact_type"
	Public Const SIRLookupTaxType As String = "tax_type"
	Public Const SIRLookupCollectType As String = "collect_type"
	Public Const SIRLookupAccumTreatmentType As String = "accum_treatment_type"
	Public Const SIRLookupStatsTreatmentType As String = "stats_treatment_type"
	' TF180298
	'   SJP 18/6/02 - commented out duplicate
	'Public Const SIRLookupLapsedReason = "lapsed_reason"
	' PH190298
	Public Const SIRLookupInsFileType As String = "insurance_file_type"
	' TF170398
	Public Const SIRLookupInsFileStatus As String = "insurance_file_status"
	' TF250398
	Public Const SIRLookupRiskStatus As String = "risk_status"
	' TF260398
	Public Const SIRLookupMTAType As String = "mta_type"
	'TF160998
	Public Const SIRLookupClaimCause As String = "Claim_Cause"
	'TF230998
	Public Const SIRLookupClaimStructure As String = "Claim_Structure"
	Public Const SIRLookupPartyStructure As String = "Party_Structure"
	Public Const SIRLookupInsuranceFileStructure As String = "Insurance_File_Structure"
	'TF250998
	Public Const SIRLookupTransactionType As String = "Transaction_Type"
	'EK130300
	Public Const SIRLookupPostingType As String = "posting_type"
	Public Const SIRLookupRenewalMethod As String = "Renewal_Method"
	' PW110702
	Public Const SIRLookupPaymentMethod As String = "payment_method"
	Public Const SIRLookupPaymentFrequency As String = "payment_frequency"
	Public Const SIRLookupAddressOnNotice As String = "agent_address_usage_type"
	'DC110803
	Public Const SIRLookupFSAInsurerStatus As String = "fsa_insurerstatus"
	Public Const SIRLookupFSAInsurerCreditRating As String = "fsa_insurercreditrating"
	'DC180803
	Public Const SIRLookupFSAAgentStatus As String = "fsa_agent_status"
	'MKW290803
	Public Const SIRLookupFSACompanyCategory As String = "FSA_CompanyCategory"
	'DC180903
	Public Const SIRLookupJobTitle As String = "job_title"
	
	' Main Table Names
	Public Const SIRTableParty As String = "party"
	Public Const SIRTableRisk As String = "risk"
	Public Const SIRTableEvent As String = "event"
	Public Const SIRTableInsuranceFile As String = "insurance_file"
	Public Const SIRTableLapsedReason As String = "lapsed_reason"
	
	' Field names
	Public Const SIRFieldDescription As String = "description"
	Public Const SIRFieldEffectiveDate As String = "effective_date"
	
	' Party Table
	Public Const SIRPartyTypeCompany As String = "C"
	Public Const SIRPartyTypePerson As String = "P"
	
	' Insurance File Table
	Public Const SIRInsFileTypePolicy As String = "POLICY"
	Public Const SIRInsFileTypeQuote As String = "QUOTE"
	Public Const SIRInsFileTypeRenewal As String = "RENEWAL"
	' PH200298 - Add insurance_file_type MTA quote
	Public Const SIRInsFileTypeMTAQuote As String = "MTAQUOTE"
	
	' Telephone Table
	Public Const SIRTelTypeLandLine As String = "L"
	Public Const SIRTelTypeFax As String = "F"
	Public Const SIRTelTypeMobile As String = "M"
	
	' Renewal Status
	Public Const SIRRenewalStatusExtract As String = "RE"
	Public Const SIRRenewalStatusPreProcess As String = "RP"
	Public Const SIRRenewalStatusScrutiny As String = "RS"
	Public Const SIRRenewalStatusCheck As String = "RC"
	Public Const SIRRenewalStatusMadeUp As String = "RM"
	Public Const SIRRenewalStatusReMadeUp As String = "RR"
	Public Const SIRRenewalStatusIncomplete As String = "RI"
	Public Const SIRRenewalStatusReferred As String = "RF"
	Public Const SIRRenewalStatusNoticePrint As String = "RN"
	Public Const SIRRenewalStatusRePrint As String = "R2"
	Public Const SIRRenewalStatusFACAdvice As String = "RA"
	Public Const SIRRenewalStatusUpdate As String = "RU"
	
	' Insurance File Status
	Public Const SIRInsFileStatusRenewal As String = "REN"
	Public Const SIRInsFileStatusLapsed As String = "LAP"
	Public Const SIRInsFileStatusCancelled As String = "CAN"
	Public Const SIRInsFileStatusReplaced As String = "REP"
	Public Const SIRInsFileStatusReplacedDRI As String = "REPDRI"
	Public Const SIRInsFileStatusReplacedPT As String = "REPPT"
	
	' Accounts Export Status
	Public Const SIRAccExportStatusPending As String = "p"
	Public Const SIRAccExportStatusSent As String = "s"
	Public Const SIRAccExportStatusFailed As String = "f"
	Public Const SIRAccExportStatusCompleted As String = "c"
	Public Const SIRAccExportStatusDeleted As String = "d"
	
	' PH160298 - Main Events
	Public Const SIRMainEventMTA As String = "MTA"
	
	' PH160298 - Sub Events
	Public Const SIRSubEventTakenUp As String = "TakenUp"
	Public Const SIRSubEventSaved As String = "Saved"
	Public Const SIRSubEventAbandoned As String = "Abandoned"
	
	' Commission Band Minimum
	Public Const SIRCommissionBandMin As Integer = 0
	' Commission Band Maximimum
	Public Const SIRCommissionBandMax As Integer = 99
	' Tax Band Minimum
	Public Const SIRTaxBandMin As Integer = 0
	' Tax Band Maximum
	Public Const SIRTaxBandMax As Integer = 9
	
	' The Following constants were merged in from SirConst.bas
	'---------------------------------------------------------
	'Solution Codes
	Public Const SIRCoreSolution As String = "SIR"
	Public Const SIRMBPSolution As String = "MBP"
	Public Const SIRPMBSolution As String = "PMB"
	Public Const SIRGEMSolution As String = "GEM" 'sj 18/10/99
	
	'General Party type codes - not to be confused with the database column,
	'party type, which denotes a specific party -ie School, Nursing Home etc
	Public Const SIRPartyTypePersonalClient As String = "PC"
	Public Const SIRPartyTypeNoneInsuredClient As String = "NI"
	
	Public Const SIRPartyTypeAgent As String = "AG"
	Public Const SIRPartyTypeCorporateClient As String = "CC"
	Public Const SIRPartyTypeGroupClient As String = "GC"
	'ECK 20/7/99
	Public Const SIRPartyTypeInsurer As String = "IN"
	'CF 13/08/99
	Public Const SIRPartyTypeAccountHandler As String = "AH"
	'EK 12/10/99
	Public Const SIRPartyTypeConsultant As String = "CO"
	'EK 9/11/99
	Public Const SIRPartyTypeExtra As String = "EX"
	Public Const SIRPartyTypeFee As String = "FE"
	Public Const SIRPartyTypeDiscount As String = "DI"
	'EK 27/1/00
	Public Const SIRPartyTypeCommission As String = "CM"
	'Tomo260500
	Public Const SIRPartyTypeNetClient As String = "NC"
	' TF161000
	Public Const SIRPartyTypeFinanceProvider As String = "FP"
	Public Const SIRPartyTypeBroker As String = "BR"
	
	'RWH(23/07/01) Other Party Type.
	Public Const SIRPartyTypeOther As String = "OT"
	' PW240702 Agent Group
	Public Const SIRPartyTypeAgentGroup As String = "AGG"
	Public Const SIRPartyTypeExecutiveHandler As String = "HC"
	
	'General Party type descriptions
	Public Const SIRPartyTypePersonalClientText As String = "Personal Client"
	Public Const SIRPartyTypeAgentText As String = "Agent"
	Public Const SIRPartyTypeCorporateClientText As String = "Corporate Client"
	Public Const SIRPartyTypeGroupClientText As String = "Group Client"
	'ECK 20/7/99
	Public Const SIRPartyTypeInsurerText As String = "Insurer"
	'CF 13/08/99
	Public Const SIRPartyTypeAccountHandlerText As String = "Account Handler"
	'EK 9/11/99
	Public Const SIRPartyTypeExtraText As String = "Extra"
	Public Const SIRPartyTypeFeeText As String = "Fee"
	Public Const SIRPartyTypeDiscountText As String = "Discount"
	'EK 27/1/00
	Public Const SIRPartyTypeCommissionText As String = "Commission"
	'Tomo260500
	Public Const SIRPartyTypeNetClientText As String = "Net Client"
	' TF161000
	Public Const SIRPartyTypeFinanceProviderText As String = "Finance Provider"
	Public Const SIRPartyTypeBrokerText As String = "Broker"
	Public Const SIRPartyTypeConsultantText As String = "Consultant"
	
	'Private Party Individual Code
	Public Const SIRPartyTypePrivateIndividual As String = "PRIVATE"
	'Agent/Broker Intermediary Code
	Public Const SIRPartyTypeBrokerIntermediary As String = "AGENT"
	
	'ABI Description for Sirius Main Address - ie this is the ABI desc for
	'the address that Sirius considers 'main' address and is used for
	'linking to Gemini
	Public Const SIRMainAddressABIDescription As String = "Correspondence Address"
	Public Const SIRMainAddressABICode As String = "3131 XCO"
	Public Const SIRBusinessAddressABIDescription As String = "Business Address"
	Public Const SIRBusinessAddressABICode As String = "3131 002"
	
	'Look up Code for a telephone contact type
	Public Const SIRTelephoneContactCode As String = "TELEPHONE"
	' CTAF 250700
	Public Const SIREmailContactCode As String = "E-MAIL"
	Public Const SIRLetterContactCode As String = "LETTER"
	Public Const SIRFaxConactCode As String = "FAX"
	'DC 04/08/00
	Public Const SIRMainContactCode As String = "MAIN"
	
	'Look up table constants
	Public Const SIRLookupPartyAgentOrigin As String = "party_agent_origin"
	Public Const SIRLookupPartyGroupType As String = "party_group_type"
	Public Const SIRLookupPartyTrade As String = "party_trade"
	'DC091204
	Public Const SIRLookupPartyAgentType As String = "party_agent_type"
	
	'*************
	' MEvans : 18-03-2003 : Issue 2114
	Public Const SIRLookupSupplierBusiness As String = "supplier_business"
	Public Const SIRLookupSupplierSpeciality As String = "supplier_speciality"
	'*************
	
	'Added by ECK 26/04/99
	Public Const SIRLookupPartyBusiness As String = "party_business"
	Public Const SIRLookupArea As String = "area"
	Public Const SIRLookupCurrency As String = "currency"
	Public Const SIRLookupReminderType As String = "reminder_type"
	Public Const SIRLookupServiceLevel As String = "service_level"
	Public Const SIRLookupSeasonalGift As String = "seasonal_gift"
	Public Const SIRLookupNationality As String = "nationality"
	Public Const SIRLookupProspectStatus As String = "prospect_status"
	'sj 19/06/2002 - start
	'Duplicate declaration removed
	'Public Const SIRLookupPartyType = "party_type"
	'sj 19/06/2002 - end
	Public Const SIRLookupPolicyType As String = "policy_type"
	Public Const SIRLookupInsuranceFileStatus As String = "insurance_file_status"
	Public Const SIRLookupRenewalFrequency As String = "renewal_frequency"
	Public Const SIRLookupRenewalStopCode As String = "renewal_stop_code"
	Public Const SIRLookupPolicyRelationshipType As String = "policy_relationship_type"
	Public Const SIRLookupLapsedReason As String = "lapsed_reason"
	Public Const SIRLookupRiskCode As String = "risk_code"
	Public Const SIRLookupAnalysisCode As String = "analysis_code"
	'ECK 05/08/99
	'sj 19/06/2002 - start
	'Duplicate declaration removed
	'Public Const SIRLookupTransactionType = "transaction_type"
	'sj 19/06/2002 - end
	'EK 130300
	Public Const SIRLookupPostingTypeType As String = "posting_type"
	'EK 7/10/99
	Public Const SIRLookupRiskGroup As String = "risk_group"
	'eck120500
	'sj 19/07/2002 - start
	'Public Const SIRLookupBranch = "source"
	Public Const SIRLookupSubBranch As String = "sub_branch"
	'sj 19/07/2002 - end
	'Tomo270300
	Public Const SIRLookupStrengthCode As String = "strength_code"
	Public Const SIRLookupSICCode As String = "SIC_code"
	Public Const SIRLookupPFBusinessCode As String = "PF_business_code"
	
	'Navigator Key Names
	Public Const SIRNavKeyAgentOnly As String = "agent_only"
	
	' System Process Codes (used in WorkManager, Navigator & various components)
	Public Const SIRProcessCodeNBQDirect As String = "NBQDIRECT"
	Public Const SIRProcessCodeNBQIndirect As String = "NBQINDIR"
	Public Const SIRProcessCodeMTA As String = "MTA"
	Public Const SIRProcessCodeRenewal As String = "RENEWAL"
	Public Const SIRProcessCodeReviewQuote As String = "RVWQUOTE"
	Public Const SIRProcessCodeReviewPolicy As String = "RVWPOLICY"
	
	' Additional Insurance File Types for MTAs
	'sj 19/06/2002 - start
	'Duplicate declaration removed
	'Public Const SIRInsFileTypeMTAQuote = "MTAQUOTE"
	'sj 19/06/2002 - end
	Public Const SIRInsFileTypeMTAPermanent As String = "MTA PERM"
	Public Const SIRInsFileTypeMTATemporary As String = "MTA TEMP"
	Public Const SIRInsFileTypeMTAIncomplete As String = "MTA INCOMP"
	
	'SJ 24/06/2004 - start
	Public Const SIRInsFileTypeMTAPermanentCancellation As String = "MTAPERMCAN"
	'SJ 24/06/2004 - end
	
	'JK280999
	'Additional Insurance File Type for Cancellations
	Public Const SIRInsFileTypeCancelled As String = "CANCEL"
	
	' Email Key Information (ECK 21/5/99)
	Public Const SIRKeyEmailAddress As String = "Email Address"
	Public Const SIRKeyEmailSubject As String = "Email Subject"
	Public Const SIRKeyEmailText As String = "Email Text"
	Public Const SIRKeyEmailAttachment As String = "Email Attachment"
	
	'RWH(26/09/2000) Transaction Code constants. RSAIB Process 28. Auto Doc Production.
	Public Const SIRTransactionCodeNewBusiness As String = "NB" 'New Business.
	Public Const SIRTransactionCodeAdditionalPremium As String = "AP" 'Additional Premium.
	Public Const SIRTransactionCodeReturnPremium As String = "RP" 'Return Premium.
	Public Const SIRTransactionCodeRenewal As String = "RN" 'Renewal.
	Public Const SIRTransactionCodeClaimOpen As String = "CO" 'Claim Open.
	Public Const SIRTransactionCodeClaimRevision As String = "CR" 'Claim Revision.
	Public Const SIRTransactionCodeClaimPaid As String = "CP" 'Claim Paid.
	
	'AJM 06/03/01 - additional transaction types for claims document production
	'               these are to be used depending on requirements and original
	'               transaction type i.e. "CO"(open claim) & "CP"(claim paymenmt)
	Public Const SIRTransactionCodeClaimNotifyClient As String = "CC" 'Claim notification Client
	Public Const SIRTransactionCodeClaimNotifyAgent As String = "CA" 'Claim notification Agent
	Public Const SIRTransactionCodeClaimNotifyInsurer As String = "CI" 'Claim notification Insurer
	Public Const SIRTransactionCodeClaimLargeLossReins As String = "LL" 'Claim large loss advice to reinsurer
	Public Const SIRTransactionCodeClaimLossAdviceReins As String = "LA" 'Claim loss advice to non-treaty reinsurer
	Public Const SIRTransactionCodeClaimJacket As String = "CJ" 'Claim jacket
	Public Const SIRTransactionCodeClaimExternalHandler As String = "EH" 'Claim notification to external handler
	Public Const SIRTransactionCodeClaimChequeRequest As String = "CQ" 'Claim cheque requisition form
	Public Const SIRTransactionCodeClaimAcceptForm As String = "AF" 'Claim acceptance form
	Public Const SIRTransactionCodeClaimPayAdviceReins As String = "NT" 'Claim non treaty reinsurer payment advice
	Public Const SIRTransactionCodeClaimAdviceAgent As String = "AS" 'Claim advice slip to agent
	
	'RWH(02/12/2001) Further Transaction Code Constants for use in
	'Underwriting Authority. (Maintaining standard with Doc Production) ***********
	Public Const SIRTransCodeIdNewBusiness As Integer = 1 'New Business.
	Public Const SIRTransCodeIdAdditionalPremium As Integer = 2 'Additional Premium.
	Public Const SIRTransCodeIdReturnPremium As Integer = 3 'Return Premium.
	Public Const SIRTransCodeIdRenewal As Integer = 4 'Renewal.
	Public Const SIRTransCodeIdClaimOpen As Integer = 5 'Claim Open.
	Public Const SIRTransCodeIdClaimRevision As Integer = 6 'Claim Revision.
	Public Const SIRTransCodeIdClaimPaid As Integer = 7 'Claim Paid.
	'sj 17/12/2002 - start
	'PS104
	Public Const SIRTransCodeIdBackdatedCancellation As Integer = 8
	Public Const SIRTransCodeIdBackdatedMTA As Integer = 9
	'sj 17/12/2002 - end
	
	Public Const SIRTransCodeDescNewBusiness As String = "New Business"
	Public Const SIRTransCodeDescAdditionalPremium As String = "Additional Premium"
	Public Const SIRTransCodeDescReturnPremium As String = "Return Premium"
	Public Const SIRTransCodeDescRenewal As String = "Renewal"
	Public Const SIRTransCodeDescClaimOpen As String = "Claim Open"
	Public Const SIRTransCodeDescClaimRevision As String = "Claim Revision"
	Public Const SIRTransCodeDescClaimPaid As String = "Claim Paid"
	'sj 17/12/2002 - start
	'PS104
	Public Const SIRTransCodeDescBackdatedCancellation As String = "Backdated Cancellation"
	Public Const SIRTransCodeDescBackdatedMTA As String = "Backdated MTA"
	'sj 17/12/2002 - end
	'******************************************************************************
	
	'RWH(26/09/2000) Document/Process Type constants. RSAIB Process 28. Auto Doc Production.
	Public Const SIRDocTypeQuotation As Integer = 1
	Public Const SIRDocTypeProposal As Integer = 2
	Public Const SIRDocTypeDebitNote As Integer = 3
	Public Const SIRDocTypeSchedule As Integer = 4
	Public Const SIRDocTypeCertificate As Integer = 5
	Public Const SIRDocTypeRenewalNotice As Integer = 6
	Public Const SIRDocTypeClaims As Integer = 7
	'RWH(12/02/2001) Added Lapse & Decline.
	Public Const SIRDocTypeLapse As Integer = 8
	Public Const SIRDocTypeDecline As Integer = 9
	Public Const SIRDocTypeCancel As Integer = 10
	Public Const SIRDocTypeReinstatePolicy As Integer = 11
	Public Const SIRDocTypeBackdatedCancellation As Integer = 12
	Public Const SIRDocTypeReinstateRisk As Integer = 13
	
	'****************************
	'MEvans : 03-12-2002 : 202 Added Purchase Order
	Public Const SIRDocTypePurchaseOrder As Integer = 11
	'****************************
	
	'RWH(26/09/2000) Document/Process Type Code constants. RSAIB Process 28. Auto Doc Production.
	Public Const SIRDocTypeCodeQuotation As String = "QUO"
	Public Const SIRDocTypeCodeProposal As String = "PRO"
	Public Const SIRDocTypeCodeDebitNote As String = "DCN"
	Public Const SIRDocTypeCodeSchedule As String = "SCH"
	Public Const SIRDocTypeCodeCertificate As String = "CER"
	Public Const SIRDocTypeCodeRenewalNotice As String = "RNC"
	Public Const SIRDocTypeCodeClaims As String = "CLM"
	'RWH(12/02/2001) Added Lapse & Decline.
	Public Const SIRDocTypeCodeLapse As String = "LAP"
	Public Const SIRDocTypeCodeDecline As String = "DEC"
	Public Const SIRDocTypeCodeCancel As String = "CAN"
	Public Const SIRDocTypeCodeCancelAgent As String = "CAG"
	Public Const SIRDocTypeCodeCancelClient As String = "CCL"
	
	'****************************
	'MEvans : 03-12-2002 : 202 Added Purchase Order Code
	Public Const SIRDocTypeCodePurchaseOrder As String = "PUR"
	'****************************
	
	'sj 01/10/2002 - start
	'Event Type Notes Constants
	Public Const ACNotesCustomer As String = "N_CUST"
	Public Const ACNotesAccount As String = "N_ACCOUNT"
	Public Const ACNotesClaims As String = "N_CLAIMS"
	Public Const ACNotesPolicy As String = "N_POLICY"
	Public Const ACNotesFSA As String = "N_FSA"
	Public Const ACNotesWarning As String = "N_WARN" '2005 Sticky Notes
	'DC080304 PN10843 -allow policy and claim complaints to be displayed
	Public Const ACPolicyComplaint As String = "POLCMPLT"
	Public Const ACClaimComplaint As String = "CLMCMPLT"
	'sj 01/10/2002 - end
	'DC200405 PN20287 adde GII Event Types
	Public Const ACNotesFSAGIIDisclosure As String = "N_FSA_DISC"
	Public Const ACNotesFSAGIIDN As String = "N_FSA_DN"
	
	' PN17216 - constant for event type of FSA Product Disclosure
	Public Const ACEventFSAProductDisclosure As String = "PD_FSA"
	
	' Reinsurance Constants
	
	'sj 19/06/2002 - start
	'Duplicate declarations removed
	' Column Positions for RI Arrangement Lines Array
	'Public Const SIRRITreatyCode = 0
	'Public Const SIRRIDefaultSharePercent = 1
	'Public Const SIRRIThisSharePercent = 2
	'Public Const SIRRISumInsured = 3
	'Public Const SIRRIPremiumValue = 4
	'Public Const SIRRICommissionPercent = 5
	'Public Const SIRRICommissionValue = 6
	'Public Const SIRRIAgreementCode = 7
	'Public Const SIRRIMethod = 8
	'Public Const SIRRIArrangementLineID = 9
	'Public Const SIRRIFacArrangementSummaryID = 10
	'Public Const SIRRIOriginalFlag = 11
	
	'sj 19/06/2002 - end
	' PWF 20/06/2003 - Moved
	'Public Const SIRRIMax = 11
	
	'sj 24/10/2002 - start
	'Printing Modes
	Public Const ACNormalMode As Integer = 0
	Public Const ACMergeMode As Integer = 1
	Public Const ACPrintMode As Integer = 2
	Public Const ACPrintSilentMode As Integer = 3
	Public Const ACSpoolDocMode As Integer = 4
	Public Const ACSpoolReportMode As Integer = 5
	Public Const ACViewMode As Integer = 6 ' To support view only mode
	Public Const ACSwiftEditMode As Integer = 7 ' RAM20050106 : Support for Swift
	'sj 24/10/2002 - end
	
	
	'sj 19/06/2002 - start
	'Duplicate declarations removed
	' Column Positions for RI FAC Arrangement Array
	'Public Const SIRFacPartyShortName = 0
	'Public Const SIRFacBidSharePercent = 1
	'Public Const SIRFacThisSharePercent = 2
	'Public Const SIRFacSumInsured = 3
	'Public Const SIRFacPremiumValue = 4
	'Public Const SIRFacCommissionPercent = 5
	'Public Const SIRFacCommissionValue = 6
	'Public Const SIRFacAgreementCode = 7
	'Public Const SIRFacPartyCnt = 8
	
	' FAC Summary Line Code
	'Public Const SIRFacSummaryCode = "FAC"
	
	' Rounding Correction Factor for Four Decimal Places
	'Public Const SIR4DPRoundFactor = 0.000049
	'sj 19/06/2002 - end
	
	'AK 010802 - removed these constants from here, as they are already available through
	'            gPMConst.bas
	
	'' SP 19/6/2002 - added from PMConst
	'' Type Of Business (Navigator)
	'Public Const PMTypeOfBusinessGeneric = ""
	'Public Const PMTypeOfBusinessNB = "NB"
	'Public Const PMTypeOfBusinessRN = "RN"
	'Public Const PMTypeOfBusinessENN = "ENN"
	'Public Const PMTypeOfBusinessENR = "ENR"
	
	'sj 02/01/2003 - start
	'PS104
	'Run mode constants for backdated cancellations and reinstatements
	Public Const ACRunModeNewMtaCancel As Integer = 1
	Public Const ACRunModeNewMtaReinstate As Integer = 2
	Public Const ACRunModeMtaLinkCancel As Integer = 3
	Public Const ACRunModeMtaLinkReinstate As Integer = 4
	Public Const ACRunModeMtaLinkMTA As Integer = 5
	Public Const ACRunModeNewMTAReinstateRisk As Integer = 6
	Public Const ACRunModeMtaLinkReinstateRisk As Integer = 7
	
	'Merge status constants for backdated cancellations and reinstatements
	Public Const ACRStatusMerge As String = "M"
	Public Const ACRStatusAddPostChange As String = "A"
	Public Const ACRStatusDeletedPostChange As String = "DP"
	Public Const ACRStatusNoMerge As String = "N"
	Public Const ACRStatusNoProcess As String = "NP"
	'sj 02/01/2003 - end
	
	' RAW 20/02/2003 : ISS2332 : added
	Public Const g_ksTaxProcessingModeReinsurance As String = "RI"
	Public Const g_ksTaxProcessingModeAgent As String = "AGENT"
	Public Const g_ksTaxProcessingModeTax As String = "TAX"
	Public Const g_ksTaxProcessingModeInstalments As String = "INSTALMENTS"
	' RAW 20/02/2003 : ISS2332 : added
	
	'Renewal Process Constants
	Public Const g_kRenewalSelection As String = "SELECTION"
	Public Const g_kRenewalAccept As String = "ACCEPT"
	Public Const g_kRenewalInvitation As String = "INVITATION"
	
	' RAW 06/06/2003 : CR23 : added
	Public Const g_klPremiumOverrideEventLevel_None As Integer = 0
	Public Const g_klPremiumOverrideEventLevel_Summary As Integer = 1
	Public Const g_klPremiumOverrideEventLevel_Full As Integer = 2
	
	'AG 05/10/2004 - PN13722 - Added
	Public Const knLapsedReasonsNotDeleted As Integer = 0
	
	
	' ******************************************************'
	'   FUNCTIONS
	' ******************************************************'
	
	' ******************************************************'
	'
	' Name: populateArray
	'
	' Description: This will return a list of Product Options
	' History: 06/06/2002 SJP - Created
	'
	'*******************************************************'
	Public Function populateArray() As Integer
		
		Dim result As Integer = 0
		Try 
			
			'   Array Usage
			'   Dimension 0 = Option Key
			'   Dimension 1 = Branch
			'   Dimension 2 = Value on install if enabled
			'   Dimension 3 = U/W Type
			'   Dimension 4 = Option Name
			'   Dimension 5 = Settings
			
			'   This part will not be used hence it will not be
			'   returned by the Master Category
			'SIROPTMasterOptions(0, 0) = SIROPTUnderwriting
			'SIROPTMasterOptions(1, 0) = SIRBCHHeadOffice
			'SIROPTMasterOptions(2, 0) = ""
			'SIROPTMasterOptions(3, 0) = ""
			'SIROPTMasterOptions(4, 0) = "Underwriting Option"
			

			SIROPTMasterOptions(0, 0) = gPMConstants.SIRHiddenOptions.SIROPTIsNRMA

			SIROPTMasterOptions(1, 0) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 0) = ""

			SIROPTMasterOptions(3, 0) = ""

			SIROPTMasterOptions(4, 0) = "NRMA Switch"

			SIROPTMasterOptions(5, 0) = "0 for OFF, 1 for ON"
			

			SIROPTMasterOptions(0, 1) = gPMConstants.SIRHiddenOptions.SIROPTValidateAlternativeIdentifier

			SIROPTMasterOptions(1, 1) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 1) = ""

			SIROPTMasterOptions(3, 1) = ""

			SIROPTMasterOptions(4, 1) = "Validate Alternative Identifier"

			SIROPTMasterOptions(5, 1) = "0 for OFF, 1 for ON"
			

			SIROPTMasterOptions(0, 2) = gPMConstants.SIRHiddenOptions.SIROPTChaserLettersEnabled

			SIROPTMasterOptions(1, 2) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 2) = ""

			SIROPTMasterOptions(3, 2) = ""

			SIROPTMasterOptions(4, 2) = "Chaser Letters Enabled"

			SIROPTMasterOptions(5, 2) = "0 for OFF, 1 for ON"
			

			SIROPTMasterOptions(0, 3) = gPMConstants.SIRHiddenOptions.SIROPTAONAffinity

			SIROPTMasterOptions(1, 3) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 3) = ""

			SIROPTMasterOptions(3, 3) = ""

			SIROPTMasterOptions(4, 3) = "AON Client Screen Changes"

			SIROPTMasterOptions(5, 3) = "0 for OFF, 1 for ON"
			

			SIROPTMasterOptions(0, 4) = gPMConstants.SIRHiddenOptions.SIROPTRestrictedInsurerAccess

			SIROPTMasterOptions(1, 4) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 4) = ""

			SIROPTMasterOptions(3, 4) = ""

			SIROPTMasterOptions(4, 4) = "Restricted Insurer Access"

			SIROPTMasterOptions(5, 4) = "0 for OFF, 1 for ON"
			

			SIROPTMasterOptions(0, 5) = gPMConstants.SIRHiddenOptions.SIROPTClientPolicyLinkage

			SIROPTMasterOptions(1, 5) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 5) = ""

			SIROPTMasterOptions(3, 5) = ""

			SIROPTMasterOptions(4, 5) = "Associated Client -> Policy Linkage"

			SIROPTMasterOptions(5, 5) = "0 for OFF, 1 for ON"
			

			SIROPTMasterOptions(0, 6) = gPMConstants.SIRHiddenOptions.SIROPTAsynchronousPosting

			SIROPTMasterOptions(1, 6) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 6) = ""

			SIROPTMasterOptions(3, 6) = ""

			SIROPTMasterOptions(4, 6) = "Asynchronous Posting of Accounts/Transactions"

			SIROPTMasterOptions(5, 6) = "0 for OFF, 1 for ON"
			

			SIROPTMasterOptions(0, 7) = gPMConstants.SIRHiddenOptions.SIROPTClientSummary

			SIROPTMasterOptions(1, 7) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 7) = ""

			SIROPTMasterOptions(3, 7) = ""

			SIROPTMasterOptions(4, 7) = "View Client Summary Info First"

			SIROPTMasterOptions(5, 7) = "0 for OFF, 1 for ON"
			

			SIROPTMasterOptions(0, 8) = gPMConstants.SIRHiddenOptions.SIROPTEnhancedOrionSecurity

			SIROPTMasterOptions(1, 8) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 8) = ""

			SIROPTMasterOptions(3, 8) = ""

			SIROPTMasterOptions(4, 8) = "Enhanced Orion Security on Account Folders"

			SIROPTMasterOptions(5, 8) = "0 for OFF, 1 for ON"
			
			'TF150702

			SIROPTMasterOptions(0, 9) = gPMConstants.SIRHiddenOptions.SIROPTUseRetailLogicLink

			SIROPTMasterOptions(1, 9) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 9) = ""

			SIROPTMasterOptions(3, 9) = ""

			SIROPTMasterOptions(4, 9) = "Use Retail Logic Link"

			SIROPTMasterOptions(5, 9) = "0 for OFF, 1 for ON"
			
			'GSD 160702

			SIROPTMasterOptions(0, 10) = gPMConstants.SIRHiddenOptions.SIROPTClaimsBuilder

			SIROPTMasterOptions(1, 10) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 10) = ""

			SIROPTMasterOptions(3, 10) = ""

			SIROPTMasterOptions(4, 10) = "Claims Builder Enabled"

			SIROPTMasterOptions(5, 10) = "0 for OFF, 1 for ON"
			
			'sj 22/07/2002 - start

			SIROPTMasterOptions(0, 11) = gPMConstants.SIRHiddenOptions.SIROPTFutureDateAddressChanges

			SIROPTMasterOptions(1, 11) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 11) = ""

			SIROPTMasterOptions(3, 11) = ""

			SIROPTMasterOptions(4, 11) = "Future Date Address Changes"

			SIROPTMasterOptions(5, 11) = "0 for OFF, 1 for ON"
			'sj 22/07/2002 - end
			
			' CTAF 20020805 - start

			SIROPTMasterOptions(0, 12) = gPMConstants.SIRHiddenOptions.SIROPTFishInstalled

			SIROPTMasterOptions(1, 12) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 12) = ""

			SIROPTMasterOptions(3, 12) = ""

			SIROPTMasterOptions(4, 12) = "Fish Document Management Installed"

			SIROPTMasterOptions(5, 12) = "0 for OFF, 1 for ON"
			' CTAF 20020805 - end
			
			' ED 20020805 - start

			SIROPTMasterOptions(0, 13) = gPMConstants.SIRHiddenOptions.SIROPTAllowRegSearch

			SIROPTMasterOptions(1, 13) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 13) = ""

			SIROPTMasterOptions(3, 13) = ""

			SIROPTMasterOptions(4, 13) = "Registration Search on Find Insurance"

			SIROPTMasterOptions(5, 13) = "0 for OFF, 1 for ON"
			' ED 20020805 - end
			
			' DD 23/08/2002: Start

			SIROPTMasterOptions(0, 14) = gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting

			SIROPTMasterOptions(1, 14) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 14) = ""

			SIROPTMasterOptions(3, 14) = ""

			SIROPTMasterOptions(4, 14) = "Accounts uses multiple Structure Trees/Ledgers/Periods"

			SIROPTMasterOptions(5, 14) = "0 for OFF, 1 for ON"
			' DD 23/08/2002: end
			
			' PWF 03/09/2002: Start - Add missing entry

			SIROPTMasterOptions(0, 15) = gPMConstants.SIRHiddenOptions.SIROPTLossSchedule

			SIROPTMasterOptions(1, 15) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 15) = ""

			SIROPTMasterOptions(3, 15) = ""

			SIROPTMasterOptions(4, 15) = "Loss Schedule Functionality enabled"

			SIROPTMasterOptions(5, 15) = "0 for OFF, 1 for ON"
			' PWF 03/09/2002: end
			
			' PW160902: Start

			SIROPTMasterOptions(0, 16) = gPMConstants.SIRHiddenOptions.SIROPTRiskVariations

			SIROPTMasterOptions(1, 16) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 16) = ""

			SIROPTMasterOptions(3, 16) = ""

			SIROPTMasterOptions(4, 16) = "Allow creation of risk variations in NB and MTAs"

			SIROPTMasterOptions(5, 16) = "0 for OFF, 1 for ON"
			' PW160902: end
			
			'sj 23/09/2002 - start

			SIROPTMasterOptions(0, 17) = gPMConstants.SIRHiddenOptions.SIROPTAutomaticCreditProcessing

			SIROPTMasterOptions(1, 17) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 17) = ""

			SIROPTMasterOptions(3, 17) = ""

			SIROPTMasterOptions(4, 17) = "Automatic Credit Processing"

			SIROPTMasterOptions(5, 17) = "0 for OFF, 1 for ON"
			'sj 23/09/2002 - end
			
			'PWF 30/09/2002

			SIROPTMasterOptions(0, 18) = gPMConstants.SIRHiddenOptions.SIROPTAccountSegregation

			SIROPTMasterOptions(1, 18) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 18) = ""

			SIROPTMasterOptions(3, 18) = ""

			SIROPTMasterOptions(4, 18) = "Account Segregation"

			SIROPTMasterOptions(5, 18) = "0 for OFF, 1 for ON"
			
			'sj 04/10/2002 - start

			SIROPTMasterOptions(0, 19) = gPMConstants.SIRHiddenOptions.SIROPTHidePublicPrivateNotes

			SIROPTMasterOptions(1, 19) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 19) = ""

			SIROPTMasterOptions(3, 19) = ""

			SIROPTMasterOptions(4, 19) = "Hide Public/Private Notes"

			SIROPTMasterOptions(5, 19) = "0 for OFF, 1 for ON"
			'sj 04/10/2002 - end
			
			'PWF 04/10/2002

			SIROPTMasterOptions(0, 20) = gPMConstants.SIRHiddenOptions.SIROPTMultiBranchCoreAccounts

			SIROPTMasterOptions(1, 20) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 20) = ""

			SIROPTMasterOptions(3, 20) = ""

			SIROPTMasterOptions(4, 20) = "Enable Multi-Branch Core Accounts"

			SIROPTMasterOptions(5, 20) = "0 for OFF, 1 for ON"
			
			'TF101002 - 3 new options

			SIROPTMasterOptions(0, 21) = gPMConstants.SIRHiddenOptions.SIROPTAllowChangeOfNCDAtRenewal

			SIROPTMasterOptions(1, 21) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 21) = ""

			SIROPTMasterOptions(3, 21) = ""

			SIROPTMasterOptions(4, 21) = "Include Change Of NCD Option At Renewal Pre-Selection"

			SIROPTMasterOptions(5, 21) = "0 for OFF, 1 for ON"
			

			SIROPTMasterOptions(0, 22) = gPMConstants.SIRHiddenOptions.SIROPTIncludeInstalmentsOnRoadmap

			SIROPTMasterOptions(1, 22) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 22) = ""

			SIROPTMasterOptions(3, 22) = ""

			SIROPTMasterOptions(4, 22) = "Include Instalments Step On NB & Renewal Roadmaps"

			SIROPTMasterOptions(5, 22) = "0 for OFF, 1 for ON"
			

			SIROPTMasterOptions(0, 23) = gPMConstants.SIRHiddenOptions.SIROPTHidePolicyDatesOnSchemeSelectScreen

			SIROPTMasterOptions(1, 23) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 23) = ""

			SIROPTMasterOptions(3, 23) = ""

			SIROPTMasterOptions(4, 23) = "Hide Cover Start/Expiry Date Input On Scheme Select Screen"

			SIROPTMasterOptions(5, 23) = "0 for SHOW, 1 for HIDE"
			
			'PR 9/10/2002 - start

			SIROPTMasterOptions(0, 24) = gPMConstants.SIRHiddenOptions.SIROPTRemoveNone

			SIROPTMasterOptions(1, 24) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 24) = ""

			SIROPTMasterOptions(3, 24) = ""

			SIROPTMasterOptions(4, 24) = "Remove (None) option for Products"

			SIROPTMasterOptions(5, 24) = "0 for OFF, 1 for ON"
			'PR 9/10/2002 - end
			
			'sj 14/10/2002 - start
			' PW240403 - add further option setting to say if html is zipped or
			' not. Note the new HTML unzipped option has been added for IAG, and
			' this is being done on the proviso that all of their graphics
			' are linked from a global destination, so only the html will be
			' spooled, without any supporting files (Document Issuance changes)

			SIROPTMasterOptions(0, 25) = gPMConstants.SIRHiddenOptions.SIROPTSpoolAsHTML

			SIROPTMasterOptions(1, 25) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 25) = ""

			SIROPTMasterOptions(3, 25) = ""

			SIROPTMasterOptions(4, 25) = "Spool documents as HTML files"

			SIROPTMasterOptions(5, 25) = "0 for OFF, 1 for ON (zipped), 2 for ON (unzipped)"
			'sj 14/10/2002 - end
			
			'ED 31/10/2002 - start

			SIROPTMasterOptions(0, 26) = gPMConstants.SIRHiddenOptions.SIROptHideRoadmapLetterQuestion

			SIROPTMasterOptions(1, 26) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 26) = ""

			SIROPTMasterOptions(3, 26) = ""

			SIROPTMasterOptions(4, 26) = "Hide Roadmap letter question"

			SIROPTMasterOptions(5, 26) = "0 for OFF, 1 for ON"
			'ED 31/10/2002 - end
			
			'PW051102 - start

			SIROPTMasterOptions(0, 27) = gPMConstants.SIRHiddenOptions.SIROPTAdmiralForceRisks

			SIROPTMasterOptions(1, 27) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 27) = ""

			SIROPTMasterOptions(3, 27) = ""

			SIROPTMasterOptions(4, 27) = "Admiral Force Risks"

			SIROPTMasterOptions(5, 27) = "0 for OFF, 1 for ON"
			'PW051102 - end
			
			
			'    'ME 06-11-2002 - start
			'    'Product Option to switch between Salvage Recovery
			'    'and Manage Salvage Mode in claims
			'    SIROPTMasterOptions(0, 28) = SIROPTSwitchToManageSalvage
			'    SIROPTMasterOptions(1, 28) = SIRBCHHeadOffice
			'    SIROPTMasterOptions(2, 28) = ""
			'    SIROPTMasterOptions(3, 28) = ""
			'    SIROPTMasterOptions(4, 28) = "Switch To Manage Salvage Mode"
			'    SIROPTMasterOptions(5, 28) = "0 for OFF, 1 for ON"
			'    'ME 06-11-2002 - end
			
			'ED 19/11/2002 - start
			'PS224

			SIROPTMasterOptions(0, 28) = gPMConstants.SIRHiddenOptions.SIROPTLimitPersonalClientEditFields

			SIROPTMasterOptions(1, 28) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 28) = ""

			SIROPTMasterOptions(3, 28) = ""

			SIROPTMasterOptions(4, 28) = "Disable PersonalClient Edit Fields"

			SIROPTMasterOptions(5, 28) = "0 for OFF, 1 for ON"
			'ED 19/11/2002 - end
			
			'TF131102 - PS221
			'Set status to QUOTE following override of REFER/DECLINE

			SIROPTMasterOptions(0, 29) = gPMConstants.SIRHiddenOptions.SIROPTSetToQuoteAfterOverride

			SIROPTMasterOptions(1, 29) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 29) = ""

			SIROPTMasterOptions(3, 29) = ""

			SIROPTMasterOptions(4, 29) = "Set status to QUOTE following override of REFER/DECLINE"

			SIROPTMasterOptions(5, 29) = "0 for OFF, 1 for ON"
			
			'TR2211202 - TS23
			'Option to give user abitlity to Override Interest Rate and
			'Commissions Rate in Instalments

			SIROPTMasterOptions(0, 30) = gPMConstants.SIRHiddenOptions.SIROPTAllowInstalmentsOverride

			SIROPTMasterOptions(1, 30) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 30) = ""

			SIROPTMasterOptions(3, 30) = ""

			SIROPTMasterOptions(4, 30) = "Allow Interest Rate/Commssion Override?"

			SIROPTMasterOptions(5, 30) = "0 for OFF, 1 for ON"
			
			'ME 25-11-2002 - start
			'Product Option to switch between Salvage Recovery
			'and Manage Salvage Mode in claims

			SIROPTMasterOptions(0, 31) = gPMConstants.SIRHiddenOptions.SIROPTManageSalvage

			SIROPTMasterOptions(1, 31) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 31) = ""

			SIROPTMasterOptions(3, 31) = ""

			SIROPTMasterOptions(4, 31) = "Switch To Manage Salvage Mode"

			SIROPTMasterOptions(5, 31) = "0 for OFF, 1 for ON"
			'ME 25-11-2002 - end
			
			'ISS1498 11/12/02 - start
			'Product Option to switch on risk screen editing in client manager

			SIROPTMasterOptions(0, 32) = gPMConstants.SIRHiddenOptions.SIROPTEnableRiskScreenEditing

			SIROPTMasterOptions(1, 32) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 32) = ""

			SIROPTMasterOptions(3, 32) = ""

			SIROPTMasterOptions(4, 32) = "Enable editing of Risk Screen in Client Manager"

			SIROPTMasterOptions(5, 32) = "0 for OFF, 1 for ON"
			'ISS1498 11/12/02 - end
			
			' Alix 10/02/03 - Claim versioning

			SIROPTMasterOptions(0, 33) = gPMConstants.SIRHiddenOptions.SIROPTEnableClaimVersions

			SIROPTMasterOptions(1, 33) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 33) = ""

			SIROPTMasterOptions(3, 33) = ""

			SIROPTMasterOptions(4, 33) = "Enable the claim versions functions"

			SIROPTMasterOptions(5, 33) = "0 for OFF, 1 for ON"
			
			'CJR 8/1/2003 419 - Handling Disclosures

			SIROPTMasterOptions(0, 34) = gPMConstants.SIRHiddenOptions.SIROPTShareDisclosures

			SIROPTMasterOptions(1, 34) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 34) = ""

			SIROPTMasterOptions(3, 34) = ""

			SIROPTMasterOptions(4, 34) = "Share Disclosures"

			SIROPTMasterOptions(5, 34) = "0 for OFF, 1 for ON"
			
			'PWF 20/01/2003 - Enable branch select at logon

			SIROPTMasterOptions(0, 35) = gPMConstants.SIRHiddenOptions.SIROPTEnableBranchSelectAtLogon

			SIROPTMasterOptions(1, 35) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 35) = ""

			SIROPTMasterOptions(3, 35) = ""

			SIROPTMasterOptions(4, 35) = "Enable Branch selection dialog at logon"

			SIROPTMasterOptions(5, 35) = "0 for OFF, 1 for ON"
			
			'PWF 20/01/2003 - Enable branch select at logon

			SIROPTMasterOptions(0, 36) = gPMConstants.SIRHiddenOptions.SIROPTIsAUA

			SIROPTMasterOptions(1, 36) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 36) = ""

			SIROPTMasterOptions(3, 36) = ""

			SIROPTMasterOptions(4, 36) = "AUA Switch"

			SIROPTMasterOptions(5, 36) = "0 for OFF, 1 for ON"
			
			' PW180303 - Force Numeric Client Code
			' PS186

			SIROPTMasterOptions(0, 37) = gPMConstants.SIRHiddenOptions.SIROPTForceNumericClientCode

			SIROPTMasterOptions(1, 37) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 37) = ""

			SIROPTMasterOptions(3, 37) = ""

			SIROPTMasterOptions(4, 37) = "Force Numeric Client Code"

			SIROPTMasterOptions(5, 37) = "0 for OFF, 1 for ON"
			
			' SJP (CMG) 01042003 - Link Account Executive To Commission Account
			' PS235

			SIROPTMasterOptions(0, 38) = gPMConstants.SIRHiddenOptions.SIROPTLinkCommACCToACCExec

			SIROPTMasterOptions(1, 38) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 38) = ""

			SIROPTMasterOptions(3, 38) = ""

			SIROPTMasterOptions(4, 38) = "Link Commission Account to Account Executive rather than Risk Group"

			SIROPTMasterOptions(5, 38) = "0 for OFF, 1 for ON"
			
			' CTAF 20030410 - Cache Transdetail data

			SIROPTMasterOptions(0, 39) = gPMConstants.SIRHiddenOptions.SIROPTCacheTransDetail

			SIROPTMasterOptions(1, 39) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 39) = ""

			SIROPTMasterOptions(3, 39) = ""

			SIROPTMasterOptions(4, 39) = "Reload last data entered on transdetail screen."

			SIROPTMasterOptions(5, 39) = "0 for OFF, 1 for ON"
			
			' RDT 20030410 - Display Credit/Debit on FindTransactions

			SIROPTMasterOptions(0, 40) = gPMConstants.SIRHiddenOptions.SIROPTDisplayDebitCredit

			SIROPTMasterOptions(1, 40) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 40) = ""

			SIROPTMasterOptions(3, 40) = ""

			SIROPTMasterOptions(4, 40) = "Toggles the display of Credit/Debit columns on the FindTransactions screen."

			SIROPTMasterOptions(5, 40) = "0 for OFF, 1 for ON"
			
			' RAW 10/04/2003 : ISS3072 : added : control whether account handler parties can be assigned to a branch other than HO

			SIROPTMasterOptions(0, 41) = gPMConstants.SIRHiddenOptions.SIROPTAccountHandlerIsMultiBranch

			SIROPTMasterOptions(1, 41) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 41) = ""

			SIROPTMasterOptions(3, 41) = ""

			SIROPTMasterOptions(4, 41) = "Controls whether an account handler can be assigned to a branch other than HO"

			SIROPTMasterOptions(5, 41) = "0 for OFF, 1 for ON"
			
			' PW240403 - Non-Printing Graphics (Document Issuance changes)

			SIROPTMasterOptions(0, 42) = gPMConstants.SIRHiddenOptions.SIROPTNonPrintingGraphics

			SIROPTMasterOptions(1, 42) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 42) = ""

			SIROPTMasterOptions(3, 42) = ""

			SIROPTMasterOptions(4, 42) = "Suppress Non-Printing Graphics"

			SIROPTMasterOptions(5, 42) = "0 for OFF, 1 for ON"
			
			' PW240403 - Sirius View in Documaster (Document Issuance changes)

			SIROPTMasterOptions(0, 43) = gPMConstants.SIRHiddenOptions.SIROPTSiriusViewInDocumaster

			SIROPTMasterOptions(1, 43) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 43) = ""

			SIROPTMasterOptions(3, 43) = ""

			SIROPTMasterOptions(4, 43) = "Allow Sirius View functionality in Documaster"

			SIROPTMasterOptions(5, 43) = "0 for OFF, 1 for ON"
			
			'25/04/2003 - PWC - (408) User Definable (party) Fields

			SIROPTMasterOptions(0, 44) = gPMConstants.SIRHiddenOptions.SIROPTUserDefinablePartyFields

			SIROPTMasterOptions(1, 44) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 44) = ""

			SIROPTMasterOptions(3, 44) = ""

			SIROPTMasterOptions(4, 44) = "Allow User Definable (party) Fields"

			SIROPTMasterOptions(5, 44) = "0 for OFF, 1 for ON"
			
			'08/05/2003 - SW - Enable Different User Binder Payment

			SIROPTMasterOptions(0, 45) = gPMConstants.SIRHiddenOptions.SIROPTInsurerPaymentPayUserCheck

			SIROPTMasterOptions(1, 45) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 45) = ""

			SIROPTMasterOptions(3, 45) = ""

			SIROPTMasterOptions(4, 45) = "Insurer Payment Paying User Check"

			SIROPTMasterOptions(5, 45) = "0 for OFF, 1 for ON"
			
			'14/05/2003 - SJP (CMG) - Show Renewal Frequency During Schemes New Business

			SIROPTMasterOptions(0, 46) = gPMConstants.SIRHiddenOptions.SIROPTShowRenewalFrequencyForSchemesNB

			SIROPTMasterOptions(1, 46) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 46) = ""

			SIROPTMasterOptions(3, 46) = ""

			SIROPTMasterOptions(4, 46) = "Selection of renewal frequency for schemes NB policy"

			SIROPTMasterOptions(5, 46) = "0 for OFF, 1 for ON"
			
			'MEvans : 27-05-2003 : 223 - Third Party Debt Recovery

			SIROPTMasterOptions(0, 47) = gPMConstants.SIRHiddenOptions.SIROPTThirdPartyRecovery

			SIROPTMasterOptions(1, 47) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 47) = ""

			SIROPTMasterOptions(3, 47) = ""

			SIROPTMasterOptions(4, 47) = "Third party debt recovery is available from claims risk screen"

			SIROPTMasterOptions(5, 47) = "0 for OFF, 1 for ON"
			
			'DD 28/05/2003: New option for generation of Credit Control for each Instalment
			'in advance

			SIROPTMasterOptions(0, 48) = gPMConstants.SIRHiddenOptions.SIROPTGenerateAdvanceCreditControlForInstalments

			SIROPTMasterOptions(1, 48) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 48) = ""

			SIROPTMasterOptions(3, 48) = ""

			SIROPTMasterOptions(4, 48) = "Generate Credit Control Items for each Instalment on a Plan."

			SIROPTMasterOptions(5, 48) = "0 for OFF, 1 for ON"
			
			'AK 16/06/2003 : New option for claims authority scripts

            'SIROPTMasterOptions(0, 49) = gPMConstants.SIRHiddenOptions.SIROPTRunClaimsAuthorisationScript

			SIROPTMasterOptions(1, 49) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 49) = ""

			SIROPTMasterOptions(3, 49) = ""

			SIROPTMasterOptions(4, 49) = "Run Authorisation scripts for claim payments."

			SIROPTMasterOptions(5, 49) = "0 for OFF, 1 for ON"
			
			'AK 09072003 - various product options for IAG Application security process PS#246 -  begin
			
			'For ability to select default subbranch against each branch

			SIROPTMasterOptions(0, 50) = gPMConstants.SIRHiddenOptions.SIROPTDefaultSubBranch

			SIROPTMasterOptions(1, 50) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 50) = ""

			SIROPTMasterOptions(3, 50) = ""

			SIROPTMasterOptions(4, 50) = "Select default subbranch against each branch."

			SIROPTMasterOptions(5, 50) = "0 for OFF, 1 for ON"
			
			'for limiting policies to client source

			SIROPTMasterOptions(0, 51) = gPMConstants.SIRHiddenOptions.SIROPTLimitPolicySource

			SIROPTMasterOptions(1, 51) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 51) = ""

			SIROPTMasterOptions(3, 51) = ""

			SIROPTMasterOptions(4, 51) = "Limit policies to client source"

			SIROPTMasterOptions(5, 51) = "0 for OFF, 1 for ON"
			
			'for by default no branch access to users

			SIROPTMasterOptions(0, 52) = gPMConstants.SIRHiddenOptions.SIROPTNoBranchAccess

			SIROPTMasterOptions(1, 52) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 52) = ""

			SIROPTMasterOptions(3, 52) = ""

			SIROPTMasterOptions(4, 52) = "Start with No-Branch-access to users"

			SIROPTMasterOptions(5, 52) = "0 for OFF, 1 for ON"
			
			'for User group / branch relationship

			SIROPTMasterOptions(0, 53) = gPMConstants.SIRHiddenOptions.SIROPTUserGroupBranchLink

			SIROPTMasterOptions(1, 53) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 53) = ""

			SIROPTMasterOptions(3, 53) = ""

			SIROPTMasterOptions(4, 53) = "Allow UserGroup / Branch relationship."

			SIROPTMasterOptions(5, 53) = "0 for OFF, 1 for ON"
			
			'for branch specific risks

			SIROPTMasterOptions(0, 54) = gPMConstants.SIRHiddenOptions.SIROPTBranchSpecificRisks

			SIROPTMasterOptions(1, 54) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 54) = ""

			SIROPTMasterOptions(3, 54) = ""

			SIROPTMasterOptions(4, 54) = "Allow Risk type / Branch relationship"

			SIROPTMasterOptions(5, 54) = "0 for OFF, 1 for ON"
			
			'JJ 10/07/2003 for mandatory business option of partycc screen

			SIROPTMasterOptions(0, 55) = gPMConstants.SIRHiddenOptions.SIROPTBusinessFieldOnClientIsMandatory

			SIROPTMasterOptions(1, 55) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 55) = ""

			SIROPTMasterOptions(3, 55) = ""

			SIROPTMasterOptions(4, 55) = "Business field is mandatory for corporate clients"

			SIROPTMasterOptions(5, 55) = "0 for OFF, 1 for ON"
			
			'DC110903 added but cannot fill all info as didnt add option

			SIROPTMasterOptions(0, 56) = gPMConstants.SIRHiddenOptions.SIROPTSubBranchShowingForBroking

			SIROPTMasterOptions(1, 56) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 56) = ""

			SIROPTMasterOptions(3, 56) = ""

			SIROPTMasterOptions(4, 56) = "Show Sub-Branch on Broking Party screens"

			SIROPTMasterOptions(5, 56) = ""
			
			' JJ 11/07/2003

			SIROPTMasterOptions(0, 57) = gPMConstants.SIRHiddenOptions.SIROPTAONPRClientScreenChanges

			SIROPTMasterOptions(1, 57) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 57) = ""

			SIROPTMasterOptions(3, 57) = ""

			SIROPTMasterOptions(4, 57) = "AON PR Client Screen Changes"

			SIROPTMasterOptions(5, 57) = "0 for OFF, 1 for ON"
			
			'DC110903 added but cannot fill all info as didnt add option

			SIROPTMasterOptions(0, 58) = gPMConstants.SIRHiddenOptions.SIROPTInstalmentDisplayStyle

			SIROPTMasterOptions(1, 58) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 58) = ""

			SIROPTMasterOptions(3, 58) = ""

			SIROPTMasterOptions(4, 58) = "Instalment Display Style"

			SIROPTMasterOptions(5, 58) = "0 for Standard, 1 for Grid Style"
			
			'DC110903 -PS253 -FSA Compliance

			SIROPTMasterOptions(0, 59) = gPMConstants.SIRHiddenOptions.SIROPTEnableFSACompliance

			SIROPTMasterOptions(1, 59) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 59) = ""

			SIROPTMasterOptions(3, 59) = ""

			SIROPTMasterOptions(4, 59) = "Enable FSA Compliance"

			SIROPTMasterOptions(5, 59) = "0 for OFF, 1 for ON"
			
			'DC110903 added but cannot fill all info as didnt add option

			SIROPTMasterOptions(0, 60) = gPMConstants.SIRHiddenOptions.SIROPTWMAutoRefreshEnabled

			SIROPTMasterOptions(1, 60) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 60) = ""

			SIROPTMasterOptions(3, 60) = ""

			SIROPTMasterOptions(4, 60) = "WorkManager Auto Refresh"

			SIROPTMasterOptions(5, 60) = ""
			
			'RSB010903 (sp#246)
			
			'Windows Unified Login -This option need not to be shown in Product Option thus
			'commented below mentioned lines. This option can be change from User maintenance
			'where proper validation can be done before switching System Security Model.
			'SIROPTMasterOptions(0, 63) = SIROPTAlternativeLogon
			'SIROPTMasterOptions(1, 63) = SIRBCHHeadOffice
			'SIROPTMasterOptions(2, 63) = ""
			'SIROPTMasterOptions(3, 63) = ""
			'SIROPTMasterOptions(4, 63) = "System Security Model"
			'SIROPTMasterOptions(5, 63) = "0 for Standard Sirius Logins, " & _
			'"1 for Mixed Mode, " & _
			'"2 for Unified Only (Windows)"
			
			' RDC 13112003

			SIROPTMasterOptions(0, 64) = gPMConstants.SIRHiddenOptions.SIROPTAllowElectronicPayment

			SIROPTMasterOptions(1, 64) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 64) = ""

			SIROPTMasterOptions(3, 64) = ""

			SIROPTMasterOptions(4, 64) = "Allow Electronic Payments"

			SIROPTMasterOptions(5, 64) = "0 for OFF, 1 for ON"
			
			' START CHANGES - Changed By: AAB  - Changed On: 02-Dec-2003 10:16
			' Added to support ICB - RFC

			SIROPTMasterOptions(0, 65) = gPMConstants.SIRHiddenOptions.SIROPTMultiStepApproval

			SIROPTMasterOptions(1, 65) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 65) = ""

			SIROPTMasterOptions(3, 65) = ""

			SIROPTMasterOptions(4, 65) = "Allow for Multi-Step Approval Process"

			SIROPTMasterOptions(5, 65) = "0 for OFF, 1 for ON"
			' END CHANGES - Changed By: AAB  - Changed On: 02-Dec-2003 10:16
			

			SIROPTMasterOptions(0, 66) = gPMConstants.SIRHiddenOptions.SIROOPTNoDeletedTempates

			SIROPTMasterOptions(1, 66) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 66) = ""

			SIROPTMasterOptions(3, 66) = ""

			SIROPTMasterOptions(4, 66) = "Exclude Deleted Templates in Find: Document Template Screen"

			SIROPTMasterOptions(5, 66) = "0 for OFF, 1 for ON"
			
			'SJ 17/02/2004 - start

			SIROPTMasterOptions(0, 67) = gPMConstants.SIRHiddenOptions.SIROPTUnderwritingBranchEnabled

			SIROPTMasterOptions(1, 67) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 67) = ""

			SIROPTMasterOptions(3, 67) = ""

			SIROPTMasterOptions(4, 67) = "Allow branches to act as underwriters/insurers in a broking environment"

			SIROPTMasterOptions(5, 67) = "0 for OFF, 1 for ON"
			'SJ 17/02/2004 - end
			
			'DD 25/03/2004 - start

			SIROPTMasterOptions(0, 68) = gPMConstants.SIRHiddenOptions.SIROPTUnderwritingYear

			SIROPTMasterOptions(1, 68) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 68) = ""

			SIROPTMasterOptions(3, 68) = ""

			SIROPTMasterOptions(4, 68) = "Enable Underwriting Year labelling"

			SIROPTMasterOptions(5, 68) = "0 for OFF, 1 for ON"
			'DD 25/03/2004 - end
			

			SIROPTMasterOptions(0, 69) = gPMConstants.SIRHiddenOptions.SIROPTHoldCoverExpiryDate

			SIROPTMasterOptions(1, 69) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 69) = ""

			SIROPTMasterOptions(3, 69) = ""

			SIROPTMasterOptions(4, 69) = "Enable Hold Cover Expiry Date field"

			SIROPTMasterOptions(5, 69) = "0 for OFF, 1 for ON"
			
			' CLG 01/09/2004 : RFC71 Added product option to allow spool AND archive of documents to be selected in document link

			SIROPTMasterOptions(0, 70) = gPMConstants.SIRHiddenOptions.SIROPTAllowSpoolAndArchive

			SIROPTMasterOptions(1, 70) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 70) = ""

			SIROPTMasterOptions(3, 70) = ""

			SIROPTMasterOptions(4, 70) = "Allow both spool and archive to be selected in document link"

			SIROPTMasterOptions(5, 70) = "0 for exclusive (standard), 1 for allow both"
			

			SIROPTMasterOptions(0, 71) = gPMConstants.SIRHiddenOptions.SIROPTAllowPartialAllocationOnInsurer

			SIROPTMasterOptions(1, 71) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 71) = ""

			SIROPTMasterOptions(3, 71) = ""

			SIROPTMasterOptions(4, 71) = "Allow Partial Allocation on Insurer Accounts"

			SIROPTMasterOptions(5, 71) = "0 Not allowed (default), 1 for Allow"
			

			SIROPTMasterOptions(0, 72) = gPMConstants.SIRHiddenOptions.SIROPTEnhancedChequeProduction

			SIROPTMasterOptions(1, 72) = SIRBCHHeadOffice

			SIROPTMasterOptions(2, 72) = ""

			SIROPTMasterOptions(3, 72) = ""

			SIROPTMasterOptions(4, 72) = "Enhanced Cheque Production"

			SIROPTMasterOptions(5, 72) = "0 for Not Enabled (default), 1 for Enabled"
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="populateArray failed", vApp:="gSIRConstants", vClass:="GSirConstants", vMethod:="populateArray", excep:=excep)
			
			Return result
		End Try
	End Function
	
	' ******************************************************'
	'
	' Name: SIRDateType
	'
	' Description: This will convert a date
	' History: 18/6/02 - converted to a BAS file
	'
	'*******************************************************'
	Function SIRDateType(ByVal v_vDate As Date) As SIREDateType
		
		Dim result As SIREDateType = SIREDateType.sireValidDate
		Try 
			

			
			If Convert.IsDBNull(v_vDate) Or IsNothing(v_vDate) Then
				Return SIREDateType.sireNullDate
			Else
				If Information.IsDate(v_vDate) Then
					If v_vDate < SIRSystemLowDate Then
						Return SIREDateType.sireNullDate
					Else
						Return SIREDateType.sireValidDate
					End If
				Else
					Return SIREDateType.sireInvalidDate
				End If
			End If
		
		Catch 
		End Try
		
		
		
		
		' Error Section.
		result = SIREDateType.sireInvalidDate
		
		Throw New System.Exception(Information.Err().Number.ToString() + ", " + Information.Err().Source + ", " + Information.Err().Description)
		
		Return result
		
	End Function
	
	
	' ***************************************************************** '
	' Name: FindVarField
	'
	' Description: Finds the Position of a Variable Data Field within
	'              a Variable Data Block
	'
	' ***************************************************************** '
	Public Function FindVarField(ByRef sRecordName As String, ByRef sFieldName As String, ByRef vVarDataBlock( ,  ) As Object, ByRef lPosition As Integer) As Integer
		
		Dim result As Integer = 0
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Check that Var Data Block is an Array
			If Not Information.IsArray(vVarDataBlock) Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Check that there is a record & field name to search for
			If (sFieldName.Trim() = "") Or (sRecordName.Trim() = "") Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Search for Field in Array using RecordName & FieldName
			' Convert both to Upper to avoid any case mistakes.
			For l_Row As Integer = vVarDataBlock.GetLowerBound(1) To vVarDataBlock.GetUpperBound(1)

				If (sRecordName.Trim() & sFieldName.Trim()).ToUpper() = (CStr(vVarDataBlock(gPMConstants.PMEVarDataArrayColPos.PMVarRecordName, l_Row)).Trim() & CStr(vVarDataBlock(gPMConstants.PMEVarDataArrayColPos.PMVarFieldName, l_Row)).Trim()).ToUpper() Then
					lPosition = l_Row
					Return result
				End If
			Next l_Row
			
			' Field Not Found so return error
			
			Return gPMConstants.PMEReturnCode.PMFalse
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			result = gPMConstants.PMELogLevel.PMLogOnError
			
			
			Throw New System.Exception(Information.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message)
			
			Return result
			
		End Try
	End Function
End Module