Option Strict Off
Option Explicit On
Imports System
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 23/06/1998
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' RAW 18/11/2002 : PS005 : Add tab6 for customer loyalty scheme
	' RKS 24/01/2005 : Added constants for option 600
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "uctPartyGC"
	
	Public Const AddressImage As String = "AddressImage"
	Public Const ContactImage As String = "ContactImage"
	Public Const LifestyleImage As String = "LifestyleImage"
	Public Const ConvictionImage As String = "ConvictionImage"
	Public Const CampaignImage As String = "CampaignImage"
	Public Const PolicyImage As String = "PolicyImage"
	Public Const ServiceLevel As String = "Service Level"
	Public Const CreditCard As String = "Credit Card"
	Public Const DebitCard As String = "Debit Card"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACTabTitle2 As Integer = 102
	Public Const ACTabTitle3 As Integer = 103
	Public Const ACTabTitle4 As Integer = 104
	Public Const ACTabTitle5 As Integer = 105
	
	' Tab 1
	Public Const ACGroupCode As Integer = 106
	Public Const ACIsAgent As Integer = 107
	'DC 20/06/00
	'New label
	Public Const ACIsProspect As Integer = 140
	Public Const ACGroupName As Integer = 108
	Public Const ACProspect As Integer = 109
	Public Const ACCharityDetails As Integer = 110
	Public Const ACCharity As Integer = 111
	Public Const ACNoOfMembers As Integer = 112
	Public Const ACGroupType As Integer = 113
	'DC 04/08/00
	Public Const ACMainContact As Integer = 142
	'MK 0916 1445 Public Const ACNoOfEmployees = 114
	'mk 990921 Public Const ACCompanyReg = 115
	Public Const ACCharityNumber As Integer = 116
	Public Const ACLeadAgent As Integer = 117
	Public Const ACConsultant As Integer = 136
	Public Const ACCode1 As Integer = 118
	Public Const ACCode2 As Integer = 119
	Public Const ACName1 As Integer = 120
	Public Const ACName2 As Integer = 121
	
	'RWH(24/07/2000)
	Public Const ACAddressListPostCode As Integer = 140
	Public Const ACAddressListUsage As Integer = 141
	Public Const ACAddressListLine1 As Integer = 142
	Public Const ACAddressListLine2 As Integer = 143
	Public Const ACAddressListLine3 As Integer = 144
	Public Const ACAddressListLine4 As Integer = 145
	
	'sj 18/06/2002 - start
	Public Const ACBranch As Integer = 143
	Public Const ACTradingName As Integer = 144
	Public Const ACSubBranch As Integer = 145
	Public Const ACAlternativeIdentifier As Integer = 146
	Public Const ACLoyaltyNumber As Integer = 147
	'sj 18/06/2002 - end
	
	' Tab 2, 3, 4
	Public Const ACAdd As Integer = 122
	Public Const ACDelete As Integer = 123
	Public Const ACEdit As Integer = 124
	
	'DC 28/06/00
	' Tab 3
	Public Const ACPreferredCorrespondence As Integer = 141
	
	' Tab 4
	Public Const ACCGC As Integer = 125
	
	' Tab 5
	Public Const ACCurrency As Integer = 126
	Public Const ACServiceLevel As Integer = 127
	Public Const ACPaymentMethod As Integer = 128
	Public Const ACCreditCardType As Integer = 129
	Public Const ACReminderType As Integer = 130
	Public Const ACTermsOfPayment As Integer = 131
	Public Const ACFinancialYear As Integer = 132
	Public Const ACAssociates As Integer = 133
	Public Const ACArea As Integer = 134
	Public Const ACFileCode As Integer = 135
	
	' CF070799
	Public Const ACWageRoll As Integer = 137
	Public Const ACTurnover As Integer = 138
	Public Const ACVatCode As Integer = 139
	
	
	' TF031298
	Public Const ACFinancial As Integer = 150
	Public Const ACNotes As Integer = 153
	Public Const ACLetter As Integer = 154
	
	Public Const ACLoyaltySchemes As Integer = 155 ' RAW 18/11/2002 : PS005 : Added
	Public Const ACTabTitle6 As Integer = 156 ' RAW 18/11/2002 : PS005 : Added
	Public Const ACConfirmDeleteTitle As Integer = 157 ' RAW 18/11/2002 : PS005 : Added
	Public Const ACConfirmDeleteDetails As Integer = 158 ' RAW 18/11/2002 : PS005 : Added
	Public Const ACTabTitle7 As Integer = 160 'ECK 2005 Roadmap
	'DC101204
	Public Const ACThirdParty As Integer = 159
	Public Const AC_CAPTION_FEECLIENT As Integer = 161
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	Public Const ACAddButton As Integer = 204
	Public Const ACEditButton As Integer = 205
	Public Const ACDeleteButton As Integer = 206
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACAgentMissing As Integer = 305
	Public Const ACRefExists As Integer = 306
	Public Const ACConsultantMissing As Integer = 307
	
	'DC101204
	Public Const ACThirdPartyMissing As Integer = 308
	
	' Menus
	
	' {* USER DEFINED CODE (End) *}
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
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
 Public g_iCurrencyID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iUserId As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	'DC 03/05/00
	'Cater for more than one Associate
	'defined in PMBGeneralFunc
	'Global reference to ListManager
	'Global g_oListManager As Object
	
	' Constants for the Policies array
	Public Const PMBPolicyPolicyID As Integer = 0
	Public Const PMBPolicyPolicyTypeID As Integer = 1
	Public Const PMBPolicyRenewalDate As Integer = 2
	Public Const PMBPolicyNoOfTimesQuoted As Integer = 3
	Public Const PMBPolicyTypeDescription As Integer = 4
	Public Const PMBPolicyTargetPremium As Integer = 5
	
	' Constants for the Campaigns array
	Public Const PMBCampaignRecordNo As Integer = 0
	Public Const PMBCampaignCampaignID As Integer = 1
	Public Const PMBCampaignCampaignDate As Integer = 2
	Public Const PMBCampaignDescription As Integer = 3
	
	' Constants for the LoyaltyScheme array
	' RAW 18/11/2002 : PS005 : Added - Begin
	Public Const PMBLoyaltyPartyLoyaltySchemeID As Integer = 0
	Public Const PMBLoyaltyPartyCnt As Integer = 1
	Public Const PMBLoyaltyLoyaltySchemeID As Integer = 2
	Public Const PMBLoyaltyLoyaltySchemeName As Integer = 3
	Public Const PMBLoyaltyMemberNumber As Integer = 4
	Public Const PMBLoyaltyOtherRef As Integer = 5
	Public Const PMBLoyaltyStartDate As Integer = 6
	Public Const PMBLoyaltyEndDate As Integer = 7
	Public Const PMBLoyaltyMainMemberNumber As Integer = 8
	Public Const PMBLoyaltyIsActive As Integer = 9
	' RAW 18/11/2002 : PS005 : End
	

	Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	
	Public Const ScreenHelpID As Integer = 6

    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oGIS As Object
	
	Public Const SYSOPTDuplicateClientIdentification As Integer = 600
	Public Const kSystemOptionClientBlacklistingInForce As Integer = 5011
	
	'MIPS Client Code Configuration
	Public Const kSystemOptionClientCodeConfiguration As Integer = 5031
	
	Public Const kAbandonNewRecordandUseSelectedClient As Integer = 0
	Public Const kCreateUniqueCode As Integer = 1
	
	'****************************************
	' Party Detail Array Position Constants
	Public Const kPartyDetailTaxNumber As Integer = 0
	Public Const kPartyDetailDomiciledForTax As Integer = 1
	Public Const kPartyDetailTaxExempt As Integer = 2
	Public Const kPartyDetailTaxPercentage As Integer = 3
	Public Const kPartyDetailBlackListReasonId As Integer = 4
	'****************************************
End Module