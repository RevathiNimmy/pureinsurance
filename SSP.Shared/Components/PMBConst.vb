Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("PMBConst_NET.PMBConst")>
Public Module PMBConst
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Name: PMBConst
    '
    ' Date: 22/04/99
    '
    ' Description: Sirius for Broking general constants module.
    '
    ' Edit History:
    ' ***************************************************************** '


    'General Party type codes - Some of these may already be SIRConst,
    'but they're all here to keep them together.
    Public Const PMBPartyTypePersonalClient As String = "PC"
    Public Const PMBPartyTypeAgent As String = "AG"
    'CMG/PB 18072002 Agent Groups
    Public Const PMBPartyTypeAgentGroup As String = "AGG"
    Public Const PMBPartyTypeSubAgent As String = "UB"
    Public Const PMBPartyTypeCorporateClient As String = "CC"
    Public Const PMBPartyTypeGroupClient As String = "GC"
    Public Const PMBPartyTypeConsultant As String = "CO"
    Public Const PMBPartyTypeAccountHandler As String = "AH"
    Public Const PMBPartyTypeInsurer As String = "IN"
    Public Const PMBPartyTypeBroker As String = "BR"
    Public Const PMBPartyTypeFee As String = "FE"
    Public Const PMBPartyTypeExtra As String = "EX"
    Public Const PMBPartyTypeDiscount As String = "DI"
    Public Const PMBPartyTypeCommissionAccount As String = "CM"
    'RWH(04/07/2000) RSAIB Process 007
    Public Const PMBPartyTypeOther As String = "OT"
    'TF181000
    Public Const PMBPartyTypeFinanceProvider As String = "FP"
    'DC151003 -PN7449 -new party type
    Public Const PMBPartyTypeExecutiveHandler As String = "HC"
    'DC101204
    Public Const PMBPartyTypeIntroducer As String = "TR"
    'PM31102006
    Public Const PMBPartyTypeIntermediary As String = "IM"
    Public Const PMBPartyTypeReassured As String = "Reassured"

    'General Party type descriptions
    Public Const PMBPartyTypePersonalClientText As String = "Personal Client"
    Public Const PMBPartyTypeAgentText As String = "Agent"
    Public Const PMBPartyTypeAgentGroupText As String = "Agent Group"
    Public Const PMBPartyTypeCorporateClientText As String = "Corporate Client"
    Public Const PMBPartyTypeGroupClientText As String = "Group Client"
    Public Const PMBPartyTypeConsultantText As String = "Account Executive"
    Public Const PMBPartyTypeAccountHandlerText As String = "Account Handler"
    Public Const PMBPartyTypeInsurerText As String = "Insurer"
    Public Const PMBPartyTypeBrokerText As String = "Broker"
    Public Const PMBPartyTypeFeeText As String = "Fee Account"
    Public Const PMBPartyTypeExtraText As String = "Extra Account"
    Public Const PMBPartyTypeDiscountText As String = "Discount Account"
    Public Const PMBPartyTypeCommissionAccountText As String = "Commission Account"
    Public Const PMBPartyTypeThirdPartyAgentText As String = "TPA"
    'RWH(04/07/2000) RSAIB Process 007
    Public Const PMBPartyTypeOtherText As String = "Other Parties"
    'TN20000918
    Public Const PMBPartyTypeReinsurerText As String = "Reinsurer"
    'TF181000
    Public Const PMBPartyTypeCoinsurerText As String = "Coinsurer"
    Public Const PMBPartyTypeFinanceProviderText As String = "Finance Provider"
    'DC151003 -PN7449 -new party type
    Public Const PMBPartyTypeExecutiveHandlerText As String = "Executive Handler"
    'DC101204
    Public Const PMBPartyTypeIntroducerText As String = "Introducer"
    'PM31102006
    Public Const PMBPartyTypeIntermediaryText As String = "Intermediary"

    'General Party prospect type codes
    Public Const PMBProspectTypeClient As String = "P"
    Public Const PMBProspectTypeProspect As String = "C"

    'General Party prospect type descriptions
    Public Const PMBProspectTypeClientText As String = "Client"
    Public Const PMBProspectTypeProspectText As String = "Prospect"

    'Events
    Public Const PMBEventNewClient As Integer = 1
    Public Const PMBEventNewPolicy As Integer = 2
    Public Const PMBEventNewClaim As Integer = 3
    Public Const PMBEventAddChange As Integer = 4
    Public Const PMBEventPolChange As Integer = 5
    Public Const PMBEventClaChange As Integer = 6
    Public Const PMBEventDelClient As Integer = 7
    Public Const PMBEventDelPolicy As Integer = 8
    Public Const PMBEventDelClaim As Integer = 9
    Public Const PMBEventDocument As Integer = 10
    Public Const PMBEventReport As Integer = 11
    Public Const PMBEventMailshot As Integer = 12
    Public Const PMBEventTransaction As Integer = 13
    ' RAW 22/09/2003 : CQ1864 : added
    Public Const PMBEventRenewal As Integer = 14
    Public Const PMBEventLSDepreciationChange As Integer = 15
    Public Const PMBEventLSDefSupplierChange As Integer = 16
    Public Const PMBEventLSAmountChange As Integer = 17
    Public Const PMBEventMTAPolicyReason As Integer = 18
    Public Const PMBEventLSDefTaxBandChange As Integer = 19
    Public Const PMBEventClientNotes As Integer = 20
    Public Const PMBEventAccountNotes As Integer = 21
    Public Const PMBEventClaimsNotes As Integer = 22
    Public Const PMBEventPolicyNotes As Integer = 23
    Public Const PMBEventLoyalty As Integer = 24
    Public Const PMBEventClaimDebtNotes As Integer = 25
    Public Const PMBEventClaimTask As Integer = 26
    Public Const PMBEventClaimDebtTask As Integer = 27
    Public Const PMBEventClientTask As Integer = 28
    Public Const PMBEventPolicyTask As Integer = 29
    Public Const PMBEventFSANotes As Integer = 30
    ' RAW 22/09/2003 : CQ1864 : end

    ' RAW 22/09/2003 : CQ1864 : added
    Public Const PMBEventGroupClient As Integer = 1
    Public Const PMBEventGroupPolicy As Integer = 2
    Public Const PMBEventGroupClaim As Integer = 3
    Public Const PMBEventGroupMailshot As Integer = 4
    Public Const PMBEventGroupReport As Integer = 5
    Public Const PMBEventGroupDocument As Integer = 6
    Public Const PMBEventGroupTransaction As Integer = 7
    Public Const PMBEventGroupRenewal As Integer = 8
    Public Const PMBEventGroupClientNotes As Integer = 9
    Public Const PMBEventGroupAccountNotes As Integer = 10
    Public Const PMBEventGroupClaimNotes As Integer = 11
    Public Const PMBEventGroupPolicyNotes As Integer = 12
    Public Const PMBEventGroupClaimDebtNotes As Integer = 13
    ' RAW 22/09/2003 : CQ1864 : end


    'Event State
    Public Const PMBRaiseEventInChildObject As Integer = 1
    Public Const PMBRaiseEventInParentObject As Integer = 2

    'Document Types
    Public Const PMBClientTextFile As Integer = 1
    Public Const PMBPolicyTextFile As Integer = 2
    Public Const PMBClaimTextFile As Integer = 3
    Public Const PMBClauseTextFile As Integer = 7

    'Policy Types
    Public Const PMBPolicyTypeSwift As Integer = 1
    Public Const PMBPolicyTypeGIIMotor As Integer = 2
    Public Const PMBPolicyTypeGeneral As Integer = 3
    Public Const PMBPolicyTypeGIIHousehold As Integer = 4
    Public Const PMBPolicyTypeUnderwriting As Integer = 5 'CT 14/09/00
    ' 25/04/2001 PSA - Start
    Public Const PMBPolicyTypeGIICommercialVehicle As Integer = 6
    ' 25/04/2001 PSA - End
    'DN 27/06/01 - Re-added Gemini I type, this time as const 9
    Public Const PMBPolicyTypeGemini As Integer = 9
    'ED 15102002 - New Type added
    Public Const PMBPolicyTypeSchemes As Integer = 10


    'Agent Types
    Public Const PMBAgentTypeBroker As Integer = 1
    Public Const PMBAgentTypeSubAgent As Integer = 2
    Public Const PMBAgentTypeCommAccount As Integer = 3
    Public Const PMBAgentTypeIntermediary As Integer = 5

    'DC101204
    Public Const PMBAgentTypeIntroducer As Integer = 3

    'Agent Type Descriptions
    Public Const PMBAgentTypeBrokerText As String = "Broker"
    Public Const PMBAgentTypeSubAgentText As String = "Sub-Agent"
    Public Const PMBAgentTypeCommAccountText As String = "Commission Account"
    Public Const PMBAgentTypeIntroducerText As String = "Introducer"
    Public Const PMBAgentTypeIntermediaryText As String = "Intermediary"


    'RWH(01/08/2000) Lookup List Constants. (RSAIB Process 005).
    Public Const PMBListGenderText As String = "Gender"
    Public Const PMBListMaritalStatusText As String = "Marital Status"
    Public Const PMBListEmploymentText As String = "Employment"
    Public Const PMBListBusinessText As String = "Business"
    Public Const PMBListOccupationText As String = "Occupation"
    Public Const PMBListTitleText As String = "Title"
    Public Const PMBListPaymentMethodText As String = "Payment Method"
    Public Const PMBListAlcoholText As String = "Alcoholic Measure"
    Public Const PMBListConvTypeText As String = "Conviction Type"
    Public Const PMBListConvStatusText As String = "Conviction Status"
    Public Const PMBListConvSentenceText As String = "Conviction Sentence"
    Public Const PMBListTimeUnitText As String = "Time Unit"

    Public Const PMBListGenderCode As Integer = 131091
    Public Const PMBListMaritalStatusCode As Integer = 131107
    Public Const PMBListEmploymentCode As Integer = 2228230
    Public Const PMBListBusinessCode As Integer = 2228228
    Public Const PMBListOccupationCode As Integer = 2228226
    Public Const PMBListTitleCode As Integer = 131085
    Public Const PMBListPaymentMethodCode As Integer = 6946819
    Public Const PMBListAlcoholCode As Integer = 1114126
    Public Const PMBListConvTypeCode As Integer = 1114113
    Public Const PMBListConvStatusCode As Integer = 1114124
    Public Const PMBListConvSentenceCode As Integer = 1114119
    Public Const PMBListTimeUnitCode As Integer = 1114122

    'RWH(16/11/2000) Renewal criteria descriptions for Renewal_report.
    Public Const PMAutoRenewalDesc As String = "Auto-renewal flag not set"
    Public Const PMPartyRenewalStopDesc As String = "Party renewal stop code"
    Public Const PMPolicyRenewalStopDesc As String = "Policy renewal stop code"
    Public Const PMReferredAtRenewalDesc As String = "Referred at renewal"
    Public Const PMClaimsMadeDesc As String = "Failed claims criteria"
    Public Const PMFailedIndexLinkDesc As String = "Failed index-linking"
    Public Const PMFailedReRateDesc As String = "Failed re-rating"

    Public Const PMBAgentTypeAgentText As String = "Agent"

    'PM23022007
    Public Const PMBAutoCancelLapsedCode As String = "CCNTRL"
End Module