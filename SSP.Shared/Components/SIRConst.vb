Option Strict Off
Option Explicit On
Imports System
<System.Runtime.InteropServices.ProgId("SIRConst_NET.SIRConst")> _
 Public Module SIRConst
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Name: SIRConst
	'
	' Date: 21/10/98
	'
	' Description: Sirius general constants module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	'JSB 10/03/03 - Following already declared in gSIRLibrary
	''Solution Codes
	'Public Const SIRCoreSolution = "SIR"
	'Public Const SIRMBPSolution = "MBP"
	'Public Const SIRPMBSolution = "PMB"
	'Public Const SIRGEMSolution = "GEM" 'sj 18/10/99
	
	'General Party type codes - not to be confused with the database column,
	'party type, which denotes a specific party -ie School, Nursing Home etc
	
	'IJR 2002-08-30 Start
	'In gPMConstants.bas
	'Public Const SIRPartyTypePersonalClient = "PC"
	'Public Const SIRPartyTypeAgent = "AG"
	'Public Const SIRPartyTypeCorporateClient = "CC"
	'Public Const SIRPartyTypeGroupClient = "GC"
	''ECK 20/7/99
	'Public Const SIRPartyTypeInsurer = "IN"
	''CF 13/08/99
	'Public Const SIRPartyTypeAccountHandler = "AH"
	''EK 12/10/99
	'Public Const SIRPartyTypeConsultant = "CO"
	''EK 9/11/99
	'Public Const SIRPartyTypeExtra = "EX"
	'Public Const SIRPartyTypeFee = "FE"
	'Public Const SIRPartyTypeDiscount = "DI"
	''EK 27/1/00
	'Public Const SIRPartyTypeCommission = "CM"
	''Tomo260500
	'Public Const SIRPartyTypeNetClient = "NC"
	'' TF161000
	'Public Const SIRPartyTypeFinanceProvider = "FP"
	'Public Const SIRPartyTypeBroker = "BR"
	'
	''RWH(23/07/01) Other Party Type.
	'Public Const SIRPartyTypeOther = "OT"
	'
	''General Party type descriptions
	Public Const SIRPartyTypePersonalClientText As String = "Personal Client" 'gp20021001 uncommented as needed by iPMBPartyConvert
	Public Const SIRPartyTypeAgentText As String = "Agent" 'gp20021001 uncommented as needed by iPMBPartyConvert
	Public Const SIRPartyTypeCorporateClientText As String = "Corporate Client" 'gp20021001 uncommented as needed by iPMBPartyConvert
	Public Const SIRPartyTypeGroupClientText As String = "Group Client" 'gp20021001 uncommented as needed by iPMBPartyConvert
	'ISS1118 add new constants for codes
	Public Const SIRPartyTypePersonalClientCode As String = "PC"
	Public Const SIRPartyTypeAgentCode As String = "AG"
	Public Const SIRPartyTypeCorporateClientCode As String = "CC"
	Public Const SIRPartyTypeGroupClientCode As String = "GC"
	
	''ECK 20/7/99
	'Public Const SIRPartyTypeInsurerText = "Insurer"
	''CF 13/08/99
	'Public Const SIRPartyTypeAccountHandlerText = "Account Handler"
	''EK 9/11/99
	'Public Const SIRPartyTypeExtraText = "Extra"
	'Public Const SIRPartyTypeFeeText = "Fee"
	'Public Const SIRPartyTypeDiscountText = "Discount"
	''EK 27/1/00
	'Public Const SIRPartyTypeCommissionText = "Commission"
	''Tomo260500
	'Public Const SIRPartyTypeNetClientText = "Net Client"
	'' TF161000
	'Public Const SIRPartyTypeFinanceProviderText = "Finance Provider"
	'Public Const SIRPartyTypeBrokerText = "Broker"
	'Public Const SIRPartyTypeConsultantText = "Consultant"
	'
	''Private Party Individual Code
	'Public Const SIRPartyTypePrivateIndividual = "PRIVATE"
	''Agent/Broker Intermediary Code
	'Public Const SIRPartyTypeBrokerIntermediary = "AGENT"
	'IJR 2002-08-30 End
	
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
	'IJR 2002-08-30 Start
	'In gPMConstants.bas
	'Public Const SIRLookupPartyAgentOrigin = "party_agent_origin"
	'Public Const SIRLookupPartyGroupType = "party_group_type"
	'Public Const SIRLookupPartyTrade = "party_trade"
	''Added by ECK 26/04/99
	'Public Const SIRLookupPartyBusiness = "party_business"
	'Public Const SIRLookupArea = "area"
	'Public Const SIRLookupCurrency = "currency"
	'Public Const SIRLookupReminderType = "reminder_type"
	'Public Const SIRLookupServiceLevel = "service_level"
	'Public Const SIRLookupSeasonalGift = "seasonal_gift"
	'Public Const SIRLookupNationality = "nationality"
	'Public Const SIRLookupProspectStatus = "prospect_status"
	'Public Const SIRLookupPartyType = "party_type"
	'Public Const SIRLookupPolicyType = "policy_type"
	'Public Const SIRLookupInsuranceFileStatus = "insurance_file_status"
	'Public Const SIRLookupRenewalFrequency = "renewal_frequency"
	'Public Const SIRLookupRenewalStopCode = "renewal_stop_code"
	'Public Const SIRLookupPolicyRelationshipType = "policy_relationship_type"
	'Public Const SIRLookupLapsedReason = "lapsed_reason"
	'Public Const SIRLookupRiskCode = "risk_code"
	'Public Const SIRLookupAnalysisCode = "analysis_code"
	''ECK 05/08/99
	'Public Const SIRLookupTransactionType = "transaction_type"
	''EK 130300
	'Public Const SIRLookupPostingTypeType = "posting_type"
	''EK 7/10/99
	'Public Const SIRLookupRiskGroup = "risk_group"
	''eck120500
	'Public Const SIRLookupBranch = "source"
	''Tomo270300
	'Public Const SIRLookupStrengthCode = "strength_code"
	'Public Const SIRLookupSICCode = "SIC_code"
	'' PW110702
	'Public Const SIRLookupPaymentMethod = "payment_method"
	'Public Const SIRLookupPaymentFrequency = "payment_frequency"
	'Public Const SIRLookupAddressOnNotice = "agent_address_usage_type"
	'Public Const SIRLookupWithholdingTaxType = "withholding_tax_type"
	'IJR 2002-08-30 End
	
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
	Public Const SIRInsFileTypeMTAQuote As String = "MTAQUOTE"
	Public Const SIRInsFileTypeMTAPermanent As String = "MTA PERM"
	Public Const SIRInsFileTypeMTATemporary As String = "MTA TEMP"
	Public Const SIRInsFileTypeMTAIncomplete As String = "MTA INCOMP"
	
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
	
	Public Const SIRTransCodeDescNewBusiness As String = "New Business"
	Public Const SIRTransCodeDescAdditionalPremium As String = "Additional Premium"
	Public Const SIRTransCodeDescReturnPremium As String = "Return Premium"
	Public Const SIRTransCodeDescRenewal As String = "Renewal"
	Public Const SIRTransCodeDescClaimOpen As String = "Claim Open"
	Public Const SIRTransCodeDescClaimRevision As String = "Claim Revision"
	Public Const SIRTransCodeDescClaimPaid As String = "Claim Paid"
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
	
	' Reinsurance Constants
	
	' Column Positions for RI Arrangement Lines Array
	Public Const SIRRITreatyCode As Integer = 0
	Public Const SIRRIDefaultSharePercent As Integer = 1
	Public Const SIRRIThisSharePercent As Integer = 2
	Public Const SIRRISumInsured As Integer = 3
	Public Const SIRRIPremiumValue As Integer = 4
	Public Const SIRRICommissionPercent As Integer = 5
	Public Const SIRRICommissionValue As Integer = 6
	Public Const SIRRIAgreementCode As Integer = 7
	Public Const SIRRIMethod As Integer = 8
	Public Const SIRRIArrangementLineID As Integer = 9
	Public Const SIRRIFacArrangementSummaryID As Integer = 10
	Public Const SIRRIOriginalFlag As Integer = 11
	
	Public Const SIRRIMax As Integer = 11
	
	
	' Column Positions for RI FAC Arrangement Array
	Public Const SIRFacPartyShortName As Integer = 0
	Public Const SIRFacBidSharePercent As Integer = 1
	Public Const SIRFacThisSharePercent As Integer = 2
	Public Const SIRFacSumInsured As Integer = 3
	Public Const SIRFacPremiumValue As Integer = 4
	Public Const SIRFacCommissionPercent As Integer = 5
	Public Const SIRFacCommissionValue As Integer = 6
	Public Const SIRFacAgreementCode As Integer = 7
	Public Const SIRFacPartyCnt As Integer = 8
	
	' FAC Summary Line Code
	Public Const SIRFacSummaryCode As String = "FAC"
	
	' Rounding Correction Factor for Four Decimal Places
	Public Const SIR4DPRoundFactor As Double = 0.000049
End Module