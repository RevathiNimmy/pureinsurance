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
	' RKS 21/01/2005 : Added constant for option 600
	' ***************************************************************** '
	
	
	'sj 23/07/2002 - start
	'Constants for Future Dated Addresses
	Public Const ACPartyVariantAddressCnt As Integer = 0
	Public Const ACPartyCnt As Integer = 1
	Public Const ACAddressCnt As Integer = 2
	Public Const ACOriginalAddressCnt As Integer = 3
	Public Const ACEffectiveDate As Integer = 4
	Public Const ACDateCreated As Integer = 5
	Public Const ACCommitInd As Integer = 6
	
	Public Const ACOptCurrentAddress As Integer = 0
	Public Const ACOptFutureAddress As Integer = 1
	'sj 23/07/2002 - end
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "uctPartyPCControl"
	Public Const ACStatusActWebLoading As String = "Loading..........."
	Public Const AssociatesImage As String = "AssociatesImage"
	Public Const AddressImage As String = "AddressImage"
	Public Const ContactImage As String = "ContactImage"
	Public Const LifestyleImage As String = "LifestyleImage"
	Public Const ConvictionImage As String = "ConvictionImage"
	Public Const CampaignImage As String = "CampaignImage"
	Public Const PolicyImage As String = "PolicyImage"
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
	'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.5.1.1)
	Public Const ACYearToDateTurnOver As Integer = 176
	Public Const ACLastYearTurnover As Integer = 177
	'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.5.1.1)
	Public Const ACReference As Integer = 106
	Public Const ACSurname As Integer = 107
	Public Const ACForename As Integer = 108
	Public Const ACTitle As Integer = 109
	Public Const ACInitials As Integer = 110
	Public Const ACIsAgent As Integer = 111
	Public Const ACAgent As Integer = 112
	Public Const ACName As Integer = 113
	Public Const ACAssociate As Integer = 114
	Public Const ACAssociateCode As Integer = 115
	Public Const ACArea As Integer = 116
	Public Const ACFileCode As Integer = 117
	Public Const ACConsultant As Integer = 118
	Public Const ACContacts As Integer = 119
	Public Const ACPaymentDetails As Integer = 120
	Public Const ACCurrency As Integer = 121
	Public Const ACPaymentMethod As Integer = 122
	Public Const ACReminderType As Integer = 123
	Public Const ACServiceLevel As Integer = 124
	Public Const ACCreditCard As Integer = 125
	Public Const ACTermsOfPayment As Integer = 126
	Public Const ACEmploymentDetails As Integer = 127
	Public Const ACOccupation As Integer = 128
	Public Const ACEmployer As Integer = 129
	Public Const ACStatus As Integer = 130
	Public Const ACCCJ As Integer = 131
	Public Const ACLifestyle As Integer = 132
	Public Const ACDOB As Integer = 133
	Public Const ACMaritalStatus As Integer = 134
	Public Const ACSeasonalGift As Integer = 135
	Public Const ACGender As Integer = 136
	Public Const ACNationality As Integer = 137
	Public Const ACOrigin As Integer = 138
	Public Const ACMailshot As Integer = 139
	Public Const ACPets As Integer = 140
	Public Const ACSmoker As Integer = 141
	Public Const ACAccommodation As Integer = 142
	Public Const ACDependants As Integer = 143
	Public Const ACProspecting As Integer = 144
	Public Const ACAgentReference As Integer = 145
	Public Const ACProspectStatus As Integer = 146
	Public Const ACCurrentAgent As Integer = 147
	Public Const ACTargetPremium As Integer = 148
	Public Const ACCampaigns As Integer = 149
	Public Const ACPolicies As Integer = 150
	'DC 20/06/00
	Public Const ACIsProspect As Integer = 151
	'DC 28/06/00
	Public Const ACPreferredCorrespondence As Integer = 152
	'RWH(24/07/2000)
	Public Const ACAddressListPostCode As Integer = 153
	Public Const ACAddressListUsage As Integer = 154
	Public Const ACAddressListLine1 As Integer = 155
	Public Const ACAddressListLine2 As Integer = 156
	Public Const ACAddressListLine3 As Integer = 157
	Public Const ACAddressListLine4 As Integer = 158
	'sj 18/06/2002 - start
	Public Const ACBranch As Integer = 159
	Public Const ACTradingName As Integer = 160
	Public Const ACSubBranch As Integer = 161
	Public Const ACAlternativeIdentifier As Integer = 162
	Public Const ACLoyaltyNumber As Integer = 163
	'sj 18/06/2002 - end
	'sj 02/07/2002 - start
	Public Const ACAffinity As Integer = 164
	Public Const ACTeam As Integer = 165
	Public Const ACMembershipId As Integer = 166
	'sj 02/07/2002 - end
	Public Const ACLoyaltySchemes As Integer = 167 ' RAW 18/11/2002 : PS005 : Added
	Public Const ACTabTitle6 As Integer = 168 ' RAW 18/11/2002 : PS005 : Added
	Public Const ACConfirmDeleteTitle As Integer = 169 ' RAW 18/11/2002 : PS005 : Added
	Public Const ACConfirmDeleteDetails As Integer = 170 ' RAW 18/11/2002 : PS005 : Added
	'DC101204
	Public Const ACThirdParty As Integer = 171
	Public Const ACCurrentThirdParty As Integer = 172
	Public Const ACThirdPartyRef As Integer = 173
	Public Const ACTabTitle7 As Integer = 174 'ECK 2005 Roadmap
	Public Const AC_CAPTION_ISFEECLIENT As Integer = 175
	
	' TF031298
	'Public Const ACFinancial = 150
	'Public Const ACPolicy = 151
	'Public Const ACClaim = 152
	'Public Const ACNotes = 153
	'Public Const ACLetter = 154
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	Public Const ACAddButton As Integer = 204
	Public Const ACEditButton As Integer = 205
	Public Const ACDeleteButton As Integer = 206
	Public Const ACLookupButton As Integer = 207
	Public Const ACProspectButton As Integer = 208
	Public Const ACMaintainButton As Integer = 209
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACEmployerMissing As Integer = 304
	Public Const ACAgentMissing As Integer = 305
	Public Const ACRefExists As Integer = 306
	Public Const ACConsultantMissing As Integer = 307
	Public Const ACAssociateMissing As Integer = 308
	' Menus
	
	'DC101204
	Public Const ACThirdPartyMissing As Integer = 309
	
	
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
 Public g_iCurrencyId As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iUserId As Integer
    'developer guide no. 107
    '' Public instance of the object manager.
    'Public g_oObjectManager As bObjectManager.ObjectManager
	'DC 03/05/00
	'Cater for more than one Associate
	'defined in PMBGeneralFunc
	'Global reference to ListManager
	'Public g_oListManager As Object
	
    Public Const ScreenHelpID As Integer = 3
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
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
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oGIS As Object
	
	Public Const kSystemOptionDuplicateClientIdentification As Integer = 600
	Public Const kSystemOptionClientBlacklistingInForce As Integer = 5011
	'Start (Girija chokkalingam) - (Tech Spec - WR38 - Personal Client Resolved Name.doc) - (5.1.1)
	Public Const kUSLanguage As Integer = 2
	Public Const kUKLanguage As Integer = 1

    Public Const kSystemOptionUpdateExistingClients As Integer = 5064

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