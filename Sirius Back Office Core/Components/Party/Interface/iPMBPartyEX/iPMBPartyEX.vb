Option Strict Off
Option Explicit On
Imports System
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
 Public Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 23/06/1998
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	'               MK 990929 Changing completely
	' ***************************************************************** '
	
	' Main public constant for all functions to identify which application this is.
	Public Const ACApp As String = "iPMBPartyEX"
	
	'EK 16/10/99
	Public Const PMKeyNameSpecialParty As String = "special_party"
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACTabTitle2 As Integer = 102
	Public Const ACTabTitle3 As Integer = 103
	Public Const ACReference As Integer = 104
	Public Const ACPostcode As Integer = 105
	Public Const ACName As Integer = 106
	Public Const ACIsBranch As Integer = 107
	Public Const ACAgencyAgreement As Integer = 110
	Public Const ACAgencyNextReview As Integer = 111
	Public Const ACSource As Integer = 112
	Public Const ACPaymentMethod As Integer = 113
	Public Const ACfraAppointment As Integer = 114
	Public Const ACHeadOffice As Integer = 115
	Public Const ACFSAProduct As Integer = 116
	Public Const ACRiskTransferAgreement As Integer = 117
	Public Const ACDelegatedAuthority As Integer = 118
	
	' TF031298
	Public Const ACFinancial As Integer = 150
	Public Const ACNotes As Integer = 153
	Public Const ACLetter As Integer = 154
	
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
	
	Public Const ACHeadOfficeMissing As Integer = 305
	Public Const ACRefExists As Integer = 306
	' Menus
	'Images
	Public Const ksAddressImage As String = "AddressImage"
	Public Const ksContactImage As String = "ContactImage"
	Public Const ksPolicyImage As String = "PolicyImage"
	
	' Public contants used for the start and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	

	Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	
	Public Const ScreenHelpID As Integer = 40000
	
	Public Const ACIADDRESS As String = "ADDRESS"
	
	Public Enum eFeesColumns
		Col_RiskGroup = 0
		Col_Scheme = 1
		Col_Currency = 2
		Col_Perc = 3
		Col_Amount = 4
		Col_CommPerc = 5
		Col_CommAmount = 6
	End Enum
	
	'Public Enum eUFeesColumns
	'    Col_Product = 0
	'    Col_Transaction = 1
	'    Col_UPerc = 2
	'    Col_UAmount = 3
	'    Col_EffecDate = 4
	'    Col_Currency = 5
	'End Enum
	
	' fee list view columns
	Public Const kUWFeeColHeaderAppliesTo As Integer = 1
	Public Const kUWFeeColHeaderEffectiveOn As Integer = 2
	Public Const kUWFeeColHeaderAppliesToType As Integer = 3
	Public Const kUWFeeColHeaderRate As Integer = 4
	Public Const kUWFeeColHeaderEffectiveDate As Integer = 5
	Public Const kUWFeeColHeaderTaxed As Integer = 6
	Public Const kUWFeeColHeaderTaxGroup As Integer = 7
	Public Const kUWFeeColHeaderIns As Integer = 8
	Public Const kUWFeeColHeaderSpread As Integer = 9
	' Adding constant for BPIS
	
	
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
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager

	Public Const kFeeTypeProduct As Integer = 1
	Public Const kFeeTypeRiskTypeGroup As Integer = 2
	Public Const kFeeTypePerilGroup As Integer = 3
	
	Public Const UWItemsFeeAmountId As Integer = 0
	Public Const UWItemsProductId As Integer = 1
	Public Const UWItemsRiskTypeGroupId As Integer = 2
	Public Const UWItemsPerilGroupId As Integer = 3
	Public Const UWItemsProductDescription As Integer = 4
	Public Const UWItemsPerilGroupDescription As Integer = 5
	Public Const UWItemsRiskTypeGroupDescription As Integer = 6
	Public Const UWItemsTransactionSubType As Integer = 7
	Public Const UWItemsFeePercentage As Integer = 8
	Public Const UWItemsFeeAmount As Integer = 9
	Public Const UWItemsEffectiveDate As Integer = 10
	Public Const UWItemsTaxGroup As Integer = 11
	Public Const UWItemsCurrencyFormatString As Integer = 12
	Public Const UWItemsIncludeIns As Integer = 13
	Public Const UWItemsSpread As Integer = 14
	
	
	'****************************************
	' Party Detail Array Position Constants
	Public Const kPartyDetailTaxNumber As Integer = 0
	Public Const kPartyDetailDomiciledForTax As Integer = 1
	Public Const kPartyDetailTaxExempt As Integer = 2
	Public Const kPartyDetailTaxPercentage As Integer = 3
	'****************************************
	
	'0 ' fa.fee_amount_id,
	'1 ' fa.product_id,
	'2 ' fa.risk_group_id,
	'3 ' fa.peril_group_id,
	'4 ' p.description AS ProductDescription,
	'5 ' pg.description as PerilGroupDescription,
	'6 ' rg.description as RiskGroupDescription
	'7 ' fa.transaction_sub_type,
	'8 ' fa.fee_percentage,
	'9 ' fa.fee_amount,
	'10 ' fa.effective_date,
	'11 ' fa.tax_group_id
	
	
	
	
	Sub Main_Renamed()
		
		
    End Sub
End Module