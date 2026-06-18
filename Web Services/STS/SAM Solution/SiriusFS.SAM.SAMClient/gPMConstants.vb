Option Strict Off
Option Explicit On
Module gPMConstants
	
	' ***************************************************************** '
	' Module Name: gPMConstants
	'
	' Date: 14th January 1998
	'
	' Description: This class maps ALL of the constants currently
	'              provided by PMConst.bas
	'
	'              Numeric constants are implemented by Enums.
	'
	'              Note: To maintain backwards compatibility the enum
	'              members will still be prefixed 'PM', NOT 'pme'. The
	'              enum itself will be prefixed PME however as this will
	'              not have been used in any previous code.
	'
	'              String constants are implemented by a Property Get
	'
	' Edit History: 14/01/98    Original created                     RFC
	'
	' BB030298 - Added DisplayMode enum for Navigator style variation
	' BB040298 - Added Control Type constants for FormControl
	' RFC270298 - Navigator Key Names moved to gSirLibs as they are
	'             Sirius specific.
	' RFC270298 - Type of Business removed as it is not needed.
	' RFC040398 - DSN's Added for Mercury, Documaster, DocumasterV2
	' RFC040398   and DocumasterScan.
	' RFC250398 - DSN's added for Sirius Architecture DB
	' RFC070498 - Broking Link Constants Added
	' RFC200498 - Architecture In Debug Reg Key constant added.
	' RFC210498 - Architecture Local Enabled Reg Key constant added.
	' RFC050698 - Sirius Broking DSN added.
	' RFC080598 - LogMessage Default Log File changed to "C:\Sirius.Log"
	' RFC170698 - Architecture Server Enabled Reg Key constant added.
	' RFC190698 - Format Month & Day Long, Medium and Short added
	' RFC190898 - QueryTimeoutSeconds Reg Key Added.
	' RFC190898 - SiriusSolutions & Nirvana Product Family and DSN's added.
	'             Note: These are all set to use the Sirius DSN at present.
	' RFC161098 - Added IncorrectDateFormat & SystemDate return codes.
	' RFC140199 - Added SplashBitMap registry key and Abort/Complete Nav Actions.
	' RFC180299 New Constants Added for SA1.4
	' RFC060799 - Added GeminiII Product Family, DSN etc etc
	' DAK280999 - New Process Mode constants
	' DAK071099 - Work Manager constants moved here.
	' DAK121099 - New responses for Licencing
	' DAK051199 - Privilege levels for amending lookup tables
	' DAK011299 - More lookup table privilege levels
	' DAK141299 - Add is_visible column to task instance
	' DAK241299 - More registry settings for Work Manager
	' DAK110100 - More registry settings for Work Manager
	' DAK250100 - Registry setting to allow error retry in Navigator
	' DAK190600 - Registry setting to determine "PM News" tab name on Work Manager
	' DAK110700 - Registry setting for Work Manager Main Form Caption
	'             Registry setting for PM Support web address
	' RDC12062002 - changed to BAS module
	' RDC12062002 - Constants and PMConstants merged
	' ***************************************************************** '
	
	' RDC 13062002 cosntants moved from RegistryFunctions ################################ START
	Public Const HKEY_CLASSES_ROOT As Integer = &H80000000
	Public Const HKEY_CURRENT_USER As Integer = &H80000001
	Public Const HKEY_LOCAL_MACHINE As Integer = &H80000002
	Public Const HKEY_USERS As Integer = &H80000003
	Public Const HKEY_PERFORMANCE_DATA As Integer = &H80000004
	Public Const HKEY_CURRENT_CONFIG As Integer = &H80000005
	Public Const HKEY_DYN_DATA As Integer = &H80000006
	
	Public Const ACRegRoot As String = "software\PM"
	
	Public Const ACRegSiriusArchitecture As String = "\SiriusArchitecture"
	Public Const ACRegSiriusUnderwriting As String = "\SiriusUnderwriting"
	Public Const ACRegSiriusBroking As String = "\SiriusBroking"
	Public Const ACRegOrion As String = "\Orion"
	Public Const ACRegGemini As String = "\Gemini"
	Public Const ACRegVoyager As String = "\Voyager"
	Public Const ACRegMercury As String = "\Mercury"
	Public Const ACRegDocumaster As String = "\Documaster"
	'RFC251198 - Added SiriusSolutions & Nirvana Registry Constants
	Public Const ACRegSiriusSolutions As String = "\SiriusSolutions"
	Public Const ACRegNirvana As String = "\Nirvana"
	'RFC060799 - Added GeminiII Product Family, DSN etc etc
	Public Const ACRegGeminiII As String = "\GeminiII"
	Public Const ACRegClaims As String = "\Claims"
	
	Public Const ACRegClient As String = "\Client"
	Public Const ACRegServer As String = "\Server"
	Public Const ACRegCommon As String = "\Common"
	' RDC 19072002
	Public Const ACRegSetup As String = "\Setup"
	
	
	Public Const REG_NONE As Integer = 0
	Public Const REG_SZ As Integer = 1
	Public Const REG_EXPAND_SZ As Integer = 2
	Public Const REG_BINARY As Integer = 3
	Public Const REG_DWORD As Integer = 4
	Public Const REG_LINK As Integer = 6
	Public Const REG_MULTI_SZ As Integer = 7
	Public Const REG_RESOURCE_LIST As Integer = 8
	
	Public Const ERROR_NONE As Short = 0
	Public Const ERROR_BADDB As Short = 1
	Public Const ERROR_BADKEY As Short = 2
	Public Const ERROR_CANTOPEN As Short = 3
	Public Const ERROR_CANTREAD As Short = 4
	Public Const ERROR_CANTWRITE As Short = 5
	Public Const ERROR_OUTOFMEMORY As Short = 6
	Public Const ERROR_INVALID_PARAMETER As Short = 7
	Public Const ERROR_ACCESS_DENIED As Short = 8
	Public Const ERROR_INVALID_PARAMETERS As Short = 87
	Public Const ERROR_NO_MORE_ITEMS As Short = 259
	
	Public Const KEY_ALL_ACCESS As Short = &H3Fs
	
	Public Const REG_OPTION_NON_VOLATILE As Short = 0
	' #################################################################################### END
	
	' Type Of Business (Navigator)
	Public Const PMTypeOfBusinessGeneric As String = ""
	Public Const PMTypeOfBusinessNB As String = "NB"
	Public Const PMTypeOfBusinessRN As String = "RN"
	Public Const PMTypeOfBusinessENN As String = "ENN"
	Public Const PMTypeOfBusinessENR As String = "ENR"
	
	'TF031297 - PMMsgBox Caption & Message constants added
	Public Const PMKeyNameMsgBoxCaption As String = "message_box_caption"
	Public Const PMKeyNameMsgBoxText As String = "message_box_text"
	
	Public Const PMKeyNameDecisionTitle As String = "decision_title"
	Public Const PMKeyNameDecisionText As String = "decision_text"
	
	'RFC180299 New Constants Added for SA1.4
	Private Const ACSiriusArchitecture As String = "Sirius"
	Private Const ACSiriusUnderwriting As String = "SirUnd"
	Private Const ACOrion As String = "Orion"
	Private Const ACGemini As String = "Gemini"
	Private Const ACVoyager As String = "Voyager"
	Private Const ACMercury As String = "Mercury"
	Private Const ACDocumaster As String = "Documaster"
	Private Const ACSiriusBroking As String = "SirBroking"
	Private Const ACSiriusSolutions As String = "SirSol"
	Private Const ACNirvana As String = "Nirvana"
	'RFC060799 - Added GeminiII Product Family, DSN etc etc
	Private Const ACGeminiII As String = "GeminiII"
	' RDC 07082000 - new product family: Claims
	Private Const ACClaims As String = "Claims"
	
	' RDC 19072002 - for Windows Terminal Services functions in gPMFunctions
	Private Const WTS_CURRENT_SERVER_HANDLE As Short = 0
	Private Const WTS_CURRENT_SESSION_HANDLE As Short = -1
	
	
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
	
	' ***************************************************************** '
	' * Return Codes                                                  * '
	' ***************************************************************** '
	Public Enum PMEReturnCode
		' General
		'********
		PMFalse = 0
		PMTrue = 1
		PMFail = 10
		PMError = 11
		PMSucceed = 12
		PMOK = 20
		PMCancel = 21
		PMNavigate = 30
		' Broking Link Set 1
		' RFC070498
		'*************
		PMMNoAuthority = 51
		PMMAlreadyInUse = 52
		PMMInvalidPassword = 53
		PMMNoAccess = 54
		' System
		'*******
		PMIncorrectUsername = 200
		PMIncorrectPassword = 201
		PMLoggedOnElsewhere = 202
		PMLogError1 = 210
		PMLogError2 = 211
		' Interface
		'**********
		PMMoveStatusBack = 400
		PMMoveStatusNext = 401
		PMMoveStatusCancel = 402
		PMMoveStatusFinish = 403
		' Broking Link Set 2
		' RFC070498
		'*************
		PMError_argcount = 500
		PMError_protocol = 501
		PMError_notconnected = 502
		PMError_timeout = 503
		PMError_usage = 504
		' Business
		'*********
		PMLogonExceeded = 600
		PMLicenceExceeded = 601
		PMInvalidLicenceKey = 602
		PMDataChanged = 610
		PMMandatoryMissing = 611
		PMDataNotChanged = 612
		PMInvalidRequest = 620
		PMIncorrectDateFormat = 621
		PMIncorrectSystemDate = 622
		'RFC180299 New Constants Added for SA1.4
		PMEarlier = 623
		PMLater = 624
		PMInstallStarted = 625
		'DAK121099 - New responses for Licencing
		PMBlockLicenceExceeded = 626
		PMWarnLicenceExceeded = 627
		' Navigator Return Codes
		'***********************
		PMNavStartNewProcess = 700
		PMNavCallComponent = 701
		PMNavBuildMap = 702
		PMNavRepeatMap = 703
		PMNavEndMap = 704
		PMNavNavigate = 705
		PMNavEndProcess = 706
		' Database
		'*********
		PMRecordChanged = 800
		PMRecordDeleted = 801
		PMRecordInUse = 810
		PMNotFound = 811
		PMBOF = 820
		PMEOF = 821
		' Broking Link Set 3
		' RFC070498
		'*************
		PMNoHostRegistry = 1002
		PMNoPortRegistry = 1003
		PMNoConnection = 1004
		PMNoPMLink = 1005
		PMNoCompanies = 1006
	End Enum
	
	' ***************************************************************** '
	' * Constants for Message Logging
	' ***************************************************************** '
	' ***************************************************************** '
	' * Log Levels
	' ***************************************************************** '
	Public Enum PMELogLevel
		PMLogFatal = 1
		PMLogError = 2
		PMLogWarning = 3
		PMLogInfo = 4
		PMLogOnError = 5
		PMLogDebug1 = 6
		PMLogDebug2 = 7
		PMLogDebug3 = 8
		PMLogDebug4 = 9
	End Enum
	
	' ***************************************************************** '
	' Column positions for PMLock table
	' ***************************************************************** '
	Public Enum PMEPMLockColumnPosition
		PMLockFormAllName = 0
		PMLockFormAllValue = 1
		PMLockFormAllUser = 2
		PMLockFormAllTime = 3
		PMLockFormAllUserID = 4
	End Enum
	
	' ***************************************************************** '
	' Resource file data types.
	' ***************************************************************** '
	Public Enum PMEResourseFileDataType
		PMResString = 0
		PMResBitmap = 1
		PMResIcon = 2
		PMResCursor = 3
	End Enum
	
	' ***************************************************************** '
	' Period constants.
	' ***************************************************************** '
	Public Enum PMETimePeriod
		PMDay = 0
		PMWeek = 1
		PMMonth = 2
		PMYear = 3
	End Enum
	
	' ***************************************************************** '
	' AutoNumbering Number Types
	' ***************************************************************** '
	Public Enum PMEAutoNumberType
		PMAutoNumInsFile = 1
		PMAutoNumInsFolder = 2
		PMAutoNumRiskFolder = 3
		PMAutoNumParty = 4
		PMAutoNumContact = 5
		PMAutoNumAddress = 6
	End Enum
	
	' ***************************************************************** '
	' Caption Array Column Position Constants
	' ***************************************************************** '
	Public Enum PMENavCaptionArrayColPosition
		PMNavCaptionStepKey = 0
		PMNavCaptionCaption = 1
		PMNavCaptionIsSubMap = 2
		PMNavCaptionComponentType = 3
	End Enum
	
	' ***************************************************************** '
	' Key Let/Get Column Position Constants
	' ***************************************************************** '
	Public Enum PMENavLetGetKeyColPosition
		PMKeyName = 0
		PMKeyValue = 1
	End Enum
	
	' ***************************************************************** '
	' Summary Detail Type Constants
	' ***************************************************************** '
	Public Enum PMENavSummaryLevel
		PMNavSummProcessSummary = 0
		PMNavSummMapInstances = 1
		PMNavSummMapSummary = 2
	End Enum
	
	' ***************************************************************** '
	' Summary Detail Array Column Position Constants
	' ***************************************************************** '
	Public Enum PMENavSummaryArrayColPosition
		PMNavSummLevel = 0
		PMNavSummHeading = 1
		PMNavSummValue = 2
	End Enum
	
	' ***************************************************************** '
	' Status type values.
	' ***************************************************************** '
	Public Enum PMEComponentAction
		PMView = 0
		PMAdd = 1
		PMEdit = 2
		PMDelete = 3
		PMDummyDelete = 4
		PMAdded = 10
	End Enum
	
	' ***************************************************************** '
	' Status type values.
	'
	' Process Mode values  (Navigator)
	'
	' Only use the following values for these constants :
	' 0,1,2,4,8,16,32,64,128,256,512,1024,2048,4096,8192,16384
	' ***************************************************************** '
	Public Enum PMEProcessMode
		PMProcessModeGeneric = 0
		PMProcessModeEnquiry = 1
		PMProcessModeNBQuote = 2
		PMProcessModeNBLive = 4
		PMProcessModeRNQuote = 8
		PMProcessModeRNLive = 16
		PMProcessModeMTAQuote = 32
		PMProcessModeMTALive = 64
		'DAK280999
		PMProcessModeFull = 101
		PMProcessModePostQuote = 102
		PMProcessModeSpecific = 103
		PMProcessModeStartAtQuote = 104
		PMProcessModeDefault = 105
		PMProcessModeReview = 106
		PMProcessModeCancellations = 107
		PMProcessModeClaims = 108
		PMProcessModeOverride = 109
	End Enum
	
	' ***************************************************************** '
	' Navigator button status.
	' ***************************************************************** '
	Public Enum PMENavigateButtonStatus
		PMNavigateNotRequired = 0
		PMNavigateEnabled = 1
		PMNavigateDisabled = 2
	End Enum
	
	' ***************************************************************** '
	' Mouse pointer states.
	' ***************************************************************** '
	Public Enum PMEMousePointerStatus
		PMMouseBusy = 0
		PMMouseNormal = 1
		PMMouseReset = 2
	End Enum
	
	' ***************************************************************** '
	' Formatting values.
	' ***************************************************************** '
	Public Enum PMEFormatStyle
		PMFormatString = 0
		PMFormatStringCase = 1
		PMFormatDateShort = 2
		PMFormatDateMedium = 3
		PMFormatDateLong = 4
		PMFormatTimeShort = 5
		PMFormatTimeMedium = 6
		PMFormatTimeLong = 7
		PMFormatDateTimeShort = 8
		PMFormatDateTimeMedium = 9
		PMFormatDateTimeLong = 10
		PMFormatCurrency = 11
		PMFormatInteger = 12
		PMFormatBoolean = 13
		PMFormatStringUpper = 14
		PMFormatDateYearOnly = 15
		PMFormatPercent = 16
		PMFormatDouble = 17
		PMFormatLong = 18
		' BB151297 - Added constants for Voyager Form Tools
		' This format uses local currency symbol and drops decimal amounts
		PMFormatWholeMoney = 19
		' This format uses local currency symbol
		PMFormatMoney = 20
		PMFormatDecimal = 21 'JY120298
		' RFC19061998 - Month & Day Long, Medium and Short added
		PMFormatMonthOnlyLong = 22
		PMFormatMonthOnlyMedium = 23
		PMFormatMonthOnlyShort = 24
		PMFormatDayOnlyLong = 25
		PMFormatDayOnlyMedium = 26
		PMFormatDayOnlyShort = 27
		PMFormatStringMultiLine = 28
		' Lookup list refresh value.
		PMListRefreshValue = 30
		PMFormatPercentFourDecimal = 31
	End Enum
	
	' ***************************************************************** '
	' Business Object Action Values.
	' ***************************************************************** '
	Public Enum PMEBusinessObjectAction
		PMActionView = 0
		PMActionEditAdd = 1
		PMActionEditUpdate = 2
		PMActionEditDelete = 3
		PMActionGetDefault = 4
		PMActionGetMandatory = 5
	End Enum
	
	' ***************************************************************** '
	' Business Object Mandatory constants
	' ***************************************************************** '
	Public Enum PMEMandatoryStatus
		PMNonMandatory = 0
		PMMandatory = 1
		PMNonVisible = 2
	End Enum
	
	' ***************************************************************** '
	' PMLookup constants
	' ***************************************************************** '
	' ***************************************************************** '
	' Column positions for input array.
	' ***************************************************************** '
	Public Enum PMELookupInArrayColPos
		PMLookupTableName = 0
		PMLookupKey = 1
		PMLookupStartPos = 2
		PMLookupNumOfItems = 3
	End Enum
	' ***************************************************************** '
	' DAK051199
	'DAK011299 - all change
	' Privilege levels for amending the lookup tables
	' ***************************************************************** '
	Public Enum PMELookupEditPrivlegeLevel
		PMLookupNoEdit = 0
		PMLookupViewOnly = 1
		PMLookupAmendCaptions = 2
		PMLookupFullPrivileges = 3
		PMLookupAdminViewUserNone = 4
		PMLookupAdminCaptionsUserNone = 5
		PMLookupAdminCaptionsUserView = 6
		PMLookupAdminFullUserNone = 7
		PMLookupAdminFullUserView = 8
		PMLookupAdminFullUserCaptions = 9
	End Enum
	
	' ***************************************************************** '
	' Column positions for output array.
	' ***************************************************************** '
	Public Enum PMELookupOutArrayColPos
		PMLookupID = 0
		PMLookupCaption = 1
		PMLookupCode = 2
	End Enum
	
	' ***************************************************************** '
	' Type of Lookup Required
	' ***************************************************************** '
	Public Enum PMELookupType
		PMLookupAll = 0
		PMLookupSingle = 1
		PMLookupAllEffective = 2
	End Enum
	
	' ***************************************************************** '
	' Variable Data Array Column Position Constants
	'
	' Note: For the Full List of Variable Data Record/Fields name
	'       etc see VarDataConst.bas
	' ***************************************************************** '
	Public Enum PMEVarDataArrayColPos
		PMVarRecordID = 0
		PMVarRecordName = 1
		PMVarFieldName = 2
		PMVarFieldType = 3
		PMVarDefaultFormat = 4
		PMVarLength = 5
		PMVarValidationID = 6
		PMVarLocatorType = 7
		PMVarCoreTable = 8
		PMVarCoreVariable = 9
		PMVarMandatoryLevel = 10
		PMVarValue = 11
	End Enum
	
	' ***************************************************************** '
	' Variable Data Valid Value Column Position Constants
	' ***************************************************************** '
	Public Enum PMEVarValidValueArrayColPos
		PMVarValValidationID = 0
		PMVarValValidValueID = 1
		PMVarValIsDefault = 2
		PMVarValCode = 3
		PMVarValNumericValue = 4
		PMVarValCaption = 5
	End Enum
	
	' ***************************************************************** '
	' Variable Data Core Variable Array Constants
	' ***************************************************************** '
	Public Enum PMEVarCoreVariableArrayColPos
		PMVarCoreVariableName = 0
		PMVarCoreVariableValue = 1
	End Enum
	
	' ***************************************************************** '
	' Variable Data Locator Array constants
	' ***************************************************************** '
	Public Enum PMEVarLocatorArrayColPos
		PMVarLocatorTypeID = 0
		PMVarLocatorTypeValue = 1
	End Enum
	
	' ***************************************************************** '
	' True/False in the Variable Data World
	' ***************************************************************** '
	Public Enum PMEVarTrueFalse
		PMVarFalse = 0
		PMVarTrue = 1
	End Enum
	
	' ***************************************************************** '
	' Lock Mode Constants
	' ***************************************************************** '
	Public Enum PMELockMode
		PMNoLock = 0
	End Enum
	
	Public Enum PMEStringCompareType
		PMBinaryCompare = 0
		PMStringCompare = 1
	End Enum
	
	' ***************************************************************** '
	' PMDAO Parameter Direction
	' ***************************************************************** '
	Public Enum PMEParameterDirection
		PMParamInput = 0
		PMParamInputOutput = 1
		PMParamOutput = 2
		PMParamReturnValue = 3
		PMParamDefault = 4
	End Enum
	
	' ***************************************************************** '
	' Data Types
	' ***************************************************************** '
	Public Enum PMEDataType
		PMString = 0
		PMInteger = 1
		PMLong = 2
		PMDouble = 3
		PMDate = 4
		PMBoolean = 5
		PMCurrency = 6
		PMBinary = 7
		' These two are used by PMLookup
		PMTableName = 8
		PMFieldName = 9
		' These two are used by variable data
		PMUniqueKey = 10
		PMCode = 11
		' RFC 18/09/1997 Added for VB Decimal Support
		PMDecimal = 12
		' BB 06/10/1997 Added for PMFormControl as a Field Type
		PMLookup = 13
	End Enum
	
	' ***************************************************************** '
	' BB030298
	' Display Mode used to vary the style of form displayed
	' ***************************************************************** '
	Public Enum PMEDisplayMode
		PMDisplayStandard = 0
		PMDisplaySimple = 1
	End Enum
	
	' ***************************************************************** '
	' BB040298
	' Display Mode used to vary the style of form displayed
	' ***************************************************************** '
	Public Enum PMEControlType
		PMUnknownCtlType = 0
		PMTextBox = 1
		PMCombo = 2
		PMCheckBox = 3
		PMListBox = 4
		PMGrid = 5
		PMSpread = 6
		PMOptionButton = 7
	End Enum
	
	'DAK071099
	'Work Manager Constants
	' Type of Task
	Public Enum PMEWrkManTaskType
		pmeWMTTMemo = 0
		pmeWMTTSingleComponent = 1
		pmeWMTTNavigatorProcess = 2
	End Enum
	
	' Task Status
	Public Enum PMEWrkManTaskStatus
		pmeWMTSNew = 0
		pmeWMTSInProgress = 1
		pmeWMTSIncomplete = 2
		pmeWMTSComplete = 3
		pmeWMTSDeleted = 4
	End Enum
	
	
	' ***************************************************************** '
	' Shared Between iPMWrkManager& bPMWrkManager
	
	' Column Positions for Available Task Array
	Public Enum PMEACAvailTaskCol
		ACAvailTaskGroupIDCol = 0
		ACAvailTaskIDCol = 1
		ACAvailTaskCaptionCol = 2
		ACAvailTaskIsSupervisorCol = 3
		ACAvailTaskIsSystemTaskCol = 4
		ACAvailTaskTypeOfTaskCol = 5
		ACAvailTaskPMNavProcessIDCol = 6
		ACAvailTaskObjectNameCol = 7
		ACAvailTaskClassNameCol = 8
		ACAvailTaskDeleteAfterDaysCol = 9
		ACAvailTaskDisplayIconCol = 10
		ACAvailTaskIsViewOnlyTaskCol = 11
		ACAvailTaskLinkedObjectNameCol = 12
		ACAvailTaskLinkedClassNameCol = 13
		ACAvailTaskLinkedCaptionCol = 14
	End Enum
	
	Public Enum PMEACSchedTaskCol
		ACSchedTaskInstanceCntCol = 0
		ACSchedTaskUrgentCol = 1
		ACSchedTaskStatusCol = 2
		ACSchedTaskTypeCol = 3
		ACSchedTaskIsSystemCol = 4
		ACSchedTaskDueDateCol = 5
		ACSchedTaskCustomerCol = 6
		ACSchedTaskDescriptionCol = 7
		ACSchedTaskUserGroupIDCol = 8
		ACSchedTaskUserIDCol = 9
		ACSchedTaskNavProcessIDCol = 10
		ACSchedTaskObjectNameCol = 11
		ACSchedTaskClassNameCol = 12
		ACSchedTaskDisplayIconCol = 13
		ACSchedTaskIsViewOnlyTaskCol = 14
		ACSchedTaskLinkedObjectNameCol = 15
		ACSchedTaskLinkedClassNameCol = 16
		ACSchedTaskLinkedCaptionCol = 17
		'DAK141299
		ACSchedTaskIsVisibleCol = 18
	End Enum
	
	Public Enum PMEACQuickStartCol
		ACQSTaskGroupIDCol = 0
		ACQSTaskIDCol = 1
	End Enum
	
	' ***************************************************************** '
	
	' ***************************************************************** '
	' Shared Between iPMWrkTaskInstLog & bPMWrkTaskInstLog
	
	' Log Entries Array Select Column Positions
	' Note: If these vales change then the SQL Statement must also change..
	' spe_PMWrk_Task_Inst_Log_sad
	Public Enum PMELogEntriesArrayColPos
		pmeLEACPTaskInstCnt = 0
		pmeLEACPDateCreated = 1
		pmeLEACPText = 2
		pmeLEACPCreatedById = 3
	End Enum
	' Enumerator for PMAutoNumber component, InsuranceFile reference
	Public Enum PMEAutoNumInsFileRefType
		pmeRefTypeNBQuotation = 1
		pmeRefTypeNBMakeLive = 2
		pmeRefTypeRenewalNotice = 3
		pmeRefTypeRenewalUpdate = 4
	End Enum
	
	' Enumerator for number of decimal places allowed for currency fields
	Public Enum PMECurrencyNoOfDP
		pmeCurDPZero = 0
		pmeCurDPOne = 1
		pmeCurDPTwo = 2
		pmeCurDPThree = 3
		pmeCurDPFour = 4
	End Enum
	
	' Enumerator for number of decimal places allowed for vdecimal fields
	Public Enum PMEVDecimalNoOfDP
		pmeVDecimalDPZero = 0
		pmeVDecimalDPOne = 1
		pmeVDecimalDPTwo = 2
		pmeVDecimalDPThree = 3
		pmeVDecimalDPFour = 4
		pmeVDecimalDPFive = 5
		pmeVDecimalDPSix = 6
	End Enum
	
	' Enumerator for the roundup factor
	Public Enum PMERoundupFactor
		pmeRFactor00Up = 0
		pmeRFactor49Up = 51
		pmeRFactor50Up = 50
		pmeRFactor51Up = 49
		pmeRFactor55Up = 45
		pmeRFactor99Up = 1
	End Enum
	
	' ***************************************************************** '
	' RFC040398
	' Database Type
	' ***************************************************************** '
	Public Enum PMEDatabaseType
		pmeDBTypeUnknown = -1
		pmeDBTypeMSAccess = 0
		pmeDBTypeMSSQLServer = 1
		pmeDBTypeSybaseSQLAnywhere = 2
	End Enum
	
	' ***************************************************************** '
	' RFC130398
	' Policy Master Product Families
	' ***************************************************************** '
	' Note: If you are adding a New Product Family
	'       then you need to amend the following :
	'
	' gPMLibraries.Constants.PMProductCode
	' gPMLibraries.Constants.PMProductFamilyByCode
	' gPMLibraries.PMFunctions.BuildKeyString
	' gPMLibraries.PMConstants - Add associated DSN/Database.
	' sPMServerCS.PMServerBusinessCS.GetDSN
	' dPMDAO.Database.CheckDSN
	Public Enum PMEProductFamily
		pmePFSiriusArchitecture = 1
		pmePFSiriusUnderwriting = 2
		pmePFOrion = 3
		pmePFGemini = 4
		pmePFVoyager = 5
		pmePFMercury = 6
		pmePFDocumaster = 7
		pmePFSiriusBroking = 8
		pmePFSiriusSolutions = 9
		pmePFNirvana = 10
		'RFC060799 - Added GeminiII Product Family, DSN etc etc
		pmePFGeminiII = 11
		' RDC 07082000 - new product family: Claims
		pmePFClaims = 12
	End Enum
	
	' ***************************************************************** '
	' RFC200498
	' MAPI Recipient Types = MSMAPI.RecipTypeConstants
	' ***************************************************************** '
	Public Enum PMEMapiRecipientTypes
		pmeMapiOrigList = 0
		pmeMapiToList = 1
		pmeMapiCcList = 2
		pmeMapiBccList = 3
	End Enum
	
	' ***************************************************************** '
	' RFC200498
	' MAPI Attachment Types = MSMAPI. AttachTypeConstants
	' ***************************************************************** '
	Public Enum PMEMapiAttachmentTypes
		pmeMapiData = 0
		pmeMapiEOLE = 1
		pmeMapiSOLE = 2
	End Enum
	
	' ***************************************************************** '
	' RFC050698
	' Policy Master Reg Setting Root
	' ***************************************************************** '
	Public Enum PMERegSettingRoot
		pmeRSRLocalMachine = 1
		pmeRSRCurrentUser = 2
	End Enum
	
	' ***************************************************************** '
	' RFC050698
	' Policy Master Reg Setting Level
	' ***************************************************************** '
	Public Enum PMERegSettingLevel
		pmeRSLClient = 1
		pmeRSLServer = 2
		pmeRSLCommon = 3
		pmeRSLSetup = 4
	End Enum
	
	' ***************************************************************** '
	' RFC201198
	' Authority Level
	' ***************************************************************** '
	Public Enum PMEAuthorityLevel
		pmeALUser = 0
		pmeALSupervisor = 1
		pmeALSysAdmin = 2
	End Enum
	' ***************************************************************** '
	
	' ***************************************************************** '
	' These constants are used by iPMTaskGroupMaintenance and PMWorkManager
	' to display the correct Icon for a Task Group.
	Public Enum PMEACTaskGroupIcon
		ACTaskGroupIconIndexClient = 1
		ACTaskGroupIconIndexPolicy = 2
		ACTaskGroupIconIndexQuote = 3
		ACTaskGroupIconIndexClaim = 4
		ACTaskGroupIconIndexAccount = 5
		ACTaskGroupIconIndexReport = 6
		ACTaskGroupIconIndexAgent = 7
		ACTaskGroupIconIndexAdmin = 8
		ACTaskGroupIconIndexRenewals = 9
		ACTaskGroupIconIndexStatistics = 10
		ACTaskGroupIconIndexGeneral = 11
	End Enum
	
	' ***************************************************************** '
	' * Log Level Descriptions
	' ***************************************************************** '
	Public ReadOnly Property PMFatalText() As String
		Get
			PMFatalText = "Fatal"
		End Get
	End Property
	Public ReadOnly Property PMErrorText() As String
		Get
			PMErrorText = "Error"
		End Get
	End Property
	Public ReadOnly Property PMWarningText() As String
		Get
			PMWarningText = "Warning"
		End Get
	End Property
	Public ReadOnly Property PMInfoText() As String
		Get
			PMInfoText = "Information"
		End Get
	End Property
	Public ReadOnly Property PMOnErrorText() As String
		Get
			PMOnErrorText = "Recoverable Error"
		End Get
	End Property
	Public ReadOnly Property PMDebug1Text() As String
		Get
			PMDebug1Text = "Debug 1"
		End Get
	End Property
	Public ReadOnly Property PMDebug2Text() As String
		Get
			PMDebug2Text = "Debug 2"
		End Get
	End Property
	Public ReadOnly Property PMDebug3Text() As String
		Get
			PMDebug3Text = "Debug 3"
		End Get
	End Property
	Public ReadOnly Property PMDebug4Text() As String
		Get
			PMDebug4Text = "Debug 4"
		End Get
	End Property
	
	' ***************************************************************** '
	' * Log Level Descriptions
	' ***************************************************************** '
	Public ReadOnly Property PMDefaultLogFile() As String
		Get
			PMDefaultLogFile = "C:\Sirius.Log"
		End Get
	End Property
	
	' ***************************************************************** '
	' * System/Product Details
	' ***************************************************************** '
	Public ReadOnly Property PMProduct() As String
		Get
			PMProduct = "SIRIUS"
		End Get
	End Property
	Public ReadOnly Property PMCustomer() As String
		Get
			PMCustomer = "AIG"
		End Get
	End Property
	
	' ***************************************************************** '
	' ClientManager/LicenceManager Timeout settings
	' ***************************************************************** '
	Public ReadOnly Property PMPollEverySeconds() As Integer
		Get
			PMPollEverySeconds = 30
		End Get
	End Property
	Public ReadOnly Property PMTimeOutSeconds() As Integer
		Get
			PMTimeOutSeconds = 90
		End Get
	End Property
	
	' ***************************************************************** '
	' Constants for the logon attempts.
	' ***************************************************************** '
	Public ReadOnly Property PMLogonAttempts() As Integer
		Get
			PMLogonAttempts = 3
		End Get
	End Property
	
	' ***************************************************************** '
	' Resource file language offset value.
	' ***************************************************************** '
	Public ReadOnly Property PMLangOffSetValue() As Integer
		Get
			PMLangOffSetValue = 1000
		End Get
	End Property
	
	' ***************************************************************** '
	' Registry constants
	' ***************************************************************** '
	' ***************************************************************** '
	' Application
	' ***************************************************************** '
	Public ReadOnly Property PMRegAppName() As String
		Get
			PMRegAppName = "Sirius"
		End Get
	End Property
	
	'DAK071099
	' Work Manager Constants
	Public ReadOnly Property ACWrkManRegSubKey() As String
		Get
			ACWrkManRegSubKey = "WorkManager"
		End Get
	End Property
	
	Public ReadOnly Property ACWrkManRegWebAddress() As String
		Get
			ACWrkManRegWebAddress = "PMNewsWebAddress"
		End Get
	End Property
	
	'DAK190600
	Public ReadOnly Property ACWrkManRegWebTabCaption() As String
		Get
			ACWrkManRegWebTabCaption = "WebTabCaption"
		End Get
	End Property
	
	'DAK110700
	Public ReadOnly Property ACWrkManRegFormCaption() As String
		Get
			ACWrkManRegFormCaption = "FormCaption"
		End Get
	End Property
	
	Public ReadOnly Property ACWrkManRegSupportWebAddress() As String
		Get
			ACWrkManRegSupportWebAddress = "PMSupportWebAddress"
		End Get
	End Property
	
	Public ReadOnly Property ACWrkManRegViewSplash() As String
		Get
			ACWrkManRegViewSplash = "ViewSplashScreen"
		End Get
	End Property
	
	Public ReadOnly Property ACWrkManRegViewQuickStart() As String
		Get
			ACWrkManRegViewQuickStart = "ViewQuickStart"
		End Get
	End Property
	
	Public ReadOnly Property ACWrkManRegViewAvailableTasks() As String
		Get
			ACWrkManRegViewAvailableTasks = "ViewAvailableTasks"
		End Get
	End Property
	
	'DAK241299
	Public ReadOnly Property ACWrkManRegViewToolbar() As String
		Get
			ACWrkManRegViewToolbar = "ViewToolbar"
		End Get
	End Property
	
	Public ReadOnly Property ACWrkManRegViewStatusBar() As String
		Get
			ACWrkManRegViewStatusBar = "ViewStatusBar"
		End Get
	End Property
	
	Public ReadOnly Property ACWrkManRegViewGridLines() As String
		Get
			ACWrkManRegViewGridLines = "ViewGridLines"
		End Get
	End Property
	
	Public ReadOnly Property ACWrkManRegViewGraphics() As String
		Get
			ACWrkManRegViewGraphics = "ViewGraphics"
		End Get
	End Property
	
	'DAK110100
	Public ReadOnly Property ACWrkManRegIsAutoRefresh() As String
		Get
			ACWrkManRegIsAutoRefresh = "IsAutoRefresh"
		End Get
	End Property
	
	Public ReadOnly Property ACWrkManRegRefreshRate() As String
		Get
			ACWrkManRegRefreshRate = "RefreshRate"
		End Get
	End Property
	
	' ***************************************************************** '
	' Sections
	' ***************************************************************** '
	Public ReadOnly Property PMRegSecSystem() As String
		Get
			PMRegSecSystem = "System"
		End Get
	End Property
	Public ReadOnly Property PMRegSecLicence() As String
		Get
			PMRegSecLicence = "LicenceManager"
		End Get
	End Property
	
	' ***************************************************************** '
	' Keys
	' ***************************************************************** '
	Public ReadOnly Property PMRegKeyPoolSize() As String
		Get
			PMRegKeyPoolSize = "PoolSize"
		End Get
	End Property
	Public ReadOnly Property PMRegKeyLogFile() As String
		Get
			PMRegKeyLogFile = "LogFileName"
		End Get
	End Property
	Public ReadOnly Property PMRegKeyLogLevel() As String
		Get
			PMRegKeyLogLevel = "UserLogLevel"
		End Get
	End Property
	' RFC200498
	Public ReadOnly Property PMRegKeyArchitectureInDebug() As String
		Get
			PMRegKeyArchitectureInDebug = "ArchitectureInDebug"
		End Get
	End Property
	' RFC210498
	Public ReadOnly Property PMRegKeyArchitectureLocalEnabled() As String
		Get
			PMRegKeyArchitectureLocalEnabled = "ArchitectureLocalEnabled"
		End Get
	End Property
	' RFC170698
	Public ReadOnly Property PMRegKeyArchitectureServerEnabled() As String
		Get
			PMRegKeyArchitectureServerEnabled = "ArchitectureServerEnabled"
		End Get
	End Property
	' RDC 11072002
	Public ReadOnly Property PMRegKeyArchitectureUnifiedLogon() As String
		Get
			PMRegKeyArchitectureUnifiedLogon = "ArchitectureUnifiedLogon"
		End Get
	End Property
	' RFC19/08/1998
	Public ReadOnly Property PMRegKeyQueryTimeoutSeconds() As String
		Get
			PMRegKeyQueryTimeoutSeconds = "QueryTimeoutSeconds"
		End Get
	End Property
	
	'RFC140199
	Public ReadOnly Property PMRegKeySplashBitMap() As String
		Get
			PMRegKeySplashBitMap = "SplashBitMap"
		End Get
	End Property
	
	' RFC180299 New Constants Added for SA1.4
	Public ReadOnly Property PMRegKeyVersion() As String
		Get
			PMRegKeyVersion = "Version"
		End Get
	End Property
	
	'DAK130100
	Public ReadOnly Property ACRegKeyViewGraphics() As String
		Get
			ACRegKeyViewGraphics = "ViewGraphics"
		End Get
	End Property
	
	'DAK250100
	'Navigator registry settings
	Public ReadOnly Property ACNavRegSubKey() As String
		Get
			ACNavRegSubKey = "Navigator"
		End Get
	End Property
	
	Public ReadOnly Property ACRegKeyAllowErrorRetry() As String
		Get
			ACRegKeyAllowErrorRetry = "AllowErrorRetry"
		End Get
	End Property
	
	' RDC 09042002
	' ***************************************************************** '
	' COM+ registry settings
	' ***************************************************************** '
	' COM+ client manager string name. Location: PM\SiriusArchitecture\Server\ClientManagerCOMPlus
	Public ReadOnly Property ACRegKeyClientManagerCOMPlus() As String
		Get
			ACRegKeyClientManagerCOMPlus = "ClientManagerCOMPlus"
		End Get
	End Property
	
	' ***************************************************************** '
	' Reference Fields
	' ***************************************************************** '
	Public ReadOnly Property PMRefFieldCoverStartDate() As String
		Get
			PMRefFieldCoverStartDate = "CSD"
		End Get
	End Property
	Public ReadOnly Property PMRefFieldCoverExpiryDate() As String
		Get
			PMRefFieldCoverExpiryDate = "CED"
		End Get
	End Property
	Public ReadOnly Property PMRefFieldBranchCode() As String
		Get
			PMRefFieldBranchCode = "BC"
		End Get
	End Property
	Public ReadOnly Property PMRefFieldProductCode() As String
		Get
			PMRefFieldProductCode = "PC"
		End Get
	End Property
	Public ReadOnly Property PMRefFieldProductAnalysisCode() As String
		Get
			PMRefFieldProductAnalysisCode = "PAC"
		End Get
	End Property
	Public ReadOnly Property PMRefFieldTransactionTypeCode() As String
		Get
			PMRefFieldTransactionTypeCode = "TTC"
		End Get
	End Property
	Public ReadOnly Property PMRefFieldTransactionBasis() As String
		Get
			PMRefFieldTransactionBasis = "TB"
		End Get
	End Property
	Public ReadOnly Property PMRefFieldSourceCode() As String
		Get
			PMRefFieldSourceCode = "SC"
		End Get
	End Property
	
	
	' ***************************************************************** '
	' Navigator
	' ***************************************************************** '
	
	' ***************************************************************** '
	' Constants used for the collection keys
	' ***************************************************************** '
	Public ReadOnly Property PMProcessKeyPrefix() As String
		Get
			PMProcessKeyPrefix = "P"
		End Get
	End Property
	Public ReadOnly Property PMMapKeyPrefix() As String
		Get
			PMMapKeyPrefix = "M"
		End Get
	End Property
	Public ReadOnly Property PMStepKeyPrefix() As String
		Get
			PMStepKeyPrefix = "S"
		End Get
	End Property
	Public ReadOnly Property PMComponentKeyPrefix() As String
		Get
			PMComponentKeyPrefix = "C"
		End Get
	End Property
	Public ReadOnly Property PMProcInstanceKeyPrefix() As String
		Get
			PMProcInstanceKeyPrefix = "PI"
		End Get
	End Property
	Public ReadOnly Property PMMapInstanceKeyPrefix() As String
		Get
			PMMapInstanceKeyPrefix = "MI"
		End Get
	End Property
	Public ReadOnly Property PMStepInstanceKeyPrefix() As String
		Get
			PMStepInstanceKeyPrefix = "SI"
		End Get
	End Property
	
	' ***************************************************************** '
	' Status settings for Process, Map and Step.
	' ***************************************************************** '
	Public ReadOnly Property PMNavStatusUnknown() As String
		Get
			PMNavStatusUnknown = ""
		End Get
	End Property
	Public ReadOnly Property PMNavStatusNotActive() As String
		Get
			PMNavStatusNotActive = "NA"
		End Get
	End Property
	Public ReadOnly Property PMNavStatusComplete() As String
		Get
			PMNavStatusComplete = "CP"
		End Get
	End Property
	Public ReadOnly Property PMNavStatusIncomplete() As String
		Get
			PMNavStatusIncomplete = "IP"
		End Get
	End Property
	
	' ***************************************************************** '
	' Incomplete Effects
	' ***************************************************************** '
	Public ReadOnly Property PMNavIncompleteNone() As String
		Get
			PMNavIncompleteNone = "NA"
		End Get
	End Property
	Public ReadOnly Property PMNavIncompleteCurrentProcess() As String
		Get
			PMNavIncompleteCurrentProcess = "CP"
		End Get
	End Property
	Public ReadOnly Property PMNavIncompleteCurrentMap() As String
		Get
			PMNavIncompleteCurrentMap = "CM"
		End Get
	End Property
	
	' ***************************************************************** '
	' Action Constants
	' ***************************************************************** '
	Public ReadOnly Property PMNavActionBackOne() As String
		Get
			PMNavActionBackOne = "B1"
		End Get
	End Property
	Public ReadOnly Property PMNavActionBackX() As String
		Get
			PMNavActionBackX = "BX"
		End Get
	End Property
	Public ReadOnly Property PMNavActionExitMap() As String
		Get
			PMNavActionExitMap = "EM"
		End Get
	End Property
	Public ReadOnly Property PMNavActionForwardOne() As String
		Get
			PMNavActionForwardOne = "F1"
		End Get
	End Property
	Public ReadOnly Property PMNavActionForwardX() As String
		Get
			PMNavActionForwardX = "FX"
		End Get
	End Property
	Public ReadOnly Property PMNavActionRepeatMap() As String
		Get
			PMNavActionRepeatMap = "RM"
		End Get
	End Property
	Public ReadOnly Property PMNavActionStartProcess() As String
		Get
			PMNavActionStartProcess = "SP"
		End Get
	End Property
	'RFC140199
	Public ReadOnly Property PMNavActionCompleteProcess() As String
		Get
			PMNavActionCompleteProcess = "CP"
		End Get
	End Property
	'RFC140199
	Public ReadOnly Property PMNavActionAbortProcess() As String
		Get
			PMNavActionAbortProcess = "AP"
		End Get
	End Property
	
	' ***************************************************************** '
	' Component Type Constants
	' ***************************************************************** '
	Public ReadOnly Property PMNavComponentDataForm() As String
		Get
			PMNavComponentDataForm = "DF"
		End Get
	End Property
	Public ReadOnly Property PMNavComponentBusinessObject() As String
		Get
			PMNavComponentBusinessObject = "BO"
		End Get
	End Property
	Public ReadOnly Property PMNavComponentFindForm() As String
		Get
			PMNavComponentFindForm = "FF"
		End Get
	End Property
	Public ReadOnly Property PMNavComponentDecisionForm() As String
		Get
			PMNavComponentDecisionForm = "QF"
		End Get
	End Property
	
	' ***************************************************************** '
	' Transaction_Type_Basis constants
	' ***************************************************************** '
	Public ReadOnly Property PMTransTypeBasisAdditional() As String
		Get
			PMTransTypeBasisAdditional = "A"
		End Get
	End Property
	Public ReadOnly Property PMTransTypeBasisRefund() As String
		Get
			PMTransTypeBasisRefund = "F"
		End Get
	End Property
	Public ReadOnly Property PMTransTypeBasisPrimary() As String
		Get
			PMTransTypeBasisPrimary = "P"
		End Get
	End Property
	Public ReadOnly Property PMTransTypeBasisReversePrimary() As String
		Get
			PMTransTypeBasisReversePrimary = "R"
		End Get
	End Property
	
	' ***************************************************************** '
	' Table names (PM Wide Lookup Tables)
	' ***************************************************************** '
	Public ReadOnly Property PMLookupLanguage() As String
		Get
			PMLookupLanguage = "language"
		End Get
	End Property
	Public ReadOnly Property PMLookupCurrency() As String
		Get
			PMLookupCurrency = "currency"
		End Get
	End Property
	Public ReadOnly Property PMLookupCountry() As String
		Get
			PMLookupCountry = "country"
		End Get
	End Property
	
	' ***************************************************************** '
	' Constants used by PMDAO (Data Access Object)
	' ***************************************************************** '
	' Database Name Constants
	' PM Data Source / DB Name
	Public ReadOnly Property PMSiriusDSN() As String
		Get
			PMSiriusDSN = "Sirius"
		End Get
	End Property
	Public ReadOnly Property PMSiriusDatabase() As String
		Get
			PMSiriusDatabase = "Sirius"
		End Get
	End Property
	
	' Orion Data Source /DB Name
	Public ReadOnly Property PMOrionDSN() As String
		Get
			PMOrionDSN = "Orion"
		End Get
	End Property
	Public ReadOnly Property PMOrionDatabase() As String
		Get
			PMOrionDatabase = "Orion"
		End Get
	End Property
	
	' Gemini Data Source /DB Name
	Public ReadOnly Property PMGeminiDSN() As String
		Get
			PMGeminiDSN = "Gemini"
		End Get
	End Property
	Public ReadOnly Property PMGeminiDatabase() As String
		Get
			PMGeminiDatabase = "Gemini"
		End Get
	End Property
	
	' BB201097 - Constants for Voyager DB
	Public ReadOnly Property PMVoyagerDSN() As String
		Get
			PMVoyagerDSN = "Voyager"
		End Get
	End Property
	Public ReadOnly Property PMVoyagerDatabase() As String
		Get
			PMVoyagerDatabase = "Voyager"
		End Get
	End Property
	
	' RFC 04/03/1998 - Constants for Mercury DB
	Public ReadOnly Property PMMercuryDSN() As String
		Get
			PMMercuryDSN = "Mercury"
		End Get
	End Property
	Public ReadOnly Property PMMercuryDatabase() As String
		Get
			PMMercuryDatabase = "Mercury"
		End Get
	End Property
	
	' RFC 04/03/1998 - Constants for Documaster DB
	Public ReadOnly Property PMDocumasterDSN() As String
		Get
			PMDocumasterDSN = "Documaster"
		End Get
	End Property
	Public ReadOnly Property PMDocumasterDatabase() As String
		Get
			PMDocumasterDatabase = "Documaster"
		End Get
	End Property
	
	' RFC 04/03/1998 - Constants for DocumasterV2 DB
	Public ReadOnly Property PMDocumasterV2DSN() As String
		Get
			PMDocumasterV2DSN = "DocumasterV2"
		End Get
	End Property
	Public ReadOnly Property PMDocumasterV2Database() As String
		Get
			PMDocumasterV2Database = "DocumasterV2"
		End Get
	End Property
	
	' RFC 04/03/1998 - Constants for DocumasterScan DB
	Public ReadOnly Property PMDocumasterScanDSN() As String
		Get
			PMDocumasterScanDSN = "DocumasterScan"
		End Get
	End Property
	Public ReadOnly Property PMDocumasterScanDatabase() As String
		Get
			PMDocumasterScanDatabase = "DocumasterScan"
		End Get
	End Property
	
	' RFC 25/03/1998 - Constants for Sirius Architecture DB
	Public ReadOnly Property PMSiriusArchitectureDSN() As String
		Get
			PMSiriusArchitectureDSN = "SiriusArchitecture"
		End Get
	End Property
	Public ReadOnly Property PMSiriusArchitectureDatabase() As String
		Get
			PMSiriusArchitectureDatabase = "SiriusArchitecture"
		End Get
	End Property
	
	' RFC 05/06/1998 - Sirius Broking DSN
	Public ReadOnly Property PMSiriusBrokingDSN() As String
		Get
			PMSiriusBrokingDSN = "SiriusBroking"
		End Get
	End Property
	Public ReadOnly Property PMSiriusBrokingDatabase() As String
		Get
			PMSiriusBrokingDatabase = "SiriusBroking"
		End Get
	End Property
	
	' RFC 19/08/1998 - Sirius Underwriting DSN
	Public ReadOnly Property PMSiriusUnderwritingDSN() As String
		Get
			PMSiriusUnderwritingDSN = "SiriusUnderwriting"
		End Get
	End Property
	Public ReadOnly Property PMSiriusUnderwritingDatabase() As String
		Get
			PMSiriusUnderwritingDatabase = "SiriusUnderwriting"
		End Get
	End Property
	
	' RFC 19/08/1998 - Sirius Solutions DSN
	Public ReadOnly Property PMSiriusSolutionsDSN() As String
		Get
			PMSiriusSolutionsDSN = "SiriusSolutions"
		End Get
	End Property
	Public ReadOnly Property PMSiriusSolutionsDatabase() As String
		Get
			PMSiriusSolutionsDatabase = "SiriusSolutions"
		End Get
	End Property
	
	' RFC 19/08/1998 - Nirvana DSN
	Public ReadOnly Property PMNirvanaDSN() As String
		Get
			PMNirvanaDSN = "Nirvana"
		End Get
	End Property
	Public ReadOnly Property PMNirvanaDatabase() As String
		Get
			PMNirvanaDatabase = "Nirvana"
		End Get
	End Property
	
	'RFC060799 - Added GeminiII Product Family, DSN etc etc
	Public ReadOnly Property PMGeminiIIDSN() As String
		Get
			PMGeminiIIDSN = "GeminiII"
		End Get
	End Property
	Public ReadOnly Property PMGeminiIIDatabase() As String
		Get
			PMGeminiIIDatabase = "GeminiII"
		End Get
	End Property
	
	' RDC 07082000 - New product family: Claims
	Public ReadOnly Property PMClaimsDSN() As String
		Get
			PMClaimsDSN = "Claims"
		End Get
	End Property
	Public ReadOnly Property PMClaimsDatabase() As String
		Get
			PMClaimsDatabase = "Claims"
		End Get
	End Property
	
	' ***************************************************************** '
	' Constants required for string manipulation
	' ***************************************************************** '
	Public ReadOnly Property PMStartDelimiter() As String
		Get
			PMStartDelimiter = "{"
		End Get
	End Property
	Public ReadOnly Property PMEndDelimiter() As String
		Get
			PMEndDelimiter = "}"
		End Get
	End Property
	
	' ***************************************************************** '
	' Database Parameter Prefix
	' Currently set to @ for SQLServer
	' ***************************************************************** '
	Public ReadOnly Property PMDBParamPrefix() As String
		Get
			PMDBParamPrefix = "@"
		End Get
	End Property
	' Length of Prefix (In Characters)
	Public ReadOnly Property PMDBParamPrefixLen() As Integer
		Get
			PMDBParamPrefixLen = 1
		End Get
	End Property
	
	' ***************************************************************** '
	' Database Hex Prefix
	' Currently Set to 0x for SQLServer
	' ***************************************************************** '
	Public ReadOnly Property PMDBHexPrefix() As String
		Get
			PMDBHexPrefix = "0x"
		End Get
	End Property
	
	' ***************************************************************** '
	' SP 23/09/1997 - Tell PMDAO not to restrict number of records returned in SQLSelect
	' ***************************************************************** '
	Public ReadOnly Property PMAllRecords() As Integer
		Get
			PMAllRecords = -1
		End Get
	End Property
	'DAK071099
	' Work Manager Constants
	Public ReadOnly Property ACTaskGroupIconDescClient() As String
		Get
			ACTaskGroupIconDescClient = "Client"
		End Get
	End Property
	
	Public ReadOnly Property ACTaskGroupIconDescPolicy() As String
		Get
			ACTaskGroupIconDescPolicy = "Policy"
		End Get
	End Property
	
	Public ReadOnly Property ACTaskGroupIconDescQuote() As String
		Get
			ACTaskGroupIconDescQuote = "Quotes"
		End Get
	End Property
	
	Public ReadOnly Property ACTaskGroupIconDescClaim() As String
		Get
			ACTaskGroupIconDescClaim = "Claims"
		End Get
	End Property
	
	Public ReadOnly Property ACTaskGroupIconDescAccount() As String
		Get
			ACTaskGroupIconDescAccount = "Accounts"
		End Get
	End Property
	
	Public ReadOnly Property ACTaskGroupIconDescReport() As String
		Get
			ACTaskGroupIconDescReport = "Reports"
		End Get
	End Property
	
	Public ReadOnly Property ACTaskGroupIconDescAgent() As String
		Get
			ACTaskGroupIconDescAgent = "Agent"
		End Get
	End Property
	
	Public ReadOnly Property ACTaskGroupIconDescAdmin() As String
		Get
			ACTaskGroupIconDescAdmin = "Administration"
		End Get
	End Property
	
	Public ReadOnly Property ACTaskGroupIconDescRenewals() As String
		Get
			ACTaskGroupIconDescRenewals = "Renewals"
		End Get
	End Property
	
	Public ReadOnly Property ACTaskGroupIconDescStatistics() As String
		Get
			ACTaskGroupIconDescStatistics = "Statistics"
		End Get
	End Property
	
	Public ReadOnly Property ACTaskGroupIconDescGeneral() As String
		Get
			ACTaskGroupIconDescGeneral = "Other Tasks"
		End Get
	End Property
	
	Public ReadOnly Property ACTaskCategoryNonLicence() As String
		Get
			ACTaskCategoryNonLicence = "NONLICENCE"
		End Get
	End Property
	
	' #########################################################################################################
	' #########################################################################################################
	' #########################################################################################################
	' #########################################################################################################
	' #########################################################################################################
	' #########################################################################################################
	' #########################################################################################################
	' #########################################################################################################
	
	' *******************************************************
	' Transaction Types
	' RFC020398
	' *******************************************************
	' Generic
	Public ReadOnly Property PMTransactionTypeGeneric() As String
		Get
			PMTransactionTypeGeneric = ""
		End Get
	End Property
	
	'DAK280999
	'New Business
	Public ReadOnly Property PMTransactionTypeNB() As String
		Get
			PMTransactionTypeNB = "G_NB"
		End Get
	End Property
	
	'MTA
	Public ReadOnly Property PMTransactionTypeMTA() As String
		Get
			PMTransactionTypeMTA = "G_MTA"
		End Get
	End Property
	
	'Renewals
	Public ReadOnly Property PMTransactionTypeRenewals() As String
		Get
			PMTransactionTypeRenewals = "G_RENEW"
		End Get
	End Property
	
	' *******************************************************
	' Unix Link Timeout Settings
	' RFC070498
	' RFC131098
	' *******************************************************
	Public ReadOnly Property PMUnixLinkSendTimeout() As Integer
		Get
			
			Dim sRegSendTimeout As String
			' RDC 12062002 classes changed to BAS modules
			Dim lReturn As Integer
			
			On Error GoTo Err_PMUnixLinkSendTimeout
			
			' Get the Unix Link Send Timoeut setting from
			' HKEY_LOCAL_MACHINE\software\PM\SiriusArchitecture\Server\UnixLink
			' RDC 12062002 classes changed to BAS modules
			'    lReturn& = oFunc.GetPMRegSetting( _
			'v_lPMERegSettingRoot:=pmeRSRLocalMachine, _
			'v_lPMEProductFamily:=pmePFSiriusArchitecture, _
			'v_lPMERegSettingLevel:=pmeRSLServer, _
			'v_sSettingName:="SendTimeout", _
			'r_sSettingValue:=sRegSendTimeout, _
			'v_sSubKey:="UnixLink")
			lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=PMERegSettingLevel.pmeRSLServer, v_sSettingName:="SendTimeout", r_sSettingValue:=sRegSendTimeout, v_sSubKey:="UnixLink")
			
			' Use setting from Registry OR Default if it doesnt exist/invalid.
			If (IsNumeric(sRegSendTimeout) = True) Then
				PMUnixLinkSendTimeout = CInt(sRegSendTimeout)
			Else
				PMUnixLinkSendTimeout = 60
			End If
			
			Exit Property
			
Err_PMUnixLinkSendTimeout: 
			
			PMUnixLinkSendTimeout = 60
			
		End Get
	End Property
	
	Public ReadOnly Property PMUnixLinkReadTimeout() As Integer
		Get
			
			Dim sRegReadTimeout As String
			' RDC 12062002 classes changed to BAS modules
			Dim lReturn As Integer
			
			On Error GoTo Err_PMUnixLinkReadTimeout
			
			' Get the Unix Link Read Timoeut setting from
			' HKEY_LOCAL_MACHINE\software\PM\SiriusArchitecture\Server\UnixLink
			' RDC 12062002
			'    lReturn& = oFunc.GetPMRegSetting( _
			'v_lPMERegSettingRoot:=pmeRSRLocalMachine, _
			'v_lPMEProductFamily:=pmePFSiriusArchitecture, _
			'v_lPMERegSettingLevel:=pmeRSLServer, _
			'v_sSettingName:="ReadTimeout", _
			'r_sSettingValue:=sRegReadTimeout, _
			'v_sSubKey:="UnixLink")
			lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=PMERegSettingLevel.pmeRSLServer, v_sSettingName:="ReadTimeout", r_sSettingValue:=sRegReadTimeout, v_sSubKey:="UnixLink")
			
			' Use setting from Registry OR Default if it doesnt exist/invalid.
			If (IsNumeric(sRegReadTimeout) = True) Then
				PMUnixLinkReadTimeout = CInt(sRegReadTimeout)
			Else
				PMUnixLinkReadTimeout = 60
			End If
			
			Exit Property
			
Err_PMUnixLinkReadTimeout: 
			
			PMUnixLinkReadTimeout = 60
			
		End Get
	End Property
	
	Public ReadOnly Property PMUnixLinkConnectTimeout() As Integer
		Get
			
			Dim sRegConnectTimeout As String
			' RDC 12062002 classes changed to BAS modules
			Dim lReturn As Integer
			
			On Error GoTo Err_PMUnixLinkConnectTimeout
			
			' Get the Unix Link Connect Timoeut setting from
			' HKEY_LOCAL_MACHINE\software\PM\SiriusArchitecture\Server\UnixLink
			' RDC 12062002 classes changed to BAS modules
			'    lReturn& = oFunc.GetPMRegSetting( _
			'v_lPMERegSettingRoot:=pmeRSRLocalMachine, _
			'v_lPMEProductFamily:=pmePFSiriusArchitecture, _
			'v_lPMERegSettingLevel:=pmeRSLServer, _
			'v_sSettingName:="ConnectTimeout", _
			'r_sSettingValue:=sRegConnectTimeout, _
			'v_sSubKey:="UnixLink")
			
			lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=PMERegSettingLevel.pmeRSLServer, v_sSettingName:="ConnectTimeout", r_sSettingValue:=sRegConnectTimeout, v_sSubKey:="UnixLink")
			
			' Use setting from Registry OR Default if it doesnt exist/invalid.
			If (IsNumeric(sRegConnectTimeout) = True) Then
				PMUnixLinkConnectTimeout = CInt(sRegConnectTimeout)
			Else
				PMUnixLinkConnectTimeout = 20
			End If
			
			Exit Property
			
Err_PMUnixLinkConnectTimeout: 
			
			PMUnixLinkConnectTimeout = 20
			
		End Get
	End Property
	
	' *******************************************************
	' Object Manager GetInstance options.
	' RFC120698
	' *******************************************************
	' Get instance via Client Manager i.e. Server Side
	Public ReadOnly Property PMGetViaClientManager() As String
		Get
			PMGetViaClientManager = "CLIENTMANAGER"
		End Get
	End Property
	' Get a local business object
	Public ReadOnly Property PMGetLocalBusiness() As String
		Get
			PMGetLocalBusiness = "LOCALBUSINESS"
		End Get
	End Property
	' Get a local interface object
	Public ReadOnly Property PMGetLocalInterface() As String
		Get
			PMGetLocalInterface = "LOCALINTERFACE"
		End Get
	End Property
	
	'RFC 16/10/1998
	Public ReadOnly Property PMProductCode(ByVal v_lPMProductFamily As Integer) As String
		Get
			
			On Error GoTo Err_PMProductCode
			
			'RFC180299 Changed to use Constants
			Select Case v_lPMProductFamily
				Case PMEProductFamily.pmePFSiriusArchitecture
					PMProductCode = ACSiriusArchitecture
				Case PMEProductFamily.pmePFSiriusUnderwriting
					PMProductCode = ACSiriusUnderwriting
				Case PMEProductFamily.pmePFOrion
					PMProductCode = ACOrion
				Case PMEProductFamily.pmePFGemini
					PMProductCode = ACGemini
				Case PMEProductFamily.pmePFVoyager
					PMProductCode = ACVoyager
				Case PMEProductFamily.pmePFMercury
					PMProductCode = ACMercury
				Case PMEProductFamily.pmePFDocumaster
					PMProductCode = ACDocumaster
				Case PMEProductFamily.pmePFSiriusBroking
					PMProductCode = ACSiriusBroking
				Case PMEProductFamily.pmePFSiriusSolutions
					PMProductCode = ACSiriusSolutions
				Case PMEProductFamily.pmePFNirvana
					PMProductCode = ACNirvana
					'RFC060799 - Added GeminiII Product Family, DSN etc etc
				Case PMEProductFamily.pmePFGeminiII
					PMProductCode = ACGeminiII
				Case Else
					PMProductCode = ""
			End Select
			
			Exit Property
			
Err_PMProductCode: 
			
			PMProductCode = ""
			
		End Get
	End Property
	
	'RFC180299 New Constants Added for SA1.4
	Public ReadOnly Property PMProductFamilyByCode(ByVal v_sPMProductCode As String) As Integer
		Get
			
			On Error GoTo Err_PMProductFamilyByCode
			
			Select Case v_sPMProductCode
				Case ACSiriusArchitecture
					PMProductFamilyByCode = PMEProductFamily.pmePFSiriusArchitecture
				Case ACSiriusUnderwriting
					PMProductFamilyByCode = PMEProductFamily.pmePFSiriusUnderwriting
				Case ACOrion
					PMProductFamilyByCode = PMEProductFamily.pmePFOrion
				Case ACGemini
					PMProductFamilyByCode = PMEProductFamily.pmePFGemini
				Case ACVoyager
					PMProductFamilyByCode = PMEProductFamily.pmePFVoyager
				Case ACMercury
					PMProductFamilyByCode = PMEProductFamily.pmePFMercury
				Case ACDocumaster
					PMProductFamilyByCode = PMEProductFamily.pmePFDocumaster
				Case ACSiriusBroking
					PMProductFamilyByCode = PMEProductFamily.pmePFSiriusBroking
				Case ACSiriusSolutions
					PMProductFamilyByCode = PMEProductFamily.pmePFSiriusSolutions
				Case ACNirvana
					PMProductFamilyByCode = PMEProductFamily.pmePFNirvana
					'RFC060799 - Added GeminiII Product Family, DSN etc etc
				Case ACGeminiII
					PMProductFamilyByCode = PMEProductFamily.pmePFGeminiII
					' RDC 07082000 - new product family: Claims
				Case ACClaims
					PMProductFamilyByCode = PMEProductFamily.pmePFClaims
				Case Else
					PMProductFamilyByCode = 0
			End Select
			
			Exit Property
			
Err_PMProductFamilyByCode: 
			
			PMProductFamilyByCode = 0
			
		End Get
	End Property
End Module