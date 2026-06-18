Option Strict Off
Option Explicit On
Public Module PMConst
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
    ' Edit History :
    '
    '   implemented 30/9/97 in accordance with PM agreed standards
    '
    ' TF300997 - Constants added for Event Primer module
    ' TF031097 - Constants added for Navigator Process Codes
    ' TF091097 - PMSucceed Constant added for use as function parameter
    ' BB201097 - Added constants for Voyager DB
    ' TF041197 - Constants added for Product Risk Selection
    ' TF191197 - Reference field constants added for JW AutoNumber functions
    ' TF031297 - PMMsgBox Caption & Message constants added
    ' BB121297 - Added constants for Voyager Form Tools
    ' BB030298 - Added DisplayMode constant for Navigator
    ' BB040298 - Added Control Type constants for FormControl
    '****************************************************************** '



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
    ' Seporate each group by a 200 offset.
    ' ***************************************************************** '

    ' General
    '********

    Public Const PMFalse As Integer = 0
    Public Const PMTrue As Integer = 1

    ' TF091097 - PMSucceed Constant added for use as function parameter
    Public Const PMFail As Integer = 10
    Public Const PMError As Integer = 11
    Public Const PMSucceed As Integer = 12

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
    '***********************

    ' Return Values from the NextStep method
    Public Const PMNavStartNewProcess As Integer = 700
    Public Const PMNavCallComponent As Integer = 701
    Public Const PMNavBuildMap As Integer = 702
    Public Const PMNavRepeatMap As Integer = 703
    Public Const PMNavEndMap As Integer = 704
    Public Const PMNavNavigate As Integer = 705
    Public Const PMNavEndProcess As Integer = 706


    ' Database
    '*********

    Public Const PMRecordChanged As Integer = 800
    Public Const PMRecordDeleted As Integer = 801

    Public Const PMRecordInUse As Integer = 810
    Public Const PMNotFound As Integer = 811

    Public Const PMBOF As Integer = 820
    Public Const PMEOF As Integer = 821


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

    ' ClientManager/LicenceManager Timeout settings
    Public Const PMPollEverySeconds As Integer = 30
    Public Const PMTimeOutSeconds As Integer = 90

    ' Constants for the logon attempts.
    Public Const PMLogonAttempts As Integer = 3

    ' Column positions for PMLock table
    Public Const PMLockFormAllName As Integer = 0
    Public Const PMLockFormAllValue As Integer = 1
    Public Const PMLockFormAllUser As Integer = 2
    Public Const PMLockFormAllTime As Integer = 3
    Public Const PMLockFormAllUserID As Integer = 4

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

    ' Period constants.
    Public Const PMDay As Integer = 0
    Public Const PMWeek As Integer = 1
    Public Const PMMonth As Integer = 2
    Public Const PMYear As Integer = 3

    ' TypeOfNumber TF191197
    ' ************

    Public Const PMAutoNumInsFile As Integer = 1
    Public Const PMAutoNumInsFolder As Integer = 2
    Public Const PMAutoNumRiskFolder As Integer = 3
    Public Const PMAutoNumParty As Integer = 4
    Public Const PMAutoNumContact As Integer = 5
    Public Const PMAutoNumAddress As Integer = 6

    ' Reference Fields TF191197
    ' ****************

    Public Const PMRefFieldCoverStartDate As String = "CSD"
    Public Const PMRefFieldCoverExpiryDate As String = "CED"
    Public Const PMRefFieldBranchCode As String = "BC"
    Public Const PMRefFieldProductCode As String = "PC"
    Public Const PMRefFieldProductAnalysisCode As String = "PAC"
    Public Const PMRefFieldTransactionTypeCode As String = "TTC"
    Public Const PMRefFieldTransactionBasis As String = "TB"
    Public Const PMRefFieldSourceCode As String = "SC"


    ' Navigator
    '**********

    ' TF031097 - Constants added for Navigator Process Codes
    Public Const PMNavProcessRenewalPreProcess As String = "RENPREPROC"
    Public Const PMNavProcessRenewalMakeUp As String = "RENMAKEUP"
    Public Const PMNavProcessRenewalNotice As String = "RENNOTICE"
    Public Const PMNavProcessRenewalUpdate As String = "RENUPDATE"

    ' Caption Array Constants
    Public Const PMNavCaptionStepKey As Integer = 0
    Public Const PMNavCaptionCaption As Integer = 1
    Public Const PMNavCaptionIsSubMap As Integer = 2
    Public Const PMNavCaptionComponentType As Integer = 3

    ' Key Let/Get Constants
    Public Const PMKeyName As Integer = 0
    Public Const PMKeyValue As Integer = 1

    ' Constants used for the collection keys
    Public Const PMProcessKeyPrefix As String = "P"
    Public Const PMMapKeyPrefix As String = "M"
    Public Const PMStepKeyPrefix As String = "S"
    Public Const PMComponentKeyPrefix As String = "C"
    Public Const PMProcInstanceKeyPrefix As String = "PI"
    Public Const PMMapInstanceKeyPrefix As String = "MI"
    Public Const PMStepInstanceKeyPrefix As String = "SI"

    ' Status settings for Process, Map and Step.
    Public Const PMNavStatusUnknown As String = ""
    Public Const PMNavStatusNotActive As String = "NA"
    Public Const PMNavStatusComplete As String = "CP"
    Public Const PMNavStatusIncomplete As String = "IP"

    ' Incomplete Effects
    Public Const PMNavIncompleteNone As String = "NA"
    Public Const PMNavIncompleteCurrentProcess As String = "CP"
    Public Const PMNavIncompleteCurrentMap As String = "CM"

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

    ' Summary Detail Type Constants
    Public Const PMNavSummProcessSummary As Integer = 0
    Public Const PMNavSummMapInstances As Integer = 1
    Public Const PMNavSummMapSummary As Integer = 2

    ' Summary Detail Array Constants
    Public Const PMNavSummLevel As Integer = 0
    Public Const PMNavSummHeading As Integer = 1
    Public Const PMNavSummValue As Integer = 2

    Public Const PMKeyNameNavigatorTitle1 As String = "navigator_title_1"
    Public Const PMKeyNamePartyCnt As String = "party_cnt"
    Public Const PMKeyNamePartyTypeID As String = "party_type_id"
    Public Const PMKeyNameSourceID As String = "source_id"
    Public Const PMKeyNameInsFileID As String = "insurance_file_id"
    Public Const PMKeyNameInsFileCnt As String = "insurance_file_cnt"
    Public Const PMKeyNameInsReference As String = "insurance_ref"
    Public Const PMKeyNameRiskID As String = "risk_id"
    Public Const PMKeyNameShortName As String = "short_name"
    Public Const PMKeyNameLongName As String = "long_name"
    Public Const PMKeyNameProductID As String = "product_id"
    Public Const PMKeyNameRenProcessCode As String = "renewal_process_code"
    Public Const PMKeyNameExtraTypeCode As String = "code"
    Public Const PMKeyNameRenewToDate As String = "renew_to_date"
    Public Const PMKeyNameInsFolderCnt As String = "insurance_folder_cnt"
    Public Const PMKeyNameBatchSetID As String = "batch_set_id"
    Public Const PMKeyNameNavProcessCode As String = "nav_process_code"

    Public Const PMKeyNameDecisionTitle As String = "decision_title"
    Public Const PMKeyNameDecisionText As String = "decision_text"

    'TF031297 - PMMsgBox Caption & Message constants added
    Public Const PMKeyNameMsgBoxCaption As String = "message_box_caption"
    Public Const PMKeyNameMsgBoxText As String = "message_box_text"


    Public Const PMKeyNameInitialNoticeFromDate As String = "initial_fromdate"
    Public Const PMKeyNameInitialNoticeToDate As String = "initial_todate"

    ' TF300997 - Constants added for Event Primer module
    Public Const PMKeyNameMainEventTypeCode As String = "main_event_type_code"
    Public Const PMKeyNameSubEventTypeCode As String = "sub_event_type_code"
    Public Const PMKeyNameMainEventID As String = "main_event_id"
    Public Const PMKeyNameSubEventID As String = "sub_event_id"

    ' TF041197 - Constants added for Product Risk Selection
    Public Const PMKeyNameProductCode As String = "product_code"
    Public Const PMKeyNameRiskTypeCode As String = "risk_type_code"
    Public Const PMKeyNameRiskTypeID As String = "risk_type_id"


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
    ' BB151297 - Added constants for Voyager Form Tools
    ' This format uses local currency symbol and drops decimal amounts
    Public Const PMFormatWholeMoney As Integer = 19
    ' This format uses local currency symbol
    Public Const PMFormatMoney As Integer = 20

    Public Const PMFormatDecimal As Integer = 21 'JY120298

    ' Lookup list refresh value.
    Public Const PMListRefreshValue As Integer = 30

    ' BB030298
    ' Display Mode Setting used in Navigator
    Public Const PMDisplayStandard As Integer = 0
    Public Const PMDisplaySimple As Integer = 1

    ' BB040298
    ' Added Control Type constants for FormControl
    Public Const PMUnknownCtlType As Integer = 0
    Public Const PMTextBox As Integer = 1
    Public Const PMCombo As Integer = 2
    Public Const PMCheckBox As Integer = 3
    Public Const PMListBox As Integer = 4
    Public Const PMGrid As Integer = 5
    Public Const PMSpread As Integer = 6
    Public Const PMOptionButton As Integer = 7

    ' Business
    '*********

    ' Business Object Action Values.
    Public Const PMActionView As Integer = 0
    Public Const PMActionEditAdd As Integer = 1
    Public Const PMActionEditUpdate As Integer = 2
    Public Const PMActionEditDelete As Integer = 3
    Public Const PMActionGetDefault As Integer = 4
    Public Const PMActionGetMandatory As Integer = 5

    ' Business Object Mandatory constants
    Public Const PMNonMandatory As Integer = 0
    Public Const PMMandatory As Integer = 1
    Public Const PMNonVisible As Integer = 2

    ' PMLookup constants

    ' Column positions for input array.
    Public Const PMLookupTableName As Integer = 0
    Public Const PMLookupKey As Integer = 1
    Public Const PMLookupStartPos As Integer = 2
    Public Const PMLookupNumOfItems As Integer = 3

    ' Column positions for output array.
    Public Const PMLookupID As Integer = 0
    Public Const PMLookupCaption As Integer = 1
    Public Const PMLookupCode As Integer = 2

    ' Type of Lookup Required
    Public Const PMLookupAll As Integer = 0
    Public Const PMLookupSingle As Integer = 1
    Public Const PMLookupAllEffective As Integer = 2

    ' Transaction_Type_Basis constants
    Public Const PMTransTypeBasisAdditional As String = "A"
    Public Const PMTransTypeBasisRefund As String = "F"
    Public Const PMTransTypeBasisPrimary As String = "P"
    Public Const PMTransTypeBasisReversePrimary As String = "R"

    ' Table names (PM Wide Lookup Tables)
    Public Const PMLookupLanguage As String = "language"
    Public Const PMLookupCurrency As String = "currency"
    Public Const PMLookupCountry As String = "country"

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
    ' Orion Data Source /DB Name
    Public Const PMOrionDSN As String = "Orion"
    Public Const PMOrionDatabase As String = "Orion"
    ' Gemini Data Source /DB Name
    Public Const PMGeminiDSN As String = "Gemini"
    Public Const PMGeminiDatabase As String = "Gemini"
    ' BB201097 - Constants for Voyager DB
    Public Const PMVoyagerDSN As String = "Voyager"
    Public Const PMVoyagerDatabase As String = "Voyager"

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
    ' RFC 18/09/1997 Added for VB Decimal Support
    Public Const PMDecimal As Integer = 12
    ' BB 06/10/1997 Added for PMFormControl as a Field Type
    Public Const PMLookup As Integer = 13

    ' Database Parameter Prefix
    ' Currently set to @ for SQLServer
    Public Const PMDBParamPrefix As String = "@"
    ' Length of Prefix (In Characters)
    Public Const PMDBParamPrefixLen As Integer = 1

    ' Database Hex Prefix
    ' Currently Set to 0x for SQLServer
    Public Const PMDBHexPrefix As String = "0x"

    ' SP 23/09/1997 - Tell PMDAO not to restrict number of records returned in SQLSelect
    Public Const PMAllRecords As Integer = -1
End Module