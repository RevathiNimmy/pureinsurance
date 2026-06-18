Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Globalization
Imports System.Windows.Forms
Imports SharedFiles
Imports System.Collections.Generic
Imports System.Text.RegularExpressions
Imports System.Runtime.ExceptionServices

<System.Runtime.InteropServices.ProgId("RiskScreen_NET.RiskScreen")>
Partial Public Class RiskScreen
    Inherits System.Windows.Forms.UserControl
    Implements IDisposable
    Public Event FromEventChange()
    Public Event BorderStyleChange()
    Public Event BackStyleChange()
    Public Event FontChange()
    Public Event EnabledChange()
    Public Event ForeColorChange()
    Public Event BackColorChange()
    Public Event EffectiveDateChange()
    Public Event TransactionTypeChange()
    Public Event ProcessModeChange()
    Public Event NavigateChange()
    Public Event TaskChange()
    Public Event StatusChange()
    Public Event CallingAppNameChange()

    ' These constants should have binary values (eg 1,2,4,8,16 etc )
#Const m_klDebugOption_Normal = 1 ' RAW 21/05/2004 : Performance Changes : added
#Const m_klDebugOption_Events = 2 ' RAW 21/05/2004 : Performance Changes : added
#Const m_klDebugOption_Script = 4 ' RAW 21/05/2004 : Performance Changes : added
#Const m_klDebugOption_DLCallBack = 8 ' RAW 09/07/2004 : JIT : added
#Const m_klDebugOption_JIT = 16 ' RAW 09/07/2004 : JIT : added

    ' Developer Guide No. 7
    Private Const vbFormCode As Integer = 0

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "uctRiskScreenControl"
    Private Const PMFormatStringMultiLine As Integer = 28
    Private m_lFindControlIndex As Integer
    Private m_vFindControlArray(,) As Object
    Private m_bDefaultClausesForRisk As Boolean

    'Default Property Values:
    Const m_def_BackColor As Integer = 0
    Const m_def_ForeColor As Integer = 0
    Const m_def_Enabled As Integer = 0
    Const m_def_BackStyle As Integer = 0
    Const m_def_BorderStyle As Integer = 0
    Const m_def_PartyCnt As Integer = 0

    'Property Variables:
    Dim m_BackColor As Integer
    Dim m_ForeColor As Integer
    Dim m_Enabled As Boolean
    Dim m_Font As Font
    Dim m_BackStyle As Integer
    Dim m_BorderStyle As BorderStyle
    Dim m_PartyCnt As Integer
    Private m_bUserMode As Boolean
    Dim m_vDefaultClausesForRisk As Object

    'non database controls
    Private Const nonDatabaseElements As String = "Non Database Elements"
    Private Const freeFormatText As String = "Free Format Text"
    Private Const hyperlink As String = "Hyperlink"

    'mandatory background color
    Private m_lMandatoryColor As Integer

    'Event Declarations:
    Shadows Event Click(ByVal Sender As Object, ByVal e As EventArgs)
    Event DblClick(ByVal Sender As Object, ByVal e As EventArgs)
    Shadows Event KeyDown(ByVal Sender As Object, ByVal e As KeyDownEventArgs)
    Shadows Event KeyPress(ByVal Sender As Object, ByVal e As KeyPressEventArgs)
    Shadows Event KeyUp(ByVal Sender As Object, ByVal e As KeyUpEventArgs)
    Shadows Event MouseDown(ByVal Sender As Object, ByVal e As MouseDownEventArgs)
    Shadows Event MouseMove(ByVal Sender As Object, ByVal e As MouseMoveEventArgs)
    Shadows Event MouseUp(ByVal Sender As Object, ByVal e As MouseUpEventArgs)

    ' AMB 10/03/2003: IS2851 - raise the PerilListChangedEvent up to the parent container
    Public Event PerilListChanged(ByVal Sender As Object, ByVal e As EventArgs)

    Public Event ClaimPaymentUnrecoverableError(ByVal Sender As Object, ByVal e As EventArgs)

    '*************
    ' MEvans : 05-09-2003 : CQ2455
    Public Event PerilEdit(ByVal Sender As Object, ByVal e As EventArgs)
    '*************
    Public Event DebugModeChanged(ByVal Sender As Object, ByVal e As DebugModeChangedEventArgs)

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    Private m_oDLEngine As DLEngine ' RAW 09/07/2004 : JIT : added


    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_iOriginalTask As Integer 'a copy of the original task value passed to dynamic logic

    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_iPartySourceId As Integer
    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the Interface object.
    Private m_oInterface As Object
    'Private m_oInterface As iPMUScreenControl.Interface

    Private m_oSIRRiskScreen As bSIRRiskScreen.Business 'PN24176

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Variables to store the lookup values/details.
    Private m_vLookupValues As Object
    Private m_vLookupDetails As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control
    Private m_lTabsetIndexlist(,) As String
    'If a control has been added ResetTabIndex should be called
    Private m_bResetTabIndexRequired As Boolean

    Private bCommandFromChildForm As Boolean
    ' Stores the details from the business object.

    ' RAW 21/05/2004 : Performance Changes : added
    ' This needs to be public because it is passed as a parameter to a public method
    Public Enum enumDLProcedureType
        eDLProcedureName_Start = 0
        eDLProcedureName_OnFocus = 1
        eDLProcedureName_OnChange = 2
        eDLProcedureName_OnLostFocus = 3 ' RAW 22/06/2004 : Performance Changes(#2) : added
        eDLProcedureName_Initialise = 4 ' RAW 09/07/2004 : JIT : added
    End Enum

    Public Enum enumOnFocusAction
        eOnFocusAction_Combo = 0
        eOnFocusAction_Text = 1
        eOnFocusAction_List = 2
        eOnFocusAction_YesNo = 3
        eOnFocusAction_FurtherDetails = 4
        eOnFocusAction_Add = 5
        eOnFocusAction_Edit = 6
        eOnFocusAction_Delete = 7
        eOnFocusAction_Tab = 8 ' RAW 21/05/2004 : Performance Changes : added
    End Enum

    ' RAW 21/05/2004 : Performance Changes : added
    ' RAW 22/06/2004 : Performance Changes(#2) : made private - was added as public in error
    Public Enum enumOnChangeAction
        eOnChangeAction_Change = 0 ' RAW 22/06/2004 : Performance Changes(#2) : renamed
        eOnChangeAction_Refresh = 2 ' RAW 22/06/2004 : Performance Changes(#2) : renamed
        eOnChangeAction_Accept = 3 ' RAW 22/06/2004 : Performance Changes(#2) : renamed
    End Enum

    Private m_vScreenHeader(,) As Object
    Private m_vScreenDetails(,) As Object
    Private m_vChildScreenDetails(,) As Object

    'Data retrieved via Get Details from Party & RiskScreen

    Private m_lPartyCnt As Integer
    Private m_sShortName As String = ""
    Private m_lInsuranceFolderCnt As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_lRiskId As Integer
    Private m_lOriginalRiskId As Integer ' RAW 03/09/2004 : Resilience (#2) : added
    Private m_lRiskTypeId As Integer
    Private m_lProductId As Integer
    Private m_lScreenId As Integer
    Private m_sScreenDesc As String = ""
    Private m_sGISDataModel As String = ""

    Private m_sCaption As String = ""
    Private m_sOKCaption As String = ""
    Private m_sHelpCaption As String = ""
    Private m_sCancelCaption As String = ""

    Private m_lTextIndex As Integer 'standard text box or named label/hyperlink
    Private m_lMlTextIndex As Integer 'multiline text box
    Private m_lFormattedTextIndex As Integer 'Formatted text box
    Private m_lCheckIndex As Integer
    Private m_lComboIndex As Integer
    Private m_lGeminiComboIndex As Integer
    Private m_lGISComboIndex As Integer
    Private m_lPMLookupIndex As Integer
    Private m_lFrameIndex As Integer
    Private m_lPanelIndex As Integer
    Private m_lPartyCommandIndex As Integer
    Private m_lPolicyCommandIndex As Integer
    Private m_lListViewIndex As Integer
    Private m_lStandardWordingIndex As Integer
    Private m_lSumInsuredIndex As Integer
    Private m_lAddressIndex As Integer
    Private m_lTabIndex As Integer
    Private m_bLossSchedule As Boolean
    Private m_lLossScheduleTypeId As Integer

    Private m_vTabArray As Object
    Private m_vFrameArray As Object
    Private m_vGeminiComboArray As Object
    Private m_vGISComboArray As Object
    Private m_vPMLookupArray As Object
    Private m_vTextArray As Object 'standard text box
    Private m_vMlTextArray As Object 'multiline text box
    Private m_vFormattedTextArray As Object 'Formatted text box
    Private m_vCheckArray As Object
    Private m_vPartyCommandArray As Object
    Private m_vPolicyCommandArray As Object
    Private m_vListViewArray As Object
    Private m_vStandardWordingArray As Object
    Private m_vSumInsuredArray As Object
    Private m_vAddressArray As Object
    Private m_vAccumulationArray As Object

    Private m_sHelpText As String = ""
    Private m_lPreQuote As Integer
    Private m_lPostQuote As Integer
    Private m_lPurchase As Integer
    Private m_lIsValuation As Integer
    Private m_lIsRateAndPremium As Integer
    Private m_lIncludeInList As Integer


    Private m_lHeight As Integer
    Private m_lWidth As Integer

    Private m_lStage As Integer

    Private m_bEvent As Boolean

    Private m_vRiskDetails(,) As Object
    Private m_vRiskTypeDetails(,) As Object 'read by top level screen and passed into child screens

    Private m_iIsRiAtRiskLevel As Integer
    Private m_iIsAutoReinsured As Integer

    'Private m_lControlCount As Long         ' RAW 09/07/2004 : JIT : removed

    Private m_oGIS As Object
    Private m_oGISOriginal As Object

    Private m_bSubScreen As Boolean
    Private m_sParentOIKey As String = ""
    Private m_sChildOIKey As String = ""
    Private m_sParentObjectName As String = ""
    Private m_sChildObjectName As String = ""
    Private m_sGISObjectName As String = ""
    Private m_bIsWhatIfQ As Boolean 'PN19313
    Private m_bRenewalConfirmationMode As Boolean 'PN19313
    Private Const ACPMTransactionTypeReview As String = "G_REVIEW" 'PN19313

    Private m_sReferReasons As String = ""
    Private m_sDeclineReasons As String = ""
    Private m_sMessages As String = ""
    Private m_sQuoteType As String = ""

    Private m_bListChanged As Boolean
    Private m_sListText As String = ""

    'tab sets indexes for non array controls
    Private m_uctCLMReserveTabIndex As Object
    Private m_uctCLMPaymentTabIndex As Object
    Private m_uctClaimPerilTabIndex As Object
    Private m_uctAssociatedClientsTabIndex As Object
    Private m_uctAssociatedClientTabIndex As Object
    Private m_uctCLMCaseClaimTabIndex As Object
    Private m_uctCLMCaseHeaderTabIndex As Object


    'validation rules
    Private WithEvents m_oMSScriptControl As New MSScriptControl.ScriptControl ' RAW 21/05/2004 : Performance Changes : added WithEvents

    Private m_cObjectAndAttribute As Collection

    Private m_cMandatoryProperties As Collection

    'performance flags
    Private m_bIsLoading As Boolean

    'Claims Builder
    Private m_lClaimID As Integer
    Private m_lClaimPerilID As Integer
    Private m_lPerilID As Integer
    Private m_lClaimWorkID As Integer
    Private m_lClaimWorkPerilID As Integer
    Private m_sClaimTransactionType As String = ""
    Private m_lClaimInsFileCnt As Integer
    Private m_lClaimRiskId As Integer
    Private m_lPerilTypeId As Integer

    Private m_lCaseID As Integer
    Private m_lBaseCaseID As Integer
    Private m_sCaseNumber As String

    'PR 10102002 - Add product option
    Private m_bRemoveNoneOption As Boolean
    'PR 10102002 - end

    'Declare an instance of the policy wording interface.
    Private m_oPolicyWording As Object
    Private m_vStandardWording(,) As Object
    Private m_lChildIndex As Integer
    Private m_lObjectType As Integer 'Risk , specials, associated client etc

    ' RAW 22/07/2003 : CQ1783 : copied code from 1.8.6
    'DP 30/05/2003 - for passing parent data to child screens
    Private m_aDataToChild As Object
    Private m_aChildDataFromParent As Object
    'DP end
    ' RAW 22/07/2003 : CQ1783 : end

    Private m_bDLFunction_Initialise_valid As Boolean ' RAW 09/07/2004 : JIT : added
    Private m_bDLFunction_onChange_valid As Boolean
    Private m_bDLFunction_onFocus_valid As Boolean
    Private m_bDLFunction_onLostFocus_valid As Boolean ' RAW 22/06/2004 : Performance Changes(#2) : added
    Private m_bDLFunction_Start_valid As Boolean ' RAW 21/05/2004 : Performance Changes : added

    ' initially these variables will default to True and must be set to False by user dynamic logic to activate the new functionality.
    ' At a later date this will default to False once all user logic has been modified accordingly
    ' RAW 22/06/2004 : Performance Changes(#2) : renamed
    Private m_bFireDLStartEvents As Boolean ' RAW 21/05/2004 : Performance Changes : added

    Private m_bScriptTimedOut As Boolean ' RAW 21/05/2004 : Performance Changes : added
    Private m_bScriptErrorDetected As Boolean ' RAW 22/06/2004 : Performance Changes(#2) : added

    Private m_bSuppressDynamicLogic As Boolean ' RAW 22/06/2004 : Performance Changes(#2) : added

    Private m_oNewControlBeingLoaded As Object ' RAW 20/08/2004 : JIT : added

    Private m_bCancelSilently As Boolean

    ' RAW 22/06/2004 : Performance Changes(#2) : added
    Private Const m_klSumInsuredArrayColNo_Description As Integer = 0
    Private Const m_klSumInsuredArrayColNo_Reference As Integer = 1
    Private Const m_klSumInsuredArrayColNo_SumInsured As Integer = 2
    Private Const m_klSumInsuredArrayColNo_DateAdded As Integer = 3
    Private Const m_klSumInsuredArrayColNo_DateDeleted As Integer = 4
    Private Const m_klSumInsuredArrayColNo_ValuationReq As Integer = 5
    Private Const m_klSumInsuredArrayColNo_ValuationDate As Integer = 6
    Private Const m_klSumInsuredArrayColNo_Rate As Integer = 7
    Private Const m_klSumInsuredArrayColNo_Premium As Integer = 8

    Private Const m_klSumInsuredListColNo_Description As Integer = 0
    Private Const m_klSumInsuredListColNo_Reference As Integer = 1
    Private Const m_klSumInsuredListColNo_SumInsured As Integer = 2
    Private Const m_klSumInsuredListColNo_DateAdded As Integer = 3
    Private Const m_klSumInsuredListColNo_DateDeleted As Integer = 4
    Private Const m_klSumInsuredListColNo_ValuationReq As Integer = 5
    Private Const m_klSumInsuredListColNo_ValuationDate As Integer = 6
    Private Const m_klSumInsuredDataType_Description As Integer = iGISSharedConstants.GISDataTypeText
    Private Const m_klSumInsuredDataType_Reference As Integer = iGISSharedConstants.GISDataTypeText
    Private Const m_klSumInsuredDataType_SumInsured As Integer = iGISSharedConstants.GISDataTypeCurrency
    Private Const m_klSumInsuredDataType_DateAdded As Integer = iGISSharedConstants.GISDataTypeDate
    Private Const m_klSumInsuredDataType_DateDeleted As Integer = iGISSharedConstants.GISDataTypeDate
    Private Const m_klSumInsuredDataType_ValuationReq As Integer = iGISSharedConstants.GISDataTypeOption
    Private Const m_klSumInsuredDataType_ValuationDate As Integer = iGISSharedConstants.GISDataTypeDate
    Private Const m_klSumInsuredDataType_Rate As Integer = iGISSharedConstants.GISDataTypeCurrency
    Private Const m_klSumInsuredDataType_Premium As Integer = iGISSharedConstants.GISDataTypeCurrency
    ' RAW 22/06/2004 : Performance Changes(#2) : end

    ' RAW 09/07/2004 : JIT : added
    Private Enum enumControlType
        eControlType_Unknown = 0
        eControlType_Tab = 1
        eControlType_Frame = 2
        eControlType_ListView = 3
        eControlType_TextBox = 4
        eControlType_CheckBox = 5
        eControlType_GeminiCombo = 6
        eControlType_PMLookup = 7
        eControlType_GISLookup = 8
        eControlType_Accumulation = 9
        eControlType_Address = 10
        eControlType_Party = 11
        eControlType_Policy = 12
        eControlType_SumInsured = 13
        eControlType_StandardWording = 14
        eControlType_Label = 15
        eControlType_HyperLink = 16
        eControlType_ClaimPeril = 17
        eControlType_ClaimPayment = 18
        eControlType_ClaimReserve = 19
        eControlType_Find = 20
        eControlType_CaseHeader = 21
        eControlType_CaseClaimList = 22
    End Enum

    'RVH 09/12/2004 - 1.9 Merge
    'DP 30/05/2003 for Stargate 1.2
    Private m_cList As Collection
    Private m_aArray() As Object
    'DP end

    ' RAW 09/07/2004 : JIT : added
    ' If focus could not be set to a control (because its container was not visible at the time ?) then store its GIS name here and try again later
    Private m_sSetFocusControlName As String = ""

    'RVH 09/12/2004 - 1.9 Merge
    Private m_vXMLDataSet As Object

    Private m_oPBRiskPolicyCurrency As Object

    Private bAllowSaveOfInvalidMandatoryData As Boolean

    Private m_bCopyRisk As Boolean
    Private m_lGISPolicyLinkID As Integer

    Private m_bSuppressDropListClicks As Boolean

    Private m_vUnderwritingOrAgency As Object
    Private m_sDocServer As String = ""

    Private m_vKeyArray(,) As Object 'PN24176
    Private m_bClaimPaymentAdded As Boolean

    Private g_oObjectManager As bObjectManager.ObjectManager

    Dim m_bValueEdited As Boolean
    Dim m_iValueEditedForIndex As Integer 'PN 29235

    Private m_bControlIsUnloading As Boolean
    Private m_bNoTransactions As Boolean

    ' CJB Moved the following 2 vars from PBRiskScreenCommon module to prevent COM errors in use of child screens PN24820
    Private m_vDataDictionary(GISDMTypeLast) As Object
    Private m_vScreenValues() As Object

    Private m_lParentHwnd As Integer
    Private m_vWindowKeys As Object

    Private m_bCaseHeader As Boolean
    Private m_sPropertyName As String
    Private m_sColumnName As String

    Private m_bDLDebuggingEnabled As Boolean
    Private m_bDLDebuggingAllowed As Boolean
    Private m_lGisLookupIndexArray As Dictionary(Of Integer, Integer)
    Private m_iLastTabIndexForTabStripClicked As Integer = -1
    Private m_bIsSilentQuote As Boolean = False

    Public Property IsSilentQuote() As Boolean
        Get
            Return m_bIsSilentQuote
        End Get
        Set(ByVal value As Boolean)
            m_bIsSilentQuote = value
        End Set
    End Property
    Private Function KeepWindowOnTop(ByVal bKeepOnTop As Boolean) As Integer

        Try

            If Not SubScreen Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            m_lReturn = SetWindowPlacement(m_lParentHwnd, bKeepOnTop)
            If m_lReturn = 0 Then Throw New Exception



        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "KeepWindowOnTop function failed", ACApp, ACClass, "KeepWindowOnTop", Information.Err().Number, Information.Err().Description, excep:=ex)

        Finally
        End Try
        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)

    'RVH 09/12/2004 - 1.9 Merge
    ' PUBLIC Property Procedures (Begin)
    <Browsable(False)>
    Public ReadOnly Property XMLDataSet() As Object
        Get
            Return m_vXMLDataSet
        End Get
    End Property

    'DP 30/05/2003 - for Stargate v1.2 MultiInsurer support
    <Browsable(False)>
    Public WriteOnly Property List() As Collection
        Set(ByVal Value As Collection)
            m_cList = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property vArray() As Object()
        Set(ByVal Value() As Object)
            m_aArray = Value
        End Set
    End Property
    'DP end

    ' RAW 22/07/2003 : CQ1783 : copied code from 1.8.6
    'DP 30/05/2003 - added for passing data from parent to child
    <Browsable(False)>
    Public WriteOnly Property ChildDataFromParent() As Object
        Set(ByVal Value As Object)
            m_aChildDataFromParent = Value
        End Set
    End Property
    'DP end
    ' RAW 22/07/2003 : CQ1783 : end

    '****************
    ' MEvans : 31-07-2003
    <Browsable(False)>
    Public ReadOnly Property DataModelCode() As String
        Get
            Return m_sGISDataModel
        End Get
    End Property
    '****************

    <Browsable(True)>
    Public Property LossSchedule() As Boolean
        Get
            Return m_bLossSchedule
        End Get
        Set(ByVal Value As Boolean)
            m_bLossSchedule = Value
        End Set
    End Property
    <Browsable(True)>
    Public Property LossScheduleTypeId() As Integer
        Get
            Return m_lLossScheduleTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lLossScheduleTypeId = Value
        End Set
    End Property



    <Browsable(True)>
    Public Property ChildIndex() As Integer
        Get
            Return m_lChildIndex
        End Get
        Set(ByVal Value As Integer)
            m_lChildIndex = Value
        End Set
    End Property

    <Browsable(False)>
    Public ReadOnly Property Interface_Renamed() As Object
        Get
            Return m_oInterface
        End Get
    End Property

    <Browsable(False)>
    Public WriteOnly Property RiskTypeDetails() As Object
        Set(ByVal Value As Object)
            m_vRiskTypeDetails = Value
        End Set
    End Property
    <Browsable(True)>
    Public Property ObjectType() As Integer
        Get
            Return m_lObjectType
        End Get
        Set(ByVal Value As Integer)
            m_lObjectType = Value
        End Set
    End Property

    <Browsable(False)>
    Public ReadOnly Property ChildAddStatus() As Boolean
        Get
            ' Return the child add status, true if child was added

            Return m_oInterface.ChildAddStatus
        End Get
    End Property
    'claims builder
    <Browsable(True)>
    Public Property ClaimID() As Integer
        Get
            Return m_lClaimID
        End Get
        Set(ByVal Value As Integer)
            m_lClaimID = Value
        End Set
    End Property
    <Browsable(True)>
    Public Property ClaimPerilID() As Integer
        Get
            Return m_lClaimPerilID
        End Get
        Set(ByVal Value As Integer)
            m_lClaimPerilID = Value
        End Set
    End Property
    <Browsable(True)>
    Public Property PerilID() As Integer
        Get
            Return m_lPerilID
        End Get
        Set(ByVal Value As Integer)
            m_lPerilID = Value
        End Set
    End Property
    <Browsable(True)>
    Public Property WorkClaimID() As Integer
        Get
            Return m_lClaimWorkID
        End Get
        Set(ByVal Value As Integer)
            m_lClaimWorkID = Value
        End Set
    End Property
    <Browsable(True)>
    Public Property WorkClaimPerilID() As Integer
        Get
            Return m_lClaimWorkPerilID
        End Get
        Set(ByVal Value As Integer)
            m_lClaimWorkPerilID = Value
        End Set
    End Property
    <Browsable(True)>
    Public Property ClaimTransactionType() As String
        Get
            Return m_sClaimTransactionType
        End Get
        Set(ByVal Value As String)
            m_sClaimTransactionType = Value
        End Set
    End Property
    <Browsable(True)>
    Public Property ClaimInsFileCnt() As Integer
        Get
            Return m_lClaimInsFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lClaimInsFileCnt = Value
        End Set
    End Property
    <Browsable(True)>
    Public Property ClaimRiskId() As Integer
        Get
            Return m_lClaimRiskId
        End Get
        Set(ByVal Value As Integer)
            m_lClaimRiskId = Value
        End Set
    End Property
    <Browsable(True)>
    Public Property PerilTypeId() As Integer
        Get
            Return m_lPerilTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lPerilTypeId = Value
        End Set
    End Property


    <Browsable(False)>
    Public ReadOnly Property Controls_Renamed() As Object
        Get
            Return Me.Controls
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    <Browsable(False)>
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value
            RaiseEvent CallingAppNameChange()

        End Set
    End Property




    <Browsable(True)>
    Public Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the interface exit status.
            m_lStatus = Value
            RaiseEvent StatusChange()

        End Set
    End Property


    <Browsable(True)>
    Public Property Task() As Integer
        Get

            ' Return the objects task.
            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the objects task.

            'take a copy of the original task value so it can be passed to dynamic logic
            m_iOriginalTask = Value

            'internally, make delete look like an edit
            If Value = gPMConstants.PMEComponentAction.PMDelete Then '
                Value = gPMConstants.PMEComponentAction.PMEdit
            End If

            m_iTask = Value
            RaiseEvent TaskChange()

        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_lNavigate = Value
            RaiseEvent NavigateChange()

        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the process mode.
            m_lProcessMode = Value
            RaiseEvent ProcessModeChange()

        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the type of business.
            m_sTransactionType = Value
            RaiseEvent TransactionTypeChange()

        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            ' Standard Property.

            ' Set the effective date.
            m_dtEffectiveDate = Value
            RaiseEvent EffectiveDateChange()

        End Set
    End Property

    ' {* USER DEFINED CODE (Begin) *}
    <Browsable(True)>
    Public Property PartySourceID() As Integer
        Get
            Return m_iPartySourceId
        End Get
        Set(ByVal Value As Integer)
            m_iPartySourceId = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property ShortName() As String
        Get
            Return m_sShortName
        End Get
        Set(ByVal Value As String)
            m_sShortName = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property InsuranceFolderCnt() As Integer
        Get
            Return m_lInsuranceFolderCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFolderCnt = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property RiskId() As Integer
        Get
            Return m_lRiskId
        End Get
        Set(ByVal Value As Integer)
            m_lRiskId = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property RiskTypeId() As Integer
        Get
            Return m_lRiskTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lRiskTypeId = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property ProductId() As Integer
        Get
            Return m_lProductId
        End Get
        Set(ByVal Value As Integer)
            m_lProductId = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property ScreenId() As Integer
        Get
            Return m_lScreenId
        End Get
        Set(ByVal Value As Integer)
            m_lScreenId = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property ScreenDesc() As String
        Get
            Return m_sScreenDesc
        End Get
        Set(ByVal Value As String)
            m_sScreenDesc = Value
            g_sScreenDesc = m_sScreenDesc ' RAW 09/07/2004 : JIT : added
        End Set
    End Property

    <Browsable(True)>
    Public Property Stage() As Integer
        Get
            Return m_lStage
        End Get
        Set(ByVal Value As Integer)
            m_lStage = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property GIS() As Object
        Set(ByVal Value As Object)
            m_oGIS = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property GISOriginal() As Object
        Set(ByVal Value As Object)
            ' AMB 10/01/03 - Start - IAG 217 Spec
            m_oGISOriginal = Value
            ' AMB 10/01/03 - End - IAG 217 Spec
        End Set
    End Property

    <Browsable(True)>
    Public Property SubScreen() As Boolean
        Get
            Return m_bSubScreen
        End Get
        Set(ByVal Value As Boolean)
            m_bSubScreen = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property ParentOIKey() As String
        Get
            Return m_sParentOIKey
        End Get
        Set(ByVal Value As String)
            m_sParentOIKey = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property ChildOIKey() As String
        Get
            Return m_sChildOIKey
        End Get
        Set(ByVal Value As String)
            m_sChildOIKey = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property ParentObjectName() As String
        Get
            Return m_sParentObjectName
        End Get
        Set(ByVal Value As String)
            m_sParentObjectName = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property ChildObjectName() As String
        Get
            Return m_sChildObjectName
        End Get
        Set(ByVal Value As String)
            m_sChildObjectName = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property GISObjectName() As String
        Get
            Return m_sGISObjectName
        End Get
        Set(ByVal Value As String)
            m_sGISObjectName = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property ScreenValues() As Object
        Get
            'Return VB6.CopyArray(m_vScreenValues)
            Return m_vScreenValues
        End Get
        Set(ByVal Value As Object)
            m_vScreenValues = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property IsRiAtRiskLevel() As Integer
        Get
            Return m_iIsRiAtRiskLevel
        End Get
        Set(ByVal Value As Integer)
            m_iIsRiAtRiskLevel = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property IsAutoReinsured() As Integer
        Get
            Return m_iIsAutoReinsured
        End Get
        Set(ByVal Value As Integer)
            m_iIsAutoReinsured = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property ReferReasons() As String
        Get
            Return m_sReferReasons
        End Get
        Set(ByVal Value As String)
            m_sReferReasons = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property DeclineReasons() As String
        Get
            Return m_sDeclineReasons
        End Get
        Set(ByVal Value As String)
            m_sDeclineReasons = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property Messages() As String
        Get
            Return m_sMessages
        End Get
        Set(ByVal Value As String)
            m_sMessages = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property QuoteType() As String
        Get
            Return m_sQuoteType
        End Get
        Set(ByVal Value As String)
            m_sQuoteType = Value
        End Set
    End Property

    ' {* USER DEFINED CODE (End) *}


    <Browsable(True)>
    Public Shadows Property BackColor() As Integer
        Get

            Return m_BackColor
        End Get
        Set(ByVal Value As Integer)

            m_BackColor = Value
            RaiseEvent BackColorChange()
        End Set
    End Property


    <Browsable(True)>
    Public Shadows Property ForeColor() As Integer
        Get
            Return m_ForeColor
        End Get
        Set(ByVal Value As Integer)
            m_ForeColor = Value
            RaiseEvent ForeColorChange()
        End Set
    End Property


    <Browsable(True)>
    Public Shadows Property Enabled() As Boolean
        Get
            Return m_Enabled
        End Get
        Set(ByVal Value As Boolean)
            m_Enabled = Value
            RaiseEvent EnabledChange()
        End Set
    End Property


    <Browsable(True)>
    Public Overrides Property Font() As Font
        Get
            m_Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Return m_Font
        End Get
        Set(ByVal Value As Font)
            m_Font = Value
            RaiseEvent FontChange()
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property DebugMode() As Boolean
        Set(ByVal Value As Boolean)
            'raise up to iPMURisk.frmInterface
            RaiseEvent DebugModeChanged(Me, New DebugModeChangedEventArgs(Value))
        End Set
    End Property

    <Browsable(True)>
    Public Property BackStyle() As Integer
        Get


            Return m_BackStyle
        End Get
        Set(ByVal Value As Integer)


            m_BackStyle = Value
            RaiseEvent BackStyleChange()
        End Set
    End Property


    <Browsable(True)>
    Public Shadows Property BorderStyle() As Integer
        Get


            Return m_BorderStyle
        End Get
        Set(ByVal Value As Integer)


            m_BorderStyle = Value
            RaiseEvent BorderStyleChange()
        End Set
    End Property

    <Browsable(True)>
    Public Property FromEvent() As Boolean
        Get


            Return m_bEvent
        End Get
        Set(ByVal Value As Boolean)


            m_bEvent = Value
            RaiseEvent FromEventChange()
        End Set
    End Property

    <Browsable(True)>
    Public Property KeyArray() As Object
        Get  'PN24176
            Return m_vKeyArray
        End Get
        Set(ByVal Value As Object)  'PN24176
            m_vKeyArray = Value
        End Set
    End Property
    <Browsable(True)>
    Public Property PropertyName() As String
        Get
            PropertyName = m_sPropertyName

        End Get
        Set(ByVal value As String)
            m_sPropertyName = value

        End Set
    End Property

    <Browsable(True)>
    Public Property ColumnName() As String
        Get
            ColumnName = m_sColumnName
        End Get
        Set(ByVal value As String)
            m_sColumnName = value

        End Set

    End Property

    <Browsable(True)>
    Public WriteOnly Property CancelSilently() As Boolean
        Set(ByVal value As Boolean)
            m_bCancelSilently = value
        End Set
    End Property

    ' RAW 21/05/2004 : Performance Changes : added
    <Browsable(False)>
    Public ReadOnly Property TabNumberForControl(ByVal v_vControl As Object) As Integer
        Get


            Dim result As Integer = 0

            Try   ' ignore errors

                result = -1

                'What frame is the control in

                Dim vFrameIndex As Object = ReflectionHelper.GetMember(ReflectionHelper.GetMember(v_vControl, "Container"), "Index")


                Dim dbNumericTemp As Double
                If Not Double.TryParse(vFrameIndex, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Or Object.Equals(vFrameIndex, Nothing) Then
                    Return result
                End If

                ' What tab is the frame on
                Dim vTabNumber As Object = m_vFrameArray(ACFTabNumber, vFrameIndex)


                Dim dbNumericTemp2 As Double
                If Not Double.TryParse((vTabNumber), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Or Object.Equals(vTabNumber, Nothing) Then
                    Return result
                End If

                Dim iTabNumber As Integer = (vTabNumber)


                Return iTabNumber

            Catch exc As System.Exception
            End Try

        End Get
    End Property

    ' RAW 09/07/2004 : JIT : added
    Private ReadOnly Property ControlType(ByVal v_lScreenDetailIndex As Integer) As enumControlType
        Get

            Dim result As enumControlType = RiskScreen.enumControlType.eControlType_Unknown
            Dim lTag As Integer
            Dim sPropertyName, sColumnName As String
            Dim lDataType As Integer

            result = enumControlType.eControlType_Unknown


            If Convert.IsDBNull(m_vScreenDetails(ACDGISPropertyId, v_lScreenDetailIndex)) Or IsNothing(m_vScreenDetails(ACDGISPropertyId, v_lScreenDetailIndex)) Or m_vScreenDetails(ACDGISPropertyId, v_lScreenDetailIndex) = "" Then
                ' tab, frame, special or list view

                If Not IsNothing(m_vScreenDetails(ACDIsFrame, v_lScreenDetailIndex)) AndAlso m_vScreenDetails(ACDIsFrame, v_lScreenDetailIndex) = gPMConstants.PMEReturnCode.PMFalse Then
                    ' this is a tab
                    result = enumControlType.eControlType_Tab
                Else
                    ' So it's a frame, special or a list view
                    If m_vScreenDetails(ACDChildScreenId, v_lScreenDetailIndex) = "0" Then
                        m_vScreenDetails(ACDChildScreenId, v_lScreenDetailIndex) = Nothing
                    End If

                    If Not (Convert.IsDBNull(m_vScreenDetails(ACDChildScreenId, v_lScreenDetailIndex)) Or IsNothing(m_vScreenDetails(ACDChildScreenId, v_lScreenDetailIndex))) Then
                        result = enumControlType.eControlType_ListView
                    Else
                        ' its a frame or a special
                        result = enumControlType.eControlType_Frame
                    End If

                    ' but wait - is it something even more special
                    If CBool(m_vScreenDetails(ACDGISObjectId, v_lScreenDetailIndex)) Then
                        ' special or a list view


                        lTag = CInt(CDbl(m_vScreenDetails(ACDDefaultObjectId, v_lScreenDetailIndex)) Mod 10000)

                        If (m_vDataDictionary(GISDMTypeRisk)(ACOIsNonGIS, lTag)) = GISOTPeril Then
                            result = enumControlType.eControlType_ClaimPeril
                        End If
                    End If

                End If
            Else
                'So it's a control linked to a property - but which one?



                If Not (Convert.IsDBNull(m_vScreenDetails(ACDGISObjectId, v_lScreenDetailIndex)) Or IsNothing(m_vScreenDetails(ACDGISObjectId, v_lScreenDetailIndex)) Or m_vScreenDetails(ACDGISObjectId, v_lScreenDetailIndex) = "") Then
                    'non null object id so its linked to a GIS object

                    lTag = CInt(m_vScreenDetails(ACDDefaultPropertyId, v_lScreenDetailIndex))
                    sPropertyName = (m_vDataDictionary(GISDMTypeRisk)(ACPPropertyName, CInt(lTag Mod 10000)))
                    sColumnName = (m_vDataDictionary(GISDMTypeRisk)(ACPColumnName, CInt(lTag Mod 10000)))

                    lDataType = 0


                    If CStr(m_vDataDictionary(GISDMTypeRisk)(uctRiskScreenControl.MainModule.ACPDataType, CInt(lTag Mod 10000))) <> "" Then
                        lDataType = CInt(m_vDataDictionary(GISDMTypeRisk)(uctRiskScreenControl.MainModule.ACPDataType, CInt(lTag Mod 10000)))
                    End If


                    Select Case (m_vDataDictionary(GISDMTypeRisk)(ACPSpecialsType, CInt(lTag Mod 10000)))
                        Case ACOGISUserDefHeaderID
                            result = enumControlType.eControlType_GISLookup
                        Case ACOPartyTypeID
                            result = enumControlType.eControlType_Party
                        Case ACOGISListID
                            result = enumControlType.eControlType_GeminiCombo
                        Case ACOProductID
                            result = enumControlType.eControlType_Policy
                        Case ACOPMLookupTableName
                            result = enumControlType.eControlType_PMLookup
                        Case ACOSumInsuredTypeID
                            result = enumControlType.eControlType_SumInsured
                        Case ACOStdWordingType
                            result = enumControlType.eControlType_StandardWording
                        Case ACOReserveID
                            result = enumControlType.eControlType_ClaimReserve
                        Case ACOPaymentID
                            result = enumControlType.eControlType_ClaimPayment
                        Case ACOCaseHeader
                            result = enumControlType.eControlType_CaseHeader
                        Case ACOCaseClaimList
                            result = enumControlType.eControlType_CaseClaimList
                        Case Else
                            If sPropertyName = "" Then
                                'OK, so here it's a listview, with the buttons etc.
                                result = enumControlType.eControlType_ListView

                            ElseIf (sColumnName.ToUpper() = "ACCUMULATION") Then
                                result = enumControlType.eControlType_Accumulation
                            Else
                                If sColumnName.ToUpper().StartsWith("ADDRESS_CNT") Then
                                    result = enumControlType.eControlType_Address
                                Else
                                    'If we're here then it's a textbox or checkbox
                                    Select Case lDataType
                                        '                    Case PMBoolean
                                        Case iGISSharedConstants.GISDataTypeOption
                                            result = enumControlType.eControlType_CheckBox
                                        Case Else
                                            result = enumControlType.eControlType_TextBox
                                    End Select
                                End If
                            End If
                    End Select

                Else
                    'must be a non DB associated control


                    Select Case CInt(m_vScreenDetails(ACDGISPropertyId, v_lScreenDetailIndex))
                        Case ndcFreeFormatText
                            result = enumControlType.eControlType_Label
                        Case ndcHyperlink
                            result = enumControlType.eControlType_HyperLink
                        Case ndcFindControl
                            result = enumControlType.eControlType_Find
                    End Select
                End If

            End If

            Return result
        End Get
    End Property

    'True if risk has just been copied
    <Browsable(False)>
    Public WriteOnly Property CopyRisk() As Boolean
        Set(ByVal Value As Boolean)

            m_bCopyRisk = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property GISPolicyLinkID() As Integer
        Set(ByVal Value As Integer)

            m_lGISPolicyLinkID = Value
        End Set
    End Property
    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' RAW 20/08/2004 : JIT : added
    Private ReadOnly Property IsDynamicLogicSuppressed() As Boolean
        Get

            Dim result As Boolean = False
            result = m_bSuppressDynamicLogic

            If Not (m_oNewControlBeingLoaded Is Nothing) Then
                ' a control is in the process of being added to the form and loaded with data
                ' RAW 21/09/2004 : JIT : removed test for (m_bFireDLStartEvents=true)
                result = True
            End If
            Return result
        End Get
    End Property

    <Browsable(False)>
    Public WriteOnly Property RenewalConfirmationMode() As Boolean
        Set(ByVal Value As Boolean)  'PN19313

            m_bRenewalConfirmationMode = Value
        End Set
    End Property
    <Browsable(False)>
    Public WriteOnly Property IsWhatIfQ() As Boolean
        Set(ByVal Value As Boolean)  'PN19313

            m_bIsWhatIfQ = Value
        End Set
    End Property

    'PN 29235
    <Browsable(True)>
    Public Property ValueEdited() As Boolean
        Get

            Return m_bValueEdited
        End Get
        Set(ByVal Value As Boolean)

            m_bValueEdited = Value
        End Set
    End Property
    <Browsable(True)>
    Public Property ValueEditedForIndex() As Integer
        Get
            Return m_iValueEditedForIndex
        End Get
        Set(ByVal Value As Integer)

            m_iValueEditedForIndex = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property NoTransactions() As Boolean
        Set(ByVal Value As Boolean)

            m_bNoTransactions = Value
        End Set
    End Property


    <Browsable(True)>
    Public Property CaseID() As Integer
        Get

            Return m_lCaseID
        End Get
        Set(ByVal Value As Integer)

            m_lCaseID = Value
        End Set
    End Property
    Public Property CaseNumber() As String
        Get
            Return m_sCaseNumber
        End Get
        Set(ByVal Value As String)
            m_sCaseNumber = Value
        End Set
    End Property
    <Browsable(True)>
    Public Property BaseCaseID() As Integer
        Get

            Return m_lBaseCaseID
        End Get
        Set(ByVal Value As Integer)

            m_lBaseCaseID = Value
        End Set
    End Property
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)
    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: CancelClick
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function CancelClick() As Integer
        Dim result As Integer = 0

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            Return CancelRisk()
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CancelClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelClick", excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CancelRisk
    '
    ' Description: Called when we wish to cancel any changes
    '
    ' ***************************************************************** '
    Private Function CancelRisk() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                ' If we are cancelling out of a new renewal what-if quote then remove the risk data
                ' and policy version. We have to be sure we are only doing this when doing a NEW what-if
                ' rather than viewing a renewal quote (original or what if) or view a confirmed quote or
                ' confirming a renewal quote!   PN19313
                If m_sTransactionType <> ACPMTransactionTypeReview And Not m_bRenewalConfirmationMode And m_bIsWhatIfQ Then
                    If m_lInsuranceFileCnt > 0 Then


                        m_lReturn = ReflectionHelper.Invoke(m_oInterface, "DeleteQuote", New Object() {m_lInsuranceFileCnt})
                    End If
                End If

                ' Everything OK, so we can hide the interface.
                'Me.Hide

                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Cancel the party", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelRisk", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetRisk
    '
    ' Description: Gets the interface details and sets the appropriate
    '              style.
    '
    ' ***************************************************************** '
    Public Function GetRisk() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_bIsLoading = True

#If quoteTiming Then

			performanceCtr(performancecntrCntr, 1) = "uctRiskSCreen.GetRisk start"
			QueryPerformanceCounter performanceCtr(performancecntrCntr, 0): performancecntrCntr = performancecntrCntr + 1
			QueryPerformanceCounter performanceCtr(performancecntrCntr, 0): performancecntrCntr = performancecntrCntr + 1
#End If

            m_lReturn = GetBusiness()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If IsSilentQuote Then
                SelectTab(TabStrip1, 0) ' this will fire click event - tab number is 0-based
                m_bIsLoading = False
                Return result
            End If
            ' Assign the details from the business object
            ' to the interface.
            '        m_lReturn& = BusinessToInterface()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '    End If

            'Then we need to build the screen, as that also populates the controls

            m_oFormFields = New iPMFormControl.FormFields()

            m_lReturn = m_oFormFields.AddNewFormField(txtDate, gPMConstants.PMEFormatStyle.PMFormatDateLong, gPMConstants.PMEDataType.PMDate)

            m_lReturn = m_oFormFields.AddNewFormField(txtCurrency, gPMConstants.PMEFormatStyle.PMFormatCurrency, gPMConstants.PMEDataType.PMCurrency)


            m_lReturn = BuildScreen()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()


            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the task.
            If Task = gPMConstants.PMEComponentAction.PMView Then
                ' Disable the interface to only allow viewing.
                m_lReturn = DisableForm(True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to disable the interface
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            m_bSuppressDynamicLogic = False

            If Not m_bFireDLStartEvents Then
                RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_Initialise, v_vDLAction:="", v_vTabNumber:=-1)
            End If


            'Reset to the first tab
            SelectTab(TabStrip1, 0)  ' this will fire click event - tab number is 0-based
            m_bIsLoading = False
            TabStrip1_Click(Nothing, Nothing)
            Return result
        Catch excep As Exception

            result = gPMConstants.PMEReturnCode.PMError
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the risk", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRisk", excep:=excep)
            Return result

        End Try

    End Function

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Static bIsInitialised As Boolean
        Const kMethodName As String = "Initialise"
        Dim sTitle, sMessage As String

        Dim sHelpFile As String = ""
        Dim m_lReturn As Integer
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Dim sValue As String = ""
        Dim oUserAuthority As Object

        Try
            g_iCheckBoxValue = 1  ' Setting it to 1 
            result = gPMConstants.PMEReturnCode.PMTrue

            ' RAW 09/07/2004 : JIT : added
#If DebugToClip Then

			Clipboard.Clear
#End If

#If quoteTiming Then

			Clipboard.Clear
			QueryPerformanceFrequency performanceFreq
			performancecntrCntr = 0
			performanceCtr(performancecntrCntr, 1) = "uctRiskScreen.Initialise"
			QueryPerformanceCounter performanceCtr(performancecntrCntr, 0): performancecntrCntr = performancecntrCntr + 1
#End If


            ' Check if already initialised
            If bIsInitialised Then
                Return result
            End If


            m_cMandatoryProperties = New Collection()

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(uctRiskScreenControl.ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.
                'LogMessagePopup(gPMConstants.PMELogLevel.PMLogError, "Failed to initialise the object manager", ACApp, ACClass, "Initialise")
                RaiseError(kMethodName, "Failed to initialise the object manager. ", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If

            ' If UserID is 0 assume that user cancelled logon
            If g_oObjectManager.UserID = 0 Then
                ' Exit application
                result = gPMConstants.PMEReturnCode.PMFalse
                g_oObjectManager = Nothing
                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                g_iUserID = .UserID
                g_sUsername = .UserName
            End With

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            m_lReturn = GetPMRegSetting(eRegSettingRoot, eProductFamily, eRegSettingLevel, "HelpFile", sHelpFile)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrieve Helpfile", Application.ProductName)
                Return result
            End If

            If sHelpFile <> "" Then
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get an instance of the business object via
            ' the public object manager.

            Dim temp_m_oInterface As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oInterface, "iPMUScreenControl.Interface_Renamed", PMGetLocalInterface)
            m_oInterface = temp_m_oInterface

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.
                ' Get description from the resource file.
                sTitle = (iPMFunc.GetResData(g_iLanguageID, ACBusinessFailTitle, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                sMessage = (iPMFunc.GetResData(g_iLanguageID, ACBusinessFail, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If


            '   PW 7/8/03 - CQ2083 : Pass through original task so that validation scripts
            '                        returns correct run mode when deleting.



            'm_lReturn = ReflectionHelper.Invoke(m_oInterface, "SetProcessModes", New Object() {m_iOriginalTask, m_sTransactionType})
            m_lReturn = m_oInterface.SetProcessModes(vTask:=m_iOriginalTask, vTransactionType:=m_sTransactionType)

            'm_lReturn = m_oInterface.SetProcessModes(vTask:=m_iTask, vTransactionType:=m_sTransactionType)

            ' Pass the keyarray on to iPMUScreenControl as it needs it to pass onto child screens  PN24176

            m_lReturn = m_oInterface.SetKeys(m_vKeyArray)

            'Reset the indexes
            m_lCheckIndex = -1
            m_lComboIndex = -1
            m_lGeminiComboIndex = -1
            m_lGISComboIndex = -1
            m_lPMLookupIndex = -1
            m_lPartyCommandIndex = -1
            m_lPolicyCommandIndex = -1
            m_lTextIndex = -1
            m_lMlTextIndex = -1
            m_lListViewIndex = -1
            m_lStandardWordingIndex = -1
            m_lSumInsuredIndex = -1
            m_lAddressIndex = -1
            m_lFrameIndex = -1
            m_lTabIndex = -1
            m_lFindControlIndex = -1

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' hold Initialised status
            bIsInitialised = True

            'PR 10102002 - Get product option

            m_lReturn = iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTRemoveNone, g_iSourceID, sValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "getProductOptionValue Failed ", ACApp, ACClass, "Initialise")
                RaiseError(kMethodName, "getProductOptionValue Failed for option [SIROPTRemoveNone]. ", gPMConstants.PMELogLevel.PMLogError)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_bRemoveNoneOption = sValue = "1"

            m_bSuppressDynamicLogic = True  ' RAW 22/06/2004 : Performance Changes(#2) : added


            'control collection initialised for JIT control loading
            ' controls must be added to here when they are created/built
            m_cObjectAndAttribute = New Collection()  ' RAW 09/07/2004 : JIT : added


            ' RAW 09/07/2004 : JIT : added
            ' create and set ref to dynamic logic engine
            m_oDLEngine = New DLEngine()
            m_lReturn = m_oDLEngine.Initialise(Me)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Failed to initialise dynamic logic engine", ACApp, ACClass, "Initialise")
                RaiseError(kMethodName, "Failed to initialise dynamic logic engine", gPMConstants.PMELogLevel.PMLogError)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' RAW 09/07/2004 : JIT : end

            m_vUnderwritingOrAgency = "U"
            'm_lReturn = iPMFunc.getUnderwritingOrAgency(m_vUnderwritingOrAgency)
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Failed to getUnderwritingOrAgency", ACApp, ACClass, "Initialise")
            '    Return gPMConstants.PMEReturnCode.PMFalse
            'End If

            'PN24176
            Dim temp_m_oSIRRiskScreen As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oSIRRiskScreen, "bSIRRiskScreen.Business", PMGetViaClientManager)
            m_oSIRRiskScreen = temp_m_oSIRRiskScreen

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Failed to initialise bSIRRiskScreen.Business", ACApp, ACClass, "Initialise")
                RaiseError(kMethodName, "Failed to initialise bSIRRiskScreen.Business", gPMConstants.PMELogLevel.PMLogError)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ReDim m_vWindowKeys(1, 0)
            m_vWindowKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMKeyNameKeepWindowOnTop
            m_vWindowKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = CStr(0)

            m_bDLDebuggingAllowed = False
            iPMFunc.RetrieveSingleSystemOption(5084, sValue)
            Dim vParams As Object
            Const ACUserCanDebugDynamicLogicScripts As Integer = 14
            If sValue = "1" Then

                Dim temp_oUserAuthority As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oUserAuthority, "bACTUserAuthorities.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oUserAuthority = temp_oUserAuthority

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise bACTUserAuthorities.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                    RaiseError(kMethodName, "Failed to initialise bACTUserAuthorities.Business", gPMConstants.PMELogLevel.PMLogError)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'UPGRADE_TODO: (1067) Member GetDetails is not defined in type bACTUserAuthorities.Business. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                m_lReturn = oUserAuthority.GetDetails(vUserID:=g_iUserID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise bACTUserAuthorities.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                    RaiseError(kMethodName, "oUserAuthority.GetDetails failed for UserID [" & CStr(g_iUserID) & "]", gPMConstants.PMELogLevel.PMLogError)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'UPGRADE_TODO: (1067) Member GetNext is not defined in type bACTUserAuthorities.Business. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                m_lReturn = oUserAuthority.GetNext(vParams:=vParams)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the details for the User Authority", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                    RaiseError(kMethodName, "oUserAuthority.GetNext( failed for UserID [" & CStr(g_iUserID) & "]", gPMConstants.PMELogLevel.PMLogError)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If gPMFunctions.ToSafeInteger(vParams(ACUserCanDebugDynamicLogicScripts), 0) = 1 Then
                    m_bDLDebuggingAllowed = True
                End If
            End If

#If quoteTiming Then

			performanceCtr(performancecntrCntr, 1) = "initialise"
			QueryPerformanceCounter performanceCtr(performancecntrCntr, 0): performancecntrCntr = performancecntrCntr + 1
#End If
            m_lGisLookupIndexArray = New Dictionary(Of Integer, Integer)
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            LogMessagePopup(gPMConstants.PMELogLevel.PMLogOnError, "Failed to initialise the object", ACApp, ACClass, "Initialise", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LoadControl
    '
    ' Description: Does all the extra stuff that initialise doesn't
    '
    ' ***************************************************************** '
    Public Function LoadControl() As Integer

        Dim result As Integer = 0
        Dim lDebugDepthCounter As Integer

        ' Forms load event.

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then


        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : LoadControl started")
#End If

            m_bIsLoading = True  ' RAW 21/05/2004 : Performance Changes : added


            'We first need to get the screen layout

            m_lReturn = GetScreenDetails()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                m_bIsLoading = False  ' RAW 21/05/2004 : Performance Changes : added
                result = gPMConstants.PMEReturnCode.PMFalse

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then


            AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : LoadControl errored")
#End If

                Return result
            End If


            m_bIsLoading = False  ' RAW 21/05/2004 : Performance Changes : added


#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then


        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : LoadControl finished")
#End If


            m_lParentHwnd = MyBase.FindForm().Handle.ToInt32()

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        Catch ex As Exception

            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to load control", ACApp, ACClass, "LoadControl", Information.Err().Number, Information.Err().Description, excep:=ex)

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then


        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : LoadControl errored")
#End If

            m_bIsLoading = False  ' RAW 21/05/2004 : Performance Changes : added



        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: Refresh
    '
    ' Description: What is this supposed to do?
    '
    ' ***************************************************************** '
    Public Overrides Sub Refresh()

    End Sub

    ' ***************************************************************** '
    ' Name: SaveRisk
    '
    ' Description: Saves the displayed party details
    '
    ' ***************************************************************** '
    Private Function SaveRisk() As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'Validate some address stuff
            'm_lReturn = ValidateOK()

            'If (m_lReturn& <> PMTrue) Then
            '    Exit Function

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = ProcessCommand()
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMCancel) Then
                ' Check the return value.
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    'update the risk cnt property - in case it has changed
                    If m_oInterface Is Nothing Then
                        Dim temp_m_oscreencontrol As Object
                        If g_oObjectManager Is Nothing Then
                            g_oObjectManager = New bObjectManager.ObjectManager
                        End If
                        m_lReturn = g_oObjectManager.GetInstance(temp_m_oscreencontrol, "iPMUScreenControl.Interface_Renamed", PMGetLocalInterface)
                        m_oInterface = temp_m_oscreencontrol
                    End If
                    m_lRiskId = ReflectionHelper.GetMember(m_oInterface, "RiskId")
                    ' Everything OK, so we can hide the interface.
                    'Me.Hide
                    result = gPMConstants.PMEReturnCode.PMTrue

                Else

                    ' Log Error.
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed on ProcessCommand", ACApp, ACClass, "SaveRisk", Information.Err().Number, Information.Err().Description)

                End If
            Else
                SaveRisk = gPMConstants.PMEReturnCode.PMCancel
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save the party", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveRisk", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SaveLinkToParty
    '
    ' Description: Calls the business-side function that encapsulates all the
    '              database updates that take place when adding or deleting
    '              a link to a party.
    '
    ' History: RAW110804 - created (resilience)
    ' ***************************************************************** '
    Private Function SaveLinkToParty(ByVal v_iTask As Integer, ByVal v_lClaimID As Integer, ByVal v_lOldPartyId As Integer, ByVal v_lNewPartyId As Integer) As Integer
        Dim result As Integer = 0
        Const sFunctionName As String = "SaveLinkToParty"


        Dim oBusinessStateless As bSIRRiskScreen.Stateless
        Dim TheInput As XMLTransRiskScreenLinkToParty.RiskScreenLinkToPartyIn = XMLTransRiskScreenLinkToParty.RiskScreenLinkToPartyIn.CreateInstance()  ' RAW 03/09/2004 : Resilience (#2) : added
        Dim TheOutput As XMLTransRiskScreenLinkToParty.RiskScreenLinkToPartyOut = XMLTransRiskScreenLinkToParty.RiskScreenLinkToPartyOut.CreateInstance()  ' RAW 03/09/2004 : Resilience (#2) : replaced SIRRiskScreenBasicOut

        Dim sInputXML, sOutputXML, sMsg As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the business object
            Dim temp_oBusinessStateless As Object
            If g_oObjectManager.GetInstance(temp_oBusinessStateless, "bSIRRiskScreen.Stateless", PMGetViaClientManager) <> gPMConstants.PMEReturnCode.PMTrue Then
                oBusinessStateless = temp_oBusinessStateless

                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, " Failed to get instance of bSIRRiskScreen.Stateless", ACApp, ACClass, sFunctionName, Nothing, Nothing, g_sUsername)
                Return result ' RAW 03/09/2004 : Resilience (#2) : replaced call to exit function
            Else
                oBusinessStateless = temp_oBusinessStateless
            End If


            ' RAW 03/09/2004 : Resilience (#2) : removed call to r_oObject.SetProcessModes


            ' save the data
            ' ===============================
            ' RAW 03/09/2004 : Resilience (#2) : added
            'Populate the XML for the call to the stateless object
            With TheInput
                .iTask = CShort(v_iTask)
                .iSourceID = CShort(g_iSourceID)
                .lNavigate = m_lNavigate
                .lProcessMode = m_lProcessMode
                .sTransactionType = m_sTransactionType
                .dtEffectiveDate = m_dtEffectiveDate
                .lOldPartyId = v_lOldPartyId
                .lNewPartyId = v_lNewPartyId
                .lClaimID = v_lClaimID
            End With

            'Serialize the input
            sInputXML = SerializeRiskScreenLinkToPartyIn(TheInput)

            ' RAW 03/09/2004 : Resilience (#2) : replaced params with XML string

            sOutputXML = oBusinessStateless.RiskScreenLinkToParty(v_sInput:=sInputXML)
            ' process the results
            TheOutput = DeserializeRiskScreenLinkToPartyOut(sOutputXML)

            If TheOutput.HasErrors Then
                sMsg = ComposeErrorString(TheOutput.Errors)
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, " Failed to save to database - " & sMsg, ACApp, ACClass, sFunctionName, Nothing, Nothing, g_sUsername)
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result  ' RAW 03/09/2004 : Resilience (#2) : replaced call to exit function

            ElseIf TheOutput.HasWarnings Then
                sMsg = ComposeWarningString(TheOutput.Warnings)
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Warning - " & sMsg, ACApp, ACClass, sFunctionName, Nothing, Nothing, g_sUsername)
                'Nothing too serious so continue
            End If


            ' RAW 03/09/2004 : Resilience (#2) : added
            'Get the values from the stateless object output
            With TheOutput
                ' none
            End With





        Catch ex As Exception

            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to save link to party", ACApp, ACClass, sFunctionName, Information.Err().Number, Information.Err().Description, excep:=ex)

        Finally
            ' terminate the business object.
            If Not (oBusinessStateless Is Nothing) Then

                oBusinessStateless.Dispose()
                oBusinessStateless = Nothing
            End If

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ShowHelpScreen
    '
    ' Description: Shows the help screen
    '
    ' ***************************************************************** '
    Public Function ShowHelpScreen(Optional ByRef cmdHelp As Object = Nothing, Optional ByRef ScreenHelpID As Object = Nothing) As Integer
        ' Fire up the help screen
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        Return SSfunc.ShowHelp(cmdHelp, ScreenHelpID)

    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(ByRef vTask_optional As Object, ByRef vNavigate_optional As Object, ByRef vProcessMode_optional As Object, ByRef vTransactionType_optional As Object, ByRef vEffectiveDate_optional As Object) As Integer
        Dim vTask As Object = Nothing
        If vTask_optional Is Nothing Or Not vTask_optional.Equals(Type.Missing) Then vTask = TryCast(vTask_optional, Object)
        Dim vNavigate As Object = Nothing
        If vNavigate_optional Is Nothing Or Not vNavigate_optional.Equals(Type.Missing) Then vNavigate = TryCast(vNavigate_optional, Object)
        Dim vProcessMode As Object = Nothing
        If vProcessMode_optional Is Nothing Or Not vProcessMode_optional.Equals(Type.Missing) Then vProcessMode = TryCast(vProcessMode_optional, Object)
        Dim vTransactionType As Object = Nothing
        If vTransactionType_optional Is Nothing Or Not vTransactionType_optional.Equals(Type.Missing) Then vTransactionType = TryCast(vTransactionType_optional, Object)
        Dim vEffectiveDate As Object = Nothing
        If vEffectiveDate_optional Is Nothing Or Not vEffectiveDate_optional.Equals(Type.Missing) Then vEffectiveDate = TryCast(vEffectiveDate_optional, Object)
        Try

            Dim result As Integer = 0
            Try

                result = gPMConstants.PMEReturnCode.PMTrue

                ' Assign the process modes to the property members.

                If Not Not (vTask_optional Is Nothing) AndAlso vTask_optional.Equals(Type.Missing) Then
                    m_iTask = CType((vTask), gPMConstants.PMEComponentAction)
                End If

                If Not Not (vNavigate_optional Is Nothing) AndAlso vNavigate_optional.Equals(Type.Missing) Then
                    m_lNavigate = (vNavigate)
                End If

                If Not Not (vProcessMode_optional Is Nothing) AndAlso vProcessMode_optional.Equals(Type.Missing) Then
                    m_lProcessMode = (vProcessMode)
                End If

                If Not Not (vTransactionType_optional Is Nothing) AndAlso vTransactionType_optional.Equals(Type.Missing) Then
                    m_sTransactionType = (vTransactionType)
                End If

                If Not Not (vEffectiveDate_optional Is Nothing) AndAlso vEffectiveDate_optional.Equals(Type.Missing) Then
                    m_dtEffectiveDate = (vEffectiveDate)
                End If

                Return result

            Catch excep As System.Exception



                ' Error Section.

                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", excep:=excep)

                Return result

            End Try
        Finally
            vTask_optional = vTask
            vNavigate_optional = vNavigate
            vProcessMode_optional = vProcessMode
            vTransactionType_optional = vTransactionType
            vEffectiveDate_optional = vEffectiveDate
        End Try
    End Function

    Public Function SetProcessModes(ByRef vTask_optional As Object, ByRef vNavigate_optional As Object, ByRef vProcessMode_optional As Object, ByRef vTransactionType_optional As Object) As Integer
        Dim tempRefParam As Object = Type.Missing
        Return SetProcessModes(vTask_optional, vNavigate_optional, vProcessMode_optional, vTransactionType_optional, tempRefParam)
    End Function

    Public Function SetProcessModes(ByRef vTask_optional As Object, ByRef vNavigate_optional As Object, ByRef vProcessMode_optional As Object) As Integer
        Dim tempRefParam2 As Object = Type.Missing
        Dim tempRefParam3 As Object = Type.Missing
        Return SetProcessModes(vTask_optional, vNavigate_optional, vProcessMode_optional, tempRefParam2, tempRefParam3)
    End Function

    Public Function SetProcessModes(ByRef vTask_optional As Object, ByRef vNavigate_optional As Object) As Integer
        Dim tempRefParam4 As Object = Type.Missing
        Dim tempRefParam5 As Object = Type.Missing
        Dim tempRefParam6 As Object = Type.Missing
        Return SetProcessModes(vTask_optional, vNavigate_optional, tempRefParam4, tempRefParam5, tempRefParam6)
    End Function

    Public Function SetProcessModes(ByRef vTask_optional As Object) As Integer
        Dim tempRefParam7 As Object = Type.Missing
        Dim tempRefParam8 As Object = Type.Missing
        Dim tempRefParam9 As Object = Type.Missing
        Dim tempRefParam10 As Object = Type.Missing
        Return SetProcessModes(vTask_optional, tempRefParam7, tempRefParam8, tempRefParam9, tempRefParam10)
    End Function

    Public Function SetProcessModes() As Integer
        Dim tempRefParam11 As Object = Type.Missing
        Dim tempRefParam12 As Object = Type.Missing
        Dim tempRefParam13 As Object = Type.Missing
        Dim tempRefParam14 As Object = Type.Missing
        Dim tempRefParam15 As Object = Type.Missing
        Return SetProcessModes(tempRefParam11, tempRefParam12, tempRefParam13, tempRefParam14, tempRefParam15)
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                m_bControlIsUnloading = True
                If m_oDLEngine IsNot Nothing Then
                    m_oDLEngine.Dispose()
                    m_oDLEngine = Nothing
                End If
                m_oMSScriptControl = Nothing
                m_oGIS = Nothing

                ' DO NOT TERMINATE this object because I did  not create it - just clear my reference to it
                m_oGISOriginal = Nothing
                ' RAW 10/07/2003 : end
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
                If m_oInterface IsNot Nothing Then
                    m_oInterface.Dispose()
                    m_oInterface = Nothing
                End If
                If m_oFormFields IsNot Nothing Then
                    m_oFormFields.Dispose()
                    m_oFormFields = Nothing


                End If

                If Not (m_oSIRRiskScreen Is Nothing) Then
                    m_oSIRRiskScreen.Dispose()
                    m_oSIRRiskScreen = Nothing
                End If
                If Not (m_oPBRiskPolicyCurrency Is Nothing) Then

                    m_oSIRRiskScreen.Dispose()
                    m_oPBRiskPolicyCurrency = Nothing
                End If
                UnloadControlArrays()
                Erase m_ctlTabFirstLast


            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: UnloadControl
    '
    ' Description: Cleans up then unloads the control
    '
    ' ***************************************************************** '
    Public Function UnLoadControl(ByRef Cancel As Integer, ByRef UnloadMode As Integer) As Integer

        Dim lDebugDepthCounter As Integer

        ' Forms query unload event.

#If (DebugOption And m_klDebugOption_Normal) = m_klDebugOption_Normal Then
		AddToDebug(lDebugDepthCounter, "unload control")
#End If


        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    'eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Function
                End If
            End If

            'If in edit mode, unlock the party
            '    If (m_iTask = PMEdit) Then
            '
            '        m_lReturn = UnlockRisk()
            '
            '        If (m_lReturn <> PMTrue) Then
            '            UnLoadControl = PMFalse
            '        End If
            '
            '    End If

            ' Terminate the general object.
            Dispose()
            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)


        Catch ex As Exception

            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to terminate the interface", ACApp, ACClass, "UnLoadControl", Information.Err().Number, Information.Err().Description, excep:=ex)


        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ValidateRisk
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function ValidateRisk() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            If IsSilentQuote Then
                m_oFormFields = New iPMFormControl.FormFields()
            End If

            ' RAW 11/05/2004 : PN10646 : added test for not PMView (copied from 1.8.6)
            If Task <> gPMConstants.PMEComponentAction.PMView Then

                m_lReturn = m_oFormFields.CheckMandatoryControls()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = ValidateOK()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateRiskFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateRisk", excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)
    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: BusinessToData
    '
    ' Description: Updates the data storage from the business object.
    '
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details to the data storage.

            ' {* USER DEFINED CODE (Begin) *}
            '    m_lReturn& = m_oInterface.GetNext(vArray:=m_vArray)


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Failed to retrieve the details from the business object", ACApp, ACClass, "BusinessToData")
            End If

            'Get additional details required for display that not stored on this
            'record
            '    m_lReturn& = m_ointerface.GetOtherDetails(vArray:=m_vOtherArray)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Failed to retrieve the agent details from the business object ", ACApp, ACClass, "BusinessToData")
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business
    '              object.
    '
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Assign the details from the business object
            ' to the data storage.
            m_lReturn = BusinessToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result
        Catch excep As System.Exception
            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: DisableForm
    '
    ' Description: Sets all of the interface details to the disable
    '              state passed.
    '
    ' ***************************************************************** '
    Public Function ControlList(ByVal root As Control, ByRef resultArray As ArrayList) As ArrayList

        If root.HasChildren Then
            For Each cControl As Control In root.Controls
                resultArray.Add(cControl)
                If cControl.HasChildren Then
                    If Not cControl.Name.Contains("uct") Then
                        ControlList(cControl, resultArray)
                    End If
                End If
            Next cControl
        End If
        Return resultArray
    End Function
    Private Function DisableForm(ByRef lDisabled As Integer) As Integer

        Dim result As Integer = 0
        Dim arrControlList As ArrayList = New ArrayList
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            If Me.HasChildren Then
                ControlList(Me, arrControlList)
            End If
            'Modified,add a for loop and a dim condition,comment the for loop
            For arrItem As Integer = 0 To arrControlList.Count - 1

                Dim ctlFormControl As Control = arrControlList(arrItem)
                'For Each ctlFormControl As Control In Controls_Renamed
                ' Check the type of the control.

                If TypeOf ctlFormControl Is TextBox Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                    If CType(ctlFormControl, TextBox).Multiline = True Then
                        ControlHelper.SetEnabled(ctlFormControl, lDisabled)
                        CType(ctlFormControl, TextBox).ReadOnly = True
                    End If
                ElseIf (TypeOf ctlFormControl Is ComboBox) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                ElseIf (TypeOf ctlFormControl Is CheckBox) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                ElseIf (TypeOf ctlFormControl Is uctPBFindRT.PBFindRT) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                ElseIf (TypeOf ctlFormControl Is uctCLMCaseHeaders.uctCLMCaseHeader) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                ElseIf (TypeOf ctlFormControl Is uctCLMCaseClaimList.uctCLMCaseClaim) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                ElseIf (TypeOf ctlFormControl Is uctGISUserDefLookupControl.cboGISLookup) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                ElseIf (TypeOf ctlFormControl Is PMLookupControl.cboPMLookup) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)  '70653
                End If
                'Next ctlFormControl
            Next arrItem

            'Now the command buttons...
            '    cmdAgentLookUp.Enabled = Not lDisabled

            Return result

        Catch excep As System.Exception
            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the form disabling", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableForm", excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: GetScreenDetails
    '
    ' Description: Retrieves the screen details from the business object.
    '
    ' ***************************************************************** '
    Private Function GetScreenDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' {* USER DEFINED CODE (Begin) *}

            m_oInterface.PartyCnt = m_lPartyCnt
            m_oInterface.ShortName = m_sShortName
            m_oInterface.InsuranceFolderCnt = m_lInsuranceFolderCnt
            m_oInterface.InsuranceFileCnt = m_lInsuranceFileCnt
            m_oInterface.RiskId = m_lRiskId
            m_oInterface.RiskTypeId = m_lRiskTypeId
            m_oInterface.ScreenId = m_lScreenId
            m_oInterface.ChildOIKey = m_sChildOIKey

            m_lReturn = m_oInterface.GetScreenDetails(m_vDataDictionary, m_vScreenHeader, m_vScreenDetails, m_vChildScreenDetails)


            If m_iTask = SharedFiles.GeneralConst.PMAdd Then
                m_lReturn = m_oSIRRiskScreen.GetDefaultClausesForRisk(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_iSoruceId:=g_iSourceID, v_lRiskTypeId:=m_lRiskTypeId, r_vDefaultClausesForRisk:=m_vDefaultClausesForRisk)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError("GetScreen Details", "Failed to load the DefaultClauses", SharedFiles.GeneralConst.PMLogError)
                Else
                    m_bDefaultClausesForRisk = True
                End If
            End If

            ' Check for errors

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Failed to get screen details from the business object", ACApp, ACClass, "GetScreenDetails")

            End If

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to get the details from the business object", ACApp, ACClass, "GetScreenDetails", Information.Err().Number, Information.Err().Description, excep:=ex)
        Finally

        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.  Via the interface object
    '
    ' ***************************************************************** '
    Private Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' RAW 03/09/2004 : Resilience (#2) : added
            If Not m_bSubScreen Then
                ' this the top level screen
                m_lOriginalRiskId = m_lRiskId
            End If


            ' Get the details from the interface object.

            ' {* USER DEFINED CODE (Begin) *}

            m_oInterface.PartyCnt = m_lPartyCnt

            m_oInterface.ShortName = m_sShortName

            m_oInterface.InsuranceFolderCnt = m_lInsuranceFolderCnt

            m_oInterface.InsuranceFileCnt = m_lInsuranceFileCnt

            m_oInterface.RiskId = m_lRiskId

            m_oInterface.RiskTypeId = m_lRiskTypeId

            m_oInterface.ProductId = m_lProductId

            m_oInterface.ScreenId = m_lScreenId

            m_oInterface.ClaimID = m_lClaimID

            m_oInterface.CaseID = m_lCaseID


            If m_oGIS IsNot Nothing Then
                m_oInterface.GIS = m_oGIS
            End If

            If Not (m_oGISOriginal Is Nothing) Then
                m_oInterface.GISOriginal = m_oGISOriginal
            End If

            m_oInterface.ParentObjectName = m_sParentObjectName

            m_oInterface.ChildObjectName = m_sChildObjectName

            m_oInterface.ParentOIKey = m_sParentOIKey

            m_oInterface.ChildOIKey = m_sChildOIKey

            m_oInterface.SubScreen = m_bSubScreen

            m_oInterface.CopyRisk = m_bCopyRisk

            m_oInterface.GISPolicyLinkID = m_lGISPolicyLinkID

            'Are we from events?

            m_oInterface.FromEvent = FromEvent

            m_lReturn = m_oInterface.getscreenvalues(m_vScreenValues, m_vRiskDetails, m_vRiskTypeDetails)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Failed to get details from the interface object", ACApp, ACClass, "GetBusiness")

                Return result
            End If

            ' Now get the risk id in case it has changed
            m_sGISDataModel = m_oInterface.GISDataModel
            m_lRiskId = m_oInterface.RiskID
            'Get the child key back in case we were adding...
            m_sChildOIKey = m_oInterface.ChildOIKey
            If Information.IsArray(m_vRiskTypeDetails) Then

                If Convert.IsDBNull(m_vRiskTypeDetails(ACRIsRiAtRiskLevel, 0)) Or IsNothing(m_vRiskTypeDetails(ACRIsRiAtRiskLevel, 0)) Then
                    m_iIsRiAtRiskLevel = 0
                Else
                    m_iIsRiAtRiskLevel = m_vRiskTypeDetails(ACRIsRiAtRiskLevel, 0)
                End If


                If Convert.IsDBNull(m_vRiskTypeDetails(ACRIsAutoReinsured, 0)) Or IsNothing(m_vRiskTypeDetails(ACRIsAutoReinsured, 0)) Then
                    m_iIsAutoReinsured = 0
                Else
                    m_iIsAutoReinsured = m_vRiskTypeDetails(ACRIsAutoReinsured, 0)
                End If
            End If


        Catch ex As Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to get the details from the interface object", ACApp, ACClass, "GetBusiness", Information.Err().Number, Information.Err().Description, excep:=ex)


        Finally


        End Try
        Return result
    End Function


    Public Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0
        Dim lBusinessDataID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the business object.

            ' Assign the details from the interface to the data storage.
            m_lReturn = InterfaceToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Set the business data ID to one because we are only
            ' dealing with one record item only.
            lBusinessDataID = 1

            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    '            m_lReturn& = m_oInterface.EditAdd(lRow:=lBusinessDataID&, _
                    'vArray:=m_vArray)
                Case gPMConstants.PMEComponentAction.PMEdit
                    '            m_lReturn& = m_oInterface.EditUpdate(lRow:=lBusinessDataID&, _
                    'vArray:=m_vArray)
            End Select

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Failed to assign the interface details to business object", ACApp, ACClass, "InterfaceToBusiness")

            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToData
    '
    ' Description: Updates the data storage from the interface details.
    '
    ' ***************************************************************** '
    Private Function InterfaceToData() As Integer

        Dim result As Integer = 0
        Dim sMsg As String = ""
        Dim vArray As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign all of the details from the
            ' interface to the data storage.
            '
            ' Example:-
            '
            '    m_DName$ = trim$(txtName.Text)
            '    m_DDate = CDate(txtDate.Text)
            '    m_iDCodeID% = cmbCode.ItemData(cmbCode.ListIndex)
            '    m_lReturn& = m_oFormFields.UnformatControl(txtName)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            'Everything is already sorted out in the array as we go.
            'Except for the sums insured and standard wordings

            'It's easier to rejig these at the end than do it when changed...

            'First the standard wordings

            ' RAW 22/06/2004 : Performance Changes(#2) : removed code to trigger onChange events for StandardWording and SUminsured

            vArray = Nothing


            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: OKClick
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function OKClick() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'KeepWindowOnTop(False)

            m_lReturn = ValidateRisk()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            m_lReturn = SaveRisk()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
            End If

            'KeepWindowOnTop(True)
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OKClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="OKClick", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessCommand
    '
    ' Description: Determines which action to take on the details
    '              depending upon the task and interface state.
    '
    ' ***************************************************************** '
    Private Function ProcessCommand() As Integer

        Dim result As Integer = 0
        Dim iMsgResult As Integer
        Dim sMessage, sTitle As String
        Dim bRevertBackRisk As Boolean

        Const ACCLAIMOUTPUT As String = "_claim_output"
        Const ACDOCREF As String = "_document_request"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check the task.
            If Status <> gPMConstants.PMEReturnCode.PMCancel Then

                Select Case Task
                    Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit
                        ' Update the business from the interface.
                        m_lReturn = InterfaceToBusiness()

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to update business.
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                End Select

            End If

            ' Check the task.
            Select Case Task
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Check if form has been cancelled, if so,
                    ' prompt if you wish to lose details.
                    If Status = gPMConstants.PMEReturnCode.PMCancel Then
                        ' Get string messages
                        sTitle = (iPMFunc.GetResData(g_iLanguageID, ACCancelDetailsTitle, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                        sMessage = (iPMFunc.GetResData(g_iLanguageID, ACCancelDetails, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        If IsSilentQuote = False Then
                            iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                        Else
                            iMsgResult = vbYes
                        End If

                        ' Check message result.
                        If iMsgResult = System.Windows.Forms.DialogResult.No Then
                            ' Set return to false, meaning
                            ' don't cancel.
                            result = gPMConstants.PMEReturnCode.PMFalse
                        Else

                            ' RAW110804 added



                            m_lReturn = m_oInterface.SaveOnCancel(Task, False)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            '*****************
                            ' MEvans : 11-08-2003 : 223 Document Production
                            ' Clear down the output and document request objects
                            ' from within the xml - no database clear down required
                            ' as nothing will have been saved to the database at this
                            ' point
                            m_lReturn = ClearDownDocumentRequests()
                            '*****************
                        End If
                    Else
                        ' Form hasn't been cancelled, so we just go
                        ' ahead and add the details.

                        ' Add the details using the business object.

                        m_lReturn = m_oInterface.Update(m_vScreenValues)

                        m_sDeclineReasons = m_oInterface.DeclineReasons

                        m_sReferReasons = m_oInterface.ReferReasons

                        m_sMessages = m_oInterface.Messages

                        m_sQuoteType = m_oInterface.QuoteType

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' RAW 20/09/2004 : CQ6832 : removed test for declined or referred
                            ' Failed to add the details
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' Log Error.
                            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Failed to add the details (m_oInterface.Update)", ACApp, ACClass, "ProcessCommand")
                        Else
                            ' RAW 20/09/2004 : CQ6832 : added test for declined or referred
                            If (m_sDeclineReasons <> "") Or (m_sReferReasons <> "") Then
                                ' declined or referred
                            Else
                                '------------------------------------------------------------------------------
                                '   20/07/2002 RVH  BEGIN
                                '                   Call SAVE method on payment and reserve controls
                                '------------------------------------------------------------------------------
                                ' RAW110804 to be removed and moved to business object once resilience is applied to claims ????
                                m_lReturn = UpdatePaymentAndReserve()
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = gPMConstants.PMEReturnCode.PMFalse

                                    ' Log Error.
                                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Failed to update the Payment or Reserve details (UpdatePaymentAndReserve)", ACApp, ACClass, "ProcessCommand")
                                End If
                                '------------------------------------------------------------------------------
                                '   20/07/2002 RVH  END
                                '------------------------------------------------------------------------------
                                m_lReturn = UpdateCase()
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = gPMConstants.PMEReturnCode.PMFalse

                                    ' Log Error.
                                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Failed to update the case details (UpdateCase)", ACApp, ACClass, "ProcessCommand")
                                End If

                            End If
                        End If
                    End If

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Check if form has been cancelled, if so,
                    ' check if the details have changed and if
                    ' so, prompt if they wish to cancel.
                    If Status = gPMConstants.PMEReturnCode.PMCancel Then

                        ' Check the details havn't changed.
                        '                m_lReturn& = m_ointerface.Cancel()

                        'MH Request - Always confirm cancellation
                        '                If (m_lReturn& = PMDataChanged) Then
                        ' Get string messages
                        sTitle = (iPMFunc.GetResData(g_iLanguageID, ACCancelDetailsTitle, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                        sMessage = (iPMFunc.GetResData(g_iLanguageID, ACCancelDetails, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        If IsSilentQuote = False Then
                            iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                        Else
                            iMsgResult = vbYes
                        End If

                        ' Check message result.
                        If iMsgResult = System.Windows.Forms.DialogResult.No Then
                            ' Set return to false, meaning
                            ' don't cancel.
                            result = gPMConstants.PMEReturnCode.PMFalse
                        Else

                            ' RAW 16/09/2003 : CQ809 : added
                            If Not SubScreen Then
                                ' only do this for a top level screen
                                If m_sTransactionType <> "C_CR" And m_sTransactionType <> "C_CO" And m_sTransactionType <> "C_CP" And m_sTransactionType <> "NB" Then
                                    ' not claims or NB
                                    ' RAW 03/09/2004 : Resilience (#2) : replaced m_oInterface.RiskId with m_lOriginalRiskId
                                    If m_lRiskId <> m_lOriginalRiskId Then
                                        ' a copy of the original risk was taken and as we are cancelling must now be removed
                                        bRevertBackRisk = True  ' RAW110804 added

                                        ' RAW110804 removed call to m_oInterface.RevertBackRisk
                                        ' PW130804 - moved the update of riskid to happen
                                        ' after the call to SaveOnCancel

                                    End If
                                End If
                            End If
                            m_lReturn = m_oInterface.SaveOnCancel(Task, bRevertBackRisk)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            ' PW130804 - the risk id needs to be changed in
                            ' iPMUScreenControl AFTER the RevertBackRisk is
                            ' done (in SaveOnCancel), otherwise it will be trying
                            ' to revert back the wrong risk
                            If bRevertBackRisk Then
                                ' RAW 03/09/2004 : Resilience (#2) : replaced m_lRiskId with m_lOriginalRiskId

                                ReflectionHelper.SetMember(m_oInterface, "RiskId", m_lOriginalRiskId)
                                m_lRiskId = m_lOriginalRiskId  ' RAW 03/09/2004 : added
                            End If

                            '*****************
                            ' MEvans : 11-08-2003 : 223 Document Production
                            ' Clear down the output and document request objects
                            ' from within the xml - no database clear down required
                            ' as nothing will have been saved to the database at this
                            ' point
                            m_lReturn = ClearDownDocumentRequests()
                            '*****************
                        End If

                        '                End If
                    Else

                        ' Update the details using the business object.


                        m_lReturn = m_oInterface.Update(m_vScreenValues)

                        m_sDeclineReasons = ReflectionHelper.GetMember(m_oInterface, "DeclineReasons")

                        m_sReferReasons = ReflectionHelper.GetMember(m_oInterface, "ReferReasons")

                        m_sMessages = ReflectionHelper.GetMember(m_oInterface, "Messages")

                        m_sQuoteType = ReflectionHelper.GetMember(m_oInterface, "QuoteType")
                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                            ' RAW 20/09/2004 : CQ6832 : removed test for declined or referred
                            ' Failed to update the details
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Failed to update the details", ACApp, ACClass, "ProcessCommand")
                        Else
                            ' RAW 20/09/2004 : CQ6832 : added test for declined or referred
                            If (m_sDeclineReasons <> "") Or (m_sReferReasons <> "") Then
                                ' declined or referred
                            Else
                                '------------------------------------------------------------------------------
                                '   20/07/2002 RVH  BEGIN
                                '                   Call SAVE method on payment and reserve controls
                                '------------------------------------------------------------------------------
                                ' RAW110804 to be removed and moved to business object once resilience is applied to claims ????
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
                                    m_lReturn = UpdatePaymentAndReserve()
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            result = gPMConstants.PMEReturnCode.PMFalse

                                            ' Log Error.
                                            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Failed to update the Payment or Reserve details (UpdatePaymentAndReserve)", ACApp, ACClass, "ProcessCommand")
                                        End If
                                        '------------------------------------------------------------------------------
                                        '   20/07/2002 RVH  END
                                        '------------------------------------------------------------------------------
                                        m_lReturn = UpdateCase()
                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            result = gPMConstants.PMEReturnCode.PMFalse

                                            ' Log Error.
                                            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Failed to update the case details (UpdateCase)", ACApp, ACClass, "ProcessCommand")
                                        End If
                                    End If
                                Else
                                    ProcessCommand = gPMConstants.PMEReturnCode.PMCancel
                                End If
                            End If
                        End If
                    End If

                Case gPMConstants.PMEComponentAction.PMView

                    ' Check if form has been cancelled, if so,
                    ' check if the details have changed and if
                    ' so, prompt if they wish to cancel.
                    If Status = gPMConstants.PMEReturnCode.PMCancel Then

                        ' HG05082003 - Check if in claims mode
                        If m_sTransactionType = "C_CR" Or m_sTransactionType = "C_CO" Or m_sTransactionType = "C_CP" Then

                            ' Check how many claim output objects are present

                            If ReflectionHelper.Invoke(ReflectionHelper.GetMember(ReflectionHelper.GetMember(m_oInterface, "GIS"), "Risk"), "Count", New Object() {m_sGISDataModel & ACCLAIMOUTPUT}) > 0 Then
                                ' Check if any document reference objects are present

                                If ReflectionHelper.Invoke(ReflectionHelper.Invoke(ReflectionHelper.GetMember(ReflectionHelper.GetMember(m_oInterface, "GIS"), "Risk"), "Item", New Object() {m_sGISDataModel & ACCLAIMOUTPUT, 1}), "Count", New Object() {m_sGISDataModel & ACDOCREF}) > 0 Then

                                    ' Ask the user if they wish to exit since they have document references present
                                    ' Get string messages
                                    sTitle = (iPMFunc.GetResData(g_iLanguageID, ACCancelDetailsTitle, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                                    sMessage = (iPMFunc.GetResData(g_iLanguageID, ACCancelDetails, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                                    iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                                    ' Check message result.
                                    If iMsgResult = System.Windows.Forms.DialogResult.No Then
                                        ' Set return to false, meaning
                                        ' don't cancel.
                                        result = gPMConstants.PMEReturnCode.PMFalse
                                    Else
                                        '*****************
                                        ' MEvans : 11-08-2003 : 223 Document Production
                                        ' Clear down the output and document request objects
                                        ' from within the xml - no database clear down required
                                        ' as nothing will have been saved to the database at this
                                        ' point
                                        m_lReturn = ClearDownDocumentRequests()
                                        '*****************
                                    End If
                                End If
                            End If
                        End If
                    End If

            End Select

            'RVH 09/12/2004 - 1.9 Merge
            'if launched from Swift Black Box and not a sub-screen
            If m_lInsuranceFolderCnt = -1 And Not m_bSubScreen Then
                'get XML as string

                m_vXMLDataSet = ReflectionHelper.GetMember(m_oInterface, "XMLDataSet")
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer
        Dim result As Integer = 0
        Dim sCaption As String = ""

        Try


            ' {* USER DEFINED CODE (Begin) *}


            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetFirstLastControls
    '
    ' Description: Sets the first and last data entry controls for
    '              each tab to the control array, for use with the
    '              keyboard navigation.
    '
    ' ***************************************************************** '
    Private Function SetFirstLastControls() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            ReDim m_ctlTabFirstLast(1, 4)

            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to set the first and last data entry
            ' controls for all of the tabs.
            '
            ' Example:-
            '
            '    Set m_ctlTabFirstLast(ACControlStart, 0) = txtName
            '    Set m_ctlTabFirstLast(ACControlEnd, 0) = txtAge
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************
            '    Set m_ctlTabFirstLast(ACControlStart, 0) = txtIDReference
            '    Set m_ctlTabFirstLast(ACControlEnd, 0) = txtConsultantRef

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            TabStrip1.Top = 0

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the status of the Navigate button.

            m_lReturn = SetFirstLastControls()
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ValidateOK
    '
    ' Description: This validates mandatory address types and duplicate
    ' addresses
    '
    ' ***************************************************************** '
    Private Function ValidateOK() As Integer

        Dim result As Integer = 0
        On Error GoTo Err_ValidateOK

        result = gPMConstants.PMEReturnCode.PMTrue

        Dim ctlFormControlFrame, ctlSetFocus As Control
        Dim lTab As Integer
        Dim bTextContainCurlyBraces As Boolean
        Dim lForeColour, lDebugDepthCounter As Integer
        Dim i As Integer
        Dim arrControl As ArrayList = New ArrayList()
        Dim ctlChildControl As Control
        bTextContainCurlyBraces = True
#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then


        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : ValidateOK ")
#End If


        'CQ3041 Ensure DL Start is run when OK button pressed
        RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnChange, v_vDLAction:=enumOnChangeAction.eOnChangeAction_Accept, v_vTabNumber:=-1)

        ' RAW 22/06/2004 : Performance Changes(#2) : added
        If m_bScriptErrorDetected Then
            result = gPMConstants.PMEReturnCode.PMFalse
            GoTo GetOutOfHere
        End If


        If Not (m_oMSScriptControl Is Nothing) Or Not (m_cMandatoryProperties Is Nothing) Then
            lTab = 99  'set high tab number, set focus to control on lowest numbered tab
            On Error Resume Next
            For Each ctlFormControl As Control In Controls_Renamed

                If TypeOf (ctlFormControl) Is GroupBox Then
                    ControlList(ctlFormControl, arrControl)
                    For i = 0 To arrControl.Count - 1
                        ctlChildControl = arrControl(i)
                        'lForeColour = ColorTranslator.ToOle(ctlFormControl.BackColor)
                        lForeColour = ColorTranslator.ToOle(Color.FromArgb(ctlChildControl.BackColor.A, ctlChildControl.BackColor.R, ctlChildControl.BackColor.G, ctlChildControl.BackColor.B))
                        If lForeColour = m_lMandatoryColor Then
                            'If ctlFormControl.Visible = True Then
                            'ctlFormControl.container.
                            ctlFormControlFrame = ctlChildControl.Parent
                            'find the frame which owns this control
                            Do While Not ctlFormControlFrame.Name.Contains("fraFrame")
                                ctlFormControlFrame = ctlFormControlFrame.Parent
                            Loop
                            'from the frame array we can (now) get the tab number
                            'need to check if the tab is visible, if not can't enforce mandatory data
                            If IsTabVisible(TabStrip1, CInt(Convert.ToString(ControlHelper.GetTag(ctlFormControlFrame)))) And (lTab > CDbl(Convert.ToString(ControlHelper.GetTag(ctlFormControlFrame)))) Then
                                'and so set it current
                                lTab = Convert.ToInt32(ControlHelper.GetTag(ctlFormControlFrame))
                                ctlSetFocus = ctlChildControl
                            End If
                        End If
                        If TypeOf (ctlChildControl) Is TextBox Then
                            If ctlChildControl.Text.Contains("{") Or ctlChildControl.Text.Contains("}") Then
                                ctlSetFocus = ctlChildControl
                                bTextContainCurlyBraces = False
                            End If
                        End If


                    Next i
                Else
                    'lForeColour = ColorTranslator.ToOle(ctlFormControl.BackColor)
                    lForeColour = ColorTranslator.ToOle(Color.FromArgb(ctlFormControl.BackColor.A, ctlFormControl.BackColor.R, ctlFormControl.BackColor.G, ctlFormControl.BackColor.B))
                    If lForeColour = m_lMandatoryColor Then
                        'If ctlFormControl.Visible = True Then
                        'ctlFormControl.container.
                        ctlFormControlFrame = ctlFormControl.Parent
                        'find the frame which owns this control
                        Do While Not ctlFormControlFrame.Name.Contains("fraFrame")
                            ctlFormControlFrame = ctlFormControlFrame.Parent
                        Loop
                        'from the frame array we can (now) get the tab number
                        'need to check if the tab is visible, if not can't enforce mandatory data
                        If IsTabVisible(TabStrip1, CInt(Convert.ToString(ControlHelper.GetTag(ctlFormControlFrame)))) And (lTab > CDbl(Convert.ToString(ControlHelper.GetTag(ctlFormControlFrame)))) Then
                            'and so set it current
                            lTab = CInt(Convert.ToString(ControlHelper.GetTag(ctlFormControlFrame)))
                            ctlSetFocus = ctlFormControl
                        End If
                    End If
                End If
            Next ctlFormControl

            ' RVH 09/12/2004 - Moved from 1.8.6
            If Not (ctlSetFocus Is Nothing) Then
                'SelectTab(TabStrip1, lTab)
                'TabStrip1_Click(Nothing, Nothing)
                If Not bTextContainCurlyBraces Then
                    Interaction.MsgBox("{ or } is not allowed", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "Validation Failure")
                    ctlSetFocus.Focus()
                    result = gPMConstants.PMEReturnCode.PMFalse

                ElseIf bAllowSaveOfInvalidMandatoryData Then

                    If MessageBox.Show("One or more mandatory fields have not been completed." & Strings.Chr(13) & Strings.Chr(10) & "Press Cancel to re-edit or OK to save without validation.", "Validation Failure", MessageBoxButtons.OKCancel) = System.Windows.Forms.DialogResult.Cancel Then
                        ctlSetFocus.Focus()
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If
                Else
                    Interaction.MsgBox("Please check the mandatory field", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "Validation Failure")
                    ctlSetFocus.Focus()
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

        End If

        If m_sTransactionType = "C_CP" Then
            If m_bClaimPaymentAdded And result = gPMConstants.PMEReturnCode.PMTrue Then
                If uctCLMPayment.ValidPayment() <> gPMConstants.PMEReturnCode.PMTrue Then
                    SelectTab(TabStrip1, CInt(m_vFrameArray(ACFTabNumber, CInt(Convert.ToString(uctCLMPayment.Parent.Tag)))))
                    TabStrip1_Click(Nothing, Nothing)
                    'MsgBox "Invalid Payment Amount specified", vbExclamation, "Claim Payment Validation"
                    uctCLMPayment.Focus()
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If m_bCaseHeader And result = gPMConstants.PMEReturnCode.PMTrue Then
            If Not uctCLMCaseHeader1.ValidCase Then
                SelectTab(TabStrip1, CInt(m_vFrameArray(ACFTabNumber, CInt(Convert.ToString(uctCLMCaseHeader1.Parent.Tag)))))
                TabStrip1_Click(Nothing, Nothing)
                uctCLMCaseHeader1.Focus()
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        GoTo GetOutOfHere

Err_ValidateOK:

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateOK Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateOK", excep:=New Exception(Information.Err().Description))

        GoTo GetOutOfHere

GetOutOfHere:   ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

        Return result
    End Function


    ' ***************************************************************** '
    '
    ' Name: BuildScreen
    '
    ' Description:
    '
    ' History: 26/06/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function BuildScreen() As Integer

        Dim result As Integer = 0
        Dim lTag As Integer

        Dim sPropertyName As String = ""
        Dim sColumnName As String = ""
        Dim bMandatory As Boolean
        Dim vObjectID, vPropertyID As Object

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            If Not Information.IsArray(m_vScreenHeader) Then
                Return result
            End If

            m_sScreenDesc = CStr(m_vScreenHeader(ACHDescription, 0))
            g_sScreenDesc = m_sScreenDesc

            MyBase.Height = VB6.TwipsToPixelsY(IIf(m_vScreenHeader(ACHScreenHeight, 0) > 0, m_vScreenHeader(ACHScreenHeight, 0), 6000))
            MyBase.Width = VB6.TwipsToPixelsX(IIf(m_vScreenHeader(ACHScreenWidth, 0) > 0, m_vScreenHeader(ACHScreenWidth, 0), 9015))

            'MyBase.Height = (IIf(m_vScreenHeader(ACHScreenHeight, 0) > 0, m_vScreenHeader(ACHScreenHeight, 0), 6000))
            'MyBase.Width = (IIf(m_vScreenHeader(ACHScreenWidth, 0) > 0, m_vScreenHeader(ACHScreenWidth, 0), 9015))

            If Not Information.IsArray(m_vScreenDetails) Then
                Return result
            End If

            'Because of the way the data is written out, we can do this quite easily
            'First we've the tabs
            'Then the frames
            'Then the controls
            'Finally the specials

            'set mandatory color before adding code
            m_lMandatoryColor = ColorTranslator.ToOle(Color.FromArgb(255, 0, 0))

            For lTemp As Integer = m_vScreenDetails.GetLowerBound(1) To m_vScreenDetails.GetUpperBound(1)

                lTag = -1  ' RAW 09/07/2004 : JIT : added
                sPropertyName = ""  ' RAW 09/07/2004 : JIT : added

                ' store m_vDataDictionary offset here for JIT control loading
                m_vScreenDetails(ACDDefaultObjectId, lTemp) = Nothing  ' DBNull.Value
                m_vScreenDetails(ACDDefaultPropertyId, lTemp) = Nothing  ' DBNull.Value

                ' use this column as a flag to record when the control has been fully loaded - so initialise it now
                m_vScreenDetails(ACDScreenDetailCnt, lTemp) = Nothing  'DBNull.Value ' RAW 20/08/2004 : JIT : added

                If Not (Convert.IsDBNull(m_vScreenDetails(ACDGISObjectId, lTemp)) Or IsNothing(m_vScreenDetails(ACDGISObjectId, lTemp)) Or m_vScreenDetails(ACDGISObjectId, lTemp) = "") Then
                    vObjectID = m_vScreenDetails(ACDGISObjectId, lTemp)
                Else
                    vObjectID = 0
                End If

                If Not (Convert.IsDBNull(m_vScreenDetails(ACDGISPropertyId, lTemp)) Or IsNothing(m_vScreenDetails(ACDGISPropertyId, lTemp)) Or m_vScreenDetails(ACDGISPropertyId, lTemp) = "") Then
                    vPropertyID = m_vScreenDetails(ACDGISPropertyId, lTemp)
                Else
                    vPropertyID = 0
                End If

                ' Get the tag for the object
                If CInt(vObjectID) > 0 Then
                    m_lReturn = GetTag(CInt(vObjectID), -1, lTag, m_vDataDictionary(GISDMTypeRisk), MainModule.ACOGISObjectId)

                    If lTag > -1 Then
                        ' Fixed to the one datamodel type only...future expansion could allow multi-datamodels per screen
                        lTag = GISDMTypeRisk * 10000 + lTag
                        m_vScreenDetails(ACDDefaultObjectId, lTemp) = CStr(lTag)  'store m_vDataDictionary offset here!
                    End If
                End If

                ' Get the tag for the property
                If CInt(vPropertyID) > 0 Then
                    m_lReturn = GetTag(CInt(vObjectID), CInt(vPropertyID), lTag, (m_vDataDictionary(GISDMTypeRisk)), 0, ACPGISPropertyId:=MainModule.ACPGISPropertyId)

                    If lTag > -1 Then
                        ' Fixed to the one datamodel type only...future expansion could allow multi-datamodels per screen
                        lTag = GISDMTypeRisk * 10000 + lTag
                        '                lTag = m_vScreenDetails(ACDDataModelType, lTemp) * 10000 + lTag
                        'm_vScreenDetails(ACDDefaultPropertyId, lTemp) = lTag 'store m_vDataDictionary offset here!
                        m_vScreenDetails(ACDDefaultPropertyId, lTemp) = CStr(lTag)  'store m_vDataDictionary offset here!
                    End If
                End If

                ' Now create the control
                ' unless we have to, delay creating controls until the appropriate tab is activated
                ' or control is referenced from DL
                ' ********************************************************************************************                
                Select Case ControlType(lTemp)
                    Case enumControlType.eControlType_Tab
                        '=================================
                        ' add the tab to the form
                        ' RAW 09/07/2004 : JIT : replaced embedded code with a call to AddTab
                        addTabControl(m_lTabIndex, m_vTabArray, m_vScreenDetails, lTemp, TabStrip1)

                        'm_cObjectAndAttribute.Add(New Object() {TabStrip1, m_lTabIndex + 1, TabGetCaption(TabStrip1, m_lTabIndex), lTemp}, ("Tab." & m_lTabIndex + 1).ToLower())
                        m_cObjectAndAttribute.Add(New Object() {TabStrip1, m_lTabIndex, TabGetCaption(TabStrip1, m_lTabIndex), lTemp}, ("Tab." & m_lTabIndex).ToLower())

                        ' now flag this in the array as having been created
                        m_vScreenDetails(ACDScreenDetailCnt, lTemp) = CStr(lTemp)  ' RAW 20/08/2004 : JIT : added

                    Case enumControlType.eControlType_Frame, enumControlType.eControlType_ListView
                        '=================================
                        ' We have to create all frames now because ACDParentId represents a zero-based sequential
                        ' numbered index to the Frame control array so they have to be created in this order
                        ' so that other controls can link to them

                        If CBool(m_vScreenDetails(ACDGISObjectId, lTemp)) Then

                            lTag = CInt(CDbl(m_vScreenDetails(ACDDefaultObjectId, lTemp)) Mod 10000)
                            ' store a reference in the DataDictionary for this object to this ScreenDetails array entry
                            m_vDataDictionary(GISDMTypeRisk)(ACPIsIdentifyingProperty, lTag Mod 10000) = lTemp
                        End If

                        m_lReturn = AddFrame(m_lFrameIndex, lTag, VB6.TwipsToPixelsX(CDbl(m_vScreenDetails(ACDLeft, lTemp))), VB6.TwipsToPixelsY(CDbl(m_vScreenDetails(ACDTop, lTemp))), 999, lTemp)
                        result = m_lReturn

                    Case enumControlType.eControlType_TextBox, enumControlType.eControlType_CheckBox, enumControlType.eControlType_GeminiCombo, enumControlType.eControlType_PMLookup, enumControlType.eControlType_GISLookup, enumControlType.eControlType_Accumulation, enumControlType.eControlType_Address, enumControlType.eControlType_Party, enumControlType.eControlType_Policy, enumControlType.eControlType_SumInsured, enumControlType.eControlType_StandardWording, enumControlType.eControlType_ClaimPayment, enumControlType.eControlType_ClaimReserve
                        '=================================
                        If Not (Convert.IsDBNull(CDbl(m_vScreenDetails(ACDDefaultPropertyId, lTemp)) Mod 10000) Or IsNothing(CDbl(m_vScreenDetails(ACDDefaultPropertyId, lTemp)) Mod 10000)) Then

                            lTag = CInt(CDbl(m_vScreenDetails(ACDDefaultPropertyId, lTemp)) Mod 10000)
                            ' store a reference in the DataDictionary for this property to this ScreenDetails array entry

                            'm_vDataDictionary(GISDMTypeRisk)(ACPIsIdentifyingProperty, lTag Mod 10000) = lTemp
                            m_vDataDictionary(GISDMTypeRisk)(MainModule.ACPIsIdentifyingProperty, lTag Mod 10000) = lTemp

                            sPropertyName = (m_vDataDictionary(GISDMTypeRisk)(ACPPropertyName, lTag Mod 10000))
                        End If

                    Case enumControlType.eControlType_ClaimPeril
                        '=================================
                        If CBool(m_vScreenDetails(ACDGISObjectId, lTemp)) Then

                            lTag = CInt(CDbl(m_vScreenDetails(ACDDefaultObjectId, lTemp)) Mod 10000)
                            ' store a reference in the DataDictionary for this object to this ScreenDetails array entry
                            m_vDataDictionary(GISDMTypeRisk)(ACPIsIdentifyingProperty, lTag Mod 10000) = lTemp
                        End If

                        m_lReturn = AddClaimPeril(CInt(m_vScreenDetails(ACDParentId, lTemp)), lTag, CInt(m_vScreenDetails(ACDLeft, lTemp)), CInt(m_vScreenDetails(ACDTop, lTemp)), False, sPropertyName, m_vScreenDetails(ACDTabSetIndex, lTemp), lTemp)
                        result = m_lReturn

                    Case enumControlType.eControlType_Find
                        m_lReturn = AddFindControl(CInt(m_vScreenDetails(ACDParentId, lTemp)), lTag, CInt(m_vScreenDetails(ACDLeft, lTemp)), CInt(m_vScreenDetails(ACDTop, lTemp)), sPropertyName, m_vScreenDetails(ACDPMFormat, lTemp), m_vScreenDetails(ACDTabSetIndex, lTemp), lTemp)

                    Case enumControlType.eControlType_CaseHeader
                        ' store a reference in the DataDictionary for this object to this ScreenDetails array entry
                        m_vDataDictionary(GISDMTypeRisk)(ACPIsIdentifyingProperty, lTag Mod 10000) = lTemp

                        m_lReturn = AddCaseHeader(CInt(m_vScreenDetails(ACDParentId, lTemp)), lTag, CInt(m_vScreenDetails(ACDLeft, lTemp)), CInt(m_vScreenDetails(ACDTop, lTemp)), False, sPropertyName, m_vScreenDetails(ACDTabSetIndex, lTemp), lTemp)
                        result = m_lReturn

                    Case enumControlType.eControlType_CaseClaimList

                        ' store a reference in the DataDictionary for this object to this ScreenDetails array entry
                        m_vDataDictionary(GISDMTypeRisk)(ACPIsIdentifyingProperty, lTag Mod 10000) = lTemp

                        m_lReturn = AddCaseClaimList(CInt(m_vScreenDetails(ACDParentId, lTemp)), lTag, CInt(m_vScreenDetails(ACDLeft, lTemp)), CInt(m_vScreenDetails(ACDTop, lTemp)), False, sPropertyName, m_vScreenDetails(ACDTabSetIndex, lTemp), lTemp)
                        result = m_lReturn

                    Case Else

                End Select

                ' is this control mandatory
                If lTag > 0 And sPropertyName <> "" Then

                    'Apply Mandatoryness set at GIS level.                    
                    bMandatory = CBool(m_vDataDictionary(GISDMTypeRisk)(ACPEditFlags, lTag Mod 10000) And GISDSEditMandatory)

                    If bMandatory Then
                        ' RAW 09/07/2003 : CQ221 : added key to collection entry
                        m_cMandatoryProperties.Add((m_vDataDictionary(GISDMTypeRisk)(ACOObjectName, lTag Mod 10000)) & "." & sPropertyName, ((m_vDataDictionary(GISDMTypeRisk)(ACOObjectName, lTag Mod 10000)) & "." & sPropertyName).ToUpper())
                    End If
                End If

            Next lTemp

            '    RAW -  code moved to initialise
            '    If m_cObjectAndAttribute Is Nothing Then
            '        Set m_cObjectAndAttribute = New Collection 'might be needed outside of DL
            '    End If

            If m_vScreenHeader(ACHScriptDynamicLogic, 0) <> "" Then
                m_lReturn = LoadDLEngine()
                result = m_lReturn
            End If
            If TabStrip1.TabCount > 0 Then
                SSTabHelper.SetTabEnabled(TabStrip1, 0, True)
            End If
            'sw.Stop()
            'iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Risk screen build screen took " + (sw.ElapsedMilliseconds / 1000).ToString, ACApp, ACClass, "BuildScreen")
            Return result
        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "BuildScreen Failed", ACApp, ACClass, "BuildScreen", Information.Err().Number, Information.Err().Description, excep:=ex)
            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: LoadDLEngine
    '
    ' Description:
    '
    ' ***************************************************************** '
	<HandleProcessCorruptedStateExceptions>
    Private Function LoadDLEngine() As Integer

        Dim result As Integer = 0
        Dim sStr As String = ""

        Dim Err_No As Integer
        Dim Err_Line As Integer
        Dim Err_Col As Integer
        Dim Err_Description As String
        Dim Err_Text As String
        Dim extractFunctionRegex As String = "(?i)^\s*(Function|Sub)\s+(\w+)"
        Dim procedureName As String = String.Empty
        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            m_oMSScriptControl = New MSScriptControl.ScriptControl()
            m_oMSScriptControl.Language = "VBScript"
            ' RAW 18/08/2003 : CQ2021 : force any msgboxes displayed by the script
            ' to be modal to this control's parent form

            m_oMSScriptControl.SitehWnd = MyBase.FindForm().Handle.ToInt32()

            sStr = ""
            sStr = sStr & "Option Explicit" & Strings.Chr(13) & Strings.Chr(10)

            'add a SetMandatoryColor method that doesn't need .Engine for legacy schemes code
            sStr = sStr & "Function SetMandatoryColor(iRed, iGreen , iBlue )" & Strings.Chr(13) & Strings.Chr(10)
            sStr = sStr & "    SetMandatoryColor=Engine.SetMandatoryColor(cint(iRed),cint(iGreen),cint(iBlue))" & Strings.Chr(13) & Strings.Chr(10)
            sStr = sStr & "End Function" & Strings.Chr(13) & Strings.Chr(10)

            sStr = sStr & CStr(m_vScreenHeader(ACHScriptDynamicLogic, 0))

            m_oMSScriptControl.AddObject("Engine", m_oDLEngine, False)
            m_oMSScriptControl.AddCode(sStr)  ' This will run any module level code (ie not in a function)

            ' If there is a START function in DL then we will use the old DL event strategy
            ' If not then we will use the new way
            m_bFireDLStartEvents = False

            Dim procedureMatches = Regex.Matches(sStr, extractFunctionRegex, RegexOptions.Multiline)
            'Dim methodNames As New List(Of String)
            For Each vProcedureName As Match In procedureMatches
                    'methodNames.Add(vProcedureName.Groups(2).Value) 
                    procedureName = vProcedureName.Groups(2).Value
                    Select Case procedureName.ToLower()
                        Case "initialise"
                            m_bDLFunction_Initialise_valid = True
                        Case "start"
                            m_bDLFunction_Start_valid = True
                            ' use the old DL event strategy
                            m_bFireDLStartEvents = True
                        Case "onchange"
                            m_bDLFunction_onChange_valid = True
                        Case "onfocus"
                            m_bDLFunction_onFocus_valid = True
                        Case "onlostfocus"
                            m_bDLFunction_onLostFocus_valid = True
                        Case Else
                        End Select
            Next

            Return result

        Catch ex As Exception

            Err_No = m_oMSScriptControl.Error.Number
            Err_Line = m_oMSScriptControl.Error.Line
            Err_Col = m_oMSScriptControl.Error.Column
            Err_Description = m_oMSScriptControl.Error.Description
            Err_Text = m_oMSScriptControl.Error.Text

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Dynamic Logic AddCode Failed. Error in Dynamic Logic Rule." & vbCrLf & "Error No     : " & Err_No & vbCrLf & "Error Desc   : " & Err_Description & vbCrLf & "Error Line   : " & Err_Line & vbCrLf & "Error Column : " & Err_Col & vbCrLf & "Error Text   : " & Err_Text & vbCrLf, ACApp, ACClass, "LoadDLEngine", Information.Err().Number, Information.Err().Description, excep:=ex)
            Return result

        End Try

    End Function


    ' ***************************************************************** '
    '
    ' Name: AddAccumulation
    '
    ' Description:
    '
    ' History: 26/06/2000 Tomo - Created.
    ' RAW 09/07/2004 : JIT : added v_lScreenDetailsIndex param and used instead of m_cControlCount
    ' ***************************************************************** '
    Private Function AddAccumulation(ByRef iFrameIndex As Integer, ByRef lTag As Integer, ByRef lX As Single, ByRef lY As Single, ByRef sPropertyName As String, ByRef vPMFormat As Object, ByRef vTabSetIndex As Object, ByVal v_lScreenDetailsIndex As Integer) As Integer

        Dim result As Integer = 0
        Dim lDebugDepthCounter As Integer
        Dim sCaption As String = ""
        Dim vArray As Object

        Try

            ' RAW 09/07/2004 : JIT : added
#If (DebugOption And m_klDebugOption_JIT) = m_klDebugOption_JIT Then


        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="JIT : Adding Accumulation - " & ObjectAndPropertyName(lTag))

#End If

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = AddFrame(iFrameIndex, lTag, lX, lY, 0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                Return result
            End If


            ' RAW 09/07/2004 : JIT : added

            m_cObjectAndAttribute.Add(New Object() {cboAccumulation, lblAccumulation, "", v_lScreenDetailsIndex}, ObjectAndPropertyName(lTag).ToLower())

            m_oNewControlBeingLoaded = cboAccumulation  ' RAW 20/08/2004 : JIT : added


            'OK, so here it's an Accumulation combo, and a label
            ReDim m_vAccumulationArray(ACCLastArrayPosition, 0)

            m_vAccumulationArray(ACCFrameNumber, 0) = iFrameIndex
            m_vAccumulationArray(ACCIsDeleted, 0) = gPMConstants.PMEReturnCode.PMFalse
            m_vAccumulationArray(ACCHelpText, 0) = m_sHelpText
            m_vAccumulationArray(ACCPreQuote, 0) = m_lPreQuote
            m_vAccumulationArray(ACCPostQuote, 0) = m_lPostQuote
            m_vAccumulationArray(ACCPurchase, 0) = m_lPurchase
            m_vAccumulationArray(ACCIsValuation, 0) = m_lIsValuation
            m_vAccumulationArray(ACCIsRateAndPremium, 0) = m_lIsRateAndPremium
            m_vAccumulationArray(ACCIncludeInList, 0) = m_lIncludeInList

            m_vAccumulationArray(ACCChildId, 0) = DBNull.Value

            m_vAccumulationArray(ACCGISObjectId, 0) = DBNull.Value
            m_vAccumulationArray(ACCPMFormat, 0) = vPMFormat
            m_vAccumulationArray(ACCTabSetIndex, 0) = vTabSetIndex


            cboAccumulation.Parent = fraFrame(iFrameIndex)
            lblAccumulation.Parent = fraFrame(iFrameIndex)

            lblAccumulation.Top = VB6.TwipsToPixelsY(lY)
            lblAccumulation.Left = VB6.TwipsToPixelsX(lX)
            lblAccumulation.Visible = True
            lblAccumulation.Text = sPropertyName
            lblAccumulation.Tag = CStr(v_lScreenDetailsIndex)

            lblAccumulation.ForeColor = SystemColors.ControlText

            If m_lPreQuote = 2 Then
                lblAccumulation.ForeColor = SystemColors.Highlight
            ElseIf (m_lPostQuote = 2) Then
                lblAccumulation.ForeColor = SystemColors.HighlightText
            ElseIf (m_lPurchase = 2) Then
                lblAccumulation.ForeColor = SystemColors.Info
            End If

            '    cboAccumulation.Top = lblAccumulation.Top - 45
            '    cboAccumulation.Left = lblAccumulation.Left + lblAccumulation.Width + 60
            cboAccumulation.Width = VB6.TwipsToPixelsX(m_lWidth)
            cboAccumulation.Visible = True
            cboAccumulation.Tag = CStr(lTag)
            cboAccumulation.ToolTipText = m_sHelpText
            cboAccumulation.AccumulationLevel = m_vRiskTypeDetails(ACRAccumulationLevel, 0)

            '    cboAccumulation.RefreshList

            'this call positions everything correctly
            textLabel_MouseMove("", MouseButtonConstants.LeftButton, g_lx, g_ly, cboAccumulation, lblAccumulation, pnlPosition, m_vTabArray(ACFTabSnapToGrid, CInt(m_vFrameArray(ACFTabNumber, iFrameIndex))))

            cboAccumulation.Enabled = False

            If Task <> gPMConstants.PMEComponentAction.PMView Then
                If (m_vDataDictionary(GISDMTypeRisk)(ACPIsInputProperty, lTag Mod 10000)) = 1 Then
                    cboAccumulation.Enabled = True
                End If
            End If

            vArray = m_vScreenValues(v_lScreenDetailsIndex)

            If CStr(vArray(0)) <> "" Then
                cboAccumulation.DefaultItemId = CInt(vArray(0))
            End If

            cboAccumulation.RefreshList()


            ' now flag this in the array as having been created
            m_vScreenDetails(ACDScreenDetailCnt, v_lScreenDetailsIndex) = v_lScreenDetailsIndex  ' RAW 20/08/2004 : JIT : added
            m_oNewControlBeingLoaded = Nothing  ' RAW 20/08/2004 : JIT : added



        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "AddAccumulation Failed", ACApp, ACClass, "AddAccumulation", Information.Err().Number, Information.Err().Description, excep:=ex)


        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddAddress
    '
    ' Description:
    '
    ' History: 26/06/2000 Tomo - Created.
    ' RAW 09/07/2004 : JIT : added v_lScreenDetailsIndex param and used instead of m_cControlCount
    ' ***************************************************************** '
    Private Function AddAddress(ByRef iFrameIndex As Integer, ByRef lTag As Integer, ByRef lX As Single, ByRef lY As Single, ByRef sColumnName As String, ByRef sPropertyName As String, ByRef bAddFrame As Boolean, ByRef vPMFormat As Object, ByRef vTabSetIndex As Object, ByVal v_lScreenDetailsIndex As Integer) As Integer

        Dim result As Integer = 0
        Dim lDebugDepthCounter As Integer
        Dim vArray(,) As Object
        Dim lControlIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If bAddFrame Then
                m_lReturn = AddFrame(iFrameIndex, lTag, lX, lY, 10000)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn

                    Return result
                End If
            End If

            ' RAW 09/07/2004 : JIT : added
            If cmdAddress.Length = 1 Then
                ' This is the first control of this type to be created so start at index 1
                ' - index 0 remains in its original state as a template for later controls of this type
                m_lAddressIndex = 1
                ReDim m_vAddressArray(ACCLastArrayPosition, m_lAddressIndex)
            Else

                m_lAddressIndex = cmdAddress.Length
                ReDim Preserve m_vAddressArray(ACCLastArrayPosition, m_lAddressIndex)
            End If

            ' RAW 09/07/2004 : JIT : added
            ' store this index locally.
            ' Do not refer to the module level variable again from within this function as it may change
            ' if DL is triggered resulting in other controls being created, and thus the index being incremented,
            ' before this function has completed
            lControlIndex = m_lAddressIndex

            If lControlIndex > 0 Then
                ' RAW 09/07/2004 : JIT : added
#If (DebugOption And m_klDebugOption_JIT) = m_klDebugOption_JIT Then


            AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="JIT : Adding Address (" & lControlIndex & ") - " & ObjectAndPropertyName(lTag))
#End If
                AddControl("txtAddress1", lControlIndex)
                AddControl("txtAddress2", lControlIndex)
                AddControl("txtAddress3", lControlIndex)
                AddControl("txtAddress4", lControlIndex)
                AddControl("txtAddress5", lControlIndex)
                AddControl("txtAddress6", lControlIndex)
                AddControl("cmdAddress", lControlIndex)

            End If

            ' RAW 09/07/2004 : JIT : added
            ' store reference to this control in a collection - keyed on object and property name

            m_cObjectAndAttribute.Add(New Object() {fraFrame(iFrameIndex), cmdAddress(lControlIndex), "ADDRESS_CNT", m_lAddressIndex}, ObjectAndPropertyName(lTag).ToLower())  'PN24885

            m_oNewControlBeingLoaded = cmdAddress(lControlIndex)  ' RAW 20/08/2004 : JIT : added


            m_vAddressArray(ACCFrameNumber, lControlIndex) = iFrameIndex
            m_vAddressArray(ACCIsDeleted, lControlIndex) = gPMConstants.PMEReturnCode.PMFalse
            m_vAddressArray(ACCHelpText, lControlIndex) = m_sHelpText
            m_vAddressArray(ACCPreQuote, lControlIndex) = m_lPreQuote
            m_vAddressArray(ACCPostQuote, lControlIndex) = m_lPostQuote
            m_vAddressArray(ACCPurchase, lControlIndex) = m_lPurchase
            m_vAddressArray(ACCIsValuation, lControlIndex) = m_lIsValuation
            m_vAddressArray(ACCIsRateAndPremium, lControlIndex) = m_lIsRateAndPremium
            m_vAddressArray(ACCIncludeInList, lControlIndex) = m_lIncludeInList

            m_vAddressArray(ACCChildId, lControlIndex) = DBNull.Value

            m_vAddressArray(ACCGISObjectId, lControlIndex) = DBNull.Value
            m_vAddressArray(ACCPMFormat, lControlIndex) = vPMFormat
            m_vAddressArray(ACCTabSetIndex, lControlIndex) = (vTabSetIndex)

            txtAddress1(lControlIndex).Parent = fraFrame(iFrameIndex)
            txtAddress2(lControlIndex).Parent = fraFrame(iFrameIndex)
            txtAddress3(lControlIndex).Parent = fraFrame(iFrameIndex)
            txtAddress4(lControlIndex).Parent = fraFrame(iFrameIndex)
            txtAddress5(lControlIndex).Parent = fraFrame(iFrameIndex)
            txtAddress6(lControlIndex).Parent = fraFrame(iFrameIndex)
            cmdAddress(lControlIndex).Parent = fraFrame(iFrameIndex)

            ' make all text as read only 
            txtAddress1(lControlIndex).ReadOnly = True
            txtAddress2(lControlIndex).ReadOnly = True
            txtAddress3(lControlIndex).ReadOnly = True
            txtAddress4(lControlIndex).ReadOnly = True
            txtAddress5(lControlIndex).ReadOnly = True
            txtAddress6(lControlIndex).ReadOnly = True

            'DJM 10/10/2002 : Only allow 255 characters as that is what SQL Server is set up to accept.
            txtAddress1(lControlIndex).MaxLength = 255
            txtAddress2(lControlIndex).MaxLength = 255
            txtAddress3(lControlIndex).MaxLength = 255
            txtAddress4(lControlIndex).MaxLength = 255
            txtAddress5(lControlIndex).MaxLength = 255
            txtAddress6(lControlIndex).MaxLength = 255

            txtAddress1(lControlIndex).Left = VB6.TwipsToPixelsX(240)
            txtAddress2(lControlIndex).Left = txtAddress1(lControlIndex).Left
            txtAddress3(lControlIndex).Left = txtAddress1(lControlIndex).Left
            txtAddress4(lControlIndex).Left = txtAddress1(lControlIndex).Left
            txtAddress5(lControlIndex).Left = txtAddress1(lControlIndex).Left
            txtAddress6(lControlIndex).Left = txtAddress1(lControlIndex).Left

            txtAddress1(lControlIndex).Top = VB6.TwipsToPixelsY(240)
            txtAddress2(lControlIndex).Top = txtAddress1(lControlIndex).Top + VB6.TwipsToPixelsY(360)
            txtAddress3(lControlIndex).Top = txtAddress2(lControlIndex).Top + VB6.TwipsToPixelsY(360)
            txtAddress4(lControlIndex).Top = txtAddress3(lControlIndex).Top + VB6.TwipsToPixelsY(360)
            txtAddress5(lControlIndex).Top = txtAddress4(lControlIndex).Top + VB6.TwipsToPixelsY(360)
            txtAddress6(lControlIndex).Top = txtAddress5(lControlIndex).Top + VB6.TwipsToPixelsY(360)
            cmdAddress(lControlIndex).Top = txtAddress6(lControlIndex).Top

            txtAddress1(lControlIndex).Width = fraFrame(iFrameIndex).Width - VB6.TwipsToPixelsX(480)
            txtAddress2(lControlIndex).Width = txtAddress1(lControlIndex).Width
            txtAddress3(lControlIndex).Width = txtAddress1(lControlIndex).Width
            txtAddress4(lControlIndex).Width = txtAddress1(lControlIndex).Width

            cmdAddress(lControlIndex).Left = txtAddress1(lControlIndex).Left + txtAddress1(lControlIndex).Width - cmdAddress(lControlIndex).Width

            txtAddress1(lControlIndex).Visible = True
            txtAddress2(lControlIndex).Visible = True
            txtAddress3(lControlIndex).Visible = True
            txtAddress4(lControlIndex).Visible = True
            txtAddress5(lControlIndex).Visible = True
            txtAddress6(lControlIndex).Visible = True
            cmdAddress(lControlIndex).Visible = True
            cmdAddress(lControlIndex).Tag = CStr(lTag)

            vArray = m_vScreenValues(v_lScreenDetailsIndex)

            txtAddress1(lControlIndex).Tag = CStr(v_lScreenDetailsIndex)

            If Information.IsArray(vArray) Then
                '        txtAddress1(lControlIndex).Tag = vArray(0, 0)
                txtAddress1(lControlIndex).Text = CStr(vArray(1, 0))
                txtAddress2(lControlIndex).Text = CStr(vArray(2, 0))
                txtAddress3(lControlIndex).Text = CStr(vArray(3, 0))
                txtAddress4(lControlIndex).Text = CStr(vArray(4, 0))
                txtAddress5(lControlIndex).Text = CStr(vArray(5, 0))
                txtAddress6(lControlIndex).Text = CStr(vArray(6, 0))
            End If

            cmdAddress(lControlIndex).Enabled = False

            If Task <> gPMConstants.PMEComponentAction.PMView Then
                If (m_vDataDictionary(GISDMTypeRisk)(ACPIsInputProperty, lTag Mod 10000)) = 1 Then
                    cmdAddress(lControlIndex).Enabled = True
                End If
            End If


            ' now flag this in the array as having been created
            m_vScreenDetails(ACDScreenDetailCnt, v_lScreenDetailsIndex) = v_lScreenDetailsIndex  ' RAW 20/08/2004 : JIT : added
            m_oNewControlBeingLoaded = Nothing  ' RAW 20/08/2004 : JIT : added


            vArray = Nothing


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "AddAddress Failed", ACApp, ACClass, "AddAddress", Information.Err().Number, Information.Err().Description, excep:=ex)


        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If


        End Try
        Return result
    End Function
    Private Sub AddControl(ByVal cControl As String, ByVal lIndex As Integer)

        Dim lControlIndex As Integer
        Dim sDirectoryPath As String = My.Application.Info.DirectoryPath
        lControlIndex = lIndex

        Select Case cControl
            Case "fraFrame"
                ReDim Preserve fraFrame(lControlIndex)
                fraFrame(lControlIndex) = New GroupBox
                fraFrame(lControlIndex).Name = "_fraFrame_" & CStr(lControlIndex)
                Me.Controls.Add(fraFrame(lControlIndex))
            Case "txtText"
                ReDim Preserve txtText(lControlIndex)
                txtText(lControlIndex) = New TextBox
                txtText(lControlIndex).Name = "_txtText_" & CStr(lControlIndex)
                Me.Controls.Add(txtText(lControlIndex))
                AddHandler txtText(lControlIndex).Enter, AddressOf txtText_Enter
                AddHandler txtText(lControlIndex).KeyPress, AddressOf txtText_KeyPress
                AddHandler txtText(lControlIndex).Validating, AddressOf txtText_Validating
            Case "txtMlText"
                ReDim Preserve txtMlText(lControlIndex)
                txtMlText(lControlIndex) = New TextBox
                txtMlText(lControlIndex).Name = "_txtMlText_" & CStr(lControlIndex)
                txtMlText(lControlIndex).Multiline = True
                txtMlText(lControlIndex).ScrollBars = ScrollBars.Vertical
                Me.Controls.Add(txtMlText(lControlIndex))
                AddHandler txtMlText(lControlIndex).Enter, AddressOf txtMlText_Enter
                AddHandler txtMlText(lControlIndex).Validating, AddressOf txtMlText_Validating
            Case "txtFormattedText"
                ReDim Preserve txtFormattedText(lControlIndex)
                txtFormattedText(lControlIndex) = New uctSIRRTFControl.uctRichTextBox
                txtFormattedText(lControlIndex).Name = "_txtFormattedText_" & CStr(lControlIndex)
                txtFormattedText(lControlIndex).SpellCheck = True
                Me.Controls.Add(txtFormattedText(lControlIndex))
                AddHandler txtFormattedText(lControlIndex).Enter, AddressOf txtFormattedText_Enter
                AddHandler txtFormattedText(lControlIndex).Validating, AddressOf txtFormattedText_Validating
            Case "chkYesNo"
                ReDim Preserve chkYesNo(lControlIndex)
                chkYesNo(lControlIndex) = New CheckBox
                chkYesNo(lControlIndex).Name = "_chkYesNo_" & CStr(lControlIndex)
                Me.Controls.Add(chkYesNo(lControlIndex))
                AddHandler chkYesNo(lControlIndex).CheckStateChanged, AddressOf chkYesNo_CheckStateChanged
                AddHandler chkYesNo(lControlIndex).MouseDown, AddressOf chkYesNo_MouseDown
                AddHandler chkYesNo(lControlIndex).KeyPress, AddressOf chkYesNo_KeyPress
                AddHandler chkYesNo(lControlIndex).Validating, AddressOf chkYesNo_Validating
                AddHandler chkYesNo(lControlIndex).Enter, AddressOf chkYesNo_Enter
            Case "lblText"
                ReDim Preserve lblText(lControlIndex)
                lblText(lControlIndex) = New Label
                lblText(lControlIndex).Name = "_lblText_" & CStr(lControlIndex)
                lblText(lControlIndex).Text = "Text Label"
                lblText(lControlIndex).AutoSize = True
                Me.Controls.Add(lblText(lControlIndex))
                AddHandler lblText(lControlIndex).Click, AddressOf lnk_Clicked
            Case "lblMlText"
                ReDim Preserve lblMlText(lControlIndex)
                lblMlText(lControlIndex) = New Label
                lblMlText(lControlIndex).Name = "_lblMlText_" & CStr(lControlIndex)
                lblMlText(lControlIndex).Text = "ML Text Label"
                Me.Controls.Add(lblMlText(lControlIndex))
            Case "lblFormattedText"
                ReDim Preserve lblFormattedText(lControlIndex)
                lblFormattedText(lControlIndex) = New Label
                lblFormattedText(lControlIndex).Name = "_lblFormattedText_" & CStr(lControlIndex)
                lblFormattedText(lControlIndex).Text = "Formatted Text Label"
                lblFormattedText(lControlIndex).AutoSize = True
                Me.Controls.Add(lblFormattedText(lControlIndex))
            Case "lblCheckLabel"
                ReDim Preserve lblCheckLabel(lControlIndex)
                lblCheckLabel(lControlIndex) = New Label
                lblCheckLabel(lControlIndex).Name = "_lblCheckLabel_" & CStr(lControlIndex)
                lblCheckLabel(lControlIndex).Text = "Check Label"
                Me.Controls.Add(lblCheckLabel(lControlIndex))
            Case "lblPMLookup"
                ReDim Preserve lblPMLookup(lControlIndex)
                lblPMLookup(lControlIndex) = New Label
                lblPMLookup(lControlIndex).Name = "_lblPMLookup_" & CStr(lControlIndex)
                Me.Controls.Add(lblPMLookup(lControlIndex))
            Case "cboPMLookup"
                ReDim Preserve cboPMLookup(lControlIndex)
                cboPMLookup(lControlIndex) = New PMLookupControl.cboPMLookup
                cboPMLookup(lControlIndex).Name = "_cboPMLookup_" & CStr(lControlIndex)
                Me.Controls.Add(cboPMLookup(lControlIndex))
                AddHandler cboPMLookup(lControlIndex).Click, AddressOf cboPMLookup_Click
                AddHandler cboPMLookup(lControlIndex).GotFocus, AddressOf cboPMLookup_GotFocus
                AddHandler cboPMLookup(lControlIndex).Validating, AddressOf cboPMLookup_Validating
                AddHandler cboPMLookup(lControlIndex).Enter, AddressOf cboPMLookup_GotFocus
            Case "cboGISLookup"
                ReDim Preserve cboGISLookup(lControlIndex)
                cboGISLookup(lControlIndex) = New uctGISUserDefLookupControl.cboGISLookup
                cboGISLookup(lControlIndex).Name = "_cboGISLookup_" & CStr(lControlIndex)
                Me.Controls.Add(cboGISLookup(lControlIndex))
                AddHandler cboGISLookup(lControlIndex).GotFocus, AddressOf cboGISLookup_GotFocus
                AddHandler cboGISLookup(lControlIndex).Click, AddressOf cboGISLookup_Click
                AddHandler cboGISLookup(lControlIndex).Validating, AddressOf cboGISLookup_Validating
                AddHandler cboGISLookup(lControlIndex).SelectedIndexChanged, AddressOf cboGISLookup_SelectedIndexChanged
                'AddHandler cboGISLookup(lControlIndex).LostFocus, AddressOf cboGISLookup_LostFocus
                AddHandler cboGISLookup(lControlIndex).Enter, AddressOf cboGISLookup_GotFocus
            Case "lblGISLookup"
                ReDim Preserve lblGISLookup(lControlIndex)
                lblGISLookup(lControlIndex) = New Label
                lblGISLookup(lControlIndex).Name = "_lblGISLookup_" & CStr(lControlIndex)
                lblGISLookup(lControlIndex).Text = "GIS Lookup"
                Me.Controls.Add(lblGISLookup(lControlIndex))
            Case "txtAddress1"
                ReDim Preserve txtAddress1(lControlIndex)
                txtAddress1(lControlIndex) = New TextBox
                txtAddress1(lControlIndex).Name = "_txtAddress1_" & CStr(lControlIndex)
                Me.Controls.Add(txtAddress1(lControlIndex))
            Case "txtAddress2"
                ReDim Preserve txtAddress2(lControlIndex)
                txtAddress2(lControlIndex) = New TextBox
                txtAddress2(lControlIndex).Name = "_txtAddress2_" & CStr(lControlIndex)
                Me.Controls.Add(txtAddress2(lControlIndex))
            Case "txtAddress3"
                ReDim Preserve txtAddress3(lControlIndex)
                txtAddress3(lControlIndex) = New TextBox
                txtAddress3(lControlIndex).Name = "_txtAddress3_" & CStr(lControlIndex)
                Me.Controls.Add(txtAddress3(lControlIndex))
            Case "txtAddress4"
                ReDim Preserve txtAddress4(lControlIndex)
                txtAddress4(lControlIndex) = New TextBox
                txtAddress4(lControlIndex).Name = "_txtAddress4_" & CStr(lControlIndex)
                Me.Controls.Add(txtAddress4(lControlIndex))
            Case "txtAddress5"
                ReDim Preserve txtAddress5(lControlIndex)
                txtAddress5(lControlIndex) = New TextBox
                txtAddress5(lControlIndex).Name = "_txtAddress5_" & CStr(lControlIndex)
                Me.Controls.Add(txtAddress5(lControlIndex))
            Case "txtAddress6"
                ReDim Preserve txtAddress6(lControlIndex)
                txtAddress6(lControlIndex) = New TextBox
                txtAddress6(lControlIndex).Name = "_txtAddress6_" & CStr(lControlIndex)
                Me.Controls.Add(txtAddress6(lControlIndex))
            Case "cmdAddress"
                ReDim Preserve cmdAddress(lControlIndex)
                cmdAddress(lControlIndex) = New Button
                cmdAddress(lControlIndex).Name = "_cmdAddress_" & CStr(lControlIndex)
                cmdAddress(lControlIndex).Text = "&Change"
                Me.Controls.Add(cmdAddress(lControlIndex))
                AddHandler cmdAddress(lControlIndex).Click, AddressOf cmdAddress_Click
                AddHandler cmdAddress(lControlIndex).Enter, AddressOf cmdAddress_Enter
            Case "PBFindRT1"
                ReDim Preserve PBFindRT1(lControlIndex)
                PBFindRT1(lControlIndex) = New uctPBFindRT.PBFindRT
                PBFindRT1(lControlIndex).Name = "_PBFindRT1_" & CStr(lControlIndex)
                Me.Controls.Add(PBFindRT1(lControlIndex))
                AddHandler PBFindRT1(lControlIndex).ClearValues, AddressOf PBFindRT1_ClearValues
                AddHandler PBFindRT1(lControlIndex).FoundValues, AddressOf PBFindRT1_FoundValues
                AddHandler PBFindRT1(lControlIndex).StartFind, AddressOf PBFindRT1_StartFind
            Case "cboList"
                ReDim Preserve cboList(lControlIndex)
                cboList(lControlIndex) = New PMListMgrDropdown.uctDropdown
                cboList(lControlIndex).Name = "_cboList_" & CStr(lControlIndex)
                Me.Controls.Add(cboList(lControlIndex))
                AddHandler cboList(lControlIndex).ListIndexChange, AddressOf cboList_ListIndexChange
                AddHandler cboList(lControlIndex).Change, AddressOf cboList_Change
                AddHandler cboList(lControlIndex).GotFocus, AddressOf cboList_GotFocus
                AddHandler cboList(lControlIndex).Validating, AddressOf cboList_Validating
                AddHandler cboList(lControlIndex).Enter, AddressOf cboList_GotFocus
            Case "lblList"
                ReDim Preserve lblList(lControlIndex)
                lblList(lControlIndex) = New Label
                lblList(lControlIndex).Name = "_lblList_" & CStr(lControlIndex)
                lblList(lControlIndex).Text = "List Label"
                lblList(lControlIndex).AutoSize = True
                Me.Controls.Add(lblList(lControlIndex))
            Case "lvwListView"
                ReDim Preserve lvwListView(lControlIndex)
                lvwListView(lControlIndex) = New ListView
                lvwListView(lControlIndex).Name = "_lvwListView_" & CStr(lControlIndex)
                lvwListView(lControlIndex).FullRowSelect = True
                lvwListView(lControlIndex).MultiSelect = False
                Me.Controls.Add(lvwListView(lControlIndex))
                AddHandler lvwListView(lControlIndex).ColumnClick, AddressOf lvwListView_ColumnClick
                AddHandler lvwListView(lControlIndex).DoubleClick, AddressOf lvwListView_DoubleClick
                AddHandler lvwListView(lControlIndex).Enter, AddressOf lvwListView_Enter
                AddHandler lvwListView(lControlIndex).MouseDown, AddressOf lvwListView_MouseDown
            Case "cmdListViewAdd"
                ReDim Preserve cmdListViewAdd(lControlIndex)
                cmdListViewAdd(lControlIndex) = New Button
                cmdListViewAdd(lControlIndex).Name = "_cmdListViewAdd_" & CStr(lControlIndex)
                cmdListViewAdd(lControlIndex).Text = iPMFunc.GetResData(g_iLanguageID, ACAddButton, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
                Me.Controls.Add(cmdListViewAdd(lControlIndex))
            Case "cmdListViewEdit"
                ReDim Preserve cmdListViewEdit(lControlIndex)
                cmdListViewEdit(lControlIndex) = New Button
                cmdListViewEdit(lControlIndex).Name = "_cmdListViewEdit_" & CStr(lControlIndex)
                cmdListViewEdit(lControlIndex).Text = iPMFunc.GetResData(g_iLanguageID, ACEditButton, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
                Me.Controls.Add(cmdListViewEdit(lControlIndex))
            Case "cmdListViewDelete"
                ReDim Preserve cmdListViewDelete(lControlIndex)
                cmdListViewDelete(lControlIndex) = New Button
                cmdListViewDelete(lControlIndex).Name = "_cmdListViewDelete_" & CStr(lControlIndex)
                cmdListViewDelete(lControlIndex).Text = iPMFunc.GetResData(g_iLanguageID, ACDeleteButton, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
                Me.Controls.Add(cmdListViewDelete(lControlIndex))
            Case "cmdListViewSequenceUp"
                ReDim Preserve cmdListViewSequenceUp(lControlIndex)
                cmdListViewSequenceUp(lControlIndex) = New Button
                cmdListViewSequenceUp(lControlIndex).Name = "_cmdListViewSequenceUp_" & CStr(lControlIndex)
                cmdListViewSequenceUp(lControlIndex).Image = My.Resources.UP.ToBitmap()
                cmdListViewSequenceUp(lControlIndex).ImageAlign = ContentAlignment.MiddleCenter
                cmdListViewSequenceUp(lControlIndex).Width = 35
                cmdListViewSequenceUp(lControlIndex).Height = 35
                Me.Controls.Add(cmdListViewSequenceUp(lControlIndex))
                AddHandler cmdListViewSequenceUp(lControlIndex).Click, AddressOf cmdListViewSequenceUp_Click

            Case "cmdListViewSequenceDown"
                ReDim Preserve cmdListViewSequenceDown(lControlIndex)
                cmdListViewSequenceDown(lControlIndex) = New Button
                cmdListViewSequenceDown(lControlIndex).Name = "_cmdListViewSequenceDown_" & CStr(lControlIndex)
                cmdListViewSequenceDown(lControlIndex).Image = My.Resources.DOWN.ToBitmap()
                cmdListViewSequenceDown(lControlIndex).ImageAlign = ContentAlignment.MiddleCenter
                cmdListViewSequenceDown(lControlIndex).Width = 35
                cmdListViewSequenceDown(lControlIndex).Height = 35
                Me.Controls.Add(cmdListViewSequenceDown(lControlIndex))
                AddHandler cmdListViewSequenceDown(lControlIndex).Click, AddressOf cmdListViewSequenceDown_Click
            Case "lvwSumInsured"
                ReDim Preserve lvwSumInsured(lControlIndex)
                lvwSumInsured(lControlIndex) = New ListView
                lvwSumInsured(lControlIndex).Name = "_lvwSumInsured_" & CStr(lControlIndex)
                lvwSumInsured(lControlIndex).FullRowSelect = True
                lvwSumInsured(lControlIndex).MultiSelect = False
                Me.Controls.Add(lvwSumInsured(lControlIndex))
                AddHandler lvwSumInsured(lControlIndex).Enter, AddressOf lvwSumInsured_Enter
                AddHandler lvwSumInsured(lControlIndex).MouseDown, AddressOf lvwSumInsured_MouseDown
            Case "cmdSumInsuredAdd"
                ReDim Preserve cmdSumInsuredAdd(lControlIndex)
                cmdSumInsuredAdd(lControlIndex) = New Button
                cmdSumInsuredAdd(lControlIndex).Name = "_cmdSumInsuredAdd_" & CStr(lControlIndex)
                cmdSumInsuredAdd(lControlIndex).Text = iPMFunc.GetResData(g_iLanguageID, ACAddButton, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
                Me.Controls.Add(cmdSumInsuredAdd(lControlIndex))
                AddHandler cmdSumInsuredAdd(lControlIndex).Click, AddressOf cmdSumInsuredAdd_Click
                AddHandler cmdSumInsuredAdd(lControlIndex).Enter, AddressOf cmdSumInsuredAdd_Enter
            Case "cmdSumInsuredDelete"
                ReDim Preserve cmdSumInsuredDelete(lControlIndex)
                cmdSumInsuredDelete(lControlIndex) = New Button
                cmdSumInsuredDelete(lControlIndex).Name = "_cmdSumInsuredDelete_" & CStr(lControlIndex)
                cmdSumInsuredDelete(lControlIndex).Text = iPMFunc.GetResData(g_iLanguageID, ACDeleteButton, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
                Me.Controls.Add(cmdSumInsuredDelete(lControlIndex))
                AddHandler cmdSumInsuredDelete(lControlIndex).Click, AddressOf cmdSumInsuredDelete_Click
                AddHandler cmdSumInsuredDelete(lControlIndex).Enter, AddressOf cmdSumInsuredDelete_Enter
            Case "cmdSumInsuredEdit"
                ReDim Preserve cmdSumInsuredEdit(lControlIndex)
                cmdSumInsuredEdit(lControlIndex) = New Button
                cmdSumInsuredEdit(lControlIndex).Name = "_cmdSumInsuredEdit_" & CStr(lControlIndex)
                cmdSumInsuredEdit(lControlIndex).Text = iPMFunc.GetResData(g_iLanguageID, ACEditButton, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
                Me.Controls.Add(cmdSumInsuredEdit(lControlIndex))
                AddHandler cmdSumInsuredEdit(lControlIndex).Click, AddressOf cmdSumInsuredEdit_Click
                AddHandler cmdSumInsuredEdit(lControlIndex).Enter, AddressOf cmdSumInsuredEdit_Enter
            Case "lblTotalSumInsured"
                ReDim Preserve lblTotalSumInsured(lControlIndex)
                lblTotalSumInsured(lControlIndex) = New Label
                lblTotalSumInsured(lControlIndex).Name = "_lblTotalSumInsured_" & CStr(lControlIndex)
                lblTotalSumInsured(lControlIndex).Text = "Total sum insured:"
                lblTotalSumInsured(lControlIndex).Width = 121
                Me.Controls.Add(lblTotalSumInsured(lControlIndex))
            Case "pnlTotalSumInsured"
                ReDim Preserve pnlTotalSumInsured(lControlIndex)
                pnlTotalSumInsured(lControlIndex) = New Label
                pnlTotalSumInsured(lControlIndex).Name = "_pnlTotalSumInsured_" & CStr(lControlIndex)
                pnlTotalSumInsured(lControlIndex).FlatStyle = FlatStyle.System
                pnlTotalSumInsured(lControlIndex).BorderStyle = Windows.Forms.BorderStyle.Fixed3D
                pnlTotalSumInsured(lControlIndex).Width = 177
                Me.Controls.Add(pnlTotalSumInsured(lControlIndex))
            Case "lblRate"
                ReDim Preserve lblRate(lControlIndex)
                lblRate(lControlIndex) = New Label
                lblRate(lControlIndex).Name = "_lblRate_" & CStr(lControlIndex)
                lblRate(lControlIndex).Text = "Rate:"
                lblRate(lControlIndex).Width = 73
                Me.Controls.Add(lblRate(lControlIndex))
            Case "txtRate"
                ReDim Preserve txtRate(lControlIndex)
                txtRate(lControlIndex) = New TextBox
                txtRate(lControlIndex).Name = "_txtRate_" & CStr(lControlIndex)
                Me.Controls.Add(txtRate(lControlIndex))
                AddHandler txtRate(lControlIndex).Enter, AddressOf txtRate_Enter
                AddHandler txtRate(lControlIndex).Validating, AddressOf txtRate_Validating
            Case "lblPremium"
                ReDim Preserve lblPremium(lControlIndex)
                lblPremium(lControlIndex) = New Label
                lblPremium(lControlIndex).Name = "_lblPremium_" & CStr(lControlIndex)
                lblPremium(lControlIndex).Text = "Premium:"
                Me.Controls.Add(lblPremium(lControlIndex))
            Case "txtPremium"
                ReDim Preserve txtPremium(lControlIndex)
                txtPremium(lControlIndex) = New TextBox
                txtPremium(lControlIndex).Name = "_txtPremium_" & CStr(lControlIndex)
                Me.Controls.Add(txtPremium(lControlIndex))
                AddHandler txtPremium(lControlIndex).Enter, AddressOf txtPremium_Enter
                AddHandler txtPremium(lControlIndex).Validating, AddressOf txtPremium_Validating
            Case "cmdPolicyCommand"
                ReDim Preserve cmdPolicyCommand(lControlIndex)
                cmdPolicyCommand(lControlIndex) = New Button
                cmdPolicyCommand(lControlIndex).Name = "_cmdPolicyCommand_" & CStr(lControlIndex)
                Me.Controls.Add(cmdPolicyCommand(lControlIndex))
                AddHandler cmdPolicyCommand(lControlIndex).Enter, AddressOf cmdPolicyCommand_Enter
                AddHandler cmdPolicyCommand(lControlIndex).Click, AddressOf cmdPolicyCommand_Click
            Case "pnlPolicyPanel"
                ReDim Preserve pnlPolicyPanel(lControlIndex)
                pnlPolicyPanel(lControlIndex) = New Label
                pnlPolicyPanel(lControlIndex).Name = "_pnlPolicyPanel_" & CStr(lControlIndex)
                pnlPolicyPanel(lControlIndex).Width = 153
                pnlPolicyPanel(lControlIndex).FlatStyle = FlatStyle.System
                pnlPolicyPanel(lControlIndex).BorderStyle = Windows.Forms.BorderStyle.Fixed3D
                Me.Controls.Add(pnlPolicyPanel(lControlIndex))
            Case "cmdPartyCommand"
                ReDim Preserve cmdPartyCommand(lControlIndex)
                cmdPartyCommand(lControlIndex) = New Button
                cmdPartyCommand(lControlIndex).Name = "_cmdPartyCommand_" & CStr(lControlIndex)
                Me.Controls.Add(cmdPartyCommand(lControlIndex))
                AddHandler cmdPartyCommand(lControlIndex).Click, AddressOf cmdPartyCommand_Click
                AddHandler cmdPartyCommand(lControlIndex).Enter, AddressOf cmdPartyCommand_Enter
            Case "pnlPartyPanel"
                ReDim Preserve pnlPartyPanel(lControlIndex)
                pnlPartyPanel(lControlIndex) = New Label
                pnlPartyPanel(lControlIndex).Name = "_pnlPartyPanel_" & CStr(lControlIndex)
                pnlPartyPanel(lControlIndex).Width = 153
                pnlPartyPanel(lControlIndex).FlatStyle = FlatStyle.System
                pnlPartyPanel(lControlIndex).BorderStyle = Windows.Forms.BorderStyle.Fixed3D
                Me.Controls.Add(pnlPartyPanel(lControlIndex))
            Case "lvwStandardWording"
                ReDim Preserve lvwStandardWording(lControlIndex)
                lvwStandardWording(lControlIndex) = New ListView
                lvwStandardWording(lControlIndex).Name = "_lvwStandardWording_" & CStr(lControlIndex)
                lvwStandardWording(lControlIndex).MultiSelect = False
                lvwStandardWording(lControlIndex).FullRowSelect = True
                Me.Controls.Add(lvwStandardWording(lControlIndex))
                AddHandler lvwStandardWording(lControlIndex).Enter, AddressOf lvwStandardWording_Enter
                AddHandler lvwStandardWording(lControlIndex).MouseDown, AddressOf lvwStandardWording_MouseDown
            Case "cmdStandardWordingAdd"
                ReDim Preserve cmdStandardWordingAdd(lControlIndex)
                cmdStandardWordingAdd(lControlIndex) = New Button
                cmdStandardWordingAdd(lControlIndex).Name = "_cmdStandardWordingAdd_" & CStr(lControlIndex)
                cmdStandardWordingAdd(lControlIndex).Text = iPMFunc.GetResData(g_iLanguageID, ACAddButton, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
                Me.Controls.Add(cmdStandardWordingAdd(lControlIndex))
                AddHandler cmdStandardWordingAdd(lControlIndex).Enter, AddressOf cmdStandardWordingAdd_Enter
                AddHandler cmdStandardWordingAdd(lControlIndex).Click, AddressOf cmdStandardWordingAdd_Click
            Case "cmdStandardWordingEdit"
                ReDim Preserve cmdStandardWordingEdit(lControlIndex)
                cmdStandardWordingEdit(lControlIndex) = New Button
                cmdStandardWordingEdit(lControlIndex).Name = "_cmdStandardWordingEdit_" & CStr(lControlIndex)
                cmdStandardWordingEdit(lControlIndex).Text = iPMFunc.GetResData(g_iLanguageID, ACEditButton, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
                Me.Controls.Add(cmdStandardWordingEdit(lControlIndex))
                AddHandler cmdStandardWordingEdit(lControlIndex).Click, AddressOf cmdStandardWordingEdit_Click
            Case "cmdStandardWordingDelete"
                ReDim Preserve cmdStandardWordingDelete(lControlIndex)
                cmdStandardWordingDelete(lControlIndex) = New Button
                cmdStandardWordingDelete(lControlIndex).Name = "_cmdStandardWordingDelete_" & CStr(lControlIndex)
                cmdStandardWordingDelete(lControlIndex).Text = iPMFunc.GetResData(g_iLanguageID, ACDeleteButton, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
                Me.Controls.Add(cmdStandardWordingDelete(lControlIndex))
                AddHandler cmdStandardWordingDelete(lControlIndex).Enter, AddressOf cmdStandardWordingDelete_Enter
                AddHandler cmdStandardWordingDelete(lControlIndex).Click, AddressOf cmdStandardWordingDelete_Click
            Case "cmdStandardWordingUp"
                ReDim Preserve cmdStandardWordingUp(lControlIndex)
                cmdStandardWordingUp(lControlIndex) = New Button
                cmdStandardWordingUp(lControlIndex).Name = "_cmdStandardWordingUp_" & CStr(lControlIndex)
                cmdStandardWordingUp(lControlIndex).Image = My.Resources.UP.ToBitmap()
                cmdStandardWordingUp(lControlIndex).ImageAlign = ContentAlignment.MiddleCenter
                cmdStandardWordingUp(lControlIndex).Width = 35
                cmdStandardWordingUp(lControlIndex).Height = 35
                Me.Controls.Add(cmdStandardWordingUp(lControlIndex))
                AddHandler cmdStandardWordingUp(lControlIndex).Click, AddressOf cmdStandardWordingUp_Click
            Case "cmdStandardWordingDown"
                ReDim Preserve cmdStandardWordingDown(lControlIndex)
                cmdStandardWordingDown(lControlIndex) = New Button
                cmdStandardWordingDown(lControlIndex).Name = "_cmdStandardWordingDown_" & CStr(lControlIndex)
                cmdStandardWordingDown(lControlIndex).Image = My.Resources.DOWN.ToBitmap()
                cmdStandardWordingDown(lControlIndex).ImageAlign = ContentAlignment.MiddleCenter
                cmdStandardWordingDown(lControlIndex).Width = 35
                cmdStandardWordingDown(lControlIndex).Height = 35
                Me.Controls.Add(cmdStandardWordingDown(lControlIndex))
                AddHandler cmdStandardWordingDown(lControlIndex).Click, AddressOf cmdStandardWordingDown_Click
            Case "lblStandardWordingMove"
                ReDim Preserve lblStandardWordingMove(lControlIndex)
                lblStandardWordingMove(lControlIndex) = New Label
                lblStandardWordingMove(lControlIndex).Name = "_lblStandardWordingMove_" & CStr(lControlIndex)
                lblStandardWordingMove(lControlIndex).Text = "Move"
                lblStandardWordingMove(lControlIndex).AutoSize = True
                Me.Controls.Add(lblStandardWordingMove(lControlIndex))
        End Select

    End Sub

    ' ***************************************************************** '
    '
    ' Name: AddCheckBox
    '
    ' Description:
    '
    ' History: 26/06/2000 Tomo - Created.
    ' RAW 09/07/2004 : JIT : added v_lScreenDetailsIndex param and used instead of m_cControlCount
    ' ***************************************************************** '
    Private Function AddCheckBox(ByRef iFrameIndex As Integer, ByRef lTag As Integer, ByRef lX As Single, ByRef lY As Single, ByRef sPropertyName As String, ByRef vPMFormat As Object, ByRef vTabSetIndex As Object, ByVal v_lScreenDetailsIndex As Integer) As Integer

        Dim result As Integer = 0
        Dim lDebugDepthCounter As Integer
        'Dim vArray As Object
        'Dim cTheControl As Control
        Dim iLogMessage As String = CStr(1)
        Dim lControlIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = AddFrame(iFrameIndex, lTag, lX, lY, 0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                Return result
            End If

            ' RAW 09/07/2004 : JIT : added
            If chkYesNo.Length = 1 Then
                ' This is the first control of this type to be created so start at index 1
                ' - index 0 remains in its original state as a template for later controls of this type
                m_lCheckIndex = 1
                ReDim m_vCheckArray(ACCLastArrayPosition, m_lCheckIndex)
            Else
                m_lCheckIndex = chkYesNo.Length
                ReDim Preserve m_vCheckArray(ACCLastArrayPosition, m_lCheckIndex)

            End If

            ' RAW 09/07/2004 : JIT : added
            ' store this index locally.
            ' Do not refer to the module level variable again from within this function as it may change
            ' if DL is triggered resulting in other controls being created, and thus the index being incremented,
            ' before this function has completed
            lControlIndex = m_lCheckIndex

            If lControlIndex > 0 Then
                ' RAW 09/07/2004 : JIT : added
#If (DebugOption And m_klDebugOption_JIT) = m_klDebugOption_JIT Then

            AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="JIT : Adding CheckBox (" & lControlIndex & ") - " & ObjectAndPropertyName(lTag))
#End If
                AddControl("chkYesNo", lControlIndex)
                AddControl("lblCheckLabel", lControlIndex)

            End If

            ' RAW 09/07/2004 : JIT : added
            ' store reference to this control in a collection - keyed on object and property name

            m_cObjectAndAttribute.Add(New Object() {chkYesNo(lControlIndex), lblCheckLabel(lControlIndex), "", v_lScreenDetailsIndex}, ObjectAndPropertyName(lTag).ToLower())

            m_oNewControlBeingLoaded = chkYesNo(lControlIndex)  ' RAW 20/08/2004 : JIT : added


            ' RDT PN5310 - If the width of the control on the PB form happens to be the same size
            ' as the constant used for Tri State Check Boxes then we will experience adverse behaviour.
            If m_lWidth = VB6.TwipsToPixelsY(cCheckBoxTriStateCaptionWidth) Then
                m_lWidth -= 1
            End If

            ' RAW 17/06/2003 : added m_vDataDictionary and m_vScreenValues arguments
            SetInitialControlValues(lControlIndex, m_vCheckArray, lblCheckLabel, chkYesNo, fraFrame, iFrameIndex, Task, vPMFormat, vTabSetIndex, 0, CInt(lX), CInt(lY), sPropertyName, lTag, v_lScreenDetailsIndex, True, m_lHeight, m_lWidth, m_sHelpText, m_lPreQuote, m_lPostQuote, m_lPurchase, m_lIsValuation, m_lIsRateAndPremium, m_lIncludeInList, m_lStage, m_oFormFields, v_lScreenDetailsIndex, (m_vDataDictionary(GISDMTypeRisk)), m_vScreenValues, ACPDataType:=MainModule.ACPDataType, ACPIsInputProperty:=MainModule.ACPIsInputProperty)
            ToolTip1.SetToolTip(chkYesNo(lControlIndex), Convert.ToString(m_sHelpText))
            'make it look like a normal check box
            chkYesNo(lControlIndex).Width = VB6.TwipsToPixelsY(cCheckBoxCaptionWidth)

            'now, and only now make it look like a tri-state
            If vPMFormat = gPMConstants.PMEFormatStyle.PMFormatBoolean Then  ' PMFormatBoolean=tri-state !
                chkYesNo(lControlIndex).Width = VB6.TwipsToPixelsY(cCheckBoxTriStateCaptionWidth)
            End If

            'set the Yes/No/Unkown
            simulateTriStateCheckBox(chkYesNo(lControlIndex).CheckState, chkYesNo(lControlIndex))

            'this call positions everything correctly
            textLabel_MouseMove("chkYesNo", MouseButtons.Left, g_lx, g_ly, chkYesNo(lControlIndex), lblCheckLabel(lControlIndex), pnlPosition, m_vTabArray(ACFTabSnapToGrid, CInt(m_vFrameArray(ACFTabNumber, iFrameIndex))))


            ' now flag this in the array as having been created
            m_vScreenDetails(ACDScreenDetailCnt, v_lScreenDetailsIndex) = v_lScreenDetailsIndex  ' RAW 20/08/2004 : JIT : added
            m_oNewControlBeingLoaded = Nothing  ' RAW 20/08/2004 : JIT : added



        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "AddCheckBox Failed", ACApp, ACClass, "AddCheckBox", Information.Err().Number, Information.Err().Description, excep:=ex)


        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If


        End Try
        Return result
    End Function

    ' RAW 09/07/2004 : JIT : added v_lScreenDetailsIndex param and used instead of m_cControlCount
    Private Function AddFindControl(ByRef iFrameIndex As Integer, ByRef lTag As Integer, ByRef lX As Single, ByRef lY As Single, ByRef sPropertyName As String, ByRef vPMFormat As Object, ByRef vTabSetIndex As Object, ByVal v_lScreenDetailsIndex As Integer) As Integer

        Dim result As Integer = 0
        Dim lDebugDepthCounter As Integer
        'Dim vArray As Object
        'Dim cTheControl As Control
        Dim iLogMessage As String = CStr(1)
        Dim lControlIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = AddFrame(iFrameIndex, lTag, lX, lY, 0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                Return result
            End If

            ' RAW 09/07/2004 : JIT : added
            If PBFindRT1.Length = 1 Then
                ' This is the first control of this type to be created so start at index 1
                ' - index 0 remains in its original state as a template for later controls of this type
                m_lFindControlIndex = 1
                ReDim m_vFindControlArray(ACCLastArrayPosition, m_lFindControlIndex)
            Else
                m_lFindControlIndex = PBFindRT1.Length
                ReDim Preserve m_vFindControlArray(ACCLastArrayPosition, m_lFindControlIndex)
            End If

            ' RAW 09/07/2004 : JIT : added
            ' store this index locally.
            ' Do not refer to the module level variable again from within this function as it may change
            ' if DL is triggered resulting in other controls being created, and thus the index being incremented,
            ' before this function has completed
            lControlIndex = m_lFindControlIndex

            If lControlIndex > 0 Then
                ' RAW 09/07/2004 : JIT : added
#If (DebugOption And m_klDebugOption_JIT) = m_klDebugOption_JIT Then

            AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="JIT : Adding Find Control ")
#End If
                AddControl("PBFindRT1", lControlIndex)


            End If

            ' RAW 09/07/2004 : JIT : added
            ' store reference to this control in a collection - keyed on object and property name
            '    m_cObjectAndAttribute.Add Array(lblText(lControlIndex), txtText(lControlIndex), "", v_lScreenDetailsIndex), _
            ''                              LCase(ObjectAndPropertyName(r_vObjIdx:=m_vScreenDetails(ACDGISPropertyId, v_lScreenDetailsIndex), _
            ''                                                          r_PropIdx:=v_lScreenDetailsIndex))

            m_oNewControlBeingLoaded = PBFindRT1(lControlIndex)  ' RAW 20/08/2004 : JIT : added

            'SetInitialControlValues(lFindControlIndex, m_vFindControlArray, "", PBFindRT1, fraFrame, iFrameIndex, Task, vPMFormat, vTabSetIndex, 0, lX, lY, sPropertyName, lTag, m_lControlCount, True, m_lHeight, m_lWidth, m_sHelpText, m_lPreQuote, m_lPostQuote, m_lPurchase, m_lIsValuation, m_lIsRateAndPremium, m_lIncludeInList, m_lStage, m_oFormFields, m_lControlCount)
            'ToolTip1.SetToolTip(PBFindRT1(lControlIndex), Convert.ToString(m_sHelpText))
            'set up findcontrol

            PBFindRT1(lControlIndex).Left = VB6.TwipsToPixelsX(lX)
            PBFindRT1(lControlIndex).Top = VB6.TwipsToPixelsY(lY)
            PBFindRT1(lControlIndex).FindControlID = CInt(m_vScreenDetails(ACDPMFormat, v_lScreenDetailsIndex))
            PBFindRT1(lControlIndex).InsuranceFileCnt = m_lInsuranceFileCnt
            PBFindRT1(lControlIndex).ClaimCnt = m_lClaimID
            PBFindRT1(lControlIndex).Start()
            PBFindRT1(lControlIndex).Visible = True
            PBFindRT1(lControlIndex).Parent = fraFrame(iFrameIndex)

            g_sControlName = "FindControl"


            ' now flag this in the array as having been created
            m_vScreenDetails(ACDScreenDetailCnt, v_lScreenDetailsIndex) = v_lScreenDetailsIndex  ' RAW 20/08/2004 : JIT : added
            m_oNewControlBeingLoaded = Nothing  ' RAW 20/08/2004 : JIT : added



        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "AddFindControl Failed", ACApp, ACClass, "AddFindControl", Information.Err().Number, Information.Err().Description, excep:=ex)


        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If


        End Try
        Return result
    End Function


    ' ***************************************************************** '
    '
    ' Name: AddFrame
    '
    ' Description:
    '
    ' History: 26/06/2000 Tomo - Created.
    ' RAW 09/07/2004 : JIT : increased FrameIndex param from integer to long
    '               added v_vScreenDetailsIndex param
    ' ***************************************************************** '
    Private Function AddFrame(ByRef iFrameIndex As Integer, ByRef lTag As Integer, ByRef lX As Single, ByRef lY As Single, ByRef lAddIfLessThan As Integer, Optional ByVal v_vScreenDetailsIndex As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lDebugDepthCounter As Integer
        Dim sName As String = ""
        Dim lHeightOffset, lContainerHeight, lContainerWidth, lControlIndex As Integer

        Try

            ' Note - m_lFrameIndex is the index to this frame,
            '        iFrameIndex may be the index to the parent frame (if applicable) otherwise it also is the index to this frame

            result = gPMConstants.PMEReturnCode.PMTrue

            If iFrameIndex >= lAddIfLessThan Then
                Return result
            End If


            ' RAW 09/07/2004 : JIT : adde
            If m_lFrameIndex < 0 Then
                ' This is the first control of this type to be created
                ' Unlike other controls we do use index 0 because other controls link to it using a 0-based numbering sequence
                m_lFrameIndex = 0
                ReDim m_vFrameArray(ACFLastArrayPosition, m_lFrameIndex)
            Else
                m_lFrameIndex += 1
                ReDim Preserve m_vFrameArray(ACFLastArrayPosition, m_lFrameIndex)

            End If

            ' RAW 09/07/2004 : JIT : added
            ' store this index locally.
            ' Do not refer to the module level variable again from within this function as it may change
            ' if DL is triggered resulting in other controls being created, and thus the index being incremented,
            ' before this function has completed
            lControlIndex = m_lFrameIndex

            If lControlIndex > 0 Then
                ' RAW 09/07/2004 : JIT : added
#If (DebugOption And m_klDebugOption_JIT) = m_klDebugOption_JIT Then

            AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="JIT : Adding Frame - " & m_lFrameIndex)
#End If
                AddControl("fraFrame", lControlIndex)
            End If


            ' RAW 09/07/2004 : JIT : added
            If Not IsNothing(v_vScreenDetailsIndex) Then


                If Not (Convert.IsDBNull(m_vScreenDetails(ACDGISObjectId, (v_vScreenDetailsIndex))) Or IsNothing(m_vScreenDetails(ACDGISObjectId, (v_vScreenDetailsIndex)))) Then

                    ' This is a frame for a control that we will not create until it is displayed or referenced.
                    ' For other controls ACDParentId defines the index of the frame that contains the control
                    ' and is used when placing that control in the correct frame.
                    ' For frames, ACDParentId represents the parent frame for a frame that is within a frame so
                    ' ACDParentId cannot be used.
                    ' Instead Add an entry to the controls collection as a marker for reference when creating the control

                    ' At the moment this applies to list views

                    ' store reference to this control in a collection - keyed on object and property name

                    m_cObjectAndAttribute.Add(New Object() {fraFrame(iFrameIndex), fraFrame(lControlIndex), "", v_vScreenDetailsIndex}, ObjectAndPropertyName(lTag, "#FRAME#").ToLower())
                End If
            End If




            m_vFrameArray(ACFFrameNumber, lControlIndex) = DBNull.Value
            m_vFrameArray(ACFIsDeleted, lControlIndex) = gPMConstants.PMEReturnCode.PMFalse

            m_vFrameArray(ACFChildId, lControlIndex) = DBNull.Value

            m_vFrameArray(ACFGISObjectId, lControlIndex) = DBNull.Value


            ' RAW 09/07/2004 : JIT : added test for v_vScreenDetailsIndex missing
            'If Not Not (v_vScreenDetailsIndex_optional Is Nothing) AndAlso v_vScreenDetailsIndex_optional.Equals(Type.Missing) Then
            If Not IsNothing(v_vScreenDetailsIndex) Then

                ' RAW 09/07/2004 : JIT : code moved from BuildScreen
                fraFrame(lControlIndex).Tag = CStr(m_vScreenDetails(ACDTabNumber, (v_vScreenDetailsIndex)))  ' RAW 09/07/2004 : JIT : replaced lControlIndex

                m_vFrameArray(ACFTabNumber, lControlIndex) = m_vScreenDetails(ACDTabNumber, (v_vScreenDetailsIndex))  ' RAW 09/07/2004 : JIT : replaced m_lTabIndex


                If Not (Convert.IsDBNull(m_vScreenDetails(ACDParentId, (v_vScreenDetailsIndex))) Or IsNothing(m_vScreenDetails(ACDParentId, (v_vScreenDetailsIndex)))) Then
                    fraFrame(lControlIndex).Parent = fraFrame(CInt(m_vScreenDetails(ACDParentId, (v_vScreenDetailsIndex))))
                    m_vFrameArray(ACFFrameNumber, lControlIndex) = m_vScreenDetails(ACDParentId, (v_vScreenDetailsIndex))
                End If

                fraFrame(lControlIndex).Top = VB6.TwipsToPixelsY(CDbl(m_vScreenDetails(ACDTop, (v_vScreenDetailsIndex))))
                fraFrame(lControlIndex).Left = VB6.TwipsToPixelsX(CDbl(m_vScreenDetails(ACDLeft, (v_vScreenDetailsIndex))))
                fraFrame(lControlIndex).Height = VB6.TwipsToPixelsY(CDbl(m_vScreenDetails(ACDHeight, (v_vScreenDetailsIndex))))
                fraFrame(lControlIndex).Width = VB6.TwipsToPixelsX(CDbl(m_vScreenDetails(ACDWidth, (v_vScreenDetailsIndex))))

                fraFrame(lControlIndex).Text = CStr(m_vScreenDetails(ACDCaption, (v_vScreenDetailsIndex)))

                fraFrame(lControlIndex).Visible = False
                fraFrame(lControlIndex).BringToFront()

                m_vFrameArray(ACFGISObjectId, lControlIndex) = m_vScreenDetails(ACDGISObjectId, (v_vScreenDetailsIndex))
                m_vFrameArray(ACFTabSetIndex, lControlIndex) = m_vScreenDetails(ACDTabSetIndex, (v_vScreenDetailsIndex))
                ' RAW 09/07/2004 : JIT : end

            Else

                ' called when adding another control that is contained within this frame

                sName = ""
                Dim auxVar As Object = m_vDataDictionary(GISDMTypeRisk)(ACOObjectName, lTag Mod 10000)

                If Not (Convert.IsDBNull(auxVar) Or IsNothing(auxVar)) Then
                    sName = ((m_vDataDictionary(GISDMTypeRisk)(ACOObjectName, lTag Mod 10000)))
                End If

                fraFrame(lControlIndex).Tag = CStr(lControlIndex)

                m_vFrameArray(ACFTabNumber, lControlIndex) = m_lTabIndex

                'Guess - fine tune later
                lHeightOffset = 720
                lContainerHeight = CInt(VB6.PixelsToTwipsY(TabStrip1.Height))
                lContainerWidth = CInt(TabStrip1.Width)

                If lAddIfLessThan > 1000 Then
                    If iFrameIndex > -1 Then
                        fraFrame(lControlIndex).Parent = fraFrame(iFrameIndex)
                        m_vFrameArray(ACFFrameNumber, lControlIndex) = iFrameIndex

                        lHeightOffset = 0
                        lContainerHeight = CInt(VB6.PixelsToTwipsY(fraFrame(iFrameIndex).Height))
                        lContainerWidth = CInt(VB6.PixelsToTwipsX(fraFrame(iFrameIndex).Width))
                    End If
                End If

                'Take into account the tab...
                fraFrame(lControlIndex).Top = VB6.TwipsToPixelsY(360 + lHeightOffset)
                lY -= 360
                fraFrame(lControlIndex).Height = VB6.TwipsToPixelsY(lContainerHeight - 480)
                fraFrame(lControlIndex).Left = VB6.TwipsToPixelsX(120)
                lX -= 120
                fraFrame(lControlIndex).Width = VB6.TwipsToPixelsX(lContainerWidth - 240)

                If sName <> "" Then
                    fraFrame(lControlIndex).Text = sName
                End If

                If lHeightOffset = 0 Then
                    'It's within another frame
                    fraFrame(lControlIndex).Visible = True
                Else
                    fraFrame(lControlIndex).Visible = False
                End If

            End If


            iFrameIndex = lControlIndex


            ' now flag this in the array as having been created
            'If Not Not (v_vScreenDetailsIndex_optional Is Nothing) AndAlso v_vScreenDetailsIndex_optional.Equals(Type.Missing) Then
            If Not IsNothing(v_vScreenDetailsIndex) Then
                'm_vScreenDetails(ACDScreenDetailCnt, (v_vScreenDetailsIndex)) = v_vScreenDetailsIndex ' RAW 20/08/2004 : JIT : added
                m_vScreenDetails(ACDScreenDetailCnt, (v_vScreenDetailsIndex)) = CStr(v_vScreenDetailsIndex)  ' RAW 20/08/2004 : JIT : added
            End If


        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "AddFrame Failed", ACApp, ACClass, "AddFrame", Information.Err().Number, Information.Err().Description, excep:=ex)

        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: AddListView
    '
    ' Description:
    '
    ' History:
    ' RAW 09/07/2004 : JIT : added - code taken from BuildScreen
    ' ***************************************************************** '
    Private Function AddListView(ByRef iFrameIndex As Integer, ByRef lTag As Integer, ByRef lX As Single, ByRef lY As Single, ByRef sPropertyName As String, ByRef vPMFormat As Object, ByRef vTabSetIndex As Object, ByVal v_lScreenDetailsIndex As Integer) As Integer

        Dim result As Integer = 0
        Dim lDebugDepthCounter As Integer
        'Dim vArray As Object
        'Dim cTheControl As Control
        Dim iLogMessage As String = CStr(1)
        Dim lControlIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = AddFrame(iFrameIndex, lTag, lX, lY, 0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                Return result
            End If



            Select Case m_vScreenDetails(ACDExtraObjectType, v_lScreenDetailsIndex)
                Case GISOTRisk, GISOTAssociatedClient, GISOTDisclosure, GISOTCase

                Case Else
                    Return result

            End Select


            ' RAW 09/07/2004 : JIT : added
            If lvwListView.Length = 1 Then
                ' This is the first control of this type to be created so start at index 1
                ' - index 0 remains in its original state as a template for later controls of this type
                m_lListViewIndex = 1
                ReDim m_vListViewArray(ACCLastArrayPosition, m_lListViewIndex)
            Else
                m_lListViewIndex = lvwListView.Length
                ReDim Preserve m_vListViewArray(ACCLastArrayPosition, m_lListViewIndex)
            End If

            ' RAW 09/07/2004 : JIT : added
            ' store this index locally.
            ' Do not refer to the module level variable again from within this function as it may change
            ' if DL is triggered resulting in other controls being created, and thus the index being incremented,
            ' before this function has completed
            lControlIndex = m_lListViewIndex

            If lControlIndex > 0 Then

                ' RAW 09/07/2004 : JIT : added
#If (DebugOption And m_klDebugOption_JIT) = m_klDebugOption_JIT Then

            AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="JIT : Adding ListView (" & lControlIndex & ") - " & ObjectAndPropertyName(r_vObjIdx:=lTag, r_PropIdx_optional:=""))
#End If
                AddControl("lvwListView", lControlIndex)
                AddControl("cmdListViewAdd", lControlIndex)
                AddControl("cmdListViewDelete", lControlIndex)
                AddControl("cmdListViewEdit", lControlIndex)
                AddControl("cmdListViewSequenceUp", lControlIndex)
                AddControl("cmdListViewSequenceDown", lControlIndex)

            End If

            ' RAW 09/07/2004 : JIT : added
            ' store reference to this control in a collection - keyed on object and property name

            m_cObjectAndAttribute.Add(New Object() {fraFrame(iFrameIndex), lvwListView(lControlIndex), "", v_lScreenDetailsIndex}, ObjectAndPropertyName(lTag, "").ToLower())

            m_oNewControlBeingLoaded = lvwListView(lControlIndex)  ' RAW 20/08/2004 : JIT : added



            m_vFrameArray(ACFChildId, iFrameIndex) = m_vScreenDetails(ACDChildScreenId, v_lScreenDetailsIndex)
            m_vFrameArray(ACFGISObjectId, iFrameIndex) = m_vScreenDetails(ACDGISObjectId, v_lScreenDetailsIndex)

            lvwListView(lControlIndex).Parent = fraFrame(iFrameIndex)
            lvwListView(lControlIndex).Columns.Add("Test")
            m_vListViewArray(ACCFrameNumber, lControlIndex) = iFrameIndex
            m_vListViewArray(ACCIsDeleted, lControlIndex) = gPMConstants.PMEReturnCode.PMFalse
            m_vListViewArray(ACCHelpText, lControlIndex) = m_vScreenDetails(ACDHelpText, v_lScreenDetailsIndex)
            m_vListViewArray(ACCPreQuote, lControlIndex) = m_vScreenDetails(ACDPreQuoteRequirement, v_lScreenDetailsIndex)
            m_vListViewArray(ACCPostQuote, lControlIndex) = m_vScreenDetails(ACDPostQuoteRequirement, v_lScreenDetailsIndex)
            m_vListViewArray(ACCPurchase, lControlIndex) = m_vScreenDetails(ACDPurchaseRequirement, v_lScreenDetailsIndex)
            m_vListViewArray(ACCIsValuation, lControlIndex) = m_vScreenDetails(ACDIsValuation, v_lScreenDetailsIndex)
            m_vListViewArray(ACCIsRateAndPremium, lControlIndex) = m_vScreenDetails(ACDIsRateAndPremium, v_lScreenDetailsIndex)

            m_vListViewArray(ACCIncludeInList, lControlIndex) = DBNull.Value
            m_vListViewArray(ACCChildId, lControlIndex) = m_vScreenDetails(ACDChildScreenId, v_lScreenDetailsIndex)
            m_vListViewArray(ACCGISObjectId, lControlIndex) = m_vScreenDetails(ACDGISObjectId, v_lScreenDetailsIndex)
            m_vListViewArray(ACCPMFormat, lControlIndex) = m_vScreenDetails(ACDPMFormat, v_lScreenDetailsIndex)
            m_vListViewArray(ACCTabSetIndex, lControlIndex) = m_vScreenDetails(ACDTabSetIndex, v_lScreenDetailsIndex)


#If quoteTiming Then

		performanceCtr(performancecntrCntr, 1) = "uctRiskScreen.AddListView PopulateListView"
		QueryPerformanceCounter performanceCtr(performancecntrCntr, 0)
		performancecntrCntr = performancecntrCntr + 1
#End If

            cmdListViewSequenceUp(lControlIndex).Tag = CStr(-1)  'signal no sequence, plv will set if there is one

            ' RAW 03/11/2003 : CQ2754/2862 : added v_lControlCount param
            ' RAW 22/06/2004 : Performance Changes(#2) : added bSuppressDynamicLogic param

            m_lReturn = PopulateListView(lControlIndex, CInt(m_vScreenDetails(ACDChildScreenId, v_lScreenDetailsIndex)), v_lScreenDetailsIndex, True, True)

            lvwListView(lControlIndex).Top = VB6.TwipsToPixelsY(240)
            lvwListView(lControlIndex).Left = VB6.TwipsToPixelsX(120)
            lvwListView(lControlIndex).Width = lvwListView(lControlIndex).Parent.Width - VB6.TwipsToPixelsX(240)
            lvwListView(lControlIndex).Height = lvwListView(lControlIndex).Parent.Height - VB6.TwipsToPixelsY(720)
            lvwListView(lControlIndex).Visible = True
            lvwListView(lControlIndex).View = View.Details
            'if this list view has a sequence column then add the sequence buttons
            If CDbl(Convert.ToString(cmdListViewSequenceUp(lControlIndex).Tag)) <> -1 Then
                lvwListView(lControlIndex).Width = fraFrame(iFrameIndex).Width - VB6.TwipsToPixelsX(795)
                cmdListViewSequenceUp(lControlIndex).Top = lvwListView(lControlIndex).Top
                cmdListViewSequenceUp(lControlIndex).Left = fraFrame(iFrameIndex).Width - VB6.TwipsToPixelsX(615)
                cmdListViewSequenceDown(lControlIndex).Top = lvwListView(lControlIndex).Top + lvwListView(lControlIndex).Height - cmdListViewSequenceDown(lControlIndex).Height
                cmdListViewSequenceDown(lControlIndex).Left = cmdListViewSequenceUp(lControlIndex).Left
                cmdListViewSequenceUp(lControlIndex).Parent = fraFrame(iFrameIndex)
                cmdListViewSequenceDown(lControlIndex).Parent = fraFrame(iFrameIndex)
                cmdListViewSequenceUp(lControlIndex).Visible = True
                cmdListViewSequenceDown(lControlIndex).Visible = True
            End If

            lTag = -1

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = GetTag(CInt(m_vScreenDetails(ACDGISObjectId, v_lScreenDetailsIndex)), -1, lTag, (m_vDataDictionary(GISDMTypeRisk)))

                lTag = GISDMTypeRisk * 10000 + lTag
                '        lTag = m_vScreenDetails(ACDDataModelType, v_lScreenDetailsIndex) * 10000 + lTag

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    Return result
                End If

                If lTag < 0 Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If

            End If
            lvwListView(lControlIndex).Tag = CStr(CDbl(m_vScreenDetails(ACDDataModelType, v_lScreenDetailsIndex)) * 10000 + lTag)

            cmdListViewAdd(lControlIndex).Parent = fraFrame(iFrameIndex)
            cmdListViewEdit(lControlIndex).Parent = fraFrame(iFrameIndex)
            cmdListViewDelete(lControlIndex).Parent = fraFrame(iFrameIndex)

            cmdListViewAdd(lControlIndex).Top = lvwListView(lControlIndex).Height + VB6.TwipsToPixelsY(300)
            cmdListViewEdit(lControlIndex).Top = lvwListView(lControlIndex).Height + VB6.TwipsToPixelsY(300)
            cmdListViewDelete(lControlIndex).Top = lvwListView(lControlIndex).Height + VB6.TwipsToPixelsY(300)

            cmdListViewAdd(lControlIndex).Left = VB6.TwipsToPixelsX(120)
            cmdListViewEdit(lControlIndex).Left = VB6.TwipsToPixelsX(1260)
            cmdListViewDelete(lControlIndex).Left = VB6.TwipsToPixelsX(2400)

            cmdListViewAdd(lControlIndex).Visible = True
            cmdListViewEdit(lControlIndex).Visible = True
            cmdListViewDelete(lControlIndex).Visible = True

            cmdListViewEdit(lControlIndex).Enabled = False
            cmdListViewDelete(lControlIndex).Enabled = False
            cmdListViewAdd(lControlIndex).Enabled = False

            AddHandler cmdListViewAdd(lControlIndex).Click, AddressOf cmdListViewAdd_Click
            AddHandler cmdListViewAdd(lControlIndex).Enter, AddressOf cmdListViewAdd_Enter

            AddHandler cmdListViewEdit(lControlIndex).Click, AddressOf cmdListViewEdit_Click
            AddHandler cmdListViewEdit(lControlIndex).Enter, AddressOf cmdListViewEdit_Enter

            AddHandler cmdListViewDelete(lControlIndex).Click, AddressOf cmdListViewDelete_Click
            AddHandler cmdListViewDelete(lControlIndex).Enter, AddressOf cmdListViewDelete_Enter

            If Task <> gPMConstants.PMEComponentAction.PMView Then
                If CDbl(m_vListViewArray(ACCChildId, lControlIndex)) <> 0 Then
                    cmdListViewAdd(lControlIndex).Enabled = True
                End If
            End If

            cmdListViewAdd(lControlIndex).Tag = CStr(v_lScreenDetailsIndex)

#If quoteTiming Then

		QueryPerformanceCounter performanceCtr(performancecntrCntr, 0)
		performancecntrCntr = performancecntrCntr + 1
#End If

#If quoteTiming Then

		performanceCtr(performancecntrCntr, 1) = "uctRiskScreen.AddListView SetExtraListViewProperties"
		QueryPerformanceCounter performanceCtr(performancecntrCntr, 0)
		performancecntrCntr = performancecntrCntr + 1
#End If

            m_lReturn = SetExtraListViewProperties(lvwListView(lControlIndex).Handle.ToInt32(), True)

#If quoteTiming Then

		QueryPerformanceCounter performanceCtr(performancecntrCntr, 0)
		performancecntrCntr = performancecntrCntr + 1
#End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If


            ' now flag this in the array as having been created
            m_vScreenDetails(ACDScreenDetailCnt, v_lScreenDetailsIndex) = v_lScreenDetailsIndex  ' RAW 20/08/2004 : JIT : added
            m_oNewControlBeingLoaded = Nothing  ' RAW 20/08/2004 : JIT : added



        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "AddListView Failed", ACApp, ACClass, "AddListView", Information.Err().Number, Information.Err().Description, excep:=ex)


        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If


        End Try
        Return result
    End Function
    ''' <summary>
    ''' Performance Changes(#2) : default v_bSuppressDynamicLogic to m_bSuppressDynamicLogic if missing - renamed and changed from boolean to variant
    ''' </summary>
    ''' <param name="lListViewIndex"></param>
    ''' <param name="lChildId"></param>
    ''' <param name="v_lControlCount"></param>
    ''' <param name="bClear"></param>
    ''' <param name="v_vSuppressDynamicLogic"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PopulateListView(ByVal lListViewIndex As Integer, ByVal lChildId As Integer,
                                      ByVal v_lControlCount As Integer, ByVal bClear As Boolean,
                                      Optional ByVal v_vSuppressDynamicLogic As Object = Nothing
                                                                                         ) As Integer

        Dim nResult As Integer = 0
        Dim lDebugDepthCounter, lChildScreenColumnIndex, lListViewColumnCount As Integer

        Dim oArray(,) As Object
        Dim oArray2, oValue As Object
        Dim oListItem As ListViewItem
        Dim nTag, nColumn As Integer
        Dim sString As String = ""
        Dim oColumnOrder As Object
        Dim bSuppressDynamicLogic As Boolean
        Dim oUseSequenceId As Object
        Dim bSequenceIdColumnWasAdded As Boolean

        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' RAW 22/06/2004 : Performance Changes(#2) : added
            If IsNothing(v_vSuppressDynamicLogic) Then
                bSuppressDynamicLogic = IsDynamicLogicSuppressed  ' RAW 20/08/2004 : JIT : replaced m_bSuppressDynamicLogic with IsDynamicLogicSuppressed
            Else
                bSuppressDynamicLogic = v_vSuppressDynamicLogic
            End If

            'Only do this if we have no columns - the user may have resized, for instance
            'Tomo12052000
            'Not sufficient.  Building up (0) we inherit everything when we load (1)
            If bClear Then
                lvwListView(lListViewIndex).Columns.Clear()
            End If

            If Not Information.IsArray(m_vChildScreenDetails) Then
                Return nResult
            End If

            'prevent flicker
            ListViewFunc.ListViewBatchStart(lvwListView(lListViewIndex))

            'find out what order we want the columns in
            lListViewColumnCount = -1  '
            lChildScreenColumnIndex = -1
            For lScreenDetailIndex As Integer = m_vChildScreenDetails.GetLowerBound(1) To m_vChildScreenDetails.GetUpperBound(1)
                If (m_vChildScreenDetails(ACDGISScreenId, lScreenDetailIndex)) = lChildId Then
                    lChildScreenColumnIndex += 1
                    'if column position is 0 then order hasn't been set so use order read from db
                    If (m_vChildScreenDetails(ACDColumnPosition, lScreenDetailIndex)) = 0 Then
                        m_vChildScreenDetails(ACDColumnPosition, lScreenDetailIndex) = lListViewColumnCount + 1
                    End If

                    If (IIf(Convert.IsDBNull(m_vChildScreenDetails(ACDExtraGISPropertyName, lScreenDetailIndex)) Or IsNothing(m_vChildScreenDetails(ACDExtraGISPropertyName, lScreenDetailIndex)), "", CStr(m_vChildScreenDetails(ACDExtraGISPropertyName, lScreenDetailIndex)))).ToLower() = cProperty_SequenceId Then
                        'this detects sequence_id fields that are on the child screen but may not be included in the list

                        oUseSequenceId = New Object() {lScreenDetailIndex, lChildScreenColumnIndex, m_vChildScreenDetails(ACDColumnWidth, lScreenDetailIndex), m_vChildScreenDetails(ACDColumnPosition, lScreenDetailIndex)}
                    End If

                    If Not (Convert.IsDBNull(m_vChildScreenDetails(ACDColumnWidth, lScreenDetailIndex)) Or IsNothing(m_vChildScreenDetails(ACDColumnWidth, lScreenDetailIndex))) Then
                        lListViewColumnCount += 1
                        If lListViewColumnCount = 0 Then
                            ReDim oColumnOrder(2, lListViewColumnCount)
                        Else
                            ReDim Preserve oColumnOrder(2, lListViewColumnCount)
                        End If
                        'add this column and its required position into the column order array
                        'sort order
                        oColumnOrder(0, lListViewColumnCount) = m_vChildScreenDetails(ACDColumnPosition, lScreenDetailIndex)

                        '(screen detail offset, child screen column offset)

                        oColumnOrder(1, lListViewColumnCount) = New Object() {lScreenDetailIndex, lChildScreenColumnIndex}
                        'add the caption just to make array easier to debug

                        If Trim(m_vChildScreenDetails(ACDCaption, lScreenDetailIndex)) = Trim(ACBlankCaption) Then
                            oColumnOrder(2, lListViewColumnCount) = Nothing
                        Else
                            oColumnOrder(2, lListViewColumnCount) = StripColonFromCaption(m_vChildScreenDetails(ACDCaption, lScreenDetailIndex))
                        End If
                    End If

                End If
            Next lScreenDetailIndex

            'check if there is a sequence number that hasn't been included in the list or on the child screen
            If Information.IsArray(oUseSequenceId) Then
                Dim auxVar As Object = (oUseSequenceId(2))

                If Convert.IsDBNull(auxVar) Or IsNothing(auxVar) Then  'only add if not already manually added
                    bSequenceIdColumnWasAdded = True
                    'add sequence number column as the last column
                    lListViewColumnCount += 1
                    ReDim Preserve oColumnOrder(2, lListViewColumnCount)
                    oColumnOrder(0, lListViewColumnCount) = lListViewColumnCount + 1
                    oUseSequenceId(3) = oColumnOrder(0, lListViewColumnCount)
                    '(screen detail offset, child screen column offset)

                    oColumnOrder(1, lListViewColumnCount) = New Object() {oUseSequenceId(0), oUseSequenceId(1)}
                    'add the caption just to make array easier to debug
                    oColumnOrder(2, lListViewColumnCount) = cProperty_SequenceId
                End If
                cmdListViewSequenceUp(lListViewIndex).Tag = (oUseSequenceId(1))  'stores offset in child data
                cmdListViewSequenceDown(lListViewIndex).Tag = (oUseSequenceId(3))  'stores listview column position

            End If

            'if no columns then probably no child screen so flag an error

            If Object.Equals(oColumnOrder, Nothing) Then
                'TBD add error message saying no coluns in child definition
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If

            SortThreeElementArray(oColumnOrder)

            lvwListView(lListViewIndex).Items.Clear()

            'add the list view column headers (if necessary)
            'moved here to ensure headers are added even if there is no data
            If bClear Then
                'remove all columns from grid
                For lTemp2 As Integer = (oColumnOrder).GetLowerBound(1) To (oColumnOrder).GetUpperBound(1)
                    lvwListView(lListViewIndex).Columns.Add("D" & lTemp2, (oColumnOrder(2, lTemp2)), 94)
                    If (oColumnOrder(2, lTemp2)) = cProperty_SequenceId And bSequenceIdColumnWasAdded Then
                        lvwListView(lListViewIndex).Columns.Item("D" & lTemp2).Width = CInt(0)
                    Else
                        lvwListView(lListViewIndex).Columns.Item("D" & lTemp2).Width = (VB6.TwipsToPixelsX((m_vChildScreenDetails(ACDWidth, (oColumnOrder(1, lTemp2)(0))))))
                    End If
                Next
            End If

            'now start looking at the data
            oArray = m_vScreenValues(v_lControlCount)  ' RAW 03/11/2003 : CQ2754/2862 : replaced with param value

            If Not Information.IsArray(oArray) Then
                Return nResult
            End If

            If Not Information.IsArray(oColumnOrder) Then
                Return nResult
            End If

            'The first one holds the key info...
            If oArray.GetUpperBound(1) = 0 Then
                Return nResult
            End If

            'This can get a little complicated.
            'Child details was used to set the column widths and captions
            'vArray has the values that go in to the list view, but we don't want all of them
            'and we need to worry about formatting.  This info comes from child details.
            'ltemp is the data row counter

            For lTemp As Integer = oArray.GetLowerBound(0) + 1 To oArray.GetUpperBound(0)
                nColumn = -1
                If CStr(oArray(lTemp, oArray.GetUpperBound(1))) <> "dElEtEd" Then
                    'First just add it...
                    oListItem = lvwListView(lListViewIndex).Items.Add("Wibble")
                    'Now find what columns we want...
                    For lTemp2 As Integer = (oColumnOrder).GetLowerBound(1) To (oColumnOrder).GetUpperBound(1)
                        m_lReturn = GetTag((m_vChildScreenDetails(ACDGISObjectId, (oColumnOrder(1, lTemp2)(0)))), (m_vChildScreenDetails(ACDGISPropertyId, (oColumnOrder(1, lTemp2)(0)))), nTag, m_vDataDictionary(GISDMTypeRisk), 0, 10)

                        oArray2 = oArray(lTemp, (oColumnOrder(1, lTemp2)(1)))

                        If Information.IsArray(oArray2) Then
                            oValue = oArray2(1)
                        Else
                            oValue = oArray2
                        End If

                        oArray2 = ""

                        Select Case m_vDataDictionary(GISDMTypeRisk)(uctRiskScreenControl.MainModule.ACPDataType, nTag Mod 10000)
                            Case iGISSharedConstants.GISDataTypeCurrency
                                m_lReturn = m_oFormFields.FormatControl(txtCurrency, oValue)
                                sString = txtCurrency.Text
                            Case iGISSharedConstants.GISDataTypeDate
                                m_lReturn = m_oFormFields.FormatControl(txtDate, oValue)

                                sString = txtDate.Text
                            Case iGISSharedConstants.GISDataTypeOption
                                Select Case ToSafeInteger(oValue)
                                    Case 0
                                        sString = "No"

                                    Case 1
                                        sString = "Yes"

                                    Case Else
                                        sString = "Unknown"
                                End Select

                            Case Else

                                If (m_vDataDictionary(GISDMTypeRisk)(ACPSpecialsType, nTag Mod 10000)) = ACOPartyTypeID Then
                                    'IH PN64528
                                    sString = oArray(lTemp, oColumnOrder(1, lTemp2)(1))
                                Else
                                    sString = (oValue)
                                End If

                        End Select

                        nColumn += 1

                        If nColumn = 0 Then
                            oListItem.Text = sString
                        Else
                            ListViewHelper.GetListViewSubItem(oListItem, nColumn).Text = sString
                        End If

                        'Set the tag of the column to the data type that is being stored into it.
                        lvwListView(lListViewIndex).Columns.Item("D" & nColumn).Tag = (m_vDataDictionary(GISDMTypeRisk)(uctRiskScreenControl.MainModule.ACPDataType, nTag Mod 10000))

                        'This really needs to give us a route to the child key...
                        oListItem.Tag = CStr(lTemp)

                    Next lTemp2

                End If

            Next lTemp

            'call the click event to sort the listview
            If bClear Or Information.IsArray(oUseSequenceId) Then
                If Not Information.IsArray(oUseSequenceId) Then
                    lvwListView_ColumnClick_Body(lListViewIndex, lvwListView(lListViewIndex).Columns.Item(0))
                Else
                    lvwListView_ColumnClick_Body(lListViewIndex, lvwListView(lListViewIndex).Columns.Item(((cmdListViewSequenceDown(lListViewIndex).Tag)) - 1), SortOrder.Ascending)
                End If
            ElseIf bClear = False Then
                lvwListView_ColumnClick_Body(CInt(lListViewIndex), lvwListView(lListViewIndex).Columns.Item(0))
            End If

            If Not bSuppressDynamicLogic Then
                ' RAW 22/06/2004 : Performance Changes(#2) : added v_vPropIdx param
                ' note - we cant really use a 'change' action here because we dont know whether anything has actually changed
                RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnChange, v_vDLAction:=enumOnChangeAction.eOnChangeAction_Refresh, v_vObjIdx:=lvwListView(lListViewIndex).Tag, v_vPropIdx:="", v_vControl:=lvwListView(lListViewIndex))
            End If


        Catch ex As Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "PopulateListView Failed", ACApp, ACClass, "PopulateListView", Information.Err().Number, Information.Err().Description, excep:=ex)



        Finally

            ListViewFunc.ListViewBatchEnd()

            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If


        End Try
        Return nResult
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddGeminiComboBox
    '
    ' Description:
    '
    ' History: 26/06/2000 Tomo - Created.
    ' RAW 09/07/2004 : JIT : added v_lScreenDetailsIndex param and used instead of m_cControlCount
    ' ***************************************************************** '
    Private Function AddGeminiComboBox(ByRef iFrameIndex As Integer, ByRef lTag As Integer, ByRef lX As Single, ByRef lY As Single, ByRef sPropertyName As String, ByRef vPMFormat As Object, ByRef vTabSetIndex As Object, ByVal v_lScreenDetailsIndex As Integer) As Integer

        Dim result As Integer = 0
        Dim lDebugDepthCounter As Integer
        Dim vArray() As Object
        Dim lControlIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = AddFrame(iFrameIndex, lTag, lX, lY, 0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                Return result
            End If

            'OK, so here it's a Gemini combo, and a label
            ' RAW 09/07/2004 : JIT : added
            If cboList.Length = 1 Then
                ' This is the first control of this type to be created so start at index 1
                ' - index 0 remains in its original state as a template for later controls of this type
                m_lGeminiComboIndex = 1
                ReDim m_vGeminiComboArray(ACCLastArrayPosition, m_lGeminiComboIndex)
            Else

                m_lGeminiComboIndex = cboList.Length
                ReDim Preserve m_vGeminiComboArray(ACCLastArrayPosition, m_lGeminiComboIndex)
            End If

            ' RAW 09/07/2004 : JIT : added
            ' store this index locally.
            ' Do not refer to the module level variable again from within this function as it may change
            ' if DL is triggered resulting in other controls being created, and thus the index being incremented,
            ' before this function has completed
            lControlIndex = m_lGeminiComboIndex

            If lControlIndex > 0 Then
                ' RAW 09/07/2004 : JIT : added
#If (DebugOption And m_klDebugOption_JIT) = m_klDebugOption_JIT Then


            AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="JIT : Adding GeminiComboBox (" & lControlIndex & ") - " & ObjectAndPropertyName(lTag))
#End If
                AddControl("cboList", lControlIndex)
                AddControl("lblList", lControlIndex)
            End If

            ' RAW 09/07/2004 : JIT : added
            ' store reference to this control in a collection - keyed on object and property name

            m_cObjectAndAttribute.Add(New Object() {cboList(lControlIndex), lblList(lControlIndex), "", v_lScreenDetailsIndex}, ObjectAndPropertyName(lTag).ToLower())

            m_oNewControlBeingLoaded = cboList(lControlIndex)  ' RAW 20/08/2004 : JIT : added


            m_vGeminiComboArray(ACCFrameNumber, lControlIndex) = iFrameIndex
            m_vGeminiComboArray(ACCIsDeleted, lControlIndex) = gPMConstants.PMEReturnCode.PMFalse
            m_vGeminiComboArray(ACCHelpText, lControlIndex) = m_sHelpText
            m_vGeminiComboArray(ACCPreQuote, lControlIndex) = m_lPreQuote
            m_vGeminiComboArray(ACCPostQuote, lControlIndex) = m_lPostQuote
            m_vGeminiComboArray(ACCPurchase, lControlIndex) = m_lPurchase
            m_vGeminiComboArray(ACCIsValuation, lControlIndex) = m_lIsValuation
            m_vGeminiComboArray(ACCIsRateAndPremium, lControlIndex) = m_lIsRateAndPremium
            m_vGeminiComboArray(ACCIncludeInList, lControlIndex) = m_lIncludeInList

            m_vGeminiComboArray(ACCChildId, lControlIndex) = DBNull.Value

            m_vGeminiComboArray(ACCGISObjectId, lControlIndex) = DBNull.Value
            m_vGeminiComboArray(ACCPMFormat, lControlIndex) = (vPMFormat)
            m_vGeminiComboArray(ACCTabSetIndex, lControlIndex) = (vTabSetIndex)

            cboList(lControlIndex).Parent = fraFrame(iFrameIndex)

            lblList(lControlIndex).Parent = fraFrame(iFrameIndex)


            lblList(lControlIndex).Top = (lY)
            lblList(lControlIndex).Left = (lX)
            lblList(lControlIndex).Visible = True
            lblList(lControlIndex).Text = sPropertyName
            lblList(lControlIndex).Tag = CStr(v_lScreenDetailsIndex)
            cboList(lControlIndex).Width = (m_lWidth)
            cboList(lControlIndex).Visible = True
            cboList(lControlIndex).Tag = CStr(lTag)
            cboList(lControlIndex).ToolTipText = m_sHelpText

            cboList(lControlIndex).PropertyId = (m_vDataDictionary(GISDMTypeRisk)(MainModule.ACPSpecialsTypeReference, lTag Mod 10000))

            'this call positions everything correctly
            textLabel_MouseMove("", MouseButtons.Left, g_lx, g_ly, cboList(lControlIndex), lblList(lControlIndex), pnlPosition, m_vTabArray(ACFTabSnapToGrid, CInt(m_vFrameArray(ACFTabNumber, iFrameIndex))))

            'hide label if ACBlankCaption
            SetBlankCaption(lblList(lControlIndex), False)

            'This forces the checking of the list versions
            cboList(lControlIndex).Login = True

            cboList(lControlIndex).DataModel = m_sGISDataModel

            'How can we tell?  Occupation is long, but gender is short...
            cboList(lControlIndex).LongList = True

            cboList(lControlIndex).Enabled = False

            lblList(lControlIndex).ForeColor = SystemColors.ControlText

            If m_lPreQuote = 2 Then
                lblList(lControlIndex).ForeColor = SystemColors.Highlight
            ElseIf (m_lPostQuote = 2) Then
                lblList(lControlIndex).ForeColor = SystemColors.HighlightText
            ElseIf (m_lPurchase = 2) Then
                lblList(lControlIndex).ForeColor = SystemColors.Info
            End If

            If Task <> gPMConstants.PMEComponentAction.PMView Then
                If (m_vDataDictionary(GISDMTypeRisk)(ACPIsInputProperty, lTag Mod 10000)) = 1 Then
                    cboList(lControlIndex).Enabled = True
                End If
            End If
            If Not IsNothing(m_vScreenValues) Then
                vArray = m_vScreenValues(v_lScreenDetailsIndex)
            End If
            'if launched from the test harness we do not get an array :o(
            If Not Information.IsArray(vArray) Then

                vArray = New Object() {0, 0}
                m_vScreenValues(v_lScreenDetailsIndex) = vArray
            End If
            cboList(lControlIndex).Text = CStr(vArray(0))

            ' now flag this in the array as having been created
            m_vScreenDetails(ACDScreenDetailCnt, v_lScreenDetailsIndex) = v_lScreenDetailsIndex  ' RAW 20/08/2004 : JIT : added
            m_oNewControlBeingLoaded = Nothing  ' RAW 20/08/2004 : JIT : added

            vArray = Nothing


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "AddGeminiComboBox Failed", ACApp, ACClass, "AddGeminiComboBox", Information.Err().Number, Information.Err().Description, excep:=ex)


        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddGISComboBox
    '
    ' Description:
    '
    ' History: 26/06/2000 Tomo - Created.
    ' RAW 09/07/2004 : JIT : added v_lScreenDetailsIndex param and used instead of m_cControlCount
    ' ***************************************************************** '
    Private Function AddGISComboBox(ByRef iFrameIndex As Integer, ByRef lTag As Integer, ByRef lX As Single, ByRef lY As Single, ByRef sPropertyName As String, ByRef vPMFormat As Object, ByRef vTabSetIndex As Object, ByVal v_lScreenDetailsIndex As Integer) As Integer

        Dim result As Integer = 0
        Dim lDebugDepthCounter As Integer
        Dim vArray() As Object
        Dim lControlIndex, lParentHeaderId As Integer

        On Error GoTo Err_AddGISComboBox

        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = AddFrame(iFrameIndex, lTag, lX, lY, 0)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn
            GoTo GetOutOfHere
        End If

        'OK, so here it's a GIS combo, and a label
        ' RAW 09/07/2004 : JIT : added
        If cboGISLookup.Length = 1 Then
            ' This is the first control of this type to be created so start at index 1
            ' - index 0 remains in its original state as a template for later controls of this type
            m_lGISComboIndex = 1
            ReDim m_vGISComboArray(ACCLastArrayPosition, m_lGISComboIndex)
        Else

            m_lGISComboIndex = cboGISLookup.Length
            ReDim Preserve m_vGISComboArray(ACCLastArrayPosition, m_lGISComboIndex)
        End If


        ' RAW 09/07/2004 : JIT : added
        ' store this index locally.
        ' Do not refer to the module level variable again from within this function as it may change
        ' if DL is triggered resulting in other controls being created, and thus the index being incremented,
        ' before this function has completed
        lControlIndex = m_lGISComboIndex

        If lControlIndex > 0 Then
            ' RAW 09/07/2004 : JIT : added
#If (DebugOption And m_klDebugOption_JIT) = m_klDebugOption_JIT Then

            AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="JIT : Adding GISComboBox (" & lControlIndex & ") - " & ObjectAndPropertyName(lTag))
#End If

            ' RAW 09/07/2004 : JIT : because index 0 is never used as a real control but is now reserved as a template,
            ' code has been removed that reset its values before creating the new control from it and restored it afterwards

            AddControl("cboGISLookup", lControlIndex)
            AddControl("lblGISLookup", lControlIndex)

        End If


        ' RAW 09/07/2004 : JIT : added
        ' store reference to this control in a collection - keyed on object and property name

        m_cObjectAndAttribute.Add(New Object() {cboGISLookup(lControlIndex), lblGISLookup(lControlIndex), "", v_lScreenDetailsIndex}, ObjectAndPropertyName(lTag).ToLower())

        m_oNewControlBeingLoaded = cboGISLookup(lControlIndex)  ' RAW 20/08/2004 : JIT : added


        ' RAW 17/06/2003 : added m_vDataDictionary and m_vScreenValues arguments
        SetInitialControlValues(lControlIndex, m_vGISComboArray, lblGISLookup, cboGISLookup, fraFrame, iFrameIndex, Task, vPMFormat, vTabSetIndex, 0, CInt(lX), CInt(lY), sPropertyName, lTag, v_lScreenDetailsIndex, False, m_lHeight, m_lWidth, m_sHelpText, m_lPreQuote, m_lPostQuote, m_lPurchase, m_lIsValuation, m_lIsRateAndPremium, m_lIncludeInList, m_lStage, m_oFormFields, v_lScreenDetailsIndex, (m_vDataDictionary(GISDMTypeRisk)), m_vScreenValues, ACPDataType:=MainModule.ACPDataType, ACPIsInputProperty:=MainModule.ACPIsInputProperty)
        ToolTip1.SetToolTip(cboGISLookup(lControlIndex), Convert.ToString(m_sHelpText))
        'this call positions everything correctly
        textLabel_MouseMove("", MouseButtons.Left, g_lx, g_ly, cboGISLookup(lControlIndex), lblGISLookup(lControlIndex), pnlPosition, m_vTabArray(ACFTabSnapToGrid, CInt(m_vFrameArray(ACFTabNumber, iFrameIndex))))
        vArray = m_vScreenValues(v_lScreenDetailsIndex)
        'if launched from the test harness we do not get an array :o(
        If Not Information.IsArray(vArray) Then
            vArray = New Object() {0, 0}
            m_vScreenValues(v_lScreenDetailsIndex) = vArray
        End If

        If CStr(vArray(0)) = "" Then vArray(0) = 0

        cboGISLookup(lControlIndex).GISDataModelCode = m_sGISDataModel

        'cboGISLookup(lControlIndex).Table = (m_vDataDictionary(GISDMTypeRisk)(ACPSpecialsTypeReference, lTag Mod 10000))
        cboGISLookup(lControlIndex).Table = (m_vDataDictionary(GISDMTypeRisk)(MainModule.ACPSpecialsTypeReference, lTag Mod 10000))
        Dim auxVar As Object = (m_vDataDictionary(GISDMTypeRisk)(ACUParent, lTag Mod 10000))

        If Convert.IsDBNull(auxVar) Or IsNothing(auxVar) Then
            cboGISLookup(lControlIndex).ParentHeaderId = 0
        Else

            cboGISLookup(lControlIndex).ParentHeaderId = CInt(IIf(CStr(m_vDataDictionary(GISDMTypeRisk)(ACUParent, lTag Mod 10000)) = "", 0, CStr(m_vDataDictionary(GISDMTypeRisk)(ACUParent, lTag Mod 10000))))
        End If

        'PR 11102002 - start PN issue 1007
        'Both .FirstItem and .RefreshList fire SQL so do one or the other

        'In View Mode no need to load the complete List
        If m_iTask = gPMConstants.PMEComponentAction.PMView Then
            cboGISLookup(lControlIndex).SingleItemId = CInt(vArray(0))
        End If

        If Not m_bRemoveNoneOption Then
            cboGISLookup(lControlIndex).FirstItem = "(None)"
        Else
            cboGISLookup(lControlIndex).RefreshList()
        End If
        'PR 11102002 - end

        ' check if this item has a parent which has already had a value defaulted
        ' before this control was loaded, so we can filter the items
        ' that are in the list as appropriate.
        If cboGISLookup(lControlIndex).ParentHeaderId > 0 Then

            ' for each combo that it to be created / has been created...
            For lTemp As Integer = m_vGISComboArray.GetLowerBound(1) To m_vGISComboArray.GetUpperBound(1)
                On Error Resume Next

                ' get the table id of the combo
                lParentHeaderId = cboGISLookup(lTemp).Table
                If Information.Err().Number > 0 Then
                    lParentHeaderId = -1
                End If

                ' if the table id of the combo matches the parent table id of the control being added
                If cboGISLookup(lControlIndex).ParentHeaderId = lParentHeaderId Then
                    ' if the item has had a default value applied
                    If cboGISLookup(lTemp).ItemId <> 0 Then
                        ' set the parent detail id to potentially filter the contents of this combo
                        cboGISLookup(lControlIndex).ParentDetailId = cboGISLookup(lTemp).ItemId
                        ' refresh the list with the filter applied
                        cboGISLookup(lControlIndex).RefreshList()
                    End If
                End If

            Next lTemp
        End If

        'don't set the value till after refresh or it will be lost
        cboGISLookup(lControlIndex).ItemId = CInt(vArray(0))

        ' now flag this in the array as having been created
        m_vScreenDetails(ACDScreenDetailCnt, v_lScreenDetailsIndex) = v_lScreenDetailsIndex  ' RAW 20/08/2004 : JIT : added
        m_oNewControlBeingLoaded = Nothing  ' RAW 20/08/2004 : JIT : added

        GoTo GetOutOfHere

Err_AddGISComboBox:

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "AddGISComboBox Failed", ACApp, ACClass, "AddGISComboBox", Information.Err().Number, Information.Err().Description)

        GoTo GetOutOfHere
        Resume

GetOutOfHere:   ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If
        'Decreasing Combobox width by 7px to match the designer width.
        cboGISLookup(lControlIndex).Width = cboGISLookup(lControlIndex).Width - 7

        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddPartyCommand
    '
    ' Description:
    '
    ' History: 26/06/2000 Tomo - Created.
    ' RAW 09/07/2004 : JIT : added v_lScreenDetailsIndex param and used instead of m_cControlCount
    ' ***************************************************************** '
    Private Function AddPartyCommand(ByRef iFrameIndex As Integer, ByRef lTag As Integer, ByRef lX As Single, ByRef lY As Single, ByRef sPropertyName As String, ByRef vPMFormat As Object, ByRef vTabSetIndex As Object, ByVal v_lScreenDetailsIndex As Integer) As Integer

        Dim result As Integer = 0
        Dim lDebugDepthCounter As Integer
        Dim vArray(,) As Object
        Dim lControlIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = AddFrame(iFrameIndex, lTag, lX, lY, 0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                Return result
            End If

            'OK, so here it's a command button, and a panel
            ' RAW 09/07/2004 : JIT : added
            If cmdPartyCommand.Length = 1 Then
                ' This is the first control of this type to be created so start at index 1
                ' - index 0 remains in its original state as a template for later controls of this type
                m_lPartyCommandIndex = 1
                ReDim m_vPartyCommandArray(ACCLastArrayPosition, m_lPartyCommandIndex)
            Else
                'm_lPartyCommandIndex = cmdPartyCommand.Length + 1
                m_lPartyCommandIndex = cmdPartyCommand.Length
                ReDim Preserve m_vPartyCommandArray(ACCLastArrayPosition, m_lPartyCommandIndex)
            End If

            ' RAW 09/07/2004 : JIT : added
            ' store this index locally.
            ' Do not refer to the module level variable again from within this function as it may change
            ' if DL is triggered resulting in other controls being created, and thus the index being incremented,
            ' before this function has completed
            lControlIndex = m_lPartyCommandIndex

            If lControlIndex > 0 Then
                ' RAW 09/07/2004 : JIT : added
#If (DebugOption And m_klDebugOption_JIT) = m_klDebugOption_JIT Then


            AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="JIT : Adding PartyCmd (" & lControlIndex & ") - " & ObjectAndPropertyName(lTag))
#End If

                AddControl("cmdPartyCommand", lControlIndex)
                AddControl("pnlPartyPanel", lControlIndex)
            End If

            ' RAW 09/07/2004 : JIT : added
            ' store reference to this control in a collection - keyed on object and property name

            m_cObjectAndAttribute.Add(New Object() {pnlPartyPanel(lControlIndex), cmdPartyCommand(lControlIndex), "", v_lScreenDetailsIndex}, ObjectAndPropertyName(lTag).ToLower())

            m_oNewControlBeingLoaded = cmdPartyCommand(lControlIndex)  ' RAW 20/08/2004 : JIT : added


            m_vPartyCommandArray(ACCFrameNumber, lControlIndex) = iFrameIndex
            m_vPartyCommandArray(ACCIsDeleted, lControlIndex) = gPMConstants.PMEReturnCode.PMFalse
            m_vPartyCommandArray(ACCHelpText, lControlIndex) = m_sHelpText
            m_vPartyCommandArray(ACCPreQuote, lControlIndex) = m_lPreQuote
            m_vPartyCommandArray(ACCPostQuote, lControlIndex) = m_lPostQuote
            m_vPartyCommandArray(ACCPurchase, lControlIndex) = m_lPurchase
            m_vPartyCommandArray(ACCIsValuation, lControlIndex) = m_lIsValuation
            m_vPartyCommandArray(ACCIsRateAndPremium, lControlIndex) = m_lIsRateAndPremium
            m_vPartyCommandArray(ACCIncludeInList, lControlIndex) = m_lIncludeInList

            m_vPartyCommandArray(ACCChildId, lControlIndex) = DBNull.Value

            m_vPartyCommandArray(ACCGISObjectId, lControlIndex) = DBNull.Value
            m_vPartyCommandArray(ACCPMFormat, lControlIndex) = (vPMFormat)
            m_vPartyCommandArray(ACCTabSetIndex, lControlIndex) = (vTabSetIndex)

            cmdPartyCommand(lControlIndex).Parent = fraFrame(iFrameIndex)



            cmdPartyCommand(lControlIndex).Top = (lY)
            cmdPartyCommand(lControlIndex).Left = (lX)
            cmdPartyCommand(lControlIndex).Visible = True
            cmdPartyCommand(lControlIndex).Tag = CStr(v_lScreenDetailsIndex)
            cmdPartyCommand(lControlIndex).Text = sPropertyName
            ToolTip1.SetToolTip(cmdPartyCommand(lControlIndex), m_sHelpText)

            '    m_vInUse(lTag) = PMTrue

            pnlPartyPanel(lControlIndex).Parent = fraFrame(iFrameIndex)

            pnlPartyPanel(lControlIndex).Top = (lY)
            pnlPartyPanel(lControlIndex).Left = (lX) + cmdPartyCommand(lControlIndex).Width
            pnlPartyPanel(lControlIndex).Visible = True
            pnlPartyPanel(lControlIndex).Text = sPropertyName
            'pnlPartyPanel(lControlIndex).ForeColor = Color.White


            'AddHandler cmdPartyCommand(lControlIndex).Click, AddressOf cmdPartyCommand_Click
            'AddHandler cmdPartyCommand(lControlIndex).Enter, AddressOf cmdPartyCommand_Enter

            cmdPartyCommand(lControlIndex).Enabled = False

            'RDT 14/06/2004 - CQ4153 - For the command buttons the size of the button doesn't change, however
            '                 the size of the panel does, so this is what we use.
            pnlPartyPanel(lControlIndex).Width = (m_lWidth)

            If Task <> gPMConstants.PMEComponentAction.PMView Then
                If (m_vDataDictionary(GISDMTypeRisk)(ACPIsInputProperty, lTag Mod 10000)) = 1 Then
                    cmdPartyCommand(lControlIndex).Enabled = True
                End If
            End If

            vArray = m_vScreenValues(v_lScreenDetailsIndex)

            pnlPartyPanel(lControlIndex).Tag = CStr(lTag)


            If Information.IsArray(vArray) Then
                pnlPartyPanel(lControlIndex).Text = CStr(vArray(1, 0))
            End If


            ' now flag this in the array as having been created
            m_vScreenDetails(ACDScreenDetailCnt, v_lScreenDetailsIndex) = v_lScreenDetailsIndex  ' RAW 20/08/2004 : JIT : added
            m_oNewControlBeingLoaded = Nothing  ' RAW 20/08/2004 : JIT : added


            vArray = Nothing


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "AddPartyCommand Failed", ACApp, ACClass, "AddPartyCommand", Information.Err().Number, Information.Err().Description, excep:=ex)


        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddPMLookup
    '
    ' Description:
    '
    ' History: 26/06/2000 Tomo - Created.
    ' RAW 09/07/2004 : JIT : added v_lScreenDetailsIndex param and used instead of m_cControlCount
    ' ***************************************************************** '
    Private Function AddPMLookup(ByRef iFrameIndex As Integer, ByRef lTag As Integer, ByRef lX As Single, ByRef lY As Single, ByRef sPropertyName As String, ByRef vPMFormat As Object, ByRef vTabSetIndex As Object, ByVal v_lScreenDetailsIndex As Integer) As Integer

        Dim result As Integer = 0
        Dim lDebugDepthCounter As Integer
        Dim sCaption As String = ""
        Dim vItemId, vValue As Object
        Dim lControlIndex As Integer
        Dim bValidUDL As Boolean
        Dim dtEffectiveDate As Object
        Dim vResults As Object



        result = gPMConstants.PMEReturnCode.PMTrue


        Try
            m_lReturn = AddFrame(iFrameIndex, lTag, lX, lY, 0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                GoTo GetOutOfHere
            End If

            'OK, so here it's a PM Lookup, and a label
            ' RAW 09/07/2004 : JIT : added
            If cboPMLookup.Length = 1 Then
                ' This is the first control of this type to be created so start at index 1
                ' - index 0 remains in its original state as a template for later controls of this type
                m_lPMLookupIndex = 1
                ReDim m_vPMLookupArray(ACCLastArrayPosition, m_lPMLookupIndex)


            Else
                m_lPMLookupIndex = cboPMLookup.Length + 1
                ReDim Preserve m_vPMLookupArray(ACCLastArrayPosition, m_lPMLookupIndex)

            End If

            ' RAW 09/07/2004 : JIT : added
            ' store this index locally.
            ' Do not refer to the module level variable again from within this function as it may change
            ' if DL is triggered resulting in other controls being created, and thus the index being incremented,
            ' before this function has completed
            lControlIndex = m_lPMLookupIndex

            If lControlIndex > 0 Then
                ' RAW 09/07/2004 : JIT : added
#If (DebugOption And m_klDebugOption_JIT) = m_klDebugOption_JIT Then


                AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="JIT : Adding PMLookup (" & lControlIndex & ") - " & ObjectAndPropertyName(lTag))
#End If

                ' RAW 09/07/2004 : JIT : because index 0 is never used as a real control but is now reserved as a template,
                ' code has been removed that reset its values before creating the new control from it and restored it afterwards

                AddControl("cboPMLookup", lControlIndex)
                AddControl("lblPMLookup", lControlIndex)

            End If

            ' RAW 09/07/2004 : JIT : added
            ' store reference to this control in a collection - keyed on object and property name

            m_cObjectAndAttribute.Add(New Object() {cboPMLookup(lControlIndex), lblPMLookup(lControlIndex), "", v_lScreenDetailsIndex}, ObjectAndPropertyName(lTag).ToLower())

            m_oNewControlBeingLoaded = cboPMLookup(lControlIndex)  ' RAW 20/08/2004 : JIT : added


            ' RAW 17/06/2003 : added m_vDataDictionary and m_vScreenValues arguments
            SetInitialControlValues(lControlIndex, m_vPMLookupArray, lblPMLookup, cboPMLookup, fraFrame, iFrameIndex, Task, vPMFormat, vTabSetIndex, 0, CInt(lX), CInt(lY), sPropertyName, lTag, v_lScreenDetailsIndex, False, m_lHeight, m_lWidth, m_sHelpText, m_lPreQuote, m_lPostQuote, m_lPurchase, m_lIsValuation, m_lIsRateAndPremium, m_lIncludeInList, m_lStage, m_oFormFields, v_lScreenDetailsIndex, (m_vDataDictionary(GISDMTypeRisk)), m_vScreenValues, ACPDataType:=uctRiskScreenControl.MainModule.ACPDataType, ACPIsInputProperty:=MainModule.ACPIsInputProperty)
            ToolTip1.SetToolTip(cboPMLookup(lControlIndex), Convert.ToString(m_sHelpText))
            'this call positions everything correctly
            textLabel_MouseMove("", MouseButtons.Left, g_lx, g_ly, cboPMLookup(lControlIndex), lblPMLookup(lControlIndex), pnlPosition, m_vTabArray(ACFTabSnapToGrid, CInt(m_vFrameArray(ACFTabNumber, iFrameIndex))))


            cboPMLookup(lControlIndex).TableName = (m_vDataDictionary(GISDMTypeRisk)(MainModule.ACPSpecialsTypeReference, lTag Mod 10000))
            cboPMLookup(lControlIndex).PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

            'if launched from the test harness we do not get an array :o(
            If Not Information.IsArray(m_vScreenValues(v_lScreenDetailsIndex)) Then
                m_vScreenValues(v_lScreenDetailsIndex) = New Object() {0, 0}
            End If

            DefaultPropHelper.SetDefaultProperty(vItemId, m_vScreenValues(v_lScreenDetailsIndex)(0))
            DefaultPropHelper.SetDefaultProperty(vValue, m_vScreenValues(v_lScreenDetailsIndex)(1))

            'disclosure types are always filtered by the risk type we are dealing with
            If cboPMLookup(lControlIndex).TableName.ToLower() = "disclosure_type" Then
                cboPMLookup(lControlIndex).WhereClause = "disclosure_type_id in (select disclosure_type_id from disclosure_type_risk_type where risk_type_id=" & m_vRiskTypeDetails(ACRRiskTypeId, 0) & ")"
            Else
                ' RAW 25/06/2003 : CQ1154 : added
                cboPMLookup(lControlIndex).WhereClause = ""
            End If

            'UDL Handling
            If m_sTransactionType = "C_CO" Or m_sTransactionType = "C_CR" Or m_sTransactionType = "C_CP" Then
                'Get the loss Date
                If m_lClaimID > 0 Then
                    m_lReturn = GetClaimDetails(m_lClaimID, vResults)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError("ADDPMLookup", "GetInsurancefileDetails Failed")
                        Exit Function
                    End If
                    If IsArray(vResults) Then
                        dtEffectiveDate = VB6.Support.Format(ToSafeDate(vResults(4, 0), Now), "MMM dd yyyy")

                    End If
                End If
            Else
                'For NB/MTA.. get the cover from Date
                If m_lInsuranceFileCnt > 0 Then
                    m_lReturn = GetInsuranceFileDetails(m_lInsuranceFileCnt, vResults)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError("ADDPMLookup", "GetInsurancefileDetails Failed")
                        Exit Function
                    End If
                    If IsArray(vResults) Then
                        dtEffectiveDate = VB6.Support.Format(ToSafeDate(vResults(2, 0), Now), "MMM dd yyyy")
                    End If

                End If
            End If
            If LCase(Mid(cboPMLookup(lControlIndex).TableName, 1, 4)) = "udl_" Then
                m_lReturn = CheckForValidUDL(cboPMLookup(lControlIndex).TableName, bValidUDL)
                If bValidUDL Then

                    If dtEffectiveDate = DateTime.MinValue Then
                        dtEffectiveDate = DateTime.Today
                    End If

                    cboPMLookup(lControlIndex).WhereClause = " udl_version = (select max(udl_version) from " + cboPMLookup(lControlIndex).TableName + " WHERE Effective_date <= '" & CDate(dtEffectiveDate).ToString("yyyyMMdd") & "') AND Effective_date <= '" & CDate(dtEffectiveDate).ToString("yyyyMMdd") & "'"
                End If
            End If

            'In View Mode no need to load the complete List
            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                'cboPMLookup(lControlIndex).SingleItemId = vItemId
            End If

            'trap a missing table
            'Both .FirstItem and .RefreshList fire SQL so do one or the other
            If Not m_bRemoveNoneOption Then
                cboPMLookup(lControlIndex).FirstItem = "(None)"
            Else
                cboPMLookup(lControlIndex).RefreshList()
            End If

            If Information.Err().Description <> "" Then
                cboPMLookup(lControlIndex).TableName = ""
                MessageBox.Show(Information.Err().Description, "Error adding PMLookup")
            Else
                If CStr(vItemId) = "" Then vItemId = 0
                cboPMLookup(lControlIndex).ItemId = (vItemId)
                m_vScreenValues(v_lScreenDetailsIndex) = New Object() {vItemId, vValue}
            End If

            ' now flag this in the array as having been created
            m_vScreenDetails(ACDScreenDetailCnt, v_lScreenDetailsIndex) = v_lScreenDetailsIndex
            m_oNewControlBeingLoaded = Nothing

            GoTo GetOutOfHere

Err_AddPMLookup:

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "AddPMLookup Failed", ACApp, ACClass, "AddPMLookup", Information.Err().Number, Information.Err().Description)

            GoTo GetOutOfHere


GetOutOfHere:   ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
            ' Debug message
#If (DebugOption) Then
            If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

            Return result

        Catch exc As System.Exception
        End Try
    End Function

    'for setting PMlookup value by find control
    Private Sub SetGISLookupValue(ByRef v_sItem As String, ByRef r_oControl As Object)
        Try
            For i As Integer = 1 To ReflectionHelper.GetMember(r_oControl, "ListCount")
                ReflectionHelper.SetMember(r_oControl, "ListIndex", i)
                If r_oControl.ItemCaption = v_sItem Then
                    Exit Sub
                End If
            Next i
        Catch
        End Try
    End Sub
    ''' <summary>
    ''' SetPMLValue
    ''' </summary>
    ''' <param name="v_sItem"></param>
    ''' <param name="r_oControl"></param>
    ''' <remarks></remarks>
    Private Sub SetPMLValue(ByRef v_sItem As String, ByRef r_oControl As Object)
        Dim nIndex As Integer

        m_bSuppressDropListClicks = True  'stop DL events firing

        If r_oControl IsNot Nothing AndAlso v_sItem IsNot Nothing AndAlso v_sItem.Length > 0 AndAlso ReflectionHelper.GetMember(r_oControl, "ListCount") > 0 Then
            nIndex = DirectCast(r_oControl, PMLookupControl.cboPMLookup).cboTypeTable.FindStringExact(v_sItem.ToString())
            If nIndex = -1 Then
                nIndex = 0
            Else
                m_bSuppressDropListClicks = False
            End If
        End If

        If nIndex = ReflectionHelper.GetMember(r_oControl, "ListCount") - 1 Then
            ReflectionHelper.SetMember(r_oControl, "ListIndex", 0)
        Else
            ReflectionHelper.SetMember(r_oControl, "ListIndex", nIndex)
        End If



    End Sub
    'for setting PMlookup value by find control
    Private Sub SetGISListValue(ByRef v_sItem As String, ByRef r_oControl As Object)
        Try

            For i As Integer = 1 To ReflectionHelper.GetMember(r_oControl, "ListCount")

                ReflectionHelper.SetMember(r_oControl, "ListIndex", i)

                If ReflectionHelper.GetMember(r_oControl, "Text") = v_sItem Then
                    Exit Sub
                End If
            Next i

        Catch
        End Try
    End Sub

    ' ***************************************************************** '
    '
    ' Name: AddPolicyCommand
    '
    ' Description:
    '
    ' History: 26/06/2000 Tomo - Created.
    ' RAW 09/07/2004 : JIT : added v_lScreenDetailsIndex param and used instead of m_cControlCount
    ' ***************************************************************** '
    Private Function AddPolicyCommand(ByRef iFrameIndex As Integer, ByRef lTag As Integer, ByRef lX As Single, ByRef lY As Single, ByRef sPropertyName As String, ByRef vPMFormat As Object, ByRef vTabSetIndex As Object, ByVal v_lScreenDetailsIndex As Integer) As Integer

        Dim result As Integer = 0
        Dim lDebugDepthCounter As Integer
        Dim vArray(,) As Object
        Dim lControlIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = AddFrame(iFrameIndex, lTag, lX, lY, 0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                Return result
            End If

            'OK, so here it's a command button, and a panel
            ' RAW 09/07/2004 : JIT : added
            If cmdPolicyCommand.Length = 1 Then
                ' This is the first control of this type to be created so start at index 1
                ' - index 0 remains in its original state as a template for later controls of this type
                m_lPolicyCommandIndex = 1
                ReDim m_vPolicyCommandArray(ACCLastArrayPosition, m_lPolicyCommandIndex)
            Else
                m_lPolicyCommandIndex = cmdPolicyCommand.Length + 1
                ReDim Preserve m_vPolicyCommandArray(ACCLastArrayPosition, m_lPolicyCommandIndex)
            End If

            ' RAW 09/07/2004 : JIT : added
            ' store this index locally.
            ' Do not refer to the module level variable again from within this function as it may change
            ' if DL is triggered resulting in other controls being created, and thus the index being incremented,
            ' before this function has completed
            lControlIndex = m_lPolicyCommandIndex

            If lControlIndex > 0 Then
                ' RAW 09/07/2004 : JIT : added
#If (DebugOption And m_klDebugOption_JIT) = m_klDebugOption_JIT Then


            AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="JIT : Adding PolicyCmd (" & lControlIndex & ") - " & ObjectAndPropertyName(lTag))
#End If
                AddControl("cmdPolicyCommand", lControlIndex)
                AddControl("pnlPolicyPanel", lControlIndex)
            End If

            ' RAW 09/07/2004 : JIT : added
            ' store reference to this control in a collection - keyed on object and property name

            m_cObjectAndAttribute.Add(New Object() {pnlPolicyPanel(lControlIndex), cmdPolicyCommand(lControlIndex), "", v_lScreenDetailsIndex}, ObjectAndPropertyName(lTag).ToLower())

            m_oNewControlBeingLoaded = cmdPolicyCommand(lControlIndex)  ' RAW 20/08/2004 : JIT : added


            m_vPolicyCommandArray(ACCFrameNumber, lControlIndex) = iFrameIndex
            m_vPolicyCommandArray(ACCIsDeleted, lControlIndex) = gPMConstants.PMEReturnCode.PMFalse
            m_vPolicyCommandArray(ACCHelpText, lControlIndex) = m_sHelpText
            m_vPolicyCommandArray(ACCPreQuote, lControlIndex) = m_lPreQuote
            m_vPolicyCommandArray(ACCPostQuote, lControlIndex) = m_lPostQuote
            m_vPolicyCommandArray(ACCPurchase, lControlIndex) = m_lPurchase
            m_vPolicyCommandArray(ACCIsValuation, lControlIndex) = m_lIsValuation
            m_vPolicyCommandArray(ACCIsRateAndPremium, lControlIndex) = m_lIsRateAndPremium
            m_vPolicyCommandArray(ACCIncludeInList, lControlIndex) = m_lIncludeInList

            m_vPolicyCommandArray(ACCChildId, lControlIndex) = DBNull.Value

            m_vPolicyCommandArray(ACCGISObjectId, lControlIndex) = DBNull.Value
            m_vPolicyCommandArray(ACCPMFormat, lControlIndex) = (vPMFormat)
            m_vPolicyCommandArray(ACCTabSetIndex, lControlIndex) = (vTabSetIndex)

            cmdPolicyCommand(lControlIndex).Parent = fraFrame(iFrameIndex)


            cmdPolicyCommand(lControlIndex).Top = (lY)
            cmdPolicyCommand(lControlIndex).Left = (lX)
            cmdPolicyCommand(lControlIndex).Visible = True
            cmdPolicyCommand(lControlIndex).Tag = CStr(v_lScreenDetailsIndex)
            cmdPolicyCommand(lControlIndex).Text = sPropertyName
            ToolTip1.SetToolTip(cmdPolicyCommand(lControlIndex), m_sHelpText)

            pnlPolicyPanel(lControlIndex).Parent = fraFrame(iFrameIndex)


            pnlPolicyPanel(lControlIndex).Top = (lY)
            pnlPolicyPanel(lControlIndex).Left = (lX) + cmdPolicyCommand(lControlIndex).Width
            pnlPolicyPanel(lControlIndex).Visible = True
            pnlPolicyPanel(lControlIndex).Text = sPropertyName

            cmdPolicyCommand(lControlIndex).Enabled = False

            'RDT 14/06/2004 - CQ4153 - For the command buttons the size of the button doesn't change, however
            '                 the size of the panel does, so this is what we use.
            pnlPolicyPanel(lControlIndex).Width = m_lWidth

            If Task <> gPMConstants.PMEComponentAction.PMView Then
                If (m_vDataDictionary(GISDMTypeRisk)(ACPIsInputProperty, lTag Mod 10000)) = 1 Then
                    cmdPolicyCommand(lControlIndex).Enabled = True
                End If
            End If

            vArray = m_vScreenValues(v_lScreenDetailsIndex)
            pnlPolicyPanel(lControlIndex).Tag = CStr(lTag)

            If Information.IsArray(vArray) Then
                pnlPolicyPanel(lControlIndex).Text = CStr(vArray(1, 0))
            End If

            vArray = Nothing


            ' now flag this in the array as having been created
            m_vScreenDetails(ACDScreenDetailCnt, v_lScreenDetailsIndex) = v_lScreenDetailsIndex  ' RAW 20/08/2004 : JIT : added
            m_oNewControlBeingLoaded = Nothing  ' RAW 20/08/2004 : JIT : added



        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "AddPolicyCommand Failed", ACApp, ACClass, "AddPolicyCommand", Information.Err().Number, Information.Err().Description, excep:=ex)


        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If


        End Try
        Return result
    End Function

    ''' <summary>
    ''' AddStandardWording
    ''' RAW 09/07/2004 : JIT : added v_lScreenDetailsIndex param and used instead of m_cControlCount
    ''' </summary>
    ''' <param name="iFrameIndex"></param>
    ''' <param name="lTag"></param>
    ''' <param name="lX"></param>
    ''' <param name="lY"></param>
    ''' <param name="bAddFrame"></param>
    ''' <param name="sPropertyName"></param>
    ''' <param name="vTabSetIndex"></param>
    ''' <param name="v_lScreenDetailsIndex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AddStandardWording(ByRef iFrameIndex As Integer, ByRef lTag As Integer, ByRef lX As Single, ByRef lY As Single, ByRef bAddFrame As Boolean, ByRef sPropertyName As String, ByRef vTabSetIndex As Object, ByVal v_lScreenDetailsIndex As Integer) As Integer
        Dim result As Integer = 0
        Dim lDebugDepthCounter As Integer
        Dim vArray(,) As Object
        Dim oListItem As ListViewItem
        Dim lControlIndex As Integer

        Dim oProduct As bSIRProduct.Business
        Dim lIsAllowedStandardWordingEdit As Integer
        Try

#If (DebugOption And m_klDebugOption_JIT) = m_klDebugOption_JIT Then
        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="JIT : Adding StandardWording - " & ObjectAndPropertyName(lTag))
#End If

            result = gPMConstants.PMEReturnCode.PMTrue

            If bAddFrame Then
                m_lReturn = AddFrame(iFrameIndex, lTag, lX, lY, 10000)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn

                    Return result
                End If
            End If

            ' RAW 09/07/2004 : JIT : added
            If lvwStandardWording.Length = 1 Then
                ' This is the first control of this type to be created so start at index 1
                ' - index 0 remains in its original state as a template for later controls of this type
                m_lStandardWordingIndex = 1
                ReDim m_vStandardWordingArray(ACCLastArrayPosition, m_lStandardWordingIndex)
            Else
                m_lStandardWordingIndex = lvwStandardWording.Length
                ReDim Preserve m_vStandardWordingArray(ACCLastArrayPosition, m_lStandardWordingIndex)
            End If

            ' Do not refer to the module level variable again from within this function as it may change
            ' if DL is triggered resulting in other controls being created, and thus the index being incremented,
            ' before this function has completed
            lControlIndex = m_lStandardWordingIndex

            If lControlIndex > 0 Then
#If (DebugOption And m_klDebugOption_JIT) = m_klDebugOption_JIT Then


            AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="JIT : Adding StandardWording (" & lControlIndex & ") - " & ObjectAndPropertyName(r_vObjIdx:=lTag, r_PropIdx_optional:=""))
#End If
                AddControl("lvwStandardWording", lControlIndex)
                AddControl("cmdStandardWordingAdd", lControlIndex)
                AddControl("cmdStandardWordingEdit", lControlIndex)
                AddControl("cmdStandardWordingDelete", lControlIndex)
                AddControl("cmdStandardWordingUp", lControlIndex)
                AddControl("cmdStandardWordingDown", lControlIndex)
                AddControl("lblStandardWordingMove", lControlIndex)

            End If


            ' RAW 09/07/2004 : JIT : added
            ' store reference to this control in a collection - keyed on object and property name

            m_cObjectAndAttribute.Add(New Object() {fraFrame(iFrameIndex), lvwStandardWording(lControlIndex), "", v_lScreenDetailsIndex}, ObjectAndPropertyName(lTag).ToLower())

            m_oNewControlBeingLoaded = lvwStandardWording(lControlIndex)

            For colIndex As Integer = 0 To lvwStandardWording(0).Columns.Count - 1
                lvwStandardWording(lControlIndex).Columns.Add(lvwStandardWording(0).Columns(colIndex).Text, lvwStandardWording(0).Columns(colIndex).Width, lvwStandardWording(0).Columns(colIndex).TextAlign)
            Next
            lvwStandardWording(lControlIndex).View = View.Details

            lvwStandardWording(lControlIndex).Parent = fraFrame(iFrameIndex)
            cmdStandardWordingAdd(lControlIndex).Parent = fraFrame(iFrameIndex)
            cmdStandardWordingEdit(lControlIndex).Parent = fraFrame(iFrameIndex)
            cmdStandardWordingDelete(lControlIndex).Parent = fraFrame(iFrameIndex)
            cmdStandardWordingUp(lControlIndex).Parent = fraFrame(iFrameIndex)
            cmdStandardWordingDown(lControlIndex).Parent = fraFrame(iFrameIndex)
            lblStandardWordingMove(lControlIndex).Parent = fraFrame(iFrameIndex)

            m_vFrameArray(ACFChildId, iFrameIndex) = m_vScreenDetails(ACDChildScreenId, v_lScreenDetailsIndex)
            m_vFrameArray(ACFGISObjectId, iFrameIndex) = m_vScreenDetails(ACDGISObjectId, v_lScreenDetailsIndex)

            m_vStandardWordingArray(ACCFrameNumber, lControlIndex) = iFrameIndex
            m_vStandardWordingArray(ACCIsDeleted, lControlIndex) = gPMConstants.PMEReturnCode.PMFalse
            m_vStandardWordingArray(ACCHelpText, lControlIndex) = m_vScreenDetails(ACDHelpText, v_lScreenDetailsIndex)

            m_vStandardWordingArray(ACCIncludeInList, lControlIndex) = DBNull.Value
            m_vStandardWordingArray(ACCChildId, lControlIndex) = m_vScreenDetails(ACDChildScreenId, v_lScreenDetailsIndex)
            m_vStandardWordingArray(ACCGISObjectId, lControlIndex) = m_vScreenDetails(ACDGISObjectId, v_lScreenDetailsIndex)
            m_vStandardWordingArray(ACCPMFormat, lControlIndex) = m_vScreenDetails(ACDPMFormat, v_lScreenDetailsIndex)
            m_vStandardWordingArray(ACCTabSetIndex, lControlIndex) = m_vScreenDetails(ACDTabSetIndex, v_lScreenDetailsIndex)


            lvwStandardWording(lControlIndex).Top = VB6.TwipsToPixelsY(240)
            lvwStandardWording(lControlIndex).Left = VB6.TwipsToPixelsX(120)
            lvwStandardWording(lControlIndex).Height = fraFrame(iFrameIndex).Height - VB6.TwipsToPixelsY(780)
            lvwStandardWording(lControlIndex).Width = fraFrame(iFrameIndex).Width - VB6.TwipsToPixelsX(795)

            cmdStandardWordingAdd(lControlIndex).Top = fraFrame(iFrameIndex).Height - VB6.TwipsToPixelsY(420)
            cmdStandardWordingAdd(lControlIndex).Left = VB6.TwipsToPixelsX(120)
            cmdStandardWordingAdd(lControlIndex).Tag = CStr(v_lScreenDetailsIndex)


            cmdStandardWordingEdit(lControlIndex).Top = fraFrame(iFrameIndex).Height - VB6.TwipsToPixelsY(420)
            cmdStandardWordingEdit(lControlIndex).Left = VB6.TwipsToPixelsX(1320)
            cmdStandardWordingEdit(lControlIndex).Tag = CStr(v_lScreenDetailsIndex)

            cmdStandardWordingDelete(lControlIndex).Top = cmdStandardWordingAdd(lControlIndex).Top
            cmdStandardWordingDelete(lControlIndex).Left = VB6.TwipsToPixelsX(2520)
            cmdStandardWordingDelete(lControlIndex).Tag = CStr(v_lScreenDetailsIndex)

            cmdStandardWordingUp(lControlIndex).Left = fraFrame(iFrameIndex).Width - VB6.TwipsToPixelsX(615)
            cmdStandardWordingUp(lControlIndex).Top = lvwStandardWording(lControlIndex).Top

            cmdStandardWordingDown(lControlIndex).Left = cmdStandardWordingUp(lControlIndex).Left
            cmdStandardWordingDown(lControlIndex).Top = lvwStandardWording(lControlIndex).Top + lvwStandardWording(lControlIndex).Height - cmdStandardWordingDown(lControlIndex).Height

            lblStandardWordingMove(lControlIndex).Left = cmdStandardWordingUp(lControlIndex).Left
            lblStandardWordingMove(lControlIndex).Top = (cmdStandardWordingUp(lControlIndex).Top + cmdStandardWordingDown(lControlIndex).Top + lblStandardWordingMove(lControlIndex).Height) / 2

            lvwStandardWording(lControlIndex).Visible = True
            cmdStandardWordingAdd(lControlIndex).Visible = True
            cmdStandardWordingEdit(lControlIndex).Visible = True
            cmdStandardWordingDelete(lControlIndex).Visible = True
            cmdStandardWordingUp(lControlIndex).Visible = True
            cmdStandardWordingDown(lControlIndex).Visible = True
            lblStandardWordingMove(lControlIndex).Visible = True

            cmdStandardWordingAdd(lControlIndex).Enabled = False
            cmdStandardWordingEdit(lControlIndex).Enabled = False
            cmdStandardWordingDelete(lControlIndex).Enabled = False

            cmdStandardWordingUp(lControlIndex).Enabled = False
            cmdStandardWordingDown(lControlIndex).Enabled = False


            If Task <> gPMConstants.PMEComponentAction.PMView Then
                If (m_vDataDictionary(GISDMTypeRisk)(ACPIsInputProperty, lTag Mod 10000)) = 1 Then
                    cmdStandardWordingAdd(lControlIndex).Enabled = True
                    cmdStandardWordingUp(lControlIndex).Enabled = True
                    cmdStandardWordingDown(lControlIndex).Enabled = True

                    ' To ensure cmdStandardWordingEdit button should be visible
                    ' Get an instance of the Product Business object via
                    ' the public object manager.
                    ' For Broking we don't do this check as the Product table is not used  PN22739
                    If m_vUnderwritingOrAgency <> "U" Then
                        lIsAllowedStandardWordingEdit = 1
                    Else
                        Dim temp_oProduct As Object
                        m_lReturn = g_oObjectManager.GetInstance(temp_oProduct, "bSIRProduct.Business", PMGetViaClientManager)
                        oProduct = temp_oProduct
                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to get Product Business  object", ACApp, ACClass, "AddStandardWording", Information.Err().Number, Information.Err().Description)

                            Return result
                        End If


                        'm_lReturn = ReflectionHelper.Invoke(oProduct, "IsAllowedStandardWordingEdit", New Object() {m_lProductId, lIsAllowedStandardWordingEdit})
                        m_lReturn = oProduct.IsAllowedStandardWordingEdit(m_lProductId, lIsAllowedStandardWordingEdit)
                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to get IsAllowedStandardWordingEdit", ACApp, ACClass, "AddStandardWording", Information.Err().Number, Information.Err().Description)

                            Return result
                        End If


                        oProduct.Dispose()
                        oProduct = Nothing
                    End If

                    If lIsAllowedStandardWordingEdit = 0 Then  'Editing is not allowed
                        cmdStandardWordingEdit(lControlIndex).Visible = False
                        cmdStandardWordingDelete(lControlIndex).Left = cmdStandardWordingEdit(lControlIndex).Left
                    End If

                End If
            Else
                cmdStandardWordingAdd(lControlIndex).Visible = False
                cmdStandardWordingDelete(lControlIndex).Visible = False
                cmdStandardWordingEdit(lControlIndex).Text = "View"
            End If

            lvwStandardWording(lControlIndex).Tag = CStr(lTag)
            vArray = m_vScreenValues(v_lScreenDetailsIndex)

            If m_bDefaultClausesForRisk = True AndAlso Information.IsArray(vArray) = False Then

                cmdStandardWordingAdd(m_lStandardWordingIndex).Tag = v_lScreenDetailsIndex
                m_lReturn = PopulateDefaultClausesForRisk(lControlIndex, lTag)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError("GetBusiness", "PopulateDefaultClausesForRisk method failed to populate the Default Clasues", SharedFiles.GeneralConst.PMLogError)
                End If
            ElseIf Information.IsArray(vArray) Then
                For lTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)
                    oListItem = lvwStandardWording(lControlIndex).Items.Add(CStr(vArray(1, lTemp)))
                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(vArray(2, lTemp))
                    If "" & CStr(vArray(3, lTemp)) = "1" Then
                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Yes"
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = ""
                    End If
                    oListItem.Tag = CStr(vArray(0, lTemp))
                Next lTemp
            End If

            m_lReturn = SetExtraListViewProperties(lvwStandardWording(lControlIndex).Handle.ToInt32(), True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If
            ' now flag this in the array as having been created
            m_vScreenDetails(ACDScreenDetailCnt, v_lScreenDetailsIndex) = v_lScreenDetailsIndex  ' RAW 20/08/2004 : JIT : added
            m_oNewControlBeingLoaded = Nothing  ' RAW 20/08/2004 : JIT : added


            vArray = Nothing


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "AddStandardWording Failed", ACApp, ACClass, "AddStandardWording", Information.Err().Number, Information.Err().Description, excep:=ex)


        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1) ' decrement the counter
#End If


        End Try
        Return result
    End Function
    Private Function PopulateDefaultClausesForRisk(ByVal lIndex As Long, ByVal lTag As Long) As Long

        Dim lCount As Integer
        Dim oListItem As Object
        Dim vDataTypes As Object
        Dim vArray As Object
        ReDim vArray(ACSelectClauseArrayIndex, 0)
        Const kMethodName As String = "PopulateDefaultClausesForRisk"


        Try

            PopulateDefaultClausesForRisk = gPMConstants.PMEReturnCode.PMTrue

            If IsArray(m_vDefaultClausesForRisk) = False Then
                Exit Function
            End If

            ' lvwStandardWording(lIndex).ListItems.Clear()
            lvwStandardWording(lIndex).Items.Clear()
            For lCount = LBound(m_vDefaultClausesForRisk, ACSelectClauseRowIndex) To UBound(m_vDefaultClausesForRisk, ACSelectClauseRowIndex)
                oListItem = lvwStandardWording(lIndex).Items.Add(m_vDefaultClausesForRisk(ACSelectClauseCode, lCount))

                oListItem.SubItems.add(ACSelectClauseCode).text = m_vDefaultClausesForRisk(ACSelectClauseDescription, lCount)
                oListItem.SubItems.add(ACSelectClauseDescription).text = " "
                oListItem.Tag = m_vDefaultClausesForRisk(ACSelectClauseId, lCount)
                ReDim Preserve vArray(ACSelectClauseArrayIndex, lCount)

                vArray(ACSelectClauseId, lCount) = m_vDefaultClausesForRisk(ACSelectClauseId, lCount)
                vArray(ACSelectClauseDescription, lCount) = m_vDefaultClausesForRisk(ACSelectClauseCode, lCount)
                vArray(ACSelectClauseArrayIndex, lCount) = m_vDefaultClausesForRisk(ACSelectClauseDescription, lCount)

                ReDim vDataTypes(ACSelectClauseArrayIndex)
                vDataTypes(ACSelectClauseId) = SharedFiles.GISSharedConstants.GISDataTypeText
                vDataTypes(ACSelectClauseDescription) = SharedFiles.GISSharedConstants.GISDataTypeText
                vDataTypes(ACSelectClauseArrayIndex) = SharedFiles.GISSharedConstants.GISDataTypeText

                SetScreenValues(cmdStandardWordingAdd(lIndex).Tag,
                                            vArray,
                                            lTag,
                                            ACObjectIndex,
                                            v_vDataTypes:=vDataTypes)


                If m_bFireDLStartEvents = True Then

                    RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_Start, v_vObjIdx:=lvwStandardWording(lIndex).Tag, v_vControl:=cmdStandardWordingAdd(lIndex))

                End If
            Next lCount




        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=PopulateDefaultClausesForRisk, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Set the mouse pointer to normal.
            ' SetMousePointer(PMMouseNormal)

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: AddSumInsured
    '
    ' Description:
    '
    ' History: 26/06/2000 Tomo - Created.
    ' RAW 09/07/2004 : JIT : added v_lScreenDetailsIndex param and used instead of m_cControlCount
    ' ***************************************************************** '
    Private Function AddSumInsured(ByVal iFrameIndex As Integer, ByRef lTag As Integer, ByRef lX As Single, ByRef lY As Single, ByRef sPropertyName As String, ByRef bAddFrame As Boolean, ByRef vPMFormat As Object, ByRef vTabSetIndex As Object, ByVal v_lScreenDetailsIndex As Integer) As Integer

        Dim result As Integer = 0
        Dim lDebugDepthCounter, lOffset, lWidth As Integer
        Dim vArray(,) As Object
        Dim oListItem As ListViewItem
        Dim cSumInsured As Decimal
        Dim lControlIndex As Integer
        Dim cRate, cPremium As Decimal

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If bAddFrame Then
                m_lReturn = AddFrame(iFrameIndex, lTag, lX, lY, 10000)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn

                    Return result
                End If
            End If

            ' RAW 09/07/2004 : JIT : added
            If lvwSumInsured.Length = 1 Then
                ' This is the first control of this type to be created so start at index 1
                ' - index 0 remains in its original state as a template for later controls of this type
                m_lSumInsuredIndex = 1
                ReDim m_vSumInsuredArray(ACCLastArrayPosition, m_lSumInsuredIndex)
            Else
                m_lSumInsuredIndex = lvwSumInsured.Length
                ReDim Preserve m_vSumInsuredArray(ACCLastArrayPosition, m_lSumInsuredIndex)
            End If

            ' RAW 09/07/2004 : JIT : added
            ' store this index locally.
            ' Do not refer to the module level variable again from within this function as it may change
            ' if DL is triggered resulting in other controls being created, and thus the index being incremented,
            ' before this function has completed
            lControlIndex = m_lSumInsuredIndex

            If lControlIndex > 0 Then
                ' RAW 09/07/2004 : JIT : added
#If (DebugOption And m_klDebugOption_JIT) = m_klDebugOption_JIT Then

            AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="JIT : Adding SumInsured (" & lControlIndex & ") - " & ObjectAndPropertyName(lTag))
#End If

                AddControl("lvwSumInsured", lControlIndex)
                AddControl("cmdSumInsuredAdd", lControlIndex)
                AddControl("cmdSumInsuredDelete", lControlIndex)
                AddControl("cmdSumInsuredEdit", lControlIndex)
                AddControl("lblTotalSumInsured", lControlIndex)
                AddControl("pnlTotalSumInsured", lControlIndex)
                AddControl("lblRate", lControlIndex)
                AddControl("txtRate", lControlIndex)
                AddControl("lblPremium", lControlIndex)
                AddControl("txtPremium", lControlIndex)

            End If

            ' RAW 09/07/2004 : JIT : added
            ' store reference to this control in a collection - keyed on object and property name

            m_cObjectAndAttribute.Add(New Object() {fraFrame(iFrameIndex), lvwSumInsured(lControlIndex), "", v_lScreenDetailsIndex}, ObjectAndPropertyName(lTag).ToLower())

            m_oNewControlBeingLoaded = lvwSumInsured(lControlIndex)  ' RAW 20/08/2004 : JIT : added
            For colIndex As Integer = 0 To lvwSumInsured(0).Columns.Count - 1
                lvwSumInsured(lControlIndex).Columns.Add(lvwSumInsured(0).Columns(colIndex).Text, lvwSumInsured(0).Columns(colIndex).Width, lvwSumInsured(0).Columns(colIndex).TextAlign)
            Next
            lvwSumInsured(lControlIndex).View = View.Details


            m_vSumInsuredArray(ACCFrameNumber, lControlIndex) = iFrameIndex
            m_vSumInsuredArray(ACCIsDeleted, lControlIndex) = gPMConstants.PMEReturnCode.PMFalse
            m_vSumInsuredArray(ACCHelpText, lControlIndex) = m_sHelpText
            m_vSumInsuredArray(ACCPreQuote, lControlIndex) = m_lPreQuote
            m_vSumInsuredArray(ACCPostQuote, lControlIndex) = m_lPostQuote
            m_vSumInsuredArray(ACCPurchase, lControlIndex) = m_lPurchase
            m_vSumInsuredArray(ACCIsValuation, lControlIndex) = m_lIsValuation
            m_vSumInsuredArray(ACCIsRateAndPremium, lControlIndex) = m_lIsRateAndPremium
            m_vSumInsuredArray(ACCIncludeInList, lControlIndex) = m_lIncludeInList

            m_vSumInsuredArray(ACCChildId, lControlIndex) = DBNull.Value

            m_vSumInsuredArray(ACCGISObjectId, lControlIndex) = DBNull.Value
            m_vSumInsuredArray(ACCPMFormat, lControlIndex) = (vPMFormat)
            m_vSumInsuredArray(ACCTabSetIndex, lControlIndex) = (vTabSetIndex)

            lvwSumInsured(lControlIndex).Parent = fraFrame(iFrameIndex)
            cmdSumInsuredAdd(lControlIndex).Parent = fraFrame(iFrameIndex)
            cmdSumInsuredDelete(lControlIndex).Parent = fraFrame(iFrameIndex)
            cmdSumInsuredEdit(lControlIndex).Parent = fraFrame(iFrameIndex)
            lblTotalSumInsured(lControlIndex).Parent = fraFrame(iFrameIndex)
            pnlTotalSumInsured(lControlIndex).Parent = fraFrame(iFrameIndex)
            lblRate(lControlIndex).Parent = fraFrame(iFrameIndex)
            txtRate(lControlIndex).Parent = fraFrame(iFrameIndex)
            lblPremium(lControlIndex).Parent = fraFrame(iFrameIndex)
            txtPremium(lControlIndex).Parent = fraFrame(iFrameIndex)

            If m_lIsRateAndPremium = gPMConstants.PMEReturnCode.PMTrue Then
                lOffset = 420
            End If

            If m_lIsValuation = gPMConstants.PMEReturnCode.PMTrue Then
                lWidth = 1440

            End If

            lvwSumInsured(lControlIndex).Left = VB6.TwipsToPixelsX(120)
            lvwSumInsured(lControlIndex).Width = fraFrame(iFrameIndex).Width - VB6.TwipsToPixelsX(240)
            lvwSumInsured(lControlIndex).Top = VB6.TwipsToPixelsY(240)
            lvwSumInsured(lControlIndex).Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(fraFrame(iFrameIndex).Height) - 720 - lOffset)
            lvwSumInsured(lControlIndex).Visible = True
            lvwSumInsured(lControlIndex).Tag = CStr(lTag)

            cmdSumInsuredAdd(lControlIndex).Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(fraFrame(iFrameIndex).Height) - 420 - lOffset)
            cmdSumInsuredAdd(lControlIndex).Left = VB6.TwipsToPixelsX(120)
            cmdSumInsuredAdd(lControlIndex).Visible = True
            cmdSumInsuredAdd(lControlIndex).Tag = CStr(v_lScreenDetailsIndex)

            cmdSumInsuredEdit(lControlIndex).Top = cmdSumInsuredAdd(lControlIndex).Top
            cmdSumInsuredEdit(lControlIndex).Left = VB6.TwipsToPixelsX(1320)
            cmdSumInsuredEdit(lControlIndex).Visible = True

            cmdSumInsuredDelete(lControlIndex).Top = cmdSumInsuredAdd(lControlIndex).Top
            cmdSumInsuredDelete(lControlIndex).Left = VB6.TwipsToPixelsX(2520)
            cmdSumInsuredDelete(lControlIndex).Visible = True

            pnlTotalSumInsured(lControlIndex).Top = cmdSumInsuredAdd(lControlIndex).Top
            pnlTotalSumInsured(lControlIndex).Left = lvwSumInsured(lControlIndex).Left + lvwSumInsured(lControlIndex).Width - pnlTotalSumInsured(lControlIndex).Width
            pnlTotalSumInsured(lControlIndex).Visible = True


            lblTotalSumInsured(lControlIndex).Top = cmdSumInsuredAdd(lControlIndex).Top + 3
            lblTotalSumInsured(lControlIndex).Left = pnlTotalSumInsured(lControlIndex).Left - lblTotalSumInsured(lControlIndex).Width
            lblTotalSumInsured(lControlIndex).Visible = True


            If m_lIsRateAndPremium = gPMConstants.PMEReturnCode.PMTrue Then
                lblRate(lControlIndex).Top = lblTotalSumInsured(lControlIndex).Top + 28
                lblRate(lControlIndex).Left = 8
                lblRate(lControlIndex).Visible = True

                txtRate(lControlIndex).Top = pnlTotalSumInsured(lControlIndex).Top + 28

                'txtRate(lControlIndex).Left = lblRate(lControlIndex).Left + lblRate(lControlIndex).Width + 45 ' VB6.TwipsToPixelsX(1440)
                txtRate(lControlIndex).Left = 96
                txtRate(lControlIndex).Visible = True

                lblPremium(lControlIndex).Top = lblRate(lControlIndex).Top
                lblPremium(lControlIndex).Left = lblTotalSumInsured(lControlIndex).Left
                lblPremium(lControlIndex).Visible = True

                txtPremium(lControlIndex).Top = txtRate(lControlIndex).Top
                txtPremium(lControlIndex).Left = pnlTotalSumInsured(lControlIndex).Left
                txtPremium(lControlIndex).Visible = True
            Else
                lblRate(lControlIndex).Visible = False
                txtRate(lControlIndex).Visible = False
                lblPremium(lControlIndex).Visible = False
                txtPremium(lControlIndex).Visible = False
            End If
            'NIIT comment: Following code is written to hide(Valuation)/Show(all) column in suminsured
            If m_lIsValuation = gPMConstants.PMEReturnCode.PMTrue Then
                For Each header As ColumnHeader In lvwSumInsured(lControlIndex).Columns
                    header.Width = CInt(VB6.TwipsToPixelsX(lWidth))
                Next
            Else
                For i As Integer = 5 To lvwSumInsured(lControlIndex).Columns.Count - 1
                    lvwSumInsured(lControlIndex).Columns(i).Width = CInt(VB6.TwipsToPixelsX(lWidth))
                Next
            End If
            'NIIT comment
            'ToDo
            'CMG/PB 06092002 Bug fix 638, This is already set correctly above.

            m_lReturn = SetExtraListViewProperties(lvwSumInsured(lControlIndex).Handle.ToInt32(), True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            'cmdSumInsuredAdd(lControlIndex).Enabled = False
            'cmdSumInsuredDelete(lControlIndex).Enabled = False
            'cmdSumInsuredEdit(lControlIndex).Enabled = False

            If Task <> gPMConstants.PMEComponentAction.PMView Then
                If (m_vDataDictionary(GISDMTypeRisk)(ACPIsInputProperty, lTag Mod 10000)) = 1 Then
                    cmdSumInsuredAdd(lControlIndex).Enabled = True
                End If
            End If

            ' RVH 09/12/2004 - Merged from 1.8.6
            m_lReturn = m_oFormFields.AddNewFormField(txtRate(lControlIndex), gPMConstants.PMEFormatStyle.PMFormatPercent, gPMConstants.PMEDataType.PMCurrency, lControlIndex, gPMConstants.PMEMandatoryStatus.PMNonMandatory, , -4)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            m_lReturn = m_oFormFields.AddNewFormField(txtPremium(lControlIndex), gPMConstants.PMEFormatStyle.PMFormatCurrency, gPMConstants.PMEDataType.PMCurrency, lControlIndex, gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            '    If (lControlIndex = 0) Then
            '        m_lReturn& = m_oFormFields.AddNewFormField( _
            'ctlControl:=txtCurrency, _
            'lFieldType:=PMCurrency, _
            'lFormat:=PMFormatCurrency, _
            'lMandatory:=PMNonMandatory)

            '        If (m_lReturn <> PMTrue) Then
            '            AddSumInsured = PMFalse
            '            goto GetOutOfHere
            '        End If

            '        m_lReturn& = m_oFormFields.AddNewFormField( _
            'ctlControl:=txtDate, _
            'lFieldType:=PMDate, _
            'lFormat:=PMFormatDateLong, _
            'lMandatory:=PMNonMandatory)

            '        If (m_lReturn <> PMTrue) Then
            '            AddSumInsured = PMFalse
            '            goto GetOutOfHere
            '        End If
            '    End If

            vArray = m_vScreenValues(v_lScreenDetailsIndex)


            'TN20001211 - Start
            'make sure controls are refreshed before loading it
            txtRate(lControlIndex).Text = ""
            txtPremium(lControlIndex).Text = ""
            lvwSumInsured(lControlIndex).Items.Clear()
            'TN20001211 - End

            cSumInsured = 0

            If Information.IsArray(vArray) Then

                For lTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)


                    If Not (Convert.IsDBNull(vArray(m_klSumInsuredArrayColNo_Rate, lTemp)) Or IsNothing(vArray(m_klSumInsuredArrayColNo_Rate, lTemp))) Then
                        m_lReturn = m_oFormFields.FormatControlArray(txtRate(lControlIndex), vArray(m_klSumInsuredArrayColNo_Rate, lTemp), GetIndex(txtRate(lControlIndex), txtRate))
                        txtPremium(lControlIndex).Enabled = False
                    End If



                    If Not (Convert.IsDBNull(vArray(m_klSumInsuredArrayColNo_Premium, lTemp)) Or IsNothing(vArray(m_klSumInsuredArrayColNo_Premium, lTemp))) Then
                        m_lReturn = m_oFormFields.FormatControlArray(txtPremium(lControlIndex), vArray(m_klSumInsuredArrayColNo_Premium, lTemp), GetIndex(txtPremium(lControlIndex), txtPremium))
                    End If

                    If CStr(vArray(m_klSumInsuredArrayColNo_Description, lTemp)) <> "" Then

                        oListItem = lvwSumInsured(lControlIndex).Items.Add(CStr(vArray(m_klSumInsuredArrayColNo_Description, lTemp)))
                        'oListItem.Tag = v_lScreenDetailsIndex
                        ListViewHelper.GetListViewSubItem(oListItem, m_klSumInsuredListColNo_Reference).Text = CStr(vArray(m_klSumInsuredArrayColNo_Reference, lTemp))


                        If Not (Convert.IsDBNull(vArray(m_klSumInsuredArrayColNo_SumInsured, lTemp)) Or IsNothing(vArray(m_klSumInsuredArrayColNo_SumInsured, lTemp))) Then
                            If CStr(vArray(m_klSumInsuredArrayColNo_SumInsured, lTemp)) <> "" Then
                                m_lReturn = m_oFormFields.FormatControl(txtCurrency, vArray(m_klSumInsuredArrayColNo_SumInsured, lTemp))
                                ListViewHelper.GetListViewSubItem(oListItem, m_klSumInsuredListColNo_SumInsured).Text = txtCurrency.Text

                            End If
                        End If


                        If Not (Convert.IsDBNull(vArray(m_klSumInsuredArrayColNo_DateAdded, lTemp)) Or IsNothing(vArray(m_klSumInsuredArrayColNo_DateAdded, lTemp))) Then
                            m_lReturn = m_oFormFields.FormatControl(txtDate, vArray(m_klSumInsuredArrayColNo_DateAdded, lTemp))
                            ListViewHelper.GetListViewSubItem(oListItem, m_klSumInsuredListColNo_DateAdded).Text = txtDate.Text
                        End If


                        If Not (Convert.IsDBNull(vArray(m_klSumInsuredArrayColNo_DateDeleted, lTemp)) Or IsNothing(vArray(m_klSumInsuredArrayColNo_DateDeleted, lTemp))) Then
                            m_lReturn = m_oFormFields.FormatControl(txtDate, vArray(m_klSumInsuredArrayColNo_DateDeleted, lTemp))
                            ListViewHelper.GetListViewSubItem(oListItem, m_klSumInsuredListColNo_DateDeleted).Text = txtDate.Text
                            If txtDate.Text = "" Then
                                cSumInsured += CDec(vArray(m_klSumInsuredArrayColNo_SumInsured, lTemp))
                            End If
                        End If

                        ListViewHelper.GetListViewSubItem(oListItem, m_klSumInsuredListColNo_ValuationReq).Text = "No"


                        If Not (Convert.IsDBNull(vArray(m_klSumInsuredArrayColNo_ValuationReq, lTemp)) Or IsNothing(vArray(m_klSumInsuredArrayColNo_ValuationReq, lTemp))) Then
                            If CStr(vArray(m_klSumInsuredArrayColNo_ValuationReq, lTemp)) = "1" Then
                                ListViewHelper.GetListViewSubItem(oListItem, m_klSumInsuredListColNo_ValuationReq).Text = "Yes"
                            End If
                        End If


                        If Not (Convert.IsDBNull(vArray(m_klSumInsuredArrayColNo_ValuationDate, lTemp)) Or IsNothing(vArray(m_klSumInsuredArrayColNo_ValuationDate, lTemp))) Then
                            m_lReturn = m_oFormFields.FormatControl(txtDate, vArray(m_klSumInsuredArrayColNo_ValuationDate, lTemp))
                            ListViewHelper.GetListViewSubItem(oListItem, m_klSumInsuredListColNo_ValuationDate).Text = txtDate.Text
                        End If

                        If CStr(vArray(m_klSumInsuredArrayColNo_Rate, lTemp)) <> "" Then
                            cRate = ToSafeCurrency(vArray(m_klSumInsuredArrayColNo_Rate, lTemp), 0)
                        End If


                    End If
                Next lTemp
            End If

            m_lReturn = m_oFormFields.FormatControl(txtCurrency, cSumInsured)

            pnlTotalSumInsured(lControlIndex).Text = txtCurrency.Text

            ' if there is a sum insured
            If cSumInsured <> 0 Then

                ' display the rate
                m_lReturn = m_oFormFields.FormatControlArray(txtRate(lControlIndex), cRate, GetIndex(txtRate(lControlIndex), txtRate))

                ' calculate the premium
                cPremium = (cSumInsured * (cRate / 100))

                ' display the calculated premium
                m_lReturn = m_oFormFields.FormatControlArray(txtPremium(lControlIndex), cPremium, GetIndex(txtPremium(lControlIndex), txtPremium))

            End If

            ' now flag this in the array as having been created
            m_vScreenDetails(ACDScreenDetailCnt, v_lScreenDetailsIndex) = v_lScreenDetailsIndex  ' RAW 20/08/2004 : JIT : added
            m_oNewControlBeingLoaded = Nothing  ' RAW 20/08/2004 : JIT : added


            vArray = Nothing


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "AddSumInsured Failed", ACApp, ACClass, "AddSumInsured", Information.Err().Number, Information.Err().Description, excep:=ex)


        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If


        End Try
        Return result
    End Function



    ' ***************************************************************** '
    '
    ' Name: AddLabel
    '
    ' Description:
    '
    ' History: 05/07/2001 CLG - Created from AddTestBox.
    ' RAW 09/07/2004 : JIT : added v_lScreenDetailsIndex param and used instead of m_cControlCount
    ' ***************************************************************** '
    Private Function AddLabel(ByRef iFrameIndex As Integer, ByRef lTag As Integer, ByRef lX As Single, ByRef lY As Single, ByRef sPropertyName As String, ByRef vPMFormat As Object, ByRef vTabSetIndex As Object, ByRef vHelpText As Object, ByVal v_lScreenDetailsIndex As Integer) As Integer

        Dim result As Integer = 0
        Dim lDebugDepthCounter As Integer
        Dim lMandatory As gPMConstants.PMEMandatoryStatus
        'Dim lFieldType, lPMFieldType, lFormat As Integer
        Dim sTempCaption As String = ""
        'Dim vArray As Object
        Dim lControlIndex, lCharPos As Integer


        Try

            ' This is called for a free-standing label that is not linked to another control

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = AddFrame(iFrameIndex, lTag, lX, lY, 0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                Return result
            End If

            ' RAW 09/07/2004 : JIT : added
            If txtText.Length = 1 Then
                ' This is the first control of this type to be created so start at index 1
                ' - index 0 remains in its original state as a template for later controls of this type
                m_lTextIndex = 1
                ReDim m_vTextArray(ACCLastArrayPosition, m_lTextIndex)
            Else
                'm_lTextIndex = txtText.Length + 1
                m_lTextIndex = txtText.Length
                ReDim Preserve m_vTextArray(ACCLastArrayPosition, m_lTextIndex)

            End If

            ' RAW 09/07/2004 : JIT : added
            ' store this index locally.
            ' Do not refer to the module level variable again from within this function as it may change
            ' if DL is triggered resulting in other controls being created, and thus the index being incremented,
            ' before this function has completed
            lControlIndex = m_lTextIndex

            If lControlIndex > 0 Then
                ' RAW 09/07/2004 : JIT : added
#If (DebugOption AndAlso m_klDebugOption_JIT) = m_klDebugOption_JIT Then

            AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="JIT : Adding Label (" & lControlIndex & ") - " & ObjectAndPropertyName(r_vObjIdx:=m_vScreenDetails(ACDGISPropertyId, v_lScreenDetailsIndex), r_PropIdx_optional:=v_lScreenDetailsIndex))
#End If

                AddControl("txtText", lControlIndex)
                AddControl("lblText", lControlIndex)
            End If

            ' RAW 09/07/2004 : JIT : added
            ' store reference to this control in a collection - keyed on object and property name

            m_cObjectAndAttribute.Add(New Object() {lblText(lControlIndex), txtText(lControlIndex), "", v_lScreenDetailsIndex}, ObjectAndPropertyName(m_vScreenDetails(ACDGISPropertyId, v_lScreenDetailsIndex), v_lScreenDetailsIndex).ToLower())

            m_oNewControlBeingLoaded = lblText(lControlIndex)  ' RAW 20/08/2004 : JIT : added


            m_vTextArray(ACCFrameNumber, lControlIndex) = iFrameIndex
            m_vTextArray(ACCIsDeleted, lControlIndex) = gPMConstants.PMEReturnCode.PMFalse
            m_vTextArray(ACCHelpText, lControlIndex) = (vHelpText)
            m_vTextArray(ACCPreQuote, lControlIndex) = m_lPreQuote
            m_vTextArray(ACCPostQuote, lControlIndex) = m_lPostQuote
            m_vTextArray(ACCPurchase, lControlIndex) = m_lPurchase
            m_vTextArray(ACCIncludeInList, lControlIndex) = m_lIncludeInList

            m_vTextArray(ACCChildId, lControlIndex) = DBNull.Value

            m_vTextArray(ACCGISObjectId, lControlIndex) = DBNull.Value
            m_vTextArray(ACCPMFormat, lControlIndex) = (vPMFormat)
            m_vTextArray(ACCTabSetIndex, lControlIndex) = (vTabSetIndex)

            txtText(lControlIndex).Parent = fraFrame(iFrameIndex)
            lblText(lControlIndex).Parent = fraFrame(iFrameIndex)


            lblText(lControlIndex).Top = (lY)
            lblText(lControlIndex).Left = (lX)
            lblText(lControlIndex).Visible = True
            lblText(lControlIndex).Text = sPropertyName.Split("|")(0)

            If (sPropertyName.Split("|").Length > 1) Then
                ToolTip1.SetToolTip(lblText(lControlIndex), sPropertyName.Split("|")(1).ToString())
            End If

            lblText(lControlIndex).Tag = CStr(lTag)

            If lTag = ndcHyperlink Then
                ' RAW 20/10/2004 : CQ1814 : added
                ' this is the target address - for starters assume that this is the same as the visible caption
                txtText(lControlIndex).Text = sPropertyName

                ' now check whether the target address is different to the visible caption that the user sees
                lCharPos = (sPropertyName.IndexOf("|"c) + 1)
                If lCharPos > 0 Then
                    ' this is what the user sees
                    lblText(lControlIndex).Text = sPropertyName.Substring(0, Math.Min(sPropertyName.Length, lCharPos - 1))
                    ' this is the target address
                    txtText(lControlIndex).Text = sPropertyName.Substring(sPropertyName.Length - Math.Min(sPropertyName.Length, sPropertyName.Length - lCharPos))
                End If
                ' RAW 20/10/2004 : CQ1814 : end
                ' lblText(lControlIndex).AutoSize = False       ' RAW 20/10/2004 : CQ1814 : removed
                ' lblText(lControlIndex).Height = 195           ' RAW 20/10/2004 : CQ1814 : removed
                lblText(lControlIndex).Font = VB6.FontChangeUnderline(lblText(lControlIndex).Font, True)
                lblText(lControlIndex).ForeColor = Color.FromArgb(0, 0, 192)

                lblText(lControlIndex).Cursor = Cursors.Hand

            End If

            lblText(lControlIndex).Width = lblText(lControlIndex).Text.Length + 200

            txtText(lControlIndex).Visible = False
            txtText(lControlIndex).Tag = CStr(lTag)
            txtText(lControlIndex).Enabled = False
            ToolTip1.SetToolTip(txtText(lControlIndex), Convert.ToString(vHelpText))

            lMandatory = gPMConstants.PMEMandatoryStatus.PMNonMandatory

            'hide label if ACBlankCaption
            SetBlankCaption(lblText(lControlIndex), False)

            ' now flag this in the array as having been created
            m_vScreenDetails(ACDScreenDetailCnt, v_lScreenDetailsIndex) = v_lScreenDetailsIndex  ' RAW 20/08/2004 : JIT : added
            m_oNewControlBeingLoaded = Nothing  ' RAW 20/08/2004 : JIT : added



        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "AddLabel Failed", ACApp, ACClass, "AddLabel", Information.Err().Number, Information.Err().Description, excep:=ex)


        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If


        End Try
        Return result
    End Function

    Private Sub lnk_Clicked(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim sTooltip As String = Trim(ToolTip1.GetToolTip(DirectCast(sender, Label)))
        If (sTooltip <> "") Then
            System.Diagnostics.Process.Start(fileName:=sTooltip, arguments:=nonDatabaseElements)
        End If
    End Sub
    ' ***************************************************************** '
    '
    ' Name: AddTextBox
    '
    ' Description:
    '
    ' History: 26/06/2000 Tomo - Created.
    ' RAW 09/07/2004 : JIT : added v_lScreenDetailsIndex param and used instead of m_cControlCount
    ' ***************************************************************** '
    Private Function AddTextBox(ByRef iFrameIndex As Integer, ByRef lTag As Integer, ByRef lX As Single, ByRef lY As Single, ByRef sPropertyName As String, ByRef vPMFormat As Object, ByRef vTabSetIndex As Object, ByRef r_bIsComment As Boolean, ByVal v_lScreenDetailsIndex As Integer, ByVal iIsFormattedText As Integer) As Integer

        Dim result As Integer = 0
        Dim lDebugDepthCounter, lControlIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = AddFrame(iFrameIndex, lTag, lX, lY, 0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                Return result
            End If

            If vPMFormat = PMFormatStringMultiLine And iIsFormattedText = 1 Then
                If txtFormattedText.Length = 1 Then
                    ' This is the first control of this type to be created so start at index 1
                    ' - index 0 remains in its original state as a template for later controls of this type
                    m_lFormattedTextIndex = 1
                    ReDim m_vFormattedTextArray(ACCLastArrayPosition, m_lFormattedTextIndex)
                Else
                    m_lFormattedTextIndex = txtMlText.Length + 1
                    ReDim Preserve m_vFormattedTextArray(ACCLastArrayPosition, m_lFormattedTextIndex)
                End If

                ' store this index locally.
                ' Do not refer to the module level variable again from within this function as it may change
                ' if DL is triggered resulting in other controls being created, and thus the index being incremented,
                ' before this function has completed
                lControlIndex = m_lFormattedTextIndex

                If lControlIndex > 0 Then
                    AddControl("txtFormattedText", lControlIndex)
                    AddControl("lblFormattedText", lControlIndex)
                End If
                ' store reference to this control in a collection - keyed on object and property name
                m_cObjectAndAttribute.Add(New Object() {txtFormattedText(lControlIndex), lblFormattedText(lControlIndex), "", v_lScreenDetailsIndex}, ObjectAndPropertyName(lTag).ToLower())
                m_oNewControlBeingLoaded = txtFormattedText(lControlIndex)
                'set maxlength before calling SetInitialControlValues otherwise we may loose some data
                txtFormattedText(lControlIndex).MaxLength = IIf(r_bIsComment, 10000, 255)
                SetInitialControlValues(lControlIndex, m_vFormattedTextArray, lblFormattedText, txtFormattedText, fraFrame, iFrameIndex, Task, vPMFormat, vTabSetIndex, 0, CInt(lX), CInt(lY), sPropertyName, lTag, v_lScreenDetailsIndex, True, m_lHeight, m_lWidth, m_sHelpText, m_lPreQuote, m_lPostQuote, m_lPurchase, m_lIsValuation, m_lIsRateAndPremium, m_lIncludeInList, m_lStage, m_oFormFields, v_lScreenDetailsIndex, (m_vDataDictionary(GISDMTypeRisk)), m_vScreenValues, ACPDataType:=uctRiskScreenControl.MainModule.ACPDataType, ACPIsInputProperty:=MainModule.ACPIsInputProperty)
                ToolTip1.SetToolTip(txtFormattedText(lControlIndex), Convert.ToString(m_sHelpText))
                'this call positions everything correctly
                textLabel_MouseMove("", MouseButtons.Left, g_lx, g_ly, txtFormattedText(lControlIndex), lblFormattedText(lControlIndex), pnlPosition, m_vTabArray(ACFTabSnapToGrid, CInt(m_vFrameArray(ACFTabNumber, iFrameIndex))))

            ElseIf vPMFormat = PMFormatStringMultiLine Then
                If txtMlText.Length = 1 Then
                    ' This is the first control of this type to be created so start at index 1
                    ' - index 0 remains in its original state as a template for later controls of this type
                    m_lMlTextIndex = 1
                    ReDim m_vMlTextArray(ACCLastArrayPosition, m_lMlTextIndex)
                Else
                    m_lMlTextIndex = txtMlText.Length + 1
                    ReDim Preserve m_vMlTextArray(ACCLastArrayPosition, m_lMlTextIndex)
                End If

                ' store this index locally.
                ' Do not refer to the module level variable again from within this function as it may change
                ' if DL is triggered resulting in other controls being created, and thus the index being incremented,
                ' before this function has completed
                lControlIndex = m_lMlTextIndex

                If lControlIndex > 0 Then
                    AddControl("txtMlText", lControlIndex)
                    AddControl("lblMlText", lControlIndex)
                End If
                ' store reference to this control in a collection - keyed on object and property name
                m_cObjectAndAttribute.Add(New Object() {txtMlText(lControlIndex), lblMlText(lControlIndex), "", v_lScreenDetailsIndex}, ObjectAndPropertyName(lTag).ToLower())
                m_oNewControlBeingLoaded = txtMlText(lControlIndex)  ' RAW 20/08/2004 : JIT : added
                'set maxlength before calling SetInitialControlValues otherwise we may loose some data
                txtMlText(lControlIndex).MaxLength = IIf(r_bIsComment, 10000, 255)
                SetInitialControlValues(lControlIndex, m_vMlTextArray, lblMlText, txtMlText, fraFrame, iFrameIndex, Task, vPMFormat, vTabSetIndex, 0, CInt(lX), CInt(lY), sPropertyName, lTag, v_lScreenDetailsIndex, True, m_lHeight, m_lWidth, m_sHelpText, m_lPreQuote, m_lPostQuote, m_lPurchase, m_lIsValuation, m_lIsRateAndPremium, m_lIncludeInList, m_lStage, m_oFormFields, v_lScreenDetailsIndex, (m_vDataDictionary(GISDMTypeRisk)), m_vScreenValues, ACPDataType:=uctRiskScreenControl.MainModule.ACPDataType, ACPIsInputProperty:=MainModule.ACPIsInputProperty)
                ToolTip1.SetToolTip(txtMlText(lControlIndex), Convert.ToString(m_sHelpText))
                'this call positions everything correctly
                textLabel_MouseMove("", MouseButtons.Left, g_lx, g_ly, txtMlText(lControlIndex), lblMlText(lControlIndex), pnlPosition, m_vTabArray(ACFTabSnapToGrid, CInt(m_vFrameArray(ACFTabNumber, iFrameIndex))))

            Else

                If txtText.Length = 1 Then
                    ' This is the first control of this type to be created so start at index 1
                    ' - index 0 remains in its original state as a template for later controls of this type
                    m_lTextIndex = 1
                    ReDim m_vTextArray(ACCLastArrayPosition, m_lTextIndex)
                Else
                    m_lTextIndex = txtText.Length
                    ReDim Preserve m_vTextArray(ACCLastArrayPosition, m_lTextIndex)
                End If

                ' store this index locally.
                ' Do not refer to the module level variable again from within this function as it may change
                ' if DL is triggered resulting in other controls being created, and thus the index being incremented,
                ' before this function has completed
                lControlIndex = m_lTextIndex

                If lControlIndex > 0 Then
                    AddControl("txtText", lControlIndex)
                    AddControl("lblText", lControlIndex)
                End If

                ' store reference to this control in a collection - keyed on object and property name
                m_cObjectAndAttribute.Add(New Object() {txtText(lControlIndex), lblText(lControlIndex), "", v_lScreenDetailsIndex}, ObjectAndPropertyName(lTag).ToLower())
                m_oNewControlBeingLoaded = txtText(lControlIndex)  ' RAW 20/08/2004 : JIT : added
                SetInitialControlValues(lControlIndex, m_vTextArray, lblText, txtText, fraFrame, iFrameIndex, Task, vPMFormat, vTabSetIndex, 0, CInt(lX), CInt(lY), sPropertyName, lTag, v_lScreenDetailsIndex, True, m_lHeight, m_lWidth, m_sHelpText, m_lPreQuote, m_lPostQuote, m_lPurchase, m_lIsValuation, m_lIsRateAndPremium, m_lIncludeInList, m_lStage, m_oFormFields, v_lScreenDetailsIndex, (m_vDataDictionary(GISDMTypeRisk)), m_vScreenValues, ACPDataType:=uctRiskScreenControl.MainModule.ACPDataType, ACPIsInputProperty:=MainModule.ACPIsInputProperty)
                ToolTip1.SetToolTip(txtText(lControlIndex), Convert.ToString(m_sHelpText))
                'this call positions everything correctly
                textLabel_MouseMove("", MouseButtons.Left, g_lx, g_ly, txtText(lControlIndex), lblText(lControlIndex), pnlPosition, m_vTabArray(ACFTabSnapToGrid, CInt(m_vFrameArray(ACFTabNumber, iFrameIndex))))
                ' Only allow 255 characters as that is what SQL Server is set up to accept.
                txtText(lControlIndex).MaxLength = 255
            End If

            ' now flag this in the array as having been created
            m_vScreenDetails(ACDScreenDetailCnt, v_lScreenDetailsIndex) = v_lScreenDetailsIndex  ' RAW 20/08/2004 : JIT : added
            m_oNewControlBeingLoaded = Nothing



        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "AddTextBox Failed", ACApp, ACClass, "AddTextBox", Information.Err().Number, Information.Err().Description, excep:=ex)


        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If


        End Try
        Return result
    End Function

    ''' <summary>
    ''' To check tab index has been set or not.
    ''' </summary>
    ''' <param name="m_lTabsetIndexlist"></param>
    ''' <param name="clblControl"></param>
    ''' <param name="sAssociatedControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function TabListContainsControl(ByVal m_lTabsetIndexlist As String(,), ByVal clblControl As Control, ByVal sAssociatedControl As Control) As Integer
        For i As Integer = 0 To m_lTabsetIndexlist.GetUpperBound(1)
            If Not IsNothing(m_lTabsetIndexlist(0, i)) Then
                If m_lTabsetIndexlist(0, i).ToString().ToUpper() = clblControl.Text.ToString().ToUpper() And ToSafeString(m_lTabsetIndexlist(2, i)) = sAssociatedControl.Tag Then
                    Return i
                End If
            End If
        Next
        Return -1
    End Function

    ' ***************************************************************** '
    '
    ' Name: ResetTabIndex
    '
    ' Description:
    '
    ' History:
    ' 20000608 Tomo - Created.
    ' 20010813 CLG  PBM0004 Allow user to overide default definition of the tab order of fields
    '
    ' ***************************************************************** '
    Private Function ResetTabIndex() As Integer
        Dim result As Integer = 0
        Dim lTemp2 As gPMConstants.PMEReturnCode
        Dim ctlControl As Control
        Dim sName, sFirstThree As String
        Dim vArray(,) As Object
        Dim lIndex As Integer
        Dim checkST As String
        Dim iCtrlIndex As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        If Not Information.IsArray(m_vFrameArray) Then
            Return result
        End If

        Dim aControl As ArrayList = New ArrayList()
        ControlList(Me, aControl)

        For iTemp As Integer = 0 To aControl.Count - 1
            ctlControl = aControl(iTemp)
            sName = ""
            checkST = ""
            Try
                sName = ctlControl.Parent.Name

            Catch
            End Try
            If sName.Contains("fraFrame") Then
                If ctlControl.Name.LastIndexOf("_") > 0 Then
                    checkST = ctlControl.Name.Substring(1, ctlControl.Name.LastIndexOf("_") - 1)
                End If
                sFirstThree = Strings.Left(checkST, 3)

                Select Case sFirstThree
                    Case "lbl", "pnl", "fra"

                    Case Else
                        lTemp2 = CType(lTemp2 + 1, gPMConstants.PMEReturnCode)
                        If lTemp2 = gPMConstants.PMEReturnCode.PMFalse Then
                            ReDim vArray(2, lTemp2)
                        Else
                            ReDim Preserve vArray(2, lTemp2)
                        End If
                        vArray(0, lTemp2) = Strings.Format(GetIndex(ctlControl.Parent, fraFrame), "0000") & Strings.Format(ctlControl.Top, "0000000") & Strings.Format(ctlControl.Left, "0000000")

                        vArray(1, lTemp2) = iTemp
                        lIndex = -1
                        Try

                            If TypeOf ctlControl Is TextBox Then
                                lIndex = GetIndex(ctlControl, txtText)
                            ElseIf (TypeOf ctlControl Is CheckBox) Then
                                lIndex = GetIndex(ctlControl, chkYesNo)
                            ElseIf (TypeOf ctlControl Is uctPBFindRT.PBFindRT) Then
                                lIndex = GetIndex(ctlControl, PBFindRT1)
                            ElseIf (TypeOf ctlControl Is uctGISUserDefLookupControl.cboGISLookup) Then
                                lIndex = GetIndex(ctlControl, cboGISLookup)
                            ElseIf (TypeOf ctlControl Is PMLookupControl.cboPMLookup) Then
                                lIndex = GetIndex(ctlControl, cboPMLookup)
                            End If

                        Catch
                        End Try



                        vArray(2, lTemp2) = ctlControl.Name & StringsHelper.Format(lIndex, "000")
                End Select
            End If

        Next iTemp

        If lTemp2 < 1 Then
            vArray = Nothing
            Return result
        End If

        SortThreeElementArray(vArray)

        For lTemp As Integer = (vArray).GetLowerBound(1) To (vArray).GetUpperBound(1)

            'ctlControl = ReflectionHelper.Invoke(Me.Controls_Renamed, "Item", New Object() {vArray(1, lTemp)})
            ctlControl = aControl(vArray(1, lTemp))
            ctlControl.TabIndex = lTemp
        Next lTemp

        'now set tab index using any stored values
        vArray = Nothing
        lTemp2 = gPMConstants.PMEReturnCode.PMFalse

        'first do the single conltrols using the helper function
        lTemp2 = CType(BuildTabSetIndexArray(lTemp2, (vArray), m_vFrameArray, ACFTabSetIndex, fraFrame, True), gPMConstants.PMEReturnCode)  'okay
        lTemp2 = CType(BuildTabSetIndexArray(lTemp2, (vArray), m_vGISComboArray, ACCTabSetIndex, cboGISLookup, True), gPMConstants.PMEReturnCode)
        lTemp2 = CType(BuildTabSetIndexArray(lTemp2, (vArray), m_vGeminiComboArray, ACCTabSetIndex, cboList, True), gPMConstants.PMEReturnCode)
        lTemp2 = CType(BuildTabSetIndexArray(lTemp2, (vArray), m_vPMLookupArray, ACCTabSetIndex, cboPMLookup, True), gPMConstants.PMEReturnCode)
        lTemp2 = CType(BuildTabSetIndexArray(lTemp2, (vArray), m_vTextArray, ACCTabSetIndex, txtText, True), gPMConstants.PMEReturnCode)
        lTemp2 = CType(BuildTabSetIndexArray(lTemp2, (vArray), m_vMlTextArray, ACCTabSetIndex, txtMlText, True), gPMConstants.PMEReturnCode)
        lTemp2 = CType(BuildTabSetIndexArray(lTemp2, (vArray), m_vCheckArray, ACCTabSetIndex, chkYesNo, True), gPMConstants.PMEReturnCode)
        lTemp2 = CType(BuildTabSetIndexArray(lTemp2, (vArray), m_vPartyCommandArray, ACCTabSetIndex, cmdPartyCommand, True), gPMConstants.PMEReturnCode)
        lTemp2 = CType(BuildTabSetIndexArray(lTemp2, (vArray), m_vPolicyCommandArray, ACCTabSetIndex, cmdPolicyCommand, True), gPMConstants.PMEReturnCode)
        lTemp2 = CType(BuildTabSetIndexArray(lTemp2, (vArray), m_vAddressArray, ACCTabSetIndex, cmdAddress, True), gPMConstants.PMEReturnCode)  'just the button
        lTemp2 = CType(BuildTabSetIndexArray(lTemp2, (vArray), m_vAccumulationArray, ACCTabSetIndex, cboAccumulation, False), gPMConstants.PMEReturnCode)

        'add list views
        If Information.IsArray(m_vListViewArray) Then
            For lTemp As Integer = m_vListViewArray.GetLowerBound(1) To m_vListViewArray.GetUpperBound(1)
                If CDbl(m_vListViewArray(ACCTabSetIndex, lTemp)) > 0 Then  'if tab set then
                    If lTemp2 = gPMConstants.PMEReturnCode.PMFalse Then
                        ReDim vArray(2, 4)
                    Else
                        ReDim Preserve vArray(2, lTemp2 + 3)
                    End If
                    vArray(0, lTemp2) = m_vListViewArray(ACCTabSetIndex, lTemp)  'tab index value
                    vArray(1, lTemp2) = lvwListView(lTemp)  'type name
                    vArray(0, lTemp2 + 1) = (vArray(0, lTemp2)) + 1  'tab index value
                    vArray(1, lTemp2 + 1) = cmdListViewAdd(lTemp)  'type name
                    vArray(0, lTemp2 + 2) = (vArray(0, lTemp2)) + 2  'tab index value
                    vArray(1, lTemp2 + 2) = cmdListViewEdit(lTemp)  'type name
                    vArray(0, lTemp2 + 3) = (vArray(0, lTemp2)) + 3  'tab index value
                    vArray(1, lTemp2 + 3) = cmdListViewDelete(lTemp)  'type name
                    lTemp2 = CType(lTemp2 + 4, gPMConstants.PMEReturnCode)
                End If
            Next lTemp
        End If

        'add sum insureds
        If Information.IsArray(m_vSumInsuredArray) Then
            For lTemp As Integer = m_vSumInsuredArray.GetLowerBound(1) To m_vSumInsuredArray.GetUpperBound(1)
                If CDbl(m_vSumInsuredArray(ACCTabSetIndex, lTemp)) > 0 Then  'if tab set then
                    If lTemp2 = gPMConstants.PMEReturnCode.PMFalse Then
                        ReDim vArray(2, 4)
                    Else
                        ReDim Preserve vArray(2, lTemp2 + 5)
                    End If
                    vArray(0, lTemp2) = m_vSumInsuredArray(ACCTabSetIndex, lTemp)  'tab index value
                    vArray(1, lTemp2) = lvwSumInsured(lTemp)  'type name
                    vArray(0, lTemp2 + 1) = (vArray(0, lTemp2)) + 1  'tab index value
                    vArray(1, lTemp2 + 1) = cmdSumInsuredAdd(lTemp)  'type name
                    vArray(0, lTemp2 + 2) = (vArray(0, lTemp2)) + 2  'tab index value
                    vArray(1, lTemp2 + 2) = cmdSumInsuredEdit(lTemp)  'type name
                    vArray(0, lTemp2 + 3) = (vArray(0, lTemp2)) + 3  'tab index value
                    vArray(1, lTemp2 + 3) = cmdSumInsuredDelete(lTemp)  'type name
                    vArray(0, lTemp2 + 4) = (vArray(0, lTemp2)) + 4  'tab index value
                    vArray(1, lTemp2 + 4) = txtRate(lTemp)  'type name
                    vArray(0, lTemp2 + 5) = (vArray(0, lTemp2)) + 5  'tab index value
                    vArray(1, lTemp2 + 5) = txtPremium(lTemp)  'type name
                    lTemp2 = CType(lTemp2 + 6, gPMConstants.PMEReturnCode)
                End If
            Next lTemp
        End If

        'add standard wording
        If Information.IsArray(m_vStandardWordingArray) Then
            For lTemp As Integer = m_vStandardWordingArray.GetLowerBound(1) To m_vStandardWordingArray.GetUpperBound(1)
                If CDbl(m_vStandardWordingArray(ACCTabSetIndex, lTemp)) > 0 Then  'if tab set then
                    If lTemp2 = gPMConstants.PMEReturnCode.PMFalse Then
                        ReDim vArray(2, 4)
                    Else
                        ReDim Preserve vArray(2, lTemp2 + 5)
                    End If
                    vArray(0, lTemp2) = m_vStandardWordingArray(ACCTabSetIndex, lTemp)  'tab index value
                    vArray(1, lTemp2) = lvwStandardWording(lTemp)  'type name
                    vArray(0, lTemp2 + 1) = (vArray(0, lTemp2)) + 1  'tab index value
                    vArray(1, lTemp2 + 1) = cmdStandardWordingUp(lTemp)  'type name
                    vArray(0, lTemp2 + 2) = (vArray(0, lTemp2)) + 2  'tab index value
                    vArray(1, lTemp2 + 2) = cmdStandardWordingDown(lTemp)  'type name
                    vArray(0, lTemp2 + 3) = (vArray(0, lTemp2)) + 3  'tab index value
                    vArray(1, lTemp2 + 3) = cmdStandardWordingAdd(lTemp)  'type name
                    vArray(0, lTemp2 + 4) = (vArray(0, lTemp2)) + 4  'tab index value
                    vArray(1, lTemp2 + 4) = cmdStandardWordingEdit(lTemp)  'type name
                    vArray(0, lTemp2 + 5) = (vArray(0, lTemp2)) + 5  'tab index value
                    vArray(1, lTemp2 + 5) = cmdStandardWordingDelete(lTemp)  'type name
                    lTemp2 = CType(lTemp2 + 6, gPMConstants.PMEReturnCode)
                End If
            Next lTemp
        End If


        'tbd
        'If m_uctCLMReserveTabIndex > 0 Then

        'If m_uctCLMPaymentTabIndex > 0 Then


        'if we have been given specific tab orders, sort them and set them
        If Information.IsArray(vArray) Then
            SortThreeElementArray((vArray), 1)
            lTemp2 = 100
            For lTemp As Integer = (vArray).GetLowerBound(1) To (vArray).GetUpperBound(1)
                If Not IsNothing(vArray(1, lTemp)) Then
                    ReflectionHelper.SetMember(vArray(1, lTemp), "TabIndex", lTemp2)
                    lTemp2 = CType(lTemp2 + 1, gPMConstants.PMEReturnCode)
                End If
            Next
        End If

        'tbd ??? tabScreen.TabIndex = 0
        vArray = Nothing
        ctlControl = Nothing

        Return result

Err_ResetTabIndex:

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "ResetTabIndex Failed", ACApp, ACClass, "ResetTabIndex", Information.Err().Number, Information.Err().Description)

        Return result




    End Function
    ' ***************************************************************** '
    '
    ' Name: BuildTabSetIndexArray
    '
    ' Description:
    '
    ' History: 06/08/2001 CLG - Created.
    '
    ' ***************************************************************** '
    Private Function BuildTabSetIndexArray(ByRef lTemp2 As Integer, ByRef vArray(,) As Object, ByRef vDetailsArray(,) As Object, ByRef iOffset As Integer, ByRef sControlType As Object, ByRef bAarrayOfControls As Boolean) As Integer

        Dim result As Integer = 0
        Dim lDebugDepthCounter As Integer

        Try
            If Information.IsArray(vDetailsArray) Then
                For lTemp As Integer = vDetailsArray.GetLowerBound(1) To vDetailsArray.GetUpperBound(1)
                    If CDbl(vDetailsArray(iOffset, (lTemp))) > 0 Then  'if tab set then
                        If lTemp2 = 0 Then
                            ReDim vArray(2, lTemp2)
                        Else
                            ReDim Preserve vArray(2, lTemp2)
                        End If
                        Dim tempRefParam As Object = vArray(0, lTemp2)
                        tempRefParam = vDetailsArray(iOffset, (lTemp))  'tab index value
                        If tempRefParam IsNot Nothing AndAlso tempRefParam >= 100 AndAlso bAarrayOfControls Then
                            sControlType(lTemp).TabIndex = tempRefParam
                        End If
                        If bAarrayOfControls Then
                            vArray(1, lTemp2) = sControlType(lTemp)  'type name
                        Else
                            vArray(1, lTemp2) = sControlType  'type name
                        End If
                        Dim tempRefParam2 As Object = vArray(2, lTemp2)
                        tempRefParam2 = lTemp  'control index
                        lTemp2 += 1
                    End If
                Next lTemp
            End If
            result = lTemp2

            ' Debug message
#If (DebugOption And m_klDebugOption_Normal) = m_klDebugOption_Normal Then
		AddToDebug(lDebugDepthCounter, "Exiting " & ACApp & "." & ACClass & ".BuildTabSetIndexArray")
#End If


        Catch ex As Exception

            ' Debug message
#If (DebugOption And m_klDebugOption_Normal) = m_klDebugOption_Normal Then
		AddToDebug(lDebugDepthCounter, "Errored in " & ACApp & "." & ACClass & ".BuildTabSetIndexArray")
#End If

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "BuildTabSetIndexArray Failed", ACApp, ACClass, "BuildTabSetIndexArray", Information.Err().Number, Information.Err().Description, excep:=ex)



        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If


        End Try
        Return result
    End Function

    ' PRIVATE Methods (End)
    ' PRIVATE Events (Begin)

    Private Sub cboAccumulation_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboAccumulation.Click

        Dim lDebugDepthCounter, lTag As Integer
        Dim vArray As Object

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : cboAccumulation_Click ")
#End If

        lTag = CInt(Convert.ToString(lblAccumulation.Tag))

        vArray = Array.CreateInstance(GetType(Object), New Integer() {2}, New Integer() {0})

        vArray(0) = cboAccumulation.ItemId
        vArray(1) = cboAccumulation.ItemCaption

        ' RAW 04/09/2003 : CQ258 : added
        If cboAccumulation.IsItemDeleted(CInt(vArray(0))) And Not cboAccumulation.IsLoadInProgress And Not cboAccumulation.IsListIndexSetByCode Then
            ' item has been deleted from list and user has made a selection rather than being changed from code
            If MessageBox.Show(CStr(vArray(1)) & " has been deleted - Are you sure you wish to use it", "Select item from list", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                ' deselect it
                cboAccumulation.ListIndex = -1
                GoTo GetOutOfHere
            End If
        End If
        ' RAW 04/09/2003 : CQ258 : end

        SetScreenValues(lTag, vArray, Convert.ToString(cboAccumulation.Tag), CDbl(m_vFrameArray(ACFTabNumber, GetIndex(cboAccumulation.Parent, fraFrame))) + 1)

        ' RAW 22/06/2004 : Performance Changes(#2) : removed call to RunDynamicLogic because when m_bFireDLStartEvents = FALSE because onChange may be raised from SetScreenValues and also called from Validate event
        If m_bFireDLStartEvents Then
            ' RAW 21/05/2004 : Performance Changes : added params
            ' RAW 22/06/2004 : Performance Changes(#2) : replaced onChange with Start
            RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_Start, v_vObjIdx:=cboAccumulation.Tag, v_vControl:=cboAccumulation)
        End If

GetOutOfHere:   ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

    End Sub

    Private Sub cboAccumulation_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboAccumulation.GotFocus

        Dim lTag As Integer



        ' lTag = ContainerHelper.GetControlIndex(cboAccumulation.Parent)
        lTag = GetIndex(cboAccumulation.Parent, fraFrame)

        SelectTab(TabStrip1, CInt(m_vFrameArray(ACFTabNumber, lTag)))
        'TabStrip1_Click(Nothing, Nothing)

        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message


    End Sub


    Private Sub cboAccumulation_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles cboAccumulation.Validating
        Dim Cancel As Boolean = eventArgs.Cancel  ' RAW 21/05/2004 : Performance Changes : replaced LostFocus()

        Dim lDebugDepthCounter As Integer

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : cboAccumulation_Validate ")
#End If

        ' RVH - CQ2213 13/8/03 : Start
        '                        Fire dynamic logic when item loses focus
        ' RAW 21/05/2004 : Performance Changes : added params
        ' RAW 22/06/2004 : Performance Changes(#2) : replaced onChange with OnLostFocus
        RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnLostFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_Combo, v_vObjIdx:=cboAccumulation.Tag, v_vControl:=cboAccumulation)
        ' RVH - CQ2213 13/8/03 : End

        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

        eventArgs.Cancel = Cancel
    End Sub

    Private Sub cboGISLookup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  ' Handles _cboGISLookup_0.Click
        Dim Index As Integer = Array.IndexOf(cboGISLookup, eventSender)

        Dim lDebugDepthCounter, lTag, lOldId As Integer
        Dim vArray(1) As Object
        Dim lParentHeaderId As Integer


        lTag = CInt(Convert.ToString(lblGISLookup(Index).Tag))

        vArray(0) = cboGISLookup(Index).ItemId
        vArray(1) = cboGISLookup(Index).ItemCaption

        If CStr(vArray(0)) = "0" Then
            'vArray(0) = Nothing
            vArray(0) = Nothing
        Else
            ' RAW 15/07/2003 : CQ258 : added
            If cboGISLookup(Index).IsItemDeleted(CInt(vArray(0))) And Not cboGISLookup(Index).IsLoadInProgress And Not cboGISLookup(Index).IsListIndexSetByCode Then
                ' item has been deleted from list and user has made a selection rather than being changed from code
                If MessageBox.Show(CStr(vArray(1)) & " has been deleted - Are you sure you wish to use it", "Select item from list", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                    ' deselect it
                    cboGISLookup(Index).ListIndex = -1
                    GoTo GetOutOfHere
                End If
            End If
            ' RAW 15/07/2003 : CQ258 : end
        End If


        SetScreenValues(lTag, vArray, Convert.ToString(cboGISLookup(Index).Tag), CDbl(m_vFrameArray(ACFTabNumber, GetIndex(cboGISLookup(Index).Parent, fraFrame))) + 1)
        vArray = Nothing

        If cboGISLookup(Index).ParentHeaderId <> 0 Then
            For lTemp As Integer = m_vGISComboArray.GetLowerBound(1) To m_vGISComboArray.GetUpperBound(1)
                ' RAW 29/06/2004 : added
                On Error Resume Next
                lParentHeaderId = cboGISLookup(lTemp).ParentHeaderId
                If Information.Err().Number > 0 Then
                    lParentHeaderId = -1
                End If
                On Error GoTo 0
                ' RAW 29/06/2004 : end
                ' RAW 29/06/2004 : replaced
                If lParentHeaderId = cboGISLookup(Index).Table Then
                    'If (cboGISLookup(lTemp).ParentHeaderId = cboGISLookup(Index).Table) Then
                    lOldId = cboGISLookup(lTemp).ItemId
                    cboGISLookup(lTemp).ParentDetailId = cboGISLookup(Index).ItemId
                    cboGISLookup(lTemp).DefaultItemId = lOldId
                    cboGISLookup(lTemp).RefreshList()
                End If
            Next lTemp
        End If

        ' RAW 22/06/2004 : Performance Changes(#2) : removed call to RunDynamicLogic because when m_bFireDLStartEvents = FALSE because onChange may be raised from SetScreenValues and also called from Validate event
        If m_bFireDLStartEvents Then
            ' RAW 21/05/2004 : Performance Changes : added params
            ' RAW 22/06/2004 : Performance Changes(#2) : replaced onChange with Start
            'RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_Start, v_vObjIdx:=cboGISLookup(Index).Tag, v_vControl:=cboGISLookup(Index))
        End If


GetOutOfHere:   ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

    End Sub

    Private Sub cboGISLookup_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cboGISLookup_0.GotFocus
        Dim Index As Integer = Array.IndexOf(cboGISLookup, eventSender)

        Dim lTag As Integer


        'lTag = ContainerHelper.GetControlIndex(cboGISLookup(Index).Parent)
        lTag = GetIndex(cboGISLookup(Index).Parent, fraFrame)

        SelectTab(TabStrip1, CInt(m_vFrameArray(ACFTabNumber, lTag)))
        'TabStrip1_Click(Nothing, Nothing)
        ' RAW 21/05/2004 : Performance Changes : replaced CallScriptOnFocus with RunDynamicLogic and an OnFocus ProcedureType and added v_vControl param
        RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_Combo, v_vObjIdx:=cboGISLookup(Index).Tag, v_vControl:=cboGISLookup(Index))

        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message


    End Sub

    Private Sub cboList_Change(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cboList_0.Change

        m_bListChanged = True
        Dim Index As Integer = Array.IndexOf(cboList, eventSender)

        Dim lDebugDepthCounter, lTag As Integer
        Dim vArray As Object

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : cboList_Click - " & " (" & Index & ")")
#End If

        lTag = CInt(Convert.ToString(lblList(Index).Tag))

        vArray = Array.CreateInstance(GetType(Object), New Integer() {2}, New Integer() {0})

        vArray(0) = cboList(Index).Text
        vArray(1) = cboList(Index).Text

        Dim cbolistIndex As Integer = 0


        SetScreenValues(lTag, vArray, Convert.ToString(cboList(Index).Tag), CDbl(m_vFrameArray(ACFTabNumber, GetIndex(cboList(Index).Parent, fraFrame))) + 1)
        vArray = Nothing

        'm_bListChanged = False

        m_sListText = cboList(Index).Text

        ' RAW 22/06/2004 : Performance Changes(#2) : removed call to RunDynamicLogic because when m_bFireDLStartEvents = FALSE because onChange may be raised from SetScreenValues and also called from Validate event
        'If m_bFireDLStartEvents Then
        '    ' RAW 21/05/2004 : Performance Changes : added params
        '    ' RAW 22/06/2004 : Performance Changes(#2) : replaced onChange with Start
        '    RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_Start, v_vObjIdx:=cboList(Index).Tag, v_vControl:=cboList(Index))
        'End If


        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If
    End Sub

    Private Sub cboList_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cboList_0.Click
        Dim Index As Integer = Array.IndexOf(cboList, eventSender)

        Dim lDebugDepthCounter, lTag As Integer
        Dim vArray As Object

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : cboList_Click - " & " (" & Index & ")")
#End If

        lTag = CInt(Convert.ToString(lblList(Index).Tag))

        vArray = Array.CreateInstance(GetType(Object), New Integer() {2}, New Integer() {0})

        vArray(0) = cboList(Index).Text
        vArray(1) = cboList(Index).Text

        Dim cbolistIndex As Integer = 0


        SetScreenValues(lTag, vArray, Convert.ToString(cboList(Index).Tag), CDbl(m_vFrameArray(ACFTabNumber, GetIndex(cboList(Index).Parent, fraFrame))) + 1)
        vArray = Nothing

        m_bListChanged = False

        m_sListText = cboList(Index).Text

        ' RAW 22/06/2004 : Performance Changes(#2) : removed call to RunDynamicLogic because when m_bFireDLStartEvents = FALSE because onChange may be raised from SetScreenValues and also called from Validate event
        'If m_bFireDLStartEvents Then
        '    ' RAW 21/05/2004 : Performance Changes : added params
        '    ' RAW 22/06/2004 : Performance Changes(#2) : replaced onChange with Start
        '    RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_Start, v_vObjIdx:=cboList(Index).Tag, v_vControl:=cboList(Index))
        'End If


        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

    End Sub

    Private Sub cboList_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  ' Handles _cboList_0.GotFocus
        Dim Index As Integer = Array.IndexOf(cboList, eventSender)

        Dim lDebugDepthCounter, lTag As Integer

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : cboList_GotFocus - " & " (" & Index & ")")
#End If

        'lTag = ContainerHelper.GetControlIndex(cboList(Index).Parent)
        lTag = GetIndex(cboList(Index).Parent, fraFrame)

        SelectTab(TabStrip1, CInt(m_vFrameArray(ACFTabNumber, lTag)))
        'TabStrip1_Click(Nothing, Nothing)
        m_sListText = cboList(Index).Text

        ' RAW 21/05/2004 : Performance Changes : replaced CallScriptOnFocus with RunDynamicLogic and an OnFocus ProcedureType and added v_vControl param
        'RunDynamicLogic(enumDLProcedureType.eDLProcedureName_OnFocus, enumOnFocusAction.eOnFocusAction_Combo, Convert.ToString(cboList(Index).Tag), , cboList(Index))
        'RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_Combo, v_vObjIdx:=cboList(Index).Tag, v_vControl:=cboList(Index))

        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

    End Sub


    Private Sub cboGISLookup_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs)  'Handles _cboGISLookup_0.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        Dim Index As Integer = Array.IndexOf(cboGISLookup, eventSender)  ' RAW 21/05/2004 : Performance Changes : replaced LostFocus

        Dim lDebugDepthCounter As Integer

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : cboGISLookup_Validate - " & " (" & Index & ")")
#End If
        If m_lGisLookupIndexArray.ContainsKey(Index) Then
            m_lGisLookupIndexArray(Index) = DirectCast(DirectCast(eventSender, uctGISUserDefLookupControl.cboGISLookup).cboTypeTable, System.Windows.Forms.ComboBox).SelectedIndex
        Else
            m_lGisLookupIndexArray.Add(Index, DirectCast(DirectCast(eventSender, uctGISUserDefLookupControl.cboGISLookup).cboTypeTable, System.Windows.Forms.ComboBox).SelectedIndex)
        End If
        ' RAW 21/05/2004 : Performance Changes : added params
        ' RAW 22/06/2004 : Performance Changes(#2) : replaced onChange with OnLostFocus
        RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnLostFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_Combo, v_vObjIdx:=cboGISLookup(Index).Tag, v_vControl:=cboGISLookup(Index))


        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

        eventArgs.Cancel = Cancel
    End Sub

    Private Sub cboGISLookup_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cboGISLookup_0.Validating
        Dim Index As Integer = Array.IndexOf(cboGISLookup, eventSender)
        If m_lGisLookupIndexArray.ContainsKey(Index) Then
            DirectCast(DirectCast(eventSender, uctGISUserDefLookupControl.cboGISLookup).cboTypeTable, System.Windows.Forms.ComboBox).SelectedIndex = m_lGisLookupIndexArray(Index)
        End If
    End Sub

    Private Sub cboGISLookup_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cboGISLookup_0.Validating
        Dim Index As Integer = Array.IndexOf(cboGISLookup, eventSender)
        If m_lGisLookupIndexArray.ContainsKey(Index) Then
            m_lGisLookupIndexArray(Index) = DirectCast(DirectCast(eventSender, uctGISUserDefLookupControl.cboGISLookup).cboTypeTable, System.Windows.Forms.ComboBox).SelectedIndex
        Else
            m_lGisLookupIndexArray.Add(Index, DirectCast(DirectCast(eventSender, uctGISUserDefLookupControl.cboGISLookup).cboTypeTable, System.Windows.Forms.ComboBox).SelectedIndex)
        End If
    End Sub

    Private Sub cboList_ListIndexChange()  'Handles _cboList_0.ListIndexChange
        Dim Index As Integer = 1  ' Array.IndexOf(cboList, eventSender)
        If m_bListChanged Then
            m_sListText = cboList(Index).Text
        End If
    End Sub

    Private Sub cboList_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs)  'Handles _cboList_0.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        Dim Index As Integer = Array.IndexOf(cboList, eventSender)  ' RAW 21/05/2004 : Performance Changes : replaced LostFocus

        Dim lDebugDepthCounter As Integer

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : cboList_Validate - " & " (" & Index & ")")
#End If

        If m_bListChanged Then
            'cboList(Index).Text = m_sListText
            m_sListText = cboList(Index).Text
            cboList_Change(eventSender, eventArgs)
            m_bListChanged = False
        End If


        ' RVH - CQ2213 13/8/03 : Start
        '                        Fire dynamic logic when focus lost (changed to match other lists)
        ' RAW 21/05/2004 : Performance Changes : added params
        ' RAW 22/06/2004 : Performance Changes(#2) : replaced onChange with OnLostFocus
        RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnLostFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_Combo, v_vObjIdx:=cboList(Index).Tag, v_vControl:=cboList(Index))
        ' RVH - CQ2213 13/8/03 : End


        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

        eventArgs.Cancel = Cancel
    End Sub

    Private Sub cboPMLookup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cboPMLookup_0.Click
        Dim Index As Integer = Array.IndexOf(cboPMLookup, eventSender)

        Dim lDebugDepthCounter, lTag As Integer
        Dim vArray As Object

        If m_bSuppressDropListClicks Then Exit Sub

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : cboPMLookup_Click - " & " (" & Index & ")")
#End If

        lTag = CInt(Convert.ToString(lblPMLookup(Index).Tag))

        vArray = Array.CreateInstance(GetType(Object), New Integer() {2}, New Integer() {0})

        vArray(0) = cboPMLookup(Index).ItemId
        vArray(1) = cboPMLookup(Index).ItemCaption

        ' RAW 15/07/2003 : CQ258 : added
        If cboPMLookup(Index).IsItemDeleted(CInt(vArray(0))) And Not cboPMLookup(Index).IsLoadInProgress And Not cboPMLookup(Index).IsListIndexSetByCode Then
            ' item has been deleted from list and user has made a selection rather than being changed from code
            If MessageBox.Show(CStr(vArray(1)) & " has been deleted - Are you sure you wish to use it", "Select item from list", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                ' deselect it
                cboPMLookup(Index).ListIndex = -1
                GoTo GetOutOfHere
            End If
        End If
        ' RAW 15/07/2003 : CQ258 : end

        SetScreenValues(lTag, vArray, Convert.ToString(cboPMLookup(Index).Tag), CDbl(m_vFrameArray(ACFTabNumber, GetIndex(cboPMLookup(Index).Parent, fraFrame))) + 1)

        ' RAW 22/06/2004 : Performance Changes(#2) : removed call to RunDynamicLogic because when m_bFireDLStartEvents = FALSE because onChange may be raised from SetScreenValues and also called from Validate event
        'If m_bFireDLStartEvents Then
        '    ' RAW 21/05/2004 : Performance Changes : added params
        '    ' RAW 22/06/2004 : Performance Changes(#2) : replaced onChange with Start
        '    RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_Start, v_vObjIdx:=cboPMLookup(Index).Tag, v_vControl:=cboPMLookup(Index))
        'End If


GetOutOfHere:   ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

    End Sub

    Private Sub cboPMLookup_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cboPMLookup_0.GotFocus
        Dim Index As Integer = Array.IndexOf(cboPMLookup, eventSender)

        Dim lDebugDepthCounter, lTag As Integer

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : cboPMLookup_GotFocus - " & " (" & Index & ")")
#End If

        'lTag = ContainerHelper.GetControlIndex(cboPMLookup(Index).Parent)
        lTag = GetIndex(cboPMLookup(Index).Parent, fraFrame)

        SelectTab(TabStrip1, CInt(m_vFrameArray(ACFTabNumber, lTag)))
        'TabStrip1_Click(Nothing, Nothing)
        ' RAW 21/05/2004 : Performance Changes : replaced CallScriptOnFocus with RunDynamicLogic and an OnFocus ProcedureType and added v_vControl param
        RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_Combo, v_vObjIdx:=cboPMLookup(Index).Tag, v_vControl:=cboPMLookup(Index))

        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

    End Sub

    Private Sub cboPMLookup_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs)  'Handles _cboPMLookup_0.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        Dim Index As Integer = Array.IndexOf(cboPMLookup, eventSender)  ' RAW 21/05/2004 : Performance Changes : replaced LostFocus

        Dim lDebugDepthCounter As Integer

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : cboPMLookup_Validate - " & " (" & Index & ")")
#End If

        ' RAW 21/05/2004 : Performance Changes : added params
        ' RAW 22/06/2004 : Performance Changes(#2) : replaced onChange with OnLostFocus
        RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnLostFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_Combo, v_vObjIdx:=cboPMLookup(Index).Tag, v_vControl:=cboPMLookup(Index))

        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

        eventArgs.Cancel = Cancel
    End Sub

    Private Sub chkYesNo_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _chkYesNo_0.CheckStateChanged
        Dim Index As Integer = Array.IndexOf(chkYesNo, eventSender)

        Dim lTag As Integer
        Dim vArray() As Object

        lTag = CInt(Convert.ToString(lblCheckLabel(Index).Tag))

        vArray = Array.CreateInstance(GetType(Object), New Integer() {2}, New Integer() {0})

        simulateTriStateCheckBox(g_iCheckBoxValue, chkYesNo(Index), True)

        vArray(0) = chkYesNo(Index).CheckState
        vArray(1) = chkYesNo(Index).CheckState

        SetScreenValues(lTag, vArray, Convert.ToString(chkYesNo(Index).Tag), CDbl(m_vFrameArray(ACFTabNumber, GetIndex(chkYesNo(Index).Parent, fraFrame))) + 1)
        vArray = Nothing

        ' RAW 22/06/2004 : Performance Changes(#2) : removed call to RunDynamicLogic because when m_bFireDLStartEvents = FALSE because onChange may be raised from SetScreenValues and also called from Validate event
        If m_bFireDLStartEvents Then
            ' RAW 21/05/2004 : Performance Changes : added params
            ' RAW 22/06/2004 : Performance Changes(#2) : replaced onChange with Start
            RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_Start, v_vObjIdx:=chkYesNo(Index).Tag, v_vControl:=chkYesNo(Index))
        End If

    End Sub


    Private Sub chkYesNo_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _chkYesNo_0.Enter
        Dim Index As Integer = Array.IndexOf(chkYesNo, eventSender)

        Dim lDebugDepthCounter, lTag As Integer

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : chkYesNo_GotFocus - " & " (" & Index & ")")
#End If

        'lTag = ContainerHelper.GetControlIndex(chkYesNo(Index).Parent)
        lTag = GetIndex(chkYesNo(Index).Parent, fraFrame)

        SelectTab(TabStrip1, CInt(m_vFrameArray(ACFTabNumber, lTag)))
        'TabStrip1_Click(Nothing, Nothing)
        ' RAW 21/05/2004 : Performance Changes : replaced CallScriptOnFocus with RunDynamicLogic and an OnFocus ProcedureType and added v_vControl param
        RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_YesNo, v_vObjIdx:=chkYesNo(Index).Tag, v_vControl:=chkYesNo(Index))

        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

    End Sub

    ' RAW 14/07/2003 : CQ1563 : added
    Private Sub chkYesNo_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs)  'Handles _chkYesNo_0.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        Dim Index As Integer = Array.IndexOf(chkYesNo, eventSender)  ' RAW 21/05/2004 : Performance Changes : replaced LostFocus

        Dim lDebugDepthCounter As Integer

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : chkYesNo_Validate - " & " (" & Index & ")")
#End If

        ' RAW 04/09/2003 : CQ2370 : replaced call to genericText_LostFocus
        'genericText_LostFocus lblCheckLabel(Index), chkYesNo(Index), Index
        m_lReturn = m_oFormFields.LostFocus(chkYesNo(Index), , Index)

        ' note - do not call m_oFormFields.UnformatControlArray because this only handles 2 states (ie not "unknown")

        ' RAW 21/05/2004 : Performance Changes : added params
        ' RAW 22/06/2004 : Performance Changes(#2) : replaced onChange with OnLostFocus
        RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnLostFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_YesNo, v_vObjIdx:=chkYesNo(Index).Tag, v_vControl:=chkYesNo(Index))
        ' RAW 04/09/2003 : CQ2370 :  end

        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

        eventArgs.Cancel = Cancel
    End Sub


    Private Sub chkYesNo_KeyPress(ByVal eventSender As Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs)  'Handles _chkYesNo_0.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        Dim Index As Integer = Array.IndexOf(chkYesNo, eventSender)
        g_iCheckBoxValue = chkYesNo(Index).CheckState
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub chkYesNo_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs)  'Handles _chkYesNo_0.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        Dim Index As Integer = Array.IndexOf(chkYesNo, eventSender)
        g_iCheckBoxValue = chkYesNo(Index).CheckState
    End Sub

    Private Sub cmdAddress_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cmdAddress_0.Click
        Dim Index As Integer = Array.IndexOf(cmdAddress, eventSender)
        ' Dim iPMUAddress As Object

        Dim lDebugDepthCounter As Integer


        Dim oAddress As Object
        Dim vArray(,) As Object
        Dim vDataTypes As Object
        Dim lControlCount, lTag As Integer
        Dim vAccumulationIds As Object
        Dim lAccumulationIdLevel1, lAccumulationIdLevel2, lAccumulationIdLevel3 As Integer

        Try

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : cmdAddress_Click - " & " (" & Index & ")")
#End If

            'Can't really use this - need a component that lets us select an address or add a
            'new one...
            'And what about the contacts?!?
            'Perhaps a few parameters can be added to the address component to only show what we want

            ' Get an instance of the address interface object via
            ' the public object manager.
            Dim temp_oAddress As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oAddress, "iPMUAddress.Interface_Renamed", PMGetLocalInterface)
            oAddress = temp_oAddress

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to get address component", ACApp, ACClass, "cmdAddress_Click", Information.Err().Number, Information.Err().Description)

                Exit Sub

            End If

            lControlCount = CInt(Convert.ToString(txtAddress1(Index).Tag))

            vArray = m_vScreenValues(lControlCount)

            If Information.IsArray(vArray) Then
                'm_lReturn = ReflectionHelper.Invoke(oAddress, "SetProcessModes", New Object() {gPMConstants.PMEComponentAction.PMEdit})
                'ReflectionHelper.SetMember(oAddress, "AddressCnt", vArray(0, 0))

                m_lReturn = oAddress.SetProcessModes(gPMConstants.PMEComponentAction.PMEdit)
                oAddress.AddressCnt = vArray(0, 0)

            Else
                'm_lReturn = ReflectionHelper.Invoke(oAddress, "SetProcessModes", New Object() {gPMConstants.PMEComponentAction.PMAdd})
                'ReflectionHelper.SetMember(oAddress, "AddressCnt", 0)

                m_lReturn = oAddress.SetProcessModes(gPMConstants.PMEComponentAction.PMAdd)
                oAddress.AddressCnt = 0

            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            '    oAddress.Address1 = txtAddress1(Index).Text
            '    oAddress.Address2 = txtAddress2(Index).Text
            '    oAddress.Address3 = txtAddress3(Index).Text
            '    oAddress.Address4 = txtAddress4(Index).Text
            '    oAddress.PostalCode = txtAddress5(Index).Text

            'm_lReturn = ReflectionHelper.Invoke(oAddress, "SetKeys", New Object() {m_vWindowKeys})
            'm_lReturn = ReflectionHelper.Invoke(oAddress, "Start", New Object() {})

            m_lReturn = oAddress.SetKeys(m_vWindowKeys)
            m_lReturn = oAddress.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If
            If oAddress.Status = gPMConstants.PMEReturnCode.PMCancel Then
                'If ReflectionHelper.GetMember(oAddress, "Status") = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            ' RAW 21/05/2004 : Performance Changes : added
            ReDim vDataTypes(6)
            vDataTypes(0) = iGISSharedConstants.GISDataTypeNumeric
            vDataTypes(1) = iGISSharedConstants.GISDataTypeText
            vDataTypes(2) = iGISSharedConstants.GISDataTypeText
            vDataTypes(3) = iGISSharedConstants.GISDataTypeText
            vDataTypes(4) = iGISSharedConstants.GISDataTypeText
            vDataTypes(5) = iGISSharedConstants.GISDataTypeText
            vDataTypes(6) = iGISSharedConstants.GISDataTypeText
            ' RAW 21/05/2004 : Performance Changes : end

            vArray = Array.CreateInstance(GetType(Object), New Integer() {7, 1}, New Integer() {0, 0})

            vArray(0, 0) = ReflectionHelper.GetMember(oAddress, "AddressCnt")
            txtAddress1(Index).Text = ReflectionHelper.GetMember(oAddress, "Address1")
            txtAddress2(Index).Text = ReflectionHelper.GetMember(oAddress, "Address2")
            txtAddress3(Index).Text = ReflectionHelper.GetMember(oAddress, "Address3")
            txtAddress4(Index).Text = ReflectionHelper.GetMember(oAddress, "Address4")
            txtAddress5(Index).Text = ReflectionHelper.GetMember(oAddress, "PostalCode")
            txtAddress6(Index).Text = ReflectionHelper.GetMember(oAddress, "Country")

            vArray(1, 0) = txtAddress1(Index).Text
            vArray(2, 0) = txtAddress2(Index).Text
            vArray(3, 0) = txtAddress3(Index).Text
            vArray(4, 0) = txtAddress4(Index).Text
            vArray(5, 0) = txtAddress5(Index).Text
            vArray(6, 0) = txtAddress6(Index).Text

            lTag = CInt(Convert.ToString(cmdAddress(Index).Tag))

            ' RAW 21/05/2004 : Performance Changes : added vDataTypes param
            SetScreenValues(lControlCount, vArray, lTag, CDbl(m_vFrameArray(ACFTabNumber, GetIndex(txtAddress1(Index).Parent, fraFrame))) + 1, , (vDataTypes))
            vArray = Nothing

            'vAccumulationIds = VB6.CopyArray(oAddress.AccumulationIds)
            vAccumulationIds = oAddress.AccumulationIds
            oAddress.Dispose()

            oAddress = Nothing

            ' RAW 22/06/2004 : Performance Changes(#2) : removed call to RunDynamicLogic when m_bFireDLStartEvents = FALSE because onChange may be raised from SetScreenValues
            If m_bFireDLStartEvents Then
                ' RAW 21/05/2004 : Performance Changes : added params
                ' RAW 22/06/2004 : Performance Changes(#2) : replaced onChange with Start
                RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_Start, v_vObjIdx:=cmdAddress(Index).Tag, v_vControl:=cmdAddress(Index))
            End If

            If (m_vDataDictionary(GISDMTypeRisk)(ACPColumnName, lTag Mod 10000)).Substring(0, Math.Min((m_vDataDictionary(GISDMTypeRisk)(ACPColumnName, lTag Mod 10000)).Length, 11)).ToUpper() <> " " Then
                Exit Sub
            End If

            If Not Information.IsArray(vAccumulationIds) Then
                Exit Sub
            End If

            If vAccumulationIds.GetUpperBound(0) > 0 Then
                lAccumulationIdLevel1 = vAccumulationIds(1)
            End If

            If vAccumulationIds.GetUpperBound(0) > 1 Then
                lAccumulationIdLevel2 = vAccumulationIds(2)
            End If

            If vAccumulationIds.GetUpperBound(0) > 2 Then
                lAccumulationIdLevel3 = vAccumulationIds(3)
            End If

            If cboAccumulation.Visible Then

                Select Case cboAccumulation.AccumulationLevel
                    Case 1
                        cboAccumulation.DefaultItemId = lAccumulationIdLevel1
                    Case 2
                        cboAccumulation.DefaultItemId = lAccumulationIdLevel2
                    Case 3
                        cboAccumulation.DefaultItemId = lAccumulationIdLevel3
                    Case Else
                        cboAccumulation.DefaultItemId = 0
                End Select

                cboAccumulation.RefreshList()
            End If


        Catch ex As Exception

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "cmdAddress_ClickFailed", ACApp, ACClass, "cmdAddress_Click", Information.Err().Number, Information.Err().Description, excep:=ex)



        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

        End Try
    End Sub

    Private Sub cmdAddress_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cmdAddress_0.Enter
        Dim Index As Integer = Array.IndexOf(cmdAddress, eventSender)

        Dim lDebugDepthCounter, lTag As Integer

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : cmdAddress_GotFocus - " & " (" & Index & ")")
#End If

        lTag = GetIndex(cmdAddress(Index).Parent, fraFrame)

        SelectTab(TabStrip1, CInt(m_vFrameArray(ACFTabNumber, lTag)))
        'TabStrip1_Click(Nothing, Nothing)
        ' RAW 21/05/2004 : Performance Changes : replaced CallScriptOnFocus with RunDynamicLogic and an OnFocus ProcedureType and added v_vControl param
        'RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_FurtherDetails, v_vObjIdx:=cmdAddress(Index).Tag, v_vControl:=cmdAddress(Index))

        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

    End Sub


    Private Sub cmdStandardWordingDown_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cmdStandardWordingDown_0.Click
        Dim Index As Integer = Array.IndexOf(cmdStandardWordingDown, eventSender)

        Dim iLine As Integer
        Dim sCode, sDescription, sEdited As String
        Dim l As Integer


        Dim vDataTypes As Object
        Dim vArray(,) As Object
        Dim vTempArray(,) As Object


        Try

            'Set row to be moved - if a valid one selected
            'Is this list view empty
            If lvwStandardWording(Index).Items.Count < 1 Then
                Exit Sub
            End If

            'iLine = lvwStandardWording(Index).FocusedItem.Index + 1
            If lvwStandardWording(Index).SelectedItems.Count > 0 Then
                iLine = lvwStandardWording(Index).SelectedItems(0).Index + 1
            Else
                iLine = -1
            End If

            'Have we selected any
            If iLine = -1 Then
                Exit Sub
            End If

            'Is it really, really selected?
            If Not lvwStandardWording(Index).Items.Item(iLine - 1).Selected Then
                Exit Sub
            End If

            'If we're at the last one, there's no need to do anything
            If iLine = lvwStandardWording(Index).Items.Count Then
                Exit Sub
            End If

            'So let's swap them
            'iLine + 1 goes to temporary storage
            sCode = lvwStandardWording(Index).Items.Item(iLine).Text
            sDescription = ListViewHelper.GetListViewSubItem(lvwStandardWording(Index).Items.Item(iLine), 1).Text
            sEdited = ListViewHelper.GetListViewSubItem(lvwStandardWording(Index).Items.Item(iLine), 2).Text
            l = (Convert.ToString(lvwStandardWording(Index).Items.Item(iLine).Tag))

            'iLine goes to iLine + 1
            lvwStandardWording(Index).Items.Item(iLine).Text = lvwStandardWording(Index).Items.Item(iLine - 1).Text
            ListViewHelper.GetListViewSubItem(lvwStandardWording(Index).Items.Item(iLine), 1).Text = ListViewHelper.GetListViewSubItem(lvwStandardWording(Index).Items.Item(iLine - 1), 1).Text
            ListViewHelper.GetListViewSubItem(lvwStandardWording(Index).Items.Item(iLine), 2).Text = ListViewHelper.GetListViewSubItem(lvwStandardWording(Index).Items.Item(iLine - 1), 2).Text
            lvwStandardWording(Index).Items.Item(iLine).Tag = (Convert.ToString(lvwStandardWording(Index).Items.Item(iLine - 1).Tag))

            'temporary storage goes to iLine
            lvwStandardWording(Index).Items.Item(iLine - 1).Text = sCode
            ListViewHelper.GetListViewSubItem(lvwStandardWording(Index).Items.Item(iLine - 1), 1).Text = sDescription
            ListViewHelper.GetListViewSubItem(lvwStandardWording(Index).Items.Item(iLine - 1), 2).Text = sEdited
            lvwStandardWording(Index).Items.Item(iLine - 1).Tag = CStr(l)

            'Reset the selected item, automatically deselects the previous one
            lvwStandardWording(Index).Items.Item(iLine).Selected = True

            cmdStandardWordingDelete(Index).Enabled = False

            'swap the data array also
            iLine -= 1
            vArray = m_vScreenValues(CInt(Convert.ToString(cmdStandardWordingAdd(Index).Tag)))

            ReDim vTempArray(3, 0)
            'Store the up record to temp array
            vTempArray(0, 0) = vArray(0, iLine + 1)
            vTempArray(1, 0) = vArray(1, iLine + 1)
            vTempArray(2, 0) = vArray(2, iLine + 1)
            vTempArray(3, 0) = vArray(3, iLine + 1)

            'swap the up record with current
            vArray(0, iLine + 1) = vArray(0, iLine)
            vArray(1, iLine + 1) = vArray(1, iLine)
            vArray(2, iLine + 1) = vArray(2, iLine)
            vArray(3, iLine + 1) = vArray(3, iLine)

            'swap the current record with up record data which is stored in temp array
            vArray(0, iLine) = (vTempArray(0, 0))
            vArray(1, iLine) = (vTempArray(1, 0))
            vArray(2, iLine) = (vTempArray(2, 0))
            vArray(3, iLine) = (vTempArray(3, 0))


            ReDim vDataTypes(3)
            vDataTypes(0) = iGISSharedConstants.GISDataTypeText
            vDataTypes(2) = iGISSharedConstants.GISDataTypeText
            vDataTypes(3) = iGISSharedConstants.GISDataTypeText

            SetScreenValues(CInt(Convert.ToString(cmdStandardWordingAdd(Index).Tag)), vArray, Convert.ToString(lvwStandardWording(Index).Tag), CDbl(m_vFrameArray(ACFTabNumber, GetIndex(lvwStandardWording(Index).Parent, fraFrame))) + 1, , (vDataTypes))




            lvwStandardWording(Index).Focus()

        Catch excep As System.Exception



            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdStandardWordingDown_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdStandardWordingDown_Click", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdListViewAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cmdListViewAdd_0.Click
        Dim Index As Integer = Array.IndexOf(cmdListViewAdd, eventSender)

        Dim lDebugDepthCounter, lTag, lScreenId As Integer
        Dim sParentOIKey, sChildOIKey, sParentObjectName, sChildObjectName As String
        Dim vArray(,) As Object
        Dim vChildScreenValues As Object
        Dim cUniqueObjects As Collection
        Dim vParentKeys() As String
        Dim bTalkedToPerson, bIsInsured As Boolean
        Dim sMessage As String = ""
        Dim vGISPropertyValue As String = ""
        ' PW141103 - CQ2772
        Dim lDisclosureTypeID As Integer
        Dim lChildStatus As gPMConstants.PMEReturnCode
        Dim lScreenDetailsIndex As Integer  ' RAW 09/07/2004 : JIT : added and to be used instead of m_lControlCount
        Dim lSequenceNumberColumn As Integer

        Try


            'Pretty much the same as add.
            'We identify which one we're doing, then get the interface to give it a go

            'This is the control
            lScreenDetailsIndex = CInt(CDbl(Convert.ToString(cmdListViewAdd(Index).Tag)) Mod 10000)

            'This is the array of values
            vArray = m_vScreenValues(lScreenDetailsIndex)

            'New item
            'And these are its keys

            sChildOIKey = ""
            sParentOIKey = CStr(vArray(0, vArray.GetUpperBound(1) - 1))
            sChildObjectName = CStr(vArray(0, vArray.GetUpperBound(1) - 2))
            sParentObjectName = CStr(vArray(0, vArray.GetUpperBound(1) - 3))

            Select Case m_vScreenDetails(ACDExtraObjectType, lScreenDetailsIndex)
                Case GISOTRisk, GISOTCase
                    lScreenId = CInt(m_vScreenDetails(ACDChildScreenId, lScreenDetailsIndex))
                Case GISOTNonGisSpecials, GISOTClaim
                Case GISOTAssociatedClient
                    lScreenId = m_vRiskTypeDetails(ACRAssociatedClientScreenId, 0)
                    If lScreenId < 1 Then
                        Interaction.MsgBox("No Associated Client Screen has been defined for this risk type", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "Cannot display child screen")
                        Exit Sub
                    End If
                    'despite what is held in the array, the parent is always the top level object

                    ReflectionHelper.Invoke(ReflectionHelper.GetMember(Interface_Renamed, "GIS"), "GetALLOIKey", New Object() {m_sGISDataModel & "_policy_binder", vParentKeys})
                    m_lReturn = m_oInterface.GIS.GetPropertyValue(v_sObjectName:=sChildObjectName, v_sPropertyName:="talked_to_person", v_sOIKey:=sChildOIKey, r_vPropertyValue:=vGISPropertyValue)
                    '            m_lReturn = ReflectionHelper.Invoke(ReflectionHelper.GetMember(m_oInterface, "GIS"), "GetPropertyValue", New Object() {sChildObjectName, "talked_to_person", sChildOIKey, vGISPropertyValue})


                    If Information.IsArray(vParentKeys) Then
                        sParentOIKey = vParentKeys(vParentKeys.GetUpperBound(0))
                    End If

                Case GISOTDisclosure
                    lScreenId = m_vRiskTypeDetails(ACRDisclosureScreenId, 0)
                    If lScreenId < 1 Then
                        Interaction.MsgBox("No Disclosure Screen has been defined for this risk type", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "Cannot display child screen")
                        Exit Sub
                    End If
                Case GISOTPeril

            End Select


            'ReflectionHelper.SetMember(m_oInterface, "Task", m_iTask)

            m_oInterface.Task = m_iTask
            m_oInterface.ObjectType = m_vScreenDetails(ACDExtraObjectType, lScreenDetailsIndex)
            m_oInterface.ChildIndex = lTag

            ' RAW 22/07/2003 : CQ1783 : copied code from 1.8.6")
            'DP 30/05/2003 - added array to pass data to the child
            ' RAW 14/10/2003 : CQ2754 : added r_vMyScreenValues param
            ' PW150604 - CQ3821 - get status of sub screen returned
            bCommandFromChildForm = True
            m_lReturn = m_oInterface.DisplaySubScreen(lScreenId:=lScreenId, sParentOIKey:=sParentOIKey, sChildOIKey:=sChildOIKey, sParentObjectName:=sParentObjectName, sChildObjectName:=sChildObjectName, r_vMyScreenValues:=m_vScreenValues, r_vSubScreenValues:=vChildScreenValues, vRiskTypeDetails:=m_vRiskTypeDetails, vData:=m_aDataToChild, r_lStatus:=lChildStatus)

            'MyBase.ParentForm.BringToFront()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' PW150604 - CQ3821 - exit if subscreen was cancelled
            ' RAW 22/06/2004 : Performance Changes(#2) : moved from after RefreshScreenControls
            If lChildStatus = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'if there is a sequence number column then set it to the correct value
            lSequenceNumberColumn = CInt(Convert.ToString(cmdListViewSequenceUp(Index).Tag))
            If lSequenceNumberColumn <> -1 Then
                'set the new sequence number by counting the non deleted entries
                m_vScreenValues(lScreenDetailsIndex)(m_vScreenValues(lScreenDetailsIndex).GetUpperBound(0), lSequenceNumberColumn) = m_vScreenValues(lScreenDetailsIndex).GetUpperBound(0)

                'this sets the value in the GIS so that the child record is correct


                m_lReturn = m_oInterface.GIS.SetPropertyValue(v_sObjectName:=m_vScreenDetails(ACDExtraGISObjectName, lScreenDetailsIndex), v_sPropertyName:=cProperty_SequenceId, v_sOIKey:=sChildOIKey, v_vPropertyValue:=m_vScreenValues(lScreenDetailsIndex)(m_vScreenValues(lScreenDetailsIndex).GetUpperBound(0), lSequenceNumberColumn))
                'm_lReturn =m_oInterface, "GIS"), "SetPropertyValue", New Object() {m_vScreenDetails(ACDExtraGISObjectName, lScreenDetailsIndex), cProperty_SequenceId, sChildOIKey, m_vScreenValues(lScreenDetailsIndex)(m_vScreenValues(lScreenDetailsIndex).GetUpperBound(0), lSequenceNumberColumn)})

            End If

            ' RAW 03/11/2003 : CQ2754/2862 : added v_lListViewIndex param
            m_lReturn = RefreshScreenControls(m_vScreenValues, Index)

            ' SET 02082002 - Scalability
            cUniqueObjects = Nothing

            If CDbl(m_vScreenDetails(ACDExtraObjectType, lScreenDetailsIndex)) = GISOTDisclosure Then

                ' RAW 30/09/2003 : CQ2673 : replaced use of lTag to locate GIS item


                m_lReturn = m_oInterface.GIS.GetPropertyValue(v_sObjectName:=sChildObjectName, v_sPropertyName:="talked_to_person", v_sOIKey:=sChildOIKey, r_vPropertyValue:=vGISPropertyValue)
                '            m_lReturn = ReflectionHelper.Invoke(ReflectionHelper.GetMember(m_oInterface, "GIS"), "GetPropertyValue", New Object() {sChildObjectName, "talked_to_person", sChildOIKey, vGISPropertyValue})
                ' PW141103 - CQ2772 - get correct value
                bTalkedToPerson = (Conversion.Val(vGISPropertyValue) <> 0)

                m_lReturn = m_oInterface.GIS.GetPropertyValue(v_sObjectName:=sParentObjectName, v_sPropertyName:="is_insured", v_sOIKey:=sParentOIKey, r_vPropertyValue:=vGISPropertyValue)
                ' m_lReturn = ReflectionHelper.Invoke(ReflectionHelper.GetMember(m_oInterface, "GIS"), "GetPropertyValue", New Object() {sParentObjectName, "is_insured", sParentOIKey, vGISPropertyValue})
                ' PW141103 - CQ2772 - get correct value
                bIsInsured = (Conversion.Val(vGISPropertyValue) <> 0)

                ' PW141103 - CQ2772 - get the Disclosure Type ID


                m_lReturn = m_oInterface.GIS.GetPropertyValue(v_sObjectName:=sChildObjectName, v_sPropertyName:="disclosure_type", v_sOIKey:=sChildOIKey, r_vPropertyValue:=vGISPropertyValue)
                '            m_lReturn = ReflectionHelper.Invoke(ReflectionHelper.GetMember(m_oInterface, "GIS"), "GetPropertyValue", New Object() {sChildObjectName, "disclosure_type", sChildOIKey, vGISPropertyValue})
                lDisclosureTypeID = CInt(Conversion.Val(vGISPropertyValue))

                ' PW200104 - CQ3894 - Show warning if Insured and not Talked to,
                ' or if non-insured (i.e. all other scenarios)
                If bTalkedToPerson And bIsInsured Then
                    m_lReturn = ShowPolicyRiskList()
                Else
                    sMessage = (iPMFunc.GetResData(g_iLanguageID, ACWarningUpdateRiskPolicies1, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    MessageBox.Show(sMessage, m_sScreenDesc, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If

            bCommandFromChildForm = False
        Catch ex As Exception

            ' RAW 22/06/2004 : added
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "cmdListViewAdd_Click Failed", ACApp, ACClass, "cmdListViewAdd_Click", Information.Err().Number, Information.Err().Description, excep:=ex)

            ' SET 02082002 - Scalability
            cUniqueObjects = Nothing

            ' Error Section



        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

        End Try
    End Sub

    Private Sub cmdListViewDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cmdListViewDelete_0.Click
        Dim Index As Integer = Array.IndexOf(cmdListViewDelete, eventSender)

        Dim lDebugDepthCounter, lTag, lScreenId As Integer
        Dim sParentOIKey, sChildOIKey, sParentObjectName, sChildObjectName As String
        Dim vArray(,) As Object
        Dim bTalkedToPerson, bIsInsured As Boolean
        Dim sMessage As String = ""
        Dim vGISPropertyValue As String = ""
        ' PW141103 - CQ2772
        Dim lDisclosureTypeID, lScreenDetailsIndex As Integer  ' RAW 09/07/2004 : JIT : added and to be used instead of m_lControlCount
        Dim lSequenceNumberColumn, lSequenceNumber As Integer


        '
        Try
#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

            AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : cmdListViewDelete_Click - " & " (" & Index & ")")
#End If

            'This is the control
            lScreenDetailsIndex = CInt(CDbl(Convert.ToString(cmdListViewAdd(Index).Tag)) Mod 10000)

            'This is the array of values
            vArray = m_vScreenValues(lScreenDetailsIndex)

            'This is the item
            lTag = (Convert.ToString(lvwListView(Index).Items.Item(lvwListView(Index).FocusedItem.Index).Tag))

            'And these are its key
            sChildOIKey = CStr(vArray(lTag, vArray.GetUpperBound(1)))
            sParentOIKey = CStr(vArray(lTag, vArray.GetUpperBound(1) - 1))
            sChildObjectName = CStr(vArray(lTag, vArray.GetUpperBound(1) - 2))
            sParentObjectName = CStr(vArray(lTag, vArray.GetUpperBound(1) - 3))

            lScreenId = CInt(m_vScreenDetails(ACDChildScreenId, lScreenDetailsIndex))



            Select Case m_vScreenDetails(ACDExtraObjectType, lScreenDetailsIndex)
                Case GISOTAssociatedClient

                    ' RAW 30/09/2003 : CQ2673 : replaced use of lTag to locate GIS item

                    m_lReturn = m_oInterface.GIS.GetPropertyValue(v_sObjectName:=sChildObjectName, v_sPropertyName:="is_insured", v_sOIKey:=sChildOIKey, r_vPropertyValue:=vGISPropertyValue)
                    ' m_lReturn = ReflectionHelper.Invoke(ReflectionHelper.GetMember(m_oInterface, "GIS"), "GetPropertyValue", New Object() {sChildObjectName, "is_insured", sChildOIKey, vGISPropertyValue})
                    ' RAW 09/12/2003 : CQ2841 : changed from <> "0" to handle ""
                    bIsInsured = (vGISPropertyValue = "1")

                    If bIsInsured Then
                        sMessage = (iPMFunc.GetResData(g_iLanguageID, ACWarningCannotDeleteAssPerson, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        MessageBox.Show(sMessage, m_sScreenDesc, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        GoTo GetOutOfHere

                    Else
                        sMessage = (iPMFunc.GetResData(g_iLanguageID, ACQuestionDeleteAssPerson, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        If MessageBox.Show(sMessage, m_sScreenDesc, MessageBoxButtons.OKCancel, MessageBoxIcon.Error) = System.Windows.Forms.DialogResult.Cancel Then
                            GoTo GetOutOfHere
                        End If

                    End If

                Case GISOTDisclosure

                    ' RAW 30/09/2003 : CQ2673 : replaced use of lTag to locate GIS item


                    m_lReturn = m_oInterface.GIS.GetPropertyValue(v_sObjectName:=sChildObjectName, v_sPropertyName:="talked_to_person", v_sOIKey:=sChildOIKey, r_vPropertyValue:=vGISPropertyValue)
                    '                    m_lReturn = ReflectionHelper.Invoke(ReflectionHelper.GetMember(m_oInterface, "GIS"), "GetPropertyValue", New Object() {sChildObjectName, "talked_to_person", sChildOIKey, vGISPropertyValue})
                    ' PW141103 - CQ2772 - get correct value
                    bTalkedToPerson = (Conversion.Val(vGISPropertyValue) <> 0)

                    ' RAW 30/09/2003 : CQ2673 : replaced use of lTag to locate GIS item


                    m_lReturn = m_oInterface.GIS.GetPropertyValue(v_sObjectName:=sParentObjectName, v_sPropertyName:="is_insured", v_sOIKey:=sParentOIKey, r_vPropertyValue:=vGISPropertyValue)
                    '                    m_lReturn = ReflectionHelper.Invoke(ReflectionHelper.GetMember(m_oInterface, "GIS"), "GetPropertyValue", New Object() {sParentObjectName, "is_insured", sParentOIKey, vGISPropertyValue})
                    ' PW141103 - CQ2772 - get correct value
                    bIsInsured = (Conversion.Val(vGISPropertyValue) <> 0)

                    ' PW141103 - CQ2772 - get the Disclosure Type ID

                    m_lReturn = m_oInterface.GIS.GetPropertyValue(v_sObjectName:=sChildObjectName, v_sPropertyName:="disclosure_type", v_sOIKey:=sChildOIKey, r_vPropertyValue:=vGISPropertyValue)
                    '                    m_lReturn = ReflectionHelper.Invoke(ReflectionHelper.GetMember(m_oInterface, "GIS"), "GetPropertyValue", New Object() {sChildObjectName, "disclosure_type", sChildOIKey, vGISPropertyValue})
                    lDisclosureTypeID = CInt(Conversion.Val(vGISPropertyValue))

                    If bTalkedToPerson And bIsInsured Then
                        sMessage = (iPMFunc.GetResData(g_iLanguageID, ACWarningUpdateRiskPolicies2, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        If MessageBox.Show(sMessage, m_sScreenDesc, MessageBoxButtons.OKCancel, MessageBoxIcon.Error) = System.Windows.Forms.DialogResult.Cancel Then
                            GoTo GetOutOfHere
                        End If

                        ' PW030204 - CQ4076 - show warning in other cases
                        ' (put back in PW240204 as accidently removed in version 11 (04/02/04))
                    Else
                        sMessage = (iPMFunc.GetResData(g_iLanguageID, ACWarningUpdateRiskPolicies1, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        MessageBox.Show(sMessage, m_sScreenDesc, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If

            End Select

            m_lReturn = ReflectionHelper.Invoke(m_oInterface, "DelObjectInstance", New Object() {sChildObjectName, sChildOIKey})

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GoTo GetOutOfHere
            End If

            lvwListView(Index).Items.RemoveAt(lvwListView(Index).FocusedItem.Index)

            'Reset the keys
            vArray(lTag, vArray.GetUpperBound(1)) = "dElEtEd"
            vArray(lTag, vArray.GetUpperBound(1) - 1) = "dElEtEd"
            vArray(lTag, vArray.GetUpperBound(1) - 2) = "dElEtEd"
            vArray(lTag, vArray.GetUpperBound(1) - 3) = "dElEtEd"

            'if there is a sequence number column then we need to check if we need to resequence any entries
            lSequenceNumberColumn = CInt(Convert.ToString(cmdListViewSequenceUp(Index).Tag))
            If lSequenceNumberColumn <> -1 Then
                lSequenceNumber = CInt(vArray(lTag, lSequenceNumberColumn))

                vArray(lTag, lSequenceNumberColumn) = DBNull.Value
                For iTarget As Integer = 1 To vArray.GetUpperBound(0)
                    If CDbl(vArray(iTarget, lSequenceNumberColumn)) > lSequenceNumber Then
                        'set the screen details array
                        vArray(iTarget, lSequenceNumberColumn) = CDbl(vArray(iTarget, lSequenceNumberColumn)) - 1
                        'set the value in the GIS





                        m_lReturn = ReflectionHelper.Invoke(ReflectionHelper.GetMember(m_oInterface, "GIS"), "SetPropertyValue", New Object() {sChildObjectName, cProperty_SequenceId, vArray(iTarget, vArray.GetUpperBound(1)), vArray(iTarget, lSequenceNumberColumn)})
                    End If
                Next
            End If

            SetScreenValues(lScreenDetailsIndex, vArray, Convert.ToString(lvwListView(Index).Tag), CDbl(m_vFrameArray(ACFTabNumber, GetIndex(lvwListView(Index).Parent, fraFrame))) + 1, "")

            ' RAW 03/11/2003 : CQ2754/2862 : added v_lControlCount param
            ' RAW 22/06/2004 : Performance Changes(#2) : added SuppressDynamicLogic param since we have already raised an onChange event from SetScreenValues
            bCommandFromChildForm = True
            m_lReturn = PopulateListView(Index, lScreenId, lScreenDetailsIndex, False, True)


            ' RAW 14/10/2004 : CQ5119 : added
            If CDbl(m_vScreenDetails(ACDExtraObjectType, lScreenDetailsIndex)) = GISOTDisclosure Then

                ' Show list of affected policies if Insured and disclosure WAS Talked to,
                If bTalkedToPerson And bIsInsured Then

                    m_lReturn = ShowPolicyRiskList()
                End If
            End If
            ' RAW 14/10/2004 : CQ5119 : end

            'disable edit and delete until we are sure focus a list view entry has been reselected.

            If lvwListView(Index).Items.Count > 0 Then
                If lvwListView(Index).SelectedItems.Count > 0 Then
                    lTag = lvwListView(Index).FocusedItem.Index + 1
                Else
                    lTag = -1
                    cmdListViewEdit(Index).Enabled = False
                    cmdListViewDelete(Index).Enabled = False
                End If
            Else
                'If Information.Err().Number <> 0 Then
                cmdListViewEdit(Index).Enabled = False
                cmdListViewDelete(Index).Enabled = False

            End If
            bCommandFromChildForm = False
            GoTo GetOutOfHere

Err_cmdListViewDeleteClick:

            ' Error Section

            ' RAW 22/06/2004 : added
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdListViewDelete_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdListViewDelete_Click", excep:=New Exception(Information.Err().Description))

            GoTo GetOutOfHere



GetOutOfHere:   ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
            ' Debug message
#If (DebugOption) Then
            If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

        Catch exc As System.Exception
        End Try

    End Sub


    Private Sub cmdListViewEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cmdListViewEdit_0.Click
        Dim Index As Integer = Array.IndexOf(cmdListViewEdit, eventSender)

        Dim lDebugDepthCounter, lTag, lScreenId As Integer
        Dim sParentOIKey, sChildOIKey, sParentObjectName, sChildObjectName As String
        Dim vArray(,) As Object
        Dim vChildScreenValues As Object
        Dim cUniqueObjects As Collection
        Dim bTalkedToPerson, bTalkedToPersonOld, bIsInsured As Boolean
        Dim sMessage As String = ""
        Dim vGISPropertyValue As String = ""
        Dim lDisclosureTypeID As Integer
        Dim lChildStatus As gPMConstants.PMEReturnCode
        Dim lScreenDetailsIndex As Integer  ' RAW 09/07/2004 : JIT : added and to be used instead of m_lControlCount
        Dim lSequenceNumberColumn As Integer
        Dim vSequenceNumber As Object
        Dim lCurrentSelection As Integer

        Try

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : cmdListViewEdit_Click - " & " (" & Index & ")")
#End If

            If Not cmdListViewEdit(Index).Enabled Then
                'edit not enabled so exit
                Exit Sub
            End If


            ' SET 02082002
            cUniqueObjects = New Collection()

            'Pretty much the same as add.
            'We identify which one we're doing, then get the interface to give it a go

            'This is the control
            lScreenDetailsIndex = CInt(CDbl(Convert.ToString(cmdListViewAdd(Index).Tag)) Mod 10000)

            'This is the array of values
            vArray = m_vScreenValues(lScreenDetailsIndex)

            'This is the item
            lTag = (Convert.ToString(lvwListView(Index).Items.Item(lvwListView(Index).FocusedItem.Index).Tag))
            lCurrentSelection = lvwListView(Index).FocusedItem.Index + 1

            'And these are its key
            sChildOIKey = CStr(vArray(lTag, vArray.GetUpperBound(1)))
            sParentOIKey = CStr(vArray(lTag, vArray.GetUpperBound(1) - 1))
            sChildObjectName = CStr(vArray(lTag, vArray.GetUpperBound(1) - 2))
            sParentObjectName = CStr(vArray(lTag, vArray.GetUpperBound(1) - 3))

            Select Case m_vScreenDetails(ACDExtraObjectType, lScreenDetailsIndex)
                Case GISOTRisk, GISOTCase
                    lScreenId = CInt(m_vScreenDetails(ACDChildScreenId, lScreenDetailsIndex))
                Case GISOTNonGisSpecials, GISOTClaim
                Case GISOTAssociatedClient
                    lScreenId = m_vRiskTypeDetails(ACRAssociatedClientScreenId, 0)
                    If lScreenId < 1 Then
                        Interaction.MsgBox("No Associated Client Screen has been defined for this risk type", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "Cannot display child screen")
                        Exit Sub
                    End If
                    sParentOIKey = "OI1"  'despite what is held in the array, the parent is always the top level object
                Case GISOTDisclosure
                    lScreenId = m_vRiskTypeDetails(ACRDisclosureScreenId, 0)
                    If lScreenId < 1 Then
                        Interaction.MsgBox("No Disclosure Screen has been defined for this risk type", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "Cannot display child screen")
                        Exit Sub
                    End If


                    ' RAW 30/09/2003 : CQ2673 : replaced use of lTag to locate GIS item
                    m_lReturn = m_oInterface.GIS.GetPropertyValue(v_sObjectName:=sChildObjectName, v_sPropertyName:="talked_to_person", v_sOIKey:=sChildOIKey, r_vPropertyValue:=vGISPropertyValue)
                    '               m_lReturn = ReflectionHelper.Invoke(ReflectionHelper.GetMember(m_oInterface, "GIS"), "GetPropertyValue", New Object() {sChildObjectName, "talked_to_person", sChildOIKey, vGISPropertyValue})
                    ' PW141103 - CQ2772 - get correct value
                    bTalkedToPerson = (Conversion.Val(vGISPropertyValue) <> 0)

                    ' RAW 30/09/2003 : CQ2673 : replaced use of lTag to locate GIS item

                    m_lReturn = m_oInterface.GIS.GetPropertyValue(v_sObjectName:=sParentObjectName, v_sPropertyName:="is_insured", v_sOIKey:=sParentOIKey, r_vPropertyValue:=vGISPropertyValue)
                    'm_lReturn = ReflectionHelper.Invoke(ReflectionHelper.GetMember(m_oInterface, "GIS"), "GetPropertyValue", New Object() {sParentObjectName, "is_insured", sParentOIKey, vGISPropertyValue})

                    ' PW141103 - CQ2772 - get correct value
                    bIsInsured = (Conversion.Val(vGISPropertyValue) <> 0)

                    ' PW150604 - CQ3821 - don't show message if in view mode
                    If bTalkedToPerson And bIsInsured And m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                        sMessage = (iPMFunc.GetResData(g_iLanguageID, ACWarningUpdateRiskPolicies2, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        MessageBox.Show(sMessage, m_sScreenDesc, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                Case GISOTPeril

            End Select

            m_oInterface.Task = IIf((cmdListViewEdit(Index).Text = "Edit" Or cmdListViewEdit(Index).Text = "&Edit"), m_iTask, PMEComponentAction.PMView)
            m_oInterface.ObjectType = m_vScreenDetails(ACDExtraObjectType, lScreenDetailsIndex)
            m_oInterface.ChildIndex = lTag


            'if there is a sequence number column then get it so we can set it to the correct value
            lSequenceNumberColumn = CInt(Convert.ToString(cmdListViewSequenceUp(Index).Tag))
            If lSequenceNumberColumn <> -1 Then
                'this gets the value from the GIS so it can be reset afterwards


                m_lReturn = m_oInterface.GIS.GetPropertyValue(v_sObjectName:=m_vScreenDetails(ACDExtraGISObjectName, lScreenDetailsIndex), v_sPropertyName:=cProperty_SequenceId, v_sOIKey:=sChildOIKey, r_vPropertyValue:=vSequenceNumber)
                'm_lReturn = ReflectionHelper.Invoke(ReflectionHelper.GetMember(m_oInterface, "GIS"), "GetPropertyValue", New Object() {m_vScreenDetails(ACDExtraGISObjectName, lScreenDetailsIndex), cProperty_SequenceId, sChildOIKey, vSequenceNumber})
            End If

            'KeepWindowOnTop(False)
            bCommandFromChildForm = True
            m_lReturn = m_oInterface.DisplaySubScreen(lScreenId:=lScreenId, sParentOIKey:=sParentOIKey, sChildOIKey:=sChildOIKey, sParentObjectName:=sParentObjectName, sChildObjectName:=sChildObjectName, r_vMyScreenValues:=m_vScreenValues, r_vSubScreenValues:=vChildScreenValues, vRiskTypeDetails:=m_vRiskTypeDetails, vData:=m_aDataToChild, r_lStatus:=lChildStatus)
            'm_lReturn = ReflectionHelper.Invoke(m_oInterface, "DisplaySubScreen", New Object() {lScreenId, sParentOIKey, sChildOIKey, sParentObjectName, sChildObjectName, m_vScreenValues, vChildScreenValues, m_vRiskTypeDetails, m_aDataToChild, lChildStatus})
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'KeepWindowOnTop(True)
            ' PW150604 - CQ3821 - exit if subscreen was cancelled
            ' RAW 22/06/2004 : Performance Changes(#2) : moved from after RefreshScreenControls
            If lChildStatus = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'if there is a sequence number column then set it to the correct value
            lSequenceNumberColumn = CInt(Convert.ToString(cmdListViewSequenceUp(Index).Tag))
            If lSequenceNumberColumn <> -1 Then
                'To ensure that the list view displays correctly we must update screenvalues
                'The edit may have removed deleted items from the array and invalidated lTag so look for the oikey
                For lTemp As Integer = 1 To m_vScreenValues(lScreenDetailsIndex).GetUpperBound(0)
                    If CStr(m_vScreenValues(lScreenDetailsIndex)(lTemp, m_vScreenValues(lScreenDetailsIndex).GetUpperBound(1))) = sChildOIKey Then
                        m_vScreenValues(lScreenDetailsIndex)(lTemp, lSequenceNumberColumn) = (vSequenceNumber)
                    End If
                Next
                'this sets the value in the GIS so that the child record is correct



                m_lReturn = m_oInterface.GIS.SetPropertyValue(v_sObjectName:=m_vScreenDetails(ACDExtraGISObjectName, lScreenDetailsIndex), v_sPropertyName:=cProperty_SequenceId, v_sOIKey:=sChildOIKey, v_vPropertyValue:=vSequenceNumber)
                'm_lReturn = ReflectionHelper.Invoke(ReflectionHelper.GetMember(m_oInterface, "GIS"), "SetPropertyValue", New Object() {m_vScreenDetails(ACDExtraGISObjectName, lScreenDetailsIndex), cProperty_SequenceId, sChildOIKey, vSequenceNumber})
            End If
            ' RAW 14/10/2003 : CQ2754 : added
            ' RAW 03/11/2003 : CQ2754/2862 : added v_lListViewIndex param
            m_lReturn = RefreshScreenControls(m_vScreenValues, Index)
            ' RAW 14/10/2003 : CQ2754 : end

            'select original line
            lvwListView(Index).Items.Item(lCurrentSelection - 1).Selected = True


            ' RAW 03/11/2003 : CQ2754/2862 : removed code from here that refreshed m_vScreenValues from vChildScreenValues since it is now done within DisplaySubScreen


            ' SET 02082002
            cUniqueObjects = Nothing


            'CJR 12/03/2003 Display Policy Risk List
            If CDbl(m_vScreenDetails(ACDExtraObjectType, lScreenDetailsIndex)) = GISOTDisclosure Then

                ' PW141103 - CQ2072 - re-get Talked To Person in case it was changed
                ' during edit




                m_lReturn = m_oInterface.GIS.GetPropertyValue(v_sObjectName:=sChildObjectName, v_sPropertyName:="talked_to_person", v_sOIKey:=sChildOIKey, r_vPropertyValue:=vGISPropertyValue)
                'm_lReturn = ReflectionHelper.Invoke(ReflectionHelper.GetMember(m_oInterface, "GIS"), "GetPropertyValue", New Object() {sChildObjectName, "talked_to_person", sChildOIKey, vGISPropertyValue})

                bTalkedToPersonOld = bTalkedToPerson  ' RAW 14/10/2004 : CQ5119 : added
                bTalkedToPerson = (Conversion.Val(vGISPropertyValue) <> 0)

                ' PW141103 - CQ2772 - get the Disclosure Type ID



                m_lReturn = m_oInterface.GIS.GetPropertyValue(v_sObjectName:=sChildObjectName, v_sPropertyName:="disclosure_type", v_sOIKey:=sChildOIKey, r_vPropertyValue:=vGISPropertyValue)
                'm_lReturn = ReflectionHelper.Invoke(ReflectionHelper.GetMember(m_oInterface, "GIS"), "GetPropertyValue", New Object() {sChildObjectName, "disclosure_type", sChildOIKey, vGISPropertyValue})
                lDisclosureTypeID = CInt(Conversion.Val(vGISPropertyValue))

                ' PW200104 - CQ3894 - Show warning if Insured and not Talked to,
                ' or if non-insured (i.e. all other scenarios)
                If bIsInsured Then
                    ' RAW 14/10/2004 : CQ5119 : added test for bTalkedToPersonOld
                    If bTalkedToPerson Or (bTalkedToPerson <> bTalkedToPersonOld) Then
                        m_lReturn = ShowPolicyRiskList()
                    Else
                        sMessage = (iPMFunc.GetResData(g_iLanguageID, ACWarningUpdateRiskPolicies1, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        MessageBox.Show(sMessage, m_sScreenDesc, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End If
            End If

            lvwListView(Index).Focus()

            bCommandFromChildForm = False
        Catch ex As Exception

            ' RAW 22/06/2004 : added
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "cmdListViewEdit_Click Failed", ACApp, ACClass, "cmdListViewEdit_Click", Information.Err().Number, Information.Err().Description, excep:=ex)

            ' SET 02082002
            cUniqueObjects = Nothing

            ' Error Section



        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

        End Try
    End Sub

    Private Sub cmdListViewEdit_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cmdListViewEdit_0.Enter
        Dim Index As Integer = Array.IndexOf(cmdListViewEdit, eventSender)

        Dim lDebugDepthCounter As Integer

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : cmdListViewEdit_GotFocus - " & " (" & Index & ")")
#End If

        ' RAW 21/05/2004 : Performance Changes : replaced CallScriptOnFocus with RunDynamicLogic and an OnFocus ProcedureType and added v_vControl param
        'RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_Edit, v_vObjIdx:=lvwListView(Index).Tag, v_vPropIdx:="", v_vControl:=cmdListViewEdit(Index))
        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

    End Sub
    Private Sub cmdListViewDelete_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cmdListViewDelete_0.Enter
        Dim Index As Integer = Array.IndexOf(cmdListViewDelete, eventSender)

        Dim lDebugDepthCounter As Integer

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : cmdListViewDelete_GotFocus - " & " (" & Index & ")")
#End If

        ' RAW 21/05/2004 : Performance Changes : replaced CallScriptOnFocus with RunDynamicLogic and an OnFocus ProcedureType and added v_vControl param
        'RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_Delete, v_vObjIdx:=lvwListView(Index).Tag, v_vPropIdx:="", v_vControl:=cmdListViewDelete(Index))
        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

    End Sub
    Private Sub cmdListViewAdd_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cmdListViewAdd_0.Enter
        Dim Index As Integer = Array.IndexOf(cmdListViewAdd, eventSender)

        Dim lDebugDepthCounter As Integer

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : cmdListViewAdd_GotFocus - " & " (" & Index & ")")
#End If

        ' RAW 21/05/2004 : Performance Changes : replaced CallScriptOnFocus with RunDynamicLogic and an OnFocus ProcedureType and added v_vControl param
        'RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_Add, v_vObjIdx:=lvwListView(Index).Tag, v_vPropIdx:="", v_vControl:=cmdListViewAdd(Index))
        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdStandardWordingEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) 'Handles _cmdStandardWordingEdit_0.Click
        Dim Index As Integer = Array.IndexOf(cmdStandardWordingEdit, eventSender)
        Dim nDebugDepthCounter As Integer = 0
        Dim sDocumentTemplateID As String = ""
        Dim nLine As Integer = 0
        Dim aoArray(,) As Object = Nothing
        Dim oDataTypes As Object = Nothing
        Dim sEditCode As String = String.Empty
        Dim sEditDescription As String = String.Empty

        Try
#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

            AddToDebug(r_lDepthCounter:=nDebugDepthCounter, v_sText:="EVENT : cmdStandardWordingEdit_Click ")
#End If

            If lvwStandardWording(Index).Items.Count < 1 Then
                Exit Sub
            End If

            sDocumentTemplateID = (Convert.ToString(lvwStandardWording(Index).FocusedItem.Tag))

            If cmdStandardWordingEdit(Index).Text = "View" Then
                m_lReturn = EditStandardWording("View", sDocumentTemplateID)
            End If

            If cmdStandardWordingEdit(Index).Text = "Edit" Then

                If lvwStandardWording(Index).FocusedItem.SubItems.Item(1).Text = "Yes" Then
                    m_lReturn = EditStandardWording("EditOriginal", sDocumentTemplateID,
                                                    r_sEditCode:=sEditCode,
                                                    r_sEditDescription:=sEditDescription)
                    If sDocumentTemplateID <> lvwStandardWording(Index).FocusedItem.Tag Then
                        lvwStandardWording(Index).FocusedItem.Tag = sDocumentTemplateID
                        If sEditCode <> "" Then
                            lvwStandardWording(Index).FocusedItem.Text = sEditCode
                            If Trim(lvwStandardWording(Index).FocusedItem.SubItems.Item(1).Text) <> Trim(sEditDescription) Then
                                lvwStandardWording(Index).FocusedItem.SubItems.Item(1).Text = sEditDescription
                            Else
                                lvwStandardWording(Index).FocusedItem.SubItems.Item(1).Text =
                                    (lvwStandardWording(Index).FocusedItem.SubItems.Item(1).Text).Substring(0,
                                    Math.Min((lvwStandardWording(Index).FocusedItem.SubItems.Item(1).Text).Length, 240)) _
                                    & "_COPY_" & sDocumentTemplateID
                            End If
                            lvwStandardWording(Index).FocusedItem.SubItems.Item(2).Text = "Yes"
                        End If
                    Else
                        If sEditDescription <> "" Then
                            If Trim(lvwStandardWording(Index).FocusedItem.SubItems.Item(1).Text) <> Trim(sEditDescription) Then
                                lvwStandardWording(Index).FocusedItem.SubItems.Item(1).Text = sEditDescription
                            End If
                        End If
                    End If
                    'update screenvalues array
                    ' include all of the standard wording data items but only set the new values for the list view
                    aoArray = m_vScreenValues(cmdStandardWordingAdd(Index).Tag)

                    nLine = lvwStandardWording(Index).FocusedItem.Index

                    aoArray(0, nLine - 1) = lvwStandardWording(Index).FocusedItem.Tag
                    aoArray(1, nLine - 1) = lvwStandardWording(Index).FocusedItem
                    aoArray(2, nLine - 1) = lvwStandardWording(Index).FocusedItem.SubItems.Item(1)
                    aoArray(3, nLine - 1) = lvwStandardWording(Index).FocusedItem.SubItems.Item(2)

                    ReDim oDataTypes(3)
                    oDataTypes(0) = SharedFiles.iGISSharedConstants.GISDataTypeText
                    oDataTypes(1) = SharedFiles.iGISSharedConstants.GISDataTypeText
                    oDataTypes(2) = SharedFiles.iGISSharedConstants.GISDataTypeText
                    oDataTypes(3) = SharedFiles.iGISSharedConstants.GISDataTypeText

                    SetScreenValues(cmdStandardWordingAdd(Index).Tag,
                                        aoArray,
                                        lvwStandardWording(Index).Tag,
                                        m_vFrameArray(ACFTabNumber, lvwStandardWording(Index).Parent.TabIndex) + 1,
                                        v_vDataTypes:=oDataTypes)

                Else
                    m_lReturn = EditStandardWording("EditNewCopy", sDocumentTemplateID,
                                                    r_sEditCode:=sEditCode,
                                                    r_sEditDescription:=sEditDescription)
                    'check if document is edit and replaced by copy
                    If sDocumentTemplateID <> lvwStandardWording(Index).FocusedItem.Tag Then
                        lvwStandardWording(Index).FocusedItem.Tag = sDocumentTemplateID

                        If sEditCode <> "" Then
                            lvwStandardWording(Index).FocusedItem.Text = sEditCode
                            If Trim(lvwStandardWording(Index).FocusedItem.SubItems.Item(1).Text) <> Trim(sEditDescription) Then
                                lvwStandardWording(Index).FocusedItem.SubItems.Item(1).Text = sEditDescription
                            Else
                                lvwStandardWording(Index).FocusedItem.SubItems.Item(1).Text =
                                            (lvwStandardWording(Index).FocusedItem.SubItems.Item(1).Text).Substring(0,
                                                Math.Min((lvwStandardWording(Index).FocusedItem.SubItems.Item(1).Text).Length, 240)) _
                                            & "_COPY_" & sDocumentTemplateID
                            End If
                            lvwStandardWording(Index).FocusedItem.SubItems.Item(2).Text = "Yes"
                        End If

                        'update screenvalues array
                        ' include all of the standard wording data items but only set the new values for the list view
                        aoArray = m_vScreenValues(CInt(Convert.ToString(cmdStandardWordingAdd(Index).Tag)))

                        nLine = lvwStandardWording(Index).FocusedItem.Index + 1

                        aoArray(0, nLine - 1) = (Convert.ToString(lvwStandardWording(Index).FocusedItem.Tag))
                        aoArray(1, nLine - 1) = lvwStandardWording(Index).FocusedItem.SubItems.Item(0).Text
                        aoArray(2, nLine - 1) = lvwStandardWording(Index).FocusedItem.SubItems.Item(1).Text


                        ReDim oDataTypes(3)
                        oDataTypes(0) = iGISSharedConstants.GISDataTypeText
                        oDataTypes(1) = iGISSharedConstants.GISDataTypeText
                        oDataTypes(2) = iGISSharedConstants.GISDataTypeText
                        oDataTypes(3) = iGISSharedConstants.GISDataTypeText

                        SetScreenValues(CInt(Convert.ToString(cmdStandardWordingAdd(Index).Tag)),
                                        aoArray, Convert.ToString(lvwStandardWording(Index).Tag),
                                        CDbl(m_vFrameArray(ACFTabNumber,
                                        GetIndex(lvwStandardWording(Index).Parent, fraFrame))) + 1, ,
                                      (oDataTypes))

                    End If
                End If
            End If


            If m_bFireDLStartEvents Then
                ' use the old DL calls
                RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_Start,
                                v_vObjIdx:=lvwStandardWording(Index).Tag,
                                v_vControl:=cmdStandardWordingEdit(Index))

            End If

            If lvwStandardWording(Index).Items.Count < 1 Then
                cmdStandardWordingEdit(Index).Enabled = False
                cmdStandardWordingDelete(Index).Enabled = False
            End If
        Catch ex As Exception
            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError,
                               "cmdStandardWordingEdit_Click Failed", ACApp, ACClass,
                               "cmdStandardWordingEdit_Click",
                               Information.Err().Number, Information.Err().Description)
        End Try
#If (DebugOption) Then
        If nDebugDepthCounter > 0 Then AddToDebug(nDebugDepthCounter * -1) ' decrement the counter
#End If

    End Sub
    Private Sub lvwListView_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _lvwListView_0.DoubleClick
        Dim Index As Integer = Array.IndexOf(lvwListView, eventSender)
        cmdListViewEdit_Click(cmdListViewEdit(Index), New EventArgs())
    End Sub

    Private Sub lvwListView_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _lvwListView_0.Enter
        Dim Index As Integer = Array.IndexOf(lvwListView, eventSender)

        Dim lDebugDepthCounter As Integer

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : lvwListView_GotFocus - " & " (" & Index & ")")
#End If

        ' RAW 21/05/2004 : Performance Changes : replaced CallScriptOnFocus with RunDynamicLogic and an OnFocus ProcedureType and added v_vControl param
        'RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_List, v_vObjIdx:=lvwListView(Index).Tag, v_vPropIdx:="", v_vControl:=lvwListView(Index))
        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

    End Sub

    Private Sub cmdPartyCommand_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cmdPartyCommand_0.Click
        Dim Index As Integer = Array.IndexOf(cmdPartyCommand, eventSender)

        Dim lDebugDepthCounter As Integer

        Dim oFindParty As Object
        Dim lDataModelOffset, lControlOffset As Integer
        Dim vArray(,) As Object
        Dim vKeyArray(,) As Object
        Dim vbResponse As DialogResult
        Dim sMsgClassDesc As String = ""
        Dim bGoFind As Boolean
        Dim iTask As gPMConstants.PMEComponentAction
        Dim bEnableNewParty As Boolean

        '***************
        ' MEvans : 09-12-2003 : CQ3136
        Dim lCurrentPartyId As Integer
        '***************

        Try

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : cmdPartyCommand_Click - " & " (" & Index & ")")
#End If

            lDataModelOffset = CInt(CDbl(Convert.ToString(pnlPartyPanel(Index).Tag)) Mod 10000)  'lControlCount
            lControlOffset = CInt(Convert.ToString(cmdPartyCommand(Index).Tag))  'lTag
            vArray = m_vScreenValues(lControlOffset)

            bGoFind = True

            '***********
            ' MEvans : 09-12-2003 : CQ3136

            ' RAW110804 removed create instance of party link maintenance object

            ' save the current selected parties details
            If CStr(vArray(0, 0)) <> "" Then
                lCurrentPartyId = CInt(vArray(0, 0))
            Else
                lCurrentPartyId = 0
            End If
            '***********

            ' Get an instance of the find party interface object via
            ' the public object manager.
            ' IR 26 Jun 2003 - Create appropriate FindParty object : 205
            If CStr(vArray(2, 0)).Trim() = "OTTHIRD" Then
                bGoFind = False
                sMsgClassDesc = "third party"
                If pnlPartyPanel(Index).Text = "" Then
                    ReDim vKeyArray(1, 1)
                Else
                    ReDim vKeyArray(1, 2)
                    vKeyArray(0, 2) = "party_cnt"
                    vKeyArray(1, 2) = vArray(0, 0)
                End If
                vKeyArray(0, 0) = "work_claim_id"
                vKeyArray(1, 0) = m_lClaimID
                vKeyArray(0, 1) = "party_other"
                vKeyArray(1, 1) = "OTTHIRD"
                Dim temp_oFindParty As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oFindParty, "iPMBPartyOT.Interface_Renamed", PMGetLocalInterface)
                oFindParty = temp_oFindParty

                m_lReturn = ReflectionHelper.Invoke(oFindParty, "SetKeys", New Object() {vKeyArray})
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    oFindParty = Nothing
                    Exit Sub
                End If
                If pnlPartyPanel(Index).Text <> "" Then

                    bGoFind = (ReflectionHelper.GetMember(oFindParty, "SystemTypeCode") <> "CLAIMPARTY")
                End If

            End If

            If bGoFind Then
                oFindParty = Nothing
                sMsgClassDesc = "find party"
                ReDim vKeyArray(1, 0)
                vKeyArray(0, 0) = "special_party"

                If CStr(vArray(2, 0)).Trim() = "OTHERPARTY" Then
                    vKeyArray(1, 0) = "OT"
                    bEnableNewParty = True
                Else
                    vKeyArray(1, 0) = CStr(vArray(2, 0)).Trim()
                    bEnableNewParty = False
                End If

                Dim temp_oFindParty2 As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oFindParty2, "iPMBFindParty.Interface_Renamed", PMGetLocalInterface)
                oFindParty = temp_oFindParty2


                ReflectionHelper.SetMember(oFindParty, "EnableNewParty", bEnableNewParty)

            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to get " & sMsgClassDesc & " component", ACApp, ACClass, "cmdPartyCommand_Click", Information.Err().Number, Information.Err().Description)
                Exit Sub
            End If

            'If SubScreen Then
            '    'ReDim Preserve vKeyArray(1, (vKeyArray).GetUpperBound(1) + 1)
            '    ' vKeyArray(PMKeyName, UBound(vKeyArray, 2)) = PMKeyNameKeepWindowOnTop
            '    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, vKeyArray.GetUpperBound(1)) = 1
            'End If

            m_lReturn = ReflectionHelper.Invoke(oFindParty, "SetKeys", New Object() {vKeyArray})
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty = Nothing
                Exit Sub
            End If

            vKeyArray = Nothing
            If CStr(vArray(2, 0)).Trim() = "OTTHIRD" Then
                If pnlPartyPanel(Index).Text = "" Then
                    oFindParty.SetProcessModes(gPMConstants.PMEComponentAction.PMAdd)
                Else
                    oFindParty.SetProcessModes(gPMConstants.PMEComponentAction.PMEdit)
                End If
            Else
                ReflectionHelper.SetMember(oFindParty, "NotEditable", 1)
            End If

            'KeepWindowOnTop(False)
            m_lReturn = oFindParty.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If
            'KeepWindowOnTop(True)


            If ReflectionHelper.GetMember(oFindParty, "Status") = gPMConstants.PMEReturnCode.PMCancel Then
                If CStr(vArray(2, 0)).Trim() = "OTTHIRD" And Not bGoFind Then

                    If ReflectionHelper.GetMember(oFindParty, "SystemTypeCode") = "CLAIMPARTY" Then vbResponse = System.Windows.Forms.DialogResult.No
                End If
                If (CDbl(vArray(0, 0)) <> 0) And vbResponse <> System.Windows.Forms.DialogResult.No Then
                    vbResponse = MessageBox.Show("Do you wish to remove this " & cmdPartyCommand(Index).Text, m_sScreenDesc, MessageBoxButtons.YesNo)
                End If

                If vbResponse <> System.Windows.Forms.DialogResult.Yes Then

                    Exit Sub

                Else

                    '***********
                    ' MEvans : 09-12-2003 : CQ3136
                    ' if there was already a selected party
                    ' RAW110804
                    If lCurrentPartyId <> 0 Then
                        ' flag it for deletion from the claim party link table
                        iTask = gPMConstants.PMEComponentAction.PMDelete
                    End If
                    '***********

                    vArray(0, 0) = 0
                    vArray(1, 0) = ""

                End If

            Else

                '***********
                ' MEvans : 09-12-2003 : CQ3136
                ' if this isnt a third party then maintain the claim party links.
                ' NB. "Third Party" claim party links are handled differently and are
                ' maintained in the iPMBPartyOT.Interface component.
                If CStr(vArray(2, 0)).Trim() <> "OTTHIRD" Then

                    ' check if a new party was selected
                    If lCurrentPartyId <> 0 Then
                        ' is it the same as the previously selected party

                        If lCurrentPartyId = ReflectionHelper.GetMember(oFindParty, "PartyCnt") Then
                            ' no change made
                            Exit Sub
                        Else

                            ' flag it to be changed in the claim party link table
                            iTask = gPMConstants.PMEComponentAction.PMEdit
                        End If

                    Else
                        ' flag it to be added to the claim party link table
                        iTask = gPMConstants.PMEComponentAction.PMAdd

                    End If
                End If
                '***********


                vArray(0, 0) = ReflectionHelper.GetMember(oFindParty, "PartyCnt")
                If CStr(vArray(2, 0)).Trim() = "OTTHIRD" Then

                    vArray(1, 0) = ReflectionHelper.GetMember(oFindParty, "ShortName")
                Else

                    vArray(1, 0) = ReflectionHelper.GetMember(oFindParty, "LongName")
                End If

            End If


            ' RAW110804 added
            If iTask <> gPMConstants.PMEComponentAction.PMView Then
                ' save to the database

                m_lReturn = SaveLinkToParty(iTask, m_lClaimID, lCurrentPartyId, ReflectionHelper.GetMember(oFindParty, "PartyCnt"))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If
            End If

            SetScreenValues(lControlOffset, vArray, lDataModelOffset, CDbl(m_vFrameArray(ACFTabNumber, Conversion.Val(pnlPartyPanel(Index).Parent.Tag))) + 1)

            pnlPartyPanel(Index).Text = CStr(vArray(1, 0))

            vArray = Nothing

            ' RAW 22/06/2004 : Performance Changes(#2) : removed call to RunDynamicLogic when m_bFireDLStartEvents = FALSE because onChange may be raised from SetScreenValues
            If m_bFireDLStartEvents Then
                ' RVH - CQ2213 13/8/03 : Start
                '                        Fire dynamic logic when item changed
                ' RAW 21/05/2004 : Performance Changes : added params
                ' RAW 22/06/2004 : Performance Changes(#2) : replaced onChange with Start
                RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_Start, v_vObjIdx:=cmdPartyCommand(Index).Tag, v_vControl:=cmdPartyCommand(Index))
                ' RVH - CQ2213 13/8/03 : End
            End If


        Catch ex As Exception

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "cmdPartyCommand_ClickFailed", ACApp, ACClass, "cmdPartyCommand_Click", Information.Err().Number, Information.Err().Description, excep:=ex)


        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

            If Not (oFindParty Is Nothing) Then

                m_lReturn = ReflectionHelper.GetMember(oFindParty, "Dispose")

            End If
            oFindParty = Nothing

        End Try
    End Sub

    Private Sub cmdPartyCommand_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cmdPartyCommand_0.Enter
        Dim Index As Integer = Array.IndexOf(cmdPartyCommand, eventSender)

        Dim lDebugDepthCounter, lTag As Integer

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : cmdPartyCommand_GotFocus - " & " (" & Index & ")")
#End If
        lTag = GetIndex(cmdPartyCommand(Index).Parent, fraFrame)
        SelectTab(TabStrip1, CInt(m_vFrameArray(ACFTabNumber, lTag)))
        'TabStrip1_Click(Nothing, Nothing)
        ' RAW 21/05/2004 : Performance Changes : replaced CallScriptOnFocus with RunDynamicLogic and an OnFocus ProcedureType and added v_vControl param
        'RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_FurtherDetails, v_vObjIdx:=cmdPartyCommand(Index).Tag, v_vControl:=cmdPartyCommand(Index))

        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

    End Sub

    Private Sub cmdPolicyCommand_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cmdPolicyCommand_0.Click
        Dim Index As Integer = Array.IndexOf(cmdPolicyCommand, eventSender)
        Dim lDebugDepthCounter As Integer


        Dim oPolicyByProduct As iPMUPolicyByProduct.Interface_Renamed
        Dim lDataModelOffset, lControlOffset As Integer
        Dim vArray(,) As Object
        Dim vbResponse As DialogResult

        Try

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : cmdPolicyCommand_Click - " & " (" & Index & ")")
#End If

            ' Get an instance of the find Policy interface object via
            ' the public object manager.
            Dim temp_oPolicyByProduct As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPolicyByProduct, "iPMUPolicyByProduct.Interface_Renamed", PMGetLocalInterface)
            oPolicyByProduct = temp_oPolicyByProduct

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to get find policy component", ACApp, ACClass, "cmdPolicyCommand_Click", Information.Err().Number, Information.Err().Description)

                Exit Sub

            End If

            lDataModelOffset = CInt(CDbl(Convert.ToString(pnlPolicyPanel(Index).Tag)) Mod 1000)  'lControlCount
            lControlOffset = CInt(Convert.ToString(cmdPolicyCommand(Index).Tag))  'lTag
            vArray = m_vScreenValues(lControlOffset)

            oPolicyByProduct.PartyCnt = m_lPartyCnt
            oPolicyByProduct.Shortname = m_sShortName
            oPolicyByProduct.ProductId = m_vDataDictionary(GISDMTypeRisk)(ACPSpecialsTypeReference, lDataModelOffset)
            oPolicyByProduct.SourceID = g_iSourceID
            If SubScreen Then
                'm_lReturn = ReflectionHelper.Invoke(oPolicyByProduct, "SetKeys", New Object() {m_vWindowKeys})
                m_lReturn = oPolicyByProduct.SetKeys(m_vWindowKeys)
            End If

            'm_lReturn = ReflectionHelper.Invoke(oPolicyByProduct, "Start", New Object() {})
            m_lReturn = oPolicyByProduct.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If ReflectionHelper.GetMember(oPolicyByProduct, "Status") = gPMConstants.PMEReturnCode.PMCancel Then
            If oPolicyByProduct.Status = gPMConstants.PMEReturnCode.PMCancel Then
                If CDbl(vArray(0, 0)) <> 0 Then
                    vbResponse = MessageBox.Show("Do you wish to remove this " & cmdPolicyCommand(Index).Text, m_sScreenDesc, MessageBoxButtons.YesNo)

                    If vbResponse = System.Windows.Forms.DialogResult.Yes Then
                        vArray(0, 0) = 0
                        vArray(1, 0) = ""

                        pnlPolicyPanel(Index).Text = ""

                        SetScreenValues(lControlOffset, vArray, lDataModelOffset, CDbl(m_vFrameArray(ACFTabNumber, GetIndex(pnlPolicyPanel(Index).Parent, fraFrame))) + 1)

                        vArray = Nothing

                        oPolicyByProduct.Dispose()
                        oPolicyByProduct = Nothing

                        ' RAW 22/06/2004 : Performance Changes(#2) : removed call to RunDynamicLogic when m_bFireDLStartEvents = FALSE because onChange may be raised from SetScreenValues
                        If m_bFireDLStartEvents Then

                            ' RAW 03/09/2003 : CQ2213 : added
                            'run dynamic logic incase details have changed
                            ' RAW 21/05/2004 : Performance Changes : added params
                            ' RAW 22/06/2004 : Performance Changes(#2) : replaced onChange with Start
                            RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_Start, v_vObjIdx:=cmdPolicyCommand(Index).Tag, v_vControl:=cmdPolicyCommand(Index))
                        End If
                    End If

                End If

                Exit Sub
            End If

            'vArray(0, 0) = ReflectionHelper.GetMember(oPolicyByProduct, "InsuranceFileCnt")
            'pnlPolicyPanel(Index).Text = ReflectionHelper.GetMember(oPolicyByProduct, "InsuranceRef")

            vArray(0, 0) = oPolicyByProduct.InsuranceFileCnt
            pnlPolicyPanel(Index).Text = oPolicyByProduct.InsuranceRef

            vArray(1, 0) = pnlPolicyPanel(Index).Text

            SetScreenValues(lControlOffset, vArray, lDataModelOffset, CDbl(m_vFrameArray(ACFTabNumber, GetIndex(pnlPolicyPanel(Index).Parent, fraFrame))) + 1)

            vArray = Nothing

            oPolicyByProduct.Dispose()

            oPolicyByProduct = Nothing

            ' RAW 22/06/2004 : Performance Changes(#2) : removed call to RunDynamicLogic when m_bFireDLStartEvents = FALSE because onChange may be raised from SetScreenValues
            If m_bFireDLStartEvents Then
                ' RAW 03/09/2003 : CQ2213 : added
                'run dynamic logic incase details have changed
                ' RAW 21/05/2004 : Performance Changes : added params
                ' RAW 22/06/2004 : Performance Changes(#2) : replaced onChange with Start
                RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_Start, v_vObjIdx:=cmdPolicyCommand(Index).Tag, v_vControl:=cmdPolicyCommand(Index))

            End If


        Catch ex As Exception

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "cmdPolicyCommand_ClickFailed", ACApp, ACClass, "cmdPolicyCommand_Click", Information.Err().Number, Information.Err().Description, excep:=ex)



        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

        End Try
    End Sub

    Private Sub cmdPolicyCommand_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cmdPolicyCommand_0.Enter
        Dim Index As Integer = Array.IndexOf(cmdPolicyCommand, eventSender)

        Dim lDebugDepthCounter, lTag As Integer

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : cmdPolicyCommand_GotFocus - " & " (" & Index & ")")
#End If

        lTag = GetIndex(cmdPolicyCommand(Index).Parent, fraFrame)

        SelectTab(TabStrip1, CInt(m_vFrameArray(ACFTabNumber, lTag)))
        'TabStrip1_Click(Nothing, Nothing)
        ' RAW 21/05/2004 : Performance Changes : replaced CallScriptOnFocus with RunDynamicLogic and an OnFocus ProcedureType and added v_vControl param
        'RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_FurtherDetails, v_vObjIdx:=cmdPolicyCommand(Index).Tag, v_vControl:=cmdPolicyCommand(Index))

        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

    End Sub

    Private Sub cmdStandardWordingAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cmdStandardWordingAdd_0.Click
        Dim Index As Integer = Array.IndexOf(cmdStandardWordingAdd, eventSender)

        Dim lDebugDepthCounter As Integer
        Dim oListItem As ListViewItem
        Dim vDataTypes As Object
        Dim vArray(,) As Object
        Const kMethodName As String = "cmdStandardWordingAdd_Click"
        Try

            ' Get an instance of the policy wording interface object via
            ' the public object manager.
            Dim temp_m_oPolicyWording As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oPolicyWording, "iSIRPickDocTemplate.Interface_Renamed", PMGetLocalInterface)
            m_oPolicyWording = temp_m_oPolicyWording

            'Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                RaiseError(kMethodName, "iSIRPickDocTemplate.Interface Failed to Create the Instance", gPMConstants.PMELogLevel.PMLogError)
                Exit Sub
            End If



            'm_lReturn = ReflectionHelper.Invoke(m_oPolicyWording, "SetProcessModes", New Object() {gPMConstants.PMEComponentAction.PMAdd})
            m_lReturn = m_oPolicyWording.SetProcessModes(gPMConstants.PMEComponentAction.PMAdd)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "m_oPolicyWording.SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
                Exit Sub
            End If

            m_oPolicyWording.SourceId = g_iSourceID
            m_oPolicyWording.ProductId = ACProductTypeNotExist
            m_oPolicyWording.RiskId = m_lRiskTypeId
            m_oPolicyWording.ClauseId = MainModule.ENClauseType.RiskType

            m_oPolicyWording.PropertyName = m_sPropertyName
            m_oPolicyWording.ColumnName = m_sColumnName
            m_oPolicyWording.Searchable = True

            vArray = m_vScreenValues(cmdStandardWordingAdd(Index).Tag)

            m_oPolicyWording.DefaultClauses = vArray

            'If SubScreen Then
            '    ReDim m_vWindowKeys(1, 0)
            '    m_vWindowKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMKeyNameKeepWindowOnTop
            '    m_vWindowKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = CStr(1)
            '    m_lReturn = m_oPolicyWording.SetKeys(m_vWindowKeys) '' ReflectionHelper.Invoke(m_oPolicyWording, "SetKeys", New Object() {m_vWindowKeys})
            'End If

            m_lReturn = m_oPolicyWording.Start()
            'm_lReturn = ReflectionHelper.Invoke(m_oPolicyWording, "Start", New Object() {})

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "m_oPolicyWording.Start() failed to load the frmSelectClause Page", gPMConstants.PMELogLevel.PMLogError)
                Exit Sub
            End If

            'If not cancelled, add to grid

            If m_oPolicyWording.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'm_vStandardWording = VB6.CopyArray(ReflectionHelper.GetMember(m_oPolicyWording, "DocumentTemplate"))
            'm_vStandardWording = VB6.CopyArray(m_oPolicyWording.DocumentTemplate)
            m_vStandardWording = m_oPolicyWording.DocumentTemplate

            vArray = Array.CreateInstance(GetType(Object), New Integer() {ACSelectClauseArrayIndex + 1, ACSelectClauseArrayDefaultIndex + 1}, New Integer() {0, 0})
            Application.DoEvents()
            lvwStandardWording(Index).Items.Clear()

            If Information.IsArray(m_vStandardWording) And Not (m_vStandardWording Is Nothing) Then

                For lCount As Integer = m_vStandardWording.GetLowerBound(ACSelectClauseRowIndex - 1) To m_vStandardWording.GetUpperBound(ACSelectClauseRowIndex - 1)

                    oListItem = lvwStandardWording(Index).Items.Add(CStr(m_vStandardWording(ACSelectClauseCode, lCount)))
                    ListViewHelper.GetListViewSubItem(oListItem, ACSelectClauseCode).Text = CStr(m_vStandardWording(ACSelectClauseDescription, lCount))
                    ListViewHelper.GetListViewSubItem(oListItem, ACSelectClauseDescription).Text = ""
                    oListItem.Tag = CStr(m_vStandardWording(ACSelectClauseId, lCount))
                    vArray = ArraysHelper.RedimPreserve(Of Object(,))(vArray, New Integer() {ACSelectClauseArrayIndex + 1, lCount + 1}, New Integer() {0, 0})
                    lCount = vArray.GetUpperBound(ACSelectClauseDescription - 1)

                    vArray(ACSelectClauseId, lCount) = m_vStandardWording(ACSelectClauseId, lCount)
                    'NIIT Comments
                    vArray(ACSelectClauseCode, lCount) = m_vStandardWording(ACSelectClauseCode, lCount)
                    'NIIT Comments
                    vArray(ACSelectClauseDescription, lCount) = m_vStandardWording(ACSelectClauseDescription, lCount)
                    vArray(ACSelectClauseArrayIndex, lCount) = m_vStandardWording(ACSelectClauseArrayIndex, lCount)

                    ReDim vDataTypes(ACSelectClauseArrayIndex)
                    vDataTypes(ACSelectClauseId) = iGISSharedConstants.GISDataTypeText
                    vDataTypes(ACSelectClauseCode) = iGISSharedConstants.GISDataTypeText
                    vDataTypes(ACSelectClauseDescription) = iGISSharedConstants.GISDataTypeText
                    vDataTypes(ACSelectClauseArrayIndex) = iGISSharedConstants.GISDataTypeText

                    SetScreenValues(CInt(Convert.ToString(cmdStandardWordingAdd(Index).Tag)), vArray, Convert.ToString(lvwStandardWording(Index).Tag), CDbl(m_vFrameArray(ACFTabNumber, GetIndex(lvwStandardWording(Index).Parent, fraFrame))) + 1, , (vDataTypes))

                    ' RAW 03/09/2004 : end

                    ' RAW 21/05/2004 : Performance Changes : added params
                    ' RAW 22/06/2004 : Performance Changes(#2) : replaced onChange with Start when m_bFireDLStartEvents = true
                    If m_bFireDLStartEvents Then
                        RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_Start, v_vObjIdx:=lvwStandardWording(Index).Tag, v_vControl:=cmdStandardWordingAdd(Index))
                    End If
                Next lCount
            End If

            m_oPolicyWording.Dispose()
            m_oPolicyWording = Nothing


        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(ACClass, kMethodName, False, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
            ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If
        End Try
    End Sub


    Private Sub cmdStandardWordingAdd_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cmdStandardWordingAdd_0.Enter
        Dim Index As Integer = Array.IndexOf(cmdStandardWordingAdd, eventSender)

        Dim lDebugDepthCounter As Integer

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : cmdStandardWordingAdd_GotFocus ")
#End If

        ' RAW 21/05/2004 : Performance Changes : replaced CallScriptOnFocus with RunDynamicLogic and an OnFocus ProcedureType and added v_vControl param
        'RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_Add, v_vObjIdx:=lvwStandardWording(Index).Tag, v_vControl:=cmdStandardWordingAdd(Index))
        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

    End Sub

    Private Sub cmdStandardWordingDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cmdStandardWordingDelete_0.Click
        Dim Index As Integer = Array.IndexOf(cmdStandardWordingDelete, eventSender)

        Dim lDebugDepthCounter As Integer
        Dim iLine As Integer
        Dim vDataTypes As Object
        Dim vArray(,) As Object
        Dim oListItem As ListViewItem

        Try

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : cmdStandardWordingDelete_Click ")
#End If

            'Set row to be deleted - if a valid one selected
            If lvwStandardWording(Index).Items.Count < 1 Then
                Exit Sub
            End If

            oListItem = lvwStandardWording(Index).FocusedItem

            If oListItem Is Nothing Then
                Exit Sub
            End If

            iLine = lvwStandardWording(Index).FocusedItem.Index + 1

            If iLine = -1 Then
                Exit Sub
            End If

            If oListItem.SubItems.Item(1).Text = "Yes" Then
                If MessageBox.Show("WARNING  Deleting this Clause will remove the edited copy the system. " &
                                   "This deletion is not reversible." & Strings.Chr(13) & Strings.Chr(10) &
                                   "Are you sure you want to continue?", "Delete Edited Clause", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = System.Windows.Forms.DialogResult.Yes Then

                    lvwStandardWording(Index).Items.RemoveAt(iLine - 1)
                Else
                    Exit Sub
                End If
            Else
                lvwStandardWording(Index).Items.RemoveAt(iLine - 1)
            End If


            ' RAW 03/09/2004 : added
            ' update screen values array and fire an onChange DL event
            ReDim vDataTypes(2)
            vDataTypes(0) = iGISSharedConstants.GISDataTypeText
            vDataTypes(2) = iGISSharedConstants.GISDataTypeText

            ' include all of the sum insured data items but only set the new values for the list view
            vArray = m_vScreenValues(CInt(Convert.ToString(cmdStandardWordingAdd(Index).Tag)))

            ' remove this row from array
            For j As Integer = iLine - 1 To vArray.GetUpperBound(1) - 1
                vArray(0, j) = vArray(0, j + 1)
                vArray(1, j) = vArray(1, j + 1)
                vArray(2, j) = vArray(2, j + 1)
                vArray(3, j) = vArray(3, j + 1)
            Next j

            If vArray.GetUpperBound(1) = 0 Then
                vArray = Nothing
            Else
                vArray = ArraysHelper.RedimPreserve(Of Object(,))(vArray, New Integer() {vArray.GetUpperBound(0) - vArray.GetLowerBound(0) + 1, vArray.GetUpperBound(1) - 1 - vArray.GetLowerBound(1) + 1}, New Integer() {vArray.GetLowerBound(0), vArray.GetLowerBound(1)})
            End If


            ' This will fire a DL onChange event if details have changed
            ReDim vDataTypes(3)
            vDataTypes(0) = iGISSharedConstants.GISDataTypeText
            vDataTypes(2) = iGISSharedConstants.GISDataTypeText
            vDataTypes(3) = iGISSharedConstants.GISDataTypeText

            SetScreenValues(CInt(Convert.ToString(cmdStandardWordingAdd(Index).Tag)), vArray, Convert.ToString(lvwStandardWording(Index).Tag), CDbl(m_vFrameArray(ACFTabNumber, GetIndex(lvwStandardWording(Index).Parent, fraFrame))) + 1, , (vDataTypes))


            ' RAW 21/05/2004 : Performance Changes : added params
            ' RAW 22/06/2004 : Performance Changes(#2) : replaced onChange with Start when m_bFireDLStartEvents = true
            If m_bFireDLStartEvents Then
                RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_Start, v_vObjIdx:=lvwStandardWording(Index).Tag, v_vControl:=cmdStandardWordingDelete(Index))
            End If

            cmdStandardWordingDelete(Index).Focus()


        Catch ex As Exception

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "cmdStandardWordingDelete_Click Failed", ACApp, ACClass, "cmdStandardWordingDelete_Click", Information.Err().Number, Information.Err().Description, excep:=ex)

        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

        End Try
    End Sub

    Private Sub cmdStandardWordingDelete_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cmdStandardWordingDelete_0.Enter
        Dim Index As Integer = Array.IndexOf(cmdStandardWordingDelete, eventSender)

        Dim lDebugDepthCounter As Integer

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : cmdStandardWordingDelete_GotFocus ")
#End If

        ' RAW 21/05/2004 : Performance Changes : replaced CallScriptOnFocus with RunDynamicLogic and an OnFocus ProcedureType and added v_vControl param
        'RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_Delete, v_vObjIdx:=lvwStandardWording(Index).Tag, v_vControl:=cmdStandardWordingDelete(Index))
        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

    End Sub

    Private Sub cmdSumInsuredAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cmdSumInsuredAdd_0.Click
        Dim Index As Integer = Array.IndexOf(cmdSumInsuredAdd, eventSender)
        Dim lDebugDepthCounter As Integer
        Dim oListItem As ListViewItem
        Dim lTag As Integer

        Dim oSumInsured As iGISSumInsured.Interface_Renamed
        Dim cSumInsured, cTotalSumInsured As Decimal
        Dim dRate As Double
        Dim cPremium As Decimal
        Dim vDataTypes() As Object
        Dim vArray(,) As Object
        Dim i As Integer

        Try

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : cmdSumInsuredAdd_Click - " & " (" & Index & ")")
#End If

            ' Get an instance of the sum insured interface object via
            ' the public object manager.
            Dim temp_oSumInsured As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oSumInsured, "iGISSumInsured.Interface_Renamed", PMGetLocalInterface)
            oSumInsured = temp_oSumInsured

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to get sum insured object", ACApp, ACClass, "cmdSumInsuredAdd_Click", Information.Err().Number, Information.Err().Description)

                Exit Sub

            End If

            lTag = CInt(Convert.ToString(cmdSumInsuredAdd(Index).Tag))

            'm_lReturn = ReflectionHelper.Invoke(oSumInsured, "SetProcessModes", New Object() {gPMConstants.PMEComponentAction.PMAdd})
            m_lReturn = oSumInsured.SetProcessModes(gPMConstants.PMEComponentAction.PMAdd)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'ReflectionHelper.SetMember(oSumInsured, "IsValuation", m_vScreenDetails(ACDIsValuation, lTag))
            oSumInsured.IsValuation = m_vScreenDetails(ACDIsValuation, lTag)

            If SubScreen Then
                m_lReturn = oSumInsured.SetKeys(m_vWindowKeys)
                'm_lReturn = ReflectionHelper.Invoke(oSumInsured, "SetKeys", New Object() {m_vWindowKeys})
            End If

            'm_lReturn = ReflectionHelper.Invoke(oSumInsured, "Start", New Object() {})
            m_lReturn = oSumInsured.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oSumInsured.Dispose()
                oSumInsured = Nothing
                Exit Sub
            End If
            'If not cancelled, update grid
            If oSumInsured.Status = gPMConstants.PMEReturnCode.PMCancel Then
                oSumInsured.Dispose()
                oSumInsured = Nothing
                Exit Sub
            End If

            txtCurrency.Text = pnlTotalSumInsured(Index).Text
            cTotalSumInsured = (m_oFormFields.UnformatControl(txtCurrency))
            cSumInsured = oSumInsured.SumInsured
            cTotalSumInsured += cSumInsured
            m_lReturn = m_oFormFields.FormatControl(txtCurrency, cTotalSumInsured)
            pnlTotalSumInsured(Index).Text = txtCurrency.Text

            If txtRate(Index).Text <> "" Then
                dRate = (m_oFormFields.UnformatControlArray(txtRate(Index), GetIndex(txtRate(Index), txtRate)))
                cPremium = cTotalSumInsured * dRate / 100.0#
                m_lReturn = m_oFormFields.FormatControlArray(txtPremium(Index), cPremium, GetIndex(txtPremium(Index), txtPremium))
            End If

            oListItem = lvwSumInsured(Index).Items.Add(oSumInsured.Description)
            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = oSumInsured.Reference
            m_lReturn = m_oFormFields.FormatControl(txtCurrency, oSumInsured.SumInsured)
            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = txtCurrency.Text
            m_lReturn = m_oFormFields.FormatControl(txtDate, oSumInsured.DateAdded)
            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtDate.Text
            m_lReturn = m_oFormFields.FormatControl(txtDate, oSumInsured.DateDeleted)
            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = txtDate.Text
            ListViewHelper.GetListViewSubItem(oListItem, 5).Text = oSumInsured.IsValuationRequired
            m_lReturn = m_oFormFields.FormatControl(txtDate, oSumInsured.ValuationDate)
            ListViewHelper.GetListViewSubItem(oListItem, 6).Text = txtDate.Text
            oSumInsured.Dispose()
            oSumInsured = Nothing

            'RAW 17/06/2004 : Performance Changes(#2) : added
            ' update screen values array and fire an onChange DL event
            ReDim vDataTypes(8)
            vDataTypes(m_klSumInsuredArrayColNo_Description) = m_klSumInsuredDataType_Description
            vDataTypes(m_klSumInsuredArrayColNo_Reference) = m_klSumInsuredDataType_Reference
            vDataTypes(m_klSumInsuredArrayColNo_SumInsured) = m_klSumInsuredDataType_SumInsured
            vDataTypes(m_klSumInsuredArrayColNo_DateAdded) = m_klSumInsuredDataType_DateAdded
            vDataTypes(m_klSumInsuredArrayColNo_DateDeleted) = m_klSumInsuredDataType_DateDeleted
            vDataTypes(m_klSumInsuredArrayColNo_ValuationReq) = m_klSumInsuredDataType_ValuationReq
            vDataTypes(m_klSumInsuredArrayColNo_ValuationDate) = m_klSumInsuredDataType_ValuationDate
            vDataTypes(m_klSumInsuredArrayColNo_Rate) = m_klSumInsuredDataType_Rate
            vDataTypes(m_klSumInsuredArrayColNo_Premium) = m_klSumInsuredDataType_Premium

            ' include all of the sum insured data items but only set the new values for the list view
            If Information.IsArray(m_vScreenValues(CInt(Convert.ToString(cmdSumInsuredAdd(Index).Tag)))) Then
                vArray = m_vScreenValues(CInt(Convert.ToString(cmdSumInsuredAdd(Index).Tag)))
                vArray = ArraysHelper.RedimPreserve(Of Object(,))(vArray, New Integer() {9, vArray.GetUpperBound(1) + 2}, New Integer() {0, 0})  ' RAW 03/09/2004 : moved so that its not called when dimenisioning array the first time
            Else
                vArray = Array.CreateInstance(GetType(Object), New Integer() {9, 1}, New Integer() {0, 0})
            End If


            i = vArray.GetUpperBound(1)

            vArray(m_klSumInsuredArrayColNo_Description, i) = oListItem.Text
            vArray(m_klSumInsuredArrayColNo_Reference, i) = ListViewHelper.GetListViewSubItem(oListItem, m_klSumInsuredListColNo_Reference).Text
            txtCurrency.Text = ListViewHelper.GetListViewSubItem(oListItem, m_klSumInsuredListColNo_SumInsured).Text
            vArray(m_klSumInsuredArrayColNo_SumInsured, i) = (m_oFormFields.UnformatControl(txtCurrency))
            txtDate.Text = ListViewHelper.GetListViewSubItem(oListItem, m_klSumInsuredListColNo_DateAdded).Text
            '    If txtDate.Text = "" Then
            '        vArray(m_klSumInsuredArrayColNo_DateAdded, i) = Empty
            '    Else
            vArray(m_klSumInsuredArrayColNo_DateAdded, i) = (m_oFormFields.UnformatControl(txtDate))
            '    End If

            ' RVH 07/01/2005 - Looks like the "deleted date" was missed in 1.9 code...re-added for this 1.9->1.8.x
            '                  claimsbuilder port
            txtDate.Text = ListViewHelper.GetListViewSubItem(oListItem, m_klSumInsuredListColNo_DateDeleted).Text
            '    If txtDate.Text = "" Then
            '        vArray(m_klSumInsuredArrayColNo_DateDeleted, i) = Empty
            '    Else
            vArray(m_klSumInsuredArrayColNo_DateDeleted, i) = (m_oFormFields.UnformatControl(txtDate))
            '    End If

            If ListViewHelper.GetListViewSubItem(oListItem, m_klSumInsuredListColNo_ValuationReq).Text = "Yes" Then
                vArray(m_klSumInsuredArrayColNo_ValuationReq, i) = 1
            Else
                vArray(m_klSumInsuredArrayColNo_ValuationReq, i) = 0
            End If

            txtDate.Text = ListViewHelper.GetListViewSubItem(oListItem, m_klSumInsuredListColNo_ValuationDate).Text
            '    If txtDate.Text = "" Then
            '        vArray(m_klSumInsuredArrayColNo_ValuationDate, i) = Empty
            '    Else
            vArray(m_klSumInsuredArrayColNo_ValuationDate, i) = (m_oFormFields.UnformatControl(txtDate))
            '    End If

            ' RVH 07/01/2005 - Added to match 1.8.x code
            vArray(m_klSumInsuredArrayColNo_Rate, i) = (m_oFormFields.UnformatControlArray(txtRate(Index), GetIndex(txtRate(Index), txtRate)))
            vArray(m_klSumInsuredArrayColNo_Premium, i) = (m_oFormFields.UnformatControlArray(txtPremium(Index), GetIndex(txtPremium(Index), txtPremium)))

            ' This will fire a DL onChange event if details have changed
            SetScreenValues(CInt(Convert.ToString(cmdSumInsuredAdd(Index).Tag)), vArray, Convert.ToString(lvwSumInsured(Index).Tag), CDbl(m_vFrameArray(ACFTabNumber, GetIndex(lvwSumInsured(Index).Parent, fraFrame))) + 1, , vDataTypes)
            ' RAW 22/06/2004 : Performance Changes(#2) : end


            ' RAW 22/06/2004 : Performance Changes(#2) : removed call to RunDynamicLogic Start when m_bFireDLStartEvents = FALSE because onChange may be raised from SetScreenValues
            If m_bFireDLStartEvents Then
                RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_Start, v_vObjIdx:=lvwSumInsured(Index).Tag, v_vControl:=cmdSumInsuredAdd(Index))
            End If


        Catch ex As Exception

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "cmdSumInsuredAdd_Click Failed", ACApp, ACClass, "cmdSumInsuredAdd_Click", Information.Err().Number, Information.Err().Description, excep:=ex)



        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

        End Try
    End Sub

    Private Sub cmdSumInsuredAdd_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  ''Handles _cmdSumInsuredAdd_0.Enter
        Dim Index As Integer = Array.IndexOf(cmdSumInsuredAdd, eventSender)

        Dim lDebugDepthCounter As Integer

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : cmdSumInsuredAdd_GotFocus - " & " (" & Index & ")")
#End If

        ' RAW 21/05/2004 : Performance Changes : replaced CallScriptOnFocus with RunDynamicLogic and an OnFocus ProcedureType and added v_vControl param
        'RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_Add, v_vObjIdx:=lvwSumInsured(Index).Tag, v_vControl:=cmdSumInsuredAdd(Index))
        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

    End Sub

    Private Sub cmdSumInsuredDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cmdSumInsuredDelete_0.Click
        Dim Index As Integer = Array.IndexOf(cmdSumInsuredDelete, eventSender)
        Dim lDebugDepthCounter As Integer
        Dim oListItem As ListViewItem
        Dim lTag As Integer


        Dim oSumInsured As iGISSumInsured.Interface_Renamed
        Dim cSumInsured, cTotalSumInsured As Decimal
        Dim dRate As Double
        Dim cPremium As Decimal
        Dim vDataTypes() As Object
        Dim vArray(,) As Object
        Dim i As Integer

        Try

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : cmdSumInsuredDelete_Click - " & " (" & Index & ")")
#End If

            If lvwSumInsured(Index).Items.Count = 0 Then
                Exit Sub
            End If

            oListItem = lvwSumInsured(Index).FocusedItem

            If oListItem Is Nothing Then
                Exit Sub
            End If

            ' Get an instance of the sum insured interface object via
            ' the public object manager.
            Dim temp_oSumInsured As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oSumInsured, "iGISSumInsured.Interface_Renamed", PMGetLocalInterface)
            oSumInsured = temp_oSumInsured

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to get sum insured object", ACApp, ACClass, "cmdSumInsuredDelete_Click", Information.Err().Number, Information.Err().Description)
                Exit Sub
            End If

            lTag = CInt(Convert.ToString(cmdSumInsuredAdd(Index).Tag))
            m_lReturn = oSumInsured.SetProcessModes(gPMConstants.PMEComponentAction.PMDelete)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ReflectionHelper.SetMember(oSumInsured, "IsValuation", m_vScreenDetails(ACDIsValuation, lTag))
            ReflectionHelper.SetMember(oSumInsured, "Description", oListItem.Text)
            ReflectionHelper.SetMember(oSumInsured, "Reference", ListViewHelper.GetListViewSubItem(oListItem, 1).Text)

            txtCurrency.Text = ListViewHelper.GetListViewSubItem(oListItem, 2).Text
            cSumInsured = (m_oFormFields.UnformatControl(txtCurrency))
            ReflectionHelper.SetMember(oSumInsured, "SumInsured", cSumInsured)
            txtDate.Text = ListViewHelper.GetListViewSubItem(oListItem, 3).Text
            ReflectionHelper.SetMember(oSumInsured, "DateAdded", (m_oFormFields.UnformatControl(txtDate)))
            txtDate.Text = ListViewHelper.GetListViewSubItem(oListItem, 4).Text
            ReflectionHelper.SetMember(oSumInsured, "DateDeleted", (m_oFormFields.UnformatControl(txtDate)))

            If txtDate.Text <> "" Then
                cSumInsured = 0
            End If

            ReflectionHelper.SetMember(oSumInsured, "IsValuationRequired", ListViewHelper.GetListViewSubItem(oListItem, 5).Text)
            txtDate.Text = ListViewHelper.GetListViewSubItem(oListItem, 6).Text
            ReflectionHelper.SetMember(oSumInsured, "ValuationDate", (m_oFormFields.UnformatControl(txtDate)))

            If SubScreen Then
                m_lReturn = oSumInsured.SetKeys(m_vWindowKeys)
            End If

            m_lReturn = oSumInsured.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oSumInsured.Dispose()
                oSumInsured = Nothing
                Exit Sub
            End If

            'If not cancelled, update grid

            If ReflectionHelper.GetMember(oSumInsured, "Status") = gPMConstants.PMEReturnCode.PMCancel Then
                oSumInsured.Dispose()
                oSumInsured = Nothing
                Exit Sub
            End If

            txtCurrency.Text = pnlTotalSumInsured(Index).Text
            cTotalSumInsured = (m_oFormFields.UnformatControl(txtCurrency))
            cTotalSumInsured -= cSumInsured
            m_lReturn = m_oFormFields.FormatControl(txtCurrency, cTotalSumInsured)
            pnlTotalSumInsured(Index).Text = txtCurrency.Text

            If txtRate(Index).Text <> "" Then
                dRate = (m_oFormFields.UnformatControlArray(txtRate(Index), GetIndex(txtRate(Index), txtRate)))
                cPremium = cTotalSumInsured * dRate / 100.0#
                m_lReturn = m_oFormFields.FormatControlArray(txtPremium(Index), cPremium, GetIndex(txtPremium(Index), txtPremium))
            End If

            oListItem.Text = ReflectionHelper.GetMember(oSumInsured, "Description")
            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = ReflectionHelper.GetMember(oSumInsured, "Reference")

            m_lReturn = m_oFormFields.FormatControl(txtCurrency, ReflectionHelper.GetMember(oSumInsured, "SumInsured"))
            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = txtCurrency.Text

            m_lReturn = m_oFormFields.FormatControl(txtDate, ReflectionHelper.GetMember(oSumInsured, "DateAdded"))
            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtDate.Text

            m_lReturn = m_oFormFields.FormatControl(txtDate, ReflectionHelper.GetMember(oSumInsured, "DateDeleted"))
            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = txtDate.Text

            ListViewHelper.GetListViewSubItem(oListItem, 5).Text = ReflectionHelper.GetMember(oSumInsured, "IsValuationRequired")

            m_lReturn = m_oFormFields.FormatControl(txtDate, ReflectionHelper.GetMember(oSumInsured, "ValuationDate"))
            ListViewHelper.GetListViewSubItem(oListItem, 6).Text = txtDate.Text

            oSumInsured.Dispose()

            oSumInsured = Nothing

            '    Me.Refresh

            ' RAW 22/06/2004 : Performance Changes(#2) : added
            ReDim vDataTypes(8)
            vDataTypes(m_klSumInsuredArrayColNo_Description) = m_klSumInsuredDataType_Description
            vDataTypes(m_klSumInsuredArrayColNo_Reference) = m_klSumInsuredDataType_Reference
            vDataTypes(m_klSumInsuredArrayColNo_SumInsured) = m_klSumInsuredDataType_SumInsured
            vDataTypes(m_klSumInsuredArrayColNo_DateAdded) = m_klSumInsuredDataType_DateAdded
            vDataTypes(m_klSumInsuredArrayColNo_DateDeleted) = m_klSumInsuredDataType_DateDeleted
            vDataTypes(m_klSumInsuredArrayColNo_ValuationReq) = m_klSumInsuredDataType_ValuationReq
            vDataTypes(m_klSumInsuredArrayColNo_ValuationDate) = m_klSumInsuredDataType_ValuationDate
            vDataTypes(m_klSumInsuredArrayColNo_Rate) = m_klSumInsuredDataType_Rate
            vDataTypes(m_klSumInsuredArrayColNo_Premium) = m_klSumInsuredDataType_Premium

            ' include all of the sum insured data items but only set the new values for the list view
            vArray = m_vScreenValues(CInt(Convert.ToString(cmdSumInsuredAdd(Index).Tag)))

            ' RAW 03/09/2004 : subtract 1
            i = oListItem.Index + 1 - 1

            txtDate.Text = ListViewHelper.GetListViewSubItem(oListItem, m_klSumInsuredListColNo_DateDeleted).Text
            If txtDate.Text = "" Then
                vArray(m_klSumInsuredArrayColNo_DateDeleted, i) = Nothing
            Else
                vArray(m_klSumInsuredArrayColNo_DateDeleted, i) = (m_oFormFields.UnformatControl(txtDate))
            End If

            ' This will fire a DL onChange event if details have changed
            SetScreenValues(CInt(Convert.ToString(cmdSumInsuredAdd(Index).Tag)), vArray, Convert.ToString(lvwSumInsured(Index).Tag), CDbl(m_vFrameArray(ACFTabNumber, GetIndex(lvwSumInsured(Index).Parent, fraFrame))) + 1, , vDataTypes)
            ' RAW 22/06/2004 : Performance Changes(#2) : end


            ' RAW 22/06/2004 : Performance Changes(#2) : removed call to RunDynamicLogic Start when m_bFireDLStartEvents = FALSE because onChange may be raised from SetScreenValues
            If m_bFireDLStartEvents Then
                RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_Start, v_vObjIdx:=lvwSumInsured(Index).Tag, v_vControl:=cmdSumInsuredDelete(Index))
            End If



        Catch ex As Exception

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "cmdSumInsuredDelete_Click Failed", ACApp, ACClass, "cmdSumInsuredDelete_Click", Information.Err().Number, Information.Err().Description, excep:=ex)


        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

        End Try
    End Sub

    Private Sub cmdSumInsuredDelete_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cmdSumInsuredDelete_0.Enter
        Dim Index As Integer = Array.IndexOf(cmdSumInsuredDelete, eventSender)

        Dim lDebugDepthCounter As Integer

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : cmdSumInsuredDelete_GotFocus - " & " (" & Index & ")")
#End If

        ' RAW 21/05/2004 : Performance Changes : replaced CallScriptOnFocus with RunDynamicLogic and an OnFocus ProcedureType and added v_vControl param
        'RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_Delete, v_vObjIdx:=lvwSumInsured(Index).Tag, v_vControl:=cmdSumInsuredDelete(Index))
        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

    End Sub

    Private Sub cmdSumInsuredEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cmdSumInsuredEdit_0.Click
        Dim Index As Integer = Array.IndexOf(cmdSumInsuredEdit, eventSender)

        Dim lDebugDepthCounter As Integer
        Dim oListItem As ListViewItem
        Dim lTag As Integer


        Dim oSumInsured As iGISSumInsured.Interface_Renamed
        Dim cSumInsured, cTotalSumInsured As Decimal
        Dim dRate As Double
        Dim cPremium As Decimal
        Dim vDataTypes() As Object
        Dim vArray(,) As Object
        Dim i As Integer

        Try

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : Err_cmdSumInsuredEdit_Click - " & " (" & Index & ")")
#End If

            If lvwSumInsured(Index).Items.Count = 0 Then
                Exit Sub
            End If

            oListItem = lvwSumInsured(Index).FocusedItem

            If oListItem Is Nothing Then
                Exit Sub
            End If

            ' Get an instance of the sum insured interface object via
            ' the public object manager.
            Dim temp_oSumInsured As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oSumInsured, "iGISSumInsured.Interface_Renamed", PMGetLocalInterface)
            oSumInsured = temp_oSumInsured

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to get sum insured object", ACApp, ACClass, "cmdSumInsuredEdit_Click", Information.Err().Number, Information.Err().Description)
                Exit Sub
            End If

            lTag = CInt(Convert.ToString(cmdSumInsuredAdd(Index).Tag))
            m_lReturn = oSumInsured.SetProcessModes(gPMConstants.PMEComponentAction.PMEdit)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ReflectionHelper.SetMember(oSumInsured, "IsValuation", m_vScreenDetails(ACDIsValuation, lTag))
            ReflectionHelper.SetMember(oSumInsured, "Description", oListItem.Text)
            ReflectionHelper.SetMember(oSumInsured, "Reference", ListViewHelper.GetListViewSubItem(oListItem, 1).Text)
            txtCurrency.Text = ListViewHelper.GetListViewSubItem(oListItem, 2).Text
            cSumInsured = (m_oFormFields.UnformatControl(txtCurrency))
            ReflectionHelper.SetMember(oSumInsured, "SumInsured", cSumInsured)
            txtDate.Text = ListViewHelper.GetListViewSubItem(oListItem, 3).Text
            ReflectionHelper.SetMember(oSumInsured, "DateAdded", (m_oFormFields.UnformatControl(txtDate)))
            txtDate.Text = ListViewHelper.GetListViewSubItem(oListItem, 4).Text
            ReflectionHelper.SetMember(oSumInsured, "DateDeleted", (m_oFormFields.UnformatControl(txtDate)))

            If txtDate.Text <> "" Then
                cSumInsured = 0
            End If

            ReflectionHelper.SetMember(oSumInsured, "IsValuationRequired", ListViewHelper.GetListViewSubItem(oListItem, 5).Text)
            txtDate.Text = ListViewHelper.GetListViewSubItem(oListItem, 6).Text
            ReflectionHelper.SetMember(oSumInsured, "ValuationDate", (m_oFormFields.UnformatControl(txtDate)))

            If SubScreen Then
                'm_lReturn = ReflectionHelper.Invoke(oSumInsured, "SetKeys", New Object() {m_vWindowKeys})
                m_lReturn = oSumInsured.SetKeys(m_vWindowKeys)
            End If

            'm_lReturn = ReflectionHelper.Invoke(oSumInsured, "Start", New Object() {})
            m_lReturn = oSumInsured.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oSumInsured.Dispose()
                oSumInsured = Nothing
                Exit Sub
            End If

            'If not cancelled, add to grid

            If ReflectionHelper.GetMember(oSumInsured, "Status") = gPMConstants.PMEReturnCode.PMCancel Then
                oSumInsured.Dispose()
                oSumInsured = Nothing
                Exit Sub
            End If

            txtCurrency.Text = pnlTotalSumInsured(Index).Text
            cTotalSumInsured = (m_oFormFields.UnformatControl(txtCurrency))
            cTotalSumInsured -= cSumInsured
            oListItem.Text = ReflectionHelper.GetMember(oSumInsured, "Description")
            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = ReflectionHelper.GetMember(oSumInsured, "Reference")
            cSumInsured = ReflectionHelper.GetMember(oSumInsured, "SumInsured")
            m_lReturn = m_oFormFields.FormatControl(txtCurrency, cSumInsured)
            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = txtCurrency.Text
            m_lReturn = m_oFormFields.FormatControl(txtDate, ReflectionHelper.GetMember(oSumInsured, "DateAdded"))
            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtDate.Text
            m_lReturn = m_oFormFields.FormatControl(txtDate, ReflectionHelper.GetMember(oSumInsured, "DateDeleted"))
            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = txtDate.Text

            If txtDate.Text <> "" Then
                cSumInsured = 0
            End If

            ListViewHelper.GetListViewSubItem(oListItem, 5).Text = ReflectionHelper.GetMember(oSumInsured, "IsValuationRequired")
            m_lReturn = m_oFormFields.FormatControl(txtDate, ReflectionHelper.GetMember(oSumInsured, "ValuationDate"))
            ListViewHelper.GetListViewSubItem(oListItem, 6).Text = txtDate.Text
            cTotalSumInsured += cSumInsured
            m_lReturn = m_oFormFields.FormatControl(txtCurrency, cTotalSumInsured)
            pnlTotalSumInsured(Index).Text = txtCurrency.Text

            If txtRate(Index).Text <> "" Then
                dRate = (m_oFormFields.UnformatControlArray(txtRate(Index), GetIndex(txtRate(Index), txtRate)))
                cPremium = cTotalSumInsured * dRate / 100.0#
                m_lReturn = m_oFormFields.FormatControlArray(txtPremium(Index), cPremium, GetIndex(txtPremium(Index), txtPremium))
            End If

            oSumInsured.Dispose()
            oSumInsured = Nothing

            ' RAW 22/06/2004 : Performance Changes(#2) : added
            ' work out whether any details have changed and fire an onChange DL event accordingly
            ReDim vDataTypes(8)
            vDataTypes(m_klSumInsuredArrayColNo_Description) = m_klSumInsuredDataType_Description
            vDataTypes(m_klSumInsuredArrayColNo_Reference) = m_klSumInsuredDataType_Reference
            vDataTypes(m_klSumInsuredArrayColNo_SumInsured) = m_klSumInsuredDataType_SumInsured
            vDataTypes(m_klSumInsuredArrayColNo_DateAdded) = m_klSumInsuredDataType_DateAdded
            vDataTypes(m_klSumInsuredArrayColNo_DateDeleted) = m_klSumInsuredDataType_DateDeleted
            vDataTypes(m_klSumInsuredArrayColNo_ValuationReq) = m_klSumInsuredDataType_ValuationReq
            vDataTypes(m_klSumInsuredArrayColNo_ValuationDate) = m_klSumInsuredDataType_ValuationDate
            vDataTypes(m_klSumInsuredArrayColNo_Rate) = m_klSumInsuredDataType_Rate
            vDataTypes(m_klSumInsuredArrayColNo_Premium) = m_klSumInsuredDataType_Premium

            ' include all of the sum insured data items but only set the new values for the list view
            vArray = m_vScreenValues(CInt(Convert.ToString(cmdSumInsuredAdd(Index).Tag)))

            ' RAW 03/09/2004 : subtract 1
            i = oListItem.Index + 1 - 1

            vArray(m_klSumInsuredArrayColNo_Description, i) = oListItem.Text
            vArray(m_klSumInsuredArrayColNo_Reference, i) = ListViewHelper.GetListViewSubItem(oListItem, m_klSumInsuredListColNo_Reference).Text

            txtCurrency.Text = ListViewHelper.GetListViewSubItem(oListItem, m_klSumInsuredListColNo_SumInsured).Text
            vArray(m_klSumInsuredArrayColNo_SumInsured, i) = (m_oFormFields.UnformatControl(txtCurrency))

            txtDate.Text = ListViewHelper.GetListViewSubItem(oListItem, m_klSumInsuredListColNo_DateAdded).Text
            If txtDate.Text = "" Then
                vArray(m_klSumInsuredArrayColNo_DateAdded, i) = Nothing
            Else
                vArray(m_klSumInsuredArrayColNo_DateAdded, i) = (m_oFormFields.UnformatControl(txtDate))
            End If

            txtDate.Text = ListViewHelper.GetListViewSubItem(oListItem, m_klSumInsuredListColNo_DateDeleted).Text
            If txtDate.Text = "" Then
                vArray(m_klSumInsuredArrayColNo_DateDeleted, i) = Nothing
            Else
                vArray(m_klSumInsuredArrayColNo_DateDeleted, i) = (m_oFormFields.UnformatControl(txtDate))
            End If

            If ListViewHelper.GetListViewSubItem(oListItem, m_klSumInsuredListColNo_ValuationReq).Text = "Yes" Then
                vArray(m_klSumInsuredArrayColNo_ValuationReq, i) = 1
            Else
                vArray(m_klSumInsuredArrayColNo_ValuationReq, i) = 0
            End If

            txtDate.Text = ListViewHelper.GetListViewSubItem(oListItem, m_klSumInsuredListColNo_ValuationDate).Text
            If txtDate.Text = "" Then
                vArray(m_klSumInsuredArrayColNo_ValuationDate, i) = Nothing
            Else
                vArray(m_klSumInsuredArrayColNo_ValuationDate, i) = (m_oFormFields.UnformatControl(txtDate))
            End If

            ' This will fire a DL onChange event if details have changed
            SetScreenValues(CInt(Convert.ToString(cmdSumInsuredAdd(Index).Tag)), vArray, Convert.ToString(lvwSumInsured(Index).Tag), CDbl(m_vFrameArray(ACFTabNumber, GetIndex(lvwSumInsured(Index).Parent, fraFrame))) + 1, , vDataTypes)
            ' RAW 22/06/2004 : Performance Changes(#2) : end



            ' RAW 22/06/2004 : Performance Changes(#2) : removed call to RunDynamicLogic when m_bFireDLStartEvents = FALSE because onChange may be raised from SetScreenValues
            If m_bFireDLStartEvents Then
                ' RAW 03/09/2003 : CQ2213 : added
                'run dynamic logic incase details have changed
                ' RAW 21/05/2004 : Performance Changes : added params
                ' RAW 22/06/2004 : Performance Changes(#2) : replaced onChange with Start
                RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_Start, v_vObjIdx:=lvwSumInsured(Index).Tag, v_vControl:=cmdSumInsuredEdit(Index))
            End If


        Catch ex As Exception

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "cmdSumInsuredEdit_Click Failed", ACApp, ACClass, "cmdSumInsuredEdit_Click", Information.Err().Number, Information.Err().Description, excep:=ex)



        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

        End Try
    End Sub

    Private Sub cmdSumInsuredEdit_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cmdSumInsuredEdit_0.Enter
        Dim Index As Integer = Array.IndexOf(cmdSumInsuredEdit, eventSender)

        Dim lDebugDepthCounter As Integer

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : cmdSumInsuredEdit_GotFocus - " & " (" & Index & ")")
#End If

        ' RAW 21/05/2004 : Performance Changes : replaced CallScriptOnFocus with RunDynamicLogic and an OnFocus ProcedureType and added v_vControl param
        'RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_Edit, v_vObjIdx:=lvwSumInsured(Index).Tag, v_vControl:=cmdSumInsuredEdit(Index))

        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

    End Sub

    Private Sub cmdStandardWordingUp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cmdStandardWordingUp_0.Click
        Dim Index As Integer = Array.IndexOf(cmdStandardWordingUp, eventSender)

        Dim iLine As Integer
        Dim sCode, sDescription, sEdited As String
        Dim lNarrative As Integer


        Dim vDataTypes As Object
        Dim vArray(,) As Object
        Dim vTempArray(,) As Object

        Try

            'Set row to be moved - if a valid one selected
            'Is this list view empty
            If lvwStandardWording(Index).Items.Count < 1 Then
                Exit Sub
            End If

            'iLine = lvwStandardWording(Index).FocusedItem.Index + 1
            If lvwStandardWording(Index).SelectedItems.Count > 0 Then
                iLine = lvwStandardWording(Index).SelectedItems(0).Index + 1
            Else
                iLine = -1
            End If

            'Have we selected any
            If iLine = -1 Then
                Exit Sub
            End If

            'Is it really, really selected?
            If Not lvwStandardWording(Index).Items.Item(iLine - 1).Selected Then
                Exit Sub
            End If

            'If we're at number 1, there's no need to do anything
            If iLine = 1 Then
                Exit Sub
            End If

            'So let's swap them
            'iLine - 1 goes to temporary storage
            sCode = lvwStandardWording(Index).Items.Item(iLine - 2).Text
            sDescription = ListViewHelper.GetListViewSubItem(lvwStandardWording(Index).Items.Item(iLine - 2), 1).Text
            sEdited = ListViewHelper.GetListViewSubItem(lvwStandardWording(Index).Items.Item(iLine - 2), 2).Text
            lNarrative = (Convert.ToString(lvwStandardWording(Index).Items.Item(iLine - 2).Tag))

            'iLine goes to iLine - 1
            lvwStandardWording(Index).Items.Item(iLine - 2).Text = lvwStandardWording(Index).Items.Item(iLine - 1).Text
            ListViewHelper.GetListViewSubItem(lvwStandardWording(Index).Items.Item(iLine - 2), 1).Text = ListViewHelper.GetListViewSubItem(lvwStandardWording(Index).Items.Item(iLine - 1), 1).Text
            ListViewHelper.GetListViewSubItem(lvwStandardWording(Index).Items.Item(iLine - 2), 2).Text = ListViewHelper.GetListViewSubItem(lvwStandardWording(Index).Items.Item(iLine - 1), 2).Text
            lvwStandardWording(Index).Items.Item(iLine - 2).Tag = (Convert.ToString(lvwStandardWording(Index).Items.Item(iLine - 1).Tag))

            'temporary storage goes to iLine
            lvwStandardWording(Index).Items.Item(iLine - 1).Text = sCode
            ListViewHelper.GetListViewSubItem(lvwStandardWording(Index).Items.Item(iLine - 1), 1).Text = sDescription
            ListViewHelper.GetListViewSubItem(lvwStandardWording(Index).Items.Item(iLine - 1), 2).Text = sEdited
            lvwStandardWording(Index).Items.Item(iLine - 1).Tag = CStr(lNarrative)

            'Reset the selected item, automatically deselects the previous one
            lvwStandardWording(Index).Items.Item(iLine - 2).Selected = True

            cmdStandardWordingDelete(Index).Enabled = False

            'swap the data array also
            iLine -= 1
            vArray = m_vScreenValues(CInt(Convert.ToString(cmdStandardWordingAdd(Index).Tag)))

            ReDim vTempArray(3, 0)
            'Store the up record to temp array
            vTempArray(0, 0) = vArray(0, iLine - 1)
            vTempArray(1, 0) = vArray(1, iLine - 1)
            vTempArray(2, 0) = vArray(2, iLine - 1)
            vTempArray(3, 0) = vArray(3, iLine - 1)

            'swap the up record with current
            vArray(0, iLine - 1) = vArray(0, iLine)
            vArray(1, iLine - 1) = vArray(1, iLine)
            vArray(2, iLine - 1) = vArray(2, iLine)
            vArray(3, iLine - 1) = vArray(3, iLine)

            'swap the current record with up record data which is stored in temp array
            vArray(0, iLine) = (vTempArray(0, 0))
            vArray(1, iLine) = (vTempArray(1, 0))
            vArray(2, iLine) = (vTempArray(2, 0))
            vArray(3, iLine) = (vTempArray(3, 0))

            ReDim vDataTypes(3)
            vDataTypes(0) = iGISSharedConstants.GISDataTypeText
            vDataTypes(2) = iGISSharedConstants.GISDataTypeText
            vDataTypes(3) = iGISSharedConstants.GISDataTypeText

            'SetScreenValues(CInt(Convert.ToString(cmdStandardWordingAdd(Index).Tag)), vArray, Convert.ToString(lvwStandardWording(Index).Tag), CDbl(m_vFrameArray(ACFTabNumber, GetIndex(lvwStandardWording(Index).Parent, fraFrame))) + 1, , (vDataTypes))
            SetScreenValues(r_lScreenIndex:=cmdStandardWordingAdd(Index).Tag, r_vValues:=vArray, r_vObjIdx:=lvwStandardWording(Index).Tag, iTabNumber:=m_vFrameArray(ACFTabNumber, GetIndex(lvwStandardWording(Index).Parent, fraFrame)) + 1, v_vDataTypes:=vDataTypes)
            lvwStandardWording(Index).Focus()

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "cmdStandardWordingUp_Click Failed", ACApp, ACClass, "cmdStandardWordingUp_Click", Information.Err().Number, excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lblCheckLabel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _lblCheckLabel_0.Click
        Dim Index As Integer = Array.IndexOf(lblCheckLabel, eventSender)

        Dim lDebugDepthCounter As Integer

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : lblCheckLabel_Click - " & " (" & Index & ")")
#End If

        ' RAW 07/07/2003 : CQ1671 : added if enabled test
        If chkYesNo(Index).Enabled Then
            chkYesNo(Index).Focus()
        End If

        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

    End Sub

    Private Sub lblText_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _lblText_0.Click
        Dim Index As Integer = Array.IndexOf(lblText, eventSender)
        ' ***************************************************************** '
        '
        ' Name: lblText_Click
        '
        ' Description:
        '
        ' History: 13/07/2001 CLG - Created.
        '
        ' ***************************************************************** '

        Dim lDebugDepthCounter As Integer

        ' Debug message
#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : lblText_Click - " & " (" & Index & ")")
#End If

        Try

            Dim lpFile As String = ""

            If CDbl(Convert.ToString(lblText(Index).Tag)) = ndcHyperlink Then
                lpFile = txtText(Index).Text.Trim()
            End If

            Dim iTabId As Integer
            If lpFile <> "" Then
                lpFile = lpFile.ToLower()
                'check if this is jump to a tab
                If lpFile.IndexOf("tab:") + 1 Then

                    lpFile = Mid(lpFile, (lpFile.IndexOf("tab:") + 1) + 4)
                    Dim dbNumericTemp As Double
                    If Double.TryParse(lpFile, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                        iTabId = CInt(lpFile)
                        'If tabScreen.Tabs >= iTabId And iTabId > -1 Then
                        'tbd
                        lblText(Index).ForeColor = Color.FromArgb(255, 0, 0)
                        SelectTab(TabStrip1, iTabId - 1)
                        TabStrip1_Click(Nothing, Nothing)

                    End If
                Else
                    ' Spawn the default e-mail client so that the user can email PM
                    lblText(Index).ForeColor = Color.FromArgb(255, 0, 0)

                End If
            End If


        Catch ex As Exception


            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "lblText_Click Failed", ACApp, ACClass, "lblText_Click", Information.Err().Number, Information.Err().Description, excep:=ex)



        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

        End Try
    End Sub

    Private Sub lvwListView_ColumnClick_Body(ByRef Index As Integer, ByVal ColumnHeader As ColumnHeader, Optional ByRef iOrder As Integer = 2)
        Static lastIndex, lastColumn As Integer
        Static lastOrder As SortOrder
        iOrder = lastOrder
        If bCommandFromChildForm Then
            iOrder = SortOrder.Ascending
        Else
            If iOrder = SortOrder.Ascending Then
                iOrder = SortOrder.Descending
            Else
                iOrder = SortOrder.Ascending
            End If
        End If

        Select Case Convert.ToString(ColumnHeader.Tag)
            Case iGISSharedConstants.GISDataTypeDate
                ListViewFunc.ListViewSortByDate(lvwListView(Index), ColumnHeader.Index + 1 - 1, iOrder)
            Case iGISSharedConstants.GISDataTypeNumeric
                ListViewFunc.ListViewSortByValue(lvwListView(Index), ColumnHeader.Index + 1 - 1, iOrder)
            Case iGISSharedConstants.GISDataTypeCurrency
                ListViewSortByCurrencyValue(lvwListView(Index), ColumnHeader.Index + 1 - 1, iOrder)
            Case Else
                ListViewHelper.SetSortedProperty(lvwListView(Index), False)
                ListViewHelper.SetSortOrderProperty(lvwListView(Index), iOrder)
                ListViewHelper.SetSortKeyProperty(lvwListView(Index), ColumnHeader.Index + 1 - 1)
                ListViewHelper.SetSortedProperty(lvwListView(Index), True)
        End Select

        lastIndex = Index
        lastColumn = ColumnHeader.Index + 1
        lastOrder = iOrder


    End Sub

    Private Sub lvwListView_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs)  'Handles _lvwListView_0.ColumnClick
        'Dim ColumnHeader As ColumnHeader = lvwListView(0).Columns(eventArgs.Column)
        Dim ColumnHeader As ColumnHeader = eventSender.Columns(eventArgs.Column)

        Dim Index As Integer = Array.IndexOf(lvwListView, eventSender)

        'check if we have a sequence number on this object and if we do then ignore the request to sort
        If CDbl(Convert.ToString(cmdListViewSequenceUp(Index).Tag)) <> -1 Then
            Exit Sub
        End If

        lvwListView_ColumnClick_Body(Index, ColumnHeader)

    End Sub
    ''' <summary>
    ''' lvwListView_MouseDown
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub lvwListView_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs)  'Handles _lvwListView_0.MouseDown
        Dim nButton As Integer = CInt(eventArgs.Button)
        Dim nShift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        Dim nIndex As Integer = Array.IndexOf(lvwListView, eventSender)

        Dim nScreenDetailsIndex As Integer = CInt(CDbl(Convert.ToString(cmdListViewAdd(nIndex).Tag)) Mod 10000)  ' RAW 09/07/2004 : JIT : added and to be used instead of m_lControlCount ' RAW 30/09/2003 : CQ2673 : added

        Dim lTag As Integer = CInt(Convert.ToString(lvwListView(nIndex).Tag))

        If lvwListView(nIndex).GetItemAt(x, y) Is Nothing Then
            ' Nothing selected
            cmdListViewEdit(nIndex).Enabled = False
            cmdListViewDelete(nIndex).Enabled = False
        Else
            cmdListViewEdit(nIndex).Enabled = True
            cmdListViewEdit(nIndex).ForeColor = SystemColors.ControlText
            'Not if we're viewing...
            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                'And only if we're read only...
                'And definitely only if we've got a sub screen defined...
                If CDbl(m_vListViewArray(ACCChildId, nIndex)) <> 0 Then
                    'this horrible bit of code prevents the delet key being enabled if the frame is
                    'signaled as read only

                    If ColorTranslator.ToOle(lvwListView(nIndex).Parent.ForeColor) <> ColorTranslator.ToOle(SystemColors.ControlDark) Then
                        cmdListViewDelete(nIndex).Enabled = True
                    End If
                End If

                If CDbl(m_vScreenDetails(ACDExtraObjectType, nScreenDetailsIndex)) = GISOTAssociatedClient Then
                    cmdListViewDelete(nIndex).Enabled = True
                End If
            End If
        End If

    End Sub

    Private Sub lvwStandardWording_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _lvwStandardWording_0.Enter
        Dim Index As Integer = Array.IndexOf(lvwStandardWording, eventSender)

        Dim lDebugDepthCounter As Integer

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : lvwStandardWording_GotFocus ")
#End If

        ' RAW 21/05/2004 : Performance Changes : replaced CallScriptOnFocus with RunDynamicLogic and an OnFocus ProcedureType and added v_vControl param
        'RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_List, v_vObjIdx:=lvwStandardWording(Index).Tag, v_vControl:=lvwStandardWording(Index))

        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

    End Sub

    Private Sub lvwStandardWording_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs)  'Handles _lvwStandardWording_0.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        'Dim Index As Integer = Array.IndexOf(lvwStandardWording, eventSender)
        Dim Index As Integer = GetIndex(eventSender, lvwStandardWording)

        Dim lTag As Integer = CInt(Convert.ToString(lvwStandardWording(Index).Tag))

        If lvwStandardWording(Index).GetItemAt(x, y) Is Nothing Then
            ' Nothing selected
            cmdStandardWordingEdit(Index).Enabled = False
            cmdStandardWordingDelete(Index).Enabled = False
        Else
            'Not if we're viewing...
            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                'And only if we're read only...
                If (m_vDataDictionary(GISDMTypeRisk)(ACPIsInputProperty, lTag Mod 10000)) = 1 Then
                    cmdStandardWordingEdit(Index).Enabled = True
                    cmdStandardWordingDelete(Index).Enabled = True
                    'cmdStandardWordingEdit(Index).Visible = True
                End If
            Else
                cmdStandardWordingEdit(Index).Enabled = True
                'cmdStandardWordingEdit(Index).Visible = True
            End If
        End If

    End Sub

    Private Sub lvwSumInsured_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  ' 'Handles _lvwSumInsured_0.Enter
        Dim Index As Integer = Array.IndexOf(lvwSumInsured, eventSender)

        Dim lDebugDepthCounter As Integer

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : lvwSumInsured_GotFocus - " & " (" & Index & ")")
#End If

        ' RAW 21/05/2004 : Performance Changes : replaced CallScriptOnFocus with RunDynamicLogic and an OnFocus ProcedureType and added v_vControl param
        'RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_List, v_vObjIdx:=lvwSumInsured(Index).Tag, v_vControl:=lvwSumInsured(Index))

        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

    End Sub

    Private Sub lvwSumInsured_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs)  'Handles _lvwSumInsured_0.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        'Dim Index As Integer = Array.IndexOf(lvwSumInsured, eventSender)
        Dim Index As Integer = GetIndex(eventSender, lvwSumInsured)


        Dim lTag As Integer = CInt(Convert.ToString(lvwSumInsured(m_lSumInsuredIndex).Tag))

        If lvwSumInsured(Index).GetItemAt(x, y) Is Nothing Then
            ' Nothing selected
            cmdSumInsuredDelete(Index).Enabled = False
            cmdSumInsuredEdit(Index).Enabled = False
        Else
            'Not if we're viewing...
            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                'And only if we're read only...
                If (m_vDataDictionary(GISDMTypeRisk)(ACPIsInputProperty, lTag Mod 10000)) = 1 Then
                    'this horrible bit of code prevents the delet key being enabled if the frame is
                    'signaled as read only

                    If ColorTranslator.ToOle(lvwSumInsured(Index).Parent.ForeColor) <> ColorTranslator.ToOle(SystemColors.ControlDark) Then
                        cmdSumInsuredDelete(Index).Enabled = True
                        cmdSumInsuredEdit(Index).Enabled = True
                    End If
                End If
            End If
        End If

    End Sub

    Private Sub PBFindRT1_ClearValues(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _PBFindRT1_0.ClearValues
        Dim Index As Integer = Array.IndexOf(PBFindRT1, eventSender)
        Try

            Dim vArray(,) As Object

            'get control values
            'vArray = ArraysHelper.CastArray(Of Byte(,))(PBFindRT1(Index).DataArray)
            vArray = PBFindRT1(Index).DataArray
            'blank values
            If Information.IsArray(vArray) Then
                For i As Integer = 0 To vArray.GetUpperBound(1)
                    'which type of control
                    Select Case ToSafeInteger(vArray(3, i))

                        Case 1
                            SetValue(CStr(vArray(10, i)) & "." & CStr(vArray(11, i)), "")
                        Case 2
                            SetValue(CStr(vArray(10, i)) & "." & CStr(vArray(11, i)), "(None)")
                    End Select
                Next i
            End If

        Catch excep As System.Exception
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to Clear Values", ACApp, ACClass, "Start", Information.Err().Number, excep.Message, excep:=excep)
        End Try
    End Sub
    Private Sub PBFindRT1_FoundValues(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _PBFindRT1_0.FoundValues
        Dim Index As Integer = Array.IndexOf(PBFindRT1, eventSender)
        Try

            Dim vArray(,) As Object

            'get control values
            'vArray = VB6.CopyArray(PBFindRT1(Index).DataArray)
            vArray = PBFindRT1(Index).DataArray
            'fill values
            If Information.IsArray(vArray) Then
                For i As Integer = 0 To (vArray).GetUpperBound(1)
                    'which type of control
                    Select Case ToSafeInteger(vArray(3, i))
                        'text box
                        Case 1
                            SetValue((vArray(10, i)) & "." & (vArray(11, i)), vArray(7, i))
                            'combo
                        Case 2
                            'cboPMLookup(vArray(1, i)).DefaultItemId = vArray(7, i)
                            SetValue((vArray(10, i)) & "." & (vArray(11, i)), vArray(7, i))
                            'SetPMLValue CStr(vArray(7, i)), cboPMLookup(vArray(1, i))
                            'set focus in order to save data
                            'cboPMLookup(vArray(1, i)).SetFocus
                    End Select
                Next i
            End If

        Catch excep As System.Exception



            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to transfer values", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", excep:=excep)

        End Try

    End Sub


    Private Sub PBFindRT1_StartFind(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _PBFindRT1_0.StartFind
        Dim Index As Integer = Array.IndexOf(PBFindRT1, eventSender)
        Try
            Dim icontrolcount, iFieldCount As Integer
            Dim vVDA As Object

            'vVDA = VB6.CopyArray(PBFindRT1(Index).DataArray)
            vVDA = PBFindRT1(Index).DataArray

            If Not Information.IsArray(vVDA) Then Exit Sub

            icontrolcount = (vVDA).GetUpperBound(1)
            iFieldCount = (vVDA).GetUpperBound(0)

            'add search items from risk screen
            For i As Integer = 0 To icontrolcount
                'select control type
                Select Case ToSafeInteger(vVDA(3, i))
                    Case 1
                        vVDA(6, i) = GetValue((vVDA(10, i)) & "." & (vVDA(11, i)))
                    Case 2
                        vVDA(6, i) = GetValue((vVDA(10, i)) & "." & (vVDA(11, i)))(1)
                        'cater for "(none)"
                        If (vVDA(6, i)) = "(None)" Then
                            vVDA(6, i) = ""
                        End If

                End Select
            Next i

            'pass values back in
            PBFindRT1(Index).DataArray = (vVDA)

            'find it
            PBFindRT1(Index).Find()

        Catch excep As System.Exception



            '            iPMFunc.LogExcepMessage(gPMConstants.PMELogLevel.PMLogOnError, "Falied Start Find Initialisation", ACApp, ACClass, "Start", Information.Err().Number, excep.Message)


            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Falied Start Find Initialisation", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", excep:=excep)
            Exit Sub


        End Try

    End Sub

    ' ' RAW 21/05/2004 : Performance Changes : added
    Private Sub TabStrip1_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles TabStrip1.Enter
        Dim lDebugDepthCounter As Integer
        Dim lTabNumber As Integer

        If Not TabStrip1.SelectedTab Is Nothing Then

            Dim sTabID As String = TabStrip1.SelectedTab.Name.ToString()

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

            AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : TabStrip1_GotFocus - (" & CDbl(sTabID.Substring(Len(sTabID) - 1)) + 1 & ")")

#End If

            If sTabID = "" Then
                lTabNumber = -1
            Else
                lTabNumber = CInt(sTabID.Substring(1, sTabID.Length - 1))
            End If

            ' make sure that correct tab details are displayed as it is not always being done automatically
            ' this will fire a click event
            ' This will fire scripts for the control losing focus and this tab receiving focus so the following CallScriptOnFocus has been made redundant
            SelectTab(TabStrip1, lTabNumber)
            TabStrip1_Click(Nothing, Nothing)


            '    RunDynamicLogic v_eDLProcedureName:=eDLProcedureName_OnFocus, _
            ''                        v_vDLAction:=eOnFocusAction_Tab, _
            ''                        v_vTabNumber:=lTab + 1


            ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
            ' Debug message
#If (DebugOption) Then
            If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If
        End If
    End Sub

    Private Sub TabStrip1_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles TabStrip1.Click

        Static sLastTabID As String = ""
        Dim lLastTabNumber As Integer
        Dim sThisTabID As String = ""
        Dim lThisTabNumber As Integer
        Dim vControls As Object
        Dim iTabSetIndex As Integer
        Dim sSetFocusControlName As String = ""
        Dim iCtrlIndex As Integer = 0
        ReDim Preserve m_lTabsetIndexlist(2, iCtrlIndex)
        Array.Clear(m_lTabsetIndexlist, 0, 2 * iCtrlIndex)
        Try
            m_iLastTabIndexForTabStripClicked = TabStrip1.SelectedIndex
            If m_iLastTabIndexForTabStripClicked < 0 Then
                Exit Sub
            End If
            sThisTabID = TabStrip1.SelectedTab.Name

            ' has the tab actually changed
            If sLastTabID = sThisTabID Then
                Exit Sub
            End If


            ' ' RAW 21/05/2004 : Performance Changes : added
            ' get the number of the previous tab
            If sLastTabID = "" Then
                lLastTabNumber = -1
            Else
                lLastTabNumber = CInt(sLastTabID.Substring(sLastTabID.Length - (sLastTabID.Length - 1)))
            End If

            ' get the number of the current tab
            If sThisTabID = "" Then
                lThisTabNumber = -1
            Else
                lThisTabNumber = CInt(sThisTabID.Substring(sThisTabID.Length - (sThisTabID.Length - 1)))
            End If

            ' refresh the static variable that stores the last tab id. Note the tab number that is passed to the scripts is left unchanged
            sLastTabID = TabStrip1.SelectedTab.Name
            ' ' RAW 21/05/2004 : Performance Changes : end

            ' ' RAW 21/05/2004 : Performance Changes : added
            ' Call a script procedure for the previous tab
            If lLastTabNumber <> -1 Then
                ' RAW 22/06/2004 : Performance Changes(#2) : replace onChange with onLostFocus
                RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnLostFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_Tab, v_vTabNumber:=lLastTabNumber + 1)
            End If

            iTabSetIndex = 10000  'impossibly large number for tab set position


            'hide/show frames as appropriate to the current tab
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            Dim ictr As Integer
            For lFrameCnt As Integer = 0 To m_lFrameIndex

                If CDbl(m_vFrameArray(ACFTabNumber, lFrameCnt)) = lThisTabNumber Then
                    ' this frame is on this tab

                    ' RAW 09/07/2004 : JIT : added
                    ' create controls for this frame that have not yet been created

                    If TypeOf (m_vFrameArray(ACFGISObjectId, lFrameCnt)) Is DBNull Then
                        m_vFrameArray(ACFGISObjectId, lFrameCnt) = Nothing
                    End If

                    'PN24238
                    If Information.IsArray(m_vScreenDetails) Then
                        Dim uBnd As Integer = m_vScreenDetails.GetUpperBound(1)
                        For iScreenDetailsCnt As Integer = 0 To uBnd
                            ictr = 0
                            If (m_vScreenDetails(ACDHelpText, iScreenDetailsCnt) Is Nothing = False) Then
                                Dim sHelptextDetails As String = m_vScreenDetails(ACDHelpText, iScreenDetailsCnt).ToString.TrimEnd
                                If sHelptextDetails <> String.Empty Then
                                    For iScrDetailcnt As Integer = iScreenDetailsCnt + 1 To uBnd
                                        If (m_vScreenDetails(ACDHelpText, iScrDetailcnt) IsNot Nothing) Then
                                            If m_vScreenDetails(ACDHelpText, iScrDetailcnt).ToString.TrimEnd() = sHelptextDetails Then
                                                m_vScreenDetails(ACDHelpText, iScrDetailcnt) = m_vScreenDetails(ACDHelpText, iScrDetailcnt).ToString & "(" & ictr.ToString & ")"
                                                ictr += 1
                                            End If
                                        End If
                                    Next iScrDetailcnt
                                End If
                            End If

                            vControls = Nothing

                            ' The use of late bound objects as array gives EXTREMELY POOR performance and this is amplified 
                            ' when the array element is access within a loop.  To alleviate this we will just get the values once.
                            Dim defaultObjectId As Integer = ToSafeInteger(m_vScreenDetails(ACDDefaultObjectId, iScreenDetailsCnt), 0)
                            Dim defaultPropertyId As Integer = ToSafeInteger(m_vScreenDetails(ACDDefaultPropertyId, iScreenDetailsCnt), 0)
                            Dim parentId As Integer = ToSafeInteger(m_vScreenDetails(ACDParentId, iScreenDetailsCnt), -1)
                            Dim gisObjectId As Integer = ToSafeInteger(m_vScreenDetails(ACDGISObjectId, iScreenDetailsCnt), 0)
                            Dim propertyId As Integer = ToSafeInteger(m_vScreenDetails(ACDGISPropertyId, iScreenDetailsCnt), 0)
                            Dim childScreenId As Integer = ToSafeInteger(m_vScreenDetails(ACDChildScreenId, iScreenDetailsCnt), 0)
                            Dim tabSetIndex As Integer = ToSafeInteger(m_vScreenDetails(ACDTabSetIndex, iScreenDetailsCnt), 0)
                            Dim frameGisObjectId As Integer = ToSafeInteger(m_vFrameArray(ACFGISObjectId, lFrameCnt), 0)

                            If (parentId = lFrameCnt) Or (gisObjectId = frameGisObjectId And (propertyId = 0)) Then

                                ' this control either is, or is on, this frame
                                If propertyId = 0 Then

                                    ' not a property
                                    If childScreenId <> 0 Then
                                        ' This is a listview. Has it already been created ?

                                        vControls = GetControlFromTag(defaultObjectId, "", gPMConstants.PMEReturnCode.PMTrue)

                                        If Not Information.IsArray(vControls) Then
                                            ' control has not been created yet so do it now
                                            AddControlToForm(iScreenDetailsCnt)
                                        End If
                                    End If
                                Else
                                    ' this is a property
                                    'object bound field?
                                    If gisObjectId <> 0 Then

                                        vControls = GetControlFromTag(defaultObjectId, defaultPropertyId, gPMConstants.PMEReturnCode.PMTrue)

                                        If Not Information.IsArray(vControls) Then
                                            AddControlToForm(iScreenDetailsCnt)
                                        End If

                                        'Find control with lowest valid tab index and use it to set focus to
                                        If tabSetIndex = True And tabSetIndex < iTabSetIndex Then
                                            iTabSetIndex = tabSetIndex
                                            sSetFocusControlName = (m_vScreenDetails(ACDExtraGISObjectName, iScreenDetailsCnt).ToString) & "." & (m_vScreenDetails(ACDExtraGISPropertyName, iScreenDetailsCnt).ToString)
                                        End If
                                        If CBool(m_vScreenDetails(ACDTabSetIndex, iScreenDetailsCnt)) Then

                                            m_lTabsetIndexlist(0, iCtrlIndex) = CStr(m_vScreenDetails(ACDCaption, iScreenDetailsCnt))
                                            m_lTabsetIndexlist(1, iCtrlIndex) = CStr(m_vScreenDetails(ACDTabSetIndex, iScreenDetailsCnt))
                                            m_lTabsetIndexlist(2, iCtrlIndex) = CStr(m_vScreenDetails(ACDDefaultPropertyId, iScreenDetailsCnt))
                                            iCtrlIndex = iCtrlIndex + 1
                                            ReDim Preserve m_lTabsetIndexlist(2, iCtrlIndex)
                                        End If
                                    Else
                                        ' not in data dictionary so cant locate via DD tag
                                        Select Case propertyId
                                            Case ndcFreeFormatText, ndcHyperlink

                                                vControls = GetControlFromTag(propertyId, iScreenDetailsCnt, gPMConstants.PMEReturnCode.PMTrue)

                                                If Not Information.IsArray(vControls) Then
                                                    ' control has not been created yet so do it now
                                                    AddControlToForm(iScreenDetailsCnt)
                                                End If

                                            Case ndcFindControl
                                        End Select
                                    End If
                                End If
                            End If
                        Next
                    End If
                    fraFrame(lFrameCnt).Visible = True
                Else
                    fraFrame(lFrameCnt).Visible = False
                End If
            Next lFrameCnt

            'set focus to a control if indicated by tab set index
            If sSetFocusControlName <> "" Then
                SetFocus(sSetFocusControlName)
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' RAW 21/05/2004 : Performance Changes : added params
            ' RAW 09/07/2004 : JIT : moved from before making frames visible
            ' Call a script procedure for the current tab
            RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_Tab, v_vTabNumber:=lThisTabNumber + 1)

            ' RAW 29/06/2004 : added : reinstated Start event
            ' RAW 09/07/2004 : JIT : removed test for m_bIsLoading = False
            If m_bFireDLStartEvents Then
                ' RAW 21/05/2004 : Performance Changes : added params
                ' RAW 22/06/2004 : Performance Changes(#2) : replaced onChange with Start
                RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_Start, v_vTabNumber:=lThisTabNumber + 1)
            End If

            'reduce overhead by only calling ResetTabIndex only if controls were added when this tab was loaded
            If m_bResetTabIndexRequired Then
                ResetTabIndex()
                m_bResetTabIndexRequired = False
            End If

            If Task = gPMConstants.PMEComponentAction.PMView Then
                ' Disable the interface to only allow viewing.
                m_lReturn = DisableForm(True)
            End If

        Catch ex As Exception

            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "TabStrip1_Click failed", ACApp, ACClass, "TabStrip1_Click", Information.Err().Number, Information.Err().Description, excep:=ex)

        End Try
    End Sub

    Private Sub TabStrip1_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles TabStrip1.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Try

            'ctrl + alt + F12 ONLY
            If Shift = 6 And eventArgs.KeyCode = Keys.F12 And m_bDLDebuggingAllowed Then
                'check the status of dynamic logic debugging  on / off
                If m_bDLDebuggingEnabled Then
                    If MessageBox.Show("Turn OFF dynamic logic debugging?", "Debugging Dynamic Logic", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.OK Then
                        m_bDLDebuggingEnabled = False

                        'Reset the Dynamic Logic script
                        LoadDLEngine()
                    End If
                Else
                    If MessageBox.Show("Turn ON dynamic logic debugging?", "Debugging Dynamic Logic", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.OK Then
                        m_bDLDebuggingEnabled = True
                    End If
                End If
                Me.DebugMode = m_bDLDebuggingEnabled
            End If

        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TabStrip1_KeyUp failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TabStrip1_KeyUP", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
        End Try
    End Sub
    Private Sub txtPremium_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _txtPremium_0.Enter
        Dim Index As Integer = Array.IndexOf(txtPremium, eventSender)

        Dim lDebugDepthCounter As Integer

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : txtPremium_GotFocus - " & " (" & Index & ")")
#End If

        m_lReturn = m_oFormFields.GotFocus(txtPremium(Index), , Index)
        ' RAW 21/05/2004 : Performance Changes : replaced CallScriptOnFocus with RunDynamicLogic and an OnFocus ProcedureType and added v_vControl param
        ' The use of v_vObjIdx and v_vPropIdx here is a bit of a fudge.
        ' Instead of passing indexes, the object and property names themselves are passed.
        ' v_vObjIdx contains the GIS object and property name whilst the v_vPropIdx contains
        ' the name of the control within the special control
        'RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_Text, v_vObjIdx:=ObjectAndPropertyName(lvwSumInsured(Index).Tag), v_vPropIdx:="PREMIUM", v_vControl:=txtPremium(Index))
        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

    End Sub

    Private Sub txtPremium_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs)  'Handles _txtPremium_0.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        Dim Index As Integer = Array.IndexOf(txtPremium, eventSender)  ' RAW 21/05/2004 : Performance Changes : replaced LostFocus

        Dim lDebugDepthCounter As Integer
        Dim dTemp As Double
        Dim cTemp As Decimal
        Dim vDataTypes() As Integer
        Dim vArray(,) As Object

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : txtPremium_Validate - " & " (" & Index & ")")
#End If

        m_lReturn = m_oFormFields.LostFocus(txtPremium(Index), , Index)
        dTemp = (m_oFormFields.UnformatControlArray(txtPremium(Index), GetIndex(txtPremium(Index), txtPremium)))

        Try

            cTemp = dTemp

            If Information.Err().Number <> 0 Then
                MessageBox.Show("Currency value is too large - please re-enter", "Risk screen", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Cancel = True  ' RAW 21/05/2004 : Performance Changes : replaced SetFocus
            Else

                ' RAW 22/06/2004 : Performance Changes(#2) : added
                ReDim vDataTypes(8)
                vDataTypes(m_klSumInsuredArrayColNo_Description) = m_klSumInsuredDataType_Description
                vDataTypes(m_klSumInsuredArrayColNo_Reference) = m_klSumInsuredDataType_Reference
                vDataTypes(m_klSumInsuredArrayColNo_SumInsured) = m_klSumInsuredDataType_SumInsured
                vDataTypes(m_klSumInsuredArrayColNo_DateAdded) = m_klSumInsuredDataType_DateAdded
                vDataTypes(m_klSumInsuredArrayColNo_DateDeleted) = m_klSumInsuredDataType_DateDeleted
                vDataTypes(m_klSumInsuredArrayColNo_ValuationReq) = m_klSumInsuredDataType_ValuationReq
                vDataTypes(m_klSumInsuredArrayColNo_ValuationDate) = m_klSumInsuredDataType_ValuationDate
                vDataTypes(m_klSumInsuredArrayColNo_Rate) = m_klSumInsuredDataType_Rate
                vDataTypes(m_klSumInsuredArrayColNo_Premium) = m_klSumInsuredDataType_Premium

                ' include all of the sum insured data items but only set the new value for premium
                If Information.IsArray(m_vScreenValues(CInt(Convert.ToString(cmdSumInsuredAdd(Index).Tag)))) Then
                    vArray = m_vScreenValues(CInt(Convert.ToString(cmdSumInsuredAdd(Index).Tag)))
                Else
                    vArray = Array.CreateInstance(GetType(Object), New Integer() {9, 1}, New Integer() {0, 0})
                End If
                vArray(m_klSumInsuredArrayColNo_Premium, 0) = cTemp

                ' This will fire a DL onChange event if details have changed
                ' The use of v_vObjIdx and v_vPropIdx here is a bit of a fudge.
                ' Instead of passing indexes, the object and property names themselves are passed.
                ' v_vObjIdx contains the GIS object and property name whilst the v_vPropIdx contains
                ' the name of the control within the special control
                SetScreenValues(CInt(Convert.ToString(cmdSumInsuredAdd(Index).Tag)), vArray, ObjectAndPropertyName(Convert.ToString(lvwSumInsured(Index).Tag)), CDbl(m_vFrameArray(ACFTabNumber, GetIndex(lvwSumInsured(Index).Parent, fraFrame))) + 1, "PREMIUM", vDataTypes)
                ' RAW 22/06/2004 : Performance Changes(#2) : end

                ' RAW 21/05/2004 : Performance Changes : added params
                ' RAW 22/06/2004 : Performance Changes(#2) : replaced onChange with OnLostFocus
                ' The use of v_vObjIdx and v_vPropIdx here is a bit of a fudge.
                ' Instead of passing indexes, the object and property names themselves are passed.
                ' v_vObjIdx contains the GIS object and property name whilst the v_vPropIdx contains
                ' the name of the control within the special control
                RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnLostFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_Text, v_vObjIdx:=ObjectAndPropertyName(lvwSumInsured(Index).Tag), v_vPropIdx:="PREMIUM", v_vControl:=txtPremium(Index))

            End If
            ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
            ' Debug message
#If (DebugOption) Then
            If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

            eventArgs.Cancel = Cancel

        Catch exc As System.Exception
        End Try
    End Sub

    Private Sub txtRate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _txtRate_0.Enter
        Dim Index As Integer = Array.IndexOf(txtRate, eventSender)

        Dim lDebugDepthCounter As Integer

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : txtRate_GotFocus - " & " (" & Index & ")")
#End If

        m_lReturn = m_oFormFields.GotFocus(txtRate(Index), , Index)
        ' RAW 21/05/2004 : Performance Changes : replaced CallScriptOnFocus with RunDynamicLogic and an OnFocus ProcedureType and added v_vControl param
        ' The use of v_vObjIdx and v_vPropIdx here is a bit of a fudge.
        ' Instead of passing indexes, the object and property names themselves are passed.
        ' v_vObjIdx contains the GIS object and property name whilst the v_vPropIdx contains
        ' the name of the control within the special control
        'RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_Text, v_vObjIdx:=ObjectAndPropertyName(lvwSumInsured(Index).Tag), v_vPropIdx:="RATE", v_vControl:=txtRate(Index))
        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

    End Sub

    Private Sub txtRate_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs)  'Handles _txtRate_0.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        Dim Index As Integer = Array.IndexOf(txtRate, eventSender)  ' RAW 21/05/2004 : Performance Changes : replaced LostFocus

        Dim lDebugDepthCounter As Integer
        Dim cSumInsured As Decimal
        Dim dRate As Double
        Dim cPremium As Decimal
        Dim vArray(,) As Object

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : txtRate_Validate - " & " (" & Index & ")")
#End If

        m_lReturn = m_oFormFields.LostFocus(txtRate(Index), , Index)

        If txtRate(Index).Text <> "" Then
            txtCurrency.Text = pnlTotalSumInsured(Index).Text

            cSumInsured = (m_oFormFields.UnformatControl(txtCurrency))

            dRate = (m_oFormFields.UnformatControlArray(txtRate(Index), GetIndex(txtRate(Index), txtRate)))

            cPremium = cSumInsured * dRate / 100.0#

            m_lReturn = m_oFormFields.FormatControlArray(txtPremium(Index), cPremium, GetIndex(txtPremium(Index), txtPremium))

            txtPremium(Index).Enabled = False
        Else
            txtPremium(Index).Enabled = True
        End If

        ' RAW 22/06/2004 : Performance Changes(#2) : added
        Dim vDataTypes(8) As Integer
        vDataTypes(m_klSumInsuredArrayColNo_Description) = m_klSumInsuredDataType_Description
        vDataTypes(m_klSumInsuredArrayColNo_Reference) = m_klSumInsuredDataType_Reference
        vDataTypes(m_klSumInsuredArrayColNo_SumInsured) = m_klSumInsuredDataType_SumInsured
        vDataTypes(m_klSumInsuredArrayColNo_DateAdded) = m_klSumInsuredDataType_DateAdded
        vDataTypes(m_klSumInsuredArrayColNo_DateDeleted) = m_klSumInsuredDataType_DateDeleted
        vDataTypes(m_klSumInsuredArrayColNo_ValuationReq) = m_klSumInsuredDataType_ValuationReq
        vDataTypes(m_klSumInsuredArrayColNo_ValuationDate) = m_klSumInsuredDataType_ValuationDate
        vDataTypes(m_klSumInsuredArrayColNo_Rate) = m_klSumInsuredDataType_Rate
        vDataTypes(m_klSumInsuredArrayColNo_Premium) = m_klSumInsuredDataType_Premium

        ' include all of the sum insured data items but only set the new value for rate
        If Information.IsArray(m_vScreenValues(CInt(Convert.ToString(cmdSumInsuredAdd(Index).Tag)))) Then
            vArray = m_vScreenValues(CInt(Convert.ToString(cmdSumInsuredAdd(Index).Tag)))
        Else
            vArray = Array.CreateInstance(GetType(Object), New Integer() {9, 1}, New Integer() {0, 0})
        End If
        vArray(m_klSumInsuredArrayColNo_Rate, 0) = dRate

        ' the premium is read only so need to set it as well because
        ' the validate event is not called
        vArray(m_klSumInsuredArrayColNo_Premium, 0) = cPremium

        SetScreenValues(CInt(Convert.ToString(cmdSumInsuredAdd(Index).Tag)), vArray, ObjectAndPropertyName(Convert.ToString(lvwSumInsured(Index).Tag)), CDbl(m_vFrameArray(ACFTabNumber, GetIndex(lvwSumInsured(Index).Parent, fraFrame))) + 1, "RATE", vDataTypes)

        RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnLostFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_Text, v_vObjIdx:=ObjectAndPropertyName(lvwSumInsured(Index).Tag), v_vPropIdx:="RATE", v_vControl:=txtRate(Index))

        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

        eventArgs.Cancel = Cancel
    End Sub
    Private Sub txtMlText_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _txtMlText_0.Enter
        Dim Index As Integer = Array.IndexOf(txtMlText, eventSender)
        genericText_GotFocus(lblMlText(Index), txtMlText(Index), Index)

    End Sub
    Private Sub txtFormattedText_Enter(ByVal eventSender As Object, ByVal e As System.EventArgs)
        Dim Index As Integer = Array.IndexOf(txtFormattedText, eventSender)
        genericText_GotFocus(lblFormattedText(Index), txtFormattedText(Index), Index)
    End Sub

    Private Sub txtFormattedText_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs)
        Dim Cancel As Boolean = eventArgs.Cancel
        Dim Index As Integer = Array.IndexOf(txtFormattedText, eventSender)
        genericText_LostFocus(lblFormattedText(Index), txtFormattedText(Index), Index)
        eventArgs.Cancel = Cancel
    End Sub
    Private Sub txtText_KeyPress(ByVal eventSender As Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs)
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        Dim Index As Integer = Array.IndexOf(txtText, eventSender)
        ValueEdited = True
        ValueEditedForIndex = Index
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Public Sub CallLostFocusAndValidateEvent()
        genericText_LostFocus(lblText(ValueEditedForIndex), txtText(ValueEditedForIndex), ValueEditedForIndex)
        genericText_GotFocus(lblText(ValueEditedForIndex), txtText(ValueEditedForIndex), ValueEditedForIndex)
    End Sub

    Private Sub txtText_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _txtText_0.Enter
        Dim Index As Integer = Array.IndexOf(txtText, eventSender)

        genericText_GotFocus(lblText(Index), txtText(Index), Index)

    End Sub
    Private Sub genericText_GotFocus(ByRef r_ctlLblControl As Object, ByRef r_ctlTxtControl As Object, ByRef Index As Integer)

        Dim lDebugDepthCounter As Integer

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : genericText_GotFocus - " & r_ctlTxtControl.Name & " (" & Index & ")")
#End If

        If TypeOf r_ctlTxtControl Is uctSIRRTFControl.uctRichTextBox Then

        Else
            m_lReturn = m_oFormFields.GotFocus((r_ctlTxtControl), 0, Index)
        End If

        ' RAW 21/05/2004 : Performance Changes : replaced CallScriptOnFocus with RunDynamicLogic and an OnFocus ProcedureType and added v_vControl param

        RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_Text, v_vObjIdx:=r_ctlTxtControl.Tag, v_vControl:=r_ctlTxtControl)

        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

    End Sub
    Private Sub txtMlText_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs)  'Handles _txtMlText_0.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        Dim Index As Integer = Array.IndexOf(txtMlText, eventSender)  ' RAW 21/05/2004 : Performance Changes : replaced LostFocus

        genericText_LostFocus(lblMlText(Index), txtMlText(Index), Index)

        eventArgs.Cancel = Cancel
    End Sub

    Private Sub txtText_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs)  'Handles _txtText_0.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        Dim Index As Integer = Array.IndexOf(txtText, eventSender)  ' RAW 21/05/2004 : Performance Changes : replaced LostFocus

        If Not m_bControlIsUnloading Then
            genericText_LostFocus(lblText(Index), txtText(Index), Index)
        End If
        eventArgs.Cancel = Cancel
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_ctlLblControl"></param>
    ''' <param name="r_ctlTxtControl"></param>
    ''' <param name="Index"></param>
    ''' <remarks></remarks>
    Private Sub genericText_LostFocus(ByRef r_ctlLblControl As Object, ByRef r_ctlTxtControl As Object, ByRef Index As Integer)
        Dim nDebugDepthCounter As Integer = 0
        Dim nTag As Integer = 0
        Dim nTag2 As Integer = 0
        Dim oArray() As Object = Nothing
        Dim sTemp As String = String.Empty
        Dim dTemp As Double = 0.0
        Dim crTemp As Decimal = 0.0
        Dim nTemp As Integer = 0

        Try
            If r_ctlTxtControl.Text.Contains("{") Or r_ctlTxtControl.Text.Contains("}") Then
                Interaction.MsgBox("{ or } is not allowed", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "Validation Failure")
            End If

            ' RAW 09/07/2004 : JIT : added test to suppress if triggered from SetValue
            If Index >= 0 Then  ' Not triggered from dynamic logic (SetValue)
#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then
                AddToDebug(r_lDepthCounter:=nDebugDepthCounter, v_sText:="EVENT : genericText_LostFocus - " & r_ctlTxtControl.Name & " (" & Index & ")")
#End If
            End If

            ' Warning - When this function is called (indirectly) from dynamic logic (SetValue) the index will be -1
            ' ===============================================================================

            'CQ2806 Whitespace Date Field Not Validated

            nTag2 = ReflectionHelper.GetMember(r_ctlTxtControl, "Tag")
            If iGISSharedConstants.GISDataTypeDate = (m_vDataDictionary(GISDMTypeRisk)(uctRiskScreenControl.MainModule.ACPDataType, nTag2 Mod 10000)) Then
                ReflectionHelper.SetMember(r_ctlTxtControl, "Text", ReflectionHelper.GetMember(r_ctlTxtControl, "Text").Trim())
            End If

            If Index >= 0 And Not (TypeOf r_ctlTxtControl Is uctSIRRTFControl.uctRichTextBox) Then  'dynamic logic doesn't know index so....
                m_lReturn = m_oFormFields.LostFocus(r_ctlTxtControl, , Index)
            End If

            nTag = ReflectionHelper.GetMember(r_ctlLblControl, "Tag")
            oArray = Array.CreateInstance(GetType(Object), New Integer() {2}, New Integer() {0})

            If TypeOf r_ctlTxtControl Is TextBox Then
                If CType(r_ctlTxtControl, TextBox).Multiline Then
                    sTemp = (m_oFormFields.UnformatControlArray(r_ctlTxtControl, GetIndex(r_ctlTxtControl, txtMlText)))
                ElseIf CType(r_ctlTxtControl, TextBox).Name.Contains("txtText") Then
                    sTemp = (m_oFormFields.UnformatControlArray(r_ctlTxtControl, GetIndex(r_ctlTxtControl, txtText)))
                End If
            ElseIf TypeOf r_ctlTxtControl Is uctSIRRTFControl.uctRichTextBox Then
                sTemp = r_ctlTxtControl.TextRTF
            Else
                sTemp = (m_oFormFields.UnformatControlArray(r_ctlTxtControl, Index))
            End If

            ' RAW 01/07/2003 : CQ1660 : added
            If Index >= 0 Then
                With m_oFormFields.Item(ReflectionHelper.GetMember(r_ctlTxtControl, "Name") & "-" & CStr(Index))
                    If .ControlType = gPMConstants.PMEControlType.PMTextBox AndAlso Not .IsMandatory AndAlso .Text = "" Then
                        ' this is optional text box that is empty and we wish to keep its contents empty
                        ' so overwrite results from unformatcontrolarray
                        sTemp = ""
                    End If
                End With
            End If
            ' RAW 01/07/2003 : CQ1660 : end

            'vArray(0) = sTemp
            oArray(0) = sTemp
            If CStr(oArray(0)) = "" Then
                '   vArray(0) = Nothing
                oArray(0) = Nothing
            End If

            oArray(1) = oArray(0)
            SetScreenValues(nTag, oArray, nTag2, CDbl(m_vFrameArray(ACFTabNumber, GetIndex(r_ctlTxtControl.Parent, fraFrame))) + 1)
            oArray = Nothing

            Select Case m_vDataDictionary(GISDMTypeRisk)(uctRiskScreenControl.MainModule.ACPDataType, nTag2 Mod 10000)
                Case iGISSharedConstants.GISDataTypePercentage
                    If sTemp <> "" Then
                        If StringsHelper.ToDoubleSafe(sTemp) > 100.0# Then
                            SelectTab(TabStrip1, CInt(m_vFrameArray(ACFTabNumber, GetIndex(r_ctlTxtControl.parent, fraFrame))))
                            MessageBox.Show("Percentage cannot be greater than 100", "Risk screen", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            TabStrip1_Click(Nothing, Nothing)
                            ReflectionHelper.Invoke(r_ctlTxtControl, "Focus", New Object() {})
                            Exit Sub
                        End If
                    End If

                Case iGISSharedConstants.GISDataTypeNumeric
                    If sTemp <> "" Then
                        Dim dbNumericTemp As Double
                        If Not Double.TryParse(sTemp, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                            SelectTab(TabStrip1, CInt(m_vFrameArray(ACFTabNumber, GetIndex(r_ctlTxtControl.Parent, fraFrame))))
                            MessageBox.Show("Value must be numeric", "Risk screen", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            r_ctlTxtControl.Text = ""
                            TabStrip1_Click(Nothing, Nothing)
                            ReflectionHelper.Invoke(r_ctlTxtControl, "Focus", New Object() {})
                            Exit Sub
                        End If

                        nTemp = CInt(sTemp)

                        If Information.Err().Number <> 0 Then
                            SelectTab(TabStrip1, CInt(m_vFrameArray(ACFTabNumber, GetIndex(r_ctlTxtControl.Parent, fraFrame))))
                            MessageBox.Show("Numeric value is too large - please re-enter", "Risk screen", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            r_ctlTxtControl.Text = ""
                            TabStrip1_Click(Nothing, Nothing)
                            r_ctlTxtControl.Text = ""
                            ReflectionHelper.Invoke(r_ctlTxtControl, "Focus", New Object() {})
                            Exit Sub
                        End If
                    End If

                Case iGISSharedConstants.GISDataTypeCurrency
                    dTemp = (m_oFormFields.UnformatControlArray(r_ctlTxtControl, GetIndex(r_ctlTxtControl, txtText)))

                    If dTemp >= 1000000000000000 Then
                        SelectTab(TabStrip1, CInt(m_vFrameArray(ACFTabNumber, GetIndex(r_ctlTxtControl.Parent, fraFrame))))
                        MessageBox.Show("Currency value is too large - please re-enter", "Risk screen", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        TabStrip1_Click(Nothing, Nothing)
                        ReflectionHelper.Invoke(r_ctlTxtControl, "Focus", New Object() {})
                        Exit Sub
                    End If


                    crTemp = dTemp

                    If Information.Err().Number <> 0 Then
                        SelectTab(TabStrip1, CInt(m_vFrameArray(ACFTabNumber, GetIndex(r_ctlTxtControl.Parent, fraFrame))))
                        MessageBox.Show("Currency value is too large - please re-enter", "Risk screen", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        TabStrip1_Click(Nothing, Nothing)
                        ReflectionHelper.Invoke(r_ctlTxtControl, "Focus", New Object() {})
                        Exit Sub
                    End If
            End Select

            If Index >= 0 Then  'don't go recursive if ran from dynamic logic !
                ' RAW 21/05/2004 : Performance Changes : added params
                ' RAW 22/06/2004 : Performance Changes(#2) : replaced onChange with OnLostFocus
                RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnLostFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_Text, v_vObjIdx:=r_ctlTxtControl.Tag, v_vControl:=r_ctlTxtControl)
            End If

        Catch ex As Exception

            ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
            ' Debug message
            ' RAW 09/07/2004 : JIT : added test to suppress if triggered from SetValue
            If Index >= 0 Then  ' Not triggered from dynamic logic (SetValue)
#If (DebugOption) Then
                If nDebugDepthCounter > 0 Then AddToDebug(nDebugDepthCounter * -1)  ' decrement the counter
#End If
            End If

            Throw New ApplicationException(gPMConstants.PMEReturnCode.PMError.ToString() + ", ExportInternalExceptionError, " + "Error: " & Information.Err().Number & " - " & ex.Message)

        End Try
    End Sub

    Private Sub uctCLMPayment_UnRecoverableError(ByVal Sender As Object, ByVal e As EventArgs) Handles uctCLMPayment.UnRecoverableError
        RaiseEvent ClaimPaymentUnrecoverableError(Me, Nothing)
    End Sub

    ' ***************************************************************** '
    '
    ' Name: uctCLMPerilRT1_AddClick
    '
    ' Description:
    '
    ' History:  24/02/2003  CLG - Created.
    '
    '           5/8/2003    RVH - Modified for IAG to call "sub Start()"
    '                       script when adding a claim peril
    '           12-11-2004  MEvans : CQ6977
    ' ***************************************************************** '
    Private Sub uctCLMPerilRT1_AddClick(ByVal Sender As Object, ByVal e As uctCLMPerilRTControl.uctCLMPerilRT.AddClickEventArgs) Handles uctCLMPerilRT1.AddClick
        Const sFunctionName As String = "uctCLMPerilRT1_AddClick"
        '
        Dim lDebugDepthCounter As Integer
        '
        ' Debug message
#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then
        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : uctCLMPerilRT1_AddClick")
#End If
        '
        Try

            Dim vTempArray As Array
            Dim vClaimPerilKeys As Object
            Dim vOrgArray As Array
            Dim lGISScreenId, lClaimPerilId As Integer
            Dim bLossSchedule As Boolean
            Dim lPerilTypeID, lLossScheduleType As Integer
            Dim sChildOIKey As String = ""
            Dim lScreenDetailsIndex As Integer  ' RAW 09/07/2004 : JIT : added and to be used instead of m_lControlCount
            Dim lObjectIndex As Integer
            Dim vKeyArray(,) As Object = e.vKeyArray
            '
            Const PBCQemQuoteTypeDefault As Integer = 4
            '
            '**************
            ' MEvans : 23-09-2003 : CQ2089 \ CQ1943
            ' populate the m_vScreenValues with the latest
            ' screen values
            If InterfaceToBusiness() = gPMConstants.PMEReturnCode.PMTrue Then

                If ReflectionHelper.Invoke(m_oInterface, "LoadGisFromScreenValues", New Object() {m_vScreenValues}) <> gPMConstants.PMEReturnCode.PMTrue Then
                    '
                    LogMessageToFile(g_oObjectManager.UserName, gPMConstants.PMELogLevel.PMLogOnError, sFunctionName & " Failed to load the screen values into the gis object", ACApp, ACClass, sFunctionName)
                    '
                End If
                '
            End If
            '**************
            '
            'update screen to buisness
            'Interface.Update r_vScreenValues:=m_vScreenValues
            '
            'reload the data
            m_lReturn = ReflectionHelper.Invoke(ReflectionHelper.GetMember(Interface_Renamed, "GIS"), "ReloadSpecialsFromDB", New Object() {m_lClaimID})
            '
            'this hasn't created us a new entry for the new claim peril so do it manually
            '
            ' RVH 08/01/2004 - CQ3350: Need to obtain correct value for lScreenDetailsIndex
            '                          before getting data
            lScreenDetailsIndex = CInt(Convert.ToString(uctCLMPerilRT1.Tag))
            '
            ' MEvans : 12-11-2004 : CQ6977
            ' get the object id from the screen details array; this is required
            ' when calling setscreenvalues
            lObjectIndex = CInt(m_vScreenDetails(ACDDefaultObjectId, lScreenDetailsIndex))
            '
            'get data for this list view
            vOrgArray = m_vScreenValues(lScreenDetailsIndex)
            '
            'make space in temp array for original + 1
            vTempArray = Array.CreateInstance(GetType(Object), New Integer() {vOrgArray.GetUpperBound(0) + 2, vOrgArray.GetUpperBound(1) + 1}, New Integer() {0, 0})
            '
            'copy from original
            For lTemp As Integer = vOrgArray.GetLowerBound(0) To vOrgArray.GetUpperBound(0)
                For lTemp2 As Integer = vOrgArray.GetLowerBound(1) To vOrgArray.GetUpperBound(1)
                    vTempArray(lTemp, lTemp2) = vOrgArray(lTemp, lTemp2)
                Next lTemp2
            Next lTemp
            '
            'copy parent values
            vTempArray(vTempArray.GetUpperBound(0), vTempArray.GetUpperBound(1) - 1) = vTempArray(0, 2)  'sParentOIKey
            vTempArray(vTempArray.GetUpperBound(0), vTempArray.GetUpperBound(1) - 2) = vTempArray(0, 1)  'sChildObjectName
            vTempArray(vTempArray.GetUpperBound(0), vTempArray.GetUpperBound(1) - 3) = vTempArray(0, 0)  'sParentObjectName
            '
            'now get the new child key
            ReflectionHelper.GetMember(Interface_Renamed, "GIS").GetALLOIKey("work_claim_peril", vClaimPerilKeys)
            '
            'new key is always the last one in the array
            vTempArray.SetValue((vClaimPerilKeys)(vClaimPerilKeys.GetUpperBound(0)), vTempArray.GetUpperBound(0), vTempArray.GetUpperBound(1)) 'sChildOIKey
            '
            sChildOIKey = CStr((vClaimPerilKeys)(vClaimPerilKeys.GetUpperBound(0)))
            '
            ' MEvans : 12-11-2004 : CQ6977
            ' changed to pass the object id rather than the screenindexvalue
            'save the fixed up data for this list view
            SetScreenValues(lScreenDetailsIndex, vTempArray, lObjectIndex, CDbl(m_vFrameArray(ACFTabNumber, GetIndex(uctCLMPerilRT1.Parent, fraFrame))) + 1)
            '
            'loop through and pick out data about the newly added peril
            For lLoop As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                Select Case vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lLoop)
                    Case "GIS_Screen_id"
                        lGISScreenId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lLoop))
                    Case "work_claim_peril_id"
                        lClaimPerilId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lLoop))
                    Case "LossSchedule"
                        bLossSchedule = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lLoop))
                    Case "PerilTypeID"
                        lPerilTypeID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lLoop))
                    Case "LossScheduleTypeID"
                        lLossScheduleType = IIf(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lLoop) = "", 0, CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lLoop)))
                End Select
            Next
            '
            'call new method to force run of "sub Start()" rule on child screen
            'associated with the newly added claim peril
            m_lReturn = m_oInterface.RunScreenRule(iPBCQemQuoteType:=PBCQemQuoteTypeDefault, sChildOIKey:=sChildOIKey, lScreenId:=lGISScreenId)
            '
            '
        Catch ex As Exception
            '
            '
            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "uctCLMPerilRT1_AddClick Failed", ACApp, ACClass, "uctCLMPerilRT1_AddClick", Information.Err().Number, Information.Err().Description, excep:=ex)
            '
            '
        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

        End Try
    End Sub

    ' ***************************************************************** '
    '
    ' Name: uctCLMPerilRT1_DeleteClick
    '
    ' Description:
    '
    ' History:  08-09-2003  MEvans - Created : CQ 2157, CQ 2490.
    '
    ' ***************************************************************** '
    Private Sub uctCLMPerilRT1_DeleteClick(ByVal Sender As Object, ByVal e As uctCLMPerilRTControl.uctCLMPerilRT.DeleteClickEventArgs) Handles uctCLMPerilRT1.DeleteClick
        Dim lDebugDepthCounter As Integer
        '
        ' Debug message
#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then
        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : uctCLMPerilRT1_AddClick")
#End If
        '
        Try
            '
            Dim vTempArray As Array
            Dim vOrginalArray As Array
            Dim sChildOIKey As String = ""
            Dim lCnt As Integer
            Dim lClaimPerilId, lScreenDetailsIndex As Integer  ' RAW 09/07/2004 : JIT : added and to be used instead of m_lControlCount
            Dim vKeyArray(,) As Object = e.vKeyArray
            '
            Const PBCQemQuoteTypeDefault As Integer = 4
            '
            ' RVH 08/01/2004 - CQ3350: Need to obtain correct value for lScreenDetailsIndex
            '                          before getting data
            lScreenDetailsIndex = CInt(Convert.ToString(uctCLMPerilRT1.Tag))
            '
            'get data for this list view
            vOrginalArray = m_vScreenValues(lScreenDetailsIndex)
            '
            'make space in temp array for original + 1
            vTempArray = Array.CreateInstance(GetType(Object), New Integer() {vOrginalArray.GetUpperBound(0) + 2, vOrginalArray.GetUpperBound(1) + 1}, New Integer() {0, 0})
            '
            vTempArray = Array.CreateInstance(GetType(Object), New Integer() {vOrginalArray.GetUpperBound(0), vOrginalArray.GetUpperBound(1) + 1}, New Integer() {0, 0})
            '
            ' reset the item counter
            lCnt = 0
            '
            'rebuild array from original - just dont include item to delete
            For lTemp As Integer = vOrginalArray.GetLowerBound(0) To vOrginalArray.GetUpperBound(0)
                '
                ' if the item is a peril then
                If CStr(vOrginalArray(lTemp, 1)) = "Work_Claim_Peril" Then
                    '
                    ' if we have a claim peril id
                    If CStr(vOrginalArray(lTemp, 3)) <> "" Then
                        '
                        ' compare the id with the one we want to delete
                        ' from the original array
                        If CStr(vOrginalArray(lTemp, 3)).Substring(2) <> vKeyArray(1, 1) Then
                            '
                            ' add the item if it doesnt match the one we want
                            ' to delete
                            For lTemp2 As Integer = vOrginalArray.GetLowerBound(1) To vOrginalArray.GetUpperBound(1)
                                vTempArray(lCnt, lTemp2) = vOrginalArray(lTemp, lTemp2)
                            Next lTemp2
                            '
                            ' increment item counter
                            lCnt += 1
                        End If
                    Else
                        ' if we dont have a claim peril id just add the item
                        ' because it isnt the one we need to delete
                        For lTemp2 As Integer = vOrginalArray.GetLowerBound(1) To vOrginalArray.GetUpperBound(1)
                            vTempArray(lCnt, lTemp2) = vOrginalArray(lTemp, lTemp2)
                        Next lTemp2
                        ' increment item counter
                        lCnt += 1
                    End If
                Else
                    ' shouldnt ever be here but just in case
                    For lTemp2 As Integer = vOrginalArray.GetLowerBound(1) To vOrginalArray.GetUpperBound(1)
                        vTempArray(lCnt, lTemp2) = vOrginalArray(lTemp, lTemp2)
                    Next lTemp2
                    '
                    ' increment item counter
                    lCnt += 1
                End If
            Next lTemp
            '
            'save the fixed up data for this list view
            SetScreenValues(lScreenDetailsIndex, vTempArray, Convert.ToString(uctCLMPerilRT1.Tag), CDbl(m_vFrameArray(ACFTabNumber, GetIndex(uctCLMPerilRT1.Parent, fraFrame))) + 1)
            '
            'get the bits we need from the array
            For lLoop As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                Select Case vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lLoop)
                    Case "peril_id"
                        lClaimPerilId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lLoop))
                End Select
            Next
            '
            'jmf 23/10/2003 - remove the node from the GIS xml
            DeleteClaimPeril(lClaimPerilId, ReflectionHelper.GetMember(Interface_Renamed, "GIS"))
            '
            '
        Catch ex As Exception
            '
            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "uctCLMPerilRT1_DeleteClick Failed", ACApp, ACClass, "uctCLMPerilRT1_DeleteClick", Information.Err().Number, Information.Err().Description, excep:=ex)
            '
            '
        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If
        End Try
    End Sub

    ' ***************************************************************** '
    '
    ' Name: uctCLMPerilRT1_EditClick
    '
    ' Description:
    '
    ' History: 24/02/2003 CLG - Created.
    '
    ' ***************************************************************** '
    Private Sub uctCLMPerilRT1_EditClick(ByVal Sender As Object, ByVal e As uctCLMPerilRTControl.uctCLMPerilRT.EditClickEventArgs) Handles uctCLMPerilRT1.EditClick

        Const sFunctionName As String = "uctCLMPerilRT1_EditClick"
        '
        Dim lDebugDepthCounter As Integer
        '
        ' Debug message
#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then
        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : uctCLMPerilRT1_EditClick")
#End If
        '
        Try
            '
            Dim vArray(,) As Object
            Dim lTag As Integer
            Dim sChildOIKey, sParentOIKey, sChildObjectName, sParentObjectName As String
            Dim lScreenId, lClaimPerilId As Integer
            Dim vChildScreenValues As Object
            Dim bLossSchedule, bReserveLimitExceeded As Boolean
            Dim lPerilTypeID, lLossScheduleType As Integer
            Dim lChildStatus As gPMConstants.PMEReturnCode
            Dim lScreenDetailsIndex As Integer  ' RAW 09/07/2004 : JIT : added and to be used instead of m_lControlCount
            Dim newObj As Object
            Dim vKeyArray(,) As Object = e.vKeyArray
            Dim dExceededReserve As Decimal
            '
            '*********
            ' MEvans : 05-09-2003 : CQ2455
            RaiseEvent PerilEdit(Me, Nothing)
            '*********
            If Not Information.IsArray(vKeyArray) Then
                Exit Sub
            End If
            '
            ' RVH 08/01/2004 - CQ3350: Need to obtain correct value for lScreenDetailsIndex
            '                          before getting data
            lScreenDetailsIndex = CInt(Convert.ToString(uctCLMPerilRT1.Tag))

            vArray = m_vScreenValues(lScreenDetailsIndex)
            '
            'get the bits we need from th array
            For lLoop As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                '
                '
                Select Case vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lLoop)
                    Case "GIS_Screen_id"
                        lScreenId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lLoop))
                    Case "work_claim_peril_id"
                        lClaimPerilId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lLoop))
                    Case "LossSchedule"

                        bLossSchedule = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lLoop)
                    Case "PerilTypeID"
                        lPerilTypeID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lLoop))
                    Case "LossScheduleTypeID"
                        lLossScheduleType = gPMFunctions.ToSafeInteger(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lLoop))
                End Select
            Next
            '
            'jump through hoop to work out which one to edit
            newObj = ReflectionHelper.GetMember(Interface_Renamed, "GIS")
            newObj.GetALLOIKey("work_claim_peril", vChildScreenValues)

            For lTag = 0 To (vChildScreenValues).GetUpperBound(0)
                newObj.GetPropertyValue("work_claim_peril", "claim_peril_id", vChildScreenValues(lTag), sChildOIKey)
                If StringsHelper.ToDoubleSafe(sChildOIKey) = lClaimPerilId Then
                    Exit For
                End If
            Next
            '
            lTag += 1  'add one to step over parent
            '
            'And these are its key
            sChildOIKey = CStr(vArray(lTag, vArray.GetUpperBound(1)))
            sParentOIKey = CStr(vArray(lTag, vArray.GetUpperBound(1) - 1))
            sChildObjectName = CStr(vArray(lTag, vArray.GetUpperBound(1) - 2))
            sParentObjectName = CStr(vArray(lTag, vArray.GetUpperBound(1) - 3))
            m_oInterface.Task = m_iTask

            m_oInterface.ObjectType = m_vScreenDetails(ACDExtraObjectType, lScreenDetailsIndex)
            m_oInterface.ChildIndex = lTag
            ''
            ''make sure all claims
            m_oInterface.ClaimID = m_lClaimID
            m_oInterface.ClaimPerilID = lClaimPerilId
            m_oInterface.PerilID = m_lPerilID
            m_oInterface.ClaimTransactionType = m_sClaimTransactionType
            m_oInterface.ClaimInsFileCnt = m_lClaimInsFileCnt
            m_oInterface.ClaimRiskId = m_lClaimRiskId
            m_oInterface.LossSchedule = bLossSchedule
            m_oInterface.PerilTypeId = lPerilTypeID
            m_oInterface.LossScheduleTypeId = lLossScheduleType

            If Not (m_lObjectType = GISOTAssociatedClient Or m_lObjectType = GISOTDisclosure) Then
                m_oInterface.PartyCnt = m_lPartyCnt
            End If

            If InterfaceToBusiness() = gPMConstants.PMEReturnCode.PMTrue Then

                If m_oInterface.LoadGisFromScreenValues(m_vScreenValues) <> gPMConstants.PMEReturnCode.PMTrue Then

                    LogMessageToFile(g_oObjectManager.UserName, gPMConstants.PMELogLevel.PMLogOnError, sFunctionName & " Failed to load the screen values into the gis object", ACApp, ACClass, sFunctionName)

                End If

            End If

            m_lReturn = m_oInterface.DisplaySubScreen(lScreenId:=lScreenId,
                                                  sParentOIKey:=sParentOIKey,
                                                  sChildOIKey:=sChildOIKey,
                                                  sParentObjectName:=sParentObjectName,
                                                  sChildObjectName:=sChildObjectName,
                                                  r_vMyScreenValues:=m_vScreenValues,
                                                  r_vSubScreenValues:=vChildScreenValues,
                                                  vRiskTypeDetails:=m_vRiskTypeDetails,
                                                  vData:=m_aDataToChild,
                                                  r_lStatus:=lChildStatus,
                                                  r_bReserveLimitExceeded:=bReserveLimitExceeded,
                                                  r_dExceededReserve:=dExceededReserve)
            '
            ' RAW 14/10/2003 : CQ2754 : added
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ReflectionHelper.SetMember(MyBase.ParentForm, "ReserveLimitExceeded", bReserveLimitExceeded)
            ReflectionHelper.SetMember(MyBase.ParentForm, "ExceededReserve", dExceededReserve)

            '
            ' RAW 22/06/2004 : Performance Changes(#2) : added
            If lChildStatus = gPMConstants.PMEReturnCode.PMCancel Then
                uctCLMPerilRT1.Status = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If
            '
            uctCLMPerilRT1.Status = gPMConstants.PMEReturnCode.PMOK
            '
            m_lReturn = RefreshScreenControls(m_vScreenValues)
            ' RAW 14/10/2003 : CQ2754 : end
            '
            '
        Catch ex As Exception
            '
            '
            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "uctCLMPerilRT1_EditClick Failed", ACApp, ACClass, "uctCLMPerilRT1_EditClick", Information.Err().Number, Information.Err().Description, excep:=ex)
            '
            '
            '
        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If
        End Try
    End Sub

    Private Sub uctCLMPerilRT1_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles uctCLMPerilRT1.GotFocus

        Dim lDebugDepthCounter As Integer

#If (DebugOption And m_klDebugOption_Events) = m_klDebugOption_Events Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="EVENT : uctCLMPerilRT1_GotFocus ")
#End If

        ' RAW 21/05/2004 : Performance Changes : replaced CallScriptOnFocus with RunDynamicLogic and an OnFocus ProcedureType and added v_vControl param
        'RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnFocus, v_vDLAction:=enumOnFocusAction.eOnFocusAction_FurtherDetails, v_vObjIdx:=uctCLMPerilRT1.Tag, v_vControl:=uctCLMPerilRT1)
        ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

    End Sub

    Private Sub uctCLMPerilRT1_PerilListChanged(ByVal Sender As Object, ByVal e As EventArgs) Handles uctCLMPerilRT1.PerilListChanged

        ' AMB 10/03/2003: IS2851 - raise this event to the parent container

        RaiseEvent PerilListChanged(Me, Nothing)

    End Sub

    Private Sub UserControl_Initialize()
        m_bFireDLStartEvents = True  ' Set this to False at a later date once existing dynamic logic scripts have been modified to make use of the new functionality this provides

        g_bIsInIDE = IsInIDE()  ' RAW 09/07/2004 : JIT : added
    End Sub

    'Initialize Properties for User Control

    Private Sub UserControl_InitProperties()
        m_BackColor = m_def_BackColor
        m_ForeColor = m_def_ForeColor
        m_Enabled = m_def_Enabled


        'm_Font = UpgradeStubs.VB_UserControl.getAmbient(Me).getFont()
        m_Font = Me.Font
        m_BackStyle = m_def_BackStyle
        m_BorderStyle = Windows.Forms.BorderStyle.None
    End Sub

    Private Sub RiskScreen_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown As Integer

        Const ACCtrlMask As Integer = 2

        Try

            ' Set the control key value.
            iCtrlDown = (Shift And ACCtrlMask) > 0

            With TabStrip1
                ' Check the key pressed.
                Select Case KeyCode
                    Case Keys.PageUp
                        ' Page Up key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the first tab.
                            '.TabPages(0).Focused = True
                            .TabPages(0).Select()
                        Else
                            ' Check we are not on the
                            ' first tab.
                            If ContainerHelper.GetControlIndex(.SelectedTab) > 0 Then
                                ' Display the previous tab.
                                '.TabPages(ContainerHelper.GetControlIndex(.SelectedTab) - 1).Focused = True
                                .TabPages(ContainerHelper.GetControlIndex(.SelectedTab) - 1).Select()
                            End If
                        End If

                    Case Keys.PageDown
                        ' Page Down key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the last tab.
                            TabStrip1.SelectTab(TabStrip1.TabPages.Count)
                            TabStrip1_Click(Nothing, Nothing)
                        Else
                            ' Check we are not on the
                            ' last tab.
                            If ContainerHelper.GetControlIndex(TabStrip1.SelectedTab) < TabStrip1.TabPages.Count Then
                                TabStrip1.SelectTab(ContainerHelper.GetControlIndex(TabStrip1.SelectedTab) + 1)
                                TabStrip1_Click(Nothing, Nothing)
                            End If
                        End If

                    Case Keys.Home
                        ' Home key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If ContainerHelper.GetControlIndex(.SelectedTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlStart, ContainerHelper.GetControlIndex(.SelectedTab)).Focus()
                            End If
                        End If

                    Case Keys.End
                        ' End key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If ContainerHelper.GetControlIndex(.SelectedTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlEnd, ContainerHelper.GetControlIndex(.SelectedTab)).Focus()
                            End If
                        End If
                End Select
            End With

        Catch
            Exit Sub
        End Try
    End Sub



    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)
        m_BackColor = (PropBag.ReadProperty("BackColor", m_def_BackColor))
        m_ForeColor = (PropBag.ReadProperty("ForeColor", m_def_ForeColor))
        m_Enabled = (PropBag.ReadProperty("Enabled", m_def_Enabled))
        m_Font = PropBag.ReadProperty("Font", Me.Font)
        m_BackStyle = (PropBag.ReadProperty("BackStyle", m_def_BackStyle))
        m_BorderStyle = PropBag.ReadProperty("BorderStyle", m_def_BorderStyle)
        m_bUserMode = Not DesignMode
    End Sub

    Private Sub RiskScreen_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        TabStrip1.SetBounds(0, 0, MyBase.Width, MyBase.Height)
    End Sub

    Private Sub RiskScreen_Paint(ByVal eventSender As Object, ByVal eventArgs As PaintEventArgs) Handles MyBase.Paint
        If m_sSetFocusControlName <> "" Then
            If gPMConstants.PMEReturnCode.PMError <> SetFocus(m_sSetFocusControlName) Then
                m_sSetFocusControlName = ""
            End If
        End If
    End Sub

    Private Sub UserControl_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        Try

            m_lParentHwnd = MyBase.FindForm().Handle.ToInt32()
            'KeepWindowOnTop(False)

        Catch
        End Try
    End Sub

    'Write property values to storage


    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)

        PropBag.WriteProperty("BackColor", m_BackColor, m_def_BackColor)

        PropBag.WriteProperty("ForeColor", m_ForeColor, m_def_ForeColor)

        PropBag.WriteProperty("Enabled", m_Enabled, m_def_Enabled)



        'PropBag.WriteProperty("Font", m_Font, UpgradeStubs.VB_UserControl.getAmbient(Me).getFont())
        PropBag.WriteProperty("Font", m_Font, Me.Font)

        PropBag.WriteProperty("BackStyle", m_BackStyle, m_def_BackStyle)

        PropBag.WriteProperty("BorderStyle", m_BorderStyle, m_def_BorderStyle)

        PropBag.WriteProperty("PartyCnt", m_PartyCnt, m_def_PartyCnt)
    End Sub


    '*****************************************************************
    '
    ' Name: GetColumnTotal
    '
    ' Description: Gets the total value of a sepcified column of a list view
    '              given the object.attribute name as a string and the column
    '              as a 1 based value
    '
    ' History: 13/06/2002 CLG - Created.
    ' ***************************************************************** '
    Public Function GetColumnTotal(ByVal sName As String, ByVal sColumnName As String) As Object
        Dim result As Double
        Try
            Dim vControls As Object
            Dim sControlName As String = ""
            result = CStr(0)
            vControls = GetControlFromObjectAndAttributeName(sName)

            If Information.IsArray(vControls) Then
                sControlName = Convert.ToString(ReflectionHelper.GetMember(vControls(0), "Name"))
                If sControlName.LastIndexOf("_") > 0 Then
                    sControlName = sControlName.Substring(1, sControlName.LastIndexOf("_") - 1)
                End If

                Select Case sControlName
                    Case "fraFrame"

                        For lTemp1 As Integer = 0 To CInt(vControls(1).Columns.Count) - 1

                            If sColumnName.Trim().ToLower() = vControls(1).Columns(lTemp1).Text.ToLower() Then

                                For lTemp2 As Integer = 0 To CInt(vControls(1).Items.Count) - 1
                                    If lTemp1 = 0 Then

                                        If IsNumeric(vControls(1).Items(lTemp2)) Then

                                            result = ToSafeDouble(result) + ToSafeDouble(vControls(1).Items(lTemp2))
                                        Else
                                            result = result + ToSafeDouble(vControls(1).Items(lTemp2).SubItems(lTemp1).Text)
                                        End If
                                    Else
                                        If IsNumeric(vControls(1).Items(lTemp2).SubItems(lTemp1).Text) Then
                                            result = result + ToSafeDouble(vControls(1).Items(lTemp2).SubItems(lTemp1).Text)
                                        End If
                                    End If
                                Next
                                Return result
                            End If
                        Next
                        MessageBox.Show("Could not find " & sName & "." & sColumnName.Trim(), "Dynamic Logic Error : GetColumnTotal", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Case Else
                        MessageBox.Show(sName & " is not a listview", "Dynamic Logic Error : GetColumnTotal", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Select
            End If


            Return result

        Catch excep As System.Exception



            result = ""

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetColumnTotal Failed for " & sName & "." & sColumnName.Trim(), vApp:=ACApp, vClass:=ACClass, vMethod:="GetColumnTotal", excep:=excep)

            Return result



            Return result
        End Try
    End Function

    'RVH 09/12/2004 - 1.9 Merge
    '*****************************************************************
    '
    ' Name: GetListViewValue
    '
    ' Description: Gets the value of a sepcified column and row of a list view
    '              given the object name and the column title as strings and the row as a 1 based value
    '
    ' History: 08/11/2004 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function GetListViewValue(ByVal sName As String, ByVal sColumnName As String, ByVal lRow As Integer) As String

        ' Debug message
        Dim result As String = String.Empty
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".GetColumnTotal")

        Try
            Dim vControls As Object
            Dim sControlName As String = ""
            result = CStr(0)
            vControls = GetControlFromObjectAndAttributeName(sName)
            If Information.IsArray(vControls) Then
                sControlName = ReflectionHelper.GetMember(vControls(0), "Name")
                If sControlName.LastIndexOf("_") > 0 Then
                    sControlName = sControlName.Substring(1, sControlName.LastIndexOf("_") - 1)
                End If
                Select Case sControlName
                    Case "fraFrame"
                        lRow -= 1

                        If CType(vControls(1), ListView).Items.Count > lRow Then

                            For lTemp1 As Integer = 0 To CInt(CType(vControls(1), ListView).Columns.Count - 1)

                                If sColumnName.ToLower() = CType(vControls(1), ListView).Columns(lTemp1).Text.ToLower() Then
                                    If lRow = 0 Then

                                        If Information.IsReference(CType(vControls(1), ListView).FocusedItem) Then

                                            If Not IsNothing(CType(vControls(1), ListView).FocusedItem) Then
                                                lRow = CType(vControls(1), ListView).FocusedItem.Index
                                            End If
                                        Else
                                            Return result
                                        End If
                                    End If
                                    If lTemp1 = 0 Then

                                        Return CType(vControls(1), ListView).Items(lRow).Text
                                    Else

                                        Return CType(vControls(1), ListView).Items(lRow).SubItems(lTemp1).Text
                                    End If
                                End If
                            Next
                            MessageBox.Show("Could not find " & sName & "." & sColumnName, "Dynamic Logic Error : GetListViewValue", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If
                    Case Else
                        MessageBox.Show(sName & " is not a listview", "Dynamic Logic Error : GetListViewValue", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Select
            End If

            Return result

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".GetListViewValue")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".GetListViewValue")

            result = ""

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListViewValue Failed for " & sName & "." & sColumnName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetListViewValue", excep:=excep)

            Return result



            Return result
        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: GetValue
    '
    ' Description: Gets the value of a control given the object.attribute name as a string
    '
    ' History: 01/02/2002 CLG - Created.
    ' ***************************************************************** '
    Public Function GetValue(ByVal sName As String) As Object

        Dim result As Object = Nothing
        Try
            Dim vControls As Object
            Dim lIndex As Integer
            Dim sControlName As String = ""
            result = ""
            If sName.ToLower().Substring(0, Math.Min(sName.ToLower().Length, 16)) = "work_claim_peril" Then
                vControls = GetControlFromObjectAndAttributeName(sName, True, False)
            Else
                vControls = GetControlFromObjectAndAttributeName(sName)
            End If

            If Information.IsArray(vControls) Then
                sControlName = Convert.ToString(vControls(0).Name)
                If sControlName.LastIndexOf("_") > 0 Then
                    sControlName = sControlName.Substring(1, sControlName.LastIndexOf("_") - 1)
                End If
                Select Case sControlName
                    Case "chkYesNo"
                        result = CType(vControls(0), CheckBox).CheckState
                         'If vControls(0).Text.ToString.ToUpper = "UNKNOWN" Then
                        '    result = 2
                        'Else
                         '    result = CInt(IIf(ReflectionHelper.GetMember(vControls(0), "Checked"), 1, 0))
                        'End If

                    Case "fraFrame"

                        If vControls(1).GetType().Name = "ListView" Then
                            result = ReflectionHelper.GetMember(ReflectionHelper.GetMember(vControls(1), "Items"), "Count")
                        ElseIf vControls(1).GetType().Name.ToUpper() = "UCTCLMRESERVE" Then
                            ReflectionHelper.Invoke(vControls(1), "GetReserveGridInArray", New Object() {result})
                        ElseIf vControls(1).GetType().Name.ToUpper() = "UCTCLMPAYMENT1" Then
                            ReflectionHelper.Invoke(vControls(1), "GetPaymentGridInArray", New Object() {result})
                        ElseIf (vControls(2)) = "ADDRESS_CNT" Then   'PN24885
                            result = New Object() {txtAddress1((vControls(3))), txtAddress2((vControls(3))), txtAddress3((vControls(3))), txtAddress4((vControls(3))), txtAddress5((vControls(3))), txtAddress6((vControls(3)))}
                        Else
                            result = ""
                        End If
                    Case "cboAccumulation"

                        If ReflectionHelper.GetMember(vControls(0), "ListIndex") = -1 Then
                            result = ""
                        Else
                            result = ReflectionHelper.Invoke(vControls(0), "List", New Object() {ReflectionHelper.GetMember(vControls(0), "ListIndex")})
                        End If
                    Case "cboGISLookup", "cboPMLookup"
                        result = New Object() {vControls(0).ItemId, vControls(0).ItemCaption}
                    Case "lblText", "lblFormattedText"
                        'result = ReflectionHelper.GetMember(vControls(0), "Text")
                        result = vControls(0).Text
                    Case "TabStrip1"
                        result = ReflectionHelper.Invoke(vControls(0), "Tabs", New Object() {vControls(1)})
                    Case "pnlPolicyPanel"
                        'Allow the user to get at data associated with the find party control
                        lIndex = ReflectionHelper.GetMember(vControls(0), "Tag")
                        result = New Object() {m_vScreenValues(lIndex)(0, 0), m_vScreenValues(lIndex)(1, 0), CStr(m_vScreenValues(lIndex)(2, 0)).Trim()}
                    Case "pnlPartyPanel"
                        lIndex = ReflectionHelper.GetMember(vControls(1), "Tag")
                        result = New Object() {m_vScreenValues(lIndex)(0, 0), m_vScreenValues(lIndex)(1, 0), CStr(m_vScreenValues(lIndex)(2, 0)).Trim()}
                    Case "txtFormattedText"
                        'result = ReflectionHelper.GetMember(vControls(0), "TextRTF")
                        result = vControls(0).TextRTF
                    Case Else
                        'result = ReflectionHelper.GetMember(vControls(0), "Text")

                        If (vControls(0).Text = "") Then
                            result = Nothing
                        Else
                            result = vControls(0).Text
                        End If

                End Select
            End If

            Return result

        Catch excep As System.Exception




            result = ""

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetValue Failed for " & sName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetValue", excep:=excep)

            Return result



            Return result
        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: SetCaption
    '
    ' Description: Sets the caption of a control given the object.attribute name as a string
    '
    ' History: 12/06/2002 CLG - Created.
    ' ***************************************************************** '
    Public Sub SetCaption(ByVal sName As String, ByRef vValue As Object)

        Try
            Dim vControls As Object
            Dim sControlName As String = ""

            vControls = GetControlFromObjectAndAttributeName(sName)

            ' RAW 14/02/2004 : added test for IsArray
            If Information.IsArray(vControls) Then
                sControlName = ReflectionHelper.GetMember(vControls(0), "Name")
                If sControlName.LastIndexOf("_") > 0 Then
                    sControlName = sControlName.Substring(1, sControlName.LastIndexOf("_") - 1)
                End If
                Select Case sControlName
                    Case "lblText", "fraFrame"

                        If Not ReflectionHelper.GetMember(vControls(0), "Visible") Then

                            ReflectionHelper.SetMember(vControls(0), "Visible", True)
                        End If

                        ReflectionHelper.SetMember(vControls(0), "Text", (vValue))
                    Case "TabStrip1"
                        TabSetCaption(TabStrip1, CInt((vControls(1)) - 1), vValue)
                    Case Else

                        If Not ReflectionHelper.GetMember(vControls(1), "Visible") Then

                            ReflectionHelper.SetMember(vControls(1), "Visible", True)
                        End If

                        ReflectionHelper.SetMember(vControls(1), "Text", (vValue))
                End Select
            End If

        Catch excep As System.Exception



            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetCaption Failed for " & sName, vApp:=ACApp, vClass:=ACClass, vMethod:="SetCaption", excep:=excep)

            Exit Sub



        End Try


    End Sub

    Public Sub SetValue(ByVal sName As String, ByVal vValue As Object, Optional ByRef vQuiet As Object = Nothing)
        Try
            Try
                Dim vControls As Object
                Dim vArray(,) As Object
                Dim cControl, cTxtControl, cLblControl As Control
                Dim vID As Object
                Dim vWhere As String = ""
                Dim checkST As String = ""

                If Not (vQuiet Is Nothing) AndAlso vQuiet.Equals(Type.Missing) Then
                    vQuiet = gPMConstants.PMEReturnCode.PMFalse
                End If

                vControls = GetControlFromObjectAndAttributeName(sName, vQuiet)

                Dim cLookupControl As Control
                If Information.IsArray(vControls) Then

                    checkST = vControls(0).Name
                    If checkST.LastIndexOf("_") > 0 Then
                        checkST = checkST.Substring(1, checkST.LastIndexOf("_") - 1)
                    End If
                    Select Case checkST
                        Case "TabStrip1"
                            TabSetCaption(TabStrip1, CInt((vControls(1)) - 1), vValue)
                        Case "chkYesNo"

                            If IsNumeric(vValue) Then
                                'In case of a tri-state checkbox the values need to be manipulated for handling value-toggling by
                                'simulateTriStateCheckBox function called through chkYesNo_CheckStateChanged event
                                If ToSafeInteger(vValue) > 2 Then
                                    vValue = 2
                                ElseIf ToSafeInteger(vValue) < 0 Then
                                    vValue = 0
                                End If
                                Select Case vValue
                                    Case 0
                                        g_iCheckBoxValue = 0
                                    Case 1
                                        g_iCheckBoxValue = 1
                                    Case 2
                                        g_iCheckBoxValue = 2
                                End Select
                                CType(vControls(0), CheckBox).CheckState = vValue
                            End If
                        Case "cboList", "cboAccumulation"

                            If IsNumeric(vValue) Then
                                If vControls(0).ListIndex <> vValue Then

                                    'ReflectionHelper.SetMember(vControls(0), "ListIndex", (vValue))
                                    vControls(0).ListIndex = vValue
                                End If
                            ElseIf (vValue) <> "" And vValue <> vControls(0).Text Then
                                SetGISListValue((vValue), vControls(0))
                            End If
                        Case "cboPMLookup", "cboGISLookup"
                            '
                            '   RVH 31/10/2003 - CQ2598 : Need ability to refine lists based on
                            '                             other lists or business rules.
                            '                             Allow "WHERE" clause to be passed which
                            '                             will be used to refine list...
                            '
                            If Information.IsArray(vValue) Then
                                If (vValue).GetUpperBound(0) = 1 Then
                                    vID = (vValue(0))
                                    vWhere = (vValue(1))
                                End If
                            Else
                                vID = vValue
                            End If


                            'If ReflectionHelper.GetMember(vControls(0), "Name") = "cboPMLookup" Then
                            If vControls(0).Name.Contains("cboPMLookup") Then
                                If vWhere.Trim().Length > 0 Then
                                    'ReflectionHelper.SetMember(vControls(0), "WhereClause", vWhere)
                                    vControls(0).WhereClause = vWhere
                                    ReflectionHelper.Invoke(vControls(0), "RefreshList", New Object() {})
                                End If
                            End If


                            If IsNumeric(vID) Then
                                If ReflectionHelper.GetMember(vControls(0), "ItemId") <> vID Then
                                    ReflectionHelper.SetMember(vControls(0), "ItemId", (vID))
                                End If
                            ElseIf (vID) <> "" And vID <> vControls(0).ItemCaption Then
                                cLookupControl = vControls(0)

                                If vControls(0).Name.Contains("cboPMLookup") Then
                                    SetPMLValue((vID), cLookupControl)
                                Else
                                    SetGISLookupValue((vID), cLookupControl)
                                End If
                            End If
                        Case "lblText", "fraFrame", "lblFormattedText"
                            If ReflectionHelper.GetMember(vControls(0), "Tag") = ndcHyperlink Then
                                ReflectionHelper.SetMember(vControls(1), "Text", (vValue))
                            ElseIf vControls(1).GetType().Name.ToUpper() = "UCTCLMRESERVE" Then
                                ReflectionHelper.Invoke(vControls(1), "SaveScriptArrayToReserve", New Object() {vValue})
                            ElseIf vControls(1).GetType().Name.ToUpper() = "UCTCLMPAYMENT1" Then
                                ReflectionHelper.Invoke(vControls(1), "SaveScriptArrayToPayment", New Object() {vValue})
                            Else
                                ReflectionHelper.SetMember(vControls(0), "Text", (vValue))
                            End If

                        Case "txtText", "txtMlText", "txtFormattedText"

                            If vControls(0).Text <> Convert.ToString(vValue) Then
                                ReflectionHelper.SetMember(vControls(0), "Text", Convert.ToString(vValue))
                                cTxtControl = vControls(0)
                                cLblControl = vControls(1)
                                If checkST = "txtText" Then
                                    genericText_LostFocus(cLblControl, cTxtControl, GetIndex(cTxtControl, txtText))
                                Else
                                    genericText_LostFocus(cLblControl, cTxtControl, -1)
                                End If
                            End If

                        Case "pnlPartyPanel", "pnlPolicyPanel"
                            '   vValue must be an array...
                            If Information.IsArray(vValue) Then
                                ' vValue must have 3 elements (0,1,2)
                                If (vValue).GetUpperBound(0) = 2 Then
                                    vArray = Array.CreateInstance(GetType(Object), New Integer() {3, 1}, New Integer() {0, 0})

                                    vArray(0, 0) = (vValue(0))
                                    vArray(1, 0) = (vValue(1))
                                    vArray(2, 0) = (vValue(2))

                                    '   put the name in the pnlPartyPanel...

                                    ReflectionHelper.SetMember(vControls(0), "Text", (vValue(1)))

                                    m_vScreenValues(ReflectionHelper.GetMember(vControls(0), "Tag")) = vArray
                                End If
                            End If
                        Case Else
                            '                vControls(0).Text = vValue          ' RAW 14/10/2003 : CQ2754 : replaced
                            cControl = vControls(0)
                            m_oFormFields.FormatControlArray(cControl, vValue)
                    End Select
                End If


                Exit Sub

            Catch excep As System.Exception



                ' Log Error Message
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetValue Failed for " & sName & " value=" & (vValue), vApp:=ACApp, vClass:=ACClass, vMethod:="SetValue", excep:=excep)

                Exit Sub



            End Try
        Finally
            vQuiet = vQuiet
        End Try
    End Sub

    'Public Sub SetValue(ByVal sName As String, ByVal vValue As Object)
    '    Dim tempRefParam As Object = Type.Missing
    '    SetValue(sName, vValue, tempRefParam)

    ' ***************************************************************** '
    '
    ' Name: SetFocus
    '
    ' Description: Sets the focus to a control given the object.attribute name as a string
    '
    ' History: 01/02/2002 CLG - Created.
    '          12/09/2003 CLG CQ2001 Wrong Tab Displayed On Opening Risk
    ' ***************************************************************** '
    Public Function SetFocus(ByVal sName As String) As Integer

        Dim result As Integer = 0
        Try
            Dim vControls As Object
            Dim iTabNumber As Integer
            Dim sControlName As String = ""
            vControls = GetControlFromObjectAndAttributeName(sName)
            iTabNumber = -1

            If Information.IsArray(vControls) Then
                Select Case ReflectionHelper.GetMember(vControls(0), "Name").Substring(1, Math.Min(ReflectionHelper.GetMember(vControls(0), "Name").Length, 3)).ToLower()
                    Case "tab"

                    Case "fra"
                        ' set focus to the tab held against this frame

                        iTabNumber = ReflectionHelper.GetMember(vControls(0), "Tag")

                    Case Else
                        ' set focus to the tab held against this controls container (frame)


                        If Not (ReflectionHelper.GetMember(vControls(0), "Container") Is Nothing) Then

                            iTabNumber = ReflectionHelper.GetMember(ReflectionHelper.GetMember(vControls(0), "Container"), "Tag")
                        End If

                End Select
            End If

            'Some tabs may be hidden (removed) so we need to point to the real tab number
            iTabNumber = GetRealTabNumber(iTabNumber)

            If iTabNumber > -1 Then
                If Not TabStrip1.TabPages(iTabNumber + 1).Focused Then
                    SelectTab(TabStrip1, iTabNumber + 1)
                    TabStrip1_Click(Nothing, Nothing)
                End If
            End If
            ' RAW 09/07/2004 : JIT : end


            ' now set focus to the control itself
            If Information.IsArray(vControls) Then
                sControlName = Convert.ToString(ReflectionHelper.GetMember(vControls(0), "Name"))
                If sControlName.LastIndexOf("_") > 0 Then
                    sControlName = sControlName.Substring(1, sControlName.LastIndexOf("_") - 1)
                End If
                Select Case sControlName
                    Case "fraFrame", "pnlPartyPanel", "pnlPolicyPanel"


                        If ReflectionHelper.GetMember(vControls(1), "Enabled") And ReflectionHelper.GetMember(vControls(1), "Visible") Then


                            ReflectionHelper.Invoke(vControls(1), "Focus", New Object() {})
                        End If
                    Case "TabStrip1"
                        SelectTab(TabStrip1, CInt((vControls(1)) - 1))
                        TabStrip1_Click(Nothing, Nothing)
                    Case "lblText"
                    Case Else


                        If ReflectionHelper.GetMember(vControls(0), "Enabled") And ReflectionHelper.GetMember(vControls(0), "Visible") Then


                            ReflectionHelper.Invoke(vControls(0), "Focus", New Object() {})
                        End If
                End Select
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' RAW 09/07/2004 : JIT : added
            ' if SetFocus fails during loading because the control is not visible then store its details
            ' and try again once it has been fully loaded.
            ' I know that this will probably mean that the DL event fires out of sequence but that is better than nothing
            If m_bIsLoading Then
                m_sSetFocusControlName = sName
                Return result
            End If
            ' RAW 09/07/2004 : JIT : end

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "SetFocus Failed for " & sName, ACApp, ACClass, "SetFocus", Information.Err().Number, excep.Message, excep:=excep)
            Return result


            Return result
        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: SetMandatory
    '
    ' Description: Sets the mandatory status of a control given the object.attribute name as a string
    '
    ' History: 01/02/2002 CLG - Created.
    ' ***************************************************************** '
    Public Function SetMandatory(ByVal sName As String, ByVal vMode As Boolean, Optional ByVal vOverride As Object = Nothing) As Integer
        Dim result As Integer = 0



        Dim vControls As Object
        Dim sTemp As String = ""
        Dim sControlName As String = ""
        If vOverride Is Nothing Then
            vOverride = False
        End If

        vControls = GetControlFromObjectAndAttributeName(sName)

        Dim bNonBlankText As Boolean
        Try


            If Information.IsArray(vControls) Then
                sControlName = Convert.ToString(vControls(0).Name)
                If sControlName.LastIndexOf("_") > 0 Then
                    sControlName = sControlName.Substring(1, sControlName.LastIndexOf("_") - 1)
                End If

                Select Case sControlName
                    Case "lblText"
                    Case "chkYesNo"


                        If vMode And (vControls(0).Checked = ToSafeBoolean(0) Or vOverride) Then
                            If ColorTranslator.ToOle(Color.FromArgb(Math.Abs(vControls(0).BackColor.R), Math.Abs(vControls(0).BackColor.G), Math.Abs(vControls(0).BackColor.B))) <> m_lMandatoryColor Then
                                vControls(0).BackColor = ColorTranslator.FromOle(m_lMandatoryColor)
                            End If
                            vMode = True  'ensure vMode reflects actual status so that value added to collection is correct
                        Else

                            If ColorTranslator.ToOle(Color.FromArgb(Math.Abs(vControls(0).BackColor.R), Math.Abs(vControls(0).BackColor.G), Math.Abs(vControls(0).BackColor.B))) <> ColorTranslator.ToOle(fraFrame(0).BackColor) Then
                                'ReflectionHelper.SetMember(vControls(0), "BackColor", ColorTranslator.ToOle(fraFrame(0).BackColor))
                                vControls(0).BackColor = fraFrame(0).BackColor
                            End If
                            vMode = False  'ensure vMode reflects actual status so that value added to collection is correct
                        End If
                    Case "TabStrip1"
                        'ignore
                    Case "fraFrame"
                        'find all controls "contained" by this one
                        Dim arrControl As ArrayList = New ArrayList()
                        Dim ctlFormControl As Control
                        ControlList(Me, arrControl)
                        'For Each ctlFormControl As Control In Controls_Renamed
                        For lTemp As Integer = 0 To arrControl.Count - 1
                            ctlFormControl = arrControl(lTemp)
                            If Not ctlFormControl.Parent.Equals(vControls(0)) Then
                                'no match or no container error
                            Else
                                If ctlFormControl.Name.Substring(1, Math.Min(ctlFormControl.Name.Length, 3)) = "lvw" Then
                                    If vMode And (ReflectionHelper.GetMember(ReflectionHelper.GetMember(ctlFormControl, "Items"), "Count") = 0 Or vOverride) Then
                                        If ColorTranslator.ToOle(ctlFormControl.BackColor) <> m_lMandatoryColor Then ctlFormControl.BackColor = ColorTranslator.FromOle(m_lMandatoryColor)
                                        vMode = True  'ensure vMode reflects actual status so that value added to collection is correct
                                    Else
                                        If ColorTranslator.ToOle(ctlFormControl.BackColor) <> ColorTranslator.ToOle(SystemColors.Window) Then ctlFormControl.BackColor = SystemColors.Window
                                        vMode = False  'ensure vMode reflects actual status so that value added to collection is correct
                                    End If
                                    Return result
                                End If
                                If (vControls(1)) = "address_cnt" And ctlFormControl.Name.Substring(1, Math.Min(ctlFormControl.Name.Length, 3)) = "txt" Then
                                    If Not (vMode And (ctlFormControl.ToString() = "" Or vOverride)) Then
                                        bNonBlankText = True
                                    End If
                                End If
                            End If
                        Next  'ctlFormControl
                        'for address fields all we have done so far is to detect if any fields are non blank
                        'now, using that flag, we need to revisit all of the controls to set their status
                        If (vControls(1)) = "address_cnt" Then
                            'For Each ctlFormControl As Control In Controls_Renamed
                            For lTemp As Integer = 0 To arrControl.Count - 1
                                ctlFormControl = arrControl(lTemp)
                                'If ctlFormControl.Parent <> vControls(0) Then
                                If Not ctlFormControl.Parent.Equals(vControls(0)) Then
                                    'no match or no container error
                                Else
                                    If ctlFormControl.Name.Substring(1, Math.Min(ctlFormControl.Name.Length, 3)) = "txt" Then
                                        If bNonBlankText Or Not vMode Then
                                            If ColorTranslator.ToOle(ctlFormControl.BackColor) <> ColorTranslator.ToOle(SystemColors.Window) Then ctlFormControl.BackColor = SystemColors.Window
                                            vMode = True  'ensure vMode reflects actual status so that value added to collection is correct
                                        Else
                                            If ColorTranslator.ToOle(ctlFormControl.BackColor) <> m_lMandatoryColor Then ctlFormControl.BackColor = ColorTranslator.FromOle(m_lMandatoryColor)
                                            vMode = False  'ensure vMode reflects actual status so that value added to collection is correct
                                        End If
                                    End If
                                End If
                            Next  'ctlFormControl
                        End If
                    Case "cboList"
                        If vMode And ((ReflectionHelper.GetMember(vControls(0), "Text") = "" Or ReflectionHelper.GetMember(vControls(0), "Text") = "(None)") Or vOverride) Then
                            If ColorTranslator.ToOle(Color.FromArgb(Math.Abs(vControls(0).BackColor.R), Math.Abs(vControls(0).BackColor.G), Math.Abs(vControls(0).BackColor.B))) <> m_lMandatoryColor Then
                                vControls(0).BackColor = ColorTranslator.FromOle(m_lMandatoryColor)
                            End If
                            vMode = True  'ensure vMode reflects actual status so that value added to collection is correct
                        Else
                            If ColorTranslator.ToOle(Color.FromArgb(Math.Abs(vControls(0).BackColor.R), Math.Abs(vControls(0).BackColor.G), Math.Abs(vControls(0).BackColor.B))) <> ColorTranslator.ToOle(SystemColors.Window) Then
                                vControls(0).BackColor = SystemColors.Window
                            End If
                            vMode = False  'ensure vMode reflects actual status so that value added to collection is correct
                        End If
                    Case "cboGISLookup", "cboPMLookup"

                        'Or ReflectionHelper.GetMember(vControls(0), "ItemCaption") = "(None)")
                        If vMode AndAlso ((Trim(vControls(0).ItemCaption) = "" OrElse Trim(vControls(0).ItemCaption).ToUpper = "(NONE)") Or vOverride) Then
                            If ColorTranslator.ToOle(Color.FromArgb(Math.Abs(vControls(0).BackColor.R), Math.Abs(vControls(0).BackColor.G), Math.Abs(vControls(0).BackColor.B))) <> m_lMandatoryColor Then
                                vControls(0).BackColor = ColorTranslator.FromOle(m_lMandatoryColor)
                            End If
                            vMode = True  'ensure vMode reflects actual status so that value added to collection is correct
                        Else
                            If ColorTranslator.ToOle(Color.FromArgb(Math.Abs(vControls(0).BackColor.R), Math.Abs(vControls(0).BackColor.G), Math.Abs(vControls(0).BackColor.B))) <> ColorTranslator.ToOle(SystemColors.Window) Then
                                vControls(0).BackColor = SystemColors.Window

                            End If
                            vMode = False  'ensure vMode reflects actual status so that value added to collection is correct
                        End If
                        ' RVH - CQ1887 22/7/03 : Start
                        '                        Allow user to set mandatory
                        ' RAW 14/01/2004 : CQ3720 : added pnlPolicyPanel
                    Case "pnlPartyPanel", "pnlPolicyPanel"

                        If vMode And (ReflectionHelper.GetMember(vControls(0), "Text") = "" Or vOverride) Then
                            If ColorTranslator.ToOle(Color.FromArgb(Math.Abs(vControls(0).BackColor.R), Math.Abs(vControls(0).BackColor.G), Math.Abs(vControls(0).BackColor.B))) <> m_lMandatoryColor Then
                                vControls(0).BackColor = ColorTranslator.FromOle(m_lMandatoryColor)
                            End If
                            vMode = True  'ensure vMode reflects actual status so that value added to collection is correct
                        Else
                            If ColorTranslator.ToOle(Color.FromArgb(Math.Abs(vControls(0).BackColor.R), Math.Abs(vControls(0).BackColor.G), Math.Abs(vControls(0).BackColor.B))) <> ColorTranslator.ToOle(SystemColors.Control) Then
                                vControls(0).BackColor = SystemColors.Control
                            End If
                            vMode = False  'ensure vMode reflects actual status so that value added to collection is correct
                        End If
                        ' RVH - CQ1887 22/7/03 : End
                        ' PW010803 - CQ2043 - Accumulation: Start
                    Case "cboAccumulation"

                        If vMode And (ReflectionHelper.GetMember(vControls(0), "ListIndex") = -1 Or vOverride) Then
                            If ColorTranslator.ToOle(Color.FromArgb(Math.Abs(vControls(0).BackColor.R), Math.Abs(vControls(0).BackColor.G), Math.Abs(vControls(0).BackColor.B))) <> m_lMandatoryColor Then
                                vControls(0).BackColor = ColorTranslator.FromOle(m_lMandatoryColor)
                            End If
                            vMode = True  'ensure vMode reflects actual status so that value added to collection is correct
                        Else
                            If ColorTranslator.ToOle(Color.FromArgb(Math.Abs(vControls(0).BackColor.R), Math.Abs(vControls(0).BackColor.G), Math.Abs(vControls(0).BackColor.B))) <> ColorTranslator.ToOle(SystemColors.Window) Then
                                vControls(0).BackColor = SystemColors.Window
                            End If
                            vMode = False  'ensure vMode reflects actual status so that value added to collection is correct
                        End If
                        ' PW010803 - CQ2043: End
                    Case "PBForm"
                        bAllowSaveOfInvalidMandatoryData = Not vMode
                    Case Else
                        If vMode And (ReflectionHelper.GetMember(vControls(0), "Text") = "" Or vOverride) Then
                            'If ReflectionHelper.GetMember(vControls(0), "BackColor") <> m_lMandatoryColor Then
                            'If vControls(0).BackColor <> m_lMandatoryColor Then
                            If ColorTranslator.ToOle(Color.FromArgb(Math.Abs(vControls(0).BackColor.R), Math.Abs(vControls(0).BackColor.G), Math.Abs(vControls(0).BackColor.B))) <> m_lMandatoryColor Then
                                vControls(0).BackColor = ColorTranslator.FromOle(m_lMandatoryColor)
                            End If
                            vMode = True
                            'ensure vMode reflects actual status so that value added to collection is correct
                        Else
                            If ColorTranslator.ToOle(Color.FromArgb(Math.Abs(vControls(0).BackColor.R), Math.Abs(vControls(0).BackColor.G), Math.Abs(vControls(0).BackColor.B))) <> ColorTranslator.ToOle(SystemColors.Window) Then
                                vControls(0).BackColor = SystemColors.Window
                            End If
                            vMode = False  'ensure vMode reflects actual status so that value added to collection is correct
                        End If
                End Select
            End If

            ' RAW 09/07/2003 : CQ221 : add mandatory controls to mandatory collection or remove them if
            ' no longer mandatory (note - status can be set via dynamic logic)
        Catch ex As Exception

        End Try

        ' is this control already in the collection
        If m_cMandatoryProperties.Contains(sName) = False Then
            If vMode = True Then
                ' is now mandatory
                ' add if not already in collection
                m_cMandatoryProperties.Add(sName, sName.ToUpper())
            End If
        Else
            If vMode = False Then
                ' is now NOT mandatory
                ' remove if  already in collection
                m_cMandatoryProperties.Remove(sName.ToUpper())
            End If
        End If

        Return result

    End Function

    'Public Function SetMandatory(ByVal sName As String, ByVal vMode As Boolean) As Integer
    '    Return SetMandatory(sName, vMode, Nothing)



    ' ***************************************************************** '
    '
    ' Name: SetVisibility
    '
    ' Description: Sets the visibility of a control given the object.attribute name as a string
    '
    ' History: 01/02/2002 CLG - Created.
    '          12/09/2003 CLG CQ2001 Wrong Tab Displayed On Opening Risk
    ' ***************************************************************** '
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sName"></param>
    ''' <param name="vMode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetVisibility(ByVal sName As String, ByVal vMode As Boolean) As Integer

        Dim result As Integer = 0
        Dim ControlName As String = ""
        Try

            Dim vControls As Object
            Dim iTabIndex As Integer
            Dim sTabName As String

            sTabName = sName.ToUpper

            If sTabName.Contains("TAB.") Then
                sTabName = sName.Substring(0, sName.IndexOf(".") + 1)
                iTabIndex = ToSafeInteger(sName.Substring(sName.IndexOf(".") + 1)) - 1
                sName = sTabName & iTabIndex.ToString()
            End If

            vControls = GetControlFromObjectAndAttributeName(sName)

            If Information.IsArray(vControls) Then
                'ControlName = Convert.ToString(ReflectionHelper.GetMember(vControls(0), "Name"))
                ControlName = Convert.ToString(vControls(0).Name)
                If ControlName.LastIndexOf("_") > 0 Then
                    ControlName = ControlName.Substring(1, ControlName.LastIndexOf("_") - 1)
                End If

                Select Case ControlName
                    Case "TabStrip1"
                        If vMode Then
                            AddTab(TabStrip1, CInt(vControls(1)), vControls(2))

                            'Mark the tab as showing
                            m_vTabArray(ACFIsDeleted, CInt(vControls(1))) = gPMConstants.PMEReturnCode.PMFalse
                        Else
                            HideTab(TabStrip1, CInt(vControls(1)))

                            'Mark the tab as Deleted
                            m_vTabArray(ACFIsDeleted, CInt(vControls(1))) = gPMConstants.PMEReturnCode.PMTrue
                        End If
                    Case "fraFrame"
                        ' I know this looks odd but disabling the controls rather than hiding them completely is by
                        ' design and not a mistake and is intended to prevent large 'holes' appearing in forms when
                        ' a complete frame is hidden

                        'If ReflectionHelper.GetMember(vControls(0), "Enabled") <> vMode Then
                        If vControls(0).Enabled <> vMode Then

                            'ReflectionHelper.SetMember(vControls(0), "Enabled", vMode)
                            vControls(0).Enabled = vMode

                            'find all controls "contained" by this one
                            'On Error Resume Next
                            Try
                                For Each ctlFormControl As Control In Controls_Renamed
                                    Try
                                        If GetIndex(ctlFormControl.Parent, fraFrame) <> vControls(0).Index Then
                                        Else
                                            ControlHelper.SetEnabled(ctlFormControl, vMode)
                                        End If
                                        Information.Err().Clear()
                                    Catch
                                    End Try
                                Next ctlFormControl
                            Catch
                            End Try
                        End If
                    ' RVH - CQ1887 22/7/03 : Start
                    '                        Allow the user to set the visibility of the find party control
                    ' RAW 14/01/2004 : CQ3720 : added pnlPolicyPanel
                    Case "pnlPartyPanel", "pnlPolicyPanel"

                        If vControls(0).Visible <> vMode Then

                            'ReflectionHelper.SetMember(vControls(0), "Visible", vMode)
                            'ReflectionHelper.SetMember(vControls(1), "Visible", vMode)
                            vControls(0).Visible = vMode
                            vControls(1).Visible = vMode

                        End If


                    ' RVH - CQ1887 22/7/03 : End

                    ' RAW 09/07/2004 : JIT : added
                    Case "lblText"


                        If vControls(0).Visible <> vMode Then

                            If vControls(0).Text <> ACBlankCaption Then 'PN24240

                                vControls(0).Visible = vMode
                            End If
                        End If

                    Case "txtText"
                        'check control's visibility before setting it as it is much quicker
                        'cboGISLookup aren't initially visible(?) so check caption not control!

                        If vControls(1).Text <> ACBlankCaption Then

                            vControls(1).Visible = vMode
                            vControls(0).Visible = vMode

                            'End If
                        Else
                            'If caption is [Blank] then the label will always be invisible so don't check on it.
                            'but we have to set the visibility of the control regardless of its current state

                            'ReflectionHelper.SetMember(vControls(0), "Visible", vMode)
                            vControls(0).Visible = vMode
                        End If

                    Case Else
            'check control's visibility before setting it as it is much quicker
            'cboGISLookup aren't initially visible(?) so check caption not control!

            If vControls(1).Text <> ACBlankCaption Then
                'ReflectionHelper.SetMember(vControls(1), "Visible", vMode)
                'ReflectionHelper.SetMember(vControls(0), "Visible", vMode)

                vControls(1).Visible = vMode
                vControls(0).Visible = vMode

            Else
                'If caption is [Blank] then the label will always be invisible so don't check on it.
                If vControls(0).Visible <> vMode Then

                    'ReflectionHelper.SetMember(vControls(0), "Visible", vMode)
                    vControls(0).Visible = vMode
                End If
            End If
            End Select

            End If

            Return result

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "SetVisibility Failed for " & sName, ACApp, ACClass, "SetVisibility", Information.Err().Number, Information.Err().Description, excep:=ex)

            Return result
            'Resume
            'Return result
        End Try
    End Function


    Private Function GetRealTabNumber(ByVal iOriginalTabNumber As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetRealTabNumber
        ' PURPOSE: Returns the Real Tab Number when passed the Original Tab Number
        ' This is required because the Risk screen uses the Tab Strip Control which
        ' does not support tab hiding - tabs are removed. Therefore, if dynamic logic
        ' is used to hide a tab, the tag properties which point to the tab a control
        ' was originally created on is incorrect. This function adjusts the number
        ' accordingly.

        ' AUTHOR: Danny Davis
        ' DATE: 24 March 2006, 12:30:31
        ' RETURNS: Real Tab Number or -1 if it's hidden
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim iCorrection As Integer


        Try

            'This is the number of tabs which have been removed prior to the original tab
            iCorrection = 0

            For iTab As Integer = m_vTabArray.GetLowerBound(1) To iOriginalTabNumber
                If m_vTabArray(ACFIsDeleted, iTab) = gPMConstants.PMEReturnCode.PMTrue Then
                    iCorrection += 1
                End If
            Next iTab

            result = iOriginalTabNumber - iCorrection


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "", ACApp, ACClass, "GetRealTabNumber", Information.Err().Number, Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

                    Return result
            End Select

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetControlFromTag
    '
    ' Description: Gets the control given the object.attribute name as a string
    '
    ' History:
    ' RAW 09/07/2004 : JIT : added
    ' ***************************************************************** '
    Private Function GetControlFromTag(ByVal v_vObjIdx As Object, Optional ByVal v_vPropIdx As Object = Nothing, Optional ByRef v_vQuiet As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse, Optional ByRef v_bCreateControl As Boolean = True) As Object
        Dim result As Object = Nothing
        Dim lDebugDepthCounter As Integer
        Dim sObjectAndPropertyName As String = ""

        ' Debug message
#If (DebugOption And m_klDebugOption_Normal) = m_klDebugOption_Normal Then
		AddToDebug(lDebugDepthCounter, "Entering " & ACApp & "." & ACClass & ".GetControlFromTag")
#End If

        Try
            result = Nothing


            sObjectAndPropertyName = ObjectAndPropertyName(v_vObjIdx, v_vPropIdx)

            If sObjectAndPropertyName <> "" Then

                result = GetControlFromObjectAndAttributeName(sObjectAndPropertyName, v_vQuiet, v_bCreateControl)
            End If

            ' Debug message
#If (DebugOption And m_klDebugOption_Normal) = m_klDebugOption_Normal Then
		AddToDebug(lDebugDepthCounter, "Exiting " & ACApp & "." & ACClass & ".GetControlFromTag")
#End If


        Catch ex As Exception

            If v_vQuiet = gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' Debug message
#If (DebugOption And m_klDebugOption_Normal) = m_klDebugOption_Normal Then
		AddToDebug(lDebugDepthCounter, "Errored in " & ACApp & "." & ACClass & ".GetControlFromTag")
#End If
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "GetControlFromTag Failed", ACApp, ACClass, "GetControlFromTag", Information.Err().Number, Information.Err().Description, excep:=ex)



        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If


        End Try
        Return result
    End Function


    ' ***************************************************************** '
    '
    ' Name: GetControlFromObjectAndAttributeName
    '
    ' Description: Gets the control given the object.attribute name as a string
    '
    ' History: 01/02/2002 CLG - Created.
    ' RAW 09/07/2004 : JIT : added v_bCreateControl param
    ' ***************************************************************** '
    'Private Function GetControlFromObjectAndAttributeName(ByVal sName As String, Optional ByRef vQuiet As Boolean = gPMConstants.PMEReturnCode.PMFalse, Optional ByRef v_bCreateControl As Boolean = True) As Object()

    Private Function GetControlFromObjectAndAttributeName(ByVal sName As String, Optional ByRef vQuiet As Boolean = gPMConstants.PMEReturnCode.PMFalse, Optional ByRef v_bCreateControl As Boolean = True) As Object
        Dim result As Object = Nothing

        Dim lTemp As Integer
        Dim sObjectName, sPropertyName As String
        Dim lTag As Integer
        Dim lObjectId As Integer
        Dim lScreenDetailIndex As Integer

        Try
            sName = sName.ToLower()

            If m_cObjectAndAttribute.Contains(sName) Then
                ' control has already been created and has been found

                Return m_cObjectAndAttribute(sName)
            End If

            ' control has not yet been created
            If v_bCreateControl Then
                ' create the control if we can

                sObjectName = sName
                If sName.Contains(".") Then
                    Dim def As System.Array = sName.Split(".")
                    If def.GetUpperBound(0) = 1 Then
                        sObjectName = def(0)
                        sPropertyName = def(1)
                    End If
                End If

                ' RVH 09/12/2004 - Moved from 1.8.6
                If sObjectName.ToLower() = "pbform" And sPropertyName = "" Then

                    Dim aDictionaryControl As Object

                    aDictionaryControl = New Object() {PBForm, "PBForm"}
                    result = (aDictionaryControl)
                    m_cObjectAndAttribute.Add(aDictionaryControl, sName)
                    Return result
                End If

                ' Is the objact/property name valid
                lTag = -1
                If sObjectName.ToLower() = "label" Then
                    ' not in dictionary
                    Dim uBnd As Integer = m_vScreenDetails.GetUpperBound(1)
                    For lTemp = 0 To uBnd
                        Select Case m_vScreenDetails(ACDGISPropertyId, lTemp)
                            Case ndcFreeFormatText, ndcHyperlink
                                If ObjectAndPropertyName(m_vScreenDetails(ACDGISPropertyId, lTemp), lTemp).ToLower() = (sObjectName & "." & sPropertyName).ToLower() Then
                                    lTag = lTemp
                                    Exit For
                                End If
                        End Select
                    Next

                    If lTag <> -1 Then
                        lScreenDetailIndex = lTemp
                    End If
                Else
                    'walk the data dictionary and find the object/attribute name combination

                    'For lTemp = CObj(m_vDataDictionary(GISDMTypeRisk)).GetLowerBound(1) To CObj(m_vDataDictionary(GISDMTypeRisk)).GetUpperBound(1)
                    Dim uBnd As Integer = m_vDataDictionary(GISDMTypeRisk).GetUpperBound(1)
                    Dim myobj(,) As Object = m_vDataDictionary(GISDMTypeRisk)

                    For lTemp = 0 To uBnd
                        'For Each myObj As Object In m_vDataDictionary(GISDMTypeRisk)
                        If myobj(MainModule.ACOObjectName, lTemp).ToString.ToLower() = sObjectName Then

                            lObjectId = Convert.ToInt32(myobj(ACO2GISObjectId, lTemp))
                            ' RAW 09/07/2004 : JIT : added test for property name = ""
                            If sPropertyName = "" Then
                                ' searching for a control linked to just an object (eg List View)
                                lTag = lTemp
                                Exit For
                            Else
                                ' searching for a property

                                If myobj(MainModule.ACPPropertyName, lTemp).ToString.ToLower() = sPropertyName Then
                                    lTag = lTemp
                                    Exit For
                                End If
                            End If

                        End If
                    Next

                    If lTag <> -1 Then
                        lScreenDetailIndex = Convert.ToInt32(myobj(MainModule.ACPIsIdentifyingProperty, lTag))
                    End If

                End If


                If lScreenDetailIndex <> 0 Then

                    ' This is a valid reference to a control - create the control now
                    AddControlToForm(lScreenDetailIndex)

                    'it is possible that the control has been added to the collection during DL so check again

                    If Not (m_cObjectAndAttribute Is Nothing) Then
                        If m_cObjectAndAttribute.Contains(sName) Then
                            Return (m_cObjectAndAttribute(sName))
                        End If
                    End If
                End If
            End If

            result = Nothing  ' did not find it
            If vQuiet = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Could not find attribute: " & sName, ACApp, ACClass, "GetControlFromObjectAndAttributeName")
            End If

            Return result

        Catch ex As Exception

            ' Debug message
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "GetControlFromObjectAndAttributeName Failed (" & sName & ")", ACApp, ACClass, "GetControlFromObjectAndAttributeName", Information.Err().Number, Information.Err().Description, excep:=ex)
            Return result
        Finally

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: RunDynamicLogic
    '
    ' Description:
    '
    ' History: 05/02/2002 CLG - Created.
    ' RAW 21/05/2004 : Performance Changes : added params
    ' ***************************************************************** '
    Public Sub RunDynamicLogic(Optional ByVal v_eDLProcedureName As enumDLProcedureType = enumDLProcedureType.eDLProcedureName_Start, Optional ByVal v_vDLAction As Object = Nothing, Optional ByVal v_vObjIdx As Object = Nothing, Optional ByVal v_vPropIdx As Object = Nothing, Optional ByVal v_vControl As Object = Nothing, Optional ByVal v_vTabNumber As Object = Nothing)

        Dim vTabNumber As Integer
        Dim bMethodNotSuported As Boolean

        ' tab number is 1-based
        Try

            ' RAW 21/05/2004 : Performance Changes : rewritten to make all calls to Script control methods through a common function
            ' RAW 20/08/2004 : JIT : replaced m_bSuppressDynamicLogic with IsDynamicLogicSuppressed
            If IsDynamicLogicSuppressed Then
                ' If IsDynamicLogicSuppressed Then
                Exit Sub
            End If

            ' set defaults
            If Not (v_vDLAction Is Nothing) AndAlso v_vDLAction.Equals(Type.Missing) Then
                If v_eDLProcedureName = enumDLProcedureType.eDLProcedureName_OnChange Then
                    v_vDLAction = enumOnChangeAction.eOnChangeAction_Change
                End If
            End If

            ' do we call the old or new procedure for OnLostFocus            
            If m_bFireDLStartEvents Then
                ' Still using the old procedure calls
                Select Case v_eDLProcedureName
                    Case enumDLProcedureType.eDLProcedureName_OnLostFocus
                        v_eDLProcedureName = enumDLProcedureType.eDLProcedureName_Start

                    Case enumDLProcedureType.eDLProcedureName_OnChange

                        Select Case v_vDLAction
                            Case enumOnChangeAction.eOnChangeAction_Refresh, enumOnChangeAction.eOnChangeAction_Change
                                v_eDLProcedureName = enumDLProcedureType.eDLProcedureName_Start
                            Case enumOnChangeAction.eOnChangeAction_Accept
                                v_eDLProcedureName = enumDLProcedureType.eDLProcedureName_Start
                                v_vTabNumber = -1
                        End Select

                    Case enumDLProcedureType.eDLProcedureName_OnFocus
                        'onFocus events were not available in 1.8.6
                        Exit Sub
                    Case Else
                End Select
            End If

            ' now call the appropriate procedure
            Select Case v_eDLProcedureName
                Case enumDLProcedureType.eDLProcedureName_OnFocus

                    CallScriptOnFocus(v_eOnFocusAction:=v_vDLAction, v_vObjIdx:=v_vObjIdx, v_vPropIdx:=v_vPropIdx, v_vControl:=v_vControl, v_vTabNumber:=v_vTabNumber)
                Case enumDLProcedureType.eDLProcedureName_OnLostFocus

                    CallScriptOnFocus(v_eOnFocusAction:=v_vDLAction, v_vObjIdx:=v_vObjIdx, v_vPropIdx:=v_vPropIdx, v_vControl:=v_vControl, v_vTabNumber:=v_vTabNumber, v_bUseLostFocus:=True)
                Case enumDLProcedureType.eDLProcedureName_OnChange

                    CallScriptOnChange(v_eOnChangeAction:=v_vDLAction, v_vObjIdx:=v_vObjIdx, v_vPropIdx:=v_vPropIdx, v_vControl:=v_vControl, v_vTabNumber:=v_vTabNumber)
                Case enumDLProcedureType.eDLProcedureName_Initialise

                    CallScriptMethod(sMethod:="Initialise", r_sObjectName:="", r_sAction:="", r_bMethodNotSuported:=bMethodNotSuported, iTabNumber:=-1)
                Case Else
                    ' get the tab number
                    If Not IsNothing(v_vTabNumber) Then
                        ' use the param value passed in
                        vTabNumber = v_vTabNumber
                    Else
                        If Not Not (v_vControl Is Nothing) AndAlso v_vControl.Equals(Type.Missing) Then
                            vTabNumber = TabNumberForControl(v_vControl)
                            If vTabNumber >= 0 Then vTabNumber += 1
                        End If
                    End If

                    CallScriptMethod(sMethod:="Start", r_sObjectName:="", r_sAction:="", r_bMethodNotSuported:=bMethodNotSuported, iTabNumber:=vTabNumber)
            End Select


        Catch ex As Exception

            ' RAW 21/05/2004 : Performance Changes : replaced used of m_oMSScriptControl.Error with m_oMSScriptControl_Error event
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "RunDynamicLogic Failed", ACApp, ACClass, "RunDynamicLogic", Information.Err().Number, Information.Err().Description, excep:=ex)
            Exit Sub
        End Try

    End Sub


    Public Function DelObjectInstance(ByVal v_sObjectName As String, ByVal v_sOIKey As String) As Integer

        Dim result As Integer = 0
        Try

            'Return ReflectionHelper.Invoke(m_oInterface, "DelObjectInstance", New Object() {v_sObjectName, v_sOIKey})
            m_oInterface.DelObjectInstance(v_sObjectName, v_sOIKey)
        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DelObjectInstance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DelObjectInstance", excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: AddClaimReserve
    '
    ' Description:
    '
    ' History: 16/07/2002 CLG - Created.
    ' RAW 09/07/2004 : JIT : added v_lScreenDetailsIndex param and used instead of m_cControlCount
    ' ***************************************************************** '
    Private Function AddClaimReserve(ByRef iFrameIndex As Integer, ByRef lTag As Integer, ByRef lX As Single, ByRef lY As Single, ByRef bAddFrame As Boolean, ByRef sPropertyName As String, ByRef vTabSetIndex As Byte, ByVal v_lScreenDetailsIndex As Integer) As Integer

        Dim nResult As Integer = 0
        Dim lDebugDepthCounter As Integer
        Dim vArray As Object
        Dim oProduct As bSIRProduct.Business = Nothing
        Dim oIsReservesReadonly As Object = Nothing
        Try

            ' RAW 09/07/2004 : JIT : added
#If (DebugOption And m_klDebugOption_JIT) = m_klDebugOption_JIT Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="JIT : Adding ClaimReserve - " & sPropertyName)
#End If

            nResult = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = g_oObjectManager.GetInstance(oProduct, "bSIRProduct.Business", PMGetViaClientManager)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to get Product Business  object", ACApp, ACClass, "AddClaimReserve", Information.Err().Number, Information.Err().Description)
                Return nResult
            End If

            If m_lProductId = 0 Then
                m_lReturn = oProduct.GetProductid(ifilecnt:=m_lInsuranceFileCnt, vProduct_id:=m_lProductId)
            End If

            m_lReturn = oProduct.GetProductValue(m_lProductId, "is_reserves_read_only", oIsReservesReadonly)

            uctCLMReserve.Initialise()

            If IsArray(oIsReservesReadonly) AndAlso oIsReservesReadonly(0, 0) = "1" Then
                uctCLMReserve.ShowEdit = False
                oIsReservesReadonly = Nothing
            End If

            uctCLMReserve.SetProcessModes(Task, , , m_sClaimTransactionType)
            uctCLMReserve.LoadControl()
            uctCLMReserve.GetDetails(m_lClaimPerilID, , m_lClaimID, m_lClaimRiskId, m_lClaimInsFileCnt)

            If bAddFrame Then
                m_lReturn = AddFrame(iFrameIndex, lTag, lX, lY, 10000)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = m_lReturn

                    Return nResult
                End If
            End If


            ' RAW 09/07/2004 : JIT : added
            ' store reference to this control in a collection - keyed on object and property name

            m_cObjectAndAttribute.Add(New Object() {fraFrame(iFrameIndex), uctCLMReserve, "", v_lScreenDetailsIndex}, ObjectAndPropertyName(lTag).ToLower())

            m_oNewControlBeingLoaded = uctCLMReserve  ' RAW 20/08/2004 : JIT : added


            uctCLMReserve.Parent = fraFrame(iFrameIndex)
            m_uctCLMReserveTabIndex = vTabSetIndex

            uctCLMReserve.Top = VB6.TwipsToPixelsY(180)
            uctCLMReserve.Left = VB6.TwipsToPixelsX(60)
            uctCLMReserve.Height = fraFrame(iFrameIndex).Height - VB6.TwipsToPixelsY(270)
            uctCLMReserve.Width = fraFrame(iFrameIndex).Width - VB6.TwipsToPixelsX(120)
            uctCLMReserve.Visible = True

            If Task <> gPMConstants.PMEComponentAction.PMView Then
                If (m_vDataDictionary(GISDMTypeRisk)(ACPIsInputProperty, lTag Mod 10000)) = 1 Then
                    uctCLMReserve.Enabled = True
                End If
            End If

            ' MEvans : 12-11-2004 : CQ6977
            ' incorrect value being assigned to the tag,
            ' it should use the screen details index rather than the object id stored in "lTag"
            uctCLMReserve.Tag = CStr(v_lScreenDetailsIndex)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If


            ' now flag this in the array as having been created
            m_vScreenDetails(ACDScreenDetailCnt, v_lScreenDetailsIndex) = v_lScreenDetailsIndex  ' RAW 20/08/2004 : JIT : added
            m_oNewControlBeingLoaded = Nothing  ' RAW 20/08/2004 : JIT : added

            vArray = Nothing


        Catch ex As Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "AddClaimReserve Failed", ACApp, ACClass, "AddClaimReserve", Information.Err().Number, Information.Err().Description, excep:=ex)



        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If
            oProduct.Dispose()
            oProduct = Nothing



        End Try
        Return nResult
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddClaimPayment
    '
    ' Description:
    '
    ' History: 16/07/2002 CLG - Created.
    ' RAW 09/07/2004 : JIT : added v_lScreenDetailsIndex param and used instead of m_cControlCount
    ' ***************************************************************** '
    Private Function AddClaimPayment(ByRef iFrameIndex As Integer, ByRef lTag As Integer, ByRef lX As Single, ByRef lY As Single, ByRef bAddFrame As Boolean, ByRef sPropertyName As String, ByRef vTabSetIndex As Byte, ByVal v_lScreenDetailsIndex As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddClaimPayment"

        Dim lDebugDepthCounter As Integer
        Dim vArray As Object
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            ' RAW 09/07/2004 : JIT : added
#If (DebugOption And m_klDebugOption_JIT) = m_klDebugOption_JIT Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="JIT : Adding ClaimPayment - " & sPropertyName)
#End If

            result = gPMConstants.PMEReturnCode.PMTrue

            '**********************************************


            lReturn = uctCLMPayment.Initialise()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "uctCLMPayment.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            '**********************************************

            lReturn = uctCLMPayment.SetProcessModes(m_iTask, , , m_sTransactionType, m_dtEffectiveDate)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "uctCLMPayment.SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            '**********************************************

            uctCLMPayment.WorkClaimID = m_lClaimID
            uctCLMPayment.WorkClaimPerilId = m_lClaimPerilID
            uctCLMPayment.IsOpenClaimNoTrans = m_bNoTransactions

            '**********************************************

            lReturn = uctCLMPayment.Load_Renamed
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "uctCLMPayment.Load Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            '**********************************************


            'uctCLMPayment.Initialise
            'uctCLMPayment.LoadControl

            ' RVH 10/12/2004 - Merged from 1.8.6
            'uctCLMPayment.SetProcessModes vTask:=Task, vTransactionType:=m_sClaimTransactionType

            'uctCLMPayment.GetDetails lPerilID:=m_lClaimPerilID, _
            'lClaimID:=m_lClaimID, _
            'lRiskId:=m_lClaimRiskId, _
            'lInsurance_File_Cnt:=m_lClaimInsFileCnt


            If bAddFrame Then
                m_lReturn = AddFrame(iFrameIndex, lTag, lX, lY, 10000)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn

                    Return result
                End If
            End If


            ' RAW 09/07/2004 : JIT : added
            ' store reference to this control in a collection - keyed on object and property name

            m_cObjectAndAttribute.Add(New Object() {fraFrame(iFrameIndex), uctCLMPayment, "", v_lScreenDetailsIndex}, ObjectAndPropertyName(lTag).ToLower())

            m_oNewControlBeingLoaded = uctCLMPayment  ' RAW 20/08/2004 : JIT : added


            uctCLMPayment.Parent = fraFrame(iFrameIndex)
            m_uctCLMPaymentTabIndex = vTabSetIndex

            uctCLMPayment.Top = VB6.TwipsToPixelsY(180)
            uctCLMPayment.Left = VB6.TwipsToPixelsX(60)
            uctCLMPayment.Height = fraFrame(iFrameIndex).Height - VB6.TwipsToPixelsY(270)
            uctCLMPayment.Width = fraFrame(iFrameIndex).Width - VB6.TwipsToPixelsX(120)

            uctCLMPayment.Visible = True

            '    If (Task <> PMView) Then
            '        If (m_vDataDictionary(GISDMTypeRisk)(ACPIsInputProperty, lTag Mod 10000) = 1) Then
            '            uctCLMPayment.Enabled = True
            '        End If
            '    End If

            ' MEvans : 12-11-2004 : CQ6977
            ' incorrect value being assigned to the tag,
            ' it should use the screen details index rather than the object id stored in "lTag"
            uctCLMPayment.Tag = CStr(v_lScreenDetailsIndex)


            '    m_lReturn = SetExtraListViewProperties(v_hWndList:=uctCLMPayment.hwnd, _
            ''                                           v_vShowRowSelect:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            '    vArray = m_vScreenValues(m_lControlCount)

            '    If IsArray(vArray) Then
            '        For lTemp = LBound(vArray, 2) To UBound(vArray, 2)
            '            Set oListItem = uctCLMPayment.ListItems.Add(, , vArray(1, lTemp))
            '            oListItem.SubItems(1) = vArray(2, lTemp)
            '            oListItem.Tag = vArray(0, lTemp)
            '        Next lTemp
            '    End If


            ' now flag this in the array as having been created
            m_vScreenDetails(ACDScreenDetailCnt, v_lScreenDetailsIndex) = v_lScreenDetailsIndex  ' RAW 20/08/2004 : JIT : added
            m_oNewControlBeingLoaded = Nothing  ' RAW 20/08/2004 : JIT : added

            m_bClaimPaymentAdded = True

            vArray = Nothing


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "AddClaimPayment Failed", ACApp, ACClass, "AddClaimPayment", Information.Err().Number, Information.Err().Description, excep:=ex)



        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If


        End Try
        Return result
    End Function


    Private Function UpdatePaymentAndReserve() As Integer
        Dim nResult As Integer = 0

        nResult = gPMConstants.PMEReturnCode.PMTrue
        Try


            Dim oProduct As bSIRProduct.Business = Nothing
            Dim oIsReservesReadonly As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(oProduct, "bSIRProduct.Business", PMGetViaClientManager)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to get Product Business  object", ACApp, ACClass, "AddClaimReserve", Information.Err().Number, Information.Err().Description)
                Return nResult
            End If

            If m_lProductId = 0 Then
                m_lReturn = oProduct.GetProductid(ifilecnt:=m_lInsuranceFileCnt, vProduct_id:=m_lProductId)
            End If

            m_lReturn = oProduct.GetProductValue(m_lProductId, "is_reserves_read_only", oIsReservesReadonly)

            If m_sTransactionType <> "C_CP" Then
                If IsArray(oIsReservesReadonly) AndAlso oIsReservesReadonly(0, 0) <> "1" Then

                    Dim dbNumericTemp As Double
                    If Double.TryParse(Convert.ToString(uctCLMReserve.Tag), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                        m_lReturn = uctCLMReserve.Save()

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                            Return m_lReturn
                        End If
                    End If
                End If

            Else
                Dim dbNumericTemp2 As Double
                If Double.TryParse(Convert.ToString(uctCLMPayment.Tag), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                    m_lReturn = uctCLMPayment.Save()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        Return m_lReturn
                    End If
                End If
            End If
            If m_bNoTransactions Then
                'Payments can be raised as well here
                m_lReturn = uctCLMPayment.Save()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If
            End If
            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogExcepMessage(gPMConstants.PMELogLevel.PMLogOnError, "UpdatePaymentAndReserve Failed", ACApp, ACClass, "UpdatePaymentAndReserve", Information.Err().Number, excep.Message)

            Return nResult
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetReadOnly
    '
    ' Description: Sets the ReadOnly status of a control given the object.attribute name as a string
    '
    ' History: 01/02/2002 CLG - Created.
    ' ***************************************************************** '
    Public Function SetReadOnly(ByVal sName As String, ByVal vMode As Object, Optional ByRef vOverride As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim vControls() As Object
        Dim sControlName As String = ""
        Dim sCtlName As String = ""
        Dim i As Integer
        Try

            If Task = gPMConstants.PMEComponentAction.PMView Then
                Return result
            End If

            If IsNothing(vOverride) Then
                vOverride = False
            End If

            vControls = GetControlFromObjectAndAttributeName(sName)

            Dim cListView As Control
            If Information.IsArray(vControls) Then
                sControlName = Convert.ToString(vControls(0).Name)
                If sControlName.LastIndexOf("_") > 0 Then
                    sControlName = sControlName.Substring(1, sControlName.LastIndexOf("_") - 1)
                End If
                Select Case sControlName
                    Case "TabStrip1"
                        'this is something like the code we'd need but its too hard
                        '                For lTemp = 0 To m_lFrameIndex
                        '                    If (m_vFrameArray(ACFTabNumber, lTemp) = CInt(Right(TabStrip1.SelectedItem.Key, Len(TabStrip1.SelectedItem.Key) - 1))) Then
                        '                        fraFrame(lTemp).Visible = True
                        '                    Else
                        '                        fraFrame(lTemp).Visible = False
                        '                    End If
                        '                Next lTemp

                    Case "fraFrame"

                        If vControls(0).ForeColor <> (IIf(vMode, SystemColors.ControlDark, SystemColors.ControlText)) Then  ' RAW 29/06/2004 : removed 'Or 1 = 1' test
                            'can't actually set the frame as read only as this sets all the children to read only
                            vControls(0).ForeColor = IIf(vMode, SystemColors.ControlDark, SystemColors.ControlText)
                            'find all controls "contained" by this one
                            Dim arrControl As ArrayList = New ArrayList()
                            ControlList(vControls(0), arrControl)

                            For i = 0 To arrControl.Count - 1
                                sCtlName = arrControl(i).Name
                                If sCtlName.LastIndexOf("_") > 0 Then
                                    sCtlName = sCtlName.Substring(1, sCtlName.LastIndexOf("_") - 1)
                                End If

                                If GetIndex(arrControl(i).Parent, fraFrame) <> GetIndex(vControls(0), fraFrame) Then
                                    'skip controls not in the container
                                ElseIf sCtlName.ToString().Trim() = "lvwListView" Then
                                    'don't disable listview as we need these to get to child screens but save copy
                                    cListView = arrControl(i)
                                ElseIf sCtlName.ToString().Trim() <> "lvwSumInsured" And arrControl(i).Enabled <> Not vMode Then
                                    'if we have an override set of flags then use those
                                    If vOverride Then
                                        sCtlName = sCtlName.ToString().Trim()
                                        If sCtlName = "cmdListViewAdd" And vOverride And 1 Then ControlHelper.SetEnabled(arrControl(i), Not vMode)
                                        If sCtlName = "cmdListViewEdit" And vOverride And 2 Then ControlHelper.SetEnabled(arrControl(i), Not vMode)
                                        If sCtlName = "cmdListViewDelete" And vOverride And 4 Then ControlHelper.SetEnabled(arrControl(i), Not vMode)
                                        If sCtlName = "cmdListViewSequenceUp" And vOverride And 8 Then ControlHelper.SetEnabled(arrControl(i), Not vMode)
                                        If sCtlName = "cmdListViewSequenceDown" And vOverride And 16 Then ControlHelper.SetEnabled(arrControl(i), Not vMode)
                                    Else
                                        'if control is not in required state then set it
                                        ControlHelper.SetEnabled(arrControl(i), Not vMode)
                                    End If
                                    If sCtlName.ToString().Trim() = "cmdListViewEdit" Then
                                        arrControl(i).Text = IIf(ControlHelper.GetEnabled(arrControl(i)), "&Edit", "&View")
                                    End If
                                End If
                                Information.Err().Clear()
                            Next
                            If Not (cListView Is Nothing) And Not vMode Then
                                'we might have incorrectly enabled the edit and delete buttons when there are no
                                'entries in the list so simulate a mouse click to sort out the button states
                                lvwListView_MouseDown(lvwListView((GetIndex(cListView, lvwListView))), New MouseEventArgs(0, 0, CInt(VB6.TwipsToPixelsX(1)), CInt(VB6.TwipsToPixelsY(1)), 0))
                            End If
                        End If
                        ' RVH - CQ1887 22/7/03 : Start
                        '                        Allow user to set read only on party control
                        ' RAW 14/01/2004 : CQ3720 : added pnlPolicyPanel
                    Case "pnlPartyPanel", "pnlPolicyPanel"

                        If vControls(1).Enabled <> Not vMode Then
                            vControls(0).Enabled = Not vMode
                            vControls(1).Enabled = Not vMode
                        End If
                        ' RVH - CQ1887 22/7/03 : end

                        ' RAW 09/07/2004 : JIT : added
                    Case "lblText"

                        If vControls(0).Enabled <> Not vMode Then
                            vControls(0).Enabled = Not vMode
                        End If

                    Case Else
                        'check control's state before setting it as it is much quicker
                        'cboGISLookup aren't initially visible(?) so check caption not control!

                        If vControls(0).Enabled <> Not vMode Then

                            vControls(0).Enabled = Not vMode
                            vControls(1).Enabled = Not vMode
                        End If
                End Select
            End If

            Return result

            'Err_SetReadOnly:
        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetReadOnly Failed for " & sName, vApp:=ACApp, vClass:=ACClass, vMethod:="SetReadOnly", excep:=New Exception(Information.Err().Description))

            Return result
        Finally
            vOverride = vOverride
        End Try

    End Function

    'Public Function SetReadOnly(ByVal sName As String, ByVal vMode As Boolean) As Integer
    '    Dim tempRefParam As Nullable(Of Boolean) = Nothing
    '    Return SetReadOnly(sName, vMode, tempRefParam)



    ' ***************************************************************** '
    '
    ' Name: SetMandatoryColor
    '
    ' Description: Sets the color used for the background of mandatory controls
    '
    ' History: 01/02/2002 CLG - Created.
    ' ***************************************************************** '
    Public Function SetMandatoryColor(ByRef iRed As Integer, ByRef iGreen As Integer, ByRef iBlue As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lMandatoryColor = ColorTranslator.ToOle(Color.FromArgb(Math.Abs(iRed), Math.Abs(iGreen), Math.Abs(iBlue)))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "SetColor Failed", ACApp, ACClass, "SetMandatoryColor", Information.Err().Number, excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: UpdateLossScheduleReserve
    '
    ' Description: Refresg the reserves
    '
    ' History: 03/12/2002 CMG/PB PS202
    '
    ' ***************************************************************** '
    Public Function UpdateLossScheduleReserve() As Integer

        Dim result As Integer = 0
        Try



            'uctCLMReserve.GetDetails lPerilID:=m_lClaimPerilID, _
            'lClaimID:=m_lClaimID, _
            'lRiskId:=m_lClaimRiskId, _
            'lInsurance_File_Cnt:=m_lClaimInsFileCnt

            'uctCLMPayment.GetDetails lPerilID:=m_lClaimPerilID, _
            'lClaimID:=m_lClaimID, _
            'lRiskId:=m_lClaimRiskId, _
            'lInsurance_File_Cnt:=m_lClaimInsFileCnt


            '    If m_lReturn <> PMTrue Then
            '        ' Log Error Message
            '        LogMessage _
            ''        iType:=PMLogOnError, _
            ''        sMsg:="UpdateLossScheduleMainReserve Failed", _
            ''        vApp:=ACApp, _
            ''        vClass:=ACClass, _
            ''        vMethod:="UpdateLossScheduleReserve", _
            ''        vErrNo:=Err.Number, _
            ''        vErrDesc:=Err.Description
            '    End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "UpdateLossScheduleReserve Failed", ACApp, ACClass, "UpdateLossScheduleReserve", Information.Err().Number, excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function ShowPolicyRiskList() As Integer
        Dim result As Integer = 0
        Try



            Dim oPolicyRiskList As Object

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oPolicyRiskList As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPolicyRiskList, "iSIRPolicyRiskList.interface_Renamed", PMGetLocalInterface)
            oPolicyRiskList = temp_oPolicyRiskList

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError

                LogMessagePopup(gPMConstants.PMELogLevel.PMLogError, "Failed to create iSIRPolicyRiskList.interface", ACApp, ACClass, "ShowPolicyRiskList", excep:=New Exception(Information.Err().Description))

                Return result
            End If
            m_lReturn = oPolicyRiskList.Initialise
            'm_lReturn = ReflectionHelper.Invoke(oPolicyRiskList, "Initialise", New Object() {})
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError

                LogMessagePopup(gPMConstants.PMELogLevel.PMLogError, "Failed to initialise iSIRPolicyRiskList.interface", ACApp, ACClass, "ShowPolicyRiskList", excep:=New Exception(Information.Err().Description))

                Return result
            End If

            Dim varr(1, 7) As FormShowConstants

            varr(0, 0) = "party_cnt"
            ' PW071103 - CQ3050 - don't use child index

            ReflectionHelper.Invoke(ReflectionHelper.GetMember(Interface_Renamed, "GIS"), "GetPropertyValue", New Object() {"ASSOCIATED_CLIENT", "PARTY_CNT", m_sChildOIKey, varr(1, 0)})

            varr(0, 1) = "shortname"
            ' PW071103 - CQ3050 - don't use child index

            ReflectionHelper.Invoke(ReflectionHelper.GetMember(Interface_Renamed, "GIS"), "GetPropertyValue", New Object() {"ASSOCIATED_CLIENT", "resolved_name", m_sChildOIKey, varr(1, 1)})

            varr(0, 2) = "insurance_file_cnt"
            varr(1, 2) = m_lInsuranceFileCnt

            varr(0, 3) = "insurance_folder_cnt"
            varr(1, 3) = m_lInsuranceFolderCnt

            varr(0, 4) = "CreateEvent"
            varr(1, 4) = True

            varr(0, 5) = "CreateTask"
            varr(1, 5) = True

            varr(0, 6) = "display_mode"
            varr(1, 6) = FormShowConstants.Modal

            varr(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = CInt(PMKeyNameKeepWindowOnTop)
            If SubScreen Then
                varr(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = FormShowConstants.Modal
            Else
                varr(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = FormShowConstants.Modeless
            End If


            'm_lReturn = ReflectionHelper.Invoke(oPolicyRiskList, "SetKeys", New Object() {varr})
            m_lReturn = oPolicyRiskList.SetKeys(varr)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError

                LogMessagePopup(gPMConstants.PMELogLevel.PMLogError, "iSIRPolicyRiskList.interface Setkeys method failed", ACApp, ACClass, "ShowPolicyRiskList", excep:=New Exception(Information.Err().Description))

                Return result
            End If


            'm_lReturn = ReflectionHelper.Invoke(oPolicyRiskList, "Start", New Object() {})
            m_lReturn = oPolicyRiskList.start()
            ' RAW 14/10/2004 : CQ5119 : removed call to SetKeys - looks like a typo

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError

                LogMessagePopup(gPMConstants.PMELogLevel.PMLogError, "iSIRPolicyRiskList.interface Start method failed", ACApp, ACClass, "ShowPolicyRiskList", excep:=New Exception(Information.Err().Description))

                Return result
            End If


            oPolicyRiskList.Dispose()

            oPolicyRiskList = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "ShowPolicyRiskList Failed", ACApp, ACClass, "ShowPolicyRiskList", Information.Err().Number, excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddClaimPeril
    '
    ' Description:
    '
    ' History: 20/02/2003 CLG - Created.
    ' RAW 09/07/2004 : JIT : added v_lScreenDetailsIndex param and used instead of m_cControlCount
    ' ***************************************************************** '
    Private Function AddClaimPeril(ByRef iFrameIndex As Integer, ByRef lTag As Integer, ByRef lX As Single, ByRef lY As Single, ByRef bAddFrame As Boolean, ByRef sPropertyName As String, ByRef vTabSetIndex As Object, ByVal v_lScreenDetailsIndex As Integer) As Integer

        Dim result As Integer = 0
        Dim lDebugDepthCounter As Integer
        Dim vArray As Object

        Try

            ' RAW 09/07/2004 : JIT : added
#If (DebugOption And m_klDebugOption_JIT) = m_klDebugOption_JIT Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="JIT : Adding ClaimPeril - " & sPropertyName)
#End If

            result = gPMConstants.PMEReturnCode.PMTrue

            uctCLMPerilRT1.Initialise()
            'russells
            uctCLMPerilRT1.Claimid = m_lClaimID
            'jmf 28/5/2003 - need to used the tasId as display will need to be shown in different modes
            uctCLMPerilRT1.ClaimMode = m_iTask

            uctCLMPerilRT1.ViewRiskFlag = False
            uctCLMPerilRT1.Policy = 23
            uctCLMPerilRT1.Risk = m_lRiskId
            'jmf 28/5/2003 - need to used the tasId as display will need to be shown in different modes
            uctCLMPerilRT1.SetProcessModes(m_iTask, , , m_sTransactionType)

            uctCLMPerilRT1.LoadControl()


            '    uctCLMPerilRT1.GetDetails lPerilID:=m_lClaimWorkPerilID, _
            ''                            lClaimID:=m_lClaimWorkID, _
            ''                            lRiskID:=m_lClaimRiskId, _
            ''                            lInsurance_File_Cnt:=m_lClaimInsFileCnt


            If bAddFrame Then
                m_lReturn = AddFrame(iFrameIndex, lTag, lX, lY, 10000)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn

                    Return result
                End If
            End If

            '************
            ' MEvans : 11-11-2004 : CQ6977
            ' missed from original JIT changes
            ' store reference to this control in a collection - keyed on object and property name

            m_cObjectAndAttribute.Add(New Object() {fraFrame(iFrameIndex), uctCLMPerilRT1, "", v_lScreenDetailsIndex}, ObjectAndPropertyName(lTag).ToLower())

            m_oNewControlBeingLoaded = uctCLMPerilRT1
            '************

            uctCLMPerilRT1.Parent = fraFrame(iFrameIndex)
            m_uctClaimPerilTabIndex = vTabSetIndex

            uctCLMPerilRT1.Top = VB6.TwipsToPixelsY(lY)
            uctCLMPerilRT1.Left = VB6.TwipsToPixelsX(lX)
            uctCLMPerilRT1.Height = fraFrame(iFrameIndex).Height - VB6.TwipsToPixelsY(270)
            uctCLMPerilRT1.Width = fraFrame(iFrameIndex).Width - VB6.TwipsToPixelsX(120)

            uctCLMPerilRT1.Visible = True

            '    If (Task <> PMView) Then
            '       If (m_vDataDictionary(lTag \ 10000)(ACPIsInputProperty, lTag Mod 10000) = 1) Then
            'uctCLMPerilRT1.Enabled = True
            '      End If
            ' End If

            ' RVH 08/01/2004 - CQ3350: Incorrect value assigned to TAG property
            uctCLMPerilRT1.Tag = CStr(v_lScreenDetailsIndex)

            '    m_lReturn = SetExtraListViewProperties(v_hWndList:=uctCLMPerilRT1.hwnd, _
            ''                                           v_vShowRowSelect:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            '    vArray = m_vScreenValues(v_lScreenDetailsIndex)

            '    If IsArray(vArray) Then
            '        For lTemp = LBound(vArray, 2) To UBound(vArray, 2)
            '            Set oListItem = uctCLMPerilRT1.ListItems.Add(, , vArray(1, lTemp))
            '            oListItem.SubItems(1) = vArray(2, lTemp)
            '            oListItem.Tag = vArray(0, lTemp)
            '        Next lTemp
            '    End If

            vArray = Nothing


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "AddClaimPeril Failed", ACApp, ACClass, "AddClaimPeril", Information.Err().Number, Information.Err().Description, excep:=ex)




        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetScreenValues
    '
    ' Description: Save screenvalues into m_vScreenValues and call script
    '              OnChanged method.
    '
    ' History: 27/02/2003 Chris Ridgard - Created.
    ' RAW 21/05/2004 : Performance Changes : added v_vDataTypes param
    ' RAW 22/06/2004 : Performance Changes(#2) : added v_bSuppressDynamicLogic param
    ' ***************************************************************** '
    Private Sub SetScreenValues(ByRef r_lScreenIndex As Integer, ByRef r_vValues As Object, ByRef r_vObjIdx As Object, ByVal iTabNumber As Integer, Optional ByRef r_PropIdx As Object = Nothing, Optional ByVal v_vDataTypes As Object = Nothing, Optional ByVal v_vSuppressDynamicLogic As Object = Nothing)

        Dim lTag As Integer
        Dim vDataTypes As Object
        Dim eOnChangeAction As enumOnChangeAction
        Dim bSuppressDynamicLogic As Boolean
        Dim j As Integer

        If IsNothing(v_vSuppressDynamicLogic) Then
            bSuppressDynamicLogic = IsDynamicLogicSuppressed  ' RAW 20/08/2004 : JIT : replaced m_bSuppressDynamicLogic with IsDynamicLogicSuppressed
        Else
            bSuppressDynamicLogic = (v_vSuppressDynamicLogic)
        End If


        eOnChangeAction = enumOnChangeAction.eOnChangeAction_Change  ' RAW 22/06/2004 : Performance Changes(#2) : added


        ' RAW 21/05/2004 : Performance Changes : added code to handle non standard properties that include multiple data items and therefore multiple data types

        If Not IsNothing(v_vDataTypes) Then
            ' use the param values
            vDataTypes = v_vDataTypes
        Else
            ' work out the datatypes for ourselves
            If Not (Convert.IsDBNull(m_vScreenDetails(ACDChildScreenId, r_lScreenIndex)) Or IsNothing(m_vScreenDetails(ACDChildScreenId, r_lScreenIndex))) Then

                ' object is a list view

                j = -1
                For i As Integer = m_vChildScreenDetails.GetLowerBound(1) To m_vChildScreenDetails.GetUpperBound(1)

                    If m_vChildScreenDetails(ACDGISScreenId, i).Equals(m_vScreenDetails(ACDChildScreenId, r_lScreenIndex)) Then

                        j += 1
                        If Information.IsArray(vDataTypes) Then
                            vDataTypes = ArraysHelper.RedimPreserve(Of Object())(vDataTypes, New Integer() {j + 1}, New Integer() {0})
                        Else
                            ' RAW 21/06/2004 : added
                            vDataTypes = Array.CreateInstance(GetType(Object), New Integer() {j + 1}, New Integer() {0})
                        End If
                        m_lReturn = GetTag(lGISObjectId:=NullToLong(m_vChildScreenDetails(ACDGISObjectId, i)), lGISPropertyId:=NullToLong(m_vChildScreenDetails(ACDGISPropertyId, i)), lTag:=lTag, r_vDataDictionary:=m_vDataDictionary(GISDMTypeRisk))

                        ' RAW 21/06/2004 : added if test
                        If Information.IsArray(m_vDataDictionary(GISDMTypeRisk)) And lTag > -1 Then
                            vDataTypes.SetValue((m_vDataDictionary(GISDMTypeRisk)(uctRiskScreenControl.MainModule.ACPDataType, lTag Mod 10000)), j)
                        End If
                    End If
                Next

            Else

                ' RAW 09/07/2004 : JIT : replaced complicated structure with a much simpler case structure

                Select Case ControlType(r_lScreenIndex)
                    Case enumControlType.eControlType_Address, enumControlType.eControlType_Party, enumControlType.eControlType_Policy, enumControlType.eControlType_SumInsured, enumControlType.eControlType_StandardWording, enumControlType.eControlType_Label, enumControlType.eControlType_HyperLink, enumControlType.eControlType_ClaimPeril, enumControlType.eControlType_ClaimPayment, enumControlType.eControlType_ClaimReserve, enumControlType.eControlType_Find
                        '=================================

                        ' these are special properties

                        ' it is better that the calling procedure passes the data types for these objects as a parameter.
                        ' in which case this code will not be reached. If this is not done then code will need to be added here.

                    Case Else
                        '=================================

                        '    object is a single value
                        Try
                            vDataTypes = m_vDataDictionary(GISDMTypeRisk)(uctRiskScreenControl.MainModule.ACPDataType, CInt(r_vObjIdx Mod 10000))

                        Catch
                        End Try



                End Select
                ' RAW 09/07/2004 : JIT : end
            End If
        End If


        ' RAW 21/05/2004 : Performance Changes : added v_vDataTypes param
        If Not CompareArray(m_vScreenValues(r_lScreenIndex), r_vValues, vDataTypes) Then
            ' note that this code will also be triggered if CompareArray fails
            m_vScreenValues(r_lScreenIndex) = r_vValues

            '***********
            ' MEvans : 28-10-2003 : CQ2998
            ' Moved valid method check into callscriptmethod function
            ' RAW 21/05/2004 : Performance Changes : replaced call to CallScriptMethod with CallScriptOnChange
            ' RAW 22/06/2004 : Performance Changes(#2) : replaced call to CallScriptOnChange with RunDynamicLogic
            ' RAW 22/06/2004 : Performance Changes(#2) : replaced hard-code Action argument with variable eOnChangeAction
            ' RAW 22/06/2004 : Performance Changes(#2) : added if test
            If Not bSuppressDynamicLogic Then
                RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnChange, v_vDLAction:=eOnChangeAction, v_vObjIdx:=r_vObjIdx, v_vPropIdx:=r_PropIdx, v_vTabNumber:=iTabNumber)
            End If
        End If

        Exit Sub

Err_SetScreenValues:
        iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "SetScreenValues Failed", ACApp, ACClass, "SetScreenValues", Information.Err().Number, Information.Err().Description)

        Exit Sub

    End Sub



    ' ***************************************************************** '
    ' Name: CallScriptOnFocus
    '
    ' Description: Call script method from GotFocus events.
    '
    ' History: 27/02/2003 Chris Ridgard - Created.
    ' ' RAW 21/05/2004 : Performance Changes : added v_vControl & v_iTabNumber param
    ' RAW 22/06/2004 : Performance Changes(#2) : added UseLostFocus param
    ' ***************************************************************** '
    Private Sub CallScriptOnFocus(ByVal v_eOnFocusAction As enumOnFocusAction, Optional ByVal v_vObjIdx As Object = "", Optional ByVal v_vPropIdx As Object = Nothing, Optional ByVal v_vControl As Object = Nothing, Optional ByVal v_vTabNumber As Object = Nothing, Optional ByVal v_bUseLostFocus As Boolean = False)

        Static bMethodNotSuported As Boolean
        Dim sAction As String = ""
        Dim vTabNumber As Integer
        Dim sMethod As String = ""

        Try

            ' tab number is 1-based

            ' are we calling onFocus or onLostFocus
            sMethod = IIf(v_bUseLostFocus, "OnLostFocus", "OnFocus")  ' RAW 22/06/2004 : Performance Changes(#2) : added


            '***********
            ' MEvans : 28-10-2003 : CQ2998
            ' Moved valid method check into call script method function
            'If m_bDLFunction_onFocus_valid = True And Not bMethodNotSuported Then
            'If Not bMethodNotSuported Then


            Select Case v_eOnFocusAction
                Case enumOnFocusAction.eOnFocusAction_Combo
                    sAction = "COMBO"

                Case enumOnFocusAction.eOnFocusAction_Text
                    sAction = "TEXT"

                Case enumOnFocusAction.eOnFocusAction_List
                    sAction = "LIST"

                Case enumOnFocusAction.eOnFocusAction_YesNo
                    sAction = "YESNO"

                Case enumOnFocusAction.eOnFocusAction_FurtherDetails
                    sAction = "FURTHER_DETAILS"

                Case enumOnFocusAction.eOnFocusAction_Add
                    sAction = "ADD"

                Case enumOnFocusAction.eOnFocusAction_Edit
                    sAction = "EDIT"

                Case enumOnFocusAction.eOnFocusAction_Delete
                    sAction = "DELETE"

                    ' ' RAW 21/05/2004 : Performance Changes : added
                Case enumOnFocusAction.eOnFocusAction_Tab
                    sAction = "TAB"

                Case Else
                    sAction = "UNKNOWN_ACTION"

            End Select


            ' ' RAW 21/05/2004 : Performance Changes : added

            If Not IsNothing(v_vTabNumber) Then
                ' use the param value passed in
                vTabNumber = v_vTabNumber
            Else
                If Not Not (v_vControl Is Nothing) AndAlso v_vControl.Equals(Type.Missing) Then
                    vTabNumber = TabNumberForControl(v_vControl)
                    If vTabNumber > -1 Then vTabNumber += 1
                End If
            End If


            ' ' RAW 21/05/2004 : Performance Changes : added vTabNumber param
            ' RAW 22/06/2004 : Performance Changes(#2) : pass sMethod variable as a param instead of "OnFocus" string
            CallScriptMethod(sMethod:=sMethod, r_sObjectName:=ObjectAndPropertyName(v_vObjIdx, v_vPropIdx), r_sAction:=sAction, r_bMethodNotSuported:=bMethodNotSuported, iTabNumber:=vTabNumber)
        Catch excep As System.Exception


            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CallScriptOnFocus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CallScriptOnFocus", excep:=excep)

            Exit Sub
        End Try

    End Sub

    'Private Sub CallScriptOnFocus(ByVal v_eOnFocusAction As enumOnFocusAction, ByVal v_vObjIdx As String, ByVal v_vPropIdx As Object, ByVal v_vControl_optional As Object, ByVal v_vTabNumber_optional As Nullable(Of Integer))
    '    CallScriptOnFocus(v_eOnFocusAction, v_vObjIdx, v_vPropIdx, v_vControl_optional, v_vTabNumber_optional, False)


    'Private Sub CallScriptOnFocus(ByVal v_eOnFocusAction As enumOnFocusAction, ByVal v_vObjIdx As String, ByVal v_vPropIdx As Object, ByVal v_vControl_optional As Object)
    '    CallScriptOnFocus(v_eOnFocusAction, v_vObjIdx, v_vPropIdx, v_vControl_optional, Nothing, False)


    'Private Sub CallScriptOnFocus(ByVal v_eOnFocusAction As enumOnFocusAction, ByVal v_vObjIdx As String, ByVal v_vPropIdx As Object)
    '    CallScriptOnFocus(v_eOnFocusAction, v_vObjIdx, v_vPropIdx, Type.Missing, Nothing, False)


    'Private Sub CallScriptOnFocus(ByVal v_eOnFocusAction As enumOnFocusAction, ByVal v_vObjIdx As String)
    '    CallScriptOnFocus(v_eOnFocusAction, v_vObjIdx, Nothing, Type.Missing, Nothing, False)


    'Private Sub CallScriptOnFocus(ByVal v_eOnFocusAction As enumOnFocusAction)
    '    CallScriptOnFocus(v_eOnFocusAction, "", Nothing, Type.Missing, Nothing, False)


    ' ***************************************************************** '
    ' Name: CallScriptOnChange
    '
    ' Description:
    '
    ' History:
    ' RAW 21/05/2004 : Performance Changes : created
    ' ***************************************************************** '
    Private Sub CallScriptOnChange(ByVal v_eOnChangeAction As enumOnChangeAction, Optional ByVal v_vObjIdx As Object = "", Optional ByVal v_vPropIdx As Object = Nothing, Optional ByVal v_vControl As Object = Nothing, Optional ByVal v_vTabNumber As Object = Nothing)

        Static bMethodNotSuported As Boolean
        Dim sAction As String = ""
        Dim vTabNumber As Integer

        Try

            ' tab number is 1-based

            Select Case v_eOnChangeAction
                Case enumOnChangeAction.eOnChangeAction_Change
                    sAction = "CHANGE"

                Case enumOnChangeAction.eOnChangeAction_Refresh
                    sAction = "REFRESH"

                Case enumOnChangeAction.eOnChangeAction_Accept
                    sAction = "ACCEPT"

                Case Else
                    sAction = "UNKNOWN_ACTION"

            End Select


            If Not IsNothing(v_vTabNumber) Then
                ' use the param value passed in
                vTabNumber = v_vTabNumber
            Else
                If Not Not (v_vControl Is Nothing) AndAlso v_vControl.Equals(Type.Missing) Then
                    vTabNumber = TabNumberForControl(v_vControl)
                    If vTabNumber > -1 Then vTabNumber += 1
                End If
            End If

            CallScriptMethod(sMethod:="OnChange", r_sObjectName:=ObjectAndPropertyName(v_vObjIdx, v_vPropIdx), r_sAction:=sAction, r_bMethodNotSuported:=bMethodNotSuported, iTabNumber:=vTabNumber)
        Catch excep As System.Exception


            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CallScriptOnChange Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CallScriptOnChange", excep:=excep)

            Exit Sub
        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: CallScriptMethod
    '
    ' Description: Generic method to call script methods.
    '
    ' History: 27/02/2003 Chris Ridgard - Created.
    ' RAW 21/05/2004 : Performance Changes : significant changes made
    ' ***************************************************************** '
    Public Sub CallScriptMethod(ByRef sMethod As String, ByRef r_sObjectName As String, ByRef r_sAction As String, ByRef r_bMethodNotSuported As Boolean, Optional ByRef iTabNumber As Object = Nothing)
        Dim lDebugDepthCounter As Integer
        Static lExecutionCountStart, lExecutionCountOnFocus, lExecutionCountOnLostFocus, lExecutionCountOnChange As Integer
        Static bExecuting, bExecutingStart As Boolean

        Dim bStartedStart, bOkToRun As Boolean
        Dim lMethodExists, lExecutionCount, lErrorNumber As Integer
        Dim sErrorDesc As String = ""

        Try
            ' tab number is 1-based

            ' RAW 21/05/2004 : Performance Changes : added
            If Not (iTabNumber Is Nothing) AndAlso iTabNumber.Equals(Type.Missing) Then
                iTabNumber = CInt(TabStrip1.SelectedTab.Name.Substring(TabStrip1.SelectedTab.Name.Length - Math.Min(TabStrip1.SelectedTab.Name.Length, Strings.Len(TabStrip1.SelectedTab.Name) - 1))) + 1
            End If

            ' determine if we need to check the function exists

            Select Case sMethod.ToUpper()
                ' RAW 09/07/2004 : JIT : added
                Case "INITIALISE"
                    lMethodExists = m_bDLFunction_Initialise_valid

                    ' ' RAW 21/05/2004 : Performance Changes : added
                Case "START"
                    lMethodExists = m_bDLFunction_Start_valid

                Case "ONFOCUS"
                    lMethodExists = m_bDLFunction_onFocus_valid

                    ' RAW 22/06/2004 : Performance Changes(#2) : added
                Case "ONLOSTFOCUS"
                    lMethodExists = m_bDLFunction_onLostFocus_valid

                Case "ONCHANGE"
                    lMethodExists = m_bDLFunction_onChange_valid

                Case Else
                    lMethodExists = False
                    ' do nothing
            End Select

            ' if the method is present in the script
            If lMethodExists Then

                r_bMethodNotSuported = False

                If Not (m_oMSScriptControl Is Nothing) Then

                    ' RAW 05/07/2004 : added
                    ' can we run this method

                    Select Case sMethod.ToUpper()
                        Case "START"
                            If bExecutingStart Then
                                ' a start script is already running - we cant allow them to nest
                            Else
                                bOkToRun = True
                            End If

                        Case Else
                            bOkToRun = True
                    End Select
                End If
            Else
                ' indicate the method isnt supported.
                r_bMethodNotSuported = True  ' RAW 05/07/2004 : moved
            End If



            ' RAW 05/07/2004 : added
            If Not bOkToRun Then
                GoTo GetOutOfHere
            End If


            ' OK , so we have reached here which means that it is ok to go ahead and call this method
            '=========================================================================================

#If (DebugOption And m_klDebugOption_Script) = m_klDebugOption_Script Then

#End If



            Select Case sMethod.ToUpper()
                Case "START"
                    bExecutingStart = True  ' RAW 05/07/2004 : added
                    bStartedStart = True  ' RAW 05/07/2004 : added

                    lExecutionCountStart += 1
                    lExecutionCount = lExecutionCountStart

                Case "ONFOCUS"
                    lExecutionCountOnFocus += 1
                    lExecutionCount = lExecutionCountOnFocus

                    ' RAW 22/06/2004 : Performance Changes(#2) : added
                Case "ONLOSTFOCUS"
                    lExecutionCountOnLostFocus += 1
                    lExecutionCount = lExecutionCountOnLostFocus

                Case "ONCHANGE"
                    lExecutionCountOnChange += 1
                    lExecutionCount = lExecutionCountOnChange

            End Select

#If (DebugOption And m_klDebugOption_Script) = m_klDebugOption_Script Then

            AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="SCRIPT : " & sMethod & "(" & lExecutionCount & ") " & IIf(r_sObjectName = "", "", " for object " & r_sObjectName) & " on tab " & iTabNumber & IIf(r_sAction = "", "", " with action " & r_sAction))
#End If

            bExecuting = True
            m_bScriptTimedOut = False
            m_bScriptErrorDetected = False  ' RAW 22/06/2004 : Performance Changes(#2) : added

            ' Prepare KeyArray to send to Dynamic Logic  PN24176
            m_lReturn = PrepareKeyArray()

            ' ' RAW 21/05/2004 : Performance Changes : added case statement to test for START
            If m_bDLDebuggingEnabled Then
                LoadDLMethodForDebug(sMethod)
            End If

            Select Case sMethod.ToUpper()
                Case "INITIALISE"

                    ' RVH 10/12/2004 - Merged from 1.8.6 - "m_aArray" param

                    m_oMSScriptControl.Run(sMethod, New Object() {New Object() {iTabNumber, lExecutionCount, m_sTransactionType, m_aArray, m_aChildDataFromParent, m_iOriginalTask, KeyArray}})  'PN24176

                    ' RVH 10/12/2004 - Merged from 1.8.6 - "m_aArray" param
                Case "START"


                    m_oMSScriptControl.Run(sMethod, New Object() {New Object() {iTabNumber, lExecutionCount, m_sTransactionType, m_aArray, m_aChildDataFromParent, m_iOriginalTask, KeyArray}})  'PN24176

                    bExecutingStart = False  ' RAW 05/07/2004 : added
                    bStartedStart = False  ' RAW 05/07/2004 : added




                Case Else


                    m_oMSScriptControl.Run(sMethod, New Object() {r_sObjectName, r_sAction}, New Object() {iTabNumber, lExecutionCount, m_sTransactionType, m_aArray, m_aChildDataFromParent, m_iOriginalTask, KeyArray})  'PN24176
            End Select


            bExecuting = False  ' ' RAW 21/05/2004 : Performance Changes : added


            If m_bScriptTimedOut Then

#If (DebugOption And m_klDebugOption_Script) = m_klDebugOption_Script Then

                AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="SCRIPT : " & sMethod & "(" & lExecutionCount & ") - time out")
#End If
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Script timed out", ACApp, ACClass, "CallScriptMethod")
                GoTo GetOutOfHere
            End If


            ' RAW 09/07/2004 : JIT : moved from RunDynamicLogic
            'Check for mandatory properties
            For Each SProperty As String In m_cMandatoryProperties
                SetMandatory(SProperty, True, True)
            Next SProperty
            ' RAW 09/07/2004 : JIT : end


            GoTo GetOutOfHere

GetOutOfHere:   ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
            ' Debug message
#If (DebugOption) Then
            If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If
        Catch ex As Exception

            lErrorNumber = Information.Err().Number
            sErrorDesc = Information.Err().Description

#If (DebugOption And m_klDebugOption_Script) = m_klDebugOption_Script Then

            AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="SCRIPT : " & sMethod & "(" & lExecutionCount & ") - Error")
#End If


            'Object doesn't support this property or method
            If lErrorNumber = 438 Then
                r_bMethodNotSuported = True
                bExecuting = False  ' RAW 21/05/2004 : Performance Changes : added
                If bStartedStart Then bExecutingStart = False  ' RAW 05/07/2004 : added
                GoTo GetOutOfHere
            End If

            ' Log Error Message
            ' RAW 21/05/2004 : Performance Changes : replaced use of m_oMSScriptControl.Error with m_oMSScriptControl_Error event
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "CallScriptMethod Failed", ACApp, ACClass, "CallScriptMethod", lErrorNumber, sErrorDesc, excep:=ex)

            bExecuting = False
            If bStartedStart Then bExecutingStart = False  ' RAW 05/07/2004 : added


            GoTo GetOutOfHere

        Finally
            iTabNumber = iTabNumber

        End Try

    End Sub

    'Public Sub CallScriptMethod(ByRef sMethod As String, ByRef r_sObjectName As String, ByRef r_sAction As String, ByRef r_bMethodNotSuported As Boolean)
    '    Dim tempRefParam As Object = Type.Missing
    '    CallScriptMethod(sMethod, r_sObjectName, r_sAction, r_bMethodNotSuported, tempRefParam)




    ' ***************************************************************** '
    ' Name: ObjectAndPropertyName
    '
    ' Description: returns a string inthe form "Object.property" from
    '              the dictionary object,ehther element can be overridden
    '              by passing a string.
    '
    ' History: 27/02/2003 Chris Ridgard - Created.
    ' RAW 09/07/2004 : JIT : renamed for clarity
    ' ***************************************************************** '
    Private Function ObjectAndPropertyName(ByRef r_vObjIdx As Object, Optional ByRef r_PropIdx_optional As Object = Nothing) As String
        Dim r_PropIdx As Object = Nothing

        If r_PropIdx_optional Is Nothing OrElse Not r_PropIdx_optional.Equals(Type.Missing) Then r_PropIdx = TryCast(r_PropIdx_optional, Object)
        Try
            Dim result As String = String.Empty
            Dim sPropertyName As String = ""
            Dim vPropName As Object

            Try

                ' ' RAW 21/05/2004 : Performance Changes : added
                If Convert.ToString(r_vObjIdx) = "" Then
                    Return ""
                End If


                Select Case gPMFunctions.ToSafeDouble(r_vObjIdx)
                    ' RAW 09/07/2004 : JIT : added
                    Case ndcFreeFormatText, ndcHyperlink, ndcFindControl

                        ' r_PropIdx contains ScreenDetail index

                        Dim dbNumericTemp As Double
                        If Not (r_PropIdx_optional Is Nothing) AndAlso r_PropIdx_optional.Equals(Type.Missing) Or Not Double.TryParse((r_PropIdx), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "PropIdx is missing or not numeric", ACApp, ACClass, "ObjectAndPropertyName")

                            Return result
                        End If


                        vPropName = Convert.ToString(m_vScreenDetails(ACDHelpText, CInt(r_PropIdx))).Trim()


                        If Not (Convert.IsDBNull(vPropName) Or IsNothing(vPropName)) And (vPropName) <> "" Then

                            result = CStr(vPropName)

                        Else


                            If (r_vObjIdx) = ndcFindControl Then
                                result = "FindControl." & (vPropName)
                            Else
                                result = "label." & (vPropName)
                            End If

                            vPropName = r_PropIdx
                            If CDbl(r_vObjIdx) = ndcFindControl Then
                                result = "FindControl." & CStr(vPropName)
                            Else
                                result = "label." & CStr(vPropName)
                            End If
                        End If

                    Case Else


                        'If Not (r_PropIdx_optional Is Nothing) AndAlso r_PropIdx_optional.Equals(Type.Missing) Then
                        If (r_PropIdx_optional Is Nothing) Then

                            r_PropIdx = r_vObjIdx
                        End If

                        Dim dbNumericTemp2 As Double

                        If Double.TryParse(CStr(r_vObjIdx), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then


                            result = CStr(m_vDataDictionary(GISDMTypeRisk)(MainModule.ACOObjectName, CInt(r_vObjIdx Mod 10000)))
                        Else

                            result = Convert.ToString(r_vObjIdx)
                        End If

                        Dim dbNumericTemp3 As Double

                        If Double.TryParse(CStr(r_PropIdx), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then


                            sPropertyName = CStr(m_vDataDictionary(GISDMTypeRisk)(ACPPropertyName, CInt(r_PropIdx Mod 10000)))
                        Else

                            sPropertyName = Convert.ToString(r_PropIdx)
                        End If

                        If sPropertyName.Trim() <> "" Then
                            result = result & "." & sPropertyName
                        End If

                End Select

                Return result

            Catch excep As System.Exception


                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ObjectAndPropertyName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ObjectAndPropertyName", excep:=excep)

                Return result


                Return result
            End Try
        Finally
            r_PropIdx_optional = r_PropIdx
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CompareArray
    '
    ' Description: Compare 2 arrays of variants recursivly called for
    '              members that are variant arrays.
    '
    ' History: 27/02/2003 Chris Ridgard - Created.
    ' RAW 21/05/2004 : Performance Changes : replaced lFieldType param with v_vDataTypes param
    ' ***************************************************************** '
    Private Function CompareArray(ByVal r_vArray1 As Object,
                                  ByVal r_vArray2 As Object,
                                  Optional ByVal v_vDataTypes As Object = Nothing) As Boolean

        Dim result As Boolean = False
        Dim vDataType As Object

        Try


            If Not (Information.IsArray(r_vArray1) And Information.IsArray(r_vArray2)) Then
                If Not Information.IsArray(r_vArray1) And Not Information.IsArray(r_vArray2) Then
                    'neither are arrays
                    result = (r_vArray1 = r_vArray2)
                End If
                Return result
            End If

            ' Both are arrays

            ' are they different dimensions
            Dim iDim2, iDim1, iDim3 As Integer

            iDim1 = r_vArray1.Rank
            iDim2 = r_vArray2.Rank

            If iDim1 <> iDim2 Then Return result
            If r_vArray1.GetUpperBound(0) <> r_vArray2.GetUpperBound(0) Then Return result

            ' RAW 22/06/2004 : Performance Changes(#2) : added
            If iDim1 = 2 Then
                If r_vArray1.GetUpperBound(1) <> r_vArray2.GetUpperBound(1) Then Return result
            End If

            ' RAW 21/05/2004 : Performance Changes : added
            If (IsNothing(v_vDataTypes) = False) Then
                If Information.IsArray(v_vDataTypes) Then
                    ' If this is an array then it will always have one dimension containing datatypes for each property

                    iDim3 = v_vDataTypes.Rank
                    If iDim3 <> 1 Then Return result
                    If v_vDataTypes.GetUpperBound(0) <> r_vArray1.GetUpperBound(0) Then Return result
                End If
            End If
            ' RAW 21/05/2004 : Performance Changes : end



            If iDim1 = 1 Then
                For iCnt1 As Integer = r_vArray1.GetLowerBound(0) To r_vArray1.GetUpperBound(0)

                    If Information.IsArray(r_vArray1(iCnt1)) Or Information.IsArray(r_vArray2(iCnt1)) Then
                        ' RAW 21/05/2004 : Performance Changes : added tests for v_vDataTypes
                        If Not (v_vDataTypes Is Nothing) AndAlso v_vDataTypes.Equals(Type.Missing) Then
                            If Not CompareArray(r_vArray1(iCnt1), r_vArray2(iCnt1)) Then Return result
                        Else
                            If Information.IsArray(v_vDataTypes) Then
                                If Not CompareArray((r_vArray1(iCnt1)), (r_vArray2(iCnt1)), (v_vDataTypes(iCnt1))) Then Return result
                            Else
                                If Not CompareArray((r_vArray1(iCnt1)), (r_vArray2(iCnt1)), v_vDataTypes) Then Return result
                            End If
                        End If
                    Else
                        ' RVH 3/2/2004 CQ4151 - Need to be matching like with like, rather than values that
                        '                       suffer from formatting...
                        ' RAW 21/05/2004 : Performance Changes : reworked
                        ' RAW 03/09/2004 : added test for either being null - but not both

                        If Convert.ToString(r_vArray1(iCnt1)) <> Convert.ToString(r_vArray2(iCnt1)) Or (IsNothing(r_vArray1(iCnt1)) Xor IsNothing(r_vArray2(iCnt1)) = True) Then
                            ' The values are different so check for formatting etc


                            If Object.Equals(r_vArray1(iCnt1), Nothing) Or Object.Equals(r_vArray2(iCnt1), Nothing) Then
                                ' one - but not both - is empty
                                Return result
                            Else
                                If Not (v_vDataTypes Is Nothing) AndAlso v_vDataTypes.Equals(Type.Missing) Then
                                    vDataType = iGISSharedConstants.GISDataTypeUnknown
                                Else
                                    If Information.IsArray(v_vDataTypes) Then
                                        vDataType = v_vDataTypes(iCnt1)
                                    Else
                                        vDataType = v_vDataTypes
                                    End If
                                End If


                                Select Case vDataType
                                    Case iGISSharedConstants.GISDataTypeNumeric, iGISSharedConstants.GISDataTypeCurrency, iGISSharedConstants.GISDataTypePercentage, iGISSharedConstants.GISDataTypeOption
                                        ' PW240204 - CQ4313 - IsNumeric on an empty
                                        ' array elemnt returns tru, so need to check
                                        ' IsEmpty too
                                        Dim dbNumericTemp2 As Double
                                        Dim dbNumericTemp As Double
                                        If Double.TryParse((r_vArray1(iCnt1)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) And Double.TryParse((r_vArray2(iCnt1)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                                            If ToSafeCurrency(r_vArray1(iCnt1)) <> ToSafeCurrency(r_vArray2(iCnt1)) Then Return result
                                        Else
                                            Return result
                                        End If

                                        ' RAW 21/05/2004 : Performance Changes : added
                                    Case iGISSharedConstants.GISDataTypeDate

                                        If Information.IsDate(r_vArray1(iCnt1)) And Information.IsDate(r_vArray2(iCnt1)) Then
                                            If (r_vArray1(iCnt1)) <> (r_vArray2(iCnt1)) Then Return result
                                        Else
                                            Return result
                                        End If

                                    Case Else
                                        Return result

                                End Select
                            End If
                        End If
                    End If

                Next iCnt1

            ElseIf iDim1 = 2 Then

                For iCnt2 As Integer = r_vArray1.GetLowerBound(1) To r_vArray1.GetUpperBound(1)  ' ie each data row

                    For iCnt1 As Integer = r_vArray1.GetLowerBound(0) To r_vArray1.GetUpperBound(0)  ' ie each property(column)

                        If Information.IsArray(r_vArray1(iCnt1, iCnt2)) Or Information.IsArray(r_vArray2(iCnt1, iCnt2)) Then
                            ' RAW 21/05/2004 : Performance Changes : added tests for v_vDataTypes
                            If Not (v_vDataTypes Is Nothing) AndAlso v_vDataTypes.Equals(Type.Missing) Then
                                If Not CompareArray((r_vArray1(iCnt1, iCnt2)), (r_vArray2(iCnt1, iCnt2))) Then Return result
                            Else
                                If Information.IsArray(v_vDataTypes) Then
                                    If Not CompareArray((r_vArray1(iCnt1, iCnt2)), (r_vArray2(iCnt1, iCnt2)), (v_vDataTypes(iCnt1))) Then Return result
                                Else
                                    If Not CompareArray((r_vArray1(iCnt1, iCnt2)), (r_vArray2(iCnt1, iCnt2)), v_vDataTypes) Then Return result
                                End If
                            End If
                        Else
                            ' RVH 3/2/2004 CQ4151 - Need to be matching like with like, rather than values that
                            '                       suffer from formatting...
                            ' RAW 21/05/2004 : Performance Changes : reworked
                            ' RAW 03/09/2004 : add test for either being null - but not both

                            If Convert.ToString(r_vArray1(iCnt1, iCnt2)) <> Convert.ToString(r_vArray2(iCnt1, iCnt2)) Or (IsNothing(r_vArray1(iCnt1, iCnt2)) Xor IsNothing(r_vArray2(iCnt1, iCnt2)) = True) Then
                                ' The values are different so check for formatting etc

                                If Object.Equals(r_vArray1(iCnt1, iCnt2), Nothing) Or Object.Equals(r_vArray2(iCnt1, iCnt2), Nothing) Then
                                    ' one - but not both - is empty
                                    Return result
                                Else
                                    If Not (v_vDataTypes Is Nothing) AndAlso v_vDataTypes.Equals(Type.Missing) Then
                                        vDataType = iGISSharedConstants.GISDataTypeUnknown
                                    Else
                                        If Information.IsArray(v_vDataTypes) Then
                                            vDataType = v_vDataTypes(iCnt1)
                                        Else
                                            vDataType = v_vDataTypes
                                        End If
                                    End If

                                    Select Case vDataType
                                        Case iGISSharedConstants.GISDataTypeNumeric, iGISSharedConstants.GISDataTypeCurrency, iGISSharedConstants.GISDataTypePercentage, iGISSharedConstants.GISDataTypeOption
                                            If IsNumeric(r_vArray1(iCnt1, iCnt2)) And IsNumeric(r_vArray2(iCnt1, iCnt2)) Then
                                                If ToSafeCurrency(r_vArray1(iCnt1, iCnt2)) <> ToSafeCurrency(r_vArray2(iCnt1, iCnt2)) Then Return result
                                            Else
                                                Return result
                                            End If
                                        Case iGISSharedConstants.GISDataTypeDate

                                            If Information.IsDate(r_vArray1(iCnt1, iCnt2)) And Information.IsDate(r_vArray2(iCnt1, iCnt2)) Then
                                                If (r_vArray1(iCnt1, iCnt2) <> r_vArray2(iCnt1, iCnt2)) Then Return result
                                            Else
                                                Return result
                                            End If

                                        Case Else
                                            Return result

                                    End Select
                                End If
                            End If
                        End If

                    Next iCnt1
                Next iCnt2
            Else
                Throw New System.Exception("-1, " + +", Unsupported number of dimensions in array.")
            End If

            Return True

        Catch excep As System.Exception


            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CompareArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CompareArray", excep:=excep)

            Return result


            Return result
        End Try
    End Function

    'Private Function CompareArray(ByVal r_vArray1 As Object, ByVal r_vArray2 As Object) As Boolean
    '    Return CompareArray(r_vArray1, r_vArray2, Type.Missing)


    ' ***************************************************************** '
    ' Name: ArrayDimensionCount
    '
    ' Description: counts the number of dimensions in an array. VB dont
    '              do this (messy stuff!!)
    '
    ' History: 27/02/2003 Chris Ridgard - Created.
    '
    ' ***************************************************************** '
    Private Function ArrayDimensionCount(ByRef r_vArray As Object) As Integer
        Dim result As Integer = 0
        Dim iTmp As Integer
        If Not Information.IsArray(r_vArray) Then Return result

        Information.Err().Clear()

        Try
            While Information.Err().Number = 0
                result += 1
                iTmp = r_vArray.GetUpperBound(result - 1)
            End While

            Information.Err().Clear()  ' RAW 22/06/2004 : Performance Changes(#2) : added

            Return result - 1

        Catch exc As System.Exception
        End Try


    End Function


    ' ***************************************************************** '
    ' Name: UnloadControlArrays
    '
    ' Description: Unoaded control arrays appear to be responsible for GPFs
    '
    '
    ' History: 13/03/2003 Chris Ridgard - Created.
    '
    ' ***************************************************************** '
    Private Sub UnloadControlArrays()

        'CJR 13/3/2003 Make sure all loaded controls are unloaded.
        If m_bUserMode Then

            ' RAW 17/07/2003 : CQ781 : added
            ' clear out all references to the controls held in m_cObjectAndAttribute collection
            ' failing to do this may prevent usercontrol_terminate from firing and iPMURisk then errors
            m_cObjectAndAttribute = Nothing


            For Each oCtrl As Control In Controls_Renamed
                If Not (TypeOf oCtrl Is Object) Then
                    oCtrl.Parent = Me
                End If
            Next oCtrl


            UnloadControlArray(cboGISLookup)
            UnloadControlArray(cboList)
            UnloadControlArray(cboPMLookup)
            UnloadControlArray(chkYesNo)
            UnloadControlArray(cmdAddress)
            UnloadControlArray(cmdListViewAdd)
            UnloadControlArray(cmdListViewEdit)
            UnloadControlArray(cmdListViewDelete)
            UnloadControlArray(cmdListViewSequenceUp)
            UnloadControlArray(cmdListViewSequenceDown)
            UnloadControlArray(cmdPartyCommand)
            UnloadControlArray(cmdPolicyCommand)
            UnloadControlArray(cmdSumInsuredAdd)
            UnloadControlArray(cmdSumInsuredDelete)
            UnloadControlArray(cmdSumInsuredEdit)
            UnloadControlArray(lblCheckLabel)
            UnloadControlArray(lblCombo)
            UnloadControlArray(lblComment)
            UnloadControlArray(lblGISLookup)
            UnloadControlArray(lblList)
            UnloadControlArray(lblMlText)
            UnloadControlArray(lblPMLookup)
            UnloadControlArray(lblPremium)
            UnloadControlArray(lblRate)
            UnloadControlArray(lblText)
            UnloadControlArray(lblTotalSumInsured)
            UnloadControlArray(lvwListView)
            UnloadControlArray(lvwStandardWording)
            UnloadControlArray(lvwSumInsured)
            UnloadControlArray(PBFindRT1)
            UnloadControlArray(pnlPartyPanel)
            UnloadControlArray(pnlPolicyPanel)
            UnloadControlArray(pnlTotalSumInsured)
            UnloadControlArray(txtAddress1)
            UnloadControlArray(txtAddress2)
            UnloadControlArray(txtAddress3)
            UnloadControlArray(txtAddress4)
            UnloadControlArray(txtAddress5)
            UnloadControlArray(txtAddress6)
            UnloadControlArray(txtPremium)
            UnloadControlArray(txtMlText)
            UnloadControlArray(txtRate)
            UnloadControlArray(txtText)
            UnloadControlArray(fraFrame)

        End If

    End Sub

    Private Sub UnloadControlArray(ByRef r_objControlArray() As Object)


        Try   ' RAW 09/07/2004 : JIT : added

            'cannot unoad first element.


            For iCnt As Integer = ReflectionHelper.GetMember(r_objControlArray, "UBound") To (CInt(ReflectionHelper.GetMember(r_objControlArray, "LBound") + 1)) Step -1

                ContainerHelper.UnloadControl(r_objControlArray(iCnt))
            Next iCnt

        Catch exc As System.Exception
        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: PassValueToChild
    '
    ' Description: Saves a value into an array that can then be passed to a
    '              child screen
    '
    ' History:
    ' RAW 22/07/2003 : CQ1783 : copied code from 1.8.6
    '                   30/05/2003 Darren Pataki - Created.
    ' ***************************************************************** '
    Public Function PassValueToChild(ByVal v_sChildName As String, ByVal v_sValueName As String, ByVal v_vValue As String) As Integer

        Dim result As Integer = 0
        Try
            Dim iCount As Integer
            Const lcChildName As Integer = 0
            Const lcValueName As Integer = 1
            Const lcValue As Integer = 2

            If Not Information.IsArray(m_aDataToChild) Then
                ReDim m_aDataToChild(2, 0)
            Else
                'Now we need to check to see whether this valuename has been passed in
                'before (as dyn logic fires on each event, then it is a possibility).
                'If it has, check to see whether the value has changed - if it has
                'then save it, else ignore & leave
                For iCount = 0 To m_aDataToChild.GetUpperBound(1)
                    If Convert.ToString(m_aDataToChild(lcValueName, iCount)).Trim().ToUpper() = v_sValueName.Trim().ToUpper() Then
                        'we've matched the valuename, so check the value
                        If m_aDataToChild(lcValue, iCount) = v_vValue Then
                            'it's the same so leave
                            Return result
                        Else
                            'set the new value then leave
                            m_aDataToChild(lcValue, iCount) = v_vValue
                            Return result
                        End If
                    End If
                Next iCount
            End If

            iCount = m_aDataToChild.GetUpperBound(1)
            m_aDataToChild(lcChildName, iCount) = v_sChildName
            m_aDataToChild(lcValueName, iCount) = v_sValueName
            m_aDataToChild(lcValue, iCount) = v_vValue
            'resize the array for use again
            ReDim Preserve m_aDataToChild(2, (iCount + 1))



            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PassValueToChild Failed for " & v_sValueName, vApp:=ACApp, vClass:=ACClass, vMethod:="PassValueToChild", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ClearDownDocumentRequests
    '
    ' Parameters: n/a
    '
    ' Description: Clear down the output and document request objects
    '           from within the xml - no database clear down required
    '           as nothing will have been saved to the database at this
    '           point
    '
    ' History:
    '           Created : MEvans : 11-08-2003 : 223 Document Production
    ' ***************************************************************** '
    Private Function ClearDownDocumentRequests() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "ClearDownDocumentRequests"

        Dim sOutputName As String = ""
        Dim vOIKeyArray() As String
        Dim sOutputOIKey As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_sTransactionType = "C_CR" Or m_sTransactionType = "C_CO" Or m_sTransactionType = "C_CP" Then

                sOutputName = m_sGISDataModel & "_claim_output"
                ' if we have

                If ReflectionHelper.GetMember(ReflectionHelper.GetMember(m_oInterface, "GIS"), "Risk").Count(m_sGISDataModel & "_claim_output") > 0 Then

                    ' Get existing output key

                    m_lReturn = ReflectionHelper.GetMember(m_oInterface, "GIS").GetALLOIKey(sOutputName, vOIKeyArray)

                    If Information.IsArray(vOIKeyArray) Then
                        sOutputOIKey = vOIKeyArray(vOIKeyArray.GetUpperBound(0))
                    End If

                    If ReflectionHelper.GetMember(m_oInterface, "GIS").DelObjectInstance(sOutputName, sOutputOIKey) <> gPMConstants.PMEReturnCode.PMTrue Then

                        result = gPMConstants.PMEReturnCode.PMFalse

                        LogMessageToFile(g_oObjectManager.UserName, gPMConstants.PMELogLevel.PMLogOnError, sFunctionName & " Failed to delete document requests from xml for claim id ", ACApp, ACClass, sFunctionName)

                    End If

                End If

            End If


            Return result

        Catch excep As System.Exception





            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            LogMessageToFile(g_oObjectManager.UserName, gPMConstants.PMELogLevel.PMLogOnError, sFunctionName & " Failed", ACApp, ACClass, sFunctionName, excep:=excep)
            '*******************************

            Return result




            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: RefreshScreenControlsFromGIS
    ' Description:
    ' History:
    ' RAW 14/10/2003 : CQ2754 : created
    ' RAW 03/11/2003 : CQ2754/2862 : added an optional param v_lListViewIndex which limits refresh of list views to just that one. If it is missing then refresh all
    ' ***************************************************************** '
    Public Function RefreshScreenControlsFromGIS(Optional ByVal v_lListViewIndex As Integer = 0) As Integer
        Dim result As Integer = 0
        Const sFunctionName As String = "RefreshScreenControlsFromGIS"
        Dim sObjectName, sPropertyName As String

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            ' refresh screen values array
            m_lReturn = ReflectionHelper.Invoke(m_oInterface, "RefreshScreenValuesFromGIS", New Object() {m_vScreenValues})
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogMessageToFile(g_oObjectManager.UserName, gPMConstants.PMELogLevel.PMLogError, "Failed to refresh screen controls from GIS", ACApp, ACClass, sFunctionName)
                Return result
            End If
            ' refresh controls on form
            ' RAW 03/11/2003 : CQ2754/2862 : added v_lListViewIndex param
            m_lReturn = RefreshScreenControls(m_vScreenValues, v_lListViewIndex)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogMessageToFile(g_oObjectManager.UserName, gPMConstants.PMELogLevel.PMLogError, "Failed to refresh screen controls", ACApp, ACClass, sFunctionName)
                Return result
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            '******************************
            ' Log Error.
            LogMessageToFile(g_oObjectManager.UserName, gPMConstants.PMELogLevel.PMLogOnError, sFunctionName & " Failed - " & sObjectName & " - " & sPropertyName, ACApp, ACClass, sFunctionName, excep:=excep)
            '*******************************
            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: RefreshScreenControls
    ' Description:
    ' History:
    ' RAW 14/10/2003 : CQ2754 : created
    ' RAW 03/11/2003 : CQ2754/2862 : added an optional param v_lListViewIndex which limits refresh of list views to just that one. It it is missing then refresh all
    ' ***************************************************************** '
    Private Function RefreshScreenControls(ByVal v_vScreenValues() As Object, Optional ByVal v_lListViewIndex As Integer = 0) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "RefreshScreenControls"

        Dim lDebugDepthCounter As Integer
        Dim vValue As Object
        Dim vArray As Object
        Dim sObjectName, sPropertyName As String
        Dim vControls As Object
        Dim lScreenId, lListViewIndex, FieldFormat As Integer

        On Error GoTo Err_RefreshScreenControls

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Calling dynamic logic whilst refreshing the screen controls here will have no effect.
        ' Any dynamic logic should be run be run after

        For lTemp As Integer = m_vScreenDetails.GetLowerBound(1) To m_vScreenDetails.GetUpperBound(1)

            vValue = Nothing
            sPropertyName = ""
            sObjectName = ""
            If lTemp Mod 10 = 0 Then Application.DoEvents()

            If Not (Convert.IsDBNull(m_vScreenDetails(ACDExtraGISObjectName, lTemp)) Or IsNothing(m_vScreenDetails(ACDExtraGISObjectName, lTemp))) Then
                sObjectName = CStr(m_vScreenDetails(ACDExtraGISObjectName, lTemp))
            End If

            If Not (Convert.IsDBNull(m_vScreenDetails(ACDExtraGISPropertyName, lTemp)) Or IsNothing(m_vScreenDetails(ACDExtraGISPropertyName, lTemp))) Then
                sPropertyName = CStr(m_vScreenDetails(ACDExtraGISPropertyName, lTemp))
            End If


            ' RAW 03/11/2003 : CQ2754/2862 : added
            ' Find out which one control it is
            ' RAW 09/07/2004 : JIT : changed value passed to sName param to cater for missing property name
            vControls = GetControlFromObjectAndAttributeName(ObjectAndPropertyName(sObjectName, sPropertyName), gPMConstants.PMEReturnCode.PMTrue)
            ' RAW 03/11/2003 : CQ2754/2862 : end

            ' RAW 09/07/2004 : JIT : replaced complicated structure with a much simpler case structure

            Select Case ControlType(lTemp)
                Case enumControlType.eControlType_Address, enumControlType.eControlType_Party, enumControlType.eControlType_Policy
                    '=================================

                    vArray = (v_vScreenValues(lTemp))
                    If Information.IsArray(vArray) Then
                        vValue = vArray(0, 0)
                    End If

                Case enumControlType.eControlType_SumInsured, enumControlType.eControlType_StandardWording, enumControlType.eControlType_ClaimPayment, enumControlType.eControlType_ClaimReserve
                    '=================================

                    ' ????

                Case enumControlType.eControlType_Accumulation
                    '=================================
                    ' ?? leave this one for now until we prove that SetValue works ok
                    ' ?? also be aware that vArray(1) does NOT hold the caption as with other lookups

                Case enumControlType.eControlType_ClaimPeril
                    '=================================
                    ' do not refresh here because the Peril control refreshes itself

                Case enumControlType.eControlType_GeminiCombo
                    '=================================

                    vArray = (v_vScreenValues(lTemp))
                    If Information.IsArray(vArray) Then
                        ' Note - at the time of writing we cannot pass the Lookup ID because SetValue expects the list index - I dont know why.
                        ' So use the caption instead.
                        vValue = vArray(1)
                    End If

                Case enumControlType.eControlType_PMLookup, enumControlType.eControlType_GISLookup
                    '=================================

                    vArray = (v_vScreenValues(lTemp))
                    If Information.IsArray(vArray) Then
                        ' use the Lookup ID
                        vValue = vArray(0)
                    End If

                Case enumControlType.eControlType_TextBox, enumControlType.eControlType_CheckBox, enumControlType.eControlType_Label, enumControlType.eControlType_HyperLink
                    '=================================

                    vArray = (v_vScreenValues(lTemp))
                    If Information.IsArray(vArray) Then
                        vValue = vArray(0)
                    End If

                Case enumControlType.eControlType_ListView
                    '=================================
                    Dim index As Integer = 0
                    If Information.IsArray(vControls) Then
                        '    For Each list As ListView In lvwListView
                        '        index = +1
                        '        If list.Equals(vControls(1)) Then
                        '            Exit For
                        '        End If
                        '    Next

                        If Not True Then
                            ' we are to refresh all listviews
                            'lListViewIndex = ReflectionHelper.GetMember(vControls(1), "Index")
                            lListViewIndex = GetIndex(vControls(1), lvwListView)
                        Else
                            ' we only need to refresh the list view specified
                            lListViewIndex = v_lListViewIndex
                        End If


                        'If ReflectionHelper.GetMember(vControls(1), "Index") = lListViewIndex Then
                        If GetIndex(vControls(1), lvwListView) = lListViewIndex Then

                            ' This is the one we want

                            ' What screen is this listview on
                            Select Case m_vScreenDetails(ACDExtraObjectType, lTemp)
                                Case GISOTRisk, GISOTCase
                                    lScreenId = CInt(m_vScreenDetails(ACDChildScreenId, lTemp))
                                Case GISOTAssociatedClient
                                    lScreenId = m_vRiskTypeDetails(ACRAssociatedClientScreenId, 0)
                                Case GISOTDisclosure
                                    lScreenId = m_vRiskTypeDetails(ACRDisclosureScreenId, 0)
                            End Select

                            ' repopulate the list view with the new values...
                            ' note - vControls(1) is the listview and vControls(0) is the frame
                            ' PW080304 - CQ4625 - suppress the running of dynamic logic
                            ' in the following call. it is now done at the end of this
                            ' function
                            ' RAW 22/06/2004 : Performance Changes(#2) : added v_vSuppressDynamicLogic param

                            m_lReturn = PopulateListView(lListViewIndex, lScreenId, lTemp, False, True)
                            ' Call the dynamic logic when list view is populated, which is not currently being called from anywhere.
                            ' This is done as the dynamic logic called at the end does not call on_change for listview controls.
                            If v_lListViewIndex <> 0 Then
                                RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnChange,
                                                v_vDLAction:=enumOnChangeAction.eOnChangeAction_Refresh,
                                                v_vTabNumber:=ToSafeInteger(m_vFrameArray(ACFTabNumber, GetIndex(lvwListView(v_lListViewIndex).Parent, fraFrame))) + 1)
                            End If
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, " Failed to PopulateListView", ACApp, ACClass, sFunctionName, Nothing, Nothing, g_oObjectManager.UserName)
                                GoTo GetOutOfHere
                            End If
                        End If
                    End If

            End Select
            ' RAW 09/07/2004 : JIT : end



            If Not (Convert.IsDBNull(m_vScreenDetails(ACDExtraGISPropertyName, lTemp)) Or IsNothing(m_vScreenDetails(ACDExtraGISPropertyName, lTemp))) Then

                ' This is a property

                ' Set the value of the property

                If Not Object.Equals(vValue, Nothing) And sObjectName <> "" And sPropertyName <> "" Then

                    ' RAW 14/01/2004 : CQ3720 : added tests for empty and ""


                    If (Not (Convert.IsDBNull(vValue) Or IsNothing(vValue))) And (Not Object.Equals(vValue, Nothing)) And (Convert.ToString(vValue) <> "") Then
                        '
                        ' RVH 19/12/2003 CQ3012 - onchange being triggered for fields that have not changed.
                        '                         This was due to formatting on certain field types, for example
                        '                         date fields where coming back as "mm/dd/yyyy hh:mm:ss" which was
                        '                         NOT matching the existing field content formatted as "dd/mm/yyyy" etc.
                        '


                        FieldFormat = m_vDataDictionary(GISDMTypeRisk)(uctRiskScreenControl.MainModule.ACPDataType, CInt(gPMFunctions.ToSafeLong(ReflectionHelper.GetMember(vControls(0), "Tag")) Mod 10000))

                        Select Case FieldFormat
                            Case iGISSharedConstants.GISDataTypeDate
                                ' RAW 14/01/2004 : CQ3720 : added test for date
                                If Information.IsDate(vValue) Then
                                    vValue = vValue
                                End If
                            Case iGISSharedConstants.GISDataTypeNumeric, iGISSharedConstants.GISDataTypeCurrency, iGISSharedConstants.GISDataTypePercentage
                                ' RAW 14/01/2004 : CQ3720 : added test for numeric
                                Dim dbNumericTemp As Double
                                If Double.TryParse((vValue), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                                    vValue = vValue
                                End If
                        End Select
                    End If

                    SetValue(sObjectName & "." & sPropertyName, vValue, gPMConstants.PMEReturnCode.PMTrue)
                End If
            End If

        Next lTemp


        ' RAW 22/06/2004 : Performance Changes(#2) : added
        If Not m_bFireDLStartEvents Then

            If Not False Then
                ' note - we should really use a 'change' action here because we dont know whether anything has actually changed
                ' but we are on the assumption that we are only calling the refresh of this list view if something has changed
                RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnChange, v_vDLAction:=enumOnChangeAction.eOnChangeAction_Change, v_vObjIdx:=lvwListView(lListViewIndex).Tag, v_vPropIdx:="", v_vControl:=lvwListView(lListViewIndex))
            End If
        End If
        ' RAW 22/06/2004 : Performance Changes(#2) : end


        ' PW080304 - CQ4625 - add a RunDynamicLogic call here, when everything
        ' has finished being updated
        ' RAW 21/05/2004 : Performance Changes : added params
        ' note - we cant really use a 'change' action here because we dont know whether anything has actually changed
        ' for all tabs
        RunDynamicLogic(v_eDLProcedureName:=enumDLProcedureType.eDLProcedureName_OnChange, v_vDLAction:=enumOnChangeAction.eOnChangeAction_Refresh, v_vTabNumber:=-1)


        GoTo GetOutOfHere
        Resume
Err_RefreshScreenControls:

        result = gPMConstants.PMEReturnCode.PMError

        '******************************
        ' Log Error.
        iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, sFunctionName & " Failed - " & sObjectName & " - " & sPropertyName, ACApp, ACClass, sFunctionName, Information.Err().Number, Information.Err().Description, g_oObjectManager.UserName)
        '*******************************

        GoTo GetOutOfHere
        Resume


GetOutOfHere:   ' RAW 09/07/2004 : JIT : added and replaced all exit calls with goto here
        ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If

        Return result
    End Function

    ' ***************************************************************** '
    ' Name:         DeleteClaimPeril
    ' Description:  Deletes a claim peril node from GIS data
    '
    ' Parameters:   ClaimPerilId - identity int field
    '
    ' History:      Created : JMF : 23/10/2003
    '
    ' ***************************************************************** '
    Private Function DeleteClaimPeril(ByVal v_lClaimPerilId As Integer, ByRef r_oEngine As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "DeleteClaimPeril"
        Dim vChildScreenValues As Object
        Dim lTag As Integer
        Dim sChildOIKey As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'jump through hoop to work out which one to edit

            ReflectionHelper.GetMember(Interface_Renamed, "GIS").GetALLOIKey("work_claim_peril", vChildScreenValues)  'get the OI keys

            For lTag = 0 To (vChildScreenValues).GetUpperBound(0)

                ReflectionHelper.GetMember(Interface_Renamed, "GIS").GetPropertyValue("work_claim_peril", "claim_peril_id", vChildScreenValues(lTag), sChildOIKey)
                If StringsHelper.ToDoubleSafe(sChildOIKey) = v_lClaimPerilId Then
                    Exit For
                End If
            Next

            ReflectionHelper.GetMember(Interface_Renamed, "GIS").DelObjectInstance("work_claim_peril", vChildScreenValues(lTag))

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            LogMessageToFile(g_oObjectManager.UserName, gPMConstants.PMELogLevel.PMLogOnError, sFunctionName & " failed", ACApp, ACClass, sFunctionName, excep:=excep)
            Return result

        End Try
    End Function

    ' ' RAW 21/05/2004 : Performance Changes : added
    ' RAW 09/07/2004 : JIT : added - based on BuildScreen
    Private Function AddControlToForm(ByVal v_lScreenDetailIndex As Integer) As Integer

        Dim result As Integer = 0
        Dim lDataDictionaryOffset As Integer
        Dim sPropertyName, sColumnName, sObjectName As String
        Dim iDataType As Integer
        Dim bIsNonGIS As Boolean
        Dim lTag As Integer
        Dim vObjectTag, vPropertyTag, vFrameControls As Object
        Dim iFrameIndex As Integer

        Try

            ' This function will NOT load tabs and frames - they will already have been loaded from BuildScreen
            ' as the sequence that they are loaded in is important

            result = gPMConstants.PMEReturnCode.PMTrue

            'when a control is added signal ResetTabIndex should be run
            m_bResetTabIndexRequired = True

            If Not (Convert.IsDBNull(m_vScreenDetails(ACDDefaultObjectId, v_lScreenDetailIndex)) Or IsNothing(m_vScreenDetails(ACDDefaultObjectId, v_lScreenDetailIndex))) Then
                vObjectTag = m_vScreenDetails(ACDDefaultObjectId, v_lScreenDetailIndex)
            Else
                vObjectTag = ""
            End If


            If Not (Convert.IsDBNull(m_vScreenDetails(ACDDefaultPropertyId, v_lScreenDetailIndex)) Or IsNothing(m_vScreenDetails(ACDDefaultPropertyId, v_lScreenDetailIndex))) Then
                vPropertyTag = m_vScreenDetails(ACDDefaultPropertyId, v_lScreenDetailIndex)
            Else
                vPropertyTag = ""
            End If


            If Convert.IsDBNull(m_vScreenDetails(ACDGISObjectId, v_lScreenDetailIndex)) Or IsNothing(m_vScreenDetails(ACDGISObjectId, v_lScreenDetailIndex)) Then
                ' not linked to an object in DD
                If Not (Convert.IsDBNull(m_vScreenDetails(ACDGISPropertyId, v_lScreenDetailIndex)) Or IsNothing(m_vScreenDetails(ACDGISPropertyId, v_lScreenDetailIndex))) Then
                    lTag = CInt(m_vScreenDetails(ACDGISPropertyId, v_lScreenDetailIndex))
                End If

            Else
                ' linked to an object
                If Convert.IsDBNull(m_vScreenDetails(ACDDefaultPropertyId, v_lScreenDetailIndex)) Or IsNothing(m_vScreenDetails(ACDDefaultPropertyId, v_lScreenDetailIndex)) Then
                    ' not linked to a property in DD

                    lTag = CInt(m_vScreenDetails(ACDDefaultObjectId, v_lScreenDetailIndex))

                Else
                    ' this is a property

                    lDataDictionaryOffset = CInt(CDbl(IIf(m_vScreenDetails(ACDDefaultPropertyId, v_lScreenDetailIndex) = "", 0, m_vScreenDetails(ACDDefaultPropertyId, v_lScreenDetailIndex))) Mod 10000)
                    lTag = CInt(IIf(m_vScreenDetails(ACDDefaultPropertyId, v_lScreenDetailIndex) = "", 0, m_vScreenDetails(ACDDefaultPropertyId, v_lScreenDetailIndex)))

                    'object bound field
                    sObjectName = CStr(m_vDataDictionary(GISDMTypeRisk)(MainModule.ACOObjectName, lDataDictionaryOffset))
                    sPropertyName = CStr(m_vDataDictionary(GISDMTypeRisk)(MainModule.ACPPropertyName, lDataDictionaryOffset))

                    sColumnName = CStr(m_vDataDictionary(GISDMTypeRisk)(MainModule.ACPColumnName, lDataDictionaryOffset))

                    m_sPropertyName = sPropertyName
                    m_sColumnName = sColumnName

                    iDataType = 0

                    If (m_vDataDictionary(GISDMTypeRisk)(uctRiskScreenControl.MainModule.ACPDataType, lDataDictionaryOffset)) <> "" Then

                        iDataType = (m_vDataDictionary(GISDMTypeRisk)(uctRiskScreenControl.MainModule.ACPDataType, lDataDictionaryOffset))
                    End If

                    bIsNonGIS = False
                    If (m_vDataDictionary(GISDMTypeRisk)(MainModule.ACOIsNonGIS, lDataDictionaryOffset)) <> "" Then

                        bIsNonGIS = CBool(m_vDataDictionary(GISDMTypeRisk)(MainModule.ACOIsNonGIS, lDataDictionaryOffset))
                    End If

                End If
            End If

            If Convert.IsDBNull(m_vScreenDetails(ACDHelpText, v_lScreenDetailIndex)) Or IsNothing(m_vScreenDetails(ACDHelpText, v_lScreenDetailIndex)) Then
                m_sHelpText = ""
            Else
                m_sHelpText = CStr(m_vScreenDetails(ACDHelpText, v_lScreenDetailIndex)).Trim()
            End If


            If Convert.IsDBNull(m_vScreenDetails(ACDColumnWidth, v_lScreenDetailIndex)) Or IsNothing(m_vScreenDetails(ACDColumnWidth, v_lScreenDetailIndex)) Then
                m_lIncludeInList = 0
            Else
                m_lIncludeInList = 1
            End If


            If Convert.IsDBNull(m_vScreenDetails(ACDPreQuoteRequirement, v_lScreenDetailIndex)) Or IsNothing(m_vScreenDetails(ACDPreQuoteRequirement, v_lScreenDetailIndex)) Then
                m_lPreQuote = 0
            Else
                m_lPreQuote = CInt(m_vScreenDetails(ACDPreQuoteRequirement, v_lScreenDetailIndex))
            End If


            If Convert.IsDBNull(m_vScreenDetails(ACDPostQuoteRequirement, v_lScreenDetailIndex)) Or IsNothing(m_vScreenDetails(ACDPostQuoteRequirement, v_lScreenDetailIndex)) Then
                m_lPostQuote = 0
            Else
                m_lPostQuote = CInt(m_vScreenDetails(ACDPostQuoteRequirement, v_lScreenDetailIndex))
            End If


            If Convert.IsDBNull(m_vScreenDetails(ACDPurchaseRequirement, v_lScreenDetailIndex)) Or IsNothing(m_vScreenDetails(ACDPurchaseRequirement, v_lScreenDetailIndex)) Then
                m_lPurchase = 0
            Else
                m_lPurchase = CInt(m_vScreenDetails(ACDPurchaseRequirement, v_lScreenDetailIndex))
            End If


            If Convert.IsDBNull(m_vScreenDetails(ACDIsValuation, v_lScreenDetailIndex)) Or IsNothing(m_vScreenDetails(ACDIsValuation, v_lScreenDetailIndex)) Then
                m_lIsValuation = 0
            Else
                m_lIsValuation = (m_vScreenDetails(ACDIsValuation, v_lScreenDetailIndex) = gPMConstants.PMEReturnCode.PMTrue)
            End If


            If Convert.IsDBNull(m_vScreenDetails(ACDIsRateAndPremium, v_lScreenDetailIndex)) Or IsNothing(m_vScreenDetails(ACDIsRateAndPremium, v_lScreenDetailIndex)) Then
                m_lIsRateAndPremium = 0
            Else
                m_lIsRateAndPremium = (m_vScreenDetails(ACDIsRateAndPremium, v_lScreenDetailIndex) = gPMConstants.PMEReturnCode.PMTrue)
            End If

            m_lHeight = VB6.TwipsToPixelsY(CInt(m_vScreenDetails(ACDHeight, v_lScreenDetailIndex)))
            m_lWidth = VB6.TwipsToPixelsX(CInt(m_vScreenDetails(ACDWidth, v_lScreenDetailIndex)))

            Select Case ControlType(v_lScreenDetailIndex)
                Case enumControlType.eControlType_ListView
                    '=================================
                    ' get a reference to the frame that has already been created and is to contain this list view

                    vFrameControls = GetControlFromTag(vObjectTag, "#FRAME#", gPMConstants.PMEReturnCode.PMTrue, False)

                    If Not Information.IsArray(vFrameControls) Then
                        ' frame has not been created
                        result = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Cannot create list view without a frame to put it into", ACApp, ACClass, "AddControlToForm")
                        Return result
                    End If


                    'iFrameIndex = Convert.ToInt32(vFrameControls(1).tag)
                    iFrameIndex = GetIndex(vFrameControls(1), fraFrame)


                    m_lReturn = AddListView(iFrameIndex, lTag, VB6.TwipsToPixelsX(CInt(m_vScreenDetails(ACDLeft, v_lScreenDetailIndex))), VB6.TwipsToPixelsY(CInt(m_vScreenDetails(ACDTop, v_lScreenDetailIndex))), CStr(m_vScreenDetails(ACDCaption, v_lScreenDetailIndex)), m_vScreenDetails(ACDPMFormat, v_lScreenDetailIndex), m_vScreenDetails(ACDTabSetIndex, v_lScreenDetailIndex), v_lScreenDetailIndex)




                Case enumControlType.eControlType_TextBox
                    '=================================



                    'Developer Guide No. 
                    m_lReturn = AddTextBox(CInt(m_vScreenDetails(ACDParentId, v_lScreenDetailIndex)), lTag, VB6.TwipsToPixelsX(CInt(m_vScreenDetails(ACDLeft, v_lScreenDetailIndex))), VB6.TwipsToPixelsY(CInt(m_vScreenDetails(ACDTop, v_lScreenDetailIndex))), CStr(m_vScreenDetails(ACDCaption, v_lScreenDetailIndex)), m_vScreenDetails(ACDPMFormat, v_lScreenDetailIndex), m_vScreenDetails(ACDTabSetIndex, v_lScreenDetailIndex), (m_vDataDictionary(GISDMTypeRisk)(uctRiskScreenControl.MainModule.ACPDataType, CInt(lDataDictionaryOffset Mod 10000))) = iGISSharedConstants.GISDataTypeComment, v_lScreenDetailIndex, m_vScreenDetails(ACDExtraIsFormattedText, v_lScreenDetailIndex))




                Case enumControlType.eControlType_CheckBox
                    '=================================

                    If m_vScreenDetails(ACDPMFormat, v_lScreenDetailIndex) = gPMConstants.PMEFormatStyle.PMFormatBoolean Then
                        If Not IsArray(m_vCheckArray) Then ReDim m_vCheckArray(ACCLastArrayPosition, 0)  '0 might not be added first
                        If IsNumeric(m_vScreenValues(v_lScreenDetailIndex)(0)) Then
                            Select Case m_vScreenValues(v_lScreenDetailIndex)(0)
                                Case "0"
                                    g_iCheckBoxValue = 0
                                    m_vScreenValues(v_lScreenDetailIndex)(0) = "0"
                                    m_vScreenValues(v_lScreenDetailIndex)(1) = "0"
                                Case "1"
                                    g_iCheckBoxValue = 1
                                    m_vScreenValues(v_lScreenDetailIndex)(0) = "1"
                                    m_vScreenValues(v_lScreenDetailIndex)(1) = "1"
                                Case "2"
                                    g_iCheckBoxValue = 2
                                    m_vScreenValues(v_lScreenDetailIndex)(0) = "2"
                                    m_vScreenValues(v_lScreenDetailIndex)(1) = "2"
                            End Select


                        End If
                    End If
                    ' RAW 09/07/2004 : JIT : removed If Not IsArray(m_vCheckArray) Then ReDim m_vCheckArray(ACCLastArrayPosition, 0) '0 might not be added first

                    m_lReturn = AddCheckBox(CInt(m_vScreenDetails(ACDParentId, v_lScreenDetailIndex)), lTag, VB6.TwipsToPixelsX(CInt(m_vScreenDetails(ACDLeft, v_lScreenDetailIndex))), VB6.TwipsToPixelsY(CInt(m_vScreenDetails(ACDTop, v_lScreenDetailIndex))), CStr(m_vScreenDetails(ACDCaption, v_lScreenDetailIndex)), m_vScreenDetails(ACDPMFormat, v_lScreenDetailIndex), m_vScreenDetails(ACDTabSetIndex, v_lScreenDetailIndex), v_lScreenDetailIndex)




                Case enumControlType.eControlType_PMLookup
                    '=================================



                    'OK, so here it's a PMLookup box, and a label
                    ' RAW 09/07/2004 : JIT : removed If Not IsArray(m_vPMLookupArray) Then ReDim m_vPMLookupArray(ACCLastArrayPosition, 0) '0 might not be added first
                    m_lReturn = AddPMLookup(CInt(m_vScreenDetails(ACDParentId, v_lScreenDetailIndex)), lTag, VB6.TwipsToPixelsX(CInt(m_vScreenDetails(ACDLeft, v_lScreenDetailIndex))), VB6.TwipsToPixelsY(CInt(m_vScreenDetails(ACDTop, v_lScreenDetailIndex))), CStr(m_vScreenDetails(ACDCaption, v_lScreenDetailIndex)), m_vScreenDetails(ACDPMFormat, v_lScreenDetailIndex), m_vScreenDetails(ACDTabSetIndex, v_lScreenDetailIndex), v_lScreenDetailIndex)




                Case enumControlType.eControlType_GISLookup
                    '=================================



                    ' RAW 09/07/2004 : JIT : removed If Not IsArray(m_vGISComboArray) Then ReDim m_vGISComboArray(ACCLastArrayPosition, 0) '0 might not be added first

                    m_lReturn = AddGISComboBox(CInt(m_vScreenDetails(ACDParentId, v_lScreenDetailIndex)), lTag, VB6.TwipsToPixelsX(CInt(m_vScreenDetails(ACDLeft, v_lScreenDetailIndex))), VB6.TwipsToPixelsY(CInt(m_vScreenDetails(ACDTop, v_lScreenDetailIndex))), CStr(m_vScreenDetails(ACDCaption, v_lScreenDetailIndex)), m_vScreenDetails(ACDPMFormat, v_lScreenDetailIndex), m_vScreenDetails(ACDTabSetIndex, v_lScreenDetailIndex), v_lScreenDetailIndex)




                Case enumControlType.eControlType_GeminiCombo
                    '=================================



                    ' RAW 09/07/2004 : JIT : removed If Not IsArray(m_vGeminiComboArray) Then ReDim m_vGeminiComboArray(ACCLastArrayPosition, 0) '0 might not be added first

                    m_lReturn = AddGeminiComboBox(CInt(m_vScreenDetails(ACDParentId, v_lScreenDetailIndex)), lTag, VB6.TwipsToPixelsX(CInt(m_vScreenDetails(ACDLeft, v_lScreenDetailIndex))), VB6.TwipsToPixelsY(CInt(m_vScreenDetails(ACDTop, v_lScreenDetailIndex))), CStr(m_vScreenDetails(ACDCaption, v_lScreenDetailIndex)), m_vScreenDetails(ACDPMFormat, v_lScreenDetailIndex), m_vScreenDetails(ACDTabSetIndex, v_lScreenDetailIndex), v_lScreenDetailIndex)



                Case enumControlType.eControlType_Accumulation
                    '=================================



                    'OK, so here it's a PMLookup box, and a label
                    'm_lReturn = AddAccumulation(CInt(m_vScreenDetails(ACDParentId, v_lScreenDetailIndex)), lTag, CInt(m_vScreenDetails(ACDLeft, v_lScreenDetailIndex)), CInt(m_vScreenDetails(ACDTop, v_lScreenDetailIndex)), CStr(m_vScreenDetails(ACDCaption, v_lScreenDetailIndex)), m_vScreenDetails(ACDPMFormat, v_lScreenDetailIndex), m_vScreenDetails(ACDTabSetIndex, v_lScreenDetailIndex), v_lScreenDetailIndex)
                    m_lReturn = AddAccumulation(CInt(m_vScreenDetails(ACDParentId, v_lScreenDetailIndex)), lTag, VB6.TwipsToPixelsX(CInt(m_vScreenDetails(ACDLeft, v_lScreenDetailIndex))), VB6.TwipsToPixelsY(CInt(m_vScreenDetails(ACDTop, v_lScreenDetailIndex))), CStr(m_vScreenDetails(ACDCaption, v_lScreenDetailIndex)), m_vScreenDetails(ACDPMFormat, v_lScreenDetailIndex), m_vScreenDetails(ACDTabSetIndex, v_lScreenDetailIndex), v_lScreenDetailIndex)




                Case enumControlType.eControlType_Address
                    '=================================

                    'OK, so here it's an address


                    m_lReturn = AddAddress(CInt(m_vScreenDetails(ACDParentId, v_lScreenDetailIndex)), lTag, VB6.TwipsToPixelsX(CInt(m_vScreenDetails(ACDLeft, v_lScreenDetailIndex))), VB6.TwipsToPixelsY(CInt(m_vScreenDetails(ACDTop, v_lScreenDetailIndex))), sColumnName, sPropertyName, False, m_vScreenDetails(ACDPMFormat, v_lScreenDetailIndex), m_vScreenDetails(ACDTabSetIndex, v_lScreenDetailIndex), v_lScreenDetailIndex)




                Case enumControlType.eControlType_Party
                    '=================================



                    ' RAW 09/07/2004 : JIT : removed If Not IsArray(m_vPartyCommandArray) Then ReDim m_vPartyCommandArray(ACCLastArrayPosition, 0) '0 might not be added first

                    m_lReturn = AddPartyCommand(CInt(m_vScreenDetails(ACDParentId, v_lScreenDetailIndex)), lTag, VB6.TwipsToPixelsX(CInt(m_vScreenDetails(ACDLeft, v_lScreenDetailIndex))), VB6.TwipsToPixelsY(CInt(m_vScreenDetails(ACDTop, v_lScreenDetailIndex))), CStr(m_vScreenDetails(ACDCaption, v_lScreenDetailIndex)), m_vScreenDetails(ACDPMFormat, v_lScreenDetailIndex), m_vScreenDetails(ACDTabSetIndex, v_lScreenDetailIndex), v_lScreenDetailIndex)




                Case enumControlType.eControlType_Policy
                    '=================================



                    ' RAW 09/07/2004 : JIT : removed If Not IsArray(m_vPolicyCommandArray) Then ReDim m_vPolicyCommandArray(ACCLastArrayPosition, 0) '0 might not be added first

                    m_lReturn = AddPolicyCommand(m_vScreenDetails(ACDParentId, v_lScreenDetailIndex), lTag, VB6.TwipsToPixelsX(CInt(m_vScreenDetails(ACDLeft, v_lScreenDetailIndex))), VB6.TwipsToPixelsY(CInt(m_vScreenDetails(ACDTop, v_lScreenDetailIndex))), m_vScreenDetails(ACDCaption, v_lScreenDetailIndex), m_vScreenDetails(ACDPMFormat, v_lScreenDetailIndex), m_vScreenDetails(ACDTabSetIndex, v_lScreenDetailIndex), v_lScreenDetailIndex)




                Case enumControlType.eControlType_SumInsured
                    '=================================



                    'OK, so here it's a sum insured
                    ' RAW 09/07/2004 : JIT : removed If Not IsArray(m_vSumInsuredArray) Then ReDim m_vSumInsuredArray(ACCLastArrayPosition, 0) '0 might not be added first
                    m_lIsRateAndPremium = CInt(m_vScreenDetails(ACDIsRateAndPremium, v_lScreenDetailIndex))
                    m_lIsValuation = CInt(m_vScreenDetails(ACDIsValuation, v_lScreenDetailIndex))

                    m_lReturn = AddSumInsured(m_vScreenDetails(ACDParentId, v_lScreenDetailIndex), lTag, VB6.TwipsToPixelsX(CInt(m_vScreenDetails(ACDLeft, v_lScreenDetailIndex))), VB6.TwipsToPixelsY(CInt(m_vScreenDetails(ACDTop, v_lScreenDetailIndex))), sPropertyName, False, m_vScreenDetails(ACDPMFormat, v_lScreenDetailIndex), m_vScreenDetails(ACDTabSetIndex, v_lScreenDetailIndex), v_lScreenDetailIndex)




                Case enumControlType.eControlType_StandardWording
                    '=================================



                    'OK, so here it's a standard wording

                    m_lReturn = AddStandardWording(m_vScreenDetails(ACDParentId, v_lScreenDetailIndex), lTag, VB6.TwipsToPixelsX(CInt(m_vScreenDetails(ACDLeft, v_lScreenDetailIndex))), VB6.TwipsToPixelsY(CInt(m_vScreenDetails(ACDTop, v_lScreenDetailIndex))), False, sPropertyName, m_vScreenDetails(ACDTabSetIndex, v_lScreenDetailIndex), v_lScreenDetailIndex)




                Case enumControlType.eControlType_ClaimPayment
                    '=================================




                    m_lReturn = AddClaimPayment(CInt(m_vScreenDetails(ACDParentId, v_lScreenDetailIndex)), lTag, VB6.TwipsToPixelsX(CInt(m_vScreenDetails(ACDLeft, v_lScreenDetailIndex))), VB6.TwipsToPixelsY(CInt(m_vScreenDetails(ACDTop, v_lScreenDetailIndex))), False, sPropertyName, CByte(m_vScreenDetails(ACDTabSetIndex, v_lScreenDetailIndex)), v_lScreenDetailIndex)




                Case enumControlType.eControlType_ClaimReserve
                    '=================================




                    m_lReturn = AddClaimReserve(CInt(m_vScreenDetails(ACDParentId, v_lScreenDetailIndex)), lTag, VB6.TwipsToPixelsX(CInt(m_vScreenDetails(ACDLeft, v_lScreenDetailIndex))), VB6.TwipsToPixelsY(CInt(m_vScreenDetails(ACDTop, v_lScreenDetailIndex))), False, sPropertyName, CByte(m_vScreenDetails(ACDTabSetIndex, v_lScreenDetailIndex)), v_lScreenDetailIndex)




                Case enumControlType.eControlType_Label, enumControlType.eControlType_HyperLink
                    '=================================

                    ' RAW 09/07/2004 : JIT : removed If Not IsArray(m_vTextArray) Then ReDim m_vTextArray(ACCLastArrayPosition, 0) '0 might not be added first


                    m_lReturn = AddLabel(CInt(m_vScreenDetails(ACDParentId, v_lScreenDetailIndex)), lTag, VB6.TwipsToPixelsX(CInt(m_vScreenDetails(ACDLeft, v_lScreenDetailIndex))), VB6.TwipsToPixelsY(CInt(m_vScreenDetails(ACDTop, v_lScreenDetailIndex))), CStr(m_vScreenDetails(ACDCaption, v_lScreenDetailIndex)), m_vScreenDetails(ACDPMFormat, v_lScreenDetailIndex), m_vScreenDetails(ACDTabSetIndex, v_lScreenDetailIndex), m_vScreenDetails(ACDHelpText, v_lScreenDetailIndex), v_lScreenDetailIndex)



                Case enumControlType.eControlType_Find
                    '=================================

                    ' RAW 09/07/2004 : JIT : removed If Not IsArray(m_vFindControlArray) Then ReDim m_vFindControlArray(ACCLastArrayPosition, 0) '0 might not be added first


                    m_lReturn = AddFindControl(CInt(m_vScreenDetails(ACDParentId, v_lScreenDetailIndex)), lTag, VB6.TwipsToPixelsX(CInt(m_vScreenDetails(ACDLeft, v_lScreenDetailIndex))), VB6.TwipsToPixelsY(CInt(m_vScreenDetails(ACDTop, v_lScreenDetailIndex))), CStr(m_vScreenDetails(ACDCaption, v_lScreenDetailIndex)), m_vScreenDetails(ACDPMFormat, v_lScreenDetailIndex), m_vScreenDetails(ACDTabSetIndex, v_lScreenDetailIndex), v_lScreenDetailIndex)




                Case enumControlType.eControlType_Tab, enumControlType.eControlType_Frame, enumControlType.eControlType_ClaimPeril
                    '=================================
                    ' these are not added from here
                Case enumControlType.eControlType_CaseHeader
                    '=================================



                    'm_lReturn = AddCaseHeader(CInt(m_vScreenDetails(ACDParentId, v_lScreenDetailIndex)), lTag, CInt(m_vScreenDetails(ACDLeft, v_lScreenDetailIndex)), CInt(m_vScreenDetails(ACDTop, v_lScreenDetailIndex)), False, sPropertyName, m_vScreenDetails(ACDTabSetIndex, v_lScreenDetailIndex), v_lScreenDetailIndex)
                    m_lReturn = AddCaseHeader(CInt(m_vScreenDetails(ACDParentId, v_lScreenDetailIndex)), lTag, CInt(m_vScreenDetails(ACDLeft, v_lScreenDetailIndex)), CInt(m_vScreenDetails(ACDTop, v_lScreenDetailIndex)), False, sPropertyName, m_vScreenDetails(ACDTabSetIndex, v_lScreenDetailIndex), v_lScreenDetailIndex)



                Case enumControlType.eControlType_CaseClaimList
                    '=================================


                    m_lReturn = AddCaseClaimList(CInt(m_vScreenDetails(ACDParentId, v_lScreenDetailIndex)), lTag, VB6.TwipsToPixelsX(CInt(m_vScreenDetails(ACDLeft, v_lScreenDetailIndex))), VB6.TwipsToPixelsY(CInt(m_vScreenDetails(ACDTop, v_lScreenDetailIndex))), False, sPropertyName, m_vScreenDetails(ACDTabSetIndex, v_lScreenDetailIndex), v_lScreenDetailIndex)



                Case Else
                    '=================================
                    MessageBox.Show("Unhandled type", "AddControlToForm", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Select

            Return m_lReturn

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddControlToForm failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddControlToForm", excep:=excep)
            Return result
        End Try

    End Function

    'RVH 09/12/2004 - 1.9 Merge
    Public Function CurrentCurrencyISOCode() As String
        m_lReturn = CreatePBRiskPolicyCurrency()

        Return ReflectionHelper.GetMember(m_oPBRiskPolicyCurrency, "CurrentCurrencyISOCode")
    End Function

    Public Function CurrentCurrencyName() As String
        m_lReturn = CreatePBRiskPolicyCurrency()

        Return ReflectionHelper.GetMember(m_oPBRiskPolicyCurrency, "CurrentCurrencyName")
    End Function

    Public Function CurrentCurrencySymbol() As String
        m_lReturn = CreatePBRiskPolicyCurrency()

        Return ReflectionHelper.GetMember(m_oPBRiskPolicyCurrency, "CurrentCurrencySymbol")
    End Function

    Public Function PreviousCurrencyISOCode() As String
        m_lReturn = CreatePBRiskPolicyCurrency()

        Return ReflectionHelper.GetMember(m_oPBRiskPolicyCurrency, "PreviousCurrencyISOCode")
    End Function

    Public Function PreviousCurrencyName() As String
        m_lReturn = CreatePBRiskPolicyCurrency()

        Return ReflectionHelper.GetMember(m_oPBRiskPolicyCurrency, "PreviousCurrencyName")
    End Function

    Public Function PreviousCurrencySymbol() As String
        m_lReturn = CreatePBRiskPolicyCurrency()

        Return ReflectionHelper.GetMember(m_oPBRiskPolicyCurrency, "PreviousCurrencySymbol")
    End Function

    Public Sub ConvertToCurrentPolicyCurrency(ByVal v_vOldAmount As Object, ByRef r_vNewAmount As Decimal)

        Dim cOldAmount, cNewAmount As Decimal

        m_lReturn = CreatePBRiskPolicyCurrency()

        Dim dbNumericTemp As Double

        If Not (Convert.IsDBNull(v_vOldAmount) Or IsNothing(v_vOldAmount)) And Double.TryParse((v_vOldAmount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            cOldAmount = (v_vOldAmount)




            m_lReturn = ReflectionHelper.Invoke(m_oPBRiskPolicyCurrency, "ConvertToCurrentPolicyCurrency", New Object() {cOldAmount, cNewAmount})

            r_vNewAmount = cNewAmount
        Else
            r_vNewAmount = 0
        End If

    End Sub

    Public Sub ConvertBetweenCurrencies(ByVal v_vOldCurrencyISOCode As Object, ByVal v_vOldAmount As Object, ByVal v_vNewCurrencyISOCode As Object, ByRef r_vNewAmount As Decimal)

        Dim cOldAmount, cNewAmount As Decimal

        m_lReturn = CreatePBRiskPolicyCurrency()

        Dim dbNumericTemp As Double

        If Not (Convert.IsDBNull(v_vOldAmount) Or IsNothing(v_vOldAmount)) And Double.TryParse((v_vOldAmount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

            cOldAmount = (v_vOldAmount)






            m_lReturn = ReflectionHelper.Invoke(m_oPBRiskPolicyCurrency, "ConvertBetweenCurrencies", New Object() {(v_vOldCurrencyISOCode), cOldAmount, (v_vNewCurrencyISOCode), cNewAmount})

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_vNewAmount = 0
                Exit Sub
            End If

            r_vNewAmount = cNewAmount

        Else
            r_vNewAmount = 0
        End If
    End Sub

    Private Function CreatePBRiskPolicyCurrency() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oPBRiskPolicyCurrency Is Nothing Then

                Dim temp_m_oPBRiskPolicyCurrency As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oPBRiskPolicyCurrency, "bGISPMUExtras.PBRiskPolicyCurrency", PMGetViaClientManager)
                m_oPBRiskPolicyCurrency = temp_m_oPBRiskPolicyCurrency

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                ReflectionHelper.SetMember(m_oPBRiskPolicyCurrency, "InsuranceFileCnt", m_lInsuranceFileCnt)

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreatePBRiskPolicyCurrency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreatePBRiskPolicyCurrency", excep:=excep)

            Return result
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Mode"></param>
    ''' <param name="vDocumentTemplateID"></param>
    ''' <param name="r_sEditCode"></param>
    ''' <param name="r_sEditDescription"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EditStandardWording(ByRef Mode As String, ByRef vDocumentTemplateID As String,
                                           Optional ByRef r_sEditCode As String = "",
                                           Optional ByRef r_sEditDescription As String = "") As Integer
        Dim nResult As Integer = 0

        Dim o_iPMBDocTemplate As iPMBDocTemplate.Interface_Renamed
        Dim o_bSIRDocTemplate As bSIRDocTemplate.Business
        Dim sDocumentDirectory, sClauseDocTemplateDirectory As String
        Dim sOriginalDocTemplateFile, sCopiedDocTemplateFile, sNewDocTemplateFile As String
        Dim sUniqueFile As String = ""
        Dim sTempZipDirectory, sOriginalDocTemplateHTMFile, sNewDocTemplateHTMFile As String
        'For New Record
        Dim oCode, oDescription, oSourceId As Object
        Dim sDocumentTypeId As String = ""
        Dim vCreatedById, vDateCreated, vModifiedById, vLastModified, vIsDeleted, vSlotNumber, vRiskCodeId, vRiskGroupId, vIsEditableAfterMerging, vPrinter, vChaser, vDocumentFilter, vCopyOfOriginal, vOriginalDocumentTemplateID As Object

        'New Document Template ID
        Dim vNewDocumentTemplateID As String = ""
        Dim sEditCode As String = ""
        Dim sEditDescription As String = ""


        Try

            'Get an instance of the iPMBDocTemplate via the public object manager.
            Dim temp_o_iPMBDocTemplate As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_o_iPMBDocTemplate,
                                                     "iPMBDocTemplate.Interface_Renamed",
                                                     PMGetLocalInterface)
            o_iPMBDocTemplate = temp_o_iPMBDocTemplate

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError,
                                   "Failed to get Document Template object",
                                   ACApp, ACClass, "EditStandardWording", Information.Err().Number,
                                   Information.Err().Description)
                Return nResult
            End If

            If SubScreen Then
                m_lReturn = o_iPMBDocTemplate.SetKeys(m_vWindowKeys)
            End If

            'View Document Code
            If Mode = "View" Then
                o_iPMBDocTemplate.DocumentTemplateId = vDocumentTemplateID
                o_iPMBDocTemplate.ViewTask = True
                m_lReturn = o_iPMBDocTemplate.Start()
            End If

            If Mode = "EditOriginal" Then
                'Get an instance of the bPMBDocTemplate via the public object manager.
                m_lReturn = g_oObjectManager.GetInstance(
                oObject:=o_bSIRDocTemplate,
                sClassName:="bSIRDocTemplate.Business",
                vInstanceManager:=PMGetViaClientManager)

                ' Check for errors.
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    ' Log Error Message
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError,
                                       "Failed to get Document Template business object",
                                       ACApp, ACClass, "EditStandardWording",
                                       Information.Err().Number, Information.Err().Description)

                    Exit Function
                End If


                'Retrieve the Original Document Details
                m_lReturn = o_bSIRDocTemplate.GetDetails(vDocumentTemplateId:=vDocumentTemplateID)
                m_lReturn = o_bSIRDocTemplate.GetNext(vDocumentTemplateId:=vDocumentTemplateID,
                    vCode:=oCode, vDescription:=oDescription,
                    vSourceId:=oSourceId, vDocumentTypeId:=sDocumentTypeId,
                    vCreatedById:=vCreatedById, vDateCreated:=vDateCreated,
                    vModifiedById:=vModifiedById, vLastModified:=vLastModified,
                    vIsDeleted:=vIsDeleted, vSlotNumber:=vSlotNumber,
                    vRiskCodeId:=vRiskCodeId, vRiskGroupId:=vRiskGroupId,
                    vIsEditableAfterMerging:=vIsEditableAfterMerging,
                    vPrinter:=vPrinter, vChaser:=vChaser,
                    vDocumentFilter:=vDocumentFilter,
                    vCopyOfOriginal:=vCopyOfOriginal,
                    vOriginalDocumentTemplateID:=vOriginalDocumentTemplateID)

                o_iPMBDocTemplate.CallingAppName = "RiskScreenStandardWordingEdit"
                o_iPMBDocTemplate.DocumentTemplateId = vDocumentTemplateID
                m_lReturn = o_iPMBDocTemplate.Start()

                If o_iPMBDocTemplate.Status = SharedFiles.PMConst.PMOK Then
                    If o_iPMBDocTemplate.IsDocumentEdited = True Then
                        r_sEditCode = o_iPMBDocTemplate.DocumentTemplateCode
                        sEditDescription = o_iPMBDocTemplate.DocumentTemplateDescription
                        If Trim(oDescription) = Trim(sEditDescription) Then
                            m_lReturn = o_bSIRDocTemplate.UpdateDocumentTemplateDescription(v_lDocumentTemplateId:=ToSafeLong(vNewDocumentTemplateID),
                                                                                            v_sDescription:=(oDescription).Substring(0,
                                                                                            Math.Min((oDescription).Length, 250)).Trim() & "_COPY_" &
                                                                                        vNewDocumentTemplateID)
                        Else
                            m_lReturn = o_bSIRDocTemplate.UpdateDocumentTemplateDescription(v_lDocumentTemplateId:=ToSafeLong(vNewDocumentTemplateID),
                                                                                            v_sDescription:=sEditDescription)
                        End If
                        r_sEditDescription = sEditDescription
                        vDocumentTemplateID = o_iPMBDocTemplate.DocumentTemplateId
                    Else
                        sEditDescription = o_iPMBDocTemplate.DocumentTemplateDescription
                        If Trim(oDescription) <> Trim(sEditDescription) Then
                            m_lReturn = o_bSIRDocTemplate.UpdateDocumentTemplateDescription(v_lDocumentTemplateId:=ToSafeLong(vDocumentTemplateID),
                                                                                            v_sDescription:=sEditDescription)
                            r_sEditDescription = sEditDescription
                        End If
                    End If

                End If

                o_bSIRDocTemplate.Dispose()
                o_bSIRDocTemplate = Nothing

            End If

            If Mode = "EditNewCopy" Then
                'Get an instance of the bPMBDocTemplate via the public object manager.
                Dim temp_o_bSIRDocTemplate As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_o_bSIRDocTemplate,
                                                         "bSIRDocTemplate.Business", PMGetViaClientManager)
                o_bSIRDocTemplate = temp_o_bSIRDocTemplate

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError,
                                       "Failed to get Document Template business object",
                                       ACApp, ACClass, "EditStandardWording",
                                       Information.Err().Number, Information.Err().Description)

                    Return nResult
                End If

                'Retrieve the Original Document Details

                m_lReturn = o_bSIRDocTemplate.GetDetails(vDocumentTemplateId:=vDocumentTemplateID)

                m_lReturn = o_bSIRDocTemplate.GetNext(vDocumentTemplateId:=vDocumentTemplateID,
                                                      vCode:=oCode, vDescription:=oDescription,
                                                      vSourceId:=oSourceId, vDocumentTypeId:=sDocumentTypeId,
                                                      vCreatedById:=vCreatedById,
                                                      vModifiedById:=vModifiedById,
                                                      vIsDeleted:=vIsDeleted, vSlotNumber:=vSlotNumber,
                                                      vRiskCodeId:=vRiskCodeId, vRiskGroupId:=vRiskGroupId,
                                                      vIsEditableAfterMerging:=vIsEditableAfterMerging,
                                                      vPrinter:=vPrinter, vChaser:=vChaser,
                                                      vDocumentFilter:=vDocumentFilter, vCopyOfOriginal:=vCopyOfOriginal,
                                                      vOriginalDocumentTemplateID:=vOriginalDocumentTemplateID)

                'Get the original template file details
                m_lReturn = GetDocumentDirectory(sDocumentDirectory)
                sClauseDocTemplateDirectory = sDocumentDirectory & "\Type " & sDocumentTypeId
                sOriginalDocTemplateFile = sClauseDocTemplateDirectory & "\Doc " & vDocumentTemplateID & ".zip"

                'create a unique file to copy the original
                m_lReturn = SharedFiles.bPMDocFunctions.GetUniqueName(sUniqueFile)
                sCopiedDocTemplateFile = sClauseDocTemplateDirectory & "\" & sUniqueFile & ".zip"

                'Create a copy of original doc template
                m_lReturn = SharedFiles.bPMDocFunctions.CopyFile(sOriginalDocTemplateFile, sCopiedDocTemplateFile, True)

                'edit the original document template
                o_iPMBDocTemplate.CallingAppName = "RiskScreenStandardWordingEdit"
                o_iPMBDocTemplate.DocumentTemplateId = vDocumentTemplateID
                m_lReturn = o_iPMBDocTemplate.Start()

                If o_iPMBDocTemplate.Status = gPMConstants.PMEReturnCode.PMOK Then
                    'original document templated edited
                    'add a new record to document_template similar to original document template
                    vNewDocumentTemplateID = CStr(0)
                    If ToSafeInteger(vDocumentTemplateID) < 0 Then
                        m_lReturn = o_bSIRDocTemplate.GetUniqueClauseCode(Trim(Strings.Left(oCode.ToString.Replace("_" & oCode.Split("_")(oCode.Split("_").Length - 1), ""), 7)),
                                                                       r_sDocumentTemplateCode:=sEditCode)
                    Else
                        m_lReturn = o_bSIRDocTemplate.GetUniqueClauseCode((Strings.Left(oCode, 7).Trim()), r_sDocumentTemplateCode:=sEditCode)
                    End If

                    Dim sTempEditCode() As String
                    sTempEditCode = sEditCode.Split("_")
                    sTempEditCode(sTempEditCode.Length - 1) = "ED" & sTempEditCode(sTempEditCode.Length - 1)

                    sEditCode = String.Join("_", sTempEditCode)
                    r_sEditCode = sEditCode

                    sEditDescription = o_iPMBDocTemplate.DocumentTemplateDescription

                    m_lReturn = o_bSIRDocTemplate.DirectAdd(vDocumentTemplateId:=vNewDocumentTemplateID, vCode:=sEditCode,
                                                            vDescription:=(oDescription).Substring(0, Math.Min((oDescription).Length, 240)).Trim() _
                                                            & "_COPY",
                                                            vSourceId:=oSourceId, vDocumentTypeId:=sDocumentTypeId, vCreatedById:=g_iUserID, vModifiedById:=g_iUserID,
                                                            vIsDeleted:=vIsDeleted,
                                                            vSlotNumber:=vSlotNumber, vRiskCodeId:=vRiskCodeId,
                                                            vRiskGroupId:=vRiskGroupId, vIsEditableAfterMerging:=vIsEditableAfterMerging,
                                                            vPrinter:=vPrinter, vChaser:=vChaser,
                                                            vDocumentFilter:=vDocumentFilter, vCopyOfOriginal:=1, vOriginalDocumentTemplateID:=vDocumentTemplateID)

                    If Trim(oDescription) = Trim(sEditDescription) Then

                        Dim sTempDescription() As String
                        If ToSafeInteger(vDocumentTemplateID) < 0 Then
                            sTempDescription = oDescription.ToString.Split("_COPY_")
                            If sTempDescription.Length = 1 Then
                                sTempDescription(sTempDescription.Length - 1) = Trim(Strings.Left(oDescription, 240)) & "_COPY_" & vNewDocumentTemplateID
                            Else
                                If Not sTempDescription(sTempDescription.Length - 1).Contains(vDocumentTemplateID) Then
                                    sTempDescription(sTempDescription.Length - 1) = sTempDescription(sTempDescription.Length - 1) & "_COPY_" & vNewDocumentTemplateID
                                Else
                                    sTempDescription(sTempDescription.Length - 1) = vNewDocumentTemplateID
                                End If
                            End If
                        Else
                            ReDim sTempDescription(0)
                            sTempDescription(0) = Trim(Strings.Left(oDescription, 240)) & "_COPY_" & vNewDocumentTemplateID
                        End If

                        sEditDescription = String.Join("_", sTempDescription)
                    End If

                    m_lReturn = o_bSIRDocTemplate.UpdateDocumentTemplateDescription(v_lDocumentTemplateId:=ToSafeLong(vNewDocumentTemplateID),
                                                                                        v_sDescription:=sEditDescription)
                    r_sEditDescription = sEditDescription

                    'copy the edited document template file with newly created document
                    'template file
                    sNewDocTemplateFile = sClauseDocTemplateDirectory & "\Doc " & vNewDocumentTemplateID & ".zip"
                    m_lReturn = SharedFiles.bPMDocFunctions.CopyFile(sOriginalDocTemplateFile, sNewDocTemplateFile, True)

                    'copy the copied document template as original template file
                    m_lReturn = SharedFiles.bPMDocFunctions.CopyFile(sCopiedDocTemplateFile, sOriginalDocTemplateFile, True)
                    m_lReturn = SharedFiles.bPMDocFunctions.DeleteFile(sCopiedDocTemplateFile)


                    'last thing unzip the newly created doc template and rename the file with
                    'new doc template id and zip again
                    sTempZipDirectory = sClauseDocTemplateDirectory & "\" & sUniqueFile
                    sOriginalDocTemplateHTMFile = sTempZipDirectory & "\Doc " & vDocumentTemplateID & ".xml"
                    sNewDocTemplateHTMFile = sClauseDocTemplateDirectory & "\Doc " & vNewDocumentTemplateID & ".xml"

                    m_lReturn = CreateFolderTree(sTempZipDirectory)
                    m_lReturn = UnZip(sNewDocTemplateFile, sTempZipDirectory, True)

                    m_lReturn = SharedFiles.bPMDocFunctions.CopyFile(sOriginalDocTemplateHTMFile, sNewDocTemplateHTMFile, True, True)
                    m_lReturn = Zip(sNewDocTemplateFile, "xml")

                    m_lReturn = DelDirectory(sTempZipDirectory)

                    vDocumentTemplateID = vNewDocumentTemplateID

                Else
                    'Delete the copied doc template
                    m_lReturn = SharedFiles.bPMDocFunctions.DeleteFile(sCopiedDocTemplateFile)
                End If

                o_bSIRDocTemplate.Dispose()
                o_bSIRDocTemplate = Nothing
            End If

            'Free up memory

            o_iPMBDocTemplate.Dispose()
            o_iPMBDocTemplate = Nothing

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogExcepMessage(gPMConstants.PMELogLevel.PMLogOnError,
                                    "EditStandardWording Failed", ACApp, ACClass,
                                    "EditStandardWording", Information.Err().Number, excep.Message)

            Return nResult
        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: PrepareKeyArray
    '
    ' Description: Ensures all necessary values are in the KeyArray before
    '              passing to Dynamic Logic.
    '
    ' History: 21/09/2005 CJB - Created as part of PN24176.
    '
    ' ***************************************************************** '
    Private Function PrepareKeyArray() As Integer

        Dim result As Integer = 0
        Dim lRiskCodeID As Integer
        Dim sRiskCode As String = ""
        Dim bItemAlreadyExists As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get value for risk code id from KeyArray

            If Information.IsArray(m_vKeyArray) Then
                For iLoop As Integer = 0 To (m_vKeyArray).GetUpperBound(1)
                    If (m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop)).ToLower() = PMKeyNameRiskCodeID Then
                        lRiskCodeID = (m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop))
                        Exit For
                    End If
                Next iLoop
            End If

            ' If we have a risk code id then get the code from it
            If lRiskCodeID > 0 Then

                'm_lReturn = ReflectionHelper.Invoke(m_oSIRRiskScreen, "GetRiskCodeFromID", New Object() {lRiskCodeID, sRiskCode})
                m_lReturn = m_oSIRRiskScreen.GetRiskCodeFromID(lRiskCodeID, sRiskCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "m_oSIRRiskScreen.GetRiskCodeFromID Failed, v_lRiskCodeID:" & lRiskCodeID & ", m_lReturn:" & CStr(m_lReturn), ACApp, ACClass, "PrepareKeyArray", Information.Err().Number, Information.Err().Description)
                Else
                    ' If we have a risk code then save in existing KeyArray passed in
                    If sRiskCode.Length > 0 Then
                        If Not Information.IsArray(m_vKeyArray) Then
                            ReDim m_vKeyArray(1, 0)
                            'm_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "RiskCode"
                            'm_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = sRiskCode

                            m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "RiskCode"
                            m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = sRiskCode

                        Else
                            ' Check that we haven't already got the risk code in there!
                            For iLoop As Integer = (m_vKeyArray).GetLowerBound(1) To (m_vKeyArray).GetUpperBound(1)
                                If (m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop)).ToUpper() = "RISKCODE" Then
                                    bItemAlreadyExists = True
                                    Exit For
                                End If
                            Next
                            If Not bItemAlreadyExists Then
                                ReDim Preserve m_vKeyArray(1, (m_vKeyArray).GetUpperBound(1) + 1)
                                'm_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, m_vKeyArray.GetUpperBound(1)) = "RiskCode"
                                ' m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, m_vKeyArray.GetUpperBound(1)) = sRiskCode

                                m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, m_vKeyArray.GetUpperBound(1)) = "RiskCode"
                                m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, m_vKeyArray.GetUpperBound(1)) = sRiskCode


                            End If



                        End If
                    End If
                End If
            End If
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PrepareKeyArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrepareKeyArray", excep:=excep)
            Return result
        End Try
    End Function

    Private Sub cmdListViewSequenceUp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cmdListViewSequenceUp_0.Click
        Dim Index As Integer = Array.IndexOf(cmdListViewSequenceUp, eventSender)

        Dim iLine As Integer

        Try

            'Set row to be moved - if a valid one selected
            'Is this list view empty
            If lvwListView(Index).Items.Count < 1 Then
                Exit Sub
            End If

            iLine = lvwListView(Index).FocusedItem.Index + 1

            'Have we selected any
            If iLine = -1 Then
                Exit Sub
            End If

            'Is it really, really selected?
            If Not lvwListView(Index).Items.Item(iLine - 1).Selected Then
                Exit Sub
            End If

            'If we're at number 1, there's no need to do anything
            If iLine = 1 Then
                Exit Sub
            End If

            SwapListViewEntries(Index, iLine, -1)

            'cmdListViewEdit_Click Index

        Catch excep As System.Exception



            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdListViewSequenceUp_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdListViewSequenceUp_Click", excep:=excep)

            Exit Sub


        End Try

    End Sub
    Private Sub cmdListViewSequenceDown_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)  'Handles _cmdListViewSequenceDown_0.Click
        Dim Index As Integer = Array.IndexOf(cmdListViewSequenceDown, eventSender)

        Dim iLine As Integer

        Try

            'Set row to be moved - if a valid one selected
            'Is this list view empty
            If lvwListView(Index).Items.Count < 1 Then
                Exit Sub
            End If

            iLine = lvwListView(Index).FocusedItem.Index + 1

            'Have we selected any
            If iLine = -1 Then
                Exit Sub
            End If

            'Is it really, really selected?
            If Not lvwListView(Index).Items.Item(iLine - 1).Selected Then
                Exit Sub
            End If

            'If we're at the last one, there's no need to do anything
            If iLine = lvwListView(Index).Items.Count Then
                Exit Sub
            End If

            SwapListViewEntries(Index, iLine, 1)

        Catch excep As System.Exception



            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdListViewSequenceDown_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdListViewSequenceDown_Click", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub SwapListViewEntries(ByVal iIndex As Integer, ByVal iLine As Integer, ByVal iDirection As Integer)
        Dim iSource, iTarget As Integer

        'swap the sequence number in the list view array
        Dim lSequenceNumberColumn As Integer = CInt(Convert.ToString(cmdListViewSequenceUp(iIndex).Tag))
        Dim vArray(,) As Object = m_vScreenValues(CInt(Convert.ToString(cmdListViewAdd(iIndex).Tag)))
        'find the lines in the array as they are not in the same order as on the screen
        For iCount As Integer = 1 To vArray.GetUpperBound(0)
            If CDbl(vArray(iCount, lSequenceNumberColumn)) = iLine Then
                iSource = iCount
            End If
            If CDbl(vArray(iCount, lSequenceNumberColumn)) = (iLine + iDirection) Then
                iTarget = iCount
            End If
        Next
        vArray(iSource, lSequenceNumberColumn) = CDbl(vArray(iSource, lSequenceNumberColumn)) + iDirection
        vArray(iTarget, lSequenceNumberColumn) = CDbl(vArray(iTarget, lSequenceNumberColumn)) + (0 - iDirection)
        m_vScreenValues(CInt(Convert.ToString(cmdListViewAdd(iIndex).Tag))) = vArray  'don't forget to store the results

        '    'swap the sequence number in the gis
        Dim lScreenDetailsIndex As Integer = CInt(CDbl(Convert.ToString(cmdListViewAdd(iIndex).Tag)) Mod 10000)
        m_lReturn = ReflectionHelper.Invoke(ReflectionHelper.GetMember(m_oInterface, "GIS"), "SetPropertyValue", New Object() {m_vScreenDetails(ACDExtraGISObjectName, lScreenDetailsIndex), cProperty_SequenceId, vArray(iSource, vArray.GetUpperBound(1)), vArray(iSource, lSequenceNumberColumn)})
        m_lReturn = ReflectionHelper.Invoke(ReflectionHelper.GetMember(m_oInterface, "GIS"), "SetPropertyValue", New Object() {m_vScreenDetails(ACDExtraGISObjectName, lScreenDetailsIndex), cProperty_SequenceId, vArray(iTarget, vArray.GetUpperBound(1)), vArray(iTarget, lSequenceNumberColumn)})
        're-display the list view
        m_lReturn = PopulateListView(iIndex, CInt(m_vScreenDetails(ACDChildScreenId, lScreenDetailsIndex)), lScreenDetailsIndex, False, True)

        'sort on the sequence column
        lvwListView_ColumnClick_Body(iIndex, lvwListView(iIndex).Columns.Item(CInt(Convert.ToString(cmdListViewSequenceDown(iIndex).Tag)) - 1), SortOrder.Ascending)

        If lvwListView(iIndex).Visible Then
            lvwListView(iIndex).Focus()
            'Reset the selected item, automatically deselects the previous one
            lvwListView(iIndex).Items.Item(iLine + iDirection - 1).Selected = True
        End If

    End Sub

    '*****************************************************************
    '
    ' Name:SetListViewColumnCaption
    '
    ' Description: Sets the caption (and optionally the width) of a list view column

    ' History: 08/11/2004 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function SetListViewColumnCaption(ByVal v_sName As String, ByVal v_sColumnName As String, ByVal v_sValue As String, Optional ByVal v_vWidth As Object = Nothing) As String
        Dim result As String = String.Empty
        Dim sControlName As String = String.Empty
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".SetListViewColumnCaption")

        Try
            Dim vControls As Object

            vControls = GetControlFromObjectAndAttributeName(v_sName)
            If Information.IsArray(vControls) Then
                sControlName = Convert.ToString(ReflectionHelper.GetMember(vControls(0), "Name"))

                If sControlName.LastIndexOf("_") > 0 Then
                    sControlName = sControlName.Substring(1, sControlName.LastIndexOf("_") - 1)
                End If

                Select Case sControlName
                    Case "fraFrame"

                        For lTemp1 As Integer = 0 To CInt(ReflectionHelper.GetMember(ReflectionHelper.GetMember(vControls(1), "ColumnHeaders"), "Count") - 1)

                            If v_sColumnName.ToLower() = ReflectionHelper.Invoke(vControls(1), "ColumnHeaders", New Object() {lTemp1 + 1}).ToLower() Then

                                result = ReflectionHelper.GetMember(ReflectionHelper.Invoke(vControls(1), "ColumnHeaders", New Object() {lTemp1 + 1}), "Width")
                                If v_sValue = "" Then

                                    ReflectionHelper.SetMember(ReflectionHelper.Invoke(vControls(1), "ColumnHeaders", New Object() {lTemp1 + 1}), "Width", 0)
                                Else

                                    ReflectionHelper.SetMember(ReflectionHelper.Invoke(vControls(1), "ColumnHeaders", New Object() {lTemp1 + 1}), "Text", v_sValue)
                                    If Not Not (v_vWidth Is Nothing) AndAlso v_vWidth.Equals(Type.Missing) Then

                                        ReflectionHelper.SetMember(ReflectionHelper.Invoke(vControls(1), "ColumnHeaders", New Object() {lTemp1 + 1}), "Width", (v_vWidth))
                                    End If
                                End If
                            End If
                        Next
                        Return result
                    Case Else
                        MessageBox.Show(v_sName & " is not a listview", "Dynamic Logic Error :SetListViewColumnCaption", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Select
            End If

            Return result

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".SetListViewColumnCaption")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".SetListViewColumnCaption")

            result = ""

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetListViewColumnCaption Failed for " & v_sName & "." & v_sColumnName, vApp:=ACApp, vClass:=ACClass, vMethod:="SetListViewColumnCaption", excep:=excep)

            Return result



            Return result
        End Try
    End Function

    'Public Function SetListViewColumnCaption(ByVal v_sName As String, ByVal v_sColumnName As String, ByVal v_sValue As String) As String
    '    Return SetListViewColumnCaption(v_sName, v_sColumnName, v_sValue, Type.Missing)




    ' ***************************************************************** '
    '
    ' Name: AddCaseHeader
    '
    ' Description:
    '
    ' History:
    ' RAW
    ' ***************************************************************** '
    Private Function AddCaseHeader(ByRef iFrameIndex As Integer, ByRef lTag As Integer, ByRef lX As Single, ByRef lY As Single, ByRef bAddFrame As Boolean, ByRef sPropertyName As String, ByRef vTabSetIndex As Object, ByVal v_lScreenDetailsIndex As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddCaseHeader"

        Dim lDebugDepthCounter As Integer
        Dim vArray As Object
        Dim lReturn As gPMConstants.PMEReturnCode

        Try


#If (DebugOption And m_klDebugOption_JIT) = m_klDebugOption_JIT Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="JIT : Adding ClaimPayment - " & sPropertyName)
#End If

            result = gPMConstants.PMEReturnCode.PMTrue

            '**********************************************

            lReturn = uctCLMCaseHeader1.Initialise()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "uctCLMCaseHeader1.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            '**********************************************

            lReturn = uctCLMCaseHeader1.SetProcessModes(m_iTask, , , m_sTransactionType, m_dtEffectiveDate)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "uctCLMCaseHeader1.SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            '**********************************************

            uctCLMCaseHeader1.CaseID = m_lCaseID

            '**********************************************

            lReturn = uctCLMCaseHeader1.Load_Renamed
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "uctCLMCaseHeader1.Load Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            '**********************************************

            If bAddFrame Then
                m_lReturn = AddFrame(iFrameIndex, lTag, lX, lY, 10000)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn

                    Return result
                End If
            End If



            ' store reference to this control in a collection - keyed on object and property name

            m_cObjectAndAttribute.Add(New Object() {fraFrame(iFrameIndex), uctCLMCaseHeader1, "", v_lScreenDetailsIndex}, ObjectAndPropertyName(lTag).ToLower())

            m_oNewControlBeingLoaded = uctCLMCaseHeader1  ' RAW 20/08/2004 : JIT : added


            uctCLMCaseHeader1.Parent = fraFrame(iFrameIndex)
            m_uctCLMCaseHeaderTabIndex = vTabSetIndex

            uctCLMCaseHeader1.Top = VB6.TwipsToPixelsY(180)
            uctCLMCaseHeader1.Left = VB6.TwipsToPixelsX(60)
            uctCLMCaseHeader1.Height = fraFrame(iFrameIndex).Height - VB6.TwipsToPixelsY(270)
            uctCLMCaseHeader1.Width = fraFrame(iFrameIndex).Width - VB6.TwipsToPixelsX(120)

            uctCLMCaseHeader1.Visible = True


            ' it should use the screen details index rather than the object id stored in "lTag"
            uctCLMCaseHeader1.Tag = CStr(v_lScreenDetailsIndex)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            ' now flag this in the array as having been created
            m_vScreenDetails(ACDScreenDetailCnt, v_lScreenDetailsIndex) = v_lScreenDetailsIndex  ' RAW 20/08/2004 : JIT : added
            m_oNewControlBeingLoaded = Nothing  ' RAW 20/08/2004 : JIT : added

            m_bCaseHeader = True

            vArray = Nothing


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "AddCaseHeader Failed", ACApp, ACClass, "AddCaseHeader", Information.Err().Number, Information.Err().Description, excep:=ex)



        Finally

#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If


        End Try
        Return result
    End Function

    Private Function UpdateCase() As Integer
        Dim result As Integer = 0
        Dim lBaseCaseID As Integer
        Try


            Dim dbNumericTemp As Double
            If Double.TryParse(Convert.ToString(uctCLMCaseHeader1.Tag), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                m_lReturn = uctCLMCaseHeader1.Save()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

                lBaseCaseID = uctCLMCaseHeader1.BaseCaseID

                ''This may have changed following an update,
                ''So get the new one - Amit
                m_lCaseID = uctCLMCaseHeader1.CaseID
                m_sCaseNumber = uctCLMCaseHeader1.CaseNumber

                If ToSafeLong(uctCLMCaseHeader1.CaseID) > 0 And m_iTask = gPMConstants.PMEComponentAction.PMAdd Then



                    m_lReturn = ReflectionHelper.Invoke(m_oSIRRiskScreen, "UpdateCaseGISPolicyLink", New Object() {ToSafeLong(uctCLMCaseHeader1.CaseID), m_lScreenId})

                End If
            End If

            Dim dbNumericTemp2 As Double
            If Double.TryParse(Convert.ToString(uctCLMCaseClaim1.Tag), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                uctCLMCaseClaim1.BaseCaseId = lBaseCaseID
                ''''62125
                uctCLMCaseClaim1.PartyCnt = m_lPartyCnt
                m_lReturn = uctCLMCaseClaim1.Save()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If
            End If
            ' End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateCase Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateCase", excep:=excep)

            Return result
        End Try
    End Function

    Private Function AddCaseClaimList(ByRef iFrameIndex As Integer, ByRef lTag As Integer, ByRef lX As Single, ByRef lY As Single, ByRef bAddFrame As Boolean, ByRef sPropertyName As String, ByRef vTabSetIndex As Object, ByVal v_lScreenDetailsIndex As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddCaseClaimList"

        Dim lDebugDepthCounter As Integer
        Dim vArray As Object
        Dim lReturn As gPMConstants.PMEReturnCode

        Try


#If (DebugOption And m_klDebugOption_JIT) = m_klDebugOption_JIT Then

        AddToDebug(r_lDepthCounter:=lDebugDepthCounter, v_sText:="JIT : Adding ClaimPayment - " & sPropertyName)
#End If

            result = gPMConstants.PMEReturnCode.PMTrue
            lReturn = uctCLMCaseClaim1.Initialise()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "uctCLMCaseClaim1.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = uctCLMCaseClaim1.SetProcessModes(m_iTask, , , m_sTransactionType, m_dtEffectiveDate)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "uctCLMCaseClaim1.SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            '**********************************************

            uctCLMCaseClaim1.BaseCaseId = m_lBaseCaseID
            uctCLMCaseClaim1.CaseID = m_lCaseID
            uctCLMCaseClaim1.CaseNumber = uctCLMCaseHeader1.CaseNumber
            uctCLMCaseClaim1.CaseProgressStatusCode = uctCLMCaseHeader1.CaseProgressStatusCode
            '**********************************************

            lReturn = uctCLMCaseClaim1.Load_Renamed
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "uctCLMCaseClaim1.Load Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            '**********************************************

            If bAddFrame Then
                m_lReturn = AddFrame(iFrameIndex, lTag, lX, lY, 10000)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn

                    Return result
                End If
            End If



            ' store reference to this control in a collection - keyed on object and property name

            m_cObjectAndAttribute.Add(New Object() {fraFrame(iFrameIndex), uctCLMCaseClaim1, "", v_lScreenDetailsIndex}, ObjectAndPropertyName(lTag).ToLower())

            m_oNewControlBeingLoaded = uctCLMCaseClaim1  ' RAW 20/08/2004 : JIT : added


            uctCLMCaseClaim1.Parent = fraFrame(iFrameIndex)
            m_uctCLMCaseClaimTabIndex = vTabSetIndex

            uctCLMCaseClaim1.Top = VB6.TwipsToPixelsY(180)
            uctCLMCaseClaim1.Left = VB6.TwipsToPixelsX(60)
            uctCLMCaseClaim1.Height = fraFrame(iFrameIndex).Height - VB6.TwipsToPixelsY(270)
            uctCLMCaseClaim1.Width = fraFrame(iFrameIndex).Width - VB6.TwipsToPixelsX(120)

            uctCLMCaseClaim1.Visible = True


            ' it should use the screen details index rather than the object id stored in "lTag"
            uctCLMCaseClaim1.Tag = CStr(v_lScreenDetailsIndex)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            ' now flag this in the array as having been created
            m_vScreenDetails(ACDScreenDetailCnt, v_lScreenDetailsIndex) = v_lScreenDetailsIndex  ' RAW 20/08/2004 : JIT : added
            m_oNewControlBeingLoaded = Nothing  ' RAW 20/08/2004 : JIT : added

            m_bCaseHeader = True

            vArray = Nothing


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "AddCaseClaimList Failed", ACApp, ACClass, "AddCaseClaimList", Information.Err().Number, Information.Err().Description, excep:=ex)



        Finally
            ' Debug message
#If (DebugOption) Then
        If lDebugDepthCounter > 0 Then AddToDebug(lDebugDepthCounter * -1)  ' decrement the counter
#End If


        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: LoadPartyDataEngine
    '
    ' Description:  Load a data engine with the Party risk details for a Party
    '               This data is then exposed as Engine.PartyDataEngine
    '               Called from Public Property Get PartyDataEngine
    ' PLICO14
    ' ***************************************************************** '

    Public Function LoadPartyDataEngine(ByRef lPartyCnt As Integer, ByRef m_oPartyDataSet As Object) As Integer
        Dim result As Integer = 0
        Dim oGIS As bGIS.Application
        Dim sXMLDataset, sXMLDataSetDef, sMsg As String


        Try

            sMsg = "Could not load bGIS.Application"
            Dim temp_oGIS As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oGIS, "bGIS.Application", PMGetViaClientManager)
            oGIS = temp_oGIS
            If oGIS Is Nothing Then
                Throw New Exception(sMsg)
            End If

            sMsg = "Could not initialise bGIS.Application"

            If oGIS.Initialise(g_sUsername, g_sPassword, g_iUserID, g_iSourceID, g_iLanguageID, g_oObjectManager.CurrencyID, gPMConstants.PMELogLevel.PMLogError, m_sCallingAppName) <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception(sMsg)
            End If

            sMsg = "Could not LoadPartyFromDB"

            If oGIS.LoadPartyFromDB(sXMLDataSetDef, sXMLDataset, "", lPartyCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception(sMsg)
            End If

            ' Create Data Set Control
            sMsg = "Could not load cGISDataSetControl.Application"

            m_lReturn = g_oObjectManager.GetInstance(m_oPartyDataSet, "cGISDataSetControl.Application", Nothing)

            If m_oPartyDataSet Is Nothing Then
                Throw New Exception(sMsg)
            End If

            ' Load From XML
            sMsg = "Could not LoadFromXML"

            If m_oPartyDataSet.LoadFromXML(sXMLDataSetDef, sXMLDataset) <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception(sMsg)
            End If

            result = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, sMsg, ACApp, ACClass, " LoadPartyDataEngine", Information.Err().Number, Information.Err().Description, excep:=ex)

        Finally
            'Tidy up
            If Not (oGIS Is Nothing) Then
                oGIS.Dispose()
            End If


        End Try
        Return result
    End Function
    Private Function GetIndex(ByVal Container As Object, ByVal ControlArray() As Object) As Integer
        Dim ControlIndex As Integer = 0
        For ControlIndex = 0 To ControlArray.Length - 1
            If Not IsNothing(ControlArray(ControlIndex)) Then
                If ControlArray(ControlIndex).Equals(Container) Then
                    Return ControlIndex
                End If
            End If
        Next
        Return ControlIndex
    End Function
	<HandleProcessCorruptedStateExceptions>
    Private Function LoadDLMethodForDebug(ByVal sMethodName As String) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "LoadDLMethodForDebug"
        Dim sStr As String = ""

        Try

            result = False

            'destroy the old ScriptControl control
            m_oMSScriptControl.Reset()
            m_oMSScriptControl = Nothing

            'create a new instance
            m_oMSScriptControl = New MSScriptControl.ScriptControl()
            m_oMSScriptControl.Language = "VBScript"

            ' to be modal to this control's parent form
            'UPGRADE_WARNING: (2074) Control property UserControl.Parent was upgraded to UserControl.FindForm which has a new behavior. More Information: http://www.vbtonet.com/ewis/ewi2074.aspx
            m_oMSScriptControl.SitehWnd = MyBase.FindForm().Handle.ToInt32()

            sStr = ""
            sStr = sStr & "Option Explicit" & Strings.Chr(13) & Strings.Chr(10)

            'add a SetMandatoryColor method that doesn't need .Engine for legacy schemes code
            sStr = sStr & "Function SetMandatoryColor(iRed, iGreen , iBlue )" & Strings.Chr(13) & Strings.Chr(10)
            sStr = sStr & "    SetMandatoryColor=Engine.SetMandatoryColor(cint(iRed),cint(iGreen),cint(iBlue))" & Strings.Chr(13) & Strings.Chr(10)
            sStr = sStr & "End Function" & Strings.Chr(13) & Strings.Chr(10)

            sStr = sStr & CStr(m_vScreenHeader(PBDatabaseConsts.ACHScriptDynamicLogic, 0))

            'add the stop if required
            'either we've got a sub Start(aParam) or a sub Start()
            sStr = Strings.Replace(sStr, "Sub " & sMethodName & "(aParam)", "Sub Start(aParam)" & Strings.Chr(13) & Strings.Chr(10) & "Stop" & Strings.Chr(13) & Strings.Chr(10), 1, , CompareMethod.Text)
            sStr = Strings.Replace(sStr, "Sub " & sMethodName & "()", "Sub Start()" & Strings.Chr(13) & Strings.Chr(10) & "Stop" & Strings.Chr(13) & Strings.Chr(10), 1, , CompareMethod.Text)

            ' RAW 10/07/2003 : remove global parameter forcing scripts to explicitly qualify calls to engine
            m_oMSScriptControl.AddObject("Engine", m_oDLEngine, False)  ' RAW 09/07/2004 : JIT : replaced Me with m_oDLEngine

            m_oMSScriptControl.AddCode(sStr)

            result = True

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
        End Try
        Return result
    End Function

    Private Function CheckForValidUDL(ByVal sTableName As String, ByRef bValid As Boolean) As Long
        Const kMethodName As String = "CheckForValidUDL"
        Dim vResultArray As Object
        Dim oBusiness As Object
        Try

            CheckForValidUDL = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = g_oObjectManager.GetInstance(
                oObject:=oBusiness,
                sClassName:="bGISListMaint.Business",
                vInstanceManager:="ClientManager")

            ' Get the risk details for the risk pertaining to a policy
            m_lReturn = oBusiness.GetGISUDLDetail(sTableName, vResultArray)

            ' Check for errors.
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                CheckForValidUDL = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If IsArray(vResultArray) Then
                bValid = True
            End If
            Exit Function
        Catch ex As Exception

            CheckForValidUDL = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=CheckForValidUDL, excep:=ex)

        Finally
            oBusiness = Nothing


        End Try
        Exit Function
    End Function

    Private Function GetInsuranceFileDetails(
                            ByVal v_lInsuranceFileCnt As Long,
                            ByRef r_vResults As Object) As Long

        Const kMethodName As String = "GetInsuranceFileDetails"
        Dim oBusiness As Object
        Try



            GetInsuranceFileDetails = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = g_oObjectManager.GetInstance(
                oObject:=oBusiness,
                sClassName:="bGISListMaint.Business",
                vInstanceManager:="ClientManager")

            ' Get the risk details for the risk pertaining to a policy
            m_lReturn = oBusiness.GetInsuranceFileDetails(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_vResults:=r_vResults)

            ' Check for errors.
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                GetInsuranceFileDetails = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

        Catch ex As Exception
            GetInsuranceFileDetails = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetInsuranceFileDetails, excep:=ex)
        Finally
            oBusiness = Nothing
        End Try
    End Function
    Private Function GetClaimDetails(ByVal v_lClaimId As Long,
                                    ByRef r_vResultArray(,) As Object) As Long

        Const kMethodName As String = "GetClaimDetails"
        Dim oBusiness As Object
        Try

            GetClaimDetails = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = g_oObjectManager.GetInstance(
                oObject:=oBusiness,
                sClassName:="bCLMChangeClaimStatus.Business",
                vInstanceManager:="ClientManager")

            ' Get the risk details for the risk pertaining to a policy
            m_lReturn = oBusiness.GetClaimDetails(v_lClaimId:=v_lClaimId, r_vResultArray:=r_vResultArray)

            ' Check for errors.
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                GetClaimDetails = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

        Catch ex As Exception

            GetClaimDetails = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetClaimDetails, excep:=ex)
        Finally
            oBusiness = Nothing
        End Try

    End Function
    Private Sub TabStrip1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabStrip1.SelectedIndexChanged
        If m_iLastTabIndexForTabStripClicked <> -1 AndAlso m_iLastTabIndexForTabStripClicked <> TabStrip1.SelectedIndex Then
            TabStrip1_Click(Nothing, Nothing)

        End If
    End Sub

    Private Sub LinkedOrUnlinkedCase() Handles uctCLMCaseClaim1.LinkedOrUnlinked
        uctCLMCaseHeader1.CaseNumber = uctCLMCaseClaim1.CaseNumber
    End Sub
    Private Sub frmInterface_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        For Each tp As TabPage In TabStrip1.TabPages
            If tp.Text.Contains(Chr(e.KeyCode)) And e.Alt Then
                TabStrip1.SelectedTab = tp
                Exit For
            End If
        Next
    End Sub

    Private Sub TabStrip1_KeyDown(sender As Object, e As KeyEventArgs) Handles TabStrip1.KeyDown
        For Each tp As TabPage In TabStrip1.TabPages
            If tp.Text.Contains(Chr(e.KeyCode)) And e.Alt Then
                TabStrip1.SelectedTab = tp
                Exit For
            End If
        Next
    End Sub

End Class

