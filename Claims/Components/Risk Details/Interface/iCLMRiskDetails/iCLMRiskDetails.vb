Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: {TodaysDate}
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History: 18th Oct DG : Changes according to code review.
	' ***************************************************************** '
	
	
	' PW150402 - add missing constant
	Public Const ACDataTypeTabName As Integer = 7
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iCLMRiskDetails"
	'Start (Sriram P) Tech Spec - WR8 - Navigator DME Link.doc section(4.6.1)
	Public Const ACTransactionType As String = "C_CO"
	'End (Sriram P) Tech Spec - WR8 - Navigator DME Link.doc section(4.6.1)
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' Peril tab captions for labels & frame
	Public Const ACInterfaceTitle As Integer = 200
	Public Const ACRiskTabTitle1 As Integer = 201
	Public Const ACRiskTabTitle2 As Integer = 202
	Public Const ACGeneralDetails As Integer = 203
	Public Const ACComments As Integer = 205
	Public Const ACProgressStaus As Integer = 206
	Public Const ACStatus As Integer = 207
	Public Const ACClaimDescription As Integer = 208
	Public Const ACPrimaryCause As Integer = 209
	Public Const ACSecondaryCause As Integer = 210
	Public Const ACCoverPerils As Integer = 211
	
	'Column Headers
	Public Const ACClaimType As Integer = 212
	Public Const ACRiskDescription As Integer = 213
	Public Const ACPerilDescription As Integer = 214
	Public Const ACSumInsured As Integer = 215
	Public Const ACCurrentReserve As Integer = 216
	
	'Peril data manipulation buttons
	Public Const ACAdd As Integer = 217
	Public Const ACEdit As Integer = 218
	Public Const ACDelete As Integer = 219
	
	' Form buttons
	Public Const ACOK As Integer = 220
	Public Const ACCancel As Integer = 221
	Public Const ACHelp As Integer = 222
	
	'me 13-11-2002 - start claims - manage salvage availability functionality
	Public Const ACSalvage As Integer = 400
	'me 13-11-2002 - end claims - manage salvage availability functionality
	
	' Party headers
	Public Const ACDriverDetails As Integer = 223
	Public Const ACThirdPartyDetails As Integer = 224
	Public Const ACRepairerDetails As Integer = 225
	Public Const ACWitnessDetails As Integer = 226
	
	'Claim status Rosurce string
	Public Const ACCLMProvisionalOpenClaim As Integer = 227
	Public Const ACCLMLiveOpenClaim As Integer = 228
	Public Const ACCLMClosed As Integer = 229
	Public Const ACCLMReOpened As Integer = 230
	Public Const ACCLMReClosed As Integer = 231
	
	'PN 15887 JT 21-10-2004 -Changed the Const value since MKR added 325,326
	'PN13417 JT 21-09-2004
	'Const for Column Headers
	Public Const ACPolicyCurrency As Integer = 232
	Public Const AClossCurrency As Integer = 233
	
	'' Cancel Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	' Business failure messages
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACEnterParties As Integer = 304
	' Add Peril
	Public Const ACAddPerilTitle As Integer = 305
	Public Const ACAddPerilDetails As Integer = 306
	
	'Delete Peril
	Public Const ACDeletePerilTitle As Integer = 307
	Public Const ACDeletePerilDetails As Integer = 308
	
	' Toolbar's tooltip text
	Public Const ACToolTipFinancialdetailsButton As Integer = 309
	Public Const ACToolTipEventLogButton As Integer = 310
	Public Const ACToolTipRiskDetailsButton As Integer = 311
	Public Const ACToolTipPartySummaryButton As Integer = 322
	Public Const ACToolTipPolicySummaryButton As Integer = 323
	
	Public Const ACMandatoryDataForPeril As Integer = 312
	' Start: Bug ID - Code review, Date - 18th Oct Author- DG
	' The following lines are added for resource strings
	Public Const ACInvalidDate As Integer = 313
	Public Const ACPleaseAddPerils As Integer = 314
	Public Const ACEnterValues As Integer = 315
	' End: Bug ID - Code review, Date - 19th Oct Author- DG
	
	' Start: Internal Bug ID - 21, Date - 21th Oct Author- DG
	' The following lines are added for resource strings to incorporate
	' the functionality of closing a claim when the Current reserve is 0.00
	Public Const ACCloseClaimTitle As Integer = 316
	Public Const ACCloseClaimDetail As Integer = 317
	Public Const ACFailedToGetCurrentReserve As Integer = 318
	Public Const ACFailedToCloseClaim As Integer = 319
	Public Const ACInvalidAction As Integer = 320
	Public Const ACGenralDetailsFrame As Integer = 321
	
	' End: Internal Bug ID - 21, Date - 21th Oct Author- DG
	
	'DC020603 -ISS4430 -added resource data
	Public Const ACFailedToOpenClaim As Integer = 324
	'MKR 13/10/2004 PN 11201 Added entries in the resource file...
	Public Const ACOpenClaimTitle As Integer = 325
	Public Const ACOpenClaimDetail As Integer = 326
	'DC020603
	
	'me 13-11-2002 - start claims - manage salvage availability functionality
	Public Const ACSalvageOutstandingMsg As Integer = 500
	'me 13-11-2002 end'me 13-11-2002 - end claims - manage salvage availability functionality
	
	
	
	' Sequence of tool buttons
	Public Const ACFinancialdetailsButton As Integer = 1
	Public Const ACEventLogButton As Integer = 2
	Public Const ACPartySummaryButton As Integer = 3 ' RAM20021021 - NRMA Changes
	Public Const ACPolicySummaryButton As Integer = 4 ' RAM20021021 - NRMA Changes
	Public Const ACRiskDetailsButton As Integer = 5
	
	'Claim Status for a claim
	Public Const CLMProvisionalOpenClaim As Integer = 1
	Public Const CLMLiveOpenClaim As Integer = 2
	Public Const CLMClosed As Integer = 3
	Public Const CLMReOpened As Integer = 4
	Public Const CLMReClosed As Integer = 5
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' ACTopOfTabInform is for setting the top of the SSTab control on the form.
	Public Const ACTopOfTabInform As Integer = 120
	
	' ACLeftOfTabInform is for setting the left of the SSTab control on the form.
	Public Const ACLeftOfTabInform As Integer = 120
	
	' ACTopOfFirstFrameInTab is for setting the top of the first frame control on the form.
	Public Const ACTopOfFirstFrameInTab As Integer = 480
	
	' ACLeftOfFrameInTab is for setting the left of the frame control contained the SSTab.
	Public Const ACLeftOfFrameInTab As Integer = 240
	
	' ACLabelHeight is for setting the height of a label
	Public Const ACLabelHeight As Integer = 255
	Public Const ACLabelWidthMedium As Integer = 1575
	Public Const ACLabelWidthLong As Integer = 2055
	
	' Text box constants.
	Public Const ACTextBoxHeight As Integer = 285
	Public Const ACTopOfFirstTextBoxInTab As Integer = 540
	Public Const ACNormalGapBetweenTopsOfTwoTextBoxes As Integer = 360
	Public Const ACButtonHeight As Integer = 330
	Public Const ACButtonWidth As Integer = 1095
	
	Public Const ACLeftOFLabelInFirstColumn As Integer = 480
	Public Const ACMinimumFormHeight As Integer = 5265
	Public Const ACMinimumFormWidth As Integer = 10500
	
	Public Const ACTwice As Integer = 2
	Public Const ACFormBorder As Integer = 120
	Public Const ACThrice As Integer = 3
	Public Const ACDiffBetweenTopsOfLabelAndText As Integer = 45
	
	Public Const ACClient_name As Integer = 0
	Public Const ACClaim_Number As Integer = 1
	Public Const ACProgress_Status_Id As Integer = 2
	Public Const ACPrimary_Cause_Id As Integer = 3
	Public Const ACSecondary_Cause_Id As Integer = 4
	Public Const ACclaim_status_id As Integer = 5
	Public Const ACDescription As Integer = 6
	'RWH(13/11/2000) Claim numbering.
	Public Const ACPolicy_Number As Integer = 7
	Public Const ACClient_short_name As Integer = 8
	
	' Public constants for Tab's
	Public Const ACPerils As Integer = 0
	Public Const ACCommentsTab As Integer = 1
	Public Const ACDriver As Integer = 2
	Public Const ACThirdParty As Integer = 3
	Public Const ACRepairer As Integer = 4
	Public Const ACWitness As Integer = 5
	Public Const ACGeneral As Integer = 6
	
	'MSS260901 - Added for merge
	'AK 19042001
	Public Const ACGeminiIIMotor As String = "GEMINI IIM"
	Public Const ACGeminiIIHouseHold As String = "GEMINI IIH"
	Public Const ACCommercialVehicle As String = "CV"
	Public Const ACGIIMMandatory As Integer = 1
	Public Const ACGIIHMandatory As Integer = 2
	Public Const ACCVMandatory As Integer = 3
	
    'AK 190401
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sPolicyType As String = ""
	'MSS260901 - Merge end
	
	Public Const ACFormatforNumber As String = "0.00"
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vKeyArray As Object
	
	Sub Main_Renamed()
		
	End Sub
End Module