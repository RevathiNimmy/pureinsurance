Option Strict Off
Option Explicit On
Imports System
<System.Runtime.InteropServices.ProgId("GeneralConst_NET.GeneralConst")> _
 Public Module GeneralConst
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	'
	' Application general contants module. Contains all of the global
	' constants needed to be included into every application.
	'
	' ***************************************************************** '
	
	
	
	' ***************************************************************** '
	' Return Codes
	'
	' The return codes below are grouped as follows :
	'
	' General   - Commonly used return codes.
	' System    - Return Codes used by system functions.
	' Interface - Interface Layer return codes.
	' Business  - Business Layer return codes.
	' Database  - Database Layer return codes.
	'
	' Separate each group by a 200 offset.
	' ***************************************************************** '
	
	' General
	'********
	
	Public Const PMFalse As Integer = 0
	Public Const PMTrue As Integer = 1
	
	Public Const PMFail As Integer = 10
	Public Const PMError As Integer = 11
	
	Public Const PMOK As Integer = 20
	Public Const PMCancel As Integer = 21
	
	Public Const PMNavigate As Integer = 30
	
	' System
	'*******
	
	Public Const PMIncorrectUsername As Integer = 200
	Public Const PMIncorrectPassword As Integer = 201
	
	Public Const PMLogError1 As Integer = 210
	Public Const PMLogError2 As Integer = 211
	
	' Interface
	'**********
	
	' Wizard component values.
	Public Const PMMoveStatusBack As Integer = 400
	Public Const PMMoveStatusNext As Integer = 401
	Public Const PMMoveStatusCancel As Integer = 402
	Public Const PMMoveStatusFinish As Integer = 403
	
	
	' Business
	'*********
	
	Public Const PMLogonExceeded As Integer = 600
	Public Const PMLicenceExceeded As Integer = 601
	Public Const PMInvalidLicenceKey As Integer = 602
	
	Public Const PMDataChanged As Integer = 610
	Public Const PMMandatoryMissing As Integer = 611
	Public Const PMDataNotChanged As Integer = 612
	
	Public Const PMInvalidRequest As Integer = 620
	
	' Navigator Return Codes
	Public Const PMStartNewProcess As Integer = 700
	
	' Database
	'*********
	
	Public Const PMRecordChanged As Integer = 800
	Public Const PMRecordDeleted As Integer = 801
	
	Public Const PMRecordInUse As Integer = 810
	Public Const PMNotFound As Integer = 811
	
	Public Const PMBOF As Integer = 820
	Public Const PMEOF As Integer = 821
	
	' Broking Link error codes
	'*************************
	
	Public Const PMNoMainRegistry As Integer = 1001
	Public Const PMNoHostRegistry As Integer = 1002
	Public Const PMNoPortRegistry As Integer = 1003
	Public Const PMNoConnection As Integer = 1004
	Public Const PMNoPMLink As Integer = 1005
	Public Const PMNoCompanies As Integer = 1006
	
	' ***************************************************************** '
	' Constants
	'
	' The constants below are grouped as follows :
	'
	' General   - Used everywhere.
	' System    - Used by system functions.
	' Interface - Interface Layer.
	' Business  - Business Layer.
	' Database  - Database Layer.
	'
	' ***************************************************************** '
	
	' General
	'********
	
	' Constants for Message Logging
	
	' Log Levels
	Public Const PMLogFatal As Integer = 1
	Public Const PMLogError As Integer = 2
	Public Const PMLogWarning As Integer = 3
	Public Const PMLogInfo As Integer = 4
	Public Const PMLogOnError As Integer = 5
	Public Const PMLogDebug1 As Integer = 6
	Public Const PMLogDebug2 As Integer = 7
	Public Const PMLogDebug3 As Integer = 8
	Public Const PMLogDebug4 As Integer = 9
	
	
	' Log Level Descriptions
	Public Const PMFatalText As String = "Fatal"
	Public Const PMErrorText As String = "Error"
	Public Const PMWarningText As String = "Warning"
	Public Const PMInfoText As String = "Information"
	Public Const PMOnErrorText As String = "Recoverable Error"
	Public Const PMDebug1Text As String = "Debug 1"
	Public Const PMDebug2Text As String = "Debug 2"
	Public Const PMDebug3Text As String = "Debug 3"
	Public Const PMDebug4Text As String = "Debug 4"
	
	
	Public Const PMDefaultLogFile As String = "C:\Sirius\Sirius.Log"
	
	
	' System
	'*******
	
	' System/Product Details
	Public Const PMProduct As String = "SIRIUS"
	Public Const PMCustomer As String = "AIG"
	
	' PMLink/LicenceManager Timeout settings
	Public Const PMPollEverySeconds As Integer = 30
	Public Const PMTimeOutSeconds As Integer = 90
	
	' Constants for the logon attempts.
	Public Const PMLogonAttempts As Integer = 3
	
	' Resource file data types.
	Public Const PMResString As Integer = 0
	Public Const PMResBitmap As Integer = 1
	Public Const PMResIcon As Integer = 2
	Public Const PMResCursor As Integer = 3
	
	' Resource file language offset value.
	Public Const PMLangOffSetValue As Integer = 1000
	
	' Registry constants
	
	' Application
	Public Const PMRegAppName As String = "Sirius"
	
	' Sections
	Public Const PMRegSecSystem As String = "System"
	Public Const PMRegSecLicence As String = "LicenceManager"
	
	' Keys
	Public Const PMRegKeyPoolSize As String = "PoolSize"
	Public Const PMRegKeyLogFile As String = "LogFileName"
	Public Const PMRegKeyLogLevel As String = "UserLogLevel"
	
	' Navigator
	'**********
	
	' Caption Array Constants
	Public Const PMCaptionArrayMapID As Integer = 0
	Public Const PMCaptionArrayStepID As Integer = 1
	Public Const PMCaptionArraySubMapID As Integer = 2
	Public Const PMCaptionArrayCaption As Integer = 3
	Public Const PMCaptionArrayComponentType As Integer = 4
	Public Const PMCaptionArrayComponentID As Integer = 5
	
	' Key Let/Get Constants
	Public Const PMKeyName As Integer = 0
	Public Const PMKeyValue As Integer = 1
	
	' Constants used for the collection keys
	Public Const PMProcessKeyPrefix As String = "P"
	Public Const PMMapKeyPrefix As String = "M"
	Public Const PMStepKeyPrefix As String = "S"
	Public Const PMComponentKeyPrefix As String = "C"
	
	' Action Constants
	Public Const PMNavActionBackOne As String = "B1"
	Public Const PMNavActionBackX As String = "BX"
	Public Const PMNavActionExitMap As String = "EM"
	Public Const PMNavActionForwardOne As String = "F1"
	Public Const PMNavActionForwardX As String = "FX"
	Public Const PMNavActionRepeatMap As String = "RM"
	Public Const PMNavActionStartProcess As String = "SP"
	
	' Component Type Constants
	Public Const PMNavComponentDataForm As String = "DF"
	Public Const PMNavComponentBusinessObject As String = "BO"
	Public Const PMNavComponentFindForm As String = "FF"
	Public Const PMNavComponentDecisionForm As String = "QF"
	
	Public Const PMKeyNameNavigatorTitle1 As String = "navigator_title_1"
	Public Const PMKeyNamePartyCnt As String = "party_cnt"
	Public Const PMKeyNamePartyTypeID As String = "party_type_id"
	Public Const PMKeyNameSourceID As String = "source_id"
	Public Const PMKeyNameInsFileID As String = "insurance_file_id"
	Public Const PMKeyNameInsReference As String = "insurance_ref"
	Public Const PMKeyNameRiskID As String = "risk_id"
	Public Const PMKeyNameShortName As String = "short_name"
	Public Const PMKeyNameLongName As String = "long_name"
	Public Const PMKeyNameProductID As String = "product_id"
	
	Public Const PMKeyNameDecisionTitle As String = "decision_title"
	Public Const PMKeyNameDecisionText As String = "decision_text"
	
	' Interface
	'**********
	
	' Status type values.
	Public Const PMView As Integer = 0
	Public Const PMAdd As Integer = 1
	Public Const PMEdit As Integer = 2
	Public Const PMDelete As Integer = 3
	Public Const PMDummyDelete As Integer = 4
	Public Const PMAdded As Integer = 10
	
	' Process Mode values  (Navigator)
	'
	' Only use the following values for these constants :
	' 0,1,2,4,8,16,32,64,128,256,512,1024,2048,4096,8192,16384
	Public Const PMProcessModeGeneric As Integer = 0
	Public Const PMProcessModeEnquiry As Integer = 1
	Public Const PMProcessModeNBQuote As Integer = 2
	Public Const PMProcessModeNBLive As Integer = 4
	Public Const PMProcessModeRNQuote As Integer = 8
	Public Const PMProcessModeRNLive As Integer = 16
	Public Const PMProcessModeENNQuote As Integer = 32
	Public Const PMProcessModeENNLive As Integer = 64
	Public Const PMProcessModeENRQuote As Integer = 128
	Public Const PMProcessModeENRLive As Integer = 256
	
	' Type Of Business (Navigator)
	Public Const PMTypeOfBusinessGeneric As String = ""
	Public Const PMTypeOfBusinessNB As String = "NB"
	Public Const PMTypeOfBusinessRN As String = "RN"
	Public Const PMTypeOfBusinessENN As String = "ENN"
	Public Const PMTypeOfBusinessENR As String = "ENR"
	
	' Navigator button status.
	Public Const PMNavigateNotRequired As Integer = 0
	Public Const PMNavigateEnabled As Integer = 1
	Public Const PMNavigateDisabled As Integer = 2
	
	' Mouse pointer states.
	Public Const PMMouseBusy As Integer = 0
	Public Const PMMouseNormal As Integer = 1
	Public Const PMMouseReset As Integer = 2
	
	' Formatting values.
	Public Const PMFormatString As Integer = 0
	Public Const PMFormatStringCase As Integer = 1
	Public Const PMFormatDateShort As Integer = 2
	Public Const PMFormatDateMedium As Integer = 3
	Public Const PMFormatDateLong As Integer = 4
	Public Const PMFormatTimeShort As Integer = 5
	Public Const PMFormatTimeMedium As Integer = 6
	Public Const PMFormatTimeLong As Integer = 7
	Public Const PMFormatDateTimeShort As Integer = 8
	Public Const PMFormatDateTimeMedium As Integer = 9
	Public Const PMFormatDateTimeLong As Integer = 10
	Public Const PMFormatCurrency As Integer = 11
	Public Const PMFormatInteger As Integer = 12
	Public Const PMFormatBoolean As Integer = 13
	Public Const PMFormatStringUpper As Integer = 14
	Public Const PMFormatDateYearOnly As Integer = 15
	Public Const PMFormatPercent As Integer = 16
	Public Const PMFormatDouble As Integer = 17
	Public Const PMFormatLong As Integer = 18
	
	' Gemini Schemes Variant array
	Public Const PMInsurerNo As Integer = 0
	Public Const PMSchemeNo As Integer = 1
	Public Const PMSchemeVer As Integer = 2
	Public Const PMSchemeDesc As Integer = 3
	Public Const PMAppId As Integer = 4
	Public Const PMAppVer As Integer = 5
	
	
	' Gemini Quote Breakdown Variant array
	Public Const PMPremium As Integer = 3
	
	
	' Policy List Refresh
	Public Const PMListRefreshValue As Integer = 30
	
	
	' Business
	'*********
	
	' PMLookup constants
	
	' Column positions for input array.
	Public Const PMLookupTableName As Integer = 0
	Public Const PMLookupKey As Integer = 1
	Public Const PMLookupStartPos As Integer = 2
	Public Const PMLookupNumOfItems As Integer = 3
	
	' Column positions for output array.
	Public Const PMLookupID As Integer = 0
	Public Const PMLookupCaption As Integer = 1
	
	' Type of Lookup Required
	Public Const PMLookupAll As Integer = 0
	Public Const PMLookupSingle As Integer = 1
	Public Const PMLookupAllEffective As Integer = 2
	
	' Table names (Lookup Tables)
	Public Const PMLookupEventType As String = "event_type"
	Public Const PMLookupEventRepeatType As String = "repeat_type"
	Public Const PMLookupProduct As String = "product"
	Public Const PMLookupLanguage As String = "language"
	Public Const PMLookupCurrency As String = "currency"
	Public Const PMLookupCountry As String = "country"
	Public Const PMLookupCollectionFrom As String = "collection_from"
	Public Const PMLookupCollectionType As String = "collect_type"
	Public Const PMLookupRenewalFrequency As String = "renewal_frequency"
	Public Const PMLookupBranch As String = "branch"
	Public Const PMLookupBusinessType As String = "business_type"
	Public Const PMLookupSectionRateType As String = "rate_type"
	Public Const PMLookupPartyType As String = "party_type"
	Public Const PMLookupContactType As String = "contact_type"
	Public Const PMLookupTaxType As String = "tax_type"
	
	' Main Table Names
	Public Const PMTableParty As String = "party"
	Public Const PMTableRisk As String = "risk"
	Public Const PMTableEvent As String = "event"
	Public Const PMTableInsuranceFile As String = "insurance_file"
	
	' Field names
	Public Const PMFieldDescription As String = "description"
	Public Const PMFieldEffectiveDate As String = "effective_date"
	
	' Accumulation constants.
	
	Public Const PMAccumCodeDelimeter As String = "-"
	Public Const PMAccumID As Integer = 0
	Public Const PMAccumCode As Integer = 1
	Public Const PMAccumQuickCode As Integer = 2
	Public Const PMAccumCaption As Integer = 3
	Public Const PMAccumParentID As Integer = 4
	Public Const PMAccumTimeStamp As Integer = 5
	Public Const PMAccumStatus As Integer = 6
	
	' Commissions Constants
	
	' Commission Basis 1/2
	Public Const PMCommissionBasisOne As Integer = 1
	Public Const PMCommissionBasisTwo As Integer = 2
	
	' Commission Band Maximimum and Minimum
	Public Const PMCommissionBandMin As Integer = 0
	Public Const PMCommissionBandMax As Integer = 99
	
	' Column Positions for Commission Band Array
	Public Const PMCommissionBand As Integer = 0
	Public Const PMCommissionPremium As Integer = 1
	Public Const PMCommissionPercent As Integer = 2
	Public Const PMCommissionValue As Integer = 3
	Public Const PMCommissionBandID As Integer = 4
	
	' Tax Constants
	
	' Tax Basis 0/1/2/3
	Public Const PMUTaxBasisZero As Integer = 0
	Public Const PMUTaxBasisOne As Integer = 1
	Public Const PMUTaxBasisTwo As Integer = 2
	Public Const PMUTaxBasisThree As Integer = 3
	
	' Tax Band Maximimum and Minimum
	Public Const PMUTaxBandMin As Integer = 0
	Public Const PMUTaxBandMax As Integer = 9
	
	' Column Positions for Tax Band Array
	Public Const PMUTaxBand As Integer = 0
	Public Const PMUTaxPremium As Integer = 1
	Public Const PMUTaxPercent As Integer = 2
	Public Const PMUTaxValue As Integer = 3
	Public Const PMUTaxBandID As Integer = 4
	Public Const PMUTaxTypeID As Integer = 5
	Public Const PMUTaxTypeBandID As Integer = 6
	Public Const PMUTaxValueID As Integer = 7
	
	' Reinsurance Constants
	
	' Column Positions for RI Arrangement Lines Array
	Public Const PMURITreatyCode As Integer = 0
	Public Const PMURIDefaultSharePercent As Integer = 1
	Public Const PMURIThisSharePercent As Integer = 2
	Public Const PMURISumInsured As Integer = 3
	Public Const PMURIPremiumValue As Integer = 4
	Public Const PMURICommissionPercent As Integer = 5
	Public Const PMURICommissionValue As Integer = 6
	Public Const PMURIAgreementCode As Integer = 7
	Public Const PMURIMethod As Integer = 8
	Public Const PMURIArrangementLineID As Integer = 9
	Public Const PMURIFacArrangementSummaryID As Integer = 10
	
	' Column Positions for RI FAC Arrangement Array
	Public Const PMUFacPartyShortName As Integer = 0
	Public Const PMUFacBidSharePercent As Integer = 1
	Public Const PMUFacThisSharePercent As Integer = 2
	Public Const PMUFacSumInsured As Integer = 3
	Public Const PMUFacPremiumValue As Integer = 4
	Public Const PMUFacCommissionPercent As Integer = 5
	Public Const PMUFacCommissionValue As Integer = 6
	Public Const PMUFacAgreementCode As Integer = 7
	Public Const PMUFacPartyCnt As Integer = 8
	
	' Variable Data Constants
	'
	' Note: For the Full List of Variable Data Record/Fields name
	'       etc see VarDataConst.bas
	'
	Public Const PMVarRecordID As Integer = 0
	Public Const PMVarRecordName As Integer = 1
	Public Const PMVarFieldName As Integer = 2
	Public Const PMVarFieldType As Integer = 3
	Public Const PMVarDefaultFormat As Integer = 4
	Public Const PMVarLength As Integer = 5
	Public Const PMVarValidationID As Integer = 6
	Public Const PMVarLocatorType As Integer = 7
	Public Const PMVarCoreTable As Integer = 8
	Public Const PMVarCoreVariable As Integer = 9
	Public Const PMVarMandatoryLevel As Integer = 10
	Public Const PMVarValue As Integer = 11
	
	' Variable Data Valid Value Constants
	Public Const PMVarValValidationID As Integer = 0
	Public Const PMVarValValidValueID As Integer = 1
	Public Const PMVarValIsDefault As Integer = 2
	Public Const PMVarValCode As Integer = 3
	Public Const PMVarValNumericValue As Integer = 4
	Public Const PMVarValCaption As Integer = 5
	
	' Variable Data Core Variable Array Constants
	Public Const PMVarCoreVariableName As Integer = 0
	Public Const PMVarCoreVariableValue As Integer = 1
	
	' Variable Data Locator Array constants
	Public Const PMVarLocatorTypeID As Integer = 0
	Public Const PMVarLocatorTypeValue As Integer = 1
	
	' True/False in the Variable Data World
	Public Const PMVarFalse As Integer = 0
	Public Const PMVarTrue As Integer = 1
	
	' Database
	'*********
	
	' Lock Mode Constants
	Public Const PMNoLock As Integer = 0
	
	' Constants used by PMDAO (Data Access Object)
	
	' Database Name Constants
	' PM Data Source / DB Name
	Public Const PMSiriusDSN As String = "Sirius"
	Public Const PMSiriusDatabase As String = "Sirius"
	' Pinstripe Data Source /DB Name
	Public Const PMPinstripeDSN As String = "Shirley"
	Public Const PMPinstripeDatabase As String = "Shirley"
	
	' Constants required for string manipulation
	Public Const PMStartDelimiter As String = "{"
	Public Const PMEndDelimiter As String = "}"
	Public Const PMBinaryCompare As Integer = 0
	Public Const PMStringCompare As Integer = 1
	
	' Parameter Direction
	Public Const PMParamInput As Integer = 0
	Public Const PMParamInputOutput As Integer = 1
	Public Const PMParamOutput As Integer = 2
	Public Const PMParamReturnValue As Integer = 3
	Public Const PMParamDefault As Integer = 4
	
	' Parameter Data Types
	Public Const PMString As Integer = 0
	Public Const PMInteger As Integer = 1
	Public Const PMLong As Integer = 2
	Public Const PMDouble As Integer = 3
	Public Const PMDate As Integer = 4
	Public Const PMBoolean As Integer = 5
	Public Const PMCurrency As Integer = 6
	Public Const PMBinary As Integer = 7
	' These two are used by PMLookup
	Public Const PMTableName As Integer = 8
	Public Const PMFieldName As Integer = 9
	' These two are used by variable data
	Public Const PMUniqueKey As Integer = 10
	Public Const PMCode As Integer = 11
	
	' Database Parameter Prefix
	' Currently set to @ for SQLServer
	Public Const PMDBParamPrefix As String = "@"
	' Length of Prefix (In Characters)
	Public Const PMDBParamPrefixLen As Integer = 1
	
	' Database Hex Prefix
	' Currently Set to 0x for SQLServer
	Public Const PMDBHexPrefix As String = "0x"
	
	' Database Attribute Values,
	' These are constants for values used by Type fields etc
	' Grouped by Table
	
	' Party Table
	Public Const PMPartyTypeCompany As String = "C"
	Public Const PMPartyTypePerson As String = "P"
	
	' Insurance File Table
	Public Const PMInsFileTypePolicy As String = "P"
	Public Const PMInsFileTypeQuote As String = "Q"
	Public Const PMInsFileTypeRenewal As String = "R"
	
	' Telephone Table
	Public Const PMTelTypeLandLine As String = "L"
	Public Const PMTelTypeFax As String = "F"
    Public Const PMTelTypeMobile As String = "M"



    Public Const kSystemOptionDocumentArchive As Integer = 10
    Public Const kSystemOptionSharepointserverName As Integer = 5085
    Public Const kSystemOptionEnhancedResolvedName As Integer = 5148
	''Compiled rules system option numbers
	Public Const kSystemOptionCompiledRulePaymentGateway As Integer = 5150
	Public Const kSystemOptionCopyPolicyToQuoteEnabled = 5153
	Public Const kSystemOptionCompiledRuleChaseCycle As Integer = 5156
    Public Const kSystemOptionCompiledRuleCreditControl As Integer = 5155
    Public Const kSystemOptionMediaTypeIsCompliedRuleEnabled As Integer = 5149
    Public Const kSystemOptionCompiledRuleAddressLookup As Integer = 5157
    Public Const kSystemOptionCompiledRuleMediaType As Integer = 5158
    Public Const kSystemOptionRuleTypePaymentGateway As Integer = 5159
    Public Const kSystemOptionRuleTypeCreditControl As Integer = 5160
    Public Const kSystemOptionRuleTypeChaseCycle As Integer = 5161
    Public Const kSystemOptionRuleTypeAddressLookup As Integer = 5162

    ''CCM system option numbers
    Public Const kSystemOptionDocumentProductionSystem As Integer = 5163
    Public Const kSystemOptionCCMWebServiceURL As Integer = 5164
    Public Const kSystemOptionCCMPartner As Integer = 5165
    Public Const kSystemOptionCCMCustomer As Integer = 5166
    Public Const kSystemOptionCCMContractTypeName As Integer = 5167
    Public Const kSystemOptionCCMRepositoryProject As Integer = 5168
    Public Const kSystemOptionCCMContractTypeVersion As Integer = 5169
    Public Const kSystemOptionCCMSSL As Integer = 5170
    Public Const kSystemOptionCCMStatus As Integer = 5171
    Public Const kSystemOptionCCMClauseLocation As Integer = 5173
    Public Const kSystemOptionCCMEnableDatasetLogging As Integer = 5181

    Public Const kSystemOptionQASDatabaseID As Integer = 13
    'Sharepoint Online
    Public Const kSystemOptionSharePointURl As Integer = 5085
    Public Const kSystemOptionSharePointDocLib As Integer = 5086
    Public Const kSystemOptionIsSharePointOnline As Integer = 5177
    Public Const kSystemOptionSharePointUserName As Integer = 5178
    Public Const kSystemOptionSharePointPassword As Integer = 5179
    Public Const kSystemOptionEnablePaymentHub As Integer = 5200
    Public Const KSystemOptionKCMForSelectedTemplate As Integer = 5207
End Module