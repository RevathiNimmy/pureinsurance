Option Strict Off
Option Explicit On
Imports System

Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 22 Aug 2000
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '

    <ThreadStatic()> _
    Public objfrmInterface As frmInterface
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iCLMPeril"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	
	' Buttons
	Public Const ACOKButton As Integer = 202
	Public Const ACCancelButton As Integer = 203
	Public Const ACHelpButton As Integer = 204
	Public Const ACEditButton As Integer = 205
	
	' Tabs
	Public Const ACGeneralTab As Integer = 101
	Public Const ACDriverTab As Integer = 102
	Public Const ACThirdPartyTab As Integer = 103
	Public Const ACRepairerTab As Integer = 104
	Public Const ACWitnessTab As Integer = 105
	Public Const ACReserveTab As Integer = 106
	Public Const ACPaymentTab As Integer = 107
	Public Const ACCommentsTab As Integer = 108
	Public Const ACOtherPartiesTab As Integer = 109
	
	' Frames
	Public Const ACGeneralFrame As Integer = 120
	Public Const ACDriverFrame As Integer = 121
	Public Const ACThirdPartyFrame As Integer = 122
	Public Const ACRepairerFrame As Integer = 123
	Public Const ACWitnessFrame As Integer = 124
	Public Const ACReserveFrame As Integer = 125
	Public Const ACPaymentFrame As Integer = 126
	Public Const ACCommentsFrame As Integer = 127
	
	' labels for frmdetails
	Public Const ACRiskType As Integer = 128
	Public Const ACInitialReserve As Integer = 129
	Public Const ACRevisedReserve As Integer = 130
	Public Const ACThispayment As Integer = 131
	Public Const ACOtherParties As Integer = 132
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	' Invalid Data
	Public Const ACInvalidDataTitle As Integer = 310
	Public Const ACInvalidIntegerData As Integer = 311
	Public Const ACInvalidDateData As Integer = 314
	Public Const ACInvalidReserveDataTitle As Integer = 315
	Public Const ACInvalidReserveData As Integer = 316
	'DC030402 new title for payment errors
	Public Const ACInvalidPaymentDataTitle As Integer = 317
	Public Const ACInvalidPaymentData As Integer = 318
	
	' Mandatory Values
	Public Const ACMandatoryTitle As Integer = 312
	Public Const ACMandatory As Integer = 313
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	Public Enum UWDetailScreenMode
		UWPaymentDetails = 0
		UWReserveDetails = 1
	End Enum
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
    ' Declare an instance of the FormControl object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_oFormFields As iPMFormControl.FormFields
	
	' Public instance of the Business Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As bCLMPeril.Business
	
	'TN20010406 Start
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oClaimTrans As bControlTransClaims.Automated
	'TN20010406 End
	
	'UNCOMMENT FOR INTEGRATION*******************************************
    'Public variables
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_iTask As gPMConstants.PMEComponentAction
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_lPerilID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_lClaimID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_lPerilTypeID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_sRisktype As String = ""
    'Public Const m_lpartycnt As Long = 1 ' Comment for Integration with Payment Screen
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_lPartycnt As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_sPartyShortName As String = "" ' RAM20021021 : Added this variable
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_lInsurance_file_cnt As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_lRiskID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_lRiskTypeID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_lSequenceNo As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_sClaimRef As String = ""
	'UNCOMMENT FOR INTEGRATION*******************************************
	
    ' RDC 03062004
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_lCurrencyID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_sCurrencyDesc As String = ""
	
	' Public constants for the fields in the Control Array
	Public Const g_ciCAuserdefperildataid As Integer = 0
	Public Const g_ciCAcaption As Integer = 1
	Public Const g_cICAtype As Integer = 2
	Public Const g_cICAdisplayorder As Integer = 3
	Public Const g_cICAmandatory As Integer = 4
	Public Const g_cICAreadonly As Integer = 5
	Public Const g_cICAclaimpartytypeid As Integer = 6
	Public Const g_cICAclaimlookupid As Integer = 7
	Public Const g_cICAvalue As Integer = 8
	Public Const g_cICTabID As Integer = 11
	Public Const g_cICTabCaption As Integer = 12
	Public Const g_cICTabCount As Integer = 13
	
	
	'DC140302
	Public Const g_cICDescription As Integer = 10
	
	' Public constants declared for the fields in the ReserveType Array
	Public Const g_cIRTADescription As Integer = 0
	
	' Public constants declared for the Reserve Details Array
	Public Const g_cIRDAreserveid As Integer = 0
	'Public Const g_cIRDAreservetypeid As Integer = 1
	Public Const g_cIRDAinitialreserve As Integer = 1
	Public Const g_cIRDApaidtodate As Integer = 2
	Public Const g_cIRDArevisedreserve As Integer = 3
	Public Const g_cIRDAsuminsured As Integer = 4
	Public Const g_cIRDAaverage As Integer = 5
	Public Const g_cIRDArevisioncount As Integer = 6
	'DC030402
	Public Const g_cIRDAreservetype As Integer = 7
	Public Const g_cIRDArevisedentered As Integer = 8
	
	' Public constants declared for the Payment Details Array
	Public Const g_cIPDApaymentid As Integer = 0
	Public Const g_cIPDAamount As Integer = 1
	Public Const g_cIPDATaxAmount As Integer = 4
	Public Const g_cIPDATaxTypeCode As Integer = 5
	
	' Public constants declated for the Party Details Array
	Public Const g_cIPartyDApartyclaimid As Integer = 0
	Public Const g_cIPartyDAType As Integer = 1
	Public Const g_cIPartyDAname As Integer = 2
	Public Const g_cIPartyDAaddress As Integer = 3
	Public Const g_cIPartyDAlicencetype As Integer = 4
	Public Const g_cIPartyDAltdescription As Integer = 5
	Public Const g_cIPartyDAlicencenumber As Integer = 6
	Public Const g_cIPartyDADOB As Integer = 7
	Public Const g_cIPartyDAsex As Integer = 8
	Public Const g_cIPartyDApartystatus As Integer = 9
	Public Const g_cIPartyDApsdescription As Integer = 10
	Public Const g_cIPartyDAphonenumber As Integer = 11
	Public Const g_cIPartyDAfaxnumber As Integer = 12
	Public Const g_cIPartyDAreferencenumber As Integer = 13
	Public Const g_cIPartyDAregnumber As Integer = 14
	
	' Public constants declared for claim_lookup
	Public Const g_cICLAlookupid As Integer = 0
	Public Const g_cICLAdescription As Integer = 1
	
	' Public constants for Tab's
	Public Const ACGeneral As Integer = 0
	Public Const ACDriver As Integer = 5
	Public Const ACThirdParty As Integer = 6
	Public Const ACRepairer As Integer = 7
	Public Const ACWitness As Integer = 8
	Public Const ACReserve As Integer = 9
	Public Const ACPayment As Integer = 10
	Public Const ACComments As Integer = 11
	
	' Public constants declared for the Recovery Details
	Public Const g_cIRecoveryDAinitialreserve As Integer = 0
	Public Const g_cIRecoveryDArevisedreserve As Integer = 1
	Public Const g_cIRecoveryDApaidtodate As Integer = 2
	
	' public constants for the Payment details
	Public Const g_ciReceiptDAamount As Integer = 0
	
	' public contants for the Edit Button in reseve and Payment Tabs
	Public Const ACReserveEdit As Integer = 0
	Public Const ACPaymentEdit As Integer = 1
	
	' payment method screen
	'RWH(25/07/01) Option number for Payment method should be 2002 NOT 2022.
	Public Const ACOptionNumber As Integer = 2002
	'RWH(25/07/01) Suspense is 0.
	Public Const ACOptionValueSuspense As Integer = 0
	
	'Public Constants for the GetKeys
	Public Const PMKeyNameSumInsured As String = "sum_insured"
	Public Const PMKeyNameCurrentReserve As String = "current_reserve"
	
	'Public constants for the SetKeys declaration
	Public Const PMKeyPerilID As String = "claim_peril_id"
	Public Const PMKeyClaimID As String = "claim_id"
	Public Const PMKeyPerilTypeID As String = "peril_type_id"
	Public Const PMKeyRiskType As String = "risk_type"
	'Public Const PMKeyPartycnt As String = "party_cnt"
	Public Const PMKeyInsuranceFilecnt As String = "insurance_file_cnt"
	Public Const PMKeyRiskID As String = "risk_id"
	
    ' Public Array Declaration for the datatypes
    'developer guide no. 107
    <ThreadStatic()> _
 Public vDataTypeArray As Object
	
	' Public constants for Date Display
	Public Const ACDateDisplay As String = "long date"
	Public Const ACShortDate As String = "short date"
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_cTotalCurrentReserve As Decimal
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_cTotalPayment As Decimal
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_iCurrencyID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_cTotalSumInsured As Decimal
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_sComments As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_bIsPostTaxes As Boolean
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lPayeeMediaType As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sPayeeName As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sPayeeBankName As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sPayeeSortCode As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sPayeeAccountNo As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lPayeeCountry As Integer
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_sPayeeComments As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lPaymentCurrencyID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_dPaymentLossRate As Double
	
    ' Public array to keep track of the reserve types that are to be included in the total.
    'developer guide no. 107
    <ThreadStatic()> _
 Public v_vReserveTotalArray As Object
	
    'TN20010406 Start
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_vReserveDetails As Object 'to keep track of original values
	'TN20010406 End
    'developer guide no. 107
    <ThreadStatic()> _
 Public intControlTypes() As Integer

    
  
	Public Const ACOptionNumberAuthoriseClaimPayment As Integer = 2020
End Module